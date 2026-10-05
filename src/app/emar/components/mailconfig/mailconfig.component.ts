import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';

@Component({
  selector: 'app-mailconfig',
  templateUrl: './mailconfig.component.html',
  styleUrls: ['./mailconfig.component.css']
})
export class MailconfigComponent implements OnInit {
public template;
pageConfig: {};
myform: FormGroup;
mailconfig:any[]=[];

constructor(private dataservice: DataService, private alertService: AlertService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

ngOnInit() {
  this.pageConfig = this.persistanceService.getPermissionsByScreen("MailConfig");
  if (this.pageConfig != undefined) {
    if (this.pageConfig["AccessRead"] == 0) {
      this.persistanceService.redirectToHomePage();
    }
    else {
      this.template = this.dataservice.template;
      this.ng4LoadingSpinnerService.show();
      this.myform = new FormGroup({
        serverIp: new FormControl('', [Validators.required, Validators.maxLength(15)]),//, Validators.pattern(this.config.numbersFewSpecialCharacters)
        port: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.pattern(this.config.numeric)]),
        username: new FormControl('', [Validators.required, Validators.maxLength(20)]),//, Validators.pattern(this.config.alphaNumericFewSpecialCharacters2)
        password: new FormControl('', [Validators.required, Validators.maxLength(25), Validators.minLength(4)]),//, Validators.pattern(this.config.password)
      });
      this.userActivity();
      this.getMailConfigDetails();
    }
  }
  else
    this.persistanceService.redirectToHomePage();
}
userActivity() {
  this.sharedService.insertUserActivityDetails(Screens.MailConfig, Activity.View, '')
    .subscribe(res => { }, error => {
      this.alertService.error(error.message);
    });
}
getMailConfigDetails() {
  this.dataservice.get<any>(this.config.Emar_Role_GetMailconfigDetails+1)
    .subscribe(res => { 
      this.mailconfig = res;
      this.ng4LoadingSpinnerService.hide(); 
      if(res!=undefined && res!=null)
      {
      this.myform.patchValue({
        serverIp:res.Host,
        port:res.Port,
        username:res.UserName,
        password:res.Pwd
      });
    }
    else
    {
      this.myform.reset();
    }
    }, error => {
      this.alertService.error(error.message)
    });
}
insertUpdateMailConfig()
{
  let obj=
  {
    ID:0,
    Host: this.myform.value.serverIp,
    Port: this.myform.value.port,
    UserName:this.myform.value.username,
    Pwd:this.myform.value.password,
    Status:1,
    CreatedBy:this.persistanceService.get(this.config.loggedInUserKey),
    Createddate:new Date().toISOString(),
  }
  this.dataservice.post(this.config.Emar_Common_inserUpdateMailconfigDetails, obj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success("Save successful");
          this.getMailConfigDetails();
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
}

}
