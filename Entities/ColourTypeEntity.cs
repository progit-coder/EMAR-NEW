using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class ColourTypeEntity
    {
        public int ColourType_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string PatientTypeId { get; set; }
        public Nullable<int> PatientType_Id { get; set; }
        public Nullable<int> ColourType_Status { get; set; }
        public Nullable<int> ColourType_CreatedBy { get; set; }
        public Nullable<System.DateTime> ColourType_CreatedOn { get; set; }

        public virtual PatientType PatientType { get; set; }
        public virtual User User { get; set; }
        public virtual Demographic Demographics1 { get; set; }
    }
    public partial class PatientColorTypeEntity
    {
        public string Color_Code { get; set; }
        public string Color_Description { get; set; }
    }
}
