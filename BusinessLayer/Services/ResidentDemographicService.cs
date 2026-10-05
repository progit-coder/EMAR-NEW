using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using System.Net.Http;
using System.Web;
using System.Configuration;
using System.IO;
using LTCPro.DAL;
using System.Data.Entity;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LTCPro.ServiceLayer
{
    public class ResidentDemographicService : IResidentDemographicService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IResidentDemographicRepository _residentDemographicRepository;
        private readonly IAllergyInfoRepository _allergyInfoRepository;
        private readonly IDiagnosisInfoRepository _diagnosisInfoRepository;
        private readonly IOutboundFileInformationRepository _fileInfoRepository;
        private readonly ILogger _log;
        public ResidentDemographicService(IAutoMapper autoMapper, IResidentDemographicRepository residentDemographicRepository,
            IAllergyInfoRepository allergyInfoRepository, IDiagnosisInfoRepository diagnosisInfoRepository, IOutboundFileInformationRepository fileInfoRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._residentDemographicRepository = residentDemographicRepository;
            this._allergyInfoRepository = allergyInfoRepository;
            this._diagnosisInfoRepository = diagnosisInfoRepository;
            this._fileInfoRepository = fileInfoRepository;
            this._log = log;
        }
        //public async Task<List<ResidentGridEntity>> GetResidentGridData(string searchPattern, int userId)
        //{
        //    this._log.Debug("---Executing GetResidentGridData() in ResidentDemographicService----");
        //    return await Task.FromResult<List<ResidentGridEntity>>(this._residentDemographicRepository.GetResidentGridData(searchPattern, userId));
        //}
        public async Task<List<ResidentAdmitDischargeEntity>> GetResidentAdmitDischargeData(int patientID)
        {
            this._log.Debug("---Executing GetResidentAdmitDischargeData() in ResidentDemographicService----");
            return await Task.FromResult<List<ResidentAdmitDischargeEntity>>(this._residentDemographicRepository.GetResidentAdmitDischargeData(patientID));
        }
        public async Task<List<LiteralOrdersEntity>> GetLiteralOrdersData(int patientID)
        {
            this._log.Debug("---Executing GetLiteralOrdersData() in ResidentDemographicService----");
            return await Task.FromResult<List<LiteralOrdersEntity>>(this._residentDemographicRepository.GetLiteralOrdersData(patientID));
        }
        public async Task<List<TreatmentInfoEntity>> GetMedications(int patientID)
        {
            this._log.Debug("---Executing GetMedications() in ResidentDemographicService----");
            return await Task.FromResult<List<TreatmentInfoEntity>>(this._residentDemographicRepository.GetMedications(patientID));
        }
        public async Task<ResidentGridEntity> GetResidentsByCompanyBed(CompanyBedConfigCustomEntity configs)
        {
            this._log.Debug("---Executing GetResidentsByCompanyBed() in ResidentDemographicService----");
            return await Task.FromResult<ResidentGridEntity>(this._residentDemographicRepository.GetResidentsByCompanyBed(configs));
        }
        public async Task<int> UploadResidentImage(int PatientID)
        {
            this._log.Debug("---Executing UploadResidentImage() in ResidentDemographicService----");
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            string folderPath = ConfigurationManager.AppSettings.GetValues("ResidentImagePath")[0].ToString();
            //string CompressedfolderPath = ConfigurationManager.AppSettings.GetValues("ResidentImageCompressPath")[0].ToString();
            string filePath = string.Empty;
            string filePathError = string.Empty;
            string CompressedfilePath = string.Empty;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                filePath = HttpContext.Current.Server.MapPath("~/" + folderPath + timeStamp +"_"+ postedFile.FileName);
                string ErrorName = "Error_" + PatientID;
                filePathError = HttpContext.Current.Server.MapPath("~/" + folderPath + ErrorName + "_" + postedFile.FileName);
                //CompressedfilePath = HttpContext.Current.Server.MapPath("~/" + CompressedfolderPath + postedFile.FileName);

                Stream strm = postedFile.InputStream;

                if(strm.Length == 0)
                {
                    return 10;
                }

                Compressimage(strm, filePath, postedFile.FileName, filePathError, postedFile);

                //postedFile.SaveAs(filePath);
                return await Task.FromResult<int>(this._residentDemographicRepository.UploadResidentImage(folderPath + timeStamp + "_" + postedFile.FileName, PatientID));
            }
            else
            {
                this._log.Debug("---Couldn't Save Image----");
                return 0;
            }
        }

        public async Task<DemographicEntity> GetResidentDemographicData(int patientId)
        {
            this._log.Debug("---Executing DemographicEntity() in GetResidentDemographicData----");
            return await Task.FromResult<DemographicEntity>(this._residentDemographicRepository.GetResidentDemographicData(patientId));
        }
        public async Task<VisitInfoEntity> GetResidentAdmitvisitInfoData(int visitId)
        {
            this._log.Debug("---Executing  VisitInfoEntity()  in GetResidentAdmitvisitInfoData----");
            return await Task.FromResult<VisitInfoEntity>(this._residentDemographicRepository.GetResidentAdmitvisitInfoData(visitId));
        }

        public async Task<CommonOrderInfoEntity> GetResidentLiteralordersData(int OrderId)
        {
            this._log.Debug("---Executing  CommonOrderInfoEntity()  in GetResidentLiteralordersData----");
            return await Task.FromResult<CommonOrderInfoEntity>(this._residentDemographicRepository.GetResidentLiteralordersData(OrderId));
        }
        public async Task<List<ResidentLiteralOrderDataEntity>> GetResidentOrderData(int patientId)
        {
            this._log.Debug("---Executing  GetResidentOrderData()  in ResidentDemographicService----");
            return await Task.FromResult(this._residentDemographicRepository.GetResidentOrderData(patientId));
        }


        public async Task<int> InsertResidentMedications(TreatmentInfoEntity medications)
        {
            this._log.Debug("---Executing  TreatmentInfoEntity()  in InsertResidentMedications----");
            return await Task.FromResult(this._residentDemographicRepository.InsertResidentMedications(medications));
        }

        public async Task<int> InsertResidentAdmitVistiInfoData(VisitInfoEntity admitvisitinfo)
        {
            this._log.Debug("---Executing  VisitInfoEntity()  in InsertResidentAdmitVistiInfoData----");
            return await Task.FromResult(this._residentDemographicRepository.InsertResidentAdmitVistiInfoData(admitvisitinfo));
        }

        public async Task<int> InsertResidentDemographicData(DemographicEntity demographics)
        {
            this._log.Debug("---Executing  DemographicEntity()  in InsertResidentDemographicData----");
            return await Task.FromResult(this._residentDemographicRepository.InsertResidentDemographicData(demographics));
        }

        //public async Task<int> InsertResidentLiteralOrdersData(CommonOrderInfoEntity literalorders)
        //{
        //    this._log.Debug("---Executing  CommonOrderInfoEntity()  in InsertResidentLiteralOrdersData----");
        //    return await Task.FromResult(this._residentDemographicRepository.InsertResidentLiteralOrdersData(literalorders));
        //}

        public async Task<ResidentCountCustomEntity> GetResidentsCount(int userId)
        {
            this._log.Debug("---Executing  ResidentCountCustomEntity()  in GetResidentsCount----");
            return await Task.FromResult(this._residentDemographicRepository.GetResidentsCount(userId));

        }
        //public async Task<List<DocFolderEntity>> GetResidentDocFolder(int patientID)

        //{

        //    this._log.Debug("---Executing GetResidentDocFolder() in ResidentDemographicService----");

        //    return await Task.FromResult<List<DocFolderEntity>>(this._residentDemographicRepository.GetResidentDocFolder(patientID));

        //}

        public async Task<TreatmentInfoEntity> GetResidentMedications(int TreatmentId)
        {
            this._log.Debug("---Executing  ResidentCountCustomEntity()  in GetResidentMedications----");
            return await Task.FromResult(this._residentDemographicRepository.GetResidentMedications(TreatmentId));
        }

        public async Task<List<DocFolderEntity>> GetResidentDocFolder(int patientID)
        {
            this._log.Debug("---Executing GetResidentDocFolder() in ResidentDemographicService----");
            return await Task.FromResult<List<DocFolderEntity>>(this._residentDemographicRepository.GetResidentDocFolder(patientID));
        }

        public async Task<DemographicInfoEntity> GetResidentInformation(int patientID)
        {
            this._log.Debug("---Executing GetResidentInformation() in ResidentDemographicService----");
            return await Task.FromResult<DemographicInfoEntity>(this._residentDemographicRepository.GetResidentInformation(patientID));
        }
        public async Task<ResidentDataEntity> GetFacilityNSResidentsDataByPId(int patientId)
        {
            this._log.Debug("---Executing GetFacilityNSResidentsDataByPId() in ResidentDemographicService----");
            return await Task.FromResult<ResidentDataEntity>(this._residentDemographicRepository.GetFacilityNSResidentsDataByPId(patientId));
        }

        public async Task<List<ResidentDropEntity>> GetResidentsByNSId(int nurseStationId)
        {
            this._log.Debug("---Executing GetResidentsNSId() in ResidentDemographicService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._residentDemographicRepository.GetResidentsByNSId(nurseStationId));
        }
        public async Task<List<ResidentDropEntity>> GetResidentsByNSIds(string nurseStationIds)
        {
            this._log.Debug("---Executing GetResidentsByNSIds() in ResidentDemographicService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._residentDemographicRepository.GetResidentsByNSIds(nurseStationIds));
        }
        public async Task<List<ResidentDropEntity>> GetResidentDropData(int userId)
        {
            this._log.Debug("---Executing GetResidentDropData() in ResidentDemographicService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._residentDemographicRepository.GetResidentDropData(userId));
        }        
        public async Task<int> UpdateResidentInfo(VisitUpdateEntity residentinfo)
        {
            this._log.Debug("---Executing  UpdateResidentInfo()  in UpdateResidentInfo----");
            return await Task.FromResult(this._residentDemographicRepository.UpdateResidentInfo(residentinfo));
        }
        public async Task<string> GetNurseStationName(int patientId)
        {
            this._log.Debug("---Executing  GetNurseStationName()  in UpdateResidentInfo----");
            return await Task.FromResult(this._residentDemographicRepository.GetNurseStationName(patientId));
        }

        public async Task<OrdersViewEntity> GetResidentOrdersView(int orderType, Int64 orderId, Int64 quantityId)
        {
            this._log.Debug("---Executing GetResidentOrdersView() in ResidentDemographicService----");
            return await Task.FromResult<OrdersViewEntity>(this._residentDemographicRepository.GetResidentOrdersView(orderType, orderId, quantityId));
        }

        public async Task<int> InsertPatientType(ColourTypeEntity entity)
        {
            this._log.Debug("---Executing InsertPatientType() in ResidentDemographicService----");
            return await Task.FromResult<int>(this._residentDemographicRepository.InsertPatientType(entity));
        }
        public async Task<List<PatientColorTypeEntity>> GetPatientType(int patientId)
        {
            this._log.Debug("---Executing GetPatientType() in EmarService----");
            return await Task.FromResult<List<PatientColorTypeEntity>>(this._residentDemographicRepository.GetPatientType(patientId));
        }
        public async Task<int> GetNurseStationByPId(int patientId)
        {
            this._log.Debug("---Executing GetNurseStationBtPId() in EmarService----");
            return await Task.FromResult<int>(this._residentDemographicRepository.GetNurseStationByPId(patientId));
        }
        public async Task<List<PatientTypeEntity>> GetAllPatientTypesByPId(int patientId)
        {
            this._log.Debug("---Executing GetAllPatientTypesByPId() in EmarService----");
            return await Task.FromResult<List<PatientTypeEntity>>(this._residentDemographicRepository.GetAllPatientTypesByPId(patientId));
        }
        public async Task<int> InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails)
        {
            this._log.Debug("---Executing InsertUpdatePhyscianDetails() in ResidentDemographic Service---");
            return await Task.FromResult<int>(this._residentDemographicRepository.InsertUpdatePhyscianDetails(physcianDetails));
        }
        public async Task<List<PhysicianGridEntity>> GetPhyscianDetails(int userId)
        {
            this._log.Debug("---Executing GetPhyscianDetails() in ResidentDemographic Service---");
            return await Task.FromResult<List<PhysicianGridEntity>>(this._residentDemographicRepository.GetPhyscianDetails(userId));
        }
        public async Task<List<PhysicianGridEntityNew>> GetPhyscianDetailsGridDataNew(int userId)
        {
            this._log.Debug("---Executing GetPhyscianDetailsGridDataNew() in ResidentDemographic Service---");
            return await Task.FromResult<List<PhysicianGridEntityNew>>(this._residentDemographicRepository.GetPhyscianDetailsGridDataNew(userId));
        }
        public async Task<PhysicianDetailsEntity> GetPhyscianDetailsById(string PhyscianNPI,string FacilityId)
        {
            this._log.Debug("---Executing GetPhyscianDetailsById(PhyscianId) in ResidentDemographic Service---");
            return await Task.FromResult<PhysicianDetailsEntity>(this._residentDemographicRepository.GetPhyscianDetailsById(PhyscianNPI,FacilityId));
        }
        public async Task<string> CheckResidentUniqueId(int patientId)
        {
            this._log.Debug("---Executing  CheckResidentUniqueId()  in ResidentDemographic Service----");
            return await Task.FromResult(this._residentDemographicRepository.CheckResidentUniqueId(patientId));
        }
        public async Task<int> InsertUpdatePatientOnLeave(OnLeaveEntity entity)
        {
            this._log.Debug("---Executing  InsertUpdatePatientOnLeave()  in ResidentDemographic Service----");
            return await Task.FromResult(this._residentDemographicRepository.InsertUpdatePatientOnLeave(entity));
        }
        public async Task<int> UpdatePhysiciansStatus(List<PhysicianDetailsEntity> data)
        {
            this._log.Debug("---Executing UpdatePhysiciansStatus() in ResidentDemographic----");
            return await Task.FromResult<int>(this._residentDemographicRepository.UpdatePhysiciansStatus(data));
        }
        public async Task<List<ResidentDropEntity>> GetResidentDetails(Nullable<int> status,string nursestationId,int residentcount)
        {
            this._log.Debug("---Executing GetResidentDetails() in ResidentDemographic----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._residentDemographicRepository.GetResidentDetails(status,nursestationId,residentcount));
        }
        public async Task<int> CheckResidentInternalId(string InternalId)
        {
            this._log.Debug("---Executing CheckResidentInternalId() in UserService----");
            return await Task.FromResult<int>(this._residentDemographicRepository.CheckResidentInternalId(InternalId));
        }
        public async Task<int> InsertMergeStatus(PostMergeDetails objPost)
        {
            this._log.Debug("---Executing InsertMergeStatus() in UserService----");
            return await Task.FromResult<int>(this._residentDemographicRepository.InsertMergeStatus(objPost));
        }
        public async Task<ResidentMergeEntity> GetMergeDetails(int PatientID, int MergePatientID)
        {
            this._log.Debug("---Executing GetMergeDetails in ResidentDemographic Service---");
            return await Task.FromResult<ResidentMergeEntity>(this._residentDemographicRepository.GetMergeDetails(PatientID, MergePatientID));
        }
        public async Task<ResidentMergeDetails> GetResidentMergeDetailsByPatientID(int PatientID)
        {
            this._log.Debug("---Executing GetResidentMergeDetailsByPatientID in ResidentDemographic Service---");
            return await Task.FromResult<ResidentMergeDetails>(this._residentDemographicRepository.GetResidentMergeDetailsByPatientID(PatientID));
        }
        public async Task<int> GetHl7PendingCount()
        {
            this._log.Debug("---Executing GetHl7PendingCount() in UserService----");
            return await Task.FromResult<int>(this._residentDemographicRepository.GetHl7PendingCount());
        }
        public async Task<IList> GetComputerNameDropData()
        {
            this._log.Debug("---Executing GetComputerNameDropData() in UserService----");
            return await Task.FromResult<IList>(this._residentDemographicRepository.GetComputerNameDropData());
        }
        public async Task<string> GetResidentActiveStatus(string mrNumber)
        {
            this._log.Debug("---Executing GetResidentActiveStatus() in UserService----");
            return await Task.FromResult<string>(this._residentDemographicRepository.GetResidentActiveStatus(mrNumber));
        }
        public async Task<string> GetUserProcessKeyByID(int userId)
        {
            this._log.Debug("---Executing GetUserProcessKeyByID() in UserService----");
            return await Task.FromResult<string>(this._residentDemographicRepository.GetUserProcessKeyByID(userId));
        }
        public async Task<List<ResidentsEntity>> GetCertifyOrderResidentGridData(string nursingStations, int userId,string phyNpi)
        {
            this._log.Debug("---Executing GetCertifyOrderResidentGridData() in ResidentDemographic----");
            return await Task.FromResult<List<ResidentsEntity>>(this._residentDemographicRepository.GetCertifyOrderResidentGridData(nursingStations, userId, phyNpi));
        }
        public async Task<IList> GetCertifyOrderGridData(int patientId, string phyNpi)
        {
            this._log.Debug("---Executing GetCertifyOrderGridData() in ResidentDemographic----");
            return await Task.FromResult<IList>(this._residentDemographicRepository.GetCertifyOrderGridData(patientId, phyNpi));
        }
        public async Task<List<ResidentDropEntity>> GetResidentsListByNSId(int nurseStationId)
        {
            this._log.Debug("---Executing GetResidentsListByNSId() in AssesmentService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._residentDemographicRepository.GetResidentsListByNSId(nurseStationId));
        }
        public async Task<List<ResidentsEntity>> GetProfileCertifyOrderResidentGridData(string nursingStations)
        {
            this._log.Debug("---Executing GetProfileCertifyOrderResidentGridData() in ResidentDemographic----");
            return await Task.FromResult<List<ResidentsEntity>>(this._residentDemographicRepository.GetProfileCertifyOrderResidentGridData(nursingStations));
        }
        public async Task<IList> GetProfileCertifyOrderGridData(int patientId)
        {
            this._log.Debug("---Executing GetProfileCertifyOrderGridData() in ResidentDemographic----");
            return await Task.FromResult<IList>(this._residentDemographicRepository.GetProfileCertifyOrderGridData(patientId));
        }
        public async Task<string> GetDefaultPhysicianNursestation(string PhysicianNPI)
        {
            this._log.Debug("---Executing GetDefaultPhysicianNursestation() in ResidentDemographic----");
            return await Task.FromResult<string>(this._residentDemographicRepository.GetDefaultPhysicianNursestation(PhysicianNPI));
        }
        public static void Compressimage(Stream sourcePath, string targetPath, String filename, string ErrorPath,HttpPostedFile postfile)  
        {  
  
  
            try  
            {  
                using (var image = Image.FromStream(sourcePath))  
                {  
                    float maxHeight = 900.0f;  
                    float maxWidth = 900.0f;  
                    int newWidth;  
                    int newHeight;  
                    string extension;  
                    Bitmap originalBMP = new Bitmap(sourcePath);  
                    int originalWidth = originalBMP.Width;  
                    int originalHeight = originalBMP.Height;  
  
                    if (originalWidth > maxWidth || originalHeight > maxHeight)  
                    {  
  
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;  
                        float ratioY = (float)maxHeight / (float)originalHeight;  
                        float ratio = Math.Min(ratioX, ratioY);  
                        newWidth = (int)(originalWidth * ratio);  
                        newHeight = (int)(originalHeight * ratio);  
                    }  
                    else  
                    {  
                        newWidth = (int)originalWidth;  
                        newHeight = (int)originalHeight;  
  
                    }  
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);  
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);  
                    extension = Path.GetExtension(targetPath);  
                    if (extension.ToLower() == ".png" || extension.ToLower() == ".gif")  
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;  
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;  
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                        bitMAP1.Save(targetPath, image.RawFormat);  
  
                        bitMAP1.Dispose();  
                        imgGraph.Dispose();  
                        originalBMP.Dispose();  
                    }  
                    else if (extension.ToLower() == ".jpg")  
                    {  
  
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;  
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;  
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);  
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;  
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);  
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);  
                        myEncoderParameters.Param[0] = myEncoderParameter;  
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);  
  
                        bitMAP1.Dispose();  
                        imgGraph.Dispose();  
                        originalBMP.Dispose();  
  
                    }  
  
                   
                }  
  
            }  
            catch (Exception)  
            {
                postfile.SaveAs(ErrorPath);
                throw;  
  
            }  
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public async Task<int> UpdatePregnecyFeeding(UpdatePregnecyFeedingEntity Role)
        {
            this._log.Debug("---Executing UpdatePregnecyFeeding() in ResidentDemographic----");
            return await Task.FromResult<int>(this._residentDemographicRepository.UpdatePregnecyFeeding(Role));

        }
    }
}
