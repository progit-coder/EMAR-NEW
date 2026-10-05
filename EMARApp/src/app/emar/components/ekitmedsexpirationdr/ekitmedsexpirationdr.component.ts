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
import { Screens,Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';

@Component({
  selector: 'app-ekitmedsexpirationdr',
  templateUrl: './ekitmedsexpirationdr.component.html',
  styleUrls: ['./ekitmedsexpirationdr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class EkitmedsexpirationdrComponent implements OnInit {
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
  dropdownSettings_FacID: any = {};
  public selectedFacItems = [];
  public facilities: any[];

  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public selectednItemsNew= [];
  public nstations: string = "";
  pageConfig = {};
  public totalRecords: any;
  expirefromDate: string;
  expiretoDate: string;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,private dateFormatPipe: CustomdatePipe,
    private persistanceService: PersistanceService, public sharedService: SharedService,private exceldownload: ExceldownloadService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("E-Kit/On-siteMedicationExpirationDateReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.fromDate = new Date( new Date().setDate(new Date().getDate()-90)).toISOString().substring(0, 10);
    this.toDate = new Date().toISOString().substring(0, 10);
    this.expirefromDate=new Date(new Date().setDate(new Date().getDate()-90)).toISOString().substring(0, 10);
    this.expiretoDate=new Date( new Date().setDate(new Date().getDate()+30)).toISOString().substring(0, 10);
    this.dashboardForm = new FormGroup({
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      nursestationName: new FormControl(''),
      facilityName: new FormControl(''),
      txtExpireFromDate:new FormControl(this.expirefromDate),
      txtExpireToDate:new FormControl(this.expiretoDate),
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
    this.sharedService.insertUserActivityDetails(Screens.EKitMedsExpirationDateReport,Activity.View,'')
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
        this.fromDate =this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform(new Date( new Date(res).setDate(new Date(res).getDate()-90))).toString().substring(0, 10));
        this.toDate = this.dateFormatPipe.transformISODate(new Date(res));
        this.expirefromDate =this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform(new Date(new Date(res).setDate(new Date(res).getDate() - 90))).toString().substring(0, 10));
        this.expiretoDate =this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform(new Date(new Date(res).setDate(new Date(res).getDate() + 30))).toString().substring(0, 10));
        this.dashboardForm.patchValue({
          txtFromDate:this.fromDate,
          txtToDate:this.toDate,
          txtExpireFromDate:this.expirefromDate,
          txtExpireToDate:this.expiretoDate,
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
            this.module = "EkitMedsExpiry";
            this.getGridData();
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
        this.module = "EkitMedsExpiry";
        this.getGridData();
        }
        this.ng4LoadingSpinnerService.hide();
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
    this.ng4LoadingSpinnerService.show();
    currentPage = currentPage==undefined?1:currentPage;
  //   if(this.fromDate=="" || this.toDate=="" || this.expirefromDate=="" || this.expiretoDate=="")
  //   {
  //     this.alertService.warn("Please select proper dates");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  // else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
  //     this.alertService.warn("“TO” date cannot be prior to “FROM” date");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
  //   {
  //     this.alertService.warn("“FROM” date cannot be after “TO” date");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else if (this.expirefromDate!="" && this.expiretoDate!="" && this.expirefromDate > this.expiretoDate) {
  //     this.alertService.warn("“TO” date cannot be prior to “FROM” date");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else if(this.expirefromDate!="" && this.expiretoDate!="" && this.expiretoDate < this.expirefromDate)
  //   {
  //     this.alertService.warn("“FROM” date cannot be after “TO” date");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
    //else{
      let obj=
      {
        // CheckinFromDate:this.fromDate,
        // CheckinToDate:this.toDate,
        // ExpiryFromDate:this.expirefromDate,
        // ExpiryToDate:this.expiretoDate,
        NusingStationId:this.nursestationid,
        FacilityId:this.dashboardForm.value.facilityName[0].Facility_Id,
        UserId:this.userId,
        currentPage:currentPage,
        pageSize:this.gridPagination
      }
    this.dataservice.post(this.config.Emar_Reports_GetEkitMedsExpiryDashboard, obj)
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
  //}
  getGridDataOnPageChange(currentPage:any)
  {
    this.getGridData(currentPage);
  }
  loadChart() {
   this.fromDate = this.dashboardForm.value.txtFromDate;
   this.toDate = this.dashboardForm.value.txtToDate;
   this.expirefromDate=this.dashboardForm.value.txtExpireFromDate;
   this.expiretoDate=this.dashboardForm.value.txtExpireToDate;
   if((this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName.length==0|| this.dashboardForm.value.nursestationName==undefined ||this.dashboardForm.value.nursestationName==null))
   {
     this.alertService.warn("Please select facility and nursing station to display data");
     this.ng4LoadingSpinnerService.hide();
     this.gridData=[];
     this.gridColumns=[];
     this.totalRecords=0;
   }
   else if(this.fromDate=="" || this.toDate=="" || this.expirefromDate=="" || this.expiretoDate=="")
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
   else if (this.expirefromDate!="" && this.expiretoDate!="" && this.expirefromDate > this.expiretoDate) {
    this.alertService.warn("“TO” date cannot be prior to “FROM” date");
    this.ng4LoadingSpinnerService.hide();
  }
  else if(this.expirefromDate!="" && this.expiretoDate!="" && this.expiretoDate < this.expirefromDate)
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
  //biometeric
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
    // else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="" || this.dashboardForm.value.txtExpireFromDate=="" || this.dashboardForm.value.expiretoDate=="")
    // {
    //   this.alertService.warn("Please select proper dates");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
    //   this.alertService.warn("Check-in  date can't be before Check-in From date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    // {
    //   this.alertService.warn("“FROM” date cannot be after “TO” date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // else if (this.expirefromDate!="" && this.expiretoDate!="" && this.expirefromDate > this.expiretoDate) {
    //   this.alertService.warn("“TO” date cannot be prior to “FROM” date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // else if(this.expirefromDate!="" && this.expiretoDate!="" && this.expiretoDate < this.expirefromDate)
    // {
    //   this.alertService.warn("“FROM” date cannot be after “TO” date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else
    {
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.dataservice.getFile(this.config.Emar_Reports_GetEkitMedsExpiryReport + this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+this.userId+"/"+ dateTime)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "EkitExpirationDate" + x.getMonth() + "_" + x.getDay() + '.pdf';
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
    // else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="" || this.dashboardForm.value.txtExpireFromDate=="" || this.dashboardForm.value.txtExpireToDate=="")
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
    // else if (this.expirefromDate!="" && this.expiretoDate!="" && this.expirefromDate > this.expiretoDate) {
    //   this.alertService.warn("“TO” date cannot be prior to “FROM” date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // else if(this.expirefromDate!="" && this.expiretoDate!="" && this.expiretoDate < this.expirefromDate)
    // {
    //   this.alertService.warn("“FROM” date cannot be after “TO” date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else
    {
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetEkitMedsExpiryDateExcel + this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+this.userId)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if(res.length   != 0)
        {

        
        //res.forEach(item => (item["Most Recent Check-in Date"] = this.dateFormatPipe.transform(item["Most Recent Check-in Date"])) && (item["Most Recent Expiration Date"] = this.dateFormatPipe.transform(item["Most Recent Expiration Date"])));
        this.exceldownload.excelDownload(res, "EkitExpirationDate");
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
  getSelectedNurseStations()
  {
    debugger
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
}

