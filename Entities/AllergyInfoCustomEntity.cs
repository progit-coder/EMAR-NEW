using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AllergyInfoCustomEntity
    {
        public int PAllergy_Id { get; set; }
        public int Patient_Id { get; set; }
        public string ClassDrug_Name { get; set; }
        public string AllergyReactionCode { get; set; }
        public int PAllergy_Status { get; set; }
        public Nullable<int> PAllergy_CreatedBy { get; set; }
        public System.DateTime PAllergy_CreatedDate { get; set; }
        public string Allergy_CreatedBy { get; set; }
        public Nullable<int> ClassDrugType { get; set; }

    }
}
