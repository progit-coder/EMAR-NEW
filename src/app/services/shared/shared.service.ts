import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Company } from '../../models/company.model';
import { DataService } from './dataservice.service';
import { APIConfiguration } from '../../models/app.constants';
import { UserActivityDetailEntity } from '../../models/useractivity.model';
import { CustomdatePipe } from '../../services/shared/customdate.pipe';
@Injectable()
export class SharedService {

  itemsList: any[];
  companies: Company[]
  private companiesList = new BehaviorSubject(this.companies);
  currentCompany = this.companiesList.asObservable();
  private patientIdSource = new BehaviorSubject(0);
  currentPatientId = this.patientIdSource.asObservable();
  public viewdata: any[];
  public screenPermissionsSource = new BehaviorSubject([]);
  //screenNames = this.screenPermissionsSource.asObservable();
  userActivityObj: UserActivityDetailEntity;
  public mailBoxId = new BehaviorSubject<number>(0);
  public mailStatus = new BehaviorSubject<string>("");
  public ToUser = new BehaviorSubject<number>(0);
  public typeId = new BehaviorSubject<number>(0);
  public mailsCount = new BehaviorSubject<number>(0);
  public alertsList = new BehaviorSubject([]);
  public pastDueAlertFlag = new BehaviorSubject<number>(0);

  private orderIdSource = new BehaviorSubject<number>(0);
  currentOrderId = this.orderIdSource.asObservable();

  private quantityIdSource = new BehaviorSubject<number>(0);
  currentQuantityId = this.quantityIdSource.asObservable();
  public saveChangesFlag = new BehaviorSubject<number>(0);
  public pastDueDoses = new BehaviorSubject([]);
  //public patientId =new BehaviorSubject<number>(0);
  public mail = new BehaviorSubject<any>({});
  private facilityIdSource = new BehaviorSubject(0);
  currentFacilityId = this.facilityIdSource.asObservable();
  public saveChangesRenewalFlag = new BehaviorSubject<number>(0);
  //preg checkboxes
  // public pregCheck = new BehaviorSubject<boolean>(false);
  // public feedCheck = new BehaviorSubject<boolean>(false);
  // weightSubject: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  // updateUsers(users: any[]) {
  //   this.weightSubject.next(users);
  // }
  // setPregCheckState(state: boolean) {
  //   this.pregCheck.next(state);
  // }
  // setFeedCheckState(state: boolean) {
  //   this.feedCheck.next(state);
  // }

  constructor(private dataservice: DataService, private config: APIConfiguration, private dateFormatPipe: CustomdatePipe) {
  }
  ngOnInit() {

  }
  changeCompany(companies) {
    this.companiesList.next(companies)
  }

  changePatientId(patientId: number) {
    this.patientIdSource.next(patientId);
  }
  inboxMail(mailBoxId: number) {
    this.mailBoxId.next(mailBoxId);
  }
  MailStatus(mailStatus: string) {
    this.mailStatus.next(mailStatus);
  }
  mailUser(userId: number) {
    this.ToUser.next(userId);
  }
  changeOrderId(orderId: number) {
    this.orderIdSource.next(orderId);
  }
  changeQuantityId(quantityId: number) {
    this.quantityIdSource.next(quantityId);
  }
  saveChangesOrderInfo(saveChangesFlag: number) {
    this.saveChangesFlag.next(saveChangesFlag);
  }
  saveChangesForRenewal(saveChangesFlag: number)
{
  this.saveChangesRenewalFlag.next(saveChangesFlag);
}
  alertTypeId(typeId: number) {
    this.typeId.next(typeId);
  }
  changeMailCounts(id: number) {
    this.mailsCount.next(id);
  }
  alerts(data) {
    this.alertsList.next(data);
  }
  pastDueFlag(flag: number) {
    this.pastDueAlertFlag.next(flag);
  }
  updateScreenNames(data) {
    this.screenPermissionsSource.next(data);
  }
  updateDosesList(list: any) {
    this.pastDueDoses.next(list);
  }
  insertUserSession(userId: number, systemIp: string, browser: string): Observable<any[]> {
    return this.dataservice.get<any>(this.config.Emar_UserActivity_InsertUserSession + userId + "/" + systemIp + "/" + browser);
  }
  insertUserActivityDetails(screenId: number, activityId: number, comments: string) {
    this.userActivityObj = {
      UserActivity_Id: 0,
      Session_Id: 0,
      Screen_Id: screenId,
      Time: this.dateFormatPipe.dateWithTime(new Date()),
      Activity_Id: activityId,
      Comments: comments
    };
    return this.dataservice.post(this.config.Emar_UserActivity_InsertUserActivityDetails, this.userActivityObj);
  }
  replyMail(mail: any) {
    this.mail.next(mail);
  }
  getUserRecentFacNs(userId: number): Observable<any> {
    return this.dataservice.get<any>(this.config.Emar_Common_GetUserRecentFacNs + userId);
  }
  changeFacilityId(facilityId: number) {
    this.facilityIdSource.next(facilityId);
  }
}
