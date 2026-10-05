using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserOTPCheckEntity
    {
        public int UserId { get; set; }
        public string OTP { get; set; }
        //public DateTime date { get; set; }
    }
}
