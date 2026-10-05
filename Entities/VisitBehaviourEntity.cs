namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class VisitBehaviourEntity
    {
        public long VisitBehaviour_ID { get; set; }
        public long PVisit_Id { get; set; }
        public Nullable<int> HallucinationsID { get; set; }
        public Nullable<int> DelusionsID { get; set; }
        public Nullable<int> Physicalbehavioral { get; set; }
        public Nullable<int> Verbalbehavioral { get; set; }
        public Nullable<int> Otherbehavioral { get; set; }
        public Nullable<int> rejectevaluation { get; set; }
        public Nullable<int> Resisdentwandered { get; set; }
        public string Comments { get; set; }
        public string Initials { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> WeeklyStatus { get; set; }
        public int VisitBehaviour_Status { get; set; }
        public Nullable<int> VisitBehaviour_CreatedBy { get; set; }
        public Nullable<System.DateTime> VisitBehaviour_CreatedDate { get; set; }
        public string UserName { get; set; }
        public string PhysicalbehavioralDesc { get; set; }
        public string VerbalbehavioralDesc { get; set; }
        public string OtherbehavioralDesc { get; set; }
        public string rejectevaluationDesc { get; set; }
        public string ResisdentwanderedDesc { get; set; }

    }
}
