import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { chart } from 'highcharts';
import { MedRefdata } from '../../../models/assesments.model';
import * as Highcharts from 'highcharts';
import { FormGroup, FormControl } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';
import { Screens,Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
 import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import * as internal from 'assert';
import { ResidentDemographic } from '../../../models/residentdemographic.model';

@Component({
  selector: 'app-therapeuticalorder',
  templateUrl: './therapeuticalorder.component.html',
  styleUrls: ['./therapeuticalorder.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class TherapeuticalorderComponent implements OnInit {
 
  public MedRefdata: MedRefdata[];
  MedRefList: MedRefdata;
  selectedLevel;
  gridData: any[] = [];
  gridColumns: any;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  dashboardForm: FormGroup;
  public module: string;
  userId: number;
  iscollapsed = true;
  showDates = false;
  fromDate: string;
  toDate: string;
  public template;
  public nursestationid: string = "";
  public gpitext : string = "";
  backButton = false;
  public nurseStations: NurseStation[];
  public gpilist: any =[];
  dropdownSettings_ID: any = {};
  dropdownSettings_gpiID:any = {};
  public selectedItems = [];
  public selecteGPIdItems = [];
  ShowFilter = true;
  modulenamereport: any;
  searchText: string = "";
  dropdownSettings_FacID: any = {};
  dropdownSettings_Type: any = {};

  public selectedFacItems = [];
  public facilities: any[];
  public RefTpe: any[];
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public selectednItemsNew= [];
  public nstations: string = "";
  pageConfig = {}; 
  public totalRecords: any;
  public isPageLaod : number = 0;
  public residents: ResidentDemographic[];
  public selectedResItem: any[];
  public selectedResidents= [];
  public patientid: string = "";
  dropdownSettings_ResID: any = {};
  year: number;
  month: number;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService,public sharedService: SharedService, private dateFormatPipe: CustomdatePipe,private exceldownload: ExceldownloadService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("RefusedByResidentReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();
    let today = new Date();

    this.fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    this.dashboardForm = new FormGroup({
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      nursestationName: new FormControl(''),
      gpidata : new FormControl(''),
      facilityName: new FormControl(''),
      RefType: new FormControl(''),
      residentstatus: new FormControl(1),
      ddlresidents: new FormControl(''),
    });
  //  this.getNurseStations();
    //this.getFacilities();
    this.dropdownSettings_ID = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      selectAllText: "Select All",
      noDataAvailablePlaceholderText: "Please Select Facility",
      itemsShowLimit: 1,
      allowSearchFilter: true

    };
    this.dropdownSettings_gpiID = {
      singleSelection: false,
      idField: "ID",
      textField: "GPI",
      selectAllText: "Select All",
      noDataAvailablePlaceholderText: "Please Select",
      itemsShowLimit: 1,
      allowSearchFilter: true

    };
    this.dropdownSettings_ResID =
    {
      singleSelection: false,
      idField: "patientId",
      textField: "patientname",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
    this.dropdownSettings_FacID = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
     // text: "Facilities",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.dropdownSettings_Type = {
      singleSelection: true,
      idField: "ID",
      textField: "Type",
     // text: "Facilities",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.userActivity();
    this.getUserRecentFacilityNurseStations();
   
   
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.RefusedByResidentDashboard,Activity.View,'')
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
        this.getFacilities()
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getFacilities() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res:any) => {
        this.facilities = res.Facilities;

        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStations(this.loginUserReceFacility);
            }
            this.dashboardForm.patchValue({
              facilityName: this.selectedfaItems,
            });
          }
        }

        this.ng4LoadingSpinnerService.hide();
        // this.dashboardForm.patchValue({
        //   facilityName:this.facilities
        // });
        // this.getNurseStations(res.Facilities[0].Facility_Id);
        // res.forEach(item => this.selectedFacItems.push(item.Facilities[0].Facility_Id));
       
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.dashboardForm.patchValue({
      nursestationName: '',
    });
    this.loginUserReceNurseStation=undefined;
    this.getNurseStations(item.Facility_Id);
    //this.getGpiData();
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.dashboardForm.patchValue({
      nursestationName: '',
    })
    this.loginUserReceNurseStation=undefined;
    this.alertService.warn("Please select facility and nursing station to display data");
    this.ng4LoadingSpinnerService.hide();
    this.gridData=[];
    this.gridColumns=[];
    this.totalRecords=0;
  }
  getGpiData() {
    debugger;
     this.dataservice.get<any>(this.config.Emar_Facility_GpiList+this.nursestationid+"/"+this.fromDate + "/" + this.toDate+"/"+this.selectedLevel)
       .subscribe(res => {
         this.gpilist = res;
         //this.resMreason.push('1,2,3');
 
         //this.gpitext = "";
         //this.selecteddItems=['1,2,3,4,5'];
         //this.selecteddItems = [res];
           
         this.dashboardForm.patchValue({
           gpidata : res,
         });
         this.getSelectedgpiLoad();
         //this.dashboardForm.value.ddlNotestype = this.selecteddItems;
         this.ng4LoadingSpinnerService.hide();
         
       },
         error => {
           this.alertService.error(error.message);
           this.ng4LoadingSpinnerService.hide();
         });
   }
  onTypeSelect(item: any) {
   
    this.getGpiData();
  }
  onTypeDeSelect(item: any) {
    this.getGpiData();
  }
  getNursingStationTimeZone(stationId: number) {
   
    debugger;
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        let date = new Date(res);
        //this.fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
  
         this.fromDate = this.dateFormatPipe.transformISODate(date.setDate(date.getDate() - 30));
        this.toDate = this.dateFormatPipe.transformISODate(res);
        this.dashboardForm.patchValue({
          txtFromDate:this.fromDate,
          txtToDate:this.toDate,
        });
        this.getGpiData();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getResidentDropData() {
  
    this.dataservice.get<any[]>(this.config.Emar_Report_GetResidentnamesmarReport+this.nursestationid+"/"+this.dashboardForm.value.residentstatus)
      .subscribe(res => {
            
        this.residents = res;
        console.log(this.residents ,"residents")
        this.selectedResItem = []
        if(res.length != 0)
        {
  
          res.forEach(itemr => {
            this.selectedResItem.push(itemr);
       // this.selectedResItem.push(this.residents)
          });
        }
        this.dashboardForm.patchValue({
          ddlresidents: this.selectedResItem
        });
        this.selectedResidents=[];
        if(this.selectedResItem.length != 0)
        {
  
        
        this.dashboardForm.value.ddlresidents.forEach(item => {
        this.selectedResidents.push(item.patientId);
       });
      }
      //   this.dashboardForm.value.ddlresidents.forEach(item => {
      //   this.selectedResidents.push(item.patientId);
      //  });
      this.patientid = "";
      
      this.patientid = this.selectedResidents.join(',');
      // this.module = "emarhole";
    
      this.getGridData(this.module, this.fromDate,this.month,this.year, this.toDate);
      }, error => this.alertService.error(error.message));
  }
  getNurseStations(facilityId: any) {
    debugger;
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<NurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId +"/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;
        if (res != undefined && res != null && res.length > 0) {
          this.getNursingStationTimeZone(res[0].NurseStation_Id);
        }
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            this.selectednItemsNew = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
              }
            }
            this.dashboardForm.patchValue({
              nursestationName: this.selectednItems,
            });
            //this.getFiltersDataBySelection(this.userId);
                       
            this.nursestationid = "";
            //this.nursestationid = this.selectednItems;
            this.selectednItems.forEach(item => this.selectednItemsNew.push(item.NurseStation_Id));
            this.nursestationid =this.selectednItemsNew.join(',');
            this.module = "refused";
            this.getResidentDropData(); 
           // this.getGridData(this.module, this.fromDate, this.toDate);
          }
        }
          else if(this.loginUserReceNurseStation == undefined)
          {
          

        this.dashboardForm.patchValue({
          nursestationName: res
        });
        this.selectedItems=[];
        //res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        this.selectedItems.push(res[0].NurseStation_Id)
        this.nursestationid = "";
        //this.selectednItems.forEach(item => this.selectednItemsNew.push(item.NurseStation_Id));
        this.nursestationid =this.selectedItems.join(',');
        this.module = "refused";
        this.loadChart();
        }

         this.ng4LoadingSpinnerService.hide();
        // this.selectedItems=[];
        // this.ng4LoadingSpinnerService.hide();
        // this.dashboardForm.patchValue({
        //   nursestationName: this.nurseStations
        // });
        // res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        // this.nursestationid = "";
        // this.nursestationid = this.selectedItems.join(',');
        // this.module = "refused";
        // this.getGridData(this.module, this.fromDate, this.toDate);
    
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  showHideToggle(cls) { 
    if (this.iscollapsed) {
      $(cls).addClass('show');
      this.iscollapsed = false;
    }
    else {
      this.iscollapsed = true;
      $(cls).removeClass('show');
    }
  }
  close(cls) {
    this.iscollapsed = true;
    $(cls).removeClass('show');
  }
  getGridData(moduleName: string, fromDate: string,month:number,year:number,toDate: string,currentPage?:any) {
    this.ng4LoadingSpinnerService.show();
    console.log(this.dashboardForm.value.ddlresidents.length);
    console.log(this.dashboardForm.value.gpidata.length);
    currentPage = currentPage==undefined?1:currentPage;
    if(this.fromDate=="" || this.toDate=="")
    {
      this.alertService.warn("Please select proper dates");
      this.ng4LoadingSpinnerService.hide();
    }
  else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
    }
    
    else if (this.dashboardForm.value.ddlresidents.length == 0 || this.dashboardForm.value.ddlresidents == undefined || this.dashboardForm.value.ddlresidents == null) {
      this.alertService.warn("Select resident");
      this.ng4LoadingSpinnerService.hide();
    }

     else if (this.dashboardForm.value.gpidata.length == 0 || this.dashboardForm.value.gpidata == undefined || this.dashboardForm.value.gpidata == null) {
      this.alertService.warn("Select resident");
      this.ng4LoadingSpinnerService.hide();
     }
    else{
      //Srikar
      this.MedRefList = {

        dashboardName : moduleName,
        fromdate:fromDate,
        todate : toDate, 
        userId : this.userId,
        nursingstationId :this.nursestationid,
        currentPage:currentPage,
        pageSize:this.gridPagination,
        //passTime:"01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350,01200010100110,36201010100310,36201025100330,1550030000120,42500010002010,44505050100330,46600033002910,49270060006520,72500010100615,72600046000340,90050010004010,90051010102005,90150085003705,90550085104210,92100030100940,95391536000350"
        passTime :this.gpitext,
        facilityId : 0,
        dateTime : this.patientid
      }
    //this.dataservice.get<any>(this.config.Emar_Dashboard_GetCommonDashboard + moduleName + "/" + fromDate + "/" + toDate + "/" + this.userId + "/" + this.nursestationid +"/"+currentPage+"/"+ this.gridPagination+"/"+ )
    this.dataservice.post(this.config.Emar_Dashboard_GetMedRefDashboard,this.MedRefList)
     
    .subscribe(res => {
        if (res != null) {
          //let seriesData = res.YaxisData;
          //let xaxisData = res.XaxisData;
          this.gridData = res.GridData;
          this.gridColumns = res.ColumnNames;
          this.totalRecords =res.TotalRecordsCount;
          this.p=currentPage; 
          //this.yaxisTitle = "Count";              
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
  }
  getGridDataOnPageChange(currentPage:any)
  {
    this.getGridData(this.module,this.fromDate,this.month,this.year, this.toDate,currentPage);
  }
  getReport() {
    this.ng4LoadingSpinnerService.show();
    if((this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName.length==0|| this.dashboardForm.value.nursestationName==undefined ||this.dashboardForm.value.nursestationName==null))
    {
      this.alertService.warn("Please select facility and nursing station to display data");
      this.ng4LoadingSpinnerService.hide();
      this.gridData=[];
      this.gridColumns=[];
      this.totalRecords=0;
    }
    else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
    {
      this.alertService.warn("Please select proper dates");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.gpitext == "")
    {
      this.alertService.warn("Please select therapeutical classification");
      this.ng4LoadingSpinnerService.hide();
    }
    
    else
    {
      //Srikar
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      this.MedRefList = {

        dashboardName : this.patientid,
        fromdate:this.fromDate,
        todate : this.toDate,
        userId : this.userId,
        nursingstationId :this.nursestationid,
        currentPage:0,
        pageSize:0,
        passTime :this.gpitext,
        facilityId : this.dashboardForm.value.facilityName[0].Facility_Id,
        dateTime : dateTime
      }
   
      //this.dataservice.getFile(this.config.Emar_Report_GetRefusedByResidentDetailsReport + this.fromDate + "/" + 
     // this.toDate+ "/" + this.userId + "/" + this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id
     // +"/"+ dateTime+"/"+ this.gpitext)
      this.dataservice.getFile1(this.config.Emar_Report_GetRefusedByResidentDetailsReport,this.MedRefList)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "Orders by Therapeutic Classification" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  getExcel() {
    this.ng4LoadingSpinnerService.show();
    if((this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName.length==0|| this.dashboardForm.value.nursestationName==undefined ||this.dashboardForm.value.nursestationName==null))
    {
      this.alertService.warn("Please select facility and nursing station to display data");
      this.ng4LoadingSpinnerService.hide();
      this.gridData=[];
      this.gridColumns=[];
      this.totalRecords=0;
    }
    else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
    {
      this.alertService.warn("Please select proper dates");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.gpitext == "")
    {
      this.alertService.warn("Please selectgpi");
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
      this.MedRefList = {

        dashboardName : this.module,
        fromdate:this.fromDate,
        todate : this.toDate,
        userId : this.userId,
        nursingstationId :this.nursestationid,
        currentPage:0,
        pageSize:0,
        passTime :this.gpitext,
        facilityId : this.dashboardForm.value.facilityName[0].Facility_Id,
        dateTime : this.patientid
      }
      //Srikar
     // this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.module + "/" + this.fromDate + "/" + this.toDate + "/" + this.userId + "/" + this.nursestationid + "/" + this.gpitext)
     this.dataservice.post(this.config.Excel_GetRefusalExcel,this.MedRefList)
     .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        // this.excelforuseractivity = res;
        //  let test:any=["Start Date"]
        //      if(res.length!=0)
        //      res.forEach(function(x) {x.test =x.test.substring(0,10);
        //      });
        if(res.length != 0)
      {

      
        this.exceldownload.excelDownload(res, "Orders by Therapeutic Classification");
      }
      else{
        this.alertService.warn("No data available");
      }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  diff_years(dt2, dt1) {
    let dateFrom = new Date(dt1);
    let dateTo = new Date(dt2);
    var diff = dateTo.getMonth() - dateFrom.getMonth()
      + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));

    if (diff > 12) {
      //  console.log('years:', (diff - (diff %12))/12 );
    }
    return diff;
  }
  getSelectedNurseStations()
  {
    debugger;
    if (this.dashboardForm.value.nursestationName.length != 0) {
      this.selectedItems.length = 0;
      this.dashboardForm.value.nursestationName.forEach(item => this.selectedItems.push(item.NurseStation_Id));
      this.nursestationid = "";
      this.nursestationid = this.selectedItems.join(',');
     this.getGpiData();
     // this.getGridData(this.module, this.fromDate, this.toDate);
    }
    else if (this.dashboardForm.value.nursestationName.length == 0) {
      this.selectedItems.length = 0;
      this.alertService.error("Please select nursing station(s) to display data");
    }
  }
  onNurseStationSelect(item: any) {
    this.getSelectedNurseStations();
    this.getResidentDropData();
  }
  onNurseStationSelectAll(item: any) {
    this.dashboardForm.value.nursestationName = item;
    this.getSelectedNurseStations();
    this.getResidentDropData();
  }
  onNurseStationDeSelect(item: any) {
    this.getSelectedNurseStations();
    this.getResidentDropData();
  }
  onNurseStationDeSelectAll(item: any) {
    this.dashboardForm.value.nursestationName.length = 0;
    this.nursestationid = "";
    this.selectedItems = [];
    this.alertService.error("Please select nursing station(s) to display data");
    this.getResidentDropData();
  }
  loadChart() {
    
    this.fromDate = this.dashboardForm.value.txtFromDate;
    this.toDate = this.dashboardForm.value.txtToDate;
    if((this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName.length==0|| this.dashboardForm.value.nursestationName==undefined ||this.dashboardForm.value.nursestationName==null))
    {
      this.alertService.warn("Please select facility and nursing station to display data");
      this.ng4LoadingSpinnerService.hide();
      this.gridData=[];
      this.gridColumns=[];
      this.totalRecords=0;
    }
    else if(this.fromDate=="" || this.toDate=="")
    {
      this.alertService.warn("Please select proper dates");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
    }
    
    else if(this.gpitext == "")
    {
      this.alertService.warn("Please select GPI");
      this.ng4LoadingSpinnerService.hide();
    }
    
    else
    {
     
     let days =  this.calculateDiff(this.fromDate,this.toDate);
     if(days > 30)
     {
      this.alertService.warn("Please select date range between 30 days");
      this.ng4LoadingSpinnerService.hide();
     }
     else{
      this.close('.test');
      this.getGridData(this.module, this.fromDate,this.month,this.year, this.toDate);
     }
   
    }
  }

  getGpiDatainchange() {
    debugger;
     this.dataservice.get<any>(this.config.Emar_Facility_GpiList+this.nursestationid+"/"+this.fromDate + "/" + this.toDate+"/"+this.selectedLevel)
       .subscribe(res => {
         this.gpilist = res;
         //this.resMreason.push('1,2,3');
 
         this.gpitext = "";
         //this.selecteddItems=['1,2,3,4,5'];
         //this.selecteddItems = [res];
           
         this.dashboardForm.patchValue({
           gpidata : res,
         });
         this.getSelectedgpi();
        // this.getSelectedgpiLoad();
         //this.dashboardForm.value.ddlNotestype = this.selecteddItems;
         this.ng4LoadingSpinnerService.hide();
         
       },
         error => {
           this.alertService.error(error.message);
           this.ng4LoadingSpinnerService.hide();
         });
   }

  getSelectedgpiLoad()
  {
    
    if (this.dashboardForm.value.gpidata.length != 0) {
      this.selecteGPIdItems.length = 0;
      this.dashboardForm.value.gpidata.forEach(item => this.selecteGPIdItems.push(item.ID));
      this.gpitext = "";
      this.gpitext = this.selecteGPIdItems.join(',');

    this.loadChart();
    }
    else if (this.dashboardForm.value.gpidata.length == 0) {
      this.selecteGPIdItems.length = 0;
     // this.alertService.error("Please select gpi to display data");
    }
  }
  getSelectedgpi()
  {
    
    if (this.dashboardForm.value.gpidata.length != 0) {
      this.selecteGPIdItems.length = 0;
      this.dashboardForm.value.gpidata.forEach(item => this.selecteGPIdItems.push(item.ID));
      this.gpitext = "";
      this.gpitext = this.selecteGPIdItems.join(',');

     // this.getGridData(this.module, this.fromDate, this.toDate);
    }
    else if (this.dashboardForm.value.gpidata.length == 0) {
      this.selecteGPIdItems.length = 0;
      //this.alertService.error("Please select gpi to display data");
    }
  }
  onGpiSelect(item: any) {
    this.getSelectedgpi();
  }
  onGpiSelectAll(item: any) {
    this.dashboardForm.value.gpidata = item;
    this.getSelectedgpi();
  }
  onGpiDeSelect(item: any) {
    this.getSelectedgpi();
  }
  onGpiDeSelectAll(item: any) {
    this.dashboardForm.value.gpidata.length = 0;
    this.gpitext = "";
    this.selecteGPIdItems = [];
    this.alertService.error("Please select therapeutical classification to display data");
  }
  
  calculateDiff(fdate,tdate){
    debugger;
    let currentDate = new Date(tdate);
    fdate = new Date(fdate);

    return Math.floor((Date.UTC(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()) - Date.UTC(fdate.getFullYear(), fdate.getMonth(), fdate.getDate()) ) /(1000 * 60 * 60 * 24));
}
forChange()
{
  debugger;
  if(this.isPageLaod != 0 && this.isPageLaod >1)
  {

    this.fromDate = this.dashboardForm.value.txtFromDate;
    this.toDate = this.dashboardForm.value.txtToDate;
  
  this.ng4LoadingSpinnerService.show();
   if(this.fromDate=="" || this.toDate=="")
    {
      this.alertService.warn("Please select proper dates");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
    }

    else
    {
      this.getGpiDatainchange();
    }
      
   
     
    }
    else{
      this.isPageLaod = this.isPageLaod+1;
    }
}
RefTypeChange()
{
  this.getGpiDatainchange();
  //console.log(this.selectedLevel +"testing");
}
onResidentSelect(item: any) {
  this.getSelectedResidents();
}
onResidentSelectAll(item: any) {
  this.dashboardForm.value.ddlresidents = item;
  this.getSelectedResidents();
}
onResidentDeSelect(item: any) {
  this.getSelectedResidents();
}
onResidentDeSelectAll(item: any) {
  this.dashboardForm.value.ddlresidents.length = 0;
  this.patientid = "";
  this.selectedResItem = [];
}
getSelectedResidents() 
{ 
  if (this.dashboardForm.value.ddlresidents.length != 0) {
     this.selectedResidents=[];
     this.dashboardForm.value.ddlresidents.forEach(item => {
       this.selectedResidents.push(item.patientId);
     });
     this.patientid = "";
     this.patientid = this.selectedResidents.join(',');
   }
 }

}
