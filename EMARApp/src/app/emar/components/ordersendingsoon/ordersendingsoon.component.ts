import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import { OrdersEndingSoon } from '../../../models/orders.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';;
import { SharedService } from '../../../services/shared/shared.service';

@Component({
  selector: 'app-ordersendingsoon',
  templateUrl: './ordersendingsoon.component.html',
  styleUrls: ['./ordersendingsoon.component.css']
})
export class OrdersendingsoonComponent implements OnInit {
  @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;
  template: string;
  @Input() nsStationsList:any;
  @Output() leaveRequestResult: EventEmitter<any> = new EventEmitter();
  ordersEndingList: any[]=[];
  errorMessage: string = '';
  confirmButton: boolean = true;
  checkAll: boolean = false;
  ordersPopPagination = 10;
  pageNo: number = 1;
  selectedOrders: OrdersEndingSoon[] = [];
  pageConfig = {};
  totalRecords:number =0;
  public nsIds:any;
  
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration,
    private persistanceService: PersistanceService, public activeDefaultModal: NgbActiveModal, private sharedService: SharedService, private route: Router) {

  }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.nsIds=this.nsStationsList;
    this.getOrdersEndingSoon();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.confirmButton = false;
      let selectedOrder: OrdersEndingSoon = {
        porder_Id: item.porder_Id,
        PQuantity_Id: item.PQuantity_Id
      };

      this.selectedOrders.push(selectedOrder);
    }
    else {
      const index = this.selectedOrders.findIndex(i => i.porder_Id == item.porder_Id && i.PQuantity_Id == item.PQuantity_Id);
      this.selectedOrders.splice(index, 1);
      if (this.selectedOrders.length == 0) {
        this.confirmButton = true;
      }
      else {
        this.confirmButton = false;
      }
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.confirmButton = false;
      this.checkAll = true;
      this.selectedOrders = this.ordersEndingList.map(o => {
        return { porder_Id: o.porder_Id, PQuantity_Id: o.PQuantity_Id };
      });
    }
    else {
      this.clearValues();
    }
  }

  confirmClick() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_Orders_ConfirmOrdersEndingSoon, this.selectedOrders)
      .subscribe(res => {
        if (res > 0) {
          this.errorMessage = "Confirmed End Dates successfully";
          this.getOrdersEndingSoon(1,1);
          this.clearValues();
        }
        else
        {
          this.errorMessage = "Something went wrong";
          this.leaveRequestResult.emit({ responce: 2, count: this.totalRecords });
        this.ng4LoadingSpinnerService.hide();
        }
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.errorMessage = "Something wrong with service.";
        });
  }
  getOrdersEndingSoon(currentPage?:any,resultEmit?:any) {
    this.ng4LoadingSpinnerService.show();
    //currentPage = currentPage==undefined?1:currentPage;
    this.dataservice.get<any>(this.config.Emar_Orders_GetOrdersEndingSoon + this.nsIds)
      .subscribe(res => {
        this.ordersEndingList = res.m_Item1;
        this.totalRecords = res.m_Item2;
        //this.pageNo =currentPage;
        if(resultEmit!=undefined)
        {
          this.leaveRequestResult.emit({ responce: 1, count: this.totalRecords });
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = "Something wrong with service.";
      });
  }
  getGridDataOnPageChange(currentPage:any)
  {
    this.getOrdersEndingSoon(currentPage);
  }
  cancelClick() {
    this.clearValues();
  }
  clearValues() {
    this.checkAll = false;
    this.checkboxes.forEach((element) => {
      element.nativeElement.checked = false;
    });
    this.confirmButton = true;
    this.selectedOrders = [];
    //this.errorMessage = '';
  }
  onRowSelect(orderId: number, patientId: number, quantityId: number) {
    this.sharedService.changePatientId(patientId);
    this.sharedService.changeOrderId(orderId);
    this.sharedService.changeQuantityId(quantityId);
    this.route.navigate(['/home/orderinfo']);
    this.activeDefaultModal.close();
  }
}
