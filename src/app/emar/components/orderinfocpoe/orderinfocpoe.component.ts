import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, ElementRef, AfterViewInit, HostListener , ChangeDetectorRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DrfirstOrderXML, HOA, Orderupdate, NurseComments, CommonOrderStatus, CommonDcOrderStatus, OrdersData, WeekMasterData, MonthMasterData, HoursMasterData, DrugAdministrationTime, OrderFavourite, OrderInfoAlert, OrderHold, PhysicianDetails, OrderDestroy, OrderApproval, CPOEOrdersData, FrequencyMasterDataWithShifts, BarcodeEntity, CPOEOrderApproval } from '../../../models/orders.model';
import { AlertService } from '../../../_services';
import { DemographicInfo, ResidentDemographic } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';
import { DomSanitizer } from '@angular/platform-browser';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { SharedService } from '../../../services/shared/shared.service';
import { Observable, Subject, Subscription, forkJoin, of } from 'rxjs';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { MergeordersComponent } from '../mergeorders/mergeorders.component';
import { SearchDrugNameComponent } from '../search-drug-name/search-drug-name.component';
import { catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { OrdersdiscardComponent } from '../ordersdiscard/ordersdiscard.component';
import { of as observableOf } from 'rxjs';
import { parse } from 'querystring';
import { throttleTime } from 'rxjs-compat/operator/throttleTime';
import { SearchpharmacynameComponent } from '../searchpharmacyname/searchpharmacyname.component';

@Component({
  selector: 'app-orderinfocpoe',
  templateUrl: './orderinfocpoe.component.html',
  styleUrls: ['./orderinfocpoe.component.css']
})
export class OrderinfocpoeComponent implements OnInit {

  @Output() saveChanges: EventEmitter<any> = new EventEmitter();
  data1: any[];
  public favoritePharmacies:any[]=[];

  public orderGridData: any[] = [];
  public ordersDetails = {} as CPOEOrdersData;
  public ordersObj: OrdersData;
  public physiciansdrop: PhysicianDetails[];
  public valueChangesFlagReceive: number = 0;
  public DoseUomSSSS:any = [];
  //public barcodeList: BarcodeDetail[];
  //public barcodeCount: number = 0;
  public template;
  private orderId: number = 0;
  public residentId: number = 0;
  myform: FormGroup = new FormGroup({});
  errorMessage: string;
  scheduleform: FormGroup = new FormGroup({});
  private userID: number;
  resOrderForm: FormGroup = new FormGroup({});
  favForm: FormGroup = new FormGroup({});
  public demographicInfoData = {} as DemographicInfo;
  pageConfig = {};
  public nurseStations: NurseStation[];
  private nurseStationId: number = 0;
  residents: any[];
  DcForm: FormGroup = new FormGroup({});
  minStartDate: string = this.dateFormatPipe.dateFormat(new Date());
  dropdownSettings_Residents: any = {};
  DoseUomDrop: any[];
  dropdownSettings_DoseUom: any = {};
  ShowFilter = true;
  public barcodesList: BarcodeEntity[];
  public barcodear: any[] = [];
  public favouriteMasterList: any[];
  private favouriteObj: OrderFavourite[] = [];
  public frequencyList: FrequencyMasterDataWithShifts[];
  public weeksList: WeekMasterData[];
  public MaxDateFalg:boolean = false;
  public Max3DateFalg:boolean = false;
  public monthsList: MonthMasterData[];
  public hoursList: HoursMasterData[];
  //public timeFormatList: TimeFormatMasterData[];
  public newOrderFlag: number = 0;
  public ordersInfo: OrderInfoAlert = {} as any;
  public monthDays: any[];
  dropdownSettings_Days: any = {};
  dropdownSettings_Month: any = {};
  dropdownSettings_Week: any = {};
  public favstatus: number = 0;
  public splits: number = 0;
  private orderholdobj: OrderHold;
  destroyObj: OrderDestroy;
  orderStatusObj: CommonOrderStatus;
  orderStatusDCObj: CommonDcOrderStatus;
  reactivateStatus: number = 0;
  orderUpdateObj: any;//Orderupdate;
  nurseNotesObj: NurseComments;
  public quantityId: number;
  public selectedNursestation = [];
  dropdownSettings_Nuresestation: any = {};
  public selectedResItem = [];
  public selectedOrder: any;
  public selectedOrderQuantity: any;
  public selectedOrderDADminId: any;
  orderholdform: FormGroup = new FormGroup({});
  public modalHistoryIsOpen: boolean = false;
  public modalholdIsOpen: boolean = false;
  public modalfavIsOpen: boolean = false;
  public modalInsuliIsOpen: boolean = false;
  public modalHOAIsOpen: boolean = false;
  public modaleMar: boolean = false;
  public drugList;
  hoaObj: HOA;
  DrugName: any;
  PharmacyName:any;
  public flag: boolean = true;
  public pharmacyflag:boolean=true;
  public searchTerms = new Subject<string>();
  public searchPharmacyTerms = new Subject<string>();
  public stockId: number = 0;
  gpiCode: string = '';
  ordersinsertObj: CPOEOrderApproval;
  public dAdminId: number = 0;
  public fav: any;
  public eMARDetails: any[] = [];
  public legend : any[] = [];
  public isReadOnly: boolean = false;
  public isReadOnlyPhy: boolean = false;
  public active: any;
  public favobj: any[] = [];
  public canceldate: number = 0;
  public acknowledge: number = 0;
  public activeTab: string = 'Active';
  selectedDays: any[];
  selectedWeeks: any[];
  selectedMonths: any[];
  isWeeksDisabled: boolean = false;
  isdaysDisabled: boolean = false;
  public hideCheckbox: boolean = false;
  emarform: FormGroup = new FormGroup({});
  public residentAllergies: string;
  public residentDiagnosis: string;
  modalOption: NgbModalOptions = {};
  defaultPhysicianNPI: any;
  public yearDrop: any[] = [];
  public modalControlIsOpen: boolean = false;
  public timesArray: any[] = [];
  public startTimeArray: number;
  public isHoursReadOnly: boolean = false;
  public times: string = "";
  timeform: FormGroup = new FormGroup({});
  public modalTimeIsOpen: boolean = false;
  public isReadOnlyforControl: boolean = false;
  reviewClickedFlag: number;
  public selectedphyItems = [];
  dropdownSettings_Physician: any = {};
  public selectedfrequencyItems = [];
  public routes: any[];
  public selectedroItems = [];
  dropdownSettings_Route: any = {};
  dropdownSettings_Frequency: any = {};
  public selectedstItems = [];
  dropdownSettings_StartTime: any = {};
  public selectedntItems = [];
  dropdownSettings_NextTime: any = {};
  //previousDrugName: any;
  public patientIdstatus: number = 0;
  drugNameChanged: boolean = false;
  pharmacyNameChanged:boolean=false;
  checkpharmacy:boolean=false;
  facilityName: any;
  isShiftSchedule: boolean = false;
  public modalDcConfirmationIsOpen: boolean = false;
  valueChangesFlag: number = 0;
  saveChangeFlag: number = 0;
  medispanControlSubBit: number = 0;
  isControlSubstanceReadOnly: boolean = false;
  isDrugOrder: boolean = false;
  discardConditionalFlag: number = 0;
  discontinueFlag: number = 0;
  barCodeFlag: number = 0;
  barCodeStatusFlag: number = 0;
  public patientTypeList: any[] = [];
  public controlledSubstancemessage: string = '';
  public checkedflag: any;
  public orderOrigin: any;
  public controlsubstanceBit = 0;
  //public ordersgridrowclick=0;
  public prnSchedleCheckModal: boolean = false;
  public btnFlag: number = 1;
  public residentAllOrders = [];
  public checkedPorderId: any;
  public checkedPquantityId: any;
  public isHoldChecked: boolean = false;
  public isDcChecked: boolean = false;
  public updateOrdersRecords = [];
  public toolTipText: any = '';
  public holdObj: any;
  public dcObj: any;
  public modalDcAllSplits: boolean = false;
  public noDCSplitsFlag = 0;
  public re: number = 1;
  public selectedindicaitem = [];
  indications: any[];
  public dropdownSettings_Indication = {};
  public userCredetialsForm: FormGroup;
  public userCredeatialsModal: boolean = false;
  public userAttempts: number = 0;
  public qtyDoseData: any[] = [];
  public unitMeasureData: any[] = [];
  public DisableFag:boolean = false;
  loginUserReceFacility: any;
  loginUserReceNurseStation: any;
  NPIbyFFacilityIdNursId: string;
  MyImages: any;
  public data:any;
  postData: { PatientID: number; Type: number; IScheck: number; };
  filteredPharmaciess: any[] = [];
  patientid: any;
  urldata: any;
  public validateEmptyField(c: FormControl) {
    return c.value && !c.value.trim() ? {
      required: {
        valid: false
      }
    } : null;
  }
  public newResOrderFlag: boolean = false;
  public scheduleTextObj: DrugAdministrationTime;
  public orderHoldData: any;
  public holdReason: string = "";
  public orderReleaseDate: any;
  public btnHoaSaveFlag: number = 1;
  public holdReleaseDate: any;
  public scheduleSave: number = 1;
  public days: number = 31;
  public drFirstModal: boolean = false;
  public holdOrdersform: FormGroup = new FormGroup({});
  public multipleOrdersplits = 0;
  public drFirstFlag: number = 0;
  public sourceData = [];
  public dropdownSettings_Source = {};
  public selectedsourceItems = [];
  public selectedsig1Frequency = [];
  public modalAdditionalAdministrationSchedules: boolean = false;
  public additionalscheduleform: FormGroup = new FormGroup({});
  public sigsDoseFreqArray = [];
  public selectedsig2Frequency = [];
  public selectedsig2DoseUom = [];
  public editSigDose: any;
  public editSigFrequency: any;
  public Sig2DoseUom: any=[];
  public Sig3DoseUom: any=[];
  public Sig4DoseUom: any=[];
  public activePharmacies: any[]=[];
  public frequencyListWithoutPRN: FrequencyMasterDataWithShifts[] = [];
  public modalCancelAdditionalScheduleIsOpen:boolean=false;
  public userRole:string;
  public weightDetails:any;
  public isWeight:boolean =false;
  public isHeight:boolean=false;
  public getID:number;
  public pharmacyList;
  public PharmacyNameinput:any;
  public favpharmacyid:number=0;
  public pharmalifeInput:boolean =false;
  public pharmacyDefault: any[]=[];
  public physicianfullList=[];
  public supervisingInactiveModal:boolean=false; 
  public physicianAlertMsg: string;
  public daysSupplyFlag: boolean = false;
  public supplydays = '1';
  public qtyhandTouched: boolean = false;
  public freqTimesArray = [];
  private formSubscription: Subscription;
  private qtyHandControl: FormControl;
  public barcodeFacilityId:number =0;
  profileONHoldChangesCheckState: { [key: string]: boolean } = {};
  profileDCChangesCheckState: { [key: string]: boolean } = {};
  rightGridDCChangesFlag: boolean= false;
  rightGridOnHoldChangesFlag: boolean = false;
  StartOnemonthDate:string = this.dateFormatPipe.dateFormat(new Date());
  @ViewChild('dateFocus') dateFocus:ElementRef
  @ViewChild('discontinueFocus') discontinueFocus:ElementRef
  @ViewChild('qtyDoseFocus') qtyDoseFocus:ElementRef
  //DTMS (saikiran reddy- 12/09/2025)
  public drugToDrugAlertPageConfig={}
  public selectedDrugName: string = '';
  public drugINteractionModal: boolean = false;
  public alertList = [];
  public alertAllergyList=[];

  constructor(private dataservice: DataService, private sharedService: SharedService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private route: Router, private alertService: AlertService,
    private persistanceService: PersistanceService, private sanitizer: DomSanitizer, private dateFormatPipe: CustomdatePipe,
    private modalService: NgbModal  ,  private readonly changeDetectorRef: ChangeDetectorRef) {

  }

  ngOnInit() {

    window.scroll(0,0);
    this.pageConfig = this.persistanceService.getPermissionsByScreen("CPOE");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {

        this.sharedService.saveChangesFlag.subscribe(res => this.valueChangesFlagReceive = res);
        this.discardConditionalFlag = 0;
        this.active = "new";
        this.sharedService.currentOrderId.subscribe(res => this.orderId = res);
        this.sharedService.currentPatientId.subscribe(res => this.residentId = res);
        this.sharedService.currentQuantityId.subscribe(res => this.quantityId = res);
        this.sharedService.currentFacilityId.subscribe(res=>this.barcodeFacilityId = res)


        if (this.orderId == 0 && this.quantityId == 0) {
          this.newResOrderFlag = true;

        }

        if (this.residentId != 0) {
          this.pageConfig = this.persistanceService.getPermissionsByScreen("CPOE");
          this.userID = this.persistanceService.get(this.config.loggedInUserKey);
          this.userRole = this.persistanceService.get("userRole");
          this.template = this.dataservice.template;
          this.ng4LoadingSpinnerService.show();
          // this.myform.enable();
          // this.DisableFag = false;
          this.myform = new FormGroup({
            resName: new FormControl(''),
            resID: new FormControl(''),
            resDOB: new FormControl(''),
            resAdmitDate: new FormControl(''),
            resDischargeDate: new FormControl(''),
            physician: new FormControl('', Validators.required),

            drug: new FormControl('', [Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]),
            // pharmacy:new FormControl('',[Validators.required, Validators.maxLength(60),Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
            pharmacy : new FormControl(''),
            dose: new FormControl('1', [Validators.required]),// ,Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
            diagnosis: new FormControl(''),
            addInst: new FormControl('', [Validators.required, Validators.maxLength(250)]),
            route: new FormControl('', Validators.required),
            notes: new FormControl(''),
            daw: new FormControl('0', Validators.required),
            startDate: new FormControl('', Validators.required),
            writtenDate: new FormControl('', Validators.required),
            endDate: new FormControl(''),
            qtyHand: new FormControl('1', [Validators.required, Validators.maxLength(9), Validators.pattern(/^[0-9]+(\.[0-9]{1,3})?$/)]),
            refill: new FormControl('0', [Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]),
            alertText: new FormControl(''),
            maxPerDay: new FormControl('',
            [Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
            Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
            Validators.pattern(/^\d*(\.\d{0,3})?$/),
            Validators.pattern(/^\d+(\.\d{0,3})?$/)] ),
            barcode: new FormControl('', [Validators.maxLength(20)]),
            prn: new FormControl(false),
            controlSubstance: new FormControl(false),
            literal: new FormControl(false),
            self: new FormControl(false),
            treatment: new FormControl(false),
            maySub: new FormControl(false),
            type: new FormControl(true),
            nursestationName: new FormControl(),
            ddlresidents: new FormControl(),
            schduleText: new FormControl(''),
            insulincomments: new FormControl(),
            orderTypeId: new FormControl(),
            waitforpharmacy: new FormControl(''),
            suppliedanother: new FormControl(''),
            indication: new FormControl('', Validators.required),
            indicationFreeText: new FormControl('', Validators.required),
            source: new FormControl('', Validators.required),
            UOM: new FormControl('', Validators.required),
            sig1Frequency: new FormControl('', Validators.required),
            DoseUom: new FormControl('' ,  Validators.required),
            daysSupply: new FormControl( this.supplydays,
              [Validators.required ,Validators.pattern(this.config.numeric1),
             ] ),
          });

          if (this.orderId != 0 && this.quantityId != 0) {
            this.myform.disable();
            this.DisableFag = true;

          }
          this.qtyHandControl = new FormControl();
          this.myform.addControl('qtyHand', this.qtyHandControl);
          this.formSubscription = this.myform.valueChanges.subscribe(value => {
            this.updateQtyHand(value);
            if (this.discardConditionalFlag != 1) {
              if (this.valueChangesFlag == 1) {
                this.saveChangeFlag = 1;
                this.sharedService.saveChangesOrderInfo(this.saveChangeFlag);
              }
            }
          });
          this.updateQtyHand(this.myform.value);
          this.userCredetialsForm = new FormGroup({
            Username: new FormControl('', Validators.required),
            password: new FormControl('', Validators.required),
          });
          this.additionalscheduleform = new FormGroup({
            sig2dose: new FormControl('', Validators.required),
            sig2Frequency: new FormControl('', Validators.required),
            sig2addInst: new FormControl('', [Validators.maxLength(250)]),
            sig2prn: new FormControl(false),
            sig2DoseUom: new FormControl('', Validators.required),
            sig2maxPerDay: new FormControl('',  [Validators.maxLength(6), Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
            Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
            Validators.pattern(/^\d*(\.\d{0,3})?$/),
            Validators.pattern(/^\d+(\.\d{0,3})?$/)]),
          });
          this.scheduleform = new FormGroup({
            frequency: new FormControl(''),
            either: new FormControl(''),
            //timeFormat: new FormControl(0),
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
            // oDay: new FormControl(0),
            // through: new FormControl(0),
            aDay: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric), Validators.min(1)]),
            hDay: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric), Validators.min(1)]),
            days: new FormControl()
          });
          this.timeform = new FormGroup({
            starttime: new FormControl(''),
          });
          this.orderholdform = new FormGroup({
            orderHoldFormDate: new FormControl('', Validators.required),
            orderHoldToDate: new FormControl('', Validators.required),
            orderHoldReason: new FormControl('', [Validators.required, Validators.maxLength(500)]),

          });
          this.favForm = new FormGroup(
            {
              favchecks: new FormControl()
            });
          this.DcForm = new FormGroup({
            DcReason: new FormControl('', [Validators.maxLength(500)]),
            dcSplits: new FormControl(false),
            multipledcSplits: new FormControl(false)
          });
          this.emarform = new FormGroup(
            {
              month: new FormControl(),
              year: new FormControl(),
              hidechk: new FormControl(),
            });
          let data = new Date();
          this.emarform.patchValue({
            month: data.getMonth() + 1,
            year: data.getFullYear(),
          })
          this.dropdownSettings_Physician = {
            singleSelection: true,
            idField: "Physician_Id",
            textField: "PhysicianFullName",
            text: "Select",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: true,
          };
          this.dropdownSettings_Route = {
            singleSelection: true,
            idField: "Route_Id",
            textField: "Route",
            text: "Select",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: true,
            disabled:false
          };
          this.dropdownSettings_Indication = {
            singleSelection: true,
            idField: "PDiagnosis_Id",
            textField: "DiagnosisDescription",
            text: "Select",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: true,
          };
          this.dropdownSettings_Frequency = {
            singleSelection: true,
            idField: "Frequency_Id",
            textField: "Frequency_Name",
            text: "Select",
            itemsShowLimit: 1,
            allowSearchFilter: true,
            closeDropDownOnSelection: true,
          };
          this.dropdownSettings_DoseUom = {
            singleSelection: true,
            idField: "Dose_Id",
            textField: "Dose_Desc",
            text: "Select",
            itemsShowLimit: 1,
            allowSearchFilter: true,
            closeDropDownOnSelection: true,
          };

          this.dropdownSettings_StartTime = {
            singleSelection: true,
            idField: "Hour_Id",
            textField: "Hour_Desc",
            itemsShowLimit: 1,
            allowSearchFilter: this.ShowFilter,
            closeDropDownOnSelection: true,
            noDataAvailablePlaceholderText: 'Please Select Facility'
          };
          this.dropdownSettings_NextTime = {
            singleSelection: true,
            idField: "Hour_Id",
            textField: "Hour_Desc",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: this.ShowFilter,
          };
          this.dropdownSettings_Nuresestation = {
            singleSelection: true,
            idField: "NurseStation_Id",
            textField: "NurseStation_Name",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: this.ShowFilter
          };
          this.dropdownSettings_Residents = {
            singleSelection: true,
            idField: "Patient_Id",
            textField: "PatientName",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: this.ShowFilter
          };
          this.dropdownSettings_Source = {
            singleSelection: true,
            idField: "Source_Id",
            textField: "Source_Desc",
            itemsShowLimit: 1,
            closeDropDownOnSelection: true,
            allowSearchFilter: this.ShowFilter
          };

         // GetDefultNPIByFailityIdNUrsId();
         // this.getUserRecentFacilityNurseStations();
          this.getFacilityNSResidentsDataByPId();
          this.drugToDrugAlertPageConfig = this.persistanceService.getPermissionsByScreen("DTMS-Alerts");
          if (this.drugToDrugAlertPageConfig == undefined) {
            this.drugToDrugAlertPageConfig =0;
          }

        }
        else
          this.route.navigate(['/home/ordergridcpoe']);
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }



  saveOrder(controlSubstanceFlag: number) {
    this.myform.patchValue({
      controlSubstance: controlSubstanceFlag,
    });
    if (this.newOrderFlag == 1) {
      //this.saveNewOrder(controlSubstanceFlag);
      this.userAttempts = 0;
      this.userCredetialsForm.reset();
      this.userCredeatialsModal = true;
    }
    else
      this.saveExistingOrder(controlSubstanceFlag);
  }

  isInteger() : void {

    if(this.myform.value.prn == true){
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.required]);
      maxpervalidation.updateValueAndValidity();
    }else{
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators(null);
      maxpervalidation.updateValueAndValidity();
    }

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
  isInteger2() : void {
    
    if(this.additionalscheduleform.value.sig2prn == true){
      const maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
      maxpervalidation.setValidators([Validators.required]);
      maxpervalidation.updateValueAndValidity();
    }else{
      const maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
      maxpervalidation.setValidators(null);
      maxpervalidation.updateValueAndValidity();
    }

    const value = this.additionalscheduleform.controls.sig2maxPerDay.value;
    const valid = /^[0-9]{1,2}(?:\.[0-9]{1,3})?$/.test(value)
   const isDotOnly = /^\.$/.test(value);

   this.additionalscheduleform.controls.sig2maxPerDay.setErrors(null);
   if(!valid){
    this.additionalscheduleform.controls.sig2maxPerDay.setErrors({'data' :true})
  }

  if (value.length > 6) {
    this.additionalscheduleform.controls.sig2maxPerDay.setErrors({ 'maxlength': true });
  } else if (isDotOnly) {
    this.additionalscheduleform.controls.sig2maxPerDay.setErrors({ 'dotOnly': true });
  } else {
    const parts = value.split('.');
    const integerPart = parts[0];
    const decimalPart = parts[1];

    if (integerPart.length > 2) {
      this.additionalscheduleform.controls.sig2maxPerDay.setErrors({ 'integerError': true });
    } else if (decimalPart && decimalPart.length > 3) {
      this.additionalscheduleform.controls.sig2maxPerDay.setErrors({ 'decimalError': true });
    }
  }

  }
  appendZero2(): void {
    let value = this.additionalscheduleform.controls.sig2maxPerDay.value;

    // Add zero after the dot if user leaves input with integer and dot
    if (/^\d+\.$/.test(value)) {
      this.additionalscheduleform.controls.sig2maxPerDay.setErrors({ 'dotOnly': true });
      value += '0';
      this.additionalscheduleform.controls.sig2maxPerDay.setValue(value);
    }
  }
  GetDefultNPIByFailityIdNUrsId(){


    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Get_Defult_Nursingstation_Prescriber +this.residentId)
        .subscribe(res => {

          this.NPIbyFFacilityIdNursId = res;
        }, error => {
          this.alertService.error(error.message);
        });
  }


  allowSaveNewOrder() {
    let controlsubstancevalue = this.myform.value.controlSubstance == true ? 1 : 0;
    this.saveNewOrder(controlsubstancevalue);
  }
  saveNewOrder(controlSubstanceFlag: number) {
    if(this.myform.value.physician != 0 && this.myform.value.physician != null && this.myform.value.physician[0].Physician_Id != 0 && this.myform.value.physician[0].Physician_Id !=null ){
      if (this.newOrderFlag == 1 ) {
        this.ng4LoadingSpinnerService.show();
        this.hoaObj == null;
        let barcode = this.barcodear.join();
        let freqId = this.myform.value.sig1Frequency == undefined || this.myform.value.sig1Frequency.length == 0 || this.myform.value.sig1Frequency == null ? null : this.myform.value.sig1Frequency[0].Frequency_Id;
        
        this.ordersinsertObj = {
          PApprovalOrder_Id: 0,
          Porder_Id: 0,
          Patient_Id: this.residentId,
          OrderingPhysicianID: this.myform.value.physician[0].Physician_Id,
          OrderControl: 'NW',
          OrderTypeID: (this.myform.value.literal == true && this.myform.value.treatment == true) ? 2 : (this.myform.value.literal == true && this.myform.value.treatment == false) ? 4 : (this.myform.value.literal == false && this.myform.value.treatment == true) ? 5 : 1,//1,
          Stock_Id: this.stockId,
          TransactionDate: this.dateFormatPipe.transform(new Date()),
          OrderEffectiveDate: this.dateFormatPipe.transform(new Date()),
          POrder_Status: 1,
          POrder_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          AlertText: this.myform.value.alertText,
          MaxPerdays: this.myform.value.maxPerDay,
          OrderStockFlag: this.myform.value.type == 1 ? true : false,
          PRNFlag: this.myform.value.prn,
          ControlSubstanceBit: controlSubstanceFlag,
          SelfAdministeredFlag: this.myform.value.self,
          TreatmentFlag: this.myform.value.treatment,
          Maysubstitute: this.myform.value.maySub,
          RouteCode: this.myform.value.route.length != 0 ? this.myform.value.route[0].Route_Id : '',
          InsulinComments: this.myform.value.insulincomments,
          Quantity: this.myform.value.dose==undefined || this.myform.value.dose==''?null: this.myform.value.dose,
          StartDate: this.dateFormatPipe.transform(this.myform.value.startDate),
          EndDate: this.myform.value.endDate == "" ? null : this.dateFormatPipe.transform(this.myform.value.endDate),
          RequestedGiveCode: this.gpiCode, //Using this for GPI Code
          GiveCodeText: this.myform.value.drug,
          //PharmacyName:this.myform.value.pharmacy,
          ProviderAdminDrugInsText: this.myform.value.addInst==undefined || this.myform.value.addInst==''?null:this.myform.value.addInst,
          NumberOfRefills: this.myform.value.refill,
          InHand: null, //this.myform.value.qtyHand,
          POOutBoundFileStatus: 1,
          POOutBoundApproval: null,
          POOutBoundApprovalBy: null,
          POOutBoundApprovalOn: null,
          POrder_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
          AdministrationType: 1,
          NursingFreq_Id: freqId != null ? (freqId.startsWith('s') ? null : parseInt(freqId)) : null,//(this.hoaObj==null)?null: this.hoaObj.freqId,
          NurseShifts_Id: freqId != null ? (freqId.startsWith('s') ? freqId.substring(1) : null) : null, //(this.hoaObj==null)?null:this.hoaObj.nurseShiftId,
          //this.hoaObj.hourId
          Hour_Id: (this.hoaObj == null) ? null : this.hoaObj.hourIds,
          Hours: (this.hoaObj == null) ? null : this.hoaObj.hours,
          Monday: (this.hoaObj == null) ? null : this.hoaObj.monday,
          Tuesday: (this.hoaObj == null) ? null : this.hoaObj.tuesday,
          Wednesday: (this.hoaObj == null) ? null : this.hoaObj.wednesday,
          Thursday: (this.hoaObj == null) ? null : this.hoaObj.thursday,
          Friday: (this.hoaObj == null) ? null : this.hoaObj.friday,
          Saturday: (this.hoaObj == null) ? null : this.hoaObj.saturday,
          Sunday: (this.hoaObj == null) ? null : this.hoaObj.sunday,
          Week_Id: (this.hoaObj == null) ? null : this.hoaObj.weekId,
          Month_Id: (this.hoaObj == null) ? null : this.hoaObj.monthId,
          Barcode: "",//barcode.replace(/\s/g, ""),
          Days: (this.hoaObj == null) ? null : this.hoaObj.days,
          Favourites: (this.hoaObj == null) ? null : this.favobj.join(),
          OnlyOnDay: 0,
          ThroughDay: 0,
          ActiveDays: (this.hoaObj == null) ? null : this.hoaObj.activedays,
          HoldDays: (this.hoaObj == null) ? null : this.hoaObj.holddays,
          ScheduleText: null,//this.myform.value.schduleText,
          Notes: this.myform.value.notes,
          Daw: this.myform.value.daw,
          DispenseQty: this.myform.value.qtyHand,
          WrittenDate: this.dateFormatPipe.transform(this.myform.value.writtenDate),
          DiagIndication: this.myform.value.indication != undefined && this.myform.value.indication != null && this.myform.value.indication.length > 0 ? this.myform.value.indication[0].PDiagnosis_Id : null,
          DiagIndicationText: this.myform.value.indicationFreeText != "" ? this.myform.value.indicationFreeText : null,
          WaitforPharmacy: null,//this.myform.value.waitforpharmacy,
          Hospice: null,// this.myform.value.suppliedanother,
          UserName: this.userCredetialsForm.value.Username,
          Password: this.userCredetialsForm.value.password,
          Source: this.myform.value.source != undefined && this.myform.value.source != null && this.myform.value.source.length > 0 ? this.myform.value.source[0].Source_Id : null,
          UOM: this.myform.value.UOM != undefined && this.myform.value.UOM != null && this.myform.value.UOM != "" ? parseInt(this.myform.value.UOM) : null,
          Sig2Quantity: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ? this.sigsDoseFreqArray[1].dose : '',
          Sig2NursingFreq_Id: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ? (this.sigsDoseFreqArray[1].frequency.startsWith('s') ? null : parseInt(this.sigsDoseFreqArray[1].frequency)) : null,
          Sig2NurseShifts_Id: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ? (this.sigsDoseFreqArray[1].frequency.startsWith('s') ? this.sigsDoseFreqArray[1].frequency.substring(1) : null) : null,
          Sig2AddInsText: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ? (this.sigsDoseFreqArray[1].additionalInst==undefined ||  this.sigsDoseFreqArray[1].additionalInst==''?null :this.sigsDoseFreqArray[1].additionalInst): null,
          Sig2PRNFlag: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ? this.sigsDoseFreqArray[1].prn : null,
          Sig2MaxPerdays: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ? this.sigsDoseFreqArray[1].maxperday : '',
          Sig3Quantity: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? this.sigsDoseFreqArray[2].dose : '',
          Sig3NursingFreq_Id: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? (this.sigsDoseFreqArray[2].frequency.startsWith('s') ? null : parseInt(this.sigsDoseFreqArray[2].frequency)) : null,
          Sig3NurseShifts_Id: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? (this.sigsDoseFreqArray[2].frequency.startsWith('s') ? this.sigsDoseFreqArray[2].frequency.substring(1) : null) : null,
          Sig3AddInsText: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? (this.sigsDoseFreqArray[2].additionalInst==undefined || this.sigsDoseFreqArray[2].additionalInst==''?null:this.sigsDoseFreqArray[2].additionalInst ): null,
          Sig3PRNFlag: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? this.sigsDoseFreqArray[2].prn : null,
          Sig3MaxPerdays: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? this.sigsDoseFreqArray[2].maxperday : '',
          Sig4Quantity: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? this.sigsDoseFreqArray[3].dose : '',
          Sig4NursingFreq_Id: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? (this.sigsDoseFreqArray[3].frequency.startsWith('s') ? null : parseInt(this.sigsDoseFreqArray[3].frequency)) : null,
          Sig4NurseShifts_Id: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? (this.sigsDoseFreqArray[3].frequency.startsWith('s') ? this.sigsDoseFreqArray[3].frequency.substring(1) : null) : null,
          Sig4AddInsText: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? (this.sigsDoseFreqArray[3].additionalInst==undefined || this.sigsDoseFreqArray[3].additionalInst==''?null :this.sigsDoseFreqArray[3].additionalInst ): null,
          Sig4PRNFlag: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? this.sigsDoseFreqArray[3].prn : null,
          Sig4MaxPerdays: this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? this.sigsDoseFreqArray[3].maxperday : '',
          DUom :this.myform.value.DoseUom.length != 0 ? this.myform.value.DoseUom[0].Dose_Id : '',
          Sig2DUom:this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 2 ?(this.sigsDoseFreqArray[1].Seg2DoseUom==undefined || this.sigsDoseFreqArray[1].Seg2DoseUom==''?null :this.sigsDoseFreqArray[1].Seg2DoseUom ): null,
          Sig3DUom:this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 3 ? (this.sigsDoseFreqArray[2].Seg2DoseUom==undefined || this.sigsDoseFreqArray[2].Seg2DoseUom==''?null :this.sigsDoseFreqArray[2].Seg2DoseUom ): null,
          Sig4DUom:this.myform.value.orderTypeId != 4 && this.sigsDoseFreqArray.length >= 4 ? (this.sigsDoseFreqArray[3].Seg2DoseUom==undefined || this.sigsDoseFreqArray[3].Seg2DoseUom==''?null :this.sigsDoseFreqArray[3].Seg2DoseUom ): null,
          PharmacyName: null,
          Dayssupply : this.myform.value.daysSupply,


        };
        // console.log(this.ordersinsertObj , "orders data")
        // console.log(this.myform ,"form")
        
        this.dataservice.post(this.config.Emar_AdminApproval_InsertApprovalCPOEOrderData, this.ordersinsertObj)
          .subscribe(res => {
            if (res != 0 && res != -1) {
              this.userCredeatialsModal = false;
              this.alertList = [];
              this.alertAllergyList=[];
              this.drugINteractionModal = false;
              this.getOrderGridData('Active', 1);
              this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
                .subscribe(res => {
                  if (res == 1) {
                    this.alertService.success("New order saved successfully, awaiting admin approval");
                  }
                  else {
                    this.alertService.success("New order placed successfully");
                    this.selectedphyItems = [];
                  if (this.defaultPhysicianNPI != null) {

                    let checkExist = this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI);
                    if (checkExist != undefined) {
                      this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
                    }
                  }
                  if(this.selectedphyItems !=null && this.selectedphyItems.length){
                    this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
                    if(this.selectedphyItems[0].PStatus == 0 ){
                      this.supervisingInactiveModal = true;
                      this.physicianAlertMsg ="Selected Physician is inactive"
                    }
                    else if( (this.selectedphyItems[0].CredeValue == 'NP' || this.selectedphyItems[0].CredeValue == 'PA' || this.selectedphyItems[0].CredeValue == 'Other' ) && (this.selectedphyItems[0].SPhy ==null || this.selectedphyItems[0].SPhy == '' || this.selectedphyItems[0].SPhy.length ===0)){
                      this.supervisingInactiveModal = true;
                      this.physicianAlertMsg = "“Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) has incomplete credentialing info. Please select another Prescriber"


                    }
                    else if(this.selectedphyItems[0].SPhy !=null && this.selectedphyItems[0].SPhy != '' && this.selectedphyItems[0].SPhyStatus==0){
                      this.supervisingInactiveModal = true;
                      this.physicianAlertMsg = "Selected Prescriber's ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) Supervising Physician( " + this.selectedphyItems[0].SphyName + " )is inactive." ;
                    }
                    else if(this.selectedphyItems.length && this.selectedphyItems[0].Credentials == 0){
                        this.supervisingInactiveModal = true;
                        this.physicianAlertMsg = "Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"
                    }
                      else{
                       this.myform.patchValue({
                         physician: this.selectedphyItems,
                        });
                      }
                  }
                  else{
                    this.myform.patchValue({physician: '',});
                  }
                  }
                }, error => {
                  this.alertService.error(error.message)
                });
                this.selectedsig1Frequency=[];
                // this.selectedsourceItems=[];
                this.supplydays = '1';
                this.qtyhandTouched = false;
               debugger;
              this.myform.patchValue({
                //physician: '',
                drug: '',
                //pharmacy:this.pharmacyDefault[0].PharmacyName,
                indicationFreeText:'',
                indication: '',
                 source: '',
                route: '',
                dose: '1',
                addInst: '',
                notes: '',
                startDate: this.minStartDate,
                writtenDate: this.minStartDate,
                endDate: '',
                waitforpharmacy: '',
                suppliedanother: '',
                barcode: '',
                diagnosis: '',
                type: '1',
                qtyHand: '1',
                refill: '0',
                alertText: '',
                maxPerDay: '',
                schduleText: '',
                UOM: '',
                DoseUom:'',
                sig1Frequency:this.selectedsig1Frequency,
                prn:'',
                controlSubstance:'',
                self:'',
                treatment:'',
                literal:'' ,
                orderTypeId:''
              });
              if (this.persistanceService.get('userRole') == '\"PHYSICIAN\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {
                  if (!Array.isArray(this.selectedsourceItems)) {
                    this.selectedsourceItems = []; // Initialize as an array if it's not already
                }
                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                  this.myform.patchValue({
                    source: this.selectedsourceItems,
                  })
                }
              }else{
                this.myform.patchValue({
                  source: '',
                })
              }
              if (this.persistanceService.get('userRole') == '\"PRESCRIBER\"') {
                if (!Array.isArray(this.selectedsourceItems)) {
                  this.selectedsourceItems = []; // Initialize as an array if it's not already
              }
                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                  this.myform.patchValue({
                    source: this.selectedsourceItems,
                  })
                }
              }else{
                this.myform.patchValue({
                  source: '',
                })
              }
          
              this.loadSearchData();
              const maxpervalidation = this.myform.get('maxPerDay');
              maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
              Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
              Validators.pattern(/^\d*(\.\d{0,3})?$/),
              Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
              //maxpervalidation.clearValidators();
              maxpervalidation.updateValueAndValidity();
              const addInstValidations = this.myform.get('addInst');
              addInstValidations.setValidators(Validators.maxLength(250));
              addInstValidations.updateValueAndValidity();
              this.favobj = [];
              this.favouriteObj = [];
              this.sigsDoseFreqArray = [];
              this.resetAdministerScheduleForm();
              //this.barcodeCount = 0;
              this.ng4LoadingSpinnerService.hide();
            }
            else if (res == 0) {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.error("Something went wrong.")
            }
            else if (res = -1) {
              this.ng4LoadingSpinnerService.hide();
              this.userAttempts += 1;
              if (this.userAttempts == 5) {
                this.persistanceService.logoutUser();
              }
              else {
                this.alertService.warn("Invalid Credentials");
              }
            }
          }, error => {

            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(error.message);
          });

        this.modalControlIsOpen = false;
      }
    }else if(this.myform.value.physician == 0 || this.myform.value.physician == null || this.myform.value.physician[0].Physician_Id == 0 || this.myform.value.physician[0].Physician_Id ==null )
    {
      this.alertService.error("Please select physician");
    }
    // else
    //   this.saveExistingOrder(controlSubstanceFlag);
  }
  InsertOrder() {
    debugger;
    this.btnFlag = 1;
    if (this.residentId > 0) {
      if (this.demographicInfoData.PVisit_Status == 2) {
        this.alertService.warn("Resident has been discharged.")
      }

      else if (this.demographicInfoData.PVisit_Status == 3) {
        this.alertService.warn("Status updated to temporarily inactive.")
      }
      else {
        if (this.newOrderFlag == 1) {

          if (this.myform.value.physician == 0 || this.myform.value.physician == null || this.myform.value.physician == undefined)
            this.alertService.error("Please select physician");
          else if ((this.stockId <= 0 || this.stockId==null || this.stockId== undefined )&& (this.myform.value.orderTypeId == 1))
          {
            //this.alertService.error("Drug name must be selected from search list");
            this.myform.patchValue({drug:''})
          }
          //  else if ((this.favpharmacyid == 0 &&  this.filteredPharmaciess.length==0 && this.pharmacyDefault.length == 0 )|| this.pharmacyflag ==true || this.PharmacyNameinput.length <= 2)
          //   this.alertService.error("Pharmacy name must be selected from search list");
          else if (this.myform.value.startDate == 0)
            this.alertService.error("Please select start date");
          else if (this.myform.value.startDate != 0 && this.myform.value.startDate < this.myform.value.writtenDate)
            this.alertService.error("Start Date should be greater than Written Date");
          else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.startDate)
            this.alertService.error("End Date should be greater than Start Date");
          else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.writtenDate)
            this.alertService.error("End Date should be greater than Written Date");
          else if (this.isControlSubstanceReadOnly == false && this.myform.value.controlSubstance == true) {
            this.controlledSubstancemessage = 'Order requires controlled substance count certification';
            this.checkedflag = true;
            this.modalControlIsOpen = true;
          }
          else {
            // let controlsubstancevalue=this.myform.value.controlSubstance==true?1:0;
            // this.saveNewOrder(controlsubstancevalue);
             if(this.drugToDrugAlertPageConfig['AccessWrite'] == 1 ){
              this.drugINteractionModal = true;
              this.getDrugInteractionAlerts();
            }else{
            this.userAttempts = 0;
            this.userCredetialsForm.reset();
            this.userCredeatialsModal = true;
            }
          }
        }
        else {
          this.updateOrderReviewClick(0);
        }
      }
    }
    else
      this.alertService.error('Select resident');
  }
  updateOrderReviewClick(reviewClicked: number) {

    let prnCheck = this.myform.value.prn;
    let prnScheduleCheck = this.myform.value.schduleText;
    if (this.demographicInfoData.PVisit_Status == 2) {
      this.alertService.warn("Resident has been discharged.")
    }
    else if (this.demographicInfoData.PVisit_Status == 3) {
      this.alertService.warn("Status updated to temporarily inactive.")
    }
    else {
      this.reviewClickedFlag = reviewClicked;
      let drugName = this.myform.value.drug == null ? '' : this.myform.value.drug;

      let pharmacyName = this.myform.value.pharmacy == null ? '' : this.myform.value.pharmacy;

      if (this.myform.value.physician == 0 || this.myform.value.physician == null || this.myform.value.physician == undefined)
        this.alertService.error("Please select physician");
      else if (this.stockId == 0 && this.myform.value.type == false)
        this.alertService.warn("Drug name doesn't exist in stock");
      else if (this.drugNameChanged == true && this.myform.value.orderTypeId == 1)
        this.alertService.error("Please select drug name from the list");
        // else if ((this.favpharmacyid == 0 &&  this.filteredPharmaciess.length==0 && this.pharmacyDefault.length == 0 )|| this.pharmacyflag ==true || this.PharmacyNameinput.length <= 2)
        // this.alertService.error("Pharmacy name must be selected from search list");
      else if (this.myform.value.startDate == 0)
        this.alertService.error("Please select start date");
      else if (this.myform.value.startDate != 0 && this.myform.value.startDate < this.myform.value.writtenDate)
        this.alertService.error("Start Date should be greater than Written Date");
      else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.startDate)
        this.alertService.error("End Date should be greater than Start Date");
      else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.writtenDate)
        this.alertService.error("End Date should be greater than Written Date");
      //ToDo: Anitha - Confirm with client whether HL7 order can be modified as Control Substance or not
      else if (this.medispanControlSubBit == 0 && this.controlsubstanceBit == 1 && this.orderOrigin == 'P') {
        if (this.myform.value.controlSubstance == false) {
          this.controlledSubstancemessage = 'You have unchecked controlled substance checbox. Are you sure this is not a controlled med?';
          this.checkedflag = false;
          this.modalControlIsOpen = true;
        }
        else {

          if (prnCheck == true && prnScheduleCheck == "PRN - As Needed" && this.btnFlag == 2) {
            this.prnSchedleCheckModal = true;
          }
          else
            this.saveExistingOrder(1);
        }
      }
      else if (this.medispanControlSubBit == 0 && this.controlsubstanceBit == 0 && this.orderOrigin == 'P') {
        if (this.myform.value.controlSubstance == true) {
          this.controlledSubstancemessage = 'Order requires controlled substance count certification';
          this.checkedflag = true;
          this.modalControlIsOpen = true;
        }
        else {
          if (prnCheck == true && prnScheduleCheck == "PRN - As Needed" && this.btnFlag == 2) {
            this.prnSchedleCheckModal = true;
          }
          else
            this.saveExistingOrder(0);
        }
      }
      else if (this.medispanControlSubBit == 0 && this.controlsubstanceBit == 1 && this.orderOrigin == 'M') {
        if (this.myform.value.controlSubstance == false) {
          this.controlledSubstancemessage = 'You have unchecked controlled substance checbox. Are you sure this is not a controlled med?';
          this.checkedflag = false;
          this.modalControlIsOpen = true;
        }
        else {
          if (prnCheck == true && prnScheduleCheck == "PRN - As Needed" && this.btnFlag == 2) {
            this.prnSchedleCheckModal = true;
          }
          else
            this.saveExistingOrder(1);
        }
      }
      else if (this.medispanControlSubBit == 0 && this.controlsubstanceBit == 0 && this.orderOrigin == 'M') {
        if (this.myform.value.controlSubstance == true) {
          this.controlledSubstancemessage = 'Order requires controlled substance count certification';
          this.checkedflag = true;
          this.modalControlIsOpen = true;
        }
        else {
          if (prnCheck == true && prnScheduleCheck == "PRN - As Needed" && this.btnFlag == 2) {
            this.prnSchedleCheckModal = true;
          }
          else
            this.saveExistingOrder(0);
        }
      }
      else {
        if (prnCheck == true && prnScheduleCheck == "PRN - As Needed" && this.btnFlag == 2) {
          this.prnSchedleCheckModal = true;
        }
        else
          this.saveExistingOrder(0);
      }
    }
  }
  loadSearchData() {
    let orderTypes = this.myform.value.type == true ? 'order' : 'stock';
    if (orderTypes == "stock") {
      this.drugList = [];
      this.drugList = this.searchTerms.pipe(
        debounceTime(300),        // wait for 300ms pause in events
        distinctUntilChanged(),   // ignore if next search term is same as previous
        switchMap(term => term   // switch to new observable each time
          // return the http search observable
          ? this.dataservice.post(this.config.Emar_Orders_GetStockQtyonHandData,{"DrugName": term.replace(/[&\/\\#,+()$~%.'";:*?<>{}]/g, ''), "NurseStationId": this.nurseStationId})
          // or the observable of empty heroes if no search term
          : observableOf<any[]>([{ "Stock_Id": 0, "DrugName": "No Record Found" }])),
        catchError(error => {
          // TODO: real error handling
          this.alertService.error(error.message)
          return observableOf<any[]>([]);
        }));
    }
    else {
      this.drugList = [];
      this.drugList = this.searchTerms.pipe(
        debounceTime(300),        // wait for 300ms pause in events
        distinctUntilChanged(),   // ignore if next search term is same as previous
        switchMap(term => term   // switch to new observable each time
          // return the http search observable
          ? this.dataservice.search(this.config.Emar_Orders_SearchDrugNameData + term.replace(/[&\/\\#,+()$~%.'";:*?<>{}]/g, ''))
          // or the observable of empty heroes if no search term
          : observableOf<any[]>([{ "Drug_Id": 0, "DrugName": "No Record Found" }])),
        catchError(error => {
          // TODO: real error handling
          this.alertService.error(error.message)
          return observableOf<any[]>([]);
        }));
    }
  }
  onselectDrugbyBoth(item: any) {

    let orderType = this.myform.value.type == true ? 'order' : 'stock';
    if (orderType == "stock") {
      if (item != '') {
        this.barcodear = [];
        //this.previousDrugName = '';
        this.drugNameChanged = false;
        this.stockId = item.Stock_Id;
        this.gpiCode = item.GPICode;
        // if (item.Barcode != "")
        //   this.barcodear = item.Barcode.split(', ');
        // else if(item.Barcode=="")
        //   this.alertService.warn("No barcode exists for this drug");
        this.selectedDrugName = item.DrugName;
        this.myform.patchValue({
          drug: item.DrugName,
          //qtyHand: item.InHand,
          //barcode:item.Barcode
          controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
          type: 0
        });
        this.flag = false;

        if (item.ControlledSubstanceSchedule == 1) {
          this.isControlSubstanceReadOnly = true;
          // const maxpervalidation = this.myform.get('maxPerDay');
          // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          // maxpervalidation.updateValueAndValidity();
        }
        else {
          this.isControlSubstanceReadOnly = false;
          //   if( this.myform.value.prn !=true)
          //   {
          //   const maxpervalidation = this.myform.get('maxPerDay');
          //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          //   maxpervalidation.updateValueAndValidity();
          //  }
        }
        if (this.barcodear != null && this.barcodear.length > 0) {
          // const barcodevalidation = this.myform.get('barcode');
          // barcodevalidation.setValidators(null);
          // barcodevalidation.clearValidators();
          // barcodevalidation.updateValueAndValidity();
          this.barCodeFlag = 0;
        }
     
        if (item != '' && (item.Route !== undefined)) {
        const route_name = item.Route ; 
        const routePrefix = route_name !=null ? route_name.substring(0, 2) : 'OTH';
        this.selectedroItems = this.routes.filter(item => item.Route.startsWith(routePrefix));
        }
      
        //this.flag = false;
      }
      else {
        return false;
      }
    }
    else {
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
        this.selectedDrugName = item.DrugName;
        this.myform.patchValue({
          drug: item.DrugName,
          //qtyHand: '',
          //barcode:item.Barcode
          //route: this.selectedroItems,
          controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
          type: 1
        });
        this.flag = false;
        this.isReadOnlyforControl = false;
        if (item.ControlledSubstanceSchedule == 1) {
          this.isControlSubstanceReadOnly = true;
          // const maxpervalidation = this.myform.get('maxPerDay');
          // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          // maxpervalidation.updateValueAndValidity();
        }
        else {
          this.isControlSubstanceReadOnly = false;
          //   if( this.myform.value.prn !=true)
          //   {
          //   const maxpervalidation = this.myform.get('maxPerDay');
          //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          //   maxpervalidation.updateValueAndValidity();
          //  }
        }
        if (item != '' && (item.Route !== undefined)) {
          const route_name = item.Route ; 
          const routePrefix = route_name !=null ? route_name.substring(0, 2) : 'OTH';
          this.selectedroItems = this.routes.filter(item => item.Route.startsWith(routePrefix));
          }
        //this.flag = false;
      }
      else {
        return false;
      }
    }
  }
  onselectDrug(item: any) {

    if (item != '') {
      this.barcodear = [];
      //this.previousDrugName = '';
      this.drugNameChanged = false;
      this.stockId = item.Stock_Id;
      this.gpiCode = item.GPICode;
      // if (item.Barcode != "")
      //   this.barcodear = item.Barcode.split(', ');
      // else if(item.Barcode=="")
      //   this.alertService.warn("No barcode exists for this drug");
      this.selectedDrugName = item.DrugName;
      this.myform.patchValue({
        drug: item.DrugName,
        //qtyHand: item.InHand,
        //barcode:item.Barcode
        controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
        type: 0
      });
      this.flag = false;
      if (item.ControlledSubstanceSchedule == 1) {
        this.isControlSubstanceReadOnly = true;
        // const maxpervalidation = this.myform.get('maxPerDay');
        // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        // maxpervalidation.updateValueAndValidity();
      }
      else {
        this.isControlSubstanceReadOnly = false;
        //   if( this.myform.value.prn !=true)
        //   {
        //   const maxpervalidation = this.myform.get('maxPerDay');
        //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        //   maxpervalidation.updateValueAndValidity();
        //  }
      }

      if (this.barcodear != null && this.barcodear.length > 0) {
        // const barcodevalidation = this.myform.get('barcode');
        // barcodevalidation.setValidators(null);
        // barcodevalidation.clearValidators();
        // barcodevalidation.updateValueAndValidity();
        this.barCodeFlag = 0;
      }
      if (item != '' && (item.Route !== undefined)) {
        const route_name = item.Route ; 
        const routePrefix = route_name !=null ? route_name.substring(0, 2) : 'OTH';
        this.selectedroItems = this.routes.filter(item => item.Route.startsWith(routePrefix));
        }
      //this.flag = false;
    }
    else {
      return false;
    }
  }
  checkLiteral(value: any) {
    debugger;
    // if (value == true && this.myform.value.treatment == false) {
    if ((value == true && this.myform.value.treatment == false) || (value == true && this.myform.value.treatment == true)) { //04/15/2026 to make literal treatment order as literal order

      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.clearValidators();
      drugnameValidations.updateValueAndValidity();
      //if(this.barcodear.length)
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.clearValidators();
      // barcodevalidation.updateValueAndValidity();
      // const schduleTextValidations = this.myform.get('schduleText');
      // schduleTextValidations.clearValidators();
      // schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.clearValidators();
      doseValidations.updateValueAndValidity();

this.DoseUomSSSS = [];
      const doseUomValidations = this.myform.get('DoseUom');
      doseUomValidations.clearValidators();
      doseUomValidations.updateValueAndValidity();

      const sig1FrequencyValidations = this.myform.get('sig1Frequency');
      sig1FrequencyValidations.clearValidators();
      sig1FrequencyValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.clearValidators();
      routeValidations.updateValueAndValidity();
      const refillValidations = this.myform.get('refill');
      refillValidations.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
      refillValidations.updateValueAndValidity();
      const uomValidations = this.myform.get('UOM');
      uomValidations.clearValidators();
      uomValidations.updateValueAndValidity();
      const dawValidations = this.myform.get('daw');
      dawValidations.clearValidators();
      dawValidations.updateValueAndValidity();
      const indicationValidations = this.myform.get('indication');
      indicationValidations.clearValidators();
      indicationValidations.updateValueAndValidity();
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.clearValidators();
      indicationFreeTextValidations.updateValueAndValidity();
      const addInstValidations = this.myform.get('addInst');
      addInstValidations.setValidators([Validators.required,Validators.maxLength(250)]);
      addInstValidations.updateValueAndValidity();
      const qtyHandValidations = this.myform.get('qtyHand');
      qtyHandValidations.setValidators([Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
      qtyHandValidations.updateValueAndValidity();
      this.barCodeStatusFlag = 1;
      this.barCodeFlag = 0;
      this.stockId = 0;
      this.selectedsig1Frequency = [];
      this.sigsDoseFreqArray = [];
      this.resetAdministerScheduleForm();
      this.myform.patchValue({
        orderTypeId: 4,
        prn: '',
        refill: '0',
        dose: '',
        sig1Frequency: this.selectedsig1Frequency,
      });
    }
    else {
      this.myform.patchValue({
        drug:''
      })
      const drugnameValidations = this.myform.get('drug');
     
      drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
      drugnameValidations.updateValueAndValidity();
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
      // barcodevalidation.updateValueAndValidity();
      // const schduleTextValidations = this.myform.get('schduleText');
      // schduleTextValidations.clearValidators();
      // schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.setValidators([Validators.required]);// Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
      doseValidations.updateValueAndValidity();

      const doseUomValidations = this.myform.get('DoseUom');
      doseUomValidations.setValidators([Validators.required]);// Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
      doseUomValidations.updateValueAndValidity();



      const sig1FrequencyValidations = this.myform.get('sig1Frequency');
      sig1FrequencyValidations.setValidators([Validators.required]);
      sig1FrequencyValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.setValidators(Validators.required);
      routeValidations.updateValueAndValidity();
      const refillValidations = this.myform.get('refill');
      refillValidations.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
      refillValidations.updateValueAndValidity();
      const uomValidations = this.myform.get('UOM');
      uomValidations.setValidators(Validators.required);
      uomValidations.updateValueAndValidity();
      const dawValidations = this.myform.get('daw');
      dawValidations.setValidators(Validators.required);
      dawValidations.updateValueAndValidity();
      const qtyHandValidations = this.myform.get('qtyHand');
      qtyHandValidations.setValidators([Validators.required, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
      qtyHandValidations.updateValueAndValidity();
      if (this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '') {
        const indicationValidations = this.myform.get('indication');
        indicationValidations.setValidators(Validators.required);
        indicationValidations.updateValueAndValidity();
      }
      if (this.selectedindicaitem.length == 0) {
        const indicationFreeTextValidations = this.myform.get('indicationFreeText');
        indicationFreeTextValidations.setValidators(Validators.required);
        indicationFreeTextValidations.updateValueAndValidity();
      }
      this.barCodeStatusFlag = 0;
      if (this.barcodear.length != 0) {
        // const barcodevalidation = this.myform.get('barcode');
        // barcodevalidation.clearValidators();
        // barcodevalidation.updateValueAndValidity();
      }
      this.myform.patchValue({
        orderTypeId: this.myform.value.treatment == true ? 5 : 1,
        barcode: '',
        refill: '0',
        dose: '1'
      });
      if (this.myform.value.dose != undefined && this.myform.value.dose != null && (this.myform.value.dose == 'SS' || this.myform.value.dose == 'UD')) {
        const addInstValidations = this.myform.get('addInst');
        addInstValidations.setValidators(([Validators.required,Validators.maxLength(250)]));
        addInstValidations.updateValueAndValidity();
      }
      else {
        const addInstValidations = this.myform.get('addInst');
        addInstValidations.setValidators(Validators.maxLength(250));
        addInstValidations.updateValueAndValidity();
      }
      if (this.myform.value.treatment == false) {
        this.myform.patchValue({
          drug: ''
        });
      }
      if (this.myform.value.orderTypeId == 1) {
        this.drugNameChanged = true;
      }
    }
  }
  checkTreatment(value: any) {
    if (this.newOrderFlag != 1 && value == true && this.myform.value.literal == false) {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
      drugnameValidations.updateValueAndValidity();
      // const schduleTextValidations = this.myform.get('schduleText');
      // schduleTextValidations.setValidators([Validators.required]);
      // schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.setValidators([Validators.required]);
      doseValidations.updateValueAndValidity();
      const sig1FrequencyValidations = this.myform.get('sig1Frequency');
      sig1FrequencyValidations.setValidators([Validators.required]);
      sig1FrequencyValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.setValidators(Validators.required);
      routeValidations.updateValueAndValidity();
      const refillValidations = this.myform.get('refill');
      refillValidations.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
      refillValidations.updateValueAndValidity();
      const uomValidations = this.myform.get('UOM');
      uomValidations.setValidators(Validators.required);
      uomValidations.updateValueAndValidity();
      const dawValidations = this.myform.get('daw');
      dawValidations.setValidators(Validators.required);
      dawValidations.updateValueAndValidity();
      const qtyHandValidations = this.myform.get('qtyHand');
      qtyHandValidations.setValidators([Validators.required, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
      qtyHandValidations.updateValueAndValidity();
      if (this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '') {
        const indicationValidations = this.myform.get('indication');
        indicationValidations.setValidators(Validators.required);
        indicationValidations.updateValueAndValidity();
      }
      if (this.selectedindicaitem.length == 0) {
        const indicationFreeTextValidations = this.myform.get('indicationFreeText');
        indicationFreeTextValidations.setValidators(Validators.required);
        indicationFreeTextValidations.updateValueAndValidity();
      }
      if (this.barcodear == null || this.barcodear.length == 0) {
        // const barcodevalidation = this.myform.get('barcode');
        // barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
        // barcodevalidation.updateValueAndValidity();
        // this.barCodeFlag =1;
      }
      this.myform.patchValue({
        orderTypeId: 5,
        refill: '0',
        dose: '1'
        //literal:true,
        //drug:''
      });
      if (this.myform.value.dose != undefined && this.myform.value.dose != null && (this.myform.value.dose == 'SS' || this.myform.value.dose == 'UD')) {
        const addInstValidations = this.myform.get('addInst');
        addInstValidations.setValidators(([Validators.required, Validators.maxLength(250)]));
        addInstValidations.updateValueAndValidity();
      }
      else {
        const addInstValidations = this.myform.get('addInst');
        addInstValidations.setValidators(Validators.maxLength(250));
        addInstValidations.updateValueAndValidity();
      }
    }
    else if (this.newOrderFlag != 1 && value == false && this.myform.value.literal == true) {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.clearValidators();
      drugnameValidations.updateValueAndValidity();
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.clearValidators();
      // barcodevalidation.updateValueAndValidity();
      // const schduleTextValidations = this.myform.get('schduleText');
      // schduleTextValidations.clearValidators();
      // schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.clearValidators();
      doseValidations.updateValueAndValidity();
      const sig1FrequencyValidations = this.myform.get('sig1Frequency');
      sig1FrequencyValidations.clearValidators();
      sig1FrequencyValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.clearValidators();
      routeValidations.updateValueAndValidity();
      const refillValidations = this.myform.get('refill');
      refillValidations.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
      refillValidations.updateValueAndValidity();
      const uomValidations = this.myform.get('UOM');
      uomValidations.clearValidators();
      uomValidations.updateValueAndValidity();
      const dawValidations = this.myform.get('daw');
      dawValidations.clearValidators();
      dawValidations.updateValueAndValidity();
      const indicationValidations = this.myform.get('indication');
      indicationValidations.clearValidators();
      indicationValidations.updateValueAndValidity();
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.clearValidators();
      indicationFreeTextValidations.updateValueAndValidity();
      const addInstValidations = this.myform.get('addInst');
      addInstValidations.setValidators(([Validators.required,Validators.maxLength(250)]));
      addInstValidations.updateValueAndValidity();
      const qtyHandValidations = this.myform.get('qtyHand');
      qtyHandValidations.setValidators([Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
      qtyHandValidations.updateValueAndValidity();
      this.selectedsig1Frequency = [];
      this.sigsDoseFreqArray = [];
      this.resetAdministerScheduleForm();
      this.myform.patchValue({
        orderTypeId: 4,
        prn: '',
        refill: '0',
        dose: '',
        sig1Frequency: this.selectedsig1Frequency,
      });
    }
    // else if ((this.myform.value.literal == true && this.myform.value.treatment == true) || (this.myform.value.literal == false && this.myform.value.treatment == true) || (this.myform.value.literal == false && this.myform.value.treatment == false) || (this.myform.value.literal == true && this.myform.value.treatment == false)) {
    else if ((this.myform.value.literal == false && this.myform.value.treatment == true) || (this.myform.value.literal == false && this.myform.value.treatment == false) ||  (this.myform.value.literal == true && this.myform.value.treatment == false)) { // 04/15/2026 to make literal treatment order into literal order.
     
      if (value == true) {
        this.myform.patchValue({
          //literal:true,
          //drug:''
        })
      }
      this.checkLiteral(this.myform.value.literal);
    }
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
      if (this.newOrderFlag == 1 && this.hoaObj != null) {
        if (this.hoaObj.freqId == 1) {
          this.hoaObj.freqId = null;
          // this.alertService.warn('Frequency is set to PRN. Change Frequency in order to change PRN field');
          // this.myform.patchValue({
          //   prn: true
          // });
          this.myform.patchValue({
            schduleText: ''
          });
          this.isShiftSchedule = false;
          this.isHoursReadOnly = false;
          this.selectedstItems = [];
          this.selectedfrequencyItems = [];
          this.scheduleform.patchValue({
            frequency: this.selectedfrequencyItems,
            either: this.selectedstItems,
            hours: '',
          });
        }
      }
      else if (this.newOrderFlag != 1) {
        this.ng4LoadingSpinnerService.show();
        this.dataservice.get<DrugAdministrationTime>(this.config.Emar_Orders_GetScheduleTimeDetails + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId)
          .subscribe(res => {
            if (res != null) {
              //if (res.NursingFreqId == 1) {
              if (this.frequencyList.find(f => f.Frequency_Id == res.NursingFreqId.toString()).Frequency_PRN == 1) {
                this.alertService.warn('Frequency is set to PRN. Change Frequency in order to change PRN field');
                this.myform.patchValue({
                  prn: true
                });
                // const maxpervalidation = this.myform.get('maxPerDay');
                // maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
                // Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
                // Validators.pattern(/^\d*(\.\d{0,3})?$/),
                // Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
                // maxpervalidation.updateValueAndValidity();
                const maxpervalidation = this.myform.get('maxPerDay');
                maxpervalidation.setValidators([Validators.required]);
                maxpervalidation.updateValueAndValidity();
                this.isInteger();
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
      }
      // if(this.myform.value.controlSubstance !=true)
      // {
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
      //maxpervalidation.clearValidators();
      maxpervalidation.updateValueAndValidity();
      //}
    }
  }
  prnSchedule() {
    if (this.hoaObj == null) {
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
        NurseStationId: this.selectedNursestation[0].NurseStation_Id,
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
            this.selectedMonths = [];
            this.selectedWeeks = [];
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
            if (this.myform.value.prn == true && this.newOrderFlag != 1) {
              //this.insertScheduleTimes();
              this.btnHoaSaveFlag = 2;
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
  hoaOpen() {
    this.ng4LoadingSpinnerService.show();
    if (this.hoaObj == null)
      this.GetFrequencyMasterData();
    this.modalHOAIsOpen = true;
    this.ng4LoadingSpinnerService.hide();
  }
  getAllBarcodes() {
    let barcodeFacilityId = this.barcodeFacilityId == null || this.barcodeFacilityId == undefined ? 0 : this.barcodeFacilityId
    this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllBarcodes + barcodeFacilityId)
      .subscribe(res => {
        this.barcodesList = res;
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getCpoeSourceDrop() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetCpoeSourceDrop + this.userID)
      .subscribe(res => {
        this.sourceData = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getEditCpoeSourceDrop(SourceId) {

    this.dataservice.get<any[]>(this.config.Emar_Orders_GetCpoeSourceDrop +this.userID+"/" + SourceId)
      .subscribe(res => {
        this.sourceData = res;
        this.selectedsourceItems = [];
        if (this.sourceData.length>0) {
          let selectedSource = this.sourceData.filter(item => item.Source_Id == SourceId)[0];
          if (selectedSource != undefined) {
            this.selectedsourceItems.push(selectedSource);
          }
        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetQuantityDoseDrop() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetQuantityDoseDrop)
      .subscribe(res => {
        this.qtyDoseData = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetUnitMeasurementsDrop() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetUnitMeasurementsDrop)
      .subscribe(res => {
        this.unitMeasureData = res;
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
  getDoseUom() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetDoseUom)
      .subscribe(res => {
        this.DoseUomDrop = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  onDrugNameChange(drugName?: string) {
    debugger
    this.stockId = 0;
    this.gpiCode = '';
    this.drugNameChanged = true;
    this.searchDrug(drugName);
  }
  searchDrug(term: string): void {
    // if (term.length > 2 && ((this.myform.value.literal == true && this.myform.value.treatment == true) || (this.myform.value.literal == false && this.myform.value.treatment == true) || (this.myform.value.literal == false && this.myform.value.treatment == false))) {
    if (term.length > 2 && ((this.myform.value.literal == false && this.myform.value.treatment == true) || (this.myform.value.literal == false && this.myform.value.treatment == false))) {
    
      this.flag = true;
      this.searchTerms.next(term);

    }
    else {
      this.flag = false;
    }
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
  onDaysSelect(item: any) {
    this.selectedDays.push(item);
    this.selectedWeeks = [];
    this.scheduleform.patchValue({
      week: this.selectedWeeks,
    })
    this.isWeeksDisabled = true;
    // this.selectedDays = item.User_Id;
    // this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onDaysDeSelect(item: any) {
    var index = this.selectedDays.indexOf(item);
    this.selectedDays.splice(index, 1);
    this.selectedWeeks = [];
    this.scheduleform.patchValue({
      week: this.selectedWeeks,
    });
    if (this.selectedDays.length == 0) {
      this.isWeeksDisabled = false;
    }
    // this.filterIds.User_Id = 0;
    // this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onMonthSelect(item: any) {

    if (item.Month_Id == 13) {
      this.selectedMonths = [];
      this.selectedMonths.push(item.Month_Id);
      let selectMonths = [];
      selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
      this.scheduleform.patchValue({
        month: selectMonths,
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
    else {
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
    if (item.Week_Id == 1) {
      this.selectedWeeks = [];
      this.selectedWeeks.push(item.Week_Id);
      let selectWeek = [];
      selectWeek = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
      this.isdaysDisabled = true;
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
        limitSelection: 1,
      };
    }
    else {
      this.selectedWeeks.push(item.Week_Id);
      this.selectedDays = [];
      this.scheduleform.patchValue({
        days: this.selectedDays,
      });
      this.isdaysDisabled = true;
    }
  }
  onWeekDeSelect(item: any) {
    var index = this.selectedWeeks.indexOf(item.Week_Id);
    this.selectedWeeks.splice(index, 1);
    this.selectedDays = [];
    this.scheduleform.patchValue({
      days: this.selectedDays,
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
      limitSelection: 6,
    };
  }
  saveExistingOrder(controlSubstanceFlag: number) {
    controlSubstanceFlag = this.myform.value.controlSubstance == true ? 1 : 0;
    let barcode = this.barcodear.join();
    if (this.myform.value.prn == true && this.newOrderFlag != 1 && this.btnHoaSaveFlag == 2) {
      this.scheduleSave = 2;
      this.insertScheduleTimes(controlSubstanceFlag);
    }
    if (this.newOrderFlag != 1 && this.btnHoaSaveFlag == 1 && this.scheduleSave == 1) {
      this.orderUpdateObj = {
        PatientId: this.residentId,
        POrderId: this.orderId,
        PQuantityId: this.quantityId,
        PhysicianId: this.myform.value.physician[0].Physician_Id,
        DrugName: this.myform.value.drug,
        //PharmacyName:this.myform.value.pharmacy,
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
        Route: this.myform.value.route.length != 0 ? this.myform.value.route[0].Route_Id : '',
        controlSubstance: controlSubstanceFlag,
        Inhand: this.myform.value.qtyHand,
        Createdby: this.userID,
        Barcode: barcode.replace(/\s/g, ""),
        DAdminId: this.dAdminId,
        RequestedGiveCode: this.gpiCode,
        OrderTypeID: (this.myform.value.literal == true && this.myform.value.treatment == true) ? 2 : (this.myform.value.literal == true && this.myform.value.treatment == false) ? 4 : (this.myform.value.literal == false && this.myform.value.treatment == true) ? 5 : 1,//this.myform.value.orderTypeId
        OrderUpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        ScheduleText: this.myform.value.schduleText,

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
                this.getOrderGridData("Active", 1);
                this.alertService.success("Reviewed successfully");
                this.reviewClickedFlag = 0;
              }, error => {
                this.ng4LoadingSpinnerService.hide();
                this.alertService.error(this.errorMessage);
              });
          }
          else {
            this.getOrderGridData('Active', 1);
            this.alertService.success("Order updated successfully");
          }
          this.modalControlIsOpen = false;
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(this.errorMessage);
          this.modalControlIsOpen = false;
        });
    }
    this.sharedService.saveChangesOrderInfo(0);
  }
  CompletePRNReview() {
    this.prnSchedleCheckModal = false;
    this.saveExistingOrder(0);
  }
  OpenHOAForPRN() {
    this.prnSchedleCheckModal = false;
    this.modalHOAIsOpen = true;
  }
  insulinOpen() {
    this.modalInsuliIsOpen = true;
  }
  getFacilityNSResidentsDataByPId() {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetFacilityNSResidentsDataByPId + this.residentId)
      .subscribe(res => {
        this.nurseStations = res.NSDrop;
        this.nurseStationId = res.NursingStationId;
        this.residents = res.ResidentDrop;
        this.facilityName = res.FacilityName;
        this.selectedNursestation = this.nurseStations.filter(item => item.NurseStation_Id == this.nurseStationId);
        this.selectedResItem = this.residents.filter(item => item.Patient_Id == this.residentId);
        this.myform.patchValue({
          nursestationName: this.selectedNursestation,
          ddlresidents: this.selectedResItem
        });
        //this.getAllFlagsForCompanyByNSId(this.nurseStationId,this.residentId);
        this.GetPhysicianByUserRes();
        this.GetPhysicianDropData(this.nurseStationId);
        this.getNursingStationTimeZone(this.nurseStationId);
        //this.GetDefultNPIByFailityIdNUrsId();

        this.getOrderRoutes();
        this.getCpoeSourceDrop();
        this.GetQuantityDoseDrop();
        this.GetUnitMeasurementsDrop();
        this.getAllBarcodes();
        this.getDemographicInfoData();
        this.loadSearchData();
this.getDoseUom();
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNursingStationTimeZone(stationId: number) {
    debugger;
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        this.minStartDate = this.dateFormatPipe.transformISODate(res);
        const toDateMaxAsDate = new Date(this.minStartDate);
        toDateMaxAsDate.setMonth(toDateMaxAsDate.getMonth() - 1);
        this.StartOnemonthDate =this.dateFormatPipe.transformISODate(toDateMaxAsDate);
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getDiagnosisDetails() {
    this.dataservice.get<OrderInfoAlert>(this.config.Emar_Orders_GetDiagnosisDetails + this.residentId)
      .subscribe(res => {
        this.ordersInfo = {
          Allergy: res.Allergy,
          Diet: res.Diet,
          Diagnosis: res.Diagnosis,
        };
        if (res.Allergy.length > 90)
          this.residentAllergies = res.Allergy.substr(0, 90);
        else
          this.residentAllergies = res.Allergy;
        if (res.Diagnosis.length > 90)
          this.residentDiagnosis = res.Diagnosis.substr(0, 90);
        else
          this.residentDiagnosis = res.Diagnosis;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getResidentDiagnosis() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographicDiagnosis_GetResDiagnosis + this.residentId)
      .subscribe(res => {
        this.indications = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  getDemographicInfoData() {
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.patientid=res.Patient_Id
        this.demographicInfoData = res;
        this.getPatientType();
        this.getDiagnosisDetails();
        this.getResidentDiagnosis();
        this.getID =res.Patient_Id;
        this.getWeightHeight();
        //this.GetPhysicianByUserRes();
        if (this.orderId == 0 && this.quantityId == 0 && this.newResOrderFlag == true) {
          this.orderGridData = [];
          this.getFavouritesMasterData();
          this.newOrder();
          this.getOrderGridData("Active", 1);
        }
        else {
          this.getOrderGridData("Active", 1);
        }
        //this.getDrFirstOrderCheck();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  GetPhysicianDropData(nsId: number) {
    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + this.userID + "/" + nsId+"/"+this.orderId)
  //this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + null +"/"+ 0)
      .subscribe(res => {
        this.physicianfullList = res;
        this.newOrderFlag == 1 ? this.physiciansdrop =  this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop =  this.physicianfullList;
        this.getFrequencyList(nsId);
      }, error => {
        this.alertService.error(error.message);
      });
  }
  GetPhysicianByUserRes() {
    this.dataservice.get<any>(this.config.Emar_Orders_GetPhysicianNPIByRoleRes + this.userID + "/" + this.residentId+"/"+this.nurseStationId)
      .subscribe(res => {
        this.defaultPhysicianNPI = res == undefined || res == null || res == "" ? null : res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.modalholdIsOpen = false;
    this.modalfavIsOpen = false;
    this.modalInsuliIsOpen = false;
    this.modalHOAIsOpen = false;
    this.modaleMar = false;
    this.modalControlIsOpen = false;
    this.userCredeatialsModal = false;
    this.modalAdditionalAdministrationSchedules = false;
    this.modalCancelAdditionalScheduleIsOpen=false;
    this.rightGridOnHoldChangesFlag = false;
    this.rightGridDCChangesFlag = false;
    this.supervisingInactiveModal = false;
    this.daysSupplyFlag = true;
    this.profileDCChangesCheckState = {};
    this.profileONHoldChangesCheckState ={};
    this.updateQtyHand(this.sigsDoseFreqArray);
    this.ng4LoadingSpinnerService.hide();
    this.alertList = [];
    this.alertAllergyList=[];
  }
  cancelHOAChanges() {
    //this.GetScheduleTimeDetails();
    this.modalHOAIsOpen = false;
  }
  getOrderStockDetailsByOrderId() {
    this.dataservice.get<any>(this.config.Emar_GetOrderStock + this.orderId)
      .subscribe(res => {
        //this.destroyform.patchValue({
        //quantity: res.Remaining
        //});
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(this.errorMessage);
      });
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  addBarcode(value: any) {
    if (this.barCodeStatusFlag != 1 && this.myform.value.orderTypeId != 4) {
      this.barCodeFlag = 1;
    }
    if (value != '') {
      let barcode = this.myform.value.barcode;
      let barcodevalue = (barcode.split('/'))[0];
      //this.BarcodeValue = barcodevalue[0];
      if (barcodevalue != '') {
        // console.log(barcode);
        // console.log("Barcode list - "+ this.barcodesList);
        if (this.myform.value.type == 1) {
          let result1 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() == this.gpiCode.toLowerCase() : false));
          let result2 = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false);
          let result3 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() == this.gpiCode.toLowerCase() : false) && (x.Patient_Id != null ? x.Patient_Id != this.residentId : false));
          //let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcode.toLowerCase() : false);
          // this.alertService.warn('r' + result1);
          // let result = this.barcodesList.find(x => x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, ''));
          let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;
          if ((result2.length > 0 && result1.length == 0) || result3.length > 0 || checkInBarcodeArray != undefined) {
            //this.alertService.error("Barcode Already Exists");
            if ((this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() != this.gpiCode.toLowerCase() : false) && x.Patient_Id != null ? x.Patient_Id == this.residentId : false).length > 0)) {
              this.alertService.warn("Barcode already assigned to a different medication");
            }
            else if (checkInBarcodeArray != undefined) {
              this.alertService.warn("Barcode already assigned to the same medication for the same resident");
            }
            else {
              //this.alertService.warn("This barcode is assigned to an order for another resident");
              this.alertService.warn("Barcode already assigned to an order");
            }
            this.myform.patchValue({
              barcode: ''
            });
            if (this.barCodeStatusFlag != 1 && this.myform.value.orderTypeId != 4) {
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
        else {
          if (this.gpiCode != '') {
            let result1 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() == this.gpiCode.toLowerCase() : false));
            let result2 = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false);
            let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;
            if ((result2.length > 0 && result1.length == 0)) {
              //this.alertService.error("Barcode Already Exists");
              this.alertService.warn("Barcode already assigned to a different medication");
              this.myform.patchValue({
                barcode: ''
              });
              if (this.barCodeStatusFlag != 1 && this.myform.value.orderTypeId != 4) {
                this.barCodeFlag = 1;
              }
            }
            else if (checkInBarcodeArray != undefined) {
              this.alertService.warn("Barcode already assigned to the same drug");
              this.myform.patchValue({
                barcode: ''
              });
              if (this.barCodeStatusFlag != 1 && this.myform.value.orderTypeId != 4) {
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
          else {
            this.alertService.error("Please select drug");
          }
        }
      }

    }
    if (this.barcodear != null && this.barcodear.length != 0) {
      if (this.barCodeStatusFlag != 1) {
        this.barCodeFlag = 0;
      }
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.setValidators(null);
      // barcodevalidation.clearValidators();
      // barcodevalidation.updateValueAndValidity();

    }
  }
  removeBarcode(i: number) {
    this.barcodear.splice(i, 1);
    if (this.barcodear.length == 0 && this.myform.value.orderTypeId != 4) {
      if (this.barCodeStatusFlag != 1) {
        this.barCodeFlag = 1;
      }
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.setValidators([Validators.required]);
      // barcodevalidation.updateValueAndValidity();

    }
  }
  getOrderGridData(status: string, tabClick: number, discardChanges?: number) {
    ;
    this.ng4LoadingSpinnerService.show();
    this.sharedService.saveChangesOrderInfo(0);
    this.valueChangesFlag = 0;
    if (discardChanges == 1) {
      this.discardConditionalFlag = 1;
    }

    this.activeTab = status;
    if (tabClick == 0)
      this.orderId = 0;
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderGridData + this.residentId + "/" + status)
      .subscribe(res => {
        ;
        this.orderGridData = res;
        this.GetResidentAllOrdersData();
        if (res.length > 0) {
          //this.pendingReviewCount = res.filter(item => item.ReviewFlag != 1).length;
          this.reactivateStatus = 0;
          if (this.orderId == 0) {
            //this.newOrder();
            // if (this.orderGridData.length > 0) {
            //   this.orderId = this.orderGridData[0].porder_Id;
            //   this.sharedService.changeOrderId(this.orderId);
            //   this.sharedService.changeQuantityId(this.orderGridData[0].PQuantity_Id);
            //   if(this.orderGridData[0].DAdmin_Id !=null && this.orderGridData[0].DAdmin_Id !=undefined){
            //   this.dAdminId = this.orderGridData[0].DAdmin_Id;
            // }
            //   this.getOrderDetailsbyorderId(this.orderId, this.orderGridData[0].PQuantity_Id, this.dAdminId);
            // }
          }
          else if (this.orderId != 0) {
            let records = res.find(item => item.porder_Id == this.orderId && item.PQuantity_Id == this.quantityId);
            if (records != null && records != undefined) {
              this.dAdminId = res.find(item => item.porder_Id == this.orderId && item.PQuantity_Id == this.quantityId).DAdmin_Id;
              this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);
            }
            else if (records == undefined) {
              if (this.orderGridData.length > 0) {
                this.orderId = this.orderGridData[0].porder_Id;
                this.sharedService.changeOrderId(this.orderId);
                this.sharedService.changeQuantityId(this.orderGridData[0].PQuantity_Id);
                if (this.orderGridData[0].DAdmin_Id != null && this.orderGridData[0].DAdmin_Id != undefined) {
                  this.dAdminId = this.orderGridData[0].DAdmin_Id;
                }
                this.getOrderDetailsbyorderId(this.orderId, this.orderGridData[0].PQuantity_Id, this.dAdminId);
              }
            }
          }
        }
        else {
          this.newOrder();
        }
        this.getAllBarcodes();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getOrderDetailsbyorderId(orderId: number, quantityId: number, dAdminId: number) {



    if (this.valueChangesFlagReceive == 1 && this.newOrderFlag == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          //this.valueChangesFlagReceive = 0;
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.myform.disable();
    this.DisableFag = true;
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive = 0;
          this.ng4LoadingSpinnerService.show();
          this.orderId = orderId;
          this.quantityId = quantityId;
          this.dAdminId = dAdminId;
          //this.notesFlag = 0;
          this.dataservice.get<CPOEOrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId  + "/" + this.userID)
            .subscribe(res => {
              this.isReadOnlyforControl = false;
              let source=res.Source != undefined && res.Source != null?res.Source:0;
              this.getEditCpoeSourceDrop(source);
              this.getOrderStockDetailsByOrderId();
              this.getFavouritesMasterData();
              //this.GetScheduleTimeDetails();
              this.newOrderFlag = 0;
              this.ordersDetails = res;
              this.resetAdministerScheduleForm();
              this.fetchOrdersData(res);
              this.selectedOrder = orderId;
              this.selectedOrderQuantity = quantityId;
              this.selectedOrderDADminId = dAdminId;
              this.hoaObj = null;
              //this.getNurseCommentNotesByQuantityId();
              this.ng4LoadingSpinnerService.hide();
            }, error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();
            });
        }
        modalRef.close();
      });
    }
    else {
      this.ng4LoadingSpinnerService.show();
      this.orderId = orderId;
      this.quantityId = quantityId;
      this.dAdminId = dAdminId;
      //this.notesFlag = 0;
      this.dataservice.get<CPOEOrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId  + "/" + this.userID)
        .subscribe(res => {
          this.isReadOnlyforControl = false;
          let source=res.Source != undefined && res.Source != null?res.Source:0;
          this.getEditCpoeSourceDrop(source);
          this.getOrderStockDetailsByOrderId();
          this.getFavouritesMasterData();
          //this.GetScheduleTimeDetails();
          this.newOrderFlag = 0;
          this.ordersDetails = res;
          this.resetAdministerScheduleForm();
          this.fetchOrdersData(res);
          this.selectedOrder = orderId;
          this.selectedOrderQuantity = quantityId;
          this.selectedOrderDADminId = dAdminId;
          this.hoaObj = null;
          //this.getNurseCommentNotesByQuantityId();
          this.valueChangesFlagReceive = 0;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

    }
  }
  // getUserRecentFacilityNurseStations() {
  //   let userId = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.sharedService.getUserRecentFacNs(userId)
  //     .subscribe(res => {
  //       if (res != undefined) {
  //         this.loginUserReceFacility = res.Facility_Id;
  //         this.loginUserReceNurseStation = res.NurseStation_Id;
  //       }
  //      // this.getFiltersData(this.userId);
  //     }, error => {
  //       this.alertService.error(error.message);
  //     });
  // }
  newOrder() {
    debugger;
    this.DoseUomSSSS = [];
    // this.myform.patchValue({
    //   pharmacy: item.PharmacyName,
    // });
    this.pharmalifeInput = true;
    this.myform.enable();
    this.DisableFag = false;
    this.getCpoeSourceDrop();
    this.supplydays ='1';
    this.qtyhandTouched = false;
    //this.defaultNameChange();

    //this.defaultNameChange();
    if (this.valueChangesFlagReceive == 1 && this.newOrderFlag == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive = 0;
          if (this.residentId != 0) {
            if (this.demographicInfoData.PVisit_Status == 2) {
              this.alertService.warn("Resident has been discharged.")
            }
            else if (this.demographicInfoData.PVisit_Status == 3) {
              this.alertService.warn("Status updated to temporarily inactive.")
            }
            else {
              //this.destroyform.reset();

              this.newOrderFlag = 1;
              this.valueChangesFlag = 1;
              this.sharedService.saveChangesOrderInfo(0);

              //this.getCpoeSourceDrop();

              this.reviewClickedFlag = 0;
              this.favstatus = 0;
              this.splits = 0;
              this.reactivateStatus = 0;
              this.favForm.reset();
              this.favobj = [];
              this.favouriteObj = [];
              this.selectedphyItems = [];
              this.selectedroItems = [];
              this.selectedindicaitem = [];
              this.barCodeStatusFlag = 0;
              this.userAttempts = 0;
              this.selectedsourceItems = [];
              this.selectedsig1Frequency = [];
              this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
              if(this.physiciansdrop ==undefined || this.physiciansdrop==null)
              {
                setTimeout(() => {
                  if (this.NPIbyFFacilityIdNursId != null && this.NPIbyFFacilityIdNursId != "" && (this.defaultPhysicianNPI=="" || this.defaultPhysicianNPI==undefined)) {

                    let checkExist9 = this.physiciansdrop.find(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId);
                    if (checkExist9 != undefined) {
                      this.selectedphyItems = [];
                      this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId)[0]);
                      this.myform.patchValue({
                        physician: this.selectedphyItems,
                      });
                    }

                    }
                    else{

                      if (this.defaultPhysicianNPI != null) {

                        let checkExist = this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI);
                        if (checkExist != undefined) {

                          this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
                          this.myform.patchValue({
                            physician: this.selectedphyItems,
                          });
                        }


                    }
                  }
                }, 3000);
              }
              else{
                if (this.NPIbyFFacilityIdNursId != null && this.NPIbyFFacilityIdNursId != "" && (this.defaultPhysicianNPI=="" || this.defaultPhysicianNPI==undefined)) {

                  let checkExist9 = this.physiciansdrop.find(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId);
                  if (checkExist9 != undefined) {
                    this.selectedphyItems = [];
                    this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId)[0]);
                    this.myform.patchValue({
                      physician: this.selectedphyItems,
                    });
                  }

                  }
                  else{

                    if (this.defaultPhysicianNPI != null) {

                      let checkExist = this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI);
                      if (checkExist != undefined) {

                        this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
                        this.myform.patchValue({
                          physician: this.selectedphyItems,
                        });
                      }


                  }
                }

              }


              // let routeExists = this.routes.find(r => r.Route_Id == 34);
              // if (routeExists != undefined) {
              //   this.selectedroItems.push(this.routes.filter(r => r.Route_Id == 34)[0]);
              // }
              if(this.sourceData ==undefined || this.sourceData==null || this.sourceData.length==0)
              {
                setTimeout(() => {
              if (this.persistanceService.get('userRole') == '\"PHYSICIAN\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              if (this.persistanceService.get('userRole') == '\"PRESCRIBER\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              this.supplydays = '1';
              this.myform.patchValue({
                physician: this.selectedphyItems,
                route: this.selectedroItems,
                indication: this.selectedindicaitem,
                indicationFreeText:'',
                source: this.selectedsourceItems,
                qtyHand: '1',
                drug: '',
               // pharmacy:this.pharmacyDefault[0].PharmacyName,
                dose: '1',
                refill: '0',
                addInst: '',
                notes: '',
                insulincomments: '',
                barcode: '',
                startDate: this.minStartDate,
                writtenDate: this.minStartDate,
                maxPerDay: '',
                alertText: '',
                endDate: '',
                waitforpharmacy: '',
                suppliedanother: '',
                daw: '0',
                type: 1,
                schduleText: '',
                self: '',
                treatment: '',
                literal: '',
                prn: '',
                controlSubstance: '',
                orderTypeId: 1,
                UOM: '',
                sig1Frequency: this.selectedsig1Frequency,
              });
            }, 3000);

            }
            else
            {
              if (this.persistanceService.get('userRole') == '\"PHYSICIAN\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              if (this.persistanceService.get('userRole') == '\"PRESCRIBER\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              this.supplydays = '1';
              this.myform.patchValue({
                physician: this.selectedphyItems,
                route: this.selectedroItems,
                indication: this.selectedindicaitem,
                indicationFreeText:'',
                source: this.selectedsourceItems,
                qtyHand: '1',
                drug: '',
              //  pharmacy:this.pharmacyDefault[0].PharmacyName,
                dose: '1',
                refill: '0',
                addInst: '',
                notes: '',
                insulincomments: '',
                barcode: '',
                startDate: this.minStartDate,
                writtenDate: this.minStartDate,
                maxPerDay: '',
                alertText: '',
                endDate: '',
                waitforpharmacy: '',
                suppliedanother: '',
                daw: '0',
                type: 1,
                schduleText: '',
                self: '',
                treatment: '',
                literal: '',
                prn: '',
                controlSubstance: '',
                orderTypeId: 1,
                UOM: '',
                sig1Frequency: this.selectedsig1Frequency,
              });

            }


              this.barcodear = [];
              if(this.userRole=='\"PHYSICIAN\"' ||this.userRole=='\"PRESCRIBER\"')
              {
                this.isReadOnlyPhy = true;
              }
              else
              {
                this.isReadOnlyPhy = false;
              }
              //this.isReadOnly = false;
              this.ordersDetails = {} as CPOEOrdersData;
              this.selectedOrder = 0;
              this.selectedOrderQuantity = 0;
              this.selectedOrderDADminId = 0;
              this.orderId = 0;
              this.quantityId = 0;
              this.stockId = 0;
              this.gpiCode = '';
              this.resetScheduleForm();
              //this.mergeFlag = 0;
              this.isReadOnlyforControl = false;
              this.isControlSubstanceReadOnly = false;
              this.isDrugOrder = false;
              //this.notesFlag = 1;
              this.barCodeFlag = 1;
              this.sigsDoseFreqArray = [];
              this.resetAdministerScheduleForm();
              const maxpervalidation = this.myform.get('maxPerDay');
              maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
              Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
              Validators.pattern(/^\d*(\.\d{0,3})?$/),
              Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
              maxpervalidation.updateValueAndValidity();

              if (this.barcodear.length == 0) {
                // const barcodevalidation = this.myform.get('barcode');
                // barcodevalidation.setValidators([Validators.required]);
                // barcodevalidation.updateValueAndValidity();

              }
              const drugnameValidations = this.myform.get('drug');
              drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
              drugnameValidations.updateValueAndValidity();
              // const barcodevalidation = this.myform.get('barcode');
              // barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
              // barcodevalidation.updateValueAndValidity();
              // const schduleTextValidations = this.myform.get('schduleText');
              // schduleTextValidations.clearValidators();
              // schduleTextValidations.updateValueAndValidity();
              const doseValidations = this.myform.get('dose');
              doseValidations.setValidators([Validators.required]);
              doseValidations.updateValueAndValidity();
              const sig1FrequencyValidations = this.myform.get('sig1Frequency');
              sig1FrequencyValidations.setValidators([Validators.required]);
              sig1FrequencyValidations.updateValueAndValidity();
              const routeValidations = this.myform.get('route');
              routeValidations.setValidators(Validators.required);
              routeValidations.updateValueAndValidity();
              const refillValidations = this.myform.get('refill');
              refillValidations.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
              refillValidations.updateValueAndValidity();
              const uomValidations = this.myform.get('UOM');
              uomValidations.setValidators(Validators.required);
              uomValidations.updateValueAndValidity();
              const dawValidations = this.myform.get('daw');
              dawValidations.setValidators(Validators.required);
              dawValidations.updateValueAndValidity();
              const qtyHandValidations = this.myform.get('qtyHand');
              qtyHandValidations.setValidators([Validators.required, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
              qtyHandValidations.updateValueAndValidity();
              if (this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '') {
                const indicationValidations = this.myform.get('indication');
                indicationValidations.setValidators(Validators.required);
                indicationValidations.updateValueAndValidity();
              }
              if (this.selectedindicaitem.length == 0) {
                const indicationFreeTextValidations = this.myform.get('indicationFreeText');
                indicationFreeTextValidations.setValidators(Validators.required);
                indicationFreeTextValidations.updateValueAndValidity();
              }
              const addInstValidations = this.myform.get('addInst');
              addInstValidations.setValidators(Validators.maxLength(250));
              addInstValidations.updateValueAndValidity();
            }
          }
          else
            this.alertService.error('Select resident');
        }
        modalRef.close();
      });
    }
    else {
      if (this.residentId != 0) {
        if (this.demographicInfoData.PVisit_Status == 2) {
          this.alertService.warn("Resident has been discharged.")
        }
        else if (this.demographicInfoData.PVisit_Status == 3) {
          this.alertService.warn("Status updated to temporarily inactive.")
        }
        else {
          //this.destroyform.reset();
          this.newOrderFlag = 1;
          this.valueChangesFlag = 1;
          this.sharedService.saveChangesOrderInfo(0);
          //this.getCpoeSourceDrop();
          this.reviewClickedFlag = 0;
          this.favstatus = 0;
          this.splits = 0;
          this.reactivateStatus = 0;
          this.favForm.reset();
          this.favobj = [];
          this.favouriteObj = [];
          this.selectedphyItems = [];
          this.selectedroItems = [];
          this.selectedindicaitem = [];
          this.userAttempts = 0;
          this.selectedsourceItems=[];
          this.selectedsig1Frequency = [];
          if(this.physiciansdrop ==undefined || this.physiciansdrop==null)
              {
                setTimeout(() => {
                  if (this.NPIbyFFacilityIdNursId != null && this.NPIbyFFacilityIdNursId != "" && (this.defaultPhysicianNPI=="" || this.defaultPhysicianNPI==undefined)) {
                    let checkExist9 = this.physiciansdrop.find(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId);
                    if (checkExist9 != undefined) {
                      this.selectedphyItems = [];
                      this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId)[0]);
                      this.myform.patchValue({
                        physician: this.selectedphyItems,
                      });
                    }

                    }
                    else{
                      if (this.defaultPhysicianNPI != null) {

                        let checkExist = this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI);
                        if (checkExist != undefined) {
                          this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
                         if(this.selectedphyItems !=null && this.selectedphyItems.length){
                               this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
                          if(this.selectedphyItems[0].PStatus == 0 ){
                            this.supervisingInactiveModal = true;
                            this.physicianAlertMsg ="Selected Physician is inactive"
                          }
                          else if( (this.selectedphyItems[0].CredeValue == 'NP' || this.selectedphyItems[0].CredeValue == 'PA' || this.selectedphyItems[0].CredeValue == 'Other' ) && (this.selectedphyItems[0].SPhy ==null || this.selectedphyItems[0].SPhy == '' || this.selectedphyItems[0].SPhy.length ===0)){
        
                            this.supervisingInactiveModal = true;
                            this.physicianAlertMsg = "“Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) has incomplete credentialing info. Please select another Prescriber"

                          }
                          else if(this.selectedphyItems[0].SPhy !=null && this.selectedphyItems[0].SPhy != '' && this.selectedphyItems[0].SPhyStatus==0){
            
                            this.supervisingInactiveModal = true;
                           this.physicianAlertMsg = "Selected Prescriber's ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) Supervising Physician( " + this.selectedphyItems[0].SphyName + " )is inactive." ;


                          }
                          else if(this.selectedphyItems.length && this.selectedphyItems[0].Credentials == 0){
    
                              this.supervisingInactiveModal = true;
                              this.physicianAlertMsg = "Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"

                          }
                            else{
                              this.supervisingInactiveModal = false;

                             this.myform.patchValue({
                               physician: this.selectedphyItems,
                              });
                            }
                        }
                        else{
                          this.myform.patchValue({physician: '',});
                        }

                        }

                    }
                  }
                }, 3000);
              }
              else
              {
                if (this.NPIbyFFacilityIdNursId != null && this.NPIbyFFacilityIdNursId != "" && (this.defaultPhysicianNPI=="" || this.defaultPhysicianNPI==undefined)) {
                  let checkExist9 = this.physiciansdrop.find(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId);
                  if (checkExist9 != undefined) {
                    this.selectedphyItems = [];
                    this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId)[0]);
                    this.myform.patchValue({
                      physician: this.selectedphyItems,
                    });
                  }

                  }
                  else{
                    if (this.defaultPhysicianNPI != null) {

                      let checkExist = this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI);
                      if (checkExist != undefined) {
                        this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
                        if(this.selectedphyItems !=null && this.selectedphyItems.length){
                              this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
                          if(this.selectedphyItems[0].PStatus == 0 ){
                            this.supervisingInactiveModal = true;
                            this.physicianAlertMsg ="Selected Physician is inactive"
                          }
                          else if( (this.selectedphyItems[0].CredeValue == 'NP' || this.selectedphyItems[0].CredeValue == 'PA' || this.selectedphyItems[0].CredeValue == 'Other' ) && (this.selectedphyItems[0].SPhy ==null || this.selectedphyItems[0].SPhy == '' || this.selectedphyItems[0].SPhy.length ===0)){
                            
                            this.supervisingInactiveModal = true;
                            this.physicianAlertMsg = "“Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) has incomplete credentialing info. Please select another Prescriber"
                           
                          }
                          else if(this.selectedphyItems[0].SPhy !=null && this.selectedphyItems[0].SPhy != '' && this.selectedphyItems[0].SPhyStatus==0){
                            
                            this.supervisingInactiveModal = true;
                            this.physicianAlertMsg = "Selected Prescriber's ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) Supervising Physician( " + this.selectedphyItems[0].SphyName + " )is inactive." ;

                          }
                          else if(this.selectedphyItems.length && this.selectedphyItems[0].Credentials == 0){
                   
                              this.supervisingInactiveModal = true;
                              this.physicianAlertMsg = "Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"

                          }
                            else{
                             this.myform.patchValue({
                               physician: this.selectedphyItems,
                              });
                            }
                        }
                        else{
                          this.myform.patchValue({physician: '',});
                        }
                  }


                  }
                }
              }


          // let routeExists = this.routes.find(r => r.Route_Id == 34);
          // if (routeExists != undefined) {
          //   this.selectedroItems.push(this.routes.filter(r => r.Route_Id == 34)[0]);
          // }
          if(this.sourceData ==undefined || this.sourceData==null || this.sourceData.length==0)
              {
                setTimeout(() => {
              if (this.persistanceService.get('userRole') == '\"PHYSICIAN\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              if (this.persistanceService.get('userRole') == '\"PRESCRIBER\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              this.myform.patchValue({
                physician: this.selectedphyItems,
                route: this.selectedroItems,
                indication: this.selectedindicaitem,
                indicationFreeText:'',
                source: this.selectedsourceItems,
                qtyHand: '1',
                drug: '',
               // pharmacy:this.pharmacyDefault[0].PharmacyName,
                dose: '1',
                refill: '0',
                addInst: '',
                notes: '',
                insulincomments: '',
                barcode: '',
                startDate: this.minStartDate,
                writtenDate: this.minStartDate,
                maxPerDay: '',
                alertText: '',
                endDate: '',
                waitforpharmacy: '',
                suppliedanother: '',
                daw: '0',
                type: 1,
                schduleText: '',
                self: '',
                treatment: '',
                literal: '',
                prn: '',
                controlSubstance: '',
                orderTypeId: 1,
                UOM: '',
                sig1Frequency: this.selectedsig1Frequency,
              });
            }, 3000);
            }
            else
            {
              if (this.persistanceService.get('userRole') == '\"PHYSICIAN\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              if (this.persistanceService.get('userRole') == '\"PRESCRIBER\"') {

                let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
                if (sourceExists != undefined) {

                  this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
                }
              }
              this.myform.patchValue({
                physician: this.selectedphyItems,
                route: this.selectedroItems,
                indication: this.selectedindicaitem,
                indicationFreeText:'',
                source: this.selectedsourceItems,
                qtyHand: '1',
                drug: '',
               // pharmacy:this.pharmacyDefault[0].PharmacyName,
                dose: '1',
                refill: '0',
                addInst: '',
                notes: '',
                insulincomments: '',
                barcode: '',
                startDate: this.minStartDate,
                writtenDate: this.minStartDate,
                maxPerDay: '',
                alertText: '',
                endDate: '',
                waitforpharmacy: '',
                suppliedanother: '',
                daw: '0',
                type: 1,
                schduleText: '',
                self: '',
                treatment: '',
                literal: '',
                prn: '',
                controlSubstance: '',
                orderTypeId: 1,
                UOM: '',
                sig1Frequency: this.selectedsig1Frequency,
              });
            }



          this.barcodear = [];
          if(this.userRole=='\"PHYSICIAN\"' ||this.userRole=='\"PRESCRIBER\"')
              {
                this.isReadOnlyPhy = true;
              }
              else
              {
                this.isReadOnlyPhy = false;
              }
          //this.isReadOnly = false;
          this.ordersDetails = {} as CPOEOrdersData;
          this.selectedOrder = 0;
          this.selectedOrderQuantity = 0;
          this.selectedOrderDADminId = 0;
          this.orderId = 0;
          this.quantityId = 0;
          this.stockId = 0;
          this.gpiCode = '';
          this.resetScheduleForm();
          this.sigsDoseFreqArray = [];
          this.resetAdministerScheduleForm();
          //this.mergeFlag = 0;
          this.isReadOnlyforControl = false;
          this.isControlSubstanceReadOnly = false;
          this.isDrugOrder = false;
          //this.notesFlag = 1;
          this.barCodeFlag = 1;
          //this.getOrderGridData("Test");
          if (this.barcodear.length == 0) {
            // const barcodevalidation = this.myform.get('barcode');
            // barcodevalidation.setValidators([Validators.required]);
            // barcodevalidation.updateValueAndValidity();

          }
          const drugnameValidations = this.myform.get('drug');
          drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
          drugnameValidations.updateValueAndValidity();
          // const barcodevalidation = this.myform.get('barcode');
          // barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
          // barcodevalidation.updateValueAndValidity();
          // const schduleTextValidations = this.myform.get('schduleText');
          // schduleTextValidations.clearValidators();
          // schduleTextValidations.updateValueAndValidity();
          const doseValidations = this.myform.get('dose');
          doseValidations.setValidators([Validators.required]);
          doseValidations.updateValueAndValidity();
          const sig1FrequencyValidations = this.myform.get('sig1Frequency');
          sig1FrequencyValidations.setValidators([Validators.required]);
          sig1FrequencyValidations.updateValueAndValidity();
          const maxpervalidation = this.myform.get('maxPerDay');
          maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
          Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          Validators.pattern(/^\d*(\.\d{0,3})?$/),
          Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
          maxpervalidation.updateValueAndValidity();
          const routeValidations = this.myform.get('route');
          routeValidations.setValidators(Validators.required);
          routeValidations.updateValueAndValidity();
          const refillValidations = this.myform.get('refill');
          refillValidations.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
          refillValidations.updateValueAndValidity();
          const uomValidations = this.myform.get('UOM');
          uomValidations.setValidators(Validators.required);
          uomValidations.updateValueAndValidity();
          const dawValidations = this.myform.get('daw');
          dawValidations.setValidators(Validators.required);
          dawValidations.updateValueAndValidity();
          const qtyHandValidations = this.myform.get('qtyHand');
          qtyHandValidations.setValidators([Validators.required, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
          qtyHandValidations.updateValueAndValidity();
          if (this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '') {
            const indicationValidations = this.myform.get('indication');
            indicationValidations.setValidators(Validators.required);
            indicationValidations.updateValueAndValidity();
          }
          if (this.selectedindicaitem.length == 0) {
            const indicationFreeTextValidations = this.myform.get('indicationFreeText');
            indicationFreeTextValidations.setValidators(Validators.required);
            indicationFreeTextValidations.updateValueAndValidity();
          }
          const addInstValidations = this.myform.get('addInst');
          addInstValidations.setValidators(Validators.maxLength(250));
          addInstValidations.updateValueAndValidity();
        }
      }
      else
        this.alertService.error('Select resident');
    }

  }
  resetScheduleForm() {
    this.scheduleform.reset();
    this.getDaysDropData();
    this.GetMonthMasterData();
    this.GetWeekMasterData();
    this.selectedDays = [];
    this.selectedMonths = [];
    this.selectedWeeks = [];
    this.timesArray = [];
    this.timeform.reset();
    this.isShiftSchedule = false;
    this.isHoursReadOnly = false;
    this.isdaysDisabled = false;
    this.isWeeksDisabled = false;
    this.resetMonthsDropSettings(13);
    this.resetWeekDropSettings(13);
    this.selectedfrequencyItems = [];
    this.selectedstItems = [];
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
      // oDay: 0,
      // through: 0,
      //days: 0
    });
    this.hoaObj = null;
  }
  ngAfterViewChecked(): void {
    this.changeDetectorRef.detectChanges();
  }
  funBindPrecriber(OrdersNPI:string)
  {

    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + this.nurseStationId+"/"+this.orderId)
    .subscribe(res => {
      this.physicianfullList = res;
      this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;

      if(OrdersNPI != "")
      {

      let physicianRecord1 = this.physiciansdrop.filter(p => p.PhysicianNPI == OrdersNPI)[0];
      if (physicianRecord1 != undefined)
      {
   
       this.selectedphyItems = [];
         this.selectedphyItems.push(physicianRecord1);
        if(this.selectedphyItems !=null && this.selectedphyItems.length){
           if(this.selectedphyItems[0].PStatus == 0 ){
                 this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
                 this.supervisingInactiveModal = true;
                 this.physicianAlertMsg ="Selected Physician is inactive"
           }
           else if( (this.selectedphyItems[0].CredeValue == 'NP' || this.selectedphyItems[0].CredeValue == 'PA' || this.selectedphyItems[0].CredeValue == 'Other' ) && (this.selectedphyItems[0].SPhy ==null || this.selectedphyItems[0].SPhy == '' || this.selectedphyItems[0].SPhy.length ===0)){
       
             this.supervisingInactiveModal = true;
             this.physicianAlertMsg = "“Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) has incomplete credentialing info. Please select another Prescriber"
            
   
           }
           else if(this.selectedphyItems[0].SPhy !=null && this.selectedphyItems[0].SPhy != '' && this.selectedphyItems[0].SPhyStatus==0){
         
               this.supervisingInactiveModal = true;
               this.physicianAlertMsg = "Selected Prescriber's ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) Supervising Physician( " + this.selectedphyItems[0].SphyName + " )is inactive. Please select another Prescriber" ;
   
   
           }
           else if(this.selectedphyItems.length && this.selectedphyItems[0].Credentials == 0){
           
               this.supervisingInactiveModal = true;
               this.physicianAlertMsg = "Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"
   
           }
             else{
               this.myform.patchValue({
                 physician: this.selectedphyItems,
               });
             }
         }
         else{
           this.myform.patchValue({physician: '',});
         }
      }
  }
    }, error => {
      this.alertService.error(error.message);
    });
  }
  fetchOrdersData(res) {
    debugger

    // if (this.demographicInfoData.PVisit_Status == 2) {
    //   this.activeTab = 'Inactive';
    // }
    // else {
    //   this.activeTab = 'Active';
    // }
    if (this.demographicInfoData.PVisit_Status == 2) {
      this.alertService.warn("Resident has been discharged.")
    }
    else if (this.demographicInfoData.PVisit_Status == 3) {
      this.alertService.warn("Status updated to temporarily inactive.")
    }
    const maxpervalidation = this.myform.get('maxPerDay');
    maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
    Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
    Validators.pattern(/^\d*(\.\d{0,3})?$/),
    Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
    maxpervalidation.updateValueAndValidity();
    this.reviewClickedFlag = 0;
    this.valueChangesFlag = 0;
    this.barCodeFlag = 0;
    this.userAttempts = 0;
    if (res.OrderTypeID == 4 || res.OrderTypeID == 2 ) { //added  (|| res.OrderTypeID == 2)  on 05/15/2026 to make literal treatment as literal order
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.clearValidators();
      drugnameValidations.updateValueAndValidity();


      const DoseUomValidations = this.myform.get('DoseUom');
      DoseUomValidations.clearValidators();
      DoseUomValidations.updateValueAndValidity();
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.clearValidators();
      // barcodevalidation.updateValueAndValidity();
      // const schduleTextValidations = this.myform.get('schduleText');
      // schduleTextValidations.clearValidators();
      // schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.clearValidators();
      doseValidations.updateValueAndValidity();
      const sig1FrequencyValidations = this.myform.get('sig1Frequency');
      sig1FrequencyValidations.clearValidators();
      sig1FrequencyValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.clearValidators();
      routeValidations.updateValueAndValidity();
      const refillValidations = this.myform.get('refill');
      refillValidations.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
      refillValidations.updateValueAndValidity();
      const uomValidations = this.myform.get('UOM');
      uomValidations.clearValidators();
      uomValidations.updateValueAndValidity();
      const dawValidations = this.myform.get('daw');
      dawValidations.clearValidators();
      dawValidations.updateValueAndValidity();
      const qtyHandValidations = this.myform.get('qtyHand');
      qtyHandValidations.setValidators([Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
      qtyHandValidations.updateValueAndValidity();
      this.barCodeStatusFlag = 1;
    }
    else {

      const DoseUomValidations = this.myform.get('DoseUom');
      DoseUomValidations.setValidators([Validators.required]);
      DoseUomValidations.updateValueAndValidity();


      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
      drugnameValidations.updateValueAndValidity();
      // const barcodevalidation = this.myform.get('barcode');
      // barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
      // barcodevalidation.updateValueAndValidity();
      // const schduleTextValidations = this.myform.get('schduleText');
      // schduleTextValidations.clearValidators();
      // schduleTextValidations.updateValueAndValidity();
      const doseValidations = this.myform.get('dose');
      doseValidations.setValidators([Validators.required]);
      doseValidations.updateValueAndValidity();
      const sig1FrequencyValidations = this.myform.get('sig1Frequency');
      sig1FrequencyValidations.setValidators([Validators.required]);
      sig1FrequencyValidations.updateValueAndValidity();
      const routeValidations = this.myform.get('route');
      routeValidations.setValidators(Validators.required);
      routeValidations.updateValueAndValidity();
      const refillValidations = this.myform.get('refill');
      refillValidations.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
      refillValidations.updateValueAndValidity();
      const uomValidations = this.myform.get('UOM');
      uomValidations.setValidators(Validators.required);
      uomValidations.updateValueAndValidity();
      const dawValidations = this.myform.get('daw');
      dawValidations.setValidators(Validators.required);
      dawValidations.updateValueAndValidity();
      const qtyHandValidations = this.myform.get('qtyHand');
      qtyHandValidations.setValidators([Validators.required, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
      qtyHandValidations.updateValueAndValidity();
      this.barCodeStatusFlag = 0;
    }
    if (res.Barcode != null) {
      let list: string = res.Barcode;
      this.barcodear = list.split(', ');
      // const barcodevalidation = this.myform.get('barcode');
      // //barcodevalidation.setValidators(null);
      // barcodevalidation.clearValidators();
      // barcodevalidation.updateValueAndValidity();
    }
    else {
      this.barcodear = [];
    }
    if (res.PRNFlag == 1) {
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
      maxpervalidation.updateValueAndValidity();
    }
    this.favstatus = res.Favouriteflag != 1 ? 0 : 1;
    this.reactivateStatus = (res.POrder_Status != 1) ? 1 : 0;
    this.splits = res.split != 1 ? 0 : 1;
    this.multipleOrdersplits = 0;
    //this.isReadOnly = res.ReviewFlag == 0 ? false : true;
    //this.mergeFlag = res.MergeFlag == 1 ? 1 : 0;
    //this.reviewFlag = res.ReviewFlag;
    this.discontinueFlag = res.DiscontinueFlag;
    //this.drugNameFromPharmacyOrder = res.DrugName == null ? '' : res.DrugName;
    this.selectedphyItems = [];
    this.selectedroItems = [];
    if (res.Route_Id != null && res.Route_Id != 0) {
      this.selectedroItems.push(this.routes.filter(r => r.Route_Id == res.Route_Id)[0]);
    }
    if (res.OrderingPhysicianNPI != null) {
      // let physicianRecord = this.physiciansdrop.filter(p => p.PhysicianNPI == res.OrderingPhysicianNPI)[0];
      // if (physicianRecord != undefined)
      //   this.selectedphyItems.push(physicianRecord);
      this.funBindPrecriber(res.OrderingPhysicianNPI);
    }
    else {
      //this.getAllFlagsForCompanyByNSId(this.nurseStationId,this.residentId);
      this.GetPhysicianByUserRes();
      if (res.OrderTypeID == 2 || res.OrderTypeID == 4) {
        this.GetPhysicianDropData(this.nurseStationId);
        this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
        let physicianRecord = this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0];
        if (physicianRecord != undefined)
          this.selectedphyItems.push(physicianRecord);
          if(this.selectedphyItems !=null && this.selectedphyItems.length){
            this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
           if(this.selectedphyItems[0].PStatus == 0 ){
             this.supervisingInactiveModal = true;
           this.physicianAlertMsg ="Selected Physician is inactive"
           }
           else if( (this.selectedphyItems[0].CredeValue == 'NP' || this.selectedphyItems[0].CredeValue == 'PA' || this.selectedphyItems[0].CredeValue == 'Other' ) && (this.selectedphyItems[0].SPhy ==null || this.selectedphyItems[0].SPhy == '' || this.selectedphyItems[0].SPhy.length ===0)){

           this.supervisingInactiveModal = true;
           this.physicianAlertMsg = "“Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) has incomplete credentialing info. Please select another Prescriber"
          

           }
           else if(this.selectedphyItems[0].SPhy !=null && this.selectedphyItems[0].SPhy != '' && this.selectedphyItems[0].SPhyStatus==0){
             this.supervisingInactiveModal = true;
             this.physicianAlertMsg = "Selected Prescriber's ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " ) Supervising Physician( " + this.selectedphyItems[0].SphyName + " )is inactive. Please select another Prescriber" ;

           }
           else if(this.selectedphyItems.length && this.selectedphyItems[0].Credentials == 0){
              
           this.supervisingInactiveModal = true;
           this.physicianAlertMsg = "Selected Prescriber ( " + this.selectedphyItems[0].PhysicianFullName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"

           }
             else{
               this.myform.patchValue({
                 physician: this.selectedphyItems,
               });
             }
         }
         else{
           this.myform.patchValue({physician: '',});
         }
      }

    }

    this.stockId = res.Stock_Id != null ? res.Stock_Id : 0;
    this.gpiCode = '';
    this.drugNameChanged = false;
    //this.previousDrugName = res.DrugName;
    if (this.demographicInfoData.PVisit_Status == 2) {
      //this.buttonsStatus = 1;
    }
    else {
      //this.buttonsStatus = 0;
    }
    if (res.AGiveCodeIdentifier != null) {
      this.gpiCode = res.AGiveCodeIdentifier;
    }
    this.selectedindicaitem = [];
    if (res.DiagIndication != undefined && res.DiagIndication != null && res.DiagIndication != "") {
      let selecteddiag = this.indications.filter(item => item.PDiagnosis_Id==res.DiagIndication)[0];
      if(selecteddiag!=undefined){
      this.selectedindicaitem .push(selecteddiag);
      }
    }
    this.selectedsourceItems = [];
    if (res.Source != undefined && res.Source != null && res.Source != "") {
      let selectedSource = this.sourceData.filter(item => item.Source_Id==res.Source)[0];
      if(selectedSource!=undefined){
      this.selectedsourceItems .push(selectedSource);
      }
    }
    this.selectedsig1Frequency = [];
    if (res.NursingFreq_Id != undefined && res.NursingFreq_Id != null && res.NursingFreq_Id != "") {
      let selectedFrequency = this.frequencyList.filter(item => item.Frequency_Id==res.NursingFreq_Id)[0];
      if(selectedFrequency!=undefined){
      this.selectedsig1Frequency .push(selectedFrequency);
      }
    }
    else if (res.NurseShifts_Id != undefined && res.NurseShifts_Id != null && res.NurseShifts_Id != "") {
      let selectedFrequency = this.frequencyList.filter(item => item.Frequency_Id=='s' + res.NurseShifts_Id)[0];
      if(selectedFrequency!=undefined){
      this.selectedsig1Frequency .push(selectedFrequency);
      }
    }
    this.DoseUomSSSS = [];
    if (res.DUom != undefined && res.DUom != null && res.DUom != "") {
      let selectedDom = this.DoseUomDrop.filter(item => item.Dose_Id==res.DUom)[0];
      if(selectedDom!=undefined){
      this.DoseUomSSSS .push(selectedDom);
      }
    }


    this.sigsDoseFreqArray = [];
    // this.qtyhandTouched = true;
    this.myform.patchValue({
      physician: this.selectedphyItems,
      qtyHand: res.DispenseQty,
      route: this.selectedroItems,
      drug: res.DrugName,
      //pharmacy:res.PharmacyName,
      dose: res.Quantity,
      refill: res.Refill,
      addInst: res.Directions,
      notes: res.Notes,
      insulincomments: res.InsulinComments,
      barcode: '',
      startDate: (res.StartDate == null ? '' : res.StartDate.substring(0, 10)),
      maxPerDay: res.MaxPerdays,
      alertText: res.AlertText,
      endDate: (res.EndDate == null ? '' : res.EndDate.substring(0, 10)),
      type: res.OrderStockFlag,
      schduleText: res.Schedule != "Select Time" ? res.Schedule : '',
      self: res.SelfAdministeredFlag,
      treatment: (res.TreatmentFlag == 1 || res.OrderTypeID == 2 || res.OrderTypeID == 5) ? true : false,
      literal: (res.OrderTypeID == 4 || res.OrderTypeID == 2) ? true : false,
      prn: res.PRNFlag,
      controlSubstance: res.ControlSubstanceBit == 1 ? true : false,
      orderTypeId: res.OrderTypeID,
      indication: this.selectedindicaitem,
      daw: res.Daw == null ? '' : res.Daw,
      waitforpharmacy: res.WaitforPharmacy == null ? '' : res.WaitforPharmacy,
      suppliedanother: res.Hospice == null ? '' : res.Hospice,
      writtenDate: (res.WrittenDate == null ? '' : this.dateFormatPipe.transformISODate(res.WrittenDate)),
      source: this.selectedsourceItems,
      sig1Frequency: this.selectedsig1Frequency,
      indicationFreeText:res.DiagIndicationText,
      UOM: res.UOM,
      daysSupply : res.Dayssupply == null ? 1 : res.Dayssupply

      // daysSupply : res.Dayssupply

    });
    //Based on form patch

    if ((this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '')&& (res.OrderTypeID!=4 && res.OrderTypeID != 2)) { //on 05/15/2026 to make literal treatment as literal order
      const indicationValidations = this.myform.get('indication');
      indicationValidations.setValidators(Validators.required);
      indicationValidations.updateValueAndValidity();
    }
    else {
    const indicationValidations = this.myform.get('indication');
    indicationValidations.clearValidators();
    indicationValidations.updateValueAndValidity();
    }
    if (this.selectedindicaitem.length == 0 && (res.OrderTypeID!=4 && res.OrderTypeID != 2)) { //on 05/15/2026 to make literal treatment as literal order
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.setValidators(Validators.required);
      indicationFreeTextValidations.updateValueAndValidity();
    }
    else {
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.clearValidators();
      indicationFreeTextValidations.updateValueAndValidity();
      }
    if ((this.myform.value.dose != undefined && this.myform.value.dose != null && (this.myform.value.dose == 'SS' || this.myform.value.dose == 'UD')) || (this.myform.value.orderTypeId == 4 || this.myform.value.orderTypeId == 2)) { //on 05/15/2026 to make literal treatment as literal order
      const addInstValidations = this.myform.get('addInst');
      addInstValidations.setValidators(([Validators.required,Validators.maxLength(250)]));
      addInstValidations.updateValueAndValidity();
    }
    else {
      const addInstValidations = this.myform.get('addInst');
      addInstValidations.setValidators(Validators.maxLength(250));
      addInstValidations.updateValueAndValidity();
    }
    if (res.OrderTypeID == 1) {
      this.isDrugOrder = true;
    }
    else {
      this.isDrugOrder = false;
    }
    if (this.ordersDetails.HoldStatus == 1) {
      this.getOrderHoldFromToDates(this.quantityId);
    }
    this.orderOrigin = res.OrderOrigin;
    // this.valueChangesFlag = 1;
    // this.sharedService.saveChangesOrderInfo(0);
    // if(this.ordersgridrowclick==1){
    //   this.saveChangeFlag = 0;
    // this.sharedService.saveChangesOrderInfo(this.saveChangeFlag);
    // }
    // else{
    //   this.valueChangesFlag = 1;
    // }
    this.medispanControlSubBit = res.MedispanControlSubBit;
    this.controlsubstanceBit = res.ControlSubstanceBit;
    if (res.ControlSubstanceBit == 1 && res.MedispanControlSubBit == 1) {
      this.isControlSubstanceReadOnly = true;
    }
    else
      this.isControlSubstanceReadOnly = false;
    this.loadSearchData();
    // this.qtyhandTouched = false;
  }
  getOrdersGridDatabyfilter(filter: any) {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderGridData + this.residentId + "/" + filter)
      .subscribe(res => {
        this.orderGridData = res;

      }, error => {
        this.alertService.error(error.message);
      });
  }
  emarModel() {
    this.ng4LoadingSpinnerService.show();
    this.modaleMar = true;
    let data = new Date();
    this.emarform.reset();
    this.emarform.patchValue({
      month: data.getMonth() + 1,
      year: data.getFullYear(),

    });
    this.getEMARDetails();
    this.getEmarPreviewYearDrop();
  }
  getEMARDetails() {

    this.eMARDetails = [];
    this.legend = [];
    var date = new Date();
    this.days = new Date(this.yearDrop.length == 0 ? date.getFullYear() : parseInt(this.emarform.value.year), parseInt(this.emarform.value.month), 0).getDate();

    //  this.dataservice.get<any[]>(this.config.Emar_getEMARDetails + date.getMonth() + "/" + date.getFullYear() + "/" + this.residentId)
    this.dataservice.get<any[]>(this.config.Emar_getEMARDetails + this.emarform.value.month + "/" + this.emarform.value.year + "/" + this.residentId)
      .subscribe(res => {
        this.eMARDetails = res;
        this.ng4LoadingSpinnerService.hide();
        if (this.emarform.value.hidechk != null && this.emarform.value.hidechk == true)
          this.HideInactiveOrders(this.emarform.value.hidechk);
      }, error => {
        this.alertService.error(error.message);
      });
      //Legend
      this.dataservice.get<any[]>(this.config.Emar_getEMARDetailsLegend + this.emarform.value.month + "/" + this.emarform.value.year + "/" + this.residentId)
      .subscribe(res1 => {
        this.legend = res1;
        this.ng4LoadingSpinnerService.hide();
        if (this.emarform.value.hidechk != null && this.emarform.value.hidechk == true)
          this.HideInactiveOrders(this.emarform.value.hidechk);
      }, error => {
        this.alertService.error(error.message);
      });

  }
  getEmarPreviewYearDrop() {
    this.dataservice.get<any>(this.config.Emar_Orders_GetEmarPreviewYearDrop + this.residentId)
      .subscribe(res => {
        this.yearDrop = res;
        if (this.yearDrop==null || this.yearDrop.length == 0) {
          this.emarform.patchValue({
            year: "0",

          });
        }
        else
        {
          let data = new Date();
          var Years = data.getFullYear();
          for (let item of this.yearDrop) {

            if (item == Years) {

              var b =  item;
            }
        }

          if(b != undefined)
          {
            this.emarform.patchValue({
              year: b,
            });
          }
          else
          {
            this.emarform.patchValue({
              year: data.getFullYear(),
            });
          }
        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  HideInactiveOrders(value: any) {

    if (value == true) {
      this.eMARDetails = this.eMARDetails.filter(order => order.POrder_Status === 1);
    }
    else if (value == false) {
      this.getEMARDetails();
    }
  }
  changeResident() {

     this.myform.enable();
     this.DisableFag = false ;
    let residents = this.myform.value.ddlresidents;
    if (residents.length != 0) {
      this.ng4LoadingSpinnerService.show();
      this.residentId = this.myform.value.ddlresidents[0].Patient_Id;
      this.sharedService.changePatientId(this.residentId);
      //this.orderId = 0;
      //this.pendingReviewCount = 0;
      this.clearData(1);
      this.GetPhysicianByUserRes();
      //this.getAllFlagsForCompanyByNSId(this.selectedNursestation[0].NurseStation_Id,this.residentId)
      //this.getNurseStationByPId();
      this.getDemographicInfoData();
      //this.getNurseCommentNotesByQuantityId();
    }
    else {

      this.clearData(1);
      this.residentId = 0;
      this.sharedService.changePatientId(this.residentId);
      //this.getNurseCommentNotesByQuantityId();
      this.orderId = 0;
      //this.pendingReviewCount = 0;
      this.patientIdstatus = 0;
      let nursestations = this.myform.value.nursestationName
      if (nursestations.length == 0) {

        this.getDemographicInfoByNurseStation(0);
      }
      else {

        let nurseStationId = this.selectedNursestation[0].NurseStation_Id;
        this.getDemographicInfoByNurseStation(nurseStationId);
      }
    }
  }
  modalfav() {
    if (this.newOrderFlag != 1) {
      this.getFavouritesMasterData();
    }
    this.modalfavIsOpen = true;
  }
  closeFavModal() {
    if (this.newOrderFlag == 1 && this.favstatus != 1) {
      this.favForm.reset();
      this.favobj = [];
      this.favouriteObj = [];
    }
    if (this.newOrderFlag == 1 && this.favobj.length == 0) {
      this.favstatus = 0;
    }
    this.closeModel();
  }
  getFavouritesMasterData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetFavouritesMasterData + this.quantityId + '/' + this.barcodeFacilityId)
      .subscribe(res => {
        if (this.newOrderFlag != 1 || this.quantityId == 0) {
          this.favouriteMasterList = res;
          this.favouriteObj = [];
          this.favobj = [];
          let favouritestatus = this.favouriteMasterList.filter(r => r.OrderFavourite_ID != 0);
          if (favouritestatus.length >= 1) {
            this.favouriteObj = this.favouriteMasterList.filter(r => r.OrderFavourite_ID != 0);
            this.favouriteObj.forEach(element => {
              this.favobj.push(element.OrderFavMaster_ID);
            });
            this.favstatus = 1;
          }
          else if (favouritestatus.length == 0) {
            this.favstatus = 0;
          }
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  FavouriteChange(FavMasterID: number, event, FavID: number) {
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
        FavListCheckFlag: 0,
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
        FavListCheckFlag: 0,
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
        if (this.newOrderFlag == 1) {
          //this.favobj;
          if (this.favouriteObj.length >= 1) {
            this.favstatus = 1;
          }
          else if (this.favouriteObj.length == 0) {
            this.favstatus = 0;
          }
          this.modalfavIsOpen = false;
          this.ng4LoadingSpinnerService.hide();
        }
        else if (this.newOrderFlag != 1) {
          if (this.favouriteObj.length == 0) {
            let objFav = new OrderFavourite();
            objFav = {
              OrderFavourite_ID: 0,
              PQuantity_Id: this.quantityId,
              OrderFavMaster_ID: 0,
              OrderFavourite_Status: 1,
              OrderFavourite_Createby: this.userID,
              OrderFavourite_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
              FavListCheckFlag: 1,
            }
            this.favobj.push(objFav.OrderFavMaster_ID);
            this.favouriteObj.push(objFav);
          }
          this.dataservice.post(this.config.Emar_Orders_InsertOrderFavourities, this.favouriteObj)
            .subscribe(res => {
              this.getFavouritesMasterData();
              this.ng4LoadingSpinnerService.hide();
              this.alertService.success("Measurements and other checks saved successfully");
              this.modalfavIsOpen = false;
              if (this.newOrderFlag != 1)
                this.favouriteObj = [];
              //this.favstatus = 1;
            }, error => {
              this.modalfavIsOpen = false;
              this.ng4LoadingSpinnerService.hide();
              this.alertService.error(error.message);
            });
        }
        else {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.warn("No records selected");
        }
      }
    }
    else {
      this.alertService.error("No Resident selected");
    }
  }
  onFrequencyChange(item: any) {

    if (this.selectedfrequencyItems.length != 0) {
      let frequencyId = this.scheduleform.value.frequency[0].Frequency_Id;
      if (frequencyId.startsWith('s') || frequencyId == 28 || this.frequencyList.find(f => f.Frequency_Id == frequencyId).Frequency_PRN == 1) {
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
        this.isWeeksDisabled = false;
        this.isdaysDisabled = false;
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
      this.scheduleform.reset();
      this.timeform.reset();
      this.getDaysDropData();
      this.selectedDays = [];
      this.selectedMonths = [];
      this.selectedWeeks = [];
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
      if (this.newOrderFlag == 1) {
        this.myform.patchValue({
          prn: '',
          schduleText: '',
        });
      }
    }
  }
  fetchFrequencyMapData(frequencyId: any, res: any) {

    if (res != null) {
      this.getDaysDropData();
      this.GetMonthMasterData();
      this.GetWeekMasterData();
      this.timesArray = [];
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
      if (res.MonthId != undefined && res.MonthId != null && res.MonthId != "") {
        this.selectedMonths = res.MonthId.toString().split(',').map(Number);
        selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
      }
      let selectWeeks = [];
      if (res.WeekId != undefined && res.WeekId != null && res.WeekId != "") {
        this.selectedWeeks = res.WeekId.toString().split(',').map(Number);
        selectWeeks = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
        this.selectedDays = [];
        this.isdaysDisabled = true;
      }
      this.scheduleform.patchValue({
        frequency: this.selectedfrequencyItems,
        either: this.selectedstItems,
        hours: (res.Hours == null || res.Hours == 0) ? '' : res.Hours,
        mon: res.Monday,
        tue: res.Tuesday,
        wed: res.Wednesday,
        thu: res.Thursday,
        fri: res.Friday,
        sat: res.Saturday,
        sun: res.Sunday,
        week: selectWeeks,//res.WeekId == null ? 0 : res.WeekId,
        month: selectMonths,//res.MonthId == null ? 0 : res.MonthId,
        // week: res.Week_Id,
        // month: res.Month_Id,
        // oDay: res.OnlyOnDay,
        // through: res.ThroughDay,
        aDay: res.ActiveDays,
        hDay: res.HoldDays,
        //days: activeDays
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
      //this.scheduleform.reset();
      this.timeform.reset();
      this.getDaysDropData();
      this.GetMonthMasterData();
      this.GetWeekMasterData();
      this.selectedDays = [];
      this.selectedWeeks = [];
      this.selectedMonths = [];
      this.timesArray = [];
      this.selectedstItems = [];
      //this.selectedfrequencyItems=[];
      this.scheduleform.patchValue({
        //frequency: this.selectedfrequencyItems,
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
        // oDay: 0,
        // through: 0,
        //days: 0
      });
    }
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
    if (this.newOrderFlag != 1) {
      this.resetScheduleForm();
      this.dataservice.get<DrugAdministrationTime>(this.config.Emar_Orders_GetScheduleTimeDetails + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId)
        .subscribe(res => {
          this.scheduleTextObj = res;
          if (res != null)
            this.fetchScheduleData(res);
          else {
            this.selectedWeeks = [];
            this.selectedMonths = [];
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
    else {
      if (this.hoaObj == null) {
        this.selectedWeeks = [];
        this.selectedMonths = [];
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
      }
      this.ng4LoadingSpinnerService.hide();
    }
  }
  fetchScheduleData(res: DrugAdministrationTime) {

    //Days are not loading so loading again
    this.getDaysDropData();
    this.GetMonthMasterData();
    this.GetWeekMasterData();
    this.dAdminId = res.DAdminId;
    this.timesArray = [];
    this.isdaysDisabled = false;
    this.isWeeksDisabled = false;
    this.timesArray = res.HoursList == null ? [] : res.HoursList;
    if (this.timesArray.length > 1)
      this.isHoursReadOnly = true;
    else
      this.isHoursReadOnly = false;
    let activeDays = [];
    let selectWeek = [];
    if (res.WeekId != undefined && res.WeekId != null && res.WeekId != "") {
      this.selectedWeeks = res.WeekId.toString().split(',').map(Number);
      selectWeek = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
      this.isdaysDisabled = true;
      if (selectWeek.filter(ite => ite.Week_Id == 1).length != 0) {
        this.onWeekSelect(selectWeek.filter(ite => ite.Week_Id == 1)[0]);
      }
      else {
        this.resetWeekDropSettings(13);
      }
    }
    else {
      this.resetWeekDropSettings(13);
    }
    if ((res.WeekId == undefined || res.WeekId == null || res.WeekId == "") && res.Days != "") {
      this.selectedDays = res.Days.toString().split(',').map(Number);
      activeDays = this.monthDays.filter(item => this.selectedDays.includes(item.item_id));
      this.isWeeksDisabled = true;
    }
    let selectMonths = [];
    if (res.MonthId != undefined && res.MonthId != null && res.MonthId != "") {
      this.selectedMonths = res.MonthId.toString().split(',').map(Number);
      selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
      if (selectMonths.filter(ite => ite.Month_Id == 13).length != 0) {
        this.onMonthSelect(selectMonths.filter(ite => ite.Month_Id == 13)[0]);
      }
      else {
        this.resetMonthsDropSettings(13);
      }
    }
    else {
      this.resetMonthsDropSettings(13);
    }
    this.selectedstItems = [];
    if (res.HoursList != null) {
      this.selectedstItems.push(this.hoursList.filter(h => h.Hour_Id == res.HoursList[0].Hour_Id)[0]);
    }
    this.selectedfrequencyItems = [];
    if (res.NursingFreqId != null) {
      this.selectedfrequencyItems.push(this.frequencyList.filter(f => f.Frequency_Id == res.NursingFreqId.toString())[0]);
      if (res.NursingFreqId == 28) {
        this.selectedstItems = null;
        this.isShiftSchedule = true;
        this.isHoursReadOnly = true;
        this.timesArray = [];
      }
      if (this.frequencyList.find(f => f.Frequency_Id == res.NursingFreqId.toString()) != undefined && this.frequencyList.find(f => f.Frequency_Id == res.NursingFreqId.toString()).Frequency_PRN == 1) {
        this.myform.patchValue({
          prn: true
        });
        // const maxpervalidation = this.myform.get('maxPerDay');
        // maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
        // Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        // Validators.pattern(/^\d*(\.\d{0,3})?$/),
        // Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        // maxpervalidation.updateValueAndValidity();
        const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.required]);
        maxpervalidation.updateValueAndValidity();
        this.isInteger();
        this.selectedstItems = [];
        this.isShiftSchedule = true;
        this.isHoursReadOnly = true;
        this.timesArray = [];
      }
      else {
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
      let checkExist = this.frequencyList.find(f => f.Frequency_Id == 's' + res.NurseShiftsId);
      if (checkExist != undefined) {
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
  insertHoa() {
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
            this.alertService.error("At once hours and multiple time can't insert.");
          }
          else {
            this.insertScheduleTimes();
          }
        }
        //if frequency is selected and it is not PRN and not shifts
        else if (this.frequencyList.find(f => f.Frequency_Id == this.scheduleform.value.frequency[0].Frequency_Id).Frequency_PRN != 1 && this.scheduleform.value.frequency[0].Frequency_Id.startsWith('s') == false && this.scheduleform.value.frequency[0].Frequency_Name.startsWith('QShift') == false) {
          if ((this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0) && ((this.scheduleform.value.hours == null || this.scheduleform.value.hours == "") || (this.timesArray.length == 0 || this.timesArray == null))) {
            this.alertService.warn("Please select start time.");
          }
          else if (this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null && this.timesArray.length > 1) {
            this.alertService.error("At once hours and multiple time can't insert.");
          }
          else {
            this.insertScheduleTimes();
          }
        }
        //if frequency is selected and it is PRN
        // else if (this.scheduleform.value.frequency[0].Frequency_Id == 1) {
        //   if ((this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0) && ((this.scheduleform.value.hours == null || this.scheduleform.value.hours == ""))) {
        //     this.alertService.warn("Please Select Start Time or Hours.");
        //   }
        //   else {
        //     this.insertScheduleTimes();
        //   }
        // }
        // if ((this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0) && ((this.scheduleform.value.hours == null || this.scheduleform.value.hours == "") || (this.timesArray.length == 0 || this.timesArray == null))) {
        //   this.alertService.warn("Please enter schedule times");
        // }
        // else if (this.scheduleform.value.frequency == null || this.scheduleform.value.frequency.length == 0 || this.scheduleform.value.frequency == undefined) {
        //   this.alertService.warn("Please enter frequency");
        //   this.ng4LoadingSpinnerService.hide();
        // }
        // else if (this.timesArray.length == 0 && (this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0)){
        // // && (this.scheduleform.value.frequency != undefined || this.scheduleform.value.frequency != null || this.scheduleform.value.frequency.length != 0)) {
        //   if (this.scheduleform.value.frequency[0].Frequency_Id != 1) {
        //     this.alertService.warn("Please Select Start Time.");
        //   }
        // }
        else if (this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null && this.timesArray.length > 1) {
          this.alertService.error("At once hours and multiple time can't insert.");
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
  insertScheduleTimes(controlSubstanceFlag?: number) {

    if (this.timesArray.length != 0) {
      this.timesArray.forEach(element => {
        this.times += element.Hour_Id + ",";
      });
      this.times = this.times.substring(0, this.times.length - 1);
    }
    else if (this.timesArray.length == 0) {
      if ((this.timesArray.length == 0 || this.timesArray == null) && (this.scheduleform.value.either == '' || this.scheduleform.value.either == null || this.scheduleform.value.either == undefined) && ((this.scheduleform.value.frequency != undefined || this.scheduleform.value.frequency != null || this.scheduleform.value.frequency.length != 0))) {
        if (this.frequencyList.find(f => f.Frequency_Id == this.scheduleform.value.frequency[0].Frequency_Id).Frequency_PRN == 1)
          this.times = '';
      }
      else {
        this.times = this.scheduleform.value.either[0].Hour_Id;
      }
    }
    let nurseStationId = this.selectedNursestation[0].NurseStation_Id;
    let freqId = this.scheduleform.value.frequency == undefined || this.scheduleform.value.frequency.length == 0 || this.scheduleform.value.frequency == null ? null : this.scheduleform.value.frequency[0].Frequency_Id;
    if (this.frequencyList.find(f => f.Frequency_Id == freqId) != undefined && this.frequencyList.find(f => f.Frequency_Id == freqId).Frequency_PRN == 1) {
      this.myform.patchValue({
        prn: true
      });
      // const maxpervalidation = this.myform.get('maxPerDay');
      // maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      // Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      // Validators.pattern(/^\d*(\.\d{0,3})?$/),
      // Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
      // maxpervalidation.updateValueAndValidity();
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.required]);
      maxpervalidation.updateValueAndValidity();
      this.isInteger();
    }
    else {
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
    this.hoaObj = {
      dadminId: this.dAdminId,
      porderId: this.orderId,
      pquantityId: this.quantityId,
      freqId: freqId != null ? (freqId.startsWith('s') ? null : parseInt(freqId)) : null,
      hourId: this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0 ? null : this.scheduleform.value.either[0].Hour_Id,
      hourIds: this.times,
      //timeformatId: this.scheduleform.value.timeFormat == 0 ? null : this.scheduleform.value.timeFormat,
      hours: this.scheduleform.value.hours == '' || this.scheduleform.value.hours == 0 ? null : this.scheduleform.value.hours,
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
      days: this.selectedDays != undefined ? (this.selectedDays.length > 0 ? this.selectedDays.join() : null) : null,
      createdby: this.userID,
      activedays: this.scheduleform.value.aDay,
      holddays: this.scheduleform.value.hDay,
      NurseStationId: nurseStationId,
      nurseShiftId: freqId != null ? (freqId.startsWith('s') ? freqId.substring(1) : null) : null
    };
    if (this.newOrderFlag != 1) {
      this.dataservice.post(this.config.Emar_Orders_InsertupdateHOA, this.hoaObj)
        .subscribe(res => {
          if (res != null) {
            this.modalHOAIsOpen = false;
            this.hoaObj = null;
            this.alertService.success("Schedule times updated successfully");
            this.myform.patchValue({
              schduleText: res,
            });
            this.btnHoaSaveFlag = 1;
            if (this.myform.value.prn == true && this.newOrderFlag != 1 && this.scheduleSave == 2) {
              this.scheduleSave = 1;
              this.saveExistingOrder(controlSubstanceFlag);
            }
            //this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);
          }
        }, error => {
          this.modalHOAIsOpen = false
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else {
      this.dataservice.post(this.config.Emar_Orders_GetScheduledTimeText, this.hoaObj)
        .subscribe(res => {
          if (res != null) {
            let text = res;
            this.myform.patchValue({
              schduleText: text
            });
            this.modalHOAIsOpen = false
          }
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
            this.modalHOAIsOpen = false
          });
      // this.myform.patchValue({
      //   schduleText: this.scheduleform.value.either.length == 0 ? '' : this.scheduleform.value.either[0].Hour_Desc
      // });
      this.modalHOAIsOpen = false;
    }
    this.ng4LoadingSpinnerService.hide();
  }
  onNurseStationSelect(item: any) {
this.myform.enable();
this.DisableFag = false;
    this.patientIdstatus = 0;
    this.changeNurseStation(item);
    //this.pendingReviewCount = 0;
  }
  onNurseStationDeSelect(item: any) {
    this.myform.enable();
this.DisableFag = false;
    this.clearData();
    this.alertService.warn("Select nursing station to view resident list");
    //this.changeNurseStation(item);
    //this.ng4LoadingSpinnerService.hide();
  }
  changeNurseStation(item: any) {
    //this.orderId = 0;
    this.clearData();
    if (item.NurseStation_Id == 0) {
      this.residents = [];
      this.selectedResItem = [];
      this.alertService.warn("Select nursing station to view resident list");
    }
    else {
      this.ng4LoadingSpinnerService.show();
      this.getDemographicInfoByNurseStation(item.NurseStation_Id);
      //this.getAllFlagsForCompanyByNSId(item.NurseStation_Id);
      this.GetPhysicianDropData(item.NurseStation_Id);
      this.ng4LoadingSpinnerService.hide();
    }
  }
  getDemographicInfoByNurseStation(stationId: number) {
    this.nurseStationId = stationId;
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetResidentsByNurseStationId + stationId)
      .subscribe(res => {
        if(res.length == 0)
      {


      this.route.navigate(['/home/ordergridcpoe']);
      return false;


      }
        this.residents = res;
        if (res.length > 0 && this.patientIdstatus != 1) {
          this.residentId = this.residents.filter(r => r.PVisit_Status == 1)[0].Patient_Id;
          //this.getAllFlagsForCompanyByNSId(stationId,this.residentId);
          this.GetPhysicianByUserRes();
          this.sharedService.changePatientId(this.residentId);
          this.selectedResItem = this.residents.filter(r => r.Patient_Id === this.residentId);
          this.getDemographicInfoData();
          if (this.orderId == 0 && this.quantityId == 0 && this.newResOrderFlag == true) {
            this.orderGridData = [];
            this.getFavouritesMasterData();
            this.newOrder();
            this.getOrderGridData("Active", 1);
          }
          else {
            this.getOrderGridData("Active", 1);
          }
          this.myform.patchValue({
            ddlresidents: this.selectedResItem,
          });
        }
        else {
          //this.getAllFlagsForCompanyByNSId(stationId);
          this.clearData();
        }
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
    this.ng4LoadingSpinnerService.hide();
  }
  clearData(clearType?: any) {
    if (clearType == undefined) {
      this.residents = [];
      this.selectedResItem = [];
      this.residentId = 0;
    }
    const drugnameValidations = this.myform.get('drug');
    drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
    drugnameValidations.updateValueAndValidity();
    // const barcodevalidation = this.myform.get('barcode');
    // barcodevalidation.setValidators([Validators.required, Validators.maxLength(20)]);
    // barcodevalidation.updateValueAndValidity();
    // const schduleTextValidations = this.myform.get('schduleText');
    // schduleTextValidations.clearValidators();
    // schduleTextValidations.updateValueAndValidity();
    const doseValidations = this.myform.get('dose');
    doseValidations.setValidators([Validators.required]);
    doseValidations.updateValueAndValidity();
    const sig1FrequencyValidations = this.myform.get('sig1Frequency');
    sig1FrequencyValidations.setValidators([Validators.required]);
    sig1FrequencyValidations.updateValueAndValidity();
    const routeValidations = this.myform.get('route');
    routeValidations.setValidators(Validators.required);
    routeValidations.updateValueAndValidity();
    const refillValidations = this.myform.get('refill');
    refillValidations.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric1)]);
    refillValidations.updateValueAndValidity();
    const uomValidations = this.myform.get('UOM');
    uomValidations.setValidators(Validators.required);
    uomValidations.updateValueAndValidity();
    const dawValidations = this.myform.get('daw');
    dawValidations.setValidators(Validators.required);
    dawValidations.updateValueAndValidity();
    const qtyHandValidations = this.myform.get('qtyHand');
    qtyHandValidations.setValidators([Validators.required, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowThreeDigits)]);
    qtyHandValidations.updateValueAndValidity();
    this.barCodeStatusFlag = 0;
    this.orderId = 0;
    this.quantityId = 0;
    this.ordersInfo = {} as any;
    this.orderGridData = [];
    this.demographicInfoData = {} as DemographicInfo;
    this.newResOrderFlag = false;
    this.patientTypeList = [];
    //this.pendingReviewCount = 0;
    this.residentAllergies = '';
    this.residentDiagnosis = '';
    //this.mergeFlag = 0;
    //this.reviewFlag = 0;
    this.demographicInfoData.HomeFlag = 0;
    //this.destroyform.reset();
    this.newOrderFlag = 1;
    this.reviewClickedFlag = 0;
    this.favstatus = 0;
    this.splits = 0;
    this.reactivateStatus = 0;
    this.favForm.reset();
    this.selectedphyItems = [];
    this.selectedroItems = [];
    this.selectedindicaitem = [];
    this.userAttempts = 0;
    this.selectedsourceItems = [];
    if (this.NPIbyFFacilityIdNursId != null && this.NPIbyFFacilityIdNursId != "") {
      this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
      let checkExist9 = this.physiciansdrop.find(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId);
      if (checkExist9 != undefined) {
        this.selectedphyItems = [];
        this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.NPIbyFFacilityIdNursId)[0]);
        this.myform.patchValue({
          physician: this.selectedphyItems,
        });
      }

      }
      else{
        if (this.defaultPhysicianNPI != null) {

          let checkExist = this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI);
          if (checkExist != undefined) {
            this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
          }
          this.myform.patchValue({
            physician: this.selectedphyItems,
          });


      }
    }
    // let routeExists = this.routes.find(r => r.Route_Id == 34);
    // if (routeExists != undefined) {
    //   this.selectedroItems.push(this.routes.filter(r => r.Route_Id == 34)[0]);
    // }
    if (this.persistanceService.get('userRole') == '\"PHYSICIAN\"') {

      let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
      if (sourceExists != undefined) {
        this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
      }
    }
    if (this.persistanceService.get('userRole') == '\"PRESCRIBER\"') {

      let sourceExists = this.sourceData.find(r => r.Source_Id == 5);
      if (sourceExists != undefined) {

        this.selectedsourceItems.push(this.sourceData.filter(r => r.Source_Id == 5)[0]);
      }
    }
    this.selectedsig1Frequency = [];
    this.myform.patchValue({
      physician: this.selectedphyItems,
      route: this.selectedroItems,
      indication: this.selectedindicaitem,
      indicationFreeText:'',
      source: this.selectedsourceItems,
      qtyHand: '1',
      drug: '',
     // pharmacy:this.pharmacyDefault[0].PharmacyName,
      dose: '1',
      refill: '0',
      addInst: '',
      notes: '',
      insulincomments: '',
      barcode: '',
      startDate: this.minStartDate,
      writtenDate: this.minStartDate,
      maxPerDay: '',
      alertText: '',
      endDate: '',
      waitforpharmacy: '',
      suppliedanother: '',
      type: 1,
      schduleText: '',
      self: '',
      treatment: '',
      literal: '',
      prn: '',
      controlSubstance: '',
      orderTypeId: 1,
      UOM: '',
      sig1Frequency: this.selectedsig1Frequency,
    });
    this.barcodear = [];
    this.isReadOnly = false;
    this.ordersDetails = {} as CPOEOrdersData;
    this.selectedOrder = 0;
    this.selectedOrderQuantity = 0;
    this.selectedOrderDADminId = 0;
    this.stockId = 0;
    this.gpiCode = '';
    this.sigsDoseFreqArray = [];
    this.resetScheduleForm();
    this.resetAdministerScheduleForm();
    // Based on Form Patch
    if (this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '') {
      const indicationValidations = this.myform.get('indication');
      indicationValidations.setValidators(Validators.required);
      indicationValidations.updateValueAndValidity();
    }
    if (this.selectedindicaitem.length == 0) {
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.setValidators(Validators.required);
      indicationFreeTextValidations.updateValueAndValidity();
    }
    const addInstValidations = this.myform.get('addInst');
    addInstValidations.setValidators(Validators.maxLength(250));
    addInstValidations.updateValueAndValidity();
  }
  getAllFlagsForCompanyByNSId(stationId: number, residentId?: any) {
    let resId = residentId == undefined ? 0 : residentId;
    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId + "/" + resId)
      .subscribe(res => {
        this.drFirstFlag = res.DrFirstRequired;
        //this.defaultPhysicianNPI = res.PhysicianNPI;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  searchDrugNames(searchText: string) {
    if (this.isReadOnly == true) {
      this.alertService.warn('Drug name search is not available for review completed orders');
    }
    // else if (this.myform.value.type == 1) {
    //   this.alertService.warn('Drug Name search is available for Stock only');
    // }
    else {

      if ((searchText != null && searchText.length > 2 && this.myform.value.type == true) || (this.myform.value.type == false)) {
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
  onselectDrugFromDrugDB(item: any) {
    if (item != '') {
      //this.barcodear = [];
      this.stockId = item.Drug_Id;
      this.gpiCode = item.GPICode;
      this.drugNameChanged = false;
      // this.selectedroItems = [];
      // if (item.Route_Id != null) {
      //   this.selectedroItems.push(this.routes.filter(r => r.Route_Id == item.Route_Id)[0]);
      // }
      //this.barcodear.push(item.Barcode)
      this.selectedDrugName = item.DrugName;
      this.myform.patchValue({
        drug: item.DrugName,
        // qtyHand: '',
        //barcode:item.Barcode
        //route: this.selectedroItems,
        controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
        type: 1
      });
      this.isReadOnlyforControl = false;
      if (item.ControlledSubstanceSchedule == 1) {
        this.isControlSubstanceReadOnly = true;
        const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
        Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        Validators.pattern(/^\d*(\.\d{0,3})?$/),
        Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        maxpervalidation.updateValueAndValidity();
      }
      else {
        this.isControlSubstanceReadOnly = false;
        if (this.myform.value.prn != true) {
          const maxpervalidation = this.myform.get('maxPerDay');
          maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
          Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          Validators.pattern(/^\d*(\.\d{0,3})?$/),
          Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
          maxpervalidation.updateValueAndValidity();
        }
      }
      if (item != '' && (item.Route !== undefined)) {
        const route_name = item.Route ; 
        const routePrefix = route_name !=null ? route_name.substring(0, 2) : 'OTH';
        this.selectedroItems = this.routes.filter(item => item.Route.startsWith(routePrefix));
        }
      //this.flag = false;
    }
    else {
      return false;
    }
    this.flag = false;
  }
  onOrderTypeChange() {
    this.myform.patchValue({
      drug: '',
      //qtyHand: '',
      barcode: ''
      //route: this.selectedroItems,
    });
    this.barcodear = [];
    this.gpiCode = '';
    this.drugNameChanged = true;
    this.stockId = 0;
    this.isReadOnlyforControl = false;
    this.loadSearchData();
  }
  addTimes() {
    if (this.timeform.value.starttime == null || this.timeform.value.starttime == undefined || this.timeform.value.starttime == '') {
      this.alertService.warn("Please Select Next Time");
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
  modalTimeIsClose() {
    this.modalTimeIsOpen = false;
    this.timeform.reset();
    if (this.timesArray.length == 0) {
      this.scheduleform.controls['either'].reset();
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
  getPatientType() {
    this.dataservice.get<any[]>(this.config.Resident_Demographic_GetPatientType + this.residentId)
      .subscribe(res => {
        this.patientTypeList = res;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  resetScheduleTimes() {
    this.hoaObj = {
      dadminId: this.dAdminId,
      porderId: this.orderId,
      pquantityId: this.quantityId,
      freqId: null,
      hourId: null,
      hourIds: null,
      //timeformatId: this.scheduleform.value.timeFormat == 0 ? null : this.scheduleform.value.timeFormat,
      hours: null,
      monday: null,
      tuesday: null,
      wednesday: null,
      thursday: null,
      friday: null,
      saturday: null,
      sunday: null,
      weekId: null,
      monthId: null,
      days: null,
      createdby: this.userID,
      activedays: null,
      holddays: null,
      NurseStationId: this.nurseStationId,
      nurseShiftId: null
    };
    if (this.newOrderFlag != 1) {
      this.dataservice.post(this.config.Emar_Orders_InsertupdateHOA, this.hoaObj)
        .subscribe(res => {

          if (res != null) {
            this.modalHOAIsOpen = false;
            this.hoaObj = null;
            this.alertService.success("Schedule times updated successfully");
            this.myform.patchValue({
              schduleText: res,
              prn: false
            });
          }
          this.scheduleTextObj = null;
        }, error => {
          this.modalHOAIsOpen = false
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else {
      this.dataservice.post(this.config.Emar_Orders_GetScheduledTimeText, this.hoaObj)
        .subscribe(res => {

          if (res != null) {
            let text = res;
            this.myform.patchValue({
              schduleText: text,
              prn: false
            });
            this.modalHOAIsOpen = false
          }
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
            this.modalHOAIsOpen = false
          });
      this.modalHOAIsOpen = false;
    }
    this.resetScheduleForm();
  }
  closePRNModel() {
    this.prnSchedleCheckModal = false;
  }
  getOrderHoldFromToDates(qtyId: number) {
    this.dataservice.get<any>(this.config.Emar_Orders_GetOrderHoldData + qtyId)
      .subscribe(res => {
        this.orderHoldData = res;
        this.orderReleaseDate = new Date((new Date(res.HoldTo)).getTime() + (60 * 60 * 24 * 1000));
        if (res.HoldReason.length > 30)
          this.holdReason = res.HoldReason.substr(0, 30);
        else
          this.holdReason = res.HoldReason;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  // Mutiple orders hold, release hold, dc, reactivate region
  GetResidentAllOrdersData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderGridData + this.residentId + "/" + "AllActive")
      .subscribe(res => {
        this.updateOrdersRecords = [];
        this.holdObj = null;
        this.dcObj = null;
        this.noDCSplitsFlag = 0;
        this.residentAllOrders = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  // onCheckHold(event: any, item: any) {

  //   let holdFlag = event == true ? 1 : 0;
  //   var record = this.residentAllOrders.find(re => re.porder_Id == item.porder_Id && re.PQuantity_Id == item.PQuantity_Id);
  //   if (record.HoldStatus == holdFlag) {
  //     let holdDate = this.dateFormatPipe.transformISODate(record.HoldStatus == 0 ? '' : record.On_Hold_Until);
  //     let holdDateId = "#holddate" + item.PQuantity_Id;
  //     $(holdDateId).val(holdDate);
  //     let holdChk = "#holdchk" + item.PQuantity_Id;
  //     $(holdChk).prop("checked", record.HoldStatus == 0 ? false : true);

  //     var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
  //     if (checkHold == undefined) {
  //       let obj =
  //       {
  //         PorderId: item.porder_Id,
  //         PQuantityId: item.PQuantity_Id,
  //         PatientId: this.residentId,
  //         HoldChangeFlag: 0,
  //         HoldStatus: 0,
  //         DcChangeFlag: 0,
  //         DcStatus: 0,
  //         DcsPlit: 0,
  //         UpdatedBy: this.userID,
  //         UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
  //       }
  //       this.updateOrdersRecords.push(obj);
  //       var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //       if (changesRecords.length > 0) {
  //         this.valueChangesFlagReceive = 1;
  //       }
  //     }
  //     else {
  //       checkHold.HoldChangeFlag = 0,
  //         checkHold.HoldStatus = 0,
  //         this.updateOrdersRecords.push();
  //       var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //       if (changesRecords.length > 0) {
  //         this.valueChangesFlagReceive = 1;
  //       }
  //     }
  //   }
  //   else if (record.HoldStatus != holdFlag) {

  //     if (event == true) {
  //       // this.isHoldChecked=true;
  //       // this.checkedPorderId=item.porder_Id;
  //       // this.checkedPquantityId=item.PQuantity_Id;
  //       // this.holdReleaseDate="";
  //       // this.modalholdIsOpen=true;
  //       var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
  //       if (checkHold == undefined) {
  //         let obj =
  //         {
  //           PorderId: item.porder_Id,
  //           PQuantityId: item.PQuantity_Id,
  //           PatientId: this.residentId,
  //           HoldChangeFlag: 1,
  //           HoldStatus: 1,
  //           DcChangeFlag: 0,
  //           DcStatus: 0,
  //           DcsPlit: 0,
  //           UpdatedBy: this.userID,
  //           UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
  //         }
  //         this.updateOrdersRecords.push(obj);
  //         this.orderholdform.reset();
  //         this.orderholdform.patchValue({
  //           orderHoldFormDate: '',
  //           orderHoldToDate: '',
  //           orderHoldReason: '',
  //         });
  //         var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //         if (changesRecords.length > 0) {
  //           this.valueChangesFlagReceive = 1;
  //         }
  //       }
  //       else {
  //         checkHold.HoldChangeFlag = 1,
  //           checkHold.HoldStatus = 1,
  //           this.updateOrdersRecords.push();

  //         var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //         if (changesRecords.length > 0) {
  //           this.valueChangesFlagReceive = 1;
  //         }
  //       }
  //     }
  //     else {
  //       let holdDate = this.dateFormatPipe.transformISODate('');
  //       let holdDateId = "#holddate" + item.PQuantity_Id;
  //       $(holdDateId).val(holdDate);
  //       var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
  //       if (checkHold == undefined) {
  //         let obj =
  //         {
  //           PorderId: item.porder_Id,
  //           PQuantityId: item.PQuantity_Id,
  //           PatientId: this.residentId,
  //           HoldChangeFlag: 1,
  //           HoldStatus: 2,
  //           DcChangeFlag: 0,
  //           DcStatus: 0,
  //           DcsPlit: 0,
  //           UpdatedBy: this.userID,
  //           UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
  //         }
  //         this.updateOrdersRecords.push(obj);
  //         var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //         if (changesRecords.length > 0) {
  //           this.valueChangesFlagReceive = 1;
  //         }
  //       }
  //       else {
  //         checkHold.HoldChangeFlag = 1,
  //           checkHold.HoldStatus = 1,
  //           this.updateOrdersRecords.push();
  //         var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //         if (changesRecords.length > 0) {
  //           this.valueChangesFlagReceive = 1;
  //         }
  //       }
  //     }
  //   }

  // }
  onCheckDC(event: any, item: any) {
    let isChecked = event == true
    this.profileDCChangesCheckState[item.PQuantity_Id] = isChecked;
    this.rightGridDCChangesFlag = Object.values(this.profileDCChangesCheckState).some((state) => state);
    let dcFlag = event == true ? 1 : 0;
    var record = this.residentAllOrders.find(re => re.porder_Id == item.porder_Id && re.PQuantity_Id == item.PQuantity_Id);
    let isRecordDc = record.POrder_Status != 1 ? 1 : 0;
    let holdChk = "#holdchk" + item.PQuantity_Id;
    let holdFlag = $(holdChk).prop("checked") == true ? 1 : 0;
    if (event == false && $(holdChk).prop("checked") == false) {
      $(holdChk).prop("disabled", false);
      this.onCheckHold(false, item);
    }
    if (event == true && record.HoldStatus != holdFlag) {
      let holdChk = "#holdchk" + item.PQuantity_Id;
      $(holdChk).prop("checked", false);
      $(holdChk).prop("disabled", true);
      this.onCheckHold(false, item);
    }
    if (isRecordDc == dcFlag) {
      let dcChk = "#dcchk" + item.PQuantity_Id;
      $(dcChk).prop("checked", record.POrder_Status != 1 ? true : false);
      var checkDc = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
      if (checkDc == undefined) {
        let obj =
        {
          PorderId: item.porder_Id,
          PQuantityId: item.PQuantity_Id,
          PatientId: this.residentId,
          HoldChangeFlag: 0,
          HoldStatus: 0,
          DcChangeFlag: 0,
          DcStatus: 0,
          DcsPlit: 0,
          UpdatedBy: this.userID,
          UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        }
        this.updateOrdersRecords.push(obj);
        var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
        if (changesRecords.length > 0) {
          this.valueChangesFlagReceive = 1;
        }
      }
      else {
        checkDc.DcChangeFlag = 0,
          checkDc.DcStatus = 0,
          checkDc.DcsPlit = 0,
          this.updateOrdersRecords.push();
        var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
        if (changesRecords.length > 0) {
          this.valueChangesFlagReceive = 1;
        }
      }
    }
    else if (isRecordDc != dcFlag) {
      if (event == true) {
        // this.isDcChecked=true;
        // this.checkedPorderId=item.porder_Id;
        // this.checkedPquantityId=item.PQuantity_Id;
        var checkDc = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
        if (checkDc == undefined) {
          let obj =
          {
            PorderId: item.porder_Id,
            PQuantityId: item.PQuantity_Id,
            PatientId: this.residentId,
            HoldChangeFlag: 0,
            HoldStatus: 0,
            DcChangeFlag: 1,
            DcStatus: 1,
            DcsPlit: item.split,
            UpdatedBy: this.userID,
            UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
          }
          this.updateOrdersRecords.push(obj);
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
        }
        else {
          checkDc.DcChangeFlag = 1,
            checkDc.DcStatus = 1,
            checkDc.DcsPlit = item.split,
            this.updateOrdersRecords.push();
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
        }
      }
      else {
        var checkDc = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
        if (checkDc == undefined) {
          let obj =
          {
            PorderId: item.porder_Id,
            PQuantityId: item.PQuantity_Id,
            PatientId: this.residentId,
            HoldChangeFlag: 0,
            HoldStatus: 0,
            DcChangeFlag: 1,
            DcStatus: 2,
            DcsPlit: 0,
            UpdatedBy: this.userID,
            UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
          }
          this.updateOrdersRecords.push(obj);
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
        }
        else {
          checkDc.DcChangeFlag = 1,
            checkDc.DcStatus = 2,
            checkDc.DcsPlit = 0,
            this.updateOrdersRecords.push();
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
        }
      }
    }
  }
  getSelectedOrderHold() {
    var dt1 = this.orderholdform.value.orderHoldFormDate;
    var dt2 = this.orderholdform.value.orderHoldToDate;
    if (dt1 > dt2) {
      this.alertService.warn("End date cannot be before start date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (dt2 < dt1) {
      this.alertService.warn("Start date cannot be after end date");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      // let holdDate=this.dateFormatPipe.transformISODate(dt2);
      // let holdDateId="#holddate"+this.checkedPquantityId;
      // $(holdDateId).val(holdDate);
      this.modalholdIsOpen = false;
      this.holdObj = {
        HoldFrom: this.dateFormatPipe.transform(this.orderholdform.value.orderHoldFormDate),
        HoldTo: this.dateFormatPipe.transform(this.orderholdform.value.orderHoldToDate),
        HoldReason: this.orderholdform.value.orderHoldReason,
      }
      this.orderholdform.reset();
      this.orderholdform.patchValue({
        orderHoldFormDate: '',
        orderHoldToDate: '',
        orderHoldReason: '',
      });
    }
    var dcChanges = this.updateOrdersRecords.filter(up => up.DcChangeFlag == 1 && up.DcStatus == 1);
    if (dcChanges.length > 0 && (this.dcObj == undefined || this.dcObj == null)) {
      this.isDcChecked = true;
      this.DcForm.reset();
      this.multipleOrdersplits = 1;
      this.modalDcConfirmationIsOpen = true;
      setTimeout(() => {
        this.discontinueFocus.nativeElement.focus()
      }, 300);
    }
    else {
      this.updateMultipleOrderChanges();
    }
  }
  selectedOrderHoldsCancel() {
    // let holdChk="#holdchk"+this.checkedPquantityId;
    // $(holdChk).prop("checked",false);
    this.modalholdIsOpen = false;
    this.rightGridOnHoldChangesFlag = false;
    this.rightGridDCChangesFlag = false;
    this.profileDCChangesCheckState = {};
    this.profileONHoldChangesCheckState ={};
    this.orderholdform.reset();
    this.orderholdform.patchValue({
      orderHoldFormDate: '',
      orderHoldToDate: '',
      orderHoldReason: '',
    });
    this.updateOrdersRecords = [];
    this.holdObj = null;
    this.dcObj = null;
    this.noDCSplitsFlag = 0;
    this.profileONHoldChangesCheckState={}
    this.profileDCChangesCheckState ={}
    this.GetResidentAllOrdersData();
  }
  multipleOrdersDiscontinueConfirmation() {
    this.isDcChecked = true;
    this.modalHistoryIsOpen = false;
    this.DcForm.reset();
    this.multipleOrdersplits = 0;
    this.modalDcConfirmationIsOpen = true;
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    this.profileDCChangesCheckState = {};
    this.profileONHoldChangesCheckState ={};
    setTimeout(() => {
      this.discontinueFocus.nativeElement.focus()
    }, 300);
  }
  closeMultpleDcModel() {
    // let dcChk="#dcchk"+this.checkedPquantityId;
    // $(dcChk).prop("checked",false);
    this.modalHistoryIsOpen = false;
    this.modalDcConfirmationIsOpen = false;
    this.DcForm.reset();
    this.multipleOrdersplits = 0;
    this.updateOrdersRecords = [];
    this.holdObj = null;
    this.dcObj = null;
    this.noDCSplitsFlag = 0;
    this.GetResidentAllOrdersData();
    this.rightGridOnHoldChangesFlag = false;
    this.rightGridDCChangesFlag = false;
    this.profileDCChangesCheckState = {};
    this.profileONHoldChangesCheckState ={};
  }
  holdDatesCheck() {
    if ((this.orderholdform.value.orderHoldFormDate != undefined && this.orderholdform.value.orderHoldFormDate != null && this.orderholdform.value.orderHoldFormDate != "") && (this.orderholdform.value.orderHoldToDate != undefined && this.orderholdform.value.orderHoldToDate != null && this.orderholdform.value.orderHoldToDate != "")) {
      var dt1 = this.orderholdform.value.orderHoldFormDate;
      var dt2 = this.orderholdform.value.orderHoldToDate;
      if (dt1 > dt2) {
        this.alertService.warn("End date cannot be before start date");
        this.holdReleaseDate = "";
        this.ng4LoadingSpinnerService.hide();
      }
      else if (dt2 < dt1) {
        this.alertService.warn("Start date cannot be after end date");
        this.holdReleaseDate = "";
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        var conDate = this.dateFormatPipe.transform(dt2);
        this.holdReleaseDate = new Date((new Date(conDate)).getTime() + (60 * 60 * 24 * 1000));
      }
    }
  }
  multipleOrdersDiscontinue() {

    this.modalDcConfirmationIsOpen = false;
    this.dcObj = {
      DcReason: this.DcForm.value.DcReason
    };
    this.DcForm.reset();
    this.updateMultipleOrderChanges();
  }
  updateMultipleOrderChanges() {

    if (this.updateOrdersRecords.length > 0) {
      var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
      if (changesRecords.length > 0) {
        var holdChanges = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 && up.HoldStatus == 1);
        var dcChanges = this.updateOrdersRecords.filter(up => up.DcChangeFlag == 1 && up.DcStatus == 1);
        if (holdChanges.length > 0 && (this.holdObj == undefined || this.holdObj == null)&& this.rightGridOnHoldChangesFlag) {
          this.isHoldChecked = true;
          this.holdReleaseDate = "";
          this.orderholdform.reset();
          this.orderholdform.patchValue({
            orderHoldFormDate: '',
            orderHoldToDate: '',
            orderHoldReason: '',
          });
          this.modalholdIsOpen = true;
          setTimeout(() => {
            this.dateFocus.nativeElement.focus()
          }, 300);
        }
        else if (dcChanges.length > 0 && (this.dcObj == undefined || this.dcObj == null)) {
          this.isDcChecked = true;
          this.DcForm.reset();
          this.multipleOrdersplits = 1;
          this.modalDcConfirmationIsOpen = true;
          setTimeout(() => {
            this.discontinueFocus.nativeElement.focus()
          }, 300);
        }
        else {
          for (let i = 0; i < changesRecords.length; i++) {
            let splits = this.residentAllOrders.filter(re => re.porder_Id == changesRecords[i].PorderId).length;
            let selectedSplits = changesRecords.filter(re => re.PorderId == changesRecords[i].PorderId).length
            if (changesRecords[i].DcsPlit == 1 && splits == selectedSplits) {
              var dcSplits = changesRecords.filter(re => re.PorderId == changesRecords[i].PorderId);
              dcSplits.forEach(element => {
                element.DcAllSplits = 1;
                changesRecords.push();
              });
            }
            else if (changesRecords[i].DcsPlit == 1 && splits != selectedSplits && this.noDCSplitsFlag == 0) {
              this.modalDcAllSplits = true;
              break;
            }
            if (changesRecords.length == (i + 1)) {
              this.ng4LoadingSpinnerService.show();
              let holdDcObj = {
                HoldFrom: this.holdObj != undefined && this.holdObj != null ? this.holdObj.HoldFrom : null,
                HoldTo: this.holdObj != undefined && this.holdObj != null ? this.holdObj.HoldTo : null,
                HoldReason: this.holdObj != undefined && this.holdObj != null ? this.holdObj.HoldReason : "",
                DcReason: this.dcObj != undefined && this.dcObj != null ? this.dcObj.DcReason : "",
              }
              let object = {
                records: changesRecords,
                HoldDC: holdDcObj
              }
              this.dataservice.post(this.config.Emar_Orders_UpdateHoldDcMultipleOrders, object)
                .subscribe(res => {
                  if (res == 1) {
                    this.alertService.success("Save successful");
                    this.updateOrdersRecords = [];
                    this.holdObj = null;
                    this.dcObj = null;
                    this.noDCSplitsFlag = 0;
                    this.sharedService.saveChangesOrderInfo(0);
                    this.valueChangesFlagReceive = 0;
                    this.profileONHoldChangesCheckState = {};
                    this.profileDCChangesCheckState ={};
                    this.rightGridOnHoldChangesFlag = false;
                    this.rightGridDCChangesFlag = false;
                    this.GetResidentAllOrdersData();
                    this.getOrderGridData("Active", 1);
                    this.ng4LoadingSpinnerService.hide();
                  }
                  else {
                    this.alertService.error("Something went wrong");
                    this.ng4LoadingSpinnerService.hide();
                  }
                }, error => {
                  this.alertService.error(error.message);
                });
            }
          }
        }
      }
      else {
        this.alertService.warn("At least one order detail must be changed to save changes");
        this.ng4LoadingSpinnerService.hide();
      }
    }
    else {
      this.alertService.warn("At least one order detail must be changed to save changes");
      this.ng4LoadingSpinnerService.hide();
    }
    //  if ( this.favpharmacyid == 0) {
    //   this.errorMessage = 'Please select one record to proceed further';
    // }
  }
  discardAllOrdersUpdate() {
    this.alertService.success("Discarded changes");
    this.updateOrdersRecords = [];
    this.holdObj = null;
    this.dcObj = null;
    this.noDCSplitsFlag = 0;
    this.sharedService.saveChangesOrderInfo(0);
    this.rightGridOnHoldChangesFlag = false;
    this.rightGridDCChangesFlag = false;
    this.valueChangesFlagReceive = 0;
    this.GetResidentAllOrdersData();
  }
  discontinueSplits(type: number) {

    if (type == 1) {
      var dcChanges = this.updateOrdersRecords.filter(up => up.DcChangeFlag == 1 && up.DcStatus == 1 && up.DcsPlit == 1);
      dcChanges.forEach((element, index) => {
        var dcSplits = this.residentAllOrders.filter(re => re.porder_Id == element.PorderId);
        dcSplits.forEach(ele => {
          this.onCheckDC(true, ele);
        });
        if (dcChanges.length == (index + 1)) {
          this.modalDcAllSplits = false;
          this.noDCSplitsFlag = 1;
          this.updateMultipleOrderChanges();
        }
      });
    }
    else if (type == 0) {
      this.modalDcAllSplits = false;
      this.noDCSplitsFlag = 1;
      this.updateMultipleOrderChanges();
    }
  }
  checkOrderHold(item: any) {
    if (this.updateOrdersRecords.length > 0) {
      var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id && re.HoldChangeFlag == 1);
      if (checkHold != undefined && checkHold.HoldStatus == 1) {
        return 1;
      }
      else if (checkHold != undefined && checkHold.HoldStatus != 1) {
        return 2;
      }
      else if (checkHold == undefined && item.HoldStatus == 1) {
        return 1;
      }
      else if (checkHold == undefined && item.HoldStatus == 0) {
        return 2;
      }
    }
    else {
      return 0;
    }
  }
  checkOrderDC(item: any) {
    if (this.updateOrdersRecords.length > 0) {
      var checkDC = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id && re.DcChangeFlag == 1);
      if (checkDC != undefined) {
        return 1;
      }
      else {
        return 0;
      }
    }
    else {
      return 0;
    }
  }
  getPdfReport() {
    this.ng4LoadingSpinnerService.show();
    let dateTime = this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.dataservice.getFile(this.config.Emar_Reports_GetCPOEOrderDetailsReport + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId + "/" + dateTime)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "CPOE" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  back() {
    //localStorage.setItem("ordersBackClick", JSON.stringify(true));
    if (this.valueChangesFlagReceive == 1 && this.newOrderFlag == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          localStorage.setItem("ordersBackClick", JSON.stringify(true));
          let fromScreenFlag=JSON.parse(localStorage.getItem("FromScreen"))
          if (fromScreenFlag == "CPOE")
            this.route.navigate(['/home/ordergridcpoe']);
          else
            this.route.navigate(['/home/ordergrid']);
        }
        modalRef.close();
      });
    }
    else {
      localStorage.setItem("ordersBackClick", JSON.stringify(true));
      let fromScreenFlag = JSON.parse(localStorage.getItem("FromScreen"))
      if (fromScreenFlag == "CPOE")
        this.route.navigate(['/home/ordergridcpoe']);
      else
        this.route.navigate(['/home/ordergrid']);
    }
  }
  resetMonthsDropSettings(limit: any) {
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
  resetWeekDropSettings(limit: any) {
    this.dropdownSettings_Week = {
      singleSelection: false,
      idField: 'Week_Id',
      textField: 'Week_Desc',
      itemsShowLimit: 1,
      allowSearchFilter: true,
      enableCheckAll: false,
      limitSelection: limit,
    };
  }
  changeIndicationText(value: any) {
    if (this.myform.value.orderTypeId != 4) {
      if (value == "") {
        const indicationValidations = this.myform.get('indication');
        indicationValidations.setValidators(Validators.required);
        indicationValidations.updateValueAndValidity();
      }
      else if (value != "") {
        //if (this.selectedindicaitem.length == 0) {
        const indicationValidations = this.myform.get('indication');
        indicationValidations.clearValidators();
        indicationValidations.updateValueAndValidity();
        // }
        // else {
        //   this.alertService.warn("Enter either Indication OR Indication Text");
        //   this.myform.patchValue({
        //     indicationFreeText: '',
        //   });
        // }
      }
    }
    else if (this.myform.value.orderTypeId == 4) {
      const indicationValidations = this.myform.get('indication');
      indicationValidations.clearValidators();
      indicationValidations.updateValueAndValidity();
      // if (value != "" && this.selectedindicaitem.length != 0) {
      //   this.alertService.warn("Enter either Indication OR Indication Text");
      //   this.myform.patchValue({
      //     indicationFreeText: '',
      //   });
      // }
    }
  }
  onDoseUomSelect(item:any)
  {

  if(this.DoseUomSSSS.length  != 0 )
  {
    if(this.DoseUomSSSS[0].Dose_Id == 1 || this.DoseUomSSSS[0].Dose_Id  == 2 )
   {
  this.selectedroItems = [];

 let routeExists = this.routes.find(r => r.Route_Id == 34);
              if (this.selectedroItems != undefined) {
                this.selectedroItems.push(this.routes.filter(r => r.Route_Id == 34)[0]);
                this.myform.patchValue({

                  route: this.selectedroItems
                })
              }
            }

  }
}
  onIndicationSelect(item: any) {
    //if (this.myform.value.indicationFreeText == undefined || this.myform.value.indicationFreeText == null || this.myform.value.indicationFreeText == '') {
    const indicationFreeTextValidations = this.myform.get('indicationFreeText');
    indicationFreeTextValidations.clearValidators();
    indicationFreeTextValidations.updateValueAndValidity();
    // }
    // else {
    //   this.alertService.warn("Enter either Indication OR Indication Text");
    //   this.selectedindicaitem = [];
    //   this.myform.patchValue({
    //     indication: this.selectedindicaitem,
    //   });
    // }
  }
  onIndicationDeSelect(item: any) {
    if (this.myform.value.orderTypeId != 4) {
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.setValidators(Validators.required);
      indicationFreeTextValidations.updateValueAndValidity();
    }
    else if (this.myform.value.orderTypeId == 4) {
      const indicationFreeTextValidations = this.myform.get('indicationFreeText');
      indicationFreeTextValidations.clearValidators();
      indicationFreeTextValidations.updateValueAndValidity();
    }
  }
  onDoseChange() {
    if (this.myform.value.dose != undefined && this.myform.value.dose != null && this.myform.value.dose != "" && this.selectedsig1Frequency.length != 0) {
      var check = this.sigsDoseFreqArray.findIndex(si => ((si.dose == this.myform.value.dose && si.frequency == this.selectedsig1Frequency[0].Frequency_Id) || (si.prn == 1 && this.frequencyList.find(f => f.Frequency_Id == this.selectedsig1Frequency[0].Frequency_Id).Frequency_PRN == 1)));
      if (check != -1 && check != 0) {
        this.alertService.warn("This is not a valid split");
        this.myform.patchValue({
          dose: '',
        });
      }
    }
    if ((this.myform.value.dose != undefined && this.myform.value.dose != null && (this.myform.value.dose == 'SS' || this.myform.value.dose == 'UD')) || (this.myform.value.orderTypeId == 4)) {
      const addInstValidations = this.myform.get('addInst');
      addInstValidations.setValidators(([Validators.required, Validators.maxLength(250)]));
      addInstValidations.updateValueAndValidity();
    }
    else {
      const addInstValidations = this.myform.get('addInst');
      addInstValidations.setValidators(Validators.maxLength(250));
      addInstValidations.updateValueAndValidity();
    }
  }
  getFrequencyList(nsId: any) {
    this.dataservice.get<FrequencyMasterDataWithShifts[]>(this.config.Emar_Orders_GetFrequencyMasterDataWithShifts + nsId)
      .subscribe(res => {
        this.frequencyList = res;
        var list = this.frequencyList.filter(f => f.Frequency_PRN != 1);
        if (list != undefined) {
          this.frequencyListWithoutPRN = list;
        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  additionalAdministerSchedules() {

    if ( (this.myform.value.sig1Frequency != undefined && this.myform.value.sig1Frequency != null && this.myform.value.sig1Frequency.length > 0 && this.myform.value.dose != undefined && this.myform.value.dose != null && this.myform.value.dose != '')
      && (((this.myform.value.dose == 'SS' || this.myform.value.dose == 'UD') && this.myform.value.addInst != '') || ((this.myform.value.dose != 'SS' && this.myform.value.dose != 'UD') && (this.myform.value.addInst != '' || this.myform.value.addInst == '')))
      && ((this.myform.value.prn == true && this.myform.value.maxPerDay != '') || (this.myform.value.prn == false && (this.myform.value.maxPerDay != '' || this.myform.value.maxPerDay == '')))) {

        if(this.myform.value.DoseUom != undefined && this.myform.value.DoseUom.length   != 0  &&  this.myform.value.DoseUom  != null  )
    {

  // if(this.myform.value.route != undefined && this.myform.value.route.length   != 0  &&  this.myform.value.route.length   != 0   )

  // {


        if (this.sigsDoseFreqArray.length > 0) {
        var sig1Record = this.sigsDoseFreqArray[0];
        sig1Record.dose = this.myform.value.dose;
        sig1Record.frequency = this.selectedsig1Frequency[0].Frequency_Id;
        sig1Record.frequencyText = this.selectedsig1Frequency[0].Frequency_Name;
        sig1Record.additionalInst = this.myform.value.addInst;
        sig1Record.maxperday = this.myform.value.maxPerDay;
        sig1Record.prn = this.myform.value.prn == true ? 1 : 0;
        sig1Record.DoseUom = this.myform.value.DoseUom[0].Dose_Id
        this.sigsDoseFreqArray.push();
      }
      else {
        let obj = {
          dose: this.myform.value.dose,
          frequency: this.selectedsig1Frequency[0].Frequency_Id,
          frequencyText: this.selectedsig1Frequency[0].Frequency_Name,
          additionalInst: this.myform.value.addInst,
          maxperday: this.myform.value.maxPerDay,
          prn: this.myform.value.prn == true ? 1 : 0,
         DoseUom : this.myform.value.DoseUom[0].Dose_Id

        };
        this.sigsDoseFreqArray.push(obj);
      }
      this.resetAdministerScheduleForm();
      this.modalAdditionalAdministrationSchedules = true;
      setTimeout(() => {
        this.qtyDoseFocus.nativeElement.focus()
      }, 300);
    //}
    // else{
    //   this.alertService.warn("Please fill SIG1 details");
    // }
  }


    else{
      this.alertService.warn("Please fill SIG1 details");
    }
  }

    else {
      this.alertService.warn("Please fill SIG1 details");
    }
  }
  onSig1FrequencyChange(item: any) {

   this.isSig1DuplicateSplit();
   if (this.selectedsig1Frequency.length != 0) {
     //if (this.myform.value.orderTypeId != 4) { //START LITERAL ORDER CAN ACCEPT PRN (10/28/2025)
       if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
         this.myform.patchValue({
           prn: true
         });
         const maxpervalidation = this.myform.get('maxPerDay');
         maxpervalidation.setValidators([Validators.required]);
         maxpervalidation.updateValueAndValidity();
         this.isInteger();
         if (this.selectedsig1Frequency.length != 0) {
           if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
             this.MaxDateFalg = true;
             this.myform.patchValue({
               prn: true
             });
             const maxpervalidation = this.myform.get('maxPerDay');
             maxpervalidation.setValidators([Validators.required]);
             maxpervalidation.updateValueAndValidity();
             this.isInteger();
           }
           else
           {
             this.MaxDateFalg = false;
             this.myform.patchValue({
               prn: false
             });
             const maxpervalidation = this.myform.get('maxPerDay');
             maxpervalidation.setValidators(null);
             maxpervalidation.updateValueAndValidity();
             //this.isInteger();
           }
         }
        //  const maxpervalidation = this.myform.get('maxPerDay');
        //  maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
        //  Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        //  Validators.pattern(/^\d*(\.\d{0,3})?$/),
        //  Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        //  maxpervalidation.updateValueAndValidity();
       }
       else {
         // const maxpervalidation = this.myform.get('maxPerDay');
         // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
         // maxpervalidation.updateValueAndValidity();

         if (this.selectedsig1Frequency.length != 0) {
           if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
             this.MaxDateFalg = true;
             this.myform.patchValue({
               prn: true
             });
             const maxpervalidation = this.myform.get('maxPerDay');
             maxpervalidation.setValidators([Validators.required]);
             maxpervalidation.updateValueAndValidity();
             this.isInteger();
           }
           else
           {
             this.MaxDateFalg = false;
             this.myform.patchValue({
               prn: false
             });
             const maxpervalidation = this.myform.get('maxPerDay');
             maxpervalidation.setValidators(null);
             maxpervalidation.updateValueAndValidity();
             this.isInteger();
           }
         }
       //   this.myform.patchValue({
       //     prn: false,
       //   });
       //   const maxpervalidation = this.myform.get('maxPerDay');
       //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
       //   maxpervalidation.updateValueAndValidity();
        }
        //START LITERAL ORDER CAN ACCEPT PRN (10/28/2025)
    //  }
    //  else if (this.myform.value.orderTypeId == 4) {
    //    if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
    //      this.alertService.warn("PRN can't set to literal orders")
    //      this.selectedsig1Frequency = [];
    //      this.myform.patchValue({
    //        sig1Frequency: this.selectedsig1Frequency,
    //      });
    //    }
    //  }
   }
   else {

     if (this.selectedsig1Frequency.length != 0) {
       if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
         this.MaxDateFalg = true;
       }
       else
       {
         this.MaxDateFalg = false;
         const maxpervalidation = this.myform.get('maxPerDay');
         maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
         Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
         Validators.pattern(/^\d*(\.\d{0,3})?$/),
         Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
         maxpervalidation.updateValueAndValidity();
       }
     }
     this.myform.patchValue({
       prn: '',
     });
   }
 }
  checkSig1PRN(value: any) {
debugger
    if (value == true) {
      if (this.selectedsig1Frequency.length != 0) {
        if (this.frequencyList.find(f => f.Frequency_Id == this.selectedsig1Frequency[0].Frequency_Id).Frequency_PRN == 1) {

          this.MaxDateFalg = true;
        }
        else
        {
          this.MaxDateFalg = false;
        }
      }
      // const maxpervalidation = this.myform.get('maxPerDay');
      // maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      // Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      // Validators.pattern(/^\d*(\.\d{0,3})?$/),
      // Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
      // maxpervalidation.updateValueAndValidity();
      const maxpervalidation = this.myform.get('maxPerDay');
       maxpervalidation.setValidators([Validators.required]);
       maxpervalidation.updateValueAndValidity();
        //the order is literal prn order then make maxperday 0 (20/28/2025) start
      if (this.myform.value.literal == true) {
        this.myform.patchValue({
          maxPerDay: "0"
        });
      }
      // end
       this.isInteger();
      //this.selectedsig1Frequency = [];
     // this.selectedsig1Frequency.push(this.frequencyList.filter(f => f.Frequency_Id == '1')[0]);

      // this.myform.patchValue({
      //   sig1Frequency: this.selectedsig1Frequency,
      // });
      this.isSig1DuplicateSplit();
    }
    else if (value == false) {

      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
      //maxpervalidation.clearValidators();
      maxpervalidation.updateValueAndValidity();
     // this.selectedsig1Frequency = [];

      // this.myform.patchValue({
      //   sig1Frequency: this.selectedsig1Frequency,
      // });

      if (this.selectedsig1Frequency.length != 0) {
        if (this.frequencyList.find(f => f.Frequency_Id == this.selectedsig1Frequency[0].Frequency_Id).Frequency_PRN == 1) {
          this.selectedsig1Frequency = [];

          this.myform.patchValue({
            sig1Frequency: this.selectedsig1Frequency,
          });
             
          const maxpervalidation = this.myform.get('maxPerDay');
          maxpervalidation.setValidators([Validators.required]);
          maxpervalidation.updateValueAndValidity();
          this.isInteger();
          this.MaxDateFalg = true;
        }
        else
        {

          this.MaxDateFalg = false;
        }
      }
      //the order is literal prn order then make maxperday 0 (20/28/2025) start
      if (this.myform.value.literal == true) {
              const maxpervalidation = this.myform.get('maxPerDay');
         maxpervalidation.setValidators(null);
      maxpervalidation.updateValueAndValidity();
      this.myform.controls.maxPerDay.setErrors(null);
      maxpervalidation.updateValueAndValidity();
      }
      //end

      this.isSig1DuplicateSplit();
    }
  }
  isSig1DuplicateSplit() {

    if (this.myform.value.dose != undefined && this.myform.value.dose != null && this.myform.value.dose != "" && this.selectedsig1Frequency.length != 0) {
      var check = this.sigsDoseFreqArray.findIndex(si => ((si.dose == this.myform.value.dose && si.frequency == this.selectedsig1Frequency[0].Frequency_Id) || (si.prn == 1 && this.frequencyList.find(f => f.Frequency_Id == this.selectedsig1Frequency[0].Frequency_Id).Frequency_PRN == 1)));
      if (check != -1 && check != 0) {
        this.alertService.warn("This is not a valid split");
        this.selectedsig1Frequency = [];
        this.myform.patchValue({
          sig1Frequency: this.selectedsig1Frequency,
          prn: '',
        });
      }
    }
  }
  resetAdministerScheduleForm() {
    this.editSigDose = null;
    this.editSigFrequency = null;
    this.additionalscheduleform.reset();
    this.selectedsig2Frequency = [];

    this.selectedsig2DoseUom = [];
    this.additionalscheduleform.patchValue({
      sig2dose: '',
      sig2DoseUom:this.myform.value.DoseUom

    });
    //SIG2
    const sig2doseValidations = this.additionalscheduleform.get('sig2dose');
    sig2doseValidations.setValidators([Validators.required]);
    sig2doseValidations.updateValueAndValidity();

    const sig2doseUomValidations = this.additionalscheduleform.get('sig2DoseUom');
    sig2doseUomValidations.setValidators([Validators.required]);
    sig2doseUomValidations.updateValueAndValidity();


    const sig2FrequencyValidations = this.additionalscheduleform.get('sig2Frequency');
    sig2FrequencyValidations.setValidators([Validators.required]);
    sig2FrequencyValidations.updateValueAndValidity();
    const sig2addInstValidations = this.additionalscheduleform.get('sig2addInst');
    sig2addInstValidations.setValidators([Validators.maxLength(250)]);
    sig2addInstValidations.updateValueAndValidity();
    const sig2maxPerDayValidations = this.additionalscheduleform.get('sig2maxPerDay');
    sig2maxPerDayValidations.setValidators([Validators.maxLength(6), Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
    Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
    Validators.pattern(/^\d*(\.\d{0,3})?$/),
    Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
    sig2maxPerDayValidations.updateValueAndValidity();
  }
  // SIG2 validations
  onSig2DoseChange() {
    if ((this.additionalscheduleform.value.sig2dose != undefined && this.additionalscheduleform.value.sig2dose != null && (this.additionalscheduleform.value.sig2dose == 'SS' || this.additionalscheduleform.value.sig2dose == 'UD')) || (this.myform.value.orderTypeId == 4)) {
      const sig2addInstValidations = this.additionalscheduleform.get('sig2addInst');
      sig2addInstValidations.setValidators(([Validators.required,Validators.maxLength(250)]));
      sig2addInstValidations.updateValueAndValidity();
    }
    else {
      const sig2addInstValidations = this.additionalscheduleform.get('sig2addInst');
      sig2addInstValidations.setValidators(Validators.maxLength(250));
      sig2addInstValidations.updateValueAndValidity();
    }
  }
  onSig2FrequencyChange(item: any) {

    if (this.additionalscheduleform.value.sig2Frequency != undefined && this.additionalscheduleform.value.sig2Frequency != null && this.additionalscheduleform.value.sig2Frequency.length != 0) {
      //if (this.myform.value.orderTypeId != 4) { // START LITERAL ORDER CAN ACCEPT PRN (10/28/2025)
        if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
          this.additionalscheduleform.patchValue({
            sig2prn: true
          });

          if (this.selectedsig2Frequency.length != 0) {
            if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {

              this.Max3DateFalg = true;
              this.additionalscheduleform.patchValue({
                sig2prn: true
              });
            }
            else
            {
              this.Max3DateFalg = false;
              this.additionalscheduleform.patchValue({
                sig2prn: false
              });
            }
          }
          const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
          sig2maxpervalidation.setValidators([Validators.maxLength(6), Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
          Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          Validators.pattern(/^\d*(\.\d{0,3})?$/),
          Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
          sig2maxpervalidation.updateValueAndValidity();
        }
        else {

        //   this.additionalscheduleform.patchValue({
        //     sig2prn: false,
        //   });
        // const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
        // sig2maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        // sig2maxpervalidation.updateValueAndValidity();
        if (this.selectedsig2Frequency.length != 0) {
          if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
            this.Max3DateFalg = true;
            this.additionalscheduleform.patchValue({
              sig2prn: false
            });
          }
          else
          {
            this.Max3DateFalg = false;
            this.additionalscheduleform.patchValue({
              sig2prn: false
            });
          }
        }


        //   const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
        //   sig2maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        //   sig2maxpervalidation.updateValueAndValidity();
         }
         //START LITERAL ORDER CAN ACCEPT PRN (10/28/2025)
      // }
      // else if (this.myform.value.orderTypeId == 4) {
      //   if (this.frequencyList.find(f => f.Frequency_Id == item.Frequency_Id).Frequency_PRN == 1) {
      //     this.alertService.warn("PRN can't set to literal orders")
      //     this.selectedsig2Frequency = [];
      //     this.additionalscheduleform.patchValue({
      //       sig2Frequency: this.selectedsig2Frequency,
      //     });
      //   }
      // }
    }//END LITERAL ORDER CAN ACCEPT PRN (10/28/2025)
    else {
      this.additionalscheduleform.patchValue({
        sig2prn: '',
      });
    }
  }
  checkSig2Prn(value: any) {

    if (value == true) {


      if (this.selectedsig2Frequency.length != 0) {
        if (this.frequencyList.find(f => f.Frequency_Id == this.selectedsig2Frequency[0].Frequency_Id).Frequency_PRN == 1) {

          this.Max3DateFalg = true;
          // const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
          // sig2maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          // sig2maxpervalidation.updateValueAndValidity();
        }
        else
        {
          this.Max3DateFalg = false;

        }

      }
      const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
      sig2maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
      sig2maxpervalidation.updateValueAndValidity();
    //  this.selectedsig2Frequency = [];
    //  this.selectedsig2Frequency.push(this.frequencyList.filter(f => f.Frequency_Id == '1')[0]);

      // this.additionalscheduleform.patchValue({
      //   si21Frequency: this.selectedsig2Frequency,
      // });
    }
    else if (value == false) {


      if (this.selectedsig2Frequency.length != 0) {
        if (this.frequencyList.find(f => f.Frequency_Id == this.selectedsig2Frequency[0].Frequency_Id).Frequency_PRN == 1) {
          this.selectedsig2Frequency = [];

          this.myform.patchValue({
            sig2Frequency: this.selectedsig2Frequency,
          });
              // const maxpervalidation = this.myform.get('sig2maxPerDay');
              // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
              // maxpervalidation.updateValueAndValidity();
              this.Max3DateFalg = false;
        }
        else
        {
          const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
      sig2maxpervalidation.setValidators([Validators.maxLength(6), Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
      Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
      Validators.pattern(/^\d*(\.\d{0,3})?$/),
      Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
      sig2maxpervalidation.updateValueAndValidity();
          this.Max3DateFalg = false;
        }

      }
     // this.selectedsig2Frequency = [];

      // this.additionalscheduleform.patchValue({
      //   si21Frequency: this.selectedsig2Frequency,
      // });
    }
  }
  addAdditionalSchedule() {
debugger;
    if (this.editSigDose == null && this.editSigFrequency == null && this.sigsDoseFreqArray.length == 4) {
      this.alertService.warn("Splits reached it's limit");
    }
    else {
      if (this.additionalscheduleform.invalid == true) {
        this.alertService.warn("Please enter required inputs");
      }
      else {

        if (this.additionalscheduleform.value.sig2dose != undefined && this.additionalscheduleform.value.sig2dose != null && this.additionalscheduleform.value.sig2dose != "" && this.selectedsig2Frequency != undefined && this.selectedsig2Frequency != null && this.selectedsig2Frequency.length != 0) {
          var check = this.sigsDoseFreqArray.find(si => ((si.dose == this.additionalscheduleform.value.sig2dose && si.frequency == this.selectedsig2Frequency[0].Frequency_Id) || (si.prn == 1 && this.frequencyList.find(f => f.Frequency_Id == this.selectedsig2Frequency[0].Frequency_Id).Frequency_PRN == 1)));
          let qtyCheck = this.sigsDoseFreqArray.find(si => ((si.dose == this.additionalscheduleform.value.sig2dose)))
          if(qtyCheck != undefined){
            this.alertService.warn("Same Qty/Dose already existed for this split");
          }
          if (check != undefined && ((this.editSigDose == null && this.editSigFrequency == null) || (check.dose != this.editSigDose || check.frequency != this.editSigFrequency))) {
            this.alertService.warn("This is not a valid split");
          }
          else {
            if (this.editSigDose == null && this.editSigFrequency == null) {
              let obj = {
                dose: this.additionalscheduleform.value.sig2dose,
                frequency: this.selectedsig2Frequency[0].Frequency_Id,
                frequencyText: this.selectedsig2Frequency[0].Frequency_Name,
                additionalInst: this.additionalscheduleform.value.sig2addInst,
                maxperday: this.additionalscheduleform.value.sig2maxPerDay,
                prn: this.additionalscheduleform.value.sig2prn == true ? 1 : 0,
               Seg2DoseUom: this.selectedsig2DoseUom[0].Dose_Id
              };
              this.sigsDoseFreqArray.push(obj);
              this.resetAdministerScheduleForm();
            }
            else {
              var editedRecord = this.sigsDoseFreqArray.find(si => si.dose == this.editSigDose && si.frequency == this.editSigFrequency);
              if (editedRecord != undefined) {
                editedRecord.dose = this.additionalscheduleform.value.sig2dose;
                editedRecord.frequency = this.selectedsig2Frequency[0].Frequency_Id;
                editedRecord.frequencyText = this.selectedsig2Frequency[0].Frequency_Name;
                editedRecord.additionalInst = this.additionalscheduleform.value.sig2addInst;
                editedRecord.maxperday = this.additionalscheduleform.value.sig2maxPerDay;
                editedRecord.prn = this.additionalscheduleform.value.sig2prn == true ? 1 : 0;
                editedRecord.Seg2DoseUom = this.selectedsig2DoseUom[0].Dose_Id;

                this.sigsDoseFreqArray.push();
                this.resetAdministerScheduleForm();
              }
              else {
                let obj = {
                  dose: this.additionalscheduleform.value.sig2dose,
                  frequency: this.selectedsig2Frequency[0].Frequency_Id,
                  frequencyText: this.selectedsig2Frequency[0].Frequency_Name,
                  additionalInst: this.additionalscheduleform.value.sig2addInst,
                  maxperday: this.additionalscheduleform.value.sig2maxPerDay,
                  prn: this.additionalscheduleform.value.sig2prn == true ? 1 : 0,
                  Seg2DoseUom: this.selectedsig2DoseUom[0].Dose_Id
                };
                this.sigsDoseFreqArray.push(obj);
                this.resetAdministerScheduleForm();
              }
            }
          }
        }
      }
    }
  }
  onAdiitionalSigSelect(index: number, item: any) {

    this.editSigDose = item.dose;
    this.editSigFrequency = item.frequency;
    this.selectedsig2Frequency = [];
    this.selectedsig2DoseUom = [] ;
    this.selectedsig2Frequency.push(this.frequencyList.filter(f => f.Frequency_Id == item.frequency.toString())[0]);
    this.selectedsig2DoseUom.push(this.DoseUomDrop.filter(f => f.Dose_Id == item.Seg2DoseUom.toString())[0]);
    this.additionalscheduleform.patchValue({
      sig2dose: item.dose,
      sig2Frequency: this.selectedsig2Frequency,
      sig2addInst: item.additionalInst,
      sig2prn: item.prn == true ? 1 : 0,
      sig2maxPerDay: item.maxperday,
      sig2DoseUom:this.selectedsig2DoseUom,
    });
    if ((item.dose != undefined && item.dose != null && (item.dose == 'SS' || item.dose == 'UD')) || (this.myform.value.orderTypeId == 4)) {
      const sig2addInstValidations = this.additionalscheduleform.get('sig2addInst');
      sig2addInstValidations.setValidators(([Validators.required,Validators.maxLength(250)]));
      sig2addInstValidations.updateValueAndValidity();
    }
    else {
      const sig2addInstValidations = this.additionalscheduleform.get('sig2addInst');
      sig2addInstValidations.setValidators(Validators.maxLength(250));
      sig2addInstValidations.updateValueAndValidity();
    }
    if ((item.Sig2DoseUom != undefined )&& (this.myform.value.orderTypeId == 4 || item.Seg2DoseUom.lenght !=0)) {
      const sig2DoseUomValidations = this.additionalscheduleform.get('sig2DoseUom');
      sig2DoseUomValidations.setValidators(Validators.required);
      sig2DoseUomValidations.updateValueAndValidity();
    }
    else {
      const sig2DoseUomValidations = this.additionalscheduleform.get('sig2DoseUom');
      sig2DoseUomValidations.setValidators(Validators.required);
      sig2DoseUomValidations.updateValueAndValidity();
    }

    if (this.selectedsig2Frequency != undefined && this.selectedsig2Frequency != null && this.selectedsig2Frequency.length != 0) {
      if (this.frequencyList.find(f => f.Frequency_Id == item.frequency.toString()).Frequency_PRN == 1) {
        this.additionalscheduleform.patchValue({
          sig2prn: true
        });
        const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
        sig2maxpervalidation.setValidators([Validators.maxLength(6), Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
        Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        Validators.pattern(/^\d*(\.\d{0,3})?$/),
        Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        sig2maxpervalidation.updateValueAndValidity();
      }
      else {
        this.additionalscheduleform.patchValue({
          sig2prn: false,
        });
        const sig2maxpervalidation = this.additionalscheduleform.get('sig2maxPerDay');
        sig2maxpervalidation.setValidators([Validators.maxLength(6), Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
        Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        Validators.pattern(/^\d*(\.\d{0,3})?$/),
        Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        sig2maxpervalidation.updateValueAndValidity();
      }
    }
    else {
      this.additionalscheduleform.patchValue({
        sig2prn: '',
      });
    }
  }
  isSig1DoseValid(inputtxt:any){

    if(inputtxt !="" && inputtxt.length!=0)
    {
    var decimal='^[0-9]+(\.[0-9]{1,3})?$';
    if(((inputtxt.match(decimal)==null && (inputtxt.toUpperCase().split('.').join('')!="SS" && inputtxt.toUpperCase()!="UD"))) || (inputtxt.match(decimal)!=null && parseFloat(inputtxt).toFixed(3)=="0.000"))
    {
      this.alertService.warn("Inavlid Qty/Dose");
      this.myform.patchValue({
        dose:'',
      });
    }
    else if(inputtxt.match(decimal)==null && (inputtxt.toUpperCase().split('.').join('')=="SS" || inputtxt.toUpperCase()=="UD"))
    {
      this.myform.patchValue({
        dose:inputtxt.toUpperCase().split('.').join(''),
      });
      this.onDoseChange();
    }
    else if(inputtxt.match(decimal)!=null){
      this.onDoseChange();
    }
  }
  else{
    this.onDoseChange();
  }
  }
  isSig2DoseValid(inputtxt:any){
    const checkQtyDose = this.sigsDoseFreqArray.find(obj => obj.dose == inputtxt);
    if(inputtxt !="" && inputtxt.length!=0){
      if(checkQtyDose == undefined){
        var decimal='^[0-9]+(\.[0-9]{1,3})?$';
        if(((inputtxt.match(decimal)==null && (inputtxt.toUpperCase().split('.').join('')!="SS" && inputtxt.toUpperCase()!="UD"))) || (inputtxt.match(decimal)!=null && parseFloat(inputtxt).toFixed(3)=="0.000"))
        {
          this.alertService.warn("Inavlid Qty/Dose");
          this.additionalscheduleform.patchValue({
            sig2dose:'',
          });
        }
        else if(inputtxt.match(decimal)==null && (inputtxt.toUpperCase().split('.').join('')=="SS" || inputtxt.toUpperCase()=="UD"))
        {
          this.additionalscheduleform.patchValue({
            sig2dose:inputtxt.toUpperCase().split('.').join(''),
          });
          this.onSig2DoseChange();
        }
        else if(inputtxt.match(decimal)!=null){
          this.onSig2DoseChange();
        }
      }
      else{
        this.alertService.warn("Same Qty/Dose already exist for another split of this order");
        this.additionalscheduleform.patchValue({
          sig2dose:'',
        });
      }
    }  
    else{
      this.onSig2DoseChange();
    }
  }
    
  removeAdditionalSchdule(index:number)
  {
    this.sigsDoseFreqArray.splice(index, 1);
    this.resetAdministerScheduleForm();
  }
  cancelAddtionalSchedules()
  {

    if(this.sigsDoseFreqArray.length>1)
    {
    this.modalCancelAdditionalScheduleIsOpen=true;
    }
    else{
      this.modalAdditionalAdministrationSchedules=false;
    }
    this.updateQtyHand(this.sigsDoseFreqArray)

  }
  removeAdditionalSchedules(type:number)
  {
    if(type==1)
    {
    this.sigsDoseFreqArray.splice(1,this.sigsDoseFreqArray.length-1);
    this.resetAdministerScheduleForm();
    this.modalCancelAdditionalScheduleIsOpen=false;
    this.modalAdditionalAdministrationSchedules=false;
    if(!this.modalAdditionalAdministrationSchedules){
      this.updateQtyHand(this.sigsDoseFreqArray)    }
    
    }
    else if(type==0)
    {
      this.modalCancelAdditionalScheduleIsOpen=false;
    }
  }
  mouseEnter(Id:any)
  {
    this.MyImages = Id;

  }
  mouseLeave()
  {
    this.MyImages =null;

  }
  updatePregnancy(isChecked: boolean){
    if(this.demographicInfoData){
      const type = 1;
      const Pregnant = isChecked ? 1 : 0;
      this.postData = {
        PatientID: this.demographicInfoData.Patient_Id,
        Type: type,
        IScheck: Pregnant
      };
      this.dataservice.post(this.config.Emar_ResidentDemographic_UpdatePregnecyFeedingEntity , this.postData).subscribe(
         res =>{}
      )
      this.demographicInfoData.Pregnant = isChecked ? 1 : 0;
    }
  }

   updateFeeding(isChecked: boolean){
    if(this.demographicInfoData){
      const type = 2;
      const BreastFeeding = isChecked ? 1 : 0;
      let dataFeed = {
        PatientID: this.demographicInfoData.Patient_Id,
        Type: type,
        IScheck: BreastFeeding
      };
      this.dataservice.post(this.config.Emar_ResidentDemographic_UpdatePregnecyFeedingEntity , dataFeed).subscribe(
         res =>{}
      )
      this.demographicInfoData.BreastFeeding = isChecked ? 1 : 0;

    }

  }
  getWeightHeight(){
    //console.log(this.residentId,"from orders")
    if(this.getID >0){
     let id = this.getID;
     this.dataservice.get<any>(this.config.Weight_GetWeightList + id )
     .subscribe(res => {
      if(res.length>=1){
        this.weightDetails = res[res.length-1];
        //console.log(this.weightDetails,"response");
        if(this.weightDetails.Patient_Id ==id){
          if(this.weightDetails.HeightFeet > 0){
            this.isHeight = true;
          }
          if(this.weightDetails.Weight>0){
            this.isWeight = true;
          }
        }
      } else{
        this.isWeight = false;
        this.isHeight = false;
      }

     })
    }


   }
  onPharmacyNameChange() {
            if (this.PharmacyNameinput && this.PharmacyNameinput.length > 2) {
              this.activePharmacies = this.data1.filter(p => p.Pharmacy_Status == 1);
              this.favoritePharmacies = this.activePharmacies.filter(item => item.Pharmacy_Fav == 1);
              this.filteredPharmaciess = this.favoritePharmacies.filter(item => {
                this.pharmacyflag = true;
                return item.PharmacyName && item.PharmacyName.toLowerCase().includes(this.PharmacyNameinput.toLowerCase())
              }
              );
              // console.log(this.filteredPharmaciess , 'pharmacy onkeyup');
              // console.log(this.filteredPharmaciess[0].PharmacyName , 'check')
     }
     else {
      this.pharmacyflag = false;
     }
  }
  defaultNameChange(){
    if(this.pharmalifeInput){
    const userId = this.persistanceService.get(this.config.loggedInUserKey);
          this.dataservice.get<any[]>(this.config.Emar_pharmacy_getPharmacyData + userId)
            .subscribe(res => {
              this.data1 = res;
              this.pharmacyDefault = this.data1.filter((item: any) => item.PharmacyName == 'pharmalife');
            })
      this.myform.patchValue({
        pharmacy: this.pharmacyDefault[0].PharmacyName,
      });
      this.pharmacyflag = false;
     }
  }
  onselectPharmacybyBoth(item: any) {
      if (item != '') {
        this.pharmacyNameChanged = false;
        this.favpharmacyid = item.Pharmacy_Id;
        this.myform.patchValue({
          pharmacy: item.PharmacyName,
        });
        this.pharmacyflag = false;
    }

  }
  onselectPharmacy(item: any) {

    if (item != '') {
      this.pharmacyNameChanged = false;
      this.favpharmacyid = item.pharmacyid

      this.myform.patchValue({
        pharmacy: item.selectedItem,
      });
      this.flag = false;

     }
    else {
      return false;
    }
  }

  onselectPharmacyFromDrugDB(item: any) {
    if (item != '') {
      if(this.onPharmacyNameChange){
        this.pharmacyNameChanged=true
      }
      this.pharmacyNameChanged = false;
      this.myform.patchValue({
        pharmacy: item.PharmacyName,
      });
      this.flag = false;
    }
    else {
      return false;
    }

    }

searchPharmacyNames(searchText: string) {
  if (this.isReadOnly == true) {
    this.alertService.warn('Pharmacy name search is not available');
  }
  else {
    if (this.PharmacyNameinput != null && this.PharmacyNameinput.length > 2) {
      let searchData = {
        "searchText": this.PharmacyNameinput,
        "pharmacyid": this.favpharmacyid
      }
      this.modalOption.size = 'lg';
      const modalRef = this.modalService.open(SearchpharmacynameComponent, this.modalOption);
      modalRef.componentInstance.searchDrugText = searchData;
      modalRef.componentInstance.searchResult.subscribe((receivedResult) => {

          this.onselectPharmacy(receivedResult);
        modalRef.close();
      })
    }
    else
      this.alertService.warn('Please enter at least 3 characters of pharmacy name');
  }
  this.pharmacyflag = false;

}
navigateToDrfirstUrl() {
  this.patientid
  let userId = this.persistanceService.get(this.config.loggedInUserKey);
  this.dataservice.get<any>(this.config.Emar_Rcopia_GetRedirectURL + this.patientid + "/" + userId).subscribe(res => {
  this.urldata=res
  window.open().location.href = this.urldata;
 })
}
onPrescriberSelect(item){
  // console.log(item,"item")
  debugger
  this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
   const selectedPhy = this.physiciansdrop.filter(x=> x.Physician_Id == item.Physician_Id);
   this.selectedphyItems = selectedPhy;
   if(selectedPhy !=null && selectedPhy.length){
         this.newOrderFlag == 1 ? this.physiciansdrop = this.physicianfullList.filter(x =>x.PStatus ===1): this.physiciansdrop = this.physicianfullList;
     if(selectedPhy[0].PStatus == 0 ){
      this.supervisingInactiveModal = true;
      this.physicianAlertMsg ="Selected Physician is inactive"
     }
     else if( (selectedPhy[0].CredeValue == 'NP' || selectedPhy[0].CredeValue == 'PA' || selectedPhy[0].CredeValue == 'Other' ) && (selectedPhy[0].SPhy ==null || selectedPhy[0].SPhy == '' || selectedPhy[0].SPhy.length ===0)){
       this.supervisingInactiveModal = true;
       this.physicianAlertMsg = "“Selected Prescriber ( " + selectedPhy[0].PhysicianFullName.toUpperCase().toUpperCase() + " ) has incomplete credentialing info. Please select another Prescriber"

     }
     else if(selectedPhy[0].SPhy !=null && selectedPhy[0].SPhy != '' && selectedPhy[0].SPhyStatus==0){
       
       this.supervisingInactiveModal = true;
       this.physicianAlertMsg =  `Selected Prescriber's (${selectedPhy[0].PhysicianFullName}) Supervising Physician( ${selectedPhy[0].SphyName}  )is inactive.`

     }
     else if(selectedPhy.length && selectedPhy[0].Credentials == 0){
        
         this.supervisingInactiveModal = true;
         this.physicianAlertMsg = "Selected Prescriber ( " + selectedPhy[0].PhysicianFullName.toUpperCase() + " )  has incomplete credentialing info. Please select another Prescriber"


     }
       else{
         this.myform.patchValue({
           physician: selectedPhy,
         });
       }
   }
   else{
     this.myform.patchValue({physician: '',});
   }
  
 }
 onPrescriberDeSelect(item){

 }

 HandlePrescriber(){
   this.newOrderFlag == 1 ? this.myform.patchValue({physician: '',}): this.myform.patchValue({physician: this.selectedphyItems,});
   this.supervisingInactiveModal = false;
 }
 updateQtyHand(value?: any) {
  if(value !=null && value != undefined && value.length != 0){
    let multqty ;
  if(!this.qtyhandTouched ){
      let freqId = value.sig1Frequency && value.sig1Frequency.length > 0 ? value.sig1Frequency[0].Frequency_Id : null;
      if (Array.isArray(this.frequencyList)) {
        this.freqTimesArray = this.frequencyList.filter(item => item.Frequency_Id == freqId);
      }
       
      const daysSup = Number(this.supplydays == '1' ? '1' : this.supplydays) ;
      
      
       if(this.sigsDoseFreqArray.length >=1  ){
        this.daysSupplyFlag = true;
       }
    
      if((this.sigsDoseFreqArray.length <=1 && (value && value.ddlresidents ? value.ddlresidents.length : 0)) || !this.daysSupplyFlag ){
        if (value.dose != null && value.dose.length > 0 ) {
         if(value.prn && value.maxPerDay != null){
          if(value.maxPerDay.length > 0 || value.maxPerDay > 0){
            multqty = Number(value.maxPerDay)
          }else{
            multqty = 0
          }
         
         }
         else{
          multqty =  ( Number(value.dose) * (this.freqTimesArray.length > 0 ? Number(this.freqTimesArray[0].Freq_Times) : 1));
        }
          multqty *= Number(daysSup) ;
        } else
        {
          multqty =0
        }
      }
      else if(this.sigsDoseFreqArray.length == 1 && (value && value.ddlresidents ? value.ddlresidents.length : 0) == 0){
        let freqId = value[0].frequency ; 
        if (Array.isArray(this.frequencyList)) {
          this.freqTimesArray = this.frequencyList.filter(item => item.Frequency_Id == freqId);
        }
        if (value[0].dose!= null && value[0].dose.length > 0) {
          if(value[0].prn && value[0].maxPerDay == null && (value[0].maxPerDay.length == 0 || value[0].maxPerDay == 0)){
            this.myform.patchValue({
              qtyHand: '',
            });
          }else{

          }
          if(value[0].prn && value[0].maxPerDay != null){
            if(value[0].maxPerDay.length > 0 || value[0].maxPerDay > 0){
              multqty = Number(value[0].maxPerDay)
            }else{
              multqty = 0
            }
           
           }
           else{
            multqty =  ( Number(value[0].dose) * (this.freqTimesArray.length > 0 ? Number(this.freqTimesArray[0].Freq_Times) : 1));
          }
          multqty *= Number(daysSup) ;
        } 
      }
      
      else if (this.sigsDoseFreqArray.length >= 1 && value.length != undefined) { 
          const prn = this.sigsDoseFreqArray.map(x => x.prn == 1);
            const freqTimeAccumulator = {};
            let totaldose = 0
            const doseValue = this.sigsDoseFreqArray.reduce((total, val) => { 
            const isPrnWithMaxPerDay = val.prn && val.maxperday !== null;
            const setValue = isPrnWithMaxPerDay ? val.maxperday : val.dose != null ? val.dose : 1;
            const matchingFreqItem = (val.frequency != null || val.frequency !=undefined) ? this.frequencyList.find((freqItem) => freqItem.Frequency_Id == val.frequency): 1;
            const freqTimes = matchingFreqItem !=1 ? matchingFreqItem.Freq_Times : 1;
            const numericValue = Number(setValue);
            if(prn.includes(true) && (val.maxperday !== null )){
              if( val.maxperday.length ==0){
                freqTimeAccumulator[val.frequency] = freqTimes;
              }else{
                if (freqTimeAccumulator[val.frequency]) {
                  freqTimeAccumulator[val.frequency] = 1;
                }else{
                  freqTimeAccumulator[val.frequency] = 1;
                }
              }
              
            }else{
              freqTimeAccumulator[val.frequency] = freqTimes;
            }
              if (!isNaN(numericValue) && numericValue !== 0) {
                
                 totaldose += numericValue * freqTimeAccumulator[val.frequency];
                }
                return total;
              }, 1);
          
            if (doseValue > 0) {
             multqty = Number(daysSup) * totaldose;
            }
        }
        else if (this.sigsDoseFreqArray.length >= 1 && value.length == undefined){
          let totaldose = 0
          const dose1 = ((value.prn && value.maxPerDay.length > 0) ? Number(value.maxPerDay) : Number(value.dose != null ? value.dose : 1))
          const matchingFreqItem = ( value.sig1Frequency.length > 0) ? this.frequencyList.find((freqItem) => freqItem.Frequency_Id == value.sig1Frequency[0].Frequency_Id): 1;
            let freqTimes = matchingFreqItem !=1 ? matchingFreqItem.Freq_Times : 1;
 
           if(matchingFreqItem !=1 && matchingFreqItem.Frequency_PRN == 1 ){
            freqTimes = 1
           }else if(value.prn){
            freqTimes = 1
           }
           const dispense = dose1 * freqTimes
          let filteredArray = this.sigsDoseFreqArray.filter(item => item.hasOwnProperty('Seg2DoseUom') && !item.hasOwnProperty('DoseUom'));
          if(filteredArray){
            let sum = 0;
            for (const item of filteredArray) {
              const isPrnWithMaxPerDay = item.prn && item.maxperday !== null;
              const dose = isPrnWithMaxPerDay ? item.maxperday : item.dose != null ? item.dose : 1
              const hanleFreq = (item.frequency !=null || item.frequency !=undefined)
              const matchingFreqItem = hanleFreq? this.frequencyList.find((freqItem) => freqItem.Frequency_Id == item.frequency) : 1;
              const freqTimes = matchingFreqItem != 1 ? matchingFreqItem.Freq_Times : 1;
              let frequency = 1
              if(isPrnWithMaxPerDay){
                frequency = 1
              }
              else{
                frequency = Number(freqTimes);
              }
              const doseFrequencySum = Number(dose) * frequency;
              sum += doseFrequencySum;
            }
            totaldose = dispense + sum
          }
          multqty = Number(daysSup) * totaldose;
       
        }
        this.formSubscription.unsubscribe();
        if (multqty !== undefined){
          let roundedNumber = Number(multqty.toFixed(2))
          this.myform.patchValue({
            qtyHand: roundedNumber,
          });
        }else if(multqty == undefined ){
          this.myform.patchValue({
            qtyHand: '',
          });
        }
        else{
          let roundedNumber = Number(multqty)
          this.myform.patchValue({
            qtyHand: roundedNumber,
          });
        }
      
        this.formSubscription = this.myform.valueChanges.subscribe(updatedValue => {
          // if(updatedValue.prn == true){
          //   const maxpervalidation = this.myform.get('maxPerDay');
          //   maxpervalidation.setValidators([Validators.required , Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
          //   Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          //   Validators.pattern(/^\d*(\.\d{0,3})?$/),
          //   Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
          //   maxpervalidation.updateValueAndValidity();
          // }
          setTimeout(() => {
            this.updateQtyHand(updatedValue);
          }, 0);
        });
     
  }

  if (this.qtyHandControl.touched) {
    this.qtyHandControl.setValue(multqty);
  }

  this.daysSupplyFlag = false;
  }
 }
 qtyTouched(){
  this.qtyhandTouched = true;
 }
  ngOnDestroy() {
    if (this.formSubscription) {
      this.formSubscription.unsubscribe();
    }
  }
  daysSupplyCheck(){
    if(this.supplydays == '0'){
      this.alertService.warn("Invalid Days Supply");
      this.myform.patchValue({
        daysSupply:'1'
      })
    }
  }
  qtyChanged(){
  
    if( parseFloat(this.myform.value.qtyHand) < 0.001){
      this.alertService.warn("Invalid Dispense Qty");
      this.myform.patchValue({
        qtyHand:''
      })
    }
   }
   onCheckHold(event: any, item: any) {
    //debugger;

    let isChecked = event == true
    this.profileONHoldChangesCheckState[item.PQuantity_Id] = isChecked;
    this.rightGridOnHoldChangesFlag = Object.values(this.profileONHoldChangesCheckState).some((state) => state);
    // if (this.rightGridOnHoldChangesFlag || this.rightGridDCChangesFlag) {
    //   this.myform.disable()
    // } else {
    //   this.myform.enable()
    // }
    let holdFlag = event == true ? 1 : 0;
    var record = this.residentAllOrders.find(re => re.porder_Id == item.porder_Id && re.PQuantity_Id == item.PQuantity_Id);

    if (record.HoldStatus == holdFlag) {
      var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
      if (checkHold == undefined) {
        let obj =
        {
          PorderId: item.porder_Id,
          PQuantityId: item.PQuantity_Id,
          PatientId: this.residentId,
          HoldChangeFlag: 1,
          HoldStatus: 1,
          DcChangeFlag: 0,
          DcStatus: 0,
          DcsPlit: 0,
          UpdatedBy: this.userID,
          UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        }

        this.updateOrdersRecords.push(obj);
        var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
        if (changesRecords.length > 0) {
          this.valueChangesFlagReceive = 1;
        } else {
          this.valueChangesFlagReceive = 0;
        }
      }
      else {
        //for existing hold ..we have record so it goes inside this else block

        checkHold.HoldChangeFlag = 1,
          checkHold.HoldStatus = 1,
          this.updateOrdersRecords.push();
        var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
        if (changesRecords.length > 0) {
          this.valueChangesFlagReceive = 1;
        } else {
          this.valueChangesFlagReceive = 0;
        }
      }
    }
    else if (record.HoldStatus != holdFlag) {

      if (event == true) {

        var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
        //for new hold ..we dont have any record so it goes inside this checkhold == undefined 
        if (checkHold == undefined) {
          let obj =
          {
            PorderId: item.porder_Id,
            PQuantityId: item.PQuantity_Id,
            PatientId: this.residentId,
            HoldChangeFlag: 1,
            HoldStatus: 1,
            DcChangeFlag: 0,
            DcStatus: 0,
            DcsPlit: 0,
            UpdatedBy: this.userID,
            UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
          }

          this.updateOrdersRecords.push(obj);
          this.orderholdform.reset();
          this.orderholdform.patchValue({
            orderHoldFormDate: '',
            orderHoldToDate: '',
            orderHoldReason: '',
          });

          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
          else {
            this.valueChangesFlagReceive = 0;
          }
        }
        else {

          checkHold.HoldChangeFlag = 1,
            checkHold.HoldStatus = 1,
            this.updateOrdersRecords.push();

          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
          else {
            this.valueChangesFlagReceive = 0;
          }
        }
      }
      else {
        let holdDate = this.dateFormatPipe.transformISODate('');
        let holdDateId = "#holddate" + item.PQuantity_Id;
        $(holdDateId).val(holdDate);
        var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id);
        if (checkHold == undefined) {
          let obj =
          {
            PorderId: item.porder_Id,
            PQuantityId: item.PQuantity_Id,
            PatientId: this.residentId,
            HoldChangeFlag: 1,
            HoldStatus: 2,
            DcChangeFlag: 0,
            DcStatus: 0,
            DcsPlit: 0,
            UpdatedBy: this.userID,
            UpdatedOn: this.dateFormatPipe.dateWithTime(new Date()),
          }

          this.updateOrdersRecords.push(obj);
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          } else {
            this.valueChangesFlagReceive = 0;
          }
        }
        else {

          checkHold.HoldChangeFlag = 1,
            checkHold.HoldStatus = 1,
            this.updateOrdersRecords.push();
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;
          }
          else {
            this.valueChangesFlagReceive = 0;
          }
        }
      }
    }
   
    //  this.holdCheckFun(item);
  }
   getDrugInteractionAlerts() {
     
    // if (!((this.myform.value.literal == true && this.myform.value.treatment == false))) {
    if (!((this.myform.value.literal == true && this.myform.value.treatment == false) || (this.myform.value.literal == true && this.myform.value.treatment == true))) {
      const gpicode = this.gpiCode;
      const patientId = this.residentId;
      const route = this.myform.value.route[0].Route_Id
      let freqId = this.myform.value.sig1Frequency == undefined || this.myform.value.sig1Frequency.length == 0 || this.myform.value.sig1Frequency == null ? null : this.myform.value.sig1Frequency[0].Frequency_Id;
      let NurseFreq_Id = freqId != null ? (freqId.startsWith('s') ? null : parseInt(freqId)) : null;
        var drug  = this.selectedDrugName
            .split('') // Split the string into individual characters
            .map(char => {
              if (char === '/') return '~';
              if (char === '.') return '$';
              if (char === '%') return '@';
              if (char === ':') return '!';
              if (char === '*') return '{';
              if (char === ">") return '_';
              if (char === "<") return '}';
              return char; // Leave any other character unchanged
            }).join('');
      this.getAllDrugAlertsDtms(gpicode,patientId,route,NurseFreq_Id,drug)
    } else {
      this.userAttempts = 0;
      this.userCredetialsForm.reset();
      this.userCredeatialsModal = true;
    }  
  }
  getAllDrugAlertsDtms(gpicode,patientId,route,NurseFreq_Id,drug){
    
          this.ng4LoadingSpinnerService.show();
    const getDrugToDrugInt =this.dataservice.get<any>(this.config.Emar_EkitMeds_DrugToDrugINT_DTMS + gpicode + '/' + patientId + '/' + route + '/' + NurseFreq_Id + '/' + encodeURIComponent(drug))
    const getDrugToAllergyInt = this.dataservice.get<any>(this.config.Emar_EkitMeds_DrugToAllergyINT_DTMS + gpicode + '/' + patientId + '/' + encodeURIComponent(drug))
     forkJoin([getDrugToDrugInt, getDrugToAllergyInt]).subscribe(([getDrugToDrugInt, getDrugToAllergyInt]) => {
      this.alertList =getDrugToDrugInt
        this.alertAllergyList=getDrugToAllergyInt
       if(this.alertList.length==0 && this.alertAllergyList.length==0 ){
        this.drugINteractionModal = false;
          this.userAttempts = 0;
          this.userCredetialsForm.reset();
          this.userCredeatialsModal = true;
        } else {
          this.drugINteractionModal = true;
        }
          this.ng4LoadingSpinnerService.hide();
     }, error => {
          this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
       });
  }
  proceedWithAlerts() {
    this.drugINteractionModal = false;
    this.userAttempts = 0;
    this.userCredetialsForm.reset();
    this.userCredeatialsModal = true;
  }
  closeDtmsPopup(){
    this.drugINteractionModal = false;
    this.alertList = [];
    this.alertAllergyList=[];
  }
}

