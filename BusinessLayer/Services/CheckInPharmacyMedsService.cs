using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using System.Collections;
using System.Security.Claims;
using System.Web;

namespace LTCPro.ServiceLayer
{
    public class CheckInPharmacyMedsService : ICheckInPharmacyMedsService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ICheckInPharmacyMedsRepository _checkInPharmacyMedsRepository;
        private readonly ILogger _log;
        private readonly IUserRepository _userRepository;
        public CheckInPharmacyMedsService(IAutoMapper autoMapper, ICheckInPharmacyMedsRepository checkInPharmacyMedsRepository, ILogger log, IUserRepository UserRepository)
        {
            this._autoMapper = autoMapper;
            this._checkInPharmacyMedsRepository = checkInPharmacyMedsRepository;            
            this._log = log;
            this._userRepository = UserRepository;
        }
        public async Task<List<EkitCustomEntity>> GetEkitInMedsDetails(int FacilityId, int NsId)
        {
            this._log.Debug("---Executing GetEkitInMedsDetails() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<EkitCustomEntity>>(this._checkInPharmacyMedsRepository.GetEkitInMedsDetails(FacilityId, NsId));
        }

        public async Task<Int64> CheckBarcode(string barcode, int porderId)
        {
            this._log.Debug("---Executing CheckBarcode() in CheckInPharmacyMedsService----");
            return await Task.FromResult<Int64>(this._checkInPharmacyMedsRepository.CheckBarcode(barcode, porderId));
        }

        public async Task<int> CheckInSelectedMeds(List<CheckInSelectedMedsEntity> entity)
        {
            this._log.Debug("---Executing CheckInSelectedMeds() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.CheckInSelectedMeds(entity));
        }
        public async Task<int> UpdateEkitDetailsInfo(EKitMedsEntity obj)
        {
            this._log.Debug("---Executing UpdateEkitDetailsInfo() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.UpdateEkitDetailsInfo(obj));
        }

        public async Task<CheckInMedsGridEntity> GetCheckInMedsList(int nsId, string barcode)
        {
            this._log.Debug("---Executing GetCheckInMedsList() in CheckInPharmacyMedsService----");
            return await Task.FromResult<CheckInMedsGridEntity>(this._checkInPharmacyMedsRepository.GetCheckInMedsList(nsId, barcode));

        }
        public async Task<List<OrderStockTransEntity>> GetOrderStockTrans(int quantityId)
        {
            this._log.Debug("---Executing GetOrderStockTrans() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<OrderStockTransEntity>>(this._checkInPharmacyMedsRepository.GetOrderStockTrans(quantityId));
        }
        public async Task<int> InsertUpdatePharmacyInfo(PharmacyInfoEntity obj)
        {
            this._log.Debug("---Executing InsertUpdatePharmacyInfo() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.InsertUpdatePharmacyInfo(obj));
        }
        public async Task<IList> GetGetPharmacyData(int UserId)
        {
            this._log.Debug("---Executing GetOrderStockTrans() in CheckInPharmacyMedsService----");
            return await Task.FromResult<IList>(this._checkInPharmacyMedsRepository.GetGetPharmacyData(UserId));
        }
        public async Task<IList> GetPharmacyInfo_ById(int Pharmacy_Id)
        {
            this._log.Debug("---Executing GetPharmacyInfo_ById() in CheckInPharmacyMedsService----");
            return await Task.FromResult<IList>(this._checkInPharmacyMedsRepository.GetPharmacyInfo_ById(Pharmacy_Id));
        }

        public async Task<int> updatePharmacyFav(Int64 PId, int Fav,int UserId)
        {
            this._log.Debug("---Executing updatePharmacyFav() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.updatePharmacyFav(PId, Fav, UserId));
        }


        public async Task<int> updatePharmacyStatus(string PIds)
        {
            this._log.Debug("---Executing updatePharmacyStatus() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.updatePharmacyStatus(PIds));
        }
        public async Task<List<BarcodeCheckEntity>> UnionBarcodeDetails(int facility_Id)
        {
            this._log.Debug("---Executing UnionBarcodeDetails() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<BarcodeCheckEntity>>(this._checkInPharmacyMedsRepository.UnionBarcodeDetails(facility_Id));
        }
        public async Task<List<EkitdrugEntity>> GetEkitGridDetails(int FacilityId, int NsId)
        {
            this._log.Debug("---Executing GetEkitGridDetails() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<EkitdrugEntity>>(this._checkInPharmacyMedsRepository.GetEkitGridDetails(FacilityId, NsId));
        }
        public async Task<List<DestroyEkitGrid>> GetExpiredEkitMedDestruction(int facilityId, int nurseStationId)
        {
            this._log.Debug("---Executing GetExpiredEkitMedDestruction() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<DestroyEkitGrid>>(this._checkInPharmacyMedsRepository.GetExpiredEkitMedDestruction(facilityId, nurseStationId));
        }
        public async Task<List<EkitlotEntity>> GetEkitLotDetails(int FacilityId, int NsId, string drugName)
        {
            this._log.Debug("---Executing GetEkitLotDetails() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<EkitlotEntity>>(this._checkInPharmacyMedsRepository.GetEkitLotDetails(FacilityId, NsId, drugName));
        }
        public async Task<string> EkitDestroyQuantity(List<EkitDestroyQuantity> EkitDestroyObj)
        {
            foreach (var item in EkitDestroyObj)
            {
                var dUserId = this._userRepository.GetUserId(item.DUserName, item.DPassword);
                int loginUserId = 0;
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    loginUserId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                }
                if (dUserId == 0)
                {
                    return "Invalid Destroyer Credentials";
                }
                else
                {
                    if (dUserId == item.LoggedInUserId)
                    {
                        item.DestroyerUserId = dUserId;
                    }
                    else
                    {
                        return "Destroyer Credentials must be loggedin User";
                    }

                }
                var aUserId = this._userRepository.GetUserId(item.AUserName, item.APassword);
                if (aUserId == 0)
                {
                    return "Invalid Approval Credentials";
                }
                else
                {
                    item.ApprovalUserId = aUserId;
                }
            }
            this._log.Debug("---Executing EkitDestroyQuantity() in CheckInPharmacyMedsService----");
            return await Task.FromResult<string>(this._checkInPharmacyMedsRepository.EkitDestroyQuantity(EkitDestroyObj));
        }
        public async Task<int> InsertEkitDetails(InsertEkitLotEntity obj)
        {
            this._log.Debug("---Executing InsertEkitDetails() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.InsertEkitDetails(obj));
        }
        public async Task<int> UpdateEkitDrugQuantity(List<UpdateEkitDrugQty> obj)
        {
            this._log.Debug("---Executing UpdateEkitDrugQuantity() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.UpdateEkitDrugQuantity(obj));
        }
        public async Task<int> FlagEkitLotsGrid(FlagekitGrid obj)
        {
            this._log.Debug("---Executing FlagEkitLotsGrid() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.FlagEkitLotsGrid(obj));
        }
        public async Task<List<ekitLostGrid>> GetEkitLotsGrid(string drugName, string barcode, int facilityId, int nursingStationId)
        {
            this._log.Debug("---Executing GetEkitLotsGrid() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<ekitLostGrid>>(this._checkInPharmacyMedsRepository.GetEkitLotsGrid(drugName, barcode, facilityId, nursingStationId));
        }
        public async Task<int> AlertEkitAdminister(InsertDrugBarcEkit obj)
        {
            this._log.Debug("---Executing AlertEkitAdminister() in CheckInPharmacyMedsService----");
            return await Task.FromResult<int>(this._checkInPharmacyMedsRepository.AlertEkitAdminister(obj));
        }
        public async Task<List<DrugEkitEntity>> GetEkitadministration(string gpi)
        {
            this._log.Debug("---Executing GetEkitadministration() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<DrugEkitEntity>>(this._checkInPharmacyMedsRepository.GetEkitadministration(gpi));
        }
        public async Task<List<DtmsEntity>> DrugToDrugINT_DTMS(string gpicode, string patientId, string route, int? freqId, string drug)
        {
            this._log.Debug("---Executing DrugToDrugINT_DTMS() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<DtmsEntity>>(this._checkInPharmacyMedsRepository.DrugToDrugINT_DTMS(gpicode, patientId, route, freqId, drug));
        }
        public async Task<List<DrugAllergyEntity>> DrugToAllergyINT_DTMS(string gpicode, int patientId, string drug)
        {
            this._log.Debug("---Executing DrugToAllergyINT_DTMS() in CheckInPharmacyMedsService----");
            return await Task.FromResult<List<DrugAllergyEntity>>(this._checkInPharmacyMedsRepository.DrugToAllergyINT_DTMS(gpicode, patientId, drug));
        }
    }
}
