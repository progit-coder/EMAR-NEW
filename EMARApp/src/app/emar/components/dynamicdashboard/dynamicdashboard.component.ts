import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-dynamicdashboard',
  templateUrl: './dynamicdashboard.component.html',
  styleUrls: ['./dynamicdashboard.component.css']
})
export class DynamicdashboardComponent implements OnInit {
  @ViewChild('chartTarget') chartTarget: ElementRef;
  @ViewChild('popUpChartTarget') popUpChartTarget: ElementRef;
  chart: Highcharts.Chart;
  modalPopupChart: boolean = false;
  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService) {
    // this.dataservice.companyId = 1;
    // this.dataservice.facilityId = 1;
  }

  ngOnInit() {
    //this.createChart(this.series1, this.y, 'chartTitle');
    this.getChartData();
  }

  getChartData() {
    this.dataservice.get<any>(this.config.Emar_DashBoard_GetInboundDashboard)
      .subscribe(res => {
        let seriesData = res.yaxisdata;
        let xaxisData = res.xaxisdata;
        this.ng4LoadingSpinnerService.hide();
        let percentage = 100 / res.length;


        //var seriesData = res.map(function (val: Company) {

        //let msg="<div id="+val.Company_Id+" class='clable'>"+val.Company_Name+"<div>";
        //return ([createLable(val.Company_Id, val.Company_Name), percentage]);
        //});
        this.createChart(seriesData, xaxisData, "Inbound Activity");
      },
        error => {
          //this.errorMessage = <any>error.message;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getPopupChartData(companyId: number, createdDate: string) {
    let formData: FormData = new FormData();
                formData.append('createdDate', createdDate);
            
    this.dataservice.postFormData(this.config.Emar_DashBoard_GetInboundDashboardPopup, companyId,formData)
      .subscribe(res => {
        let seriesData = res.yaxisdata;
        let xaxisData = res.xaxisdata;
        this.ng4LoadingSpinnerService.hide();
        let percentage = 100 / res.length;
        this.createPopupChart(seriesData, xaxisData, "Inbound Activity");
      },
        error => {
          //this.errorMessage = <any>error.message;
          this.ng4LoadingSpinnerService.hide();
        });
  }

  createChart(seriesData, ydata, chartTitle): void {
    const options: Highcharts.Options = {
      chart: {
        type: 'column'
      },
      title: {
        text: chartTitle
      },
      rangeSelector: {
        enabled: true,
        selected: 1
      },
      // subtitle: {
      //   text: 'Resize the frame or click buttons to change appearance'
      // },
      plotOptions: {
        series: {
          stacking: 'normal',
          cursor: 'pointer',
          // keys: ['x', 'y', 'z'],
          point: {
            events: {
              click: function (e) {
                const p = e.point;

                //alert('Category: ' + p.category + ', value: ' + p.series.name);
                
                this.modalPopupChart = true;
                this.getPopupChartData(p.series.name, p.category);
              }.bind(this)
            }
          }

        }
      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: ydata,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: 'Count'
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
                text: null
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
      }
    }
    this.chart = chart(this.chartTarget.nativeElement, options);

  };

  createLineChart(seriesData, ydata, chartTitle): void {
    const options: Highcharts.Options = {
      title: {
        text: chartTitle
      },
      rangeSelector: {
        enabled: true,
        selected: 1
      },
      // subtitle: {
      //   text: 'Resize the frame or click buttons to change appearance'
      // },
      plotOptions: {
        series: {
          stacking: 'normal',
          cursor: 'pointer',
          point: {
            events: {
              click: function () {
                alert('Category: ' + this.category + ', value: ' + this.y);
              }
            }
          }

        }
      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: ydata,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: 'Count'
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
                text: null
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
      }
    }
    this.chart = chart(this.chartTarget.nativeElement, options);

  };

  closeModel() {
    this.modalPopupChart = false;
  }

  createPopupChart(seriesData, ydata, chartTitle): void {
    const options: Highcharts.Options = {
      chart: {
        type: 'column'
      },
      title: {
        text: chartTitle
      },
      rangeSelector: {
        enabled: true,
        selected: 1
      },
      // subtitle: {
      //   text: 'Resize the frame or click buttons to change appearance'
      // },
      plotOptions: {
        series: {
          stacking: 'normal',
          cursor: 'pointer',
          point: {
            events: {
              click: function (e) {
                const p = e.point;

                alert('Category: ' + p.category + ', value: ' + p.y);

                this.modalPopupChart = true;
              }.bind(this)
            }
          }

        }
      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: ydata,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: 'Count'
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
                text: null
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
      }
    }
    this.chart = chart(this.popUpChartTarget.nativeElement, options);

  };
  loadXAxis(xaxisId:number)
  {

  }
  loadYAxis(yaxisId:number)
  {

  }
  day(){}
  month(){}
  quarter(){}
  year(){}
}
