import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Facility, NurseStation } from '../../../models/facility.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from './../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { of as observableOf, Observable, Subject } from 'rxjs';
import { catchError, switchMap, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DomSanitizer } from '@angular/platform-browser';
import { FiltersConfig } from '../../../models/facility.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-residentmerge',
  templateUrl: './residentmerge.component.html',
  styleUrls: ['./residentmerge.component.css'],
  providers: [DataService, APIConfiguration]
})
export class ResidentmergeComponent implements OnInit {

  public template;
  @Output() result: EventEmitter<any> = new EventEmitter();
  censusform: FormGroup;
  aliasForm:FormGroup;
  public userfacilitydrop: Facility[];
  dropdownSettings_Facilities: any = {};
  dropdownSettings_NurseStations: any = {};
  ShowFilter = true;
  public selectedfaItems = [];
  public selectednItems = [];
  public nurseStations: NurseStation[];
  userId: number;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  ResidentName: any;
  public residentslist:any[]=[];
  private searchResidentTerms = new Subject<string>();
  public residents: ResidentDemographic[];
  pageConfig = {};
  p: number = 1;
  searchText: string = "";
  gridPagination = this.config.gridPagination;
  totalRecords: number = 0;
  public filterConfigs: FiltersConfig;
  public residentTypeFlag:number=1;
  public warningMessage:string="";
  @Input() selectedResident: any;
  public residentId:number;
  public modalControlIsOpen: boolean = false;
  public mergePatientId:number;
  public mergeDetails:any;
  public residentDetails:any;
  MyImages: any;
  constructor(private sanitizer: DomSanitizer,private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,private sharedService: SharedService,private alertService: AlertService,public config: APIConfiguration,private dataservice: DataService,public activeDefaultModal: NgbActiveModal,private persistanceService: PersistanceService,private router: Router) { }

  ngOnInit() {
    debugger;
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.residentId = this.selectedResident;
    this.GetResidentMergeDetailsByPatientID(this.residentId);
    this.getUserRecentFacilityNurseStations();
    this.censusform = new FormGroup({
      FacilityId: new FormControl(''),
      NursingStationId: new FormControl(''),
    });
    this.aliasForm =new FormGroup({
      aliasName: new FormControl('',[Validators.maxLength(150)]),
      addresscheck:new FormControl(false),
      allergycheck:new FormControl(false),
      diagnosischeck:new FormControl(false)
    })
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
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: this.ShowFilter
    };
  }
  GetResidentMergeDetailsByPatientID(patientId:number)
  {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentMergeDetailsByPatientID+patientId)
      .subscribe(res => {
        debugger;
        this.residentDetails=res;
      }
        , error => {
          this.alertService.error(error.message)
        });
  }
  getUserRecentFacilityNurseStations() {
    debugger;
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          debugger;
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFacilityDrop(this.userId);
      }, error => {
        this.alertService.error(error.message);
      });
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  getFacilityDrop(userId: number): any {
    debugger;
    this.ng4LoadingSpinnerService.show();
    //let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
    this.dataservice.get<any>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.userfacilitydrop = res.Facilities;

        if (this.loginUserReceFacility != null) {
          if (this.userfacilitydrop.length > 0) {
            let checkFacExist = this.userfacilitydrop.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.censusform.patchValue({
              FacilityId: this.selectedfaItems,
            });
          }
        }

       else if (this.userfacilitydrop.length == 1) {
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.censusform.patchValue({
            FacilityId: this.userfacilitydrop,
          });
        }
        else
        {
        this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationByFacilityID(facilityId: any) {
    debugger;
    this.ng4LoadingSpinnerService.show();
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
            this.censusform.patchValue({
              NursingStationId: this.selectednItems,
            });
            this.getResidentList(1,'');
          }
          else if (this.userfacilitydrop.length == 1) {
            this.selectednItems.push(this.nurseStations[0]);
            this.censusform.patchValue({
              NursingStationId: this.selectednItems,
            });
            this.getResidentList(1,'');
          }
          else{
            this.ng4LoadingSpinnerService.hide();
          }
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
    this.nurseStations=[];
    this.selectednItems=[];
    this.residentslist=[];
    this.warningMessage="";
    this.getNurseStationByFacilityID(item.Facility_Id);
    this.loginUserReceNurseStation=undefined;
  }
  onFacilityDeSelect(item:any)
  {
    this.nurseStations=[];
    this.selectednItems=[];
    this.residentslist=[];
    this.warningMessage="Use filters to display resident list."
  }
  onNurseStationSelect(item:any)
  {
    this.getResidentList(1,'');
  }
  onNurseStationDeSelect(item:any)
  {
    this.residentslist=[];
    this.warningMessage="Use filters to display resident list.";
  }
  searchResidemts(term: string): void {
    debugger;
    // if (term.length > 1) {
      this.searchResidentTerms.next(term.replace(/[&\/\\#,+()$~%'":.*?<>{}\s]/g, '"'));
    //}
  }
  getResidentList(pageNumber: number, searchText: string)
  {
    this.ng4LoadingSpinnerService.show();
    this.p = pageNumber;
    this.filterConfigs = {
      Facilities: this.censusform.value.FacilityId.length != 0 ? this.censusform.value.FacilityId.map(item => item.Facility_Id) : [],
      NurseStations: this.censusform.value.NursingStationId.length != 0 ? this.censusform.value.NursingStationId.map(item => item.NurseStation_Id) : [],
      Floors:  [],
      Wings:[],
      Rooms:  [],
      Beds: [],
      ResidentType:1,
      CurrentPage: pageNumber,
      PageSize: this.gridPagination,
      SearchText:searchText!=undefined && searchText!=null?searchText.replace(new RegExp('/', 'g'), '-'):"",
      RecentFacNsFalg:0,
    }
    let formData: FormData = new FormData();
    this.dataservice.postFormData(this.config.Emar_ResidentDemographic_GetResidentsByCompanyBed, this.filterConfigs, formData)
    .subscribe(res => {
     this.ng4LoadingSpinnerService.hide();
     this.residentslist = res.Data.filter(fl=>fl.Patient_Id!=this.residentId);
     this.totalRecords = res.TotalRecords;
     this.warningMessage="";
     if (this.residentslist.length == 0)
     this.warningMessage="No data available.";
     //this.alertService.warn("No data available.");
    });
}
getFiltersDataBySelection(): any {
  this.searchText = "";
  this.ng4LoadingSpinnerService.show();
  if (this.censusform.value.FacilityId.length == 0 || this.censusform.value.NursingStationId.length == 0) {
    this.residentslist = [];
    this.alertService.error("Use filters to display resident list.")
    this.ng4LoadingSpinnerService.hide();
  }
  else {
    this.getResidentList(this.residentTypeFlag, '');
  }
}
onSearchChange(searchValue: string): void {
  if (searchValue.length >= 3) {
   this.getResidentList(1, searchValue);
  }
  else if (searchValue.length == 0) {
   this.getResidentList(this.p, '');
  }
}
aliasNameOpen(patientId:number)
{
this.mergePatientId =patientId;
debugger;
  this.ng4LoadingSpinnerService.show();
  this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetMergeDetails + this.residentId + "/" + this.mergePatientId)
      .subscribe(res => {
        debugger;
        this.mergeDetails=res;
        this.modalControlIsOpen =true;
        this.ng4LoadingSpinnerService.hide();
      }
        , error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message)
        });
}
saveMergeResident()
{
  debugger;
  this.ng4LoadingSpinnerService.show();
  let objMerge={
    patientId:this.residentId,
    mergepatientId:this.mergePatientId,
    aliasName:this.aliasForm.value.aliasName,
    DetailsUpdate:this.aliasForm.value.addresscheck==true?1:0,
    AllergyMerge:this.aliasForm.value.addresscheck==true?this.mergeDetails.Allergy1.length>0?1:0: this.mergeDetails.Allergy2.length>0?1:0,
    DiagnosisMerge:this.aliasForm.value.addresscheck==true?this.mergeDetails.Diagnosis1.length>0?1:0:this.mergeDetails.Diagnosis2.length>0?1:0,
    // AllergyMerge:this.aliasForm.value.allergycheck==true?1:0,
    // DiagnosisMerge:this.aliasForm.value.diagnosischeck==true?1:0
  }
  this.dataservice.post(this.config.Emar_ResidentDemographic_InsertMergeStatus,objMerge)
      .subscribe(res => {
        debugger;
        this.alertService.success("Residents merged successfully")
        this.ng4LoadingSpinnerService.hide();
        let outPatientId=this.aliasForm.value.addresscheck==true?this.mergePatientId:this.residentId
        this.sharedService.changePatientId(outPatientId);
        this.result.emit(2);
        this.modalControlIsOpen =false;
    //     this.router.routeReuseStrategy.shouldReuseRoute = function(){return false;};
    //     let url=this.router.url+'?';
    //     this.router.navigateByUrl(url)
    // .then(() => {
    //   this.router.navigated = false;
    //   this.router.navigate([this.router.url]);
    // });
      }
        , error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message)
        });
}
closeModel()
{
  this.modalControlIsOpen =false;
}
mouseEnter(Id:any)
{
  
  this.MyImages = Id;
 
}
mouseLeave()
{
  this.MyImages =null;

}
closeresidentModel()
{
  debugger;
this.activeDefaultModal.close("Close click");
this.result.emit(2);
}
Reset()
{
  this.aliasForm.reset();
}
}