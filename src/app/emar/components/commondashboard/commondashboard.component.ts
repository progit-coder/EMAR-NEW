import { Component, OnInit, ViewChild, ElementRef, SimpleChange, Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { FormGroup, FormControl } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';

@Component({
  selector: 'app-commondashboard',
  templateUrl: './commondashboard.component.html',
  styleUrls: ['./commondashboard.component.css']
})
export class CommondashboardComponent implements OnInit {
//   @ViewChild('chartTarget') chartTarget: ElementRef;
//   // @ViewChild('popUpChartTarget') popUpChartTarget: ElementRef;
//   chart: Highcharts.Chart;
//   gridData: any;
//   gridColumns: any;
//   ChartTable: any;
//   yaxisTitle: string;
//   // modalPopupChart: boolean = false;
//   p: number = 1;
//   gridPagination = this.config.gridPagination;
//   title: string;
//   dashboardForm: FormGroup;
//   yData: any = [];
//   zData: any = [];
//   @Input() LoadDashboard: any;
//   public module: string;
//   chartLevel: number = 0;
//   drillUpModule: string;
//   drillUpXaxis: string;
//   drillUpYaxis: string;
//   drillUpZaxis: string;
//   inverted: boolean = false;
//   userId: number;
//   iscollapsed = true;
//   showDates = false;
//   fromDate: string;
//   toDate: string;
//   filterType: number = 1;
//   reportType: number = 1;
//   public template;
//   public itemfordate;
//   public nursestationid: string = "";
//   backButton = false;
//   actualChartLevel: number = 1;
//   defaultXAxis: string = 'date';
//   defaultYAxis: string = 'count';
//   defaultZAxis: string = 'nursestation';
//   chartTitle: string = 'ActiveResidents';
//   public nurseStations: NurseStation[];
//   dropdownSettings_ID: any = {};
//   public selectedItems = [];
//   ShowFilter = true;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService) { }

//   ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
//     this.ng4LoadingSpinnerService.show();
//     if (changes.LoadDashboard && changes.LoadDashboard.currentValue != undefined) {
//       this.userId = this.persistanceService.get(this.config.loggedInUserKey);
//       let date = new Date();
//       this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
//       this.toDate = new Date().toISOString().substring(0, 10);
//       this.getChartData(this.LoadDashboard, this.defaultXAxis, this.defaultYAxis, this.defaultZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
//       this.module = this.LoadDashboard;
//     }
//   }
  ngOnInit() {}
//     this.template = this.dataservice.template;
//     this.ng4LoadingSpinnerService.show();
//     let date = new Date();
//     //date.setDate(1);
//     this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
//     this.toDate = new Date().toISOString().substring(0, 10);
//     this.dashboardForm = new FormGroup({
//       ddlChartXAxis: new FormControl('0'),
//       ddlChartYAxis: new FormControl('0'),
//       ddlChartZAxis: new FormControl('0'),
//       ddlCalender: new FormControl('1'),
//       txtFromDate: new FormControl(this.fromDate),
//       txtToDate: new FormControl(this.toDate),
//       ddlcategory: new FormControl('1'),
//       nursestationName: new FormControl(''),
//     });
//     this.getNurseStations();
//     this.dropdownSettings_ID = {
//       singleSelection: false,
//       idField: "NurseStation_Id",
//       textField: "NurseStation_Name",
//       itemsShowLimit: 1,
//       allowSearchFilter: false
//     };
//     //  if(this.iscollapsed = false)
//     //  { 
//     // this.dashboardForm.get('fromdate').valueChanges.subscribe(date => {

//     //   this.fromdatedata(this.dashboardForm.value.fromdate);
//     // });
//     // }
//   }
//   getNurseStations() {
//     let userId = this.persistanceService.get(this.config.loggedInUserKey);
//     this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserNurseStations + userId)
//       .subscribe(res => {
//         this.nurseStations = res;
//         //this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
//       },
//         error => {
//           this.alertService.error(error.message);
//         });
//   }
//   showHideToggle(cls) {
//     if (this.iscollapsed) {
//       $(cls).addClass('show');
//       this.iscollapsed = false;
//     }
//     else {
//       this.iscollapsed = true;
//       $(cls).removeClass('show');
//     }
//   }
//   close(cls) {
//     this.iscollapsed = true;
//     $(cls).removeClass('show');
//   }
//   getChartData(moduleName: string, xAxis: string, yAxis: string, zAxis: string, fromDate: string, toDate: string, filterType: number, reportType: number) {
//     if (moduleName.startsWith("vitals")) {
//       this.ng4LoadingSpinnerService.show();
//       this.dataservice.get<any>(this.config.Emar_DashBoard_GetCommonDashboard + moduleName + "/" + this.userId + "/" + fromDate + "/" + toDate + "/" + filterType + "/" + reportType + "/" + xAxis + "/" + yAxis + "/" + zAxis)
//         .subscribe(res => {

//           let seriesData = res.YaxisData;
//           let xaxisData = res.XaxisData;
//           this.gridData = seriesData;
//           this.gridColumns = res.ColumnNames;
//           this.yaxisTitle = "Count";
//           this.title = "Vitals";
//           this.createChart(moduleName, seriesData, xaxisData, this.yaxisTitle, null, null, null);
//           this.ng4LoadingSpinnerService.hide();

//         },
//           error => {
//             this.ng4LoadingSpinnerService.hide();
//           });
//     }
//     else
//       if (moduleName.startsWith("census")) {
//         this.ng4LoadingSpinnerService.show();
//         this.dataservice.get<any>(this.config.Emar_DashBoard_GetCommonDashboard + moduleName + "/" + this.userId + "/" + fromDate + "/" + toDate + "/" + filterType + "/" + reportType + "/" + xAxis + "/" + yAxis + "/" + zAxis)
//           .subscribe(res => {
//             if (res != null) {
//               let seriesData = res.YaxisData;
//               let xaxisData = res.XaxisData;
//               this.gridData = seriesData;
//               this.gridColumns = res.ColumnNames;
//               this.yaxisTitle = "Count";
//               this.title = this.LoadDashboard;
//               moduleName = 'census';
//               this.createChart(moduleName, seriesData, xaxisData, this.yaxisTitle, xAxis, yAxis, zAxis);
//             }
//             this.ng4LoadingSpinnerService.hide();
//           },
//             error => {
//               this.ng4LoadingSpinnerService.hide();
//             });
//       }

//   }
//   createChart(moduleName, seriesData, xaxisdata, yaxisTitle, xAxis, yAxis, zAxis): void {
//     this.ng4LoadingSpinnerService.show();
//     const options: Highcharts.Options = {
//       chart: {
//         type: 'column',
//         zoomType: 'x',
//         inverted: this.inverted,
//         options3d: {
//           enabled: true,
//           alpha: 10,
//           beta: 25,
//           depth: 70
//         }
//       },
//       colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572',
//         '#FF9655', '#FFF263', '#6AF9C4'],
//       title: {
//         text: this.chartTitle
//       },
//       // rangeSelector: {
//       //   enabled: true,
//       //   selected: 1
//       // },
//       // subtitle: {
//       //   text: 'Resize the frame or click buttons to change appearance'
//       // },
//       plotOptions: {
//         column: {
//           depth: 25
//         },
//         series: {
//           stacking: 'normal',
//           cursor: this.chartLevel < this.actualChartLevel ? 'pointer' : '',
//           // keys: ['x', 'y', 'z'],
//           point: {
//             events: {
//               click: function (e) {
//                 const p = e.point;
//                 if (this.chartLevel < this.actualChartLevel) {

//                   this.drillUpModule = this.chartLevel == 0 ? moduleName : moduleName + this.chartLevel;
//                   this.drillUpXaxis = xAxis;
//                   this.drillUpYaxis = yAxis;
//                   this.drillUpZaxis = zAxis;
//                   this.backButton = true;
//                   this.chartLevel++;

//                   //if (xAxis == 'date' && yAxis == 'count' && zAxis == 'nursestation')
//                   this.getChartData(moduleName + this.chartLevel, p.series.name, null, p.category, this.fromDate, this.toDate, this.filterType, this.reportType);
//                   //else if (xAxis == 'nursestation' && yAxis == 'count' && zAxis == 'date')
//                   // this.getChartData(moduleName + this.chartLevel, p.category, null, p.series.name,this.fromDate, this.toDate, this.filterType, this.reportType);
//                 }
//               }.bind(this)

//             }
//           }

//         },

//       },
//       legend: {
//         align: 'right',
//         verticalAlign: 'middle',
//         layout: 'vertical'
//       },

//       xAxis: {
//         categories: xaxisdata,
//         labels: {
//           x: -10
//         }
//       },

//       yAxis: {
//         allowDecimals: false,
//         title: {
//           text: yaxisTitle
//         }
//       },
//       series: seriesData,
//       responsive: {
//         rules: [{
//           condition: {
//             maxWidth: 500
//           },
//           chartOptions: {
//             legend: {
//               align: 'center',
//               verticalAlign: 'bottom',
//               layout: 'horizontal'
//             },
//             yAxis: {
//               labels: {
//                 align: 'left',
//                 x: 0,
//                 y: -5
//               },
//               title: {
//                 text: null
//               }
//             },
//             subtitle: {
//               text: null
//             },
//             credits: {
//               enabled: false
//             }
//           }
//         }]
//       },
//       credits: {
//         enabled: false
//       }
//     }
//     this.chart = chart(this.chartTarget.nativeElement, options);
//     this.ng4LoadingSpinnerService.hide();
//   };
//   drillup() {
//     if (this.chartLevel != 0) {
//       this.chartLevel--;
//     }
//     if (this.chartLevel == 0)
//       this.backButton = false;
//     this.getChartData(this.drillUpModule, this.drillUpXaxis, this.drillUpYaxis, this.drillUpZaxis, this.fromDate, this.toDate, this.filterType, this.reportType);
//     if (this.chartLevel == 1) {
//       this.drillUpModule = this.LoadDashboard;
//       this.drillUpXaxis = this.defaultXAxis;
//       this.drillUpYaxis = this.defaultYAxis;
//       this.drillUpZaxis = this.defaultZAxis;
//     }
//   }
//   loadXAxis(xaxis: string) {
//     if (xaxis == 'date') {
//       this.yData = [
//         {
//           "Id": "count",
//           "Name": "Count"
//         }];
//       this.zData = [
//         {
//           "Id": "nursestation",
//           "Name": "NurseStation"
//         }];
//     }
//     else if (xaxis == 'nursestation') {
//       this.yData = [
//         {
//           "Id": "count",
//           "Name": "Count"
//         }];
//       this.zData = [
//         {
//           "Id": "date",
//           "Name": "Date"
//         }];
//     }
//     else if (xaxis == 'count') {
//       this.yData = [
//         {
//           "Id": "date",
//           "Name": "Date"
//         }];
//       this.zData = [
//         {
//           "Id": "nursestation",
//           "Name": "NurseStation"
//         }];
//     }
//     else {
//       this.yData = [];
//       this.zData = [];
//     }
//     this.dashboardForm.patchValue({
//       ddlChartYAxis: '0',
//       ddlChartZAxis: '0'
//     });
//   }
//   loadYAxis(yaxis: string) {

//   }
//   loadZAxis(zaxis: string) {

//   }
//   loadChart() {
//     this.chartLevel = 0;
//     this.fromDate = this.dashboardForm.value.txtFromDate;
//     this.toDate = this.dashboardForm.value.txtToDate;
//     this.filterType = this.dashboardForm.value.ddlCalender;
//     this.reportType = this.dashboardForm.value.ddlcategory;
//     if (this.filterType == 1) {
//       this.actualChartLevel = 1;
//       var dt1 = new Date(this.dashboardForm.value.txtFromDate);
//       var dt2 = new Date(this.dashboardForm.value.txtToDate);
//       console.log(this.diff_years(dt2, dt1));
//       if (this.diff_years(dt2, dt1) == 11) {
//         this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
//       }
//       else {
//         this.alertService.error("Please select dates with in one year period");
//       }
//     }
//     else if (this.filterType == 2) {
//       this.actualChartLevel = 2;
//       var dt1 = new Date(this.dashboardForm.value.txtFromDate);
//       var dt2 = new Date(this.dashboardForm.value.txtToDate);
//       if (this.diff_years(dt2, dt1) == 35) {
//         this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
//       }
//       else {
//         this.alertService.error("Please select dates with in three year period");
//       }
//     }
//     else if (this.filterType == 3) {
//       this.actualChartLevel = 2;
//       var dt1 = new Date(this.dashboardForm.value.txtFromDate);
//       var dt2 = new Date(this.dashboardForm.value.txtToDate);
//       if (this.diff_years(dt2, dt1) == 119) {
//         this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
//       }
//       else {
//         this.alertService.error("Please select dates with in ten year period");
//       }
//     }
//     else {
//       this.filterType = 1;
//       this.showDates = false;
//     }
//     this.backButton = false;
//     this.close('.test');
//     // this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);

//     // if (this.dashboardForm.value.ddlChartXAxis == "count" && this.dashboardForm.value.ddlChartYAxis == "date") {
//     //   this.inverted = true;
//     //   this.getChartData(this.LoadDashboard, this.dashboardForm.value.ddlChartYAxis, this.dashboardForm.value.ddlChartXAxis, this.dashboardForm.value.ddlChartZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
//     // }
//     // else {
//     //   this.inverted = false;
//     //   this.getChartData(this.LoadDashboard, this.dashboardForm.value.ddlChartXAxis, this.dashboardForm.value.ddlChartYAxis, this.dashboardForm.value.ddlChartZAxis, this.fromDate, this.toDate, this.filterType, this.reportType);
//     // }
//   }
//   calenderFilters(item: number) {

//     this.itemfordate = item;
//     let date = new Date();
//     this.toDate = new Date().toISOString().substring(0, 10);
//     if (item == 1) {
//       this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
//       this.filterType = 1;
//       this.actualChartLevel = 1;
//       this.showDates = true;
//     }
//     else if (item == 2) {
//       this.fromDate = new Date(date.setMonth(date.getMonth() - 35)).toISOString().substring(0, 10);
//       this.filterType = 2;
//       this.showDates = true;
//       this.actualChartLevel = 2;
//     }
//     else if (item == 3) {
//       this.fromDate = new Date(date.setMonth(date.getMonth() - 119)).toISOString().substring(0, 10);
//       this.filterType = 3;
//       this.showDates = true;
//       this.actualChartLevel = 2;
//     }
//     else {
//       this.filterType = 1;
//       this.showDates = false;
//     }
//     this.dashboardForm.patchValue({
//       txtFromDate: this.fromDate,
//       txtToDate: this.toDate
//     });
//     // this.chartLevel = 0;
//     //this.backButton = false;
//     // this.close('.test');
//     // this.getChartData(this.LoadDashboard, "date", "count", "nursestation", this.fromDate, this.toDate, this.filterType, this.reportType);
//   }

//   GetReport() {

//     this.selectedItems.forEach(element => {
//       this.nursestationid += element.NurseStation_Id + ',';
//     });
//     this.nursestationid = this.nursestationid.substring(0, this.nursestationid.length - 1);

//     if (this.module == "vitals") {
//       this.dataservice.getFile(this.config.Emar_DashboardReport_GetDashboardReport)
//         .subscribe(res => {
//           this.ng4LoadingSpinnerService.hide();
//           var a = document.createElement("a");
//           a.setAttribute('style', 'display:none;');
//           document.body.appendChild(a);
//           var file = new Blob([res], { type: 'application/pdf' });
//           var url = window.URL.createObjectURL(file);
//           a.href = url;
//           var x: Date = new Date();
//           var link: string = "VitalReport_" + x.getMonth() + "_" + x.getDay() + '.pdf';
//           a.download = link.toLocaleLowerCase();
//           a.click();
//         },
//           error => {
//             this.alertService.error(error.message);
//             this.ng4LoadingSpinnerService.hide();
//           });
//     }
//     else
//       if (this.module == "census") {
//         if (this.dashboardForm.value.txtFromDate == "" && this.dashboardForm.value.txtToDate == "") {
//           this.dashboardForm.value.txtFromDate = null;
//           this.dashboardForm.value.txtToDate = null;
//         }
//         let date = new Date();
//         this.fromDate = new Date(date.setMonth(date.getMonth() - 12)).toISOString().substring(0, 10);
//         this.toDate = new Date().toISOString().substring(0, 10);

//         this.dataservice.getFile(this.config.Emar_CensusDashboard_GetCensusReport + this.dashboardForm.value.txtFromDate + "/" + this.dashboardForm.value.txtToDate + "/" + this.userId + "/" + this.nursestationid + "/" + this.filterType + "/" + this.reportType)
//           .subscribe(res => {
//             this.ng4LoadingSpinnerService.hide();
//             var a = document.createElement("a");
//             a.setAttribute('style', 'display:none;');
//             document.body.appendChild(a);
//             var file = new Blob([res], { type: 'application/pdf' });
//             var url = window.URL.createObjectURL(file);
//             a.href = url;
//             var x: Date = new Date();
//             var link: string = "Census_" + x.getMonth() + "_" + x.getDay() + '.pdf';
//             a.download = link.toLocaleLowerCase();
//             a.click();
//           },
//             error => {
//               this.alertService.error(error.message);
//               this.ng4LoadingSpinnerService.hide();
//             });
//       }
//   }

//   diff_years(dt2, dt1) {
//     let dateFrom = new Date(dt1);
//     let dateTo = new Date(dt2);
//     var diff = dateTo.getMonth() - dateFrom.getMonth()
//       + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));

//     if (diff > 12) {
//       //  console.log('years:', (diff - (diff %12))/12 );
//     }
//     return diff;
//   }
}
