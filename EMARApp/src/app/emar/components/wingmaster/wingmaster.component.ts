import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Wing } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services/alert.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-wingmaster',
  templateUrl: './wingmaster.component.html',
  styleUrls: ['./wingmaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class WingmasterComponent implements OnInit {
  myform: FormGroup;
  public template;
  private wing = new Wing();
  private wingId: number = 0;
  public wings: Wing[] = [];
  errorMessage: string;
  TableName = "Wing";
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  CheckAll: boolean = false;
  searchText: string = "";
  public wingList: any[] = [];
  public inactivecheckbox: boolean = false;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  pageConfig: {};
  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

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
      wingname: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1')

    });
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.WingMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }
  getHistoryById(wingId: number) {
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
      Wing_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Wing_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertWing, this.wing)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.getWings();
        this.inactivecheckbox = false;
      },
        error => {
          this.alertService.error(error.message);
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
  }
  getWings() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllWings)
      .subscribe(res => {
        this.wings = res;
        this.wingList = this.wings.filter(w => w.Wing_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage)
          this.ng4LoadingSpinnerService.hide();
        });

  }
  updateWingStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateWingStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getWings();
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
      this.wingList.forEach(element => {
        element.Wing_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Wing_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
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
      item.Wing_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Wing_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
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
      this.wingList = this.wings.filter(w => w.Wing_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getWings();
    }
  }
  getWingDetailsByID(wingId: number) {
    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    this.dataservice.get<Wing>(this.config.Emar_Facility_GetWingDetailsById + wingId)
      .subscribe(res => {
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  fetchData(res: Wing) {
    this.myform.patchValue({
      wingname: res.Wing_Desc,
      status: res.Wing_Status
    });
    this.wingId = res.Wing_Id;
  }

  checkWingName(): any {
    let Wing_Name = this.myform.value.wingname;
    let result = this.wings.find(x => x.Wing_Desc.replace(/\s/g, '').toLowerCase() === Wing_Name.toLowerCase().replace(/\s/g, ''));
    if (result) {
      this.alertService.error("Wing Name Already Exists");
      this.myform.patchValue({
        wingname: ''
      });
    }
    else { }

  }
}
