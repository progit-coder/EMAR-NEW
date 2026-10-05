import { Component, OnInit, SimpleChanges, Input,Output,EventEmitter } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { ResidentAdmitDischarge } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VisitinfohistorymodalComponent }from '../visitinfohistorymodal/visitinfohistorymodal.component';
import { VistinfoeditmodalComponent }from '../vistinfoeditmodal/vistinfoeditmodal.component';
import { TransferresidentmodalComponent} from '../transferresidentmodal/transferresidentmodal.component';
import { UpdateadmitvisitinfoComponent} from '../updateadmitvisitinfo/updateadmitvisitinfo.component';
import { DischargeresidentmodalComponent } from '../dischargeresidentmodal/dischargeresidentmodal.component';
@Component({
  selector: 'app-census',
  templateUrl: './census.component.html',
  styleUrls: ['./census.component.css'],
  providers: [DataService, APIConfiguration]
})
export class CensusComponent implements OnInit {
  public residentcensus: ResidentAdmitDischarge[];
  @Output() patientId: EventEmitter<any> = new EventEmitter();
  public residentId: number;
  public template;
  public visitStatus: number;
  @Input() selectedResident: any;
  pageConfig = {};
  transferPageConfig={};
  dischargePageConfig={};
  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sharedService: SharedService, private modalService: NgbModal) {
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes.selectedResident.currentValue != 0 && changes.selectedResident.currentValue != undefined) {
      this.residentId = this.selectedResident.residentId;;
      this.template = this.dataservice.template;
      this.ng4LoadingSpinnerService.show();
      this.getResidentAdmitDischargeData();
    }
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("AdmitVisitInfo");
    this.transferPageConfig = this.persistanceService.getPermissionsByScreen("Transfer");
    this.dischargePageConfig = this.persistanceService.getPermissionsByScreen("Discharge");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        if (this.transferPageConfig == undefined) {
          this.transferPageConfig=0;
        }
        if (this.dischargePageConfig == undefined) {
          this.dischargePageConfig=0;
        }
    this.template = this.dataservice.template;
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    if (this.residentId != 0 && this.residentId != undefined) {
      this.ng4LoadingSpinnerService.show();
      this.getResidentAdmitDischargeData();
    }
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.AdmitVisitInfo, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getHistoryById(visitId: number) {
    const modalRef = this.modalService.open(VisitinfohistorymodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.visitId = visitId;
  }
  getResidentAdmitVisitInfoByvisit_Id(visitId: number) {
    const modalRef = this.modalService.open(UpdateadmitvisitinfoComponent, { size: 'lg', windowClass: 'custom-class' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.PvisitId=visitId;
    modalRef.componentInstance.visitInfoData.subscribe((receivedResult) => {
      if (receivedResult.approval == 1) {
          if (receivedResult.result == 1)
          {
          this.alertService.success("Resident transferred, awaiting admin approvall.");
          }
          else if (receivedResult.result == 2)
          {
            this.alertService.success("Resident discharged, awaiting admin approval.");
          }
          else if (receivedResult.result == 3|| receivedResult.result == 4) {
            this.alertService.success("Update successful.");
          }
      }
      else 
      {
        if (receivedResult.result == 1)
        {
          this.alertService.success("Resident transfer successful.");
        }
        else if (receivedResult.result == 2)
        {
          this.alertService.success("Resident discharge successful.");
        }
        else if (receivedResult.result == 3 || receivedResult.result == 4) {
          this.alertService.success("Update successful.");
          this.patientId.emit(this.residentId);
        }
      }
      this.getResidentAdmitDischargeData();
      modalRef.close();
    });
  }
  getResidentAdmitDischargeData() {
    this.dataservice.get<ResidentAdmitDischarge[]>(this.config.Emar_ResidentDemographic_GetResidentAdmitDischargeData + this.residentId)
      .subscribe(res => {
        this.residentcensus = res;
      }, error => this.alertService.error(error.message));
  }
  residentTransferByvisit_Id(pVisitId)
  {
    const modalRef = this.modalService.open(TransferresidentmodalComponent, { size: 'lg', windowClass: 'custom-class' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.PvisitId=pVisitId;
     modalRef.componentInstance.transferResult.subscribe((receivedResult) => {
      if (receivedResult.approval == 1) {
        if (receivedResult.result == 1)
        {
        this.alertService.success("Resident transferred, awaiting admin approvall.");
        //this.patientId.emit(this.residentId);
        }
        else if (receivedResult.result == 2)
        {
          this.alertService.success("Resident discharged, awaiting admin approval.");
        }
        else if (receivedResult.result == 3) {
          this.alertService.success("Update successful.");
        }
        else if (receivedResult.result == 4) {
          this.alertService.success("Resident Transferred.");
          this.patientId.emit(this.residentId);
        }
    }
    else 
    {
      if (receivedResult.result == 1)
      {
        this.alertService.success("Resident transfer successful.");
        this.patientId.emit(this.residentId);
      }
      else if (receivedResult.result == 2)
      {
        this.alertService.success("Resident discharge successful.");
      }
      else if (receivedResult.result == 3) {
        this.alertService.success("Update successful.");
      }
      else if (receivedResult.result == 4) {
        this.alertService.success("Resident Transferred.");
      }
    }
    this.getResidentAdmitDischargeData();
    modalRef.close();
  });
  }
  residentDischargeByvisit_Id(pVisitId:any)
  {
    const modalRef = this.modalService.open(DischargeresidentmodalComponent, { size: 'lg', windowClass: 'custom-class' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.PvisitId=pVisitId;
     modalRef.componentInstance.dischargeResult.subscribe((receivedResult) => {

      if (receivedResult.approval == 1) {
        if (receivedResult.result == 1)
        {
        this.alertService.success("Resident transferred, awaiting admin approvall.");
        this.patientId.emit(this.residentId);
        }
        else if (receivedResult.result == 2)
        {
          this.alertService.success("Resident discharged, awaiting admin approval.");
          this.patientId.emit(this.residentId);
        }
        else if (receivedResult.result == 3) {
          this.alertService.success("Update successful.");
        }
    }
    else 
    {
      if (receivedResult.result == 1)
      {
        this.alertService.success("Resident transfer successful.");
        this.patientId.emit(this.residentId);
      }
      else if (receivedResult.result == 2)
      {
        this.alertService.success("Resident discharge successful.");
        this.patientId.emit(this.residentId);
      }
      else if (receivedResult.result == 3) {
        this.alertService.success("Update successful.");
      }
    }
    this.getResidentAdmitDischargeData();
    modalRef.close();
  });
  }
}

