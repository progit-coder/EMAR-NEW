using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using LTCPro.DAL;
using System.Data.Entity;
using System.IO;
using System.Configuration;
//using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LTCPro.ServiceLayer
{
    public class AdminApprovalService : IAdminApprovalService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IAdminApprovalRepository _adminApprovalRepository;
        private readonly IResidentDemographicRepository _residentDemographicRepository;
        private readonly IOutboundFileInformationRepository _fileInfoRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger _log;
        private readonly ICommonRepository _commonRepository;
        private readonly IUserRepository _userRepository;
        string accessKey = ConfigurationManager.AppSettings.GetValues("azureStorekey")[0].ToString();
        string folderName = ConfigurationManager.AppSettings.GetValues("folderName")[0].ToString();
        string azurePath = ConfigurationManager.AppSettings.GetValues("azurePath")[0].ToString();
        private const int MaxBlockSize = 4000000;
        enum OutboundEvents
        {
            TransferPatient = 9,
            DischargeEndVisit = 10,
            UpdatePatientInformation = 11,
            PatientGoesOnALeaveOfAbsence = 12,
            PatientReturnsFromALeaveOfAbsence = 13,
            OrderMessage = 14,
            AdmitVisitNotification = 15,
        }

        public AdminApprovalService(IAutoMapper autoMapper, IDbContextEmar dbContext, IAdminApprovalRepository adminApprovalRepository, ResidentDemographicRepository residentDemographicRepository, IOutboundFileInformationRepository fileInfoRepository, ICompanyRepository companyRepository, ILogger log, IOrdersRepository ordersRepository, CommonRepository commonRepository, UserRepository userRepository)
        {
            this._autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._adminApprovalRepository = adminApprovalRepository;
            this._residentDemographicRepository = residentDemographicRepository;
            this._fileInfoRepository = fileInfoRepository;
            this._companyRepository = companyRepository;
            this._ordersRepository = ordersRepository;
            this._log = log;
            this._commonRepository = commonRepository;
            this._userRepository = userRepository;
        }

        public async Task<int> InsertResidentAllergy(ApprovalAllergyInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentAllergy() in AdminApprovalService----");
            entity.PAllergy_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (this._companyRepository.GetApprovalFlag((int)entity.Patient_Id) == 1)
            {
                return await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentAllergy(entity));
            }
            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)entity.Patient_Id);
                            int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertUpdateAllergyWithoutApproval(entity, context));
                            if (result == 1)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)
                                    filePath = GenerateOutboundFile((int)entity.Patient_Id, "Allergy", 0, context, transaction);
                                else
                                    transaction.Commit();

                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }

        }

        public async Task<int> InsertResidentDemographicData(ApprovalDemographicEntity demographics)
        {
            this._log.Debug("---Executing InsertResidentDemographicData() in AdminApprovalService----");
            demographics.Patient_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (this._companyRepository.GetApprovalFlag((int)demographics.Patient_Id) == 1)
            {
                return await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentDemographicData(demographics));
            }

            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)demographics.Patient_Id);
                            int result = await Task.FromResult<int>(this._adminApprovalRepository.UpdateDemographicWithoutApproval(demographics, context));
                            if (result == 1)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)
                                    filePath = GenerateOutboundFile((int)demographics.Patient_Id, "Demographics", 0, context, transaction);
                                else
                                    transaction.Commit();
                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }
            return 0;

        }
        public async Task<int> InsertNewResidentDemographicData(NewResidentInfoEntity residentInfo)
        {
            this._log.Debug("---Executing InsertResidentDemographicData() in AdminApprovalService----");
            residentInfo.VisitInfoDetails.PVisit_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            residentInfo.DemographicDetails.Patient_CreatedDate = residentInfo.VisitInfoDetails.PVisit_CreatedDate;
            //if (this._companyRepository.GetApprovalFlagByFacilityId((int)residentInfo.VisitInfoDetails.FacilityId) == 1)
            //{
            //    return await Task.FromResult<int>(this._adminApprovalRepository.InsertNewDemographicDetails(residentInfo,1));
            //}
            if (residentInfo.DemographicDetails.Patient_Id == 0)
            {

                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {

                            int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertNewDemographicDetails(residentInfo, 0));
                            List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)result).ToList();
                            if (result != 0)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.AdmitVisitNotification).Count() != 0)
                                    filePath = GenerateOutboundFile((int)result, "NewResident", 0, context, transaction);
                                else
                                    transaction.Commit();
                            }
                            //if (result == 1)
                            //{
                            //    if (allowedEvents.Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)
                            //        filePath = GenerateOutboundFile((int)demographics.Patient_Id, "Demographics", 0, context, transaction);
                            //    else
                            //        transaction.Commit();
                            //}
                            else
                            {
                                transaction.Commit();
                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }
            return 0;

        }
        public async Task<int> InsertUpdateApprovalRefill(ApprovalRefillEntity refillObj)
        {
            this._log.Debug("---Executing InsertUpdateApprovalRefill() in AdminApprovalService----");
            refillObj.Refill_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (this._companyRepository.GetApprovalFlag((int)refillObj.Patient_Id) == 1)
            {
                return await Task.FromResult<int>(this._adminApprovalRepository.InsertUpdateApprovalRefill(refillObj));
            }
            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //Insert into Approval Table
                            int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertUpdateApprovalRefill(refillObj));
                            //Approve the record
                            ApprovalPendingCustomEntity item = new ApprovalPendingCustomEntity()
                            {
                                ApprovalId = result,
                                Patient_Id = (int)refillObj.Patient_Id,
                                ApprovedBy = (int)refillObj.Refill_CreatedBy,
                                ApprovalStatus = 1,
                                ApprovedDate = (DateTime)refillObj.Refill_CreatedDate

                            };

                            result = await Task.FromResult<int>(this._adminApprovalRepository.ApproveRefill(item, context));
                            if (result == 1)
                            {
                                filePath = GenerateOutboundFile((int)refillObj.Patient_Id, "Refill", (int)refillObj.Porder_Id, context, transaction);

                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }
        }
        public async Task<int> DiscontinueOrder(OrdersCommonStatusEntity entity)
        {
            this._log.Debug("---Executing DiscontinueOrder() in AdminApprovalService----");
            if (this._companyRepository.GetApprovalFlag((int)entity.PatientId) == 1)
            {
                //return await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentAllergy(entity));
                return 0;
            }
            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)entity.Patient_Id);
                            //int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertUpdateDiagnosisWithoutApproval(entity, context));
                            //if (result == 1)
                            //{
                            //    if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)

                            //        filePath = GenerateOutboundFile((int)entity.Patient_Id, "Diagnosis", 0, context, transaction);
                            //    else
                            //        transaction.Commit();

                            //}
                            //return result;

                            int result = await Task.FromResult<int>(this._adminApprovalRepository.DiscontinueOrder(entity, context));
                            //string fileData = context.PrcGenerateHL7forPatientDiscontinueOrder((int)entity.PatientId, entity.OrderId).FirstOrDefault();
                            if (result == 1) 
                             //&& (entity.Split == 0 || (entity.Split == 1 && entity.DiscontinueAllSplits == 1)))
                            {
                                //if ((entity.Split == 1 || entity.Split==0) && fileData != "")
                                //{
                                    filePath = GenerateOutboundFile((int)entity.PatientId, "Discontinue", entity.OrderId, context, transaction);
                                //}
                                //else
                                //{
                                    //transaction.Commit();
                                //}
                            }
                            //else
                            //{
                            //    if (entity.Split == 1)
                            //    {
                            //        int generateFile = await Task.FromResult<int>(this._adminApprovalRepository.DiscontinuedSplitOrder(entity, context));
                            //        if (generateFile == 1)
                            //        {
                            //            filePath = GenerateOutboundFile((int)entity.PatientId, "Discontinue", entity.OrderId, context, transaction);
                            //        }
                            //        else
                            //            transaction.Commit();
                            //    }
                            //}
                            this.dbContext.InsertOrderChangesforReport(entity.OrderId);
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            transaction.Rollback();
                            if(File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            throw ex;
                        }
                    }
                }
            }

        }

        public string GenerateOutboundFile(int patientId, string category, int orderId, EMAREntities context, DbContextTransaction transaction)
        {
            this._log.Debug("---Executing GenerateOutboundFile() " + category + " in AdminApprovalService----");
            string filePath = string.Empty;
            string fileData = string.Empty;
            string uniqueid = context.PrcGetMessageId().FirstOrDefault();
            string ID = "";
            if (category == "Allergy")
            {
                fileData = context.PrcGenerateHL7forPatientUpdate(patientId, category, uniqueid).FirstOrDefault();
                ID = patientId.ToString();
            }
            else if (category == "Diagnosis")
            {
                fileData = context.PrcGenerateHL7forPatientUpdate(patientId, category, uniqueid).FirstOrDefault();
                ID = patientId.ToString();

            }
            else if (category == "Demographics")
            {
                fileData = context.PrcGenerateHL7forPatientUpdate(patientId, category, uniqueid).FirstOrDefault();
                ID = patientId.ToString();

            }
            else if (category == "Transfer")
            {
                fileData = context.prcGenerateHL7forPatientTransfer(patientId, uniqueid).FirstOrDefault();
                ID = patientId.ToString();

            }
            else if (category == "Discharge")
            {
                fileData = context.prcGenerateHL7forPatientDischarge(patientId, uniqueid).FirstOrDefault();
                ID = patientId.ToString();

            }
            else if (category == "NewOrder")
            {
                fileData = context.PrcGenerateHL7forPatientNewOrder(patientId, orderId, uniqueid).FirstOrDefault();
                ID = orderId.ToString();

            }
            else if (category == "Refill")
            {
                fileData = context.PrcGenerateHL7forPatientRefillOrder(patientId, orderId, uniqueid).FirstOrDefault();
                ID = orderId.ToString();

            }
            else if (category == "Discontinue")
            {
                fileData = context.PrcGenerateHL7forPatientDiscontinueOrder(patientId, orderId, uniqueid).FirstOrDefault();
                ID = orderId.ToString();

            }
            else if (category == "NewResident")
            {
                fileData = context.PrcGenerateHL7forNewPatient(patientId,uniqueid).FirstOrDefault();
                ID = patientId.ToString();

            }
            else if (category == "CPOENewOrder")
            {
                fileData = context.PrcGenerateHL7forPatientNewCPOEOrder(patientId, orderId, uniqueid).FirstOrDefault();
                ID = orderId.ToString();

            }
            if (fileData != string.Empty && fileData != null)
            {
                this._log.Debug("---Generating OutboundFile " + category + " in AdminApprovalService----");
                //string username = context.Users.Where(u => u.User_Id == record.ApprovedBy).Select(u => u.UserName).FirstOrDefault();
                //insert into outboundfileinformation and update respected table
                OutBoundFileInformationEntity entity = new OutBoundFileInformationEntity();
                entity.File_Name = category + "_" +this._commonRepository.GetNursingStationTimeZoneDate().ToString("MMddyyyyhhmmss") + "_" + ID + ".hl7";
                entity.File_Data = fileData;
                entity.File_Category = 2;
                entity.Event = category;//ToDo - get category from [EventCategory]
                entity.File_CreatedDate = this._commonRepository.GetNursingStationTimeZoneDate(); //DateTime.Now;
                entity.File_Status = 1;
                entity.File_Error = string.Empty;
                entity.Patient_Id = patientId;
                entity.Porder_Id = orderId == 0 ? null as int? : orderId;
                entity.MessageId = uniqueid;
                int FileId = this._fileInfoRepository.InsertFileInformation(entity, patientId, context);
                if (category == "Refill")
                {
                    this._adminApprovalRepository.InsertApprovedRefillFileId(patientId, orderId, FileId, context);
                }

                string destPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
                filePath = destPath + "/" + entity.File_Name;
                // UploadBlob(Encoding.UTF8.GetBytes(fileData), entity.File_Name);
                this._log.Debug("---Generated OutboundFile Download Start " + filePath + " in AdminApprovalService----");
                System.IO.File.WriteAllText(@filePath, fileData);
                this._log.Debug("---Generated OutboundFile Download Completed " + filePath + " in AdminApprovalService----");

            }
            else
            {
                this._log.Debug("---Received FileData is " + fileData + " for PatientId: " + patientId + ",  Category is " + category + "----");
            }
            this._log.Debug("---Executed GenerateOutboundFile() " + category + " in AdminApprovalService----");
            if (filePath != null && filePath != string.Empty)
            {
                context.SaveChanges();
                transaction.Commit();
                return filePath;
            }
            else
            {
                transaction.Rollback();
                return filePath;
            }

        }
        public void UploadBlob(byte[] fileContent, string blobName)
        {


            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(folderName);
            CloudBlobDirectory directory = cloudBlobContainer.GetDirectoryReference(azurePath);

            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(azurePath + blobName);
            HashSet<string> blocklist = new HashSet<string>();
            foreach (FileBlock block in GetFileBlocks(fileContent))
            {
                blob.PutBlock(
                    block.Id,
                    new MemoryStream(block.Content, true),
                    null
                    );

                blocklist.Add(block.Id);
            }
            blob.PutBlockList(blocklist);
        }

        private IEnumerable<FileBlock> GetFileBlocks(byte[] fileContent)
        {
            HashSet<FileBlock> hashSet = new HashSet<FileBlock>();
            if (fileContent.Length == 0)
                return new HashSet<FileBlock>();

            int blockId = 0;
            int ix = 0;

            int currentBlockSize = MaxBlockSize;

            while (currentBlockSize == MaxBlockSize)
            {
                if ((ix + currentBlockSize) > fileContent.Length)
                    currentBlockSize = fileContent.Length - ix;
                byte[] chunk = new byte[currentBlockSize];
                Array.Copy(fileContent, ix, chunk, 0, currentBlockSize);

                hashSet.Add(
                    new FileBlock()
                    {
                        Content = chunk,
                        Id = Convert.ToBase64String(System.BitConverter.GetBytes(blockId))
                    });

                ix += currentBlockSize;
                blockId++;
            }

            return hashSet;
        }
        internal class FileBlock
        {
            public string Id { get; set; }
            public byte[] Content { get; set; }
        }
        public async Task<int> InsertResidentDianosisData(ApprovalDiagnosisInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentDianosisData() in AdminApprovalService----");
            entity.PDiagnosis_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (this._companyRepository.GetApprovalFlag((int)entity.Patient_Id) == 1)
            {
                return await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentDianosisData(entity));
            }
            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)entity.Patient_Id);
                            int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertUpdateDiagnosisWithoutApproval(entity, context));
                            if (result == 1)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)

                                    filePath = GenerateOutboundFile((int)entity.Patient_Id, "Diagnosis", 0, context, transaction);
                                else
                                    transaction.Commit();

                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }

        }

        public async Task<int> InsertResidentVisitInfo(ApprovalVisitInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentVisitInfo() in AdminApprovalService----");
            entity.PVisit_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (this._companyRepository.GetApprovalFlag((int)entity.Patient_Id) == 1)
            {
                return await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentVisitInfo(entity));
            }
            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)entity.Patient_Id);

                            int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentVisitInfoWithoutApproval(entity, context));
                            if (result == 1)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.TransferPatient).Count() != 0)
                                    filePath = GenerateOutboundFile((int)entity.Patient_Id, "Transfer", 0, context, transaction);
                                else
                                    transaction.Commit();

                            }
                            else if (result == 2)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.DischargeEndVisit).Count() != 0)
                                    filePath = GenerateOutboundFile((int)entity.Patient_Id, "Discharge", 0, context, transaction);
                                else
                                    transaction.Commit();
                            }
                            else if (result == 3)
                            {
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)
                                    filePath = GenerateOutboundFile((int)entity.Patient_Id, "Demographics", 0, context, transaction);
                                else
                                    transaction.Commit();
                            }
                            
                            else if (result == 4)
                                transaction.Commit();
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }
        }
        /*
        public async Task<int> InsertResidentInfo(ApprovalVisitInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentInfo() in AdminApprovalService----");
            return await Task.FromResult<int>(this._adminApprovalRepository.InsertResidentInfo(entity));
        }
        */
        public async Task<List<ApprovalPendingCustomEntity>> GetAllApprovalPendingList()
        {
            List<ApprovalPendingCustomEntity> records = new List<ApprovalPendingCustomEntity>();
            this._log.Debug("---Executing GetApprovalPendingList() in AdminApprovalService----");
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingAllergies().OrderByDescending(a => a.PAllergy_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPAllergy_Id,
                    Record_Id = (int)a.PAllergy_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Category = "Allergy",
                    CategoryId = 1,
                    CreatedBy = Convert.ToInt32(a.PAllergy_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PAllergy_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PAllergy_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());

            records.AddRange(this._adminApprovalRepository.GetApprovalPendingDiagnosis().OrderByDescending(a => a.PDiagnosis_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPDiagnosis_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.PDiagnosis_Id,
                    Category = "Diagnosis",
                    CategoryId = 2,
                    CreatedBy = Convert.ToInt32(a.PDiagnosis_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PDiagnosis_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PDiagnosis_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingDemographics().OrderByDescending(a => a.Patient_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPatient_Id,
                    Patient_Id = a.Patient_Id == null ? 0 : (int)a.Patient_Id,
                    Patient_Name = a.Patient_Id == null ? a.PatientLastName + "," + a.PatientFirstName : this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = a.Patient_Id == null ? 0 : (int)a.Patient_Id,
                    Category = "Demographics",
                    CategoryId = 3,
                    CreatedBy = Convert.ToInt32(a.Patient_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.Patient_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.Patient_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingDischarge().OrderByDescending(a => a.PVisit_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPVisit_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.PVisit_Id,
                    Category = "Discharge",
                    CategoryId = 4,
                    CreatedBy = Convert.ToInt32(a.PVisit_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PVisit_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PVisit_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingTransfer().OrderByDescending(a => a.PVisit_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPVisit_Id,
                    Patient_Id = a.Patient_Id == null ? 0 : (int)a.Patient_Id,
                    Patient_Name = a.Patient_Id == null ? "" : this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = a.PVisit_Id == null ? 0 : (int)a.PVisit_Id,
                    Category = "Transfer",
                    CategoryId = 5,
                    CreatedBy = Convert.ToInt32(a.PVisit_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PVisit_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PVisit_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingOrders().OrderByDescending(a => a.POrder_CreatedBy).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.PApprovalOrder_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.Porder_Id,
                    Category = "NewOrder",
                    CategoryId = 6,
                    CreatedBy = Convert.ToInt32(a.POrder_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.POrder_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.POrder_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingRefill().OrderByDescending(a => a.Refill_CreatedDate).Select
               (a => new ApprovalPendingCustomEntity
               {
                   ApprovalId = a.Refill_Id,
                   Patient_Id = (int)a.Patient_Id,
                   Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                   Record_Id = (int)a.Porder_Id,
                   Category = "Refill",
                   CategoryId = 5,
                   CreatedBy = Convert.ToInt32(a.Refill_CreatedBy),
                   UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.Refill_CreatedBy)),
                   CreatedDate = Convert.ToDateTime(a.Refill_CreatedDate)
               }).OrderBy(item => item.Patient_Name).ToList());
            this._log.Debug("---Executing GetAllApprovalPendingList() in AdminApprovalService----");
            return await Task.FromResult<List<ApprovalPendingCustomEntity>>(records.OrderBy(item => item.Patient_Name).ThenBy(item => item.Category).ToList());
        }
        public async Task<List<ApprovalPendingCustomEntity>> GetApprovalPendingList(int patientId)
        {
            List<ApprovalPendingCustomEntity> records = new List<ApprovalPendingCustomEntity>();
            this._log.Debug("---Executing GetApprovalPendingList() in ResidentDemographicService----");
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingAllergiesByPatientId(patientId).OrderByDescending(a => a.PAllergy_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPAllergy_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.PAllergy_Id,
                    Category = "Allergy",
                    CategoryId = 1,
                    CreatedBy = Convert.ToInt32(a.PAllergy_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PAllergy_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PAllergy_CreatedDate)

                }).OrderBy(item => item.Patient_Name).ToList());

            records.AddRange(this._adminApprovalRepository.GetApprovalPendingDiagnosisByPatientId(patientId).OrderByDescending(a => a.PDiagnosis_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPDiagnosis_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.PDiagnosis_Id,
                    Category = "Diagnosis",
                    CategoryId = 2,
                    CreatedBy = Convert.ToInt32(a.PDiagnosis_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PDiagnosis_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PDiagnosis_CreatedDate)
                }).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingDemographicsByPatientId(patientId).OrderByDescending(a => a.Patient_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPatient_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.Patient_Id,
                    Category = "Demographics",
                    CategoryId = 3,
                    CreatedBy = Convert.ToInt32(a.Patient_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.Patient_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.Patient_CreatedDate)
                }).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingDischargeByPatientId(patientId).OrderByDescending(a => a.PVisit_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPVisit_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.PVisit_Id,
                    Category = "Discharge",
                    CategoryId = 4,
                    CreatedBy = Convert.ToInt32(a.PVisit_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PVisit_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PVisit_CreatedDate)
                }).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingTransferByPatientId(patientId).OrderByDescending(a => a.PVisit_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.ApprovalPVisit_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.PVisit_Id,
                    Category = "Transfer",
                    CategoryId = 5,
                    CreatedBy = Convert.ToInt32(a.PVisit_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.PVisit_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.PVisit_CreatedDate)
                }).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingOrdersByPatientId(patientId).OrderByDescending(a => a.POrder_CreatedBy).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.PApprovalOrder_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.Porder_Id,
                    Category = "NewOrder",
                    CategoryId = 6,
                    CreatedBy = Convert.ToInt32(a.POrder_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.POrder_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.POrder_CreatedDate)
                }).OrderBy(item => item.Patient_Name).ToList());
            records.AddRange(this._adminApprovalRepository.GetApprovalPendingRefillByPatientId(patientId).OrderByDescending(a => a.Refill_CreatedDate).Select
                (a => new ApprovalPendingCustomEntity
                {
                    ApprovalId = a.Refill_Id,
                    Patient_Id = (int)a.Patient_Id,
                    Patient_Name = this._residentDemographicRepository.GetResidentName((int)a.Patient_Id),
                    Record_Id = (int)a.Porder_Id,
                    Category = "Refill",
                    CategoryId = 5,
                    CreatedBy = Convert.ToInt32(a.Refill_CreatedBy),
                    UserName = this._adminApprovalRepository.GetUserNameById(Convert.ToInt16(a.Refill_CreatedBy)),
                    CreatedDate = Convert.ToDateTime(a.Refill_CreatedDate)
                }).ToList());
            this._log.Debug("---Executed GetApprovalPendingList() in AdminApprovalService----");
            return await Task.FromResult<List<ApprovalPendingCustomEntity>>(records);
        }

        public async Task<int> ApprovePendingData(List<ApprovalPendingCustomEntity> data)
        {
            this._log.Debug("---Executing ApprovePendingData() in AdminApprovalService----");
            int result = 0;
            string filePath = null;
            //var keywords = data.GroupBy(w => w.CategoryId).Select(g => new
            //{
            //    category = g.Key,
            //    RecordIds = g.Select(c => c.Record_Id)
            //});
            var keywords = data.GroupBy(w => w.CategoryId).Select(g => new
            {
                category = g.Key,
                ApprovalIds = g.Select(c => c.ApprovalId)
            });
            foreach (var record in keywords)
            {
                if (record.ApprovalIds.Count() > 1)
                {

                    using (EMAREntities context = new EMAREntities())
                    {
                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (int approvalId in record.ApprovalIds)
                                {
                                    var item = data.Where(p => p.ApprovalId == approvalId).FirstOrDefault();
                                    item.ApprovedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                                    item.CreatedDate = item.ApprovedDate;
                                    this._log.Debug("---Executing ApprovePendingData() for " + item.Category + " in AdminApprovalService----");
                                    if (item.Category == "Allergy")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.ApproveAllergy(item, context));
                                    }
                                    else if (item.Category == "Diagnosis")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.ApproveDiagnosis(item, context));
                                    }
                                    else if (item.Category == "Demographics")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.ApproveDemographic(item, context));
                                    }
                                    else if (item.Category == "Transfer")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.ApproveTransfer(item, context));
                                    }
                                    else if (item.Category == "Discharge")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.ApproveDischarge(item, context));
                                    }
                                    else if (item.Category == "Refill")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.ApproveRefill(item, context));
                                    }
                                    else if (item.Category == "NewOrder")
                                    {
                                        int orderId = await Task.FromResult<int>(this._adminApprovalRepository.ApproveNewOrder(item, context));
                                        if (orderId > 0)
                                        {
                                            item.Record_Id = orderId;
                                            item.Category = this._adminApprovalRepository.IsCPOEOrder(item.ApprovalId, context) == 1 ? "CPOENewOrder" : item.Category;
                                            filePath = ApproveInsertOutboundFileData(item, context);
                                            if (filePath != null)
                                            {
                                               context.SaveChanges();
                                                transaction.Commit();
                                                result = 1;
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                result = 0;
                                            }
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            result = 0;
                                        }
                                    }

                                }

                                var proc = data.Where(p => p.ApprovalId == record.ApprovalIds.FirstOrDefault()).FirstOrDefault();
                                if (proc.Category != "NewOrder")
                                {
                                    filePath = ApproveInsertOutboundFileData(proc, context);
                                    if (filePath != null)
                                    {
                                        context.SaveChanges();
                                        transaction.Commit();
                                        result = 1;
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        result = 0;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                                transaction.Rollback();
                                File.Delete(filePath);
                                this._log.Debug("---Delete file if generated " + filePath + " ----");
                                this._log.Debug("---Exception is: " + ex.Message + " ----");
                                throw ex;
                            }
                        }
                    }
                }
                else if (record.ApprovalIds.Count() == 1)
                {

                    using (EMAREntities context = new EMAREntities())
                    {
                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                var item = data.Where(p => p.ApprovalId == record.ApprovalIds.FirstOrDefault()).FirstOrDefault();
                                item.ApprovedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                                item.CreatedDate = item.ApprovedDate;
                                this._log.Debug("---Executing ApprovePendingData() for " + item.Category + " in AdminApprovalService----");

                                if (item.Category == "Allergy")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.ApproveAllergy(item, context));
                                }
                                else if (item.Category == "Diagnosis")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.ApproveDiagnosis(item, context));
                                }
                                else if (item.Category == "Demographics")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.ApproveDemographic(item, context));
                                }
                                else if (item.Category == "Transfer")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.ApproveTransfer(item, context));
                                }
                                else if (item.Category == "Discharge")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.ApproveDischarge(item, context));
                                }
                                else if (item.Category == "Refill")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.ApproveRefill(item, context));
                                }
                                else if (item.Category == "NewOrder")
                                {
                                    int orderId = await Task.FromResult<int>(this._adminApprovalRepository.ApproveNewOrder(item, context));
                                    if (orderId > 0)
                                    {
                                        item.Record_Id = orderId;
                                        item.Category = this._adminApprovalRepository.IsCPOEOrder(item.ApprovalId, context) == 1 ? "CPOENewOrder" : item.Category;
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        result = 0;
                                    }

                                }

                                filePath = ApproveInsertOutboundFileData(item, context);
                                if (filePath != null && filePath != string.Empty)
                                {
                                    context.SaveChanges();
                                    transaction.Commit();
                                    result = 1;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    result = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                                transaction.Rollback();
                                File.Delete(filePath);
                                this._log.Debug("---Delete file if generated " + filePath + " ----");
                                this._log.Debug("---Exception is: " + ex.Message + " ----");
                                throw ex;


                            }
                        }
                    }
                }
            }
            this._log.Debug("---Executed ApprovePendingData() in AdminApprovalService----");
            return await Task.FromResult<int>(result);
        }
        public string ApproveInsertOutboundFileData(ApprovalPendingCustomEntity record, EMAREntities context)
        {
            this._log.Debug("---Executing ApproveInsertOutboundFileData() for " + record.Category + " in AdminApprovalService----");

            string fileData = string.Empty;
            string uniqueid = context.PrcGetMessageId().FirstOrDefault();
            if (record.Category == "Allergy")
            {
                fileData = context.PrcGenerateHL7forPatientUpdate(record.Patient_Id, record.Category, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "Diagnosis")
            {
                fileData = context.PrcGenerateHL7forPatientUpdate(record.Patient_Id, record.Category, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "Demographics")
            {
                fileData = context.PrcGenerateHL7forPatientUpdate(record.Patient_Id, record.Category, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "Transfer")
            {
                fileData = context.prcGenerateHL7forPatientTransfer(record.Patient_Id, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "Discharge")
            {
                fileData = context.prcGenerateHL7forPatientDischarge(record.Patient_Id, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "NewOrder")
            {
                fileData = context.PrcGenerateHL7forPatientNewOrder(record.Patient_Id, record.Record_Id, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "Refill")
            {
                fileData = context.PrcGenerateHL7forPatientRefillOrder(record.Patient_Id, record.Record_Id, uniqueid).FirstOrDefault();
            }
            else if (record.Category == "CPOENewOrder")
            {
                fileData = context.PrcGenerateHL7forPatientNewCPOEOrder(record.Patient_Id, record.Record_Id, uniqueid).FirstOrDefault();
            }
            if (fileData != string.Empty && fileData != null)
            {
                this._log.Debug("---Generating OutboundFile " + record.Category + " in AdminApprovalService----");
                //string username = context.Users.Where(u => u.User_Id == record.ApprovedBy).Select(u => u.UserName).FirstOrDefault();
                //insert into outboundfileinformation and update respected table
                OutBoundFileInformationEntity entity = new OutBoundFileInformationEntity();
                entity.File_Name = record.Category + "_" + this._commonRepository.GetNursingStationTimeZoneDate().ToString("MMddyyyyhhmmss") + ".hl7";
                entity.File_Data = fileData;
                entity.File_Category = 2;
                entity.Event = record.Category;//ToDo - get category from [EventCategory]
                entity.File_CreatedDate = this._commonRepository.GetNursingStationTimeZoneDate(); //DateTime.Now;
                entity.File_Status = 1;
                entity.File_Error = string.Empty;
                entity.MessageId = uniqueid;
                int FileId = this._fileInfoRepository.InsertFileInformation(entity, record.Patient_Id, context);

                string destPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
                var filePath = destPath + "/" + entity.File_Name;
             //   UploadBlob(Encoding.UTF8.GetBytes(fileData), entity.File_Name);
                System.IO.File.WriteAllText(@filePath, fileData);
                this._log.Debug("---Generated OutboundFile " + filePath + " in AdminApprovalService----");
                if (record.Category == "Refill")
                {
                    this._adminApprovalRepository.InsertApprovedRefillFileId(record.Patient_Id, record.Record_Id, FileId, context);
                }

                return filePath;
            }
            else
            {
                this._log.Debug("---Received FileData is " + fileData + " for PatientId: " + record.Patient_Id + ", Record_Id: " + record.Record_Id + " Category is " + record.Category + "----");
            }
            this._log.Debug("---Executed ApproveInsertOutboundFileData() " + record.Category + " in AdminApprovalService----");

            return null;

        }

        public async Task<List<DemographicResidentDropEnity>> GetAdminApprovalResidentsData()
        {
            this._log.Debug("---Executing  GetAdminApprovalResidentsData()  in AdminApprovalService----");
            return await Task.FromResult(this._adminApprovalRepository.GetAdminApprovalResidentsData());
        }
        public async Task<int> RejectPendingData(List<ApprovalPendingCustomEntity> data)
        {
            int result = 0;
            var keywords = data.GroupBy(w => w.CategoryId).Select(g => new
            {
                category = g.Key,
                RecordIds = g.Select(c => c.Record_Id)
            });

            foreach (var record in keywords)
            {
                if (record.RecordIds.Count() > 1)
                {

                    using (EMAREntities context = new EMAREntities())
                    {
                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (int recordId in record.RecordIds)
                                {
                                    var item = data.Where(p => p.Record_Id == recordId).FirstOrDefault();
                                    item.ApprovedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                                    item.CreatedDate = item.ApprovedDate;
                                    if (item.Category == "Allergy")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectAllergy(item));
                                    }
                                    else if (item.Category == "Diagnosis")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectDiagnosis(item));
                                    }
                                    else if (item.Category == "Demographics")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectDemographic(item));
                                    }
                                    else if (item.Category == "Transfer")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectTransferDischarge(item));
                                    }
                                    else if (item.Category == "Discharge")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectTransferDischarge(item));
                                    }
                                    else if (item.Category == "NewOrder")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectNewOrder(item));
                                    }
                                    else if (item.Category == "Refill")
                                    {
                                        await Task.FromResult<int>(this._adminApprovalRepository.RejectRefill(item));
                                    }
                                }
                                context.SaveChanges();
                                transaction.Commit();
                                result = 1;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else if (record.RecordIds.Count() == 1)
                {

                    using (EMAREntities context = new EMAREntities())
                    {
                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                var item = data.Where(p => p.Record_Id == record.RecordIds.FirstOrDefault()).FirstOrDefault();
                                item.ApprovedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                                item.CreatedDate = item.ApprovedDate;
                                if (item.Category == "Allergy")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectAllergy(item));
                                }
                                else if (item.Category == "Diagnosis")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectDiagnosis(item));
                                }
                                else if (item.Category == "Demographics")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectDemographic(item));
                                }
                                else if (item.Category == "Transfer")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectTransferDischarge(item));
                                }
                                else if (item.Category == "Discharge")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectTransferDischarge(item));
                                }
                                else if (item.Category == "NewOrder")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectNewOrder(item));
                                }
                                else if (item.Category == "Refill")
                                {
                                    await Task.FromResult<int>(this._adminApprovalRepository.RejectRefill(item));
                                }
                                context.SaveChanges();
                                transaction.Commit();
                                result = 1;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
            }
            return await Task.FromResult<int>(result);
        }

        public async Task<List<ApprovalChangesCustomEntity>> GetModifiedData(int approvalId, int recordId, string category)
        {
            this._log.Debug("---Executing  GetModifiedData()  in AdminApprovalService----");
            return await Task.FromResult(this._adminApprovalRepository.GetModifiedData(approvalId, recordId, category));
        }

        public async Task<int> InsertApprovalOrderData(ApprovalOrderEntity entity)
        {
            this._log.Debug("---Executing  InsertApprovalOrderData()  in AdminApprovalService----");
            entity.TransactionDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            entity.OrderEffectiveDate = entity.TransactionDate;
            entity.POrder_CreatedDate = entity.TransactionDate;
            if (this._companyRepository.GetApprovalFlag((int)entity.Patient_Id) == 1)
            {
                return await Task.FromResult<int>(this._adminApprovalRepository.InsertApprovalOrderData(entity));
            }
            else
            {
                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)entity.Patient_Id);

                            //Insert into Approval Table
                            int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertApprovalOrderData(entity));
                            if (result != 2)
                            {
                                //Approve the record
                                ApprovalPendingCustomEntity item = new ApprovalPendingCustomEntity()
                                {
                                    ApprovalId = result,
                                    Patient_Id = (int)entity.Patient_Id,
                                    ApprovedBy = (int)entity.POrder_CreatedBy,
                                    ApprovalStatus = 1,
                                    ApprovedDate = (DateTime)entity.POrder_CreatedDate

                                };
                                int orderId = await Task.FromResult<int>(this._adminApprovalRepository.ApproveNewOrder(item, context));
                                if (orderId > 0)
                                    item.Record_Id = orderId;
                                else
                                {
                                    transaction.Rollback();
                                    result = 0;
                                }

                                if (result > 0)
                                {
                                    if (allowedEvents != null && allowedEvents.ToList().Where(i => i.EventCat_Id == (int)OutboundEvents.OrderMessage).Count() != 0)//&& entity.OrderTypeID==1
                                        filePath = GenerateOutboundFile((int)entity.Patient_Id, "NewOrder", item.Record_Id, context, transaction);
                                    else
                                        transaction.Commit();
                                    if (entity.EndDate != null && Convert.ToDateTime(entity.EndDate).Date <= Convert.ToDateTime(entity.POrder_CreatedDate).Date)
                                    {
                                        OrdersCommonStatusEntity discontinueOrder = new OrdersCommonStatusEntity();
                                        var quantityId = this.dbContext.QuantityDetails.Where(q => q.POrder_Id == orderId).Select(q => q.PQuantity_Id).FirstOrDefault();
                                        var dAdministerId = this.dbContext.DrugAdministrationTimes.Where(d => d.POrder_Id == orderId && d.PQuantity_Id == quantityId).FirstOrDefault();
                                        discontinueOrder.PatientId = (int)entity.Patient_Id;
                                        discontinueOrder.DAdminId = dAdministerId == null ? 0 : dAdministerId.DAdmin_Id;
                                        discontinueOrder.OrderId = orderId;
                                        discontinueOrder.QuantityId = quantityId;
                                        discontinueOrder.POrderCreatedBy = entity.POrder_CreatedBy;
                                        discontinueOrder.POrderStatus = 2;
                                        discontinueOrder.POOutBoundApproval = 0;
                                        discontinueOrder.POOutBoundApprovalBy = 0;
                                        discontinueOrder.OrderType = "Discontinue";
                                        discontinueOrder.DiscontinueFlag = 1;
                                        discontinueOrder.DiscontinueReason = "";
                                        discontinueOrder.DiscontinueAllSplits = 0;
                                        discontinueOrder.DiscontinuedOn = entity.POrder_CreatedDate;
                                        discontinueOrder.Split = 0;
                                        this.DiscontinueOrder(discontinueOrder);
                                    }
                                    //insert Schedule Text
                                    var quantityID= this.dbContext.QuantityDetails.Where(q => q.POrder_Id == orderId).Select(q => q.PQuantity_Id).FirstOrDefault();
                                    this._ordersRepository.InsertScheduleTimeText(Convert.ToInt32(orderId), Convert.ToInt32(quantityID), entity.ScheduleText);

                                }
                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }


        }
        public async Task<int> InsertApprovalCPOEOrderData(ApprovalCPOEOrderEntity entity)
        {
            this._log.Debug("---Executing Check User Credentials in OrdersService----");
            var UserId = this._userRepository.GetUserId(entity.UserName, entity.Password);
            if (UserId == 0 || entity.POrder_CreatedBy != UserId)
            {
                return -1;
            }
            else
            {
                this._log.Debug("---Executing  InsertApprovalOrderData()  in AdminApprovalService----");
                entity.TransactionDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                entity.OrderEffectiveDate = entity.TransactionDate;
                entity.POrder_CreatedDate = entity.TransactionDate;
                if (this._companyRepository.GetApprovalFlag((int)entity.Patient_Id) == 1)
                {
                    return await Task.FromResult<int>(this._adminApprovalRepository.InsertApprovalCPOEOrderData(entity));
                }
                else
                {
                    string filePath = string.Empty;
                    using (EMAREntities context = new EMAREntities())
                    {
                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)entity.Patient_Id);

                                //Insert into Approval Table
                                int result = await Task.FromResult<int>(this._adminApprovalRepository.InsertApprovalCPOEOrderData(entity));
                                this._log.Debug("---Executing InsertApprovalCPOEOrderData in OrdersService + result----" + result);
                                if (result != 2)
                                {
                                    //Approve the record
                                    ApprovalPendingCustomEntity item = new ApprovalPendingCustomEntity()
                                    {
                                        ApprovalId = result,
                                        Patient_Id = (int)entity.Patient_Id,
                                        ApprovedBy = (int)entity.POrder_CreatedBy,
                                        ApprovalStatus = 1,
                                        ApprovedDate = (DateTime)entity.POrder_CreatedDate,
                                        DUom = entity.DUom == null ? null :  (Nullable<int>) entity.DUom,
                                        Sig2DUom = entity.Sig2DUom == null ? null : (Nullable<int>)entity.Sig2DUom,
                                        Sig3DUom = entity.Sig3DUom == null ? null : (Nullable<int>)entity.Sig3DUom,
                                        Sig4DUom = entity.Sig4DUom == null ? null : (Nullable<int>)entity.Sig4DUom,


                                    };
                                    int orderId = await Task.FromResult<int>(this._adminApprovalRepository.ApproveCPOENewOrder(item, context));
                                    this._log.Debug("---Executing ApproveCPOENewOrder in OrdersService + orderId----" + orderId);

                                    if (orderId > 0)
                                        item.Record_Id = orderId;
                                    else
                                    {
                                        transaction.Rollback();
                                        result = 0;
                                    }

                                    if (result > 0)
                                    {
                                        this._log.Debug("---Executing GenerateOutboundFile in OrdersService , entity.Patient_Id , CPOENewOrder , item.Record_Id , context ,transaction----" + entity.Patient_Id + "," + item.Record_Id + "," + context + "," + transaction);
                                        if (allowedEvents != null && allowedEvents.ToList().Where(i => i.EventCat_Id == (int)OutboundEvents.OrderMessage).Count() != 0)//&& entity.OrderTypeID==1
                                             filePath = GenerateOutboundFile((int)entity.Patient_Id, "CPOENewOrder", item.Record_Id, context, transaction);
                                        else
                                            transaction.Commit();
                                        if (entity.EndDate != null && Convert.ToDateTime(entity.EndDate).Date <= Convert.ToDateTime(entity.POrder_CreatedDate).Date)
                                        {
                                            OrdersCommonStatusEntity discontinueOrder = new OrdersCommonStatusEntity();
                                            var quantityId = this.dbContext.QuantityDetails.Where(q => q.POrder_Id == orderId).Select(q => q.PQuantity_Id).FirstOrDefault();
                                            var dAdministerId = this.dbContext.DrugAdministrationTimes.Where(d => d.POrder_Id == orderId && d.PQuantity_Id == quantityId).FirstOrDefault();
                                            discontinueOrder.PatientId = (int)entity.Patient_Id;
                                            discontinueOrder.DAdminId = dAdministerId == null ? 0 : dAdministerId.DAdmin_Id;
                                            discontinueOrder.OrderId = orderId;
                                            discontinueOrder.QuantityId = quantityId;
                                            discontinueOrder.POrderCreatedBy = entity.POrder_CreatedBy;
                                            discontinueOrder.POrderStatus = 2;
                                            discontinueOrder.POOutBoundApproval = 0;
                                            discontinueOrder.POOutBoundApprovalBy = 0;
                                            discontinueOrder.OrderType = "Discontinue";
                                            discontinueOrder.DiscontinueFlag = 1;
                                            discontinueOrder.DiscontinueReason = "";
                                            discontinueOrder.DiscontinueAllSplits = entity.Sig2Quantity!=null && entity.Sig2Quantity !=""? 1: 0;
                                            discontinueOrder.DiscontinuedOn = entity.POrder_CreatedDate;
                                            discontinueOrder.Split = entity.Sig2Quantity != null && entity.Sig2Quantity != "" ? 1 : 0;
                                            this.DiscontinueOrder(discontinueOrder);
                                        }
                                        //insert Schedule Text
                                        var quantityID = this.dbContext.QuantityDetails.Where(q => q.POrder_Id == orderId).Select(q => q.PQuantity_Id).FirstOrDefault();
                                        this._ordersRepository.InsertScheduleTimeText(Convert.ToInt32(orderId), Convert.ToInt32(quantityID), entity.ScheduleText);

                                    }
                                }
                                return result;
                            }
                            catch (Exception ex)
                            {
                                this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                                transaction.Rollback();
                                File.Delete(filePath);
                                this._log.Debug("---Delete file if generated " + filePath + " ----");
                                this._log.Debug("---Exception is: " + ex.Message + " ----");
                                throw ex;
                            }
                        }
                    }
                }
            }

        }
        public async Task<int> PatientReAdmission(int patientId, DateTime AdmitDate)
        {
            this._log.Debug("---Executing PatientReAdmission() in AdminApprovalService----");
            //if (this._companyRepository.GetApprovalFlagByFacilityId((int)residentInfo.VisitInfoDetails.FacilityId) == 1)
            //{
            //    return await Task.FromResult<int>(this._adminApprovalRepository.InsertNewDemographicDetails(residentInfo,1));
            //}
            if (patientId!= 0)
            {

                string filePath = string.Empty;
                using (EMAREntities context = new EMAREntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {

                            int result = await Task.FromResult<int>(this._adminApprovalRepository.PatientReAdmission(patientId, AdmitDate));
                            if (result != 0)
                            {
                                List<EventCategoryEntity> allowedEvents = this._companyRepository.GetCompanyEventCategoriesByPid((int)result).ToList();
                                if (allowedEvents != null && allowedEvents.ToList().Where(item => item.EventCat_Id == (int)OutboundEvents.AdmitVisitNotification).Count() != 0)
                                    filePath = GenerateOutboundFile((int)result, "NewResident", 0, context, transaction);
                                else
                                    transaction.Commit();
                            }
                            //if (result == 1)
                            //{
                            //    if (allowedEvents.Where(item => item.EventCat_Id == (int)OutboundEvents.UpdatePatientInformation).Count() != 0)
                            //        filePath = GenerateOutboundFile((int)demographics.Patient_Id, "Demographics", 0, context, transaction);
                            //    else
                            //        transaction.Commit();
                            //}
                            else
                            {
                                transaction.Commit();
                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            this._log.Debug("---Exception and Roll back transaction in AdminApprovalService----");
                            transaction.Rollback();
                            File.Delete(filePath);
                            this._log.Debug("---Delete file if generated " + filePath + " ----");
                            this._log.Debug("---Exception is: " + ex.Message + " ----");
                            throw ex;
                        }
                    }
                }
            }
            return 0;

        }
    }
}
