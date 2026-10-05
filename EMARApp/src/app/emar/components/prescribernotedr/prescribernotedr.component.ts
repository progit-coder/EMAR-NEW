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
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { SharedService } from '../../../services/shared/shared.service';
@Component({
  selector: 'app-prescribernotedr',
  templateUrl: './prescribernotedr.component.html',
  styleUrls: ['./prescribernotedr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class PrescribernotedrComponent implements OnInit {
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
  constructor(private dataservice: DataService, private config: APIConfiguration, public sharedService: SharedService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService,private exceldownload: ExceldownloadService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("PrescriberNotesReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();

    this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
    this.toDate = new Date().toISOString().substring(0, 10);
    this.dashboardForm = new FormGroup({
      txtFromDate: new FormControl(this.fromDate),
      txtToDate: new FormControl(this.toDate),
      nursestationName: new FormControl(''),
      facilityName: new FormControl(''),
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
    this.getUserRecentFacilityNurseStations();
  }
}
else
this.persistanceService.redirectToHomePage();
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
  }

  getNurseStations(facilityId: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<NurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId +"/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;

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
            this.module = "prescribernote";
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
        this.module = "prescribernote";
        this.getGridData(this.module, this.fromDate, this.toDate);
        }
        this.ng4LoadingSpinnerService.hide();
      //   this.selectedItems=[];
      //   this.ng4LoadingSpinnerService.hide();
      //   this.dashboardForm.patchValue({
      //     nursestationName: this.nurseStations
      //   });
      //   res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
      //  this.nursestationid = "";
      //  this.nursestationid = this.selectedItems.join(',');
      //  this.module = "prescribernote";
      //  this.getGridData(this.module, this.fromDate, this.toDate);

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
  getGridData(moduleName: string, fromDate: string, toDate: string) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_Dashboard_GetCommonDashboard + moduleName + "/" + fromDate + "/" + toDate + "/" + this.userId + "/" + this.nursestationid)
      .subscribe(res => {
        if (res != null) {
          //let seriesData = res.YaxisData;
          //let xaxisData = res.XaxisData;
          this.gridData = res.GridData;
          this.gridColumns = res.ColumnNames;
          //this.yaxisTitle = "Count";              
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  loadChart() {
   this.fromDate = this.dashboardForm.value.txtFromDate;
   this.toDate = this.dashboardForm.value.txtToDate;
   this.close('.test');
   this.getGridData(this.module, this.fromDate, this.toDate);
 }
  //PrescriberNotes
  getReport() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.getFile(this.config.Emar_Report_GetPrescriberNotesReport + this.fromDate + "/" + this.toDate + "/" + this.userId + "/" + this.nursestationid)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "PrescriberNotes" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getExcel() {
    this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any[]>(this.config.Excel_GetCommonExcel + this.module + "/" + this.fromDate + "/" + this.toDate + "/" + this.userId + "/" + this.nursestationid)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        // this.excelforuseractivity = res;
        //  let test:any=["Start Date"]
        //      if(res.length!=0)
        //      res.forEach(function(x) {x.test =x.test.substring(0,10);
        //      });
        this.exceldownload.excelDownload(res, "PrescriberNotes");
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
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
