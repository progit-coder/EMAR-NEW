using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.Repositories
{
    public interface IDrFirstIntegrationRepository
    {
        int InsertUpdateApiIntegration(ApiIntegrationEntity integrationdata);
        List<ApiIntegrationEntity> GetApiIntegrationAllData(int companyID);
        ApiIntegrationEntity GetApiIntegrationDetails(int integrationID);
        string DrFirstXMLSave(string xmlData);
        List<DrFirstOrderCheck_ResultEntity> GetDrFirstOrderInfo(int orderId);
        int UpdateApprovalStatus(int PrescriptionID,string Username,string Password);
        List<DrFirstOrderCheck_ResultEntity> GetDrFirstOrderCheck(int patientId);
        List<PrcDrFirstOrderHL7_ResultEntity> GetHL7DrFirstOrderCheck(int patientId);
        int InsertDrFirstMap(List<DrFirstEntity> entity);
        List<NurseCommentTypeDropEntity> GetNurseComments();
        List<MedicationReasonEntity> GetNoteComments();        
        List<PrcGetReportDrFirst_ResultEntity> PrcGetReportDrFirst(int patientId);
        List<DemographicResidentDropEnity> GetDrFirstResidentDrop(int userId);
        int InsertHlSevenApprove(HlSevenApproveEntity entity);
        int InsertDrFirstApprove(int usetId, int DrFirstOrderId);
        string DownloadXML(int fileId);
    }
}
