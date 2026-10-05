import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { AlertService } from '../../../_services/index';
import { Vitals, AdmintDateDrop } from '../../../models/assesments.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { DomSanitizer } from '@angular/platform-browser';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NurseStation } from '../../../models/facility.model';
@Component({
  selector: 'app-vitals',
  templateUrl: './vitals.component.html',
  styleUrls: ['./vitals.component.css']
})
export class VitalsComponent implements OnInit {
  public template;
  vitalsId: number = 0;
  public dateArrival: any;
  public residentID: number=0;
  private residentVisitId: number;
  public residents: ResidentDemographic[];
  public demographicInfoData: any={};
  myform: FormGroup;
  private vitalsobj: Vitals;
  public residentAdmitDate: AdmintDateDrop[];
  public vitalDetails: any[] = [];
  errorMessage: string;
  pageConfig = {};
  private value: string;
  patientID: number;
  private visitVital: any[];
  //nitial:string;
  displayName: string;
  searchText: string = "";
  gridPagination = this.config.gridPagination;
  p: number = 1;
  public selectedResItem = [];
  public dropdownSettings_Resident: any = {};
  public nurseStationName: string;
  public saveDisable=false;
  public BPMask = [/[0-9]/, /\d/, /\d/, '/', /\d/, /\d/];
  public nursingStation:any;
  nursingStationZoneCurrentDate: any;
  public selectedfaItems = [];
  public nurseStations: NurseStation[];
  public facilities: any[];
  public selectednItems: any[];
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  public userId:number;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  MyImages: any;
  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sanitizer: DomSanitizer, private sharedService: SharedService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.userActivity();
    this.displayName = this.persistanceService.get('displayname');

    if (this.displayName != null)
      this.displayName = this.displayName.substring(1, this.displayName.length - 1);
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Vitals");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.dateArrival = new Date();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dropdownSettings_Facilities = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Facilities",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.dropdownSettings_NurseStations = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Nursing Stations",
      // selectAllText: "Select All",
      //  unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: true
    };

    this.dropdownSettings_Resident = {
      singleSelection: true,
      idField: "Patient_Id",
      textField: "PatientName",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Nursing Station',
      allowSearchFilter: true
    };
    this.myform = new FormGroup({
      date: new FormControl(new Date().toISOString().substring(0, 10)),
      bp: new FormControl('', [Validators.minLength(6), Validators.maxLength(6)]),
      temperature: new FormControl('', [Validators.maxLength(6),Validators.pattern(this.config.decimalAllowTwoDigits)]),
      respiration: new FormControl('', [Validators.pattern(this.config.numeric), Validators.maxLength(2)]),
      bloodsugar: new FormControl('', [Validators.pattern(this.config.numeric), Validators.maxLength(3)]),
      pulseoximetry: new FormControl('', [ Validators.pattern(this.config.numeric), Validators.maxLength(3)]),
      heartrate: new FormControl('', [ Validators.pattern(this.config.numeric), Validators.maxLength(3)]),
      initial: new FormControl(this.displayName),
      nursenotes: new FormControl('', [ Validators.maxLength(1000)]),
    });
    this.getUserRecentFacilityNurseStations();
    //this.getResidentDropData();
  }
}
else
this.persistanceService.redirectToHomePage();
    //this.value=this.persistanceService.get(this.config.loggedInUserKey);
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Vitals, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }

getUserRecentFacilityNurseStations() {
    this.ng4LoadingSpinnerService.show();
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          //this.ng4LoadingSpinnerService.hide();
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFiltersData(this.userId);
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getFiltersData(userId: number): any {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.facilities = res.Facilities;
        //this.ng4LoadingSpinnerService.hide();
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            // this.myform.patchValue({
            //   ddlfacility: this.selectedfaItems,
            // });
          }
        } else if (res.Facilities.length == 1) {
          //  this.getCompanyToBedData(res.Facilities[0].Facility_Id);
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.selectedfaItems=this.facilities;
          // this.myform.patchValue({
          //   ddlfacilities: this.facilities,
          // });
        }
        else{
        this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        this.nurseStations = res;
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
              }
              if(this.selectednItems.length==1)
              break;
            }
            if(this.selectednItems.length!=0)
            {
              this.getResidentDropData(this.selectednItems[0].NurseStation_Id);
            //this.getDemographicInfoByNurseStation(this.selectednItems[0].NurseStation_Id);
            }
          }
        }
        else if (this.facilities.length == 1 && this.nurseStations!=undefined && this.nurseStations!=null&& this.nurseStations.length>0) {
          this.selectednItems.push(this.nurseStations[0]);
          this.getResidentDropData(this.nurseStations[0].NurseStation_Id);
        }
        else{
        this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  insertVitals() {
    if((this.myform.value.bp==undefined || this.myform.value.bp==null || this.myform.value.bp=="")&&
    (this.myform.value.heartrate==undefined || this.myform.value.heartrate==null || this.myform.value.heartrate=="")&&
    (this.myform.value.respiration==undefined || this.myform.value.respiration==null || this.myform.value.respiration=="")&&
    (this.myform.value.temperature==undefined || this.myform.value.temperature==null || this.myform.value.temperature=="")&&
    (this.myform.value.pulseoximetry==undefined || this.myform.value.pulseoximetry==null || this.myform.value.pulseoximetry=="") &&
    (this.myform.value.nursenotes==undefined || this.myform.value.nursenotes==null || this.myform.value.nursenotes=="")
    ){
       this.alertService.warn("At least one detail must be entered");
       this.ng4LoadingSpinnerService.hide();
    }
    else
    {
    this.saveDisable=true;
    let dp: string =  this.myform.value.bp;
    let ar: string[] =dp!=undefined && dp!=null && dp!=""?dp.split('/'):[];
    this.vitalsobj = {
      Vitals_ID: this.vitalsId,
      PVisit_Id: this.residentVisitId,
      VitalDate: new Date().toISOString().substring(0, 10),
      VitalTime: new Date().toTimeString().substring(0, 8),//toISOString().substring(11,),
      CistolicBP:ar.length>0? ar[0]:"",
      DiastolicBP:ar.length>0? parseInt(ar[1]):null,
      HeartRate: this.myform.value.heartrate,
      RespiratoryRate: this.myform.value.respiration,
      OxygenRate: 0,
      Temperature: this.myform.value.temperature,
      Pain: null,
      Remark: this.myform.value.nursenotes,
      PulseRate: this.myform.value.pulseoximetry,
      Initials: this.myform.value.initial,

      PDAID: null,
      VitalSigns_status: 1,
      VitalSigns_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      VitalSigns_CreatedOn: new Date().toISOString(),
      BloodSugar: this.myform.value.bloodsugar
    }
    this.dataservice.post(this.config.Emar_Assessments_InsertUpdateVisitVitals, this.vitalsobj)
      .subscribe(res => {

        this.getVitalDetails(this.residentID);
        this.alertService.success("Save successful");

        this.resetScreen();
        this.saveDisable=false;
      }, error => {

        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
        this.saveDisable=false;
      });
    }
  }
  resetScreen() {
    this.myform.reset();
    this.myform.patchValue({
      date: new Date().toISOString().substring(0, 10),
      initial: this.displayName
    });
    this.vitalsId = 0;
    this.saveDisable=false;
  }
  getNurseStationNameByPatientId(residentId: number) {

    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetNurseStationName + residentId)
      .subscribe(res => {
        this.nurseStationName = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getResidentDropData(stationId:any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<ResidentDemographic[]>(this.config.Emar_Assesments_GetResidentsListByNSId + stationId)
      .subscribe(res => {
        this.residents = res;
        this.selectedResItem=[];
        this.ng4LoadingSpinnerService.hide();
        if (res!=undefined && res != null && res.length>0) {
          this.residentID = this.residents[0].Patient_Id;
          this.selectedResItem.push(this.residents[0]);
          this.getDemographicInfoData(this.residentID);
          this.getResidentAdmitDropData(this.residentID);
          this.getVitalDetails(this.residentID);
          //this.ng4LoadingSpinnerService.hide();
        }
        else{
          this.alertService.warn("No data available");
          this.selectedResItem = [];
          this.residentID = 0;
          this.demographicInfoData = {};
          this.vitalDetails = [];
          this.residentAdmitDate = [];
          this.resetScreen();
          this.nurseStationName = "";
          this.ng4LoadingSpinnerService.hide();
        }
      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.selectednItems=[];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.vitalDetails=[];
    this.residentAdmitDate=[];
    this.resetScreen();
    this.nurseStationName="";
    this.ng4LoadingSpinnerService.hide();
    this.getNurseStationByFacilityID(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.selectednItems=[];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.vitalDetails=[];
    this.residentAdmitDate=[];
    this.resetScreen();
    this.nurseStationName="";
  }
  onNurseStationSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.vitalDetails=[];
    this.residentAdmitDate=[];
    this.resetScreen();
    this.nurseStationName="";
    this.getResidentDropData(item.NurseStation_Id);
  }
  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.vitalDetails=[];
    this.residentAdmitDate=[];
    this.resetScreen();
    this.nurseStationName="";
  }
  getDemographicInfoData(residentID: number) {

    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + residentID)
      .subscribe(res => {

        this.demographicInfoData = res;
        this.getNursingStationTimeZone(res.NursingStationId);
        this.getNurseStationNameByPatientId(residentID);
        //this.selectedResItem.push(this.residents.filter(f => f.Patient_Id == residentID)[0]);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        });
  }

  getVitalDetails(residentID: number) {

    this.dataservice.get<any[]>(this.config.Emar_Assessments_GetAllVisitVitalList + residentID)
      .subscribe(res => {
        this.vitalDetails = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  checkVisitVitalDetailsByID(vitalDate: any)

{
  let vitalsDate = new Date(vitalDate);
  let todayDate = new Date(this.nursingStationZoneCurrentDate);
  if (vitalsDate.setHours(0, 0, 0, 0) == todayDate.setHours(0, 0, 0, 0)) {
    
    return true;
  }
  else
  {
    return false;
  }

}
  getVisitVitalDetailsByID(Vitals_ID: number, vitalDate: any) {
    let vitalsDate = new Date(vitalDate);
    let todayDate = new Date(this.nursingStationZoneCurrentDate);
    if (vitalsDate.setHours(0, 0, 0, 0) == todayDate.setHours(0, 0, 0, 0)) {
      // Date equals today's date
      this.dataservice.get<Vitals>(this.config.Emar_Assessments_GetVisitVitalDetailsByID + Vitals_ID)
        .subscribe(res => {
          this.fetchData(res);
        }, error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
        });
      window.scroll(0, 0);
    }
    else {
      window.scroll(0, 0);
      this.resetScreen();
      this.alertService.error("Unable to update previous record");

    }
  }


  fetchData(res: Vitals) {
    this.vitalsId = res.Vitals_ID;
    this.myform.patchValue({
      bp:res.CistolicBP!=undefined && res.CistolicBP!=null && res.DiastolicBP!=undefined && res.DiastolicBP!=null ? (res.CistolicBP + '/' + res.DiastolicBP):"",
      date:this.dateFormatPipe.transformISODate(res.VitalDate), //new Date(res.VitalDate).toISOString().substring(0, 10),
      temperature: res.Temperature,
      respiration: res.RespiratoryRate,
      bloodsugar: res.BloodSugar,
      pulseoximetry: res.PulseRate,
      heartrate: res.HeartRate,
      initial: res.Initials,
      nursenotes: res.Remark

    });
  }
  changeResident() {

    this.residentID == this.selectedResItem[0].Patient_Id;
    this.sharedService.changePatientId(this.residentID);
    this.resetScreen();
    this.getDemographicInfoData(this.residentID);
    this.getResidentAdmitDropData(this.residentID);
    this.getVitalDetails(this.residentID);
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }

  getResidentAdmitDropData(residentID: number) {
    this.dataservice.get<AdmintDateDrop[]>(this.config.Emar_Assesments_GetAdmitDateByID + residentID)
      .subscribe(res => {
        this.residentAdmitDate = res;
        this.residentVisitId = this.residentAdmitDate[0].PVisit_Id;
      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNursingStationTimeZone(stationId: number) {
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        debugger
        this.nursingStationZoneCurrentDate=res;
        this.myform.patchValue({
          date:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate),
        });
      }, error => {
        this.alertService.error(error.message);
      });
  }
  onResidentSelect(item: any) {

    this.residentID = item.Patient_Id;
    this.selectedResItem = this.residents.filter(r => r.Patient_Id == item.Patient_Id);
    this.changeResident();
  }
  onResidentDeSelect(item: any) {
    //this.changeResident();
    this.residentID=0;
    this.demographicInfoData={};
    this.vitalDetails=[];
    this.residentAdmitDate=[];
    this.resetScreen();
    this.nurseStationName="";
    this.selectedResItem=[];
    this.residentID = this.residents[0].Patient_Id;
    this.selectedResItem.push(this.residents[0]);
    this.changeResident();
  }
  mouseEnter(Id:any)
  {
    
    this.MyImages = Id;
   
  }
  mouseLeave()
  {
    this.MyImages =null;
 
  }
 

}





