using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class PRNInsertCustomEntity
    {
        public Int64 DrugAdminister_Id { get; set; }
        public string PRNComment { get; set; }
        public int PRNCommentBy { get; set; }
        public DateTime PRNCommentOn { get; set; }
    }
    public partial class PRNFilterCustomEntity
    {
        public int User_Id { get; set; }
        public int Company_Id { get; set; }
        public List<int> Facilities { get; set; }
        public List<int> NurseStations { get; set; }
        public List<int> Floors { get; set; }
        public List<int> Wings { get; set; }
        public List<int> Rooms { get; set; }
        public List<int> Beds { get; set; }

    }
}