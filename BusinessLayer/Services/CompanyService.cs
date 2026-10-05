using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using log4net.Core;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class CompanyService : ICompanyService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger _log;
        public CompanyService(IAutoMapper autoMapper, ICompanyRepository companyRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._companyRepository = companyRepository;
            this._log = log;
        }
        public async Task<int> GetApprovalFlag(int patientId)
        {
            this._log.Debug("---Executing GetApprovalFlag() in CompanyService----");
            return await Task.FromResult<int>(this._companyRepository.GetApprovalFlag(patientId));
        }
        public async Task<List<CompanyCustomEntity>> GetUserActiveCompanyMasterList()
        {
            this._log.Debug("---Executing GetActiveCompanyMasterList() in CompanyService----");
            return await Task.FromResult<List<CompanyCustomEntity>>(this._companyRepository.GetUserActiveCompanyMasterList());
        }
        public async Task<List<CompanyCustomEntity>> GetAllCompanyMasterList()
        {
            this._log.Debug("---Executing GetAllCompanyMasterList() in CompanyService----");
            return await Task.FromResult<List<CompanyCustomEntity>>(this._companyRepository.GetAllCompanyMasterList());
        }

        public async Task<List<CompanyConfigEntity>> GetAllCompanyConfigList()
        {
            this._log.Debug("---Executing GetAllCompanyConfigList() in CompanyService----");
            return await Task.FromResult<List<CompanyConfigEntity>>(this._companyRepository.GetAllCompanyConfigList());
        }
        public async Task<CompanyCustomEntity> GetCompanyDetailsByID(int companyId)
        {
            this._log.Debug("---Executing GetCompanyDetailsByID() in CompanyService----");
            return await Task.FromResult<CompanyCustomEntity>(this._companyRepository.GetCompanyDetailsByID(companyId));
        }
        public async Task<int> InsertComapnyConfigDetails(CompanyConfigEntity config)
        {
            this._log.Debug("---Executing InsertComapnyConfigDetails() in CompanyService----");
            return await Task.FromResult<int>(this._companyRepository.InsertComapnyConfigDetails(config));
        }
        public async Task<int> InsertUpdateCompanyMaster(CompanyEntity company)
        {
            this._log.Debug("---Executing InsertUpdateCompanyMaster() in CompanyService----");
            return await Task.FromResult<int>(this._companyRepository.InsertUpdateCompanyMaster(company));
        }
        public async Task<List<CompanyDropEntity>> GetUserActiveCompanyNames()
        {
            this._log.Debug("---Executing GetActiveCompanyNames() in CompanyService----");
            return await Task.FromResult<List<CompanyDropEntity>>(this._companyRepository.GetUserActiveCompanyNames());

        }
        public async Task<List<FTECategoryEntity>> GetAllFTECategory()
        {
            this._log.Debug("---Executing GetAllFTECategory() in CompanyService---- ");
            return await Task.FromResult<List<FTECategoryEntity>>(this._companyRepository.GetAllFTECategory());
        }
        public async Task<List<EventCategoryEntity>> GetAllEventCategroy(int directionalId, int categoryId)
        {
            this._log.Debug("---Executing GetAllEventCategroy() in CompanyService---- ");
            return await Task.FromResult<List<EventCategoryEntity>>(this._companyRepository.GetAllEventCategroy(directionalId, categoryId));
        }
        public async Task<List<HLDirectionEntity>> GetHLDirectionList()
        {
            this._log.Debug("---Executing GetHLDirectionList() in CompanyService----");
            return await Task.FromResult<List<HLDirectionEntity>>(this._companyRepository.GetHLDirectionList());
        }
        public async Task<List<CompanyDropEntity>> GetAllActiveCompanyNames()
        {
            this._log.Debug("---Executing GetAllActiveCompanyNames() in CompanyService----");
            return await Task.FromResult<List<CompanyDropEntity>>(this._companyRepository.GetAllActiveCompanyNames());
        }        
        public async Task<List<CompanyUIDDropEntity>> GetCompanyUIDDropDownList()
        {
            this._log.Debug("---Executing GetCompanyUIDDropDownList() in CompanyService----");
            return await Task.FromResult<List<CompanyUIDDropEntity>>(this._companyRepository.GetCompanyUIDDropDownList());
        }
        public async Task<List<FingersdescEntity>> GetFingersDescDrop()
        {
            this._log.Debug("---Executing GetFingersDescDrop() in CompanyService----");
            return await Task.FromResult<List<FingersdescEntity>>(this._companyRepository.GetFingersDescDrop());
        }
        public async Task<CompanyConfigEntity> GetCompanyConfigById(int companyConfigId, int hl7Configured, int FteCategoryId)
        {
            this._log.Debug("---Executing GetCompanyConfigById() in CompanyService----");
            return await Task.FromResult<CompanyConfigEntity>(this._companyRepository.GetCompanyConfigById(companyConfigId, hl7Configured, FteCategoryId));
        }
        public async Task<List<EventCategoryEntity>> GetCompanyEventCategoriesByPid(int patientId)
        {
            this._log.Debug("---Executing GetCompanyEventCategoriesByPid() in CompanyService----");
            return await Task.FromResult<List<EventCategoryEntity>>(this._companyRepository.GetCompanyEventCategoriesByPid(patientId));
        }
        public async Task<List<StockReportEntity>> GetStockReportForDropData()
        {
            this._log.Debug("---Executing GetStockReportForDropData() in CompanyService----");
            return await Task.FromResult<List<StockReportEntity>>(this._companyRepository.GetStockReportForDropData());
        }
        public async Task<int> UpdateCompaniesStatus(List<CompanyCustomEntity> data)
        {
            this._log.Debug("---Executing GetNurseShifts() in CompanyService----");
            return await Task.FromResult<int>(this._companyRepository.UpdateCompaniesStatus(data));
        }
        public async Task<int?> GetBiometricFingerConfigByNsId(int nurseStationId)
        {
            this._log.Debug("---Executing GetBiometricFingerConfigByNsId() in CompanyService----");
            return await Task.FromResult<int?>(this._companyRepository.GetBiometricFingerConfigByNsId(nurseStationId));
        }
        public async Task<List<CompanyDropEntity>> GetStockUserCompanyDrop(int userId)
        {
            this._log.Debug("---Executing GetStockUserCompanyDrop() in CompanyService----");
            return await Task.FromResult<List<CompanyDropEntity>>(this._companyRepository.GetStockUserCompanyDrop(userId));
        }
        public async Task<CompanyFlagsEntity> GetAllFlagsForCompanyByNSId(int nurseStationId, int? residentId = 0)
        {
            this._log.Debug("---Executing GetDrFirstFlagForCompanyByNSId() in CompanyService----");
            return await Task.FromResult<CompanyFlagsEntity>(this._companyRepository.GetAllFlagsForCompanyByNSId(nurseStationId,residentId));
        }
        public async Task<int> GetCompanyHlSevenFlag(int fcailityId, int companyId)
        {
            this._log.Debug("---Executing GetCompanyHlSevenFlag() in CompanyService----");
            return await Task.FromResult<int>(this._companyRepository.GetCompanyHlSevenFlag(fcailityId, companyId));
        }
        public async Task<int> InsertUpdateNurseStationhierarchyMaster(NurseStationHierarchyEntity hierarchyentity)
        {
            this._log.Debug("---Executing InsertUpdateNurseStationhierarchyMaster() in CompanyService----");
            return await Task.FromResult<int>(this._companyRepository.InsertUpdateNurseStationhierarchyMaster(hierarchyentity));
        }
        public async Task<List<NurseStationHierarchyEntity>> GetCompanyToBedMapGrid(int? facilityId = null, int? NsId = null)
        {
            this._log.Debug("---Executing GetNurseStationHierarchyDetailsbyNsId() in CompanyService----");
            return await Task.FromResult<List<NurseStationHierarchyEntity>>(this._companyRepository.GetCompanyToBedMapGrid(facilityId, NsId));
        }
        public async Task<NurseStationHierarchyEntity> GetNurseStationHierarchyDetailsById(int hierarchyId)
        {
            this._log.Debug("---Executing GetNurseStationHierarchyDetailsById() in CompanyService----");
            return await Task.FromResult<NurseStationHierarchyEntity>(this._companyRepository.GetNurseStationHierarchyDetailsById(hierarchyId));
        }
        public async Task<NurseStationHierarchyEntity> GetNurseStationHierarchyDetailsbyNsId(int NsId)
        {
            this._log.Debug("---Executing GetNurseStationHierarchyDetailsbyNsId() in CompanyService----");
            return await Task.FromResult<NurseStationHierarchyEntity>(this._companyRepository.GetNurseStationHierarchyDetailsbyNsId(NsId));
        }
    }
}
