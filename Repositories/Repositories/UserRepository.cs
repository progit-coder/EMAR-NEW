using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;
using System.Web;
using System.Security.Claims;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace LTCPro.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLHelper dbHelper;
        string storedprocedure = "";
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public UserRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, SQLHelper _dbContext)
        {
            this.dbHelper = _dbContext;
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }
        public IList GetUsersMasterGrid(int UserId)


        {
            //var records = this.dbContext.PrcGetUserRoleConfigData(UserId).ToList();
            //var list = this.autoMapper.Map<List<PrcGetUserRoleConfigData_Result>, List<UserRoleFacilityConfigGridEntity>>(records);
            //return list.OrderBy(item => item.DisplayName).ThenBy(item => item.Company_Facility_Nursestation).ToList();

            storedprocedure = "[Admin].[PrcGetUserRoleConfigData]";
            var parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@UserId", UserId);

            DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);

            var records = (from d in dt.AsEnumerable()
                           select new
                           {
                               User_Id = Convert.ToInt32(d["User_Id"]),
                               RoleName = d["RoleName"].ToString(),
                               UserName = d["UserName"].ToString(),
                               DisplayName = d["DisplayName"].ToString(),
                               EmailId = d["EmailId"].ToString(),
                               PhoneNumber = (d["PhoneNumber"]).ToString(),
                               User_Status = Convert.ToInt32(d["User_Status"]),
                               Company_Facility_Nursestation = d["Company-Facility-Nursestation"].ToString(),
                               User_Lock = Convert.ToInt32(d["User_Lock"]),
                               Role_Status = Convert.ToInt32(d["Role_Status"]),
                               User_ShortName = d["User_ShortName"].ToString(),                               
                           }).ToList();

            return records.OrderBy(item => item.DisplayName).ThenBy(item => item.Company_Facility_Nursestation).ToList();

        }

        public List<UserDropEntity> GetUserDropData()
        {
            return (from u in this.dbContext.Users
                    where u.User_Status == 1
                    select new UserDropEntity
                    {
                        User_Id = u.User_Id,
                        User_DisplayName = u.User_DisplayName
                    }).OrderBy(item => item.User_DisplayName).ToList();
        }

        public int InsertUpdateUserMaster(UserEntity entity)
        {
            if (entity.User_Id == 0 ? this.dbContext.Users.Any(e => e.UserName == entity.UserName) : this.dbContext.Users.Any(e => e.UserName == entity.UserName && e.User_Id != entity.User_Id))
            {
                //UserName already exisits
                return 2;
            }
            else if (entity.User_Id == 0 ? this.dbContext.Users.Any(e => e.User_Email == entity.User_Email) : this.dbContext.Users.Any(e => e.User_Email == entity.User_Email && e.User_Id != entity.User_Id))
            {
                //Email Id already exisits
                return 3;
            }
            else if (entity.User_Id == 0 ? this.dbContext.Users.Any(e => e.User_DisplayName == entity.User_DisplayName) : this.dbContext.Users.Any(e => e.User_DisplayName == entity.User_DisplayName && e.User_Id != entity.User_Id))
            {
                //Display Name already exisits
                return 4;
            }
            else if (entity.PhysicianNPI != null && entity.PhysicianNPI != "" && this.dbContext.PhysicianDetails.Where(ph => ph.PhysicianNPI == entity.PhysicianNPI).FirstOrDefault() == null)
            {
                //Physician NPI not exisits
                return 5;
            }
            else
            {
                int userSuffix = Convert.ToInt32(entity.User_Suffix);
                int result = this.dbContext.PrcInsertUpdateUser(entity.User_Id, userSuffix, entity.User_Lname, entity.User_Fname, entity.User_Mname, entity.UserName, entity.Password, entity.User_DisplayName, entity.User_Email, entity.User_Phone, entity.StkReportReq, entity.User_Status, entity.User_CreatedBy, entity.NewUserFlag, entity.RoleId, entity.Facilities, entity.NurseStations, entity.ProcessKey, entity.PhysicianNPI, entity.PastDueAlertFlag);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Users,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = entity.User_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }
        public UserDetailsCustomEntity GetUserDetailsByUserId(int userId)
        {
            return (from us in this.dbContext.Users
                    where us.User_Id == userId && us.User_Status == 1
                    select new UserDetailsCustomEntity
                    {
                        User_Id = userId,
                        UserName = us.UserName,
                        Password = us.Password
                    }).FirstOrDefault();
        }
        public UserEntity GetUserDetailsByID(int userId)
        {
            int loginUser = userId;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            //if (claimsIdentity.FindFirst("UserId").Value != "")
            //{
            //    loginUser = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
            //}
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == loginUser && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> loginUserFacilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> loginUserNstationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();
            List<int?> Nslist = new List<int?>();
            List<string> NsCodelist = new List<string>();
            List<UserFacilityNurseStationEntity> userFacNurseList = new List<UserFacilityNurseStationEntity>();
            string nursestationId = string.Empty;
            string nursestationCode = string.Empty;
            var userData = this.dbContext.Users.Where(u => u.User_Id == userId).FirstOrDefault();
            var ProcessKeydata = (from a in this.dbContext.Users
                                  join b in this.dbContext.ProcessKeyMasters on a.ProcessKey equals b.ProcessID
                                  where a.User_Id == userId
                                  select new
                                  {
                                      ProcessID = a.ProcessKey,
                                      Computername = b.ComputerName
                                  }).FirstOrDefault();
            var data = (from us in this.dbContext.Users
                        join urf in this.dbContext.UserRoleFacilityConfigs on us.User_Id equals urf.User_Id
                        where us.User_Id == userId
                        select new
                        {
                            User = us,
                            UserRoleFacility = urf,
                        }).ToList();
            var FacilityIdsArray = data.Select(n => n.UserRoleFacility.Facility_id).Distinct().ToArray();
            int roleId = data.Select(r => r.UserRoleFacility.Role_ID).FirstOrDefault();
            foreach (var fac in FacilityIdsArray)
            {
                Nslist = new List<int?>();
                NsCodelist = new List<string>();
                nursestationId = string.Empty;
                nursestationCode = string.Empty;
                int FacId = (int)fac;
                var FacName = this.dbContext.Facilities.Where(f => f.Facility_Id == FacId && f.Facility_Status == 1 && loginUserFacilityIds.Contains(f.Facility_Id)).FirstOrDefault();
                if (FacName != null)
                {
                    var NsIdsArray = data.Where(n => n.UserRoleFacility.Facility_id == FacId && n.UserRoleFacility.User_Id == userId).Select(n => n.UserRoleFacility.NurseStation_Id).ToArray();
                    foreach (var items in NsIdsArray)
                    {
                        int NsId = (int)items;
                        var NsIdCheck = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == NsId && n.NurseStation_Status == 1 && loginUserNstationIds.Contains(n.NurseStation_Id)).FirstOrDefault();
                        if (NsIdCheck != null && NsIdCheck.NurseStation_Id != 0)
                        {
                            Nslist.Add(items);
                            NsCodelist.Add(NsIdCheck.NurseStation_Name);
                        }
                    }
                    if (Nslist.Count() > 0 && NsCodelist.Count > 0)
                    {
                        NsCodelist = NsCodelist.OrderBy(s => s).ToList();
                        nursestationId = string.Join(",", Nslist);
                        nursestationCode = string.Join(", ", NsCodelist);
                        UserFacilityNurseStationEntity userFacObj = new UserFacilityNurseStationEntity();
                        userFacObj.FacilityId = FacId;
                        userFacObj.FacilityName = FacName.Facility_Name;
                        userFacObj.NurseStationIds = nursestationId;
                        userFacObj.NurseStatioNames = nursestationCode;
                        userFacNurseList.Add(userFacObj);
                    }
                }
            }
            UserEntity record = new UserEntity();
            record.User_Id = userData.User_Id;
            record.User_Suffix = userData.User_Suffix;
            record.User_Fname = userData.User_Fname;
            record.User_Lname = userData.User_Lname;
            record.User_Mname = userData.User_Mname;
            record.User_Phone = userData.User_Phone;
            record.User_DisplayName = userData.User_DisplayName;
            record.User_Email = userData.User_Email;
            record.UserName = userData.UserName;
            record.Password = userData.Password;
            record.StkReportReq = userData.StkReportReq;
            record.NewUserFlag = userData.NewUserFlag;
            record.User_Status = userData.User_Status;
            record.RoleId = roleId;
            record.UserFacilityNurseList = userFacNurseList.OrderBy(item => item.FacilityName).ToList();
            record.ProcessKey = userData.ProcessKey;
            record.PhysicianNPI = userData.PhysicianNPI;
            record.PastDueAlertFlag = userData.PastDueAlertFlag;
            if (ProcessKeydata != null)
                record.ComputerName = ProcessKeydata.Computername;
            return record;
        }
        public int ValidateUser(string userName, string password)
        {
            try
            {
                var user = this.dbContext.Users.Where(u => u.UserName == userName && u.User_Lock == 1).FirstOrDefault();
                if (user != null)
                {
                    return 2;

                }
                user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
                if (user != null && user.Password == password)
                {
                    if (user.User_Status == 0)
                    {
                        return 7;
                    }
                    else
                    {
                        var role = (from r in this.dbContext.UserRoleFacilityConfigs
                                    where r.User_Id == user.User_Id && r.UserRole_Status == 1 && r.NurseStation_Id != null
                                    select r).FirstOrDefault();
                        if (role != null)
                        {
                            if (user.User_PwdCount != 0)
                            {
                                user.User_PwdCount = 0;
                                this.dbContext.SaveChanges();
                            }
                            return 1;
                        }
                        else
                            //No Role or NS mapped
                            return 5;
                    }
                }
                else
                {
                    user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    if (user != null && user.Password != password)
                    {
                        if (user.User_Status == 0)
                        {
                            return 7;
                        }
                        else
                        {
                            if (user.User_Lock == 0 && user.User_PwdCount < 3)
                            {
                                user.User_PwdCount = user.User_PwdCount + 1;
                                if (user.User_PwdCount == 3)
                                    user.User_Lock = 1;
                                this.dbContext.SaveChanges();
                                //Wrong Password
                                return 3;
                            }
                            else
                            {
                                //User locked
                                return 2;
                            }
                        }
                    }
                    else
                        //Wrong UserName and Password
                        return 4;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckUser(string userName, string password)
        {
            try
            {
                var user = this.dbContext.Users.Where(u => u.UserName == userName && u.User_Lock == 1).FirstOrDefault();
                if (user != null)
                {
                    return 2;
                }
                user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
                if (user != null && user.Password == password)
                {
                    if (user.User_Status != 1)
                    {
                        return 6;
                    }
                    if (user.User_Status == 1 && user.NewUserFlag == 1)
                    {
                        return 7;
                    }
                    var role = (from r in this.dbContext.UserRoleFacilityConfigs
                                where r.User_Id == user.User_Id && r.UserRole_Status == 1
                                select r).FirstOrDefault();
                    if (role != null)
                    {
                        user.User_PwdCount = 0;
                        user.User_Lock = 0;
                        this.dbContext.SaveChanges();
                        //if (user.User_PwdCount != 0)
                        //{
                        //    user.User_PwdCount = 0;
                        //    this.dbContext.SaveChanges();
                        //}
                        if (GetUserToken(user.User_Id) == null)
                            return 1;
                        else
                        {
                            LogoutUserTime(user.User_Id);
                            return 1;
                        }
                    }
                    else
                        //No Role or NS mapped
                        return 5;
                }
                else
                {
                    user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    if (user != null && user.Password != password)
                    {
                        if (user.User_Lock == 0 && user.User_PwdCount < 3)
                        {
                            user.User_PwdCount = user.User_PwdCount + 1;
                            if (user.User_PwdCount == 3)
                                user.User_Lock = 1;
                            this.dbContext.SaveChanges();
                            //Wrong Password
                            return 3;
                        }
                        else
                        {
                            //User locked
                            return 2;
                        }
                    }
                    else
                        //Wrong UserName and Password
                        return 4;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetUserId(string UserName, string Password)
        {
            var user = this.dbContext.Users.Where(us => us.UserName.ToUpper() == UserName.ToUpper() && us.Password == Password).FirstOrDefault();
            if (user != null)
                return user.User_Id;
            else
                return 0;
        }
        public UserCustomEntity FindUser(string userName, string password)
        {
            try
            {
                var data = (from u in this.dbContext.Users
                                //join g in this.dbContext.Genders on u.User_Gender equals g.Gender_Id
                            join ur in this.dbContext.UserRoleFacilityConfigs on u.User_Id equals ur.User_Id
                            join r in this.dbContext.Roles on ur.Role_ID equals r.Role_Id
                            join d in this.dbContext.DefaultScreens on r.DefaultScreen_Id equals d.DefaultScreen_Id
                            join s in this.dbContext.Screens on d.Screen_Id equals s.Screen_Id
                            where u.UserName == userName && u.Password == password

                            select new UserCustomEntity
                            {
                                User_Id = u.User_Id,
                                User_Suffix = u.User_Suffix,
                                UserName = u.UserName,
                                User_Gender = null,//u.User_Gender,
                                User_MaritalStatus = null,//u.User_MaritalStatus,
                                User_DisplayName = u.User_DisplayName,
                                User_Email = u.User_Email,
                                User_Phone = u.User_Phone,
                                UserGender_Desc = null,//g.Gender_Desc,
                                User_Lock = u.User_Lock,
                                RoleName = r.Role_Desc,
                                RoleId = r.Role_Id,
                                DefaultScreenId = s.Screen_Id,
                                Facility_Id = (this.dbContext.UserRoleFacilityConfigs.Where(e => e.User_Id == u.User_Id).Select(ur => ur.Facility_id).ToList()),
                                NurseStation_Id = (this.dbContext.UserRoleFacilityConfigs.Where(e => e.User_Id == u.User_Id && e.NurseStation_Id != null).Select(ur => ur.NurseStation_Id).ToList()),
                            }).FirstOrDefault();
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertUserToken(int userId, string userToken)
        {
            var record = this.dbContext.Users.Where(u => u.User_Id == userId).FirstOrDefault();
            if (record != null)
            {
                record.User_Token = userToken;
                this.dbContext.SaveChanges();
                return 1;
            }
            else
            {
                //No such record
                return 2;
            }
        }
        public string GetUserToken(int userId)
        {
            var record = this.dbContext.Users.Where(u => u.User_Id == userId).SingleOrDefault();
            if (record != null)
            {
                return record.User_Token;
            }
            else
                return null;
        }

        public int LogoutUser(int userId)
        {
            var record = this.dbContext.Users.Where(u => u.User_Id == userId).SingleOrDefault();
            if (record != null)
            {
                record.User_Token = null;
                this.dbContext.SaveChanges();
                if (HttpContext.Current.User != null)
                {
                    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                    if (claimsIdentity.FindFirst("SessionId").Value != "")
                    {
                        int sessionId = Convert.ToInt32(claimsIdentity.FindFirst("SessionId").Value);
                        var entity = this.dbContext.UserSessions.Where(u => u.Session_Id == sessionId).SingleOrDefault();
                        if (entity != null)
                        {
                            entity.LogOutTime = DateTime.Now;
                            this.dbContext.SaveChanges();
                            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                            {
                                Screen_Id = (int)ScreenEntity.Screens.Users,
                                Activity_Id = (int)ActivityEntity.ActivityMaster.Signout,
                                Comments = Convert.ToString(userId),
                                Session_Id = 0,
                                Time = DateTime.Now,
                                UserActivity_Id = 0,

                            };

                            _userActivityRepository.InsertUserActivityDetails(activityEntity);
                        }
                    }
                }
                else
                {
                    var entity = this.dbContext.UserSessions.Where(u => u.user_Id == userId && u.LogOutTime == null).OrderByDescending(u => u.Session_Id).FirstOrDefault();
                    if (entity != null)
                    {
                        var activityDetail = this.dbContext.UserActivityDetails.Where(ua => ua.Session_Id == entity.Session_Id).OrderByDescending(u => u.Time).FirstOrDefault();
                        if (activityDetail != null)
                            entity.LogOutTime = activityDetail.Time;
                        else
                            entity.LogOutTime = DateTime.Now;

                        this.dbContext.SaveChanges();
                    }
                }

                return 1;
            }
            else
                return 0;

        }
        public int LogoutUserTime(int userId)
        {
            var entity = this.dbContext.UserSessions.Where(u => u.user_Id == userId && u.LogOutTime == null).OrderByDescending(u => u.Session_Id).FirstOrDefault();
            if (entity != null)
            {
                var activityDetail = this.dbContext.UserActivityDetails.Where(ua => ua.Session_Id == entity.Session_Id).OrderByDescending(u => u.Time).FirstOrDefault();
                if (activityDetail != null)
                    entity.LogOutTime = activityDetail.Time;
                else
                    entity.LogOutTime = DateTime.Now;

                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public Int64 InsertUserSession(int userId, int role, string systemIp, string browserName)
        {
            UserSession userObj = new UserSession();
            userObj.user_Id = userId;
            userObj.LoginTime = DateTime.Now;
            userObj.Session_Status = 1;
            userObj.SystemIP = systemIp == "undefined" ? "" : systemIp;
            userObj.BrowserName = browserName;
            userObj.role_Id = role;
            this.dbContext.UserSessions.Add(userObj);
            this.dbContext.SaveChanges();
            //var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            //var identity = new ClaimsIdentity(claimsIdentity);
            //identity.AddClaim(new Claim("SessionId", userObj.Session_Id.ToString()));

            //ClaimsPrincipal principal = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
            //principal.AddIdentity(identity);
            //var customClaimValue = principal.Claims.Where(c => c.Type == "CompanyID").Single().Value;
            return userObj.Session_Id;
        }
        public List<UserEntity> GetStockReportUserDropData()
        {
            var userlist = this.dbContext.UserRoleFacilityConfigs.Select(ur => ur.User_Id).Distinct().ToList();
            return (from u in this.dbContext.Users
                    where u.User_Status == 1 && u.StkReportReq == 1 && userlist.Contains(u.User_Id)
                    select new UserEntity
                    {
                        User_Id = u.User_Id,
                        UserName = u.UserName
                    }).OrderBy(item => item.UserName).ToList();
        }
        public int UpdateUsersStatus(List<UserEntity> data)
        {
            foreach (var item in data)
            {
                item.User_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Users.Find(item.User_Id);
                if (record != null)
                {
                    record.User_Status = record.User_Status == 1 ? 0 : 1;
                    record.User_CreatedBy = item.User_CreatedBy;
                    record.User_CreatedDate = item.User_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int ResetUserPwdByAdmin(UserEntity Obj)
        {
            if (Obj.NewUserFlag == 1)
            {
                var reset = this.dbContext.Users.Where(u => u.User_Id == Obj.User_Id).FirstOrDefault();
                if (reset != null)
                {
                    User user = this.dbContext.Users.Find(reset.User_Id);
                    user.Password = Obj.Password;
                    user.NewUserFlag = Obj.NewUserFlag;
                    user.User_CreatedBy = Obj.User_CreatedBy;
                    user.User_CreatedDate = Obj.User_CreatedDate;
                    this.dbContext.SaveChanges();
                    return 1;
                }
                return 0;
            }
            else if (Obj.NewUserFlag == 0)
            {
                var unlock = this.dbContext.Users.Where(u => u.User_Id == Obj.User_Id).FirstOrDefault();
                if (unlock != null)
                {
                    User user = this.dbContext.Users.Find(unlock.User_Id);
                    user.User_PwdCount = Obj.User_PwdCount;
                    user.User_Lock = Obj.User_Lock;
                    user.User_CreatedBy = Obj.User_CreatedBy;
                    user.User_CreatedDate = Obj.User_CreatedDate;
                    this.dbContext.SaveChanges();
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public int GetFacilityOfUser(int dUserId, int facility_Id)
        {
            try
            {
                var data = this.dbContext.UserRoleFacilityConfigs.Where(e => e.User_Id == dUserId && e.Facility_id == facility_Id).Select(ur => ur.Facility_id).ToList();
                if (data.Count > 0)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

            }
            return 0;

        }

        public List<UserDropEntity> GetUsersByNSID(int nS_Id)
        {
            List<UserDropEntity> toUsers = new List<UserDropEntity>();
            int userId = 0;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
            }
            List<int> facilityIds = this.dbContext.UserRoleFacilityConfigs.Where(i => i.User_Id == userId && i.NurseStation_Id == nS_Id).Select(i => i.Facility_id).ToList();
            if (facilityIds.Count() > 0)
            {
                List<int> companyIds = this.dbContext.Facilities.Where(p => facilityIds.Contains(p.Facility_Id)).Select(p => p.Company_Id).ToList();

                var toFacilityIds = this.dbContext.Facilities.Where(f => companyIds.Contains(f.Company_Id)).Select(f => f.Facility_Id).ToList();
                var userIds = this.dbContext.UserRoleFacilityConfigs.Where(f => toFacilityIds.Contains(f.Facility_id)).Select(u => u.User_Id).Distinct();
                toUsers = (from u in this.dbContext.Users
                           where u.User_Status == 1 && userIds.Contains(u.User_Id)
                           select new UserDropEntity
                           {
                               User_Id = u.User_Id,
                               User_DisplayName = u.User_DisplayName
                           }).OrderBy(item => item.User_DisplayName).ToList();

            }
            return toUsers;
        }
        public int AutoLogoutUser(int userId)
        {
            var record = this.dbContext.Users.Where(u => u.User_Id == userId).SingleOrDefault();
            if (record != null)
            {
                if (HttpContext.Current.User != null)
                {
                    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                    if (claimsIdentity.FindFirst("SessionId").Value != "")
                    {
                        int sessionId = Convert.ToInt32(claimsIdentity.FindFirst("SessionId").Value);
                        var entity = this.dbContext.UserSessions.Where(u => u.Session_Id == sessionId).SingleOrDefault();
                        if (entity != null)
                        {
                            entity.LogOutTime = DateTime.Now;
                            this.dbContext.SaveChanges();
                            var userActivity = this.dbContext.UserActivityDetails.Where(ua => ua.Session_Id == sessionId && ua.Activity_Id == 9).FirstOrDefault();
                            if (userActivity == null)
                            {
                                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                                {
                                    Screen_Id = (int)ScreenEntity.Screens.Users,
                                    Activity_Id = (int)ActivityEntity.ActivityMaster.AutoSignout,
                                    Comments = Convert.ToString(userId),
                                    Session_Id = 0,
                                    Time = DateTime.Now,
                                    UserActivity_Id = 0,

                                };

                                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                            }
                        }
                    }
                }
                else
                {
                    var entity = this.dbContext.UserSessions.Where(u => u.user_Id == userId && u.LogOutTime == null).OrderByDescending(u => u.Session_Id).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.LogOutTime = DateTime.Now;
                        this.dbContext.SaveChanges();
                        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                        {
                            Screen_Id = (int)ScreenEntity.Screens.Users,
                            Activity_Id = (int)ActivityEntity.ActivityMaster.AutoSignout,
                            Comments = Convert.ToString(userId),
                            Session_Id = 0,
                            Time = DateTime.Now,
                            UserActivity_Id = 0,

                        };

                        _userActivityRepository.InsertUserActivityDetails(activityEntity);

                    }
                }

                return 1;
            }
            else
                return 0;

        }
    }
}

