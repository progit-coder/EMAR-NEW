import { Component, OnInit, SimpleChanges, Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { LiteralOdersData } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { LiteralOders } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LiteralorderseditmodalComponent }from '../literalorderseditmodal/literalorderseditmodal.component';

@Component({
  selector: 'app-literalorders',
  templateUrl: './literalorders.component.html',
  styleUrls: ['./literalorders.component.css'],
  providers: [DataService, APIConfiguration, AlertService]
})
export class LiteralordersComponent implements OnInit {
  public LiteralordersData: LiteralOdersData[];
  public literalordersObj: LiteralOdersData;
  public literalOrders: LiteralOders[];
  public ordersData: any[] = [];
  displayfield: any = {};
  public residentId: number;
  public template;
  @Input() selectedResident: any;
  TableName = "CommonOrderInfo";

  searchText: string = "";
  orderType:number;
  public modalHistoryIsOpen: boolean = false;

  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private modalService: NgbModal,
    private sharedService: SharedService) {
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes.selectedResident.currentValue != 0 && changes.selectedResident.currentValue != undefined) {
      this.residentId = this.selectedResident;
      this.template = this.dataservice.template;
      this.ng4LoadingSpinnerService.show();
      this.getResidentOrdersById();
    }
  }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    if (this.residentId != 0 && this.residentId != undefined) {
      this.ng4LoadingSpinnerService.show();
      //this.getResidentLiteralOrderData();
      this.getResidentOrdersById();
    }
    this.userActivity();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ResidentGridOrders, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  getResidentOrdersById() {
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographicDiagnosis_GetResidentOrderData + this.residentId)
      .subscribe(res => {
        this.ordersData = res;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getResidentOrdersView(orderType: number, orderId: number, quantityId: number) {
    const modalRef = this.modalService.open(LiteralorderseditmodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.orderType = orderType;
    modalRef.componentInstance.orderId = orderId;
    modalRef.componentInstance.quantityId = quantityId;
  }
}
