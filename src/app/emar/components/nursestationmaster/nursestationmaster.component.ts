import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators, FormsModule } from '@angular/forms';
import { NurseStation, NurseShift } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { HoursMasterData, TimeFormatMasterData, PhysicianDetails } from '../../../models/orders.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-nursestationmaster',
  templateUrl: './nursestationmaster.component.html',
  styleUrls: ['./nursestationmaster.component.css'],
  providers: [DataService, APIConfiguration]
})

export class NursestationmasterComponent implements OnInit {
  myform: FormGroup;
  shiftsform: FormGroup;
  militaryshiftfrom: FormGroup;
  private nurseStation;
  public template;
  private nurseStationId: number = 0;
  public selectedfacilityItems = [];
  dropdownSettings_Facility: any = {};
  public nurseStations: NurseStation[] = [];
  TableName = "NurseStation";
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  searchText: string = "";
  p: number = 1;
  q:number = 1;
  public hoursList: HoursMasterData[];
  gridPagination = this.config.gridPagination;
  public facilities: any = [];
  public modalShiftIsOpen: boolean = false;
  public timeFormatList: TimeFormatMasterData[];
  public nurseShiftObj: NurseShift;
  public modalmilitaryShiftIsOpen: boolean = false;
  public cmpTimeFormat: number;
  public shiftsArray: NurseShift[] = [];
  public nurseStationList:any[]=[];
  public inactivecheckbox:boolean=false;
  public selectedRecords:any[]=[];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  public nurseStationListByFid:any[]=[];
  dropdownSettings_FromTime: any = {};
  dropdownSettings_ToTime: any = {};
  public selectedPhtsicianItem: any[] = [];
  public physiciansdrop: PhysicianDetails[];
  public dropdownSettings_Physician: any = {};
  pageConfig: {};
  public companyHlFlag:number=0;
  public modalAddComputerIsOpen:boolean=false;
  public computers:any[]=[];
  public nursingStationId:number;
  public nursingStationName:string="";
  public computerform:FormGroup;
  public processId: number=0;
  public AllcomputersList: any[]=[];
  public modalRemoveComputerName:boolean=false;
  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("NursingStationMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.getNurseStations();
    this.myform = new FormGroup({
      facilityName: new FormControl('', Validators.required),
      nursestationcode: new FormControl('', [ Validators.maxLength(25),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters4)]),
      nursestationname: new FormControl('', [Validators.required, Validators.maxLength(50),Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1'),
      defaultphysician: new FormControl('',),
    });
    this.shiftsform = new FormGroup({
      nextDayTime: new FormControl(false),
      fromtime: new FormControl('', Validators.required),
      fromtimeFormat: new FormControl(),
      totime: new FormControl('', Validators.required),
      totimeFormat: new FormControl(),
      shiftname: new FormControl(),
      
    });
    
    this.computerform= new FormGroup({
      computername: new FormControl('', [Validators.required, Validators.minLength(3)]),
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
    this.userActivity();
    this.getFacilityDropData();
  }
}
  else
  this.persistanceService.redirectToHomePage();
    //this.GetPhysicianDropData();
    //this.getHoursMasterData();
  }
  getHistoryById(nurseStationId: number) {
    this.auditTable = {
      "tableName": "NurseStation",
      "recordId": nurseStationId
    }
    this.modalHistoryIsOpen = true;
  }
  getHoursMasterData(facilityId:number) {
    this.dataservice.get<HoursMasterData[]>(this.config.Emar_Orders_GetHoursDataByNSId + 0 + "/" + facilityId)
      .subscribe(res => {
        this.hoursList = res;
      }, error => {
        this.alertService.error(error.message);
      });
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

  shiftSave() {
    if (this.nurseStationId == 0) {
      this.modalShiftIsOpen = false;
    }
    else {
      this.dataservice.post(this.config.Emar_NurseShift_InsertUpdateNurseShift, this.shiftsArray)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.success("Save successful");
          this.shiftsform.reset();
          this.shiftsArray = [];
          this.resetScreen();
          this.closeModel();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
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
  //  else if((this.shiftsform.value.nextDayTime == true ) && this.shiftsform.value.totime[0].Hour_Id > this.shiftsform.value.fromtime[0].Hour_Id)
  //  {
  //   this.alertService.warn("To time can't before from time.");
  //  }
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
    this.modalShiftIsOpen = false;
    this.modalAddComputerIsOpen=false;
  }
  getFacilityDropData() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllActiveFacilityDropData)
      .subscribe(res => this.facilities = res,
        error => {
          this.alertService.error(error.message)
        });

  }
  shiftsopen() {
    if (this.myform.value.facilityName == '' || this.myform.value.facilityName == null || this.myform.value.facilityName == undefined) {
      this.alertService.warn("Please select facility");
    }
    else if (this.myform.value.facilityName != '') {
      this.getHoursMasterData(this.myform.value.facilityName[0].Facility_Id);
      this.modalShiftIsOpen = true;
      this.shiftsform.reset();
    }
  }
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
      NurseStationShiftList: this.shiftsArray,
      DefaultPhysician_Id:this.myform.value.defaultphysician==null || this.myform.value.defaultphysician[0]==undefined||this.myform.value.defaultphysician.length==0?null: this.myform.value.defaultphysician[0].Physician_Id,
    };
    this.dataservice.post(this.config.Emar_Facility_InsertNurseStation, this.nurseStation)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.nurseStationId = res;
        this.alertService.success("Save successful");
        this.getNurseStations();
        this.resetScreen();
        this.inactivecheckbox=false;
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
    this.inactivecheckbox=false;
    this.physiciansdrop=[];
  }
  getNurseStations() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStations)
      .subscribe(res => {
        this.nurseStations = res;
        this.nurseStationList=this.nurseStations.filter(n=>n.NurseStation_Status==1);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseStationByFacilityID() {
    this.ng4LoadingSpinnerService.show();
    this.nurseStationListByFid =[];
    if(this.myform.value.facilityName.length==0)
      this.getNurseStations();
      else{
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStations + "/" + this.myform.value.facilityName[0].Facility_Id)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.getCompanyTimaFormat();
        if (res.length > 0)
        {
          this.nurseStationListByFid =res;
          this.nurseStationList=res.filter(n=>n.NurseStation_Status==1);
          this.inactivecheckbox=false;
        }
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
  getNurseStationDetailsByID(nurseStationId: number, facilityId: number,facilityStatus:number,companyHl:number,physicianNPI:string) {
    debugger;
    this.ng4LoadingSpinnerService.show();
    if(facilityStatus==1)
    {
    this.companyHlFlag=companyHl;
    this.dataservice.get<NurseStation>(this.config.Emar_Facility_GetNurseStationDetailsById + nurseStationId)
      .subscribe(res => {
        //this.fetchData(res);
        this.GetPhysicianDropDataForFetch(facilityId,res,physicianNPI);
        this.ng4LoadingSpinnerService.hide();
        this.getNurseShiftDetailsByID(nurseStationId, facilityId);
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else {
        if(facilityStatus==0)
        {
       // this.alertService.warn("Selected Nursing Station Facility is Inactive, can not edit the record");
       this.alertService.error("Selected Nursing Station's Company is InActive you can't update");
        this.selectedfacilityItems =[];
        }
        // else if(physicianStatus==0)
        // {
        //   this.alertService.warn("Selected Nursing Station Default Physiacian is Inactive, can not edit the record");
        //   this.selectedPhtsicianItem =[];
        // }
        this.ng4LoadingSpinnerService.hide();
      }
      window.scroll(0, 0);
  }
  getNurseShiftDetailsByID(nurseStationId: number, facilityId: number) {
    this.shiftsArray = [];
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseShifts + nurseStationId + "/" + facilityId)
      .subscribe(res => {
        this.shiftsArray = res;
      }, error => {
        this.alertService.error(error.message)
      });
  }
  GetPhysicianDropDataForFetch(facility:number,fetchObj:NurseStation,physicianNPI)
  {
    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + facility)
      .subscribe(res => {
        this.physiciansdrop = res;
        this.fetchData(fetchObj,physicianNPI);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  fetchData(res: NurseStation,physicianNPI) {
    this.selectedfacilityItems =[];
    this.selectedPhtsicianItem=[]
    this.selectedfacilityItems.push(this.facilities.filter(f => f.Facility_Id == res.Facility_Id)[0]);
    if(res.DefaultPhysician_Id !=null && this.physiciansdrop.length!=0)
    {
      this.selectedPhtsicianItem.push(this.physiciansdrop.filter(f => f.PhysicianNPI == physicianNPI)[0]);
    }
    this.myform.patchValue({
      facilityName: this.selectedfacilityItems,
      defaultphysician:this.selectedPhtsicianItem,
      nursestationcode: res.NurseStation_Code,
      nursestationname: res.NurseStation_Name,
      status: res.NurseStation_Status
    });
    this.nurseStationId = res.NurseStation_Id;
    this.getCompanyTimaFormat();
  }

  checkNurseStationName(): any {
    let NurseStation_Name = this.myform.value.nursestationname.toLowerCase();
    let result = this.nurseStations.find(x =>this.nurseStationId==0?(x.NurseStation_Name).toLowerCase() === NurseStation_Name :(x.NurseStation_Name).toLowerCase() === NurseStation_Name && x.NurseStation_Id!=this.nurseStationId);
    if (result) {
      this.alertService.error("Nursing Station Name already exists");
      this.myform.patchValue({
        nursestationname: ''
      });
    }
    else { }

  }

  checkNurseStationCode(): any {
    let NurseStation_Code = this.myform.value.nursestationcode.toLowerCase();
    let result1 = this.nurseStations.find(x =>this.nurseStationId==0?(x.NurseStation_Code).toLowerCase() === NurseStation_Code && x.NurseStation_Code!=""&& x.NurseStation_Code!=null :(x.NurseStation_Code).toLowerCase() === NurseStation_Code && x.NurseStation_Id!=this.nurseStationId && x.NurseStation_Code!=""&& x.NurseStation_Code!=null);
    if (result1) {
      this.alertService.error("Nursing Station External ID Already Exists");
      this.myform.patchValue({
        nursestationcode: ''
      });
    }
    else { }
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.NurseStationMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
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
  showInactiveRecords(value:any)
  {
    this.ng4LoadingSpinnerService.show();
    if(value==true)
    {
    //  this.inactivecheckbox=true;
    if(this.myform.value.facilityName=='' ||this.myform.value.facilityName==null || this.myform.value.facilityName==undefined)
    {
      this.nurseStationList= this.nurseStations.filter(n=>n.NurseStation_Status==0);
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
      this.nurseStationList= this.nurseStationListByFid.filter(n=>n.NurseStation_Status==0);
      this.ng4LoadingSpinnerService.hide();
    }
    }
    if(value==false)
    {
      if(this.myform.value.facilityName=='' ||this.myform.value.facilityName==null || this.myform.value.facilityName==undefined)
      this.getNurseStations();
      else
      this.getNurseStationByFacilityID();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.nurseStationList.forEach(element => {
        element.NurseStation_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.NurseStation_CreatedDate=this.dateFormatPipe.dateWithTime(new Date());
        //element.NurseStation_Status=1;
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
      item.NurseStation_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.NurseStation_CreatedDate=this.dateFormatPipe.dateWithTime(new Date());
      //item.NurseStation_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.NurseStation_Id == item.NurseStation_Id );
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateNurseStationStatus()
  {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateNursestationsStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox=false;
            this.CheckAll = false;
            this.getNurseStations();
            this.selectedRecords = [];
          }
          else if(res == 0)
          {
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
    this.selectedPhtsicianItem=[];

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
  addComputerName(nsId:number,nsName:string)
  {
    this.modalAddComputerIsOpen=true;
    this.nursingStationName=nsName;
    this.nursingStationId=nsId;
    this.getComputersListByNsId();
    this.getComputersList();
    this.computerform.reset();
  }
  saveComputerName()
  {
    let cmpName = this.computerform.value.computername.toLowerCase();
    let result1 = this.AllcomputersList.find(x =>this.processId==0?(x.ComputerName).toLowerCase() === cmpName :(x.ComputerName).toLowerCase() === cmpName && x.ProcessID!=this.processId);
    //let result1 = this.AllcomputersList.find(x => (x.ComputerName).toLowerCase() === cmpName);
    if (result1) {
      if(result1.NurseStationId==this.nursingStationId)
      this.alertService.error("Computer Name Already Exists For This Nursing Station");
      else
      this.alertService.error("Computer Name Already Exists For Another Nursing Station");

      this.computerform.patchValue({
        computername: ''
      });
    }
    else
    {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let record=
    {
      ProcessID:this.processId,
      NurseStationId:this.nursingStationId,
      ComputerName:this.computerform.value.computername,
      ProcessID_CreatedBy:userId,
    }
    this.dataservice.post(this.config.Emar_FacilityMaster_InsertUpateComputerName, record)
    .subscribe(res => {
      this.alertService.success("Save successful");
      this.computerform.reset();
      this.processId=0;
      this.getComputersList();
      this.getComputersListByNsId();
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
    }
  }
  getComputersListByNsId() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Role_GetProcessKeyMasterList+this.nursingStationId)
      .subscribe(res => {
        this.computers = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
  getComputersList()
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Role_GetProcessKeyMasterList)
      .subscribe(res => {
        this.AllcomputersList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
  checkComputerName()
  {
    let cmpName = this.computerform.value.computername!=null && this.computerform.value.computername!=undefined? this.computerform.value.computername.toLowerCase():"";
    let result1 = this.AllcomputersList.find(x =>this.processId==0?(x.ComputerName).toLowerCase() === cmpName :(x.ComputerName).toLowerCase() === cmpName && x.ProcessID!=this.processId);
  //let result1 = this.AllcomputersList.find(x => (x.ComputerName).toLowerCase() === cmpName);
    if (result1) {
      if(result1.NurseStationId==this.nursingStationId)
      this.alertService.warn("Computer Name Already Exists For This Nursing Station");
      else
      this.alertService.warn("Computer Name Already Exists For Another Nursing Station");

      this.computerform.patchValue({
        computername: ''
      });
    }
  }
  getComputerName(compName:string,processId:number)
  {
    this.processId=processId,
    this.computerform.reset();
    this.computerform.patchValue({
      computername: compName
    });
  } 
  downLoadFile(comName:any, processKey:any) {
    this.ng4LoadingSpinnerService.show();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([processKey==null?"":processKey], { type: 'text/plain' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "Key.txt";
        a.download = link.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
  }
  closeRemoveComputerModel() {
    this.modalRemoveComputerName = false;
    this.processId = 0;
  }
  openRemoveComputerName(processID: any) {
    this.processId = processID;
    this.modalRemoveComputerName = true;
  } 
  RemoveComputerName()
  {
      this.dataservice.get<any>(this.config.Emar_FacilityMaster_RemoveComputerName +this.processId)
        .subscribe(res => {
          this.modalRemoveComputerName = false;
          this.processId = 0;
          if (res == 1) {
            this.alertService.success("Computer Name Removed Successfully.");
            this.getComputersListByNsId();
            this.getComputersList();
          }
          else if (res == 0)
            this.alertService.error("Computer Name Remove failed.");
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
}
