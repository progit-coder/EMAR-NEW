import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Floor } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Facility } from '../../../models/facility.model';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-floormaster',
  templateUrl: './floormaster.component.html',
  styleUrls: ['./floormaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class FloormasterComponent implements OnInit {
  myform: FormGroup;
  public template;
  // public facilities: Facility[];
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  private floor = new Floor();
  private floorId: number = 0;
  public floorMaster: any[] = [];
  errorMessage: string;
  TableName = "Floor";
  auditTable: any;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  CheckAll: boolean = false;
  public floorList: any[] = [];
  public inactivecheckbox: boolean = false;
  public modalHistoryIsOpen: boolean = false;
  // selectedFacility:number;
  pageConfig: {};

  pageconfiguration = { "Field1": true, "Field2": false };
  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

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
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }
  getHistoryById(floorId: number) {
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
      Floor_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Floor_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertFloor, this.floor)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.getFloorMaster();
        this.inactivecheckbox = false;
      },
        error => {
          this.alertService.error(error.message)
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
  }
  getFloorMaster() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllFloors)
      .subscribe(res => {
        this.floorMaster = res;
        this.floorList = this.floorMaster.filter(fl => fl.Floor_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  updateFloorStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select data to update status.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateFloorStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getFloorMaster();
            this.selectedRecords = [];
          }
          else if (res == 0) {
            this.alertService.error("Status Updated Failed.");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.floorList.forEach(element => {
        element.Floor_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Floor_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.UpdateStatus = true;
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.UpdateStatus = false;
      item.Floor_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Floor_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Floor_Id == item.Floor_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.floorList = this.floorMaster.filter(fl => fl.Floor_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getFloorMaster();
    }
  }
  // getFacilityMaster() {
  //   this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetFacilityMaster)
  //     .subscribe(res => this.facilities = res, error => this.errorMessage = <any>error);
  // }
  getFloorDetailsByID(floorId: number) {
    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    this.dataservice.get<Floor>(this.config.Emar_Facility_GetFloorDetailsById + floorId)
      .subscribe(res => {
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
  fetchData(res: Floor) {
    this.myform.patchValue({
      // facilityName: res.Facility_Id,
      floorNumber: res.Floor_Name,
      status: res.Floor_Status
    });
    this.floorId = res.Floor_Id;
  }

  checkFloorNumber(): any {

    let Floor_Number = this.myform.value.floorNumber;
    let result = this.floorMaster.find(x => x.Floor_Name.replace(/\s/g, '').toLowerCase() === Floor_Number.toLowerCase().replace(/\s/g, ''));
    if (result) {
      this.alertService.error("Floor Number Already Exists");
      this.myform.patchValue({
        floorNumber: ''
      });
    }
    else { }
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.FloorMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
}
