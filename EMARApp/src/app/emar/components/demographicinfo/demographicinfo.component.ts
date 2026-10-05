import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Gender, MaritalStatus, Suffix } from '../../../models/common.model';
import { DemographicInfo, ResidentDemographicMaster, AdmitvisitInfo, NewResidentInfo, VisitInfoCuston, DemographicCustomInfo } from '../../../models/residentdemographic.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SharedService } from '../../../services/shared/shared.service';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ResidentreactivateComponent } from '../residentreactivate/residentreactivate.component';
import { NurseStation, Room, Facility, Bed, Floor, Wing } from '../../../models/facility.model';
import { getLocaleDateTimeFormat } from '@angular/common';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';

@Component({
  selector: 'app-demographicinfo',
  templateUrl: './demographicinfo.component.html',
  styleUrls: ['./demographicinfo.component.css'],
  providers: [DataService, APIConfiguration]
})
export class DemographicinfoComponent implements OnInit {
  public demographicInfoData = {} as DemographicInfo;
  public ResidentdemographicData: ResidentDemographicMaster[];
  private demographicsObj: DemographicCustomInfo;
  public genders: Gender[];
  public template;
  myform: FormGroup;
  censusform: FormGroup;
  pageConfig = {};
  public errorMessage: any;
  public MiddileIntialPattern = '^[a-zA-Z0-9-_` \x27]*$';
  public nurseStation: NurseStation[];
  public nurseStations: NurseStation[];
  public rooms: Room[] = [];
  dropdownSettings_Facilities: any = {};
  dropdownSettings_NurseStations: any = {};
  public userfacilitydrop: Facility[];
  public facilityDrop: Facility[];
  public room: Room[];
  public bed: Bed[];
  public floors: Floor[];
  public floorMaster: any[];
  public beds: Bed[];
  public wings: Wing[];
  public admitvisitinfoObj: VisitInfoCuston;
  public newResident: NewResidentInfo;
  public Mrnumber: any;
  public selectedNurseStationId: number;
  public selectedFacilityId: number;
  public selectedfaItems = [];
  public selectednItems = [];
  public residentUniqueId: string = "";
  public generatedMrNumber: string = "";
  public generatedExPatientId: string = "";
  public today: any;
  ShowFilter = true;
  public companyToBed: number = 0;
  public comToBedApi:any;
  public nstations: string = "";
  @Input() resdata: any;
  @Output() Result: EventEmitter<any> = new EventEmitter();
  public physiciansdrop:any[]=[];
  public defaultPhysicianNPI: any;
  public selectedphyItems: any[];
  public dropdownSettings_Physician={};
  public Hlsevenconfig: any[]; 
  displayfield: any = {};
  public AdmitvisitInfoHlsevenconfig: any[]; 
  AdmitvisitInfodisplayfield: any = {};
  public savedisable=false;
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  public form: FormGroup;
  public fields: any[];
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  @ViewChild('firstNameFocus') firstNameFocus:ElementRef

  constructor(private dataservice: DataService, public config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private dateFormatPipe: CustomdatePipe, private alertService: AlertService,
    private sharedService: SharedService, private persistanceService: PersistanceService, private modalService: NgbModal, public activeDefaultModal: NgbActiveModal) {
  }
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("NewResident");
    this.selectedNurseStationId = this.resdata.selectedNurseStationId;
    this.selectedFacilityId = this.resdata.selectedFacilityId;
    this.getUserFacilityDrop();
    //this.getResidentuniqueId(this.selectedFacilityId);
    this.myform = new FormGroup({
      // SetID: new FormControl('', Validators.required),
      ExternalPatientId: new FormControl('', [Validators.maxLength(50)]),
      ExternalFacShortName: new FormControl('', [Validators.maxLength(50)]),
      ExternalFacPatientId: new FormControl('', [Validators.pattern(this.config.numeric), Validators.maxLength(50)]),
      AlternatePatientId: new FormControl('', [Validators.pattern(this.config.numeric), Validators.maxLength(50)]),
      PatientLastName: new FormControl('', [Validators.pattern(this.config.alphaNumericFewSpecialCharacters3), Validators.maxLength(50),Validators.minLength(2)]),
      PatientFirstName: new FormControl('', [Validators.required, Validators.pattern(this.config.alphaNumericFewSpecialCharacters3), Validators.maxLength(50)]),
      PatientMiddleInitial: new FormControl('', [Validators.pattern(this.MiddileIntialPattern), Validators.maxLength(1)]),
      NameTypeCode: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(1)]),
      MotherMaidenName: new FormControl('', [Validators.maxLength(100)]),
      DOB: new FormControl('', Validators.required),
      AdministrativeSex: new FormControl('',Validators.required),
      PatientAlias: new FormControl('', [Validators.maxLength(100)]),
      Race: new FormControl('', [Validators.maxLength(100)]),
      PatientAddress1: new FormControl('', [Validators.maxLength(100)]),
      PatientAddress2: new FormControl('', [Validators.maxLength(100)]),
      PatientCity: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(50)]),
      PatientState: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(50)]),
      PatientZipCode: new FormControl('', [Validators.maxLength(11)]),
      CountyCode: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(10)]),
      PhoneHome: new FormControl('', [Validators.minLength(13)]),
      PhoneBusiness: new FormControl('', [Validators.minLength(13)]),
      PrimaryLanguage: new FormControl('', [Validators.maxLength(50)]),
      MaritalStatus: new FormControl('', [Validators.maxLength(50)]),
      Religion: new FormControl('', [Validators.maxLength(50)]),
      PatientMRNumber: new FormControl('', [Validators.pattern(this.config.alphaNumeric), Validators.minLength(5)]),
      SSN: new FormControl('', [Validators.maxLength(11)]),
      DriverLicense: new FormControl('', [Validators.maxLength(50)]),
      MotherIdentifier: new FormControl('', [Validators.maxLength(100)]),
      EthnicGroup: new FormControl('', [Validators.maxLength(50)]),
      BirthPlace: new FormControl('', [Validators.maxLength(50)]),
      MultipleBirthIndicator: new FormControl('', [Validators.maxLength(50)]),
      BirthOrder: new FormControl('', [Validators.maxLength(50)]),
      Citizenship: new FormControl('', [Validators.maxLength(50)]),
      MilitaryStatus: new FormControl('', [Validators.maxLength(50)]),
      Nationality: new FormControl('', [Validators.maxLength(50)]),
      DeathDateTime: new FormControl(''),
      DeathIndicator: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(1)]),
      IdentityIndicator: new FormControl('', [Validators.maxLength(1)]),
      IdentityReliability: new FormControl('', [Validators.maxLength(1)]),
      LastUpdate: new FormControl('', [Validators.maxLength(25)]),
      LastFacilityUpdate: new FormControl('', [Validators.maxLength(25)]),
      SpeciesCode: new FormControl('', [Validators.maxLength(50)]),
      BreedCode: new FormControl('', [Validators.maxLength(50)]),
      Strain: new FormControl('', [Validators.maxLength(50)]),
      ProductionClassCode: new FormControl('', [Validators.maxLength(50)]),
      TribalCitizenship: new FormControl('', [Validators.maxLength(50)]),
    });
    this.censusform = new FormGroup({
      PVisit_Id: new FormControl(''),
      Patient_Id: new FormControl(''),
      PatientClass: new FormControl('', Validators.maxLength(1)),
      NursingStationId: new FormControl(this.selectedNurseStationId,Validators.required),
      //Room: new FormControl(''),
      //Bed: new FormControl(''),
      FacilityId: new FormControl(this.selectedFacilityId,Validators.required),
      //Floor: new FormControl(''),
      //Wing: new FormControl(''),
      AdmissionType: new FormControl('', Validators.maxLength(2)),
      PreAdmitNumber: new FormControl('', Validators.maxLength(150)),
      PriorNursingStationId: new FormControl(''),
      PriorRoom: new FormControl(''),
      PriorBed: new FormControl(''),
      PriorFacilityId: new FormControl(''),
      PriorFloor: new FormControl(''),
      PrimaryPhysicianNPI: new FormControl('', Validators.maxLength(10)),
      PrimaryPhysicianLName: new FormControl('', Validators.maxLength(75)),
      PrimaryPhysicianFName: new FormControl('', Validators.maxLength(75)),
      ReferringDoctor: new FormControl('', Validators.maxLength(150)),
      ConsultingDoctor: new FormControl('', Validators.maxLength(150)),
      HospitalService: new FormControl('', Validators.maxLength(50)),
      TemporaryLocation: new FormControl('', Validators.maxLength(80)),
      PreAdmitTestIndicator: new FormControl('', Validators.maxLength(50)),
      ReAdmissionIndicator: new FormControl('', [Validators.pattern(this.config.alphabets), , Validators.maxLength(5)]),
      AdmitSource: new FormControl('', Validators.maxLength(6)),
      AmbulatoryStatus: new FormControl('', Validators.maxLength(2)),
      VIPIndicator: new FormControl('', Validators.maxLength(2)),
      AdmittingDoctor: new FormControl('', Validators.maxLength(150)),
      PatientType: new FormControl('', Validators.maxLength(2)),
      VisitNumber: new FormControl('', Validators.maxLength(150)),
      FinancialClass: new FormControl('', Validators.maxLength(50)),
      ChargePriceIndicator: new FormControl('', Validators.maxLength(2)),
      CourtesyCode: new FormControl('', Validators.maxLength(2)),
      CreditRating: new FormControl('', Validators.maxLength(2)),
      ContractCode: new FormControl('', Validators.maxLength(2)),
      ContractEffDate: new FormControl(''),
      ContractAmount: new FormControl('', Validators.maxLength(20)),
      ContractPeriod: new FormControl('', Validators.maxLength(20)),
      InterestCode: new FormControl('', Validators.maxLength(2)),
      BadDebtCode: new FormControl('', Validators.maxLength(2)),
      BadDebtDate: new FormControl(''),
      BadDebtAgencyCode: new FormControl('', Validators.maxLength(10)),
      BadDebtTransferAmt: new FormControl('', Validators.maxLength(20)),
      BadDebtRecoveryAmt: new FormControl('', Validators.maxLength(20)),
      DeleteAccIndicator: new FormControl(''),
      DeleteAccDate: new FormControl(''),
      DischargeDisposition: new FormControl('', Validators.maxLength(3)),
      DischargedLocation: new FormControl('', Validators.maxLength(47)),
      DietType: new FormControl('', Validators.maxLength(150)),
      ServicingFacility: new FormControl('', Validators.maxLength(2)),
      BedStatus: new FormControl('', Validators.maxLength(1)),
      AccStatus: new FormControl('', Validators.maxLength(5)),
      PendingLocation: new FormControl('', Validators.maxLength(80)),
      PriorTemporaryLocation: new FormControl('', Validators.maxLength(80)),
      AdmitDate: new FormControl('', Validators.required),
      DischargeDate: new FormControl(''),
      CurrentPatientBalance: new FormControl('', Validators.maxLength(20)),
      TotalCharges: new FormControl('', Validators.maxLength(20)),
      TotalAdjustments: new FormControl('', Validators.maxLength(20)),
      TotalPayments: new FormControl('', Validators.maxLength(20)),
      AlternateVisitId: new FormControl('', Validators.maxLength(150)),
      VisitIndicator: new FormControl('', Validators.maxLength(5)),
      OtherHealthProvider: new FormControl('', Validators.maxLength(150)),
      physicianname: new FormControl('', [Validators.required]),
    });
    this.form = new FormGroup({
      fields: new FormControl(JSON.stringify(this.fields))
    });
    this.dropdownSettings_Facilities = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Facilities",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Physician = {
      singleSelection: true,
      idField: "PhysicianNPI",
      textField: "PhysicianFullName",
      text: "Select",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Nursing Station',
      allowSearchFilter: true,
    };
    this.dropdownSettings_NurseStations = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Nursing Stations",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: this.ShowFilter
    };
    // this.getFloorWingDrop(this.selectedFacilityId);
    this.getGenders();
    //this.getUserNurseStations(this.selectedFacilityId);
    this.getFacilityDrop();
    this.getHlsevenconfigData();
    this.getHlsevenconfigforAdmitVisitInfoData();
    // this.getRoomDetails();
    // this.getFloorDropData();
    // this.getBeds();
    //this.getAllNurseStations();
    // this.getWings();
  }
  ngAfterViewInit(){
    setTimeout(() => {
    this.firstNameFocus.nativeElement.focus()
    }, 300);
  }
  getToday(): string {
    return new Date().toISOString().split('T')[0]
  }
  getGenders() {
    this.dataservice.get<Gender[]>(this.config.Common_GetGenders)
      .subscribe(res => this.genders = res, error => {
        this.alertService.error(error.message)
      });
  }
  getHlsevenconfigData() {
    this.dataservice.get<any[]>(this.config.Emar_HlSevenConfigs_GetOutboundHLSevenConfigs + 0 + "/" + 4 +"/" + this.selectedFacilityId)
      .subscribe(res => {
        this.Hlsevenconfig = res;
        for (let index = 0; index < this.Hlsevenconfig.length; index++) {
          this.displayfield[this.Hlsevenconfig[index].OSegDetail_Desc] = this.Hlsevenconfig[index].DisplayConfigId;
      }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getHlsevenconfigforAdmitVisitInfoData() {
    this.dataservice.get<any[]>(this.config.Emar_HlSevenConfigs_GetOutboundHLSevenConfigs + 0 + "/" + 5 +"/" + this.selectedFacilityId)
      .subscribe(res => {
        this.AdmitvisitInfoHlsevenconfig = res;
        for (let index = 0; index < this.AdmitvisitInfoHlsevenconfig.length; index++) {
          this.AdmitvisitInfodisplayfield[this.AdmitvisitInfoHlsevenconfig[index].OSegDetail_Desc] = this.AdmitvisitInfoHlsevenconfig[index].DisplayConfigId;
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
        // this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  getUserNurseStations(facilityId: any)
  {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.selectedfaItems.push(this.userfacilitydrop.filter(f => f.Facility_Id ===this.selectedFacilityId)[0]);
        this.selectednItems.push(this.nurseStations.filter(n=>n.NurseStation_Id===this.selectedNurseStationId)[0]);
        this.censusform.patchValue({
        FacilityId:this.selectedfaItems,
        NursingStationId:this.selectednItems
    });
    this.getFloorWingDrop();
    this.getDefaultPhysicianByNSId();
        // this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  getWings() {
    this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetAllActiveWingNames)
      .subscribe(res => {
        //this.wings = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });

  }
  getFacilityDrop() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserFacilitiesDrop + userId)
      .subscribe(res => {
        this.facilityDrop = res;
      },
        error => {
          this.alertService.error(error.message);
        });

  }
  getUserFacilityDrop() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserFacilitiesDrop + userId)
      .subscribe(res => {
        this.userfacilitydrop = res;
        this.getUserNurseStations(this.selectedFacilityId);
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  getRoomDetails() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllRooms)
      .subscribe(res => {
        this.rooms = res;
      },
        error => {
          this.alertService.error(error.message);
        }
      );
  }
  getFloorDropData() {
    this.dataservice.get<Floor[]>(this.config.Emar_Facility_GetAllFloors)
      .subscribe(res => this.floorMaster = res,
        error => {
          this.alertService.error(error.message);
        }
      );
  }
  getBeds() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllBeds)
      .subscribe(res => {
        this.beds = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getAllNurseStations() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllNurseStationsNew + 1)
      .subscribe(res => {
        this.nurseStation = res;
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.companyToBed = 0;
    this.floors =[];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    //this.resident = [];
    this.selectedFacilityId=item.Facility_Id;
    this.selectedphyItems=[];
    this.physiciansdrop=[];
    this.displayfield={};
    this.AdmitvisitInfodisplayfield={};
    this.censusform.patchValue({
      NursingStationId: '',
      Room: '',
      Bed:'',
      Floor: '',
      Wing:'',
    })
    this.getNurseStations(item.Facility_Id);
    //this.getFloorWingDrop();
    this.getHlsevenconfigData();
    this.getHlsevenconfigforAdmitVisitInfoData();
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.companyToBed = 0;
    this.floors =[];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.selectedphyItems=[];
    this.physiciansdrop=[];
    //this.resident = [];
    this.censusform.patchValue({
      NursingStationId: '',
      Room: '',
      Bed:'',
      Floor: '',
      Wing:'',
    })
    this.displayfield={};
    this.AdmitvisitInfodisplayfield={};
  }
  onNurseStationSelect(item:any) {
  this.getFloorWingDrop();
  this.getDefaultPhysicianByNSId();
  }
  onNurseStationDeSelect(item:any) {
    //this.getFloorWingDrop();
    this.companyToBed = 0;
    this.floors =[];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.selectedphyItems=[];
    this.physiciansdrop=[];
    //this.getDefaultPhysicianByNSId();
  }
  getFloorWingDrop() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.nstations = "";
    if((this.censusform.value.NursingStationId!=null && this.censusform.value.NursingStationId!=undefined && this.censusform.value.NursingStationId.length!=0)&&(this.censusform.value.FacilityId!=null && this.censusform.value.FacilityId!=undefined && this.censusform.value.FacilityId.length!=0) )
    {
      let facilityId = this.censusform.value.FacilityId[0].Facility_Id;
      this.nstations=this.censusform.value.NursingStationId[0].NurseStation_Id;  
      this.comToBedApi=this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId+"/"+this.nstations;
    }
    else if(this.censusform.value.FacilityId!=null && this.censusform.value.FacilityId!=undefined && this.censusform.value.FacilityId.length!=0)
    {
      let facilityId = this.censusform.value.FacilityId[0].Facility_Id;
      this.comToBedApi=this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId;
    }
     if(this.censusform.value.FacilityId!=null && this.censusform.value.FacilityId!=undefined && this.censusform.value.FacilityId.length!=0)
    {
    this.dataservice.get<any>(this.comToBedApi)
    .subscribe(res => {
      if (res.companyBedFlag == 1) {
        this.companyToBed = 1;
        this.floors = res.Floors;
        this.wings = res.Wings;
        this.rooms = res.Rooms;
        this.beds = res.Beds;
      }
      else if (res.companyBedFlag == 0) {
        this.companyToBed = 0;
        this.floors = [];
        this.wings = [];
        this.rooms = [];
        this.beds = [];
      }
      this.floorIndex =0;
      this.wingIndex =0;
      this.roomIndex =0;
      this.bedIndex =0;
      this.form.reset();
      this.fields =[];
      if(this.censusform.value.NursingStationId!=null && this.censusform.value.NursingStationId!=undefined && this.censusform.value.NursingStationId.length!=0)
      {
        this.getNurseStationHierarchyDetailsbyNsId(this.censusform.value.NursingStationId[0].NurseStation_Id);
      }
      this.ng4LoadingSpinnerService.hide();
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else{
      this.companyToBed = 0;
      this.floors = [];
      this.wings = [];
      this.rooms = [];
      this.beds = [];
      this.censusform.patchValue({
        Floor:'',
        Bed:'',
        Wing:'',
        Room:'',
      });
    }
  }
  // getResidentuniqueId(ID: number) {
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<any>(this.config.Emar_FacilityMaster_GetFacilityDetailsByID + ID)
  //     .subscribe(res => {
  //       this.residentUniqueId = res.Company_UniqueId
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.ng4LoadingSpinnerService.hide();
  //       this.alertService.error(error.message);
  //     });
  // }

  insertNewDemographicDetails() {
    //this.savedisable=true;
    if (this.censusform.value.AdmitDate == "") {
      this.errorMessage = "Please select Admit Date";
    }
    else if (this.myform.value.PatientFirstName == '')
      this.errorMessage = "Please enter First Name";
    else if (this.myform.value.PatientLastName == '')
      this.errorMessage = "Please enter Last Name";
    else if (this.myform.value.DOB == '')
      this.errorMessage = "Please select DOB";
      else if (this.myform.value.AdministrativeSex == '')
      this.errorMessage = "Please select Gender";
    else {
      this.savedisable=true; 
      // if (this.residentUniqueId == "PatientMRNumber") {
      //   this.generatedMrNumber = ((this.myform.value.PatientLastName).substring(0, 3) + (this.myform.value.PatientFirstName).substring(0, 4) + (this.myform.value.DOB).substring(8, 10));
      // }
      // else {
      //   this.generatedMrNumber = this.myform.value.PatientMRNumber;
      // }
      // if (this.residentUniqueId == "ExternalPatientId") {
      //   this.generatedExPatientId = ((this.myform.value.PatientLastName).substring(0, 3) + (this.myform.value.PatientFirstName).substring(0, 4) + (this.myform.value.DOB).substring(8, 10));
      // }
      // else {
      //   this.generatedExPatientId = this.myform.value.ExternalPatientId;
      // }
      debugger
      let phyName=this.censusform.value.physicianname[0].PhysicianFullName.split(',');
      let physelected=this.physiciansdrop.filter(ph=>ph.PhysicianNPI==this.censusform.value.physicianname[0].PhysicianNPI)[0];
      let phyId=null;
if(physelected!=null && physelected!=undefined)
phyId=physelected.Physician_Id
      this.demographicsObj =
        {
          Patient_Id: 0,
          ExternalPatientId: this.generatedExPatientId,
          ExternalFacShortName: this.myform.value.ExternalFacShortName,
          ExternalFacPatientId: this.myform.value.ExternalFacPatientId,
          AlternatePatientId: this.myform.value.AlternatePatientId,
          PatientLastName: this.myform.value.PatientLastName!=undefined && this.myform.value.PatientLastName!=null? this.myform.value.PatientLastName.toUpperCase():this.myform.value.PatientLastName,
          PatientFirstName:this.myform.value.PatientFirstName!=undefined && this.myform.value.PatientFirstName!=null? this.myform.value.PatientFirstName.toUpperCase():this.myform.value.PatientFirstName,
          PatientMiddleInitial:this.myform.value.PatientMiddleInitial!=undefined && this.myform.value.PatientMiddleInitial!=null? this.myform.value.PatientMiddleInitial.toUpperCase():this.myform.value.PatientMiddleInitial,
          NameTypeCode:"D",
          MotherMaidenName: this.myform.value.MotherMaidenName,
          DOB: this.myform.value.DOB,
          AdministrativeSex: this.myform.value.AdministrativeSex,
          PatientAlias: this.myform.value.PatientAlias,
          Race: this.myform.value.Race,
          PatientAddress1: this.myform.value.PatientAddress1,
          PatientAddress2: this.myform.value.PatientAddress2,
          PatientCity: this.myform.value.PatientCity,
          PatientState: this.myform.value.PatientState,
          PatientZipCode: this.myform.value.PatientZipCode,
          CountyCode: this.myform.value.CountyCode,
          PhoneHome: this.myform.value.PhoneHome,
          PhoneBusiness: this.myform.value.PhoneBusiness,
          PrimaryLanguage: this.myform.value.PrimaryLanguage,
          MaritalStatus: this.myform.value.MaritalStatus,
          Religion: this.myform.value.Religion,
          PatientMRNumber: this.generatedMrNumber,
          SSN: this.myform.value.SSN,
          DriverLicense: this.myform.value.DriverLicense,
          MotherIdentifier: this.myform.value.MotherIdentifier,
          EthnicGroup: this.myform.value.EthnicGroup,
          BirthPlace: this.myform.value.BirthPlace,
          MultipleBirthIndicator: this.myform.value.MultipleBirthIndicator,
          BirthOrder: this.myform.value.BirthOrder,
          Citizenship: this.myform.value.Citizenship,
          MilitaryStatus: this.myform.value.MilitaryStatus,
          Nationality: this.myform.value.Nationality,
          DeathDateTime: this.myform.value.DeathDateTime,
          DeathIndicator: "N",
          IdentityIndicator: this.myform.value.IdentityIndicator,
          IdentityReliability: this.myform.value.IdentityReliability,
          LastUpdate: this.myform.value.LastUpdate,
          LastFacilityUpdate: this.myform.value.LastFacilityUpdate,
          SpeciesCode: this.myform.value.SpeciesCode,
          BreedCode: this.myform.value.BreedCode,
          Strain: this.myform.value.Strainm,
          ProductionClassCode: this.myform.value.ProductionClassCode,
          TribalCitizenship: this.myform.value.TribalCitizenship,
          ImageLocation: this.myform.value.ImageLocation,
          Patient_Status: 1,
          Patient_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          Patient_CreatedDate: new Date().toISOString(),
          PDOutBoundFileStatus: 1,
          PDOutBoundApproval: null,
          PDOutBoundApprovalBy: null,
          PDOutBoundApprovalOn: null,
          Alert: null,
          Diet: null,
          ApprovalPatient_Id: 0,
          BiometricInfo: '',
        },
        this.admitvisitinfoObj =
        {
          PVisit_Id: this.censusform.value.PVisit_Id,
          Patient_Id: 0,
          PatientClass: this.censusform.value.PatientClass,
          NursingStationId: this.censusform.value.NursingStationId[0].NurseStation_Id,
          Room: this.form.value.Room!=null && this.form.value.Room!=undefined && this.form.value.Room.length!=0?this.form.value.Room[0].Room_Id:"" ,
          Bed: this.form.value.Bed!=null && this.form.value.undefined!=null && this.form.value.Bed.length!=0?this.form.value.Bed[0].Bed_Id:"",
          FacilityId: this.censusform.value.FacilityId[0].Facility_Id,
          Floor: this.form.value.Floor!=null &&this.form.value.Floor!=undefined && this.form.value.Floor.length!=0 ?this.form.value.Floor[0].Floor_Id:"",
          AdmissionType: this.censusform.value.AdmissionType,
          PreAdmitNumber: this.censusform.value.PreAdmitNumber,
          PriorNursingStationId: this.censusform.value.PriorNursingStationId,
          PriorRoom: this.censusform.value.PriorRoom,
          PriorBed: this.censusform.value.PriorBed,
          PriorFacilityId: this.censusform.value.PriorFacilityId,
          PriorFloor: this.censusform.value.PriorFloor,
          PrimaryPhysicianNPI: this.censusform.value.physicianname[0].PhysicianNPI,
          PrimaryPhysicianLName: phyName[0],
          PrimaryPhysicianFName: phyName[1],
          ReferringDoctor: this.censusform.value.ReferringDoctor,
          ConsultingDoctor: this.censusform.value.ConsultingDoctor,
          HospitalService: this.censusform.value.HospitalService,
          TemporaryLocation: this.censusform.value.TemporaryLocation,
          PreAdmitTestIndicator: this.censusform.value.PreAdmitTestIndicator,
          ReAdmissionIndicator: this.censusform.value.ReAdmissionIndicator,
          AdmitSource: this.censusform.value.AdmitSource,
          AmbulatoryStatus: this.censusform.value.AmbulatoryStatus,
          VIPIndicator: this.censusform.value.VIPIndicator,
          AdmittingDoctor: this.censusform.value.AdmittingDoctor,
          PatientType: this.censusform.value.PatientType,
          VisitNumber: this.censusform.value.VisitNumber,
          FinancialClass: this.censusform.value.FinancialClass,
          ChargePriceIndicator: this.censusform.value.ChargePriceIndicator,
          CourtesyCode: this.censusform.value.CourtesyCode,
          CreditRating: this.censusform.value.CreditRating,
          ContractCode: this.censusform.value.ContractCode,
          ContractEffDate: this.censusform.value.ContractEffDate,
          ContractAmount: this.censusform.value.ContractAmount,
          ContractPeriod: this.censusform.value.ContractPeriod,
          InterestCode: this.censusform.value.InterestCode,
          BadDebtCode: this.censusform.value.BadDebtCode,
          BadDebtDate: this.censusform.value.BadDebtDate,
          BadDebtAgencyCode: this.censusform.value.BadDebtAgencyCode,
          BadDebtTransferAmt: this.censusform.value.BadDebtTransferAmt,
          BadDebtRecoveryAmt: this.censusform.value.BadDebtRecoveryAmt,
          DeleteAccIndicator: this.censusform.value.DeleteAccIndicator,
          DeleteAccDate: this.censusform.value.DeleteAccDate,
          DischargeDisposition: this.censusform.value.DischargeDisposition,
          DischargedLocation: this.censusform.value.DischargedLocation,
          DietType: this.censusform.value.DietType,
          ServicingFacility: this.censusform.value.ServicingFacility,
          BedStatus: this.censusform.value.BedStatus,
          AccStatus: this.censusform.value.AccStatus,
          PendingLocation: this.censusform.value.PendingLocation,
          PriorTemporaryLocation: this.censusform.value.PriorTemporaryLocation,
          AdmitDate: this.censusform.value.AdmitDate,
          DischargeDate: this.censusform.value.DischargeDate,
          CurrentPatientBalance: this.censusform.value.CurrentPatientBalance,
          TotalCharges: this.censusform.value.TotalCharges,
          TotalAdjustments: this.censusform.value.TotalAdjustments,
          TotalPayments: this.censusform.value.TotalPayments,
          AlternateVisitId: this.censusform.value.AlternateVisitId,
          VisitIndicator: this.censusform.value.VisitIndicator,
          OtherHealthProvider: this.censusform.value.OtherHealthProvider,
          PVisit_Status: 1,
          PVisit_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          PVisit_CreatedDate: new Date().toISOString(),
          PVOutBoundFileStatus: 1,
          PVOutBoundApproval: null,
          PVOutBoundApprovalBy: null,
          PVOutBoundApprovalOn: null,
          Wing: this.form.value.Wing!=null && this.form.value.Wing!=undefined && this.form.value.Wing.length!=0?this.form.value.Wing[0].Wing_Id:"",
          Physician_Id: phyId,
          ApprovalPVisit_Id: 0,
          DischargeTime:null
        }
      this.newResident =
        {
          DemographicDetails: this.demographicsObj,
          VisitInfoDetails: this.admitvisitinfoObj,
          ResidentUniqueId:this.residentUniqueId,
        }
      this.dataservice.post(this.config.Emar_Resident_InsertResidentDemographicData, this.newResident)
        .subscribe(res => {
          if (res > 0)
          {
            this.Result.emit(res);
        }
        else
            this.alertService.error("Couldn't able to create New Resident. Something went wrong.")
            this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    
  }
  GetPhysicianDropData() {
    this.selectedphyItems=[];
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetPhysicianDropData +0+ "/" + this.censusform.value.NursingStationId[0].NurseStation_Id)
      .subscribe(res => {
        debugger
        this.physiciansdrop = res;
        if (this.defaultPhysicianNPI != null) {
          this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
        this.censusform.patchValue({
          physicianname: this.selectedphyItems,
        });
      }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getDefaultPhysicianByNSId() {
    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + this.censusform.value.NursingStationId[0].NurseStation_Id)
      .subscribe(res => {
        this.defaultPhysicianNPI = res.PhysicianNPI;
        this.GetPhysicianDropData();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationHierarchyDetailsbyNsId(nsId:any)
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyBedMapping_GetNurseStationHierarchyDetailsbyNsId + nsId)
      .subscribe((res: any) => {
    
            if(res == null && this.companyToBed !=0)
            {
              this.floorIndex =1;
              this.wingIndex =2;
              this.roomIndex =3;
              this.bedIndex =4;  
            }
            else if(res != null && this.companyToBed !=0 ){
            this.floorIndex =res.FloorPrior;
            this.wingIndex =res.WingPrior;
            this.roomIndex =res.RoomPrior;
            this.bedIndex =res.BedPrior;
           }
           else if((res == null && this.companyToBed ==0) || this.companyToBed ==0)
           {
            this.floorIndex =0;
            this.wingIndex =0;
            this.roomIndex =0;
            this.bedIndex =0;  
           }
            this.fields = [
              {
                label: 'Floor',
                name: 'Floor',
                data: this.floors,
                settings: {
                  singleSelection: true,
                  idField: "Floor_Id",
                  textField: "Floor_Name",
                  text: "Floors",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: true
                },
                ngModel: this.selectedflItems,
                placeholder: 'Floors',
                index:this.floorIndex
              },
              {
                label: 'Wing',
                name: 'Wing',
                data: this.wings,
                settings: {
                  singleSelection: true,
                  idField: "Wing_Id",
                  textField: "Wing_Desc",
                  text: "Wings",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: true
                },
                ngModel: this.selectedwItems,
                placeholder: 'Wings',
                index:this.wingIndex
              },
              {
                label: 'Room',
                name: 'Room',
                data: this.rooms,
                settings: {
                  singleSelection: true,
                  idField: "Room_Id",
                  textField: "Room_Name",
                  text: 'Rooms',
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: true
                },
                ngModel: this.selectedflItems,
                placeholder: 'Rooms',
                index:this.roomIndex
              },
              {
                label: 'Bed',
                name: 'Bed',
                data: this.beds,
                settings: {
                  singleSelection: true,
                  idField: "Bed_Id",
                  textField: "Bed_Name",
                  text: "Beds",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  closeDropDownOnSelection:true,
                  allowSearchFilter: true
                },
                ngModel: this.selectedbItems,
                placeholder: 'Beds',
                index:this.bedIndex
              }
            ];
       
             let fieldsCtrls = {};
             for (let f of this.fields) {
                 fieldsCtrls[f.name] = new FormControl(f.value)
             }
            this.form = new FormGroup(fieldsCtrls);
            this.dataFields=this.fields.filter(s=>s.index != 0); 
            this.dataFields.sort((a, b) => {
              if(a.index > b.index) {
                return 1;
              } else if(a.index < b.index) {
                return -1;
              } else {
                return 0;
              }
              
            });
            this.fields =this.dataFields;
          
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
