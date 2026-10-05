import { Component, OnInit } from '@angular/core';
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
  selector: 'app-ekitdr',
  templateUrl: './ekitdr.component.html',
  styleUrls: ['./ekitdr.component.css'],
  providers: [DataService, APIConfiguration,ExceldownloadService]
})
export class EkitdrComponent implements OnInit {
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
    dropdownSettings_NurseStations: any = {};
    public selectedItems = [];
    ShowFilter = true;
    modulenamereport: any;
    searchText: string = "";
    public excelforuseractivity = [];
    dropdownSettings_FacID: any = {};
    public selectedFacItems = [];
    public facilities: any[];
    public loginUserReceFacility: any;
    public loginUserReceNurseStation: any;
    public selectedfaItems = [];
    public selectednItems = [];
    public selectednItemsNew = [];
    pageConfig = {};
    public totalRecords: any;
    constructor(private dataservice: DataService, private config: APIConfiguration,
      private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,private dateFormatPipe: CustomdatePipe,private exceldownload: ExceldownloadService, private alertService: AlertService,
      private persistanceService: PersistanceService,private sharedService: SharedService) { }
  
      ngOnInit() {
        this.pageConfig = this.persistanceService.getPermissionsByScreen("E-Kit/On-siteMedicationInventoryReport");
        if (this.pageConfig != undefined) {
          if (this.pageConfig["AccessRead"] == 0) {
            this.persistanceService.redirectToHomePage();
          }
          else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        let date = new Date();
    
        this.fromDate =  this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1))).setHours(0)).toString().substring(0, 10));
        this.toDate = this.dateFormatPipe.transformISODate(new Date());
        this.dashboardForm = new FormGroup({
          txtFromDate: new FormControl(this.fromDate),
          txtToDate: new FormControl(this.toDate),
          users: new FormControl(),
          nursestationName: new FormControl(''),
          facilityName: new FormControl(''),
          ekitstatus:new FormControl("1"),
        });
        //this.getFacilities();
        this.dropdownSettings_NurseStations = {
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
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.userActivity();
        this.getUserRecentFacilityNurseStations();
      }
    }
    else
    this.persistanceService.redirectToHomePage();
    }
    getUserRecentFacilityNurseStations() {
      this.sharedService.getUserRecentFacNs(this.userId)
        .subscribe(res => {
          if (res != undefined) {
            this.loginUserReceFacility = res.Facility_Id;
            this.loginUserReceNurseStation = res.NurseStation_Id;
          }
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
          this.ng4LoadingSpinnerService.hide();
          if (this.loginUserReceFacility != null) {
            if (this.facilities.length > 0) {
              let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
              this.selectedFacItems = [];
              if (checkFacExist != undefined) {
                this.selectedFacItems.push(checkFacExist);
                this.getNurseStations(this.loginUserReceFacility);
              }
              this.dashboardForm.patchValue({
                facilityName: this.selectedFacItems,
              });
            }
          }
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
      this.loginUserReceNurseStation = undefined;
      this.getNurseStations(item.Facility_Id);
    }
    onFacilityDeSelect(item: any) {
      this.nurseStations = [];
      this.dashboardForm.patchValue({
        nursestationName: '',
      });
      this.loginUserReceNurseStation = undefined;
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
          this.fromDate =  this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1))).setHours(0)).toString().substring(0, 10));
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
          this.ng4LoadingSpinnerService.hide();
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
            
              this.getGridData();
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
            
            this.getGridData();
          }
          // this.selectedItems=[];
          // this.ng4LoadingSpinnerService.hide();
          // this.dashboardForm.patchValue({
          //   nursestationName: this.nurseStations
          // });
          // res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
          // this.nursestationid = "";
          // this.nursestationid = this.selectedItems.join(',');
          // this.module = "ekit";
          // this.getGridData(this.module, this.fromDate, this.toDate, this.userIds);
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
    getGridData(currentPage?:any) {
      debugger
      this.ng4LoadingSpinnerService.show();
      currentPage = currentPage==undefined?1:currentPage;
      let userid = parseInt(this.persistanceService.get(this.config.loggedInUserKey));
        let obj=
        {
          UserId:userid,
          NusingStationId:this.nursestationid,
          Status:1,
          currentPage:currentPage,
          pageSize:this.gridPagination
        }
      this.dataservice.post(this.config.Emar_Dashboard_GetMedicationActiveInventoryReportDashboard, obj)
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
    getGridDataOnPageChange(currentPage:any)
    {
      this.getGridData(currentPage);
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
      this.getGridData();
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
      // else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
      // {
      //   this.alertService.warn("Please select proper dates");
      //   this.ng4LoadingSpinnerService.hide();
      // }
      // else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      //   this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      //   this.ng4LoadingSpinnerService.hide();
      // }
      // else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
      // {
      //   this.alertService.warn("“FROM” date cannot be after “TO” date");
      //   this.ng4LoadingSpinnerService.hide();
      // }
      else
      {
        let userid = parseInt(this.persistanceService.get(this.config.loggedInUserKey));
        let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      this.dataservice.getFile(this.config.Emar_Report_GetEkitDetailsReport + userid +"/"+this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+1+"/"+ dateTime)
        .subscribe((res) => {
          this.ng4LoadingSpinnerService.hide();
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "ekit  " + x.getMonth() + "_" + x.getDay() + '.pdf';
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
      // else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
      // {
      //   this.alertService.warn("Please select proper dates");
      //   this.ng4LoadingSpinnerService.hide();
      // }
      // else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      //   this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      //   this.ng4LoadingSpinnerService.hide();
      // }
      // else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
      // {
      //   this.alertService.warn("“FROM” date cannot be after “TO” date");
      //   this.ng4LoadingSpinnerService.hide();
      // }
      else
      {
        debugger
        let userid = parseInt(this.persistanceService.get(this.config.loggedInUserKey));
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetEkitDetailsReportExcel + userid + "/" + this.nursestationid+"/"+1)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res.length != 0)
          {
  
          
          //res.forEach(item => (item["Date Received"] = this.dateFormatPipe.dateWithTimeFormat(item["Date Received"])) && (item["Expiration Date"] = this.dateFormatPipe.transform(item["Expiration Date"])));
          this.exceldownload.excelDownload(res, "Ekit");
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
    getSelectedNurseStations() {
      
      if (this.dashboardForm.value.nursestationName.length != 0) {
        this.selectedItems.length = 0;
        this.dashboardForm.value.nursestationName.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        this.nursestationid = "";
        this.nursestationid = this.selectedItems.join(',');
        this.getGridData();
      }
      else if (this.dashboardForm.value.nursestationName.length == 0) {
        this.selectedItems.length = 0;
        this.alertService.error("Select at least one nursing station to display data");
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
      this.alertService.error("Select at least one nursing station to display data");
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
        this.alertService.error("Select at least one nursing station to display data");
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
      this.alertService.error("Select at least one nursing station to display data");
    }
    userActivity()
    {    
      this.sharedService.insertUserActivityDetails(Screens.EkitDashboard,Activity.View,'')
      .subscribe(res=>{},error=>{
        this.alertService.error(error.message);
      });
    }
  }