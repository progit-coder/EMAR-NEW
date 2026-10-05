export class Hlsevensegment {
    HLConfig_Id: number;
    Company_Id: number;
    SegDetail_Id: number;
    SegDetailConfig_Id: number;
    SegDisplay_Id: number;
    HLConfig_Status: number;
    HLConfig_CreatedBy: number;
    HLConfig_CreatedDate: string;

}
export class HLSevenClone {
    InputCompanyId: number;
    OutputCompanyId: number;
    SegmentId: number;
    CreatedBy: number;
}

export class Hlsevenconfigs {
    IsMandatory: number;
    FieldName: string;
    FieldValue: string;

}
export class OutboundHLSegmentFieldDisplayConfigs {
    OSegment_Id: number;
    OSegment_Desc: string;
    OSegment_Status: number;
    OSegment_CreatedBy: number;
    OSegment_CreatedDate: string;
}
export class HLSevenOutboundDisplayCompanyConfigs {
    OHLConfig_Id: number;
    Company_Id: number;
    OSegDetail_Id: number;
    DisplayConfigId: number;
    OHLConfig_Status: number;
    OHLConfig_CreatedBy: number;
    OHLConfig_CreatedDate: string;
    OSegment_Id:number;
    HlSequence:number;
}