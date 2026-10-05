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
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-dischargeresidentmodal',
  templateUrl: './dischargeresidentmodal.component.html',
  styleUrls: ['./dischargeresidentmodal.component.css']
})
export class DischargeresidentmodalComponent implements OnInit {
  @Output() dischargeResult = new EventEmitter<any>();
  @Input() selectedResident: any;
  @Input() PvisitId: any;
  public admitvisitinfoObj: AdmitvisitInfo;
  public residentId: number;
  public visitId: number;
  public Hlsevenconfig: any[];
  displayfield: any = {};
  myform: FormGroup;
  public userfacilitydrop: Facility[];
  public Approval: any;
  public approvalerrormessage: string;
  public rooms: Room[] = [];
  public floors: Floor[];
  public beds: Bed[];
  public wings: any[];
  public nurseStations: NurseStation[];
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
  public comToBedApi: any;
  public nstations: string = "";
  public visitStatus: number;
  public dropdownSettings_Physician={};
  public physiciansdrop:any[]=[];
  public selectedphyItems: any[];
  public oldFacility:any;
  public oldNursestation: any;
  public oldFloor: any;
  public oldRoom: any;
  public oldBed: any;
  public oldWing: any;
  public errormessage: string;
  public checkAdmitdate:any;
  public allFields:number;
  public fetchform: FormGroup;
  public fetchfields: any[];
  public fetchdataFields:any[];
  public fetchfloorIndex:number =0;
  public fetchwingIndex:number=0;
  public fetchroomIndex:number =0;
  public fetchbedIndex:number =0;

  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private dateFormatPipe: CustomdatePipe, public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Discharge");
    this.residentId = this.selectedResident;
    this.visitId = this.PvisitId;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.ng4LoadingSpinnerService.show();
      this.myform = new FormGroup({
        DischargeDate: new FormControl(''),
        DischargeTime: new FormControl(''),
      });
      this.fetchform = new FormGroup({
        fetchfields: new FormControl(JSON.stringify(this.fetchfields))
      });
      this.getHlsevenconfigData();
    }
  }
  getHlsevenconfigData() {
    this.dataservice.get<any[]>(this.config.Emar_HlSevenConfigs_GetOutboundHLSevenConfigs + this.residentId + "/" + 3)
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
      if (this.displayfield.DischargeDate == 1) {
        const dischargeValidation = this.myform.get('DischargeDate');
        dischargeValidation.setValidators([Validators.required]);
        dischargeValidation.updateValueAndValidity();
        const dischargeTimeValidation = this.myform.get('DischargeTime');
        dischargeTimeValidation.setValidators([Validators.required]);
        dischargeTimeValidation.updateValueAndValidity();
      }
        this.getUserFacilityDrop();
        this.getResidentAdmitVisitInfoDatathroughvisit_Id(this.PvisitId);
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
        });
  }
  getResidentAdmitVisitInfoDatathroughvisit_Id(visitId: number) {
    this.visitId = visitId;
    this.dataservice.get<AdmitvisitInfo>(this.config.Emar_AdmitVisitInfo_GetResidentAdmitInfoData + visitId)
      .subscribe(res => {
        this.Approval = res.PVOutBoundApproval;
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
  getNurseStations(visitObj: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let facilityID =  visitObj.FacilityId;
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId + "/" + facilityID)
      .subscribe(res => {
          this.nurseStations = res;
          this.fetchData(visitObj);
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

      if (res.Floor != null && this.floors!=undefined && this.floors.length > 0) {
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
      if (res.Room != null && this.rooms!=undefined && this.rooms.length > 0) {
        let checkRoExist = this.rooms.find(r => r.Room_Id === parseInt(res.Room));
        if (checkRoExist != undefined) {
          this.RoomName = checkRoExist.Room_Name;
        }
      }
      if (res.Bed != null&& this.beds!=undefined && this.beds.length > 0) {
        let checkBeExist = this.beds.find(b => b.Bed_Id === parseInt(res.Bed));
        if (checkBeExist != undefined) {
          this.BedName = checkBeExist.Bed_Name;
        }
      }
      this.AdmitDate = this.dateFormatPipe.transform(res.AdmitDate);
      if (res.PrimaryPhysicianLName != null && res.PrimaryPhysicianFName != null) {
        this.PhysicianName = res.PrimaryPhysicianLName + ", " + res.PrimaryPhysicianFName;
      }
      this.myform.patchValue({
        DischargeDate:res.DischargeDate!=null?this.dateFormatPipe.dateFormat(res.DischargeDate):res.DischargeDate,
        DischargeTime:res.DischargeDate!=null?this.dateFormatPipe.get24HourTime(res.DischargeDate):null,
      });
      this.checkAdmitdate=res.AdmitDate;
      this.oldFacility=res.FacilityId;
      this.oldNursestation = res.NursingStationId;
      this.oldFloor = res.Floor;
      this.oldRoom = res.Room;
      this.oldBed = res.Bed;
      this.oldWing = res.Wing;
    }
  }
  dischargeResident() 
  {
    debugger;
    console.log(this.myform.value.DischargeDate);
    console.log(this.dateFormatPipe.transformISODate(this.checkAdmitdate));


    if (this.myform.value.DischargeDate < this.dateFormatPipe.transformISODate(this.checkAdmitdate)) {
      this.errormessage = "Discharge date cannot be less than Admit date.!"
    }



    else if (this.myform.value.DischargeDate > this.dateFormatPipe.transformISODate(new Date())) {
      this.errormessage = "Discharge date cannot be future date.!";
    }
    else 
    {
    this.admitvisitinfoObj =
    {
      PVisit_Id: this.visitId,
      Patient_Id: this.residentId,
      PatientClass: null,
      NursingStationId: this.oldNursestation,
      Room: this.oldRoom,
      Bed: this.oldBed,
      FacilityId: this.oldFacility,
      Floor: this.oldFloor,
      Wing: this.oldWing,
      AdmissionType: null,
      PreAdmitNumber: null,
      PriorNursingStationId: null,
      PriorRoom:null,
      PriorBed: null,
      PriorFacilityId: null,
      PriorFloor: null,
      PrimaryPhysicianNPI: null,
      PrimaryPhysicianLName:null,
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
      DischargeDate: this.myform.value.DischargeDate,
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
      Physician_Id:null,
      DischargeTime:this.timeConvert(this.myform.value.DischargeTime)
    }
    debugger
  this.dataservice.post(this.config.Emar_AdminApproval_InsertResidentAdmitVisitInfoData, this.admitvisitinfoObj)
    .subscribe(res => {
      this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
        .subscribe(approvalFlag => {
          if (approvalFlag == 1) {
            //if (res == 1) {
             // this.visitInfoData.emit({ approval: approvalFlag, result: res });
            //}
              if (res == 2) {
               this.dischargeResult.emit({ approval: approvalFlag, result: res });
             }
            //   if (res == 3) {
            //   this.visitInfoData.emit({ approval: approvalFlag, result: res });
            //  }
          }
          else {
            // if (res == 1) {
            //   this.visitInfoData.emit({ approval: 0, result: res });
            // }
              if (res == 2) {
               this.dischargeResult.emit({ approval: 0, result: res });
            }
            //  if (res == 3) {
            //    this.visitInfoData.emit({ approval: 0, result: res });
            //  }
          }
        }, error => {
          this.alertService.error(error.message)
        });

    }, error => {
      this.alertService.error(error.message);
    });
  }
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
  timeConvert(time) {
    // Check correct time format and split into components
    time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

    if (time.length > 1) { // If time format correct
      time = time.slice(1);  // Remove full string match value
      time[5] = +time[0] < 12 ? 'AM' : 'PM'; // Set AM/PM
      time[0] = +time[0] % 12 || 12; // Adjust hours
    }
    return time.join(''); // return adjusted time or original string
  }
}
