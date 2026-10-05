using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;
using System.Web;
using System.Security.Claims;
using EncDec;
using System.Collections;
    
namespace LTCPro.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public FacilityRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }
        //public List<SectionEntity> GetSection()
        //{
        //    List<Section> section = this.dbContext.Sections.Where(sc => sc.Section_Status == 1).ToList();
        //    return this.autoMapper.Map<List<Section>, List<SectionEntity>>(section);
        //}

        //public int InsertFacADTDefaults(FacADTDefaultEntity ADTDefaults)
        //{
        //    var adt = this.autoMapper.Map<FacADTDefaultEntity, FacADTDefault>(ADTDefaults);
        //    this.dbContext.FacADTDefaults.Add(adt);
        //    this.dbContext.SaveChanges();
        //    return 1;
        //}
        //public int InsertFacCarePlan(FacCarePlanEntity CarePlan)
        //{
        //    var care = this.autoMapper.Map<FacCarePlanEntity, FacCarePlan>(CarePlan);
        //    this.dbContext.FacCarePlans.Add(care);
        //    this.dbContext.SaveChanges();
        //    return 1;
        //}
        //public int InsertEHRInfo(FacEhrInfoEntity EHRInfo)
        //{
        //    var care = this.autoMapper.Map<FacEhrInfoEntity, FacEhrInfo>(EHRInfo);
        //    this.dbContext.FacEhrInfoes.Add(care);
        //    this.dbContext.SaveChanges();
        //    return 1;
        //}
        //public int InsertFacOther(FacOtherEntity FacOther)
        //{
        //    var check = this.dbContext.FacOthers.Where(fo => fo.Status == 1 && fo.Facility_Id == FacOther.Facility_Id).ToList();
        //    if (check.Count > 0)
        //    {
        //        var row = this.dbContext.FacOthers.Where(fo => fo.Status == 1 && fo.Facility_Id == FacOther.Facility_Id).First<FacOther>();
        //        row.CareTrackerExportDate = FacOther.CareTrackerExportDate;
        //        row.CareTrackerExportPath = FacOther.CareTrackerExportPath;
        //        row.CareTrackerImportPath = FacOther.CareTrackerImportPath;
        //        row.UseCareTracker = FacOther.UseCareTracker;
        //        this.dbContext.SaveChanges();
        //        //var others = this.autoMapper.Map<FacOtherEntity, FacOther>(FacOther);
        //        //db.FacOthers.Add(others);
        //        //db.SaveChanges();
        //    }
        //    else
        //    {
        //        var others = this.autoMapper.Map<FacOtherEntity, FacOther>(FacOther);
        //        this.dbContext.FacOthers.Add(others);
        //        this.dbContext.SaveChanges();
        //    }
        //    return 1;
        //}
        //public int InsertFacilityPO(FacilityPOEntity PhysicianOrders)
        //{
        //    var po = this.autoMapper.Map<FacilityPOEntity, FacilityPO>(PhysicianOrders);
        //    this.dbContext.FacilityPOes.Add(po);
        //    this.dbContext.SaveChanges();
        //    return 1;

        //}
        //public int InsertFacilityICD(FacilityICDEntity Icd)
        //{
        //    var facicd = this.autoMapper.Map<FacilityICDEntity, FacilityICD>(Icd);
        //    this.dbContext.FacilityICDs.Add(facicd);
        //    this.dbContext.SaveChanges();
        //    return 1;
        //}

        //public int InsertUpdateAssessment(FacAssessments Facass)
        //{
        //    var check = this.dbContext.FacOthers.Where(fo => fo.Facility_Id == Facass.Facility_Id && fo.Status == 1).ToList();
        //    if (check.Count > 0)
        //    {
        //        var row = this.dbContext.FacOthers.Where(fo => fo.Facility_Id == Facass.Facility_Id && fo.Status == 1).First<FacOther>();
        //        row.AssmtLevelBasis = Facass.AssmtLevelBasis;
        //        this.dbContext.SaveChanges();
        //    }
        //    else
        //    {
        //        var fac = this.autoMapper.Map<FacAssessments, FacOther>(Facass);
        //        this.dbContext.FacOthers.Add(fac);
        //        this.dbContext.SaveChanges();
        //    }
        //    return 1;
        //}

        public int InsertUpdateFacilityMaster(FacilityEntity entity)
        {
            entity.Facility_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<FacilityEntity, Facility>(entity);
            if (record.Facility_Id == 0)
            {
                if (this.dbContext.Facilities.Any(e => e.Facility_Name == entity.Facility_Name && e.Company_Id == entity.Company_Id))
                {
                    //FacilityName already exisits
                    return 2;
                }
                else if (this.dbContext.Facilities.Any(e => e.Facility_ShortName!="" && e.Facility_ShortName != null && e.Facility_ShortName == entity.Facility_ShortName && e.Company_Id == entity.Company_Id))
                {
                    //FacilityShortName already exisits
                    return 3;
                }
                else
                {
                    this.dbContext.Facilities.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.FacilityMaster,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.Facility_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
            }
            else
            {

                if (this.dbContext.Facilities.Any(e => (e.Facility_Name == entity.Facility_Name && e.Company_Id == entity.Company_Id) && e.Facility_Id != entity.Facility_Id))
                {
                    //FacilityName already exisits
                    return 2;
                }
                if (this.dbContext.Facilities.Any(e => (e.Facility_ShortName == entity.Facility_ShortName && e.Company_Id == entity.Company_Id) && e.Facility_Id != entity.Facility_Id && e.Facility_ShortName != "" && e.Facility_ShortName!=null ))
                {
                    //Facility_ShortName already exisits
                    return 3;
                }
                else
                {
                    Facility facility = this.dbContext.Facilities.Find(entity.Facility_Id);
                    facility.Company_Id = entity.Company_Id;
                    facility.Facility_Name = entity.Facility_Name;
                    facility.Facility_Addr1 = entity.Facility_Addr1;
                    facility.Facility_Addr2 = entity.Facility_Addr2;
                    facility.Facility_Zip = entity.Facility_Zip;
                    facility.Facility_CountryId = entity.Facility_CountryId;
                    //facility.Facility_StateId = entity.Facility_StateId;
                    //facility.Facility_CityId = entity.Facility_CityId;
                    facility.Facility_Phone = entity.Facility_Phone;
                    facility.Facility_Fax = entity.Facility_Fax;
                    facility.Facility_ShortName = entity.Facility_ShortName;
                    facility.Facility_ContactName = entity.Facility_ContactName;
                    facility.Facility_ContactPhone = entity.Facility_ContactPhone;
                    facility.Facility_Status = entity.Facility_Status;
                    facility.Facility_CreatedBy = entity.Facility_CreatedBy;
                    facility.Facility_CreatedDate = entity.Facility_CreatedDate;
                    facility.Facility_City = entity.Facility_City;
                    facility.Facility_State = entity.Facility_State;
                    facility.TZ_Id = entity.TZ_Id;
                    if (record.Facility_Logo.Length > 0)
                        facility.Facility_Logo = record.Facility_Logo;
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.FacilityMaster,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = record.Facility_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0
                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;

                }
            }
        }
        public List<FacilityCustomEntity> GetActiveFacilityMasterList()
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
                        return (from facility in this.dbContext.Facilities
                                join country in this.dbContext.Countries on facility.Facility_CountryId equals country.Country_Id
                                join user in this.dbContext.Users on facility.Facility_CreatedBy equals user.User_Id
                                join company in this.dbContext.Companies on facility.Company_Id equals company.Company_Id
                                where facility.Facility_Status == 1 && facilityIds.Contains(facility.Facility_Id)
                                select new FacilityCustomEntity
                                {
                                    Company_Id = facility.Company_Id,
                                    Facility_Id = facility.Facility_Id,
                                    Company_Name = company.Company_Name,
                                    Facility_Addr1 = facility.Facility_Addr1,
                                    Facility_Addr2 = facility.Facility_Addr2,
                                    Facility_Zip = facility.Facility_Zip,
                                    Facility_CountryId = facility.Facility_CountryId,
                                    Facility_Phone = facility.Facility_Phone,
                                    Facility_Fax = facility.Facility_Fax,
                                    Facility_ShortName = facility.Facility_ShortName,
                                    Facility_ContactName = facility.Facility_ContactName,
                                    Facility_ContactPhone = facility.Facility_ContactPhone,
                                    Facility_Logo = facility.Facility_Logo,
                                    Facility_Status = facility.Facility_Status,
                                    Facility_CreatedBy = facility.Facility_CreatedBy,
                                    Facility_CreatedDate = facility.Facility_CreatedDate,
                                    UserName = user.UserName,
                                    Facility_Name = facility.Facility_Name,
                                    PatientCount=(this.dbContext.VisitInfoes.Where(vs=>vs.FacilityId==facility.Facility_Id).Select(vs=>vs.Patient_Id).Count())
                                }).ToList();
                    }
                }
            }
            return null;
        }

        public List<FacilityCustomEntity> GetAllFacilityMasterList()
        {
            return (from facility in this.dbContext.Facilities
                    join country in this.dbContext.Countries on facility.Facility_CountryId equals country.Country_Id
                    join user in this.dbContext.Users on facility.Facility_CreatedBy equals user.User_Id
                    join company in this.dbContext.Companies on facility.Company_Id equals company.Company_Id

                    select new FacilityCustomEntity
                    {
                        Company_Id = facility.Company_Id,
                        Facility_Id = facility.Facility_Id,
                        Company_Name = company.Company_Name,
                        Company_Status = company.Company_Status,
                        Facility_Addr1 = facility.Facility_Addr1,
                        Facility_Addr2 = facility.Facility_Addr2,
                        Facility_Zip = facility.Facility_Zip,
                        Facility_CountryId = facility.Facility_CountryId,
                        Facility_Phone = facility.Facility_Phone,
                        Facility_Fax = facility.Facility_Fax,
                        Facility_ShortName = facility.Facility_ShortName,
                        Facility_ContactName = facility.Facility_ContactName,
                        Facility_ContactPhone = facility.Facility_ContactPhone,
                        Facility_Logo = facility.Facility_Logo,
                        Facility_Status = facility.Facility_Status,
                        Facility_CreatedBy = facility.Facility_CreatedBy,
                        Facility_CreatedDate = facility.Facility_CreatedDate,
                        Country_Code = country.Country_Name,
                        UserName = user.User_DisplayName,
                        Facility_Name = facility.Facility_Name,
                        Facility_City = facility.Facility_City,
                        Facility_State = facility.Facility_State,
                        TimeZone=this.dbContext.tblTimeZones.Where(tz=>tz.TZ_Id == facility.TZ_Id).Select(tz=>tz.ZoneDesc).FirstOrDefault()
                    }).OrderBy(item => item.Company_Name).ThenBy(item => item.Facility_Name).ToList();
        }

        public FacilityCustomEntity GetFacilityDetailsById(int facilityId)
        {


            return (from facility in this.dbContext.Facilities
                    join country in this.dbContext.Countries on facility.Facility_CountryId equals country.Country_Id
                    join user in this.dbContext.Users on facility.Facility_CreatedBy equals user.User_Id
                    join company in this.dbContext.Companies on facility.Company_Id equals company.Company_Id
                    join x in this.dbContext.CompanyConfigs on facility.Company_Id equals x.Company_Id into comco
                    from companyConfig in comco.DefaultIfEmpty()
                    where facility.Facility_Id == facilityId

                    select new FacilityCustomEntity
                    {
                        Company_Id = facility.Company_Id,
                        Facility_Id = facility.Facility_Id,
                        Company_Name = company.Company_Name,
                        Company_Status = company.Company_Status,
                        Facility_Addr1 = facility.Facility_Addr1,
                        Facility_Addr2 = facility.Facility_Addr2,
                        Facility_Zip = facility.Facility_Zip,
                        Facility_CountryId = facility.Facility_CountryId,
                        Facility_Phone = facility.Facility_Phone,
                        Facility_Fax = facility.Facility_Fax,
                        Facility_ShortName = facility.Facility_ShortName,
                        Facility_ContactName = facility.Facility_ContactName,
                        Facility_ContactPhone = facility.Facility_ContactPhone,
                        Facility_Logo = facility.Facility_Logo,
                        Facility_Status = facility.Facility_Status,
                        Facility_CreatedBy = facility.Facility_CreatedBy,
                        Facility_CreatedDate = facility.Facility_CreatedDate,
                        Country_Code = country.Country_Name,
                        UserName = user.UserName,
                        Facility_Name = facility.Facility_Name,
                        Facility_City = facility.Facility_City,
                        Facility_State = facility.Facility_State,
                        Company_UniqueId = company.Company_UniqueId,
                        CompanyHlsevenFlag = (companyConfig == null ? 0 : (int)companyConfig.Hl7Configured),
                        TZ_Id=facility.TZ_Id
                    }).FirstOrDefault();
        }

        public int InsertUpdateFloorMaster(FloorEntity entity)
        {
            entity.Floor_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<FloorEntity, Floor>(entity);
            if (record.Floor_Id == 0)
            {
                this.dbContext.Floors.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FloorMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.Floor_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                Floor floor = this.dbContext.Floors.Find(record.Floor_Id);
                floor.Floor_Name = record.Floor_Name;
                floor.Floor_Status = record.Floor_Status;
                floor.Floor_CreatedBy = record.Floor_CreatedBy;
                floor.Floor_CreatedDate = record.Floor_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FloorMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Floor_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }
        public List<FloorEntity> GetAllFloorsList()
        {
            var floors = this.dbContext.Floors.OrderBy(item => item.Floor_Name).ToList();
            return this.autoMapper.Map<List<Floor>, List<FloorEntity>>(floors);
        }
        public List<FloorEntity> GetFloorsForFacility(int companyId, int facilityId, int stationId)
        {
            var floorIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId && c.NurseStation_Id == stationId).Distinct().Select(c => c.Floor_Id);
            var floors = this.dbContext.Floors.Where(i => floorIds.Contains(i.Floor_Id)).ToList();
            return this.autoMapper.Map<List<Floor>, List<FloorEntity>>(floors);
        }
        public int InsertUpdateNurseStation(NursingStationEntity entity)
        {
            entity.NurseStation_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            NurseShift nurseShiftrecord = new NurseShift();
            var record = this.autoMapper.Map<NursingStationEntity, NursingStation>(entity);
            if (record.NurseStation_Id == 0)
            {
                this.dbContext.NursingStations.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseStationMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.NurseStation_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);

                //Insert NurseShifts
                var shiftsList = entity.NurseStationShiftList.Select(item => { item.NurseStation_Id = record.NurseStation_Id; return item; }).ToList();
                if (shiftsList.Count() > 0)
                {
                    foreach (var item in shiftsList)
                    {
                        nurseShiftrecord = new NurseShift();
                        nurseShiftrecord.NurseShifts_Id = item.NurseShifts_Id;
                        nurseShiftrecord.NurseStation_Id = item.NurseStation_Id;
                        nurseShiftrecord.NurseShifts_Name = item.NurseShifts_Name;
                        nurseShiftrecord.Fromtime_hoursId = item.Fromtime_hoursId;
                        nurseShiftrecord.FromTime_TimeFormatId = null;
                        nurseShiftrecord.Totime_hoursId = item.Totime_hoursId;
                        nurseShiftrecord.ToTime_TimeFormatId = null;
                        nurseShiftrecord.NurseShifts_Status = item.NurseShifts_Status;
                        nurseShiftrecord.NurseShifts_CreatedBy = item.NurseShifts_CreatedBy;
                        nurseShiftrecord.NurseShifts_CreatedOn = item.NurseShifts_CreatedOn;
                        this.dbContext.NurseShifts.Add(nurseShiftrecord);
                        this.dbContext.SaveChanges();
                    }
                }
                if (record.DefaultPhysician_Id != null)
                {
                    var physician = this.dbContext.PhysicianDetails.Where(ph => ph.Physician_Id == record.DefaultPhysician_Id).FirstOrDefault();
                    var data = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.NurseStation_Id == record.NurseStation_Id && p.PhysicianNPI == physician.PhysicianNPI).FirstOrDefault();
                    if (data == null)
                    {
                        PhysicianDetail physicianRecord = new PhysicianDetail();
                        physicianRecord.PhysicianAddress1 = physician.PhysicianAddress1;
                        physicianRecord.PhysicianAddress2 = physician.PhysicianAddress2;
                        physicianRecord.PhysicianCity = physician.PhysicianCity;
                        physicianRecord.PhysicianCountryID = physician.PhysicianCountryID;
                        physicianRecord.PhysicianDEANumber = physician.PhysicianDEANumber;
                        physicianRecord.PhysicianFName = physician.PhysicianFName;
                        physicianRecord.PhysicianLName = physician.PhysicianLName;
                        physicianRecord.PhysicianNPI = physician.PhysicianNPI;
                        physicianRecord.PhysicianState = physician.PhysicianState;
                        physicianRecord.PhysicianZip = physician.PhysicianZip;
                        physicianRecord.Physician_CreatedBy = physician.Physician_CreatedBy;
                        physicianRecord.Physician_CreatedDate = physician.Physician_CreatedDate;
                        physicianRecord.Physician_Status = physician.Physician_Status;
                        physicianRecord.PhysicianCountryID = physician.PhysicianCountryID;
                        physicianRecord.Facility_Id = record.Facility_Id;
                        physicianRecord.NurseStation_Id = record.NurseStation_Id;
                        this.dbContext.PhysicianDetails.Add(physicianRecord);
                        this.dbContext.SaveChanges();
                    }
                }
                return record.NurseStation_Id;

            }
            else
            {
                NursingStation nurseStation = this.dbContext.NursingStations.Find(record.NurseStation_Id);
                nurseStation.Facility_Id = record.Facility_Id;
                nurseStation.NurseStation_Code = record.NurseStation_Code;
                nurseStation.NurseStation_Name = record.NurseStation_Name;
                nurseStation.NurseStation_Status = record.NurseStation_Status;
                nurseStation.DefaultPhysician_Id = record.DefaultPhysician_Id;
                nurseStation.NurseStation_CreatedBy = record.NurseStation_CreatedBy;
                nurseStation.NurseStation_CreatedDate = record.NurseStation_CreatedDate;
                nurseStation.Default_Pharmacy = record.Default_Pharmacy;
                nurseStation.Backup_Pharmacy = record.Backup_Pharmacy;
                this.dbContext.SaveChanges();
                if(entity.NurseStationShiftList.Count==0)
                {
                    var exisitngRecords = this.dbContext.NurseShifts.Where(n => n.NurseStation_Id == record.NurseStation_Id).ToList();
                    if (exisitngRecords.Count() > 0)
                    {
                        //this.dbContext.NurseShifts.RemoveRange(exisitngRecords);
                        exisitngRecords.ForEach(a => a.NurseShifts_Status = 2);
                        this.dbContext.SaveChanges();
                    }
                }
               else if (entity.NurseStationShiftList.Count() > 0)
                {
                    var existingRecordIds = entity.NurseStationShiftList.Where(e => e.NurseShifts_Id != 0).Select(e => e.NurseShifts_Id).ToList();
                    var nurseStationId = entity.NurseStationShiftList.Select(e => e.NurseStation_Id).FirstOrDefault();
                    var exisitngRecords = this.dbContext.NurseShifts.Where(n => n.NurseStation_Id == nurseStationId).ToList();
                    var toBeDeletedRecords = exisitngRecords.Where(e => !existingRecordIds.Contains(e.NurseShifts_Id)).ToList();
                    if (toBeDeletedRecords.Count() > 0)
                    {
                        //this.dbContext.NurseShifts.RemoveRange(toBeDeletedRecords);
                        toBeDeletedRecords.ForEach(a => a.NurseShifts_Status = 2);
                        this.dbContext.SaveChanges();
                    }
                    var newRecords = entity.NurseStationShiftList.Where(e => e.NurseShifts_Id == 0).ToList();
                    var result = this.autoMapper.Map<List<NurseShiftEntity>, List<NurseShift>>(newRecords);
                    this.dbContext.NurseShifts.AddRange(result);
                    this.dbContext.SaveChanges();
                }
                if (record.DefaultPhysician_Id != null)
                {
                    var physician = this.dbContext.PhysicianDetails.Where(ph => ph.Physician_Id == record.DefaultPhysician_Id).FirstOrDefault();
                    var data = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.NurseStation_Id == record.NurseStation_Id && p.PhysicianNPI == physician.PhysicianNPI).FirstOrDefault();
                    if (data == null)
                    {
                        PhysicianDetail physicianRecord = new PhysicianDetail();
                        physicianRecord.PhysicianAddress1 = physician.PhysicianAddress1;
                        physicianRecord.PhysicianAddress2 = physician.PhysicianAddress2;
                        physicianRecord.PhysicianCity = physician.PhysicianCity;
                        physicianRecord.PhysicianCountryID = physician.PhysicianCountryID;
                        physicianRecord.PhysicianDEANumber = physician.PhysicianDEANumber;
                        physicianRecord.PhysicianFName = physician.PhysicianFName;
                        physicianRecord.PhysicianLName = physician.PhysicianLName;
                        physicianRecord.PhysicianNPI = physician.PhysicianNPI;
                        physicianRecord.PhysicianState = physician.PhysicianState;
                        physicianRecord.PhysicianZip = physician.PhysicianZip;
                        physicianRecord.Physician_CreatedBy = physician.Physician_CreatedBy;
                        physicianRecord.Physician_CreatedDate = physician.Physician_CreatedDate;
                        physicianRecord.Physician_Status = physician.Physician_Status;
                        physicianRecord.PhysicianCountryID = physician.PhysicianCountryID;
                        physicianRecord.Facility_Id = record.Facility_Id;
                        physicianRecord.NurseStation_Id = record.NurseStation_Id;
                        this.dbContext.PhysicianDetails.Add(physicianRecord);
                        this.dbContext.SaveChanges();
                    }
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseStationMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.NurseStation_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return record.NurseStation_Id;
            }
        }
        public int InsertUpdateNurseShift(List<NurseShiftEntity> entity)
        {
            if (entity.Count() > 0)
            {
                var existingRecordIds = entity.Where(e => e.NurseShifts_Id != 0).Select(e => e.NurseShifts_Id).ToList();
                var nurseStationId = entity.Select(e => e.NurseStation_Id).FirstOrDefault();
                var exisitngRecords = this.dbContext.NurseShifts.Where(n => n.NurseStation_Id == nurseStationId).ToList();
                var toBeDeletedRecords = exisitngRecords.Where(e => !existingRecordIds.Contains(e.NurseShifts_Id)).ToList();
                if (toBeDeletedRecords.Count() > 0)
                {
                    toBeDeletedRecords.ForEach(a => a.NurseShifts_Status = 2);
                    //this.dbContext.NurseShifts.RemoveRange(toBeDeletedRecords);
                    this.dbContext.SaveChanges();
                }
                var newRecords = entity.Where(e => e.NurseShifts_Id == 0).ToList();
                var result = this.autoMapper.Map<List<NurseShiftEntity>, List<NurseShift>>(newRecords);
                this.dbContext.NurseShifts.AddRange(result);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseStationMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = nurseStationId.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            return 0;
        }
        //ToDo:Delete if not used
        //public List<NursingStationEntity> GetNurseStationsForFloor(int facilityId, int companyId, int floorId)
        //{
        //    var nurseStationIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId && c.Floor_Id == floorId).Distinct().Select(c => c.NurseStation_Id);
        //    var nurseStations = this.dbContext.NursingStations.Where(i => nurseStationIds.Contains(i.NurseStation_Id)).ToList();
        //    return this.autoMapper.Map<List<NursingStation>, List<NursingStationEntity>>(nurseStations);
        //}
        public List<NursingStationEntity> GetAllNurseStationsList(int? facilityId = null)
        {
            List<NursingStationEntity> nurseStations;
            List<string> list = new List<string>();
            int cmpTimeFormate = 0;
            if (facilityId != null)
            {
                nurseStations = (from ns in this.dbContext.NursingStations
                                 join user in this.dbContext.Users on ns.NurseStation_CreatedBy equals user.User_Id
                                 join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                                 join Com in this.dbContext.Companies on fa.Company_Id equals Com.Company_Id
                                 join ph in this.dbContext.PhysicianDetails on ns.DefaultPhysician_Id equals ph.Physician_Id into phy
                                 from p in phy.DefaultIfEmpty()
                                 join x in this.dbContext.CompanyConfigs on fa.Company_Id equals x.Company_Id into comco
                                 from companyConfig in comco.DefaultIfEmpty()
                                 where ns.Facility_Id == facilityId
                                 select new NursingStationEntity
                                 {
                                     Facility_Id = ns.Facility_Id,
                                     NurseStation_Id = ns.NurseStation_Id,
                                     FacilityName = fa.Facility_Name,
                                     NurseStation_Name = ns.NurseStation_Name,
                                     PhysicianName = (p == null ? String.Empty : (p.PhysicianLName + " , " + p.PhysicianFName)),
                                     PhysicianNPI = (p == null ? String.Empty : (p.PhysicianNPI)),
                                     NurseStation_Code = ns.NurseStation_Code,
                                     NurseStation_CreatedBy = ns.NurseStation_CreatedBy,
                                     NurseStation_CreatedDate = ns.NurseStation_CreatedDate,
                                     NurseStation_Status = ns.NurseStation_Status,
                                     UserName = user.UserName,
                                     FacilityStatus = Com.Company_Status,
                                     PhysicianStatus = (p == null ? 0 : p.Physician_Status),
                                     CompanyHlsevenFlag = (companyConfig == null ? 0 : (int)companyConfig.Hl7Configured),
                                     Biometric_Status= companyConfig==null?0: companyConfig.Fingersdesc_Id == null ? 0 : 1,
                                 }).ToList();
                cmpTimeFormate = this.GetCompanyTimeFormat((int)facilityId);

            }
            else
            {
                nurseStations = (from ns in this.dbContext.NursingStations
                                 join user in this.dbContext.Users on ns.NurseStation_CreatedBy equals user.User_Id
                                 join fac in this.dbContext.Facilities on ns.Facility_Id equals fac.Facility_Id
                               join Com in this.dbContext.Companies on fac.Company_Id equals Com.Company_Id 
                                 join ph in this.dbContext.PhysicianDetails on ns.DefaultPhysician_Id equals ph.Physician_Id into phy
                                 from p in phy.DefaultIfEmpty()
                                 join x in this.dbContext.CompanyConfigs on fac.Company_Id equals x.Company_Id into comco
                                 from companyConfig in comco.DefaultIfEmpty()
                                 select new NursingStationEntity
                                 {
                                     Facility_Id = ns.Facility_Id,
                                     NurseStation_Id = ns.NurseStation_Id,
                                     NurseStation_Name = ns.NurseStation_Name,
                                     PhysicianName = (p == null ? String.Empty : (p.PhysicianLName + " , " + p.PhysicianFName)),
                                     NurseStation_Code = ns.NurseStation_Code,
                                     NurseStation_CreatedBy = ns.NurseStation_CreatedBy,
                                     NurseStation_CreatedDate = ns.NurseStation_CreatedDate,
                                     NurseStation_Status = ns.NurseStation_Status,
                                     UserName = user.User_DisplayName,
                                     FacilityName = fac.Facility_Name,
                                     FacilityStatus = Com.Company_Status ,
                                     PhysicianNPI = (p == null ? String.Empty : (p.PhysicianNPI)),
                                     PhysicianStatus = (p == null ? 0 : p.Physician_Status),
                                     CompanyHlsevenFlag = (companyConfig == null ? 0 : (int)companyConfig.Hl7Configured),
                                     Biometric_Status = companyConfig == null ? 0 : companyConfig.Fingersdesc_Id==null?0: 1,
                                 }).ToList();
            }

            foreach (var item in nurseStations)
            {
                if (facilityId == null)
                    cmpTimeFormate = this.GetCompanyTimeFormat((int)item.Facility_Id);

                if (cmpTimeFormate == 0)
                {
                    var shiftHours = this.dbContext.NurseShifts.Where(nr => nr.NurseStation_Id == item.NurseStation_Id && nr.NurseShifts_Status==1).Select(nr => new { nr.NurseShifts_Name, nr.Fromtime_hoursId, nr.Totime_hoursId }).ToArray();
                    list = new List<string>();
                    if (shiftHours.Length > 0)
                    {
                        foreach (var hour in shiftHours)
                        {
                            int fromHour = hour.Fromtime_hoursId == null ? 0 : (int)hour.Fromtime_hoursId;
                            int toHour = hour.Totime_hoursId == null ? 0 : (int)hour.Totime_hoursId;
                            var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                            var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                            if (from != null && to != null)
                            {
                                string shiftTime = hour.NurseShifts_Name + " " + from.RegularTime + from.RegularTimeFormat + " - " + to.RegularTime + to.RegularTimeFormat;
                                list.Add(shiftTime);
                            }
                        }

                    }
                    if (list.Count > 0)
                    {
                        item.NurseStationShifts = string.Join(", ", list);
                    }
                    else
                    {
                        item.NurseStationShifts = null;
                    }
                }
                else if (cmpTimeFormate == 1)
                {
                    var shiftHours = this.dbContext.NurseShifts.Where(nr => nr.NurseStation_Id == item.NurseStation_Id && nr.NurseShifts_Status == 1).Select(nr => new { nr.NurseShifts_Name, nr.Fromtime_hoursId, nr.Totime_hoursId }).ToArray();
                    list = new List<string>();
                    if (shiftHours.Length > 0)
                    {
                        foreach (var hour in shiftHours)
                        {
                            int fromHour = hour.Fromtime_hoursId == null ? 0 : (int)hour.Fromtime_hoursId;
                            int toHour = hour.Totime_hoursId == null ? 0 : (int)hour.Totime_hoursId;
                            var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                            var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                            if (from != null && to != null)
                            {

                                string shiftTime = hour.NurseShifts_Name + " " + from.Hour_Desc + " - " + to.Hour_Desc;
                                list.Add(shiftTime);
                            }
                        }

                    }
                    if (list.Count > 0)
                    {
                        item.NurseStationShifts = string.Join(", ", list);
                    }
                    else
                    {
                        item.NurseStationShifts = null;
                    }
                }

            }
            return nurseStations.OrderBy(item => item.FacilityName).ThenBy(item => item.NurseStation_Name).ToList();
        }

        public List<WingEntity> GetAllWingsList()
        {
            List<Wing> wings = this.dbContext.Wings.OrderBy(item => item.Wing_Desc).ToList();
            return this.autoMapper.Map<List<Wing>, List<WingEntity>>(wings);
        }
        public List<WingEntity> GetWingsForFloor(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            var wingIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
            c.Facility_Id == facilityId && (floorId == 0 || c.Floor_Id == floorId) && (wingId == 0 || c.Wing_Id == wingId) && (roomId == 0 || c.Room_Id == roomId) && (bedId == 0 || c.Bed_Id == bedId) && c.NurseStation_Id == stationId && c.BedConfig_Status == 1).Distinct()
            .Select(c => c.Wing_Id);
            //var wings = this.dbContext.Wings.Where(i => wingIds.Contains(i.Wing_Id) && i.Wing_Status == 1).ToList();
            //return this.autoMapper.Map<List<Wing>, List<WingEntity>>(wings);
            var wings = (from wi in this.dbContext.Wings
                         where wingIds.Contains(wi.Wing_Id) && wi.Wing_Status == 1
                         select new WingEntity
                         {
                             Wing_Id=wi.Wing_Id,
                             Wing_Desc=wi.Wing_Desc,
                             Wing_Status=wi.Wing_Status,
                             PatientCount = (this.dbContext.VisitInfoes.Where(vs => vs.NursingStationId == stationId && (vs.Wing == wi.Wing_Id)).Count()),
                         }).Distinct().ToList();
            return wings;
        }

        public List<FloorEntity> GetFloorsForNurseStations(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            var floorIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
              c.Facility_Id == facilityId && (floorId == 0 || c.Floor_Id == floorId) && (wingId == 0 || c.Wing_Id == wingId) && (roomId == 0 || c.Room_Id == roomId) && (bedId == 0 || c.Bed_Id == bedId) && c.NurseStation_Id == stationId && c.BedConfig_Status == 1).Distinct()
            .Select(c => c.Floor_Id);
            //var floors = this.dbContext.Floors.Where(i => floorIds.Contains(i.Floor_Id) && i.Floor_Status == 1).ToList();
            //return this.autoMapper.Map<List<Floor>, List<FloorEntity>>(floors);
            var floors = (from fl in this.dbContext.Floors
                          where floorIds.Contains(fl.Floor_Id) && fl.Floor_Status == 1
                          select new FloorEntity
                          {
                              Floor_Id = fl.Floor_Id,
                              Floor_Name=fl.Floor_Name,
                              Floor_Status=fl.Floor_Status,
                              PatientCount = (this.dbContext.VisitInfoes.Where(vs => vs.NursingStationId == stationId && (vs.Floor == fl.Floor_Id.ToString())).Count())
                          }).Distinct().ToList();
            return floors;
        }


        public List<NursingStationEntity> GetNurseStationsForFacility(int companyId, int facilityId)
        {
            var stationIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
              c.Facility_Id == facilityId).Distinct()
            .Select(c => c.NurseStation_Id);
            var stations = this.dbContext.NursingStations.Where(i => stationIds.Contains(i.NurseStation_Id) && i.NurseStation_Status == 1).ToList();
            return this.autoMapper.Map<List<NursingStation>, List<NursingStationEntity>>(stations);
        }



        public int InsertUpdateWing(WingEntity entity)
        {
            entity.Wing_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<WingEntity, Wing>(entity);
            if (record.Wing_Id == 0)
            {
                this.dbContext.Wings.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.WingMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.Wing_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                Wing wing = this.dbContext.Wings.Find(record.Wing_Id);
                wing.Wing_Desc = record.Wing_Desc;
                wing.Wing_Status = record.Wing_Status;
                wing.Wing_CreatedBy = record.Wing_CreatedBy;
                wing.Wing_CreatedDate = record.Wing_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.WingMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Wing_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }

        public List<RoomEntity> GetAllRoomsList()
        {
            List<Room> rooms = this.dbContext.Rooms.OrderBy(item => item.Room_Name).ToList();
            return this.autoMapper.Map<List<Room>, List<RoomEntity>>(rooms);
        }

        public List<RoomEntity> GetRoomsForWing(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            var roomIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
            c.Facility_Id == facilityId && (floorId == 0 || c.Floor_Id == floorId) && (wingId == 0 || c.Wing_Id == wingId) && (roomId == 0 || c.Room_Id == roomId) && (bedId == 0 || c.Bed_Id == bedId) && c.NurseStation_Id == stationId  && c.BedConfig_Status == 1).Distinct()
            .Select(c => c.Room_Id);
            //var rooms = this.dbContext.Rooms.Where(i => roomIds.Contains(i.Room_Id) && i.Room_Status == 1).ToList();
            //return this.autoMapper.Map<List<Room>, List<RoomEntity>>(rooms);
            var rooms = (from ro in this.dbContext.Rooms
                         where roomIds.Contains(ro.Room_Id) && ro.Room_Status == 1
                         select new RoomEntity
                         {
                             Room_Id = ro.Room_Id,
                             Room_Code = ro.Room_Code,
                             Room_Name = ro.Room_Name,
                             Room_Status = ro.Room_Status,
                             PatientCount = (this.dbContext.VisitInfoes.Where(vs => vs.NursingStationId == stationId  && (vs.Room == ro.Room_Id.ToString())).Count()),

                         }).Distinct().ToList();
            return rooms;
         }
        public int InsertUpdateRoom(RoomEntity entity)
        {
            entity.Room_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<RoomEntity, Room>(entity);
            if (record.Room_Id == 0)
            {
                this.dbContext.Rooms.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RoomMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.Room_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                Room room = this.dbContext.Rooms.Find(record.Room_Id);
                room.Room_Name = record.Room_Name;
                room.Room_Code = record.Room_Code;
                room.Room_Status = record.Room_Status;
                room.Room_CreatedBy = record.Room_CreatedBy;
                room.Room_CreatedDate = record.Room_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RoomMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Room_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }

        public List<BedEntity> GetAllBedsList()
        {
            List<Bed> beds = this.dbContext.Beds.OrderBy(item => item.Bed_Name).ToList();
            return this.autoMapper.Map<List<Bed>, List<BedEntity>>(beds);
        }


        public List<BedEntity> GetBedsForRoom(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            var bedIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
            c.Facility_Id == facilityId && (floorId ==0 || c.Floor_Id == floorId) && c.NurseStation_Id == stationId
            && (wingId==0 || c.Wing_Id == wingId) && (roomId==0 || c.Room_Id == roomId) && (bedId==0 || c.Bed_Id==bedId) && c.BedConfig_Status == 1).Distinct()
            .Select(c => c.Bed_Id);
            //var beds = this.dbContext.Beds.Where(i => bedIds.Contains(i.Bed_Id) && i.Bed_Status == 1).ToList();
            //return this.autoMapper.Map<List<Bed>, List<BedEntity>>(beds);
            var beds = (from be in this.dbContext.Beds
                        where bedIds.Contains(be.Bed_Id) && be.Bed_Status == 1
                        select new BedEntity
                        {
                            Bed_Id=be.Bed_Id,
                            Bed_Code=be.Bed_Code,
                            Bed_Name=be.Bed_Name,
                            Bed_Status=be.Bed_Status,
                            PatientCount = (this.dbContext.VisitInfoes.Where(vs => vs.NursingStationId == stationId && (vs.Bed == be.Bed_Id.ToString())).Count()),
                        }).Distinct().ToList();
            return beds;
        }
        public int InsertUpdateBed(BedEntity entity)
        {
            entity.Bed_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<BedEntity, Bed>(entity);
            if (record.Bed_Id == 0)
            {
                this.dbContext.Beds.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.BedMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.Bed_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                Bed bed = this.dbContext.Beds.Find(record.Bed_Id);
                bed.Bed_Name = record.Bed_Name;
                bed.Bed_Code = record.Bed_Code;
                bed.Bed_Status = record.Bed_Status;
                bed.Bed_CreatedBy = record.Bed_CreatedBy;
                bed.Bed_CreatedDate = record.Bed_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.BedMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Bed_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0
                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }
        public int InsertUpdateCompanyBedConfigs(CompanyBedConfigEntity bedConfig)
        {
            bedConfig.BedConfig_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<CompanyBedConfigEntity, CompanyBedConfig>(bedConfig);
            if (record.BedConfig_Id == 0)
            {
                if (this.dbContext.CompanyBedConfigs.Where(i => i.Company_Id != bedConfig.Company_Id && i.Facility_Id == bedConfig.Facility_Id).Count() > 0)
                {
                    //Facility is already mapped to another company
                    return 2;
                }
                else if (this.dbContext.CompanyBedConfigs.Where(i => i.Facility_Id != bedConfig.Facility_Id && i.NurseStation_Id == bedConfig.NurseStation_Id).Count() > 0)
                {
                    //NS is already mapped to another facility
                    return 3;
                }
                else if (this.dbContext.CompanyBedConfigs.Where(i => i.Company_Id == bedConfig.Company_Id && i.Facility_Id == bedConfig.Facility_Id && i.NurseStation_Id == bedConfig.NurseStation_Id
                && i.Floor_Id == bedConfig.Floor_Id && i.Wing_Id == bedConfig.Wing_Id && i.Room_Id == bedConfig.Room_Id && i.Bed_Id == bedConfig.Bed_Id).Count() > 0)
                {
                    //Record already exist
                    return 4;
                }
                else
                {
                    this.dbContext.CompanyBedConfigs.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.CompanyBedConfig,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.BedConfig_Id.ToString(),
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
                CompanyBedConfig config = this.dbContext.CompanyBedConfigs.Find(record.BedConfig_Id);
                if (this.dbContext.CompanyBedConfigs.Where(i => i.Company_Id != bedConfig.Company_Id && i.Facility_Id == bedConfig.Facility_Id && i.BedConfig_Id != record.BedConfig_Id).Count() > 0)
                {
                    //Facility is already mapped to another company
                    return 2;
                }
                else if (this.dbContext.CompanyBedConfigs.Where(i => i.Facility_Id != bedConfig.Facility_Id && i.NurseStation_Id == bedConfig.NurseStation_Id && i.BedConfig_Id != record.BedConfig_Id).Count() > 0)
                {
                    //NS is already mapped to another facility
                    return 3;
                }
                else if (this.dbContext.CompanyBedConfigs.Where(i => i.Company_Id == bedConfig.Company_Id && i.Facility_Id == bedConfig.Facility_Id && i.NurseStation_Id == bedConfig.NurseStation_Id
                && i.Floor_Id == bedConfig.Floor_Id && i.Wing_Id == bedConfig.Wing_Id && i.Room_Id == bedConfig.Room_Id && i.Bed_Id == bedConfig.Bed_Id && i.BedConfig_Id != record.BedConfig_Id).Count() > 0)
                {
                    //Record already exist
                    return 4;
                }
                else
                {
                    config.Company_Id = record.Company_Id;
                    config.Facility_Id = record.Facility_Id;
                    config.Floor_Id = record.Floor_Id;
                    config.NurseStation_Id = record.NurseStation_Id;
                    config.Wing_Id = record.Wing_Id;
                    config.Room_Id = record.Room_Id;
                    config.Bed_Id = record.Bed_Id;
                    config.BedConfig_Status = record.BedConfig_Status;
                    config.BedConfig_CreatedBy = record.BedConfig_CreatedBy;
                    config.BedConfig_CreatedDate = record.BedConfig_CreatedDate;
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.CompanyBedConfig,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = record.BedConfig_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0
                    };
                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
            }
        }

        public CompanyBedConfigEntity GetCompanyBedConfigByBedConfigId(int bedConfigId)
        {
            CompanyBedConfig companyBedConfig = this.dbContext.CompanyBedConfigs.Find(bedConfigId);
            return this.autoMapper.Map<CompanyBedConfig, CompanyBedConfigEntity>(companyBedConfig);
        }
        public FloorEntity GetFloorById(int floorId)
        {
            Floor floor = this.dbContext.Floors.Find(floorId);
            return this.autoMapper.Map<Floor, FloorEntity>(floor);
        }

        public NursingStationEntity GetNurseStationById(int nurseStationId)
        {
            NursingStation nurseStation = this.dbContext.NursingStations.Find(nurseStationId);
            return this.autoMapper.Map<NursingStation, NursingStationEntity>(nurseStation);
        }

        public WingEntity GetWingById(int wingId)
        {
            Wing wing = this.dbContext.Wings.Find(wingId);
            return this.autoMapper.Map<Wing, WingEntity>(wing);
        }

        public RoomEntity GetRoomById(int roomId)
        {
            Room room = this.dbContext.Rooms.Find(roomId);
            return this.autoMapper.Map<Room, RoomEntity>(room);
        }

        public BedEntity GetBedById(int bedId)
        {
            Bed bed = this.dbContext.Beds.Find(bedId);
            return this.autoMapper.Map<Bed, BedEntity>(bed);
        }

        //  public List<NursingStationEntity> GetNurseStationsForFacility(int companyId, int facilityId)
        //  {
        //    var nurseStationIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId).Distinct().Select(c => c.NurseStation_Id);
        //    var nurseStations = this.dbContext.NursingStations.Where(i => nurseStationIds.Contains(i.NurseStation_Id)).ToList();
        //   return this.autoMapper.Map<List<NursingStation>, List<NursingStationEntity>>(nurseStations);
        // }

        public List<WingEntity> GetWingsForFacility(int companyId, int facilityId)
        {
            var wingIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId).Distinct()
            .Select(c => c.Wing_Id);
            var wings = this.dbContext.Wings.Where(i => wingIds.Contains(i.Wing_Id)).ToList();
            return this.autoMapper.Map<List<Wing>, List<WingEntity>>(wings);
        }

        public List<RoomEntity> GetRoomsForFacility(int companyId, int facilityId)
        {
            var roomIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId).Distinct()
            .Select(c => c.Room_Id);
            var rooms = this.dbContext.Rooms.Where(i => roomIds.Contains(i.Room_Id)).ToList();
            return this.autoMapper.Map<List<Room>, List<RoomEntity>>(rooms);
        }

        public List<BedEntity> GetBedsForFacility(int companyId, int facilityId)
        {
            var bedIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId).Distinct()
            .Select(c => c.Room_Id);
            var beds = this.dbContext.Beds.Where(i => bedIds.Contains(i.Bed_Id)).ToList();
            return this.autoMapper.Map<List<Bed>, List<BedEntity>>(beds);
        }

        //public CompanyBedConfigCustomEntity GetCompanyBedConfigs(CompanyBedConfigCustomEntity filterConfigs)
        //{
        //    var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
        //                                    join f in this.dbContext.Facilities on us.Facility_id equals f.Facility_Id
        //                                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
        //                                    where us.User_Id == filterConfigs.UserId && f.Facility_Status == 1 && ns.NurseStation_Status == 1
        //                                    select new {
        //                                        NurseStationId = ns.NurseStation_Id,
        //                                        FacilityId = us.Facility_id
        //                                    }).Distinct().ToList();
        //    List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
        //    List<int> nsIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

        //    int companyId = this.dbContext.Facilities.Where(f => facilityIds.Contains(f.Facility_Id)).Select(f => f.Company_Id).FirstOrDefault();

        //    //var records=this.dbContext.CompanyBedConfigs.Where(co=>facilityIds.Contains((int)co.Facility_Id) && facility_nurseStationIds.Contains((int)co.NurseStation_Id) )

        //    // var qq1 = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId));
        //    //var configs = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == filterConfigs.Company_Id && c.Facility_Id == filterConfigs.Facility_Id && filterConfigs.Floors.Contains((int)c.Floor_Id)
        //    //&& filterConfigs.NurseStations.Contains((int)c.NurseStation_Id) && filterConfigs.Wings.Contains((int)c.Wing_Id) && filterConfigs.Rooms.Contains((int)c.Room_Id)
        //    //&& filterConfigs.Beds.Contains((int)c.Bed_Id) && c.BedConfig_Status == 1).ToList();
        //    //var q1 = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == filterConfigs.Company_Id && c.Facility_Id == filterConfigs.Facility_Id);

        //    CompanyBedConfigCustomEntity entity = new CompanyBedConfigCustomEntity();
        //    entity.Company_Id = companyId;
        //    entity.Facilities = filterConfigs.Facilities;
        //    entity.NurseStations = filterConfigs.NurseStations;
        //    if (filterConfigs.CompanyToBedFlag == 0) {
        //        if (filterConfigs.Facilities.Count() != 0) {
        //            var q1 = this.dbContext.NursingStations.Where(c => filterConfigs.Facilities.Contains((int)c.Facility_Id));
        //            if (filterConfigs.NurseStations.Count() != 0) {
        //                q1 = q1.Where(c => filterConfigs.NurseStations.Contains((int)c.NurseStation_Id));
        //            }
        //            entity.Facilities = q1.Select(f => (int)f.Facility_Id).Distinct().ToList();
        //            entity.NurseStations = q1.Select(f => (int)f.NurseStation_Id).Distinct().ToList();
        //        } else {
        //            if (filterConfigs.NurseStations.Count() != 0) {
        //                var q1 = this.dbContext.NursingStations.Where(c => filterConfigs.NurseStations.Contains((int)c.NurseStation_Id));
        //                entity.NurseStations = q1.Select(f => (int)f.NurseStation_Id).Distinct().ToList();
        //            }

        //        }
        //        entity.Floors = null;
        //        entity.Wings = null;
        //        entity.Rooms = null;
        //        entity.Beds = null;
        //        return entity;

        //    } else {
        //        var q1 = this.dbContext.CompanyBedConfigs.Where(c => facilityIds.Contains((int)c.Facility_Id));

        //        var q2 = q1;
        //        if (filterConfigs.Facilities.Count() != 0) {
        //            q2 = q1.Where(c => filterConfigs.Facilities.Contains((int)c.Facility_Id));
        //        }
        //        var q3 = q2;
        //        if (filterConfigs.NurseStations.Count() != 0) {
        //            q3 = q2.Where(c => filterConfigs.NurseStations.Contains((int)c.NurseStation_Id));
        //        }
        //        var q4 = q3;
        //        if (filterConfigs.Floors.Count() != 0) {
        //            q4 = q3.Where(c => filterConfigs.Floors.Contains((int)c.Floor_Id));
        //        }
        //        var q5 = q4;
        //        if (filterConfigs.Wings.Count() != 0) {
        //            q5 = q4.Where(c => filterConfigs.Wings.Contains((int)c.Wing_Id));
        //        }
        //        var q6 = q5;
        //        if (filterConfigs.Rooms.Count() != 0) {
        //            q6 = q5.Where(c => filterConfigs.Rooms.Contains((int)c.Room_Id));
        //        }
        //        var q7 = q6;
        //        if (filterConfigs.Beds.Count() != 0) {
        //            q7 = q6.Where(c => filterConfigs.Beds.Contains((int)c.Bed_Id));
        //        }

        //        entity.Facilities = q7.Select(f => (int)f.Facility_Id).Distinct().ToList();
        //        entity.Floors = q7.Select(f => (int)f.Floor_Id).Distinct().ToList();
        //        entity.NurseStations = q7.Select(f => (int)f.NurseStation_Id).Distinct().ToList();
        //        entity.Wings = q7.Select(f => (int)f.Wing_Id).Distinct().ToList();
        //        entity.Rooms = q7.Select(f => (int)f.Room_Id).Distinct().ToList();
        //        entity.Beds = q7.Select(f => (int)f.Bed_Id).Distinct().ToList();
        //        return entity;
        //    }
        //}
        public List<FloorEntity> GetFloorsForFacility(int companyId, int facilityId)
        {
            var floorIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId).Distinct()
            .Select(c => c.Floor_Id);
            var floors = this.dbContext.Floors.Where(i => floorIds.Contains(i.Floor_Id)).ToList();
            return this.autoMapper.Map<List<Floor>, List<FloorEntity>>(floors);
        }
        public List<CompanyBedConfigEntity> GetCompanyBedGridData(int companyId, int facilityId, int stationId, int floorId, int wingId, int roomId, int bedId)
        {
            var q1 = this.dbContext.CompanyBedConfigs.ToList();
            var q2 = q1;
            if (companyId > 0)
            {
                q2 = q1.Where(c => c.Company_Id == companyId).ToList();
            }
            var q3 = q2;
            if (facilityId > 0)
            {
                q3 = q2.Where(c => c.Facility_Id == facilityId).ToList();
            }
            var q4 = q3;
            if (stationId > 0)
            {
                q4 = q3.Where(c => c.NurseStation_Id == stationId).ToList();
            }
            var q5 = q4;
            if (floorId > 0)
            {
                q5 = q4.Where(c => c.Floor_Id == floorId).ToList();
            }
            var q6 = q5;
            if (wingId > 0)
            {
                q6 = q5.Where(c => c.Wing_Id == wingId).ToList();
            }
            var q7 = q6;
            if (roomId > 0)
            {
                q7 = q6.Where(c => c.Room_Id == roomId).ToList();
            }
            var q8 = q7;
            if (bedId > 0)
            {
                q8 = q7.Where(c => c.Bed_Id == bedId).ToList();
            }

            var data = (from cb in q8
                        join c in this.dbContext.Companies on cb.Company_Id equals c.Company_Id
                        join f in this.dbContext.Facilities on cb.Facility_Id equals f.Facility_Id
                        join ns in this.dbContext.NursingStations on cb.NurseStation_Id equals ns.NurseStation_Id
                        join flo in this.dbContext.Floors on cb.Floor_Id equals flo.Floor_Id into fl
                        from floo in fl.DefaultIfEmpty()
                        join w in this.dbContext.Wings on cb.Wing_Id equals w.Wing_Id into wi
                        from win in wi.DefaultIfEmpty()
                        join r in this.dbContext.Rooms on cb.Room_Id equals r.Room_Id into ro
                        from roo in ro.DefaultIfEmpty()
                        join b in this.dbContext.Beds on cb.Bed_Id equals b.Bed_Id into be
                        from bed in be.DefaultIfEmpty()
                            //join fl in this.dbContext.Floors on cb.Floor_Id equals fl.Floor_Id
                            //join w in this.dbContext.Wings on cb.Wing_Id equals w.Wing_Id
                            //join r in this.dbContext.Rooms on cb.Room_Id equals r.Room_Id
                            //join b in this.dbContext.Beds on cb.Bed_Id equals b.Bed_Id
                        join u in this.dbContext.Users on cb.BedConfig_CreatedBy equals u.User_Id
                        select new CompanyBedConfigEntity
                        {
                            BedConfig_Id = cb.BedConfig_Id,
                            Company_Id = cb.Company_Id,
                            Facility_Id = cb.Facility_Id,
                            NurseStation_Id = cb.NurseStation_Id,
                            Floor_Id = cb.Floor_Id,
                            Wing_Id = cb.Wing_Id,
                            Room_Id = cb.Room_Id,
                            Bed_Id = cb.Bed_Id,
                            Company_Name = c.Company_Name,
                            Facility_Name = f.Facility_Name,
                            NurseStation_Name = ns.NurseStation_Name,
                            Floor_Name = floo!=null? floo.Floor_Name:"",
                            Wing_Name = win!=null?win.Wing_Desc:"",
                            Room_Name = roo!=null? roo.Room_Name:"",
                            Bed_Name = bed!=null?bed.Bed_Name:"",
                            UserName = u.User_DisplayName,
                            BedConfig_CreatedBy = cb.BedConfig_CreatedBy,
                            BedConfig_CreatedDate = cb.BedConfig_CreatedDate,
                            BedConfig_Status = cb.BedConfig_Status
                        }).OrderBy(item => item.Company_Name).ThenBy(item => item.Facility_Name).ThenBy(item => item.NurseStation_Name).ThenBy(item => item.Floor_Name).ThenBy(item => item.Wing_Name).ThenBy(item => item.Room_Name).ThenBy(item => item.Bed_Name).ToList();
            return data;
        }
        public List<FacilityEntity> GetFacilityDropData()
        {
            var facilityDrop = (from f in this.dbContext.Facilities
                                where f.Facility_Status == 1
                                select new FacilityEntity
                                {
                                    Facility_Id = f.Facility_Id,
                                    Company_Id = f.Company_Id,
                                    Facility_Name = f.Facility_Name,
                                    Facility_Addr1 = f.Facility_Addr1,
                                    Facility_Addr2 = f.Facility_Addr2,
                                    Facility_Zip = f.Facility_Zip,
                                    Facility_City = f.Facility_City,
                                    Facility_State = f.Facility_State,
                                    Facility_CountryId = f.Facility_CountryId,
                                    Facility_Phone = f.Facility_Phone,
                                    Facility_Fax = f.Facility_Fax,
                                    Facility_ContactName = f.Facility_ContactName,
                                    Facility_ContactPhone = f.Facility_ContactPhone,
                                    Facility_ShortName = f.Facility_ShortName,
                                    Facility_Logo = f.Facility_Logo,
                                    Facility_Status = f.Facility_Status,
                                    Facility_CreatedBy = f.Facility_CreatedBy,
                                    Facility_CreatedDate = f.Facility_CreatedDate,
                                    FacilityName = f.Facility_Name,

                                }).OrderBy(item => item.FacilityName).ToList();
            return facilityDrop;
        }
        public List<NurseStationDropEntity> GetNurseStationDropData()
        {
            var nurseDrop = (from ns in this.dbContext.NursingStations
                             where ns.NurseStation_Status == 1
                             select new NurseStationDropEntity
                             {
                                 NurseStation_Id = ns.NurseStation_Id,
                                 NurseStation_Code = ns.NurseStation_Code,
                                 NurseStation_Name = ns.NurseStation_Name,
                             }).OrderBy(item => item.NurseStation_Name).ToList();
            return nurseDrop;
        }

        public List<NurseStationDropEntity> GetAllActiveNurseStationDropData(string facilityIds)
        {
            List<string> facilityIdList = facilityIds.Split(',').ToList();
            int facilityId = Convert.ToInt32(facilityIdList[0]);

            //var nsids = (this.dbContext.CompanyBedConfigs.Where(cb => cb.Facility_Id == facilityId).Select(cb => cb.NurseStation_Id).Distinct()).ToList();
            var nurseStations = (from ns in this.dbContext.NursingStations
                                 join user in this.dbContext.Users on ns.NurseStation_CreatedBy equals user.User_Id
                                 where ns.Facility_Id == facilityId && ns.NurseStation_Status == 1
                                 select new NurseStationDropEntity
                                 {
                                     NurseStation_Id = ns.NurseStation_Id,
                                     NurseStation_Name = ns.NurseStation_Name,
                                     NurseStation_Code = ns.NurseStation_Code,
                                 }).ToList();

            return nurseStations;
        }
        public List<FacilitiesbyCompanyIdEntity> GetFacilitesByCompanyId(int companyId)
        {
            var facilities = (from cm in this.dbContext.Facilities.Where(cm => cm.Company_Id == companyId && cm.Facility_Status == 1)
                              select new FacilitiesbyCompanyIdEntity
                              {
                                  Facility_Id = cm.Facility_Id,
                                  Facility_Name = cm.Facility_Name
                              }).ToList();
            return facilities;

        }
        public List<NursingStationEntity> GetAllNurseStationsListNew(int facilityid)
        {

            //var nsids = (this.dbContext.CompanyBedConfigs.Where(cb => cb.Facility_Id == facilityid).Select(cb => cb.NurseStation_Id).Distinct()).ToList();
            var nurseStations = (from ns in this.dbContext.NursingStations
                                 join user in this.dbContext.Users on ns.NurseStation_CreatedBy equals user.User_Id
                                 join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                                 where ns.Facility_Id == facilityid && ns.NurseStation_Status == 1
                                 select new NursingStationEntity
                                 {
                                     NurseStation_Id = ns.NurseStation_Id,
                                     FacilityName = fa.Facility_Name,
                                     NurseStation_Name = ns.NurseStation_Name,
                                     NurseStation_Code = ns.NurseStation_Code,
                                     NurseStation_CreatedBy = ns.NurseStation_CreatedBy,
                                     NurseStation_CreatedDate = ns.NurseStation_CreatedDate,
                                     NurseStation_Status = ns.NurseStation_Status,
                                     UserName = user.UserName,
                                     NurseStationName = ns.NurseStation_Name
                                 }).OrderBy(item => item.NurseStationName).ToList();


            return nurseStations;

        }
        public List<NurseStationDropEntity> BedConfigNurseStationDrop()
        {
            var nurseDrop = this.dbContext.NursingStations.Where(n => n.NurseStation_Status == 1).ToList();
            return this.autoMapper.Map<List<NursingStation>, List<NurseStationDropEntity>>(nurseDrop);
        }
        public List<gpiEntity> GpiList(string Nursing,string fromdate,string todate,string Type)
        {
            if (Type == "undefined")
                Type = "1";


           return (from gp in this.dbContext.Prcgetgpidropdown(Nursing,Convert.ToDateTime(fromdate), Convert.ToDateTime(todate),Convert.ToInt16(Type))

            select new gpiEntity
            {
                ID = gp.ID ??0,

                GPI = gp.GPI,
              

            }).ToList();

        }
        public List<NurseStationDropEntity> UserNurseStationDrop(int userId)
        {
            return (from us in this.dbContext.UserRoleFacilityConfigs
                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                    where us.User_Id == userId && ns.NurseStation_Status == 1
                    select new NurseStationDropEntity
                    {
                        NurseStation_Id = ns.NurseStation_Id,
                        //NurseStation_Code =ns.NurseStation_Code,
                        NurseStation_Name = ns.NurseStation_Name,
                        NurseStation_Status = ns.NurseStation_Status

                    }).Distinct().OrderBy(item => item.NurseStation_Name).ToList();
        }

        public List<NurseStationDropEntity> UserNurseStationDropMultiple(int userId, string facilityId)
        {
            List<string> Fac = facilityId.Split(',').ToList();

            return (from us in this.dbContext.UserRoleFacilityConfigs
                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                    where us.User_Id == userId && ns.NurseStation_Status == 1 && (Fac.Any(al => us.Facility_id.ToString().Contains(al)))// us.Facility_id == facilityId
                    select new NurseStationDropEntity
                    {
                        NurseStation_Id = ns.NurseStation_Id,
                        NurseStation_Code = ns.NurseStation_Code,
                        NurseStation_Name = ns.NurseStation_Name,
                        NurseStation_Status = ns.NurseStation_Status,
                        PatientCount = (this.dbContext.VisitInfoes.Where(vs => vs.NursingStationId == ns.NurseStation_Id).Count())
                    }).Distinct().OrderBy(user => user.NurseStation_Name).ToList();
        }
        public List<NurseStationDropEntity> UserNurseStationDrop(int userId, int facilityId)
        {
            return (from us in this.dbContext.UserRoleFacilityConfigs
                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                    where us.User_Id == userId && ns.NurseStation_Status == 1 && us.Facility_id == facilityId
                    select new NurseStationDropEntity
                    {
                        NurseStation_Id = ns.NurseStation_Id,
                        NurseStation_Code = ns.NurseStation_Code,
                        NurseStation_Name = ns.NurseStation_Name,
                        NurseStation_Status = ns.NurseStation_Status,
                        PatientCount=(this.dbContext.VisitInfoes.Where(vs=>vs.NursingStationId==ns.NurseStation_Id).Count())
                    }).Distinct().OrderBy(user => user.NurseStation_Name).ToList();
        }
        public List<FacilityDropEntity> UserFacilityDrop(int userId)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("RoleId").Value != "")
            {
                int roleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                return (from us in this.dbContext.UserRoleFacilityConfigs
                        join f in this.dbContext.Facilities on us.Facility_id equals f.Facility_Id
                        where us.User_Id == userId && us.Role_ID == roleId && f.Facility_Status == 1
                        select new FacilityDropEntity
                        {
                            Facility_Id = f.Facility_Id,
                            Facility_Name = f.Facility_Name
                        }).Distinct().OrderBy(item => item.Facility_Name).ToList();
            }
            else
                return null;
        }
        public List<FloorDropEntity> UserFloorDrop(int userId)
        {
            int companyId = 1;
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            var floorIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
                            facilityIds.Contains((int)c.Facility_Id) && stationIds.Contains((int)c.NurseStation_Id)).Distinct()
                           .Select(c => c.Floor_Id);
            var floors = this.dbContext.Floors.Where(i => floorIds.Contains(i.Floor_Id) && i.Floor_Status == 1)
                .Select(f => new FloorDropEntity
                {
                    Floor_Id = f.Floor_Id,
                    Floor_Name = f.Floor_Name
                }).OrderBy(item => item.Floor_Name).ToList();
            return floors;
        }
        public List<WingDropEntity> UserWingDrop(int userId)
        {
            int companyId = 1;
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            var wingIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
                            facilityIds.Contains((int)c.Facility_Id) && stationIds.Contains((int)c.NurseStation_Id)).Distinct()
                           .Select(c => c.Wing_Id);
            var wings = this.dbContext.Wings.Where(i => wingIds.Contains(i.Wing_Id) && i.Wing_Status == 1)
                .Select(f => new WingDropEntity
                {
                    Wing_Id = f.Wing_Id,
                    Wing_Desc = f.Wing_Desc
                }).OrderBy(item => item.Wing_Desc).ToList();
            return wings;
        }
        public List<RoomDropEntity> UserRoomDrop(int userId)
        {
            int companyId = 1;
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            var roomIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
                            facilityIds.Contains((int)c.Facility_Id) && stationIds.Contains((int)c.NurseStation_Id)).Distinct()
                           .Select(c => c.Room_Id);
            var rooms = this.dbContext.Rooms.Where(i => roomIds.Contains(i.Room_Id) && i.Room_Status == 1)
                .Select(f => new RoomDropEntity
                {
                    Room_Id = f.Room_Id,
                    Room_Name = f.Room_Name
                }).OrderBy(item => item.Room_Name).ToList();
            return rooms;
        }
        public List<BedDropEntity> UserBedDrop(int userId)
        {
            int companyId = 1;
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            var bedIds = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId &&
                            facilityIds.Contains((int)c.Facility_Id) && stationIds.Contains((int)c.NurseStation_Id)).Distinct()
                           .Select(c => c.Bed_Id);
            var beds = this.dbContext.Beds.Where(i => bedIds.Contains(i.Bed_Id) && i.Bed_Status == 1)
                .Select(f => new BedDropEntity
                {
                    Bed_Id = f.Bed_Id,
                    Bed_Name = f.Bed_Name
                }).OrderBy(item => item.Bed_Name).ToList();
            return beds;
        }
        public int GetCompanyToBedFlag(int userId)
        {
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join f in this.dbContext.Facilities on us.Facility_id equals f.Facility_Id
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && f.Facility_Status == 1 && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();
            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> nsIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            int companyId = this.dbContext.Facilities.Where(f => facilityIds.Contains(f.Facility_Id)).Select(f => f.Company_Id).FirstOrDefault();

            var records = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && facilityIds.Contains((int)c.Facility_Id) && nsIds.Contains((int)c.NurseStation_Id) && c.BedConfig_Status == 1).ToList();
            if (records.Count() > 0)
                return 1;
            else
                return 0;

        }
        public int GetCompanyTimeFormat(int facilityId)
        {
            var timeType = (from fc in this.dbContext.Facilities
                            join cmp in this.dbContext.CompanyConfigs on fc.Company_Id equals cmp.Company_Id
                            where fc.Facility_Id == facilityId
                            select cmp.TimeFormat).FirstOrDefault();
            //var cmpId = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Company_Id).FirstOrDefault();
            //var timeType = this.dbContext.CompanyConfigs.Where(c => c.Company_Id == cmpId).Select(c => c.TimeFormat).FirstOrDefault();
            return timeType != null ? (int)timeType : 0;
        }
        public List<NurseShiftEntity> GetNurseShifts(int nurseStationId, int facilityId)
        {
            List<NurseShiftEntity> list = new List<NurseShiftEntity>();
            NurseShiftEntity data = new NurseShiftEntity();
            int cmpTimeFormate = this.GetCompanyTimeFormat(facilityId);
            if (cmpTimeFormate == 0)
            {
                var Shifts = this.dbContext.NurseShifts.Where(nr => nr.NurseStation_Id == nurseStationId && nr.NurseShifts_Status==1).ToList();
                if (Shifts != null && Shifts.Count > 0)
                {
                    foreach (var item in Shifts)
                    {
                        data = new NurseShiftEntity();
                        int fromHour = item.Fromtime_hoursId == null ? 0 : (int)item.Fromtime_hoursId;
                        int toHour = item.Totime_hoursId == null ? 0 : (int)item.Totime_hoursId;
                        var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                        var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                        data.NurseStation_Id = nurseStationId;
                        data.NurseShifts_Name = item.NurseShifts_Name;
                        data.NurseShifts_Id = item.NurseShifts_Id;
                        data.Fromtime_hoursId = fromHour;
                        data.Totime_hoursId = toHour;
                        data.FromHours = from == null ? "" : from.RegularTime + " " + from.RegularTimeFormat;
                        data.ToHours = to == null ? "" : to.RegularTime + " " + to.RegularTimeFormat;
                        data.FromTimeFormat = "";
                        data.ToTimeFormat = "";
                        data.NurseShifts_Status = item.NurseShifts_Status;
                        data.NurseShifts_CreatedBy = item.NurseShifts_CreatedBy;
                        data.NurseShifts_CreatedOn = item.NurseShifts_CreatedOn;
                        list.Add(data);
                    }

                }
            }
            if (cmpTimeFormate == 1)
            {
                var Shifts = this.dbContext.NurseShifts.Where(nr => nr.NurseStation_Id == nurseStationId && nr.NurseShifts_Status == 1).ToList();
                if (Shifts != null && Shifts.Count > 0)
                {
                    foreach (var item in Shifts)
                    {
                        data = new NurseShiftEntity();
                        int fromHour = item.Fromtime_hoursId == null ? 0 : (int)item.Fromtime_hoursId;
                        int toHour = item.Totime_hoursId == null ? 0 : (int)item.Totime_hoursId;
                        var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                        var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                        data.NurseStation_Id = nurseStationId;
                        data.NurseShifts_Name = item.NurseShifts_Name;
                        data.NurseShifts_Id = item.NurseShifts_Id;
                        data.Fromtime_hoursId = fromHour;
                        data.Totime_hoursId = toHour;
                        data.FromHours = from.Hour_Desc;
                        data.ToHours = to.Hour_Desc;
                        data.NurseShifts_Status = item.NurseShifts_Status;
                        data.NurseShifts_CreatedBy = item.NurseShifts_CreatedBy;
                        data.NurseShifts_CreatedOn = item.NurseShifts_CreatedOn;
                        data.FromTimeFormat = null;
                        data.ToTimeFormat = null;
                        list.Add(data);
                    }
                }
            }
            return list;
        }
        public int UpdateNursestationsStatus(List<NursingStationEntity> data)
        {
            foreach (var item in data)
            {
                item.NurseStation_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.NursingStations.Find(item.NurseStation_Id);
                if (record != null)
                {
                    record.NurseStation_Status = record.NurseStation_Status == 1 ? 0 : 1;
                    record.NurseStation_CreatedBy = item.NurseStation_CreatedBy;
                    record.NurseStation_CreatedDate = item.NurseStation_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateFacilityStatus(List<FacilityCustomEntity> data)
        {
            foreach (var item in data)
            {
                item.Facility_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Facilities.Find(item.Facility_Id);
                if (record != null)
                {
                    record.Facility_Status = record.Facility_Status == 1 ? 0 : 1;
                    record.Facility_CreatedBy = item.Facility_CreatedBy;
                    record.Facility_CreatedDate = item.Facility_CreatedDate;
                    this.dbContext.SaveChanges();
                }

            }
            return 1;
        }
        public int UpdateFloorStatus(List<FloorEntity> data)
        {
            foreach (var item in data)
            {
                item.Floor_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Floors.Find(item.Floor_Id);
                if (record != null)
                {
                    record.Floor_Status = record.Floor_Status == 1 ? 0 : 1;
                    record.Floor_CreatedBy = item.Floor_CreatedBy;
                    record.Floor_CreatedDate = item.Floor_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }

        public int UpdateWingStatus(List<WingEntity> data)
        {
            foreach (var item in data)
            {
                item.Wing_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Wings.Find(item.Wing_Id);
                if (record != null)
                {
                    record.Wing_Status = record.Wing_Status == 1 ? 0 : 1;
                    record.Wing_CreatedBy = item.Wing_CreatedBy;
                    record.Wing_CreatedDate = item.Wing_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateRoomStatus(List<RoomEntity> data)
        {
            foreach (var item in data)
            {
                item.Room_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Rooms.Find(item.Room_Id);
                if (record != null)
                {
                    record.Room_Status = record.Room_Status == 1 ? 0 : 1;
                    record.Room_CreatedBy = item.Room_CreatedBy;
                    record.Room_CreatedDate = item.Room_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateCompanyBedConfigsStatus(List<CompanyBedConfigEntity> data)
        {
            foreach (var item in data)
            {
                item.BedConfig_CreatedDate =Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.CompanyBedConfigs.Find(item.BedConfig_Id);
                if (record != null)
                {
                    record.BedConfig_Status = record.BedConfig_Status == 1 ? 0 : 1;
                    record.BedConfig_CreatedBy = item.BedConfig_CreatedBy;
                    record.BedConfig_CreatedDate = item.BedConfig_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateBedStatus(List<BedEntity> data)
        {
            foreach (var item in data)
            {
                item.Bed_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Beds.Find(item.Bed_Id);
                if (record != null)
                {
                    record.Bed_Status = record.Bed_Status == 1 ? 0 : 1;
                    record.Bed_CreatedBy = item.Bed_CreatedBy;
                    record.Bed_CreatedDate = item.Bed_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }

        public List<FloorDropEntity> GetAllActiveFloorNames()
        {
            return this.dbContext.Floors.Where(c => c.Floor_Status == 1).Select(c => new FloorDropEntity { Floor_Id = c.Floor_Id, Floor_Name = c.Floor_Name }).OrderBy(item => item.Floor_Name).ToList();
        }
        public List<WingDropEntity> GetAllActiveWingNames()
        {
            return this.dbContext.Wings.Where(w => w.Wing_Status == 1).Select(w => new WingDropEntity { Wing_Id = w.Wing_Id, Wing_Desc = w.Wing_Desc }).OrderBy(item => item.Wing_Desc).ToList();
        }
        public List<RoomDropEntity> GetAllActiveRoomNames()
        {
            return this.dbContext.Rooms.Where(r => r.Room_Status == 1).Select(c => new RoomDropEntity
            {
                Room_Id = c.Room_Id,
                Room_Name = c.Room_Name,
                Room_Code = c.Room_Code,
                Room = c.Room_Name
            }).OrderBy(item => item.Room_Name).ToList();
        }
        public List<BedDropEntity> GetAllActiveBedNames()
        {
            return this.dbContext.Beds.Where(r => r.Bed_Status == 1).Select(b => new BedDropEntity
            {
                Bed_Id = b.Bed_Id,
                Bed_Name = b.Bed_Name,
                Bed_Code = b.Bed_Code,
                Bed = b.Bed_Name
            }).OrderBy(item => item.Bed_Name).ToList();
        }
        public CompanyToBedEntity GetCompanyToBedFlagByFacId(int userId, int facilityId, string nurseStations)
        {
            //var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
            //                                join f in this.dbContext.Facilities on us.Facility_id equals f.Facility_Id
            //                                join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
            //                                where us.User_Id == userId && f.Facility_Status == 1 && ns.NurseStation_Status == 1
            //                                select new
            //                                {
            //                                    NurseStationId = ns.NurseStation_Id,
            //                                    FacilityId = us.Facility_id
            //                                }).Distinct().ToList();
            //List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            //List<int> nsIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();
            List<CompanyBedConfig> records = new List<CompanyBedConfig>();
            string[] nsIds = null;
            int companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Company_Id).FirstOrDefault();
            if (nurseStations == "")
            {
                records = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId && c.BedConfig_Status == 1).ToList();
            }
            else if (nurseStations != "")
            {
                nsIds = nurseStations.ToString().Split(',');
                records = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == companyId && c.Facility_Id == facilityId && c.BedConfig_Status == 1 && nsIds.Contains(c.NurseStation_Id.ToString())).ToList();
            }
            CompanyToBedEntity data = new CompanyToBedEntity();
            if (records.Count() > 0)
            {
                // Company to Bed Floors
                var floorIds = records.Distinct().Select(c => c.Floor_Id);
                var floors = this.dbContext.Floors.Where(i => floorIds.Contains(i.Floor_Id) && i.Floor_Status == 1)
                    .Select(f => new FloorDropEntity
                    {
                        Floor_Id = f.Floor_Id,
                        Floor_Name = f.Floor_Name
                    }).OrderBy(item => item.Floor_Name).ToList();

                //Company To Bed Wings
                var wingIds = records.Distinct().Select(c => c.Wing_Id);
                var wings = this.dbContext.Wings.Where(i => wingIds.Contains(i.Wing_Id) && i.Wing_Status == 1)
                    .Select(f => new WingDropEntity
                    {
                        Wing_Id = f.Wing_Id,
                        Wing_Desc = f.Wing_Desc
                    }).OrderBy(item => item.Wing_Desc).ToList();

                //Company To Bed Rooms
                var roomIds = records.Distinct().Select(c => c.Room_Id);
                var rooms = this.dbContext.Rooms.Where(i => roomIds.Contains(i.Room_Id) && i.Room_Status == 1)
                    .Select(f => new RoomDropEntity
                    {
                        Room_Id = f.Room_Id,
                        Room_Name = f.Room_Name
                    }).OrderBy(item => item.Room_Name).ToList();
                //Company To Bed Beds
                var bedIds = records.Distinct().Select(c => c.Bed_Id);
                var beds = this.dbContext.Beds.Where(i => bedIds.Contains(i.Bed_Id) && i.Bed_Status == 1)
                    .Select(f => new BedDropEntity
                    {
                        Bed_Id = f.Bed_Id,
                        Bed_Name = f.Bed_Name
                    }).OrderBy(item => item.Bed_Name).ToList();
                data.companyBedFlag = 1;
                data.Floors = floors;
                data.Wings = wings;
                data.Rooms = rooms;
                data.Beds = beds;
                return data;
            }
            else
            {
                data.companyBedFlag = 0;
                return data;
            }
        }
        public int InsertUpateComputerName(ProcessKeyEntity entity)
        {
            clsEncDec encrypy = new clsEncDec();
            if (entity.ProcessID == 0)
            {
                ProcessKeyMaster record = new ProcessKeyMaster();
                record.ComputerName = entity.ComputerName;
                record.NursingStation_Id = entity.NurseStationId;
                record.ProcessKey = encrypy.psEncrypt("Biometric" + entity.ComputerName);
                record.ProcessID_CreatedBy = entity.ProcessID_CreatedBy;
                record.Status = 1;
                record.ProcessID_CreatedOn = DateTime.Now;
                this.dbContext.ProcessKeyMasters.Add(record);
                this.dbContext.SaveChanges();
                return 1;
            }
            else
            {
                var data = this.dbContext.ProcessKeyMasters.Where(p => p.ProcessID == entity.ProcessID).FirstOrDefault();
                data.ComputerName = entity.ComputerName;
                data.ProcessKey = encrypy.psEncrypt("Biometric" + entity.ComputerName); 
                data.ProcessID_CreatedBy = entity.ProcessID_CreatedBy;
                data.ProcessID_CreatedOn = DateTime.Now;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 1;
        }
        public List<ProcessMasterEntity> GetProcessKeyMasterList(int? nsId = null)
        {
            if (nsId == null)
            {
                var records = (from pr in this.dbContext.ProcessKeyMasters
                               join ns in this.dbContext.NursingStations on pr.NursingStation_Id equals ns.NurseStation_Id
                               where pr.Status == 1
                               select new ProcessMasterEntity
                               {
                                   ProcessID = pr.ProcessID,
                                   ProcessKey = pr.ProcessKey,
                                   NurseStationId = pr.NursingStation_Id,
                                   NurseStationName = ns.NurseStation_Name,
                                   ComputerName = pr.ComputerName
                               }).ToList();
                return records;
            }
            else
            {
                var records = (from pr in this.dbContext.ProcessKeyMasters
                               join ns in this.dbContext.NursingStations on pr.NursingStation_Id equals ns.NurseStation_Id
                               where pr.Status == 1 && ns.NurseStation_Id == nsId
                               select new ProcessMasterEntity
                               {
                                   ProcessID = pr.ProcessID,
                                   NurseStationId = ns.NurseStation_Id,
                                   NurseStationName = ns.NurseStation_Name,
                                   ComputerName = pr.ComputerName,
                                   ProcessKey = pr.ProcessKey,
                               }).ToList();
                return records;
            }
        }
        public List<NurseStationDropEntity> UserBiometricNurseStationDrop(int userId)
        {
            return (from us in this.dbContext.UserRoleFacilityConfigs
                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                    join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                    join x in this.dbContext.CompanyConfigs on fa.Company_Id equals x.Company_Id into comco
                    from companyConfig in comco.DefaultIfEmpty()
                    where us.User_Id == userId && ns.NurseStation_Status == 1 && companyConfig!=null && companyConfig.Fingersdesc_Id!=null
                    select new NurseStationDropEntity
                    {
                        NurseStation_Id = ns.NurseStation_Id,
                        NurseStation_Name = ns.NurseStation_Name,
                        NurseStation_Status = ns.NurseStation_Status
                    }).Distinct().OrderBy(item => item.NurseStation_Name).ToList();
        }
        public Tuple<int, string> GetDashboardMethodCount(int type,int nurseStationId)
        {
            var localtype = type - 2;
            var record = this.dbContext.NurseStationHierarchies.Where(nh => nh.NurseStation_Id == nurseStationId).FirstOrDefault();
            var facId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
            var cmpId = this.dbContext.Facilities.Where(f => f.Facility_Id == facId).Select(f => f.Company_Id).FirstOrDefault();
            var data = this.dbContext.CompanyBedConfigs.Where(c => c.Company_Id == cmpId && c.Facility_Id == facId && c.NurseStation_Id == nurseStationId && c.BedConfig_Status==1).ToList();
            if(record!=null && data.Count()>0)
            {
                if(record.FloorPrior== localtype)
                {
                    return new Tuple<int, string>(3, "Floor");
                }
                else if (record.WingPrior == localtype)
                {
                    return new Tuple<int, string>(4, "Wing");
                }
                else if (record.RoomPrior == localtype)
                {
                    return new Tuple<int, string>(5, "Room");
                }
                else if (record.BedPrior == localtype)
                {
                    return new Tuple<int, string>(6, "Bed");
                }
                else
                {
                    return new Tuple<int, string>(7, "Floor");
                }
            }
            else
            {
                return new Tuple<int, string>(7, "Residents");
            }
        }
        public List<NurseStationDropEntity> UsersConfigNurseStationDrop(string username, string password)
        {
            var checkUser = this.dbContext.Users.Where(us => us.UserName ==  username).FirstOrDefault();
            if (checkUser != null && checkUser.Password == password)
            {
                int? NewPasswordCheck = checkUser.NewUserFlag;
                var record = (from us in this.dbContext.UserRoleFacilityConfigs
                              join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                              join fa in this.dbContext.Facilities on ns.Facility_Id equals fa.Facility_Id
                              join x in this.dbContext.CompanyConfigs on fa.Company_Id equals x.Company_Id into comco
                              from companyConfig in comco.DefaultIfEmpty()
                              where us.User_Id == checkUser.User_Id && ns.NurseStation_Status == 1 && companyConfig != null/*&& companyConfig.Fingersdesc_Id != null*/
                              select new NurseStationDropEntity
                              {
                                  NurseStation_Id = ns.NurseStation_Id,
                                  NurseStation_Name = ns.NurseStation_Name,
                                  NewUserFlag=NewPasswordCheck
                              }).Distinct().OrderBy(item => item.NurseStation_Name).ToList();

                return record;
            }
            else
            {
                return null;
            }
        }
        public int RemoveComputerName(int processId)
        {
            var record = this.dbContext.ProcessKeyMasters.Where(item => item.ProcessID == processId).FirstOrDefault();
            if (record != null)
            {
                this.dbContext.ProcessKeyMasters.Remove(record);
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;

        }
        public List<CensusEntity> GetCensusByNursingStatios(int stationId)
        {
            List<CensusEntity> list = new List<CensusEntity>();
            CensusEntity obj = new CensusEntity();
            CensusEntity obj2= new CensusEntity();
            CensusEntity obj3 = new CensusEntity();
            string nsName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == stationId).Select(n => n.NurseStation_Name).FirstOrDefault();
            obj.CensusTypeId = 1;
            obj.NursingStation = nsName;
            obj.CensusType = "Active";
            obj.CensusCount = this.dbContext.VisitInfoes.Where(v => v.NursingStationId == stationId && v.PVisit_Status==1 ).Count();
            obj2.CensusTypeId = 2;
            obj2.NursingStation = nsName;
            obj2.CensusType = "Discharged";
            obj2.CensusCount= this.dbContext.VisitInfoes.Where(v => v.NursingStationId == stationId && v.PVisit_Status == 2).Count();
            obj3.CensusTypeId = 3;
            obj3.NursingStation = nsName;
            obj3.CensusType = "Temporary Absent";
            obj3.CensusCount = this.dbContext.VisitInfoes.Where(v => v.NursingStationId == stationId && v.PVisit_Status == 3).Count();
            list.Add(obj);
            list.Add(obj2);
            list.Add(obj3);
            return list;
        }
        public List<ResidentsEntity> GetCensusResidentsList(CompanyBedConfigCustomEntity entity)
        {
            var query = from vi in this.dbContext.VisitInfoes
                        join de in dbContext.Demographics on vi.Patient_Id equals de.Patient_Id
                        group new { dm = de, vis = vi, date = vi.PVisit_CreatedDate } by vi.Patient_Id into grouped
                        from g in grouped
                        where g.date == grouped.Max(recentVisit => recentVisit.date)
                        select g;
            var records = (from vi in query
                           join ns in this.dbContext.NursingStations on vi.vis.NursingStationId equals ns.NurseStation_Id
                           where entity.NurseStations.Contains(vi.vis.NursingStationId.HasValue ? vi.vis.NursingStationId.Value : 0) && vi.vis.PVisit_Status == entity.ResidentType
                           orderby ns.NurseStation_Name, vi.dm.PatientLastName
                           select new ResidentsEntity
                           {
                               Patient_Id = vi.dm.Patient_Id,
                               PatientLastName = vi.dm.PatientLastName,
                               PatientFirstName = vi.dm.PatientFirstName,
                               NurseStationName = ns.NurseStation_Name,
                               DOB = (vi.dm.DOB),
                               PatientGender = vi.dm.AdministrativeSex != "" ? vi.dm.AdministrativeSex == "M" ? "Male" : vi.dm.AdministrativeSex == "F" ? "Female" : "Unknown" : "",
                           }).Distinct().ToList();
            return records;
        }
        public List<tblTimeZoneEntity> GetTimeZones()
        {
            var record = this.dbContext.tblTimeZones.Where(tz => tz.Status == 1).ToList();
            return this.autoMapper.Map<List<tblTimeZone>,List<tblTimeZoneEntity>>(record);
        }
    }
}
