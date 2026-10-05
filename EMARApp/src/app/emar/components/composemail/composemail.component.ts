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
import { ComposeMail } from '../../../models/mailbox.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { UserDrop } from '../../../models/user.model';
import { FavouriteMail } from '../../../models/mailbox.model';
@Component({
  selector: 'app-composemail',
  templateUrl: './composemail.component.html',
  styleUrls: ['./composemail.component.css']
})
export class ComposemailComponent implements OnInit {
  myform: FormGroup;
  public template;
  public selectedMailItem = [];
  dropdownSettings_Mail: any = {};
  ShowFilter = true;
  public userId: number;
  public composeMailObj: ComposeMail;
  public inboxflag: boolean = false;
  mailboxdetails: any[];
  mailStatus: string = "";
  public userDrop: any[];
  public CheckAll: boolean = false;
  public errorMessage: string;
  mailbox: any[];
  checkform: FormGroup;
  public favouriteMailObj: FavouriteMail;
  public searchText: string = "";
  public replyMail: any;
  public replyButton: boolean;
  public MailBoxId: number;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  public selectedCCMailItem=[];
  public mailData:any;
  constructor(private route: Router, private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private sharedService: SharedService,
    private alertService: AlertService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService, ) {
  }
  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.sharedService.mail.subscribe(res => this.replyMail = res);
    this.myform = new FormGroup({
      tomail: new FormControl(''),
      ccmail: new FormControl(''),
      subject: new FormControl(''),
      mailbody: new FormControl(''),
    });
    this.checkform = new FormGroup({
      check: new FormControl('')
    });
    this.myform.reset();
    this.myform.patchValue({
      tomail: '',
      ccmail: '',
    });
    this.dropdownSettings_Mail = {
      singleSelection: false,
      idField: "User_Id",
      textField: "User_DisplayName",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter,
      limitSelection: 10,
    };
    this.getUsers();
    this.getMailData();
    //this.replyTo(this.replyMail);
  }
  mainMailBox() {
    this.route.navigate(['/home/mailbox']);
    this.myform.reset();
  }
  insertDraftimail() {
    if ((this.myform.value.tomail == null || this.myform.value.tomail == undefined || this.myform.value.tomail == "") && (this.myform.value.ccmail == null || this.myform.value.ccmail == undefined || this.myform.value.ccmail == "") && (this.myform.value.subject == null || this.myform.value.subject == undefined || this.myform.value.subject == "") && (this.myform.value.mailbody == null || this.myform.value.mailbody == undefined || this.myform.value.mailbody == "")) {
      this.alertService.warn("Enter data to save in draft");
    }
    else {
      this.ng4LoadingSpinnerService.show();
      let toUsers=[];
      let ccUsers=[];
      if(this.myform.value.tomail!=undefined && this.myform.value.tomail!=null && this.myform.value.tomail.length!=0)
      {
        let userIds=this.myform.value.tomail.forEach(element => {
          toUsers.push(element.User_Id)
        });
      }
      if(this.myform.value.ccmail!=undefined && this.myform.value.ccmail!=null && this.myform.value.ccmail.length!=0)
      {
        let userIds=this.myform.value.ccmail.forEach(element => {
          ccUsers.push(element.User_Id)
        });
      }
      this.composeMailObj = {
        MailBox_Id: this.MailBoxId,
        Fromuser_Id: this.persistanceService.get(this.config.loggedInUserKey),
        Touser_Id:toUsers.length==0?null:toUsers.join(","),
        CcUser_Id:ccUsers.length==0?null:ccUsers.join(","),
        ToMalId: "test",
        Subject: this.myform.value.subject,
        MailBody: this.myform.value.mailbody,
        AttachmentPath: "test",
        MailBox_Status: 0,
        MailBox_Date: this.dateFormatPipe.dateWithTime(new Date()),
        ParentId: 1,
      };
      this.dataservice.post(this.config.Emar_InsertComposeMail, this.composeMailObj)
        .subscribe(res => {
          this.route.navigate(['/home/mailbox']);
          this.MailBoxId = 0;
          this.myform.reset();
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  mailBody(emailBoxId: number,userId:number) {
    if(this.mailStatus!="" && this.mailStatus!="Sent" && this.mailStatus!="Draft")
    {
    this.insertMailRead(emailBoxId);
    }
    
    this.sharedService.inboxMail(emailBoxId);
    this.sharedService.mailUser(userId);
    this.route.navigate(['/home/readmail']);
    this.myform.reset();
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
  insertComposemail() {
    debugger;
    if (((this.myform.value.tomail.length == 0 || this.myform.value.tomail.length == undefined || this.myform.value.tomail.length == 0) && (this.myform.value.ccmail == null || this.myform.value.ccmail == undefined || this.myform.value.ccmail == ""))) {
      this.alertService.warn("Please enter To or CC to send")
    }

    // else if (((this.myform.value.tomail.length > 10 && this.myform.value.ccmail.length > 10))) {
    //   this.alertService.warn("Please select max 10 To and CC only  to send")
    // }

    else if (((this.myform.value.tomail.length > 10 ))) {
      this.alertService.warn("Please select max 10 To only  to send")
    }

    else if (((this.myform.value.ccmail.length > 10 ))) {
      this.alertService.warn("Please select max 10 CC only  to send")
    }
    
    else {
      this.ng4LoadingSpinnerService.show();
      let toUsers=[];
      let ccUsers=[];
      if(this.myform.value.tomail!=undefined && this.myform.value.tomail!=null && this.myform.value.tomail.length!=0)
      {
        let userIds=this.myform.value.tomail.forEach(element => {
          toUsers.push(element.User_Id)
        });
      }
      if(this.myform.value.ccmail!=undefined && this.myform.value.ccmail!=null && this.myform.value.ccmail.length!=0)
      {
        let userIds=this.myform.value.ccmail.forEach(element => {
          ccUsers.push(element.User_Id)
        });
      }
      this.composeMailObj = {
        MailBox_Id: this.MailBoxId,
        Fromuser_Id: this.persistanceService.get(this.config.loggedInUserKey),
        Touser_Id:toUsers.length==0?null:toUsers.join(","),
        CcUser_Id:ccUsers.length==0?null:ccUsers.join(","),
        ToMalId: "test",
        Subject: this.myform.value.subject,
        MailBody: this.myform.value.mailbody,
        AttachmentPath: "test",
        MailBox_Status: 1,
        MailBox_Date: this.dateFormatPipe.dateWithTime(new Date()),
        ParentId: 1,
      };
      this.dataservice.post(this.config.Emar_InsertComposeMail, this.composeMailObj)
        .subscribe(res => {
          this.route.navigate(['/home/mailbox']);
          this.MailBoxId = 0;
          this.myform.reset();
          this.getMailData();
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  getMailboxDetails(userId: number): any {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll = false;
    this.selectedRecords = [];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Inbox")
      .subscribe(res => {
        this.inboxflag = true;
        this.mailboxdetails = res;
        this.mailStatus = "Inbox";
        this.myform.reset();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getUsers() {
    this.dataservice.get<any[]>(this.config.Emar_MailboxGetToUsers + this.userId)
      .subscribe(res => {
        this.userDrop = res;
        this.replyTo(this.replyMail);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getsentMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll = false;
    this.selectedRecords = [];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Sent")
      .subscribe(res => {
        this.inboxflag = true;
        this.mailboxdetails = res;
        this.mailStatus = "Sent";
        this.myform.reset();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getdraftsMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll = false;
    this.selectedRecords = [];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Drafts")
      .subscribe(res => {
        this.inboxflag = true;
        this.mailboxdetails = res;
        this.mailStatus = "Drafts";
        this.myform.reset();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getimportantMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll = false;
    this.selectedRecords = [];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Fav")
      .subscribe(res => {
        this.inboxflag = true;
        this.mailboxdetails = res;
        this.mailStatus = "Favourite";
        this.myform.reset();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getTrashMailDetails() {
    this.ng4LoadingSpinnerService.show();
    this.CheckAll = false;
    this.selectedRecords = [];
    this.dataservice.get<any[]>(this.config.Mailbox_GetMailboxDetails + this.userId + "/" + "Trash")
      .subscribe(res => {
        this.mailStatus = "Trash";
        this.mailboxdetails = res;
        this.myform.reset();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  selectedRecords: any[] = [];
  onCheckAll(event) {

    if (event == true) {
      this.checkform.patchValue({
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
      this.checkform.patchValue({
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
        this.myform.reset();
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
  trashRecord() {
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select record to move to trash");
    }
    else if (this.selectedRecords.length != 0) {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.post(this.config.Emar_Mailbox_TrashMailRecord, this.selectedRecords)
        .subscribe(res => {
          if (res = 1)
            this.checkform.reset();
          this.selectedRecords = [];
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
  replyTo(obj: any) {
    debugger
    this.MailBoxId = obj.MailBoxId;
    this.selectedMailItem=[];
    if (obj.Type == 0) {
    let userExist=this.userDrop.find(u=>u.User_Id==obj.FromUserId);
    if(userExist)
    {
      this.selectedMailItem.push(this.userDrop.filter(e => e.User_Id === parseInt(obj.FromUserId))[0]);
    }
      this.myform.patchValue({
        tomail: this.selectedMailItem,
        subject: obj.Subject,
        mailbody: obj.MailBody,
      })
    }
    if (obj.Type == 1) {
      this.myform.patchValue({
        subject: obj.Subject,
        mailbody: obj.MailBody,
      })
    }
    if (obj.Type == 2) {
      this.myform.patchValue({
        tomail: obj.FromUserId,
        ccmail: obj.CcUserId,
        subject: obj.Subject,
        mailbody: obj.MailBody,
      })
    }
    if (obj.Type == 3) {
      let toUsers=obj.ToUserName !=null ?obj.ToUserName.split(","):[];
      let ccUsers=obj.CcUserName!=null ?obj.CcUserName.split(","):[];
      this.selectedMailItem=[];
      if(toUsers.length>0)
      {
      toUsers.forEach(element => {
        let userExist=this.userDrop.find(u=>u.User_Id===parseInt(element));
        if(userExist)
        {
        this.selectedMailItem.push(this.userDrop.filter(e => e.User_Id === parseInt(element))[0]);
        }
      });
    }
      // let userExist=this.userDrop.find(u=>u.User_Id===obj.ToUserId);
      // if(userExist)
      // {
      //   this.selectedMailItem.push(this.userDrop.filter(e => e.User_Id === parseInt(obj.ToUserId)));
      // }
      this.selectedCCMailItem=[];
      if(ccUsers.length>0)
      {
        ccUsers.forEach(element => {
          let userExist=this.userDrop.find(u=>u.User_Id===parseInt(element));
          if(userExist)
          {
          this.selectedCCMailItem.push(this.userDrop.filter(e => e.User_Id === parseInt(element))[0]);
          }
        });
      }
        this.myform.patchValue({
        tomail: this.selectedMailItem,
        ccmail: this.selectedCCMailItem,
        subject: obj.Subject,
        mailbody: obj.MailBody,
      })
    }
  }
  forwardMail() {
    this.myform.reset();
    if (this.selectedRecords.length == 0) {
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

          this.inboxflag = false;
          this.myform.reset();
          this.mailbox = res;
          this.mailbox["Type"] = 1;
          this.replyTo(this.mailbox);
          this.selectedRecords = [];
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  replyToMail() {
    this.myform.reset();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select record to reply");
    }
    else if (this.selectedRecords.length > 1) {
      this.alertService.warn("Please select one record to reply");
      this.selectedRecords = [];
    }
    else if (this.selectedRecords.length == 1) {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any>(this.config.Mailbox_GetMailboxDetailsByMailboxId + this.selectedRecords[0].MailBoxId+"/"+this.selectedRecords[0].ToUserId)
        .subscribe(res => {
          this.inboxflag = false;
          this.myform.reset();
          this.mailbox = res;
          this.mailbox["Type"] = 0;
          this.replyTo(this.mailbox);
          this.selectedRecords = [];
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
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
