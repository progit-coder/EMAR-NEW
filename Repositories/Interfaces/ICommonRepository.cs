using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface ICommonRepository
    {
        List<CountryEntity> GetCountries();
        List<GenderEntity> GetGenders();
        List<SuffixEntity> GetSuffix();
        List<FTECategoryEntity> GetFTECategories();
        List<FteConnectionEntity> GetFTEConnections();
        int InsertUpdatePatientType(PatientTypeEntity entity);
        List<PatientTypeCustomEntity> GetPatientTypeData();
        PatientTypeCustomEntity GetPatientTypeByID(int patientTypeId);
        List<PatientTypeCustomEntity> GetPatientTypeDrop(int patientId);
        int UpdatePatientTypeStatus(List<PatientTypeCustomEntity> data);
        int InsertUpdateUserRecentFacNs(RecentFacEntity obj);
        RecentFacEntity GetUserRecentFacNs(int userId);
        List<OrderFavouriteCustomEntity> GetOderFavInfo();
        int InsertUpdateMeasurementsandUserInputs(PrcInsertOrderFavFacilityConfigEntity obj);
        List<PrcGetOrderFavConfigInfoEntity> GetOrderFavDetails(int FacilityId);
        PrcGetOrderFavConfigByCodeEntity GetOrderFavConfigDetailsById(CustomOrderFavInfoDataEntity entity);
        //Document Check
        int InsertUpdateDocumentCheck(List<DrugAdministerEntity> data);
        int RemoveDocumentCheck(List<DrugAdministerEntity> data);
        List<GetDocumentCheckDataEntity> GetOrdersListData(OrdersListFilter filter);
        List<GetDocumentCheckDataEntity> GetDrugsListData(int residentId, string startdate, string enddate);
        List<DosesDetailsEntity> GetPendingDosesList(int userId,string nsIds,int? facId);
        int InsertIgnoreDosesDetails(long drugAdministerId);
        List<PrcGetDocAdminOrderAudit_ResultEntity> GetDocAdminOrderAudit(Int64 DrugAdminsterID);
        List<OutboundErrorDetailsEntity> GetOutboundErrorDetails(int userId);
        int InsertIgnoreOutboundErrorDetails (int  fileId);
        int GetUserPassedDueFlag(int userId);
        int InsertUpdateRefillMailConfig(RefillMailConfigCustomEntity entity);
        List<RefillMailConfigGridData> GetRefillConfigGridData(int userId);
        int insertUserRecentCompany(int userId, int companyId);
        string GetTimeZoneDate(int nursingStationId);
        DateTime GetNursingStationTimeZoneDate();
        int inserUpdateMailconfigDetails(MailConfigEntity entity);
        int NoAdminitration(DrugAdministerEntity drugAdministerEntity);
        DateTime GetNursingStationTimeZoneDateOrderByPatient(int patientId);
        List<StateEntity> GetState();
        List<PrimarySpeciality> GetPrimarySpeciality();
        List<CredentialMaster> GetCredentialMaster();
    }
}
