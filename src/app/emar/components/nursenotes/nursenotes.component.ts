
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services/index';
import { NurseNote,AdmintDateDrop } from '../../../models/assesments.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { DomSanitizer } from '@angular/platform-browser';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens,Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NurseStation } from '../../../models/facility.model';
@Component({
  selector: 'app-nursenotes',
  templateUrl: './nursenotes.component.html',
  styleUrls: ['./nursenotes.component.css']
})
export class NursenotesComponent implements OnInit {
  public template;
  public dateArrival: any;
  visitNurseNoteId: number = 0
  public residentID: number;
  private residentVisitId: number;
  public residents: ResidentDemographic[];
  public demographicInfoData: any={};
  myform: FormGroup;
  nurseNoteObj: NurseNote;
  public nurseNoteDetails: any[]=[];
  errorMessage: string;
  admitDateDrop: AdmintDateDrop[];
  displayName:string;
  pageConfig = {};
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
  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sanitizer: DomSanitizer, private sharedService: SharedService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("NurseNotes");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
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
      nursename: new FormControl(this.displayName),
      date:  new FormControl(new Date().toISOString().substring(0,10)),
      description: new FormControl('',[Validators.required,Validators.maxLength(1000)])
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
    this.sharedService.insertUserActivityDetails(Screens.NurseNotes,Activity.View,'')
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
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.selectednItems=[];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.nurseNoteDetails=[];
    this.admitDateDrop=[];
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
    this.nurseNoteDetails=[];
    this.admitDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
  }
  onNurseStationSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.nurseNoteDetails=[];
    this.admitDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
    this.getResidentDropData(item.NurseStation_Id);
  }
  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.selectedResItem=[];
    this.residentID=0;
    this.demographicInfoData={};
    this.nurseNoteDetails=[];
    this.admitDateDrop=[];
    this.resetScreen();
    this.nurseStationName="";
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
        this.getAdmitDropData(this.residentID);
        this.getNurseNoteDetails(this.residentID);
        }
        else{
          this.alertService.warn("No data available");
          this.selectedResItem = [];
          this.residentID = 0;
          this.demographicInfoData = {};
          this.nurseNoteDetails = [];
          this.admitDateDrop = [];
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
          this.errorMessage = <any> error;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNursingStationTimeZone(stationId: number) {
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
         
        this.nursingStationZoneCurrentDate=res;
        this.myform.patchValue({
          date:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate),
        });
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getAdmitDropData(residentID: number) {
    this.dataservice.get<AdmintDateDrop[]>(this.config.Emar_Assesments_GetAdmitDateByID + residentID)
      .subscribe(res => {
        this.admitDateDrop = res;
        this.residentVisitId = this.admitDateDrop[0].PVisit_Id;

      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
        })
  }

  inserNurseNote() {
    
    this.nurseNoteObj = {
      VisitNursingNotes_ID: this.visitNurseNoteId,
      PVisit_Id: this.residentVisitId,
      NoteDate: new Date().toISOString().substring(0,10),
      NoteTime: null,
      NurseName: this.myform.value.nursename,
      Notes: this.myform.value.description,
      PDAID: null,
      VisitNursingNotes_Status: 1,
      VisitNursingNotes_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      VisitNursingNotes_CreatedOn: new Date().toISOString(),
    }
    this.dataservice.post(this.config.Emar_Assessments_InsertUpdateVisitNurseNote, this.nurseNoteObj)
      .subscribe(res => {
        this.getNurseNoteDetails(this.residentID);
        this.alertService.success("Save successful");

        this.resetScreen();
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
      });
  }
  resetScreen() {
    this.myform.reset();
    this.visitNurseNoteId=0;
    this.myform.patchValue({
      date:new Date().toISOString().substring(0,10),
      nursename: this.displayName,
      

    });
  }
  getNurseNoteDetails(residentID: number) {
    this.dataservice.get<any[]>(this.config.Emar_Assessments_GetVisitNurseNote+residentID)
      .subscribe(res => {
        this.nurseNoteDetails = res;

        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getVisitNurseNoteDetailsByID(VisitNursingNotes_ID: number,NoteDate:any) {
    let nurseNoteDate=new Date(NoteDate);
    let todayDate=new Date(this.nursingStationZoneCurrentDate);
    if(nurseNoteDate.setHours(0,0,0,0) == todayDate.setHours(0,0,0,0)){
       // Date equals today's date
    this.dataservice.get<NurseNote>(this.config.Emar_Assessments_GetVisitNurseNoteDetailsByID + VisitNursingNotes_ID)
      .subscribe(res => {
        this.fetchData(res);
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
      });
    window.scroll(0, 0);
    }
    else
    {
      window.scroll(0, 0);
      this.resetScreen();
      this.alertService.error("Previous record cannot be updated");
     
    }
  }

  fetchData(res: NurseNote) {
    this.visitNurseNoteId = res.VisitNursingNotes_ID;
    this.myform.patchValue({
      nursename: res.NurseName,
      date:this.dateFormatPipe.transformISODate(res.NoteDate), //new Date(res.NoteDate).toISOString().substring(0,10),
      description: res.Notes

    });
  }
  changeResident() {
    this.residentID== this.selectedResItem[0].Patient_Id;
    this.sharedService.changePatientId(this.residentID);
    this.resetScreen();
    this.getDemographicInfoData(this.residentID);
    this.getNurseNoteDetails(this.residentID);
    this.getAdmitDropData(this.residentID);
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
    this.nurseNoteDetails=[];
    this.admitDateDrop=[];
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
