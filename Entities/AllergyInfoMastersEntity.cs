using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AllergyInfoMastersEntity
    {
        public int Allergy_Id { get; set; }
        public Nullable<int> AllergyTypeMaster_Id { get; set; }
        public string AllergyDesc_Id { get; set; }
        public string AllergyDesc { get; set; }
        public int Allergy_Status { get; set; }
        public string AllergyStatus { get; set; }
        public Nullable<int> Allergy_CreatedBy { get; set; }
        public System.DateTime Allergy_CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
