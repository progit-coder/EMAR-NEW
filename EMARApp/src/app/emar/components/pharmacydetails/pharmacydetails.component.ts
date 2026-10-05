import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators , ValidationErrors, ValidatorFn } from '@angular/forms';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from 'src/app/_services';
import { APIConfiguration } from 'src/app/models/app.constants';
import { Country, State, City, ZipCode } from '../../../models/common.model';
import { PharmacyModel } from 'src/app/models/pharmacy.model';
import { RoleFacility, RoleNurseStation } from 'src/app/models/role.model';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
import { DataService } from 'src/app/services/shared/dataservice.service';
import { PersistanceService } from 'src/app/services/shared/persistance.service';
import { SharedService } from 'src/app/services/shared/shared.service';
import { TextMaskModule } from 'angular2-text-mask';
import { Screens,Activity } from '../../../models/useractivity.model';
import { NurseStation } from 'src/app/models/facility.model';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-pharmacydetails',
  templateUrl: './pharmacydetails.component.html',
  styleUrls: ['./pharmacydetails.component.css']
})
export class PharmacydetailsComponent implements OnInit {
  myform: FormGroup;
  public nurseStations: NurseStation[];
  public template;
  pageConfig: {};
  public selectedfItems=[];
  public selectednItems = [];
  dropdownSettings_Facility: any = {};
  public roleNurseStation: RoleNurseStation[];
  errorMessage: string;
  ShowFilter = true;
  dropdownSettings_NurseStation: any = {};
  public roleFacilty: RoleFacility[];
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public zipcode = this.config.zipcode;
  private zipCodes: ZipCode[];
  public zipcodemask:any;
  public countries: Country[];
  public nstations: string = "";
  public arNurseStations: any[];
  public savedisable=false;
  private PhysicianObj: PharmacyModel;
  public oldFacilityId:number;
  public userId: number = this.persistanceService.get(this.config.loggedInUserKey);
  searchText: string = "";
  public zipcodeisvalid=true;
  public zipcodeValue:string="";
  public mobileNumberMask = this.config.mobileNumberMask;
  public pharmacyncpdp=this.config.pharmacyncpdp;
  public pharmacynpi=this.config.pharmacynpi;
  public PharmacyId: number=0;
  public phyisicianList: any[] = [];
  public phydetails = [];
  public UpdateStatus: boolean = true;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public inactivecheckbox: boolean = false;
  public modalHistoryIsOpen: boolean = false;
  auditTable: any;
  gridPagination = 10;
  p: number = 1;

  isClicked: boolean = false;
  public facilityNurseStations: any[] = [];
  public statusList: any[];
  pharmacyIds:any;
  pids={};




  constructor(private dataservice: DataService,
     private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,
      private config: APIConfiguration, private dateFormatPipe: CustomdatePipe,
      private alertService: AlertService, private persistanceService: PersistanceService,
      public sharedService: SharedService  ,  private readonly changeDetectorRef: ChangeDetectorRef ,  private modalService: NgbModal) { }


  ngOnInit() {

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
      phylastname: new FormControl('', [Validators.required,Validators.maxLength(25),Validators.minLength(3), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      phyadd1: new FormControl('', [Validators.required,Validators.maxLength(100)]),
      city: new FormControl('', [Validators.required,Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters5)]),
      state: new FormControl('', [Validators.required,Validators.maxLength(2),Validators.minLength(2), Validators.pattern(this.config.alphabets)]),
      status:new FormControl('1'),
      ncpdp: new FormControl('', [Validators.required,Validators.maxLength(7),Validators.minLength(7), Validators.pattern(this.config.numeric)]),
      npi:new FormControl('',[Validators.maxLength(10),Validators.minLength(10), Validators.pattern(this.config.numeric)]),
      // myCheckboxGroup : new FormGroup({
      //   retail: new FormControl(false),
      //   mail: new FormControl(false),
      //   speciality: new FormControl(false),
      //   ltc: new FormControl(false),
      //   ihd: new FormControl(false),
      // }, [this.requireAtLeastOneCheckboxCheckedValidator()]),
      //   myCheckboxOptions : new FormGroup({
      //   hours: new FormControl(false ),
      //   epcs: new FormControl(false ),
      // }, [this.requireAtLeastOneCheckboxCheckedValidator()]),

      zipcode: new FormControl('', [Validators.required,Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]),
      contactNumber: new FormControl('', [Validators.required, Validators.minLength(14)]),
      fax: new FormControl('', [Validators.minLength(14)]),
    });
    this.dropdownSettings_Facility = {
      singleSelection: true,
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
    //this.getAllCountries();
    //this.getRolefacilityMasterData();
    //this.getNurseStationByFacilityId();
    this.getPhysicianDetails();
    this.getUserRecentFacilityNurseStations();
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {
    this.sharedService.insertUserActivityDetails(Screens.PharmacyDetails,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  requireAtLeastOneCheckboxCheckedValidator(): ValidatorFn {
    return (formGroup: FormGroup): ValidationErrors | null => {
      const checkboxGroup = formGroup.controls;
      const checkboxes = Object.values(checkboxGroup);
      const checked = checkboxes.some(checkbox => checkbox.value === true);
      return checked ? null : { requireAtLeastOneCheckboxChecked: true };
    };
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


  getRolefacilityMasterData() {
    let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions+ loginUser)
      .subscribe(res =>
        {
          this.roleFacilty =  res.Facilities;

          if (this.loginUserReceFacility != null) {
            if (this.roleFacilty.length > 0) {
              let checkFacExist = this.roleFacilty.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
              this.selectedfaItems = [];
              if (checkFacExist != undefined) {
                this.selectedfaItems.push(checkFacExist);
                this.getRoleNurseStationMasterData(this.loginUserReceFacility);
              }
              this.myform.patchValue({
                ddlfacility: this.selectedfaItems,
              });
            }
          }

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
  getNurseStations() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserNurseStations + userId)
      .subscribe(res => {
        this.nurseStations = res;
        //this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getRoleNurseStationMasterData(facilityId: number) {
    if (facilityId != 0) {
      let loginUser=this.persistanceService.get(this.config.loggedInUserKey);
      this.dataservice.get<RoleNurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + loginUser + "/" + facilityId)
        .subscribe(res => {
          this.roleNurseStation = res;
          console.log( this.roleNurseStation, ' role nurse station responce')

          if (this.loginUserReceNurseStation != undefined) {
            let userReceNSList = this.loginUserReceNurseStation.split(',');
            if (userReceNSList.length > 0) {
              this.selectednItems = [];
              for (let i = 0; i < userReceNSList.length; i++) {
                let checkNsExist = this.roleNurseStation.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
                if (checkNsExist != undefined) {
                  this.selectednItems.push(checkNsExist);
                }
              }
              this.myform.patchValue({
                ddlnursestation: this.selectednItems,
              });

            }
          }

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
              closeDropDownOnSelection:true,

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
  onFacilitySelect(item: any) {
    this.selectednItems=[];
    this.myform.patchValue({ ddlnursestation: '' });
    this.myform.value.ddlfacility = item;
    this.getRoleNurseStationMasterData(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.getRoleNurseStationMasterData(0);
    this.myform.patchValue({ ddlnursestation: '' });
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
  insertPhysicianDetails(){

        this.savedisable=true;
        this.ng4LoadingSpinnerService.show();
        this.nstations = '';
        this.arNurseStations = this.myform.value.ddlnursestation;
        console.log(this.arNurseStations , "formcontrol")
        this.arNurseStations.forEach(element => {
        this.nstations += element.NurseStation_Id + ",";
          });
          console.log(this.nstations , "for each nurse station")
          this.nstations = this.nstations.substring(0, this.nstations.length - 1);
          console.log(this.nstations , "for each nurse station after length")
          console.log(this.PharmacyId ,"ph id")
        this.PhysicianObj = {

          Pharmacy_Name: this.myform.value.phylastname,
          Pharmacy_Address1: this.myform.value.phyadd1,
          Pharmacy_Address2: "dummy data",
          Pharmacy_City: this.myform.value.city,
          Pharmacy_State: this.myform.value.state,
          Pharmacy_Zip: this.myform.value.zipcode,
          Pharmacy_Status: this.myform.value.status == true ? 1 : 0,
          Pharmacy_24_Hours: 0,
          Pharmacy_EPCS_Enabled : 0,
          Pharmacy_IHD: 0,
          Pharmacy_LTC:0,
          Pharmacy_Mail_Order:0,
          Pharmacy_Retail: 0,
          Pharmacy_Speciality: 0,
          // Pharmacy_Retail: this.myform.value.myCheckboxGroup.retail == true ? 1 : 0,
          // Pharmacy_Mail_Order: this.myform.value.myCheckboxGroup.mail == true ? 1 : 0,
          // Pharmacy_Speciality: this.myform.value.myCheckboxGroup.speciality == true ? 1 : 0,
          // Pharmacy_LTC: this.myform.value.myCheckboxGroup.ltc == true ? 1 : 0,
          // Pharmacy_IHD: this.myform.value.myCheckboxGroup.ihd == true ? 1 : 0,
          // Pharmacy_24_Hours:this.myform.value.myCheckboxOptions.hours == true ? 1 : 0,
          // Pharmacy_EPCS_Enabled:this.myform.value.myCheckboxOptions.epcs == true ? 1 : 0,
          Pharmacy_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          Facility_Id:this.myform.value.ddlfacility[0].Facility_Id,
          NurseStation_Id: this.nstations,
          OldFacilityId:this.oldFacilityId,
          Pharmacy_Phone : this.myform.value.contactNumber,
          Pharmacy_Id: this.PharmacyId,
          Pharmacy_Fax: this.myform.value.fax,
          Pharmacy_CountryId:1,
          NCPDP:this.myform.value.ncpdp,
          NPI:this.myform.value.npi,

        }
        console.log(this.PhysicianObj,"main res")

        this.dataservice.post(this.config.Emar_Pharmacy_insertUpdateData, this.PhysicianObj).subscribe(res =>{
          this.ng4LoadingSpinnerService.hide();
            if (res == 1) {
              this.alertService.success("Save successful");
            console.log(res,'updated data')
            this.getPhysicianDetails();
            this.resetScreen();
            this.savedisable=false;
            console.log(res  )
        }
        },
          _error => {
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
        console.log(this.PhysicianObj )


      }





  getPhysicianDetails() {

    debugger;
    let userId=this.persistanceService.get(this.config.loggedInUserKey);
    console.log(userId);
    this.dataservice.get<any[]>(this.config.Emar_pharmacy_getPharmacyData + userId )
    .subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      this.phydetails = res;

      this.phyisicianList = this.phydetails.filter(p => p.Pharmacy_Status == 1);
      console.log(this.phyisicianList,"pharmacy data")

      // this.phyisicianList = this.phydetails.filter(p => p.Pharmacy_Status == 0);
      // console.log(this.phyisicianList,"pharmacy data with status 0")
      this.ng4LoadingSpinnerService.hide();

    },
      error => {
        this.alertService.error(error.Message);
        this.ng4LoadingSpinnerService.hide();
      }
    );

  }

  getPhysicianDetailsById(PharmacyId: number) {
    debugger;
    this.ng4LoadingSpinnerService.show();
    // window.scroll(0, 0);
    // function high(edit);
    this.selectednItems = [];
    this.dataservice.get<any>(this.config.Emar_GetPharmacyInfo_ById + PharmacyId )
      .subscribe(res => {
        this.fetchData(res);
        // this.updatestatus(res)
        console.log(res,"get pd details")
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }

 toggleS(PharmacyId: number, favourite: number ){
  debugger;
    this.ng4LoadingSpinnerService.show();
    favourite == 1 ? 1 : 0;
    let userId=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Emar_update_PharmacyFav +  PharmacyId + "/" + favourite + "/" + userId )
      .subscribe(res => {
        this.getPhysicianDetails()


        this.ng4LoadingSpinnerService.hide();
      },
       error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });

}



  resetNotMandatoryFields(value: any) {
    // if (value == 'Pharmacy_City' && this.myform.value.city == '') {
    //   this.myform.controls['city'].reset();
    // }
    // if (value == 'Pharmacy_State' && this.myform.value.state == '') {
    //   this.myform.controls['state'].reset();
    // }
    // if (value == 'Pharmacy_Phone' && this.myform.value.contactNumber == '') {
    //   this.myform.controls['contactNumber'].reset();
    // }
    if (value == 'Pharmacy_Fax' && this.myform.value.fax == '') {
      this.myform.controls['fax'].reset();
    }
    // if (value == 'Pharmacy_Zip' && this.myform.value.zipcode == '') {
    //   this.myform.controls['zipcode'].reset();
    // }

  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.selectednItems = [];
    this.selectedfItems=[];
    this.selectedfItems.push(this.roleFacilty.filter(f=>f.Facility_Id)[0]);
    this.selectednItems.push(this.roleNurseStation.filter(n => n.NurseStation_Id )[0]);
    this.myform.patchValue({
      ddlnursestation: this.selectednItems,
      ddlfacility:  this.selectedfItems,
      country: '1',
      status: '1',
    })
    this.ng4LoadingSpinnerService.hide();
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode;
    this.zipcodeisvalid=true;
    this.PharmacyId = 0;
    this.oldFacilityId=0;
    this.inactivecheckbox =  false;


  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      //  this.inactivecheckbox=true;

      this.phyisicianList = this.phydetails.filter(n => n.Pharmacy_Status == 0);
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
      element.Pharmacy_Id;
       this.selectedRecords.push( element.Pharmacy_Id);
    });
    this.pharmacyIds = this.selectedRecords.join(',');

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
      this.selectedRecords.push( item.Pharmacy_Id);
      this.pharmacyIds = this.selectedRecords.join(',')
    }
    else {
      const index = this.selectedRecords.findIndex(i => i == item.Pharmacy_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }


  fetchData(res: any) {
    debugger;
    console.log(res,' ncpdp')
    this.selectednItems = [];
      this.selectedfItems=[];
      this.PharmacyId = res[0].Pharmacy_Id;
      this.selectedfItems.push(this.roleFacilty.filter(f=>f.Facility_Id)[0]);
      if(res.length > 0){
        let arraynurse = res[0].NurseStation_Name.split(',');
        console.log(arraynurse ,"array nurse")
        if (arraynurse.length > 0) {
           for (let i = 0; i < arraynurse.length; i++) {
             let nsexist =this.roleNurseStation.find(s=>s.NurseStation_Name == arraynurse[i]);
             if(nsexist != undefined)
             {
              this.selectednItems.push(this.roleNurseStation.filter(r => r.NurseStation_Name == arraynurse[i])[0]);
             }
        }
        }
      }

    this.myform.patchValue({
      ddlnursestation: this.selectednItems,

      ddlfacility:  this.selectedfItems,
      phylastname: res[0].Pharmacy_Name,
      phyadd1: res[0].Pharmacy_Address1,
      //country: res.PhysicianCountryID,
      state: res[0].Pharmacy_State,
      city: res[0].Pharmacy_City,
      zipcode: res[0].Pharmacy_Zip,
      status: Number( res[0].Pharmacy_Status),
      // myCheckboxGroup : {
      //   retail: res[0].Pharmacy_Retail == "1" ? true : false,
      //   mail: res[0].Pharmacy_Mail_Order == "1" ? true : false,
      //   speciality: res[0].Pharmacy_Speciality == "1" ? true : false,
      //   ltc:res[0].Pharmacy_LTC == "1" ? true : false,
      //   ihd: res[0].Pharmacy_IHD == "1" ? true : false,
      // },
      // myCheckboxOptions : {
      //   hours: res[0].Pharmacy_24_Hours == "1" ? true : false,
      //   epcs:res[0].Pharmacy_EPCS_Enabled == "1" ? true : false,
      // },
      fax:res[0].Pharmacy_Fax,
      contactNumber:res[0].Pharmacy_Phone,
      ncpdp:res[0].NCPDP,
      npi:res[0].NPI


    });
    if( res[0].Pharmacy_Zip !=null &&  res[0].Pharmacy_Zip!="")
    {
    this.myform.controls['zipcode'].markAsTouched();
    this.zipcodemask=this.config.zipcode;
    if(res[0].Pharmacy_Zip.length<=5){
    this.zipcodeValue =res[0].Pharmacy_Zip.replace('-','');
    if(res[0].Pharmacy_Zip.length<5)
    {
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode5digit;
    }
    else if(res[0].Pharmacy_Zip.length==6){
    if(res[0].Pharmacy_Zip.indexOf("-")==-1)
    {
      this.zipcodeValue =res[0].Pharmacy_Zip.match(/.{1,5}/g).join("-");
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

  }


  getHistoryById(PharmacyId: number) {
    this.auditTable = {
      "tableName": "PharmacyDetails",
      "recordId": PharmacyId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  ngAfterViewChecked(): void {
    this.changeDetectorRef.detectChanges();
  }
  getAllCountries() {
    this.dataservice.get<Country[]>(this.config.Common_GetAllCountries)
      .subscribe(res => {

        this.countries = res;
        this.ng4LoadingSpinnerService.hide();
        if (res != null) {
          this.myform.patchValue({

          });

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
  zipcodeChange(value:any)
  {
    this.zipcodemask=this.config.zipcode;
    this.zipcodeisvalid=true;
    if(value.trim() == '')
    {
      const zipcodevalidation = this.myform.get('zipcode');
      zipcodevalidation.setValidators([ Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]);
      //zipcodevalidation.clearValidators();
      zipcodevalidation.updateValueAndValidity();
    }
    else
    {
    const zipcodevalidation = this.myform.get('zipcode');
    zipcodevalidation.setValidators([  Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]);
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
  updatePhysicianStatus() {

    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      this.pids=this.pharmacyIds
      const objectData = { Status: this.pids };
      console.log(objectData ,"selected update")
      this.dataservice.post(this.config.Emar_update_PharmacyStatus , objectData )
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




}
