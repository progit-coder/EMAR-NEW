using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface ICompanyService
    {
        Task<int> InsertUpdateCompanyMaster(CompanyEntity company);
        Task<List<CompanyCustomEntity>> GetUserActiveCompanyMasterList();
        Task<List<CompanyCustomEntity>> GetAllCompanyMasterList();
        Task<CompanyCustomEntity> GetCompanyDetailsByID(int companyId);
        Task<List<CompanyDropEntity>> GetUserActiveCompanyNames();
        Task<List<CompanyDropEntity>> GetAllActiveCompanyNames();
        Task<List<CompanyUIDDropEntity>> GetCompanyUIDDropDownList();
        Task<int> GetApprovalFlag(int patientId);
        Task<List<FingersdescEntity>> GetFingersDescDrop();
        Task<List<HLDirectionEntity>> GetHLDirectionList();
        Task<List<FTECategoryEntity>> GetAllFTECategory();
        Task<List<EventCategoryEntity>> GetAllEventCategroy(int directionalId, int categoryId);
        Task<int> InsertComapnyConfigDetails(CompanyConfigEntity config);
        Task<List<CompanyConfigEntity>> GetAllCompanyConfigList();
        Task<CompanyConfigEntity> GetCompanyConfigById(int companyConfigId,int hl7Configured, int FteCategoryId);
        Task<List<EventCategoryEntity>> GetCompanyEventCategoriesByPid(int patientId);
        Task<List<StockReportEntity>> GetStockReportForDropData();
        Task<int> UpdateCompaniesStatus(List<CompanyCustomEntity> data);
        Task<int?> GetBiometricFingerConfigByNsId(int nurseStationId);
        Task<List<CompanyDropEntity>> GetStockUserCompanyDrop(int userId);
        Task<CompanyFlagsEntity> GetAllFlagsForCompanyByNSId(int nurseStationId, int? residentId = 0);
        Task<int> GetCompanyHlSevenFlag(int fcailityId, int companyId);
        Task<int> InsertUpdateNurseStationhierarchyMaster(NurseStationHierarchyEntity hierarchyentity);
        Task<List<NurseStationHierarchyEntity>> GetCompanyToBedMapGrid(int? facilityId = null, int? NsId = null);
        Task<NurseStationHierarchyEntity> GetNurseStationHierarchyDetailsById(int hierarchyId);
        Task<NurseStationHierarchyEntity> GetNurseStationHierarchyDetailsbyNsId(int NsId);
    }
}
