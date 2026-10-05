namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class ScreenPermissionsCustomEntity
    {
        public int Screen_Id { get; set; }
        public string Screen_Desc { get; set; }
        public int AccessRead { get; set; }
        public int AccessWrite { get; set; }
        public int PrintPdf { get; set; }
        public int PrintExcel { get; set; }
    }
}
