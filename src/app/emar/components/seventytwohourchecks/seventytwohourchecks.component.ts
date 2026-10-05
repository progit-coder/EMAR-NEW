import { PersistanceService } from './../../../services/shared/persistance.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Floor, Wing, NurseStation,Room,Bed } from '../../../models/facility.model';
import { AlertService } from '../../../_services/index';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { EncodeDrugDemographic, Seventytwohourcheck } from '../../../models/emar.model';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Screens,Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-seventytwohourchecks',
  templateUrl: './seventytwohourchecks.component.html',
  styleUrls: ['./seventytwohourchecks.component.css'],
  providers: [DataService, APIConfiguration]
})
export class SeventytwohourchecksComponent implements OnInit {
  public template;
  errorMessage: string;
  public filterData: any = [];
  public floors: Floor[];
  public nurseStations: NurseStation[];
  public wings: Wing[];
  public rooms: Room[];
  public beds: Bed[];
  public facilities: any[];
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Rooms: any = {};
  dropdownSettings_Beds: any = {};
  dropdownSettings_Facilities: any = {};
  arfloor = []; arfacility = []; arnstation = []; arwing = []; arroom = []; arbed = [];
  ddlform: FormGroup;
  myform: FormGroup;
  private controlType: string = "NW";
  ShowFilter = true;
  public ordersList: any[];
  private type: string;
  private filterConfigs: any = [];
  public drugInfoData: any = [];
  SeventytwohourcheckObj: EncodeDrugDemographic[] = [];
  userCheckObj: Seventytwohourcheck;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  searchText: string = "";
  public userId: number = this.persistanceService.get(this.config.loggedInUserKey);
  public stationId: number = 0;
  pageConfig = {};
  public companyToBed: number = 0;

  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public nstations: string = "";
  public fields: any[];
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  public form: FormGroup;

  constructor(private dataservice: DataService, private config: APIConfiguration, private alertService: AlertService, private persistanceService: PersistanceService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,
    public sharedService: SharedService, private dateFormatPipe: CustomdatePipe,) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("SeventyTwoHourChecks");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.getFiltersData(userId);
    this.ddlform = new FormGroup({
      ddlfacilities: new FormControl(''),
    //  ddlfloors: new FormControl(''),
      ddlnursestations: new FormControl(''),
     // ddlwings: new FormControl(''),
    //  ddlrooms: new FormControl(''),
     // ddlbeds: new FormControl(''),
    });
    this.myform = new FormGroup({
      text: new FormControl('',Validators.required)
    });
    this.form = new FormGroup({
      fields: new FormControl(JSON.stringify(this.fields))
    });
    this.userActivity();
    //this.getCompanyToBedData();
    //this.getFiltersData(this.userId);
    //this.getSeventyTwoHourCheckDetails();
    this.getUserRecentFacilityNurseStations();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.SeventyTwoHourChecks,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }

  getUserRecentFacilityNurseStations() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFiltersData(this.userId);
      }, error => {
        this.alertService.error(error.message);
      });
  }

  getCompanyToBedData(facilityId:number) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" +facilityId)
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
        this.floors =[];
        this.wings = [];
        this.rooms = [];
        this.beds = [];
      }
      if (this.facilities.length == 1) {
        this.myform.patchValue({
          ddlfloors: this.floors,
          ddlwings:this.wings,
          ddlrooms:this.rooms,
          ddlbeds:this.beds,
        });
      }
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });


  }
  getFiltersData(userId: number): any {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {

        this.facilities = res.Facilities;

        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.ddlform.patchValue({
              ddlfacilities: this.selectedfaItems,
            });
          }
        }
        
        if(res.Facilities.length==1)
        {
          //this.getCompanyToBedData(res.Facilities[0].Facility_Id);
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.ddlform.patchValue({
            ddlfacilities:this.facilities,
          });
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
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
    this.dropdownSettings_Floors = {
      singleSelection: false,
      idField: "Floor_Id",
      textField: "Floor_Name",
      text: "Floors",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
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

    this.dropdownSettings_Wings = {
      singleSelection: false,
      idField: "Wing_Id",
      textField: "Wing_Desc",
      text: "Wings",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Rooms = {
      singleSelection: false,
      idField: "Room_Id",
      textField: "Room_Name",
      text: "Rooms",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Beds = {
      singleSelection: false,
      idField: "Bed_Id",
      textField: "Bed_Name",
      text: "Beds",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    };
  }

  getFiltersDataBySelection(userId: number, category?: string, result?: any[]): any {
    this.ng4LoadingSpinnerService.show();
    if (this.ddlform.value.ddlfacilities.length == 0 || this.ddlform.value.ddlnursestations.length == 0) {
      this.drugInfoData = [];
      this.alertService.error("please use filters to display 72 Hours list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
    this.filterConfigs = {
      User_Id: this.userId,
      Company_Id: 0,
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ?this.form.value.ddlfloors.map(item => item.Floor_Id):[],
      Facilities: this.ddlform.value.ddlfacilities.length != 0? this.ddlform.value.ddlfacilities.map(item => item.Facility_Id): [],
      NurseStations: this.ddlform.value.ddlnursestations.length != 0? this.ddlform.value.ddlnursestations.map(item => item.NurseStation_Id): [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id): [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0)? this.form.value.ddlrooms.map(item => item.Room_Id): [],
      Beds: (this.form.value.ddlbeds !=null && this.form.value.ddlbeds.length != 0)? this.form.value.ddlbeds.map(item => item.Bed_Id): [],
    }
    this.dataservice.post(this.config.Emar_GetSeventyTwoHourDetailsByfilter, this.filterConfigs)
      .subscribe((res: any[]) => {
        if (res == null)
        {
          this.drugInfoData = [];
          this.alertService.warn("No data available.");
        }
        else
          this.drugInfoData = res;
          this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.floors=[];
    this.wings=[];
    this.rooms=[];
    this.beds=[];
    this.drugInfoData = [];
    this.companyToBed=0;
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.ddlform.patchValue({
      ddlnursestations: '',
     // ddlfloors:'',
    //  ddlwings:'',
    //  ddlrooms:'',
     // ddlbeds:'',
    })
    this.getNurseStationByFacilityID(item.Facility_Id);
    //this.getCompanyToBedData(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.floors=[];
    this.wings=[];
    this.rooms=[];
    this.beds=[];
    this.drugInfoData = [];
    this.companyToBed=0;
    this.ddlform.patchValue({
      ddlnursestations: '',
     // ddlfloors:'',
      //ddlwings:'',
    //  ddlrooms:'',
     // ddlbeds:'',
    })
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
    this.getFiltersDataBySelection(this.userId);
    this.getCompanyToBedByNurseStation();
  }
  onNurseStationSelectAll(item: any) {
    this.ddlform.value.ddlnursestations = item;
    this.getFiltersDataBySelection(this.userId);
  }
  onNurseStationDeSelect(item: any) {
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
    this.getFiltersDataBySelection(this.userId);
  }
  onNurseStationDeSelectAll(item: any) {
    this.ddlform.value.ddlnursestations.length = 0;
    this.drugInfoData = [];
    this.alertService.error("Please select at least one nursing station to display the data.");
  }
  onCommonSelectItems(item:any) {
    this.getFiltersDataBySelection(this.userId);
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
    this.getFiltersDataBySelection(this.userId);
  }
  onCommonDeSelectItems(item:any) {
    this.getFiltersDataBySelection(this.userId);
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
    this.getFiltersDataBySelection(this.userId);
  }
  // onFloorSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onFloorSelectAll(item: any) {
  //   this.ddlform.value.ddlfloors = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onFloorDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onFloorDeSelectAll(item: any) {
  //   this.ddlform.value.ddlfloors.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }

  // onWingSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onWingSelectAll(item: any) {
  //   this.ddlform.value.ddlwings = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onWingDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onWingDeSelectAll(item: any) {
  //   this.ddlform.value.ddlwings.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomSelectAll(item: any) {
  //   this.ddlform.value.ddlrooms = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomDeSelectAll(item: any) {
  //   this.ddlform.value.ddlrooms.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedSelectAll(item: any) {
  //   this.ddlform.value.ddlbeds = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedDeSelectAll(item: any) {
  //   this.ddlform.value.ddlbeds.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }

  getSeventyTwoHourCheckDetails() {
    this.filterConfigs = {
      User_Id: this.userId,
      Company_Id: 0,
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ?this.form.value.ddlfloors.map(item => item.Floor_Id):[],
      Facilities: this.ddlform.value.ddlfacilities.length != 0? this.ddlform.value.ddlfacilities.map(item => item.Facility_Id): [],
      NurseStations: this.ddlform.value.ddlnursestations.length != 0? this.ddlform.value.ddlnursestations.map(item => item.NurseStation_Id): [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0)? this.form.value.ddlwings.map(item => item.Wing_Id): [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0)? this.form.value.ddlrooms.map(item => item.Room_Id): [],
      Beds: (this.form.value.ddlbeds !=null && this.form.value.ddlbeds.length != 0)? this.form.value.ddlbeds.map(item => item.Bed_Id): [],
    }
    let userid = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.post(this.config.Emar_GetSeventyTwoHourCheckDetails ,this.filterConfigs)
      .subscribe(res => {
        this.drugInfoData = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  resetScreen() {
    this.myform.reset();
    this.form.reset();
    this.fields =[];
  }
  UpdateArray(DrugAdministerId: number, Value: string) {
    if (Value != "") {
      let svObj = new EncodeDrugDemographic();
      svObj.DrugAdminister_Id = DrugAdministerId;
      svObj.SeventyTwoComment = Value;
      svObj.SeventyTwoCommentBy = this.persistanceService.get(this.config.loggedInUserKey);
      svObj.SeventyTwoCommentOn = this.dateFormatPipe.dateWithTime(new Date());
      let index = this.SeventytwohourcheckObj.findIndex(cs => cs.DrugAdminister_Id == DrugAdministerId);
      if (index >= 0) {
        this.SeventytwohourcheckObj.splice(index, 1);
        this.SeventytwohourcheckObj.push(svObj);
      }
      else
        this.SeventytwohourcheckObj.push(svObj);
    }
    else if (Value == "") {
      let index = this.SeventytwohourcheckObj.findIndex(cs => cs.DrugAdminister_Id == DrugAdministerId);
      this.SeventytwohourcheckObj.splice(index, 1);
    }
  }
  insertChecks() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_InsertUpdateSeventyTwoHourChecks, this.SeventytwohourcheckObj)
      .subscribe(res => {
        this.SeventytwohourcheckObj = [];
        this.alertService.success("Save successful");
        this.getSeventyTwoHourCheckDetails();
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    this.resetScreen();
    window.scroll(0,0);
  }
  getNurseStationByFacilityID(facilityId: any) {
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
            }
            this.ddlform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            //this.getFiltersDataBySelection(this.userId);
            this.getCompanyToBedByNurseStation();
            this.getFiltersDataBySelection(this.userId);
          }
        }

        else if(this.facilities.length==1)
        {
          this.ddlform.patchValue({
            ddlnursestations:this.nurseStations,
          });
          this.getFiltersDataBySelection(this.userId);
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
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
            else if(res != null && this.companyToBed!=0){
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
          
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getCompanyToBedByNurseStation() {
    this.nstations = "";
    let facilityId = this.ddlform.value.ddlfacilities[0].Facility_Id;
    let arNurseStations = this.ddlform.value.ddlnursestations;
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
          this.getNurseStationHierarchyDetailsbyNsId(this.ddlform.value.ddlnursestations[0].NurseStation_Id);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else if (arNurseStations.length == 0) {
     // this.getCompanyToBedData(facilityId);
    }
  }

}



