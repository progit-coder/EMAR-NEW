import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
@Component({
  selector: 'app-search-drug-name',
  templateUrl: './search-drug-name.component.html',
  styleUrls: ['./search-drug-name.component.css']
})
export class SearchDrugNameComponent implements OnInit {
  template: string;
  @Input() searchDrugText: any;
  @Output() searchResult: EventEmitter<any> = new EventEmitter();
  errorMessage: string = '';
  gridPagination = 10;
  p: number = 1;
  myform: FormGroup;
  searchText: string;
  orderType: string;
  nurseStationId: number;
  drugDataForOrders: any[] = [];
  drugDataForStock: any[] = [];
  DrugName: any;
  selectedDrugId: number = 0;
  selectedStockId: number = 0;
  selectedItem: any;
  allStockData: any[]=[];
  public isAllStockClicked:boolean=false;
  public searchStockText:string="";

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration,
    private persistanceService: PersistanceService) {

  }
  ngOnInit() {
    this.searchText = this.searchDrugText.searchText;
    this.orderType = this.searchDrugText.orderType;
    this.nurseStationId = this.searchDrugText.nurseStationId;
    this.template = this.dataservice.template;

    this.myform = new FormGroup({
      drug: new FormControl(''),
      type: new FormControl(true),
    });
    this.myform.patchValue({
      drug: this.searchText,
      type: this.orderType == 'order' ? true : false,
    });
    this.searchDrugNames();
  }

  searchDrugNames() {
    this.selectedDrugId = 0;
    this.selectedStockId = 0;
    this.drugDataForOrders = [];
    this.drugDataForStock = [];
    this.searchText = this.myform.value.drug;
    this.searchStockText=this.myform.value.drug;
    this.orderType = this.myform.value.type == 1 ? 'order' : 'stock';
    if ((this.searchText.length > 2 && this.myform.value.type==true)||this.myform.value.type==false) {
      this.errorMessage = '';
      this.ng4LoadingSpinnerService.show();
      if (this.orderType == "order") {
        this.dataservice.post(this.config.Emar_Orders_SearchDrugName, this.searchText)
          .subscribe((res: any[]) => {
            this.drugDataForOrders = res;
            this.ng4LoadingSpinnerService.hide();
          }, error => {
            this.errorMessage = error.message;
            this.ng4LoadingSpinnerService.hide();
          });
      }
      else if(this.orderType == "stock" && this.searchText!="") {
        let requestObj = {
          DrugName: this.searchText,
          NurseStationId: this.nurseStationId
        };
        this.dataservice.post(this.config.Emar_Orders_GetStockQtyonHand, requestObj)
          .subscribe((res: any[]) => {
            this.drugDataForStock = res;
            this.ng4LoadingSpinnerService.hide();
          }, error => {
            this.errorMessage = error.message;
            this.ng4LoadingSpinnerService.hide();
          });
        //this.config. + term.replace(/[\s&\/\\#,+()$~%.'":*?<>{}]/g, '') + "/" + this.nurseStationId
      }
      else if(this.orderType == "stock" && this.searchText=="")
      {
        this.GetAllStockData();
      }
    }
    else {
      this.errorMessage = 'Please enter minimum 3 characters';
      this.drugDataForOrders = [];
      this.drugDataForStock = [];
    }
  }
  getSelectedDrug(item: any) {
    this.errorMessage = '';
    this.selectedDrugId = item.Drug_Id;
    this.selectedItem = item;
  }
  getSelectedStock(item: any) {
    this.errorMessage = '';
    this.selectedStockId = item.Stock_Id;
    this.selectedItem = item;
  }
  okClick() {
    if (this.selectedDrugId == 0 && this.selectedStockId == 0) {
      this.errorMessage = 'Please select one record to proceed further';
    }
    else {
      let searchData = {
        "orderType": this.myform.value.type == 1 ? 'order' : 'stock',
        "id": this.myform.value.type == 1 ? this.selectedDrugId : this.selectedStockId,
        "selectedItem": this.selectedItem
      }
      this.searchResult.emit(searchData);
    }
  }
  cancelClick() {
    this.errorMessage = '';
    let searchData = {
      "orderType": 'cancel'
    }
    this.searchResult.emit(searchData);
  }
  GetAllStockData()
  {
    this.isAllStockClicked=true;
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetAllOrderStockQtyonHand+ this.nurseStationId)
    .subscribe((res: any[]) => {
      this.allStockData = res;
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.errorMessage = error.message;
      this.ng4LoadingSpinnerService.hide();
    });
  }
}
