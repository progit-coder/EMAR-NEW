import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import {  DefaultScreen } from '../../../models/role.model';
import { AlertService } from '../../../_services/index';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SharedService } from '../../../services/shared/shared.service';
import { NurseStation, Room, Floor, Bed, Wing } from './../../../models/facility.model';
import { AdmitInfo } from '../../../models/residentdemographic.model';

@Component({
  selector: 'app-updatevisitinfo',
  templateUrl: './updatevisitinfo.component.html',
  styleUrls: ['./updatevisitinfo.component.css']
})
export class UpdatevisitinfoComponent implements OnInit {

  @Output() updateVisitInfo: EventEmitter<any> = new EventEmitter();
  myform: FormGroup;
  private residentId: number;
  public floors: Floor[];
  public room: Room[];
  public wing: Wing[];
  public bed: Bed[];
  public admitvisitinfoObj: AdmitInfo;
  public demographicInfoData:any;
  public updateVisitObj: any;

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    private persistanceService: PersistanceService,public activeDefaultModal: NgbActiveModal,private sharedService: SharedService,) { }

  ngOnInit() {
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    this.myform = new FormGroup({
      Room: new FormControl('0'),
      Bed: new FormControl('0'),
      Floor: new FormControl('0'),
      Wing: new FormControl('0'),
    });
    this.getFloorWingDrop();
  }

  getFloorWingDrop() {

    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetCompanyToBedData + userId)
      .subscribe((res: any) => {
        this.room = res.Rooms;
        this.floors = res.Floors;
        this.bed = res.Beds;
        this.wing = res.Wings;
        this.getDemographicInfoData();
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getDemographicInfoData() {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.demographicInfoData=res;
        this.myform.patchValue({
          Room: res.RoomId,
          Bed: res.BedId,
          Floor: res.FloorId,
          Wing: res.WingId
        });
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  insertResidentAdmitvisitinfodetails() {

    this.updateVisitObj =
      {
        // PVisit_Id: this.PVisit_Id,
        Patient_Id: this.residentId,
        Room: this.myform.value.Room == '0' ? null : this.myform.value.Room,
        Bed: this.myform.value.Bed == '0' ? null : this.myform.value.Bed,
        Floor: this.myform.value.Floor == '0' ? null : this.myform.value.Floor,
        Wing: this.myform.value.Wing == '0' ? null : this.myform.value.Wing
      }

    //ToDo: Create one more API which update room,bed,floor,wing not through admin approval.

    this.dataservice.post(this.config.Emar_Demographic_UpdateResidentInfo, this.updateVisitObj)
      .subscribe(res => {
        if (res == 1) {
          this.updateVisitInfo.emit(1);
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
