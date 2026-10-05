import { Component, OnInit } from '@angular/core';
import { Floor, NurseStation, Wing } from '../../../models/facility.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import { AlertService } from '../../../_services/index';
import { Validators, FormControl, FormGroup } from '@angular/forms';
import { FrequencyMasterData, WeekMasterData, MonthMasterData, HoursMasterData, TimeFormatMasterData } from '../../../models/orders.model';
import { NursingFrequencyConfig } from '../../../models/emar.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';


@Component({
  selector: 'app-frequencymapping',
  templateUrl: './frequencymapping.component.html',
  styleUrls: ['./frequencymapping.component.css'],
  providers: [DataService, APIConfiguration]
})
export class FrequencymappingComponent implements OnInit {

  public nurseStations: NurseStation[];
  public frequencyList: FrequencyMasterData[];
  private nursingfrequencyObj: NursingFrequencyConfig;
  public frequencyConfigs: any[] = [];
  public floor = new Floor();
  public weeksList: WeekMasterData[];
  public monthsList: MonthMasterData[];
  public floorMaster: Floor[];
  public wings: Wing[];
  public wing = new Wing();
  public hoursList: HoursMasterData[];
  public timeFormatList: TimeFormatMasterData[];
  searchText: string = "";
  gridPagination = this.config.gridPagination;
  errorMessage: string;
  public template;
  public dropdownList = [];
  public selectedItems = [];
  public dropdownSettings = {};
  ShowFilter = true;
  public item_id: number;
  public nursingFreqId: number = 0;
  myform: FormGroup;
  TableName = "NursingFrequencyConfig";
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  public weeks: any[];
  public mon: number = 0;
  public tue: number = 0;
  public wed: number = 0;
  public thu: number = 0;
  public fri: number = 0;
  public sat: number = 0;
  public sun: number = 0;
  p: number = 1;
  public selectedfrequencyItems = [];
  public selectedfItems = [];
  public selectednItems = [];
  dropdownSettings_Facility: any = {};
  dropdownSettings_Frequency: any = {};
  dropdownSettings_NurseStation: any = {};
  public facilities: any[] = [];
  public nstations: string = "";
  public facilityNurseStations: any[] = [];
  public arNurseStations: any[];
  public timesArray: any[] = [];
  public isReadOnly: boolean = false;
  public times: string = "";
  timeform: FormGroup;
  public modalTimeIsOpen: boolean = false;
  public selectedstItems=[];
  dropdownSettings_StartTime: any = {};
  public selectedntItems=[];
  dropdownSettings_NextTime: any = {};
  public userId:number;
  public frequencyId:number=0;
  public savedisable=false;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  pageConfig: {};
  public oldFrequencyId:number=0;
  public oldFacilityId:number=0;
  public oldNursingStationId:number=0;

  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private sharedService: SharedService, private alertService: AlertService,
    private persistanceService: PersistanceService) {
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("FrequencyMapping");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.ng4LoadingSpinnerService.show();
    this.dropdownList = [
      { item_id: 1, item_text: 'Monday' },
      { item_id: 2, item_text: 'Tuesday' },
      { item_id: 3, item_text: 'Wednesday' },
      { item_id: 4, item_text: 'Thursday' },
      { item_id: 5, item_text: 'Friday' },
      { item_id: 6, item_text: 'Saturday' },
      { item_id: 7, item_text: 'Sunday' },
    ];
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      itemsShowLimit: 1,
      allowSearchFilter: false,
    };
    this.myform = new FormGroup({
      facilityname: new FormControl('', Validators.required),
      nursestationName: new FormControl('', Validators.required),
      frequency: new FormControl('', Validators.required),
      floorName: new FormControl(''),
      wingName: new FormControl(''),
      time: new FormControl(''),
      timeFormat: new FormControl(''),
      hours: new FormControl('', [Validators.maxLength(2), Validators.pattern(this.config.numeric),Validators.min(1),Validators.max(23)]),
      week: new FormControl(''),
      month: new FormControl(''),
      oDay: new FormControl('', [Validators.maxLength(4), Validators.pattern(this.config.numeric)]),
      through: new FormControl('', [Validators.maxLength(4), Validators.pattern(this.config.numeric)]),
      aDay: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric),Validators.min(1)]),
      hDay: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric),Validators.min(1)]),
      weeksdata: new FormControl(''),
      // mon: new FormControl('', Validators.required),
      // tue: new FormControl('', Validators.required),
      // wed: new FormControl('', Validators.required),
      // thu: new FormControl('', Validators.required),
      // fri: new FormControl('', Validators.required),
      // sat: new FormControl('', Validators.required),
      // sun: new FormControl('', Validators.required),

    });
    this.timeform = new FormGroup({
      starttime: new FormControl(''),
    })
    this.selectedItems = this.dropdownList;
    this.myform.patchValue({
      weeksdata: this.selectedItems,
    });

    this.dropdownSettings_Facility = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Select Facility",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.dropdownSettings_NurseStation = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Select Nursing Stations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      noDataAvailablePlaceholderText: 'Please Select Facility'
    };
    this.dropdownSettings_Frequency = {
      singleSelection: true,
      idField: "Frequency_Id",
      textField: "Frequency_Name",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_StartTime = {
      singleSelection: true,
      idField: "Hour_Id",
      textField: "Hour_Desc",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility'
    };
    this.dropdownSettings_NextTime = {
      singleSelection: true,
      idField: "Hour_Id",
      textField: "Hour_Desc",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter,
    };
    //this.getFacilityDropData()
    this.getFrequencyMasterData();
    //this.getFloorWingDrop();
    //this.getHoursMasterData();
    //this.getTimeFormatMasterData();
    this.getWeekMasterData();
    this.getMonthMasterData();
    this.getNursingFrequencyConfigData();
    this.userActivity();

    this.getUserRecentFacilityNurseStations();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.FrequencyMapping, Activity.View, '')
      .subscribe(res => { }, error => {
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
        this.getFacilityDropData();
      }, error => {
        this.alertService.error(error.message);
      });
  }

  getHistoryById(nurseFreqId: number) {
    this.auditTable = {
      "tableName": "NursingFrequencyConfig",
      "recordId": nurseFreqId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.modalTimeIsOpen = false;
    this.timeform.reset();
    if(this.timesArray.length==0)
    {
      this.myform.controls['time'].reset();
    }
  }
  getFacilityDropData() {
    this.dataservice.get<any>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + this.userId)
      .subscribe(res => 
        {
          this.facilities = res.Facilities;
          if (this.loginUserReceFacility != null) {
            if (this.facilities.length > 0) {
              let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
              this.selectedfaItems = [];
              if (checkFacExist != undefined) {
                this.selectedfaItems.push(checkFacExist);
                this.getFacilityNurseStationMasterData(this.loginUserReceFacility);
                this.getHoursMasterData(this.loginUserReceFacility);
              }
              this.myform.patchValue({
                facilityname: this.selectedfaItems,
              });
            }
          }

          else if(this.facilities.length==1)
          {
            this.getFacilityNurseStationMasterData(this.facilities[0].Facility_Id);
            this.getHoursMasterData(this.facilities[0].Facility_Id);
            this.myform.patchValue({
              facilityname:this.facilities,
            });
          }
        },
        error => {
          this.alertService.error(error.message)
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
  getFloorWingDrop() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetCompanyToBedData + userId)
      .subscribe((res: any) => {
        this.floorMaster = res.Floors;
        this.wings = res.Wings;
        this.ng4LoadingSpinnerService.hide();
        //this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getFrequencyMasterData() {
    this.dataservice.get<FrequencyMasterData[]>(this.config.Emar_Orders_GetFrequencyMasterData)
      .subscribe(res => {
        this.frequencyList = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  // getTimeFormatMasterData() {
  //   this.dataservice.get<TimeFormatMasterData[]>(this.config.Emar_Orders_GetTimeFormatMasterData)
  //     .subscribe(res => {
  //       this.timeFormatList = res;
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  getWeekMasterData() {
    this.dataservice.get<WeekMasterData[]>(this.config.Emar_Orders_GetWeekMasterData)
      .subscribe(res => {
        this.weeksList = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

    this.myform.patchValue({
      week: '1',
    });
  }
  getMonthMasterData() {
    this.dataservice.get<MonthMasterData[]>(this.config.Emar_Orders_GetMonthMasterData)
      .subscribe(res => {
        this.monthsList = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

    this.myform.patchValue({
      month: '13',
    });

  }
  insertNursingfrequencyConfigsData() {
    this.savedisable=true;
    this.ng4LoadingSpinnerService.show();
  //   if (this.myform.value.hours != '' && this.timesArray.length > 1) {
  //     this.alertService.error("At once hours and multiple time can't insert.");
  //     this.savedisable=false;
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //  else
   if (this.myform.value.time.length == 0 && (this.frequencyList.find(f=>f.Frequency_Id==this.myform.value.frequency[0].Frequency_Id).Frequency_PRN !=1)) {
      this.alertService.warn("Please select start time.");
      this.savedisable=false;
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.timesArray.length!=this.myform.value.hours && (this.frequencyList.find(f=>f.Frequency_Id==this.myform.value.frequency[0].Frequency_Id).Frequency_PRN !=1))
    {
      let FrequnceLimitAlertText1 = this.frequencyList.find(f=>f.Frequency_Id== this.myform.value.frequency[0].Frequency_Id).Freq_Descalert1;
      this.alertService.warn(FrequnceLimitAlertText1);
      this.savedisable=false;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.nstations = '';
      this.times = '';
      this.arNurseStations = this.myform.value.nursestationName;
      this.arNurseStations.forEach(element => {
        this.nstations += element.NurseStation_Id + ",";
      });
      this.nstations = this.nstations.substring(0, this.nstations.length - 1);
      if(this.timesArray.length==0 ||this.timesArray==null)
    {
     if((this.timesArray.length==0 ||this.timesArray==null) && (this.myform.value.time == '' || this.myform.value.time == null || this.myform.value.time == undefined)&&(this.frequencyList.find(f=>f.Frequency_Id==this.myform.value.frequency[0].Frequency_Id).Frequency_PRN==1)) 
    {
      this.times='';
    }
    else if((this.myform.value.time!=undefined||this.myform.value.time!=null||this.myform.value.time!='')&&(this.frequencyList.find(f=>f.Frequency_Id==this.myform.value.frequency[0].Frequency_Id).Frequency_PRN!=1))
    {
      this.times=this.myform.value.time[0].Hour_Id;
    }
  }
    else{
    this.timesArray.forEach(element => {
      this.times += element.Hour_Id + ",";
    });
    this.times = this.times.substring(0, this.times.length - 1);
  }
      this.weeks = this.myform.value.weeksdata;
      if(this.weeks.length!=0){
      if (this.weeks.filter(e => e.item_id === 1).length) {
        this.mon = 1;
      }
      else
        this.mon = 0;
      if (this.weeks.filter(e => e.item_id === 2).length) {
        this.tue = 1;
      }
      else
        this.tue = 0;
      if (this.weeks.filter(e => e.item_id === 3).length) {
        this.wed = 1;
      }
      else
        this.wed = 0;
      if (this.weeks.filter(e => e.item_id === 4).length) {
        this.thu = 1;
      }
      else
        this.thu = 0;
      if (this.weeks.filter(e => e.item_id === 5).length) {
        this.fri = 1;
      }
      else
        this.fri = 0;
      if (this.weeks.filter(e => e.item_id === 6).length) {
        this.sat = 1;
      }
      else
        this.sat = 0;
      if (this.weeks.filter(e => e.item_id === 7).length) {
        this.sun = 1;
      }
      else
        this.sun = 0;
    }
    else{
      this.mon = 0;
      this.tue = 0;
      this.wed=0;
      this.thu=0;
      this.fri=0;
      this.sat=0;
      this.sun=0;
    }

      this.nursingfrequencyObj =
        {
          NursingFreq_Id: this.nursingFreqId,
          Facility_Id: this.myform.value.facilityname[0].Facility_Id,
          NursingStations: this.nstations,
          Frequency_Id: this.myform.value.frequency[0].Frequency_Id,
          StartTime: null,
          Times: this.times,
          TimeFormat_ID: null,
          Hours: this.myform.value.hours,
          Monday: this.mon,
          Tuesday: this.tue,
          Wednesday: this.wed,
          Thursday: this.thu,
          Friday: this.fri,
          Saturday: this.sat,
          Sunday: this.sun,
          Week_Id: this.myform.value.week,
          Month_Id: null,
          OnlyOnDay: null,
          ThroughDay: null,
          ActiveDays: this.myform.value.aDay,
          HoldDays: this.myform.value.hDay,
          NursingFreq_Status: 1,
          NursingFreq_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          NursingFreq_CreatedDate: new Date().toISOString(),
          OldFrequencyId:this.oldFrequencyId,
          OldFacilityId:this.oldFacilityId,
          OldNursingStationId:this.oldNursingStationId,
        }
      // this.ng4LoadingSpinnerService.show();
      this.dataservice.post(this.config.Emar_InsertNursingFrequencyConfigData, this.nursingfrequencyObj)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.success("Save successful");
          this.getNursingFrequencyConfigData();
          this.arNurseStations = [];
          this.reSet();
          this.savedisable=false;
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
          this.savedisable=false;
        });
    }
   
  }
  getHoursMasterData(facilityId: any) {
    this.dataservice.get<HoursMasterData[]>(this.config.Emar_Orders_GetHoursDataByNSId + 0 + "/" + facilityId)
      .subscribe(res => {
        this.hoursList = res;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNursingFrequencyConfigData() {
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_GetNursingFrequencyConfigData + userId)
      .subscribe(res => {
        this.frequencyConfigs = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  getNursingFrequencyConfigDataByID(nurseFreqId: number,facilityStatus:number,nurseStationStatus:number) {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.timesArray=[];
    this.timeform.reset();
    if(facilityStatus==1 && nurseStationStatus==1)
    {
    this.dataservice.get<any>(this.config.Emar_GetNursingFrequencyConfigByID + nurseFreqId)
      .subscribe(res => {
        this.timesArray = res.HoursList == null ? [] : res.HoursList;
        if (this.timesArray.length > 1) {
          this.isReadOnly = true;
        }
        else {
          this.isReadOnly = false;
        }
        this.getHours(res);
        //this.getFetchData(res);
        //this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else if(facilityStatus==0 && nurseStationStatus==0)
      {
        this.alertService.error("Selected Frequency Facility and Nursing Station both InActive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
      else if(facilityStatus==0)
      {
        this.alertService.error("Selected Frequency Facility is InActive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
      else if(nurseStationStatus==0)
      {
        this.alertService.error("Selected Frequency Nursing Station is InActive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
    window.scroll(0, 0);
  }
  getHours(fetch:any)
  {
    this.dataservice.get<HoursMasterData[]>(this.config.Emar_Orders_GetHoursDataByNSId + 0 + "/" + fetch.Facility_Id)
    .subscribe(res => {
      this.hoursList = res;
      this.getFetchData(fetch);
    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
  fetchData(res: any) {
    if(res.Frequency_Id ==28)
    {
      this.isReadOnly =true;
    }
    this.selectedfrequencyItems = [];
    this.selectedItems = [];
    this.selectedfItems = [];
    this.selectednItems = [];
    this.selectedstItems=[];
    this.mon = res.Monday;
    this.tue = res.Tuesday;
    this.wed = res.Wednesday;
    this.thu = res.Thursday;
    this.fri = res.Friday;
    this.sat = res.Saturday;
    this.sun = res.Sunday;
    this.frequencyId=res.Frequency_Id;
    this.selectedfrequencyItems.push(this.frequencyList.filter(fr => fr.Frequency_Id == res.Frequency_Id)[0]);
    this.selectedfItems.push(this.facilities.filter(f => f.Facility_Id == res.Facility_Id)[0]);
    this.selectednItems.push(this.facilityNurseStations.filter(n => n.NurseStation_Id === res.NursingStation_Id)[0]);
    if (this.mon == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 1)[0]);
    }
    if (this.tue == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 2)[0]);
    }
    if (this.wed == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 3)[0]);
    }
    if (this.thu == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 4)[0]);
    }
    if (this.fri == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 5)[0]);
    }
    if (this.sat == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 6)[0]);
    }
    if (this.sun == 1) {
      this.selectedItems.push(this.dropdownList.filter(e => e.item_id === 7)[0]);
    }
    if(this.timesArray.length!=0)
    {
    this.selectedstItems.push(this.hoursList.filter(h=>h.Hour_Id===parseInt(this.timesArray[0].Hour_Id))[0]); 
    }
    this.myform.patchValue({
      facilityname: this.selectedfItems,
      nursestationName: this.selectednItems,
      //floorName: res.Floor_Id,
      //wingName: res.Wing_Id,
      frequency: this.selectedfrequencyItems,
      time:this.selectedstItems,
      //time: this.timesArray.length==0?'': this.timesArray[0].Hour_Id,
      //timeFormat: res.TimeFormat_ID,
      hours: res.Hours == null ? '' : res.Hours,
      weeksdata: this.selectedItems,
      week: res.Week_Id,
      month: res.Month_Id,
      //oDay: res.OnlyOnDay,
      //through: res.ThroughDay,
      aDay: res.ActiveDays,
      hDay: res.HoldDays,
    });
    this.nursingFreqId = res.NursingFreq_Id;
    this.oldFrequencyId=res.Frequency_Id;
    this.oldFacilityId=res.Facility_Id;
    this.oldNursingStationId=res.NursingStation_Id;
  }

  reSet() {
    this.ng4LoadingSpinnerService.show();
    this.selectedItems = this.dropdownList;
    //this.myform.reset();
    this.myform.patchValue({
      // facilityname: '',
      // nursestationName: '',
      frequency: '',
      floorName: '',
      wingName: '',
      time: '',
      timeFormat: '',
      hours: '',
      week: '1',
      month: '13',
      oDay: '',
      through: '',
      aDay: '',
      hDay: '',
      weeksdata:this.dropdownList,
    });
    this.ng4LoadingSpinnerService.hide();
    this.mon = 0;
    this.tue = 0;
    this.wed = 0;
    this.thu = 0;
    this.fri = 0;
    this.sat = 0;
    this.sun = 0;
    this.nursingFreqId = 0;
    this.arNurseStations = [];
    this.timesArray = [];
    this.isReadOnly = false;
    this.frequencyId=0;
    this.oldFrequencyId=0;
    this.oldFacilityId=0;
    this.oldNursingStationId=0;
    //this.hoursList = [];
    //this.facilityNurseStations = [];
  }
  getFacilityNurseStationMasterData(facilityId: number) {
    if (facilityId != 0) {

      this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
        .subscribe(res => {
          this.facilityNurseStations = res;

          if (this.loginUserReceNurseStation != undefined) {
            let userReceNSList = this.loginUserReceNurseStation.split(',');
            if (userReceNSList.length > 0) {
              this.selectednItems = [];
              for (let i = 0; i < userReceNSList.length; i++) {
                let checkNsExist = this.facilityNurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
                if (checkNsExist != undefined) {
                  this.selectednItems.push(checkNsExist);
                }
              }
              this.myform.patchValue({
                nursestationName: this.selectednItems,
              });
              //this.getFiltersDataBySelection(this.userId);
              //this.getCompanyToBedByNurseStation();
              //this.getFiltersDataBySelection(this.userId);
            }
          }

          if (res.length == 0) {
            this.facilityNurseStations = [];
            this.selectednItems = [];
            this.myform.patchValue({
              nursestationName: this.selectednItems,
            })
            this.alertService.warn("Please select facility for nursing stations.");
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else {
      this.facilityNurseStations = [];
      this.selectednItems = [];
      this.alertService.warn("Please select facility for nursing stations.");
    }
  }
  getFetchData(frequencyObj: any) {
    if (frequencyObj.Facility_Id != 0) {
      this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + frequencyObj.Facility_Id)
        .subscribe(res => {
          this.facilityNurseStations = res;
          if (res.length == 0) {
            this.facilityNurseStations = [];
            this.selectednItems = [];
            this.myform.patchValue({
              nursestationName: this.selectednItems,
            })
            this.alertService.warn("Please select facility for nursing stations.");
          }
          this.fetchData(frequencyObj);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  onFacilitySelect(item: any) {
    this.selectednItems=[];
    this.myform.patchValue({ nurseStations: '' });
    this.getFacilityNurseStationMasterData(item.Facility_Id);
    this.getHoursMasterData(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.selectednItems=[];
    this.myform.patchValue({ nurseStations: '' });
    this.getFacilityNurseStationMasterData(0);
    this.getHoursMasterData(0);
    this.timesArray = [];
    this.myform.controls['time'].reset();
    this.timeform.reset();
  }

  onNurseStationSelect(item: any) {
    this.nstations = "";
  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.nursestationName = item;
  }
  onNurseStationDeSelect(item: any) {
    this.nstations = "";
  }
  onNurseStationDeSelectAll(item: any) {
    this.myform.value.nursestationName.length = 0;
  }

  addTimes() {
    if(this.timeform.value.starttime==null||this.timeform.value.starttime==undefined||this.timeform.value.starttime=='')
    {
     this.alertService.warn("Please Select Next Time");
    }
    else if(this.timesArray.length>=this.myform.value.hours)
    {
      let FrequnceLimitAlertText = this.frequencyList.find(f=>f.Frequency_Id== this.myform.value.frequency[0].Frequency_Id).Freq_Descalert;
      this.alertService.warn(FrequnceLimitAlertText);
    }
    else if(this.timeform.value.starttime!=null||this.timeform.value.starttime!=undefined||this.timeform.value.starttime!='')
    {
    var hourId = this.timeform.value.starttime[0].Hour_Id;
    let hourDesc = this.hoursList.find(h => h.Hour_Id == parseInt(hourId)).Hour_Desc;
    let result = this.timesArray.find(t => t.Hour_Id == parseInt(hourId));
    if (result != undefined) {
      this.alertService.warn("selected time already exists.")
    }
    else {
      if (this.timesArray.length >= 1 && parseInt(hourId) <= this.timesArray[0].Hour_Id) {
        this.alertService.warn("Selected time can not before start time.");
      }
      else {
        let timeObj: any = {
          Hour_Id: hourId,
          Hour_Desc: hourDesc,
        }
        this.timesArray.push(timeObj);
        this.timeform.reset();
        if (this.timesArray.length > 1) {
          this.isReadOnly = true;
        }
        else {
          this.isReadOnly = false;
        }
      }
    }
  }
  }
  removeTime(i: number) {
    if(i==0)
    {
      this.timesArray=[];
      this.timesArray.splice(i, 1);
      this.myform.controls['time'].reset();
    }
    else{
      this.timesArray.splice(i, 1);
    }
    if(this.timesArray.length>1)
    {
    this.isReadOnly=true;
    }
    else{
      this.isReadOnly=false;
  }
  }
  timemodalopen() {
    if (this.myform.value.facilityname == '' || this.myform.value.facilityname == null || this.myform.value.facilityname == undefined) {
      this.alertService.warn("Please select facility.");
    }
    else if (this.myform.value.facilityname != '' && (this.myform.value.time == '' || this.myform.value.time == null || this.myform.value.time == undefined)) {
      this.alertService.warn("Please select start Time.");
    }
    // else if (this.myform.value.hours != '' && this.myform.value.time != '') {
    //   this.alertService.warn("Hours already given. You can't give multiple times.");
    // }
    else if (((this.myform.value.hours == '' || this.myform.value.hours == null || this.myform.value.hours == undefined) && this.timesArray.length != 0) || (this.myform.value.facilityname != '' && this.myform.value.time != '')) {
      this.modalTimeIsOpen = true;
    }
  }
  addstartTime() {
    this.timesArray = [];
    if(this.selectedstItems==undefined ||this.selectedstItems==null ||this.selectedstItems.length==0)
    {
    this.isReadOnly = false;
    this.timesArray=[];
    }
    else
    {
    var starthourId = this.myform.value.time[0].Hour_Id;
    let starthourDesc = this.hoursList.find(h => h.Hour_Id == parseInt(starthourId)).Hour_Desc;
    let timeObj: any = {
      Hour_Id: starthourId,
      Hour_Desc: starthourDesc,
    }
    this.timesArray.push(timeObj);
  }
  }
  onFrequencySelect()
  {
    if(this.myform.value.frequency!=undefined || this.myform.value.frequency!=null||this.myform.value.frequency.length!=0)
    {
      this.frequencyId=this.myform.value.frequency[0].Frequency_Id;
      let freqfilter=this.frequencyList.find(f=>f.Frequency_Id==this.myform.value.frequency[0].Frequency_Id);
      if(freqfilter!=null && freqfilter!=undefined)
      {
        this.myform.patchValue({
          hours:freqfilter.Freq_Times
        });
      }
      if( freqfilter.Frequency_PRN ==1 && this.myform.value.frequency[0].Frequency_Id==28)
      {
        this.isReadOnly =true;
        this.myform.patchValue({
          //hours:'',
          time:''
        });
      }
      else
      {
        this.isReadOnly =false;
      }
    }
    else
    {
      this.frequencyId=0;
    }
  }
}