export class RoleMaster {
    Role_Id: number;
    Role_Desc: string;
    Role_Status: number;
    Role_CreatedBy: number;
    Role_CreatedDate: string;
    DefaultScreen_Id:number;
    IsAdmin:number;
    Parent_Id:number;
}
export class RoleDrop
{
    Role_Id: number;
    Role_Desc: string;
}
export class RoleConfigMaster {
    RoleConfig_Id: number;
    Role_Id: number;
    Screen_Id: number;
    Screens:string;
    AccessRead: number;
    AccessWrite: number;
    PrintPdf: number;
    PrintExcel: number;
    RoleConfig_Status: number;
    RoleConfig_CreatedBy: number;
    RoleConfig_CreatedDate: string;
    CreatedBy:string;
    RoleName:string;
    ScreenName:string;
    Read: boolean;
    Write: boolean;
    Pdf: boolean;
    Excel: boolean;
}
export class RoleScreens{
    Role_Id:number;
    Screen_Id:string;
}
export class RoleFacility {
    Facility_Id: number;
    Facility_Name: string;
}
export class RoleNurseStation{
    NurseStation_Id:number;
    NurseStation_Name:string;
    NurseStation_Code:string;
}
export class UserRoleFacilityConfigEntity {
    UserRole_Id: number;
    Role_Id: number;
    User_Id: number;
    Facility_id: number;
    UserRole_Status: number;
    UserRole_CreatedBy: number;
    UserRole_CreatedDate: string;
    Users: string;
    Facilities: string;
    NurseStations:string;
    User_DisplayName: string;
    NurseStationNames: string;
    FacilityName: string;
    RoleName: string;
    CreatedUser:string;
    NurseStation_Id:number;
    UserName :   string;
    CompanyName:string;
    DisplayName:string;
}
export class UserRoleConfigIds
{
    Role_Id: number;
    User_Id: number;
    Facility_Id: number;
}

export class Screen {
    Screen_Id: number;
    Screen_Desc: string;
    Screen_Status: number;
    Screen_CreatedBy: number;
    Screen_CreatedDate: string;
}
export class UserRoleFacilityConfigCustomEntity {
    UserRole_Id: number;
    Role_Id: number;
    User_Id: number;
    Facility_Id: number;
    UserRole_CreatedBy: number;
    UserRole_CreatedDate: string;
    NurseStations: string;
    OldRole_Id:number;
    OldUser_Id:number;
    OldFacility_Id:number;

}
export class DefaultScreen{
  DefaultScreen_Id :number;
  Screen_Id:number;
  DefaultScreen_Status:number;
  DefaultScreen_CreatedBy:number;
  DefaultScreen_CreatedOn:string;
}


