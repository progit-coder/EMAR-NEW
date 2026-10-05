import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services/index';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EncodeDrugDemographic, Seventytwohourcheck } from '../../../models/emar.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';
@Component({
  selector: 'app-ordersgrid72hourscheck',
  templateUrl: './ordersgrid72hourscheck.component.html',
  styleUrls: ['./ordersgrid72hourscheck.component.css']
})
export class Ordersgrid72hourscheckComponent implements OnInit {
  @Input() orderGridFilter:any;
  @Output() leaveRequestResult: EventEmitter<any> = new EventEmitter();
  public template;
  public drugInfoData: any = [];
  SeventytwohourcheckObj: EncodeDrugDemographic[] = [];
  userCheckObj: Seventytwohourcheck;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  searchText: string = "";
  public userId: number = this.persistanceService.get(this.config.loggedInUserKey);
  public stationId: number = 0;
  pageConfig = {};
  myform: FormGroup;
  private filterConfigs: any = [];
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
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.myform = new FormGroup({
      text: new FormControl('',Validators.required)
    });
    this.filterConfigs=this.orderGridFilter;
    this.getSeventyTwoHourCheckDetails();
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
    this.sharedService.insertUserActivityDetails(Screens.SeventyTwoHourChecks,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  getSeventyTwoHourCheckDetails(resultEmit?:any) {

    this.dataservice.post(this.config.Emar_GetSeventyTwoHourDetailsByfilter ,this.filterConfigs)
      .subscribe(res => {
        this.drugInfoData = res;
        if(resultEmit!=undefined)
        {
          this.leaveRequestResult.emit({ responce: 1, count:  this.drugInfoData.length });
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  resetScreen() {
    this.myform.reset();
  }
  UpdateArray(DrugAdministerId: number, Value: string) {
    if (Value != "") {
      let svObj = new EncodeDrugDemographic();
      svObj.DrugAdminister_Id = DrugAdministerId;
      svObj.SeventyTwoComment = Value;
      svObj.SeventyTwoCommentBy = this.persistanceService.get(this.config.loggedInUserKey);
      svObj.SeventyTwoCommentOn = this.dateFormatPipe.dateWithTime(new Date());
      let index = this.SeventytwohourcheckObj.findIndex(cs => cs.DrugAdminister_Id == DrugAdministerId);
      if (index >= 0) {
        this.SeventytwohourcheckObj.splice(index, 1);
        this.SeventytwohourcheckObj.push(svObj);
      }
      else
        this.SeventytwohourcheckObj.push(svObj);
    }
    else if (Value == "") {
      let index = this.SeventytwohourcheckObj.findIndex(cs => cs.DrugAdminister_Id == DrugAdministerId);
      this.SeventytwohourcheckObj.splice(index, 1);
    }
  }
  insertChecks() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_InsertUpdateSeventyTwoHourChecks, this.SeventytwohourcheckObj)
      .subscribe(res => {
        this.SeventytwohourcheckObj = [];
        if(res==1)
        {
        this.getSeventyTwoHourCheckDetails(1);
        }
        else 
        {
          this.leaveRequestResult.emit({ responce: 2, count: this.drugInfoData.length });
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    this.resetScreen();
    window.scroll(0,0);
      }
}
