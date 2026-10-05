using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderDestroyEntity
    {
        public int OrderDestroy_Id { get; set; }
        public int PQuantity_Id { get; set; }
        public string Quantity { get; set; }
        public string Reason { get; set; }
        public Nullable<int> DestroyerUserId { get; set; }
        public Nullable<int> ApprovalUserId { get; set; }
        public int OrderDestroy_Status { get; set; }
        public Nullable<int> OrderDestroy_CreatedBy { get; set; }
        public System.DateTime OrderDestroy_CreatedOn { get; set; }
        public string DUserName { get; set; }
        public string DPassword { get; set; }
        public string AUserName { get; set; }
        public string APassword { get; set; }

        //public virtual User User { get; set; }
        //public virtual User User1 { get; set; }
        //public virtual User User2 { get; set; }
        //public virtual CommonOrderInfo CommonOrderInfo1 { get; set; }
    }
}
