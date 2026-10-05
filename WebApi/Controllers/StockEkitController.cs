using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [CustomAuthorizationFilter]
    [RoutePrefix("StockEkit")]
    public class StockEkitController : ApiController
    {
        private readonly IStockEkitService _stockEkitService;
        private readonly ILogger _log;
        public StockEkitController(IStockEkitService stockEkitService, ILogger log)
        {
            this._stockEkitService = stockEkitService;
            this._log = log;
        }
        [Route("GetAllBarcodes/{facilityId}")]
        [HttpGet]
        public JsonResult<List<BarcodeEntity>> GetAllBarcodes(int facilityId)
        {
            this._log.Debug("---Executing GetAllBarcodes() in StockEkitController----");
            var barcodes = this._stockEkitService.GetAllBarcodes(facilityId).Result;
            this._log.Debug("---Executed Successfully GetAllBarcodes() in StockEkitController----");
            return Json<List<BarcodeEntity>>(barcodes);
        }
        [Route("InsertUpdateStock")]
        [HttpPost]
        public JsonResult<int> InsertUpdateStock(StockCustomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateStock() in StockEkitController----");
            var result = this._stockEkitService.InsertUpdateStock(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateStock() in StockEkitController----");
            return Json<int>(result);
        }
        [Route("GetAllStock")]
        [HttpPost]
        public JsonResult<StockGridEntity> GetAllStock(StockEkitSearchCustomEntity obj)
        {
            this._log.Debug("---Executing GetAllStock() in StockEkitController----");
            var result = this._stockEkitService.GetAllStock(obj).Result;
            this._log.Debug("---Executed Successfully GetAllStock() in StockEkitController----");
            return Json<StockGridEntity>(result);
        }
        [Route("GetStockById/{stockId}")]
        [HttpGet]
        public JsonResult<StockEntity> GetStockById(int stockId)
        {
            this._log.Debug("---Executing GetStockById() in StockEkitController----");
            var stock = this._stockEkitService.GetStockById(stockId).Result;
            this._log.Debug("---Executed Successfully GetStockById() in StockEkitController----");
            return Json<StockEntity>(stock);
        }
        [Route("InsertUpdateEkit")]
        [HttpPost]
        public JsonResult<int> InsertUpdateEkit(EkitCustomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateEkit() in StockEkitController----");
            var result = this._stockEkitService.InsertUpdateEkit(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateEkit() in StockEkitController----");
            return Json<int>(result);
        }
        [Route("GetAllEkit")]
        [HttpPost]
        public JsonResult<EkitGridEntity> GetAllEkit(StockEkitSearchCustomEntity obj)
        {
            this._log.Debug("---Executing GetAllEkit() in StockEkitController----");
            var result = this._stockEkitService.GetAllEkit(obj).Result;
            this._log.Debug("---Executed Successfully GetAllEkit() in StockEkitController----");
            return Json<EkitGridEntity>(result);
        }
        [Route("GetEkitById/{ekitId}")]
        [HttpGet]
        public JsonResult<EkitEntity> GetEkitById(int ekitId)
        {
            this._log.Debug("---Executing GetEkitById() in StockEkitController----");
            var result = this._stockEkitService.GetEkitById(ekitId).Result;
            this._log.Debug("---Executed Successfully GetEkitById() in StockEkitController----");
            return Json<EkitEntity>(result);
        }
        [Route("GetGenericName/{searchPattern}")]
        [HttpGet]
        public JsonResult<List<DrugInfo>> GetGenericName(string searchPattern)
        {
            this._log.Debug("---Executing GetGenericName() in StockEkitController----");
            var result = this._stockEkitService.GetGenericName(searchPattern).Result;
            this._log.Debug("---Executed Successfully GetGenericName() in StockEkitController----");
            return Json<List<DrugInfo>>(result);
        }
        [Route("UpdateEkitsStatus")]
        [HttpPost]
        public int UpdateEkitsStatus(List<EkitEntity> data)
        {
            this._log.Debug("---Executing UpdateEkitsStatus() in StockEkitController----");
            var result = this._stockEkitService.UpdateEkitsStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateEkitsStatus() in StockEkitController----");
            return result;
        }
        [Route("UpdateStocksStatus")]
        [HttpPost]
        public int UpdateStocksStatus(List<StockEntity> data)
        {
            this._log.Debug("---Executing UpdateStocksStatus() in StockEkitController----");
            var result = this._stockEkitService.UpdateStocksStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateStocksStatus() in StockEkitController----");
            return result;
        }
        [Route("GetAllStockGPICodes")]
        [HttpGet]
        public List<StockCustomEntity> GetAllStockGPICodes()
        {
            this._log.Debug("---Executing GetAllStockGPICodes() in StockEkitController----");
            var result = this._stockEkitService.GetAllStockGPICodes().Result;
            this._log.Debug("---Executed Successfully GetAllStockGPICodes() in StockEkitController----");
            return result;
        }
        [Route("GetAllEkitGPICodes")]
        [HttpGet]
        public List<EkitCustomEntity> GetAllEkitGPICodes()
        {
            this._log.Debug("---Executing GetAllEkitGPICodes() in StockEkitController----");
            var result = this._stockEkitService.GetAllEkitGPICodes().Result;
            this._log.Debug("---Executed Successfully GetAllEkitGPICodes() in StockEkitController----");
            return result;
        }
        [Route("InsertStockEkitClone")]
        [HttpPost]
        public int InsertStockEkitClone(StockEkitCloneEntity stockClone)
        {
            this._log.Debug("---Executing InsertStockEkitClone() in StockEkitController----");
            var result = this._stockEkitService.InsertStockEkitClone(stockClone).Result;
            this._log.Debug("---Executed Successfully InsertStockEkitClone() in StockEkitController----");
            return result;
        }
        [Route("IsDataAvailableToCloneStock/{facilityId}/{nsId}")]
        [HttpGet]
        public int IsDataAvailableToCloneStock(int facilityId, int nsId)
        {
            this._log.Debug("---Executing IsDataAvailableToCloneStock() in StockEkitController----");
            var result = this._stockEkitService.IsDataAvailableToCloneStock(facilityId,nsId).Result;
            this._log.Debug("---Executed Successfully IsDataAvailableToCloneStock() in StockEkitController----");
            return result;
        }
        [Route("IsDataAvailableToCloneEkit/{facilityId}/{nsId}")]
        [HttpGet]
        public int IsDataAvailableToCloneEkit(int facilityId, int nsId)
        {
            this._log.Debug("---Executing IsDataAvailableToCloneEkit() in StockEkitController----");
            var result = this._stockEkitService.IsDataAvailableToCloneEkit(facilityId,nsId).Result;
            this._log.Debug("---Executed Successfully IsDataAvailableToCloneEkit() in StockEkitController----");
            return result;
        }
        [Route("GetAllStockEkitBarcodes/{Flag}")]
        [HttpGet]
        public JsonResult<List<BarcodeEntity>> GetAllStockEkitBarcodes(int Flag)
        {
            this._log.Debug("---Executing GetAllStockEkitBarcodes() in RoleController----");
            var role = this._stockEkitService.GetAllStockEkitBarcodes(Flag).Result;
            this._log.Debug("---Executed Successfully GetAllStockEkitBarcodes() in RoleController----");
            return Json<List<BarcodeEntity>>(role);
        }
        [Route("CheckAutoBarcodeAlert/{BarcodeData}")]
        [HttpGet]
        public int CheckAutoBarcodeAlert(string BarcodeData)
        {
            this._log.Debug("---Executing CheckAutoBarcodeAlert() in StockEkitController----");
            var result = this._stockEkitService.CheckAutoBarcodeAlert(BarcodeData).Result;
            this._log.Debug("---Executed Successfully CheckAutoBarcodeAlert() in StockEkitController----");
            return result;
        }
        [Route("StockGPIAlertint/{facilityId}/{nsId}/{gpiCode}")]
        [HttpGet]
        public List<stockGpiInfo> StockGPIAlertint(int facilityId, int nsId, string gpiCode)
        {
            this._log.Debug("---Executing StockGPIAlertint() in RoleController----");
            var result = this._stockEkitService.StockGPIAlert(facilityId, nsId, gpiCode).Result;
            this._log.Debug("---Executed Successfully StockGPIAlertint() in RoleController----");
            return result;
        }
        [Route("UpdateStockQtyGpi")]
        [HttpPost]
        public int UpdateStockQtyGpi(StockUpdateEntity stockUpdate)
        {
            this._log.Debug("---Executing UpdateStockQtyGpi() in StockEkitController----");
            var result = this._stockEkitService.UpdateStockQtyGpi(stockUpdate).Result;
            this._log.Debug("---Executed Successfully UpdateStockQtyGpi() in StockEkitController----");
            return result;
        }
    }
}