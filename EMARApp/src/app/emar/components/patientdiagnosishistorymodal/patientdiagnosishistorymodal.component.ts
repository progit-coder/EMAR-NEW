import { Component, OnInit,Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { Diagnosis } from '../../../models/residentdemographic.model';
@Component({
  selector: 'app-patientdiagnosishistorymodal',
  templateUrl: './patientdiagnosishistorymodal.component.html',
  styleUrls: ['./patientdiagnosishistorymodal.component.css']
})
export class PatientdiagnosishistorymodalComponent implements OnInit {

  auditTable: any;
  @Input() diagnosisId: any;
  patientDiagnosiId:any;
  @Input() selectedResident: any;
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;
  public diagnosis:any;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.ng4LoadingSpinnerService.show();
    this.patientDiagnosiId=this.diagnosisId;
    this.residentId = this.selectedResident;
    this.getPatientAllergyHistoryById();
    this.getDemographicInfoData();
    this.getDiagnosissbyDiagnosisId(this.patientDiagnosiId);
  }

    getPatientAllergyHistoryById()
    {
      this.auditTable = {
        "tableName": "Diagnosis",
        "recordId": this.patientDiagnosiId
      }
    }
    getDemographicInfoData() {
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
    getDiagnosissbyDiagnosisId(ID: number) {
      this.dataservice.get<Diagnosis>(this.config.Emar_Diagnosis_GetDiagnosisById + ID)
        .subscribe(res => {
          this.diagnosis=res.ICD10_RawFormat;
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
  
    }
}
