import { Component, OnInit, ChangeDetectorRef, ViewChild, Input, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Gender, MaritalStatus, Suffix } from '../../../models/common.model';
import { UserModel } from '../../../models/user.model';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { TextMaskModule } from 'angular2-text-mask';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { RoleMaster, RoleConfigMaster, RoleFacility, UserRoleFacilityConfigEntity, RoleNurseStation, RoleDrop } from '../../../models/role.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminresetpasswordmodalComponent } from '../adminresetpasswordmodal/adminresetpasswordmodal.component';

@Component({
  selector: 'app-emaruser',
  templateUrl: './emaruser.component.html',
  styleUrls: ['./emaruser.component.css'],
  providers: [DataService, APIConfiguration]
})
export class EmaruserComponent implements OnInit {
  public mobileNumberMask = this.config.mobileNumberMask;
  public genders: Gender[];
  public template;
  public maritalStatuses: MaritalStatus[];
  public suffixes: Suffix[];
  private user = new UserModel();
  userId: number = 0;
  public usersList: any[] = [];
  errorMessage: string;
  myform: FormGroup;
  p: number = 1;
  auditTable: any;
  gridPagination = this.config.gridPagination;
  searchText: string = "";
  public modalHistoryIsOpen: boolean = false;
  public inactivecheckbox: boolean = false;
  public users: any[] = [];
  public selectedRecords: any[] = [];
  public CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  public newUserFlag: number = 0;
  dropdownSettings_Roles: any = {};
  dropdownSettings_Facility: any = {};
  dropdownSettings_NurseStation: any = {};
  dropdownSettings_Computername:any={};
  public roleDrop: RoleDrop[];
  public roleFacilty: RoleFacility[];
  public roleNurseStation: RoleNurseStation[];
  public selectedrItems = [];
  public selectedfItems = [];
  public selectednItems = [];
  public selectedcItems = [];
  public facilities: string = "";
  public nstations: string = "";
  ShowFilter = true;
  public RoleNPIlag:boolean = false;
  public modalfcilitypemission: boolean = false;
  public facilityNurseStationarray: any[] = [];
  public arFacilities: any[];
  public arNurseStations: any[];
  public nurseStationNames: string = "";
  public nurseStationIds: string = "";
  public selectedFacilities: string = "";
  public selectedNurseStations: string = "";
  public isUserAdmin: boolean = false;
  public modalUnlockIsOpen: boolean = false;
  public unlockFlag: boolean = false;
  public unlockUserId: number;
  public isReadOnly: boolean = false;
  public loginUserReceFacility:any;
  public loginUserReceNurseStation:any;
  public loginUserId:any;
  public isRoleReadonly: boolean = false;
  pageConfig: {};
  public loggedInUserRoleId:number;
  public statusbutton:number=0;
  public Computernamedrop:any[]=[];
  public isRolePhysician:boolean=false;
  @Input('inputFocus') inputFocus:ElementRef

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef,
    private alertService: AlertService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService, public sharedService: SharedService, private modalService: NgbModal) { }

  ngOnInit() {
    this.loginUserId=this.persistanceService.get(this.config.loggedInUserKey);
    this.loggedInUserRoleId= parseInt(this.persistanceService.get(this.config.loggedInUserRoleKey));
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Users");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    if (this.persistanceService.get('userRole') == '\"SuperAdmin\"') {
      this.isUserAdmin = true;
    }
    this.myform = new FormGroup({
      fname: new FormControl('', [Validators.required, Validators.maxLength(25), Validators.pattern(this.config.alphaNumericFewSpecialCharacters3)]),
      mname: new FormControl('', [Validators.maxLength(25), Validators.pattern(this.config.alphaNumericFewSpecialCharacters3)]),
      lname: new FormControl('', [Validators.required, Validators.maxLength(25), Validators.pattern(this.config.alphaNumericFewSpecialCharacters3)]),
      username: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      password: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password), Validators.minLength(4)]),
      displayname: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters9)]),
      email: new FormControl('', [Validators.required, Validators.pattern(this.config.eMail)]),
      phone: new FormControl('', [Validators.minLength(14)]),
      //gender: new FormControl(''),
      //maritalstatus: new FormControl(''),                  
      suffix: new FormControl(''),
      status: new FormControl('1'),
      stockreq: new FormControl(''),
      roleDesc: new FormControl('', Validators.required),
      ddlfacility: new FormControl(''),
      ddlnursestation: new FormControl(''),
      Computername: new FormControl(''),
      phynpi: new FormControl('', [Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]),
      pastDueAlert: new FormControl(''),
    });
    this.getSuffixes();
    // this.getGenders();
    // this.getMaritalStatus();
    this.getUsers();
    this.getRoleMasterData();
    this.getRolefacilityMasterData();
    this.getProcessMasterData();
    this.userActivity();
    this.dropdownSettings_Facility = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Select Facilities",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_NurseStation = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Select Nursing Stations",
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
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    }
    this.dropdownSettings_Computername = {
      singleSelection: true,
      idField: "ProcessID",
      textField: "ComputerName",
      text: "Select Computer name",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    }
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Users, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getRoleMasterData() {
    this.dataservice.get<RoleDrop[]>(this.config.Emar_Role_GetSubRolesList)
      .subscribe(res => this.roleDrop = res, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
      });
  }
  getRolefacilityMasterData() {
    let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions+ loginUser)
      .subscribe(res => 
        {
          this.roleFacilty =  res.Facilities;
          if(this.roleFacilty.length==1)
          {
            this.getRoleNurseStationMasterData(this.roleFacilty[0].Facility_Id);
            this.myform.patchValue({
              ddlfacility:this.roleFacilty,
            });
          }
        }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
      });
  }
  // CompareGender(e: any) {
  //   if (this.myform.value.suffix == 1) {
  //     this.myform.patchValue({
  //       gender: 1
  //     });
  //   }
  //   else if (this.myform.value.suffix == 2 || this.myform.value.suffix == 3) {
  //     this.myform.patchValue({
  //       gender: 2
  //     });
  //   }
  //   else if (this.myform.value.suffix == '' || this.myform.value.suffix == null) {
  //     this.myform.patchValue({
  //       gender: 3
  //     });
  //   }
  // }
  // CompareSuffix($event) {
  //   if (this.myform.value.gender == 1) {
  //     this.myform.patchValue({
  //       suffix: 1
  //     });
  //   }
  //   else if (this.myform.value.gender == 2) {
  //     this.myform.patchValue({
  //       suffix: 2
  //     })
  //   }

  // }
  getHistoryById(userId: number) {
    this.auditTable = {
      "tableName": "User",
      "recordId": userId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.modalfcilitypemission = false;
  }
  getGenders() {
    this.dataservice.get<Gender[]>(this.config.Common_GetGenders)
      .subscribe(res => this.genders = res, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
      });
  }
  getSuffixes() {
    this.dataservice.get<Suffix[]>(this.config.Common_GetSuffixes)
      .subscribe(res => this.suffixes = res, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
      });
  }
  // getcompanyselected(item:number)
  // {
  // alert('Selected vlalue : '+item);
  // }
  insertUser() {
    this.selectedFacilities = "";
    this.selectedNurseStations = "";
    if (this.facilityNurseStationarray.length == 0) {
      this.alertService.warn("Add facility nursing station permission.")
    }
    else {
      this.ng4LoadingSpinnerService.show();
      this.facilityNurseStationarray.forEach(element => {
        this.selectedFacilities += element.FacilityId + ",";
        this.selectedNurseStations += element.NurseStationIds + ",";
      });
      this.selectedFacilities = this.selectedFacilities.substring(0, this.selectedFacilities.length - 1);
      this.selectedNurseStations = this.selectedNurseStations.substring(0, this.selectedNurseStations.length - 1);
      this.user = {
        User_Id: this.userId,
        User_Fname: this.myform.value.fname,
        User_Mname: this.myform.value.mname,
        User_Lname: this.myform.value.lname,
        UserName: this.myform.value.username,
        Password: this.myform.value.password,
        User_DisplayName: this.myform.value.displayname,
        User_Email: this.myform.value.email,
        User_Phone: this.myform.value.phone,
        User_Gender: null,
        User_MaritalStatus: null,
        User_Suffix: this.myform.value.suffix == "" ? null : this.myform.value.suffix,
        User_Status: (this.myform.value.status == true ? 1 : 0),
        User_PwdCount: 0,
        User_Lock: 0,
        User_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        User_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
        StkReportReq: this.myform.value.stockreq == true ? 1 : 0,
        NewUserFlag: this.userId == 0 || this.newUserFlag == 1 ? 1 : 0,
        RoleId: this.myform.value.roleDesc[0].Role_Id,
        Facilities: this.selectedFacilities,
        NurseStations: this.selectedNurseStations,
        Processkey:null,
        PhysicianNPI:this.myform.value.phynpi,
        PastDueAlertFlag:this.myform.value.pastDueAlert == true ? 1 : 0,
      };
       
      this.dataservice.post(this.config.Emar_UserMaster_InsertUser, this.user)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res == 1) {
             
            this.alertService.success("Save successful");
            this.inactivecheckbox = false;
            this.isReadOnly = false;
            this.isRoleReadonly=false;
            this.isRolePhysician=false;
            this.getUsers();
            if(this.userId==this.persistanceService.get(this.config.loggedInUserKey))
            {
              this.sharedService.pastDueFlag(this.user.PastDueAlertFlag);
            }
            this.resetScreen();
          }
          else if (res == 2)
            this.alertService.error("User Name already exists");
          else if (res == 3)
            this.alertService.error("User email ID already exists");
          else if (res == 4)
            this.alertService.error("User Display Name already exists");
          else if (res == 5)
          {

          
          if(this.RoleNPIlag == true)
          {

          
            this.alertService.error("Prescriber NPI not exists in Prescriber Details");
          }
          else{
            this.alertService.error("Physician NPI not exists in Prescriber Details");
          }

          }
          else
            this.alertService.error("Something went wrong. Please try again");
        },
          error => {
            this.alertService.error(error.message)
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      suffix: '',
      gender: '',
      maritalstatus: '',
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   fname: '',
    //   mname: '',
    //   lname: '',
    //   username: '',
    //   password: '',
    //   displayname: '',
    //   email: '',
    //   phone: '',
    //   gender: '',
    //   maritalstatus: '',
    //   status: '1'
    // });
    this.userId = 0;
    this.newUserFlag = 0;
    this.isReadOnly = false;
    this.isRoleReadonly=false;
    this.isRolePhysician=false;
    this.roleNurseStation = [];
    this.facilityNurseStationarray = [];
  }
  getUsers() {
    this.ng4LoadingSpinnerService.show();
    let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_UserMaster_GetUsersMasterGrid +loginUser)
      .subscribe(res => {
        this.usersList = res;
        this.users = this.usersList.filter(n => n.User_Status == 1);
        let checkList = this.users.filter(n => n.User_Lock == 1);
        if (checkList.length > 0)
          this.unlockFlag = true;
        else
          this.unlockFlag = false;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  getUserDetailsByID(userId: number,role_status:number) {
    this.facilityNurseStationarray = [];
    this.isRoleReadonly=false;
    this.ng4LoadingSpinnerService.show();
    if(role_status==1)
    {
    this.dataservice.get<any>(this.config.Emar_UserMaster_GetUserDetailsByID + userId)
      .subscribe(res => {
        if(parseInt(this.loginUserId)==userId || parseInt(res.RoleId)===1)
        {
          this.isRoleReadonly=true;
        }
        this.facilityNurseStationarray = res.UserFacilityNurseList;
        this.isReadOnly = true;
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    }
    else
    {
    this.alertService.error("Selected user role is inactive, cannot update");
        this.ng4LoadingSpinnerService.hide();
    }
      window.scroll(0, 0);
  }
  fetchData(res: any) {
     
    this.selectedrItems = [];
    this.selectedcItems=[];
    this.statusbutton=0;
    this.selectedrItems.push(this.roleDrop.filter(e => e.Role_Id === parseInt(res.RoleId))[0]);
    if(res.ProcessKey!=null){  
      this.selectedcItems.push(this.Computernamedrop.filter(e => e.ProcessID ==res.ProcessKey)[0]);
      }
    if(res.User_Id==this.loginUserId || res.RoleId===1)
    {
      this.statusbutton=res.User_Status;
    }
    this.myform.patchValue({
      fname: res.User_Fname,
      mname: res.User_Mname,
      lname: res.User_Lname,
      username: res.UserName,
      password: res.Password,
      displayname: res.User_DisplayName,
      email: res.User_Email,
      phone: res.User_Phone,
      gender: res.User_Gender,
      maritalstatus: res.User_MaritalStatus,
      suffix: res.User_Suffix,
      status: res.User_Status,
      stockreq: res.StkReportReq,
      roleDesc: this.selectedrItems,
      Computername:this.selectedcItems,
      phynpi:res.PhysicianNPI,
      pastDueAlert:res.PastDueAlertFlag
    });
    this.userId = res.User_Id;
    this.newUserFlag = res.NewUserFlag;
    this.myform.controls['ddlfacility'].reset();
    this.myform.controls['ddlnursestation'].reset();
    this.roleNurseStation = [];
    if(this.myform.value.roleDesc[0].Role_Desc=="PHYSICIAN" || this.myform.value.roleDesc[0].Role_Desc == "PRESCRIBER")
    {
    this.isRolePhysician=true;
    if(this.myform.value.roleDesc[0].Role_Desc =="PRESCRIBER")
    {
      this.RoleNPIlag = true;
    }
    else{
      this.RoleNPIlag   = false;
    }
    const phynpivalidation = this.myform.get('phynpi');
    phynpivalidation.setValidators([Validators.required, Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]);
    phynpivalidation.updateValueAndValidity();
    }
    else
    {
    this.isRolePhysician=false;
    const phynpivalidation = this.myform.get('phynpi');
    phynpivalidation.setValidators([Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]);
    phynpivalidation.updateValueAndValidity();
    this.myform.controls['phynpi'].reset();
    }
  }
  showInactiveRecords(value: any) {
    if (value == true) {
      //  this.inactivecheckbox=true;
      this.users = this.usersList.filter(n => n.User_Status == 0);
    }
    if (value == false) {
      this.getUsers();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.users.forEach(element => {
        element.User_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.User_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.User_Status=1;
        if(element.User_Id!=this.loginUserId && element.RoleName!="SuperAdmin")
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
      item.User_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.User_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.User_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.User_Id == item.User_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateUserStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select data to update status.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.selectedRecords.find(e => e.RoleName === "SuperAdmin"))
    {
      this.alertService.warn('Super admin user records cannot be inactive.');
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_UserMaster_UpdateUsersStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getUsers();
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
  onFacilitySelect(item: any) {
    this.myform.patchValue({ ddlnursestation: '' });
    this.myform.value.ddlfacility = item;
    this.getRoleNurseStationMasterData(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.getRoleNurseStationMasterData(0);
  }
  onNurseStationSelect(item: any) {
    this.nstations = "";
  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestation = item;
  }
  onNurseStationDeSelect(item: any) {
    this.nstations = "";
  }
  onNurseStationDeSelectAll(item: any) {
    this.myform.value.ddlnursestation.length = 0;
  }
  getRoleNurseStationMasterData(facilityId: number) {
    if (facilityId != 0) {
      let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
      this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + loginUser + "/" + facilityId)
        .subscribe(res => {
          this.roleNurseStation = res;
          if (res.length == 0) {
            this.roleNurseStation = [];
            this.selectednItems = [];
            this.myform.patchValue({
              ddlnursestation: this.selectednItems,
            })
            this.alertService.warn("Please select facility for nursing stations.");
          }
          else {
            this.dropdownSettings_NurseStation = {
              singleSelection: false,
              idField: "NurseStation_Id",
              textField: "NurseStation_Name",
              text: "Select Nursing Stations",
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
      this.alertService.warn("Please select facility for nursing stations.");
    }
  }
  modalfcilitypemissionopen() {
    this.getRolefacilityMasterData();
    this.modalfcilitypemission = true;
    setTimeout(() => {
      this.inputFocus.nativeElement.focus()
    }, 300);
  }
  addFacilityNurseStation() {
    this.nurseStationNames = "";
    this.nurseStationIds = "";
    if ((this.myform.value.ddlfacility == undefined || this.myform.value.ddlfacility == "" || this.myform.value.ddlfacility == null) || (this.myform.value.ddlnursestation == undefined || this.myform.value.ddlnursestation == "" || this.myform.value.ddlnursestation == null)) {
      this.alertService.warn("Please select both facility and nursing station.")
    }
    else {

      let result = this.facilityNurseStationarray.find(f => f.FacilityId == this.myform.value.ddlfacility[0].Facility_Id);
      if (result) {
        let nsIds = result.NurseStationIds.split(',').map(Number);
        var selectedNSIds = this.myform.value.ddlnursestation.map(a => a.NurseStation_Id);
        let newNsIds = selectedNSIds.filter(f => !nsIds.includes(f));
        // if (newNsIds.length == 0)
        //   this.alertService.warn("Selected Nursing Stations are already assigned.");
        //   //ToDo: Fix this
        //   else
        //   {
          var checkExistingRecord= this.facilityNurseStationarray.find(f => f.FacilityId == this.myform.value.ddlfacility[0].Facility_Id);
          checkExistingRecord.NurseStationIds="";
          checkExistingRecord.NurseStatioNames="";
          selectedNSIds.forEach(Id=>{
               this.nurseStationIds += Id + ",";
               let nurseStationcode = this.roleNurseStation.find(x => x.NurseStation_Id == Id).NurseStation_Name;
               this.nurseStationNames += nurseStationcode + ", ";
            });
            checkExistingRecord.NurseStatioNames =this.nurseStationNames.substring(0, this.nurseStationNames.length - 2);
            checkExistingRecord.NurseStationIds= this.nurseStationIds.substring(0, this.nurseStationIds.length - 1);
            this.facilityNurseStationarray.push();
            this.myform.controls['ddlfacility'].reset();
            this.myform.controls['ddlnursestation'].reset();
            this.roleNurseStation = [];
          //}
      }
      else {
        this.arNurseStations = this.myform.value.ddlnursestation;
        this.arNurseStations.forEach(element => {
          let nurseStationcode = this.roleNurseStation.find(x => x.NurseStation_Id == element.NurseStation_Id).NurseStation_Name;
          this.nurseStationNames += nurseStationcode + ", ";
          this.nurseStationIds += element.NurseStation_Id + ",";
        });
        this.nurseStationNames = this.nurseStationNames.substring(0, this.nurseStationNames.length - 2);
        this.nurseStationIds = this.nurseStationIds.substring(0, this.nurseStationIds.length - 1);
        let facnurseObj: any = {
          FacilityId: this.myform.value.ddlfacility[0].Facility_Id,
          FacilityName: this.myform.value.ddlfacility[0].Facility_Name,
          NurseStatioNames: this.nurseStationNames,
          NurseStationIds: this.nurseStationIds,
        }
        this.facilityNurseStationarray.push(facnurseObj);
        this.myform.controls['ddlfacility'].reset();
        this.myform.controls['ddlnursestation'].reset();
        this.roleNurseStation = [];

      }
    }
  }
  removeFacNursestation(i: number) {
    this.facilityNurseStationarray.splice(i, 1);
  }
  changeUserPassword(userId: number) {
    const modalRef = this.modalService.open(AdminresetpasswordmodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedUser = userId;
    modalRef.componentInstance.changePasswordResult.subscribe((receivedResult) => {
      if (receivedResult == 1) {
        this.alertService.success("Reset password successful.");
        this.getUsers();
      }
      else if (receivedResult == 0) {
        this.alertService.success("Reset password failed.");
        this.getUsers();
      }
      modalRef.close();
    });
  }
  modalUnlockopen(userId: any) {
    this.userId = userId;
    this.modalUnlockIsOpen = true;
  }
  closeUnlockModel() {
    this.modalUnlockIsOpen = false;
    this.userId = 0;
  }
  unlockUser() {
    let userObj =
    {
      User_Id: this.userId,
      NewUserFlag: 0,
      User_PwdCount: 0,
      User_Lock: 0,
      User_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      User_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
    }
    this.dataservice.post(this.config.Emar_UserMaster_ResetUserPwdByAdmin, userObj)
      .subscribe(res => {
        this.modalUnlockIsOpen = false;
        this.userId = 0;
        if (res == 1) {
          this.alertService.success("User unlocked successfully.");
          this.getUsers();
        }
        else if (res == 0)
          this.alertService.error("User unlock failed.");
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getProcessMasterData() {
     
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetComputerNameDropData)
      .subscribe(res => this.Computernamedrop = res, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationByFacilityID(facilityId: any,nursingStationIds:any) {
    if (facilityId != 0) {
      let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
      this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + loginUser + "/" + facilityId)
        .subscribe(res => {
          this.roleNurseStation = res;
          if (res.length == 0) {
            this.roleNurseStation = [];
            this.selectednItems = [];
            this.myform.patchValue({
              ddlnursestation: this.selectednItems,
            });
            this.alertService.warn("Please select facility for nursing stations.");
          }
          else {
            this.dropdownSettings_NurseStation = {
              singleSelection: false,
              idField: "NurseStation_Id",
              textField: "NurseStation_Name",
              text: "Select Nursing Stations",
              selectAllText: "Select All",
              unSelectAllText: "UnSelect All",
              itemsShowLimit: 1,
              noDataAvailablePlaceholderText: "Please Select Facility",
              allowSearchFilter: this.ShowFilter
            };
            this.selectedfItems=[];
            this.selectednItems=[];
           let facExist= this.roleFacilty.find(f=>f.Facility_Id==facilityId);
           if(facExist!=undefined)
           {
            this.selectedfItems.push(facExist);
           }
           let nsIds = nursingStationIds.split(',').map(Number);
           if (nsIds.length > 0) {
             this.selectednItems = [];
             for (let i = 0; i < nsIds.length; i++) {
               let checkNsExist = this.roleNurseStation.find(r => r.NurseStation_Id === parseInt(nsIds[i]));
               if (checkNsExist != undefined) {
                 this.selectednItems.push(checkNsExist);
               }
             }
             this.myform.patchValue({
              ddlfacility:this.selectedfItems,
              ddlnursestation: this.selectednItems,
             });
           }
          }
        }, error => {
          this.alertService.error(error.message)
        });
    }
    else {
      this.roleNurseStation = [];
      this.selectednItems = [];
      this.alertService.warn("Please select facility for nursing stations.");
    }
  }
  onRoleSelect(item:any)
  {
    if(item.Role_Desc=="PHYSICIAN" || item.Role_Desc=="PRESCRIBER")
    {
    this.isRolePhysician=true;
    if(item.Role_Desc =="PRESCRIBER")
    {
      this.RoleNPIlag = true;
    }
    else{
      this.RoleNPIlag   = false;
    }
    const phynpivalidation = this.myform.get('phynpi');
    phynpivalidation.setValidators([Validators.required,Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]);
    phynpivalidation.updateValueAndValidity();
    }
    else
    {
    this.isRolePhysician=false;
    const phynpivalidation = this.myform.get('phynpi');
    phynpivalidation.setValidators([Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]);
    phynpivalidation.updateValueAndValidity();
    }
    this.myform.controls['phynpi'].reset();
  }
  onRoleDeSelect(item:any)
  {
    this.isRolePhysician=false;
    const phynpivalidation = this.myform.get('phynpi');
    phynpivalidation.setValidators([Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]);
    phynpivalidation.updateValueAndValidity();
  }
}
