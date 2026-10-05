using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class ActivityEntity
    {
        public enum ActivityMaster
        {
            View = 1,
            Save = 2,
            Edit = 3,
            PDF = 4,
            Excel = 5,
            Search = 6,
            Delete = 7 , 
            Signout  = 8,
            AutoSignout = 9,
        }
        public int Activity_Id { get; set; }
        public string Activity_Type { get; set; }
        public string Activity_Desc { get; set; }
    }
   

}
