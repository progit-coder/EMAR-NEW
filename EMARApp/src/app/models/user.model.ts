export class UserModel {
    User_Id: number;
    User_Suffix: number;
    User_Fname: string;
    User_Lname: string;
    User_Mname: string;
    UserName: string;
    Password: string;
    User_Gender: number;
    User_MaritalStatus: number;
    User_DisplayName: string;
    User_Email: string;
    User_Phone: string;
    User_PwdCount: number;
    User_Lock: number;
    User_Status: number;
    User_CreatedBy: number;
    User_CreatedDate: string;
    StkReportReq:number;
    NewUserFlag:number;
    RoleId :number;
    Facilities :string;
    NurseStations:string;
    Processkey:number;
    PhysicianNPI:string;
    PastDueAlertFlag:number;
}
export class UserDrop{
    User_Id: number;
    User_DisplayName: string;
}
export class UserRecModel {
    // tslint:disable-next-line:variable-name
    RecFac_Id: number;
    // tslint:disable-next-line:variable-name
    User_Id: number;
    Facility_Id: number;
    NurseStation_Id: string;
    companyId:number
  }