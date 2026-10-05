import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Gender, MaritalStatus, Suffix } from '../../../models/common.model';
import { DemographicInfo, ResidentDemographicMaster } from '../../../models/residentdemographic.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { SharedService } from '../../../services/shared/shared.service';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ResidentreactivateComponent } from '../residentreactivate/residentreactivate.component';
import { ResidentonleaveComponent } from '../residentonleave/residentonleave.component';
import { DemographicinfoComponent } from '../demographicinfo/demographicinfo.component';
import { DemographichistorymodalComponent } from '../demographichistorymodal/demographichistorymodal.component';
import { DemographiceditmodalComponent } from '../demographiceditmodal/demographiceditmodal.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-demographicinformation',
  templateUrl: './demographicinformation.component.html',
  styleUrls: ['./demographicinformation.component.css'],
  providers: [DataService, APIConfiguration]
})
export class DemographicinformationComponent implements OnInit {
  public demographicInfoData = {} as DemographicInfo;
  public residentId: number;
  public template;
  @Input() selectedResident: any;
  @Output() newResidentId: EventEmitter<any> = new EventEmitter();
  pageConfig = {};
  modalOption: NgbModalOptions = {};
  NewResidentpageConfig ={};

  constructor(private dataservice: DataService, private config: APIConfiguration, private route: Router,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private sharedService: SharedService, private persistanceService: PersistanceService, private modalService: NgbModal) {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.selectedResident.currentValue != 0 && changes.selectedResident.currentValue != undefined) {
      this.demographicInfoData = this.selectedResident;
      this.residentId = this.demographicInfoData.Patient_Id;
      this.template = this.dataservice.template;
      if (this.residentId != 0 && this.residentId != undefined) {
        this.getDemographicInfoData();
      }
    }
  }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DemographicInformation");
    this.NewResidentpageConfig = this.persistanceService.getPermissionsByScreen("NewResident");
    if(this.NewResidentpageConfig ==undefined)
    {
      this.NewResidentpageConfig =0;
    }
    this.template = this.dataservice.template;
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    if (this.residentId != 0 && this.residentId != undefined) {
      this.getDemographicInfoData();
    }
  }

  getDemographicInfoData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.demographicInfoData = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  insertnewResident() {
    this.modalOption.size = 'lg';

    const modalRef = this.modalService.open(DemographicinfoComponent, this.modalOption);
    modalRef.componentInstance.title = 'New Resident Details';
    let residentData = {
      "selectedPatientId": this.residentId,
      "selectedNurseStationId": this.demographicInfoData.NursingStationId,
      "selectedFacilityId": this.demographicInfoData.FacilityId
    }
    modalRef.componentInstance.resdata = residentData;
    modalRef.componentInstance.Result.subscribe((receivedResult) => {
      if (receivedResult > 0) {
        this.alertService.success("New Resident Added Successfully");
        modalRef.close();
        //this.sharedService.changePatientId(receivedResult);
        //this.route.navigate(['/home/residentinformation'])        
        this.newResidentId.emit(receivedResult);
      }
    })
  }
  updateLeaveStatus(patientId:number,visitId, visitStatus: number) {
    if (visitStatus == 3) {
      const modalRef = this.modalService.open(ResidentreactivateComponent);
      let leaveChanges = {
        "visitId": visitId,
        "isOnLeave": 1
      }
      modalRef.componentInstance.leaveChanges = leaveChanges;
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 1) {
          this.alertService.success("Resident reactivated successfully");
          modalRef.close();
          //this.getDemographicInfoData();
          this.newResidentId.emit(patientId);
        }
        else if (receivedResult == 0 || 2) {
          this.alertService.error("Something went wrong");
          modalRef.close();
        }
        else
          modalRef.close();
      })
    }
    else {
      //Open modal popup to give reason and submit
      const modalRef = this.modalService.open(ResidentonleaveComponent);
      let leaveChanges = {
        "visitId": visitId,
        "isOnLeave": 0
      }
      modalRef.componentInstance.leaveChanges = leaveChanges;
      modalRef.componentInstance.leaveRequestResult.subscribe((receivedResult) => {
        if (receivedResult == 1) {
          this.alertService.success("Temporary absence added successfully");
          modalRef.close();
          //this.getDemographicInfoData();
          this.newResidentId.emit(patientId);
        }
        else
          modalRef.close();
      })
    }
  }
  getHistoryById() {
    const modalRef = this.modalService.open(DemographichistorymodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.historyType =1;
  }
  getAllResidentHistoryById()
  {
    const modalRef = this.modalService.open(DemographichistorymodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.historyType =2;
  }
  getResidentDetailsById() {
    const modalRef = this.modalService.open(DemographiceditmodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedResident = this.residentId;
    modalRef.componentInstance.residetData.subscribe((receivedResult) => {
      if (receivedResult == 1) {
        this.alertService.success("Save successful, waiting for admin approval");
      }
      else if (receivedResult == 0) {
        this.alertService.success("Save successful");
        this.getDemographicInfoData();
      }
      modalRef.close();
    });
  }
}
