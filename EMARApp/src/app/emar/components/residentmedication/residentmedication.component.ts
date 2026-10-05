import { Component, OnInit, resolveForwardRef, SimpleChanges, Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { ResidentMedications } from '../../../models/residentdemographic.model';
import { LiteralOders, Medications } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens,Activity } from '../../../models/useractivity.model';

@Component({
  selector: 'app-residentmedication',
  templateUrl: './residentmedication.component.html',
  styleUrls: ['./residentmedication.component.css'],
  providers: [DataService, APIConfiguration, AlertService]
})
export class ResidentmedicationComponent implements OnInit {

  public medications: ResidentMedications[];
  public Hlsevenconfig: Hlsevenconfigs[];
  displayfield: any = {};
  public residentId: number;
  medicationsObj: ResidentMedications;
  public segmentDesc: string = "Treatment";
  public template;
  myform: FormGroup;
  searchText: string = "";
  @Input() selectedResident: any;
  TableName = "TreatmentInfo";
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  public modalEditIsOpen: boolean = false;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private persistanceService: PersistanceService,
    private sharedService: SharedService) {
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes.selectedResident.currentValue!=0 && changes.selectedResident.currentValue != undefined) {
      this.residentId = this.selectedResident;
      this.template = this.dataservice.template;
      this.getHlsevenconfigData();
      this.ng4LoadingSpinnerService.show();
      this.getMedicationsData();
    }
  }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    if (this.residentId != 0 && this.residentId != undefined) {
      this.getHlsevenconfigData();
      this.ng4LoadingSpinnerService.show();
      //this.GetDemographicInfoData();
      // this.getResidentMedicationsData();
      this.getMedicationsData();
    }
    this.userActivity();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.Medication,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  getHistoryById(PatientId: number) {
    this.auditTable = {
      "tableName": "TreatmentInfo",
      "recordId": PatientId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  getHlsevenconfigData() {

    this.dataservice.get<Hlsevenconfigs[]>(this.config.Emar_HlSevenSegment_GetHlSevenConfigs + this.residentId + "/" + this.segmentDesc)
      .subscribe(res => {
        this.Hlsevenconfig = res;
        for (let index = 0; index < this.Hlsevenconfig.length; index++) {
          this.displayfield[this.Hlsevenconfig[index].FieldName] = this.Hlsevenconfig[index].IsMandatory;
        }
        this.myform = new FormGroup({
          PTreatment_Id: new FormControl('', Validators.required),
          POrder_Id: new FormControl('', Validators.required),
          ReqGiveCodeIdentifier: new FormControl('', Validators.required),
          RequestedGiveCode: new FormControl('', Validators.required),
          RequestedGiveAmtMin: new FormControl('', Validators.required),
          RequestedGiveAmtMax: new FormControl('', Validators.required),
          RequestedGiveUnits: new FormControl('', Validators.required),
          RequestedDosageForm: new FormControl('', Validators.required),
          ProvidersTreatmentInstructions: new FormControl('', Validators.required),
          ProvidersAdministrationInstructions: new FormControl('', Validators.required),
          DeliverToLocation: new FormControl('', Validators.required),
          AllowSubstitutions: new FormControl('', Validators.required),
          RequestedDispenseCode: new FormControl('', Validators.required),
          RequestedDispenseAmount: new FormControl('', Validators.required),
          RequestedDispenseUnits: new FormControl('', Validators.required),
          NumberOfRefills: new FormControl('', Validators.required),
          OrderingProviderDEANumber: new FormControl('', Validators.required),
          TreatmentSupplierVerifierID: new FormControl('', Validators.required),
          NeedsHumanReview: new FormControl('', Validators.required),
          RequestedGivePer: new FormControl('', Validators.required),
          RequestedGiveStrength: new FormControl('', Validators.required),
          RequestedGiveStrengthUnits: new FormControl('', Validators.required),
          IndicationIdentifier: new FormControl('', Validators.required),
          IndicationText: new FormControl('', Validators.required),
          IndicationCodingSystem: new FormControl('', Validators.required),
          AIndicationIdentifier: new FormControl('', Validators.required),
          AIndicationText: new FormControl('', Validators.required),
          AIndicationCodingSystem: new FormControl('', Validators.required),
          RequestedGiveRateAmount: new FormControl('', Validators.required),
          RequestedGiveRateUnits: new FormControl('', Validators.required),
          TotalDailyDose: new FormControl('', Validators.required),
          SupplementaryCode: new FormControl('', Validators.required),
          RequestedDrugStrengthVol: new FormControl('', Validators.required),
          RequestedDrugStrengthVolUnits: new FormControl('', Validators.required),
          PharmacyOrderType: new FormControl('', Validators.required),
          DispensingInterval: new FormControl('', Validators.required),

        });
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        })

  }
  getMedicationsData() {
    this.dataservice.get<ResidentMedications[]>(this.config.Emar_ResidentDemographicDiagnosis_GetMedications + this.residentId)
      .subscribe(res => {
        this.medications = res;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getResidentMedicationsData(TreatmentId: number) {

    this.dataservice.get<ResidentMedications>(this.config.Emar_Medications_GetResidentMedicationsData + TreatmentId)
      .subscribe(res => this.fetchData(res),

        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  fetchData(res: ResidentMedications) {
    if (res != null) {
      this.myform.patchValue({
        // SetID:res.SetID,
        PTreatment_Id: res.PTreatment_Id,
        POrder_Id: res.POrder_Id,
        ReqGiveCodeIdentifier: res.ReqGiveCodeIdentifier,
        RequestedGiveCode: res.RequestedGiveCode,
        RequestedGiveAmtMin: res.RequestedGiveAmtMin,
        RequestedGiveAmtMax: res.RequestedGiveAmtMax,
        RequestedGiveUnits: res.RequestedGiveUnits,
        RequestedDosageForm: res.RequestedDosageForm,
        ProvidersTreatmentInstructions: res.ProvidersTreatmentInstructions,
        ProvidersAdministrationInstructions: res.ProvidersAdministrationInstructions,
        DeliverToLocation: res.DeliverToLocation,
        AllowSubstitutions: res.AllowSubstitutions,
        RequestedDispenseCode: res.RequestedDispenseCode,
        RequestedDispenseAmount: res.RequestedDispenseAmount,
        RequestedDispenseUnits: res.RequestedDispenseUnits,
        NumberOfRefills: res.NumberOfRefills,
        OrderingProviderDEANumber: res.OrderingProviderDEANumber,
        TreatmentSupplierVerifierID: res.TreatmentSupplierVerifierID,
        NeedsHumanReview: res.NeedsHumanReview,
        RequestedGivePer: res.RequestedGivePer,
        RequestedGiveStrength: res.RequestedGiveStrength,
        RequestedGiveStrengthUnits: res.RequestedGiveStrengthUnits,
        IndicationIdentifier: res.IndicationIdentifier,
        IndicationText: res.IndicationText,
        IndicationCodingSystem: res.IndicationCodingSystem,
        AIndicationIdentifier: res.AIndicationIdentifier,
        AIndicationText: res.AIndicationText,
        AIndicationCodingSystem: res.AIndicationCodingSystem,
        RequestedGiveRateAmount: res.RequestedGiveRateAmount,
        RequestedGiveRateUnits: res.RequestedGiveRateUnits,
        TotalDailyDose: res.TotalDailyDose,
        SupplementaryCode: res.SupplementaryCode,
        RequestedDrugStrengthVol: res.RequestedDrugStrengthVol,
        RequestedDrugStrengthVolUnits: res.RequestedDrugStrengthVolUnits,
        PharmacyOrderType: res.PharmacyOrderType,
        DispensingInterval: res.DispensingInterval,
      });
    }
    this.modalEditIsOpen = true;
  }
  EditModalClose() {
    this.modalEditIsOpen = false;
  }
  insertResidentMedicationsdetails() {

    this.medicationsObj =
      {
        PTreatment_Id: this.myform.value.PTreatment_Id,
        POrder_Id: this.myform.value.POrder_Id,
        ReqGiveCodeIdentifier: this.myform.value.ReqGiveCodeIdentifier,
        RequestedGiveCode: this.myform.value.RequestedGiveCode,
        RequestedGiveAmtMin: this.myform.value.RequestedGiveAmtMin,
        RequestedGiveAmtMax: this.myform.value.RequestedGiveAmtMax,
        RequestedGiveUnits: this.myform.value.RequestedGiveUnits,
        RequestedDosageForm: this.myform.value.RequestedDosageForm,
        ProvidersTreatmentInstructions: this.myform.value.ProvidersTreatmentInstructions,
        ProvidersAdministrationInstructions: this.myform.value.ProvidersAdministrationInstructions,
        DeliverToLocation: this.myform.value.DeliverToLocation,
        AllowSubstitutions: this.myform.value.AllowSubstitutions,
        RequestedDispenseCode: this.myform.value.RequestedDispenseCode,
        RequestedDispenseAmount: this.myform.value.RequestedDispenseAmount,
        RequestedDispenseUnits: this.myform.value.RequestedDispenseUnits,
        NumberOfRefills: this.myform.value.NumberOfRefills,
        OrderingProviderDEANumber: this.myform.value.OrderingProviderDEA,
        TreatmentSupplierVerifierID: this.myform.value.TreatmentSupplierVerifierID,
        NeedsHumanReview: this.myform.value.NeedsHumanReview,
        RequestedGivePer: this.myform.value.RequestedGivePer,
        RequestedGiveStrength: this.myform.value.RequestedGiveStrength,
        RequestedGiveStrengthUnits: this.myform.value.RequestedGiveStrengthUnits,
        IndicationIdentifier: this.myform.value.IndicationIdentifier,
        IndicationText: this.myform.value.IndicationText,
        IndicationCodingSystem: this.myform.value.IndicationCodingSystem,
        AIndicationIdentifier: this.myform.value.AIndicationIdentifier,
        AIndicationText: this.myform.value.AIndicationText,
        AIndicationCodingSystem: this.myform.value.AIndicationCodingSystem,
        RequestedGiveRateAmount: this.myform.value.RequestedGiveRateAmount,
        RequestedGiveRateUnits: this.myform.value.RequestedGiveRateUnits,
        TotalDailyDose: this.myform.value.TotalDailyDose,
        SupplementaryCode: this.myform.value.SupplementaryCode,
        RequestedDrugStrengthVol: this.myform.value.RequestedDrugStrengthVol,
        RequestedDrugStrengthVolUnits: this.myform.value.RequestedDrugStrengthVolUnits,
        PharmacyOrderType: this.myform.value.PharmacyOrderType,
        DispensingInterval: this.myform.value.DispensingInterval,
        PTreatment_Status: 1,
        PTreatment_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        PTreatment_CreatedDate: new Date().toISOString(),
        PTIOutBoundFileStatus: 1,
        PTIOutBoundApproval: null,
        PTIOutBoundApprovalBy: null,
        PTIOutBoundApprovalOn: null,
      }
    this.dataservice.post(this.config.Emar_Medications_InsertResidentMedicationsData, this.medicationsObj)
      .subscribe(res => {
        this.alertService.success("Save successful");
        // this.getResidentMedicationsData();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
    });
  }
}
