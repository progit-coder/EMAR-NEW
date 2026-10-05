using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class BypassBiometricEntity
    {
        public int PatientID { get; set; }
        public string Time { get; set; }
        public string dateValue { get; set; }
        public string reason { get; set; }
        public int byPass { get; set; }
    }
}
