using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class OrdersCertifyCustomEntity
    {
        public string Cert_UserName { get; set; }
        public string Cert_Password { get; set; }
        public string CertifyOrders { get; set; }
        public string ProfessionalCredentials { get; set; }
        public int Month { get; set; }
        public int User_Id { get; set; }
        public System.DateTime CertifiedOn { get; set; }
    }
    public class CertifiedDatesDropEntity
    {
        public int CertifyTime_ID { get; set; }
        public string CertifyedPhysician { get; set; }
        public string CertifyedDate { get; set; }
        public string CertifyedPhysicianWithDate { get; set; }
        public Nullable<System.DateTime> ConvertedCerifiedDate { get; set; }
    }
    public class ProfileCertifiedDatesDropEntity
    {
        public string CertifyTime_ID { get; set; }
        public string CertifyedPhysician { get; set; }
        public string CertifyedDate { get; set; }
        public string CertifyedPhysicianWithDate { get; set; }
        public Nullable<System.DateTime> ConvertedCerifiedDate { get; set; }
    }
}
