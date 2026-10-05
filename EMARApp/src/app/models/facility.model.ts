export class Facility {
    Facility_Id: number;
    Company_Id: number;
    Facility_Name: string;
    Facility_Addr1: string;
    Facility_Addr2: string;
    Facility_Zip: string;
    Facility_City: string;
    Facility_State: string;
    Facility_CountryId: number;
    Facility_Phone: string;
    Facility_Fax: string;
    Facility_ContactName: string;
    Facility_ContactPhone: string;
    Facility_ShortName: string;
    Facility_Logo: string;
    Facility_Status: number;
    Facility_CreatedBy: number;
    Facility_CreatedDate: string;
    TZ_Id:number;
    //State_Name:string;
}
export class Floor {
    Floor_Id: number;
    Floor_Name: string;
    // Facility_Id:number;
    Floor_Status: number;
    Floor_CreatedBy: number;
    Floor_CreatedDate: string;
}
export class NurseStation {
    Facility_Id: number;
    NurseStation_Id: number;
    NurseStation_Code: string;
    NurseStation_Name: string;
    NurseStation_Status: number;
    NurseStation_CreatedBy: number;
    NurseStation_CreatedDate: string;
    DefaultPhysician_Id:number;
    Default_Pharmacy:number;
    Backup_Pharmacy:string;
}
export class Wing {
    Wing_Id: number;
    Wing_Desc: string;
    Wing_Status: number;
    Wing_CreatedBy: number;
    Wing_CreatedDate: string;
}
export class Room {
    Room_Id: number;
    Room_Code: string;
    Room_Name: string;
    Room_Status: number;
    Room_CreatedBy: number;
    Room_CreatedDate: string;
}
export class Bed {
    Bed_Id: number;
    Bed_Code: string;
    Bed_Name: string;
    Bed_Status: number;
    Bed_CreatedBy: number;
    Bed_CreatedDate: string;
}

export class FiltersConfig {
    Facilities: any[];
    NurseStations: any[];
    Floors: any[];
    Wings: any[];
    Rooms: any[];
    Beds: any[];
    CurrentPage: number;
    PageSize: number;
    SearchText: string;
    ResidentType:number;
    RecentFacNsFalg:number;
}
export class CompanyBedConfigSave {
    BedConfig_Id: number;
    Company_Id: number;
    Facility_Id: number;
    Floor_Id: number;
    NurseStation_Id: number;
    Wing_Id: number;
    Room_Id: number;
    Bed_Id: number;
    BedConfig_Status: number;
    BedConfig_CreatedBy: number;
    BedConfig_CreatedDate: string;
}
export class NurseShift {
    NurseShifts_Id: number;
    NurseStation_Id: number;
    NurseShifts_Name: string;
    Fromtime_hoursId: number;
    Totime_hoursId: number;
    NurseShifts_Status: number;
    NurseShifts_CreatedBy: number;
    NurseShifts_CreatedOn: string;
    FromHours:string;
    ToHours:string;
    FromTimeFormat:string;
    ToTimeFormat:string;
}
export class NurseStationHierarchy
{
    NSHierarchy_Id:number;
    NurseStation_Id:number;
    FloorPrior:number;
    WingPrior:number;
    RoomPrior:number;
    BedPrior:number;
    CreatedBy:number;
    CreatedDate:string;
}
