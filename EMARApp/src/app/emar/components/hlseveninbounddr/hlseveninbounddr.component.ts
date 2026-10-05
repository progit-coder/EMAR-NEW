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
import { ResidentDemographic, DemographicInfo } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Screens, Activity } from '../../../models/useractivity.model';
@Component({
  selector: 'app-hlseveninbounddr',
  templateUrl: './hlseveninbounddr.component.html',
  styleUrls: ['./hlseveninbounddr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class HlseveninbounddrComponent implements OnInit {



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
  backButton = false;
  public nurseStations: NurseStation[];
  dropdownSettings_ID: any = {};
  public selectedItems = [];
  ShowFilter = true;
  modulenamereport: any;
  searchText: string = "";
  // public residents: any[];
  public residents: ResidentDemographic[];
  public demographicInfoData: DemographicInfo;
  public patientName: string;
  public patient_Id: number;
  dropdownSettings_Residents:any={};

  public selectedRItems = [];
  dropdownSettings_FacID: any = {};
  public selectedFacItems = [];
  public facilities: any[];
  maxSelectDate: string = this.dateFormatPipe.dateFormat(new Date());
  pageConfig = {};
  public totalRecords: any;
  public residentId: string;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public selectednItemsNew = [];

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private exceldownload: ExceldownloadService, private dateFormatPipe: CustomdatePipe,public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("HL7InboundReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();

    this.fromDate = this.fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    this.dashboardForm = new FormGroup({
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      ddlresidents: new FormControl(''),
      nursestationName: new FormControl(''),
      facilityName: new FormControl(''),
      fileStatus:new FormControl(''),
    });
    this.dropdownSettings_ID = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      selectAllText: "Select All",
      noDataAvailablePlaceholderText: "Please Select Facility",
      itemsShowLimit: 1,
      allowSearchFilter: true

    };
    this.dropdownSettings_Residents = {
      singleSelection: true,
      idField: "PatientName",
      textField: "PatientName",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    }
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
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    //this.getResidentDropData();
    this.patientName = "";
    this.userActivity();
    this.getUserRecentFacilityNurseStations();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.HL7InboundDashboard, Activity.View, '')
      .subscribe(res => { }, error => {
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
       
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.selectedRItems = [];
    this.dashboardForm.patchValue({
      nursestationName: '',
      ddlresidents:this.selectedRItems
    });
    this.getNurseStations(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.selectedRItems = [];
    this.residents = [];
    this.dashboardForm.patchValue({
      nursestationName: '',
      ddlresidents:this.selectedRItems
    });
    this.loginUserReceNurseStation = undefined;
    this.alertService.warn("Please select facility and nursing station to display data");
    this.ng4LoadingSpinnerService.hide();
    this.gridData = [];
    this.gridColumns = [];
    this.totalRecords = 0;
  }
  getNursingStationTimeZone(stationId: number) {
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        let date = new Date(res);
        //this.maxSelectDate=this.dateFormatPipe.dateFormat(res);
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
        this.selectedItems=[];
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
           
            this.nursestationid = "";
            this.selectednItems.forEach(item => this.selectednItemsNew.push(item.NurseStation_Id));
            this.nursestationid = this.selectednItemsNew.join(',');
            this.module = "inbound";
            this.getResidentDetailsDrop();
          }
        }
        else if (this.loginUserReceNurseStation == undefined) {
          this.dashboardForm.patchValue({
            nursestationName: res
          });
          this.selectedItems = [];
          res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
          this.nursestationid = "";
          this.nursestationid = this.selectedItems.join(',');
          this.module = "inbound";
          this.getResidentDetailsDrop();
          // this.getGridData(this.module, this.fromDate, this.toDate);
        }

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
    else if(this.patientName=="")
    {
      this.alertService.warn("Please Select atleast one resident to display data");
      this.ng4LoadingSpinnerService.hide();
    }
    else{
    this.close('.test');
    this.getGridData(this.module, this.fromDate, this.toDate);
  }
}
  onResidentDeSelect(event1:any)
  {
   this.patientName="";
   this.gridData = [];
    this.gridColumns = [];
    this.totalRecords = 0;
    this.alertService.warn("Please select atleast one resident to display data");
  }
  getResidentData(event: any) {
   this.patientName = event;
   //this.getGridData(this.module, this.fromDate, this.toDate);
  }
  getGridData(moduleName: string, fromDate: string, toDate: string,currentPage?:any) {
    this.ng4LoadingSpinnerService.show();
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
    else if(this.patientName=="")
    {
      this.alertService.warn("Please Select atleast one resident to display data");
      this.ng4LoadingSpinnerService.hide();
    }
    else{
    let status=3;
    if(this.dashboardForm.value.fileStatus!=''){
    status=this.dashboardForm.value.fileStatus;
  }
    this.patientName=this.patientName=null?null:this.patientName;
    currentPage = currentPage==undefined?1:currentPage;
    let obj={
      FromDate:fromDate,
      ToDate:toDate,
      ResidentName: this.patientName,
      Status:status,
      currentPage:currentPage,
      pageSize:this.gridPagination
    }
    this.dataservice.post(this.config.Emar_Reports_GetInboundDetailsDashboard ,obj)
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
    this.getGridData(this.module, this.fromDate, this.toDate,currentPage);
  }
  //hlseveninbound
  getReport() {
    debugger;
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
    else if(this.patientName == "")
    {
      this.alertService.warn("Please Select atleast one resident to display data");
      this.ng4LoadingSpinnerService.hide();
    }
    else{
    let pName= null;
    if(this.patientName != "")
    pName= this.patientName;
    let status=3;
    if(this.dashboardForm.value.fileStatus!='')
    status=this.dashboardForm.value.fileStatus;
    let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    let facilityId=this.dashboardForm.value.facilityName[0].Facility_Id;
    this.dataservice.getFile(this.config.Emar_Report_GetInboundDetails + this.fromDate + "/" + this.toDate + "/" + status +"/"+ this.userId+"/" +dateTime  +"/"+facilityId+"/" +pName )
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "hl7inboundmessages" + x.getMonth() + "_" + x.getDay() + '.pdf';
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
    else if(this.patientName == "")
    {
      this.alertService.warn("Please Select atleast one resident to display data");
      this.ng4LoadingSpinnerService.hide();
    }
    else{
    let pName = null;
    if(this.patientName != "")
    pName= this.patientName;
    let status=3;
    if(this.dashboardForm.value.fileStatus!='')
    status=this.dashboardForm.value.fileStatus;
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetInboundExcel + this.fromDate + "/" + this.toDate+"/"+status+"/"+ pName)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        // this.excelforuseractivity = res;
        //  let test:any=["Start Date"]
        //      if(res.length!=0)
        //      res.forEach(function(x) {x.test =x.test.substring(0,10);
        //      });
        if (res.length != 0)
        {

        
        res.forEach(item => (item["File Created Date"] = this.dateFormatPipe.dateWithTimeFormat(item["File Created Date"])))

        this.exceldownload.excelDownload(res, "hl7inboundmessages");
        }
        else
        {
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
  getSelectedNurseStations() {
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
  getResidentDetailsDrop() {
    this.ng4LoadingSpinnerService.show();
    if (this.dashboardForm.value.nursestationName.length != 0) {
    this.dataservice.get<any>(this.config.Emar_Reports_GetResidentDetails + null + "/" + this.nursestationid)
      .subscribe(res => {
        this.residents = res;
        this.selectedRItems = [];
        if (res!=undefined && res != null && res.length>0) {
        this.selectedRItems.push(this.residents[0]);
        this.dashboardForm.patchValue({
          ddlresidents: this.selectedRItems,
        });
        this.patientName=this.dashboardForm.value.ddlresidents[0].PatientName;
        this.residentId = (this.residents[0].Patient_Id).toString();
        this.module = "inbound";
        this.getGridData(this.module, this.fromDate, this.toDate);
        }
        else{
        this.ng4LoadingSpinnerService.hide();
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else if (this.dashboardForm.value.nursestationName.length == 0) {
        this.selectedRItems= [];
        this.residents=[];
        this.dashboardForm.patchValue({
          ddlresidents: this.selectedRItems,
        });
        this.gridData = [];
        this.gridColumns = [];
        this.totalRecords = 0;
        this.alertService.error("Please select nursing station(s) to display data");
        this.ng4LoadingSpinnerService.hide();
      }
  }
  onNurseStationSelect(item: any) {
    this.getSelectedNurseStations();
    this.getResidentDetailsDrop();
  }
  onNurseStationSelectAll(item: any) {
    this.dashboardForm.value.nursestationName = item;
    this.getSelectedNurseStations();
    this.getResidentDetailsDrop();
  }
  onNurseStationDeSelect(item: any) {
    this.getSelectedNurseStations();
    this.getResidentDetailsDrop();
  }
  onNurseStationDeSelectAll(item: any) {
    this.dashboardForm.value.nursestationName.length = 0;
    this.nursestationid = "";
    this.selectedItems = [];
    this.residents = [];
    this.selectedRItems=[];
    this.gridData=[];
    this.gridColumns=[];
    this.totalRecords=[];
    this.alertService.error("Please select nursing station(s) to display data");
  }
  getResidentDropData() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetResidentDropData + userId)
      .subscribe(res => {
        this.residents = res;
        this.ng4LoadingSpinnerService.hide();
        // if (res != null) {
        //   this.patientName = res[0].PatientLastName +' '+ res[0].PatientFirstName;
        //   this.dashboardForm.patchValue({
        //     ddlresidents: this.patientName
        //   });
        //   this.getNurseStations();
        // }
       //this.getFacilities();       
       this.module = "inbound";
       this.getGridData(this.module, this.fromDate, this.toDate);
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
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
}
