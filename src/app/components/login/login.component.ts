import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserModel } from '../../models/user.model';
import { UserRecModel } from '../../models/user.model';
import { PersistanceService } from '../../services/shared/persistance.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SharedService } from '../../services/shared/shared.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { APIConfiguration } from '../../models/app.constants';
import { DefaultScreen } from '../../models/useractivity.model';
import { DataService } from '../../services/shared/dataservice.service';
import { AlertService } from '../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  user: UserModel = new UserModel();
  public template;
  errmsg: string;
  isLoginError: boolean;
  screenNames: any[];
  public UseRecList: UserRecModel = new UserRecModel();
  myform: FormGroup;
  email: string = "";
  userId: string = "";
  type: number = 0;
  userName: string = "";
  public modalHistoryIsOpen: boolean = false;
  userIpAddress: string;
  browserName: string;
  public userLoggedIn = new BehaviorSubject<boolean>(false);
  public forgotPassFlag = 0;
  public clickPassFlag = 0;
  public msg: number = 1;
  public modalNewUserIsOpen: boolean = false;
  changePwdform: FormGroup;
  public message: string = "";
  public mismatch: string;
  public changePwdFlag: number = 0;
  public nurseStationList: any = [];
  public computersList: any = [];
  public display: boolean = true;
  public displayLogin: boolean = true;

  //@ViewChild("ddrole") ddroleEle: ElementRef;
  constructor(private router: Router, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, private sharedService: SharedService, private dataservice: DataService, private alertService: AlertService,
    private config: APIConfiguration, private route: ActivatedRoute,) {
    if (JSON.parse(localStorage.getItem("userToken")) != null) {
      this.persistanceService.clear();
    }
  }

  ngOnInit(): void {

    debugger;

    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.browserName = this.detectBrowser();

    this.sharedService.saveChangesOrderInfo(0);
    this.sharedService.saveChangesForRenewal(0);

    // Create login form FIRST
    this.myform = new FormGroup({

      userName: new FormControl('', [
        Validators.required,
        Validators.maxLength(20)
      ]),

      password: new FormControl('', [
        Validators.required,
        Validators.maxLength(20)
      ]),

      nurseStation: new FormControl('', Validators.required),

      computerName: new FormControl('')
    });


    // Create change password form
    this.changePwdform = new FormGroup({

      resetPass: new FormControl('', [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern(this.config.password),
        Validators.minLength(4)
      ]),

      confirmPass: new FormControl('', [
        Validators.required
      ])
    });


    // Clear existing login
    if (JSON.parse(localStorage.getItem('userToken')) != null) {
      this.persistanceService.clear();
    }


    // Read Single Login parameters
    this.route.queryParams.subscribe(params => {

      const username = params['username'];
      const password = params['password'];
      const Nrstid = params['Nrstid'];
      const ScreenId = params['ScreenId'];
      const KeyID = params['KeyID'];

      console.log('Login parameters:', {
        username: username,
        Nrstid: Nrstid,
        ScreenId: ScreenId,
        KeyID: KeyID
      });


      if (username && password && Nrstid) {

        this.myform.patchValue({
          userName: username,
          password: password,
          nurseStation: Nrstid
        });

        console.log('Form populated');

        // Automatic login
        this.loginClick();
      }
    });


    this.getIP();
  }

  NoteDisplay() {
    this.msg = 0;
  }
  getIP() {
    this.persistanceService.getIP()
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.userIpAddress = res.ip;
      },
        error => {
          //this.errmsg = <any>error.message;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  loginClick() {
    this.displayLogin = false;
    this.errmsg = '';
    this.login(0);
  }
  InsertUpdateUserRecentFacNs(obj) {
    this.persistanceService.InsertUserREC(obj).subscribe(res => {
      return res;
    }

      , error => {
        this.errmsg = <any>error.message;
        this.ng4LoadingSpinnerService.hide();
      });

  }


  login(isLoggedIn: number) {

    this.ng4LoadingSpinnerService.show();
    this.clickPassFlag = 0;
    let username = this.myform.value.userName;
    let password = this.myform.value.password;
    this.persistanceService.userAuthentication(username, password, isLoggedIn, this.userIpAddress, this.browserName).subscribe((data: any) => {

      this.persistanceService.set('userToken', data.access_token);
      this.persistanceService.set('userRole', data.role);
      this.persistanceService.set('userid', data.userid);
      this.persistanceService.set('roleid', data.roleid);
      this.persistanceService.set('displayname', data.displayname);
      this.persistanceService.set('screen', data.screen);

      var NRSId = this.myform.value.nurseStation;

      // this.UseRecList =[{NurseStation_Id:NRSId , User_Id:User_Id ,Facility_Id:0 , RecFac_Id:0 ,companyId:0  }];
      // this.UseRecList = {NurseStation_Id :String( NRSId) , Facility_Id:0 , User_Id : parseInt( User_Id) , RecFac_Id:0 , companyId:0}
      // this.UseRecList.NurseStation_Id = NRSId;
      //  this.UseRecList.Facility_Id = 0;
      // this.UseRecList.User_Id = data.userid;
      // this.UseRecList.RecFac_Id = 0;
      // this.UseRecList.companyId = 0;

      this.loadScreens(data.roleid, data.screen);

      this.InsertUpdateUserRecentFacNs(NRSId);
      if (this.computersList.length > 0 && this.myform.value.computerName != "") {

        let computerName = this.myform.value.computerName;
        this.persistanceService.set('emarProcessKey', computerName);
      }
      else {
        this.persistanceService.set('emarProcessKey', "");
      }
    },
      (error: any) => {
        debugger
        this.displayLogin = true;
        //this.persistanceService.clear();
        localStorage.removeItem('userToken');
        localStorage.removeItem('userid');
        localStorage.removeItem('roleid');
        localStorage.removeItem('userRole');
        localStorage.removeItem('screenNames');
        localStorage.removeItem('displayname');
        localStorage.removeItem('screen');
        this.isLoginError = true;
        // if (error.status === 500)
        //   this.errmsg = "Unable to connect to Service";
        // else 
        if (error.status === 400 || error.status === 500) {

          this.errmsg = error.error.error_description;
          if (this.errmsg == "User already logged in.") {
            // if (confirm("User already logged in. Do you want to continue?")) {
            //   this.errmsg = '';
            //   this.login(1);
            // }
            // else {
            //   this.myform.patchValue({
            //     userName: '',
            //     password: '',
            //   });
            //   this.roleFlag = false;
            //   this.roleList = [];
            //   this.errmsg = '';
            // }
            this.modalHistoryIsOpen = true;
            this.ng4LoadingSpinnerService.hide();
          }
          else if (this.errmsg == "The user locked, to Unlock ") {
            this.clickPassFlag = 1;
          }
          else if (this.errmsg == "New User Change Password") {

            this.modalNewUserIsOpen = true;
          }
          else {
            this.errmsg = "Unable to connect to Service";
          }
        }
        else

          // if(this.modalNewUserIsOpen != true && this.errmsg != "New User Change Password")
          // {


          //   this.errmsg = error.message;
          //  }
          //  else{
          //    this.errmsg = '';
          //  }
          this.errmsg = error.message;
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getUserEmailByUserName() {
    this.persistanceService.getEmailByUserName(this.userName).subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      this.email = res.User_Email;
      this.userId = res.UserId;
    }, error => {
      this.ng4LoadingSpinnerService.hide();
    });
  }
  loadScreens(roleId: number, defaultScreenName: any) {
    this.persistanceService.getScreenPermissions(roleId).subscribe(res => {
      let screenPermissions = res;
      let arScreenConfig = [];
      if (screenPermissions != null) {
        if (screenPermissions.length > 0) {
          if (defaultScreenName != undefined || defaultScreenName != null || defaultScreenName != '' || defaultScreenName != 0) {

            var defaultScreen = +defaultScreenName;
            let check = screenPermissions.find(s => s.Screen_Id == defaultScreen);
            if (check == undefined) {
              this.errmsg = "No permissions given to assigned default screen";
              this.ng4LoadingSpinnerService.hide();
            }
            else if (check != undefined && check.AccessRead == 0) {
              this.errmsg = "No permissions given to assigned default screen";
              this.ng4LoadingSpinnerService.hide();
            }
            else {
              for (let index = 0; index < screenPermissions.length; index++) {
                let obj = {};
                obj["AccessRead"] = screenPermissions[index].AccessRead;
                obj["AccessWrite"] = screenPermissions[index].AccessWrite;
                obj["PrintPdf"] = screenPermissions[index].PrintPdf;
                obj["PrintExcel"] = screenPermissions[index].PrintExcel;
                let sjson = {};
                sjson["name"] = screenPermissions[index].Screen_Desc;
                sjson["values"] = obj;
                arScreenConfig.push(sjson);
              }
              this.sharedService.updateScreenNames(arScreenConfig);
              this.persistanceService.set("screenNames", arScreenConfig);
              this.persistanceService.userLoggedIn.next(true);
              //this.sharedService.insertUserSession(userid, this.userIpAddress, this.browserName).subscribe(res => {
              //this.persistanceService.set('sessionId', res);
              //var dScreenname =  'd'.concat(defaultScreenName);
              var screen = DefaultScreen['d'.concat(defaultScreenName)];
              if (screen == undefined)
                this.router.navigate(['/home/']);
              else
                this.router.navigate([screen]);
              // if (JSON.parse(defaultScreenName) == DefaultScreen) {
              //   this.router.navigate([DefaultScreen]);
              // }
              // //ToDo: Fix This
              // else if (JSON.parse(defaultScreenName) == "ResidentGrid") {
              //   this.router.navigate(['/home/residentgrid']);
              // }
              // else if (JSON.parse(defaultScreenName) == "EMAR") {
              //   this.router.navigate(['/home/administration']);
              // }
              // else {
              //   this.router.navigate(['/home/dashboard']);
              // }
              this.ng4LoadingSpinnerService.hide();
            }
          }
          else {
            this.errmsg = "No Default screen is assigned. Please contact Admin";
            this.persistanceService.logoutUser();
          }
        }
        else {
          this.errmsg = "No screen permissions. Please contact Admin";
          this.persistanceService.logoutUser();
        }
      }
      else {
        this.errmsg = "No screen permissions. Please contact Admin";
        this.persistanceService.logoutUser();
      }
    }, error => {
      this.persistanceService.logoutUser();
      this.errmsg = error.message;
    });
  }
  forgotPassword() {
    this.ng4LoadingSpinnerService.show();
    if (this.userName != '') {
      this.persistanceService.sendForgotPassword(this.userName, 0).subscribe(res => {
        if (res == 1) {
          this.ng4LoadingSpinnerService.hide();
          //alert('OTP Sent to your Registered Email Id');
          this.forgotPassFlag = 1;
        }
      }, error => {
        this.errmsg = "Problem with Mail Configuration. Contact Administrator";
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else {
      this.ng4LoadingSpinnerService.hide();
    }
  }
  userUnlock() {
    this.type = 1;
    this.errmsg = '';
    this.clickPassFlag = 0;
  }
  forgotPasswordFlag() {
    this.type = 0;
  }

  forgotReset() {
    this.modalNewUserIsOpen = false;
    this.userName = "";
    this.email = "";
    this.forgotPassFlag = 0;
    this.errmsg = "";
    this.clickPassFlag = 0;
    this.msg = 1;
    this.changePwdFlag = 0;
    this.myform.reset();
  }

  detectBrowser(): any {
    var ua = navigator.userAgent, tem,
      M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
      tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
      return 'IE ' + (tem[1] || '');
    }
    if (M[1] === 'Chrome') {
      tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
      if (tem != null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
    return M.join(' ');
  }
  continueLogin() {
    this.modalHistoryIsOpen = false;
    this.errmsg = '';
    this.login(1);
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.myform.patchValue({
      userName: '',
      password: '',
    });
    this.errmsg = '';
  }
  resetPWD() {

    if (this.changePwdform.value.resetPass == this.changePwdform.value.confirmPass) {

      let username = this.myform.value.userName;
      let password = this.myform.value.password;
      this.dataservice.getNoAuth(this.config.Emar_Role_UpdateNewUserpassword + username + "/" + password + "/" + this.changePwdform.value.resetPass)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res == 1) {
            this.message = "Change password is successful";
            this.changePwdform.reset();
            this.myform.reset();
            this.changePwdFlag = 1;
          }
          else {
            this.alertService.error("Something went wrong.Please try again");
            this.ng4LoadingSpinnerService.hide();
          }
        },
          error => {

            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else {
      this.mismatch = "Change password and confirm password should be same.";
      this.ng4LoadingSpinnerService.hide();

    }
  }
  GetNurseStationsByUser() {

    this.nurseStationList = [];
    let username = this.myform.value.userName;
    let password = this.myform.value.password;
    if ((username != undefined && username != null && username != "") && (password != undefined && password != null && password != "")) {

      this.ng4LoadingSpinnerService.show();
      this.dataservice.getNoAuth(this.config.Emar_Role_UsersConfigNurseStationDrop + username + "/" + password)
        .subscribe(res => {

          if (res != null) {
            this.errmsg = "";
            this.nurseStationList = res;
            if (this.nurseStationList.length > 0) {
              let defaultNs = res[0].NurseStation_Id;
              this.myform.patchValue({
                nurseStation: defaultNs,
              });
              this.GetComputersListByNsId();
              this.display = true;
              const nusringStationvalidation = this.myform.get('nurseStation');
              nusringStationvalidation.setValidators([Validators.required]);
              nusringStationvalidation.updateValueAndValidity();
            }
            else {
              this.computersList = [];
              this.display = false;
              const nusringStationvalidation = this.myform.get('nurseStation');
              nusringStationvalidation.setValidators(null);
              nusringStationvalidation.clearValidators();
              nusringStationvalidation.updateValueAndValidity();
              const computerNamevalidation = this.myform.get('computerName');
              computerNamevalidation.setValidators(null);
              computerNamevalidation.clearValidators();
              computerNamevalidation.updateValueAndValidity();
            }
            if (this.nurseStationList.length > 0) {
              let checkNewUser = this.nurseStationList[0].NewUserFlag;
              if (checkNewUser == 1) {
                this.modalNewUserIsOpen = true;
              }
            }
          }
          else {
            this.computersList = [];
            this.errmsg = "The user name or password is incorrect";
          }
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  GetComputersListByNsId() {

    this.computersList = [];
    this.myform.patchValue({
      computerName: "",
    });
    let username = this.myform.value.userName;
    let password = this.myform.value.password;
    if (username != "" && password != "" && this.myform.value.nurseStation != "") {
      this.ng4LoadingSpinnerService.show();
      let nurseStation = parseInt(this.myform.value.nurseStation);
      this.dataservice.getNoAuth(this.config.Emar_Role_GetProcessKeyMasterList + nurseStation)
        .subscribe(res => {

          if (res != null) {
            this.computersList = res;
            if (this.computersList.length > 0) {
              // let defaultComputer=res[0].ProcessKey;
              // this.myform.patchValue({
              //   computerName:defaultComputer,
              // });
              if (this.computersList.length == 1) {
                this.myform.patchValue({
                  computerName: this.computersList[0].ProcessKey
                })
              }
              const computerNamevalidation = this.myform.get('computerName');
              computerNamevalidation.setValidators([Validators.required]);
              computerNamevalidation.updateValueAndValidity();
            }
            else {
              const computerNamevalidation = this.myform.get('computerName');
              computerNamevalidation.setValidators(null);
              computerNamevalidation.clearValidators();
              computerNamevalidation.updateValueAndValidity();
            }
          }
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  reloadCurrentPage() {
    window.location.reload();
  }
}