using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Repositories;
using LTCPro.Entities;
using System.Configuration;
using System.Web;
using System.IO;
using LTCPro.DAL;
using System.Data.Entity;

namespace LTCPro.ServiceLayer
{
    public class EmarService : IEmarService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IEmarRepository _emarRepository;
        private readonly ILogger _log;
        public EmarService(IAutoMapper autoMapper, IEmarRepository emarRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._emarRepository = emarRepository;
            this._log = log;
        }
        public async Task<EmarData> GetEmarResidentGridData(string time, string dateValue, int nurseStationId, int NurseStationShiftId, int showTwoHours)
        {
            this._log.Debug("---Executing GetEmarResidentGridData() in EmarService----");
            var residents = await Task.FromResult<EmarData>(this._emarRepository.GetEmarResidentGridData(time, dateValue, nurseStationId, NurseStationShiftId, showTwoHours));

            //foreach (var resident in residents.GridData)
            //{
            //    resident.ResidentImage = CheckResImageinBiometric(resident.PatientMRNumber) == "" ? ConvertImage(resident.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(resident.PatientMRNumber));
            //    //ConvertImage(resident.ImageLocation);
            //}
            return residents;
        }
        public async Task<IList> GetEmarResidentBarcodes(string dateValue, int nurseStationId)
        {
            this._log.Debug("---Executing GetEmarResidentBarcodes() in EmarService----");
            var residents = await Task.FromResult<IList>(this._emarRepository.GetEmarResidentBarcodes(dateValue, nurseStationId));
            return residents;
        }
        public byte[] ConvertImage(string url)
        {
            if (url != null)
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath("~/" + url);
                if (path != null && File.Exists(path))
                    buffer = File.ReadAllBytes(path);
                else
                {
                    path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                    buffer = File.ReadAllBytes(path);
                }
                return buffer;
            }
            else
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                if (path != null)
                    buffer = File.ReadAllBytes(path);
                return buffer;
            }
        }
        public string CheckResImageinBiometric(string mrnumber)
        {

            string imageLocation = ConfigurationManager.AppSettings.GetValues("ResidentImages")[0].ToString() + mrnumber + ".jpg";
            if (File.Exists(imageLocation))
            {
                return imageLocation;
            }
            else
            {
                return string.Empty;
            }
            return imageLocation;
        }
        public byte[] ConvertBiometricImage(string url)
        {
            if (url != null)
            {
                byte[] buffer = new byte[16 * 1024];
                //string path = HttpContext.Current.Server.MapPath("~/" + url);
                if (url != null && File.Exists(url))
                    buffer = File.ReadAllBytes(url);
                else
                {
                    string path = HttpContext.Current.Server.MapPath("~/" + url);
                    path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                    buffer = File.ReadAllBytes(path);
                }
                return buffer;
            }
            else
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                if (path != null)
                    buffer = File.ReadAllBytes(path);
                return buffer;
            }
        }
        public async Task<List<NursingScheduleDropEntity>> GetNursingScheduleData(int nurseStationId, string scheduleDate)
        {
            this._log.Debug("---Executing GetNursingScheduleData() in EmarService----");
            return await Task.FromResult<List<NursingScheduleDropEntity>>(this._emarRepository.GetNursingScheduleData(nurseStationId, scheduleDate));
        }
        public async Task<List<NursingScheduleDropEntity>> GetNursingScheduleDataByPatientID(int patientId, string scheduleDate,int nursingStationId)
        {
            this._log.Debug("---Executing GetNursingScheduleData() in EmarService----");
            return await Task.FromResult<List<NursingScheduleDropEntity>>(this._emarRepository.GetNursingScheduleDataByPatientID(patientId, scheduleDate, nursingStationId));
        }
      
        public async Task<string> GetResidentBiometricInfo(int patientId)
        {
            this._log.Debug("---Executing GetResidentBiometricInfo() in EmarService----");
            return await Task.FromResult<string>(this._emarRepository.GetResidentBiometricInfo(patientId));
        }
        public async Task<List<EmarOrdersListEntity>> GetEmarOrdersList(int patientId, string time, string dateValue, int shiftId, int window, int userId)
        {
            this._log.Debug("---Executing GetEmarOrdersList() in EmarService----");
            return await Task.FromResult<List<EmarOrdersListEntity>>(this._emarRepository.GetEmarOrdersList(patientId, time, dateValue, shiftId, window,userId));
        }
        public async Task<RefillDataEntity> GetEmarRefillNotes(int orderId)
        {
            this._log.Debug("---Executing GetEmarRefillNotes () in EmarService----");
            return await Task.FromResult<RefillDataEntity>(this._emarRepository.GetEmarRefillNotes(orderId));
        }
        public async Task<int> InsertDrugAdminister(DrugAdministerEntity drugAdministar)
        {
            var result = 0;
            this._log.Debug("---Executing InsertDrugAdminister() in EmarService----POrderID, PQuantityID, Medication_ReasonID " + drugAdministar.POrder_Id + ", " + drugAdministar.pquantity_Id + "," + drugAdministar.MedicationReason_ID);
            using (EMAREntities context = new EMAREntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        result =  this._emarRepository.InsertDrugAdminister(drugAdministar);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return result;
            //this._log.Debug("---Executing InsertDrugAdminister() in EmarService----");
            //return await Task.FromResult<int>(this._emarRepository.InsertDrugAdminister(drugAdministar));
        }

        public async Task<int> InsertNursingFrequencyConfig(NursingFrequencyConfigCustomEntity frequencyconfig)
        {
            this._log.Debug("---Executing InsertNursingFrequencyConfig() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.InsertNursingFrequencyConfig(frequencyconfig));
        }
        public async Task<List<PrcGetFreqMappingData_ResultEntity>> GetNursingFrequencyConfigsData(int userId)
        {
            this._log.Debug("---Executing GetNursingFrequencyConfigsData() in EmarService----");
            return await Task.FromResult<List<PrcGetFreqMappingData_ResultEntity>>(this._emarRepository.GetNursingFrequencyConfigsData(userId));
        }

        public async Task<List<MedicationReasonEntity>> GetMedicationReason()
        {
            this._log.Debug("---Executing GetMedicationReason() in EmarService----");
            return await Task.FromResult<List<MedicationReasonEntity>>(this._emarRepository.GetMedicationReason());
        }
        public async Task<NursingFrequencyConfigCustomEntity> GetNursingFrequencyDetailsByID(int nursingFreqId)
        {
            this._log.Debug("---Executing GetNursingFrequencyDetailsByID() in EmarService----");
            return await Task.FromResult<NursingFrequencyConfigCustomEntity>(this._emarRepository.GetNursingFrequencyDetailsByID(nursingFreqId));
        }
        public async Task<List<PRNDetailsCustomEntity>> GetPRNData(PRNFilterCustomEntity configs)
        {
            this._log.Debug("---Executing GetPRNData() in EmarService----");
            return await Task.FromResult<List<PRNDetailsCustomEntity>>(this._emarRepository.GetPRNData(configs));
        }
        public async Task<List<SeventyTwoHourCheckEntity>> GetSeventyTwoHourCheckDetails(SeventyTwoHourCustomEntity configs)
        {
            this._log.Debug("---Executing GetSeventyTwoHourCheckDetails() in EMARService----");
            return await Task.FromResult<List<SeventyTwoHourCheckEntity>>(this._emarRepository.GetSeventyTwoHourCheckDetails(configs));
        }
        public async Task<int> InsertPRNData(List<PRNInsertCustomEntity> prnData)
        {
            this._log.Debug("---Executing InsertPRNData() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.InsertPRNData(prnData));
        }

        public async Task<int> InsertUpdateSeventyTwoHourChecks(List<SeventyTwoHourInsertEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdateSeventyTwoHourChecks() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.InsertUpdateSeventyTwoHourChecks(entity));
        }
        public async Task<List<SeventyTwoHourCheckEntity>> GetSeventyTwoHourDetailsByfilter(SeventyTwoHourCustomEntity configs)
        {
            this._log.Debug("---Executing GetSeventyTwoHourDetailsByfilter() in EmarService----");
            return await Task.FromResult<List<SeventyTwoHourCheckEntity>>(this._emarRepository.GetSeventyTwoHourDetailsByfilter(configs));
        }
        public async Task<List<PRNDetailsCustomEntity>> GetPRNDetailsByfilter(PRNFilterCustomEntity configs)
        {
            this._log.Debug("---Executing GetPRNDetailsByfilter() in EmarService----");
            return await Task.FromResult<List<PRNDetailsCustomEntity>>(this._emarRepository.GetPRNDetailsByfilter(configs));
        }
        public async Task<int> ByPassBiometric(BypassBiometricEntity byPass)
        {
            this._log.Debug("---Executing ByPassBiometric() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.ByPassBiometric(byPass));
        }

        public async Task<int> InsertAdministredwithoutscanning(NurseCommentsEntity nurseobj)
        {
            this._log.Debug("---Executing InsertAdministredwithoutscanning() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.InsertAdministredwithoutscanning(nurseobj));
        }
        public async Task<List<VitalsCheckEntity>> GetVitalsCheckList(int PQuantityId)
        {
            List<VitalsCheckEntity> result;
            using (EMAREntities context = new EMAREntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        result = this._emarRepository.GetVitalsCheckList(PQuantityId);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return result;
            //this._log.Debug("---Executing GetVitalsCheckList() in EmarService----");
            //return await Task.FromResult<List<VitalsCheckEntity>>(this._emarRepository.GetVitalsCheckList(PQuantityId));
        }
        public async Task<List<VitalsCheckEntity>> GetVitalsChecksListbyQuantityIds(string PQuantityIds)
        {
            this._log.Debug("---Executing GetVitalsChecksListbyQuantityIds() in EmarService----");
            return await Task.FromResult<List<VitalsCheckEntity>>(this._emarRepository.GetVitalsChecksListbyQuantityIds(PQuantityIds));
        }
        public async Task<int> InsertOrderFavouritesData(List<OrderFavouriteDataEntity> entity)
        {
            this._log.Debug("---Executing InsertOrderFavouritesData() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.InsertOrderFavouritesData(entity));
        }

        public async Task<int> GetDueMARAlertData(int userId)
        {
            this._log.Debug("---Executing GetDueMARAlertData() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.GetDueMARAlertData(userId));
        }
        public async Task<List<EkitDropEntity>> GetEkitDropData(int nursestaionId, int orderId, int userId)
        {
            this._log.Debug("---Executing GetEkitDropData() in CommonService----");
            return await Task.FromResult<List<EkitDropEntity>>(this._emarRepository.GetEkitDropData(nursestaionId, orderId, userId));
        }
        public async Task<List<NurseShiftsEntity>> GetNurseShiftDrop(int nursestationId, string scheduleDate)
        {
            this._log.Debug("---Executing GetNurseShiftDrop() in CommonService----");
            return await Task.FromResult<List<NurseShiftsEntity>>(this._emarRepository.GetNurseShiftDrop(nursestationId, scheduleDate));
        }
        public async Task<List<ControlSubstanceTransEntity>> GetControlSubstanceTrans(int quantityId)
        {
            this._log.Debug("---Executing GetControlSubstanceTrans() in EmarService----");
            return await Task.FromResult<List<ControlSubstanceTransEntity>>(this._emarRepository.GetControlSubstanceTrans(quantityId));
        }
        public async Task<int> GetPRNAdministerCountByDate(int orderId, int quantityd, string dateValue)
        {
            this._log.Debug("---Executing GetPRNAdministerCountByDate() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.GetPRNAdministerCountByDate(orderId,quantityd,dateValue));
        }
        public async Task<string> GetInsulinCommentsByQuantityId(int quantityId)
        {
            this._log.Debug("---Executing GetInsulinCommentsByQuantityId() in EmarService----");
            return await Task.FromResult<string>(this._emarRepository.GetInsulinCommentsByQuantityId(quantityId));
        }
        public async Task<List<NurseCommentsEntity>> GetNurseComments(int patientId)
        {
            this._log.Debug("---Executing GetNurseNotes() in OrdersService----");
            return await Task.FromResult<List<NurseCommentsEntity>>(this._emarRepository.GetNurseComments(patientId));
        }
        public async Task<int> InsertUpdateNurseComments(NurseCommentsEntity notes)
        {
            this._log.Debug("---Executing InsertUpdateNurseNotes() in OrdersService----");
            return await Task.FromResult<int>(this._emarRepository.InsertUpdateNurseComments(notes));
        }
        public async Task<string> GetSideEffectsByGPICode(string GPICode)
        {
            this._log.Debug("---Executing GetSideEffectsByGPICode() in EmarService----");
            return await Task.FromResult<string>(this._emarRepository.GetSideEffectsByGPICode(GPICode));
        }
        public async Task<string> AdministerOdersVitalsInfo(VitalsEntity obj)
        {
            this._log.Debug("---Executing AdministerOdersVitalsInfo() in OrdersService----");
            return await Task.FromResult<string>(this._emarRepository.AdministerOdersVitalsInfo(obj));
        }
        public async Task<List<ControlSubstanceTransEntity>> GetEkitControlSubstanceTrans(int ekitId)
        {
            this._log.Debug("---Executing GetEkitControlSubstanceTrans() in OrdersService----");
            return await Task.FromResult<List<ControlSubstanceTransEntity>>(this._emarRepository.GetEkitControlSubstanceTrans(ekitId));
        }
        public async Task<int> GetDate2hrsDiff(TimezoneEntity obj)
        {
            this._log.Debug("---Executing GetDate2hrsDiff() in EmarService----");
            return await Task.FromResult<int>(this._emarRepository.GetDate2hrsDiff(obj));
        }
    }
}
