using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IAllergyInfoRepository
    {
        //List<AllergyInfoEntity> GetOutBoundAllergies();
        //int InsertUpdateAllergyInfo(AllergyInfoEntity entity);
        List<AllergyInfoCustomEntity> GetResidentAllergies(int patientId);
        //List<AllergyInfoEntity> GetApprovalPendingAllergies(int patientId);
        //List<AllergyInfoEntity> GetApprovalPendingAllergies();
        //int ApproveAllergy(ApprovalPendingCustomEntity entity);
        //AllergyInfoEntity GetAllergyInfoDetails(int PAllergy_Id);
        AllergyInfoEntity GetAllegyDetails(int AllergyId);
        int RemoveResidentAllergiesInfo(int pAllergyId);
    }
}
