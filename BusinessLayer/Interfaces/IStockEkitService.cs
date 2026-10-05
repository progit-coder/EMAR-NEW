using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
   public interface IStockEkitService
    {
        Task<int> InsertUpdateStock(StockCustomEntity entity);
        Task<StockGridEntity> GetAllStock(StockEkitSearchCustomEntity obj);
        Task<StockEntity> GetStockById(int stockId);
        Task<List<BarcodeEntity>> GetAllBarcodes(int facilityId);
        Task<int> InsertUpdateEkit(EkitCustomEntity entity);
        Task<EkitGridEntity> GetAllEkit(StockEkitSearchCustomEntity obj);
        Task<EkitEntity> GetEkitById(int ekitId);
        Task<List<DrugInfo>> GetGenericName(string searchPattern);
        Task<int> UpdateEkitsStatus(List<EkitEntity> data);
        Task<int> UpdateStocksStatus(List<StockEntity> data);
        Task<List<StockCustomEntity>> GetAllStockGPICodes();
        Task<List<EkitCustomEntity>> GetAllEkitGPICodes();
        Task<int> InsertStockEkitClone(StockEkitCloneEntity stockClone);
        Task<int> IsDataAvailableToCloneStock(int facilityId, int nsId);
        Task<int> IsDataAvailableToCloneEkit(int facilityId, int nsId);
        Task<List<BarcodeEntity>> GetAllStockEkitBarcodes(int Flag);
        Task<int> CheckAutoBarcodeAlert(string BarcodeData);
        Task<List<stockGpiInfo>> StockGPIAlert(int facilityId, int nsId, string gpiCode);
        Task<int> UpdateStockQtyGpi(StockUpdateEntity stockUpdate);
    }
}
