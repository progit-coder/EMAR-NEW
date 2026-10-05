using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public partial class ICD10Entity
    {
        public int ICD10_Id { get; set; }
        public string ICD10_RawFormat { get; set; }
        public string ICD10_Formatted { get; set; }
        public string ICD10_Description { get; set; }
        public int ICD10_Status { get; set; }
        public Nullable<int> ICD10_CreatedBy { get; set; }
        public System.DateTime ICD10_CreatedDate { get; set; }
        //public virtual UserEntity User { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
    }

    public class ICD10GridEntity
    {
        public int TotalRecords { get; set; }
        public List<ICD10Entity> Data { get; set; }
    }

    public class SearchCustomEntity
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int Status { get; set; }
    }
}
