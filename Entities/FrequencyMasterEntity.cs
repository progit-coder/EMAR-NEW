using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FrequencyMasterEntity
    {
        public int Frequency_Id { get; set; }
        public string Frequency_Code { get; set; }
        public string Frequency_Shortname { get; set; }
        public string Frequency_Description { get; set; }
        public int Frequency_Status { get; set; }
        public Nullable<int> Frequency_CreatedBy { get; set; }
        public System.DateTime Frequency_CreatedDate { get; set; }
        public string Frequency_Name { get; set; }
        public int? Frequency_PRN { get; set; }
        public Nullable<int> Freq_Times { get; set; }
        public string Freq_Descalert { get; set; }
        public string Freq_Descalert1 { get; set; }
    }
    public class FrequencyMasterEntityWithShifts
    {
        public string Frequency_Id { get; set; }
        public string Frequency_Name { get; set; }
        public int? Frequency_PRN { get; set; }
        public string Freq_Group { get; set; }
        public Nullable<int> Freq_Times { get; set; }
        public string Freq_Descalert { get; set; }
        public string Freq_Descalert1 { get; set; }
    }
}
