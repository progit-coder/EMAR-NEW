import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AlertService } from 'src/app/_services';


@Component({
  selector: 'app-searchpharmacyname',
  templateUrl: './searchpharmacyname.component.html',
  styleUrls: ['./searchpharmacyname.component.css']
})
export class SearchpharmacynameComponent implements OnInit {


  template: string;
  @Input() searchDrugText: any;
  @Output() searchResult: EventEmitter<any> = new EventEmitter();
  errorMessage: string = '';
  gridPagination = 10;
  p: number = 1;
  myform: FormGroup;
  searchText: string ;
  phydetails: any[] = [];
  activePharmacies: any[] = [];
  searchTerm: string = '';
  filteredData: any[] = [];
  selectedPharmacyId:number = 0;
  selectedItem: any;
  selectedPhone:any;
  selectedaddress:any;
  totaldata:any;

  //public isAllStockClicked:boolean=false;

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration,
    private persistanceService: PersistanceService ,private alertService: AlertService) {

  }
  ngOnInit() {
    this.searchText = this.searchDrugText.searchText;
    this.template = this.dataservice.template;


    this.myform = new FormGroup({
      pharmacyname: new FormControl('', [Validators.required, Validators.maxLength(60), Validators.pattern(this.config.alphaNumericFewSpecialCharacters8)]),

    });
    this.myform.patchValue({
      pharmacyname: this.searchText,

    });
    this.searchPharmacyNames();
  }
  searchPharmacyNames(){
    this.searchText = this.myform.value.pharmacyname;
    this.searchTerm=this.myform.value.pharmacyname;
    this.selectedPharmacyId = 0;
    let userId=this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_pharmacy_getPharmacyData + userId )
    .subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      this.phydetails = res;
      this.activePharmacies =  this.phydetails.filter(p => p.Pharmacy_Status == 1);
      console.log(this.activePharmacies,"activve pharmacies")
      this.filteredData = this.activePharmacies;
      this.search();
    }
    , error => {
      this.errorMessage = error.message;
      this.ng4LoadingSpinnerService.hide();
    })

  }
  search() {
    if (this.searchTerm && this.searchTerm.length > 2) {
      this.filteredData = this.activePharmacies.filter(item => {
        return item.PharmacyName && item.PharmacyName.toLowerCase().includes(this.searchTerm.toLowerCase())||
                item.Pharmacy_Address1 && item.Pharmacy_Address1.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
                item.Pharmacy_Phone && item.Pharmacy_Phone.toLowerCase().includes(this.searchTerm.toLowerCase()) })
         
        
      }
    else {
      this.filteredData = this.activePharmacies;
    }
  }
  toggleS(PharmacyId: number, favourite: number , event:Event ){
    event.stopPropagation();
    console.log(PharmacyId ,"star clicked")
    debugger;
      this.ng4LoadingSpinnerService.show();
      favourite == 1 ? 1 : 0;
      let userId=this.persistanceService.get(this.config.loggedInUserKey);
      this.dataservice.get<any>(this.config.Emar_update_PharmacyFav +  PharmacyId + "/" + favourite + "/" + userId   )
        .subscribe(res => {
          this.searchPharmacyNames()
          this.ng4LoadingSpinnerService.hide();
        },
         error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });

  }
  getSelectedPharmacy(item: any) {
    this.errorMessage = '';
    this.selectedPharmacyId = item.Pharmacy_Id;
    console.log(this.selectedPharmacyId , "selected id")
    this.selectedaddress=item.Pharmacy_Address1
    console.log(this.selectedaddress, ' selected address')
    this.selectedItem = item.PharmacyName;
    console.log(this.selectedItem , "selected item")
    this.selectedPhone= item.Pharmacy_Phone;
   this.totaldata=this.selectedItem +(this.selectedaddress.length>0?','+this.selectedaddress:'')+(this.selectedPhone.length>0?','+this.selectedPhone:'');
   console.log(this.totaldata, ' total data')
  }

  okClick() {
      if ( this.selectedPharmacyId == 0) {
        this.errorMessage = 'Please select one record to proceed further';
      }
      else {
        let searchData = {
          "selectedItem": this.totaldata,
          "pharmacyid" : this.selectedPharmacyId
        }
        this.searchResult.emit(searchData);
      }
    }
    cancelClick() {
    this.errorMessage = '';
    let searchData = {
      "selectedItem": this.searchText,
      "pharmacyid" : 0 ,
    }
    this.searchResult.emit(searchData);
   }
  }


