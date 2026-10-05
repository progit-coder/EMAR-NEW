import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators, RequiredValidator } from '@angular/forms';
import { FTEConfigMaster, FTECategory, FTEConnection } from '../../../models/fteconfig.model';
import { Company } from '../../../models/company.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';

@Component({
  selector: 'app-fteconfiguration',
  templateUrl: './fteconfiguration.component.html',
  styleUrls: ['./fteconfiguration.component.css'],
  providers: [DataService, APIConfiguration]
})
export class FteconfigurationComponent implements OnInit {
  public template;
  public FTEConfigMaster: FTEConfigMaster[] = [];
  public companies: Company[];
  public FTECategories: FTECategory[];
  public FTEConnections: FTEConnection[];
  public FteConfigID: number = 0;
  FTEConfigObj: FTEConfigMaster;
  private url: string;
  errorMessage: string;
  success: string
  myform: FormGroup;
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  pageConfig: {};
  constructor(private dataservice: DataService, private alertService: AlertService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("FTPConfiguration");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();

        this.myform = new FormGroup({
          // company: new FormControl('', Validators.required),
          serverIp: new FormControl('', [Validators.required, Validators.maxLength(15), Validators.pattern(this.config.numbersFewSpecialCharacters)]),
          category: new FormControl('', Validators.required),
          connectionType: new FormControl('', Validators.required),
          port: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.pattern(this.config.numeric)]),
          // userName: new FormControl(''),
          // password: new FormControl(''),
          status: new FormControl('1')
        });
        //this.getCompanies();
        this.getFTECategories();
        this.getFTEConnections();
        this.getFTEConfigMaster();
        this.userActivity();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.FTPConfiguration, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getCompanies() {
    this.dataservice.get<Company[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => { this.companies = res; }, error => {
        this.alertService.error(error.message)
      });
  }
  getFTECategories() {
    this.dataservice.get<FTECategory[]>(this.config.Emar_Common_GetFTECategories)
      .subscribe(res => { this.FTECategories = res; },
        error => {
          this.alertService.error(error.message);
        });
  }
  getFTEConnections() {
    this.dataservice.get<FTEConnection[]>(this.config.Emar_Common_GetFTEConnections)
      .subscribe(res => { this.FTEConnections = res; },
        error => {
          this.alertService.error(error.message);
        });
  }

  getFTEConfigMaster() {
    this.dataservice.get<FTEConfigMaster[]>(this.config.Emar_FTEConfigMaster_GetFTEConfigDetailsAll)
      .subscribe(res => {
        this.FTEConfigMaster = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = error.message;
        this.alertService.error(this.errorMessage);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getFTEConfigDetailsByID(FteConfigID: number) {
    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    this.url = this.config.Emar_FTEConfigMaster_GetFTEConfigByID + "/" + FteConfigID;
    this.dataservice.get<FTEConfigMaster>(this.url)
      .subscribe(res => {
        this.fetchDetails(res);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchDetails(res: FTEConfigMaster) {
    this.FteConfigID = res.FteConfig_Id;
    this.myform.patchValue({
      company: res.Company_Id,
      serverIp: res.ServerIp,
      category: res.Category,
      connectionType: res.ConnectionType,
      port: res.Port,
      userName: null,
      password: null,
      status: res.FteConfig_Status
    });
  }
  insertFTEConfig() {
    this.ng4LoadingSpinnerService.show();
    this.FTEConfigObj = {
      FteConfig_Id: this.FteConfigID,
      Company_Id: this.myform.value.company,
      ServerIp: this.myform.value.serverIp,
      Category: this.myform.value.category,
      ConnectionType: this.myform.value.connectionType,
      Port: this.myform.value.port,
      UserName: null,
      Password: null,
      FteConfig_Status: (this.myform.value.status == true ? 1 : 0),
      FteConfig_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      FteConfig_CreatedDate: new Date().toISOString(),
      ConnectionStatus: null
    };

    if (this.myform.value.connectionType != "3") {
      this.alertService.error("We are currently supporting only VPN");
      return;
    }

    this.dataservice.post(this.config.Emar_FTEConfigMaster_InsertFTEConfig, this.FTEConfigObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 2) {
          this.alertService.error("FTEConfiguration already exist.You can only update ");
          this.reSet();
        }
        if (res == 1) {
          this.alertService.success("Save successful");
          this.getFTEConfigMaster();
          this.reSet();
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });


  }
  reSet() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.FteConfigID = 0
    this.myform.patchValue({
      company: '',
      serverIp: '',
      category: '',
      connectionType: '',
      port: '',
      userName: '',
      password: '',
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
  }
  connectService(fteConfigId: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<string>(this.config.Emar_FTEConfigMaster_ReceiveMessageService + fteConfigId)
      .subscribe(res => {
        if (res.includes('timed')) {
          this.alertService.warn(res + ' Check whether Service started. If not try again.');
          this.ng4LoadingSpinnerService.show();
        }
        else
          this.alertService.success(res);
          this.getFTEConfigMaster();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      })
  }
}
