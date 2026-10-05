import { Component, OnInit, Output, EventEmitter, ElementRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FileInformation } from '../../../models/InboundFiles.model';
import { FormGroup, FormControl } from '@angular/forms';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { GridFilterPipe } from '../../../services/shared/grid-filter.pipe';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { AlertService } from '../../../_services';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-inboundfiles',
  templateUrl: './inboundfiles.component.html',
  styleUrls: ['./inboundfiles.component.css'],
  providers: [DataService, APIConfiguration, GridFilterPipe]
})
export class InboundfilesComponent implements OnInit {
  public template;
  //public inboundFiles: InboundFiles[];
  errorMessage: string;
  searchText: string = "";
  myform: FormGroup;
  CheckAll: boolean = false;
  // @Output() companyId: EventEmitter<number> = new EventEmitter<number>();
  savedFiles: FileInformation[] = [];
  public companyMaster: any[];
  companyId: number;
  // public loading = true;
  companyselected;
  //hlFieldsinfo: HlFields[];
  viewTitle: string = "";
  total: number = 0;
  success: number;
  error: number;
  rejected: number;
  Pendingfiles:number=0;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  totalRecords: number = 0;
  fileViewData: any;
  lastAccess: string;
  lastAccessImp: string;
  @ViewChild('livechart') chartTarget: ElementRef;
  @ViewChild('livechartwin') livechartwin: ElementRef;
  chart: Highcharts.Chart;
  public modalEditIsOpen: boolean = false;
  intervalIdGet: any;
  intervalIdImport: any;
  intervalChart1: any;
  intervalChart2: any;
  intervalPending: any;
  public fileError: any[] = [];
  public fileAck: string;
  dropdownSettings_Resident: any = {};
  dropdownSettings_Company: any = {};
  public selectedResItem = [];
  public selectedComItem = [];
  public residents: any[];
  public fromDate: string;
  public toDate: string;
  public residentName: string;
  statusApiUrl: any;
  maxSelectDate: string = this.dateFormatPipe.dateFormat(new Date());
  public display: boolean = true;
  pageConfig = {};
  selectedStatus: number = 2;
  public userId: number;
  public loginUserReceCompany:any;
  public fileData:string="";
  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, public sharedService: SharedService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, ) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Inbound");
    this.GetHl7PendingCount();
    this.intervalChart1 = setInterval(function () {
      this.GetHl7PendingCount();
    }.bind(this), 10000);
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        //this.getCompanyMaster();
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.getUserRecentCompany();
        this.fromDate = this.dateFormatPipe.transformISODate(new Date());
        this.toDate = this.dateFormatPipe.transformISODate(new Date());
        this.myform = new FormGroup({
          company: new FormControl(''),
          resident: new FormControl(''),
        });
        this.dataservice.get<any>(this.config.Emar_Inbound_GetInboundFoltChartData + 1)
          .subscribe(res => {
            let sp: string = res[0].lastAccess;
            let sp2 = sp.split(':');
            this.lastAccess = sp2[0] + " Hr: " + sp2[1] + " Min: " + sp2[2].substring(0, 2) + " Sec";
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
            //this.alertService.error(error.message);
            this.alertService.error("Something went wrong on Server.");
          });
        this.dataservice.get<any>(this.config.Emar_Inbound_GetInboundImportFoltChartData + 1)
          .subscribe(res => {
            let sp: string = res[0].lastAccess;
            let sp2 = sp.split(':');
            this.lastAccessImp = sp2[0] + " Hr: " + sp2[1] + " Min: " + sp2[2].substring(0, 2) + " Sec";
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
            this.FlotChartWin(data2);
          }, error => {
            //this.alertService.error(error.message);
            this.alertService.error("Something went wrong on Server.");
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
        this.GetInboundErrorFileCountByTime();
        this.userActivity();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  getUserRecentCompany() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceCompany = res.companyId;
        }
        this.getCompanyMaster()
      }, error => {
        this.alertService.error(error.message);
      });
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Inbound, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  ngOnDestroy() {
    if (this.intervalIdGet) {
      clearInterval(this.intervalIdGet);
    }
    if (this.intervalIdImport) {
      clearInterval(this.intervalIdImport);
    }
    if (this.intervalChart1) {
      clearInterval(this.intervalChart1);
    }
    if (this.intervalPending) {
      clearInterval(this.intervalPending);
    }
    if (this.intervalChart2) {
      clearInterval(this.intervalChart2);
    }
    localStorage.removeItem('data');
    localStorage.removeItem('data2');
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
              this.getSavedFilesByFilter(1);
              }
              else {
                this.companyselected = this.companyMaster[0].Company_Id;
                this.getResidentDropData(this.companyMaster[0].Company_Id);
                this.getSavedFilesByFilter(1);
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
              this.getSavedFilesByFilter(1);
              this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
              this.myform.patchValue({
                company: this.selectedComItem,
              });
          }
          // this.companyselected = this.companyMaster[0].Company_Id;
          // this.getResidentDropData(this.companyMaster[0].Company_Id);
          // this.getSavedFilesByFilter(1);
          // this.selectedComItem.push(this.companyMaster.filter(c => c.Company_Id == this.companyMaster[0].Company_Id)[0]);
          // this.myform.patchValue({
          //   company: this.selectedComItem,
          // });
        }
        //this.getAllRejectedFiles();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  // getAllRejectedFiles() {
  //   this.ng4LoadingSpinnerService.show();
  //   if (this.fromDate != "" && this.toDate != "") {
  //     var dt1 = this.fromDate;
  //     var dt2 = this.toDate;
  //     this.dataservice.get<any[]>(this.config.Emar_Inbound_GetInboundRejectedFiles + dt1 + "/" + dt2)
  //       .subscribe(res => {

  //         this.rejected = res.length;
  //         this.ng4LoadingSpinnerService.hide();
  //       }, error => {
  //         this.alertService.error(error.message);
  //         this.ng4LoadingSpinnerService.hide();
  //       });
  //   }
  // }
  // getRejectedFiles(currentPage: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   var dt1 = this.fromDate;
  //   var dt2 = this.toDate;
  //   this.dataservice.get<any[]>(this.config.Emar_Inbound_GetInboundRejectedFiles + dt1 + "/" + dt2 + "/" + currentPage + "/" + this.gridPagination)
  //     .subscribe(res => {
  //       this.savedFiles = res;
  //       this.totalRecords = res.length;
  //       this.display = false;
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  // public changeCompany(companyId: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   this.companyselected = companyId;
  //   this.GetSavedFiles(companyId);
  // }
  // getAllInboundFiles(companyId: number) {
  //   this.dataservice.get<InboundFiles[]>(this.config.Emar_Inbound_GetInboundFilesFromLocal + companyId)
  //     .subscribe(res => {

  //       this.inboundFiles = res;
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  //   this.getSavedFilesByFilter(null, companyId);
  // }
  // getInboundFilesFromFTP(companyId: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<InboundFiles[]>(this.config.Emar_Inbound_GetInboundFilesFromFTP + companyId)
  //     .subscribe(res => {
  //       this.inboundFiles = res
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  // chekedFiles: string[] = [];// = { "fileNames": [] };
  // chekedFilesdown: number[] = [];
  // onChangeCategory(event, item: InboundFiles) {
  //   this.chekedFiles.push(item.FileName);

  // }
  // onChangeCategorydown(event, item: FileInformation) {
  //   this.chekedFilesdown.push(item.File_Id);

  // }
  // onCheckAll(event) {
  //   if (event == true) {
  //     this.CheckAll = true;
  //     this.inboundFiles.forEach(element => {
  //       this.chekedFiles.push(element.FileName);
  //     });
  //   }
  //   else {
  //     this.CheckAll = false;
  //     this.chekedFiles = [];
  //   }
  // }
  // GetSavedFiles(companyID: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   let fileCategory = "Inbound";
  //   var dt1 = this.fromDate;
  //   var dt2 = this.toDate;
  //   this.dataservice.get<any[]>(this.config.Emar_Inbound_GetInboundOutboundFilesByResident + companyID + "/" + fileCategory + "/" + dt1 + "/" + dt2)
  //     .subscribe(res => {
  //       // this.dataservice.get<any[]>(this.config.Emar_Inbound_GetSavedFilesList + companyID + "/" + fileCategory)
  //       //   .subscribe(res => {
  //       this.savedFiles = res;
  //       this.success = this.savedFiles.filter(sf => sf.File_Error == "0").length;
  //       this.error = this.savedFiles.filter(sf => sf.File_Error == "1").length;
  //       this.total = + this.success + this.error;
  //       this.getResidentDropData(companyID);
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  // GetHlFiledsInformation(FileID: number, FileName: string) {

  //   this.viewTitle = FileName;
  //   this.dataservice.get<HlFields[]>(this.config.Emar_Inbound_GetHlFieldsInformation + FileID)
  //     .subscribe(res => {

  //       this.hlFieldsinfo = res;
  //       this.modalEditIsOpen = true;
  //       this.modalEditIsOpen = true;
  //     }, error => this.alertService.error(error.message));
  // }
  Modalclose() {
    this.modalEditIsOpen = false;
    this.fileError = [];
  }
  // ImportInboundFiles() {
  //   //this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<number>(this.config.Emar_Inbound_ImportInboundFiles)
  //     .subscribe(res => {
  //       this.getSavedFilesByFilter(null, this.companyselected);
  //       //this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       //this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  GetFileAckData(fileID: number, FileName: string) {
    this.ng4LoadingSpinnerService.show();
    this.viewTitle = FileName;
    this.dataservice.get<any>(this.config.Emar_Inbound_GetFileAckData + fileID)
      .subscribe(res => {
        this.fileViewData = res;
        this.fileData= this.fileViewData.FileData;
        this.ng4LoadingSpinnerService.hide();
        if (res.FileError != null) {
          this.fileAck = "File Ack";
          this.modalEditIsOpen = true;
          this.fileError = res.FileError;
          //this.fileError=this.fileError.split(',')

        }
        else if (res.FileError == null) {
          this.fileAck = "File Ack Error";
          this.modalEditIsOpen = true;

        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  FlotChart(data) {
    const options: Highcharts.Options = {
      chart: {
        type: 'spline',
        //animation: Highcharts.svg, // don't animate in old IE
        marginRight: 10,
        events: {
          load: function (e) {
            //const p = e.point;
            // set up the updating of the chart each second
            // set up the updating of the chart each second
            //var series = this.series[0];
            var series = e.target.series[0];
            this.intervalChart1 = setInterval(function () {
              let da = localStorage.getItem("data");
              var x = (new Date()).getTime(), // current time
                y = parseInt(da);//Math.random();
              series.addPoint([x, y], true, true);
            }, 2000);
          }.bind(this)
        }
      },

      time: {
        useUTC: false
      },
      colors: ['#ade39d',
    ],
      title: {
        text: 'HL7 Listener'
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
  FlotChartWin(data) {
    const options: Highcharts.Options = {
      chart: {
        type: 'spline',
        //animation: Highcharts.svg, // don't animate in old IE
        marginRight: 10,
        events: {
          load: function (e) {
            //const p = e.point;
            // set up the updating of the chart each second
            // set up the updating of the chart each second
            //var series = this.series[0];
            var series = e.target.series[0];
            this.intervalChart2 = setInterval(function () {
              let da = localStorage.getItem("data2");
              var x = (new Date()).getTime(), // current time
                y = parseInt(da);//Math.random();
              series.addPoint([x, y], true, true);
            }, 2000);
          }.bind(this)
        }
      },

      time: {
        useUTC: false
      },
      colors: ['#ffdf62',
      ],
      title: {
        text: 'HL7 Import'
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

    this.chart = chart(this.livechartwin.nativeElement, options);
  }
  GetInboundFileCountByTime() {
    this.intervalIdGet = setInterval(() => {
      this.dataservice.get<any>(this.config.Emar_Inbound_GetInboundFileCountByTime + 1)
        .subscribe(res => {
          localStorage.setItem("data", res);
        }, error => {
          this.alertService.error("Something went wrong on Server.");
          clearInterval(this.intervalIdGet);
        });
    }, 2000);
  }
  GetInboundErrorFileCountByTime() {
    this.intervalIdImport = setInterval(() => {
      this.dataservice.get<any>(this.config.Emar_Inbound_GetInboundFileErrorCountByTime + 1)
        .subscribe(res => {
          localStorage.setItem("data2", res);
        }, error => {
          this.alertService.error("Something went wrong on Server.");
          clearInterval(this.intervalIdImport);
        });
    }, 2000);
  }
  // getInboundSuccessErrorCount(companyId: number) {
  //   this.dataservice.get<any>(this.config.Emar_Inbound_GetInboundSuccessErrorCount + companyId)
  //     .subscribe(res => {
  //       res.forEach(element => {
  //         if (element.type == "Error")
  //           this.error = element.count;
  //         else if (element.type == "Completed")
  //           this.success = element.count;
  //         else if (element.type == "Total")
  //           this.total = element.count;
  //       });
  //     }, error => {
  //       this.alertService.error(error.message);
  //     });
  // }
  getInboundFilesByStatus(fileStatus: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedStatus = fileStatus;
    this.display = fileStatus == 3 ? false : true;
    this.loadGridData(fileStatus);

    // var dt1 = this.fromDate;
    // var dt2 = this.toDate;
    // if (this.display == false) {
    //   //this.getCompanyMaster();
    //   this.selectedResItem = [];
    //   this.myform.patchValue({ resident: this.selectedResItem });
    //   this.display = true;
    // }
    // if (this.selectedComItem.length != 0) {
    //   if (this.selectedResItem.length == 0 && this.selectedComItem.length != 0)
    //     this.statusApiUrl = this.config.Emar_Inbound_GetInboundFilesByStatus + this.companyselected + "/" + fileStatus + "/" + dt1 + "/" + dt2;
    //   else if (this.selectedResItem.length != 0 && this.selectedComItem.length != 0)
    //     this.statusApiUrl = this.config.Emar_Inbound_GetInboundFilesByStatus + this.companyselected + "/" + fileStatus + "/" + dt1 + "/" + dt2 + "/" + this.selectedResItem;
    //   this.dataservice.get<FileInformation[]>(this.statusApiUrl)
    //     .subscribe(res => {
    //       this.savedFiles = res;
    //       this.ng4LoadingSpinnerService.hide();
    //     }, error => {
    //       this.alertService.error(error.message);
    //       this.ng4LoadingSpinnerService.hide();
    //     });
    // }
    // else {
    //   this.savedFiles = [];
    //   this.alertService.warn("Please select proper data");
    //   this.ng4LoadingSpinnerService.hide();
    // }
  }
  getResidentDropData(companyId: number) {
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Inbound_GetInboundResidentsDrop + userId + "/" + companyId)
      .subscribe(res => {
        this.residents = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  onResidentSelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.getSavedFilesByFilter(1);
  }
  onResidentDeSelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.getSavedFilesByFilter(1);
  }
  onSearchChange(searchValue: string): void {
    if (searchValue.length >= 3) {
      if (searchValue.includes(':')) {
        this.alertService.warn('Invalid character');
      }
      else {
        this.ng4LoadingSpinnerService.show();
        this.getSavedFilesByFilter(1);
      }
    }
    else if (searchValue.length == 0) {
      this.ng4LoadingSpinnerService.show();
      this.getSavedFilesByFilter(this.p);
    }
  }
  getSavedFilesByFilter(currentPage: number, status?: any) {
    var dt1 = this.fromDate;
    var dt2 = this.toDate;
    let fileCategory = "Inbound";
    this.p = currentPage;
    let searchValue = '';
    searchValue = this.searchText != undefined && this.searchText != null ? this.searchText.replace(/[&\\\#,+()$~%'":.*?<>{}\s]/g, '').replace(new RegExp('/', 'g'), '-') : '';
    searchValue = searchValue != '' && searchValue.length < 3 ? '' : searchValue;
    searchValue = searchValue == '' && this.selectedResItem.length != 0 ? null : searchValue;
    searchValue = searchValue == '' ? null : searchValue;
    status = status != undefined ? status : this.selectedStatus;
    let resName=this.selectedResItem.length!=0?this.selectedResItem[0].split('.').join('|'):'';
    // if (this.display == false)
    //   status = 3;
    // if (status == 3)
    //   this.getRejectedFiles(currentPage);
    // else if (this.display == false)
    //   this.getRejectedFiles(currentPage);
    // else {
    let url = this.config.Emar_Inbound_GetInboundOutboundFilesByResident + this.companyselected + "/" + fileCategory + "/" + dt1 + "/" + dt2 + "/" + currentPage + "/" + this.gridPagination + "/" + status + "/" + searchValue + "/" + resName;
    this.dataservice.get<any>(url)
      .subscribe(res => {
        this.savedFiles = res.Data;
        this.totalRecords = res.TotalRecords;
        this.total = res.Total;
        this.success = res.Success;
        this.error = res.Error;
        this.rejected = res.Reject;
        this.ng4LoadingSpinnerService.hide();
        // this.success = this.savedFiles.filter(sf => sf.File_Error == "0").length;
        // this.error = this.savedFiles.filter(sf => sf.File_Error == "1").length;
        // this.total = + this.success + this.error;
        //this.getAllRejectedFiles();       
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    //}
  }
  onCompanySelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.p = 1;
    this.searchText = '';
    this.companyselected = item.Company_Id;
    this.getSavedFilesByFilter(1);
    this.getResidentDropData(item.Company_Id);
    this.residents = [];
    this.myform.controls['resident'].reset();
    this.selectedResItem = [];
  }
  onCompanyDeSelect(item: any) {
    this.ng4LoadingSpinnerService.show();
    this.searchText = '';
    this.savedFiles = [];
    this.success = 0;
    this.error = 0;
    this.total = 0;
    this.residents = [];
    this.companyselected = 0;
    this.myform.controls['resident'].reset();
    this.selectedResItem = [];
    this.fromDate = this.dateFormatPipe.transformISODate(new Date());
    this.toDate = this.dateFormatPipe.transformISODate(new Date());
    //this.getAllRejectedFiles();
    this.alertService.warn("Please Select atleast one company to display data");
    this.ng4LoadingSpinnerService.hide();
  }
  loadGridData(status?: any) {
    debugger;
    this.ng4LoadingSpinnerService.show();
    if (this.fromDate != "" && this.toDate != "") {
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
        if (status == 3 && this.companyselected != 0) {
          this.display = false;
          this.getSavedFilesByFilter(1, status);
        }
        else {
          if (this.display == true) {
            if (this.companyselected == 0) {
              this.alertService.warn("Please Select atleast one company to display data");
              this.ng4LoadingSpinnerService.hide();
            }
            else {
              if (status != undefined)
                this.getSavedFilesByFilter(1, status);
              else
                this.getSavedFilesByFilter(1);
            }
          }
          else
            this.getSavedFilesByFilter(1, 3);
        }
      }
      //this.ng4LoadingSpinnerService.hide();
    }
  }
  GetHl7PendingCount()
  {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetHl7PendingCount)
    .subscribe(res => {
      this.Pendingfiles = res;
    }, error => {
      //this.alertService.error(error.message);
    });
  }
  resendFile()
  {
    let obj=
    {
      FileData:this.fileData,
      File_ResendDate:this.dateFormatPipe.dateWithTime(new Date()),
    }
    this.dataservice.post(this.config.Emar_Inbound_ResendInboundFile,obj)
    .subscribe(res => {
      this.modalEditIsOpen = false;
      this.alertService.success("File resent successfully");
    }, error => {
      this.alertService.error(error.message);
    });
  }
}
