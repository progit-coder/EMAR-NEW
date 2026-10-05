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
import { Months } from '../../../models/common.model';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-censusdashboard',
  templateUrl: './censusdashboard.component.html',
  styleUrls: ['./censusdashboard.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class CensusdashboardComponent implements OnInit {

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
  dropdownSettings_FacID: any = {};
  public selectedFacItems = [];
  public facilities: any[];
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
  total: number = 0;
  showPDF: boolean = true; 
  public tableData  = {
    "columns": [],
    "rows": [],
    "type": 0
  }
  chartData: any = null;
  public modalPopIsOpen: boolean = false;
  public allergies: any[];
  public patientName: any;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public selectednItemsNew = [];
  pageConfig = {};
  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, public sharedService: SharedService, private exceldownload: ExceldownloadService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Census");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        let date = new Date();
        //date.setDate(1);
        this.selectedOption = "average";
        this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
        this.toDate = new Date().toISOString().substring(0, 10);

        this.dropdownSettings_ID = {
          singleSelection: false,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          selectAllText: "Select All",
          itemsShowLimit: 1,
          noDataAvailablePlaceholderText: "Please Select Facility",
          allowSearchFilter: true

        };
        this.dropdownSettings_FacID = {
          singleSelection: true,
          idField: "Facility_Id",
          textField: "Facility_Name",
          // text: "Facilities",
          selectAllText: "Select All",
          itemsShowLimit: 1,
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: "No data available",
        };
        this.comparedropdownList = [];
        let today = new Date();
        let year = today.getFullYear();
        let i: number = 2010;
        while (year >= i) {
          this.comparedropdownList.push({ item_id: year, item_text: year });
          year--;
        }

        this.dropdownSettings_Compare = {
          singleSelection: false,
          idField: 'item_id',
          textField: 'item_text',
          itemsShowLimit: 1,
          allowSearchFilter: true,
          enableCheckAll: false,
          limitSelection: 3
        };
        this.dashboardForm = new FormGroup({
          ddlChartXAxis: new FormControl('0'),
          ddlChartYAxis: new FormControl('0'),
          ddlChartZAxis: new FormControl('0'),
          ddlCalender: new FormControl('1'),
          txtFromDate: new FormControl(this.fromDate),
          txtToDate: new FormControl(this.toDate),
          ddlcategory: new FormControl('4'),
          facilityName: new FormControl(''),
          nursestationName: new FormControl(''),
          // txtYear1: new FormControl('2017'),
          // txtYear2: new FormControl('2018'),
          chartType: new FormControl('column'),
          compare: new FormControl(),
          years: new FormControl(today.getFullYear()),
          months: new FormControl(today.getMonth() + 1)
        });
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.module = this.dashboardType;

        //this.getFacilities();
        this.userActivity();
        this.getUserRecentFacilityNurseStations();
        //this.getChartData(this.dashboardType, this.defaultXAxis, this.defaultYAxis, this.defaultZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Census, Activity.View, '')
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
        this.getFacilities();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  closeModel() {
    this.modalPopIsOpen = false;
  }
  getPatientAllergies(event) {
    this.modalPopIsOpen = true;
    this.patientName = event;
    this.dataservice.get<any>(this.config.Emar_Dashboard_GetPatientAllergiesByName + event)
      .subscribe(res => {
        this.allergies = res.split(',');
      },
        error => {
          this.alertService.error(error.message);
        });
  }

  loadGrid(columns: any, records: any, type: number) {
    this.tableData = {
      "columns": columns,
      "rows": records,
      "type": type
    }
    let data=this.tableData["rows"].length;
  }
  loadDynamicChart(seriesData: any, xaxisData: any, yaxisTitle: string, chartTitle: string, chartModule: string, chartType: string) {
    this.chartData = {
      "seriesData": seriesData,
      "xaxisData": xaxisData,
      "yaxisTitle": yaxisTitle,
      "chartTitle": chartTitle,
      "chartModule": chartModule,
      "chartType": chartType
    }
  }
  getFacilities() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
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
    this.chartData=null;
      this.tableData = {
        "columns": [],
        "rows": [],
        "type": 0
      }
    this.loginUserReceNurseStation = undefined;
    this.alertService.error("Please select at least one facility for nursing stations");
  }
  getNursingStationTimeZone(stationId: number) {
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        let date = new Date(res);
        this.fromDate =this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform(new Date(date.setMonth(date.getMonth() - 11))).toString().substring(0, 10));
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
    this.dataservice.get<NurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;
        if (res != undefined && res != null && res.length > 0) {
          this.getNursingStationTimeZone(res[0].NurseStation_Id);
        }
        this.selectedItems = []
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
            this.chartTitle = "Average Census (Jan-2019)"
            this.getAverageCensusChartData(this.nursestationid);
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
          this.chartTitle = "Average Census (Jan-2019)"
          this.getAverageCensusChartData(this.nursestationid);
        }
        // this.dashboardForm.patchValue({
        //   nursestationName: this.nurseStations
        // });
        // res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
        // this.nursestationid = "";
        // this.nursestationid = this.selectedItems.join(',');
        // this.chartTitle = "Average Census (Jan-2019)"
        // this.getAverageCensusChartData(this.nursestationid);
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
  // totalSum(val: number) {
  //   this.total+=val;
  //   return this.total;
  // }
  getChartData(moduleName: string, xAxis: string, yAxis: string, zAxis: string, fromDate: string, toDate: string, filterType: number, reportType: number) {
    this.chartData = null;
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

            this.yaxisTitle = "Count";
            if (moduleName != 'census') {
              if (this.reportType == 1)
                this.chartTitle = 'Active Residents (' + zAxis.replace('_', "\'") + ')';
              else if (this.reportType == 2)
                this.chartTitle = 'Discharge Residents (' + zAxis.replace('_', "'") + ')';
              else if (this.reportType == 3)
                this.chartTitle = 'Census with Allergy (' + zAxis.replace('_', ' ') + ')';
              let tableType = 3;
              if ((moduleName == 'census2' && (filterType == 2 || filterType == 3) && this.reportType == 3) || (moduleName == 'census1' && filterType == 1 && this.reportType == 3)) {
                tableType = 4;
              }

              this.loadGrid(res.ColumnNames, res.GridData, tableType);
            }
            else {
              this.loadGrid(res.ColumnNames, res.GridData, 3);
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
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  chartTypechange() {
    if (this.selectedCompareItems.length != 0) {
      this.loadCompareChart();
    }
    else if (this.reportType == 4) {
      this.getAverageCensusChartData(this.nursestationid);
    }
    else {
      this.getChartData(this.dashboardType, this.defaultXAxis, this.defaultYAxis, this.defaultZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
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
      colors: ['#FFBF00',
        '#9966CC',
        '#FBCEB1',
        '#7FFFD4',
        '#CCCCFF',
        '#007FFF',
        '#89CFF0',
        '#0000FF',
        '#DE5D83',
        '#6F4E37',
        '#EDC9Af',
        '#50C878',
        '#4B0082',
        '#FF007F',
        '#FF6600',
        '#DA70D6',
        '#FFE5B4',
        '#FF0000',
        '#FA8072',
        '#C0C0C0',
        '#964B00',
      ], title: {
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
                  if (this.chartLevel == 1 && this.filterType != 1) {
                    this.showPDF = false;
                  }
                  else
                    this.showPDF = true;

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
          this.yaxisTitle = "Count";
          if (this.reportType == 1)
            this.chartTitle = 'Active Residents (' + years + ')';
          else if (this.reportType == 2)
            this.chartTitle = 'Discharge Residents (' + years + ')';
          else if (this.reportType == 3)
            this.chartTitle = 'Census with Allergy (' + years + ')';
          this.loadDynamicChart(seriesData, xaxisData, this.yaxisTitle, this.chartTitle, 'compare', this.dashboardForm.value.chartType);
          this.loadGrid(res.ColumnNames, res.GridData, 3);

          //this.createCompareChart(seriesData, xaxisData, this.yaxisTitle);
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getAverageCensusChartData(nStation: string) {
    //string years, int userId, int nursingstationId, int type, int reportType
    this.ng4LoadingSpinnerService.show();

    this.dataservice.get<any>(this.config.Emar_Dashboard_GetAverageCensusDashboard + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months + "/" + this.userId + "/" + nStation)
      .subscribe(res => {
        if (res != null) {
          let seriesData = res.YaxisData;
          let xaxisData = res.XaxisData;
          this.yaxisTitle = "Count";
          this.chartTitle = 'Average Census (' + Months[this.dashboardForm.value.months] + '-' + this.dashboardForm.value.years + ')';
          this.loadDynamicChart(seriesData, xaxisData, this.yaxisTitle, this.chartTitle, 'average', this.dashboardForm.value.chartType);
          this.loadGrid(res.ColumnNames, res.GridData, 2);
          // if (this.reportType == 1)
          //   this.chartTitle = 'Active Residents (' + years + ')';
          // else if (this.reportType == 2)
          //   this.chartTitle = 'Discharge Residents (' + years + ')';
          // else if (this.reportType == 3)
          //   this.chartTitle = 'Census with Allergy (' + years + ')';
          //this.createAverageCensusChart(seriesData, xaxisData, this.yaxisTitle);
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });
  }
  loadChart() {
    this.showPDF = true;
    if((this.dashboardForm.value.facilityName=="" || this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName=="" || this.dashboardForm.value.nursestationName.length==0 || this.dashboardForm.value.nursestationName==undefined || this.dashboardForm.value.nursestationName==null))
    {
      this.chartData=null;
      this.tableData = {
        "columns": [],
        "rows": [],
        "type": 0
      }
      this.alertService.error("Please select at least one Nursing Station to display dashboard");
    }
    // if (this.dashboardForm.value.compare != null && this.dashboardForm.value.compare != "") {
    //   this.dashboardForm.value.compare.length = 0;
    //   this.selectedCompareItems = [];
    //   this.dashboardForm.patchValue({
    //     compare: null
    //   });
    // }
    else
    {
    this.chartLevel = 0;
    this.fromDate = this.dashboardForm.value.txtFromDate;
    this.toDate = this.dashboardForm.value.txtToDate;
    this.filterType = this.dashboardForm.value.ddlCalender;
    this.reportType = this.dashboardForm.value.ddlcategory;

    if (this.reportType == 4) {
      this.getAverageCensusChartData(this.nursestationid);
    }
    else if (this.filterType == 1) {
      this.actualChartLevel = 1;
      var dt1 = new Date(this.dashboardForm.value.txtFromDate);
      var dt2 = new Date(this.dashboardForm.value.txtToDate);
      console.log(this.diff_years(dt2, dt1));
      if (this.diff_years(dt2, dt1) <= 11) {
        this.getChartData(this.dashboardType, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
      }
      else {
        this.alertService.error("Please select dates within one year period");
      }
    }
    else if (this.filterType == 2) {
      this.actualChartLevel = 2;
      var dt1 = new Date(this.dashboardForm.value.txtFromDate);
      var dt2 = new Date(this.dashboardForm.value.txtToDate);
      if (this.diff_years(dt2, dt1) <= 35) {
        this.getChartData(this.dashboardType, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
      }
      else {
        this.alertService.error("Please select dates within three year period");
      }
    }
    else if (this.filterType == 3) {
      this.actualChartLevel = 2;
      var dt1 = new Date(this.dashboardForm.value.txtFromDate);
      var dt2 = new Date(this.dashboardForm.value.txtToDate);
      if (this.diff_years(dt2, dt1) <= 119) {
        this.getChartData(this.dashboardType, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
      }
      else {
        this.alertService.error("Please select dates within ten year period");
      }
    }
    else if (this.filterType == 4) {
      this.actualChartLevel = 0;
      this.loadCompareChart();
    }
    else {
      this.filterType = 1;
      this.selectedOption = "date";
    }
    this.backButton = false;
    this.close('.test');
    // this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);

    // if (this.dashboardForm.value.ddlChartXAxis == "count" && this.dashboardForm.value.ddlChartYAxis == "date") {
    //   this.inverted = true;
    //   this.getChartData(this.LoadDashboard, this.dashboardForm.value.ddlChartYAxis, this.dashboardForm.value.ddlChartXAxis, this.dashboardForm.value.ddlChartZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
    // }
    // else {
    //   this.inverted = false;
    //   this.getChartData(this.LoadDashboard, this.dashboardForm.value.ddlChartXAxis, this.dashboardForm.value.ddlChartYAxis, this.dashboardForm.value.ddlChartZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
    // }
  }
  }
  categoryFilters(item: number) {

    this.itemfordate = item;
    let date = new Date();
    this.dashboardForm.patchValue({
      ddlCalender: 1,
      years: date.getFullYear(),
      months: date.getMonth() + 1
    });
    if (this.dashboardForm.value.compare != null) {
      this.dashboardForm.value.compare.length = 0;
      this.selectedCompareItems = [];
    }
    this.toDate = new Date().toISOString().substring(0, 10);
    if (item == 1) {
      this.reportType = 1;
      this.selectedOption = "date";
    }
    else if (item == 2) {
      this.reportType = 2;
      this.selectedOption = "date";
    }
    else if (item == 3) {
      this.reportType = 3;
      this.selectedOption = "date";
    }
    else if (item == 4) {
      this.reportType = 4;
      this.selectedOption = "average";
    }
    else {
      this.reportType = 1;
      this.selectedOption = "date";
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
      this.selectedOption = "date";
    }
    else if (item == 2) {
      this.fromDate = new Date(date.setMonth(date.getMonth() - 35)).toISOString().substring(0, 10);
      this.filterType = 2;
      this.selectedOption = "date";
      this.actualChartLevel = 2;
    }
    else if (item == 3) {
      this.fromDate = new Date(date.setMonth(date.getMonth() - 119)).toISOString().substring(0, 10);
      this.filterType = 3;
      this.selectedOption = "date";
      this.actualChartLevel = 2;
    }
    else if (item == 4) {
      this.filterType = 4;
      this.selectedOption = "compare";

    }
    else {
      this.filterType = 1;
      this.selectedOption = "date";
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

  GetReport() {
    if((this.dashboardForm.value.facilityName=="" || this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName=="" || this.dashboardForm.value.nursestationName.length==0 || this.dashboardForm.value.nursestationName==undefined || this.dashboardForm.value.nursestationName==null))
    {
      this.alertService.error("Please select at least one Nursing Station to display dashboard");
    }
    else 
    {
    this.ng4LoadingSpinnerService.show();
    let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    if (this.selectedOption == "average") {
      //if (this.selectedCompareItems.length > 0) {
      //  let years = this.selectedCompareItems.join(",");
      //Write report code

      this.dataservice.getFile(this.config.Emar_CensusAverage_GetAverageCensusDataReport + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months + "/" + this.userId + "/" + this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime)
        .subscribe(res => {
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "Census Average_" + x.getMonth() + "_" + x.getDay() + '.pdf';
          a.download = link.toLocaleLowerCase();
          a.click();
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    // }
    else if (this.selectedOption == "compare") {
      if (this.selectedCompareItems.length > 0) {
        let years = this.selectedCompareItems.join(",");
        //Write report code
        this.dataservice.getFile(this.config.Emar_CensusCompare_GetCensusCompareReport + years + "/" + this.userId + "/" + this.nursestationid + "/" + 1 + "/" + this.reportType +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime)
          .subscribe(res => {
            var a = document.createElement("a");
            a.setAttribute('style', 'display:none;');
            document.body.appendChild(a);
            var file = new Blob([res], { type: 'application/pdf' });
            var url = window.URL.createObjectURL(file);
            a.href = url;
            var x: Date = new Date();
            var link: string = "Census Compare_" + x.getMonth() + "_" + x.getDay() + '.pdf';
            a.download = link.toLocaleLowerCase();
            a.click();
            this.ng4LoadingSpinnerService.hide();
          },
            error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();
            });
      }
    }
    else if (this.modulenamereport == "census") {
      if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
        this.dashboardForm.value.txtFromDate = null;
        this.dashboardForm.value.txtToDate = null;

      }
      if (this.nursestationid == "") {
        this.nursestationid = null;
      }
      let date = new Date();
      this.dataservice.getFile(this.config.Emar_CensusDashboard_GetCensusReport + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid + "/" + this.filterType + "/" + this.reportType + "/" + this.dashboardForm.value.ddlcategory +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime)
        .subscribe(res => {
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "Census_" + x.getMonth() + "_" + x.getDay() + '.pdf';
          a.download = link.toLocaleLowerCase();
          a.click();
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else if (this.modulenamereport == "census1") {
      if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
        this.dashboardForm.value.txtFromDate = null;
        this.dashboardForm.value.txtToDate = null;
      }

      this.dataservice.getFile(this.config.Emar_CensusDashboard_GetCensusDatesReport + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.xData + "/" + this.filterType + "/" + this.reportType + "/" + this.z_Data + "/" + this.dashboardForm.value.ddlcategory +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime)
        .subscribe(res => {
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "Census_" + x.getMonth() + "_" + x.getDay() + '.pdf';
          a.download = link.toLocaleLowerCase();
          a.click();
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else if (this.modulenamereport == "census2") {
      if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
        this.dashboardForm.value.txtFromDate = null;
        this.dashboardForm.value.txtToDate = null;
      }
      this.filterType = 1;
      this.dataservice.getFile(this.config.Emar_CensusDashboard_GetCensusDatesReport + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.xData + "/" + this.filterType + "/" + this.reportType + "/" + this.z_Data + "/" + this.dashboardForm.value.ddlcategory +"/"+this.dashboardForm.value.facilityName[0].Facility_Id+"/"+ dateTime)
        .subscribe(res => {
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "Census_" + x.getMonth() + "_" + x.getDay() + '.pdf';
          a.download = link.toLocaleLowerCase();
          a.click();
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  }
  getExcel() {
    if((this.dashboardForm.value.facilityName=="" || this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName=="" || this.dashboardForm.value.nursestationName.length==0 || this.dashboardForm.value.nursestationName==undefined || this.dashboardForm.value.nursestationName==null))
    {
      this.alertService.error("Please select at least one Nursing Station to display dashboard");
    }
    else 
    {
    if (this.selectedOption == "average") {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.selectedOption + "/" + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid + "/" + null + "/" + 0 + "/" + null + "/" + null + "/" + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
        if(res.length != 0)
        {

        
          this.exceldownload.excelDownload(res, "Average");
        }
        else{
          this.alertService.warn("No data available");
        }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else if (this.selectedOption == "compare") {
      if (this.selectedCompareItems.length > 0) {
        let years = this.selectedCompareItems.join(",");
        this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.selectedOption + "/" + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid + "/" + null + "/" + 0 + "/" + null + "/" + null + "/" + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months + "/" + 1 + "/" + this.reportType + "/" + years)
          .subscribe(res => {
            this.ng4LoadingSpinnerService.hide();
            if(res.length != 0)
            {

            
            this.exceldownload.excelDownload(res, "compare");
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
    else if (this.modulenamereport == "census") {
      if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
        this.dashboardForm.value.txtFromDate = null;
        this.dashboardForm.value.txtToDate = null;

      }
      if (this.nursestationid == "") {
        this.nursestationid = null;
      }
      let date = new Date();
      this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.modulenamereport + "/" + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid + "/" + null + "/" + 0 + "/" + null + "/" + null + "/" + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months + "/" + this.filterType + "/" + this.reportType)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if(res.length != 0)
          {

          
          this.exceldownload.excelDownload(res, "census");
          }
          else{
            this.alertService.warn("No data available");
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else if (this.modulenamereport == "census1") {
      if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
        this.dashboardForm.value.txtFromDate = null;
        this.dashboardForm.value.txtToDate = null;
      }
      this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.modulenamereport + "/" + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.xData + "/" + null + "/" + 0 + "/" + null + "/" + null + "/" + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months + "/" + this.filterType + "/" + this.reportType + "/" + null + "/" + this.z_Data)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if(res.length != 0)
          {

          
          this.exceldownload.excelDownload(res, "census");
          }
          else{
            this.alertService.warn("No data available");
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else if (this.modulenamereport == "census2") {
      if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
        this.dashboardForm.value.txtFromDate = null;
        this.dashboardForm.value.txtToDate = null;
      }
      this.filterType = 1;
      this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.modulenamereport + "/" + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.xData + "/" + null + "/" + 0 + "/" + null + "/" + null + "/" + this.dashboardForm.value.years + "/" + this.dashboardForm.value.months + "/" + this.filterType + "/" + this.reportType + "/" + null + "/" + this.z_Data)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if(res.length != 0)
          {
          this.exceldownload.excelDownload(res, "census");
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
  loadCompareChart() {
    if (this.dashboardForm.value.compare.length != 0) {
      this.selectedCompareItems.length = 0;
      this.dashboardForm.value.compare.forEach(item => this.selectedCompareItems.push(item));
    }
    if (this.selectedCompareItems.length > 3) {
      this.alertService.warn('Please select max 3 years to compare');
    }
    else if (this.selectedCompareItems.length < 2) {
      this.alertService.warn('Please select min 2 years to compare');
    }
    else if(this.dashboardForm.value.nursestationName.length>1){
      this.alertService.warn('Please select only one nursing station to compare');
    }
    else if (this.dashboardForm.value.nursestationName.length == 0) {
      this.alertService.warn('Please select only one nursing station to compare');
    }
    else {
      this.backButton = false;
      this.chartLevel = 0;
      this.actualChartLevel = 0;
      let years: string = "";
      years = this.selectedCompareItems.join(",");
      this.getCompareChartData(years, this.dashboardForm.value.nursestationName[0].NurseStation_Id);
    }
  }
  // onCompareSelect(item: any) {
  //   this.loadCompareChart();
  // }
  // onCompareDeSelect(item: any) {
  //   this.loadCompareChart();
  // }
  getSelectedNurseStations() {
    this.backButton = false;
    this.close('.test');
    if (this.dashboardForm.value.nursestationName.length != 0) {
      this.selectedItems.length = 0;
      this.dashboardForm.value.nursestationName.forEach(item => this.selectedItems.push(item.NurseStation_Id));
      this.nursestationid = "";
      this.nursestationid = this.selectedItems.join(',');
      this.backButton = false;
      this.chartLevel = 0;
      this.loadChart();
      // if (this.selectedCompareItems.length > 0) {
      //   this.loadCompareChart();
      // }
      // else {
      //   this.getChartData(this.dashboardType, this.defaultXAxis, this.defaultYAxis, this.defaultZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
      // }
    }
    else if (this.dashboardForm.value.nursestationName.length == 0) {
      this.selectedItems.length = 0;
      this.alertService.error("Please select at least one Nursing Station to display dashboard");
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
    this.backButton = false;
    this.chartLevel = 0;
    this.chartData=null;
      this.tableData = {
        "columns": [],
        "rows": [],
        "type": 0
      }
    this.alertService.error("Please select at least one Nursing Station to display dashboard");
  }
}
