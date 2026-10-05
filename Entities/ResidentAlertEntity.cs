using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentAlertEntity
    {
        public int Resident_Id { get; set; }
        public Nullable<int> AlertType_Id { get; set; }
        public Nullable<int> Stay_Id { get; set; }
        public Nullable<System.DateTime> ThruDate { get; set; }
        public string AlertDescription { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
