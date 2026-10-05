import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { BehaviourDropData, VisitBehaviour, AdmintDateDrop } from '../../../models/assesments.model';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { DomSanitizer } from '@angular/platform-browser';
import { Screens,Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NurseStation } from '../../../models/facility.model';
@Component({
  selector: 'app-behaviour',
  templateUrl: './behaviour.component.html',
  styleUrls: ['./behaviour.component.css']
})
export class BehaviourComponent implements OnInit {
  public residentID: number;
  private residentVisitId: number;
  public template;
  private visitBehaviorId: number = 0;
  behaviourDrop: BehaviourDropData[];
  behaviourObj: VisitBehaviour;
  allVisitBehaviour: any[]=[];
  errorMessage: string;
  public residents: ResidentDemographic[];
  public demographicInfoData: any={};
  public adminDateDrop: AdmintDateDrop[];
  myform: FormGroup;
  pageConfig = {};
  displayName:any;
  searchText:string="";
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
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Behavior");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.sharedService.currentPatientId.subscribe(patientId => this.residentID = patientId);
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getBehaviourDropData();
    //this.getResidentDropData();
    //this.getVisitBehaviourData(this.residentID);
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
      Initials: new FormControl(this.displayName),
      Date: new FormControl(new Date().toISOString().substring(0,10)),
      Hallucinations: new FormControl(''),
      Delusions: new FormControl(''),
      PhysicalBehaviour: new FormControl('', Validators.required),
      VerbalBehaviour: new FormControl('', Validators.required),
      OtherBehaviour: new FormControl('', Validators.required),
      ResidentReject: new FormControl('', Validators.required),
      Residentwandered: new FormControl('', Validators.required),
      Comments: new FormControl(''),
      status: new FormControl('1'),


    });
    this.userActivity();
    this.getUserRecentFacilityNurseStations();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.Behaviour,Activity.View,'')
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
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.selectednItems=[];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.allVisitBehaviour=[];
    this.adminDateDrop=[];
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
    this.allVisitBehaviour=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
  }
  onNurseStationSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.allVisitBehaviour=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
    this.getResidentDropData(item.NurseStation_Id);
  }
  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.allVisitBehaviour=[];
    this.adminDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
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
        this.getDemographicInfoData(this.residentID);
        this.getAdminDropData(this.residentID);
         this.getVisitBehaviourData(this.residentID);
        }
        else{
          this.alertService.warn("No data available");
          this.selectedResItem = [];
          this.residentID = 0;
          this.demographicInfoData = {};
          this.allVisitBehaviour = [];
          this.adminDateDrop = [];
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
          Date:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate),
        });
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getBehaviourDropData() {
    this.dataservice.get<any[]>(this.config.Emar_Assesments_GetVisitBehaviourDropData)
      .subscribe(res => {
        this.behaviourDrop = res;
        this.sharedService.changeCompany(this.behaviourDrop);
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  getAdminDropData(residentID: number) {
    this.dataservice.get<AdmintDateDrop[]>(this.config.Emar_Assesments_GetAdmitDateByID + residentID)
      .subscribe(res => {

        this.adminDateDrop = res;
        this.residentVisitId = this.adminDateDrop[0].PVisit_Id;

      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  insertVisitBehaviour() {

    this.behaviourObj = {
      VisitBehaviour_ID: this.visitBehaviorId,
      PVisit_Id: this.residentVisitId,
      HallucinationsID: (this.myform.value.Hallucinations == true ? 1 : 0),
      DelusionsID: (this.myform.value.Delusions == true ? 1 : 0),
      Physicalbehavioral: this.myform.value.PhysicalBehaviour,
      Verbalbehavioral: this.myform.value.VerbalBehaviour,
      Otherbehavioral: this.myform.value.OtherBehaviour,
      rejectevaluation: this.myform.value.ResidentReject,
      Resisdentwandered: this.myform.value.Residentwandered,
      Comments: this.myform.value.Comments,
      Initials: this.myform.value.Initials,
      Date: new Date().toISOString().substring(0,10),
      WeeklyStatus: (this.myform.value.status == true ? 1 : 0),
      VisitBehaviour_Status: 1,
      VisitBehaviour_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      VisitBehaviour_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Assesments_InsertVisitBehaviourData, this.behaviourObj)
      .subscribe(res => {

        this.alertService.success("Save successful");
        this.getVisitBehaviourData(this.residentID);
      }, error => {

        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
      });
    this.resetScreen();
  }
  resetScreen() {
    this.myform.reset();
    this.visitBehaviorId = 0;
    this.myform.patchValue({
      Date:new Date().toISOString().substring(0,10),
      Initials: this.displayName,
      // Hallucinations: '',
      // Delusions: '',
       PhysicalBehaviour: '',
       VerbalBehaviour: '',
       OtherBehaviour:'',
       ResidentReject:'',
       Residentwandered:'',
      // Comments:'',
      status: '1'

    });
  }
  getVisitBehaviourData(residentID: number) {
    
    this.dataservice.get<any[]>(this.config.Emar_Assesments_GetVisitBehaviourData+residentID)
      .subscribe(res => {
        this.allVisitBehaviour = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getVisitBehaviourById(ID: number,assDate:any) {
    let assesmentDate=new Date(assDate);
    let todayDate=new Date(this.nursingStationZoneCurrentDate);
    if(assesmentDate.setHours(0,0,0,0) == todayDate.setHours(0,0,0,0)){
      // Date equals today's date
    this.ng4LoadingSpinnerService.hide();
    this.dataservice.get<VisitBehaviour>(this.config.Emar_Assesments_GetVisitBehaviourByID + ID)
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
      this.alertService.error("Previous record cannot be updated");
     
     }
  }
  fetchData(res: VisitBehaviour) {
    this.visitBehaviorId = res.VisitBehaviour_ID;
    this.myform.patchValue({
      Initials: res.Initials,
      Date: new Date(res.Date).toISOString().substring(0,10),
      Hallucinations: res.HallucinationsID,
      Delusions: res.DelusionsID,
      PhysicalBehaviour: res.Physicalbehavioral,
      VerbalBehaviour: res.Verbalbehavioral,
      OtherBehaviour: res.Otherbehavioral,
      ResidentReject: res.rejectevaluation,
      Residentwandered: res.Resisdentwandered,
      Comments: res.Comments,
      status: res.WeeklyStatus,

    });
  }
  changeResident() {
    this.residentID== this.selectedResItem[0].Patient_Id;
    this.sharedService.changePatientId(this.residentID);
    this.resetScreen();
    this.getDemographicInfoData(this.residentID);
    this.getAdminDropData(this.residentID);
    this.getVisitBehaviourData(this.residentID);
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
      this.residentID = 0;
      this.demographicInfoData = {};
      this.allVisitBehaviour = [];
      this.adminDateDrop = [];
      this.resetScreen();
      this.nurseStationName = "";
      this.selectedResItem = [];
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
