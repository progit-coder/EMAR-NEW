using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class AllergyInfoService : IAllergyInfoService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IAllergyInfoRepository _allergyInfoRepository;
        private readonly ILogger _log;

        public AllergyInfoService(IAutoMapper autoMapper, IAllergyInfoRepository allergyInfoRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._allergyInfoRepository = allergyInfoRepository;
            this._log = log;
        }
        public async Task<List<AllergyInfoCustomEntity>> GetResidentAllergies(int patientId)
        {
            this._log.Debug("---Executing GetResidentAllergies() in AllergyInfoService----");
            return await Task.FromResult<List<AllergyInfoCustomEntity>>(this._allergyInfoRepository.GetResidentAllergies(patientId));
        }
        //public async Task<int> InsertUpdateAllergyInfo(AllergyInfoEntity entity)
        //{
        //    this._log.Debug("---Executing InsertUpdateAllergyInfo() in AllergyInfoService----");
        //    return await Task.FromResult<int>(this._allergyInfoRepository.InsertUpdateAllergyInfo(entity));

        //}
        //public async Task<AllergyInfoEntity> GetAllergyInfoDetails(int PAllergy_Id)
        //{
        //    this._log.Debug("---Executing GetAllergyInfoDetails() in AllergyInfoService----");

        //    return await Task.FromResult<AllergyInfoEntity>(this._allergyInfoRepository.GetAllergyInfoDetails(PAllergy_Id));
        //}

        public async Task<AllergyInfoEntity> GetAllegyDetails(int AllergyId)
        {
            this._log.Debug("---Executing GetAllegyDetails() in AllergyInfoService----");

            return await Task.FromResult<AllergyInfoEntity>(this._allergyInfoRepository.GetAllegyDetails(AllergyId));
        }
        public async Task<int> RemoveResidentAllergiesInfo(int pAllergyId)
        {
            this._log.Debug("---Executing RemoveResidentAllergiesInfo() in AllergyInfoService----");

            return await Task.FromResult<int>(this._allergyInfoRepository.RemoveResidentAllergiesInfo(pAllergyId));
        }
        
    }
}
