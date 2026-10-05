import { Component, OnInit } from '@angular/core';
import { AlertService } from '../../../_services/index';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Router } from '@angular/router';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FavouriteMail } from '../../../models/mailbox.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
@Component({
  selector: 'app-mailindox',
  templateUrl: './mailindox.component.html',
  styleUrls: ['./mailindox.component.css']
})
export class MailindoxComponent implements OnInit {
  public template;
  errorMessage: string;
  public userId: number;
  mailboxdetails: any[] = [];
  mailbox: any = [];
  favourites: any[];
  dataRefresher: any;
  myform: FormGroup;
  searchText: string = "";
  gridPagination = this.config.gridPagination;
  p: number = 1;
  CheckAll: boolean = false;
  mailStatus: string = "Inbox";
  public favouriteMailObj: FavouriteMail;
  pageConfig = {};
  public mailData:number=0;
  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private config: APIConfiguration, private route: Router, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService) { }

  ngOnInit() {
    debugger
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Mailbox");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.mailStatus = "Inbox";
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.getMailboxDetails(this.userId);
    this.userActivity();
    this.getMailData();
    this.myform = new FormGroup({
      check: new FormControl('')
    })
    this.mailbox["Type"] = 3;
    this.sharedService.replyMail(this.mailbox);
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  selectedRecords: any[] = [];
  onCheckAll(event) {

    if (event == true) {
      this.myform.patchValue({
        check: 1,
      })
      this.CheckAll = true;
      if (this.mailStatus == "Inbox")
        this.selectedRecords = this.mailboxdetails;
      else if (this.mailStatus == "Sent")
        this.selectedRecords = this.mailboxdetails;
      else if (this.mailStatus == "Drafts")
        this.selectedRecords = this.mailboxdetails;
      else if (this.mailStatus == "Favourite")
        this.selectedRecords = this.mailboxdetails;
      else if (this.mailStatus == "Trash")
        this.selectedRecords = this.mailboxdetails;
    }
    else if (event == false) {
      this.CheckAll = false;
      this.myform.patchValue({
        check: 0,
      })
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    debugger
    if (event == true) {
      item.InboxFlag =this.mailStatus;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.MailBoxId == item.MailBoxId && i.ToUserId==item.ToUserId);
      this.selectedRecords.splice(index, 1);
    }
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Mailbox, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  trashRecord() {
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select record to move to trash");
    }
    else if (this.selectedRecords.length != 0)
    {
      this.ng4LoadingSpinnerService.show();
      
      this.dataservice.post(this.config.Emar_Mailbox_TrashMailRecord, this.selectedRecords)
        .subscribe(res => {
          if (res = 1)
            this.myform.reset();
          this.selectedRecords = [];
          if(this.mailStatus=="Inbox")
          this.getMailboxDetails(this.userId);
          else if(this.mailStatus=="Sent")
          this.getsentMailDetails();
          else if(this.mailStatus=="Drafts")
          this.getdraftsMailDetails();
          else if(this.mailStatus=="Favourite")
          this.getimportantMailDetails();
          else
          this.getTrashMailDetails();
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
  }
  mailBody(emailBoxId: number,userId:any) {
    if(this.mailStatus!="" && this.mailStatus!="Sent" && this.mailStatus!="Draft")
        {
        this.insertMailRead(emailBoxId);
        }
        
      this.sharedService.inboxMail(emailBoxId);
      this.sharedService.MailStatus(this.mailStatus);
      this.sharedService.mailUser(userId);
      this.route.navigate(['/home/readmail']);
  }
  insertMailRead(mailBoxId: number)
  {
    this.dataservice.get<any>(this.config.Emar_Mailbox_InsertReadMail+ mailBoxId+"/"+this.userId)
    .subscribe(res => {
      this.getMailData();
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
  getMailboxDetails(userId: number): any {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll=false;
    this.selectedRecords=[];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Inbox")
      .subscribe(res => {
        this.mailStatus = "Inbox";
        this.mailboxdetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getsentMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll=false;
    this.selectedRecords=[];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Sent")
      .subscribe(res => {
        this.mailStatus = "Sent";
        this.mailboxdetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getdraftsMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll=false;
    this.selectedRecords=[];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Drafts")
      .subscribe(res => {
        this.mailStatus = "Drafts";
        this.mailboxdetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getimportantMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll=false;
    this.selectedRecords=[];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Fav")
      .subscribe(res => {
        this.CheckAll=false;
        this.mailStatus = "Favourite";
        this.mailboxdetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getTrashMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll=false;
    this.selectedRecords=[];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Trash")
      .subscribe(res => {
        this.mailStatus = "Trash";
        this.mailboxdetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  favourite(mailBoxId: number, favouriteFlag: number) {
    this.favouriteMailObj = {
      MailFavourite_Id: 0,
      MailFavourite_Status: favouriteFlag,
      MailBox_Id: mailBoxId,
      MailFavourite_date: this.dateFormatPipe.transform(new Date()),
      user_Id: this.userId
    };
    this.dataservice.post(this.config.Mailbox_UpdateFavourites, this.favouriteMailObj)
      .subscribe((res: any) => {
        //this.getMailboxDetails(this.userId);
        if(this.mailStatus=="Favourite")
        this.getimportantMailDetails();
        else
        this.getMailboxDetails(this.userId);
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  forwardMail() {
    if(this.selectedRecords.length==0)
    {
      this.alertService.warn("Please select record to forward");
    }
    else if (this.selectedRecords.length > 1) {
      this.alertService.warn("Please select one record to forward");
    }
    else if (this.selectedRecords.length == 1) {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any>(this.config.Mailbox_GetMailboxDetailsByMailboxId + this.selectedRecords[0].MailBoxId+"/"+this.selectedRecords[0].ToUserId)
        .subscribe(res => {

          this.mailbox = res;
          this.ng4LoadingSpinnerService.hide();
          this.mailbox["Type"] = 1;
          this.sharedService.replyMail(this.mailbox);
          this.route.navigate(['/home/composemail']);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  replyToMail() {
    if(this.selectedRecords.length==0)
    {
      this.alertService.warn("Please select record to reply");
    }
   else if (this.selectedRecords.length > 1) {
      this.alertService.warn("Please select one record to reply");
    }
    else if (this.selectedRecords.length == 1)
    this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any>(this.config.Mailbox_GetMailboxDetailsByMailboxId + this.selectedRecords[0].MailBoxId+"/"+this.selectedRecords[0].ToUserId)
        .subscribe(res => {

          this.mailbox = res;
          this.ng4LoadingSpinnerService.hide();
          this.mailbox["Type"] = 0;
          this.sharedService.replyMail(this.mailbox);
          this.route.navigate(['/home/composemail']);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  Composemail() {
    this.route.navigate(['/home/composemail']);
  }
  getMailData() {
    debugger
    this.dataservice.get<number>(this.config.Emar_Mailbox_GetReadMailsCount + this.persistanceService.get(this.config.loggedInUserKey))
    .subscribe(res => {
            this.mailData = res;
            this.sharedService.changeMailCounts(res);
        }, error => {
            this.alertService.error(error.message);
        });
}
}
