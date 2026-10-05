using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ChatMessageEntity
    {
        public string type { get; set; }
        public string fromId { get; set; }
        public string toId { get; set; }
        public string message { get; set; }
        public DateTime? dateSent { get; set; }
        public DateTime? dateSeen { get; set; }
    }
}
