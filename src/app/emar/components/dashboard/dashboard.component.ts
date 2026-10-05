/** 
Screen Name: DashBoard Screen
Description:
            Dashboard Screen contains Shows company wise percentage of Census of patients in Pie chart, It has drilldown with Facility, 
Nurse Station and finally Patients name.
*/
import { Component, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Company } from '../../../models/company.model';
import { Facility, Floor, NurseStation, Wing, Room, Bed } from '../../../models/facility.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
function createLable(id, name): string {
  let clable = "<div id=" + id + " class='clable'>" + name + "</div>";
  return clable;
}
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public template;
  public companyMaster: any[];
  public companyObj: Company;
  errorMessage: string;
  @ViewChild('chartTarget') chartTarget: ElementRef;
  chart: Highcharts.Chart;
  drilldownLevel = 0;
  drillUpData = [];
  facilities: any[];
  dataTableCompany: any;
  dataTableFacility: any;
  floors: any[]=[];
  stations: any[];
  wings: any[]=[];
  rooms: any[]=[];
  beds: any[]=[];
  companyId: number;
  facilityId: any;
  floorId: any;
  stationId: any;
  wingId: any;
  roomId: any;
  bedId:any;
  sessionId: number;
  censusList:any=[];
  censusType:any;
  residentsList:any=[];
  r: number = 1;
  gridPagination = this.config.gridPagination;
  isHierarchyExists:number=0;
  public dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
  constructor(private dataservice: DataService, private config: APIConfiguration, private chRef: ChangeDetectorRef,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sharedService: SharedService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe
    ) {
    // this.dataservice.companyId = 1;
    // this.dataservice.facilityId = 1;
  }
  ngAfterViewInit() {
    this.getCompanyMaster();
  }

  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserCompanyMaster)
      .subscribe(res => {
        this.companyMaster = res;
        this.floors=[];
        this.wings=[];
        this.rooms=[];
        this.beds=[];
        this.censusList=[];
        this.residentsList=[];
        this.drillUpData=[];
        this.ng4LoadingSpinnerService.hide();
        //let percentage = 100 / res.length;
        var seriesData = res.map(function (val: any) {
          debugger
        let percentage=parseInt(val.PatientCount);
          //let msg="<div id="+val.Company_Id+" class='clable'>"+val.Company_Name+"<div>";
          return ([createLable(val.Company_Id, val.Company_Name), percentage]);
        });
        this.createChart(seriesData, "Companies");
        //   $('#tableFacilities_wrapper').css('display', 'none');
        // this.chRef.detectChanges();
        // const table: any = $('#tableCompany');
        //  this.dataTableCompany = table.DataTable();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getFacilityMaster(companyId: any, chartTitle: string) {

    this.companyId = companyId;
    this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetFacilityMaster)
      .subscribe(res => {
        let facilitiesList = res;
        this.floors=[];
        this.wings=[];
        this.rooms=[];
        this.beds=[];
        this.censusList=[];
        this.facilities = facilitiesList.filter(function (val: Facility) {
          if (val.Company_Id == companyId) {
            return val;
          }
        });
        let arr = this.facilities;
        //let percentage = 100 / arr.length;
        let seriesData = arr.map(function (val: any) {

          let percentage=parseInt(val.PatientCount);
          return ([createLable(val.Facility_Id, val.Facility_Name), percentage]);
          //  return ([val.Facility_Name + ':' + val.Facility_Id, percentage]);
        });
        this.createChart(seriesData, chartTitle);
        // $('#tableCompany_wrapper').css('display', 'none');
        //  this.chRef.detectChanges();
        //const table: any = $('#tableFacilities');
        //this.dataTableFacility = table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationsForFacility(facilityId: any, chartTitle: string) {

    this.facilityId = facilityId;
    let userId=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + this.facilityId)
      .subscribe(res => {
        this.stations = res;
        this.floorId=undefined;
        this.wingId=undefined;
        this.roomId=undefined;
        this.bedId=undefined;
        this.floors=[];
        this.wings=[];
        this.rooms=[];
        this.beds=[];
        this.censusList=[];
        let arr = this.stations;
        //let percentage = 100 / arr.length;
        this.drillUpData=this.drillUpData.filter(d=>d.level==1 || d.level==2);
        let seriesData = arr.map(function (val: any) {
          let percentage=parseInt(val.PatientCount);
          return ([createLable(val.NurseStation_Id, val.NurseStation_Name), percentage]);

        });
        this.createChart(seriesData, chartTitle);
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getFloorsForNurseStation(stationId: any, chartTitle: string) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetFloorsForNurseStations + this.companyId + "/" + this.facilityId + "/" + this.stationId + "/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId))
      .subscribe(res => {
        this.floors = res;
        this.wings=[];
        this.rooms=[];
        this.beds=[];
        this.censusList=[];
        this.residentsList=[];
        this.isHierarchyExists=0;
        if(this.floors.length>0)
        {
        this.isHierarchyExists=1;
        let arr = this.floors;
        //let percentage = 100 / arr.length;
        let seriesData = arr.map(function (val: any) {
          let percentage=parseInt(val.PatientCount);
          return ([createLable(val.Floor_Id, val.Floor_Name), percentage]);
        });
        this.createChart(seriesData, chartTitle);
        }
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }

  getWingsForFloor(floorId: any, chartTitle: string) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetWingsForFloor + this.companyId + "/" + this.facilityId + "/" + this.stationId  + "/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId))
      .subscribe(res => {
        this.wings = res;
        this.floors=[];
        this.rooms=[];
        this.beds=[];
        this.censusList=[];
        this.residentsList=[];
        this.isHierarchyExists=0;
        if(this.wings.length>0)
        {
        this.isHierarchyExists=1;
        let arr = this.wings;
        //let percentage = 100 / arr.length;

        let seriesData = arr.map(function (val: any) {
          let percentage=parseInt(val.PatientCount);
          return ([createLable(val.Wing_Id, val.Wing_Desc), percentage]);

        });
        this.createChart(seriesData, chartTitle);
        }
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getRoomsForWing(wingId: any, chartTitle: string) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetRoomsForWing
      + this.companyId + "/" + this.facilityId + "/"
      + this.stationId  + "/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId))
      .subscribe(res => {
        this.rooms = res;
        this.floors=[];
        this.wings=[];
        this.beds=[];
        this.censusList=[];
        this.residentsList=[];
        this.isHierarchyExists=0;
        if(this.rooms.length>0)
        {
        this.isHierarchyExists=1;
        let arr = this.rooms;
        //let percentage = 100 / arr.length;

        let seriesData = arr.map(function (val: any) {
          let percentage=parseInt(val.PatientCount);
          return ([createLable(val.Room_Id, val.Room_Name), percentage]);

        });
        this.createChart(seriesData, chartTitle);
        }
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getBedsForRoom(roomId: any, chartTitle: string) {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetBedsForRoom + this.companyId + "/" + this.facilityId + "/" + this.stationId + "/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId))
      .subscribe(res => {
        this.beds = res;
        this.floors=[];
        this.wings=[];
        this.rooms=[];
        this.censusList=[];
        this.residentsList=[];
        this.isHierarchyExists=0;
        if(this.beds.length>0)
        {
        this.isHierarchyExists=1;
        let arr = this.beds;
        //let percentage = 100 / arr.length;

        let seriesData = arr.map(function (val: any) {
          let percentage=parseInt(val.PatientCount);
          return ([createLable(val.Bed_Id, val.Bed_Name), percentage]);

        });
        this.createChart(seriesData, chartTitle);
        }
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.companyId = this.companyId;
    this.userActivity();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Dashboard, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }

  onchartclick(pid: string, y: any) {
    let id = pid;
    if(this.drilldownLevel>2)
    {
      if(this.drilldownLevel==3)
      {
        this.stationId = id;
      }
    this.dataservice.get<any>(this.config.Emar_Facility_GetDashboardMethodCount+this.drilldownLevel+"/"+this.stationId)
      .subscribe(res => {
        debugger
        let tty=res.m_Item1;
        switch (tty) {
          case 0: this.getCompanyMaster();
            // this.chart.options.title.text="Companies";
            break;
          case 1: this.getFacilityMaster(id, "Facilities");
            //this.chart.options.title.text="Facilities";
            break;
          case 2: this.getNurseStationsForFacility(id, "Nursing Stations");
            break;
          case 3: this.getFloorsForNurseStation(id, "Floors");
            break;
          case 4: this.getWingsForFloor(id, "Wings");
            break;
          case 5: this.getRoomsForWing(id, "Rooms");
            break;
          case 6: this.getBedsForRoom(id, "Beds");
            break;
          case 7: this.getCensusByNursingStation(id, "Census");
           break;
    
          default:
            break;
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else{
        switch (this.drilldownLevel) {
          case 0: this.getCompanyMaster();
            // this.chart.options.title.text="Companies";
            break;
          case 1: this.getFacilityMaster(id, "Facilities");
            //this.chart.options.title.text="Facilities";
            break;
          case 2: this.getNurseStationsForFacility(id, "Nursing Stations");
            break;
          case 3: this.getFloorsForNurseStation(id, "Floors");
            break;
          case 4: this.getWingsForFloor(id, "Wings");
            break;
          case 5: this.getRoomsForWing(id, "Rooms");
            break;
          case 6: this.getBedsForRoom(id, "Beds");
            break;
          case 7: this.getCensusByNursingStation(id, "Census");
           break;
    
          default:
            break;
        }
      }
    
  }
  drillup() {
debugger
    let drilldown = this.drilldownLevel;
    if (drilldown > 1) {
      var data = this.drillUpData.filter(function (val: any) {
        if (val.level == drilldown - 1) {
          // this.drilldownLevel = this.drilldownLevel - 1;
          drilldown = drilldown - 1;
          return val;
        }
      });
      this.drilldownLevel = drilldown;
      this.onchartclick(data[0].name, null);
    }
    else if (drilldown === 1) {
      this.drilldownLevel = drilldown - 1;
      this.onchartclick('0', null);
    }
  }
  createChart(seriesData, chartTitle): void {
    const options: Highcharts.Options = {
      chart: {
        type: 'pie'
      },
      credits: {
        enabled: false
      },
      tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
      },
      plotOptions: {
        pie: {
          innerSize: 100,
          depth: 45,
          cursor: 'pointer',
          dataLabels: {
            enabled: true,
            format: '<b>{point.name}</b>: {point.percentage:.1f} %',

          },
          point: {
            events: {
              click: function (e) {
                debugger
                const p = e.point;
                let parser = new DOMParser();
                let parsedHtml = parser.parseFromString(p.name, 'text/html');
                let element = parsedHtml.getElementsByClassName('clable')[0];
                let id = element.id;
                let name = element.innerHTML;
                if(chartTitle=="Floors")
                this.floorId=id;
                if(chartTitle=="Wings")
                this.wingId=id;
                if(chartTitle=="Rooms")
                this.roomId=id;
                if(chartTitle=="Beds")
                this.bedId=id;
                if(chartTitle=="Census")
                this.censusType=id;
                
                if (this.drilldownLevel < 8 && this.censusList.length==0 && this.residentsList.length==0) {
                  this.drilldownLevel = this.drilldownLevel + 1;
                  const index = this.drillUpData.findIndex(i => i.level-1 == this.drilldownLevel);
                  if(index<0)
                  {
                  this.drillUpData.push({ 'level': this.drilldownLevel, 'name': id });
                  }
                  else
                  {
                  this.drillUpData[index].level=this.drilldownLevel;
                  this.drillUpData[index].name=id;
                  this.drillUpData.push();
                  }
                  this.onchartclick(id, p.y);
                }
                else if (this.drilldownLevel < 8 && this.censusList.length>0 && this.residentsList.length==0) {
                  this.drilldownLevel = this.drilldownLevel + 1;
                  const index = this.drillUpData.findIndex(i => i.level-1 == this.drilldownLevel);
                  if(index<0)
                  {
                  this.drillUpData.push({ 'level': this.drilldownLevel, 'name': id });
                  }
                  else
                  {
                    this.drillUpData[index].level=this.drilldownLevel;
                    this.drillUpData[index].name=id;
                    this.drillUpData.push();
                  }
                  this.getCensusResidentsByNursingStation(id,"Residents");
                }
              }.bind(this)
            }
          }
        }
      },
      title: {
        text: chartTitle
      },
      series: [{
        type: 'pie',
        name: '',
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

    };

    this.chart = chart(this.chartTarget.nativeElement, options);
  }

  GetCompanyReports() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.getFile(this.config.Emar_CompanyReport_GetCompanyReport+ this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "CompanyReport_" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }

  GetFacilityReports(companyId: any) {
    this.ng4LoadingSpinnerService.show();
    companyId = this.companyId
    this.dataservice.getFile(this.config.Emar_FaclityReport_GetFacilityReportbyCompanyId + companyId+"/"+ this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "FacilityReport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  GetNursestationReports(companyId: any, facilityId: any) {
    this.ng4LoadingSpinnerService.show();
    companyId = this.companyId;
    facilityId = this.facilityId;
    this.dataservice.getFile(this.config.Emar_NursestationReport_GetNursestationReportbyId + companyId + "/" + facilityId+"/"+ this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "NursingStationReport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  GetFloorReport() {
    this.ng4LoadingSpinnerService.show();
    let companyId = this.companyId;
    let facilityId = this.facilityId;
    let stationId = this.stationId;
    this.dataservice.getFile(this.config.Emar_FloorReport_GetFloorReportbyId + companyId + "/" + facilityId + "/" + stationId+ "/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId)+"/" + this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "FloorReport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }

  GetWingReports() {
    this.ng4LoadingSpinnerService.show();
    let companyId = this.companyId;
    let facilityId = this.facilityId;
    let stationId = this.stationId;
    this.dataservice.getFile(this.config.Emar_WingReport_GetWingReportbyId + companyId + "/" + facilityId + "/" + stationId+ "/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId)+"/"+ this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "WingReport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  GetRoomReports() {
    this.ng4LoadingSpinnerService.show();
    let companyId = this.companyId;
    let facilityId = this.facilityId;
    let stationId = this.stationId;
    this.dataservice.getFile(this.config.Emar_RoomReport_GetRoomReportbyId + companyId + "/" + facilityId + "/" + stationId +"/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId)+"/"+ this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "RoomReport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  GetBedReports() {
    this.ng4LoadingSpinnerService.show();
    let companyId = this.companyId;
    let facilityId = this.facilityId;
    let stationId = this.stationId;
    this.dataservice.getFile(this.config.Emar_BedReport_GetBedReportbyId + companyId + "/" + facilityId + "/" + stationId +"/" + (this.floorId==undefined?0:this.floorId) + "/" + (this.wingId==undefined?0:this.wingId) + "/" + (this.roomId==undefined?0:this.roomId)+"/"+(this.bedId==undefined?0:this.bedId)+"/"+ this.dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "BedReport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getCensusByNursingStation(nsId: any, chartTitle: string)
  {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetCensusByNursingStation + this.stationId)
      .subscribe(res => {
        this.censusList = res;
        this.floorId=undefined;
        this.wingId=undefined;
        this.roomId=undefined;
        this.bedId=undefined;
        this.floors=[];
        this.wings=[];
        this.rooms=[];
        this.beds=[];
        this.residentsList=[];
        if(this.censusList.length>0)
        {
          if(this.drilldownLevel==4)
          {
            this.isHierarchyExists=0;
          }
        let arr = this.censusList;
        //let percentage = 100 / arr.length;

        let seriesData = arr.map(function (val: any) {
          let percentage=parseInt(val.CensusCount);
          return ([createLable(val.CensusTypeId, val.CensusType), percentage]);

        });
        this.createChart(seriesData, chartTitle);
        }
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getCensusResidentsByNursingStation(nsId: any, chartTitle: string)
  {
    debugger
    let obj= {
      Floors: [],
      Facilities: [],
      NurseStations:[this.stationId],
      Wings: [],
      Rooms:  [],
      Beds:[],
      ResidentType:parseInt(this.censusType),
      CurrentPage: 1,
      PageSize: 100,
      SearchText: "",
      RecentFacNsFalg:1,
    }
    this.dataservice.post(this.config.Emar_Facility_GetCensusResidentsList,obj)
      .subscribe(res => {
        this.residentsList = res;
        this.floorId=undefined;
        this.wingId=undefined;
        this.roomId=undefined;
        this.bedId=undefined;
        this.floors=[];
        this.wings=[];
        this.rooms=[];
        this.beds=[];
        if(this.residentsList.length>0)
        {
          this.censusList=[];
        let arr = this.residentsList;
        let percentage = 100 / arr.length;

        let seriesData = arr.map(function (val: any) {
          return ([createLable(val.Patient_Id, (val.PatientLastName+", "+val.PatientFirstName+" "+(val.PatientMiddleInitial==null?'':val.PatientMiddleInitial))), percentage]);

        });
        this.createChart(seriesData, "Residents");
        }
        //  $('#tableCompany_wrapper').css('display','none');
        //  this.chRef.detectChanges();        
        //const table:any=$('#tableFacilities');
        //this.dataTableFacility=table.DataTable();
        else{
          this.drilldownLevel=this.drilldownLevel-1;
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
