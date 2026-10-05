import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
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
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-transferresidentmodal',
  templateUrl: './transferresidentmodal.component.html',
  styleUrls: ['./transferresidentmodal.component.css']
})
export class TransferresidentmodalComponent implements OnInit {
  @Output() transferResult = new EventEmitter<any>();
  @Input() selectedResident: any;
  @Input() PvisitId: any;
  public residentId: number;
  public visitId: number;
  myform: FormGroup;
  public admitvisitinfoObj: AdmitvisitInfo;
  public Hlsevenconfig: any[];
  displayfield: any = {};
  public dropdownSettings_NurseStations: any = {};
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_Rooms: any = {};
  dropdownSettings_Beds: any = {};
  dropdownSettings_Facility = {};
  public selectedfalItems = [];
  public selectednItems = [];
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  public companyToBed: number = 0;
  public comToBedApi: any;
  public nstations: string = "";
  public facilityDrop: Facility[];
  public userfacilitydrop: Facility[];
  public Approval: any;
  public approvalerrormessage: string;
  public rooms: Room[] = [];
  public floors: Floor[];
  public beds: Bed[];
  public wings: any[];
  public nurseStations: NurseStation[];
  public nurseStation: NurseStation[];
  public visitStatus: number;
  public oldFacility:any;
  public oldNursestation: any;
  public oldFloor: any;
  public oldRoom: any;
  public oldBed: any;
  public oldWing: any;
  public pageConfig = {};
  public Facilityname: string;
  public NursingStationName: string;
  public FloorName: string;
  public WingName: string;
  public RoomName: string;
  public BedName: string;
  public AdmitDate: string;
  public PhysicianName: string;
  public fetchComToBed: number = 0;
  public errormessage: string;
  public allFields:number;
  residentVisitInfoObj: AdmitvisitInfo;
  public form: FormGroup;
  public fields: any[];
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  public fetchform: FormGroup;
  public fetchfields: any[];
  public fetchdataFields:any[];
  public fetchfloorIndex:number =0;
  public fetchwingIndex:number=0;
  public fetchroomIndex:number =0;
  public fetchbedIndex:number =0;

  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private sharedService: SharedService, public activeDefaultModal: NgbActiveModal,private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Transfer");
    this.residentId = this.selectedResident;
    this.visitId = this.PvisitId;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.ng4LoadingSpinnerService.show();
      this.myform = new FormGroup({
        FacilityId: new FormControl(''),
        NursingStationId: new FormControl(''),
        //Floor: new FormControl(''),
        //Wing: new FormControl(''),
        //Room: new FormControl(''),
        //Bed: new FormControl(''),
      });
      this.form = new FormGroup({
        fields: new FormControl(JSON.stringify(this.fields))
      });
      this.fetchform = new FormGroup({
        fetchfields: new FormControl(JSON.stringify(this.fetchfields))
      });
      this.dropdownSettings_Facility = {
        singleSelection: true,
        idField: "Facility_Id",
        textField: "Facility_Name",
        text: "Facility",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true
      };
      this.dropdownSettings_NurseStations = {
        singleSelection: true,
        idField: "NurseStation_Id",
        textField: "NurseStation_Name",
        text: "Nursing Stations",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        noDataAvailablePlaceholderText: "Please Select Facility",
        allowSearchFilter: true
      };
      // this.dropdownSettings_Wings = {
      //   singleSelection: true,
      //   idField: "Wing_Id",
      //   textField: "Wing_Desc",
      //   text: "Wings",
      //   itemsShowLimit: 1,
      //   allowSearchFilter: true
      // };
      // this.dropdownSettings_Floors = {
      //   singleSelection: true,
      //   idField: "Floor_Id",
      //   textField: "Floor_Name",
      //   text: "Floors",
      //   itemsShowLimit: 1,
      //   allowSearchFilter: true
      // };
      // this.dropdownSettings_Rooms = {
      //   singleSelection: true,
      //   idField: "Room_Id",
      //   textField: "Room_Name",
      //   text: "Rooms",
      //   itemsShowLimit: 1,
      //   allowSearchFilter: true
      // };
      // this.dropdownSettings_Beds = {
      //   singleSelection: true,
      //   idField: "Bed_Id",
      //   textField: "Bed_Name",
      //   text: "Beds",
      //   itemsShowLimit: 1,
      //   allowSearchFilter: true
      // };
      this.getHlsevenconfigData();
    }
  }
  getHlsevenconfigData() {
    this.dataservice.get<any[]>(this.config.Emar_HlSevenConfigs_GetOutboundHLSevenConfigs + this.residentId + "/" + 2)
      .subscribe(res => {
        this.Hlsevenconfig = res;
        for (let index = 0; index < this.Hlsevenconfig.length; index++) {
          this.displayfield[this.Hlsevenconfig[index].OSegDetail_Desc] = this.Hlsevenconfig[index].DisplayConfigId;
      }
      for (let index = 0; index < this.Hlsevenconfig.length; index++) {
        this.allFields = this.Hlsevenconfig[index].DisplayConfigId;
        
        if(this.allFields==1)
        break;
      }
      if (this.displayfield.NursingStationId == 1) {
        const nurseStationValidation = this.myform.get('NursingStationId');
        nurseStationValidation.setValidators([Validators.required]);
        nurseStationValidation.updateValueAndValidity();
      }
      if (this.displayfield.FacilityId == 1) {
        const facilityValidation = this.myform.get('FacilityId');
        facilityValidation.setValidators([Validators.required]);
        facilityValidation.updateValueAndValidity();
      }
        this.getUserFacilityDrop();
        this.getResidentAdmitVisitInfoDatathroughvisit_Id(this.PvisitId);
      },
        error => {
          this.alertService.error(error.message);
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
          this.ng4LoadingSpinnerService.hide();
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
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getResidentAdmitVisitInfoDatathroughvisit_Id(visitId: number) {
    this.visitId = visitId;
    this.dataservice.get<AdmitvisitInfo>(this.config.Emar_AdmitVisitInfo_GetResidentAdmitInfoData + visitId)
      .subscribe(res => {
        this.Approval = res.PVOutBoundApproval;
        this.residentVisitInfoObj=res;
        this.ng4LoadingSpinnerService.hide();
        this.getUserFacilityDrop();
        this.getFetchFloorWingDrop(res);
        if (this.Approval == 0) {
          this.approvalerrormessage = "This resident record is pending for admin approval."
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getFetchFloorWingDrop(pVisitObj: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let facilityId = pVisitObj.FacilityId;
    this.nstations = "";
    this.fetchfloorIndex =0;
    this.fetchwingIndex =0;
    this.fetchroomIndex =0;
    this.fetchbedIndex =0;
    this.fetchform.reset();
    this.fetchfields =[];
    if (pVisitObj.NursingStationId != null && pVisitObj.NursingStationId != undefined && pVisitObj.NursingStationId != 0) {
      this.nstations = pVisitObj.NursingStationId;
      this.comToBedApi = this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId + "/" + this.nstations;
    }
    else {
      this.comToBedApi = this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId;
    }

    this.dataservice.get<any>(this.comToBedApi)
      .subscribe(res => {
        if (res.companyBedFlag == 1) {
          this.fetchComToBed = 1;
          this.floors = res.Floors;
          this.wings = res.Wings;
          this.rooms = res.Rooms;
          this.beds = res.Beds;
        }
        else if (res.companyBedFlag == 0) {
          this.fetchComToBed = 0;
          this.floors = [];
          this.wings = [];
          this.rooms = [];
          this.beds = [];
        }
        this.getNurseStations(pVisitObj);
        if(pVisitObj.NursingStationId != null && pVisitObj.NursingStationId != undefined && pVisitObj.NursingStationId != 0)
        {
        this.getFetchNurseStationHierarchy(pVisitObj.NursingStationId);
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getFloorWingDrop(type:number,facId?:number,nsId?:any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let facilityId =type==2?this.displayfield.FacilityId!=1?this.residentVisitInfoObj.FacilityId: this.myform.value.FacilityId[0].Facility_Id :facId;
    this.nstations = "";
    if (type==2 && this.myform.value.NursingStationId != null && this.myform.value.NursingStationId != undefined && this.myform.value.NursingStationId.length != 0) {
      this.nstations = this.myform.value.NursingStationId[0].NurseStation_Id;
      this.comToBedApi = this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId + "/" + this.nstations;
    }
    else if(type==1)
    {
      this.nstations=nsId;
      this.comToBedApi = this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId + "/" + this.nstations;
    }
    else {
      this.comToBedApi = this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId;
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
          this.floorIndex =0;
          this.wingIndex =0;
          this.roomIndex =0;
          this.bedIndex =0;
          this.form.reset();
          this.fields =[];
        if(this.myform.value.NursingStationId != null && this.myform.value.NursingStationId != undefined && this.myform.value.NursingStationId.length != 0)
        {
        this.getNurseStationHierarchyDetailsbyNsId(this.myform.value.NursingStationId[0].NurseStation_Id);
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseStations(visitObj: any, facilityId?: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let facilityID = visitObj == null || visitObj == undefined ? facilityId : visitObj.FacilityId;
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityID)
      .subscribe(res => {
        if (visitObj == null) {
          this.nurseStation = res;
        }
        else {
          this.nurseStations = res;
          this.fetchData(visitObj);
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchData(res: AdmitvisitInfo) {
    this.visitStatus = res.PVisit_Status;
    this.Facilityname = '';
    this.NursingStationName = '';
    this.FloorName = '';
    this.WingName = '';
    this.RoomName = '';
    this.BedName = '';
    this.AdmitDate = '';
    this.PhysicianName = '';
    if (res != null) {
      if (res.FacilityId != null) {
        let checkFacExist = this.userfacilitydrop.find(r => r.Facility_Id == parseInt(res.FacilityId));
        if (checkFacExist != undefined) {
          this.Facilityname = checkFacExist.Facility_Name;
        }
      }
      if (res.NursingStationId != null) {
        let checkNsExist = this.nurseStations.find(n => n.NurseStation_Id === parseInt(res.NursingStationId));
        if (checkNsExist != undefined) {
          this.NursingStationName = checkNsExist.NurseStation_Name;
        }
      }

      if (res.Floor != null  && this.floors!=undefined && this.floors.length > 0) {
        let checkFlExist = this.floors.find(f => f.Floor_Id === parseInt(res.Floor));
        if (checkFlExist != undefined) {
          this.FloorName = checkFlExist.Floor_Name;
        }
      }
      if (res.Wing != null && this.wings!=undefined&& this.wings.length > 0) {
        let checkWiExist = this.wings.find(w => w.Wing_Id === res.Wing);
        if (checkWiExist != undefined) {
          this.WingName = checkWiExist.Wing_Desc;
        }
      }
      if (res.Room != null&& this.rooms!=undefined && this.rooms.length > 0) {
        let checkRoExist = this.rooms.find(r => r.Room_Id === parseInt(res.Room));
        if (checkRoExist != undefined) {
          this.RoomName = checkRoExist.Room_Name;
        }
      }
      if (res.Bed != null && this.beds!=undefined&& this.beds.length > 0) {
        let checkBeExist = this.beds.find(b => b.Bed_Id === parseInt(res.Bed));
        if (checkBeExist != undefined) {
          this.BedName = checkBeExist.Bed_Name;
        }
      }
      this.AdmitDate = this.dateFormatPipe.transform(res.AdmitDate);
      if (res.PrimaryPhysicianLName != null && res.PrimaryPhysicianFName != null) {
        this.PhysicianName = res.PrimaryPhysicianLName + ", " + res.PrimaryPhysicianFName;
      }
      this.oldFacility=res.FacilityId;
      this.oldNursestation = res.NursingStationId;
      this.oldFloor = res.Floor;
      this.oldRoom = res.Room;
      this.oldBed = res.Bed;
      this.oldWing = res.Wing;
      if(this.displayfield.FacilityId!=1 && this.displayfield.NursingStationId==1)
      {
        this.getNurseStations(null,res.FacilityId);
      }
      if((this.displayfield.FacilityId!=1 && this.displayfield.NursingStationId!=1) && (this.displayfield.Floor==1 ||this.displayfield.Room==1 ||this.displayfield.Bed==1))
      {
        this.getFloorWingDrop(1,parseInt(res.FacilityId),parseInt(res.NursingStationId))
      }
    }
  }
  onFacilitySelect(item: any) {
    this.errormessage="";
    this.nurseStation=[];
    this.selectednItems = [];
    this.selectedflItems = [];
    this.selectedwItems = [];
    this.selectedrItems = [];
    this.selectedbItems = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.myform.patchValue({
    NursingStationId:this.selectednItems,
    Floor:this.selectedflItems,
    Wing:this.selectedwItems,
    Room:this.selectedwItems,
    Bed:this.selectedbItems,
    });
    this.getNurseStations(null, item.Facility_Id);
    //this.getFloorWingDrop(2);
  }
  onFacilityDeSelect(item: any) {
    this.errormessage="";
    this.nurseStation = [];
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.selectednItems = [];
    this.selectedflItems = [];
    this.selectedwItems = [];
    this.selectedrItems = [];
    this.selectedbItems = [];
    this.myform.patchValue({
    NursingStationId:this.selectednItems,
    Floor:this.selectedflItems,
    Wing:this.selectedwItems,
    Room:this.selectedwItems,
    Bed:this.selectedbItems,
    });
  }
  onNurseStationSelect(item: any) {
    this.errormessage="";
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.selectedflItems =[];
    this.selectedwItems = [];
    this.selectedrItems = [];
    this.selectedbItems = [];
    this.myform.patchValue({
    Floor:this.selectedflItems,
    Wing:this.selectedwItems,
    Room:this.selectedwItems,
    Bed:this.selectedbItems,
    });
    this.getFloorWingDrop(2);
  }
  transferResident() {
    this.errormessage='';
    if((this.myform.value.FacilityId == undefined || this.myform.value.FacilityId==""&& this.myform.value.FacilityId.length==0) && 
    (this.myform.value.NursingStationId.length == 0 || this.myform.value.NursingStationId=="" && this.myform.value.NursingStationId == undefined) &&
    (this.form.value.Floor.length==0 || this.form.value.Floor=="" || this.form.value.Floor ==null || this.form.value.Floor == undefined ) && (this.form.value.Room.length ==0 ||  this.form.value.Room==null||  this.form.value.Room==""|| this.form.value.Room == undefined) &&
    (this.form.value.Bed.length ==0 || this.form.value.Bed==null || this.form.value.Bed=="" || this.form.value.Bed.length ==0 || this.form.value.Bed == undefined ) &&
    (this.form.value.Wing.length ==0 || this.form.value.Wing ==null|| this.myform.value.Wing =="" || this.form.value.Wing.length==0 || this.form.value.Wing == undefined))
    {
      this.errormessage = "Please select data";
    }
   else if((this.oldFacility != null && this.myform.value.FacilityId != undefined && this.myform.value.FacilityId!=""&& this.myform.value.FacilityId.length!=0 && this.oldFacility != this.myform.value.FacilityId[0].Facility_Id) || 
    (this.oldNursestation != null && this.myform.value.NursingStationId.length != 0 && this.myform.value.NursingStationId!="" && this.myform.value.NursingStationId != undefined && this.oldNursestation != this.myform.value.NursingStationId[0].NurseStation_Id) ||
    (this.form.value.Floor != undefined && this.form.value.Floor.length!=0 && this.form.value.Floor!=null && this.form.value.Floor!="" && this.oldFloor != this.form.value.Floor[0].Floor_Id) || (this.form.value.Room != null && this.form.value.Room != undefined  && this.form.value.Room.length !=0 &&  this.form.value.Room!="" && this.oldRoom != this.form.value.Room[0].Room_Id) ||
    (this.form.value.Bed != undefined && this.form.value.Bed.length !=0 && this.form.value.Bed!=null && this.form.value.Bed!="" && this.form.value.Bed.length !=0  && this.oldBed != this.form.value.Bed[0].Bed_Id) ||
    (this.form.value.Wing != undefined && this.form.value.Wing !=""  && this.form.value.Wing !=null && this.form.value.Wing.length!=0 && this.oldWing != this.form.value.Wing[0].Wing_Id))
    {
      this.insertTransferResident();
    }
    else
    {
      this.errormessage = "Resident current location & To be Transferred can't be same!";
    }
}
insertTransferResident()
{
    this.admitvisitinfoObj =
      {
        PVisit_Id: this.visitId,
        Patient_Id: this.residentId,
        PatientClass: null,
        NursingStationId: this.myform.value.NursingStationId != undefined && this.myform.value.NursingStationId!="" &&this.myform.value.NursingStationId !=null && this.myform.value.NursingStationId.length !=0? this.myform.value.NursingStationId[0].NurseStation_Id : null,
        Room: (this.form.value.Room != null &&  this.form.value.Room.length!=0)? this.form.value.Room[0].Room_Id : null,
        Bed: (this.form.value.Bed != null && this.form.value.Bed.length !=0 )? this.form.value.Bed[0].Bed_Id : null,
        FacilityId: this.myform.value.FacilityId !=undefined &&this.myform.value.FacilityId!="" && this.myform.value.FacilityId.length!=0 && this.myform.value.FacilityId!=null ? this.myform.value.FacilityId[0].Facility_Id:null,
        Floor: (this.form.value.Floor !=null &&this.form.value.Floor.length != 0)? this.form.value.Floor[0].Floor_Id : null,
        Wing: (this.form.value.Wing != null && this.form.value.Wing.length !=0)? this.form.value.Wing[0].Wing_Id : null,
        AdmissionType: null,
        PreAdmitNumber: null,
        PriorNursingStationId: null,
        PriorRoom:null,
        PriorBed: null,
        PriorFacilityId: null,
        PriorFloor: null,
        PrimaryPhysicianNPI: null,
        PrimaryPhysicianLName: null,
        PrimaryPhysicianFName: null,
        ReferringDoctor: null,
        ConsultingDoctor:null,
        HospitalService: null,
        TemporaryLocation: null,
        PreAdmitTestIndicator: null,
        ReAdmissionIndicator: null,
        AdmitSource: null,
        AmbulatoryStatus: null,
        VIPIndicator: null,
        AdmittingDoctor: null,
        PatientType: null,
        VisitNumber:null,
        FinancialClass: null,
        ChargePriceIndicator: null,
        CourtesyCode: null,
        CreditRating: null,
        ContractCode: null,
        ContractEffDate: null,
        ContractAmount: null,
        ContractPeriod: null,
        InterestCode: null,
        BadDebtCode: null,
        BadDebtDate: null,
        BadDebtAgencyCode: null,
        BadDebtTransferAmt: null,
        BadDebtRecoveryAmt: null,
        DeleteAccIndicator: null,
        DeleteAccDate: null,
        DischargeDisposition: null,
        DischargedLocation: null,
        DietType: null,
        ServicingFacility: null,
        BedStatus: null,
        AccStatus: null,
        PendingLocation: null,
        PriorTemporaryLocation: null,
        AdmitDate: null,
        DischargeDate: null,
        CurrentPatientBalance: null,
        TotalCharges: null,
        TotalAdjustments: null,
        TotalPayments: null,
        AlternateVisitId: null,
        VisitIndicator: null,
        OtherHealthProvider: null,
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
                this.transferResult.emit({ approval: approvalFlag, result: res });
              }
              // else if (res == 2) {
              //   this.transferResult.emit({ approval: approvalFlag, result: res });
              // }
              // else if (res == 3) {
              //   this.transferResult.emit({ approval: approvalFlag, result: res });
              // }
              else if (res == 4) {
                this.transferResult.emit({ approval: approvalFlag, result: res });
              }
            }
            else {
              if (res == 1) {
                this.transferResult.emit({ approval: 0, result: res });
              }
              else if (res == 4) {
                this.transferResult.emit({ approval: approvalFlag, result: res });
              }
              // else if (res == 2) {
              //   this.transferResult.emit({ approval: 0, result: res });
              // }
              // else if (res == 3) {
              //   this.transferResult.emit({ approval: 0, result: res });
              // }
            }
          }, error => {
            this.alertService.error(error.message)
          });

      }, error => {
        this.alertService.error(error.message);
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
  getFetchNurseStationHierarchy(nursingStationId:any)
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyBedMapping_GetNurseStationHierarchyDetailsbyNsId + nursingStationId)
      .subscribe((res: any) => {
    
            if(res == null && this.fetchComToBed !=0)
            {
              this.fetchfloorIndex =1;
              this.fetchwingIndex =2;
              this.fetchroomIndex =3;
              this.fetchbedIndex =4;  
            }
            else if(res != null ){
            this.fetchfloorIndex =res.FloorPrior;
            this.fetchwingIndex =res.WingPrior;
            this.fetchroomIndex =res.RoomPrior;
            this.fetchbedIndex =res.BedPrior;
           }
           else if(res == null && this.fetchComToBed ==0)
           {
            this.fetchfloorIndex =0;
            this.fetchwingIndex =0;
            this.fetchroomIndex =0;
            this.fetchbedIndex =0;  
           }
            this.fetchfields = [
              {
                label: 'Floor',
                name: 'Floor',
                index:this.fetchfloorIndex
              },
              {
                label: 'Wing',
                name: 'Wing',
                index:this.fetchwingIndex
              },
              {
                label: 'Room',
                name: 'Room',
                index:this.fetchroomIndex
              },
              {
                label: 'Bed',
                name: 'Bed',
                index:this.fetchbedIndex
              }
            ];
       
             let fieldsCtrls = {};
             for (let f of this.fetchfields) {
                 fieldsCtrls[f.name] = new FormControl(f.value)
             }
            this.fetchform = new FormGroup(fieldsCtrls);
            this.fetchdataFields=this.fetchfields.filter(s=>s.index != 0); 
            this.fetchdataFields.sort((a, b) => {
              if(a.index > b.index) {
                return 1;
              } else if(a.index < b.index) {
                return -1;
              } else {
                return 0;
              }
              
            });
            this.fetchfields =this.fetchdataFields;
          
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
