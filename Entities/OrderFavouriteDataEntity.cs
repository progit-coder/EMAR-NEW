using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderFavouriteDataEntity
    {
        public long FavouriteData_Id { get; set; }
        //public Nullable<long> DrugAdminister_Id { get; set; }
        public int POrder_Id { get; set; }
        public int pquantity_Id { get; set; }
        public Nullable<System.DateTime> AdminsterSchedule { get; set; }
        public Nullable<int> OrderFavMaster_ID { get; set; }
        public string value { get; set; }
        public Nullable<int> FavouriteData_Status { get; set; }
        public Nullable<int> FavouriteData_CreatedBy { get; set; }
        public System.DateTime Favourite_CreatedOn { get; set; }

        public string InputTime { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public Nullable<int> Window { get; set; }

        //public virtual OrderFavouriteMaster OrderFavouriteMaster { get; set; }
        //public virtual User User { get; set; }
        //public virtual OrderFavouriteData OrderFavouriteData1 { get; set; }
        //public virtual OrderFavouriteData OrderFavouriteData2 { get; set; }
    }
}
