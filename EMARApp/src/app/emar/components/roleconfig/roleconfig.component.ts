import { Screen, RoleScreens } from './../../../models/role.model';
import { AlertService } from './../../../_services/alert.service';
import { DataService } from './../../../services/shared/dataservice.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RoleMaster, RoleConfigMaster, RoleDrop } from '../../../models/role.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
@Component({
  selector: 'app-roleconfig',
  templateUrl: './roleconfig.component.html',
  styleUrls: ['./roleconfig.component.css'],
  providers: [DataService, APIConfiguration,ExceldownloadService]
})
export class RoleconfigComponent implements OnInit {
  public dropdownList = [];
  public selectedItems = [];
  public selectedItems1 = [];
  public dropdownSettings = {};
  public item_id: number;
  public Access: any[];
  public selectedRoleItems = [];
  public read: number = 0;
  public write: number = 0;
  public printPdf: number = 0;
  public printExcel: number = 0;
  public screenList = [];
  dropdownSettings_Screens: any = {};
  public template;
  public roleMaster: RoleMaster[];
  public roleDrop: RoleDrop[];
  roleConfig: any[] = [];
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  myform: FormGroup;
  roleObj: RoleConfigMaster;
  roleConfigID: number = 0;
  dropdownSettings_Roles: any = {};
  public url: string;
  public screens: RoleConfigMaster[];
  searchText: string = '';
  p: number = 1;
  disabled = false;
  public Ids: RoleScreens;
  rcScreens: any[];
  screenString: string = '';
  gridPagination = this.config.gridPagination;
  public inactivecheckbox: boolean = false;
  public roleConfigList: RoleConfigMaster[] = [];
  public excelFileData: any[] = [];
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  pageConfig: {};
  constructor(private dataservice: DataService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, public sharedService: SharedService, private dateFormatPipe: CustomdatePipe,private exceldownload: ExceldownloadService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("RoleConfiguration");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.dropdownList = [
      { item_id: 1, item_text: 'Read' },
      { item_id: 2, item_text: 'Write' },
      { item_id: 3, item_text: 'PrintPdf' },
      { item_id: 4, item_text: 'PrintExcel' },
    ];
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
    this.dropdownSettings_Screens = {
      singleSelection: false,
      idField: "Screen_Id",
      textField: "Screen_Desc",
      text: "screens",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      noDataAvailablePlaceholderText: 'Please Select Role',
    };

    this.myform = new FormGroup({
      roleDesc: new FormControl('', Validators.required),
      screen: new FormControl('', Validators.required),
      Accessdata: new FormControl(''),
      status: new FormControl('1'),
    });
    this.dropdownSettings_Roles = {
      singleSelection: true,
      idField: "Role_Id",
      textField: "Role_Desc",
      text: "Select",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.getRoleMasterData();
    //  this.getRoleScreensData();
    this.getRoleConfigByIDData(0);
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.RoleConfig, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getRoleMasterData() {
    this.dataservice.get<RoleDrop[]>(this.config.Emar_Role_GetSubRolesList)
      .subscribe(res => {
        this.roleDrop = res
      }
        , error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message)
        });
  }
  getRoleConfigByIDData(roleId: any) {
    this.ng4LoadingSpinnerService.show();
    let role = roleId == "" ? 0 : roleId;
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.url = this.config.Emar_RoleConfigMaster_GetRoleConfigsGrid + "/" + userId + "/" + roleId;
    this.dataservice.get<RoleConfigMaster[]>(this.url)
      .subscribe(res => {
        if (res.length > 0) {
          this.roleConfig = res;
          this.roleConfigList = res.filter(r => r.RoleConfig_Status == 1);
        }
        else {
          this.roleConfig = [];
          this.roleConfigList = [];
        }
        if (this.myform.value.screen != '' && this.myform.value.screen != null) {
          this.getRoleConfigDetailsByScreenID(this.myform.value.screen);
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    this.selectedItems = [];
  }
  onScreenSelect(item: any) {
    this.getRoleConfigDetailsByScreenID(this.myform.value.screen);
  }
  onScreenSelectAll(item: any) {
    this.myform.value.screen = item;
    this.getRoleConfigDetailsByScreenID(this.myform.value.screen);
  }
  onScreenDeSelect(item: any) {
    this.getRoleConfigDetailsByScreenID(this.myform.value.screen);
  }
  onScreenDeSelectAll(item: any) {
    this.myform.value.screen.length = 0;
    this.getRoleConfigDetailsByScreenID(this.myform.value.screen);
  }
  getRoleConfigDetailsByScreenID(screenId: number) {
    if (this.myform.value.roleDesc != '') {
      this.ng4LoadingSpinnerService.show();
    }

    let screenfilter = "";
    if (this.myform.value.roleDesc != '' && this.myform.value.roleDesc != null) {
      let loggedInUserRoleId: number = this.persistanceService.get(this.config.loggedInUserRoleKey);
      if (this.myform.value.roleDesc[0].Role_Id == loggedInUserRoleId && loggedInUserRoleId != 1)
        this.alertService.warn('Cannot edit the Role which is assigned to you. Your Superior can edit this.');
      else {
        this.myform.value.screen.forEach(element => { screenfilter += element.Screen_Id + ',' });
        screenfilter = screenfilter.substring(0, screenfilter.length - 1);
        this.Ids = {
          Role_Id: this.myform.value.roleDesc[0].Role_Id,
          Screen_Id: screenfilter
        }
        this.dataservice.post(this.config.Emar_RoleMaster_GetRoleConfigDetailsByScreenId, this.Ids)
          .subscribe(res => {
            if (res.length > 0) {
              this.roleConfig = res;
              this.roleConfigList = res.filter(r => r.RoleConfig_Status == 1);
            }
            else {
              this.roleConfig = [];
              this.roleConfigList = [];
            }
            this.ng4LoadingSpinnerService.hide();
          }, error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
      }
    }
    else {
      this.alertService.warn('Please select Role Description');
    }
  }
  /*addRoleConfigToArray() {
    this.roleObj = new RoleConfigMaster();
    this.roleObj.RoleConfig_Id = this.roleConfigID;
    this.roleObj.Role_Id = this.myform.value.roleDesc;
    this.roleObj.Screen_Id = this.myform.value.screen;
    this.roleObj.AccessRead = (this.myform.value.read == true ? 1 : 0);
    this.roleObj.AccessWrite = (this.myform.value.write == true ? 1 : 0);
    this.roleObj.PrintExcel = (this.myform.value.printExcel == true ? 1 : 0);
    this.roleObj.PrintPdf = (this.myform.value.printPdf == true ? 1 : 0);
    this.roleObj.RoleConfig_Status = 1;
    this.roleObj.RoleConfig_CreatedBy = 1;
    this.roleObj.RoleConfig_CreatedDate = new Date().toISOString();
 
    this.roleConfig.push(this.roleObj);
  }*/

  saveRoleConfigData() {
    this.ng4LoadingSpinnerService.show();
    this.screenString = '';
    this.rcScreens = this.myform.value.screen;
    this.rcScreens.forEach(element => { this.screenString += element.Screen_Id + ',' });
    this.screenString = this.screenString.substring(0, this.screenString.length - 1);
    this.Access = this.myform.value.Accessdata == "" ? this.myform.value.Accessdata = [] : this.myform.value.Accessdata;
    this.write = 0;
    this.read = 0;
    this.printPdf = 0;
    this.printExcel = 0;

    if (this.Access.filter(e => e.item_id === 1).length) {
      this.read = 1;
    }
    if (this.Access.filter(e => e.item_id === 2).length) {
      this.write = 1;
    }
    if (this.Access.filter(e => e.item_id === 3).length) {
      this.printPdf = 1;
    }
    if (this.Access.filter(e => e.item_id === 4).length) {
      this.printExcel = 1;
    }
    // if (this.read == 0 && this.write == 0 && this.printPdf == 0 && this.printExcel == 0) {
    // }
    // else {
    //   if (this.selectedItems.filter(e => e.item_id === 1).length == 0)
    //     this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 1)[0]);
    //   this.myform.patchValue({
    //     Accessdata: this.selectedItems,
    //   });
    // }


    this.roleObj = new RoleConfigMaster();
    this.roleObj.RoleConfig_Id = this.roleConfigID;
    this.roleObj.Role_Id = this.myform.value.roleDesc[0].Role_Id;
    this.roleObj.Screen_Id = 0;
    this.roleObj.Screens = this.screenString;
    this.roleObj.AccessRead = this.read;
    this.roleObj.AccessWrite = this.write;
    this.roleObj.PrintExcel = this.printExcel;
    this.roleObj.PrintPdf = this.printPdf;
    this.roleObj.RoleConfig_Status = (this.myform.value.status == true ? 1 : 0);
    this.roleObj.RoleConfig_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
    this.roleObj.RoleConfig_CreatedDate = new Date().toISOString();
    this.dataservice.post(this.config.Emar_RoleConfigMaster_InsertRoleConfigMaster, this.roleObj)
      .subscribe(res => {

        this.alertService.success("Save successful");
        this.ng4LoadingSpinnerService.hide();
        this.reSet();
        this.inactivecheckbox = false;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  getRoleScreensData(RoleId: any) {
    this.dataservice.get<RoleConfigMaster[]>(this.config.Emar_RoleConfigMaster_GetSubRolesScreenList + RoleId)
      .subscribe(res => {
        this.screens = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message)
      });
  }
  reSet() {
    this.ng4LoadingSpinnerService.show();
    //this.myform.reset();
    this.read = 0;
    this.write = 0;
    this.printPdf = 0;
    this.printExcel = 0;
    if (this.myform.value.roleDesc != "")
      this.getRoleConfigByIDData(this.myform.value.roleDesc[0].Role_Id);
    this.screenString = '';
    this.myform.patchValue({
      screen: '',
      Accessdata: '',
      status: '1',

    });
    this.ng4LoadingSpinnerService.hide();
  }
  onRoleSelect(item: any) {
    this.screens = [];
    this.selectedItems1 = [];
    this.myform.patchValue({
      screen: '',
    });
    this.getRoleConfigByIDData(item.Role_Id);
    this.getRoleScreensData(item.Role_Id);
  }
  onRoleDeSelect(item: any) {
    this.screens = [];
    this.selectedItems1 = [];
    this.myform.patchValue({
      screen: '',
    });
    this.getRoleConfigByIDData(0);

  }

  //Download Excel Code
  excelDownload() {
    let roleId=this.myform.value.roleDesc==null||this.myform.value.roleDesc==undefined||this.myform.value.roleDesc.length==0?0: this.myform.value.roleDesc[0].Role_Id;
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Reports_GetRoleConfigExcel + userId +"/"+ roleId)
    .subscribe(res => {
      this.exceldownload.excelDownload(res, "Role Config")
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.error(error.message)
    });
  }
  
  GetRoleConfigByID(roleId:number, roleConfigID: number) {
    let loggedInUserRoleId: number = this.persistanceService.get(this.config.loggedInUserRoleKey);
    if (roleId == loggedInUserRoleId && loggedInUserRoleId != 1)
      this.alertService.warn('Cannot edit the screens which are assigned to you. Your Superior can edit this.');
    else {
      this.ng4LoadingSpinnerService.show();
      window.scroll(0, 0);
      this.dataservice.get<RoleConfigMaster>(this.config.Emar_Role_GetRoleConfigDetailsByID + roleConfigID)
        .subscribe(res => {
          this.getRoleScreensByRoleId(res);
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  getRoleScreensByRoleId(data: any) {
    this.dataservice.get<RoleConfigMaster[]>(this.config.Emar_RoleConfigMaster_GetSubRolesScreenList + data.Role_Id)
      .subscribe(res => {
        this.screens = res;
        this.fetchData(data);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message)
      });
  }
  fetchData(res: RoleConfigMaster) {
    this.selectedItems = [];
    this.selectedItems1 = [];
    this.selectedRoleItems = [];
    this.selectedItems1.push(this.screens.filter(e => e.Screen_Id == res.Screen_Id)[0]);
    this.selectedRoleItems.push(this.roleDrop.filter(r => r.Role_Id == res.Role_Id)[0]);
    this.read = res.AccessRead;
    this.write = res.AccessWrite;
    this.printPdf = res.PrintPdf;
    this.printExcel = res.PrintExcel;

    if (this.read == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 1)[0]);
    }
    if (this.write == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 2)[0]);
    }
    if (this.printPdf == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 3)[0]);
    }
    if (this.printExcel == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 4)[0]);
    }
    this.myform.patchValue({
      roleDesc: this.selectedRoleItems,
      screen: this.selectedItems1,
      Accessdata: this.selectedItems,
      status: res.RoleConfig_Status,

    });
    this.roleConfigID = res.RoleConfig_Id;
  }
  getHistoryById(roleConfig_Id: number) {
    this.auditTable = {
      "tableName": "RoleConfig",
      "recordId": roleConfig_Id
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      //  this.inactivecheckbox=true;
      this.roleConfigList = this.roleConfig.filter(c => c.RoleConfig_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.roleConfigList = this.roleConfig.filter(c => c.RoleConfig_Status == 1);
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.roleConfigList.forEach(element => {
        element.RoleConfig_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.RoleConfig_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.RoleConfig_Status=1;
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
      item.RoleConfig_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.RoleConfig_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.RoleConfig_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.RoleConfig_Id == item.RoleConfig_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateRoleConfigStatus() {
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      let loggedInUserRoleId: number = this.persistanceService.get(this.config.loggedInUserRoleKey);
      if (this.selectedRecords.find(e => e.Role_Id == loggedInUserRoleId) && loggedInUserRoleId != 1)
        this.alertService.warn('Cannot edit the screens which are assigned to you. Your Superior can edit this.');
      else if (this.selectedRecords.find(e => e.Role_Id == 1))
        this.alertService.warn('Super Admin Role screens cannot be Inactive.');
      else {
        this.ng4LoadingSpinnerService.show();
        this.dataservice.post(this.config.Emar_RoleMaster_UpdateRoleConfigsStatus, this.selectedRecords)
          .subscribe(res => {
            if (res == 1) {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.success("Status Updated Successfully");
              this.UpdateStatus = true;
              this.inactivecheckbox = false;
              this.CheckAll = false;
              if (this.myform.value.roleDesc != "" && this.myform.value.roleDesc!=undefined && this.myform.value.roleDesc!=null && this.myform.value.roleDesc.length!=0)
              this.getRoleConfigByIDData(this.myform.value.roleDesc[0].Role_Id);
              else
              this.getRoleConfigByIDData(0);
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
  }
}
