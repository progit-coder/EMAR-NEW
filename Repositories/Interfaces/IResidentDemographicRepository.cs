using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.Repositories
{
    public interface IResidentDemographicRepository
    {
        //List<ResidentGridEntity> GetResidentGridData(string searchPattern, int userId);
        ResidentDataEntity GetFacilityNSResidentsDataByPId(int patientId);
        DemographicInfoEntity GetResidentInformation(int patientID);
        List<ResidentDropEntity> GetResidentsByNSId(int nurseStationId);
        List<ResidentDropEntity> GetResidentsByNSIds(string nurseStationIds);
        List<ResidentAdmitDischargeEntity> GetResidentAdmitDischargeData(int patientID);
        List<LiteralOrdersEntity> GetLiteralOrdersData(int patientID);
        List<ResidentLiteralOrderDataEntity> GetResidentOrderData(int patientId);
        List<TreatmentInfoEntity> GetMedications(int patientID);
        ResidentGridEntity GetResidentsByCompanyBed(CompanyBedConfigCustomEntity configs);
        int UploadResidentImage(string body, int PatientID);
        List<DemographicEntity> GetOutBoundDemographics();
        List<DemographicEntity> GetApprovalPendingDemographics(int patientId);
        List<DemographicEntity> GetApprovalPendingDemographics();
        int InsertUpdateDemographics(DemographicInfoEntity entity);
        string GetResidentName(int patientId);
        int ApproveDemographic(ApprovalPendingCustomEntity entity);
        DemographicEntity GetResidentDemographicData(int patientId);
        VisitInfoEntity GetResidentAdmitvisitInfoData(int visitId);
        CommonOrderInfoEntity GetResidentLiteralordersData(int OrderId);
        int InsertResidentMedications(TreatmentInfoEntity medications);
        int InsertResidentAdmitVistiInfoData(VisitInfoEntity admitvisitinfo);
        int InsertResidentDemographicData(DemographicEntity demographics);
        //int InsertResidentLiteralOrdersData(CommonOrderInfoEntity literalorders);
        ResidentCountCustomEntity GetResidentsCount(int userId);
        List<VisitInfoEntity> GetApprovalPendingTransfer(int patientId);
        List<VisitInfoEntity> GetApprovalPendingTransfer();
        List<VisitInfoEntity> GetApprovalPendingDischarge(int patientId);
        List<VisitInfoEntity> GetApprovalPendingDischarge();
        int ApproveTransfer(ApprovalPendingCustomEntity entity);
        int ApproveDischarge(ApprovalPendingCustomEntity entity);
        List<VisitInfoEntity> GetOutBoundTransfers();
        List<VisitInfoEntity> GetOutBoundDischarges();
        List<DocFolderEntity> GetResidentDocFolder(int patientId);
        TreatmentInfoEntity GetResidentMedications(int TreatmentId);
        List<ResidentDropEntity> GetResidentDropData(int userId);
        int UpdateResidentInfo(VisitUpdateEntity residentinfo);
        string GetNurseStationName(int patientId);
        OrdersViewEntity GetResidentOrdersView(int orderType, Int64 orderId, Int64 quantityId);
        int InsertPatientType(ColourTypeEntity entity);
        List<PatientColorTypeEntity> GetPatientType(int patientId);
        int GetNurseStationByPId(int patientId);
        List<PatientTypeEntity> GetAllPatientTypesByPId(int patientId);
        int InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails);
        List<PhysicianGridEntity> GetPhyscianDetails(int userId);
        PhysicianDetailsEntity GetPhyscianDetailsById(string PhyscianNPI, string FacilityId);
        string CheckResidentUniqueId(int patientId);
        int InsertUpdatePatientOnLeave(OnLeaveEntity entity);
        int UpdatePhysiciansStatus(List<PhysicianDetailsEntity> data);
        List<ResidentDropEntity> GetResidentDetails(Nullable<int> status, string nursestationId, int residentcount);
        int CheckResidentInternalId(string InternalId);
        int InsertMergeStatus(PostMergeDetails objPost);
        ResidentMergeEntity GetMergeDetails(int PatientID, int MergePatientID);
        ResidentMergeDetails GetResidentMergeDetailsByPatientID(int patientId);
        int GetHl7PendingCount();
        IList GetComputerNameDropData();
        string GetResidentActiveStatus(string mrNumber);
        string GetUserProcessKeyByID(int userId);
        List<ResidentsEntity> GetCertifyOrderResidentGridData(string nursingStations, int userId, string phyNpi);
        IList GetCertifyOrderGridData(int patientId,string phyNpi);
        List<ResidentDropEntity> GetResidentsListByNSId(int nurseStationId);
        List<ResidentsEntity> GetProfileCertifyOrderResidentGridData(string nursingStations);
        IList GetProfileCertifyOrderGridData(int patientId);
        string GetDefaultPhysicianNursestation(string PhysicianNPI);
        int UpdatePregnecyFeeding(UpdatePregnecyFeedingEntity Role);
        List<PhysicianGridEntityNew> GetPhyscianDetailsGridDataNew(int userId);

    }
}
