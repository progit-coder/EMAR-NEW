using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrdersViewEntity
    {
        public CommonOrderInfoEntity CommonOrderInfo { get; set; }
        public EncodedOrderDetailEntity EncodedOrderDetail { get; set; }
        public QuantityDetailEntity QuantityDetail { get; set; }
        public TreatmentRouteInfoEntity TreatmentRouteInfo { get; set; }
        public TreatmentInfoEntity TreatmentInfo { get; set; }
        public AddlInstructionDetailEntity AddlInstructionDetail { get; set; }
        public AncillaryDetailEntity AncillaryDetail { get; set; }
    }
}
