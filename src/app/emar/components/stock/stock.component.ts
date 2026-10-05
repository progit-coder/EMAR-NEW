
import { of as observableOf, Observable, Subject } from 'rxjs';

import { catchError, switchMap, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
import { Stock } from '../../../models/common.model';
import { BarcodeEntity } from '../../../models/orders.model';
@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit {
  p: number = 1;
  gridPagination = this.config.gridPagination;
  public template;
  myform: FormGroup;
  public drugList;
  public flag: boolean = true;
  searchText: string = "";
  public switch: boolean = true;
  public barcodeflag: boolean = false;
  public stockObj: Stock;
  DrugName: any;
  public modalClone: boolean = false;
  public searchTerms = new Subject<string>();
  public stockId: number = 0;
  public allStock: any[] = [];
  public barcodear: string[] = [];
  public barcodesList: BarcodeEntity[];
  public nurseStations: any[];
  public ClonenurseStations: any[];
  public CloneFacilities: any[];
  public userId: number;
  pageConfig = {};
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_CloneNurseStations: any = {};
  ShowFilter = true;
  public selectedItems = [];
  public barcodeItems: any[] = [];
  public buttonSave: boolean = false;
  public warnMsg: string = "";
  public stockList: any[] = [];
  public inactivecheckbox: boolean = false;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  dropdownSettings_FacID: any = {};
  dropdownSettings_CloneFacID: any = {};
  public selectedFacItems = [];
  public selectedCloneFacItems = [];
  public selectedCloneNurItems = [];
  public facilities: any[];
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectednItemsNew = [];
  public onRecordEdit: boolean = false;
  EkitpageConfig = {};
  totalRecords: number = 0;
  public stockGPIs: any[];
  public isUserAdmin: boolean = false;
  public isReadOnly: boolean = false;
  public barcode: any;
  public cloneFlag: number = 0;
  public drugFlag: boolean = true;
  public sharedstockFlag: number = 0;
  public validateEmptyField(c: FormControl) {
    return c.value && !c.value.trim() ? {
      pattern: {
        valid: false
      }
    } : null;
  }
  @ViewChild('facilityNameFocus') facilityNameFocus: ElementRef;
  public gpiCodeAlert: boolean = false;
  public duplicateGpicode: string = '';
  public duplicatestockgpi = [];
  public selectedInhand: string = '0';
  public responseInhandData: string = '0';
  public gpiCodeFlag:boolean=false
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService, private router: Router) { }

  ngAfterViewInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Stock");
  }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Stock");
    this.EkitpageConfig = this.persistanceService.getPermissionsByScreen("Ekit");
    if (this.EkitpageConfig == undefined) {
      this.EkitpageConfig = 0;
    }
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        if (this.persistanceService.get('userRole') == '\"SuperAdmin\"') {
          this.isUserAdmin = true;
        }
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.myform = new FormGroup({
          facilityName: new FormControl('', Validators.required),
          nurseStation: new FormControl('', Validators.required),
          drugName: new FormControl('', Validators.required),
          gpicode: new FormControl('', [Validators.pattern(this.config.alphaNumeric), Validators.maxLength(14), Validators.minLength(14)]),
          inhand: new FormControl('', [this.validateEmptyField, Validators.pattern(this.config.decimalAllowTwoDigits)]),
          barcode: new FormControl('', [Validators.maxLength(150)]),
          status: new FormControl('1'),
          trackable: new FormControl(''),
          sharedstock: new FormControl(''),
        });
        this.userActivity();
        this.getAllStockData(1, 1);
        //this.getAllStockGPIs();
        //this.getFacilities();
        //  this.getNurseStations();
        this.getAllBarcodes();

        this.getUserRecentFacilityNurseStations();
        this.loadSearchData();

        this.dropdownSettings_NurseStations = {
          singleSelection: true,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          text: "Nursing Stations",
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          allowSearchFilter: this.ShowFilter,
          noDataAvailablePlaceholderText: 'Please Select Facility'
        };
        this.dropdownSettings_CloneNurseStations = {
          singleSelection: false,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          text: "Nursing Stations",
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter,
          noDataAvailablePlaceholderText: 'Please Select Facility'
        };
        this.dropdownSettings_FacID = {
          singleSelection: true,
          idField: "Facility_Id",
          textField: "Facility_Name",
          // text: "Facilities",
          selectAllText: "Select All",
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          allowSearchFilter: true
        };
        this.dropdownSettings_CloneFacID = {
          singleSelection: false,
          idField: "Facility_Id",
          textField: "Facility_Name",
          // text: "Facilities",
          selectAllText: "Select All",
          itemsShowLimit: 1,
          allowSearchFilter: true
        };
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Stock, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getUserRecentFacilityNurseStations() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFacilities();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getFacilities() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.facilities = res.Facilities;
        //this.ng4LoadingSpinnerService.hide();
        if (this.loginUserReceFacility != null && this.onRecordEdit == false) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedFacItems = [];
            if (checkFacExist != undefined) {
              this.selectedFacItems.push(checkFacExist);
              this.getNurseStations(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              facilityName: this.selectedFacItems,
            });
          }
        }
        if (this.facilities.length == 1 && this.onRecordEdit == false) {
          this.myform.patchValue({
            facilityName: this.facilities,
          });
          this.getNurseStations(res.Facilities[0].Facility_Id);
        }
        //  res.forEach(item => this.selectedFacItems.push(item.Facilities[0].Facility_Id));

      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.selectedItems = [];
    this.myform.patchValue({
      nursestationName: '',
    });
    this.loginUserReceNurseStation = undefined;
    this.getNurseStations(item.Facility_Id);
    this.getIsDataAvailableToCloneStock();
  }
  onCloneFacilitySelect(item: any) {
    this.ClonenurseStations = [];
    this.selectedCloneNurItems = [];
    this.getCloneNurseStations(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.selectedItems = [];
    this.myform.patchValue({
      nursestationName: '',
    });
    this.cloneFlag = 0;
    this.loginUserReceNurseStation = undefined;
  }
  onCloneFacilityDeSelect(item: any) {
    this.ClonenurseStations = [];
    this.selectedCloneNurItems = [];
  }
  getCloneNurseStations(facilityId: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityId)
      .subscribe(res => {
        if (this.myform.value.facilityName[0].Facility_Id == facilityId) {
          this.ClonenurseStations = res.filter(ele => ele.NurseStation_Id != this.myform.value.nurseStation[0].NurseStation_Id);
        }
        else {
          this.ClonenurseStations = res;
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseStations(facilityId: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;

        if (this.loginUserReceNurseStation != undefined && this.onRecordEdit == false) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectedItems = [];
            this.selectednItemsNew = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItemsNew.push(checkNsExist);
              }
            }
            if (this.selectednItemsNew.length != 0) {
              let lastNsId = this.selectednItemsNew[this.selectednItemsNew.length - 1];
              this.selectedItems.push(res.filter(s => s.NurseStation_Id === parseInt(lastNsId.NurseStation_Id))[0]);
            }
            this.myform.patchValue({
              nurseStation: this.selectedItems,
            });
            this.getIsDataAvailableToCloneStock();
          }
        }
        else if (this.loginUserReceNurseStation == undefined && this.facilities.length == 1 && this.onRecordEdit == false) {
          if (res.length > 0) {
            this.selectedItems.push(res[0]);
            this.myform.patchValue({
              nurseStation: this.selectedItems,
            });
          }
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }

  getAllBarcodes() {
    this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllStockEkitBarcodes + "/" + 0)
      .subscribe(res => {
        this.barcodesList = res;
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  changeSwitch() {
    if (this.switch == true) {
      this.router.navigate(['/home/stock']);
    }
    else if (this.switch == false) {
      this.router.navigate(['/home/ekit']);
    }
  }
  addBarcode(value: any) {
    debugger
    this.warnMsg = "";
    if (value != '' && value.length <= 150) {
      let barcode = this.myform.value.barcode;
      let GPIS = this.myform.value.gpicode;
      let Fac = this.myform.value.facilityName[0].Facility_Id;
      let barcodeValue = (barcode.split('/'))[0];
      debugger;
      if (barcodeValue != "") {
        if (value.includes('NW')) {
          this.ng4LoadingSpinnerService.show()
          this.dataservice.get<any>(this.config.Emar_StockEkit_CheckAutoBarcodeAlert + barcode)
            .subscribe(res => {
              if(res==1){
                this.alertService.warn("system generated barcode cannot be added");
                this.myform.patchValue({
                  barcode: ''
                });
              }
              this.ng4LoadingSpinnerService.hide();
            },
              error => {
                this.alertService.error(error.message);
                this.ng4LoadingSpinnerService.hide();
              });
        }
        else {
          // console.log("Barcode list - " + this.barcodesList.length);
          //let result = this.barcodesList.find(x => x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, ''));
          let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcodeValue.toLowerCase().replace(/\s/g, '') : false);
          let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodeValue.toLowerCase().replace(/\s/g, '')) : undefined;

          //let Re = checkInBarcodeArray.filter()
          let Recods = result.filter(x => x.GPICode == GPIS);

          if (checkInBarcodeArray != undefined) {

            this.warnMsg = "Barcode already exist for another Med";
            this.alertService.warn("Barcode already exists for this item");
            this.myform.patchValue({
              barcode: ''
            });
          }
          else if (result.length > 0 && Recods.length == 0) {
            this.warnMsg = "Barcode already exist for another Med";
            // this.alertService.warn("This barcode is assigned to an item/drug with a different GPI");
            this.alertService.warn("This barcode is assigned to different drug");

            this.myform.patchValue({
              barcode: ''
            });
          }
          else {
            this.barcodear.push(barcodeValue);
            this.warnMsg = "";
            this.myform.patchValue({
              barcode: '',
            })
          }
        }
      }
    }
    else {
      this.barcode = "Barcode cannot be more than 150 characters long."
      this.ng4LoadingSpinnerService.hide();
    }
  }
  removeBarcode(i: number) {
    this.barcodear.splice(i, 1);
  }
  insertStock() {

    if ((this.stockId == 0 && this.isUserAdmin == true) || this.stockId != 0) {
      if (this.myform.value.barcode != null) {
        if (this.myform.value.barcode != "") {
          let barcode = this.myform.value.barcode;
          let GPIS = this.myform.value.gpicode;
          let Fac = this.myform.value.facilityName[0].Facility_Id;
          let barcodeValue = (barcode.split('/'))[0];
          if (barcodeValue != "") {
            //let result = this.barcodesList.find(x => x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, ''));
            let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodeValue.toLowerCase() : false);
            let Recods = result.filter(x => x.BarcodeDetail1 == barcode).filter(x => x.GPICode == GPIS);

            if (result.length > 0 && Recods.length == 0) {
              this.warnMsg = "Barcode already exist for another Med";
              // this.alertService.warn("This barcode is assigned to an item/drug with a different GPI");
              this.alertService.warn("This barcode is assigned to different drug");
              this.myform.patchValue({
                barcode: ''
              });
            }
          }
        }
      }
      if (this.warnMsg != "Barcode already exist for another Med") {
        if (this.isUserAdmin == true && (this.myform.value.gpicode == null || this.myform.value.gpicode == "" || this.myform.value.gpicode == undefined)) {
          debugger
          this.alertService.warn("GPI Code is required");
        }
        else {
          // let gpi = this.myform.value.gpicode.toLowerCase();
          // let result = this.stockGPIs.find(x => this.stockId == 0 ? x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() : null : x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() && x.Stock_Id != this.stockId : null);
          // if (result != undefined || result != null) {
          //   this.alertService.error("GPI Code already exists");
          //   this.myform.patchValue({
          //     gpicode: ''
          //   });
          // }
          // else {
          this.ng4LoadingSpinnerService.show();
          let qty = 0;
          debugger;
          let stockids = '';
          if (this.stockId != 0) {
            // if(Number(this.selectedInhand) > 0 ){
            //   if(Number(this.myform.value.inhand) > Number(this.selectedInhand)){
            //     let qtyUpdated = Number(this.myform.value.inhand) - Number(this.selectedInhand);
            //     qty = (qtyUpdated + Number(this.responseInhandData))
            //   }
            // }
            let selectedStockRecord = this.stockList.find(x => x.Stock_Id == this.stockId);
            stockids = selectedStockRecord.MergedStockIds.length > 0 ? selectedStockRecord.MergedStockIds : '';

          } else {
            qty = this.myform.value.inhand;
          }
          debugger
          this.stockObj = {
            Stock_Id: this.stockId,
            Facility_Id: this.stockId == 0 ? this.myform.value.facilityName[0].Facility_Id : this.selectedFacItems[0].Facility_Id,
            NurseStation_Id: this.myform.value.sharedstock == true ? null : (this.stockId == 0 ? this.myform.value.nurseStation[0].NurseStation_Id : this.selectedItems[0].NurseStation_Id),
            DrugName: this.myform.value.drugName.trim(),
            InHand: this.myform.value.inhand == undefined || this.myform.value.inhand == '' ? null : this.replaceCommaWithPeriod(this.myform.value.inhand),
            Barcode: this.barcodear.length ? this.barcodear.map(item => item.replace(/ /g, '')) : this.barcodear,
            GPICode: this.myform.value.gpicode,
            TrackableBit: this.myform.value.trackable == true ? 1 : 0,
            Sharedstockbit: this.myform.value.sharedstock == true ? 1 : 0,
            Stock_Status: this.myform.value.status == true ? 1 : 0,
            Stock_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
            Stock_CreatedDate: this.dateFormatPipe.transform(new Date()),
            MergedStockIds: this.stockId == 0 ? '' : stockids
          }
          this.dataservice.post(this.config.Emar_StockEkit_InsertUpdateStock, this.stockObj)

            .subscribe(res => {
              this.gpiCodeAlert = false;
              this.duplicateGpicode = ''
              this.ng4LoadingSpinnerService.hide();
              if (res == -1) {
                this.alertService.warn("Same drug name and GPI code already exists in the same nursing station .Please edit existing record");
              }
              else if (res == -2) {
                //this.alertService.warn("Same GPI code record already existed in same Facility. Edit existing record to Share same Drug among Other Nursing Stations");
                this.alertService.warn("Same drug name and GPI code already exists in the same facility.Please edit existing record");
              }
              else {
                this.alertService.success("Save successful");

                this.gpiCodeFlag=false
                this.resetScreen();
                this.warnMsg = "";
                this.inactivecheckbox = false;
                this.getAllStockData(1, 1);
                this.getAllBarcodes();
                //this.getAllStockGPIs();
              }
            },
              error => {
                this.alertService.error(error.message);
                this.ng4LoadingSpinnerService.hide();
              });

          //}
        }
      }
    }
    else if (this.stockId == 0 && this.isUserAdmin == false) {
      this.alertService.warn("Only Power Admin can insert new record");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onSearchChange(searchValue: string): void {
    if (searchValue.length > 2) {
      this.inactivecheckbox == true ? this.getAllStockData(1, 0) : this.getAllStockData(1, 1);
    }
    else if (searchValue.length == 0) {
      this.inactivecheckbox == true ? this.getAllStockData(1, 0) : this.getAllStockData(1, 1);
    }
  }

  getAllStockData(pageNumber: number, status: number, pageClick?: number) {
    debugger;
    this.ng4LoadingSpinnerService.show();
    this.p = pageNumber;
    pageClick == 1 ? (this.inactivecheckbox == true ? status = 0 : 1) : status = status;
    let searchValue = this.searchText != undefined && this.searchText != null ? this.searchText.replace(new RegExp('/', 'g'), '-') : '';
    let obj =
    {
      CurrentPage: pageNumber,
      PageSize: this.gridPagination,
      SearchText: searchValue,
      Status: status,
      UserId: this.userId,
    }
    this.dataservice.post(this.config.Emar_StockEkit_GetAllStock, obj)
      .subscribe(res => {
        this.stockList = res.GridData; console.log(this.stockList, "stockList")
        this.totalRecords = res.TotalRecords;
        //this.stockList=res.filter(s=>s.Stock_Status==1);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getStockDetailsID(stock_Id: number, facilityStatus: number, nurseStationStatus: number) {

    this.barcodear = [];
    if (facilityStatus == 1 && nurseStationStatus == 1) {
      this.ng4LoadingSpinnerService.show();
      this.onRecordEdit = true;
      this.dataservice.get<any>(this.config.Emar_StockEkit_GetStockById + stock_Id)
        .subscribe(res => {
          this.getFacilities();
          this.isReadOnly = this.isUserAdmin == true ? false : true;
          this.getNurseStationsId(res);
          // this.selectedItems = [];
          // this.selectedFacItems=[];
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else {
      if (facilityStatus == 0) {
        this.alertService.warn("Selected stock facility is inactive, cannot edit record");
        this.selectedFacItems = [];
        this.selectedItems = [];
        this.nurseStations = [];
        this.resetScreen();
      }
      else if (nurseStationStatus == 0) {
        this.alertService.warn("Selected stock nursing station is inactive, cannot edit record");
        this.selectedItems = [];
        this.resetScreen();
      }
      this.ng4LoadingSpinnerService.hide();
    }
    window.scroll(0, 0);
  }
  getNurseStationsId(data: any) {

    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + data.Facility_Id)
      .subscribe(res => {
        this.selectedItems = [];
        this.selectedFacItems = [];
        this.nurseStations = res;
        this.fetchData(data);
        this.getSecondaryStockDetails(data.Stock_Id)

      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchData(res: any) {
    this.selectedFacItems.push(this.facilities.filter(e => e.Facility_Id == res.Facility_Id)[0]);

    if (res.NurseStation_Id != null) {
      let checkNsExist = this.nurseStations.find(e => e.NurseStation_Id == res.NurseStation_Id);
      if (checkNsExist != undefined) {
        this.selectedItems.push(this.nurseStations.filter(e => e.NurseStation_Id == res.NurseStation_Id)[0]);
      }
      else
        this.selectedItems = [];
    }
    // if(res.MergedDrugNames != null && res.MergedStockIds != null && res.MergedDrugNames.length > 0 && res.MergedStockIds.length > 0){

    // }else{
    this.responseInhandData = res.InHand;
    this.myform.patchValue({
      facilityName: this.selectedFacItems,
      nurseStation: this.selectedItems,
      drugName: res.DrugName,
      gpicode: res.GPICode,
      trackable: res.TrackableBit,
      sharedstock: res.SharedStockBit,
      inhand: res.InHand,
      status: res.Stock_Status,
    });
      this.isReadOnly=true
      this.gpiCodeFlag=true
    this.getIsDataAvailableToCloneStock();
    this.stockId = res.Stock_Id;

    if (res.SharedStockBit == 1) {
      this.selectedItems = [];
      this.myform.patchValue({
        nurseStation: this.selectedItems
      });
      const nsvalidation = this.myform.get('nurseStation');
      nsvalidation.setValidators(null);
      nsvalidation.clearValidators();
      nsvalidation.updateValueAndValidity();
    }
    else {
      const nsvalidation = this.myform.get('nurseStation');
      nsvalidation.setValidators([Validators.required]);
      nsvalidation.updateValueAndValidity();
    }
  }
  resetScreen() {
    //this.myform.reset();
    debugger;
    this.myform.patchValue({
      status: '1',
      drugName: '',
      gpicode: '',
      trackable: '',
      sharedstock: '',
      inhand: ''
    });
    this.stockId = 0;
    this.barcodear = [];
    this.isReadOnly = false;
    this.drugFlag = true;
    this.myform.controls['drugName'].reset();
    const nsvalidation = this.myform.get('nurseStation');
    nsvalidation.setValidators([Validators.required]);
    nsvalidation.updateValueAndValidity();
  }
  checkSharedStock(value: any) {
    if (value == true) {
      this.selectedItems = [];
      this.myform.patchValue({
        nurseStation: this.selectedItems
      });
      const nsvalidation = this.myform.get('nurseStation');
      nsvalidation.setValidators(null);
      nsvalidation.clearValidators();
      nsvalidation.updateValueAndValidity();
    }
    else {
      const nsvalidation = this.myform.get('nurseStation');
      nsvalidation.setValidators([Validators.required]);
      nsvalidation.updateValueAndValidity();
    }
  }
  onNurseStationSelect(item: any) {
    this.getIsDataAvailableToCloneStock();
    if (this.myform.value.sharedstock == true) {
      this.checkSharedStock(true);
    }
    this.myform.value.nurseStation = item;
  }
  onNurseStationDeSelect(item: any) {
    this.myform.value.nurseStation = [];
    this.getIsDataAvailableToCloneStock();
  }
  loadSearchData() {
    this.drugList = this.searchTerms.pipe(
      debounceTime(300),        // wait for 300ms pause in events  
      distinctUntilChanged(),   // ignore if next search term is same as previous  
      switchMap(term => term   // switch to new observable each time  
        // return the http search observable  
        ? this.dataservice.search(this.config.Emar_StockEkit_GetGenericName + term.replace(/[&\/\\#,+()$~%.'":*?<>{}]/g, ''))
        // or the observable of empty heroes if no search term  
        : observableOf<any[]>([{ "GPI": 0, "DrugName": "No Record Found" }])),
      catchError(error => {
        // TODO: real error handling 
        this.alertService.error(error.message)
        return observableOf<any[]>([]);
      }));
    console.log(this.drugList)
  }
  onselectDrug(item: any) {
    debugger
    console.log(item)
    if (item != '') {
      this.myform.patchValue({
        drugName: item.DrugName,
        gpicode: item.GPI
      });
      this.flag = false;
      this.drugFlag = false;
      this.gpiCodeFlag=true;

    }
    else {
      this.gpiCodeFlag=false;
      return false;
    }

  }
  searchDrug(term: string): void {
    this.drugFlag = true;
    if (term.length > 1) {
      this.flag = true;
      this.searchTerms.next(term);
      this.gpiCodeFlag=false;
    }
    else {
      this.flag = false;
      this.myform.patchValue({
        gpicode: ''
      })
      this.gpiCodeFlag=false;
    }
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      //  this.inactivecheckbox=true;

      //this.stockList= this.allStock.filter(n=>n.Stock_Status==0);
      this.getAllStockData(1, 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getAllStockData(1, 1);
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.stockList.forEach(element => {
        element.Stock_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Stock_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.Stock_Status=1;
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.UpdateStatus = true;
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.UpdateStatus = false;
      item.Stock_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Stock_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.Stock_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Stock_Id == item.Stock_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateStockStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      this.dataservice.post(this.config.Emar_StockEkit_UpdateStocksStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.inactivecheckbox == true ? this.getAllStockData(1, 1) : this.getAllStockData(1, 0);
            this.UpdateStatus = true;
            this.CheckAll = false;
            this.inactivecheckbox == true ? this.inactivecheckbox = false : this.inactivecheckbox = true;
            //this.inactivecheckbox=false;
            //this.getAllStockData(this.userId);
            this.selectedRecords = [];

          }
          else if (res == 0) {
            this.alertService.error("Status Updated Failed.");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  // getAllStockGPIs() {
  //   this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllStockGPICodes)
  //     .subscribe(res => {
  //       this.stockGPIs = res;
  //       this.ng4LoadingSpinnerService.hide();
  //     },
  //       error => {
  //         this.alertService.error(error.message);
  //         this.ng4LoadingSpinnerService.hide();
  //       });
  // }
  // checkGPI(): any {
  //   let gpi = this.myform.value.gpicode.toLowerCase();
  //   if (gpi.length == 14) {
  //     let result = this.stockGPIs.find(x => this.stockId == 0 ? x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() : null : x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() && x.Stock_Id != this.stockId : null);
  //     if (result != undefined || result != null) {
  //       this.alertService.error("GPI Code already exists");
  //       this.myform.patchValue({
  //         gpicode: ''
  //       });
  //     }
  //   }
  //   else { }

  // }
  OpenCloneModel() {

    this.selectedCloneFacItems = [];
    this.selectedCloneNurItems = [];
    this.ClonenurseStations = [];
    if (this.myform.value.facilityName.length != 0 && this.myform.value.nurseStation.length == 0) {
      this.sharedstockFlag = 1;
      this.CloneFacilities = this.facilities.filter(elememt => elememt.Facility_Id != this.myform.value.facilityName[0].Facility_Id);
    }
    if (this.myform.value.facilityName.length != 0 && this.myform.value.nurseStation.length != 0) {
      this.CloneFacilities = this.facilities;
      this.sharedstockFlag = 0;
    }
    this.modalClone = true;
    setTimeout(() => {
      this.facilityNameFocus.nativeElement.focus()
    }, 300)
  }
  closeClone() {
    this.modalClone = false;
  }
  CloneConfiguration() {

    if (this.selectedCloneFacItems.length != 0 || this.selectedCloneNurItems.length != 0) {
      this.ng4LoadingSpinnerService.show();
      let fac = this.selectedCloneFacItems;
      let nur = this.selectedCloneNurItems;
      let nurs = "";
      let facs = "";
      let sharedstock;
      let stockEkit;
      for (let i = 0; i < nur.length; i++) {
        nurs = nurs + nur[i].NurseStation_Id + ',';
      }
      for (let j = 0; j < fac.length; j++) {
        facs = facs + fac[j].Facility_Id + ',';
      }
      if (nur.length > 0) {
        sharedstock = 0;
        this.sharedstockFlag = 0;
      }
      else {
        sharedstock = 1;
        this.sharedstockFlag = 0;
      }
      if (this.switch == true) {
        stockEkit = 1;
      }
      else {
        stockEkit = 2;
      }
      let obj = {
        Input: sharedstock == 0 ? this.myform.value.nurseStation[0].NurseStation_Id : this.myform.value.facilityName[0].Facility_Id,
        clone: sharedstock == 0 ? nurs : facs,
        sharedstockbit: sharedstock,
        stockekit: stockEkit,
        createdBy: this.userId
      }
      this.dataservice.post(this.config.Emar_StockEkit_InsertStockEkitClone, obj)
        .subscribe(res => {

          this.modalClone = false;
          this.getAllStockData(1, 1);
          this.selectedCloneFacItems = [];
          this.selectedCloneNurItems = [];
          this.alertService.success("Clone successful");
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  getIsDataAvailableToCloneStock() {
    if (this.myform.value.facilityName.length != 0 || this.myform.value.nurseStation.length != 0) {

      let facilityId = this.myform.value.facilityName.length != 0 && this.myform.value.nurseStation.length == 0 ? this.myform.value.facilityName[0].Facility_Id : 0;
      let nsId = this.myform.value.facilityName.length != 0 && this.myform.value.nurseStation.length != 0 ? this.myform.value.nurseStation[0].NurseStation_Id : 0;

      this.dataservice.get<any>(this.config.Emar_StockEkit_IsDataAvailableToCloneStock + facilityId + "/" + nsId)
        .subscribe(res => {
          this.cloneFlag = res;
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  onGpiCodeChange() {
    debugger;

  }
  beforeSavingStock() {
    debugger;
    let facilityId = this.myform.value.facilityName[0].Facility_Id
    let nsId = Array.isArray(this.myform.value.nurseStation) && this.myform.value.nurseStation.length > 0
      ? this.myform.value.nurseStation[0].NurseStation_Id
      : this.myform.value.nurseStation.NurseStation_Id || 0;

    this.dataservice.get(this.config.Emar_StockEkit_StockGPIAlertint + facilityId + '/' + nsId + '/' + this.myform.value.gpicode).subscribe((res: any) => {
      console.log(res, "res");
      this.duplicatestockgpi = res
      if (res != null && res.length > 0 && this.stockId == 0) {
        if (res[0].DrugName.toLowerCase().trim() == this.myform.value.drugName.toLowerCase().trim()) {
          this.addStockDrugname();
        } else {
          this.gpiCodeAlert = true;
          this.duplicateGpicode = res[0].DrugName;
        }

      } else {
        this.addStockDrugname();
      }
    })
    // let data = this.stockList.find(x => x.GPICode == this.myform.value.gpicode);
    // if(data != undefined && data != null){
    //   this.gpiCodeAlert =true;
    //   this.duplicateGpicode = data.DrugName;
    // }else{
    //   this.addStockDrugname();
    // }
  }
  addStockDrugname() {
    this.insertStock()
  }
  closeStockModal() {
    debugger;
    if (this.stockId == 0) {
      this.gpiCodeAlert = false;
      this.duplicateGpicode = '';
      console.log(this.duplicatestockgpi, "duplicatestockgpi")

      if (this.duplicatestockgpi.length) {
        let qty = Number(this.myform.value.inhand) > 0 ? this.myform.value.inhand : '0';
        let obj = {
          StockId: this.duplicatestockgpi[0].StockId,
          InHand: qty,
          Barcode: this.barcodear,
          StockCreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          StockCreatedDate: this.dateFormatPipe.transform(new Date()),
        }
        this.dataservice.post(this.config.Emar_StockEkit_UpdateStockQtyGpi, obj).subscribe((res) => {
          if (res == 1) {
            this.gpiCodeAlert = false;
            this.duplicatestockgpi = [];
            this.barcodear = [];
            // this.myform.reset();
            this.resetScreen();
            this.searchText = '';
            this.getAllStockData(1, 1);

          }

        })

      }
    }

    // this.gpiCodeAlert = false;

    //proc

  }
  getSecondaryStockDetails(stock_Id: number) {
    debugger
    let selectedStockRecord = this.stockList.find(x => x.Stock_Id == stock_Id);
    console.log(selectedStockRecord)
    if (selectedStockRecord != undefined && selectedStockRecord.MergedStockIds.length > 0) {
      this.myform.patchValue({
        inhand: selectedStockRecord.InHand,
      });
      if (selectedStockRecord.Barcode != null && selectedStockRecord.Barcode.length) {
        this.barcodear = [];
        let list: string = selectedStockRecord.Barcode;
        this.barcodear = list.split(', ');
      }
      this.selectedInhand = selectedStockRecord.InHand;
      // this.selectedBarcode = selectedStockRecord.Barcode; 
    } else {
      this.barcodear = [];
      if (selectedStockRecord.Barcode != null && selectedStockRecord.Barcode.length) {
        let list: string = selectedStockRecord.Barcode;
        this.barcodear = list.split(', ');
      }
      else {
        this.barcodear = [];
      }
    }

  }
  replaceCommaWithPeriod(value: any): string {
    if (value == null || value === '') {
      return '0';
    }
    return value.toString().replace(/,/g, '.'); // Replace all commas with periods
  }
}
