using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OutBoundFileInformationEntity
    {
        public int File_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public int File_Category { get; set; }
        public string File_Name { get; set; }
        public string File_Data { get; set; }
        public string Event { get; set; }
        public int File_Status { get; set; }
        public string File_Error { get; set; }
        public Nullable<System.DateTime> File_CreatedDate { get; set; }
        public Nullable<int> File_CreatedBy { get; set; }
        public string File_ErrorDesc { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public Nullable<int> Porder_Id { get; set; }
        public Nullable<System.DateTime> ETConvertedFile_CreatedDate { get; set; }
        public string MessageId { get; set; }

    }
}
