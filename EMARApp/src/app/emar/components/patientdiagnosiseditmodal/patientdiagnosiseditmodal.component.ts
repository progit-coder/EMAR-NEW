
import { of as observableOf, Subject, Observable } from 'rxjs';

import { catchError, switchMap, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ICD10 } from './../../../models/allergyandicd.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators, RequiredValidator } from '@angular/forms';
import { Diagnosis } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { AlertService } from '../../../_services';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-patientdiagnosiseditmodal',
  templateUrl: './patientdiagnosiseditmodal.component.html',
  styleUrls: ['./patientdiagnosiseditmodal.component.css']
})
export class PatientdiagnosiseditmodalComponent implements OnInit {

  @Output() diagnosisInfoResult = new EventEmitter<any>();
  @Input() selectedResident: any;
  @Input() DiagnosisId: any;
  @Input() Pstatus: any;
  public diagnosis: any[];
  public url: string;
  residentId: number;
  template: string;
  icdform: FormGroup;
  public getIcdsearchData: ICD10[];
  public searchTerms = new Subject<string>();
  public flag: boolean = true;
  public diagnosisList;//: Observable<any[]>;
  DiagnosisName: any;
  public Hlsevenconfig: Hlsevenconfigs[] = [];
  public segmentDesc: string = "Diagnosis";
  displayfield: any = {};
  diagnosisInfoObj: Diagnosis;
  icd10Id: number = 0;
  pageConfig = {};
  searchText: string = "";
  pDiagnosisId: number;
  public errormessage1: any;
  public errormessage2: any;
  public errormessage3: any;
  public Approval: any;
  residentStatus: number;
  public result: any
  public allFields:number;
  public requiredErrorMessage:string='';
  public dropdownSettings_Physician={};
  public physiciansdrop:any[]=[];
  public NsId:number;
  public selectedphyItems: any[];
  public defaultPhysicianNPI: any;
  public icdDesc:string;
  public rowformated:any;
  public maxDate:string=this.dateFormatPipe.dateFormat(new Date());
  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService,
    private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.residentStatus = this.Pstatus;
    this.residentId = this.selectedResident;
    this.pDiagnosisId = this.DiagnosisId;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.loadSearchData();
      this.getResidentDiagnosis();
    }
      this.pageConfig = this.persistanceService.getPermissionsByScreen("Diagnosis");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.icdform = new FormGroup({
          ddldiagnosis: new FormControl('', [Validators.required]),
          diagnosisdate: new FormControl(''),
          physicianname: new FormControl('', [Validators.required]),
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
        this.checkDiagnosisStatus(this.pDiagnosisId);
      }
    }
  else
  this.persistanceService.redirectToHomePage();
}
  checkDiagnosisStatus(diagnosisId: any) {
    if (diagnosisId == 0) {
      this.icdform.reset();
      this.errormessage2 = '';
      this.getNurseStationByPId();
    }
    else {
      this.getNurseStationByPId();
    }
  }
  getDiagnosissbyDiagnosisId(ID: number) {
    this.errormessage1 = '';
    this.requiredErrorMessage='';
    this.dataservice.get<Diagnosis>(this.config.Emar_Diagnosis_GetDiagnosisById + ID)
      .subscribe(res => {
        this.Approval = res.PDGOutBoundApproval;
        if (this.Approval == 0 && res.PDiagnosis_Id != 0) {
          this.errormessage2 = "This resident record is pending for admin approval."
        }
        this.fetchData(res);
      },
        error => {
          this.alertService.error(error.message)
        });

  }
  fetchData(res: Diagnosis) {
    this.selectedphyItems=[];
    if(res.PhysicianNPI!=null && this.physiciansdrop.length>0)
    {
    let checkPhyExist = this.physiciansdrop.find(r => r.PhysicianNPI == res.PhysicianNPI); 
    if(checkPhyExist!=undefined)
    {
    this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == res.PhysicianNPI)[0]);
    }
    }
    this.icdform.patchValue({
      ddldiagnosis: res.ICD10_RawFormat,
      diagnosisdate: this.dateFormatPipe.dateFormat(res.DiagnosisDate),
      physicianname:this.selectedphyItems,
    });
    this.pDiagnosisId = res.PDiagnosis_Id;
    this.icd10Id = res.ICD10_Id;
    this.rowformated=res.AltCodingId;
    this.icdDesc =res.ICD10_RawFormat;
  }
  saveDiagnosisForResident() {
    this.ng4LoadingSpinnerService.show();
    this.requiredErrorMessage='';
    this.errormessage3='';
    this.errormessage1='';
    let diagnosisDesc = this.rowformated;
    let result = this.pDiagnosisId == 0 ? this.diagnosis.find(x => x.icd10Id != 0 && x.AltCodingId.replace(/\s/g, '').toLowerCase() === diagnosisDesc.toLowerCase().replace(/\s/g, '')) : this.diagnosis.find(x => x.PDiagnosis_Id != this.pDiagnosisId && x.icd10Id != 0 && x.AltCodingId.replace(/\s/g, '').toLowerCase() === diagnosisDesc.toLowerCase().replace(/\s/g, ''));
    if (result) {
      this.ng4LoadingSpinnerService.hide();
      this.diagnosisInfoResult.emit(-1);
    }
    else if (this.icd10Id == 0) {
      this.errormessage1 = "Please select valid Diagnosis";
      this.ng4LoadingSpinnerService.hide();
    }
    // else if((this.icdform.value.diagnosisdate !=null && this.icdform.value.diagnosisdate !=undefined) && inputDate.setHours(0, 0, 0, 0) > todaysDate.setHours(0, 0, 0, 0))
    // {
    //    // Date equals today's date
    //    this.errormessage3 = "Diagnosis date cannot be future date.!";
    //    this.ng4LoadingSpinnerService.hide();
    // }
    else {
      let phyName=this.icdform.value.physicianname[0].PhysicianFullName.split(',');
      this.diagnosisInfoObj = {
        Patient_Id: this.residentId,
        PDiagnosis_Id: this.pDiagnosisId,
        ICD10_Id: this.icd10Id,
        DCodingType_Id: 1,
        CodingMethod: "ICD10",
        DiagnosisDescription: null,
        AltCodingId: this.rowformated,
        AltCodingText: this.icdDesc,
        AltCodingMethod: "MDDX",
        DiagnosisDate:this.icdform.value.diagnosisdate==null ||this.icdform.value.diagnosisdate==undefined?null: this.icdform.value.diagnosisdate,
        DiagnosisType: null,
        MajorDiagnosticId: null,
        MajorDiagnosticText: null,
        MDCodingSystem: null,
        MDAlternateId: null,
        MDAlternateText: null,
        MDAltCodingSystem: null,
        DiagnosticGroupId: null,
        DiagnosticGroupText: null,
        DGCodingSystem: null,
        DGAlternateId: null,
        DGAlternateText: null,
        DGAltCodingSystem: null,
        DRGApprovalIndicator: null,
        DRGGrouperReviewCode: null,
        OutlierId: null,
        OutlierText: null,
        OutlierCodingSystem: null,
        OAlternateId: null,
        OAlternateText: null,
        OAltCodingSystem: null,
        OutlierDays: null,
        OutlierQuantity: null,
        OutlierDenomination: null,
        PriceType: null,
        FromValue: null,
        ToValue: null,
        RangeId: null,
        RangeText: null,
        RangeCodingSystem: null,
        RangeAltId: null,
        RangeAltText: null,
        RangeAltCodingSystem: null,
        RangeType: null,
        GrouperVersion: null,
        DiagnosisPriority: null,
        DiagnosisClassification: null,
        ConfidentialIndicator: null,
        AttestationDate: null,
        DiagnosisIdentifier:null,
        DiagnosisActionCode: null,
        PhysicianNPI: this.icdform.value.physicianname[0].PhysicianNPI,
        PhysicianLName: phyName[0],
        PhysicianFName: phyName[1],
        PDiagnosis_Status: 1,
        PDiagnosis_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        PDiagnosis_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
        PDGOutBoundFileStatus: 1,
        PDGOutBoundApproval: null,
        PDGOutBoundApprovalBy: null,
        PDGOutBoundApprovalOn: null,
        ICD10_RawFormat: null

      };
      this.dataservice.post(this.config.Emar_AdminApproval_InsertUpdateDiagnosisInfo, this.diagnosisInfoObj)
        .subscribe(res => {
          this.getResidentDiagnosis();
          this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
            .subscribe(res => {
              if (res == 1) {
                this.diagnosisInfoResult.emit(0);
                this.ng4LoadingSpinnerService.hide();
              }
              else {
                this.diagnosisInfoResult.emit(1);
                this.ng4LoadingSpinnerService.hide();
              }
              this.errormessage1 = '';
              this.requiredErrorMessage='';
            }, error => {
              this.alertService.error(error.message)
              this.ng4LoadingSpinnerService.hide();
            });

        }, error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  searchDiagnosis(term: string): void {
   this.icd10Id=0;
    if (term.length > 1 && term.length<=50) {
      this.flag = true;
      this.searchTerms.next(term.replace(/[&\/\\#,+()$~%'":.*?<>{}\s]/g, '"'));
    }
    else {
      this.flag = false;
    }
  }
  loadSearchData() {
    this.diagnosisList = this.searchTerms.pipe(
      debounceTime(300),        // wait for 300ms pause in events
      distinctUntilChanged(),   // ignore if next search term is same as previous
      switchMap(term => term   // switch to new observable each time
        // return the http search observable
        ? this.dataservice.search(this.config.Emar_ICD_GetActiveICD10ByRawFormat + term)
        // or the observable of empty ICDs if no search term
        : observableOf<any[]>([{ "ICD10_Id": 0, "ICD10_Description": "No Record Found" }])),
      catchError(error => {
        // TODO: real error handling
        console.log(error);
        return observableOf<any[]>([]);
      }));
  }
  onselectDiagnosis(item) {
    if (item.ICD10_Id != 0) {
      this.icd10Id = item.ICD10_Id;
      this.DiagnosisName = item.ICD10_Description;
      this.rowformated =item.ICD10_Formatted;
      this.icdDesc=item.ICD10_Description;
      this.errormessage1 = '';
      this.flag = false;
    }
    else {
      return false;
    }
  }
  getResidentDiagnosis() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographicDiagnosis_GetResDiagnosis + this.residentId)
      .subscribe(res => {
        this.diagnosis = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  GetPhysicianDropData() {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetPhysicianDropData +0+ "/" + this.NsId)
      .subscribe(res => {
        this.physiciansdrop = res;
        if (this.defaultPhysicianNPI != null && this.pDiagnosisId==0) {
          this.selectedphyItems.push(this.physiciansdrop.filter(p => p.PhysicianNPI == this.defaultPhysicianNPI)[0]);
        this.icdform.patchValue({
          physicianname: this.selectedphyItems,
        });
      }
      else if(this.pDiagnosisId!=0)
      {
        this.getDiagnosissbyDiagnosisId(this.pDiagnosisId);
      }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  
  getNurseStationByPId() {
    this.selectedphyItems=[];
    this.dataservice.get<any>(this.config.Resident_Demographic_GetNurseStationByPId + this.residentId)
      .subscribe(res => {
       this.NsId = res;
       if(this.pDiagnosisId==0)
       {
       this.getDefaultPhysicianByNSId(this.NsId,this.residentId);
       }
       else if(this.pDiagnosisId!=0)
       {
       this.GetPhysicianDropData();
       }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getDefaultPhysicianByNSId(stationId: number,resId:any) {
    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId+"/"+resId)
      .subscribe(res => {
        this.defaultPhysicianNPI = res.PhysicianNPI;
        this.GetPhysicianDropData();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
}
