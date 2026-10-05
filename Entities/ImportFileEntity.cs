using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class ImportFileEntity
    {
        public long ImportFile_Id { get; set; }
        public string ImportFile_Name { get; set; }
        public Nullable<System.DateTime> EncryptedDate { get; set; }
        public Nullable<System.DateTime> DecryptedDate { get; set; }
        public Nullable<int> ImportFile_Status { get; set; }
        public Nullable<int> ImportFile_CreatedBy { get; set; }
        public Nullable<System.DateTime> ImportFile_CreatedDate { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
