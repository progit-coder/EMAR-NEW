import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { RoleMaster, RoleDrop } from '../../../models/role.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DefaultscreenComponent } from '../defaultscreen/defaultscreen.component';
@Component({
  selector: 'app-rolemaster',
  templateUrl: './rolemaster.component.html',
  styleUrls: ['./rolemaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class RolemasterComponent implements OnInit {
  public roleMaster: any[] = [];
  public template;
  myform: FormGroup;
  defaultScreenform: FormGroup;
  roleObj: RoleMaster;
  errorMessage: string;
  public url: string;
  private RoleID: number = 0;
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  auditTable: any;
  public roleList: any[] = [];
  public selectedScreenItems = [];
  dropdownSettings_Screen: any = {};
  public inactivecheckbox: boolean = false;
  public modalHistoryIsOpen: boolean = false;
  public screens: any[] = [];
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  dropdownSettings_Roles: any = {};
  public selectedrItems = [];
  public roleDrop: RoleDrop[];
  public statusbutton: any = 0;
  public isReadOnly: boolean = false;
  pageConfig: {};
  DefaultScreenpageConfig: {};
  public loggedInUserRoleId:number;
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private modalService: NgbModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("RoleMaster");
    this.DefaultScreenpageConfig =this.persistanceService.getPermissionsByScreen("DefaultScreen");
    if(this.DefaultScreenpageConfig ==undefined)
    {
      this.DefaultScreenpageConfig =0;
    }
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.loggedInUserRoleId= parseInt(this.persistanceService.get(this.config.loggedInUserRoleKey));
    this.ng4LoadingSpinnerService.show();
    this.myform = new FormGroup({
      roleDesc: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphabets)]),
      screen: new FormControl('', Validators.required),
      status: new FormControl('1'),
      superiorRole: new FormControl('', Validators.required),
    });
    this.dropdownSettings_Screen = {
      singleSelection: true,
      idField: "DefaultScreen_Id",
      textField: "Screen_Desc",
      text: "Select",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Role',
    };
    this.dropdownSettings_Roles = {
      singleSelection: true,
      idField: "Role_Id",
      textField: "Role_Desc",
      text: "Select Role",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    }
    this.getRoleMaster();
    //this.getRoleScreensData();
    this.getSubRoles();
    this.userActivity();
    
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.RoleMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getHistoryById(roleId: number) {
    this.auditTable = {
      "tableName": "Role",
      "recordId": roleId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  getSubRoles() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<RoleDrop[]>(this.config.Emar_Role_GetSubRolesList)
      .subscribe(res => {
        this.roleDrop = res
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(this.config.commonErrorMessage(error));
      });
  }
  onSuperiorRoleSelect(item: any) {
    this.selectedScreenItems = [];
    this.myform.patchValue({
      screen: '',
    });
    this.getRoleScreensData(item.Role_Id);
  }
  onSuperiorRoleDeSelect(item:any) {
    this.screens = [];
    this.selectedScreenItems = [];
    this.myform.patchValue({
      screen: '',
    });
    this.alertService.warn("Please Select role for screens");
  }
  getRoleScreensData(roleId: any) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_RoleConfigMaster_GetSubRolesScreenList + roleId + "/" + "RoleMaster")
      .subscribe(res => {
        this.screens = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(this.config.commonErrorMessage(error));
      });
  }
  getRoleMaster() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_RoleMaster_GetRoleDetailsAll)
      .subscribe(res => {

        this.roleMaster = res;
        this.roleList = this.roleMaster.filter(ro => ro.Role_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(this.config.commonErrorMessage(error));
        this.ng4LoadingSpinnerService.hide();
      })
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.roleList = this.roleMaster.filter(ro => ro.Role_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getRoleMaster();
    }
  }
  getRoleDetailsByID(RoleID: number) {
    let loggedInUserRoleId: number = this.persistanceService.get(this.config.loggedInUserRoleKey);
    if (RoleID == loggedInUserRoleId && loggedInUserRoleId != 1)
      this.alertService.warn('Cannot edit the Role which is assigned to you. Your Superior can edit this.');
    else {
      this.ng4LoadingSpinnerService.show();
      window.scroll(0, 0);
      this.url = this.config.Emar_RoleMaster_GetRoleDetailsByID + "/" + RoleID;
      this.dataservice.get<any>(this.url).subscribe(
        res => {
          this.getRoleScreensDataById(res);
          this.ng4LoadingSpinnerService.hide();
        },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  getRoleScreensDataById(data: any) {
    if (data.Parent_Id == null) {
      data.Parent_Id = 1;
    }
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_RoleConfigMaster_GetSubRolesScreenList + data.Parent_Id + "/" + "RoleMaster")
      .subscribe(res => {
        this.screens = res;
        this.fetchDetails(data);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(this.config.commonErrorMessage(error));
      });
  }
  fetchDetails(res: any) {
    this.selectedScreenItems = [];
    this.selectedrItems = [];
    if (res.Role_Id == 1) {
      this.selectedrItems.push(this.roleDrop.filter(s => s.Role_Id == 1)[0]);
      this.statusbutton = res.Role_Status;
      this.isReadOnly = true;
    }
    else {
      this.selectedrItems.push(this.roleDrop.filter(s => s.Role_Id == res.Parent_Id)[0]);
      this.statusbutton = 0;
      this.isReadOnly = false;
    }
    if(res.DefaultScreen_Id !=null || res.DefaultScreen_Id !=undefined)
    {
      this.selectedScreenItems.push(this.screens.filter(s => s.DefaultScreen_Id == res.DefaultScreen_Id)[0]);
    }
    this.RoleID = res.Role_Id;
    this.myform.patchValue({
      roleDesc: res.Role_Desc,
      superiorRole: this.selectedrItems,
      screen: this.selectedScreenItems,
      status: res.Role_Status
    });
  }
  insertRole() {
    if (this.RoleID != 0 && this.RoleID != 1 && this.RoleID == this.myform.value.superiorRole[0].Role_Id) {
      this.alertService.warn('Same Role Cannot be Superior Role');
    }
    else {
      this.roleObj = {
        Role_Id: this.RoleID,
        Role_Desc: this.myform.value.roleDesc,
        DefaultScreen_Id: this.myform.value.screen[0].DefaultScreen_Id,
        Role_Status: (this.myform.value.status == true ? 1 : 0),
        IsAdmin:1,
        Role_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        Role_CreatedDate: new Date().toISOString(),
        Parent_Id: this.myform.value.superiorRole[0].Role_Id,
      };
      this.ng4LoadingSpinnerService.show();
      this.dataservice.post(this.config.Emar_RoleMaster_InsertRole, this.roleObj)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.success("Save successful");
          this.getRoleMaster();
          this.inactivecheckbox = false;
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      this.reSet();
    }
  }
  reSet() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.isReadOnly = false;
    this.statusbutton = 0;
    this.myform.patchValue({
      screen: '',
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   roleDesc: '',
    //   status: '1'
    // });
    this.RoleID = 0;
  }
  checkRoleDesc(): any {

    let Role_Desc = this.myform.value.roleDesc.replace(/\s/g, '').toLowerCase();
    let result1 = this.roleMaster.find(x => (x.Role_Description).replace(/\s/g, '').toLowerCase() === Role_Desc);
    if (result1) {
      this.alertService.error("RoleDescription Already Exists");
      this.myform.patchValue({
        roleDesc: ''
      });
    }
    else { }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.roleList.forEach(element => {
        element.Role_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Role_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.Role_Status=1;
        if(element.Role_id!=1 && element.Role_id!=this.loggedInUserRoleId)
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
      item.Role_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Role_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.Role_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Role_Id == item.Role_id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateRoleStatus() {
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      let loggedInUserRoleId: number = this.persistanceService.get(this.config.loggedInUserRoleKey);
      if (this.selectedRecords.find(e => e.Role_id == loggedInUserRoleId) && loggedInUserRoleId != 1)
        this.alertService.warn('Cannot edit the Role which is assigned to you. Your Superior can edit this.');
      else if (this.selectedRecords.find(e => e.Role_id == 1))
        this.alertService.warn('Super Admin Role status cannot be Inactive.');        
      else {
        this.ng4LoadingSpinnerService.show();
        this.dataservice.post(this.config.Emar_RoleMaster_UpdateRolesStatus, this.selectedRecords)
          .subscribe(res => {
            if (res == 1) {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.success("Status Updated Successfully");
              this.UpdateStatus = true;
              this.inactivecheckbox = false;
              this.CheckAll = false;
              this.getRoleMaster();
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
  addDefaultScreenModel() {
    const modalRef = this.modalService.open(DefaultscreenComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.allDefaultScreens.subscribe((receivedResult) => {
      if (receivedResult == 2) {
        this.alertService.warn("Default screen assigned to role.You can't remove it.")
      }
      else if (receivedResult == 1) {
        this.alertService.success("Save successful.");
        this.getRoleMaster();
        //this.getRoleScreensData();
      }
      else if (receivedResult == 0) {
        this.alertService.success("Removed Successfully.");
        this.getRoleMaster();
        //this.getRoleScreensData();
      }
      modalRef.close();
    });
  }
}
