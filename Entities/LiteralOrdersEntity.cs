using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class LiteralOrdersEntity
    {
        public int POrder_Id { get; set; }
        public int Patient_Id { get; set; }
        public Nullable<int> OrderTypeID { get; set; }
        public string FillerType { get; set; }
        public string OrderControl { get; set; }
        public string FacilityId { get; set; }
        public string PatientId { get; set; }
        public string Room { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string EnteredBy { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VEffectivedate { get; set; }
        public string OPhysicianLname { get; set; }
        public string OPhysicianFname { get; set; }
        public DateTime? OrderEffectiveDate { get; set; }
        public string OrderingFacilityName { get; set; }
        public string OrderingFacilityAddress1 { get; set; }
        public string OrderingFacilityPhone { get; set; }
        public int POrder_Status { get; set; }
        public Nullable<int> POrder_CreatedBy { get; set; }
        public System.DateTime POrder_CreatedDate { get; set; }
        public string GiveCodeText { get; set; }
    }
}
