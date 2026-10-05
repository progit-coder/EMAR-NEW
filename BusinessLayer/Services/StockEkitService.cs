using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
namespace LTCPro.ServiceLayer
{
   public class StockEkitService : IStockEkitService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IStockEkitRepository _stockEkitRepository;
        private readonly ILogger _log;
        public StockEkitService(IAutoMapper autoMapper, IStockEkitRepository stockEkitRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._stockEkitRepository = stockEkitRepository;
            this._log = log;
        }
        public async Task<List<BarcodeEntity>> GetAllBarcodes(int facilityId)
        {
            this._log.Debug("---Executing GetAllBarcodes() in StockEkitService----");
            return await Task.FromResult<List<BarcodeEntity>>(this._stockEkitRepository.GetAllBarcodes(facilityId));
        }
        public async Task<int> InsertUpdateStock(StockCustomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateStock() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.InsertUpdateStock(entity));
        }
        public async Task<StockGridEntity> GetAllStock(StockEkitSearchCustomEntity obj)
        {
            this._log.Debug("---Executing GetPatientTypeDrop() in StockEkitService----");
            return await Task.FromResult<StockGridEntity>(this._stockEkitRepository.GetAllStock(obj));
        }
        public async Task<StockEntity> GetStockById(int stockId)
        {
            this._log.Debug("---Executing GetStockById() in StockEkitService----");
            return await Task.FromResult<StockEntity>(this._stockEkitRepository.GetStockById(stockId));
        }
        public async Task<int> InsertUpdateEkit(EkitCustomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateEkit() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.InsertUpdateEkit(entity));
        }
        public async Task<EkitGridEntity> GetAllEkit(StockEkitSearchCustomEntity obj)
        {
            this._log.Debug("---Executing GetAllEkit() in StockEkitService----");
            return await Task.FromResult<EkitGridEntity>(this._stockEkitRepository.GetAllEkit(obj));
        }
        public async Task<EkitEntity> GetEkitById(int ekitId)
        {
            this._log.Debug("---Executing GetEkitById() in StockEkitService----");
            return await Task.FromResult<EkitEntity>(this._stockEkitRepository.GetEkitById(ekitId));
        }
        public async Task<List<DrugInfo>> GetGenericName(string searchPattern)
        {
            this._log.Debug("---Executing GetGenericName() in StockEkitService----");
            return await Task.FromResult<List<DrugInfo>>(this._stockEkitRepository.GetGenericName(searchPattern));
        }
        public async Task<int> UpdateEkitsStatus(List<EkitEntity> data)
        {
            this._log.Debug("---Executing UpdateEkitsStatus() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.UpdateEkitsStatus(data));
        }
        public async Task<int> UpdateStocksStatus(List<StockEntity> data)
        {
            this._log.Debug("---Executing UpdateStocksStatus() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.UpdateStocksStatus(data));
        }
        public async Task<List<StockCustomEntity>> GetAllStockGPICodes()
        {
            this._log.Debug("---Executing GetAllStockGPICodes() in StockEkitService----");
            return await Task.FromResult<List<StockCustomEntity>>(this._stockEkitRepository.GetAllStockGPICodes());
        }
        public async Task<List<EkitCustomEntity>> GetAllEkitGPICodes()
        {
            this._log.Debug("---Executing GetAllEkitGPICodes() in StockEkitService----");
            return await Task.FromResult<List<EkitCustomEntity>>(this._stockEkitRepository.GetAllEkitGPICodes());
        }
        public async Task<int> InsertStockEkitClone(StockEkitCloneEntity stockClone)
        {
            this._log.Debug("---Executing InsertStockEkitClone() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.InsertStockEkitClone(stockClone));
        }
        public async Task<int> IsDataAvailableToCloneStock(int facilityId, int nsId)
        {
            this._log.Debug("---Executing IsDataAvailableToCloneStock() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.IsDataAvailableToCloneStock(facilityId, nsId));
        }
        public async Task<int> IsDataAvailableToCloneEkit(int facilityId, int nsId)
        {
            this._log.Debug("---Executing IsDataAvailableToCloneEkit() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.IsDataAvailableToCloneEkit(facilityId, nsId));
        }
        public async Task<List<BarcodeEntity>> GetAllStockEkitBarcodes(int Flag)
        {
            this._log.Debug("---Executing GetAllStockEkitBarcodes() in StockEkitService----");
            return await Task.FromResult<List<BarcodeEntity>>(this._stockEkitRepository.GetAllStockEkitBarcodes(Flag));
        }
        public async Task<int> CheckAutoBarcodeAlert(string BarcodeData)
        {
            this._log.Debug("---Executing CheckAutoBarcodeAlert() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.CheckAutoBarcodeAlert(BarcodeData));
        }
        public async Task<List<stockGpiInfo>> StockGPIAlert(int facilityId, int nsId, string gpiCode)
        {
            this._log.Debug("---Executing StockGPIAlert() in StockEkitService----");
            return await Task.FromResult<List<stockGpiInfo>>(this._stockEkitRepository.StockGPIAlert(facilityId, nsId, gpiCode));
        }
        public async Task<int> UpdateStockQtyGpi(StockUpdateEntity stockUpdate)
        {
            this._log.Debug("---Executing UpdateStockQtyGpi() in StockEkitService----");
            return await Task.FromResult<int>(this._stockEkitRepository.UpdateStockQtyGpi(stockUpdate));
        }
    }
}
