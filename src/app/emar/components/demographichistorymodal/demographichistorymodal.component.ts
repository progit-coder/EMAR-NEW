import { Component, OnInit,Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SharedService } from '../../../services/shared/shared.service';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { DemographicInfo } from '../../../models/residentdemographic.model';
@Component({
  selector: 'app-demographichistorymodal',
  templateUrl: './demographichistorymodal.component.html',
  styleUrls: ['./demographichistorymodal.component.css']
})
export class DemographichistorymodalComponent implements OnInit {

  @Input() historyType: any;
  auditTable: any;
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;
  constructor(public activeDefaultModal: NgbActiveModal,private sharedService: SharedService,private dataservice: DataService, public config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService ) { }

  ngOnInit() {
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    this.getDemographicHistoryByPId();
    this.getDemographicInfoData();
  }
  getDemographicHistoryByPId() {
    this.auditTable = {
      "tableName":this.historyType==1?"Demographics": "AllResidentChanges",
      "recordId": this.residentId
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
