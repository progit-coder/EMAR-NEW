export class PhyscianModel {
    Physician_Id: number;
    PhysicianNPI: string;
    PhysicianLName: string;
    PhysicianFName: string
    PhysicianAddress1: string;
    PhysicianAddress2: string;
    PhysicianCity: string;
    PhysicianState: string;
    PhysicianZip: number;
    Physician_Status: number;
    Physician_CreatedBy: number;
    Physician_CreatedDate: Date;
    NurseStations:string;
    PhysicianCountryID:number;
    // Facility_Id:number;
    Facility_Id:string;
    OldPhysicianNPI:string;
   // OldFacilityId:number;
    OldFacilityId:string;
    LicensesData:any;
    Credentials:number;
    PrimarySpec:number;
    PId:number;
    SupervisingPhy:string;
    Physician_Phone:number;
    FacId:string;

}