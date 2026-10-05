import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, of, BehaviorSubject } from "rxjs";
import { APIConfiguration } from "../../models/app.constants";
import { DataService } from "./dataservice.service";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class PersistanceService {
  public userLoggedIn = new BehaviorSubject<boolean>(false);
  public dueAlert = new BehaviorSubject<number>(0);

  screenNames: any;

  constructor(private router: Router, private dataservice: DataService, private config: APIConfiguration, private ngbModal: NgbModal) { }

  isUserLoggedIn(): boolean {

    let loggedIn;
    this.userLoggedIn.subscribe(res => loggedIn = res);
    return loggedIn;
  }

  set(key: string, data: any): void {
    try {
      localStorage.setItem(key, JSON.stringify(data));
    } catch (e) {
      console.error("Error saving to localStorage", e);
    }
  }

  get(key: string) {
    try {
      let data = JSON.parse(localStorage.getItem(key));
      if (data != null)
        return data;
      else {
        this.clear();
      }

    } catch (e) {
      console.error("Error getting data from localStorage", e);
      return null;
    }
  }

  // isAuthenticated() {
  //   let screenNames: any;
  //   screenNames = this.sharedService.screenPermissionsSource.asObservable();
  //   debugger;
  //   if (localStorage.getItem('userToken') != null && localStorage.getItem('screenNames') != null) {
  //     this.IsAuthenticated = true;
  //     return this.loggedIn.asObservable();
  //   }
  //   this.IsAuthenticated = false;
  //   return this.loggedIn.next(false);
  // }
  // redirectToLogin() {
  //   this.isUserLoggedIn.next(false);
  //   this.router.navigateByUrl("/login");
  // }
  clear() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('userid');
    localStorage.removeItem('roleid');
    localStorage.removeItem('userRole');
    localStorage.removeItem('screenNames');
    localStorage.removeItem('displayname');
    localStorage.removeItem('screen');
    localStorage.clear();
    this.userLoggedIn.next(false);
    this.router.navigateByUrl("/login");
  }
  getIP(): Observable<any> {
    return this.dataservice.getIP<any>(this.config.ipAddressUrl);
  }
  // insertToken(userId: any, userToken: any): Observable<any> {
  //   let userObj = {
  //     User_Id: userId,
  //     User_Token: userToken
  //   };
  //   let formData: FormData = new FormData();
  //   return this.dataservice.postFormData(this.config.Emar_User_InsertUserToken, userObj, formData);
  // }
  userAuthentication(userName, password, isloggedin, userIp, browser) {
    var data = "username=" + userName + "&password=" + password + "&grant_type=password" + "&loggedin=" + isloggedin + "&ip=" + userIp + "&browser=" + browser;
    return this.dataservice.postLogin(this.config.serverWithApiUrl, data);
  }
  getScreenPermissions(roleId: number): Observable<any[]> {
    let userId = this.get(this.config.loggedInUserKey);
    return this.dataservice.get<any[]>(this.config.Emar_RoleMaster_GetScreenPermissions + userId + "/" + roleId);
  }
  // tslint:disable-next-line:variable-name
  InsertUserREC(NurseStation_Id: string): Observable<any[]> {
    debugger;
    let userId = this.get(this.config.loggedInUserKey);
    return this.dataservice.get<any[]>(this.config.Emar_Insert_Update_User_Recent_FacNs + userId + "/" + NurseStation_Id);
  }
  getPermissionsByScreen(screenName: string) {
    let screens;
    let pageConfig = {};
    screens = this.get("screenNames");
    for (let index = 0; index < screens.length; index++) {
      if (screens[index]["name"] == screenName) {
        pageConfig = screens[index]["values"];
        return pageConfig;
      }
    }
  }
  getEmailByUserName(userName: string): Observable<any> {
    return this.dataservice.getNoAuth<any>(this.config.Emar_Role_GetUserEmailByUserName + userName);
  }
  sendForgotPassword(userName: string, userId: number): Observable<any> {
    return this.dataservice.getNoAuth<any>(this.config.Emar_Role_SendForgotPasswordMail + userName + "/" + userId);
  }
  getRolesByUser(userName: string, password: string): Observable<any> {
    return this.dataservice.getNoAuth<any>(this.config.Emar_Role_GetRolesByUser + userName + "/" + password);
  }
  logoutUser() {
    if (this.ngbModal.hasOpenModals() == true)
      this.ngbModal.dismissAll();
    this.dataservice.get<any>(this.config.Emar_UserMaster_LogoutUser + this.get(this.config.loggedInUserKey)).subscribe(
      res => {
        this.clear();
      }
    );
  }
  getDueMARAlert() {
    this.dataservice.get<any>(this.config.Emar_GetDueMARAlertData + this.get(this.config.loggedInUserKey))
      .subscribe(res => {
        this.dueAlert.next(res);
      });
  }
  redirectToHomePage() {
    this.router.navigate(['/home/']);
  }
}