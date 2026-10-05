using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using LTCPro.DAL;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public class CommonService : ICommonService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger _log;

        public CommonService(IAutoMapper autoMapper, ICommonRepository commonRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._commonRepository = commonRepository;
            this._log = log;
        }
        public async Task<List<CountryEntity>> GetAllCountries()
        {
            this._log.Debug("---Executing GetAllCountries() in CommonService----");
            return await Task.FromResult<List<CountryEntity>>(this._commonRepository.GetCountries());
        }
        public async Task<List<OrderFavouriteCustomEntity>> GetOderFavInfo()
        {
            this._log.Debug("---Executing GetOderFavInfo() in CommonService----");
            return await Task.FromResult<List<OrderFavouriteCustomEntity>>(this._commonRepository.GetOderFavInfo());
        }

        public async Task<List<GenderEntity>> GetGenders()
        {
            this._log.Debug("---Executing GetGenders() in CommonService----");
            return await Task.FromResult<List<GenderEntity>>(this._commonRepository.GetGenders());
        }
        public async Task<List<SuffixEntity>> GetSuffixes()
        {
            this._log.Debug("---Executing GetSuffixes() in CommonService----");
            return await Task.FromResult<List<SuffixEntity>>(this._commonRepository.GetSuffix());
        }

        public async Task<List<FTECategoryEntity>> GetFTECategories()
        {
            this._log.Debug("---Executing GetFTECategories() in CommonService----");
            return await Task.FromResult<List<FTECategoryEntity>>(this._commonRepository.GetFTECategories());
        }

        public async Task<List<FteConnectionEntity>> GetFTEConnections()
        {
            this._log.Debug("---Executing GetFTEConnections() in CommonService----");
            return await Task.FromResult<List<FteConnectionEntity>>(this._commonRepository.GetFTEConnections());
        }
        public async Task<List<PrcGetOrderFavConfigInfoEntity>> GetOrderFavDetails(int FacilityId)
        {
            this._log.Debug("---Executing GetOrderFavDetails() in CommonService----");
            return await Task.FromResult<List<PrcGetOrderFavConfigInfoEntity>>(this._commonRepository.GetOrderFavDetails(FacilityId));
        }
        public async Task<PrcGetOrderFavConfigByCodeEntity> GetOrderFavConfigDetailsById(CustomOrderFavInfoDataEntity entity)
        {
            this._log.Debug("---Executing GetOrderFavConfigDetailsById() in CommonService----");
            return await Task.FromResult<PrcGetOrderFavConfigByCodeEntity>(this._commonRepository.GetOrderFavConfigDetailsById(entity));
        }

        public async Task<int> InsertUpdatePatientType(PatientTypeEntity entity)
        {
            this._log.Debug("---Executing InsertUpdatePatientType() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.InsertUpdatePatientType(entity));
        }

        public async Task<int> InsertUpdateMeasurementsandUserInputs(PrcInsertOrderFavFacilityConfigEntity obj)
        {
            this._log.Debug("---Executing InsertUpdateMeasurementsandUserInputs() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.InsertUpdateMeasurementsandUserInputs(obj));
        }

        public async Task<List<PatientTypeCustomEntity>> GetPatientTypeData()
        {
            this._log.Debug("---Executing GetPatientTypeData() in CommonService----");
            return await Task.FromResult<List<PatientTypeCustomEntity>>(this._commonRepository.GetPatientTypeData());
        }
        public async Task<PatientTypeCustomEntity> GetPatientTypeByID(int patientTypeId)
        {
            this._log.Debug("---Executing GetPatientTypeByID() in CommonService----");
            return await Task.FromResult<PatientTypeCustomEntity>(this._commonRepository.GetPatientTypeByID(patientTypeId));
        }
        public async Task<List<PatientTypeCustomEntity>> GetPatientTypeDrop(int patientId)
        {
            this._log.Debug("---Executing GetPatientTypeDrop() in CommonService----");
            return await Task.FromResult<List<PatientTypeCustomEntity>>(this._commonRepository.GetPatientTypeDrop(patientId));
        }
        public async Task<int> UpdatePatientTypeStatus(List<PatientTypeCustomEntity> data)
        {
            this._log.Debug("---Executing UpdatePatientTypeStatus() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.UpdatePatientTypeStatus(data));
        }
        public async Task<RecentFacEntity> GetUserRecentFacNs(int userId)
        {
            this._log.Debug("---Executing GetUserRecentFacNs() in UserService----");
            return await Task.FromResult<RecentFacEntity>(this._commonRepository.GetUserRecentFacNs(userId));
        }

        // Document Check
        public async Task<int> RemoveDocumentCheck(List<DrugAdministerEntity> entity)
        {
            this._log.Debug("---Executing RemoveDocumentCheck() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.RemoveDocumentCheck(entity));
        }
        public async Task<int> InsertUpdateDocumentCheck(List<DrugAdministerEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdateDocumentCheck() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.InsertUpdateDocumentCheck(entity));
        }

        public async Task<List<GetDocumentCheckDataEntity>> GetOrdersListData(OrdersListFilter filter)
        {
            this._log.Debug("---Executing GetOrdersListData() in CommonService----");
            return await Task.FromResult<List<GetDocumentCheckDataEntity>>(this._commonRepository.GetOrdersListData(filter));
        }
        public async Task<List<GetDocumentCheckDataEntity>> GetDrugsListData(int residentId, string startdate, string enddate)
        {
            this._log.Debug("---Executing GetDrugsListData() in CommonService----");
            return await Task.FromResult<List<GetDocumentCheckDataEntity>>(this._commonRepository.GetDrugsListData(residentId, startdate, enddate));
        }
        public async Task<List<DosesDetailsEntity>> GetPendingDosesList(int userId,string nsIds,int? facId)
        {
            this._log.Debug("---Executing GetPendingDosesList() in CommonService----");
            return await Task.FromResult<List<DosesDetailsEntity>>(this._commonRepository.GetPendingDosesList(userId, nsIds,facId));
        }
        public async Task<int> InsertIgnoreDosesDetails(long drugAdministerId)
        {
            this._log.Debug("---Executing InsertIgnoreDosesDetails() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.InsertIgnoreDosesDetails(drugAdministerId));
        }
        public async Task<List<PrcGetDocAdminOrderAudit_ResultEntity>> GetDocAdminOrderAudit(Int64 DrugAdminsterID)
        {
            this._log.Debug("---Executing GetDocAdminOrderAudit() in CommonService----");
            return await Task.FromResult<List<PrcGetDocAdminOrderAudit_ResultEntity>>(this._commonRepository.GetDocAdminOrderAudit(DrugAdminsterID));
        }
        public async Task<List<OutboundErrorDetailsEntity>> GetOutboundErrorDetails(int userId)
        {
            this._log.Debug("---Executing GetOutboundErrorDetails() in CommonService----");
            return await Task.FromResult<List<OutboundErrorDetailsEntity>>(this._commonRepository.GetOutboundErrorDetails(userId));
        }
        public async Task<int> InsertIgnoreOutboundErrorDetails(int fileId)
        {
            this._log.Debug("---Executing InsertIgnoreOutboundErrorDetails() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.InsertIgnoreOutboundErrorDetails(fileId));
        }
        public async Task<int> GetUserPassedDueFlag(int userId)
        {
            this._log.Debug("---Executing GetUserPassedDueFlag() in UserService----");
            return await Task.FromResult<int>(this._commonRepository.GetUserPassedDueFlag(userId));
        }
        public async Task<int> InsertUpdateRefillMailConfig(RefillMailConfigCustomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateRefillMailConfig() in UserService----");
            return await Task.FromResult<int>(this._commonRepository.InsertUpdateRefillMailConfig(entity));
        }
        public async Task<List<RefillMailConfigGridData>> GetRefillConfigGridData(int userId)
        {
            this._log.Debug("---Executing GetRefillConfigGridData() in CommonService----");
            return await Task.FromResult<List<RefillMailConfigGridData>>(this._commonRepository.GetRefillConfigGridData(userId));
        }
        public async Task<string> GetTimeZoneDate(int nursingStationId)
        {
            this._log.Debug("---Executing GetTimeZoneDate() in CommonService----");
            return await Task.FromResult<string>(this._commonRepository.GetTimeZoneDate(nursingStationId));
        }
        public async Task<int> inserUpdateMailconfigDetails(MailConfigEntity entity)
        {
            this._log.Debug("---Executing inserUpdateMailconfigDetails() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.inserUpdateMailconfigDetails(entity));
        }

        public async Task<int> InsertUpdateUserRecentFacNs(RecentFacEntity obj)
        {
            this._log.Debug("---Executing InsertUpdateUserRecentFacNs() in CommonService----");
            return await Task.FromResult<int>(this._commonRepository.InsertUpdateUserRecentFacNs(obj));
        }

        public async Task<int> NoAdminitration(DrugAdministerEntity drugAdministerEntity)
        {
            this._log.Debug("---Executing NoAdminitration() in CommonService----DrugAdminsterID "+ drugAdministerEntity.DrugAdminister_Id);
            return await Task.FromResult<int>(this._commonRepository.NoAdminitration(drugAdministerEntity));
        }

        public async Task<List<StateEntity>> GetState()
        {
            this._log.Debug("---Executing GetState() in CommonService----");
            return await Task.FromResult<List<StateEntity>>(this._commonRepository.GetState());
        }
        public async Task<List<PrimarySpeciality>> GetPrimarySpeciality()
        {
            this._log.Debug("---Executing GetPrimarySpeciality() in CommonService----");
            return await Task.FromResult<List<PrimarySpeciality>>(this._commonRepository.GetPrimarySpeciality());
        }

        public async Task<List<CredentialMaster>> GetCredentialMaster()
        {
            this._log.Debug("---Executing GetCredentialMaster() in CommonService----");
            return await Task.FromResult<List<CredentialMaster>>(this._commonRepository.GetCredentialMaster());
        }

    }
}
