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
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-seventytwocheckdr',
  templateUrl: './seventytwocheckdr.component.html',
  styleUrls: ['./seventytwocheckdr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class SeventytwocheckdrComponent implements OnInit {


  @ViewChild('chartTarget') chartTarget: ElementRef;
  // @ViewChild('popUpChartTarget') popUpChartTarget: ElementRef;
  chart: Highcharts.Chart;
  gridData: any[] = [];
  gridColumns: any;
  ChartTable: any;
  yaxisTitle: string;
  // modalPopupChart: boolean = false;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  dashboardForm: FormGroup;
  yData: any = [];
  zData: any = [];
  public module: string;
  chartLevel: number = 0;
  drillUpModule: string;
  drillUpXaxis: string;
  drillUpYaxis: string;
  drillUpZaxis: string;
  inverted: boolean = false;
  userId: number;
  iscollapsed = true;
  showDates = false;
  fromDate: string;
  toDate: string;
  filterType: number = 1;
  reportType: number = 1;
  public template;
  public itemfordate;
  public nursestationid: string = "";
  backButton = false;
  actualChartLevel: number = 1;
  defaultXAxis: string = 'date';
  defaultYAxis: string = 'count';
  defaultZAxis: string = 'nursestation';
  chartTitle: string = 'ActiveResidents';
  public nurseStations: NurseStation[];
  public nurse: NurseStation;
  dropdownSettings_ID: any = {};
  public selectedItems = [];
  ShowFilter = true;
  dashboardType: string = 'census';
  xData: string = "";
  z_Data: string = "";
  modulenamereport: any;
  selectedOption: string = "";
  public comparedropdownList = [];
  public dropdownSettings_Compare = {};
  searchText: string = "";
  selectedCompareItems = [];

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
  maxSelectDate: string = this.dateFormatPipe.dateFormat(new Date());
  constructor(private dataservice: DataService, private config: APIConfiguration, private exceldownload: ExceldownloadService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("72HoursFollow-upReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();
    //date.setDate(1);

    this.fromDate =this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    this.dashboardForm = new FormGroup({
      // ddlChartXAxis: new FormControl('0'),
      // ddlChartYAxis: new FormControl('0'),
      // ddlChartZAxis: new FormControl('0'),
      // ddlCalender: new FormControl('1'),
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      // ddlcategory: new FormControl('1'),
      nursestationName: new FormControl(''),
      // txtYear1: new FormControl('2017'),
      // txtYear2: new FormControl('2018'),
      chartType: new FormControl('column'),
      // compare: new FormControl(),
      facilityName:new FormControl(''),
    });
    //this.getFacilities();
    this.dropdownSettings_ID = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      noDataAvailablePlaceholderText: "Please Select Facility",
      allowSearchFilter: true

    };
    // this.comparedropdownList = [];
    // let today = new Date();
    // let year = today.getFullYear();
    // let i: number = 2010;
    // while (year >= i) {
    //   this.comparedropdownList.push({ item_id: year, item_text: year });
    //   year--;
    // }

    // this.dropdownSettings_Compare = {
    //   singleSelection: false,
    //   idField: 'item_id',
    //   textField: 'item_text',
    //   itemsShowLimit: 1,
    //   allowSearchFilter: true
    // };
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

    //this.getChartData(this.dashboardType, this.defaultXAxis, this.defaultYAxis, this.defaultZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.SeventyTwoHoursDashboard, Activity.View, '')
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
        let date=new Date(res);
        //this.maxSelectDate=this.dateFormatPipe.dateFormat(res);
        this.fromDate =this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 1,1))).setHours(0)).toString().substring(0, 10));
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
            this.module = "seventytwo";
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
        this.module = "seventytwo";
        this.getGridData(this.module, this.fromDate, this.toDate);
        }
        
        this.ng4LoadingSpinnerService.hide();
        // this.dashboardForm.patchValue({
        //   nursestationName: res
        // });
        //res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        //this.nursestationid = "";
        //this.nursestationid = this.selectedItems.join(',');
       
        //this.getGridData(this.module, this.fromDate, this.toDate);
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
    this.dataservice.get<any>(this.config.Emar_Dashboard_GetCommonDashboard + moduleName + "/" + fromDate + "/" + toDate + "/" + this.userId + "/" + this.nursestationid +"/"+currentPage+"/"+ this.gridPagination)
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
  getChartData(moduleName: string, xAxis: string, yAxis: string, zAxis: string, fromDate: string, toDate: string, filterType: number, reportType: number) {

    this.xData = xAxis;
    this.z_Data = zAxis;
    this.modulenamereport = moduleName;
    if (moduleName.startsWith("census")) {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any>(this.config.Emar_DashBoard_GetCensusDashboard + moduleName + "/" + this.userId + "/" + fromDate + "/" + toDate + "/" + filterType + "/" + reportType + "/" + xAxis + "/" + yAxis + "/" + zAxis + "/" + this.nursestationid)
        .subscribe(res => {
          if (res != null) {
            let seriesData = res.YaxisData;
            let xaxisData = res.XaxisData;
            this.gridData = seriesData;
            this.gridColumns = res.ColumnNames;
            this.yaxisTitle = "Count";
            if (moduleName != 'census') {
              if (this.reportType == 1)
                this.chartTitle = 'Active Residents (' + zAxis.replace('_', "\'") + ')';
              else if (this.reportType == 2)
                this.chartTitle = 'Discharge Residents (' + zAxis.replace('_', "'") + ')';
              else if (this.reportType == 3)
                this.chartTitle = 'Census with Allergy (' + zAxis.replace('_', ' ') + ')';
            }
            else {
              if (this.reportType == 1)
                this.chartTitle = 'Active Residents';
              else if (this.reportType == 2)
                this.chartTitle = 'Discharge Residents';
              else if (this.reportType == 3)
                this.chartTitle = 'Census with Allergy';
            }
            moduleName = 'census';
            this.createChart(moduleName, seriesData, xaxisData, this.yaxisTitle, xAxis, yAxis, zAxis);

          }
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }

  createChart(moduleName, seriesData, xaxisdata, yaxisTitle, xAxis, yAxis, zAxis): void {

    this.ng4LoadingSpinnerService.show();
    const options: Highcharts.Options = {
      chart: {
        type: this.dashboardForm.value.chartType,
        zoomType: 'x',
        inverted: this.inverted,
        options3d: {
          enabled: true,
          alpha: 10,
          beta: 25,
          depth: 70
        }
      },
      colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572',
        '#FF9655', '#FFF263', '#6AF9C4'],
      title: {
        text: this.chartTitle
      },
      // rangeSelector: {
      //   enabled: true,
      //   selected: 1
      // },
      // subtitle: {
      //   text: 'Resize the frame or click buttons to change appearance'
      // },
      plotOptions: {
        column: {
          depth: 25
        },
        series: {
          stacking: 'normal',
          cursor: this.chartLevel < this.actualChartLevel ? 'pointer' : '',
          // keys: ['x', 'y', 'z'],
          point: {
            events: {
              click: function (e) {
                const p = e.point;
                if (this.chartLevel < this.actualChartLevel) {

                  this.drillUpModule = this.chartLevel == 0 ? moduleName : moduleName + this.chartLevel;
                  this.drillUpXaxis = xAxis;
                  this.drillUpYaxis = yAxis;
                  this.drillUpZaxis = zAxis;
                  this.backButton = true;
                  this.chartLevel++;

                  //if (xAxis == 'date' && yAxis == 'count' && zAxis == 'nursestation')
                  this.getChartData(moduleName + this.chartLevel, p.series.name, null, p.category, this.fromDate, this.toDate, this.filterType, this.reportType);
                  //else if (xAxis == 'nursestation' && yAxis == 'count' && zAxis == 'date')
                  // this.getChartData(moduleName + this.chartLevel, p.category, null, p.series.name,this.fromDate, this.toDate, this.filterType, this.reportType);
                }
              }.bind(this)

            }
          }

        },

      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: xaxisdata,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: yaxisTitle
        }
      },
      series: seriesData,
      responsive: {
        rules: [{
          condition: {
            maxWidth: 500
          },
          chartOptions: {
            legend: {
              align: 'center',
              verticalAlign: 'bottom',
              layout: 'horizontal'
            },
            yAxis: {
              labels: {
                align: 'left',
                x: 0,
                y: -5
              },
              title: {
                text: this.chartTitle
              }
            },
            subtitle: {
              text: null
            },
            credits: {
              enabled: false
            }
          }
        }]
      },
      credits: {
        enabled: false
      }
    }
    this.chart = chart(this.chartTarget.nativeElement, options);
    this.ng4LoadingSpinnerService.hide();
  };
  createCompareChart(seriesData, xaxisData, yaxisTitle): void {
    this.ng4LoadingSpinnerService.show();
    const options: Highcharts.Options = {
      chart: {
        type: this.dashboardForm.value.chartType,
        zoomType: 'x',
        inverted: this.inverted,
        options3d: {
          enabled: true,
          alpha: 10,
          beta: 25,
          depth: 70
        }
      },
      colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572',
        '#FF9655', '#FFF263', '#6AF9C4'],
      title: {
        text: this.chartTitle
      },
      // rangeSelector: {
      //   enabled: true,
      //   selected: 1
      // },
      // subtitle: {
      //   text: 'Resize the frame or click buttons to change appearance'
      // },
      plotOptions: {
        column: {
          depth: 25
        },
        series: {
          //stacking: '',
          cursor: this.chartLevel < this.actualChartLevel ? 'pointer' : '',
          // keys: ['x', 'y', 'z'],
          // point: {
          //   events: {
          //     click: function (e) {
          //       const p = e.point;
          //       if (this.chartLevel < this.actualChartLevel) {

          //         this.drillUpModule = this.chartLevel == 0 ? moduleName : moduleName + this.chartLevel;
          //         this.drillUpXaxis = xAxis;
          //         this.drillUpYaxis = yAxis;
          //         this.drillUpZaxis = zAxis;
          //         this.backButton = true;
          //         this.chartLevel++;

          //         //if (xAxis == 'date' && yAxis == 'count' && zAxis == 'nursestation')
          //         this.getChartData(moduleName + this.chartLevel, p.series.name, null, p.category, this.fromDate, this.toDate, this.filterType, this.reportType);
          //         //else if (xAxis == 'nursestation' && yAxis == 'count' && zAxis == 'date')
          //         // this.getChartData(moduleName + this.chartLevel, p.category, null, p.series.name,this.fromDate, this.toDate, this.filterType, this.reportType);
          //       }
          //     }.bind(this)

          //   }
          // }

        },

      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: xaxisData,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: yaxisTitle
        }
      },
      series: seriesData,
      responsive: {
        rules: [{
          condition: {
            maxWidth: 500
          },
          chartOptions: {
            legend: {
              align: 'center',
              verticalAlign: 'bottom',
              layout: 'horizontal'
            },
            yAxis: {
              labels: {
                align: 'left',
                x: 0,
                y: -5
              },
              title: {
                text: this.chartTitle
              }
            },
            subtitle: {
              text: null
            },
            credits: {
              enabled: false
            }
          }
        }]
      },
      credits: {
        enabled: false
      }
    }
    this.chart = chart(this.chartTarget.nativeElement, options);
    this.ng4LoadingSpinnerService.hide();
  };
  drillup() {
    if (this.chartLevel != 0) {
      this.chartLevel--;
    }
    if (this.chartLevel == 0)
      this.backButton = false;
    this.getChartData(this.drillUpModule, this.drillUpXaxis, this.drillUpYaxis, this.drillUpZaxis, this.fromDate, this.toDate, this.filterType, this.reportType);
    if (this.chartLevel == 1) {
      this.drillUpModule = this.dashboardType;
      this.drillUpXaxis = this.defaultXAxis;
      this.drillUpYaxis = this.defaultYAxis;
      this.drillUpZaxis = this.defaultZAxis;
    }
  }
  loadXAxis(xaxis: string) {
    if (xaxis == 'date') {
      this.yData = [
        {
          "Id": "count",
          "Name": "Count"
        }];
      this.zData = [
        {
          "Id": "nursestation",
          "Name": "NurseStation"
        }];
    }
    else if (xaxis == 'nursestation') {
      this.yData = [
        {
          "Id": "count",
          "Name": "Count"
        }];
      this.zData = [
        {
          "Id": "date",
          "Name": "Date"
        }];
    }
    else if (xaxis == 'count') {
      this.yData = [
        {
          "Id": "date",
          "Name": "Date"
        }];
      this.zData = [
        {
          "Id": "nursestation",
          "Name": "NurseStation"
        }];
    }
    else {
      this.yData = [];
      this.zData = [];
    }
    this.dashboardForm.patchValue({
      ddlChartYAxis: '0',
      ddlChartZAxis: '0'
    });
  }
  loadYAxis(yaxis: string) {

  }
  loadZAxis(zaxis: string) {

  }
  getCompareChartData(years: string, nStationId: number) {
    //string years, int userId, int nursingstationId, int type, int reportType
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_DashBoard_GetCensusCompareDashboard + years + "/" + this.userId + "/" + nStationId + "/" + 1 + "/" + this.reportType)
      .subscribe(res => {
        if (res != null) {
          let seriesData = res.YaxisData;
          let xaxisData = res.XaxisData;
          this.gridData = seriesData;
          this.gridColumns = res.ColumnNames;
          this.yaxisTitle = "Count";
          if (this.reportType == 1)
            this.chartTitle = 'Active Residents (' + years + ')';
          else if (this.reportType == 2)
            this.chartTitle = 'Discharge Residents (' + years + ')';
          else if (this.reportType == 3)
            this.chartTitle = 'Census with Allergy (' + years + ')';
          this.createCompareChart(seriesData, xaxisData, this.yaxisTitle);
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });
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
  calenderFilters(item: number) {

    this.itemfordate = item;
    let date = new Date();
    this.toDate = new Date().toISOString().substring(0, 10);
    if (item == 1) {
      this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
      this.filterType = 1;
      this.actualChartLevel = 1;
      this.showDates = true;
      this.selectedOption = "All";
    }
    else if (item == 2) {
      this.fromDate = new Date(date.setMonth(date.getMonth() - 35)).toISOString().substring(0, 10);
      this.filterType = 2;
      this.showDates = true;
      this.actualChartLevel = 2;
      this.selectedOption = "All";
    }
    else if (item == 3) {
      this.fromDate = new Date(date.setMonth(date.getMonth() - 119)).toISOString().substring(0, 10);
      this.filterType = 3;
      this.showDates = true;
      this.actualChartLevel = 2;
      this.selectedOption = "All";
    }
    else {
      this.filterType = 1;
      this.showDates = false;
    }
    this.dashboardForm.patchValue({
      txtFromDate: this.fromDate,
      txtToDate: this.toDate
    });
    // this.chartLevel = 0;
    //this.backButton = false;
    // this.close('.test');
    // this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
  }

  getReport() {
    let userid = this.persistanceService.get(this.config.loggedInUserKey);
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
    this.dataservice.getFile(this.config.Emar_72Hours_Get72HoursReport + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "72Hours" + x.getMonth() + "_" + x.getDay() + '.pdf';
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
    this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.module + "/" + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        // this.excelforuseractivity = res;
        if (res.length != 0)
        {

        
        this.exceldownload.excelDownload(res, "seventytwo");
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
  //PRN
  // getReport() {
  //   let userid = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.getFile(this.config.Emar_PRN_GetPRNDetailsReport + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + null + "/" + 1 +"/"+1 +"/"+1)
  //     .subscribe((res) => {
  //       this.ng4LoadingSpinnerService.hide();
  //       var a = document.createElement("a");
  //       a.setAttribute('style', 'display:none;');
  //       document.body.appendChild(a);
  //       var file = new Blob([res], { type: 'application/pdf' });
  //       var url = window.URL.createObjectURL(file);
  //       a.href = url;
  //       var x: Date = new Date();
  //       var link: string = "72Hours" + x.getMonth() + "_" + x.getDay() + '.pdf';
  //       a.download = link.toLocaleLowerCase();
  //       a.click();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  //Orders
  // getReport() {
  //   
  //   let userid = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.ng4LoadingSpinnerService.show();
  //   let passtime="16:00";
  //   this.dataservice.getFile(this.config.Emar_Order_GetOrderDetailsReport + passtime + "/" + this.dashboardForm.value.txtFromDate + "/" +this.dashboardForm.value.txtToDate + "/" + 0 + "/" + 1 +"/"+ null +"/"+1 +"/"+1 +"/"+1)
  //     .subscribe((res) => {
  //       this.ng4LoadingSpinnerService.hide();
  //       var a = document.createElement("a");
  //       a.setAttribute('style', 'display:none;');
  //       document.body.appendChild(a);
  //       var file = new Blob([res], { type: 'application/pdf' });
  //       var url = window.URL.createObjectURL(file);
  //       a.href = url;
  //       var x: Date = new Date();
  //       var link: string = "Order" + x.getMonth() + "_" + x.getDay() + '.pdf';
  //       a.download = link.toLocaleLowerCase();
  //       a.click();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
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
}



