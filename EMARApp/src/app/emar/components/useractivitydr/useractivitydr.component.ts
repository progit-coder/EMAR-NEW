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
import { UserModel } from '../../../models/user.model';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
@Component({
  selector: 'app-useractivitydr',
  templateUrl: './useractivitydr.component.html',
  styleUrls: ['./useractivitydr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class UseractivitydrComponent implements OnInit {

  [x: string]: any;
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
  userIds: string = '';
  public template;
  public nursestationid: string = "";
  backButton = false;
  public nurseStations: NurseStation[];
  dropdownSettings_ID: any = {};
  public selectedItems = [];
  ShowFilter = true;
  modulenamereport: any;
  searchText: string = "";
  public usersList: UserModel[] = [];
  public excelforuseractivity = [];
  userActivityDetails:any;
  pageConfig = {};
  public totalRecords: any;
  public activitymodal:boolean=false;
  constructor(private dataservice: DataService,private dateFormatPipe: CustomdatePipe,public sharedService: SharedService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private exceldownload: ExceldownloadService,) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("UserActivityReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();
    this.fromDate = this.dateFormatPipe.transformISODate(new Date());
  //  this.fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1))).setHours(0)).toString().substring(0, 10));
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    this.dashboardForm = new FormGroup({
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      users: new FormControl()
    });
    this.getNurseStations();
    this.getUsers();
    this.dropdownSettings_ID = {
      singleSelection: false,
      idField: "User_Id",
      textField: "User_DisplayName",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      allowSearchFilter: true,
    //  limitSelection: 3

    };
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.userIds = this.userId.toString();
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getUsers() {
    this.dataservice.get<any[]>(this.config.Emar_UserMaster_GetUserDropData)
      .subscribe(res => {
        this.usersList = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStations() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<NurseStation[]>(this.config.Emar_Facility_GetUserNurseStations + userId)
      .subscribe(res => {
        this.nurseStations = res;
        this.ng4LoadingSpinnerService.hide();
        this.dashboardForm.patchValue({
          nursestationName: res
        });
        res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        this.nursestationid = "";
        this.nursestationid = this.selectedItems.join(',');
        this.module = "useractivity";
        this.getGridData(this.module, this.fromDate, this.toDate, this.userIds);

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
  getGridData(moduleName: string, fromDate: string, toDate: string, userIds: string,currentPage?:any) {
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
          
      var dt1 = new Date(this.dashboardForm.value.txtFromDate);
      var dt2 = new Date(this.dashboardForm.value.txtToDate);
      if (this.diff_years(dt2, dt1) <= 1) {
        let Obj = {
          dashboardName:moduleName,
          fromDate:fromDate ,
          toDate:toDate,
          userId:this.userId,
          nursingstationId:null,
          currentPage:currentPage,
          pageSize:this.gridPagination ,
          passTime:null,
          orderType:0,
          month:0 ,
          year:0 ,
          patientId:0 ,
          patientName:0 ,
          commentType:0 ,
          shiftTime:0 ,
          facilityid:0 ,
          userIds:this.userIds
        }
    this.dataservice.post(this.config.Emar_Dashboard_GetCommonDashboard1  , Obj)
      .subscribe(res => {
        if (res != null) {
          this.gridData = res.GridData;
          this.gridColumns = res.ColumnNames;
          this.totalRecords =res.TotalRecordsCount;
          this.p=currentPage;
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else {
        this.alertService.error("Please select dates within a one month period");
        this.ng4LoadingSpinnerService.hide();
      }
  }
}
  getGridDataOnPageChange(currentPage:any)
  {
    this.getGridData(this.module, this.fromDate, this.toDate, this.userIds,currentPage);
  }
  loadChart() {
    this.fromDate = this.dashboardForm.value.txtFromDate;
    this.toDate = this.dashboardForm.value.txtToDate;
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
    var dt1 = new Date(this.dashboardForm.value.txtFromDate);
    var dt2 = new Date(this.dashboardForm.value.txtToDate);
    if (this.diff_years(dt2, dt1) <= 1) {
      this.close('.test');
      this.getGridData(this.module, this.fromDate, this.toDate, this.userIds);
    }
    else {
      this.alertService.error("Please select dates within a one month period");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  }
  //useractivity
  getReport() {
    this.ng4LoadingSpinnerService.show();
    if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
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
      debugger;
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      var dt1 = new Date(this.dashboardForm.value.txtFromDate);
      var dt2 = new Date(this.dashboardForm.value.txtToDate);
      if (this.diff_years(dt2, dt1) <= 1) {
        let Obj = {
          userIds:this.userIds ,
          fromdate:this.fromDate ,
          todate:this.toDate ,
          userId:this.userId ,
          dateTime:dateTime
        }
        debugger;
    this.dataservice.getFile1(this.config.Report_GetUserActivityDetailsReport  , Obj)
      .subscribe((res) => {
        debugger;
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "Useractivity  " + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else {
      this.alertService.error("Please select dates within a one month period");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  }
  excelDownload(objArray, name) {

    var csvData = this.ConvertToCSV(objArray);
    var a = document.createElement("a");
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    var blob = new Blob([csvData], { type: 'text/csv' });
    var url = window.URL.createObjectURL(blob);
    a.href = url;
    var x: Date = new Date();
    var link: string = name + x.getMonth() + "_" + x.getDay() + '.csv';
    a.download = link.toLocaleLowerCase();
    a.click();

  }
  ConvertToCSV(objArray) {

    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';
    var row = "";

    for (var index in objArray[0]) {
      //Now convert each value to string and comma-separated
      row += index + ',';
    }
    row = row.slice(0, -1);
    //append Label row with line break
    str += row + '\r\n';

    for (var i = 0; i < array.length; i++) {
      var line = '';
      for (var index in array[i]) {
        if (line != '') line += ','

        line += array[i][index];
      }
      str += line + '\r\n';
    }
    return str;
  }
  getExcel() {
    
    this.ng4LoadingSpinnerService.show();
    if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
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
      var dt1 = new Date(this.dashboardForm.value.txtFromDate);
      var dt2 = new Date(this.dashboardForm.value.txtToDate);
      if (this.diff_years(dt2, dt1) <= 1) {
        let Obj = {
          userIds:this.userIds ,
          fromdate:this.fromDate ,
          todate:this.toDate ,
          userId:this.userId ,
          
        }
    this.dataservice.post(this.config.Report_GetUserActivityExcel , Obj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
       // this.excelforuseractivity = res;
       if(res.length!=0)
       {

       
       res.forEach(item => (item["LogIn Time"] = this.dateFormatPipe.dateWithTimeFormat(item["LogIn Time"])) && (item["Session Time"] = this.dateFormatPipe.dateWithTimeFormat(item["Session Time"])) && (item["LogOut Time"] = this.dateFormatPipe.dateWithTimeFormat(item["LogOut Time"])));
       this.exceldownload.excelDownload(res, "Activity");  
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
  else {
    this.alertService.error("Please select dates within a one month period");
    this.ng4LoadingSpinnerService.hide();
  }
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
      this.getGridData(this.module, this.fromDate, this.toDate, this.userIds);
    }
    else if (this.dashboardForm.value.nursestationName.length == 0) {
      this.selectedItems.length = 0;
      this.alertService.error("Please select at least one User to display data");
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

  getSelectedUsers() {
    if (this.dashboardForm.value.users.length != 0) {
      this.selectedItems.length = 0;
      this.dashboardForm.value.users.forEach(item => this.selectedItems.push(item.User_Id));
      this.userIds = "";
      this.userIds = this.selectedItems.join(',');
      //this.getGridData(this.module, this.fromDate, this.toDate, this.userIds);
    }
    else if (this.dashboardForm.value.users.length == 0) {
      this.selectedItems.length = 0;
      this.alertService.error("Please select at least one User to display data");
    }
  }
  onUsersSelect(item: any) {
    this.getSelectedUsers();
  }
  onusersSelectAll(item: any) {
    this.dashboardForm.value.users = item;
    this.getSelectedUsers();
  }
  onUsersDeSelect(item: any) {
    this.getSelectedUsers();
  }
  onUsersDeSelectAll(item: any) {
    this.dashboardForm.value.users.length = 0;
    this.userIds = "";
    this.selectedItems = [];
    this.alertService.error("Please select at least one User to display data");
  }
  rowClick(sessionId:number)
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_Dashboard_GetUserActivityDetails+sessionId)
    .subscribe(res=>{
      this.userActivityDetails=res;
      this.activitymodal=true;
      this.ng4LoadingSpinnerService.hide();
    },error=>{
      this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
    });
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.UserActivityDashboard, Activity.View, '')
      .subscribe(res => { 
      }, error => {
        this.alertService.error(error.message);
      });
  }
  closeModal()
  {
    this.activitymodal=false;
  }
}
