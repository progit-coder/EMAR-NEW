import { Component, OnInit } from '@angular/core';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import { Validators, FormControl, FormGroup } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';;
import { Screens, Activity } from '../../../models/useractivity.model';
import { AlertService } from '../../../_services';
import { Router } from '@angular/router';
import { FavouriteMail } from '../../../models/mailbox.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-readmail',
  templateUrl: './readmail.component.html',
  styleUrls: ['./readmail.component.css']
})
export class ReadmailComponent implements OnInit {

  public mailBoxId: number = 0;
  mailbox: any;
  public userId: number;
  public inboxflag: boolean = false;
  mailboxdetails: any[];
  mailStatus: string = "";
  myform: FormGroup;
  public CheckAll: boolean = false;
  public favouriteMailObj: FavouriteMail;
  errorMessage: string;
  public searchText: string = "";
  public replyButton: boolean = false;
  template: string;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  public mailData:number=0;
  public toUser:number=0;
  constructor(private route: Router, private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private sharedService: SharedService,
    private alertService: AlertService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) {
  }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.ng4LoadingSpinnerService.show();
    this.sharedService.mailBoxId.subscribe(res => this.mailBoxId = res);
    this.sharedService.mailStatus.subscribe(res => this.mailStatus = res);
    this.sharedService.ToUser.subscribe(res => this.toUser = res);
    if(this.mailBoxId!=0 && this.toUser!=0)
    {
    this.mailBody(this.mailBoxId,this.toUser);
    this.getMailData();
    this.myform = new FormGroup({
      check: new FormControl('')
    });
  }
  else
  {
    this.mainMailBox();
  }
  }
  mailBody(mailBoxId: number,userId:number) {
    this.dataservice.get<any>(this.config.Mailbox_GetMailboxDetailsByMailboxId + mailBoxId+"/"+userId)
      .subscribe(res => {
debugger
        this.mailbox = res;
        this.inboxflag=false;
        this.getMailData();
        if(this.mailStatus!="" && this.mailStatus!="Sent" && this.mailStatus!="Draft")
        {
        this.insertMailRead(mailBoxId);
        }
        this.ng4LoadingSpinnerService.hide();
      //   if(this.mailbox.MailBox_Status==0)
      //   {
      // this.mailbox["Type"] = 2;
      // this.sharedService.replyMail(this.mailbox);
      // this.route.navigate(['/home/composemail']);
      //   }
        if (this.mailbox.FromUserId == this.userId)
          this.replyButton = false;
        else
          this.replyButton = true;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
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
  trashRecord() {
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select record to move to trash");
    }
    else if (this.selectedRecords.length != 0) {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.post(this.config.Emar_Mailbox_TrashMailRecord, this.selectedRecords)
        .subscribe(res => {
          if (res = 1)
            this.myform.reset();
            this.ng4LoadingSpinnerService.hide();
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
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  mainMailBox() {
    this.route.navigate(['/home/mailbox']);
  }
  getMailboxDetails(userId: number): any {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll=false;
    this.selectedRecords=[];
    this.mailboxdetails = [];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Inbox")
      .subscribe(res => {
        this.inboxflag = true;

        this.mailboxdetails = res;
         this.mailStatus = "Inbox";
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
        this.inboxflag = true;
        this.mailboxdetails = res;
         this.mailStatus = "Sent";
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
        this.inboxflag = true;

        this.mailboxdetails = res;
         this.mailStatus = "Drafts";
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
        this.inboxflag = true;
        this.mailboxdetails = res;
         this.mailStatus = "Favourite";
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
        this.inboxflag = true;
         this.mailStatus = "Trash";
        this.mailboxdetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  trashReadMail() {
    this.ng4LoadingSpinnerService.show();
    debugger
    this.mailbox.InboxFlag =this.mailStatus;
    this.dataservice.post(this.config.Emar_Mailbox_TrashReadMailRecord, this.mailbox)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1)
        if(this.mailStatus=="Trash" || this.mailStatus=="Draft")
        {
          this.alertService.warn("Deleted");
        }
        else{
          this.alertService.warn("Moved To Trash");
        }
        this.getMailboxDetails(this.userId);
        
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
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
    if (event == true) {
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.MailBoxId == item.MailBoxId && i.ToUserId==item.ToUserId);
      this.selectedRecords.splice(index, 1);
    }
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
  checkedReply()
  {
    if(this.selectedRecords.length==0)
    {
      this.alertService.warn("Please select record to reply");
    }
 else if (this.selectedRecords.length > 1) {
    this.alertService.warn("Please select one record to reply");
    this.selectedRecords = [];
  }
  else if (this.selectedRecords.length == 1)
  this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Mailbox_GetMailboxDetailsByMailboxId + this.selectedRecords[0].MailBoxId+"/"+this.selectedRecords[0].ToUserId)
      .subscribe(res => {
        this.mailbox = res;
        this.ng4LoadingSpinnerService.hide();
        this.mailbox["Type"] = 0;
        this.replyMail();
        this.selectedRecords = [];
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
    checkedForward() {
      if(this.selectedRecords.length==0)
      {
        this.alertService.warn("Please select record to forward");
      }
      else if (this.selectedRecords.length > 1) {
        this.alertService.warn("Please select one record to forward");
        this.selectedRecords = [];
    }
      else if (this.selectedRecords.length == 1) {
        this.ng4LoadingSpinnerService.show();
        this.dataservice.get<any>(this.config.Mailbox_GetMailboxDetailsByMailboxId + this.selectedRecords[0].MailBoxId+"/"+this.selectedRecords[0].ToUserId)
          .subscribe(res => {
            this.mailbox = res;
            this.ng4LoadingSpinnerService.hide();
            this.mailbox["Type"] = 1;
            this.forwardMail();
            this.selectedRecords = [];

          }, error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
      }
    }
  replyMail() {
    if (this.mailbox.MailBox_Status == 1) {
      this.mailbox["Type"] = 0;
      this.sharedService.replyMail(this.mailbox);
      this.route.navigate(['/home/composemail']);
    }
  }
  forwardMail() {
    this.mailbox["Type"] = 1;
    this.sharedService.replyMail(this.mailbox);
    this.route.navigate(['/home/composemail']);
  }
  sendDraftMail()
  {
    this.mailbox["Type"]=3;
    this.sharedService.replyMail(this.mailbox);
    this.route.navigate(['/home/composemail']);
  }
  getMailData() {
    this.dataservice.get<any>(this.config.Emar_Mailbox_GetReadMailsCount + this.persistanceService.get(this.config.loggedInUserKey))
    .subscribe(res => {
            this.mailData = res;
            this.sharedService.changeMailCounts(res);
        }, error => {
            this.alertService.error(error.message);
        });
}
}
