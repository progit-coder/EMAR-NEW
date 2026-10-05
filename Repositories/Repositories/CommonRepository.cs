using CrystalDecisions.CrystalReports.Engine;
using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.SqlClient;

namespace LTCPro.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        public CommonRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, FacilityRepository facilityRepository, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._facilityRepository = facilityRepository;
            _userActivityRepository = userActivityRepository;
        }
        public List<CountryEntity> GetCountries()
        {
            var countries = this.dbContext.Countries.Where(c => c.Country_Status == 1).ToList();
            return this.autoMapper.Map<List<Country>, List<CountryEntity>>(countries);
        }

        public List<GenderEntity> GetGenders()
        {
            var gender = this.dbContext.Genders.Where(g => g.Gender_Status == 1).ToList();
            return this.autoMapper.Map<List<Gender>, List<GenderEntity>>(gender);
        }

        public List<SuffixEntity> GetSuffix()
        {
            var suffix = this.dbContext.Suffixes.Where(g => g.Suffix_Status == 1).ToList();
            return this.autoMapper.Map<List<Suffix>, List<SuffixEntity>>(suffix);
        }

        public List<FTECategoryEntity> GetFTECategories()
        {
            var fteCategories = this.dbContext.FTECategories.ToList();
            return this.autoMapper.Map<List<FTECategory>, List<FTECategoryEntity>>(fteCategories);
        }

        public List<FteConnectionEntity> GetFTEConnections()
        {
            var fteConnections = this.dbContext.FteConnections.ToList();
            return this.autoMapper.Map<List<FteConnection>, List<FteConnectionEntity>>(fteConnections);
        }
        public int InsertUpdatePatientType(PatientTypeEntity entity)
        {
            entity.PatientType_CreatedDate = Convert.ToDateTime(GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<PatientTypeEntity, PatientType>(entity);
            var patientType = this.dbContext.PatientTypes.Where(pt => pt.PatientType_Id == record.PatientType_Id).FirstOrDefault();
            if (patientType == null)
            {
                this.dbContext.PatientTypes.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ColorPicker,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.PatientType_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            else
            {
                PatientType data = this.dbContext.PatientTypes.Find(entity.PatientType_Id);
                data.Company_Id = record.Company_Id;
                data.Color_Description = record.Color_Description;
                data.Color_Code = record.Color_Code;
                data.PatientType_CreatedBy = record.PatientType_CreatedBy;
                data.PatientType_Status = record.PatientType_Status;
                data.PatientType_CreatedDate = record.PatientType_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ColorPicker,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.PatientType_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            return 1;
        }

        public List<PatientTypeCustomEntity> GetPatientTypeData()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (claimsIdentity.FindFirst("RoleId").Value != "")
                {
                    int roleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);

                    List<int> facilityIds = this.dbContext.UserRoleFacilityConfigs.Where(i => i.User_Id == userId && i.Role_ID == roleId).Select(i => i.Facility_id).ToList();
                    if (facilityIds.Count() > 0)
                    {
                        List<int> companyIds = this.dbContext.Facilities.Where(p => facilityIds.Contains(p.Facility_Id)).Select(p => p.Company_Id).ToList();

                        return (from a in this.dbContext.PatientTypes
                                join user in this.dbContext.Users on a.PatientType_CreatedBy equals user.User_Id
                                join ch in this.dbContext.Companies on a.Company_Id equals ch.Company_Id
                                where ch.Company_Status == 1 && companyIds.Contains(ch.Company_Id)
                                select new PatientTypeCustomEntity
                                {
                                    PatientType_Id = a.PatientType_Id,
                                    Company_Id = a.Company_Id,
                                    Company_Name = ch.Company_Name,
                                    Color_Code = a.Color_Code,
                                    Color_Description = a.Color_Description,
                                    PatientType_Status = a.PatientType_Status,
                                    PatientType_CreatedBy = user.User_DisplayName,
                                    CreatedBy = (int)a.PatientType_CreatedBy,
                                    PatientType_CreatedDate = a.PatientType_CreatedDate
                                }).OrderBy(item => item.Company_Name).ThenBy(item => item.Color_Description).ToList();
                    }
                }
            }
            return null;
        }
        public PatientTypeCustomEntity GetPatientTypeByID(int patientTypeId)
        {
            var record = this.dbContext.PatientTypes.Where(pt => pt.PatientType_Id == patientTypeId).FirstOrDefault();
            return this.autoMapper.Map<PatientType, PatientTypeCustomEntity>(record);
        }
        public List<PatientTypeCustomEntity> GetPatientTypeDrop(int patientId)
        {
            var visitInfoData = this.dbContext.VisitInfoes.Where(dm => dm.Patient_Id == patientId);
            if (visitInfoData.Count() > 1)
            {
                var notDischargedRecord = visitInfoData.Where(v => v.DischargeDate == null);
                if (notDischargedRecord.Count() == 0)
                    visitInfoData = visitInfoData.OrderByDescending(v => v.DischargeDate);
                else
                    visitInfoData = notDischargedRecord;
            }
            int facilityId = visitInfoData.FirstOrDefault().FacilityId != null ? (int)visitInfoData.FirstOrDefault().FacilityId : 0;
            var companyId = this.dbContext.Facilities.Where(c => c.Facility_Id == facilityId).Select(c => c.Company_Id).FirstOrDefault();


            return (from a in this.dbContext.PatientTypes
                    join user in this.dbContext.Users on a.PatientType_CreatedBy equals user.User_Id
                    join ch in this.dbContext.Companies on a.Company_Id equals ch.Company_Id
                    where a.PatientType_Status == 1 && (int)a.Company_Id == companyId
                    select new PatientTypeCustomEntity
                    {
                        PatientType_Id = a.PatientType_Id,
                        Company_Name = ch.Company_Name,
                        Color_Code = a.Color_Code,
                        Color_Description = a.Color_Description,
                        PatientType_Status = a.PatientType_Status,
                        PatientType_CreatedBy = user.UserName,
                        CreatedBy = (int)a.PatientType_CreatedBy,
                        PatientType_CreatedDate = a.PatientType_CreatedDate
                    }).ToList();

        }
        public int UpdatePatientTypeStatus(List<PatientTypeCustomEntity> data)
        {
            foreach (var item in data)
            {
                item.PatientType_CreatedDate = Convert.ToDateTime(GetNursingStationTimeZoneDate());
                var record = this.dbContext.PatientTypes.Find(item.PatientType_Id);
                if (record != null)
                {
                    record.PatientType_Status = record.PatientType_Status == 1 ? 0 : 1;
                    record.PatientType_CreatedBy = item.CreatedBy;
                    record.PatientType_CreatedDate = item.PatientType_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int InsertUpdateUserRecentFacNs(RecentFacEntity obj)
        {
            if (obj.User_Id == 0)
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    obj.User_Id = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                }
            }
            if (obj.Facility_Id == 0)
            {
                var nsIds = obj.NurseStation_Id.Split(',');
                int nsId = Convert.ToInt32(nsIds[0]);
                obj.Facility_Id = (int)this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nsId).Select(ns => ns.Facility_Id).FirstOrDefault();
            }
            var record = this.autoMapper.Map<RecentFacEntity, RecentFac>(obj);
            var data = this.dbContext.RecentFacs.Where(re => re.User_Id == record.User_Id).FirstOrDefault();
            if (data == null)
            {
                this.dbContext.RecentFacs.Add(record);
                this.dbContext.SaveChanges();
                return 1;
            }
            else
            {
                data.Facility_Id = obj.Facility_Id;
                data.NurseStation_Id = obj.NurseStation_Id;
                this.dbContext.SaveChanges();
                return 1;
            }
        }
        public int InsertUpdateMeasurementsandUserInputs(PrcInsertOrderFavFacilityConfigEntity obj)
        {
            obj.oFConfig_CreatedDate = Convert.ToDateTime(GetNursingStationTimeZoneDate());
            var records = this.dbContext.OrderFavConfigs.Where(s => s.Facility_Id == obj.facility_Id && s.OrderFavCode == obj.orderFavCode).FirstOrDefault();
            if (records != null && obj.events == 1)
            {
                return 2;
            }
            else
            { 
            int? i = this.dbContext.PrcInsertOrderFavFacilityConfig(obj.facility_Id, obj.orderFavCode, obj.orderFavMaster, obj.oFConfig_Status,obj.oFConfig_CreatedBy,obj.oFConfig_CreatedDate,obj.events);
                return 1;
            }
        }
        public PrcGetOrderFavConfigByCodeEntity GetOrderFavConfigDetailsById(CustomOrderFavInfoDataEntity entity)
        {
            var record = this.dbContext.PrcGetOrderFavConfigByCode(entity.FacilityId, entity.Code,Convert.ToDateTime(entity.CreatedDate)).FirstOrDefault();
            return this.autoMapper.Map<PrcGetOrderFavConfigByCode_Result, PrcGetOrderFavConfigByCodeEntity>(record);
        }
        public List<PrcGetOrderFavConfigInfoEntity> GetOrderFavDetails(int FacilityId)
      {
           
            var data = this.dbContext.PrcGetOrderFavConfigInfo(FacilityId).ToList();
            return this.autoMapper.Map<List<PrcGetOrderFavConfigInfo_Result>, List<PrcGetOrderFavConfigInfoEntity>>(data);
        }
        public RecentFacEntity GetUserRecentFacNs(int userId)
        {
            var record = this.dbContext.RecentFacs.Where(re => re.User_Id == userId).FirstOrDefault();
            var result= this.autoMapper.Map<RecentFac, RecentFacEntity>(record);
            if (record != null)
            {
                result.companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == record.Facility_Id).Select(e => e.Company_Id).FirstOrDefault();
            }
            return result;
        }

        // Document CHeck 

        public int RemoveDocumentCheck(List<DrugAdministerEntity> data)
        {
            //var record = this.autoMapper.Map<DrugAdministerEntity, DrugAdminister>(entity);
            foreach (var item in data)
            {
                DrugAdminister record = this.dbContext.DrugAdministers.Where(i => i.DrugAdminister_Id == item.DrugAdminister_Id && i.is_deleted == null).FirstOrDefault();
                var fac = (from da in this.dbContext.DrugAdministers
                           join cm in this.dbContext.CommonOrderInfoes on da.Porder_Id equals cm.POrder_Id
                           join vs in this.dbContext.VisitInfoes on cm.Patient_Id equals vs.Patient_Id
                           where da.DrugAdminister_Id==record.DrugAdminister_Id
                           select new
                           {
                               fac = vs.FacilityId
                           }).Select(vc => vc.fac).FirstOrDefault();
                var convetedDate = (this.dbContext.GetTimeZoneConvertedDateTime(Convert.ToDateTime(GetNursingStationTimeZoneDate()), fac).FirstOrDefault());
                if (data != null)
                {
                    var insulinSites = this.dbContext.tblAdministeredSites.Where(t => t.DrugAdminister_Id == item.DrugAdminister_Id).ToList();
                    if (insulinSites.Count() > 0)
                    {
                        this.dbContext.tblAdministeredSites.RemoveRange(insulinSites);
                        this.dbContext.SaveChanges();
                    }
                    var TransAdminsterOn = record.AdminsterOn;
                    var resonDate = (this.dbContext.GetTimeZoneConvertedDateTime(record.AdminsterOn, fac).FirstOrDefault());
                    var USR = this.dbContext.Users.Where(U => U.User_Id == item.AdminsterBy).FirstOrDefault();
                    var MedicationReaosn = this.dbContext.MedicationReasons.Where(U => U.MedicationReason_ID == item.MedicationReason_ID).Select(U => U.MedicationReason_Desc).FirstOrDefault();
                    var UserRL = USR.User_Lname.Substring(0, 1);
                    var UserRF = USR.User_Fname.Substring(0, 1);
                    var UsersResonss = "";
                    if (record.ManualDocumentedBy != null)
                        UsersResonss = UserRL + UserRF + " " + Convert.ToString(TransAdminsterOn.Value.ToString("h:mm tt")) + " " + MedicationReaosn;
                    else
                        UsersResonss = UserRL + UserRF + " " + Convert.ToString(resonDate.Value.ToString("h:mm tt")) + " " + MedicationReaosn;

                    //record.AdminsterBy = null;
                    //record.AdminsterOn = null;
                    
                    //record.ManualDocumentedBy = null;
                    record.AdminsterBy = item.AdminsterBy;
                    //record.AdminsterOn = convetedDate;//Convert.ToDateTime(GetNursingStationTimeZoneDate());
                    record.AdminsterStatus = 0;
                    record.MedicationReason_ID = item.MedicationReason_ID;
                    record.DrugQuantity = item.quantity;
                    record.additionalcomments= item.AdministerComment;
                    this.dbContext.SaveChanges();
                   

                    

                    DocAdministerTran docObj = new DocAdministerTran();
                    docObj.Dacd_Id = 2;
                    docObj.DrugAdminister_Id = record.DrugAdminister_Id;
                    docObj.ManualDocumentedBy = item.AdminsterBy;
                    docObj.AdminsterOn = TransAdminsterOn;
                    docObj.EnteredBy = item.AdminsterBy;
                    docObj.EnteredDate = convetedDate;//DateTime.Now;
                    docObj.ManualDocumentedDate = convetedDate;//Convert.ToDateTime(GetNursingStationTimeZoneDate());
                    docObj.Reason = UsersResonss;
                    this.dbContext.DocAdministerTrans.Add(docObj);
                    this.dbContext.SaveChanges();
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.DocumentAdministeredOrders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0
            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int InsertUpdateDocumentCheck(List<DrugAdministerEntity> data)
        {
            //var record = this.autoMapper.Map<DrugAdministerEntity, DrugAdminister>(entity);
            foreach (var item in data)
            {
                if(item.AdminsterOnTime != null)
                {
                    if (!item.AdminsterOnTime.Contains(":"))
                    {
                        item.AdminsterOnTime = item.AdminsterOnTime.Insert(2,":");
                        // Do Something // 
                    }
                }
                item.AdminsterOn = Convert.ToDateTime(item.AdminsterOnDate + " " + item.AdminsterOnTime);
                item.ManualDocumentedDate = Convert.ToDateTime(GetNursingStationTimeZoneDate());
                DrugAdminister record = this.dbContext.DrugAdministers.Where(i => i.DrugAdminister_Id == item.DrugAdminister_Id && i.is_deleted == null).FirstOrDefault();
                if (data != null)
                {

                    record.MedicationReason_ID = item.MedicationReason_ID;
                    record.AdminsterBy = item.AdminsterBy;
                    record.AdminsterOn = item.AdminsterOn;
                    record.AdminsterStatus=1;
                    record.ManualDocumentedBy = item.ManualDocumentedBy;
                    record.ManualDocumentedDate = item.ManualDocumentedDate;
                    record.AdministerComment = item.AdministerComment;
                    if (record.EkitAdminister != null)
                    {
                        record.EkitAdminister = 0;
                        record.Ekit_Id = null;
                    }
                    this.dbContext.SaveChanges();

                    DocAdministerTran docObj = new DocAdministerTran();
                    docObj.Dacd_Id = 1;
                    docObj.DrugAdminister_Id = item.DrugAdminister_Id;
                    docObj.ManualDocumentedBy = item.ManualDocumentedBy;
                    docObj.ManualDocumentedDate = item.ManualDocumentedDate;
                    docObj.EnteredBy = item.ManualDocumentedBy;
                    docObj.EnteredDate = DateTime.Now;
                    //docObj.ManualDocumentedDate = item.ManualDocumentedDate;
                    //docObj.ManualDocumentedDate = item.ManualDocumentedDate;
                    docObj.AdminsterBy = item.AdminsterBy;
                    docObj.AdminsterOn = item.AdminsterOn;

                    this.dbContext.DocAdministerTrans.Add(docObj);
                    this.dbContext.SaveChanges();
                    //Discard Date Inser/Update
                    if (item.DiscardDate != null && record.DAdmin_Id != null)
                    {
                        var checkExist = this.dbContext.tblDrugDiscardInfoes.Where(td => td.DAdmin_Id == record.DAdmin_Id).FirstOrDefault();
                        if (checkExist == null)
                        {
                            tblDrugDiscardInfo discardObj = new tblDrugDiscardInfo();
                            discardObj.DAdmin_Id = record.DAdmin_Id;
                            discardObj.DiscardDate = item.DiscardDate;
                            discardObj.CreatedDate = (DateTime)item.AdminsterOn;
                            discardObj.CreatedBy = item.AdminsterBy;
                            this.dbContext.tblDrugDiscardInfoes.Add(discardObj);
                            this.dbContext.SaveChanges();
                        }
                        else
                        {
                            checkExist.DiscardDate = item.DiscardDate;
                            checkExist.CreatedDate = (DateTime)item.AdminsterOn;
                            checkExist.CreatedBy = item.AdminsterBy;
                            this.dbContext.SaveChanges();
                        }
                    }
                    //Insulin Sites
                    if (record.DAdmin_Id != null && item.AdministerInsulinSites != null && item.AdministerInsulinSites != "null" && item.AdministerInsulinSites != "")
                    {
                        List<string> siteId = item.AdministerInsulinSites.Split(',').ToList();
                        if (siteId.Count() > 0)
                        {
                            foreach (var id in siteId)
                            {
                                int insulinSite = Convert.ToInt32(id);
                                var administerSchedule = this.dbContext.DrugAdministers.Where(d => d.DrugAdminister_Id == record.DrugAdminister_Id && d.is_deleted == null).Select(d => d.AdminsterSchedule).FirstOrDefault();
                                var checkRecord = this.dbContext.tblAdministeredSites.Where(t => t.DrugAdminister_Id == record.DrugAdminister_Id && t.Site_Id == insulinSite).FirstOrDefault();
                                if (checkRecord == null)
                                {
                                    tblAdministeredSite siteObj = new tblAdministeredSite();
                                    siteObj.DrugAdminister_Id = record.DrugAdminister_Id;
                                    siteObj.DAdmin_Id = record.DAdmin_Id;
                                    siteObj.Route = item.RouteCode;
                                    siteObj.Site_Id = insulinSite;
                                    siteObj.AdministeredDate = (DateTime)administerSchedule;
                                    siteObj.CreatedDate = (DateTime)item.AdminsterOn;
                                    siteObj.CreatedBy = item.AdminsterBy;
                                    this.dbContext.tblAdministeredSites.Add(siteObj);
                                    this.dbContext.SaveChanges();
                                }
                                else
                                {
                                    checkRecord.DrugAdminister_Id = record.DrugAdminister_Id;
                                    checkRecord.DAdmin_Id = record.DAdmin_Id;
                                    checkRecord.Route = item.RouteCode;
                                    checkRecord.Site_Id = insulinSite;
                                    checkRecord.AdministeredDate = (DateTime)(DateTime)administerSchedule;
                                    checkRecord.CreatedDate = (DateTime)(DateTime)item.AdminsterOn;
                                    checkRecord.CreatedBy = item.AdminsterBy;
                                    this.dbContext.SaveChanges();
                                }
                            }
                        }
                    }

                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.DocumentAdministeredOrders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public List<GetDocumentCheckDataEntity> GetOrdersListData(OrdersListFilter filter)
        {
            //time = time == "null" ? null : time.Replace('-', ':');
            //dateValue = dateValue.Replace('-', '/');
            //DateTime date = Convert.ToDateTime(dateValue);

            //var records = this.dbContext.PrcGetDocumentCheckData(residentId, date, time, shiftId).ToList();
            string tim = filter.time == null ? null : string.Join(",", filter.time).Replace('-', ':');
            filter.dateValue = filter.dateValue.Replace('-', '/');
            DateTime date = Convert.ToDateTime(filter.dateValue);

            var records = this.dbContext.PrcGetDocumentCheckData(filter.residentId, date, tim, filter.shiftId).ToList();
            if (records.Count > 0 && records.Find(o => o.PRNFlag == true) != null)
            {
                foreach (var item in records)
                {
                    var Date = Convert.ToDateTime(filter.dateValue.Substring(0, 10));
                    int orderId = Convert.ToInt32(item.Porder_Id);
                    int quantityd = Convert.ToInt32(item.PQuantity_Id);
                    int count = this.dbContext.DrugAdministers.Where(d => d.Porder_Id == orderId && d.is_deleted == null && d.PQuantity_Id == quantityd && EntityFunctions.TruncateTime(d.AdminsterOn) == Date).ToList().Count();
                    item.PRNAdministered = count;
                }
            }
            var data = this.autoMapper.Map<PrcGetDocumentCheckData_Result, GetDocumentCheckDataEntity>(records);

            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                var recentFacNs = this.dbContext.VisitInfoes.Where(n => n.Patient_Id == filter.residentId).FirstOrDefault();
                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = userId,
                    Facility_Id = (int)recentFacNs.FacilityId,
                    NurseStation_Id = recentFacNs.NursingStationId.ToString(),
                };
                InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            return data.OrderBy(item => item.Order).ToList();
        }


        public List<GetDocumentCheckDataEntity> GetDrugsListData(int residentId, string startdate, string enddate)
        {
            //time = time == "null" ? null : time.Replace('-', ':');
            startdate = startdate.Replace('-', '/');
            enddate = enddate.Replace('-', '/');
            DateTime date1 = Convert.ToDateTime(startdate);
            DateTime date2 = Convert.ToDateTime(enddate);
            var records = this.dbContext.PrcGetRemoveDocumentOrderData(residentId, date1, date2).ToList();
            var data = this.autoMapper.Map<PrcGetRemoveDocumentOrderData_Result, GetDocumentCheckDataEntity>(records);

            return data.OrderBy(item => item.Order).ToList();
        }
        public List<DosesDetailsEntity> GetPendingDosesList(int userId, string nsIds,int? facIds)
        {
            if (facIds == 0)
            {
                facIds = null;
            }
            if (nsIds == "null")
            {
                nsIds = null;
            }
            var doses = this.dbContext.prcgetDosesDetails(userId, nsIds,facIds).ToList();
            return this.autoMapper.Map<List<prcgetDosesDetails_Result>, List<DosesDetailsEntity>>(doses);
        }
        public int InsertIgnoreDosesDetails(long drugAdministerId)
        {
            DrugAdminister record = this.dbContext.DrugAdministers.Where(i => i.DrugAdminister_Id == drugAdministerId && i.is_deleted ==null).FirstOrDefault();
            if (record != null)
            {
                record.NotificationStat = 1;
                this.dbContext.SaveChanges();
            }
            return 1;

        }
        public List<PrcGetDocAdminOrderAudit_ResultEntity> GetDocAdminOrderAudit(Int64 DrugAdminsterID)
        {
            var records = this.dbContext.PrcGetDocAdminOrderAudit(DrugAdminsterID).ToList();
            return this.autoMapper.Map<List<PrcGetDocAdminOrderAudit_Result>, List<PrcGetDocAdminOrderAudit_ResultEntity>>(records);
        }
        public List<OutboundErrorDetailsEntity> GetOutboundErrorDetails(int userId)
        {
            var records = this.dbContext.PrcGetOutBoundErrorAck(userId).ToList();
            return this.autoMapper.Map<List<PrcGetOutBoundErrorAck_Result>, List<OutboundErrorDetailsEntity>>(records);
        }
        public int InsertIgnoreOutboundErrorDetails(int fileId)
        {
            OutBoundFileInformation record = this.dbContext.OutBoundFileInformations.Where(i => i.File_Id == fileId).FirstOrDefault();
            if (record != null)
            {
                record.File_Status = 2;
                this.dbContext.SaveChanges();
            }
            return 1;

        }

        public List<OrderFavouriteCustomEntity> GetOderFavInfo()
        {
            return this.dbContext.OrderFavouriteMasters.Where(c => c.OrderFavMaster_status == 1).Select(c => new OrderFavouriteCustomEntity { OrderFavMaster_ID = c.OrderFavMaster_ID, OrderFavDesc = c.OrderFavDesc }).ToList();

        }

        //public List<NurseStationHierarchyEntity> GetNurseStationHierarchyMaster()
        //{
        //}

        public int GetUserPassedDueFlag(int userId)
        {
            var record = this.dbContext.Users.Where(u => u.User_Id == userId).FirstOrDefault();
            int flag = record.PastDueAlertFlag == null ? 0 : (int)record.PastDueAlertFlag;
            return flag;
        }
        public int InsertUpdateRefillMailConfig(RefillMailConfigCustomEntity entity)
        {
            entity.CreatedDate = Convert.ToDateTime(GetNursingStationTimeZoneDate());
            var nursingStations = entity.NurseStation_Id.Split(',').ToList();
            var timesIds = this.dbContext.tblRDCMailConfigs.Where(t=>t.Facility_Id==entity.Facility_Id && nursingStations.Contains(t.NurseStation_Id.ToString())).Select(t => t.Rdc_Id).ToList();
            var deleteTimeIds = this.dbContext.tblRdcMailTimes.Where(t => timesIds.Contains((int)t.Rdc_Id)).ToList();
            if(deleteTimeIds.Count()>0)
            {
                this.dbContext.tblRdcMailTimes.RemoveRange(deleteTimeIds);
                this.dbContext.SaveChanges();
            }
                List<string> nsIds = entity.NurseStation_Id.Split(',').ToList();
                foreach (var item in nsIds)
                {
                int nusringStationId = Convert.ToInt32(item);
                var checkExist = this.dbContext.tblRDCMailConfigs.Where(t => t.Facility_Id == entity.Facility_Id && t.NurseStation_Id == nusringStationId).FirstOrDefault();
                if (checkExist==null)
                {
                    tblRDCMailConfig obj = new tblRDCMailConfig();
                    obj.Facility_Id = entity.Facility_Id;
                    obj.NurseStation_Id = nusringStationId;
                    obj.MailTo = entity.MailTo;
                    obj.MailCC = entity.MailCC;
                    obj.Status = entity.Status;
                    obj.CreatedBy = entity.CreatedBy;
                    obj.CreatedDate = entity.CreatedDate;
                    this.dbContext.tblRDCMailConfigs.Add(obj);
                    this.dbContext.SaveChanges();
                    List<string> hours = entity.TimeIDs.Split(',').ToList();
                    foreach (var time in hours)
                    {
                        int hour = Convert.ToInt32(time);
                        if (this.dbContext.tblRdcMailTimes.Where(t => t.Rdc_Id == obj.Rdc_Id && t.Time_Id == hour).FirstOrDefault() == null)
                        {
                            tblRdcMailTime data = new tblRdcMailTime();
                            data.Rdc_Id = obj.Rdc_Id;
                            data.Time_Id = hour;
                            this.dbContext.tblRdcMailTimes.Add(data);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    checkExist.MailTo = entity.MailTo;
                    checkExist.MailCC = entity.MailCC;
                    checkExist.CreatedBy = entity.CreatedBy;
                    checkExist.CreatedDate = entity.CreatedDate;
                    this.dbContext.SaveChanges();
                    List<string> hours = entity.TimeIDs.Split(',').ToList();
                    foreach (var time in hours)
                    {
                        int hour = Convert.ToInt32(time);
                        if (this.dbContext.tblRdcMailTimes.Where(t => t.Rdc_Id == checkExist.Rdc_Id && t.Time_Id == hour).FirstOrDefault() == null)
                        {
                            tblRdcMailTime data = new tblRdcMailTime();
                            data.Rdc_Id = checkExist.Rdc_Id;
                            data.Time_Id = hour;
                            this.dbContext.tblRdcMailTimes.Add(data);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.RefillRejectMailConfig,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = entity.CreatedBy.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public List<RefillMailConfigGridData> GetRefillConfigGridData(int userId)
        {
            List<RefillMailConfigGridData> gridData = new List<RefillMailConfigGridData>();
            var records = (from us in this.dbContext.UserRoleFacilityConfigs
                           join tb in this.dbContext.tblRDCMailConfigs on us.Facility_id equals tb.Facility_Id
                           join tbl in this.dbContext.tblRDCMailConfigs on us.NurseStation_Id equals tbl.NurseStation_Id
                           join fa in this.dbContext.Facilities on us.Facility_id equals fa.Facility_Id
                           join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                           where us.User_Id == userId && fa.Facility_Status == 1 && ns.NurseStation_Status == 1
                           select new
                           {
                               RDC_Id=tbl.Rdc_Id,
                               FacilityId = fa.Facility_Id,
                               NursingStationId = ns.NurseStation_Id,
                               Facility_Name = fa.Facility_Name,
                               NursingStationName = ns.NurseStation_Name,
                           }).ToList().Distinct();

            var data = records.GroupBy(m => new { m.RDC_Id }).Select(group => group.FirstOrDefault()).ToList();
            foreach(var item in data)
            {
                int facId = Convert.ToInt32(item.FacilityId);
                int rdId = Convert.ToInt32(item.RDC_Id);
                List<string> timesList = new List<string>();
                int cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat(facId);
                string toMail = this.dbContext.tblRDCMailConfigs.Where(r => r.Rdc_Id == rdId).Select(r => r.MailTo).FirstOrDefault();
                string ccMail = this.dbContext.tblRDCMailConfigs.Where(r => r.Rdc_Id == rdId).Select(r => r.MailCC).FirstOrDefault();
                List<int> timeIds = this.dbContext.tblRdcMailTimes.Where(t => rdId==t.Rdc_Id).Select(t => (int)t.Time_Id).Distinct().ToList();
                if(cmpTimeFormate==0)
                {
                    foreach(var time in timeIds)
                    {
                        int timeId = (int)time;
                        var atHour = this.dbContext.Hours.Where(h => h.Hour_Id == timeId).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                        string hour = atHour.RegularTime + " " + atHour.RegularTimeFormat;
                        timesList.Add(hour);
                    }
                }
                else if (cmpTimeFormate == 1)
                {
                    foreach (var time in timeIds)
                    {
                        int timeId = (int)time;
                        var atHour = this.dbContext.Hours.Where(h => h.Hour_Id == timeId).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                        string hour = atHour.Hour_Desc;
                        timesList.Add(hour);
                    }
                }
                RefillMailConfigGridData obj= new RefillMailConfigGridData();
                obj.Facility_Id = facId;
                obj.FacilityName = item.Facility_Name;
                obj.MailTo = toMail;
                obj.MailCC = ccMail;
                obj.TimeIds = string.Join(",", timeIds);
                obj.TimeText = string.Join(", ", timesList);
                obj.NurseStation_Id = item.NursingStationId.ToString();
                obj.NursingStationNames = item.NursingStationName;
                gridData.Add(obj);
            }

            return gridData;
        }
        public int insertUserRecentCompany(int userId, int companyId)
        {
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join fac in this.dbContext.Facilities on us.Facility_id equals fac.Facility_Id
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1 && fac.Facility_Status==1 && fac.Company_Id==companyId
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();
            int facilityId = facility_nurseStationIds[0].FacilityId;
            List<int> nsIds= facility_nurseStationIds.Where(f=>f.FacilityId==facilityId).Select(f => f.NurseStationId).Distinct().ToList();
            if (nsIds.Count() > 0)
            {
                var selectedNurseStations = string.Join(",", nsIds);
                RecentFacEntity userRecentFacObj = new RecentFacEntity();
                userRecentFacObj.User_Id = userId;
                userRecentFacObj.Facility_Id = facilityId;
                userRecentFacObj.NurseStation_Id = selectedNurseStations;
                this.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            return 1;
        }
        public string  GetTimeZoneDate(int nursingStationId)
        {
            var result= this.dbContext.PrcGetTimeZoneDate(nursingStationId).FirstOrDefault();
            return result;
        }
        public DateTime GetNursingStationTimeZoneDateOrderByPatient(int patientId)
        {
            DataTable dt = new DataTable();
            string query = "[dbo].[GetTimeZoneConvertedDateTimeByPatientID]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                   
                    cmd.Parameters.Add("@Datetime", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var ordersList = (from d in dt.AsEnumerable()
                              select new 
                              {
                                  Date = Convert.ToDateTime(d["DateTime"]),
                                 
                              }).FirstOrDefault();

            return ordersList.Date;
        }
        public DateTime GetNursingStationTimeZoneDate()
        {
            DateTime currentDateTime = this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault();
            return currentDateTime;
            //if (nursingStationId != null)
            //{
            //    var result = this.dbContext.PrcGetTimeZoneDate(nursingStationId).FirstOrDefault();
            //    return result;
            //}
            //else if(porderId!=null)
            //{
            //    var residentId = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == porderId).FirstOrDefault().Patient_Id;
            //    var nsId = this.dbContext.VisitInfoes.Where(vi => vi.Patient_Id == residentId).ToList().OrderByDescending(vi => vi.PVisit_Id).FirstOrDefault().NursingStationId;
            //    var result = this.dbContext.PrcGetTimeZoneDate(nursingStationId).FirstOrDefault();
            //    return result;
            //}
            //else if(patientId!=null)
            //{
            //    var nsId = this.dbContext.VisitInfoes.Where(vi => vi.Patient_Id == patientId).ToList().OrderByDescending(vi => vi.PVisit_Id).FirstOrDefault().NursingStationId;
            //    var result = this.dbContext.PrcGetTimeZoneDate(nursingStationId).FirstOrDefault();
            //    return result;
            //}
            //return null;
        }
        public int inserUpdateMailconfigDetails(MailConfigEntity entity)
        {
            entity.Createddate = this.GetNursingStationTimeZoneDate();
            var record = this.autoMapper.Map<MailConfigEntity, MailConfig>(entity);
            var mailConfig = this.dbContext.MailConfigs.Find(1);
            if (mailConfig != null)
            {
                mailConfig.UserName = entity.UserName;
                mailConfig.Pwd = entity.Pwd;
                mailConfig.Port = entity.Port;
                mailConfig.Host = entity.Host;
                mailConfig.CreatedBy = entity.CreatedBy;
                mailConfig.Createddate = entity.Createddate;
                this.dbContext.SaveChanges();
            }
            else
            {
                this.dbContext.MailConfigs.Add(record);
                this.dbContext.SaveChanges();
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.MailConfig,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = entity.CreatedBy.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public int NoAdminitration(DrugAdministerEntity drugAdministerEntity)
        {
            try
            {
                var Dates = Convert.ToDateTime(drugAdministerEntity.AdminsterOn.Value.ToShortDateString() + " " + drugAdministerEntity.InputTime);
                var time = Dates.ToShortTimeString();
                var record = this.dbContext.DrugAdministers.Where(U => U.DrugAdminister_Id == drugAdministerEntity.DrugAdminister_Id && U.Porder_Id == drugAdministerEntity.POrder_Id && U.PQuantity_Id == drugAdministerEntity.pquantity_Id && U.is_deleted == null).FirstOrDefault();
                var res = this.dbContext.PrcNoAdministrationUpdate(drugAdministerEntity.DrugAdminister_Id, drugAdministerEntity.POrder_Id, drugAdministerEntity.pquantity_Id, drugAdministerEntity.AdditionalComments, drugAdministerEntity.AdminsterStatus, drugAdministerEntity.AdminsterBy, Dates, drugAdministerEntity.MedicationReason_ID, drugAdministerEntity.AdministerComment, drugAdministerEntity.Reason, drugAdministerEntity.ManualDocumentedBy, drugAdministerEntity.ManualDocumentedDate, drugAdministerEntity.ManualDocumentedBy, drugAdministerEntity.ManualDocumentedDate, time);
            }
            catch(Exception ex)
            {

            }
     
            //var record = this.dbContext.DrugAdministers.Where(U => U.DrugAdminister_Id == drugAdministerEntity.DrugAdminister_Id && U.Porder_Id == drugAdministerEntity.POrder_Id && U.PQuantity_Id == drugAdministerEntity.pquantity_Id).FirstOrDefault();
          //  var EnteredDates = record.AdminsterOn;
          //  var EnteredBys = record.AdminsterBy;
          //  var Dates = Convert.ToDateTime( drugAdministerEntity.AdminsterOn.Value.ToShortDateString() + " " + drugAdministerEntity.InputTime);
          //  drugAdministerEntity.AdminsterOn = Dates;
          //  record.additionalcomments = drugAdministerEntity.AdditionalComments==""?null : drugAdministerEntity.AdditionalComments;
          //  record.AdminsterStatus = drugAdministerEntity.AdminsterStatus;
          //  record.AdminsterBy = drugAdministerEntity.AdminsterBy;
          //  record.AdminsterOn = drugAdministerEntity.AdminsterOn;
          //  record.MedicationReason_ID = drugAdministerEntity.MedicationReason_ID;
            
           
          //  //if (drugAdministerEntity.MedicationReason_ID != 7)
          //  record.AdministerComment = drugAdministerEntity.AdministerComment == "" ? null : drugAdministerEntity.AdministerComment;
          //  this.dbContext.SaveChanges();
          //  var record1 = this.dbContext.DrugAdministers.Where(U => U.DrugAdminister_Id == drugAdministerEntity.DrugAdminister_Id && U.Porder_Id == drugAdministerEntity.POrder_Id && U.PQuantity_Id == drugAdministerEntity.pquantity_Id).FirstOrDefault();
            
           
          //  var drugtrans = this.dbContext.DocAdministerTrans.Where(U => U.DrugAdminister_Id == drugAdministerEntity.DrugAdminister_Id).ToList();
          //  if(drugtrans.Count == 0 &&  !string.IsNullOrEmpty( drugAdministerEntity.Reason ))
          //  {
          //      DocAdministerTran docObj = new DocAdministerTran();
          //      docObj.Dacd_Id = null;

          //      docObj.DrugAdminister_Id = record1.DrugAdminister_Id;
          //      docObj.ManualDocumentedBy = drugAdministerEntity.ManualDocumentedBy;
          //      docObj.ManualDocumentedDate = drugAdministerEntity.ManualDocumentedDate;
          //      docObj.EnteredBy = EnteredBys;
          //      docObj.EnteredDate = EnteredDates;
          //      //docObj.ManualDocumentedDate = item.ManualDocumentedDate;
          //      //docObj.ManualDocumentedDate = item.ManualDocumentedDate;
          //      docObj.AdminsterBy = record1.AdminsterBy;
          //      docObj.AdminsterOn = record1.AdminsterOn;
          //      docObj.Reason = drugAdministerEntity.Reason;
                

          //      this.dbContext.DocAdministerTrans.Add(docObj);
          //      this.dbContext.SaveChanges();
          //  }
          //  DocAdministerTran docObj1 = new DocAdministerTran();
          //  docObj1.Dacd_Id = null;
          //  var USR = this.dbContext.Users.Where(U => U.User_Id == drugAdministerEntity.AdminsterBy).FirstOrDefault();
        
          //  var MedicationReaosn = this.dbContext.MedicationReasons.Where(U => U.MedicationReason_ID == drugAdministerEntity.MedicationReason_ID).Select(U=>U.MedicationReason_Desc).FirstOrDefault();
          //  var UserRL = USR.User_Lname.Substring(0 , 1);
          //var UserRF =   USR.User_Fname.Substring(0 , 1);
          //  var UsersResonss = UserRL+UserRF +" "+Convert.ToString(  drugAdministerEntity.AdminsterOn.Value.ToString("h:mm tt"))+" "+ MedicationReaosn;
            

          //                     docObj1.DrugAdminister_Id = record1.DrugAdminister_Id;
          //  docObj1.ManualDocumentedBy = drugAdministerEntity.ManualDocumentedBy;
          //  docObj1.ManualDocumentedDate = drugAdministerEntity.ManualDocumentedDate;
          //  docObj1.EnteredBy = drugAdministerEntity.ManualDocumentedBy;
          //  docObj1.EnteredDate = DateTime.Now;
          //  //docObj.ManualDocumentedDate = item.ManualDocumentedDate;
          //  //docObj.ManualDocumentedDate = item.ManualDocumentedDate;
          //  docObj1.AdminsterBy = record1.AdminsterBy;
          //  docObj1.AdminsterOn = record1.AdminsterOn;
          //  docObj1.Reason = UsersResonss;

          //  this.dbContext.DocAdministerTrans.Add(docObj1);
          //  this.dbContext.SaveChanges();


          //  //  this.dbContext.SaveChanges();
          //  //else 
          //  if (drugAdministerEntity.MedicationReason_ID == 7)
          //  {
          //      //record.AdministerComment = string.Empty;
          //      NurseComment nurseComment = new NurseComment()
          //      {
          //          DrugAdminister_Id = record.DrugAdminister_Id,
          //          NurseCommentType_Id = 1,
          //          Comment = drugAdministerEntity.AdministerComment,
          //          comment_CreatedBy = drugAdministerEntity.AdminsterBy,
          //          Comment_CreatedOn = (DateTime)drugAdministerEntity.AdminsterOn,
          //          comment_Status = 1,
          //          Comments_Id = 0
          //      };
          //      this.dbContext.NurseComments.Add(nurseComment);

          //      this.dbContext.SaveChanges();
          //  }
       
            return 1;
        }

        public List<StateEntity> GetState()
        {
            DataTable dt = new DataTable();
            string query = "admin.GetStates";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    // cmd.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
                    // cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateValues3;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var Data = (from d in dt.AsEnumerable()
                        select new StateEntity
                        {
                            State_Id = Convert.ToInt16(d["State_Id"]),
                            State_Name = Convert.ToString(d["State_Name"]),
                            State_Code = Convert.ToString(d["State_Code"])
                        }).ToList();

            return Data;
        }

        public List<PrimarySpeciality> GetPrimarySpeciality()
        {
            DataTable dt = new DataTable();
            string query = "[DrFirst].[PrcGetPrimarySpeciality]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    // cmd.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
                    // cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateValues3;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var Data = (from d in dt.AsEnumerable()
                        select new PrimarySpeciality
                        {
                            Id = Convert.ToInt32(d["PSpeciality_Id"]),
                            Name = Convert.ToString(d["Speciality"])
                        }).ToList();

            return Data;
        }
        public List<CredentialMaster> GetCredentialMaster()
        {
            DataTable dt = new DataTable();
            string query = "[DrFirst].[PrcGetCredentialMaster]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    // cmd.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
                    // cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateValues3;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var Data = (from d in dt.AsEnumerable()
                        select new CredentialMaster
                        {
                            Id = Convert.ToInt32(d["Credential_Id"]),
                            Name = Convert.ToString(d["Value"])
                        }).ToList();

            return Data;
        }

    }
}
