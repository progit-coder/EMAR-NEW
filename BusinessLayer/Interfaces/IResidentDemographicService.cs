using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public interface IResidentDemographicService
    {
        //Task<List<ResidentGridEntity>> GetResidentGridData(string searchPattern, int userId);
        Task<List<ResidentAdmitDischargeEntity>> GetResidentAdmitDischargeData(int patientID);
        Task<List<LiteralOrdersEntity>> GetLiteralOrdersData(int patientID);
        Task<List<ResidentLiteralOrderDataEntity>> GetResidentOrderData(int patientId);
        Task<ResidentGridEntity> GetResidentsByCompanyBed(CompanyBedConfigCustomEntity configs);
        Task<int> UploadResidentImage(int PatientID);
        Task<List<TreatmentInfoEntity>> GetMedications(int patientID);
        Task<DemographicEntity> GetResidentDemographicData(int patientId);
        Task<VisitInfoEntity> GetResidentAdmitvisitInfoData(int visitId);
        Task<CommonOrderInfoEntity> GetResidentLiteralordersData(int OrderId);
        Task<int> InsertResidentMedications(TreatmentInfoEntity medications);
        Task<int> InsertResidentAdmitVistiInfoData(VisitInfoEntity admitvisitinfo);
        Task<int> InsertResidentDemographicData(DemographicEntity demographics);
        //Task<int> InsertResidentLiteralOrdersData(CommonOrderInfoEntity literalorders);
        Task<ResidentCountCustomEntity> GetResidentsCount(int userId);
        Task<List<DocFolderEntity>> GetResidentDocFolder(int patientId);
        Task<TreatmentInfoEntity> GetResidentMedications(int TreatmentId);
        Task<DemographicInfoEntity> GetResidentInformation(int patientID);
        Task<ResidentDataEntity> GetFacilityNSResidentsDataByPId(int patientId);
        Task<List<ResidentDropEntity>> GetResidentsByNSId(int nurseStationId);
        Task<List<ResidentDropEntity>> GetResidentsByNSIds(string nurseStationIds);
        Task<List<ResidentDropEntity>> GetResidentDropData(int userId);
        Task<int> UpdateResidentInfo(VisitUpdateEntity residentinfo);
        Task<string> GetNurseStationName(int patientId);
        Task<OrdersViewEntity> GetResidentOrdersView(int orderType, Int64 orderId, Int64 quantityId);
        Task<int> InsertPatientType(ColourTypeEntity entity);
        Task<List<PatientColorTypeEntity>> GetPatientType(int patientId);
        Task<int> GetNurseStationByPId(int patientId);
        Task<List<PatientTypeEntity>> GetAllPatientTypesByPId(int patientId);
        Task<int> InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails);
        Task<List<PhysicianGridEntity>> GetPhyscianDetails(int userId);
        Task<PhysicianDetailsEntity> GetPhyscianDetailsById(string PhyscianNPI, string FacilityId);
        Task<string> CheckResidentUniqueId(int patientId);
        Task<int> InsertUpdatePatientOnLeave(OnLeaveEntity entity);
        Task<int> UpdatePhysiciansStatus(List<PhysicianDetailsEntity> data);
        Task<List<ResidentDropEntity>> GetResidentDetails(Nullable<int> status,string nursestationId, int residentcount);
        Task<int> CheckResidentInternalId(string InternalId);
        Task<int> InsertMergeStatus(PostMergeDetails objPost);
        Task<ResidentMergeEntity> GetMergeDetails(int PatientID, int MergePatientID);
        Task<ResidentMergeDetails> GetResidentMergeDetailsByPatientID(int patientId);
        Task<int> GetHl7PendingCount();
        Task<IList> GetComputerNameDropData();
        Task<string> GetResidentActiveStatus(string mrNumber);
        Task<string> GetUserProcessKeyByID(int userId);
        Task<List<ResidentsEntity>> GetCertifyOrderResidentGridData(string nursingStations, int userId, string phyNpi);
        Task<IList> GetCertifyOrderGridData(int patientId, string phyNpi);
        Task<List<ResidentDropEntity>> GetResidentsListByNSId(int nurseStationId);
        Task<List<ResidentsEntity>> GetProfileCertifyOrderResidentGridData(string nursingStations);
        Task<IList> GetProfileCertifyOrderGridData(int patientId);
        Task<string> GetDefaultPhysicianNursestation(string PhysicianNPI);
        Task<int> UpdatePregnecyFeeding(UpdatePregnecyFeedingEntity Role);

        Task<List<PhysicianGridEntityNew>> GetPhyscianDetailsGridDataNew(int userId);

    }
}
