export class WeightMaster
{
    WeightLog_ID:number;
    Patient_Id:number;
    Weight:number;
    Type:string;
    HeightFeet:number;
    HeightInc:number;
    DateTime:string;
    PreDialysis:number;
    PostDialysis:number;
    Remarks:string;
    IBW:string;
    initials:string;
    WeightLog_Status:number;
    WeightLog_CreatedBy:number;
    WeightLog_CreatedOn:string;
}
export class VisitBehaviour
{
    VisitBehaviour_ID:number;
    PVisit_Id:number;
    HallucinationsID:number;
    DelusionsID:number;
    Physicalbehavioral:number;
    Verbalbehavioral:number;
    Otherbehavioral:number;
    rejectevaluation:number;
    Resisdentwandered:number;
    Comments:string;
    Initials:string;
    Date:string;
    WeeklyStatus:number;
    VisitBehaviour_Status:number;
    VisitBehaviour_CreatedBy:number;
    VisitBehaviour_CreatedDate:string;
}


export class Foodintake
{
    VisitFoodintake_ID:number;
    PVisit_Id:number;
    AssessmentDate:string;
    Initials:string;
    Attendingphysician:string;
    Fluidsb:string;
    Alternateb:string;
    supplementb:string;
    Fluidsl:string;
    Alternatel:string;
    supplementl:string;
    Fluidss:string;
    Alternates:string;
    supplements:string;
    VisitFoodintake_Status:number;
    VisitFoodintake_CreatedBy:number;
    VisitFoodintake_CreatedDate:string;  
}

export class BehaviourDropData
{
    BehavioralSym_ID:number;
    BehavioralSym_Desc:string;
}
export class Vitals{
    
  Vitals_ID: number;
  PVisit_Id: number;
  VitalDate: string;
  VitalTime: string;
  CistolicBP: string
  DiastolicBP: number;
  HeartRate: number;
  RespiratoryRate: number;
  OxygenRate: number;
  Temperature: string;
  Pain: string;
  Remark: string;
  PulseRate: number;
  Initials: string;
  PDAID: string;
  VitalSigns_status: number;
  VitalSigns_CreatedBy: number;
  VitalSigns_CreatedOn:string;
  BloodSugar: string;
}
export class NurseNote{
  VisitNursingNotes_ID: number;
  PVisit_Id: number;
  NoteDate: string;
  NoteTime: string;
  NurseName: string;
  Notes: string;
  PDAID: string;
  VisitNursingNotes_Status: number;
  VisitNursingNotes_CreatedBy: number;
  VisitNursingNotes_CreatedOn: string;
}
export class AdmintDateDrop
{
    PVisit_Id:number;
    AdmitDate:string;
    Patient_Id:number;
}

export class MedRefdata
{
    dashboardName:string;
    fromdate:string;
    todate:string;
    userId:number;
    nursingstationId :string;
    currentPage:number;
    pageSize:number;
    passTime:string;
    dateTime : string;
    facilityId : number;
}

