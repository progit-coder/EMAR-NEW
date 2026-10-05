import { Component, OnInit, ChangeDetectorRef, Output,EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Bed } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import {AlertService} from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
//import { EventEmitter } from 'protractor';

@Component({
  selector: 'app-bedconfigbedmaster',
  templateUrl: './bedconfigbedmaster.component.html',
  styleUrls: ['./bedconfigbedmaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class BedconfigbedmasterComponent implements OnInit {
  @ViewChild("closeAddBedModal") closeBedModal:ElementRef;
  myform: FormGroup;
  private bed = new Bed();
  private bedId: number = 0;
  public template;
  public beds: Bed[]=[];
  errorMessage: string;
  auditTable:any;
  public modalHistoryIsOpen : boolean = false;
  searchText:string="";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  @Output()
  NewBed=new EventEmitter();
  pageConfig: {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService,private persistanceService: PersistanceService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("BedMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getBeds();
    this.myform = new FormGroup({
      bedname: new FormControl('', [Validators.required,Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      bedcode: new FormControl('', [Validators.required, Validators.maxLength(20),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
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
  getHistoryById(bedId: number)
  {
    this.auditTable = {
      "tableName": "Bed",
      "recordId": bedId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
     this.modalHistoryIsOpen = false;
  } 
  insertBed() {
    this.ng4LoadingSpinnerService.show();
    this.bed = {
      Bed_Id: this.bedId,
      Bed_Code: this.myform.value.bedcode,
      Bed_Name: this.myform.value.bedname,
      Bed_Status: (this.myform.value.status == true ? 1 : 0),
      Bed_CreatedBy:this.persistanceService.get(this.config.loggedInUserKey),
      Bed_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertBed, this.bed)
      .subscribe(res =>{
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.NewBed.emit();
        this.closeBedModal.nativeElement.click();
        this.getBeds();
      },error => {this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
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
    //   bedname: '',
    //   bedcode: '',
    //   status: '1'
    // });
    this.bedId = 0;
    this.NewBed.emit();
  }
  getBeds() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllBeds)
      .subscribe(res => {
      this.beds = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
      this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getBedDetailsByID(bedId: number) {
    window.scroll(0, 0);
    this.dataservice.get<Bed>(this.config.Emar_Facility_GetBedDetailsById + bedId)
      .subscribe(res => this.fetchData(res), error =>{ this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)});
  }
  fetchData(res: Bed) {
    this.myform.patchValue({
      bedname: res.Bed_Name,
      bedcode: res.Bed_Code,
      status: res.Bed_Status
    });
    this.bedId = res.Bed_Id;
  }

  checkBedName():any{
    let Bed_Name=this.myform.value.bedname;
    let result=this.beds.find(x => x.Bed_Name.replace(/\s/g, '').toLowerCase() === Bed_Name.toLowerCase().replace(/\s/g, ''));
    if(result)
    {
      this.alertService.warn("Bed Name Already Exists");
      this.myform.patchValue({
        bedname:''
      });
    }
    else{}
  }
}
