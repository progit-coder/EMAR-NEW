import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';

import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ResidentDemographic, ResidentsCount } from '../../../models/residentdemographic.model';
import { Router } from '@angular/router';
import { Floor, NurseStation, Wing, Bed, Room, FiltersConfig } from '../../../models/facility.model';
import { FormGroup, FormControl } from '@angular/forms';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { DomSanitizer
 } from '@angular/platform-browser';
import { Screens, Activity } from '../../../models/useractivity.model';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { DemographicinfoComponent } from '../demographicinfo/demographicinfo.component';
import { DemographicInfo, ResidentDemographicMaster } from '../../../models/residentdemographic.model';
@Component({
  selector: 'app-residentgrid',
  templateUrl: './residentgrid.component.html',
  styleUrls: ['./residentgrid.component.css'],
  providers: [DataService, APIConfiguration]
})
export class ResidentgridComponent implements OnInit {
  public form: FormGroup;
  unsubcribe: any
  public template;
  modalOption: NgbModalOptions = {};
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;
  @Output() newResidentId: EventEmitter<any> = new EventEmitter();
  NewResidentpageConfig = {};
  resident: ResidentDemographic[] = [];
  public residentsCount: ResidentsCount;
  public filterData: any = [];
  public floors: Floor[];
  public nurseStations: NurseStation[];
  public wings: Wing[];
  public rooms: Room[];
  public beds: Bed[];
  public facilities: any[];
  public filterConfigs: FiltersConfig;
  public ResFacfilterData: any;
  public ResNsfilterData: any;
  public ResFloorfilterData: any;
  public ResWingfilterData: any;
  public ResRoomfilterData: any;
  public ResBedfilterData: any;
  disabled = false;
  ShowFilter = true;
  public MyImages:any;
  limitSelection = false;
  myform: FormGroup;
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Rooms: any = {};
  dropdownSettings_Beds: any = {};
  dropdownSettings_Facilities: any = {};
  public selectedfaItems = [];
  public selectednItems = [];
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  arfloor = []; arfacility = []; arnstation = []; arwing = []; arroom = []; arbed = [];
  patientId: number;
  public residentsCount_Admit: number;
  public residentsCount_Readmit: number;
  public residentsCount_Discharge: number;
  public residentsCount_Inactive: number;
  public residentsCount_Transfer: number;
  pageConfig = {};
  p: number = 1;
  searchText: string = "";
  gridPagination = this.config.gridPagination;
  userId: number;
  public companyToBed: number = 0;
  public nursestationid: string = "";
  totalRecords: number = 0;
  public nstations: string = "";
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public residentTypeFlag: number = 1;
  public fields: any[];
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService,
    private route: Router, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private sanitizer: DomSanitizer, private modalService: NgbModal) {
  }
  ngAfterViewInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ResidentGrid");
  }

  ngOnInit() {
    window.scroll(0,0);
    this.NewResidentpageConfig = this.persistanceService.getPermissionsByScreen("NewResident");
    if (this.NewResidentpageConfig == undefined) {
      this.NewResidentpageConfig = 0;
    }
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ResidentGrid");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
        //  ddlfloors: new FormControl(''),
          ddlnursestations: new FormControl(''),
         // ddlwings: new FormControl(''),
         // ddlrooms: new FormControl(''),
        //  ddlbeds: new FormControl(''),
        });
        this.ResFacfilterData = [];
        this.ResNsfilterData = [];
        this.ResFloorfilterData = [];
        this.ResWingfilterData = [];
        this.ResRoomfilterData = [];
        this.ResBedfilterData = [];
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
       // this.getCompanyToBedData(1);
        this.form = new FormGroup({
          fields: new FormControl(JSON.stringify(this.fields))
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
        // this.dropdownSettings_Floors = {
        //   singleSelection: false,
        //   idField: "Floor_Id",
        //   textField: "Floor_Name",
        //   text: "Floors",
        //   selectAllText: "Select All",
        //   unSelectAllText: "UnSelect All",
        //   itemsShowLimit: 1,
        //   allowSearchFilter: this.ShowFilter
        // };
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

        // this.dropdownSettings_Wings = {
        //   singleSelection: false,
        //   idField: "Wing_Id",
        //   textField: "Wing_Desc",
        //   text: "Wings",
        //   selectAllText: "Select All",
        //   unSelectAllText: "UnSelect All",
        //   itemsShowLimit: 1,
        //   allowSearchFilter: this.ShowFilter
        // };
        // this.dropdownSettings_Rooms = {
        //   singleSelection: false,
        //   idField: "Room_Id",
        //   textField: "Room_Name",
        //   text: "Rooms",
        //   selectAllText: "Select All",
        //   unSelectAllText: "UnSelect All",
        //   itemsShowLimit: 1,
        //   allowSearchFilter: this.ShowFilter
        // };
        // this.dropdownSettings_Beds = {
        //   singleSelection: false,
        //   idField: "Bed_Id",
        //   textField: "Bed_Name",
        //   text: "Beds",
        //   selectAllText: "Select All",
        //   unSelectAllText: "UnSelect All",
        //   itemsShowLimit: 1,
        //   allowSearchFilter: this.ShowFilter
        // };
        this.userActivity();
        this.getUserRecentFacilityNurseStations();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ResidentGrid, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getUserRecentFacilityNurseStations() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceFacility = res.Facility_Id;
          this.sharedService.changeFacilityId(this.loginUserReceFacility);

          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFiltersData(this.userId);
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getCompanyToBedData(facilityId: number) {
    //let backClick = JSON.parse(localStorage.getItem("BackClick"));
    this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + this.userId + "/" + facilityId)
      .subscribe(res => {
        if (res.companyBedFlag == 1) {
          this.companyToBed = res.companyBedFlag;
          this.floors = res.Floors;
          this.wings = res.Wings;
          this.rooms = res.Rooms;
          this.beds = res.Beds;
        }
        
        else if (res.companyBedFlag == 0) {
          this.companyToBed = 0;
          this.floors = [];
          this.wings = [];
          this.rooms = [];
          this.beds = [];
        }
        // this.test();

        // if (this.facilities.length == 1 && backClick != true) {
        //   this.myform.patchValue({
        //     ddlfloors: this.floors,
        //     ddlwings: this.wings,
        //     ddlrooms: this.rooms,
        //     ddlbeds: this.beds,
        //   });
        // }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
  onRowSelect(patientId: any) {
    // localStorage.setItem("ResdentGridFacility", JSON.stringify(this.filterConfigs.Facilities));
    // localStorage.setItem("ResdentGridNurseStations", JSON.stringify(this.filterConfigs.NurseStations));
    // localStorage.setItem("ResdentGridFloors", JSON.stringify(this.filterConfigs.Floors));
    // localStorage.setItem("ResdentGridWings", JSON.stringify(this.filterConfigs.Wings));
    // localStorage.setItem("ResdentGridRooms", JSON.stringify(this.filterConfigs.Rooms));
    localStorage.setItem("ResdentGridResType", JSON.stringify(this.residentTypeFlag));
    this.sharedService.changePatientId(patientId);
    this.route.navigate(['/home/residentinformation'])
  }
  getNurseStationHierarchyDetailsbyNsId(nsId:any)
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyBedMapping_GetNurseStationHierarchyDetailsbyNsId + nsId)
      .subscribe((res: any) => {
    
            if(res == null && this.companyToBed !=0)
            {
              this.floorIndex =1;
              this.wingIndex =2;
              this.roomIndex =3;
              this.bedIndex =4;  
            }
            else if(res != null && this.companyToBed!=0 ){
            this.floorIndex =res.FloorPrior;
            this.wingIndex =res.WingPrior;
            this.roomIndex =res.RoomPrior;
            this.bedIndex =res.BedPrior;
           }
           else if((res == null && this.companyToBed ==0) || this.companyToBed==0)
           {
            this.floorIndex =0;
            this.wingIndex =0;
            this.roomIndex =0;
            this.bedIndex =0;  
           }
            this.fields = [
              {
                label: 'Floor',
                name: 'ddlfloors',
                data: this.floors,
                settings: {
                  singleSelection: false,
                  idField: "Floor_Id",
                  textField: "Floor_Name",
                  text: "Floors",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedflItems,
                placeholder: 'Floors',
                index:this.floorIndex
              },
              {
                label: 'Wing',
                name: 'ddlwings',
                data: this.wings,
                settings: {
                  singleSelection: false,
                  idField: "Wing_Id",
                  textField: "Wing_Desc",
                  text: "Wings",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedwItems,
                placeholder: 'Wings',
                index:this.wingIndex
              },
              {
                label: 'Room',
                name: 'ddlrooms',
                data: this.rooms,
                settings: {
                  singleSelection: false,
                  idField: "Room_Id",
                  textField: "Room_Name",
                  text: 'Rooms',
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedflItems,
                placeholder: 'Rooms',
                index:this.roomIndex
              },
              {
                label: 'Bed',
                name: 'ddlbeds',
                data: this.beds,
                settings: {
                  singleSelection: false,
                  idField: "Bed_Id",
                  textField: "Bed_Name",
                  text: "Beds",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedbItems,
                placeholder: 'Beds',
                index:this.bedIndex
              }
            ];
       
             let fieldsCtrls = {};
             for (let f of this.fields) {
                 fieldsCtrls[f.name] = new FormControl(f.value)
             }
            this.form = new FormGroup(fieldsCtrls);
            this.dataFields=this.fields.filter(s=>s.index != 0); 
            this.dataFields.sort((a, b) => {
              if(a.index > b.index) {
                return 1;
              } else if(a.index < b.index) {
                return -1;
              } else {
                return 0;
              }
              
            });
            this.fields =this.dataFields;
          
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getFiltersData(userId: number) {
    this.ng4LoadingSpinnerService.show();
    //let backClick = JSON.parse(localStorage.getItem("BackClick"));
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.ng4LoadingSpinnerService.hide();
        this.facilities = res.Facilities;
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacility: this.selectedfaItems,
            });
          }
        } else if (res.Facilities.length == 1) {
         // this.getCompanyToBedData(res.Facilities[0].Facility_Id);
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.myform.patchValue({
            ddlfacilities: this.facilities,
          });
        }
        // if (backClick == true) {
        //   this.getBackClickFilterData();
        // }
        // else if (this.facilities.length > 1) {
        //   let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
        //   this.selectedfaItems = [];
        //   if (checkFacExist != undefined) {
        //     this.selectedfaItems.push(checkFacExist);
        //     this.getNurseStationByFacilityID(this.loginUserReceFacility);
        //   }
        //   this.myform.patchValue({
        //     ddlfacility: this.selectedfaItems,
        //   });
        // }
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getFiltersDataBySelection() {
    this.searchText = "";
    this.ng4LoadingSpinnerService.show();
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.resident = [];
      this.alertService.error("Use filters to display resident list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      let backClick = JSON.parse(localStorage.getItem("BackClick"));
      if(backClick!=null && backClick==true)
      {
        let resTypeFalg=JSON.parse(localStorage.getItem("ResdentGridResType"));
        this.residentTypeFlag=resTypeFalg;
      }
      this.loadGrid(1, '');
    }
  }
  loadGrid(pageNumber: number, searchText: string) {
    this.ng4LoadingSpinnerService.show();
    this.p = pageNumber;
    this.filterConfigs = {
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ? this.form.value.ddlfloors.map(item => item.Floor_Id) : [],
      Facilities: this.myform.value.ddlfacilities.length != 0 ? this.myform.value.ddlfacilities.map(item => item.Facility_Id) : [],
      NurseStations: this.myform.value.ddlnursestations.length != 0 ? this.myform.value.ddlnursestations.map(item => item.NurseStation_Id) : [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id) : [],
      Rooms: (this.form.value.ddlrooms !=null &&this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id) : [],
      Beds: (this.form.value.ddlbeds !=null&& this.form.value.ddlbeds.length != 0)? this.form.value.ddlbeds.map(item => item.Bed_Id) : [],
      ResidentType: this.residentTypeFlag,
      CurrentPage: pageNumber,
      PageSize: this.gridPagination,
      SearchText: searchText != undefined && searchText != null ? searchText.replace(new RegExp('/', 'g'), '-') : "",
      RecentFacNsFalg:1,
    }
    let formData: FormData = new FormData();
    this.dataservice.postFormData(this.config.Emar_ResidentDemographic_GetResidentsByCompanyBed, this.filterConfigs, formData)
      .subscribe((res: any) => {

        this.resident = res.Data;
        this.totalRecords = res.TotalRecords;
        localStorage.removeItem('BackClick');
        if (this.resident.length == 0)
          this.alertService.warn("No data available.");
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  onSearchChange(searchValue: string): void {
    if (searchValue.length >= 3) {
      this.loadGrid(1, searchValue);
    }
    else if (searchValue.length == 0) {
      this.loadGrid(this.p, '');
    }
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.resident = [];
    this.companyToBed = 0;
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.myform.patchValue({
      ddlnursestations: '',
     // ddlfloors: '',
    //  ddlwings: '',
     // ddlrooms: '',
    //  ddlbeds: '',
    });
    this.getNurseStationByFacilityID(item.Facility_Id);
    this.ng4LoadingSpinnerService.hide();
   // this.getCompanyToBedData(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.resident = [];
    this.companyToBed = 0;
    this.myform.patchValue({
      ddlnursestations: '',
     // ddlfloors: '',
    //  ddlwings: '',
    //  ddlrooms: '',
    //  ddlbeds: '',
    });
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
  }
  onNurseStationSelect(item: any) {
    this.getFiltersDataBySelection();
    this.getCompanyToBedByNurseStation();

  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestations = item;
    this.getFiltersDataBySelection();
    this.getCompanyToBedByNurseStation();
  }
  onNurseStationDeSelect(item: any) {
    this.getFiltersDataBySelection();
    //this.getCompanyToBedByNurseStation();
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
  }
  onNurseStationDeSelectAll(item: any) {
    this.resident = [];
   // this.getCompanyToBedData(this.myform.value.ddlfacilities[0].Facility_Id);
    this.alertService.error("Please select at least one nursing station to display data");
  }


  onCommonSelectItems(item:any) {
    this.getFiltersDataBySelection();
  }
  onCommonSelectAllItems(item:any) {
    if(item[0].Floor_Id >0)
    {
      this.form.value.ddlfloors = item;
    }
    else if(item[0].Wing_Id >0)
    {
      this.form.value.ddlwings = item;
    }
    else if(item[0].Room_Id >0)
    {
      this.form.value.ddlrooms = item;
    }
    else if(item[0].Bed_Id >0)
    {
      this.form.value.ddlbeds = item;
    }
    this.getFiltersDataBySelection();
  }
  onCommonDeSelectItems(item:any) {
    this.getFiltersDataBySelection();
  }
  onCommonDeSelectAllItems(item:any) {
    if(item == "ddlfloors")
    {
      this.form.value.ddlfloors.length = 0;
    }
    else if(item == "ddlwings")
    {
      this.form.value.ddlwings.length = 0;
    }
    else if(item == "ddlrooms")
    {
      this.form.value.ddlrooms.length = 0;
    }
    else if(item == "ddlbeds")
    {
      this.form.value.ddlbeds.length = 0;
    }
    this.getFiltersDataBySelection();
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  getNurseStationByFacilityID(facilityId: any) {    
    this.ng4LoadingSpinnerService.show();
    ///let backClick = JSON.parse(localStorage.getItem("BackClick"));
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        debugger
        this.nurseStations = res;
        if (res.length == 1) {
          debugger
          this.selectednItems = [];
          this.selectednItems.push(this.nurseStations[0]);
          this.myform.patchValue({
            ddlnursestations: this.selectednItems,
          });
          this.getFiltersDataBySelection();
            this.getCompanyToBedByNurseStation();
        }
        else{
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
                if(this.selectednItems.length ==1)
                break;
                this.getCompanyToBedByNurseStation();
              }
          //     else if(this.nurseStations!=undefined && this.nurseStations.length>0)
          // {
          //   this.selectednItems.push(this.nurseStations[0]);
          //   this.myform.patchValue({
          //     ddlnursestations: this.selectednItems,
          //   });
          //   this.getFiltersDataBySelection();
          //   this.getCompanyToBedByNurseStation();
          // }
            }
            this.myform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            this.getFiltersDataBySelection();
            this.getCompanyToBedByNurseStation();
          }
          else if(this.nurseStations!=undefined && this.nurseStations.length==1)
          {
            this.selectednItems.push(this.nurseStations[0]);
            this.myform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            this.getFiltersDataBySelection();
            this.getCompanyToBedByNurseStation();
          }
        }
      }
         if (this.facilities.length == 1) {
          this.myform.patchValue({
            ddlnursestations: this.nurseStations,
          });
          this.getCompanyToBedByNurseStation();
          this.loadGrid(1, '');
        }
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  // getCompanyToBedDataForBackClickFetch() {
  //   this.nstations = "";
  //   let facilityId = this.ResFacfilterData;
  //   if (this.ResNsfilterData.length != 0) {
  //     this.ResNsfilterData.forEach(element => {
  //       this.nstations += element + ",";
  //     });
  //     this.nstations = this.nstations.substring(0, this.nstations.length - 1);
  //     this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + this.userId + "/" + facilityId + "/" + this.nstations)
  //       .subscribe(res => {
  //         if (res.companyBedFlag == 1) {
  //           this.companyToBed = res.companyBedFlag;
  //           this.floors = res.Floors;
  //           this.wings = res.Wings;
  //           this.rooms = res.Rooms;
  //           this.beds = res.Beds;
  //         }
  //         else if (res.companyBedFlag == 0) {
  //           this.companyToBed = 0;
  //           this.floors = [];
  //           this.wings = [];
  //           this.rooms = [];
  //           this.beds = [];
  //         }
  //         this.fetchData();
  //       }, error => {
  //         this.alertService.error(error.message);
  //         this.ng4LoadingSpinnerService.hide();
  //       });
  //   }
  // }
  // getBackClickFilterData() {
  //   this.ng4LoadingSpinnerService.show();
  //   let backClick = JSON.parse(localStorage.getItem("BackClick"));
  //   if (backClick == true) {
  //     this.ResFacfilterData = JSON.parse(localStorage.getItem("ResdentGridFacility"));
  //     this.ResNsfilterData = JSON.parse(localStorage.getItem("ResdentGridNurseStations"));
  //     this.ResFloorfilterData = JSON.parse(localStorage.getItem("ResdentGridFloors"));
  //     this.ResWingfilterData = JSON.parse(localStorage.getItem("ResdentGridWings"));
  //     this.ResRoomfilterData = JSON.parse(localStorage.getItem("ResdentGridRooms"));
  //     this.ResBedfilterData = JSON.parse(localStorage.getItem("ResdentGridBeds"));
  //     if (this.ResFacfilterData != null && this.ResNsfilterData != null) {
  //       this.getNurseStationByFacilityID(this.ResFacfilterData);
  //       this.selectedfaItems = [];
  //       this.selectedfaItems.push(this.facilities.filter(f => f.Facility_Id == this.ResFacfilterData)[0]);
  //     }
  //   }
  //   this.ng4LoadingSpinnerService.hide();
  // }
  // fetchData() {
  //   this.ng4LoadingSpinnerService.show();
  //   this.selectedbItems = [];
  //   this.selectedflItems = [];
  //   this.selectednItems = [];
  //   this.selectedrItems = [];
  //   this.selectedwItems = [];
  //   if (this.ResNsfilterData.length > 0) {
  //     for (let i = 0; i < this.ResNsfilterData.length; i++) {
  //       this.selectednItems.push(this.nurseStations.filter(r => r.NurseStation_Id == parseInt(this.ResNsfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResFloorfilterData.length > 0) {
  //     for (let i = 0; i < this.ResFloorfilterData.length; i++) {
  //       this.selectedflItems.push(this.floors.filter(f => f.Floor_Id == parseInt(this.ResFloorfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResWingfilterData.length > 0) {
  //     for (let i = 0; i < this.ResWingfilterData.length; i++) {
  //       this.selectedwItems.push(this.wings.filter(w => w.Wing_Id == parseInt(this.ResWingfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResRoomfilterData.length > 0) {
  //     for (let i = 0; i < this.ResRoomfilterData.length; i++) {
  //       this.selectedrItems.push(this.rooms.filter(r => r.Room_Id == parseInt(this.ResRoomfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResBedfilterData.length > 0) {
  //     for (let i = 0; i < this.ResBedfilterData.length; i++) {
  //       this.selectedbItems.push(this.beds.filter(b => b.Bed_Id == parseInt(this.ResBedfilterData[i]))[0]);
  //     }
  //   }
  //   this.myform.patchValue({
  //     ddlfacilities: this.selectedfaItems,
  //     ddlnursestations: this.selectednItems,
  //     ddlfloors: this.selectedflItems,
  //     ddlwings: this.selectedwItems,
  //     ddlrooms: this.selectedrItems,
  //     ddlbeds: this.selectedbItems
  //   });
  //   this.getFiltersDataBySelection(this.userId);
  //   this.ng4LoadingSpinnerService.hide();
  // }
  getCompanyToBedByNurseStation() {
    this.nstations = "";
    let facilityId = this.myform.value.ddlfacilities[0].Facility_Id;
    let arNurseStations = this.myform.value.ddlnursestations;
    if (arNurseStations.length != 0) {
      arNurseStations.forEach(element => {
        this.nstations += element.NurseStation_Id + ",";
      });
      this.nstations = this.nstations.substring(0, this.nstations.length - 1);
      this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + this.userId + "/" + facilityId + "/" + this.nstations)
        .subscribe(res => {
          if (res.companyBedFlag == 1) {
            this.companyToBed = res.companyBedFlag;
            this.floors = res.Floors;
            this.wings = res.Wings;
            this.rooms = res.Rooms;
            this.beds = res.Beds;
          }
          else if (res.companyBedFlag == 0) {
            this.companyToBed = 0;
            this.floors = [];
            this.wings = [];
            this.rooms = [];
            this.beds = [];
          }
          this.getNurseStationHierarchyDetailsbyNsId(this.myform.value.ddlnursestations[0].NurseStation_Id);
         // this.getFiltersDataBySelection();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    // else if (arNurseStations.length == 0) {
    //   this.getNurseStationHierarchyDetailsbyNsId(arNurseStations);
           
    // }
  }
  insertnewResident() {

    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.resident = [];
      this.alertService.error("Please select facility and nursing station to add new resident")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.modalOption.size = 'lg';

      const modalRef = this.modalService.open(DemographicinfoComponent, this.modalOption);
      modalRef.componentInstance.title = 'New Resident Details';
      let residentData = {
        "selectedPatientId": 0,
        "selectedNurseStationId": this.myform.value.ddlnursestations[0].NurseStation_Id,
        //"selectedNurseStationId": this.demographicInfoData.NursingStationId,
        "selectedFacilityId": this.myform.value.ddlfacilities[0].Facility_Id
      }
      modalRef.componentInstance.resdata = residentData;
      modalRef.componentInstance.Result.subscribe((receivedResult) => {
        if (receivedResult > 0) {
          this.alertService.success("New resident added successfully");
          this.getFiltersDataBySelection();
          modalRef.close();
          //this.sharedService.changePatientId(receivedResult);
          //this.route.navigate(['/home/residentinformation'])        
          //this.newResidentId.emit(receivedResult);
        }
      })
    }
  }
  mouseEnter(Id:any)
  {
    
   // this.MyImages = Id;

   this.MyImages= "";
   
//this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_Orders_GetOrdersGetImage + Id )
      .subscribe(res => {
        

        //this.VisitViewFlag = res;
        //this.ng4LoadingSpinnerService.hide();
        this.MyImages = res
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });
   
  }
  mouseLeave()
  {
    this.MyImages =null;
 
  }
}
