import { Component, OnInit, ViewChild, ElementRef, Input, AfterViewInit } from '@angular/core';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from './../../../models/facility.model';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { EmarResident, NursingSchedule, EmarOrdersList, DrugAdminister, MedicationReason, BypassBiometric, Refill, NurseComments, OrderFavouriteData, RefillNotes } from '../../../models/emar.model';
import { TimeFormatMasterData, OrderInfoAlert, BarcodeEntity, } from '../../../models/orders.model';
import { AlertService } from '../../../_services';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import * as Highcharts from 'highcharts';
import { chart } from 'highcharts';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RejectedrefillsComponent } from '../rejectedrefills/rejectedrefills.component';
import { PendingforreviewComponent } from '../pendingforreview/pendingforreview.component';
import { GridFilterPipe } from 'src/app/services/shared/grid-filter.pipe';
import { error } from 'console';
import { InsertBarcodeekit } from 'src/app/models/common.model';
import { HttpParams } from '@angular/common/http';
import { ClassGetter } from '@angular/compiler/src/output/output_ast';


@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit,  AfterViewInit {

  @ViewChild('chartTarget') chartTarget: ElementRef;
  chart: Highcharts.Chart;

  pageConfig = {};
  public template;
  public userId: number = 0;
  public searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  timeFormatId: number;
  PRNReasonForm: FormGroup;
  myform: FormGroup;
  adminform: FormGroup;
  medicationForm: FormGroup;
  withoutAdminsterForm: FormGroup;
  favouritesForm: FormGroup;
  biometricForm: FormGroup;

  //quantityform: FormGroup;

  minDate: Date = new Date();
  public nurseNotes: any[] = [];
  public insulinComments: string = "";
  public insulinCommentsToolTip: string = "";
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;
  public administerDate: any;
  public selectstyle: number = 1;
  public modalBypass: boolean = false;
  public byPassReason: string = '';
  private biometricInfo: string = "";
  public iconFlag: number = 0;
  public scanFlag: boolean = false;
  public barcodevalue: string = '';
  public orderItem: number;
  public accordionOpen: boolean = false;
  //public medicationValue: number;
  public ordersInfo: OrderInfoAlert = {} as any;
  public drugAdminsterObj: DrugAdminister;
  public nursecommentsObj: NurseComments;
  public undoFlag: boolean = false;
  public refillObj: Refill;
  public passtime: any;
  public vitalsCheckList: any[] = [];
  public CheckVitalsObj: OrderFavouriteData[] = [];
  barcodeScanCheckbox: boolean = false;
  byPassObj: BypassBiometric;
  public patientTypeList: any[] = [];
  public statusFlag: number = -1;
  yaxisTitle: string;
  chartData: any = null;
  public xaxisData: any;
  public y: any;
  emarform: FormGroup = new FormGroup({});
  public controlSubFlag: any;
  public controlSubCertBy: any;
  public pieChartFlag: boolean = false;
  rightTimeFlag: boolean = false;
  public ekit: any[] = [];
  public quantity: any;
  public days: number = 31;
  public yearDrop: any[] = [];
  public modaleMar1: boolean = false;
  //public timeFormatList: TimeFormatMasterData[];
  public passTime: any;
  public quantityInhand: any;
  //public modalQuantityIsOpen: boolean = false;
  modalVitalsIsOpen: boolean = false;
  modalUndoIsOpen: boolean = false;
  modalPRNIsOpen: boolean = false;
  modalLiterGridIsOpen: boolean = false;
  public undoOrderDetails: any;
  fingerId: number;
  modalPRNCommentIsOpen: boolean = false;
  processKey: string = "";
  public timesDisplay: boolean = false;
  public shiftDisplay: boolean = false;
  public windowCheck: boolean = true;
  public modalAdministerWithoutBarcodeOpen: boolean = false;
  public modalPRNCommentIsOpenWithoutScanner: boolean = false;

  public gridList: EmarResident[] = [];
  public ordersList: EmarOrdersList[] = [];
  public ordersListWithoutScanner: EmarOrdersList[] = [];
  public ordersListSelected: EmarOrdersList[] = [];
  CheckAll: boolean = false;
  public MyImages: any;

  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectednItemsNew = [];
  public nurseStationvalue: number = 0;
  public selectednItem: any = [];
  public nurseStations: NurseStation[];
  public NurseStationName: any;
  facilityName: any;
  public selectedfaItems = [];
  facilities = [];
  public eMARDetails: any[] = [];
  public legend: any[] = [];
  public timeDropList: any[] = [];
  public ekitDrop: any[] = [];
  public shiftsList: any[] = [];
  public medicationReasonList: MedicationReason[] = [];
  public externalPID: string = null;
  public PatientInside: number = 0;
  public passAdminister: boolean = true;
  public onemanyCHeck: boolean = true;
  public visible: boolean = true;
  dropdownSettings_NurseStation = {};
  dropdownSettings_Facilities: any = {};
  dropdownSettings_eKit = {};
  dropdownSettings_Time = {};
  dropdownSettings_Shifts = {};
  dropdownSettings_MedicationReason = {};
  ShowFilter = true;
  public selectedRecords: any[] = [];
  admScheduleForVitalsSave: string;
  orderDueFlag: boolean = false;
  orderId: any;
  quantityId: any;
  inputTime: string;
  shiftId: any;
  window: any;
  notestatus: number = 0;
  vitalsStatus: number = 0;
  lastPassedTime: any;
  public administrationDate: any;
  public administerFromEkitModal: boolean = false;
  public administerOrder: any;
  public LiteralorderGridData: any[] = [];
  public modalNoShowAdministerIsOpen: boolean = false;
  public noShowFlag: boolean = false;
  public prnAdministerFlag: number = 0;
  public isEkitReadonly: boolean = false;
  public prnMaxperday: string = '';
  public maxPerDayCount: number = 0;
  public lastPassed: string = "";
  public dueOrdersCheck: any = [];
  public administerBarcodeMatchModal: boolean = false;
  public dueOrderSelected: number = 0;
  public duePquantitySelected: number = 0;
  public selectedOrderIdForAdminister: number = 0;
  public modalAdditionalCommentsIsOpen: boolean = false;
  public modalAdditionalCommentsFormIsOpen: boolean = false;
  public addComments: number = 0;
  public tempOrderId: any;
  public tempQtyId: any;
  public additionalComForm: FormGroup;
  public nurseComments: any[] = [];
  nurseAdditionalCommentsObj: any;
  public modalAdministerWithoutScannerVitalsIsOpen: boolean = false;
  public withoutScannerDisplayVitals = [];
  public withoutScannerOrderVitals = [];
  public withoutScannerTempVitals = [];
  public WsPRN: FormGroup;
  public dropdownSettings_ComputerName: any = {};
  computernameform: FormGroup;
  public selectedComItems = [];
  public computers = [];
  public modalBiometricComputerName: boolean = false;
  public modalDiscardDateIsOpen: boolean = false;
  public discardform: FormGroup;
  public manuallySetDiscard: boolean = false;
  public today: any = this.dateFormatPipe.transformISODate(new Date());
  public newProductIsOpen: boolean = false;
  public discardAlertMsg: string = "";
  public discardAlertColorFlag: number = 1;
  public modalSideEffectsIsOpen: boolean = false;
  public sideEffects: string = '';
  public sideEffectsDrugName: string = "";
  public WsDiscardDate: FormGroup;
  public wsDiscard: number = 1;
  public modalAdministerInsulinSiteIsOpen: boolean = false;
  public insulinSites: any = [];
  public displayInsulinSites = [];
  public ent: boolean = false;
  public modalInsulinSiteWSIsOpen: boolean = false;
  public insulinSiteIdsWS = [];
  public porderIdForInsulin: any;
  public routeForInsulin: any;
  public wsOrderInsulinSites = [];
  public displayInsuliSitesForWS = [];
  public lastUsedSiteWS: string = "";
  public lastUsedSites: string = "";
  public sitesType: string = "";
  public nursingStationZoneCurrentDate: any;
  public listChage = 0;
  private controlType: string = "NW";
  public allInsulinSites = [
    { item_id: 1, item_text: 'Thigh, Left (Quadricep)', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 2, item_text: 'Thigh, Right (Quadricep)', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 3, item_text: 'Arm, Left (Deltoid)', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 4, item_text: 'Arm, Right (Deltoid)', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 5, item_text: 'Abdomen, RUQ', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 6, item_text: 'Abdomen, RLQ', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 7, item_text: 'Abdomen, LUQ', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 8, item_text: 'Abdomen, LLQ', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 9, item_text: 'Buttocks, Left (Gluteus)', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 10, item_text: 'Buttocks, Right (Gluteus)', item_type: 1, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 11, item_text: 'Chest, Left', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 12, item_text: 'Chest, Right', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 13, item_text: 'Back, Left', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 14, item_text: 'Back, Right', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 15, item_text: 'Arm, Left', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 16, item_text: 'Arm, Right', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 17, item_text: 'Ear, behind Left', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
    { item_id: 18, item_text: 'Ear, behind Right', item_type: 2, lastUsed: 0, lastUsedDate: "", IsChacked: false, IsDisabled: false },
  ];
  r: number = 1;
  q: number = 1;
  public modalPendingOrderIsOpen: boolean = false;
  public pendingOrdersList: any[] = [];
  private filterConfigs: any = [];
  private type: string;
  public residentOrdersList: any[] = [];
  visitStatusForm: FormGroup;
  pendingOrderSearch: string = "";
  public modalResidentAllOrderIsOpen: boolean = false;
  public form: FormGroup;
  public fields: any[];
  public clicked: boolean = false;
  public clk: boolean = false;
  public cllk: boolean = false;
  public apiExecuted: boolean = false;
  public validateEmptyField(c: FormControl) {
    return c.value && !c.value.trim() ? {
      required: {
        valid: false
      }
    } : null;
  }
  public getRefillNotes: string = '';
  public spinnerLoading: number = 0;
  prnyaxisTitle: string;
  filteredItems: EmarResident[];
  filteredprnitems: EmarResident[];
  patientIdForGrid: number;
  prnpatientidforgrid: number;
  @ViewChild('prnchartTarget') prnchartTarget: ElementRef;
  prnchart: Highcharts.Chart;
  public barcodeFacilityId: number = 0;
  public barcodesPList = [];
  @ViewChild('myInput') inputField: ElementRef;

  public barcodeAlert: string = '';
  public barcodeAlertsPop: boolean = false;
  public twoHours: string = '';
  public ekitDrugDrop: any[] = [];
  dropdownSettings_DrugeKit = {};
  public ekitDrug: any[] = [];
  public ekitDrugForm: FormGroup;
  public InsertBarcodeekit = new InsertBarcodeekit();
  public selectEkitDrug: any
  public selectEkitGpi: any
  public lotdetailsGrid: any;
  public lotsGrid: any;
  public modalekitAdLotQty: boolean = false;
  public textform3: FormGroup;
  public displayDrug = '';
  totalQtyval: number = 0;
  inHandValues: number[] = [];
  barcodeFlag: any;
  barcodeFlagKeys: { [key: string]: number } = {};
  public barcodeEkitScanCheckboxValue: boolean = false;

  @ViewChild('myInput2') myInputLotFocus: ElementRef;
  public selectedEkit_Id: any;
  public selectedInhandQty: any;
  public selectedQtyAdmin: any;
  @ViewChild('pendingOrderSearchFocus') pendingOrderSearchFocusField: ElementRef;
  @ViewChild('AdditionalCommentsFocus') AdditionalCommentsFocus: ElementRef;
  @ViewChild('prnOrderFocus') prnOrderFocus: ElementRef;
  @ViewChild('reasonForWithOutScannerFocus') reasonForWithOutScannerFocus: ElementRef;
  @ViewChild('modalBypassReasonFocus') modalBypassReasonFocus: ElementRef;
  @ViewChild('textFocus') textFocus: ElementRef;
  @ViewChild('ekitItem') ekitItem: ElementRef;
  administerWithoutScannerInputTime: string;
  administerWithoutScannerShiftId: any;
  administerWithoutScannerWindow: any;
  public barcodeExpAlert: boolean = false;
  public selectedDrugName = '';
  public ekitSelctedarray = [];
  public dataEkitPost = [];
  public ekitWithoutScanningExceedQtyFlag:boolean =false
  public isQuantityExceeded: boolean=false;

  constructor(private persistanceService: PersistanceService, private sharedService: SharedService, private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sanitizer: DomSanitizer,
    private dateFormatPipe: CustomdatePipe, private modalService: NgbModal, private filterPipe: GridFilterPipe) {

  }
  ngOnInit() {
    window.scroll(0, 0);
    this.persistanceService.getDueMARAlert();
    this.pageConfig = this.persistanceService.getPermissionsByScreen("EMAR");

    if (this.ordersList.length) {
      console.log("orderlistloded")
    }
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.processKey = this.persistanceService.get('emarProcessKey');
        var date = this.dateFormatPipe.transform(new Date());
        this.administrationDate = this.dateFormatPipe.transformISODate(new Date());
        this.listChage = 0;
        // this.nurseStationvalue=1;
        this.myform = new FormGroup({
          dateCheck: new FormControl(this.administrationDate, Validators.required),
          nurseSheduleTime: new FormControl(''),
          nsShiftTime: new FormControl(''),
          shiftTimeFormat: new FormControl('0'),
          twoHrsWindow: new FormControl(false)
        });
        this.form = new FormGroup({
          fields: new FormControl(JSON.stringify(this.fields))
        });
        this.adminform = new FormGroup({
          nstationName: new FormControl(''),
          ddlfacilities: new FormControl(''),
        });
        this.ekitDrugForm = new FormGroup({
          barcode: new FormControl('', Validators.required),
          drugSelect: new FormControl('', Validators.required)
        })
        this.PRNReasonForm = new FormGroup({
          prnReason: new FormControl('', [Validators.required, Validators.maxLength(50)])
        })
        this.medicationForm = new FormGroup({
          medicationReason: new FormControl('0', Validators.required),
          note: new FormControl('', Validators.maxLength(50)),
          undoQuantity: new FormControl('0', Validators.required)
        });
        this.withoutAdminsterForm = new FormGroup({
          reasonForWithoutAdministred: new FormControl(''),
        });
        this.favouritesForm = new FormGroup({
          comments: new FormControl('', [Validators.required, Validators.maxLength(50)]),
        });
        this.biometricForm = new FormGroup({
          BiometricBypas: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
        });
        this.additionalComForm = new FormGroup({
          addCom: new FormControl('', [Validators.required, Validators.maxLength(50), this.validateEmptyField])
        })
        this.WsPRN = new FormGroup({
          WSPrnReason: new FormControl('',)
        })
        //this.getUserFacilities(this.userId);
        this.computernameform = new FormGroup({
          computerName: new FormControl('', Validators.required),
        });
        this.discardform = new FormGroup({
          discardDate: new FormControl('', Validators.required)
        });
        this.WsDiscardDate = new FormGroup({
          WSDiscard: new FormControl('',)
        })
        this.emarform = new FormGroup(
          {
            month: new FormControl(),
            year: new FormControl(),
            hidechk: new FormControl(),
          });
        this.textform3 = new FormGroup({
          InHandqty: new FormArray([])
        })
        let data = new Date();
        this.emarform.patchValue({
          month: data.getMonth() + 1,
          year: data.getFullYear(),
        })
        this.dropdownSettings_ComputerName = {
          singleSelection: true,
          idField: "ProcessKey",
          textField: "ComputerName",
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          noDataAvailablePlaceholderText: "Please Change Nursing Station",
          allowSearchFilter: true
        };

        this.dropdownSettings_Facilities = {
          singleSelection: true,
          idField: "Facility_Id",
          textField: "Facility_Name",
          text: "Facilities",
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          allowSearchFilter: this.ShowFilter
        };
        this.dropdownSettings_NurseStation = {
          singleSelection: true,
          idField: 'NurseStation_Id',
          textField: 'NurseStation_Name',
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter,
          closeDropDownOnSelection: true,
          noDataAvailablePlaceholderText: 'Please Select Facility',
        };
        this.dropdownSettings_eKit = {
          singleSelection: true,
          idField: 'Ekit_Id',
          textField: 'DrugName',
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          allowSearchFilter: this.ShowFilter,
        }
        this.dropdownSettings_DrugeKit = {
          singleSelection: true,
          idField: 'DrugName',
          textField: 'DrugName',
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          allowSearchFilter: this.ShowFilter,
        };

        this.dropdownSettings_Time = {
          singleSelection: true,
          idField: 'ScheduleTime',
          textField: 'ScheduleTime',
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter,
          closeDropDownOnSelection: true,
          noDataAvailablePlaceholderText: 'No orders due',
        }
        this.dropdownSettings_Shifts = {
          singleSelection: true,
          idField: 'NurseShifts_Id',
          textField: 'NurseShifts_Name',
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter,
          closeDropDownOnSelection: true,
          noDataAvailablePlaceholderText: 'No orders due',
        }
        this.visitStatusForm = new FormGroup({
          visitStatus: new FormControl('1'),
        });
        this.dropdownSettings_MedicationReason = {
          singleSelection: true,
          idField: "MedicationReason_ID",
          textField: "MedicationReason_Desc",
          text: "Medication Reason",
          itemsShowLimit: 1,
          closeDropDownOnSelection: true,
          allowSearchFilter: this.ShowFilter
        };
        //this.getNurseStations();
        this.userActivity();
        //this.getAverageCensusChartData();
        this.getUserRecentFacilityNurseStations();
        this.textform3.valueChanges.subscribe(res => console.log(res, "text3"))
        //this.GetUserProcessKeyByID();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  onMedicationReasonSelect(item: any) {
    if ((item != null && item != undefined) && item.MedicationReason_ID == 7) {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators([Validators.required, Validators.maxLength(50)]);
      notevalidation.updateValueAndValidity();
      this.notestatus = 1;
    }
    else {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators(null);
      notevalidation.clearValidators();
      notevalidation.updateValueAndValidity();
      this.notestatus = 0;
      this.medicationForm.patchValue({
        note: ''
      });
    }
  }
  onMedicationReasonDeSelect(item: any) {
    if ((item != null && item != undefined) && item.MedicationReason_ID == 7) {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators(null);
      notevalidation.clearValidators();
      notevalidation.updateValueAndValidity();
      this.notestatus = 0;
      this.medicationForm.patchValue({
        note: ''
      });
    }
    this.medicationForm.patchValue({
      note: ''
    });
  }
  getUserRecentFacilityNurseStations() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getUserFacilities(this.userId);
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getUserFacilities(userId: number): any {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetCompanyToBedData + userId)
      .subscribe((res: any) => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.facilities = res.Facilities;
        this.ng4LoadingSpinnerService.hide();

        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.sharedService.changeFacilityId(this.selectedfaItems[0].Facility_Id);

              this.getNurseStations(this.loginUserReceFacility);
            }
            this.adminform.patchValue({
              ddlfacilities: this.selectedfaItems,
            });
          }
        }
        else if (res.Facilities.length == 1) {
          this.getNurseStations(res.Facilities[0].Facility_Id);
          this.adminform.patchValue({
            ddlfacilities: this.facilities,
          });
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);

        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }




  loadDynamicChart(seriesData: any, xaxisData: any, yaxisTitle: string, chartModule: string, chartType: string) {
    this.chartData = {
      "seriesData": seriesData,
      "xaxisData": xaxisData,
      "yaxisTitle": yaxisTitle,
      "chartModule": chartModule,
      "chartType": chartType
    }
  }
  getAverageCensusChartData() {
    //string years, int userId, int nursingstationId, int type, int reportType
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();

    this.dataservice.get<any>(this.config.Emar_Dashboard_GetAverageCensusDashboard + 2019 + "/" + 4 + "/" + 26 + "/" + "1,2,3,4,5,7,8,19,20,22,23,26,27,28,29,30,32,33,34,35,36,38,39,40,41,42,43,45,53,54,56,57")
      .subscribe(res => {
        if (res != null) {
          let seriesData = res.YaxisData;
          let xaxisData = res.XaxisData;
          this.yaxisTitle = "Count";
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          this.loadDynamicChart(seriesData, xaxisData, this.yaxisTitle, 'average', 'column');

          // if (this.reportType == 1)
          //   this.chartTitle = 'Active Residents (' + years + ')';
          // else if (this.reportType == 2)
          //   this.chartTitle = 'Discharge Residents (' + years + ')';
          // else if (this.reportType == 3)
          //   this.chartTitle = 'Census with Allergy (' + years + ')';
          //this.createAverageCensusChart(seriesData, xaxisData, this.yaxisTitle);
        }
      },
        error => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  createChart(seriesData, xaxisData, yaxisTitle, chartTitle): void {
    // this.ng4LoadingSpinnerService.show();

    const options: Highcharts.Options = {
      chart: {
        type: 'pie',
        zoomType: 'x',
        renderTo: 'container',
        height: 250,
        inverted: false,
        options3d: {
          enabled: true,
          alpha: 10,
          beta: 25,
          depth: 70
        }
      },
      colors: [
        '#28a745',
        '#dc3545',
        '#ffc107',
        '#007bff'
      ],
      title: {
        text: ''
      },
      //   tooltip: {
      //     pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
      // },
      plotOptions: {
        pie: {

          allowPointSelect: true,
          cursor: '',
          dataLabels: {
            enabled: false,
            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
          },
          showInLegend: true
        }
      },
      legend: {
        align: 'center',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: xaxisData,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: yaxisTitle
        }
      },
      series: [{
        type: 'pie',
        name: 'Count',
        data: seriesData
      }],
      responsive: {
        rules: [{
          condition: {
            maxWidth: 400
          },
          chartOptions: {
            legend: {
              align: 'center',
              verticalAlign: 'bottom',
              layout: 'vertical'
            },
            yAxis: {
              labels: {
                align: 'left',
                x: 0,
                y: -5
              },
              title: {
                text: chartTitle
              }
            },
            subtitle: {
              text: 'Scheduled Med Pass Status',
              style: {
                color: '#212529',
                fontWeight: '600'
              }
            },
            credits: {
              enabled: false
            }
          }
        }]
      },
      credits: {
        enabled: false
      }
    }
    this.chart = chart(this.chartTarget.nativeElement, options);
    // this.ng4LoadingSpinnerService.hide();
  };
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.EMAR, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  getNurseStations(facilityId: any) {
    this.selectednItem = [];
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItem = [];
            this.selectednItemsNew = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItemsNew.push(checkNsExist);
              }
              else if (this.nurseStations != undefined && this.nurseStations.length > 0) {
                this.selectednItemsNew.push(this.nurseStations[0]);
                this.myform.patchValue({
                  nstationName: this.selectednItemsNew,
                });
              }
            }
            if (this.selectednItemsNew.length != 0) {
              let lastNsId = this.selectednItemsNew[this.selectednItemsNew.length - 1];
              this.nurseStationvalue = parseInt(lastNsId.NurseStation_Id);
              this.selectednItem.push(res.filter(s => s.NurseStation_Id === parseInt(lastNsId.NurseStation_Id))[0]);
              this.getAllFlagsForCompanyByNSId(this.nurseStationvalue);
              this.getNursingScheduleData();
              this.getPendingOrders();
            }
            this.adminform.patchValue({
              nstationName: this.selectednItem,
            });
          }
        }
        else if (this.loginUserReceNurseStation == undefined) {
          if (res.length > 0) {
            this.nurseStationvalue = this.nurseStations[0].NurseStation_Id;
            this.selectednItem.push(res[0]);
            this.adminform.patchValue({
              nstationName: this.selectednItem,
            });
            this.getPendingOrders();
            this.getAllFlagsForCompanyByNSId(this.nurseStationvalue);
            this.getNursingScheduleData();
          }
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        }
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  changeNurseStation(item: any) {
    this.gridList = [];
    this.timeDropList = [];
    this.shiftsList = [];
    this.pieChartFlag = false;
    this.timesDisplay = false;
    this.shiftDisplay = false;
    this.windowCheck = true;
    this.pendingOrdersList = [];
    this.getPendingOrders();

    //this.myform.controls["nurseSheduleTime"].reset();
    this.myform.patchValue({
      nurseSheduleTime: 0,
      nsShiftTime: 0,
      twoHrsWindow: 0
    });
    if (this.selectednItem.length == 0) {
      this.nurseStationvalue = 0;
      this.alertService.warn("Please select at least one nursing station")
    }
    else if (this.myform.value.dateCheck == "") {
      this.alertService.warn("Please select date");
    }
    else {
      this.ng4LoadingSpinnerService.show();
      this.nurseStationvalue = item.NurseStation_Id;
      // this.myform.controls["nurseSheduleTime"].reset();
      // this.myform.patchValue({
      //   // nurseSheduleTime: 0,
      //   nsShiftTime: 0,
      //   twoHrsWindow: 0
      // });
      this.getAllFlagsForCompanyByNSId(this.nurseStationvalue);
      this.getNursingScheduleData();
      //this.getNurseStationName(this.nurseStationvalue);
      //this.pieChartFlag = false;
      //this.getCompanyConfiguredBiometricFinger(this.nurseStationvalue);
    }
  }
  getNurseStationName(nurseStationId: number) {
    this.ekit = [];
    this.lotsGrid = [];
    this.quantity = '';
    //this.getEkitDrop(nurseStationId);
    this.NurseStationName = this.nurseStations.find(item => item.NurseStation_Id == nurseStationId).NurseStation_Name;
  }
  getEkitDrop(nursestation: number, orderId: number, drugName: string) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_GetEkitDropData + nursestation + "/" + orderId + "/" + this.userId)
      .subscribe(res => {

        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.ekitDrop = [];
        if (res != undefined && res != null && res.length > 1) {
          //this.ekitDrop = res[0];
          this.ekitDrop.push(res[0]);
        }
        else {
          this.ekitDrop = res;
        }
        this.isEkitReadonly = false;
        this.ekit = [];
        if (this.ekitDrop != undefined && this.ekitDrop != null && this.ekitDrop.length > 0) {
          let selectedEkit = this.ekitDrop.find(e => e.DrugName.toLowerCase().trim() == drugName.toLowerCase().trim());
          if (selectedEkit != undefined) {
            this.ekit.push(selectedEkit);
            //this.isEkitReadonly=true;
          }
        }
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  getekitQuantity(value: string) {
    if (this.ekit != null && this.ekit != undefined && this.ekit.length != 0) {
      this.quantityInhand = this.ekitDrop.find(item => item.Ekit_Id == this.ekit[0].Ekit_Id).InHand;
      if (this.ekit != null && this.ekit != undefined && parseFloat(value) == 0) {
        this.alertService.warn("Please enter valid numeric quantity");
        this.quantity = "";
      }
      else if (parseFloat(value) > parseFloat(this.quantityInhand)) {
        //this.alertService.warn("Entered more quantity.You have only " + this.quantityInhand + " On Hand Quantity");
        this.alertService.warn("Quantity entered exceeds on-hand quantity");
        this.quantity = "";
      }
    }
  }
  checkEkitQuantity() {
    this.quantityInhand = this.ekitDrop.find(item => item.Ekit_Id == this.ekit[0].Ekit_Id).InHand;
    if (parseFloat(this.quantity) > parseFloat(this.quantityInhand))
      return true;
    else
      return false;
  }
  getNursingScheduleData() {
    let scheduleDate = this.myform.value.dateCheck.split('/').join('-');
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<NursingSchedule[]>(this.config.Emar_Emar_GetNursingScheduleData + this.nurseStationvalue + "/" + scheduleDate)
      .subscribe(res => {
        if (res == null) {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.warn('No Orders Due');
          this.gridList = [];
          this.timeDropList = [];
          this.shiftsList = [];
          //this.getEmarResidentGridData();

        }
        else {
          this.timeDropList = res;
          this.getShiftHoursMasterData();
          //this.getEmarResidentGridData();
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }
  onTwoHoursWindowClick(event) {
    this.getEmarResidentGridData();
  }
  onTimeSelect(item: any) {
    this.shiftDisplay = true;
    this.windowCheck = false;
    this.myform.patchValue({
      twoHrsWindow: true
    })
    this.getEmarResidentGridData();
  }
  onTimeDeSelect(item: any) {
    this.shiftDisplay = false;
    this.pieChartFlag = false;
    this.windowCheck = true;
    this.gridList = [];
    this.myform.patchValue({
      twoHrsWindow: 0
    });
    //this.getEmarResidentGridData();
  }
  onShiftSelect(item: any) {
    this.timesDisplay = true;
    this.windowCheck = true;
    this.myform.patchValue({
      twoHrsWindow: 0
    });
    this.getEmarResidentGridData();
  }
  onShiftDeSelect(item: any) {
    this.timesDisplay = false;
    this.myform.patchValue({
      twoHrsWindow: 0
    });
    this.pieChartFlag = false;
    this.gridList = [];
    //this.getEmarResidentGridData();
  }
  getEmarResidentGridData() {

    this.gridList = [];
    // if (type ) {
    //   this.pieChartFlag = false;
    //   this.timesDisplay = false;
    //   this.shiftDisplay = false;
    //   this.windowCheck = true;
    //   this.gridList = [];
    //   this.myform.patchValue({
    //     twoHrsWindow: 0
    //   });
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // if (this.myform.value.nsShiftTime != 0) {
    //   this.timesDisplay = true;
    //   this.windowCheck = true;
    //   this.myform.patchValue({
    //     twoHrsWindow: 0
    //   });
    // }
    // if (this.myform.value.nurseSheduleTime != undefined && this.myform.value.nurseSheduleTime != null && this.myform.value.nurseSheduleTime.length != 0) {
    //   this.shiftDisplay = true;
    //   this.windowCheck = false;
    // }
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    let date = this.myform.value.dateCheck.split('/').join('-');
    let time = this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime == null || this.myform.value.nurseSheduleTime == undefined || this.myform.value.nurseSheduleTime == 0 ? null : this.myform.value.nurseSheduleTime;
    time = time == null ? null : (time.length == 0 ? null : time[0].replace(':', '-'));
    this.passTime = time;
    let shift = this.myform.value.nsShiftTime == 0 ? 0 : this.myform.value.nsShiftTime[0].NurseShifts_Id;
    let showTwoHours = this.myform.value.twoHrsWindow == true ? 1 : 0;

    this.dataservice.get<any>(this.config.Emar_Emar_GetEmarResidentGridData + this.nurseStationvalue + "/" + time + "/" + date + "/" + shift + "/" + showTwoHours)
      .subscribe(res => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.externalPID = null;
        if (res.GridData.length > 0) {
          this.pieChartFlag = true;
          console.log(res, 'gridlist resp')
          this.gridList = res.GridData;
          this.filteredItems = this.gridList.filter(item => {
            if (item.MedReasonCount === 0 && item.Pending_count === 0 && item.Total_Count === 0) {
              this.patientIdForGrid = item.Patient_Id; // Set patientId if conditions are met
              return item; // Include this item in the filtered list
            } else
              return 0;
          })
          console.log(this.filteredItems, 'filter')
          this.filteredprnitems = this.gridList.filter(item => {
            if (item.Prnmedreasoncount == 0 &&
              item.PrnPending == 0 &&
              item.PRN_Drugs == 0) {
              this.prnpatientidforgrid = item.Patient_Id;
              return item;
            } else
              return 0;

          });

          let seriesData = res.YAxisData;
          let xaxisData = res.XAxisData;
          this.yaxisTitle = "Count";

          let prnseriesData = res.PYAxisData;
          let prnxaxisData = res.PXAxisData;
          this.prnyaxisTitle = "Count";

          this.createChart(seriesData, xaxisData, this.yaxisTitle, 'Admin');
          this.prncreateChart(prnseriesData, prnxaxisData, this.prnyaxisTitle, 'Admin');

        }
        else {
          this.pieChartFlag = false;
        }
        this.ng4LoadingSpinnerService.hide();

      }, error => {
        this.alertService.error(error.message);

      });
  }
  getReport() {
    //this.passtime = null;
    let shift = this.myform.value.nsShiftTime == 0 ? 0 : this.myform.value.nsShiftTime[0].NurseShifts_Id;
    let showTwoHours = this.myform.value.twoHrsWindow == true ? 1 : 0;
    this.passtime = this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime.length == 0 || this.myform.value.nurseSheduleTime == null || this.myform.value.nurseSheduleTime == undefined ? null : this.myform.value.nurseSheduleTime[0].replace(':', '-');
    let facilityId = this.adminform.value.ddlfacilities[0].Facility_Id;
    this.ng4LoadingSpinnerService.show();
    let dateTime = this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.getFile(this.config.Emar_Report_EmarResidentReportsecurity + this.nurseStationvalue + "/" + this.myform.value.dateCheck + "/" + this.passtime + "/" + shift + "/" + showTwoHours + "/" + this.userId + "/" + facilityId + "/" + dateTime + "/" + this.timeFormatId + "/" + 2)
      .subscribe((res) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "residentreport" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);

        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  getDayReport() {
    this.ng4LoadingSpinnerService.show();
    this.passtime = this.passTime == null ? null : this.passTime
    let shiftTime = this.passtime == null ? ((this.myform.value.nsShiftTime == undefined || this.myform.value.nsShiftTime == 0 || this.myform.value.nsShiftTime.length == 0 || this.myform.value.nsShiftTime == null) ? 0 : this.myform.value.nsShiftTime[0].NurseShifts_Id) : 0;
    let showTwoHours = this.myform.value.twoHrsWindow == true ? 1 : 0;
    let facilityId = this.adminform.value.ddlfacilities[0].Facility_Id;
    let dateTime = this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.getFile(this.config.Emar_Report_EmarResidentReportsecurity + this.nurseStationvalue + "/" + this.myform.value.dateCheck + "/" + this.passtime + "/" + shiftTime + "/" + showTwoHours + "/" + this.userId + "/" + facilityId + "/" + dateTime + "/" + this.timeFormatId + "/" + 1)
      .subscribe((res) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "residentreportbydate" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }
  byPass() {
    //this.residentId = patientId;
    this.modalBypass = true;
    setTimeout(() => {
      this.modalBypassReasonFocus.nativeElement.focus();
    }, 300);

    this.byPassReason = "";
    //this.passTime = passtimeValue;
  }
  modalBypassClose() {
    this.modalBypass = false;
    //this.byPassReason = "";
    //this.selectstyle = 1;
    //this.biometricForm.reset();
  }
  // closeModel() {
  //   let inhandQuantity = this.undoOrderDetails.Inhand;
  //   if (parseFloat(this.quantityform.value.undoQuantity) > parseFloat(inhandQuantity)) {
  //     this.alertService.warn("Entered Quantity is more than On Hand Quantity");
  //     this.modalQuantityIsOpen = true;
  //   }
  //   else {
  //     this.modalQuantityIsOpen = false;
  //   }
  // }
  closeVitalsModel() {
    this.CheckVitalsObj = [];
    this.barcodevalue = '';
    this.favouritesForm.reset();
    this.ekit = [];
    this.quantity = '';
    this.administerOrder = null;
    this.undoFlag = false;
    this.modalVitalsIsOpen = false;
  }
  closeUndoModel() {
    // if (parseFloat(this.quantityform.value.undoQuantity) > parseFloat(inhandQuantity)) {
    //   //     this.alertService.warn("Entered Quantity is more than On Hand Quantity");
    this.modalUndoIsOpen = false;
  }
  closePRNModel() {
    this.modalPRNIsOpen = false;
    this.barcodevalue = '';
  }
  closePRNCommentModel() {
    this.modalPRNCommentIsOpen = false;
    this.barcodevalue = '';
  }
  getLiteralorders(PatientID: number) {
    this.residentId = PatientID;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetLiteralOrderGridData + this.residentId)
      .subscribe(res => {

        this.LiteralorderGridData = res;

        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();

        }
      )
  }
  OpenLiteralordergridmodal() {
    this.modalLiterGridIsOpen = true;
  }
  CloseLiteralordergridmodal() {
    this.modalLiterGridIsOpen = false;
  }
  getDemographicInfoData(PatientID: number) {

    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    this.residentId = PatientID;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {

        this.demographicInfoData = res;
        this.getEmarOrdersList();
        this.scanFlag = false;
        this.iconFlag = 0;
        this.getNurseStationName(this.nurseStationvalue);
        this.GetDiagnosisDetails();
        this.getPatientType();
        this.getMedicationReason();
        if (this.passTime != null)
          this.administerDate = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.passTime.replace('-', ':');
        else if (this.myform.value.nsShiftTime != undefined && this.myform.value.nsShiftTime != null && this.myform.value.nsShiftTime.length != 0)
          this.administerDate = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.myform.value.nsShiftTime[0].NurseShifts_Name;
        else
          this.administerDate = this.dateFormatPipe.transform(this.myform.value.dateCheck);
        this.facilityName = this.adminform.value.ddlfacilities[0].Facility_Name;
        // let scheduleDate = new Date(this.administerDate);
        // let startdate = new Date(this.administerDate);
        // let enddate = new Date(this.administerDate);
        // startdate.setMinutes(scheduleDate.getMinutes() - 60);
        // enddate.setMinutes(scheduleDate.getMinutes() + 60);
        // let currentDate = new Date();
        // if (currentDate >= startdate && currentDate <= enddate)
        //   this.rightTimeFlag = true;
        // else
        //   this.rightTimeFlag = false;
        if (this.passTime != null && this.adminsterRightTime(this.passTime.replace('-', ':')) == true)
          this.rightTimeFlag = true;
        else if (this.passTime != null && this.adminsterRightTime(this.passTime.replace('-', ':')) == false)
          this.rightTimeFlag = false;
        else if (this.passTime == null)
          this.rightTimeFlag = true;
        // if(this.fingerId==null || this.fingerId==0)
        // {
        //   this.passAdminister=false;
        // }
        //this.ng4LoadingSpinnerService.hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  adminsterRightTime(element: any): any {

    let admDate = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + element;
    let scheduleDate = new Date(admDate);
    let startdate = new Date(admDate);
    let enddate = new Date(admDate);
    startdate.setMinutes(scheduleDate.getMinutes() - 60);
    enddate.setMinutes(scheduleDate.getMinutes() + 60);
    let currentDate = new Date();
    if (currentDate >= startdate && currentDate <= enddate)
      return true;
    else
      return false;
  }
  byPassSubmit() {
    this.passAdminister = false;
    // this.getDemographicInfoData(this.residentId);
    // this.getLiteralorders(this.residentId);
    // //this.byPassBiometric(2);
    // this.selectstyle = 2;
    this.modalBypass = false;
    // this.barcodevalue = '';
    // this.orderItem = -1;
    // this.statusFlag = -1;
    // this.reSet();
    // this.barcodeScanCheckbox = false;
  }
  // byPassBiometric(bypass: number) {
  //   this.byPassObj = {
  //     PatientID: this.residentId,
  //     Time: this.passTime.replace(':', '-'),
  //     dateValue: this.myform.value.dateCheck,
  //     reason: this.byPassReason,
  //     byPass: bypass
  //   }
  //   this.dataservice.post(this.config.Emar_ByPassBiometric, this.byPassObj)
  //     .subscribe(res => {
  //       this.byPassReason = "";
  //       this.getDemographicInfoData(this.residentId);
  //     }, error => {
  //       this.alertService.error(error.message);
  //     });
  // }
  back() {
    this.clicked = false;
    this.clk = false;
    this.selectstyle = 1;
    this.barcodevalue = '';
    this.reSet();
    this.biometricForm.reset();
    this.getEmarResidentGridData();
    this.getPendingOrders();
  }
  onResidentSelectdForAdminister(patientId: number, mrNumber: string) {

    //this.residentId = patientId;
    this.apiExecuted = false;
    this.getAllBarcodes(patientId)
    this.getPendingOrdersInside(patientId)
    this.externalPID = mrNumber;
    this.PatientInside = patientId;
    this.onemanyCHeck = true;
    this.passAdminister = true;
    this.getDemographicInfoData(patientId);
    this.selectstyle = 2;
    this.barcodevalue = '';
    this.orderItem = -1;
    this.statusFlag = -1;
    this.reSet();

    this.getLiteralorders(this.residentId);
    this.inputField.nativeElement.focus();
  }
  getResidentBiometricInfo() {

    // if (this.fingerId == null || this.fingerId == 0)
    // //this.alertService.warn("Biometric finger is not configured for this company");
    // {
    //   this.getDemographicInfoData(patientId);
    //   this.selectstyle = 2;
    //   this.barcodevalue = '';
    //   this.orderItem = -1;
    //   this.statusFlag = -1;
    //   this.reSet();
    //   this.getLiteralorders(this.residentId);
    // }
    // else
    this.onemanyCHeck = true;
    if (this.externalPID != null) {
      this.ng4LoadingSpinnerService.show();
      if (this.processKey != "") {
        let bioObj = {
          PatientMRNumber: this.externalPID,
          Fingersdesc_Id: this.fingerId,
          PatientBiometric_CreatedBy: this.userId,
          ProcessKey: this.processKey,
          AppType: 2,
          App: 2
        };

        //let processk=this.processKey;
        this.dataservice.postBiometric(this.config.Biometric_Demographic_VerifyResidentBiometricByMRNumber, bioObj)
          .subscribe(res => {

            if (res.m_Item2 == "Match Found") {
              this.passAdminister = false;
              //           if(res.m_Item1!=null)
              //           {
              //           let record = this.gridList.find(g => g.ExternalPatientId == res.m_Item1);
              //             if (record != undefined) {
              //               this.getDemographicInfoData(record.Patient_Id);
              //               this.selectstyle = 2;
              //               this.barcodevalue = '';
              //               this.orderItem = -1;
              //               this.statusFlag = -1;
              //               this.reSet();
              //             }
              //             else
              //             {
              //               this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentActiveStatus + res.m_Item1)
              //       .subscribe(res => {
              // if(res=='Active')
              // {
              //   this.alertService.warn("No Active Adminstration");
              // }
              // else
              // {
              //   this.alertService.warn("Discharged Resident");
              // }
              //       },
              //       error => {
              //         this.alertService.error(error.message);
              //         this.ng4LoadingSpinnerService.hide();
              //       });
              //             }
              //           }
              //           else
              //           {
              //             this.alertService.warn("ExternalPatientId not Updated");
              //           }
              this.alertService.success(res.m_Item2);
              this.ng4LoadingSpinnerService.hide();
            }
            else if (res.m_Item2 == "No Match Found") {
              this.passAdminister = true;
              this.ng4LoadingSpinnerService.hide();
              this.alertService.error("No match found");
              //this.alertService.error(res.m_Item2);
            }
            else if (res.m_Item1 == 3) {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.error(res.m_Item2);
            };

          },
            error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();

            });
      }
      else {
        this.alertService.error("Biometric computer not configured");
        this.ng4LoadingSpinnerService.hide();
      }
      // this.dataservice.getBiometric<string>(this.config.Biometric_GetTemplateDescByMrNumber + mrNumber + '/' + this.fingerId)
      //   .subscribe(res => {
      //     this.biometricInfo = res;
      //     if (this.biometricInfo.startsWith('Error:')) {
      //       this.alertService.error(this.biometricInfo.substring(6));
      //       this.ng4LoadingSpinnerService.hide();
      //     }
      //     else {
      //       this.ScanFingerPrintVerification(1);
      //       this.ng4LoadingSpinnerService.hide();
      //     }
      //   }, error => {
      //     this.ng4LoadingSpinnerService.hide();
      //     if (error.statusText == "Unknown Error")
      //       this.alertService.error("Biometric application is not running");
      //   });

    }
    else {
      this.alertService.error("Resident has no external patient ID");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  ScanFingerPrintVerification(searchType: number) {
    //searchType - 1 for Verification, 2 for Identification
    // 8.16.2017 - At this time, only SSL client will be supported.
    var uri = "https://localhost:8443/SGIFPCapture";
    var fpobject = '';
    var secugen_lic = '';
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
      if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
        fpobject = JSON.parse(xmlhttp.responseText);
        if (searchType == 1)
          this.SuccessFuncVerification(fpobject);
        else if (searchType == 2)
          this.SuccessFuncIdentification(fpobject);
      }
      else if (xmlhttp.status == 404) {

        this.alertService.error('Check if biometric service is running');
      }
    }.bind(this);
    var params = "Timeout=" + "10000";
    params += "&Quality=" + "50";
    params += "&licstr=" + encodeURIComponent(secugen_lic);
    params += "&templateFormat=" + "ISO";
    xmlhttp.open("POST", uri, true);
    xmlhttp.send(params);

    xmlhttp.onerror = function () {

      this.alertService.error('Check if biometric service is running');
    }.bind(this)
  }
  SuccessFuncVerification(result): any {
    if (result.ErrorCode == 0) {
      console.log(this.biometricInfo);
      this.MatchFingerPrint(result.TemplateBase64, this.biometricInfo);
    }
    else {
      this.alertService.error(this.ErrorCodeToString(result.ErrorCode));
    }
  }
  SuccessFuncIdentification(result): any {
    if (result.ErrorCode == 0) {
      console.log(result.TemplateBase64);
      //this.MatchFingerPrint(result.TemplateBase64, this.biometricInfo);
      this.identifyFingerPrint(this.fingerId, result.TemplateBase64)
    }
    else {
      this.alertService.error(this.ErrorCodeToString(result.ErrorCode));
    }
  }
  MatchFingerPrint(temp2, resbios): any {
    var template_1 = resbios;
    var template_2 = temp2;
    var secugen_lic = '';
    if (template_1 == "" || template_1 == null || template_2 == "") {
      this.alertService.error("Resident data not available");
      return;
    }
    var uri = "https://localhost:8443/SGIMatchScore";
    var fpobject: any = '';
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
      if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
        fpobject = JSON.parse(xmlhttp.responseText);
        this.succMatch(fpobject);
      }
      else if (xmlhttp.status == 404) {
        return 0;
      }
    }.bind(this);

    xmlhttp.onerror = function () {
    }
    var params = "template1=" + encodeURIComponent(template_1);
    params += "&template2=" + encodeURIComponent(template_2);
    params += "&licstr=" + encodeURIComponent(secugen_lic);
    params += "&templateFormat=" + "ISO";
    xmlhttp.open("POST", uri, false);
    xmlhttp.send(params);
  }
  succMatch(result) {
    if (result.ErrorCode == 0) {
      if (result.MatchingScore >= 100) {
        this.getDemographicInfoData(this.residentId);
        this.selectstyle = 2;
        this.barcodevalue = '';
        this.orderItem = -1;
        this.statusFlag = -1;
        this.reSet();
      }
      else {
        this.alertService.error("NOT MATCHED");
      }
    }
    else {
      this.alertService.error(result.ErrorCode);
    }
  }
  ErrorCodeToString(ErrorCode) {
    var Description;
    switch (ErrorCode) {
      case 51:
        Description = "System file load failure";
        break;
      case 52:
        Description = "Sensor chip initialization failed";
        break;
      case 53:
        Description = "Device not found";
        break;
      case 54:
        Description = "Fingerprint image capture timeout";
        break;
      case 55:
        Description = "No device available";
        break;
      case 56:
        Description = "Driver load failed";
        break;
      case 57:
        Description = "Wrong Image";
        break;
      case 58:
        Description = "Lack of bandwidth";
        break;
      case 59:
        Description = "Device Busy";
        break;
      case 60:
        Description = "Cannot get serial number of the device";
        break;
      case 61:
        Description = "Unsupported device";
        break;
      case 63:
        Description = "SgiBioSrv didn't start; Try image capture again";
        break;
      default:

        Description = "Device error. Please check connections";
        break;
    }
    return Description;
  }
  getEmarOrdersList() {
debugger;
    let passtime = this.passTime != null ? this.passTime.replace(':', '-') : null;

    //   let time = this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime == null || this.myform.value.nurseSheduleTime == undefined ? null : this.myform.value.nurseSheduleTime;
    // time = this.myform.value.nurseSheduleTime == null ? null : (this.myform.value.nurseSheduleTime.length == 0 ? null : this.myform.value.nurseSheduleTime[0].replace(':', '-'));
    //this.passTime = time;
    let shift = this.myform.value.nsShiftTime == 0 ? 0 : this.myform.value.nsShiftTime[0].NurseShifts_Id;
    let showTwoHours = this.myform.value.twoHrsWindow == true ? 1 : 0;

    if (this.residentId != undefined && this.residentId != null && (shift != 0 || (passtime != null && passtime != undefined))) {
        this.apiExecuted = false;
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.get<EmarOrdersList[]>(this.config.Emar_Emar_GetEmarOrdersList + this.residentId + "/" + passtime + "/" + this.myform.value.dateCheck + "/" + shift + "/" + showTwoHours + "/" + this.userId)
        .subscribe(res => {
          this.ordersList = res;
          console.log(this.ordersList, 'ordersList');
          console.log(res);
          if (res.length > 0) {
            this.apiExecuted = true;
            setTimeout(() => {
              this.inputField.nativeElement.focus();
            }, 1000);


          }
          console.log(this.apiExecuted, "this.apiExecuted")
          //this.undoButtonDisplay();
          this.ordersListWithoutScanner = this.ordersList.filter(ol => (ol.AdministerStatus == 'Not Administered' || ol.AdministerStatus == 'Administered' || ol.AdministerStatus == '') && ol.dueflag == 0 && ol.ReviewFlag == 1 && (ol.ControlSubstanceFlag == null || ol.ControlSubstanceFlag == 0 || (ol.ControlSubstanceCertifiedBy != null && ol.ControlSubstanceFlag == 1 && ol.ControlSubstanceCertifiedBy.split(',').includes(this.userId.toString()))));
          //this.visible= false;
          // cons
          this.withoutScannerVitals();
          this.byPassSubmit();
          this.ng4LoadingSpinnerService.hide();
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();

        });
    }
    else {
      this.ng4LoadingSpinnerService.hide();
    }

    // }
    // else if (this.myform.value.nsShiftTime != 0) {
    //   this.alertService.warn('Development In Progress');
    // }

  }
  openpanels(searchValue: any, event: any) {

    debugger;
    setTimeout(() => {
      this.ent = false;
    }, 3000);
    this.accordionOpen = false;
    this.undoFlag = false;
    this.administerOrder = null;
    //let record = this.ordersList.find(item => item.Barcode == searchValue);
    let records = [];
    let noDuerecords = [];
    let recordBarcodes = [];
    this.ordersList.every(order => {
      let barcodes = order.Barcode != null ? this.filterPipe.splitData(order.Barcode) : null;
      recordBarcodes.push(barcodes);
      console.log(this.barcodesPList, "barcodesPList")
      if (barcodes != null) {
        // if (barcodes.length == 1 && barcodes[0] == searchValue) {
        //   record = order;
        //   return false;
        // }
        //else


        if (barcodes != undefined && barcodes.length > 0) {

          let barcodevalue = searchValue.split('/');
          let findText = barcodevalue[0];
          let checkBarcodeExist = barcodes.find(item => item.toLowerCase() == findText.toLowerCase());
          if (checkBarcodeExist != undefined && (order.AdministerStatus == 'Not Administered' || order.AdministerStatus == '') && order.dueflag == 0) {
            records.push(order);
            return true;
          }
          else if (checkBarcodeExist != undefined && order.dueflag == 1) {
            noDuerecords.push(order);
            return true;
          }
          else if (checkBarcodeExist != undefined && (order.AdministerStatus == 'Administered')) {
            records.push(order);
            return true;
          }
          else
            return true;
        }
      }
      else
        return true;
      //find(item => item.Barcode == searchValue)
    });

    const flatBarcodes = [].concat(...recordBarcodes);
    console.log(flatBarcodes, "flatBarcodes");
    const uniqueBarcodes = this.barcodesPList.filter(item => !flatBarcodes.includes(item.BarcodeDetails));
    console.log(uniqueBarcodes, "uniqueBarcodes");
    let barcodevalue = searchValue.split('/');
    let findText = barcodevalue[0];
    let notDueFlag = uniqueBarcodes.find(item => item.BarcodeDetails.toLowerCase() == findText.toLowerCase());
    console.log(notDueFlag, "notdueflag");
    //let notDueFlag = uniqueBarcodes.filter(item =>)


    if (searchValue != '') {

      if (records.length == 0) {
        //this.alertService.warn("No Record found with Barcode");
        if (noDuerecords.length > 0) {
          this.barcodevalue = '';
          this.barcodeAlertsPop = true;
          this.barcodeAlert = "Do not administer, this Order is not due at this time"
          // this.alertService.warn("Do not administer, this Order not due at this time");

          // setTimeout(() => {
          //   this.inputField.nativeElement.focus();
          //  }, 1000);
        }
        else {
          if (notDueFlag != undefined) {
            if (notDueFlag.HoldStatus == 1) {
              this.barcodevalue = '';
              this.barcodeAlertsPop = true;
              this.barcodeAlert = "Do not administer, this order is on hold"
              // this.alertService.warn("Do not administer, this order is on hold");

              // setTimeout(() => {
              //   this.inputField.nativeElement.focus();
              //  }, 1000);

            } else if (notDueFlag.OrderStatus == 0) {
              this.barcodevalue = '';
              this.barcodeAlertsPop = true;
              this.barcodeAlert = "Do not administer, this order is inactive";

              // this.alertService.warn("Do not administer, this order is inactive");
              // setTimeout(() => {
              //   this.inputField.nativeElement.focus();
              //  }, 1000);

            }

            else {
              this.barcodevalue = '';
              this.barcodeAlertsPop = true;
              this.barcodeAlert = "Do not administer, this order is not due at this time";

              // this.alertService.warn("Do not administer, this order not due at this time");
              // setTimeout(() => {
              //   this.inputField.nativeElement.focus();
              //  }, 1000);

            }

          }
          else {
            this.barcodevalue = '';
            this.barcodeAlertsPop = true;
            this.barcodeAlert = "Do not administer, this order is not for this Resident";
            // this.alertService.warn("Do not administer, this order is not for this Resident");
            // setTimeout(() => {
            //   this.inputField.nativeElement.focus();
            //  }, 1000);

          }

        }
      }
      //the below condition is to do not administer the system generated barcodes
      else if (searchValue.includes('NW') && records.length == 1 && records[0].OrderStockFlag == false) {
        let barcodevalues = [];
        let patientvalue = this.demographicInfoData.Patient_Id;
        this.ordersList.filter(item => item.dueflag == 0).forEach(item => {
          // Split the barcode by pipe (|) if it contains multiple parts
          let barcodeParts = item.Barcode.split('|');
          // Push each part of the barcode into the array
          barcodeParts.forEach(barcode => {
            if (!barcodevalues.includes(barcode)) {
              barcodevalues.push(barcode);
            }
          });
        })
        console.log(barcodevalues, 'barcodevalues')

        let vall = barcodevalues.find(item => {
          return item == searchValue
        })
        let someval = this.ordersList.filter(item => {
          // Split the barcode by pipe (|)
          const barcodeParts = item.Barcode.split('|');
          // Check if the 'vall' exists in any part of the barcode
          return barcodeParts.some(part => part === vall);
        });
        if (vall == patientvalue + 'NW' + someval[0].POrder_Id) {
          this.barcodevalue = '';
          this.barcodeAlertsPop = true;
          this.barcodeAlert = "Do not administer,Please administer order with stock barcode."
        }
        else {
          if (records.length == 1) {
            if (records[0].PRNFlag == true && records[0].Last_Passed != null) {
              this.getPRNAdministerCount(records[0]);
            }
            else {
              this.checkOrders(records[0]);
            }
          }
          else {
            let dueNow = records.filter(o => o.dueflag == 0);
            if (dueNow.length == 1) {
              if (dueNow[0].PRNFlag == true && dueNow[0].Last_Passed != null) {
                this.getPRNAdministerCount(records[0]);
              }
              else {
                this.checkOrders(dueNow[0]);
              }
            }
            else if (dueNow.length == 0) {
              if (records[0].PRNFlag == true && records[0].Last_Passed != null) {
                this.getPRNAdministerCount(records[0]);
              }
              else {
                this.checkOrders(records[0]);
              }
            }
            else if (dueNow.length > 1) {

              this.dueOrdersCheck = dueNow;
              this.administerBarcodeMatchModal = true;
              //this.alertService.warn('More than one drug with same barcode Due Now.');
            }
          }
        }
      }
      else {
        if (records.length == 1) {
          if (records[0].PRNFlag == true && records[0].Last_Passed != null) {
            this.getPRNAdministerCount(records[0]);
          }
          else {
            this.checkOrders(records[0]);
          }
        }
        else {
          let dueNow = records.filter(o => o.dueflag == 0);
          if (dueNow.length == 1) {
            if (dueNow[0].PRNFlag == true && dueNow[0].Last_Passed != null) {
              this.getPRNAdministerCount(records[0]);
            }
            else {
              this.checkOrders(dueNow[0]);
            }
          }
          else if (dueNow.length == 0) {
            if (records[0].PRNFlag == true && records[0].Last_Passed != null) {
              this.getPRNAdministerCount(records[0]);
            }
            else {
              this.checkOrders(records[0]);
            }
          }
          else if (dueNow.length > 1) {

            this.dueOrdersCheck = dueNow;
            this.administerBarcodeMatchModal = true;
            //this.alertService.warn('More than one drug with same barcode Due Now.');
          }
        }
      }
      //ToDo:
      // else if (record.NoDueFlag == 1) {
      //   this.alertService.warn("This Barcode Orders Due Now Only.");
      // }
      // else if (record.NoDueFlag == 0) {
      //   this.alertService.warn("This Barcode Orders Due Not Now");
      // }
    }
  }
  checkOrders(record: any) {
    debugger;
    this.newProductIsOpen = false;
    this.insulinSites = [];
    if (record.AdministerStatus == 'Not Administered' || record.AdministerStatus == "") {
      if (record.PRNFlag == true && record.Last_Passed != null) {
        let lastPassedDate = this.dateFormatPipe.transform(record.Last_Passed);
        if (lastPassedDate == this.dateFormatPipe.transform(this.myform.value.dateCheck)) {
          this.prnMaxperday = "";
          let caldose = record.Quantity * (this.maxPerDayCount + 1);

          if (this.maxPerDayCount != 0 && caldose > record.MaxPerday) {
            caldose = caldose - record.Quantity;
            console.log(lastPassedDate, "lastPassedDate")
            console.log(this.dateFormatPipe.transform(this.myform.value.dateCheck), " dateFormatPipe date check")
            // this.prnMaxperday="This is a PRN Med which is reached Maxperday limit. Do you still want to Administer this Order again?"
            this.prnMaxperday = `This has been already given ${caldose} ${record.DUOM} ${this.twoHours} today. This dose will
            cause the total amount of drug to exceed maximum quantity per day of ${record.MaxPerday} ${record.DUOM}. Do you still want to administer the drug?`;
            console.log(this.twoHours, "this.twoHours")
          }
          else {
            this.lastPassedTime = this.timeFormatId == 0 ? this.dateFormatPipe.get12HourTime(record.Last_Passed) : this.dateFormatPipe.get24HourTime(record.Last_Passed);
          }
          this.orderId = record.POrder_Id;
          this.quantityId = record.pquantity_Id;
          let administerStatus = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id);
          this.controlSubFlag = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id).ControlSubstanceFlag;
          this.controlSubCertBy = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id).ControlSubstanceCertifiedBy;
          let checkUser = this.controlSubCertBy != null ? this.controlSubCertBy.split(',').includes(this.userId) ? true : false : false;
          if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
            this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
            this.ng4LoadingSpinnerService.hide();
          }
          else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
            this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
            this.ng4LoadingSpinnerService.hide();
          }
          else if (((administerStatus.AdministerStatus == 'Not Administered' || administerStatus.AdministerStatus == '')) && ((this.controlSubFlag == 1 && checkUser == false) || this.controlSubFlag == 2) && this.administerOrder == null) {
            this.alertService.warn("Controlled substance count certification required prior to administration")
            this.ng4LoadingSpinnerService.hide();
          }
          else {

            this.GetDate2hrsDiff(record.Last_Passed, record);
            console.log(this.twoHours, "this.twoHours")
            // setTimeout(() => {
            //   this.modalPRNIsOpen = true;
            // }, 1000);
          }
        }
        else if (lastPassedDate != this.dateFormatPipe.transform(this.myform.value.dateCheck)) {

          this.orderId = record.POrder_Id;
          this.quantityId = record.pquantity_Id;
          let administerStatus = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id);
          this.controlSubFlag = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id).ControlSubstanceFlag;
          this.controlSubCertBy = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id).ControlSubstanceCertifiedBy;
          let checkUser = this.controlSubCertBy != null ? this.controlSubCertBy.split(',').includes(this.userId) ? true : false : false;
          if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
            this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
            this.ng4LoadingSpinnerService.hide();
          }
          else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
            this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
            this.ng4LoadingSpinnerService.hide();
          }
          else if (((administerStatus.AdministerStatus == 'Not Administered' || administerStatus.AdministerStatus == '')) && ((this.controlSubFlag == 1 && checkUser == false) || this.controlSubFlag == 2) && this.administerOrder == null) {
            this.alertService.warn("Controlled substance count certification required prior to administration")
            this.ng4LoadingSpinnerService.hide();
          }
          else {
            this.PRNReasonForm.controls['prnReason'].reset();
            this.modalPRNCommentIsOpen = true;

            setTimeout(() => {
              this.prnOrderFocus.nativeElement.focus();
            }, 300);
          }
        }
        else
          this.getVitalsListForExpandedOrder(record);
        //this.orderGivenStatus(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window);
      }
      else if (record.PRNFlag == true) {
        this.orderId = record.POrder_Id;
        this.quantityId = record.pquantity_Id;
        let administerStatus = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id);
        this.controlSubFlag = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id).ControlSubstanceFlag;
        this.controlSubCertBy = this.ordersList.find(li => li.POrder_Id == record.POrder_Id && li.pquantity_Id == record.pquantity_Id).ControlSubstanceCertifiedBy;
        let checkUser = this.controlSubCertBy != null ? this.controlSubCertBy.split(',').includes(this.userId) ? true : false : false;
        if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
          this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
          this.ng4LoadingSpinnerService.hide();
        }
        else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
          this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
          this.ng4LoadingSpinnerService.hide();
        }
        else if (((administerStatus.AdministerStatus == 'Not Administered' || administerStatus.AdministerStatus == '')) && ((this.controlSubFlag == 1 && checkUser == false) || this.controlSubFlag == 2) && this.administerOrder == null) {
          this.alertService.warn("Controlled substance count certification required prior to administration")
          this.ng4LoadingSpinnerService.hide();
        }
        else {
          this.PRNReasonForm.controls['prnReason'].reset();
          this.modalPRNCommentIsOpen = true;
          setTimeout(() => {
            this.prnOrderFocus.nativeElement.focus();
          }, 300);
        }
      }
      else
        this.getVitalsListForExpandedOrder(record);
      //this.orderGivenStatus(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window);
    }
    else if (record.AdministerStatus == 'Administered') {
      this.barcodevalue = '';
      this.alertService.warn("Order has already been administered.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.barcodevalue = '';
      this.alertService.warn("Barcode not found")
      this.ng4LoadingSpinnerService.hide();
    }
  }
  getNurseCommentNotesByQuantityId(quantityId: any) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetNurseNotes + quantityId)
      .subscribe(res => {
        this.nurseNotes = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  getInsulinCommentsByQuantityId(quantityId: any) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_GetInsulinCommentsByQuantityId + quantityId)
      .subscribe(res => {
        this.insulinCommentsToolTip = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (res != null) {
          if (res.length > 10) {
            this.insulinComments = res.substring(0, 15);
          }
          else {
            this.insulinComments = res;
          }
        }
        else {
          this.insulinComments = "";
        }

      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
  }
  expandOrder(i: any, item: any, undoFlag: boolean, ekitFalg?: any) {
    this.clicked = false;
    this.clk = false;
    if (item.POrder_Id != null && item.POrder_Id != undefined) {
      this.GetRefillNotes(item.POrder_Id)
    }
    if (this.orderItem == i) {
      this.orderItem = -1;
      this.statusFlag = -1;
    }
    else {
      this.orderItem = i;

      if (undoFlag == false)
        this.getVitalsCheckListData(item.pquantity_Id, item.ReviewFlag, item.dueflag, ekitFalg == undefined ? 0 : 1);
      let barcodes = item.Barcode != null ? this.filterPipe.splitData(item.Barcode) : null;
      if (barcodes != null) {
        if (barcodes.length > 0) {
          let checkBarcodeExist = barcodes.find(item => item.toLowerCase() == this.barcodevalue.toLowerCase());
          if (checkBarcodeExist == undefined)
            this.barcodevalue = '';
        }
      }
      // if (item.barcode != this.barcodevalue) {
      //   this.barcodevalue = '';
      // }
      this.accordionOpen = false;
      this.undoFlag = undoFlag;
      this.favouritesForm.reset();
      this.statusFlag = i;
      this.getNurseCommentNotesByQuantityId(item.pquantity_Id);
      this.getInsulinCommentsByQuantityId(item.pquantity_Id);
    }
  }
  getMedicationReason() {
    this.ng4LoadingSpinnerService.show();
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<MedicationReason[]>(this.config.Emar_GetMedicationReason)
      .subscribe(res => {
        this.medicationReasonList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading(); this.alertService.error(error.message);

      });
  }
  reSet() {
    this.favouritesForm.reset();
    this.medicationForm.patchValue({
      note: '',
      medicationReason: '',
      undoQuantity: '',
    });
    this.withoutAdminsterForm.patchValue({
      reasonForWithoutAdministred: '',
    });
    this.closeVitalsModel();
    this.closeUndoModel();
    this.closePRNModel();
    this.barcodeScanCheckbox = false;
  }
  administerPRNOrder(prnCheck: any) {
    debugger
    this.modalPRNIsOpen = false;
    let record = this.ordersList.find(li => li.POrder_Id == this.orderId && li.pquantity_Id == this.quantityId);
    if (record != null) {
      if (record.PRNFlag == true) {
        this.PRNReasonForm.controls['prnReason'].reset();
        this.prnAdministerFlag = prnCheck;
        this.modalPRNCommentIsOpen = true;
        setTimeout(() => {
          this.prnOrderFocus.nativeElement.focus();
        }, 300);
      }
      //this.orderGivenStatus(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window);
    }
  }
  administerPRNOrderReason() {
    debugger
    this.modalPRNCommentIsOpen = false;
    let record = this.ordersList.find(li => li.POrder_Id == this.orderId && li.pquantity_Id == this.quantityId);
    if (record != null) {
      if (record.PRNFlag == true) {
        debugger
        this.getVitalsListForExpandedOrder(record);
        console.log('administerprnorder reason')
        // if (record.Ekit == null || record.Ekit == 0) {
        //   //this.modalPRNCommentIsOpen =true;
        //   this.orderGivenStatus(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window);
        // }
        // else if (record.Ekit != undefined && record.Ekit == 1) {
        //   this.getVitalsCheckListData(record.pquantity_Id, record.ReviewFlag, record.dueflag, 1);
        // }
      }
    }

  }
  barcodeScanCheckboxChaeck() {
    this.ng4LoadingSpinnerService.show();
    //(async () => {
    //this.ordersListWithoutScanner=this.ordersList.filter(ol=>(ol.AdministerStatus=='Not Administered' ||ol.AdministerStatus=='') && ol.dueflag==0);
    this.withoutAdminsterForm.patchValue({
      reasonForWithoutAdministred: '',
    });
    this.ordersListSelected = [];
    this.favouritesForm.reset();
    this.CheckVitalsObj = [];
    this.insulinSiteIdsWS = [];
    //await this.delay(500);
    //this.modalAdministerWithoutBarcodeOpen=true;
    if (this.barcodeScanCheckbox == true) {
      this.afterGetVitalsList();
      this.afterDiscardDateGetWS();
    }
    this.ng4LoadingSpinnerService.hide();
    //})();
  }
  GetDiagnosisDetails() {
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
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }
  // InsertAdministredwithoutscanning(orderId: number, quantityId: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   //ToDo: Anitha - change this
  //   this.nursecommentsObj = {
  //     Comments_Id: 0,
  //     //DrugAdminister_Id: DrugAdminister_Id,
  //     POrder_Id: orderId,
  //     pquantity_Id: quantityId,
  //     NurseCommentType_Id: 4,
  //     Comment: this.withoutAdminsterForm.value.reasonForWithoutAdministred,
  //     comment_Status: 1,
  //     comment_CreatedBy: this.userId,
  //     Comment_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
  //   };
  //   this.dataservice.post(this.config.Emar_NurseComments_InsertAdministredWithoutScanning, this.nursecommentsObj)
  //     .subscribe(res => {
  //       this.persistanceService.getDueMARAlert();
  //       //this.orderGivenStatus(DrugAdminister_Id);
  //       this.ng4LoadingSpinnerService.hide();

  //     }, error => {
  //       this.ng4LoadingSpinnerService.hide();
  //       this.alertService.error(error.message);
  //     })
  // }
  orderGivenStatus(orderId: number, quantityId: number, reviewFlag: number, PRNFlag: boolean, dueFlag: number, inputTime: string, shiftId: number, window: number) {
    //POrder_Id, item.pquantity_Id
    debugger;
    setTimeout(() => {
      this.ent = false;
    }, 300);
    this.ng4LoadingSpinnerService.show();
    console.log(this.drugAdminsterObj, "start drug")
    this.orderDueFlag = dueFlag == 0 ? false : true;
    if (dueFlag != 0) {
      this.barcodevalue = '';
      this.barcodeAlertsPop = true;
      this.barcodeAlert = "Order is not due at this time";
      // this.alertService.warn("Order is not due at this time")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (reviewFlag != 1) {
      this.alertService.warn("Order is pending review and cannot be administered until review is completed.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      //Right Administer Time Logiv. Not Needed
      // passTimes = passTimes.trim().replace(/AM/g, ' AM');
      // passTimes = passTimes.replace(/PM/g, ' PM');
      // let passTimesArray = passTimes.split(', ');
      // let rightAdministerPassTime: any = '';
      // if (passTimesArray.length > 0) {
      //   passTimesArray.every(element => {
      //     console.log(element);
      //     //let res = this.adminsterRightTime(element);
      //     //console.log(res);
      //     if (this.adminsterRightTime(element) == true) {
      //       rightAdministerPassTime = element; return false;
      //     }
      //     else
      //       return true;
      //   });
      // }
      // if (rightAdministerPassTime != '') {
      //   console.log(rightAdministerPassTime);
      let barcodesList = this.ordersList.find(o => o.POrder_Id == orderId && o.pquantity_Id == quantityId).Barcode;
      let rightAdministerPassTime;
      if (this.passTime != null)
        rightAdministerPassTime = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.passTime.replace('-', ':');
      else
        rightAdministerPassTime = this.dateFormatPipe.transform(this.myform.value.dateCheck);
      let administerStatus = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId);
      this.controlSubFlag = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId).ControlSubstanceFlag;
      this.controlSubCertBy = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId).ControlSubstanceCertifiedBy;
      let checkUser = this.controlSubCertBy != null ? this.controlSubCertBy.split(',').includes(this.userId) ? true : false : false;
      let barcode = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId).Barcode;
      if (barcode == undefined || barcode == null || barcode == '') {
        this.alertService.warn("Order has no barcode.")
        this.ng4LoadingSpinnerService.hide();
      }
      else if (((administerStatus.AdministerStatus == 'Not Administered' || administerStatus.AdministerStatus == '')) && ((this.controlSubFlag == 1 && checkUser == false) || this.controlSubFlag == 2) && this.administerOrder == null) {
        this.alertService.warn("Controlled substance count certification required prior to administration")
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        let orderItem = this.ordersList.find(ele => ele.POrder_Id == orderId && ele.pquantity_Id == quantityId);
        if (orderItem.Undo == 0 && orderItem.Ekit == 1) {
          this.undoFlag = false
        }
        if (this.undoFlag == false) {

          if (this.barcodeScanCheckbox == true && this.withoutAdminsterForm.value.reasonForWithoutAdministred == '')
            this.alertService.warn("Please enter the reason for not using the scanner");
          else {
            if ((orderItem.Route == "SC" || orderItem.Route == "IM" || orderItem.Route == "TD") && this.insulinSites.length == 0) {
              this.displayInsulinSites = orderItem.Route == "TD" ? this.allInsulinSites.filter(f => f.item_type == 2) : this.allInsulinSites.filter(f => f.item_type == 1);
              this.lastUsedSites = orderItem.AdministerSites != null && orderItem.AdministerSites != "" ? orderItem.AdministerSites : "";
              this.insulinSites = [];
              this.administerFromEkitModal = false;
              this.displayInsulinSitesChecksReset();
              this.sitesType = orderItem.Route == "TD" ? "Patch Sites" : "Administration Sites";
              this.modalAdministerInsulinSiteIsOpen = true;
              this.orderId = orderId;
              this.quantityId = quantityId;
            }
            else if (this.newProductIsOpen == false && orderItem != undefined && orderItem.DiscardDays != null && orderItem.DiscardDays != 0 && orderItem.DiscardDays <= 3) {

              //var discardDate=orderItem.DiscardDate;
              this.newProductIsOpen = false;
              this.manuallySetDiscard = false;
              this.discardform.reset();
              this.discardDateChange(orderItem.DiscardDate);
              this.orderId = orderId;
              this.quantityId = quantityId;
              this.discardform.patchValue({
                discardDate: this.dateFormatPipe.transformISODate(orderItem.DiscardDate)
              });
              this.modalDiscardDateIsOpen = true;
            }
            else if (this.vitalsCheckList.length != this.CheckVitalsObj.length) {
              this.admScheduleForVitalsSave = rightAdministerPassTime;//this.dateFormatPipe.transform(this.myform.value.dateCheck);
              //this.alertService.warn("Check Vitals Before Administer.");
              this.orderId = orderId;
              this.quantityId = quantityId;
              this.inputTime = inputTime;
              console.log(this.inputTime, 'inputTime1')
              this.shiftId = shiftId;
              this.window = window;
              this.modalVitalsIsOpen = true;
              this.favouritesForm.reset()
              setTimeout(() => {
                this.textFocus.nativeElement.focus();
              }, 500);
            }
            else {
              // if (this.medicationForm.value.medicationReason[0].MedicationReason_ID == 7 && this.medicationForm.value.note == '')
              //   this.alertService.warn("Please enter note");
              //else {
              // if (((this.ekit != null && this.ekit != undefined && this.ekit.length != 0) && (this.quantity == '' || this.quantity == null)) || ((this.ekit == null || this.ekit == undefined || this.ekit.length == 0) && this.quantity != '')){
              //   // this.alertService.warn("Please enter ekit details");

              // }
              if (this.ekit != null && this.ekit != undefined && this.ekit.length != 0 && parseFloat(this.ekit[0].Ekit_Id) == 0) {
                this.alertService.warn("Please enter valid numeric quantity");
                this.quantity = "";
              }
              else if (this.ekit != null && this.ekit != undefined && this.ekit.length != 0 && this.ekit.length != 0 && this.quantity != '' && this.checkEkitQuantity() == true) {
                //this.alertService.warn("Entered more quantity.You have only " + this.quantityInhand + " On Hand Quantity");
                this.alertService.warn("Quantity entered exceeds on-hand quantity");
                this.quantity = "";
              }
              else {
                //if (this.barcodeScanCheckbox == true && this.withoutAdminsterForm.value.reasonForWithoutAdministred != '') {
                //this.InsertAdministredwithoutscanning(orderId, quantityId);
                //}
                console.log("wkit 2")

                console.log(this.drugAdminsterObj, "second drug")

                this.drugAdminsterObj = {
                  //DrugAdminister_Id: drugAdministor_Id,
                  POrder_Id: orderId,
                  pquantity_Id: quantityId,
                  AdminsterSchedule: rightAdministerPassTime,//this.dateFormatPipe.transform(this.myform.value.dateCheck),
                  AdministerComment: this.PRNReasonForm.value.prnReason,//this.medicationForm.value.note,
                  AdminsterStatus: 1,
                  AdminsterBy: this.userId,
                  AdminsterOn: this.dateFormatPipe.dateWithTime(new Date()),
                  MedicationReason_ID: 1,
                  BCScanner: this.barcodeScanCheckbox == true ? 1 : 0,
                  BCScannerText: this.withoutAdminsterForm.value.reasonForWithoutAdministred,
                  Ekit_Id: this.ekit == null || this.ekit == undefined || this.ekit.length == 0 ? 0 : this.ekit[0].Ekit_Id,
                  //Ekit_Id: this.ekitSelctedarray == null || this.ekitSelctedarray == undefined || this.ekitSelctedarray.length == 0 ? 0 : this.ekitSelctedarray.length >1 ?this.lotsGrid[0].Ekit_Id : this.ekitSelctedarray[0],
                  Quantity: this.quantity,
                  Patient_Id: this.residentId,
                  DrugQuantity: this.undoFlag == false ? 0 : this.medicationForm.value.undoQuantity,
                  ByPassReason: this.byPassReason,
                  InputTime: inputTime,
                  ShiftId: shiftId,
                  Window: window,
                  PRNFlag: PRNFlag,
                  UndoFlag: this.undoFlag,
                  Last_Passed: null,
                  AdditionalComments: this.additionalComForm.value.addCom,
                  AdministeredBarcode: this.barcodeScanCheckbox == false ? this.barcodevalue : barcodesList,
                  DiscardDate: this.discardform.value.discardDate != undefined && this.discardform.value.discardDate != null && this.discardform.value.discardDate != "" ? this.dateFormatPipe.dateFormat(this.discardform.value.discardDate) : null,
                  AdministerInsulinSites: this.insulinSites.length == 0 ? null : this.insulinSites.join(','),
                  RouteCode: this.insulinSites.length == 0 ? null : this.ordersList.find(o => o.POrder_Id == orderId).Route,
                  // Ekit_AllIds:this.ekitSelctedarray.toString()
                  Ekit_AllIds: this.dataEkitPost
                };
              }
              //}
            }
          }
        }
        else {
          console.log(this.drugAdminsterObj, "third drug")

          // if (this.medicationForm.value.medicationReason[0].MedicationReason_ID == 0)
          //   this.alertService.warn("Please select reason");
          if ((this.medicationForm.value.medicationReason != null && this.medicationForm.value.medicationReason.length > 0) && this.medicationForm.value.medicationReason[0].MedicationReason_ID == 7 && this.medicationForm.value.note == '')
            this.alertService.warn("Please enter note");
          // else if (((this.ekit != null && this.ekit != undefined && this.ekit.length != 0) && (this.quantity == '' || this.quantity == null)) || ((this.ekit == null || this.ekit == undefined || this.ekit.length == 0) && this.quantity != ''))
          //   this.alertService.warn("Please enter ekit details");
          // else if (this.ekit != null && this.ekit.length != 0 && this.ekit != undefined && parseFloat(this.ekit[0].Ekit_Id) == 0) {
          //   this.alertService.warn("Please enter valid numeric quantity");
          //   this.quantity = "";
          // }
          // else if (this.ekit != null && this.ekit != undefined && this.ekit.length != 0 && this.quantity != '' && this.checkEkitQuantity() == true) {
          //   this.alertService.warn("Entered more quantity. You have only " + this.quantityInhand + " On Hand Quantity");
          //  this.quantity = "";
          // }
          else {
            this.drugAdminsterObj = {
              //DrugAdminister_Id: drugAdministor_Id,
              POrder_Id: orderId,
              pquantity_Id: quantityId,
              AdminsterSchedule: rightAdministerPassTime,//this.dateFormatPipe.transform(this.myform.value.dateCheck),
              AdministerComment: this.medicationForm.value.note,
              AdminsterStatus: 0,
              AdminsterBy: this.userId,
              AdminsterOn: this.dateFormatPipe.dateWithTime(new Date()),
              MedicationReason_ID: (this.medicationForm.value.medicationReason != null && this.medicationForm.value.medicationReason.length > 0) ? this.medicationForm.value.medicationReason[0].MedicationReason_ID : 0,
              BCScanner: this.barcodeScanCheckbox == true ? 1 : 0,
              BCScannerText: this.withoutAdminsterForm.value.reasonForWithoutAdministred,
              Ekit_Id: this.ekit == null || this.ekit == undefined || this.ekit.length == 0 ? 0 : this.ekit[0].Ekit_Id,
              //Ekit_Id: this.ekitSelctedarray == null || this.ekitSelctedarray == undefined || this.ekitSelctedarray.length == 0 ? 0 : this.ekitSelctedarray.length >1 ?this.lotsGrid[0].Ekit_Id : this.ekitSelctedarray[0],
              Quantity: this.quantity,
              Patient_Id: this.residentId,
              DrugQuantity: this.undoFlag == true && this.medicationForm.value.undoQuantity != '' ? this.medicationForm.value.undoQuantity : 0,
              ByPassReason: this.byPassReason,
              InputTime: inputTime,
              ShiftId: shiftId,
              Window: window,
              PRNFlag: PRNFlag,
              UndoFlag: this.undoFlag,
              Last_Passed: this.ordersList.find(o => o.POrder_Id == orderId && o.pquantity_Id == quantityId).Last_Passed != null ? this.ordersList.find(o => o.POrder_Id == orderId && o.pquantity_Id == quantityId).Last_Passed.split('/').join('-') : null,
              AdditionalComments: this.additionalComForm.value.addCom,
              AdministeredBarcode: this.barcodeScanCheckbox == false ? this.barcodevalue : barcodesList,
              DiscardDate: this.undoFlag == true ? null : (this.discardform.value.discardDate != undefined && this.discardform.value.discardDate != null && this.discardform.value.discardDate != "" ? this.dateFormatPipe.dateFormat(this.discardform.value.discardDate) : null),
              AdministerInsulinSites: this.undoFlag == true ? null : this.insulinSites.join(','),
              RouteCode: this.undoFlag == true ? null : this.ordersList.find(o => o.POrder_Id == orderId).Route,
              //Ekit_AllIds:this.ekitSelctedarray.toString()
              Ekit_AllIds: this.dataEkitPost


            };
          }
        }
        console.log(this.drugAdminsterObj, "final drug")

        if (this.drugAdminsterObj != null) {
          this.spinnerLoading++;
          this.checkAndHideSpinnerLoading();
          this.dataservice.post(this.config.Emar_Emar_InsertDrugAdminister, this.drugAdminsterObj)
            .subscribe(res => {
              this.ng4LoadingSpinnerService.show();
              this.reSet();
              this.getEmarOrdersList();
              this.dataEkitPost = [];
              //this.getEkitDrop(this.nurseStationvalue);
              //this.getEmarResidentGridData();
              this.persistanceService.getDueMARAlert();
              this.listChage = 0;
              if (res == 1 && ((this.undoFlag == false) || (this.undoFlag == true && this.noShowFlag == true))) {
                this.alertService.success('Order administration successful');

              }
              else if (res == 1 && this.undoFlag == true && this.noShowFlag == false)
                this.alertService.success('Undo order administration successful');
              else if (res == 0)
                this.alertService.error('Order administration failed');
              this.orderItem = -1;
              this.statusFlag = -1;
              this.undoFlag = !this.undoFlag;
              this.CheckVitalsObj = [];
              this.barcodeScanCheckbox = false;
              this.ekit = [];
              this.lotsGrid = [];
              this.dataEkitPost = [];
              this.ekitSelctedarray = []
              this.quantity = '';
              this.administerFromEkitModal = false;
              this.modalNoShowAdministerIsOpen = false;
              this.administerOrder = null;
              this.addComments = 0;
              this.tempOrderId = 0;
              this.tempQtyId = 0;
              this.withoutAdminsterForm.patchValue({
                reasonForWithoutAdministred: '',
              });
              this.discardform.reset();
              this.newProductIsOpen = false;
              this.insulinSites = [];
              this.getDosesDetails(0, null);
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
              // this.ng4LoadingSpinnerService.hide();
            }, error => {
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading(); this.alertService.error(error.message);

            });
          //this.visible = true;
        }
      }
      this.drugAdminsterObj = null;
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();      // }
      // else {
      //   this.alertService.warn("This Order is Not Due now");
      //   this.ng4LoadingSpinnerService.hide();
      // }
    }
  }
  undoGivenOrder(i: number, item: any) {


    if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.medicationForm.reset();
      const undoQuantityValidations = this.medicationForm.get('undoQuantity');
      undoQuantityValidations.setValidators([Validators.required]);;
      undoQuantityValidations.updateValueAndValidity();
      this.medicationForm.patchValue({
        undoQuantity: '0'
      })
      this.undoFlag = true;
      this.noShowFlag = false;
      this.undoOrderDetails = item;
      this.modalUndoIsOpen = true;

      this.barcodevalue = this.ordersList[0].ABarcode;
      if (this.orderItem != i)
        this.expandOrder(i, item, true);
    }
  }
  insertRefill(item: any) {

    this.ng4LoadingSpinnerService.show();
    this.refillObj = {
      Refill_Id: 0,
      Porder_Id: item.POrder_Id,
      Patient_Id: this.demographicInfoData.Patient_Id,
      NumberOfRefillsRemaining: item.NumberOfRefillsRemaining,
      Refill_Status: 1,
      Refill_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Refill_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
      POOutBoundFileStatus: 1,
      POOutBoundApproval: null,
      POOutBoundApprovalBy: null,
      POOutBoundApprovalOn: null,

    };
    let orderid = item.POrder_Id
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_AdminApproval_InsertRefill, this.refillObj)
      .subscribe(res => {
        if (res == 1) {
          this.alertService.success("Refill request sent to pharmacy");
          this.getEmarOrdersList();
          this.GetRefillNotes(orderid);

        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading(); this.alertService.error(error.message);

        });

  }
  getVitalsCheckListData(PQuantityId: number, reviewFlag: number, dueFlag: number, ekitFalg?: any) {
    debugger
    console.log(this.selectEkitGpi, "selectEkitGpi")
    if (dueFlag != 0) {
      this.orderDueFlag = true;
      this.barcodevalue = '';
      this.barcodeAlertsPop = true;
      this.barcodeAlert = "Order is not due at this time";
      // this.alertService.warn("Order is not due at this time")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (reviewFlag != 1) {
      this.alertService.warn("Order is pending review and cannot be administered until review is completed.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.orderDueFlag = false;
      this.admScheduleForVitalsSave = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.passTime.replace('-', ':');

      console.log(this.admScheduleForVitalsSave, this.myform.value.dateCheck, "dT")
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.get<any[]>(this.config.Emar_GetVitalsCheckList + PQuantityId)
        .subscribe(res => {
          this.vitalsCheckList = res;
          if (ekitFalg != undefined && ekitFalg == 1 && res.length > 0) {
            this.modalVitalsIsOpen = true;
            this.favouritesForm.reset()
            setTimeout(() => {
              this.textFocus.nativeElement.focus();
            }, 300);
          }

          else if (ekitFalg != undefined && ekitFalg == 1 && this.ekitItem != undefined) {
            console.log(this.barcodeExpAlert, "barcodeExpAlert")
            console.log(this.ekitItem, "this.ekitItem")
            //issue we got for stock administration without ekit

            this.administerFromEkitModal = true;

            this.ekitDrugDropdown(this.selectEkitGpi, this.selectEkitDrug)
            setTimeout(() => {
              this.ekitItem.nativeElement.focus();
            }, 300);
          }
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        },
          error => {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();

          });
    }
    // if (dueFlag == 0) {
    //   this.orderDueFlag = false;
    //   this.admScheduleForVitalsSave = this.dateFormatPipe.transform(this.myform.value.dateCheck);// + " " + rightAdministerPassTime;
    // }
    // else if (dueFlag == 1) {
    //   this.orderDueFlag = true;
    //   //this.alertService.warn("This Order is Not Due now");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    // else if (reviewFlag != 1) {
    //   this.alertService.warn("Order is pending review and cannot be administered until review is completed.")
    //   this.ng4LoadingSpinnerService.hide();
    // }
    //else {
    //Right Administer Time Check is not Needed
    // passTimes = passTimes.trim().replace(/AM/g, ' AM');
    // passTimes = passTimes.replace(/PM/g, ' PM');
    // let passTimesArray = passTimes.split(', ');
    // let rightAdministerPassTime: any = '';
    // if (passTimesArray.length > 0) {
    //   passTimesArray.every(element => {
    //     console.log(element);
    //     //let res = this.adminsterRightTime(element);
    //     //console.log(res);
    //     if (this.adminsterRightTime(element) == true) {
    //       rightAdministerPassTime = element; return false;
    //     }
    //     else
    //       return true;
    //   });
    //}
  }
  checkVitalsComment(OrderFavMaster_ID: number, Value: string) {
    debugger;
    this.ng4LoadingSpinnerService.show();
    this.admScheduleForVitalsSave = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.passTime.replace('-', ':');
    if (Value != "") {
      debugger
      // console.log(this.administerOrder, 'this.administerOrder')
      // console.log(this.inputTime, 'this.inputTime')
      // console.log(this.administerOrder.InputTime, 'this.administerOrder.InputTime')
      // console.log(this.administerWithoutScannerInputTime, 'this.administerWithoutScannerInputTime')
      // console.log(this.shiftId, 'this.shiftId')
      // console.log(this.administerOrder.ShiftId, 'this.administerOrder.ShiftId')
      // console.log(this.administerWithoutScannerShiftId, 'this.administerWithoutScannerShiftId')
      // console.log(this.window, 'this.window')
      // console.log(this.administerWithoutScannerWindow, 'this.administerWithoutScannerWindow')
      // console.log(this.administerOrder.Window, 'this.administerOrder.Window')

      let favObj = new OrderFavouriteData();
      favObj.FavouriteData_Id = 0;
      favObj.POrder_Id = this.orderId;
      favObj.pquantity_Id = this.quantityId;
      favObj.AdminsterSchedule = this.admScheduleForVitalsSave;
      // changed here input time and shift id and window,
      favObj.InputTime = this.inputTime;
      favObj.ShiftId = this.shiftId;
      favObj.Window = this.window;

      if (this.inputTime == '' || this.inputTime == undefined || this.inputTime == null) {
        debugger
        if (this.administerWithoutScannerInputTime != '' && this.administerWithoutScannerInputTime != undefined && this.administerWithoutScannerInputTime != null) {
          debugger
          favObj.InputTime = this.administerWithoutScannerInputTime
        }
        else {
          favObj.InputTime = this.administerOrder.InputTime;
        }
      }





      if (this.shiftId == undefined || this.shiftId == null) {
        debugger
        if (this.administerWithoutScannerShiftId != undefined && this.administerWithoutScannerShiftId != null) {
          debugger
          favObj.ShiftId = this.administerWithoutScannerShiftId
        }
        else {
          debugger
          favObj.ShiftId = this.administerOrder.ShiftId;
        }
      }

      if (this.window == undefined || this.window == null) {
        debugger
        favObj.Window = this.window
        if (this.administerWithoutScannerWindow != undefined && this.administerWithoutScannerWindow != null) {
          debugger
          favObj.Window = this.administerWithoutScannerWindow
        }
        else {
          debugger
          favObj.Window = this.administerOrder.Window;
        }
      }

      // favObj.InputTime =  this.inputTime? this.inputTime:this.administerOrder.InputTime;
      // favObj.ShiftId = this.shiftId?this.shiftId:this.administerOrder.ShiftId;
      // favObj.Window = this.window?this.window:this.administerOrder.Window;

      favObj.OrderFavMaster_ID = OrderFavMaster_ID;
      favObj.value = Value;
      favObj.FavouriteData_Status = 1;
      favObj.FavouriteData_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      favObj.Favourite_CreatedOn = this.dateFormatPipe.dateWithTime(new Date());
      let index = this.CheckVitalsObj.findIndex(cs => cs.OrderFavMaster_ID == OrderFavMaster_ID);
      console.log(favObj, 'favobj')
      if (index >= 0) {
        this.CheckVitalsObj.splice(index, 1);
        this.CheckVitalsObj.push(favObj);
        console.log(this.CheckVitalsObj, 'this.CheckVitalsObj 1')
      }
      else
        this.CheckVitalsObj.push(favObj);
      console.log(this.CheckVitalsObj, 'this.CheckVitalsObj 2')
    }
    else if (Value == "") {
      let index = this.CheckVitalsObj.findIndex(cs => cs.OrderFavMaster_ID == OrderFavMaster_ID);
      this.CheckVitalsObj.splice(index, 1);
      console.log(this.CheckVitalsObj, 'this.CheckVitalsObj 3')
    }
    if (this.CheckVitalsObj.length != this.vitalsCheckList.length) {
      const vitalsvalidation = this.favouritesForm.get('comments');
      //barcodevalidation.setValidators(null);
      vitalsvalidation.setValidators([Validators.required]);
      vitalsvalidation.updateValueAndValidity();
      this.vitalsStatus = 1;
    }
    else {
      const vitalsvalidation = this.favouritesForm.get('comments');
      //barcodevalidation.setValidators(null);
      vitalsvalidation.setValidators(null);
      vitalsvalidation.clearValidators();
      vitalsvalidation.updateValueAndValidity();
      this.vitalsStatus = 0;
    }
    this.ng4LoadingSpinnerService.hide();
  }
  vitalsCheckSave() {
    debugger
    console.log(this.selectEkitGpi, "selectEkitGpi")
    this.ng4LoadingSpinnerService.show();
    if (this.vitalsCheckList.length == this.CheckVitalsObj.length) {
      console.log(this.CheckVitalsObj, "CheckVitalsObj")
      this.dataservice.post(this.config.Emar_InsertOrderFavouritesData, this.CheckVitalsObj)
        .subscribe(res => {
          //this.alertService.success("Saved Successfully");
          //this.vitalCheckStatus = "Vitals Check Completed";

          this.ng4LoadingSpinnerService.hide();
          let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.CheckVitalsObj[0].POrder_Id && ele.pquantity_Id == this.CheckVitalsObj[0].pquantity_Id);

          console.log(orderItem, ' orderItem')
          if (orderItem != undefined && this.administerOrder == null) {
            this.orderGivenStatus(orderItem.POrder_Id, orderItem.pquantity_Id, orderItem.ReviewFlag, orderItem.PRNFlag, orderItem.dueflag, orderItem.InputTime, orderItem.ShiftId, orderItem.Window);
            this.favouritesForm.reset();
            this.modalVitalsIsOpen = false;
          }
          else if (this.administerOrder != null) {
            this.modalVitalsIsOpen = false;
            console.log(this.barcodeExpAlert, "barcodeExpAlert")

            this.administerFromEkitModal = true;
            this.ekitDrugDropdown(this.selectEkitGpi, this.selectEkitDrug)
            setTimeout(() => {
              this.ekitItem.nativeElement.focus();
            }, 300);
          }
          else
            this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();

          });
    }
    else {
      this.alertService.warn("Please fill all required User Inputs.")
      this.ng4LoadingSpinnerService.hide();
    }
    window.scroll(0, 0);
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
  getResidentBiometric() {
    if (this.gridList.length > 0)
      this.ScanFingerPrintVerification(2);
    else
      this.alertService.error("No Orders Scheduled at selected time.");
  }
  /// Start Biometric Verification and Identification

  // getCompanyConfiguredBiometricFinger(stationId: number) {
  //   this.dataservice.get<any>(this.config.Emar_Company_GetBiometricFingerConfigByNsId + stationId)
  //     .subscribe(res => {
  //       this.fingerId = res;
  //     }, error => {
  //       this.alertService.error(error.message);
  //     });
  // }
  getAllFlagsForCompanyByNSId(stationId: number) {

    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId)
      .subscribe(res => {
        this.fingerId = res.Fingersdesc_Id;
        this.timeFormatId = res.TimeFormat;
        this.getNursingStationTimeZone(stationId);

        //this.getComputersListByNsId(stationId);
      }, error => {
        this.alertService.error(error.message);

      });
  }
  getNursingStationTimeZone(stationId: number) {

    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {

        this.nursingStationZoneCurrentDate = res;
        //this.administrationDate=this.dateFormatPipe.transformISODate(res);
        this.myform.patchValue({
          dateCheck: this.dateFormatPipe.transformISODate(res),
        });
        this.minDate = this.dateFormatPipe.transformISODate(res);
        this.today = this.dateFormatPipe.transformISODate(res);

      }, error => {
        this.alertService.error(error.message);

      });
  }
  identifyFingerPrint(fingerId, template: any) {
    let requestObj = {
      Type: "search",
      ResidentId: 0,
      FingerDescId: fingerId,
      UserId: this.userId,
      FPTemplate: template
    }

    this.dataservice.postBiometric(this.config.Biometric_Common_IdentifyResident, requestObj)
      .subscribe(res => {

        if (res != null) {
          if (res.m_Item2 == "success") {
            console.log(res.m_Item1);
            let record = this.gridList.find(g => g.PatientMRNumber == res.m_Item1);
            console.log(record);
            if (record != undefined) {
              this.getDemographicInfoData(record.Patient_Id);
              this.selectstyle = 2;
              this.barcodevalue = '';
              this.orderItem = -1;
              this.statusFlag = -1;
              this.reSet();
            }
            else {
              this.alertService.error("Resident found but no schedules");
            }
          }
          else if (res.m_Item2 == "No Candidate") {
            this.alertService.error("No record found");
          }
          else if (res.m_Item2 == "NoMRNumber") {
            this.alertService.error("Resident mr number not found.");
          }
          else {
            this.alertService.error(res.m_Item2);
          }
        }
        else {
          this.alertService.error("Some thing wrong with Service. Please contact technical support")
        }
      }, error => {
        this.alertService.error("System Error. Please contact technical support");

      });
  }
  /// End 1:N
  getShiftHoursMasterData() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    let scheduleDate = this.myform.value.dateCheck.split('/').join('-');
    this.dataservice.get<any[]>(this.config.Emar_GetNurseShiftDrop + this.nurseStationvalue + "/" + scheduleDate)
      .subscribe(res => {
        this.shiftsList = res;
        this.ng4LoadingSpinnerService.hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
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

  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.timeDropList = [];
    this.gridList = [];
    this.shiftsList = [];
    this.pieChartFlag = false;
    this.timesDisplay = false;
    this.shiftDisplay = false;
    this.windowCheck = true;
    this.pendingOrdersList = [];
    this.adminform.patchValue({
      nstationName: '',
      twoHrsWindow: 0,
    });
    this.myform.patchValue({
      nurseSheduleTime: 0,
      nsShiftTime: 0,
      twoHrsWindow: 0
    });
    this.loginUserReceNurseStation = undefined;
    this.getNurseStations(item.Facility_Id);
    this.sharedService.changeFacilityId(item.Facility_Id);

  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.timeDropList = [];
    this.pendingOrdersList = [];
    //this.selectedTimeItems = [];
    this.adminform.patchValue({
      nstationName: '',
      twoHrsWindow: 0,
    });
    this.myform.patchValue({
      nurseSheduleTime: 0,
      nsShiftTime: 0,
      twoHrsWindow: 0
    });
    this.loginUserReceNurseStation = undefined;
    this.gridList = [];
    this.nurseStations = [];
    this.timeDropList = [];
    this.shiftsList = [];
    this.pieChartFlag = false;
    this.timesDisplay = false;
    this.shiftDisplay = false;
    this.windowCheck = true;
  }
  onEkitSelect(item: any) {
    if (this.ekit != null && this.ekit != undefined && parseFloat(this.ekit[0].Ekit_Id) == 0) {
      this.alertService.warn("Please enter valid numeric quantity");
      this.quantity = "";
    }
    else if (this.quantity != null && this.quantity != '' && this.checkEkitQuantity() == true) {
      //this.alertService.warn("Entered more quantity.You have only " + this.quantityInhand + " On Hand Quantity");
      this.alertService.warn("Quantity entered exceeds on-hand quantity");
      this.quantity = "";
    }
  }
  onEkitDeSelect(item: any) {
    if (this.quantity != null && this.quantity != '')
      this.alertService.warn("Please enter ekit details");
  }
  openPendingForReviewModal(orderId: any, quantityId: any) {

    this.sharedService.changePatientId(this.residentId);
    this.sharedService.changeOrderId(orderId);
    this.sharedService.changeQuantityId(quantityId);
    const modalRef = this.modalService.open(PendingforreviewComponent, { size: 'lg', windowClass: 'my-class' });
    modalRef.componentInstance.modalTitle = this.demographicInfoData.PatientMiddleInitial == null ? "Pending for Review" + " " + "(" + this.demographicInfoData.PatientLastName + ", " + this.demographicInfoData.PatientFirstName + "  " + this.dateFormatPipe.transform(this.demographicInfoData.DOB) + ")" : "Pending for Review" + " " + "(" + this.demographicInfoData.PatientLastName + ", " + this.demographicInfoData.PatientFirstName + " " + this.demographicInfoData.PatientMiddleInitial + "  " + this.dateFormatPipe.transform(this.demographicInfoData.DOB) + ")";
    modalRef.componentInstance.nsId = this.nurseStationvalue;
    modalRef.componentInstance.reviewInfoResult.subscribe((receivedResult) => {
      this.ng4LoadingSpinnerService.show();
      if (receivedResult == 1) {
        this.alertService.success("Review successful");
        this.getEmarOrdersList();
      }
      else if (receivedResult == 2) {
        this.alertService.success("Order discontinued successfully");
        this.getEmarOrdersList();
      }
      modalRef.close();
    });
  }
  openPendingForReviewModalOutside(orderId: any, quantityId: any, residentId: any) {

    this.sharedService.changePatientId(residentId);
    this.sharedService.changeOrderId(orderId);
    this.sharedService.changeQuantityId(quantityId);

    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + residentId)
      .subscribe(res => {

        this.demographicInfoData = res;
        const modalRef = this.modalService.open(PendingforreviewComponent, { size: 'lg', windowClass: 'my-class' });
        modalRef.componentInstance.modalTitle = this.demographicInfoData.PatientMiddleInitial == null ? "Pending for Review" + " " + "(" + this.demographicInfoData.PatientLastName + ", " + this.demographicInfoData.PatientFirstName + "  " + this.dateFormatPipe.transform(this.demographicInfoData.DOB) + ")" : "Pending for Review" + " " + "(" + this.demographicInfoData.PatientLastName + ", " + this.demographicInfoData.PatientFirstName + " " + this.demographicInfoData.PatientMiddleInitial + "  " + this.dateFormatPipe.transform(this.demographicInfoData.DOB) + ")";
        modalRef.componentInstance.nsId = this.nurseStationvalue;
        modalRef.componentInstance.reviewInfoResult.subscribe((receivedResult) => {
          this.ng4LoadingSpinnerService.show();
          if (receivedResult == 1) {
            this.alertService.success("Review successful");
            this.getEmarOrdersList();
          }
          else if (receivedResult == 2) {
            this.alertService.success("Order discontinued successfully");
            this.getEmarOrdersList();
          }
          modalRef.close();
        });
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

    this.closeModel();

  }

  calculateDiff(dateSent) {
    let currentDate = new Date();
    dateSent = new Date(dateSent);

    return Math.floor((Date.UTC(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()) - Date.UTC(dateSent.getFullYear(), dateSent.getMonth(), dateSent.getDate())) / (1000 * 60 * 60 * 24));
  }

  onDateChange() {
    console.log(this.myform.value.dateCheck);
    var date1 = this.dateFormatPipe.transformISODate(new Date(this.myform.value.dateCheck));
    this.gridList = [];
    this.timeDropList = [];
    this.shiftsList = [];
    this.pieChartFlag = false;
    this.timesDisplay = false;
    this.shiftDisplay = false;
    this.windowCheck = true;
    this.myform.patchValue({
      nurseSheduleTime: 0,
      nsShiftTime: 0,
      twoHrsWindow: 0
    });
    if (this.selectedfaItems.length == 0 || this.selectednItem.length == 0) {
      this.alertService.warn("Please select facility and nursing station.");
    }
    else if (this.myform.value.dateCheck == "") {
      this.alertService.warn("Please select date");
    }
    else if (-(this.calculateDiff(date1)) > 31) {
      this.alertService.warn("Selected date should not exceed 31 days from today's date");
    }

    else {
      //this.getAllFlagsForCompanyByNSId(this.nurseStationvalue);
      this.getNursingScheduleData();
    }
  }
  identifyPatientFoodBiometric() {

    this.ng4LoadingSpinnerService.show();
    if (this.processKey != "") {
      let obj =
      {
        UserID: this.userId,
        FingerId: this.fingerId,
        ProcessKey: this.processKey,
        App: 2
      }
      this.dataservice.postBiometric(this.config.Biometric_Common_IdentifyResidentGetMRNumber, obj)
        .subscribe(res => {
          console.log(res);
          if (res.m_Item2 == "Match Found") {
            if (res.m_Item1 != null) {
              // let record = this.gridList.find(g => g.PatientMRNumber == res.m_Item1);
              let record = this.gridList.find(g => g.ExternalPatientId == res.m_Item1);
              console.log(this.gridList);
              console.log(record);
              if (record != undefined) {
                this.onemanyCHeck = false;
                this.passAdminister = false;
                this.getDemographicInfoData(record.Patient_Id);
                this.selectstyle = 2;
                this.barcodevalue = '';
                this.orderItem = -1;
                this.statusFlag = -1;
                this.reSet();
              }
              else {
                this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentActiveStatus + res.m_Item1)
                  .subscribe(res => {
                    if (res == 'Active') {
                      this.alertService.warn("No Active Adminstration");
                    }
                    else if (res == 'Discharged') {
                      this.alertService.warn("Discharged Resident");
                    }
                    else {
                      this.alertService.warn("Resident data not available");
                    }
                  },
                    error => {
                      this.alertService.error(error.message);
                      this.ng4LoadingSpinnerService.hide();
                    });
              }
            }
            else {
              this.alertService.warn("ExternalPatientId not Updated");
            }
            this.alertService.success(res.m_Item2);
            this.ng4LoadingSpinnerService.hide();
          }
          else if (res.m_Item1 == 3) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(res.m_Item2);
          };
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else {
      this.alertService.error("Biometric computer not configured");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  GetUserProcessKeyByID() {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetUserProcessKeyByID + this.userId)
      .subscribe(res => {
        if (res != null) {
          this.processKey = res;
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  openAdminsterFromEkitModal(i: any, item: any) {
    debugger;
    console.log(item, "item,ite");
    this.selectEkitDrug = item.Drug;
    this.selectEkitGpi = item.GPI;
    this.selectedQtyAdmin = item.Quantity;
    this.ekitDrugDropdown(item.GPI, item.Drug);
    console.log(this.selectEkitDrug, "this.selectEkitDrug,ite");
    console.log(this.selectEkitDrug, "this.selectEkitDrug,ite");


    if (this.passAdminister == true) {
      this.alertService.warn("Must bypass or verify biometric data to administer")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (item.dueflag != 0) {
      this.barcodevalue = '';
      this.barcodeAlertsPop = true;
      this.barcodeAlert = "Order is not due at this time";
      // this.alertService.warn("Order is not due at this time")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (item.ReviewFlag != 1) {
      this.alertService.warn("Order is pending review and cannot be administered until review is completed.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      // this.getVitalsCheckListData(item.pquantity_Id, item.ReviewFlag, item.dueFlag,1);
      debugger
      console.log(item, 'item')
      this.administerOrder = item;
      console.log(this.administerOrder, ' this.administerOrder')

      this.orderItem = i;
      if (item.PRNFlag == true && item.Last_Passed != null) {
        this.getPRNAdministerCount(item);
      }
      else {
        this.checkOrders(item);
      }

      // this.isEkitReadonly=false;
      // this.ekit=[];
      // let selectedEkit=this.ekitDrop.find(e=>e.DrugName.toLowerCase()==item.Drug.toLowerCase());
      // if(selectedEkit!=undefined)
      // {
      //   this.ekit.push(selectedEkit);
      //   this.isEkitReadonly=true;
      // }
    }
  }
  administerFromEkit() {
    debugger
    this.undoFlag = false;
    this.newProductIsOpen = false;
    this.administerFromEkitModal = false;
    this.orderGivenStatus(this.administerOrder.POrder_Id, this.administerOrder.pquantity_Id, this.administerOrder.ReviewFlag, this.administerOrder.PRNFlag, this.administerOrder.dueflag, this.administerOrder.InputTime, this.administerOrder.ShiftId, this.administerOrder.Window);
    console.log('given order 3')
  }
  closeAdministerFromEkitModel() {
    this.administerFromEkitModal = false;
    this.ekit = [];
    this.quantity = '';
    this.administerOrder = null;
    this.undoFlag = false;
    this.favouritesForm.reset();
    this.CheckVitalsObj = [];
    this.ekitDrugForm.patchValue({
      barcode: '',
      drugSelect: ''
    });
    this.ekitDrug = [];
    this.ekitDrugForm.reset();
    this.lotsGrid = [];
    this.ekitSelctedarray = [];
    this.dataEkitPost = [];


  }
  openNoShowAdministerModal(i: number, item: any) {
    debugger;
    if (this.passAdminister == true) {
      this.alertService.warn("Must bypass or verify biometric data to document non-administration")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.medicationForm.reset();
      const undoQuantityValidations = this.medicationForm.get('undoQuantity');
      undoQuantityValidations.clearValidators();
      undoQuantityValidations.updateValueAndValidity();
      this.medicationForm.patchValue({
        undoQuantity: '0'
      });
      this.undoFlag = true;
      this.noShowFlag = true;
      this.undoOrderDetails = item;
      this.administerOrder = null;
      this.modalNoShowAdministerIsOpen = true;
      if (this.orderItem != i)
        this.expandOrder(i, item, true);
    }
  }
  closeNoShowModel() {
    this.undoOrderDetails = null;
    this.undoFlag = false;
    this.noShowFlag = false;
    this.modalNoShowAdministerIsOpen = false;
  }
  getPRNAdministerCount(record: any) {
    debugger
    this.dataservice.get<any>(this.config.Emar_GetPRNAdministerCountByDate + record.POrder_Id + "/" + record.pquantity_Id + "/" + this.myform.value.dateCheck)
      .subscribe(res => {
        this.maxPerDayCount = res;
        this.checkOrders(record);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  selectDrugstoAdminister(PorderId: number, Pquantity_Id: number) {

    this.dueOrderSelected = PorderId;
    this.duePquantitySelected = Pquantity_Id;
    this.selectedOrderIdForAdminister = PorderId;
  }
  closeAdministerBarcodeMatchModel() {
    this.administerBarcodeMatchModal = false;
    this.dueOrderSelected = 0;
    this.duePquantitySelected = 0;
    this.selectedOrderIdForAdminister = 0;
    this.dueOrdersCheck = [];
  }
  administerSelectedDrug() {

    let dueNow = this.dueOrdersCheck.find(it => it.POrder_Id == this.dueOrderSelected && it.pquantity_Id == this.duePquantitySelected);
    this.checkOrders(dueNow);
    this.closeAdministerBarcodeMatchModel()
  }
  getVitalsListForExpandedOrder(record: any) {

    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    debugger;
    console.log(this.selectEkitGpi, "selectEkitGpi")

    let orderItem = this.ordersList.find((ele) => ele.pquantity_Id == record.pquantity_Id);

    this.dataservice.get<any[]>(this.config.Emar_GetVitalsCheckList + record.pquantity_Id)
      .subscribe(res => {
        this.vitalsCheckList = res;
        console.log(this.vitalsCheckList, 'vitalscheck list')
        // if ((this.administerOrder == null) || (orderItem.Route == "SC" || orderItem.Route == "IM" || orderItem.Route == "TD") && this.insulinSites.length == 0) {
        //   debugger
        //   console.log('given order status 1')
        //   this.orderGivenStatus(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window);
        // }
        if (this.administerOrder == null) {
          this.orderGivenStatus(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window);
        }
        else if (this.administerOrder != null && res.length > 0) {
          this.modalVitalsIsOpen = true;
          this.favouritesForm.reset()

          setTimeout(() => {
            this.textFocus.nativeElement.focus();
          }, 300);
        }

        else if (this.administerOrder != null && res.length == 0) {
          console.log(this.barcodeExpAlert, "barcodeExpAlert")
          this.ekitDrugForm.patchValue({
            barcode: '',
            drugSelect: ''
          });
          this.administerFromEkitModal = true;

          this.ekitDrugDropdown(this.selectEkitGpi, this.selectEkitDrug)
          setTimeout(() => {
            this.ekitItem.nativeElement.focus();
          }, 300);
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  openAdditionalComments() {
    this.additionalComForm.reset();
    this.getNurseComments();
    this.modalAdditionalCommentsFormIsOpen = true;
    setTimeout(() => {
      this.AdditionalCommentsFocus.nativeElement.focus();
    }, 200);


  }
  closeCommentsModal() {
    this.modalAdditionalCommentsFormIsOpen = false;
  }
  nurseCommentsSave() {
    let notes = this.additionalComForm.value.notes;
    if (notes != "") {
      this.nurseAdditionalCommentsObj =
      {
        Comments_Id: 0,
        NurseCommentType_Id: 6,
        Comment: this.additionalComForm.value.addCom,
        comment_Status: 1,
        comment_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        Comment_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        Patient_Id: this.residentId,
      }
      this.dataservice.post(this.config.Emar_InsertUpdateNurseComments, this.nurseAdditionalCommentsObj)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          this.additionalComForm.reset();
          this.alertService.success("Comment added successfully");
          this.getNurseComments();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else {
      this.alertService.error("Please enter nurse comment");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  getNurseComments() {
    this.dataservice.get<any[]>(this.config.Emar_GetNurseComments + this.residentId)
      .subscribe(res => {
        this.nurseComments = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
  closeAdministerWithoutBarcodeModal() {
    this.modalAdministerWithoutBarcodeOpen = false;
    this.barcodeScanCheckbox = false;
    this.CheckAll = false;
    this.withoutScannerVitals();
    this.WsPRN.reset();
    this.insulinSiteIdsWS = [];
  }
  onCheckAll(event) {
    this.ordersListSelected = [];
    if (event == true) {
      if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckAllBoxId = "#wsCheckAll";
        $(ordercheckAllBoxId).prop("checked", false);
        this.CheckAll = false;
        this.ordersListSelected = [];
        this.ng4LoadingSpinnerService.hide();
      }
      else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckAllBoxId = "#wsCheckAll";
        $(ordercheckAllBoxId).prop("checked", false);
        this.CheckAll = false;
        this.ordersListSelected = [];
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        this.CheckAll = true;
        this.ordersListWithoutScanner.forEach(element => {
          element.MedicationReason_Desc = "";
          this.ordersListSelected.push(element);
        });
      }
    }
    else {
      this.CheckAll = false;
      this.ordersListSelected = [];
    }
  }
  onselectRecord(event, item: any) {

    if (event == true) {
      if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckBoxId = "#" + item.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        this.ng4LoadingSpinnerService.hide();
      }
      else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckBoxId = "#" + item.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        item.MedicationReason_Desc = '';
        this.ordersListSelected.push(item);
      }
    }
    else {
      const index = this.ordersListSelected.findIndex(i => i.POrder_Id == item.porder_Id);
      this.ordersListSelected.splice(index, 1);
    }
  }
  AdminsterWithoutScanner() {

    this.undoFlag = false;
    let prnCommentsCheck = this.ordersListSelected.filter(pc => pc.PRNFlag == true);
    let insulinSitesRequireOrderWS = this.ordersListSelected.filter(o => o.Route == "SC" || o.Route == "IM" || o.Route == "TD")
    let checkFlag = 0;
    for (let k = 0; k < prnCommentsCheck.length; k++) {
      // if(prnCommentsCheck[k].MedicationReason_Desc=='')
      // checkFlag=checkFlag+1;
      let prnText = "#PRN" + prnCommentsCheck[k].pquantity_Id;
      let prnReason = $(prnText).val();
      if (prnReason != null && prnReason != "")
        checkFlag = 0;
      else {
        checkFlag = 1;
        break;
      }
    }

    if (checkFlag == 0 && insulinSitesRequireOrderWS.length == this.insulinSiteIdsWS.length) {
      this.modalAdministerWithoutBarcodeOpen = false;
      this.administerPRNOrderReasonWS();
    }
    else if (checkFlag != 0) {
      this.alertService.warn("Reason for administration required on PRN orders");
    }
    else if (insulinSitesRequireOrderWS.length != this.insulinSiteIdsWS.length) {
      this.alertService.warn("Administration site required");
    }
  }
  administerPRNOrderReasonWS() {
    this.modalPRNCommentIsOpenWithoutScanner = false;
    for (let j = 0; j < this.ordersListSelected.length; j++) {
      (async () => {
        let record = this.ordersListSelected[j];
        let prnText = "#PRN" + record.pquantity_Id;
        let prnReasonText = $(prnText).val() != undefined ? $(prnText).val() : "";
        if (record != null) {
          this.orderGivenStatusWS(record.POrder_Id, record.pquantity_Id, record.ReviewFlag, record.PRNFlag, record.dueflag, record.InputTime, record.ShiftId, record.Window, prnReasonText.toString());
          await this.delay(500);
        }
      })();
    }
  }
  delay(ms: number) {

    return new Promise(resolve => setTimeout(resolve, ms));
  }
  prnReasonUpdate(orderId: number, quantityId: number, text: string) {

    let orderdata = this.ordersListSelected.findIndex(so => so.POrder_Id == orderId && so.pquantity_Id == quantityId && so.PRNFlag == true);
    // if(orderdata!=-1)
    // {
    //   this.ordersListSelected[orderdata].MedicationReason_Desc=text;
    // }
  }
  orderGivenStatusWS(orderId: number, quantityId: number, reviewFlag: number, PRNFlag: boolean, dueFlag: number, inputTime: string, shiftId: number, window: number, PrnReason: string) {
    //POrder_Id, item.pquantity_Id
    debugger;
    this.ng4LoadingSpinnerService.show();
    this.orderDueFlag = dueFlag == 0 ? false : true;
    if (dueFlag != 0) {
      this.barcodevalue = '';
      this.barcodeAlertsPop = true;
      this.barcodeAlert = "Order is not due at this time";
      // this.alertService.warn("Order is not due at this time")
      this.ng4LoadingSpinnerService.hide();
    }
    else if (reviewFlag != 1) {
      this.alertService.warn("Order is pending review and cannot be administered until review is completed.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      //Right Administer Time Logiv. Not Needed
      // passTimes = passTimes.trim().replace(/AM/g, ' AM');
      // passTimes = passTimes.replace(/PM/g, ' PM');
      // let passTimesArray = passTimes.split(', ');
      // let rightAdministerPassTime: any = '';
      // if (passTimesArray.length > 0) {
      //   passTimesArray.every(element => {
      //     console.log(element);
      //     //let res = this.adminsterRightTime(element);
      //     //console.log(res);
      //     if (this.adminsterRightTime(element) == true) {
      //       rightAdministerPassTime = element; return false;
      //     }
      //     else
      //       return true;
      //   });
      // }
      // if (rightAdministerPassTime != '') {
      //   console.log(rightAdministerPassTime);
      let barcodesList = this.ordersList.find(o => o.POrder_Id == orderId && o.pquantity_Id == quantityId).Barcode;
      let rightAdministerPassTime;
      if (this.passTime != null)
        rightAdministerPassTime = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.passTime.replace('-', ':');
      else
        rightAdministerPassTime = this.dateFormatPipe.transform(this.myform.value.dateCheck);
      let administerStatus = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId);
      this.controlSubFlag = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId).ControlSubstanceFlag;
      this.controlSubCertBy = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId).ControlSubstanceCertifiedBy;
      let checkUser = this.controlSubCertBy != null ? this.controlSubCertBy.split(',').includes(this.userId) ? true : false : false;
      let barcode = this.ordersList.find(li => li.POrder_Id == orderId && li.pquantity_Id == quantityId).Barcode;
      if (barcode == undefined || barcode == null || barcode == '') {
        this.alertService.warn("Order has no barcode.")
        this.ng4LoadingSpinnerService.hide();
      }
      else if (((administerStatus.AdministerStatus == 'Not Administered' || administerStatus.AdministerStatus == '')) && ((this.controlSubFlag == 1 && checkUser == false) || this.controlSubFlag == 2) && this.administerOrder == null) {
        this.alertService.warn("Controlled substance count certification required prior to administration")
        this.ng4LoadingSpinnerService.hide();
      }
      else {

        if (this.undoFlag == false) {
          if (this.barcodeScanCheckbox == true && this.withoutAdminsterForm.value.reasonForWithoutAdministred == '')
            this.alertService.warn("Please enter the reason for not using the scanner");
          else {

            if (this.vitalsCheckList.length != this.CheckVitalsObj.length && this.barcodeScanCheckbox == false) {
              this.admScheduleForVitalsSave = rightAdministerPassTime;//this.dateFormatPipe.transform(this.myform.value.dateCheck);
              //this.alertService.warn("Check Vitals Before Administer.");
              this.orderId = orderId;
              this.quantityId = quantityId;
              this.inputTime = inputTime;
              console.log(this.inputTime, 'inputTime2')

              this.shiftId = shiftId;
              this.window = window;
              this.modalVitalsIsOpen = true;
              this.favouritesForm.reset()

              setTimeout(() => {
                this.textFocus.nativeElement.focus();
              }, 300);
            }
            else {
              // if (this.medicationForm.value.medicationReason[0].MedicationReason_ID == 7 && this.medicationForm.value.note == '')
              //   this.alertService.warn("Please enter note");
              //else {
              // if (((this.ekit != null && this.ekit != undefined && this.ekit.length != 0) && (this.quantity == '' || this.quantity == null)) || ((this.ekit == null || this.ekit == undefined || this.ekit.length == 0) && this.quantity != ''))
              //   this.alertService.warn("Please enter ekit details");
              // if (this.ekit != null && this.ekit != undefined && this.ekit.length != 0 && parseFloat(this.ekit[0].Ekit_Id) == 0) {
              //   this.alertService.warn("Please enter valid numeric quantity");
              //   this.quantity = "";
              // }
              // else if (this.ekit != null && this.ekit != undefined && this.ekit.length!=0 && this.ekit.length != 0 && this.quantity != '' && this.checkEkitQuantity() == true) {
              //   //this.alertService.warn("Entered more quantity.You have only " + this.quantityInhand + " On Hand Quantity");
              //   this.alertService.warn("Quantity entered exceeds on-hand quantity");
              //   this.quantity = "";
              // }
              //  else {
              //if (this.barcodeScanCheckbox == true && this.withoutAdminsterForm.value.reasonForWithoutAdministred != '') {
              //this.InsertAdministredwithoutscanning(orderId, quantityId);
              //}
              this.drugAdminsterObj = {
                //DrugAdminister_Id: drugAdministor_Id,
                POrder_Id: orderId,
                pquantity_Id: quantityId,
                AdminsterSchedule: rightAdministerPassTime,//this.dateFormatPipe.transform(this.myform.value.dateCheck),
                AdministerComment: PrnReason,//this.medicationForm.value.note,
                AdminsterStatus: 1,
                AdminsterBy: this.userId,
                AdminsterOn: this.dateFormatPipe.dateWithTime(new Date()),
                MedicationReason_ID: 1,
                BCScanner: this.barcodeScanCheckbox == true ? 1 : 0,
                BCScannerText: this.withoutAdminsterForm.value.reasonForWithoutAdministred,
                Ekit_Id: this.ekit == null || this.ekit == undefined || this.ekit.length == 0 ? 0 : this.ekit[0].Ekit_Id,
                //Ekit_Id: this.ekitSelctedarray == null || this.ekitSelctedarray == undefined || this.ekitSelctedarray.length == 0 ? 0 : this.ekitSelctedarray.length >1 ?this.lotsGrid[0].Ekit_Id : this.ekitSelctedarray[0],
                Quantity: this.quantity,
                Patient_Id: this.residentId,
                DrugQuantity: this.undoFlag == false ? 0 : this.medicationForm.value.undoQuantity,
                ByPassReason: this.byPassReason,
                InputTime: inputTime,
                ShiftId: shiftId,
                Window: window,
                PRNFlag: PRNFlag,
                UndoFlag: this.undoFlag,
                Last_Passed: null,
                AdditionalComments: this.additionalComForm.value.addCom,
                AdministeredBarcode: this.barcodeScanCheckbox == false ? this.barcodevalue : barcodesList,
                DiscardDate: this.getDiscardDateForAdminister(quantityId) != undefined && this.getDiscardDateForAdminister(quantityId) != null && this.getDiscardDateForAdminister(quantityId) != "" ? this.dateFormatPipe.dateFormat(this.getDiscardDateForAdminister(quantityId)) : null,
                AdministerInsulinSites: this.insulinSiteIdsWS.length > 0 && this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId) != undefined ? this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId).SiteIds : null,
                RouteCode: this.insulinSiteIdsWS.length > 0 && this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId) != undefined ? this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId).RouteCode : null,
                //Ekit_AllIds:this.ekitSelctedarray.toString()
                Ekit_AllIds: this.dataEkitPost

              };
              // }
              //}
            }
          }
        }
        else {
          // if (this.medicationForm.value.medicationReason[0].MedicationReason_ID == 0)
          //   this.alertService.warn("Please select reason");
          if ((this.medicationForm.value.medicationReason != null && this.medicationForm.value.medicationReason.length > 0) && this.medicationForm.value.medicationReason[0].MedicationReason_ID == 7 && this.medicationForm.value.note == '')
            this.alertService.warn("Please enter note");
          // else if (((this.ekit != null && this.ekit != undefined && this.ekit.length != 0) && (this.quantity == '' || this.quantity == null)) || ((this.ekit == null || this.ekit == undefined || this.ekit.length == 0) && this.quantity != ''))
          //   this.alertService.warn("Please enter ekit details");
          // else if (this.ekit != null && this.ekit.length != 0 && this.ekit != undefined && parseFloat(this.ekit[0].Ekit_Id) == 0) {
          //   this.alertService.warn("Please enter valid numeric quantity");
          //   this.quantity = "";
          // }
          // else if (this.ekit != null && this.ekit != undefined && this.ekit.length != 0 && this.quantity != '' && this.checkEkitQuantity() == true) {
          //   this.alertService.warn("Entered more quantity. You have only " + this.quantityInhand + " On Hand Quantity");
          //   this.quantity = "";
          // }
          else {
            this.drugAdminsterObj = {
              //DrugAdminister_Id: drugAdministor_Id,
              POrder_Id: orderId,
              pquantity_Id: quantityId,
              AdminsterSchedule: rightAdministerPassTime,//this.dateFormatPipe.transform(this.myform.value.dateCheck),
              AdministerComment: this.medicationForm.value.note,
              AdminsterStatus: 0,
              AdminsterBy: this.userId,
              AdminsterOn: this.dateFormatPipe.dateWithTime(new Date()),
              MedicationReason_ID: (this.medicationForm.value.medicationReason != null && this.medicationForm.value.medicationReason.length > 0) ? this.medicationForm.value.medicationReason[0].MedicationReason_ID : 0,
              BCScanner: this.barcodeScanCheckbox == true ? 1 : 0,
              BCScannerText: this.withoutAdminsterForm.value.reasonForWithoutAdministred,
              Ekit_Id: this.ekit == null || this.ekit == undefined || this.ekit.length == 0 ? 0 : this.ekit[0].Ekit_Id,
              //Ekit_Id: this.ekitSelctedarray == null || this.ekitSelctedarray == undefined || this.ekitSelctedarray.length == 0 ? 0 : this.ekitSelctedarray.length >1 ?this.lotsGrid[0].Ekit_Id : this.ekitSelctedarray[0],

              Quantity: this.quantity,
              Patient_Id: this.residentId,
              DrugQuantity: this.undoFlag == true && this.medicationForm.value.undoQuantity != '' ? this.medicationForm.value.undoQuantity : 0,
              ByPassReason: this.byPassReason,
              InputTime: inputTime,
              ShiftId: shiftId,
              Window: window,
              PRNFlag: PRNFlag,
              UndoFlag: this.undoFlag,
              Last_Passed: this.ordersList.find(o => o.POrder_Id == orderId && o.pquantity_Id == quantityId).Last_Passed != null ? this.ordersList.find(o => o.POrder_Id == orderId && o.pquantity_Id == quantityId).Last_Passed.split('/').join('-') : null,
              AdditionalComments: this.additionalComForm.value.addCom,
              AdministeredBarcode: this.barcodeScanCheckbox == false ? this.barcodevalue : barcodesList,
              DiscardDate: this.undoFlag == true ? null : (this.getDiscardDateForAdminister(quantityId) != undefined && this.getDiscardDateForAdminister(quantityId) != null && this.getDiscardDateForAdminister(quantityId) != "" ? this.dateFormatPipe.dateFormat(this.getDiscardDateForAdminister(quantityId)) : null),
              AdministerInsulinSites: this.undoFlag == true ? null : this.insulinSiteIdsWS.length > 0 && this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId) != undefined ? this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId).SiteIds : null,
              RouteCode: this.undoFlag == true ? null : this.insulinSiteIdsWS.length > 0 && this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId) != undefined ? this.insulinSiteIdsWS.find(o => o.PQuantity_Id == quantityId).RouteCode : null,
              //Ekit_AllIds:this.ekitSelctedarray.toString()
              Ekit_AllIds: this.dataEkitPost

            };
          }
        }
        if (this.drugAdminsterObj != null) {
          this.dataservice.post(this.config.Emar_Emar_InsertDrugAdminister, this.drugAdminsterObj)
            .subscribe(res => {
              this.ng4LoadingSpinnerService.show();
              this.reSet();
              this.getEmarOrdersList();
              //this.getEkitDrop(this.nurseStationvalue);
              //this.getEmarResidentGridData();
              this.persistanceService.getDueMARAlert();

              if (res == 1 && ((this.undoFlag == false) || (this.undoFlag == true && this.noShowFlag == true))) {
                this.alertService.success('Order administration successful');
              }
              else if (res == 1 && this.undoFlag == true && this.noShowFlag == false)
                this.alertService.success('Undo order administration successful');
              else if (res == 0)
                this.alertService.error('Order administration failed');
              this.orderItem = -1;
              this.statusFlag = -1;
              this.undoFlag = !this.undoFlag;
              this.CheckVitalsObj = [];
              this.barcodeScanCheckbox = false;
              this.ekit = [];
              this.quantity = '';
              this.administerFromEkitModal = false;
              this.modalNoShowAdministerIsOpen = false;
              this.administerOrder = null;
              this.addComments = 0;
              this.tempOrderId = 0;
              this.tempQtyId = 0;
              this.withoutAdminsterForm.patchValue({
                reasonForWithoutAdministred: '',
              });
              this.getDosesDetails(0, null);
              // this.ng4LoadingSpinnerService.hide();
            }, error => {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.error(error.message);
            });
        }
        this.ng4LoadingSpinnerService.hide();
      }
      this.drugAdminsterObj = null;
      this.ng4LoadingSpinnerService.hide();
      // }
      // else {
      //   this.alertService.warn("This Order is Not Due now");
      //   this.ng4LoadingSpinnerService.hide();
      // }
    }
  }
  //Administer Without Scanner Vitals Check Related Logic begin
  closeWithoutScannerVitalsModel() {
    this.modalAdministerWithoutScannerVitalsIsOpen = false;
  }
  userInputsOpen() {
    if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
      this.modalAdministerWithoutScannerVitalsIsOpen = false;
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
      this.modalAdministerWithoutScannerVitalsIsOpen = false;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      if (this.CheckVitalsObj.length == 0) {
        this.modalAdministerWithoutScannerVitalsIsOpen = true;
        this.favouritesForm.reset();
      }
      else {
        this.modalAdministerWithoutScannerVitalsIsOpen = true;
        this.favouritesForm.reset();

      }
    }
  }
  withoutScannerVitals() {
    debugger;
    //this.cllk=true;
    this.withoutScannerDisplayVitals = [];
    this.withoutScannerTempVitals = [];
    if (this.ordersListWithoutScanner.length > 0) {
      this.ordersListWithoutScanner.forEach((element, index) => {
        let checkBoxId = "#" + element.pquantity_Id;
        $(checkBoxId).prop("checked", false);
        let ordercheckBoxId = "#" + element.pquantity_Id;
        $(ordercheckBoxId).css("display", "block");

        console.log(element.pquantity_Id);
        this.withoutScannerOrderVitals = [];
        this.WsPRN.reset();
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.dataservice.get<any[]>(this.config.Emar_GetVitalsCheckList + element.pquantity_Id)
          .subscribe(res => {
            if (res.length > 0) {
              let vitalIds = [];
              let vitalDescs = [];
              res.forEach(item => vitalIds.push(item.OrderFavMaster_ID));
              res.forEach(item => vitalDescs.push(item.OrderFavDesc));
              let obj =
              {
                orderId: element.POrder_Id,
                qtyId: element.pquantity_Id,
                Vitals: vitalIds.join(","),
                InputTime: element.InputTime,
                ShiftId: element.ShiftId,
                Window: element.Window,
                Drug: element.Drug + " (" + vitalDescs.join(", ") + ")",
              }

              this.withoutScannerTempVitals.push(...res);
              //this error may be removed by adding this alternative code for inserting res in tempvitals
              // res.forEach(item => {
              //   this.withoutScannerTempVitals.push(item);
              // });

              this.withoutScannerOrderVitals.push(obj);
              let checkBoxId = "#" + element.pquantity_Id;
              $(checkBoxId).prop("disabled", true);
              let prnText = "#PRN" + element.pquantity_Id;

              $(prnText).attr("disabled", "disabled");


            }
            else {
              let cllk = "#edit" + element.pquantity_Id;
              $(cllk).css("display", "none");
            }
            this.withoutScannerDiscardDate(element, index);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          },
            error => {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.error(error.message);
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();

            });
      });
    }
  }
  afterDiscardDateGetWS() {
    this.wsDiscard = 1;
    if (this.ordersListWithoutScanner.length > 0) {
      this.ordersListWithoutScanner.forEach((element, index) => {
        if (element.DiscardDays != null && element.DiscardDays != 0)
          this.withoutScannerDiscardDate(element, index);
      });
    }
  }
  withoutScannerDiscardDate(orderItem: any, index: any) {

    let ordercheckBoxId = "#" + orderItem.pquantity_Id;
    $(ordercheckBoxId).prop("checked", false);
    // $(ordercheckBoxId).prop("display",false);
    if (orderItem.DiscardDays != null && orderItem.DiscardDays != 0 && (orderItem.DiscardDate == null || orderItem.DiscardDate == "")) {
      let manSetUnCheckBoxId = "#manset" + orderItem.pquantity_Id;
      $(manSetUnCheckBoxId).prop("checked", false);
      let newPrtUnCheckBoxId = "#newpr" + orderItem.pquantity_Id;
      $(newPrtUnCheckBoxId).prop("checked", false);
      let manSetCheckBoxId = "#manset" + orderItem.pquantity_Id;
      $(manSetCheckBoxId).prop("disabled", true);
      let discardDate = "#disca" + orderItem.pquantity_Id;
      $(discardDate).attr("disabled", "disabled");
      let ordercheckBoxId = "#" + orderItem.pquantity_Id;
      $(ordercheckBoxId).css("display", "none");
      let rowBgColor = "#WSOrderRow" + index + orderItem.pquantity_Id;
      $(rowBgColor).css("background-color", "#ffdf62");
      this.wsDiscard = 0;
      let discardDateId = "#disca" + orderItem.pquantity_Id;
      $(discardDateId).val(this.dateFormatPipe.transformISODate(orderItem.DiscardDate));
    }
    else if (orderItem.DiscardDays != null && orderItem.DiscardDays != 0 && orderItem.DiscardDate != null && orderItem.DiscardDate != "") {
      let manSetUnCheckBoxId = "#manset" + orderItem.pquantity_Id;
      $(manSetUnCheckBoxId).prop("checked", false);
      let newPrtUnCheckBoxId = "#newpr" + orderItem.pquantity_Id;
      $(newPrtUnCheckBoxId).prop("checked", false);
      let manSetCheckBoxId = "#manset" + orderItem.pquantity_Id;
      $(manSetCheckBoxId).prop("disabled", false);
      let discardDate = "#disca" + orderItem.pquantity_Id;
      $(discardDate).attr("disabled", "disabled");
      let discardDateId = "#disca" + orderItem.pquantity_Id;
      $(discardDateId).val(this.dateFormatPipe.transformISODate(orderItem.DiscardDate));
      if (((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) >= 0 && ((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) <= 3) {
        let rowBgColor = "#WSOrderRow" + index + orderItem.pquantity_Id;
        $(rowBgColor).css("background-color", "#ade39d");
        let ordercheckBoxId = "#" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        $(ordercheckBoxId).css("display", "block");
      }
      else if (((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) < 0) {
        let rowBgColor = "#WSOrderRow" + index + orderItem.pquantity_Id;
        $(rowBgColor).css("background-color", "#ffdf62");
        let ordercheckBoxId = "#" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        $(ordercheckBoxId).css("display", "none");
        this.wsDiscard = 0;
      }
      else if (((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) > 3) {
        let rowBgColor = "#WSOrderRow" + index + orderItem.pquantity_Id;
        $(rowBgColor).css("background-color", "white");
        let ordercheckBoxId = "#" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        $(ordercheckBoxId).css("display", "block");
      }
    }
  }
  afterGetVitalsList() {
    debugger;

    if (this.withoutScannerTempVitals.length > 0) {

      this.withoutScannerDisplayVitals = [];
      this.withoutScannerTempVitals.filter(el => {
        if (this.withoutScannerDisplayVitals.length == 0 || (this.withoutScannerDisplayVitals.length > 0 && this.withoutScannerDisplayVitals.find(wi => wi.OrderFavMaster_ID == el.OrderFavMaster_ID) == null)) {
          // If not present in array, then add it
          this.withoutScannerDisplayVitals.push(el);
        } else {
          // Already present in array, don't add it
        }
      });
      let vitalsChecks = this.withoutScannerDisplayVitals.sort((a, b) => {
        if (a.PQuantity_Id > b.PQuantity_Id) {
          return 1;
        } else if (a.PQuantity_Id < b.PQuantity_Id) {
          return -1;
        } else {
          return 0;
        }

      });
      this.withoutScannerDisplayVitals = vitalsChecks;
    }
    this.modalAdministerWithoutBarcodeOpen = true;
    setTimeout(() => {
      this.reasonForWithOutScannerFocus.nativeElement.focus();
    }, 300);
  }
  withoutScannervitalsCheckSave() {

    debugger;
    this.ng4LoadingSpinnerService.show();
    if (this.CheckVitalsObj.length != 0) {
      let vitalsCompletedOrder = [];
      this.withoutScannerOrderVitals.forEach(record => {
        let tempVitals = [];
        this.CheckVitalsObj.forEach(item => tempVitals.push(item.OrderFavMaster_ID.toString()));
        let vitalsByOrder = record.Vitals.split(',');
        let clicck = "#edit" + record.qtyId;
        //console.log(record.qtyId);
        $(clicck).css("color", "green");


        //based order all inputs given or not
        let test = vitalsByOrder.map(x => tempVitals.includes(x.toString()));
        if ((test.every((val, i, arr) => val === arr[0] && val == true)) == true) {
          var vitalsRecords = this.CheckVitalsObj.filter(ch => vitalsByOrder.includes((ch.OrderFavMaster_ID).toString()));
          vitalsRecords[0].POrder_Id = record.orderId;
          vitalsRecords[0].pquantity_Id = record.qtyId;
          vitalsRecords[0].InputTime = record.InputTime;
          vitalsRecords[0].ShiftId = 0;
          vitalsRecords[0].Window = 0;
          //rightAdministerPassTime
          vitalsRecords[0].AdminsterSchedule = this.dateFormatPipe.transform(this.myform.value.dateCheck) + " " + this.passTime.replace('-', ':');
          console.log(vitalsRecords, "vitalsRecords")

          this.dataservice.post(this.config.Emar_InsertOrderFavouritesData, vitalsRecords)
            .subscribe(res => {
              debugger;
              //this.alertService.success("Saved Successfully");
              //this.vitalCheckStatus = "Vitals Check Completed";
              let checkBoxId = "#" + record.qtyId;
              $(checkBoxId).prop("disabled", false);
              let clk = "#" + record.pquantity_Id;
              $(clk).prop("disabled", true);
              $(clk).prop("hidden", true);
              let prnText = "#PRN" + record.qtyId;
              $(prnText).removeAttr("disabled");
              vitalsCompletedOrder.push(record.qtyId);
              if (vitalsCompletedOrder.length == this.withoutScannerOrderVitals.length) {
                this.withoutScannerDisplayVitals = [];
              }
            },
              error => {
                this.alertService.error(error.message);
                this.ng4LoadingSpinnerService.hide();
              });
        }
      });
      this.ng4LoadingSpinnerService.hide();
      this.modalAdministerWithoutScannerVitalsIsOpen = false;
    }
    else {
      this.alertService.warn("Please fill required User Inputs.")
      this.ng4LoadingSpinnerService.hide();

    }
    window.scroll(0, 0);
    this.favouritesForm.reset();
  }
  closeBiometricModel() {
    this.modalBiometricComputerName = false;
    this.persistanceService.set('emarProcessKey', "");
    this.processKey = this.persistanceService.get('emarProcessKey');
  }
  openComputerNameModel() {
    this.selectedComItems = [];
    this.computernameform.reset();
    this.modalBiometricComputerName = true;
  }
  getComputersListByNsId(nsId: number) {
    this.dataservice.get<any[]>(this.config.Emar_Role_GetProcessKeyMasterList + nsId)
      .subscribe(res => {
        this.computers = res;
        this.persistanceService.set('emarProcessKey', "");
        this.processKey = this.persistanceService.get('emarProcessKey');
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  closeComputerModel() {

    if (this.computernameform.value.computerName != undefined && this.computernameform.value.computerName != null && this.computernameform.value.computerName.length != 0) {
      this.persistanceService.set('emarProcessKey', this.computernameform.value.computerName[0].ProcessKey);
      this.processKey = this.persistanceService.get('emarProcessKey');
    }
    else {
      this.persistanceService.set('emarProcessKey', "");
      this.processKey = this.persistanceService.get('emarProcessKey');
    }
    this.modalBiometricComputerName = false;
  }
  closeDiscardModel() {

    this.modalDiscardDateIsOpen = false;
    this.newProductIsOpen = true;
    this.manuallySetDiscard = false;
    this.barcodevalue = '';
  }
  newProductIsOpenCheck(event: any) {
    let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
    if (event == true) {
      if (orderItem != undefined && orderItem.DiscardDays != null && orderItem.DiscardDays != 0) {
        let discardDate = new Date(new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate)).setDate(new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate)).getDate() + (orderItem.DiscardDays - 1))).toISOString().substring(0, 10);
        this.discardform.patchValue({
          discardDate: discardDate,
        });
        this.discardDateChange(this.discardform.value.discardDate);
      }
    }
    else if (event == false) {
      if (orderItem != undefined && orderItem.DiscardDays != null) {
        this.discardform.patchValue({
          discardDate: this.dateFormatPipe.transformISODate(orderItem.DiscardDate),
        });
        this.discardDateChange(this.discardform.value.discardDate);
      }
      if (orderItem.DiscardDate == null) {
        this.manuallySetDiscard = false;
      }
    }
  }
  manuallySetCheck(event: any) {
    let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
    if (event == false) {
      if (orderItem != undefined && orderItem.DiscardDays != null) {
        this.newProductIsOpen = false;
        this.discardform.patchValue({
          discardDate: this.dateFormatPipe.transformISODate(orderItem.DiscardDate),
        });
        this.discardDateChange(this.discardform.value.discardDate);
      }
    }
  }
  discardDateChange(date: any) {

    if (date == undefined || date == null || date == "") {
      this.discardAlertMsg = "Check if new product is open";
      this.discardAlertColorFlag = 2;
    }
    if (((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) >= 0 && ((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) <= 3) {
      this.discardAlertMsg = "Drug  must be discarded soon, reorder promptly";
      this.discardAlertColorFlag = 1;
    }
    else if (((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) < 0) {
      this.discardAlertMsg = "This drug package should no longer be used, open a new package";
      this.discardAlertColorFlag = 2;
    }
    else if (((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) > 3) {
      this.discardAlertMsg = "";
      this.discardAlertColorFlag = 0;
    }
  }
  setDiscardAfterDate() {
    if (this.discardform.value.discardDate == undefined || this.discardform.value.discardDate == null || this.discardform.value.discardDate == "") {
      this.alertService.warn("Check if new product is open");
      this.ng4LoadingSpinnerService.hide();
    }
    else {

      this.newProductIsOpen = true;
      this.manuallySetDiscard = false;
      this.modalDiscardDateIsOpen = false;
      let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
      if (orderItem != undefined) {
        this.getVitalsListForExpandedOrder(orderItem);
      }
    }
  }
  openSideEffectsModal(GPI: any, drugName: string) {
    this.dataservice.get<any>(this.config.Emar_Emar_GetSideEffectsByGPICode + GPI)
      .subscribe(res => {
        this.sideEffects = res;
        this.sideEffectsDrugName = drugName;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    this.modalSideEffectsIsOpen = true;
  }
  closeSideEffectsModel() {
    this.modalSideEffectsIsOpen = false;
  }
  onselectNewProductOpenWS(event: any, orderItem: any, index: any) {
    if (event == true) {
      if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckBoxId = "#newpr" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        this.ng4LoadingSpinnerService.hide();
      }
      else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckBoxId = "#newpr" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        let orderItemRecords = this.ordersListWithoutScanner.filter(or => or.POrder_Id == orderItem.POrder_Id);
        orderItemRecords.forEach((orderElement, len) => {
          let ordercheckBoxId = "#newpr" + orderElement.pquantity_Id;
          $(ordercheckBoxId).prop("checked", true);
          if (orderItem != undefined && orderItem.DiscardDays != null && orderItem.DiscardDays != 0) {
            let discardDate = new Date(new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate)).setDate(new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate)).getDate() + (orderItem.DiscardDays - 1))).toISOString().substring(0, 10);
            let discardDateId = "#disca" + orderElement.pquantity_Id;
            $(discardDateId).val(discardDate);
            let manSetCheckBoxId = "#manset" + orderElement.pquantity_Id;
            $(manSetCheckBoxId).prop("disabled", false);
            let manSetUnCheckBoxId = "#manset" + orderElement.pquantity_Id;
            $(manSetUnCheckBoxId).prop("checked", false);
            $(discardDateId).attr("disabled", "disabled");
            if (((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) >= 0 && ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) <= 3) {
              let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
              let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
              $(rowBgColor).css("background-color", "#ade39d");
              let ordercheckBoxId = "#" + orderElement.pquantity_Id;
              $(ordercheckBoxId).prop("checked", false);
              $(ordercheckBoxId).css("display", "block");
              //this.wsCheckAllButtonDisplay();
            }
            else if (((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) < 0) {
              let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
              let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
              $(rowBgColor).css("background-color", "#ffdf62");
              let ordercheckBoxId = "#" + orderElement.pquantity_Id;
              $(ordercheckBoxId).css("display", "none");
              //this.wsCheckAllButtonDisplay();
              let objIndex = this.ordersListSelected.findIndex(cs => cs.pquantity_Id == orderElement.pquantity_Id);
              if (objIndex >= 0) {
                this.ordersListSelected.splice(objIndex, 1);
              }
            }
            else if (((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) > 3) {
              let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
              let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
              $(rowBgColor).css("background-color", "white");
              let ordercheckBoxId = "#" + orderElement.pquantity_Id;
              $(ordercheckBoxId).prop("checked", false);
              $(ordercheckBoxId).css("display", "block");
              //this.wsCheckAllButtonDisplay();
            }
          }
          if (orderItemRecords.length == (len + 1)) {
            this.wsCheckAllButtonDisplay();
          }
        });
      }
    }
    else if (event == false) {
      let orderItemRecords = this.ordersListWithoutScanner.filter(or => or.POrder_Id == orderItem.POrder_Id);
      orderItemRecords.forEach((orderElement, len) => {
        let ordercheckBoxId = "#newpr" + orderElement.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        if (orderItem.DiscardDays != null && orderItem.DiscardDays != 0 && (orderItem.DiscardDate == null || orderItem.DiscardDate == "")) {
          let manSetUnCheckBoxId = "#manset" + orderElement.pquantity_Id;
          $(manSetUnCheckBoxId).prop("checked", false);
          let manSetCheckBoxId = "#manset" + orderElement.pquantity_Id;
          $(manSetCheckBoxId).prop("disabled", true);
          let discardDate = "#disca" + orderElement.pquantity_Id;
          $(discardDate).attr("disabled", "disabled");
          let ordercheckBoxId = "#" + orderElement.pquantity_Id;
          $(ordercheckBoxId).css("display", "none");
          let discardDateId = "#disca" + orderElement.pquantity_Id;
          $(discardDateId).val(orderItem.DiscardDate);
          let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
          let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
          $(rowBgColor).css("background-color", "#ffdf62");
          this.wsCheckAllButtonDisplay();
          let objIndex = this.ordersListSelected.findIndex(cs => cs.pquantity_Id == orderElement.pquantity_Id);
          if (objIndex >= 0) {
            this.ordersListSelected.splice(objIndex, 1);
          }
        }
        else if (orderItem.DiscardDays != null && orderItem.DiscardDays != 0 && orderItem.DiscardDate != null && orderItem.DiscardDate != "") {
          let manSetCheckBoxId = "#manset" + orderElement.pquantity_Id;
          $(manSetCheckBoxId).prop("disabled", false);
          let discardDate = "#disca" + orderElement.pquantity_Id;
          var check = $(manSetCheckBoxId).prop("checked");
          if (check = false) {
            $(discardDate).attr("disabled", "disabled");
          }
          else if (check == true) {
            $(discardDate).removeAttr("disabled");
          }
          let discardDateId = "#disca" + orderElement.pquantity_Id;
          $(discardDateId).val(this.dateFormatPipe.transformISODate(orderItem.DiscardDate));
          if (((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) >= 0 && ((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) <= 3) {
            let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
            let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
            $(rowBgColor).css("background-color", "#ade39d");
            let ordercheckBoxId = "#" + orderElement.pquantity_Id;
            $(ordercheckBoxId).prop("checked", false);
            $(ordercheckBoxId).css("display", "block");
            this.wsCheckAllButtonDisplay();
          }
          else if (((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) < 0) {
            let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
            let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
            $(rowBgColor).css("background-color", "#ffdf62");
            let ordercheckBoxId = "#" + orderElement.pquantity_Id;
            $(ordercheckBoxId).css("display", "none");
            this.wsCheckAllButtonDisplay();
            let objIndex = this.ordersListSelected.findIndex(cs => cs.pquantity_Id == orderElement.pquantity_Id);
            if (objIndex >= 0) {
              this.ordersListSelected.splice(objIndex, 1);
            }
          }
          else if (((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) > 3) {
            let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
            let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
            $(rowBgColor).css("background-color", "white");
            let ordercheckBoxId = "#" + orderElement.pquantity_Id;
            $(ordercheckBoxId).prop("checked", false);
            $(ordercheckBoxId).css("display", "block");
            this.wsCheckAllButtonDisplay();
          }
        }
        if (orderItemRecords.length == (len + 1)) {
          this.wsCheckAllButtonDisplay();
        }
      });
    }
  }
  onselectManSetDiscardWS(event: any, orderItem: any, index: any) {
    if (event == true) {
      if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckBoxId = "#manset" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        this.ng4LoadingSpinnerService.hide();
      }
      else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
        this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
        let ordercheckBoxId = "#manset" + orderItem.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        let orderItemRecords = this.ordersListWithoutScanner.filter(or => or.POrder_Id == orderItem.POrder_Id);
        orderItemRecords.forEach((orderElement, len) => {
          let ordercheckBoxId = "#manset" + orderElement.pquantity_Id;
          $(ordercheckBoxId).prop("checked", true);
          let discardDate = "#disca" + orderElement.pquantity_Id;
          $(discardDate).removeAttr("disabled");
        });
      }
    }
    else if (event == false) {
      let orderItemRecords = this.ordersListWithoutScanner.filter(or => or.POrder_Id == orderItem.POrder_Id);
      orderItemRecords.forEach((orderElement, len) => {
        let ordercheckBoxId = "#manset" + orderElement.pquantity_Id;
        $(ordercheckBoxId).prop("checked", false);
        let discardDate = "#disca" + orderElement.pquantity_Id;
        $(discardDate).attr("disabled", "disabled");
        let newPrtCheckBoxId = "#newpr" + orderElement.pquantity_Id;
        if ($(newPrtCheckBoxId).prop("checked") == false) {
          this.onselectNewProductOpenWS(false, orderItem, index)
        }
        else if ($(newPrtCheckBoxId).prop("checked") == true) {
          this.onselectNewProductOpenWS(true, orderItem, index)
        }
      });
    }
  }
  changeDiscardDateWS(date: any, orderItem: any, index: any) {
    if (date != undefined && date != null) {
      var orderItemRecords = this.ordersListWithoutScanner.filter(or => or.POrder_Id == orderItem.POrder_Id);
      orderItemRecords.forEach((orderElement, len) => {
        let num = (new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - new Date().setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24);
        let discardDateId = "#disca" + orderElement.pquantity_Id;
        $(discardDateId).val(this.dateFormatPipe.transformISODate(date));
        if (((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) >= 0 && ((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) <= 3) {
          let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
          let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
          $(rowBgColor).css("background-color", "#ade39d");
          let ordercheckBoxId = "#" + orderElement.pquantity_Id;
          $(ordercheckBoxId).prop("checked", false);
          $(ordercheckBoxId).css("display", "block");
          //this.wsCheckAllButtonDisplay();
        }
        else if (((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) < 0) {
          let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
          let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
          $(rowBgColor).css("background-color", "#ffdf62");
          let ordercheckBoxId = "#" + orderElement.pquantity_Id;
          $(ordercheckBoxId).css("display", "none");
          //this.wsCheckAllButtonDisplay();
          let objIndex = this.ordersListSelected.findIndex(cs => cs.pquantity_Id == orderElement.pquantity_Id);
          if (objIndex >= 0) {
            this.ordersListSelected.splice(objIndex, 1);
          }
        }
        else if (((new Date(this.dateFormatPipe.transform(date)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) > 3) {
          let rowIn = this.ordersListWithoutScanner.findIndex(or => or.pquantity_Id == orderElement.pquantity_Id);
          let rowBgColor = "#WSOrderRow" + rowIn + orderElement.pquantity_Id;
          $(rowBgColor).css("background-color", "white");
          let ordercheckBoxId = "#" + orderElement.pquantity_Id;
          $(ordercheckBoxId).prop("checked", false);
          $(ordercheckBoxId).css("display", "block");
          //this.wsCheckAllButtonDisplay();
        }
        if (orderItemRecords.length == (len + 1)) {
          this.wsCheckAllButtonDisplay();
        }
      });
    }
  }
  getDiscardDateForAdminister(quntityId: any) {
    let discardDateId = "#disca" + quntityId;
    let discardValue = $(discardDateId).val();
    return discardValue;
  }
  wsCheckAllButtonDisplay() {

    for (let i = 0; i < this.ordersListWithoutScanner.length; i++) {
      if (this.ordersListWithoutScanner[i].DiscardDays != null && this.ordersListWithoutScanner[i].DiscardDays != 0) {
        let rowBgColor = "#WSOrderRow" + i + this.ordersListWithoutScanner[i].pquantity_Id;
        if ($(rowBgColor).css("background-color") == "rgb(255, 223, 98)") {
          this.wsDiscard = 0;
          break;
        }
        else {
          //this.wsDiscard=1;
        }
      }
      if (this.ordersListWithoutScanner.length == (i + 1)) {
        this.wsDiscard = 1;
        break;
      }
    }
  }
  getTextColorForDiscardDate(discardDate: any) {
    if (discardDate == undefined || discardDate == null || discardDate == "") {
      return 2;
    }
    if (((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) >= 0 && ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - (new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) <= 3) {
      //Drug  must be discarded soon, reorder promptly
      return 1;
    }
    else if (((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) < 0) {
      //This drug package should no longer be used, open a new package
      return 2;
    }
    else if (((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0, 0, 0, 0) - ((new Date(this.dateFormatPipe.transform(this.nursingStationZoneCurrentDate))).setHours(0, 0, 0, 0))) / (1000 * 60 * 60 * 24)) > 3) {
      return 0;
    }
  }
  closeAdministerSitesModel() {
    this.modalAdministerInsulinSiteIsOpen = false;
    this.administerFromEkitModal = false;
    if (this.barcodeScanCheckbox == false) {
      this.barcodevalue = "";
      this.insulinSites = [];
    }
  }
  insertInsulinSites() {
    this.modalAdministerInsulinSiteIsOpen = false;
    //let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
    // if (orderItem != undefined) {
    //   this.orderGivenStatus(orderItem.POrder_Id, orderItem.pquantity_Id, orderItem.ReviewFlag, orderItem.PRNFlag, orderItem.dueflag, orderItem.InputTime, orderItem.ShiftId, orderItem.Window);
    // }
    let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
    if (orderItem != undefined) {
      this.getVitalsListForExpandedOrder(orderItem);
    }
  }
  insulinSiteCheck(event: any, siteId) {
    if (event == true) {
      if (this.insulinSites.find(id => id == siteId) == undefined) {
        this.insulinSites.push(siteId);
      }
      let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
      if (orderItem != undefined && (orderItem.Route == "SC" || orderItem.Route == "IM")) {
        this.displayInsulinSites.forEach(element => {
          if (element.item_id != siteId) {
            let siteCheckBoxId = "#site" + element.item_id;
            $(siteCheckBoxId).prop("disabled", true);
          }
        });
      }
    }
    else if (event == false) {
      let index = this.insulinSites.findIndex(id => id == siteId);
      if (index >= 0) {
        this.insulinSites.splice(index, 1);
      }
      let orderItem = this.ordersList.find(ele => ele.POrder_Id == this.orderId && ele.pquantity_Id == this.quantityId);
      if (orderItem != undefined && (orderItem.Route == "SC" || orderItem.Route == "IM")) {
        this.displayInsulinSites.forEach(element => {
          if (element.item_id != siteId) {
            let siteCheckBoxId = "#site" + element.item_id;
            $(siteCheckBoxId).prop("disabled", false);
          }
        });
      }
    }
  }
  displayInsulinSitesChecksReset() {

    this.displayInsulinSites.forEach(element => {
      let siteCheckBoxId = "#site" + element.item_id;
      $(siteCheckBoxId).prop("disabled", false);
      $(siteCheckBoxId).prop("checked", false);
      element.lastUsed = 0;
      element.lastUsedDate = "";
      this.displayInsulinSites.push();
      if (this.lastUsedSites != "") {
        let insulinSites = this.lastUsedSites.split(",");
        if (insulinSites.length > 0) {
          insulinSites.forEach(value => {

            var site = value.split(" | ");
            if (site[0] == (element.item_id.toString())) {
              element.lastUsed = 1;
              element.lastUsedDate = this.timeFormatId == 1 ? this.dateFormatPipe.get24HourDateTime(site[1]) : site[1];
              this.displayInsulinSites.push();
            }
          });
        }
        if (this.lastUsedSites.split(',').includes(element.item_id.toString())) {
          element.lastUsed = 1;
          this.displayInsulinSites.push();
        }
      }
    });
  }
  openInsulinSiteWithoutScannerModal(PQuantity_Id: any, route: any, administerSites: any) {

    if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) < this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered at this time. Past orders cannot be administered at this time.");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.lastUsedSiteWS = administerSites != null && administerSites != "" ? administerSites : "";
      this.porderIdForInsulin = PQuantity_Id;
      this.routeForInsulin = route;
      this.displayInsuliSitesForWS = [];
      this.displayInsuliSitesForWS = route == "TD" ? this.allInsulinSites.filter(f => f.item_type == 2) : this.allInsulinSites.filter(f => f.item_type == 1);
      this.sitesType = route == "TD" ? "Patch Sites" : "Administration Sites";
      this.wsOrderInsulinSites = [];
      this.displayInsulinSitesChecksResetWS();
    }
  }
  displayInsulinSitesChecksResetWS() {

    this.displayInsuliSitesForWS.forEach((element, index) => {
      if (this.insulinSiteIdsWS.length == 0 || this.insulinSiteIdsWS.find(i => i.PQuantity_Id == this.porderIdForInsulin) == undefined) {
        let siteCheckBoxId = "#insulin" + element.item_id;
        $(siteCheckBoxId).prop("disabled", false);
        $(siteCheckBoxId).prop("checked", false);
      }
      element.lastUsed = 0;
      element.lastUsedDate = "";
      this.displayInsuliSitesForWS.push();
      if (this.lastUsedSiteWS != "") {
        let insulinSites = this.lastUsedSiteWS.split(",");
        if (insulinSites.length > 0) {
          insulinSites.forEach(value => {

            var site = value.split(" | ");
            if (site[0] == (element.item_id.toString())) {
              element.lastUsed = 1;
              element.lastUsedDate = this.timeFormatId == 1 ? this.dateFormatPipe.get24HourDateTime(site[1]) : site[1];
              this.displayInsuliSitesForWS.push();
            }
          });
        }
      }
      if (this.insulinSiteIdsWS.length > 0 && this.insulinSiteIdsWS.find(i => i.PQuantity_Id == this.porderIdForInsulin) != undefined) {

        let Ids = this.insulinSiteIdsWS.find(i => i.PQuantity_Id == this.porderIdForInsulin).SiteIds.split(",");
        element.IsChacked = Ids.includes(element.item_id.toString()) == true ? true : false;
        element.IsDisabled = (this.routeForInsulin != "TD" && Ids.includes(element.item_id.toString()) == false) ? true : false;
        this.displayInsuliSitesForWS.push();
        let siteCheckBoxId = "#insulin" + element.item_id;
        $(siteCheckBoxId).prop("disabled", element.IsDisabled);
        $(siteCheckBoxId).prop("checked", element.IsChacked);
        if (element.IsChacked == true) {
          this.wsOrderInsulinSites.push(element.item_id);
        }
      }
      if (this.displayInsuliSitesForWS.length == (index + 1)) {
        this.administerFromEkitModal = false;
        console.log(this.modalAdministerInsulinSiteIsOpen, 'modalAdministerInsulinSiteIsOpen')
        this.modalAdministerInsulinSiteIsOpen = true;
      }
    });
  }
  insulinSiteCheckWS(event: any, siteId: any) {
    if (event == true) {
      if (this.wsOrderInsulinSites.find(id => id == siteId) == undefined) {
        this.wsOrderInsulinSites.push(siteId);
      }
      if (this.routeForInsulin != undefined && this.routeForInsulin != null && (this.routeForInsulin == "SC" || this.routeForInsulin == "IM")) {
        this.displayInsuliSitesForWS.forEach(element => {
          if (element.item_id != siteId) {
            let siteCheckBoxId = "#insulin" + element.item_id;
            $(siteCheckBoxId).prop("disabled", true);
          }
        });
      }
    }
    else if (event == false) {
      let index = this.wsOrderInsulinSites.findIndex(id => id == siteId);
      if (index >= 0) {
        this.wsOrderInsulinSites.splice(index, 1);
      }
      if (this.routeForInsulin != undefined && this.routeForInsulin != null && (this.routeForInsulin == "SC" || this.routeForInsulin == "IM")) {
        this.displayInsuliSitesForWS.forEach(element => {
          if (element.item_id != siteId) {
            let siteCheckBoxId = "#insulin" + element.item_id;
            $(siteCheckBoxId).prop("disabled", false);
          }
        });
      }
    }
  }
  insertInsulinSitesForWS() {
    if (this.insulinSiteIdsWS.length == 0) {
      let obj = {
        PQuantity_Id: this.porderIdForInsulin,
        RouteCode: this.routeForInsulin,
        SiteIds: this.wsOrderInsulinSites.join(','),
      }
      this.insulinSiteIdsWS.push(obj);
    }
    else if (this.insulinSiteIdsWS.length > 0) {
      let record = this.insulinSiteIdsWS.find(i => i.PQuantity_Id == this.porderIdForInsulin);
      if (record == undefined) {
        let obj = {
          PQuantity_Id: this.porderIdForInsulin,
          RouteCode: this.routeForInsulin,
          SiteIds: this.wsOrderInsulinSites.join(','),
        }
        this.insulinSiteIdsWS.push(obj);
      }
      else {
        let index = this.insulinSiteIdsWS.findIndex(i => i.PQuantity_Id == this.porderIdForInsulin);
        this.insulinSiteIdsWS.splice(index, 1);
        let obj = {
          PQuantity_Id: this.porderIdForInsulin,
          RouteCode: this.routeForInsulin,
          SiteIds: this.wsOrderInsulinSites.join(','),
        }
        this.insulinSiteIdsWS.push(obj);
      }
    }
    this.modalAdministerInsulinSiteIsOpen = false;
  }
  getDosesDetails(FacilityId: any, NsId: string) {
    this.dataservice.get<any>(this.config.Emar_Common_GetPendingDosesList + this.persistanceService.get(this.config.loggedInUserKey) + "/" + NsId + "/" + FacilityId)
      .subscribe(res => {
        this.sharedService.updateDosesList(res);
      });
  }
  undoButtonDisplay() {

    let check = this.ordersList.find(or => or.AdministerStatus == "Administered" && or.Undo == 1);
    if (check != undefined) {
      setTimeout(() => {

        this.getEmarOrdersList();
      }, 450);
    }
  }
  emarModel() {

    //this.getDemographicInfoData(this.residentId);
    this.ng4LoadingSpinnerService.show();
    this.modaleMar1 = true;
    let data = new Date();
    this.emarform.reset();
    this.emarform.patchValue({
      month: data.getMonth() + 1,
      year: data.getFullYear(),

    });
    this.ng4LoadingSpinnerService.hide();
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

        if (this.yearDrop == null || this.yearDrop.length == 0) {
          this.emarform.patchValue({
            year: "0",

          });
        }
        else {
          let data = new Date();
          var Years = data.getFullYear();
          for (let item of this.yearDrop) {

            if (item == Years) {

              var b = item;
            }
          }

          if (b != undefined) {
            this.emarform.patchValue({
              year: b,
            });
          }
          else {
            this.emarform.patchValue({
              year: data.getFullYear(),
            });
          }
        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  closeModelEmarPrVIew() {

    this.modaleMar1 = false;

  }
  mouseEnterLatest(Id: any) {

    this.MyImages = Id;

  }
  mouseEnter(Id: any) {

    // this.MyImages = Id;

    this.MyImages = "";

    //this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_Orders_GetOrdersGetImage + Id)
      .subscribe(res => {


        //this.VisitViewFlag = res;
        //this.ng4LoadingSpinnerService.hide();
        this.MyImages = res
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });

  }
  mouseLeave() {
    this.MyImages = null;

  }

  HideInactiveOrders(value: any) {

    if (value == true) {
      this.eMARDetails = this.eMARDetails.filter(order => order.POrder_Status === 1);
    }
    else if (value == false) {
      this.getEMARDetails();
    }
  }
  openPendingOrdersModal() {
    debugger
    this.r = 1;
    this.modalPendingOrderIsOpen = true;
    setTimeout(() => {
      this.pendingOrderSearchFocusField.nativeElement.focus();
    }, 200);
    this.getPendingOrders();
    // this.closeModel();
  }
  closeModel() {
    this.modalResidentAllOrderIsOpen = false;
    this.modalPendingOrderIsOpen = false;
    this.pendingOrderSearch = "";
    this.barcodeAlertsPop = false;
    this.administerFromEkitModal = false;
    this.ekitDrugForm.patchValue({
      barcode: '',
      drugSelect: ''
    })
    this.ekitDrugForm.reset();
    this.ekitDrug = [];
    this.barcodeFlagKeys = {};
    this.ekitSelctedarray = []
    this.dataEkitPost = [];
    //this.getPendingOrders();
    // this.openPendingOrdersModal();
  }
  closeModalEkitAdLotQty(){
    this.modalekitAdLotQty = false;
    this.barcodeEkitScanCheckboxValue = false;
    this.inHandValues = [];
    this.lotsGrid = [];
    this.calculateTotal();
  }
  getPendingOrders() {
    this.filterConfigs = {
      CompanyID: 0,
      Floors: [],
      Facilities: this.adminform.value.ddlfacilities.length != 0 ? this.adminform.value.ddlfacilities.map(item => item.Facility_Id) : [],
      NurseStations: this.adminform.value.nstationName.length != 0 ? this.adminform.value.nstationName.map(item => item.NurseStation_Id) : [],
      Wings: [],
      Rooms: [],
      Beds: [],
      ControlType: this.controlType,
      Type: this.type,
      User_Id: this.userId,
      VisitStatus: this.visitStatusForm.value.visitStatus,
    }
    this.pendingOrdersList = [];
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_GetPendingOrdersGridData, this.filterConfigs)
      .subscribe((res: any) => {
        this.ng4LoadingSpinnerService.hide();
        this.pendingOrdersList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
    // this.closeModel();
  }


  openPendingOrdersModalInside() {
    debugger
    this.r = 1;
    this.modalPendingOrderIsOpen = true;
    setTimeout(() => {
      this.pendingOrderSearchFocusField.nativeElement.focus();

    }, 200);

    this.getPendingOrdersInside(this.PatientInside);
    // this.closeModel();
  }
  getPendingOrdersInside(id: number) {
    debugger;
    //this.pendingOrdersList = this.pendingOrdersList.find(x=>x.Patient_Id == id);

    this.filterConfigs = {
      CompanyID: id,
      Floors: [],
      Facilities: this.adminform.value.ddlfacilities.length != 0 ? this.adminform.value.ddlfacilities.map(item => item.Facility_Id) : [],
      NurseStations: this.adminform.value.nstationName.length != 0 ? this.adminform.value.nstationName.map(item => item.NurseStation_Id) : [],
      Wings: [],
      Rooms: [],
      Beds: [],
      ControlType: this.controlType,
      Type: this.type,
      User_Id: this.userId,
      VisitStatus: this.visitStatusForm.value.visitStatus,
    }
    this.pendingOrdersList = [];
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_Orders_GetPendingOrdersGridDataInside, this.filterConfigs)
      .subscribe((res: any) => {
        this.ng4LoadingSpinnerService.hide();
        this.pendingOrdersList = res;

        console.log(this.pendingOrdersList.length);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      });
    //   // this.closeModel();
  }

  RowClickwithoutScannerVitals(item: any) {
    this.administerWithoutScannerInputTime = ''
    this.administerWithoutScannerShiftId = null
    this.administerWithoutScannerWindow = null
    debugger;
    this.withoutScannerTempVitals = [];
    var vitalCheckRecordsrc = this.ordersListWithoutScanner.filter(r => r.pquantity_Id == item);
    console.log(vitalCheckRecordsrc);

    if ((this.myform.value.length == 0 || this.myform.value == null || this.myform.value == undefined) || (this.myform.value.dateCheck == "" || this.myform.value.dateCheck == null) || (this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime == null)) {
      this.CheckAll = false;
      const index = this.selectedRecords.findIndex(i => i.DrugAdminister_Id == item.DrugAdminister_Id);
      if (index >= 0) {
        this.selectedRecords.splice(index, 1);
      }
      let ordercheckBoxId = "#" + item.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked", false);
      this.alertService.warn("Please select Administer By & Date/Time");
      this.ng4LoadingSpinnerService.hide();

    }
    else {
      if (vitalCheckRecordsrc.length > 0) {
        this.administerWithoutScannerInputTime = vitalCheckRecordsrc[0].InputTime
        this.administerWithoutScannerShiftId = vitalCheckRecordsrc[0].ShiftId
        this.administerWithoutScannerWindow = vitalCheckRecordsrc[0].Window
        vitalCheckRecordsrc.forEach((element, index) => {
          this.withoutScannerOrderVitals = [];
          this.WsPRN.reset();
          this.spinnerLoading++;
          this.checkAndHideSpinnerLoading();
          this.dataservice.get<any[]>(this.config.Emar_GetVitalsCheckList + item)
            .subscribe(res => {
              if (res.length > 0) {
                let vitalIds = [];
                let vitalDescs = [];
                res.forEach(item => vitalIds.push(item.OrderFavMaster_ID));
                res.forEach(item => vitalDescs.push(item.OrderFavDesc));
                let obj =
                {
                  orderId: element.POrder_Id,
                  qtyId: element.pquantity_Id,
                  Vitals: vitalIds.join(","),
                  DadminId: item.DrugAdminister_Id,
                  Drug: element.Drug + " (" + vitalDescs.join(", ") + ")",
                  InputTime: element.InputTime,
                }
                this.withoutScannerTempVitals.push(...res);
                if (this.withoutScannerOrderVitals.length == 0 || (this.withoutScannerOrderVitals.find(w => w.qtyId == element.pquantity_Id) == undefined)) {
                  this.withoutScannerOrderVitals.push(obj);
                }
                this.afterGetVitalsList();
              }
              this.spinnerLoading--;
              this.checkAndHideSpinnerLoading();
            },
              error => {
                this.ng4LoadingSpinnerService.hide();
                this.alertService.error(error.message);
                this.spinnerLoading--;
                this.checkAndHideSpinnerLoading();
              });
        });
        this.userInputsOpen();
      }
    }
  }
  GetRefillNotes(porder_Id: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<RefillNotes>(this.config.Emar_Emar_GetEmarRefillNotes + porder_Id)
      .subscribe(res => {
        if (res.Refill_Note != '') {
          this.getRefillNotes = res.Refill_Note
        }
        console.log(res, "res")
        this.ng4LoadingSpinnerService.hide();

      })
  }
  checkAndHideSpinnerLoading() {

    if (this.spinnerLoading === 0 || this.spinnerLoading < 0) {
      this.ng4LoadingSpinnerService.hide();
    } else {
      this.ng4LoadingSpinnerService.show();

    }
  }
  prncreateChart(prnseriesData, prnxaxisData, prnyaxisTitle, chartTitle): void {
    // this.ng4LoadingSpinnerService.show();

    const options: Highcharts.Options = {
      chart: {
        type: 'pie',
        zoomType: 'x',
        renderTo: 'container',
        height: 250,
        inverted: false,
        options3d: {
          enabled: true,
          alpha: 10,
          beta: 25,
          depth: 70
        }
      },
      colors: [
        '#007bff',
        '#dddddd',
        '#ffc107',
        '#007bff'
      ],
      title: {
        text: ''
      },
      //   tooltip: {
      //     pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
      // },
      plotOptions: {
        pie: {

          allowPointSelect: true,
          cursor: '',
          dataLabels: {
            enabled: false,
            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
          },
          showInLegend: true
        }
      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },

      xAxis: {
        categories: prnxaxisData,
        labels: {
          x: -10
        }
      },

      yAxis: {
        allowDecimals: false,
        title: {
          text: prnyaxisTitle
        }
      },
      series: [{
        type: 'pie',
        name: 'Count',
        data: prnseriesData
      }],
      responsive: {
        rules: [{
          condition: {
            maxWidth: 400
          },
          chartOptions: {
            legend: {
              align: 'center',
              verticalAlign: 'bottom',
              layout: 'vertical'
            },
            yAxis: {
              labels: {
                align: 'left',
                x: 0,
                y: -5
              },
              title: {
                text: chartTitle
              }
            },
            subtitle: {
              text: 'PRN Med Pass Status',
              style: {
                color: '#212529',
                fontWeight: '600'
              }
            },
            credits: {
              enabled: false
            }
          }
        }]
      },
      credits: {
        enabled: false
      }
    }
    this.prnchart = chart(this.prnchartTarget.nativeElement, options);
    // this.ng4LoadingSpinnerService.hide();
  };
  itemMatchesFilter(item): boolean {
    return this.filteredItems.some(filteredItem => filteredItem.Patient_Id === item.Patient_Id);
  }
  itemMatchesFilterPrn(item): boolean {
    return this.filteredprnitems.some(filteredItem => filteredItem.Patient_Id === item.Patient_Id);
  }
  getAllBarcodes(patientId: number) {
    let date = this.myform.value.dateCheck.split('/').join('-');
    this.dataservice.get<any[]>(this.config.Emar_Emar_GetEmarResidentBarcodes + date + "/" + patientId)
      .subscribe(res => {
        this.barcodesPList = res;
        console.log(this.barcodesPList, "barcodesPList")
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  ngAfterViewInit() {

  }
  closeModelbarcodeAlertsPop() {
    this.barcodeAlertsPop = false;
    this.inputField.nativeElement.focus();
  }
  GetDate2hrsDiff(lastPassedDate: any, record: any) {
    console.log(lastPassedDate, "lastPassedDate")
    this.sharedService.currentFacilityId.subscribe(res => this.barcodeFacilityId = res);
    const obj = {
      FacilityId: this.barcodeFacilityId,
      LastPassedDate: lastPassedDate
    }
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_GetDate2hrsDiff, obj)
      .subscribe(res => {
        if (res == 1) {
          this.twoHours = "must have 2 hours gap";
          this.modalPRNIsOpen = true;

        } else {
          this.twoHours = "";
          if (record.Ekit != undefined && record.Ekit == 1) {
            this.getVitalsCheckListData(record.pquantity_Id, record.ReviewFlag, record.dueflag, 1);
          }
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        })
  }
  ekitDrugDropdown(gpi: string, DrugName: string) {
    debugger;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar__EkitMeds_GetEkitadministration + gpi)
      .subscribe(res => {
        this.ekitDrugDrop = res;
        debugger
        if (this.ekitDrugDrop.length) {
          let ekitDrugFiltered = this.ekitDrugDrop.filter(x => x.DrugName.toLowerCase().trim() == DrugName.toLowerCase().trim());
          if (ekitDrugFiltered.length == 0) {
            this.ekitDrug = this.ekitDrugDrop;
          } else {
            this.ekitDrug = ekitDrugFiltered;
          }
          console.log(this.ekitDrug, "ekitdrug");
          console.log(this.ekitDrugDrop, "ekitDrugDrop");

          // this.ekitDrugForm.patchValue({
          //   drugSelect: selectDrug
          // })
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  onEkitDrugSelect(item: any) {
    console.log(item, "selected irtem")
  }
  onEkitDrugDeSelect(item: any) {
    console.log(item, "selected irtem")

  }
  SaveAlertEkitAdminister() {

    if (this.ekitDrug.length) {
      this.InsertBarcodeekit = {
        DrugName: this.ekitDrug[0].DrugName,
        Barcode: this.ekitDrugForm.value.barcode.trim()
      }
      // if(this.barcodeEkitScanCheckboxValue){
      //   this.GetEkitLotsGrid(this.ekitDrugForm.value.drugSelect[0].DrugName,'0');
      // }
      // else{
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_EkitMeds_AlertEkitAdminister, this.InsertBarcodeekit)
        .subscribe(res => {
          this.lotdetailsGrid = res;
          this.barcodeEkitScanCheckboxValue = false;
          // this.sharedService.currentFacilityId.subscribe(res=>this.barcodeFacilityId = res);
          this.lotModelEkitOpen(this.ekitDrugForm.value.drugSelect[0].DrugName, this.ekitDrugForm.value.barcode.trim());
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          //this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();
          });
      // }

    } else {
      this.alertService.warn("Please select Drug Name")
    }

    console.log(this.InsertBarcodeekit, "InsertBarcodeekit")

  }
  lotModelEkitOpen(drug: string, barcode: string) {
    debugger
    if (this.lotdetailsGrid == 0) {
      this.modalekitAdLotQty = true;
      this.GetEkitLotsGrid(drug, barcode.trim());
    }
    else {

      if (this.lotdetailsGrid == 1) {
        this.modalekitAdLotQty = false;
        this.administerFromEkitModal = false;
        this.ekitDrugForm.patchValue({
          barcode: '',
          drugSelect: ''
        })
        this.ekitDrugForm.reset();
        this.ekitDrug = [];
        this.alertService.warn("This is not the correct drug, do not administer")
      }
      else if (this.lotdetailsGrid == 2) {
        this.modalekitAdLotQty = false;
        this.administerFromEkitModal = false;
        this.ekitDrugForm.patchValue({
          barcode: '',
          drugSelect: ''
        })
        this.ekitDrugForm.reset();
        this.ekitDrug = [];
        this.alertService.warn("This package has expired, do not administer");

      }
      else if (this.lotdetailsGrid == 3) {
        this.barcodeExpAlert = true;
        this.administerFromEkitModal = false;
        //this.alertService.warn("This is not the correct drug, do not administer")

      }
    }
  }
  GetEkitLotsGrid(drug: string, barcode: string) {
    this.sharedService.currentFacilityId.subscribe(res => this.barcodeFacilityId = res);

    // let safeDrugName =  drug.replace(/[\/.]/g, function(match) {
    //   if (match === '/') return '#';
    //   if (match === '.') return '$';
    //   if (match === '%') return '@';
    // });
    let safeDrugName = drug
      .split('') // Split the string into individual characters
      .map(char => {
        if (char === '/') return '~';
        if (char === '.') return '$';
        if (char === '%') return '@';
        if (char === ':') return '!';
        if (char === '*') return '{';
        // if(char === "'") return '}';
        if (char === ">") return '_';
        if (char === "<") return '}';
        return char; // Leave any other character unchanged
      })
      .join('');
    this.selectedDrugName = drug;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get(this.config.Emar_EkitMeds_GetEkitLotsGrid + encodeURIComponent(safeDrugName) + '/' + barcode + '/' + this.barcodeFacilityId+'/'+this.demographicInfoData.NursingStationId)

      .subscribe(res => {
        debugger;
        this.lotsGrid = res;
        this.administerFromEkitModal = false;
        //  this.barcodeEkitScanCheckboxValue = false;
        this.lotsGrid.forEach(item => {
          if (item.BarcodeMatch == 1) {
            this.barcodeFlagKeys[item.Ekit_Id] = 1;
            this.selectedEkit_Id = item.Ekit_Id;
            this.selectedInhandQty = item.Inhand;

          }
        }
        );

        // this.initializeForm();
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  //  getTotalInhand() {
  //    this.totalQtyval = this.textform3.reduce((sum, item) => sum + item.Inhand, 0);
  // }
  initializeForm() {
    if (this.lotsGrid.length > 0) {
      this.lotsGrid.forEach((item, index) => {
        this.inHandValues[index] = (item.InHandqty == undefined || item.InHandqty == '') ? '' : item.InHandqty;
      });
      this.calculateTotal();
    }
  }

  //  inHands() {
  //   return this.textform3.get('InHandqty') as FormArray;
  // }

  // getControlName(index: number) {
  //   return `InHandqty${index}`;
  // }

  // getControl(index: number) {
  //   return this.inHands().at(index) as FormControl;
  // }
  // calculateTotal() {
  //   this.totalQtyval= (this.inHands() != null &&  this.inHands() != undefined) ? this.inHands().value.reduce((acc, curr) => acc + Number(curr), 0):0;
  //   console.log(this.totalQtyval,"this.totalQtyval")
  // }

  onValueChange(index: number, event: Event, lotNumber: any, ekitId: any, inhand: any): void {
    debugger
    this.ekitWithoutScanningExceedQtyFlag = false;
    const value = (event.target as HTMLInputElement).value;
    this.inHandValues[index] = Number(value);

   

    // Track if any input exceeds available quantity
    let isAnyQuantityExceeded = false;

    // Loop through all the values to check if any of them exceed the available quantity.
    for (let i = 0; i < this.inHandValues.length; i++) {
        if (this.inHandValues[i] > this.lotsGrid[i].Inhand ) {
            isAnyQuantityExceeded = true;
            break;  // No need to check further once we know a quantity exceeds.
        }
    }

    if (isAnyQuantityExceeded) {
        this.alertService.warn("You have exceeded the quantity available in the lot.");
        this.ekitWithoutScanningExceedQtyFlag = true;
    } else {
        this.ekitWithoutScanningExceedQtyFlag = false;
    }

    // Check if the selected eKit ID does not match and the value entered exceeds the selected quantity.
    if (this.selectedEkit_Id != ekitId && Number(this.selectedInhandQty) >= Number(value)) {
        this.alertService.warn("You have enough units to dispense using early Expiration Date");
    }

    // Disable Ok button if any value exceeds available quantity.
    this.calculateTotal();
    this.updateOkButtonStatus();
    
}

// Method to update the Ok button's disabled state
updateOkButtonStatus(): void {
    // Disable the Ok button if the flag is set
    this.isQuantityExceeded = this.ekitWithoutScanningExceedQtyFlag || this.totalQtyval === 0;
}

  calculateTotal(): void {
    debugger
    this.totalQtyval = this.inHandValues.reduce((acc, curr) => acc + Number(curr), 0);
    console.log(this.inHandValues, "this.inHandValues")

    console.log(this.totalQtyval, "this.totalQtyval")
  }
  UpdateEkitDrugQuantity() {
    debugger;
    //   const dataToPost = this.lotsGrid.map((item, index) => ({
    //     LotNumber: item.LotNumber,
    //     ExpiryDate: item.ExpiryDate,
    //     Ekit_Id:item.Ekit_Id,
    //     InHandqty: (this.inHandValues[index] == null || this.inHandValues[index] == undefined) ? '0' : this.inHandValues[index]
    //   }
    //     )

    //   );
    //   this.ekitSelctedarray =[]
    // let val=  this.lotsGrid.filter((item,index) => {
    //     if(this.inHandValues[index] >0){
    //       this.ekitSelctedarray.push(item.Ekit_Id);
    //     }
    //   });
    //cod estarts
    // this.dataEkitPost =[];
    // this.dataEkitPost = this.lotsGrid
    //   .filter((item, index) => {
    //     const inHandQty = this.inHandValues[index];
    //     if (inHandQty > 0) {
    //       this.ekitSelctedarray.push(item.Ekit_Id); // Add to selected array
    //       return true; // Include this item in dataToPost
    //     }
    //     return false; // Exclude this item
    //   })
    //   .map((item, index) => ({
    //     LotNumber: item.LotNumber,
    //     ExpiryDate: item.ExpiryDate,
    //     Ekit_Id: item.Ekit_Id,
    //     InHandqty: this.inHandValues[index] || '0', // Default to '0' if undefined or null
    //   }));
    //code ends
    const ekitInHandMap = this.lotsGrid.reduce((acc, item, index) => {
      acc[item.Ekit_Id] = this.inHandValues[index] || '0'; // Default to '0' if no value
      return acc;
    }, {});

    // Step 2: Filter and map lotsGrid based on the mapping
    this.dataEkitPost = this.lotsGrid
      .filter(item => ekitInHandMap[item.Ekit_Id] > 0) // Only include items with InHandqty > 0
      .map(item => ({
        LotNumber: item.LotNumber,
        ExpiryDate: item.ExpiryDate,
        Ekit_Id: item.Ekit_Id,
        InHandqty: ekitInHandMap[item.Ekit_Id], // Retrieve InHandqty using Ekit_Id
        remainingekitids:item.remaining_ekit_ids
      }));

    // Step 3: Update ekitSelctedarray based on filtered data
    this.ekitSelctedarray = this.dataEkitPost.map(post => post.Ekit_Id);

    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.undoFlag = false;
    console.log(this.dataEkitPost)
    this.orderGivenStatus(this.administerOrder.POrder_Id, this.administerOrder.pquantity_Id, this.administerOrder.ReviewFlag, this.administerOrder.PRNFlag, this.administerOrder.dueflag, this.administerOrder.InputTime, this.administerOrder.ShiftId, this.administerOrder.Window);

    this.dataservice.post(this.config.Emar_EkitMeds_UpdateEkitDrugQuantity, this.dataEkitPost)
      .subscribe(res => {
        if (res == 1) {
          this.modalekitAdLotQty = false;
          this.administerFromEkitModal = false;
          this.undoFlag = false;
          this.newProductIsOpen = false;
          this.ekitDrugForm.patchValue({
            barcode: '',
            drugSelect: ''
          });
          this.ekitDrugForm.reset();
          this.ekitDrug = [];
          this.administerOrder = null;
          this.inHandValues = [];
          this.barcodeFlagKeys = {};
          // this.lotsGrid=[];
          this.barcodeEkitScanCheckboxValue = false;
          this.calculateTotal();
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
          console.log("given order status 2")
        }
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });

  }
  Checkbarcodelot(barcode: any) {
    if (barcode == 1) {
      // this.alertService.warn("Administered barcode does not belong to this lot")
    }

  }
  onBarcodeCheck(index: number, event: Event, lotNumber: any, ekitId: number) {
    console.log(event, "event cgecj");
    let barcode = (event.target as HTMLInputElement).value;
    let obj = {
      lotNumber: lotNumber,
      Ekit_Id: ekitId,
      Barcode: barcode
    }
    console.log(obj, "obj")
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_EkitMeds_FlagEkitLotsGrid, obj)
      .subscribe(res => {
        this.barcodeFlag = res;
        if (this.barcodeFlag == 1) {
          this.barcodeFlagKeys[ekitId] = 1;
        }
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });
  }
  replaceCommaWithPeriod(value: string | number): string {
    if (typeof value === 'number') {
      value = value.toString();
    }
    return value.includes(',') ? value.replace(/,/g, '.') : value;
  }
  ContinueNonExpLot() {
    debugger
    this.modalekitAdLotQty = true;
    this.barcodeExpAlert = false;
    this.GetEkitLotsGrid(this.ekitDrugForm.value.drugSelect[0].DrugName, this.ekitDrugForm.value.barcode.trim());
  }
  OpenEkitAlert() {
    // this.barcodeAlertsPop = true;
    console.log(this.selectEkitGpi, "selectEkitGpi");
    console.log(this.barcodeExpAlert, "barcodeExpAlert")

    this.barcodeExpAlert = false;
    this.administerFromEkitModal = true;
    this.ekitDrugForm.controls.barcode.reset()
    setTimeout(() => {
      this.ekitItem.nativeElement.focus();
    }, 300);
  }
  barcodeEkitScanCheckbox() {
    this.barcodeEkitScanCheckboxValue = !this.barcodeEkitScanCheckboxValue
    this.GetEkitLotsGrid(this.ekitDrugForm.value.drugSelect[0].DrugName, '0');
    this.modalekitAdLotQty = true;
   

  }
  barcodeAdministerAlertCheck() {
    setTimeout(() => {
      this.SaveAlertEkitAdminister()
    }, 500)
  }
}
