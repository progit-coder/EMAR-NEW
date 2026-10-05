using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentAIMEntity
    {
        public Nullable<int> ResidentId { get; set; }
        public Nullable<long> StayID { get; set; }
        public Nullable<System.DateTime> AssmtDate { get; set; }
        public Nullable<int> A01FacialExpresion { get; set; }
        public Nullable<int> A02Lips { get; set; }
        public Nullable<int> A03Jaw { get; set; }
        public Nullable<int> A04Tongue { get; set; }
        public Nullable<int> B05Upper { get; set; }
        public Nullable<int> B06Lower { get; set; }
        public Nullable<int> C07Neck { get; set; }
        public Nullable<int> D08Severity { get; set; }
        public Nullable<int> D09Incapacitation { get; set; }
        public Nullable<int> D10Awareness { get; set; }
        public Nullable<int> E11Teeth { get; set; }
        public Nullable<int> E12Dentures { get; set; }
        public Nullable<int> Interpretation { get; set; }
        public string Signature { get; set; }
        public Nullable<System.DateTime> DateSigned { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
