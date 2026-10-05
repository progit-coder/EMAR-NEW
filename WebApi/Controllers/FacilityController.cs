using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using LTCPro.Entities;
using System.Web;
using System.IO;
using LTCPro.ServiceLayer;
using Newtonsoft.Json;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Facility")]
    [CustomAuthorizationFilter]
    public class FacilityController : ApiController
    {
        private readonly IFacilityService _facilityService;
        private readonly ILogger _log;
        public FacilityController(IFacilityService facilityService, ILogger log)
        {
            this._facilityService = facilityService;
            this._log = log;
        }
        /*
        [Route("GetSection")]
        [HttpGet]
        public JsonResult<List<SectionEntity>> GetSection()
        {
            var section = this._facilityService.GetSection().Result;
            return Json<List<SectionEntity>>(section);
        }
        [Route("InsertFacADTDefaults")]
        [HttpPost]
        public int InsertFacADTDefaults(FacADTDefaultEntity ADTDefaults)
        {
            this._log.Debug("---Executed Successfully InsertFacADTDefaults() in FacilityController----");
            return this._facilityService.InsertFacADTDefaults(ADTDefaults);
        }
        [Route("InsertFacCarePlan")]
        [HttpPost]
        public int InsertFacCarePlan(FacCarePlanEntity CarePlan)
        {
            this._log.Debug("---Executed Successfully InsertFacCarePlan() in FacilityController----");
            return this._facilityService.InsertFacCarePlan(CarePlan);
        }

        [Route("InsertEHRInfo")]
        [HttpPost]
        public int InsertEHRInfo(FacEhrInfoEntity EHRInfo)
        {
            this._log.Debug("---Executed Successfully InsertEHRInfo() in FacilityController----");
            return this._facilityService.InsertEHRInfo(EHRInfo);
        }

        [Route("InsertFacOther")]
        [HttpPost]
        public int InsertFacOther(FacOtherEntity FacOther)
        {
            this._log.Debug("---Executed Successfully InsertFacOther() in FacilityController----");
            return this._facilityService.InsertFacOther(FacOther);
        }

        [Route("InsertFacilityPO")]
        [HttpPost]
        public int InsertFacilityPO(FacilityPOEntity PhysicianOrders)
        {
            this._log.Debug("---Executed Successfully InsertFacilityPO() in FacilityController----");
            return this._facilityService.InsertFacilityPO(PhysicianOrders);
        }

        [Route("InsertFacilityICD")]
        [HttpPost]
        public int InsertFacilityICD(FacilityICDEntity Icd)
        {
            this._log.Debug("---Executed Successfully InsertFacilityICD() in FacilityController----");
            return this._facilityService.InsertFacilityICD(Icd);
        }
        [Route("InsertUpdateAssessment")]
        [HttpPost]
        public int InsertUpdateAssessment(FacAssessments Facass)
        {
            this._log.Debug("---Executed Successfully InsertUpdateAssessment() in FacilityController----");
            return this._facilityService.InsertUpdateAssessment(Facass);
        }
        */
        [Route("InsertFacility")]
        [HttpPost]
        public int InsertUpdateFacilityMaster(string body)
        {
            FacilityEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<FacilityEntity>(body);
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                byte[] buffer = new byte[16 * 1024];
                Stream input = httpRequest.Files[0].InputStream;
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    entity.Facility_Logo = ms.ToArray();
                }
            }

            this._log.Debug("---Executing InsertUpdateFacilityMaster() in FacilityController----");
            int result = this._facilityService.InsertUpdateFacilityMaster(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateFacilityMaster() in FacilityController----");
            return result;
        }
        [Route("FacilityMaster")]
        [HttpGet]
        public JsonResult<List<FacilityCustomEntity>> GetActiveFacilityMasterList()
        {
            this._log.Debug("---Executing GetActiveFacilityMasterList() in FacilityController----");
            var facilities = this._facilityService.GetActiveFacilityMasterList().Result;
            this._log.Debug("---Executed Successfully GetActiveFacilityMasterList() in FacilityController----");
            return Json<List<FacilityCustomEntity>>(facilities);
        }
        [Route("GetAllFacilityMaster")]
        [HttpGet]
        public JsonResult<List<FacilityCustomEntity>> GetAllFacilityMasterList()
        {
            this._log.Debug("---Executing GetAllFacilityMasterList() in FacilityController----");
            var facilities = this._facilityService.GetAllFacilityMasterList().Result;
            this._log.Debug("---Executed Successfully GetAllFacilityMasterList() in FacilityController----");
            return Json<List<FacilityCustomEntity>>(facilities);
        }
        [Route("Facility/{FacilityId}")]
        [HttpGet]
        public JsonResult<FacilityCustomEntity> GetFacilityDetailsByID(int facilityId)
        {
            this._log.Debug("---Executing Get() in FacilityController----");
            var facility = this._facilityService.GetFacilityDetailsById(facilityId).Result;
            this._log.Debug("---Executed Successfully Get() in FacilityController----");
            return Json<FacilityCustomEntity>(facility);
        }
        [Route("FloorById/{FloorId}")]
        [HttpGet]
        public JsonResult<FloorEntity> GetFloorById(int floorId)
        {
            this._log.Debug("---Executing GetFloorById() in FacilityController----");
            var floor = this._facilityService.GetFloorById(floorId).Result;
            this._log.Debug("---Executed Successfully GetFloorById() in FacilityController----");
            return Json<FloorEntity>(floor);
        }
        [Route("GetAllFloors")]
        [HttpGet]
        public JsonResult<List<FloorEntity>> GetAllFloorsList()
        {
            this._log.Debug("---Executing GetAllFloorsList() in FacilityController----");
            var floors = this._facilityService.GetAllFloorsList().Result;
            this._log.Debug("---Executed Successfully GetAllFloorsList() in FacilityController----");
            return Json<List<FloorEntity>>(floors);
        }
        [Route("InsertFloor")]
        [HttpPost]
        public int InsertUpdateFloorMaster(FloorEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateFloorMaster() in FacilityController----");
            int result = this._facilityService.InsertUpdateFloorMaster(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateFloorMaster() in FacilityController----");
            return result;
        }
        [Route("WingById/{WingId}")]
        [HttpGet]
        public JsonResult<WingEntity> GetWingById(int wingId)
        {
            this._log.Debug("---Executing GetWingById() in FacilityController----");
            var wing = this._facilityService.GetWingById(wingId).Result;
            this._log.Debug("---Executed Successfully GetWingById() in FacilityController----");
            return Json<WingEntity>(wing);
        }

        [Route("GetAllWings")]
        [HttpGet]
        public JsonResult<List<WingEntity>> GetWingsList()
        {
            this._log.Debug("---Executing GetWingsList() in FacilityController----");
            var wing = this._facilityService.GetAllWingsList().Result;
            this._log.Debug("---Executed Successfully GetWingsList() in FacilityController----");
            return Json<List<WingEntity>>(wing);
        }
        [Route("InsertWing")]
        [HttpPost]
        public int InsertUpdateWing(WingEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateWing() in FacilityController----");
            int result = this._facilityService.InsertUpdateWing(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateWing() in FacilityController----");
            return result;
        }
        [Route("NurseStationById/{NurseStationId}")]
        [HttpGet]
        public JsonResult<NursingStationEntity> GetNurseStationById(int nurseStationId)
        {
            this._log.Debug("---Executing GetNurseStationById() in FacilityController----");
            var nurseStation = this._facilityService.GetNurseStationById(nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetNurseStationById() in FacilityController----");
            return Json<NursingStationEntity>(nurseStation);
        }

        [Route("GetAllNurseStations/{facilityId?}")]
        [HttpGet]
        public JsonResult<List<NursingStationEntity>> GetAllNurseStationsList(int? facilityId = null)
        {
            this._log.Debug("---Executing GetAllNurseStationsList() in FacilityController----");
            var nurseStations = this._facilityService.GetAllNurseStationsList(facilityId).Result;
            this._log.Debug("---Executed Successfully GetAllNurseStationsList() in FacilityController----");
            return Json<List<NursingStationEntity>>(nurseStations);
        }

        [Route("GetAllNurseStationsListNew/{facilityid}")]
        [HttpGet]
        public JsonResult<List<NursingStationEntity>> GetAllNurseStationsListNew(int facilityid)
        {
            this._log.Debug("---Executing GetAllNurseStationsListNew() in FacilityController----");
            var nurseStationsnew = this._facilityService.GetAllNurseStationsListNew(facilityid).Result;
            this._log.Debug("---Executed Successfully GetAllNurseStationsListNew(facilityid) in FacilityController----");
            return Json<List<NursingStationEntity>>(nurseStationsnew);
        }

        [Route("InsertNurseStation")]
        [HttpPost]
        public int InsertUpdateNurseStation(NursingStationEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateNurseStation() in FacilityController----");
            int result = this._facilityService.InsertUpdateNurseStation(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateNurseStation() in FacilityController----");
            return result;
        }
        [Route("RoomById/{RoomId}")]
        [HttpGet]
        public JsonResult<RoomEntity> GetRoomById(int roomId)
        {
            this._log.Debug("---Executing GetRoomById() in FacilityController----");
            var room = this._facilityService.GetRoomById(roomId).Result;
            this._log.Debug("---Executed Successfully GetRoomById() in FacilityController----");
            return Json<RoomEntity>(room);
        }

        [Route("GetAllRooms")]
        [HttpGet]
        public JsonResult<List<RoomEntity>> GetAllRoomsList()
        {
            this._log.Debug("---Executing GetAllRoomsList() in FacilityController----");
            var rooms = this._facilityService.GetAllRoomsList().Result;
            this._log.Debug("---Executed Successfully GetAllRoomsList() in FacilityController----");
            return Json<List<RoomEntity>>(rooms);
        }
        [Route("InsertRoom")]
        [HttpPost]
        public int InsertUpdateRoom(RoomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateRoom() in FacilityController----");
            int result = this._facilityService.InsertUpdateRoom(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateRoom() in FacilityController----");
            return result;
        }
        [Route("BedById/{BedId}")]
        [HttpGet]
        public JsonResult<BedEntity> GetBedById(int bedId)
        {
            this._log.Debug("---Executing GetBedById() in FacilityController----");
            var bed = this._facilityService.GetBedById(bedId).Result;
            this._log.Debug("---Executed Successfully GetBedById() in FacilityController----");
            return Json<BedEntity>(bed);
        }

        [Route("GetAllBeds")]
        [HttpGet]
        public JsonResult<List<BedEntity>> GetAllBedsList()
        {
            this._log.Debug("---Executing GetAllBedsList() in FacilityController----");
            var beds = this._facilityService.GetAllBedsList().Result;
            this._log.Debug("---Executed Successfully GetAllBedsList() in FacilityController----");
            return Json<List<BedEntity>>(beds);
        }
        [Route("InsertBed")]
        [HttpPost]
        public int InsertUpdateBed(BedEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateBed() in FacilityController----");
            int result = this._facilityService.InsertUpdateBed(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateBed() in FacilityController----");
            return result;
        }
        [Route("InsertCompanyBedConfig")]
        [HttpPost]
        public int InsertUpdateCompanyBedConfigs(CompanyBedConfigEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateCompanyBedConfigs() in FacilityController----");
            int result = this._facilityService.InsertUpdateCompanyBedConfigs(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateCompanyBedConfigs() in FacilityController----");
            return result;

        }
        [Route("GetCompanyBedConfig/{bedConfigId}")]
        [HttpGet]
        public JsonResult<CompanyBedConfigEntity> GetCompanyBedConfigByBedConfigId(int bedConfigId)
        {
            this._log.Debug("---Executing GetCompanyBedConfigByBedConfigId() in FacilityController----");
            var result = this._facilityService.GetCompanyBedConfigByBedConfigId(bedConfigId).Result;
            this._log.Debug("---Executed Successfully GetCompanyBedConfigByBedConfigId() in FacilityController----");
            return Json<CompanyBedConfigEntity>(result);

        }
        [Route("GetFloorsForNurseStations/{CompanyId}/{FacilityId}/{StationId}/{FloorId}/{WingId}/{RoomId}/{BedId}")]
        [HttpGet]
        public JsonResult<List<FloorEntity>> GetFloorsForNurseStations(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetFloorsForNurseStations() in FacilityController----");
            var floors = this._facilityService.GetFloorsForNurseStations(companyId, facilityId, stationId, floorId, wingId, roomId, bedId).Result;
            this._log.Debug("---Executed Successfully GetFloorsForNurseStations() in FacilityController----");
            return Json<List<FloorEntity>>(floors);
        }
        [Route("GetNurseStationsForFacility/{CompanyId}/{FacilityId}")]
        [HttpGet]
        public JsonResult<List<NursingStationEntity>> GetNurseStationsForFacility(int companyId, int facilityId)
        {
            this._log.Debug("---Executing GetNurseStationsForFacility() in FacilityController----");
            var nurseStations = this._facilityService.GetNurseStationsForFacility(companyId, facilityId).Result;
            this._log.Debug("---Executed Successfully GetNurseStationsForFacility() in FacilityController----");
            return Json<List<NursingStationEntity>>(nurseStations);
        }
        [Route("GetWingsForFloor/{CompanyId}/{FacilityId}/{StationId}/{FloorId}/{WingId}/{RoomId}/{BedId}")]
        [HttpGet]
        public JsonResult<List<WingEntity>> GetWingsForFloor(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetWingsForFloor() in FacilityController----");
            var wings = this._facilityService.GetWingsForFloor(companyId, facilityId, stationId, floorId, wingId, roomId, bedId).Result;
            this._log.Debug("---Executed Successfully GetWingsForFloor() in FacilityController----");
            return Json<List<WingEntity>>(wings);
        }
        [Route("GetRoomsForWing/{CompanyId}/{FacilityId}/{StationId}/{FloorId}/{WingId}/{RoomId}/{BedId}")]
        [HttpGet]
        public JsonResult<List<RoomEntity>> GetRoomsForWing( int companyId, int facilityId,  int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetRoomsForWing() in FacilityController----");
            var rooms = this._facilityService.GetRoomsForWing(companyId, facilityId, stationId, floorId, wingId, roomId, bedId).Result;
            this._log.Debug("---Executed Successfully GetRoomsForWing() in FacilityController----");
            return Json<List<RoomEntity>>(rooms);
        }
        [Route("GetBedsForRoom/{CompanyId}/{FacilityId}/{StationId}/{FloorId}/{WingId}/{RoomId}/{BedId}")]
        [HttpGet]
        public JsonResult<List<BedEntity>> GetBedsForRoom(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId,int bedId )
        {
            this._log.Debug("---Executing GetBedsForRoom() in FacilityController----");
            var beds = this._facilityService.GetBedsForRoom(companyId, facilityId, stationId, floorId, wingId, roomId, bedId ).Result;
            this._log.Debug("---Executed Successfully GetBedsForRoom() in FacilityController----");
            return Json<List<BedEntity>>(beds);
        }
        //[Route("GetBedsForRoom/{FacilityId}/{CompanyId}/{FloorId}/{StationId}/{WingId}/{RoomId}")]
        //[HttpGet]
        //public JsonResult<List<BedEntity>> GetFacilityConfig(int companyId, int facilityId, int[] floors, int[] stations, int[] wings, int[] rooms, int[] beds)
        //{
        //    this._log.Debug("---Executing GetBedsForRoom() in FacilityController----");
        //    //var bedss = this._facilityService.GetBedsForRoom(facilityId, companyId, floors, stations, wings, rooms, beds).Result;
        //    return Json<List<BedEntity>>(bedss);
        //}
        //[Route("GetCompanyBedConfigs")]
        //[HttpPost]
        //public JsonResult<CompanyBedConfigCustomEntity> GetCompanyBedConfigs(string body)
        //{
        //    this._log.Debug("---Executing GetCompanyBedConfigs() in FacilityService----");
        //    CompanyBedConfigCustomEntity filterConfigs = JsonConvert.DeserializeObject<CompanyBedConfigCustomEntity>(body);
        //    var entity = this._facilityService.GetCompanyBedConfigs(filterConfigs).Result;
        //    this._log.Debug("---Executed Successfully GetCompanyBedConfigs() in FacilityService----");
        //    return Json<CompanyBedConfigCustomEntity>(entity);
        //}

        [Route("GetCompanyToBedData/{UserId}")]
        [HttpGet]
        public JsonResult<CompanyBedCustomEntity> GetCompanyToBedData(int userId)
        {
            this._log.Debug("---Executing GetCompanyToBedData() in FacilityController----");
            var record = this._facilityService.GetCompanyToBedData(userId).Result;
            this._log.Debug("---Executed Successfully GetCompanyToBedData() in FacilityController----");
            return Json<CompanyBedCustomEntity>(record);
        }
        [Route("GetUserAccessFacilityNurseStaions/{UserId}")]
        [HttpGet]
        public JsonResult<UserAccesssFacilityNurseStationsEntity> GetUserAccessFacilityNurseStaions(int userId)
        {
            this._log.Debug("---Executing GetUserAccessFacilityNurseStaions() in FacilityController----");
            var record = this._facilityService.GetUserAccessFacilityNurseStaions(userId).Result;
            this._log.Debug("---Executed Successfully GetUserAccessFacilityNurseStaions() in FacilityController----");
            return Json<UserAccesssFacilityNurseStationsEntity>(record);
        }
        [Route("GetCompanyBedGridData/{companyId}/{facilityId}/{stationId}/{floorId}/{wingId}/{roomId}/{bedId}")]
        [HttpGet]
        public JsonResult<List<CompanyBedConfigEntity>> GetCompanyBedGridData(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("----Executing GetCompanyBedGridData() in FacilityController----");
            var records = this._facilityService.GetCompanyBedGridData(companyId, facilityId, stationId, floorId, wingId, roomId, bedId).Result;
            this._log.Debug("----Executed Successfully GetCompanyBedGridData() in FacilityController----");
            return Json<List<CompanyBedConfigEntity>>(records);
        }
        [Route("AllActiveFacilityDrop")]
        [HttpGet]
        public JsonResult<List<FacilityEntity>> GetFacilityDropData()
        {
            this._log.Debug("---Executing GetFacilityDropData() in FacilityController----");
            var facData = this._facilityService.GetFacilityDropData().Result;
            this._log.Debug("---Executed Successfully GetFacilityDropData() in FacilityController----");
            return Json<List<FacilityEntity>>(facData);
        }
        [Route("GetNurseStationDropData")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> GetNurseStationDropData()
        {
            this._log.Debug("---Executing GetNurseStationDropData() in FacilityController----");
            var nurseData = this._facilityService.GetNurseStationDropData().Result;
            this._log.Debug("---Executed Successfully GetNurseStationDropData() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(nurseData);
        }
        [Route("AllActiveNurseStationDrop/{facilityIds}")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> GetAllActiveNurseStationDropData(string facilityIds)
        {
            this._log.Debug("---Executing GetAllActiveNurseStationDropData() in FacilityController----");
            var facData = this._facilityService.GetAllActiveNurseStationDropData(facilityIds).Result;
            this._log.Debug("---Executed Successfully GetAllActiveNurseStationDropData() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(facData);
        }
        [Route("GetFacilitesByCompanyId/{companyId}")]
        [HttpGet]
        public JsonResult<List<FacilitiesbyCompanyIdEntity>> GetFacilitesByCompanyId(int companyId)
        {

            this._log.Debug("---Executing GetFacilitesByCompanyId() in FacilityController----");
            var result = this._facilityService.GetFacilitesByCompanyId(companyId).Result;
            this._log.Debug("---Executed Successfully GetFacilitesByCompanyId() in FacilityController----");
            return Json<List<FacilitiesbyCompanyIdEntity>>(result);
        }

        [Route("GetNurseStations/{userId}")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> UserNurseStationDrop(int userId)
        {

            this._log.Debug("---Executing UserNurseStationDrop() in FacilityController----");
            var result = this._facilityService.UserNurseStationDrop(userId).Result;
            this._log.Debug("---Executed Successfully UserNurseStationDrop() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(result);
        }

        [Route("GetUserNurseStationDropMultiple/{userId}/{facilityId}")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> UserNurseStationDropMultiple(int userId, string facilityId)
        {

            this._log.Debug("---Executing UserNurseStationDrop() in FacilityController----");
            var result = this._facilityService.UserNurseStationDropMultiple(userId, facilityId).Result;
            this._log.Debug("---Executed Successfully UserNurseStationDrop() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(result);
        }

        [Route("GetNurseStationsByFacilityId/{userId}/{facilityId}")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> UserNurseStationDrop(int userId, int facilityId)
        {

            this._log.Debug("---Executing UserNurseStationDrop() in FacilityController----");
            var result = this._facilityService.UserNurseStationDrop(userId, facilityId).Result;
            this._log.Debug("---Executed Successfully UserNurseStationDrop() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(result);
        }
        [Route("GetGpiList/{Nursing}/{fromdate}/{todate}/{Type}")]
        [HttpGet]
        public JsonResult<List<gpiEntity>> GpiList(string Nursing, string fromdate, string todate,string Type)
        {

            this._log.Debug("---Executing GpiList() in FacilityController----");
            var result = this._facilityService.GpiList(Nursing, fromdate, todate, Type).Result;
            this._log.Debug("---Executed Successfully GpiList() in FacilityController----");
            return Json<List<gpiEntity>>(result);
        }


       

        [Route("BedConfigNurseStationDrop")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> BedConfigNurseStationDrop()
        {

            this._log.Debug("---Executing BedConfigNurseStationDrop() in FacilityController----");
            var result = this._facilityService.BedConfigNurseStationDrop().Result;
            this._log.Debug("---Executed Successfully BedConfigNurseStationDrop() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(result);
        }
        [Route("GetUserFacilityDrop/{userId}")]
        [HttpGet]
        public JsonResult<List<FacilityDropEntity>> UserFacilityDrop(int userId)
        {

            this._log.Debug("---Executing UserFacilityDrop() in FacilityController----");
            var result = this._facilityService.UserFacilityDrop(userId).Result;
            this._log.Debug("---Executed Successfully UserFacilityDrop() in FacilityController----");
            return Json<List<FacilityDropEntity>>(result);
        }
     
        [Route("GetCompanyToBedFlag/{userId}")]
        [HttpGet]
        public JsonResult<int> GetCompanyToBedFlag(int userId)
        {

            this._log.Debug("---Executing GetCompanyToBedFlag() in FacilityController----");
            var result = this._facilityService.GetCompanyToBedFlag(userId).Result;
            this._log.Debug("---Executed Successfully GetCompanyToBedFlag() in FacilityController----");
            return Json<int>(result);
        }
        [Route("InsertUpdateNurseShift")]
        [HttpPost]
        public JsonResult<int> InsertUpdateNurseShift(List<NurseShiftEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdateNurseShift() in FacilityController----");
            var result = this._facilityService.InsertUpdateNurseShift(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateNurseShift() in FacilityController----");
            return Json<int>(result);
        }
        [Route("GetCompanyTimeFormat/{facilityId}")]
        [HttpGet]
        public JsonResult<int> GetCompanyTimeFormat(int facilityId)
        {
            this._log.Debug("---Executing GetCompanyTimeFormat() in FacilityController----");
            var result = this._facilityService.GetCompanyTimeFormat(facilityId).Result;
            this._log.Debug("---Executed Successfully GetCompanyTimeFormat() in FacilityController----");
            return Json<int>(result);
        }
        [Route("GetNurseShifts/{nurseStationId}/{facilityId}")]
        [HttpGet]
        public JsonResult<List<NurseShiftEntity>> GetNurseShifts(int nurseStationId, int facilityId)
        {
            this._log.Debug("---Executing GetNurseShifts() in FacilityController----");
            var result = this._facilityService.GetNurseShifts(nurseStationId,facilityId).Result;
            this._log.Debug("---Executed Successfully GetNurseShifts() in FacilityController----");
            return Json<List<NurseShiftEntity>>(result);
        }
        [Route("UpdateNursestationsStatus")]
        [HttpPost]
        public int UpdateNursestationsStatus(List<NursingStationEntity> data)
        {
            this._log.Debug("---Executing UpdateNursestationsStatus() in FacilityController----");
            var result = this._facilityService.UpdateNursestationsStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateNursestationsStatus() in FacilityController----");
            return result;
        }
        [Route("UpdateFacilityStatus")]
        [HttpPost]
        public int UpdateFacilityStatus(List<FacilityCustomEntity> data)
        {
            this._log.Debug("---Executing UpdateFacilityStatus() in FacilityController----");
            var result = this._facilityService.UpdateFacilityStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateFacilityStatus() in FacilityController----");
            return result;
        }
        [Route("UpdateFloorStatus")]
        [HttpPost]
        public int UpdateFloorStatus(List<FloorEntity> data)
        {
            this._log.Debug("---Executing UpdateFloorStatus() in FacilityController----");
            var result = this._facilityService.UpdateFloorStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateFloorStatus() in FacilityController----");
            return result;
        }
        [Route("UpdateWingStatus")]
        [HttpPost]
        public int UpdateWingStatus(List<WingEntity> data)
        {
            this._log.Debug("---Executing UpdateWingStatus() in FacilityController----");
            var result = this._facilityService.UpdateWingStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateWingStatus() in FacilityController----");
            return result;
        }
        [Route("UpdateCompanyBedConfigsStatus")]
        [HttpPost]
        public int UpdateCompanyBedConfigsStatus(List<CompanyBedConfigEntity> data)
        {
            this._log.Debug("---Executing UpdateCompanyBedConfigsStatus() in FacilityController----");
            var result = this._facilityService.UpdateCompanyBedConfigsStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateCompanyBedConfigsStatus() in FacilityController----");
            return result;
        }
        [Route("UpdateRoomStatus")]
        [HttpPost]
        public int UpdateRoomStatus(List<RoomEntity> data)
        {
            this._log.Debug("---Executing UpdateRoomStatus() in FacilityController----");
            var result = this._facilityService.UpdateRoomStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateRoomStatus() in FacilityController----");
            return result;
        }
        [Route("UpdateBedStatus")]
        [HttpPost]
        public int UpdateBedStatus(List<BedEntity> data)
        {
            this._log.Debug("---Executing UpdateBedStatus() in FacilityController----");
            var result = this._facilityService.UpdateBedStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateBedStatus() in FacilityController----");
            return result;
        }
        [Route("GetAllActiveFloorNames")]
        [HttpGet]
        public JsonResult<List<FloorDropEntity>> GetAllActiveFloorNames()
        {
            this._log.Debug("---Executing GetAllActiveFloorNames() in FacilityController----");
            var result = this._facilityService.GetAllActiveFloorNames().Result;
            this._log.Debug("---Executed Successfully GetAllActiveFloorNames() in FacilityController----");
            return Json<List<FloorDropEntity>>(result);
        }
        [Route("GetAllActiveWingNames")]
        [HttpGet]
        public JsonResult<List<WingDropEntity>> GetAllActiveWingNames()
        {
            this._log.Debug("---Executing GetAllActiveWingNames() in FacilityController----");
            var result = this._facilityService.GetAllActiveWingNames().Result;
            this._log.Debug("---Executed Successfully GetAllActiveWingNames() in FacilityController----");
            return Json<List<WingDropEntity>>(result);
        }
        [Route("GetAllActiveBedNames")]
        [HttpGet]
        public JsonResult<List<BedDropEntity>> GetAllActiveBedNames()
        {
            this._log.Debug("---Executing GetAllActiveBedNames() in FacilityController----");
            var result = this._facilityService.GetAllActiveBedNames().Result;
            this._log.Debug("---Executed Successfully GetAllActiveBedNames() in FacilityController----");
            return Json<List<BedDropEntity>>(result);
        }
        [Route("GetAllActiveRoomNames")]
        [HttpGet]
        public JsonResult<List<RoomDropEntity>> GetAllActiveRoomNames()
        {
            this._log.Debug("---Executing GetAllActiveRoomNames() in FacilityController----");
            var result = this._facilityService.GetAllActiveRoomNames().Result;
            this._log.Debug("---Executed Successfully GetAllActiveRoomNames() in FacilityController----");
            return Json<List<RoomDropEntity>>(result);
        }
        [Route("GetCompanyToBedFlagByFacId/{userId}/{facilityId}/{nurseStations?}")]
        [HttpGet]
        public JsonResult<CompanyToBedEntity>  GetCompanyToBedFlagByFacId(int userId, int facilityId,string nurseStations="")
        {
            this._log.Debug("---Executing GetCompanyToBedFlagByFacId() in FacilityController----");
            var result = this._facilityService.GetCompanyToBedFlagByFacId(userId, facilityId, nurseStations).Result;
            this._log.Debug("---Executed Successfully GetCompanyToBedFlagByFacId() in FacilityController----");
            return Json<CompanyToBedEntity>(result);
        }
        [Route("InsertUpateComputerName")]
        [HttpPost]
        public int InsertUpateComputerName(ProcessKeyEntity entity)
        {
            this._log.Debug("---Executing InsertUpateComputerName() in FacilityController----");
            var result = this._facilityService.InsertUpateComputerName(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpateComputerName() in FacilityController----");
            return result;
        }
        [Route("UserBiometricNurseStationDrop/{userId}")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> UserBiometricNurseStationDrop(int userId)
        {
            this._log.Debug("---Executing UserBiometricNurseStationDrop() in FacilityController----");
            var result = this._facilityService.UserBiometricNurseStationDrop(userId).Result;
            this._log.Debug("---Executed Successfully UserBiometricNurseStationDrop() in FacilityController----");
            return Json<List<NurseStationDropEntity>>(result);
        }
[Route("GetDashboardMethodCount/{type}/{nurseStationId}")]
        [HttpGet]
        public Tuple<int,string> GetDashboardMethodCount(int type, int nurseStationId)
        {
            this._log.Debug("---Executing GetDashboardMethodCount() in FacilityController----");
            var result = this._facilityService.GetDashboardMethodCount(type, nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetDashboardMethodCount() in FacilityController----");
            //return Json<Tuple<int>(result);
            return result;
        }
[Route("RemoveComputerName/{processId}")]
        [HttpGet]
        public JsonResult<int> RemoveComputerName(int processId)
        {
            this._log.Debug("---Executing RemoveComputerName() in FacilityController----");
            var result = this._facilityService.RemoveComputerName(processId).Result;
            this._log.Debug("---Executed Successfully RemoveComputerName() in FacilityController----");
            return Json<int>(result);
        }
        [Route("GetCensusByNursingStation/{stationId}")]
        [HttpGet]
        public JsonResult<List<CensusEntity>> GetCensusByNursingStatios(int stationId)
        {
            this._log.Debug("---Executing GetCensusByNursingStatios() in FacilityController----");
            var result = this._facilityService.GetCensusByNursingStatios(stationId).Result;
            this._log.Debug("---Executed Successfully GetCensusByNursingStatios() in FacilityController----");
            return Json<List<CensusEntity>>(result);
        }
        [Route("GetCensusResidentsList")]
        [HttpPost]
        public JsonResult<List<ResidentsEntity>> GetCensusResidentsList(CompanyBedConfigCustomEntity entity)
        {
            this._log.Debug("---Executing GetCensusResidentsList() in FacilityController----");
            var result = this._facilityService.GetCensusResidentsList(entity).Result;
            this._log.Debug("---Executed Successfully GetCensusResidentsList() in FacilityController----");
            return Json<List<ResidentsEntity>>(result);
        }
        [Route("GetTimeZones")]
        [HttpGet]
        public JsonResult<List<tblTimeZoneEntity>> GetTimeZones()
        {
            this._log.Debug("---Executing GetTimeZones() in FacilityController----");
            var result = this._facilityService.GetTimeZones().Result;
            this._log.Debug("---Executed Successfully GetTimeZones() in FacilityController----");
            return Json<List<tblTimeZoneEntity>>(result);
        }
    }
}