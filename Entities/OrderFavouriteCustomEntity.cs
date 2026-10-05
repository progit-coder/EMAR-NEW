using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderFavouriteCustomEntity
    {
        public int OrderFavourite_ID { get; set; }
        public int OrderFavMaster_ID { get; set; }
        public string OrderFavDesc { get; set; }
        public int OrderFavMaster_status { get; set; }
        public Nullable<int> OrderFavMaster_CreatedBy { get; set; }
        public System.DateTime OrderFavMaster_CreatedOn { get; set; }
        public int check { get; set; }
    }
    public class OrderFavConfigEntity
    {
        public int OFConfig_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public string OrderFavCode { get; set; }
        public Nullable<int> OrderFavMaster_ID { get; set; }
        public Nullable<int> OFConfig_Status { get; set; }
        public Nullable<int> OFConfig_CreatedBy { get; set; }
        public Nullable<System.DateTime> OFConfig_CreatedDate { get; set; }
        public string OrderFavMasterIds { get; set; }
        public string OrderFavDesc { get; set; }
    }
    public class PrcGetOrderFavConfigByCodeEntity
    {
        public Nullable<int> Facility_ID { get; set; }
        public string OrderFavCode { get; set; }
        public string OrderFavDesc { get; set; }
        public Nullable<int> OFConfig_Status { get; set; }
        public Nullable<int> OFConfig_CreatedBy { get; set; }
        public Nullable<System.DateTime> OFConfig_CreatedDate { get; set; }
    }
    public class CustomOrderFavInfoDataEntity
    {
        public int FacilityId { get; set; }
        public string Code { get; set; }
        public string CreatedDate { get; set; }
    }
    public  class PrcGetOrderFavConfigInfoEntity
    {
        public string Facility_Name { get; set; }
        public string OrderFavCode { get; set; }
        public string OrderFavDesc { get; set; }
        public Nullable<int> OFConfig_Status { get; set; }
        public Nullable<int> OFConfig_CreatedBy { get; set; }
        public Nullable<System.DateTime> OFConfig_CreatedDate { get; set; }
        public Nullable<int> Facility_Id { get; set; }
    }
    public class PrcInsertOrderFavFacilityConfigEntity
    {
        public Nullable<int> facility_Id { get; set; }
        public string orderFavCode { get; set; }
        public string orderFavMaster { get; set; }
        public Nullable<int> oFConfig_Status { get; set; }
        public Nullable<int> oFConfig_CreatedBy { get; set; }
        public Nullable<System.DateTime> oFConfig_CreatedDate { get; set; }
        public Nullable<int> events { get; set; }
    }
}
