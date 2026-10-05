using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ChatParticipantEntity
    {
        public int id { get; set; }
        public string displayName { get; set; }
        public string avatar { get; set; }
        public int status { get; set; }
        public int participantType { get; set; }
    }
}
