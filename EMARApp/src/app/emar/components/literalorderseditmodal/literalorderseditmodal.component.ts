import { Component, OnInit,Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { LiteralOdersData } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { LiteralOders } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import {  NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-literalorderseditmodal',
  templateUrl: './literalorderseditmodal.component.html',
  styleUrls: ['./literalorderseditmodal.component.css']
})
export class LiteralorderseditmodalComponent implements OnInit {

  @Input() selectedResident:any;
  @Input() orderType: any;
  @Input() orderId: any;
  @Input() quantityId: any;
  public literalOrderType:any;
  public literalOrderId:any;
  public literalOrderQuantityId:any;
  public Hlsevenconfig: Hlsevenconfigs[];
  public residentId:any;
  displayfield: any = {};
  myform: FormGroup;
  myformEncoded: FormGroup;
  myformQuantity: FormGroup;
  myformRoute: FormGroup;
  myformTreatmentinfo: FormGroup;
  myformAddlIns: FormGroup;
  myformAncillary: FormGroup;
  public segmentDescs: any = ["CommonOrder","EncodedOrder","Quantity","TreatmentRoute","Treatment","AddlInstruction","Ancillary"];
  orderViewData: any;
  

  constructor(private dataservice: DataService, private config: APIConfiguration, private persistanceService: PersistanceService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private sharedService: SharedService,public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.residentId=this.selectedResident;
    if (this.residentId != 0 && this.residentId != undefined) {
      this.getHlsevenconfigData();
      this.literalOrderType=this.orderType;
      this.literalOrderId=this.orderId;
      this.literalOrderQuantityId=this.quantityId;
      this.getLiteralOrderDetailsById();
    }
  }
  getHlsevenconfigData() {
    let configObj={
      ResidentId:this.residentId,
      SegmentDescs:this.segmentDescs
    }
    this.dataservice.post(this.config.Emar_HlSevenConfigs_GetHLSevenConfigsBysegments,configObj)
      .subscribe(res => {        
        this.Hlsevenconfig = res;
        for (let index = 0; index < this.Hlsevenconfig.length; index++) {
          this.displayfield[this.Hlsevenconfig[index].FieldName] = this.Hlsevenconfig[index].IsMandatory;
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        })
        this.myform = new FormGroup({
          POrder_Id: new FormControl('', Validators.required),
          OrderControl: new FormControl('', Validators.required),
          PlacerOrderNumber: new FormControl('', Validators.required),
          FacilityId: new FormControl('', Validators.required),
          PatientId: new FormControl('', Validators.required),
          Room: new FormControl('', Validators.required),
          PlacerGroupNumber: new FormControl('', Validators.required),
          OrderStatus: new FormControl('', Validators.required),
          ResponseFlag: new FormControl('', Validators.required),
          QuantityTiming: new FormControl('', Validators.required),
          Parent: new FormControl('', Validators.required),
          TransactionDate: new FormControl('', Validators.required),
          EnteredBy: new FormControl('', Validators.required),
          EPharmacistLName: new FormControl('', Validators.required),
          EPharmacistFName: new FormControl('', Validators.required),
          VerifiedBy: new FormControl('', Validators.required),
          VPharmacistLName: new FormControl('', Validators.required),
          VPharmacistFName: new FormControl('', Validators.required),
          VEffectivedate: new FormControl('', Validators.required),
          OrderingPhysicianNPI: new FormControl('', Validators.required),
          OPhysicianLname: new FormControl('', Validators.required),
          OPhysicianFname: new FormControl('', Validators.required),
          EntererLocation: new FormControl('', Validators.required),
          CallBackPhoneNumber: new FormControl('', Validators.required),
          OrderEffectiveDate: new FormControl('', Validators.required),
          OrderControlCodeReason: new FormControl('', Validators.required),
          EnteringOrganisation: new FormControl('', Validators.required),
          EnteringDevice: new FormControl('', Validators.required),
          AltCodingSystem: new FormControl('', Validators.required),
          AdvBeneficiaryNoticeCode: new FormControl('', Validators.required),
          OrderingFacilityName: new FormControl('', Validators.required),
          OrderingFacilityAddress1: new FormControl('', Validators.required),
          OrderingFacilityAddress2: new FormControl('', Validators.required),
          OrderingFacilityCity: new FormControl('', Validators.required),
          OrderingFacilityState: new FormControl('', Validators.required),
          OrderingFacilityZip: new FormControl('', Validators.required),
          OrderingFacilityPhone: new FormControl('', Validators.required),
          OrderingPhysicianAddress1: new FormControl('', Validators.required),
          OrderingPhysicianAddress2: new FormControl('', Validators.required),
          OrderingPhysicianCity: new FormControl('', Validators.required),
          OrderingPhysicianState: new FormControl('', Validators.required),
          OrderingProviderZip: new FormControl('', Validators.required),
          OrderStatusModifier: new FormControl('', Validators.required),
          AdvBeneficiaryNoticeOverrideReason: new FormControl('', Validators.required),
          ExpectedAvailabilityDate: new FormControl('', Validators.required),
          ConfidentialityCode: new FormControl('', Validators.required),
          OrderType: new FormControl('', Validators.required),
          EntererAuthorizationMode: new FormControl('', Validators.required),

        });
        this.myformEncoded = new FormGroup({
          PEncOrder_Id: new FormControl('', Validators.required),
          Quantity: new FormControl('', Validators.required),
          GiveCodeIdentifier: new FormControl('', Validators.required),
          GiveCodeText: new FormControl('', Validators.required),
          AGiveCodeIdentifier: new FormControl('', Validators.required),
          GiveAmountMin: new FormControl('', Validators.required),
          GiveAmountMax: new FormControl('', Validators.required),
          GiveUnits: new FormControl('', Validators.required),
          GiveDosageForm: new FormControl('', Validators.required),
          ProviderAdminDrugIdentifier: new FormControl('', Validators.required),
          ProviderAdminDrugInsText: new FormControl('', Validators.required),
          DeliverToLocation: new FormControl('', Validators.required),
          SubstitutionStatus: new FormControl('', Validators.required),
          DispenseAmount: new FormControl('', Validators.required),
          DispenseUnits: new FormControl('', Validators.required),
          NumberOfRefills: new FormControl('', Validators.required),
          PhysicianDEANumber: new FormControl('', Validators.required),
          PhysicianLastName: new FormControl('', Validators.required),
          PhysicianFirstName: new FormControl('', Validators.required),
          TreatmentSupplierVerifierID: new FormControl('', Validators.required),
          PrescriptionNumber: new FormControl('', Validators.required),
          NumberOfRefillsRemaining: new FormControl('', Validators.required),
          NumberOfRefillsDispensed: new FormControl('', Validators.required),
          RecentRefillDate: new FormControl('', Validators.required),
          TotalDailyDose: new FormControl('', Validators.required),
          NeedsHumanReview: new FormControl('', Validators.required),
          SpecialDispensingInstruction: new FormControl('', Validators.required),
          GivePer: new FormControl('', Validators.required),
          GiveRateAmount: new FormControl('', Validators.required),
          GiveRateUnits: new FormControl('', Validators.required),
          GiveStrength: new FormControl('', Validators.required),
          GiveStrengthUnits: new FormControl('', Validators.required),
          GiveIndication: new FormControl('', Validators.required),
          DispensePackageSize: new FormControl('', Validators.required),
          DispensePackageSizeUnit: new FormControl('', Validators.required),
          DispensePackageMethod: new FormControl('', Validators.required),
          SupplementaryCode: new FormControl('', Validators.required),
          OriginalOrderDate: new FormControl('', Validators.required),
          GiveDrugStrengthVolume: new FormControl('', Validators.required),
          GiveDrugStrengthVolUnits: new FormControl('', Validators.required),
          ControlledSubstanceSchedule: new FormControl('', Validators.required),
          FormularyStatus: new FormControl('', Validators.required),
          PharmaceuticalSubstance: new FormControl('', Validators.required),
          PharmacyRecentFill: new FormControl('', Validators.required),
          InitialDispenseAmount: new FormControl('', Validators.required),
          DispensingPharmacyID: new FormControl('', Validators.required),
          DispensingPharmacyName: new FormControl('', Validators.required),
          DispensingPharmacyAddr1: new FormControl('', Validators.required),
          DispensingPharmacyAddr2: new FormControl('', Validators.required),
          DispensingPharmacyCity: new FormControl('', Validators.required),
          DispensingPharmacyState: new FormControl('', Validators.required),
          DispensingPharmacyZip: new FormControl('', Validators.required),
          DeliverToPatientLocation: new FormControl('', Validators.required),
          DeliverToAddress: new FormControl('', Validators.required),
          PharmacyOrderType: new FormControl('', Validators.required),
        });
        this.myformQuantity = new FormGroup({
          PQuantity_Id: new FormControl('', Validators.required),
          Quantity: new FormControl('', Validators.required),
          RepeatPattern: new FormControl('', Validators.required),
          ExplicitTime: new FormControl('', Validators.required),
          RelativeTimeUnits: new FormControl('', Validators.required),
          ServiceDuration: new FormControl('', Validators.required),
          StartDate: new FormControl('', Validators.required),
          EndDate: new FormControl('', Validators.required),
          Priority: new FormControl('', Validators.required),
          ConditionText: new FormControl('', Validators.required),
          TextInstruction: new FormControl('', Validators.required),
          Conjunction: new FormControl('', Validators.required),
          OccuranceDuration: new FormControl('', Validators.required),
          TotalOccurances: new FormControl('', Validators.required),
        });
        this.myformRoute = new FormGroup({
          PRoute_Id: new FormControl('', Validators.required),
          RouteCode: new FormControl('', Validators.required),
          RouteText: new FormControl('', Validators.required),
          AdministrationSite: new FormControl('', Validators.required),
          AdministrationDevice: new FormControl('', Validators.required),
          AdministrationMethod: new FormControl('', Validators.required),
          RoutingInstruction: new FormControl('', Validators.required),
          AdminstrationSiteModifier: new FormControl('', Validators.required),
        });
        this.myformTreatmentinfo = new FormGroup({
          PTreatment_Id: new FormControl('', Validators.required),
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
        this.myformAddlIns = new FormGroup({
          PAddl_Id: new FormControl('', Validators.required),
          PackageType: new FormControl('', Validators.required),
          NumberOfLabels: new FormControl('', Validators.required),
          LabelQuantity: new FormControl('', Validators.required),
          Zone: new FormControl('', Validators.required),
          Bin: new FormControl('', Validators.required),
          TotalQuantityWritten: new FormControl('', Validators.required),
          TotalQuantityDispensed: new FormControl('', Validators.required),
          FillQuantityWritten: new FormControl('', Validators.required),
          MaxDailyQuantity: new FormControl('', Validators.required),
          DaysSupply: new FormControl('', Validators.required),
          TimesPerDay: new FormControl('', Validators.required),
          DateWritten: new FormControl('', Validators.required),
          PrePack: new FormControl('', Validators.required),
          CycleFill: new FormControl('', Validators.required),
          MARGroupLevel: new FormControl('', Validators.required),
          MARGroup: new FormControl('', Validators.required),
          PartialStatus: new FormControl('', Validators.required),
          IntendedQuantity: new FormControl('', Validators.required),
          IntendedDaysSupply: new FormControl('', Validators.required),
          OriginCode: new FormControl('', Validators.required),
          RxGuid: new FormControl('', Validators.required),
          ToteId: new FormControl('', Validators.required),
          CustomFieldID: new FormControl('', Validators.required),
          CustomFieldValue: new FormControl('', Validators.required),
          CustomFieldName: new FormControl('', Validators.required),
          ACustomFieldID: new FormControl('', Validators.required),
          ACustomFieldValue: new FormControl('', Validators.required),
          ACustomFieldName: new FormControl('', Validators.required),
          RxType: new FormControl('', Validators.required),
          LinkedReorderNumber: new FormControl('', Validators.required),
          ExtraDoseIndicator: new FormControl('', Validators.required),
          LeaveOfAbsenceIndicator: new FormControl('', Validators.required),
          DeliveryID: new FormControl('', Validators.required),
          PartialTabletIndicator: new FormControl('', Validators.required),
          RawAdministrationTimes: new FormControl('', Validators.required),
          ProductType: new FormControl('', Validators.required),
          Treatment: new FormControl('', Validators.required),
          PhRxExpireDate: new FormControl('', Validators.required),
          RxNumber: new FormControl('', Validators.required),
        });
        this.myformAncillary = new FormGroup({
          PAnc_Id: new FormControl('', Validators.required),
          LiteralOrderCode: new FormControl('', Validators.required),
          LiteralOrderDesc: new FormControl('', Validators.required),
          MAROrderCatSeq: new FormControl('', Validators.required),
          POOrderCatSeq: new FormControl('', Validators.required),
          TAROrderCatSeq: new FormControl('', Validators.required),
          Filler: new FormControl('', Validators.required),
          IncludeOnMAR: new FormControl('', Validators.required),
          IncludeOnPO: new FormControl('', Validators.required),
          IncludeOnTAR: new FormControl('', Validators.required),
          RawAdministrationTimes: new FormControl('', Validators.required),
        });
  }
  getLiteralOrderDetailsById()
  {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentOrdersView + this.literalOrderType + "/" + this.literalOrderId + "/" + this.literalOrderQuantityId)
      .subscribe(res => {
        this.orderViewData = res;
        this.fetchData(res.CommonOrderInfo);
        this.fetchEncodedDetails(res.EncodedOrderDetail);
        this.fetchQuantityDetails(res.QuantityDetail);
        if(this.literalOrderType==1)
        {
        this.fetchRouteDetails(res.TreatmentRouteInfo);
        this.fetchTreatmentinfoDetails(res.TreatmentInfo);
        this.fetchAddlInsDetails(res.AddlInstructionDetail);
        }
        else
        {
          this.fetchAncillaryDetails(res.AncillaryDetail);
        }

      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  fetchData(res: LiteralOdersData) {
    if (res != null) {
      this.myform.patchValue({
        // SetID:res.SetID,
        POrder_Id: res.POrder_Id,
        OrderControl: res.OrderControl,
        PlacerOrderNumber: res.PlacerOrderNumber,
        FacilityId: res.FacilityId,
        PatientId: res.PatientId,
        Room: res.Room,
        PlacerGroupNumber: res.PlacerGroupNumber,
        OrderStatus: res.OrderStatus,
        ResponseFlag: res.ResponseFlag,
        QuantityTiming: res.QuantityTiming,
        Parent: res.Parent,
        TransactionDate: res.TransactionDate,
        EnteredBy: res.EnteredBy,
        EPharmacistLName: res.EPharmacistLName,
        EPharmacistFName: res.EPharmacistFName,
        VerifiedBy: res.VerifiedBy,
        VPharmacistLName: res.VPharmacistLName,
        VPharmacistFName: res.VPharmacistFName,
        VEffectivedate: res.VEffectivedate,
        OrderingPhysicianNPI: res.OrderingPhysicianNPI,
        OPhysicianLname: res.OPhysicianLname,
        OPhysicianFname: res.OPhysicianFname,
        EntererLocation: res.EntererLocation,
        CallBackPhoneNumber: res.CallBackPhoneNumber,
        OrderEffectiveDate: res.OrderEffectiveDate,
        OrderControlCodeReason: res.OrderControlCodeReason,
        EnteringOrganisation: res.EnteringOrganisation,
        EnteringDevice: res.EnteringDevice,
        AltCodingSystem: res.AltCodingSystem,
        AdvBeneficiaryNoticeCode: res.AdvBeneficiaryNoticeCode,
        OrderingFacilityName: res.OrderingFacilityName,
        OrderingFacilityAddress1: res.OrderingFacilityAddress1,
        OrderingFacilityAddress2: res.OrderingFacilityAddress2,
        OrderingFacilityCity: res.OrderingFacilityCity,
        OrderingFacilityState: res.OrderingFacilityState,
        OrderingFacilityZip: res.OrderingFacilityZip,
        OrderingFacilityPhone: res.OrderingFacilityPhone,
        OrderingPhysicianAddress1: res.OrderingPhysicianAddress1,
        OrderingPhysicianAddress2: res.OrderingPhysicianAddress2,
        OrderingPhysicianCity: res.OrderingPhysicianCity,
        OrderingPhysicianState: res.OrderingPhysicianState,
        OrderingProviderZip: res.OrderingProviderZip,
        OrderStatusModifier: res.OrderStatusModifier,
        AdvBeneficiaryNoticeOverrideReason: res.AdvBeneficiaryNoticeOverrideReason,
        ExpectedAvailabilityDate: res.ExpectedAvailabilityDate,
        ConfidentialityCode: res.ConfidentialityCode,
        OrderType: res.OrderType,
        EntererAuthorizationMode: res.EntererAuthorizationMode,
      });
    }
  }
  fetchEncodedDetails(res:any)
  {
    if (res != null) {
      this.myformEncoded.patchValue({
        PEncOrder_Id: res.PEncOrder_Id,
        Quantity: res.Quantity,
        GiveCodeIdentifier: res.GiveCodeIdentifier,
        GiveCodeText: res.GiveCodeText,
        AGiveCodeIdentifier: res.AGiveCodeIdentifier,
        GiveAmountMin: res.GiveAmountMin,
        GiveAmountMax: res.GiveAmountMax,
        GiveUnits: res.GiveUnits,
        GiveDosageForm: res.GiveDosageForm,
        ProviderAdminDrugIdentifier: res.ProviderAdminDrugIdentifier,
        ProviderAdminDrugInsText: res.ProviderAdminDrugInsText,
        DeliverToLocation: res.DeliverToLocation,
        SubstitutionStatus: res.SubstitutionStatus,
        DispenseAmount: res.DispenseAmount,
        DispenseUnits: res.DispenseUnits,
        NumberOfRefills: res.NumberOfRefills,
        PhysicianDEANumber: res.PhysicianDEANumber,
        PhysicianLastName: res.PhysicianLastName,
        PhysicianFirstName: res.PhysicianFirstName,
        TreatmentSupplierVerifierID: res.TreatmentSupplierVerifierID,
        PrescriptionNumber: res.PrescriptionNumber,
        NumberOfRefillsRemaining: res.NumberOfRefillsRemaining,
        NumberOfRefillsDispensed: res.NumberOfRefillsDispensed,
        RecentRefillDate: res.RecentRefillDate,
        TotalDailyDose: res.TotalDailyDose,
        NeedsHumanReview: res.NeedsHumanReview,
        SpecialDispensingInstruction: res.SpecialDispensingInstruction,
        GivePer: res.GivePer,
        GiveRateAmount: res.GiveRateAmount,
        GiveRateUnits: res.GiveRateUnits,
        GiveStrength: res.GiveStrength,
        GiveStrengthUnits: res.GiveStrengthUnits,
        GiveIndication: res.GiveIndication,
        DispensePackageSize: res.DispensePackageSize,
        DispensePackageSizeUnit: res.DispensePackageSizeUnit,
        DispensePackageMethod: res.DispensePackageMethod,
        SupplementaryCode: res.SupplementaryCode,
        OriginalOrderDate: res.OriginalOrderDate,
        GiveDrugStrengthVolume: res.GiveDrugStrengthVolume,
        GiveDrugStrengthVolUnits: res.GiveDrugStrengthVolUnits,
        ControlledSubstanceSchedule: res.ControlledSubstanceSchedule,
        FormularyStatus: res.FormularyStatus,
        PharmaceuticalSubstance: res.PharmaceuticalSubstance,
        PharmacyRecentFill: res.PharmacyRecentFill,
        InitialDispenseAmount: res.InitialDispenseAmount,
        DispensingPharmacyID: res.DispensingPharmacyID,
        DispensingPharmacyName: res.DispensingPharmacyName,
        DispensingPharmacyAddr1: res.DispensingPharmacyAddr1,
        DispensingPharmacyAddr2: res.DispensingPharmacyAddr2,
        DispensingPharmacyCity: res.DispensingPharmacyCity,
        DispensingPharmacyState: res.DispensingPharmacyState,
        DispensingPharmacyZip: res.DispensingPharmacyZip,
        DeliverToPatientLocation: res.DeliverToPatientLocation,
        DeliverToAddress: res.DeliverToAddress,
        PharmacyOrderType: res.PharmacyOrderType,
      });
    }
  }
  fetchQuantityDetails(res:any)
  {
    if (res != null) {
      this.myformQuantity.patchValue({
        PQuantity_Id: res.PQuantity_Id,
        Quantity: res.Quantity,
        RepeatPattern: res.RepeatPattern,
        ExplicitTime: res.ExplicitTime,
        RelativeTimeUnits: res.RelativeTimeUnits,
        ServiceDuration: res.ServiceDuration,
        StartDate: res.StartDate,
        EndDate: res.EndDate,
        Priority: res.Priority,
        ConditionText: res.ConditionText,
        TextInstruction: res.TextInstruction,
        Conjunction: res.Conjunction,
        OccuranceDuration: res.OccuranceDuration,
        TotalOccurances: res.TotalOccurances,
      });
    }
  }
  fetchRouteDetails(res:any)
  {
    if (res != null) {
      this.myformRoute.patchValue({
        PRoute_Id: res.PRoute_Id,
        RouteCode: res.RouteCode,
        RouteText: res.RouteText,
        AdministrationSite: res.AdministrationSite,
        AdministrationDevice: res.AdministrationDevice,
        AdministrationMethod: res.AdministrationMethod,
        RoutingInstruction: res.RoutingInstruction,
        AdminstrationSiteModifier: res.AdminstrationSiteModifier,
      });
    }
  }
  fetchTreatmentinfoDetails(res:any)
  {
    if (res != null) {
      this.myformTreatmentinfo.patchValue({
        PTreatment_Id: res.PTreatment_Id,
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
  }
  fetchAddlInsDetails(res:any)
  {
    if (res != null) {
      this.myformAddlIns.patchValue({
        PAddl_Id: res.PAddl_Id,
        PackageType: res.PackageType,
        NumberOfLabels: res.NumberOfLabels,
        LabelQuantity: res.LabelQuantity,
        Zone: res.Zone,
        Bin: res.Bin,
        TotalQuantityWritten: res.TotalQuantityWritten,
        TotalQuantityDispensed: res.TotalQuantityDispensed,
        PAdFillQuantityWrittendl_Id: res.FillQuantityWritten,
        MaxDailyQuantity: res.MaxDailyQuantity,
        DaysSupply: res.DaysSupply,
        TimesPerDay: res.TimesPerDay,
        DateWritten: res.DateWritten,
        PrePack: res.PrePack,
        CycleFill: res.CycleFill,
        MARGroupLevel: res.MARGroupLevel,
        MARGroup: res.MARGroup,
        PartialStatus: res.PartialStatus,
        IntendedQuantity: res.IntendedQuantity,
        IntendedDaysSupply: res.IntendedDaysSupply,
        OriginCode: res.OriginCode,
        RxGuid: res.RxGuid,
        ToteId: res.ToteId,
        CustomFieldID: res.CustomFieldID,
        CustomFieldValue: res.CustomFieldValue,
        CustomFieldName: res.CustomFieldName,
        ACustomFieldID: res.ACustomFieldID,
        ACustomFieldValue: res.ACustomFieldValue,
        ACustomFieldName: res.ACustomFieldName,
        RxType: res.RxType,
        LinkedReorderNumber: res.LinkedReorderNumber,
        ExtraDoseIndicator: res.ExtraDoseIndicator,
        LeaveOfAbsenceIndicator: res.LeaveOfAbsenceIndicator,
        DeliveryID: res.DeliveryID,
        PartialTabletIndicator: res.PartialTabletIndicator,
        RawAdministrationTimes: res.RawAdministrationTimes,
        ProductType: res.ProductType,
        Treatment: res.Treatment,
        PhRxExpireDate: res.PhRxExpireDate,
        RxNumber: res.RxNumber,
      });
    }
  }
  fetchAncillaryDetails(res:any)
  {
    if (res != null) {
      this.myformAncillary.patchValue({
        PAnc_Id: res.PAnc_Id,
        LiteralOrderCode: res.LiteralOrderCode,
        LiteralOrderDesc: res.LiteralOrderDesc,
        MAROrderCatSeq: res.MAROrderCatSeq,
        POOrderCatSeq: res.POOrderCatSeq,
        TAROrderCatSeq: res.TAROrderCatSeq,
        Filler: res.Filler,
        IncludeOnMAR: res.IncludeOnMAR,
        IncludeOnPO: res.IncludeOnPO,
        IncludeOnTAR: res.IncludeOnTAR,
        RawAdministrationTimes: res.RawAdministrationTimes,
      });
    }
  }
}
