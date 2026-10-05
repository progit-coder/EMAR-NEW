import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { FormGroup, FormControl } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';
import { UserModel } from '../../../models/user.model';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-adminusersdr',
  templateUrl: './adminusersdr.component.html',
  styleUrls: ['./adminusersdr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class AdminusersdrComponent implements OnInit {
  [x: string]: any;
  gridData: any[] = [];
  p: number = 1;
  gridPagination = this.config.gridPagination;
  dashboardForm: FormGroup;
  backButton = false;
  public companies: any[];
  dropdownSettings_Company: any = {};
  public selectedItems = [];
  ShowFilter = true;
  searchText: string = "";
  public companyId: string;
  public template;
  public userId: number;
  public nursestationid: string = "";
  pageConfig = {};
  public totalRecords: any;
  constructor(private dataservice: DataService, private config: APIConfiguration, private exceldownload: ExceldownloadService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private sharedService: SharedService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ListofUsersReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        let date = new Date();
        this.dashboardForm = new FormGroup({
          companyName: new FormControl(''),
        });
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.userActivity();
        this.dropdownSettings_Company = {
          singleSelection: false,
          idField: "Company_Id",
          textField: "Company_Name",
          text: "Select Company",
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter
        };
        this.getCompanies();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.AdminUsersReport, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getCompanies() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => {
        this.companies = res;
        this.dashboardForm.patchValue({
          companyName: res,
        });
        res.forEach(item => this.selectedItems.push(item.Company_Id));
        this.companyId = "";
        this.companyId = this.selectedItems.join(',');
        this.ng4LoadingSpinnerService.hide();
        this.getGridData(this.companyId);

      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getGridData(companies: any, currentPage?: any) {
    this.ng4LoadingSpinnerService.show();
    currentPage = currentPage == undefined ? 1 : currentPage;
    this.dataservice.get<any>(this.config.Emar_Dashboard_GetAdminUseraDashboardGrid + companies + "/" + this.userId + "/" + currentPage + "/" + this.gridPagination)
      .subscribe(res => {
        if (res != null) {
          this.gridData = res.GridData;
          this.totalRecords = res.TotalRecordsCount;
          this.p = currentPage;
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getGridDataOnPageChange(currentPage: any) {
    this.getGridData(this.companyId, currentPage);
  }

  getReport() {
    this.ng4LoadingSpinnerService.show();
    if (this.dashboardForm.value.companyName.length == 0 || this.dashboardForm.value.companyName == undefined || this.dashboardForm.value.companyName == null) {
      this.alertService.warn("Please select at least one company to display data");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      let dateTime = this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      this.dataservice.getFile(this.config.Emar_Reports_GetAdminUsersDetailsReport + this.companyId + "/" + this.userId + "/" + dateTime)
        .subscribe((res) => {
          this.ng4LoadingSpinnerService.hide();
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "Admin_Users  " + x.getMonth() + "_" + x.getDay() + '.pdf';
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
    if (this.dashboardForm.value.companyName.length == 0 || this.dashboardForm.value.companyName == undefined || this.dashboardForm.value.companyName == null) {
      this.alertService.warn("Please select at least one company to display data");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetAdminUsersDetailsExcel + this.companyId + "/" + this.userId)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res.length != 0)
          {

          
          this.exceldownload.excelDownload(res, "Admin_Users");
          }
          else
          {
            this.alertService.warn("No data available");
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  getSelectedCompany() {

    if (this.dashboardForm.value.companyName.length != 0) {
      this.selectedItems.length = 0;
      this.dashboardForm.value.companyName.forEach(item => this.selectedItems.push(item.Company_Id));
      this.companyId = "";
      this.companyId = this.selectedItems.join(',');
      this.getGridData(this.companyId);
    }
    else if (this.dashboardForm.value.companyName.length == 0) {
      this.selectedItems.length = 0;
      this.alertService.warn("Please select at least one company to display data");
      this.ng4LoadingSpinnerService.hide();
      this.gridData = [];
      this.totalRecords = 0;
    }
  }
  onCompanySelect(item: any) {
    this.getSelectedCompany();
  }
  onCompanySelectAll(item: any) {
    this.dashboardForm.value.companyName = item;
    this.getSelectedCompany();
  }
  onCompanyDeSelect(item: any) {
    this.getSelectedCompany();
  }
  onCompanyDeSelectAll(item: any) {
    this.dashboardForm.value.companyName.length = 0;
    this.nursestationid = "";
    this.selectedItems = [];
    this.alertService.warn("Please select at least one company to display data");
    this.ng4LoadingSpinnerService.hide();
    this.gridData = [];
    this.totalRecords = 0;
  }
}
