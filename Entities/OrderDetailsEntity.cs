using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderDetailsEntity
    {
        public string ResName { get; set; }
        public int patientID { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? AdmitDate { get; set; }
        public string DischargeDate { get; set; }
        public string PhyName { get; set; }
        public string Drug { get; set; }
        public string Quantity { get; set; }
        public string RouteCode { get; set; }
        public string AdditionalInst { get; set; }
        public string NumberofRefill { get; set; }
        public string Inhand { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AlertText { get; set; }
        public int? MaxPerDay { get; set; }
        public bool? Prn { get; set; }
        public bool? Self { get; set; }
        public bool? Treatment { get; set; }
        public bool? MaySubtitude { get; set; }
        public bool? OrderStock { get; set; }
        public bool ControlsubFlag { get; set; }
    }
}
