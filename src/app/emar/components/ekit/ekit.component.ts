
import { of as observableOf, Observable, Subject } from 'rxjs';

import { catchError, switchMap, distinctUntilChanged, debounceTime } from 'rxjs/operators';
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
import { Ekit } from '../../../models/common.model';
import { BarcodeEntity } from '../../../models/orders.model';
@Component({
  selector: 'app-ekit',
  templateUrl: './ekit.component.html',
  styleUrls: ['./ekit.component.css']
})
export class EkitComponent implements OnInit {

  
  p: number = 1;
  gridPagination = this.config.gridPagination;
  public template;
  myform: FormGroup;
  searchText: string = "";
  public switch = false;
  DrugName: any;
  public flag: boolean = true;
  public searchTerms = new Subject<string>();
  public drugList;
  public barcodear: any[] = [];
  public barcodesList: BarcodeEntity[];
  public nurseStations: any[];
  public userId: number;
  public ekitObj: Ekit;
  public buttonSave: boolean = false;
  pageConfig = {};
  public ekitId: number = 0;
  public allEkit: any[] = [];
  dropdownSettings_NurseStations: any = {};
  ShowFilter = true;
  public selectedItems = [];
  public warnMsg: string = "";
  public ekitList: any[] = [];
  public inactivecheckbox: boolean = false;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  dropdownSettings_FacID: any = {};
  public selectedFacItems = [];
  public facilities: any[];
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectednItemsNew = [];
  public onRecordEdit: boolean = false;
  StockpageConfig = {};
  totalRecords: number = 0;
  public ekitGPIs: any[];
  public isUserAdmin: boolean = false;
  public isReadOnly: boolean = false;
  public barcode: any;
  dropdownSettings_CloneFacID: any = {};
  public modalClone:boolean=false;
  public ClonenurseStations: any[];
  dropdownSettings_CloneNurseStations: any = {};
  public selectedCloneFacItems=[];
  public selectedCloneNurItems=[];
  public cloneFlag:number=0;
  public drugFlag:boolean=true;
  public sharedstockFlag:number=0;
  public CloneFacilities: any[];
  public modalControlsubstanceConfirmationIsOpen:boolean=false;
  public isEkitControl:number=0;
  public  validateEmptyField(c: FormControl) {
    return c.value && !c.value.trim() ? {
      pattern: {
        valid: false
      }
    } : null;
  }
  public today:any=this.dateFormatPipe.transformISODate(new Date());

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService, private router: Router) { }

  ngAfterViewInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Ekit");
  }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Ekit");
    this.StockpageConfig = this.persistanceService.getPermissionsByScreen("Stock");
    const currentDate = new Date(); 

    this.today=this.dateFormatPipe.transformISODate(currentDate);

    if (this.StockpageConfig == undefined) {
      this.StockpageConfig = 0;
    }
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.ng4LoadingSpinnerService.show();
        if (this.persistanceService.get('userRole') == '\"SuperAdmin\"') {
          this.isUserAdmin = true;
        }
        this.userActivity();
        this.loadSearchData();
        this.getAllBarcodes();
        //this.getAllEkitGPIs();
        this.getAllEkitData(1, 1);
        //this.getFacilities();
        this.getUserRecentFacilityNurseStations();
        this.myform = new FormGroup({
          facilityName: new FormControl('', Validators.required),
          nurseStation: new FormControl('', Validators.required),
          drugName: new FormControl('', [Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]),
          gpicode: new FormControl('', [Validators.pattern(this.config.alphaNumeric), Validators.maxLength(14), Validators.minLength(14)]),
          inhand: new FormControl('',[this.validateEmptyField, Validators.pattern(this.config.decimalAllowTwoDigits)]),
          barcode: new FormControl('', [Validators.maxLength(150)]),
          status: new FormControl('1'),
          lotnumber: new FormControl(''),
          expirydate: new FormControl('',),
          trackable: new FormControl(''),
          sharedekit: new FormControl(''),
          controlsubstance:new FormControl(''),
        });
        this.dropdownSettings_NurseStations = {
          singleSelection: true,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          text: "Nursing Stations",
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please Select Facility'
        };
        this.dropdownSettings_FacID = {
          singleSelection: true,
          idField: "Facility_Id",
          textField: "Facility_Name",
          text: "Facilities",
          //  selectAllText: "Select All",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
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
        this.ng4LoadingSpinnerService.hide();
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
            facilityName: this.facilities
          });
          this.getNurseStations(res.Facilities[0].Facility_Id);
        }
        // this.myform.patchValue({
        //   facilityName:this.facilities
        // });
        //   this.getNurseStations(res.Facilities[0].Facility_Id);
        //  res.forEach(item => this.selectedFacItems.push(item.Facilities[0].Facility_Id));

      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.selectedItems = [];
    this.myform.patchValue({
      nurseStation: '',
    });
    this.loginUserReceNurseStation = undefined;
    this.getNurseStations(item.Facility_Id);
    this.getIsDataAvailableToCloneEkit();
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];

    this.selectedItems = [];
    this.myform.patchValue({
      nurseStation: '',
    });
    this.cloneFlag=0;
    this.loginUserReceNurseStation = undefined;
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
          }
          this.getIsDataAvailableToCloneEkit();
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
    this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllStockEkitBarcodes+'/'+1)
      .subscribe(res => {
        this.barcodesList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  addBarcode(value: any) {
    this.warnMsg = "";
     
    if (value != '' && value.length <= 150) {
      let barcode = this.myform.value.barcode;
      let GPIS = this.myform.value.gpicode;
      let barcodeValue=(barcode.split('/'))[0];
       ;
      if(barcodeValue!="")
      {
      //let result = this.barcodesList.find(x => x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, ''));
      let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcodeValue.toLowerCase().replace(/\s/g, '') : false);
      let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodeValue.toLowerCase().replace(/\s/g, '')) : undefined;
      let Recods = result.filter(x=>x.GPICode == GPIS);
      if(checkInBarcodeArray != undefined)
      {
       this.warnMsg = "Barcode already exist for another Med";
       this.alertService.warn("Barcode already exists for this medication");
       this.myform.patchValue({
         barcode: ''
       });
      }
      else if (result.length > 0 && Recods.length == 0 ) {
        this.warnMsg = "Barcode already exist for another Med";
        this.alertService.warn("This barcode is assigned to an item/drug with a different GPI");
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
    else {
      this.barcode = "Barcode cannot be more than 150 characters long."
      this.ng4LoadingSpinnerService.hide();
    }
  }
  removeBarcode(i: number) {
    this.barcodear.splice(i, 1);
  }
  changeSwitch() {
    if (this.switch == true) {
      this.router.navigate(['/home/stock']);
    }
    else if (this.switch == false) {
      this.router.navigate(['/home/ekit']);
    }
  }
  insertEkit() {
    debugger
    if ((this.ekitId == 0 && this.isUserAdmin == true) || this.ekitId != 0) {
      if (this.myform.value.barcode != null) {
        if (this.myform.value.barcode != "") {

          let barcode = this.myform.value.barcode;
          let GPIS = this.myform.value.gpicode;
          let barcodeValue=(barcode.split('/'))[0];
          if(barcodeValue!="")
          {
          //let result = this.barcodesList.find(x => x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, ''));
          let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodeValue.toLowerCase() : false);
          let Recods = result.filter(x=>x.GPICode == GPIS && Recods.length == 0);
          if (result.length > 0) {
            this.warnMsg = "Barcode already exist for another Med";
            this.alertService.warn("Barcode already exists for another medication");
            this.myform.patchValue({
              barcode: ''
            });
          }
        }
      }
      }
      if (this.warnMsg != "Barcode already exist for another Med") {
        if (this.isUserAdmin == true && (this.myform.value.gpicode == null || this.myform.value.gpicode == "" || this.myform.value.gpicode == undefined)) {
          this.alertService.warn("GPI Code is required");
        }
        else {
          // let gpi = this.myform.value.gpicode.toLowerCase();
          // let result = this.ekitGPIs.find(x => this.ekitId == 0 ? x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() : null : x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() && x.Ekit_Id != this.ekitId : null);
          // if (result != undefined || result != null) {
          //   this.alertService.error("GPI Code already exists");
          //   this.myform.patchValue({
          //     gpicode: ''
          //   });
          // }
          // else {
          this.ng4LoadingSpinnerService.show();
          this.ekitObj = {
            Ekit_Id: this.ekitId,
            Facility_Id: this.ekitId == 0 ? this.myform.value.facilityName[0].Facility_Id : this.selectedFacItems[0].Facility_Id,
            NurseStation_Id:this.myform.value.sharedekit == true?null:(this.ekitId == 0 ? this.selectedItems[0].NurseStation_Id : this.selectedItems[0].NurseStation_Id),
            DrugName: this.myform.value.drugName,
            GPICode: this.myform.value.gpicode,
            TrackableBit: this.myform.value.trackable == true ? 1 : 0,
            SharedeKitbit: this.myform.value.sharedekit == true ? 1 : 0,
            ControlSubstance:this.myform.value.controlsubstance == true?1:0,
            InHand:'0',
            //Barcode: this.barcodear,
            Barcode:[],
            NDC: null,
            //LotNumber: this.myform.value.lotnumber,
            LotNumber:'',
            ExpiryDate:null,
            //ExpiryDate: this.dateFormatPipe.transform(this.myform.value.expirydate),
            Ekit_Status: this.myform.value.status == true ? 1 : 0,
            Ekit_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
            Ekit_CreatedOn: this.dateFormatPipe.transform(new Date()),
          }
           
          this.dataservice.post(this.config.Emar_StockEkit_InsertUpdateEkit, this.ekitObj)
            .subscribe(res => {
              this.ng4LoadingSpinnerService.hide();
              if (res == -1) {
                this.alertService.warn("Same drug name and GPI code already exists in the same nursing station. Please edit existing record");
              }
              else if (res == -2) {
               // this.alertService.warn("Same GPI code record already existed in same Facility. Edit existing record to Share same Drug among Other Nursing Stations");
               this.alertService.warn("Same drug name and GPI code already exists in the same facility.Please edit existing record");
              }
              else {
                this.resetScreen();
                this.alertService.success("Save successful");
                this.warnMsg = "";
                this.barcodear = [];
                this.inactivecheckbox = false;
                this.getAllEkitData(1, 1);
                this.getAllBarcodes();
                //this.getAllEkitGPIs();
              }
            },
              error => {
                this.alertService.error(error.message);
                this.ng4LoadingSpinnerService.hide();
              });
          // }
        }
      }
    }
    else if (this.ekitId == 0 && this.isUserAdmin == false) {
      this.alertService.warn("Only Power Admin can insert new record");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onSearchChange(searchValue: string): void {
    if (searchValue.length > 2) {
      this.inactivecheckbox == true ? this.getAllEkitData(1, 0) : this.getAllEkitData(1, 1);
    }
    else if (searchValue.length == 0) {
      this.inactivecheckbox == true ? this.getAllEkitData(1, 0) : this.getAllEkitData(1, 1);
    }

  }
  getAllEkitData(pageNumber: number, status: number, pageClick?: number) {
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
    this.dataservice.post(this.config.Emar_StockEkit_GetAllEkit, obj)
      .subscribe(res => {
        this.ekitList = res.GridData;
        this.totalRecords = res.TotalRecords;
        //this.ekitList=res.filter(e=>e.Ekit_Status==1);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getEkitDetailsID(ekit_Id: number, facilityStatus: number, nurseStationStatus: number) {
    this.barcodear = [];
    if (facilityStatus == 1 && nurseStationStatus == 1) {
      this.ng4LoadingSpinnerService.show();
      this.onRecordEdit = true;
      this.dataservice.get<any>(this.config.Emar_StockEkit_GetEkitById + ekit_Id)
        .subscribe(res => {
          // this.selectedItems = [];
          // this.selectedFacItems=[];
          this.isReadOnly = this.isUserAdmin == true ? false : true;
          this.getFacilities();
          this.getNurseStationsId(res);
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else {
      if (facilityStatus == 0) {
        this.alertService.warn("Selected eKit Facility is inactive, unable to edit record");
        this.selectedFacItems = [];
        this.selectedItems = [];
        this.nurseStations = [];
        this.resetScreen();
      }
      else if (nurseStationStatus == 0) {
        this.alertService.warn("Selected eKit nursing station is inactive, unable to edit record");
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
        this.isEkitControl=0;
        this.nurseStations = res;
        this.fetchData(data);
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchData(res: any) {
    this.selectedFacItems.push(this.facilities.filter(e => e.Facility_Id == res.Facility_Id)[0]);
    if (res.NurseStation_Id != null)
    {
    let checkNsExist=this.nurseStations.find(e => e.NurseStation_Id == res.NurseStation_Id);
    if(checkNsExist!=undefined)
    {
      this.selectedItems.push(this.nurseStations.filter(e => e.NurseStation_Id == res.NurseStation_Id)[0]);
    }
    else
      this.selectedItems = [];
    }
    this.myform.patchValue({
      facilityName: this.selectedFacItems,
      nurseStation: this.selectedItems,
      drugName: res.DrugName,
      gpicode: res.GPICode,
      trackable: res.TrackableBit,
      sharedekit: res.SharedeKitBit,
      inhand: res.InHand,
      status: res.Ekit_Status,
      lotnumber: res.LotNumber,
      expirydate: this.dateFormatPipe.dateFormat(res.ExpiryDate),
      controlsubstance:res.ControlSubstanceBit,
    });
    this.isEkitControl=res.ControlSubstanceBit;
    this.getIsDataAvailableToCloneEkit();
    this.ekitId = res.Ekit_Id;
    if (res.Barcode != null) {
      let list: string = res.Barcode;
      this.barcodear = list.split(', ');
    }
    else {
      this.barcodear = [];
    }
    if (res.SharedeKitBit == 1) {
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
  checkSharedeKit(value: any) {
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
  resetScreen() {
    this.myform.patchValue({
      status: '1',
      drugName: '',
      gpicode: '',
      trackable: '',
      sharedekit: '',
      inhand: '',
      lotnumber: '',
      expirydate: '',
      controlsubstance:''
    });
    this.barcodear = [];
    this.ekitId = 0;
    this.isReadOnly = false;
    this.drugFlag=true;
    this.myform.controls['drugName'].reset();
    const nsvalidation = this.myform.get('nurseStation');
    nsvalidation.setValidators([Validators.required]);
    nsvalidation.updateValueAndValidity();
    this.isEkitControl=0;
  }
  onNurseStationSelect(item: any) {
    this.getIsDataAvailableToCloneEkit();
    if(this.myform.value.sharedekit == true)
    {
      this.checkSharedeKit(true);
    }
    this.myform.value.nurseStation = item;
  }
  onNurseStationDeSelect(item: any) {
    this.myform.value.nurseStation = [];
    this.getIsDataAvailableToCloneEkit();
  }
  loadSearchData() {
    this.drugList = this.searchTerms.pipe(
      debounceTime(300),        // wait for 300ms pause in events  
      distinctUntilChanged(),   // ignore if next search term is same as previous  
      switchMap(term => term   // switch to new observable each time  
        // return the http search observable  
        ? this.dataservice.search(this.config.Emar_StockEkit_GetGenericName + term.replace(/[&\/\\#,+()$~%.'":*?<>{}]/g, ''))
        // or the observable of empty heroes if no search term  
        : observableOf<any[]>([{"GPI": 0, "DrugName": "No Record Found" }])),
      catchError(error => {
        // TODO: real error handling 
        this.alertService.error(error.message)
        return observableOf<any[]>([]);
      }));
  }
  onselectDrug(item: any) {
    if (item != '') {
      this.myform.patchValue({
        drugName: item.DrugName,
        gpicode:item.GPI
      });
      this.flag = false;
      this.drugFlag=false;
    }
    else {
      return false;
    }

  }
  searchDrug(term: string): void {
    this.drugFlag=true;
    if (term.length > 1) {
      this.flag = true;
      this.searchTerms.next(term);

    }
    else {
      this.flag = false;
      this.myform.patchValue({
        gpicode:''
      })
    }
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      //  this.inactivecheckbox=true;

      //this.ekitList= this.allEkit.filter(n=>n.Ekit_Status==0);
      this.getAllEkitData(1, 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getAllEkitData(1, 1);
      this.ng4LoadingSpinnerService.hide();
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.ekitList.forEach(element => {
        element.Ekit_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Ekit_CreatedOn = this.dateFormatPipe.dateWithTime(new Date());
        //element.Ekit_Status=1;
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
      item.Ekit_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Ekit_CreatedOn = this.dateFormatPipe.dateWithTime(new Date());
      //item.Ekit_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Ekit_Id == item.Ekit_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateekitStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_StockEkit_UpdateEkitsStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.inactivecheckbox == true ? this.getAllEkitData(1, 1) : this.getAllEkitData(1, 0);
            this.UpdateStatus = true;
            this.CheckAll = false;
            this.inactivecheckbox == true ? this.inactivecheckbox = false : this.inactivecheckbox = true;
            //this.inactivecheckbox=false;
            //this.getAllEkitData(this.userId);
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
  // getAllEkitGPIs() {
  //   this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllEkitGPICodes)
  //     .subscribe(res => {
  //       this.ekitGPIs = res;
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
  //     let result = this.ekitGPIs.find(x => this.ekitId == 0 ? x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() : null : x.GPICode != null ? (x.GPICode).toLowerCase() === gpi.toLowerCase() && x.Ekit_Id != this.ekitId : null);
  //     if (result != undefined || result != null) {
  //       this.alertService.error("GPI Code already exists");
  //       this.myform.patchValue({
  //         gpicode: ''
  //       });
  //     }
  //   }
  //   else { }

  // }
  onCloneFacilitySelect(item: any) {
    this.ClonenurseStations = [];
    this.selectedCloneNurItems = [];
    this.getCloneNurseStations(item.Facility_Id);
  }
  onCloneFacilityDeSelect(item: any) {
    this.ClonenurseStations = [];
    this.selectedCloneNurItems = [];
  }
  getCloneNurseStations(facilityId: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityId)
      .subscribe(res => {
         
        if(this.myform.value.facilityName[0].Facility_Id==facilityId)
        {
          this.ClonenurseStations=res.filter(ele=>ele.NurseStation_Id!=this.myform.value.nurseStation.NurseStation_Id);
        }
        else
        {
        this.ClonenurseStations = res;
        }
      },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
}
OpenCloneModel()
{
  this.selectedCloneFacItems=[];
  this.selectedCloneNurItems=[];
  this.ClonenurseStations=[];
  if(this.myform.value.facilityName.length!=0 &&  this.myform.value.nurseStation.length==0)
  {
    this.sharedstockFlag=1;
    this.CloneFacilities=this.facilities.filter(elememt=>elememt.Facility_Id != this.myform.value.facilityName[0].Facility_Id);
  }
  if(this.myform.value.facilityName.length!=0 &&  this.myform.value.nurseStation.length!=0)
  {
    this.CloneFacilities=this.facilities;
    this.sharedstockFlag=0;
  }
  this.modalClone=true;
}
closeClone()
{
  this.modalClone=false;
}
CloneConfiguration()
{
   
  if(this.selectedCloneFacItems.length!=0 || this.selectedCloneNurItems.length!=0)
  {
  this.ng4LoadingSpinnerService.show();
  let fac=this.selectedCloneFacItems;
  let nur=this.selectedCloneNurItems;
  let nurs="";
  let facs="";
  let sharedstock;
  let stockEkit;
  for(let i=0;i<nur.length;i++)
  {
    nurs=nurs+nur[i].NurseStation_Id+',';
  }
  for(let j=0;j<fac.length;j++)
  {
    facs=facs+fac[j].Facility_Id+',';
  }
  if(nur.length>0)
  {
    sharedstock=0;
  }
  else
  {
    sharedstock=1;
  }
  if(this.switch==true)
  {
    stockEkit=1;
  }
  else
  {
    stockEkit=2;
  }
  let obj={
    Input:sharedstock==0?this.myform.value.nurseStation[0].NurseStation_Id:this.myform.value.facilityName[0].Facility_Id,
    clone:sharedstock==0?nurs:facs,
    sharedstockbit:sharedstock,
    stockekit:stockEkit,
    createdBy:this.userId
  }
  this.dataservice.post(this.config.Emar_StockEkit_InsertStockEkitClone, obj)
    .subscribe(res => {
       
      this.modalClone=false;
      this.getAllEkitData(1, 1);
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
getIsDataAvailableToCloneEkit() 
{
  if (this.myform.value.facilityName.length != 0 || this.myform.value.nurseStation.length != 0) {

  let facilityId=this.myform.value.facilityName.length != 0 && this.myform.value.nurseStation.length == 0?this.myform.value.facilityName[0].Facility_Id:0;
  let nsId=this.myform.value.facilityName.length != 0 && this.myform.value.nurseStation.length != 0?this.myform.value.nurseStation[0].NurseStation_Id:0;
  
  this.dataservice.get<any>(this.config.Emar_StockEkit_IsDataAvailableToCloneEkit+ facilityId + "/" + nsId)
    .subscribe(res => {
      this.cloneFlag = res;
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
}
checkControlSubstance(event:any)
{
  if(this.ekitId!=0 && event==false && this.isEkitControl==1)
  {
    this.modalControlsubstanceConfirmationIsOpen=true;
  }
}
controlSubtanceConfirm(value:number)
{
  this.modalControlsubstanceConfirmationIsOpen=false;
  if(value==0)
  {
    //Control med
    this.myform.patchValue({
      controlsubstance:1,
    });
  }
  else if(value==1)
  {
    // Not control med
  }
}
}