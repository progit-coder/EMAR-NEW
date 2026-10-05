import { Component, OnInit, ChangeDetectorRef, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FileInformation, OutboundList } from '../../../models/InboundFiles.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { AlertService } from '../../../_services';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { PersistanceService } from '../../../services/shared/persistance.service';
@Component({
  selector: 'app-outboundfiles',
  templateUrl: './outboundfiles.component.html',
  styleUrls: ['./outboundfiles.component.css'],
  providers: [DataService, APIConfiguration]
})
export class OutboundfilesComponent implements OnInit {
  public template;
  data: string;
  errorMessage: any;
  companyMaster: any[];
  savedFiles: FileInformation[] = [];
  outboundList: OutboundList[];
  CheckAll: boolean = false;
  selectedOutboundList = [];
  total: number = 0;
  success: number;
  await:number;
  error: number;
  companyselected: number;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  searchText: string = "";
  @ViewChild('livechart') chartTarget: ElementRef;
  chart: Highcharts.Chart;
  lastAccess: string;
  intervalIdGet: any;
  viewTitle: string = "";
  fileViewData: any;
  public modalEditIsOpen: boolean = false;
  myform: FormGroup;
  dropdownSettings_Resident: any = {};
  dropdownSettings_Company: any = {};
  public selectedResItem = [];
  public selectedComItem = [];
  public residents: any[];
  public fromDate: string;
  public toDate: string;
  public residentName: string;
  pageConfig = {};
  statusApiUrl: any;
  public userId: number;
  public loginUserReceCompany:any;
  maxSelectDate: string = this.dateFormatPipe.dateFormat(new Date());
  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService) {

  }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Outbound");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.getUserRecentCompany();
    //this.getCompanyMaster();
    //this.getOutboundList();
    let date = new Date();
    this.fromDate =  this.dateFormatPipe.transformISODate(new Date());
    this.toDate =  this.dateFormatPipe.transformISODate(new Date());
    this.myform = new FormGroup({
      company: new FormControl(''),
      resident: new FormControl(''),
    });
    this.dataservice.get<any>(this.config.Emar_Inbound_GetInboundFoltChartData + 2)
      .subscribe(res => {

        this.lastAccess = res[0].lastAccess
        var data = [];
        for (var i = 0; i < res.length; i++) {
          var time = (new Date()).getTime();
          data.push({
            x: time + (19 + i) * 1000,
            y: parseInt(res[i].count)
          });
        }
        let data2 = [{
          name: 'File Count',
          data: data
        }]
        this.FlotChart(data2);
      }, error => {
        this.alertService.error("Something went wrong on Server.");
        clearInterval(this.intervalIdGet);
      });
      this.dropdownSettings_Resident = {
        singleSelection: true,
        idField: "PatientName",
        textField: "PatientName",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true
      };
      this.dropdownSettings_Company = {
        singleSelection: true,
        idField: "Company_Id",
        textField: "Company_Name",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true
      };
    this.GetInboundFileCountByTime();
    this.userActivity();
    
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Outbound, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  ngOnDestroy() {
    if (this.intervalIdGet) {
      clearInterval(this.intervalIdGet);
    }
    localStorage.removeItem('data');
  }
  getUserRecentCompany() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceCompany = res.companyId;
        }
        this.getCompanyMaster();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => {
        this.companyMaster = res;
        //this.ng4LoadingSpinnerService.hide();
        if (this.companyMaster.length > 0) {
          if (this.loginUserReceCompany != null) {
              let checkComExist = this.companyMaster.find(r => r.Company_Id == parseInt(this.loginUserReceCompany));
              this.selectedComItem = [];
              if (checkComExist != undefined) {
                this.selectedComItem.push(checkComExist);
              this.myform.patchValue({
                company: this.selectedComItem,
              });
              this.companyselected = this.selectedComItem[0].Company_Id;
              this.getResidentDropData(this.companyselected);
              this.getOutboundFilesByFilter(null, this.companyselected);
              }
              else {
                this.companyselected = this.companyMaster[0].Company_Id;
                this.getResidentDropData(this.companyMaster[0].Company_Id);
                this.getOutboundFilesByFilter(null, this.companyselected);
                this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
                this.myform.patchValue({
                  company: this.selectedComItem,
                });
              }
          } 
          else
          {
              this.companyselected = this.companyMaster[0].Company_Id;
              this.getResidentDropData(this.companyMaster[0].Company_Id);
              this.getOutboundFilesByFilter(null, this.companyselected);
              this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
              this.myform.patchValue({
                company: this.selectedComItem,
              });
          }
        }
        // this.companyselected = this.companyMaster[0].Company_Id;
        // this.getOutboundFilesByFilter(null,this.companyMaster[0].Company_Id);
        // this.getResidentDropData(this.companyMaster[0].Company_Id);
        // this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
        // this.myform.patchValue({
        //   company: this.selectedComItem,
        // });
      },
        error => {
          this.errorMessage = <any>error;
          this.ng4LoadingSpinnerService.hide();
          // this.loading = false;
        });
  }
  downLoadFile(fileId, fileName) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.getFile(this.config.Emar_Outbound_DownloadOutboundFile + fileId)
      .subscribe((res: any) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'text/plain' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = fileName;
        a.download = link.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = <any>error.message
        console.log(this.errorMessage)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getOutboundList() {
    this.dataservice.get<OutboundList[]>(this.config.Emar_Outbound_GetOutboundDataList)
      .subscribe(res => {
        this.outboundList = res;
      }, error => {
        this.errorMessage = <any>error.message;
        this.ng4LoadingSpinnerService.hide();
        console.log(this.errorMessage);
      });

  }

  // getOutboundFiles(companyId: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   let fileCategory = "Outbound";
  //   var dt1 = this.fromDate;
  //   var dt2 = this.toDate;
  //   this.dataservice.get<FileInformation[]>(this.config.Emar_Outbound_GetOutboundFilesList + companyId + "/" + fileCategory+ "/" + dt1 + "/" + dt2)
  //     .subscribe(res => {
  //       this.savedFiles = res;
  //       this.success = this.savedFiles.filter(sf => sf.File_Error == '0' || sf.File_Error == '').length;
  //       this.error = this.savedFiles.filter(sf => sf.File_Error == '1').length;
  //       this.total = this.success + this.error;
  //       this.getResidentDropData(companyId);
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.errorMessage = <any>error.message;
  //       this.ng4LoadingSpinnerService.hide();
  //       console.log(this.errorMessage);
  //     });
  // }
  getResidentDropData(companyId: number) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Outbound_GetOutboundResidentsDrop + userId + "/" + companyId)
      .subscribe(res => {
        this.residents = res;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  onOutboundSelect(event, item: OutboundList) {

    if (event.target.checked)
      this.selectedOutboundList.push(item);
    else {
      const index = this.selectedOutboundList.findIndex(list => list.RecordId == item.RecordId && list.Patient_Id == item.Patient_Id);
      this.selectedOutboundList.splice(index, 1);
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.CheckAll = true;
      this.outboundList.forEach(element => {
        this.selectedOutboundList.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.selectedOutboundList.length = 0;
    }
  }
  // generateOutboundFiles() {
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.post(this.config.Emar_Outbound_GenerateOutboundFile, this.selectedOutboundList)
  //     .subscribe(res => {
  //       this.selectedOutboundList.length = 0;
  //       this.getOutboundList();
  //       //this.getOutboundFiles();
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.errorMessage = <any>error.message;
  //       this.ng4LoadingSpinnerService.hide();
  //       console.log(this.errorMessage);
  //     });

  // }
  GetFileAckDataForOutbound(fileID: number, FileName: string) {
    this.ng4LoadingSpinnerService.show();
    this.viewTitle = FileName;
    this.dataservice.get<any>(this.config.Emar_Outbound_GetFileAckDataForOutbound + fileID)
      .subscribe(res => {
        this.fileViewData = res;
        this.ng4LoadingSpinnerService.hide();
        this.modalEditIsOpen = true;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  Modalclose() {
    this.modalEditIsOpen = false;
  }
  FlotChart(data) {


    const options: Highcharts.Options = {
      chart: {
        type: 'spline',
        //animation: Highcharts.svg, // don't animate in old IE
        marginRight: 10,
        events: {
          load: function () {
            //const p = e.point;
            // set up the updating of the chart each second
            // set up the updating of the chart each second
            var series = this.series[0];
            setInterval(function () {

              let da = localStorage.getItem("data");
              var x = (new Date()).getTime(), // current time
                y = parseInt(da);//Math.random();
              series.addPoint([x, y], true, true);
            }, 2000);
          }
        }
      },

      time: {
        useUTC: false
      },
      colors: ['#ade39d',
    ],
      title: {
        text: 'Outbound'
      },
      xAxis: {
        type: 'datetime',
        tickPixelInterval: 150
      },
      yAxis: {
        title: {
          text: 'Value'
        },
        plotLines: [{
          value: 0,
          width: 1,
          color: '#808080'
        }]
      },
      tooltip: {
        headerFormat: '<b>{series.name}</b><br/>',
        pointFormat: '{point.x:%Y-%m-%d %H:%M:%S}<br/>{point.y:.2f}'
      },
      legend: {
        enabled: false
      },
      exporting: {
        enabled: false
      },
      credits: {
        enabled: false
      },
      series: data
    };

    this.chart = chart(this.chartTarget.nativeElement, options);
  }
  GetInboundFileCountByTime() {
    // return this.dataservice.get<number>(this.config.Emar_Inbound_GetInboundFileCountByTime + '2/3/2019 4:32:35 PM').map(
    //   (response) => {
    //     return response;
    //   }
    // );
    this.intervalIdGet = setInterval(() => {
      this.dataservice.get<number>(this.config.Emar_Inbound_GetInboundFileCountByTime + 2)
        .subscribe(res => {
          localStorage.setItem("data", res.toString());
        }, error => {
          this.alertService.error("Something went wrong on Server.");
          clearInterval(this.intervalIdGet);
        });
    }, 2000);
    // return this.dataservice.get<number>(this.config.Emar_Inbound_GetInboundFileCountByTime+ i)
    //   .subscribe(res => {        
    //     return res;
    //   }, error => this.errorMessage = <any>error);
  }
  getOutboundFilesStatus(fileStatus: number) {
    this.ng4LoadingSpinnerService.show();
    var dt1 = this.fromDate;
    var dt2 = this.toDate;
    if (this.selectedComItem.length != 0) {
      if (this.selectedResItem.length == 0 && this.selectedComItem.length != 0)

        this.statusApiUrl = this.config.Emar_Outbound_GetOutboundFilesStatus + this.companyselected + "/" + fileStatus + "/" + dt1 + "/" + dt2;
      else if (this.selectedResItem.length != 0 && this.selectedComItem.length != 0)

        this.statusApiUrl = this.config.Emar_Outbound_GetOutboundFilesStatus + this.companyselected + "/" + fileStatus + "/" + dt1 + "/" + dt2 + "/" + this.selectedResItem;
    this.dataservice.get<FileInformation[]>(this.statusApiUrl)
      .subscribe(res => {
        this.savedFiles = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = <any>error.message;
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else {
      this.alertService.warn("Please select proper data");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onCompanySelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.companyselected = item.Company_Id;
    this.residents = [];
    this.myform.controls['resident'].reset();
    this.selectedResItem=[];
    this.getResidentDropData(item.Company_Id);
    this.getOutboundFilesByFilter(null,item.Company_Id);
  }
  onCompanyDeSelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.savedFiles = [];
    this.success = 0;
    this.error = 0;
    this.total = 0;
    this.residents = [];
    this.companyselected = 0;
    this.myform.controls['resident'].reset();
    this.selectedResItem=[];
    this.fromDate =  this.dateFormatPipe.transformISODate(new Date());
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    this.alertService.warn("Please Select atleast one company to display data");
    this.ng4LoadingSpinnerService.hide();
  }
  onResidentSelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.getOutboundFilesByFilter(item);
  }
  onResidentDeSelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.getOutboundFilesByFilter(null,this.companyselected);
  }
  getOutboundFilesByFilter(residentName: any, companyId?: any) {
    debugger
    var dt1 = this.fromDate;
    var dt2 = this.toDate;
    let fileCategory = "Outbound";
    let url = (residentName == null || residentName==undefined) ? this.config.Emar_Outbound_GetOutboundFilesList + companyId + "/" + fileCategory + "/" + dt1 + "/" + dt2 : this.config.Emar_Outbound_GetOutboundFilesList + this.companyselected + "/" + fileCategory + "/" + dt1 + "/" + dt2 + "/" + residentName.split('.').join('|');
    this.dataservice.get<FileInformation[]>(url)
      .subscribe(res => {
        this.savedFiles = res;
        this.success = this.savedFiles.filter(sf => sf.File_ErrorDesc == 'Success').length;
        this.error = this.savedFiles.filter(sf => sf.File_ErrorDesc == 'Error').length;
        this.await = this.savedFiles.filter(sf => sf.File_ErrorDesc == 'Awaiting').length;
        this.total = this.success + this.error+this.await;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  loadGridData() {
    if(this.fromDate!="" && this.toDate!="")
    {
    var dt1 = this.fromDate;
    var dt2 = this.toDate;
    if (dt1 > dt2) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
      this.savedFiles = [];
      this.success = 0;
      this.error = 0;
      this.total = 0;
    }
    else if (dt2 < dt1) {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
      this.savedFiles = [];
      this.success = 0;
      this.error = 0;
      this.total = 0;
    }
    else {
      this.ng4LoadingSpinnerService.show();
      if (this.selectedResItem.length == 0 && this.selectedComItem.length != 0) {
        this.getOutboundFilesByFilter(null,this.selectedComItem[0].Company_Id);
      }
      else if ( this.selectedResItem.length != 0 && this.selectedComItem.length != 0) {
        this.getOutboundFilesByFilter(this.selectedResItem[0]);
      }
      else {
        this.alertService.warn("Please Select proper data.");
        this.ng4LoadingSpinnerService.hide();
      }
    }
    //this.ng4LoadingSpinnerService.hide();
  }
}
}