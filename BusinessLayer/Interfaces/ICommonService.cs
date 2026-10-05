using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface ICommonService
    {
        Task<List<CountryEntity>> GetAllCountries();
        Task<List<GenderEntity>> GetGenders();
        Task<List<SuffixEntity>> GetSuffixes();
        Task<List<FTECategoryEntity>> GetFTECategories();
        Task<List<FteConnectionEntity>> GetFTEConnections();
        Task<int> InsertUpdatePatientType(PatientTypeEntity entity);
        Task<List<PatientTypeCustomEntity>> GetPatientTypeData();
        Task<PatientTypeCustomEntity> GetPatientTypeByID(int patientTypeId);
        Task<List<PatientTypeCustomEntity>> GetPatientTypeDrop(int patientId);
        Task<int> UpdatePatientTypeStatus(List<PatientTypeCustomEntity> data);
        Task<RecentFacEntity> GetUserRecentFacNs(int userId);
        Task<List<OrderFavouriteCustomEntity>> GetOderFavInfo();
        Task<int> InsertUpdateMeasurementsandUserInputs(PrcInsertOrderFavFacilityConfigEntity obj);
        Task<List<PrcGetOrderFavConfigInfoEntity>> GetOrderFavDetails(int FacilityId);
        Task<PrcGetOrderFavConfigByCodeEntity> GetOrderFavConfigDetailsById(CustomOrderFavInfoDataEntity entity);
        //Document Check

        Task<int> InsertUpdateDocumentCheck(List<DrugAdministerEntity> data);
        Task<int> RemoveDocumentCheck(List<DrugAdministerEntity> data);
        Task<List<GetDocumentCheckDataEntity>> GetOrdersListData(OrdersListFilter filter);
        Task<List<GetDocumentCheckDataEntity>> GetDrugsListData(int residentId, string startdate, string enddate);
        Task<List<DosesDetailsEntity>> GetPendingDosesList(int userId,string nsIds,int? facId);
        Task<int> InsertIgnoreDosesDetails(long drugAdministerId);
        Task<List<PrcGetDocAdminOrderAudit_ResultEntity>> GetDocAdminOrderAudit(Int64 DrugAdminsterID);
        Task<List<OutboundErrorDetailsEntity>> GetOutboundErrorDetails(int userId);
        Task<int> InsertIgnoreOutboundErrorDetails(int fileId);
        Task<int> GetUserPassedDueFlag(int userId);
        Task<int> InsertUpdateRefillMailConfig(RefillMailConfigCustomEntity entity);
        Task<List<RefillMailConfigGridData>> GetRefillConfigGridData(int userId);
        Task<string> GetTimeZoneDate(int nursingStationId);
        Task<int> inserUpdateMailconfigDetails(MailConfigEntity entity);
        Task<int> InsertUpdateUserRecentFacNs(RecentFacEntity obj);
       Task< int> NoAdminitration(DrugAdministerEntity drugAdministerEntity);
        Task<List<StateEntity>> GetState();
        Task<List<PrimarySpeciality>> GetPrimarySpeciality();
        Task<List<CredentialMaster>> GetCredentialMaster();
    }
}
