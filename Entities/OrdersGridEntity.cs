using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrdersGridEntity
    {
        public int Patient_Id { get; set; }
        public int porder_Id { get; set; }
        public int PQuantity_Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string ResidentName { get; set; }
        public string Drug { get; set; }
        public string Dosage_Form { get; set; }
        public string Quantity { get; set; }
        public string Directions { get; set; }
        public string Physician_Name { get; set; }
        public string POrder_Status { get; set; }
        public int ScheduleTimeFlag { get; set; }
        public int SplitFlag { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public byte[] ImageLocation { get; set; }
        public int OrderStatus { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string PRN { get; set; }
        public string HoldUntill { get; set; }
        public string ImgPath { get; set; }
        public string NumberOfRefillsRemaining { get; set; }
        public int HoldStatus { get; set; }
        public string HoldFrom { get; set; }

    }
    public class OrdersEndDateEntity
    {
        public int Patient_Id { get; set; }
        public int porder_Id { get; set; }
        public int PQuantity_Id { get; set; }
        public string ResidentName { get; set; }
        public string Drug { get; set; }
        public string Directions { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
    public class ConfirmOrdersEndDateEntity
    {
        public int porder_Id { get; set; }
        public int PQuantity_Id { get; set; }        
    }
    public  class DoseUomEntity
    {
        public int Dose_Id { get; set; }
        public string Dose_Desc { get; set; }
      
    }
}
