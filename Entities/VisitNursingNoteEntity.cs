namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class VisitNursingNoteEntity
    {
        public long VisitNursingNotes_ID { get; set; }
        public long PVisit_Id { get; set; }
        public Nullable<System.DateTime> NoteDate { get; set; }
        public string NoteTime { get; set; }
        public string NurseName { get; set; }
        public string Notes { get; set; }
        public string PDAID { get; set; }
        public int VisitNursingNotes_Status { get; set; }
        public Nullable<int> VisitNursingNotes_CreatedBy { get; set; }
        public Nullable<System.DateTime> VisitNursingNotes_CreatedOn { get; set; }
        public string UserName { get; set; }
        

    }
}
