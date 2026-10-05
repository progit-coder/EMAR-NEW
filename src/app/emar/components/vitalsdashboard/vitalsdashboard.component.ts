import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-vitalsdashboard',
  templateUrl: './vitalsdashboard.component.html',
  styleUrls: ['./vitalsdashboard.component.css']
})
export class VitalsdashboardComponent implements OnInit {
  @ViewChild('chartTarget') chartTarget: ElementRef;
  // @ViewChild('popUpChartTarget') popUpChartTarget: ElementRef;
  chart: Highcharts.Chart;
  gridData:any;
  gridColumns:any;
  dashboardType:any;
  yaxisTitle:string;
  // modalPopupChart: boolean = false;
  p: number = 1;
  gridPagination = this.config.gridPagination;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService) { }

  ngOnInit() {
    this.dashboardType ="vitals";
  }
  // getChartData(moduleName:string) {
  //   if(moduleName == "vitals")
  //   {
  //   this.dataservice.get<any>(this.config.Emar_DashBoard_GetCommonDashboard + moduleName + "/"+"" +"/"+"")
  //     .subscribe(res => {
  //       let seriesData = res.YaxisData;
  //       let xaxisData = res.XaxisData;
  //       this.gridData = res.GridData;
  //       this.gridColumns = res.ColumnNames;
  //       this.yaxisTitle = "Count";
  //       this.ng4LoadingSpinnerService.hide();
 
  //       this.createChart(seriesData, xaxisData, "",this.yaxisTitle);
  //     },
  //       error => {
  //         //this.errorMessage = <any>error.message;
  //         this.ng4LoadingSpinnerService.hide();
  //       });
  //     }
  // }
  // getPopupChartData(moduleName:string, category:string, seriesName: string) {
            
  //   this.dataservice.get<any>(this.config.Emar_DashBoard_GetCommonDashboard + moduleName+"/"+ category + "/" + seriesName)
  //     .subscribe(res => {
  //       debugger;
  //       let seriesData = res.YaxisData;
  //       let xaxisData = res.XaxisData;
  //       this.gridData = res.GridData;
  //       this.gridColumns = res.ColumnNames;
  //       this.yaxisTitle = "Count";
  //       this.ng4LoadingSpinnerService.hide();
 
  //       this.createChart(seriesData, xaxisData, "",this.yaxisTitle);
  //     },
  //       error => {
  //         //this.errorMessage = <any>error.message;
  //         this.ng4LoadingSpinnerService.hide();
  //       });
  // }
  // createChart(seriesData, xaxisdata, chartTitle,yaxisTitle): void {
  //   debugger;
  //   const options: Highcharts.Options = {
  //     chart: {
  //       type: 'column',
  //       options3d: {
  //         enabled: true,
  //         alpha: 10,
  //         beta: 25,
  //         depth: 70
  //     }
  //     },
  //     title: {
  //       text: chartTitle
  //     },
  //     rangeSelector: {
  //       enabled: true,
  //       selected: 1
  //     },
  //     // subtitle: {
  //     //   text: 'Resize the frame or click buttons to change appearance'
  //     // },
  //     plotOptions: {
  //       column: {
  //         depth: 25
  //     },
  //       series: {
  //         stacking: 'normal',
  //         cursor: 'pointer',
  //         // keys: ['x', 'y', 'z'],
  //         point: {
  //           events: {
  //             click: function (e) {
  //               const p = e.point;
  //               alert(p.category);
  //               this.getPopupChartData('vitalslevel1',p.series.name, p.category);
  //             }.bind(this)
  //           }
  //         }

  //       }
  //     },
  //     legend: {
  //       align: 'right',
  //       verticalAlign: 'middle',
  //       layout: 'vertical'
  //     },

  //     xAxis: {
  //       categories: xaxisdata,
  //       labels: {
  //         x: -10
  //       }
  //     },

  //     yAxis: {
  //       allowDecimals: false,
  //       title: {
  //         text: yaxisTitle
  //       }
  //     },
  //     series: seriesData,
  //     responsive: {
  //       rules: [{
  //         condition: {
  //           maxWidth: 500
  //         },
  //         chartOptions: {
  //           legend: {
  //             align: 'center',
  //             verticalAlign: 'bottom',
  //             layout: 'horizontal'
  //           },
  //           yAxis: {
  //             labels: {
  //               align: 'left',
  //               x: 0,
  //               y: -5
  //             },
  //             title: {
  //               text: null
  //             }
  //           },
  //           subtitle: {
  //             text: null
  //           },
  //           credits: {
  //             enabled: false
  //           }
  //         }
  //       }]
  //     }
  //   }
  //   this.chart = chart(this.chartTarget.nativeElement, options);

  // };

}
