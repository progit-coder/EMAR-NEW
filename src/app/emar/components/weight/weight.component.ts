import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators, RequiredValidator } from '@angular/forms';
import { WeightMaster } from '../../../models/assesments.model';
import { Company } from '../../../models/company.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { SharedService } from '../../../services/shared/shared.service';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { DomSanitizer } from '@angular/platform-browser';
import { Screens,Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NurseStation } from '../../../models/facility.model';
@Component({
  selector: 'app-weight',
  templateUrl: './weight.component.html',
  styleUrls: ['./weight.component.css']
})
export class WeightComponent implements OnInit {
  public template;
  private url: string;
  errorMessage: string;
  success: string
  myform: FormGroup;
  private WeightLog_ID: number = 0;
  private WeightLog_Status: number = 1;
  public WeightMaster: WeightMaster[];
  weightObj: WeightMaster;
  public weightList: any[]=[];
  public residents: ResidentDemographic[];
  public residentID: number;
  public demographicInfoData: any={};
  public dateArrival: any;
  pageConfig = {};
  Patient_Id: number;
  displayName: string;
  searchText:string="";
  type:any;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  public selectedResItem=[];
  public dropdownSettings_Resident: any = {};
  public nurseStationName:string;
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
  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private alertService: AlertService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, private sharedService: SharedService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.template=this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.displayName = this.persistanceService.get('displayname');

    if (this.displayName != null)

      this.displayName = this.displayName.substring(1, this.displayName.length - 1);

    this.pageConfig = this.persistanceService.getPermissionsByScreen("WeightLog");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.sharedService.currentPatientId.subscribe(patientId => this.residentID = patientId);
    this.template = this.dataservice.template;
    // this.ng4LoadingSpinnerService.show();
    //this.getWeightList(Patient_Id);
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
      weight: new FormControl('', [Validators.pattern(this.config.decimalAllowTwoDigits),Validators.maxLength(6)]),
      // lb: new FormControl('',),
      // kg: new FormControl('',),
      date: new FormControl(new Date().toISOString().substring(0,10)),
       height: new FormControl('',[Validators.pattern(this.config.numeric),Validators.maxLength(1)]),
      //height: new FormControl('',Validators.maxLength(5)),
     // feet: new FormControl('',[Validators.pattern('/\b([0-9]|1[0-1])\b/g'),Validators.maxLength(2)]),
    //  ^([0-9]|1[01])$
     feet: new FormControl('',[Validators.pattern('^([0-9]|1[01])$'),Validators.maxLength(2)]),
      initials: new FormControl(this.displayName),
      ibw: new FormControl('', [Validators.pattern('^(?=.*[1-9])[0-9]+$'),Validators.maxLength(6)]),
      // preDialysis: new FormControl(false, ),
      // postDialysis: new FormControl(false, ),
      dialysis: new FormControl('1'),
      remarks: new FormControl('',Validators.maxLength(1000)),
      type: new FormControl('1')
    });
    this.getUserRecentFacilityNurseStations();
    //this.getResidentDropData();
    this.userActivity();
  }
}

else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {
    this.sharedService.insertUserActivityDetails(Screens.WeightLog,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  resetScreen() {
    this.myform.reset();
    this.myform.patchValue({
      date:new Date().toISOString().substring(0,10),
      initials:this.displayName,
      type:'1',
      dialysis:'1',

    });
      this.WeightLog_ID = 0;
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
  getNurseStationNameByPatientId(residentId:number)
  {

    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetNurseStationName + residentId)
      .subscribe(res => {
         this.nurseStationName= res;
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
        this.ng4LoadingSpinnerService.hide();
        this.residents = res;
        this.selectedResItem=[];
        if (res!=undefined && res != null && res.length>0) {
        this.residentID = this.residents[0].Patient_Id;
        this.selectedResItem= this.residents.filter(r=>r.Patient_Id==this.residentID);
        this.getDemographicInfoData(this.residentID);
        this.getWeightList(this.residentID);
        }
        else{
          this.alertService.warn("No data available");
          this.selectedResItem = [];
          this.residentID = 0;
          this.demographicInfoData = {};
          this.weightList = [];
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
    this.weightList=[];
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
    this.weightList=[];
    this.resetScreen();
    this.nurseStationName="";
  }
  onNurseStationSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.weightList=[];
    this.resetScreen();
    this.nurseStationName="";
    this.getResidentDropData(item.NurseStation_Id);
  }
  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.weightList=[];
    this.resetScreen();
    this.nurseStationName="";
  }
  changeResident() {
    this.ng4LoadingSpinnerService.show();
    this.residentID== this.selectedResItem[0].Patient_Id;
    this.sharedService.changePatientId(this.residentID);
    this.resetScreen();
    this.getDemographicInfoData(this.residentID);
    this.getWeightList(this.residentID);
  }

  getDemographicInfoData(residentID: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + residentID)
      .subscribe(res => {

        this.demographicInfoData = res;
        this.getNursingStationTimeZone(res.NursingStationId);
        this.getNurseStationNameByPatientId(residentID);
        this.ng4LoadingSpinnerService.hide();
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

  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }



  insertWeight() {
    debugger;
    if((this.myform.value.weight==undefined || this.myform.value.weight==null || this.myform.value.weight=="")&&
    (this.myform.value.height==undefined || this.myform.value.height==null || this.myform.value.height=="")&&
    (this.myform.value.feet==undefined || this.myform.value.feet==null || this.myform.value.feet=="")&&
    (this.myform.value.ibw==undefined || this.myform.value.ibw==null || this.myform.value.ibw=="")&&
    (this.myform.value.remarks==undefined || this.myform.value.remarks==null || this.myform.value.remarks=="")
    ){
       this.alertService.warn("At least one detail must be entered");
       this.ng4LoadingSpinnerService.hide();
    }
    else
    {
    this.weightObj = {
      WeightLog_ID: this.WeightLog_ID,
      Patient_Id: this.residentID,
      Weight: this.myform.value.weight,
      Type: this.myform.value.type == true ? '2' : '1',
      DateTime: this.myform.value.date,
      HeightFeet: this.myform.value.height,
      HeightInc: this.myform.value.feet,
      initials: this.myform.value.initials,
      IBW: this.myform.value.ibw,
      PreDialysis: this.myform.value.dialysis == true ? 1 : 0,
      PostDialysis: this.myform.value.dialysis == false ? 1 : 0,
      Remarks: this.myform.value.remarks,
      WeightLog_Status: this.WeightLog_Status,
      WeightLog_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      WeightLog_CreatedOn: new Date().toISOString()
    };

    this.dataservice.post(this.config.Weight_InsertUpdateWeight, this.weightObj)
      .subscribe(res => {
        this.getWeightList(this.residentID);
        this.alertService.success("Save successful");
        this.resetScreen();
        //console.log(this.getWeightList(this.residentID));
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
        });
      }

  }

  getWeightList(residentID: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Weight_GetWeightList + residentID)
      .subscribe(res => {
        this.weightList = res;
        console.log(this.weightList ,"weight details");
        this.ng4LoadingSpinnerService.hide();

         // this.sharedService.updateUsers(res);

      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }

  getWeightDetailsByID(WeightLog_ID: number) {
    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    this.url = this.config.Weight_GetWeightDetailsByID + WeightLog_ID;
    this.dataservice.get<WeightMaster>(this.url).subscribe(
      res =>{
        this.fetchDetails(res);
        this.ng4LoadingSpinnerService.hide();
      },

      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      }
    );
  }
  fetchDetails(res: WeightMaster) {
    this.WeightLog_ID = res.WeightLog_ID;
    this.myform.patchValue({
      weight: res.Weight,
      ibw: res.IBW,
      preDialysis: res.PreDialysis,
      postDialysis: res.PostDialysis,
      type: res.Type,
      date: res.DateTime,
      height: res.HeightFeet,
      feet: res.HeightInc,
      intials: res.initials,
      remarks: res.Remarks
    });
  }
  onResidentSelect(item: any) {

  this.residentID=item.Patient_Id;
   this.selectedResItem= this.residents.filter(r=>r.Patient_Id==item.Patient_Id);
    this.changeResident();
  }
  onResidentDeSelect(item: any) {
    //this.changeResident();
    this.residentID=0;
    this.demographicInfoData={};
    this.weightList=[];
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
