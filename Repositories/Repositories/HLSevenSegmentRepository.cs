using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.Web;
using System.Security.Claims;

namespace LTCPro.Repositories
{
    public class HLSevenSegmentRepository : IHLSevenSegmentRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public HLSevenSegmentRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }
        public List<HLSevenSegmentEntity> GetHLSevenSegmentData()
        {
            var segments = (from hl in this.dbContext.HLSevenSegments
                            select new HLSevenSegmentEntity
                            {
                                Segment_Id = hl.Segment_Id,
                                Segment_Desc = hl.Segment_Desc
                            }).ToList();
            return segments;
            //return this.autoMapper.Map<List<HLSevenSegment>, List<HLSevenSegmentEntity>>(segments);
        }
        public List<HLSevenSegmentDetailEntity> GetHLSevenSegmentDetailData(int segmentID)
        {
            var segmentDetails = this.dbContext.HLSevenSegmentDetails.Where(hs => hs.Segment_Id == segmentID).ToList();
            return this.autoMapper.Map<List<HLSevenSegmentDetail>, List<HLSevenSegmentDetailEntity>>(segmentDetails);
        }
        public List<HLSevenCompanyConfigEntity> GetHLSevenCompanyConfigEntity(int CompanyID, int SegmentID)
        {
            //var segmentDetails = this.dbContext.HLSevenCompanyConfigs.Where(hs => hs.Company_Id == CompanyID).ToList();
            //List<HLSevenCompanyConfigEntity> segments =(from hl in this.dbContext.HLSevenCompanyConfigs
            //                                            join seg in this.dbContext.HLSevenSegmentDetails on hl.SegDetail_Id equals seg.SegDetail_Id
            //                      where sd.Segment_Id == segmentID
            //                      select new HLSevenCompanyConfig {
            //                          HLConfig_Id=cc.HLConfig_Id,
            //                          Company_Id=cc.Company_Id,
            //                          SegDetail_Id=cc.SegDetail_Id,
            //                          SegDetailConfig_Id=cc.SegDetailConfig_Id,
            //                          HLConfig_Status=cc.HLConfig_Status,
            //                          HLConfig_CreatedBy=cc.HLConfig_CreatedBy,
            //                          HLConfig_CreatedDate=cc.HLConfig_CreatedDate
            //                      }).ToList();

            //var segmentDetails = this.dbContext.HLSevenCompanyConfigs.Where(hs => hs.Company_Id == CompanyID).ToList();
            //List<HLSevenCompanyConfigEntity> segments =(from hl in this.dbContext.HLSevenCompanyConfigs
            //                                            join seg in this.dbContext.HLSevenSegmentDetails on hl.SegDetail_Id equals seg.SegDetail_Id
            //                                            where seg.Segment_Id == SegmentID && hl.Company_Id == CompanyID
            //                                            select hl).ToList();
            var configs = (from hl in this.dbContext.HLSevenCompanyConfigs
                           join seg in this.dbContext.HLSevenSegmentDetails on hl.SegDetail_Id equals seg.SegDetail_Id
                           where seg.Segment_Id == SegmentID && hl.Company_Id == CompanyID
                           select new HLSevenCompanyConfigEntity
                           {
                               HLConfig_Id = hl.HLConfig_Id,
                               Company_Id = hl.Company_Id,
                               SegDetail_Id = hl.SegDetail_Id,
                               SegDetail_Desc = seg.SegDetail_Desc,
                               SegDisplay_Id = hl.SegDisplay_Id,
                               HLConfig_Status = hl.HLConfig_Status,
                               SegDetailConfig_Id = hl.SegDetailConfig_Id,
                               HLConfig_CreatedBy = hl.HLConfig_CreatedBy,
                               HLConfig_CreatedDate = hl.HLConfig_CreatedDate
                           }).ToList();

            return configs;
        }
        public int InsertHLSevenCompanyConfig(HLSevenCompanyConfigEntity hlSevenconfig)
        {
            hlSevenconfig.HLConfig_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var hlSevenDetails = this.autoMapper.Map<HLSevenCompanyConfigEntity, HLSevenCompanyConfig>(hlSevenconfig);
            if (hlSevenDetails.HLConfig_Id == 0)
            {
                this.dbContext.HLSevenCompanyConfigs.Add(hlSevenDetails);
            }
            else
            {
                HLSevenCompanyConfig record = this.dbContext.HLSevenCompanyConfigs.Find(hlSevenDetails.HLConfig_Id);
                record.Company_Id = hlSevenDetails.Company_Id;
                record.SegDetailConfig_Id = hlSevenDetails.SegDetailConfig_Id;
                record.SegDisplay_Id = hlSevenDetails.SegDisplay_Id;
                record.HLConfig_CreatedBy = hlSevenDetails.HLConfig_CreatedBy;
                record.HLConfig_CreatedDate = hlSevenDetails.HLConfig_CreatedDate;
            }
            this.dbContext.SaveChanges();

            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.HLSegmentFieldsConfig,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = hlSevenDetails.HLConfig_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public List<HlFieldsEntity> GetHLSevenConfigs(int residentId, string segmentDesc)
        {
            var visitInfoData = this.dbContext.VisitInfoes.Where(dm => dm.Patient_Id == residentId);
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
                int companyId = this.dbContext.Facilities.Where(p => p.Facility_Id == facilityId).Select(p => p.Company_Id).FirstOrDefault();

                List<HlFieldsEntity> configList = (from c in this.dbContext.HLSevenCompanyConfigs
                                                   join sd in this.dbContext.HLSevenSegmentDetails on c.SegDetail_Id equals sd.SegDetail_Id
                                                   join s in this.dbContext.HLSevenSegments on sd.Segment_Id equals s.Segment_Id
                                                   where s.Segment_Desc == segmentDesc && sd.SegDetail_Status == 1 && c.Company_Id == companyId
                                                   select new HlFieldsEntity
                                                   {
                                                       FieldName = sd.SegDetail_Desc,
                                                       IsMandatory = (int)c.SegDisplay_Id

                                                   }).ToList();
                return configList;
            }
            else
                return null;

        }

        public int CloneHlSevenCompanyConfig(HLSevenCloneEntity hlCloneConfig)
        {
            var segDetailIds = this.dbContext.HLSevenSegmentDetails.Where(s => s.Segment_Id == hlCloneConfig.SegmentId).Select(s => s.SegDetail_Id).ToList();

            var existingRecords = this.dbContext.HLSevenCompanyConfigs.Where(hl => hl.Company_Id == hlCloneConfig.OutputCompanyId && segDetailIds.Contains(hl.SegDetail_Id)).ToList();

            if (existingRecords.Count() == 0)
            {
                var records = (from hl in this.dbContext.HLSevenCompanyConfigs
                               where hl.Company_Id == hlCloneConfig.InputCompanyId && segDetailIds.Contains(hl.SegDetail_Id)
                               select new HLSevenCompanyConfigEntity
                               {
                                   Company_Id = hlCloneConfig.OutputCompanyId,
                                   SegDetail_Id = hl.SegDetail_Id,
                                   SegDetailConfig_Id = hl.SegDetailConfig_Id,
                                   SegDisplay_Id = hl.SegDisplay_Id,
                                   HLConfig_Status = hl.HLConfig_Status,
                                   HLConfig_CreatedBy = hlCloneConfig.CreatedBy,
                                   HLConfig_CreatedDate = DateTime.Now
                               }).ToList();
                var hlSevenDetails = this.autoMapper.Map<HLSevenCompanyConfigEntity, HLSevenCompanyConfig>(records);

                this.dbContext.HLSevenCompanyConfigs.AddRange(hlSevenDetails);
                this.dbContext.SaveChanges();
                return 1;
            }
            else
            {
                foreach (var item in existingRecords)
                {
                    var data = this.dbContext.HLSevenCompanyConfigs.Where(hl => hl.Company_Id == hlCloneConfig.InputCompanyId && hl.SegDetail_Id == item.SegDetail_Id).FirstOrDefault();
                    item.SegDisplay_Id = data.SegDisplay_Id;
                    item.HLConfig_CreatedBy = hlCloneConfig.CreatedBy;
                    item.HLConfig_CreatedDate = DateTime.Now;
                }
                this.dbContext.SaveChanges();
                return 1;
            }
        }
        public List<HlFieldsEntity> GetHLSevenConfigsBysegments(HLSevenConfigsEntity hlConfigs)
        {
            var visitInfoData = this.dbContext.VisitInfoes.Where(dm => dm.Patient_Id == hlConfigs.ResidentId);
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
                int companyId = this.dbContext.Facilities.Where(p => p.Facility_Id == facilityId).Select(p => p.Company_Id).FirstOrDefault();

                List<HlFieldsEntity> configList = (from c in this.dbContext.HLSevenCompanyConfigs
                                                   join sd in this.dbContext.HLSevenSegmentDetails on c.SegDetail_Id equals sd.SegDetail_Id
                                                   join s in this.dbContext.HLSevenSegments on sd.Segment_Id equals s.Segment_Id
                                                   where hlConfigs.SegmentDescs.Contains(s.Segment_Desc) && sd.SegDetail_Status == 1 && c.Company_Id == companyId
                                                   select new HlFieldsEntity
                                                   {
                                                       FieldName = sd.SegDetail_Desc,
                                                       IsMandatory = (int)c.SegDisplay_Id

                                                   }).ToList();
                return configList;
            }
            else
                return null;
        }

        public List<HLSevenOutboundDisplaySegmentEntity> GetHLSevenOutboundDisplaySegmentsData()
        {
            var segmentDetails = this.dbContext.HLSevenOutboundDisplaySegments.Where(hs => hs.OSegment_Status == 1).ToList();
            return this.autoMapper.Map<List<HLSevenOutboundDisplaySegment>, List<HLSevenOutboundDisplaySegmentEntity>>(segmentDetails);
        }

        public List<CompanyDropEntity> GetHlsevenEnabledCompanyDetails()
        {
            int roleId = 0;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (claimsIdentity.FindFirst("RoleId").Value != "")
                {
                    roleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);

                    List<int> facilityIds = this.dbContext.UserRoleFacilityConfigs.Where(i => i.User_Id == userId && i.Role_ID == roleId).Select(i => i.Facility_id).ToList();
                    if (facilityIds.Count() > 0)
                    {
                        List<int> companyIds = this.dbContext.Facilities.Where(p => facilityIds.Contains(p.Facility_Id)).Select(p => p.Company_Id).ToList();

                        var records = (from company in this.dbContext.Companies
                                       join country in this.dbContext.Countries on company.Company_CountryId equals country.Country_Id
                                       join user in this.dbContext.Users on company.Company_CreatedBy equals user.User_Id
                                       join cs in this.dbContext.CompanyConfigs on company.Company_Id equals cs.Company_Id
                                       where company.Company_Status == 1 && companyIds.Contains(company.Company_Id) && cs.Hl7Configured == 1
                                       select new CompanyDropEntity
                                       {
                                           Company_Id = company.Company_Id,
                                           Company_Name = company.Company_Name,
                                       }).ToList();
                        if (roleId == 1)
                        {
                            var companydrop = new CompanyDropEntity
                            {
                                Company_Id = 0,
                                Company_Name = "PowerAdmin"
                            };
                            records.Add(companydrop);
                        }
                        var data = records.OrderBy(s => s.Company_Id).ToList();
                        return data;
                    }
                }
            }
            return null;
        }

        public List<CustomHLSevenOutboundDisplaySegmentGridDataEntity> GetHLSevenOutboundDisplaySegmentsGridData(int segmentId, Nullable<int> companyId)
        {
            Nullable<int> EventId = null;
            int OutboundHlsevenSettingsResult = 0;
            if (segmentId == 1 || segmentId == 4 || segmentId == 5)
            {
                EventId = 11;
            }
            else if (segmentId == 2)
            {
                EventId = 9;
            }
            else if (segmentId == 3)
            {
                EventId = 10;
            }
            else if (segmentId == 6)
            {
                EventId = 15;
            }
            var Outboundsettings = (from cc in this.dbContext.CompanyConfigs
                                    join chl in this.dbContext.CompanyHlCategories on cc.CompanyConfig_Id equals chl.CompanyConfig_Id
                                    join ent in this.dbContext.CompanyHLEvents on chl.CompanyHlCategory_Id equals ent.CompanyHlCategory_Id
                                    join fte in this.dbContext.FTECategories on chl.FteCategory_Id equals fte.FteCategory_Id
                                    join evt in this.dbContext.EventCategories on fte.FteCategory_Id equals evt.Category_Id
                                    where evt.Category_Id == 2 && cc.Company_Id == companyId && ent.EventCat_Id == EventId
                                    select new
                                    {
                                        EventCatDesc = evt.EventCat_Desc,
                                        EventCategoryId = evt.EventCat_Id
                                    }).FirstOrDefault();
            if (Outboundsettings != null)
            {
                OutboundHlsevenSettingsResult = 1;
            }
            if (companyId == 0)
            {
                companyId = null;
                OutboundHlsevenSettingsResult = 1;
            }
            var records = (from hlc in this.dbContext.HLSevenOutboundDisplayCompanyConfigs
                           join hsd in this.dbContext.HLSevenOutboundDisplaySegmentDetails on hlc.OSegDetail_Id equals hsd.OSegDetail_Id
                           join hs in this.dbContext.HLSevenOutboundDisplaySegments on hsd.OSegment_Id equals hs.OSegment_Id
                           where hs.OSegment_Id == segmentId && hlc.Company_Id == companyId && hs.OSegment_Status == 1 && hsd.OSegDetail_Status == 1 && hlc.OHLConfig_Status == 1
                           select new CustomHLSevenOutboundDisplaySegmentGridDataEntity
                           {
                               OHLConfig_Id = hlc.OHLConfig_Id,
                               OSegDetail_Id = hsd.OSegDetail_Id,
                               OSegDetail_Desc = hsd.OSegDetail_Desc,
                               DisplayConfigId = hlc.DisplayConfigId,
                               HlSequence = hsd.HlSequence,
                               OutboundHlsevenSettingsResult = OutboundHlsevenSettingsResult
                           }).ToList();
            return records;
        }

        public int InsertUpdateHlsevenOutboundDisplayConfigsData(CustomHLSevenOutboundDisplayCompanyConfigEntity displayConfigs)
        {
            displayConfigs.OHLConfig_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var hlSevenOutboundDetails = this.autoMapper.Map<CustomHLSevenOutboundDisplayCompanyConfigEntity, HLSevenOutboundDisplayCompanyConfig>(displayConfigs);
            //var Commonfields = this.dbContext.HLSevenOutboundDisplaySegmentDetails.Where(s => s.OSegDetail_Id == displayConfigs.OSegDetail_Id).Select(x => new { x.HlSequence, x.OSegment_Id}).FirstOrDefault();
            //if ((Commonfields.OSegment_Id == 2 || Commonfields.OSegment_Id == 5 || Commonfields.OSegment_Id == 6) && Commonfields.HlSequence == 7)
            //{
            //    var Data = this.dbContext.HLSevenOutboundDisplaySegmentDetails.Where(e => e.HlSequence == 7 && (e.OSegment_Id == displayConfigs.OSegment_Id )).Select(s=>s.OSegDetail_Id).ToList();
            //    if (Data.Count() > 0)
            //    {
            //        var ids = this.dbContext.HLSevenOutboundDisplayCompanyConfigs.Where(e => Data.Contains(e.OSegDetail_Id) &&e.Company_Id ==displayConfigs.Company_Id).Select(s => s.OHLConfig_Id).ToList();
            //    }
            //} 
            if ((displayConfigs.OSegment_Id == 2 || displayConfigs.OSegment_Id == 5 || displayConfigs.OSegment_Id == 6) && displayConfigs.HlSequence == 7)
            {
                var Data = this.dbContext.HLSevenOutboundDisplaySegmentDetails.Where(e => e.HlSequence == 7 && (e.OSegment_Id == displayConfigs.OSegment_Id)).Select(s => s.OSegDetail_Id).ToList();
                if (Data.Count() > 0)
                {
                    var ids = this.dbContext.HLSevenOutboundDisplayCompanyConfigs.Where(e => Data.Contains(e.OSegDetail_Id) && e.Company_Id == (displayConfigs.Company_Id == 0 ? null : displayConfigs.Company_Id)).ToList();
                    foreach (var item in ids)
                    {
                        HLSevenOutboundDisplayCompanyConfig record = this.dbContext.HLSevenOutboundDisplayCompanyConfigs.Find(item.OHLConfig_Id);
                        record.Company_Id = hlSevenOutboundDetails.Company_Id == 0 ? null : hlSevenOutboundDetails.Company_Id;
                        record.OSegDetail_Id = item.OSegDetail_Id;
                        record.DisplayConfigId = hlSevenOutboundDetails.DisplayConfigId;
                        record.OHLConfig_CreatedBy = hlSevenOutboundDetails.OHLConfig_CreatedBy;
                        record.OHLConfig_CreatedDate = hlSevenOutboundDetails.OHLConfig_CreatedDate;
                        this.dbContext.SaveChanges();
                        // return 1;
                    }
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.ADTFieldsDisplayConfig,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = "",
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0
                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
            }
            if (displayConfigs.OSegment_Id == 2 && displayConfigs.OSegDetail_Id==66 && displayConfigs.DisplayConfigId==1 && displayConfigs.HlSequence == 3)
            {
                var Data = this.dbContext.HLSevenOutboundDisplaySegmentDetails.Where(e => e.HlSequence == 3 && (e.OSegDetail_Id==66 || e.OSegDetail_Id == 63 )&& (e.OSegment_Id == displayConfigs.OSegment_Id)).Select(s => s.OSegDetail_Id).ToList();
                if (Data.Count() > 0)
                {
                    var ids = this.dbContext.HLSevenOutboundDisplayCompanyConfigs.Where(e => Data.Contains(e.OSegDetail_Id) && e.Company_Id == (displayConfigs.Company_Id == 0 ? null : displayConfigs.Company_Id)).ToList();
                    foreach (var item in ids)
                    {
                        HLSevenOutboundDisplayCompanyConfig record = this.dbContext.HLSevenOutboundDisplayCompanyConfigs.Find(item.OHLConfig_Id);
                        record.Company_Id = hlSevenOutboundDetails.Company_Id == 0 ? null : hlSevenOutboundDetails.Company_Id;
                        record.OSegDetail_Id = item.OSegDetail_Id;
                        record.DisplayConfigId = hlSevenOutboundDetails.DisplayConfigId;
                        record.OHLConfig_CreatedBy = hlSevenOutboundDetails.OHLConfig_CreatedBy;
                        record.OHLConfig_CreatedDate = hlSevenOutboundDetails.OHLConfig_CreatedDate;
                        this.dbContext.SaveChanges();
                    }
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.ADTFieldsDisplayConfig,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = "",
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0
                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
            }
            else
            {
                HLSevenOutboundDisplayCompanyConfig record = this.dbContext.HLSevenOutboundDisplayCompanyConfigs.Find(displayConfigs.OHLConfig_Id);
                record.Company_Id = hlSevenOutboundDetails.Company_Id == 0 ? null : hlSevenOutboundDetails.Company_Id;
                record.OSegDetail_Id = hlSevenOutboundDetails.OSegDetail_Id;
                record.DisplayConfigId = hlSevenOutboundDetails.DisplayConfigId;
                record.OHLConfig_CreatedBy = hlSevenOutboundDetails.OHLConfig_CreatedBy;
                record.OHLConfig_CreatedDate = hlSevenOutboundDetails.OHLConfig_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ADTFieldsDisplayConfig,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.DisplayConfigId.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            return 0;
        }

        public List<HLSevenOutboundDisplayCompanyConfigEntity> GetOutboundHLSevenConfigs(int residentId, int segmentId, int? facilityID = null)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            Nullable<int> companyId = 0;
            if (claimsIdentity.FindFirst("RoleId").Value != "" && Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value) == 1)
            {
                companyId = null;
            }
            else
            {
                if (residentId != 0)
                {
                    var visitInfoData = this.dbContext.VisitInfoes.Where(dm => dm.Patient_Id == residentId);
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
                        companyId = this.dbContext.Facilities.Where(p => p.Facility_Id == facilityId).Select(p => p.Company_Id).FirstOrDefault();
                    }
                }
                else
                {
                    companyId = this.dbContext.Facilities.Where(f=>f.Facility_Id==facilityID).Select(f=>f.Company_Id).FirstOrDefault();
                }
            }

            List<HLSevenOutboundDisplayCompanyConfigEntity> records = (from hlc in this.dbContext.HLSevenOutboundDisplayCompanyConfigs
                                                                       join hsd in this.dbContext.HLSevenOutboundDisplaySegmentDetails on hlc.OSegDetail_Id equals hsd.OSegDetail_Id
                                                                       join hs in this.dbContext.HLSevenOutboundDisplaySegments on hsd.OSegment_Id equals hs.OSegment_Id
                                                                       where hs.OSegment_Id == segmentId && hlc.Company_Id == companyId && hs.OSegment_Status == 1 && hsd.OSegDetail_Status == 1 && hlc.OHLConfig_Status == 1
                                                                       select new HLSevenOutboundDisplayCompanyConfigEntity
                                                                       {
                                                                           OSegDetail_Desc = hsd.OSegDetail_Desc,
                                                                           DisplayConfigId = hlc.DisplayConfigId,
                                                                       }).ToList();
            return records;
        }
    }

}
