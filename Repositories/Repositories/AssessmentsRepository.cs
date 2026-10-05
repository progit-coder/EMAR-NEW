using LTCPro.Entities;
using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public class AssessmentsRepository : IAssessmentsRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly ICommonRepository _commonRepository;

        public AssessmentsRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, ICommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
            this._commonRepository = commonRepository;
        }
        public int InsertUpdateWeight(WeightLogEntity weightLog)
        {
            weightLog.WeightLog_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<WeightLogEntity, WeightLog>(weightLog);
            if (record.WeightLog_ID == 0)
            {
                this.dbContext.WeightLogs.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FrequencyMapping,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.WeightLog_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                WeightLog weightlog = this.dbContext.WeightLogs.Find(weightLog.WeightLog_ID);
                weightlog.WeightLog_ID = record.WeightLog_ID;
                weightlog.Patient_Id = record.Patient_Id;
                weightlog.Weight = record.Weight;
                weightlog.Type = record.Type;
                weightlog.HeightFeet = record.HeightFeet;
                weightlog.HeightInc = record.HeightInc;
                weightlog.DateTime = record.DateTime;
                weightlog.PreDialysis = record.PreDialysis;
                weightlog.PostDialysis = record.PostDialysis;
                weightlog.Remarks = record.Remarks;
                weightlog.IBW = record.IBW;
                weightlog.initials = record.initials;
                weightlog.WeightLog_Status = record.WeightLog_Status;
                weightlog.WeightLog_CreatedBy = record.WeightLog_CreatedBy;
                weightlog.WeightLog_CreatedOn = record.WeightLog_CreatedOn;


                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.WeightLog,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = weightLog.WeightLog_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);

                this.dbContext.SaveChanges();

                return 1;
            }
        }
        public List<WeightLogEntity> GetWeightList(int Patient_Id)
        {
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == Patient_Id).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var weightlogs = (from weight in this.dbContext.WeightLogs
                              join user in this.dbContext.Users on weight.WeightLog_CreatedBy equals user.User_Id
                              where weight.Patient_Id == Patient_Id
                              select new //WeightLogEntity
                              {
                                  weight = weight,
                                  user = user,
                                  //WeightLog_ID = weight.WeightLog_ID,
                                  //Patient_Id = weight.Patient_Id,
                                  //Weight = weight.Weight,
                                  //Type = weight.Type=="1"?"Kg":"Lb",
                                  //HeightFeet = weight.HeightFeet,
                                  //HeightInc = weight.HeightInc,
                                  //DateTime = weight.DateTime,
                                  //PreDialysis = weight.PreDialysis,
                                  //PostDialysis = weight.PostDialysis,
                                  //Remarks = weight.Remarks,
                                  //IBW = weight.IBW,
                                  //initials = weight.initials,
                                  //WeightLog_Status = weight.WeightLog_Status,
                                  //WeightLog_CreatedBy = weight.WeightLog_CreatedBy,
                                  //WeightLog_CreatedOn = weight.WeightLog_CreatedOn,
                                  //UserName = user.User_DisplayName

                              }).ToList()
                        .Select(x => new WeightLogEntity
                        {
                            WeightLog_ID = x.weight.WeightLog_ID,
                            Patient_Id = x.weight.Patient_Id,
                            Weight = x.weight.Weight,
                            Type = x.weight.Type == "1" ? "Kg" : "Lb",
                            HeightFeet = x.weight.HeightFeet,
                            HeightInc = x.weight.HeightInc,
                            DateTime = x.weight.DateTime,
                            PreDialysis = x.weight.PreDialysis,
                            PostDialysis = x.weight.PostDialysis,
                            Remarks = x.weight.Remarks,
                            IBW = x.weight.IBW,
                            initials = x.weight.initials,
                            WeightLog_Status = x.weight.WeightLog_Status,
                            WeightLog_CreatedBy = x.weight.WeightLog_CreatedBy,
                            WeightLog_CreatedOn =GetTimeZoneDateTime(x.weight.WeightLog_CreatedOn,companyId),
                            UserName = x.user.User_DisplayName
                        }).ToList();
            return weightlogs;
        }
        public WeightLogEntity GetWeightDetailsByID(int WeightLog_ID)
        {
            var weight = this.dbContext.WeightLogs.Find(WeightLog_ID);
            return this.autoMapper.Map<WeightLog, WeightLogEntity>(weight);
        }
        public List<AdmitDateEntity> GetAdmitDateByID(int Patient_Id)
        {
            var admit = (from da in this.dbContext.VisitInfoes
                         where da.Patient_Id == Patient_Id
                         select new AdmitDateEntity
                         {
                             Patient_Id = da.Patient_Id,
                             PVisit_Id = da.PVisit_Id,
                             AdmitDate = da.AdmitDate,
                         }).ToList();
            return admit;
        }
        public int insertUpdateVisitBehaviour(VisitBehaviourEntity visitBehaviour)
        {
            visitBehaviour.VisitBehaviour_CreatedDate= Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<VisitBehaviourEntity, VisitBehaviour>(visitBehaviour);
            if (record.VisitBehaviour_ID == 0)
            {
                this.dbContext.VisitBehaviours.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Behaviour,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.VisitBehaviour_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                VisitBehaviour visitbehaviour = this.dbContext.VisitBehaviours.Find(visitBehaviour.VisitBehaviour_ID);
                visitbehaviour.PVisit_Id = record.PVisit_Id;
                visitbehaviour.HallucinationsID = record.HallucinationsID;
                visitbehaviour.DelusionsID = record.DelusionsID;
                visitbehaviour.Physicalbehavioral = record.Physicalbehavioral;
                visitbehaviour.Verbalbehavioral = record.Verbalbehavioral;
                visitbehaviour.Otherbehavioral = record.Otherbehavioral;
                visitbehaviour.rejectevaluation = record.rejectevaluation;
                visitbehaviour.Resisdentwandered = record.Resisdentwandered;
                visitbehaviour.Comments = record.Comments;
                visitbehaviour.Initials = record.Initials;
                visitbehaviour.Date = record.Date;
                visitbehaviour.WeeklyStatus = record.WeeklyStatus;
                visitbehaviour.VisitBehaviour_Status = record.VisitBehaviour_Status;
                visitbehaviour.VisitBehaviour_CreatedBy = record.VisitBehaviour_CreatedBy;
                visitbehaviour.VisitBehaviour_CreatedDate = record.VisitBehaviour_CreatedDate;

                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Behaviour,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = visitBehaviour.VisitBehaviour_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }

        }
        public List<VisitBehaviourEntity> GetVisitBehaviourList(int Patient_Id)
        {
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == Patient_Id).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            //12/09/2022 chnaged companyid to facilityid
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var data = (from visitbehaviour in this.dbContext.VisitBehaviours
                        join visitinfo in this.dbContext.VisitInfoes on visitbehaviour.PVisit_Id equals visitinfo.PVisit_Id
                        join user in this.dbContext.Users on visitbehaviour.VisitBehaviour_CreatedBy equals user.User_Id
                        join pbehave in this.dbContext.BehavioralSymptomsMasters on visitbehaviour.Physicalbehavioral equals pbehave.BehavioralSym_ID
                        join vbehave in this.dbContext.BehavioralSymptomsMasters on visitbehaviour.Verbalbehavioral equals vbehave.BehavioralSym_ID
                        join obehave in this.dbContext.BehavioralSymptomsMasters on visitbehaviour.Otherbehavioral equals obehave.BehavioralSym_ID
                        join rbehave in this.dbContext.BehavioralSymptomsMasters on visitbehaviour.rejectevaluation equals rbehave.BehavioralSym_ID
                        join wbehave in this.dbContext.BehavioralSymptomsMasters on visitbehaviour.Resisdentwandered equals wbehave.BehavioralSym_ID
                        where visitinfo.Patient_Id == Patient_Id
                        select new //VisitBehaviourEntity
                        {
                            visitBehaviour = visitbehaviour,
                            user = user,
                            phyBeha=pbehave,
                            verBeha=vbehave,
                            otherBeha=obehave,
                            rejeBeha=rbehave,
                            resBeha=wbehave,
                            //VisitBehaviour_ID = visitbehaviour.VisitBehaviour_ID,
                            //PVisit_Id = visitbehaviour.PVisit_Id,
                            //HallucinationsID = visitbehaviour.HallucinationsID,
                            //DelusionsID = visitbehaviour.DelusionsID,
                            //Physicalbehavioral = visitbehaviour.Physicalbehavioral,
                            //Verbalbehavioral = visitbehaviour.Verbalbehavioral,
                            //Otherbehavioral = visitbehaviour.Otherbehavioral,
                            //rejectevaluation = visitbehaviour.rejectevaluation,
                            //Resisdentwandered = visitbehaviour.Resisdentwandered,
                            //PhysicalbehavioralDesc=pbehave.BehavioralSym_Desc,
                            //VerbalbehavioralDesc= vbehave.BehavioralSym_Desc,
                            //OtherbehavioralDesc=obehave.BehavioralSym_Desc,
                            //rejectevaluationDesc=rbehave.BehavioralSym_Desc,
                            //ResisdentwanderedDesc=wbehave.BehavioralSym_Desc,
                            //Comments = visitbehaviour.Comments,
                            //Initials = visitbehaviour.Initials,
                            //Date = visitbehaviour.Date,
                            //WeeklyStatus = visitbehaviour.WeeklyStatus,
                            //VisitBehaviour_Status = visitbehaviour.VisitBehaviour_Status,
                            //VisitBehaviour_CreatedBy = visitbehaviour.VisitBehaviour_CreatedBy,
                            //VisitBehaviour_CreatedDate = visitbehaviour.VisitBehaviour_CreatedDate,
                            //UserName = user.User_DisplayName

                        }).ToList()
                        .Select(x => new VisitBehaviourEntity
                        {
                            VisitBehaviour_ID = x.visitBehaviour.VisitBehaviour_ID,
                            PVisit_Id = x.visitBehaviour.PVisit_Id,
                            HallucinationsID = x.visitBehaviour.HallucinationsID,
                            DelusionsID = x.visitBehaviour.DelusionsID,
                            Physicalbehavioral = x.visitBehaviour.Physicalbehavioral,
                            Verbalbehavioral = x.visitBehaviour.Verbalbehavioral,
                            Otherbehavioral = x.visitBehaviour.Otherbehavioral,
                            rejectevaluation = x.visitBehaviour.rejectevaluation,
                            Resisdentwandered = x.visitBehaviour.Resisdentwandered,
                            PhysicalbehavioralDesc = x.phyBeha.BehavioralSym_Desc,
                            VerbalbehavioralDesc = x.verBeha.BehavioralSym_Desc,
                            OtherbehavioralDesc = x.otherBeha.BehavioralSym_Desc,
                            rejectevaluationDesc = x.rejeBeha.BehavioralSym_Desc,
                            ResisdentwanderedDesc = x.resBeha.BehavioralSym_Desc,
                            Comments = x.visitBehaviour.Comments,
                            Initials = x.visitBehaviour.Initials,
                            Date = x.visitBehaviour.Date,
                            WeeklyStatus = x.visitBehaviour.WeeklyStatus,
                            VisitBehaviour_Status = x.visitBehaviour.VisitBehaviour_Status,
                            VisitBehaviour_CreatedBy = x.visitBehaviour.VisitBehaviour_CreatedBy,
                            VisitBehaviour_CreatedDate =GetTimeZoneDateTime(x.visitBehaviour.VisitBehaviour_CreatedDate,companyId),
                            UserName = x.user.User_DisplayName
                        }).ToList();
            return data;

            
        }
        public List<BehavioralSymptomsMasterEntity> GetBehaviourDropData()
        {
            var behaviourDropData = this.dbContext.BehavioralSymptomsMasters.ToList();
            return this.autoMapper.Map<List<BehavioralSymptomsMaster>, List<BehavioralSymptomsMasterEntity>>(behaviourDropData);
        }
        public VisitBehaviourEntity GetBehaviourDetailsByID(int VisitBehaviourId)
        {
            var behaviordata = this.dbContext.VisitBehaviours.Find(VisitBehaviourId);
            return this.autoMapper.Map<VisitBehaviour, VisitBehaviourEntity>(behaviordata);
        }
        public int insertUpdateVisitFoodIn(VisitFoodintakeEntity visitiFoodIn)
        {
            visitiFoodIn.VisitFoodintake_CreatedDate= Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<VisitFoodintakeEntity, VisitFoodintake>(visitiFoodIn);
            if (record.VisitFoodintake_ID == 0)
            {
                VisitFoodintake visitfoodin = this.dbContext.VisitFoodintakes.Where(e => e.AssessmentDate == visitiFoodIn.AssessmentDate && e.PVisit_Id == visitiFoodIn.PVisit_Id).FirstOrDefault();
                if (visitfoodin != null)
                {
                    //Data already exisits
                    visitfoodin.PVisit_Id = record.PVisit_Id;
                    visitfoodin.AssessmentDate = record.AssessmentDate;

                    visitfoodin.Initials = visitiFoodIn.Initials == null ? visitfoodin.Initials: visitiFoodIn.Initials == "" ? visitfoodin.Initials: visitiFoodIn.Initials;
                    visitfoodin.Attendingphysician = visitiFoodIn.Attendingphysician == null ? visitfoodin.Attendingphysician : visitiFoodIn.Attendingphysician == "" ? visitfoodin.Attendingphysician : visitiFoodIn.Attendingphysician;
                    visitfoodin.Fluidsb = visitiFoodIn.Fluidsb == null ? visitfoodin.Fluidsb : visitiFoodIn.Fluidsb == "" ? visitfoodin.Fluidsb : visitiFoodIn.Fluidsb;
                    visitfoodin.Alternateb = visitiFoodIn.Alternateb == null ? visitfoodin.Alternateb : visitiFoodIn.Alternateb == "" ? visitfoodin.Alternateb : visitiFoodIn.Alternateb;
                    visitfoodin.supplementb = visitiFoodIn.supplementb == null ? visitfoodin.supplementb : visitiFoodIn.supplementb == "" ? visitfoodin.supplementb : visitiFoodIn.supplementb;
                    visitfoodin.Fluidsl = visitiFoodIn.Fluidsl == null ? visitfoodin.Fluidsl : visitiFoodIn.Fluidsl == "" ? visitfoodin.Fluidsl : visitiFoodIn.Fluidsl;
                    visitfoodin.Alternatel = visitiFoodIn.Alternatel == null ? visitfoodin.Alternatel : visitiFoodIn.Alternatel == "" ? visitfoodin.Alternatel : visitiFoodIn.Alternatel;
                    visitfoodin.supplementl = visitiFoodIn.supplementl == null ? visitfoodin.supplementl : visitiFoodIn.supplementl == "" ? visitfoodin.supplementl : visitiFoodIn.supplementl;
                    visitfoodin.Fluidss = visitiFoodIn.Fluidss == null ? visitfoodin.Fluidss : visitiFoodIn.Fluidss == "" ? visitfoodin.Fluidss : visitiFoodIn.Fluidss;
                    visitfoodin.Alternates = visitiFoodIn.Alternates == null ? visitfoodin.Alternates : visitiFoodIn.Alternates == "" ? visitfoodin.Alternates : visitiFoodIn.Alternates;
                    visitfoodin.supplements = visitiFoodIn.supplements == null ? visitfoodin.supplements : visitiFoodIn.supplements == "" ? visitfoodin.supplements : visitiFoodIn.supplements;
                    visitfoodin.VisitFoodintake_Status = record.VisitFoodintake_Status;
                    visitfoodin.VisitFoodintake_CreatedBy = record.VisitFoodintake_CreatedBy;
                    visitfoodin.VisitFoodintake_CreatedDate = record.VisitFoodintake_CreatedDate;
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.FoodIntake,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = visitfoodin.VisitFoodintake_ID.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
                else
                {
                    this.dbContext.VisitFoodintakes.Add(record);
                    this.dbContext.SaveChanges();
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.FoodIntake,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = record.VisitFoodintake_ID.ToString(),
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
                VisitFoodintake visitfoodin = this.dbContext.VisitFoodintakes.Find(visitiFoodIn.VisitFoodintake_ID);
                visitfoodin.VisitFoodintake_ID = record.VisitFoodintake_ID;
                visitfoodin.PVisit_Id = record.PVisit_Id;
                visitfoodin.AssessmentDate = record.AssessmentDate;
                visitfoodin.Initials = record.Initials;
                visitfoodin.Attendingphysician = record.Attendingphysician;
                visitfoodin.Fluidsb = record.Fluidsb;
                visitfoodin.Alternateb = record.Alternateb;
                visitfoodin.supplementb = record.supplementb;
                visitfoodin.Fluidsl = record.Fluidsl;
                visitfoodin.Alternatel = record.Alternatel;
                visitfoodin.supplementl = record.supplementl;
                visitfoodin.Fluidss = record.Fluidss;
                visitfoodin.Alternates = record.Alternates;
                visitfoodin.supplements = record.supplements;
                visitfoodin.VisitFoodintake_Status = record.VisitFoodintake_Status;
                visitfoodin.VisitFoodintake_CreatedBy = record.VisitFoodintake_CreatedBy;
                visitfoodin.VisitFoodintake_CreatedDate = record.VisitFoodintake_CreatedDate;

                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FoodIntake,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = visitfoodin.VisitFoodintake_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
               
                return 1;
            }

        }
        public List<VisitFoodintakeEntity> GetVisitFoodIntakeList(int Patient_Id)
        {
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == Patient_Id).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var data = (from visitfoodintake in this.dbContext.VisitFoodintakes
                        join visitinfo in this.dbContext.VisitInfoes on visitfoodintake.PVisit_Id equals visitinfo.PVisit_Id
                        join user in this.dbContext.Users on visitfoodintake.VisitFoodintake_CreatedBy equals user.User_Id
                        where visitinfo.Patient_Id == Patient_Id
                        select new //VisitFoodintakeEntity
                        {
                            visitFoodIntake = visitfoodintake,
                            user = user,
                            //VisitFoodintake_ID = visitfoodintake.VisitFoodintake_ID,
                            //PVisit_Id = visitfoodintake.PVisit_Id,
                            //AssessmentDate = visitfoodintake.AssessmentDate,
                            //Initials = visitfoodintake.Initials,
                            //Attendingphysician = visitfoodintake.Attendingphysician,
                            //Fluidsb = visitfoodintake.Fluidsb,
                            //Alternateb = visitfoodintake.Alternateb,
                            //supplementb = visitfoodintake.supplementb,
                            //Fluidsl = visitfoodintake.Fluidsl,
                            //Alternatel = visitfoodintake.Alternatel,
                            //supplementl = visitfoodintake.supplementl,
                            //Fluidss = visitfoodintake.Fluidss,
                            //Alternates = visitfoodintake.Alternates,
                            //supplements = visitfoodintake.supplements,
                            //VisitFoodintake_Status = visitfoodintake.VisitFoodintake_Status,
                            //VisitFoodintake_CreatedBy = visitfoodintake.VisitFoodintake_CreatedBy,
                            //VisitFoodintake_CreatedDate = visitfoodintake.VisitFoodintake_CreatedDate,
                            //UserName = user.User_DisplayName

                        }).ToList()
                        .Select(x => new VisitFoodintakeEntity
                        {
                            VisitFoodintake_ID = x.visitFoodIntake.VisitFoodintake_ID,
                            PVisit_Id = x.visitFoodIntake.PVisit_Id,
                            AssessmentDate = x.visitFoodIntake.AssessmentDate,
                            Initials = x.visitFoodIntake.Initials,
                            Attendingphysician = x.visitFoodIntake.Attendingphysician,
                            Fluidsb = x.visitFoodIntake.Fluidsb,
                            Alternateb = x.visitFoodIntake.Alternateb,
                            supplementb = x.visitFoodIntake.supplementb,
                            Fluidsl = x.visitFoodIntake.Fluidsl,
                            Alternatel = x.visitFoodIntake.Alternatel,
                            supplementl = x.visitFoodIntake.supplementl,
                            Fluidss = x.visitFoodIntake.Fluidss,
                            Alternates = x.visitFoodIntake.Alternates,
                            supplements = x.visitFoodIntake.supplements,
                            VisitFoodintake_Status = x.visitFoodIntake.VisitFoodintake_Status,
                            VisitFoodintake_CreatedBy = x.visitFoodIntake.VisitFoodintake_CreatedBy,
                            VisitFoodintake_CreatedDate =GetTimeZoneDateTime(x.visitFoodIntake.VisitFoodintake_CreatedDate,companyId),
                            UserName = x.user.User_DisplayName
                        }).ToList();
            return data;

        }
        public VisitFoodintakeEntity GetFoodIntakeDetailsByID(int VisitFoodIntakeId)
        {
            var foodIntake = this.dbContext.VisitFoodintakes.Find(VisitFoodIntakeId);
            return this.autoMapper.Map<VisitFoodintake, VisitFoodintakeEntity>(foodIntake);
        }
        public int InsertUpdateVisitVitals(VisitVitalEntity visitvitals)
        {
            visitvitals.VitalSigns_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            visitvitals.VitalDate= Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<VisitVitalEntity, VisitVital>(visitvitals);
            if (record.Vitals_ID == 0)
            {
                
                this.dbContext.VisitVitals.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Vitals,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.Vitals_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
                
            }

            else
            {
                VisitVital visitvital = this.dbContext.VisitVitals.Find(visitvitals.Vitals_ID);
                visitvital.PVisit_Id = record.PVisit_Id;
                visitvital.VitalDate = record.VitalDate;
                visitvital.VitalTime = record.VitalTime;
                visitvital.CistolicBP = record.CistolicBP;
                visitvital.DiastolicBP = record.DiastolicBP;
                visitvital.HeartRate = record.HeartRate;
                visitvital.RespiratoryRate = record.RespiratoryRate;
                visitvital.OxygenRate = record.OxygenRate;
                visitvital.Temperature = record.Temperature;
                visitvital.Pain = record.Pain;
                visitvital.Remark = record.Remark;
                visitvital.PulseRate = record.PulseRate;
                visitvital.Initials = record.Initials;
                visitvital.PDAID = record.PDAID;
                visitvital.BloodSugar = record.BloodSugar;
                visitvital.VitalSigns_status = record.VitalSigns_status;
                visitvital.VitalSigns_CreatedBy = record.VitalSigns_CreatedBy;
                visitvital.VitalSigns_CreatedOn = record.VitalSigns_CreatedOn;



                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Vitals,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = visitvital.Vitals_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            
                return 1;
            }

        }

        public List<VisitVitalEntity> GetAllVisitVitalList(int Patient_Id)
        {
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == Patient_Id).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();

            return (from visitvital in this.dbContext.VisitVitals
                    join visitinfo in this.dbContext.VisitInfoes on visitvital.PVisit_Id equals visitinfo.PVisit_Id
                    join user in this.dbContext.Users on visitvital.VitalSigns_CreatedBy equals user.User_Id
                    where visitinfo.Patient_Id == Patient_Id
                    select new //VisitVitalEntity
                    {
                        vitals = visitvital,
                        user = user,
                        //Vitals_ID = visitvital.Vitals_ID,
                        //PVisit_Id = visitvital.PVisit_Id,
                        //VitalDate = visitvital.VitalDate,
                        //VitalTime = visitvital.VitalTime,
                        //CistolicBP = visitvital.CistolicBP,
                        //DiastolicBP = visitvital.DiastolicBP,
                        //HeartRate = visitvital.HeartRate,
                        //RespiratoryRate = visitvital.RespiratoryRate,
                        //OxygenRate = visitvital.OxygenRate,
                        //Temperature = visitvital.Temperature,
                        //Pain = visitvital.Pain,
                        //Remark = visitvital.Remark,
                        //PulseRate = visitvital.PulseRate,
                        //Initials = visitvital.Initials,
                        //PDAID = visitvital.PDAID,
                        //VitalSigns_status = visitvital.VitalSigns_status,
                        //VitalSigns_CreatedBy = visitvital.VitalSigns_CreatedBy,
                        //VitalSigns_CreatedOn = visitvital.VitalSigns_CreatedOn,
                        //BloodSugar = visitvital.BloodSugar,
                        //UserName = user.User_DisplayName,
                    }).ToList()
                    .Select(x => new VisitVitalEntity
                    {
                        Vitals_ID = x.vitals.Vitals_ID,
                        PVisit_Id = x.vitals.PVisit_Id,
                        VitalDate = x.vitals.VitalDate,
                        VitalTime = x.vitals.VitalTime,
                        CistolicBP = x.vitals.CistolicBP,
                        DiastolicBP = x.vitals.DiastolicBP,
                        HeartRate = x.vitals.HeartRate,
                        RespiratoryRate = x.vitals.RespiratoryRate,
                        OxygenRate = x.vitals.OxygenRate,
                        Temperature = x.vitals.Temperature,
                        Pain = x.vitals.Pain,
                        Remark = x.vitals.Remark,
                        PulseRate = x.vitals.PulseRate,
                        Initials = x.vitals.Initials,
                        PDAID = x.vitals.PDAID,
                        VitalSigns_status = x.vitals.VitalSigns_status,
                        VitalSigns_CreatedBy = x.vitals.VitalSigns_CreatedBy,
                        VitalSigns_CreatedOn =GetTimeZoneDateTime(x.vitals.VitalSigns_CreatedOn,companyId),
                        BloodSugar = x.vitals.BloodSugar,
                        UserName = x.user.User_DisplayName,
                    }).ToList();

        }
        public VisitVitalEntity GetVisitVitalDetailsByID(int Vitals_ID)
        {
            var visit = this.dbContext.VisitVitals.Find(Vitals_ID);
            return this.autoMapper.Map<VisitVital, VisitVitalEntity>(visit);
        }

        public int InsertUpdateVisitNurseNote(VisitNursingNoteEntity visitnursenote)
        {
            visitnursenote.VisitNursingNotes_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<VisitNursingNoteEntity, VisitNursingNote>(visitnursenote);
            if (record.VisitNursingNotes_ID == 0)
            {
                this.dbContext.VisitNursingNotes.Add(record);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseNotes,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.VisitNursingNotes_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else
            {
                VisitNursingNote visitnursenotes = this.dbContext.VisitNursingNotes.Find(visitnursenote.VisitNursingNotes_ID);
                visitnursenotes.PVisit_Id = record.PVisit_Id;
                visitnursenotes.NoteDate = record.NoteDate;
                visitnursenotes.NoteTime = record.NoteTime;
                visitnursenotes.NurseName = record.NurseName;
                visitnursenotes.Notes = record.Notes;
                visitnursenotes.PDAID = record.PDAID;
                visitnursenotes.VisitNursingNotes_Status = record.VisitNursingNotes_Status;
                visitnursenotes.VisitNursingNotes_CreatedBy = record.VisitNursingNotes_CreatedBy;
                visitnursenotes.VisitNursingNotes_CreatedOn = record.VisitNursingNotes_CreatedOn;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseNotes,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.VisitNursingNotes_ID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }

        }
        public List<VisitNursingNoteEntity> GetVisitNurseNote(int Patient_Id)
        {
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == Patient_Id).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();

            var data = (from visitnursenote in this.dbContext.VisitNursingNotes
                        join visitinfo in this.dbContext.VisitInfoes on visitnursenote.PVisit_Id equals visitinfo.PVisit_Id
                        join user in this.dbContext.Users on visitnursenote.VisitNursingNotes_CreatedBy equals user.User_Id
                        where visitinfo.Patient_Id == Patient_Id
                        select new //WeightLogEntity
                        {
                            visitnursenote = visitnursenote,
                            visitinfo = visitinfo,
                            user = user,
                            //WeightLog_ID = weight.WeightLog_ID,
                            //Patient_Id = weight.Patient_Id,
                            //Weight = weight.Weight,
                            //Type = weight.Type=="1"?"Kg":"Lb",
                            //HeightFeet = weight.HeightFeet,
                            //HeightInc = weight.HeightInc,
                            //DateTime = weight.DateTime,
                            //PreDialysis = weight.PreDialysis,
                            //PostDialysis = weight.PostDialysis,
                            //Remarks = weight.Remarks,
                            //IBW = weight.IBW,
                            //initials = weight.initials,
                            //WeightLog_Status = weight.WeightLog_Status,
                            //WeightLog_CreatedBy = weight.WeightLog_CreatedBy,
                            //WeightLog_CreatedOn = weight.WeightLog_CreatedOn,
                            //UserName = user.User_DisplayName

                        }).ToList()
                         .Select(x => new VisitNursingNoteEntity
                        {
                       // select new VisitNursingNoteEntity
                       // {
                            VisitNursingNotes_ID = x.visitnursenote.VisitNursingNotes_ID,
                            PVisit_Id = x.visitnursenote.PVisit_Id,
                            NoteDate = x.visitnursenote.NoteDate,
                            NoteTime = x.visitnursenote.NoteTime,
                            NurseName = x.visitnursenote.NurseName,
                            Notes = x.visitnursenote.Notes,
                            PDAID = x.visitnursenote.PDAID,
                            VisitNursingNotes_Status = x.visitnursenote.VisitNursingNotes_Status,
                            VisitNursingNotes_CreatedBy = x.visitnursenote.VisitNursingNotes_CreatedBy,
                            VisitNursingNotes_CreatedOn = GetTimeZoneDateTime(x.visitnursenote.VisitNursingNotes_CreatedOn, companyId),
                            UserName = x.user.User_DisplayName

                        }).ToList();
            return data;

            
        }
        public VisitNursingNoteEntity GetVisitNurseNoteDetailsByID(int VisitNursingNotes_ID)
        {
            var visitnurse = this.dbContext.VisitNursingNotes.Find(VisitNursingNotes_ID);
            return this.autoMapper.Map<VisitNursingNote, VisitNursingNoteEntity>(visitnurse);
        }
        public Nullable<DateTime> GetTimeZoneDateTime(DateTime? dateTime, int? companyId)
        {
            var convetedDate = (this.dbContext.GetTimeZoneConvertedDateTime(dateTime, companyId).FirstOrDefault());
            return convetedDate;
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
                    where v.NursingStationId == nurseStationId && v.PVisit_Status==1
                    select new ResidentDropEntity()
                    {
                        Patient_Id = d.Patient_Id,
                        PatientName = d.PatientLastName + ", " + d.PatientFirstName + " " + (d.PatientMiddleInitial == null ? "" : d.PatientMiddleInitial),
                        PVisit_Status = v.PVisit_Status
                    }).Distinct().OrderBy(item => item.PatientName).ToList();
        }
    }
}
