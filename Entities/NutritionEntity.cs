using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LTCPro.Entities
{
   public class NutritionEntity
    {
        public int Nutrition_Id { get; set; }
        public string Nutrition_Type { get; set; }
        public string Nutrition_Desc { get; set; }
        public int Nutrition_Status { get; set; }
        public int Nutrition_CreatedBy { get; set; }
        public System.DateTime Nutrition_CreatedDate { get; set; }
    }
}
