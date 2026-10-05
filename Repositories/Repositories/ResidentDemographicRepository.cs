using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.IO;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Security.Claims;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;

namespace LTCPro.Repositories
{
    public class ResidentDemographicRepository : IResidentDemographicRepository
    {

        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly ICommonRepository _commonRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        public ResidentDemographicRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, CommonRepository commonRepository, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._commonRepository = commonRepository;
            _userActivityRepository = userActivityRepository;
        }

        //public List<ResidentGridEntity> GetResidentGridData(string searchPattern, int userId)
        //{
        //    List<int> patientIds = null;
        //    var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
        //                                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
        //                                    where us.User_Id == userId && ns.NurseStation_Status == 1
        //                                    select new
        //                                    {
        //                                        NurseStationId = ns.NurseStation_Id,
        //                                        FacilityId = us.Facility_id
        //                                    }).Distinct().ToList();

        //    List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
        //    List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

        //    if (searchPattern == "Admit")
        //    {
        //        patientIds = this.dbContext.VisitInfoes.Where(p => p.DischargeDate == null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
        //    }
        //    else if (searchPattern == "Inactive")
        //    {
        //        //Findout the functionality
        //        patientIds = this.dbContext.Demographics.Where(p => p.Patient_Status == 0).Select(p => p.Patient_Id).ToList();
        //    }

        //    else if (searchPattern == "Readmit")
        //    {
        //        patientIds = this.dbContext.VisitInfoes.Where(p => p.ReAdmissionIndicator == "R" && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
        //    }

        //    else if (searchPattern == "Transfer")
        //    {
        //        patientIds = this.dbContext.VisitInfoes.Where(p => (p.PriorFacilityId != null || p.PriorNursingStationId != null || p.PriorFloor != null || p.PriorRoom != null || p.PriorBed != null) && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
        //    }
        //    else if (searchPattern == "Discharge")
        //    {
        //        var patientList = this.dbContext.VisitInfoes.Where(p => p.PVisit_Status == 1).Select(p => p.Patient_Id).ToList();
        //        var pIds = this.dbContext.VisitInfoes.Where(p => p.DischargeDate != null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();

        //        patientIds = pIds.Except(patientList).ToList();
        //    }
        //    else if (searchPattern == "All")
        //    {
        //        patientIds = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
        //    }

        //    if (patientIds != null)
        //    {
        //        return this.dbContext.Demographics.Where(p => patientIds.Distinct().Contains(p.Patient_Id)).AsEnumerable()
        //            .Join(this.dbContext.Users, d => d.Patient_CreatedBy, us => us.User_Id,
        //            (demographic, user) => new ResidentGridEntity()
        //            {
        //                Patient_Id = demographic.Patient_Id,
        //                PatientLastName = demographic.PatientLastName,
        //                PatientFirstName = demographic.PatientFirstName,
        //                PatientMiddleInitial = demographic.PatientMiddleInitial,
        //                PatientAddress1 = demographic.PatientAddress1,
        //                PatientAddress2 = demographic.PatientAddress2,
        //                PatientCity = demographic.PatientCity,
        //                PatientState = demographic.PatientState,
        //                PatientZipCode = demographic.PatientZipCode,
        //                PhoneHome = demographic.PhoneHome,
        //                ImageLocation = ConvertImage(demographic.ImageLocation),
        //                Patient_Status = demographic.Patient_Status,
        //                Patient_CreatedBy = demographic.Patient_CreatedBy,
        //                Patient_CreatedDate = demographic.Patient_CreatedDate,
        //                UserName = user.User_DisplayName
        //            }).ToList();
        //    }
        //    return null;

        //}
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
        public ResidentGridEntity GetResidentsByCompanyBed(CompanyBedConfigCustomEntity configs)
        {
            //Showing user access facilities and filters in UI so no need of this check
            //List<int> facilityIds = (from us in this.dbContext.UserRoleFacilityConfigs
            //                         where us.User_Id == configs.UserId && us.UserRole_Status == 1
            //                         select us.Facility_id).Distinct().ToList();

            List<string> floorsList = configs.Floors.ConvertAll<string>(delegate (int i)
            { return i.ToString(); });
            List<string> roomsList = configs.Rooms.ConvertAll<string>(delegate (int i)
            { return i.ToString(); });
            List<string> bedsList = configs.Beds.ConvertAll<string>(delegate (int i)
            { return i.ToString(); });


            var count = 0;
            var records = new List<ResidentsEntity>();
            string searchText = configs.SearchText != null && configs.SearchText != string.Empty ? configs.SearchText.ToLower() : string.Empty;
            if (searchText != string.Empty)
            {
                var query = from vi in this.dbContext.VisitInfoes
                            join de in dbContext.Demographics on vi.Patient_Id equals de.Patient_Id
                            group new { dm = de, vis = vi, date = vi.PVisit_CreatedDate } by vi.Patient_Id into grouped
                            from g in grouped
                            where g.date == grouped.Max(recentVisit => recentVisit.date)
                            select g;

                count = (from vi in query
                             //join dm in this.dbContext.Demographics on vi.vis.Patient_Id equals dm.Patient_Id
                         join ns in this.dbContext.NursingStations on vi.vis.NursingStationId equals ns.NurseStation_Id
                         join f in this.dbContext.Floors on vi.vis.Floor equals f.Floor_Id.ToString() into fl
                         from flo in fl.DefaultIfEmpty()
                         join w in this.dbContext.Wings on vi.vis.Wing equals w.Wing_Id into wi
                         from win in wi.DefaultIfEmpty()
                         join r in this.dbContext.Rooms on vi.vis.Room equals r.Room_Id.ToString() into ro
                         from roo in ro.DefaultIfEmpty()
                         join b in this.dbContext.Beds on vi.vis.Bed equals b.Bed_Id.ToString() into be
                         from bed in be.DefaultIfEmpty()
                         where (configs.NurseStations.Contains(vi.vis.NursingStationId.HasValue ? vi.vis.NursingStationId.Value : 0) &&
                         (configs.Floors.Count == 0 || floorsList.Contains(vi.vis.Floor)) &&
                         //(configs.Wings.Count == 0 || configs.Wings.Contains(vi.Wing.HasValue ? vi.NursingStationId.Value : 0)) &&
                         (configs.Wings.Count == 0 || configs.Wings.Contains(vi.vis.Wing.HasValue ? vi.vis.Wing.Value : 0)) &&
                         (configs.Rooms.Count == 0 || roomsList.Contains(vi.vis.Room)) &&
                         (configs.Beds.Count == 0 || bedsList.Contains(vi.vis.Bed))) &&
                         (vi.dm.PatientLastName.ToLower().Contains(searchText) || vi.dm.PatientFirstName.ToLower().Contains(searchText) || EntityFunctions.TruncateTime(vi.dm.DOB).ToString().Contains(searchText) || (vi.dm.AdministrativeSex != "" ? vi.dm.AdministrativeSex == "M" ? "Male" : vi.dm.AdministrativeSex == "F" ? "Female" : "Unknown" : "").ToLower().Contains(searchText) ||
                         ns.NurseStation_Code.ToLower().Contains(searchText) || ns.NurseStation_Name.ToLower().Contains(searchText) || flo.Floor_Name.ToLower().Contains(searchText) ||
                         win.Wing_Desc.ToLower().Contains(searchText) || roo.Room_Name.ToLower().Contains(searchText) || bed.Bed_Name.ToLower().Contains(searchText)) && vi.vis.PVisit_Status == configs.ResidentType
                         select vi.vis.Patient_Id).Distinct().Count();
                int skipRows = (configs.CurrentPage - 1) * configs.PageSize;

                records = (from vi in query
                               //join dm in this.dbContext.Demographics on vi.vis.Patient_Id equals dm.Patient_Id
                           join ns in this.dbContext.NursingStations on vi.vis.NursingStationId equals ns.NurseStation_Id
                           join f in this.dbContext.Floors on vi.vis.Floor equals f.Floor_Id.ToString() into fl
                           from flo in fl.DefaultIfEmpty()
                           join w in this.dbContext.Wings on vi.vis.Wing equals w.Wing_Id into wi
                           from win in wi.DefaultIfEmpty()
                           join r in this.dbContext.Rooms on vi.vis.Room equals r.Room_Id.ToString() into ro
                           from roo in ro.DefaultIfEmpty()
                           join b in this.dbContext.Beds on vi.vis.Bed equals b.Bed_Id.ToString() into be
                           from bed in be.DefaultIfEmpty()

                           where (configs.NurseStations.Contains(vi.vis.NursingStationId.HasValue ? vi.vis.NursingStationId.Value : 0) &&
                           (configs.Floors.Count == 0 || floorsList.Contains(vi.vis.Floor)) &&
                           (configs.Wings.Count == 0 || configs.Wings.Contains(vi.vis.Wing.HasValue ? vi.vis.Wing.Value : 0)) &&

                           (configs.Rooms.Count == 0 || roomsList.Contains(vi.vis.Room)) &&
                           (configs.Beds.Count == 0 || bedsList.Contains(vi.vis.Bed))) &&
                         (vi.dm.PatientLastName.ToLower().Contains(searchText) || vi.dm.PatientFirstName.ToLower().Contains(searchText) ||
                         (vi.dm.DOB == null ? "" : EntityFunctions.TruncateTime(vi.dm.DOB).ToString()).Contains(searchText) ||
                         (vi.dm.AdministrativeSex != "" ? (vi.dm.AdministrativeSex == "M" ? "Male" : vi.dm.AdministrativeSex == "F" ? "Female" : "Unknown") : "").ToLower().Contains(searchText) ||
                         ns.NurseStation_Code.ToLower().Contains(searchText) || ns.NurseStation_Name.ToLower().Contains(searchText) || flo.Floor_Name.ToLower().Contains(searchText) ||
                         win.Wing_Desc.ToLower().Contains(searchText) || roo.Room_Name.ToLower().Contains(searchText) || bed.Bed_Name.ToLower().Contains(searchText)) && vi.vis.PVisit_Status == configs.ResidentType
                           orderby ns.NurseStation_Name, vi.dm.PatientLastName
                           select new
                           {
                               Demographics = vi.dm,
                               NurseStation = ns,
                               VisitInfo = vi.vis,
                               Floor = flo,
                               Wing = win,
                               Room = roo,
                               Bed = bed
                           }).Skip(skipRows).Take(configs.PageSize).AsEnumerable()
                                .Select(x => new ResidentsEntity()
                                {
                                    Patient_Id = x.Demographics.Patient_Id,
                                    PatientLastName = x.Demographics.PatientLastName,
                                    PatientFirstName = x.Demographics.PatientFirstName,
                                    PatientMiddleInitial = x.Demographics.PatientMiddleInitial,
                                    AliasName = x.Demographics.AliasName != null ? "(" + x.Demographics.AliasName + ")" : "",
                                    //PatientAddress1 = item.Demographics.PatientAddress1,
                                    //PatientAddress2 = item.Demographics.PatientAddress2,
                                    //PatientCity = item.Demographics.PatientCity,
                                    //PatientState = item.Demographics.PatientState,
                                    //PatientZipCode = item.Demographics.PatientZipCode,
                                    //PhoneHome = item.Demographics.PhoneHome,
                                    // ImageLocation = CheckResImageinBiometric(x.Demographics.PatientMRNumber) == "" ? ConvertImage(x.Demographics.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(x.Demographics.PatientMRNumber)),

                                    imgchk = x.Demographics.ImageLocation != null ? "Yes" : "No",


                                    ImageLocation = null,
                                    Patient_Status = x.Demographics.Patient_Status,
                                    //Patient_CreatedBy = item.Demographics.Patient_CreatedBy,
                                    //Patient_CreatedDate = item.Demographics.Patient_CreatedDate,
                                    //UserName = this.dbContext.Users.Where(us => us.User_Id == item.Demographics.Patient_CreatedBy).Select(us => us.User_DisplayName).FirstOrDefault(),
                                    NurseStationName = x.NurseStation.NurseStation_Name,
                                    FloorName = x.Floor != null ? x.Floor.Floor_Name : "",
                                    WingName = x.Wing != null ? x.Wing.Wing_Desc : "",
                                    RoomName = x.Room != null ? x.Room.Room_Name : "",
                                    BedName = x.Bed != null ? x.Bed.Bed_Name : "",
                                    PatientDOB = Convert.ToDateTime(x.Demographics.DOB).ToString("MM/dd/yyyy"),
                                    PatientGender = x.Demographics.AdministrativeSex != "" ? x.Demographics.AdministrativeSex == "M" ? "Male" : x.Demographics.AdministrativeSex == "F" ? "Female" : "Unknown" : "",
                                }).ToList();
            }
            else
            {
                var query = from vi in this.dbContext.VisitInfoes
                            join de in dbContext.Demographics on vi.Patient_Id equals de.Patient_Id
                            group new { dm = de, vis = vi, date = vi.PVisit_CreatedDate } by vi.Patient_Id into grouped
                            from g in grouped
                            where g.date == grouped.Max(taskInGroup => taskInGroup.date)
                            select g;

                count = (from vi in query
                             //join dm in this.dbContext.Demographics on vi.vis.Patient_Id equals dm.Patient_Id
                         join ns in this.dbContext.NursingStations on vi.vis.NursingStationId equals ns.NurseStation_Id
                         join f in this.dbContext.Floors on vi.vis.Floor equals f.Floor_Id.ToString() into fl
                         from flo in fl.DefaultIfEmpty()
                         join w in this.dbContext.Wings on vi.vis.Wing equals w.Wing_Id into wi
                         from win in wi.DefaultIfEmpty()
                         join r in this.dbContext.Rooms on vi.vis.Room equals r.Room_Id.ToString() into ro
                         from roo in ro.DefaultIfEmpty()
                         join b in this.dbContext.Beds on vi.vis.Bed equals b.Bed_Id.ToString() into be
                         from bed in be.DefaultIfEmpty()
                         where (configs.NurseStations.Contains(vi.vis.NursingStationId.HasValue ? vi.vis.NursingStationId.Value : 0) &&
                         (configs.Floors.Count == 0 || floorsList.Contains(vi.vis.Floor)) &&
                         (configs.Wings.Count == 0 || configs.Wings.Contains(vi.vis.Wing.HasValue ? vi.vis.Wing.Value : 0)) &&
                         (configs.Rooms.Count == 0 || roomsList.Contains(vi.vis.Room)) &&
                         (configs.Beds.Count == 0 || bedsList.Contains(vi.vis.Bed))) && vi.vis.PVisit_Status == configs.ResidentType
                         select vi.vis.Patient_Id).Distinct().Count();
                int skipRows = (configs.CurrentPage - 1) * configs.PageSize;
                records = (from vi in query
                               //join dm in this.dbContext.Demographics on vi.vis.Patient_Id equals dm.Patient_Id
                           join ns in this.dbContext.NursingStations on vi.vis.NursingStationId equals ns.NurseStation_Id
                           join f in this.dbContext.Floors on vi.vis.Floor equals f.Floor_Id.ToString() into fl
                           from flo in fl.DefaultIfEmpty()
                           join w in this.dbContext.Wings on vi.vis.Wing equals w.Wing_Id into wi
                           from win in wi.DefaultIfEmpty()
                           join r in this.dbContext.Rooms on vi.vis.Room equals r.Room_Id.ToString() into ro
                           from roo in ro.DefaultIfEmpty()
                           join b in this.dbContext.Beds on vi.vis.Bed equals b.Bed_Id.ToString() into be
                           from bed in be.DefaultIfEmpty()
                           where (configs.NurseStations.Contains(vi.vis.NursingStationId.HasValue ? vi.vis.NursingStationId.Value : 0) &&
                           (configs.Floors.Count == 0 || floorsList.Contains(vi.vis.Floor)) &&
                           (configs.Wings.Count == 0 || configs.Wings.Contains(vi.vis.Wing.HasValue ? vi.vis.Wing.Value : 0)) &&
                           (configs.Rooms.Count == 0 || roomsList.Contains(vi.vis.Room)) &&
                           (configs.Beds.Count == 0 || bedsList.Contains(vi.vis.Bed))) && vi.vis.PVisit_Status == configs.ResidentType
                           orderby ns.NurseStation_Name, vi.dm.PatientLastName
                           select new
                           {
                               Demographics = vi.dm,
                               NurseStation = ns,
                               VisitInfo = vi,
                               Floor = flo,
                               Wing = win,
                               Room = roo,
                               Bed = bed
                           }).Skip(skipRows).Take(configs.PageSize).AsEnumerable()
                                .Select(x => new ResidentsEntity()
                                {
                                    Patient_Id = x.Demographics.Patient_Id,
                                    PatientLastName = x.Demographics.PatientLastName,
                                    PatientFirstName = x.Demographics.PatientFirstName,
                                    PatientMiddleInitial = x.Demographics.PatientMiddleInitial,
                                    AliasName = x.Demographics.AliasName != null ? "(" + x.Demographics.AliasName + ")" : "",
                                    //PatientAddress1 = item.Demographics.PatientAddress1,
                                    //PatientAddress2 = item.Demographics.PatientAddress2,
                                    //PatientCity = item.Demographics.PatientCity,
                                    //PatientState = item.Demographics.PatientState,
                                    //PatientZipCode = item.Demographics.PatientZipCode,
                                    //PhoneHome = item.Demographics.PhoneHome,
                                    // ImageLocation = CheckResImageinBiometric(x.Demographics.PatientMRNumber) == "" ? ConvertImage(x.Demographics.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(x.Demographics.PatientMRNumber)),
                                    ImageLocation = null,
                                    imgchk = x.Demographics.ImageLocation != null ? "Yes" : "No",
                                    Patient_Status = x.Demographics.Patient_Status,
                                    //Patient_CreatedBy = item.Demographics.Patient_CreatedBy,
                                    //Patient_CreatedDate = item.Demographics.Patient_CreatedDate,
                                    //UserName = this.dbContext.Users.Where(us => us.User_Id == item.Demographics.Patient_CreatedBy).Select(us => us.User_DisplayName).FirstOrDefault(),
                                    NurseStationName = x.NurseStation.NurseStation_Name,
                                    FloorName = x.Floor != null ? x.Floor.Floor_Name : "",
                                    WingName = x.Wing != null ? x.Wing.Wing_Desc : "",
                                    RoomName = x.Room != null ? x.Room.Room_Name : "",
                                    BedName = x.Bed != null ? x.Bed.Bed_Name : "",
                                    PatientDOB = x.Demographics.DOB == null ? "" : Convert.ToDateTime(x.Demographics.DOB).ToString("MM/dd/yyyy"),
                                    PatientGender = x.Demographics.AdministrativeSex != "" ? (x.Demographics.AdministrativeSex == "M" ? "Male" : x.Demographics.AdministrativeSex == "F" ? "Female" : "Unknown") : "",
                                }).ToList();
            }
            var list = new ResidentGridEntity
            {
                TotalRecords = count,
                Data = records
            };

            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "" && configs.RecentFacNsFalg == 1)
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (configs.NurseStations.Count() > 0)
                {
                    var selectedNurseStations = string.Join(",", configs.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = configs.Facilities[0];
                    userRecentFacObj.NurseStation_Id = selectedNurseStations;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            return list;
            //var q1 = this.dbContext.VisitInfoes.Where(p => configs.Facilities.Contains((int)p.FacilityId)).ToList();
            //var q2 = q1;
            //if (configs.NurseStations.Count != 0)
            //{
            //    q2 = q1.Where(p => configs.NurseStations.Contains(Convert.ToInt32(p.NursingStationId))).ToList();
            //}
            //var q3 = q2;
            //if (configs.Floors.Count != 0)
            //{
            //    q3 = q2.Where(p => configs.Floors.Contains(Convert.ToInt32(p.Floor))).ToList();
            //}
            //var q4 = q3;
            //if (configs.Wings.Count != 0)
            //{
            //    q4 = q3.Where(w => configs.Wings.Contains(Convert.ToInt32(w.Wing))).ToList();
            //}
            //var q5 = q4;
            //if (configs.Rooms.Count != 0)
            //{
            //    q5 = q4.Where(p => configs.Rooms.Contains(Convert.ToInt32(p.Room))).ToList();
            //}
            //var q6 = q5;
            //if (configs.Beds.Count != 0)
            //{
            //    q6 = q5.Where(p => configs.Beds.Contains(Convert.ToInt32(p.Bed))).ToList();
            //}
            //List<ResidentGridEntity> residentGridList = new List<ResidentGridEntity>();
            //ResidentGridEntity entity;
            //if (q6.Count() > 0)
            //{
            //    //var patientIds = q6.Select(p => p.Patient_Id);
            //    //var list = this.dbContext.Demographics.Where(p => patientIds.Contains(p.Patient_Id));
            //    var patientIds = q6.Select(p => p.Patient_Id);
            //    //ToDo: This needs to be changed. Will not work incase of more than one entry in VisitInfo
            //    var list = (from dm in this.dbContext.Demographics
            //                join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
            //                join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
            //                where patientIds.Contains(dm.Patient_Id)
            //                select new
            //                {
            //                    Demographics = dm,
            //                    NurseStation = ns,
            //                    Visit = vi,
            //                }).ToList();

            //foreach (var item in records)
            //{
            //    //int floorId = item.Visit.Floor == "" || item.Visit.Floor == null ? 0 : Convert.ToInt32(item.Visit.Floor);
            //    //string floorName = this.dbContext.Floors.Where(f => f.Floor_Id == floorId).Select(f => f.Floor_Name).FirstOrDefault();
            //    //int wingId = item.Visit.Wing == null ? 0 : Convert.ToInt32(item.Visit.Wing);
            //    //string wingName = this.dbContext.Wings.Where(w => w.Wing_Id == wingId).Select(w => w.Wing_Desc).FirstOrDefault();
            //    //int roomId = item.Visit.Room == "" || item.Visit.Room == null ? 0 : Convert.ToInt32(item.Visit.Room);
            //    //string roomName = this.dbContext.Rooms.Where(r => r.Room_Id == roomId).Select(r => r.Room_Name).FirstOrDefault();
            //    //int bedId = item.Visit.Bed == "" || item.Visit.Bed == null ? 0 : Convert.ToInt32(item.Visit.Bed);
            //    //string bedName = this.dbContext.Beds.Where(b => b.Bed_Id == bedId).Select(b => b.Bed_Name).FirstOrDefault();
            //    entity = new ResidentGridEntity
            //    {
            //        Patient_Id = item.Demographics.Patient_Id,
            //        PatientLastName = item.Demographics.PatientLastName,
            //        PatientFirstName = item.Demographics.PatientFirstName,
            //        PatientMiddleInitial = item.Demographics.PatientMiddleInitial,
            //        PatientAddress1 = item.Demographics.PatientAddress1,
            //        PatientAddress2 = item.Demographics.PatientAddress2,
            //        PatientCity = item.Demographics.PatientCity,
            //        PatientState = item.Demographics.PatientState,
            //        PatientZipCode = item.Demographics.PatientZipCode,
            //        PhoneHome = item.Demographics.PhoneHome,
            //        ImageLocation = ConvertImage(item.Demographics.ImageLocation),
            //        Patient_Status = item.Demographics.Patient_Status,
            //        Patient_CreatedBy = item.Demographics.Patient_CreatedBy,
            //        Patient_CreatedDate = item.Demographics.Patient_CreatedDate,
            //        UserName = this.dbContext.Users.Where(us => us.User_Id == item.Demographics.Patient_CreatedBy).Select(us => us.User_DisplayName).FirstOrDefault(),
            //        NurseStationName = item.NurseStation.NurseStation_Code + " - " + item.NurseStation.NurseStation_Name,
            //        FloorName = item.Floor != null ? item.Floor.Floor_Name : "",
            //        WingName = item.Wing != null ? item.Wing.Wing_Desc : "",
            //        RoomName = item.Room != null ? item.Room.Room_Code : "",
            //        BedName = item.Bed != null ? item.Bed.Bed_Name : "",
            //    };

            //    residentGridList.Add(entity);
            //}
            //var list = this.dbContext.Demographics.Where(p => patientIds.Contains(p.Patient_Id));

            //foreach (Demographic p in list)
            //{

            //    entity = new ResidentGridEntity
            //    {
            //        Patient_Id = p.Patient_Id,
            //        PatientLastName = p.PatientLastName,
            //        PatientFirstName = p.PatientFirstName,
            //        PatientMiddleInitial = p.PatientMiddleInitial,
            //        PatientAddress1 = p.PatientAddress1,
            //        PatientAddress2 = p.PatientAddress2,
            //        PatientCity = p.PatientCity,
            //        PatientState = p.PatientState,
            //        PatientZipCode = p.PatientZipCode,
            //        PhoneHome = p.PhoneHome,
            //        ImageLocation = ConvertImage(p.ImageLocation),
            //        Patient_Status = p.Patient_Status,
            //        Patient_CreatedBy = p.Patient_CreatedBy,
            //        Patient_CreatedDate = p.Patient_CreatedDate,
            //        UserName = this.dbContext.Users.Where(us => us.User_Id == p.Patient_CreatedBy).Select(us => us.User_DisplayName).FirstOrDefault(),
            //    };

            //    residentGridList.Add(entity);
            //}
            //}
            //return residentGridList.OrderBy(item => item.NurseStationName).ThenBy(item => item.FloorName).ThenBy(item => item.PatientLastName).ToList();
            //return residentGridList;
        }
        public List<ResidentAdmitDischargeEntity> GetResidentAdmitDischargeData(int patientID)
        {
            DateTime admit, discharge;
            TimeSpan diff;
            var visitInfo = this.dbContext.VisitInfoes.Where(vi => vi.Patient_Id == patientID).ToList();
            List<ResidentAdmitDischargeEntity> residentAdmitDischrageList = new List<ResidentAdmitDischargeEntity>();
            for (int i = 0; i < visitInfo.Count; i++)
            {
                ResidentAdmitDischargeEntity residentAdmit = new ResidentAdmitDischargeEntity();
                admit = Convert.ToDateTime(visitInfo[i].AdmitDate);
                if (visitInfo[i].DischargeDate == null)
                    discharge = DateTime.Now;
                else
                    discharge = Convert.ToDateTime(visitInfo[i].DischargeDate);
                diff = discharge - admit;
                residentAdmit.NoofDays = diff.Days + 1;
                residentAdmit.Patient_Id = visitInfo[i].Patient_Id;
                residentAdmit.PVisit_Id = visitInfo[i].PVisit_Id;
                residentAdmit.AdmitDate = Convert.ToDateTime(visitInfo[i].AdmitDate);
                residentAdmit.DischargeDate = visitInfo[i].DischargeDate != null ? discharge : visitInfo[i].DischargeDate;
                residentAdmit.PVisit_Status = visitInfo[i].PVisit_Status;
                residentAdmitDischrageList.Add(residentAdmit);
            }
            return residentAdmitDischrageList;
        }
        public List<ResidentDropEntity> GetResidentsByNSId(int nurseStationId)
        {
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();

            RecentFacEntity userRecentFacObj = new RecentFacEntity()
            {
                User_Id = 0,
                Facility_Id = (int)facilityId,
                NurseStation_Id = nurseStationId.ToString()
            };
            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            //ToDo: Search Nursestation of current or latest admit record
            return (from d in this.dbContext.Demographics
                    join v in this.dbContext.VisitInfoes on d.Patient_Id equals v.Patient_Id
                    where v.NursingStationId == nurseStationId && v.PVisit_Status != 10
                    select new ResidentDropEntity()
                    {
                        Patient_Id = d.Patient_Id,
                        PatientName = d.PatientLastName + ", " + d.PatientFirstName + " " + (d.PatientMiddleInitial == null ? "" : d.PatientMiddleInitial),
                        PVisit_Status = v.PVisit_Status
                    }).Distinct().OrderBy(item => item.PatientName).ToList();
        }
        public List<ResidentDropEntity> GetResidentsByNSIds(string nurseStationIds)
        {
            RecentFacEntity userRecentFacObj = new RecentFacEntity();
            userRecentFacObj.User_Id = 0;
            userRecentFacObj.Facility_Id = 0;
            userRecentFacObj.NurseStation_Id = nurseStationIds;
            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);

            //ToDo: Search Nursestation of current or latest admit record
            var stations = nurseStationIds.Split(',');
            return (from d in this.dbContext.Demographics
                    join v in this.dbContext.VisitInfoes on d.Patient_Id equals v.Patient_Id
                    where stations.Contains(v.NursingStationId.ToString())
                    select new ResidentDropEntity()
                    {
                        Patient_Id = d.Patient_Id,
                        PatientName = d.PatientLastName + " " + d.PatientFirstName + " " + d.PatientMiddleInitial
                    }).Distinct().OrderBy(item => item.PatientName).ToList();
        }
        public DemographicInfoEntity GetResidentInformation(int patientID)
        {
            var demographic = this.dbContext.Demographics.Where(dm => dm.Patient_Id == patientID).FirstOrDefault();
            var visitInfoData = this.dbContext.VisitInfoes.Where(dm => dm.Patient_Id == patientID);
            var patientType = this.dbContext.PatientTypes.Where(dm => dm.PatientType_Id == demographic.PatientType_Id);
            if (visitInfoData.Count() > 1)
            {
                var notDischargedRecord = visitInfoData.Where(v => v.DischargeDate == null);
                if (notDischargedRecord.Count() == 0)
                    visitInfoData = visitInfoData.OrderByDescending(v => v.DischargeDate);
                else
                    visitInfoData = notDischargedRecord;
            }

            if (demographic != null)
            {
                DemographicInfoEntity residentInfo = new DemographicInfoEntity();
                residentInfo.Patient_Id = demographic.Patient_Id;
                residentInfo.PatientLastName = demographic.PatientLastName;
                residentInfo.PatientFirstName = demographic.PatientFirstName;
                residentInfo.PatientMiddleInitial = demographic.PatientMiddleInitial;
                residentInfo.AliasName = demographic.AliasName;
                residentInfo.DOB = demographic.DOB;
                if (demographic.AdministrativeSex == "M")
                    residentInfo.AdministrativeSex = "Male";
                else if (demographic.AdministrativeSex == "F")
                    residentInfo.AdministrativeSex = "Female";
                else if (demographic.AdministrativeSex == "T")
                    residentInfo.AdministrativeSex = "Transgender";
                else if (demographic.AdministrativeSex == "U")
                    residentInfo.AdministrativeSex = "Unknown";
                residentInfo.PatientAddress1 = demographic.PatientAddress1;
                residentInfo.PatientAddress2 = demographic.PatientAddress2;
                residentInfo.PhoneHome = demographic.PhoneHome;
                residentInfo.PatientMRNumber = demographic.PatientMRNumber;
                residentInfo.SSN = demographic.SSN;
                //residentInfo.DeathIndicator = demographic.DeathIndicator;
                residentInfo.Patient_Status = demographic.Patient_Status;
                residentInfo.Patient_CreatedBy = demographic.Patient_CreatedBy;
                residentInfo.Patient_CreatedDate = demographic.Patient_CreatedDate;
                residentInfo.ImageLocation = CheckResImageinBiometric(demographic.PatientMRNumber) == "" ? ConvertImage(demographic.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(demographic.PatientMRNumber));
                residentInfo.PatientCity = demographic.PatientCity;
                residentInfo.PatientState = demographic.PatientState;
                residentInfo.PatientZipcode = demographic.PatientZipCode;
                residentInfo.NurseStation_Name = GetNurseStationName(patientID);
                residentInfo.NursingStationId = visitInfoData.FirstOrDefault().NursingStationId == null ? 0 : (int)visitInfoData.FirstOrDefault().NursingStationId;
                residentInfo.FacilityId = visitInfoData.FirstOrDefault().FacilityId == null ? 0 : (int)visitInfoData.FirstOrDefault().FacilityId;
                residentInfo.WingId = visitInfoData.FirstOrDefault().Wing == null ? 0 : (int)visitInfoData.FirstOrDefault().Wing;
                residentInfo.RoomId = visitInfoData.FirstOrDefault().Room == null ? 0 : Convert.ToInt32(visitInfoData.FirstOrDefault().Room);
                residentInfo.BedId = visitInfoData.FirstOrDefault().Bed == null ? 0 : Convert.ToInt32(visitInfoData.FirstOrDefault().Bed);
                residentInfo.FloorId = visitInfoData.FirstOrDefault().Floor == null ? 0 : Convert.ToInt32(visitInfoData.FirstOrDefault().Floor);
                residentInfo.HomeFlag = visitInfoData.FirstOrDefault().HomeFlag == null ? 0 : (int)(visitInfoData.FirstOrDefault().HomeFlag);
                residentInfo.PVisit_Status = visitInfoData.FirstOrDefault().PVisit_Status;
                residentInfo.PVisit_Id = visitInfoData.FirstOrDefault().PVisit_Id;
                residentInfo.DietType = visitInfoData.FirstOrDefault().DietType == null ? "" : visitInfoData.FirstOrDefault().DietType;
                residentInfo.Pregnant = demographic.Pregnant;
                residentInfo.BreastFeeding = demographic.BreastFeeding;
                return residentInfo;
            }
            return null;
        }
        public string GetNurseStationName(int patientId)
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
            return this.dbContext.NursingStations.Where(n => n.NurseStation_Id == visitInfoData.FirstOrDefault().NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();
        }
        public DemographicEntity GetResidentDemographicData(int patientId)
        {
            var residentdemograhic = this.dbContext.Demographics.Where(dm => dm.Patient_Id == patientId).FirstOrDefault();
            var approvaldemographic = this.dbContext.ApprovalDemographics.Where(ad => ad.Patient_Id == patientId && (ad.PDOutBoundApproval == null || ad.PDOutBoundApproval == 0)).FirstOrDefault();

            DemographicEntity residentInfo = new DemographicEntity();
            residentInfo.Patient_Id = residentdemograhic.Patient_Id;
            residentInfo.ExternalPatientId = residentdemograhic.ExternalPatientId;
            residentInfo.ExternalFacShortName = residentdemograhic.ExternalFacShortName;
            residentInfo.ExternalFacPatientId = residentdemograhic.ExternalFacPatientId;
            residentInfo.AlternatePatientId = residentdemograhic.AlternatePatientId;
            residentInfo.PatientLastName = residentdemograhic.PatientLastName;
            residentInfo.PatientFirstName = residentdemograhic.PatientFirstName;
            residentInfo.PatientMiddleInitial = residentdemograhic.PatientMiddleInitial;
            residentInfo.NameTypeCode = residentdemograhic.NameTypeCode;
            residentInfo.MotherMaidenName = residentdemograhic.MotherMaidenName;
            residentInfo.DOB = residentdemograhic.DOB;
            residentInfo.AdministrativeSex = residentdemograhic.AdministrativeSex;
            residentInfo.PatientAlias = residentdemograhic.PatientAlias;
            residentInfo.Race = residentdemograhic.Race;
            residentInfo.PatientAddress1 = residentdemograhic.PatientAddress1;
            residentInfo.PatientAddress2 = residentdemograhic.PatientAddress2;
            residentInfo.PatientCity = residentdemograhic.PatientCity;
            residentInfo.PatientState = residentdemograhic.PatientState;
            residentInfo.PatientZipCode = residentdemograhic.PatientZipCode;
            residentInfo.CountyCode = residentdemograhic.CountyCode;
            residentInfo.PhoneHome = residentdemograhic.PhoneHome;
            residentInfo.PhoneBusiness = residentdemograhic.PhoneBusiness;
            residentInfo.PrimaryLanguage = residentdemograhic.PrimaryLanguage;
            residentInfo.MaritalStatus = residentdemograhic.MaritalStatus;
            residentInfo.Religion = residentdemograhic.Religion;
            residentInfo.PatientMRNumber = residentdemograhic.PatientMRNumber;
            residentInfo.SSN = residentdemograhic.SSN;
            residentInfo.DriverLicense = residentdemograhic.DriverLicense;
            residentInfo.MotherIdentifier = residentdemograhic.MotherIdentifier;
            residentInfo.EthnicGroup = residentdemograhic.EthnicGroup;
            residentInfo.BirthPlace = residentdemograhic.BirthPlace;
            residentInfo.MultipleBirthIndicator = residentdemograhic.MultipleBirthIndicator;
            residentInfo.BirthOrder = residentdemograhic.BirthOrder;
            residentInfo.Citizenship = residentdemograhic.Citizenship;
            residentInfo.MilitaryStatus = residentdemograhic.MilitaryStatus;
            residentInfo.Nationality = residentdemograhic.Nationality;
            residentInfo.DeathDateTime = residentdemograhic.DeathDateTime;
            residentInfo.DeathIndicator = residentdemograhic.DeathIndicator;
            residentInfo.IdentityIndicator = residentdemograhic.IdentityIndicator;
            residentInfo.IdentityReliability = residentdemograhic.IdentityReliability;
            residentInfo.LastUpdate = residentdemograhic.LastUpdate;
            residentInfo.LastFacilityUpdate = residentdemograhic.LastFacilityUpdate;
            residentInfo.SpeciesCode = residentdemograhic.SpeciesCode;
            residentInfo.BreedCode = residentdemograhic.BreedCode;
            residentInfo.Strain = residentdemograhic.Strain;
            residentInfo.ProductionClassCode = residentdemograhic.ProductionClassCode;
            residentInfo.TribalCitizenship = residentdemograhic.TribalCitizenship;
            residentInfo.ImageLocation = residentdemograhic.ImageLocation;
            residentInfo.Patient_Status = residentdemograhic.Patient_Status;
            residentInfo.Patient_CreatedBy = residentdemograhic.Patient_CreatedBy;
            residentInfo.Patient_CreatedDate = residentdemograhic.Patient_CreatedDate;
            residentInfo.PDOutBoundFileStatus = residentdemograhic.PDOutBoundFileStatus;
            if (approvaldemographic != null)
                residentInfo.PDOutBoundApproval = 0;
            else
                residentInfo.PDOutBoundApproval = 1;
            residentInfo.PDOutBoundApprovalBy = residentdemograhic.PDOutBoundApprovalBy;
            residentInfo.PDOutBoundApprovalOn = residentdemograhic.PDOutBoundApprovalOn;
            residentInfo.Alert = residentdemograhic.Alert;
            residentInfo.Diet = residentdemograhic.Diet;
            return residentInfo;
        }

        public List<LiteralOrdersEntity> GetLiteralOrdersData(int patientID)
        {
            var ordersList = (from pd in this.dbContext.PhysicianDetails
                              join co in this.dbContext.CommonOrderInfoes on pd.Physician_Id equals co.OrderingPhysicianID
                              join dt in this.dbContext.OrderTypes on co.OrderTypeID equals dt.OrderTypeID
                              join oc in this.dbContext.OrderControlMasters on co.OrderControl equals oc.OrderControlValue
                              join eo in this.dbContext.EncodedOrderDetails on co.POrder_Id equals eo.POrder_Id
                              where co.Patient_Id == patientID
                              select new LiteralOrdersEntity
                              {
                                  Patient_Id = co.Patient_Id,
                                  POrder_Id = co.POrder_Id,
                                  OrderControl = oc.OrderControlDesc,
                                  FacilityId = co.FacilityId,
                                  PatientId = co.PatientId,
                                  Room = co.Room,
                                  TransactionDate = co.TransactionDate,
                                  EnteredBy = co.EnteredBy,
                                  VerifiedBy = co.VerifiedBy,
                                  VEffectivedate = co.VEffectivedate,
                                  OPhysicianLname = pd.PhysicianLName,
                                  OPhysicianFname = pd.PhysicianFName,
                                  OrderEffectiveDate = co.OrderEffectiveDate,
                                  OrderingFacilityName = co.OrderingFacilityName,
                                  OrderingFacilityAddress1 = co.OrderingFacilityAddress1,
                                  OrderingFacilityPhone = co.OrderingFacilityPhone,
                                  POrder_Status = co.POrder_Status,
                                  POrder_CreatedBy = co.POrder_CreatedBy,
                                  POrder_CreatedDate = co.POrder_CreatedDate,
                                  OrderTypeID = co.OrderTypeID,
                                  FillerType = dt.OrderType_Desc,
                                  GiveCodeText = eo.GiveCodeText//this.dbContext.TreatmentInfoes.Where(tinfo => tinfo.POrder_Id == co.POrder_Id).Select(ti => ti.RequestedGiveCode).FirstOrDefault()

                              }).ToList();
            ////  this.dbContext.CommonOrderInfoes.Where(dm => dm.Patient_Id == patientID).ToList();
            //List<LiteralOrdersEntity> ordersList = new List<LiteralOrdersEntity>();
            //for (int i = 0; i < commanOrders.Count(); i++)
            //{
            //    LiteralOrdersEntity ordersInfo = new LiteralOrdersEntity();
            //    if (commanOrders[i].FacilityId != null && commanOrders[i].PatientId != null && commanOrders[i].Room != null)
            //    {
            //        ordersInfo.FillerType = "Drug Order";
            //    }
            //    else
            //    {
            //        ordersInfo.FillerType = "Literal Order";
            //    }
            //    ordersInfo.FillerType = commanOrders[i].Orde
            //    ordersInfo.Patient_Id = commanOrders[i].Patient_Id;
            //    ordersInfo.POrder_Id = commanOrders[i].POrder_Id;
            //    ordersInfo.OrderControl = commanOrders[i].OrderControl;
            //    ordersInfo.FacilityId = commanOrders[i].FacilityId;
            //    ordersInfo.PatientId = commanOrders[i].PatientId;
            //    ordersInfo.Room = commanOrders[i].Room;
            //    ordersInfo.TransactionDate = commanOrders[i].TransactionDate;
            //    ordersInfo.EnteredBy = commanOrders[i].EnteredBy;
            //    ordersInfo.VerifiedBy = commanOrders[i].VerifiedBy;
            //    ordersInfo.VEffectivedate = commanOrders[i].VEffectivedate;
            //    ordersInfo.OPhysicianLname = commanOrders[i].OPhysicianLname;
            //    ordersInfo.OPhysicianFname = commanOrders[i].OPhysicianFname;
            //    ordersInfo.OrderEffectiveDate = commanOrders[i].OrderEffectiveDate;
            //    ordersInfo.OrderingFacilityName = commanOrders[i].OrderingFacilityName;
            //    ordersInfo.OrderingFacilityAddress1 = commanOrders[i].OrderingFacilityAddress1;
            //    ordersInfo.OrderingFacilityPhone = commanOrders[i].OrderingFacilityPhone;
            //    ordersInfo.POrder_Status = commanOrders[i].POrder_Status;
            //    ordersInfo.POrder_CreatedBy = commanOrders[i].POrder_CreatedBy;
            //    ordersInfo.POrder_CreatedDate = commanOrders[i].POrder_CreatedDate;
            //    ordersList.Add(ordersInfo);
            //}
            return ordersList;
        }
        public List<ResidentLiteralOrderDataEntity> GetResidentOrderData(int patientId)
        {
            var ordersList = this.dbContext.PrcGetResientOrderData(patientId).OrderBy(item => item.FillerType).ThenBy(item => item.OrderType).ThenBy(item => item.DrugName).ToList();

            return this.autoMapper.Map<List<PrcGetResientOrderData_Result>, List<ResidentLiteralOrderDataEntity>>(ordersList);
        }
        public List<TreatmentInfoEntity> GetMedications(int patientID)
        {
            var orderIds = this.dbContext.CommonOrderInfoes.Where(dm => dm.Patient_Id == patientID).Select(p => p.POrder_Id).ToList();

            var medications = (from treat in this.dbContext.TreatmentInfoes
                               join user in this.dbContext.Users on treat.PTreatment_CreatedBy equals user.User_Id
                               where orderIds.Contains(treat.POrder_Id)
                               select new TreatmentInfoEntity
                               {
                                   PTreatment_Id = treat.PTreatment_Id,
                                   POrder_Id = treat.POrder_Id,
                                   ReqGiveCodeIdentifier = treat.ReqGiveCodeIdentifier,
                                   RequestedGiveCode = treat.RequestedGiveCode,
                                   RequestedGiveAmtMin = treat.RequestedGiveAmtMin,
                                   RequestedGiveAmtMax = treat.RequestedGiveAmtMax,
                                   RequestedGiveUnits = treat.RequestedGiveUnits,
                                   RequestedDosageForm = treat.RequestedDosageForm,
                                   ProvidersTreatmentInstructions = treat.ProvidersTreatmentInstructions,
                                   ProvidersAdministrationInstructions = treat.ProvidersAdministrationInstructions,
                                   DeliverToLocation = treat.DeliverToLocation,
                                   AllowSubstitutions = treat.AllowSubstitutions,
                                   RequestedDispenseCode = treat.RequestedDispenseCode,
                                   RequestedDispenseAmount = treat.RequestedDispenseAmount,
                                   RequestedDispenseUnits = treat.RequestedDispenseUnits,
                                   NumberOfRefills = treat.NumberOfRefills,
                                   OrderingProviderDEANumber = treat.OrderingProviderDEANumber,
                                   TreatmentSupplierVerifierID = treat.TreatmentSupplierVerifierID,
                                   NeedsHumanReview = treat.NeedsHumanReview,
                                   RequestedGivePer = treat.RequestedGivePer,
                                   RequestedGiveStrength = treat.RequestedGiveStrength,
                                   RequestedGiveStrengthUnits = treat.RequestedGiveStrengthUnits,
                                   IndicationIdentifier = treat.IndicationIdentifier,
                                   IndicationText = treat.IndicationText,
                                   IndicationCodingSystem = treat.IndicationCodingSystem,
                                   AIndicationIdentifier = treat.AIndicationIdentifier,
                                   AIndicationText = treat.AIndicationText,
                                   AIndicationCodingSystem = treat.AIndicationCodingSystem,
                                   RequestedGiveRateAmount = treat.RequestedGiveRateAmount,
                                   RequestedGiveRateUnits = treat.RequestedGiveRateUnits,
                                   TotalDailyDose = treat.TotalDailyDose,
                                   SupplementaryCode = treat.SupplementaryCode,
                                   RequestedDrugStrengthVol = treat.RequestedDrugStrengthVol,
                                   RequestedDrugStrengthVolUnits = treat.RequestedDrugStrengthVolUnits,
                                   PharmacyOrderType = treat.PharmacyOrderType,
                                   DispensingInterval = treat.DispensingInterval,
                                   PTreatment_Status = treat.PTreatment_Status,
                                   PTreatment_CreatedBy = treat.PTreatment_CreatedBy,
                                   PTreatment_CreatedDate = treat.PTreatment_CreatedDate,
                                   PTIOutBoundFileStatus = treat.PTIOutBoundFileStatus,
                                   PTIOutBoundApproval = treat.PTIOutBoundApproval,
                                   PTIOutBoundApprovalBy = treat.PTIOutBoundApprovalBy,
                                   PTIOutBoundApprovalOn = treat.PTIOutBoundApprovalOn,
                                   UserName = user.UserName

                               }).ToList();
            return medications;
            //var medications = this.dbContext.TreatmentInfoes.Where(p => orderIds.Contains(p.POrder_Id)).OrderByDescending(p => p.PTreatment_CreatedDate).ToList();
            //return this.autoMapper.Map<List<TreatmentInfo>, List<TreatmentInfoEntity>>(medications);
        }
        public TreatmentInfoEntity GetResidentMedications(int TreatmentId)
        {

            var treatments = this.dbContext.TreatmentInfoes.Where(dm => dm.PTreatment_Id == TreatmentId).FirstOrDefault();
            return this.autoMapper.Map<TreatmentInfo, TreatmentInfoEntity>(treatments);
        }

        public int UploadResidentImage(string path, int PatientID)
        {
            var demographic = this.dbContext.Demographics.Find(PatientID);
            demographic.ImageLocation = path;
            this.dbContext.SaveChanges();
            return 1;
        }

        public List<DemographicEntity> GetOutBoundDemographics()
        {
            var approvedOBDemographics = this.dbContext.Demographics.Where(a => a.PDOutBoundFileStatus == 1 && a.PDOutBoundApproval == 1).ToList();
            return this.autoMapper.Map<List<Demographic>, List<DemographicEntity>>(approvedOBDemographics);
        }
        public List<VisitInfoEntity> GetOutBoundTransfers()
        {
            var approvedOBTransfer = this.dbContext.VisitInfoes.Where(a => a.PVOutBoundFileStatus == 1 && a.PVOutBoundApproval == 1).ToList();
            return this.autoMapper.Map<List<VisitInfo>, List<VisitInfoEntity>>(approvedOBTransfer);
        }
        public List<VisitInfoEntity> GetOutBoundDischarges()
        {
            var approvedOBDischarge = this.dbContext.VisitInfoes.Where(a => a.PVOutBoundFileStatus == 1 && a.PVOutBoundApproval == 1).ToList();
            return this.autoMapper.Map<List<VisitInfo>, List<VisitInfoEntity>>(approvedOBDischarge);
        }
        public int InsertUpdateDemographics(DemographicInfoEntity entity)
        {
            throw new NotImplementedException();
        }
        public string GetResidentName(int patientId)
        {
            var demographic = this.dbContext.Demographics.Where(dm => dm.Patient_Id == patientId).FirstOrDefault();
            if (demographic.PatientMiddleInitial != null)
                return demographic.PatientLastName + ", " + demographic.PatientFirstName + " " + (demographic.PatientMiddleInitial == null ? "" : demographic.PatientMiddleInitial);
            else
                return demographic.PatientLastName + ", " + demographic.PatientFirstName + " " + (demographic.PatientMiddleInitial == null ? "" : demographic.PatientMiddleInitial);
        }

        public List<DemographicEntity> GetApprovalPendingDemographics(int patientId)
        {
            var pendingDemographics = this.dbContext.Demographics.Where(a => a.Patient_Id == patientId && a.PDOutBoundFileStatus == 1 && (a.PDOutBoundApproval == 0 || a.PDOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<Demographic>, List<DemographicEntity>>(pendingDemographics);
        }
        public List<DemographicEntity> GetApprovalPendingDemographics()
        {
            var pendingDemographics = this.dbContext.Demographics.Where(a => a.PDOutBoundFileStatus == 1 && (a.PDOutBoundApproval == 0 || a.PDOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<Demographic>, List<DemographicEntity>>(pendingDemographics);
        }
        public List<VisitInfoEntity> GetApprovalPendingTransfer(int patientId)
        {
            var pendingTransfer = this.dbContext.VisitInfoes.Where(a => a.Patient_Id == patientId && a.PriorNursingStationId != null && a.PVOutBoundFileStatus == 1 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<VisitInfo>, List<VisitInfoEntity>>(pendingTransfer);
        }
        public List<VisitInfoEntity> GetApprovalPendingTransfer()
        {
            var pendingTransfer = this.dbContext.VisitInfoes.Where(a => a.PriorNursingStationId != null && a.PVOutBoundFileStatus == 1 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<VisitInfo>, List<VisitInfoEntity>>(pendingTransfer);
        }
        public List<VisitInfoEntity> GetApprovalPendingDischarge(int patientId)
        {
            var pendingDischarge = this.dbContext.VisitInfoes.Where(a => a.Patient_Id == patientId && a.DischargeDate != null && a.ReAdmissionIndicator != "R" && a.PVOutBoundFileStatus == 1 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<VisitInfo>, List<VisitInfoEntity>>(pendingDischarge);
        }
        public List<VisitInfoEntity> GetApprovalPendingDischarge()
        {
            var pendingDischarge = this.dbContext.VisitInfoes.Where(a => a.DischargeDate != null && a.ReAdmissionIndicator != "R" && a.PVOutBoundFileStatus == 1 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<VisitInfo>, List<VisitInfoEntity>>(pendingDischarge);
        }

        public int ApproveDemographic(ApprovalPendingCustomEntity entity)
        {
            Demographic demographic = this.dbContext.Demographics.Find(entity.Record_Id);
            if (demographic.Patient_Id == entity.Patient_Id)
            {
                demographic.PDOutBoundApprovalBy = entity.ApprovedBy;
                demographic.PDOutBoundApproval = entity.ApprovalStatus;
                demographic.PDOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();

                return 1;
            }
            return 0;
        }

        public VisitInfoEntity GetResidentAdmitvisitInfoData(int visitId)
        {
            var residentvisitinfodata = this.dbContext.VisitInfoes.Where(dm => dm.PVisit_Id == visitId).FirstOrDefault();
            var approvalvisitinfo = this.dbContext.ApprovalVisitInfoes.Where(av => av.PVisit_Id == visitId && (av.PVOutBoundApproval == null || av.PVOutBoundApproval == 0)).FirstOrDefault();

            VisitInfoEntity residentVisitInfo = new VisitInfoEntity();
            residentVisitInfo.PVisit_Id = residentvisitinfodata.PVisit_Id;
            residentVisitInfo.Patient_Id = residentvisitinfodata.Patient_Id;
            residentVisitInfo.PatientClass = residentvisitinfodata.PatientClass;
            residentVisitInfo.NursingStationId = residentvisitinfodata.NursingStationId;
            residentVisitInfo.Room = residentvisitinfodata.Room;
            residentVisitInfo.Bed = residentvisitinfodata.Bed;
            residentVisitInfo.FacilityId = residentvisitinfodata.FacilityId;
            residentVisitInfo.Floor = residentvisitinfodata.Floor;
            residentVisitInfo.Wing = residentvisitinfodata.Wing;
            residentVisitInfo.AdmissionType = residentvisitinfodata.AdmissionType;
            residentVisitInfo.PreAdmitNumber = residentvisitinfodata.PreAdmitNumber;
            residentVisitInfo.PriorNursingStationId = residentvisitinfodata.PriorNursingStationId;
            residentVisitInfo.PriorRoom = residentvisitinfodata.PriorRoom;
            residentVisitInfo.PriorBed = residentvisitinfodata.PriorBed;
            residentVisitInfo.PriorFacilityId = residentvisitinfodata.PriorFacilityId;
            residentVisitInfo.PriorFloor = residentvisitinfodata.PriorFloor;
            residentVisitInfo.PrimaryPhysicianNPI = residentvisitinfodata.PrimaryPhysicianNPI;
            residentVisitInfo.PrimaryPhysicianLName = residentvisitinfodata.PrimaryPhysicianLName;
            residentVisitInfo.PrimaryPhysicianFName = residentvisitinfodata.PrimaryPhysicianFName;
            residentVisitInfo.ReferringDoctor = residentvisitinfodata.ReferringDoctor;
            residentVisitInfo.ConsultingDoctor = residentvisitinfodata.ConsultingDoctor;
            residentVisitInfo.HospitalService = residentvisitinfodata.HospitalService;
            residentVisitInfo.TemporaryLocation = residentvisitinfodata.TemporaryLocation;
            residentVisitInfo.PreAdmitTestIndicator = residentvisitinfodata.PreAdmitTestIndicator;
            residentVisitInfo.ReAdmissionIndicator = residentvisitinfodata.ReAdmissionIndicator;
            residentVisitInfo.AdmitSource = residentvisitinfodata.AdmitSource;
            residentVisitInfo.AmbulatoryStatus = residentvisitinfodata.AmbulatoryStatus;
            residentVisitInfo.VIPIndicator = residentvisitinfodata.VIPIndicator;
            residentVisitInfo.AdmittingDoctor = residentvisitinfodata.AdmittingDoctor;
            residentVisitInfo.PatientType = residentvisitinfodata.PatientType;
            residentVisitInfo.VisitNumber = residentvisitinfodata.VisitNumber;
            residentVisitInfo.FinancialClass = residentvisitinfodata.FinancialClass;
            residentVisitInfo.ChargePriceIndicator = residentvisitinfodata.ChargePriceIndicator;
            residentVisitInfo.CourtesyCode = residentvisitinfodata.CourtesyCode;
            residentVisitInfo.CreditRating = residentvisitinfodata.CreditRating;
            residentVisitInfo.ContractCode = residentvisitinfodata.ContractCode;
            residentVisitInfo.ContractEffDate = residentvisitinfodata.ContractEffDate;
            residentVisitInfo.ContractAmount = residentvisitinfodata.ContractAmount;
            residentVisitInfo.ContractPeriod = residentvisitinfodata.ContractPeriod;
            residentVisitInfo.InterestCode = residentvisitinfodata.InterestCode;
            residentVisitInfo.BadDebtCode = residentvisitinfodata.BadDebtCode;
            residentVisitInfo.BadDebtDate = residentvisitinfodata.BadDebtDate;
            residentVisitInfo.BadDebtAgencyCode = residentvisitinfodata.BadDebtAgencyCode;
            residentVisitInfo.BadDebtTransferAmt = residentvisitinfodata.BadDebtTransferAmt;
            residentVisitInfo.BadDebtRecoveryAmt = residentvisitinfodata.BadDebtRecoveryAmt;
            residentVisitInfo.DeleteAccIndicator = residentvisitinfodata.DeleteAccIndicator;
            residentVisitInfo.DeleteAccDate = residentvisitinfodata.DeleteAccDate;
            residentVisitInfo.DischargeDisposition = residentvisitinfodata.DischargeDisposition;
            residentVisitInfo.DischargedLocation = residentvisitinfodata.DischargedLocation;
            residentVisitInfo.DietType = residentvisitinfodata.DietType;
            residentVisitInfo.ServicingFacility = residentvisitinfodata.ServicingFacility;
            residentVisitInfo.BedStatus = residentvisitinfodata.BedStatus;
            residentVisitInfo.AccStatus = residentvisitinfodata.AccStatus;
            residentVisitInfo.PendingLocation = residentvisitinfodata.PendingLocation;
            residentVisitInfo.PriorTemporaryLocation = residentvisitinfodata.PriorTemporaryLocation;
            residentVisitInfo.AdmitDate = residentvisitinfodata.AdmitDate;
            residentVisitInfo.DischargeDate = residentvisitinfodata.DischargeDate;
            residentVisitInfo.CurrentPatientBalance = residentvisitinfodata.CurrentPatientBalance;
            residentVisitInfo.TotalCharges = residentvisitinfodata.TotalCharges;
            residentVisitInfo.TotalAdjustments = residentvisitinfodata.TotalAdjustments;
            residentVisitInfo.TotalPayments = residentvisitinfodata.TotalPayments;
            residentVisitInfo.AlternateVisitId = residentvisitinfodata.AlternateVisitId;
            residentVisitInfo.VisitIndicator = residentvisitinfodata.VisitIndicator;
            residentVisitInfo.OtherHealthProvider = residentvisitinfodata.OtherHealthProvider;
            residentVisitInfo.PVisit_Status = residentvisitinfodata.PVisit_Status;
            residentVisitInfo.PVisit_CreatedBy = residentvisitinfodata.PVisit_CreatedBy;
            residentVisitInfo.PVisit_CreatedDate = residentvisitinfodata.PVisit_CreatedDate;
            residentVisitInfo.PVOutBoundFileStatus = residentvisitinfodata.PVOutBoundFileStatus;
            residentVisitInfo.DefaultPhysicianFlag = 0;
            if (approvalvisitinfo != null)
                residentVisitInfo.PVOutBoundApproval = 0;
            else
                residentVisitInfo.PVOutBoundApproval = 1;
            residentVisitInfo.PVOutBoundApprovalBy = residentvisitinfodata.PVOutBoundApprovalBy;
            residentVisitInfo.PVOutBoundApprovalOn = residentvisitinfodata.PVOutBoundApprovalOn;
            residentVisitInfo.Wing = residentvisitinfodata.Wing;
            residentVisitInfo.Physician_Id = residentvisitinfodata.Physician_Id;
            if (residentVisitInfo.PrimaryPhysicianNPI == null)
            {
                var defaultPhysicianId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == residentVisitInfo.NursingStationId).Select(n => n.DefaultPhysician_Id).FirstOrDefault();
                if (defaultPhysicianId != null)
                {
                    residentVisitInfo.PrimaryPhysicianNPI = this.dbContext.PhysicianDetails.Where(e => e.Physician_Id == defaultPhysicianId).Select(e => e.PhysicianNPI).FirstOrDefault();
                    residentVisitInfo.DefaultPhysicianFlag = 1;
                }
            }
            return residentVisitInfo;
        }


        public CommonOrderInfoEntity GetResidentLiteralordersData(int OrderId)
        {
            var residentliteralorders = this.dbContext.CommonOrderInfoes.Where(dm => dm.POrder_Id == OrderId).FirstOrDefault();
            return this.autoMapper.Map<CommonOrderInfo, CommonOrderInfoEntity>(residentliteralorders);
        }

        public int InsertResidentMedications(TreatmentInfoEntity medications)
        {
            var residentmedications = this.autoMapper.Map<TreatmentInfoEntity, TreatmentInfo>(medications);

            TreatmentInfo record = this.dbContext.TreatmentInfoes.Find(residentmedications.PTreatment_Id);
            if (record == null)
            {
                return 0;
            }
            else
            {
                record.POrder_Id = residentmedications.POrder_Id;
                record.ReqGiveCodeIdentifier = residentmedications.ReqGiveCodeIdentifier;
                record.RequestedGiveCode = residentmedications.RequestedGiveCode;
                record.RequestedGiveAmtMin = residentmedications.RequestedGiveAmtMin;
                record.RequestedGiveAmtMax = residentmedications.RequestedGiveAmtMax;
                record.RequestedGiveUnits = residentmedications.RequestedGiveUnits;
                record.RequestedDosageForm = residentmedications.RequestedDosageForm;
                record.ProvidersTreatmentInstructions = residentmedications.ProvidersTreatmentInstructions;
                record.ProvidersAdministrationInstructions = residentmedications.ProvidersAdministrationInstructions;
                record.DeliverToLocation = residentmedications.DeliverToLocation;
                record.AllowSubstitutions = residentmedications.AllowSubstitutions;
                record.RequestedDispenseCode = residentmedications.RequestedDispenseCode;
                record.RequestedDispenseAmount = residentmedications.RequestedDispenseAmount;
                record.RequestedDispenseUnits = residentmedications.RequestedDispenseUnits;
                record.NumberOfRefills = residentmedications.NumberOfRefills;
                record.OrderingProviderDEANumber = residentmedications.OrderingProviderDEANumber;
                record.TreatmentSupplierVerifierID = residentmedications.TreatmentSupplierVerifierID;
                record.NeedsHumanReview = residentmedications.NeedsHumanReview;
                record.RequestedGivePer = residentmedications.RequestedGivePer;
                record.RequestedGiveStrength = residentmedications.RequestedGiveStrength;
                record.RequestedGiveStrengthUnits = residentmedications.RequestedGiveStrengthUnits;
                record.IndicationIdentifier = residentmedications.IndicationIdentifier;
                record.IndicationText = residentmedications.IndicationText;
                record.IndicationCodingSystem = residentmedications.IndicationCodingSystem;
                record.AIndicationIdentifier = residentmedications.AIndicationIdentifier;
                record.AIndicationText = residentmedications.AIndicationText;
                record.AIndicationCodingSystem = residentmedications.AIndicationCodingSystem;
                record.RequestedGiveRateAmount = residentmedications.RequestedGiveRateAmount;
                record.RequestedGiveRateUnits = residentmedications.RequestedGiveRateUnits;
                record.TotalDailyDose = residentmedications.TotalDailyDose;
                record.SupplementaryCode = residentmedications.SupplementaryCode;
                record.RequestedDrugStrengthVol = residentmedications.RequestedDrugStrengthVol;
                record.RequestedDrugStrengthVolUnits = residentmedications.RequestedDrugStrengthVolUnits;
                record.PharmacyOrderType = residentmedications.PharmacyOrderType;
                record.DispensingInterval = residentmedications.DispensingInterval;
                record.PTreatment_Status = residentmedications.PTreatment_Status;
                record.PTreatment_CreatedBy = residentmedications.PTreatment_CreatedBy;
                record.PTreatment_CreatedDate = residentmedications.PTreatment_CreatedDate;
                record.PTIOutBoundFileStatus = residentmedications.PTIOutBoundFileStatus;
                record.PTIOutBoundApproval = residentmedications.PTIOutBoundApproval;
                record.PTIOutBoundApprovalBy = residentmedications.PTIOutBoundApprovalBy;
                record.PTIOutBoundApprovalOn = residentmedications.PTIOutBoundApprovalOn;

                this.dbContext.SaveChanges();
                return 1;
            }
        }
        public int UpdateResidentInfo(VisitUpdateEntity residentinfo)
        {
            var record = this.dbContext.VisitInfoes.Where(p => p.Patient_Id == residentinfo.Patient_Id).FirstOrDefault();
            var resinfo = this.autoMapper.Map<VisitUpdateEntity, VisitInfo>(residentinfo);
            if (record != null)
            {
                if (residentinfo.Floor != null && record.Floor != residentinfo.Floor)
                {

                    record.PriorFloor = record.Floor;
                    record.Floor = resinfo.Floor;

                }
                if (residentinfo.Room != null && record.Room != residentinfo.Room)
                {
                    record.PriorRoom = record.Room;
                    record.Room = resinfo.Room;
                }
                if (residentinfo.Bed != null && record.Bed != residentinfo.Bed)
                {
                    record.PriorBed = record.Bed;
                    record.Bed = resinfo.Bed;
                }
                if (residentinfo.Wing != null && record.Wing != residentinfo.Wing)
                {

                    record.Wing = resinfo.Wing;
                }


                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public int InsertResidentAdmitVistiInfoData(VisitInfoEntity admitvisitinfo)
        {
            var admitinfo = this.autoMapper.Map<VisitInfoEntity, VisitInfo>(admitvisitinfo);

            VisitInfo record = this.dbContext.VisitInfoes.Find(admitinfo.PVisit_Id);
            if (record == null)
            {
                return 0;
            }
            else
            {
                record.Patient_Id = admitinfo.Patient_Id;
                record.PatientClass = admitinfo.PatientClass;
                record.NursingStationId = admitinfo.NursingStationId;
                record.Room = admitinfo.Room;
                record.Bed = admitinfo.Bed;
                record.FacilityId = admitinfo.FacilityId;
                record.Floor = admitinfo.Floor;
                record.AdmissionType = admitinfo.AdmissionType;
                record.PreAdmitNumber = admitinfo.PreAdmitNumber;
                record.PriorNursingStationId = admitinfo.PriorNursingStationId;
                record.PriorRoom = admitinfo.PriorRoom;
                record.PriorBed = admitinfo.PriorBed;
                record.PriorFacilityId = admitinfo.PriorFacilityId;
                record.PriorFloor = admitinfo.PriorFloor;
                record.PrimaryPhysicianNPI = admitinfo.PrimaryPhysicianNPI;
                record.PrimaryPhysicianLName = admitinfo.PrimaryPhysicianLName;
                record.PrimaryPhysicianFName = admitinfo.PrimaryPhysicianFName;
                record.ReferringDoctor = admitinfo.ReferringDoctor;
                record.ConsultingDoctor = admitinfo.ConsultingDoctor;
                record.HospitalService = admitinfo.HospitalService;
                record.TemporaryLocation = admitinfo.TemporaryLocation;
                record.PreAdmitTestIndicator = admitinfo.PreAdmitTestIndicator;
                record.ReAdmissionIndicator = admitinfo.ReAdmissionIndicator;
                record.AdmitSource = admitinfo.AdmitSource;
                record.AmbulatoryStatus = admitinfo.AmbulatoryStatus;
                record.VIPIndicator = admitinfo.VIPIndicator;
                record.AdmittingDoctor = admitinfo.AdmittingDoctor;
                record.PatientType = admitinfo.PatientType;
                record.VisitNumber = admitinfo.VisitNumber;
                record.FinancialClass = admitinfo.FinancialClass;
                record.ChargePriceIndicator = admitinfo.ChargePriceIndicator;
                record.CourtesyCode = admitinfo.CourtesyCode;
                record.CreditRating = admitinfo.CreditRating;
                record.ContractCode = admitinfo.ContractCode;
                record.ContractEffDate = admitinfo.ContractEffDate;
                record.ContractAmount = admitinfo.ContractAmount;
                record.ContractPeriod = admitinfo.ContractPeriod;
                record.InterestCode = admitinfo.InterestCode;
                record.BadDebtCode = admitinfo.BadDebtCode;
                record.BadDebtDate = admitinfo.BadDebtDate;
                record.BadDebtAgencyCode = admitinfo.BadDebtAgencyCode;
                record.BadDebtTransferAmt = admitinfo.BadDebtTransferAmt;
                record.BadDebtRecoveryAmt = admitinfo.BadDebtRecoveryAmt;
                record.DeleteAccIndicator = admitinfo.DeleteAccIndicator;
                record.DeleteAccDate = admitinfo.DeleteAccDate;
                record.DischargeDisposition = admitinfo.DischargeDisposition;
                record.DischargedLocation = admitinfo.DischargedLocation;
                record.DietType = admitinfo.DietType;
                record.ServicingFacility = admitinfo.ServicingFacility;
                record.BedStatus = admitinfo.BedStatus;
                record.AccStatus = admitinfo.AccStatus;
                record.PendingLocation = admitinfo.PendingLocation;
                record.PriorTemporaryLocation = admitinfo.PriorTemporaryLocation;
                record.AdmitDate = admitinfo.AdmitDate;
                record.DischargeDate = admitinfo.DischargeDate;
                record.CurrentPatientBalance = admitinfo.CurrentPatientBalance;
                record.TotalCharges = admitinfo.TotalCharges;
                record.TotalAdjustments = admitinfo.TotalAdjustments;
                record.TotalPayments = admitinfo.TotalPayments;
                record.AlternateVisitId = admitinfo.AlternateVisitId;
                record.VisitIndicator = admitinfo.VisitIndicator;
                record.OtherHealthProvider = admitinfo.OtherHealthProvider;
                record.PVisit_Status = admitinfo.PVisit_Status;
                record.PVisit_CreatedBy = admitinfo.PVisit_CreatedBy;
                record.PVisit_CreatedDate = admitinfo.PVisit_CreatedDate;
                record.PVOutBoundFileStatus = admitinfo.PVOutBoundFileStatus;
                record.PVOutBoundApproval = admitinfo.PVOutBoundApproval;
                record.PVOutBoundApprovalBy = admitinfo.PVOutBoundApprovalBy;
                record.PVOutBoundApprovalOn = admitinfo.PVOutBoundApprovalOn;
                record.Wing = admitinfo.Wing;
                //record.Physician_Id = admitinfo.Physician_Id;

                this.dbContext.SaveChanges();
                return 1;
            }
        }

        public int InsertResidentDemographicData(DemographicEntity demographics)
        {

            var demographicdata = this.autoMapper.Map<DemographicEntity, Demographic>(demographics);

            Demographic record = this.dbContext.Demographics.Find(demographicdata.Patient_Id);

            if (record.Patient_Id != 0)
            {
                var demographicinfo = this.autoMapper.Map<DemographicEntity, Demographic>(demographics);
                demographicinfo.Patient_Id = 0;
                demographicinfo.ExternalFacShortName = record.ExternalFacShortName;
                this.dbContext.Demographics.Add(demographicinfo);
                this.dbContext.SaveChanges();
                return 0;
            }
            else
            {
                record.Patient_Id = demographicdata.Patient_Id;
                record.ExternalPatientId = demographicdata.ExternalPatientId;
                record.ExternalFacShortName = demographicdata.ExternalFacShortName;
                record.ExternalFacPatientId = demographicdata.ExternalFacPatientId;
                record.AlternatePatientId = demographicdata.AlternatePatientId;
                record.PatientLastName = demographicdata.PatientLastName;
                record.PatientFirstName = demographicdata.PatientFirstName;
                record.PatientMiddleInitial = demographicdata.PatientMiddleInitial;
                record.NameTypeCode = demographicdata.NameTypeCode;
                record.MotherMaidenName = demographicdata.MotherMaidenName;
                record.DOB = demographicdata.DOB;
                record.AdministrativeSex = demographicdata.AdministrativeSex;
                record.PatientAlias = demographicdata.PatientAlias;
                record.Race = demographicdata.Race;
                record.PatientAddress1 = demographicdata.PatientAddress1;
                record.PatientAddress2 = demographicdata.PatientAddress2;
                record.PatientCity = demographicdata.PatientCity;
                record.PatientState = demographicdata.PatientState;
                record.PatientZipCode = demographicdata.PatientZipCode;
                record.CountyCode = demographicdata.CountyCode;
                record.PhoneHome = demographicdata.PhoneHome;
                record.PhoneBusiness = demographicdata.PhoneBusiness;
                record.PrimaryLanguage = demographicdata.PrimaryLanguage;
                record.MaritalStatus = demographicdata.MaritalStatus;
                record.Religion = demographicdata.Religion;
                record.PatientMRNumber = demographicdata.PatientMRNumber;
                record.SSN = demographicdata.SSN;
                record.DriverLicense = demographicdata.DriverLicense;
                record.MotherIdentifier = demographicdata.MotherIdentifier;
                record.EthnicGroup = demographicdata.EthnicGroup;
                record.BirthPlace = demographicdata.BirthPlace;
                record.MultipleBirthIndicator = demographicdata.MultipleBirthIndicator;
                record.BirthOrder = demographicdata.BirthOrder;
                record.Citizenship = demographicdata.Citizenship;
                record.MilitaryStatus = demographicdata.MilitaryStatus;
                record.Nationality = demographicdata.Nationality;
                record.DeathDateTime = demographicdata.DeathDateTime;
                record.DeathIndicator = demographicdata.DeathIndicator;
                record.IdentityIndicator = demographicdata.IdentityIndicator;
                record.IdentityReliability = demographicdata.IdentityReliability;
                record.LastUpdate = demographicdata.LastUpdate;
                record.LastFacilityUpdate = demographicdata.LastFacilityUpdate;
                record.SpeciesCode = demographicdata.SpeciesCode;
                record.BreedCode = demographicdata.BreedCode;
                record.Strain = demographicdata.Strain;
                record.ProductionClassCode = demographicdata.ProductionClassCode;
                record.TribalCitizenship = demographicdata.TribalCitizenship;
                record.ImageLocation = demographicdata.ImageLocation;
                record.Patient_Status = demographicdata.Patient_Status;
                record.Patient_CreatedBy = demographicdata.Patient_CreatedBy;
                record.Patient_CreatedDate = demographicdata.Patient_CreatedDate;
                record.PDOutBoundFileStatus = demographicdata.PDOutBoundFileStatus;
                record.PDOutBoundApproval = demographicdata.PDOutBoundApproval;
                record.PDOutBoundApprovalBy = demographicdata.PDOutBoundApprovalBy;
                record.PDOutBoundApprovalOn = demographicdata.PDOutBoundApprovalOn;
                record.Alert = demographicdata.Alert;
                record.Diet = demographicdata.Diet;

                this.dbContext.SaveChanges();
                return 1;
            }
        }
        /* 
        public int InsertResidentLiteralOrdersData(CommonOrderInfoEntity literalorders)
        {
            var orders = this.autoMapper.Map<CommonOrderInfoEntity, CommonOrderInfo>(literalorders);
            CommonOrderInfo record = this.dbContext.CommonOrderInfoes.Find(orders.POrder_Id);
            if (record == null)
            {
                return 0;

            }
            else
            {
                record.Patient_Id = orders.Patient_Id;
                record.OrderControl = orders.OrderControl;
                record.PlacerOrderNumber = orders.PlacerOrderNumber;
                record.FacilityId = orders.FacilityId;
                record.PatientId = orders.PatientId;
                record.Room = orders.Room;
                record.OrderTypeID = orders.OrderTypeID;
                record.PlacerGroupNumber = orders.PlacerGroupNumber;
                record.OrderStatus = orders.OrderStatus;
                record.ResponseFlag = orders.ResponseFlag;
                record.QuantityTiming = orders.QuantityTiming;
                record.Parent = orders.Parent;
                record.TransactionDate = orders.TransactionDate;
                record.EnteredBy = orders.EnteredBy;
                record.EPharmacistLName = orders.EPharmacistLName;
                record.EPharmacistFName = orders.EPharmacistFName;
                record.VerifiedBy = orders.VerifiedBy;
                record.VPharmacistLName = orders.VPharmacistLName;
                record.VPharmacistFName = orders.VPharmacistFName;
                record.VEffectivedate = orders.VEffectivedate;
                record.OrderingPhysicianNPI = orders.OrderingPhysicianNPI;
                record.OPhysicianLname = orders.OPhysicianLname;
                record.OPhysicianFname = orders.OPhysicianFname;
                record.EntererLocation = orders.EntererLocation;
                record.CallBackPhoneNumber = orders.CallBackPhoneNumber;
                record.OrderEffectiveDate = orders.OrderEffectiveDate;
                record.OrderControlCodeReason = orders.OrderControlCodeReason;
                record.EnteringOrganisation = orders.EnteringOrganisation;
                record.EnteringDevice = orders.EnteringDevice;
                record.AltCodingSystem = orders.AltCodingSystem;
                record.AdvBeneficiaryNoticeCode = orders.AdvBeneficiaryNoticeCode;
                record.OrderingFacilityName = orders.OrderingFacilityName;
                record.OrderingFacilityAddress1 = orders.OrderingFacilityAddress1;
                record.OrderingFacilityAddress2 = orders.OrderingFacilityAddress2;
                record.OrderingFacilityCity = orders.OrderingFacilityCity;
                record.OrderingFacilityState = orders.OrderingFacilityState;
                record.OrderingFacilityZip = orders.OrderingFacilityZip;
                record.OrderingFacilityPhone = orders.OrderingFacilityPhone;
                record.OrderingPhysicianAddress1 = orders.OrderingPhysicianAddress1;
                record.OrderingPhysicianAddress2 = orders.OrderingPhysicianAddress2;
                record.OrderingPhysicianCity = orders.OrderingPhysicianCity;
                record.OrderingPhysicianState = orders.OrderingPhysicianState;
                record.OrderingProviderZip = orders.OrderingProviderZip;
                record.OrderStatusModifier = orders.OrderStatusModifier;
                record.AdvBeneficiaryNoticeOverrideReason = orders.AdvBeneficiaryNoticeOverrideReason;
                record.ExpectedAvailabilityDate = orders.ExpectedAvailabilityDate;
                record.ConfidentialityCode = orders.ConfidentialityCode;
                record.OrderType = orders.OrderType;
                record.EntererAuthorizationMode = orders.EntererAuthorizationMode;
                record.POrder_Status = orders.POrder_Status;
                record.POrder_CreatedBy = orders.POrder_CreatedBy;
                record.POrder_CreatedDate = orders.POrder_CreatedDate;
                record.POOutBoundFileStatus = orders.POOutBoundFileStatus;
                record.POOutBoundApproval = orders.POOutBoundApproval;
                record.POOutBoundApprovalBy = orders.POOutBoundApprovalBy;
                record.POOutBoundApprovalOn = orders.POOutBoundApprovalOn;
                record.AlertText = orders.AlertText;
                record.MaxPerdays = orders.MaxPerdays;
                record.OrderStockFlag = orders.OrderStockFlag;
                record.PRNFlag = orders.PRNFlag;
                record.SelfAdministeredFlag = orders.SelfAdministeredFlag;
                record.TreatmentFlag = orders.TreatmentFlag;
                record.Maysubstitute = orders.Maysubstitute;
                record.OrderingPhysicianID = orders.OrderingPhysicianID;
                record.InsulinComments = orders.InsulinComments;

                this.dbContext.SaveChanges();
                return 1;
            }
        }
        */
        public ResidentCountCustomEntity GetResidentsCount(int userId)
        {
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

            ResidentCountCustomEntity entity = new ResidentCountCustomEntity();
            entity.Admit = this.dbContext.VisitInfoes.Where(p => p.DischargeDate == null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Count();
            entity.Readmit = this.dbContext.VisitInfoes.Where(p => p.ReAdmissionIndicator == "R" && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Count();
            //Findout Functionality
            entity.Inactive = this.dbContext.Demographics.Where(p => p.Patient_Status == 0).Count();
            //entity.Transfer = this.dbContext.VisitInfoes.Where(p => p.PriorNursingStationId != null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Count();
            entity.Transfer = this.dbContext.VisitInfoes.Where(p => (p.PriorFacilityId != null || p.PriorNursingStationId != null || p.PriorFloor != null || p.PriorRoom != null || p.PriorBed != null) && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Count();
            //entity.Discharge = this.dbContext.VisitInfoes.Where(p => p.DischargeDate != null && p.ReAdmissionIndicator != "R" && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Count();
            var patientList = this.dbContext.VisitInfoes.Where(p => p.PVisit_Status == 1).Select(p => p.Patient_Id).ToList();
            var pIds = this.dbContext.VisitInfoes.Where(p => p.DischargeDate != null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();

            entity.Discharge = pIds.Except(patientList).Count();
            return entity;
        }
        public int ApproveTransfer(ApprovalPendingCustomEntity entity)
        {
            VisitInfo transfer = this.dbContext.VisitInfoes.Find(entity.Record_Id);
            if (transfer.Patient_Id == entity.Patient_Id)
            {
                transfer.PVOutBoundApprovalBy = entity.ApprovedBy;
                transfer.PVOutBoundApproval = entity.ApprovalStatus;
                transfer.PVOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();

                return 1;
            }
            return 0;
        }

        public int ApproveDischarge(ApprovalPendingCustomEntity entity)
        {
            VisitInfo discharge = this.dbContext.VisitInfoes.Find(entity.Record_Id);
            if (discharge.Patient_Id == entity.Patient_Id)
            {
                discharge.PVOutBoundApprovalBy = entity.ApprovedBy;
                discharge.PVOutBoundApproval = entity.ApprovalStatus;
                discharge.PVOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();

                return 1;
            }
            return 0;
        }
        public List<DocFolderEntity> GetResidentDocFolder(int patientID)
        {
            var residentDocFolder = this.dbContext.DocFolders.Where(d => d.Folder_Status == 1).ToList();
            return this.autoMapper.Map<List<DocFolder>, List<DocFolderEntity>>(residentDocFolder);
        }

        public List<ResidentDropEntity> GetResidentDropData(int userId)
        {
            //List<int> patientIds = null;
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

            List<ResidentDropEntity> list = (from dm in this.dbContext.Demographics
                                             join vs in this.dbContext.VisitInfoes on dm.Patient_Id equals vs.Patient_Id
                                             where facilityIds.Contains((int)vs.FacilityId) && stationIds.Contains((int)vs.NursingStationId)
                                             select new ResidentDropEntity
                                             {
                                                 Patient_Id = dm.Patient_Id,
                                                 //PatientLastName = p.PatientLastName,
                                                 //PatientFirstName = p.PatientFirstName,
                                                 //PatientMiddleInitial = p.PatientMiddleInitial,
                                                 PatientName = dm.PatientLastName + ", " + dm.PatientFirstName + " " + (dm.PatientMiddleInitial == null ? "" : dm.PatientMiddleInitial)
                                             }).Distinct().OrderBy(item => item.PatientName).ToList();

            return list;
        }
        public OrdersViewEntity GetResidentOrdersView(int orderType, Int64 orderId, Int64 quantityId)
        {
            var order = this.dbContext.CommonOrderInfoes.Where(cm => cm.POrder_Id == orderId).FirstOrDefault();
            var encodedOrder = this.dbContext.EncodedOrderDetails.Where(en => en.POrder_Id == orderId).FirstOrDefault();
            var quantityDetails = this.dbContext.QuantityDetails.Where(qd => qd.POrder_Id == orderId && qd.PQuantity_Id == quantityId).FirstOrDefault();
            var treatmentRoute = this.dbContext.TreatmentRouteInfoes.Where(tr => tr.POrder_Id == orderId).FirstOrDefault();
            var treatmentInfo = this.dbContext.TreatmentInfoes.Where(ti => ti.POrder_Id == orderId).FirstOrDefault();
            var addlInstruction = this.dbContext.AddlInstructionDetails.Where(ad => ad.POrder_Id == orderId).FirstOrDefault();
            var ancillaryDetails = this.dbContext.AncillaryDetails.Where(ad => ad.POrder_Id == orderId).FirstOrDefault();
            OrdersViewEntity ordersView = new OrdersViewEntity();
            ordersView.CommonOrderInfo = this.autoMapper.Map<CommonOrderInfo, CommonOrderInfoEntity>(order);
            ordersView.EncodedOrderDetail = this.autoMapper.Map<EncodedOrderDetail, EncodedOrderDetailEntity>(encodedOrder);
            if (quantityDetails != null)
                ordersView.QuantityDetail = this.autoMapper.Map<QuantityDetail, QuantityDetailEntity>(quantityDetails);
            if (orderType == 1)
            {
                if (treatmentRoute != null)
                    ordersView.TreatmentRouteInfo = this.autoMapper.Map<TreatmentRouteInfo, TreatmentRouteInfoEntity>(treatmentRoute);
                if (treatmentInfo != null)
                    ordersView.TreatmentInfo = this.autoMapper.Map<TreatmentInfo, TreatmentInfoEntity>(treatmentInfo);
                if (addlInstruction != null)
                    ordersView.AddlInstructionDetail = this.autoMapper.Map<AddlInstructionDetail, AddlInstructionDetailEntity>(addlInstruction);
            }
            else if (orderType == 1)
            {
                if (ancillaryDetails != null)
                    ordersView.AncillaryDetail = this.autoMapper.Map<AncillaryDetail, AncillaryDetailEntity>(ancillaryDetails);
            }
            return ordersView;
        }
        public int InsertPatientType(ColourTypeEntity entity)
        {
            entity.ColourType_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var colourCode = this.autoMapper.Map<ColourTypeEntity, ColourType>(entity);
            string[] colorCodeString = entity.PatientTypeId.ToString().Split(',');
            string[] residentColorCodes = this.dbContext.ColourTypes.Where(c => c.Patient_Id == colourCode.Patient_Id).Select(c => c.PatientType_Id.ToString()).ToArray();
            foreach (var item in colorCodeString)
            {
                colourCode.PatientType_Id = Convert.ToInt32(item == "" ? null : item);
                ColourType record = this.dbContext.ColourTypes.Where(c => c.Patient_Id == colourCode.Patient_Id && c.PatientType_Id == colourCode.PatientType_Id).FirstOrDefault();
                if (record != null)
                {
                    record.ColourType_Status = colourCode.ColourType_Status;
                    record.ColourType_CreatedBy = colourCode.ColourType_CreatedBy;
                    record.ColourType_CreatedOn = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
                else
                {
                    if (colourCode.PatientType_Id == 0)
                    {
                        colourCode.PatientType_Id = null;
                    }
                    this.dbContext.ColourTypes.Add(colourCode);
                    this.dbContext.SaveChanges();
                }

            }
            var items = residentColorCodes.Except(colorCodeString);
            int patientId = Convert.ToInt32(colourCode.Patient_Id);
            foreach (var item in items)
            {
                int patientTypeId = Convert.ToInt32(item == "" ? null : item);
                ColourType record = this.dbContext.ColourTypes.Where(c => c.Patient_Id == patientId && c.PatientType_Id == patientTypeId).FirstOrDefault();
                if (record != null)
                {
                    this.dbContext.ColourTypes.Remove(record);
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public List<PatientColorTypeEntity> GetPatientType(int patientId)
        {
            return (from pt in this.dbContext.PatientTypes
                    join ct in this.dbContext.ColourTypes on pt.PatientType_Id equals ct.PatientType_Id
                    where ct.Patient_Id == patientId
                    select new PatientColorTypeEntity
                    {
                        Color_Code = pt.Color_Code,
                        Color_Description = pt.Color_Description
                    }).ToList();
        }
        public int GetNurseStationByPId(int patientId)
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
            int nurseStationId = visitInfoData.FirstOrDefault().NursingStationId != null ? (int)visitInfoData.FirstOrDefault().NursingStationId : 0;

            return nurseStationId;
        }
        public ResidentDataEntity GetFacilityNSResidentsDataByPId(int patientId)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                ResidentDataEntity data = new ResidentDataEntity();
                var visitInfoData = this.dbContext.VisitInfoes.Where(dm => dm.Patient_Id == patientId);
                if (visitInfoData.Count() > 1)
                {
                    var notDischargedRecord = visitInfoData.Where(v => v.DischargeDate == null);
                    if (notDischargedRecord.Count() == 0)
                        visitInfoData = visitInfoData.OrderByDescending(v => v.DischargeDate);
                    else
                        visitInfoData = notDischargedRecord;
                }
                int nurseStationId = visitInfoData.FirstOrDefault().NursingStationId != null ? (int)visitInfoData.FirstOrDefault().NursingStationId : 0;
                if (nurseStationId != 0)
                {

                    var facility = (from ns in this.dbContext.NursingStations
                                    join f in this.dbContext.Facilities on ns.Facility_Id equals f.Facility_Id
                                    where ns.NurseStation_Id == nurseStationId && ns.NurseStation_Status == 1 && f.Facility_Status == 1
                                    select f).FirstOrDefault();

                    data.NursingStationId = nurseStationId;
                    data.FacilityName = facility.Facility_Name;
                    data.NSDrop = (from ns in this.dbContext.NursingStations
                                   join us in this.dbContext.UserRoleFacilityConfigs on ns.NurseStation_Id equals us.NurseStation_Id
                                   where ns.Facility_Id == facility.Facility_Id && ns.NurseStation_Status == 1 && us.User_Id == userId && us.UserRole_Status == 1
                                   select new NurseStationDropEntity
                                   {
                                       NurseStation_Id = ns.NurseStation_Id,
                                       NurseStation_Code = ns.NurseStation_Code,
                                       NurseStation_Name = ns.NurseStation_Name,
                                   }).Distinct().OrderBy(item => item.NurseStation_Name).ToList();

                    //ToDo: Search Nursestation of current or latest admit record
                    data.ResidentDrop = (from d in this.dbContext.Demographics
                                         join v in this.dbContext.VisitInfoes on d.Patient_Id equals v.Patient_Id
                                         where v.NursingStationId == nurseStationId && v.PVisit_Status != 10
                                         //&& d.MergeID == null
                                         select new ResidentDropEntity()
                                         {
                                             Patient_Id = d.Patient_Id,
                                             PatientName = d.PatientLastName + ", " + d.PatientFirstName + " " + d.PatientMiddleInitial
                                         }).Distinct().OrderBy(item => item.PatientName).ToList();
                }
                return data;
            }
            return null;
        }
        public List<PatientTypeEntity> GetAllPatientTypesByPId(int patientId)
        {
            var records = (from ct in this.dbContext.ColourTypes
                           join pt in this.dbContext.PatientTypes on ct.PatientType_Id equals pt.PatientType_Id
                           where ct.Patient_Id == patientId
                           select new PatientTypeEntity
                           {
                               PatientType_Id = (int)pt.PatientType_Id,
                               Color_Code = pt.Color_Code,
                               Color_Description = pt.Color_Description

                           }).ToList();
            return records;
        }

        //public int InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails)
        //{
        //    physcianDetails.Physician_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
        //    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //    var record = this.autoMapper.Map<PhysicianDetailsEntity, PhysicianDetail>(physcianDetails);
        //    string[] nursestations = physcianDetails.NurseStations.ToString().Split(',');
        //    if (physcianDetails.Physician_Id == 0)
        //    {
        //        var data = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.PhysicianNPI == record.PhysicianNPI).ToList();
        //        if (data.Count == 0)
        //        {
        //            foreach (var item in nursestations)
        //            {
        //                int NsId = Convert.ToInt32(item);

        //                record.NurseStation_Id = NsId;
        //                this.dbContext.PhysicianDetails.Add(record);
        //                this.dbContext.SaveChanges();


        //                foreach (var lic in physcianDetails.LicensesData)
        //                {

        //                    string query = "[DrFirst].InsertPrescribeLicense";
        //                    string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                    DataSet ds = new DataSet();
        //                    using (SqlConnection con = new SqlConnection(constrEmar))
        //                    {
        //                        con.Open();
        //                        using (SqlCommand cmd = new SqlCommand(query))
        //                        {
        //                            cmd.Connection = con;
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.CommandTimeout = 180;
        //                            cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                            cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                            cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                            cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = record.Physician_Id;
        //                            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                            {
        //                                sda.Fill(ds);
        //                            }


        //                        }
        //                    }
        //                }

        //            }

        //            if (claimsIdentity.FindFirst("UserId").Value != "")
        //            {
        //                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
        //                if (physcianDetails.NurseStations.Count() > 0)
        //                {
        //                    var selectedNurseStationsSave = string.Join(",", physcianDetails.NurseStations);
        //                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
        //                    userRecentFacObj.User_Id = userId;
        //                    userRecentFacObj.Facility_Id = (int)physcianDetails.Facility_Id;
        //                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
        //                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
        //                }
        //            }
        //            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //            {
        //                Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                Comments = record.Physician_Id.ToString(),
        //                Session_Id = 0,
        //                Time = DateTime.Now,
        //                UserActivity_Id = 0
        //            };

        //            //Insert Licenses




        //            _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //            return 1;
        //        }
        //        else
        //        {
        //            //With facility and physician NPI already records exists
        //            return 2;
        //        }
        //    }
        //    //update
        //    else if (physcianDetails.Physician_Id == 1)
        //    {
        //        string query1 = "[DrFirst].[PrcupdatePrescribeLicenseStatus]";
        //        string constrEmar1 = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //        DataSet ds1 = new DataSet();
        //        using (SqlConnection con = new SqlConnection(constrEmar1))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(query1))
        //            {
        //                cmd.Connection = con;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandTimeout = 180;
        //                cmd.Parameters.Add("@PrescriberId", SqlDbType.VarChar).Value = physcianDetails.PId;

        //                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                {
        //                    sda.Fill(ds1);
        //                }


        //            }
        //        }





        //        if (physcianDetails.OldPhysicianNPI == record.PhysicianNPI && physcianDetails.OldFacilityId == record.Facility_Id)
        //        {
        //            int PhyId = 0;
        //            var list = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.PhysicianNPI == record.PhysicianNPI).ToList();
        //            var nurseStationIds = list.Select(p => (int)p.NurseStation_Id).ToArray();
        //            int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
        //            var items = nurseStationIds.Except(nsIds);
        //            var defaultCheck = (from ns in this.dbContext.NursingStations
        //                                join pd in this.dbContext.PhysicianDetails on ns.DefaultPhysician_Id equals pd.Physician_Id
        //                                where pd.PhysicianNPI == record.PhysicianNPI && items.Contains(ns.NurseStation_Id)
        //                                select new { ns.NurseStation_Id }).Count();
        //            if (defaultCheck == 0)
        //            {
        //                foreach (var item in items)
        //                {
        //                    var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == record.Facility_Id && ph.PhysicianNPI == record.PhysicianNPI && ph.NurseStation_Id == item).FirstOrDefault();
        //                    if (existingRecord != null)
        //                    {
        //                        //this.dbContext.PhysicianDetails.Remove(existingRecord);
        //                        existingRecord.Physician_Status = 2;
        //                        this.dbContext.SaveChanges();
        //                    }
        //                }
        //                foreach (var item in nursestations)
        //                {
        //                    int NsId = Convert.ToInt32(item);

        //                    var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == record.Facility_Id && ph.PhysicianNPI == record.PhysicianNPI && ph.NurseStation_Id == NsId).FirstOrDefault();
        //                    if (existingRecord == null)
        //                    {

        //                        record.NurseStation_Id = NsId;
        //                        this.dbContext.PhysicianDetails.Add(record);
        //                        this.dbContext.SaveChanges();
        //                        PhyId = record.Physician_Id;
        //                        foreach (var lic in physcianDetails.LicensesData)
        //                        {

        //                            string query = "[DrFirst].InsertPrescribeLicense";
        //                            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                            DataSet ds = new DataSet();
        //                            using (SqlConnection con = new SqlConnection(constrEmar))
        //                            {
        //                                con.Open();
        //                                using (SqlCommand cmd = new SqlCommand(query))
        //                                {
        //                                    cmd.Connection = con;
        //                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                    cmd.CommandTimeout = 180;
        //                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                    cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                    cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                    {
        //                                        sda.Fill(ds);
        //                                    }


        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        existingRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                        existingRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                        existingRecord.PhysicianCity = record.PhysicianCity;
        //                        existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                        existingRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                        existingRecord.PhysicianFName = record.PhysicianFName;
        //                        existingRecord.PhysicianLName = record.PhysicianLName;
        //                        existingRecord.PhysicianNPI = record.PhysicianNPI;
        //                        existingRecord.PhysicianState = record.PhysicianState;
        //                        existingRecord.PhysicianZip = record.PhysicianZip;
        //                        existingRecord.Physician_CreatedBy = record.Physician_CreatedBy;
        //                        existingRecord.Physician_CreatedDate = record.Physician_CreatedDate;
        //                        existingRecord.Physician_Status = record.Physician_Status;
        //                        existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                        existingRecord.NurseStation_Id = NsId;
        //                        existingRecord.Credentials = record.Credentials;
        //                        existingRecord.PrimarySpec = record.PrimarySpec;
        //                        existingRecord.SupervisingPhy = record.SupervisingPhy;
        //                        existingRecord.Physician_Phone = record.Physician_Phone;

        //                        this.dbContext.SaveChanges();
        //                        PhyId = existingRecord.Physician_Id;


        //                    }

        //                    foreach (var lic in physcianDetails.LicensesData)
        //                    {

        //                        string query = "[DrFirst].InsertPrescribeLicense";
        //                        string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                        DataSet ds = new DataSet();
        //                        using (SqlConnection con = new SqlConnection(constrEmar))
        //                        {
        //                            con.Open();
        //                            using (SqlCommand cmd = new SqlCommand(query))
        //                            {
        //                                cmd.Connection = con;
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.CommandTimeout = 180;
        //                                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                {
        //                                    sda.Fill(ds);
        //                                }


        //                            }
        //                        }
        //                    }

        //                }
        //                var recordsBasedOnNpi = this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == record.PhysicianNPI).ToList();
        //                foreach (var phyRecord in recordsBasedOnNpi)
        //                {
        //                    phyRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                    phyRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                    phyRecord.PhysicianCity = record.PhysicianCity;
        //                    phyRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                    phyRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                    phyRecord.PhysicianFName = record.PhysicianFName;
        //                    phyRecord.PhysicianLName = record.PhysicianLName;
        //                    phyRecord.PhysicianNPI = record.PhysicianNPI;
        //                    phyRecord.PhysicianState = record.PhysicianState;
        //                    phyRecord.PhysicianZip = record.PhysicianZip;
        //                    phyRecord.Credentials = record.Credentials;
        //                    phyRecord.PrimarySpec = record.PrimarySpec;
        //                    phyRecord.SupervisingPhy = record.SupervisingPhy;
        //                    phyRecord.Physician_Phone = record.Physician_Phone;
        //                    this.dbContext.SaveChanges();
        //                    PhyId = phyRecord.Physician_Id;


        //                }
        //                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //                {
        //                    Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                    Comments = record.Physician_Id.ToString(),
        //                    Session_Id = 0,
        //                    Time = DateTime.Now,
        //                    UserActivity_Id = 0
        //                };

        //                //Insert data in License


        //                _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //            }
        //            else
        //            {
        //                return 3;
        //            }

        //        }
        //        else if (physcianDetails.OldPhysicianNPI != record.PhysicianNPI && physcianDetails.OldFacilityId == record.Facility_Id)
        //        {
        //            int PhyId = 0;
        //            var oldPhNpi = physcianDetails.OldPhysicianNPI;
        //            var list = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.PhysicianNPI == oldPhNpi).ToList();
        //            if (list.Count >= 0)
        //            {
        //                var nurseStationIds = list.Select(p => (int)p.NurseStation_Id).ToArray();
        //                int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
        //                var items = nurseStationIds.Except(nsIds);
        //                foreach (var item in items)
        //                {
        //                    var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == record.Facility_Id && ph.PhysicianNPI == oldPhNpi && ph.NurseStation_Id == item).FirstOrDefault();
        //                    if (existingRecord != null)
        //                    {
        //                        //this.dbContext.PhysicianDetails.Remove(existingRecord);
        //                        existingRecord.PhysicianNPI = record.PhysicianNPI;
        //                        this.dbContext.SaveChanges();
        //                        PhyId = existingRecord.Physician_Id;
        //                    }
        //                }
        //                foreach (var item in nursestations)
        //                {
        //                    int NsId = Convert.ToInt32(item);

        //                    var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == record.Facility_Id && ph.PhysicianNPI == oldPhNpi && ph.NurseStation_Id == NsId).FirstOrDefault();
        //                    if (existingRecord == null)
        //                    {

        //                        record.NurseStation_Id = NsId;
        //                        this.dbContext.PhysicianDetails.Add(record);
        //                        this.dbContext.SaveChanges();
        //                        PhyId = record.Physician_Id;
        //                        foreach (var lic in physcianDetails.LicensesData)
        //                        {

        //                            string query = "[DrFirst].InsertPrescribeLicense";
        //                            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                            DataSet ds = new DataSet();
        //                            using (SqlConnection con = new SqlConnection(constrEmar))
        //                            {
        //                                con.Open();
        //                                using (SqlCommand cmd = new SqlCommand(query))
        //                                {
        //                                    cmd.Connection = con;
        //                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                    cmd.CommandTimeout = 180;
        //                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                    cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                    cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                    {
        //                                        sda.Fill(ds);
        //                                    }


        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        existingRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                        existingRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                        existingRecord.PhysicianCity = record.PhysicianCity;
        //                        existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                        existingRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                        existingRecord.PhysicianFName = record.PhysicianFName;
        //                        existingRecord.PhysicianLName = record.PhysicianLName;
        //                        existingRecord.PhysicianNPI = record.PhysicianNPI;
        //                        existingRecord.PhysicianState = record.PhysicianState;
        //                        existingRecord.PhysicianZip = record.PhysicianZip;
        //                        existingRecord.Physician_CreatedBy = record.Physician_CreatedBy;
        //                        existingRecord.Physician_CreatedDate = record.Physician_CreatedDate;
        //                        existingRecord.Physician_Status = record.Physician_Status;
        //                        existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                        existingRecord.NurseStation_Id = NsId;
        //                        existingRecord.Credentials = record.Credentials;
        //                        existingRecord.PrimarySpec = record.PrimarySpec;
        //                        existingRecord.SupervisingPhy = record.SupervisingPhy;
        //                        this.dbContext.SaveChanges();
        //                        PhyId = existingRecord.Physician_Id;
        //                    }

        //                    foreach (var lic in physcianDetails.LicensesData)
        //                    {

        //                        string query = "[DrFirst].InsertPrescribeLicense";
        //                        string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                        DataSet ds = new DataSet();
        //                        using (SqlConnection con = new SqlConnection(constrEmar))
        //                        {
        //                            con.Open();
        //                            using (SqlCommand cmd = new SqlCommand(query))
        //                            {
        //                                cmd.Connection = con;
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.CommandTimeout = 180;
        //                                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                {
        //                                    sda.Fill(ds);
        //                                }


        //                            }
        //                        }
        //                    }


        //                }
        //                var recordsBasedOnNpi = this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == physcianDetails.OldPhysicianNPI).ToList();
        //                foreach (var phyRecord in recordsBasedOnNpi)
        //                {
        //                    phyRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                    phyRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                    phyRecord.PhysicianCity = record.PhysicianCity;
        //                    phyRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                    phyRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                    phyRecord.PhysicianFName = record.PhysicianFName;
        //                    phyRecord.PhysicianLName = record.PhysicianLName;
        //                    phyRecord.PhysicianNPI = record.PhysicianNPI;
        //                    phyRecord.PhysicianState = record.PhysicianState;
        //                    phyRecord.PhysicianZip = record.PhysicianZip;
        //                    phyRecord.Credentials = record.Credentials;
        //                    phyRecord.PrimarySpec = record.PrimarySpec;
        //                    phyRecord.SupervisingPhy = record.SupervisingPhy;
        //                    this.dbContext.SaveChanges();
        //                    PhyId = phyRecord.Physician_Id;


        //                }
        //                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //                {
        //                    Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                    Comments = record.Physician_Id.ToString(),
        //                    Session_Id = 0,
        //                    Time = DateTime.Now,
        //                    UserActivity_Id = 0
        //                };
        //                //Insert Licenses


        //                _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //            }
        //        }
        //        else if (physcianDetails.OldPhysicianNPI == record.PhysicianNPI && physcianDetails.OldFacilityId != record.Facility_Id)
        //        {
        //            int PhyId = 0;
        //            var oldFacility = physcianDetails.OldFacilityId;
        //            if (this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.PhysicianNPI == record.PhysicianNPI).Count() > 0)
        //            {
        //                //With facility and physician NPI already records exists
        //                return 2;
        //            }
        //            else
        //            {
        //                var list = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == oldFacility && p.PhysicianNPI == record.PhysicianNPI).ToList();
        //                if (list.Count >= 0)
        //                {
        //                    var nurseStationIds = list.Select(p => (int)p.NurseStation_Id).ToArray();
        //                    int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
        //                    var items = nurseStationIds.Except(nsIds);
        //                    foreach (var item in items)
        //                    {
        //                        var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == oldFacility && ph.PhysicianNPI == record.PhysicianNPI && ph.NurseStation_Id == item).FirstOrDefault();
        //                        if (existingRecord != null)
        //                        {
        //                            //this.dbContext.PhysicianDetails.Remove(existingRecord);
        //                            existingRecord.Physician_Status = 2;
        //                            this.dbContext.SaveChanges();
        //                            PhyId = existingRecord.Physician_Id;

        //                            foreach (var lic in physcianDetails.LicensesData)
        //                            {

        //                                string query = "[DrFirst].InsertPrescribeLicense";
        //                                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                                DataSet ds = new DataSet();
        //                                using (SqlConnection con = new SqlConnection(constrEmar))
        //                                {
        //                                    con.Open();
        //                                    using (SqlCommand cmd = new SqlCommand(query))
        //                                    {
        //                                        cmd.Connection = con;
        //                                        cmd.CommandType = CommandType.StoredProcedure;
        //                                        cmd.CommandTimeout = 180;
        //                                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                        cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                        cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                        cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                        {
        //                                            sda.Fill(ds);
        //                                        }


        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    foreach (var item in nursestations)
        //                    {
        //                        int NsId = Convert.ToInt32(item);

        //                        var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == oldFacility && ph.PhysicianNPI == record.PhysicianNPI && ph.NurseStation_Id == NsId).FirstOrDefault();
        //                        if (existingRecord == null)
        //                        {

        //                            record.NurseStation_Id = NsId;
        //                            this.dbContext.PhysicianDetails.Add(record);
        //                            this.dbContext.SaveChanges();
        //                            PhyId = record.Physician_Id;
        //                        }
        //                        else
        //                        {
        //                            existingRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                            existingRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                            existingRecord.PhysicianCity = record.PhysicianCity;
        //                            existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                            existingRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                            existingRecord.PhysicianFName = record.PhysicianFName;
        //                            existingRecord.PhysicianLName = record.PhysicianLName;
        //                            existingRecord.PhysicianNPI = record.PhysicianNPI;
        //                            existingRecord.PhysicianState = record.PhysicianState;
        //                            existingRecord.PhysicianZip = record.PhysicianZip;
        //                            existingRecord.Physician_CreatedBy = record.Physician_CreatedBy;
        //                            existingRecord.Physician_CreatedDate = record.Physician_CreatedDate;
        //                            existingRecord.Physician_Status = record.Physician_Status;
        //                            existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                            existingRecord.NurseStation_Id = NsId;
        //                            existingRecord.Credentials = record.Credentials;
        //                            existingRecord.PrimarySpec = record.PrimarySpec;
        //                            existingRecord.SupervisingPhy = record.SupervisingPhy;
        //                            this.dbContext.SaveChanges();
        //                            PhyId = existingRecord.Physician_Id;



        //                        }

        //                        foreach (var lic in physcianDetails.LicensesData)
        //                        {

        //                            string query = "[DrFirst].InsertPrescribeLicense";
        //                            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                            DataSet ds = new DataSet();
        //                            using (SqlConnection con = new SqlConnection(constrEmar))
        //                            {
        //                                con.Open();
        //                                using (SqlCommand cmd = new SqlCommand(query))
        //                                {
        //                                    cmd.Connection = con;
        //                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                    cmd.CommandTimeout = 180;
        //                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                    cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                    cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                    {
        //                                        sda.Fill(ds);
        //                                    }


        //                                }
        //                            }
        //                        }


        //                    }
        //                    var recordsBasedOnNpi = this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == record.PhysicianNPI).ToList();
        //                    foreach (var phyRecord in recordsBasedOnNpi)
        //                    {
        //                        phyRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                        phyRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                        phyRecord.PhysicianCity = record.PhysicianCity;
        //                        phyRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                        phyRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                        phyRecord.PhysicianFName = record.PhysicianFName;
        //                        phyRecord.PhysicianLName = record.PhysicianLName;
        //                        phyRecord.PhysicianNPI = record.PhysicianNPI;
        //                        phyRecord.PhysicianState = record.PhysicianState;
        //                        phyRecord.PhysicianZip = record.PhysicianZip;
        //                        phyRecord.Credentials = record.Credentials;
        //                        phyRecord.PrimarySpec = record.PrimarySpec;
        //                        phyRecord.SupervisingPhy = record.SupervisingPhy;
        //                        this.dbContext.SaveChanges();
        //                        PhyId = phyRecord.Physician_Id;


        //                    }
        //                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //                    {
        //                        Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                        Comments = record.Physician_Id.ToString(),
        //                        Session_Id = 0,
        //                        Time = DateTime.Now,
        //                        UserActivity_Id = 0
        //                    };
        //                    //Insert Licenses


        //                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //                }
        //            }
        //        }
        //        else if (physcianDetails.OldPhysicianNPI != record.PhysicianNPI && physcianDetails.OldFacilityId != record.Facility_Id)
        //        {
        //            if (this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == record.Facility_Id && p.PhysicianNPI == record.PhysicianNPI).Count() > 0)
        //            {
        //                //With facility and physician NPI already records exists
        //                return 2;
        //            }
        //            else
        //            {
        //                int PhyId = 0;
        //                var data = this.dbContext.PhysicianDetails.Where(p => p.Facility_Id == physcianDetails.OldFacilityId && p.PhysicianNPI == physcianDetails.OldPhysicianNPI).ToList();
        //                if (data.Count == 0)
        //                {
        //                    foreach (var item in nursestations)
        //                    {
        //                        int NsId = Convert.ToInt32(item);

        //                        record.NurseStation_Id = NsId;
        //                        this.dbContext.PhysicianDetails.Add(record);
        //                        this.dbContext.SaveChanges();
        //                        PhyId = record.Physician_Id;


        //                        foreach (var lic in physcianDetails.LicensesData)
        //                        {

        //                            string query = "[DrFirst].InsertPrescribeLicense";
        //                            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                            DataSet ds = new DataSet();
        //                            using (SqlConnection con = new SqlConnection(constrEmar))
        //                            {
        //                                con.Open();
        //                                using (SqlCommand cmd = new SqlCommand(query))
        //                                {
        //                                    cmd.Connection = con;
        //                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                    cmd.CommandTimeout = 180;
        //                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                    cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                    cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                    {
        //                                        sda.Fill(ds);
        //                                    }


        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var nurseStationIds = data.Select(p => (int)p.NurseStation_Id).ToArray();
        //                    int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
        //                    var items = nurseStationIds.Except(nsIds);
        //                    foreach (var item in items)
        //                    {
        //                        var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == physcianDetails.OldFacilityId && ph.PhysicianNPI == physcianDetails.OldPhysicianNPI && ph.NurseStation_Id == item).FirstOrDefault();
        //                        if (existingRecord != null)
        //                        {
        //                            //this.dbContext.PhysicianDetails.Remove(existingRecord);
        //                            existingRecord.Physician_Status = 2;
        //                            this.dbContext.SaveChanges();
        //                        }
        //                    }
        //                    foreach (var item in nursestations)
        //                    {
        //                        int NsId = Convert.ToInt32(item);

        //                        var existingRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Facility_Id == physcianDetails.OldFacilityId && ph.PhysicianNPI == physcianDetails.OldPhysicianNPI && ph.NurseStation_Id == NsId).FirstOrDefault();
        //                        if (existingRecord == null)
        //                        {

        //                            record.NurseStation_Id = NsId;
        //                            this.dbContext.PhysicianDetails.Add(record);
        //                            this.dbContext.SaveChanges();
        //                            PhyId = record.Physician_Id;
        //                        }
        //                        else
        //                        {
        //                            existingRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                            existingRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                            existingRecord.PhysicianCity = record.PhysicianCity;
        //                            existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                            existingRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                            existingRecord.PhysicianFName = record.PhysicianFName;
        //                            existingRecord.PhysicianLName = record.PhysicianLName;
        //                            existingRecord.PhysicianNPI = record.PhysicianNPI;
        //                            existingRecord.PhysicianState = record.PhysicianState;
        //                            existingRecord.PhysicianZip = record.PhysicianZip;
        //                            existingRecord.Physician_CreatedBy = record.Physician_CreatedBy;
        //                            existingRecord.Physician_CreatedDate = record.Physician_CreatedDate;
        //                            existingRecord.Physician_Status = record.Physician_Status;
        //                            existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                            existingRecord.NurseStation_Id = NsId;
        //                            existingRecord.Credentials = record.Credentials;
        //                            existingRecord.PrimarySpec = record.PrimarySpec;
        //                            existingRecord.SupervisingPhy = record.SupervisingPhy;
        //                            this.dbContext.SaveChanges();
        //                            PhyId = existingRecord.Physician_Id;
        //                        }

        //                        foreach (var lic in physcianDetails.LicensesData)
        //                        {

        //                            string query = "[DrFirst].InsertPrescribeLicense";
        //                            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                            DataSet ds = new DataSet();
        //                            using (SqlConnection con = new SqlConnection(constrEmar))
        //                            {
        //                                con.Open();
        //                                using (SqlCommand cmd = new SqlCommand(query))
        //                                {
        //                                    cmd.Connection = con;
        //                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                    cmd.CommandTimeout = 180;
        //                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                    cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                    cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                    {
        //                                        sda.Fill(ds);
        //                                    }


        //                                }
        //                            }
        //                        }

        //                    }
        //                    var recordsBasedOnNpi = this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == physcianDetails.OldPhysicianNPI).ToList();
        //                    foreach (var phyRecord in recordsBasedOnNpi)
        //                    {
        //                        phyRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                        phyRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                        phyRecord.PhysicianCity = record.PhysicianCity;
        //                        phyRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                        phyRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                        phyRecord.PhysicianFName = record.PhysicianFName;
        //                        phyRecord.PhysicianLName = record.PhysicianLName;
        //                        phyRecord.PhysicianNPI = record.PhysicianNPI;
        //                        phyRecord.PhysicianState = record.PhysicianState;
        //                        phyRecord.PhysicianZip = record.PhysicianZip;
        //                        phyRecord.Credentials = record.Credentials;
        //                        phyRecord.PrimarySpec = record.PrimarySpec;
        //                        phyRecord.SupervisingPhy = record.SupervisingPhy;
        //                        this.dbContext.SaveChanges();
        //                        PhyId = phyRecord.Physician_Id;



        //                    }
        //                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //                    {
        //                        Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                        Comments = record.Physician_Id.ToString(),
        //                        Session_Id = 0,
        //                        Time = DateTime.Now,
        //                        UserActivity_Id = 0
        //                    };
        //                    //Insert Licenses


        //                    _userActivityRepository.InsertUserActivityDetails(activityEntity);

        //                }
        //            }
        //            //var physicinaNpi = physcianDetails.OldPhysicianNPI == record.PhysicianNPI ? record.PhysicianNPI : physcianDetails.OldPhysicianNPI;

        //        }
        //    }
        //    if (claimsIdentity.FindFirst("UserId").Value != "")
        //    {
        //        int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
        //        if (physcianDetails.NurseStations.Count() > 0)
        //        {
        //            var selectedNurseStationsSave = string.Join(",", physcianDetails.NurseStations);
        //            RecentFacEntity userRecentFacObj = new RecentFacEntity();
        //            userRecentFacObj.User_Id = userId;
        //            userRecentFacObj.Facility_Id = (int)physcianDetails.Facility_Id;
        //            userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
        //            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
        //        }
        //    }
        //    return 1;

        //}
        //public int InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails)
        //{
        //    physcianDetails.Physician_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
        //    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //    var record = this.autoMapper.Map<PhysicianDetailsEntity, PhysicianDetail>(physcianDetails);


        //    string[] nursestations = physcianDetails.NurseStations.ToString().Split(',');
        //    List<string> Fac = physcianDetails.FacId.Split(',').ToList();


        //    List<string> nst = physcianDetails.NurseStations.Split(',').ToList();

        //    //&& (Fac.Any(al => us.Facility_id.ToString().Contains(al))
        //    //inserting New record
        //    if (physcianDetails.Physician_Id == 0)
        //    {
        //        var data = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == record.PhysicianNPI && (Fac.Any(al => p.Facility_Id.ToString().Contains(al)))).ToList();
        //        if (data.Count == 0)
        //        {
        //            foreach (var item in nursestations)
        //            {
        //                int NsId = Convert.ToInt32(item);
        //                int? facId = this.dbContext.NursingStations.Where(p => p.NurseStation_Id == NsId && p.NurseStation_Status == 1).Select(p => p.Facility_Id).FirstOrDefault();


        //                if(facId > 0)
        //                {

        //                    record.NurseStation_Id = NsId;
        //                    record.Facility_Id = facId;

        //                    this.dbContext.PhysicianDetails.Add(record);
        //                    this.dbContext.SaveChanges();


        //                    foreach (var lic in physcianDetails.LicensesData)
        //                    {

        //                        string query = "[DrFirst].InsertPrescribeLicense";
        //                        string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                        DataSet ds = new DataSet();
        //                        using (SqlConnection con = new SqlConnection(constrEmar))
        //                        {
        //                            con.Open();
        //                            using (SqlCommand cmd = new SqlCommand(query))
        //                            {
        //                                cmd.Connection = con;
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.CommandTimeout = 180;
        //                                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = record.Physician_Id;
        //                                cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                {
        //                                    sda.Fill(ds);
        //                                }


        //                            }
        //                        }
        //                    }

        //                }




        //            }
        //            if (claimsIdentity.FindFirst("UserId").Value != "")
        //            {
        //                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
        //                if (physcianDetails.NurseStations.Count() > 0)
        //                {
        //                    var selectedNurseStationsSave = string.Join(",", physcianDetails.NurseStations);
        //                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
        //                    userRecentFacObj.User_Id = userId;
        //                    userRecentFacObj.Facility_Id = 0;
        //                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
        //                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
        //                }
        //            }

        //            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //            {
        //                Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                Comments = record.Physician_Id.ToString(),
        //                Session_Id = 0,
        //                Time = DateTime.Now,
        //                UserActivity_Id = 0
        //            };

        //            //Insert Licenses




        //            _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //            return 1;
        //        }
        //        else
        //        {
        //            //With facility and physician NPI already records exists
        //            return 2;
        //        }
        //    }
        //    //update Record
        //    else if (physcianDetails.Physician_Id == 1)
        //    {


        //        // int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
        //        // var items = nurseStationIds.Except(nsIds);











        //        //checking purpose updating records
        //        if (0 == 0)
        //        {
        //            int PhyId = 0;

        //            var listbasedionNpi = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == physcianDetails.OldPhysicianNPI).ToList();

        //            var nurseStationIds = listbasedionNpi.Select(p => (int)p.NurseStation_Id).ToArray();
        //            int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
        //            var items = nurseStationIds.Except(nsIds);



        //            foreach (var it in listbasedionNpi)
        //            {
        //                string query1 = "[DrFirst].[PrcupdatePrescribeLicenseStatus]";
        //                string constrEmar1 = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                DataSet ds1 = new DataSet();
        //                using (SqlConnection con = new SqlConnection(constrEmar1))
        //                {
        //                    con.Open();
        //                    using (SqlCommand cmd = new SqlCommand(query1))
        //                    {
        //                        cmd.Connection = con;
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.CommandTimeout = 180;
        //                        cmd.Parameters.Add("@PrescriberId", SqlDbType.VarChar).Value = physcianDetails.PId;

        //                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                        {
        //                            sda.Fill(ds1);
        //                        }


        //                    }
        //                }
        //                var existingRecord = this.dbContext.PhysicianDetails.Where(p => p.Physician_Id == it.Physician_Id).FirstOrDefault();




        //                var defaultCheck = (from ns in this.dbContext.NursingStations
        //                                    join pd in this.dbContext.PhysicianDetails on ns.DefaultPhysician_Id equals pd.Physician_Id
        //                                    where pd.PhysicianNPI == record.PhysicianNPI && items.Contains(ns.NurseStation_Id)
        //                                    select new { ns.NurseStation_Id }).Count();
        //                if (defaultCheck == 0 || record.Physician_Status == 1)
        //                {


        //                    existingRecord.PhysicianAddress1 = record.PhysicianAddress1;
        //                    existingRecord.PhysicianAddress2 = record.PhysicianAddress2;
        //                    existingRecord.PhysicianCity = record.PhysicianCity;
        //                    existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                    existingRecord.PhysicianDEANumber = record.PhysicianDEANumber;
        //                    existingRecord.PhysicianFName = record.PhysicianFName;
        //                    existingRecord.PhysicianLName = record.PhysicianLName;
        //                    existingRecord.PhysicianNPI = record.PhysicianNPI;
        //                    existingRecord.PhysicianState = record.PhysicianState;
        //                    existingRecord.PhysicianZip = record.PhysicianZip;
        //                    existingRecord.Physician_CreatedBy = record.Physician_CreatedBy;
        //                    existingRecord.Physician_CreatedDate = record.Physician_CreatedDate;
        //                    existingRecord.Physician_Status = record.Physician_Status;
        //                    existingRecord.PhysicianCountryID = record.PhysicianCountryID;
        //                    //   existingRecord.NurseStation_Id = NsId;
        //                    existingRecord.Credentials = record.Credentials;
        //                    existingRecord.PrimarySpec = record.PrimarySpec;
        //                    existingRecord.SupervisingPhy = record.SupervisingPhy;
        //                    existingRecord.Physician_Phone = record.Physician_Phone;

        //                    this.dbContext.SaveChanges();
        //                    int PhyIdi = existingRecord.Physician_Id;




        //                    foreach (var lic in physcianDetails.LicensesData)
        //                    {

        //                        string query = "[DrFirst].InsertPrescribeLicense";
        //                        string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                        DataSet ds = new DataSet();
        //                        using (SqlConnection con = new SqlConnection(constrEmar))
        //                        {
        //                            con.Open();
        //                            using (SqlCommand cmd = new SqlCommand(query))
        //                            {
        //                                cmd.Connection = con;
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.CommandTimeout = 180;
        //                                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                                cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                                cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyIdi;
        //                                cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                                {
        //                                    sda.Fill(ds);
        //                                }


        //                            }
        //                        }
        //                    }



        //                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //                    {
        //                        Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
        //                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //                        Comments = record.Physician_Id.ToString(),
        //                        Session_Id = 0,
        //                        Time = DateTime.Now,
        //                        UserActivity_Id = 0
        //                    };

        //                    //Insert data in License


        //                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //                }

        //                else
        //                {
        //                    return 3;
        //                }

        //            }
        //        }



        //        //inserting new nursing station those are not avilable previously
        //        //from front end
        //        string[] nsidfe = physcianDetails.NurseStations.ToString().Split(','); 

        //         var listbasedionNpiNST1 = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == physcianDetails.OldPhysicianNPI && p.Physician_Status == 1).Select(p => p.NurseStation_Id).ToList();

        //        //converting intocomma sepearated
        //        string listbasedionNpiNST2 = string.Join(",", listbasedionNpiNST1.Select(x => x.Value));




        //        //then converting into array
        //        string[] listbasedionNpiNST = listbasedionNpiNST2.ToString().Split(',');




        //        //checking nurse station are not avilable in db
        //        var array3 =  nsidfe.Except(listbasedionNpiNST).ToList();
        //        //getting not avilble in table 
        //        //var onlyInArray2 = nursestationsup.Except(listbasedionNpiNST);

        //        // insert new records in update 
        //        foreach (var ninst in array3)
        //        {
        //            int Nid = Convert.ToInt32(ninst);

        //            int? facId = this.dbContext.NursingStations.Where(p => p.NurseStation_Id == Nid && p.NurseStation_Status == 1).Select(p => p.Facility_Id).FirstOrDefault();

        //            if(facId > 0)
        //            {
        //                record.NurseStation_Id = Nid;
        //                record.Facility_Id = facId;
        //                this.dbContext.PhysicianDetails.Add(record);
        //                this.dbContext.SaveChanges();
        //                int PhyId = record.Physician_Id;

        //                foreach (var lic in physcianDetails.LicensesData)
        //                {

        //                    string query = "[DrFirst].InsertPrescribeLicense";
        //                    string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //                    DataSet ds = new DataSet();
        //                    using (SqlConnection con = new SqlConnection(constrEmar))
        //                    {
        //                        con.Open();
        //                        using (SqlCommand cmd = new SqlCommand(query))
        //                        {
        //                            cmd.Connection = con;
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.CommandTimeout = 180;
        //                            cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
        //                            cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
        //                            cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
        //                            cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
        //                            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

        //                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //                            {
        //                                sda.Fill(ds);
        //                            }


        //                        }
        //                    }
        //                }
        //            }



        //        }




        //        //}


        //    }
        //    if (claimsIdentity.FindFirst("UserId").Value != "")
        //    {
        //        int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
        //        if (physcianDetails.NurseStations.Count() > 0)
        //        {
        //            var selectedNurseStationsSave = string.Join(",", physcianDetails.NurseStations);
        //            RecentFacEntity userRecentFacObj = new RecentFacEntity();
        //            userRecentFacObj.User_Id = userId;
        //            //userRecentFacObj.Facility_Id = (int)physcianDetails.Facility_Id;
        //            userRecentFacObj.Facility_Id = 0;

        //            userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
        //            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
        //        }
        //    }
        //    return 1;

        //}
        public int InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails)
        {
            physcianDetails.Physician_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var record = this.autoMapper.Map<PhysicianDetailsEntity, PhysicianDetail>(physcianDetails);


            string[] nursestations = physcianDetails.NurseStations.ToString().Split(',');
            List<string> Fac = physcianDetails.FacId.Split(',').ToList();


            List<string> nst = physcianDetails.NurseStations.Split(',').ToList();

            //&& (Fac.Any(al => us.Facility_id.ToString().Contains(al))
            //inserting New record
            if (physcianDetails.Physician_Id == 0)
            {
                var data = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == record.PhysicianNPI && (Fac.Any(al => p.Facility_Id.ToString().Contains(al)))).ToList();
                if (data.Count == 0)
                {
                    foreach (var item in nursestations)
                    {
                        int NsId = Convert.ToInt32(item);
                        int? facId = this.dbContext.NursingStations.Where(p => p.NurseStation_Id == NsId && p.NurseStation_Status == 1).Select(p => p.Facility_Id).FirstOrDefault();


                        if (facId > 0)
                        {

                            record.NurseStation_Id = NsId;
                            record.Facility_Id = facId;

                            this.dbContext.PhysicianDetails.Add(record);
                            this.dbContext.SaveChanges();


                            foreach (var lic in physcianDetails.LicensesData)
                            {

                                string query = "[DrFirst].InsertPrescribeLicense";
                                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                                DataSet ds = new DataSet();
                                using (SqlConnection con = new SqlConnection(constrEmar))
                                {
                                    con.Open();
                                    using (SqlCommand cmd = new SqlCommand(query))
                                    {
                                        cmd.Connection = con;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandTimeout = 180;
                                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
                                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
                                        cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
                                        cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = record.Physician_Id;
                                        cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

                                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                        {
                                            sda.Fill(ds);
                                        }


                                    }
                                }
                            }

                        }




                    }
                    if (claimsIdentity.FindFirst("UserId").Value != "")
                    {
                        int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                        if (physcianDetails.NurseStations.Count() > 0)
                        {
                            var selectedNurseStationsSave = string.Join(",", physcianDetails.NurseStations);
                            RecentFacEntity userRecentFacObj = new RecentFacEntity();
                            userRecentFacObj.User_Id = userId;
                            userRecentFacObj.Facility_Id = 0;
                            userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                        }
                    }

                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.Physician_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0
                    };

                    //Insert Licenses




                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
                else
                {
                    //With facility and physician NPI already records exists
                    return 2;
                }
            }
            //update Record
            else if (physcianDetails.Physician_Id == 1)
            {


                if (0 == 0)
                {
                    int PhyId = 0;

                    var listbasedionNpi = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == physcianDetails.OldPhysicianNPI).ToList();

                    var nurseStationIds = listbasedionNpi.Select(p => (int)p.NurseStation_Id).ToArray();
                    int[] nsIds = Array.ConvertAll(nursestations, int.Parse);
                    var items = nurseStationIds.Except(nsIds);

                    foreach (var it in listbasedionNpi)
                    {

                        string query1 = "[DrFirst].[PrcupdatePrescribeLicenseStatus]";
                        string constrEmar1 = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                        DataSet ds1 = new DataSet();
                        using (SqlConnection con = new SqlConnection(constrEmar1))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query1))
                            {
                                cmd.Connection = con;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 180;
                                cmd.Parameters.Add("@PrescriberId", SqlDbType.VarChar).Value = it.Physician_Id;

                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    sda.Fill(ds1);
                                }


                            }
                        }



                        var existingRecord = this.dbContext.PhysicianDetails.Where(p => p.Physician_Id == it.Physician_Id).FirstOrDefault();




                        var defaultCheck = (from ns in this.dbContext.NursingStations
                                            join pd in this.dbContext.PhysicianDetails on ns.DefaultPhysician_Id equals pd.Physician_Id
                                            where pd.PhysicianNPI == record.PhysicianNPI && items.Contains(ns.NurseStation_Id)
                                            select new { ns.NurseStation_Id }).Count();
                        if (defaultCheck == 0 || record.Physician_Status == 1)
                        {


                            existingRecord.PhysicianAddress1 = record.PhysicianAddress1;
                            existingRecord.PhysicianAddress2 = record.PhysicianAddress2;
                            existingRecord.PhysicianCity = record.PhysicianCity;
                            existingRecord.PhysicianCountryID = record.PhysicianCountryID;
                            existingRecord.PhysicianDEANumber = record.PhysicianDEANumber;
                            existingRecord.PhysicianFName = record.PhysicianFName;
                            existingRecord.PhysicianLName = record.PhysicianLName;
                            existingRecord.PhysicianNPI = record.PhysicianNPI;
                            existingRecord.PhysicianState = record.PhysicianState;
                            existingRecord.PhysicianZip = record.PhysicianZip;
                            existingRecord.Physician_CreatedBy = record.Physician_CreatedBy;
                            existingRecord.Physician_CreatedDate = record.Physician_CreatedDate;
                            existingRecord.Physician_Status = record.Physician_Status;
                            existingRecord.PhysicianCountryID = record.PhysicianCountryID;
                            //   existingRecord.NurseStation_Id = NsId;
                            existingRecord.Credentials = record.Credentials;
                            existingRecord.PrimarySpec = record.PrimarySpec;
                            existingRecord.SupervisingPhy = record.SupervisingPhy;
                            existingRecord.Physician_Phone = record.Physician_Phone;

                            this.dbContext.SaveChanges();
                            int PhyIdi = existingRecord.Physician_Id;




                            foreach (var lic in physcianDetails.LicensesData)
                            {

                                string query = "[DrFirst].InsertPrescribeLicense";
                                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                                DataSet ds = new DataSet();
                                using (SqlConnection con = new SqlConnection(constrEmar))
                                {
                                    con.Open();
                                    using (SqlCommand cmd = new SqlCommand(query))
                                    {
                                        cmd.Connection = con;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandTimeout = 180;
                                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
                                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
                                        cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
                                        cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyIdi;
                                        cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

                                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                        {
                                            sda.Fill(ds);
                                        }


                                    }
                                }
                            }



                            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                            {
                                Screen_Id = (int)ScreenEntity.Screens.PhysicianDetails,
                                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                                Comments = record.Physician_Id.ToString(),
                                Session_Id = 0,
                                Time = DateTime.Now,
                                UserActivity_Id = 0
                            };

                            //Insert data in License


                            _userActivityRepository.InsertUserActivityDetails(activityEntity);
                        }

                        else
                        {
                            return 3;
                        }

                    }
                }



                //inserting new nursing station those are not avilable previously
                //from front end
                string[] nsidfe = physcianDetails.NurseStations.ToString().Split(',');

                var listbasedionNpiNST1 = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == physcianDetails.OldPhysicianNPI && p.Physician_Status == 1).Select(p => p.NurseStation_Id).ToList();

                //converting intocomma sepearated
                string listbasedionNpiNST2 = string.Join(",", listbasedionNpiNST1.Select(x => x.Value));




                //then converting into array
                string[] listbasedionNpiNST = listbasedionNpiNST2.ToString().Split(',');




                //checking nurse station are not avilable in db
                var array3 = nsidfe.Except(listbasedionNpiNST).ToList();
                //getting not avilble in table 
                //var onlyInArray2 = nursestationsup.Except(listbasedionNpiNST);

                // insert new records in update 
                foreach (var ninst in array3)
                {
                    int Nid = Convert.ToInt32(ninst);

                    int? facId = this.dbContext.NursingStations.Where(p => p.NurseStation_Id == Nid && p.NurseStation_Status == 1).Select(p => p.Facility_Id).FirstOrDefault();

                    if (facId > 0)
                    {
                        record.NurseStation_Id = Nid;
                        record.Facility_Id = facId;
                        this.dbContext.PhysicianDetails.Add(record);
                        this.dbContext.SaveChanges();
                        int PhyId = record.Physician_Id;

                        foreach (var lic in physcianDetails.LicensesData)
                        {

                            string query = "[DrFirst].InsertPrescribeLicense";
                            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                            DataSet ds = new DataSet();
                            using (SqlConnection con = new SqlConnection(constrEmar))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Connection = con;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 180;
                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = lic.State;
                                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = lic.City;
                                    cmd.Parameters.Add("@Number", SqlDbType.VarChar).Value = lic.Number;
                                    cmd.Parameters.Add("@PrescriberId", SqlDbType.Int).Value = PhyId;
                                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(physcianDetails.CreatedBy);

                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                    {
                                        sda.Fill(ds);
                                    }


                                }
                            }
                        }
                    }



                }




                //}


            }
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (physcianDetails.NurseStations.Count() > 0)
                {
                    var selectedNurseStationsSave = string.Join(",", physcianDetails.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    //userRecentFacObj.Facility_Id = (int)physcianDetails.Facility_Id;
                    userRecentFacObj.Facility_Id = 0;

                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            return 1;

        }

        public List<PhysicianGridEntityNew> GetPhyscianDetailsGridDataNew(int userId)
        {

            //var records = this.dbContext.PrcGetPhysicianGridDataNew(userId).ToList();
            //var list = this.autoMapper.Map<List<PrcGetPhysicianData_Result>, List<PhysicianGridEntity>>(records.OrderBy(item => item.Facility_Name).ThenBy(item => item.NurseStation_Name).ThenBy(item => item.PhysicianName).ToList());
            //return list;

            DataTable dt = new DataTable();
            string query = "[dbo].[PrcGetPhysicianGridData_2]";
            //string query = "[Admin].[PrcGetPhysicianData_Sv]";

            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId == null ? (object)DBNull.Value : userId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }



            var physicianData = (from d in dt.AsEnumerable()
                                 select new PhysicianGridEntityNew
                                 {
                                     Facility_Name = d["Facility_Name"].ToString(),
                                     NurseStation_Name = d["NurseStation_Name"].ToString(),
                                     PhysicianNPI = d["PhysicianNPI"].ToString(),
                                     PhysicianName = d["PhysicianName"].ToString(),


                                     PhysicianCity = d["PhysicianCity"].ToString(),
                                     PhysicianState = d["PhysicianState"].ToString(),
                                     PhysicianCountry = d["PhysicianCountry"].ToString(),
                                     PhysicianZip = d["PhysicianZip"].ToString(),

                                     Physician_Status = Convert.ToInt32(d["Physician_Status"]),
                                     Facility_Status = Convert.ToInt32(d["Facility_Status"]),
                                     Credentials = d["Credentials"].ToString(),
                                     PrimarySpec = d["PrimarySpec"].ToString(),

                                     SupervisingPhy = d["SupervisingPhy"].ToString(),
                                     Physician_Phone = d["Physician_Phone"].ToString(),

                                     Facility_Id = d["Facility_Id"].ToString(),
                                     ColorFlag = Convert.ToInt32(d["ColorFlag"]),

                                 }).OrderBy(item => item.Facility_Name).ThenBy(item => item.NurseStation_Name).ThenBy(item => item.PhysicianName).ToList();

            return physicianData;



        }
        public List<PhysicianGridEntity> GetPhyscianDetails(int userId)
        {

            var records = this.dbContext.PrcGetPhysicianData(userId).ToList();
            var list = this.autoMapper.Map<List<PrcGetPhysicianData_Result>, List<PhysicianGridEntity>>(records.OrderBy(item => item.Facility_Name).ThenBy(item => item.NurseStation_Name).ThenBy(item => item.PhysicianName).ToList());
            return list;

        }
        //public PhysicianDetailsEntity GetPhyscianDetailsById(string PhyscianNPI, int FacilityId)
        //{
        //    List<int?> Nslist = new List<int?>();
        //    string nursestationId = string.Empty;
        //    var physician = this.dbContext.PhysicianDetails.Where(u => u.PhysicianNPI == PhyscianNPI && u.Facility_Id == FacilityId).OrderBy(p => p.Physician_Status).FirstOrDefault();
        //    var NurseStationIdsArray = this.dbContext.PhysicianDetails.Where(u => u.PhysicianNPI == PhyscianNPI && u.Facility_Id == FacilityId && u.Physician_Status != 2).Select(n => n.NurseStation_Id).Distinct().ToList();
        //    PhysicianDetailsEntity record = new PhysicianDetailsEntity();
        //    record.Physician_Id = physician.Physician_Id;
        //    record.PhysicianNPI = physician.PhysicianNPI;
        //    record.PhysicianLName = physician.PhysicianLName;
        //    record.PhysicianFName = physician.PhysicianFName;
        //    record.PhysicianAddress1 = physician.PhysicianAddress1;
        //    record.PhysicianAddress2 = physician.PhysicianAddress2;
        //    record.PhysicianCity = physician.PhysicianCity;
        //    record.PhysicianState = physician.PhysicianState;
        //    record.PhysicianZip = physician.PhysicianZip;
        //    record.Physician_Status = physician.Physician_Status == 2 ? 0 : physician.Physician_Status;
        //    record.Facility_Id = physician.Facility_Id;
        //    record.PhysicianCountryID = physician.PhysicianCountryID;
        //    record.Physician_CreatedBy = physician.Physician_CreatedBy;
        //    record.Physician_CreatedDate = physician.Physician_CreatedDate;
        //    record.Credentials = physician.Credentials;
        //    record.PrimarySpec = physician.PrimarySpec;
        //    record.SupervisingPhy = physician.SupervisingPhy;
        //    record.Physician_Phone = physician.Physician_Phone;
        //    record.NursestationIds = string.Join(",", NurseStationIdsArray);

        //    string query = "[DrFirst].[GetPrescribeLicenses]";
        //    string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
        //    DataSet ds = new DataSet();
        //    using (SqlConnection con = new SqlConnection(constrEmar))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand(query))
        //        {
        //            cmd.Connection = con;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 180;
        //            cmd.Parameters.Add("@PId", SqlDbType.Int).Value = record.Physician_Id;


        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                sda.Fill(ds);
        //            }


        //        }
        //    }
        //    // List<Licenses> LicensesData
        //    var emarGridData = (from d in ds.Tables[0].AsEnumerable()
        //                        select new Licenses
        //                        {

        //                            Id = Convert.ToInt32(d["Id"]),
        //                            State = Convert.ToString(d["State"]),
        //                            City = Convert.ToString(d["City"]),
        //                            Number = Convert.ToString(d["Number"])
        //                        }).ToList();
        //    record.LicensesData = emarGridData;

        //    return record;
        //}
        public PhysicianDetailsEntity GetPhyscianDetailsById(string PhyscianNPI, string FacilityId)
        {
            List<int?> Nslist = new List<int?>();
            string nursestationId = string.Empty;

            //string Fac = FacilityId;
            List<string> Fac = FacilityId.Split(',').ToList();


            // var data = this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == record.PhysicianNPI && (Fac.Any(al => p.Facility_Id.ToString().Contains(al)))).ToList();

            if (FacilityId != "" && FacilityId != null && FacilityId != "0")
            {


                var physician = this.dbContext.PhysicianDetails.Where(u => u.PhysicianNPI == PhyscianNPI && (Fac.Any(al => u.Facility_Id.ToString().Contains(al)))).OrderBy(p => p.Physician_Status).FirstOrDefault();

                var NurseStationIdsArray = this.dbContext.PhysicianDetails.Where(u => u.PhysicianNPI == PhyscianNPI && (Fac.Any(al => u.Facility_Id.ToString().Contains(al))) && u.Physician_Status != 2).Select(n => n.NurseStation_Id).Distinct().ToList();

                PhysicianDetailsEntity record = new PhysicianDetailsEntity();
                record.Physician_Id = physician.Physician_Id;
                record.PhysicianNPI = physician.PhysicianNPI;
                record.PhysicianLName = physician.PhysicianLName;
                record.PhysicianFName = physician.PhysicianFName;
                record.PhysicianAddress1 = physician.PhysicianAddress1;
                record.PhysicianAddress2 = physician.PhysicianAddress2;
                record.PhysicianCity = physician.PhysicianCity;
                record.PhysicianState = physician.PhysicianState;
                record.PhysicianZip = physician.PhysicianZip;
                record.Physician_Status = physician.Physician_Status == 2 ? 0 : physician.Physician_Status;
                record.Facility_Id = physician.Facility_Id;
                record.PhysicianCountryID = physician.PhysicianCountryID;
                record.Physician_CreatedBy = physician.Physician_CreatedBy;
                record.Physician_CreatedDate = physician.Physician_CreatedDate;
                record.Credentials = physician.Credentials;
                record.PrimarySpec = physician.PrimarySpec;
                record.SupervisingPhy = physician.SupervisingPhy;
                record.Physician_Phone = physician.Physician_Phone;
                record.NursestationIds = string.Join(",", NurseStationIdsArray);
                record.FacId = FacilityId;

                string query = "[DrFirst].[GetPrescribeLicenses]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@PId", SqlDbType.Int).Value = record.Physician_Id;


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }


                    }
                }
                // List<Licenses> LicensesData
                var emarGridData = (from d in ds.Tables[0].AsEnumerable()
                                    select new Licenses
                                    {

                                        Id = Convert.ToInt32(d["Id"]),
                                        State = Convert.ToString(d["State"]),
                                        City = Convert.ToString(d["City"]),
                                        Number = Convert.ToString(d["Number"])
                                    }).ToList();
                record.LicensesData = emarGridData;

                return record;
            }
            else
            {
                var physician = this.dbContext.PhysicianDetails.Where(u => u.PhysicianNPI == PhyscianNPI).OrderBy(p => p.Physician_Status).FirstOrDefault();

                var NurseStationIdsArray = this.dbContext.PhysicianDetails.Where(u => u.PhysicianNPI == PhyscianNPI && u.Physician_Status != 2).Select(n => n.NurseStation_Id).Distinct().ToList();

                PhysicianDetailsEntity record = new PhysicianDetailsEntity();
                record.Physician_Id = physician.Physician_Id;
                record.PhysicianNPI = physician.PhysicianNPI;
                record.PhysicianLName = physician.PhysicianLName;
                record.PhysicianFName = physician.PhysicianFName;
                record.PhysicianAddress1 = physician.PhysicianAddress1;
                record.PhysicianAddress2 = physician.PhysicianAddress2;
                record.PhysicianCity = physician.PhysicianCity;
                record.PhysicianState = physician.PhysicianState;
                record.PhysicianZip = physician.PhysicianZip;
                record.Physician_Status = physician.Physician_Status == 2 ? 0 : physician.Physician_Status;
                record.Facility_Id = physician.Facility_Id;
                record.PhysicianCountryID = physician.PhysicianCountryID;
                record.Physician_CreatedBy = physician.Physician_CreatedBy;
                record.Physician_CreatedDate = physician.Physician_CreatedDate;
                record.Credentials = physician.Credentials;
                record.PrimarySpec = physician.PrimarySpec;
                record.SupervisingPhy = physician.SupervisingPhy;
                record.Physician_Phone = physician.Physician_Phone;
                record.NursestationIds = string.Join(",", NurseStationIdsArray);
                record.FacId = FacilityId;

                string query = "[DrFirst].[GetPrescribeLicenses]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@PId", SqlDbType.Int).Value = record.Physician_Id;


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }


                    }
                }
                // List<Licenses> LicensesData
                var emarGridData = (from d in ds.Tables[0].AsEnumerable()
                                    select new Licenses
                                    {

                                        Id = Convert.ToInt32(d["Id"]),
                                        State = Convert.ToString(d["State"]),
                                        City = Convert.ToString(d["City"]),
                                        Number = Convert.ToString(d["Number"])
                                    }).ToList();
                record.LicensesData = emarGridData;

                return record;
            }

        }
        public string CheckResidentUniqueId(int patientId)
        {
            int nurseSationId = this.GetNurseStationByPId(patientId);
            var facilityId = this.dbContext.NursingStations.Where(c => c.NurseStation_Id == nurseSationId).Select(c => c.Facility_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(c => c.Facility_Id == facilityId).Select(c => c.Company_Id).FirstOrDefault();
            string uniqueId = this.dbContext.Companies.Where(co => co.Company_Id == companyId).Select(co => co.Company_UniqueId).FirstOrDefault();
            return uniqueId;
        }
        public int InsertUpdatePatientOnLeave(OnLeaveEntity entity)
        {
            if (entity.IsOnLeave == 0)
            {
                var record = this.autoMapper.Map<OnLeaveEntity, OnLeave>(entity);
                var visitInfo = this.dbContext.VisitInfoes.Find(entity.PVisit_Id);
                if (visitInfo != null)
                {
                    if (record.LeaveFrom <= DateTime.Now)
                        visitInfo.PVisit_Status = 3;
                    record.OnLeave_Status = 1;
                    record.OnLeave_CreatedOn = DateTime.Now;
                    this.dbContext.OnLeaves.Add(record);
                    this.dbContext.SaveChanges();
                    return 1;
                }
                return 0;
            }
            else
            {
                var record = this.dbContext.OnLeaves.Where(item => item.PVisit_Id == entity.PVisit_Id).OrderByDescending(item => item.Onleave_Id).FirstOrDefault();
                if (record != null)
                {
                    record.LeaveTo = DateTime.Now;
                    var visitInfo = this.dbContext.VisitInfoes.Find(entity.PVisit_Id);
                    if (visitInfo != null)
                        visitInfo.PVisit_Status = 1;
                    this.dbContext.SaveChanges();
                    return 1;
                }
                return 0;
            }
        }
        public int UpdatePhysiciansStatus(List<PhysicianDetailsEntity> data)
        {
            foreach (var item in data)
            {
                var records = this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == item.PhysicianNPI).ToList();
                foreach (var items in records)
                {
                    //var nsId = (int)items.NurseStation_Id;
                    // var updateRecord = this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == items.PhysicianNPI && ph.NurseStation_Id == nsId).FirstOrDefault();
                    var updateRecord = this.dbContext.PhysicianDetails.Where(ph => ph.Physician_Id == items.Physician_Id).FirstOrDefault();

                    if (updateRecord != null)
                    {
                        updateRecord.Physician_Status = items.Physician_Status == 1 ? 0 : 1;
                        updateRecord.Physician_CreatedBy = item.Physician_CreatedBy;
                        updateRecord.Physician_CreatedDate = item.Physician_CreatedDate;
                        this.dbContext.SaveChanges();
                    }
                }
            }
            return 1;
        }
        public List<ResidentDropEntity> GetResidentDetails(Nullable<int> status, string nursestationId, int residentcount)
        {
            string[] nursestations = nursestationId.Split(',');
            if (status == 1)
            {
                var records = (from rr in this.dbContext.Demographics
                               join vi in this.dbContext.VisitInfoes on rr.Patient_Id equals vi.Patient_Id
                               where (vi.PVisit_Status == 1 || vi.PVisit_Status == 3) && nursestations.Contains(vi.NursingStationId.ToString())
                               select new ResidentDropEntity
                               {
                                   Patient_Id = rr.Patient_Id,
                                   PatientName = rr.PatientLastName + ", " + rr.PatientFirstName + " " + rr.PatientMiddleInitial
                               }).Distinct().OrderBy(item => item.PatientName).ToList();
                return records;
            }
            else if (status == 2)
            {
                var records = (from rr in this.dbContext.Demographics
                               join vi in this.dbContext.VisitInfoes on rr.Patient_Id equals vi.Patient_Id
                               where vi.PVisit_Status == 2 && nursestations.Contains(vi.NursingStationId.ToString())
                               select new ResidentDropEntity
                               {
                                   Patient_Id = rr.Patient_Id,
                                   PatientName = rr.PatientLastName + ", " + rr.PatientFirstName + " " + rr.PatientMiddleInitial
                               }).Distinct().OrderBy(item => item.PatientName).ToList();
                return records;
            }
            else if (status == null)
            {
                var records = (from rr in this.dbContext.Demographics
                               join vi in this.dbContext.VisitInfoes on rr.Patient_Id equals vi.Patient_Id
                               where nursestations.Contains(vi.NursingStationId.ToString())
                               select new ResidentDropEntity
                               {
                                   Patient_Id = rr.Patient_Id,
                                   PatientName = rr.PatientLastName + ", " + rr.PatientFirstName + " " + rr.PatientMiddleInitial
                               }).Distinct().OrderBy(item => item.PatientName).ToList();
                return records;
            }
            return null;
        }
        public int CheckResidentInternalId(string InternalId)
        {
            if (InternalId != "")
            {
                var records = this.dbContext.Demographics.Where(s => s.ExternalPatientId == InternalId).FirstOrDefault();
                if (records != null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 3;
        }
        public int InsertMergeStatus(PostMergeDetails objPost)
        {
            Demographic residentDetails = this.dbContext.Demographics.Where(s => s.Patient_Id == objPost.patientId).FirstOrDefault();
            Demographic mergeResidentDetails = this.dbContext.Demographics.Where(p => p.Patient_Id == objPost.mergepatientId).FirstOrDefault();
            var PatientVisits = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == objPost.patientId).ToList();
            var mergeVisits = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == objPost.mergepatientId).ToList();
            var mergeVisitsLatest = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == objPost.mergepatientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            if (residentDetails != null && mergeResidentDetails != null)
            {
                if (objPost.DetailsUpdate == 1)
                {
                    residentDetails.AliasName = objPost.aliasName;
                    residentDetails.MergeID = objPost.mergepatientId;
                    residentDetails.MergeDate = DateTime.Now;
                    residentDetails.Patient_Status = 10;
                    this.dbContext.SaveChanges();
                    mergeResidentDetails.AliasName = objPost.aliasName;
                    this.dbContext.SaveChanges();
                    //residentDetails.PatientAddress1 = mergeResidentDetails.PatientAddress1;
                    //residentDetails.PatientAddress2 = mergeResidentDetails.PatientAddress2;
                    //residentDetails.PatientCity = mergeResidentDetails.PatientCity;
                    //residentDetails.PatientState = mergeResidentDetails.PatientState;
                    if (PatientVisits.Count() > 0)
                    {
                        PatientVisits.ForEach(va => va.PVisit_Status = 10);
                        this.dbContext.SaveChanges();
                        //PatientVisits.AdmitDate = mergeVisitsLatest.AdmitDate;
                        //PatientVisits.DischargeDate = mergeVisitsLatest.DischargeDate;
                    }
                    this.dbContext.SaveChanges();
                    if (objPost.AllergyMerge == 1)
                    {
                        var resAllergy = this.dbContext.AllergyInfoes.Where(al => al.Patient_Id == objPost.patientId).ToList();
                        for (int i = 0; i < resAllergy.Count(); i++)
                        {
                            resAllergy[i].OldPatient_Id = objPost.patientId;
                            resAllergy[i].Patient_Id = objPost.mergepatientId;
                        }
                        this.dbContext.SaveChanges();
                    }
                    if (objPost.DiagnosisMerge == 1)
                    {
                        var resDiagnosis = this.dbContext.DiagnosisInfoes.Where(di => di.Patient_Id == objPost.patientId).ToList();
                        for (int j = 0; j < resDiagnosis.Count(); j++)
                        {
                            resDiagnosis[j].OldPatient_Id = objPost.patientId;
                            resDiagnosis[j].Patient_Id = objPost.mergepatientId;
                        }
                        this.dbContext.SaveChanges();
                    }
                    var resOrders = this.dbContext.CommonOrderInfoes.Where(cm => cm.Patient_Id == objPost.patientId).ToList();
                    if (resOrders.Count() > 0)
                    {
                        for (int k = 0; k < resOrders.Count(); k++)
                        {
                            resOrders[k].OldPatient_Id = objPost.patientId;
                            resOrders[k].Patient_Id = objPost.mergepatientId;
                        }
                        this.dbContext.SaveChanges();
                    }
                }
                if (objPost.DetailsUpdate == 0)
                {
                    mergeResidentDetails.AliasName = objPost.aliasName;
                    mergeResidentDetails.MergeID = objPost.patientId;
                    mergeResidentDetails.MergeDate = DateTime.Now;
                    mergeResidentDetails.Patient_Status = 10;
                    this.dbContext.SaveChanges();
                    residentDetails.AliasName = objPost.aliasName;
                    if (mergeVisits.Count() > 0)
                    {
                        mergeVisits.ForEach(va => va.PVisit_Status = 10);
                        this.dbContext.SaveChanges();
                    }
                    if (objPost.AllergyMerge == 1)
                    {
                        var resAllergy = this.dbContext.AllergyInfoes.Where(al => al.Patient_Id == objPost.mergepatientId).ToList();
                        for (int i = 0; i < resAllergy.Count(); i++)
                        {
                            resAllergy[i].OldPatient_Id = objPost.mergepatientId;
                            resAllergy[i].Patient_Id = objPost.patientId;
                        }
                        this.dbContext.SaveChanges();
                    }
                    if (objPost.DiagnosisMerge == 1)
                    {
                        var resDiagnosis = this.dbContext.DiagnosisInfoes.Where(di => di.Patient_Id == objPost.mergepatientId).ToList();
                        for (int j = 0; j < resDiagnosis.Count(); j++)
                        {
                            resDiagnosis[j].OldPatient_Id = objPost.mergepatientId;
                            resDiagnosis[j].Patient_Id = objPost.patientId;
                        }
                        this.dbContext.SaveChanges();
                    }
                    var resOrders = this.dbContext.CommonOrderInfoes.Where(cm => cm.Patient_Id == objPost.mergepatientId).ToList();
                    if (resOrders.Count() > 0)
                    {
                        for (int k = 0; k < resOrders.Count(); k++)
                        {
                            resOrders[k].OldPatient_Id = objPost.mergepatientId;
                            resOrders[k].Patient_Id = objPost.patientId;
                        }
                        this.dbContext.SaveChanges();
                    }
                }
            }
            return 1;
        }
        public ResidentMergeEntity GetMergeDetails(int PatientID, int MergePatientID)
        {
            var facilityId = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == PatientID).OrderByDescending(vs => vs.AdmitDate).Select(vs => vs.FacilityId).FirstOrDefault();
            var residentDetails = (from dm in this.dbContext.Demographics
                                   where dm.Patient_Id == PatientID
                                   select new ResidentMergeDetails
                                   {
                                       PatientId = dm.Patient_Id,
                                       ResidentName = dm.PatientLastName + ", " + dm.PatientFirstName + " " + dm.PatientMiddleInitial,
                                       DOB = dm.DOB,
                                       Gender = dm.AdministrativeSex == "M" ? "Male" : "Female",
                                       Addr1 = dm.PatientAddress1,
                                       Addr2 = dm.PatientAddress2,
                                       City = dm.PatientCity,
                                       State = dm.PatientState,
                                       ExternalFacPatientId = dm.ExternalFacPatientId,
                                       ExternalPatientId = dm.ExternalPatientId == null ? dm.PatientMRNumber : dm.ExternalPatientId,
                                       PatientMRNumber = dm.PatientMRNumber,
                                       Facilityname = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Facility_Name).FirstOrDefault(),
                                       AdmitDate = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == dm.Patient_Id).OrderByDescending(vs => vs.AdmitDate).Select(vs => vs.AdmitDate).FirstOrDefault()
                                   }).FirstOrDefault();
            var mergeFacilityId = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == MergePatientID).OrderByDescending(vs => vs.AdmitDate).Select(vs => vs.FacilityId).FirstOrDefault();
            var mergeResidentDetails = (from dm in this.dbContext.Demographics
                                        where dm.Patient_Id == MergePatientID
                                        select new ResidentMergeDetails
                                        {
                                            PatientId = dm.Patient_Id,
                                            ResidentName = dm.PatientLastName + ", " + dm.PatientFirstName + " " + dm.PatientMiddleInitial,
                                            DOB = dm.DOB,
                                            Gender = dm.AdministrativeSex == "M" ? "Male" : "Female",
                                            Addr1 = dm.PatientAddress1,
                                            Addr2 = dm.PatientAddress2,
                                            City = dm.PatientCity,
                                            State = dm.PatientState,
                                            ExternalFacPatientId = dm.ExternalFacPatientId,
                                            ExternalPatientId = dm.ExternalPatientId == null ? dm.PatientMRNumber : dm.ExternalPatientId,
                                            PatientMRNumber = dm.PatientMRNumber,
                                            Facilityname = this.dbContext.Facilities.Where(f => f.Facility_Id == mergeFacilityId).Select(f => f.Facility_Name).FirstOrDefault(),
                                            AdmitDate = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == dm.Patient_Id).OrderByDescending(vs => vs.AdmitDate).Select(vs => vs.AdmitDate).FirstOrDefault()
                                        }).FirstOrDefault();
            var residentVisit = (from vs in this.dbContext.VisitInfoes
                                 where vs.Patient_Id == PatientID
                                 select new ResidentMergeVisitInfo
                                 {
                                     AdmitDate = vs.AdmitDate,
                                     DischargeDate = vs.DischargeDate
                                 }).ToList();
            var mergeresidentVisit = (from vs in this.dbContext.VisitInfoes
                                      where vs.Patient_Id == MergePatientID
                                      select new ResidentMergeVisitInfo
                                      {
                                          AdmitDate = vs.AdmitDate,
                                          DischargeDate = vs.DischargeDate
                                      }).ToList();
            var residentAllergies = (from al in this.dbContext.AllergyInfoes
                                     where al.Patient_Id == PatientID
                                     select new ResidentMergeAllergies
                                     {
                                         Allergy = al.ClassDrug_Name,
                                         Reaction = al.AllergyReactionCode
                                     }).ToList();
            var mergeResidentAllergies = (from al in this.dbContext.AllergyInfoes
                                          where al.Patient_Id == MergePatientID
                                          select new ResidentMergeAllergies
                                          {
                                              Allergy = al.ClassDrug_Name,
                                              Reaction = al.AllergyReactionCode
                                          }).ToList();
            var residentDiagnosis = (from di in this.dbContext.DiagnosisInfoes
                                     join icd in this.dbContext.ICD10 on di.ICD10_Id equals icd.ICD10_Id
                                     where di.Patient_Id == PatientID
                                     select new ResidentMergeDiagnosis
                                     {
                                         DiagnosisDesc = icd.ICD10_Description
                                     }).ToList();
            var mergeResidentDiagnosis = (from di in this.dbContext.DiagnosisInfoes
                                          join icd in this.dbContext.ICD10 on di.ICD10_Id equals icd.ICD10_Id
                                          where di.Patient_Id == MergePatientID
                                          select new ResidentMergeDiagnosis
                                          {
                                              DiagnosisDesc = icd.ICD10_Description
                                          }).ToList();
            ResidentMergeEntity objMerge = new ResidentMergeEntity();
            if (residentDetails != null && mergeResidentDetails != null)
            {
                objMerge.Merge1 = residentDetails;
                objMerge.Merge2 = mergeResidentDetails;
            }
            objMerge.Visit1 = residentVisit;
            objMerge.Visit2 = mergeresidentVisit;
            //if(residentAllergies.Count()>0)
            {
                objMerge.Allergy1 = residentAllergies;
            }
            //if (mergeResidentAllergies.Count() > 0)
            {
                objMerge.Allergy2 = mergeResidentAllergies;
            }
            //if(residentDiagnosis.Count()>0)
            {
                objMerge.Diagnosis1 = residentDiagnosis;
            }
            //if (mergeResidentDiagnosis.Count() > 0)
            {
                objMerge.Diagnosis2 = mergeResidentDiagnosis;
            }
            return objMerge;
        }
        public ResidentMergeDetails GetResidentMergeDetailsByPatientID(int patientId)
        {
            var facilityId = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == patientId).OrderByDescending(vs => vs.AdmitDate).Select(vs => vs.FacilityId).FirstOrDefault();
            var mergeResidentDetails = (from dm in this.dbContext.Demographics
                                        where dm.Patient_Id == patientId
                                        select new ResidentMergeDetails
                                        {
                                            PatientId = dm.Patient_Id,
                                            ResidentName = dm.PatientLastName + ", " + dm.PatientFirstName + " " + dm.PatientMiddleInitial,
                                            DOB = dm.DOB,
                                            Gender = dm.AdministrativeSex == "M" ? "Male" : "Female",
                                            Addr1 = dm.PatientAddress1,
                                            Addr2 = dm.PatientAddress2,
                                            City = dm.PatientCity,
                                            State = dm.PatientState,
                                            ExternalFacPatientId = dm.ExternalFacPatientId,
                                            ExternalPatientId = dm.ExternalPatientId == null ? dm.PatientMRNumber : dm.ExternalPatientId,
                                            PatientMRNumber = dm.PatientMRNumber,
                                            Facilityname = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Facility_Name).FirstOrDefault(),
                                            AdmitDate = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == dm.Patient_Id).OrderByDescending(vs => vs.AdmitDate).Select(vs => vs.AdmitDate).FirstOrDefault()
                                        }).FirstOrDefault();
            return mergeResidentDetails;
        }
        public int GetHl7PendingCount()
        {
            var result = this.dbContext.FileAckInformations.Where(fa => fa.File_Id == null).ToList().Count();
            return result;
        }
        public IList GetComputerNameDropData()
        {
            var records = (from PK in this.dbContext.ProcessKeyMasters
                           select new
                           {
                               ProcessID = PK.ProcessID,
                               ComputerName = PK.ComputerName
                           }).ToList();
            return records;
        }
        public string GetResidentActiveStatus(string mrNumber)
        {
            var result = (from dm in this.dbContext.Demographics
                          join vs in this.dbContext.VisitInfoes on dm.Patient_Id equals vs.Patient_Id
                          where dm.ExternalPatientId == mrNumber
                          select new
                          {
                              DischargeDate = vs.DischargeDate == null ? "Active" : "Discharged"
                          }).FirstOrDefault();
            if (result != null)
                return result.DischargeDate;
            else
                return "Resident Data Not Available";
        }
        public string GetUserProcessKeyByID(int userId)
        {
            var record = (from us in this.dbContext.Users
                          join pc in this.dbContext.ProcessKeyMasters on us.ProcessKey equals pc.ProcessID
                          where us.User_Id == userId
                          select new
                          {
                              pc.ProcessKey
                          }).FirstOrDefault();
            if (record != null)
                return record.ProcessKey;
            else
                return null;
        }
        public List<ResidentsEntity> GetCertifyOrderResidentGridData(string nursingStations, int userId, string phyNpi)
        {
            if (nursingStations == "null")
            {
                nursingStations = null;
            }
            if (phyNpi == "null")
            {
                phyNpi = null;
            }
            var records = (from pg in this.dbContext.PrcGetCertifyOrderResidentGridData(nursingStations, userId, phyNpi)
                           select new ResidentsEntity
                           {
                               PatientName = pg.ResidentName,
                               ImgPath = pg.ImageLocation != null ? "Yes" : "No",
                               // ImageLocation = CheckResImageinBiometric(this.dbContext.Demographics.Where(p=>p.Patient_Id==pg.Patient_Id).Select(p=>p.PatientMRNumber).FirstOrDefault()) == "" ? ConvertImage(pg.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(this.dbContext.Demographics.Where(p => p.Patient_Id == pg.Patient_Id).Select(p => p.PatientMRNumber).FirstOrDefault())),
                               PatientGender = pg.Gender,
                               Patient_Id = pg.Patient_Id,
                               PatientDOB = pg.DOB.ToString(),
                               NurseStationName = pg.NurseStation_Name
                           }).OrderBy(item => item.PatientName).ToList();
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int loginUserId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                var stations = nursingStations.Split(',');
                if (stations.Count() > 0)
                {
                    int nurseStationNewID = Convert.ToInt32(stations[0]);
                    var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();
                    var selectedNurseStationsSave = string.Join(",", nursingStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = loginUserId;
                    userRecentFacObj.Facility_Id = (int)facilityId;
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            return records;
        }
        public IList GetCertifyOrderGridData(int patientId, string phyNpi)
        {
            int loginUserId = 0;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                loginUserId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
            }
            if (phyNpi == "null")
            {
                phyNpi = null;
            }
            var records = this.dbContext.PrcGetCertifyOrderGridData(patientId, loginUserId, phyNpi).ToList();
            return records;
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
        public List<ResidentDropEntity> GetResidentsListByNSId(int nurseStationId)
        {
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();

            RecentFacEntity userRecentFacObj = new RecentFacEntity()
            {
                User_Id = 0,
                Facility_Id = (int)facilityId,
                NurseStation_Id = nurseStationId.ToString()
            };
            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            return (from d in this.dbContext.Demographics
                    join v in this.dbContext.VisitInfoes on d.Patient_Id equals v.Patient_Id
                    where v.NursingStationId == nurseStationId && v.PVisit_Status != 10
                    //&& d.MergeID==null
                    select new ResidentDropEntity()
                    {
                        Patient_Id = d.Patient_Id,
                        PatientName = d.PatientLastName + ", " + d.PatientFirstName + " " + (d.PatientMiddleInitial == null ? "" : d.PatientMiddleInitial),
                        PVisit_Status = v.PVisit_Status
                    }).Distinct().OrderBy(item => item.PatientName).ToList();
        }
        public List<ResidentsEntity> GetProfileCertifyOrderResidentGridData(string nursingStations)
        {
            var records = (from pg in this.dbContext.PrcGetProfieOrderResidentGridData(nursingStations)
                           select new ResidentsEntity
                           {
                               PatientName = pg.ResidentName,
                               ImgPath = pg.ImageLocation != null ? "Yes" : "No",
                               // ImageLocation = CheckResImageinBiometric(this.dbContext.Demographics.Where(p => p.Patient_Id == pg.Patient_Id).Select(p => p.PatientMRNumber).FirstOrDefault()) == "" ? ConvertImage(pg.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(this.dbContext.Demographics.Where(p => p.Patient_Id == pg.Patient_Id).Select(p => p.PatientMRNumber).FirstOrDefault())),
                               PatientGender = pg.Gender,
                               Patient_Id = pg.Patient_Id,
                               PatientDOB = pg.DOB.ToString(),
                               NurseStationName = pg.NurseStation_Name
                           }).OrderBy(item => item.PatientName).ToList();
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int loginUserId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                var stations = nursingStations.Split(',');
                if (stations.Count() > 0)
                {
                    int nurseStationNewID = Convert.ToInt32(stations[0]);
                    var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();
                    var selectedNurseStationsSave = string.Join(",", nursingStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = loginUserId;
                    userRecentFacObj.Facility_Id = (int)facilityId;
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            return records;
        }
        public IList GetProfileCertifyOrderGridData(int patientId)
        {
            var records = this.dbContext.PrcGetProfileOrderGridData(patientId).ToList();
            return records;
        }
        public string GetDefaultPhysicianNursestation(string PhysicianNPI)
        {
            var records = (from ns in this.dbContext.NursingStations
                           join pd in this.dbContext.PhysicianDetails on ns.DefaultPhysician_Id equals pd.Physician_Id
                           where pd.PhysicianNPI == PhysicianNPI
                           select new
                           {
                               NurseStation = ns.NurseStation_Name
                           }).FirstOrDefault();
            if (records != null)
                return records.NurseStation;
            else
                return "";
        }

        public int UpdatePregnecyFeeding(UpdatePregnecyFeedingEntity Role)
        {

            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);

            if (Role.Type == 1)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
                using (SqlCommand cmd = new SqlCommand("[Patient].PrcUpdatePatientPrgenancy", conn))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 360;
                    adapt.SelectCommand.Parameters.Add(new SqlParameter("@patientid", SqlDbType.BigInt));
                    adapt.SelectCommand.Parameters["@patientid"].Value = Role.PatientID;
                    adapt.SelectCommand.Parameters.Add(new SqlParameter("@prgenancyid", SqlDbType.Int));
                    adapt.SelectCommand.Parameters["@prgenancyid"].Value = Role.IScheck;
                    adapt.SelectCommand.Parameters.Add(new SqlParameter("@Patient_CreatedBy", SqlDbType.Int));
                    adapt.SelectCommand.Parameters["@Patient_CreatedBy"].Value = userId;
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return 1;

                }

            }
            else
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
                using (SqlCommand cmd = new SqlCommand("[Patient].PrcUpdatePatientBreastFeeding", conn))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 360;
                    adapt.SelectCommand.Parameters.Add(new SqlParameter("@patientid", SqlDbType.BigInt));
                    adapt.SelectCommand.Parameters["@patientid"].Value = Role.PatientID;
                    adapt.SelectCommand.Parameters.Add(new SqlParameter("@BreastFeeding", SqlDbType.Int));
                    adapt.SelectCommand.Parameters["@BreastFeeding"].Value = Role.IScheck;
                    adapt.SelectCommand.Parameters.Add(new SqlParameter("@Patient_CreatedBy", SqlDbType.Int));
                    adapt.SelectCommand.Parameters["@Patient_CreatedBy"].Value = userId;
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return 1;

                }

            }
        }
    }
}

