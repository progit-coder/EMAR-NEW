using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class BarcodeDetailEntity
    {
        public int PBarcode_Id { get; set; }
        public int POrder_Id { get; set; }
        public string BarcodeDetail1 { get; set; }
        public int PBarcode_Status { get; set; }
        public Nullable<int> PBarcode_CreatedBy { get; set; }
        public System.DateTime PBarcode_CreatedDate { get; set; }

        public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
    public partial class BarcodeEntity
    {
        public int PBarcode_Id { get; set; }
        public string BarcodeDetail1 { get; set; }
        public string GPICode { get; set; }
        public int? Patient_Id { get; set; }
        public int? Facility_Id { get; set; }
    }
}
