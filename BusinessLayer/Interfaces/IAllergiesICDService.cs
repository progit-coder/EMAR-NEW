using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IAllergiesICDService
    {
        Task<List<AllergyInfoMastersEntity>> GetAllAllergiesList(int type);
        Task<List<AllergyInfoMastersEntity>> GetAllergiesByName(string searchPattern, int type);
        Task<List<AllergyInfoMastersEntity>> GetActiveAllergiesByName(string searchPattern);
        Task<int> InsertUpdateAllergy(AllergyInfoMastersEntity entity);
        Task<int> InsertUpdateAllergyFromFile(List<AllergyInfoMastersEntity> entities);
        Task<int> InsertUpdateICD10(ICD10Entity entity);
        System.IO.StringReader GeneratePDF();
        DataTable ToDataTable<T>(List<T> iList);
        Task<int> InsertUpdateICD10FromFile(List<ICD10Entity> entities);
        Task<ICD10GridEntity> GetICD10ByRawFormat(SearchCustomEntity data);
        Task<List<ICD10Entity>> GetActiveICD10ByRawFormat(string searchPattern);
        Task<List<ICD10Entity>> GetAllICD10();
        Task<List<ICD10Entity>> GetICDGrid();
        Task<int> UpdateICDsStatus(List<ICD10Entity> data);
        Task<int> UpdateAllergysStatus(List<AllergyInfoMastersEntity> data);
    }
}
