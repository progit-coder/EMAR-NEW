using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingNoteEntity
    {
        public int StagNote_id { get; set; }
        public Nullable<int> StagEncOrder_Id { get; set; }
        public Nullable<int> SetID { get; set; }
        public string SourceOfComment { get; set; }
        public string Comment { get; set; }
        public string CommentType { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
