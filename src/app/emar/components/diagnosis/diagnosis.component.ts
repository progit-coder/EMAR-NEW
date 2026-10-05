import { ICD10 } from './../../../models/allergyandicd.model';
import { Component, OnInit, SimpleChange, Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators, RequiredValidator } from '@angular/forms';
import { Diagnosis } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { AlertService } from '../../../_services';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Subject, Observable } from 'rxjs';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PatientdiagnosiseditmodalComponent } from '../patientdiagnosiseditmodal/patientdiagnosiseditmodal.component';
import { PatientdiagnosishistorymodalComponent } from '../patientdiagnosishistorymodal/patientdiagnosishistorymodal.component';
@Component({
  selector: 'app-diagnosis',
  templateUrl: './diagnosis.component.html',
  styleUrls: ['./diagnosis.component.css'],
  providers: [DataService, APIConfiguration]
})
export class DiagnosisComponent implements OnInit {
  public diagnosis: any[];
  residentId: number;
  template: string;
  icd10Id: number = 0;
  @Input() selectedResident: any;
  pageConfig = {};
  pDiagnosisId: number;
  residentStatus: number;
  modalDiagnosisIsOpen: boolean;

  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService,
    private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, private modalService: NgbModal) {

  }
  ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
    if (changes.selectedResident && changes.selectedResident.currentValue != undefined) {
      this.residentId = this.selectedResident.residentId;
      this.residentStatus = this.selectedResident.residentStatus;
      this.template = this.dataservice.template;
      this.getResidentDiagnosis();
    }
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Diagnosis");
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    this.template = this.dataservice.template;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.getResidentDiagnosis();
    }

    this.userActivity();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Diagnosis, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }

  getHistoryById(diagnosisId: number) {
    const modalRef = this.modalService.open(PatientdiagnosishistorymodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.diagnosisId = diagnosisId;
  }
  getResidentDiagnosis() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographicDiagnosis_GetResDiagnosis + this.residentId)
      .subscribe(res => {
        this.diagnosis = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    }
    getDiagnosissbyDiagnosisId(diagnosisId:any)
    {
      const modalRef = this.modalService.open(PatientdiagnosiseditmodalComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.selectedResident=this.residentId;
      modalRef.componentInstance.DiagnosisId=diagnosisId;
      modalRef.componentInstance.Pstatus=this.residentStatus
      modalRef.componentInstance.diagnosisInfoResult.subscribe((receivedResult) => {
            if (receivedResult == -1)
            {
             this.alertService.error("Diagnosis already recorded");
            }
            else if (receivedResult == 0)
            {
              this.alertService.success("Save successful, awaiting admin approval");
            }
            else if (receivedResult == 1) {
              this.alertService.success("Save successful");
            }
        this.getResidentDiagnosis();
        modalRef.close();
      });
    }
    
  closeDiagnosisModel() {
    this.modalDiagnosisIsOpen = false;
    this.pDiagnosisId = 0;
  }
  modalDiagnosisOpen(diagnosisId: any) {
    this.pDiagnosisId = diagnosisId;
    this.modalDiagnosisIsOpen = true;
  }

  RemoveDiagnosis() {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_RemoveResidentDiagnosis +this.pDiagnosisId)
      .subscribe(res => {
        this.modalDiagnosisIsOpen = false;
        this.pDiagnosisId = 0;
        if (res == 1) {
          this.alertService.success("Diagnosis removal successfull");
          this.getResidentDiagnosis();
        }
        else if (res == 0)
          this.alertService.error("Diagnosis removal failed");
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
