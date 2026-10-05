export class FTEConfigMaster {
    FteConfig_Id: number;
    Company_Id: number;
    ServerIp: string;
    Category: number;
    ConnectionType: number;
    Port: string;
    UserName: string;
    Password: string;
    FteConfig_Status: number;
    FteConfig_CreatedBy: number;
    FteConfig_CreatedDate: string;
    ConnectionStatus:string;
}
export class FTECategory {
    FteCategory_Id: number;
    FteCategory_Desc: string;
    FteCategory_Status: number;
    FteCategory_CreatedBy: number;
    FteCategory_CreatedDate: string;
}
export class FTEConnection {
    FteConn_Id: number;
    FteConn_Desc: string;
    FteConn_Status: number;
    FteConn_CreatedBy: number;
    FteConn_CreatedDate: string;
}
