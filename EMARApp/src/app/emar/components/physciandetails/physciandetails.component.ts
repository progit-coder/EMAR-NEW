import { PhyscianModel } from './../../../models/Physcian.model';
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Country, State, City, ZipCode } from '../../../models/common.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { RoleFacility, RoleNurseStation, } from '../../../models/role.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Screens,Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
@Component({
  selector: 'app-physciandetails',
  templateUrl: './physciandetails.component.html',
  styleUrls: ['./physciandetails.component.css']
})
export class PhysciandetailsComponent implements OnInit {
  searchText: string = "";
  numberflag:boolean=false;
  public records: string[] = [];
  public newRecord: string = '';
  public countries: Country[];
  public states: State[];
  public cities: City[];
  myform: FormGroup;
  public zipcode = this.config.zipcode;
  private zipCodes: ZipCode[];
  public roleNurseStation: RoleNurseStation[];
  pageConfig: {};
  p: number = 1;
  public supervisingPhysicianList=[];
  public selectednItems = [];
  public selectedfItems=[];
  public newcredlist=[];
  public newprimarylist=[];
  gridPagination = this.config.gridPagination;
  public template;
  ShowFilter = true;
  public roleFacilty: RoleFacility[];
  dropdownSettings_Facility: any = {};
  public dropdownSettings_credentials:any={};
  public dropdownSettings_primarySpeciality:any={}
  public dropdownSettings_State:any={};
  public dropdownSettings_license:any={};
  dropdownSettings_NurseStation: any = {};
  errorMessage: string;
  private PhysicianObj: PhyscianModel;
  public PhysicianId: number = 0;
  public phydetails: any[] = [];
  public inactivecheckbox: boolean = false;
  public phyisicianList: any[] = [];
  public updatedSuperphyst:any[]=[]
  public record: any;
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  public nstations: string = "";
  public arNurseStations: any[];
  public selectedCredentialItems='';
  public primarySpecialityItems='';
  public Stateitems=[];
  public licenseItems=[];
  public savedisable=false;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public userId: number = this.persistanceService.get(this.config.loggedInUserKey);
  public oldPhyNpi:string;
  public oldFacilityId:string;
  public zipcodemask:any;  
  public zipcodeisvalid=true;
  public zipcodeValue:string="";
  public credentialsList=[];
  public primarySpecialityList=[];
  public Statelist=[];
  public licenseList=[];
  public supervisingPhysicianItems=[];
  public licenseform: FormGroup;
  public licensearray:any=[];
  public maxRecords: number = 3;
  dropdownSettings_supervisingPhysician: {};
  public supervisinglimiteditems: any[];
  superCredentialFlag: Boolean = false
  supervisFlag:boolean =false;
  hidelicenseArray = true;
  statelistnew: any;
  licensenew: any;
  supervisst = [];
  licenseNumber: any;
  licenseformdetails=[];
  stateList: any;
  licensetestdata=[];
  fetchlicenseArray: boolean = false;
  insertlicenseArray: boolean=false;
  public mobileNumberMask = this.config.mobileNumberMask;
  public pharmacynpi = this.config.pharmacynpi;
  @Input() phyNpiUser: number;
  PhyUserDet: any[];
  TempNpi :number = 0;
  public arFacilities: any[];
  public fstations: string = "";
  physicianAlertMsg: string;
  public supervisingInactiveModal:boolean=false; 


  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private dateFormatPipe: CustomdatePipe, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService  ,  private readonly changeDetectorRef: ChangeDetectorRef) { }

  ngOnInit() {
    this.getPhysicianDetails()
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
   this.pageConfig = this.persistanceService.getPermissionsByScreen("PrescriberDetails");
   if (this.pageConfig != undefined) {
    if (this.pageConfig["AccessRead"] == 0) {
      this.persistanceService.redirectToHomePage();
    }
    else {
    this.myform = new FormGroup({
      ddlfacility: new FormControl('', Validators.required),
      ddlnursestation: new FormControl('', Validators.required),
      phyfirstname: new FormControl('', [Validators.required,Validators.maxLength(25), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      phylastname: new FormControl('', [Validators.required,Validators.maxLength(25), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      phyadd1: new FormControl('', [Validators.required,Validators.maxLength(100)]),
      phyadd2: new FormControl('', [Validators.maxLength(100)]),
      country: new FormControl('1'),
      city: new FormControl('', [Validators.required,Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters5)]),
      state: new FormControl('', [Validators.required,Validators.maxLength(2),Validators.minLength(2), Validators.pattern(this.config.alphabets)]),
      zipcode: new FormControl('',[Validators.required,Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]),
      status: new FormControl('1'),
      phynpi: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(10),Validators.pattern(this.config.numeric)]),
      credentials:new FormControl('',[Validators.required]),
      primarySpeciality: new FormControl('',Validators.required),
      supervisingPhysician:new FormControl(''),
      phone:new FormControl('',[Validators.required ,  Validators.minLength(14)]),
      licenseform : new FormGroup({
        State:new FormControl('',Validators.required),
        license: new FormControl(''),
        Number:new FormControl('', Validators.required),
  
      }),
      pid:new FormControl(this.PhysicianId)

    });
    this.dropdownSettings_Facility = {
      singleSelection: false,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Select Facilities",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_NurseStation = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Nursing Stations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_credentials = {
      singleSelection: true,
      idField: "Id",
      textField: "Name",
      text: "Select Credentials/Degree",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_primarySpeciality={
      singleSelection: true,
      idField: "Id",
      textField: "Name",
      text: "Select primarySpeciality",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_State={
      singleSelection: true,
      idField: "State_Id",
      textField: "State_Code",
      text: "Select state",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      placeHolder:'please slect',
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_supervisingPhysician={
      singleSelection: true,
      idField: "PhysicianNPI",
      textField: "PhysicianName",
      text: "Select license",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
      
    }
    this.getAllCountries();
    //this.getRolefacilityMasterData();
    this.getPhysicianDetails();
    //this.getNurseStationByFacilityId();

    this.getUserRecentFacilityNurseStations();
    this.userActivity();
    this.TempNpi = (this.phyNpiUser != null && this.phyNpiUser != undefined && this.phyNpiUser != 0 )? this.phyNpiUser : 0 ;
    // if(this.phyNpiUser !=0){
    //   this.fetchUserData()
    // }
  
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.PhysicianDetails,Activity.View,'')
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
        //this.getFiltersData(this.userId);
        this.getRolefacilityMasterData();
      }, error => {
        this.alertService.error(error.message);
      });
  }

  insertPhysicianDetails() {
    debugger
    this.savedisable=true;
    this.licenseformdetails = [];
    this.arFacilities = [];
    this.ng4LoadingSpinnerService.show();
    this.nstations = '';
    this.fstations = '';
    this.arNurseStations = this.myform.value.ddlnursestation;
    this.arNurseStations.forEach(element => {
    this.nstations += element.NurseStation_Id + ",";
      });
       //facility multiselect start
    this.arFacilities =  this.myform.value.ddlfacility;
    this.arFacilities.forEach(element => {
      this.fstations += element.Facility_Id + ",";
        });
        if (this.fstations && this.fstations.endsWith(',')) {
          // Remove the trailing comma
          // /this.fstations = this.fstations.slice(0, -1);
        }
      if (this.licensearray.length == 1) {
     
        this.licenseformdetails.push({
            // State: this.licensearray[0].State[0].item_name,
            State: this.licensearray[0].State,
            City: 0,
            Number: this.licensearray[0].Number
        });
    }
    
    if (this.licensearray.length == 2) {
      this.licenseformdetails.push(
        {
          State: this.licensearray[0].State,
            City: 0,
            Number: this.licensearray[0].Number
      },
      {
        State: this.licensearray[1].State,
        City: 0,
        Number: this.licensearray[1].Number
        });
    }
    
    if (this.licensearray.length == 3) {
      this.licenseformdetails.push(
        {
          State: this.licensearray[0].State,
            City: 0,
            Number: this.licensearray[0].Number
      },
      {
        State: this.licensearray[1].State,
        City: 0,
        Number: this.licensearray[1].Number
        },
        {
            State: this.licensearray[2].State,
            City: 0,
            Number: this.licensearray[2].Number
        });
    }
      this.nstations = this.nstations.substring(0, this.nstations.length - 1);
       this.fstations = this.fstations.substring(0, this.fstations.length - 1);
      (this.myform.value.supervisingPhysician != '' && this.myform.value.supervisingPhysician != null && this.myform.value.supervisingPhysician.length > 0 )? this.supervisFlag = true : this.supervisFlag = false;
      console.log(this.oldFacilityId , "oldfaf")

    this.PhysicianObj = {
      Physician_Id: this.PhysicianId,
      PhysicianNPI: this.myform.value.phynpi,
      PhysicianLName: this.myform.value.phylastname,
      PhysicianFName: this.myform.value.phyfirstname,
      PhysicianAddress1: this.myform.value.phyadd1,
      PhysicianAddress2: this.myform.value.phyadd2,
      PhysicianCity: this.myform.value.city,
      PhysicianState: this.myform.value.state,
      PhysicianZip: this.myform.value.zipcode,
      Physician_Status: this.myform.value.status == true ? 1 : 0,
      Physician_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Physician_CreatedDate: this.dateFormatPipe.transform(new Date()),
      Facility_Id:this.fstations,

      // Facility_Id:this.myform.value.ddlfacility[0].Facility_Id,
      NurseStations: this.nstations,
      PhysicianCountryID: this.myform.value.country,
      OldPhysicianNPI:this.oldPhyNpi,
      OldFacilityId:this.oldFacilityId,
      PId : this.myform.value.pid,
      Credentials:this.myform.value.credentials[0].Id,
      PrimarySpec:this.myform.value.primarySpeciality[0].Id,
     // SupervisingPhy:'',
      SupervisingPhy:  this.myform.value.supervisingPhysician!= null && this.supervisFlag ? (this.myform.value.supervisingPhysician[0].PhysicianNPI) : '',
      LicensesData:this.licenseformdetails,
      Physician_Phone:this.myform.value.phone,
      FacId:this.fstations

    }
    this.dataservice.post(this.config.Emar_PhysicianDetails_InsertUpdatePhysicianDetails, this.PhysicianObj)

      .subscribe(res => {
        if(res==1)
        {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.licensearray= []
        this.hidelicenseArray = false
        }
        else if(res==2)
        {
        console.log("1")
        this.ng4LoadingSpinnerService.hide();
        this.alertService.warn("Same Physician NPI is already mapped with this facility.Can't insert new record. Edit the existing record.");
        }
        else if(res==3)
        {
          this.dataservice.get<any>(this.config.Emar_PhysicianDetails_GetDefaultPhysicianNursestation+"/"+this.myform.value.phynpi)
          .subscribe(res=>{
            this.ng4LoadingSpinnerService.hide();
            this.alertService.warn(this.PhysicianObj.PhysicianLName+" "+this.PhysicianObj.PhysicianFName+" is mapped as default physician for a Nursing station("+res+"). Remove "+this.PhysicianObj.PhysicianLName+" "+this.PhysicianObj.PhysicianFName+" as default physician in Nursing station Master.");
          },
          err=>{
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(this.errorMessage);
          })
        }
        this.getPhysicianDetails();
        this.resetScreen();
        this.savedisable=false;
      },
        error => {
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
          this.savedisable=false;
        }
      );
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.licensearray=[];
    this.myform.reset();
    this.selectednItems = [];
    this.selectedfItems=[];
    this.selectedfItems.push(this.roleFacilty.filter(f=>f.Facility_Id));
    this.selectednItems.push(this.roleNurseStation);
    this.myform.patchValue({
      ddlnursestation: this.selectednItems[0],
      ddlfacility:  this.selectedfItems[0],
      country: '1',
      status: '1',
    })

    this.ng4LoadingSpinnerService.hide();
    this.PhysicianId = 0;
    this.oldPhyNpi='';
    this.oldFacilityId='0';
    this.inactivecheckbox =  false;
  }
  getPhysicianDetails() {
    this.getcredentialsdata()
    this.getPrimarySpecialtyData()
    this.getStateData()
    this.ng4LoadingSpinnerService.show();
    let userId=this.persistanceService.get(this.config.loggedInUserKey);
    debugger
    // this.dataservice.get<any>(this.config.Emar_PhysicianDetails_GetPhysicianDetails + userId)
    this.dataservice.get<any>(this.config.Emar__PhysicianDetails_GetPhyscianDetailsGridDataNew + userId)

      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.phydetails = res;
        this.phyisicianList = this.phydetails.filter(p => p.Physician_Status == 1);
        // this.PhyUserDet = this.phyisicianList;
        if(this.TempNpi !=0){
          this.fetchUserData()
        }
        this.supervisingPhysicianItems=(this.phyisicianList.map((item, index) =>({ id: index + 1,
          PhysicianName: item.PhysicianName})))
          this.supervisinglimiteditems=this.phyisicianList.filter(t=>t.Credentials === 'NP' || t.Credentials === 'PA' ||  t.Credentials === 'Other')
        
          // console.log(this.supervisinglimiteditems, 'supervisinglimiteditems')
          this.updatedSuperphyst = this.phyisicianList.filter(parentItem =>
            !this.supervisinglimiteditems.some(childItem => childItem.PhysicianNPI === parentItem.PhysicianNPI)
          );
          console.log(this.updatedSuperphyst,"this.updatedSuperphyst")
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.Message);
          this.ng4LoadingSpinnerService.hide();
        }
      );
  }
  getRolefacilityMasterData() {
    debugger
    let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions+ loginUser)
      .subscribe(res => 
        {
          this.roleFacilty =  res.Facilities;
          if (this.loginUserReceFacility != null) {
            let userReceFList = this.loginUserReceNurseStation.split(',');
            if (userReceFList.length > 0) {
              this.selectedfaItems = [];
              for (let i = 0; i < userReceFList.length; i++) {
                let checkFacExist = this.roleFacilty.find(r => r.Facility_Id == parseInt(userReceFList[i]));
                if (checkFacExist != undefined) {
                  this.selectedfaItems.push(checkFacExist);
            


                  this.getRoleNurseStationMasterData(this.loginUserReceFacility);
                }
              }
              this.myform.patchValue({
                ddlfacility: this.selectedfaItems,
              });
              console.log(this.selectedfaItems,"this.selectedfaItems facilty")

            }
          }

          // if (this.loginUserReceFacility != null) {
          //   if (this.roleFacilty.length > 0) {
          //     let checkFacExist = this.roleFacilty.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
          //     this.selectedfaItems = [];
          //     if (checkFacExist != undefined) {
          //       this.selectedfaItems.push(checkFacExist);
          //       this.getRoleNurseStationMasterData(this.loginUserReceFacility);
          //     }
          //     this.myform.patchValue({
          //       ddlfacility: this.selectedfaItems,
          //     });
          //   }
          // }

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
  getRoleNurseStationMasterData(facilityId) {
    if (facilityId !=  0) {
      let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
      // this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + loginUser + "/" + facilityId)
      this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetUserNurseStationDropMultiple + loginUser + "/" + facilityId)
        .subscribe(res => {
          this.roleNurseStation = res;
          this.selectednItems=[]
                if (this.roleNurseStation.length) {
                  this.selectednItems.push(this.roleNurseStation);
                }
                this.myform.patchValue({
                        ddlnursestation: this.selectednItems[0],
                      });
          // if (this.loginUserReceNurseStation != undefined) {
          //   let userReceNSList = this.loginUserReceNurseStation.split(',');
          //   if (userReceNSList.length > 0) {
          //     this.selectednItems = [];
          //     for (let i = 0; i < userReceNSList.length; i++) {
          //       let checkNsExist = this.roleNurseStation.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
          //       if (checkNsExist != undefined) {
          //         this.selectednItems.push(checkNsExist);
          //       }
          //     }
          //     this.myform.patchValue({
          //       ddlnursestation: this.selectednItems,
          //     });
          //     //this.getFiltersDataBySelection(this.userId);
          //     //this.getCompanyToBedByNurseStation();
          //     //this.getFiltersDataBySelection(this.userId);
          //   }
          // }

          if (res.length == 0) {
            this.roleNurseStation = [];
            this.selectednItems = [];
            this.myform.patchValue({
              ddlnursestation: this.selectednItems,
            })
            // this.alertService.warn("Please select facility for nursing stations.");
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
      // this.alertService.warn("Please select facility for nursing stations.");
    }
  }
  onFacilitySelect(item: any) {
    this.myform.patchValue({ ddlnursestation: '' });
    let fstations = '';
     let arFacilities= []
    arFacilities =  this.myform.value.ddlfacility;
    arFacilities.map(element => {
      fstations += element.Facility_Id + ",";
        });

     this.getRoleNurseStationMasterData( fstations);
    

    //  this.getRoleNurseStationMasterData(this.fstations);
  }
  onFacilityDeSelect(item: any) {
    let fstations = '';
    let arFacilities= []
    arFacilities =  this.myform.value.ddlfacility;
    arFacilities.map(element => {
      fstations += element.Facility_Id + ",";
        });
     this.getRoleNurseStationMasterData(fstations);

  }
  onFacilitySelectAll(item:any){
    // console.log(item,"items 2")
     this.myform.value.ddlfacility = item;
    let fstations = '';
    let arFacilities= [];
    arFacilities =  this.myform.value.ddlfacility;
    arFacilities.forEach(element => {
       fstations += element.Facility_Id + ",";
         });

      this.getRoleNurseStationMasterData(fstations);
    // this.myform.value.ddlfacility = item;
    // console.log(this.myform.value.ddlfacility,"this.myform.value.2")

    // this.getRoleNurseStationMasterData( this.myform.value.ddlfacility);

  }
  onFacilityDeSelectAll(item:any){
    this.myform.value.ddlfacility = '';
    this.getRoleNurseStationMasterData(0);
    this.myform.value.ddlnursestation = '';
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
  getHistoryById(PhysicianId: number) {
    this.auditTable = {
      "tableName": "PhysicianDetails",
      "recordId": PhysicianId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.supervisingInactiveModal = false;

  }
  ngAfterViewChecked(): void {
    this.changeDetectorRef.detectChanges();
  }
  getPhysicianDetailsById(PhysicianId: string,nurseStationStatus:number,facilityStatus:number,facilityId:string ) {
 
   debugger;
    this.ng4LoadingSpinnerService.show();
    this.selectednItems = [];
    this.licensearray=[];
     const facids = (facilityId == '' || facilityId == null ? '0' : facilityId)
    if (nurseStationStatus == 1 && facilityStatus==1) {
    this.dataservice.get<any>(this.config.Emar_PhysicianDetails_GetPhysicianDetailsById + PhysicianId +"/"+ facids)
      .subscribe(res => {
        this.getFetchData(res);
       console.log(res,"res")
        //this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.Message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else if(facilityStatus==0 && nurseStationStatus==0)
      {
        // this.alertService.error("Selected Physician Facility and Nursing Station both InActive you can't update");
        this.alertService.error("Selected Physician Facility is InActive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
      else if(facilityStatus==0)
      {
        this.alertService.error("Selected Physician Facility is InActive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
      else if(nurseStationStatus==0)
      {
        // this.alertService.error("Selected Physician Nursing Station is InActive you can't update");
        this.alertService.error("Selected Physician Facility is InActive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
    window.scroll(0, 0);
  }
  getFetchData(physicianObj: any) {
    if (physicianObj.FacId != 0 || physicianObj.FacId != '' || physicianObj.FacId != '0') {
      let userId=this.persistanceService.get(this.config.loggedInUserKey);
      // this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + physicianObj.Facility_Id)
      this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserNurseStationDropMultiple + userId + "/" + physicianObj.FacId)

        .subscribe(res => {
          this.roleNurseStation = res;
          if (res.length == 0) {
            this.roleNurseStation = [];
            this.selectednItems = [];
            this.myform.patchValue({
              nursestationName: this.selectednItems,
            })
            // this.alertService.warn("Please select facility for nursing stations.");
          }
          this.fetchData(physicianObj);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  fetchData(res: any) {
    this.selectednItems = [];
    this.selectedfItems=[];
     this.newcredlist=[];
    this.newprimarylist=[];
    this.PhysicianId = 1;
    this.supervisst = [];
    this.oldPhyNpi=res.PhysicianNPI;
    // this.oldFacilityId=res.Facility_Id;
    this.oldFacilityId = res.FacId
    this.fetchlicenseArray = true;
    this.hidelicenseArray =true;
    
    let arraynurse = res.NursestationIds.split(',');
    // if (arraynurse.length > 0) {
    //    for (let i = 0; i < arraynurse.length; i++) {
    //      let nsexist =this.roleNurseStation.find(s=>s.NurseStation_Id == parseInt(arraynurse[i]));
    //      if(nsexist != undefined)
    //      {
    //       this.selectednItems.push(this.roleNurseStation.filter(r => r.NurseStation_Id == parseInt(arraynurse[i]))[0]);
    //      }
    // }
    // }

    if (arraynurse.length > 0) {
      this.selectednItems.push(this.roleNurseStation.filter(r => r.NurseStation_Id));

    }

    let arrayfacility = String(res.FacId).split(',');
    if (arrayfacility.length > 0) {
       for (let i = 0; i < arrayfacility.length; i++) {
         let nsexist =this.roleFacilty.find(s=>s.Facility_Id == parseInt(arrayfacility[i]));
         if(nsexist != undefined)
         {
          this.selectedfItems.push(this.roleFacilty.filter(r => r.Facility_Id == parseInt(arrayfacility[i]))[0]);
         }
    }
    }
    if(res.Credentials != null){
      this.newcredlist.push(this.credentialsList.filter(f=>f.Id == res.Credentials))
      // console.log(this.newcredlist[0][0].Name , 'name')
      this.superCredentialFlag = (this.newcredlist[0][0].Name  ===  'PA' || this.newcredlist[0][0].Name  === 'NP' || this.newcredlist[0][0].Name  === 'Other' )
      // console.log(this.superCredentialFlag  , 'flag')
    }else{
      this.newcredlist = []
    }
    // console.log(this.newcredlist, 'credlist')
    if(res.PrimarySpec !=null){
      this.newprimarylist.push(this.primarySpecialityList.filter(f=>f.Id == res.PrimarySpec))
    }else{
      this.newprimarylist
    }
    if(res.LicensesData != null && res.LicensesData.length != 0){
      this.licensearray = res.LicensesData;
      // console.log(this.licensearray,"fetch license")
      this.statelistnew=this.Statelist.filter(f=>f.State_Code == res.LicensesData[0].State);
      // this.licensenew=this.licenseList.filter(f=>f.item_name == res.LicensesData[0].City);
      this.licenseNumber = res.LicensesData[0].Number;
    }
    if(res.SupervisingPhy!=null){
      let supervislist = this.phydetails.filter(n => n.Physician_Status == 1);
      this.supervisst = supervislist.filter(f => f.PhysicianNPI == res.SupervisingPhy);
      if(this.supervisst !=null && this.supervisst.length){
        if((this.supervisst[0].Credentials == null || this.supervisst[0].Credentials =='')){
          this.supervisingInactiveModal = true;
          this.physicianAlertMsg = "Selected Supervising Physician ( " + this.supervisst[0].PhysicianName.toUpperCase() + " )  has incomplete credentialing info."
        }
        else if( (this.supervisst[0].Credentials.toLowerCase() == 'np' || this.supervisst[0].Credentials.toLowerCase() == 'pa' || this.supervisst[0].Credentials.toLowerCase() == 'Other' )){
          this.supervisingInactiveModal = true;
          this.physicianAlertMsg = "Selected Supervising Physician (" + this.supervisst[0].PhysicianName.toUpperCase() + " ) credentialing info changed to (" + this.supervisst[0].Credentials + ")."
        }
        //  this.alertService.warn(`Selected Supervising Physician (${this.supervisst[0].PhysicianName}) dont have valid Credentials`)
        }
      

    }else{
      
    }
    // this.selectedfItems.push(this.roleFacilty.filter(f=>f.Facility_Id == res.Facility_Id)[0]);
    this.myform.patchValue({
      ddlnursestation: this.selectednItems[0],
      ddlfacility:  this.selectedfItems,
      phyfirstname: res.PhysicianFName,
      phylastname: res.PhysicianLName,
      phyadd1: res.PhysicianAddress1,
      phyadd2: res.PhysicianAddress2,
      country: res.PhysicianCountryID,
      state: res.PhysicianState,
      city: res.PhysicianCity,
      zipcode: res.PhysicianZip,
      phone:res.Physician_Phone,
      status: res.Physician_Status,
      phynpi: res.PhysicianNPI,
      credentials:this.newcredlist[0], 
      primarySpeciality:this.newprimarylist[0],
      pid:res.Physician_Id,
      supervisingPhysician: this.supervisst,
      licenseform : {
        // State:this.statelistnew,
        // // license:this.licensenew,
        // number: this.licenseNumber,
  
      }
      
    });
    if( res.PhysicianState !=null &&  res.PhysicianState!="")
    {
    this.myform.controls['state'].markAsTouched();
    }
    if( res.PhysicianZip !=null &&  res.PhysicianZip!="")
    {
    this.myform.controls['zipcode'].markAsTouched();
    this.zipcodemask=this.config.zipcode;
    if(res.PhysicianZip.length<=5){
    this.zipcodeValue =res.PhysicianZip.replace('-','');
    if(res.PhysicianZip.length<5)
    {
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode5digit;
    }
    else if(res.PhysicianZip.length==6){
    if(res.PhysicianZip.indexOf("-")==-1)
    {
      this.zipcodeValue =res.PhysicianZip.match(/.{1,5}/g).join("-");
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode;      
    }
    if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length==5 && this.myform.value.zipcode.indexOf("-")!=5)
    {
      this.zipcodeisvalid=true;
    }
    else if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length>5 && this.myform.value.zipcode.length==10 && (this.myform.value.zipcode.substr(6,10).split('0').length-1!=4) && (this.myform.value.zipcode.split('-').length-1)==1 && this.myform.value.zipcode.indexOf("-")==5)
    {
      this.zipcodeisvalid=true;
    }
    else 
    {
      this.zipcodeisvalid=false;
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
  }
  if(this.myform.value.credentials[0].Name === 'NP' || this.myform.value.credentials[0].Name === 'PA' || this.myform.value.credentials[0].Name === 'Other'){
    const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
    supervisingPhysicianControl.setValidators(Validators.required);
    supervisingPhysicianControl.updateValueAndValidity();
    this.supervisFlag = true
  }else{
    this.supervisFlag = false; 
    const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
    supervisingPhysicianControl.setValidators(null);
    supervisingPhysicianControl.updateValueAndValidity();
  }
  this.updateValidations()

  }
  // getNurseStationByFacilityId() {
  //   this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationDropData)
  //     .subscribe(res => {
  //       this.roleNurseStation = res;
  //     }, error => {
  //       this.alertService.error(error.message)
  //     });
  // }
  getAllCountries() {
    this.dataservice.get<Country[]>(this.config.Common_GetAllCountries)
      .subscribe(res => {

        this.countries = res;
        this.ng4LoadingSpinnerService.hide();
        if (res != null) {
          this.myform.patchValue({
            // country: res[0].Country_Id
          });
          //this.getStatesByCountryId(res[0].Country_Id);
        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getZipCodes() {
    this.dataservice.get<ZipCode[]>(this.config.Emar_CompanyMaster_GetZipCodes)
      .subscribe(res => this.zipCodes = res, error => {
        this.alertService.error(error.message);
      });
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      //  this.inactivecheckbox=true;

      this.phyisicianList = this.phydetails.filter(n => n.Physician_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getPhysicianDetails();
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.phyisicianList.forEach(element => {
        element.Physician_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Physician_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.Physician_Status=1;
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
      item.Physician_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Physician_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.Physician_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.PhysicianNPI == item.PhysicianNPI);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updatePhysicianStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      this.dataservice.post(this.config.Emar_ResidentDemographic_UpdatePhysiciansStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getPhysicianDetails();
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
  zipcodeChange(value:any)
  {
    this.zipcodemask=this.config.zipcode;
    this.zipcodeisvalid=true;
    if(value.trim() == '')
    {
      const zipcodevalidation = this.myform.get('zipcode');
      zipcodevalidation.setValidators([Validators.required, Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]);
      //zipcodevalidation.clearValidators();
      zipcodevalidation.updateValueAndValidity();
    }
    else
    {
    const zipcodevalidation = this.myform.get('zipcode');
    zipcodevalidation.setValidators([Validators.required, Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]);
    zipcodevalidation.updateValueAndValidity();
    if(value.length<=5){
    this.zipcodeValue =value.replace('-','');
    if(value.length<5 || (value.length>6 && value.length!=10))
    {
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode5digit;
    }
    else if(value.length==6){
    if(value.indexOf("-")==-1)
    {
      this.zipcodeValue =value.match(/.{1,5}/g).join("-");
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode;      
    }
    if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length==5 && this.myform.value.zipcode.indexOf("-")==-1)
    {
      this.zipcodeisvalid=true;
    }
    else if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length>6 && this.myform.value.zipcode.length==10 && (this.myform.value.zipcode.substr(6,10).split('0').length-1!=4) && (this.myform.value.zipcode.split('-').length-1)==1 && this.myform.value.zipcode.indexOf("-")==5)
    {
      this.zipcodeisvalid=true;
    }
    else 
    {
      this.zipcodeisvalid=false;
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
  }
  }
  onCredentialsChange(item:any){
    // console.log(item, 'item')
    this.superCredentialFlag = (this.myform.value.credentials[0].Name ===  'PA' || this.myform.value.credentials[0].Name === 'NP' || this.myform.value.credentials[0].Name === 'Other' )
    // this.getSupervisingPhysicianValidators()
    if(this.superCredentialFlag){
      const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
      supervisingPhysicianControl.setValidators(Validators.required);
      supervisingPhysicianControl.updateValueAndValidity();
    }else{
      const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
    supervisingPhysicianControl.setValidators(null);
    supervisingPhysicianControl.updateValueAndValidity();
    }
    // console.log( this.superCredentialFlag,"check item")
    // this.myform.get('supervisingPhysician').setValidators(this.superCredentialFlag ? Validators.required : null); this.myform.get('supervisingPhysician').updateValueAndValidity();


  }
  onPrimarySpecialityChange(item:any){
    // console.log(item)
  }
  getcredentialsdata(){
    this.dataservice.get<any>(this.config.Emar_Common_GetCredentialMaster)
          .subscribe(res=>{
            this.credentialsList=res;
          //  console.log(this.credentialsList, 'credentials')
           
          })
          // console.log(this.phyisicianList, 'phy list for supervising ')
  }
  getPrimarySpecialtyData(){
    this.dataservice.get<any>(this.config.Emar_Common_GetPrimarySpeciality).subscribe(res=>{
      this.primarySpecialityList=res;
      // console.log(this.primarySpecialityList, 'datta')
    })

  }
  onStateSelect(item:any){
    debugger
    let statelist=[];
     statelist.push(item.item_name)
    
    // console.log(statelist, 'statelist')
    // console.log(item,'state select item')


    this.records.push(item.name)
    // this.myform.valueChanges.subscribe(res=>{
    //   this.fetchlicenseArray=false
    //   console.log(res, 'myform changes')
    // })
    this.insertlicenseArray=true
    
    // console.log(this.records, ' records2')
    
  }
  onStateDeSelect(item:any){
    // console.log(item,'state de select item')
  }
  onLicenseSelect(item:any){
// console.log(item, 'license select item')
  }
  onLicenseDeSelect(item:any){
    // console.log(item, 'license deselect item')
  }
  submitLicenseForm() {
    // this.fetchlicenseArray=false
    // console.log(this.myform.get('licenseform').valid,"boolean check")
//     if (this.myform.get('licenseform').valid) {
//       this.hidelicenseArray = true
//    // this.licensearray.push(this.myform.value.licenseform)
//     this.licensearray.push
//       ({
//             State: this.myform.value.licenseform.State[0].State_Code,
//             Number: this.myform.value.licenseform.Number,
//           });
//     // console.log(this.licensearray , "license array")
//     // if (this.licensearray.length >= this.maxRecords) {
//     //   console.log(this.licensearray.length,'length')
//     //   this.myform.get('licenseform').disable();
//     // }
  
//     // this.myform.get('licenseform').reset()
//     this.myform.get('licenseform').reset({}, { emitEvent: false });
//     this.updateValidations()
// // console.log(this.myform ,"form")  
//     // this.licensearray.forEach ((license) => {
//     //   this.myform.controls.licenseform.setValue({
//     //     State: [{ item_name: license.State }],
//     //     license: [{ item_name: license.license }],
//     //     number: license.number,
//     //   });
//     // });
   
//     }
    if (this.myform.get('licenseform').valid) {
      this.hidelicenseArray = true; 
      if(this.licensearray.length){
        let licenseCheck = this.licensearray.find(si => ((si.State == this.myform.value.licenseform.State[0].State_Code)))
   
      if(licenseCheck != undefined){
        this.alertService.warn("This state license already exists for this prescriber");
      }else{
        this.licensearray.push
        ({
              State: this.myform.value.licenseform.State[0].State_Code,
              Number: this.myform.value.licenseform.Number,
            });
      }
      }else{
        this.licensearray.push
        ({
              State: this.myform.value.licenseform.State[0].State_Code,
              Number: this.myform.value.licenseform.Number,
            });
      }
   // this.licensearray.push(this.myform.value.licenseform)
   
    // console.log(this.licensearray , "license array")
    // if (this.licensearray.length >= this.maxRecords) {
    //   console.log(this.licensearray.length,'length')
    //   this.myform.get('licenseform').disable();
    // }
  
    // this.myform.get('licenseform').reset()
    this.myform.get('licenseform').reset({}, { emitEvent: false });
    this.updateValidations()
// console.log(this.myform ,"form")  
    // this.licensearray.forEach ((license) => {
    //   this.myform.controls.licenseform.setValue({
    //     State: [{ item_name: license.State }],
    //     license: [{ item_name: license.license }],
    //     number: license.number,
    //   });
    // });
   
    }
  
  }
  removeNumber(index: number) {
    this.licensearray.splice(index, 1);
    if(this.licensearray){
      this.numberflag=false
    }
    this.updateValidations()
  }
  onsupervisingPhysicianSelect(item:any){
     const selectedPhy = this.updatedSuperphyst.filter(x=> x.PhysicianNPI == item.PhysicianNPI);
     if(selectedPhy !=null && selectedPhy.length){
       if(selectedPhy[0].Credentials == null || selectedPhy[0].Credentials =='' ){
        this.supervisingInactiveModal = true;
        this.physicianAlertMsg = "Selected Prescriber ( " + selectedPhy[0].PhysicianName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"
        // this.myform.patchValue({
        //   supervisingPhysician:''
        // })
       }
     }

    if (item ) {
      // If an item is selected, add the required validator to the form control
      const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
      supervisingPhysicianControl.setValidators(Validators.required);
      supervisingPhysicianControl.updateValueAndValidity();
      this.supervisFlag = true
    // this.myform.valueChanges.subscribe(res =>console.log(res,"ch"))
    } else{
      this.supervisFlag = false;
      const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
      supervisingPhysicianControl.setValidators(null);
      supervisingPhysicianControl.updateValueAndValidity();
    }
   
  }
 
  onsupervisingPhysicianDeSelect(item:any){
    if(item){
      if(this.myform.value.credentials[0].Name === 'NP' || this.myform.value.credentials[0].Name === 'PA' || this.myform.value.credentials[0].Name === 'Other'){
        const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
        supervisingPhysicianControl.setValidators(Validators.required);
        supervisingPhysicianControl.updateValueAndValidity();
        this.supervisFlag = true
      }else{
        this.supervisFlag = false; 
        const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
        supervisingPhysicianControl.setValidators(null);
        supervisingPhysicianControl.updateValueAndValidity();
      }
    }
    
    // console.log(item, ' supervising deselected')
  }
  updateValidations(){
    if(this.licensearray.length > 0){
      this.myform.get('licenseform.State').setValidators(null);
      this.myform.get('licenseform.Number').setValidators(null);
      this.myform.get('licenseform.State').updateValueAndValidity();
      this.myform.get('licenseform.Number').updateValueAndValidity();
    }
  }
  getStateData(){
    this.dataservice.get<any>(this.config.Emar_Common_GetState)
          .subscribe(res=>{
            this.Statelist=res;
            // console.log(this.Statelist, 'state list from api')
          
          })
          
  }
  fetchUserData(){
    //  console.log(this.TempNpi ,"dfgd")
    const Usernpi = this.TempNpi;
    // console.log(Usernpi,"Usernpi");
    // console.log(this.phydetails,"this.phyisicianList")
    // console.log(this.PhyUserDet ,"usertdetr")
    if(this.TempNpi != 0 || this.TempNpi != null || this.TempNpi != undefined){
        let editDetails = this.phyisicianList.filter(x =>x.PhysicianNPI.trim() == this.TempNpi);
      if(editDetails.length){
      // console.log(editDetails ,"editDetails")
      // PhysicianId: string,nurseStationStatus:number,facilityStatus:number,facilityId:number;

      const PhysicianId = editDetails[0].PhysicianNPI;
      const nurseStationStatus = editDetails[0].Facility_Status;
      const facilityStatus = editDetails[0].Physician_Status;
      const facilityId = editDetails[0].Facility_Id
      this.getPhysicianDetailsById(PhysicianId ,nurseStationStatus , facilityStatus ,facilityId)
      }else{
        this.alertService.warn("Please enter valid Physician NPI")
      }
    }
  }
  checkPhyArray(){
    const npi = this.myform.value.phynpi ;
     if(npi.length ==10 && this.PhysicianId ==0){
       const arr = this.phydetails.filter(f => f.PhysicianNPI == npi);
       if(arr.length){
        this.alertService.warn("Same Physician NPI is already mapped with this facility.Can't insert new record. Edit the existing record.");
       }
     }
  }
  HandlePrescriber(){
    this.myform.patchValue({
      supervisingPhysician:''
    })
    this.supervisingInactiveModal = false;
    if(this.myform.value.credentials[0].Name === 'NP' || this.myform.value.credentials[0].Name === 'PA' || this.myform.value.credentials[0].Name === 'Other'){
      const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
      supervisingPhysicianControl.setValidators(Validators.required);
      supervisingPhysicianControl.updateValueAndValidity();
      this.supervisFlag = true
    }else{
      this.supervisFlag = false; 
      const supervisingPhysicianControl = this.myform.get('supervisingPhysician');
      supervisingPhysicianControl.setValidators(null);
      supervisingPhysicianControl.updateValueAndValidity();
    }
  }
}
