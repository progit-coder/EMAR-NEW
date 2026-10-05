using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class InboundFileInformationEntity
    {
        public int CompanyId { get; set; } 
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime EncryptedDate { get; set; }
        public DateTime DecryptedDate { get; set; }
    }
}
