using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderFavouriteMasterEntity
    {        
        public OrderFavouriteMasterEntity()
        {
            this.OrderFavourites = new HashSet<OrderFavouriteEntity>();
        }

        public int OrderFavMaster_ID { get; set; }
        public string OrderFavDesc { get; set; }
        public int OrderFavMaster_status { get; set; }
        public Nullable<int> OrderFavMaster_CreatedBy { get; set; }
        public System.DateTime OrderFavMaster_CreatedOn { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderFavouriteEntity> OrderFavourites { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
