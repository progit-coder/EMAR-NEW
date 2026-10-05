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
    public class RoleRepository : IRoleRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public RoleRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }
        public int InsertRole(RoleEntity roleEntity)
        {
            roleEntity.Role_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<RoleEntity, Role>(roleEntity);
            var defaultscreen = this.dbContext.DefaultScreens.Where(s => s.DefaultScreen_Id == roleEntity.DefaultScreen_Id).FirstOrDefault();
            var roleconfig = this.dbContext.RoleConfigs.Where(u => u.Role_Id == roleEntity.Role_Id && u.Screen_Id == defaultscreen.Screen_Id).FirstOrDefault();
            RoleConfig res = new RoleConfig();
            if (record.Role_Id == 0)
            {
                record.DefaultScreen_Id = defaultscreen.DefaultScreen_Id;
                this.dbContext.Roles.Add(record);
                this.dbContext.SaveChanges();
                res.Role_Id = record.Role_Id;
                res.Screen_Id = defaultscreen.Screen_Id;
                res.AccessRead = 1;
                res.AccessWrite = 0;
                res.PrintExcel = 0;
                res.PrintPdf = 0;
                res.RoleConfig_CreatedBy = record.Role_CreatedBy;
                res.RoleConfig_CreatedDate = record.Role_CreatedDate;
                res.RoleConfig_Status = 1;
                this.dbContext.RoleConfigs.Add(res);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RoleMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.Role_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                Role role = this.dbContext.Roles.Find(record.Role_Id);
                //floor.Facility_Id = record.Facility_Id;
                role.Role_Desc = record.Role_Desc;
                role.DefaultScreen_Id = defaultscreen.DefaultScreen_Id;
                if (record.Role_Id == 1)
                {
                    role.Parent_Id = null;
                }
                else
                {
                    role.Parent_Id = record.Parent_Id;
                }
                role.IsAdmin = record.IsAdmin;
                role.Role_Status = record.Role_Status;
                role.Role_CreatedDate = record.Role_CreatedDate;
                role.Role_CreatedBy = record.Role_CreatedBy;
                this.dbContext.SaveChanges();
                if (roleconfig == null)
                {
                    res.Role_Id = record.Role_Id;
                    res.Screen_Id = defaultscreen.Screen_Id;
                    res.AccessRead = 1;
                    res.AccessWrite = 0;
                    res.PrintExcel = 0;
                    res.PrintPdf = 0;
                    res.RoleConfig_CreatedBy = record.Role_CreatedBy;
                    res.RoleConfig_CreatedDate = record.Role_CreatedDate;
                    res.RoleConfig_Status = 1;
                    this.dbContext.RoleConfigs.Add(res);
                    this.dbContext.SaveChanges();
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RoleMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Role_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }
        public RoleEntity GetRoleDetailsByID(int RoleID)
        {
            var role = this.dbContext.Roles.Find(RoleID);
            return this.autoMapper.Map<Role, RoleEntity>(role);
        }
        public List<RoleDropdownEntity> GetSubRolesList()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("RoleId").Value != "")
            {
                int roleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                if (roleId == 1)
                {
                    var records = this.dbContext.Roles.Where(e => e.Role_Status == 1).OrderBy(r => r.Role_Desc).ToList();
                    if (records.Count() > 0)
                    {
                        return this.autoMapper.Map<List<Role>, List<RoleDropdownEntity>>(records);
                    }
                }
                else
                {
                    var roleEntity = this.dbContext.Roles.Where(e => e.Role_Id == roleId && e.Role_Status == 1).FirstOrDefault();
                    if (roleEntity != null)
                    {
                        var record = this.autoMapper.Map<Role, RoleDropdownEntity>(roleEntity);
                        var result = new List<RoleDropdownEntity>();
                        result.Add(record);
                        result.AddRange(GetSubRoles(record));
                        return result.OrderBy(r=>r.Role_Desc).ToList();
                    }
                }

            }
            return null;
        }
        public List<RoleDropdownEntity> GetSubRoles(RoleDropdownEntity roleEntity)
        {
            var result = new List<RoleDropdownEntity>();

            var roles = this.dbContext.Roles.Where(e => e.Parent_Id == roleEntity.Role_Id && e.Role_Status == 1).ToList();

            foreach (var role in roles)
            {
                var record = this.autoMapper.Map<Role, RoleDropdownEntity>(role);

                result.Add(record);
                result.AddRange(GetSubRoles(record));
            }

            return result;
        }

        public List<RoleGridData> GetRoleDetailsAll()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                var records = this.dbContext.PrcGetRoleData(userId).ToList();
                var list = this.autoMapper.Map<List<PrcGetRoleData_Result>, List<RoleGridData>>(records);
                return list;
            }
            return null;
        }
        public List<RoleEntity> GetRoleDropData()
        {
            return (from re in this.dbContext.Roles
                    where re.Role_Status == 1
                    select new RoleEntity
                    {
                        Role_Id = re.Role_Id,
                        Role_Desc = re.Role_Desc
                    }).Distinct().OrderBy(item => item.Role_Desc).ToList();
        }
        public List<ScreenEntity> GetAllScreens(int screen)
        {
            List<ScreenEntity> screens = new List<ScreenEntity>();
            if (screen == 1)
            {
                var list = this.dbContext.Screens.Where(s => s.Screen_Status == 1).OrderBy(item => item.Screen_Desc).ToList();
                screens = this.autoMapper.Map<List<Screen>, List<ScreenEntity>>(list);
            }
            else if (screen == 0)
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("RoleId").Value != "")
                {
                    int RoleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                    screens = (from d in this.dbContext.DefaultScreens
                               join s in this.dbContext.Screens on d.Screen_Id equals s.Screen_Id
                               join rc in this.dbContext.RoleConfigs on d.Screen_Id equals rc.Screen_Id
                               where s.Screen_Status == 1 && d.DefaultScreen_Status == 1 && rc.Role_Id ==RoleId
                               select new ScreenEntity
                               {
                                   DefaultScreen_Id = d.DefaultScreen_Id,
                                   Screen_Id = (int)d.Screen_Id,
                                   Screen_Desc = s.Screen_Desc,
                               }
                         ).OrderBy(item => item.Screen_Desc).ToList();
                }
            }
            return screens;
        }
        public List<ScreenEntity> GetSubRolesScreenList(int roleID, string status)
        {
            var superiorRoleId = this.dbContext.Roles.Where(e => e.Role_Id == roleID).Select(e => e.Parent_Id).FirstOrDefault();

            if (superiorRoleId != null && status != "RoleMaster")

            {
                var list = (from us in this.dbContext.RoleConfigs
                            join sc in this.dbContext.Screens on us.Screen_Id equals sc.Screen_Id
                            where (sc.Screen_Status == 1 || sc.Screen_Status == 2) && us.Role_Id == superiorRoleId
                            select new ScreenEntity
                            {
                                Screen_Id = (int)sc.Screen_Id,
                                Screen_Desc = sc.Screen_Desc,
                            }).OrderBy(item => item.Screen_Desc).ToList();
                return list;
            }
            else if (status == "RoleMaster")
            {
                var list = (from us in this.dbContext.RoleConfigs
                            join ds in this.dbContext.DefaultScreens on us.Screen_Id equals ds.Screen_Id
                            join sc in this.dbContext.Screens on ds.Screen_Id equals sc.Screen_Id
                            where sc.Screen_Status == 1 && us.Role_Id == roleID
                            select new ScreenEntity
                            {
                                Screen_Id = (int)sc.Screen_Id,
                                DefaultScreen_Id = (int)ds.DefaultScreen_Id,
                                Screen_Desc = sc.Screen_Desc,
                            }).OrderBy(item => item.Screen_Desc).ToList();
                return list;
            }


            return null;
        }
        public int InsertRoleConfigMaster(RoleConfigEntity role)
        {
            role.RoleConfig_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var roleCnf = this.autoMapper.Map<RoleConfigEntity, RoleConfig>(role);
            string[] screenString = role.Screens.ToString().Split(',');
            List<RoleConfig> record = this.dbContext.RoleConfigs.Where(r => screenString.Contains(r.Screen_Id.ToString()) && r.Role_Id == role.Role_Id).ToList();
            string[] isRecords = record.Select(s => s.Screen_Id.ToString()).ToArray();
            var resCheck = screenString.Except(isRecords);
            string[] noRecords = resCheck.ToArray();
            string insertScreen = string.Join(",", noRecords);
            string updateScreen = string.Join(",", isRecords);
            if(noRecords.Count()>0)
            {
                var res = this.dbContext.PrcInsertRoleConfig(role.Role_Id, insertScreen, role.RoleConfig_CreatedBy);
            }
            if(isRecords.Count()>0)
            {
                var Updateres = this.dbContext.PrcUpdateRoleConfig(role.Role_Id, updateScreen, role.RoleConfig_CreatedBy, roleCnf.AccessRead, roleCnf.AccessWrite, roleCnf.PrintPdf, roleCnf.PrintExcel, roleCnf.RoleConfig_Status);
            }
            //UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            //{
            //    Screen_Id = (int)ScreenEntity.Screens.RoleConfig,
            //    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
            //    Comments = roleCnf.Role_Id.ToString(),
            //    Session_Id = 0,
            //    Time = DateTime.Now,
            //    UserActivity_Id = 0,

            //};

            //_userActivityRepository.InsertUserActivityDetails(activityEntity);


            //foreach (var item in screenString)
            //{
            //    int ScreenId = Convert.ToInt32(item);
            //    RoleConfig record = this.dbContext.RoleConfigs.Where(r => r.Screen_Id == ScreenId && r.Role_Id == role.Role_Id).FirstOrDefault();

            //    if (record != null)
            //    {
            //        //record.Role_Id = roleCnf.Role_Id;
            //        //record.Screen_Id = roleCnf.Screen_Id;
            //        record.AccessRead = roleCnf.AccessRead;
            //        record.AccessWrite = roleCnf.AccessWrite;
            //        record.PrintExcel = roleCnf.PrintExcel;
            //        record.PrintPdf = roleCnf.PrintPdf;
            //        record.RoleConfig_Status = roleCnf.RoleConfig_Status;
            //        record.RoleConfig_CreatedBy = roleCnf.RoleConfig_CreatedBy;
            //        record.RoleConfig_CreatedDate = roleCnf.RoleConfig_CreatedDate;
            //        this.dbContext.SaveChanges();

            //        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            //        {
            //            Screen_Id = (int)ScreenEntity.Screens.RoleConfig,
            //            Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
            //            Comments = record.Role_Id.ToString(),
            //            Session_Id = 0,
            //            Time = DateTime.Now,
            //            UserActivity_Id = 0,

            //        };

            //        _userActivityRepository.InsertUserActivityDetails(activityEntity);

            //    }
            //    else
            //    {
            //        roleCnf.Screen_Id = Convert.ToInt32(item);
            //        this.dbContext.RoleConfigs.Add(roleCnf);
            //        this.dbContext.SaveChanges();
            //        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            //        {
            //            Screen_Id = (int)ScreenEntity.Screens.RoleConfig,
            //            Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
            //            Comments = roleCnf.Role_Id.ToString(),
            //            Session_Id = 0,
            //            Time = DateTime.Now,
            //            UserActivity_Id = 0,

            //        };

            //        _userActivityRepository.InsertUserActivityDetails(activityEntity);

            //    }

            //}
            return 1;
        }
        public List<RoleConfigEntity> GetRoleConfigDetailsAll()
        {
            var role = this.dbContext.RoleConfigs.ToList();
            return this.autoMapper.Map<List<RoleConfig>, List<RoleConfigEntity>>(role);
        }
        public List<RoleConfigGridEntity> GetRoleConfigsGrid(int userId, int roleId)
        {
            if (roleId == 0)
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                {
                    roleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                }
            }

            var records = this.dbContext.PrcGetRoleconfigData(userId, roleId).ToList();
            var list = this.autoMapper.Map<List<PrcGetRoleconfigData_Result>, List<RoleConfigGridEntity>>(records);
            return list;

        }
        public int InsertUserFacilityRoleConfig(UserRoleFacilityConfigCustomEntity userConfig)
        {
            if (userConfig.UserRole_Id == 0)
            {
                var userRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id).ToList();
                if (userRecords.Count() > 0)
                {
                    var roleIds = userRecords.Select(u => u.Role_ID).Distinct().ToArray();
                    var facilityIds = userRecords.Select(f => f.Facility_id).Distinct().ToArray();
                    if (roleIds.Count() > 0)
                    {
                        if (roleIds.Contains(userConfig.Role_Id))
                        {
                            if (facilityIds.Count() > 0 && facilityIds.Contains(userConfig.Facility_Id))
                            {
                                //Another Role is already assigned to this user
                                return 2;
                            }
                        }
                        else
                            //Another Role is already assigned to this user
                            return 2;
                    }
                }

                string[] nstations = userConfig.NurseStations.ToString().Split(',');

                var record = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id).ToList();
                if (record.Count() == 0)
                {
                    UserRoleFacilityConfig entity;
                    foreach (var item in nstations)
                    {
                        entity = new UserRoleFacilityConfig()
                        {
                            UserRole_Id = 0,
                            User_Id = userConfig.User_Id,
                            Role_ID = userConfig.Role_Id,
                            Facility_id = userConfig.Facility_Id,
                            NurseStation_Id = Convert.ToInt32(item),
                            UserRole_Status = 1,
                            UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                            UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                        };

                        this.dbContext.UserRoleFacilityConfigs.Add(entity);

                        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                        {
                            Screen_Id = (int)ScreenEntity.Screens.UserRoleConfig,
                            Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                            Comments = entity.UserRole_Id.ToString(),
                            Session_Id = 0,
                            Time = DateTime.Now,
                            UserActivity_Id = 0,

                        };
                        _userActivityRepository.InsertUserActivityDetails(activityEntity);
                        this.dbContext.SaveChanges();


                    }
                }
                else
                {
                    var nurseStationIds = record.Select(r => (int)r.NurseStation_Id).ToArray();
                    int[] nsIds = Array.ConvertAll(nstations, int.Parse);
                    var items = nurseStationIds.Except(nsIds);
                    foreach (var item in items)
                    {
                        var existingRecord = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id && r.NurseStation_Id == item).FirstOrDefault();
                        if (existingRecord != null)
                        {
                            this.dbContext.UserRoleFacilityConfigs.Remove(existingRecord);
                            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                            {
                                Screen_Id = (int)ScreenEntity.Screens.UserRoleConfig,
                                Activity_Id = (int)ActivityEntity.ActivityMaster.Delete,
                                Comments = existingRecord.UserRole_Id.ToString(),
                                Session_Id = 0,
                                Time = DateTime.Now,
                                UserActivity_Id = 0,

                            };
                            _userActivityRepository.InsertUserActivityDetails(activityEntity);
                            this.dbContext.SaveChanges();
                        }
                    }

                    UserRoleFacilityConfig entity;
                    foreach (var item in nstations)
                    {
                        var existingRecord = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id && r.NurseStation_Id == Convert.ToInt32(item)).FirstOrDefault();
                        if (existingRecord == null)
                        {
                            entity = new UserRoleFacilityConfig()
                            {
                                UserRole_Id = 0,
                                User_Id = userConfig.User_Id,
                                Role_ID = userConfig.Role_Id,
                                Facility_id = userConfig.Facility_Id,
                                NurseStation_Id = Convert.ToInt32(item),
                                UserRole_Status = 1,
                                UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                            };

                            this.dbContext.UserRoleFacilityConfigs.Add(entity);
                            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                            {
                                Screen_Id = (int)ScreenEntity.Screens.UserRoleConfig,
                                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                                Comments = entity.UserRole_Id.ToString(),
                                Session_Id = 0,
                                Time = DateTime.Now,
                                UserActivity_Id = 0,

                            };
                            _userActivityRepository.InsertUserActivityDetails(activityEntity);
                            this.dbContext.SaveChanges();
                        }
                        else
                        {
                            existingRecord.UserRole_CreatedBy = userConfig.UserRole_CreatedBy;
                            existingRecord.UserRole_CreatedDate = userConfig.UserRole_CreatedDate;
                            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                            {
                                Screen_Id = (int)ScreenEntity.Screens.UserRoleConfig,
                                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                                Comments = existingRecord.UserRole_Id.ToString(),
                                Session_Id = 0,
                                Time = DateTime.Now,
                                UserActivity_Id = 0,

                            };
                            _userActivityRepository.InsertUserActivityDetails(activityEntity);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
            }
            else if (userConfig.UserRole_Id == 1)
            {
                //For Editing
                string[] nstations = userConfig.NurseStations.ToString().Split(',');
                List<UserRoleFacilityConfig> record = new List<UserRoleFacilityConfig>();
                if ((userConfig.Role_Id == userConfig.OldRole_Id) || (userConfig.User_Id == userConfig.OldUser_Id) || (userConfig.Facility_Id == userConfig.OldFacility_Id))
                {
                    if (userConfig.Role_Id == userConfig.OldRole_Id && userConfig.User_Id == userConfig.OldUser_Id && userConfig.Facility_Id == userConfig.OldFacility_Id)
                    {
                        record = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id).ToList();
                        if (record.Count() == 0)
                        {
                            UserRoleFacilityConfig entity;
                            foreach (var item in nstations)
                            {
                                entity = new UserRoleFacilityConfig()
                                {
                                    UserRole_Id = 0,
                                    User_Id = userConfig.User_Id,
                                    Role_ID = userConfig.Role_Id,
                                    Facility_id = userConfig.Facility_Id,
                                    NurseStation_Id = Convert.ToInt32(item),
                                    UserRole_Status = 1,
                                    UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                    UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                                };

                                this.dbContext.UserRoleFacilityConfigs.Add(entity);
                                this.dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var nurseStationIds = record.Select(r => (int)r.NurseStation_Id).ToArray();
                            int[] nsIds = Array.ConvertAll(nstations, int.Parse);
                            var items = nurseStationIds.Except(nsIds);
                            foreach (var item in items)
                            {
                                var tableRecord = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id && r.NurseStation_Id == item).FirstOrDefault();
                                if (tableRecord != null)
                                {
                                    this.dbContext.UserRoleFacilityConfigs.Remove(tableRecord);
                                    this.dbContext.SaveChanges();
                                }
                            }
                            UserRoleFacilityConfig entity;
                            foreach (var item in nstations)
                            {
                                var NsId = Convert.ToInt32(item);
                                var existingRecord = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id && r.NurseStation_Id == NsId).FirstOrDefault();
                                if (existingRecord == null)
                                {
                                    entity = new UserRoleFacilityConfig()
                                    {
                                        UserRole_Id = 0,
                                        User_Id = userConfig.User_Id,
                                        Role_ID = userConfig.Role_Id,
                                        Facility_id = userConfig.Facility_Id,
                                        NurseStation_Id = Convert.ToInt32(item),
                                        UserRole_Status = 1,
                                        UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                        UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                                    };

                                    this.dbContext.UserRoleFacilityConfigs.Add(entity);
                                    this.dbContext.SaveChanges();
                                }
                                else
                                {
                                    existingRecord.User_Id = userConfig.User_Id;
                                    existingRecord.Role_ID = userConfig.Role_Id;
                                    existingRecord.Facility_id = userConfig.Facility_Id;
                                    existingRecord.NurseStation_Id = Convert.ToInt32(item);
                                    existingRecord.UserRole_Status = 1;
                                    existingRecord.UserRole_CreatedBy = userConfig.UserRole_CreatedBy;
                                    existingRecord.UserRole_CreatedDate = userConfig.UserRole_CreatedDate;
                                    this.dbContext.SaveChanges();
                                }
                            }
                        }
                        foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                        {
                            item.Role_ID = userConfig.Role_Id;

                        }
                        this.dbContext.SaveChanges();
                    }
                    else if (userConfig.Role_Id != userConfig.OldRole_Id && userConfig.User_Id == userConfig.OldUser_Id && userConfig.Facility_Id == userConfig.OldFacility_Id)
                    {
                        record = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID != userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id).ToList();
                        if (record.Count() == 0)
                        {
                            UserRoleFacilityConfig entity;
                            foreach (var item in nstations)
                            {
                                entity = new UserRoleFacilityConfig()
                                {
                                    UserRole_Id = 0,
                                    User_Id = userConfig.User_Id,
                                    Role_ID = userConfig.Role_Id,
                                    Facility_id = userConfig.Facility_Id,
                                    NurseStation_Id = Convert.ToInt32(item),
                                    UserRole_Status = 1,
                                    UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                    UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                                };

                                this.dbContext.UserRoleFacilityConfigs.Add(entity);
                                this.dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var nurseStationIds = record.Select(r => (int)r.NurseStation_Id).ToArray();
                            int[] nsIds = Array.ConvertAll(nstations, int.Parse);
                            var items = nurseStationIds.Except(nsIds);
                            foreach (var item in items)
                            {
                                var tableRecord = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id && r.NurseStation_Id == item).FirstOrDefault();
                                if (tableRecord != null)
                                {
                                    this.dbContext.UserRoleFacilityConfigs.Remove(tableRecord);
                                    this.dbContext.SaveChanges();
                                }
                            }
                            UserRoleFacilityConfig entity;
                            foreach (var item in nstations)
                            {
                                var NsId = Convert.ToInt32(item);
                                var existingRecord = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID != userConfig.Role_Id && r.Facility_id == userConfig.Facility_Id && r.NurseStation_Id == NsId).FirstOrDefault();
                                if (existingRecord == null)
                                {
                                    entity = new UserRoleFacilityConfig()
                                    {
                                        UserRole_Id = 0,
                                        User_Id = userConfig.User_Id,
                                        Role_ID = userConfig.Role_Id,
                                        Facility_id = userConfig.Facility_Id,
                                        NurseStation_Id = Convert.ToInt32(item),
                                        UserRole_Status = 1,
                                        UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                        UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                                    };

                                    this.dbContext.UserRoleFacilityConfigs.Add(entity);
                                    this.dbContext.SaveChanges();
                                }
                                else
                                {
                                    existingRecord.User_Id = userConfig.User_Id;
                                    existingRecord.Role_ID = userConfig.Role_Id;
                                    existingRecord.Facility_id = userConfig.Facility_Id;
                                    existingRecord.NurseStation_Id = Convert.ToInt32(item);
                                    existingRecord.UserRole_Status = 1;
                                    existingRecord.UserRole_CreatedBy = userConfig.UserRole_CreatedBy;
                                    existingRecord.UserRole_CreatedDate = userConfig.UserRole_CreatedDate;
                                    this.dbContext.SaveChanges();
                                }
                            }
                        }
                        foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                        {
                            item.Role_ID = userConfig.Role_Id;

                        }
                        this.dbContext.SaveChanges();
                    }
                    else if (userConfig.Role_Id == userConfig.OldRole_Id && userConfig.User_Id != userConfig.OldUser_Id && userConfig.Facility_Id == userConfig.OldFacility_Id)
                    {
                        var existingOldUserRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.OldUser_Id && r.Facility_id == userConfig.OldFacility_Id).ToList();
                        foreach (var item in existingOldUserRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        var existingNewUserRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Facility_id == userConfig.Facility_Id).ToList();
                        foreach (var item in existingNewUserRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        UserRoleFacilityConfig entity;
                        foreach (var item in nstations)
                        {
                            entity = new UserRoleFacilityConfig()
                            {
                                UserRole_Id = 0,
                                User_Id = userConfig.User_Id,
                                Role_ID = userConfig.Role_Id,
                                Facility_id = userConfig.Facility_Id,
                                NurseStation_Id = Convert.ToInt32(item),
                                UserRole_Status = 1,
                                UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                            };

                            this.dbContext.UserRoleFacilityConfigs.Add(entity);
                            this.dbContext.SaveChanges();
                        }
                        foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                        {
                            item.Role_ID = userConfig.Role_Id;

                        }
                        this.dbContext.SaveChanges();
                    }
                    else if (userConfig.Role_Id != userConfig.OldRole_Id && userConfig.User_Id != userConfig.OldUser_Id && userConfig.Facility_Id == userConfig.OldFacility_Id)
                    {
                        var existingOldUserRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.OldUser_Id).ToList();
                        foreach (var item in existingOldUserRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        var existingNewUserRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id).ToList();
                        foreach (var item in existingNewUserRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        UserRoleFacilityConfig entity;
                        foreach (var item in nstations)
                        {
                            entity = new UserRoleFacilityConfig()
                            {
                                UserRole_Id = 0,
                                User_Id = userConfig.User_Id,
                                Role_ID = userConfig.Role_Id,
                                Facility_id = userConfig.Facility_Id,
                                NurseStation_Id = Convert.ToInt32(item),
                                UserRole_Status = 1,
                                UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                            };

                            this.dbContext.UserRoleFacilityConfigs.Add(entity);
                            this.dbContext.SaveChanges();
                        }
                        foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                        {
                            item.Role_ID = userConfig.Role_Id;

                        }
                        this.dbContext.SaveChanges();
                    }
                    else if (userConfig.Role_Id == userConfig.OldRole_Id && userConfig.User_Id != userConfig.OldUser_Id && userConfig.Facility_Id != userConfig.OldFacility_Id)
                    {
                        var existingOldUserRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.OldUser_Id).ToList();
                        foreach (var item in existingOldUserRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        var existingNewUserRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id).ToList();
                        foreach (var item in existingNewUserRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        UserRoleFacilityConfig entity;
                        foreach (var item in nstations)
                        {
                            entity = new UserRoleFacilityConfig()
                            {
                                UserRole_Id = 0,
                                User_Id = userConfig.User_Id,
                                Role_ID = userConfig.Role_Id,
                                Facility_id = userConfig.Facility_Id,
                                NurseStation_Id = Convert.ToInt32(item),
                                UserRole_Status = 1,
                                UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                            };

                            this.dbContext.UserRoleFacilityConfigs.Add(entity);
                            this.dbContext.SaveChanges();
                        }
                        foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                        {
                            item.Role_ID = userConfig.Role_Id;

                        }
                        this.dbContext.SaveChanges();
                    }
                    else if (userConfig.Role_Id != userConfig.OldRole_Id && userConfig.User_Id == userConfig.OldUser_Id && userConfig.Facility_Id != userConfig.OldFacility_Id)
                    {
                        var existingRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Role_ID == userConfig.OldRole_Id && r.Facility_id == userConfig.OldFacility_Id).ToList();
                        foreach (var item in existingRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        UserRoleFacilityConfig entity;
                        foreach (var item in nstations)
                        {
                            entity = new UserRoleFacilityConfig()
                            {
                                UserRole_Id = 0,
                                User_Id = userConfig.User_Id,
                                Role_ID = userConfig.Role_Id,
                                Facility_id = userConfig.Facility_Id,
                                NurseStation_Id = Convert.ToInt32(item),
                                UserRole_Status = 1,
                                UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                            };

                            this.dbContext.UserRoleFacilityConfigs.Add(entity);
                            this.dbContext.SaveChanges();
                        }
                        foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                        {
                            item.Role_ID = userConfig.Role_Id;

                        }
                        this.dbContext.SaveChanges();
                    }
                    else if (userConfig.Facility_Id != userConfig.OldFacility_Id && userConfig.Role_Id == userConfig.OldRole_Id && userConfig.User_Id == userConfig.OldUser_Id)
                    {

                        var existingOldfacilityRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Facility_id == userConfig.OldFacility_Id).ToList();
                        foreach (var item in existingOldfacilityRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        var existingNewFacilityRecords = this.dbContext.UserRoleFacilityConfigs.Where(r => r.User_Id == userConfig.User_Id && r.Facility_id == userConfig.Facility_Id).ToList();
                        foreach (var item in existingNewFacilityRecords)
                        {
                            if (item != null)
                            {
                                this.dbContext.UserRoleFacilityConfigs.Remove(item);
                                this.dbContext.SaveChanges();
                            }
                        }
                        UserRoleFacilityConfig entity;
                        foreach (var item in nstations)
                        {
                            entity = new UserRoleFacilityConfig()
                            {
                                UserRole_Id = 0,
                                User_Id = userConfig.User_Id,
                                Role_ID = userConfig.Role_Id,
                                Facility_id = userConfig.Facility_Id,
                                NurseStation_Id = Convert.ToInt32(item),
                                UserRole_Status = 1,
                                UserRole_CreatedBy = userConfig.UserRole_CreatedBy,
                                UserRole_CreatedDate = userConfig.UserRole_CreatedDate
                            };

                            this.dbContext.UserRoleFacilityConfigs.Add(entity);
                            this.dbContext.SaveChanges();
                        }

                    }
                    foreach (var item in this.dbContext.UserRoleFacilityConfigs.Where(x => x.User_Id == userConfig.User_Id).ToList())
                    {
                        item.Role_ID = userConfig.Role_Id;

                    }
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public List<UserRoleFacilityConfigEntity> GetUserRoleFacilityConfigByID(int UserRole_Id)
        {
            var userRole = (from uf in this.dbContext.UserRoleFacilityConfigs
                            join r in this.dbContext.Roles on uf.Role_ID equals r.Role_Id
                            join u in this.dbContext.Users on uf.User_Id equals u.User_Id
                            join cu in this.dbContext.Users on uf.UserRole_CreatedBy equals cu.User_Id
                            join f in this.dbContext.Facilities on uf.Facility_id equals f.Facility_Id
                            join n in this.dbContext.NursingStations on uf.NurseStation_Id equals n.NurseStation_Id
                            where uf.Role_ID == UserRole_Id
                            select new UserRoleFacilityConfigEntity
                            {
                                User_Id = uf.User_Id,
                                UserRole_Id = uf.UserRole_Id,
                                Role_Id = uf.Role_ID,
                                Facility_Id = uf.Facility_id,
                                FacilitName = f.Facility_Name,
                                NurseStationName = n.NurseStation_Name,
                                UserName = u.UserName,
                                CreatedUser = cu.User_DisplayName,
                                UserRole_Status = uf.UserRole_Status,
                                UserRole_CreatedBy = uf.UserRole_CreatedBy,
                                UserRole_CreatedDate = uf.UserRole_CreatedDate,
                                RoleName = r.Role_Desc,
                                User_DisplayName = u.User_DisplayName

                            }).ToList();
            return userRole;
        }
        public List<UserRoleFacilityConfigGridEntity> UserRoleFacilityConfigGrid(UserRoleConfigIds filterIds)
        {
            if (filterIds.User_Id == 0 && filterIds.Role_Id == 0 && filterIds.Facility_Id == 0)
            {
                var records = this.dbContext.PrcGetUserRoleConfigData(filterIds.User_Id).ToList();
                var list = this.autoMapper.Map<List<PrcGetUserRoleConfigData_Result>, List<UserRoleFacilityConfigGridEntity>>(records);
                return list.OrderBy(item => item.Company_Facility_Nursestation).ToList();
            }
            else if (filterIds.User_Id != 0 || filterIds.Role_Id != 0 || filterIds.Facility_Id != 0)
            {
                var records = this.dbContext.PrcGetUserRoleConfigData(filterIds.User_Id).ToList();
                if (filterIds.User_Id != 0)
                {
                    records = records.Where(u => u.User_Id == filterIds.User_Id).ToList();
                }
                var list = this.autoMapper.Map<List<PrcGetUserRoleConfigData_Result>, List<UserRoleFacilityConfigGridEntity>>(records.ToList());
                return list.OrderBy(item => item.Company_Facility_Nursestation).ToList();
            }
            return null;
        }

        public UserRoleFacilityConfigCustomEntity GetUserRoleFacilityConfigByRoleId(int roleId, int userId, int facilityId)
        {
            List<int?> Nslist = new List<int?>();
            var records = this.dbContext.UserRoleFacilityConfigs.Where(r => r.Role_ID == roleId && r.User_Id == userId && r.Facility_id == facilityId).ToList();
            string nursestation = string.Empty;
            var NsIdsArray = records.Select(n => n.NurseStation_Id).ToArray();
            foreach (var items in NsIdsArray)
            {
                int NsId = (int)items;
                int NsIdCheck = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == NsId && n.NurseStation_Status == 1).Select(n => n.NurseStation_Id).FirstOrDefault();
                if (NsIdCheck != 0)
                {
                    Nslist.Add(items);
                }
            }
            if (Nslist.Count() > 0)
            {
                nursestation = string.Join(",", Nslist);
            }
            var entity = new UserRoleFacilityConfigCustomEntity
            {
                UserRole_Id = 1,
                User_Id = userId,
                Facility_Id = facilityId,
                Role_Id = roleId,
                NurseStations = nursestation

            };
            return entity;
        }

        public List<ScreenPermissionsCustomEntity> GetScreenPermissionsForLoggedInUser(int userId, int roleId)
        {
            //int roleId = this.dbContext.UserRoleFacilityConfigs.Where(u => u.User_Id == userId && u.Facility_id == facilityId && u.UserRole_Status == 1).Select(u => u.Role_ID).FirstOrDefault();            
            if (roleId != 0)
            {
                List<ScreenPermissionsCustomEntity> result = (from rc in this.dbContext.RoleConfigs
                                                              join s in this.dbContext.Screens on rc.Screen_Id equals s.Screen_Id
                                                              where (rc.Role_Id == roleId && (s.Screen_Status == 1 || s.Screen_Status ==2) && rc.RoleConfig_Status == 1)
                                                              orderby (s.Screen_Order)
                                                              select new ScreenPermissionsCustomEntity
                                                              {
                                                                  Screen_Id = s.Screen_Id,
                                                                  Screen_Desc = s.Screen_Desc,
                                                                  AccessRead = (int)rc.AccessRead,
                                                                  AccessWrite = (int)rc.AccessWrite,
                                                                  PrintPdf = (int)rc.PrintPdf,
                                                                  PrintExcel = (int)rc.PrintExcel
                                                              }).ToList();
                return result;
            }
            return null;
        }
        public string GetRoleByUserId(int userId, int facilityId)
        {
            int roleId = this.dbContext.UserRoleFacilityConfigs.Where(u => u.User_Id == userId && u.Facility_id == facilityId && u.UserRole_Status == 1).Select(u => u.Role_ID).FirstOrDefault();
            string RoleName = this.dbContext.Roles.Where(r => r.Role_Id == roleId && r.Role_Status == 1).Select(r => r.Role_Desc).FirstOrDefault();
            return RoleName;
        }

        public List<RoleConfigEntity> GetRoleConfigDetailsByScreenID(RoleScreensEntity Ids)
        {
            string[] screenIds = null;
            if (Ids.Screen_Id != "")
                screenIds = Ids.Screen_Id.ToString().Split(',');
            // var roles = this.dbContext.RoleConfigs.Where(rl => rl.Role_Id == roleID && rl.Screen_Id == ScreenID).ToList();
            // return this.autoMapper.Map<List<RoleConfig>, List<RoleConfigEntity>>(roles);
            var role = (from roleconfigobj in this.dbContext.RoleConfigs
                        join roleobj in this.dbContext.Roles on roleconfigobj.Role_Id equals roleobj.Role_Id
                        join screenobj in this.dbContext.Screens on roleconfigobj.Screen_Id equals screenobj.Screen_Id
                        join user in this.dbContext.Users on roleconfigobj.RoleConfig_CreatedBy equals user.User_Id
                        where roleconfigobj.Role_Id == Ids.Role_Id
                        select new RoleConfigEntity
                        {
                            RoleConfig_Id = roleconfigobj.RoleConfig_Id,
                            Role_Id = roleconfigobj.Role_Id,
                            AccessRead = roleconfigobj.AccessRead,
                            AccessWrite = roleconfigobj.AccessWrite,
                            PrintExcel = roleconfigobj.PrintExcel,
                            PrintPdf = roleconfigobj.PrintPdf,
                            Screen_Id = roleconfigobj.Screen_Id,
                            RoleConfig_CreatedBy = roleconfigobj.RoleConfig_CreatedBy,
                            RoleConfig_CreatedDate = roleconfigobj.RoleConfig_CreatedDate,
                            RoleConfig_Status = roleconfigobj.RoleConfig_Status,
                            CreatedBy = user.User_DisplayName,
                            RoleName = roleobj.Role_Desc,
                            Role_Name = roleobj.Role_Desc,
                            ScreenName = screenobj.Screen_Desc,
                            Screen = screenobj.Screen_Desc,
                            Read = roleconfigobj.AccessRead == 1 ? true : false,
                            Write = roleconfigobj.AccessWrite == 1 ? true : false,
                            Excel = roleconfigobj.PrintExcel == 1 ? true : false,
                            Pdf = roleconfigobj.PrintPdf == 1 ? true : false,
                            PDF = roleconfigobj.PrintPdf == 1 ? true : false,
                            Status = roleconfigobj.RoleConfig_Status == 1 ? "Active" : "InActive"

                        }).OrderBy(item => item.ScreenName).ToList();
            if (screenIds != null && screenIds.Length > 0)
            {
                role = role.Where(rd => screenIds.Contains(rd.Screen_Id.ToString())).ToList();
            }
            return role;

        }
        public UserIdCustomEntity GetUserEmailByUserName(string userName)
        {
            UserIdCustomEntity mailObj = new UserIdCustomEntity();
            var email = this.dbContext.Users.Where(us => us.UserName == userName).FirstOrDefault();
            if (email != null)
            {
                mailObj.User_Email = email.User_Email;
                mailObj.UserId = email.User_Id.ToString();
            }
            return mailObj;
        }
        public UserEntity GetUserDetails(string userName)
        {
            var userDetails = this.dbContext.Users.Where(us => us.UserName == userName).FirstOrDefault();
            return this.autoMapper.Map<User, UserEntity>(userDetails);
        }
        public MailConfigEntity GetMailconfigDetails(int Id)
        {
            var mailConfig = this.dbContext.MailConfigs.Find(Id);
            return this.autoMapper.Map<MailConfig, MailConfigEntity>(mailConfig);
        }
        public RoleConfigEntity GetRoleConfigDetailsByID(int RoleConfig_Id)
        {
            var roleconfigdata = this.dbContext.RoleConfigs.Find(RoleConfig_Id);
            return this.autoMapper.Map<RoleConfig, RoleConfigEntity>(roleconfigdata);
        }
        public List<UserRolesEntity> GetRolesByUser(int userId)
        {
            var roles = (from rl in this.dbContext.Roles
                         join usr in this.dbContext.UserRoleFacilityConfigs on rl.Role_Id equals usr.Role_ID
                         where usr.User_Id == userId
                         select new UserRolesEntity
                         {
                             RoleId = rl.Role_Id,
                             RoleDesc = rl.Role_Desc
                         }).Distinct().ToList();
            return roles;
        }
        public int InsertUserOTP(int userId, string otp, int type)
        {
            var record = this.dbContext.UserOTPs.Where(us => us.UserID == userId && us.Status == 1).FirstOrDefault();
            if (record != null)
            {
                record.Status = 2;
                this.dbContext.SaveChanges();
            }
            UserOTP otpObj = new UserOTP();
            otpObj.UserID = userId;
            otpObj.OTP = otp;
            otpObj.LoginDate = DateTime.Now;
            otpObj.LoginType = type;
            otpObj.Status = 1;
            this.dbContext.UserOTPs.Add(otpObj);
            this.dbContext.SaveChanges();
            return 1;
        }
        public string GetUserOTPCheckStatus(int UserId, string OTP)
        {
            string status = string.Empty;
            var userOTP = this.dbContext.UserOTPs.Where(uo => uo.UserID == UserId && uo.Status == 1).FirstOrDefault();
            if (userOTP != null)
            {
                int attempt = Convert.ToInt16(userOTP.Attempts.ToString() == "" ? "0" : userOTP.Attempts.ToString());
                userOTP.Attempts = attempt + 1;
                this.dbContext.SaveChanges();
                DateTime otpDate = Convert.ToDateTime(userOTP.LoginDate);
                TimeSpan span = DateTime.Now.Subtract(otpDate);
                if (span.Minutes < 10)
                {
                    if (userOTP.OTP == OTP)
                    {
                        status = "Valid";
                    }
                    else
                    {
                        status = "Invalid OTP";
                        if (userOTP.Attempts == 3)
                        {
                            userOTP.Status = 2;
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    status = "OTP Expired";
                }
            }
            else
            {
                status = "OTP Expired";
            }
            return status;
        }
        public string GetLockOTPCheckStatus(int UserId, string OTP)
        {
            string status = string.Empty;
            var userOTP = this.dbContext.UserOTPs.Where(uo => uo.UserID == UserId && uo.Status == 1).FirstOrDefault();
            if (userOTP != null)
            {
                int attempt = Convert.ToInt16(userOTP.Attempts.ToString() == "" ? "0" : userOTP.Attempts.ToString());
                userOTP.Attempts = attempt + 1;
                this.dbContext.SaveChanges();
                DateTime otpDate = Convert.ToDateTime(userOTP.LoginDate);
                TimeSpan span = DateTime.Now.Subtract(otpDate);
                if (span.Minutes < 10)
                {
                    if (userOTP.OTP == OTP)
                    {
                        var record = this.dbContext.Users.Where(us => us.User_Id == UserId).FirstOrDefault();
                        record.User_PwdCount = 0;
                        record.User_Lock = 0;
                        this.dbContext.SaveChanges();
                        status = "User Unlocked Successfully";
                    }
                    else
                    {
                        status = "Invalid OTP";

                        if (userOTP.Attempts == 3)
                        {
                            userOTP.Status = 2;
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    status = "OTP Expired";
                }
            }
            else
            {
                status = "OTP Expired";
            }
            return status;
        }
        public int ResetPassword(int UserId, string PWD)
        {
            var reset = this.dbContext.Users.Where(u => u.User_Id == UserId).FirstOrDefault();
            if (reset != null)
            {
                User user = this.dbContext.Users.Find(reset.User_Id);
                user.Password = PWD;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public int UpdateNewUserpassword(string userName, string password, string newPassword)
        {
            var user = this.dbContext.Users.Where(u => u.UserName == userName && u.User_Status == 1).FirstOrDefault();
            if (user != null && user.Password == password)
            {
                user.Password = newPassword;
                user.NewUserFlag = 0;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int UpdateRoleConfigsStatus(List<RoleConfigEntity> data)
        {
            foreach (var item in data)
            {
                item.RoleConfig_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.RoleConfigs.Find(item.RoleConfig_Id);
                if (record != null)
                {
                    record.RoleConfig_Status = record.RoleConfig_Status == 1 ? 0 : 1;
                    record.RoleConfig_CreatedBy = item.RoleConfig_CreatedBy;
                    record.RoleConfig_CreatedDate = item.RoleConfig_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateRolesStatus(List<RoleEntity> data)
        {
            foreach (var item in data)
            {
                item.Role_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.Roles.Find(item.Role_Id);
                if (record != null)
                {
                    record.Role_Status = record.Role_Status == 1 ? 0 : 1;
                    record.Role_CreatedBy = item.Role_CreatedBy;
                    record.Role_CreatedDate = item.Role_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public List<ScreenEntity> GetDefaultScreenDropData()
        {
            List<int> subscreens = new List<int>();
            var defaultScreens = this.dbContext.DefaultScreens.Where(d => d.DefaultScreen_Status == 1).Select(r => (int)r.Screen_Id).Distinct().ToArray();
            var screens = this.dbContext.Screens.Select(r => (int)r.Screen_Id).Distinct().ToArray();
            var items = screens.Except(defaultScreens);
            int roleId;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("RoleId").Value != "")
            {
                 roleId= Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                //foreach (ScreenEntity.SubScreens item in Enum.GetValues(typeof(ScreenEntity.SubScreens)))
                //{
                //    subscreens.Add(Convert.ToInt32(item));
                //}
                var records = (from sc in this.dbContext.Screens
                               join rc in this.dbContext.RoleConfigs on sc.Screen_Id equals rc.Screen_Id
                               where items.Contains(sc.Screen_Id) && rc.Role_Id == roleId  && sc.Screen_Status == 1
                               //!subscreens.Contains(sc.Screen_Id)
                               select new ScreenEntity
                               {
                                   Screen_Id = sc.Screen_Id,
                                   Screen_Desc = sc.Screen_Desc,
                               }).OrderBy(item => item.Screen_Desc).ToList();
                return records;
            }
            return null;
        }
        public int InsertDefaultScreen(DefaultScreenEntity data)
        {
            var defaultScreen = this.dbContext.DefaultScreens.Where(d => d.Screen_Id == data.Screen_Id).FirstOrDefault();
            if (defaultScreen != null)
            {
                if (data.DefaultScreen_Id != 0)
                {
                    var defaultScreenId = this.dbContext.DefaultScreens.Where(d => d.Screen_Id == data.Screen_Id).Select(d => d.DefaultScreen_Id).FirstOrDefault();
                    var checkDefaultScreen = this.dbContext.Roles.Where(r => r.DefaultScreen_Id == defaultScreenId).ToList();
                    if (checkDefaultScreen.Count != 0)
                    {
                        // Default Screen already assigned to one can't remove
                        return 2;
                    }
                    else
                    {
                        defaultScreen.DefaultScreen_Status = data.DefaultScreen_Status;
                        defaultScreen.DefaultScreen_CreatedBy = data.DefaultScreen_CreatedBy;
                        defaultScreen.DefaultScreen_CreatedOn = data.DefaultScreen_CreatedOn;
                        this.dbContext.SaveChanges();
                        return 1;
                    }
                }
                else if (data.DefaultScreen_Id == 0)
                {
                    defaultScreen.DefaultScreen_Status = data.DefaultScreen_Status;
                    defaultScreen.DefaultScreen_CreatedBy = data.DefaultScreen_CreatedBy;
                    defaultScreen.DefaultScreen_CreatedOn = data.DefaultScreen_CreatedOn;
                    this.dbContext.SaveChanges();
                    return 1;
                }
            }
            else
            {
                var record = this.autoMapper.Map<DefaultScreenEntity, DefaultScreen>(data);
                this.dbContext.DefaultScreens.Add(record);
                this.dbContext.SaveChanges();
            }
            return 1;
        }
    }
}
