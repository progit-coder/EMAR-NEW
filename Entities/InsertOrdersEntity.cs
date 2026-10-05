using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class InsertOrdersEntity
    {
        public int FacilityID { get; set; }
        public int OrderingPhysicianID { get; set; }
        public string ResName { get; set; }
        public int ResidentID { get; set; }
        public string DOB { get; set; }
        public string AdmitDate { get; set; }
        public string PhyName { get; set; }
        public string Drug { get; set; }
        public string Quantity { get; set; }
        public string RouteCode { get; set; }
        public string AdditionalInst { get; set; }
        public string NumberofRefill { get; set; }
        public string Inhand { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Diagnosis { get; set; }
        public int OrderID { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public string AlertText { get; set; }
        public int MaxPerdays { get; set; }
        public int OrderStockFlag { get; set; }
        public int PRNFlag { get; set; }
        public int SelfAdministeredFlag { get; set; }
        public int TreatmentFlag { get; set; }
        public int Maysubstitute { get; set; }
        public int newOrderFlag { get; set; }

    }
    public class DrugNameSearch
    {
        public int? Drug_Id { get; set; }
        public string DrugName { get; set; }
        public string DosageForm { get; set; }
        public string Strength { get; set; }
        public string GPICode { get; set; }
        public int? ControlledSubstanceSchedule { get; set; }
        public string Route { get; set; }
        public string RouteDescription { get; set; }
    }
}
