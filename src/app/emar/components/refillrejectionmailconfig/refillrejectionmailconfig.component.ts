import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-refillrejectionmailconfig',
  templateUrl: './refillrejectionmailconfig.component.html',
  styleUrls: ['./refillrejectionmailconfig.component.css']
})
export class RefillrejectionmailconfigComponent implements OnInit {
  public refillMailConfigList: any[] = [];
  public template;
  myform: FormGroup;
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  pageConfig: {};
  public facilities: any[];
  public nurseStations: any=[];
  dropdownSettings_Facilities: any = {};
  dropdownSettings_NurseStations: any = {};
  public selectedfaItems = [];
  public selectednItems = [];
  userId: number;
  public refillConfigId:number=0;
  public hoursList: any=[];
  public selectedTimeItems = [];
  dropdownSettings_Times: any = {};

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Refill/DiscontinueRejectionMailConfiguration");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
      this.template = this.dataservice.template;
      this.userId= this.persistanceService.get(this.config.loggedInUserKey);
      this.ng4LoadingSpinnerService.show();
      this.myform = new FormGroup({
      facilityName: new FormControl('', Validators.required),
      nursestationname: new FormControl('',Validators.required),
      tomail:new FormControl('',[Validators.required,Validators.pattern(this.config.eMail)]),
      ccmail:new FormControl('',Validators.pattern(this.config.eMail)),
      hours: new FormControl('', Validators.required),
    });
    this.dropdownSettings_Facilities = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Facilities",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter:true
    };
    this.dropdownSettings_NurseStations = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Nursing Stations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: true
    };
    this.dropdownSettings_Times = {
      singleSelection: false,
      idField: "Hour_Id",
      textField: "Hour_Desc",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
    };
    this.userActivity();
    this.getUserFacilies(this.userId);
    this.getRefillCMailConfigMasterData();
  }
 }
 else
 this.persistanceService.redirectToHomePage();
}
userActivity() {
  this.sharedService.insertUserActivityDetails(Screens.RefillRejectMailConfig, Activity.View, '')
    .subscribe(res => { }, error => {
      this.alertService.error(error.message);
    });
}
getUserFacilies(userId: number) {
  this.ng4LoadingSpinnerService.show();
  this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
    .subscribe((res: any) => {
      this.facilities = res.Facilities;
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
}
onFacilitySelect(item:any)
{
  this.nurseStations=[];
  this.selectednItems=[];
  this.selectedTimeItems=[];
  this.getNurseStationByFacilityID(item.Facility_Id);
  this.getHoursMasterData(item.Facility_Id)
}
getNurseStationByFacilityID(facilityId: number) {
  this.ng4LoadingSpinnerService.show();
  this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
    .subscribe((res: any) => {
      this.nurseStations = res;
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
onFacilityDeSelect(item:any)
{
  this.selectednItems=[];
  this.nurseStations=[];
  this.selectedTimeItems=[];
  this.hoursList=[];
}
reSet()
{
  this.myform.reset();
  this.selectedfaItems=[];
  this.selectednItems=[];
  this.nurseStations=[];
  this.selectedTimeItems=[];
  this.hoursList=[];
  this.refillConfigId=0;
}
getHoursMasterData(facilityId:number,fetchRecord?:any) {
  this.dataservice.get<any[]>(this.config.Emar_Orders_GetHoursDataByNSId + 0 + "/" + facilityId)
    .subscribe(res => {
       
      this.hoursList = res;
      if(fetchRecord!=undefined)
      {
        this.fetchData(fetchRecord);
      }
    }, error => {
      this.alertService.error(error.message);
    });
}
insertRefillConfig()
{
   
  let nurseStations="";
  let hours="";
  if(this.myform.value.ccmail!=undefined && this.myform.value.ccmail!=null && this.myform.value.ccmail!=""&& this.myform.value.tomail.toString().toLowerCase()==this.myform.value.ccmail.toString().toLowerCase())
  {
    this.alertService.warn("To Email Id and CC Email Id can't be same");
    this.ng4LoadingSpinnerService.hide();
  }
  else
  {
  if(this.myform.value.nursestationname!=undefined && this.myform.value.nursestationname!=null && this.myform.value.nursestationname.length!=0)
  {
    let arNurseStations = this.myform.value.nursestationname;
      arNurseStations.forEach(element => {
        nurseStations += element.NurseStation_Id + ",";
      });
      nurseStations = nurseStations.substring(0, nurseStations.length - 1);
  }
  if(this.myform.value.hours!=undefined && this.myform.value.hours!=null && this.myform.value.hours.length!=0)
  {
    let arHours = this.myform.value.hours;
    arHours.forEach(element => {
        hours += element.Hour_Id + ",";
      });
      hours = hours.substring(0, hours.length - 1);
  }
  let obj=
  {
    Rdc_Id:this.refillConfigId,
    Facility_Id:this.myform.value.facilityName!=undefined && this.myform.value.facilityName!=null && this.myform.value.facilityName.length!=0?this.myform.value.facilityName[0].Facility_Id:null,
    NurseStation_Id:nurseStations,
    TimeIDs:hours,
    MailTo:this.myform.value.tomail,
    MailCC:this.myform.value.ccmail!=undefined && this.myform.value.ccmail!=null && this.myform.value.ccmail!=""?this.myform.value.ccmail:null,
    Status:1,
    CreatedBy:this.userId,
    CreatedDate:this.dateFormatPipe.dateWithTime(new Date()),
  }
  this.ng4LoadingSpinnerService.show();
  this.dataservice.post(this.config.Emar_Common_InsertUpdateRefillMailConfig, obj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if(res==1)
        {
        this.alertService.success("Save successful");
        this.getRefillCMailConfigMasterData();
        this.reSet();
        }
        else if(res==2)
        {
          this.alertService.warn("Selected faciity already configred. You can't insert you can update it")
        }
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
      }
}
getRefillCMailConfigMasterData() {
  this.ng4LoadingSpinnerService.show();
  this.dataservice.get<any[]>(this.config.Emar_Common_GetRefillConfigGridData + this.userId)
    .subscribe(res => {
       
      this.refillMailConfigList = res;
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
    });
}
getRefillConfigs(facId:any,nsIds:any,timeIds:any,mailTo:any,mailCC:any)
{
   
  this.getNurseStationByFacilityID(facId);
  this.refillConfigId=1;
  let obj=
  {
    FacilityId:facId,
    NusringStationIds:nsIds,
    Times:timeIds,
    MailTo:mailTo,
    MailCC:mailCC
  }
  this.getHoursMasterData(facId,obj);
  //this.fetchData(obj);
}
fetchData(res:any)
{
  this.selectedfaItems=[];
  this.selectednItems=[];
  this.selectedTimeItems=[];
  let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(res.FacilityId));
  if(checkFacExist!=undefined)
  {
    this.selectedfaItems.push(checkFacExist);
    this.myform.patchValue({
      facilityName:this.selectedfaItems,
    });
  }
  let NSList = res.NusringStationIds.split(',');
  if(NSList.length>0)
  {
    for (let i = 0; i < NSList.length; i++) {
      let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(NSList[i]));
      if (checkNsExist != undefined) {
        this.selectednItems.push(checkNsExist);
      }
    }
    this.myform.patchValue({
      nursestationname:this.selectednItems,
    });
  }
  let TimesList = res.Times.split(',');
  if(TimesList.length>0)
  {
    for (let i = 0; i < TimesList.length; i++) {
      let checkHourExist = this.hoursList.find(r => r.Hour_Id === parseInt(TimesList[i]));
      if (checkHourExist != undefined) {
        this.selectedTimeItems.push(checkHourExist);
      }
    }
    this.myform.patchValue({
      hours:this.selectedTimeItems,
    });
  }
  this.myform.patchValue({
    tomail:res.MailTo,
    ccmail:res.MailCC,
  });
  window.scroll(0, 0);
}
}
