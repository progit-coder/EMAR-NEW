using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.Repositories
{
    public interface ICheckInPharmacyMedsRepository
    {
        CheckInMedsGridEntity GetCheckInMedsList(int nsId, string barcode);
        Int64 CheckBarcode(string barcode, int porderId);
        int CheckInSelectedMeds(List<CheckInSelectedMedsEntity> entity);
        List<OrderStockTransEntity> GetOrderStockTrans(int quantityId);
        List<EkitCustomEntity> GetEkitInMedsDetails(int FacilityId,int NsId);
        int UpdateEkitDetailsInfo(EKitMedsEntity obj);
        int InsertUpdatePharmacyInfo(PharmacyInfoEntity obj);
        IList GetGetPharmacyData(int UserId);
        IList GetPharmacyInfo_ById(int Pharmacy_Id);
        int updatePharmacyFav(Int64 PId, int Fav, int UserId);
        int updatePharmacyStatus(string PIds);
        List<BarcodeCheckEntity> UnionBarcodeDetails(int facility_Id);
        List<EkitdrugEntity> GetEkitGridDetails(int FacilityId, int NsId);
        List<DestroyEkitGrid> GetExpiredEkitMedDestruction(int facilityId, int nurseStationId);
        List<EkitlotEntity> GetEkitLotDetails(int FacilityId, int NsId, string drugName);
        string EkitDestroyQuantity(List<EkitDestroyQuantity> EkitDestroyObj);
        int InsertEkitDetails(InsertEkitLotEntity obj);
        int UpdateEkitDrugQuantity(List<UpdateEkitDrugQty> obj);
        int FlagEkitLotsGrid(FlagekitGrid obj);
        List<ekitLostGrid> GetEkitLotsGrid(string drugName, string barcode, int facilityId, int nursingStationId);
        int AlertEkitAdminister(InsertDrugBarcEkit obj);
        List<DrugEkitEntity> GetEkitadministration(string gpi);
        List<DtmsEntity> DrugToDrugINT_DTMS(string gpicode, string patientId, string route, int? freqId, string drug);
        List<DrugAllergyEntity> DrugToAllergyINT_DTMS(string gpicode, int patientId, string drug);
    }
}
