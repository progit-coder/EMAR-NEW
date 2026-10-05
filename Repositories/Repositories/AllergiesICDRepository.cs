using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;

namespace LTCPro.Repositories
{
    public class AllergiesICDRepository : IAllergiesICDRepository
    {

        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public AllergiesICDRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }

        public List<AllergyInfoMastersEntity> GetAllergiesByName(string searchPattern, int type)
        {
            var allergies = (from allergy in dbContext.AllergyInfoMasters
                             join user in this.dbContext.Users on allergy.Allergy_CreatedBy equals user.User_Id
                             where (allergy.AllergyDesc_Id.Contains(searchPattern) || allergy.AllergyDesc.Contains(searchPattern)) && allergy.AllergyTypeMaster_Id == type
                             select new AllergyInfoMastersEntity
                             {
                                 Allergy_Id = allergy.Allergy_Id,
                                 AllergyDesc_Id = allergy.AllergyDesc_Id,
                                 AllergyDesc = allergy.AllergyDesc,
                                 AllergyTypeMaster_Id = allergy.AllergyTypeMaster_Id,
                                 Allergy_CreatedBy = allergy.Allergy_CreatedBy,
                                 Allergy_CreatedOn = allergy.Allergy_CreatedOn,
                                 Allergy_Status = allergy.Allergy_Status,
                                 AllergyStatus = allergy.Allergy_Status== 1 ? "Active" : "Inactive",
                                 CreatedBy = user.User_DisplayName
                             }).ToList(); 
            return allergies;
        }

        public List<AllergyInfoMastersEntity> GetActiveAllergiesByName(string searchPattern)
        {
            var allergies = (from allergy in dbContext.AllergyInfoMasters
                             join user in this.dbContext.Users on allergy.Allergy_CreatedBy equals user.User_Id
                             where (allergy.AllergyDesc_Id.Contains(searchPattern) || allergy.AllergyDesc.Contains(searchPattern)) 
                              && allergy.Allergy_Status == 1
                             select new AllergyInfoMastersEntity
                             {
                                 Allergy_Id = allergy.Allergy_Id,
                                 AllergyDesc_Id = allergy.AllergyDesc_Id,
                                 AllergyDesc = allergy.AllergyDesc,
                                 AllergyTypeMaster_Id = allergy.AllergyTypeMaster_Id,
                                 Allergy_CreatedBy = allergy.Allergy_CreatedBy,
                                 Allergy_CreatedOn = allergy.Allergy_CreatedOn,
                                 Allergy_Status = allergy.Allergy_Status,
                                 AllergyStatus = allergy.Allergy_Status == 1 ? "Active" : "Inactive",
                                 CreatedBy = user.UserName
                             }).ToList();
            return allergies;
        }
        public List<AllergyInfoMastersEntity> GetAllAllergiesList(int type)
        {
            var allergies = (from allergy in dbContext.AllergyInfoMasters
                             join user in this.dbContext.Users on allergy.Allergy_CreatedBy equals user.User_Id
                             where allergy.AllergyTypeMaster_Id == type
                             select new AllergyInfoMastersEntity
                             {
                                 AllergyTypeMaster_Id=allergy.AllergyTypeMaster_Id,
                                 AllergyDesc_Id = allergy.AllergyDesc_Id,
                                 AllergyDesc = allergy.AllergyDesc,
                                 Allergy_CreatedBy = allergy.Allergy_CreatedBy,
                                 Allergy_CreatedOn = allergy.Allergy_CreatedOn,
                                 Allergy_Id = allergy.Allergy_Id,
                                 Allergy_Status = allergy.Allergy_Status,
                                 AllergyStatus = allergy.Allergy_Status == 1 ? "Active" : "Inactive",
                                 CreatedBy = user.User_DisplayName
                             }).ToList();
            return allergies;
        }

        public List<ICD10Entity> GetAllICD10()
        {
            var allergies = (from icd in this.dbContext.ICD10
                             join user in this.dbContext.Users on icd.ICD10_CreatedBy equals user.User_Id
                             select new ICD10Entity
                             {
                                 ICD10_CreatedBy = icd.ICD10_CreatedBy,
                                 ICD10_CreatedDate = icd.ICD10_CreatedDate,
                                 ICD10_Description = icd.ICD10_Description,
                                 ICD10_Formatted = icd.ICD10_Formatted,
                                 ICD10_Id = icd.ICD10_Id,
                                 ICD10_RawFormat = icd.ICD10_RawFormat,
                                 ICD10_Status = icd.ICD10_Status,
                                 CreatedBy = user.UserName,
                                 Status=icd.ICD10_Status==1? "Active":"Inactive"

                             }).ToList();
            return allergies;
        }
        public List<ICD10Entity> GetICDGrid()
        {
            var data = (from i in this.dbContext.ICD10
                        join user in this.dbContext.Users on i.ICD10_CreatedBy equals user.User_Id
                        orderby i.ICD10_CreatedDate descending
                        select new ICD10Entity
                        {
                            ICD10_Id = i.ICD10_Id,
                            ICD10_CreatedBy = i.ICD10_CreatedBy,
                            ICD10_RawFormat = i.ICD10_RawFormat,
                            ICD10_Formatted = i.ICD10_Formatted,
                            ICD10_Description = i.ICD10_Description,
                            ICD10_Status = i.ICD10_Status,
                            ICD10_CreatedDate = i.ICD10_CreatedDate,
                            CreatedBy = user.User_DisplayName,
                            Status = i.ICD10_Status == 1 ? "Active" : "Inactive"
                        }).Take(100).OrderBy(item=>item.ICD10_RawFormat).ToList();
                        return data;
        }
       

        public ICD10GridEntity GetICD10ByRawFormat(SearchCustomEntity data)
        {
            var count = 0;
            var records = new List<ICD10Entity>();
            string searchText = data.SearchText != null && data.SearchText != string.Empty ? data.SearchText.ToLower() : string.Empty;
            if (searchText != string.Empty)
            {
                count = (from icd in this.dbContext.ICD10
                         join user in this.dbContext.Users on icd.ICD10_CreatedBy equals user.User_Id
                         where (icd.ICD10_RawFormat.Contains(data.SearchText) || icd.ICD10_Formatted.Contains(data.SearchText) || icd.ICD10_Description.Contains(data.SearchText))
                         && icd.ICD10_Status == data.Status
                         select icd.ICD10_Id).Distinct().Count();

                int skipRows = (data.CurrentPage - 1) * data.PageSize;
                records = (from icd in this.dbContext.ICD10
                           join user in this.dbContext.Users on icd.ICD10_CreatedBy equals user.User_Id
                           where (icd.ICD10_RawFormat.Contains(data.SearchText) || icd.ICD10_Formatted.Contains(data.SearchText) || icd.ICD10_Description.Contains(data.SearchText))
                           && icd.ICD10_Status == data.Status
                           select new ICD10Entity
                           {

                               ICD10_Description = icd.ICD10_Description,
                               ICD10_Formatted = icd.ICD10_Formatted,
                               ICD10_Id = icd.ICD10_Id,
                               ICD10_RawFormat = icd.ICD10_RawFormat,
                               ICD10_Status = icd.ICD10_Status,
                               

                           }).OrderBy(item => item.ICD10_RawFormat).Skip(skipRows).Take(data.PageSize).AsEnumerable().ToList();
            }
            else
            {
                count = (from icd in this.dbContext.ICD10
                         join user in this.dbContext.Users on icd.ICD10_CreatedBy equals user.User_Id
                         where icd.ICD10_Status == data.Status
                         select icd.ICD10_Id).Distinct().Count();

                int skipRows = (data.CurrentPage - 1) * data.PageSize;
                records = (from icd in this.dbContext.ICD10
                           join user in this.dbContext.Users on icd.ICD10_CreatedBy equals user.User_Id
                           where icd.ICD10_Status == data.Status
                           select new ICD10Entity
                           {

                               ICD10_Description = icd.ICD10_Description,
                               ICD10_Formatted = icd.ICD10_Formatted,
                               ICD10_Id = icd.ICD10_Id,
                               ICD10_RawFormat = icd.ICD10_RawFormat,
                               ICD10_Status = icd.ICD10_Status,

                           }).OrderBy(item => item.ICD10_RawFormat).Skip(skipRows).Take(data.PageSize).AsEnumerable().ToList();
            }
            var list = new ICD10GridEntity
            {
                TotalRecords = count,
                Data = records
            };
            return list;
        }
        public List<ICD10Entity> GetActiveICD10ByRawFormat(string searchPattern)
        {
            var data = (from icd in this.dbContext.ICD10
                        join user in this.dbContext.Users on icd.ICD10_CreatedBy equals user.User_Id
                        where icd.ICD10_Description.Contains(searchPattern)
                        && icd.ICD10_Status == 1
                        select new ICD10Entity
                        {

                            ICD10_CreatedBy = icd.ICD10_CreatedBy,
                            ICD10_CreatedDate = icd.ICD10_CreatedDate,
                            ICD10_Description = icd.ICD10_Description,
                            ICD10_Formatted = icd.ICD10_Formatted,
                            ICD10_Id = icd.ICD10_Id,
                            ICD10_RawFormat = icd.ICD10_RawFormat,
                            ICD10_Status = icd.ICD10_Status,
                            CreatedBy = user.UserName,
                            Status = icd.ICD10_Status == 1 ? "Active" : "Inactive"

                        }).ToList();
            return data;
        }

        public int InsertUpdateAllergy(AllergyInfoMastersEntity entity)
        {
            entity.Allergy_CreatedOn = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<AllergyInfoMastersEntity, AllergyInfoMaster>(entity);
            if (record.Allergy_Id == 0)
            {
                AllergyInfoMaster allergies = this.dbContext.AllergyInfoMasters.Where(e => e.AllergyDesc_Id == entity.AllergyDesc_Id && e.AllergyDesc == entity.AllergyDesc).FirstOrDefault();
                if (allergies == null)
                {
                    this.dbContext.AllergyInfoMasters.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.AllergyMaster,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.Allergy_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
                else
                    //Name already exists
                    return 2;
            }            
            else
            {
                AllergyInfoMaster allergies = this.dbContext.AllergyInfoMasters.Find(entity.Allergy_Id);
                allergies.AllergyTypeMaster_Id = record.AllergyTypeMaster_Id;
                allergies.AllergyDesc_Id = record.AllergyDesc_Id;
                allergies.AllergyDesc = record.AllergyDesc;
                allergies.Allergy_Status = record.Allergy_Status;
                allergies.Allergy_CreatedBy = record.Allergy_CreatedBy;
                allergies.Allergy_CreatedOn = record.Allergy_CreatedOn;

                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.AllergyMaster,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Allergy_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
         }

        public int InsertUpdateAllergyFromFile(List<AllergyInfoMastersEntity> entities)
        {
            var records = this.autoMapper.Map<List<AllergyInfoMastersEntity>, List<AllergyInfoMaster>>(entities);
            var missingRecords = records.Where(x => !this.dbContext.AllergyInfoMasters.Any(z => z.AllergyDesc == x.AllergyDesc)).ToList();
            this.dbContext.AllergyInfoMasters.AddRange(missingRecords);
            this.dbContext.SaveChanges();

            return missingRecords.Count;
        }

        public int InsertUpdateICD10(ICD10Entity entity)
        {
            entity.ICD10_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
            var record = this.autoMapper.Map<ICD10Entity, ICD10>(entity);
            if (record.ICD10_Id == 0)
            {
                ICD10 icd = this.dbContext.ICD10.Where(e => e.ICD10_RawFormat == entity.ICD10_RawFormat && e.ICD10_Formatted == entity.ICD10_Formatted).FirstOrDefault();
                if (icd == null)
                {
                    this.dbContext.ICD10.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.ICD10Master,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.ICD10_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
                else
                    //Name already exist
                    return 2;
            }
            else
            {
                ICD10 icd = this.dbContext.ICD10.Find(entity.ICD10_Id);
                icd.ICD10_Id = record.ICD10_Id;
                icd.ICD10_RawFormat = record.ICD10_RawFormat;
                icd.ICD10_Formatted = record.ICD10_Formatted;
                icd.ICD10_Description = record.ICD10_Description;
                icd.ICD10_Status = record.ICD10_Status;
                icd.ICD10_CreatedBy = record.ICD10_CreatedBy;
                icd.ICD10_CreatedDate = record.ICD10_CreatedDate;

                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ICD10Master,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.ICD10_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
        }

        public int InsertUpdateICD10FromFile(List<ICD10Entity> entities)
        {
            var records = this.autoMapper.Map<List<ICD10Entity>, List<ICD10>>(entities);
            var icd = this.dbContext.ICD10.ToList();
            var missingRecords = records.Where(x => !icd.Any(z => z.ICD10_RawFormat == x.ICD10_RawFormat && z.ICD10_Formatted == x.ICD10_Formatted)).ToList();
            this.dbContext.ICD10.AddRange(missingRecords);
            this.dbContext.SaveChanges();

            return missingRecords.Count;
        }
        public int UpdateICDsStatus(List<ICD10Entity> data)
        {
            foreach (var item in data)
            {
                item.ICD10_CreatedDate = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.ICD10.Find(item.ICD10_Id);
                if (record != null)
                {
                    record.ICD10_Status = record.ICD10_Status == 1 ? 0 : 1;
                    record.ICD10_CreatedBy = item.ICD10_CreatedBy;
                    record.ICD10_CreatedDate = item.ICD10_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateAllergysStatus(List<AllergyInfoMastersEntity> data)
        {
            foreach (var item in data)
            {
                item.Allergy_CreatedOn = Convert.ToDateTime(this.dbContext.OrderTypes.Select(q => DateTime.Now).FirstOrDefault());
                var record = this.dbContext.AllergyInfoMasters.Find(item.Allergy_Id);
                if (record != null)
                {
                    record.Allergy_Status = record.Allergy_Status == 1 ? 0 : 1;
                    record.Allergy_CreatedBy = item.Allergy_CreatedBy;
                    record.Allergy_CreatedOn = item.Allergy_CreatedOn;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
    }
}
