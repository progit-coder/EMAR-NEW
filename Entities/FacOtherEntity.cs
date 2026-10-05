using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FacOtherEntity
    {        
        public int Facility_Id { get; set; }
        public System.DateTime CareTrackerExportDate { get; set; }
        public string CareTrackerExportPath { get; set; }
        public Nullable<bool> UseCareTracker { get; set; }
        public string CareTrackerImportPath { get; set; }        
        public int FacOther_CreatedBy { get; set; }
        public System.DateTime FacOther_CreatedDate { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
