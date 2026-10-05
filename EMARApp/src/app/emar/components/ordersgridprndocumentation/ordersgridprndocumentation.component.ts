import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services/index';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { PRNDataSave } from '../../../models/emar.model';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';
@Component({
  selector: 'app-ordersgridprndocumentation',
  templateUrl: './ordersgridprndocumentation.component.html',
  styleUrls: ['./ordersgridprndocumentation.component.css']
})
export class OrdersgridprndocumentationComponent implements OnInit {
  @Input() orderGridFilter:any;
  @Output() leaveRequestResult: EventEmitter<any> = new EventEmitter();
  public template;
  private filterConfigs: any = [];
  PRNDataSaveObj: PRNDataSave[] = [];
  public userId: number = this.persistanceService.get(this.config.loggedInUserKey);
  public stationId: number = 0;
  public searchText:string="";
  gridPagination = this.config.gridPagination;
  p: number = 1;
  pageConfig = {};
  textform:FormGroup;
  public PRNData: any[] = [];
  @ViewChild('inputFocus') inputFocus:ElementRef

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    private persistanceService: PersistanceService,public activeDefaultModal: NgbActiveModal,private dateFormatPipe: CustomdatePipe, public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.textform=new FormGroup({
      text:new FormControl('',Validators.required),
    });
    this.filterConfigs=this.orderGridFilter;
    this.getPRNData();
    this.userActivity();
  }
  }
  else
  this.persistanceService.redirectToHomePage();
  }
  ngAfterViewInit(){
    setTimeout(() => {
      this.inputFocus.nativeElement.focus()
    }, 500);
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.PRNDocumentation,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  getPRNData(resultEmit?:any): any{
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_GetPRNDetailsByfilter ,this.filterConfigs)
    .subscribe(res => {
      this.PRNData = res;
      if(resultEmit!=undefined)
        {
          this.leaveRequestResult.emit({ responce: 1, count:  this.PRNData.length });
        }
      this.ng4LoadingSpinnerService.hide();
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  SavePRNData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_Emar_InsertPRNData, this.PRNDataSaveObj)
      .subscribe(res => {
        this.PRNDataSaveObj = [];
        if(res==1)
        {
        this.getPRNData(1);
        }
        else 
        {
          this.leaveRequestResult.emit({ responce: 2, count: this.PRNData.length });
        }
        this.ng4LoadingSpinnerService.hide();

      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    this.textform.reset();
  }
  UpdateArray(AdminId: number, Value: string) {
    if (Value != "") {
      let prnObj = new PRNDataSave();
      prnObj.DrugAdminister_Id = AdminId;
      prnObj.PRNComment = Value;
      prnObj.PRNCommentBy = this.persistanceService.get(this.config.loggedInUserKey);
      prnObj.PRNCommentOn = this.dateFormatPipe.dateWithTime(new Date());
      let index = this.PRNDataSaveObj.findIndex(cs => cs.DrugAdminister_Id == AdminId);
      if (index >= 0) {
        this.PRNDataSaveObj.splice(index, 1);
        this.PRNDataSaveObj.push(prnObj);
      }
      else
        this.PRNDataSaveObj.push(prnObj);
    }
    else if (Value == "") {
      let index = this.PRNDataSaveObj.findIndex(cs => cs.DrugAdminister_Id == AdminId);
      this.PRNDataSaveObj.splice(index, 1);
    }
  }
}
