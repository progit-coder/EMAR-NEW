import { Component, OnInit, Input, Output,EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { APIConfiguration } from '../../models/app.constants';
import { DataService } from '../../services/shared/dataservice.service';
import { AlertService } from '../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
//import { EventEmitter } from 'events';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css'],
  providers: [DataService, APIConfiguration]
})
export class ResetpasswordComponent implements OnInit {
public verifyFlag=0;
public template;
public myform:FormGroup;
public Otp:string;
public verifyObj:CheckUser;
resetForm:FormGroup;
public message:string;
public mismatch:string;
public OtpFlag:number=1;
@Input()
public userId:number;
@Input()
public typeFlag:number;
@Output()
   Note=new EventEmitter<number>();
// @Output()
// Message=new EventEmitter<string>();
  constructor(private config: APIConfiguration,private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,private dataservice: DataService,private alertService: AlertService,) { }

  ngOnInit() {
    this.template = this.dataservice.template;
   // this.ng4LoadingSpinnerService.hide();
    this.resetForm=new FormGroup({
      resetPass: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password),Validators.minLength(4)]),
      confirmPass: new FormControl('', [Validators.required]),
    });
  }
  
verifyOTP()
{
  this.ng4LoadingSpinnerService.show();
  this.verifyObj={
    OTP:this.Otp,
    UserId:this.userId,
   // date:this.myform.value.date
  }
if(this.typeFlag==0){

  this.dataservice.getNoAuth<any>(this.config.Emar_Role_GetUserOTPCheckStatus+this.userId+"/"+this.Otp)
  .subscribe(res => 
    {
      this.ng4LoadingSpinnerService.hide();
    this.message=res;
    if(res=="Valid")
    {
    this.verifyFlag=1;
    this.Note.emit(this.OtpFlag);
    }
    else
    this.verifyFlag=0;
    //this.Message.emit(this.message);
    },
      error => {
        this.ng4LoadingSpinnerService.hide();
         this.alertService.error(error.message);
        
      }); 
    }
    else if(this.typeFlag==1)
    {
      this.dataservice.getNoAuth<any>(this.config.Emar_Role_GetLockOTPCheckStatus+this.userId+"/"+this.Otp)
  .subscribe(res => 
    {
      this.ng4LoadingSpinnerService.hide();
    this.message=res;
    if(res=="User Unlocked Successfully")
    this.OtpFlag=0;
    this.Note.emit(this.OtpFlag)
    //this.Message.emit(this.message);
    },
      error => {
        this.ng4LoadingSpinnerService.hide();
         this.alertService.error(error.message);
        
      }); 
    }

 
}
cancel()
{
  this.verifyFlag=0;
  this.Otp="";
}
resetPWD()
{
  this.ng4LoadingSpinnerService.show();
  if(this.resetForm.value.resetPass==this.resetForm.value.confirmPass)
  {
  this.dataservice.getNoAuth(this.config. Emar_Role_ResetPassword+this.userId+"/"+this.resetForm.value.resetPass)
  .subscribe(res=>{
    if(res==1)
    {
      this.ng4LoadingSpinnerService.hide();
     this.message="Reset password is successful";
      this.resetScreen();
      this.OtpFlag=0;
      this.Note.emit(this.OtpFlag)
    }
    else
    {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.error("Something went wrong.Please try again");
    }
   },
   error => {
    this.ng4LoadingSpinnerService.hide();
     this.alertService.error(error.message);
   });
}
else{
  this.mismatch="Change password and confirm password should be same.";
  this.ng4LoadingSpinnerService.hide();
  this.mismatch="Change password and confirm password should be same.";
  
}
}

resetScreen() {
  this.resetForm.reset();
  this.verifyFlag=0;
  this.Otp="";
}
}
export class CheckUser
{
  UserId:number;
  OTP :string;
 // date:string;
}
