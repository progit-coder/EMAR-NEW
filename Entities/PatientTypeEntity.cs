using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class PatientTypeEntity
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public PatientType()
        //{
        //    this.Demographics1 = new HashSet<Demographic>();
        //}

        public int PatientType_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public string Color_Code { get; set; }
        public string Color_Description { get; set; }
        public Nullable<int> PatientType_Status { get; set; }
        public Nullable<int> PatientType_CreatedBy { get; set; }
        public Nullable<System.DateTime> PatientType_CreatedDate { get; set; }
    }
    public class PatientTypeCustomEntity
    {
        public int PatientType_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public string Company_Name { get; set; }
        public string Color_Code { get; set; }
        public string Color_Description { get; set; }
        public Nullable<int> PatientType_Status { get; set; }
        public int CreatedBy { get; set; }
        public string PatientType_CreatedBy { get; set; }
        public Nullable<System.DateTime> PatientType_CreatedDate { get; set; }
    }
}
