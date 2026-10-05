import { Component, OnInit, OnDestroy } from '@angular/core';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';;
import { Screens, Activity } from '../../../models/useractivity.model';
import { AlertService } from '../../../_services';
import { Router, NavigationEnd } from '@angular/router';
import { AlertStatus } from '../../../models/mailbox.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.css']
})
export class AlertsComponent implements OnInit, OnDestroy {
  public template;
  alertsData: any[]=[];
  public userId: number;
  public typeId: number = 1;
  public searchText:string="";
  myform: FormGroup;
  public selectedRecords: any[] = [];
  public alertStatusObj:AlertStatus;
  pageConfig = {};
  CheckAll: boolean = false;
  public alertType:string="";
  public p:number=1;
  public gridPagination:number=this.config.gridPagination;
  public loginUserReceCompany:any;
  dropdownSettings_Company: any = {};
  public selectedResItem = [];
  public selectedComItem = [];
  public companyMaster: any[];
  companyId: number;
  companyselected;
  dropdownSettings_Resident: any = {};
  public residents: any[];
  selectedResidents: any[];
  patientid: string;
  public AlertTypeId:number=0
  public display:number=1;
  public alertTextDisplay:any;
  fileViewData: any;
  public fileError: any[] = [];
  public fileAck: string;
  navigationSubscription;
  public totalRecords:number=0;

  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private config: APIConfiguration, private route: Router, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,private formBuilder: FormBuilder) {
    this.navigationSubscription = this.route.events.subscribe((e: any) => {
      // If it is a NavigationEnd event re-initalise the component
      if (e instanceof NavigationEnd) {
        debugger
       this.initialiseInvites();
      }
    });
   }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Alerts");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.sharedService.typeId.subscribe(res => this.AlertTypeId = res);
    this.myform = new FormGroup({
      check: new FormControl(''),
      company: new FormControl(''),
      resident: new FormControl(''),
    });
    this.dropdownSettings_Resident = {
      singleSelection: false,
      idField: "Patient_Id",
      textField: "PatientName",
      itemsShowLimit: 1,
      limitSelection: 10,
      allowSearchFilter: true
    };
    this.dropdownSettings_Company = {
      singleSelection: true,
      idField: "Company_Id",
      textField: "Company_Name",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.userActivity();
    //this.getAlertsData();
    //this.getUserRecentCompany();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getUserRecentCompany() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceCompany = res.companyId;
        }
        this.getCompanyMaster()
      }, error => {
        this.alertService.error(error.message);
      });
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Alerts, Activity.View, '')
      .subscribe(res => { 
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getAlertsData() {
    debugger
    this.ng4LoadingSpinnerService.show();
    this.alertType="All";
    this.CheckAll=false;
    if (this.typeId == 0 || this.typeId == undefined) {
      this.dataservice.get<any[]>(this.config.Alerts_GetAlertsData + this.userId + "/" + 0)
        .subscribe(res => {
          this.alertsData = res;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else {
      this.dataservice.get<any[]>(this.config.Alerts_GetAlertsData + this.userId + "/" + this.typeId)
        .subscribe(res => {
          this.alertsData = res;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  getAlertsDataAll() {
    this.ng4LoadingSpinnerService.show();
    this.alertType="All";
    this.CheckAll=false;
    this.dataservice.get<any[]>(this.config.Alerts_GetAlertsData + this.userId + "/" + 0)
      .subscribe(res => {
        this.alertsData = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  favourite(AlertTextId:number, favouriteFlag: number)
  {
    this.alertStatusObj={
      AlertText_Id:AlertTextId,
      Favourite_Flag:favouriteFlag,
      Alert_CreatedDate:this.dateFormatPipe.transform(new Date()),
    };
    this.dataservice.post(this.config.Emar_Alerts_InsertAlertFavStatus,this.alertStatusObj)
        .subscribe((res: any) => {
          //this.getFavdetails();
          //this.getTrashdetails();
          //this.getAlertsData();
          //this.getAlertsDataAll();
          this.getAllAlerts(1,this.typeId);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getFavdetails()
  {
    this.ng4LoadingSpinnerService.show();
    this.alertType="Fav";
    this.CheckAll=false;
    this.dataservice.get<any[]>(this.config.Alerts_GetAlertsData + this.userId + "/" + 0 +"/"+1)
      .subscribe(res => {
        this.alertsData = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getTrashdetails()
  {
    this.ng4LoadingSpinnerService.show();
    this.alertType="Trash";
    this.CheckAll=false;
    this.dataservice.get<any[]>(this.config.Alerts_GetAlertsData + this.userId + "/" + 0 +"/"+0+"/"+1)
      .subscribe(res => {
        this.alertsData = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  onCheckAll(event) {

    if (event == true) {
      this.myform.patchValue({
        check: 1,
      });
      this.CheckAll = true;
      this.selectedRecords=this.alertsData;
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
      const index = this.selectedRecords.findIndex(i => i.AlertText_Id == item.AlertText_Id);
      this.selectedRecords.splice(index, 1);
    }
  }
  trashRecord() {
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select record to move to trash");
    }
    else if (this.selectedRecords.length != 0)
    {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.post(this.config.Emar_Alerts_TrashAlertRecord, this.selectedRecords)
        .subscribe(res => {
          if (res = 1)
          {
          this.myform.controls["check"].reset();
          if(this.typeId==3)
          {
            this.alertService.warn("Record Deleted");
          }
          else
          {
            this.alertService.warn("Moved To Trash");
          }
          this.selectedRecords = [];
          this.getAllAlerts(1,this.typeId);
          // this.getFavdetails();
          // this.getTrashdetails();
          // this.getAlertsData();
          // this.alertType="All";
          }
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
  }
  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => {
        this.companyMaster = res;
        if (this.companyMaster.length > 0) {
          if (this.loginUserReceCompany != null) {
              let checkComExist = this.companyMaster.find(r => r.Company_Id == parseInt(this.loginUserReceCompany));
              this.selectedComItem = [];
              if (checkComExist != undefined) {
                this.selectedComItem.push(checkComExist);
              this.myform.patchValue({
                company: this.selectedComItem,
              });
              this.companyselected = this.selectedComItem[0].Company_Id;
              this.getResidentDropData(this.companyselected);
              }
              else {
                this.companyselected = this.companyMaster[0].Company_Id;
                this.getResidentDropData(this.companyMaster[0].Company_Id);
                this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
                this.myform.patchValue({
                  company: this.selectedComItem,
                });
              }
          } 
          else
          {
              this.companyselected = this.companyMaster[0].Company_Id;
              this.getResidentDropData(this.companyMaster[0].Company_Id);
              this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
              this.myform.patchValue({
                company: this.selectedComItem,
              });
          }
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getResidentDropData(companyId: number) {
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Alerts_GetAlertsResDrop + userId + "/" + this.companyselected)
      .subscribe(res => {
        this.residents = res;
        if(res.length>0)
        {
        for(let i=0;i<this.residents.length;i++)
        {
          this.selectedResItem.push(this.residents[i]);
          if(this.selectedResItem.length==10)
          break;
        }
        this.myform.patchValue({
          resident: this.selectedResItem
        });
      this.getAllAlerts(1,1);
      }
      else{
        this.alertsData=[];
        this.ng4LoadingSpinnerService.hide();
      }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getAllAlerts(currentPage:number, typeId?:any) {
    if((this.myform.value.company==undefined ||this.myform.value.company==null ||this.myform.value.company.length==0) || (this.myform.value.resident==undefined ||this.myform.value.resident==null ||this.myform.value.resident.length==0))
    {
      this.alertService.warn("Please use filters to display alerts");
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
    this.ng4LoadingSpinnerService.show();
    this.typeId=typeId==undefined?this.typeId: typeId;
    this.display=1;
    this.p=currentPage;
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.selectedResidents=[];
    this.selectedResItem.forEach(element => {
    this.selectedResidents.push(element.Patient_Id);
    });
    this.patientid = "";
    this.patientid = this.selectedResidents.join(',');
    let obj=
    {
      UserId:userId,
      TypeId:this.typeId,
      Residents:this.patientid,
      AlertTypeId:this.AlertTypeId,
      CurrentPage:this.p,
      PageSize:this.gridPagination,
      SearchText:this.searchText != undefined && this.searchText != null ? this.searchText.replace(/[&\\\#,+()$~%'":.*?<>{}\s]/g, '').replace(new RegExp('/', 'g'), '-') : '',
    }
    this.dataservice.post(this.config.Emar_Alerts_GetAllAlertsData, obj)
      .subscribe(res => {
        this.alertsData = res;
        if(res.length>0)
        {
          this.totalRecords=res[0].TotalRecords;
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
onCompanySelect(item:any)
{
  debugger
  this.companyselected=item.Company_Id;
  this.selectedResItem=[];
  this.residents==[];
  this.myform.patchValue({
    resident: this.selectedResItem
  });
  this.getResidentDropData(this.companyselected);
}
onCompanyDeSelect(item:any)
{
  this.companyselected=0;
  this.alertsData=[];
  this.residents=[];
  this.myform.value.resident.length = 0;
  this.patientid = "";
  this.selectedResItem = [];
  this.myform.patchValue({
    resident: this.selectedResItem
  });
  this.alertService.warn("Please use filters to display alerts");
  this.ng4LoadingSpinnerService.hide();
}
  onResidentSelect(item: any) {
    this.getSelectedResidents();
  }
  onResidentSelectAll(item: any) {
    this.myform.value.resident = item;
    this.getSelectedResidents();
  }
  onResidentDeSelect(item: any) {
    this.getSelectedResidents();
  }
  onResidentDeSelectAll(item: any) {
    this.myform.value.resident.length = 0;
    this.patientid = "";
    this.selectedResItem = [];
    this.alertsData=[];
    this.alertService.warn("Please use filters to display alerts");
    this.ng4LoadingSpinnerService.hide();
  }
  getSelectedResidents() {
    if (this.myform.value.resident.length != 0) {
       this.selectedResidents=[];
       this.myform.value.resident.forEach(item => {
         this.selectedResidents.push(item.Patient_Id);
       });
       this.patientid = "";
       this.patientid = this.selectedResidents.join(',');
       this.getAllAlerts(1,this.typeId);
     }
   }
   alertBody(item:any)
   {
    this.fileViewData=null;
    this.fileError=[];
    this.fileAck="";
    this.dataservice.get<any>(this.config.Emar_Alerts_InsertReadAlert+ item.AlertTextId+"/"+this.userId)
    .subscribe(res => {
      this.alertTextDisplay=item;
      this.display=2;
      this.getAlertCount();
      if(item.File_Id!=null)
      {
      this.GetFileAckData(item.File_Id);
      }
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
   }
   GetFileAckData(fileID: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_Inbound_GetFileAckData + fileID)
      .subscribe(res => {
        this.fileViewData = res;
        this.ng4LoadingSpinnerService.hide();
        if (res.FileError != null) {
          this.fileAck = "File Ack";
          this.fileError = res.FileError;
        }
        else if (res.FileError == null) {
          this.fileAck = "File Ack Error";

        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getAllAlertsRecords(id:any)
  {
    this.AlertTypeId=0;
    this.typeId=id;
    this.getAllAlerts(1,id);
  }
  initialiseInvites() {
    // Set default values and re-fetch any data you need.
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.sharedService.typeId.subscribe(res => this.AlertTypeId = res);
   
    this.selectedComItem=[];
    this.selectedResItem=[]; 
    this.dropdownSettings_Resident = {
      singleSelection: false,
      idField: "Patient_Id",
      textField: "PatientName",
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
    this.dropdownSettings_Company = {
      singleSelection: true,
      idField: "Company_Id",
      textField: "Company_Name",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      closeDropDownOnSelection:true,
    };
    this.getUserRecentCompany();
  }
  ngOnDestroy() {
    // avoid memory leaks here by cleaning up after ourselves. If we  
    // don't then we will continue to run our initialiseInvites()   
    // method on every navigationEnd event.
    if (this.navigationSubscription) {  
       this.navigationSubscription.unsubscribe();
    }
  }
  onSearchChange(searchValue:string)
  {
    if (searchValue.length >= 3) {
      if (searchValue.includes(':')) {
        this.alertService.warn('Invalid character');
      }
      else {
        this.ng4LoadingSpinnerService.show();
        this.getAllAlerts(1);
      }
    }
    else if (searchValue.length == 0) {
      this.ng4LoadingSpinnerService.show();
      this.getAllAlerts(this.p);
    }
  }
  getRefreshRecords(id:number)
  {
    this.AlertTypeId=0;
    this.typeId=id;
    this.getAllAlerts(1,id);
    this.getAlertCount();
  }
  getAlertCount()
  {
    this.dataservice.get<any[]>(this.config.Emar_AlertCount_GetAlertsDetailsCount + this.persistanceService.get(this.config.loggedInUserKey))
        .subscribe(res => {
                this.sharedService.alerts(res[0].TypeCount);
            }, error => {
                this.alertService.error(error.message);
            });
  }
}
