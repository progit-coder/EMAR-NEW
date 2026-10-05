import { Component, OnInit, Output, EventEmitter, Input, } from '@angular/core';
import { NurseStation, Facility, Room, Floor, Bed, } from './../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AdmitvisitInfo } from '../../../models/residentdemographic.model';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { OutboundEvents } from '../../../models/useractivity.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-vistinfoeditmodal',
  templateUrl: './vistinfoeditmodal.component.html',
  styleUrls: ['./vistinfoeditmodal.component.css']
})
export class VistinfoeditmodalComponent implements OnInit {

  @Output() visitInfoData = new EventEmitter<any>();
  @Input() selectedResident: any;
  @Input() PvisitId: any;
  public AdmitVisitInfoData: AdmitvisitInfo[];
  public Hlsevenconfig: Hlsevenconfigs[];
  public admitvisitinfoObj: AdmitvisitInfo;
  public residentId: number;
  public nurseStations: NurseStation[];
  public nurseStation: NurseStation[];
  public rooms: Room[] = [];
  public userfacilitydrop: Facility[];
  public facilityDrop: Facility[];
  public room: Room[];
  public bed: Bed[];
  public floors: Floor[];
  public floorMaster: any[];
  public floor = new Floor();
  public beds: Bed[];
  public wings: any[];
  displayfield: any = {};
  public segmentDesc: string = "Visit Details";
  public template;
  myform: FormGroup;
  errorMessage: string;
  public visitStatus: number;
  public oldNursestation: any;
  public oldFloor: any;
  public oldRoom: any;
  public oldBed: any;
  public oldWing: any;
  public errormessage: string;
  public approvalerrormessage: string;
  public modalHistoryIsOpen: boolean = false;
  public modalEditIsOpen: boolean = false;
  public Approval: any;
  pageConfig = {};
  public residentTransfer: boolean = false;
  public residentDischarge: boolean = false;
  public outboundEventsList: any[] = [];
  public visitId: number;
  public dropdownSettings_NurseStations: any = {};
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_Rooms: any = {};
  dropdownSettings_Beds: any = {};
  public selectednItems = [];
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  public companyToBed: number = 0;
  public comToBedApi:any;
  public nstations: string = "";

  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sharedService: SharedService, public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("AdmitVisitInfo");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.residentId = this.selectedResident;
    this.visitId = this.PvisitId;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.getHlsevenconfigData();
    }
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getHlsevenconfigData() {
    this.getCompanyOutboundCategoriesByPid();
    this.dataservice.get<Hlsevenconfigs[]>(this.config.Emar_HlSevenSegment_GetHlSevenConfigs + this.residentId + "/" + this.segmentDesc)
      .subscribe(res => {
        this.Hlsevenconfig = res;
        for (let index = 0; index < this.Hlsevenconfig.length; index++) {
          this.displayfield[this.Hlsevenconfig[index].FieldName] = this.Hlsevenconfig[index].IsMandatory;
        }
        this.myform = new FormGroup({
          PVisit_Id: new FormControl(''),
          Patient_Id: new FormControl(''),
          PatientClass: new FormControl('', Validators.maxLength(1)),
          NursingStationId: new FormControl(''),
          Wing: new FormControl(''),
          Room: new FormControl(''),
          Bed: new FormControl(''),
          FacilityId: new FormControl(''),
          Floor: new FormControl(''),
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
          AdmitDate: new FormControl(''),
          DischargeDate: new FormControl(''),
          CurrentPatientBalance: new FormControl('', Validators.maxLength(20)),
          TotalCharges: new FormControl('', Validators.maxLength(20)),
          TotalAdjustments: new FormControl('', Validators.maxLength(20)),
          TotalPayments: new FormControl('', Validators.maxLength(20)),
          AlternateVisitId: new FormControl('', Validators.maxLength(150)),
          VisitIndicator: new FormControl('', Validators.maxLength(5)),
          OtherHealthProvider: new FormControl('', Validators.maxLength(150)),
        });
        //this.getNurseStations();
        this.getFacilityDrop();
        //this.getRoomDetails();
        //this.getFloorDropData();
        //this.getBeds();
        this.getAllNurseStations();
        //this.getFloorWingDrop();
        this.getUserFacilityDrop();
        this.getResidentAdmitVisitInfoDatathroughvisit_Id(this.PvisitId);
        this.dropdownSettings_NurseStations = {
          singleSelection: true,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          text: "Nursing Stations",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
        this.dropdownSettings_Wings = {
          singleSelection: true,
          idField: "Wing_Id",
          textField: "Wing_Desc",
          text: "Wings",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
        this.dropdownSettings_Floors = {
          singleSelection: true,
          idField: "Floor_Id",
          textField: "Floor_Name",
          text: "Floors",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
        this.dropdownSettings_Rooms = {
          singleSelection: true,
          idField: "Room_Id",
          textField: "Room_Name",
          text: "Rooms",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
        this.dropdownSettings_Beds = {
          singleSelection: true,
          idField: "Bed_Id",
          textField: "Bed_Name",
          text: "Beds",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  getResidentAdmitVisitInfoDatathroughvisit_Id(visitId: number) {
    this.visitId = visitId;
    this.getCompanyOutboundCategoriesByPid();
    this.dataservice.get<AdmitvisitInfo>(this.config.Emar_AdmitVisitInfo_GetResidentAdmitInfoData + visitId)
      .subscribe(res => {
        this.Approval = res.PVOutBoundApproval;
        this.getFloorWingDrop(res);
        if (this.Approval == 0) {
          this.approvalerrormessage = "This resident record is pending for admin approval."
        }
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  getNurseStations(fetchObj: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + fetchObj.FacilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.fetchData(fetchObj);
        // this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
      },
        error => {
          this.alertService.error(error.message);
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
  getWingDetails() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllWings)
      .subscribe(res => {
        this.wings = res;
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
  getFloorWingDrop(pVisitObj: AdmitvisitInfo) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let facilityId = pVisitObj.FacilityId;
    this.nstations = "";
    if(pVisitObj.NursingStationId!=null && pVisitObj.NursingStationId!=undefined)
    {
      this.nstations=pVisitObj.NursingStationId;
      this.comToBedApi=this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId+"/"+this.nstations;
    }
    else
    {
      this.comToBedApi=this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId;
    }
    
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
        this.getNurseStations(pVisitObj);
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  fetchData(res: AdmitvisitInfo) {
    this.visitStatus = res.PVisit_Status;
    this.selectednItems = [];
    this.selectedflItems = [];
    this.selectedbItems = [];
    this.selectedrItems = [];
    this.selectedwItems = [];
    if (res != null) {
      if (res.NursingStationId != null) { this.selectednItems.push(this.nurseStations.filter(n => n.NurseStation_Id === parseInt(res.NursingStationId))[0]); }

      if (res.Floor != null) { this.selectedflItems.push(this.floors.filter(f => f.Floor_Id === parseInt(res.Floor))[0]); }

      if (res.Room != null) { this.selectedrItems.push(this.rooms.filter(r => r.Room_Id === parseInt(res.Room))[0]); }

      if (res.Bed != null) { this.selectedbItems.push(this.beds.filter(b => b.Bed_Id === parseInt(res.Bed))[0]); }

      if (res.Wing != null) { this.selectedwItems.push(this.wings.filter(w => w.Wing_Id === res.Wing)[0]); }

      this.myform.patchValue({
        // SetID:res.SetID,
        PVisit_Id: res.PVisit_Id,
        Patient_Id: res.Patient_Id,
        PatientClass: res.PatientClass,
        NursingStationId: this.selectednItems,
        Room: this.selectedrItems,
        Bed: this.selectedbItems,
        FacilityId: res.FacilityId,
        Floor: this.selectedflItems,
        Wing: this.selectedwItems,
        AdmissionType: res.AdmissionType,
        PreAdmitNumber: res.PreAdmitNumber,
        PriorNursingStationId: res.PriorNursingStationId,
        PriorRoom: res.PriorRoom,
        PriorBed: res.PriorBed,
        PriorFacilityId: res.PriorFacilityId,
        PriorFloor: res.PriorFloor,
        PrimaryPhysicianNPI: res.PrimaryPhysicianNPI,
        PrimaryPhysicianLName: res.PrimaryPhysicianLName,
        PrimaryPhysicianFName: res.PrimaryPhysicianFName,
        ReferringDoctor: res.ReferringDoctor,
        ConsultingDoctor: res.ConsultingDoctor,
        HospitalService: res.HospitalService,
        TemporaryLocation: res.TemporaryLocation,
        PreAdmitTestIndicator: res.PreAdmitTestIndicator,
        ReAdmissionIndicator: res.ReAdmissionIndicator,
        AdmitSource: res.AdmitSource,
        AmbulatoryStatus: res.AmbulatoryStatus,
        VIPIndicator: res.VIPIndicator,
        AdmittingDoctor: res.AdmittingDoctor,
        PatientType: res.PatientType,
        VisitNumber: res.VisitNumber,
        FinancialClass: res.FinancialClass,
        ChargePriceIndicator: res.ChargePriceIndicator,
        CourtesyCode: res.CourtesyCode,
        CreditRating: res.CreditRating,
        ContractCode: res.ContractCode,
        ContractEffDate: (res.ContractEffDate == null ? '' : res.ContractEffDate.substring(0, 10)),
        ContractAmount: res.ContractAmount,
        ContractPeriod: res.ContractPeriod,
        InterestCode: res.InterestCode,
        BadDebtCode: res.BadDebtCode,
        BadDebtDate: (res.BadDebtDate == null ? '' : res.BadDebtDate.substring(0, 10)),
        BadDebtAgencyCode: res.BadDebtAgencyCode,
        BadDebtTransferAmt: res.BadDebtTransferAmt,
        BadDebtRecoveryAmt: res.BadDebtRecoveryAmt,
        DeleteAccIndicator: res.DeleteAccIndicator,
        DeleteAccDate: (res.DeleteAccDate == null ? '' : res.DeleteAccDate.substring(0, 10)),
        DischargeDisposition: res.DischargeDisposition,
        DischargedLocation: res.DischargedLocation,
        DietType: res.DietType,
        ServicingFacility: res.ServicingFacility,
        BedStatus: res.BedStatus,
        AccStatus: res.AccStatus,
        PendingLocation: res.PendingLocation,
        PriorTemporaryLocation: res.PriorTemporaryLocation,
        AdmitDate: res.AdmitDate.substring(0, 10),
        DischargeDate: res.DischargeDate,
        CurrentPatientBalance: res.CurrentPatientBalance,
        TotalCharges: res.TotalCharges,
        TotalAdjustments: res.TotalAdjustments,
        TotalPayments: res.TotalPayments,
        AlternateVisitId: res.AlternateVisitId,
        VisitIndicator: res.VisitIndicator,
        OtherHealthProvider: res.OtherHealthProvider,

      });
      this.oldNursestation = res.NursingStationId;
      this.oldFloor = res.Floor;
      this.oldRoom = res.Room;
      this.oldBed = res.Bed;
      this.oldWing = res.Wing;
    }
  }
  insertResidentAdmitvisitinfodetails() {
    if (((this.oldNursestation != null && this.myform.value.NursingStationId != undefined && this.oldNursestation != this.myform.value.NursingStationId[0].NurseStation_Id) ||
      (this.oldFloor != null && this.myform.value.Floor != undefined && this.oldFloor != this.myform.value.Floor[0].Floor_Id) ||
      (this.oldRoom != null && this.myform.value.Room != undefined && this.oldRoom != this.myform.value.Room[0].Room_Id) ||
      (this.oldBed != null && this.myform.value.Bed != undefined && this.oldBed != this.myform.value.Bed[0].Bed_Id) ||
      (this.oldWing != null && this.myform.value.Wing != undefined && this.oldWing != this.myform.value.Wing[0].Wing_Id)) && this.myform.value.DischargeDate != null) {
      this.errormessage = "Transfer and Discharge cannot be done at a time!"
    }
    else if (this.myform.value.DischargeDate != null) {
      if (this.myform.value.DischargeDate < this.myform.value.AdmitDate) {
        this.errormessage = "Discharge date cannot be less than Admit date.!"
      }
      else if (new Date(this.myform.value.DischargeDate).setHours(0,0,0,0) > new Date().setHours(0,0,0,0)) {
        this.errormessage = "Discharge date cannot be future date.!";
      }
      else {
        this.updateVisitInfoRecord();
      }
    }
    else {
      this.updateVisitInfoRecord();
    }
  }
  updateVisitInfoRecord() {
    this.admitvisitinfoObj =
      {
        PVisit_Id: this.myform.value.PVisit_Id,
        Patient_Id: this.residentId,
        PatientClass: this.myform.value.PatientClass,
        NursingStationId: this.myform.value.NursingStationId != undefined &&this.myform.value.NursingStationId !=null && this.myform.value.NursingStationId.length !=0? this.myform.value.NursingStationId[0].NurseStation_Id : null,
        Room: this.myform.value.Room != undefined && this.myform.value.Room !=null && this.myform.value.Room.length!=0? this.myform.value.Room[0].Room_Id : null,
        Bed: this.myform.value.Bed != undefined &&this.myform.value.Bed !=null &&this.myform.value.Bed.length !=0 ? this.myform.value.Bed[0].Bed_Id : null,
        FacilityId: this.myform.value.FacilityId,
        Floor: this.myform.value.Floor != undefined &&this.myform.value.Floor !=null && this.myform.value.Floor.length !=0? this.myform.value.Floor[0].Floor_Id : null,
        Wing: this.myform.value.Wing != undefined &&this.myform.value.Wing !=null && this.myform.value.Wing.length !=0? this.myform.value.Wing[0].Wing_Id : null,
        AdmissionType: this.myform.value.AdmissionType,
        PreAdmitNumber: this.myform.value.PreAdmitNumber,
        PriorNursingStationId: this.myform.value.PriorNursingStationId,
        PriorRoom: this.myform.value.PriorRoom,
        PriorBed: this.myform.value.PriorBed,
        PriorFacilityId: this.myform.value.PriorFacilityId,
        PriorFloor: this.myform.value.PriorFloor,
        PrimaryPhysicianNPI: this.myform.value.PrimaryPhysicianNPI,
        PrimaryPhysicianLName: this.myform.value.PrimaryPhysicianLName,
        PrimaryPhysicianFName: this.myform.value.PrimaryPhysicianFName,
        ReferringDoctor: this.myform.value.ReferringDoctor,
        ConsultingDoctor: this.myform.value.ConsultingDoctor,
        HospitalService: this.myform.value.HospitalService,
        TemporaryLocation: this.myform.value.TemporaryLocation,
        PreAdmitTestIndicator: this.myform.value.PreAdmitTestIndicator,
        ReAdmissionIndicator: this.myform.value.ReAdmissionIndicator,
        AdmitSource: this.myform.value.AdmitSource,
        AmbulatoryStatus: this.myform.value.AmbulatoryStatus,
        VIPIndicator: this.myform.value.VIPIndicator,
        AdmittingDoctor: this.myform.value.AdmittingDoctor,
        PatientType: this.myform.value.PatientType,
        VisitNumber: this.myform.value.VisitNumber,
        FinancialClass: this.myform.value.FinancialClass,
        ChargePriceIndicator: this.myform.value.ChargePriceIndicator,
        CourtesyCode: this.myform.value.CourtesyCode,
        CreditRating: this.myform.value.CreditRating,
        ContractCode: this.myform.value.ContractCode,
        ContractEffDate: this.myform.value.ContractEffDate,
        ContractAmount: this.myform.value.ContractAmount,
        ContractPeriod: this.myform.value.ContractPeriod,
        InterestCode: this.myform.value.InterestCode,
        BadDebtCode: this.myform.value.BadDebtCode,
        BadDebtDate: this.myform.value.BadDebtDate,
        BadDebtAgencyCode: this.myform.value.BadDebtAgencyCode,
        BadDebtTransferAmt: this.myform.value.BadDebtTransferAmt,
        BadDebtRecoveryAmt: this.myform.value.BadDebtRecoveryAmt,
        DeleteAccIndicator: this.myform.value.DeleteAccIndicator,
        DeleteAccDate: this.myform.value.DeleteAccDate,
        DischargeDisposition: this.myform.value.DischargeDisposition,
        DischargedLocation: this.myform.value.DischargedLocation,
        DietType: this.myform.value.DietType,
        ServicingFacility: this.myform.value.ServicingFacility,
        BedStatus: this.myform.value.BedStatus,
        AccStatus: this.myform.value.AccStatus,
        PendingLocation: this.myform.value.PendingLocation,
        PriorTemporaryLocation: this.myform.value.PriorTemporaryLocation,
        AdmitDate: this.myform.value.AdmitDate,
        DischargeDate: this.myform.value.DischargeDate,
        CurrentPatientBalance: this.myform.value.CurrentPatientBalance,
        TotalCharges: this.myform.value.TotalCharges,
        TotalAdjustments: this.myform.value.TotalAdjustments,
        TotalPayments: this.myform.value.TotalPayments,
        AlternateVisitId: this.myform.value.AlternateVisitId,
        VisitIndicator: this.myform.value.VisitIndicator,
        OtherHealthProvider: this.myform.value.OtherHealthProvider,
        PVisit_Status: 1,
        PVisit_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        PVisit_CreatedDate: new Date().toISOString(),
        PVOutBoundFileStatus: 1,
        PVOutBoundApproval: null,
        PVOutBoundApprovalBy: null,
        PVOutBoundApprovalOn: null,
        Physician_Id: null,
        DischargeTime:null,
      }
    this.dataservice.post(this.config.Emar_AdminApproval_InsertResidentAdmitVisitInfoData, this.admitvisitinfoObj)
      .subscribe(res => {
        this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
          .subscribe(approvalFlag => {
            if (approvalFlag == 1) {
              if (res == 1) {
                this.visitInfoData.emit({ approval: approvalFlag, result: res });
              }
              else if (res == 2) {
                this.visitInfoData.emit({ approval: approvalFlag, result: res });
              }
              else if (res == 3) {
                this.visitInfoData.emit({ approval: approvalFlag, result: res });
              }
            }
            else {
              if (res == 1) {
                this.visitInfoData.emit({ approval: 0, result: res });
              }
              else if (res == 2) {
                this.visitInfoData.emit({ approval: 0, result: res });
              }
              else if (res == 3) {
                this.visitInfoData.emit({ approval: 0, result: res });
              }
            }
          }, error => {
            this.alertService.error(error.message)
          });

      }, error => {
        this.alertService.error(error.message);
      });
  }
  getCompanyOutboundCategoriesByPid() {
    this.dataservice.get<any[]>(this.config.Emar_Company_GetCompanyEventCategoriesByPid + this.residentId)
      .subscribe(res => {
        if (res == null) {
          this.residentTransfer = false;
          this.residentDischarge = false;
        }
        else {
          this.outboundEventsList = res;
          let transfer = this.outboundEventsList.find(o => o.EventCat_Id == OutboundEvents.TransferPatient);
          let discharge = this.outboundEventsList.find(o => o.EventCat_Id == OutboundEvents.DischargeEndVisit);
          if (transfer == undefined) {
            this.residentTransfer = false;
          }
          else {
            this.residentTransfer = true;
          }
          if (discharge == undefined) {
            this.residentDischarge = false;
          }
          else {
            this.residentDischarge = true;
          }
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onNurseStationSelect(item: any) {
    this.getFloorWingDropOnNurseStationChange();
  }
  onNurseStationDeSelect(item: any) {
    this.getFloorWingDropOnNurseStationChange();
  }
  getFloorWingDropOnNurseStationChange() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let facilityId = this.myform.value.FacilityId;
    this.nstations = "";
    if(this.myform.value.NursingStationId!=null && this.myform.value.NursingStationId!=undefined && this.myform.value.NursingStationId.length!=0)
    {
      this.nstations=this.myform.value.NursingStationId[0].NurseStation_Id;  
      this.comToBedApi=this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId+"/"+this.nstations;
    }
    else
    {
      this.comToBedApi=this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId;
    }
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
      },
        error => {
          this.alertService.error(error.message);
        });
  }
}
