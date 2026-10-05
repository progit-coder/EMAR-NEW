import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts'; 
import { FormGroup, FormControl } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
import { Screens,Activity } from '../../../models/useractivity.model';
@Component({
  selector: 'app-nursenotesdr',
  templateUrl: './nursenotesdr.component.html',
  styleUrls: ['./nursenotesdr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class NursenotesdrComponent implements OnInit {
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
  public MedicationReas : string = "";
  public MedicationReasTxt : string = "";
  backButton = false;
  public nurseStations: NurseStation[];
  dropdownSettings_ID: any = {};
  public selectedItems = [];
  ShowFilter = true;
  modulenamereport: any;
  searchText: string = "";
  nurseComments: any;
  isMRED = true;
  selecteddItems=[];
 

  dropdownSettings_FacID: any = {};
  public selectedFacItems = [];
  public facilities: any[];

  dropdownSettings_Note: any = {};
  public noteComments: any[];


  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public selectednItemsNew= [];
  public selectedMItems= [];
  public selectedMTItems = [];
  
  public nstations: string = "";
  pageConfig = {};
  public totalRecords: any;

  constructor(private dataservice: DataService, private config: APIConfiguration,public sharedService: SharedService, 
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService,private exceldownload: ExceldownloadService,private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Nurses'NotesReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.getNurseComments();
    this.getNurseNoteComments();
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();

    this.fromDate = this.fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    this.dashboardForm = new FormGroup({
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      nursestationName: new FormControl(''),
      ddlcommenttype:new FormControl('1'),
      ddlNotestype:new FormControl(''),
      facilityName:new FormControl(''),
    });
    
    //this.getFacilities();
    this.dropdownSettings_ID = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      selectAllText: "Select All",
      noDataAvailablePlaceholderText: "Please Select Facility",
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
    this.dropdownSettings_Note = {
      singleSelection: false,
      idField: "MedicationReason_ID",
      textField: "MedicationReason_Desc",
     // text: "Facilities",
     noDataAvailablePlaceholderText: "Please Select Reason",
     itemsShowLimit: 1,
      
      allowSearchFilter: true
    };

    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.getUserRecentFacilityNurseStations();
    this.userActivity();
  }
  
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.NurseNotesDashboard,Activity.View,'')
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
        this.getFacilities();
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
  getNursingStationTimeZone(stationId: number) {
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        let date = new Date(res);
        this.fromDate = this.fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
        this.toDate = this.dateFormatPipe.transformISODate(res);
        this.dashboardForm.patchValue({
          txtFromDate:this.fromDate,
          txtToDate:this.toDate,
        });
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getNurseStations(facilityId: any) {
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
            this.module = "nursenote";
            this.getGridData(this.module, this.fromDate, this.toDate);
          }
        }
          else if(this.loginUserReceNurseStation == undefined)
          {
          

        this.dashboardForm.patchValue({
          nursestationName: res
        });
        this.selectedItems=[];
        res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        this.nursestationid = "";
        this.nursestationid = this.selectedItems.join(',');
        this.module = "nursenote";
        this.getGridData(this.module, this.fromDate, this.toDate);
        }
        this.ng4LoadingSpinnerService.hide();
      //   this.selectedItems=[];
      //   this.ng4LoadingSpinnerService.hide();
      //   this.dashboardForm.patchValue({
      //     nursestationName: this.nurseStations
      //   });
      //   res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
      //  this.nursestationid = "";
      //  this.nursestationid = this.selectedItems.join(',');
      //  this.module = "nursenote";
      //  this.getGridData(this.module, this.fromDate, this.toDate);

      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseComments() {
    
    this.dataservice.get<any>(this.config.Emar_DrFirstIntegration_GetNurseComments)
      .subscribe(res => {
        this.nurseComments = res;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseNoteComments() {
    
    this.dataservice.get<any>(this.config.Emar_DrFirstIntegration_GetNurseNoteComments)
      .subscribe(res => {
        this.noteComments = res;
        //this.resMreason.push('1,2,3');


        //this.selecteddItems=['1,2,3,4,5'];
        //this.selecteddItems = [res];
          
        this.dashboardForm.patchValue({
          ddlNotestype: res,
        });
        this.getSelectMedicationReason();
        //this.dashboardForm.value.ddlNotestype = this.selecteddItems;
        
        
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
  getGridData(moduleName: string, fromDate: string, toDate: string,currentPage?:any) {
    

    this.ng4LoadingSpinnerService.show();
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
    else{
    this.dataservice.get<any>(this.config.Emar_Dashboard_GetCommonDashboard + moduleName + "/" + fromDate + "/" + toDate + "/" + this.userId + "/" + this.nursestationid  +"/"+currentPage+"/"+ this.gridPagination +"/"+1+"/"+1 +"/"+0+"/"+1+"/"+1+"/"+1+"/"+this.dashboardForm.value.ddlcommenttype+"/"+1+"/"+1+"/"+1+"/"+this.MedicationReas)
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
          this.ng4LoadingSpinnerService.hide();
        });
      }
  }
  getGridDataOnPageChange(currentPage:any)
  {
    this.getGridData(this.module, this.fromDate, this.toDate,currentPage);
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
   else
   {
   this.close('.test');
   this.getGridData(this.module, this.fromDate, this.toDate);
   }
 }
  //NurseNotesDetails
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
    else
    {
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.dataservice.getFile(this.config.Emar_Report_GetNurseNotesDetailsReport + this.fromDate + "/" + this.toDate + "/" + this.userId + "/" + this.nursestationid + "/" + this.dashboardForm.value.ddlcommenttype +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime+"/"+this.MedicationReas+"/"+this.MedicationReasTxt)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "NurseNotes" + x.getMonth() + "_" + x.getDay() + '.pdf';
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
    else
    {
      this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.module + "/" + this.fromDate + "/" + this.toDate + "/" + this.userId + "/" + this.nursestationid+"/"+null+"/"+0+"/"+ this.dashboardForm.value.ddlcommenttype+"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ +"/"+0+"/"+ this.MedicationReas)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        // this.excelforuseractivity = res;
        //  let test:any=["Start Date"]
        //      if(res.length!=0)
        //      res.forEach(function(x) {x.test =x.test.substring(0,10);
        //      });
        if(res.length != 0)
        {

        
        res.forEach(item => (item["Comment On"] = this.dateFormatPipe.dateWithTimeFormat(item["Comment On"])));
        this.exceldownload.excelDownload(res, "NurseNotes");
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
    if (this.dashboardForm.value.nursestationName.length != 0) {
      this.selectedItems.length = 0;
      this.dashboardForm.value.nursestationName.forEach(item => this.selectedItems.push(item.NurseStation_Id));
      this.nursestationid = "";
      this.nursestationid = this.selectedItems.join(',');
      this.getGridData(this.module, this.fromDate, this.toDate);
    }
    else if (this.dashboardForm.value.nursestationName.length == 0) {
      this.selectedItems.length = 0;
      this.alertService.error("Please select nursing station(s) to display data");
    }
  }
  onNurseStationSelect(item: any) {
    this.getSelectedNurseStations();
  }
  onNurseStationSelectAll(item: any) {
    this.dashboardForm.value.nursestationName = item;
    this.getSelectedNurseStations();
  }
  onNurseStationDeSelect(item: any) {
    this.getSelectedNurseStations();
  }
  onNurseStationDeSelectAll(item: any) {
    this.dashboardForm.value.nursestationName.length = 0;
    this.nursestationid = "";
    this.selectedItems = [];
    this.alertService.error("Please select nursing station(s) to display data");
  }

  getSelectMedicationReason()
  {
    debugger;
    this.selectedMItems.length = 0;
    this.selectedMTItems.length = 0;
    
    this.dashboardForm.value.ddlNotestype.forEach(item => this.selectedMItems.push(item.MedicationReason_ID));
    this.dashboardForm.value.ddlNotestype.forEach(item => this.selectedMTItems.push(item.MedicationReason_Desc));

    this.MedicationReas = "";
    this.MedicationReas = this.selectedMItems.join(',');
    this.MedicationReasTxt = "";
    this.MedicationReasTxt = this.selectedMTItems.join(',');

  }
  onMedicationSelect(item: any) {
    this.getSelectMedicationReason();
  }
  onMedicationSelectAll(item: any) {
    this.dashboardForm.value.ddlNotestype = item;
    this.getSelectMedicationReason();
  }
  onMedicationDeSelect(item: any) {
    this.getSelectMedicationReason();
  }
  onMedicationDeSelectAll(item: any) {
    this.dashboardForm.value.ddlNotestype.length = 0;
    this.MedicationReas = "";
    this.MedicationReasTxt = "";
    this.selectedMItems = [];
    
  }
  onCommentChange(deviceValue) {
    if(deviceValue == "1")
    {
      this.isMRED = true;
    }
    
    else
    {
      this.isMRED = false;
      this.MedicationReas = "";
      this.MedicationReasTxt = "";
      
      
    }
    
   // console.log(deviceValue);
}

  
}
