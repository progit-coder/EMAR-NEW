using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class RoleConfigEntity
    {
        public int RoleConfig_Id { get; set; }
        public int Role_Id { get; set; }
        public Nullable<int> Screen_Id { get; set; }
        public string Screens { get; set; }
        public Nullable<int> AccessRead { get; set; }
        public Nullable<int> AccessWrite { get; set; }
        public Nullable<int> PrintPdf { get; set; }
        public Nullable<int> PrintExcel { get; set; }
        public int RoleConfig_Status { get; set; }
        public Nullable<int> RoleConfig_CreatedBy { get; set; }
        public System.DateTime RoleConfig_CreatedDate { get; set; }
        //public virtual UserEntity User { get; set; }
        //public virtual RoleEntity Role { get; set; }
        //public virtual ScreenEntity Screen { get; set; }
        public string CreatedBy { get; set; }
        public string RoleName { get; set; }
        public string  ScreenName { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Excel { get; set; }
        public bool Pdf { get; set; }
        public string Status { get; set; }
        
        public string Role_Name { get; set; }
        public string Screen { get; set; }
        public bool PDF { get; set; }
    }
   public class RoleScreensEntity
    {
       public int Role_Id { get; set; }
       public string Screen_Id { get; set; }
    }
    public class RoleConfigGridEntity
    {
        public int RoleConfig_Id { get; set; }
        public string Role_Name { get; set; }
        public string Screen { get; set; }
        public string Read { get; set; }
        public string Write { get; set; }
        public string PDF { get; set; }
        public string Excel { get; set; }
        public int RoleConfig_Status { get; set; }
        public int Role_Id { get; set; }
    }
}
