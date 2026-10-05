import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Company } from '../../../models/company.model';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Facility, Floor, NurseStation, Wing, Room, Bed, CompanyBedConfigSave } from '../../../models/facility.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { BedconfigcompanymasterComponent } from '../bedconfigcompanymaster/bedconfigcompanymaster.component';

@Component({
  selector: 'app-companybedconfig',
  templateUrl: './companybedconfig.component.html',
  styleUrls: ['./companybedconfig.component.css'],
  providers: [DataService, APIConfiguration]
})
export class CompanybedconfigComponent implements OnInit {
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  myform: FormGroup;
  public facilities: Facility[];
  public selectedCompanyItem = [];
  public selectedFacilityItem = [];
  public selectedNurseStationItem = [];
  public selectedFloorItem = [];
  public selectedWingItem = [];
  public selectedRoomItem = [];
  public selectedBedItem = [];
  dropdownSettings_Company: any = {};
  dropdownSettings_Facility: any = {};
  dropdownSettings_Nurse: any = {};
  dropdownSettings_Floor: any = {};
  dropdownSettings_Wing: any = {};
  dropdownSettings_Room: any = {};
  dropdownSettings_Bed: any = {};
  public companybedconfigs: any[] = [];
  public room = new Room();
  public template;
  public rooms: Room[];
  public floorMaster: any[];
  public companies: Company[];
  public floor = new Floor();
  public wing = new Wing();
  public wings: Wing[];
  public bed = new Bed();
  public beds: Bed[];
  ShowFilter = true;
  public companyselected: number;
  public nurseStation = new NurseStation();
  public nurseStations: NurseStation[];
  public Bedconfigid = 0;
  private CompanybedconfigObj: CompanyBedConfigSave;
  private selectedCompanyId = 0;
  public selectedFacilityId = 0;
  private selectedNStationId = 0;
  private selectedFloorId = 0;
  private selectedWingId = 0;
  private selectedRoomId = 0;
  private selectedBedId = 0;
  fid: any;
  auditTable: any;
  public inactivecheckbox: boolean = false;
  public companybedconfigsList: any[] = [];
  public modalHistoryIsOpen: boolean = false;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  pageConfig: {};
  FacilitypageConfig: {};
  CompanypageConfig: {};
  FloorpageConfig: {};
  NursepageConfig: {};
  RoompageConfig: {};
  WingpageConfig: {};
  BedpageConfig:{};
  public form: FormGroup;
  unsubcribe: any
  public fields: any[]=[];
  public seQ:number=0;
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  public companymastermodal:boolean=false;
  public facilitymastermodal:boolean=false;
  public nursestationmastermodal:boolean=false;
  public floormastermodal:boolean=false;
  public wingmastermodal:boolean=false;
  public roommastermodal:boolean=false;
  public bedmastermodal:boolean=false;
  @ViewChild('companybedconfig') comp: BedconfigcompanymasterComponent;

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private alertService: AlertService,
    private chRef: ChangeDetectorRef, private sharedService: SharedService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) { }

  @Output() notifyParent = new EventEmitter<number>();
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("CompanyBedConfiguration");
    this.FacilitypageConfig = this.persistanceService.getPermissionsByScreen("FacilityMaster");
    this.CompanypageConfig = this.persistanceService.getPermissionsByScreen("CompanyMaster");
    this.FloorpageConfig = this.persistanceService.getPermissionsByScreen("FloorMaster");
    this.NursepageConfig = this.persistanceService.getPermissionsByScreen("NursingStationMaster");
    this.RoompageConfig = this.persistanceService.getPermissionsByScreen("RoomMaster");
    this.WingpageConfig = this.persistanceService.getPermissionsByScreen("WingMaster");
    this.BedpageConfig = this.persistanceService.getPermissionsByScreen("BedMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getCompanies();
    //this.getFacilityDropData();
    //this.getNurseStations();
    this.getFloorDropData();
    this.getWings();
    this.getRooms();
    this.getBeds();
    this.myform = new FormGroup({
      companyName: new FormControl('', Validators.required),
      facilityName: new FormControl('', Validators.required),
      nursestationName: new FormControl('', Validators.required),
      // floorName: new FormControl(''),
      // wingName: new FormControl(''),
      // roomName: new FormControl(''),
      // bedName: new FormControl('', Validators.required),
      status: new FormControl('1', Validators.required),

    });
    this.form = new FormGroup({
      fields: new FormControl(JSON.stringify(this.fields))
    });
    this.dropdownSettings_Company = {
      singleSelection: true,
      idField: "Company_Id",
      textField: "Company_Name",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Facility = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Company'
    };
    this.dropdownSettings_Nurse = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStationName",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility'
    };
    // this.dropdownSettings_Floor = {
    //   singleSelection: true,
    //   idField: "Floor_Id",
    //   textField: "Floor_Name",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    // this.dropdownSettings_Wing = {
    //   singleSelection: true,
    //   idField: "Wing_Id",
    //   textField: "Wing_Desc",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    // this.dropdownSettings_Room = {
    //   singleSelection: true,
    //   idField: "Room_Id",
    //   textField: "Room",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    // this.dropdownSettings_Bed = {
    //   singleSelection: true,
    //   idField: "Bed_Id",
    //   textField: "Bed",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    this.ng4LoadingSpinnerService.hide();
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getHistoryById(BedConfigId: number) {
    this.auditTable = {
      "tableName": "ComapnyBedConfig",
      "recordId": BedConfigId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  getCompanyBedCogfigDetailsByID(ID: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyBedConfig + ID)
      .subscribe(res => {
        this.selectedFacilityId = res.Facility_Id;
        this.getFetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    window.scroll(0, 0);
  }
  getFetchData(data: any) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStationsNew + this.selectedFacilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.getNurseStationHierarchyDetailsbyNsId(data.NurseStation_Id,data)
        //this.fetchData(data);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchData(res: any,indexValues?:any) {
    this.selectedCompanyItem = [];
    this.selectedFacilityItem = [];
    this.selectedNurseStationItem = [];
    this.selectedFloorItem = [];
    this.selectedWingItem = [];
    this.selectedRoomItem = [];
    this.selectedBedItem = [];
    debugger
    //  this.selectedFacilityId=res.Facility_Id;
    this.selectedCompanyItem.push(this.companies.filter(c => c.Company_Id == res.Company_Id)[0]);
    this.selectedFacilityItem.push(this.facilities.filter(f => f.Facility_Id == res.Facility_Id)[0]);
    this.selectedNurseStationItem.push(this.nurseStations.filter(n => n.NurseStation_Id == res.NurseStation_Id)[0]);
    if(this.floorIndex!=0)
    {
    this.selectedFloorItem.push(this.floorMaster.filter(f => f.Floor_Id == res.Floor_Id)[0]);
    }
    if(this.wingIndex!=0)
    {
    this.selectedWingItem.push(this.wings.filter(w => w.Wing_Id == res.Wing_Id)[0]);
    }
    if(this.roomIndex!=0)
    {
    this.selectedRoomItem.push(this.rooms.filter(r => r.Room_Id == res.Room_Id)[0]);
    }
    if(this.bedIndex!=0)
    {
    this.selectedBedItem.push(this.beds.filter(b => b.Bed_Id == res.Bed_Id)[0]);
    }
    this.myform.patchValue({
      companyName: this.selectedCompanyItem,
      facilityName: this.selectedFacilityItem,
      nursestationName: this.selectedNurseStationItem,
      // floorName: this.selectedFloorItem,
      // wingName: this.selectedWingItem,
      // roomName: this.selectedRoomItem,
      // bedName: this.selectedBedItem,
       status: res.BedConfig_Status,
    });
    // this.form.controls["floorName"].setValue(this.selectedFloorItem);
    // this.form.controls["wingName"].setValue(this.selectedWingItem);
    // this.form.controls["roomName"].setValue(this.selectedRoomItem);
    // this.form.controls["bedName"].setValue(this.selectedBedItem);
    //this.fields.name
    this.fields = [
      {
        label: 'FloorName',
        name: 'floorName',
        data: this.floorMaster,
        settings: {
          singleSelection: true,
          idField: "Floor_Id",
          textField: "Floor_Name",
          text: "Floors",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        },
        ngModel: this.selectedFloorItem,
        placeholder: 'Floors',
        index:this.floorIndex
      },
      {
        label: 'Wing Name',
        name: 'wingName',
        data: this.wings,
        settings: {
          singleSelection: true,
          idField: "Wing_Id",
          textField: "Wing_Desc",
          text: "Wings",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        },
        ngModel: this.selectedWingItem,
        placeholder: 'Wings',
        index:this.wingIndex
      },
      {
        label: 'Room Name',
        name: 'roomName',
        data: this.rooms,
        settings: {
          singleSelection: true,
          idField: "Room_Id",
          textField: "Room_Name",
          text: 'Rooms',
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        },
        ngModel: this.selectedRoomItem,
        placeholder: 'Rooms',
        index:this.roomIndex
      },
      {
        label: 'Bed Name',
        name: 'bedName',
        data: this.beds,
        settings: {
          singleSelection: true,
          idField: "Bed_Id",
          textField: "Bed_Name",
          text: "Beds",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        },
        ngModel: this.selectedBedItem,
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
            if(indexValues!=null)
            {
            if(this.floorIndex!=0 && this.floorIndex!=null)
            {
              const floorvalidation = this.form.get('floorName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            if(this.wingIndex!=0 && this.wingIndex!=null)
            {
              const floorvalidation = this.form.get('wingName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            if(this.roomIndex!=0 && this.roomIndex!=null)
            {
              const floorvalidation = this.form.get('roomName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            if(this.bedIndex!=0 && this.bedIndex!=null)
            {
              const floorvalidation = this.form.get('bedName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
          }
    this.Bedconfigid = res.BedConfig_Id;
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.CompanyBedConfig, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  sendNotification() {

    this.notifyParent.emit(this.companyselected);
    this.companymastermodal=true;
    setTimeout(() => {
      this.comp.inputFocus.nativeElement.focus()
      }, 500);
    this.getCompanies();
  }
  closeCompanyMaster()
  {
    this.companymastermodal=false;
    this.getCompanies();
  }
  getCompanies() {
    this.dataservice.get<Company[]>(this.config.Emar_CompanyMaster_GetAllActiveCompanyDrop)
      .subscribe(res => {
        this.companies = res;

        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message)
      });
  }
  closeFacilityMaster()
  {
    this.facilitymastermodal=false;
    if(this.selectedCompanyId!=undefined && this.selectedCompanyId!=null && this.selectedCompanyId!=0)
    {
    this.getFacilityDropData();
    }
  }
  getFacilityDropData() {
    this.dataservice.get<Facility[]>(this.config.Emar_Facility_GetFacilitiesBycompanyId + this.selectedCompanyId)
      .subscribe(res => {
        this.facilities = res;
        this.ng4LoadingSpinnerService.hide();
        if (res.length == 0) {
          this.alertService.warn("Company don’t have any facilities.");
          this.nurseStations = [];
        }
      },
        error => {
          this.alertService.error(error.message)
        });

  }
  closeFloorMaster()
  {
    this.floormastermodal=false;
    this.getFloorDropData();
  }
  getFloorDropData() {
    this.dataservice.get<Floor[]>(this.config.Emar_FacilityMaster_GetAllActiveFloorNames)
      .subscribe(res => this.floorMaster = res,
        error => {
          this.alertService.error(error.message)
        });
  }
  closeNsMaster()
  {
    this.nursestationmastermodal=false;
    if(this.selectedFacilityId!=undefined && this.selectedFacilityId!=null && this.selectedFacilityId!=0)
    {
    this.getNurseStations();
    }
  }
  getNurseStations() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStationsNew + this.selectedFacilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.ng4LoadingSpinnerService.hide();
        if (res.length == 0) {
          this.alertService.warn("Please select facility for nursing stations.");
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
  }
  closeWingMaster()
  {
    this.wingmastermodal=false;
    this.getWings();
  }
  getWings() {
    this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetAllActiveWingNames)
      .subscribe(res => {
        this.wings = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });

  }
  closeRoomMaster()
  {
    this.roommastermodal=false;
    this.getRooms();
  }
  getRooms() {
    this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetAllActiveRoomNames)
      .subscribe(res => {
        this.rooms = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  closeBedMaster()
  {
    this.bedmastermodal=false;
    this.getBeds();
  }
  getBeds() {
    this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetAllActiveBedNames)
      .subscribe(res => {
        this.beds = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getCompanyDetailsByID(companyId: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = companyId;
    this.getFacilityDropData();
    this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    // this.selectedFloorId = this.myform.value.floorName.length == 0 ? 0 : this.myform.value.floorName[0].Floor_Id;
    // this.selectedWingId = this.myform.value.wingName.length == 0 ? 0 : this.myform.value.wingName[0].Wing_Id;
    // this.selectedRoomId = this.myform.value.roomName.length == 0 ? 0 : this.myform.value.roomName[0].Room_Id;
    // this.selectedBedId = this.myform.value.bedName.length == 0 ? 0 : this.myform.value.bedName[0].Bed_Id;

    //this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getCompanyDetailsByIDDeselect(companyId: number) {
    this.ng4LoadingSpinnerService.show();
    this.facilities=[];
    this.selectedCompanyId = companyId;
    this.ng4LoadingSpinnerService.hide();
    //this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    //this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    // this.selectedFloorId = this.form.value.floorName !=null && this.form.value.floorName.length != 0 ?this.form.value.floorName[0].Floor_Id:0;
    // this.selectedWingId = this.form.value.wingName !=null && this.form.value.wingName.length != 0 ?this.form.value.wingName[0].Wing_Id:0;
    // this.selectedRoomId = this.form.value.roomName !=null && this.form.value.roomName.length != 0 ?this.form.value.roomName[0].Room_Id:0;
    // this.selectedBedId = this.form.value.bedName !=null && this.form.value.bedName.length != 0 ?this.form.value.bedName[0].Bed_Id:0;

    //this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getFacilityDetailsByID(facilityId: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
    this.selectedFacilityId = facilityId;
    this.getNurseStations();
    this.ng4LoadingSpinnerService.hide();
    //this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    // this.selectedFloorId = this.form.value.floorName !=null && this.form.value.floorName.length != 0 ?this.form.value.floorName[0].Floor_Id:0;
    // this.selectedWingId = this.form.value.wingName !=null && this.form.value.wingName.length != 0 ?this.form.value.wingName[0].Wing_Id:0;
    // this.selectedRoomId = this.form.value.roomName !=null && this.form.value.roomName.length != 0 ?this.form.value.roomName[0].Room_Id:0;
    // this.selectedBedId = this.form.value.bedName !=null && this.form.value.bedName.length != 0 ?this.form.value.bedName[0].Bed_Id:0;
    //this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getNurseStationDetailsByID(nursestationId: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
    this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    this.selectedNStationId = nursestationId;
    this.ng4LoadingSpinnerService.hide();
    // this.selectedFloorId = this.form.value.floorName !=null && this.form.value.floorName.length != 0 ?this.form.value.floorName[0].Floor_Id:0;
    // this.selectedWingId = this.form.value.wingName !=null && this.form.value.wingName.length != 0 ?this.form.value.wingName[0].Wing_Id:0;
    // this.selectedRoomId = this.form.value.roomName !=null && this.form.value.roomName.length != 0 ?this.form.value.roomName[0].Room_Id:0;
    // this.selectedBedId = this.form.value.bedName !=null && this.form.value.bedName.length != 0 ?this.form.value.bedName[0].Bed_Id:0;
    //this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getFloorDetailsByID(floorId: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
    this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    this.selectedFloorId = floorId;
    this.selectedWingId = this.myform.value.wingName.length == 0 ? 0 : this.myform.value.wingName[0].Wing_Id;
    this.selectedRoomId = this.myform.value.roomName.length == 0 ? 0 : this.myform.value.roomName[0].Room_Id;
    this.selectedBedId = this.myform.value.bedName.length == 0 ? 0 : this.myform.value.bedName[0].Bed_Id;

    this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getWingDetailsByID(wingId: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
    this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    this.selectedFloorId = this.myform.value.floorName.length == 0 ? 0 : this.myform.value.floorName[0].Floor_Id;
    this.selectedWingId = wingId;
    this.selectedRoomId = this.myform.value.roomName.length == 0 ? 0 : this.myform.value.roomName[0].Room_Id;
    this.selectedBedId = this.myform.value.bedName.length == 0 ? 0 : this.myform.value.bedName[0].Bed_Id;

    this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getRoomDetailsByID(roomId: number) {
    // this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
    this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    this.selectedFloorId = this.myform.value.floorName.length.length == 0 ? 0 : this.myform.value.floorName[0].Floor_Id;
    this.selectedWingId = this.myform.value.wingName.length == 0 ? 0 : this.myform.value.wingName[0].Wing_Id;
    this.selectedRoomId = roomId;
    this.selectedBedId = this.myform.value.bedName == "" ? 0 : this.myform.value.bedName;

    this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getBedDetailsByID(bedId: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
    this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
    this.selectedNStationId = this.myform.value.nursestationName.length == 0 ? 0 : this.myform.value.nursestationName[0].NurseStation_Id;
    this.selectedFloorId = this.myform.value.floorName.length == 0 ? 0 : this.myform.value.floorName[0].Floor_Id;
    this.selectedWingId = this.myform.value.wingName.length == 0 ? 0 : this.myform.value.wingName[0].Wing_Id;
    this.selectedRoomId = this.myform.value.roomName.length == 0 ? 0 : this.myform.value.roomName[0].Room_Id;
    this.selectedBedId = bedId

    this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
  }
  getCompanyBedConfigs(companyId: number, facilityId: number, nursestationId: number, floorId: number, wingId: number, roomId: number, bedId: number) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetCompanyBedGridData + companyId + "/" + facilityId + "/" + nursestationId + "/" + floorId + "/" + wingId + "/" + roomId + "/" + bedId)
      .subscribe(res => {
        this.inactivecheckbox = false;
        this.companybedconfigs = res;
        this.companybedconfigsList = res.filter(c => c.BedConfig_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();

      });
  }
  insertCompanyBedConfig() {
    this.ng4LoadingSpinnerService.show();
    if((this.form.value.floorName!=null && this.form.value.floorName.length!=0)||(this.form.value.wingName!=null && this.form.value.wingName.length!=0) ||(this.form.value.roomName!=null && this.form.value.roomName.length!=0) || (this.form.value.bedName!=null && this.form.value.bedName.length!=0))
    {
    this.CompanybedconfigObj = {
      BedConfig_Id: this.Bedconfigid,
      Company_Id: this.myform.value.companyName[0].Company_Id,
      Facility_Id: this.myform.value.facilityName[0].Facility_Id,
      Floor_Id: this.form.value.floorName!=null && this.form.value.floorName.length!=0? this.form.value.floorName[0].Floor_Id:null,
      NurseStation_Id: this.myform.value.nursestationName[0].NurseStation_Id,
      Wing_Id: this.form.value.wingName!=null && this.form.value.wingName.length!=0? this.form.value.wingName[0].Wing_Id:null,
      Room_Id: this.form.value.roomName!=null && this.form.value.roomName.length!=0? this.form.value.roomName[0].Room_Id:null,
      Bed_Id: this.form.value.bedName!=null && this.form.value.bedName.length!=0? this.form.value.bedName[0].Bed_Id:null,
      BedConfig_Status: (this.myform.value.status == true ? 1 : 0),
      BedConfig_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      BedConfig_CreatedDate: new Date().toISOString(),
    };

    this.dataservice.post(this.config.Emar_Facility_InsertCompanyBedConfig, this.CompanybedconfigObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success("Save successful");
          //this.resetScreen();
          this.Bedconfigid=0;
          this.inactivecheckbox=false;
          this.selectedRecords=[];
          this.CheckAll=false;
          this.UpdateStatus=true;
          //this.resetDynamicFormControls();
          this.getNurseStationHierarchyDetailsbyNsId(this.CompanybedconfigObj.NurseStation_Id);
        }
        else if (res == 2)
          this.alertService.warn("Selected Facility is already mapped to another Company");
        else if (res == 3)
          this.alertService.warn("Selected Nursing Station is already mapped to another Facility");
        else if (res == 4)
          this.alertService.warn("Record already exist with selected details.");

        this.getCompanyBedConfigs(this.CompanybedconfigObj.Company_Id, this.CompanybedconfigObj.Facility_Id, this.CompanybedconfigObj.NurseStation_Id, this.CompanybedconfigObj.Floor_Id==null?0:this.CompanybedconfigObj.Floor_Id, this.CompanybedconfigObj.Wing_Id==null?0:this.CompanybedconfigObj.Wing_Id, this.CompanybedconfigObj.Room_Id==null?0:this.CompanybedconfigObj.Room_Id, this.CompanybedconfigObj.Bed_Id==null?0:this.CompanybedconfigObj.Bed_Id);
        this.ng4LoadingSpinnerService.hide();

      }, error => {
        this.alertService.error(error.message)
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else 
    {
      this.alertService.warn("Please select at least one hierarchy");
      this.ng4LoadingSpinnerService.hide();
    }

  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      companyName: '',
      facilityName: '',
      status: '1',

    });
    this.ng4LoadingSpinnerService.hide();
    this.Bedconfigid = 0;
    this.selectedCompanyId = 0;
    this.selectedFacilityId = 0;
    this.selectedNStationId = 0;
    this.selectedFloorId = 0;
    this.selectedWingId = 0;
    this.selectedRoomId = 0;
    this.selectedBedId = 0;
    this.nurseStations = [];
    this.companybedconfigsList=[];
    this.companybedconfigs=[];
    this.inactivecheckbox=false;
    this.selectedRecords=[];
    this.CheckAll=false;
    this.UpdateStatus=true;
    this.resetDynamicFormControls();
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      //  this.inactivecheckbox=true;
      this.companybedconfigsList = this.companybedconfigs.filter(c => c.BedConfig_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.companybedconfigsList = this.companybedconfigs.filter(c => c.BedConfig_Status == 1);
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.companybedconfigsList.forEach(element => {
        element.BedConfig_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.BedConfig_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.BedConfig_Status=1;
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.UpdateStatus = true;
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.UpdateStatus = false;
      item.BedConfig_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.BedConfig_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.BedConfig_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.BedConfig_Id == item.BedConfig_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateComapnyBedConfigStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateCompanyBedConfigsStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getCompanyBedConfigs(this.selectedRecords[0].Company_Id, this.selectedRecords[0].Facility_Id, this.selectedRecords[0].NurseStation_Id, 0, 0, 0, 0);
            this.selectedRecords = [];

          }
          else if (res == 0) {
            this.alertService.error("Status Updated Failed.");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  onCompanySelect(item: any) {
    this.getCompanyDetailsByID(item.Company_Id);
  }
  onCompanyDeSelect(item: any) {
    this.getCompanyDetailsByIDDeselect(0);
    this.selectedFacilityItem=[];
    this.nurseStations=[];
    this.selectedNurseStationItem=[];
    this.resetDynamicFormControls();
  }
  onFacilitySelect(item: any) {
    this.getFacilityDetailsByID(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.getFacilityDetailsByID(0);
    this.nurseStations=[];
    this.selectedNurseStationItem=[];
    this.resetDynamicFormControls();
  }
  onNurseStationSelect(item: any) {
    this.companybedconfigs=[];
    this.companybedconfigsList=[];
    this.resetDynamicFormControls();
    this.getNurseStationDetailsByID(item.NurseStation_Id);
    this.getNurseStationHierarchyDetailsbyNsId(item.NurseStation_Id);
    
  }
  onNurseStationDeSelect(item: any) {
    this.getNurseStationDetailsByID(0);
    this.companybedconfigs=[];
    this.companybedconfigsList=[];
    this.resetDynamicFormControls();
  }
  onFloorSelect(item: any) {
    this.getFloorDetailsByID(item.Floor_Id);
  }
  onFloorDeSelect(item: any) {
    this.getFloorDetailsByID(0);
  }
  onWingSelect(item: any) {
    this.getWingDetailsByID(item.Wing_Id);
  }
  onWingDeSelect(item: any) {
    this.getWingDetailsByID(0);
  }
  onRoomSelect(item: any) {
    this.getRoomDetailsByID(item.Room_Id);
    this.ng4LoadingSpinnerService.hide();
  }
  onRoomDeSelect(item: any) {
    this.getRoomDetailsByID(0);
  }
  onBedSelect(item: any) {
    this.getBedDetailsByID(item.Bed_Id);
  }
  onBedDeSelect(item: any) {
    this.getBedDetailsByID(0);
  }
  getNurseStationHierarchyDetailsbyNsId(nsId:any,data?:any)
  {
    debugger
    this.ng4LoadingSpinnerService.show();
    this.selectedFloorItem=[];
    this.selectedWingItem=[];
    this.selectedRoomItem=[];
    this.selectedBedItem=[];
    this.dataservice.get<any[]>(this.config.Emar_CompanyBedMapping_GetNurseStationHierarchyDetailsbyNsId + nsId)
      .subscribe((res: any) => {
    
            if(res == null)
            {
              this.floorIndex =1;
              this.wingIndex =2;
              this.roomIndex =3;
              this.bedIndex =4;  
            }
            else if(res != null ){
            this.floorIndex =res.FloorPrior;
            this.wingIndex =res.WingPrior;
            this.roomIndex =res.RoomPrior;
            this.bedIndex =res.BedPrior;
           }
           if(data==undefined)
           {
            this.fields = [
              {
                label: 'Floor Name',
                name: 'floorName',
                data: this.floorMaster,
                settings: {
                  singleSelection: true,
                  idField: "Floor_Id",
                  textField: "Floor_Name",
                  text: "Floors",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedFloorItem,
                placeholder: 'Floors',
                index:this.floorIndex
              },
              {
                label: 'Wing Name',
                name: 'wingName',
                data: this.wings,
                settings: {
                  singleSelection: true,
                  idField: "Wing_Id",
                  textField: "Wing_Desc",
                  text: "Wings",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedWingItem,
                placeholder: 'Wings',
                index:this.wingIndex
              },
              {
                label: 'Room Name',
                name: 'roomName',
                data: this.rooms,
                settings: {
                  singleSelection: true,
                  idField: "Room_Id",
                  textField: "Room_Name",
                  text: 'Rooms',
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedRoomItem,
                placeholder: 'Rooms',
                index:this.roomIndex
              },
              {
                label: 'Bed Name',
                name: 'bedName',
                data: this.beds,
                settings: {
                  singleSelection: true,
                  idField: "Bed_Id",
                  textField: "Bed_Name",
                  text: "Beds",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedBedItem,
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
            if(res!=null)
            {
            if(this.floorIndex!=0 && this.floorIndex!=null)
            {
              const floorvalidation = this.form.get('floorName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            if(this.wingIndex!=0 && this.wingIndex!=null)
            {
              const floorvalidation = this.form.get('wingName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            if(this.roomIndex!=0 && this.roomIndex!=null)
            {
              const floorvalidation = this.form.get('roomName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            if(this.bedIndex!=0 && this.bedIndex!=null)
            {
              const floorvalidation = this.form.get('bedName');
              floorvalidation.setValidators([Validators.required]);
              floorvalidation.updateValueAndValidity();
            }
            this.getCompanyBedConfigs(this.selectedCompanyId, this.selectedFacilityId, this.selectedNStationId, this.selectedFloorId, this.selectedWingId, this.selectedRoomId, this.selectedBedId);
            }
          }
            else
            {
              this.fetchData(data,res);
            }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  resetDynamicFormControls()
  {
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
  }
  openFacMaster()
  {
    this.facilitymastermodal=true;
  }
  openNsMaster()
  {
    this.nursestationmastermodal=true;
  }
  openFloorMaster()
  {
    this.floormastermodal=true;
  }
  openWingMaster()
  {
    this.wingmastermodal=true;
  }
  openRoomMaster()
  {
    this.roommastermodal=true;
  }
  openBedMaster()
  {
    this.bedmastermodal=true;
  }
}
