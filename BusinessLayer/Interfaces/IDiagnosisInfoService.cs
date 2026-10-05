using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IDiagnosisInfoService
    {
        //Task<int> InsertUpdateDiagnosisInfo(DiagnosisInfoCustomEntity entity);
        Task<List<DiagnosisInfoCustomEntity>> GetResidentDiagnosisInfo(int patientId);
        Task<DiagnosisInfoEntity> GetDiagnosisDetails(int DiagnosisId);
        Task<int> RemoveResidentDiagnosis(int pDiagnosisId);
    }
}
