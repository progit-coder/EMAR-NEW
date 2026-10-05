using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class FacilityService : IFacilityService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ILogger _log;

        public FacilityService(IAutoMapper autoMapper, IFacilityRepository facilityRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._facilityRepository = facilityRepository;
            this._log = log;
        }
        /*
        public async Task<List<SectionEntity>> GetSection()
        {
            this._log.Debug("---Executing GetSection() in FacilityService----");
            return await Task.FromResult<List<SectionEntity>>(this._facilityRepository.GetSection());
        }
        public int InsertFacADTDefaults(FacADTDefaultEntity ADTDefaults)
        {
            this._log.Debug("---Executing InsertFacADTDefaults() in FacilityService----");
            return this._facilityRepository.InsertFacADTDefaults(ADTDefaults);
        }
        public int InsertFacCarePlan(FacCarePlanEntity CarePlan)
        {
            this._log.Debug("---Executing InsertFacCarePlan() in FacilityService----");
            return this._facilityRepository.InsertFacCarePlan(CarePlan);

        }
        public int InsertEHRInfo(FacEhrInfoEntity EHRInfo)
        {
            this._log.Debug("---Executing InsertEHRInfo() in FacilityService----");
            return this._facilityRepository.InsertEHRInfo(EHRInfo);
        }
        public int InsertFacOther(FacOtherEntity FacOther)
        {
            this._log.Debug("---Executing InsertFacOther() in FacilityService----");
            return this._facilityRepository.InsertFacOther(FacOther);
        }
        public int InsertFacilityPO(FacilityPOEntity PhysicianOrders)
        {
         this._log.Debug("---Executing InsertFacilityPO() in FacilityService----");
            return this._facilityRepository.InsertFacilityPO(PhysicianOrders);
        }
       public int InsertFacilityICD(FacilityICDEntity Icd)
        {
            this._log.Debug("---Executing InsertFacilityICD() in FacilityService----");
            return this._facilityRepository.InsertFacilityICD(Icd);
        }
        public int InsertUpdateAssessment(FacAssessments Facass)
        {
            this._log.Debug("---Executing InsertUpdateAssessment() in FacilityService----");
            return this._facilityRepository.InsertUpdateAssessment(Facass);
        }*/

        public async Task<int> InsertUpdateFacilityMaster(FacilityEntity facility)
        {
            this._log.Debug("---Executing InsertUpdateFacilityMaster() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateFacilityMaster(facility));
        }

        public async Task<List<FacilityCustomEntity>> GetActiveFacilityMasterList()
        {
            this._log.Debug("---Executing GetActiveFacilityMasterList() in FacilityService----");
            return await Task.FromResult<List<FacilityCustomEntity>>(this._facilityRepository.GetActiveFacilityMasterList());
        }

        public async Task<List<FacilityCustomEntity>> GetAllFacilityMasterList()
        {
            this._log.Debug("---Executing GetAllFacilityMasterList() in FacilityService----");
            return await Task.FromResult<List<FacilityCustomEntity>>(this._facilityRepository.GetAllFacilityMasterList());
        }

        public async Task<FacilityCustomEntity> GetFacilityDetailsById(int facilityId)
        {
            this._log.Debug("---Executing GetFacilityDetailsById() in FacilityService----");
            return await Task.FromResult<FacilityCustomEntity>(this._facilityRepository.GetFacilityDetailsById(facilityId));
        }

        public async Task<int> InsertUpdateFloorMaster(FloorEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateFloorMaster() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateFloorMaster(entity));
        }

        public async Task<List<FloorEntity>> GetAllFloorsList()
        {
            this._log.Debug("---Executing GetAllFloorsList() in FacilityService----");
            return await Task.FromResult<List<FloorEntity>>(this._facilityRepository.GetAllFloorsList());
        }

        public async Task<int> InsertUpdateNurseStation(NursingStationEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateNurseStation() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateNurseStation(entity));
        }

        public async Task<List<NursingStationEntity>> GetAllNurseStationsList(int? facilityId = null)
        {
            this._log.Debug("---Executing GetAllNurseStationsList() in FacilityService----");
            return await Task.FromResult<List<NursingStationEntity>>(this._facilityRepository.GetAllNurseStationsList(facilityId));
        }
        public async Task<List<NursingStationEntity>> GetAllNurseStationsListNew(int facilityid)
        {
            this._log.Debug("---Executing GetAllNurseStationsListNew(facilityid) in FacilityService----");
            return await Task.FromResult<List<NursingStationEntity>>(this._facilityRepository.GetAllNurseStationsListNew(facilityid));
        }

        public async Task<List<WingEntity>> GetAllWingsList()
        {
            this._log.Debug("---Executing GetAllWingsList() in FacilityService----");
            return await Task.FromResult<List<WingEntity>>(this._facilityRepository.GetAllWingsList());
        }

        public async Task<int> InsertUpdateWing(WingEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateWing() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateWing(entity));
        }

        public async Task<List<RoomEntity>> GetAllRoomsList()
        {
            this._log.Debug("---Executing GetAllRoomsList() in FacilityService----");
            return await Task.FromResult<List<RoomEntity>>(this._facilityRepository.GetAllRoomsList());
        }

        public async Task<int> InsertUpdateRoom(RoomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateRoom() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateRoom(entity));
        }

        public async Task<List<BedEntity>> GetAllBedsList()
        {
            this._log.Debug("---Executing GetAllBedsList() in FacilityService----");
            return await Task.FromResult<List<BedEntity>>(this._facilityRepository.GetAllBedsList());
        }

        public async Task<int> InsertUpdateBed(BedEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateBed() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateBed(entity));
        }
        public async Task<int> InsertUpdateCompanyBedConfigs(CompanyBedConfigEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateCompanyBedConfigs() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateCompanyBedConfigs(entity));
        }
        public async Task<CompanyBedConfigEntity> GetCompanyBedConfigByBedConfigId(int bedConfigId)
        {
            this._log.Debug("---Executing GetCompanyBedConfigByBedConfigId() in FacilityService----");
            return await Task.FromResult<CompanyBedConfigEntity>(this._facilityRepository.GetCompanyBedConfigByBedConfigId(bedConfigId));
        }
        public async Task<FloorEntity> GetFloorById(int floorId)
        {
            this._log.Debug("---Executing GetFloorById() in FacilityService----");
            return await Task.FromResult<FloorEntity>(this._facilityRepository.GetFloorById(floorId));
        }

        public async Task<NursingStationEntity> GetNurseStationById(int nurseStationId)
        {
            this._log.Debug("---Executing GetNurseStationById() in FacilityService----");
            return await Task.FromResult<NursingStationEntity>(this._facilityRepository.GetNurseStationById(nurseStationId));
        }

        public async Task<WingEntity> GetWingById(int wingId)
        {
            this._log.Debug("---Executing GetWingById() in FacilityService----");
            return await Task.FromResult<WingEntity>(this._facilityRepository.GetWingById(wingId));
        }

        public async Task<RoomEntity> GetRoomById(int roomId)
        {
            this._log.Debug("---Executing GetRoomById() in FacilityService----");
            return await Task.FromResult<RoomEntity>(this._facilityRepository.GetRoomById(roomId));
        }

        public async Task<BedEntity> GetBedById(int bedId)
        {
            this._log.Debug("---Executing GetBedById() in FacilityService----");
            return await Task.FromResult<BedEntity>(this._facilityRepository.GetBedById(bedId));
        }

        public async Task<List<FloorEntity>> GetFloorsForNurseStations(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetFloorsForNurseStations() in FacilityService----");
            return await Task.FromResult<List<FloorEntity>>(this._facilityRepository.GetFloorsForNurseStations(companyId, facilityId, stationId, floorId, wingId, roomId, bedId));
        }

        public async Task<List<NursingStationEntity>> GetNurseStationsForFacility(int companyId, int facilityId)
        {
            this._log.Debug("---Executing GetNurseStationsForFacility() in FacilityService----");
            return await Task.FromResult<List<NursingStationEntity>>(this._facilityRepository.GetNurseStationsForFacility(companyId, facilityId));
        }

        public async Task<List<WingEntity>> GetWingsForFloor(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetWingsForFloor() in FacilityService----");
            return await Task.FromResult<List<WingEntity>>(this._facilityRepository.GetWingsForFloor(companyId, facilityId, stationId, floorId, wingId, roomId, bedId));
        }

        public async Task<List<RoomEntity>> GetRoomsForWing(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetRoomsForWing() in FacilityService----");
            return await Task.FromResult<List<RoomEntity>>(this._facilityRepository.GetRoomsForWing(companyId, facilityId, stationId, floorId, wingId, roomId, bedId));
        }

        public async Task<List<BedEntity>> GetBedsForRoom(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetBedsForRoom() in FacilityService----");
            return await Task.FromResult<List<BedEntity>>(this._facilityRepository.GetBedsForRoom(companyId, facilityId, stationId, floorId, wingId, roomId,bedId));
        }

        public async Task<CompanyBedCustomEntity> GetCompanyToBedData(int userId)
        {
            this._log.Debug("---Executing GetCompanyToBedData() in FacilityService----");
            CompanyBedCustomEntity entity = new CompanyBedCustomEntity();
            entity.Facilities = this._facilityRepository.UserFacilityDrop(userId);
            entity.NurseStations = this._facilityRepository.UserNurseStationDrop(userId);
            entity.Floors = this._facilityRepository.UserFloorDrop(userId);
            entity.Wings = this._facilityRepository.UserWingDrop(userId);
            entity.Rooms = this._facilityRepository.UserRoomDrop(userId);
            entity.Beds = this._facilityRepository.UserBedDrop(userId);

            return await Task.FromResult<CompanyBedCustomEntity>(entity);

        }
        public async Task<UserAccesssFacilityNurseStationsEntity> GetUserAccessFacilityNurseStaions(int userId)
        {
            this._log.Debug("---Executing GetUserAccessFacilityNurseStaions() in FacilityService----");
            UserAccesssFacilityNurseStationsEntity entity = new UserAccesssFacilityNurseStationsEntity();
            entity.Facilities = this._facilityRepository.UserFacilityDrop(userId);
            entity.NurseStations = this._facilityRepository.UserNurseStationDrop(userId);

            return await Task.FromResult<UserAccesssFacilityNurseStationsEntity>(entity);
        }

        //public async Task<CompanyBedConfigCustomEntity> GetCompanyBedConfigs(CompanyBedConfigCustomEntity filterConfigs)
        //{
        //    this._log.Debug("---Executing GetCompanyBedConfigs() in FacilityService----");
        //    var entity = this._facilityRepository.GetCompanyBedConfigs(filterConfigs);
        //    return await Task.FromResult<CompanyBedConfigCustomEntity>(entity);
        //}
        public async Task<List<CompanyBedConfigEntity>> GetCompanyBedGridData(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetCompanyBedGridData() in FacilityService----");
            return await Task.FromResult<List<CompanyBedConfigEntity>>(this._facilityRepository.GetCompanyBedGridData(companyId, facilityId, stationId, floorId, wingId, roomId, bedId));
        }
        public async Task<List<FacilityEntity>> GetFacilityDropData()
        {
            this._log.Debug("---Executing GetFacilityDropData() in FacilityService----");
            return await Task.FromResult<List<FacilityEntity>>(this._facilityRepository.GetFacilityDropData());
        }
        public async Task<List<NurseStationDropEntity>> GetNurseStationDropData()
        {
            this._log.Debug("---Executing GetNurseStationDropData() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.GetNurseStationDropData());
        }
        public async Task<List<NurseStationDropEntity>> GetAllActiveNurseStationDropData(string facilityIds)
        {
            this._log.Debug("---Executing GetAllActiveNurseStationDropData() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.GetAllActiveNurseStationDropData(facilityIds));
        }

        public async Task<List<FacilitiesbyCompanyIdEntity>> GetFacilitesByCompanyId(int companyId)
        {
            this._log.Debug("---Executing GetFacilitesByCompanyId() in FacilityService----");
            return await Task.FromResult<List<FacilitiesbyCompanyIdEntity>>(this._facilityRepository.GetFacilitesByCompanyId(companyId));
        }

        public async Task<List<NurseStationDropEntity>> UserNurseStationDrop(int userId)
        {
            this._log.Debug("---Executing UserNurseStationDrop() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.UserNurseStationDrop(userId));
        }

        public async Task<List<NurseStationDropEntity>> UserNurseStationDropMultiple(int userId, string facilityId)
        {
            this._log.Debug("---Executing UserNurseStationDrop() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.UserNurseStationDropMultiple(userId, facilityId));
        }
        public async Task<List<NurseStationDropEntity>> UserNurseStationDrop(int userId, int facilityId)
        {
            this._log.Debug("---Executing UserNurseStationDrop() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.UserNurseStationDrop(userId, facilityId));
        }
        public async Task<List<gpiEntity>> GpiList(string Nursing, string fromdate, string todate,string Type)
        {
            this._log.Debug("---Executing UserNurseStationDrop() in FacilityService----");
            return await Task.FromResult<List<gpiEntity>>(this._facilityRepository.GpiList(Nursing, fromdate, todate, Type));
        }

       
        public async Task<List<NurseStationDropEntity>> BedConfigNurseStationDrop()
        {
            this._log.Debug("---Executing BedConfigNurseStationDrop() in FacilityService---");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.BedConfigNurseStationDrop());
        }
        public async Task<List<FacilityDropEntity>> UserFacilityDrop(int userId)
        {
            this._log.Debug("---Executing UserFacilityDrop() in FacilityService----");
            return await Task.FromResult<List<FacilityDropEntity>>(this._facilityRepository.UserFacilityDrop(userId));
        }
        public async Task<int> GetCompanyToBedFlag(int userId)
        {
            this._log.Debug("---Executing GetCompanyToBedFlag() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.GetCompanyToBedFlag(userId));
        }

        public async Task<int> InsertUpdateNurseShift(List<NurseShiftEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdateNurseShift() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpdateNurseShift(entity));
        }
        public async Task<int> GetCompanyTimeFormat(int facilityId)
        {
            this._log.Debug("---Executing GetCompanyTimeFormat() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.GetCompanyTimeFormat(facilityId));
        }
        public async Task<List<NurseShiftEntity>> GetNurseShifts(int nurseStationId, int facilityId)
        {
            this._log.Debug("---Executing GetNurseShifts() in FacilityService----");
            return await Task.FromResult<List<NurseShiftEntity>>(this._facilityRepository.GetNurseShifts(nurseStationId, facilityId));
        }
        public async Task<int> UpdateNursestationsStatus(List<NursingStationEntity> data)
        {
            this._log.Debug("---Executing UpdateNursestationsStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateNursestationsStatus(data));
        }
        public async Task<int> UpdateFacilityStatus(List<FacilityCustomEntity> data)
        {
            this._log.Debug("---Executing UpdateFacilityStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateFacilityStatus(data));
        }
        public async Task<int> UpdateFloorStatus(List<FloorEntity> data)
        {
            this._log.Debug("---Executing UpdateFloorStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateFloorStatus(data));
        }
        public async Task<int> UpdateWingStatus(List<WingEntity> data)
        {
            this._log.Debug("---Executing UpdateWingStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateWingStatus(data));
        }
        public async Task<int> UpdateCompanyBedConfigsStatus(List<CompanyBedConfigEntity> data)
        {
            this._log.Debug("---Executing UpdateCompanyBedConfigsStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateCompanyBedConfigsStatus(data));
        }
        public async Task<int> UpdateRoomStatus(List<RoomEntity> data)
        {
            this._log.Debug("---Executing UpdateRoomStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateRoomStatus(data));
        }
        public async Task<int> UpdateBedStatus(List<BedEntity> data)
        {
            this._log.Debug("---Executing UpdateBedStatus() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.UpdateBedStatus(data));
        }
        public async Task<List<FloorDropEntity>> GetAllActiveFloorNames()
        {
            this._log.Debug("---Executing GetAllActiveFloorNames() in FacilityService----");
            return await Task.FromResult<List<FloorDropEntity>>(this._facilityRepository.GetAllActiveFloorNames());
        }
        public async Task<List<WingDropEntity>> GetAllActiveWingNames()
        {
            this._log.Debug("---Executing GetAllActiveWingNames() in FacilityService----");
            return await Task.FromResult<List<WingDropEntity>>(this._facilityRepository.GetAllActiveWingNames());
        }
        public async Task<List<BedDropEntity>> GetAllActiveBedNames()
        {
            this._log.Debug("---Executing GetAllActiveBedNames() in FacilityService----");
            return await Task.FromResult<List<BedDropEntity>>(this._facilityRepository.GetAllActiveBedNames());
        }
        public async Task<List<RoomDropEntity>> GetAllActiveRoomNames()
        {
            this._log.Debug("---Executing GetAllActiveRoomNames() in FacilityService----");
            return await Task.FromResult<List<RoomDropEntity>>(this._facilityRepository.GetAllActiveRoomNames());
        }
        public async Task<CompanyToBedEntity> GetCompanyToBedFlagByFacId(int userId, int facilityId, string nurseStations)
        {
            this._log.Debug("---Executing GetCompanyToBedFlagByFacId() in FacilityService----");
            return await Task.FromResult<CompanyToBedEntity>(this._facilityRepository.GetCompanyToBedFlagByFacId(userId, facilityId, nurseStations));
        }
        public async Task<int> InsertUpateComputerName(ProcessKeyEntity entity)
        {
            this._log.Debug("---Executing InsertUpateComputerName() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.InsertUpateComputerName(entity));
        }
        public async Task<List<ProcessMasterEntity>> GetProcessKeyMasterList(int? nsId = null)
        {
            this._log.Debug("---Executing GetProcessKeyMasterList() in FacilityService----");
            return await Task.FromResult<List<ProcessMasterEntity>>(this._facilityRepository.GetProcessKeyMasterList(nsId));
        }
        public async Task<List<NurseStationDropEntity>> UserBiometricNurseStationDrop(int userId)
        {
            this._log.Debug("---Executing UserBiometricNurseStationDrop() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.UserBiometricNurseStationDrop(userId));
        }
        public async Task<Tuple<int, string>> GetDashboardMethodCount(int type, int nurseStationId)
        {
            this._log.Debug("---Executing GetDashboardMethodCount() in FacilityService----");
            return await Task.FromResult<Tuple<int, string>>(this._facilityRepository.GetDashboardMethodCount(type, nurseStationId));
        }
        public async Task<List<NurseStationDropEntity>> UsersConfigNurseStationDrop(string username, string password)
        {
            this._log.Debug("---Executing UsersConfigNurseStationDrop() in FacilityService----");
            return await Task.FromResult<List<NurseStationDropEntity>>(this._facilityRepository.UsersConfigNurseStationDrop(username, password));
        }
        public async Task<int> RemoveComputerName(int processId)
        {
            this._log.Debug("---Executing RemoveComputerName() in FacilityService----");
            return await Task.FromResult<int>(this._facilityRepository.RemoveComputerName(processId));
        }
        public async Task<List<CensusEntity>> GetCensusByNursingStatios(int stationId)
        {
            this._log.Debug("---Executing GetCensusByNursingStatios() in FacilityService----");
            return await Task.FromResult<List<CensusEntity>>(this._facilityRepository.GetCensusByNursingStatios(stationId));
        }
        public async Task<List<ResidentsEntity>> GetCensusResidentsList(CompanyBedConfigCustomEntity entity)
        {
            this._log.Debug("---Executing GetCensusResidentsList() in FacilityService----");
            return await Task.FromResult<List<ResidentsEntity>>(this._facilityRepository.GetCensusResidentsList(entity));
        }
        public async Task<List<tblTimeZoneEntity>> GetTimeZones()
        {
            this._log.Debug("---Executing GetTimeZones() in FacilityService----");
            return await Task.FromResult<List<tblTimeZoneEntity>>(this._facilityRepository.GetTimeZones());
        }
    }
}