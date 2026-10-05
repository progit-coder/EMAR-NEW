import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef} from '@angular/core';
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
  selector: 'app-updateadmitvisitinfo',
  templateUrl: './updateadmitvisitinfo.component.html',
  styleUrls: ['./updateadmitvisitinfo.component.css']
})
export class UpdateadmitvisitinfoComponent implements OnInit {
  @Output() visitInfoData = new EventEmitter<any>();
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
  public allFields:number;
  public defaultPhyFlag:number=0;
  public fetchform: FormGroup;
  public fetchfields: any[];
  public fetchdataFields:any[];
  public fetchfloorIndex:number =0;
  public fetchwingIndex:number=0;
  public fetchroomIndex:number =0;
  public fetchbedIndex:number =0;
  @ViewChild('inputFocus') inputFocus:ElementRef

  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private dateFormatPipe: CustomdatePipe, public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("AdmitVisitInfo");
    this.residentId = this.selectedResident;
    this.visitId = this.PvisitId;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.ng4LoadingSpinnerService.show();
      this.myform = new FormGroup({
        AdmitDate: new FormControl(''),
        physicianname:new FormControl(''),
        DietType: new FormControl('', Validators.maxLength(150)),
      });
      this.fetchform = new FormGroup({
        fetchfields: new FormControl(JSON.stringify(this.fetchfields))
      });
      this.dropdownSettings_Physician = {
        singleSelection: true,
        idField: "PhysicianNPI",
        textField: "PhysicianFullName",
        text: "Select",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: true,
      };
      this.getHlsevenconfigData();
    }
  }
  ngAfterViewInit(){
    setTimeout(() => {
      this.inputFocus.nativeElement.focus()
    }, 300);
  }
  getHlsevenconfigData() {
    this.dataservice.get<any[]>(this.config.Emar_HlSevenConfigs_GetOutboundHLSevenConfigs + this.residentId + "/" + 6)
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
      if (this.displayfield.AdmitDate == 1) {
        const admitValidation = this.myform.get('AdmitDate');
        admitValidation.setValidators([Validators.required]);
        admitValidation.updateValueAndValidity();
      }
      if (this.displayfield.PrimaryPhysicianNPI == 1 || this.displayfield.PrimaryPhysicianLName == 1 || this.displayfield.PrimaryPhysicianFName == 1) {
        const admitValidation = this.myform.get('physicianname');
        admitValidation.setValidators([Validators.required]);
        admitValidation.updateValueAndValidity();
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
          this.ng4LoadingSpinnerService.hide();
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
          this.GetPhysicianDropData(visitObj);
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchData(res: any) {
    this.visitStatus = res.PVisit_Status;
    this.selectedphyItems=[];
    this.Facilityname = '';
    this.NursingStationName = '';
    this.FloorName = '';
    this.WingName = '';
    this.RoomName = '';
    this.BedName = '';
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

      if (res.Floor != null&& this.floors!=undefined && this.floors.length > 0) {
        let checkFlExist = this.floors.find(f => f.Floor_Id === parseInt(res.Floor));
        if (checkFlExist != undefined) {
          this.FloorName = checkFlExist.Floor_Name;
        }
      }
      if (res.Wing != null&& this.wings!=undefined  && this.wings.length > 0) {
        let checkWiExist = this.wings.find(w => w.Wing_Id === res.Wing);
        if (checkWiExist != undefined) {
          this.WingName = checkWiExist.Wing_Desc;
        }
      }
      if (res.Room != null&& this.rooms!=undefined  && this.rooms.length > 0) {
        let checkRoExist = this.rooms.find(r => r.Room_Id === parseInt(res.Room));
        if (checkRoExist != undefined) {
          this.RoomName = checkRoExist.Room_Name;
        }
      }
      if (res.Bed != null && this.beds!=undefined && this.beds.length > 0) {
        let checkBeExist = this.beds.find(b => b.Bed_Id === parseInt(res.Bed));
        if (checkBeExist != undefined) {
          this.BedName = checkBeExist.Bed_Name;
        }
      }
      if(res.PrimaryPhysicianNPI!=null && this.physiciansdrop!=undefined && this.physiciansdrop.length>0)
      {
      let checkPhyExist = this.physiciansdrop.find(r => r.PhysicianNPI == res.PrimaryPhysicianNPI); 
      if(checkPhyExist!=undefined)
      {
      this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == res.PrimaryPhysicianNPI)[0]);
      }
      }
      this.myform.patchValue({
        AdmitDate: res.AdmitDate.substring(0, 10),
        physicianname:this.selectedphyItems,
        DietType: res.DietType,
      });
      this.oldFacility=res.FacilityId;
      this.oldNursestation = res.NursingStationId;
      this.oldFloor = res.Floor;
      this.oldRoom = res.Room;
      this.oldBed = res.Bed;
      this.oldWing = res.Wing;
      this.defaultPhyFlag=res.DefaultPhysicianFlag;
    }
  }
  GetPhysicianDropData(visitObj:any) {
    let nsId=visitObj.NursingStationId
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetPhysicianDropData +0+ "/" + nsId)
      .subscribe(res => {
        this.physiciansdrop = res;
        this.fetchData(visitObj);
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  updateAdmitVisitInfo() {
    debugger
    let phyName=this.myform.value.physicianname[0].PhysicianFullName.split(',');
    let phyId=this.physiciansdrop.find(p=>p.PhysicianNPI==this.myform.value.physicianname[0].PhysicianNPI);
    if (new Date(this.myform.value.AdmitDate).setHours(0,0,0,0) > new Date().setHours(0,0,0,0)) {
      this.errormessage = "Admit date cannot be future date.!";
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
        PrimaryPhysicianNPI: this.myform.value.physicianname[0].PhysicianNPI,
        PrimaryPhysicianLName: phyName[0],
        PrimaryPhysicianFName: phyName[1],
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
        DietType: this.myform.value.DietType,
        ServicingFacility: null,
        BedStatus: null,
        AccStatus: null,
        PendingLocation: null,
        PriorTemporaryLocation: null,
        AdmitDate: this.myform.value.AdmitDate,
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
        Physician_Id: phyId!=undefined?phyId.Physician_Id:null,
        DischargeTime:null,
      }
    this.dataservice.post(this.config.Emar_AdminApproval_InsertResidentAdmitVisitInfoData, this.admitvisitinfoObj)
      .subscribe(res => {
        this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
          .subscribe(approvalFlag => {
            if (approvalFlag == 1) {
              debugger
              //if (res == 1) {
               // this.visitInfoData.emit({ approval: approvalFlag, result: res });
              //}
              // else if (res == 2) {
              //   this.visitInfoData.emit({ approval: approvalFlag, result: res });
              // }
                if (res == 3 || res==4) {
                this.visitInfoData.emit({ approval: approvalFlag, result: res });
               }
            }
            else {
              // if (res == 1) {
              //   this.visitInfoData.emit({ approval: 0, result: res });
              // }
              // else if (res == 2) {
              //   this.visitInfoData.emit({ approval: 0, result: res });
              // }
               if (res == 3 || res==4) {
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
