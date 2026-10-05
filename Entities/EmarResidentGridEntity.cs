using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class EmarResidentGridEntity
    {
        public string ImageLocation { get; set; }
        public string Resident_Name { get; set; }
        public Nullable<int> Total_Count { get; set; }
        public Nullable<int> Pending_count { get; set; }
        public Nullable<int> PRN_Drugs { get; set; }
        public int Patient_Id { get; set; }
        public string PatientMRNumber { get; set; }
        public Byte[] ResidentImage { get; set; }
        public string ImgPath { get; set; }
        public Nullable<int> MedReasonCount { get; set; }
        //public string PassTime { get; set; }
        public Nullable<int> ReviewFlag { get; set; }
        public string ExternalPatientId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
    }
    public class EmarData
    {
        public IList GridData { get; set; }
        public List<string> XAxisData { get; set; }
        public List<PieSeriesDataEntity> YAxisData { get; set; }
        public List<string> PXAxisData { get; set; }
        public List<PieSeriesDataEntity> PYAxisData { get; set; }

    }
}
