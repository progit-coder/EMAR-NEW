using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderFavouriteEntity
    {
        public int OrderFavourite_ID { get; set; }
        public int PQuantity_Id { get; set; }
        public int OrderFavMaster_ID { get; set; }
        public int OrderFavourite_Status { get; set; }
        public Nullable<int> OrderFavourite_Createby { get; set; }
        public System.DateTime OrderFavourite_CreatedOn { get; set; }
        public int FavListCheckFlag { get; set; }

        //public virtual OrderFavouriteMasterEntity OrderFavouriteMaster { get; set; }
        //public virtual UserEntity User { get; set; }
        //public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
}
