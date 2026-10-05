using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;
using System.Web;
using System.Security.Claims;

namespace LTCPro.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IResidentDemographicRepository _residentDemographicRepository;
        private readonly ICommonRepository _commonRepository;
        public CompanyRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, ResidentDemographicRepository residentDemographicRepository, ICommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
            this._residentDemographicRepository = residentDemographicRepository;
            this._commonRepository = commonRepository;
        }
        public int GetApprovalFlag(int patientId)
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
            if (facilityId != 0)
            {
                int companyId = this.dbContext.Facilities.Where(p => p.Facility_Id == facilityId).Select(p => p.Company_Id).FirstOrDefault();
                return (int)this.dbContext.CompanyConfigs.Where(p => p.Company_Id == companyId).Select(p => p.ApprovalFlag).FirstOrDefault();
            }
            else
                return 0;
        }
        public int GetApprovalFlagByFacilityId(int facilityId)
        {
            if (facilityId != 0)
            {
                int companyId = this.dbContext.Facilities.Where(p => p.Facility_Id == facilityId).Select(p => p.Company_Id).FirstOrDefault();
                return (int)this.dbContext.CompanyConfigs.Where(p => p.Company_Id == companyId).Select(p => p.ApprovalFlag).FirstOrDefault();
            }
            else
                return 0;
        }
        public List<CompanyCustomEntity> GetUserActiveCompanyMasterList()
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

                        //Int32 PatientTotalCount = (this.dbContext.VisitInfoes.Where(vs => facilityIds.Contains(vs.FacilityId.Value)).Select(vs => vs.Patient_Id).Distinct().Count());

                        return (from company in this.dbContext.Companies
                                join country in this.dbContext.Countries on company.Company_CountryId equals country.Country_Id
                                join user in this.dbContext.Users on company.Company_CreatedBy equals user.User_Id
                                where company.Company_Status == 1 && companyIds.Contains(company.Company_Id)
                                select new CompanyCustomEntity
                                {
                                    Company_Id = company.Company_Id,
                                    Company_Name = company.Company_Name,
                                    Company_Addr1 = company.Company_Addr1,
                                    Company_Addr2 = company.Company_Addr2,
                                    Company_Zip = company.Company_Zip,
                                    Company_CountryId = company.Company_CountryId,
                                    Company_Fax = company.Company_Fax,
                                    Company_Email = company.Company_Email,
                                    Com_ContactPerson = company.Com_ContactPerson,
                                    Com_ContactPhone = company.Com_ContactPhone,
                                    Company_EIN = company.Company_EIN,
                                    Company_Logo = company.Company_Logo,
                                    Company_Status = company.Company_Status,
                                    Company_CreatedBy = company.Company_CreatedBy,
                                    Company_CreatedDate = company.Company_CreatedDate,
                                    UserName = user.UserName,
                                    ApprovalFlag = company.ApprovalFlag == null ? 0 : (int)company.ApprovalFlag,
                                    TimeFormat = company.TimeFormat,
                                    PatientCount = (from vs in this.dbContext.VisitInfoes
                                                    join cm in this.dbContext.Facilities on vs.FacilityId equals cm.Facility_Id
                                                    where cm.Company_Id == company.Company_Id
                                                    select new { vs.Patient_Id}).Count()
                                }).ToList();
                    }
                }
            }
            return null;
        }
        public List<HLDirectionEntity> GetHLDirectionList()
        {
            var hlDesc = this.dbContext.HLDirectionalWays.Where(di => di.HLDirectionalWay_Status == 1).ToList();
            return this.autoMapper.Map<List<HLDirectionalWay>, List<HLDirectionEntity>>(hlDesc);
        }
        public List<FTECategoryEntity> GetAllFTECategory()
        {
            var category = this.dbContext.FTECategories.Where(fi => fi.FteCategory_Status == 1).ToList();
            return this.autoMapper.Map<List<FTECategory>, List<FTECategoryEntity>>(category);
        }
        public List<EventCategoryEntity> GetAllEventCategroy(int directionalId, int categoryId)
        {
            List<EventCategory> category = new List<EventCategory>();
            EventCategory categoryObj = new EventCategory();
            if (directionalId == 0 && categoryId == 0)
            {
                var records = this.dbContext.EventCategories.Where(ev => ev.EventCat_Status == 1).ToList();
                foreach (var item in records)
                {
                    categoryObj = new EventCategory();
                    categoryObj.EventCat_Id = item.EventCat_Id;
                    categoryObj.Category_Id = item.Category_Id;
                    categoryObj.EventCat_Code = item.EventCat_Code;
                    categoryObj.EventCat_Desc = item.Category_Id == 1 ? item.EventCat_Desc + " (Inbound)" : item.EventCat_Desc + " (Outbound)";
                    categoryObj.EventCat_ShortCode = item.EventCat_ShortCode;
                    categoryObj.EventCat_Type = item.EventCat_Type;
                    categoryObj.EventCat_Status = item.EventCat_Status;
                    categoryObj.EventCat_CreatedBy = item.EventCat_CreatedBy;
                    categoryObj.EventCat_CreatedDate = item.EventCat_CreatedDate;
                    category.Add(categoryObj);

                }
            }
            else if (directionalId == 1)
            {
                if (categoryId == 1)
                {
                    category = this.dbContext.EventCategories.Where(ev => ev.EventCat_Status == 1 && ev.Category_Id == 1).ToList();
                }
                if (categoryId == 2)
                {
                    category = this.dbContext.EventCategories.Where(ev => ev.EventCat_Status == 1 && ev.Category_Id == 2).ToList();
                }
            }
            else if (directionalId == 2)
            {
                if (categoryId == 1)
                {
                    category = this.dbContext.EventCategories.Where(ev => ev.EventCat_Status == 1 && ev.Category_Id == 1).ToList();
                }
                else if (categoryId == 2)
                {
                    category = this.dbContext.EventCategories.Where(ev => ev.EventCat_Status == 1 && ev.Category_Id == 2).ToList();
                }
                else if (categoryId == 0)
                {
                    var records = this.dbContext.EventCategories.Where(ev => ev.EventCat_Status == 1).ToList();
                    foreach (var item in records)
                    {
                        categoryObj = new EventCategory();
                        categoryObj.EventCat_Id = item.EventCat_Id;
                        categoryObj.Category_Id = item.Category_Id;
                        categoryObj.EventCat_Code = item.EventCat_Code;
                        categoryObj.EventCat_Desc = item.Category_Id == 1 ? item.EventCat_Desc + " (Inbound)" : item.EventCat_Desc + " (Outbound)";
                        categoryObj.EventCat_ShortCode = item.EventCat_ShortCode;
                        categoryObj.EventCat_Type = item.EventCat_Type;
                        categoryObj.EventCat_Status = item.EventCat_Status;
                        categoryObj.EventCat_CreatedBy = item.EventCat_CreatedBy;
                        categoryObj.EventCat_CreatedDate = item.EventCat_CreatedDate;
                        category.Add(categoryObj);

                    }
                }
            }
            return this.autoMapper.Map<List<EventCategory>, List<EventCategoryEntity>>(category);
        }
        public List<CompanyCustomEntity> GetAllCompanyMasterList()
        {
            return (from company in this.dbContext.Companies
                    join country in this.dbContext.Countries on company.Company_CountryId equals country.Country_Id
                    join user in this.dbContext.Users on company.Company_CreatedBy equals user.User_Id
                    // join finger in this.dbContext.Fingersdescs on company.Fingersdesc_Id equals finger.Fingersdesc_Id

                    select new CompanyCustomEntity
                    {
                        Company_Id = company.Company_Id,
                        Company_Name = company.Company_Name,
                        Company_Addr1 = company.Company_Addr1,
                        Company_Addr2 = company.Company_Addr2,
                        Company_Zip = company.Company_Zip,
                        Company_CountryId = company.Company_CountryId,
                        Company_Phone = company.Company_Phone,
                        Company_Fax = company.Company_Fax,
                        Company_Email = company.Company_Email,
                        Com_ContactPerson = company.Com_ContactPerson,
                        Com_ContactPhone = company.Com_ContactPhone,
                        Company_EIN = company.Company_EIN,
                        Company_Logo = company.Company_Logo,
                        Company_Status = company.Company_Status,
                        Company_CreatedBy = company.Company_CreatedBy,
                        Company_CreatedDate = company.Company_CreatedDate,
                        Company_City = company.Company_City,
                        Company_State = company.Company_State,
                        UserName = user.User_DisplayName,
                        ApprovalFlag = company.ApprovalFlag == null ? 0 : (int)company.ApprovalFlag,
                        // FingerDesc = finger.FingersDesc1,
                        TimeFormat = company.TimeFormat,
                        Company_UniqueId = company.Company_UniqueId,
                    }).OrderBy(item => item.Company_Name).ToList();

        }
        public int UpdateCompaniesStatus(List<CompanyCustomEntity> data)
        {
            foreach (var item in data)
            {
                item.Company_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                var record = this.dbContext.Companies.Find(item.Company_Id);
                if (record != null)
                {
                    record.Company_Status = record.Company_Status == 1 ? 0 : 1;
                    record.Company_CreatedBy = item.Company_CreatedBy;
                    record.Company_CreatedDate = item.Company_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }



        public CompanyCustomEntity GetCompanyDetailsByID(int companyId)
        {
            var record= (from company in this.dbContext.Companies
                    join country in this.dbContext.Countries on company.Company_CountryId equals country.Country_Id
                    join user in this.dbContext.Users on company.Company_CreatedBy equals user.User_Id
                    where company.Company_Id == companyId
                    select new CompanyCustomEntity
                    {
                        Company_Id = company.Company_Id,
                        Company_Name = company.Company_Name,
                        Company_Addr1 = company.Company_Addr1,
                        Company_Addr2 = company.Company_Addr2,
                        Company_Zip = company.Company_Zip,
                        Company_CountryId = company.Company_CountryId,
                        Company_Phone = company.Company_Phone,
                        Company_Fax = company.Company_Fax,
                        Company_Email = company.Company_Email,
                        Com_ContactPerson = company.Com_ContactPerson,
                        Com_ContactPhone = company.Com_ContactPhone,
                        Company_EIN = company.Company_EIN,
                        Company_Logo = company.Company_Logo,                        
                        Company_UniqueId = company.Company_UniqueId,
                        Company_Status = company.Company_Status,
                        Company_CreatedBy = company.Company_CreatedBy,
                        Company_CreatedDate = company.Company_CreatedDate,
                        Country_Code = country.Country_Name,
                        UserName = user.UserName,
                        Company_State = company.Company_State,
                        Company_City = company.Company_City,
                        // ApprovalFlag = company.ApprovalFlag == null ? 0 : (int)company.ApprovalFlag,
                        // Fingersdesc_Id = (int)company.Fingersdesc_Id,
                        // TimeFormat=company.TimeFormat
                    }).FirstOrDefault();
            var record1 = this.dbContext.FooterLogoes.Where(f => f.Footer_Status == 1).FirstOrDefault();
            record.Company_FooterLogo = record1.Footer_Image;
            return record;
        }

        public int InsertComapnyConfigDetails(CompanyConfigEntity entity)
        {
            if (this.dbContext.CompanyConfigs.Any(e => e.Company_Id == entity.Company_Id && entity.CompanyConfig_Id == 0))
            {
                //Company already configured
                return 2;
            }
            var entityRecord = this.autoMapper.Map<CompanyConfigEntity, CompanyConfig>(entity);
            int record = this.dbContext.PrcInsertUpdateCompanyConfig(entity.CompanyConfig_Id, entity.Company_Id, entity.ApprovalFlag, entity.Fingersdesc_Id, entity.TimeFormat, entity.StockReport_Id, entity.DrFirstRequired, entity.Hl7Configured, entity.HLDirectionalWay_Id, entity.Category, entity.Events, entity.CompanyConfig_Status, entity.CompanyConfig_CreatedBy);
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.CompanyConfiguration,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0
            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;

        }
        public List<CompanyConfigEntity> GetAllCompanyConfigList()
        {
            List<CompanyConfigEntity> companyConfigList = new List<CompanyConfigEntity>();

            var records = (from cc in this.dbContext.CompanyConfigs
                           join c in this.dbContext.Companies on cc.Company_Id equals c.Company_Id
                           join p in this.dbContext.PhysicianDetails on cc.Physician_Id equals p.Physician_Id into phy
                           from ph in phy.DefaultIfEmpty()
                           join stk in this.dbContext.StockReports on cc.StockReport_Id equals stk.StockReport_Id into stock
                           from st in stock.DefaultIfEmpty()
                           join f in this.dbContext.Fingersdescs on cc.Fingersdesc_Id equals f.Fingersdesc_Id into fing
                           from fi in fing.DefaultIfEmpty()
                           join h in this.dbContext.HLDirectionalWays on cc.HLDirectionalWay_Id equals h.HLDirectionalWay_Id into hl
                           from hld in hl.DefaultIfEmpty()
                           select new CompanyConfigEntity
                           {
                               Company_Id = c.Company_Id,
                               CompanyConfig_Id = cc.CompanyConfig_Id,
                               Company_Name = c.Company_Name,
                               FingersDesc1 = fi.FingersDesc1,
                               Hl7Configured = cc.Hl7Configured,
                               ApprovalFlag = cc.ApprovalFlag,
                               TimeFormat = cc.TimeFormat,
                               HLDirectionalWaysDesc = hld.HLDirectionalWaysDesc,
                               StockReport_Id = cc.StockReport_Id,
                               StockReportFor = st.StockReportFor,
                               //Physician_Id=cc.Physician_Id,
                               PhysicianFullFName = ph.PhysicianFName == null && ph.PhysicianLName == null ? "" : ph.PhysicianLName + ", " + ph.PhysicianFName,
                               DrFirstRequired = cc.DrFirstRequired,
                               Company_Status = c.Company_Status,
                           }).ToList();
            for (int i = 0; i < records.Count(); i++)
            {
                if (records[i].Hl7Configured == 1)
                {
                    int fte = 0;
                    List<string> eventList = new List<string>();
                    int cmpId = (int)records[i].Company_Id;
                    int cmpConfig = this.dbContext.CompanyConfigs.Where(c => c.Company_Id == cmpId).Select(c => c.CompanyConfig_Id).FirstOrDefault();
                    var hlCatgs = this.dbContext.CompanyHlCategories.Where(br => br.CompanyConfig_Id == cmpConfig).ToList().Distinct();
                    if (hlCatgs.Count() > 0)
                    {
                        foreach (var item in hlCatgs)
                        {
                            eventList = new List<string>();
                            fte = (int)item.FteCategory_Id;
                            int catg = item.CompanyHlCategory_Id;
                            var catgeries = this.dbContext.CompanyHLEvents.Where(ce => ce.CompanyHlCategory_Id == catg).ToList().Distinct();
                            foreach (var items in catgeries)
                            {
                                int eventId = (int)items.EventCat_Id;
                                var eventDesc = this.dbContext.EventCategories.Where(e => e.EventCat_Id == eventId && e.Category_Id == fte).FirstOrDefault();
                                if (eventDesc != null)
                                    eventList.Add(eventDesc.EventCat_Desc);
                            }
                            CompanyConfigEntity data = new CompanyConfigEntity();
                            data.Company_Id = records[i].Company_Id;
                            data.CompanyConfig_Id = records[i].CompanyConfig_Id;
                            data.Company_Name = records[i].Company_Name;
                            data.Hl7Configured = records[i].Hl7Configured;
                            data.FingersDesc1 = records[i].FingersDesc1;
                            data.ApprovalFlag = records[i].ApprovalFlag;
                            data.TimeFormat = records[i].TimeFormat;
                            data.StockReportFor = records[i].StockReportFor;
                            data.HLDirectionalWaysDesc = records[i].HLDirectionalWaysDesc;
                            data.FteCategory_Id = fte;
                            //  data.Physician_Id = records[i].Physician_Id;
                            data.PhysicianFullFName = records[i].PhysicianFullFName;
                            data.DrFirstRequired = records[i].DrFirstRequired;
                            data.Company_Status = records[i].Company_Status;
                            data.FteCategory_Desc = this.dbContext.FTECategories.Where(f => f.FteCategory_Id == fte).Select(f => f.FteCategory_Desc).FirstOrDefault();
                            data.EventCat_Desc = string.Join(",", eventList);
                            companyConfigList.Add(data);
                        }
                    }
                }
                else if (records[i].Hl7Configured == 0)
                {
                    CompanyConfigEntity data = new CompanyConfigEntity();
                    data.Company_Id = records[i].Company_Id;
                    data.CompanyConfig_Id = records[i].CompanyConfig_Id;
                    data.Company_Name = records[i].Company_Name;
                    data.Hl7Configured = records[i].Hl7Configured;
                    data.FingersDesc1 = records[i].FingersDesc1;
                    data.ApprovalFlag = records[i].ApprovalFlag;
                    data.TimeFormat = records[i].TimeFormat;
                    data.StockReportFor = records[i].StockReportFor;
                    data.HLDirectionalWaysDesc = records[i].HLDirectionalWaysDesc;
                    data.FteCategory_Id = 0;
                    data.FteCategory_Desc = "";
                    data.EventCat_Desc = "";
                    //data.Physician_Id = records[i].Physician_Id;
                    data.PhysicianFullFName = records[i].PhysicianFullFName;
                    data.DrFirstRequired = records[i].DrFirstRequired;
                    data.Company_Status = records[i].Company_Status;
                    companyConfigList.Add(data);
                }
            }
            return companyConfigList.OrderBy(item => item.Company_Name).ToList();
        }


        public int InsertUpdateCompanyMaster(CompanyEntity entity)
        {
            entity.Company_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<CompanyEntity, Company>(entity);
            var footerlogo = this.dbContext.FooterLogoes.Where(a => a.Footer_Status == 1).FirstOrDefault();           

                if (record.Company_Id == 0)
            {
                if (this.dbContext.Companies.Any(e => e.Company_Name == entity.Company_Name))
                {
                    //CompanyName already exisits
                    return 2;
                }
                else
                {
                    if (footerlogo != null)
                    {
                        footerlogo.Footer_Image = entity.Company_FooterLogo;
                    }
                    else
                    {
                        footerlogo.Footer_Image = entity.Company_FooterLogo;
                        footerlogo.Footer_Status = 1;
                        footerlogo.Footer_CreatedBy = entity.Company_CreatedBy;
                        footerlogo.Footer_CreatedOn = entity.Company_CreatedDate;
                        this.dbContext.FooterLogoes.Add(footerlogo);
                    }                           
                    record.Company_FooterLogo = entity.Company_FooterLogo;
                    this.dbContext.Companies.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.CompanyMaster,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.Company_Id.ToString(),
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
                if (this.dbContext.Companies.Any(e => e.Company_Name == entity.Company_Name && e.Company_Id != entity.Company_Id))
                {
                    //CompanyName already exisits
                    return 2;
                }
                else
                {

                    if (record.Company_FooterLogo.Length > 0)
                        footerlogo.Footer_Image = entity.Company_FooterLogo;
                    footerlogo.Footer_Status = 1;
                    footerlogo.Footer_CreatedBy = entity.Company_CreatedBy;
                    footerlogo.Footer_CreatedOn = entity.Company_CreatedDate;
                    Company company = this.dbContext.Companies.Find(entity.Company_Id);
                    company.Company_Name = record.Company_Name;
                    company.Company_Addr1 = record.Company_Addr1;
                    company.Company_Addr2 = record.Company_Addr2;
                    company.Company_Zip = record.Company_Zip;
                    //company.Company_CityId = record.Company_CityId;
                    //company.Company_StateId = record.Company_StateId;
                    company.Company_CountryId = record.Company_CountryId;
                    company.Company_Phone = record.Company_Phone;
                    company.Company_Fax = record.Company_Fax;
                    company.Company_Email = record.Company_Email;
                    company.Com_ContactPerson = record.Com_ContactPerson;
                    company.Com_ContactPhone = record.Com_ContactPhone;
                    company.Company_EIN = record.Company_EIN;
                    if (record.Company_Logo.Length > 0)
                        company.Company_Logo = record.Company_Logo;

                    if (record.Company_FooterLogo.Length > 0)
                        company.Company_FooterLogo = entity.Company_FooterLogo;


                    company.Company_UniqueId = record.Company_UniqueId;
                    company.Company_Status = record.Company_Status;
                    company.ApprovalFlag = record.ApprovalFlag;
                    company.Fingersdesc_Id = record.Fingersdesc_Id;
                    company.Company_CreatedBy = record.Company_CreatedBy;
                    company.Company_CreatedDate = record.Company_CreatedDate;
                    company.TimeFormat = record.TimeFormat;
                    company.Company_City = record.Company_City;
                    company.Company_State = record.Company_State;

                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.CompanyMaster,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = record.Company_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0
                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    this.dbContext.SaveChanges();
                    return 1;
                }
            }
        }
        public string GetCompanyNameById(int companyId)
        {
            return this.dbContext.Companies.Find(companyId).Company_Name;
        }
        public List<CompanyDropEntity> GetUserActiveCompanyNames()
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

                        return this.dbContext.Companies.Where(c => c.Company_Status == 1 && companyIds.Contains(c.Company_Id)).Select(c => new CompanyDropEntity { Company_Id = c.Company_Id, Company_Name = c.Company_Name }).OrderBy(c => c.Company_Name).ToList();
                    }

                }
            }
            return null;
        }
        public List<CompanyDropEntity> GetAllActiveCompanyNames()
        {
            return this.dbContext.Companies.Where(c => c.Company_Status == 1).Select(c => new CompanyDropEntity { Company_Id = c.Company_Id, Company_Name = c.Company_Name }).OrderBy(item => item.Company_Name).ToList();
        }
        public List<CompanyUIDDropEntity> GetCompanyUIDDropDownList()
        {
            List<CompanyUIDDropEntity> entityList = new List<CompanyUIDDropEntity>();
            entityList.Add(new CompanyUIDDropEntity() { UIDText = "ExternalPatientId", UIDValue = "ExternalPatientId" });
            //entityList.Add(new CompanyUIDDropEntity() { UIDText = "ExternalFacPatientId", UIDValue = "ExternalFacPatientId" });
            //entityList.Add(new CompanyUIDDropEntity() { UIDText = "AlternatePatientId", UIDValue = "AlternatePatientId" });
            entityList.Add(new CompanyUIDDropEntity() { UIDText = "PatientMRNumber", UIDValue = "PatientMRNumber" });
            //entityList.Add(new CompanyUIDDropEntity() { UIDText = "SSN", UIDValue = "SSN" });
            //            var columnNames = new List
            //typeof(Demographic).GetProperties()
            //                             .Select(property => property.Name)
            //                             .ToList();
            //foreach (string colName in columnNames)
            //{
            //    entityList.Add(new CompanyUIDDropEntity() { UIDText = colName, UIDValue = colName });
            //}
            return entityList;
        }
        public List<FingersdescEntity> GetFingersDescDrop()
        {
            var fingersDesc = this.dbContext.Fingersdescs.Where(fi => fi.Fingersdesc_Status == 1).ToList();
            return this.autoMapper.Map<List<Fingersdesc>, List<FingersdescEntity>>(fingersDesc);
        }
        public CompanyConfigEntity GetCompanyConfigById(int companyConfigId, int hl7Configured, int FteCategoryId)
        {
            CompanyConfigEntity data = new CompanyConfigEntity();
            if (hl7Configured == 0)
            {
                var cmpConfig = this.dbContext.CompanyConfigs.Where(c => c.CompanyConfig_Id == companyConfigId).FirstOrDefault();

                data.Company_Id = cmpConfig.Company_Id;
                data.CompanyConfig_Id = cmpConfig.CompanyConfig_Id;
                data.Fingersdesc_Id = cmpConfig.Fingersdesc_Id;
                data.ApprovalFlag = cmpConfig.ApprovalFlag;
                data.TimeFormat = cmpConfig.TimeFormat;
                data.Hl7Configured = cmpConfig.Hl7Configured;
                data.HLDirectionalWay_Id = cmpConfig.HLDirectionalWay_Id;
                data.StockReport_Id = cmpConfig.StockReport_Id;
                data.DrFirstRequired = cmpConfig.DrFirstRequired;
                //  data.Physician_Id = cmpConfig.Physician_Id;
                data.FteCategory_Id = 0;
                data.Events = "";
                return data;
            }
            else
            {
                List<int> eventList = new List<int>();
                var cmpConfig = this.dbContext.CompanyConfigs.Where(c => c.CompanyConfig_Id == companyConfigId).FirstOrDefault();
                var hlCatgs = this.dbContext.CompanyHlCategories.Where(br => br.CompanyConfig_Id == companyConfigId && br.FteCategory_Id == FteCategoryId).FirstOrDefault();
                eventList = new List<int>();
                int catg = hlCatgs.CompanyHlCategory_Id;
                var list = this.dbContext.CompanyHLEvents.Where(ce => ce.CompanyHlCategory_Id == catg).Select(ce => ce.EventCat_Id).ToList();
                foreach (var item in list)
                {
                    int EventCatId = (int)item;
                    var eventId = this.dbContext.EventCategories.Where(e => e.EventCat_Id == EventCatId && e.Category_Id == FteCategoryId).FirstOrDefault();
                    if (eventId != null)
                        eventList.Add(eventId.EventCat_Id);
                }
                data.Company_Id = cmpConfig.Company_Id;
                data.CompanyConfig_Id = cmpConfig.CompanyConfig_Id;
                data.Fingersdesc_Id = cmpConfig.Fingersdesc_Id;
                data.Hl7Configured = cmpConfig.Hl7Configured;
                data.ApprovalFlag = cmpConfig.ApprovalFlag;
                data.TimeFormat = cmpConfig.TimeFormat;
                data.HLDirectionalWay_Id = cmpConfig.HLDirectionalWay_Id;
                data.StockReport_Id = cmpConfig.StockReport_Id;
                data.DrFirstRequired = cmpConfig.DrFirstRequired;
                // data.Physician_Id = cmpConfig.Physician_Id;
                data.FteCategory_Id = FteCategoryId;
                data.Events = string.Join(",", eventList);
                return data;
            }
        }
        public List<EventCategoryEntity> GetCompanyEventCategoriesByPid(int patientId)
        {
            List<EventCategoryEntity> eventList = new List<EventCategoryEntity>();
            int nursestationId = this._residentDemographicRepository.GetNurseStationByPId(patientId);
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nursestationId).Select(n => n.Facility_Id).FirstOrDefault();
            int companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Company_Id).FirstOrDefault();
            var cmpConfig = this.dbContext.CompanyConfigs.Where(c => c.Company_Id == companyId && c.Hl7Configured == 1).FirstOrDefault();
            if (cmpConfig != null)
            {
                var hlCatgs = this.dbContext.CompanyHlCategories.Where(br => br.CompanyConfig_Id == cmpConfig.CompanyConfig_Id && br.FteCategory_Id == 2).FirstOrDefault();
                if (hlCatgs != null)
                {
                    int catg = hlCatgs.CompanyHlCategory_Id;
                    var list = this.dbContext.CompanyHLEvents.Where(ce => ce.CompanyHlCategory_Id == catg).Select(ce => ce.EventCat_Id).ToList();
                    foreach (var item in list)
                    {
                        int eventId = (int)item;
                        var record = (from ev in this.dbContext.EventCategories
                                      where ev.EventCat_Id == eventId
                                      select new EventCategoryEntity
                                      {
                                          EventCat_Id = ev.EventCat_Id,
                                          Category_Id = ev.Category_Id,
                                          EventCat_Code = ev.EventCat_Code,
                                          EventCat_Desc = ev.EventCat_Desc,
                                          EventCat_ShortCode = ev.EventCat_ShortCode,
                                          EventCat_Type = ev.EventCat_Type,
                                          EventCat_Status = ev.EventCat_Status,
                                          EventCat_CreatedBy = ev.EventCat_CreatedBy,
                                          EventCat_CreatedDate = ev.EventCat_CreatedDate,

                                      }).FirstOrDefault();
                        eventList.Add(record);
                    }
                    return eventList;
                }
            }
            return eventList;
        }
        public List<StockReportEntity> GetStockReportForDropData()
        {
            var stockreports = this.dbContext.StockReports.ToList();
            return this.autoMapper.Map<List<StockReport>, List<StockReportEntity>>(stockreports);
        }
        public int? GetBiometricFingerConfigByNsId(int nurseStationId)
        {
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
            int companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Company_Id).FirstOrDefault();
            return this.dbContext.CompanyConfigs.Where(c => c.Company_Id == companyId).Select(c => c.Fingersdesc_Id).FirstOrDefault();
        }
        public List<CompanyDropEntity> GetStockUserCompanyDrop(int userId)
        {
            var list = this.dbContext.PrcGetCompanyStockReport(userId).OrderBy(item => item.Company_Id).ToList();
            return this.autoMapper.Map<List<PrcGetCompanyStockReport_Result>, List<CompanyDropEntity>>(list);
        }
        public CompanyFlagsEntity GetAllFlagsForCompanyByNSId(int nurseStationId,int? residentId=0)
        {
            var result = (from ns in this.dbContext.NursingStations
                          join f in this.dbContext.Facilities on ns.Facility_Id equals f.Facility_Id                          
                          join cc in this.dbContext.CompanyConfigs on f.Company_Id equals cc.Company_Id
                          where ns.NurseStation_Id == nurseStationId
                          select new CompanyFlagsEntity()
                          {
                              ApprovalFlag = cc.ApprovalFlag,
                              DrFirstRequired = cc.DrFirstRequired,
                              Fingersdesc_Id = cc.Fingersdesc_Id,
                              Physician_Id = ns.DefaultPhysician_Id,
                              PhysicianNPI = residentId==0 || (this.dbContext.VisitInfoes.Where(p => p.Patient_Id == residentId).Select(p => p.PrimaryPhysicianNPI).FirstOrDefault()==null) ? (ns.DefaultPhysician_Id != null && ns.DefaultPhysician_Id != 0) ? this.dbContext.PhysicianDetails.Where(e => e.Physician_Id == ns.DefaultPhysician_Id).Select(e => e.PhysicianNPI).FirstOrDefault() : null:this.dbContext.VisitInfoes.Where(p=>p.Patient_Id==residentId).Select(p=>p.PrimaryPhysicianNPI).FirstOrDefault(),
                              StockReport_Id = cc.StockReport_Id,
                              TimeFormat = cc.TimeFormat
                          }).FirstOrDefault();
            return result;

        }
        public int GetCompanyHlSevenFlag(int fcailityId, int companyId)
        {
            if (fcailityId == 0)
            {
                var record = this.dbContext.CompanyConfigs.Where(co => co.Company_Id == companyId).FirstOrDefault();
                if (record != null)
                {
                    return (int)record.Hl7Configured;
                }
            }
            if (companyId == 0)
            {

                var companyID = this.dbContext.Facilities.Where(f => f.Facility_Id == fcailityId).Select(f => f.Company_Id).FirstOrDefault();
                var record = this.dbContext.CompanyConfigs.Where(co => co.Company_Id == companyID).FirstOrDefault();
                if (record != null)
                {
                    return (int)record.Hl7Configured;
                }
            }
            return 0;
        }
        public int InsertUpdateNurseStationhierarchyMaster(NurseStationHierarchyEntity hierarchyentity)
        {
            hierarchyentity.CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<NurseStationHierarchyEntity, NurseStationHierarchy>(hierarchyentity);
            var data = this.dbContext.NurseStationHierarchies.Where(pt =>pt.NurseStation_Id==record.NurseStation_Id).FirstOrDefault();
            if (data == null)
            {
                this.dbContext.NurseStationHierarchies.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.CompanyBedHierarchy,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            else
            {
                NurseStationHierarchy nsdata = this.dbContext.NurseStationHierarchies.Find(data.NSHierarchy_Id);
                nsdata.FloorPrior = record.FloorPrior;
                nsdata.WingPrior = record.WingPrior;
                nsdata.RoomPrior = record.RoomPrior;
                nsdata.BedPrior = record.BedPrior;
                nsdata.CreatedBy = record.CreatedBy;
                nsdata.CreatedDate = record.CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.CompanyBedHierarchy,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            
            return 1;
        }
        public List<NurseStationHierarchyEntity> GetCompanyToBedMapGrid(int? facilityId = null, int? NsId=null)
       {
            List<NurseStationHierarchyEntity> hierarchies=new List<NurseStationHierarchyEntity>();

            if (facilityId ==null && NsId == null)
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                    var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                                    where us.User_Id == userId
                                                    select new
                                                    {
                                                        NurseStationId = ns.NurseStation_Id,
                                                        FacilityId = us.Facility_id
                                                    }).Distinct().ToList();

                    List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
                    List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();
                    hierarchies = (from hi in this.dbContext.NurseStationHierarchies
                                   join ns in this.dbContext.NursingStations on hi.NurseStation_Id equals ns.NurseStation_Id
                                   join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                                   join com in this.dbContext.Companies on fa.Company_Id equals com.Company_Id
                                   where  ns.NurseStation_Status==1
                                   //&& stationIds.Contains((int)hi.NurseStation_Id)
                                   select new NurseStationHierarchyEntity
                                   {
                                       NSHierarchy_Id=hi.NSHierarchy_Id,
                                       CompanyName = com.Company_Name,
                                       FacilityName=fa.Facility_Name,
                                       NursingStationName=ns.NurseStation_Name,
                                       Hierarchy = "",
                                       FloorPrior = hi.FloorPrior == null ? 0 : hi.FloorPrior,
                                       WingPrior = hi.WingPrior == null ? 0 : hi.WingPrior,
                                       RoomPrior = hi.RoomPrior == null ? 0 : hi.RoomPrior,
                                       BedPrior = hi.BedPrior == null ? 0 : hi.BedPrior,

                                   }).OrderBy(item=> item.CompanyName).ThenBy(item=>item.FacilityName).ThenBy(item=>item.NursingStationName).ToList();
                }
            }
            else if(facilityId != null && NsId == null)
            {

                hierarchies = (from hi in this.dbContext.NurseStationHierarchies
                               join ns in this.dbContext.NursingStations on hi.NurseStation_Id equals ns.NurseStation_Id
                               join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                               join com in this.dbContext.Companies on fa.Company_Id equals com.Company_Id
                               where ns.Facility_Id==facilityId && ns.NurseStation_Status == 1
                               select new NurseStationHierarchyEntity
                               {
                                   NSHierarchy_Id = hi.NSHierarchy_Id,
                                   CompanyName = com.Company_Name,
                                   FacilityName = fa.Facility_Name,
                                   NursingStationName = ns.NurseStation_Name,
                                   Hierarchy = "",
                                   FloorPrior = hi.FloorPrior == null ? 0 : hi.FloorPrior,
                                   WingPrior = hi.WingPrior == null ? 0 : hi.WingPrior,
                                   RoomPrior = hi.RoomPrior == null ? 0 : hi.RoomPrior,
                                   BedPrior = hi.BedPrior == null ? 0 : hi.BedPrior,
                               }).OrderBy(item => item.CompanyName).ThenBy(item => item.FacilityName).ThenBy(item => item.NursingStationName).ToList();
            }
            else if(facilityId!=null && NsId!=null)
            {
                hierarchies = (from hi in this.dbContext.NurseStationHierarchies
                               join ns in this.dbContext.NursingStations on hi.NurseStation_Id equals ns.NurseStation_Id
                               join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                               join com in this.dbContext.Companies on fa.Company_Id equals com.Company_Id
                               where hi.NurseStation_Id == NsId && ns.NurseStation_Status==1
                               select new NurseStationHierarchyEntity
                               {
                                   NSHierarchy_Id = hi.NSHierarchy_Id,
                                   CompanyName = com.Company_Name,
                                   FacilityName = fa.Facility_Name,
                                   NursingStationName = ns.NurseStation_Name,
                                   Hierarchy = "",
                                   FloorPrior = hi.FloorPrior==null?0: hi.FloorPrior,
                                   WingPrior = hi.WingPrior==null?0: hi.WingPrior,
                                   RoomPrior = hi.RoomPrior==null?0: hi.RoomPrior,
                                   BedPrior = hi.BedPrior==null?0:hi.BedPrior,
                               }).OrderBy(item => item.CompanyName).ThenBy(item => item.FacilityName).ThenBy(item => item.NursingStationName).ToList();
            }
            return hierarchies;
        }
        public NurseStationHierarchyEntity GetNurseStationHierarchyDetailsById(int hierarchyId)
        {
            var result = (from hi in this.dbContext.NurseStationHierarchies
                          join ns in this.dbContext.NursingStations on hi.NurseStation_Id equals ns.NurseStation_Id
                          join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                          join com in this.dbContext.Companies on fa.Company_Id equals com.Company_Id
                          where hi.NSHierarchy_Id == hierarchyId
                          select new NurseStationHierarchyEntity
                          {
                              CompanyId = com.Company_Id,
                              FacilityId = fa.Facility_Id,
                              NurseStation_Id = ns.NurseStation_Id,
                              FloorPrior = hi.FloorPrior == null ? 0 : hi.FloorPrior,
                              WingPrior = hi.WingPrior == null ? 0 : hi.WingPrior,
                              RoomPrior = hi.RoomPrior == null ? 0 : hi.RoomPrior,
                              BedPrior = hi.BedPrior == null ? 0 : hi.BedPrior,
                          }).FirstOrDefault();
            return result;
        }
        public NurseStationHierarchyEntity GetNurseStationHierarchyDetailsbyNsId(int NsId)
        {
            var list = this.dbContext.NurseStationHierarchies.Where(s => s.NurseStation_Id == NsId).FirstOrDefault();
            return this.autoMapper.Map<NurseStationHierarchy, NurseStationHierarchyEntity>(list);
        }
    }
}
