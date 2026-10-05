import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { PhysicianDetails } from '../../../models/orders.model';
import { Foodintake, AdmintDateDrop } from '../../../models/assesments.model';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Screens,Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NurseStation } from '../../../models/facility.model';
@Component({
  selector: 'app-fluidintake',
  templateUrl: './fluidintake.component.html',
  styleUrls: ['./fluidintake.component.css']
})
export class FluidintakeComponent implements OnInit {
  public residentID: number;
  private residentVisitId: number;
  myform: FormGroup;
  public template;
  foodInObj: Foodintake;
  errorMessage: string;
  VisitFoodinId: number = 0;
  VisitFoodinData: any[]=[];
  public physiciansdrop: PhysicianDetails[];
  public residents: ResidentDemographic[];
  public demographicInfoData: any={};
  public adminDateDrop: AdmintDateDrop[];
  pageConfig = {};
  displayName:string;
  searchText:string="";
  gridPagination = this.config.gridPagination;
  p: number = 1;
  public selectedResItem=[];
  public dropdownSettings_Resident: any = {};
  public dropdownSettings_Physician:any={};
  public selectedPhtsicianItem:any[]=[];
  public nurseStationName:string;
  public saveDisable=false;
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
  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private alertService: AlertService, private persistanceService: PersistanceService, private sharedService: SharedService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("FoodIntake");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.sharedService.currentPatientId.subscribe(patientId => this.residentID = patientId);
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    //this.GetPhysicianDropData();
    //this.getResidentDropData();
    this.displayName = this.persistanceService.get('displayname');

      if(this.displayName!=null)
      {
      this.displayName=this.displayName.substring(1,this.displayName.length-1);
      }
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

      AssessmentDate: new FormControl(new Date().toISOString().substring(0,10)),
      Initials: new FormControl(this.displayName),
      Attendingphysician:new FormControl('' ,Validators.required),
      Fluidsb: new FormControl('',[Validators.maxLength(50),Validators.pattern(this.config.alphaNumeric)]),
      Alternateb: new FormControl('',Validators.maxLength(50)),
      supplementb: new FormControl('',Validators.maxLength(50)),
      Fluidsl: new FormControl('',[Validators.maxLength(50),Validators.pattern(this.config.alphaNumeric)]),
      Alternatel: new FormControl('',Validators.maxLength(50)),
      supplementl: new FormControl('',Validators.maxLength(50)),
      Fluidss: new FormControl('',[Validators.maxLength(50),Validators.pattern(this.config.alphaNumeric)]),
      Alternates: new FormControl('',Validators.maxLength(50)),
      supplements: new FormControl('',Validators.maxLength(50)),
      status: new FormControl('1'),
    });
    this.dropdownSettings_Physician= {
      singleSelection: true,
      idField: "PhysicianNPI",
      textField: "PhysicianFullName",
      text: "Select",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true,
    };
    this.userActivity();
    this.getUserRecentFacilityNurseStations();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.FoodIntake,Activity.View,'')
    .subscribe(res=>{},error=>{
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
        this.residents = res;
        this.selectedResItem=[];
        this.ng4LoadingSpinnerService.hide();
        if (res!=undefined && res != null && res.length>0) {
          
        this.residentID = this.residents[0].Patient_Id;
        this.selectedResItem= this.residents.filter(r=>r.Patient_Id==this.residentID);
        this.getAdminDropData(this.residentID);
        this.getDemographicInfoData(this.residentID);
        this.getVisitFoodIntakeData(this.residentID);
        this.GetPhysicianDropData(stationId);
        }
        else{
          this.alertService.warn("No data available");
          this.residents = [];
          this.selectedResItem = [];
          this.residentID = 0;
          this.demographicInfoData = {};
          this.VisitFoodinData = [];
          this.adminDateDrop = [];
          this.resetScreen();
          this.nurseStationName = "";
          this.selectedPhtsicianItem = [];
          this.physiciansdrop = [];
          this.ng4LoadingSpinnerService.hide();
        }
      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getDemographicInfoData(residentID: number) {

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
          AssessmentDate:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate),
        });
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getAdminDropData(residentID: number) {
    this.dataservice.get<AdmintDateDrop[]>(this.config.Emar_Assesments_GetAdmitDateByID + residentID)
      .subscribe(res => {

        this.adminDateDrop = res;
        this.residentVisitId = this.adminDateDrop[0].PVisit_Id;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  GetPhysicianDropData(nsId: number) {
    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + nsId)
      .subscribe(res => {
        this.physiciansdrop = res;
      }, error => {
        this.errorMessage = <any>error;
      });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.selectednItems=[];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.VisitFoodinData=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
    this.selectedPhtsicianItem=[];
    this.physiciansdrop=[];
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
    this.VisitFoodinData=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.selectedPhtsicianItem=[];
    this.physiciansdrop=[];
    this.nurseStationName="";
  }
  onNurseStationSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.VisitFoodinData=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
    this.selectedPhtsicianItem=[];
    this.physiciansdrop=[];
    this.getResidentDropData(item.NurseStation_Id);
  }
  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.VisitFoodinData=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.selectedPhtsicianItem=[];
    this.physiciansdrop=[];
    this.nurseStationName="";
  }
  insertVisitFoodIn() {
    this.saveDisable=true;
    if((this.myform.value.Fluidsb=='' && this.myform.value.Fluidsl=='' && this.myform.value.Fluidss=='')|| this.myform.value.Fluidsb==null && this.myform.value.Fluidsl==null && this.myform.value.Fluidss==null){
    window.scroll(0, 0);
    this.alertService.error("Please enter food intake");
    this.saveDisable=false;
    }
    else{
      
    this.foodInObj = {
      VisitFoodintake_ID: this.VisitFoodinId,
      PVisit_Id: this.residentVisitId,
      AssessmentDate: new Date().toISOString().substring(0,10),
      Initials: this.myform.value.Initials,
      Attendingphysician:this.myform.value.Attendingphysician[0].PhysicianNPI,
      Fluidsb: this.myform.value.Fluidsb,
      Alternateb: this.myform.value.Alternateb,
      supplementb: this.myform.value.supplementb,
      Fluidsl: this.myform.value.Fluidsl,
      Alternatel: this.myform.value.Alternatel,
      supplementl: this.myform.value.supplementl,
      Fluidss: this.myform.value.Fluidss,
      Alternates: this.myform.value.Alternates,
      supplements: this.myform.value.supplements,
      VisitFoodintake_Status: (this.myform.value.status == true ? 1 : 0),
      VisitFoodintake_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      VisitFoodintake_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Assesments_insertUpdateVisitFoodIntake, this.foodInObj)
      .subscribe(res => {

        this.getVisitFoodIntakeData(this.residentID);
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
    this.VisitFoodinId = 0;
    this.myform.patchValue({
      AssessmentDate:new Date().toISOString().substring(0,10),
      Initials: this.displayName,
      // Attendingphysician: '',
      // Fluidsb: '',
      // Alternateb: '',
      // supplementb: '',
      // Fluidsl:'',
      // Alternatel:'',
      // supplementl:'',
      // Fluidss:'',
      // Alternates:'',
      // supplements:'',
      status: '1'

    });
   
    
  }
  getVisitFoodIntakeData(residentID: number) {
    
    this.dataservice.get<any[]>(this.config.Emar_Assesments_GetVisitFoodIntakeList+residentID)
      .subscribe(res => {
        
        this.VisitFoodinData = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getVisitFoodIntakeById(ID: number,assDate:any) {
    
     var assesmentDate=new Date(assDate);
     var todayDate=new Date(this.nursingStationZoneCurrentDate);
    if(assesmentDate.setHours(0,0,0,0) == todayDate.setHours(0,0,0,0)) {
      // Date equals today's date
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<Foodintake>(this.config.Emar_Assesments_GetVisitFoodIntakeByID + ID)
      .subscribe(res => {

        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {

        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
        this.ng4LoadingSpinnerService.hide();
      });
      window.scroll(0, 0);
    }
   else{
    window.scroll(0, 0);
    this.resetScreen();
    this.alertService.error("Unable to update previous record");
   
   }
  }
  fetchData(res: Foodintake) {
    this.selectedPhtsicianItem=[];
    this.GetPhysicianDropData(this.selectednItems[0].NurseStation_Id);
    let record=this.physiciansdrop.filter(p=>p.PhysicianNPI.toLowerCase()==res.Attendingphysician.toLowerCase());
    if(record!=undefined && record !=null){
    this.selectedPhtsicianItem.push(record[0]);
    }
    this.VisitFoodinId = res.VisitFoodintake_ID;

    this.myform.patchValue({
      
      AssessmentDate:this.dateFormatPipe.transformISODate(res.AssessmentDate) ,//new Date(res.AssessmentDate).toISOString().substring(0,10),
      Initials: res.Initials,
      Attendingphysician: this.selectedPhtsicianItem,
      Fluidsb: res.Fluidsb,
      Alternateb: res.Alternateb,
      supplementb: res.supplementb,
      Fluidsl: res.Fluidsl,
      Alternatel: res.Alternatel,
      supplementl: res.supplementl,
      Fluidss: res.Fluidss,
      Alternates: res.Alternates,
      supplements: res.supplements,
      status: res.VisitFoodintake_Status,
    });
  }

  changeResident() {
    this.residentID== this.selectedResItem[0].Patient_Id;
    this.sharedService.changePatientId(this.residentID);
    this.resetScreen();
    this.getDemographicInfoData(this.residentID);
    this.getAdminDropData(this.residentID);
    this.getVisitFoodIntakeData(this.residentID);
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
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
    this.VisitFoodinData=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
    this.selectedResItem=[];
    this.physiciansdrop=[];
    this.selectedPhtsicianItem=[];
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
