using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class PharmacyMedsExpiryDateDashboardd
    {
        public string CheckinFromDate { get; set; }
        public string CheckinToDate { get; set; }
        public string ExpiryFromDate { get; set; }
        public string ExpiryToDate { get; set; }
        public string NusingStationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
    public class EKitMedsExpiryDateDashboard
    {
        public string NusingStationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
    public class GetMedicationActiveInventoryReportDashboard
    {
        public int UserId { get; set; }
        public string NusingStationId { get; set; }
        public int Status { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
}
