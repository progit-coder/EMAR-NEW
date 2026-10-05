using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ZipCodeEntity
    {
        public ZipCodeEntity()
        {
            this.Companies = new HashSet<CompanyEntity>();
        }

        public int Zip_Id { get; set; }
        public string Zip_Code { get; set; }
        public int Zip_City { get; set; }
        public int Zip_Status { get; set; }
        public Nullable<int> Zip_CreatedBy { get; set; }
        public System.DateTime Zip_CreatedDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<CompanyEntity> Companies { get; set; }
    }
}

