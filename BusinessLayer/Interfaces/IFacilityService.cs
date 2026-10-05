using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.ServiceLayer
{
    public interface IFacilityService
    {
        //Task<List<SectionEntity>> GetSection();
        //int InsertFacADTDefaults(FacADTDefaultEntity ADTDefaults);
        //int InsertFacCarePlan(FacCarePlanEntity CarePlan);
        //int InsertEHRInfo(FacEhrInfoEntity EHRInfo);
        //int InsertFacOther(FacOtherEntity FacOther);
        //int InsertFacilityPO(FacilityPOEntity PhysicianOrders);
        //int InsertFacilityICD(FacilityICDEntity Icd);
        //int InsertUpdateAssessment(FacAssessments FacAss);
        Task<List<FloorEntity>> GetFloorsForNurseStations(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        Task<List<NursingStationEntity>> GetNurseStationsForFacility(int companyId, int facilityId);
        Task<List<WingEntity>> GetWingsForFloor(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        Task<List<RoomEntity>> GetRoomsForWing(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        Task<List<BedEntity>> GetBedsForRoom(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);

        #region Facility
        Task<int> InsertUpdateFacilityMaster(FacilityEntity facility);
        Task<List<FacilityCustomEntity>> GetActiveFacilityMasterList();
        Task<List<FacilityCustomEntity>> GetAllFacilityMasterList();
        Task<FacilityCustomEntity> GetFacilityDetailsById(int facilityId);
        Task<List<FacilitiesbyCompanyIdEntity>> GetFacilitesByCompanyId(int companyId);
        Task<List<FacilityEntity>> GetFacilityDropData();
        Task<List<NurseStationDropEntity>> GetNurseStationDropData();
        Task<List<FacilityDropEntity>> UserFacilityDrop(int userId);

        #endregion
        #region Floor
        Task<int> InsertUpdateFloorMaster(FloorEntity entity);
        Task<List<FloorEntity>> GetAllFloorsList();
        Task<FloorEntity> GetFloorById(int floorId);
        #endregion
        #region NurseStation
        Task<int> InsertUpdateNurseStation(NursingStationEntity entity);
        Task<List<NursingStationEntity>> GetAllNurseStationsList(int? facilityId = null);
        Task<List<NursingStationEntity>> GetAllNurseStationsListNew(int facilityid);
        Task<NursingStationEntity> GetNurseStationById(int nurseStationId);
        Task<List<NurseStationDropEntity>> UserNurseStationDrop(int userId);
        Task<List<NurseStationDropEntity>> UserNurseStationDrop(int userId, int facilityId);
        Task<List<NurseStationDropEntity>> BedConfigNurseStationDrop();
        Task<List<NurseStationDropEntity>> GetAllActiveNurseStationDropData(string facilityIds);
        Task<int>InsertUpdateNurseShift(List<NurseShiftEntity> entity);
        #endregion
        #region Wing
        Task<List<WingEntity>> GetAllWingsList();
        Task<int> InsertUpdateWing(WingEntity entity);
        Task<WingEntity> GetWingById(int wingId);

        #endregion
        #region Room
        Task<List<RoomEntity>> GetAllRoomsList();
        Task<int> InsertUpdateRoom(RoomEntity entity);
        Task<RoomEntity> GetRoomById(int roomId);

        #endregion
        #region Bed
        Task<List<BedEntity>> GetAllBedsList();
        Task<int> InsertUpdateBed(BedEntity entity);
        Task<BedEntity> GetBedById(int bedId);

        #endregion

        Task<CompanyBedCustomEntity> GetCompanyToBedData(int userId);
        //Task<CompanyBedConfigCustomEntity> GetCompanyBedConfigs(CompanyBedConfigCustomEntity filterConfigs);
        Task<int> InsertUpdateCompanyBedConfigs(CompanyBedConfigEntity BedConfig);
        Task<CompanyBedConfigEntity> GetCompanyBedConfigByBedConfigId(int bedConfigId);
        Task<List<CompanyBedConfigEntity>> GetCompanyBedGridData(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        Task<int> GetCompanyToBedFlag(int userId);
        Task<int> GetCompanyTimeFormat(int facilityId);
        Task<List<NurseShiftEntity>> GetNurseShifts(int nurseStationId,int facilityId);
        Task<int> UpdateNursestationsStatus(List<NursingStationEntity> data);
        Task<int> UpdateFacilityStatus(List<FacilityCustomEntity> data);
        Task<int> UpdateFloorStatus(List<FloorEntity> data);
        Task<int> UpdateWingStatus(List<WingEntity> data);
        Task<int> UpdateCompanyBedConfigsStatus(List<CompanyBedConfigEntity> data);
        Task<int> UpdateRoomStatus(List<RoomEntity> data);
        Task<int> UpdateBedStatus(List<BedEntity> data);
        Task<List<FloorDropEntity>> GetAllActiveFloorNames();
        Task<List<WingDropEntity>> GetAllActiveWingNames();
        Task<List<BedDropEntity>> GetAllActiveBedNames();
        Task<List<RoomDropEntity>> GetAllActiveRoomNames();
        Task<CompanyToBedEntity> GetCompanyToBedFlagByFacId(int userId, int facilityId, string nurseStations);
        Task<UserAccesssFacilityNurseStationsEntity> GetUserAccessFacilityNurseStaions(int userId);
        Task<int> InsertUpateComputerName(ProcessKeyEntity entity);
        Task<List<ProcessMasterEntity>> GetProcessKeyMasterList(int? nsId = null);
        Task<List<NurseStationDropEntity>> UserBiometricNurseStationDrop(int userId);
        Task<Tuple<int, string>> GetDashboardMethodCount(int type, int nurseStationId);
        Task<List<NurseStationDropEntity>> UsersConfigNurseStationDrop(string username, string password);
        Task<int> RemoveComputerName(int processId);
        Task<List<CensusEntity>> GetCensusByNursingStatios(int stationId);
        Task<List<ResidentsEntity>> GetCensusResidentsList(CompanyBedConfigCustomEntity entity);
        Task<List<tblTimeZoneEntity>> GetTimeZones();
        Task<List<gpiEntity>> GpiList(string Nursing, string fromdate, string todate,string Type);

        Task<List<NurseStationDropEntity>> UserNurseStationDropMultiple(int userId, string facilityId);

    }
}
