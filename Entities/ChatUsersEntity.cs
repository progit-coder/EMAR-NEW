using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ChatUsersEntity
    {
        public ChatParticipantEntity participant { get; set; }
        public int metadata { get; set; }
    }
}
