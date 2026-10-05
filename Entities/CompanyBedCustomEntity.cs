using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CompanyBedCustomEntity
    {
        public List<FloorDropEntity> Floors { get; set; }
        public List<NurseStationDropEntity> NurseStations { get; set; }
        public List<WingDropEntity> Wings { get; set; }
        public List<RoomDropEntity> Rooms { get; set; }
        public List<BedDropEntity> Beds { get; set; }
        public List<FacilityDropEntity> Facilities { get; set; }
    }
    public class CompanyToBedEntity
    {
        public  int companyBedFlag { get; set; }
        public List<FloorDropEntity> Floors { get; set; }
        public List<WingDropEntity> Wings { get; set; }
        public List<RoomDropEntity> Rooms { get; set; }
        public List<BedDropEntity> Beds { get; set; }
    }
    public class UserAccesssFacilityNurseStationsEntity
    {
        public List<FacilityDropEntity> Facilities { get; set; }
        public List<NurseStationDropEntity> NurseStations { get; set; }
    }
}
