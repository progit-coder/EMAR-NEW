using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace LTCPro.Repositories
{
    public class OutboundFileInformationRepository : IOutboundFileInformationRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly ICommonRepository _commonRepository;
        enum Category { Allergy, Diagnosis, Demographics };
        public OutboundFileInformationRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, CommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._commonRepository = commonRepository;
            this._userActivityRepository = userActivityRepository;
        }

        public List<OutBoundFileInformationEntity> GetOutboundFiles(int companyId, string fileCategory, string fromDate, string toDate, string patientName)
        {
            List<OutBoundFileInformationEntity> savedFiles = new List<OutBoundFileInformationEntity>();
            var ConvertedFromDate = GetDateByComanyId(Convert.ToDateTime(fromDate.Substring(0, 10)), companyId,1);
            var ConvertedToDate = GetDateByComanyId(Convert.ToDateTime(toDate.Substring(0, 10)), companyId,2);
            var fileFromDate = Convert.ToDateTime(ConvertedFromDate);
            var fileToDate = Convert.ToDateTime(ConvertedToDate);
            if (patientName == "")
            {
                var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                              join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                              where ou.Company_Id == companyId && fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1
                              && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate)
                              select new //OutBoundFileInformationEntity
                              {
                                  outboundFiles = ou,
                                  //File_Id = ou.File_Id,
                                  //File_ErrorDesc = ou.File_Acknowledge==null?"Awaiting":((ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error") ,//(ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                  //Company_Id = ou.Company_Id,
                                  //File_Category = ou.File_Category,
                                  //File_CreatedBy = ou.File_CreatedBy,
                                  //File_CreatedDate = ou.File_CreatedDate,
                                  //File_Data = ou.File_Data,
                                  //File_Error = ou.File_Error,
                                  //File_Name = ou.File_Name,
                                  //File_Status = ou.File_Status,
                                  //Event = ou.Event

                              }).ToList();
                        savedFiles= displayRecords.OrderByDescending(f => f.outboundFiles.File_CreatedDate).ToList()
                              .Select(x => new OutBoundFileInformationEntity()
                              {
                                  File_Id = x.outboundFiles.File_Id,
                                  File_ErrorDesc = x.outboundFiles.File_Acknowledge == null && x.outboundFiles.File_Data.Length >1 ? "Awaiting" : ((x.outboundFiles.File_Error == "0" || x.outboundFiles.File_Error == "") ? "Success" : "Error"),//(ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                  Company_Id = x.outboundFiles.Company_Id,
                                  File_Category = x.outboundFiles.File_Category,
                                  File_CreatedBy = x.outboundFiles.File_CreatedBy,
                                  File_CreatedDate = x.outboundFiles.File_CreatedDate,//GetTimeZoneDateTime(x.outboundFiles.File_CreatedDate,x.outboundFiles.Company_Id),
                                  File_Data = x.outboundFiles.File_Data,
                                  File_Error = x.outboundFiles.File_Error,
                                  File_Name = x.outboundFiles.File_Name,
                                  File_Status = x.outboundFiles.File_Status,
                                  Event = x.outboundFiles.Event,
                                  ETConvertedFile_CreatedDate= GetDateByComanyId(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id,0)
                              }).ToList();
                
            }
            else
            {
                var residentName = patientName.Replace(", ", "^").Replace("|",".");
                var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate) 
                                      && ou.File_Data.Contains(residentName)
                                      select new //OutBoundFileInformationEntity
                                      {
                                          outboundFiles = ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = ou.File_Acknowledge==null?"Awaiting":((ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error") ,//(ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
        savedFiles = displayRecords.OrderByDescending(f => f.outboundFiles.File_CreatedDate).ToList()
                            .Select(x => new OutBoundFileInformationEntity()
                              {
                                  File_Id = x.outboundFiles.File_Id,
                                  File_ErrorDesc = x.outboundFiles.File_Acknowledge == null && x.outboundFiles.File_Data.Length > 1 ? "Awaiting" : ((x.outboundFiles.File_Error == "0" || x.outboundFiles.File_Error == "") ? "Success" : "Error"),//(ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                  Company_Id = x.outboundFiles.Company_Id,
                                  File_Category = x.outboundFiles.File_Category,
                                  File_CreatedBy = x.outboundFiles.File_CreatedBy,
                                  File_CreatedDate = x.outboundFiles.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id),
                                  File_Data = x.outboundFiles.File_Data,
                                  File_Error = x.outboundFiles.File_Error,
                                  File_Name = x.outboundFiles.File_Name,
                                  File_Status = x.outboundFiles.File_Status,
                                  Event = x.outboundFiles.Event,
                                  ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id, 0)
                            }).ToList();

            }
            return savedFiles;
        }

        public int InsertFileInformation(OutBoundFileInformationEntity entity, int patientId,EMAREntities context)
        {
            var record = this.autoMapper.Map<OutBoundFileInformationEntity, OutBoundFileInformation>(entity);
            var visitInfoData = context.VisitInfoes.Where(dm => dm.Patient_Id == patientId);

                if (visitInfoData.Count() > 1)
                {
                    var notDischargedRecord = visitInfoData.Where(v => v.DischargeDate == null);
                    if (notDischargedRecord.Count() == 0)
                        visitInfoData = visitInfoData.OrderByDescending(v => v.DischargeDate);
                    else
                        visitInfoData = notDischargedRecord;
                }
                int facilityId = visitInfoData.FirstOrDefault().FacilityId != null ? (int)visitInfoData.FirstOrDefault().FacilityId : 0;
                if (facilityId != 0)
                {
                    int companyId = context.Facilities.Where(p => p.Facility_Id == facilityId).Select(p => p.Company_Id).FirstOrDefault();
                    record.Company_Id = companyId;
                }
                context.OutBoundFileInformations.Add(record);
                context.SaveChanges();
                return record.File_Id;
        }
        public FileInformationViewCustomEntity GetFileAckDataForOutbound(int FileId)
        {
            var fileData = this.dbContext.OutBoundFileInformations.Where(fi => fi.File_Id == FileId).FirstOrDefault();

            FileInformationViewCustomEntity fileobj1 = new FileInformationViewCustomEntity();
            fileobj1.FileData = fileData.File_Data!=null?(fileData.File_Data.ToString()):"";
            if (fileData.File_Acknowledge != null)
                fileobj1.FileAckData = fileData.File_Acknowledge.Replace("\v", string.Empty).Replace("\r\u001c\r", string.Empty).Replace("\u001c\r\n", string.Empty).Replace("\u001c\r", string.Empty);
            else
                fileData.File_Acknowledge = "";
            return fileobj1;
        }
        public OutBoundFileInformationEntity GetOutboundFileById(int fileId)
        {
            var fileEntity = this.dbContext.OutBoundFileInformations.Where(f => f.File_Id == fileId).FirstOrDefault();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Outbound,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return this.autoMapper.Map<OutBoundFileInformation, OutBoundFileInformationEntity>(fileEntity);
        }
        //public int InsertRespectedTable(OutboundRecordsCustomEntity record, EMAREntities context)
        //{
        //    if (record.Category == "Allergy")
        //    {
        //        AllergyInfo entity = context.AllergyInfoes.Find(record.RecordId); ;
        //        if (entity.Patient_Id == record.Patient_Id)
        //        {
        //            entity.PAOutBoundApproval = 0;
        //            entity.PAOutBoundApprovalBy = 0;
        //            entity.PAOutBoundFileStatus = 0;
        //            entity.PAOutBoundApprovalOn = null;
        //            return 1;
        //        }
        //    }
        //    else if (record.Category == "Diagnosis")
        //    {
        //        DiagnosisInfo entity = context.DiagnosisInfoes.Find(record.RecordId); ;
        //        if (entity.Patient_Id == record.Patient_Id)
        //        {
        //            entity.PDGOutBoundApproval = 0;
        //            entity.PDGOutBoundApprovalBy = 0;
        //            entity.PDGOutBoundFileStatus = 0;
        //            entity.PDGOutBoundApprovalOn = null;
        //            return 1;
        //        }
        //    }
        //    else if (record.Category == "Demographics")
        //    {
        //        Demographic entity = context.Demographics.Find(record.RecordId); ;
        //        if (entity.Patient_Id == record.Patient_Id)
        //        {
        //            entity.PDOutBoundApproval = 0;
        //            entity.PDOutBoundApprovalBy = 0;
        //            entity.PDOutBoundFileStatus = 0;
        //            entity.PDOutBoundApprovalOn = null;
        //            return 1;
        //        }
        //    }
        //    else if (record.Category == "Transfer")
        //    {
        //        VisitInfo entity = context.VisitInfoes.Where(p=>p.Patient_Id == record.RecordId).FirstOrDefault();
        //        if (entity.Patient_Id == record.Patient_Id)
        //        {
        //            entity.PVOutBoundApproval = 0;
        //            entity.PVOutBoundApprovalBy = 0;
        //            entity.PVOutBoundFileStatus = 0;
        //            entity.PVOutBoundApprovalOn = null;
        //            return 1;
        //        }
        //    }
        //    else if (record.Category == "Discharge")
        //    {
        //        VisitInfo entity = context.VisitInfoes.Where(p => p.Patient_Id == record.RecordId).FirstOrDefault();
        //        if (entity.Patient_Id == record.Patient_Id)
        //        {
        //            entity.PVOutBoundApproval = 0;
        //            entity.PVOutBoundApprovalBy = 0;
        //            entity.PVOutBoundFileStatus = 0;
        //            entity.PVOutBoundApprovalOn = null;
        //            this.dbContext.SaveChanges();
        //            return 1;
        //        }
        //    }
        //    return 0;
        //}
        //public int InsertOutboundFileData(OutboundRecordsCustomEntity record, EMAREntities context)
        //{
        //    string fileData = string.Empty;
        //    if (record.Category == "Allergy" || record.Category == "Diagnosis" || record.Category == "Demographics")
        //    {
        //        fileData = context.PrcGenerateHL7forPatientUpdate(record.Patient_Id, record.Category).FirstOrDefault();
        //        InsertRespectedTable(record, context);
        //    }
        //    else if (record.Category == "Transfer")
        //    {
        //        fileData = context.prcGenerateHL7forPatientTransfer(record.Patient_Id).FirstOrDefault();
        //        InsertRespectedTable(record, context);
        //    }
        //    else if (record.Category == "Discharge")
        //    {
        //        fileData = context.prcGenerateHL7forPatientDischarge(record.Patient_Id).FirstOrDefault();
        //        InsertRespectedTable(record, context);
        //    }
        //    if (fileData != string.Empty)
        //    {
        //        //insert into outboundfileinformation and update respected table
        //        OutBoundFileInformationEntity entity = new OutBoundFileInformationEntity();
        //        entity.File_Name = record.Category + ".txt";
        //        entity.Company_Id = 1;
        //        entity.File_Data = fileData;
        //        entity.File_Category = 2;
        //        entity.Event = record.Category;//ToDo - get category from [EventCategory]
        //        entity.File_CreatedDate = DateTime.Now;
        //        entity.File_Status = 1;
        //        entity.File_Error = string.Empty;
        //        return InsertFileInformation(entity);
        //    }
        //    return 0;
        //}
        //public int GenerateFileSaveData(List<OutboundRecordsCustomEntity> records)
        //{
        //    var patientIds = records.Distinct().Select(p => p.Patient_Id);
        //    int result = 0;
        //    foreach (int id in patientIds)
        //    {
        //        List<OutboundRecordsCustomEntity> record = records.Where(p => p.Patient_Id == id).ToList();
        //        if (record.Count() == 1)
        //        {
        //            using (EMAREntities context = new EMAREntities())
        //            {
        //                using (DbContextTransaction transaction = context.Database.BeginTransaction())
        //                {
        //                    try
        //                    {
        //                        result = InsertOutboundFileData(record.Single(), context);
        //                        context.SaveChanges();
        //                        transaction.Commit();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        transaction.Rollback();
        //                        throw ex;
        //                    }
        //                }
        //            }
        //        }
        //        else if (record.Count() > 1)
        //        {
        //            var keywords = record.GroupBy(w => w.CategoryId).
        //                Select(g => new
        //                {
        //                    category = g.Key,
        //                    RecordIds = g.Select(c => c.RecordId)
        //                });
        //            foreach (var data in keywords)
        //            {
        //                if (data.category == 1)
        //                {
        //                    foreach (int recordId in data.RecordIds)
        //                    {
        //                        var item = record.Where(p => p.RecordId == recordId).Single();
        //                        using (EMAREntities context = new EMAREntities())
        //                        {
        //                            using (DbContextTransaction transaction = context.Database.BeginTransaction())
        //                            {
        //                                try
        //                                {
        //                                    result = InsertOutboundFileData(item, context);
        //                                    context.SaveChanges();
        //                                    transaction.Commit();
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    transaction.Rollback();
        //                                    throw ex;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var item = record.Where(p => p.RecordId == data.RecordIds.Single()).Single();
        //                    using (EMAREntities context = new EMAREntities())
        //                    {
        //                        using (DbContextTransaction transaction = context.Database.BeginTransaction())
        //                        {
        //                            try
        //                            {
        //                                result = InsertOutboundFileData(item, context);
        //                                context.SaveChanges();
        //                                transaction.Commit();
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                transaction.Rollback();
        //                                throw ex;
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
        public List<OutBoundFileInformationEntity> GetOutboundFilesStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName)
        {
            var ConvertedFromDate = GetDateByComanyId(Convert.ToDateTime(fromDate.Substring(0, 10)), companyId,1);
            var ConvertedToDate = GetDateByComanyId(Convert.ToDateTime(toDate.Substring(0, 10)), companyId,2);
            var fileFromDate = Convert.ToDateTime(ConvertedFromDate);
            var fileToDate = Convert.ToDateTime(ConvertedToDate);
            if (patientName == "")
            {
                if (fileStatus == 2)
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                          join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                          where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1
                                          && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate)
                                          select new //OutBoundFileInformationEntity
                                          {
                                              outboundFile = ou,
                                              //File_Id = ou.File_Id,
                                              //File_ErrorDesc = (ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                              //Company_Id = ou.Company_Id,
                                              //File_Category = ou.File_Category,
                                              //File_CreatedBy = ou.File_CreatedBy,
                                              //File_CreatedDate = ou.File_CreatedDate,
                                              //File_Data = ou.File_Data,
                                              //File_Error = ou.File_Error,
                                              //File_Name = ou.File_Name,
                                              //File_Status = ou.File_Status,
                                              //Event = ou.Event

                                          }).ToList();
                    var savedFiles = displayRecords.OrderByDescending(f => f.outboundFile.File_CreatedDate).ToList()
                                      .Select(x => new OutBoundFileInformationEntity
                                      {
                                          File_Id = x.outboundFile.File_Id,
                                          File_ErrorDesc = (x.outboundFile.File_Error == "0" || x.outboundFile.File_Error == "") ? "Success" : "Error",
                                          Company_Id = x.outboundFile.Company_Id,
                                          File_Category = x.outboundFile.File_Category,
                                          File_CreatedBy = x.outboundFile.File_CreatedBy,
                                          File_CreatedDate = x.outboundFile.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFile.File_CreatedDate,x.outboundFile.Company_Id),
                                          File_Data = x.outboundFile.File_Data,
                                          File_Error = x.outboundFile.File_Error,
                                          File_Name = x.outboundFile.File_Name,
                                          File_Status = x.outboundFile.File_Status,
                                          Event = x.outboundFile.Event,
                                          ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFile.File_CreatedDate, x.outboundFile.Company_Id, 0)
                                      }).ToList();
                    return savedFiles;
                }
                else if (fileStatus == 0)
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1 && (ou.File_Acknowledge != null && ou.File_Error == "0" || ou.File_Acknowledge != null && ou.File_Error == "")
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate)
                                      select new //OutBoundFileInformationEntity 
                                      {
                                          outboundFile = ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = "Success",//(ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
                  var  savedFiles = displayRecords.OrderByDescending(f => f.outboundFile.File_CreatedDate).ToList()
                           .Select(x => new OutBoundFileInformationEntity
                                      {
                                          File_Id = x.outboundFile.File_Id,
                                          File_ErrorDesc = "Success",//(ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          Company_Id = x.outboundFile.Company_Id,
                                          File_Category = x.outboundFile.File_Category,
                                          File_CreatedBy = x.outboundFile.File_CreatedBy,
                                          File_CreatedDate = x.outboundFile.File_CreatedDate,//GetTimeZoneDateTime(x.outboundFile.File_CreatedDate,x.outboundFile.Company_Id),
                                          File_Data = x.outboundFile.File_Data,
                                          File_Error = x.outboundFile.File_Error,
                                          File_Name = x.outboundFile.File_Name,
                                          File_Status = x.outboundFile.File_Status,
                                          Event = x.outboundFile.Event,
                                          ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFile.File_CreatedDate, x.outboundFile.Company_Id, 0)
                           }).ToList();
                    return savedFiles;
                }
                else if (fileStatus == 1)
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1 && ou.File_Acknowledge != null && ou.File_Error != "" && ou.File_Error != "0" 
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate)
                                      select new //OutBoundFileInformationEntity
                                      {
                                          outboundFile=ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = (ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
                    var savedFiles = displayRecords.OrderByDescending(f => f.outboundFile.File_CreatedDate).ToList()
                           .Select(x => new OutBoundFileInformationEntity
                           {
                               File_Id = x.outboundFile.File_Id,
                               File_ErrorDesc = (x.outboundFile.File_Error == "0" || x.outboundFile.File_Error == "") ? "Success" : "Error",
                               Company_Id = x.outboundFile.Company_Id,
                               File_Category = x.outboundFile.File_Category,
                               File_CreatedBy = x.outboundFile.File_CreatedBy,
                               File_CreatedDate = x.outboundFile.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFile.File_CreatedDate, x.outboundFile.Company_Id),
                               File_Data = x.outboundFile.File_Data,
                               File_Error = x.outboundFile.File_Error,
                               File_Name = x.outboundFile.File_Name,
                               File_Status = x.outboundFile.File_Status,
                               Event = x.outboundFile.Event,
                               ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFile.File_CreatedDate, x.outboundFile.Company_Id, 0)
                           }).ToList();
                    return savedFiles;
                }
                else
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1 && ou.File_Acknowledge == null 
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate)
                                      select new //OutBoundFileInformationEntity
                                      {
                                          outboundFiles=ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = (ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
                    var savedFiles = displayRecords.OrderByDescending(f => f.outboundFiles.File_CreatedDate).ToList()
                                        .Select(x => new OutBoundFileInformationEntity
                                        {
                                            File_Id = x.outboundFiles.File_Id,
                                            File_ErrorDesc = (x.outboundFiles.File_Error == "0" || x.outboundFiles.File_Error == "") ? "Success" : "Error",
                                            Company_Id = x.outboundFiles.Company_Id,
                                            File_Category = x.outboundFiles.File_Category,
                                            File_CreatedBy = x.outboundFiles.File_CreatedBy,
                                            File_CreatedDate = x.outboundFiles.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id),
                                            File_Data = x.outboundFiles.File_Data,
                                            File_Error = x.outboundFiles.File_Error,
                                            File_Name = x.outboundFiles.File_Name,
                                            File_Status = x.outboundFiles.File_Status,
                                            Event = x.outboundFiles.Event,
                                            ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id, 0)
                                        }).ToList();
                    return savedFiles;
                }
            }
            else
            {
                var residentName = patientName.Replace(", ", "^");
                if (fileStatus == 2)
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id 
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1 
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate) 
                                      && ou.File_Data.Contains(residentName)
                                      select new //OutBoundFileInformationEntity
                                      {
                                          outboundFiles=ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = (ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
                    var savedFiles = displayRecords.OrderByDescending(f => f.outboundFiles.File_CreatedDate).ToList()
                                        .Select(x => new OutBoundFileInformationEntity
                                        {
                                            File_Id = x.outboundFiles.File_Id,
                                            File_ErrorDesc = (x.outboundFiles.File_Error == "0" || x.outboundFiles.File_Error == "") ? "Success" : "Error",
                                            Company_Id = x.outboundFiles.Company_Id,
                                            File_Category = x.outboundFiles.File_Category,
                                            File_CreatedBy = x.outboundFiles.File_CreatedBy,
                                            File_CreatedDate = x.outboundFiles.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id),
                                            File_Data = x.outboundFiles.File_Data,
                                            File_Error = x.outboundFiles.File_Error,
                                            File_Name = x.outboundFiles.File_Name,
                                            File_Status = x.outboundFiles.File_Status,
                                            Event = x.outboundFiles.Event,
                                            ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id, 0)
                                        }).ToList();
                    return savedFiles;
                }
                else if (fileStatus == 0)
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1 && (ou.File_Error == fileStatus.ToString() || ou.File_Error == "") 
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate) 
                                      && ou.File_Data.Contains(residentName)
                                      select new //OutBoundFileInformationEntity
                                      {
                                          outboundFiles=ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = (ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
                   var savedFiles = displayRecords.OrderByDescending(f => f.outboundFiles.File_CreatedDate).ToList()
                                    .Select(x => new OutBoundFileInformationEntity
                                        {
                                            File_Id = x.outboundFiles.File_Id,
                                            File_ErrorDesc = (x.outboundFiles.File_Error == "0" || x.outboundFiles.File_Error == "") ? "Success" : "Error",
                                            Company_Id = x.outboundFiles.Company_Id,
                                            File_Category = x.outboundFiles.File_Category,
                                            File_CreatedBy = x.outboundFiles.File_CreatedBy,
                                            File_CreatedDate =x.outboundFiles.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id),
                                            File_Data = x.outboundFiles.File_Data,
                                            File_Error = x.outboundFiles.File_Error,
                                            File_Name = x.outboundFiles.File_Name,
                                            File_Status = x.outboundFiles.File_Status,
                                            Event = x.outboundFiles.Event,
                                            ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id, 0)
                                    }).ToList(); 
                   return savedFiles;
                }
                else
                {
                    var displayRecords = (from ou in this.dbContext.OutBoundFileInformations
                                      join fc in this.dbContext.FTECategories on ou.File_Category equals fc.FteCategory_Id
                                      where ou.Company_Id == companyId && fc.FteCategory_Desc == "OutBound" && fc.FteCategory_Status == 1 && ou.File_Error == fileStatus.ToString() 
                                      && (ou.File_CreatedDate >= fileFromDate && ou.File_CreatedDate <= fileToDate) 
                                      && ou.File_Data.Contains(residentName)
                                      select new //OutBoundFileInformationEntity
                                      {
                                          outboundFiles=ou,
                                          //File_Id = ou.File_Id,
                                          //File_ErrorDesc = (ou.File_Error == "0" || ou.File_Error == "") ? "Success" : "Error",
                                          //Company_Id = ou.Company_Id,
                                          //File_Category = ou.File_Category,
                                          //File_CreatedBy = ou.File_CreatedBy,
                                          //File_CreatedDate = ou.File_CreatedDate,
                                          //File_Data = ou.File_Data,
                                          //File_Error = ou.File_Error,
                                          //File_Name = ou.File_Name,
                                          //File_Status = ou.File_Status,
                                          //Event = ou.Event

                                      }).ToList();
                  var  savedFiles = displayRecords.OrderByDescending(f => f.outboundFiles.File_CreatedDate).ToList()
                                        .Select(x => new OutBoundFileInformationEntity
                                        {
                                            File_Id = x.outboundFiles.File_Id,
                                            File_ErrorDesc = (x.outboundFiles.File_Error == "0" || x.outboundFiles.File_Error == "") ? "Success" : "Error",
                                            Company_Id = x.outboundFiles.Company_Id,
                                            File_Category = x.outboundFiles.File_Category,
                                            File_CreatedBy = x.outboundFiles.File_CreatedBy,
                                            File_CreatedDate = x.outboundFiles.File_CreatedDate, //GetTimeZoneDateTime(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id),
                                            File_Data = x.outboundFiles.File_Data,
                                            File_Error = x.outboundFiles.File_Error,
                                            File_Name = x.outboundFiles.File_Name,
                                            File_Status = x.outboundFiles.File_Status,
                                            Event = x.outboundFiles.Event,
                                            ETConvertedFile_CreatedDate = GetDateByComanyId(x.outboundFiles.File_CreatedDate, x.outboundFiles.Company_Id, 0)
                                        }).ToList();

                    return savedFiles;
                }
            }
        }
        public List<ResidentDropEntity> GetOutboundResidentsDrop(int userId, int companyId)
        {
            //insert User Recent Company
            this._commonRepository.insertUserRecentCompany(userId, companyId);
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> userFacilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> facilityIds = this.dbContext.Facilities.Where(fa => userFacilityIds.Contains(fa.Facility_Id) && fa.Company_Id == companyId).Select(fa => fa.Facility_Id).Distinct().ToList();
            List<int> userNstationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();
            List<int> nstations = this.dbContext.NursingStations.Where(ns => facilityIds.Contains((int)ns.Facility_Id) && userNstationIds.Contains(ns.NurseStation_Id)).Select(ns => ns.NurseStation_Id).Distinct().ToList();


            //var q1 = this.dbContext.ApprovalVisitInfoes.Where(pi => pi.PVOutBoundApproval == 1).Select(pi => pi.Patient_Id).Distinct();
            //var q2 = this.dbContext.ApprovalDemographics.Where(pi => pi.PDOutBoundApproval == 1).Select(pi => pi.Patient_Id).Distinct();
            //var q3 = this.dbContext.ApprovalDiagnosisInfoes.Where(pi => pi.PDGOutBoundApproval==1).Select(pi => pi.Patient_Id).Distinct();
            //var q4 = this.dbContext.ApprovalAllergyInfoes.Where(pi => pi.PAOutBoundApproval==1).Select(pi => pi.Patient_Id).Distinct();
            //var q5 = this.dbContext.ApprovalOrders.Where(pi => pi.POOutBoundApproval == 1).Select(pi => pi.Patient_Id).Distinct();
            //var q6 = this.dbContext.ApprovalRefills.Where(pi => pi.POOutBoundApproval == 1).Select(pi => pi.Patient_Id).Distinct();
            //var q7 = this.dbContext.VisitInfoes.Where(vi => facilityIds.Contains((int)vi.FacilityId) && nstations.Contains((int)vi.NursingStationId) && (q1.Contains(vi.Patient_Id) || q2.Contains(vi.Patient_Id) || q3.Contains(vi.Patient_Id) || q4.Contains(vi.Patient_Id) || q5.Contains(vi.Patient_Id) || q6.Contains(vi.Patient_Id) || vi.File_Id == null)).Select(vi => vi.Patient_Id).Distinct();
            var orderList = this.dbContext.OutBoundFileInformations.Where(o => o.Porder_Id != null && o.Company_Id==companyId).Select(o => o.Porder_Id).Distinct();
            var q1 = this.dbContext.CommonOrderInfoes.Where(com => orderList.Contains(com.POrder_Id)).Select(com => com.Patient_Id).Distinct();
            var q2 = this.dbContext.OutBoundFileInformations.Where(o => o.Patient_Id != null && o.Company_Id == companyId).Select(o => o.Patient_Id).Distinct();
            var q3 = this.dbContext.Demographics.Where(de => q2.Contains(de.Patient_Id) || q1.Contains(de.Patient_Id))
                .Select(de => new ResidentDropEntity()
                {
                    Patient_Id = de.Patient_Id,
                    PatientName = de.PatientLastName + ", " + de.PatientFirstName,
                }).Distinct().OrderBy(item => item.PatientName).ToList();

            return q3;
        }
        public Nullable<DateTime> GetTimeZoneDateTime(DateTime? dateTime, int? companyId)
        {
            var convetedDate = (this.dbContext.GetTimeZoneConvertedDateTime(dateTime, companyId).FirstOrDefault());
            return convetedDate;
        }
        public Nullable<DateTime> GetDateByComanyId(DateTime? dateTime, int? companyId, int type)
        {
            DateTime convetedDate = Convert.ToDateTime(this.dbContext.GetFileTimeZoneConvertedDateTime(dateTime, companyId, type).FirstOrDefault());
            return convetedDate;
        }
    }
}