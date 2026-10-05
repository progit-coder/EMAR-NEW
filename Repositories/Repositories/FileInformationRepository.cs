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
    public class FileInformationRepository : IFileInformationRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly ICommonRepository _commonRepository;
        public FileInformationRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, CommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this._commonRepository = commonRepository;
            this.dbContext = dbContext;
        }

        public List<FileInformationEntity> GetInboundOutboundFiles(int companyId, string fileCategory)
        {
            var savedFiles = (from inb in this.dbContext.FileInformations
                              join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                              //join co in this.dbContext.CommonOrderInfoes on inb.File_Id equals co.File_Id into c
                              //from com in c.DefaultIfEmpty()
                              //join vi in this.dbContext.VisitInfoes on inb.File_Id equals vi.File_Id into v
                              //from visit in v.DefaultIfEmpty()
                              //join cd in this.dbContext.Demographics on com.Patient_Id equals cd.Patient_Id into cod
                              //from comde in cod.DefaultIfEmpty()
                              //join vd in this.dbContext.Demographics on visit.Patient_Id equals vd.Patient_Id into vsd
                              //from visitde in vsd.DefaultIfEmpty()
                              where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                              select new FileInformationEntity
                              {
                                  File_Id = inb.File_Id,
                                  File_Category = inb.File_Category,
                                  File_Data = inb.File_Data,
                                  File_Error = inb.File_Error,
                                  File_Name = inb.File_Name,
                                  File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                                  File_Status = inb.File_Status,
                                  File_CreatedBy = inb.File_CreatedBy,
                                  File_CreatedDate = inb.File_CreatedDate,
                                  Company_Id = inb.Company_Id,
                                  Event = inb.Event,
                                  //PatientId = comde != null || visitde != null ? comde != null ? comde.Patient_Id : visitde.Patient_Id : 0,
                                  //PatientName = comde != null || visitde != null ? comde != null ? comde.PatientLastName + " " + comde.PatientFirstName : visitde.PatientLastName + " " + comde.PatientFirstName : "",
                              }).OrderByDescending(f => f.File_CreatedDate).ToList();

            return savedFiles;
        }

        public int InsertFileInformation(FileInformationEntity entity)
        {
            var record = this.autoMapper.Map<FileInformationEntity, FileInformation>(entity);
            this.dbContext.FileInformations.Add(record);
            this.dbContext.SaveChanges();
            return 1;

        }
        public List<HlFieldsEntity> GetHlFieldsInformation(int FileID)
        {
            List<HlFieldsEntity> records = (from hc in this.dbContext.HLSevenCompanyConfigs
                                            join hd in this.dbContext.HLSevenSegmentDetails on hc.SegDetail_Id equals hd.SegDetail_Id
                                            join seg in this.dbContext.HLSevenSegments on hd.Segment_Id equals seg.Segment_Id
                                            where hc.Company_Id == 1
                                            select new HlFieldsEntity { FieldName = hd.SegDetail_Desc, IsMandatory = hc.SegDetailConfig_Id, FieldValue = hd.File_Values }).ToList();
            return records;
        }
        public FileInformationEntity GetInboundOutboundFileById(int fileId)
        {
            var fileEntity = this.dbContext.FileInformations.Where(f => f.File_Id == fileId).FirstOrDefault();
            return this.autoMapper.Map<FileInformation, FileInformationEntity>(fileEntity);
        }
        public int SaveInboundFilesDownGridData(List<int> fileId, int userID)
        {
            string result = "";
            if (fileId.Count > 0)
            {
                fileId.ForEach(fl =>
                {
                    result = result + fl + ",";
                });
                result = result.Substring(0, result.Length - 1);
                using (EMAREntities context = new EMAREntities())
                {
                    int? r = context.PrcInsertDataAfterFileUpload(result, userID).FirstOrDefault();
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public FileInformationViewCustomEntity GetFileAckData(int FileId)
        {

            List<string> fileError = new List<string>();
            var fileInfoData = this.dbContext.FileInformations.Where(fi => fi.File_Id == FileId).FirstOrDefault();
            var fileAckData = this.dbContext.FileAckInformations.Where(fa => fa.File_Id == FileId).FirstOrDefault();
            if (fileInfoData.File_Error == "1")
            {
                var fileInfo = this.dbContext.FileInfoErrors.Where(fi => fi.File_Id == FileId).Select(fi => fi.Error_Desc).ToList();
                var tempSegment = this.dbContext.tmptblSegmentChecks.Where(tm => tm.FileId == FileId).Select(tm => tm.SegmentCode + " - " + tm.SegmentDesc + " - " + tm.SegmentValue).ToList();
                if (fileInfo.Count > 0)
                {
                    fileError = fileInfo;
                }
                else if (tempSegment.Count > 0)
                {
                    fileError = tempSegment;
                }
                else
                {
                    fileError = null;
                }

            }
            FileInformationViewCustomEntity fileObj = new FileInformationViewCustomEntity();
            fileObj.FileData = fileInfoData.File_Data.Replace("\n", string.Empty).Replace("\r\u001c", string.Empty);
            if (fileAckData != null)
                fileObj.FileAckData = fileAckData.FileAck_Data.Replace("\v", string.Empty).Replace("\u001c\r\n", string.Empty).Replace("\u001c\r", string.Empty);
            else
                fileObj.FileAckData = "";
            fileObj.FileError = fileError;
            return fileObj;
        }
        public InboundDashBoardDisplay GetInboundDashboard()
        {
            var dates = this.dbContext.FileInformations.Select(p => DbFunctions.TruncateTime(p.File_CreatedDate)).Distinct();

            var companyIds = this.dbContext.FileInformations.Select(p => p.Company_Id).Distinct();
            var gby = this.dbContext.FileInformations.GroupBy(e => new { e.Company_Id, CreatedDate = DbFunctions.TruncateTime(e.File_CreatedDate) })
                .Select(e => new
                {
                    CompanyId = e.Key.Company_Id,
                    CreatedDate = e.Key.CreatedDate,
                    Count = e.Count()
                })
                .ToList();
            List<InboundDashBoardCompanyCountDate> clist = new List<InboundDashBoardCompanyCountDate>();
            InboundDashBoardCompanyCountDate c1;
            foreach (var date in dates)
            {
                foreach (var cId in companyIds)
                {
                    c1 = new InboundDashBoardCompanyCountDate();
                    c1.data = gby.Where(e => e.CreatedDate == date && e.CompanyId == cId).Select(e => e.Count).FirstOrDefault();
                    c1.name = cId.ToString();
                    c1.CreatedDate = ((DateTime)date).ToString("MM/dd/yyyy");
                    //this.dbContext.FileInformations.Where(e => DbFunctions.TruncateTime(e.File_CreatedDate) == date).Select(e => e.File_Id).ToList().Count();
                    clist.Add(c1);
                }
            }
            List<InboundDashBoard> recordsList = new List<InboundDashBoard>();

            InboundDashBoard returnData;
            foreach (var cId in companyIds)
            {
                returnData = new InboundDashBoard();
                returnData.name = cId.ToString();
                returnData.data = clist.Where(e => e.name == cId.ToString()).Select(e => e.data).ToList();
                recordsList.Add(returnData);
            }

            InboundDashBoardDisplay displayData = new InboundDashBoardDisplay();
            displayData.xaxisdata = clist.Select(r => r.CreatedDate).ToList();
            displayData.yaxisdata = recordsList;
            return displayData;


            /*
            List<InboundDashBoardData> list = new List<InboundDashBoardData>();
            InboundDashBoardData entity;
            foreach (var date in dates)
            {
                entity = new InboundDashBoardData();
                entity.SuccessRecords = this.dbContext.FileInformations.Where(e => DbFunctions.TruncateTime(e.File_CreatedDate) == date && e.File_Error == "0").Select(e => e.File_Id).ToList().Count();
                entity.ErrorRecords = this.dbContext.FileInformations.Where(e => DbFunctions.TruncateTime(e.File_CreatedDate) == date && e.File_Error == "1").Select(e => e.File_Id).ToList().Count();
                entity.CreatedDate = ((DateTime)date).ToString("MM/dd/yyyy");
                list.Add(entity);
            }

            List<int> successCount = list.Select(r => r.SuccessRecords).ToList();
            List<int> errorRecords = list.Select(r => r.ErrorRecords).ToList();

            List<string> recordsDate = list.Select(r => r.CreatedDate).ToList();

            List<InboundDashBoard> recordsList = new List<InboundDashBoard>();

            InboundDashBoard returnData = new InboundDashBoard() { name = "Success Records", data = successCount };
            recordsList.Add(returnData);
            returnData = new InboundDashBoard() { name = "Error Records", data = errorRecords };
            recordsList.Add(returnData);

            InboundDashBoardDisplay displayData = new InboundDashBoardDisplay();
            displayData.xaxisdata = recordsDate;
            displayData.yaxisdata = recordsList;
            return displayData;            
            */
        }

        public InboundDashBoardDisplay GetInboundDashboardPopup(int companyId, string createdDate)
        {
            DateTime cDate = Convert.ToDateTime(createdDate);
            InboundDashBoardData entity;
            entity = new InboundDashBoardData();
            entity.SuccessRecords = this.dbContext.FileInformations.Where(e => DbFunctions.TruncateTime(e.File_CreatedDate) == cDate && e.File_Error == "0" && e.Company_Id == companyId).Select(e => e.File_Id).ToList().Count();
            entity.ErrorRecords = this.dbContext.FileInformations.Where(e => DbFunctions.TruncateTime(e.File_CreatedDate) == cDate && e.File_Error == "1" && e.Company_Id == companyId).Select(e => e.File_Id).ToList().Count();
            entity.CreatedDate = createdDate;

            List<InboundDashBoard> recordsList = new List<InboundDashBoard>();

            InboundDashBoard returnData = new InboundDashBoard() { name = "Success Records", data = new List<int>() { entity.SuccessRecords } };
            recordsList.Add(returnData);
            returnData = new InboundDashBoard() { name = "Error Records", data = new List<int>() { entity.ErrorRecords } };
            recordsList.Add(returnData);

            InboundDashBoardDisplay displayData = new InboundDashBoardDisplay();
            displayData.xaxisdata = new List<string>() { "Success", "Error" };
            displayData.yaxisdata = recordsList;
            return displayData;
        }
        public List<FileInformationEntity> GetInboundFilesByStatus(int companyId, int fileStatus,string fromDate, string toDate, string patientName)
        {
            var fileFromDate = Convert.ToDateTime(fromDate.Substring(0, 10));
            var fileToDate = Convert.ToDateTime(toDate.Substring(0, 10));
            if (patientName == "")
            {
                if (fileStatus == 2)
                {
                    var savedFiles = (from inb in this.dbContext.FileInformations
                                      join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                      where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && inb.Company_Id == companyId && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                                      select new FileInformationEntity
                                      {
                                          File_Id = inb.File_Id,
                                          File_Category = inb.File_Category,
                                          File_Data = inb.File_Data,
                                          File_Error = inb.File_Error,
                                          File_Name = inb.File_Name,
                                          File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                                          File_Status = inb.File_Status,
                                          File_CreatedBy = inb.File_CreatedBy,
                                          File_CreatedDate = inb.File_CreatedDate,
                                          Company_Id = inb.Company_Id,
                                          Event = inb.Event
                                      }).OrderByDescending(f => f.File_CreatedDate).ToList();

                    return savedFiles;
                }
                else
                {
                    var savedFiles = (from inb in this.dbContext.FileInformations
                                      join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                      where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) && inb.Company_Id == companyId && inb.File_Error == fileStatus.ToString()
                                      select new FileInformationEntity
                                      {
                                          File_Id = inb.File_Id,
                                          File_Category = inb.File_Category,
                                          File_Data = inb.File_Data,
                                          File_Error = inb.File_Error,
                                          File_Name = inb.File_Name,
                                          File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                                          File_Status = inb.File_Status,
                                          File_CreatedBy = inb.File_CreatedBy,
                                          File_CreatedDate = inb.File_CreatedDate,
                                          Company_Id = inb.Company_Id,
                                          Event = inb.Event
                                      }).OrderByDescending(f => f.File_CreatedDate).ToList();

                    return savedFiles;
                }
            }
            else
            {
                var residentName = patientName.Replace(", ", "^");
                if (fileStatus == 2)
                {
                    var savedFiles = (from inb in this.dbContext.FileInformations
                                      join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                      where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && inb.Company_Id == companyId && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) && inb.File_Data.Contains(residentName)
                                      select new FileInformationEntity
                                      {
                                          File_Id = inb.File_Id,
                                          File_Category = inb.File_Category,
                                          File_Data = inb.File_Data,
                                          File_Error = inb.File_Error,
                                          File_Name = inb.File_Name,
                                          File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                                          File_Status = inb.File_Status,
                                          File_CreatedBy = inb.File_CreatedBy,
                                          File_CreatedDate = inb.File_CreatedDate,
                                          Company_Id = inb.Company_Id,
                                          Event = inb.Event
                                      }).OrderByDescending(f => f.File_CreatedDate).ToList();

                    return savedFiles;
                }
                else
                {
                    var savedFiles = (from inb in this.dbContext.FileInformations
                                      join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                      where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) && inb.File_Data.Contains(residentName) && inb.Company_Id == companyId && inb.File_Error == fileStatus.ToString()
                                      select new FileInformationEntity
                                      {
                                          File_Id = inb.File_Id,
                                          File_Category = inb.File_Category,
                                          File_Data = inb.File_Data,
                                          File_Error = inb.File_Error,
                                          File_Name = inb.File_Name,
                                          File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                                          File_Status = inb.File_Status,
                                          File_CreatedBy = inb.File_CreatedBy,
                                          File_CreatedDate = inb.File_CreatedDate,
                                          Company_Id = inb.Company_Id,
                                          Event = inb.Event
                                      }).OrderByDescending(f => f.File_CreatedDate).ToList();

                    return savedFiles;
                }
            }
        }
        public FilesCountEntity GetFilesCount(int companyId)
        {
            int totalCount = this.dbContext.FileInformations.Where(a => a.Company_Id == companyId).ToList().Count();
            int successCount = this.dbContext.FileInformations.Where(a => a.Company_Id == companyId && (a.File_Error == "0" || a.File_Error == "")).ToList().Count();
            int errorCount = this.dbContext.FileInformations.Where(a => a.Company_Id == companyId && a.File_Error == "1").ToList().Count();
            FilesCountEntity count = new FilesCountEntity();
            count.Total = totalCount;
            count.Success = successCount;
            count.Error = errorCount;
            return count;
        }
        public List<ResidentDropEntity> GetInboundResidentsDrop(int userId,int companyId)
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

            List<ResidentDropEntity> list = (from dm in this.dbContext.Demographics
                                             join vs in this.dbContext.VisitInfoes on dm.Patient_Id equals vs.Patient_Id
                                             where facilityIds.Contains((int)vs.FacilityId) && nstations.Contains((int)vs.NursingStationId) &&  vs.File_Id!=null 
                                             select new ResidentDropEntity
                                             {
                                                 Patient_Id = dm.Patient_Id,
                                                 PatientName = dm.PatientLastName + ", " + dm.PatientFirstName,
                                             }).OrderBy(item => item.PatientName).ToList();

            return list;
        }
        public InboundFilesGridEntity GetInboundFilesByResident(int companyId, string fileCategory, string fromDate, string toDate, int currentPage, int pageSize, string searchValue, string patientName = "", int status = 2)
        {
            var ConvertedFromDate = GetDateByComanyId(Convert.ToDateTime(fromDate.Substring(0, 10)), companyId,1);
            var ConvertedToDate = GetDateByComanyId(Convert.ToDateTime(toDate.Substring(0, 10)), companyId,2);
            var fileFromDate = Convert.ToDateTime(ConvertedFromDate);
            var fileToDate = Convert.ToDateTime(ConvertedToDate);
            int rejected = (from inb in this.dbContext.FileInformations
                         join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                         where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == null 
                         && inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <=fileToDate
                            select inb).Count();
            //int rejected = rejectedRecords.Where(cr => (GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) >= fileFromDate && GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) <= fileToDate)).Count();
            int total = (from inb in this.dbContext.FileInformations
                         join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                         where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId 
                         && inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate
                         select inb).Count();
            //int total = totalRecords.Where(cr => (GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) >= fileFromDate && GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) <= fileToDate)).Count();
            int Success = (from inb in this.dbContext.FileInformations
                           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId && inb.File_Error == "0" 
                           && inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate
                           select inb).Count();
            //int Success = SuccessRecords.Where(cr => (GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) >= fileFromDate && GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) <= fileToDate)).Count();
            int Error = (from inb in this.dbContext.FileInformations
                         join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                         where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId && inb.File_Error == "1" 
                         && inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate
                         select inb).Count();
            //int Error = ErrorRecords.Where(cr => (GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) >= fileFromDate && GetDateByComanyId(cr.File_CreatedDate, cr.Company_Id) <= fileToDate)).Count();
            if (status == 3)
            {
               var rejectedFiles = this.GetInboundRejectedFiles(fromDate, toDate, searchValue, currentPage, pageSize,companyId);
                return new InboundFilesGridEntity
                {
                    Reject = rejected,
                    Total = total,
                    Error = Error,
                    Success = Success,
                    TotalRecords = rejectedFiles.TotalRecords,
                    Data = rejectedFiles.Data
                };
            }
            else
            {
                //Without Patientname
                if (patientName == "")
                {
                    int count = 0;
                    searchValue = searchValue != "null" && searchValue != null && searchValue != string.Empty ? searchValue.ToLower() : string.Empty;
                    var records = new List<FileInformationEntity>();
                    //SearchValue empty
                    if (searchValue == string.Empty)
                    {
                        //All Records - search empty - without patient name
                        if (status == 2)
                        {
                            var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                                     && (inb.File_CreatedDate >= fileFromDate &&inb.File_CreatedDate <= fileToDate)
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var displayRecords = (from inb in this.dbContext.FileInformations
                            //                      join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //                      where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //                      && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                            //                      select new FileInformationEntity
                            //                      {
                            //                          //fileInfo = inb,
                            //                          File_Id = inb.File_Id,
                            //                          File_Category = inb.File_Category,
                            //                          File_Data = inb.File_Data,
                            //                          File_Error = inb.File_Error,
                            //                          File_Name = inb.File_Name,
                            //                          File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //                          File_Status = inb.File_Status,
                            //                          File_CreatedBy = inb.File_CreatedBy,
                            //                          File_CreatedDate = inb.File_CreatedDate,
                            //                          Company_Id = inb.Company_Id,
                            //                          Event = inb.Event,
                            //                      }).ToList();
                                   records= countRecords.OrderByDescending(li=>li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                       .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate =GetTimeZoneDateTime(x.File_CreatedDate,x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();

                        }
                        //status selected Records - search empty - without patient name
                        else
                        {
                            var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate)
                                     && inb.File_Error == status.ToString()
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var displayRecords = (from inb in this.dbContext.FileInformations
                            //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //           && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                            //           && inb.File_Error == status.ToString()
                            //           select new FileInformationEntity
                            //           {
                            //               //fileInfo = inb,
                            //               File_Id = inb.File_Id,
                            //               File_Category = inb.File_Category,
                            //               File_Data = inb.File_Data,
                            //               File_Error = inb.File_Error,
                            //               File_Name = inb.File_Name,
                            //               File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //               File_Status = inb.File_Status,
                            //               File_CreatedBy = inb.File_CreatedBy,
                            //               File_CreatedDate = inb.File_CreatedDate,
                            //               Company_Id = inb.Company_Id,
                            //               Event = inb.Event,
                            //           }).ToList();
                            records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                     .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                    }
                    //search value - without patient name
                    else
                    {
                        //All Records - search value - without patient name
                        if (status == 2)
                        {
                           var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate)
                                     && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var displayRecords = (from inb in this.dbContext.FileInformations
                            //                      join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //                      where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //                      && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                            //                      && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                            //                      select new FileInformationEntity
                            //                      {
                            //                          //fileInfo = inb,
                            //                          File_Id = inb.File_Id,
                            //                          File_Category = inb.File_Category,
                            //                          File_Data = inb.File_Data,
                            //                          File_Error = inb.File_Error,
                            //                          File_Name = inb.File_Name,
                            //                          File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //                          File_Status = inb.File_Status,
                            //                          File_CreatedBy = inb.File_CreatedBy,
                            //                          File_CreatedDate = inb.File_CreatedDate,
                            //                          Company_Id = inb.Company_Id,
                            //                          Event = inb.Event,
                            //                      }).ToList();
                            records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                       .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                        //selected status  Records - search value - without patient name
                        else
                        {
                          var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate)
                                     && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                                     && inb.File_Error == status.ToString()
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var displayRecords = (from inb in this.dbContext.FileInformations
                            //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //           && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                            //           && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                            //           && inb.File_Error == status.ToString()
                            //           select new FileInformationEntity
                            //           {
                            //               //fileInfo = inb,
                            //               File_Id = inb.File_Id,
                            //               File_Category = inb.File_Category,
                            //               File_Data = inb.File_Data,
                            //               File_Error = inb.File_Error,
                            //               File_Name = inb.File_Name,
                            //               File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //               File_Status = inb.File_Status,
                            //               File_CreatedBy = inb.File_CreatedBy,
                            //               File_CreatedDate = inb.File_CreatedDate,
                            //               Company_Id = inb.Company_Id,
                            //               Event = inb.Event,
                            //           }).ToList();
                            records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                            .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                    }
                    return new InboundFilesGridEntity
                    {
                        Reject = rejected,
                        Total = total,
                        Error = Error,
                        Success = Success,
                        TotalRecords = count,
                        Data = records
                    };

                }
                //with patient name
                else
                {
                    var count = 0;
                    var records = new List<FileInformationEntity>();
                    searchValue = searchValue != "null" && searchValue != null && searchValue != string.Empty ? searchValue.ToLower() : string.Empty;
                    //search empty - with patient name
                    if (searchValue == string.Empty)
                    {
                        //All Records - search value empty- with patient name
                        if (status == 2)
                        {
                            var residentName = patientName.Replace(", ", "^").Replace("|",".");
                           var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId 
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate) 
                                     && inb.File_Data.Contains(residentName)
                                     select inb).ToList();
                            //inbound filter start 
                            var filteredRecords = new List<FileInformation>();
                            foreach (var record in countRecords)
                            {

                                // Read the content of the HL7 file
                                string hl7Content = record.File_Data;

                                // Extract patient name from PID segment
                                string extractedName = ExtractPatientNameFromHL7(hl7Content);

                                // Check if extracted name matches the residentName
                                if (extractedName.Contains(residentName))
                                {
                                    filteredRecords.Add(record);
                                }

                            }


                            count = filteredRecords.Count();

                            //count = countRecords.Count();
                            //Inbound filter ended 
                            int skipRows = (currentPage - 1) * pageSize;
                            //records = (from inb in this.dbContext.FileInformations
                            //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) && inb.File_Data.Contains(residentName)
                            //           select new FileInformationEntity
                            //           {
                            //               //fileInfo = inb,
                            //               File_Id = inb.File_Id,
                            //               File_Category = inb.File_Category,
                            //               File_Data = inb.File_Data,
                            //               File_Error = inb.File_Error,
                            //               File_Name = inb.File_Name,
                            //               File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //               File_Status = inb.File_Status,
                            //               File_CreatedBy = inb.File_CreatedBy,
                            //               File_CreatedDate = inb.File_CreatedDate,
                            //               Company_Id = inb.Company_Id,
                            //               Event = inb.Event,
                            //           }).ToList()
                            records = filteredRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                       .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                        //selected status - search value empty- with patient name
                        else
                        {
                            var residentName = patientName.Replace(", ", "^");
                            var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId 
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate) 
                                     && inb.File_Data.Contains(residentName)
                                     && inb.File_Error == status.ToString()
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var displayRecords = (from inb in this.dbContext.FileInformations
                            //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //           && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) 
                            //           && inb.File_Data.Contains(residentName)
                            //           && inb.File_Error == status.ToString()
                            //           select new FileInformationEntity
                            //           {
                            //               fileInfo = inb,
                            //               File_Id = inb.File_Id,
                            //               File_Category = inb.File_Category,
                            //               File_Data = inb.File_Data,
                            //               File_Error = inb.File_Error,
                            //               File_Name = inb.File_Name,
                            //               File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //               File_Status = inb.File_Status,
                            //               File_CreatedBy = inb.File_CreatedBy,
                            //               File_CreatedDate = inb.File_CreatedDate,
                            //               Company_Id = inb.Company_Id,
                            //               Event = inb.Event,
                            //           }).ToList();
                            records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                            .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                    }
                    //search value - with patient name
                    else
                    {
                        //All Records - search value - with patient name
                        if (status == 2)
                        {
                            var residentName = patientName.Replace(", ", "^");
                            var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate) 
                                     && inb.File_Data.Contains(residentName)
                                     && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var displayRecords = (from inb in this.dbContext.FileInformations
                            //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //           && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) 
                            //           && inb.File_Data.Contains(residentName)
                            //           && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                            //           select new FileInformationEntity
                            //           {
                            //               //fileInfo = inb,
                            //               File_Id = inb.File_Id,
                            //               File_Category = inb.File_Category,
                            //               File_Data = inb.File_Data,
                            //               File_Error = inb.File_Error,
                            //               File_Name = inb.File_Name,
                            //               File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //               File_Status = inb.File_Status,
                            //               File_CreatedBy = inb.File_CreatedBy,
                            //               File_CreatedDate = inb.File_CreatedDate,
                            //               Company_Id = inb.Company_Id,
                            //               Event = inb.Event,
                            //           }).ToList();
                            records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                            .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                        //Selected status - search value - with patient name
                        else
                        {
                            var residentName = patientName.Replace(", ", "^");
                            var countRecords = (from inb in this.dbContext.FileInformations
                                     join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                                     where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                                     && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate) 
                                     && inb.File_Data.Contains(residentName)
                                     && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                                     && inb.File_Error == status.ToString()
                                     select inb).ToList();
                            count = countRecords.Count();
                            int skipRows = (currentPage - 1) * pageSize;
                            //var display = (from inb in this.dbContext.FileInformations
                            //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                            //           where fc.FteCategory_Desc == fileCategory && fc.FteCategory_Status == 1 && inb.Company_Id == companyId
                            //           && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate) 
                            //           && inb.File_Data.Contains(residentName)
                            //           && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                            //           && inb.File_Error == status.ToString()
                            //           select new FileInformationEntity
                            //           {
                            //               //fileInfo = inb,
                            //               File_Id = inb.File_Id,
                            //               File_Category = inb.File_Category,
                            //               File_Data = inb.File_Data,
                            //               File_Error = inb.File_Error,
                            //               File_Name = inb.File_Name,
                            //               File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                            //               File_Status = inb.File_Status,
                            //               File_CreatedBy = inb.File_CreatedBy,
                            //               File_CreatedDate = inb.File_CreatedDate,
                            //               Company_Id = inb.Company_Id,
                            //               Event = inb.Event,
                            //           }).ToList();
                            records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                 .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();
                        }
                    }
                    return new InboundFilesGridEntity
                    {
                        Reject = rejected,
                        Total = total,
                        Error = Error,
                        Success = Success,
                        TotalRecords = count,
                        Data = records
                    };
                }
            }
        }
        private string ExtractPatientNameFromHL7(string hl7Content)
        {
            string[] segments = hl7Content.Split('\r');

            foreach (var segment in segments)
            {
                if (segment.StartsWith("PID|"))
                {
                    string[] fields = segment.Split('|');
                    if (fields.Length > 5)
                    {
                        return fields[5];
                    }
                }
            }

            return string.Empty;
        }
        public InboundFilesGridEntity GetInboundRejectedFiles(string fromDate, string toDate, string searchValue, int currentPage, int pageSize,int companyId)
        {
            var ConvertedFromDate = GetDateByComanyId(Convert.ToDateTime(fromDate.Substring(0, 10)), companyId,1);
            var ConvertedToDate = GetDateByComanyId(Convert.ToDateTime(toDate.Substring(0, 10)), companyId,2);
            var fileFromDate = Convert.ToDateTime(ConvertedFromDate);
            var fileToDate = Convert.ToDateTime(ConvertedToDate);

            int count = 0;
            searchValue = searchValue != "null" && searchValue != null && searchValue != string.Empty ? searchValue.ToLower() : string.Empty;
            var records = new List<FileInformationEntity>();
            //SearchValue empty
            if (searchValue == string.Empty)
            {

               var countRecords = (from inb in this.dbContext.FileInformations
                         join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                         where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && inb.Company_Id == null
                         && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate)
                         select inb).ToList();
                count = countRecords.Count();
                int skipRows = (currentPage - 1) * pageSize;
                //records = (from inb in this.dbContext.FileInformations
                //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                //           where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && inb.Company_Id == null
                //           && (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                //           select new //FileInformationEntity
                //           {
                //               fileInfo = inb,
                //               //File_Id = inb.File_Id,
                //               //File_Category = inb.File_Category,
                //               //File_Data = inb.File_Data,
                //               //File_Error = inb.File_Error,
                //               //File_Name = inb.File_Name,
                //               //File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                //               //File_Status = inb.File_Status,
                //               //File_CreatedBy = inb.File_CreatedBy,
                //               //File_CreatedDate = inb.File_CreatedDate,
                //               //Company_Id = inb.Company_Id,
                //               //Event = inb.Event,
                //           }).ToList();

                records = countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                       .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();

            }
            else
            {
               var countRecords = (from inb in this.dbContext.FileInformations
                         join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                         where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && inb.Company_Id == null
                         && (inb.File_CreatedDate >= fileFromDate && inb.File_CreatedDate <= fileToDate)
                         && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                         select inb).ToList();
                count = countRecords.Count();
                int skipRows = (currentPage - 1) * pageSize;
                //records = (from inb in this.dbContext.FileInformations
                //           join fc in this.dbContext.FTECategories on inb.File_Category equals fc.FteCategory_Id
                //           where fc.FteCategory_Desc == "Inbound" && fc.FteCategory_Status == 1 && inb.Company_Id == null
                //           //&& (EntityFunctions.TruncateTime(inb.File_CreatedDate) >= fileFromDate && EntityFunctions.TruncateTime(inb.File_CreatedDate) <= fileToDate)
                //           && (inb.File_Name.ToLower().Contains(searchValue) || inb.Event.ToLower().Contains(searchValue) || (EntityFunctions.TruncateTime(inb.File_CreatedDate).ToString()).Contains(searchValue))
                //           select new //FileInformationEntity
                //           {
                //               fileInfo = inb,
                //               //File_Id = inb.File_Id,
                //               //File_Category = inb.File_Category,
                //               //File_Data = inb.File_Data,
                //               //File_Error = inb.File_Error,
                //               //File_Name = inb.File_Name,
                //               //File_ErrorDesc = inb.File_Error == "0" ? "Success" : "Error",
                //               //File_Status = inb.File_Status,
                //               //File_CreatedBy = inb.File_CreatedBy,
                //               //File_CreatedDate = inb.File_CreatedDate,
                //               //Company_Id = inb.Company_Id,
                //               //Event = inb.Event,
                //           }).ToList();
                records =countRecords.OrderByDescending(li => li.File_CreatedDate).Skip(skipRows).Take(pageSize).ToList()
                                       .Select(x => new FileInformationEntity
                                       {
                                           File_Id = x.File_Id,
                                           File_Category = x.File_Category,
                                           File_Data = x.File_Data,
                                           File_Error = x.File_Error,
                                           File_Name = x.File_Name,
                                           File_ErrorDesc = x.File_Error == "0" ? "Success" : "Error",
                                           File_Status = x.File_Status,
                                           File_CreatedBy = x.File_CreatedBy,
                                           File_CreatedDate = GetTimeZoneDateTime(x.File_CreatedDate, x.Company_Id),
                                           Company_Id = x.Company_Id,
                                           Event = x.Event,
                                       }).ToList();

            }
            return new InboundFilesGridEntity
            {
                TotalRecords = count,
                Data = records
            };
        }
        public Nullable<DateTime> GetTimeZoneDateTime(DateTime? dateTime, int? companyId)
        {
           var convetedDate=(this.dbContext.GetFileTimeZoneConvertedDateTime(dateTime, companyId,0).FirstOrDefault());
            return convetedDate;
        }
        public Nullable<DateTime> GetDateByComanyId(DateTime? dateTime, int? companyId,int type)
        {
            DateTime convetedDate =Convert.ToDateTime(this.dbContext.GetFileTimeZoneConvertedDateTime(dateTime, companyId,type).FirstOrDefault());
            return convetedDate;
        }
    }
}
