using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IAllergiesICDRepository
    {
        List<AllergyInfoMastersEntity> GetAllAllergiesList(int type);
        List<AllergyInfoMastersEntity> GetAllergiesByName(string searchPattern, int type);
        List<AllergyInfoMastersEntity> GetActiveAllergiesByName(string searchPattern);
        int InsertUpdateAllergy(AllergyInfoMastersEntity entity);
        int InsertUpdateAllergyFromFile(List<AllergyInfoMastersEntity> entities);
        int InsertUpdateICD10(ICD10Entity entity);
        int InsertUpdateICD10FromFile(List<ICD10Entity> entities);
        ICD10GridEntity GetICD10ByRawFormat(SearchCustomEntity data);
        List<ICD10Entity> GetActiveICD10ByRawFormat(string searchPattern);
        List<ICD10Entity> GetAllICD10();
        List<ICD10Entity> GetICDGrid();
        int UpdateICDsStatus(List<ICD10Entity> data);
        int UpdateAllergysStatus(List<AllergyInfoMastersEntity> data);

    }
}
