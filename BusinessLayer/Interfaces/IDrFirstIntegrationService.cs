using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;
using LTCPro.DAL;

namespace LTCPro.ServiceLayer
{
    public interface IDrFirstIntegrationService
    {
        Task<int> InsertUpdateApiIntegration(ApiIntegrationEntity integrationdata);
        Task<List<ApiIntegrationEntity>> GetApiIntegrationAllData(int companyID);
        Task<ApiIntegrationEntity> GetApiIntegrationDetails(int integrationID);
        Task<string> DrFirstXMLSave(string xmlData);
        Task<List<DrFirstOrderCheck_ResultEntity>> GetDrFirstOrderInfo(int orderId);
        Task<int> UpdateApprovalStatus(int PrescriptionID, string Username, string Password);
        Task<List<DrFirstOrderCheck_ResultEntity>> GetDrFirstOrderCheck(int patientId);
        Task<List<PrcDrFirstOrderHL7_ResultEntity>> GetHL7DrFirstOrderCheck(int patientId);
        Task<int> InsertDrFirstMap(List<DrFirstEntity> entity);
        Task<List<NurseCommentTypeDropEntity>> GetNurseComments();
        Task<List<MedicationReasonEntity>> GetNoteComments();
        Task<List<PrcGetReportDrFirst_ResultEntity>> PrcGetReportDrFirst(int patientId);
        Task<List<DemographicResidentDropEnity>> GetDrFirstResidentDrop(int userId);
        Task<int> InsertHlSevenApprove(HlSevenApproveEntity entity);
        Task<int> InsertDrFirstApprove(int usetId, int DrFirstOrderId);
        Task<string> DownloadXML(int fileId);
    }
}
