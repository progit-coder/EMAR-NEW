using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class TreatmentRouteInfoEntity
    {
        public int PRoute_Id { get; set; }
        public int POrder_Id { get; set; }
        public string RouteCode { get; set; }
        public string RouteText { get; set; }
        public string AdministrationSite { get; set; }
        public string AdministrationDevice { get; set; }
        public string AdministrationMethod { get; set; }
        public string RoutingInstruction { get; set; }
        public string AdminstrationSiteModifier { get; set; }
        public int PRoute_Status { get; set; }
        public Nullable<int> PRoute_CreatedBy { get; set; }
        public System.DateTime PRoute_CreatedDate { get; set; }
        public Nullable<int> PTROutBoundFileStatus { get; set; }
        public Nullable<int> PTROutBoundApproval { get; set; }
        public Nullable<int> PTROutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PTROutBoundApprovalOn { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual UserEntity User1 { get; set; }
        public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
}
