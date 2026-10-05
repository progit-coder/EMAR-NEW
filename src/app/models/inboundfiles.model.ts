import { Company } from "./company.model";

export class InboundFiles {
    FileName: string;
    FileSize: number;
    ModifiedDate: string;
    FileData: string;
    CompanyId: number;
    EncryptedDate: string;
    DecryptedDate: string;
}
export class FileInformation {
    File_Id: number;
    File_Category: number;
    File_Name: string;
    File_Data: string;
    Event: string;
    File_Status: number;
    File_Error: string;
    File_CreatedDate: string;
    File_CreatedBy: number;
    Company_Id: number;
    Company: Company[];
    File_ErrorDesc:string;
}
export class HlFields {
    IsMandatory: number;
    FieldName: string;
    FieldValue: string;
}
export class OutboundList {
    Patient_Id: number;
    Patient_Name: string;
    RecordId: number;
    Category: string;
    UploadedBy: number;
    ApprovedBy: number;
    ApprovedOn: string;
}