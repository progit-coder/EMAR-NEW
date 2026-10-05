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
@Component({
  selector: 'app-stockdr',
  templateUrl: './stockdr.component.html',
  styleUrls: ['./stockdr.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class StockdrComponent implements OnInit {
  [x: string]: any;
  gridData:any[]=[];
  p: number = 1;
  gridPagination = this.config.gridPagination;
  dashboardForm: FormGroup;
  backButton = false;
  public nurseStations: NurseStation[];
  dropdownSettings_User: any = {};
  dropdownSettings_Company:any={};
  public selectedcItem=[];
  public selectedItems = [];
  ShowFilter = true;
  searchText: string = "";
  public selectedUser:number;
  public usersList:any[]=[];
  public columnDisplayStatus:number;
  public template;
  public userId:number;
  public selectedItem:any[]=[];
  public companyMaster:any[]=[];
  public selectedCompany:number;
  pageConfig = {};
  public totalRecords: any;
  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService,private sharedService: SharedService, private exceldownload: ExceldownloadService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("E-Kit/On-siteMedsDispensingReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    let date = new Date();
    this.dashboardForm = new FormGroup({
      user: new FormControl(''),
      company:new FormControl('')
    });
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.userActivity();
    this.getUsers();
    this.dropdownSettings_User = {
      singleSelection: true,
      idField: "User_Id",
      textField: "UserName",
      text: "Select User",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Company = {
      singleSelection: true,
      idField: "Company_Id",
      textField: "Company_Name",
      text: "Select User",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getUsers() {
    this.dataservice.get<any[]>(this.config.Emar_UserMaster_GetStockReportUserDropData)
      .subscribe(res => {
        this.usersList = res;
        this.selectedUser=this.usersList[0].User_Id;
        this.selectedItem=this.usersList.filter(u=>u.User_Id==this.selectedUser);
        this.dashboardForm.patchValue({
          user:this.selectedItem,
        });
        this.getCompanyMaster(this.selectedUser);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getGridData(currentPage?:any) {
    this.ng4LoadingSpinnerService.show();
    currentPage = currentPage==undefined?1:currentPage;
    this.dataservice.get<any>(this.config.Emar_Reports_GetStockDashboardGrid +this.selectedUser+"/"+ this.selectedCompany+"/"+currentPage+"/"+ this.gridPagination)
      .subscribe(res => {
        if(res==null)
        {
          this.alertService.warn("No data available.");
          this.gridData = [];
        }
        if (res != null) {
          this.gridData = res.GridData;
          this.columnDisplayStatus=res.GridData[0].status;
          this.totalRecords =res.TotalRecordsCount;
          this.p=currentPage;
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getGridDataOnPageChange(currentPage:any)
  {
    this.getGridData(currentPage);
  }
  getReport() {
    if( this.selectedUser==0|| this.selectedUser==undefined ||this.selectedCompany==0 || this.selectedCompany==undefined)
    {
      this.alertService.error("Please select at least one user to display data");
    }
    else{
      this.ng4LoadingSpinnerService.show();
    this.dataservice.getFile(this.config.Emar_Reports_GetStockDetailsReport + this.selectedUser + "/" + this.userId + "/"+ this.dashboardForm.value.company[0].Company_Id)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "Stock  " + x.getMonth() + "_" + x.getDay() + '.pdf';
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
  if( this.selectedUser==0|| this.selectedUser==undefined ||this.selectedCompany==0 || this.selectedCompany==undefined)
  {
    this.alertService.error("Please select at least one user to display data");
  }
  else{
  this.dataservice.get<any[]>(this.config.Emar_Reports_GetEkitMedsDispensingExcel + this.selectedUser + "/"+ this.dashboardForm.value.company[0].Company_Id)
    .subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      // this.excelforuseractivity = res;
      if (res.length != 0)
      {

      
      this.exceldownload.excelDownload(res, "stock");
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
  onUserSelect(item: any) {
    this.selectedUser=item.User_Id;
    this.getCompanyMaster(this.selectedUser);
  }
  onUserDeSelect(item: any) {
    this.alertService.error("Please select at least one user to display data");
    this.selectedUser=0;
    this.gridData = [];
    this.dashboardForm.patchValue({
      company:'',
    })
    //this.getGridData();
  }
  onCompanySelect(item: any) {
    this.selectedCompany=item.Company_Id;
    this.getGridData();
  }
  onCompanyDeSelect(item: any) {
    this.alertService.error("Please select at least one company to display data");
    this.selectedCompany=0;
    this.gridData = [];
  }
  getCompanyMaster(userId:any) {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetStockUserCompanyDrop +userId)
      .subscribe(res => {
        this.companyMaster = res;
        this.selectedCompany=this.companyMaster[0].Company_Id; 
        this.selectedcItem=this.companyMaster.filter(u=>u.Company_Id==this.selectedCompany);
        this.dashboardForm.patchValue({
          company:this.selectedcItem,
        });
        this.getGridData();
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.StockDashboard,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
}
