using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IDiagnosisInfoRepository
    {
        //List<DiagnosisInfoEntity> GetOutBoundDiagnosis();
        //int InsertUpdateDiagnosisInfo(DiagnosisInfoCustomEntity entity);
        List<DiagnosisInfoCustomEntity> GetResidentDiagnosisInfo(int patientId);
        //List<DiagnosisInfoEntity> GetApprovalPendingDiagnosis(int patientId);
        //List<DiagnosisInfoEntity> GetApprovalPendingDiagnosis();
        // ApproveDiagnosis(ApprovalPendingCustomEntity entity);
        DiagnosisInfoEntity GetDiagnosisDetails(int DiagnosisId);
        int RemoveResidentDiagnosis(int pDiagnosisId);
    }
}
