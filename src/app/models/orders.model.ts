export class FrequencyMasterData {
    Frequency_Id: number;
    Frequency_Code: string;
    Frequency_Shortname: string;
    Frequency_Description: string;
    Frequency_Status: number;
    Frequency_CreatedBy: number;
    Frequency_CreatedDate: string;
    Frequency_PRN:number;
    Freq_Times:any;
    Freq_Descalert:string;
    Freq_Descalert1:string;
}
export class FrequencyMasterDataWithShifts {
    Frequency_Id: string;
    Frequency_Name: string;
    Frequency_PRN:number;
    Freq_Group:string;  
    Freq_Times:any;
    Freq_Descalert:string;
    Freq_Descalert1:string;
}
export class WeekMasterData {
    Week_Id: number;
    Week_Desc: string;
    Week_Status: number;
    Week_CreatedBy: number;
    Week_CreatedDate: string;
}
export class MonthMasterData {
    Month_Id: number;
    Month_Name: string;
    Month_Status: number;
    Month_CreatedBy: number;
    Month_CreatedDate: string;
}
export class HoursMasterData {
    Hour_Id: number;
    Hour_Desc: string;
    Hour_Status: number;
    Hour_CreatedBy: number;
    Hour_Createddate: string;
}
export class TimeFormatMasterData {
    TimeFormat_Id: number;
    TimeFormat_Desc: string;
    TimeFormat_Status: number;
    TimeFormat_CreatedBy: number;
    TimeFormat_CreatedDate: string;
}
export class DrugAdministrationTime {
    DAdminId: number;
    POrderId: number;
    AdministrationType: number;
    NursingFreqId: number;
    HourId: string;
    TimeFormatId: number;
    Hours: number;
    Monday: boolean;
    Tuesday: boolean;
    Wednesday: boolean;
    Thursday: boolean;
    Friday: boolean;
    Saturday: boolean;
    Sunday: boolean;
    WeekId: string;
    MonthId: string;
    OnlyOnDay: number;
    ThroughDay: number;
    ActiveDays: number;
    HoldDays: number;
    DAdminStatus: number;
    DAdminCreatedBy: number;
    DAdminCreatedDate: string;
    ActiveDay: string;
    Days: string;
    HoursList: any;
    NurseShiftsId: string;
    Freq_Group:string
}
export class ResidentOrder {
    ResOrder_Id: number;
    Patient_ID: number;
    ResOrderType: string;
    ResOrderDate: string;
    ResPhysician: string;
    ResOrderText: string;
    ResOrder_Status: number;
    ResOrder_CreatedBy: number;
    ResOrder_CreatedDate: string;
    PhysicianName: string;
}
export class InsertOrders {
    ResName: string;
    ResidentID: number;
    DOB: string;
    AdmitDate: string;
    PhyName: string;
    Drug: string;
    Quantity: string;
    RouteCode: string;
    AdditionalInst: string;
    NumberofRefill: string;
    Inhand: string;
    StartDate: string;
    EndDate: string;
    Diagnosis: string;
    OrderID: number;
    Status: number;
    CreatedBy: number;
    AlertText: string;
    MaxPerdays: number;
    OrderStockFlag: number;
    PRNFlag: number;
    SelfAdministeredFlag: number;
    TreatmentFlag: number;
    Maysubstitute: number;
    newOrderFlag: number;
}
export class BarcodeDetail {
    PBarcode_Id: number;
    POrder_Id: number;
    BarcodeDetail1: string;
    PBarcode_Status: number;
    PBarcode_CreatedBy: number;
    PBarcode_CreatedDate: string;
}
export class OrderFavourite {
    OrderFavourite_ID: number;
    PQuantity_Id: number;
    OrderFavMaster_ID: number;
    OrderFavourite_Status: number;
    OrderFavourite_Createby: number;
    OrderFavourite_CreatedOn: string;
    FavListCheckFlag:number;
}
export class OrderInfoAlert {
    Allergy: string;
    Diet: string;
    Diagnosis: string;
}
export class OrderHold {
    OrderHold_Id: number;
    PQuantity_Id: number;
    HoldFrom: string;
    HoldTo: string;
    OrderHold_Status: number;
    OrderHold_CreatedBy: number;
    OrderHold_CreatedDate: string;
    HoldReason: string;

}
export class PhysicianDetails {
    Physician_Id: number;
    PhysicianNPI: string;
    PhysicianLName: string;
    PhysicianFName: string;
    PhysicianFullName: string;
    Credentials: number;
    PStatus: number;
    SPhyStatus: number;
    SphyName: string;
    SPhy:string;
    CredeValue:string;
}
export class OrdersGrid {
    POrder_Id: number;
    GiveCodeText: string;
    GiveDosageForm: string;
    ProviderAdminDrugInsText: string;
    OrderControl: string;
    OrderStockFlag: string;
}
export class OrderDestroy {
    OrderDestroy_Id: number;
    PQuantity_Id: number;
    Quantity: string;
    Reason: string;
    DestroyerUserId: number;
    ApprovalUserId: number;
    OrderDestroy_Status: number;
    OrderDestroy_CreatedBy: number;
    OrderDestroy_CreatedOn: string;
    DUserName: string;
    DPassword: string;
    AUserName: string;
    APassword: string;
}
export class OrderApproval {
    PApprovalOrder_Id: number;
    Porder_Id: number;
    Patient_Id: number;
    OrderingPhysicianID: number;
    OrderControl: string;
    OrderTypeID: number;
    TransactionDate: string;
    OrderEffectiveDate: string;
    POrder_Status: number;
    POrder_CreatedBy: number;
    AlertText: string;
    MaxPerdays: number;
    OrderStockFlag: boolean;
    PRNFlag: boolean;
    ControlSubstanceBit: number;
    SelfAdministeredFlag: boolean;
    TreatmentFlag: boolean;
    Maysubstitute: boolean;
    InsulinComments: string;
    RouteCode: string;
    Quantity: string;
    StartDate: string;
    EndDate: string;
    RequestedGiveCode: string;
    GiveCodeText: string;
    ProviderAdminDrugInsText: string;
    NumberOfRefills: string;
    //NumberOfRefillsRemaining: string;
    POOutBoundFileStatus: number;
    POOutBoundApproval: number;
    POOutBoundApprovalBy: number;
    POOutBoundApprovalOn: string;
    POrder_CreatedDate: string;
    InHand: number;
    AdministrationType: number;
    NursingFreq_Id: number;
    Hour_Id: string;
    Hours: number;
    Monday: boolean;
    Tuesday: boolean;
    Wednesday: boolean;
    Thursday: boolean;
    Friday: boolean;
    Saturday: boolean;
    Sunday: boolean;
    Week_Id: string;
    Month_Id: string;
    OnlyOnDay: number;
    ThroughDay: number;
    ActiveDays: number;
    HoldDays: number;
    Stock_Id: number;
    Barcode: string;
    Days: string;
    Favourites: string;
    NurseShifts_Id: string;
    ScheduleText:string;
}
//CPOE New Order
export class CPOEOrderApproval {
    PApprovalOrder_Id: number;
    Porder_Id: number;
    Patient_Id: number;
    OrderingPhysicianID: number;
    OrderControl: string;
    OrderTypeID: number;
    TransactionDate: string;
    OrderEffectiveDate: string;
    POrder_Status: number;
    POrder_CreatedBy: number;
    AlertText: string;
    MaxPerdays: number;
    OrderStockFlag: boolean;
    PRNFlag: boolean;
    ControlSubstanceBit: number;
    SelfAdministeredFlag: boolean;
    TreatmentFlag: boolean;
    Maysubstitute: boolean;
    InsulinComments: string;
    RouteCode: string;
    Quantity: string;
    StartDate: string;
    EndDate: string;
    RequestedGiveCode: string;
    GiveCodeText: string;
    ProviderAdminDrugInsText: string;
    NumberOfRefills: string;
    //NumberOfRefillsRemaining: string;
    POOutBoundFileStatus: number;
    POOutBoundApproval: number;
    POOutBoundApprovalBy: number;
    POOutBoundApprovalOn: string;
    POrder_CreatedDate: string;
    InHand: number;
    AdministrationType: number;
    NursingFreq_Id: number;
    Hour_Id: string;
    Hours: number;
    Monday: boolean;
    Tuesday: boolean;
    Wednesday: boolean;
    Thursday: boolean;
    Friday: boolean;
    Saturday: boolean;
    Sunday: boolean;
    Week_Id: string;
    Month_Id: string;
    OnlyOnDay: number;
    ThroughDay: number;
    ActiveDays: number;
    HoldDays: number;
    Stock_Id: number;
    Barcode: string;
    Days: string;
    Favourites: string;
    NurseShifts_Id: string;
    ScheduleText:string;
    Notes:string;
    Daw:number;
    DispenseQty:number;
    WrittenDate:string;
    DiagIndication:number;
    WaitforPharmacy:number;
    Hospice:number;
    UserName:string;
    Password:string;
    DiagIndicationText:string;
    Source:number;
    UOM:number;
    Sig2Quantity:string;
    Sig2NursingFreq_Id:number;
    Sig2NurseShifts_Id:string;
    Sig2AddInsText:string;
    Sig2PRNFlag:boolean;
    Sig2MaxPerdays:number;
    Sig3Quantity:string;
    Sig3NursingFreq_Id:number;
    Sig3NurseShifts_Id:string;
    Sig3AddInsText:string;
    Sig3PRNFlag:boolean;
    Sig3MaxPerdays:number;
    Sig4Quantity:string;
    Sig4NursingFreq_Id:number;
    Sig4NurseShifts_Id:string;
    Sig4AddInsText:string;
    Sig4PRNFlag:boolean;
    Sig4MaxPerdays:number;
DUom:number;
Sig2DUom:number;
Sig3DUom:number;
Sig4DUom:number;
PharmacyName: string;
Dayssupply:string;

}
//CPOE Orders Data
export class CPOEOrdersData{
    OrderID: number;
    OrderingPhysicianID: number;
    DrugName: string;
    Quantity: string;
    Directions: string;
    StartDate: string;
    EndDate: string;
    Refill: string;
    MaxPerdays: number;
    AlertText: string;
    InsulinComments: string;
    OrderStockFlag: boolean;
    PRNFlag: boolean;
    TreatmentFlag: boolean;
    SelfAdministeredFlag: boolean;
    Favouriteflag: number;
    Route: string;
    Barcode: string;
    Schedule: string;
    Split: number;
    HoldStatus: number;
    POrderStatus: number;
    DestroyStatus: number;
    PQuantityId: number;
    Inhand: string;
    ControlSubCreatedBy: number;
    ControlSubstanceBit: number;
    Notes:string;
    Daw:number;
    DispenseQty:number;
    DiagIndication:string;
    WaitforPharmacy:number;
    Hospice:number;
    WrittenDate:string;
    Source:number;
    UOM:number;
    DiagIndicationText:string
    DUom:number;

}
//#region 
//created by:sampath
export class OrdersData {
    OrderID: number;
    OrderingPhysicianID: number;
    DrugName: string;
    Quantity: string;
    Directions: string;
    StartDate: string;
    EndDate: string;
    Refill: string;
    MaxPerdays: number;
    AlertText: string;
    InsulinComments: string;
    OrderStockFlag: boolean;
    PRNFlag: boolean;
    TreatmentFlag: boolean;
    SelfAdministeredFlag: boolean;
    Favouriteflag: number;
    Route: string;
    Barcode: string;
    Schedule: string;
    Split: number;
    HoldStatus: number;
    POrderStatus: number;
    DestroyStatus: number;
    PQuantityId: number;
    Inhand: string;
    ControlSubCreatedBy: number;
    ControlSubstanceBit: number;
    WrittenDate:string;
    CpoeFlag:number;
    Refill_Request:number;
    Refill_Note : string;

}
export class CommonOrderStatus {
    OrderId: number;
    PatientId: number;
    DAdminId: number;
    QuantityId: number;
    POrderStatus: number;
    POrderCreatedBy: number;
    POOutBoundApproval: number;
    POOutBoundApprovalBy: number;
    OrderType: string;
    UpdatedOn: string;
}
export class CommonDcOrderStatus {
    OrderId: number;
    PatientId: number;
    DAdminId: number;
    QuantityId: number;
    POrderStatus: number;
    POrderCreatedBy: number;
    POOutBoundApproval: number;
    POOutBoundApprovalBy: number;
    OrderType: string;
    DiscontinueFlag: number;
    DiscontinueReason: string;
    DiscontinuedOn: string;
    DiscontinueAllSplits: number;
    Split: number;
}
export class Orderupdate {
    PatientId: number;
    POrderId: number;
    PQuantityId: number;
    DAdminId: number;
    PhysicianId: number;
    DrugName: string;
    Quantity: string;
    Directions: string;
    StartDate: string;
    EndDate: string;
    NumberofRefills: string;
    MaxPerdays: string;
    Alerttext: string;
    InsulinComments: string;
    OrderstockFlag: boolean;
    PRNFlag: boolean;
    TreatmentFlag: boolean;
    SelfAdministeredFlag: boolean;
    controlSubstance:number;
    Route: number;
    Inhand: string;
    Createdby: number;
    Barcode: string;
    OrderTypeID:number;
    OrderUpdatedOn:string;
    RequestedGiveCode:string;
    ScheduleText:string;
}
export class HOA {
    dadminId: number;
    porderId: number;
    pquantityId: number;
    freqId: number;
    hourId: number;
    hours: number;
    monday: boolean;
    tuesday: boolean;
    wednesday: boolean;
    thursday: boolean;
    friday: boolean;
    saturday: boolean;
    sunday: boolean;
    weekId: string;
    monthId: string;
    days: string;
    createdby: number;
    activedays: number;
    holddays: number;
    hourIds: string;
    NurseStationId: number;
    nurseShiftId: string;
}
export class DrfirstOrderXML {
    DrFirstOrderId: number;
    DrFirstOrderXMLTransApproval: number;
    DrFirstOrderXMLTransApprovalBy: number;
    DrFirstOrderXMLTransApprovalOn: string;
}
//endregion
export class OrdersEndingSoon {
    porder_Id: number;
    PQuantity_Id: number;
}
export class BarcodeEntity {
    PBarcode_Id: number;
    BarcodeDetail1: string;
    GPICode:string;
    Patient_Id:number;
    Facility_Id:number;
}
export class NurseComments {
    Comments_Id: number;
    DrugAdminister_Id: number;
    NurseCommentType_Id: number;
    Comment: string;
    comment_Status: number;
    comment_CreatedBy: number;
    Comment_CreatedOn: string;
    PQuantity_Id: number;
}