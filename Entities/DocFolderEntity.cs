using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{    
    public partial class DocFolderEntity
    {
        public int FolderID { get; set; }
        public string FolderName { get; set; }
        public Nullable<int> FolderParentID { get; set; }
        public string FolderLocation { get; set; }

        public int Folder_Status { get; set; }
        public Nullable<int> Folder_CreatedBy { get; set; }
        public System.DateTime Folder_CreatedDate { get; set; }
    }
    public class DocFloderCustomEntity
    {       
        public string text { get; set; }
        public int value { get; set; }
        public bool @checked { get; set; }
        public Nullable<int> FolderParentID { get; set; }
    
        public List<DocFloderCustomEntity> children { get;set;}
    }

}
