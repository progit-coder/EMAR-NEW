import { Component, OnInit, ViewChild, ElementRef, Input, SimpleChange } from '@angular/core';
import * as Highcharts from 'highcharts';
import { chart } from 'highcharts';

@Component({
  selector: 'app-charttemplate',
  templateUrl: './charttemplate.component.html',
  styleUrls: ['./charttemplate.component.css']
})
export class CharttemplateComponent implements OnInit {

  @ViewChild('chartTarget') chartTarget: ElementRef;
  chart: Highcharts.Chart;
  @Input() ChartData: any;
  chartLevel: number = 0;
  drillUpModule: string;
  drillUpXaxis: string;
  drillUpYaxis: string;
  drillUpZaxis: string;
  backButton = false;
  dashboardType: string = 'census';
  defaultXAxis: string = 'date';
  defaultYAxis: string = 'count';
  defaultZAxis: string = 'nursestation';

  constructor() { }
  ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
    if (changes.ChartData && changes.ChartData.currentValue != undefined) {
      if (this.ChartData.chartModule == "average") {
        this.createAverageCensusChart(this.ChartData.seriesData, this.ChartData.xaxisData, this.ChartData.yaxisTitle, this.ChartData.chartTitle);
      }
      else if (this.ChartData.chartModule == "compare") {
        this.createCompareChart(this.ChartData.seriesData, this.ChartData.xaxisData, this.ChartData.yaxisTitle, this.ChartData.chartTitle, this.ChartData.chartType);
      }
    }
  }

  ngOnInit() {
  }
  createCompareChart(seriesData, xaxisData, yaxisTitle, chartTitle, chartType): void {
    //this.ng4LoadingSpinnerService.show();
    const options: Highcharts.Options = {
      chart: {
        type: chartType,
        zoomType: 'x',
        inverted: false,
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
      ],

      title: {
        text: chartTitle
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
          cursor: '',
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
                text: chartTitle
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
    //this.ng4LoadingSpinnerService.hide();
  };

  createAverageCensusChart(seriesData, xaxisData, yaxisTitle, chartTitle): void {
    // this.ng4LoadingSpinnerService.show();
    const options: Highcharts.Options = {
      chart: {
        type: 'pie',
        zoomType: 'x',
        inverted: false,
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
      ],

      title: {
        text: chartTitle
      },
      //   tooltip: {
      //     pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
      // },
      plotOptions: {
        pie: {
          allowPointSelect: true,
          cursor: '',
          dataLabels: {
            enabled: true,
            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
          },
          showInLegend: true
        }
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
      series: [{
        type: 'pie',
        name: 'Average Census',
        data: seriesData
      }],
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
                text: chartTitle
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
    // this.ng4LoadingSpinnerService.hide();
  };

  // createChart(moduleName, seriesData, xaxisdata, yaxisTitle, xAxis, yAxis, zAxis,chartTitle,chartType,actualChartLevel): void {

  //   //this.ng4LoadingSpinnerService.show();
  //   const options: Highcharts.Options = {
  //     chart: {
  //       type: chartType,
  //       zoomType: 'x',
  //       inverted: false,
  //       options3d: {
  //         enabled: true,
  //         alpha: 10,
  //         beta: 25,
  //         depth: 70
  //       }
  //     },
  //     colors: ['#FFBF00',
  //       '#9966CC',
  //       '#FBCEB1',
  //       '#7FFFD4',
  //       '#CCCCFF',
  //       '#007FFF',
  //       '#89CFF0',
  //       '#0000FF',
  //       '#DE5D83',
  //       '#6F4E37',
  //       '#EDC9Af',
  //       '#50C878',
  //       '#4B0082',
  //       '#FF007F',
  //       '#FF6600',
  //       '#DA70D6',
  //       '#FFE5B4',
  //       '#FF0000',
  //       '#FA8072',
  //       '#C0C0C0',
  //       '#964B00',
  //     ], title: {
  //       text: chartTitle
  //     },
  //     // rangeSelector: {
  //     //   enabled: true,
  //     //   selected: 1
  //     // },
  //     // subtitle: {
  //     //   text: 'Resize the frame or click buttons to change appearance'
  //     // },
  //     plotOptions: {
  //       column: {
  //         depth: 25
  //       },
  //       series: {
  //         stacking: 'normal',
  //         cursor: this.chartLevel < actualChartLevel ? 'pointer' : '',
  //         // keys: ['x', 'y', 'z'],
  //         point: {
  //           events: {
  //             click: function (e) {
  //               const p = e.point;
  //               if (this.chartLevel < actualChartLevel) {

  //                 this.drillUpModule = this.chartLevel == 0 ? moduleName : moduleName + this.chartLevel;
  //                 this.drillUpXaxis = xAxis;
  //                 this.drillUpYaxis = yAxis;
  //                 this.drillUpZaxis = zAxis;
  //                 this.backButton = true;
  //                 this.chartLevel++;
  //                 if (this.chartLevel == 1 && this.filterType != 1) {
  //                   this.showPDF = false;
  //                 }
  //                 else
  //                   this.showPDF = true;

  //                 //if (xAxis == 'date' && yAxis == 'count' && zAxis == 'nursestation')
  //                 this.getChartData(moduleName + this.chartLevel, p.series.name, null, p.category, this.fromDate, this.toDate, this.filterType, this.reportType);
  //                 //else if (xAxis == 'nursestation' && yAxis == 'count' && zAxis == 'date')
  //                 // this.getChartData(moduleName + this.chartLevel, p.category, null, p.series.name,this.fromDate, this.toDate, this.filterType, this.reportType);
  //               }
  //             }.bind(this)

  //           }
  //         }

  //       },

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
  //               text: chartTitle
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
  //     },
  //     credits: {
  //       enabled: false
  //     }
  //   }
  //   this.chart = chart(this.chartTarget.nativeElement, options);
  //   //this.ng4LoadingSpinnerService.hide();
  // };
  // drillup() {
  //   if (this.chartLevel != 0) {
  //     this.chartLevel--;
  //   }
  //   if (this.chartLevel == 0)
  //     this.backButton = false;
  //   this.getChartData(this.drillUpModule, this.drillUpXaxis, this.drillUpYaxis, this.drillUpZaxis, this.fromDate, this.toDate, this.filterType, this.reportType);
  //   if (this.chartLevel == 1) {
  //     this.drillUpModule = this.dashboardType;
  //     this.drillUpXaxis = this.defaultXAxis;
  //     this.drillUpYaxis = this.defaultYAxis;
  //     this.drillUpZaxis = this.defaultZAxis;
  //   }
  // }
}
