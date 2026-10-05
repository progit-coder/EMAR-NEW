
export class Gender {
    Gender_Id: number;
    Gender_Desc: string;
}

export class Ethnic {
    Ethnic_Id: number;
    Ethnic_OriginShortCode: string;
}
export class OrderFavConfig
{
    facility_Id:number;
    orderFavCode:string;
    orderFavMaster:string;
    oFConfig_Status:number;
    oFConfig_CreatedBy:number;
    oFConfig_CreatedDate:string;
    events:number;
}
export class OrderDetailsBYId
{
    FacilityId:number;
    Code:string;
    CreatedDate:string;
}
export class Race {
    Race_Id: number;
    Race_Desc: string;
}

export class MaritalStatus {
    Marital_id: number;
    Marital_Desc: string;
}
export class MilitaryService {
    Military_Id: number;
    Military_Desc: string;
}
export class Suffix {
    Suffix_Id: number;
    Suffix_Desc: string;
}
export class Language {
    Language_Id: number;
    Language_Code: string;
}
export class Country {
    Country_Id: number;
    Country_Code: string;
}

export class MBICheck {
    MBICheck_Id: number;
    MBICheck_Desc: string;
}

export class ResponsibleParty {
    Resparty_Id: number;
    Resparty_Desc: string;
}

export class AdvDirective {
    AdvDirectives_Id: number;
    AdvDirectives_Desc: string;
}

export class Mobility {
    Mobility_Id: number;
    Mobility_Type: string;
}
export class Nutrition {
    Nutrition_Id: number;
    Nutrition_Type: string;
}
export class Fricition {
    Friction_Id: number;
    Friction_Type: string;
}
export class Sensory {
    Sensory_Id: number;
    Sensory_Type: string;
}
export class Skin {
    Skin_Id: number;
    Skin_Type: string;
}
export class Activity {
    Activity_Id: number;
    Activity_Type: string;
}
export class ProfessionalContactMaster {
    ProfContact_Id: number;
    ProfContact_Desc: string;
}
export class State {
    State_Id: number;
    State_Code: string;
}
export class City {
    CIty_Id: number;
    City_Name: string;
}
export class ZipCode {
    Zip_Id: number;
    Zip_Code: string;
}
export enum Months {
    Jan = 1,
    Feb = 2,
    Mar = 3,
    Apr = 4,
    May = 5,
    Jun = 6,
    Jul = 7,
    Aug = 8,
    Sep = 9,
    Oct = 10,
    Nov = 11,
    Dec = 12
}
export class PatientType {
    PatientType_Id: number;
    Company_Id: number;
    Color_Code: string;
    Color_Description: string;
    PatientType_Status: number;
    PatientType_CreatedBy: number;
    PatientType_CreatedDate: string;
}
export class Stock {
    Stock_Id: number;
    Facility_Id: number;
    NurseStation_Id: number;
    DrugName: string;
    InHand: string;
    Barcode: string[];
    Stock_Status: number;
    Stock_CreatedBy: number;
    Stock_CreatedDate: string;
    GPICode: string;
    TrackableBit: number;
    Sharedstockbit: number;
    MergedStockIds:string;
}
export class Ekit {
    Ekit_Id: number;
    Facility_Id: number;
    NurseStation_Id: number;
    DrugName: string;
    InHand: string;
    Barcode: string[];
    NDC: string;
    LotNumber: string;
    ExpiryDate: string;
    Ekit_Status: number;
    Ekit_CreatedBy: number;
    Ekit_CreatedOn: string;
    GPICode: string;
    TrackableBit: number;
    SharedeKitbit: number;
    ControlSubstance:number
}
export class EkitMeds
{
     Ekit_Id:number;
     Facility_Id:number;
     NurseStation_Id:number;
     DrugName:string;
     InHand:string;
     LotNumber:string;
    ExpiryDate:string;
     Ekit_Status:number;
    Ekit_CreatedBy:number;
     Ekit_CreatedOn:string;
    BarCodeDetails:string;

}
export class InsertLotEkit{
    Facility_Id:number;
     NurseStation_Id:number;
     DrugName:string;
     LotNumber:string;
     ExpiryDate:string;
     InHand:string;
     Barcode:string;
     Ekit_CreatedBy:number;
     GpiCode:string;
     EkitUpdate:number;
     EditEkit_id:number;
     Reason:string;
}
export class InsertBarcodeekit{
    DrugName:string;
    Barcode:string;

}