import { Component, OnInit, ChangeDetectorRef, Output,EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Floor } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Facility } from '../../../models/facility.model';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
//import { EventEmitter } from 'events';
@Component({
  selector: 'app-bedconfigfloor',
  templateUrl: './bedconfigfloor.component.html',
  styleUrls: ['./bedconfigfloor.component.css'],
  providers: [DataService, APIConfiguration]
})
export class BedconfigfloorComponent implements OnInit {
  @ViewChild("closeAddFloorModal") closeFloorModal:ElementRef;
  myform: FormGroup;
  public template;
  // public facilities: Facility[];
  searchText:string="";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  private floor = new Floor();
  private floorId: number = 0;
  public floorMaster: any[]=[];
  errorMessage: string;
  TableName="Floor";
  auditTable:any;
  public modalHistoryIsOpen : boolean = false;
  // selectedFacility:number;
  pageConfig: {};
  pageconfiguration = { "Field1": true, "Field2": false };
 
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService,private persistanceService: PersistanceService) { }
  @Output()
  NeWFloor=new EventEmitter();
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("FloorMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getFloorMaster();
    // this.getFacilityMaster();
    this.myform = new FormGroup({
      // facilityName: new FormControl('', Validators.required),
      floorNumber: new FormControl('', [Validators.required, Validators.maxLength(50),, Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1'),
    });
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }
  getHistoryById(floorId: number)
  {
    this.auditTable = {
      "tableName": "Floor",
      "recordId": floorId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
     this.modalHistoryIsOpen = false;
  } 
  insertFloor() {
    this.ng4LoadingSpinnerService.show();
    this.floor = {
      Floor_Id: this.floorId,
      Floor_Name: this.myform.value.floorNumber,
      // Facility_Id: this.myform.value.facilityName,
      Floor_Status: (this.myform.value.status == true ? 1 : 0),
      Floor_CreatedBy:  this.persistanceService.get(this.config.loggedInUserKey),
      Floor_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertFloor, this.floor)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.NeWFloor.emit();
        this.closeFloorModal.nativeElement.click();
        this.getFloorMaster();
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
    //   facilityName: '',
    //   floorNumber: '',
    //   status: '1'
    // });
     this.floorId = 0;
     this.NeWFloor.emit();
  }
  getFloorMaster() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllFloors)
      .subscribe(res => {
      this.floorMaster = res;
      
      this.ng4LoadingSpinnerService.hide();
      }, error => {
      this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)
      this.ng4LoadingSpinnerService.hide();
      });

  }
  // getFacilityMaster() {
  //   this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetFacilityMaster)
  //     .subscribe(res => this.facilities = res, error => this.errorMessage = <any>error);
  // }
  getFloorDetailsByID(floorId: number) {
    window.scroll(0, 0);
    this.dataservice.get<Floor>(this.config.Emar_Facility_GetFloorDetailsById + floorId)
      .subscribe(res => this.fetchData(res),     
      error => {this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)});
  }
  fetchData(res: Floor) {
    this.myform.patchValue({
      // facilityName: res.Facility_Id,
      floorNumber: res.Floor_Name,
      status: res.Floor_Status
    });
    this.floorId = res.Floor_Id;
  }

  checkFloorNumber():any{
   
    let Floor_Number=this.myform.value.floorNumber;
    let result=this.floorMaster.find(x => x.Floor_Name.replace(/\s/g, '').toLowerCase() === Floor_Number.toLowerCase().replace(/\s/g, ''));
    if(result)
    {
      this.alertService.warn("Floor Number Already Exists");
      this.myform.patchValue({
        floorNumber:''
      });
    }
    else{}
  }

}
