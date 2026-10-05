import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ApiIntegration } from '../../../models/apiintegration.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import {AlertService} from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens,Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
@Component({
  selector: 'app-integrations',
  templateUrl: './integrations.component.html',
  styleUrls: ['./integrations.component.css'],
  providers: [AlertService]
})
export class IntegrationsComponent implements OnInit {
  public myform: FormGroup;
  public companyMaster: any;
  errorMessage: string;
  companyselected: number = 1;
  public template;
  private Integration_Id: number = 0;
  private apiIntegration: ApiIntegration;
  public apiIntegrationData: any[]=[];
  searchText:string="";
  gridPagination = this.config.gridPagination;
  p: number = 1;
  pageConfig = {};

  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,private alertService: AlertService,private persistanceService: PersistanceService,public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Integrations");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();    
    this.myform = new FormGroup({
      apiPath: new FormControl('', [Validators.required,Validators.maxLength(50)]),
      userName: new FormControl('', [Validators.required,Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required,Validators.maxLength(20)]),
      type: new FormControl('', Validators.required)
    });
    this.GetApiIntegrationAllData(this.companyselected);
    this.getCompanyMaster();
    
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.Integrations,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => {
        this.companyMaster = res;
        //this.getAllInboundFiles(this.companyMaster[0].Company_Id);
       
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  InsertUpdateApiIntegration() {
    this.ng4LoadingSpinnerService.show();
    this.apiIntegration = {
      Integration_Id: this.Integration_Id,
      Integration_TypeId: this.myform.value.type,
      Company_Id: this.companyselected,
      ApiPath: this.myform.value.apiPath,
      UserName: this.myform.value.userName,
      Password: this.myform.value.password,
      Integration_Status: 1,
      Integration_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Integration_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_DrFirstIntegration_InsertUpdateApiIntegration, this.apiIntegration)
      .subscribe(res => {
        this.alertService.success("Save successful");
        this.resetDetail();
        this.GetApiIntegrationAllData(this.companyselected);
        
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
       
      });
  }
  GetApiIntegrationAllData(companyID:number)
  {
    if(this.myform.value.apiPath!=null&&this.myform.value.userName!=null&&this.myform.value.password!=null)
    {
      //this.alertService.success("Do you want to save changes");
      this.myform.patchValue({
        apiPath:'',
        userName:'',
        password:'',
      });
    }
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetApiIntegrationAllData+companyID)
    .subscribe(res=>{   
      this.apiIntegrationData=res;
      this.ng4LoadingSpinnerService.hide();
    },error=>{      
      this.errorMessage=<any>error.message;
      this.alertService.error(this.errorMessage);
      this.ng4LoadingSpinnerService.hide();
    });
  }
  GetApiIntegrationDetails(IntegrationID:number)
  {
    window.scroll(0,0);
    this.ng4LoadingSpinnerService.show();
this.dataservice.get<ApiIntegration>(this.config.Emar_DrFirstIntegration_GetApiIntegrationDetails+IntegrationID)
.subscribe(res=>{
this.fetchDetails(res);
this.ng4LoadingSpinnerService.hide();
},error=>{
  this.errorMessage=<any>error.message;
  this.alertService.error(this.errorMessage);
  this.ng4LoadingSpinnerService.hide();
});
  }
  resetDetail()
  {
this.Integration_Id=0;
this.myform.reset();
// this.myform.patchValue({
//   apiPath:'',
//   userName:'',
//   password:'',
//   type:''
// });
  }
  fetchDetails(res:ApiIntegration)
  {
    this.Integration_Id=res.Integration_Id;
this.myform.patchValue({
  apiPath:res.ApiPath,
  userName:res.UserName,
  password:res.Password,
  type:res.Integration_TypeId
});
  }
}
