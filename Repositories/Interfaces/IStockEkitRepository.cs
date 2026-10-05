using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IStockEkitRepository
    {
        int InsertUpdateStock(StockCustomEntity entity);
        StockGridEntity GetAllStock(StockEkitSearchCustomEntity obj);
        StockEntity GetStockById(int stockId);
        List<BarcodeEntity> GetAllBarcodes(int facilityId);
        int InsertUpdateEkit(EkitCustomEntity entity);
        EkitGridEntity GetAllEkit(StockEkitSearchCustomEntity obj);
        EkitEntity GetEkitById(int ekitId);
        List<DrugInfo> GetGenericName(string searchPattern);
        int UpdateEkitsStatus(List<EkitEntity> data);
        int UpdateStocksStatus(List<StockEntity> data);
        List<StockCustomEntity> GetAllStockGPICodes();
        List<EkitCustomEntity> GetAllEkitGPICodes();
        int InsertStockEkitClone(StockEkitCloneEntity stockClone);
        int IsDataAvailableToCloneStock(int facilityId, int nsId);
        int IsDataAvailableToCloneEkit(int facilityId, int nsId);
        List<BarcodeEntity> GetAllStockEkitBarcodes(int Flag);
        int CheckAutoBarcodeAlert(string BarcodeData);
        List<stockGpiInfo> StockGPIAlert(int facilityId, int nsId, string gpiCode);
        int UpdateStockQtyGpi(StockUpdateEntity stockUpdate);
    }
}
