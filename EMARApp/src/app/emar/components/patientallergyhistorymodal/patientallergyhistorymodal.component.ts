import { Component, OnInit,Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { DemographicInfo } from '../../../models/residentdemographic.model';

@Component({
  selector: 'app-patientallergyhistorymodal',
  templateUrl: './patientallergyhistorymodal.component.html',
  styleUrls: ['./patientallergyhistorymodal.component.css']
})
export class PatientallergyhistorymodalComponent implements OnInit {
  auditTable: any;
  @Input() selectedResident: any;
  @Input() allergyID: any;
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;
  patientAllergyId:any;
  public allergy:any;
  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.patientAllergyId=this.allergyID;
    this.residentId = this.selectedResident;
    this.getPatientAllergyHistoryById();
    this.getDemographicInfoData();
    this.getAllergiesbyAllergiesId(this.patientAllergyId);
  }
getPatientAllergyHistoryById()
{
  this.auditTable = {
    "tableName": "AllergyInfo",
    "recordId": this.patientAllergyId
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
getAllergiesbyAllergiesId(ID: number) {
  this.ng4LoadingSpinnerService.show();
  this.dataservice.get<any>(this.config.Emar_Allgeries_GetAllgeriesById + ID)
    .subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      this.allergy=res.ClassDrug_Name;
    },
      error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });

}
}
