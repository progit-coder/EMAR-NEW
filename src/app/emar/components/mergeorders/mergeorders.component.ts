import { Component, OnInit, ChangeDetectorRef, Input, SimpleChanges, SimpleChange, EventEmitter, Output } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { APIConfiguration } from '../../../models/app.constants';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-mergeorders',
  templateUrl: './mergeorders.component.html',
  styleUrls: ['./mergeorders.component.css']
})
export class MergeordersComponent implements OnInit {
  template: string;
  ordersList: any[];
  ordersListExceptSelected: any;
  @Input() mergeChanges: any;
  @Output() mergeResult: EventEmitter<any> = new EventEmitter();
  selectedOrderId: any;
  selectedPatientId: any;
  selectedQuantityId: any;
  selectedOrderQtyForMerge: number = 0;
  selectedOrderQtyForDirHoa: number = 0;
  errorMessage: string = '';
  public modalEndDateMergeOpen:boolean=false;
  public ddlEndDate:any='';
  public endDate:any='';
  public endDateArray=[];
  minEndDate: string = this.dateFormatPipe.dateFormat(new Date());
  userID:number;
    ddlStartDate:any='';
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, public dateFormatPipe:CustomdatePipe) { }

  ngOnInit() {
    this.selectedOrderId = this.mergeChanges.selectedOrderId;
    this.selectedPatientId = this.mergeChanges.selectedPatientId;
    this.selectedQuantityId = this.mergeChanges.selectedQuantityId;
    this.userID=this.mergeChanges.userId;
    this.template = this.dataservice.template;
    this.getMergeOrdersByPatientId(this.selectedPatientId, this.selectedOrderId, this.selectedQuantityId);
  }
  getMergeOrdersByPatientId(patientId: number, orderId: number, quantityId: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetMergeOrdersByPatientId + patientId + "/" + orderId + "/" + quantityId)
      .subscribe(res => {
        this.ordersList = res;
        this.ordersListExceptSelected = this.ordersList.filter(n => n.POrder_Id != this.selectedOrderId);
        if (this.ordersListExceptSelected.length > 0) {
          this.selectedOrderQtyForMerge = this.ordersListExceptSelected[0].PQuantity_Id;
        }
        this.ordersList.forEach(element => {
          if(element.EndDate!=null && element.EndDate!="")
          {
            let endDate={ id: element.POrder_Id, date: element.EndDate };
            //this is remove end date dropdown , user can manually enter the endate. 
            this.endDateArray=[]  // this is to make hide dropdown.
            // this.endDateArray.push(endDate);
          }
        });

        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.errorMessage = error.message;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  saveClick() {
    this.errorMessage = '';
    if (this.selectedOrderQtyForMerge != 0 && this.selectedOrderQtyForDirHoa != 0)
    {
      //this.modalEndDateMergeOpen=true;
      this.mergeSelectedOrders(this.selectedQuantityId, this.selectedOrderQtyForMerge, this.selectedOrderQtyForDirHoa);
    }
    else if (this.selectedOrderQtyForMerge == 0)
      this.errorMessage = "Please Select an order to Merge with";
    else if (this.selectedOrderQtyForDirHoa == 0)
      this.errorMessage = "Please Select an order for Directions and HOA";
  }
  orderEndDateMergeConfirmation()
  {
    this.modalEndDateMergeOpen=false;
    this.mergeSelectedOrders(this.selectedQuantityId, this.selectedOrderQtyForMerge, this.selectedOrderQtyForDirHoa,1);
  }
  closeModel()
  {
    this.modalEndDateMergeOpen=false;
    this.mergeSelectedOrders(this.selectedQuantityId, this.selectedOrderQtyForMerge, this.selectedOrderQtyForDirHoa,0);
  }
  mergeSelectedOrders(orderQtyId1: number, orderId2: number, orderId3: number,ensDateConformation?:any) {
    this.ng4LoadingSpinnerService.show();
    let selectedEndDate=this.ddlEndDate!='' && this.ddlEndDate!=null && this.ddlEndDate!=undefined?this.dateFormatPipe.transformISODate(this.ddlEndDate):this.endDate!='' && this.endDate!=null && this.endDate!=undefined?this.endDate:null
    let selectedStartDate=this.ddlStartDate!='' && this.ddlStartDate!=null && this.ddlStartDate!=undefined?this.dateFormatPipe.transformISODate(this.ddlStartDate):'';

    this.dataservice.get<any>(this.config.Emar_Orders_MergeTwoOrders + orderQtyId1 + "/" + orderId2 + "/" + orderId3 + "/" + selectedEndDate+"/"+this.userID +"/"+selectedStartDate)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res > 0) {
          this.errorMessage = '';
          this.mergeResult.emit(res);
        }
        else {
          this.errorMessage = 'Could not able to merge the selected.';
        }
      },
        error => {
          this.errorMessage = error.message;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onSelectMergeWith(orderQtyIdForMerge: number) {
    this.errorMessage = '';
    this.selectedOrderQtyForMerge = orderQtyIdForMerge;
  }
  onSelectForDirectionsHoa(orderQtyIdForDirHoa: number,startdate:any) {
    this.errorMessage = '';
    this.selectedOrderQtyForDirHoa = orderQtyIdForDirHoa;
      this.ddlStartDate = startdate ? startdate.split('T')[0] : '';

  }
  cancelClick() {
    this.errorMessage = '';
    this.selectedOrderQtyForMerge = 0;
    this.selectedOrderQtyForDirHoa = 0;
    this.mergeResult.emit(-1);
  }
    isDateValid(): boolean {
  if (!this.ddlStartDate) return false;

  const selectedEndDate =
    this.ddlEndDate ? this.ddlEndDate :
    this.endDate ? this.endDate : null;

  if (!selectedEndDate) return false;

  const start = new Date(this.ddlStartDate);
  const end = new Date(selectedEndDate);

  return end > start;
} 
}
