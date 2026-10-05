using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FileInformationEntity
    {
        public int File_Id { get; set; }
        public int File_Category { get; set; }
        public string File_Name { get; set; }
        public string File_Data { get; set; }
        public string Event { get; set; }
        public int File_Status { get; set; }
        public string File_Error { get; set; }
        public Nullable<System.DateTime> File_CreatedDate { get; set; }
        public Nullable<int> File_CreatedBy { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public string File_ErrorDesc { get; set; }

    }
    public class FilesCountEntity
    {
        public int Total { get; set; }
        public int Success { get; set; }
        public int Error { get; set; }
    }

    public class InboundFilesGridEntity
    {
        public int TotalRecords { get; set; }
        public int Total { get; set; }
        public int Success { get; set; }
        public int Error { get; set; }
        public int Reject { get; set; }
        public List<FileInformationEntity> Data { get; set; }
    }
    public class ResendFileEntity
    {
        public string FileData { get; set; }
        public System.DateTime File_ResendDate{ get; set; }
    }

}
