import { Component, OnInit, Input, HostListener, Output, EventEmitter, } from '@angular/core';
import { Router } from '@angular/router';
import { PersistanceService } from '../../services/shared/persistance.service';
import { Alert, AlertType } from '../../_models/index';
import { AlertService } from '../../_services/index';
import { DataService } from '../../services/shared/dataservice.service';
import { APIConfiguration } from '../../models/app.constants';
import { SharedService } from '../../services/shared/shared.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { OrdersdiscardComponent } from '../../emar/components/ordersdiscard/ordersdiscard.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NurseStation } from '../../models/facility.model';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
    myform: FormGroup;
    public loginUserReceFacility: any;
    public loginUserReceNurseStation: any;
    public selectedfaItems = [];
    public selectednItems = [];
    @Output() saveChanges: EventEmitter<any> = new EventEmitter();
    public isUserLoggedIn: boolean = false;
    isUserToken = false;
    public errmsg:string;
    @Input() id: string;
    displayName;
    userRole;
    public facilities: any[];
    public nurseStations: NurseStation[];
    ShowFilter = true;
    dropdownSettings_NurseStations: any = {};
    dropdownSettings_Facilities: any = {};
    alerts: Alert[] = [];
    alertsPop: Alert[] = [];
    public dueAlert: number = 0;
    alertsData: any[] = [];
    public userId: number;
    public forgotPassFlag = 0;
    public type: number = 0;
    public msg: number = 1;
    public otpflag: number = 0;
    iscollapsed = true;
    iscollapsedAlerts = true;
    iscollapsedOutClick =true;
    public modalChangePwd:boolean=false;
    public modalDosesIsOpen: boolean = false;
    public modalOutboundErrorIsOpen: boolean = false;
    public DosesList:any=[];
    public OutBoundErrorList:any;
    public ErrorCount:number =0;
    public valueChangesFlagReceive: number = 0;
    public modalAlertOpen:boolean=false;
    public mailData:number;
    mailPageConfig = {};
    alertsPageConfig={};
    iscollapsedMail: boolean=true;
    nsIds:string='';
    public userPastDueAlertFlag:number=0;
    constructor(private modalService: NgbModal,public sharedService: SharedService, private router: Router, private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService, private alertService: AlertService) {
        if (this.persistanceService.isUserLoggedIn()) {
            this.isUserLoggedIn = true;
        }
        else
            this.isUserLoggedIn = false;
        if (JSON.parse(localStorage.getItem("userToken")) != null) {
            this.isUserToken = true;
         }
        this.sharedService.mailsCount.subscribe(res => this.mailData = res);
        this.sharedService.alertsList.subscribe(res => this.alertsData = res);
        this.sharedService.pastDueAlertFlag.subscribe(res => this.userPastDueAlertFlag = res);
        this.sharedService.pastDueDoses.subscribe(res=>this.DosesList=res);
        this.dueAlert=this.DosesList.length;
    }

    ngOnInit() {
        this.getAlertsData();
        this.getMailData();
        this.getPastDueAlertFlag();
        this.sharedService.mailsCount.subscribe(res => this.mailData = res);
        this.sharedService.alertsList.subscribe(res => this.alertsData = res);
        this.mailPageConfig = this.persistanceService.getPermissionsByScreen("Mailbox");
        this.alertsPageConfig = this.persistanceService.getPermissionsByScreen("Alerts");
        this.userRole = this.persistanceService.get('userRole');
        this.sharedService.saveChangesFlag.subscribe(res => this.valueChangesFlagReceive = res);
        if (this.userRole != null)
            this.userRole = this.userRole.substring(1, this.userRole.length - 1);
        // if (this.persistanceService.get(this.config.loggedInUserRoleKey) != 1) {
            this.getDueMARAlert();
            this.getOutboundErrorDetails();
            this.persistanceService.dueAlert.subscribe(res => this.dueAlert = res);
            
        // }
        this.myform = new FormGroup({
            ddlfacilities: new FormControl(''),
            ddlnursestations: new FormControl(''),
          });
          this.dropdownSettings_Facilities = {
            singleSelection: true,
            idField: "Facility_Id",
            textField: "Facility_Name",
            text: "Facilities",
            itemsShowLimit: 1,
            closeDropDownOnSelection:true,
            allowSearchFilter: this.ShowFilter
          };
          this.dropdownSettings_NurseStations = {
            singleSelection: true,
            idField: "NurseStation_Id",
            textField: "NurseStation_Name",
            text: "Nursing Stations",
            selectAllText: "Select All",
            unSelectAllText: "UnSelect All",
            itemsShowLimit: 1,
            closeDropDownOnSelection:true,
            noDataAvailablePlaceholderText: 'Please Select Facility',
            allowSearchFilter: this.ShowFilter
          };
  
        this.alertService.getAlert(this.id).subscribe((alert: Alert) => {
            if (!alert.message) {
                // clear alerts when an empty alert is received
                this.alerts = [];
                return;
            }
            // add alert to array
            if(alert.type!=3 || alert.message=="No orders due" ||alert.message=="No data available."||alert.message=="End date cannot be before start date")
            {
                this.alerts.push(alert);
                setTimeout(() => this.removeAlert(alert), 750);
            }
            else
            {
                this.alertsPop=[];
                this.alertsPop.push(alert);
                this.modalAlertOpen=true;
            }

        });
        this.displayName = this.persistanceService.get('displayname');
        if (this.displayName != null)
            this.displayName = this.displayName.substring(1, this.displayName.length - 1);
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.getDosesDetails(0,null);
        this.getUserRecentFacilityNurseStations();
    }
    getUserRecentFacilityNurseStations() {
        this.sharedService.getUserRecentFacNs(this.userId)
          .subscribe(res => {
            if (res != undefined) {
              this.loginUserReceFacility = res.Facility_Id;
              this.loginUserReceNurseStation = res.NurseStation_Id;
              this.nsIds =res.NurseStation_Id;
            }
            this.getFiltersData(this.userId);
          }, error => {
            this.alertService.error(error.message);
          });
      }
    getFiltersData(userId: number): any {
        this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
          .subscribe((res: any) => {
           // this.ng4LoadingSpinnerService.hide();
            this.facilities = res.Facilities;
    
            // if (this.loginUserReceFacility != null) {
            //   if (this.facilities.length > 0) {
            //     let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            //     this.selectedfaItems = [];
            //     if (checkFacExist != undefined) {
            //       this.selectedfaItems.push(checkFacExist);
            //       this.getNurseStationByFacilityID(this.loginUserReceFacility);
            //     }
            //     this.myform.patchValue({
            //       ddlfacilities: this.selectedfaItems,
            //     });
            //   }
            // }
            // if (res.Facilities.length == 1) {
            //   this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
            //   this.myform.patchValue({
            //     ddlfacilities: this.facilities,
            //   });
            // }
          }, error => {
            this.alertService.error(error.message);
    //        this.ng4LoadingSpinnerService.hide();
          });
          
      }
    @HostListener('document:click', ['$event.target'])
    // public onClick(targetElement) {
    //     if (this.iscollapsed ==false) {
    //         $('.login').addClass('show');
    //         this.iscollapsedOutClick = false;
    //         this.iscollapsed =true;
    //       }
    //       else {
    //         $('.login').removeClass('show');
    //         this.iscollapsedOutClick = true;
    //       }
    // }
    public onClick(targetElement) {
        if (this.iscollapsed ==false) {
            $('.login').addClass('show'); 
            if (!this.iscollapsedAlerts) {
                $('.doses').removeClass('show'); 
                this.iscollapsedAlerts = true;               
              }         
              if(targetElement.classList[1]=="openmsgclass") {
                $('.doses').addClass('show'); 
                this.iscollapsedAlerts = false;    
              }
            this.iscollapsedOutClick = false;
            this.iscollapsed =true;
          }         
          else {
            if (!this.iscollapsedAlerts) {
                $('.doses').removeClass('show'); 
                this.iscollapsedAlerts = true;               
              }
              if(targetElement.classList[1]=="openmsgclass") {
                $('.doses').addClass('show'); 
                this.iscollapsedAlerts = false;    
              }
            $('.login').removeClass('show');            
            this.iscollapsedOutClick = true;
          }
        }

    logoutClicked() {
        this.persistanceService.logoutUser();
    }
    removeAlert(alert: Alert) {
        this.alerts = this.alerts.filter(x => x !== alert);
    }
    getDosesData()
    {
      this.modalDosesIsOpen =true;
    }
    getOutboundData()
    {
      this.modalOutboundErrorIsOpen =true;
    }
    closeModel()
    {
        this.myform.patchValue({
            ddlnursestations: '',
          });
        this.myform.reset();
        this.nurseStations =[];
        this.getDosesDetails(0,null);
        this.modalDosesIsOpen =false;
    }
    closeModelOrder()
    {
        this.modalOutboundErrorIsOpen =false;
    }
    getNurseStationByFacilityID(facilityId: any) {
        // this.ng4LoadingSpinnerService.show();
        //let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
        this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
          .subscribe((res: any[]) => {
            this.nurseStations = res;
    
            // if (this.loginUserReceNurseStation != undefined) {
            //   let userReceNSList = this.loginUserReceNurseStation.split(',');
            //   if (userReceNSList.length > 0) {
            //     this.selectednItems = [];
            //     for (let i = 0; i < userReceNSList.length; i++) {
            //       let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
            //       if (checkNsExist != undefined) {
            //         this.selectednItems.push(checkNsExist);
            //       }
            //       if(this.selectednItems.length==1)
            //       break;
            //     }
            //     this.myform.patchValue({
            //       ddlnursestations: this.selectednItems,
            //     });
            //   }
            // }
          }, error => {
            this.alertService.error(error.message);
          });
      }
    onFacilitySelect(item: any) {
        this.nurseStations = [];
        this.myform.patchValue({
          ddlnursestations: '',
        });
        this.getNurseStationByFacilityID(item.Facility_Id);
        this.getDosesDetails(item.Facility_Id,null);
        //this.getCompanyToBedData(item.Facility_Id);
      }
      onFacilityDeSelect(item: any) {
        
        this.nurseStations = [];
        this.myform.patchValue({
          ddlnursestations: '',
        })
        this.getDosesDetails(0,null);
      }
      onNurseStationSelect(item: any) {
        this.getDosesDetails(0,item.NurseStation_Id);
      }
      onNurseStationSelectAll(item: any) {
        this.myform.value.ddlnursestations = item;
        
      }
      onNurseStationDeSelect(item: any) {
        this.getDosesDetails(item.Facility_Id,null);
      }
      onNurseStationDeSelectAll(item: any){
        this.alertService.error("Please select at least one nursing station to display the data.")
        
      }
    getDueMARAlert() {
        this.dataservice.get<any>(this.config.Emar_GetDueMARAlertData + this.persistanceService.get(this.config.loggedInUserKey))
            .subscribe(res => {
                this.dueAlert = res;
            });
    }
    getDosesDetails(FacilityId:any,NsId:string)
    {
        this.dataservice.get<any>(this.config.Emar_Common_GetPendingDosesList + this.persistanceService.get(this.config.loggedInUserKey) +"/"+NsId +"/"+FacilityId)
        .subscribe(res => {
            this.DosesList = res;
        });  
    }
    getOutboundErrorDetails()
    {
        this.dataservice.get<any>(this.config.Emar_Common_GetOutboundErrorDetails + this.persistanceService.get(this.config.loggedInUserKey))
        .subscribe(res => {
            this.OutBoundErrorList = res;
            if(res.length > 0)
            {
            this.ErrorCount =this.OutBoundErrorList.length
           }
           else
           {
               this.ErrorCount =0;
           }
        });  
    }
    insertOutboundErrorIgnoreStatus(fileId:number)
    {
        this.dataservice.get<any>(this.config.Emar_Common_InsertIgnoreOutboundErrorDetails + fileId)
        .subscribe(res => {
         this.getOutboundErrorDetails();
        });   
    }
    insertDosesIgnoreStatus(DrugAdministerId:number)
    {
        this.dataservice.get<any>(this.config.Emar_Common_InsertIgnoreDosesDetails + DrugAdministerId)
        .subscribe(res => {
         this.getDosesDetails(0,null);
         this.myform.patchValue({
            ddlnursestations: '',
          });
          this.myform.reset();
          this.nurseStations =[];
         this.getDueMARAlert();
         this.getOutboundErrorDetails();
        });   
    }
    getAlertsData() {
        this.dataservice.get<any[]>(this.config.Emar_AlertCount_GetAlertsDetailsCount + this.persistanceService.get(this.config.loggedInUserKey))
        .subscribe(res => {
                this.alertsData = res[0].TypeCount;
            }, error => {
                this.alertService.error(error.message);
            });
    }
    getAllAlertsData(TypeId: number) {
        $('.alerts').removeClass('show');
        this.iscollapsedAlerts = true;
        this.sharedService.alertTypeId(TypeId);
        this.router.navigate(['/home/alerts']);
    }
    getAllMailData(type:any)
    {
        $('.mail').removeClass('show');
        this.iscollapsedAlerts = true;
        this.router.navigate(['/home/mailbox']);
    }
    getMailData() {
        this.dataservice.get<any>(this.config.Emar_Mailbox_GetReadMailsCount + this.persistanceService.get(this.config.loggedInUserKey))
        .subscribe(res => {
                this.mailData = res;
            }, error => {
                this.alertService.error(error.message);
            });
    }
    closeModelAlert()
    {
        this.modalAlertOpen=false;
    }
    showHideToggle(cls) {
        if (this.valueChangesFlagReceive == 1) {
            const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
            modalRef.componentInstance.result.subscribe((receivedResult) => {
              if (receivedResult == 2) {
                modalRef.close();
              }
              else if (receivedResult == 1) {
                this.sharedService.saveChangesOrderInfo(0);
                this.valueChangesFlagReceive=0;
                if (!this.iscollapsed || !this.iscollapsedOutClick) {
                    $(cls).removeClass('show');
                    this.iscollapsed = true;
                  }
                  else {
                    $(cls).addClass('show');
                    this.iscollapsed = false;
                  }
              }
              modalRef.close();
            });
          }
          else {
            if (!this.iscollapsed || !this.iscollapsedOutClick) {
                $(cls).removeClass('show');
                this.iscollapsed = true;
              }
              else {
                $(cls).addClass('show');
                this.iscollapsed = false;
              }
          }
      }
      showHideToggleAlerts(cls) {
        if (this.valueChangesFlagReceive == 1) {
            const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
            modalRef.componentInstance.result.subscribe((receivedResult) => {
              if (receivedResult == 2) {
                modalRef.close();
              }
              else if (receivedResult == 1) {
                this.sharedService.saveChangesOrderInfo(0);
                this.valueChangesFlagReceive=0;
                if (!this.iscollapsedAlerts) {
                    $(cls).removeClass('show');
                    this.iscollapsedAlerts = true;
                  }
                  else {
                    this.getDosesDetails(0,null);
                    this.getDueMARAlert();
                    $(cls).addClass('show');
                    this.iscollapsedAlerts = false;
                    if(cls==".alerts")
                    {
                    $(".mail").removeClass('show');
                    $(".doses").removeClass('show');
                    }
                    else if(cls==".mail")
                    {
                    $(".alerts").removeClass('show');
                    $(".doses").removeClass('show');
                    }
                    else if(cls==".doses")
                    {
                    $(".alerts").removeClass('show');
                    $(".mail").removeClass('show');
                    }
                  }
              }
              modalRef.close();
            });
          }
        else
        {
        if (!this.iscollapsedAlerts) {
            $(cls).removeClass('show');
            this.iscollapsedAlerts = true;
          }
          else {
            this.getDosesDetails(0,null);
            this.getDueMARAlert();
            $(cls).addClass('show');
            this.iscollapsedAlerts = false;
            if(cls==".alerts")
            {
            $(".mail").removeClass('show');
            $(".doses").removeClass('show');
            }
            else if(cls==".mail")
            {
            $(".alerts").removeClass('show');
            $(".doses").removeClass('show');
            }
            else if(cls==".doses")
            {
            $(".alerts").removeClass('show');
            $(".mail").removeClass('show');
            }
          }
        }
         
    }
    Home() {
        if (this.valueChangesFlagReceive == 1) {
            const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
            modalRef.componentInstance.result.subscribe((receivedResult) => {
              if (receivedResult == 2) {
                modalRef.close();
              }
              else if (receivedResult == 1) {
                this.sharedService.saveChangesOrderInfo(0);
                this.valueChangesFlagReceive=0;
                this.router.navigate(['/home/dashboard']);
              }
              modalRef.close();
            });
          }
          else {
            this.router.navigate(['/home/dashboard']);
          }
      }
    cssClass(alert: Alert) {

        if (!alert) {
            return;
        }

        // return css class based on alert type
        switch (alert.type) {
            case AlertType.Success:
                return 'alert alert-success';
            case AlertType.Error:
                return 'alert alert-danger';
            case AlertType.Info:
                return 'alert alert-info';
            case AlertType.Warning:
                return 'alert alert-warning';
        }
    }
    changePwd() {
        this.persistanceService.sendForgotPassword(null, this.userId).subscribe(res => {
            if (res == 1) {
                //alert('OTP Sent to your Registered Email Id');
                this.forgotPassFlag = 1;
                this.type = 0;
                this.otpflag = 1;
            }
        }, error => {
            this.errmsg="Problem with Mail Configuration. Contact Administrator";

        });
    }
    NoteDisplay() {
        this.msg = 0;
    }
    ChangePwdReset() {
        this.forgotPassFlag = 0;
        this.msg = 1;
        this.otpflag = 0;
    }  
    getPastDueAlertFlag() {
      this.dataservice.get<any>(this.config.Emar_Common_GetUserPassedDueFlag + this.persistanceService.get(this.config.loggedInUserKey))
      .subscribe(res => {
              this.userPastDueAlertFlag = res;
          }, error => {
              this.alertService.error(error.message);
          });
   }
}
// export class AlertComponent {
//   @Input() id: string;

//   alerts: Alert[] = [];

//   constructor(private alertService: AlertService) { }

//   ngOnInit() {
//       this.alertService.getAlert(this.id).subscribe((alert: Alert) => {
//           if (!alert.message) {
//               // clear alerts when an empty alert is received
//               this.alerts = [];
//               return;
//           }
//           // add alert to array
//           this.alerts.push(alert);
//           setTimeout(() => this.removeAlert(alert), 5000);
//       });
//   }

//   removeAlert(alert: Alert) {
//       this.alerts = this.alerts.filter(x => x !== alert);
//   }

//   cssClass(alert: Alert) {
//     debugger;
//       if (!alert) {
//           return;
//       }

//       // return css class based on alert type
//       switch (alert.type) {
//           case AlertType.Success:
//               return 'alert alert-success';
//           case AlertType.Error:
//               return 'alert alert-danger';
//           case AlertType.Info:
//               return 'alert alert-info';
//           case AlertType.Warning:
//               return 'alert alert-warning';
//       }
//   }
// }