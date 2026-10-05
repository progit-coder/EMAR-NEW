using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{    
    public partial class UploadedDocumentCustomEntity
    {
        public int PatientDoc_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string DocName { get; set; }
        public string DocLocation { get; set; }
        public string DocDescription { get; set; }
        public Nullable<int> FolderID { get; set; }
        public Nullable<System.DateTime> UDocuments_CreatedDate { get; set; }
        public Nullable<int> UDocuments_CreatedBy { get; set; }
        public string DocType { get; set; }
    }
}
