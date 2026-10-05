using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IAllergyInfoService
    {
        //Task<int> InsertUpdateAllergyInfo(AllergyInfoEntity entity);
        Task<List<AllergyInfoCustomEntity>> GetResidentAllergies(int patientId);
        //Task<AllergyInfoEntity> GetAllergyInfoDetails(int PAllergy_Id);
        Task<AllergyInfoEntity> GetAllegyDetails(int AllergyId);
        Task<int> RemoveResidentAllergiesInfo(int pAllergyId);
    }
}
