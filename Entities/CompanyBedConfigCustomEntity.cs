using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CompanyBedConfigCustomEntity
    {
        public List<int> Facilities { get; set; }
        public List<int> NurseStations { get; set; }
        public List<int> Floors { get; set; }
        public List<int> Wings { get; set; }
        public List<int> Rooms { get; set; }
        public List<int> Beds { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int ResidentType { get; set; }
        public int RecentFacNsFalg { get; set; }
    }
    public partial class tblTimeZoneEntity
    {
        public int TZ_Id { get; set; }
        public string ZoneDesc { get; set; }
        public string ZoneCode { get; set; }
        public string OffSetTime { get; set; }
        public Nullable<int> Dst { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
