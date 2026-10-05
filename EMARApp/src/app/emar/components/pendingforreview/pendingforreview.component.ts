import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {  HOA, Orderupdate, NurseComments, CommonOrderStatus, OrdersData, WeekMasterData, MonthMasterData, HoursMasterData, DrugAdministrationTime, OrderFavourite, PhysicianDetails, FrequencyMasterDataWithShifts,CommonDcOrderStatus } from '../../../models/orders.model';
import { AlertService } from '../../../_services';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { SharedService } from '../../../services/shared/shared.service';
import {  Subject } from 'rxjs';
import { NgbModal, NgbModalOptions,NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SearchDrugNameComponent } from '../search-drug-name/search-drug-name.component';
import { catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { of as observableOf} from 'rxjs';
@Component({
  selector: 'app-pendingforreview',
  templateUrl: './pendingforreview.component.html',
  styleUrls: ['./pendingforreview.component.css']
})
export class PendingforreviewComponent implements OnInit {
  @Output() reviewInfoResult: EventEmitter<any> = new EventEmitter();
  @Input() nsId:number;
  @Input() modalTitle:string;
  template: string;
  pageConfig = {};
  public myform: FormGroup;
  public isReadOnly: boolean = false;
  private orderId: number = 0;
  public residentId: number;
  public quantityId: number;
  public userID: any;
  public nurseNotes: any[] = [];
  scheduleform: FormGroup;
  timeform: FormGroup;
  noteform: FormGroup;
  favForm: FormGroup;
  DcForm: FormGroup;
  DrugName: any;
  dropdownSettings_Physician: {};
  dropdownSettings_Route: { };
  dropdownSettings_Frequency: {};
  dropdownSettings_StartTime: {};
  dropdownSettings_NextTime: { };
  public nurseStationId: number = 0;
  defaultPhysicianNPI: any;
  public physiciansdrop: PhysicianDetails[];
  public routes: any[];
  public barcodesList: any[];
  notesFlag: number = 0;
  public favstatus: number = 0;
  public splits: number = 0;
  demographicInfoData: any;
  public timesArray: any[] = [];
  public startTimeArray: number;
  public isHoursReadOnly: boolean = false;
  public times: string = "";
  public modalTimeIsOpen: boolean = false;
  public isReadOnlyforControl: boolean = true;
  reviewClickedFlag: number;
  public selectedphyItems = [];
  hoaObj: HOA;
  public dAdminId: number = 0;
  selectedDays: any[];
  public modalfavIsOpen: boolean = false;
  public modalnoteIsOpen: boolean = false;
  public modalInsuliIsOpen: boolean = false;
  public modalHOAIsOpen: boolean = false;
  frequencyList: FrequencyMasterDataWithShifts[];
  weeksList: WeekMasterData[];
  monthsList: MonthMasterData[];
  hoursList: HoursMasterData[];
  monthDays: any[];
  isShiftSchedule: boolean = false;
  public selectedfrequencyItems = [];
  selectedstItems: any;
  public selectedntItems = [];
  public favouriteMasterList: any[];
  private favouriteObj: OrderFavourite[] = [];
  public favobj: any[] = [];
  public selectedOrder: any;
  public selectedOrderQuantity: any;
  public selectedOrderDADminId: any;
  public ordersDetails = {} as OrdersData;
  public barcodear: any[] = [];
  reactivateStatus: number = 0;
  reviewFlag: any;
  selectedroItems: any[];
  public stockId: number = 0;
  gpiCode: string = '';
  drugNameChanged: boolean = false;
  isControlSubstanceReadOnly: boolean = false;
  buttonsStatus: number = 0;
  modalOption: NgbModalOptions = {};
  minStartDate: string = this.dateFormatPipe.dateFormat(new Date());
  public modalControlIsOpen: boolean = false;
  orderUpdateObj: Orderupdate;
  orderStatusObj: CommonOrderStatus;
  public fav: any;
  nurseNotesObj: NurseComments;
  dropdownSettings_Days: any = {};
  dropdownSettings_Month:any = {};
  dropdownSettings_Week:any = {};
  public flag: boolean = true;
  public searchTerms = new Subject<string>();
  public drugList: any;
  medispanControlSubBit: number = 0;
  barCodeFlag:number =0;
  barCodeStatusFlag:number =0;
  public patientTypeList:any[]=[];
  public controlledSubstancemessage:string ='';
  public orderOrigin:any;
  public controlsubstanceBit =0;
  public modalBarcodeIsOpen:boolean=false;
  public isDrugOrder: boolean=false;
  public prnSchedleCheckModal:boolean=false;
  public btnFlag:number=1;
  public btnHoaSaveFlag: number=1;
  selectedWeeks:any[];
  selectedMonths:any[];
  isWeeksDisabled:boolean=false;
  isdaysDisabled:boolean=false;
  public modalFieldsChangesDcConfirmation:boolean=false;
  public fieldsChangesDcFlag="";
  public selectedOrderDetails:any;
  public modalDcConfirmationIsOpen: boolean = false;
  orderStatusDCObj: CommonDcOrderStatus;
  public scheduleTextObj:DrugAdministrationTime;
  public clicked:boolean = true;
  public disabledButtons:any;
  public barcodeFacilityId:number =0;

  public  validateEmptyField(c: FormControl) {
  return c.value && !c.value.trim() ? {
    required: {
      valid: false
    }
  } : null;
}

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration,
    private persistanceService: PersistanceService, public activeDefaultModal: NgbActiveModal, private sharedService: SharedService, private route: Router, private alertService: AlertService,private dateFormatPipe: CustomdatePipe,private modalService: NgbModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("EMAR");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.sharedService.currentOrderId.subscribe(res => this.orderId = res);
    this.sharedService.currentPatientId.subscribe(res => this.residentId = res);
    this.sharedService.currentQuantityId.subscribe(res => this.quantityId = res);
    this.sharedService.currentFacilityId.subscribe(res=>this.barcodeFacilityId = res);

    this.nurseStationId=this.nsId;
    if (this.residentId != 0) {
      this.pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
      this.userID = this.persistanceService.get(this.config.loggedInUserKey);
      this.getNurseCommentNotesByQuantityId();
    this.myform = new FormGroup({
      resName: new FormControl(''),
      resID: new FormControl(''),
      resDOB: new FormControl(''),
      resAdmitDate: new FormControl(''),
      druggname: new FormControl(''),
      resDischargeDate: new FormControl(''),
      physician: new FormControl('', Validators.required),
      drug: new FormControl('', [Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]),
      dose: new FormControl('', [Validators.required]),//, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
      route: new FormControl('', Validators.required),
      diagnosis: new FormControl(''),
      addInst: new FormControl('', [Validators.required, Validators.maxLength(250)]),
      startDate: new FormControl('', Validators.required),
      endDate: new FormControl(''),
      qtyHand: new FormControl('', [Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)]),
      refill: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric)]),
      alertText: new FormControl(''),
      maxPerDay: new FormControl('',
            [Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
            Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
            Validators.pattern(/^\d*(\.\d{0,3})?$/),
            Validators.pattern(/^\d+(\.\d{0,3})?$/)] ),
      barcode: new FormControl('', [Validators.required, Validators.maxLength(20)]),
      prn: new FormControl(false),
      controlSubstance: new FormControl(false),
      literal:new FormControl(false),
      self: new FormControl(false),
      treatment: new FormControl(false),
      maySub: new FormControl(false),
      type: new FormControl(true),
      nursestationName: new FormControl(),
      ddlresidents: new FormControl(),
      schduleText: new FormControl('', Validators.required),
      insulincomments: new FormControl(),
      orderTypeId: new FormControl()
    });
    this.scheduleform = new FormGroup({
      frequency: new FormControl(''),
      either: new FormControl(''),
      hours: new FormControl('', [Validators.maxLength(2), Validators.pattern(this.config.numeric), Validators.min(1), Validators.max(23)]),
      mon: new FormControl(true),
      tue: new FormControl(true),
      wed: new FormControl(true),
      thu: new FormControl(true),
      fri: new FormControl(true),
      sat: new FormControl(true),
      sun: new FormControl(true),
      week: new FormControl(0),
      month: new FormControl(0),
      aDay: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric), Validators.min(1)]),
      hDay: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric), Validators.min(1)]),
      days: new FormControl()
    });
    this.timeform = new FormGroup({
      starttime: new FormControl(''),
    })
    this.noteform = new FormGroup({
      notes: new FormControl('', [Validators.required,this.validateEmptyField, Validators.maxLength(500)])
    });
    this.favForm = new FormGroup({
        favchecks: new FormControl()
      });
    this.DcForm = new FormGroup({
        DcReason: new FormControl('', [Validators.maxLength(500)]),
        dcSplits: new FormControl(false),
        multipledcSplits: new FormControl(false)
    });
      this.dropdownSettings_Physician = {
        singleSelection: true,
        idField: "Physician_Id",
        textField: "PhysicianFullName",
        text: "Select",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true,
      };
      this.dropdownSettings_Route = {
        singleSelection: true,
        idField: "Route_Id",
        textField: "Route",
        text: "Select",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true,
      };
      this.dropdownSettings_Frequency = {
        singleSelection: true,
        idField: "Frequency_Id",
        textField: "Frequency_Name",
        text: "Select",
        itemsShowLimit: 1,
        allowSearchFilter: true,
        closeDropDownOnSelection: true
      };
      this.dropdownSettings_StartTime = {
        singleSelection: true,
        idField: "Hour_Id",
        textField: "Hour_Desc",
        itemsShowLimit: 1,
        allowSearchFilter: true,
        closeDropDownOnSelection:true,
        noDataAvailablePlaceholderText: 'Please Select Facility'
      };
      this.dropdownSettings_NextTime = {
        singleSelection: true,
        idField: "Hour_Id",
        textField: "Hour_Desc",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true,
      };
      this.getAllFlagsForCompanyByNSId(this.nurseStationId,this.residentId);
  }
  }
}
else
this.persistanceService.redirectToHomePage();
}
//Get order details
getOrderDetailsbyorderId(orderId: number, quantityId: number, dAdminId: number) {
    this.ng4LoadingSpinnerService.show();
    this.orderId = orderId;
    this.quantityId = quantityId;
    this.dAdminId = dAdminId;
    this.notesFlag = 0;
    this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId  + "/" + this.userID)
      .subscribe(res => {
        this.isReadOnlyforControl = true;
        this.getFavouritesMasterData();
        this.ordersDetails = res;
        this.selectedOrderDetails=res;
        this.fieldsChangesDcFlag="";
        this.fetchOrdersData(res);
        this.selectedOrder = orderId;
        this.selectedOrderQuantity = quantityId;
        this.selectedOrderDADminId = dAdminId;
        this.hoaObj = null;
        this.getNurseCommentNotesByQuantityId();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
}
fetchOrdersData(res) {
  this.reviewClickedFlag = 1;
  this.barCodeFlag =0;
  const maxpervalidation = this.myform.get('maxPerDay');
  maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
  Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
  Validators.pattern(/^\d*(\.\d{0,3})?$/),
  Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
    maxpervalidation.updateValueAndValidity();
  if (res.OrderTypeID == 4) {
    const drugnameValidations = this.myform.get('drug');
    drugnameValidations.clearValidators();
    drugnameValidations.updateValueAndValidity();
    const barcodevalidation = this.myform.get('barcode');
    barcodevalidation.clearValidators();
    barcodevalidation.updateValueAndValidity();
    const schduleTextValidations = this.myform.get('schduleText');
    schduleTextValidations.clearValidators();
    schduleTextValidations.updateValueAndValidity();
    const doseValidations = this.myform.get('dose');
    doseValidations.clearValidators();
    doseValidations.updateValueAndValidity();
    const routeValidations = this.myform.get('route');
    routeValidations.clearValidators();
    routeValidations.updateValueAndValidity();
    this.barCodeStatusFlag =1;
  }
  else {
    const drugnameValidations = this.myform.get('drug');
    drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
    drugnameValidations.updateValueAndValidity();
    const barcodevalidation = this.myform.get('barcode');
    barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
    barcodevalidation.updateValueAndValidity();
    const schduleTextValidations = this.myform.get('schduleText');
    schduleTextValidations.setValidators([Validators.required]);
    schduleTextValidations.updateValueAndValidity();
    const doseValidations = this.myform.get('dose');
    doseValidations.setValidators([Validators.required]);//, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
    doseValidations.updateValueAndValidity();
    const routeValidations = this.myform.get('route');
    routeValidations.setValidators(Validators.required);
    routeValidations.updateValueAndValidity();
    this.barCodeStatusFlag =0;
  }
  if (res.Barcode != null) {
    let list: string = res.Barcode;
    this.barcodear = list.split(', ');
    const barcodevalidation = this.myform.get('barcode');
    //barcodevalidation.setValidators(null);
    barcodevalidation.clearValidators();
    barcodevalidation.updateValueAndValidity();
  }
  else {
    this.barcodear = [];
  }
  this.favstatus = res.Favouriteflag != 1 ? 0 : 1;
  this.reactivateStatus = (res.POrder_Status != 1) ? 1 : 0;
  this.splits = res.split != 1 ? 0 : 1;
  //this.isReadOnly = res.ReviewFlag == 0 ? false : true;
  this.reviewFlag = res.ReviewFlag;
  //this.drugNameFromPharmacyOrder = res.DrugName == null ? '' : res.DrugName;
  this.selectedphyItems = [];
  this.selectedroItems = [];
  if (res.OrderingPhysicianNPI != null) {
    let physicianRecord = this.physiciansdrop.filter(p => p.PhysicianNPI == res.OrderingPhysicianNPI)[0];
    if (physicianRecord != undefined)
      this.selectedphyItems.push(physicianRecord);
  }
  else {
    this.getAllFlagsForCompanyByNSId(this.nurseStationId,this.residentId);
    if (res.OrderTypeID == 2 || res.OrderTypeID == 4) {
      this.GetPhysicianDropData(this.nurseStationId);

      if (res.OrderingPhysicianNPI != null)
       {
        console.log("Filter 1");
      let physicianRecord = this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0];
      if (physicianRecord != undefined)
        this.selectedphyItems.push(physicianRecord);
       }
    }

  }
  if (res.Route_Id != null && res.Route_Id != 0 && this.routes !=undefined) {

    this.selectedroItems.push(this.routes.filter(r => r.Route_Id == res.Route_Id)[0]);
  }
  this.stockId = res.Stock_Id != null ? res.Stock_Id : 0;
  this.gpiCode = '';
  this.drugNameChanged = false;
  //this.previousDrugName = res.DrugName;
  if (this.demographicInfoData.PVisit_Status == 2) {
    this.buttonsStatus = 1;
  }
  else {
    this.buttonsStatus = 0;
  }
  if(res.AGiveCodeIdentifier!=null)
  {
    this.gpiCode=res.AGiveCodeIdentifier;
  }
  this.myform.patchValue({
    physician: this.selectedphyItems,
    route: this.selectedroItems,
    qtyHand: res.Inhand == null ? 0.00 : res.Inhand,
    drug: res.DrugName,
    druggname : res.DrugName,
    dose: res.Quantity,
    refill: res.Refill,
    addInst: res.Directions,
    insulincomments: res.InsulinComments,
    barcode:'',
    startDate: (res.StartDate == null ? '' : res.StartDate.substring(0, 10)),
    maxPerDay: res.MaxPerdays,
    alertText: res.AlertText,
    endDate: (res.EndDate == null ? '' : res.EndDate.substring(0, 10)),
    type: res.OrderStockFlag,
    schduleText: res.Schedule != "Select Time" ? res.Schedule : '',
    self: res.SelfAdministeredFlag,
    treatment:(res.TreatmentFlag==1 ||res.OrderTypeID==2 ||res.OrderTypeID==5)?true:false,
    prn: res.PRNFlag,
    literal:(res.OrderTypeID==4||res.OrderTypeID==2)?true:false,
    controlSubstance: res.ControlSubstanceBit == 1 ? true : false,
    orderTypeId: res.OrderTypeID
  });
  if(res.OrderTypeID==1)
    {
      this.isDrugOrder=true;
    }
    else
    {
      this.isDrugOrder=false;
    }
  this.orderOrigin =res.OrderOrigin;
  this.medispanControlSubBit =res.MedispanControlSubBit;
  this.controlsubstanceBit =res.ControlSubstanceBit;
  if (res.ControlSubstanceBit == 1 && res.MedispanControlSubBit == 1) {
    this.isControlSubstanceReadOnly = true;
  }
  else
    this.isControlSubstanceReadOnly = false;

    if(res.MaxPerdays !==null && res.MaxPerdays >0)
    this.isInteger();


    this.loadSearchData();
}
getAllFlagsForCompanyByNSId(stationId: number,residentId?:any) {
  let resId=residentId==undefined?0:residentId;
    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId+"/"+ resId)
    .subscribe(res => {
      this.defaultPhysicianNPI = res.PhysicianNPI;
      this.GetPhysicianDropData(this.nurseStationId);
      this.getOrderRoutes();
      this.getAllBarcodes();
      this.getDemographicInfoData();
      this.loadSearchData();
    }, error => {
      this.alertService.error(error.message);
    });
}
getDemographicInfoData() {
  this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
    .subscribe(res => {
      this.demographicInfoData = res;
      this.getOrderDetailsbyorderId(this.orderId,this.quantityId,this.residentId);
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
}
GetPhysicianDropData(nsId: number) {
  this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + nsId+"/"+this.orderId)
    .subscribe(res => {
      this.physiciansdrop = res;
    }, error => {
      this.alertService.error(error.message);
    });
}
getOrderRoutes() {
  this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderRoutes)
    .subscribe(res => {
      this.routes = res;
    }, error => {
      this.alertService.error(error.message);
    });
}
getAllBarcodes() {
  let barcodeFacilityId = this.barcodeFacilityId == null || this.barcodeFacilityId == undefined ? 0 : this.barcodeFacilityId

  this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllBarcodes+barcodeFacilityId )
    .subscribe(res => {
      this.barcodesList = res;
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
}
//Nurse Comments
  getNurseCommentNotesByQuantityId() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetNurseNotes + this.quantityId)
      .subscribe(res => {
        this.nurseNotes = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  nurseCommentsSave() {
    this.nurseNotesObj =
      {
        Comments_Id: 0,
        DrugAdminister_Id: 0,
        NurseCommentType_Id: 5,
        Comment: this.noteform.value.notes,
        comment_Status: 1,
        comment_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        Comment_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        PQuantity_Id: this.quantityId
      }
    this.dataservice.post(this.config.Emar_Orders_InsertUpdateNurseNotes, this.nurseNotesObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.noteform.reset();
        this.alertService.success("Added notes successfully");
        this.getNurseCommentNotesByQuantityId();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  //HOA Methods
  insertHoa() {
    this.clicked = false;
    if (this.residentId > 0) {
      if (this.demographicInfoData.PVisit_Status == 2) {
        this.alertService.warn("Resident has been discharged.")
      }
      else if (this.demographicInfoData.PVisit_Status == 3) {
        this.alertService.warn("Status updated to temporarily inactive.")
      }
      else {
        this.ng4LoadingSpinnerService.show();
        this.times = '';

        //If Frequency is selected or not
        if ((this.scheduleform.value.frequency == null || this.scheduleform.value.frequency.length == 0 || this.scheduleform.value.frequency == undefined)) {
          if ((this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0) && ((this.scheduleform.value.hours == null || this.scheduleform.value.hours == "") || (this.timesArray.length == 0 || this.timesArray == null))) {
            this.alertService.warn("Please select start time.");
          }
          else if (this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null && this.timesArray.length > 1) {
            this.alertService.warn("At once hours and multiple time can't insert.");
          }
          else {
            this.insertScheduleTimes();
          }
        }
        //if frequency is selected and it is not PRN and not shifts
        else if (this.frequencyList.find(f=>f.Frequency_Id==this.scheduleform.value.frequency[0].Frequency_Id).Frequency_PRN != 1 &&this.scheduleform.value.frequency[0].Frequency_Id.startsWith('s') == false && this.scheduleform.value.frequency[0].Frequency_Name.startsWith('QShift') == false) {
          if ((this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0) && ((this.scheduleform.value.hours == null || this.scheduleform.value.hours == "") || (this.timesArray.length == 0 || this.timesArray == null))) {
            this.alertService.warn("Please select start time.");
          }
          else if (this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null && this.timesArray.length > 1) {
            this.alertService.warn("At once hours and multiple time can't insert.");
          }
          else {
            this.insertScheduleTimes();
          }
        }
        else if (this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null && this.timesArray.length > 1) {
          this.alertService.warn("At once hours and multiple time can't insert.");
        }
        else {
          this.insertScheduleTimes();
        }
        this.ng4LoadingSpinnerService.hide();
      }
    }
    else
      this.alertService.error("No Resident selected");
  }
  modalTimeIsClose() {
    this.modalTimeIsOpen = false;
    this.timeform.reset();
    if (this.timesArray.length == 0) {
      this.scheduleform.controls['either'].reset();
    }
  }
  insertScheduleTimes() {
    if (this.timesArray.length != 0) {
      this.timesArray.forEach(element => {
        this.times += element.Hour_Id + ",";
      });
      this.times = this.times.substring(0, this.times.length - 1);
    }
    else if (this.timesArray.length == 0) {
      if ((this.timesArray.length == 0 || this.timesArray == null) && (this.scheduleform.value.either == '' || this.scheduleform.value.either == null || this.scheduleform.value.either == undefined) && ((this.scheduleform.value.frequency != undefined || this.scheduleform.value.frequency != null || this.scheduleform.value.frequency.length != 0))) {
        if (this.frequencyList.find(f=>f.Frequency_Id==this.scheduleform.value.frequency[0].Frequency_Id).Frequency_PRN ==1)
          this.times = '';
      }
      else {
        this.times = this.scheduleform.value.either[0].Hour_Id;
      }
    }
    let nurseStationId = this.nurseStationId;
    let freqId = this.scheduleform.value.frequency == undefined || this.scheduleform.value.frequency.length == 0 || this.scheduleform.value.frequency == null ? null : this.scheduleform.value.frequency[0].Frequency_Id;
    if ((this.frequencyList.find(f=>f.Frequency_Id==freqId )!=undefined && this.frequencyList.find(f=>f.Frequency_Id==freqId ).Frequency_PRN == 1) || this.myform.value.prn==true) {
      this.myform.patchValue({
        prn: true
      });
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        maxpervalidation.updateValueAndValidity();
     }
      else
      {
        this.myform.patchValue({
          prn:false,
        });
        const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        maxpervalidation.updateValueAndValidity();
      }
    this.hoaObj = {
      dadminId: this.dAdminId,
      porderId: this.orderId,
      pquantityId: this.quantityId,
      freqId: freqId != null ? (freqId.startsWith('s') ? null : freqId) : null,
      hourId: this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0 ? null : this.scheduleform.value.either[0].Hour_Id,
      hourIds: this.times,
      //timeformatId: this.scheduleform.value.timeFormat == 0 ? null : this.scheduleform.value.timeFormat,
      hours: this.scheduleform.value.hours=='' || this.scheduleform.value.hours==0?null:this.scheduleform.value.hours,
      monday: this.scheduleform.value.mon,
      tuesday: this.scheduleform.value.tue,
      wednesday: this.scheduleform.value.wed,
      thursday: this.scheduleform.value.thu,
      friday: this.scheduleform.value.fri,
      saturday: this.scheduleform.value.sat,
      sunday: this.scheduleform.value.sun,
      weekId: this.selectedWeeks != undefined ? (this.selectedWeeks.length > 0 ? this.selectedWeeks.join() : null) : null,//this.scheduleform.value.week == 0 ? null : this.scheduleform.value.week,
      monthId: this.selectedMonths != undefined ? (this.selectedMonths.length > 0 ? this.selectedMonths.join() : null) : null, //this.scheduleform.value.month == 0 ? null : this.scheduleform.value.month,
      //days: this.selectedDays.length > 0 ? this.selectedDays.join() : null,
      days:this.selectedDays != undefined ? (this.selectedDays.length > 0 ? this.selectedDays.join() : null) : null,
      createdby: this.userID,
      activedays: this.scheduleform.value.aDay,
      holddays: this.scheduleform.value.hDay,
      NurseStationId: nurseStationId,
      nurseShiftId: freqId != null ? (freqId.startsWith('s') ? freqId.substring(1) : null) : null
    };
    let orderFreqId =(this.scheduleTextObj==undefined || this.scheduleTextObj==null)?0: (this.scheduleTextObj.NurseShiftsId != undefined && this.scheduleTextObj.NurseShiftsId != null && this.scheduleTextObj.NurseShiftsId != "" ? ("s"+this.scheduleTextObj.NurseShiftsId) : (this.scheduleTextObj.NursingFreqId != undefined && this.scheduleTextObj.NursingFreqId != null && this.scheduleTextObj.NursingFreqId != 0) ? this.scheduleTextObj.NursingFreqId : 0);
    let orderType = (this.myform.value.literal == true && this.myform.value.treatment == true) ? 2 : (this.myform.value.literal == true && this.myform.value.treatment == false) ? 4 : (this.myform.value.literal == false && this.myform.value.treatment == true) ? 5 : 1;
    if (orderType != 4 && orderFreqId.toString() != freqId.toString() && (this.fieldsChangesDcFlag==""|| this.fieldsChangesDcFlag!="NO")) {
      this.modalFieldsChangesDcConfirmation = true;
    }
    else{
      this.dataservice.post(this.config.Emar_Orders_InsertupdateHOA, this.hoaObj)
        .subscribe(res => {
          if (res != null) {
            this.modalHOAIsOpen = false;
            this.hoaObj = null;
            this.alertService.success("Schedule Times Updated Successfully");
            this.myform.patchValue({
              schduleText: res,
            });
            this.btnHoaSaveFlag=1;
          }
        }, error => {
          this.modalHOAIsOpen = false
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
    this.ng4LoadingSpinnerService.hide();
  }
  hoaOpen() {
    this.ng4LoadingSpinnerService.show();
    if (this.hoaObj == null)
      this.GetFrequencyMasterData();
    this.modalHOAIsOpen = true;
    this.ng4LoadingSpinnerService.hide();
  }
  GetFrequencyMasterData() {
    this.dataservice.get<FrequencyMasterDataWithShifts[]>(this.config.Emar_Orders_GetFrequencyMasterDataWithShifts + this.nurseStationId)
      .subscribe(res => {
        this.frequencyList = res;
        this.GetHoursMasterData();
        this.GetWeekMasterData();
        this.GetMonthMasterData();
        this.GetScheduleTimeDetails();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetWeekMasterData() {
    this.dataservice.get<WeekMasterData[]>(this.config.Emar_Orders_GetWeekMasterData)
      .subscribe(res => {
        this.weeksList = res;
        this.dropdownSettings_Week = {
          singleSelection: false,
          idField: 'Week_Id',
          textField: 'Week_Desc',
          itemsShowLimit: 1,
          allowSearchFilter: true,
          enableCheckAll: false,

        };
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetMonthMasterData() {
    this.dataservice.get<MonthMasterData[]>(this.config.Emar_Orders_GetMonthMasterData)
      .subscribe(res => {
        this.monthsList = res;
        this.dropdownSettings_Month = {
          singleSelection: false,
          idField: 'Month_Id',
          textField: 'Month_Name',
          itemsShowLimit: 1,
          allowSearchFilter: true,
          enableCheckAll: false,
        };
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetHoursMasterData() {
    this.dataservice.get<HoursMasterData[]>(this.config.Emar_Orders_GetHoursDataByNSId + this.nurseStationId + "/" + 0)
      .subscribe(res => {
        this.hoursList = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetScheduleTimeDetails() {
    this.ng4LoadingSpinnerService.show();
      this.resetScheduleForm();
      this.dataservice.get<DrugAdministrationTime>(this.config.Emar_Orders_GetScheduleTimeDetails + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId)
        .subscribe(res => {
          this.scheduleTextObj=res;
          if (res != null)
            this.fetchScheduleData(res);
          else {
            this.selectedWeeks=[];
            this.selectedMonths=[];
            this.scheduleform.patchValue({
              week: this.selectedWeeks,
              month: this.selectedMonths,
              monday: true,
              tuesday: true,
              wednesday: true,
              thursday: true,
              friday: true,
              saturday: true,
              sunday: true,
            });
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    resetScheduleForm() {
      this.scheduleform.reset();
      this.getDaysDropData();
      this.GetMonthMasterData();
      this.GetWeekMasterData();
      this.selectedDays = [];
      this.selectedMonths=[];
      this.selectedWeeks=[];
      this.timesArray = [];
      this.timeform.reset();
      this.isShiftSchedule = false;
      this.isHoursReadOnly = false;
      this.isdaysDisabled=false;
      this.isWeeksDisabled=false;
      this.resetMonthsDropSettings(13);
      this.resetWeekDropSettings(13);
      this.selectedfrequencyItems = [];
      this.scheduleform.patchValue({
        frequency: this.selectedfrequencyItems,
        either: this.selectedstItems,
        week: this.selectedWeeks,
        month: this.selectedMonths,
        mon: true,
        tue: true,
        wed: true,
        thu: true,
        fri: true,
        sat: true,
        sun: true,
      });
      this.hoaObj = null;
      this.scheduleTextObj=null;
    }
fetchScheduleData(res: DrugAdministrationTime) {
      debugger
      //Days are not loading so loading again
      this.getDaysDropData();
      this.GetMonthMasterData();
      this.GetWeekMasterData();
      this.dAdminId = res.DAdminId;
      this.timesArray = [];
      this.isdaysDisabled=false;
      this.isWeeksDisabled=false;
      this.timesArray = res.HoursList == null ? [] : res.HoursList;
      if (this.timesArray.length > 1)
        this.isHoursReadOnly = true;
      else
        this.isHoursReadOnly = false;
      let activeDays = [];
      let selectWeek=[];
      if(res.WeekId!=undefined && res.WeekId!=null && res.WeekId!="")
    {
      this.selectedWeeks = res.WeekId.toString().split(',').map(Number);
      selectWeek = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
      this.isdaysDisabled=true;
      if(selectWeek.filter(ite=>ite.Week_Id==1).length!=0)
      {
        this.onWeekSelect(selectWeek.filter(ite=>ite.Week_Id==1)[0]);
      }
      else{
        this.resetWeekDropSettings(13);
      }
    }
    else {
      this.resetWeekDropSettings(13);
    }
    if((res.WeekId==undefined || res.WeekId==null || res.WeekId=="") && res.Days != "") {
      this.selectedDays = res.Days.toString().split(',').map(Number);
      activeDays = this.monthDays.filter(item => this.selectedDays.includes(item.item_id));
      this.isWeeksDisabled=true;
    }
    let selectMonths=[];
    if(res.MonthId!=undefined && res.MonthId!=null && res.MonthId!="")
    {
      this.selectedMonths = res.MonthId.toString().split(',').map(Number);
      selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
      if (selectMonths.filter(ite => ite.Month_Id == 13).length != 0) {
        this.onMonthSelect(selectMonths.filter(ite => ite.Month_Id == 13)[0]);
      }
      else{
        this.resetMonthsDropSettings(13);
      }
    }
    else{
      this.resetMonthsDropSettings(13);
    }
      this.selectedstItems = [];

      if (res.HoursList != null && this.hoursList != undefined && this.hoursList != null) {
        this.selectedstItems.push(this.hoursList.filter(h => h.Hour_Id == res.HoursList[0].Hour_Id)[0]);
      }
      this.selectedfrequencyItems = [];
      if (res.NursingFreqId != null && this.frequencyList != undefined && this.frequencyList != null) {
        this.selectedfrequencyItems.push(this.frequencyList.filter(f => f.Frequency_Id == res.NursingFreqId.toString())[0]);
        if (res.NursingFreqId == 28) {
          this.selectedstItems = null;
          this.isShiftSchedule = true;
          this.isHoursReadOnly = true;
          this.timesArray = [];
        }
        if (this.frequencyList.find(f=>f.Frequency_Id== res.NursingFreqId.toString() )!=undefined && this.frequencyList.find(f=>f.Frequency_Id== res.NursingFreqId.toString() ).Frequency_PRN == 1) {
          this.myform.patchValue({
            prn: true
          });
          const maxpervalidation = this.myform.get('maxPerDay');
          maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
          Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          Validators.pattern(/^\d*(\.\d{0,3})?$/),
          Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
            maxpervalidation.updateValueAndValidity();
            this.selectedstItems = [];
            this.isShiftSchedule = true;
            this.isHoursReadOnly = true;
            this.timesArray = [];
         }
          else
          {
            // this.myform.patchValue({
            //   prn:false,
            // });
            const maxpervalidation = this.myform.get('maxPerDay');
            maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
            maxpervalidation.updateValueAndValidity();
          }
      }
      else if (res.NurseShiftsId != null) {
        let checkExist=this.frequencyList.find(f => f.Frequency_Id == 's' + res.NurseShiftsId);
        if(checkExist!=undefined)
        {
        this.selectedfrequencyItems.push(this.frequencyList.filter(f => f.Frequency_Id == 's' + res.NurseShiftsId)[0]);
        this.selectedstItems = null;
        this.isShiftSchedule = true;
        this.isHoursReadOnly = true;
        this.timesArray = [];
      }
      }
      this.scheduleform.patchValue({
        frequency: this.selectedfrequencyItems,
        either: this.selectedstItems,
        hours: res.Hours == null ? '' : res.Hours,
        mon: res.Monday,
        tue: res.Tuesday,
        wed: res.Wednesday,
        thu: res.Thursday,
        fri: res.Friday,
        sat: res.Saturday,
        sun: res.Sunday,
        week: selectWeek,//res.WeekId == null ? 0 : res.WeekId,
        month: selectMonths, //res.MonthId == null ? 0 : res.MonthId,
        // oDay: res.OnlyOnDay,
        // through: res.ThroughDay,
        aDay: res.ActiveDays,
        hDay: res.HoldDays,
        days: activeDays
      });
      this.ng4LoadingSpinnerService.hide();
}
onFrequencyChange(item: any) {
  if (this.selectedfrequencyItems.length != 0) {
    let frequencyId = this.scheduleform.value.frequency[0].Frequency_Id;
    if (frequencyId.startsWith('s') || frequencyId == 28  || this.frequencyList.find(f=>f.Frequency_Id==frequencyId).Frequency_PRN==1) {
      this.isShiftSchedule = true;
      this.isHoursReadOnly = true;
      this.selectedstItems = [];
      this.scheduleform.patchValue({
        either: this.selectedstItems,
        hours: ''
      });
      this.timesArray = [];
    }
    else {
      this.isShiftSchedule = false;
      this.isHoursReadOnly = false;
      this.isWeeksDisabled=false;
      this.isdaysDisabled=false;
      this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any>(this.config.Emar_Orders_GetNurseFrequencyDropSelect + frequencyId + '/' + this.nurseStationId)
        .subscribe(res => {
          this.fetchFrequencyMapData(frequencyId, res);
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  else if (this.selectedfrequencyItems.length == 0) {
    this.hoaObj = null;
    this.isShiftSchedule = false;
    this.isHoursReadOnly = false;
    this.isdaysDisabled=false;
    this.isWeeksDisabled=false;
    this.selectedMonths=[];
    this.selectedWeeks=[];
    this.scheduleform.reset();
    this.timeform.reset();
    this.getDaysDropData();
    this.selectedDays = [];
    this.timesArray = [];
    this.selectedfrequencyItems = [];
    this.selectedstItems = [];
    this.scheduleform.patchValue({
      frequency: this.selectedfrequencyItems,
      either: this.selectedstItems,
      week: this.selectedMonths,
      month: this.selectedMonths,
      monday: true,
      tuesday: true,
      wednesday: true,
      thursday: true,
      friday: true,
      saturday: true,
      sunday: true,
      // oDay: 0,
      // through: 0,
      //days: 0
    });
  }
}
fetchFrequencyMapData(frequencyId: any, res: any) {

  if (res != null) {
    this.timesArray = [];
    this.getDaysDropData();
    this.GetMonthMasterData();
    this.GetWeekMasterData();
    this.scheduleform.reset();
    this.timeform.reset();
    this.timesArray = res.HoursList == null ? [] : res.HoursList;
    if (this.timesArray.length > 1) {
      this.isHoursReadOnly = true;
    }
    else {
      this.isHoursReadOnly = false;
    }
    this.selectedfrequencyItems = [];
    if (frequencyId != 0) {
      this.selectedfrequencyItems.push(this.frequencyList.filter(f => f.Frequency_Id == frequencyId)[0]);
    }
    this.selectedstItems = [];
    if (this.timesArray.length > 0) {
      this.selectedstItems.push(this.hoursList.filter(h => h.Hour_Id == res.HoursList[0].Hour_Id)[0]);
    }
    let selectMonths = [];
      if (res.MonthId!=undefined && res.MonthId!=null &&  res.MonthId!= "") {
        this.selectedMonths = res.MonthId.toString().split(',').map(Number);
        selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
      }
      let selectWeeks = [];
      if (res.WeekId!=undefined && res.WeekId!=null && res.WeekId!= "") {
        this.selectedWeeks = res.WeekId.toString().split(',').map(Number);
        selectWeeks = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
        this.selectedDays=[];
        this.isdaysDisabled=true;
      }
    this.scheduleform.patchValue({
      frequency: this.selectedfrequencyItems,
      either: this.selectedstItems,
      hours: (res.Hours == null  || res.Hours==0)? '' : res.Hours,
      mon: res.Monday,
      tue: res.Tuesday,
      wed: res.Wednesday,
      thu: res.Thursday,
      fri: res.Friday,
      sat: res.Saturday,
      sun: res.Sunday,
      week: selectWeeks,//res.WeekId == null ? 0 : res.WeekId,
      month: selectMonths,//res.MonthId == null ? 0 : res.MonthId,
      aDay: res.ActiveDays,
      hDay: res.HoldDays,
    });
    if (frequencyId == 28) {
      this.isShiftSchedule = true;
      this.isHoursReadOnly = true;
      this.selectedstItems = [];
      this.scheduleform.patchValue({
        either: this.selectedstItems,
        hours: ''
      });
      this.timesArray = [];
    }
  }
  else {
    this.timeform.reset();
    this.getDaysDropData();
    this.GetMonthMasterData();
    this.GetWeekMasterData();
    this.selectedDays = [];
    this.timesArray = [];
    this.selectedstItems = [];
    this.selectedWeeks=[];
    this.selectedMonths=[];
    this.scheduleform.patchValue({
      either: this.selectedstItems,
      week: this.selectedWeeks,
      month: this.selectedMonths,
      monday: true,
      tuesday: true,
      wednesday: true,
      thursday: true,
      friday: true,
      saturday: true,
      sunday: true,
    });
  }
}
addTimes() {
  if (this.timeform.value.starttime == null || this.timeform.value.starttime == undefined || this.timeform.value.starttime == '') {
    this.alertService.warn("Please select next time");
  }
  else if (this.timeform.value.starttime != null || this.timeform.value.starttim !== undefined || this.timeform.value.starttime != '') {
    let hourId = this.timeform.value.starttime[0].Hour_Id;
    let hourDesc = this.hoursList.find(h => h.Hour_Id == parseInt(hourId)).Hour_Desc;
    let result = this.timesArray.find(t => t.Hour_Id == parseInt(hourId));
    if (result != undefined) {
      this.alertService.warn("Selected time already exists.")
    }
    else {
      if (this.timesArray.length >= 1 && parseInt(hourId) <= this.timesArray[0].Hour_Id) {
        this.alertService.warn("Selected time cannot be before start time");
      }
      else {
        let timeObj: any = {
          Hour_Id: hourId,
          Hour_Desc: hourDesc,
        }
        this.timesArray.push(timeObj);
        if (this.startTimeArray == 1) {
          this.scheduleform.patchValue({
            either: this.timesArray,
          });
          this.startTimeArray = 0;
        }

        this.timeform.reset();
        if (this.timesArray.length > 1) {
          this.isHoursReadOnly = true;
        }
        else {
          this.isHoursReadOnly = false;
        }
      }
    }
  }
}
removeTime(i: number) {
  if (i == 0) {
    this.timesArray = [];
    this.timesArray.splice(i, 1);
    this.scheduleform.controls['either'].reset();
    this.startTimeArray = 1;
  }
  else {
    this.timesArray.splice(i, 1);
  }
  if (this.timesArray.length > 1) {
    this.isHoursReadOnly = true;
  }
  else {
    this.isHoursReadOnly = false;
  }
}
timemodalopen() {
  if (this.scheduleform.value.either == '' || this.scheduleform.value.either == null || this.scheduleform.value.either == undefined) {
    this.alertService.warn("Please select start time.");
  }
  else if ((this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null) && (this.scheduleform.value.either != undefined || this.scheduleform.value.either != "0")) {
    this.alertService.warn("Hours already given. You can't give multiple times.");
  }
  else if (((this.scheduleform.value.hours == '' || this.scheduleform.value.hours == null || this.scheduleform.value.hours == undefined) && this.timesArray.length != 0) || (this.scheduleform.value.either != '0')) {
    this.modalTimeIsOpen = true;
    this.timeform.reset();
  }
}
addstartTime() {
  this.timesArray = [];
  if (this.selectedstItems == undefined || this.selectedstItems == null || this.selectedstItems.length == 0)
    this.timesArray = [];
  else {
    let starthourId = this.scheduleform.value.either[0].Hour_Id;
    let starthourDesc = this.hoursList.find(h => h.Hour_Id == parseInt(starthourId)).Hour_Desc;
    let timeObj: any = {
      Hour_Id: starthourId,
      Hour_Desc: starthourDesc,
    }
    this.timesArray.push(timeObj);
  }
}
onDaysSelect(item: any) {
  this.selectedDays.push(item);
  this.selectedWeeks=[];
    this.scheduleform.patchValue({
      week:this.selectedWeeks,
    })
    this.isWeeksDisabled=true;
}
onDaysDeSelect(item: any) {
  var index = this.selectedDays.indexOf(item);
  this.selectedDays.splice(index, 1);
  this.selectedWeeks=[];
    this.scheduleform.patchValue({
      week:this.selectedWeeks,
    });
  if (this.selectedDays.length == 0) {
    this.isWeeksDisabled = false;
  }
}
onMonthSelect(item: any) {

  if(item.Month_Id==13)
  {
    this.selectedMonths=[];
    this.selectedMonths.push(item.Month_Id);
    let selectMonths = [];
    selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
    this.scheduleform.patchValue({
      month:selectMonths,
    });
    this.dropdownSettings_Month = {
      singleSelection: false,
      idField: 'Month_Id',
      textField: 'Month_Name',
      itemsShowLimit: 1,
      allowSearchFilter: true,
      enableCheckAll: false,
      limitSelection: 1,
    };
  }
  else
  {
    this.selectedMonths.push(item.Month_Id);
  }
}
onMonthDeSelect(item: any) {
  var index = this.selectedMonths.indexOf(item.Month_Id);
  this.selectedMonths.splice(index, 1);
    this.dropdownSettings_Month = {
      singleSelection: false,
      idField: 'Month_Id',
      textField: 'Month_Name',
      itemsShowLimit: 1,
      allowSearchFilter: true,
      enableCheckAll: false,
      limitSelection: 13,
  }
}
onWeekSelect(item: any) {
  if(item.Week_Id==1)
  {
    this.selectedWeeks = [];
    this.selectedWeeks.push(item.Week_Id);
    let selectWeek = [];
    selectWeek = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
    this.isdaysDisabled=true;
    this.scheduleform.patchValue({
      week: selectWeek,
    });
    this.dropdownSettings_Week = {
      singleSelection: false,
      idField: 'Week_Id',
      textField: 'Week_Desc',
      itemsShowLimit: 1,
      allowSearchFilter: true,
      enableCheckAll: false,
      limitSelection:1,
    };
  }
  else{
  this.selectedWeeks.push(item.Week_Id);
  this.selectedDays=[];
  this.scheduleform.patchValue({
    days:this.selectedDays,
  });
  this.isdaysDisabled=true;
}
}
onWeekDeSelect(item: any) {
  var index = this.selectedWeeks.indexOf(item.Week_Id);
  this.selectedWeeks.splice(index, 1);
  this.selectedDays=[];
  this.scheduleform.patchValue({
    days:this.selectedDays,
  });
  if (this.selectedWeeks.length == 0) {
    this.isdaysDisabled = false;
  }
  this.dropdownSettings_Week = {
    singleSelection: false,
    idField: 'Week_Id',
    textField: 'Week_Desc',
    itemsShowLimit: 1,
    allowSearchFilter: true,
    enableCheckAll: false,
    limitSelection:6,
  };
}
cancelHOAChanges() {
  this.modalHOAIsOpen = false;
}
//Favourities methods
FavouriteChange(FavMasterID: number, event, FavID: number) {
  debugger
  this.fav = FavMasterID;
  if (event == true) {
    let objFav = new OrderFavourite();
    objFav = {
      OrderFavourite_ID: FavID,
      PQuantity_Id: this.quantityId,
      OrderFavMaster_ID: FavMasterID,
      OrderFavourite_Status: 1,
      OrderFavourite_Createby: this.userID,
      OrderFavourite_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
      FavListCheckFlag:0
    }
    this.favobj.push(objFav.OrderFavMaster_ID);
    this.favouriteObj.push(objFav);
  }
  else if (FavID != 0 && event == true) {
    let objFav = new OrderFavourite();
    objFav = {
      OrderFavourite_ID: FavID,
      PQuantity_Id: this.quantityId,
      OrderFavMaster_ID: FavMasterID,
      OrderFavourite_Status: 1,
      OrderFavourite_Createby: this.userID,
      OrderFavourite_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
      FavListCheckFlag:0
    }
    this.favobj.push(objFav.OrderFavMaster_ID);
    this.favouriteObj.push(objFav);
  }
  else {
    let index = this.favouriteObj.findIndex(fav => fav.OrderFavMaster_ID == FavMasterID);
    this.favouriteObj.splice(index, 1);
    let favindex = this.favobj.findIndex(f => f === FavMasterID);
    this.favobj.splice(favindex, 1);
  }
}
InsertOrderFavourities() {
  if (this.residentId > 0) {
    if (this.demographicInfoData.PVisit_Status == 2) {
      this.alertService.warn("Resident has been discharged.")
    }
    else if (this.demographicInfoData.PVisit_Status == 3) {
      this.alertService.warn("Status updated to temporarily inactive.")
    }
    else {
      this.ng4LoadingSpinnerService.show();
      if(this.favouriteObj.length ==0 )
      {
        let objFav = new OrderFavourite();
        objFav = {
        OrderFavourite_ID: 0,
        PQuantity_Id: this.quantityId,
        OrderFavMaster_ID: 0,
        OrderFavourite_Status: 1,
        OrderFavourite_Createby: this.userID,
        OrderFavourite_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        FavListCheckFlag:1,
     }
     this.favobj.push(objFav.OrderFavMaster_ID);
     this.favouriteObj.push(objFav);
      }
        this.dataservice.post(this.config.Emar_Orders_InsertOrderFavourities, this.favouriteObj)
          .subscribe(res => {
            this.getFavouritesMasterData();
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Measurements and Other checks Save successful");
            this.modalfavIsOpen = false;
            this.favouriteObj = [];
            //this.favstatus = 1;
          }, error => {
            this.modalfavIsOpen = false;
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(error.message);
          });
      // else {
      //   this.ng4LoadingSpinnerService.hide();
      //   this.alertService.warn("No records selected");
      // }
    }
  }
  else {
    this.alertService.error("No Resident selected");
  }
}
modalfav() {
  this.getFavouritesMasterData();
  this.modalfavIsOpen = true;
}
getFavouritesMasterData() {
  this.ng4LoadingSpinnerService.show();
  this.dataservice.get<any[]>(this.config.Emar_Orders_GetFavouritesMasterData + this.quantityId + '/' + this.barcodeFacilityId)
    .subscribe(res => {
        this.favouriteMasterList = res;
        this.favouriteObj = [];
        this.favobj = [];
        let favouritestatus = this.favouriteMasterList.filter(r => r.OrderFavourite_ID != 0);

        if (favouritestatus.length >= 1 && this.favouriteMasterList != undefined && this.favouriteMasterList != null) {
          this.favouriteObj = this.favouriteMasterList.filter(r => r.OrderFavourite_ID != 0);
          this.favouriteObj.forEach(element => {
            this.favobj.push(element.OrderFavMaster_ID);
          });
          this.favstatus = 1;
        }
        else if (favouritestatus.length == 0) {
          this.favstatus = 0;
        }
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.error(error.message);
    });
}
//All modal open close methods
closeModel() {
  this.modalfavIsOpen = false;
  this.modalInsuliIsOpen = false;
  this.modalHOAIsOpen = false;
  this.modalnoteIsOpen = false;
}
insulinOpen() {
  this.modalInsuliIsOpen = true;
}
modalnote() {
  this.modalnoteIsOpen = true;
}
searchDrugNames(searchText: string) {
  if (this.isReadOnly == true) {
    this.alertService.warn('Drug Name search is not available for Review Completed Orders');
  }
  else {
    if (searchText.length > 2) {
      let searchData = {
        "searchText": searchText,
        "orderType": this.myform.value.type == 1 ? 'order' : 'stock',
        "nurseStationId": this.nurseStationId
      }
      this.modalOption.size = 'lg';
      const modalRef = this.modalService.open(SearchDrugNameComponent, this.modalOption);
      modalRef.componentInstance.searchDrugText = searchData;
      modalRef.componentInstance.searchResult.subscribe((receivedResult) => {
        if (receivedResult.orderType == 'stock') {
          this.onselectDrug(receivedResult.selectedItem);
        }
        else if (receivedResult.orderType == 'order') {
          this.onselectDrugFromDrugDB(receivedResult.selectedItem);
        }
        modalRef.close();
      })
    }
    else
      this.alertService.warn('Please enter at least 3 characters of drug name');
  }
  this.flag = false;
}
onselectDrug(item: any) {
  if (item != '') {
    this.barcodear = [];
    this.drugNameChanged = false;
    this.stockId = item.Stock_Id;
    this.gpiCode = item.GPICode;
    if (item.Barcode != "")
      this.barcodear = item.Barcode.split(', ');
    this.myform.patchValue({
      drug: item.DrugName,
      qtyHand: item.InHand,
      controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
      type: 0
    });
    if (item.ControlledSubstanceSchedule == 1)
      this.isControlSubstanceReadOnly = true;
    else
      this.isControlSubstanceReadOnly = false;
    if (this.barcodear != null && this.barcodear.length > 0) {
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators(null);
      barcodevalidation.clearValidators();
      barcodevalidation.updateValueAndValidity();
      this.barCodeFlag=0;
    }
  }
  else {
    return false;
  }
}
onselectDrugFromDrugDB(item: any) {
  if (item != '') {
    this.barcodear = [];
    this.stockId = item.Drug_Id;
    this.gpiCode = item.GPICode;
    this.drugNameChanged = false;
    this.myform.patchValue({
      drug: item.DrugName,
      qtyHand: '',
      controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
      type: 1
    });
    this.isReadOnlyforControl = false;
    if (item.ControlledSubstanceSchedule == 1)
      this.isControlSubstanceReadOnly = true;
    else
      this.isControlSubstanceReadOnly = false;
  }
  else {
    return false;
  }
  this.flag = false;
}
onOrderTypeChange() {
  this.myform.patchValue({
    drug: '',
    qtyHand: '',
    barcode: ''
  });
  this.barcodear = [];
  this.gpiCode = '';
  this.drugNameChanged = true;
  this.stockId = 0;
  this.isReadOnlyforControl = true;
  this.loadSearchData();
}
checkPRN(value: any) {
  if (value == true) {
    this.prnSchedule();
    const maxpervalidation = this.myform.get('maxPerDay');
    maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
    maxpervalidation.updateValueAndValidity();
  }
  else if (value == false) {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.get<DrugAdministrationTime>(this.config.Emar_Orders_GetScheduleTimeDetails + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId)
        .subscribe(res => {
          if (res != null) {
            // if (res.NursingFreqId == 1) {
              if(this.frequencyList.find(f=>f.Frequency_Id==res.NursingFreqId.toString()).Frequency_PRN==1){
              this.alertService.warn('Frequency is set to PRN. Change Frequency in order to change PRN field');
              this.myform.patchValue({
                prn: true
              });
              const maxpervalidation = this.myform.get('maxPerDay');
              maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
              maxpervalidation.updateValueAndValidity();
            }
            else {
              this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + this.orderId + "/" + this.quantityId  + "/" + this.userID)
                .subscribe(res => {
                  this.myform.patchValue({
                    schduleText: res.Schedule != "Select Time" ? res.Schedule : '',
                  });
                  this.hoaObj = null;
                }, error => {
                  this.alertService.error(error.message);
                });
            }
          }
          else {
            this.myform.patchValue({
              schduleText: ''
            });
            this.hoaObj = null;
            this.selectedfrequencyItems = [];
            this.scheduleform.patchValue({
              frequency: this.selectedfrequencyItems,
            });
          }
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    const maxpervalidation = this.myform.get('maxPerDay');
    maxpervalidation.setValidators(null);
    maxpervalidation.clearValidators();
    maxpervalidation.updateValueAndValidity();
  }
}
prnSchedule() {
  if (this.hoaObj == null ) {
    this.dataservice.get<FrequencyMasterDataWithShifts[]>(this.config.Emar_Orders_GetFrequencyMasterDataWithShifts + this.nurseStationId)
      .subscribe(res => {
        this.frequencyList = res;
        this.GetHoursMasterData();
        this.GetWeekMasterData();
        this.GetMonthMasterData();
        //this.GetScheduleTimeDetails();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  if (this.hoaObj == null || this.hoaObj.freqId == null) {
    this.hoaObj = {
      dadminId: this.dAdminId,
      porderId: this.orderId,
      pquantityId: this.quantityId,
      freqId: 1,
      hourId: null,
      hourIds: '',
      //timeformatId: this.scheduleform.value.timeFormat == 0 ? null : this.scheduleform.value.timeFormat,
      hours: null,
      monday: this.scheduleform.value.mon,
      tuesday: this.scheduleform.value.tue,
      wednesday: this.scheduleform.value.wed,
      thursday: this.scheduleform.value.thu,
      friday: this.scheduleform.value.fri,
      saturday: this.scheduleform.value.sat,
      sunday: this.scheduleform.value.sun,
      weekId: this.selectedWeeks != undefined ? (this.selectedWeeks.length > 0 ? this.selectedWeeks.join() : null) : null, //this.scheduleform.value.week == 0 ? null : this.scheduleform.value.week,
      monthId: this.selectedMonths != undefined ? (this.selectedMonths.length > 0 ? this.selectedMonths.join() : null) : null, //this.scheduleform.value.month == 0 ? null : this.scheduleform.value.month,
      days: this.selectedDays != undefined ? (this.selectedDays.length > 0 ? this.selectedDays.join() : null) : null,
      createdby: this.userID,
      activedays: this.scheduleform.value.aDay,
      holddays: this.scheduleform.value.hDay,
      NurseStationId: this.nurseStationId,
      nurseShiftId: null
    };
    this.dataservice.post(this.config.Emar_Orders_GetScheduledTimeText, this.hoaObj)
      .subscribe(res => {
        if (res != null) {
          let text = res;
          this.myform.patchValue({
            schduleText: text
          });
          this.getDaysDropData();
          this.GetMonthMasterData();
            this.GetWeekMasterData();
          this.isShiftSchedule = true;
          this.isHoursReadOnly = true;
          this.selectedstItems = [];
          this.timesArray = [];
          this.selectedfrequencyItems = [];
          this.selectedMonths=[];
          this.selectedWeeks=[];
          this.selectedfrequencyItems.push(this.frequencyList.filter(f => f.Frequency_Id == '1')[0]);

          this.scheduleform.patchValue({
            frequency: this.selectedfrequencyItems,
            either: this.selectedstItems,
            hours: '',
            week: this.selectedMonths,
            month: this.selectedWeeks,
            mon: true,
            tue: true,
            wed: true,
            thu: true,
            fri: true,
            sat: true,
            sun: true,
          });

          this.modalHOAIsOpen = false;
          if(this.myform.value.prn==true)
          {
            //this.insertScheduleTimes();
            this.btnHoaSaveFlag=2;
          }
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
          this.modalHOAIsOpen = false
        });
  }
}
addBarcode(value: any) {
  if(this.barCodeStatusFlag !=1 && this.myform.value.orderTypeId!=4)
    {
      this.barCodeFlag = 1;
    }
    if (value != '') {
      let barcode = this.myform.value.barcode;
      let barcodevalue = (barcode.split('/'))[0];
      //this.BarcodeValue = barcodevalue[0];
      if (barcodevalue != '') {
        // console.log(barcode);
        // console.log("Barcode list - "+ this.barcodesList);
        if(this.myform.value.type == 1)
        {
          let result1 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode!=null?x.GPICode.toLowerCase()==this.gpiCode.toLowerCase():false));
          let result2 = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false);
          let result3 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode!=null?x.GPICode.toLowerCase()==this.gpiCode.toLowerCase():false) &&(x.Patient_Id!=null?x.Patient_Id!=this.residentId:false));
          //let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcode.toLowerCase() : false);
          // this.alertService.warn('r' + result1);
          // let result = this.barcodesList.find(x => x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, ''));
          let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;
          if ((result2.length>0 && result1.length == 0)|| result3.length>0 || checkInBarcodeArray != undefined) {
            //this.alertService.error("Barcode Already Exists");
            if((this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode!=null?x.GPICode.toLowerCase()!=this.gpiCode.toLowerCase():false)&& x.Patient_Id!=null?x.Patient_Id==this.residentId:false).length>0))
            {
              this.alertService.warn("Barcode already assigned to a different medication");
            }
            else if(checkInBarcodeArray!=undefined)
            {
              this.alertService.warn("Barcode already assigned to the same medication for the same resident");
            }
            else {
              //this.alertService.warn("This barcode is assigned to an order for another resident");
              this.alertService.warn("Barcode already assigned to an order");
            }
            this.myform.patchValue({
              barcode: ''
            });
            if(this.barCodeStatusFlag !=1 && this.myform.value.orderTypeId!=4)
            {
              this.barCodeFlag = 1;
            }
          }
          else {
            this.barcodear.push(barcodevalue);
            this.barCodeFlag = 0;
            this.myform.patchValue({
              barcode: '',
            })
          }
        }
        else
        {
          if(this.gpiCode!='')
          {
            let result1 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode!=null?x.GPICode.toLowerCase()==this.gpiCode.toLowerCase():false));
            let result2 = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false);
            let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;
            if ((result2.length > 0 && result1.length==0)) {
              //this.alertService.error("Barcode Already Exists");
              this.alertService.warn("Barcode already assigned to a different medication");
              this.myform.patchValue({
                barcode: ''
              });
              if(this.barCodeStatusFlag !=1 && this.myform.value.orderTypeId!=4)
              {
                this.barCodeFlag = 1;
              }
            }
            else if(checkInBarcodeArray != undefined)
            {
              this.alertService.warn("Barcode already assigned to the same drug");
              this.myform.patchValue({
                barcode: ''
              });
              if(this.barCodeStatusFlag !=1 && this.myform.value.orderTypeId!=4)
              {
                this.barCodeFlag = 1;
              }
            }
            else {
              this.barcodear.push(barcodevalue);
              this.barCodeFlag = 0;
              this.myform.patchValue({
                barcode: '',
              })
            }
          }
          else
          {
            this.alertService.error("Please select drug");
          }
        }
        }

    }
    if (this.barcodear != null && this.barcodear.length !=0) {
      if(this.barCodeStatusFlag !=1)
      {
      this.barCodeFlag = 0;
      }
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators(null);
      barcodevalidation.clearValidators();
      barcodevalidation.updateValueAndValidity();

    }
}
removeBarcode(i: number) {

  this.barcodear.splice(i, 1);
  if(this.barCodeStatusFlag !=1)
      {
      this.barCodeFlag = 1;
      }
  if (this.barcodear.length == 0) {
    const barcodevalidation = this.myform.get('barcode');
    barcodevalidation.setValidators([Validators.required]);
    barcodevalidation.updateValueAndValidity();

  }
}
updateOrderReviewClick(reviewClicked: number) {
  debugger
  //this.clicked = true;
  // if (this.demographicInfoData.PVisit_Status == 2) {

  //   //this.alertService.warn("Resident has been discharged.")
  // }
  // else if (this.demographicInfoData.PVisit_Status == 3) {
  //   this.alertService.warn("Status updated to temporarily inactive.")
  // }
  // else {
    let prnCheck=this.myform.value.prn;
    let prnScheduleCheck=this.myform.value.schduleText;
    this.reviewClickedFlag = reviewClicked;
    let drugName = this.myform.value.drug == null ? '' : this.myform.value.drug;
    if (this.myform.value.physician == 0 || this.myform.value.physician == null || this.myform.value.physician == undefined)
      this.alertService.warn("Please select physician");
    else if (this.stockId == 0 && this.myform.value.type == false)
      this.alertService.warn("Drug name doesn't exist in stock");
    else if (this.drugNameChanged == true && this.myform.value.orderTypeId == 1)
      this.alertService.warn("Please select Drug name from the list");
    else if (this.myform.value.orderTypeId !=4 && (this.myform.value.route == 0 || this.myform.value.route == null || this.myform.value.route == undefined))
      this.alertService.warn("Please select route");
    else if (this.myform.value.startDate == 0)
      this.alertService.warn("Please select start date");
    else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.startDate)
      this.alertService.warn("End date cannot be before start date");
    else if (this.barcodear == null && this.myform.value.orderTypeId != 4)
      this.alertService.warn("Barcode required");
    else if (this.barcodear.length == 0 && this.myform.value.orderTypeId != 4)
      this.alertService.warn("Barcode required");
    else if ((this.hoaObj == null || this.hoaObj == undefined) && (this.myform.value.schduleText == '' || this.myform.value.schduleText == null) && this.myform.value.orderTypeId != 4)
      this.alertService.warn("Please enter schedule times");


      else if (this.medispanControlSubBit == 0 && this.controlsubstanceBit == 1 && this.orderOrigin == 'P') {
        if(this.myform.value.controlSubstance == false){
          this.controlledSubstancemessage ='You have unchecked controlled substance checbox. Are you sure this is not a controlled med?';
          this.modalControlIsOpen = true;
        }
        else {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
          this.prnSchedleCheckModal=true;
        }
        else
          this.saveExistingOrder(1);
      }
    }
    else if(this.medispanControlSubBit == 0 && this.controlsubstanceBit == 0 && this.orderOrigin == 'P')
      {
        if(this.myform.value.controlSubstance == true){
          this.controlledSubstancemessage ='Order requires controlled substance count certification';
          this.modalControlIsOpen = true;
        }
        else {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
          {
            this.prnSchedleCheckModal=true;
          }
          else
          this.saveExistingOrder(0);
      }
      }
      else if(this.medispanControlSubBit == 0 && this.controlsubstanceBit == 1 && this.orderOrigin == 'M')
      {
        if(this.myform.value.controlSubstance == false){
          this.controlledSubstancemessage ='You have unchecked controlled substance checbox. Are you sure this is not a controlled med?';
          this.modalControlIsOpen = true;
        }
        else  {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
          this.prnSchedleCheckModal=true;
        }
        else
          this.saveExistingOrder(1);
        }
      }
      else if(this.medispanControlSubBit == 0 && this.controlsubstanceBit == 0 && this.orderOrigin == 'M')
      {
        if(this.myform.value.controlSubstance == true){
          this.controlledSubstancemessage ='Order requires controlled substance count certification';
          this.modalControlIsOpen = true;
        }
        else  {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
          {
            this.prnSchedleCheckModal=true;
          }
          else
          this.saveExistingOrder(0);
        }
      }
      else  {
        if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
          this.prnSchedleCheckModal=true;
        }
        else
      this.saveExistingOrder(0);
    }
  //}
}
CompletePRNReview()
{
  this.prnSchedleCheckModal=false;
  this.saveExistingOrder(0);
}
OpenHOAForPRN()
{
  this.prnSchedleCheckModal=false;
  this.modalHOAIsOpen=true;
}
saveExistingOrder(controlSubstanceFlag: number) {
  controlSubstanceFlag=this.myform.value.controlSubstance==true?1:0;
  let barcode = this.barcodear.join();
  if(this.myform.value.prn==true && this.btnHoaSaveFlag==2)
  {
    this.insertScheduleTimes();
  }
  if (this.fieldsChangesDcFlag == "" && this.checkIsFieldsChanged() == 1) {
    this.ng4LoadingSpinnerService.hide();
    this.modalFieldsChangesDcConfirmation = true;
  }
  else{
  this.orderUpdateObj = {
    PatientId:this.residentId,
    POrderId: this.orderId,
    PQuantityId: this.quantityId,
    PhysicianId: this.myform.value.physician[0].Physician_Id,
    DrugName: this.myform.value.drug,
    Quantity: this.myform.value.dose,
    Directions: this.myform.value.addInst,
    StartDate: this.dateFormatPipe.transform(this.myform.value.startDate),
    EndDate: this.myform.value.endDate == "" ? null : this.dateFormatPipe.transform(this.myform.value.endDate),
    NumberofRefills: this.myform.value.refill,
    MaxPerdays: this.myform.value.maxPerDay,
    Alerttext: this.myform.value.alertText,
    InsulinComments: this.myform.value.insulincomments,
    OrderstockFlag: this.myform.value.type,
    PRNFlag: this.myform.value.prn,
    TreatmentFlag: this.myform.value.treatment,
    SelfAdministeredFlag: this.myform.value.self,
    controlSubstance: controlSubstanceFlag,
    Route: this.myform.value.route.length != 0 ? this.myform.value.route[0].Route_Id : '',
    Inhand: this.myform.value.qtyHand,
    Createdby: this.userID,
    Barcode: barcode.replace(/\s/g, ""),
    DAdminId: this.dAdminId,
    RequestedGiveCode: this.gpiCode,
    OrderTypeID:(this.myform.value.literal==true && this.myform.value.treatment==true)?2:(this.myform.value.literal==true && this.myform.value.treatment==false)?4:(this.myform.value.literal==false && this.myform.value.treatment==true)?5:1,//this.myform.value.orderTypeId,
    OrderUpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
    ScheduleText:this.myform.value.schduleText,
  };
  this.dataservice.post(this.config.Emar_Orders_UpdateOrdersDatabyOrderId, this.orderUpdateObj)
    .subscribe(res => {
      if (this.reviewClickedFlag == 1) {
        this.orderStatusObj = {
          PatientId: this.residentId,
          OrderId: this.orderId,
          DAdminId: this.dAdminId,
          QuantityId: this.quantityId,
          POrderCreatedBy: this.userID,
          POrderStatus: 0,
          POOutBoundApproval: 1,
          POOutBoundApprovalBy: this.userID,
          OrderType: 'Review',
          UpdatedOn: this.dateFormatPipe.dateWithTime(new Date())
        };
        this.dataservice.post(this.config.Emar_Orders_InsertDiscountiueStatus, this.orderStatusObj)
          .subscribe(res => {
            this.reviewInfoResult.emit(1);
            this.reviewClickedFlag = 0;
          }, error => {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(error.message);
          });
      }
      this.reviewInfoResult.emit(0);
      this.modalControlIsOpen = false;
    }, error => {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.error(error.message);
      this.modalControlIsOpen = false;
    });
  }
}
CompletedReviewClick(reviewflag:any)
{
  this.btnFlag=2;
  this.updateOrderReviewClick(reviewflag);
  setTimeout(() => {
      this.clicked = false;
    }, 3000);
}
saveOrder(controlSubstanceFlag: number) {
    this.saveExistingOrder(controlSubstanceFlag);
}
getDaysDropData() {
  this.monthDays = [];
  let day = 1;
  while (day <= 31) {
    this.monthDays.push({ item_id: day, item_text: day });
    day++;
  }

  this.dropdownSettings_Days = {
    singleSelection: false,
    idField: 'item_id',
    textField: 'item_text',
    itemsShowLimit: 1,
    allowSearchFilter: true,
    enableCheckAll: false,

  };
}
onDrugNameChange(drugName?:string) {
  debugger;
  this.stockId = 0;
  this.gpiCode = '';
  if(this.myform.value.druggname !=drugName)
  {
  this.drugNameChanged = true;
  }
  this.searchDrug(drugName);
}
searchDrug(term: string): void {
  if (term.length > 2&& ((this.myform.value.literal==true && this.myform.value.treatment==true) ||(this.myform.value.literal==false && this.myform.value.treatment==true)||(this.myform.value.literal==false && this.myform.value.treatment==false))) {
    this.flag = true;
    this.searchTerms.next(term);
  }
  else {
    this.flag = false;
  }
}
onselectDrugbyBoth(item:any)
  {
    debugger;
    let orderType = this.myform.value.type == true  ? 'order' : 'stock';
    if(orderType == "stock")
    {
    if (item != '') {
      this.barcodear = [];
      //this.previousDrugName = '';
      this.drugNameChanged = false;
      this.stockId = item.Stock_Id;
      this.gpiCode = item.GPICode;
      if (item.Barcode != "")
        this.barcodear = item.Barcode.split(', ');
      this.myform.patchValue({
        drug: item.DrugName,
        qtyHand: item.InHand,
        //barcode:item.Barcode
        controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
        type: 0
      });
      this.flag = false;
      if (item.ControlledSubstanceSchedule == 1)
        this.isControlSubstanceReadOnly = true;
      else
        this.isControlSubstanceReadOnly = false;
      if (this.barcodear != null && this.barcodear.length > 0) {
        const barcodevalidation = this.myform.get('barcode');
        barcodevalidation.setValidators(null);
        barcodevalidation.clearValidators();
        barcodevalidation.updateValueAndValidity();
        this.barCodeFlag=0;
      }
      //this.flag = false;
    }
    else {
      return false;
    }
  }
  else
  {
    if (item != '') {
    //  this.barcodear = [];
      this.stockId = item.Drug_Id;
      this.gpiCode = item.GPICode;
      this.drugNameChanged = false;
      // this.selectedroItems = [];
      // if (item.Route_Id != null) {
      //   this.selectedroItems.push(this.routes.filter(r => r.Route_Id == item.Route_Id)[0]);
      // }
      //this.barcodear.push(item.Barcode)
      this.myform.patchValue({
        drug: item.DrugName,
        qtyHand: '',
        //barcode:item.Barcode
        //route: this.selectedroItems,
        controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
        type: 1
      });
      this.flag = false;
      this.isReadOnlyforControl = false;
      if (item.ControlledSubstanceSchedule == 1)
        this.isControlSubstanceReadOnly = true;
      else
        this.isControlSubstanceReadOnly = false;
      //this.flag = false;
    }
    else {
      return false;
    }
  }
  }
  loadSearchData() {
    let orderTypes = this.myform.value.type == true  ? 'order' : 'stock';
    if(orderTypes == "stock")
    {
    this.drugList = [];
    this.drugList = this.searchTerms.pipe(
      debounceTime(300),        // wait for 300ms pause in events
      distinctUntilChanged(),   // ignore if next search term is same as previous
      switchMap(term => term   // switch to new observable each time
        // return the http search observable
        ? this.dataservice.post(this.config.Emar_Orders_GetStockQtyonHandData ,{"DrugName":term.replace(/[&\/\\#,+()$~%.'":*?<>{}]/g, ''),"NurseStationId":this.nurseStationId} )
        // or the observable of empty heroes if no search term
        : observableOf<any[]>([{ "Stock_Id": 0, "DrugName": "No Record Found" }])),
      catchError(error => {
        // TODO: real error handling
        this.alertService.error(error.message)
        return observableOf<any[]>([]);
      }));
    }
    else
    {
      this.drugList = [];
      this.drugList = this.searchTerms.pipe(
      debounceTime(300),        // wait for 300ms pause in events
      distinctUntilChanged(),   // ignore if next search term is same as previous
      switchMap(term => term   // switch to new observable each time
        // return the http search observable
        ? this.dataservice.search(this.config.Emar_Orders_SearchDrugNameData + term.replace(/[\s&\/\\#,+()$~%.'":*?<>{}]/g, ''))
        // or the observable of empty heroes if no search term
        : observableOf<any[]>([{ "Drug_Id": 0, "DrugName": "No Record Found" }])),
      catchError(error => {
        // TODO: real error handling
        this.alertService.error(error.message)
        return observableOf<any[]>([]);
      }));
    }
  }
  checkLiteral(value:any)
  {
    if(value==true && this.myform.value.treatment==false)
    {
    const drugnameValidations = this.myform.get('drug');
    drugnameValidations.clearValidators();
    drugnameValidations.updateValueAndValidity();
    //if(this.barcodear.length)
    const barcodevalidation = this.myform.get('barcode');
    barcodevalidation.clearValidators();
    barcodevalidation.updateValueAndValidity();
    const schduleTextValidations = this.myform.get('schduleText');
    schduleTextValidations.clearValidators();
    schduleTextValidations.updateValueAndValidity();
    const doseValidations = this.myform.get('dose');
    doseValidations.clearValidators();
    doseValidations.updateValueAndValidity();
    const routeValidations = this.myform.get('route');
    routeValidations.clearValidators();
    routeValidations.updateValueAndValidity();
    this.barCodeStatusFlag =1;
    this.barCodeFlag=0;
    this.stockId=0;
    this.myform.patchValue({
      orderTypeId: 4,
      prn:'',
    });
    }
    else
    {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
      drugnameValidations.updateValueAndValidity();
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
      barcodevalidation.updateValueAndValidity();
      const schduleTextValidations = this.myform.get('schduleText');
      schduleTextValidations.setValidators([Validators.required]);
      schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.setValidators([Validators.required]);//, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
      doseValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.setValidators(Validators.required);
      routeValidations.updateValueAndValidity();
      this.barCodeStatusFlag =0;
      if (this.barcodear.length!=0) {
        const barcodevalidation = this.myform.get('barcode');
        barcodevalidation.clearValidators();
        barcodevalidation.updateValueAndValidity();
      }
      this.myform.patchValue({
        orderTypeId:this.myform.value.treatment==true? 5:1,
        barcode:''
      });
      if(this.myform.value.treatment==false)
      {
        this.myform.patchValue({
          drug:''
        });
      }
      if(this.myform.value.orderTypeId==1)
      {
        this.drugNameChanged=true;
      }
    }
  }
  checkTreatment(value:any)
  {
    if (value == true  && this.myform.value.literal ==false) {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
      drugnameValidations.updateValueAndValidity();
      const schduleTextValidations = this.myform.get('schduleText');
      schduleTextValidations.setValidators([Validators.required]);
      schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.setValidators([Validators.required]);//, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
      doseValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.setValidators(Validators.required);
      routeValidations.updateValueAndValidity();
      if(this.barcodear ==null || this.barcodear.length ==0)
      {
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
      barcodevalidation.updateValueAndValidity();
     // this.barCodeFlag =1;
       }
      this.myform.patchValue({
        orderTypeId:5,
        //literal:true,
        //drug:''
      });
    }
    else if (value == false && this.myform.value.literal ==true) {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.clearValidators();
      drugnameValidations.updateValueAndValidity();
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.clearValidators();
      barcodevalidation.updateValueAndValidity();
      const schduleTextValidations = this.myform.get('schduleText');
      schduleTextValidations.clearValidators();
      schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.clearValidators();
      doseValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.clearValidators();
      routeValidations.updateValueAndValidity();
      this.myform.patchValue({
        orderTypeId:4,
        prn:''
      });
    }
    else if((this.myform.value.literal==true && this.myform.value.treatment==true) ||(this.myform.value.literal==false && this.myform.value.treatment==true)||(this.myform.value.literal==false && this.myform.value.treatment==false)||(this.myform.value.literal==true && this.myform.value.treatment==false))
    {
      if(value==true)
      {
        this.myform.patchValue({
          //literal:true,
          //drug:''
        })
      }
      this.checkLiteral(this.myform.value.literal);
    }
  }
  barcodeModalOpen()
  {
    this.modalBarcodeIsOpen=true;
  }
  modalBarcodeIsClose()
  {
    this.modalBarcodeIsOpen=false;
  }
  closeNotesModel()
  {
    this.noteform.reset();
    this.closeModel();
  }
    closePRNModel()
  {
    this.prnSchedleCheckModal=false;
  }
  resetMonthsDropSettings(limit:any)
{
  this.dropdownSettings_Month = {
    singleSelection: false,
    idField: 'Month_Id',
    textField: 'Month_Name',
    itemsShowLimit: 1,
    allowSearchFilter: true,
    enableCheckAll: false,
    limitSelection: limit,
}
}
resetWeekDropSettings(limit:any)
{
  this.dropdownSettings_Week = {
    singleSelection: false,
    idField: 'Week_Id',
    textField: 'Week_Desc',
    itemsShowLimit: 1,
    allowSearchFilter: true,
    enableCheckAll: false,
    limitSelection:limit,
  };
}
// Fileds Changes Dc order
checkIsFieldsChanged()
{
  debugger
  let orderType=(this.myform.value.literal==true && this.myform.value.treatment==true)?2:(this.myform.value.literal==true && this.myform.value.treatment==false)?4:(this.myform.value.literal==false && this.myform.value.treatment==true)?5:1;
  let physicianRecord = this.physiciansdrop.filter(p => p.Physician_Id == this.myform.value.physician[0].Physician_Id)[0];
  let prn=this.myform.value.prn==true?1:0;
  let orderPrn=this.selectedOrderDetails.PRNFlag==true?1:0;
  let refill=this.myform.value.refill==undefined ||this.myform.value.refill==null || this.myform.value.refill==""?0:this.myform.value.refill;
  let orderrefill=this.selectedOrderDetails.Refill==undefined || this.selectedOrderDetails.Refill==null || this.selectedOrderDetails.Refill==""?0:this.selectedOrderDetails.Refill;
  if((orderType!=4 && ((this.selectedOrderDetails.OrderingPhysicianNPI !=physicianRecord.PhysicianNPI)
  || (this.selectedOrderDetails.AGiveCodeIdentifier!=this.gpiCode)
  || (this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id)
  //|| ((this.selectedOrderDetails.Quantity=="SS" && this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id && this.myform.value.route[0].Route_Id!=38) || (this.selectedOrderDetails.Quantity=="UD" && this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id && (this.myform.value.route[0].Route_Id!=40 && this.myform.value.route[0].Route_Id!=48 )))
  || (parseFloat(orderrefill)!=parseFloat(refill))
  || (((((this.selectedOrderDetails.Quantity=="SS" && this.myform.value.route[0].Route_Id!=38) || (this.selectedOrderDetails.Quantity=="UD" && this.myform.value.route[0].Route_Id!=40 && this.myform.value.route[0].Route_Id!=48)) && this.selectedOrderDetails.Quantity!=this.myform.value.dose)) || (this.selectedOrderDetails.Quantity!="SS" && this.selectedOrderDetails.Quantity!="UD" && parseFloat(this.selectedOrderDetails.Quantity).toFixed(2)!=parseFloat(this.myform.value.dose).toFixed(2)))
  || (orderPrn!=prn))) || (orderType==4 && this.selectedOrderDetails.OrderingPhysicianNPI !=physicianRecord.PhysicianNPI))
  {
    //this.fieldsChangesDcFlag="";
    return 1;
  }
  else {
    this.fieldsChangesDcFlag="No Changes";
    return 2;
  }
}
isDoseValid(inputtxt:any)
{
  if(inputtxt !="" && inputtxt.length!=0)
    {
    var decimal='^[0-9]+(\.[0-9]{1,3})?$';
    if(((inputtxt.match(decimal)==null && (inputtxt.toUpperCase().split('.').join('')!="SS" && inputtxt.toUpperCase()!="UD"))) || (inputtxt.match(decimal)!=null && parseFloat(inputtxt).toFixed(3)=="0.000"))
    {
      this.alertService.warn("Inavlid Qty/Dose");
      this.myform.patchValue({
        dose:this.selectedOrderDetails.Quantity!=undefined && this.selectedOrderDetails.Quantity!=null?this.selectedOrderDetails.Quantity:'',
      });
    }
    else if(inputtxt.match(decimal)==null && (inputtxt.toUpperCase().split('.').join('')=="SS" || inputtxt.toUpperCase()=="UD"))
    {
      this.myform.patchValue({
        dose:inputtxt.toUpperCase().split('.').join(''),
      });
    }
  }
}
fileldsChangesDcOrder(type:any)
{
  if(type==1)
  {
    this.fieldsChangesDcFlag="YES";
    this.modalFieldsChangesDcConfirmation=false;
    if(this.modalHOAIsOpen==false)
    {
      this.orderDiscontinueConfirmation();
    }
    else{
      this.modalHOAIsOpen = false;
      this.hoaObj = null;
      this.orderDiscontinueConfirmation();
    }
  }
  else if(type==0)
  {
    this.fieldsChangesDcFlag="NO";
    this.modalFieldsChangesDcConfirmation=false;
    this.modalHOAIsOpen = false;
    this.hoaObj = null;
    this.getOrderDetailsbyorderId(this.orderId,this.quantityId,this.residentId);
  }
}
orderDiscontinueConfirmation()
{
  this.modalDcConfirmationIsOpen = true;
  this.DcForm.reset();
}
closeDcModel() {
  this.modalDcConfirmationIsOpen = false;
  this.DcForm.reset();
}
orderDiscontinue() {
  debugger;
  this.orderStatusDCObj = {
    PatientId: this.residentId,
    OrderId: this.orderId,
    DAdminId: this.dAdminId,
    QuantityId: this.quantityId,
    POrderCreatedBy: this.userID,
    POrderStatus: 2,
    POOutBoundApproval: 0,
    POOutBoundApprovalBy: 0,
    OrderType: 'Discontinue',
    DiscontinueFlag: 1,
    DiscontinueReason: this.DcForm.value.DcReason,
    DiscontinueAllSplits: this.DcForm.value.dcSplits == true ? 1 : 0,
    DiscontinuedOn: this.dateFormatPipe.dateWithTime(new Date()),
    Split: this.splits
  };
  this.dataservice.post(this.config.Emar_AdminApproval_DiscontinueOrder, this.orderStatusDCObj)
    .subscribe(res => {
      this.DcForm.reset();
      this.fieldsChangesDcFlag="";
      if (res !=0) {
        this.modalDcConfirmationIsOpen = false;
        this.reviewInfoResult.emit(2);
        // this.alertService.success("Order discontinued successfully");
      }
      else {
        this.modalDcConfirmationIsOpen = false;
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error("Something went wrong");
      }

    }, error => {
      this.modalDcConfirmationIsOpen = false;
      this.ng4LoadingSpinnerService.hide();
    });
}
isInteger() {

  const value = this.myform.controls.maxPerDay.value;

  if(value !==null && value !=="")
  {
    const valid = /^[0-9]{1,3}(?:\.[0-9]{1,3})?$/.test(value)

    const isDotOnly = /^\.$/.test(value);
  
    this.myform.controls.maxPerDay.setErrors(null);
    if(!valid){
      this.myform.controls.maxPerDay.setErrors({'data' :true})
    }
  
    if (value.length > 7) {
      this.myform.controls.maxPerDay.setErrors({ 'maxlength': true });
    } else if (isDotOnly) {
      this.myform.controls.maxPerDay.setErrors({ 'dotOnly': true });
    } else {
      const parts = value.split('.');
      const integerPart = parts[0];
      const decimalPart = parts[1];
  
      if (integerPart.length > 3) {
        this.myform.controls.maxPerDay.setErrors({ 'integerError': true });
      } else if (decimalPart && decimalPart.length > 3) {
        this.myform.controls.maxPerDay.setErrors({ 'decimalError': true });
      }
    }
  }
  
 }
appendZero(): void {
  let value = this.myform.controls.maxPerDay.value;

  // Add zero after the dot if user leaves input with integer and dot
  if (/^\d+\.$/.test(value)) {
    this.myform.controls.maxPerDay.setErrors({ 'dotOnly': true });
    value += '0';
    this.myform.controls.maxPerDay.setValue(value);
  }
}
}
