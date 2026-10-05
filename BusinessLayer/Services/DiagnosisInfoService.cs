using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class DiagnosisInfoService : IDiagnosisInfoService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDiagnosisInfoRepository _diagnosisInfoRepository;
        private readonly ILogger _log;

        public DiagnosisInfoService(IAutoMapper autoMapper, IDiagnosisInfoRepository diagnosisInfoRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._diagnosisInfoRepository = diagnosisInfoRepository;
            this._log = log;
        }
        public async Task<List<DiagnosisInfoCustomEntity>> GetResidentDiagnosisInfo(int patientId)
        {
            this._log.Debug("---Executing GetResidentDiagnosisInfo() in DiagnosisInfoService----");
            return await Task.FromResult<List<DiagnosisInfoCustomEntity>>(this._diagnosisInfoRepository.GetResidentDiagnosisInfo(patientId));
        }

        //public async Task<int> InsertUpdateDiagnosisInfo(DiagnosisInfoCustomEntity entity)
        //{
        //    this._log.Debug("---Executing InsertUpdateDiagnosisInfo() in DiagnosisInfoService----");
        //    return await Task.FromResult<int>(this._diagnosisInfoRepository.InsertUpdateDiagnosisInfo(entity));
        //}
        public async Task<DiagnosisInfoEntity> GetDiagnosisDetails(int DiagnosisId)
        {
            this._log.Debug("---Executing GetDiagnosisDetails(DiagnosisId) in DiagnosisInfoService----");

            return await Task.FromResult<DiagnosisInfoEntity>(this._diagnosisInfoRepository.GetDiagnosisDetails(DiagnosisId));
        }
        public async Task<int> RemoveResidentDiagnosis(int pDiagnosisId)
        {
            this._log.Debug("---Executing RemoveResidentDiagnosis(pDiagnosisId) in DiagnosisInfoService----");

            return await Task.FromResult<int>(this._diagnosisInfoRepository.RemoveResidentDiagnosis(pDiagnosisId));
        }
        
    }
}
