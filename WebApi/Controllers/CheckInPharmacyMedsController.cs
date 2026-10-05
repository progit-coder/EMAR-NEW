using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Filters;
using LTCPro.ServiceLayer;
using System.Web.Http.Results;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.WebApi
{

    [CustomAuthorizationFilter]
    [CustomApiExceptionFilter]
    [RoutePrefix("CheckInPharmacyMeds")]
    public class CheckInPharmacyMedsController : ApiController
    {
        private readonly ICheckInPharmacyMedsService _checkInPharmacyMedsService;
        private readonly ILogger _log;
        public CheckInPharmacyMedsController(ICheckInPharmacyMedsService checkInPharmacyMedsService, ILogger log)
        {
            this._checkInPharmacyMedsService = checkInPharmacyMedsService;            
            this._log = log;
        }
        [Route("GetCheckInMedsList/{NSId}/{Barcode}")]
        [HttpGet]
        public JsonResult<CheckInMedsGridEntity> GetCheckInMedsList(int nsId, string barcode)
        {
            this._log.Debug("---Executing GetCheckInMedsList() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetCheckInMedsList(nsId, barcode).Result;
            this._log.Debug("---Executed Successfully GetCheckInMedsList() in CheckInPharmacyMedsController----");
            return Json<CheckInMedsGridEntity>(records);
        }
        [Route("GetEkitInMedsDetails/{FacilityId}/{NsId}")]
        [HttpGet]
        public JsonResult<List<EkitCustomEntity>> GetEkitInMedsDetails(int FacilityId, int NsId)
        {
            this._log.Debug("---Executing GetEkitInMedsDetails() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetEkitInMedsDetails(FacilityId, NsId).Result;
            this._log.Debug("---Executed Successfully GetEkitInMedsDetails() in CheckInPharmacyMedsController----");
            return Json<List<EkitCustomEntity>>(records);
        }
        [Route("CheckBarcode/{Barcode}/{porderId}")]
        [HttpGet]
        public JsonResult<Int64> CheckBarcode(string barcode ,int porderId)
        {
            this._log.Debug("---Executing CheckBarcode() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.CheckBarcode(barcode, porderId).Result;
            this._log.Debug("---Executed Successfully CheckBarcode() in CheckInPharmacyMedsController----");
            return Json<Int64>(result);

        }
        [Route("CheckInSelectedMeds")]
        [HttpPost]
        public JsonResult<int> CheckInSelectedMeds(List<CheckInSelectedMedsEntity> entity)
        {
            try
            {
                this._log.Debug("---Executing CheckInSelectedMeds() in CheckInPharmacyMedsController----");
                var result = this._checkInPharmacyMedsService.CheckInSelectedMeds(entity).Result;
                this._log.Debug("---Executed Successfully CheckInSelectedMeds() in CheckInPharmacyMedsController----");
                return Json<int>(result);
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing CheckInSelectedMeds() in CheckInPharmacyMedsController Exception---- Message " + ex.Message.ToString());
                this._log.Debug("---Executing CheckInSelectedMeds() in CheckInPharmacyMedsController Exception---- InnerException" + ex.InnerException.Message.ToString());
                return null;
            }
       

        }
        [Route("UpdateEkitDetailsInfo")]
        [HttpPost]
        public JsonResult<int> UpdateEkitDetailsInfo(EKitMedsEntity obj)
        {
            this._log.Debug("---Executing UpdateEkitDetailsInfo() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.UpdateEkitDetailsInfo(obj).Result;
            this._log.Debug("---Executed Successfully UpdateEkitDetailsInfo() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }
        [Route("GetOrderStockTrans/{quantityId}")]
        [HttpGet]
        public JsonResult<List<OrderStockTransEntity>> GetOrderStockTrans(int quantityId)
        {
            this._log.Debug("---Executing GetOrderStockTrans() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.GetOrderStockTrans(quantityId).Result;
            this._log.Debug("---Executed Successfully GetOrderStockTrans() in CheckInPharmacyMedsController----");
            return Json<List<OrderStockTransEntity>>(result);

        }

        [Route("InsertUpdatePharmacyInfo")]
        [HttpPost]
        public JsonResult<int> InsertUpdatePharmacyInfo(PharmacyInfoEntity obj)
        {
            this._log.Debug("---Executing InsertUpdatePharmacyInfo() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.InsertUpdatePharmacyInfo(obj).Result;
            this._log.Debug("---Executed Successfully InsertUpdatePharmacyInfo() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }
        [Route("GetGetPharmacyData/{UserId}")]
        [HttpGet]
        public JsonResult<IList> GetGetPharmacyData(int UserId)
         
    {
            this._log.Debug("---Executing GetGetPharmacyData() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.GetGetPharmacyData(UserId).Result;
            this._log.Debug("---Executed Successfully GetGetPharmacyData() in CheckInPharmacyMedsController----");
            return Json<IList>(result);

        }
        [Route("GetPharmacyInfo_ById/{Pharmacy_Id}")]
        [HttpGet]
        public JsonResult<IList> GetPharmacyInfo_ById(int Pharmacy_Id)
        {
            this._log.Debug("---Executing GetPharmacyInfo_ById() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.GetPharmacyInfo_ById(Pharmacy_Id).Result;
            this._log.Debug("---Executed Successfully GetPharmacyInfo_ById() in CheckInPharmacyMedsController----");
            return Json<IList>(result);

        }

        [Route("updatePharmacyFav/{PId}/{Fav}/{UserId}")]
        [HttpGet]
        public JsonResult<int> updatePharmacyFav(Int64 PId, int Fav, int UserId)
        {
            this._log.Debug("---Executing updatePharmacyFavint() in CheckInPharmacyMedsController----");
            //int UserId = 0;
            var result = this._checkInPharmacyMedsService.updatePharmacyFav(PId, Fav, UserId).Result;
            this._log.Debug("---Executed Successfully updatePharmacyFavint() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }

        [Route("updatePharmacyStatus")]
        [HttpPost]
        public JsonResult<int> updatePharmacyStatus(PharmacyStatus obj)
        {
            this._log.Debug("---Executing updatePharmacyStatus() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.updatePharmacyStatus(obj.Status).Result;
            this._log.Debug("---Executed Successfully updatePharmacyStatus() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }
        [Route("UnionBarcodeDetails/{facility_Id}")]
        [HttpGet]
        public JsonResult<List<BarcodeCheckEntity>> UnionBarcodeDetails(int facility_Id)
        {
            this._log.Debug("---Executing UnionBarcodeDetails() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.UnionBarcodeDetails(facility_Id).Result;
            this._log.Debug("---Executed Successfully UnionBarcodeDetails() in CheckInPharmacyMedsController----");
            return Json<List<BarcodeCheckEntity>>(records);
        }
        [Route("GetEkitGridDetails/{FacilityId}/{NsId}")]
        [HttpGet]
        public JsonResult<List<EkitdrugEntity>> GetEkitGridDetails(int FacilityId, int NsId)
        {
            this._log.Debug("---Executing GetEkitGridDetails() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetEkitGridDetails(FacilityId, NsId).Result;
            this._log.Debug("---Executed Successfully GetEkitGridDetails() in CheckInPharmacyMedsController----");
            return Json<List<EkitdrugEntity>>(records);
        }
        [Route("GetExpiredEkitMedDestruction/{facilityId}/{nurseStationId}")]
        [HttpGet]
        public JsonResult<List<DestroyEkitGrid>> GetExpiredEkitMedDestruction(int facilityId, int nurseStationId)
        {
            this._log.Debug("---Executing GetExpiredEkitMedDestruction() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetExpiredEkitMedDestruction(facilityId, nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetExpiredEkitMedDestruction() in CheckInPharmacyMedsController----");
            return Json<List<DestroyEkitGrid>>(records);
        }
        [Route("GetEkitLotDetails/{FacilityId}/{NsId}/{drugName}")]
        [HttpGet]
        public JsonResult<List<EkitlotEntity>> GetEkitLotDetails(int FacilityId, int NsId, string drugName)
        {
            this._log.Debug("---Executing GetEkitLotDetails() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetEkitLotDetails(FacilityId, NsId, drugName).Result;
            this._log.Debug("---Executed Successfully GetEkitLotDetails() in CheckInPharmacyMedsController----");
            return Json<List<EkitlotEntity>>(records);
        }
        [Route("EkitDestroyQuantity")]
        [HttpPost]
        public JsonResult<string> EkitDestroyQuantity(List<EkitDestroyQuantity> EkitDestroyObj)
        {
            this._log.Debug("---Executing EkitDestroyQuantity() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.EkitDestroyQuantity(EkitDestroyObj).Result;
            this._log.Debug("---Executed Successfully EkitDestroyQuantity() in CheckInPharmacyMedsController----");
            return Json<string>(result);

        }
        [Route("InsertEkitDetails")]
        [HttpPost]
        public JsonResult<int> InsertEkitDetails(InsertEkitLotEntity obj)
        {
            this._log.Debug("---Executing InsertEkitDetails() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.InsertEkitDetails(obj).Result;
            this._log.Debug("---Executed Successfully InsertEkitDetails() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }
        [Route("UpdateEkitDrugQuantity")]
        [HttpPost]
        public JsonResult<int> UpdateEkitDrugQuantity(List<UpdateEkitDrugQty> obj)
        {
            this._log.Debug("---Executing UpdateEkitDrugQuantity() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.UpdateEkitDrugQuantity(obj).Result;
            this._log.Debug("---Executed Successfully UpdateEkitDrugQuantity() in CheckInPharmacyMedsController----");
            return Json<int>(result);
        }
        [Route("FlagEkitLotsGrid")]
        [HttpPost]
        public JsonResult<int> FlagEkitLotsGrid(FlagekitGrid obj)
        {
            this._log.Debug("---Executing FlagEkitLotsGrid() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.FlagEkitLotsGrid(obj).Result;
            this._log.Debug("---Executed Successfully FlagEkitLotsGrid() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }
        [Route("GetEkitLotsGrid/{drugName}/{barcode}/{facilityId}/{nursingStationId}")]
        [HttpGet]
        public JsonResult<List<ekitLostGrid>> GetEkitLotsGrid(string drugName, string barcode, int facilityId, int nursingStationId)
        {
            this._log.Debug("---Executing GetEkitLotsGrid() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetEkitLotsGrid(drugName, barcode, facilityId, nursingStationId).Result;
            this._log.Debug("---Executed Successfully GetEkitLotsGrid() in CheckInPharmacyMedsController----");
            return Json<List<ekitLostGrid>>(records);
        }
        [Route("AlertEkitAdminister")]
        [HttpPost]
        public JsonResult<int> AlertEkitAdminister(InsertDrugBarcEkit obj)
        {
            this._log.Debug("---Executing AlertEkitAdminister() in CheckInPharmacyMedsController----");
            var result = this._checkInPharmacyMedsService.AlertEkitAdminister(obj).Result;
            this._log.Debug("---Executed Successfully AlertEkitAdminister() in CheckInPharmacyMedsController----");
            return Json<int>(result);

        }
        [Route("GetEkitadministration/{gpi}")]
        [HttpGet]
        public JsonResult<List<DrugEkitEntity>> GetEkitadministration(string gpi)
        {
            this._log.Debug("---Executing GetEkitadministration() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.GetEkitadministration(gpi).Result;
            this._log.Debug("---Executed Successfully GetEkitadministration() in CheckInPharmacyMedsController----");
            return Json<List<DrugEkitEntity>>(records);
        }
        [Route("DrugToDrugINT_DTMS/{gpicode}/{patientId}/{route}/{freqId}/{drug}")]
        [HttpGet]
        public JsonResult<List<DtmsEntity>> DrugToDrugINT_DTMS(string gpicode, string patientId, string route, int? freqId, string drug)
        {
            this._log.Debug("---Executing DrugToDrugINT_DTMS() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.DrugToDrugINT_DTMS(gpicode, patientId, route, freqId, drug).Result;
            this._log.Debug("---Executed Successfully DrugToDrugINT_DTMS() in CheckInPharmacyMedsController----");
            return Json<List<DtmsEntity>>(records);
        }
        [Route("DrugToAllergyINT_DTMS/{gpicode}/{patientId}/{drug}")]
        [HttpGet]
        public JsonResult<List<DrugAllergyEntity>> DrugToAllergyINT_DTMS(string gpicode, int patientId, string drug)
        {
            this._log.Debug("---Executing DrugToAllergyINT_DTMS() in CheckInPharmacyMedsController----");
            var records = this._checkInPharmacyMedsService.DrugToAllergyINT_DTMS(gpicode, patientId, drug).Result;
            this._log.Debug("---Executed Successfully DrugToAllergyINT_DTMS() in CheckInPharmacyMedsController----");
            return Json<List<DrugAllergyEntity>>(records);
        }
    }
}
