using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CompoundOrderEntity
    {
        public int PComp_Id { get; set; }
        public int POrder_Id { get; set; }
        public string ComponentType { get; set; }
        public string ComponentId { get; set; }
        public string DrugName { get; set; }
        public string CodingSystem { get; set; }
        public string CAlternateId { get; set; }
        public string CAlternateText { get; set; }
        public string CACodingSystem { get; set; }
        public string ComponentAmount { get; set; }
        public string UnitsId { get; set; }
        public string UnitsText { get; set; }
        public string UCodingSystem { get; set; }
        public string UAlternateId { get; set; }
        public string UAlternateText { get; set; }
        public string UACodingSystem { get; set; }
        public string ComponentStrength { get; set; }
        public string CSUnitsId { get; set; }
        public string CSUnitsText { get; set; }
        public string CSUCodingSystem { get; set; }
        public string CSUAlternateId { get; set; }
        public string CSUAlternateText { get; set; }
        public string CSUACodingSystem { get; set; }
        public string SCodeId { get; set; }
        public string SCodeText { get; set; }
        public string SCCodingSystem { get; set; }
        public string SCAlternateId { get; set; }
        public string SCAlternateText { get; set; }
        public string SCACodingSystem { get; set; }
        public string DrugStrengthVolume { get; set; }
        public string CDSUnitsId { get; set; }
        public string CDSUnitsText { get; set; }
        public string CDSUnitsCodingSystem { get; set; }
        public string CDSVAlternateId { get; set; }
        public string CDSVAlternateText { get; set; }
        public string CDSVACodingSystem { get; set; }
        public string CodingSystemVersion { get; set; }
        public string ACodingSystemVersion { get; set; }
        public string OriginalText { get; set; }
        public int PComp_Status { get; set; }
        public Nullable<int> PComp_CreatedBy { get; set; }
        public System.DateTime PComp_CreatedDate { get; set; }
        public Nullable<int> PCOutBoundFileStatus { get; set; }
        public Nullable<int> PCOutBoundApproval { get; set; }
        public Nullable<int> PCOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PCOutBoundApprovalOn { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual UserEntity User1 { get; set; }
        public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
}
