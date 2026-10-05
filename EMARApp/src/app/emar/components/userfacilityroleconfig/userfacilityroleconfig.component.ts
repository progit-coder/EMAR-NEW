import { UserRoleConfigIds, UserRoleFacilityConfigCustomEntity } from './../../../models/role.model';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RoleMaster, RoleConfigMaster, RoleFacility, UserRoleFacilityConfigEntity, RoleNurseStation, RoleDrop } from '../../../models/role.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserModel, UserDrop } from '../../../models/user.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-userfacilityroleconfig',
  templateUrl: './userfacilityroleconfig.component.html',
  styleUrls: ['./userfacilityroleconfig.component.css'],
  providers: [DataService, APIConfiguration]
})
export class UserfacilityroleconfigComponent implements OnInit {
  records: Observable<any[]>;
  headers: string[];
  public template;
  public dropdownList = [];
  public roleMaster: RoleMaster[];
  public roleDrop: RoleDrop[];
  public usersList: UserModel[];
  public userDrop: UserDrop[];
  public roleFacilty: RoleFacility[];
  public roleNurseStation: RoleNurseStation[];
  private roleUserObj: UserRoleFacilityConfigCustomEntity;
  public roleUserObjData: UserRoleFacilityConfigEntity[] = [];
  public filterIds: UserRoleConfigIds;
  public roleId: number = 0;
  public userDisplayName: string;
  public facilityName: string;
  public nurseStation: string;
  public role: string;
  public arUsers: any[];
  public arFacilities: any[];
  public arNurseStations: any[];
  private users: string = "";
  public facilities: string = "";
  public nstations: string = "";
  disabled = false;
  ShowFilter = true;
  limitSelection = false;
  dropdownSettings_Facility: any = {};
  dropdownSettings_NurseStation: any = {};
  dropdownSettings_Users: any = {};
  dropdownSettings_Roles: any = {};
  myform: FormGroup;
  errorMessage: string;
  public selectedrItems = [];
  public selecteduItems = [];
  public selectedfItems = [];
  public selectednItems = [];
  public selectedNSList = [];
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  public excelFileData: any[] = [];
  disableButton = false;
  public selectedRoleList = [];
  public record: any;
  public oldRole: number = 0;
  public oldUser: number = 0;
  public oldFacility: number = 0;
  public modalHistoryIsOpen:boolean=false;
  public roleUserObjDataList:any[]=[];
  public firstCreated: any;
  public indexValue:number;
  constructor(private dataservice: DataService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, public sharedService: SharedService) { }
  ngOnInit() {
    this.template = this.dataservice.template;
    this.myform = new FormGroup({
      roleDesc: new FormControl('', Validators.required),
      ddluser: new FormControl('', Validators.required),
      chkstatus: new FormControl('1'),
      ddlfacility: new FormControl('', Validators.required),
      ddlnursestation: new FormControl('', Validators.required),
    });
    this.getRoleMasterData();
    this.getUsers();
    this.getRolefacilityMasterData();
    this.userActivity();
    this.getUserRoleFacilityConfigGridInfo(null);
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.UserRoleConfig, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getRoleMasterData() {
    this.dataservice.get<RoleDrop[]>(this.config.Emar_RoleMaster_GetRoleDropData)
      .subscribe(res => this.roleDrop = res, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
      });
  }

  GetUserRoleFacilityConfigByID() {

    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<UserRoleFacilityConfigEntity[]>(this.config.Emar_RoleMaster_GetUserRoleFacilityConfigByID + this.myform.value.roleDesc.Role_Id)
      .subscribe(res => {
        this.roleUserObjData = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getRolefacilityMasterData() {
    this.dataservice.get<RoleFacility[]>(this.config.Emar_Facility_GetAllActiveFacilityDropData)
      .subscribe(res => this.roleFacilty = res, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
      });

    this.dropdownSettings_Facility = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "FacilityName",
      text: "Select Facilities",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_NurseStation = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Select Nurse Stations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      noDataAvailablePlaceholderText: "Please Select Facility",
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Roles = {
      singleSelection: true,
      idField: "Role_Id",
      textField: "Role_Desc",
      text: "Select Role",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    }
  }
  getNurseStationByFacilityId(data: any) {

    this.record = data;
    if (data.Facility_Id != 0) {
      this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetAllNurseStationsNew + data.Facility_Id)
        .subscribe(res => {
          
          this.roleNurseStation = res;
          this.fetchData(this.record);
          if (res.length == 0) {
            this.roleNurseStation = [];
            this.selectednItems = [];
            this.alertService.warn("Please Select Facility For NurseStations.");
          }
          else {
            this.dropdownSettings_NurseStation = {
              singleSelection: false,
              idField: "NurseStation_Id",
              textField: "NurseStation_Name",
              text: "Select Nurse Stations",
              selectAllText: "Select All",
              unSelectAllText: "UnSelect All",
              itemsShowLimit: 1,
              noDataAvailablePlaceholderText: "Please Select Facility",
              allowSearchFilter: this.ShowFilter
            };
          }
        }, error => {
          this.alertService.error(error.message)
        });
    }
    else {
      this.roleNurseStation = [];
      this.selectednItems = [];
      this.alertService.warn("Please Select Facility For NurseStations.");
    }
  }
  getRoleNurseStationMasterData(facilityId: number) {
    if (facilityId != 0) {

      this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetAllActiveNurseStationDropData + facilityId)
        .subscribe(res => {

          this.roleNurseStation = res
          if (res.length == 0) {
            this.roleNurseStation = [];
            this.selectednItems = [];
            this.myform.patchValue({
              ddlnursestation: this.selectednItems,
            })
            this.alertService.warn("Please Select Facility For NurseStations.");
          }
          else {
            this.dropdownSettings_NurseStation = {
              singleSelection: false,
              idField: "NurseStation_Id",
              textField: "NurseStation_Name",
              text: "Select Nurse Stations",
              selectAllText: "Select All",
              unSelectAllText: "UnSelect All",
              itemsShowLimit: 1,
              noDataAvailablePlaceholderText: "Please Select Facility",
              allowSearchFilter: this.ShowFilter
            };
          }
        }, error => {
          this.alertService.error(error.message)
        });
    }
    else {
      this.roleNurseStation = [];
      this.selectednItems = [];
      this.alertService.warn("Please Select Facility For NurseStations.");
    }
  }
  getUsers() {
    this.dataservice.get<UserDrop[]>(this.config.Emar_UserMaster_GetUserDropData)
      .subscribe(res => this.userDrop = res, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
      });

    this.dropdownSettings_Users = {

      singleSelection: true,
      idField: "User_Id",
      textField: "User_DisplayName",
      text: "Select Users",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: this.ShowFilter
    };
  }
  InsertUserRoleFacilityConfigData() {
    this.nstations='';
    this.ng4LoadingSpinnerService.show();
    this.disableButton = !this.disableButton;
    this.ng4LoadingSpinnerService.show();
    this.arNurseStations = this.myform.value.ddlnursestation;
    this.arNurseStations.forEach(element => {
      this.nstations += element.NurseStation_Id + ",";
    });
    this.nstations = this.nstations.substring(0, this.nstations.length - 1);

    this.roleUserObj = {
      UserRole_Id: this.roleId,
      Role_Id: this.myform.value.roleDesc[0].Role_Id,
      User_Id: this.myform.value.ddluser[0].User_Id,
      Facility_Id: this.myform.value.ddlfacility[0].Facility_Id,
      UserRole_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      UserRole_CreatedDate: new Date().toISOString(),
      NurseStations: this.nstations,
      OldRole_Id: this.oldRole,
      OldUser_Id: this.oldUser,
      OldFacility_Id: this.oldFacility,

    };

    this.dataservice.post(this.config.Emar_RoleMaster_InsertUserFacilityRoleConfig, this.roleUserObj)
      .subscribe(res => {
        if (res == 1) {
          this.alertService.success("Save successful");
          this.getUserRoleFacilityConfigGridInfo(null);
          this.myform.reset();
          this.myform.patchValue({
            chkstatus: '1'
          });
          //this.GetUserRoleFacilityConfigByID();
          this.roleId = 0;
          this.oldRole = 0;
          this.oldUser = 0;
          this.oldFacility = 0
        }
        else if (res == 2)
          this.alertService.warn("Another Role is already assigned to this user");
        else
          this.alertService.error("Something went wrong.Please try again");
        this.ng4LoadingSpinnerService.hide();

      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
          this.users = "";
          this.facilities = "";
          this.nstations = "";
          this.arUsers = [];
          this.arFacilities = [];
          this.arNurseStations = [];
        });
    //this.reSet();
  }
  reSet() {
    this.myform.reset();
    this.myform.patchValue({
      chkstatus: '1'
    });
    // this.myform.patchValue({
    //   roleDesc: '',
    //   ddluser: [],
    //   status: '1',
    //   ddlfacility: []
    // });
    this.users = "";
    this.facilities = "";
    this.nstations = "";
    this.arUsers = [];
    this.arFacilities = [];
    this.arNurseStations = [];
    this.disableButton = !this.disableButton;
    this.roleId = 0;
    this.getUserRoleFacilityConfigGridInfo(null);
  }
  getRoleDetailsByID(roleId: number, userId: number, facilityId: number) {
    this.ng4LoadingSpinnerService.show()
    this.dataservice.get<any>(this.config.Emar_RoleMaster_GetUserRoleFacilityConfigByRoleID + roleId + "/" + userId + "/" + facilityId)
      .subscribe(res => {
        this.getNurseStationByFacilityId(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    window.scroll(0, 0);
  }
  fetchData(fetchRecord: any) {

    this.roleId = 1;
    this.oldRole = fetchRecord.Role_Id;
    this.oldUser = fetchRecord.User_Id;
    this.oldFacility = fetchRecord.Facility_Id;
    this.selecteduItems = [];
    this.selectedfItems = [];
    this.selectednItems = [];
    this.selectedrItems = [];
    this.selecteduItems.push(this.userDrop.filter(u => u.User_Id == fetchRecord.User_Id)[0]);
    this.selectedrItems.push(this.roleDrop.filter(e => e.Role_Id == fetchRecord.Role_Id)[0]);
    this.selectedfItems.push(this.roleFacilty.filter(f => f.Facility_Id == fetchRecord.Facility_Id)[0]);
    this.selectedNSList = fetchRecord.NurseStations.split(',');
    if (this.selectedNSList.length > 0) {
      for (let i = 0; i < this.selectedNSList.length; i++) {
        this.selectednItems.push(this.roleNurseStation.filter(r => r.NurseStation_Id == parseInt(this.selectedNSList[i]))[0]);
      }
      return this.selectednItems;
    }

    this.myform.patchValue({
      ddluser: this.selecteduItems,
      roleDesc: this.selectedrItems,
      ddlfacility: this.selectedfItems,
      ddlnursestation: this.selectednItems,

    });

  }
  //Download Excel Code
  excelDownload() {
    if (this.roleUserObjData.length != 0) {
      this.roleUserObjData.forEach((x) => {
        var row = {
          "User": x.UserName,
          "Role": x.RoleName,
          "Company": x.CompanyName,
          "Facility": x.FacilityName,
          "Nurse Station": x.NurseStationNames.replace(/,/g, ':'),
          // "Created By": x.User_DisplayName,
          // "Created Date": x.UserRole_CreatedDate.substring(0, 10)
        };
        this.excelFileData.push(row);
      });
      var csvData = this.ConvertToCSV(this.excelFileData);
      var a = document.createElement("a");
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      var blob = new Blob([csvData], { type: 'text/csv' });
      var url = window.URL.createObjectURL(blob);
      a.href = url;
      var x: Date = new Date();
      var link: string = "UserRoleConfig_" + x.getMonth() + "_" + x.getDay() + '.csv';
      a.download = link.toLocaleLowerCase();
      a.click();

    }
    else {
      this.alertService.warn('Please select Role to download data.');
    }
  }
  // convert Json to CSV data
  ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';
    var row = "";

    for (var index in objArray[0]) {
      //Now convert each value to string and comma-separated
      row += index + ',';
    }
    row = row.slice(0, -1);
    //append Label row with line break
    str += row + '\r\n';

    for (var i = 0; i < array.length; i++) {
      var line = '';
      for (var index in array[i]) {
        if (line != '') line += ','

        line += array[i][index];
      }
      str += line + '\r\n';
    }
    return str;
  }
  getUserRoleFacilityConfigGridInfo(filters: UserRoleConfigIds) {
    this.ng4LoadingSpinnerService.show();
    if (filters == null) {
      this.filterIds = {
        User_Id: 0,
        Role_Id: 0,
        Facility_Id: 0
      }
    }
    else
      this.filterIds = filters;
    this.dataservice.post(this.config.Emar_RoleMaster_UserRoleFacilityConfigGrid, this.filterIds).subscribe(res => {
      this.roleUserObjData = res;
      this.roleUserObjDataList=res.filter(r=>r.User_Status==1);
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
  onRoleSelect(item: any) {
    this.filterIds.Role_Id = item.Role_Id;
    this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onRoleDeSelect(item: any) {
    this.filterIds.Role_Id = 0;
    this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onUserSelect(item: any) {
    this.filterIds.User_Id = item.User_Id;
    this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onUserDeSelect(item: any) {
    this.filterIds.User_Id = 0;
    this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onFacilitySelect(item: any) {
    this.filterIds.Facility_Id = item.Facility_Id;
    this.myform.patchValue({ddlnursestation:''});
    this.getRoleNurseStationMasterData(item.Facility_Id);
    this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onFacilityDeSelect(item: any) {
    this.filterIds.Facility_Id = 0;
    this.getRoleNurseStationMasterData(0);
    this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }

  onNurseStationSelect(item: any) {
    this.nstations="";
    //this.getUserRoleFacilityConfigGridInfo();
  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestation = item;
    //this.getUserRoleFacilityConfigGridInfo();
  }
  onNurseStationDeSelect(item: any) {
    this.nstations="";
    //this.getUserRoleFacilityConfigGridInfo();
  }
  onNurseStationDeSelectAll(item: any) {
    this.myform.value.ddlnursestation.length = 0;
    //this.getUserRoleFacilityConfigGridInfo();
  }
  getHistoryById(userId: number,roleId:number,facilityId:number) {
    this.dataservice.get<any>(this.config.Emar_AuditTables_GetUserRoleConfigAudit + userId + "/" + roleId+"/"+facilityId)
    .subscribe(res => {
      this.indexValue = res.Records.findIndex(r => r.ColumnName == null && r.OldValue == null && r.NewValue == null);
        this.firstCreated = res.Records[this.indexValue];
        this.headers = ["ColumnName", "OldValue", "NewValue", "UpdatedBy", "UpdatedOn"];
        this.records = res.Records.filter(r => r.ColumnName != null && r.OldValue != null && r.NewValue != null);
    this.modalHistoryIsOpen = true;
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
}
