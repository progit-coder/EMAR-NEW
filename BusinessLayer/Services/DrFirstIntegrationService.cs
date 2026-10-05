using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Repositories;
using LTCPro.Entities;
using EncDec;
using LTCPro.DAL;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public class DrFirstIntegrationService: IDrFirstIntegrationService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDrFirstIntegrationRepository _drFirstIntegrationRepository;
        private readonly ILogger _log;
        public DrFirstIntegrationService(IAutoMapper autoMapper, IDrFirstIntegrationRepository drFirstIntegrationRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._drFirstIntegrationRepository = drFirstIntegrationRepository;
            this._log = log;
        }
        public async Task<int> InsertUpdateApiIntegration(ApiIntegrationEntity integrationdata)
        {
            clsEncDec encrypt = new clsEncDec();
            integrationdata.UserName = encrypt.psEncrypt(integrationdata.UserName);
            integrationdata.Password = encrypt.psEncrypt(integrationdata.Password);
            this._log.Debug("---Executing InsertApiIntegration() in DrFirstIntegrationService----");
            return await Task.FromResult<int>(this._drFirstIntegrationRepository.InsertUpdateApiIntegration(integrationdata));
        }
        public async Task<List<ApiIntegrationEntity>> GetApiIntegrationAllData(int companyID)
        {
            this._log.Debug("---Executing GetApiIntegrationAllData() in DrFirstIntegrationService----");
            clsEncDec encrypt = new clsEncDec();
            var integrationDetails = this._drFirstIntegrationRepository.GetApiIntegrationAllData(companyID);
            for(int i=0;i< integrationDetails.Count;i++)
            {
                integrationDetails[i].UserName= encrypt.psDecrypt(integrationDetails[i].UserName);
                integrationDetails[i].Password = encrypt.psDecrypt(integrationDetails[i].Password);
            }
            return await Task.FromResult<List<ApiIntegrationEntity>>(integrationDetails);
          
        }
        public async Task<ApiIntegrationEntity> GetApiIntegrationDetails(int integrationID)
        {
            clsEncDec encrypt = new clsEncDec();            
            this._log.Debug("---Executing GetApiIntegrationDetails() in DrFirstIntegrationService----");
            var integrationDetails = this._drFirstIntegrationRepository.GetApiIntegrationDetails(integrationID);
            integrationDetails.UserName = encrypt.psDecrypt(integrationDetails.UserName);
            integrationDetails.Password= encrypt.psDecrypt(integrationDetails.Password);
            return await Task.FromResult<ApiIntegrationEntity>(integrationDetails);
        }
        public async Task<string> DrFirstXMLSave(string xmlData)
        {
            this._log.Debug("---Executing DrFirstXMLSave() in DrFirstIntegrationService----");
            return await Task.FromResult<string>(this._drFirstIntegrationRepository.DrFirstXMLSave(xmlData));
        }
        public async Task<List<DrFirstOrderCheck_ResultEntity>> GetDrFirstOrderInfo(int orderId)
        {
            this._log.Debug("---Executing GetDrFirstOrderInfo() in ReportsService----");
            var entity = this._drFirstIntegrationRepository.GetDrFirstOrderInfo(orderId);
            return await Task.FromResult(entity);
        }
        public async Task<int> UpdateApprovalStatus(int PrescriptionID, string Username, string Password)
        {
            this._log.Debug("---Executing UpdateApprovalStatus() in CompanyService----");
            return await Task.FromResult<int>(this._drFirstIntegrationRepository.UpdateApprovalStatus(PrescriptionID, Username, Password));
        }

        public async Task<List<DrFirstOrderCheck_ResultEntity>> GetDrFirstOrderCheck(int patientId)
        {
            this._log.Debug("---Executing GetDrFirstOrderCheck() in DrFirstIntegrationService----");
            var entity = this._drFirstIntegrationRepository.GetDrFirstOrderCheck(patientId);
            return await Task.FromResult(entity);
        }

        public async Task<List<PrcDrFirstOrderHL7_ResultEntity>> GetHL7DrFirstOrderCheck(int patientId)
        {
            this._log.Debug("---Executing GetHL7DrFirstOrderCheck() in DrFirstIntegrationService----");
            var entity = this._drFirstIntegrationRepository.GetHL7DrFirstOrderCheck(patientId);
            return await Task.FromResult(entity);
        }
        public async Task<int> InsertDrFirstMap(List<DrFirstEntity> entity)
        {
            this._log.Debug("---Executing InsertDrFirstMap() in DrFirstIntegrationService----");
            return await Task.FromResult<int>(this._drFirstIntegrationRepository.InsertDrFirstMap(entity));
        }
        public async Task<List<NurseCommentTypeDropEntity>> GetNurseComments()
        {
            this._log.Debug("---Executing GetNurseComments() in DrFirstIntegrationService----");
            var entity = this._drFirstIntegrationRepository.GetNurseComments();
            return await Task.FromResult(entity);
        }
        public async Task<List<MedicationReasonEntity>> GetNoteComments()
        {
            this._log.Debug("---Executing GetNoteComments() in DrFirstIntegrationService----");
            var entity = this._drFirstIntegrationRepository.GetNoteComments();
            return await Task.FromResult(entity);
        }
        public async Task<List<PrcGetReportDrFirst_ResultEntity>> PrcGetReportDrFirst(int patientId)
        {
            this._log.Debug("---Executing PrcGetReportDrFirst() in DrFirstIntegrationService----");
            var entity = this._drFirstIntegrationRepository.PrcGetReportDrFirst(patientId);
            return await Task.FromResult(entity);
        }
        public async Task<List<DemographicResidentDropEnity>> GetDrFirstResidentDrop(int userId)
        {
            this._log.Debug("---Executing GetDrFirstResidentDrop() in DrFirstIntegrationService----");
            var entity = this._drFirstIntegrationRepository.GetDrFirstResidentDrop(userId);
            return await Task.FromResult<List<DemographicResidentDropEnity>>(entity);
        }
        public async Task<int> InsertHlSevenApprove(HlSevenApproveEntity entity)
        {
            this._log.Debug("---Executing InsertHlSevenApprove() in DrFirstIntegrationService----");
            var result = this._drFirstIntegrationRepository.InsertHlSevenApprove(entity);
            return await Task.FromResult<int>(result);
        }
        public async Task<int> InsertDrFirstApprove(int usetId, int DrFirstOrderId)
        {
            this._log.Debug("---Executing InsertDrFirstApprove() in DrFirstIntegrationService----");
            var result = this._drFirstIntegrationRepository.InsertDrFirstApprove(usetId, DrFirstOrderId);
            return await Task.FromResult<int>(result);
        }
        public async Task<string> DownloadXML(int fileId)
        {
            this._log.Debug("---Executing DownloadXML() in DrFirstIntegrationService----");
            var result = this._drFirstIntegrationRepository.DownloadXML(fileId);
            return await Task.FromResult<string>(result);
        }
    }
}
