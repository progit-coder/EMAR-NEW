using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrdersGridCustomEntity
    {
        public int POrder_Id { get; set; }
        public string GiveCodeText { get; set; }
        public string GiveDosageForm { get; set; }
        public string ProviderAdminDrugInsText { get; set; }
        public string OrderControl { get; set; }
        public string OrderStockFlag { get; set; }
    }
}
