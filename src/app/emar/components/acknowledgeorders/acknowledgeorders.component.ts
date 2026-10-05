import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Router } from '@angular/router';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
@Component({
  selector: 'app-acknowledgeorders',
  templateUrl: './acknowledgeorders.component.html',
  styleUrls: ['./acknowledgeorders.component.css']
})
export class AcknowledgeordersComponent implements OnInit {
  gridPagination = this.config.gridPagination;
  public template;
  myform: FormGroup;
  searchText: string = "";
  pageConfig = {};
  public ackOrders: any[] = [];
  public AcceptButton: boolean = true;
  public RejectButton: boolean = true;
  public CheckAll: boolean = false;
  public modalViewIsOpen: boolean = false;
  public fileViewData: any;
  public fileError: any[] = [];
  public fileAck: string;
  public viewTitle: string = "";
  public userId: number;
  public residents: any[];
  p:any = 1;
  
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService, private router: Router) { }

  ngAfterViewInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("AcknowledgeOrders");
  }
  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey)
    this.userActivity();
    this.getResidentDropData();
    this.getAllAckOrders(this.userId);
    this.myform = new FormGroup({
      ddlresidents: new FormControl('0')
    });
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.AcknowledgeOrders, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getResidentDropData() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetAckOrdersResidentDrop + this.userId)
      .subscribe(res => this.residents = res, error => this.alertService.error(error.message)
      );
  }
  getAllAckOrders(userId: number) {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetAckOrders + userId)
      .subscribe(res => {
        this.ackOrders = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  selectedRecords: any[] = [];
  onCheckAll(event) {
    if (event == true) {
      this.AcceptButton = false;
      this.RejectButton = false;
      this.CheckAll = true;
      this.ackOrders.forEach(element => {
        element.ApprovalStatus = 1;
        element.ApprovedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.ApprovedDate = new Date().toISOString();
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.AcceptButton = true;
      this.RejectButton = true;
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.AcceptButton = false;
      this.RejectButton = false;
      item.ApprovalStatus = 1;
      item.ApprovedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.ApprovedDate = new Date().toISOString();
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.POrder_Id == item.POrder_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.AcceptButton = true;
        this.RejectButton = true;
      }
      else {
        this.AcceptButton = false;
        this.RejectButton = false;
      }
    }
  }
  acceptcheck() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select records");
      this.AcceptButton = true;
      this.RejectButton = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Orders_AcceptAckOrders, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Acknowledged Successfully");
            this.AcceptButton = true;
            this.RejectButton = true;
            this.getAllAckOrders(this.userId);
            this.selectedRecords = [];
            this.ng4LoadingSpinnerService.hide();
          }
          else {
            this.alertService.error("Rejected selected orders");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  rejectcheck() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select Records");
      this.RejectButton = true;
      this.AcceptButton = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.selectedRecords.forEach(element => {
        element.ApprovalStatus = 2;
        element.ApprovedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.ApprovedDate = new Date().toISOString();
      });
      this.dataservice.post(this.config.Emar_Orders_RejectAckOrders, this.selectedRecords)
        .subscribe(res => {
          this.alertService.error("Rejected");
          this.AcceptButton = true;
          this.RejectButton = true;
          this.selectedRecords = [];
          this.getAllAckOrders(this.userId);
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  GetFileAckData(fileID: number, FileName: string) {
    this.viewTitle = FileName;
    this.dataservice.get<any>(this.config.Emar_Inbound_GetFileAckData + fileID)
      .subscribe(res => {
        this.fileViewData = res;
        if (res.FileError != null) {
          this.fileAck = "File Ack";
          this.modalViewIsOpen = true;
          this.fileError = res.FileError;

        }
        else if (res.FileError == null) {
          this.fileAck = "File Ack Error";
          this.modalViewIsOpen = true;

        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  Modalclose() {
    this.modalViewIsOpen = false;
    this.fileError = [];
  }
  changeResident() {
    if (this.myform.value.ddlresidents == "0")
      this.getAllAckOrders(this.userId);
    this.getAckOrdersByPatientId(this.myform.value.ddlresidents);
  }
  getAckOrdersByPatientId(residentId: number) {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetAckOrdersByPatientId + residentId)
      .subscribe(res => {
        this.ackOrders = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getOrderDetailsID(orderId: number) {

  }
}
