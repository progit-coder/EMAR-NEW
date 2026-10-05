
export class EmarResident {
    ImageLocation: string;
    Patient_Id: number;
    Resident_Name: string;
    ReviewFlag: number;
    PatientMRNumber: string;
    DOB: string;
    ExternalPatientId: string;
    Total_Count: number;
    Pending_count: number;
    PRN_Drugs: number;
    MedReasonCount: number;
    PrnPending:number;
    Prnmedreasoncount:number;
}
export class NursingSchedule {
    NursingSchedule_Id: number;
    NursingStation_Id: number;
    ScheduleTime: string;
    NursingSchedule_Status: number;
    NursingSchedule_CreatedBy: number;
    NursingSchedule_CreatedDate: string;
}
export class EmarOrdersList {
    // Date: string;
    // Medication: string;
    // Quantity: string;
    // Barcode: string;
    // PorderID: string;
    // OrderInst: string;
    // DrugAdminister_Id: number;
    // Status: string;
    POrder_Id: number;
    pquantity_Id: number;
    //DrugAdminister_Id: number;
    Drug: string;
    Quantity: string;
    Route: string;
    AdditionalInst: string;
    Inhand: string;
    NumberOfRefillsRemaining: string;
    MaxPerday: number;
    Diagnosis: string;
    Barcode: string;
    Last_Modified: string;
    //Administer_Schedule: string;
    Pass_Time: string;
    Last_Passed: string;
    AdministerStatus: string;
    NumberOfRefills: string;
    Last_ModifiedDate: string;
    Last_PassedBy: string;
    Refill_Request: number;
    AlertText: string;
    PsychiatricFlag: number;
    AllergyFlag: number;
    PRNFlag: boolean;
    OrderStockFlag: boolean;
    MedicationReason_Desc: string;
    //NoDueFlag :number;
    ControlSubstanceFlag: number;
    ControlSubstanceCertifiedBy: string;
    ReviewFlag: number;
    dueflag: number;
    InputTime: string;
    ShiftId: number;
    Window: number;
    Ekit:number;
    Undo:number;
    PRNAdministered:number;
    GPI:string;
    DiscardDays:number;
    DiscardDate:string;
    AdministerSites:string;
    ABarcode:string;
    Refill_Note : string;
}
export class DrugAdminister {
    //DrugAdminister_Id: number;
    POrder_Id: number;
    pquantity_Id: number;
    AdminsterSchedule: string;
    AdministerComment: string;
    AdminsterStatus: number;
    AdminsterBy: number;
    AdminsterOn: string;
    MedicationReason_ID: number;
    BCScanner: number;
    BCScannerText: string;
    Ekit_Id: number;
    Quantity: number;
    Patient_Id: number;
    DrugQuantity: number;
    ByPassReason: string;
    InputTime: string;
    ShiftId: number;
    Window: number;
    PRNFlag: boolean;
    UndoFlag:boolean;
    Last_Passed:string;
    AdditionalComments:string;
    AdministeredBarcode:string;
    DiscardDate:string;
    AdministerInsulinSites:string;
    RouteCode:string;
    Ekit_AllIds:any;
}
export class NursingFrequencyConfig {
    NursingFreq_Id: number;
    Facility_Id: number;
    NursingStations: string;
    Times: string;
    // Floor_Id: number;
    // Wing_Id: number;
    Frequency_Id: number;
    StartTime: string;
    TimeFormat_ID: number;
    Hours: number;
    Monday: number;
    Tuesday: number;
    Wednesday: number;
    Thursday: number;
    Friday: number;
    Saturday: number;
    Sunday: number;
    Week_Id: number;
    Month_Id: number;
    OnlyOnDay: number;
    ThroughDay: number;
    ActiveDays: number;
    HoldDays: number;
    NursingFreq_Status: number;
    NursingFreq_CreatedBy: number;
    NursingFreq_CreatedDate: string;
    OldFrequencyId:number;
    OldFacilityId:number;
    OldNursingStationId:number;
}
export class MedicationReason {
    MedicationReason_ID: number;
    MedicationReason_Desc: string;
}
export class Seventytwohourcheck {
    DrugAdminister_Id: number;
    SeventTwoComment: string;
}
export class EncodeDrugDemographic {
    DrugAdminister_Id: number;
    SeventyTwoComment: string;
    SeventyTwoCommentBy: number;
    SeventyTwoCommentOn: string;
}
export class EncodeDrugDemographicDetails {
    AdministerOn: Date;
    PatientLastName: string;
    PatientFirstName: string;
    GiveCodeText: string;
}


export class PRNData {
    PatientName: string;
    AdminsterOn: string;
    AdminsterBy: string;
    AdministerComment: string;
    MedicationReason_Desc: string;
    GiveCodeText: string;
    POrder_Id: number;
    Patient_Id: number;
    PRNControl: PRNDataSave[];

}
export class PRNDataSave {
    DrugAdminister_Id: number;
    PRNComment: string;
    PRNCommentBy: number;
    PRNCommentOn: string;
}
export class ControlSubstanceFilter {
    CompanyId: number;
    CompanyTobedFlag: number;
    Facilities: any[];
    NurseStations: any[];
    Floors: any[];
    Wings: any[];
    Rooms: any[];
    Beds: any[];
    History: number;
    UserName: string;
    Password: string;
    User_Id: number;
    ResidentId: number;
}
export class ControlSubstanceGrid {
    PorderId: number;
    ResidentName: string;
    DOB: string;
    Order: string;
    LastCertified: string;
    CertifiedDate: string;
    Quantity: string;
    Color: string; 
    QuantityId:number;
    Ekit_Id:number   
}
export class ControlSubstanceSave {
    POrderId: number;
    Quantity: string;
    DiscrepancyReason: string;
    ReasonRequired: boolean;
    PQuantity_Id:number;
    eKitFlag:number;
    Ekit_Id:number
}
export class CertifyAndApprovalCheck {
    Cert_UserName: string;
    Cert_Password: string;
    Approval_UserName: string;
    Approval_Password: string;
    ControlOrders: ControlSubstanceSave[];
    User_Id: number;
    Facility_Id: number;
    ApprovedOn: string;
}
export class BypassBiometric {
    PatientID: number;
    Time: number;
    dateValue: string;
    reason: string;
    byPass: number;
}
export class Refill {
    Refill_Id: number;
    Porder_Id: number;
    Patient_Id: number;
    NumberOfRefillsRemaining: string;
    Refill_Status: number;
    Refill_CreatedBy: number;
    Refill_CreatedDate: string;
    POOutBoundFileStatus: number;
    POOutBoundApproval: number;
    POOutBoundApprovalBy: number;
    POOutBoundApprovalOn: string;
}

export class NurseComments {
    Comments_Id: number;
    //DrugAdminister_Id: number;
    POrder_Id: number;
    pquantity_Id: number;
    NurseCommentType_Id: number;
    Comment: string;
    comment_Status: number;
    comment_CreatedBy: number;
    Comment_CreatedOn: string;
}
export class OrderFavouriteData {
    FavouriteData_Id: number;
    POrder_Id: number;
    pquantity_Id: number;
    AdminsterSchedule: string;
    InputTime: string;
    ShiftId: number;
    Window: number;
    OrderFavMaster_ID: number;
    value: string;
    FavouriteData_Status: number;
    FavouriteData_CreatedBy: number;
    Favourite_CreatedOn: string;
}
export class RefillNotes{
    Refill_Note : string;
}
