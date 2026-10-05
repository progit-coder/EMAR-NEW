import { Component, OnInit, ChangeDetectorRef, Output,EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Wing } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services/alert.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
//import { EventEmitter } from 'events';

@Component({
  selector: 'app-bedconfigwing',
  templateUrl: './bedconfigwing.component.html',
  styleUrls: ['./bedconfigwing.component.css'],
  providers: [DataService, APIConfiguration]
})
export class BedconfigwingComponent implements OnInit {
  @ViewChild("closeAddWingModal") closeWingModal:ElementRef;
  myform: FormGroup;
  public template;
  private wing = new Wing();
  private wingId: number = 0;
  public wings: Wing[]=[];
  errorMessage: string;
  TableName="Wing";
  auditTable:any;
  public modalHistoryIsOpen : boolean = false;
  searchText:string="";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  @Output()
  NewWing=new EventEmitter();
  pageConfig: {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService,private persistanceService: PersistanceService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("WingMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getWings();
    this.myform = new FormGroup({
      wingname: new FormControl('', [Validators.required,Validators.maxLength(10), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1')

    });
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }
  getHistoryById(wingId: number)
  {
    this.auditTable = {
      "tableName": "Wing",
      "recordId": wingId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
     this.modalHistoryIsOpen = false;
  } 
  insertWing() {
    this.ng4LoadingSpinnerService.show();
    this.wing = {
      Wing_Id: this.wingId,
      Wing_Desc: this.myform.value.wingname,
      Wing_Status: (this.myform.value.status == true ? 1 : 0),
      Wing_CreatedBy:  this.persistanceService.get(this.config.loggedInUserKey),
      Wing_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertWing, this.wing)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.NewWing.emit();
        this.closeWingModal.nativeElement.click();
        this.getWings();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage)
          this.ng4LoadingSpinnerService.hide();
        });
    this.resetScreen();
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   wingname: '',
    //   status: '1'
    // });
    // this.wingId = 0;
    this.NewWing.emit();
  }
  getWings() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllWings)
      .subscribe(res => {
        this.wings = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage)
          this.ng4LoadingSpinnerService.hide();
        });

  }
  getWingDetailsByID(wingId: number) {
    window.scroll(0, 0);
    this.dataservice.get<Wing>(this.config.Emar_Facility_GetWingDetailsById + wingId)
      .subscribe(res => this.fetchData(res), error => {this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)});
  }
  fetchData(res: Wing) {
    this.myform.patchValue({
      wingname: res.Wing_Desc,
      status: res.Wing_Status
    });
    this.wingId = res.Wing_Id;
  }

  checkWingName():any{
    let Wing_Name=this.myform.value.wingname;
    let result=this.wings.find(x => x.Wing_Desc.replace(/\s/g, '').toLowerCase() === Wing_Name.toLowerCase().replace(/\s/g, ''));
    if(result)
    {
      this.alertService.warn("Wing Name Already Exists");
      this.myform.patchValue({
        wingname:''
      });
    }
    else
    {}

  }

}
