import { ChangeDetectorRef, Component, ElementRef, OnInit, QueryList, Renderer2, ViewChild } from '@angular/core';
import { DataService } from 'src/app/services/shared/dataservice.service';
import { APIConfiguration } from 'src/app/models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from 'src/app/services/shared/persistance.service';
import { AlertService } from 'src/app/_services';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SharedService } from 'src/app/services/shared/shared.service';
import { NurseStation } from 'src/app/models/facility.model';
import { CheckInMeds } from 'src/app/models/checkinmeds.model';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Ekit, EkitMeds, InsertLotEkit } from '../../../models/common.model';
import { BarcodeEntity } from '../../../models/orders.model';
import { DatePipe } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EkitmedsqtyupdatesdiscardComponent } from '../ekitmedsqtyupdatesdiscard/ekitmedsqtyupdatesdiscard.component';
import { TodaysdatealertComponent } from '../todaysdatealert/todaysdatealert.component';

@Component({
  selector: 'app-ekitmedscheckin',
  templateUrl: './ekitmedscheckin.component.html',
  styleUrls: ['./ekitmedscheckin.component.css'],
  providers: [DataService, APIConfiguration]
})
export class EkitmedscheckinComponent implements OnInit {
  pageConfig = {};
  template;
  public barcodear: any[] = [];
  public barcodesList: BarcodeEntity[];
  myform: FormGroup;
  userId: number;
  loginUserReceFacility: any;
  loginUserReceNurseStation: any;
  facilities: any[];
  nurseStations: NurseStation[];
  selectedfaItems = [];
  selectednItems = [];
  public ekitObj: any = [];
  public ekitGrid: any = [];
  public ekitLotGrid: any = [];
  searchText: string = "";
  private EkitMeds = new EkitMeds();

  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  //dropdownSettings_Residents: any = {};
  ShowFilter = true;
  residents: any[];
  //selectedResItem: any[];
  ordersList: any[] = [];
  currentPageNo: number = 1;
  gridPagination = this.config.gridPagination;
  //totalRecords: number = 0;
  checkInMedsObj: any[] = [];
  textform: FormGroup;
  textform1: FormGroup;
  textform2: FormGroup;
  textform3: FormGroup;
  textform4: FormGroup;
  residentId: number = 0;
  statusFlag: boolean = false;
  BarcodeValue: string = "";
  public p: number = 1;
  public modalekitLotQty: boolean = false;
  public lotform: FormGroup;
  lotnumverVal: string;
  expiryDateVal: string;
  barCodeVal: string;
  qtyInhand: number;
  private InsertLotEkit = new InsertLotEkit();
  selectedDrugName = '';
  selectedGpiCode = '';
  totalQtyval: number;
  public today: any = this.dateFormatPipe.transformISODate(new Date());
  public nextDate: any = this.dateFormatPipe.transformISODate(new Date());
  // public selectedExpLot :any[]=[]
  selectedExpLot: Set<number> = new Set<number>();
  myExpform: FormGroup;
  expiryModal: boolean = false;
  allbarcodesList = [];
  destroyQty: number = 0;
  totalInhandQty: number = 0;
  ekitUpdateId: number = 0;
  selectedEditEkit_id: number = 0;
  enableDestroy: boolean = false;
  updateInhandQty: number = 0;
  updateEkitId: number;
  destroyFlagKeys: { [key: string]: number } = {};
  public selectedLotNum: any = 0;
  public selectedExpDate: string = '';
  fetchflag: boolean = false;
  @ViewChild('barcodeFocus') barcodeFocus: ElementRef;
  public spinnerLoading: number = 0;
  public DestructionOrderSearch = '';
  public isDestructionModal: boolean = false;
  public destroyGrid = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  public selectedRecords: any[] = [];
  public selectedDrug: any[] = [];
  public destroyFlag: boolean = false
  public destroyItems: any[] = []
  public inHandValues: number[] = [];
  destroyform: FormGroup;
  public modalDesIsOpen: boolean = false;
  public selectedItems: any[] = []
  orderDestroyObj: any[] = [];
  selectedNursestationid: number = 0;
  public alertFlag: boolean = true
  public lastExpiryDate: string = '';
  public fieldDisableFlag: boolean = false
  public editReasonFlag: boolean = false;
  public ekitFetchItems: any[] = [];
  public initialDestroyGrid: any[] = [];
  public valueChangesFlagReceive: number = 0;
  public newRecordFlag:boolean=true
  @ViewChild('destroyFocus') destroyFocus: ElementRef
  constructor(private dataservice: DataService, private sharedService: SharedService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, private alertService: AlertService, private dateFormatPipe: CustomdatePipe,
    private el: ElementRef, private modalService: NgbModal, private renderer: Renderer2, private cd: ChangeDetectorRef, private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("EkitMedicationCheck-in");
    const currentDate = new Date();
    currentDate.setDate(currentDate.getDate() );
    this.today = this.dateFormatPipe.transformISODate(currentDate);
    //this.nextDate=this.dateFormatPipe.transformISODate(nextDate);

    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;

        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
          ddlnursestations: new FormControl(''),
        });
        this.textform = new FormGroup({
          lotnumber: new FormControl('', Validators.required),
        });
        this.textform1 = new FormGroup({
          expiryDate: new FormControl('', Validators.required)
        });
        this.textform2 = new FormGroup({
          barcode: new FormControl('', Validators.required)
        });
        this.textform3 = new FormGroup({
          InHand: new FormControl('', Validators.required)
        });
        this.textform4 = new FormGroup({
          reason: new FormControl('')
        });
        this.myExpform = new FormGroup({
          destroy: new FormControl('')
        })
        this.destroyform = new FormGroup({
          destroyerUsername: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          destroyerPass: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password), Validators.minLength(4)]),
          approvalUsername: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          approvalPass: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password), Validators.minLength(4)]),
        });
        //this.getAllBarcodes(); to avoid loading issue in testurl
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.getUserRecentFacilityNurseStations();
        this.userActivity();
        // this.UnionBarcodeDetails();
        this.sharedService.saveChangesFlag.subscribe(res => {
          this.valueChangesFlagReceive = res;
          console.log(this.valueChangesFlagReceive, "value changes flag")
        });
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.EkitMedsCheckIn, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  addBarcode(EKitId: any, GPICode: any) {

    let barcodeControl = "#br" + EKitId;
    var barcode = $(barcodeControl).val().toString();
    if (barcode != '' && barcode.length <= 20) {

      let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcode.toLowerCase().replace(/\s/g, '') : false);
      let Recods = result.filter(x => x.GPICode == GPICode && Recods.length == 0);
      if (result.length > 0 && Recods.length == 0) {
        // this.alertService.warn("This barcode is assigned to an item/drug with a different GPI");
        this.alertService.warn("This barcode is assigned to different drug");
        let barcodeControl = "#br" + EKitId;
        var barcode2 = $(barcodeControl).val("");
      }
    }
  }
  insertEkitMedsInfo(EKitId: number, drugName: string) {

    let lotNumbercontrol = "#lot" + EKitId;
    var lotNumber = $(lotNumbercontrol).val().toString();
    let expiryDatecontrol = "#exp" + EKitId;
    var expiryDate = $(expiryDatecontrol).val().toString();
    let barcodeControl = "#br" + EKitId;
    var barcode = $(barcodeControl).val().toString();
    let InHandControl = "#hand" + EKitId;
    var InHand = $(InHandControl).val().toString();
    debugger
    if (expiryDate == undefined || expiryDate == "") {
      this.alertService.warn("Expiration date must be entered to proceed Check-in");
    }
    else if (expiryDate != undefined && expiryDate != "" && ((new Date(this.dateFormatPipe.transform(expiryDate)).setHours(0, 0, 0, 0) - new Date().setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) < 0) {
      // this.alertService.warn("Expiration date cannot be earlier than today’s date");
      this.alertService.warn("Expiration Date entered is in the past, please re-enter");
      var item = this.ekitObj.find(o => o.Ekit_Id == EKitId);
      if (item != undefined && item.ExpiryDate != null && item.ExpiryDate != "") {
        let control = "#exp" + EKitId;
        var date = $(control).val(this.dateFormatPipe.transformISODate(item.ExpiryDate));
        return date;
      }
      else if (item != undefined && (item.ExpiryDate == null || item.ExpiryDate == "")) {
        let control = "#exp" + EKitId;
        var date = $(control).val("");
        return date;
      }
    }
    else {

      this.EkitMeds = {
        Ekit_Id: EKitId,
        Facility_Id: this.myform.value.ddlfacilities[0].Facility_Id,
        NurseStation_Id: this.myform.value.ddlnursestations[0].NurseStation_Id,
        DrugName: drugName,
        InHand: InHand,
        LotNumber: lotNumber,
        ExpiryDate: expiryDate,
        Ekit_Status: 1,
        Ekit_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        Ekit_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
        BarCodeDetails: barcode
      };
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_EkitMeds_UpdateEkitMedsCheckIn, this.EkitMeds)
        .subscribe(res => {

          this.alertService.success("Save successful");
          this.getEkitMedsCheckInData();
          this.GetEkitGridDetails();
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        }, error => {
          this.alertService.error(error.message);

        });
    }
  }
  getAllBarcodes() {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_StockEkit_GetAllStockEkitBarcodes + "/" + 1)
      .subscribe(res => {
        this.barcodesList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.alertService.error(error.message);

        });
  }
  getUserRecentFacilityNurseStations() {
    debugger
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          console.log(res, 'getUserRecentFacNs')
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFiltersData(this.userId);
        this.UnionBarcodeDetails();


      }, error => {
        this.alertService.error(error.message);
      });
  }
  getFiltersData(userId: number): any {
    debugger
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    //let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        this.facilities = res.Facilities;

        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacilities: this.selectedfaItems,
            });
          }
        }

        if (res.Facilities.length == 1) {
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.myform.patchValue({
            ddlfacilities: this.facilities,
          });
        }
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
    this.dropdownSettings_Facilities = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Facilities",
      itemsShowLimit: 1,
      closeDropDownOnSelection: true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_NurseStations = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Nursing Stations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection: true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: this.ShowFilter
    };
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.spinnerLoading++;
    debugger
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        console.log(res, 'GetNurseStationsByFacilityId')
        this.nurseStations = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
        if (this.nurseStations.length == 1) {
          this.myform.patchValue({
            ddlnursestations: this.nurseStations,
          });
          this.GetEkitGridDetails();

        }
        else {
          if (this.loginUserReceNurseStation != undefined) {
            let userReceNSList = this.loginUserReceNurseStation.split(',');
            if (userReceNSList.length > 0) {
              this.selectednItems = [];
              for (let i = 0; i < userReceNSList.length; i++) {
                let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
                if (checkNsExist != undefined) {
                  this.selectednItems.push(checkNsExist);

                }
              }
              this.myform.patchValue({
                ddlnursestations: this.selectednItems,
              });
            }
            // this.getEkitMedsCheckInData();
            this.GetEkitGridDetails();
          }
        }

      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }

  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.selectednItems = [];
    this.ekitObj = [];
    this.ekitGrid = [];
    this.myform.patchValue({
      ddlnursestations: '',
    });
    this.getNurseStationByFacilityID(item.Facility_Id);
    // this.getEkitMedsCheckInData();
    this.GetEkitGridDetails();
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.selectednItems = [];
    this.ekitObj = [];
    this.ekitGrid = [];
    this.myform.patchValue({
      ddlnursestations: '',
    });
  }
  onNurseStationSelect(item: any) {
    this.ekitObj = [];
    this.ekitGrid = [];
    // this.getEkitMedsCheckInData();
    this.GetEkitGridDetails();
  }

  getEkitMedsCheckInData() {

    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    let nsId = this.selectednItems.length > 0 ? this.selectednItems[0].NurseStation_Id : 0;
    this.dataservice.get<any[]>(this.config.Emar_EkitMeds_GetEkitMedsCheckIn + this.selectedfaItems[0].Facility_Id + "/" + nsId)
      .subscribe((res: any) => {

        this.ekitObj = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }
  onNurseStationDeSelect(item: any) {
    this.alertService.error("Please select nursing station");
    this.ekitObj = [];
    this.ekitGrid = [];
    //this.getEkitMedsCheckInData();
    this.GetEkitGridDetails();
  }

  GetEkitGridDetails() {

    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    let nsId = this.selectednItems.length > 0 ? this.selectednItems[0].NurseStation_Id : 0;
    this.dataservice.get<any[]>(this.config.Emar_EkitMeds_GetEkitGridDetails + this.selectedfaItems[0].Facility_Id + "/" + nsId)
      .subscribe((res: any) => {

        this.ekitObj = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);

      });
  }

  GetEkitLotDetails(drugName: string) {
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    let facilityId = this.selectedfaItems.length > 0 ? this.selectedfaItems[0].Facility_Id : 0;
    // let safeDrugName = drugName.replace(/\//g, '#');
    // let safeDrugName =  drugName.replace(/[\/.]/g, function(match) {
    //   if (match === '/') return '#';
    //   if (match === '.') return '$';
    //   if (match === '%') return '^';
    // });
    //     Backslash: \  NO
    // Hash: #     YES
    // Dollar sign: $  NO
    // Tilde: ~  NO
    // Apostrophe: '  YES
    // Colon: :  YES
    // Asterisk: *  YES
    // Question mark: ?  NO
    // Curly braces: {}  NO
    // Less than greater than<>  YES
    //term.replace(/[&\/\\#,+()$~%.'":*?<>{}]


    let safeDrugName = drugName
      .trim()
      .split('') // Split the string into individual characters
      .map(char => {
        // Check for special characters and letters
        if (char === '/') return '~';
        if (char === '.') return '$';
        if (char === '%') return '@';
        if (char === ':') return '!';
        if (char === '*') return '{';
        // if(char === "'") return '}';
        if (char === ">") return '_';
        if (char === "<") return '}';
        if (char === "+") return '^';
         if (char === "&") return '`';
          

        return char; // Leave any other character unchanged
      })
      .join('');
    console.log(safeDrugName, "safeDrugName")

    let nsId = this.selectednItems.length > 0 ? this.selectednItems[0].NurseStation_Id : 0;
    this.dataservice.get<any[]>(`/CheckInPharmacyMeds/GetEkitLotDetails/${facilityId}/${nsId}/${encodeURIComponent(safeDrugName)}`)
      .subscribe((res: any) => {
        this.ekitLotGrid = res;
        this.getTotalInhand()
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }
  openEkitForm(drug: string, GpiCode: string, nonExpiredQty: string, expiredQty: string, nurseStationId: number) {
    debugger
    console.log(this.ekitObj)
    let drugName = drug;
    this.selectedDrugName = drug;
    this.selectedGpiCode = GpiCode;
    this.totalInhandQty = Number(nonExpiredQty) + Number(expiredQty)
    this.modalekitLotQty = true;
    let selectedDrugs = this.ekitObj.filter(item => item.DrugName == drugName)
    this.selectedDrug = selectedDrugs[0].DrugName;
    this.selectedNursestationid = (nurseStationId == 0 || nurseStationId == null) ? 0 : this.myform.value.ddlnursestations[0].NurseStation_Id;
    console.log(this.selectedDrug)
    setTimeout(() => {
      this.barcodeFocus.nativeElement.focus()
    }, 300);
    this.GetEkitLotDetails(drugName);
    console.log(nonExpiredQty, "nonExpiredQty");
    console.log(this.totalInhandQty, "totalInhandQty");
    console.log(expiredQty, "expiredQty")
  }
  formatDateToLocal(date) {
    var year = date.getFullYear();
    var month = ("0" + (date.getMonth() + 1)).slice(-2);  // Add leading zero to month
    var day = ("0" + date.getDate()).slice(-2);            // Add leading zero to day
    return year + "-" + month + "-" + day;
  }
  InsertLotDetails() {
    var lotNumber = this.textform.value.lotnumber;

    var expiryDate = this.textform1.value.expiryDate;
    // var barcode = this.textform2.value.barcode;
    var InHand = this.textform3.value.InHand;
    let barcode = this.barcodear.join();

    debugger

    if (expiryDate == undefined || expiryDate == "") {
      this.alertService.warn("Expiration date must be entered to proceed Check-in");
    }
    else if (expiryDate != undefined && expiryDate != "" && ((new Date(this.dateFormatPipe.transform(expiryDate)).setHours(0, 0, 0, 0) - new Date().setHours(0, 0, 0, 0)) / (1000 * 60 * 60 * 24)) < 0) {
      this.alertService.warn("Expiration Date entered is in the past, please re-enter");
      // this.alertService.warn("Expiration date cannot be earlier than today’s date");
      if (expiryDate != null && expiryDate != "") {
        var date = this.dateFormatPipe.transformISODate(expiryDate);
        return date;
      }
      // else if (expiryDate != undefined && (expiryDate == null || expiryDate == "")) {
      //   var date = ''
      //   return date 
      // }
    }

    else {
      debugger
      var expiryDateObj = new Date(expiryDate);
      var formattedExpiryDate = expiryDateObj.toISOString().split('T')[0]; // "2024-12-26"

      // Iterate over ekitLotGrid to find a matching entry
      // for (let i = 0; i < this.ekitLotGrid.length; i++) {
      //   var gridLotNumber = this.ekitLotGrid[i].LotNumber;
      //   var gridBarcode = this.ekitLotGrid[i].barcodedetail;
      //   var gridExpiryDate = this.ekitLotGrid[i].ExpiryDate; // Format: "12/26/2024"

      //   var gridExpiryDateObj = new Date(gridExpiryDate);
      //   var formattedGridExpiryDate = this.formatDateToLocal(gridExpiryDateObj); // "2024-12-26"

      //   // Check if all three fields match
      //   if ((lotNumber === gridLotNumber && barcode === gridBarcode && formattedExpiryDate === formattedGridExpiryDate) && this.ekitUpdateId == 0) {
      //     this.alertService.warn("A lot with the same barcode and expiry date exists. Please edit the existing record.");
      //     this.alertFlag = false;
      //     this.textform.reset();
      //     this.textform1.reset();
      //     this.textform2.reset();
      //     this.textform3.reset();
      //     this.barcodear = [];
      //     break;  // Exit loop once a match is found
      //   }
      //   else {
      //     this.alertFlag = true;
      //   }
      // }

      if (this.alertFlag) {
        this.InsertLotEkit = {
          Facility_Id: this.myform.value.ddlfacilities[0].Facility_Id,
          NurseStation_Id: this.selectedNursestationid,
          DrugName: this.selectedDrugName,
          InHand: InHand,
          LotNumber: lotNumber,
          ExpiryDate: expiryDate,
          Ekit_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          Barcode: barcode.replace(/\s/g, ""),
          GpiCode: this.selectedGpiCode,
          EkitUpdate: this.ekitUpdateId,
          EditEkit_id: this.selectedEditEkit_id,
          Reason: this.textform4.value.reason
        }
        console.log(this.InsertLotEkit, "InsertLotEkit");
        this.spinnerLoading++;
        this.checkAndHideSpinnerLoading();
        this.dataservice.post(this.config.Emar_EkitMeds_InsertEkitDetails, this.InsertLotEkit)
          .subscribe(res => {

            this.alertService.success("Save successful");
            this.textform.reset();
            this.textform1.reset();
            this.textform2.reset();
            this.textform3.reset();
            this.textform4.reset();
            this.barcodear = [];
            this.fetchflag = false;
            this.editReasonFlag = false;
            this.GetEkitGridDetails();
            this.ekitUpdateId = 0;
            this.selectedLotNum = 0;
            this.selectedExpDate = '';
            this.GetEkitLotDetails(this.selectedDrugName);
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
  CloseModel() {
    this.modalekitLotQty = false;
    this.expiryModal = false;
    this.ekitLotGrid = [];
    this.barcodear = [];
    this.fetchflag = false;
    this.editReasonFlag = false;
    this.selectedLotNum = 0;
    this.selectedExpDate = '';
    this.textform.reset();
    this.textform1.reset();
    this.textform2.reset();
    this.textform4.reset();
    this.textform3.reset();
    this.GetEkitGridDetails()
    this.destroyFlagKeys = {};
    this.isDestructionModal = false;
    this.resetDestroyGrid()
    this.DestructionOrderSearch = ''
    //this.destroyDesableFlag=true
  }
  addBarcodeLot() {
    debugger
    this.barcodear = [];
    let GpiCode = this.selectedGpiCode == null ? '' : this.selectedGpiCode;
    var barcode = this.textform2.value.barcode;
    let barcodevalue = (barcode.split('/'))[0]
    if (barcode != '' && barcode.length <= 20) {

      // let result = this.barcodesList.filter(x => x.BarcodeDetail1 != null ? x.BarcodeDetail1.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '') : false);
      let result = this.allbarcodesList.filter(x => x.Barcode != null ? x.Barcode.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '') : false);
      console.log(result, "result")
      // let Recods = result.filter(x => x.GPICode == GpiCode && Recods.length == 0);
      let Recods = result.filter(x => x.AGiveCodeIdentifier == GpiCode);


      console.log(Recods, "Recods");
      console.log(this.allbarcodesList, "allbarcodesList");
      let checkInBarcodeArray = this.barcodear.length > 0 ? this.barcodear.find(x => x.replace(/\s/g, '').toLowerCase() === barcodevalue.toLowerCase().replace(/\s/g, '')) : undefined;

      if (result.length > 0 && Recods.length == 0 && result[0].AGiveCodeIdentifier != this.selectedGpiCode) {
        this.alertService.warn("This barcode is assigned to different drug");
        this.textform2.patchValue({
          barcode: '',
        })
      }
      else if (checkInBarcodeArray != undefined) {
        this.alertService.warn("Barcode already assigned to the same medication for the same resident");
        this.textform2.patchValue({
          barcode: '',
        })
      } else {
        this.barcodear.push(barcodevalue);
        console.log(this.barcodear, "barcodear")
        // this.textform2.patchValue({
        //   barcode: '',
        // })
      }
    }
  }

  areFormsValid(): boolean {

    return this.textform.valid && this.textform1.valid && this.barcodear.length && this.textform3.valid;
  }
  removeBarcode(i: number) {

    this.barcodear.splice(i, 1);
    if (this.barcodear.length == 0) {

      const barcodevalidation = this.textform2.get('barcode');
      barcodevalidation.setValidators([Validators.required]);
      barcodevalidation.updateValueAndValidity();
      this.alertService.warn("Minimum one barcode is required");

    }

  }
  getTotalInhand() {
    debugger
    // this.totalQtyval = this.ekitLotGrid.reduce((sum, item) => sum + Number(item.Inhand), 0);
    //this.totalQtyval = this.ekitLotGrid.reduce((sum, item) => this.isExpired(item.ExpiryDate) ? sum : sum + Number(parseFloat(item.Inhand.replace(",", "."))), 0);
    this.totalQtyval = this.ekitLotGrid.reduce(
      (sum, item) => sum + Number(parseFloat(item.Inhand.replace(",", "."))),
      0
    );
    console.log(this.totalQtyval, " this.totalQtyval")
  }
  LotNumberDetailsCheck() {
    debugger;
    let lotNumber = this.textform.value.lotnumber;
    let barCode = this.textform2.value.barcode
    if (lotNumber.length) {
      console.log(this.textform.value.lotnumber, "this.textform.value.lotnumber")
      let DuplicateCheck = this.ekitLotGrid.filter(x => x.LotNumber == lotNumber && x.barcodedetail == barCode);
      //ExpiryDate
      console.log(DuplicateCheck, "DuplicateCheck");
      if (DuplicateCheck.length) {
        // this.alertService.warn("Duplicate Lot number");
        // this.textform.reset();
        return DuplicateCheck;
      } else {
        return 0;
      }

    }
  }
  isExpired(expiryDate: string): boolean {
    //this.nextDate=this.dateFormatPipe.transformISODate(nextDate);
    const currentDate = new Date();
    this.nextDate = this.dateFormatPipe.transformISODate(currentDate);
    const expiry = new Date(expiryDate);
    let expirDate = this.dateFormatPipe.transformISODate(expiry);
    return expirDate <= this.nextDate;
  }
  OnDestroyExp() {
    debugger
    const Obj = Array.from(this.selectedExpLot).join(', ');
    const currentDate = new Date();
    console.log(Obj, "Obj");
    let expReason = this.myExpform.value.destroy
    let expiryIds = {
      // Ekit_Ids:Obj,
      Ekit_Ids: this.updateEkitId,
      ExpiredReason: expReason,
      UpdatedBy: this.userId,
      QtyonHand: String(this.totalInhandQty),
      QtyDestroyed: String(this.updateInhandQty),
      DestroyDate: this.dateFormatPipe.dateWithTime(currentDate)
      //destroyedby
    }
    if (this.updateEkitId > 0) {
      this.spinnerLoading++;
      this.checkAndHideSpinnerLoading();
      this.dataservice.post(this.config.Emar_EkitMeds_UpdateExpEkitIDs, expiryIds)
        .subscribe(res => {

          if (res == 1) {
            this.alertService.success("Destroyed successful");
            this.myExpform.reset();
            this.GetEkitLotDetails(this.selectedDrugName);
            this.GetEkitGridDetails();
            this.destroyFlagKeys = {};
            this.expiryModal = false;
            this.destroyQty = 0;
            this.updateInhandQty = 0;
          }
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        },
          error => {
            this.alertService.error(error.message);
            this.spinnerLoading--;
            this.checkAndHideSpinnerLoading();

          });
    }

  }
  isSelected(ekitId: number): boolean {
    return this.selectedExpLot.has(ekitId);
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.selectedExpLot.add(item.Ekit_Id);
      this.destroyQty += Number(item.Inhand)
    } else {
      this.selectedExpLot.delete(item.Ekit_Id);
      this.destroyQty -= Number(item.Inhand)

      // const index = this.selectedExpLot.findIndex(i => i.Ekit_Id == item.Ekit_Id);
      // this.selectedExpLot.splice(index, 1);

    }
    console.log(item, "item")
    console.log(this.selectedExpLot, "selectedExpLot")
  }
  OnDestroyModal() {
    this.expiryModal = true;
    setTimeout(() => {
      this.destroyFocus.nativeElement.focus()
    }, 300);

  }

  UnionBarcodeDetails() {
    debugger
    console.log(this.loginUserReceFacility)
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any[]>(this.config.Emar_EkitMeds_UnionBarcodeDetails + this.loginUserReceFacility)
      .subscribe((res: any) => {

        this.allbarcodesList = res;
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      }, error => {
        this.alertService.error(error.message);
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();

      });
  }
  CancelEkitDetails() {
    this.ekitUpdateId = 0;
    this.textform.reset();
    this.textform1.reset();
    this.textform2.reset();
    this.textform3.reset();
    this.textform4.reset();
    this.barcodear = [];
    this.fetchflag = false;
    this.editReasonFlag = false;
    this.selectedLotNum = 0;
    this.selectedExpDate = '';
    this.selectedEditEkit_id = 0;

  }
  getEkitDetailsById(ekitId: number) {
    debugger;
    this.ekitUpdateId = 1;
    this.selectedEditEkit_id = 0;
    this.selectedEditEkit_id = ekitId;
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.get<any>(this.config.Emar_EkitMeds_GetEkitID_Edit + ekitId).subscribe(res => {
      this.ekitFetchItems = res
      this.fetchEkitGrid(res);
      this.spinnerLoading--;
      this.checkAndHideSpinnerLoading();
    })

  }
  fetchEkitGrid(ekitDetails: any) {
    debugger;
    this.fetchflag = true;
    this.editReasonFlag = true;
    console.log(ekitDetails, "ekitDetails")
    if (ekitDetails[0].Barcode != null && ekitDetails[0].Barcode != '') {
      let list: string = ekitDetails[0].Barcode;
      this.barcodear = list.split(', ');
      const barcodevalidation = this.textform2.get('barcode');
      //barcodevalidation.setValidators(null);
      barcodevalidation.setValidators([Validators.maxLength(150)]);
      barcodevalidation.updateValueAndValidity();
      this.barcodear = [];
      this.barcodear.push(ekitDetails[0].Barcode == null ? '' : ekitDetails[0].Barcode);

      this.textform2.patchValue({
        barcode: (ekitDetails[0].Barcode == null ? '' : ekitDetails[0].Barcode)
      });

    }
    else {
      this.barcodear = [];
    }

    this.textform.patchValue({
      lotnumber: ekitDetails[0].LotNumber
    });
    // const lotvalidation = this.textform.get('LotNumber');

    // lotvalidation.setValidators(Validators.required);
    // lotvalidation.updateValueAndValidity();
    this.selectedLotNum = ekitDetails[0].LotNumber == null ? 0 : ekitDetails[0].LotNumber;
    this.textform1.patchValue({
      expiryDate: (ekitDetails[0].ExpiryDate == null ? '' : ekitDetails[0].ExpiryDate.substring(0, 10))
    });
    // const expValidation = this.textform1.get('expiryDate');
    // expValidation.setValidators(Validators.required);
    // expValidation.updateValueAndValidity();
    this.selectedExpDate = ekitDetails[0].ExpiryDate == null ? '' : ekitDetails[0].ExpiryDate.substring(0, 10);
    this.textform3.patchValue({
      InHand: ekitDetails[0].Inhand
    });

    // const inhandValidations = this.textform3.get('InHand');
    // inhandValidations.setValidators(Validators.required);
    // inhandValidations.updateValueAndValidity();

  }
  destroyQtyonHand(event: Event, item: any) {
    debugger
    console.log(event, "event");
    console.log(item, "selected item");
    let inputQty = Number((event.target as HTMLInputElement).value);
    let inhandQty = parseFloat(item.Inhand.replace(",", "."));
    this.updateInhandQty = inputQty;
    this.updateEkitId = item.Ekit_Id
    if (inputQty > 0 && inputQty <= inhandQty) {
      this.destroyFlagKeys[item.Ekit_Id] = 1;
      this.enableDestroy = true;
    } else {
      this.enableDestroy = false;
      this.destroyFlagKeys[item.Ekit_Id] = 0;
      this.updateInhandQty = 0;

    }
  }
  expDateChanged() {
    debugger;



    let existingLot = this.LotNumberDetailsCheck();
    let expiryDate = existingLot != 0 ? existingLot[0].ExpiryDate.substring(0, 10) : 0;
    if ((this.selectedExpDate != this.textform1.value.expiryDate && this.selectedLotNum == this.textform.value.lotnumber && this.ekitUpdateId == 0) || ((expiryDate != 0 && expiryDate != this.textform1.value.expiryDate || existingLot[0].barcodedetail != this.textform2.value.barcode) && this.ekitUpdateId == 0)

    ) {
      let formattedExpiryDate = this.datePipe.transform(existingLot[0].ExpiryDate, 'MM-dd-yyyy');
      this.alertService.warn(`This lot number has already been received with expiration date ${formattedExpiryDate}. Please edit existing record`);
      //  this.alertService.warn(`this lot number has been already received with expiration date${existingLot[0].ExpiryDate}`);
      this.textform1.reset();
    }
  }
  checkalert() {
    debugger
    var expiryDate = this.textform1.value.expiryDate;
    var lotNumber = this.textform.value.lotnumber;
    let barcode = this.barcodear.join();
    var expiryDateObj = new Date(expiryDate);
    var formattedExpiryDate = expiryDateObj.toISOString().split('T')[0]; // "2024-12-26"
    //   for (let i = 0; i < this.ekitLotGrid.length; i++) {
    //     var gridLotNumber = this.ekitLotGrid[i].LotNumber;
    //     var gridBarcode = this.ekitLotGrid[i].barcodedetail;
    //     var gridExpiryDate = this.ekitLotGrid[i].ExpiryDate; // Format: "12/26/2024"

    //     var gridExpiryDateObj = new Date(gridExpiryDate);
    //     var formattedGridExpiryDate = this.formatDateToLocal(gridExpiryDateObj); // "2024-12-26"

    //     // Check if all three fields match
    //     if((lotNumber === gridLotNumber && barcode === gridBarcode)&&this.ekitUpdateId==0){
    //       debugger
    //       console.log('went inside') 
    //        const newdate =gridExpiryDateObj.toISOString().split('T')[0];
    //       this.textform1.patchValue({
    //         expiryDate:newdate
    //       })
    //         break; 
    //     }
    //     // if ((lotNumber === gridLotNumber && barcode === gridBarcode && formattedExpiryDate === formattedGridExpiryDate)&&this.ekitUpdateId==0) {
    //     //     this.alertService.warn("A lot with the same barcode and expiry date exists.");
    //     //     this.alertFlag=false;
    //     //     this.textform.reset();
    //     //      this.textform1.reset();
    //     //     this.textform2.reset();
    //     //      this.textform3.reset();
    //     //     this.barcodear = [];
    //     //     break;  // Exit loop once a match is found
    //     // }
    //     // else{
    //     //   this.alertFlag=true;
    //     // }
    // }
    let newvalue = this.ekitLotGrid.map(item => (item.LotNumber == lotNumber && item.barcodedetail == barcode))
  }
  CheckbarcodeLot() {

  }
  checkAndHideSpinnerLoading() {

    if (this.spinnerLoading === 0 || this.spinnerLoading < 0) {
      this.ng4LoadingSpinnerService.hide();
      console.log(this.spinnerLoading, "this.spinnerLoading++")
    } else {
      this.ng4LoadingSpinnerService.show();
      //console.log(this.spinnerLoading,"this.--")

    }
  }
  replaceCommaWithPeriod(value: any): string {
    if (value == null || value === '') {
      return '0';
    }
    return value.toString().replace(/,/g, '.'); // Replace all commas with periods
  }
  // ngAfterViewInit(){
  //   debugger
  //   const editEkit = this.el.nativeElement.querySelector('#parentTd');
  //   if(editEkit){
  //     this.renderer.listen(editEkit,'click',(event)=>{
  //       const target = event.target as HTMLElement;

  //       // Extract the ID from the clicked element
  //       if (target && target.tagName === 'SPAN' && target.hasAttribute('data-id')) {
  //         const ekitId = target.getAttribute('data-id');
  //         this.getEkitDetailsById(+ekitId);
  //       }
  //      })
  //   }
  // }
  ngAfterViewInit() {
    this.cd.detectChanges();
  }
  openExpDestructionModal() {
    this.GetExpiredEkitMedDestruction()
    this.isDestructionModal = true;
    this.selectedRecords = []
    this.isAllChecked = false
    this.sharedService.saveChangesOrderInfo(0);
  }

  GetExpiredEkitMedDestruction() {
    let nsId = this.selectednItems.length > 0 ? this.selectednItems[0].NurseStation_Id : 0;
    debugger
    this.dataservice.get<any>(this.config.Emar_EkitMeds_GetExpiredEkitMedDestruction + parseInt(this.selectedfaItems[0].Facility_Id) + '/' + parseInt(nsId))
      .subscribe(res => {
        this.initialDestroyGrid = JSON.parse(JSON.stringify(res));
        console.log(this.initialDestroyGrid)
        this.destroyGrid = res.map(item => ({
          ...item,
          checkboxChecked: false, // Add checkbox state 
          inputValue: '',
          validationError: '', // Validation error  
          reasonValidationError: '', // Validation error for  reason
        }));
      });

    console.log(this.destroyGrid);
  }

  resetDestroyGrid(): void {
    this.destroyGrid = this.destroyGrid.map(item => ({
      ...item,
      checkboxChecked: false, // Reset checkbox state
      inputValue: null,       // Reset input field to null
    }));
  }

  onCheckAll(event) {

    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.destroyGrid.forEach(element => {

        //element.Physician_Status=1;
        this.selectedRecords.push(element);
        // this.destroyItems.push(this.)
      });
    }
    else {
      this.CheckAll = false;
      this.UpdateStatus = true;
      this.selectedRecords = [];
    }
  }
  orderDestroyDetailsCancel() {
    this.destroyform.reset();
    this.modalDesIsOpen = false;
  }
  isDestroyEnabled = false;
  isAllChecked = false;

  validateInputValue(item: any, index: number, field: string) {
    debugger;

    // Validate the "Quantity to Destroy" field
    if (field == 'updatedQtyInput') {
      // If the checkbox is checked and input value is empty, show validation error
      if (item.checkboxChecked && (!item.inputValue || item.inputValue.trim() === '')) {
        item.validationError = 'Quantity is Required';
      }

      else if ( Number(item.inputValue >= 0) && item.inputValue.trim() !== '') {
        item.checkboxChecked = true
        item.validationError = '';
        if (item.checkboxChecked && (!item.reason || item.reason.trim() == '')) {
          item.reasonValidationError = 'Reason is Required';
        }
      }
      // If the input value is negative
      else if (+item.inputValue < 0) {
        item.validationError = 'It should be greater than 0.';
      }
      // If the value is not a valid number
      else if (!Number(item.inputValue >= 0) && item.inputValue !== '') {
        item.validationError = 'Invalid Quantity';
      }
      else if (item.checkboxChecked == false && (!item.inputValue || item.inputValue.trim() === '')) {
        item.validationError = '';
      }
      else {
        item.validationError = ''; // Clear validation error if valid
      }
    }

    // Validate the "Quantity Not Expired to Destroy" field
    if (field == 'updatedQtyReason') {
      // If the checkbox is checked and input value is empty, show validation error
      if (item.checkboxChecked && (!item.reason || item.reason.trim() == '')) {
        item.reasonValidationError = 'Reason is Required';
      }
      else {
        item.reasonValidationError = ''; // Clear validation error if valid
      }
    }
    // Recalculate the "Destroy" button status
    this.checkDestroyButtonStatus();
  }

  // Check All functionality
  toggleCheckAll(event: any) {
    // debugger
    this.isAllChecked = event.target.checked;
    this.destroyGrid.forEach((item) => {
      item.checkboxChecked = event.target.checked;
      this.validateInputValue(item, null, "updatedQtyInput")
      this.validateInputValue(item, null, "updatedQtyReason")
    });
    this.checkDestroyButtonStatus();
  }


  signatureForDestroy() {
    this.modalDesIsOpen = true
  }
  updateDestroyStatus() {
    debugger
    this.selectedItems = this.destroyGrid.filter((item) => item.checkboxChecked);
    console.log('Selected items for destruction:', this.selectedItems);
    this.orderDestroyObj = this.selectedItems.map((item, index) => ({
      DUserName: this.destroyform.value.destroyerUsername,
      DPassword: this.destroyform.value.destroyerPass,
      AUserName: this.destroyform.value.approvalUsername,
      APassword: this.destroyform.value.approvalPass,
      Ekit_CreatedBy: this.userId,
      Ekit_CreatedOn: this.dateFormatPipe.dateWithTime(new Date()),
      Ekit_Id: item.Ekit_Id,
      InHandqty: item.inputValue,
      ApprovalUserId: 0,
      DestroyerUserId: 0,
      LoggedInUserId: this.userId,
      Reason: item.reason
    }));
    console.log(this.orderDestroyObj)
    this.spinnerLoading++;
    this.checkAndHideSpinnerLoading();
    this.dataservice.post(this.config.Emar_EkitMeds_EkitDestroyQuantity, this.orderDestroyObj)
      .subscribe(res => {
        if (res == "Done") {
          this.alertService.success("Updated Successfully");
          this.GetEkitGridDetails()
          this.orderDestroyDetailsCancel()
          this.GetExpiredEkitMedDestruction()
        }
        else {
          this.alertService.error(res);
        }
        console.log(res)
        this.spinnerLoading--;
        this.checkAndHideSpinnerLoading();
      },
        error => {
          this.alertService.error(error.message);
          this.spinnerLoading--;
          this.checkAndHideSpinnerLoading();
        });

  }
  onValueChanges(): void {
    const barcodeControl = this.textform2.get('barcode');
    const lotControl = this.textform.get('lotnumber');

    // Check if the controls exist before subscribing to their valueChanges
    if (barcodeControl) {
      barcodeControl.valueChanges.subscribe(() => {
        this.onBarcodeLotChange();
      });
    }

    if (lotControl) {
      lotControl.valueChanges.subscribe(() => {
        this.onBarcodeLotChange();
      });
    }
  }

  onBarcodeLotChange(): void {
    debugger
    const barcodeControl = this.textform2.get('barcode');
    const lotControl = this.textform.get('lotnumber');

    // Check if both form controls exist
    let barcode = '';
    let lot = '';

    if (barcodeControl) {
      barcode = barcodeControl.value;
    }

    if (lotControl) {
      lot = lotControl.value;
    }

    if (barcode && lot) {
      this.fetchDate(barcode, lot);
    }
  }

  fetchDate(barcode, lot) {
    debugger
    const matchedRecord = this.ekitLotGrid.find(item =>  item.LotNumber === lot);
    if (matchedRecord) {
      var gridExpiryDateObj = new Date(matchedRecord.ExpiryDate);
      const newdate = gridExpiryDateObj.toISOString().split('T')[0];
      // this.textform1.patchValue({
      //   expiryDate: newdate
      // })
      this.textform1.patchValue({
        expiryDate: (matchedRecord.ExpiryDate == null ? '' : matchedRecord.ExpiryDate.substring(0, 10))
      });
      this.lastExpiryDate = matchedRecord.ExpiryDate
      this.newRecordFlag=false
    }
    else {
      this.lastExpiryDate = '';
      this.newRecordFlag=true
    }
  }
  onDateChange(): void {
    debugger
    const inputExpiryDate = this.textform1.get('expiryDate');
    let currentExpiryDate = null;
    if (inputExpiryDate) {
      if(inputExpiryDate.value==this.today && this.newRecordFlag){
        const modalRef = this.modalService.open(TodaysdatealertComponent, { size: 'lg', windowClass: '' ,
           backdrop: 'static',  // Prevent modal from closing when clicking outside
          keyboard: false       // Prevent modal from closing on ESC key press
           });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          debugger
          this.textform1.patchValue({
            expiryDate: '',
          });
          modalRef.close();
        }
        else if (receivedResult == 1) {
          currentExpiryDate = inputExpiryDate.value;
          this.alertFlag = true;
        }
        modalRef.close();
      });
      }
      else{
      currentExpiryDate = inputExpiryDate.value;
      this.alertFlag = true;
      }
    }
    if (currentExpiryDate !== this.lastExpiryDate && this.lastExpiryDate != '') {
      this.alertService.warn("Same lot exist with different expiration date.");
      //this.alertFlag = false;
      this.lastExpiryDate = '';

    }
    else {
      this.alertFlag = true;
    }

  }

  onCheckboxChange(index: number, event: any, item) {
    debugger;
    const gridItem = this.destroyGrid.find((gridItem) => gridItem.Ekit_Id === item.Ekit_Id);
    if (gridItem) {
      gridItem.checkboxChecked = event.target.checked;

      // Validate the correct field when the checkbox is checked
      if (gridItem.checkboxChecked) {
        if (gridItem.inputValue || gridItem.inputValue.trim() !== '' || gridItem.inputValue == '') {
          this.validateInputValue(gridItem, index, 'updatedQtyInput');
        }
        if (gridItem.reason == '') {
          this.validateInputValue(gridItem, index, 'updatedQtyReason');
        }
      } else {
        // If unchecked, clear the validation error
        gridItem.validationError = '';
        gridItem.reasonValidationError = '';
      }
    }

    // Recalculate the "Destroy" button status
    this.checkDestroyButtonStatus();
  }

  checkDestroyButtonStatus() {
    debugger;

    // Check if any checked item has a validation error
    const hasValidationError = this.destroyGrid.some((item) => item.checkboxChecked && (item.validationError || item.reasonValidationError));

    // Check if there are any checked items and they have valid input (i.e., no validation errors)
    const hasCheckedItems = this.destroyGrid.some(
      (item) => item.checkboxChecked && !item.validationError && !item.reasonValidationError
    );

    // Enable the Destroy button if there are checked items and no validation errors
    this.isDestroyEnabled = hasCheckedItems && !hasValidationError;

    // Update the "Check All" state: all checked if all items are checked
    this.isAllChecked = this.destroyGrid.every((item) => item.checkboxChecked);
  }

  isDataModified(): boolean {
    debugger
    // Compare each item in the grids and check if any value has changed
    for (let i = 0; i < this.destroyGrid.length; i++) {
      const currentItem = this.destroyGrid[i];
      const initialItem = this.initialDestroyGrid[i];
      // Compare input values, checkbox state, and other modified fields
      if (
        currentItem.inputValue !== '' ||
        currentItem.checkboxChecked == true ||
        currentItem.reason !== initialItem.reason
      ) {
        this.sharedService.saveChangesOrderInfo(1);
        return
      }
    }
    this.sharedService.saveChangesOrderInfo(0);
    return
  }
  closeDestruction() {
    debugger
    // Check if there are any changes between destroyGrid and initialDestroyGrid

    this.isDataModified()
    if (this.valueChangesFlagReceive == 1) {
      const modalRef = this.modalService.open(EkitmedsqtyupdatesdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.isDestructionModal = false;
          this.resetDestroyGrid()
          this.DestructionOrderSearch = ''
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlagReceive = 0;
        }
        modalRef.close();
      });

    }
    else {
      // Close the modal if no changes
      this.isDestructionModal = false;
      this.resetDestroyGrid()
      this.DestructionOrderSearch = ''
    }
  }
}
