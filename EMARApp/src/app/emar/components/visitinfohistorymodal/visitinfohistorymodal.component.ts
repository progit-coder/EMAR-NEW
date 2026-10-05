import { Component, OnInit,Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { DemographicInfo } from '../../../models/residentdemographic.model';

@Component({
  selector: 'app-visitinfohistorymodal',
  templateUrl: './visitinfohistorymodal.component.html',
  styleUrls: ['./visitinfohistorymodal.component.css']
})
export class VisitinfohistorymodalComponent implements OnInit {
  auditTable: any;
  @Input() visitId: any;
  @Input() selectedResident: any;
  patientVisitId:any;
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.patientVisitId=this.visitId;
    this.residentId = this.selectedResident;
    this.getVisitInfoHistoryById();
    this.getDemographicInfoData();
  }
  getVisitInfoHistoryById() {
    this.auditTable = {
      "tableName": "VisitInfo",
      "recordId": this.patientVisitId
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
}
