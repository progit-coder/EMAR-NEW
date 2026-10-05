import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, ElementRef, AfterViewInit, HostListener, ChangeDetectorRef, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DrfirstOrderXML, HOA, Orderupdate, NurseComments, CommonOrderStatus, CommonDcOrderStatus, OrdersData, WeekMasterData, MonthMasterData, HoursMasterData, DrugAdministrationTime, OrderFavourite, OrderInfoAlert, OrderHold, PhysicianDetails, OrderDestroy, OrderApproval, FrequencyMasterDataWithShifts, BarcodeEntity} from '../../../models/orders.model';
import { EmarResident, NursingSchedule, EmarOrdersList, DrugAdminister, MedicationReason, BypassBiometric, Refill, OrderFavouriteData } from '../../../models/emar.model';
import { RejectedrefillsComponent } from '../rejectedrefills/rejectedrefills.component';

import { AlertService } from '../../../_services';
import { DemographicInfo, ResidentDemographic } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';
import { DomSanitizer } from '@angular/platform-browser';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { SharedService } from '../../../services/shared/shared.service';
import { Observable, Subject, of } from 'rxjs';
import { NgbModal, NgbModalOptions, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { MergeordersComponent } from '../mergeorders/mergeorders.component';
import { SearchDrugNameComponent } from '../search-drug-name/search-drug-name.component';
import { catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { OrdersdiscardComponent } from '../ordersdiscard/ordersdiscard.component';
import { of as observableOf} from 'rxjs';
import { data } from 'jquery';
@Component({
  selector: 'app-orderinfo',
  templateUrl: './orderinfo.component.html',
  styleUrls: ['./orderinfo.component.css'],
  providers: [DataService, APIConfiguration]
})

export class OrderinfoComponent implements OnInit {
  //#region
  //created by:sampath
  @Output() saveChanges: EventEmitter<any> = new EventEmitter();
  public orderGridData: any[] = [];
  public ordersDetails = {} as OrdersData;
  public ordersObj: OrdersData;
  public physiciansdrop: PhysicianDetails[];
  public valueChangesFlagReceive: number = 0;
  //public barcodeList: BarcodeDetail[];
  //public barcodeCount: number = 0;
public isChecked:boolean = false;
  public RordersList3 :  EmarOrdersList[] = [];
  public template;
  private orderId: number = 0;
  public residentId: number=0;
public BUttonFlag:boolean = false;
  myform: FormGroup=new FormGroup({});
  public OrderFrequncryGroup:string = "" ;
  errorMessage: string;
  scheduleform: FormGroup=new FormGroup({});
  noteform: FormGroup=new FormGroup({});
  private userID: number;
  public RifillFalg:number = 999;
  resOrderForm: FormGroup=new FormGroup({});
  favForm: FormGroup=new FormGroup({});
  public demographicInfoData = {} as DemographicInfo;
  orderholdform: FormGroup=new FormGroup({});
  pageConfig = {};
  cpoepageConfig={};
  public nurseStations: NurseStation[];
  public nurseNotes: any[] = [];
  private nurseStationId: number = 0;
  residents: any[];
  destroyform: FormGroup=new FormGroup({});
  DcForm: FormGroup=new FormGroup({});
  public routes: any[];
  dropdownSettings_Residents: any = {};
  ShowFilter = true;
  public barcodesList: BarcodeEntity[];
  public barcodear: any[] = [];
  public pbarcodear: any[] = [];
  public favouriteMasterList: any[];
  private favouriteObj: OrderFavourite[] = [];
  public frequencyList: FrequencyMasterDataWithShifts[];
  public weeksList: WeekMasterData[];
  public monthsList: MonthMasterData[];
  public hoursList: HoursMasterData[];
  public DirectDiscFlag:boolean = false;
  //public timeFormatList: TimeFormatMasterData[];
  public newOrderFlag: number = 0;
  public ordersInfo: OrderInfoAlert = {} as any;
  public monthDays: any[];
  dropdownSettings_Days: any = {};
  dropdownSettings_Month:any = {};
  dropdownSettings_Week:any = {};
  public favstatus: number = 0;
  public splits: number = 0;
  private orderholdobj: OrderHold;
  destroyObj: OrderDestroy;
  orderStatusObj: CommonOrderStatus;
  orderStatusDCObj: CommonDcOrderStatus;
  reactivateStatus: number = 0;
  orderUpdateObj: Orderupdate;
  nurseNotesObj: NurseComments;
  public quantityId: number;
  public chngeflag : number = 0;
  public selectedNursestation = [];
  dropdownSettings_Nuresestation: any = {};
  public selectedResItem = [];
  public selectedOrder: any;
  public selectedOrderQuantity: any;
  public selectedOrderDADminId: any;

  public drFirstOrderInfo: any[] = [];
  public modalHistoryIsOpen: boolean = false;
  public modalholdIsOpen: boolean = false;
  public modalDesIsOpen: boolean = false;
  public modalfavIsOpen: boolean = false;
  public modalnoteIsOpen: boolean = false;
  public modalInsuliIsOpen: boolean = false;
  public modalHOAIsOpen: boolean = false;
  public modaleMar: boolean = false;
  public drugList;
  hoaObj: HOA;
  DrugName: any;
  public flag: boolean = true;
  public searchTerms = new Subject<string>();
  public stockId: number = 0;
  gpiCode: string = '';
  ordersinsertObj: OrderApproval;
  public dAdminId: number = 0;
  public fav: any;
  public eMARDetails: any[] = [];
  public isReadOnly: boolean = false;
  public active: any;
  public favobj: any[] = [];
  public canceldate: number = 0;
  public acknowledge: number = 0;
  public DrfirstOrderXMLObj: DrfirstOrderXML;
  pendingReviewCount: number;
  public activeFlag: string = "";
  mergeFlag: number = 0;
  public activeTab: string = 'Active';
  selectedDays: any[];
  selectedWeeks:any[];
  selectedMonths:any[];
  isWeeksDisabled:boolean=false;
  isdaysDisabled:boolean=false;
  public hideCheckbox: boolean = false;
  emarform: FormGroup=new FormGroup({});
  public residentAllergies: string;
  public residentDiagnosis: string;
  public drFirstFlag: number = 0;
  reviewFlag: any;
  //drugNameFromPharmacyOrder: string = '';
  modalOption: NgbModalOptions = {};
  defaultPhysicianNPI: any;
  public yearDrop: any[] = [];
  public modalControlIsOpen: boolean = false;
  public timesArray: any[] = [];
  public startTimeArray: number;
  public isHoursReadOnly: boolean = false;
  public times: string = "";
  timeform: FormGroup=new FormGroup({});
  public modalTimeIsOpen: boolean = false;
  public isReadOnlyforControl: boolean = true;
  reviewClickedFlag: number;
  public selectedphyItems = [];
  dropdownSettings_Physician: any = {};
  public selectedroItems = [];
  public refillObj: Refill;
  dropdownSettings_Route: any = {};
  public selectedfrequencyItems = [];
  dropdownSettings_Frequency: any = {};
  public selectedstItems = [];
  dropdownSettings_StartTime: any = {};
  public selectedntItems = [];
  dropdownSettings_NextTime: any = {};
  //previousDrugName: any;
  public patientIdstatus: number = 0;
  drugNameChanged: boolean = false;
  facilityName: any;
  isShiftSchedule: boolean = false;
  public modalDcConfirmationIsOpen: boolean = false;
  notesFlag: number = 0;
  buttonsStatus: number = 0;
  valueChangesFlag: number = 0;
  saveChangeFlag: number = 0;
  medispanControlSubBit: number = 0;
  isControlSubstanceReadOnly: boolean = false;
  isDrugOrder:boolean=false;
  discardConditionalFlag: number = 0;
  discontinueFlag:number =0;
  barCodeFlag:number =0;
  barCodeStatusFlag:number =0;
  public patientTypeList:any[]=[];
  public controlledSubstancemessage:string ='';
  public checkedflag:any;
  public orderOrigin:any;
  public prePId : any;
  public preRId : any;
  public ddlloadflag:number = 0;
  public controlsubstanceBit =0;
  //public ordersgridrowclick=0;
  public prnSchedleCheckModal:boolean=false;
  public btnFlag:number=1;
  public residentAllOrders=[];
  public checkedPorderId:any;
  public checkedPquantityId:any;
  public isHoldChecked:boolean=false;
  public isDcChecked:boolean=false;
  public updateOrdersRecords=[];
  public toolTipText:any='';
  public holdObj:any;
  public dcObj:any;
  public modalDcAllSplits:boolean=false;
  public noDCSplitsFlag=0;
  public re:number=1;
  public modalFieldsChangesDcConfirmation:boolean=false;
  public fieldsChangesDcFlag="";
  public MyImages: any;
  public prnAlert: string;

  public  validateEmptyField(c: FormControl) {
  return c.value && !c.value.trim() ? {
    required: {
      valid: false
    }
  } : null;
}
  public modalBarcodeIsOpen:boolean=false;
  public newResOrderFlag:boolean=false;
  public scheduleTextObj:DrugAdministrationTime;
  public orderHoldData:any;
  public holdReason:string="";
  public orderReleaseDate:any;
  public btnHoaSaveFlag:number=1;
  public holdReleaseDate:any;
  public scheduleSave:number=1;
  public days:number=31;
  public drFirstModal:boolean=false;
  public holdOrdersform: FormGroup=new FormGroup({});
  public multipleOrdersplits=0;
  public selectedOrderDetails:any;
  public selectedOrderHoaDetails:any;
  public FrequnceLimitFlag:any = null;
  public FrequnceLimitAlertText:any = "";
  public FrequnceLimitAlertText1:any = "";
  public reviewButtonDisable:boolean=false;
  public discardChanges:boolean=false;
  public SchFlag:boolean=false;
  public clicked:boolean=false;
  public legend: any[]=[];
  isFeeding:boolean  ;
  isPregnant:boolean  ;
  public weightDetails:any;
  public isWeight:boolean =false;
  public isHeight:boolean =false;
  public getID:number;
  public postData:any;
  public latestDrugNameResponse = [];
  public preventDrugName = '';
  public checkOrderID :any;
  public gpiConst: any;
  public barcodeFacilityId: number = 0;
  rightGridDCChangesFlag: boolean = false;
  rightGridOnHoldChangesFlag: boolean = false;
  profileONHoldChangesCheckState: { [key: string]: boolean } = {};
  profileDCChangesCheckState: { [key: string]: boolean } = {};
  public spinnerLoading: number = 0;
  public writtendate:string= this.dateFormatPipe.dateFormat(new Date());
  public minStartDate: string = this.dateFormatPipe.dateFormat(new Date());
  public nursingStationZoneCurrentDate: any;
  public minDate: Date = new Date();
  public StartOnemonthDate:string = this.dateFormatPipe.dateFormat(new Date());
  public modalEndDateConfirm:boolean = false;
  public nurseValidation:boolean= false;
  public NurseEndFlag:boolean = false;
  public resetCheckbox:boolean = false;
  public literalBarcodeAlert:boolean= false;
  public OverrideBarcodeLiteralreview:boolean= false; 
  @ViewChild('maxperdayFocus') MaxperdayIpFocus : ElementRef;
  public styleMaxperday = {};
  @ViewChild('qtyFocus') qtyIpFocus:ElementRef;
  public qtydisabled :boolean = true;
  public barcodeAlertFlag:boolean = true;
  public autobarcode:string='';
  @ViewChild('nursecomments') nursecomments:ElementRef;
  @ViewChild('insulincomments') insulincomments:ElementRef;

  @ViewChild('discontinueFocus') discontinueFocus:ElementRef;
  @ViewChild('quantityFocus') quantityFocus:ElementRef;
  @ViewChild('holdFocus') holdFocus:ElementRef;
  public userRole: string;
  public adminLoggedin: boolean = false;
   
  //endregion
  constructor(configs: NgbTooltipConfig, private dataservice: DataService, private sharedService: SharedService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private route: Router, private alertService: AlertService,
    private persistanceService: PersistanceService, private sanitizer: DomSanitizer, private dateFormatPipe: CustomdatePipe,
    private modalService: NgbModal , private readonly changeDetectorRef: ChangeDetectorRef) {

  }
  ngOnInit() {

    window.scroll(0,0);

    this.pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
    this.cpoepageConfig = this.persistanceService.getPermissionsByScreen("CPOE");

    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.sharedService.saveChangesFlag.subscribe(res => {
          this.valueChangesFlagReceive = res
        });
        this.discardConditionalFlag = 0;
        this.active = "new";
        this.sharedService.currentOrderId.subscribe(res => this.orderId = res);
        this.sharedService.currentPatientId.subscribe(res => this.residentId = res);
        this.sharedService.currentQuantityId.subscribe(res => this.quantityId = res);
        this.sharedService.currentFacilityId.subscribe(res => this.barcodeFacilityId = res);

        if(this.orderId==0 && this.quantityId==0)
        {
          this.newResOrderFlag=true;
        }
        if (this.residentId != 0) {
          this.pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
          this.cpoepageConfig = this.persistanceService.getPermissionsByScreen("CPOE");
          this.userID = this.persistanceService.get(this.config.loggedInUserKey);
          this.template = this.dataservice.template;
          this.userRole = this.persistanceService.get("userRole");
          if (this.persistanceService.get("userRole") == '\"SuperAdmin\"') {
            this.adminLoggedin = true;
          } else {
            this.adminLoggedin = false
          }
          //this.ng4LoadingSpinnerService.show();
          // this.getNurseCommentNotesByQuantityId();
          //this.getResidentDropData();
          //this.getDemographicInfoData();
          //this.getFavouritesMasterData();
        //  this.loadSearchData();
         // this.onDrugNameChange();
          //this.GetTimeFormatMasterData(); --No need
          //this.GetScheduleTimeDetails();
          //check//this.loadSearchData();
          //this.GetBarcodeData(); --No need
          //this.getDaysDropData();
          this.myform = new FormGroup({
            resName: new FormControl(''),
            resID: new FormControl(''),
            resDOB: new FormControl(''),
            resAdmitDate: new FormControl(''),
            resDischargeDate: new FormControl(''),
            physician: new FormControl('', Validators.required),
            drug: new FormControl('', [Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]),
            dose: new FormControl('', [Validators.required, Validators.min(0.001)]),//, Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)
            route: new FormControl('', Validators.required),
            diagnosis: new FormControl(''),
            addInst: new FormControl('', [Validators.required, Validators.maxLength(250)]),
            startDate: new FormControl('', Validators.required),
            endDate: new FormControl(''),
            qtyHand: new FormControl('', [Validators.maxLength(9), Validators.pattern(this.config.decimalAllowTwoDigits)]),
            refill: new FormControl('', [Validators.maxLength(3), Validators.pattern(this.config.numeric1)]),
            alertText: new FormControl(''),
            maxPerDay: new FormControl('',
            [Validators.maxLength(7), Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
            Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
            Validators.pattern(/^\d*(\.\d{0,3})?$/),
            Validators.pattern(/^\d+(\.\d{0,3})?$/)] ),
            // maxPerDay: new FormControl('',),
            barcode: new FormControl('', [Validators.required, Validators.maxLength(150)]),
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
            orderTypeId: new FormControl(),


            presName: new FormControl(''),
            presID: new FormControl(''),
            presDOB: new FormControl(''),
            presAdmitDate: new FormControl(''),
            presDischargeDate: new FormControl(''),
            pphysician: new FormControl(''),
            pdrug: new FormControl(''),
            pdose: new FormControl(''),
            proute: new FormControl(''),
            pdiagnosis: new FormControl(''),
            paddInst: new FormControl(''),
            pstartDate: new FormControl(''),
            pendDate: new FormControl(''),
            pqtyHand: new FormControl(''),
            prefill: new FormControl(''),
            palertText: new FormControl(''),
            pmaxPerDay: new FormControl(''),
            pbarcode: new FormControl(''),
            pprn: new FormControl(),
            pcontrolSubstance: new FormControl(''),
            pliteral:new FormControl(''),
            pself: new FormControl(''),
            ptreatment: new FormControl(''),
            pmaySub: new FormControl(''),
            ptype: new FormControl(''),
            pnursestationName: new FormControl(),
            pddlresidents: new FormControl(),
            pschduleText: new FormControl(''),
            pinsulincomments: new FormControl(),
            porderTypeId: new FormControl(),

          });
          // console.log(this.myform, "info check form")


          this.RifillFalg = 999;
          // if (this.checkIsFieldsChanged() == 1 || (this.discardChanges)) {
          //   this.sharedService.saveChangesOrderInfo(1);
          // }else{
          //   this.sharedService.saveChangesOrderInfo(0);

          // }
        //  this.RordersList3[0].Refill_Request = null;
        // this.myform.valueChanges
                
        //         .subscribe(value => {
        //           console.log('1')

        //           console.log(value,"value")
        //           if (this.discardConditionalFlag != 1 || (this.discardChanges )) {
        //             console.log('2')

        //             if (this.valueChangesFlag == 1) {
        //               console.log('3')

        //               if (this.checkIsFieldsChanged() == 1 || (this.discardChanges)) {
        //                 this.sharedService.saveChangesOrderInfo(1);
        //                 console.log('1')
        //               } 
        //             }
        //           }
        //         });
        this.myform.valueChanges.subscribe(value => {

          if (this.discardConditionalFlag != 1 || (this.discardChanges || this.rightGridDCChangesFlag || this.rightGridOnHoldChangesFlag || this.checkIsFieldsChanged() == 1 )) {
            if (this.valueChangesFlag == 1) {
              // this.saveChangeFlag = 1;
              if (this.checkIsFieldsChanged() == 1 || (this.discardChanges || this.rightGridDCChangesFlag || this.rightGridOnHoldChangesFlag)) {
                this.sharedService.saveChangesOrderInfo(1);
              } else {
                this.sharedService.saveChangesOrderInfo(0);
              }

              // this.sharedService.saveChangesOrderInfo(this.saveChangeFlag);
            }
          } 
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
          // this.holdOrdersform = new FormGroup({
          //   onholduntil: new FormControl(''),
          // });


          this.noteform = new FormGroup({
            //  notes:new FormControl('',[Validators.required,Validators.maxLength(500), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)])

            notes: new FormControl('', [Validators.required,this.validateEmptyField, Validators.maxLength(500)])
          })
          this.resOrderForm = new FormGroup({
            orderType: new FormControl('', Validators.required),
            orderDate: new FormControl(this.dateFormatPipe.transform(new Date()), Validators.required),
            orderPhysican: new FormControl('', Validators.required),
            orderNote: new FormControl('', Validators.required)
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
            DcReason: new FormControl('', [ Validators.maxLength(500)]),
            dcSplits: new FormControl(false),
            multipledcSplits: new FormControl(false)
          });
          this.destroyform = new FormGroup({
            quantity: new FormControl('', [Validators.required, Validators.maxLength(5), Validators.pattern(this.config.decimalAllowTwoDigits)]),
            reason: new FormControl('', [Validators.required, Validators.maxLength(500), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
            destroyerUsername: new FormControl('', [Validators.required, Validators.maxLength(20) , Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
            destroyerPass: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password), Validators.minLength(4)]),
            approvalUsername: new FormControl('', [Validators.required, Validators.maxLength(20),  Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
            approvalPass: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password), Validators.minLength(4)]),
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
            closeDropDownOnSelection:true,
          };
          this.dropdownSettings_StartTime = {
            singleSelection: true,
            idField: "Hour_Id",
            textField: "Hour_Desc",
            itemsShowLimit: 1,
            allowSearchFilter: this.ShowFilter,
            closeDropDownOnSelection:true,
            noDataAvailablePlaceholderText: 'Please Select Facility'
          };
          this.dropdownSettings_NextTime = {
            singleSelection: true,
            idField: "Hour_Id",
            textField: "Hour_Desc",
            itemsShowLimit: 1,
            closeDropDownOnSelection:true,
            allowSearchFilter: this.ShowFilter,
          };
          this.dropdownSettings_Nuresestation = {
            singleSelection: true,
            idField: "NurseStation_Id",
            textField: "NurseStation_Name",
            itemsShowLimit: 1,
            closeDropDownOnSelection:true,
            allowSearchFilter: this.ShowFilter
          };
          this.dropdownSettings_Residents = {
            singleSelection: true,
            idField: "Patient_Id",
            textField: "PatientName",
            itemsShowLimit: 1,
            closeDropDownOnSelection:true,
            allowSearchFilter: this.ShowFilter
          };
          this.getFacilityNSResidentsDataByPId();




        }
        else
          this.route.navigate(['/home/ordergrid']);
      }
    }

    else
      this.persistanceService.redirectToHomePage();
      // this.myform.valueChanges.subscribe((res) => {
      //   console.log(res,"form");
      //   console.log(this.myform,"myform");

      //   console.log(this.barCodeFlag,"barCodeFlag");

      //   console.log(this.BUttonFlag,"BUttonFlag");
      //   console.log(this.barcodear.length,"barcodear")
      //   console.log(this.newOrderFlag,"newOrderFlag");
      //   console.log(this.discardChanges , "discard")



      // })


  }

  // ngOnChanges(){
  //
  //   if(this.initilazied){

  //   }
  // }
  saveOrder(controlSubstanceFlag: number) {
    this.myform.patchValue({
      controlSubstance:controlSubstanceFlag,
    });
    this.modalControlIsOpen = false;
    if (this.newOrderFlag == 1)
      this.saveNewOrder(controlSubstanceFlag);
    else
      this.saveExistingOrder(controlSubstanceFlag);
  }
  saveNewOrder(controlSubstanceFlag: number) {

    if (this.newOrderFlag == 1) {
      //this.ng4LoadingSpinnerService.show();
      let barcode = this.barcodear.join();
      this.ordersinsertObj = {
        PApprovalOrder_Id: 0,
        Porder_Id: 0,
        Patient_Id: this.residentId,
        OrderingPhysicianID: this.myform.value.physician[0].Physician_Id,
        OrderControl: 'NW',
        OrderTypeID: (this.myform.value.literal==true && this.myform.value.treatment==true)?2:(this.myform.value.literal==true && this.myform.value.treatment==false)?4:(this.myform.value.literal==false && this.myform.value.treatment==true)?5:1,//1,
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
        InsulinComments: this.myform.value.insulincomments,
        RouteCode:this.myform.value.route.length != 0 ? this.myform.value.route[0].Route_Id : '',
        Quantity: this.myform.value.dose,
        StartDate: this.dateFormatPipe.transform(this.myform.value.startDate),
        EndDate: this.myform.value.endDate == "" ? null : this.dateFormatPipe.transform(this.myform.value.endDate),
        RequestedGiveCode: this.gpiCode, //Using this for GPI Code
        GiveCodeText: this.myform.value.drug,
        ProviderAdminDrugInsText: this.myform.value.addInst,
        NumberOfRefills: this.myform.value.refill,
        InHand: this.myform.value.qtyHand,
        POOutBoundFileStatus: 1,
        POOutBoundApproval: null,
        POOutBoundApprovalBy: null,
        POOutBoundApprovalOn: null,
        POrder_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
        AdministrationType: 1,
        NursingFreq_Id:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.freqId,
        NurseShifts_Id: (this.hoaObj==null && this.myform.value.orderTypeId==4)?null:this.hoaObj.nurseShiftId,
        //this.hoaObj.hourId
        Hour_Id:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.hourIds,
        Hours: (this.hoaObj==null && this.myform.value.orderTypeId==4)?null:this.hoaObj.hours,
        Monday: (this.hoaObj==null && this.myform.value.orderTypeId==4)?null:this.hoaObj.monday,
        Tuesday:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.tuesday,
        Wednesday:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.wednesday,
        Thursday:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.thursday,
        Friday:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.friday,
        Saturday:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.saturday,
        Sunday:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.sunday,
        Week_Id:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.weekId,
        Month_Id:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.monthId,
        Barcode: barcode.replace(/\s/g, ""),
        Days:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.days,
        Favourites:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.favobj.join(),
        OnlyOnDay: 0,
        ThroughDay: 0,
        ActiveDays:(this.hoaObj==null && this.myform.value.orderTypeId==4)?null: this.hoaObj.activedays,
        HoldDays: (this.hoaObj==null && this.myform.value.orderTypeId==4)?null:this.hoaObj.holddays,
        ScheduleText:this.myform.value.schduleText,
      };
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_AdminApproval_InsertApprovalOrderData, this.ordersinsertObj)
        .subscribe(res => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          if (res !=0) {
            this.getOrderGridData('Active', 1);
            this.getAllBarcodes();
            this.GetResidentAllOrdersData();
            this.spinnerLoading++;
            this.checkAndHideSpinnerLoading();
            this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
              .subscribe(res => {
                this.spinnerLoading--;
                this.checkAndHideSpinnerLoading();
                if (res == 1) {
                  this.alertService.success("New order saved successfully, awaiting admin approval");
                }
                else {
                  this.alertService.success("New order placed successfully");
                }
              }, error => {
                this.alertService.error(error.message);
                this.spinnerLoading--;
                this.checkAndHideSpinnerLoading();
              });

            this.myform.patchValue({
              physician: '',
              drug: '',
              dose: '',
              route: '',
              addInst: '',
              startDate: '',
              endDate: '',
              barcode: '',
              diagnosis: '',
              type: '1',
              qtyHand: 0.00,
              refill: 0,
              alertText: '',
              maxPerDay: 0,
              schduleText: '',
            });
            this.favobj = [];
            this.favouriteObj = [];
            //this.barcodeCount = 0;
            //this.ng4LoadingSpinnerService..hide();
          }
          // else if (res == 2) {
          //   this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
          //     .subscribe(res => {

          //       if (res == 1) {
          //         this.alertService.error("Can't place duplicate order. An Order with same combination is pending for admin approval.")
          //       }
          //       else {
          //         this.alertService.error("An Order with same Drug name and start date is already placed.")
          //       }
          //     }, error => {
          //       this.alertService.error(error.message)
          //     });
          //   //this.ng4LoadingSpinnerService..hide();
          // }
          else if (res == 0) {
            //this.ng4LoadingSpinnerService..hide();
            this.alertService.error("Something went wrong.")
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          }
        }, error => {
this.BUttonFlag = false;
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });

      this.modalControlIsOpen = false;
    }
    // else
    //   this.saveExistingOrder(controlSubstanceFlag);
  }

  NormalOrder() {
    this.myform.patchValue({
      controlSubstance: false
    });
    this.modalControlIsOpen = false;
  }
  isInteger() {
debugger;
  const value = this.myform.controls.maxPerDay.value;
  if(value !== null && value !== "")
  {
    //const valid = /^[0-9]{1,3}(?:\.[0-9]{1,3})?$/.test(value)
    // it should accept the 0 value (10/28/2025)
    const valid = /^(?:0|[1-9]{1,3}(?:\.[0-9]{1,3})?)$/.test(value);

  const isDotOnly = /^\.$/.test(value);

  this.myform.controls.maxPerDay.setErrors(null);

  if(!valid){
    this.myform.controls.maxPerDay.setErrors({'data' :true})
  }

  if (value.length > 7) {
    this.myform.controls.maxPerDay.setErrors({ 'maxlength': true });
  } 
  //else if (isDotOnly || (value ==0 && this.myform.value.porderTypeId !=4)) {
  // it should accept the 0 value (10/28/2025)
  else if (isDotOnly ) {
    this.myform.controls.maxPerDay.setErrors({ 'dotOnly': true });
  }
  else {
    const integ = String(value);
    if (integ.includes('.')) {
      const parts = integ.split('.');
      const integerPart = parts[0];
      const decimalPart = parts[1];
    
      if (integerPart.length > 3) {
        this.myform.controls.maxPerDay.setErrors({ 'integerError': true });
      } else if (decimalPart && decimalPart.length > 3) {
        this.myform.controls.maxPerDay.setErrors({ 'decimalError': true });
      }
    } 
    else{
      const integ = String(value);
      if(integ.length > 3){
        this.myform.controls.maxPerDay.setErrors({ 'integerError': true });
      }else{
        this.myform.controls.maxPerDay.setErrors(null);
        this.styleMaxperday = {'border-color':'black'};

      }
    }
  }
  }
  if((value === '' || value == null || value == undefined) && this.myform.value.pprn == true ){
    this.myform.controls.maxPerDay.setErrors({ 'dotOnly': true });
  }
  
 }
appendZero(): void {
  //debugger;
  let value = this.myform.controls.maxPerDay.value;

  // Add zero after the dot if user leaves input with integer and dot
  if (/^\d+\.$/.test(value)) {
    this.myform.controls.maxPerDay.setErrors({ 'dotOnly': true });
    value += '0';
    this.myform.controls.maxPerDay.setValue(value);
  }
}
  InsertOrder() {
    this.btnFlag=1;
    if (this.residentId > 0) {
      if (this.demographicInfoData.PVisit_Status == 2) {
        this.alertService.warn("Resident has been discharged.")
      }
      else if (this.demographicInfoData.PVisit_Status == 3) {
        this.alertService.warn("Status updated to temporarily inactive.")
      }
      else {
        if (this.newOrderFlag == 1) {
          //let drugName = this.myform.value.drug;
          //let dose = this.myform.value.dose;
          //let directions = this.myform.value.addInst;
          //let qtyHand = this.myform.value.qtyHand;
          //let refill = this.myform.value.refill;
          //let maxPerDay = this.myform.value.maxPerDay;

          if (this.myform.value.physician == 0 || this.myform.value.physician == null || this.myform.value.physician == undefined)
            this.alertService.error("Please select physician");
          // else if (drugName == '')
          //   this.alertService.error("Please enter Drug name");
          // else if (drugName.length > 60)
          //   this.alertService.error("Drug name must not exceed 60 characters");
          else if (this.stockId == 0 && (this.myform.value.orderTypeId ==1))
            this.alertService.error("Drug name must be selected from search list");
          //Validation for pattern also
          // else if (dose == '')
          //   this.alertService.error("Please enter Quantity/Dose");
          // else if (dose.length > 20)
          //   this.alertService.error("Quantity/Dose must not exceed 20 characters");
          //Validation for pattern also

          else if ((this.myform.value.route == 0 || this.myform.value.route == null || this.myform.value.route == undefined) && this.myform.value.orderTypeId !=4)
            this.alertService.error("Please select route");

          // else if (directions == '')
          //   this.alertService.error("Please enter Directions");
          // else if (directions.length > 150)
          //   this.alertService.error("Directions must not exceed 150 characters");

          else if (this.myform.value.startDate == 0)
            this.alertService.error("Please select start date");
          else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.startDate)
            this.alertService.error("End Date should be greater than Start Date");
          // else if (qtyHand.length > 6)
          //   this.alertService.error("Quantity on Hand must not exceed 3 characters");

          // else if (refill.length > 3)
          //   this.alertService.error("Refill must not exceed 3 characters");

          // else if (maxPerDay.length > 3)
          //   this.alertService.error("MaxPerDay must not exceed 3 characters");

          else if (this.barcodear.length == 0 && this.myform.value.orderTypeId !=4 && this.reviewFlag == 1)
            this.alertService.error("Barcode required");
          else if ((this.hoaObj == null || this.hoaObj == undefined) && this.myform.value.schduleText == '' && this.myform.value.orderTypeId !=4  && this.myform.value.orderTypeId !=2)
            this.alertService.error("Please enter schedule times");
          else if (this.isControlSubstanceReadOnly == false && this.myform.value.controlSubstance == true) {
            this.controlledSubstancemessage='Order requires controlled substance count certification';
            this.checkedflag=true;
            this.modalControlIsOpen = true;
          }
          else {
            let controlsubstancevalue=this.myform.value.controlSubstance==true?1:0;
            this.saveNewOrder(controlsubstancevalue);
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
        ? this.dataservice.post(this.config.Emar_Orders_GetStockQtyonHandData ,{"DrugName":term.replace(/[&\/\\#,+()$~%.'":;*?<>{}]/g, ''),"NurseStationId":this.nurseStationId} )
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
  onselectDrugbyBoth(item:any)
  {
    let orderType = this.myform.value.type == true  ? 'order' : 'stock';
    if(orderType == "stock")
    {
    if (item != '') {
      this.barcodear = [];
      //this.previousDrugName = '';
      this.drugNameChanged = false;
      this.stockId = item.Stock_Id;
      this.gpiCode = item.GPICode;
      if (this.gpiCode == this.gpiConst) {
        if (item.Barcode != "")
          this.barcodear = item.Barcode.split(', ');
        else if(item.Barcode=="")
          this.alertService.warn("No barcode exists for this drug");
        this.myform.patchValue({
          drug: item.DrugName,
          qtyHand: item.InHand,
          //barcode:item.Barcode
          // controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
          controlSubstance: (item.ControlledSubstanceSchedule == 1 || this.controlsubstanceBit == 1) ? true : false,

          type: 0
        });
        this.flag = false;

        if (item.ControlledSubstanceSchedule == 1)
        {
          this.isControlSubstanceReadOnly = true;
          // const maxpervalidation = this.myform.get('maxPerDay');
          // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          // maxpervalidation.updateValueAndValidity();
        }
        else
        {
          this.isControlSubstanceReadOnly = false;
        //   if( this.myform.value.prn !=true)
        //   {
        //   const maxpervalidation = this.myform.get('maxPerDay');
        //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        //   maxpervalidation.updateValueAndValidity();
        //  }
        }
        if (this.barcodear != null && this.barcodear.length > 0) {
          const barcodevalidation = this.myform.get('barcode');
          barcodevalidation.setValidators([Validators.maxLength(150)]);
          //barcodevalidation.clearValidators();
          barcodevalidation.updateValueAndValidity();
          this.barCodeFlag=0;
        }
      }
      else{
        this.modalFieldsChangesDcConfirmation = true;
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
      if (this.gpiCode == this.gpiConst) {
        this.myform.patchValue({
          drug: item.DrugName,
          qtyHand: '',
          //barcode:item.Barcode
          //route: this.selectedroItems,
          controlSubstance: (item.ControlledSubstanceSchedule == 1 || this.controlsubstanceBit == 1) ? true : false,
          type: 1
        });
        this.flag = false;
        this.isReadOnlyforControl = false;
        if (item.ControlledSubstanceSchedule == 1)
        {
          this.isControlSubstanceReadOnly = true;
          // const maxpervalidation = this.myform.get('maxPerDay');
          // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
          // maxpervalidation.updateValueAndValidity();
        }
        else
        {
          this.isControlSubstanceReadOnly = false;
        //   if( this.myform.value.prn !=true)
        //   {
        //   const maxpervalidation = this.myform.get('maxPerDay');
        //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        //   maxpervalidation.updateValueAndValidity();
        //  }
        }
      }
      else{
        this.modalFieldsChangesDcConfirmation = true;
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
      if (item.Barcode != "")
        this.barcodear = item.Barcode.split(', ');
      else if(item.Barcode=="")
        this.alertService.warn("No barcode exists for this drug");
      this.myform.patchValue({
        drug: item.DrugName,
        qtyHand: item.InHand,
        //barcode:item.Barcode
        controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
        type: 0
      });
      this.flag = false;
      if (item.ControlledSubstanceSchedule == 1)
      {
        this.isControlSubstanceReadOnly = true;
        // const maxpervalidation = this.myform.get('maxPerDay');
        // maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
        // maxpervalidation.updateValueAndValidity();
      }
      else
      {
        this.isControlSubstanceReadOnly = false;
      //   if( this.myform.value.prn !=true)
      //   {
      //   const maxpervalidation = this.myform.get('maxPerDay');
      //   maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
      //   maxpervalidation.updateValueAndValidity();
      //  }
      }

      if (this.barcodear != null && this.barcodear.length > 0) {
        const barcodevalidation = this.myform.get('barcode');
        barcodevalidation.setValidators(null);
        barcodevalidation.setValidators([Validators.maxLength(150)]);
        barcodevalidation.updateValueAndValidity();
        this.barCodeFlag=0;
      }
      //this.flag = false;
    }
    else {
      return false;
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
    barcodevalidation.setValidators([Validators.maxLength(150)]);
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
      barcodevalidation.setValidators([Validators.required, Validators.maxLength(150)]);
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
        barcodevalidation.setValidators([Validators.maxLength(150)]);
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
  // checkControlledSub(value:any)
  // {
  //   if (value == true) {
  //     const maxpervalidation = this.myform.get('maxPerDay');
  //     maxpervalidation.setValidators([Validators.required, Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
  //     maxpervalidation.updateValueAndValidity();
  //   }
  //   else if(value == false && this.myform.value.prn !=true)
  //   {
  //     const maxpervalidation = this.myform.get('maxPerDay');
  //     maxpervalidation.setValidators([Validators.maxLength(3), Validators.pattern(this.config.numeric)]);
  //     maxpervalidation.updateValueAndValidity();
  //   }
  // }
  checkTreatment(value:any)
  {
    if (this.newOrderFlag != 1 && value == true && this.myform.value.literal ==false) {
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
      barcodevalidation.setValidators([Validators.required, Validators.maxLength(150)]);
      barcodevalidation.updateValueAndValidity();
     // this.barCodeFlag =1;
       }
      this.myform.patchValue({
        orderTypeId:5,
        //literal:true,
        //drug:''
      });
    }
    else if (this.newOrderFlag != 1 && value == false && this.myform.value.literal ==true) {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.clearValidators();
      drugnameValidations.updateValueAndValidity();
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators([Validators.maxLength(150)]);
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
  checkPRN(value: any) {

    if (value == true) {
      this.prnSchedule();
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.required, Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
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
          this.isShiftSchedule=false;
          this.isHoursReadOnly=false;
          this.selectedstItems=[];
          this.selectedfrequencyItems = [];
          this.scheduleform.patchValue({
            frequency: this.selectedfrequencyItems,
            either: this.selectedstItems,
            hours:'',
          });
        }
      }
      else if (this.newOrderFlag !=1) {
        //this.ng4LoadingSpinnerService.show();
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.dataservice.get<DrugAdministrationTime>(this.config.Emar_Orders_GetScheduleTimeDetails + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId)
          .subscribe(res => {
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            if (res != null) {

              //if (res.NursingFreqId == 1) {
                if((this.frequencyList !=null && this.frequencyList!=undefined) && this.frequencyList.find(f=>f.Frequency_Id==res.NursingFreqId.toString()).Frequency_PRN==1){
                this.alertService.warn('Frequency is set to PRN. Change Frequency in order to change PRN field');
                this.myform.patchValue({
                  prn: true
                });
                const maxpervalidation = this.myform.get('maxPerDay');
                maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
                Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
                Validators.pattern(/^\d*(\.\d{0,3})?$/),
                Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
                maxpervalidation.updateValueAndValidity();
              }
              else {
                this.spinnerLoading++;
                this.checkAndHideSpinnerLoading();
                this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + this.orderId + "/" + this.quantityId + "/" + this.userID)
                  .subscribe(res => {
                    this.spinnerLoading--;
                    this.checkAndHideSpinnerLoading();
                    this.myform.patchValue({
                      schduleText: res.Schedule != "Select Time" ? res.Schedule : '',
                    });
                    this.hoaObj = null;
                  }, error => {
                    this.alertService.error(error.message);
                    this.spinnerLoading--;
                    this.checkAndHideSpinnerLoading();
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

            if(this.myform.value.prn==false && this.newOrderFlag!=1)
            {

              //this.insertScheduleTimes();
              this.btnHoaSaveFlag=1;
            }
            //this.ng4LoadingSpinnerService..hide();
          }, error => {
            this.alertService.error(error.message);
            //this.ng4LoadingSpinnerService..hide();
          });
      }
      // if(this.myform.value.controlSubstance !=true)
      // {
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
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
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.get<FrequencyMasterDataWithShifts[]>(this.config.Emar_Orders_GetFrequencyMasterDataWithShifts + this.nurseStationId)
        .subscribe(res => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          this.frequencyList = res;
          this.GetHoursMasterData();
          this.GetWeekMasterData();
          this.GetMonthMasterData();
          //this.GetScheduleTimeDetails();
        }, error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
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
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_GetScheduledTimeText, this.hoaObj)
        .subscribe(res => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
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

            if(this.myform.value.prn==true && this.newOrderFlag!=1)
            {

              //this.insertScheduleTimes();
              this.btnHoaSaveFlag=2;
            }
          }
        },
          error => {
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            //this.ng4LoadingSpinnerService..hide();
            this.modalHOAIsOpen = false
          });
    }
  }
  hoaOpen() {
    //this.ng4LoadingSpinnerService.show();
    if (this.hoaObj == null)
      this.GetFrequencyMasterData();
    this.modalHOAIsOpen = true;
    //this.ng4LoadingSpinnerService..hide();
  }
  getAllBarcodes() {
    let barcodeFacilityId = this.barcodeFacilityId == null || this.barcodeFacilityId == undefined ? 0 : this.barcodeFacilityId;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllBarcodes + barcodeFacilityId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.barcodesList = res;
        ////this.ng4LoadingSpinnerService..hide();

      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
  }
  onDrugNameChange(drugName?:string) {

    this.stockId = 0;
    this.gpiCode = '';
    this.drugNameChanged = true;
    this.searchDrug(drugName);
  }
  searchDrug(term: string): void {

    if (term.length > 2 && ((this.myform.value.literal==false && this.myform.value.treatment==true)||(this.myform.value.literal==false && this.myform.value.treatment==false))) {
      this.flag = true;
      this.searchTerms.next(term);

    }
    else {
      this.flag = false;
    }
  }

  // getResidentDropData() {
  //   //this.ng4LoadingSpinnerService.show();
  //   let userId = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentDropData + userId)
  //     .subscribe(res => {
  //       this.residents = res;
  //       if (res != null) {
  //         if (this.residentId == 0) {
  //           this.residentId = this.residents[0].Patient_Id;
  //           this.sharedService.changePatientId(this.residentId);
  //         }
  //         this.getNurseStationByPId();
  //         this.GetPhysicianDropData();
  //         this.getOrderRoutes();
  //         this.getAllBarcodes();
  //         this.getDemographicInfoData();
  //         this.selectedResItem = this.residents.filter(item => item.Patient_Id == this.residentId);
  //         this.myform.patchValue({
  //           ddlresidents: this.selectedResItem
  //         });

  //       }
  //     }, error => {
  //       this.alertService.error(error.message);
  //       //this.ng4LoadingSpinnerService..hide();
  //     });
  // }
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
    this.selectedWeeks=[];
    this.scheduleform.patchValue({
      week:this.selectedWeeks,
    })
    this.isWeeksDisabled=true;
    // this.selectedDays = item.User_Id;
    // this.getUserRoleFacilityConfigGridInfo(this.filterIds);
  }
  onDaysDeSelect(item: any) {
    var index = this.selectedDays.indexOf(item);
    this.selectedDays.splice(index, 1);
    this.selectedWeeks=[];
    this.scheduleform.patchValue({
      week:this.selectedWeeks,
    });
    if(this.selectedDays.length==0)
    {
    this.isWeeksDisabled=false;
    }
    // this.filterIds.User_Id = 0;
    // this.getUserRoleFacilityConfigGridInfo(this.filterIds);
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
    if(this.selectedWeeks.length==0)
    {
    this.isdaysDisabled=false;
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
  nurseCommentsSave() {
    let notes=this.noteform.value.notes;
    if(notes!=""){
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
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_InsertUpdateNurseNotes, this.nurseNotesObj)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
        this.noteform.reset();
        this.nurseNotesObj = null;
        // this.alertService.success("Added notes successfully");
        this.nurseValidation= false;
        this.modalnoteIsOpen = false;
        this.modalEndDateConfirm = false;
        this.getNurseCommentNotesByQuantityId();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });
    }
    else
    {
      this.alertService.error("Please enter nurse comment");
      //this.ng4LoadingSpinnerService..hide();
    }
  }
  getNurseCommentNotesByQuantityId() {
    if(this.quantityId!=0)
    {
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetNurseNotes + this.quantityId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.nurseNotes = res;
        this.valueChangesFlagReceive = 0;
        //this.ng4LoadingSpinnerService..hide();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });
    }

  }
  DelayCalling(){
    try{
      this.flag = false;
    this.discardChanges=false;
    this.BUttonFlag = true;
    setTimeout(()=>{
     this.InsertOrder();
    },1000);
  }catch(e)
  {
    this.discardChanges=true;
    this.BUttonFlag = false;
    this.alertService.error(e);
    //this.ng4LoadingSpinnerService..hide();
  }

  }
  saveExistingOrder(controlSubstanceFlag: number) {

    //Srikar
  
    controlSubstanceFlag=this.myform.value.controlSubstance==true?1:0;
    let barcode = this.barcodear.join();
    //if(this.myform.value.prn==true && this.btnHoaSaveFlag==2)
    if(this.myform.value.prn==true && this.newOrderFlag!=1 && this.btnHoaSaveFlag==2)
    {
      this.scheduleSave=2;
      this.insertScheduleTimes(controlSubstanceFlag);
    }
    if(this.newOrderFlag!=1 && this.btnHoaSaveFlag==1 && this.scheduleSave==1)
    {

    if(
    //((this.ordersDetails.WrittenDate==undefined || this.ordersDetails.WrittenDate==null || this.ordersDetails.WrittenDate=="") && this.fieldsChangesDcFlag=="" && this.checkIsFieldsChanged()==1) ||
    (this.fieldsChangesDcFlag=="" && this.checkIsFieldsChanged()==1))
    {
      this.modalFieldsChangesDcConfirmation=true;
    }
    else{
   
    this.orderUpdateObj = {
      PatientId:this.residentId,
      POrderId: this.orderId,
      PQuantityId: this.quantityId,
      PhysicianId: this.myform.value.physician[0].Physician_Id,
      DrugName: this.myform.value.drug,
      // DrugName: this.preventDrugName,

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
      OrderTypeID:(this.myform.value.literal==true && this.myform.value.treatment==true)?2:(this.myform.value.literal==true && this.myform.value.treatment==false)?4:(this.myform.value.literal==false && this.myform.value.treatment==true)?5:1,//this.myform.value.orderTypeId
      OrderUpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
      ScheduleText:this.myform.value.schduleText,
    };
    if(this.NurseEndFlag = true && this.nurseNotesObj != undefined && this.nurseNotesObj != null){
      this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_InsertUpdateNurseNotes, this.nurseNotesObj)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });
    }
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_UpdateOrdersDatabyOrderId, this.orderUpdateObj)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
       if(res == 2){
        this.modalFieldsChangesDcConfirmation=true;
       
       }
       else if(res == 1){
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
              this.getAllBarcodes();
              this.GetResidentAllOrdersData();
              this.alertService.success("Reviewed successfully");
              this.reviewClickedFlag = 0;
              this.OverrideBarcodeLiteralreview=false;

            }, error => {
              //this.ng4LoadingSpinnerService..hide();
              this.alertService.error(this.errorMessage);
            });
        }
        else {
          // if(this.barcodeAlertFlag){
          //   this.alertService.success("Order updated successfully");
          //  }
          this.alertService.success("Order updated successfully");
          this.getOrderGridData('Active', 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
          this.BUttonFlag = false;
      //     setTimeout(()=>{                           //<<<---using ()=> syntax
      //       this.getOrderGridData('Active', 1);

      //       this.BUttonFlag = false;

      //  }, 500);




        }
        this.modalControlIsOpen = false;
     
      }
       }, error => {
        this.BUttonFlag = false;
        //this.ng4LoadingSpinnerService..hide();
        this.alertService.error(this.errorMessage);
        this.modalControlIsOpen = false;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
    }
    this.sharedService.saveChangesOrderInfo(0);
    }
  }
  CompletedReviewClick(reviewflag:number)
  {
    debugger;
    this.reviewButtonDisable=true;
     //this.ng4LoadingSpinnerService.show();
    this.btnFlag=2;
    this.updateOrderReviewClick(reviewflag);
    setTimeout(()=>{
      this.reviewButtonDisable=false;
    },4000);
  }
  updateOrderReviewClick(reviewClicked: number) {

    let prnCheck=this.myform.value.prn;
    let prnScheduleCheck=this.myform.value.schduleText;
    let prnliteral = this.myform.value.literal
    if (this.demographicInfoData.PVisit_Status == 2) {
      this.alertService.warn("Resident has been discharged.")
    }
    else if (this.demographicInfoData.PVisit_Status == 3) {
      this.alertService.warn("Status updated to temporarily inactive.")
    }
    else {
      this.reviewClickedFlag = reviewClicked;
      let drugName = this.myform.value.drug == null ? '' : this.myform.value.drug;

      if (this.myform.value.physician == 0 || this.myform.value.physician == null || this.myform.value.physician == undefined)
        this.alertService.error("Please select physician");

      else if (this.stockId == 0 && this.myform.value.type == false)
        this.alertService.warn("Drug name doesn't exist in stock");
      else if (this.drugNameChanged == true && this.myform.value.orderTypeId ==1)
        this.alertService.error("Please select drug name from the list");


      else if (this.myform.value.orderTypeId !=4 && (this.myform.value.route == 0 || this.myform.value.route == null || this.myform.value.route == undefined))
        this.alertService.error("Please select route");


      else if (this.myform.value.startDate == 0)
        this.alertService.error("Please select start date");
      else if (this.myform.value.endDate != 0 && this.myform.value.endDate < this.myform.value.startDate)
        this.alertService.error("End Date should be greater than Start Date");

      else if (this.barcodear == null && this.myform.value.orderTypeId != 4 && this.btnFlag == 2 && this.reviewFlag == 1)
        this.alertService.error("Barcode required");
      else if (this.barcodear.length == 0 && this.myform.value.orderTypeId != 4 && this.btnFlag == 2 && this.reviewFlag == 1)
        this.alertService.error("Barcode required");
        // else if (this.barcodear.length == 0 && this.myform.value.orderTypeId == 4 && this.OverrideBarcodeLiteralreview == false)
        // this.literalBarcodeAlert = true;
      else if ((this.hoaObj == null || this.hoaObj == undefined) && (this.myform.value.schduleText == '' || this.myform.value.schduleText == null) && this.myform.value.orderTypeId != 4  && this.myform.value.orderTypeId !=2)
        this.alertService.error("Please enter schedule times");
      //ToDo: Anitha - Confirm with client whether HL7 order can be modified as Control Substance or not
      else if (this.medispanControlSubBit == 0 && this.controlsubstanceBit == 1 && this.orderOrigin == 'P') {
        //Srikar
        if(this.myform.value.controlSubstance == false){
          this.controlledSubstancemessage ='You have unchecked controlled substance checbox. Are you sure this is not a controlled med?';
          this.checkedflag=false;
          this.modalControlIsOpen = true;
        }
        else  {

          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
           if(prnliteral==true){
            this.prnSchedleCheckModal = true;
            this.prnAlert="Are you sure you want to Complete Review this Literal PRN Without HOA?"
           }else{
            this.prnSchedleCheckModal = true;
            this.prnAlert='Are you sure you want to Complete Review this PRN Without HOA?'
           }
        }
        else
          this.saveExistingOrder(1);
        }
      }
      else if(this.medispanControlSubBit == 0 && this.controlsubstanceBit == 0 && this.orderOrigin == 'P')
      {
        if(this.myform.value.controlSubstance == true){
          this.controlledSubstancemessage ='Order requires controlled substance count certification';
          this.checkedflag=true;
          this.modalControlIsOpen = true;
        }
        else  {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
           if(prnliteral==true){
            this.prnSchedleCheckModal = true;
            this.prnAlert="Are you sure you want to Complete Review this Literal PRN Without HOA?"
           }else{
            this.prnSchedleCheckModal = true;
            this.prnAlert='Are you sure you want to Complete Review this PRN Without HOA?'
           }
        }
        else
          this.saveExistingOrder(0);
        }
      }
      else if(this.medispanControlSubBit == 0 && this.controlsubstanceBit == 1 && this.orderOrigin == 'M')
      {
        if(this.myform.value.controlSubstance == false){
          this.controlledSubstancemessage ='You have unchecked controlled substance checbox. Are you sure this is not a controlled med?';
          this.checkedflag=false;
          this.modalControlIsOpen = true;
        }
        else  {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
           if(prnliteral==true){
            this.prnSchedleCheckModal = true;
            this.prnAlert="Are you sure you want to Complete Review this Literal PRN Without HOA?"
           }else{
            this.prnSchedleCheckModal = true;
            this.prnAlert='Are you sure you want to Complete Review this PRN Without HOA?'
           }
        }
        else
          this.saveExistingOrder(1);
        }
      }
      else if(this.medispanControlSubBit == 0 && this.controlsubstanceBit == 0 && this.orderOrigin == 'M')
      {
        if(this.myform.value.controlSubstance == true){
          this.controlledSubstancemessage ='Order requires controlled substance count certification';
          this.checkedflag=true;
          this.modalControlIsOpen = true;
        }
        else  {
          if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
          if(prnliteral==true){
            this.prnSchedleCheckModal = true;
            this.prnAlert="Are you sure you want to Complete Review this Literal PRN Without HOA?"
           }else{
            this.prnSchedleCheckModal = true;
            this.prnAlert='Are you sure you want to Complete Review this PRN Without HOA?'
           }
        }
        else
          this.saveExistingOrder(0);
        }
      }
      else  {
        if(prnCheck==true && prnScheduleCheck=="PRN - As Needed" && this.btnFlag==2)
        {
           if(prnliteral==true){
            this.prnSchedleCheckModal = true;
            this.prnAlert="Are you sure you want to Complete Review this Literal PRN Without HOA?"
           }else{
            this.prnSchedleCheckModal = true;
            this.prnAlert='Are you sure you want to Complete Review this PRN Without HOA?'
           }
        }
        else
        this.saveExistingOrder(0);
      }
    }
    //this.ng4LoadingSpinnerService..hide();
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
  insulinOpen() {
    this.modalInsuliIsOpen = true;
    setTimeout(() => {
      this.insulincomments.nativeElement.focus()
    }, 300);
  }
  // getNurseStations() {
  //   let userId = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserNurseStations + userId)
  //     .subscribe(res => {
  //       this.dropdownSettings_Nuresestation = {
  //         singleSelection: true,
  //         idField: "NurseStation_Id",
  //         textField: "NurseStation_Name",
  //         itemsShowLimit: 1,
  //         allowSearchFilter: this.ShowFilter
  //       };
  //       this.nurseStations = res;
  //       this.nurseStationId = this.nurseStations[0].NurseStation_Id;
  //     },
  //       error => {
  //         this.alertService.error(error.message);
  //       });
  // }
  getFacilityNSResidentsDataByPId() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
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
        this.getAllFlagsForCompanyByNSId(this.nurseStationId,this.residentId);
        this.GetPhysicianDropData(this.nurseStationId);
        this.getOrderRoutes();
        this.getAllBarcodes();
        this.getDemographicInfoData();

        this.loadSearchData();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        ////this.ng4LoadingSpinnerService..hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
  }
  getDiagnosisDetails() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<OrderInfoAlert>(this.config.Emar_Orders_GetDiagnosisDetails + this.residentId)
      .subscribe(res => {
        this.ordersInfo = {
          Allergy: res.Allergy,
          Diet: res.Diet,
          Diagnosis: res.Diagnosis,
        };
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
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
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });
  }
  getDemographicInfoData() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.demographicInfoData = res;
        this.getPatientType();
        this.getDiagnosisDetails();
        this.getID =res.Patient_Id;
        this.getWeightHeight();
        if(this.orderId==0 && this.quantityId==0 && this.newResOrderFlag==true)
        {
          this.orderGridData=[];
          this.getFavouritesMasterData();
          this.newOrder();
          this.getOrderGridData("Active", 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
        }
        else
        {
        this.getOrderGridData("Active", 1);
        this.getAllBarcodes();
        this.GetResidentAllOrdersData();
        }
        this.getDrFirstOrderCheck();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
  }
  orderHoldDetailsCancel() {
    this.orderholdform.reset();
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    this.holdReleaseDate="";
    this.closeModel();
    this.myform.enable();
  }
  closeNotesModel()
  {
    this.noteform.reset();
    this.closeModel();
  }

  GetPhysicianDropData(nsId: number) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + nsId+"/"+this.orderId)
      .subscribe(res => {
        this.physiciansdrop = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }

  //#region
  //   created by:sampath
  getActiveDrFirst() {
    this.active = "Verified";
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetDrFirstOrderCheck + this.residentId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.drFirstOrderInfo = [];
        this.drFirstOrderInfo = res.filter(item => item.DrFirstOrderXMLTrans_Approval == 1);
        if (res.filter(item => item.DrFirstOrderXMLTrans_Approval == 1)) {
          this.canceldate = 0;
          this.acknowledge = 0;
        }
        //this.ng4LoadingSpinnerService..hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });

  }
  getVoidDrFirst() {
    this.active = "void";
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetDrFirstOrderCheck + this.residentId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.drFirstOrderInfo = [];
        this.drFirstOrderInfo = res.filter(item => item.DrFirstOrderXMLTrans_Approval == 1 && item.status == 1);
        if (res.filter(item => item.DrFirstOrderXMLTrans_Approval == 1 && item.status == 1)) {
          this.canceldate = 1;
          this.acknowledge = 0;
        }
        // this.getHL7DrFirstOrderCheck(this.residentID);
        //this.ng4LoadingSpinnerService..hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
          // this.MapButton = 0;
        });
  }
  getDrFirstOrderCheck() {
    //this.ng4LoadingSpinnerService.show();
    this.active = "new";
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetDrFirstOrderCheck + this.residentId)
      .subscribe(res => {
        this.drFirstOrderInfo = [];
        this.drFirstOrderInfo = res.filter(item => item.DrFirstOrderXMLTrans_Approval == null);
        if (res.filter(item => item.DrFirstOrderXMLTrans_Approval == (0 || null) && item.status == 2)) {
          this.canceldate = 0;
          this.acknowledge = 1;
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        // this.getHL7DrFirstOrderCheck(this.residentID);
        //this.ng4LoadingSpinnerService..hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
          // this.MapButton = 0;
        });
  }
  reactivateOrder() {

    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive=0;
          this.orderStatusObj = {
            PatientId: this.residentId,
            OrderId: this.orderId,
            DAdminId: this.dAdminId,
            QuantityId: this.quantityId,
            POrderCreatedBy: this.userID,
            POrderStatus: 1,
            POOutBoundApproval: 0,
            POOutBoundApprovalBy: this.userID,
            OrderType: 'Reactivate',
            UpdatedOn: this.dateFormatPipe.dateWithTime(new Date())
          };
          this.funFavOrderChange();
          this.spinnerLoading++;
          this.checkAndHideSpinnerLoading();
          this.dataservice.post(this.config.Emar_Orders_InsertDiscountiueStatus, this.orderStatusObj)
            .subscribe(res => {
              this.getOrderGridData("Active", 1);
              this.getAllBarcodes();
              this.GetResidentAllOrdersData();
              if (res == 1) {


                this.alertService.success("Order reactivated successfully");
              }
              else {
                this.alertService.error("Something went wrong");
              }
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
            }, error => {
              //this.ng4LoadingSpinnerService..hide();
              this.alertService.error(this.errorMessage);
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
            });
        }
        modalRef.close();
      });
    }
    else {
      this.orderStatusObj = {
        PatientId: this.residentId,
        OrderId: this.orderId,
        DAdminId: this.dAdminId,
        QuantityId: this.quantityId,
        POrderCreatedBy: this.userID,
        POrderStatus: 1,
        POOutBoundApproval: 0,
        POOutBoundApprovalBy: this.userID,
        OrderType: 'Reactivate',
        UpdatedOn: this.dateFormatPipe.dateWithTime(new Date())
      };

      this.funFavOrderChange();
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_InsertDiscountiueStatus, this.orderStatusObj)
        .subscribe(res => {
          this.getOrderGridData("Active", 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
          if (res == 1) {

            this.alertService.success("Order reactivated successfully");
          }
          else {
            this.alertService.error("Something went wrong");
          }
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        }, error => {
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.error(this.errorMessage);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
    }

  }
  orderDiscontinueopen() {
    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive = 0;
          this.isDcChecked=false;
          this.fieldsChangesDcFlag="";
          this.modalHistoryIsOpen = true;
        }
        modalRef.close();
      });
    }
    else {
      this.DirectDiscFlag = true;
      this.isDcChecked=false;
      this.fieldsChangesDcFlag="";
      this.modalHistoryIsOpen = true;
    }

  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.modalholdIsOpen = false;
    this.modalDesIsOpen = false;
    this.modalfavIsOpen = false;
    this.modalInsuliIsOpen = false;
    this.modalHOAIsOpen = false;
    this.modaleMar = false;
    this.modalnoteIsOpen = false;
    this.DirectDiscFlag = false;
    this.modalnoteIsOpen = false;
    this.modalEndDateConfirm = false;
    this.nurseValidation = false;
    this.literalBarcodeAlert = false;
    this.forChange();
  }
  closeDcModel() {
    this.modalDcConfirmationIsOpen = false;
    this.modalHistoryIsOpen = false;
    this.DirectDiscFlag = false;
    this.BUttonFlag = false;
    this.DcForm.reset();
    this.rightGridDCChangesFlag = false;
    this.myform.enable();
    this.rightGridOnHoldChangesFlag = false;
    this.profileDCChangesCheckState={};
    this.profileONHoldChangesCheckState={};
  }
  cancelHOAChanges() {
    //this.GetScheduleTimeDetails();
    this.modalHOAIsOpen = false;
    this.OrderFrequncryGroup = "";
  }
  orderDiscontinueConfirmation() {
    this.isDcChecked=false;
    this.modalDcConfirmationIsOpen = true;
    setTimeout(() => {
      this.discontinueFocus.nativeElement.focus()
    }, 400);
    this.modalHistoryIsOpen = false;
  }
  orderDiscontinue() {
    // console.log("dc pop up clicked 2")
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
      DiscontinueFlag:  this.DirectDiscFlag == true ? 1 : 6,
      DiscontinueReason: this.DcForm.value.DcReason,
      DiscontinueAllSplits: this.DcForm.value.dcSplits == true ? 1 : 0,
      DiscontinuedOn: this.dateFormatPipe.dateWithTime(new Date()),
      Split: this.splits
    };
    // console.log(  "dc pop up clicked 2 end")

    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    // console.log(this.orderStatusDCObj , "dc pop up clicked 2")
    this.dataservice.post(this.config.Emar_AdminApproval_DiscontinueOrder, this.orderStatusDCObj).subscribe(res => {
        this.getOrderGridData("Active", 1);
        this.getAllBarcodes();
        this.GetResidentAllOrdersData();
        this.DcForm.reset();
        this.fieldsChangesDcFlag="";
        // console.log("dc ins")

        if (res !=0) {
          this.modalHistoryIsOpen = false;
          this.modalDcConfirmationIsOpen = false;
          this.BUttonFlag = false;
          this.alertService.success("Order discontinued successfully");
        this.DirectDiscFlag = false;
        }
        else {
          this.modalHistoryIsOpen = false;
          this.modalDcConfirmationIsOpen = false;
          this.BUttonFlag = false;
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.error("Something went wrong");
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, 
      error => {
        this.modalHistoryIsOpen = false;
        this.modalDcConfirmationIsOpen = false;
        //this.ng4LoadingSpinnerService..hide();
        this.alertService.success(this.errorMessage);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
      // console.log("dc out")
  }

  destroyStatus() {
    this.orderStatusObj = {
      PatientId: this.residentId,
      OrderId: this.orderId,
      DAdminId: this.dAdminId,
      QuantityId: this.quantityId,
      POrderCreatedBy: this.userID,
      POrderStatus: 2,
      POOutBoundApproval: 0,
      POOutBoundApprovalBy: this.userID,
      OrderType: 'Destroy',
      UpdatedOn: this.dateFormatPipe.dateWithTime(new Date())
    };
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_InsertDiscountiueStatus, this.orderStatusObj)
      .subscribe(res => {
        this.getOrderGridData("Active", 1);
        this.getAllBarcodes();
        this.GetResidentAllOrdersData();
        this.modalDesIsOpen = false;
        this.alertService.success("Specified quantity was documented for destruction successfully");
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.modalDesIsOpen = false;
        //this.ng4LoadingSpinnerService..hide();
        this.alertService.error(this.errorMessage);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });

  }
  holdOrder() {
    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive=0;
          this.isHoldChecked=false;
          this.modalholdIsOpen = true;
          setTimeout(() => {
            this.holdFocus.nativeElement.focus()
          }, 300);
          this.discardChanges = false;
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          this.updateOrdersRecords = [];
          this.myform.enable();
          this.resetCheckbox = false;
        }
        modalRef.close();
      });
    }
    else {
      this.holdReleaseDate="";
      this.isHoldChecked=false;
      this.modalholdIsOpen = true;
      setTimeout(() => {
        this.holdFocus.nativeElement.focus()
      }, 300);
    }
  }
  getOrderStockDetailsByOrderId() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_GetOrderStock + this.orderId)
      .subscribe(res => {
        this.destroyform.patchValue({
          //quantity: res.Remaining
        });
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        //this.ng4LoadingSpinnerService..hide();
        this.alertService.error(this.errorMessage);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  //orderId: number, dAdminId: number
  orderReview() {
    let drugName = this.myform.value.drug == null ? '' : this.myform.value.drug;
    //let dose = this.myform.value.dose == null ? '' : this.myform.value.dose;
    //let directions = this.myform.value.addInst;
    //let qtyHand = this.myform.value.qtyHand == null ? '' : this.myform.value.qtyHand;
    // let refill = this.myform.value.refill == null ? '' : this.myform.value.refill;
    // let maxPerDay = this.myform.value.maxPerDay == null ? '' : this.myform.value.maxPerDay;

    if (this.myform.value.physician == 0 || this.myform.value.physician == undefined || this.myform.value.physician == null)
      this.alertService.error("Please select physician");
    else if (drugName == '')
      this.alertService.error("Please enter Drug name");
    else if (drugName.length > 120)
      this.alertService.error("Drug name must not exceed 120 characters");
    // else if (this.drugNameFromPharmacyOrder != '' && this.drugNameFromPharmacyOrder != drugName && this.stockId == 0)
    //   this.alertService.error("Please select drug name from the list");
    else if (this.drugNameChanged == true && this.myform.value.orderTypeId == 1)
      this.alertService.error("Please select drug name from the list");
    //Validation for pattern also
    // else if (dose == '')
    //   this.alertService.error("Please enter Quantity/Dose");
    // else if (dose.length > 20)
    //   this.alertService.error("Quantity/Dose must not exceed 20 characters");
    //Validation for pattern also

    else if (this.myform.value.route == 0 || this.myform.value.route == null || this.myform.value.route == undefined)
      this.alertService.error("Please select route");

    // else if (directions == '')
    //   this.alertService.error("Please enter Directions");
    // else if (directions.length > 150)
    //   this.alertService.error("Directions must not exceed 150 characters");

    else if (this.myform.value.startDate == 0)
      this.alertService.error("Please select start date");

    // else if (qtyHand.length > 6)
    //   this.alertService.error("Quantity on Hand must not exceed 3 characters");

    // else if (refill.length > 3)
    //   this.alertService.error("Refill must not exceed 3 characters");

    // else if (maxPerDay.length > 3)
    //   this.alertService.error("MaxPerDay must not exceed 3 characters");
    else if (this.barcodear == null)
      this.alertService.error("Barcode required");
    else if (this.barcodear.length == 0)
      this.alertService.error("Barcode required");
    else if ((this.hoaObj == null || this.hoaObj == undefined) && (this.myform.value.schduleText == '' || this.myform.value.schduleText == null)) {
      this.alertService.error("Please enter schedule times");
    }
    else {
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
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_InsertDiscountiueStatus, this.orderStatusObj)
        .subscribe(res => {
          this.getOrderGridData("Active", 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
          this.alertService.success("Reviewed successfully");
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
         // this.getEmarOrdersList3();
        }, error => {
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.error(this.errorMessage);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
    }
  }
  releasefromholdStatus() {
    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive=0;
          this.discardChanges = false;
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          this.updateOrdersRecords = [];
          this.myform.enable();
          this.resetCheckbox = false;
          this.orderholdobj = {
            PQuantity_Id: this.quantityId,
            OrderHold_CreatedBy: this.userID,
            OrderHold_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
            OrderHold_Id: 0,
            OrderHold_Status: 2,
            HoldFrom: '',
            HoldTo: '',
            HoldReason: ''
          };
          this.spinnerLoading++;
          this.checkAndHideSpinnerLoading();
          this.dataservice.post(this.config.Emar_Orders_UpdateOrderHoldStatus, this.orderholdobj)
            .subscribe(res => {
              this.getOrderGridData("Active", 1);
              this.getAllBarcodes();
              this.GetResidentAllOrdersData();
              this.alertService.success("Hold order released successfully");
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
            }, error => {
              //this.ng4LoadingSpinnerService..hide();
              this.alertService.error(this.errorMessage);
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
            });
        }
        modalRef.close();
      });
    }
    else {
      this.orderholdobj = {
        PQuantity_Id: this.quantityId,
        OrderHold_CreatedBy: this.userID,
        OrderHold_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
        OrderHold_Id: 0,
        OrderHold_Status: 2,
        HoldFrom: '',
        HoldTo: '',
        HoldReason: ''
      };
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_UpdateOrderHoldStatus, this.orderholdobj)
        .subscribe(res => {
          this.getOrderGridData("Active", 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
          this.alertService.success("Hold order released successfully");
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        }, error => {
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.error(this.errorMessage);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
    }

  }
  // GetBarcodeData() {
  //   //this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<BarcodeDetail[]>(this.config.Emar_Orders_GetBarcodeData + this.orderId)
  //     .subscribe(res => {
  //       this.barcodeList = res;
  //       this.barcodeCount = res.length;
  //       //this.ng4LoadingSpinnerService..hide();
  //     }, error => {
  //       //this.ng4LoadingSpinnerService..hide();
  //       this.alertService.error(this.errorMessage);
  //     });
  // }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }

  addBarcode(value: any) {
    debugger;
    let BarcodeGPialert = 0;
    // console.log(value, "value");
    let gpiCode = this.gpiConst == '' || this.gpiConst == null ? '0' : this.gpiConst ;
    
    if (this.barCodeStatusFlag != 1 && this.myform.value.orderTypeId != 4) {
      this.barCodeFlag = 1;
    }
    //if(this.myform.value.orderTypeId != 4 && gpiCode !='') {
      this.spinnerLoading++;
      let barcodeLatesVal = value;
      let barcodeLatesvalue = (barcodeLatesVal.split('/'))[0];
      this.checkAndHideSpinnerLoading();
      this.dataservice.get(this.config.Emar_Orders_CheckBarcodeAlert + barcodeLatesvalue + '/' + gpiCode +'/' + this.selectedResItem[0].Patient_Id + '/' + this.barcodeFacilityId + '/' + this.orderId)
      .subscribe(res => {
      //  console.log(res,"resgpicode");
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();
       BarcodeGPialert = Number(res);
       if(BarcodeGPialert == 1){
        this.alertService.warn("This barcode is associated with another drug or with an order for another patient");
        this.myform.patchValue({
          barcode: ''
        });
      }
      else{
        if (value != '') {
          let barcode = this.myform.value.barcode;
          let barcodevalue = (barcode.split('/'))[0];
            if (barcodevalue != '') {
              if (this.myform.value.type == 1) {
                
                let result1 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() == gpiCode.toLowerCase() : false));
                let result2 = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false);
                let result3 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() == gpiCode.toLowerCase() : false) && (x.Patient_Id != null ? x.Patient_Id != this.residentId : false));
               
                let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;
                if ((result2.length > 0 && result1.length == 0) || result3.length > 0 || checkInBarcodeArray != undefined) {
                  if ((this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() != gpiCode.toLowerCase() : false) && x.Patient_Id != null ? x.Patient_Id == this.residentId : false).length > 0)) {
                    this.alertService.warn("This barcode is assigned to another item/drug");
                  }
                  else if (checkInBarcodeArray != undefined) {
                    this.alertService.warn("Barcode already assigned to the same medication for the same resident");
                  }
                  else {
                    this.alertService.warn("This barcode is assigned to an item/drug for another patient");
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
      
                  this.barCodeChange();
      
      
                }
              }
              else {
                if (this.gpiCode != '') {
                  let result1 = this.barcodesList.filter(x => (x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false) && (x.GPICode != null ? x.GPICode.toLowerCase() == gpiCode.toLowerCase() : false));
                  let result2 = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.toLowerCase() === barcodevalue.toLowerCase() : false);
                  let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;
                  if ((result2.length > 0 && result1.length == 0)) {
                    this.alertService.warn("This barcode is assigned to another item/drug");
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
                    this.barCodeChange()
                  }
                }
                else {
                  this.alertService.error("Please select drug");
                }
              }
            }         
        }
      }
      if (this.barcodear != null && this.barcodear.length != 0) {
        if (this.barCodeStatusFlag != 1) {
          this.barCodeFlag = 0;
        }
  
        const barcodevalidation = this.myform.get('barcode');
        barcodevalidation.setValidators([Validators.maxLength(150)]);
        barcodevalidation.updateValueAndValidity();
  
      }
      })
  
    // } else {
    //   let barcode = this.myform.value.barcode;
    //   let barcodevalue = (barcode.split('/'))[0];
    //   this.barcodear.push(barcodevalue);
    //   this.barCodeFlag = 0;
    //   this.myform.patchValue({
    //     barcode: '',
    //   })
    //   this.barCodeChange();
   

    // }
   
  }
  removeBarcode(i: number) {

    this.barcodear.splice(i, 1);
    if (this.barcodear.length == 0 && this.myform.value.orderTypeId!=4) {
      if(this.barCodeStatusFlag !=1)
      {
      this.barCodeFlag = 1;
      }
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators([Validators.required]);
      barcodevalidation.updateValueAndValidity();


    }
    this.barCodeChange();
    if (this.reviewFlag == 1 && this.barcodear.length == 0) {
      this.alertService.warn("Minimum one barcode is required");
    }
  }
  getOrderGridData(status: string, tabClick: number, discardChanges?: number) {
     ;
    //this.ng4LoadingSpinnerService.show();
    this.sharedService.saveChangesOrderInfo(0);
    this.valueChangesFlag = 0;
    if (discardChanges == 1) {
      this.discardConditionalFlag = 1;
    }

    this.activeTab = status;
    this.notesFlag = 0;
    // if (tabClick == 0)
    //   this.orderId = 0;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderGridData + this.residentId + "/" + status)
      .subscribe(res => {
        this.orderGridData = res;
        this.GetResidentAllOrdersData();
        if (res.length > 0) {
          this.pendingReviewCount = res.filter(item => item.ReviewFlag != 1).length;

          this.reactivateStatus = 0;
          if (this.orderId == 0) {
            if (this.orderGridData.length > 0) {
              this.orderId = this.orderGridData[0].porder_Id;
              this.sharedService.changeOrderId(this.orderId);
              this.sharedService.changeQuantityId(this.orderGridData[0].PQuantity_Id);
              if(this.orderGridData[0].DAdmin_Id !=null && this.orderGridData[0].DAdmin_Id !=undefined){
              this.dAdminId = this.orderGridData[0].DAdmin_Id;
            }
              this.getOrderDetailsbyorderId(this.orderId, this.orderGridData[0].PQuantity_Id, this.dAdminId);

            }
          }
          else if (this.orderId != 0) {
         let records = (res!=null && res!=undefined) ? res.find(item => item.porder_Id == this.orderId && item.PQuantity_Id == this.quantityId):null;
          if(records !=null && records !=undefined)
          {
            this.dAdminId =(res!=null && res!=undefined) ? res.find(item => item.porder_Id == this.orderId && item.PQuantity_Id == this.quantityId).DAdmin_Id:0;
           
            this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);

          }
          else if(records==undefined)
          {
            if (this.orderGridData.length > 0) {
              this.orderId = this.orderGridData[0].porder_Id;
              this.sharedService.changeOrderId(this.orderId);
              this.sharedService.changeQuantityId(this.orderGridData[0].PQuantity_Id);
              if(this.orderGridData[0].DAdmin_Id !=null && this.orderGridData[0].DAdmin_Id !=undefined){
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
        // this.getAllBarcodes();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      }, error => {
        this.BUttonFlag = false;
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });

  }
  insertRefill(item: any) {

    //this.ng4LoadingSpinnerService.show();
    //this.clk = true;
    this.refillObj = {
      Refill_Id: 0,
      Porder_Id: item.porder_Id,
      Patient_Id: this.demographicInfoData.Patient_Id,
      NumberOfRefillsRemaining: item.Refill,
      Refill_Status: 1,
      Refill_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Refill_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
      POOutBoundFileStatus: 1,
      POOutBoundApproval: null,
      POOutBoundApprovalBy: null,
      POOutBoundApprovalOn: null,

    };
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_AdminApproval_InsertRefill, this.refillObj)
      .subscribe(res => {

        if (res == 1)
        {
         //this.clicked = false;
          this.alertService.success("Refill request sent to pharmacy");
         // this.getEmarOrdersList3();
         this.getOrderGridData("Active", 1);
         this.getAllBarcodes();
         this.GetResidentAllOrdersData();
        // this.getOrderDetailsbyorderId(this.orderId, this.quantityId,this.dAdminId);

          if(res == 2)
          {
            //this.clicked = false;
            this.getOrderGridData("Active", 1);
            this.getAllBarcodes();
            this.GetResidentAllOrdersData();
           // this.getOrderDetailsbyorderId(this.orderId, this.quantityId,this.dAdminId);

          }
          if(res == 3)
          {
            //this.clicked = false;
            this.getOrderGridData("Active", 1);
            this.getAllBarcodes();
            this.GetResidentAllOrdersData();
          //  this.getOrderDetailsbyorderId(this.orderId, this.quantityId,this.dAdminId);

          }
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });

  }
  rejectedRefill(item: any) {
    const modalRef = this.modalService.open(RejectedrefillsComponent);
    // modalRef.componentInstance.title = 'Merge Orders';
    // let mergeData = {
    //   "selectedOrderId": this.orderId,
    //   "selectedPatientId": this.residentId
    // }
    // modalRef.componentInstance.mergeChanges = mergeData;
    modalRef.componentInstance.result.subscribe((receivedResult) => {
      modalRef.close();
    })
  }
//   getEmarOrdersList3() {

//    // let passtime = this.passTime != null ? this.passTime.replace(':', '-') : null;
//     //   let time = this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime == null || this.myform.value.nurseSheduleTime == undefined ? null : this.myform.value.nurseSheduleTime;
//     // time = this.myform.value.nurseSheduleTime == null ? null : (this.myform.value.nurseSheduleTime.length == 0 ? null : this.myform.value.nurseSheduleTime[0].replace(':', '-'));
//     //this.passTime = time;
// //let shift = this.myform.value.nsShiftTime == 0 ? 0 : this.myform.value.nsShiftTime[0].NurseShifts_Id;
//    // let showTwoHours = this.myform.value.twoHrsWindow == true ? 1 : 0;
//    let userId = this.persistanceService.get(this.config.loggedInUserKey);
//    var Datesss = this.myform.value.startDate == "" ? null : this.myform.value.startDate ;
//     this.dataservice.get<EmarOrdersList[]>(this.config.Emar_Emar_GetEmarOrdersList + this.residentId + "/" + null + "/" + Datesss + "/" + 0 + "/" + 0+"/"+ userId)
//       .subscribe(res => {

//       if(res.length != 0)
//       {


//           this.RordersList3 = res.filter(U=>U.Drug == this.myform.value.drug && U.POrder_Id == this.orderId && U.pquantity_Id == this.quantityId);
//           if(this.RordersList3.length != 0)
//           {


//           this.RifillFalg = this.RordersList3[0].Refill_Request ;
//           }
//           else
//           {
//             this.RifillFalg = 999;
//           }
//      //   this.getOrderDetailsbyorderId(this.orderId, this.quantityId,this.dAdminId)
//         //this.ng4LoadingSpinnerService..hide();
//       }



//       }, error => {
//         //this.ng4LoadingSpinnerService..hide();
//         this.alertService.error(error.message);
//       });
//     // }
//     // else if (this.myform.value.nsShiftTime != 0) {
//     //   this.alertService.warn('Development In Progress');
//     // }

//   }
getOrderDetailsbyorderId(orderId: number, quantityId: number, dAdminId: number) {
  // if(ordersrowclick==1){
  //    this.ordersgridrowclick=1;
  // }
  // else
  // {
  //   this.ordersgridrowclick=0;
  // }
  debugger;
  this.chngeflag = 0;



  if (this.valueChangesFlagReceive == 1 && this.newOrderFlag!=1) {
    const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.result.subscribe((receivedResult) => {
      if (receivedResult == 2) {
        //this.valueChangesFlagReceive = 0;
        modalRef.close();
      }
      else if (receivedResult == 1) {
        this.discardChanges=false;
        this.sharedService.saveChangesOrderInfo(0);
        this.valueChangesFlagReceive = 0;
        //this.ng4LoadingSpinnerService.show();
        this.orderId = orderId;
        this.quantityId = quantityId;
        this.dAdminId = dAdminId;
        this.notesFlag = 0;
        this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          this.updateOrdersRecords = [];
          this.resetCheckbox = false;
          this.myform.enable();
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.getOrderStockDetailsByOrderId();

        this.getFavouritesMasterData();

        this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId + "/" + this.userID)
          .subscribe(res => {
            // if (res.ControlSubstanceBit == 1) {
            //   if (res.ControlSubCreatedBy != this.userID) {
            //     this.isReadOnlyforControl = true;
            //   }
            //   else {
            //     this.isReadOnlyforControl = false;
            //   }
            // }
            this.isReadOnlyforControl = true;
            // this.getOrderStockDetailsByOrderId();
            // this.getFavouritesMasterData();
            //this.GetScheduleTimeDetails();
            this.newOrderFlag = 0;
            this.ordersDetails = res;
            this.selectedOrderDetails=res;
            this.qtydisabled = true;
            this.fieldsChangesDcFlag="";
            this.GetPhysicianDropData(this.nurseStationId);
            this.fetchOrdersData(res);

           // this.getEmarOrdersList3();
            this.selectedOrder = orderId;
            this.selectedOrderQuantity = quantityId;
            this.selectedOrderDADminId = dAdminId;
            this.hoaObj = null;
            // this.getNurseCommentNotesByQuantityId();
            //this.ng4LoadingSpinnerService..hide();
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          }, error => {

            this.alertService.error(error.message);
            //this.ng4LoadingSpinnerService..hide();
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();

          });
      }

     // this.forChange();
      modalRef.close();

    });

  }
  else {
    //this.ng4LoadingSpinnerService.show();
    this.orderId = orderId;
    this.quantityId = quantityId;
    this.dAdminId = dAdminId;
    this.notesFlag = 0;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.getOrderStockDetailsByOrderId();
    this.getFavouritesMasterData();
    this.getNurseCommentNotesByQuantityId();
    this.GetPhysicianDropData(this.nurseStationId);
    this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId + "/" + this.userID)
      .subscribe(res => {
        // if (res.ControlSubstanceBit == 1) {
        //   if (res.ControlSubCreatedBy != this.userID) {
        //     this.isReadOnlyforControl = true;
        //   }
        //   else {
        //     this.isReadOnlyforControl = false;
        //   }
        // }
        this.isReadOnlyforControl = true;
        // this.getOrderStockDetailsByOrderId();
        // this.getFavouritesMasterData();
        //this.GetScheduleTimeDetails();
        this.newOrderFlag = 0;
        this.ordersDetails = res;
        this.selectedOrderDetails=res;
        this.fieldsChangesDcFlag="";
        this.qtydisabled = true;

        this.fetchOrdersData(res);
      //  this.getEmarOrdersList3()
        this.selectedOrder = orderId;
        this.selectedOrderQuantity = quantityId;
        this.selectedOrderDADminId = dAdminId;
        this.hoaObj = null;
        // this.getNurseCommentNotesByQuantityId();
        this.valueChangesFlagReceive = 0;
        //this.ng4LoadingSpinnerService..hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      }, error => {
        this.alertService.error(error.message);
        //this.ng4LoadingSpinnerService..hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });


    }

   // this.getEmarOrdersList3()
}



  getOrderDetailsbyorderIdRowClick(orderId: number, quantityId: number, dAdminId: number,POrder_Status:number) {
    // if(ordersrowclick==1){
    //    this.ordersgridrowclick=1;
    // }
    // else
    // {
    //   this.ordersgridrowclick=0;
    // }
    debugger;
    this.chngeflag = 0;
    this.clicked=false;



    if (this.valueChangesFlagReceive == 1 && this.newOrderFlag!=1 && this.discardChanges) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          //this.valueChangesFlagReceive = 0;
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.discardChanges=false;
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive = 0;
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          //this.ng4LoadingSpinnerService.show();
          this.updateOrdersRecords = [];
          this.resetCheckbox = false;
          this.myform.enable();
          this.orderId = orderId;
          this.quantityId = quantityId;
          this.dAdminId = dAdminId;
          this.notesFlag = 0;
          this.spinnerLoading++;
            this.checkAndHideSpinnerLoading();
            this.getOrderStockDetailsByOrderId();
            this.getFavouritesMasterData();
                    this.GetPhysicianDropData(this.nurseStationId);

          this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId + "/" + this.userID)
            .subscribe(res => {

              // if (res.ControlSubstanceBit == 1) {
              //   if (res.ControlSubCreatedBy != this.userID) {
              //     this.isReadOnlyforControl = true;
              //   }
              //   else {
              //     this.isReadOnlyforControl = false;
              //   }
              // }
              this.isReadOnlyforControl = true;
              // this.getOrderStockDetailsByOrderId();
              // this.getFavouritesMasterData();
              //this.GetScheduleTimeDetails();
              this.newOrderFlag = 0;
              this.ordersDetails = res;
              this.selectedOrderDetails=res;
              this.fieldsChangesDcFlag="";
              this.qtydisabled = true;
              this.fetchOrdersData(res);

             // this.getEmarOrdersList3();
              this.selectedOrder = orderId;
              this.selectedOrderQuantity = quantityId;
              this.selectedOrderDADminId = dAdminId;
              this.hoaObj = null;
              this.getNurseCommentNotesByQuantityId();
              //this.ng4LoadingSpinnerService..hide();
              this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            }, error => {
              this.alertService.error(error.message);
              //this.ng4LoadingSpinnerService..hide();
              this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            });
        }

       // this.forChange();
        modalRef.close();

      });

    }
    else {
      //this.ng4LoadingSpinnerService.show();
      this.orderId = orderId;
      this.quantityId = quantityId;
      this.dAdminId = dAdminId;
      this.notesFlag = 0;
      this.spinnerLoading++;
            this.checkAndHideSpinnerLoading();
            // this.getOrderStockDetailsByOrderId(); 
            // this.getFavouritesMasterData();
            
            // this.GetPhysicianDropData(this.nurseStationId);
            this.getOrderStockDetailsByOrderId();
            this.getFavouritesMasterData();
                    this.GetPhysicianDropData(this.nurseStationId);
      this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId + "/" + this.userID)
        .subscribe(res => {

          // if (res.ControlSubstanceBit == 1) {
          //   if (res.ControlSubCreatedBy != this.userID) {
          //     this.isReadOnlyforControl = true;
          //   }
          //   else {
          //     this.isReadOnlyforControl = false;
          //   }
          // }
          this.isReadOnlyforControl = true;
          // this.getOrderStockDetailsByOrderId();
          // this.getFavouritesMasterData();
          //this.GetScheduleTimeDetails();
          this.newOrderFlag = 0;
          this.ordersDetails = res;
          this.selectedOrderDetails=res;
          this.fieldsChangesDcFlag="";
          this.qtydisabled = true;
              this.fetchOrdersData(res);
        //  this.getEmarOrdersList3()
          this.selectedOrder = orderId;
          this.selectedOrderQuantity = quantityId;
          this.selectedOrderDADminId = dAdminId;
          this.hoaObj = null;
          this.getNurseCommentNotesByQuantityId();
          this.valueChangesFlagReceive = 0;
          //this.ng4LoadingSpinnerService..hide();
          this.spinnerLoading--;
          
          this.checkAndHideSpinnerLoading();
        }, error => {
          this.alertService.error(error.message);
          //this.ng4LoadingSpinnerService..hide();
          this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
        });


      }

     // this.getEmarOrdersList3()
  }
  newOrder() {

    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive=0;
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          if (this.residentId != 0) {
            if (this.demographicInfoData.PVisit_Status == 2) {
              this.alertService.warn("Resident has been discharged.")
            }
            else if (this.demographicInfoData.PVisit_Status == 3) {
              this.alertService.warn("Status updated to temporarily inactive.")
            }
            else {
              this.destroyform.reset();
              this.fieldsChangesDcFlag="";
              this.newOrderFlag = 1;
              this.reviewClickedFlag = 0;
              this.favstatus = 0;
              this.splits=0;
              this.reactivateStatus=0;
              this.favForm.reset();
              this.favobj = [];
              this.favouriteObj = [];
              this.selectedphyItems = [];
              this.selectedroItems = [];
              this.barCodeStatusFlag=0;
              if (this.defaultPhysicianNPI != null) {
                let checkExist=(this.physiciansdrop!=null && this.physiciansdrop!=undefined) ? this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI):null;
                if(checkExist!=null && checkExist!=undefined)
                {
                this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);

                }
              }
              this.myform.patchValue({
                physician: this.selectedphyItems,
                route: this.selectedroItems,
                qtyHand: '',
                drug: '',
                dose: '',
                refill: '',
                addInst: '',
                insulincomments: '',
                barcode: '',
                startDate: '',
                maxPerDay: '',
                alertText: '',
                endDate: '',
                type: 1,
                schduleText: '',
                self: '',
                treatment: '',
                literal:'',
                prn: '',
                controlSubstance: '',
                orderTypeId: 1
              });
              this.barcodear = [];
              this.isReadOnly = false;
              this.ordersDetails = {} as OrdersData;
              this.selectedOrderDetails={};
              this.fieldsChangesDcFlag="";
              this.selectedOrder = 0;
              this.selectedOrderQuantity = 0;
              this.selectedOrderDADminId = 0;
              this.orderId = 0;
              this.quantityId = 0;
              this.stockId = 0;
              this.gpiCode = '';
              this.resetScheduleForm();
              this.mergeFlag = 0;
              this.isReadOnlyforControl = true;
              this.isControlSubstanceReadOnly = false;
              this.isDrugOrder=false;
              this.notesFlag = 1;
              this.barCodeFlag =1;
              this.discardChanges = false;
              //this.getOrderGridData("Test");
              const maxpervalidation = this.myform.get('maxPerDay');
              maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
              Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
              Validators.pattern(/^\d*(\.\d{0,3})?$/),
              Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
              maxpervalidation.updateValueAndValidity();

              if (this.barcodear.length == 0) {
                const barcodevalidation = this.myform.get('barcode');
                barcodevalidation.setValidators([Validators.required]);
                barcodevalidation.updateValueAndValidity();

              }
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
          this.destroyform.reset();
          this.fieldsChangesDcFlag="";
          this.newOrderFlag = 1;
          this.reviewClickedFlag = 0;
          this.favstatus = 0;
          this.splits=0;
          this.reactivateStatus=0;
          this.favForm.reset();
          this.favobj = [];
          this.favouriteObj = [];
          this.selectedphyItems = [];
          this.selectedroItems = [];
          if (this.defaultPhysicianNPI != null) {
            let checkExist=(this.physiciansdrop!=null && this.physiciansdrop!=undefined)?this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI):null
            if(checkExist!=null && checkExist!=undefined)
            {
            this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);

          }
        }
          this.myform.patchValue({
            physician: this.selectedphyItems,
            route: this.selectedroItems,
            qtyHand: '',
            drug: '',
            dose: '',
            refill: '',
            addInst: '',
            insulincomments: '',
            barcode: '',
            startDate: '',
            maxPerDay: '',
            alertText: '',
            endDate: '',
            type: 1,
            schduleText: '',
            self: '',
            treatment: '',
            literal:'',
            prn: '',
            controlSubstance: '',
            orderTypeId: 1
          });
          this.barcodear = [];
          this.isReadOnly = false;
          this.ordersDetails = {} as OrdersData;
          this.selectedOrderDetails={};
          this.fieldsChangesDcFlag="";
          this.selectedOrder = 0;
          this.selectedOrderQuantity = 0;
          this.selectedOrderDADminId = 0;
          this.orderId = 0;
          this.quantityId = 0;
          this.stockId = 0;
          this.gpiCode = '';
          this.resetScheduleForm();
          this.mergeFlag = 0;
          this.isReadOnlyforControl = true;
          this.isControlSubstanceReadOnly = false;
          this.isDrugOrder=false;
          this.notesFlag = 1;
          this.barCodeFlag =1;
          this.discardChanges = false;
          //this.getOrderGridData("Test");
          if (this.barcodear.length == 0) {
            const barcodevalidation = this.myform.get('barcode');
            barcodevalidation.setValidators([Validators.required]);
            barcodevalidation.updateValueAndValidity();

          }
          const drugnameValidations = this.myform.get('drug');
          drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
          drugnameValidations.updateValueAndValidity();
          const barcodevalidation = this.myform.get('barcode');
          barcodevalidation.setValidators([Validators.required, Validators.maxLength(150)]);
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
          const maxpervalidation = this.myform.get('maxPerDay');
          maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
          Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          Validators.pattern(/^\d*(\.\d{0,3})?$/),
          Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
          maxpervalidation.updateValueAndValidity();
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
    this.selectedMonths=[];
    this.selectedWeeks=[];
    this.timesArray = [];
    this.timeform.reset();
    this.isShiftSchedule = false;
    this.isHoursReadOnly = false;
    this.isdaysDisabled=false;
    this.isWeeksDisabled=false;
    this.selectedfrequencyItems = [];
    this.selectedstItems = [];
    this.resetMonthsDropSettings(13);
    this.resetWeekDropSettings(13);
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
        if(OrdersNPI != "")
        {
            this.spinnerLoading++;
            this.checkAndHideSpinnerLoading();
            this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + this.nurseStationId+"/"+this.orderId)
            .subscribe(res => {
              this.physiciansdrop = res;
                let physicianRecord1 = this.physiciansdrop.filter(p => p.PhysicianNPI == OrdersNPI)[0];
          if (physicianRecord1 != undefined)
          {
            this.selectedphyItems = [];
            this.selectedphyItems.push(physicianRecord1);
             // console.log(physicianRecord1.Physician_Id +"Srikar");

              this.prePId = physicianRecord1.Physician_Id;
           }
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          }, error => {
            this.alertService.error(error.message);
            //this.ng4LoadingSpinnerService..hide();
            this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
          });
    }
  // this.spinnerLoading++;
  // this.checkAndHideSpinnerLoading();
  //   this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData + 0 + "/" + this.nurseStationId+"/"+this.orderId)
  //   .subscribe(res => {
  //     this.spinnerLoading--;
  //     this.checkAndHideSpinnerLoading();

  //     this.physiciansdrop = res;
  //     if(OrdersNPI != "")
  //     {

  //     let physicianRecord1 = this.physiciansdrop.filter(p => p.PhysicianNPI == OrdersNPI)[0];
  //   if (physicianRecord1 != undefined)
  //  {

  //   this.selectedphyItems = [];
  //     this.selectedphyItems.push(physicianRecord1);
  //     // console.log(physicianRecord1.Physician_Id +"Srikar");

  //       this.prePId = physicianRecord1.Physician_Id;



  //  }
  // }
  //   }, error => {
  //     this.alertService.error(error.message);
  //     this.spinnerLoading--;
  //     this.checkAndHideSpinnerLoading();
  //   });
  }
  
  
   fetchOrdersData(res) {

    debugger;
    // if (this.demographicInfoData.PVisit_Status == 2) {
    //   this.activeTab = 'Inactive';
    // }
    // else {
    //   this.activeTab = 'Active';
    // }
    this.autobarcode = '';
    this.gpiConst = res.AGiveCodeIdentifier
    let writtendate = res.WrittenDate;
    this.writtendate =this.dateFormatPipe.transformISODate(writtendate);
    if (this.demographicInfoData.PVisit_Status == 2) {
      this.alertService.warn("Resident has been discharged.")
    }
    else if (this.demographicInfoData.PVisit_Status == 3) {
      this.alertService.warn("Status updated to temporarily inactive.")
    }
    const maxpervalidation = this.myform.get('maxPerDay');
    maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
    Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
    Validators.pattern(/^\d*(\.\d{0,3})?$/),
    Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
    maxpervalidation.updateValueAndValidity();
    this.reviewClickedFlag = 0;
    this.valueChangesFlag = 0;
    this.barCodeFlag =0;
    if(res.AutoBarcode!=null && res.AutoBarcode!=''){
      this.autobarcode=res.AutoBarcode.trim('');
      //console.log(this.autobarcode,"autobarcode")
    }
    if (res.OrderTypeID == 4 || res.OrderTypeID == 2) {
      const drugnameValidations = this.myform.get('drug');
      drugnameValidations.clearValidators();
      drugnameValidations.updateValueAndValidity();
      const barcodevalidation = this.myform.get('barcode');
      barcodevalidation.setValidators([Validators.maxLength(150)]);
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
      barcodevalidation.setValidators([Validators.required, Validators.maxLength(150)]);
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
    //Srikar
    if (res.Barcode != null && res.Barcode != '') {
      let list: string = res.Barcode;
      this.barcodear = list.split(', ');
      this.pbarcodear =list.split(', ');
      const barcodevalidation = this.myform.get('barcode');
      //barcodevalidation.setValidators(null);
      barcodevalidation.setValidators([Validators.maxLength(150)]);
      barcodevalidation.updateValueAndValidity();
    }
    else {
      this.barcodear = [];
      this.pbarcodear = [];
    }
   
  
    this.favstatus = res.Favouriteflag != 1 ? 0 : 1;
    this.reactivateStatus = (res.POrder_Status != 1) ? 1 : 0;
    this.splits = res.split != 1 ? 0 : 1;
    this.multipleOrdersplits=0;
    //this.isReadOnly = res.ReviewFlag == 0 ? false : true;
    this.mergeFlag = res.MergeFlag == 1 ? 1 : 0;
    this.reviewFlag = res.ReviewFlag;
    this.discontinueFlag =res.DiscontinueFlag;
    //this.drugNameFromPharmacyOrder = res.DrugName == null ? '' : res.DrugName;
    this.selectedphyItems = [];
    this.selectedroItems = [];
    let OrdersNPI = res.OrderingPhysicianNPI;
     if(res.SchFlag!=undefined && res.SchFlag==1)
     {
       this.SchFlag=true;
     }
     else{
       this.SchFlag=false;
     }
    if (res.OrderingPhysicianNPI != null) {

    //   let physicianRecord = this.physiciansdrop.filter(p => p.PhysicianNPI == res.OrderingPhysicianNPI)[0];
    //   if (physicianRecord != undefined)
    //  {

    //     this.selectedphyItems.push(physicianRecord);
    //  }
     this.funBindPrecriber(res.OrderingPhysicianNPI);




    }
    else {
      this.getAllFlagsForCompanyByNSId(this.nurseStationId,this.residentId);
      if (res.OrderTypeID == 2 || res.OrderTypeID == 4) {
        // this.GetPhysicianDropData(this.nurseStationId);

        let physicianRecord = this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0];

        if (physicianRecord != undefined)
          this.selectedphyItems.push(physicianRecord);
      }

    }
    if (res.Route_Id != null && res.Route_Id != 0  && res.Route_Id != undefined) {

      this.selectedroItems.push(this.routes.filter(r => r.Route_Id == res.Route_Id)[0]);

      // console.log(res.Route_Id + "routeid");
      this.preRId = res.Route_Id;

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
//Srikar
    this.myform.patchValue({

      physician: this.selectedphyItems,
      route: this.selectedroItems,

      qtyHand: res.Inhand == null ? 0.00 : res.Inhand,
      drug: res.DrugName,
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
      literal:(res.OrderTypeID==4||res.OrderTypeID==2)?true:false,
      prn: res.PRNFlag,
      controlSubstance: res.ControlSubstanceBit == 1 ? true : false,
      orderTypeId: res.OrderTypeID,

      pphysician: this.myform.value.physician,
      proute: this.myform.value.route,

      pqtyHand: res.Inhand == null ? 0.00 : res.Inhand,
      pdrug: res.DrugName,
      pdose: res.Quantity,
      prefill: res.Refill,
      paddInst: res.Directions,
      pinsulincomments: res.InsulinComments,

      pbarcode:(res.Barcode == null || res.Barcode == '') ? '' : res.Barcode.trim(),
      pstartDate: (res.StartDate == null ? '' : res.StartDate.substring(0, 10)),
      pmaxPerDay: res.MaxPerdays,
      palertText: res.AlertText,
      pendDate: (res.EndDate == null ? '' : res.EndDate.substring(0, 10)),
      ptype: res.OrderStockFlag,
      pschduleText: res.Schedule != "Select Time" ? res.Schedule : '',
      pself: res.SelfAdministeredFlag,
      ptreatment:(res.TreatmentFlag==1 ||res.OrderTypeID==2 ||res.OrderTypeID==5)?true:false,
      pliteral:(res.OrderTypeID==4||res.OrderTypeID==2)?true:false,
      pprn: res.PRNFlag,
      pcontrolSubstance: res.ControlSubstanceBit == 1 ? true : false,
      porderTypeId: res.OrderTypeID


    });
    // this.barcodeAlertFlag=true;
    // if(res.OrderTypeID==2 && res.Barcode==''){
    //   this.discardChanges = false;
    //   const residentIdOderId=(`${this.residentId}NW${res.porder_Id}`)
    //   this.barcodear.push(residentIdOderId);
    //   //this.discardChanges = true;
    //    this.barcodeAlertFlag=false;
    //   this.DelayCalling()
    // }
    // if (res.Quantity == 0 && res.OrderTypeID !=4) {
    //   this.alertService.warn("Invalid Qty/Dose");
    //   this.myform.patchValue({
    //     dose: ''
    //   });
    //   const doseValidations = this.myform.get('dose');
    //   doseValidations.setValidators([Validators.required]);
    // }
    if(res.PRNFlag==1)
    {
    //if(res.MaxPerdays == null || res.MaxPerdays == undefined || (res.MaxPerdays == 0)){
          // it should accept the 0 value (10/28/2025)
    if(res.MaxPerdays == null || res.MaxPerdays == undefined){
      // this.alertService.warn("Please enter Max Per Day")
      this.myform.patchValue({
        maxPerDay: ''
      });
      this.isInteger();
      const maxpervalidation = this.myform.get('maxPerDay');
      maxpervalidation.setValidators([Validators.required]);
      maxpervalidation.updateValueAndValidity();   
      this.styleMaxperday = {'border-color':'red'};
       
      setTimeout(()=>{
        this.MaxperdayIpFocus.nativeElement.focus();
     
      },1000);
    }
    const maxpervalidation = this.myform.get('maxPerDay');
    maxpervalidation.setValidators([Validators.maxLength(7),Validators.required,  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
    Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
    Validators.pattern(/^\d*(\.\d{0,3})?$/),
    Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
    maxpervalidation.updateValueAndValidity();
    }
    else{

      this.styleMaxperday = {'border-color':'black'};
      this.myform.controls.maxPerDay.updateValueAndValidity();
  
  }
    if ((res.Quantity == 0 || res.Quantity == null || res.Quantity == undefined) && res.OrderTypeID != 4) {

  // this.alertService.warn("Invalid Qty/Dose");

  this.myform.patchValue({
    dose: ''
  });

  setTimeout(() => {
    this.qtyIpFocus.nativeElement.focus();
  }, 1000);

  this.qtydisabled = false;

} else {

  // Admin can edit, non-admin cannot
  this.qtydisabled = !this.adminLoggedin;

}
  
    if(res.OrderTypeID==1)
    {
      this.isDrugOrder=true;
    }
    else
    {
      this.isDrugOrder=false;
    }
    if(this.ordersDetails.HoldStatus == 1)
    {
      this.getOrderHoldFromToDates( this.quantityId);
    }
    this.orderOrigin =res.OrderOrigin;
    //this.valueChangesFlag = 1;
    // if(this.ordersgridrowclick==1){
    //   this.saveChangeFlag = 0;
    // this.sharedService.saveChangesOrderInfo(this.saveChangeFlag);
    // }
    // else{
    //   this.valueChangesFlag = 1;
    // }
    this.medispanControlSubBit =res.MedispanControlSubBit;
    this.controlsubstanceBit =res.ControlSubstanceBit;
    if (res.ControlSubstanceBit == 1 && res.MedispanControlSubBit == 1) {
      this.isControlSubstanceReadOnly = true;
    }
    else
      this.isControlSubstanceReadOnly = false;
      this.loadSearchData();
      //if(res.MaxPerdays != null && res.MaxPerdays >0)
      // it should accept the 0 value (10/28/2025)

      if(res.MaxPerdays != null && res.MaxPerdays >=0)
      this.isInteger();

      setTimeout(()=>{
        this.valueChangesFlag = 1;
      this.sharedService.saveChangesOrderInfo(0);
    }, 3000);

  }



  getOrdersGridDatabyfilter(filter: any) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderGridData + this.residentId + "/" + filter)
      .subscribe(res => {
        this.orderGridData = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  emarModel() {

    //this.ng4LoadingSpinnerService.show();
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
    var date=new Date();
    this.days=new Date(this.yearDrop.length==0? date.getFullYear():parseInt(this.emarform.value.year),parseInt(this.emarform.value.month),0).getDate();
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    //  this.dataservice.get<any[]>(this.config.Emar_getEMARDetails + date.getMonth() + "/" + date.getFullYear() + "/" + this.residentId)
    this.dataservice.get<any[]>(this.config.Emar_getEMARDetails + this.emarform.value.month + "/" + this.emarform.value.year + "/" + this.residentId)
      .subscribe(res => {

        this.eMARDetails = res;
        //this.ng4LoadingSpinnerService..hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (this.emarform.value.hidechk != null && this.emarform.value.hidechk == true)
          this.HideInactiveOrders(this.emarform.value.hidechk);
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
      //Legend
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.get<any[]>(this.config.Emar_getEMARDetailsLegend + this.emarform.value.month + "/" + this.emarform.value.year + "/" + this.residentId)
      .subscribe(res1 => {

        this.legend = res1;
        //this.ng4LoadingSpinnerService..hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (this.emarform.value.hidechk != null && this.emarform.value.hidechk == true)
          this.HideInactiveOrders(this.emarform.value.hidechk);
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });

  }
  getEmarPreviewYearDrop() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_Orders_GetEmarPreviewYearDrop + this.residentId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.yearDrop = res;
        if(this.yearDrop==null || this.yearDrop.length==0)
        {
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
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
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
    if (this.valueChangesFlagReceive == 1) {
      const modalRefs = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRefs.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          this.selectedResItem = this.residents.filter(r => r.Patient_Id === this.residentId);
          modalRefs.close();
        }
        else if (receivedResult == 1) {
          this.chngeflag = 0;
    this.discardChanges = false;
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    this.profileDCChangesCheckState={};
    this.profileONHoldChangesCheckState={};
    this.sharedService.saveChangesOrderInfo(0);
    let residents = this.myform.value.ddlresidents;
    if (residents.length != 0) {
      //this.ng4LoadingSpinnerService.show();
      this.residentId = this.myform.value.ddlresidents[0].Patient_Id;
      this.sharedService.changePatientId(this.residentId);
      //this.orderId = 0;
      //this.pendingReviewCount = 0;
      this.clearData(1);
      this.getAllFlagsForCompanyByNSId(this.selectedNursestation[0].NurseStation_Id, this.residentId)
      //this.getNurseStationByPId();
      this.getDemographicInfoData();
      // this.getNurseCommentNotesByQuantityId();

    }
    else {
      this.clearData(1);
      this.residentId = 0;
      this.sharedService.changePatientId(this.residentId);
      // this.getNurseCommentNotesByQuantityId();
      this.orderId = 0;
      this.pendingReviewCount = 0;
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
        modalRefs.close();
      });
    }else{
      this.chngeflag = 0;
    this.discardChanges = false;
    let residents = this.myform.value.ddlresidents;
    if (residents.length != 0) {
      //this.ng4LoadingSpinnerService.show();
      this.residentId = this.myform.value.ddlresidents[0].Patient_Id;
      this.sharedService.changePatientId(this.residentId);
      //this.orderId = 0;
      //this.pendingReviewCount = 0;
      this.clearData(1);
      this.getAllFlagsForCompanyByNSId(this.selectedNursestation[0].NurseStation_Id, this.residentId)
      //this.getNurseStationByPId();
      this.getDemographicInfoData();
      // this.getNurseCommentNotesByQuantityId();

    }
    else {
      this.clearData(1);
      this.residentId = 0;
      this.sharedService.changePatientId(this.residentId);
      // this.getNurseCommentNotesByQuantityId();
      this.orderId = 0;
      this.pendingReviewCount = 0;
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
   
  }

  orderDestroyDetailsCancel() {
    this.destroyform.reset();
    this.closeModel();
  }
  getOrderRoutes() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderRoutes)
      .subscribe(res => {
        this.routes = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  DestroyOrder() {
    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive=0;
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          this.discardChanges = false;
          let onHandQty = this.myform.value.qtyHand;
          if (onHandQty == undefined || onHandQty == null || onHandQty == "" || (onHandQty != undefined && onHandQty != null && onHandQty != "" && parseFloat(onHandQty)==0)) {
            this.alertService.warn("Quantity on hand not available");
            //this.ng4LoadingSpinnerService..hide();
          }
          else
          {
            this.resetOrderDestroy();
          this.modalDesIsOpen = true;
          setTimeout(() => {
            this.quantityFocus.nativeElement.focus()
            }, 300);    
          }
        }
        modalRef.close();
      });
    }
    else {
      let onHandQty = this.myform.value.qtyHand;
          if (onHandQty == undefined || onHandQty == null || onHandQty == "" || (onHandQty != undefined && onHandQty != null && onHandQty != "" && parseFloat(onHandQty)==0)) {
            this.alertService.warn("Quantity on hand not available");
            //this.ng4LoadingSpinnerService..hide();
          }
          else
          {
            this.resetOrderDestroy();
          this.modalDesIsOpen = true;
          setTimeout(() => {
            this.quantityFocus.nativeElement.focus()
            }, 300);
          }
    }
  }
  modalfav() {
    if(this.newOrderFlag!=1)
    {
      this.getFavouritesMasterData();
    }
    this.modalfavIsOpen = true;
  }
  closeFavModal()
  {
    if(this.newOrderFlag==1 && this.favstatus!=1)
      {
        this.favForm.reset();
        this.favobj=[];
        this.favouriteObj=[];
      }
    if(this.newOrderFlag==1 && this.favobj.length==0)
      {
        this.favstatus=0;
      }
    this.closeModel();
  }
  modalnote() {
    this.modalnoteIsOpen = true;
    this.getNurseCommentNotesByQuantityId();
    setTimeout(() => {
      this.nursecomments.nativeElement.focus()
    }, 300);
  }
  destroyOrderSave() {

    let onHandQty=this.myform.value.qtyHand;
    if(onHandQty==undefined || onHandQty==null || onHandQty==""|| (onHandQty != undefined && onHandQty != null && onHandQty != "" && parseFloat(onHandQty)==0))
    {
      this.alertService.warn("Quantity on hand not available");
      //this.ng4LoadingSpinnerService..hide();
    }
    else if(onHandQty!=undefined && onHandQty!=null && onHandQty!="" && parseFloat(this.destroyform.value.quantity)>parseFloat(onHandQty))
    {
      this.alertService.warn("Quantity to be destroyed is greater than current on hand quantity");
      //this.ng4LoadingSpinnerService..hide();
    }
    else{
    //this.ng4LoadingSpinnerService.show();
    this.destroyObj = {
      OrderDestroy_Id: 0,
      PQuantity_Id: this.quantityId,
      Quantity: this.destroyform.value.quantity,
      Reason: this.destroyform.value.reason,
      DestroyerUserId: 0,
      ApprovalUserId: 0,
      OrderDestroy_Status: 1,
      OrderDestroy_CreatedBy: this.userID,
      OrderDestroy_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
      DUserName: this.destroyform.value.destroyerUsername,
      DPassword: this.destroyform.value.destroyerPass,
      AUserName: this.destroyform.value.approvalUsername,
      APassword: this.destroyform.value.approvalPass
    };
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_InsertOrderdestroy, this.destroyObj)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (res == "Done") {
          this.resetOrderDestroy();
          //this.destroyStatus();
          this.alertService.success("Specified quantity was documented for destruction successfully");

          this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);

          this.modalDesIsOpen = false;
         // this.
          //this.alertService.success("Successfully Order Destroyed");
        }
        else {
          this.alertService.error(res);
        }
        //this.ng4LoadingSpinnerService..hide();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });
    }
  }
  resetOrderDestroy() {
    this.destroyform.reset();
  }
  // getNurseStationByPId() {
  //   this.dataservice.get<any>(this.config.Resident_Demographic_GetNurseStationByPId + this.residentId)
  //     .subscribe(res => {
  //       let id = res;
  //       this.selectedNursestation = this.nurseStations.filter(item => item.NurseStation_Id == id);
  //       this.myform.patchValue({
  //         nursestationName: this.selectedNursestation,
  //       });
  //       this.nurseStationId = this.selectedNursestation[0].NurseStation_Id;
  //       this.getAllFlagsForCompanyByNSId(this.nurseStationId);
  //     },
  //       error => {
  //         this.alertService.error(error.message);
  //         //this.ng4LoadingSpinnerService..hide();
  //       });
  // }
  getFavouritesMasterData() {
    //this.ng4LoadingSpinnerService.show();
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetFavouritesMasterData + this.quantityId + '/' + this.barcodeFacilityId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (this.newOrderFlag != 1 || this.quantityId==0) {
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
     
        //this.ng4LoadingSpinnerService..hide();
      }, error => {
        //this.ng4LoadingSpinnerService..hide();
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
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
        FavListCheckFlag:0,
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
        FavListCheckFlag:0,
      }
      this.favobj.push(objFav.OrderFavMaster_ID);
      this.favouriteObj.push(objFav);
    }
    else {
      let index = (this.favouriteObj!=null && this.favouriteObj!=undefined) ? this.favouriteObj.findIndex(fav => fav.OrderFavMaster_ID == FavMasterID):null;
      if(this.favouriteObj!=null && this.favouriteObj!=undefined)
      this.favouriteObj.splice(index, 1);
      let favindex = (this.favobj!=null && this.favobj!=null) ? this.favobj.findIndex(f => f === FavMasterID):null;
      if(this.favobj!=null && this.favobj!=undefined)
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
        //this.ng4LoadingSpinnerService.show();
        if (this.newOrderFlag == 1) {
          //this.favobj;
          if (this.favouriteObj.length >= 1) {
            this.favstatus = 1;
          }
          else if (this.favouriteObj.length == 0) {
            this.favstatus = 0;
          }
          this.modalfavIsOpen = false;
          //this.ng4LoadingSpinnerService..hide();
        }
        else if (this.newOrderFlag != 1) {
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
          this.funFavOrderChange();
          this.spinnerLoading++;
          this.checkAndHideSpinnerLoading();
          this.dataservice.post(this.config.Emar_Orders_InsertOrderFavourities, this.favouriteObj)
            .subscribe(res => {
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
              this.getFavouritesMasterData();
              //this.ng4LoadingSpinnerService..hide();
              this.alertService.success("Measurements and other checks saved successfully");


              this.modalfavIsOpen = false;
              if(this.newOrderFlag != 1)
              this.favouriteObj = [];

              //this.favstatus = 1;
            }, error => {
              this.modalfavIsOpen = false;
              //this.ng4LoadingSpinnerService..hide();
              this.alertService.error(error.message);
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
            });
        }
        else {
          //this.ng4LoadingSpinnerService..hide();
          this.alertService.warn("No records selected");
        }
      }
    }
    else {
      this.alertService.error("No Resident selected");
    }
  }
  funFavOrderChange()
  {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_Orders_OrderFavouritiesOrderChange + this.orderId + '/' + this.userID)
    .subscribe(res => {
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();
      return true;
    }, error => {
      this.modalfavIsOpen = false;
      //this.ng4LoadingSpinnerService..hide();
      this.alertService.error(error.message);
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();
    });
  }
  onFrequencyChange(item: any) {

    if (this.selectedfrequencyItems.length != 0) {
      let frequencyId = this.scheduleform.value.frequency[0].Frequency_Id;
      if ((frequencyId.startsWith('s') || frequencyId == 28 || ((this.frequencyList != null && this.frequencyList != undefined) && this.frequencyList.find(f => f.Frequency_Id == frequencyId).Frequency_PRN == 1)) && ((frequencyId != 12) && (frequencyId != 13))) {
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
        //this.ng4LoadingSpinnerService.show();
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.dataservice.get<any>(this.config.Emar_Orders_GetNurseFrequencyDropSelect + frequencyId + '/' + this.nurseStationId)
          .subscribe(res => {
            this.fetchFrequencyMapData(frequencyId, res);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            //this.ng4LoadingSpinnerService..hide();
          }, error => {
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            //this.ng4LoadingSpinnerService..hide();
          });
      }
      if((this.scheduleform.value.frequency!=null && this.scheduleform.value.frequency!=undefined) && this.frequencyList.length != 0 && this.selectedfrequencyItems.length != 0 )
    {

this.FrequnceLimitFlag = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Times;
this.FrequnceLimitAlertText = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Descalert;

}
    }
    else if (this.selectedfrequencyItems.length == 0) {
      this.hoaObj = null;
      this.GetMonthMasterData();
      this.GetWeekMasterData();
      this.isShiftSchedule = false;
      this.isHoursReadOnly = false;
      this.isdaysDisabled=false;
      this.isWeeksDisabled=false;
      this.scheduleform.reset();
      this.timeform.reset();
      this.getDaysDropData();
      this.selectedDays = [];
      this.selectedMonths=[];
      this.selectedWeeks=[];
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
      if((this.scheduleform.value.frequency!=null && this.scheduleform.value.frequency!=undefined) && this.frequencyList.length != 0 && this.selectedfrequencyItems.length != 0 )
    {


this.FrequnceLimitFlag = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Times;
this.FrequnceLimitAlertText = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Descalert;
}
      if(this.newOrderFlag==1)
      {
        this.myform.patchValue({
          prn:'',
          schduleText:'',
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
      if (res.MonthId!=undefined && res.MonthId!=null &&  res.MonthId!= "") {
        this.selectedMonths = res.MonthId.toString().split(',').map(Number);
        selectMonths = this.monthsList.filter(item => this.selectedMonths.includes(item.Month_Id));
        if(selectMonths.filter(ite=>ite.Month_Id==13).length!=0)
        {
          this.onMonthSelect(selectMonths.filter(ite=>ite.Month_Id==13)[0]);
        }
        else{
          this.resetMonthsDropSettings(13);
        }

      }
      else{
        this.resetMonthsDropSettings(13);
      }
      let selectWeeks = [];
      if (res.WeekId!=undefined && res.WeekId!=null && res.WeekId!= "") {
        this.selectedWeeks = res.WeekId.toString().split(',').map(Number);
        selectWeeks = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));

        this.selectedDays=[];
        this.isdaysDisabled=true;

        if(selectWeeks.filter(ite=>ite.Week_Id==1).length!=0)
        {
          this.onWeekSelect(selectWeeks.filter(ite=>ite.Week_Id==1)[0]);
        }
        else{
          this.resetWeekDropSettings(13);
        }

      }
      else{
        this.resetWeekDropSettings(13);
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
      this.selectedWeeks=[];
      this.selectedMonths=[];
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
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<FrequencyMasterDataWithShifts[]>(this.config.Emar_Orders_GetFrequencyMasterDataWithShifts + this.nurseStationId)
      .subscribe(res => {
        this.frequencyList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.GetHoursMasterData();
        this.GetWeekMasterData();
        this.GetMonthMasterData();
        this.GetScheduleTimeDetails();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  GetWeekMasterData() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<WeekMasterData[]>(this.config.Emar_Orders_GetWeekMasterData)
      .subscribe(res => {
        this.weeksList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
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
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  GetMonthMasterData() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<MonthMasterData[]>(this.config.Emar_Orders_GetMonthMasterData)
      .subscribe(res => {
        this.monthsList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.dropdownSettings_Month = {
          singleSelection: false,
          idField: 'Month_Id',
          textField: 'Month_Name',
          itemsShowLimit: 1,
          allowSearchFilter: true,
          enableCheckAll: false,
          //limitSelection: 13,
        };
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  GetHoursMasterData() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<HoursMasterData[]>(this.config.Emar_Orders_GetHoursDataByNSId + this.nurseStationId + "/" + 0)
      .subscribe(res => {
        this.hoursList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  // GetTimeFormatMasterData() {
  //   this.dataservice.get<TimeFormatMasterData[]>(this.config.Emar_Orders_GetTimeFormatMasterData)
  //     .subscribe(res => {
  //       this.timeFormatList = res;
  //     }, error => {
  //       this.alertService.error(error.message);
  //     });
  // }

  GetScheduleTimeDetails() {

    //this.ng4LoadingSpinnerService.show();
    if (this.newOrderFlag != 1) {
      this.resetScheduleForm();
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.get<DrugAdministrationTime>(this.config.Emar_Orders_GetScheduleTimeDetails + this.orderId + "/" + this.quantityId + "/" + this.nurseStationId)
        .subscribe(res => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          this.scheduleTextObj=res;
          if (res != null)
          {


          this.OrderFrequncryGroup = res.Freq_Group;
            this.fetchScheduleData(res);
          }
          else {
            this.GetMonthMasterData();
            this.GetWeekMasterData();
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
            //this.ng4LoadingSpinnerService..hide();
          }
        }, error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
    }
    else {
      if (this.hoaObj == null) {
        this.GetMonthMasterData();
        this.GetWeekMasterData();
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
      }
      //this.ng4LoadingSpinnerService..hide();
    }
  }
  fetchScheduleData(res: DrugAdministrationTime) {
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
      if(this.weeksList!=null && this.weeksList!=undefined)
      selectWeek = this.weeksList.filter(item => this.selectedWeeks.includes(item.Week_Id));
      
      this.isdaysDisabled=true;
      if((selectWeek!=null && selectWeek!=undefined) && selectWeek.filter(ite=>ite.Week_Id==1).length!=0)
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
      if(this.monthDays!=null && this.monthDays!=undefined)

      activeDays = this.monthDays.filter(item => this.selectedDays.includes(item.item_id));

      this.isWeeksDisabled=true;
    }
    let selectMonths=[];
    if(res.MonthId!=undefined && res.MonthId!=null && res.MonthId!="")
    {
      this.selectedMonths = res.MonthId.toString().split(',').map(Number);
      if(this.monthsList!=null && this.monthsList!=undefined)
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
    if (res.HoursList != null) {
      if(this.hoursList!=undefined)
      this.selectedstItems.push(this.hoursList.filter(h => h.Hour_Id == res.HoursList[0].Hour_Id)[0]);
    }
    this.selectedfrequencyItems = [];
    if (res.NursingFreqId != null) {
      if(this.frequencyList!=undefined)
      this.selectedfrequencyItems.push(this.frequencyList.filter(f => f.Frequency_Id == res.NursingFreqId.toString())[0]);
      if (res.NursingFreqId == 28) {
        this.selectedstItems = null;
        this.isShiftSchedule = true;
        this.isHoursReadOnly = true;
        this.timesArray = [];
      }
      //it is to desible the start time if the freq is prn as needed.
      if (res.NursingFreqId == 1) {
        this.isShiftSchedule = true;
      }
      if (((this.frequencyList != null && this.frequencyList != undefined) && this.frequencyList.find(f => f.Frequency_Id == res.NursingFreqId.toString()) != undefined && this.frequencyList.find(f => f.Frequency_Id == res.NursingFreqId.toString()).Frequency_PRN == 1) && (res.NursingFreqId != 12) && (res.NursingFreqId != 13)) {
        this.myform.patchValue({
          prn: true
        });
        const maxpervalidation = this.myform.get('maxPerDay');
          maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
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
          maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
          Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
          Validators.pattern(/^\d*(\.\d{0,3})?$/),
          Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
          maxpervalidation.updateValueAndValidity();
        }
    }
    else if (res.NurseShiftsId != null) {
      let checkExist=(this.frequencyList!=null && this.frequencyList!=undefined) ? this.frequencyList.find(f => f.Frequency_Id == 's' + res.NurseShiftsId):null;
      if(checkExist!=null && checkExist!=undefined)
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

    if((this.scheduleform.value.frequency!=null && this.scheduleform.value.frequency!=undefined) && this.frequencyList.length != 0 && this.selectedfrequencyItems.length != 0 )
    {


this.FrequnceLimitFlag = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Times;
this.FrequnceLimitAlertText = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Descalert;
}
    //If Schedule text empty frequency is there based on frequency map fetch form
    if(this.newOrderFlag!=1 && this.selectedOrderDetails!=undefined && this.myform.value.schduleText=="" && this.selectedfrequencyItems!=undefined && this.selectedfrequencyItems!=null && this.selectedfrequencyItems.length!=0)
    {
      let frequencyId = this.selectedfrequencyItems[0].Frequency_Id;
      if (((this.frequencyList != null && this.frequencyList != undefined) && (frequencyId.startsWith('s') || frequencyId == 28 || this.frequencyList.find(f => f.Frequency_Id == frequencyId).Frequency_PRN == 1)) && ((frequencyId != 12) && (frequencyId != 13))) {
        // this.isShiftSchedule = true;
        // this.isHoursReadOnly = true;
        this.selectedstItems = [];
        this.scheduleform.patchValue({
          either: this.selectedstItems,
          hours: '',
        });
        this.timesArray = [];
      }
      else {
        this.isShiftSchedule = false;
        this.isHoursReadOnly = false;
        this.isWeeksDisabled=false;
        this.isdaysDisabled=false;
        //this.ng4LoadingSpinnerService.show();
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.dataservice.get<any>(this.config.Emar_Orders_GetNurseFrequencyDropSelect + frequencyId + '/' + this.nurseStationId)
          .subscribe(res => {
            this.fetchFrequencyMapData(frequencyId, res);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            //this.ng4LoadingSpinnerService..hide();
          }, error => {
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            //this.ng4LoadingSpinnerService..hide();
          });
      }

    }
    //this.ng4LoadingSpinnerService..hide();
  }
  insertOrderHoldDetails() {
    var dt1 = this.orderholdform.value.orderHoldFormDate;
    var dt2 = this.orderholdform.value.orderHoldToDate;
    if (dt1 > dt2) {
      this.alertService.warn("End date cannot be before start date");
      this.holdReleaseDate="";
      //this.ng4LoadingSpinnerService..hide();
    }
    else if (dt2 < dt1) {
      this.alertService.warn("Start date cannot be after end date");
      this.holdReleaseDate="";
      //this.ng4LoadingSpinnerService..hide();
    }
    else
    {
    this.holdReleaseDate=new Date((new Date(dt2)).getTime() + (60*60*24*1000));
    this.orderholdobj = {
      OrderHold_Id: 0,
      PQuantity_Id: this.quantityId,
      HoldFrom: this.dateFormatPipe.transform(this.orderholdform.value.orderHoldFormDate),
      HoldTo: this.dateFormatPipe.transform(this.orderholdform.value.orderHoldToDate),
      HoldReason: this.orderholdform.value.orderHoldReason,
      OrderHold_Status: 1,
      OrderHold_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      OrderHold_CreatedDate: this.dateFormatPipe.dateWithTime(new Date())

    };
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_InsertOrderHoldDetails, this.orderholdobj)
      .subscribe(res => {
        this.modalholdIsOpen = false;
        this.alertService.success("Hold order successful");
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.getOrderGridData('Active', 1);
        this.getAllBarcodes();
        this.GetResidentAllOrdersData();
      }, error => {
        this.modalholdIsOpen = false;
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
    this.resetScreen();
    }
  }
  resetScreen() {
    this.orderholdform.reset();
    this.orderholdform.patchValue({
      orderHoldFormDate: '',
      orderHoldToDate: '',
      orderHoldReason: '',

    });

  }

  insertHoa() {
debugger

    if (this.residentId > 0) {
      if (this.demographicInfoData.PVisit_Status == 2) {
        this.alertService.warn("Resident has been discharged.")
      }
      else if (this.demographicInfoData.PVisit_Status == 3) {
        this.alertService.warn("Status updated to temporarily inactive.")
      }
      else if((this.scheduleform.value.frequency!=null && this.scheduleform.value.frequency!=undefined) && this.FrequnceLimitFlag > this.timesArray.length)
      {
          this.FrequnceLimitAlertText1 = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Descalert1;
          this.alertService.warn(this.FrequnceLimitAlertText1);
      }
      // else if((this.scheduleform.value.frequency!=null && this.scheduleform.value.frequency!=undefined) && this.scheduleform.value.frequency[0].Frequency_Id==9 && this.timesArray.length<2)
      // {
      //   this.FrequnceLimitAlertText1 = this.frequencyList.find(f=>f.Frequency_Id== this.selectedfrequencyItems[0].Frequency_Id).Freq_Descalert1;
      //     this.alertService.warn(this.FrequnceLimitAlertText1);
      // }
      else {
        //this.ng4LoadingSpinnerService.show();
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
            this.SchFlag=false;
            this.insertScheduleTimes();
          }
        }
        //if frequency is selected and it is not PRN and not shifts
        else if ((this.frequencyList!=null && this.frequencyList!=undefined) && this.frequencyList.find(f=>f.Frequency_Id==this.scheduleform.value.frequency[0].Frequency_Id).Frequency_PRN != 1 && this.scheduleform.value.frequency[0].Frequency_Id.startsWith('s') == false&& this.scheduleform.value.frequency[0].Frequency_Name.startsWith('QShift') == false) {
          if ((this.scheduleform.value.either == undefined || this.scheduleform.value.either.length == 0) && ((this.scheduleform.value.hours == null || this.scheduleform.value.hours == "") || (this.timesArray.length == 0 || this.timesArray == null))) {
            this.alertService.warn("Please select start time.");
          }
          else if (this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null && this.timesArray.length > 1) {
            this.alertService.error("At once hours and multiple time can't insert.");
          }
          else {
            this.SchFlag=false;
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
        //   //this.ng4LoadingSpinnerService..hide();
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
        //this.ng4LoadingSpinnerService..hide();
      }
    }
    else
      this.alertService.error("No Resident selected");
  }
  insertScheduleTimes(controlSubstanceFlag?:number) {

    this.times = '';
    if (this.timesArray.length != 0) {
      this.timesArray.forEach(element => {
        this.times += element.Hour_Id + ",";
      });
      this.times = this.times.substring(0, this.times.length - 1);
    }
    else if (this.timesArray.length == 0) {
      if ((this.timesArray.length == 0 || this.timesArray == null) && (this.scheduleform.value.either == '' || this.scheduleform.value.either == null || this.scheduleform.value.either == undefined) && ((this.scheduleform.value.frequency != undefined || this.scheduleform.value.frequency != null || this.scheduleform.value.frequency.length != 0))) {
        if (this.scheduleform.value.frequency!=undefined && this.scheduleform.value.frequency!=null && this.scheduleform.value.frequency.length!=0 && this.frequencyList.find(f=>f.Frequency_Id==this.scheduleform.value.frequency[0].Frequency_Id).Frequency_PRN == 1)
          this.times = '';
      }
      else {
        this.times = this.scheduleform.value.either[0].Hour_Id;
      }
    }
    let nurseStationId = this.selectedNursestation[0].NurseStation_Id;
    let freqId = this.scheduleform.value.frequency == undefined || this.scheduleform.value.frequency.length == 0 || this.scheduleform.value.frequency == null ? null : this.scheduleform.value.frequency[0].Frequency_Id;
    if ((this.frequencyList!=null && this.frequencyList!=undefined) && this.frequencyList.find(f=>f.Frequency_Id==freqId )!=undefined && this.frequencyList.find(f=>f.Frequency_Id==freqId ).Frequency_PRN == 1) {
      this.myform.patchValue({
        prn: true
      });
      const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
        Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        Validators.pattern(/^\d*(\.\d{0,3})?$/),
        Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        maxpervalidation.updateValueAndValidity();
     }
      else
      {
        // this.myform.patchValue({
        //   prn:false,
        // });
        const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
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
      hours: null,//this.scheduleform.value.hours=='' || this.scheduleform.value.hours==0?null:this.scheduleform.value.hours,
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
    if (this.newOrderFlag != 1) {

      let orderFreqId =(this.scheduleTextObj==undefined || this.scheduleTextObj==null)?0: (this.scheduleTextObj.NurseShiftsId != undefined && this.scheduleTextObj.NurseShiftsId != null && this.scheduleTextObj.NurseShiftsId != "" ? ("s"+this.scheduleTextObj.NurseShiftsId) : (this.scheduleTextObj.NursingFreqId != undefined && this.scheduleTextObj.NursingFreqId != null && this.scheduleTextObj.NursingFreqId != 0) ? this.scheduleTextObj.NursingFreqId : 0);
      let freqId = this.scheduleform.value.frequency == undefined || this.scheduleform.value.frequency.length == 0 || this.scheduleform.value.frequency == null ? 0 : this.scheduleform.value.frequency[0].Frequency_Id;
      let orderType = (this.myform.value.literal == true && this.myform.value.treatment == true) ? 2 : (this.myform.value.literal == true && this.myform.value.treatment == false) ? 4 : (this.myform.value.literal == false && this.myform.value.treatment == true) ? 5 : 1;

   var OrderFreqnceGroup  = "";
   if(freqId != null && freqId != undefined && freqId != "")
   {

        OrderFreqnceGroup = this.frequencyList.filter(U => U.Frequency_Id == freqId)[0].Freq_Group;

      }
      // if (this.OrderFrequncryGroup != OrderFreqnceGroup && orderType != 4 && orderFreqId.toString() != freqId.toString() && (this.fieldsChangesDcFlag == "" || this.fieldsChangesDcFlag != "NO")) {
      if (this.OrderFrequncryGroup != OrderFreqnceGroup && orderType != 4 && orderFreqId.toString() != freqId.toString() && (this.fieldsChangesDcFlag == "" || this.fieldsChangesDcFlag != "NO") && ((orderFreqId.toString() != '12' && freqId.toString() != '1') || (orderFreqId.toString() != '13' && freqId.toString() != '1'))) {

        this.modalFieldsChangesDcConfirmation = true;
      }
      else{
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.dataservice.post(this.config.Emar_Orders_InsertupdateHOA, this.hoaObj)
          .subscribe(res => {
            if (res == 2) {
              this.modalFieldsChangesDcConfirmation = true;
            }
            else if (res != null) {
              if (((orderFreqId.toString() == '12' && freqId.toString() == '1') || (orderFreqId.toString() == '13' && freqId.toString() == '1'))) {
                
                this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);

                this.getOrderGridData("Active", 1);
              }
              setTimeout(() => {

                this.modalHOAIsOpen = false;
                this.hoaObj = null;
                this.fieldsChangesDcFlag = "";
                this.alertService.success("Schedule times updated successfully");
                this.OrderFrequncryGroup = "";
                this.myform.patchValue({
                  schduleText: res,
                });

                this.btnHoaSaveFlag = 1;
                this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);

                if (this.myform.value.prn == true && this.newOrderFlag != 1 && this.scheduleSave == 2) {
                  this.scheduleSave = 1;
                  this.saveExistingOrder(controlSubstanceFlag);
                }
              }, 0);
            }
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          }, error => {
            this.modalHOAIsOpen = false
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
        });
    }
  }
    else {
      this.spinnerLoading++;
            this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_GetScheduledTimeText, this.hoaObj)
        .subscribe(res => {
          if (res != null) {
            let text = res;
            this.myform.patchValue({
              schduleText: text
            });
            this.modalHOAIsOpen = false
          }
          this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
        },
          error => {
            this.alertService.error(error.message);
            //this.ng4LoadingSpinnerService..hide();
            this.modalHOAIsOpen = false;
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          });
      // this.myform.patchValue({
      //   schduleText: this.scheduleform.value.either.length == 0 ? '' : this.scheduleform.value.either[0].Hour_Desc
      // });
      this.modalHOAIsOpen = false;
    }
    //this.ng4LoadingSpinnerService..hide();
  }
  // onNurseStationSelect(item: any) {

  //     this.patientIdstatus = 0;
  //     this.changeNurseStation(item);
  //     this.pendingReviewCount = 0;
  // }
  onNurseStationSelect(item: any) {
    // console.log(this.demographicInfoData , "demo ata")
    let lastNurseStation = this.demographicInfoData.NursingStationId;

    if (this.valueChangesFlagReceive == 1) {
      const modalRefs = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRefs.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          this.selectedNursestation = this.nurseStations.filter(item => item.NurseStation_Id == lastNurseStation);
          modalRefs.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          this.patientIdstatus = 0;
          this.changeNurseStation(item);
          this.pendingReviewCount = 0;
        }
        modalRefs.close();
      });
    }else{
      this.patientIdstatus = 0;
      this.changeNurseStation(item);
      this.pendingReviewCount = 0;
    }
    
   

  }
  onNurseStationDeSelect(item: any) {
      this.clearData();
      this.alertService.warn("Select nursing station to view resident list");
    //this.changeNurseStation(item);
    ////this.ng4LoadingSpinnerService..hide();
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

      //this.ng4LoadingSpinnerService.show();
      this.getDemographicInfoByNurseStation(item.NurseStation_Id);
      //this.getAllFlagsForCompanyByNSId(item.NurseStation_Id);
      this.GetPhysicianDropData(item.NurseStation_Id);
      //this.ng4LoadingSpinnerService..hide();
    }
  }
  getDemographicInfoByNurseStation(stationId: number) {
    this.nurseStationId = stationId;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetResidentsByNurseStationId + stationId)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      if(res.length == 0)
      {


      this.route.navigate(['/home/ordergrid']);
      return false;


      }
        this.residents = res;

        if (res.length > 0 && this.patientIdstatus != 1) {
          this.residentId = this.residents.filter(r=>r.PVisit_Status==1)[0].Patient_Id;
          this.getAllFlagsForCompanyByNSId(stationId,this.residentId);
          this.sharedService.changePatientId(this.residentId);
          this.selectedResItem = this.residents.filter(r => r.Patient_Id === this.residentId);
          this.getDemographicInfoData();
          if(this.orderId==0 && this.quantityId==0 && this.newResOrderFlag==true)
          {
            this.orderGridData=[];
            this.getFavouritesMasterData();
            this.newOrder();
            this.getOrderGridData("Active", 1);
            this.getAllBarcodes();
            this.GetResidentAllOrdersData();
          }
          else{
          this.getOrderGridData("Active", 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
          }
          this.myform.patchValue({
            ddlresidents: this.selectedResItem,
          });
        }
        else
        {

          this.getAllFlagsForCompanyByNSId(stationId);
          this.clearData();
        }
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
    //this.ng4LoadingSpinnerService..hide();
  }
  clearData(clearType?:any) {
    if(clearType==undefined)
    {
    this.residents = [];
    this.selectedResItem = [];
    this.residentId = 0;
    }
    const drugnameValidations = this.myform.get('drug');
    drugnameValidations.setValidators([Validators.required, Validators.maxLength(120), Validators.pattern(this.config.drugPattern)]);
    drugnameValidations.updateValueAndValidity();
    const barcodevalidation = this.myform.get('barcode');
    barcodevalidation.setValidators([Validators.required, Validators.maxLength(150)]);
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
    this.orderId = 0;
    this.quantityId = 0;
    this.ordersInfo = {} as any;
    this.orderGridData = [];
    this.demographicInfoData = {} as DemographicInfo;
    this.newResOrderFlag=false;
    this.patientTypeList=[];
    this.pendingReviewCount = 0;
    this.residentAllergies = '';
    this.residentDiagnosis = '';
    this.mergeFlag = 0;
    this.reviewFlag = 0;
    this.demographicInfoData.HomeFlag = 0;
    this.destroyform.reset();
    this.newOrderFlag = 1;
    this.reviewClickedFlag = 0;
    this.favstatus = 0;
    this.splits=0;
    this.reactivateStatus=0;
    this.favForm.reset();
    this.selectedphyItems = [];
    this.selectedroItems = [];

    if (this.defaultPhysicianNPI != null && this.residents.length > 0 && (this.physiciansdrop!=null && this.physiciansdrop!=undefined)) {
      let checkExist=this.physiciansdrop.find(p => p.PhysicianNPI == this.defaultPhysicianNPI)

      if(checkExist!=undefined)
      {
        // debugger
      this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);

      }
   }
    this.myform.patchValue({
      physician: this.selectedphyItems,
      route: this.selectedroItems,
      qtyHand: '',
      drug: '',
      dose: '',
      refill: '',
      addInst: '',
      insulincomments: '',
      barcode: '',
      startDate: '',
      maxPerDay: '',
      alertText: '',
      endDate: '',
      type: 1,
      schduleText: '',
      self: '',
      treatment: '',
      literal:'',
      prn: '',
      controlSubstance: '',
      orderTypeId:1,
    });
    this.barcodear = [];
    this.isReadOnly = false;
    this.ordersDetails = {} as OrdersData;
    this.selectedOrderDetails={};
    this.fieldsChangesDcFlag="";
    this.selectedOrder = 0;
    this.selectedOrderQuantity = 0;
    this.selectedOrderDADminId = 0;
    this.stockId = 0;
    this.gpiCode = '';
    this.resetScheduleForm();
  }
  Updatedrfirstacknowledge(drOrderId: number) {
    this.DrfirstOrderXMLObj = {
      DrFirstOrderId: drOrderId,
      DrFirstOrderXMLTransApproval: 1,
      DrFirstOrderXMLTransApprovalBy: this.userID,
      DrFirstOrderXMLTransApprovalOn: this.dateFormatPipe.dateFormat(new Date()),
    };
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_UpdateDrFirstOrderAcknowledge, this.DrfirstOrderXMLObj)
      .subscribe(res => {
        this.alertService.success("Acknowledged successfully");
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.getDrFirstOrderCheck();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  mergeOrder() {
    if (this.valueChangesFlagReceive == 1) {
      const modalRefs = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRefs.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRefs.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive=0;
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          const modalRef = this.modalService.open(MergeordersComponent, { size: 'lg', windowClass: '' });
          modalRef.componentInstance.title = 'Merge Orders';
          let mergeData = {
            "selectedOrderId": this.orderId,
            "selectedPatientId": this.residentId,
            "selectedQuantityId": this.quantityId,
            "userId":this.userID
          }
          modalRef.componentInstance.mergeChanges = mergeData;
          modalRef.componentInstance.mergeResult.subscribe((receivedResult) => {
            if (receivedResult > 0) {
              this.alertService.success("Merged successfully");
              this.getOrderGridData("Active", 1);
              this.getAllBarcodes();
              this.GetResidentAllOrdersData();
            }
            modalRef.close();
          })
        }
        modalRefs.close();
      });
    }
    else {
      const modalRef = this.modalService.open(MergeordersComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.title = 'Merge Orders';
      let mergeData = {
        "selectedOrderId": this.orderId,
        "selectedPatientId": this.residentId,
        "selectedQuantityId": this.quantityId,
        "userId":this.userID
      }
      modalRef.componentInstance.mergeChanges = mergeData;
      modalRef.componentInstance.mergeResult.subscribe((receivedResult) => {
        if (receivedResult > 0) {
          this.alertService.success("Merged successfully");
          this.getOrderGridData("Active", 1);
          this.getAllBarcodes();
          this.GetResidentAllOrdersData();
        }
        modalRef.close();
      })
    }

  }
  //#endregion
  getAllFlagsForCompanyByNSId(stationId: number,residentId?:any) {
    let resId=residentId==undefined?0:residentId;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId+"/"+ resId)
      .subscribe(res => {
        this.drFirstFlag = res.DrFirstRequired;
        this.defaultPhysicianNPI = res.PhysicianNPI;
        this.getNursingStationTimeZone(stationId);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
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

      if ((searchText!=null && searchText.length > 2 && this.myform.value.type==true)||(this.myform.value.type == false)) {
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
      this.myform.patchValue({
        drug: item.DrugName,
       // qtyHand: '',
        //barcode:item.Barcode
        //route: this.selectedroItems,
        controlSubstance: item.ControlledSubstanceSchedule == 1 ? true : false,
        type: 1
      });
      this.isReadOnlyforControl = false;
      if (item.ControlledSubstanceSchedule == 1)
      {
        this.isControlSubstanceReadOnly = true;
        const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.maxLength(7),  Validators.pattern(/^[1-9]{1,3}(?:\.[0-9]{1,3})?$/),
        Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        Validators.pattern(/^\d*(\.\d{0,3})?$/),
        Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        maxpervalidation.updateValueAndValidity();
      }
      else
      {
        this.isControlSubstanceReadOnly = false;
        if( this.myform.value.prn !=true)
        {
        const maxpervalidation = this.myform.get('maxPerDay');
        maxpervalidation.setValidators([Validators.maxLength(6),  Validators.pattern(/^[1-9]{1,2}(?:\.[0-9]{1,3})?$/),
        Validators.pattern(/^(?!.*\.\.)\d+(\.\d{0,3})?$/),
        Validators.pattern(/^\d*(\.\d{0,3})?$/),
        Validators.pattern(/^\d+(\.\d{0,3})?$/)]);
        maxpervalidation.updateValueAndValidity();
       }
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
      qtyHand: '',
      barcode: ''
      //route: this.selectedroItems,
    });
    this.barcodear = [];
    this.gpiCode = '';
    this.drugNameChanged = true;
    this.stockId = 0;
    this.isReadOnlyforControl = true;
    this.loadSearchData();
  }
  back() {
    //localStorage.setItem("ordersBackClick", JSON.stringify(true));
    if (this.valueChangesFlagReceive == 1 && this.newOrderFlag!=1 && this.discardChanges==true) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
          this.resetCheckbox = false;
          localStorage.setItem("ordersBackClick", JSON.stringify(true));
          this.route.navigate(['/home/ordergrid']);
        }
        modalRef.close();
      });
    }
    else {
      localStorage.setItem("ordersBackClick", JSON.stringify(true));
      this.route.navigate(['/home/ordergrid']);
    }
  }
  addTimes() {
    debugger
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

          if( this.FrequnceLimitFlag != null && (this.FrequnceLimitFlag <= this.timesArray.length))
          {
            this.alertService.warn(this.FrequnceLimitAlertText);

            this.timeform.reset();

          }
          else{


          this.timesArray.push(timeObj);

          }
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
    debugger
    if (this.scheduleform.value.either == '' || this.scheduleform.value.either == null || this.scheduleform.value.either == undefined) {
      this.alertService.warn("Please select start time.");
    }
    // else if ((this.scheduleform.value.hours != '' && this.scheduleform.value.hours != null) && (this.scheduleform.value.either != undefined || this.scheduleform.value.either != "0")) {
    //   this.alertService.warn("Hours already given. You can't give multiple times.");
    // }
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
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Resident_Demographic_GetPatientType + this.residentId)
      .subscribe(res => {
        this.patientTypeList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
  }
  barcodeModalOpen()
  {
    this.modalBarcodeIsOpen=true;
  }
  modalBarcodeIsClose()
  {
    this.modalBarcodeIsOpen=false;
    this.myform.controls["barcode"].reset();
  }
  resetScheduleTimes()
  {
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
      weekId: null ,
      monthId: null,
      days: null,
      createdby: this.userID,
      activedays: null,
      holddays: null,
      NurseStationId: this.nurseStationId,
      nurseShiftId:  null
    };
    if (this.newOrderFlag != 1) {
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_InsertupdateHOA, this.hoaObj)
        .subscribe(res => {

          if (res != null) {
            this.modalHOAIsOpen = false;
            this.hoaObj = null;
            this.OrderFrequncryGroup = ""
            this.alertService.success("Schedule times updated successfully");
            this.myform.patchValue({
              schduleText: res,
              prn:false
            });
          }
           this.getOrderDetailsbyorderId(this.orderId, this.quantityId, this.dAdminId);

          this.scheduleTextObj=null;
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();

        }, error => {
          this.modalHOAIsOpen = false
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService..hide();
        });
    }
    else {
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_Orders_GetScheduledTimeText, this.hoaObj)
        .subscribe(res => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          if (res != null) {
            let text = res;
            this.myform.patchValue({
              schduleText: text,
              prn:false
            });
            this.modalHOAIsOpen = false
          }
        },
          error => {
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
            //this.ng4LoadingSpinnerService..hide();
            this.modalHOAIsOpen = false
          });
      this.modalHOAIsOpen = false;
    }
    this.resetScheduleForm();
  }
  closePRNModel()
  {
    this.prnSchedleCheckModal=false;
  }
  getOrderHoldFromToDates(qtyId: number) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_Orders_GetOrderHoldData + qtyId)
      .subscribe(res => {
        this.orderHoldData = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.orderReleaseDate=new Date((new Date(res.HoldTo)).getTime() + (60*60*24*1000));
        if (res.HoldReason.length > 30)
          this.holdReason = res.HoldReason.substr(0, 30);
        else
          this.holdReason = res.HoldReason;
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  checkDestroyQuantity()
  {
    let destroyQty=this.destroyform.value.quantity;
    if (parseFloat(destroyQty) ==0)
    {
      this.destroyform.patchValue({
        quantity:'',
      });
    }
  }
  holdDatesCheck()
  {
    if ((this.orderholdform.value.orderHoldFormDate != undefined && this.orderholdform.value.orderHoldFormDate != null && this.orderholdform.value.orderHoldFormDate != "") && (this.orderholdform.value.orderHoldToDate != undefined && this.orderholdform.value.orderHoldToDate != null && this.orderholdform.value.orderHoldToDate != "")) {
      var dt1 = this.orderholdform.value.orderHoldFormDate;
      var dt2 = this.orderholdform.value.orderHoldToDate;
      if (dt1 > dt2) {
        this.alertService.warn("End date cannot be before start date");
        this.holdReleaseDate="";
        //this.ng4LoadingSpinnerService..hide();
      }
      else if (dt2 < dt1) {
        this.alertService.warn("Start date cannot be after end date");
        this.holdReleaseDate="";
        //this.ng4LoadingSpinnerService..hide();
      }
      else
      {
        var conDate=this.dateFormatPipe.transform(dt2);
        this.holdReleaseDate=new Date((new Date(conDate)).getTime() + (60*60*24*1000));
      }
    }
  }
  closeDrFirstModel()
  {
    this.drFirstModal=false;
  }
  opendrFirst()
  {
    this.drFirstModal=true;
  }
  // Mutiple orders hold, release hold, dc, reactivate region
  GetResidentAllOrdersData() {
    //this.ng4LoadingSpinnerService.show();
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetOrderGridData + this.residentId + "/" + "AllActive")
      .subscribe(res => {
        this.updateOrdersRecords=[];
        this.holdObj=null;
        this.dcObj=null;
        this.noDCSplitsFlag=0;
        this.residentAllOrders = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  // onCheckHold(event:any,item:any)
  // {

  //   let holdFlag=event==true?1:0;
  //   var record=this.residentAllOrders.find(re=>re.porder_Id==item.porder_Id && re.PQuantity_Id==item.PQuantity_Id);
  //   if(record.HoldStatus==holdFlag)
  //   {
  //       let holdDate=this.dateFormatPipe.transformISODate(record.HoldStatus==0?'':record.On_Hold_Until);
  //       let holdDateId="#holddate"+item.PQuantity_Id;
  //       $(holdDateId).val(holdDate);
  //       let holdChk="#holdchk"+item.PQuantity_Id;
  //       $(holdChk).prop("checked",record.HoldStatus==0?false:true);

  //     var checkHold=this.updateOrdersRecords.find(re=>re.PorderId==item.porder_Id && re.PQuantityId==item.PQuantity_Id);
  //     if(checkHold==undefined)
  //     {
  //       let obj=
  //       {
  //         PorderId:item.porder_Id,
  //         PQuantityId:item.PQuantity_Id,
  //         PatientId:this.residentId,
  //         HoldChangeFlag:0,
  //         HoldStatus:0,
  //         DcChangeFlag:0,
  //         DcStatus:0,
  //         DcsPlit:0,
  //         UpdatedBy:this.userID,
  //         UpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
  //       }

  //       this.updateOrdersRecords.push(obj);
  //       var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //       if (changesRecords.length > 0) {
  //         this.valueChangesFlagReceive = 1;
          
  //           this.sharedService.saveChangesOrderInfo(1);
          
  //       }
  //     }
  //     else
  //     {

  //       checkHold.HoldChangeFlag=0,
  //       checkHold.HoldStatus=0,
  //       this.updateOrdersRecords.push();
  //       var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //       if (changesRecords.length > 0) {
  //         this.valueChangesFlagReceive = 1;
        
  //         this.sharedService.saveChangesOrderInfo(1);
    
        
  //       }
  //     }
  //   }
  //   else if(record.HoldStatus!=holdFlag){

  //     if(event==true)
  //     {
  //       // this.isHoldChecked=true;
  //       // this.checkedPorderId=item.porder_Id;
  //       // this.checkedPquantityId=item.PQuantity_Id;
  //       // this.holdReleaseDate="";
  //       // this.modalholdIsOpen=true;
  //       var checkHold=this.updateOrdersRecords.find(re=>re.PorderId==item.porder_Id && re.PQuantityId==item.PQuantity_Id);
  //   if(checkHold==undefined)
  //   {
  //     let obj=
  //     {
  //       PorderId:item.porder_Id,
  //       PQuantityId:item.PQuantity_Id,
  //       PatientId:this.residentId,
  //       HoldChangeFlag:1,
  //       HoldStatus:1,
  //       DcChangeFlag:0,
  //       DcStatus:0,
  //       DcsPlit:0,
  //       UpdatedBy:this.userID,
  //       UpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
  //     }

  //     this.updateOrdersRecords.push(obj);
  //     this.orderholdform.reset();
  //     this.orderholdform.patchValue({
  //     orderHoldFormDate: '',
  //     orderHoldToDate: '',
  //     orderHoldReason: '',
  //   });

  //     var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //     if (changesRecords.length > 0) {
  //       this.valueChangesFlagReceive = 1;
  //       this.sharedService.saveChangesOrderInfo(1);

  //     }
  //   }
  //   else
  //   {

  //     checkHold.HoldChangeFlag=1,
  //     checkHold.HoldStatus=1,
  //     this.updateOrdersRecords.push();

  //   var changesRecords=this.updateOrdersRecords.filter(up=>up.HoldChangeFlag==1 || up.DcChangeFlag==1);
  //     if (changesRecords.length > 0) {
  //       this.valueChangesFlagReceive = 1;
  //     }
  //   }
  //     }
  //     else{
  //       let holdDate=this.dateFormatPipe.transformISODate('');
  //       let holdDateId="#holddate"+item.PQuantity_Id;
  //       $(holdDateId).val(holdDate);
  //       var checkHold=this.updateOrdersRecords.find(re=>re.PorderId==item.porder_Id && re.PQuantityId==item.PQuantity_Id);
  //       if(checkHold==undefined)
  //       {
  //         let obj=
  //         {
  //           PorderId:item.porder_Id,
  //           PQuantityId:item.PQuantity_Id,
  //           PatientId:this.residentId,
  //           HoldChangeFlag:1,
  //           HoldStatus:2,
  //           DcChangeFlag:0,
  //           DcStatus:0,
  //           DcsPlit:0,
  //           UpdatedBy:this.userID,
  //           UpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
  //         }

  //         this.updateOrdersRecords.push(obj);
  //         var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //         if (changesRecords.length > 0) {
  //           this.valueChangesFlagReceive = 1;
  //           this.sharedService.saveChangesOrderInfo(1);

  //         }
  //       }
  //       else
  //       {

  //         checkHold.HoldChangeFlag=1,
  //         checkHold.HoldStatus=1,
  //         this.updateOrdersRecords.push();
  //         var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
  //         if (changesRecords.length > 0) {
  //           this.valueChangesFlagReceive = 1;
  //           this.sharedService.saveChangesOrderInfo(1);

  //         }
  //       }
  //     }
  //   }
  //   if (this.discardChanges ) {
  //     this.sharedService.saveChangesOrderInfo(1);

  //   } else {
  //     this.sharedService.saveChangesOrderInfo(0);

  //   }

  // }
  onCheckDC(event:any,item:any)
  {
    let isChecked = event == true
    this.profileDCChangesCheckState[item.PQuantity_Id] = isChecked;
    this.rightGridDCChangesFlag = Object.values(this.profileDCChangesCheckState).some((state) => state);
    let dcFlag = event == true ? 1 : 0;
    var record = this.residentAllOrders.find(re => re.porder_Id == item.porder_Id && re.PQuantity_Id == item.PQuantity_Id);
    let isRecordDc = record.POrder_Status != 1 ? 1 : 0;

    let holdChk="#holdchk"+item.PQuantity_Id;
    let holdFlag=$(holdChk).prop("checked")==true?1:0;
    if(event==false && $(holdChk).prop("checked")==false)
    {
      $(holdChk).prop("disabled",false);
      this.onCheckHold(false,item);
    }
    if(event==true && record.HoldStatus!=holdFlag)
      {
        let holdChk="#holdchk"+item.PQuantity_Id;
        $(holdChk).prop("checked",false);
        $(holdChk).prop("disabled",true);
        this.onCheckHold(false,item);
      }
    if(isRecordDc==dcFlag)
    {
      let dcChk="#dcchk"+item.PQuantity_Id;
      $(dcChk).prop("checked",record.POrder_Status!=1?true:false);
      var checkDc=this.updateOrdersRecords.find(re=>re.PorderId==item.porder_Id && re.PQuantityId==item.PQuantity_Id);
      if(checkDc==undefined)
      {
        let obj=
        {
          PorderId:item.porder_Id,
          PQuantityId:item.PQuantity_Id,
          PatientId:this.residentId,
          HoldChangeFlag:0,
          HoldStatus:0,
          DcChangeFlag:0,
          DcStatus:0,
          DcsPlit:0,
          UpdatedBy:this.userID,
          UpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
        }

        this.updateOrdersRecords.push(obj);

        var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);

        if (changesRecords.length > 0) {
          this.valueChangesFlagReceive = 1;
        } else {
          this.valueChangesFlagReceive = 0;
        }
      }
      else{

        checkDc.DcChangeFlag=0,
        checkDc.DcStatus=0,
        checkDc.DcsPlit=0,
        this.updateOrdersRecords.push();
        var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
        if (changesRecords.length > 0) {
          this.valueChangesFlagReceive = 1;
        } else {
          this.valueChangesFlagReceive = 0;
        }
      }
    }
    else if(isRecordDc!=dcFlag)
    {
      if(event==true)
      {
        // this.isDcChecked=true;
        // this.checkedPorderId=item.porder_Id;
        // this.checkedPquantityId=item.PQuantity_Id;
        var checkDc=this.updateOrdersRecords.find(re=>re.PorderId==item.porder_Id && re.PQuantityId==item.PQuantity_Id);
        if(checkDc==undefined)
        {
          let obj=
          {
            PorderId:item.porder_Id,
            PQuantityId:item.PQuantity_Id,
            PatientId:this.residentId,
            HoldChangeFlag:0,
            HoldStatus:0,
            DcChangeFlag:1,
            DcStatus:1,
            DcsPlit:item.split,
            UpdatedBy:this.userID,
            UpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
          }

          this.updateOrdersRecords.push(obj);

          var changesRecords=this.updateOrdersRecords.filter(up=>up.HoldChangeFlag==1 || up.DcChangeFlag==1);

          if(changesRecords.length>0)
          {
            this.valueChangesFlagReceive=1;
          } else {
            this.valueChangesFlagReceive = 0;
          }
        }
        else{

          checkDc.DcChangeFlag=1,
          checkDc.DcStatus=1,
          checkDc.DcsPlit=item.split,
          this.updateOrdersRecords.push();
          var changesRecords=this.updateOrdersRecords.filter(up=>up.HoldChangeFlag==1 || up.DcChangeFlag==1);

          if(changesRecords.length>0)
          {
            this.valueChangesFlagReceive=1;
          } else {
            this.valueChangesFlagReceive = 0;
          }
        }
      }
      else{
        var checkDc=this.updateOrdersRecords.find(re=>re.PorderId==item.porder_Id && re.PQuantityId==item.PQuantity_Id);
        if(checkDc==undefined)
        {
          let obj=
          {
            PorderId:item.porder_Id,
            PQuantityId:item.PQuantity_Id,
            PatientId:this.residentId,
            HoldChangeFlag:0,
            HoldStatus:0,
            DcChangeFlag:1,
            DcStatus:2,
            DcsPlit:0,
            UpdatedBy:this.userID,
            UpdatedOn:this.dateFormatPipe.dateWithTime(new Date()),
          }
          this.updateOrdersRecords.push(obj);
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;

          }
        }
        else{

          checkDc.DcChangeFlag=1,
          checkDc.DcStatus=2,
          checkDc.DcsPlit=0,
          this.updateOrdersRecords.push();
          var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
          if (changesRecords.length > 0) {
            this.valueChangesFlagReceive = 1;

          }
        }
      }
    }
    if (this.rightGridDCChangesFlag || this.rightGridOnHoldChangesFlag) {
      this.myform.disable();
      this.valueChangesFlagReceive = 1;
      if (this.rightGridDCChangesFlag) {
        $(holdChk).prop("disabled", true);
      }
    } else {
      this.myform.enable();
      this.valueChangesFlagReceive = 0;
    }
  }
  getSelectedOrderHold()
  {
    var dt1 = this.orderholdform.value.orderHoldFormDate;
    var dt2 = this.orderholdform.value.orderHoldToDate;
    if (dt1 > dt2) {
      this.alertService.warn("End date cannot be before start date");
      //this.ng4LoadingSpinnerService..hide();
    }
    else if (dt2 < dt1) {
      this.alertService.warn("Start date cannot be after end date");
      //this.ng4LoadingSpinnerService..hide();
    }
    else
    {
    // let holdDate=this.dateFormatPipe.transformISODate(dt2);
    // let holdDateId="#holddate"+this.checkedPquantityId;
    // $(holdDateId).val(holdDate);
    this.modalholdIsOpen=false;
    this.holdObj={
      HoldFrom:this.dateFormatPipe.transform(this.orderholdform.value.orderHoldFormDate),
      HoldTo:this.dateFormatPipe.transform(this.orderholdform.value.orderHoldToDate),
      HoldReason:this.orderholdform.value.orderHoldReason,
    }
    this.orderholdform.reset();
      this.orderholdform.patchValue({
      orderHoldFormDate: '',
      orderHoldToDate: '',
      orderHoldReason: '',
    });
    }
    var dcChanges=this.updateOrdersRecords.filter(up=>up.DcChangeFlag==1 && up.DcStatus==1);

    if(dcChanges.length>0 && (this.dcObj==undefined || this.dcObj==null))
        {
          this.isDcChecked=true;
          this.DcForm.reset();
          this.multipleOrdersplits=1;
          this.modalDcConfirmationIsOpen=true;
          setTimeout(() => {
            this.discontinueFocus.nativeElement.focus()
          }, 400);
        }
    else
    {
      this.updateMultipleOrderChanges();
    }
  }
  selectedOrderHoldsCancel()
  {
    // let holdChk="#holdchk"+this.checkedPquantityId;
    // $(holdChk).prop("checked",false);
    this.modalholdIsOpen=false;
    this.orderholdform.reset();
    this.orderholdform.patchValue({
      orderHoldFormDate: '',
      orderHoldToDate: '',
      orderHoldReason: '',
    });
    this.updateOrdersRecords=[];
    this.holdObj=null;
    this.dcObj=null;
    this.noDCSplitsFlag=0;
    this.profileONHoldChangesCheckState= {};
    this.profileDCChangesCheckState = {};
    this.GetResidentAllOrdersData();
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    this.myform.enable();
  }
  multipleOrdersDiscontinueConfirmation()
  {
    this.isDcChecked=true;
    this.modalHistoryIsOpen=false;
    this.DcForm.reset();
    this.multipleOrdersplits=0;
    this.modalDcConfirmationIsOpen=true;
    setTimeout(() => {
      this.discontinueFocus.nativeElement.focus()
    }, 400);
  }
  closeMultpleDcModel()
  {
    // let dcChk="#dcchk"+this.checkedPquantityId;
    // $(dcChk).prop("checked",false);
    this.modalHistoryIsOpen=false;
    this.modalDcConfirmationIsOpen=false;
    this.DcForm.reset();
    this.multipleOrdersplits=0;
    this.updateOrdersRecords=[];
    this.holdObj=null;
    this.dcObj=null;
    this.noDCSplitsFlag=0;
    this.DirectDiscFlag = false;
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    this.myform.enable();
    this.profileONHoldChangesCheckState= {};
    this.profileDCChangesCheckState = {};
    this.GetResidentAllOrdersData();
  }
  multipleOrdersDiscontinue()
  {

    this.modalDcConfirmationIsOpen=false;
    this.dcObj={
      DcReason:this.DcForm.value.DcReason
    };
    this.DcForm.reset();
    this.updateMultipleOrderChanges();
  }
  updateMultipleOrderChanges() {
    debugger;
  
    if (this.rightGridOnHoldChangesFlag || this.rightGridDCChangesFlag) {
      this.myform.disable()
    } else {
      this.myform.enable()

    }
    if (this.updateOrdersRecords.length > 0 && (this.discardChanges == false) && (this.rightGridOnHoldChangesFlag || this.rightGridDCChangesFlag)) {
      var changesRecords = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 || up.DcChangeFlag == 1);
      if (changesRecords.length > 0) {
        var holdChanges = this.updateOrdersRecords.filter(up => up.HoldChangeFlag == 1 && up.HoldStatus == 1);
        var dcChanges = this.updateOrdersRecords.filter(up => up.DcChangeFlag == 1 && up.DcStatus == 1);

        if (holdChanges.length > 0 && (this.holdObj == undefined || this.holdObj == null) && this.rightGridOnHoldChangesFlag) {
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
            this.holdFocus.nativeElement.focus()
          }, 300);
        }
        else if (dcChanges.length > 0 && (this.dcObj == undefined || this.dcObj == null)) {
          this.isDcChecked = true;
          this.DcForm.reset();
          this.multipleOrdersplits = 1;
          this.modalDcConfirmationIsOpen = true;
          setTimeout(() => {
            this.discontinueFocus.nativeElement.focus()
          }, 400);
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
              //this.ng4LoadingSpinnerService.show();
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
              this.spinnerLoading++;
              this.checkAndHideSpinnerLoading();
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
                    this.rightGridOnHoldChangesFlag = false;
                    this.rightGridDCChangesFlag = false;
                    this.myform.enable();
                    this.profileDCChangesCheckState = {};
                    this.profileONHoldChangesCheckState = {};
                    this.GetResidentAllOrdersData();
                    this.getOrderGridData("Active", 1);
                    this.getAllBarcodes();
                    this.GetResidentAllOrdersData();
                    //this.ng4LoadingSpinnerService..hide();
                    this.spinnerLoading--;
                    this.checkAndHideSpinnerLoading();
                  }
                  else {
                    this.alertService.error("Something went wrong");
                    //this.ng4LoadingSpinnerService..hide();
                   
                  }
                  this.spinnerLoading--;
                  this.checkAndHideSpinnerLoading();
                }, error => {
                  this.alertService.error(error.message);
                  this.spinnerLoading--;
                  this.checkAndHideSpinnerLoading();
                });
            }
          }
        }
      }
      else {
        this.alertService.warn("At least one order detail must be changed to save changes");
        //this.ng4LoadingSpinnerService..hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }
    }
    else {
      this.alertService.warn("At least one order detail must be changed to save changes");
      //this.ng4LoadingSpinnerService..hide();
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();
    }

  }
  discardAllOrdersUpdate()
  {
    this.alertService.success("Discarded changes");
    this.updateOrdersRecords=[];
    this.holdObj=null;
    this.dcObj=null;
    this.noDCSplitsFlag=0;
    this.sharedService.saveChangesOrderInfo(0);
    this.valueChangesFlagReceive=0;
    this.GetResidentAllOrdersData();
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    this.myform.enable();
  }
  discontinueSplits(type:number)
  {

    if(type==1)
    {
      var dcChanges=this.updateOrdersRecords.filter(up=>up.DcChangeFlag==1 && up.DcStatus==1 && up.DcsPlit==1);
      dcChanges.forEach((element,index)=> {
        var dcSplits=this.residentAllOrders.filter(re=>re.porder_Id==element.PorderId);
              dcSplits.forEach(ele => {
                this.onCheckDC(true,ele);
              });
          if(dcChanges.length==(index+1))
          {
            this.modalDcAllSplits=false;
            this.noDCSplitsFlag=1;
            this.updateMultipleOrderChanges();
          }
      });

    }
    else if(type==0)
    {
      this.modalDcAllSplits=false;
      this.noDCSplitsFlag=1;
      this.updateMultipleOrderChanges();
    }
  }
  checkOrderHold(item:any)
  {
    if (this.updateOrdersRecords.length > 0) {
      var checkHold = this.updateOrdersRecords.find(re => re.PorderId == item.porder_Id && re.PQuantityId == item.PQuantity_Id && re.HoldChangeFlag == 1);
      if (checkHold != undefined && checkHold.HoldStatus==1) {
        return 1;
      }
      else if(checkHold!=undefined && checkHold.HoldStatus!=1) {
        return 2;
      }
      else if(checkHold==undefined && item.HoldStatus==1)
      {
        return 1;
      }
      else if(checkHold==undefined && item.HoldStatus==0)
      {
        return 2;
      }
    }
    else {
      return 0;
    }
  }
  checkOrderDC(item:any)
  {
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
// Fileds Changes Dc order
checkIsFieldsChanged()
{
  // console.log(this.myform,"myform")
 debugger;
  let orderType=(this.myform.value.literal==true && this.myform.value.treatment==true)?2:(this.myform.value.literal==true && this.myform.value.treatment==false)?4:(this.myform.value.literal==false && this.myform.value.treatment==true)?5:1;
  debugger
  let physicianRecord = this.physiciansdrop.filter(p => p.Physician_Id == this.myform.value.physician[0].Physician_Id)[0];
  let prn=this.myform.value.prn==true?1:0;
  let orderPrn=this.selectedOrderDetails.PRNFlag==true?1:0;
  let refill=this.myform.value.refill==undefined ||this.myform.value.refill==null || this.myform.value.refill==""?0:this.myform.value.refill;
  let orderrefill=this.selectedOrderDetails.Refill==undefined || this.selectedOrderDetails.Refill==null || this.selectedOrderDetails.Refill==""?0:this.selectedOrderDetails.Refill;
  
  if((orderType!=4 && orderType!=2 && (((this.selectedOrderDetails.OrderingPhysicianNPI !=physicianRecord.PhysicianNPI) && !this.adminLoggedin)
  || ((this.selectedOrderDetails.AGiveCodeIdentifier==null? "": this.selectedOrderDetails.AGiveCodeIdentifier) !=this.gpiCode)
  || ((this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id)  && !this.adminLoggedin)
  //|| ((this.selectedOrderDetails.Quantity=="SS" && this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id && this.myform.value.route[0].Route_Id!=38) || (this.selectedOrderDetails.Quantity=="UD" && this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id && (this.myform.value.route[0].Route_Id!=40 && this.myform.value.route[0].Route_Id!=48 )))
  || (parseFloat(orderrefill)!=parseFloat(refill) && !this.adminLoggedin)
  //|| (((((this.selectedOrderDetails.Quantity=="SS" && this.myform.value.route[0].Route_Id!=38) || (this.selectedOrderDetails.Quantity=="UD" && this.myform.value.route[0].Route_Id!=40 && this.myform.value.route[0].Route_Id!=48)) && ((this.selectedOrderDetails.Quantity == 0 || this.selectedOrderDetails.Quantity =='') ? false:this.selectedOrderDetails.Quantity!=this.myform.value.dose))) || (this.selectedOrderDetails.Quantity!="SS" && this.selectedOrderDetails.Quantity!="UD" && parseFloat(this.selectedOrderDetails.Quantity).toFixed(2)!=parseFloat(this.myform.value.dose).toFixed(2)))
  ||
  ((((this.selectedOrderDetails.Quantity == "" || this.selectedOrderDetails.Quantity == 0) ? false :
  (((this.selectedOrderDetails.Quantity == "SS" && this.myform.value.route[0].Route_Id != 38) ||
    (this.selectedOrderDetails.Quantity == "UD" && this.myform.value.route[0].Route_Id != 40 && this.myform.value.route[0].Route_Id != 48)) &&
   (this.selectedOrderDetails.Quantity == 0 || this.selectedOrderDetails.Quantity == '' ? false : this.selectedOrderDetails.Quantity != this.myform.value.dose)) ||
  (this.selectedOrderDetails.Quantity != "SS" && this.selectedOrderDetails.Quantity != "UD" && parseFloat(this.selectedOrderDetails.Quantity).toFixed(2) != parseFloat(this.myform.value.dose).toFixed(2))))  && !this.adminLoggedin)

  || (orderPrn!=prn)))
  || (orderType==2 && ((((this.selectedOrderDetails.OrderingPhysicianNPI!=null && this.selectedOrderDetails.OrderingPhysicianNPI !=physicianRecord.PhysicianNPI) && !this.adminLoggedin) || (this.selectedOrderDetails.OrderingPhysicianNPI==null && this.defaultPhysicianNPI!=physicianRecord.PhysicianNPI))
  || ((this.selectedOrderDetails.AGiveCodeIdentifier==null? "": this.selectedOrderDetails.AGiveCodeIdentifier)!=this.gpiCode)
  || (this.selectedOrderDetails.Route_Id!=this.myform.value.route[0].Route_Id  && !this.adminLoggedin)
  || (parseFloat(orderrefill)!=parseFloat(refill) && !this.adminLoggedin)
  //|| (((((this.selectedOrderDetails.Quantity=="SS" && this.myform.value.route[0].Route_Id!=38) || (this.selectedOrderDetails.Quantity=="UD" && this.myform.value.route[0].Route_Id!=40 && this.myform.value.route[0].Route_Id!=48)) && ((this.selectedOrderDetails.Quantity == 0 || this.selectedOrderDetails.Quantity =='') ? false:this.selectedOrderDetails.Quantity!=this.myform.value.dose))) || (this.selectedOrderDetails.Quantity!="SS" && this.selectedOrderDetails.Quantity!="UD" && parseFloat(this.selectedOrderDetails.Quantity).toFixed(2)!=parseFloat(this.myform.value.dose).toFixed(2)))
  ||
  ((((this.selectedOrderDetails.Quantity !== "" || this.selectedOrderDetails.Quantity == 0) ? false :
  (((this.selectedOrderDetails.Quantity == "SS" && this.myform.value.route[0].Route_Id != 38) ||
    (this.selectedOrderDetails.Quantity == "UD" && this.myform.value.route[0].Route_Id != 40 && this.myform.value.route[0].Route_Id != 48)) &&
   (this.selectedOrderDetails.Quantity == 0 || this.selectedOrderDetails.Quantity == '' ? false : this.selectedOrderDetails.Quantity != this.myform.value.dose)) ||
  (this.selectedOrderDetails.Quantity != "SS" && this.selectedOrderDetails.Quantity != "UD" && parseFloat(this.selectedOrderDetails.Quantity).toFixed(2) != parseFloat(this.myform.value.dose).toFixed(2))))  && !this.adminLoggedin)

  || (orderPrn!=prn)))
  || (orderType==4 && (((this.selectedOrderDetails.OrderingPhysicianNPI!=null && this.selectedOrderDetails.OrderingPhysicianNPI !=physicianRecord.PhysicianNPI) && !this.adminLoggedin) || (this.selectedOrderDetails.OrderingPhysicianNPI==null && this.defaultPhysicianNPI!=physicianRecord.PhysicianNPI))))

  {
    this.fieldsChangesDcFlag="";


    return 1;
  }
  else {
    this.fieldsChangesDcFlag="No Changes";
    return 2;
  }
}
openFiledsChangesDcConfirmaton()
{
  this.modalFieldsChangesDcConfirmation=true;
}
fileldsChangesDcOrder(type: any) {

  if (type == 1) {
    this.fieldsChangesDcFlag = "YES";
    this.modalFieldsChangesDcConfirmation = false;
    this.discardChanges = false;
    this.rightGridDCChangesFlag = false;
    this.rightGridOnHoldChangesFlag = false;
    if (this.modalHOAIsOpen == false) {
      this.orderDiscontinueConfirmation();
    }
    else {
      this.modalHOAIsOpen = false;
      this.hoaObj = null;
      this.orderDiscontinueConfirmation();
    }
  }
  else if (type == 0) {
    this.fieldsChangesDcFlag = "NO";
    this.modalFieldsChangesDcConfirmation = false;
    this.modalHOAIsOpen = false;
    this.hoaObj = null;
    this.BUttonFlag = false;
    this.discardChanges = false;
    this.getOrderGridData("Active", 1);
    this.getAllBarcodes();
    this.GetResidentAllOrdersData();
  }
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
cpoeNewOrder() {
  if (this.valueChangesFlagReceive == 1 && this.residentAllOrders.length != 0) {
    const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.result.subscribe((receivedResult) => {
      if (receivedResult == 2) {
        modalRef.close();
      }
      else if (receivedResult == 1) {
        this.sharedService.saveChangesOrderInfo(0);
        this.valueChangesFlagReceive=0;
        this.rightGridDCChangesFlag = false;
          this.rightGridOnHoldChangesFlag = false;
          this.profileDCChangesCheckState={};
          this.profileONHoldChangesCheckState={};
        localStorage.setItem("OrdersGridResType", JSON.stringify(JSON.parse(localStorage.getItem("OrdersGridResType"))));
        localStorage.setItem("FromScreen", JSON.stringify("Orders"));
        this.sharedService.changePatientId(this.residentId);
        this.sharedService.changeOrderId(0);
        this.sharedService.changeQuantityId(0);
        this.route.navigate(['/home/orderinfocpoe']);
      }
      modalRef.close();
    });
  }
  else{
  localStorage.setItem("OrdersGridResType", JSON.stringify(JSON.parse(localStorage.getItem("OrdersGridResType"))));
  localStorage.setItem("FromScreen", JSON.stringify("Orders"));
  this.sharedService.changePatientId(this.residentId);
  this.sharedService.changeOrderId(0);
  this.sharedService.changeQuantityId(0);
  this.route.navigate(['/home/orderinfocpoe']);
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
        dose:this.newOrderFlag!=1 && this.selectedOrderDetails.Quantity!=undefined && this.selectedOrderDetails.Quantity!=null?this.selectedOrderDetails.Quantity:'',
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
mouseEnter(Id:any)
{

  this.MyImages = Id;

}
mouseLeave()
{
  this.MyImages =null;

}
barCodeChange()
{

debugger
  let num = this.barcodear;
  let num2 = this.myform.value.pbarcode.split(',');
  let flag=false;
  num2 = num2.filter(item => item.trim() !== "");
  //let result = num2.filter(o1 => !num.some(o2 => o1.id.trim() === o2.id.trim()));

  let allFounded1=num.every(a=>num2.find(b=> a.trim()==b.trim()));
  let allFounded=num2.filter(a=>num.find(b=> a.trim()==b.trim()));

// if(num2.length!=allFounded.length)
// {
//   this.discardChanges=true;
// }
// else
// {
//   this.discardChanges=false;
// }
if(allFounded1 &&num2.length ==allFounded.length)
{
  this.discardChanges=false;
}
else{
  this.discardChanges=true;
}
if (this.checkIsFieldsChanged() == 1) {
  this.discardChanges = true;
  this.sharedService.saveChangesOrderInfo(1);
}else{
  this.sharedService.saveChangesOrderInfo(0);

}
//   num2.forEach(function (value2) {
//     num.forEach(function (value) {
//      // if(value2.indexOf(value) >= 0){
//         // console.log("found");
//         // this.discardChanges=false;
//       //}
//        //else{
//       //   console.log("not found");
//       //   this.discardChanges=true;
//     //}
//       if(value2!=value)
//       {
//         flag=true;
//        }
//     })

// });

// if(flag)
// {
//   this.discardChanges=true;
// }
// else{
//   this.discardChanges=false;
// }

}
onClickChaneg()
{
  setTimeout(()=>{
    this.forChange();
}, 5000);
}

forChange()
{
  // console.log(this.chngeflag ,"chngeflag")
debugger;
 if(this.chngeflag == 2)
 {


if(this.myform.value.physician.length >0)
{
  this.myform.value.physician[0].Physician_Id;
}

if(this.myform.value.route.length >0)
{
  this.myform.value.route[0].Route_Id;
}


 this.prePId;
this.preRId;

  this.discardChanges=false;
  this.barcodear;
  //this.barcodear.join() != this.myform.value.pbarcode||
if(this.myform.value.self == null)
{
  this.myform.value.self = false;
}
if(this.myform.value.treatment == null)
{
  this.myform.value.treatment = false;
}
if(this.myform.value.controlSubstance == null)
{
  this.myform.value.controlSubstance = false;
}

if(this.myform.value.pself == null)
{
  this.myform.value.pself = false;
}
if(this.myform.value.ptreatment == null)
{
  this.myform.value.ptreatment = false;
}
if(this.myform.value.pcontrolSubstance == null)
{
  this.myform.value.pcontrolSubstance = false;
}

if(this.myform.value.physician.length>0 && this.myform.value.physician[0].Physician_Id != this.prePId)

{
  this.discardChanges=true;
}
if(this.myform.value.route.length>0 && this.myform.value.route[0].Route_Id != this.preRId)
{
  this.discardChanges=true;
}
if(this.myform.value.maxPerDay =="")
{
  this.myform.value.maxPerDay = null;
}
if(this.myform.value.pmaxPerDay == "")
{
  this.myform.value.pmaxPerDay = null;
}

  if(this.myform.value.maxPerDay != this.myform.value.pmaxPerDay ||  this.myform.value.qtyHand != this.myform.value.pqtyHand|| this.myform.value.drug != this.myform.value.pdrug||  ( this.myform.value.dose != this.myform.value.pdose)|| this.myform.value.refill != this.myform.value.prefill)
  {
    this.discardChanges=true;
  }

  if( (this.myform.value.addInst != this.myform.value.paddInst) || this.myform.value.insulincomments != this.myform.value.pinsulincomments||  this.myform.value.startDate != this.myform.value.pstartDate||  this.myform.value.alertText != this.myform.value.palertText|| this.myform.value.endDate != this.myform.value.pendDate)
  {
    this.discardChanges=true;
  }
  if(this.myform.value.type != this.myform.value.ptype || this.myform.value.self != this.myform.value.pself || this.myform.value.treatment != this.myform.value.ptreatment || this.myform.value.literal != this.myform.value.pliteral|| this.myform.value.prn != this.myform.value.pprn|| this.myform.value.controlSubstance != this.myform.value.pcontrolSubstance|| this.myform.value.orderTypeId != this.myform.value.porderTypeId)
  {
    this.discardChanges=true;
  }
}
  else{
    this.chngeflag = this.chngeflag + 1;
  }
}
discardOrderchange()
{
  //this.discardChanges=false;
  // this.getOrderDetailsbyorderId(this.orderId,this.quantityId,this.dAdminId);
  this.DiscardChangesbtn(this.orderId, this.quantityId, this.dAdminId);

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
       res =>{

       }
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
   this.spinnerLoading++;
   this.checkAndHideSpinnerLoading();
   this.dataservice.get<any>(this.config.Weight_GetWeightList + id )
   .subscribe(res => {
    this.spinnerLoading--;
    this.checkAndHideSpinnerLoading();
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
 checkformvalid()
 {
  debugger;
  const invalid = [];
  const controls = this.myform.controls;
  for (const name in controls) {
      if (controls[name].invalid) {
         // invalid.push(name);
      //   alert(name);
      }
  }
 }
 DiscardChangesbtn(orderId: number, quantityId: number, dAdminId: number) {
  {
    //this.ng4LoadingSpinnerService.show();
    this.sharedService.saveChangesOrderInfo(0);
    //this.ng4LoadingSpinnerService.show();
    this.orderId = orderId;
    this.quantityId = quantityId;
    this.dAdminId = dAdminId;
    this.notesFlag = 0;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.getOrderStockDetailsByOrderId();

    this.getFavouritesMasterData();
    
    this.GetPhysicianDropData(this.nurseStationId);

    this.dataservice.get<OrdersData>(this.config.Emar_Orders_GetOrderDetailsByOrderId + orderId + "/" + quantityId + "/" + this.userID)
      .subscribe(res => {

        // if (res.ControlSubstanceBit == 1) {
        //   if (res.ControlSubCreatedBy != this.userID) {
        //     this.isReadOnlyforControl = true;
        //   }
        //   else {
        //     this.isReadOnlyforControl = false;
        //   }
        // }
        this.alertService.success("Discarded changes");

        this.isReadOnlyforControl = true;
        // this.getOrderStockDetailsByOrderId();
        // this.getFavouritesMasterData();
        //this.GetScheduleTimeDetails();
        this.newOrderFlag = 0;
        this.ordersDetails = res;
        this.selectedOrderDetails = res;
        this.fieldsChangesDcFlag = "";
        this.qtydisabled = true;
        this.fetchOrdersData(res);
        this.GetPhysicianDropData(this.nurseStationId);
        // this.getEmarOrdersList3();
        this.selectedOrder = orderId;
        this.selectedOrderQuantity = quantityId;
        this.selectedOrderDADminId = dAdminId;
        this.hoaObj = null;
        // this.getNurseCommentNotesByQuantityId();
        //this.ng4LoadingSpinnerService..hide();
        this.discardChanges = false;
        this.valueChangesFlagReceive = 0;
        this.valueChangesFlag = 0;
        this.NurseEndFlag = false;
        this.nurseNotesObj = null;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService..hide();
      });
  }
}
onCheckHold(event: any, item: any) {
  debugger;

  let isChecked = event == true
  this.profileONHoldChangesCheckState[item.PQuantity_Id] = isChecked;
  this.rightGridOnHoldChangesFlag = Object.values(this.profileONHoldChangesCheckState).some((state) => state);
  if (this.rightGridOnHoldChangesFlag || this.rightGridDCChangesFlag) {
    this.myform.disable()
  } else {
    this.myform.enable()
  }
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
    else if(holdFlag == 0 && isChecked == false){
      checkHold.HoldChangeFlag = 0,
      checkHold.HoldStatus = 0,
      this.updateOrdersRecords.push();
      if (changesRecords.length > 0) {
        this.valueChangesFlagReceive = 1;
        // console.log("02")

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
ngDoCheck(): void {
  if (this.discardChanges || this.rightGridDCChangesFlag || this.rightGridOnHoldChangesFlag) {
    this.sharedService.saveChangesOrderInfo(1);
  }else{
    this.sharedService.saveChangesOrderInfo(0);

  } 
}
getNursingStationTimeZone(stationId: number) {
  this.spinnerLoading++;
  this.checkAndHideSpinnerLoading();
  this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
    .subscribe(res => {

      this.nursingStationZoneCurrentDate = res;
      //this.administrationDate=this.dateFormatPipe.transformISODate(res);
      this.myform.patchValue({
        dateCheck: this.dateFormatPipe.transformISODate(res),
      });
      this.minDate = this.dateFormatPipe.transformISODate(res);
      //this.today=this.dateFormatPipe.transformISODate(res);
      const toDateMaxAsDate = new Date(this.minStartDate);
      toDateMaxAsDate.setMonth(toDateMaxAsDate.getMonth() - 1);
      // console.log(this.minStartDate,"minStartDate");
      // console.log(toDateMaxAsDate,"toDateMaxAsDate")
      this.StartOnemonthDate =this.dateFormatPipe.transformISODate(toDateMaxAsDate);
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();
    }, error => {
      this.alertService.error(error.message);
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();

    });
}
EnddateChanged(){
  let selectDate = this.myform.value.endDate;
  if(selectDate < this.minDate){
    this.modalEndDateConfirm = true;
  }else{
    this.modalnoteIsOpen = false;
    this.noteform.reset();
    this.nurseNotes=[];
    this.modalEndDateConfirm = false;
  }
}
OpenNurseComments(){
  this.modalnoteIsOpen = true;
  this.getNurseCommentNotesByQuantityId();
  setTimeout(() => {
    this.nursecomments.nativeElement.focus()
  }, 300);
  this.nurseValidation = true;
  this.modalEndDateConfirm = false;
  const nurseComments = this.noteform.get('notes');
   nurseComments.setValidators([Validators.required]);
  nurseComments.updateValueAndValidity();

}
nurseEndComments(){
  let notes = this.noteform.value.notes;
  if (notes != "") {
    // this.nurseNotes=[];
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
    this.nurseNotes.push( this.nurseNotesObj);
    // console.log( this.nurseNotes," this.nurseNotes")
    this.NurseEndFlag = true;
    this.noteform.reset();
    this.alertService.success("Added notes successfully");
    this.nurseValidation= false;
    this.modalnoteIsOpen = false;
    this.modalEndDateConfirm = false;
  }
  else {
    this.alertService.error("Please enter nurse comment");
    //this.ng4LoadingSpinnerService..hide();
  }
}
CancelNurseComments(){
  this.myform.patchValue({
    endDate:''
  })
  this.modalEndDateConfirm = false;
  this.nurseValidation = false;
}
LiteralBarcCompleteReview(){
  this.literalBarcodeAlert= false;
  this.OverrideBarcodeLiteralreview = true;
  this.CompletedReviewClick(1);
}
checkAndHideSpinnerLoading() {

  if (this.spinnerLoading === 0 || this.spinnerLoading < 0) {
    this.ng4LoadingSpinnerService.hide();
  } else {
    this.ng4LoadingSpinnerService.show();

  }
}
}
