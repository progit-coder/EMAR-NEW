using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingRouteEntity
    {
        public int StagRoute_Id { get; set; }
        public Nullable<int> StagTreatment_Id { get; set; }
        public string Route { get; set; }
        public string AdministrationSite { get; set; }
        public string AdministrationDevice { get; set; }
        public string AdministrationMethod { get; set; }
        public string RoutingInstruction { get; set; }
        public string AdminstrationSiteModifier { get; set; }
    }
}
