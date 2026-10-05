using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;

namespace LTCPro.Repositories
{
    public class FTEConfigurationRepository : IFTEConfigurationRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public FTEConfigurationRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }

        public List<FTConfigurationGridEntity> GetAllFTEConfigurationsList()
        {
            var fteConfigs = (from ftc in this.dbContext.FTEConfigurations
                              join cat in this.dbContext.FTECategories on ftc.Category equals cat.FteCategory_Id
                              join con in this.dbContext.FteConnections on ftc.ConnectionType equals con.FteConn_Id
                              orderby ftc.Category
                              select new FTConfigurationGridEntity()
                              {
                                  FteConfig_Id = ftc.FteConfig_Id,
                                  Category = cat.FteCategory_Desc,
                                  ConnectionType = con.FteConn_Desc,
                                  ServerIp = ftc.ServerIp,
                                  Port = ftc.Port,
                                  FteConfig_Status = ftc.FteConfig_Status,
                                  ConnectionStatus=ftc.FteConnectionStatus
                              }).ToList();
            return fteConfigs;
        }

        public FTEConfigurationEntity GetFTEConfigurationDetailsByID(int fteConfigId)
        {
            var fteConfig = this.dbContext.FTEConfigurations.Find(fteConfigId);
            return this.autoMapper.Map<FTEConfiguration, FTEConfigurationEntity>(fteConfig);
        }

        public int InsertUpdateFTEConfiguration(FTEConfigurationEntity entity)
        {
            entity.FteConfig_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<FTEConfigurationEntity, FTEConfiguration>(entity);
            if (entity.FteConfig_Id == 0 ? this.dbContext.FTEConfigurations.Any(f => f.Category == entity.Category && f.ConnectionType == entity.ConnectionType && f.ServerIp==entity.ServerIp) : this.dbContext.FTEConfigurations.Any(f => f.Category == entity.Category && f.ConnectionType == entity.ConnectionType && f.ServerIp == entity.ServerIp && f.FteConfig_Id != entity.FteConfig_Id))
            {
                //UserName already exisits
                return 2;
            }

            if (record.FteConfig_Id==0)
            {
                //if(this.dbContext.FTEConfigurations.Any(f=>f.Category==entity.Category))
                //{
                //    //FTEConfiguration already exisits
                //    return 2;
                //}
                //else
                //{
                    this.dbContext.FTEConfigurations.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.FTPConfiguration,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.FteConfig_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
               // }   
            }
            else
            {
                //FTEConfiguration FTECon = this.dbContext.FTEConfigurations.Where(f =>f.Category == entity.Category).FirstOrDefault();
                FTEConfiguration FTECon = this.dbContext.FTEConfigurations.Where(f => f.FteConfig_Id == record.FteConfig_Id).FirstOrDefault();
                //FTECon.Company_Id = record.Company_Id;
                FTECon.ServerIp = record.ServerIp;
                FTECon.Category = record.Category;
                FTECon.ConnectionType = record.ConnectionType;
                FTECon.Port = record.Port;
                FTECon.UserName = record.UserName;
                FTECon.Password = record.Password;
                FTECon.FteConfig_Status = record.FteConfig_Status;
                FTECon.FteConfig_CreatedBy = record.FteConfig_CreatedBy;
                FTECon.FteConfig_CreatedDate = record.FteConfig_CreatedDate;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FTPConfiguration,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.FteConfig_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }
        public List<string> GetFilesList()
        {
            var filesNames = this.dbContext.FileInformations.Select(f=>f.File_Name).ToList();
            return filesNames;
        }
    }
}
