using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class OutboundFilesService : IOutboundFilesService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IFTEConfigurationRepository _fteConfigRepository;
        private readonly IOutboundFileInformationRepository _fileInfoRepository;
        private readonly IAllergyInfoRepository _allergyInfoRepository;
        private readonly IDiagnosisInfoRepository _diagnosisInfoRepository;
        private readonly IResidentDemographicRepository _residentDemographicInfoRepository;

        private readonly ILogger _log;
        public OutboundFilesService(IAutoMapper autoMapper, IFTEConfigurationRepository fteConfigRepository, IOutboundFileInformationRepository fileInfoRepository,
            IAllergyInfoRepository allergyInfoRepository, IDiagnosisInfoRepository diagnosisInfoRepository, IResidentDemographicRepository residentDemographicInfoRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._fteConfigRepository = fteConfigRepository;
            this._fileInfoRepository = fileInfoRepository;
            this._allergyInfoRepository = allergyInfoRepository;
            this._diagnosisInfoRepository = diagnosisInfoRepository;
            this._residentDemographicInfoRepository = residentDemographicInfoRepository;
            this._log = log;
        }
        public async Task<List<OutBoundFileInformationEntity>> GetOutboundFiles(int companyId, string fileCategory, string fromDate, string toDate, string patientName)
        {
            this._log.Debug("---Executing GetOutboundFiles() in OutboundFilesService----");
            return await Task.FromResult<List<OutBoundFileInformationEntity>>(this._fileInfoRepository.GetOutboundFiles(companyId, fileCategory, fromDate, toDate, patientName));
        }
        public async Task<OutBoundFileInformationEntity> GetOutboundFileById(int fileId)
        {
            this._log.Debug("---Executing GetOutboundFileById() in OutboundFilesService----");
            return await Task.FromResult<OutBoundFileInformationEntity>(this._fileInfoRepository.GetOutboundFileById(fileId));

        }
        public async Task<FileInformationViewCustomEntity> GetFileAckDataForOutbound(int FileId)
        {
            this._log.Debug("---Executing GetFileAckData() in OutboundFilesService----");
            return await Task.FromResult<FileInformationViewCustomEntity>(this._fileInfoRepository.GetFileAckDataForOutbound(FileId));
        }
        //public async Task<List<OutboundRecordsCustomEntity>> GetOutBoundList()
        //{
        //    List<OutboundRecordsCustomEntity> records = new List<OutboundRecordsCustomEntity>();
        //    this._log.Debug("---Executing GetOutBoundList() in OutboundFilesService----");
        //    records.AddRange(this._allergyInfoRepository.GetOutBoundAllergies().OrderByDescending(a => a.PAOutBoundApprovalOn).Select
        //        (a => new OutboundRecordsCustomEntity
        //        {
        //            Patient_Id = a.Patient_Id,
        //            Patient_Name = this._residentDemographicInfoRepository.GetResidentName(a.Patient_Id),
        //            RecordId = a.PAllergy_Id,
        //            Category = "Allergy",
        //            CategoryId = 1,
        //            UploadedBy = Convert.ToInt32(a.PAllergy_CreatedBy),
        //            ApprovedBy = Convert.ToInt32(a.PAOutBoundApprovalBy),
        //            ApprovedOn = Convert.ToDateTime(a.PAOutBoundApprovalOn)

        //        }).ToList());

        //    records.AddRange(this._diagnosisInfoRepository.GetOutBoundDiagnosis().OrderByDescending(a => a.PDGOutBoundApprovalOn).Select
        //        (a => new OutboundRecordsCustomEntity
        //        {
        //            Patient_Id = a.Patient_Id,
        //            Patient_Name = this._residentDemographicInfoRepository.GetResidentName(a.Patient_Id),
        //            RecordId = a.PDiagnosis_Id,
        //            Category = "Diagnosis",
        //            CategoryId = 1,
        //            UploadedBy = Convert.ToInt32(a.PDiagnosis_CreatedBy),
        //            ApprovedBy = Convert.ToInt32(a.PDGOutBoundApprovalBy),
        //            ApprovedOn = Convert.ToDateTime(a.PDGOutBoundApprovalOn)
        //        }).ToList());
        //    records.AddRange(this._residentDemographicInfoRepository.GetOutBoundDemographics().OrderByDescending(a => a.PDOutBoundApprovalOn).Select
        //        (a => new OutboundRecordsCustomEntity
        //        {
        //            Patient_Id = a.Patient_Id,
        //            Patient_Name = this._residentDemographicInfoRepository.GetResidentName(a.Patient_Id),
        //            RecordId = a.Patient_Id,
        //            Category = "Demographics",
        //            CategoryId = 1,
        //            UploadedBy = Convert.ToInt32(a.Patient_CreatedBy),
        //            ApprovedBy = Convert.ToInt32(a.PDOutBoundApprovalBy),
        //            ApprovedOn = Convert.ToDateTime(a.PDOutBoundApprovalOn)
        //        }).ToList());
        //    records.AddRange(this._residentDemographicInfoRepository.GetOutBoundDischarges().OrderByDescending(a => a.PVOutBoundApprovalOn).Select
        //       (a => new OutboundRecordsCustomEntity
        //       {
        //           Patient_Id = a.Patient_Id,
        //           Patient_Name = this._residentDemographicInfoRepository.GetResidentName(a.Patient_Id),
        //           RecordId = a.Patient_Id,
        //           Category = "Discharge",
        //           CategoryId = 1,
        //           UploadedBy = Convert.ToInt32(a.PVisit_CreatedBy),
        //           ApprovedBy = Convert.ToInt32(a.PVOutBoundApprovalBy),
        //           ApprovedOn = Convert.ToDateTime(a.PVOutBoundApprovalOn)
        //       }).ToList());
        //    records.AddRange(this._residentDemographicInfoRepository.GetOutBoundTransfers().OrderByDescending(a => a.PVOutBoundApprovalOn).Select
        //       (a => new OutboundRecordsCustomEntity
        //       {
        //           Patient_Id = a.Patient_Id,
        //           Patient_Name = this._residentDemographicInfoRepository.GetResidentName(a.Patient_Id),
        //           RecordId = a.Patient_Id,
        //           Category = "Transfer",
        //           CategoryId = 1,
        //           UploadedBy = Convert.ToInt32(a.PVisit_CreatedBy),
        //           ApprovedBy = Convert.ToInt32(a.PVOutBoundApprovalBy),
        //           ApprovedOn = Convert.ToDateTime(a.PVOutBoundApprovalOn)
        //       }).ToList());

        //    return await Task.FromResult<List<OutboundRecordsCustomEntity>>(records);
        //}
        //public async Task<int> GenerateFileSaveData(List<OutboundRecordsCustomEntity> records)
        //{
        //    this._log.Debug("---Executing GenerateFileSaveData() in OutboundFilesService----");
        //    return await Task.FromResult<int>(this._fileInfoRepository.GenerateFileSaveData(records));

        //}
        public async Task<List<OutBoundFileInformationEntity>> GetOutboundFilesStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName)
        {
            this._log.Debug("---Executing GetOutboundFilesStatus() in OutboundFilesService----");
            return await Task.FromResult<List<OutBoundFileInformationEntity>>(this._fileInfoRepository.GetOutboundFilesStatus(companyId, fileStatus, fromDate, toDate, patientName));
        }
        public async Task<List<ResidentDropEntity>> GetOutboundResidentsDrop(int userId, int companyId)
        {
            this._log.Debug("---Executing GetInboundResidentsDrop() in OutboundFilesService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._fileInfoRepository.GetOutboundResidentsDrop(userId, companyId));
        }
    }
}
