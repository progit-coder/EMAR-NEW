using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AlertTypeEntity
    {
        public int AlertType_Id { get; set; }
        public string AlertType_Code { get; set; }
        public string AlertType_Desc { get; set; }
        public int AlertType_Status { get; set; }
        public int AlertType_CreatedBy { get; set; }
        public System.DateTime AlertType_CreatedDate { get; set; }
    }
    public class AlertNotificationEntity
    {
        public long AlertTextId { get; set; }
        public string ResidentName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> FavouriteFlag { get; set; }
        public Nullable<int> ReadFlag { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> AlertStatus { get; set; }
        public Nullable<long> File_Id { get; set; }
        public int TotalRecords { get; set; }
        public string NurseStationName { get; set; }
    }
    public class AlertStatusEntity
    {
        public Nullable<long> AlertText_Id { get; set; }
        public Nullable<int> Favourite_Flag { get; set; }
        public System.DateTime Alert_CreatedDate { get; set; }

    }
    public class AlertsCustomEntity
    {
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public string Residents { get; set; }
        public  int AlertTypeId {get;set;}
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
    }
}
