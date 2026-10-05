using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public interface ICheckInPharmacyMedsService
    {
        Task<CheckInMedsGridEntity> GetCheckInMedsList(int nsId, string barcode);
        Task<Int64> CheckBarcode(string barcode, int porderId);
        Task<int> CheckInSelectedMeds(List<CheckInSelectedMedsEntity> entity);
        Task<List<OrderStockTransEntity>> GetOrderStockTrans(int quantityId);
        Task<List<EkitCustomEntity>> GetEkitInMedsDetails(int FacilityId, int NsId);
        Task<int> UpdateEkitDetailsInfo(EKitMedsEntity obj);
        Task<int> InsertUpdatePharmacyInfo(PharmacyInfoEntity obj);
        Task<IList> GetGetPharmacyData(int UserId);
        Task<IList> GetPharmacyInfo_ById(int Pharmacy_Id);
        Task<int> updatePharmacyFav(Int64 PId, int Fav, int UserId);
        Task<int> updatePharmacyStatus(string PIds);
        Task<List<BarcodeCheckEntity>> UnionBarcodeDetails(int facility_Id);
        Task<List<EkitdrugEntity>> GetEkitGridDetails(int FacilityId, int NsId);
        Task<List<DestroyEkitGrid>> GetExpiredEkitMedDestruction(int facilityId, int nurseStationId);
        Task<List<EkitlotEntity>> GetEkitLotDetails(int FacilityId, int NsId, string drugName);
        Task<string> EkitDestroyQuantity(List<EkitDestroyQuantity> EkitDestroyObj);
        Task<int> InsertEkitDetails(InsertEkitLotEntity obj);
        Task<int> UpdateEkitDrugQuantity(List<UpdateEkitDrugQty> obj);
        Task<int> FlagEkitLotsGrid(FlagekitGrid obj);
        Task<List<ekitLostGrid>> GetEkitLotsGrid(string drugName, string barcode, int facilityId, int nursingStationId);
        Task<int> AlertEkitAdminister(InsertDrugBarcEkit obj);
        Task<List<DrugEkitEntity>> GetEkitadministration(string gpi);
        Task<List<DtmsEntity>> DrugToDrugINT_DTMS(string gpicode, string patientId, string route, int? freqId, string drug);
        Task<List<DrugAllergyEntity>> DrugToAllergyINT_DTMS(string gpicode, int patientId, string drug);
    }
}
