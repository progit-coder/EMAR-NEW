import { Component, OnInit, ChangeDetectorRef, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { NurseStation, NurseShift } from '../../../models/facility.model';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { HoursMasterData, TimeFormatMasterData ,PhysicianDetails} from '../../../models/orders.model';

@Component({
  selector: 'app-bedconfignursestation',
  templateUrl: './bedconfignursestation.component.html',
  styleUrls: ['./bedconfignursestation.component.css'],
  providers: [DataService, APIConfiguration]
})
export class BedconfignursestationComponent implements OnInit {
  @ViewChild("closeAddNursingStationModal") closeNursingStationModal:ElementRef;
  myform: FormGroup;
  shiftsform: FormGroup;
  private nurseStation;
  public template;
  public facilities: any = [];
  public selectedfacilityItems = [];
  dropdownSettings_Facility: any = {};
  private nurseStationId: number = 0;
  public nurseStations: NurseStation[] = [];
  errorMessage: string;
  TableName = "NurseStation";
  auditTable: any;
  public shiftsArray: NurseShift[] = [];
  public modalHistoryIsOpen: boolean = false;
  searchText: string = "";
  public cmpTimeFormat: number;
  p: number = 1;
  public modalShiftIsOpen: boolean = false;
  public nurseStationList: any[] = [];
  gridPagination = this.config.gridPagination;
  @Output()
  NewNurseStation = new EventEmitter();
  public hoursList: HoursMasterData[];
  existsErrorMessage: string="";
  pageConfig: {};
  public companyHlFlag:number=0;
  dropdownSettings_FromTime: any = {};
  dropdownSettings_ToTime: any = {};
  public selectedPhysicianItem: any[] = [];
  public physiciansdrop: PhysicianDetails[];
  public dropdownSettings_Physician: any = {};

  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("NursingStationMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getNurseStations();
    this.getFacilityDropData();
    this.getHoursMasterData();

    this.myform = new FormGroup({
      facilityName: new FormControl('', Validators.required),
      nursestationcode: new FormControl('', [ Validators.maxLength(25),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters4)]),
      nursestationname: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1'),
      defaultphysician: new FormControl(''),
    });
    this.shiftsform = new FormGroup({
      nextDayTime: new FormControl(false),
      fromtime: new FormControl('', Validators.required),
      fromtimeFormat: new FormControl(),
      totime: new FormControl('', Validators.required),
      totimeFormat: new FormControl(),
      shiftname: new FormControl()
    });
    this.dropdownSettings_Facility = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Select",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.dropdownSettings_Physician = {
      singleSelection: true,
      idField: "Physician_Id",
      textField: "PhysicianFullName",
      text: "Select",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility'
    };
    this.dropdownSettings_FromTime = {
      singleSelection: true,
      idField: "Hour_Id",
      textField: "Hour_Desc",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true,
    };
    this.dropdownSettings_ToTime = {
      singleSelection: true,
      idField: "Hour_Id",
      textField: "Hour_Desc",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true,
    };
  }
}
  else
  this.persistanceService.redirectToHomePage();
  }
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }
  getHistoryById(nurseStationId: number) {
    this.auditTable = {
      "tableName": "NurseStation",
      "recordId": nurseStationId
    }
    this.modalHistoryIsOpen = true;
  }
  // closeModel() {
  //    this.modalHistoryIsOpen = false;
  // } 

  insertNurseStation() {
    this.ng4LoadingSpinnerService.show();
    if(this.myform.value.facilityName=='' ||this.myform.value.facilityName==null || this.myform.value.facilityName==undefined)
    {
    this.alertService.warn("Please Select Facility");
    this.ng4LoadingSpinnerService.hide();
    }
    else if(this.companyHlFlag==1 &&(this.myform.value.nursestationcode=='' ||this.myform.value.nursestationcode==undefined ||this.myform.value.nursestationcode==null))
    {
      this.alertService.warn(" Nursing Station External ID is required.");
      this.ng4LoadingSpinnerService.hide();
    }
    else{
    this.nurseStation = {
      Facility_Id: this.myform.value.facilityName[0].Facility_Id,
      NurseStation_Id: this.nurseStationId,
      NurseStation_Name: this.myform.value.nursestationname,
      NurseStation_Code: this.companyHlFlag==0 &&(this.myform.value.nursestationcode==undefined ||this.myform.value.nursestationcode==null)?null:this.myform.value.nursestationcode,
      NurseStation_Status: (this.myform.value.status == true ? 1 : 0),
      NurseStation_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      NurseStation_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
      DefaultPhysician_Id:this.myform.value.defaultphysician==null || this.myform.value.defaultphysician[0]==undefined||this.myform.value.defaultphysician.length==0?null: this.myform.value.defaultphysician[0].Physician_Id,
      NurseStationShiftList: this.shiftsArray,
    };
    this.dataservice.post(this.config.Emar_Facility_InsertNurseStation, this.nurseStation)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.nurseStationId = res;
        this.alertService.success("Save successful");
        this.NewNurseStation.emit();
        this.closeNursingStationModal.nativeElement.click();
        this.getNurseStations();
        this.resetScreen();
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
      facilityName:'',
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   nursestationname: '',
    //   nursestationcode: '',
    //   status: '1'
    // });
    this.nurseStationId = 0;
    this.getNurseStations();
    this.shiftsform.reset();
    this.shiftsArray=[];
    this.physiciansdrop=[];
    this.NewNurseStation.emit();
  }
  shiftSave() {
    if (this.nurseStationId == 0) {
      this.modalShiftIsOpen = false;
    }
    else {
      this.dataservice.post(this.config.Emar_NurseShift_InsertUpdateNurseShift, this.shiftsArray)
        .subscribe(res => {
          this.alertService.success("Save successful");
          this.shiftsform.reset();
          this.shiftsArray = [];
          this.resetScreen();
          this.closeshiftModel();
        },
          error => {
            this.alertService.error(error.message);
          });
    }
  }
  addShiftForm() {
    if(((this.shiftsform.value.fromtime==''||this.shiftsform.value.fromtime==undefined||this.shiftsform.value.fromtime==null ||this.shiftsform.value.fromtime.length==0)||(this.shiftsform.value.totime==''||this.shiftsform.value.totime==undefined||this.shiftsform.value.totime==null ||this.shiftsform.value.totime.length==0)||(this.shiftsform.value.shiftname==''||this.shiftsform.value.shiftname==undefined||this.shiftsform.value.shiftname==null)))
    {
       this.alertService.warn("Please enter data to add shift")
    }
    else if (this.shiftsform.value.fromtime[0].Hour_Id == this.shiftsform.value.totime[0].Hour_Id) {
      this.alertService.warn("Please select proper shift times");
    }
    else if((this.shiftsform.value.nextDayTime == false || this.shiftsform.value.nextDayTime == null) &&this.shiftsform.value.totime[0].Hour_Id < this.shiftsform.value.fromtime[0].Hour_Id)
    {
      this.alertService.warn("To time can't before from time.");
  }
    else 
    {
      let fromTime=this.shiftsform.value.fromtime[0].Hour_Id;
      let toTime=this.shiftsform.value.totime[0].Hour_Id;
      var shiftExist=this.shiftsArray.length>0?this.shiftsArray.find(sh=>sh.Fromtime_hoursId==fromTime && sh.Totime_hoursId==toTime):undefined;
      if(shiftExist!=undefined)
      {
        this.alertService.warn("Selected shifts times already exists");
      }
     else{
     let shiftObj: NurseShift = {
      NurseShifts_Id: 0,
      NurseStation_Id: this.nurseStationId,
      NurseShifts_Name: this.shiftsform.value.shiftname,
      Fromtime_hoursId:this.shiftsform.value.fromtime[0].Hour_Id,
      Totime_hoursId: this.shiftsform.value.totime[0].Hour_Id,
      FromHours: this.hoursList.find(h=>h.Hour_Id==this.shiftsform.value.fromtime[0].Hour_Id).Hour_Desc,
      ToHours: this.hoursList.find(h=>h.Hour_Id==this.shiftsform.value.totime[0].Hour_Id).Hour_Desc,
      FromTimeFormat: null,
      ToTimeFormat: null,
      NurseShifts_Status: (this.myform.value.status == true ? 1 : 0),
      NurseShifts_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      NurseShifts_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
    }
    this.shiftsArray.push(shiftObj);
     this.shiftsform.reset();
  }
}
}
  removeShift(i: number) {
    this.shiftsArray.splice(i, 1);
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  closeshiftModel() {
    this.shiftsform.reset();
    this.modalShiftIsOpen = false;
  }
  getFacilityDropData() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllActiveFacilityDropData)
      .subscribe(res => this.facilities = res,
        error => {
          this.alertService.error(error.message)
        });
  }
  getHoursMasterData() {
    this.dataservice.get<HoursMasterData[]>(this.config.Emar_Orders_GetHoursMasterData)
      .subscribe(res => {
        this.hoursList = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getNurseStationByFacilityID() {
    if (this.myform.value.facilityName.length == 0)
      this.getNurseStations();
    else {
      this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStations + "/" + this.myform.value.facilityName)
        .subscribe(res => {
          this.getCompanyTimaFormat();
          if (res.length > 0)
            this.nurseStationList = res;
          else
            this.nurseStationList = [];
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message)
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  getCompanyTimaFormat() {
    this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyTimeFormat + "/" + this.myform.value.facilityName[0].Facility_Id)
      .subscribe(res => {
        this.cmpTimeFormat = res;
      });
    error => {
      this.alertService.error(error.message);
    }
  }
  getNurseStations() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStations)
      .subscribe(res => {
        this.nurseStations = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage)
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseStationDetailsByID(nurseStationId: number) {
    window.scroll(0, 0);
    this.dataservice.get<NurseStation>(this.config.Emar_Facility_GetNurseStationDetailsById + nurseStationId)
      .subscribe(res => this.fetchData(res), error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
      });
  }
  fetchData(res: NurseStation) {
    this.selectedfacilityItems = [];
    this.selectedfacilityItems.push(this.facilities.filter(f => f.Facility_Id == res.Facility_Id)[0]);
    this.myform.patchValue({
      facilityName: this.selectedfacilityItems,
      nursestationcode: res.NurseStation_Code,
      nursestationname: res.NurseStation_Name,
      status: res.NurseStation_Status
    });
    this.nurseStationId = res.NurseStation_Id;
    this.getCompanyTimaFormat();
  }

  checkNurseStationName(): any {
    let NurseStation_Name = this.myform.value.nursestationname;
    let result = this.nurseStations.find(x =>this.nurseStationId==0?(x.NurseStation_Name).toLowerCase() === NurseStation_Name.toLowerCase() :(x.NurseStation_Name).toLowerCase() === NurseStation_Name.toLowerCase() && x.NurseStation_Id!=this.nurseStationId);
    if (result) {
      //this.existsErrorMessage="Nursing Station Name already exists";
      this.alertService.warn("Nursing Station Name already exists");
      this.myform.patchValue({
        nursestationname: ''
      });
    }
    else { 
      this.existsErrorMessage="";
    }
  }

  checkNurseStationCode(): any {
    let NurseStation_Code = this.myform.value.nursestationcode;
    let result1=  this.nurseStations.find(x =>this.nurseStationId==0?(x.NurseStation_Code).toLowerCase() === NurseStation_Code && x.NurseStation_Code!=""&& x.NurseStation_Code!=null :(x.NurseStation_Code).toLowerCase() === NurseStation_Code && x.NurseStation_Id!=this.nurseStationId && x.NurseStation_Code!=""&& x.NurseStation_Code!=null);
    if (result1) {
     //this.existsErrorMessage="Nursing Station External ID Already Exists";
     this.alertService.warn("Nursing Station External ID Already Exists");
      this.myform.patchValue({
        nursestationcode: ''
      });
    }
    else {
      this.existsErrorMessage="";
     }
  }
  shiftsopen() {
    if (this.myform.value.facilityName == '' || this.myform.value.facilityName == null || this.myform.value.facilityName == undefined) {
      this.alertService.warn("Please select facility");
    }
    else if (this.myform.value.facilityName != '') {
      this.modalShiftIsOpen = true;
    }
  }
  GetPhysicianDropData(facilityID:number) {
    this.ng4LoadingSpinnerService.show();
    if(this.myform.value.facilityName.length==0)
    this.physiciansdrop =[];
      else{
    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + facilityID)
      .subscribe(res => {
        this.physiciansdrop = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  onFacilitySelect(item: any) {
    this.physiciansdrop=[];
    this.getNurseStationByFacilityID();
    this.GetPhysicianDropData(item.Facility_Id);
    this.getCompanyHlSevenFlag(item.Facility_Id);
    this.shiftsArray=[];
  }
  onFacilityDeSelect(item: any) {
    this.getNurseStationByFacilityID();
    this.physiciansdrop=[];
    this.companyHlFlag=0;
    this.shiftsArray=[];
    this.selectedPhysicianItem=[];
  }
  getCompanyHlSevenFlag(facilityId:number)
  {
    this.dataservice.get<any>(this.config.Emar_Company_GetCompanyHlSevenFlag + facilityId +"/"+ 0)
        .subscribe(res => {
          this.companyHlFlag=res;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
}
