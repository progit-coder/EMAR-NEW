import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Company } from '../../../models/company.model';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Facility, Floor, NurseStation, Wing, Room, Bed, CompanyBedConfigSave,NurseStationHierarchy } from '../../../models/facility.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { FunctionCall } from '@angular/compiler';
@Component({
  selector: 'app-companytobedmaping',
  templateUrl: './companytobedmaping.component.html',
  styleUrls: ['./companytobedmaping.component.css'],
  providers: [DataService, APIConfiguration]
})
export class CompanytobedmapingComponent implements OnInit {
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
  public room = [];
  public template;
  public rooms: Room[];
  public floor =[];
  public companies: Company[];
  public wing =[];
  public wings: Wing[];
  public bed =[];
  public beds: Bed[];
  ShowFilter = true;
  public companyselected: number;
  public nurseStation = new NurseStation();
  public nurseStations: NurseStation[];
  public Bedconfigid = 0;
  private NurseStationHierarchyObj: NurseStationHierarchy;
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
  public dataFields:any[];
  public companyList: any[] = [];
  public selectedItems = [];
  public nsHierarchy:number=0;
  public mappingArray:any[]=[];
  public saveFlag:boolean=true;
  public records=[];

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private alertService: AlertService,
    private chRef: ChangeDetectorRef, private sharedService: SharedService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) { }
    ngOnInit() {
      this.pageConfig = this.persistanceService.getPermissionsByScreen("CompanyBedHierarchy");
      if (this.pageConfig != undefined) {
        if (this.pageConfig["AccessRead"] == 0) {
          this.persistanceService.redirectToHomePage();
        }
        else {
      this.template = this.dataservice.template;
      this.ng4LoadingSpinnerService.show();
      this.getCompanies();
      this.userActivity();
      this.myform = new FormGroup({
        companyName: new FormControl('', Validators.required),
        facilityName: new FormControl('', Validators.required),
        nursestationName: new FormControl('', Validators.required),
        floorName: new FormControl(''),
        wingName: new FormControl(''),
        roomName: new FormControl(''),
        bedName: new FormControl(''),
        //status: new FormControl('1', Validators.required),
  
      });
      this.records = [
        { item_id: 1, item_text: '1' },
        { item_id: 2, item_text: '2' },
        { item_id: 3, item_text: '3' },
        { item_id: 4, item_text: '4' },
      ];
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
      this.dropdownSettings_Floor = {
        singleSelection: true,
        idField: "item_id",
        textField: "item_text",
        itemsShowLimit: 1,
        allowSearchFilter: this.ShowFilter,
        closeDropDownOnSelection:true,
        noDataAvailablePlaceholderText: 'Please Select NurseStation'
      };
      this.dropdownSettings_Wing = {
        singleSelection: true,
        idField: "item_id",
        textField: "item_text",
        itemsShowLimit: 1,
        allowSearchFilter: this.ShowFilter,
        closeDropDownOnSelection:true,
        noDataAvailablePlaceholderText: 'Please Select NurseStation'
      };
      this.dropdownSettings_Room = {
        singleSelection: true,
        idField: "item_id",
        textField: "item_text",
        itemsShowLimit: 1,
        allowSearchFilter: this.ShowFilter,
        closeDropDownOnSelection:true,
        noDataAvailablePlaceholderText: 'Please Select NurseStation'
      };
      this.dropdownSettings_Bed = {
        singleSelection: true,
        idField: "item_id",
        textField: "item_text",
        itemsShowLimit: 1,
        allowSearchFilter: this.ShowFilter,
        closeDropDownOnSelection:true,
        noDataAvailablePlaceholderText: 'Please Select NurseStation'
      };
      this.ng4LoadingSpinnerService.hide();
    }
    //this.getCompanyBedConfigs();
  }
  else
  this.persistanceService.redirectToHomePage();
    }
    userActivity() {
      this.sharedService.insertUserActivityDetails(Screens.CompanyBedHierarchy, Activity.View, '')
        .subscribe(res => { }, error => {
          this.alertService.error(error.message);
        });
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
    getNurseStations(editObj?:any) {
      this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStationsNew + this.selectedFacilityId)
        .subscribe(res => {
          this.nurseStations = res;
          if (res.length == 0) {
            this.alertService.warn("Please select facility for nursing stations.");
          }
          if(editObj!=undefined)
          {
            this.fetchData(editObj);
          }
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message)
            this.ng4LoadingSpinnerService.hide();
          });
    }
    onCompanySelect(item: any) {
      this.selectedFacilityItem=[];
      this.facilities=[];
      this.nurseStations=[];
      this.selectedNurseStationItem=[];
      this.companybedconfigs=[];
      this.getCompanyDetailsByID(item.Company_Id);
    }
    onCompanyDeSelect(item: any) {
      this.getCompanyDetailsByIDDeselect(0);
      this.nurseStations=[];
      this.selectedFacilityItem=[];
    }
    onFacilitySelect(item: any) {
      this.selectedNurseStationItem=[];
      this.nurseStations=[];
      this.getFacilityDetailsByID(item.Facility_Id);
      this.getCompanyBedConfigs(item.Facility_Id);
    }
    onFacilityDeSelect(item: any) {
      this.getFacilityDetailsByID(0);
      this.nurseStations=[];
      this.selectedFacilityItem=[];
      this.companybedconfigs=[];
    }
    onNurseStationSelect(item: any) {
      this.getNurseStationDetailsByID(item.NurseStation_Id);
      if(this.myform.value.facilityName!=undefined && this.myform.value.facilityName!=null && this.myform.value.facilityName.length!=0)
      {
      this.getCompanyBedConfigs(this.myform.value.facilityName[0].Facility_Id,item.NurseStation_Id);
      }
      else{
        this.getCompanyBedConfigs(item.NurseStation_Id);
      }
    }
    onNurseStationDeSelect(item: any) {
      this.getNurseStationDetailsByID(0);
      if(this.myform.value.facilityName!=undefined && this.myform.value.facilityName!=null && this.myform.value.facilityName.length!=0)
      {
      this.getCompanyBedConfigs(this.myform.value.facilityName[0].Facility_Id);
      }
      else{
        this.getCompanyBedConfigs();
      }
    }
    onFloorDeSelect(item:any)
    {
     this.checkMapping(item.item_id,"remove","Floor");
    }
    onFloorSelect(item:any)
    {
     if(this.mappingArray.length>0)
     {
      let exist=this.mappingArray.find(i=>i.item_Id==item.item_id);
      if(exist==undefined)
      {
        let previousExist=this.mappingArray.find(i=>i.item_text=="Floor");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Floor");
          this.mappingArray.splice(index,1);
        }
        this.checkMapping(item.item_id,"add","Floor");
      }
      else
      {
        this.alertService.warn("This sequence already selected");
        this.myform.patchValue({
          floorName:'',
        });
        let previousExist=this.mappingArray.find(i=>i.item_text=="Floor");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Floor");
          this.mappingArray.splice(index,1);
        }
      }
     } 
     else
     {
     this.checkMapping(item.item_id,"add","Floor");
     }
    }
    onWingSelect(item:any)
    {
      if(this.mappingArray.length>0)
     {
      let exist=this.mappingArray.find(i=>i.item_Id==item.item_id);
      if(exist==undefined)
      {
        let previousExist=this.mappingArray.find(i=>i.item_text=="Wing");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Wing");
          this.mappingArray.splice(index,1);
        }
        this.checkMapping(item.item_id,"add","Wing");
      }
      else
      {
        this.alertService.warn("This sequence already selected");
        this.myform.patchValue({
          wingName:'',
        });
        let previousExist=this.mappingArray.find(i=>i.item_text=="Wing");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Wing");
          this.mappingArray.splice(index,1);
        }
      }
     } 
     else
     {
     this.checkMapping(item.item_id,"add","Wing");
     }
    }
    onWingDeSelect(item:any)
    {
      this.checkMapping(item.item_id,"remove","Wing");
    }
    onBedSelect(item:any)
    {
      if(this.mappingArray.length>0)
     {
      let exist=this.mappingArray.find(i=>i.item_Id==item.item_id);
      if(exist==undefined)
      {
        let previousExist=this.mappingArray.find(i=>i.item_text=="Bed");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Bed");
          this.mappingArray.splice(index,1);
        }
        this.checkMapping(item.item_id,"add","Bed");
      }
      else
      {
        this.alertService.warn("This sequence already selected");
        this.myform.patchValue({
          bedName:'',
        });
        let previousExist=this.mappingArray.find(i=>i.item_text=="Bed");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Bed");
          this.mappingArray.splice(index,1);
        }
      }
     } 
     else
     {
     this.checkMapping(item.item_id,"add","Bed");
     }
    }
    onRoomSelect(item:any)
    {
      if(this.mappingArray.length>0)
     {
      let exist=this.mappingArray.find(i=>i.item_Id==item.item_id);
      if(exist==undefined)
      {
        let previousExist=this.mappingArray.find(i=>i.item_text=="Room");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Room");
          this.mappingArray.splice(index,1);
        }
        this.checkMapping(item.item_id,"add","Room");
      }
      else
      {
        this.alertService.warn("This sequence already selected");
        this.myform.patchValue({
          roomName:'',
        });
        let previousExist=this.mappingArray.find(i=>i.item_text=="Room");
        if(previousExist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_text=="Room");
          this.mappingArray.splice(index,1);
        }
      }
     } 
     else
     {
     this.checkMapping(item.item_id,"add","Room");
     }
    }
    onRoomDeSelect(item:any)
    {
      this.checkMapping(item.item_id,"remove","Room");
    }
    onBedDeSelect(item:any)
    {
      this.checkMapping(item.item_id,"remove","Bed");
    }
    getCompanyDetailsByID(companyId: number) {
      this.ng4LoadingSpinnerService.show();
      this.selectedCompanyId = companyId;
      this.getFacilityDropData();
      this.ng4LoadingSpinnerService.hide();
    }
    getFacilityDropData(editObj?:any) {
      this.dataservice.get<Facility[]>(this.config.Emar_Facility_GetFacilitiesBycompanyId + this.selectedCompanyId)
        .subscribe(res => {
          this.facilities = res;
          if (res.length == 0) {
            this.alertService.warn("Company don’t have any facilities.");
            this.nurseStations = [];
          }
          if(editObj!=undefined)
          {
            this.getNurseStations(editObj);
          }
        },
          error => {
            this.alertService.error(error.message)
          });
  
    }
    getCompanyDetailsByIDDeselect(companyId: number) {
      
      this.facilities=[];
      this.selectedCompanyId = companyId;
      this.nurseStations=[];
      this.selectedFacilityItem=[];
      this.selectedNurseStationItem=[];
    }
    getFacilityDetailsByID(facilityId: number) {
      this.selectedFacilityId = facilityId;
      this.getNurseStations();
    }
    getNurseStationDetailsByID(nursestationId: number) {
      this.selectedCompanyId = this.myform.value.companyName.length == 0 ? 0 : this.myform.value.companyName[0].Company_Id;
      this.selectedFacilityId = this.myform.value.facilityName.length == 0 ? 0 : this.myform.value.facilityName[0].Facility_Id;
      this.selectedNStationId = nursestationId;
    }
    getCompanyBedConfigs(facilityId?:number, nursestationId?: number) {
      let url=facilityId!=undefined && nursestationId!=undefined?this.config.Emar_Company_GetCompanyToBedMapGrid+ facilityId +"/" + nursestationId:facilityId!=undefined && nursestationId==undefined?this.config.Emar_Company_GetCompanyToBedMapGrid + facilityId:this.config.Emar_Company_GetCompanyToBedMapGrid;
      //nursestationId!=undefined?(this.config.Emar_Company_GetCompanyToBedMapGrid + nursestationId):this.config.Emar_Company_GetCompanyToBedMapGrid
      this.dataservice.get<any[]>(url)
        .subscribe(res => {
          this.companybedconfigs = res;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
  
        });
    }
    insertUpdatenurseStationhierarchy()
    {
      this.ng4LoadingSpinnerService.show();
      this.NurseStationHierarchyObj = {
        NSHierarchy_Id: this.nsHierarchy,
        NurseStation_Id: this.myform.value.nursestationName[0].NurseStation_Id,
        FloorPrior: this.myform.value.floorName!=null&& this.myform.value.floorName!=undefined && this.myform.value.floorName.length!=0? this.myform.value.floorName[0].item_id:0,
        WingPrior: this.myform.value.wingName!=null&& this.myform.value.wingName!=undefined && this.myform.value.wingName.length!=0? this.myform.value.wingName[0].item_id:0,
        RoomPrior: this.myform.value.roomName!=null&& this.myform.value.roomName!=undefined && this.myform.value.roomName.length!=0? this.myform.value.roomName[0].item_id:0,
        BedPrior: this.myform.value.bedName!=null&& this.myform.value.bedName!=undefined && this.myform.value.bedName.length!=0? this.myform.value.bedName[0].item_id:0,
        CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        CreatedDate: new Date().toISOString(),
      };
  
      this.dataservice.post(this.config.Emar_CompanyBedMapping_InsertUpdateNurseStationhierarchyMaster, this.NurseStationHierarchyObj)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res == 1) {
            this.alertService.success("Save successful");
            this.getCompanyBedConfigs(this.myform.value.facilityName[0].Facility_Id,this.NurseStationHierarchyObj.NurseStation_Id);
            this.myform.patchValue({
              floorName:'',
              wingName:'',
              roomName:'',
              bedName:'',
            });
            this.mappingArray=[];
            this.saveFlag=true;
          }
          this.ng4LoadingSpinnerService.hide();
  
        }, error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
    }
    resetScreen()
    {
      this.myform.reset();
      this.mappingArray=[];
      this.getCompanyBedConfigs();
    }
    getHistoryById(NSHierarchyId: number) {
      this.auditTable = {
        "tableName": "CompanyToBedMap",
        "recordId": NSHierarchyId
      }
      this.modalHistoryIsOpen = true;
  
    }
    closeModel() {
      this.modalHistoryIsOpen = false;
    }
    getCompanyToBedDetailsByID(id:any)
    {
      this.dataservice.get<any>(this.config.Emar_Company_GetNurseStationHierarchyDetailsById + id)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res != null) {
            this.selectedCompanyId=res.CompanyId;
            this.selectedFacilityId=res.FacilityId;
            this.selectedNStationId=res.NurseStation_Id;
            this.getFacilityDropData(res);
          }
          this.ng4LoadingSpinnerService.hide();
  
        }, error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
    }
    fetchData(res:any)
    {
      this.saveFlag=true;
      this.mappingArray=[];
      this.selectedCompanyItem=[];
      this.selectedFacilityItem=[];
      this.selectedNurseStationItem=[];
      this.selectedFloorItem=[];
      this.selectedWingItem=[];
      this.selectedRoomItem=[];
      this.selectedBedItem=[];
      if(res.CompanyId!=null)
      {
        let exist=this.companies.find(c=>c.Company_Id==res.CompanyId);
        if(exist!=undefined)
        this.selectedCompanyItem.push(exist);
      }
      if(res.FacilityId!=null)
      {
        let exist=this.facilities.find(f=>f.Facility_Id==res.FacilityId);
        if(exist!=undefined)
        this.selectedFacilityItem.push(exist);
      }
      if(res.NurseStation_Id!=null)
      {
        let exist=this.nurseStations.find(n=>n.NurseStation_Id==res.NurseStation_Id);
        if(exist!=undefined)
        this.selectedNurseStationItem.push(exist);
      }
      if(res.FloorPrior!=null && res.FloorPrior!=0)
      {
        let exist=this.records.find(n=>n.item_id==res.FloorPrior);
        if(exist!=undefined)
        {
        this.selectedFloorItem.push(exist);
        this.checkMapping(res.FloorPrior,"add","Floor");
        }
      }
      if(res.WingPrior!=null && res.WingPrior!=0)
      {
        let exist=this.records.find(w=>w.item_id==res.WingPrior);
        if(exist!=undefined)
        {
        this.selectedWingItem.push(exist);
        this.checkMapping(res.WingPrior,"add","Wing");
        }
      }
      if(res.RoomPrior!=null && res.RoomPrior!=0)
      {
        let exist=this.records.find(w=>w.item_id==res.RoomPrior);
        if(exist!=undefined)
        {
        this.selectedRoomItem.push(exist);
        this.checkMapping(res.RoomPrior,"add","Room");
        }
      }
      if(res.BedPrior!=null && res.BedPrior!=0)
      {
        let exist=this.records.find(w=>w.item_id==res.BedPrior);
        if(exist!=undefined)
        {
        this.selectedBedItem.push(exist);
        this.checkMapping(res.BedPrior,"add","Bed");
        }
      }
      this.myform.patchValue({
        company:this.selectedCompanyItem,
        facilityName:this.selectedFacilityItem,
        nursestationName:this.selectedNurseStationItem,
        floorName:this.selectedFloorItem,
        wingName:this.selectedWingItem,
        roomName:this.selectedRoomItem,
        bedName:this.selectedBedItem
      });
      //this.nsHierarchy=res.NSHierarchy_Id
    }
    checkMapping(id:any,eventType:any,eventFor:string)
    {
      debugger
      this.saveFlag=true;
      if(eventType=="add")
      {
      if(this.mappingArray.length>0)
      {
        let exist=this.mappingArray.find(i=>i.item_Id==id);
        if(exist==undefined)
        {
          let obj={
            item_Id:id,
            item_text:eventFor
          }
          this.mappingArray.push(obj);
        }
        if(this.mappingArray.length>0)
        {
        this.saveFlag=false;
        }
        // if(this.mappingArray.length==2 && this.mappingArray.indexOf(1) != -1 &&this.mappingArray.indexOf(2) != -1)
        // {
        //   this.saveFlag=false;
        // }
        // if(this.mappingArray.length==3 && this.mappingArray.indexOf(1) != -1 &&this.mappingArray.indexOf(2) != -1 && this.mappingArray.indexOf(3) != -1)
        // {
        //   this.saveFlag=false;
        // }
        // if(this.mappingArray.length==4 && this.mappingArray.indexOf(1) != -1 &&this.mappingArray.indexOf(2) != -1 && this.mappingArray.indexOf(3) != -1 &&this.mappingArray.indexOf(4) != -1)
        // {
        //   this.saveFlag=false;
        // }
      }
      else
      {
        let obj={
          item_Id:id,
          item_text:eventFor
        }
        this.mappingArray.push(obj);
        if(this.mappingArray.length>0)
        {
        this.saveFlag=false;
        }
      }
    }
    else if(eventType=="remove")
    {
      debugger
      if(this.mappingArray.length>0)
      {
        let exist=this.mappingArray.find(i=>i.item_Id==id);
        if(exist!=undefined)
        {
          let index=this.mappingArray.findIndex(i=>i.item_Id==id);
          this.mappingArray.splice(index,1);
        }
        if(this.mappingArray.length>0)
        {
        this.saveFlag=false;
        }
        // if(this.mappingArray.length==2 && this.mappingArray.indexOf(1) != -1 &&this.mappingArray.indexOf(2) !=-1)
        // {
        //   this.saveFlag=false;
        // }
        // if(this.mappingArray.length==3 && this.mappingArray.indexOf(1) != -1 &&this.mappingArray.indexOf(2) != -1 &&this.mappingArray.indexOf(3) != -1)
        // {
        //   this.saveFlag=false;
        // }
        // if(this.mappingArray.length==4 && this.mappingArray.indexOf(1) != -1 &&this.mappingArray.indexOf(2) != -1 &&this.mappingArray.indexOf(3) != -1 &&this.mappingArray.indexOf(4) != -1)
        // {
        //   this.saveFlag=false;
        // }
      }
    }
    }
}
