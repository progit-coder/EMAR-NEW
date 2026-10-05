import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Bed } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-bedmaster',
  templateUrl: './bedmaster.component.html',
  styleUrls: ['./bedmaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class BedmasterComponent implements OnInit {
  myform: FormGroup;
  private bed = new Bed();
  private bedId: number = 0;
  public template;
  public beds: Bed[] = [];
  errorMessage: string;
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  searchText: string = "";
  p: number = 1;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  CheckAll: boolean = false;
  public bedList: any[] = [];
  public inactivecheckbox: boolean = false;
  gridPagination = this.config.gridPagination;
  pageConfig: {};
  constructor(private dataservice: DataService,private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

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
      bedname: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      bedcode: new FormControl('', [Validators.required, Validators.maxLength(20),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1')

    });
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.BedMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }
  getHistoryById(bedId: number) {
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
      Bed_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Bed_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertBed, this.bed)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.getBeds();
        this.inactivecheckbox = false;
      }, error => {
        this.errorMessage = <any>error.message;
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
  }
  getBeds() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllBeds)
      .subscribe(res => {
        this.beds = res;
        this.bedList = this.beds.filter(b => b.Bed_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });

  }
  updateBedStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateBedStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getBeds();
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
      this.bedList.forEach(element => {
        element.Bed_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Bed_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
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
      item.Bed_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Bed_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Room_Id == item.Room_Id);
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
    if (value == true) {
      this.bedList = this.beds.filter(b => b.Bed_Status == 0);
    }
    if (value == false) {
      this.getBeds();
    }
  }
  getBedDetailsByID(bedId: number) {
    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    this.dataservice.get<Bed>(this.config.Emar_Facility_GetBedDetailsById + bedId)
      .subscribe(res => {
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  fetchData(res: Bed) {
    this.myform.patchValue({
      bedname: res.Bed_Name,
      bedcode: res.Bed_Code,
      status: res.Bed_Status
    });
    this.bedId = res.Bed_Id;
  }

  checkBedName(): any {
    let Bed_Name = this.myform.value.bedname;
    let result = this.beds.find(x => x.Bed_Name.replace(/\s/g, '').toLowerCase() === Bed_Name.toLowerCase().replace(/\s/g, ''));
    if (result) {
      this.alertService.error("Bed Name Already Exists");
      this.myform.patchValue({
        bedname: ''
      });
    }
    else { }
  }
}
