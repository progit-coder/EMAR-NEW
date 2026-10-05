using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentAllergyEntity
    {
        public Nullable<int> ResidentId { get; set; }
        public string Allergy { get; set; }
        public string Description { get; set; }
        public string NDCCode { get; set; }
        public string NDCDesc { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
