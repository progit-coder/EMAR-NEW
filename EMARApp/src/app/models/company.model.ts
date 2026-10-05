export class Company {
    Company_Id: number;
    Company_Name: string;
    Company_Addr1: string;
    Company_Addr2: string;
    Company_City: string;
    Company_State: string;
    Company_CountryId: number;
    Company_Zip: number;
    Company_Phone: string;
    Company_Fax: string;
    Company_Email: string;
    Com_ContactPerson: string;
    Com_ContactPhone: string;
    Company_EIN: string;
    Company_Logo: string;
    Company_Footerlogo:string
    Company_Status: number;
    Company_CreatedBy: number;
    Company_CreatedDate: string;
    Company_UniqueId: string;
    ApprovalFlag:number;
    //Fingersdesc_Id:number;
    //TimeFormat:number;
}
export class HLDesc {
     HLDirectionalWay_Id :number;
     HLDirectionalWaysDesc : string;
}
export class companyconfig
{
   CompanyConfig_Id :number;
   Company_Id :number;
   ApprovalFlag :number;
   Fingersdesc_Id :number;
   TimeFormat :number;
   Hl7Configured :number;
   HLDirectionalWay_Id :number;
   CompanyConfig_Status :number;
   CompanyConfig_CreatedBy :number;
   CompanyConfig_CreatedOn :string;
   Category: string;
   Events:string;
   StockReport_Id:number;
   DrFirstRequired:number;
   //Physician_Id:number;
   
}