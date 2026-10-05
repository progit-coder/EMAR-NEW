using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.Repositories
{
    public interface IFacilityRepository
    {
        //List<SectionEntity> GetSection();
        //int InsertFacADTDefaults(FacADTDefaultEntity ADTDefaults);
        //int InsertFacCarePlan(FacCarePlanEntity CarePlan);
        //int InsertEHRInfo(FacEhrInfoEntity EHRInfo);
        //int InsertFacOther(FacOtherEntity FacOther);
        //int InsertFacilityPO(FacilityPOEntity PhysicianOrders);
        //int InsertFacilityICD(FacilityICDEntity Icd);
        //int InsertUpdateAssessment(FacAssessments FacAss);
        List<FloorEntity> GetFloorsForNurseStations(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        //ToDo:Delete if not used
        //List<NursingStationEntity> GetNurseStationsForFloor(int facilityId, int companyId, int floorId);
        List<WingEntity> GetWingsForFloor(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        List<RoomEntity> GetRoomsForWing(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        List<BedEntity> GetBedsForRoom(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId,int bedId);
        #region Facility
        int InsertUpdateFacilityMaster(FacilityEntity facility);
        List<FacilityCustomEntity> GetActiveFacilityMasterList();
        List<FacilityCustomEntity> GetAllFacilityMasterList();
        FacilityCustomEntity GetFacilityDetailsById(int facilityId);
        List<FacilitiesbyCompanyIdEntity> GetFacilitesByCompanyId(int companyId);
        #endregion
        #region Floor
        int InsertUpdateFloorMaster(FloorEntity entity);
        List<FloorEntity> GetAllFloorsList();
        FloorEntity GetFloorById(int floorId);
        #endregion
        #region NurseStation
        int InsertUpdateNurseStation(NursingStationEntity entity);
        List<NursingStationEntity> GetAllNurseStationsList(int? facilityId = null);
        List<NursingStationEntity> GetAllNurseStationsListNew(int facilityid);
        NursingStationEntity GetNurseStationById(int nurseStationId);
        int InsertUpdateNurseShift(List<NurseShiftEntity> entity);

        #endregion
        #region Wing
        List<WingEntity> GetAllWingsList();
        int InsertUpdateWing(WingEntity entity);
        WingEntity GetWingById(int wingId);

        #endregion
        #region Room
        List<RoomEntity> GetAllRoomsList();
        int InsertUpdateRoom(RoomEntity entity);
        RoomEntity GetRoomById(int roomId);

        #endregion
        #region Bed
        List<BedEntity> GetAllBedsList();
        int InsertUpdateBed(BedEntity entity);
        BedEntity GetBedById(int bedId);

        #endregion
        List<NursingStationEntity> GetNurseStationsForFacility(int companyId, int facilityId);
       // List<WingEntity> GetWingsForFloor(int companyId, int facilityId,int stationId);
        List<RoomEntity> GetRoomsForFacility(int companyId, int facilityId);
        List<BedEntity> GetBedsForFacility(int companyId, int facilityId);
        //CompanyBedConfigCustomEntity GetCompanyBedConfigs(CompanyBedConfigCustomEntity filterConfigs);
        int InsertUpdateCompanyBedConfigs(CompanyBedConfigEntity BedConfig);
        CompanyBedConfigEntity GetCompanyBedConfigByBedConfigId(int bedConfigId);
        List<FloorEntity> GetFloorsForFacility(int companyId, int facilityId);
        List<WingEntity> GetWingsForFacility(int companyId, int facilityId);
        List<CompanyBedConfigEntity> GetCompanyBedGridData(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId);
        List<FacilityEntity> GetFacilityDropData();
        List<NurseStationDropEntity> GetNurseStationDropData();
        List<NurseStationDropEntity> GetAllActiveNurseStationDropData(string facilityIds);
        List<NurseStationDropEntity> UserNurseStationDrop(int userId);
        List<NurseStationDropEntity> UserNurseStationDrop(int userId, int facilityId);
        List<NurseStationDropEntity> BedConfigNurseStationDrop();
        List<FloorDropEntity> UserFloorDrop(int userId);
        List<WingDropEntity> UserWingDrop(int userId);
        List<RoomDropEntity> UserRoomDrop(int userId);
        List<BedDropEntity> UserBedDrop(int userId);
        List<FacilityDropEntity> UserFacilityDrop(int userId);
        int GetCompanyToBedFlag(int userId);
        int GetCompanyTimeFormat(int facilityId);
        List<NurseShiftEntity> GetNurseShifts(int nurseStationId,int facilityId);
        int UpdateNursestationsStatus(List<NursingStationEntity> data);
        int UpdateFacilityStatus(List<FacilityCustomEntity> data);
        int UpdateFloorStatus(List<FloorEntity> data);
        int UpdateWingStatus(List<WingEntity> data);
        int UpdateCompanyBedConfigsStatus(List<CompanyBedConfigEntity> data);
        int UpdateRoomStatus(List<RoomEntity> data);
        int UpdateBedStatus(List<BedEntity> data);
        List<FloorDropEntity> GetAllActiveFloorNames();
        List<WingDropEntity> GetAllActiveWingNames();
        List<RoomDropEntity> GetAllActiveRoomNames();
        List<BedDropEntity> GetAllActiveBedNames();
        CompanyToBedEntity GetCompanyToBedFlagByFacId(int userId,int facilityId, string nurseStations);
        int InsertUpateComputerName(ProcessKeyEntity entity);
        List<ProcessMasterEntity> GetProcessKeyMasterList(int? nsId = null);
        List<NurseStationDropEntity> UserBiometricNurseStationDrop(int userId);
        Tuple<int, string> GetDashboardMethodCount(int type, int nurseStationId);
        List<NurseStationDropEntity> UsersConfigNurseStationDrop(string username, string password);
        int RemoveComputerName(int processId);
        List<CensusEntity> GetCensusByNursingStatios(int stationId);
        List<ResidentsEntity> GetCensusResidentsList(CompanyBedConfigCustomEntity entity);
        List<tblTimeZoneEntity> GetTimeZones();
        List<gpiEntity> GpiList(string Nursing, string fromdate, string todate,string Type);

        List<NurseStationDropEntity> UserNurseStationDropMultiple(int userId, string facilityId);

    }
}
