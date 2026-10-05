using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface ICompanyRepository
    {
        int InsertUpdateCompanyMaster(CompanyEntity company);
        List<CompanyCustomEntity> GetUserActiveCompanyMasterList();
        List<CompanyCustomEntity> GetAllCompanyMasterList();
        CompanyCustomEntity GetCompanyDetailsByID(int companyId);
        string GetCompanyNameById(int companyId);
        List<CompanyDropEntity> GetUserActiveCompanyNames();
        List<CompanyDropEntity> GetAllActiveCompanyNames();
        List<CompanyUIDDropEntity> GetCompanyUIDDropDownList();
        int GetApprovalFlag(int patientId);
        List<FingersdescEntity> GetFingersDescDrop();
        List<HLDirectionEntity> GetHLDirectionList();
        List<FTECategoryEntity> GetAllFTECategory();
        List<EventCategoryEntity> GetAllEventCategroy(int directionalId, int categoryId);
        int InsertComapnyConfigDetails(CompanyConfigEntity config);
        List<CompanyConfigEntity> GetAllCompanyConfigList();
        CompanyConfigEntity GetCompanyConfigById(int companyConfigId, int hl7Configured, int FteCategoryId);
        List<EventCategoryEntity> GetCompanyEventCategoriesByPid(int patientId);
        List<StockReportEntity> GetStockReportForDropData();
        int UpdateCompaniesStatus(List<CompanyCustomEntity> data);
        int? GetBiometricFingerConfigByNsId(int nurseStationId);
        List<CompanyDropEntity> GetStockUserCompanyDrop(int userId);
        CompanyFlagsEntity GetAllFlagsForCompanyByNSId(int nurseStationId,int? residentId= 0);
        int GetApprovalFlagByFacilityId(int facilityId);
        int GetCompanyHlSevenFlag(int fcailityId, int companyId);
        int InsertUpdateNurseStationhierarchyMaster(NurseStationHierarchyEntity hierarchyentity);
        List<NurseStationHierarchyEntity> GetCompanyToBedMapGrid(int? facilityId = null, int? NsId = null);
        NurseStationHierarchyEntity GetNurseStationHierarchyDetailsById(int hierarchyId);
        NurseStationHierarchyEntity GetNurseStationHierarchyDetailsbyNsId(int NsId);
    }
}
