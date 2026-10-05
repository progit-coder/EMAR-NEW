using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public class AdminApprovalRepository : IAdminApprovalRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IFacilityRepository _facilityRepository;
        public AdminApprovalRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, FacilityRepository facilityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
            this._facilityRepository = facilityRepository;
        }
        public int InsertResidentDemographicData(ApprovalDemographicEntity demographics)
        {
            var demographicdata = this.autoMapper.Map<ApprovalDemographicEntity, ApprovalDemographic>(demographics);
            this.dbContext.ApprovalDemographics.Add(demographicdata);
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.DemographicInformation,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = demographicdata.ApprovalPatient_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int InsertResidentAllergy(ApprovalAllergyInfoEntity entity)
        {
            var record = this.autoMapper.Map<ApprovalAllergyInfoEntity, ApprovalAllergyInfo>(entity);
            this.dbContext.ApprovalAllergyInfoes.Add(record);
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Allergies,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.ApprovalPAllergy_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int InsertResidentDianosisData(ApprovalDiagnosisInfoEntity entity)
        {
            var record = this.autoMapper.Map<ApprovalDiagnosisInfoEntity, ApprovalDiagnosisInfo>(entity);
            this.dbContext.ApprovalDiagnosisInfoes.Add(record);
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Diagnosis,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.ApprovalPDiagnosis_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int InsertResidentVisitInfo(ApprovalVisitInfoEntity entity)
        {
            var existingRecord = this.dbContext.VisitInfoes.Where(p => p.PVisit_Id == entity.PVisit_Id).FirstOrDefault();
            var admitinfo = this.autoMapper.Map<ApprovalVisitInfoEntity, ApprovalVisitInfo>(entity);

            if (existingRecord.FacilityId != entity.FacilityId || existingRecord.NursingStationId != entity.NursingStationId ||
                existingRecord.Floor != entity.Floor || existingRecord.Room != entity.Room ||
                existingRecord.Bed != entity.Bed)
            {
                admitinfo.PVisit_Status = 1;
                this.dbContext.ApprovalVisitInfoes.Add(admitinfo);
                this.dbContext.SaveChanges();
                //return 1 for Transfer
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Transfer,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = admitinfo.ApprovalPVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else if (existingRecord.FacilityId == entity.FacilityId && existingRecord.NursingStationId == entity.NursingStationId &&
                existingRecord.Floor == entity.Floor && existingRecord.Room == entity.Room &&
                existingRecord.Bed == entity.Bed && entity.Wing != null && existingRecord.Wing != entity.Wing)
            {
                existingRecord.Wing = entity.Wing;
                existingRecord.PVisit_Status = admitinfo.PVisit_Status;
                existingRecord.PVisit_CreatedBy = admitinfo.PVisit_CreatedBy;
                existingRecord.PVisit_CreatedDate = admitinfo.PVisit_CreatedDate;
                this.dbContext.SaveChanges();
                //return 4 for  Wing Transfer but Outbound Not generate
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = entity.PVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else if (entity.DischargeDate != null)
            {
                admitinfo.PVisit_Status = 2;
                this.dbContext.ApprovalVisitInfoes.Add(admitinfo);
                this.dbContext.SaveChanges();
                //return 2 for Discharge
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Discharge,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = admitinfo.ApprovalPVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 2;
            }
            else
            {
                var record = this.dbContext.VisitInfoes.Where(p => p.PVisit_Id == entity.PVisit_Id).FirstOrDefault();
                if (record != null)
                {
                    record.PatientClass = admitinfo.PatientClass;
                    //record.NursingStationId = admitinfo.NursingStationId;
                    //record.Room = admitinfo.Room;
                    //record.Bed = admitinfo.Bed;
                    //record.FacilityId = admitinfo.FacilityId;
                    //record.Floor = admitinfo.Floor;
                    record.AdmissionType = admitinfo.AdmissionType;
                    record.PreAdmitNumber = admitinfo.PreAdmitNumber;
                    //record.PriorNursingStationId = admitinfo.PriorNursingStationId;
                    //record.PriorRoom = admitinfo.PriorRoom;
                    //record.PriorBed = admitinfo.PriorBed;
                    //record.PriorFacilityId = admitinfo.PriorFacilityId;
                    //record.PriorFloor = admitinfo.PriorFloor;
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
                    //record.AdmitDate = admitinfo.AdmitDate;
                    //record.DischargeDate = admitinfo.DischargeDate;
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
                    this.dbContext.SaveChanges();
                    //return 3 for Just Update
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = record.PVisit_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 3;
                }
            }

            return 0;
        }
        public int InsertResidentVisitInfoWithoutApproval(ApprovalVisitInfoEntity entity, EMAREntities context)
        {
            var existingRecord = context.VisitInfoes.Where(p => p.PVisit_Id == entity.PVisit_Id).FirstOrDefault();
            int flag = 0;

            if ((entity.FacilityId != null && existingRecord.FacilityId != entity.FacilityId) || (entity.NursingStationId != null && existingRecord.NursingStationId != entity.NursingStationId) ||
                 (entity.Floor != null && existingRecord.Floor != entity.Floor) || (entity.Room != null && existingRecord.Room != entity.Room) ||
                 (existingRecord.Bed != null && existingRecord.Bed != entity.Bed))
            {
                //existingRecord.NursingStationId = entity.NursingStationId;
                //existingRecord.Floor = entity.Floor;
                //existingRecord.Room = entity.Room;
                //existingRecord.Bed = entity.Bed;
                existingRecord.PVisit_Status = 1;
                if (entity.FacilityId != null && existingRecord.FacilityId != entity.FacilityId)
                {

                    existingRecord.PriorFacilityId = existingRecord.FacilityId;
                    existingRecord.FacilityId = entity.FacilityId;
                    flag = 1;

                }
                if (entity.NursingStationId != null && existingRecord.NursingStationId != entity.NursingStationId)
                {

                    existingRecord.PriorNursingStationId = existingRecord.NursingStationId;
                    existingRecord.NursingStationId = entity.NursingStationId;
                    if (flag!=1)
                    {
                        existingRecord.PriorFacilityId = existingRecord.FacilityId;
                        flag = 0;
                    }

                }
                if (existingRecord.Floor != entity.Floor)
                {

                    existingRecord.PriorFloor = existingRecord.Floor;
                    existingRecord.Floor = entity.Floor;

                }
                if (existingRecord.Room != entity.Room)
                {
                    existingRecord.PriorRoom = existingRecord.Room;
                    existingRecord.Room = entity.Room;
                }
                if (existingRecord.Bed != entity.Bed)
                {
                    existingRecord.PriorBed = existingRecord.Bed;
                    existingRecord.Bed = entity.Bed;
                }
                if (existingRecord.Wing != entity.Wing)
                {
                    existingRecord.Wing = entity.Wing;
                    context.SaveChanges();
                }
                //return 1 for Transfer
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Transfer,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = entity.PVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                existingRecord.PVisit_CreatedBy = entity.PVisit_CreatedBy;
                existingRecord.PVisit_CreatedDate = entity.PVisit_CreatedDate;
                context.SaveChanges();
                return 1;
            }
            else if ((existingRecord.FacilityId == entity.FacilityId || entity.FacilityId == null) && (existingRecord.NursingStationId == entity.NursingStationId || entity.NursingStationId == null) &&
                (entity.Floor == null || existingRecord.Floor == entity.Floor) && (entity.Room == null || existingRecord.Room == entity.Room) &&
                (entity.Bed == null || existingRecord.Bed == entity.Bed) && entity.Wing != null && existingRecord.Wing != entity.Wing)
            {
                existingRecord.Wing = entity.Wing;
                context.SaveChanges();
                //return 4 for  Wing Transfer but Outbound Not generate
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = entity.PVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 4;
            }
            else if (entity.DischargeDate != null)
            {
                string disDate = Convert.ToDateTime(entity.DischargeDate).ToString("MM/dd/yyyy");
                existingRecord.DischargeDate =Convert.ToDateTime(disDate+" "+entity.DischargeTime);
                existingRecord.PVisit_CreatedBy = entity.PVisit_CreatedBy;
                existingRecord.PVisit_CreatedDate = entity.PVisit_CreatedDate;
                existingRecord.PVisit_Status = 2;
                context.SaveChanges();
                //return 2 for Discharge
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Discharge,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = entity.PVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 2;
            }
            else
            {
                var record = context.VisitInfoes.Where(p => p.PVisit_Id == entity.PVisit_Id).FirstOrDefault();
                if (record != null)
                {
                    //record.PatientClass = entity.PatientClass;
                    //record.NursingStationId = admitinfo.NursingStationId;
                    //record.Room = admitinfo.Room;
                    //record.Bed = admitinfo.Bed;
                    //record.FacilityId = admitinfo.FacilityId;
                    //record.Floor = admitinfo.Floor;
                    //record.AdmissionType = entity.AdmissionType;
                    //record.PreAdmitNumber = entity.PreAdmitNumber;
                    //record.PriorNursingStationId = admitinfo.PriorNursingStationId;
                    //record.PriorRoom = admitinfo.PriorRoom;
                    //record.PriorBed = admitinfo.PriorBed;
                    //record.PriorFacilityId = admitinfo.PriorFacilityId;
                    //record.PriorFloor = admitinfo.PriorFloor;
                    //record.AdmitDate = entity.AdmitDate;
                    //record.PrimaryPhysicianNPI = entity.PrimaryPhysicianNPI;
                    //record.PrimaryPhysicianLName = entity.PrimaryPhysicianLName;
                    //record.PrimaryPhysicianFName = entity.PrimaryPhysicianFName;
                    record.DietType = entity.DietType;
                    if (record.Physician_Id!= entity.Physician_Id || record.AdmitDate != entity.AdmitDate)
                    {
                        if(record.Physician_Id != entity.Physician_Id)
                        {
                            record.Physician_Id = entity.Physician_Id;
                            record.PrimaryPhysicianNPI = entity.PrimaryPhysicianNPI;
                            record.PrimaryPhysicianLName = entity.PrimaryPhysicianLName;
                            record.PrimaryPhysicianFName = entity.PrimaryPhysicianFName;
                        }
                        if(record.AdmitDate != entity.AdmitDate)
                        {
                            record.AdmitDate = entity.AdmitDate;
                        }
                        record.PVisit_CreatedBy = entity.PVisit_CreatedBy;
                        record.PVisit_CreatedDate = entity.PVisit_CreatedDate;
                        context.SaveChanges();
                        //return 3 for Physician or Admit Date Change
                        return 3;
                    }
                    //record.ReferringDoctor = entity.ReferringDoctor;
                    //record.ConsultingDoctor = entity.ConsultingDoctor;
                    //record.HospitalService = entity.HospitalService;
                    //record.TemporaryLocation = entity.TemporaryLocation;
                    //record.PreAdmitTestIndicator = entity.PreAdmitTestIndicator;
                    //record.ReAdmissionIndicator = entity.ReAdmissionIndicator;
                    //record.AdmitSource = entity.AdmitSource;
                    //record.AmbulatoryStatus = entity.AmbulatoryStatus;
                    //record.VIPIndicator = entity.VIPIndicator;
                    //record.AdmittingDoctor = entity.AdmittingDoctor;
                    //record.PatientType = entity.PatientType;
                    //record.VisitNumber = entity.VisitNumber;
                    //record.FinancialClass = entity.FinancialClass;
                    //record.ChargePriceIndicator = entity.ChargePriceIndicator;
                    //record.CourtesyCode = entity.CourtesyCode;
                    //record.CreditRating = entity.CreditRating;
                    //record.ContractCode = entity.ContractCode;
                    //record.ContractEffDate = entity.ContractEffDate;
                    //record.ContractAmount = entity.ContractAmount;
                    //record.ContractPeriod = entity.ContractPeriod;
                    //record.InterestCode = entity.InterestCode;
                    //record.BadDebtCode = entity.BadDebtCode;
                    //record.BadDebtDate = entity.BadDebtDate;
                    //record.BadDebtAgencyCode = entity.BadDebtAgencyCode;
                    //record.BadDebtTransferAmt = entity.BadDebtTransferAmt;
                    //record.BadDebtRecoveryAmt = entity.BadDebtRecoveryAmt;
                    //record.DeleteAccIndicator = entity.DeleteAccIndicator;
                    //record.DeleteAccDate = entity.DeleteAccDate;
                    //record.DischargeDisposition = entity.DischargeDisposition;
                    //record.DischargedLocation = entity.DischargedLocation;
                    //record.ServicingFacility = entity.ServicingFacility;
                    //record.BedStatus = entity.BedStatus;
                    //record.AccStatus = entity.AccStatus;
                    //record.PendingLocation = entity.PendingLocation;
                    //record.PriorTemporaryLocation = entity.PriorTemporaryLocation;
                    //record.DischargeDate = admitinfo.DischargeDate;
                    //record.CurrentPatientBalance = entity.CurrentPatientBalance;
                    //record.TotalCharges = entity.TotalCharges;
                    //record.TotalAdjustments = entity.TotalAdjustments;
                    //record.TotalPayments = entity.TotalPayments;
                    //record.AlternateVisitId = entity.AlternateVisitId;
                    //record.VisitIndicator = entity.VisitIndicator;
                    //record.OtherHealthProvider = entity.OtherHealthProvider;
                    record.PVisit_Status = entity.PVisit_Status;
                    record.PVisit_CreatedBy = entity.PVisit_CreatedBy;
                    record.PVisit_CreatedDate = entity.PVisit_CreatedDate;
                    context.SaveChanges();
                    //return 4 for Just Update
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = record.PVisit_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 4;
                }
            }

            return 0;
        }

        /*
        public int InsertResidentInfo(ApprovalVisitInfoEntity entity)
        {
            var existingRecord = this.dbContext.VisitInfoes.Where(p => p.PVisit_Id == entity.PVisit_Id).FirstOrDefault();
            var admitinfo = this.autoMapper.Map<ApprovalVisitInfoEntity, ApprovalVisitInfo>(entity);

            if (
                existingRecord.Floor != entity.Floor || existingRecord.Room != entity.Room ||
                existingRecord.Bed != entity.Bed)
            {
                admitinfo.PVisit_Status = 1;
                this.dbContext.ApprovalVisitInfoes.Add(admitinfo);
                this.dbContext.SaveChanges();
                //return 1 for Transfer
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = admitinfo.ApprovalPVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            else if (entity.DischargeDate != null)
            {
                admitinfo.PVisit_Status = 2;
                this.dbContext.ApprovalVisitInfoes.Add(admitinfo);
                this.dbContext.SaveChanges();
                //return 2 for Discharge
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = admitinfo.ApprovalPVisit_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 2;
            }
            else
            {
                var record = this.dbContext.VisitInfoes.Where(p => p.PVisit_Id == entity.PVisit_Id).FirstOrDefault();
                if (record != null)
                {
                    record.PatientClass = admitinfo.PatientClass;
                    //record.NursingStationId = admitinfo.NursingStationId;
                    record.Room = admitinfo.Room;
                    record.Bed = admitinfo.Bed;
                    record.Wing = admitinfo.Wing;
                    //record.FacilityId = admitinfo.FacilityId;
                    record.Floor = admitinfo.Floor;
                    //record.AdmissionType = admitinfo.AdmissionType;
                    //record.PreAdmitNumber = admitinfo.PreAdmitNumber;
                    //record.PriorNursingStationId = admitinfo.PriorNursingStationId;
                    //record.PriorRoom = admitinfo.PriorRoom;
                    //record.PriorBed = admitinfo.PriorBed;
                    //record.PriorFacilityId = admitinfo.PriorFacilityId;
                    //record.PriorFloor = admitinfo.PriorFloor;
                    //record.PrimaryPhysicianNPI = admitinfo.PrimaryPhysicianNPI;
                    // record.PrimaryPhysicianLName = admitinfo.PrimaryPhysicianLName;
                    // record.PrimaryPhysicianFName = admitinfo.PrimaryPhysicianFName;
                    // record.ReferringDoctor = admitinfo.ReferringDoctor;
                    // record.ConsultingDoctor = admitinfo.ConsultingDoctor;
                    //record.HospitalService = admitinfo.HospitalService;
                    //record.TemporaryLocation = admitinfo.TemporaryLocation;
                    //record.PreAdmitTestIndicator = admitinfo.PreAdmitTestIndicator;
                    // record.ReAdmissionIndicator = admitinfo.ReAdmissionIndicator;
                    //record.AdmitSource = admitinfo.AdmitSource;
                    // record.AmbulatoryStatus = admitinfo.AmbulatoryStatus;
                    // record.VIPIndicator = admitinfo.VIPIndicator;
                    // record.AdmittingDoctor = admitinfo.AdmittingDoctor;
                    //record.PatientType = admitinfo.PatientType;
                    // record.VisitNumber = admitinfo.VisitNumber;
                    // record.FinancialClass = admitinfo.FinancialClass;
                    //record.ChargePriceIndicator = admitinfo.ChargePriceIndicator;
                    // record.CourtesyCode = admitinfo.CourtesyCode;
                    // record.CreditRating = admitinfo.CreditRating;
                    //  record.ContractCode = admitinfo.ContractCode;
                    // record.ContractEffDate = admitinfo.ContractEffDate;
                    // record.ContractAmount = admitinfo.ContractAmount;
                    // record.ContractPeriod = admitinfo.ContractPeriod;
                    // record.InterestCode = admitinfo.InterestCode;
                    // record.BadDebtCode = admitinfo.BadDebtCode;
                    //record.BadDebtDate = admitinfo.BadDebtDate;
                    //record.BadDebtAgencyCode = admitinfo.BadDebtAgencyCode;
                    // record.BadDebtTransferAmt = admitinfo.BadDebtTransferAmt;
                    // record.BadDebtRecoveryAmt = admitinfo.BadDebtRecoveryAmt;
                    // record.DeleteAccIndicator = admitinfo.DeleteAccIndicator;
                    // record.DeleteAccDate = admitinfo.DeleteAccDate;
                    // record.DischargeDisposition = admitinfo.DischargeDisposition;
                    //  record.DischargedLocation = admitinfo.DischargedLocation;
                    //  record.DietType = admitinfo.DietType;
                    //  record.ServicingFacility = admitinfo.ServicingFacility;
                    //  record.BedStatus = admitinfo.BedStatus;
                    //  record.AccStatus = admitinfo.AccStatus;
                    //  record.PendingLocation = admitinfo.PendingLocation;
                    //  record.PriorTemporaryLocation = admitinfo.PriorTemporaryLocation;
                    //record.AdmitDate = admitinfo.AdmitDate;
                    //record.DischargeDate = admitinfo.DischargeDate;
                    //  record.CurrentPatientBalance = admitinfo.CurrentPatientBalance;
                    // record.TotalCharges = admitinfo.TotalCharges;
                    // record.TotalAdjustments = admitinfo.TotalAdjustments;
                    // record.TotalPayments = admitinfo.TotalPayments;
                    // record.AlternateVisitId = admitinfo.AlternateVisitId;
                    // record.VisitIndicator = admitinfo.VisitIndicator;
                    // record.OtherHealthProvider = admitinfo.OtherHealthProvider;
                    record.PVisit_Status = admitinfo.PVisit_Status;
                    // record.PVisit_CreatedBy = admitinfo.PVisit_CreatedBy;
                    //  record.PVisit_CreatedDate = admitinfo.PVisit_CreatedDate;
                    this.dbContext.SaveChanges();
                    //return 3 for Just Update
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.AdmitVisitInfo,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                        Comments = record.PVisit_Id.ToString(),
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 3;
                }
            }

            return 0;
        }
        */
        public List<ApprovalDemographicEntity> GetApprovalPendingDemographics()
        {
            var pendingDemographics = this.dbContext.ApprovalDemographics.Where(a => a.PDOutBoundApproval == 0 || a.PDOutBoundApproval == null).ToList();
            return this.autoMapper.Map<List<ApprovalDemographic>, List<ApprovalDemographicEntity>>(pendingDemographics);
        }
        public List<ApprovalAllergyInfoEntity> GetApprovalPendingAllergies()
        {
            var pendingAllergies = this.dbContext.ApprovalAllergyInfoes.Where(a => a.PAOutBoundApproval == 0 || a.PAOutBoundApproval == null).ToList();
            return this.autoMapper.Map<List<ApprovalAllergyInfo>, List<ApprovalAllergyInfoEntity>>(pendingAllergies);
        }
        public List<ApprovalDiagnosisInfoEntity> GetApprovalPendingDiagnosis()
        {
            var pendingDiagnosis = this.dbContext.ApprovalDiagnosisInfoes.Where(a => a.PDGOutBoundApproval == 0 || a.PDGOutBoundApproval == null).ToList();
            return this.autoMapper.Map<List<ApprovalDiagnosisInfo>, List<ApprovalDiagnosisInfoEntity>>(pendingDiagnosis);
        }
        public List<ApprovalVisitInfoEntity> GetApprovalPendingTransfer()
        {
            var pendingTransfer = this.dbContext.ApprovalVisitInfoes.Where(a => a.PVisit_Status == 1 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalVisitInfo>, List<ApprovalVisitInfoEntity>>(pendingTransfer);
        }
        public List<ApprovalVisitInfoEntity> GetApprovalPendingDischarge()
        {
            var pendingDischarge = this.dbContext.ApprovalVisitInfoes.Where(a => a.PVisit_Status == 2 && a.DischargeDate != null && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalVisitInfo>, List<ApprovalVisitInfoEntity>>(pendingDischarge);
        }
        public List<ApprovalDemographicEntity> GetApprovalPendingDemographicsByPatientId(int patientId)
        {
            var pendingDemographics = this.dbContext.ApprovalDemographics.Where(a => a.Patient_Id == patientId && (a.PDOutBoundApproval == 0 || a.PDOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalDemographic>, List<ApprovalDemographicEntity>>(pendingDemographics);
        }
        public List<ApprovalAllergyInfoEntity> GetApprovalPendingAllergiesByPatientId(int patientId)
        {
            var pendingAllergies = this.dbContext.ApprovalAllergyInfoes.Where(a => a.Patient_Id == patientId && (a.PAOutBoundApproval == 0 || a.PAOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalAllergyInfo>, List<ApprovalAllergyInfoEntity>>(pendingAllergies);
        }
        public List<ApprovalDiagnosisInfoEntity> GetApprovalPendingDiagnosisByPatientId(int patientId)
        {
            var pendingDiagnosis = this.dbContext.ApprovalDiagnosisInfoes.Where(a => a.Patient_Id == patientId && (a.PDGOutBoundApproval == 0 || a.PDGOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalDiagnosisInfo>, List<ApprovalDiagnosisInfoEntity>>(pendingDiagnosis);
        }
        public List<ApprovalVisitInfoEntity> GetApprovalPendingTransferByPatientId(int patientId)
        {
            var pendingTransfer = this.dbContext.ApprovalVisitInfoes.Where(a => a.Patient_Id == patientId && a.PVisit_Status == 1 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalVisitInfo>, List<ApprovalVisitInfoEntity>>(pendingTransfer);
        }
        public List<ApprovalVisitInfoEntity> GetApprovalPendingDischargeByPatientId(int patientId)
        {
            var pendingDischarge = this.dbContext.ApprovalVisitInfoes.Where(a => a.Patient_Id == patientId && a.DischargeDate != null && a.PVisit_Status == 2 && (a.PVOutBoundApproval == 0 || a.PVOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalVisitInfo>, List<ApprovalVisitInfoEntity>>(pendingDischarge);
        }
        public int ApproveAllergy(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalAllergyInfo allergyInfo = context.ApprovalAllergyInfoes.Where(e => e.ApprovalPAllergy_Id == entity.ApprovalId && e.PAllergy_Id == entity.Record_Id).FirstOrDefault();

            if (allergyInfo != null)
            {
                allergyInfo.PAOutBoundApprovalBy = entity.ApprovedBy;
                allergyInfo.PAOutBoundApproval = entity.ApprovalStatus;
                allergyInfo.PAOutBoundApprovalOn = entity.ApprovedDate;
                context.SaveChanges();

                AllergyInfo record = context.AllergyInfoes.Where(e => e.PAllergy_Id == allergyInfo.PAllergy_Id).FirstOrDefault();
                if (record == null)
                {
                    record = new AllergyInfo();
                    record.Patient_Id = (int)allergyInfo.Patient_Id;
                    record.AllergyType_Id = allergyInfo.AllergyType_Id;
                    record.ClassDrugType = allergyInfo.ClassDrugType;
                    record.ClassDrug_Id = allergyInfo.ClassDrug_Id;
                    record.ClassDrug_Name = allergyInfo.ClassDrug_Name;
                    record.NameOfCoding = allergyInfo.NameOfCoding;
                    record.AllergySeverityCode = allergyInfo.AllergySeverityCode;
                    record.AllergyReactionCode = allergyInfo.AllergyReactionCode;
                    record.AllergyIdentificationDate = allergyInfo.AllergyIdentificationDate;
                    record.PAllergy_Status = allergyInfo.PAllergy_Status;
                    record.PAllergy_CreatedBy = allergyInfo.PAllergy_CreatedBy;
                    record.PAllergy_CreatedDate = allergyInfo.PAllergy_CreatedDate;
                    record.PAOutBoundApprovalBy = allergyInfo.PAOutBoundApprovalBy;
                    record.PAOutBoundApproval = allergyInfo.PAOutBoundApproval;
                    record.PAOutBoundApprovalOn = allergyInfo.PAOutBoundApprovalOn;

                    context.AllergyInfoes.Add(record);
                    context.SaveChanges();
                    return 1;
                }
                else
                {
                    record.Patient_Id = (int)allergyInfo.Patient_Id;
                    record.AllergyType_Id = allergyInfo.AllergyType_Id;
                    record.ClassDrugType = allergyInfo.ClassDrugType;
                    record.ClassDrug_Id = allergyInfo.ClassDrug_Id;
                    record.ClassDrug_Name = allergyInfo.ClassDrug_Name;
                    record.NameOfCoding = allergyInfo.NameOfCoding;
                    record.AllergySeverityCode = allergyInfo.AllergySeverityCode;
                    record.AllergyReactionCode = allergyInfo.AllergyReactionCode;
                    record.AllergyIdentificationDate = allergyInfo.AllergyIdentificationDate;
                    record.PAllergy_Status = allergyInfo.PAllergy_Status;
                    record.PAllergy_CreatedBy = allergyInfo.PAllergy_CreatedBy;
                    record.PAllergy_CreatedDate = allergyInfo.PAllergy_CreatedDate;
                    record.PAOutBoundApprovalBy = allergyInfo.PAOutBoundApprovalBy;
                    record.PAOutBoundApproval = allergyInfo.PAOutBoundApproval;
                    record.PAOutBoundApprovalOn = allergyInfo.PAOutBoundApprovalOn;

                    context.SaveChanges();
                    return 1;
                }
            }
            return 0;
        }
        public int ApproveDiagnosis(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalDiagnosisInfo diagnosisInfo = context.ApprovalDiagnosisInfoes.Where(e => e.ApprovalPDiagnosis_Id == entity.ApprovalId && e.PDiagnosis_Id == entity.Record_Id).FirstOrDefault();

            if (diagnosisInfo != null)
            {
                diagnosisInfo.PDGOutBoundApprovalBy = entity.ApprovedBy;
                diagnosisInfo.PDGOutBoundApproval = entity.ApprovalStatus;
                diagnosisInfo.PDGOutBoundApprovalOn = entity.ApprovedDate;
                context.SaveChanges();


                DiagnosisInfo record = context.DiagnosisInfoes.Where(e => e.PDiagnosis_Id == diagnosisInfo.PDiagnosis_Id).FirstOrDefault();
                if (record == null)
                {
                    record = new DiagnosisInfo();
                    record.Patient_Id = (int)diagnosisInfo.Patient_Id;
                    record.ICD10_Id = diagnosisInfo.ICD10_Id;
                    record.CodingMethod = diagnosisInfo.CodingMethod;
                    record.DCodingType_Id = diagnosisInfo.DCodingType_Id;
                    record.AltCodingId = diagnosisInfo.AltCodingId;
                    record.AltCodingText = diagnosisInfo.AltCodingText;
                    record.AltCodingMethod = diagnosisInfo.AltCodingMethod;
                    record.DiagnosisDescription = diagnosisInfo.DiagnosisDescription;
                    record.DiagnosisDate = diagnosisInfo.DiagnosisDate;
                    record.PDiagnosis_Status = diagnosisInfo.PDiagnosis_Status;
                    record.PDiagnosis_CreatedBy = diagnosisInfo.PDiagnosis_CreatedBy;
                    record.PDiagnosis_CreatedDate = diagnosisInfo.PDiagnosis_CreatedDate;
                    record.PDGOutBoundFileStatus = diagnosisInfo.PDGOutBoundFileStatus;
                    record.PDGOutBoundApproval = diagnosisInfo.PDGOutBoundApproval;
                    record.PDGOutBoundApprovalBy = diagnosisInfo.PDGOutBoundApprovalBy;
                    record.PDGOutBoundApprovalOn = diagnosisInfo.PDGOutBoundApprovalOn;

                    context.DiagnosisInfoes.Add(record);
                    context.SaveChanges();

                    return 1;
                }
                else
                {
                    record.Patient_Id = (int)diagnosisInfo.Patient_Id;
                    record.ICD10_Id = diagnosisInfo.ICD10_Id;
                    record.CodingMethod = diagnosisInfo.CodingMethod;
                    record.DCodingType_Id = diagnosisInfo.DCodingType_Id;
                    record.AltCodingId = diagnosisInfo.AltCodingId;
                    record.AltCodingText = diagnosisInfo.AltCodingText;
                    record.AltCodingMethod = diagnosisInfo.AltCodingMethod;
                    record.DiagnosisDescription = diagnosisInfo.DiagnosisDescription;
                    record.DiagnosisDate = diagnosisInfo.DiagnosisDate;
                    record.PDiagnosis_Status = diagnosisInfo.PDiagnosis_Status;
                    record.PDiagnosis_CreatedBy = diagnosisInfo.PDiagnosis_CreatedBy;
                    record.PDiagnosis_CreatedDate = diagnosisInfo.PDiagnosis_CreatedDate;
                    record.PDGOutBoundFileStatus = diagnosisInfo.PDGOutBoundFileStatus;
                    record.PDGOutBoundApproval = diagnosisInfo.PDGOutBoundApproval;
                    record.PDGOutBoundApprovalBy = diagnosisInfo.PDGOutBoundApprovalBy;
                    record.PDGOutBoundApprovalOn = diagnosisInfo.PDGOutBoundApprovalOn;
                    context.SaveChanges();

                    return 1;

                }
            }
            return 0;
        }
        public int ApproveDemographic(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            if (entity.Patient_Id == 0)
            {
                ApprovalDemographic demographicdata = context.ApprovalDemographics.Where(e => e.ApprovalPatient_Id == entity.ApprovalId).FirstOrDefault();
                if (demographicdata != null)
                {
                    Demographic record = new Demographic();
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
                    this.dbContext.Demographics.Add(record);
                    this.dbContext.SaveChanges();
                    entity.Patient_Id = record.Patient_Id;
                    entity.Record_Id = record.Patient_Id;
                    return 1;
                }
            }
            else
            {
                ApprovalDemographic demographicdata = context.ApprovalDemographics.Where(e => e.ApprovalPatient_Id == entity.ApprovalId && e.Patient_Id == entity.Record_Id).FirstOrDefault();

                if (demographicdata != null)
                {
                    demographicdata.PDOutBoundApprovalBy = entity.ApprovedBy;
                    demographicdata.PDOutBoundApproval = entity.ApprovalStatus;
                    demographicdata.PDOutBoundApprovalOn = entity.ApprovedDate;
                    context.SaveChanges();

                    Demographic record = context.Demographics.Find(demographicdata.Patient_Id);
                    if (record != null)
                    {
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
                        context.SaveChanges();

                        return 1;
                    }
                    return 0;
                }
            }
            return 0;
        }
        public int ApproveTransfer(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalVisitInfo admitinfo = context.ApprovalVisitInfoes.Where(e => e.ApprovalPVisit_Id == entity.ApprovalId && e.PVisit_Id == entity.Record_Id).FirstOrDefault();
            if (admitinfo != null)
            {
                admitinfo.PVOutBoundApprovalBy = entity.ApprovedBy;
                admitinfo.PVOutBoundApproval = entity.ApprovalStatus;
                admitinfo.PVOutBoundApprovalOn = entity.ApprovedDate;
                context.SaveChanges();

                VisitInfo record = context.VisitInfoes.Where(p => p.PVisit_Id == admitinfo.PVisit_Id).FirstOrDefault();
                if (record != null)
                {
                    //oldRecord.DischargeDate = DateTime.Now;
                    //record.PVisit_Status = 1;
                    //this.dbContext.SaveChanges();

                    //VisitInfo record = new VisitInfo();
                    //record.PVisit_Id = 0;
                    //record.Patient_Id = (int)admitinfo.Patient_Id;
                    //record.PatientClass = admitinfo.PatientClass;
                    //record.AdmissionType = admitinfo.AdmissionType;
                    //record.PreAdmitNumber = admitinfo.PreAdmitNumber;
                    if (record.NursingStationId != admitinfo.NursingStationId)
                    {
                        record.PriorNursingStationId = record.NursingStationId;
                        record.NursingStationId = admitinfo.NursingStationId;
                        record.PriorFacilityId = admitinfo.FacilityId;
                    }
                    if (record.FacilityId != admitinfo.FacilityId)
                    {
                        record.PriorFacilityId = record.FacilityId;
                        record.FacilityId = admitinfo.FacilityId;
                    }
                    if (record.Floor != admitinfo.Floor)
                    {
                        record.PriorFloor = record.Floor;
                        record.Floor = admitinfo.Floor;
                    }
                    if (record.Room != admitinfo.Room)
                    {
                        record.PriorRoom = record.Room;
                        record.Room = admitinfo.Room;
                    }
                    if (record.Bed != admitinfo.Bed)
                    {
                        record.PriorBed = record.Bed;
                        record.Bed = admitinfo.Bed;
                    }

                    //record.PrimaryPhysicianNPI = admitinfo.PrimaryPhysicianNPI;
                    //record.PrimaryPhysicianLName = admitinfo.PrimaryPhysicianLName;
                    //record.PrimaryPhysicianFName = admitinfo.PrimaryPhysicianFName;
                    //record.ReferringDoctor = admitinfo.ReferringDoctor;
                    //record.ConsultingDoctor = admitinfo.ConsultingDoctor;
                    //record.HospitalService = admitinfo.HospitalService;
                    //record.TemporaryLocation = admitinfo.TemporaryLocation;
                    //record.PreAdmitTestIndicator = admitinfo.PreAdmitTestIndicator;
                    //record.ReAdmissionIndicator = admitinfo.ReAdmissionIndicator;
                    //record.AdmitSource = admitinfo.AdmitSource;
                    //record.AmbulatoryStatus = admitinfo.AmbulatoryStatus;
                    //record.VIPIndicator = admitinfo.VIPIndicator;
                    //record.AdmittingDoctor = admitinfo.AdmittingDoctor;
                    //record.PatientType = admitinfo.PatientType;
                    //record.VisitNumber = admitinfo.VisitNumber;
                    //record.FinancialClass = admitinfo.FinancialClass;
                    //record.ChargePriceIndicator = admitinfo.ChargePriceIndicator;
                    //record.CourtesyCode = admitinfo.CourtesyCode;
                    //record.CreditRating = admitinfo.CreditRating;
                    //record.ContractCode = admitinfo.ContractCode;
                    //record.ContractEffDate = admitinfo.ContractEffDate;
                    //record.ContractAmount = admitinfo.ContractAmount;
                    //record.ContractPeriod = admitinfo.ContractPeriod;
                    //record.InterestCode = admitinfo.InterestCode;
                    //record.BadDebtCode = admitinfo.BadDebtCode;
                    //record.BadDebtDate = admitinfo.BadDebtDate;
                    //record.BadDebtAgencyCode = admitinfo.BadDebtAgencyCode;
                    //record.BadDebtTransferAmt = admitinfo.BadDebtTransferAmt;
                    //record.BadDebtRecoveryAmt = admitinfo.BadDebtRecoveryAmt;
                    //record.DeleteAccIndicator = admitinfo.DeleteAccIndicator;
                    //record.DeleteAccDate = admitinfo.DeleteAccDate;
                    //record.DischargeDisposition = admitinfo.DischargeDisposition;
                    //record.DischargedLocation = admitinfo.DischargedLocation;
                    //record.DietType = admitinfo.DietType;
                    //record.ServicingFacility = admitinfo.ServicingFacility;
                    //record.BedStatus = admitinfo.BedStatus;
                    //record.AccStatus = admitinfo.AccStatus;
                    //record.PendingLocation = admitinfo.PendingLocation;
                    //record.PriorTemporaryLocation = admitinfo.PriorTemporaryLocation;
                    //record.AdmitDate = admitinfo.AdmitDate;
                    //record.DischargeDate = admitinfo.DischargeDate;
                    //record.CurrentPatientBalance = admitinfo.CurrentPatientBalance;
                    //record.TotalCharges = admitinfo.TotalCharges;
                    //record.TotalAdjustments = admitinfo.TotalAdjustments;
                    //record.TotalPayments = admitinfo.TotalPayments;
                    //record.AlternateVisitId = admitinfo.AlternateVisitId;
                    //record.VisitIndicator = admitinfo.VisitIndicator;
                    //record.OtherHealthProvider = admitinfo.OtherHealthProvider;
                    //record.Wing = admitinfo.Wing;
                    record.PVisit_Status = 1;
                    record.PVisit_CreatedBy = admitinfo.PVisit_CreatedBy;
                    record.PVisit_CreatedDate = admitinfo.PVisit_CreatedDate;
                    record.PVOutBoundFileStatus = admitinfo.PVOutBoundFileStatus;
                    record.PVOutBoundApproval = admitinfo.PVOutBoundApproval;
                    record.PVOutBoundApprovalBy = admitinfo.PVOutBoundApprovalBy;
                    record.PVOutBoundApprovalOn = admitinfo.PVOutBoundApprovalOn;
                    //this.dbContext.VisitInfoes.Add(record);
                    context.SaveChanges();

                    return 1;
                }
                return 0;
            }
            return 0;
        }
        public int ApproveDischarge(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalVisitInfo admitinfo = context.ApprovalVisitInfoes.Where(e => e.ApprovalPVisit_Id == entity.ApprovalId && e.PVisit_Id == entity.Record_Id).FirstOrDefault();
            if (admitinfo != null)
            {
                admitinfo.PVOutBoundApprovalBy = entity.ApprovedBy;
                admitinfo.PVOutBoundApproval = entity.ApprovalStatus;
                admitinfo.PVOutBoundApprovalOn = entity.ApprovedDate;
                context.SaveChanges();

                VisitInfo record = context.VisitInfoes.Where(p => p.PVisit_Id == admitinfo.PVisit_Id).FirstOrDefault();
                if (record != null)
                {
                    //record.PatientClass = admitinfo.PatientClass;
                    //record.NursingStationId = admitinfo.NursingStationId;
                    //record.Room = admitinfo.Room;
                    //record.Bed = admitinfo.Bed;
                    //record.FacilityId = admitinfo.FacilityId;
                    //record.Floor = admitinfo.Floor;
                    //record.AdmissionType = admitinfo.AdmissionType;
                    //record.PreAdmitNumber = admitinfo.PreAdmitNumber;
                    //record.PriorNursingStationId = admitinfo.PriorNursingStationId;
                    //record.PriorRoom = admitinfo.PriorRoom;
                    //record.PriorBed = admitinfo.PriorBed;
                    //record.PriorFacilityId = admitinfo.PriorFacilityId;
                    //record.PriorFloor = admitinfo.PriorFloor;
                    //record.PrimaryPhysicianNPI = admitinfo.PrimaryPhysicianNPI;
                    //record.PrimaryPhysicianLName = admitinfo.PrimaryPhysicianLName;
                    //record.PrimaryPhysicianFName = admitinfo.PrimaryPhysicianFName;
                    //record.ReferringDoctor = admitinfo.ReferringDoctor;
                    //record.ConsultingDoctor = admitinfo.ConsultingDoctor;
                    //record.HospitalService = admitinfo.HospitalService;
                    //record.TemporaryLocation = admitinfo.TemporaryLocation;
                    //record.PreAdmitTestIndicator = admitinfo.PreAdmitTestIndicator;
                    //record.ReAdmissionIndicator = admitinfo.ReAdmissionIndicator;
                    //record.AdmitSource = admitinfo.AdmitSource;
                    //record.AmbulatoryStatus = admitinfo.AmbulatoryStatus;
                    //record.VIPIndicator = admitinfo.VIPIndicator;
                    //record.AdmittingDoctor = admitinfo.AdmittingDoctor;
                    //record.PatientType = admitinfo.PatientType;
                    //record.VisitNumber = admitinfo.VisitNumber;
                    //record.FinancialClass = admitinfo.FinancialClass;
                    //record.ChargePriceIndicator = admitinfo.ChargePriceIndicator;
                    //record.CourtesyCode = admitinfo.CourtesyCode;
                    //record.CreditRating = admitinfo.CreditRating;
                    //record.ContractCode = admitinfo.ContractCode;
                    //record.ContractEffDate = admitinfo.ContractEffDate;
                    //record.ContractAmount = admitinfo.ContractAmount;
                    //record.ContractPeriod = admitinfo.ContractPeriod;
                    //record.InterestCode = admitinfo.InterestCode;
                    //record.BadDebtCode = admitinfo.BadDebtCode;
                    //record.BadDebtDate = admitinfo.BadDebtDate;
                    //record.BadDebtAgencyCode = admitinfo.BadDebtAgencyCode;
                    //record.BadDebtTransferAmt = admitinfo.BadDebtTransferAmt;
                    //record.BadDebtRecoveryAmt = admitinfo.BadDebtRecoveryAmt;
                    //record.DeleteAccIndicator = admitinfo.DeleteAccIndicator;
                    //record.DeleteAccDate = admitinfo.DeleteAccDate;
                    //record.DischargeDisposition = admitinfo.DischargeDisposition;
                    //record.DischargedLocation = admitinfo.DischargedLocation;
                    //record.DietType = admitinfo.DietType;
                    //record.ServicingFacility = admitinfo.ServicingFacility;
                    //record.BedStatus = admitinfo.BedStatus;
                    //record.AccStatus = admitinfo.AccStatus;
                    //record.PendingLocation = admitinfo.PendingLocation;
                    //record.PriorTemporaryLocation = admitinfo.PriorTemporaryLocation;
                    //record.AdmitDate = admitinfo.AdmitDate;
                    record.DischargeDate = admitinfo.DischargeDate;
                    //record.CurrentPatientBalance = admitinfo.CurrentPatientBalance;
                    //record.TotalCharges = admitinfo.TotalCharges;
                    //record.TotalAdjustments = admitinfo.TotalAdjustments;
                    //record.TotalPayments = admitinfo.TotalPayments;
                    //record.AlternateVisitId = admitinfo.AlternateVisitId;
                    //record.VisitIndicator = admitinfo.VisitIndicator;
                    //record.OtherHealthProvider = admitinfo.OtherHealthProvider;
                    record.PVisit_Status = 2;
                    record.PVisit_CreatedBy = admitinfo.PVisit_CreatedBy;
                    record.PVisit_CreatedDate = admitinfo.PVisit_CreatedDate;
                    record.PVOutBoundFileStatus = admitinfo.PVOutBoundFileStatus;
                    record.PVOutBoundApproval = admitinfo.PVOutBoundApproval;
                    record.PVOutBoundApprovalBy = admitinfo.PVOutBoundApprovalBy;
                    record.PVOutBoundApprovalOn = admitinfo.PVOutBoundApprovalOn;
                    context.SaveChanges();

                    return 1;
                }
                return 0;
            }
            return 0;
        }
        public List<DemographicResidentDropEnity> GetAdminApprovalResidentsData()
        {
            var q1 = this.dbContext.ApprovalVisitInfoes.Where(pi => pi.PVOutBoundApproval == 0 || pi.PVOutBoundApproval == null).Select(pi => pi.Patient_Id).Distinct();
            var q2 = this.dbContext.ApprovalDemographics.Where(pi => pi.PDOutBoundApproval == 0 || pi.PDOutBoundApproval == null).Select(pi => pi.Patient_Id).Distinct();
            var q3 = this.dbContext.ApprovalDiagnosisInfoes.Where(pi => pi.PDGOutBoundApproval == 0 || pi.PDGOutBoundApproval == null).Select(pi => pi.Patient_Id).Distinct();
            var q4 = this.dbContext.ApprovalAllergyInfoes.Where(pi => pi.PAOutBoundApproval == 0 || pi.PAOutBoundApproval == null).Select(pi => pi.Patient_Id).Distinct();
            var q5 = this.dbContext.ApprovalOrders.Where(pi => pi.POOutBoundApproval == 0 || pi.POOutBoundApproval == null).Select(pi => pi.Patient_Id).Distinct();
            var q6 = this.dbContext.ApprovalRefills.Where(pi => pi.POOutBoundApproval == 0 || pi.POOutBoundApproval == null).Select(pi => pi.Patient_Id).Distinct();
            var q7 = this.dbContext.Demographics.Where(pi => q1.Contains(pi.Patient_Id) || q2.Contains(pi.Patient_Id) || q3.Contains(pi.Patient_Id) || q4.Contains(pi.Patient_Id) || q5.Contains(pi.Patient_Id) || q6.Contains(pi.Patient_Id))
                .Select(pi => new DemographicResidentDropEnity()
                {
                    Patient_Id = pi.Patient_Id,
                    PatientName = pi.PatientLastName + "," + pi.PatientFirstName + " " + (pi.PatientMiddleInitial==null?"": pi.PatientMiddleInitial),
                    PatientFirstName = pi.PatientFirstName,
                    PatientLastName = pi.PatientLastName,
                    PatientMiddleInitial = pi.PatientMiddleInitial
                }).OrderBy(item => item.PatientName).ToList();
            return q7;
        }
        public int RejectAllergy(ApprovalPendingCustomEntity entity)
        {
            ApprovalAllergyInfo allergyInfo = this.dbContext.ApprovalAllergyInfoes.Where(e => e.ApprovalPAllergy_Id == entity.ApprovalId && e.PAllergy_Id == entity.Record_Id).FirstOrDefault();

            if (allergyInfo != null)
            {
                allergyInfo.PAOutBoundApprovalBy = entity.ApprovedBy;
                allergyInfo.PAOutBoundApproval = entity.ApprovalStatus;
                allergyInfo.PAOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int RejectDiagnosis(ApprovalPendingCustomEntity entity)
        {
            ApprovalDiagnosisInfo diagnosisInfo = this.dbContext.ApprovalDiagnosisInfoes.Where(e => e.ApprovalPDiagnosis_Id == entity.ApprovalId && e.PDiagnosis_Id == entity.Record_Id).FirstOrDefault();

            if (diagnosisInfo != null)
            {
                diagnosisInfo.PDGOutBoundApprovalBy = entity.ApprovedBy;
                diagnosisInfo.PDGOutBoundApproval = entity.ApprovalStatus;
                diagnosisInfo.PDGOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                return 1;

            }
            return 0;
        }
        public int RejectDemographic(ApprovalPendingCustomEntity entity)
        {
            ApprovalDemographic demographicdata = this.dbContext.ApprovalDemographics.Where(e => e.ApprovalPatient_Id == entity.ApprovalId && e.Patient_Id == entity.Record_Id).FirstOrDefault();

            if (demographicdata != null)
            {
                demographicdata.PDOutBoundApprovalBy = entity.ApprovedBy;
                demographicdata.PDOutBoundApproval = entity.ApprovalStatus;
                demographicdata.PDOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int RejectTransferDischarge(ApprovalPendingCustomEntity entity)
        {
            ApprovalVisitInfo admitinfo = this.dbContext.ApprovalVisitInfoes.Where(e => e.ApprovalPVisit_Id == entity.ApprovalId && e.PVisit_Id == entity.Record_Id).FirstOrDefault();
            if (admitinfo != null)
            {
                admitinfo.PVOutBoundApprovalBy = entity.ApprovedBy;
                admitinfo.PVOutBoundApproval = entity.ApprovalStatus;
                admitinfo.PVOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public List<ApprovalChangesCustomEntity> GetModifiedData(int approvalId, int recordId, string category)
        {
            List<ApprovalChangesCustomEntity> data = new List<ApprovalChangesCustomEntity>();
            if (category == "Allergy")
            {
                data = (this.dbContext.PrcGetAllergyInfoChanges(recordId, approvalId).Select(e => new ApprovalChangesCustomEntity
                {
                    ColumnName = e.ColumnName,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    UpdatedBy = e.Company_UpdatedBy,
                    UpdatedOn = e.Company_UpdatedOn

                })).ToList();
            }
            else if (category == "Diagnosis")
            {
                data = (this.dbContext.PrcGetDiagnosisInfoChanges(recordId, approvalId).Select(e => new ApprovalChangesCustomEntity
                {
                    ColumnName = e.ColumnName,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    UpdatedBy = e.Company_UpdatedBy,
                    UpdatedOn = e.Company_UpdatedOn

                })).ToList();
            }
            else if (category == "Demographics")
            {
                data = (this.dbContext.PrcGetDempographicsChanges(recordId, approvalId).Select(e => new ApprovalChangesCustomEntity
                {
                    ColumnName = e.ColumnName,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    UpdatedBy = e.Company_UpdatedBy,
                    UpdatedOn = e.Company_UpdatedOn

                })).ToList();
            }
            else if (category == "Transfer")
            {
                data = (this.dbContext.PrcGetVisitInfoChanges(recordId, approvalId).Select(e => new ApprovalChangesCustomEntity
                {
                    ColumnName = e.ColumnName,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    UpdatedBy = e.Company_UpdatedBy,
                    UpdatedOn = e.Company_UpdatedOn

                })).ToList();
            }
            else if (category == "Discharge")
            {
                data = (this.dbContext.PrcGetVisitInfoChanges(recordId, approvalId).Select(e => new ApprovalChangesCustomEntity
                {
                    ColumnName = e.ColumnName,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    UpdatedBy = e.Company_UpdatedBy,
                    UpdatedOn = e.Company_UpdatedOn

                })).ToList();
            }
            else if (category == "NewOrder")
            {
                data = (this.dbContext.PrcGetOrderInfoChanges(approvalId).Select(e => new ApprovalChangesCustomEntity
                {
                    ColumnName = e.ColumnName,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    UpdatedBy = e.Company_UpdatedBy,
                    UpdatedOn = (DateTime)e.Company_UpdatedOn

                })).ToList();

            }
            //else if (category == "Refill")
            //{
            //    data = (this.dbContext.PrcApprovalRefillDetails(approvalId,recordId).Select(e => new ApprovalChangesCustomEntity
            //    {
            //        ColumnName = e.ColumnName,
            //        OldValue = e.OldValue,
            //        NewValue = e.NewValue,
            //        UpdatedBy = e.Company_UpdatedBy,
            //        UpdatedOn = (DateTime)e.Company_UpdatedOn

            //    })).ToList();

            //}
            return data;
        }
        public int InsertApprovalOrderData(ApprovalOrderEntity entity)
        {
            var checkRecords = this.dbContext.ApprovalOrders.Where(ap => ap.Patient_Id == entity.Patient_Id && ap.OrderControl == "NW").ToList();
            //foreach (var item in checkRecords)
            //{
            //    if (item.GiveCodeText == entity.GiveCodeText && item.StartDate == entity.StartDate)
            //    {
            //        return 2;
            //    }
            //}

            //Using RequestedGiveCode for GPI Code
            var record = this.autoMapper.Map<ApprovalOrderEntity, ApprovalOrder>(entity);
            if (record.Hours == 0)
                record.Hours = null;
            this.dbContext.ApprovalOrders.Add(record);
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.PApprovalOrder_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return (int)record.PApprovalOrder_Id;
        }
        public int InsertApprovalCPOEOrderData(ApprovalCPOEOrderEntity entity)
        {
            var record = this.autoMapper.Map<ApprovalCPOEOrderEntity, ApprovalOrder>(entity);
            if (record.Hours == 0)
                record.Hours = null;
            this.dbContext.ApprovalOrders.Add(record);
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.CPOE,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.PApprovalOrder_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return (int)record.PApprovalOrder_Id;
        }
        public List<ApprovalOrderEntity> GetApprovalPendingOrders()
        {
            var pendingNewOrders = this.dbContext.ApprovalOrders.Where(a => a.Porder_Id == 0 && (a.POOutBoundApproval == 0 || a.POOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalOrder>, List<ApprovalOrderEntity>>(pendingNewOrders);
        }

        public List<ApprovalOrderEntity> GetApprovalPendingOrdersByPatientId(int patientId)
        {
            var pendingNewOrders = this.dbContext.ApprovalOrders.Where(a => a.Patient_Id == patientId && a.Porder_Id == 0 && (a.POOutBoundApproval == 0 || a.POOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalOrder>, List<ApprovalOrderEntity>>(pendingNewOrders);
        }
        public int ApproveNewOrder(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalOrder record = context.ApprovalOrders.Where(e => e.PApprovalOrder_Id == entity.ApprovalId).FirstOrDefault();

            if (record != null)
            {
                var result = context.PrcInsertOrderData(entity.ApprovalId).SingleOrDefault();
                if (result > 0)
                {
                    record.POOutBoundApprovalBy = entity.ApprovedBy;
                    record.POOutBoundApproval = entity.ApprovalStatus;
                    record.POOutBoundApprovalOn = entity.ApprovedDate;
                    context.SaveChanges();
                }
                return (int)result;
            }
            return 0;
        }
        public int ApproveCPOENewOrder(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalOrder record = context.ApprovalOrders.Where(e => e.PApprovalOrder_Id == entity.ApprovalId).FirstOrDefault();

            if (record != null)
            {
                //var result = context.PrcInsertOrderData(entity.ApprovalId).SingleOrDefault();


                var result = ReturnOrderID(entity.ApprovalId);

                if (result > 0)
                {
                    record.POOutBoundApprovalBy = entity.ApprovedBy;
                    record.POOutBoundApproval = entity.ApprovalStatus;
                    record.POOutBoundApprovalOn = entity.ApprovedDate;
                    context.SaveChanges();
                }
                return (int)result;
            }
            return 0;
        }

        public int ReturnOrderID(Int64 ID)
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcInsertOrderData]", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ApprovalOrderId", SqlDbType.BigInt));
                adapt.SelectCommand.Parameters["@ApprovalOrderId"].Value = ID;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
              
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["POrderID"]);
                }
                else
                {
                    return 0;
                }
            }
               
        }
        public int RejectNewOrder(ApprovalPendingCustomEntity entity)
        {
            ApprovalOrder record = this.dbContext.ApprovalOrders.Where(e => e.PApprovalOrder_Id == entity.ApprovalId).FirstOrDefault();

            if (record != null)
            {
                record.POOutBoundApprovalBy = entity.ApprovedBy;
                record.POOutBoundApproval = entity.ApprovalStatus;
                record.POOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public List<ApprovalRefillEntity> GetApprovalPendingRefill()
        {
            var pendingRefill = this.dbContext.ApprovalRefills.Where(a => (a.POOutBoundApproval == 0 || a.POOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalRefill>, List<ApprovalRefillEntity>>(pendingRefill);
        }
        public List<ApprovalRefillEntity> GetApprovalPendingRefillByPatientId(int patientId)
        {
            var pendingRefill = this.dbContext.ApprovalRefills.Where(a => a.Patient_Id == patientId && (a.POOutBoundApproval == 0 || a.POOutBoundApproval == null)).ToList();
            return this.autoMapper.Map<List<ApprovalRefill>, List<ApprovalRefillEntity>>(pendingRefill);
        }
        public int ApproveRefill(ApprovalPendingCustomEntity entity, EMAREntities context)
        {
            ApprovalRefill record = context.ApprovalRefills.Where(e => e.Refill_Id == entity.ApprovalId).FirstOrDefault();

            if (record != null)
            {
                record.POOutBoundApprovalBy = entity.ApprovedBy;
                record.POOutBoundApproval = entity.ApprovalStatus;
                record.POOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                context.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int RejectRefill(ApprovalPendingCustomEntity entity)
        {
            ApprovalRefill record = this.dbContext.ApprovalRefills.Where(e => e.Refill_Id == entity.ApprovalId).FirstOrDefault();

            if (record != null)
            {
                record.POOutBoundApprovalBy = entity.ApprovedBy;
                record.POOutBoundApproval = entity.ApprovalStatus;
                record.POOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public string GetUserNameById(int userId)
        {
            var userName = this.dbContext.Users.Where(us => us.User_Id == userId).Select(us => us.User_DisplayName).FirstOrDefault();
            return userName;
        }
        public int RandomNumber()
        {
            Random rnd = new Random();
            int single = rnd.Next(10, 99);
            return single;
        }
        public int InsertNewDemographicDetails(NewResidentInfoEntity residentInfo, int ApprovalFlag)
        {
            string lastName = residentInfo.DemographicDetails.PatientLastName;
            string firstName = residentInfo.DemographicDetails.PatientFirstName;
            var externalPID = "X0" + (lastName.Length > 4 ? lastName.Substring(0, 4) : lastName) + (firstName.Length > 4 ? firstName.Substring(0, 4) : firstName);
            var result1 = this.dbContext.Demographics.Any(de => de.ExternalPatientId == externalPID);
            //var result2 = this.dbContext.Demographics.Any(de => de.PatientMRNumber == externalPID);
            if (result1 == false)// && result2==false)
            {
                residentInfo.DemographicDetails.ExternalPatientId = externalPID;
                //residentInfo.DemographicDetails.PatientMRNumber = externalPID;
            }
            else
            {
                externalPID = externalPID + Convert.ToDateTime(residentInfo.DemographicDetails.DOB).Day;
                if (this.dbContext.Demographics.Any(de => de.ExternalPatientId == externalPID) == true)// || this.dbContext.Demographics.Any(de => de.PatientMRNumber == externalPID) == true)
                {
                    int randomNum = RandomNumber();
                    var idWithNameDOB = externalPID;
                    externalPID = externalPID + randomNum;
                    while (this.dbContext.Demographics.Any(de => de.ExternalPatientId == externalPID) == true)// || this.dbContext.Demographics.Any(de => de.PatientMRNumber == externalPID)==true)
                    {
                        randomNum = RandomNumber();
                        externalPID = idWithNameDOB + randomNum;
                    }

                    residentInfo.DemographicDetails.ExternalPatientId = externalPID;
                    //residentInfo.DemographicDetails.PatientMRNumber = externalPID;
                }
                else
                {
                    residentInfo.DemographicDetails.ExternalPatientId = externalPID;
                    //residentInfo.DemographicDetails.PatientMRNumber = externalPID;
                }
            }

            //else if (compayUniqueId != null && compayUniqueId.Company_UniqueId == "PatientMRNumber")
            //{
            //    //var patientMrNum = compayUniqueId.Facility_ShortName.Substring(0, 4) + residentInfo.DemographicDetails.PatientLastName.Substring(0, 2) + Convert.ToDateTime(residentInfo.DemographicDetails.DOB).Day;
            //    var patientMrNum = "X0" + (lastName.Length > 4 ? lastName.Substring(0, 4) : lastName) + (firstName.Length > 4 ? firstName.Substring(0, 4) : firstName);
            //    var result = this.dbContext.Demographics.Any(de => de.PatientMRNumber == patientMrNum);

            //    if (result == false)
            //    {
            //        residentInfo.DemographicDetails.PatientMRNumber = patientMrNum;
            //    }
            //    else
            //    {
            //        //patientMrNum = compayUniqueId.Facility_ShortName.Substring(0, 4) + residentInfo.DemographicDetails.PatientFirstName.Substring(0, 2) + Convert.ToDateTime(residentInfo.DemographicDetails.DOB).Day;
            //        patientMrNum = patientMrNum + Convert.ToDateTime(residentInfo.DemographicDetails.DOB).Day;
            //        if (this.dbContext.Demographics.Any(de => de.PatientMRNumber == patientMrNum))
            //        {
            //            int randomNum = RandomNumber();
            //            //patientMrNum = compayUniqueId.Facility_ShortName.Substring(0, 4) + residentInfo.DemographicDetails.PatientFirstName.Substring(0, 2) + randomNum;
            //            var idWithNameDOB = patientMrNum;
            //            patientMrNum = patientMrNum + randomNum;
            //            while ((this.dbContext.Demographics.Any(de => de.PatientMRNumber == patientMrNum)) == true)
            //            {
            //                randomNum = RandomNumber();
            //                //patientMrNum = compayUniqueId.Facility_ShortName.Substring(0, 4) + residentInfo.DemographicDetails.PatientFirstName.Substring(0, 2) + randomNum;
            //                patientMrNum = idWithNameDOB + randomNum;
            //            }

            //            residentInfo.DemographicDetails.PatientMRNumber = patientMrNum;
            //        }
            //        else
            //        {
            //            residentInfo.DemographicDetails.PatientMRNumber = patientMrNum;
            //        }
            //    }
            //}
            if (ApprovalFlag == 0)
            {
                var demographicinfo = this.autoMapper.Map<DemographicCustomEntity, Demographic>(residentInfo.DemographicDetails);
                this.dbContext.Demographics.Add(demographicinfo);
                this.dbContext.SaveChanges();
                var visitinfoDetails = this.autoMapper.Map<VisitInfoCustomEntity, VisitInfo>(residentInfo.VisitInfoDetails);
                visitinfoDetails.Bed = residentInfo.VisitInfoDetails.Bed == "" ? null : visitinfoDetails.Bed;
                visitinfoDetails.Floor = residentInfo.VisitInfoDetails.Floor == "" ? null : visitinfoDetails.Floor;
                visitinfoDetails.Room = residentInfo.VisitInfoDetails.Room == "" ? null : visitinfoDetails.Room;
                visitinfoDetails.Patient_Id = demographicinfo.Patient_Id;
                this.dbContext.VisitInfoes.Add(visitinfoDetails);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NewResident,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = demographicinfo.Patient_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return demographicinfo.Patient_Id;
            }
            else if (ApprovalFlag == 1)
            {
                var demographics = this.autoMapper.Map<DemographicCustomEntity, ApprovalDemographic>(residentInfo.DemographicDetails);
                demographics.Patient_Id = null;
                this.dbContext.ApprovalDemographics.Add(demographics);
                this.dbContext.SaveChanges();
                var visitinfo = this.autoMapper.Map<VisitInfoCustomEntity, ApprovalVisitInfo>(residentInfo.VisitInfoDetails);
                visitinfo.PVisit_Id = null;
                visitinfo.Patient_Id = demographics.Patient_Id;
                this.dbContext.ApprovalVisitInfoes.Add(visitinfo);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NewResident,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = demographics.ApprovalPatient_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            return 0;
        }
        public int UpdateDemographicWithoutApproval(ApprovalDemographicEntity demographicdata, EMAREntities context)
        {

            Demographic record = context.Demographics.Find(demographicdata.Patient_Id);
            if (record != null)
            {
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
                context.SaveChanges();

                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DemographicInformation,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = demographicdata.ApprovalPatient_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);

                return 1;
            }
            return 0;

        }

        public int InsertUpdateAllergyWithoutApproval(ApprovalAllergyInfoEntity allergyInfo, EMAREntities context)
        {
            AllergyInfo record = context.AllergyInfoes.Where(e => e.PAllergy_Id == allergyInfo.PAllergy_Id).FirstOrDefault();
            if (record == null)
            {
                record = new AllergyInfo();
                record.Patient_Id = (int)allergyInfo.Patient_Id;
                record.AllergyType_Id = allergyInfo.AllergyType_Id;
                record.ClassDrugType = allergyInfo.ClassDrugType;
                record.ClassDrug_Id = allergyInfo.ClassDrug_Id;
                record.ClassDrug_Name = allergyInfo.ClassDrug_Name;
                record.NameOfCoding = allergyInfo.NameOfCoding;
                record.AllergySeverityCode = allergyInfo.AllergySeverityCode;
                record.AllergyReactionCode = allergyInfo.AllergyReactionCode;
                record.AllergyIdentificationDate = allergyInfo.AllergyIdentificationDate;
                record.PAllergy_Status = allergyInfo.PAllergy_Status;
                record.PAllergy_CreatedBy = allergyInfo.PAllergy_CreatedBy;
                record.PAllergy_CreatedDate = allergyInfo.PAllergy_CreatedDate;
                record.PAOutBoundApprovalBy = allergyInfo.PAOutBoundApprovalBy;
                record.PAOutBoundApproval = allergyInfo.PAOutBoundApproval;
                record.PAOutBoundApprovalOn = allergyInfo.PAOutBoundApprovalOn;

                context.AllergyInfoes.Add(record);
                context.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Allergies,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.PAllergy_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);

                return 1;
            }
            else
            {
                record.Patient_Id = (int)allergyInfo.Patient_Id;
                record.AllergyType_Id = allergyInfo.AllergyType_Id;
                record.ClassDrugType = allergyInfo.ClassDrugType;
                record.ClassDrug_Id = allergyInfo.ClassDrug_Id;
                record.ClassDrug_Name = allergyInfo.ClassDrug_Name;
                record.NameOfCoding = allergyInfo.NameOfCoding;
                record.AllergySeverityCode = allergyInfo.AllergySeverityCode;
                record.AllergyReactionCode = allergyInfo.AllergyReactionCode;
                record.AllergyIdentificationDate = allergyInfo.AllergyIdentificationDate;
                record.PAllergy_Status = allergyInfo.PAllergy_Status;
                record.PAllergy_CreatedBy = allergyInfo.PAllergy_CreatedBy;
                record.PAllergy_CreatedDate = allergyInfo.PAllergy_CreatedDate;
                record.PAOutBoundApprovalBy = allergyInfo.PAOutBoundApprovalBy;
                record.PAOutBoundApproval = allergyInfo.PAOutBoundApproval;
                record.PAOutBoundApprovalOn = allergyInfo.PAOutBoundApprovalOn;

                context.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Allergies,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.PAllergy_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);

                return 1;
            }

        }

        public int InsertUpdateDiagnosisWithoutApproval(ApprovalDiagnosisInfoEntity diagnosisInfo, EMAREntities context)
        {
            DiagnosisInfo record = context.DiagnosisInfoes.Where(e => e.PDiagnosis_Id == diagnosisInfo.PDiagnosis_Id).FirstOrDefault();
            if (record == null)
            {
                record = new DiagnosisInfo();
                record.Patient_Id = (int)diagnosisInfo.Patient_Id;
                record.ICD10_Id = diagnosisInfo.ICD10_Id;
                record.CodingMethod = diagnosisInfo.CodingMethod;
                record.DCodingType_Id = diagnosisInfo.DCodingType_Id;
                record.AltCodingId = diagnosisInfo.AltCodingId;
                record.AltCodingText = diagnosisInfo.AltCodingText;
                record.AltCodingMethod = diagnosisInfo.AltCodingMethod;
                record.DiagnosisDescription = diagnosisInfo.DiagnosisDescription;
                record.DiagnosisDate = diagnosisInfo.DiagnosisDate;
                record.AttestationDate = diagnosisInfo.AttestationDate;
                record.ConfidentialIndicator = diagnosisInfo.ConfidentialIndicator;
                record.DGAltCodingSystem = diagnosisInfo.DGAltCodingSystem;
                record.DGAlternateId = diagnosisInfo.DGAlternateId;
                record.DGAlternateText = diagnosisInfo.DGAlternateText;
                record.DGCodingSystem = diagnosisInfo.DGCodingSystem;
                record.DiagnosisActionCode = diagnosisInfo.DiagnosisActionCode;
                record.DiagnosisClassification = diagnosisInfo.DiagnosisClassification;
                record.DiagnosisIdentifier = diagnosisInfo.DiagnosisIdentifier;
                record.DiagnosisPriority = diagnosisInfo.DiagnosisPriority;
                record.DiagnosticGroupId = diagnosisInfo.DiagnosticGroupId;
                record.DiagnosticGroupText = diagnosisInfo.DiagnosticGroupText;
                record.DRGApprovalIndicator = diagnosisInfo.DRGApprovalIndicator;
                record.DRGGrouperReviewCode = diagnosisInfo.DRGGrouperReviewCode;
                record.FromValue = diagnosisInfo.FromValue;
                record.ToValue = diagnosisInfo.ToValue;
                record.GrouperVersion = diagnosisInfo.GrouperVersion;
                record.MajorDiagnosticId = diagnosisInfo.MajorDiagnosticId;
                record.MajorDiagnosticText = diagnosisInfo.MajorDiagnosticText;
                record.MDAltCodingSystem = diagnosisInfo.MDAltCodingSystem;
                record.MDAlternateId = diagnosisInfo.MDAlternateId;
                record.MDAlternateText = diagnosisInfo.MDAlternateText;
                record.MDCodingSystem = diagnosisInfo.MDCodingSystem;
                record.OAltCodingSystem = diagnosisInfo.OAltCodingSystem;
                record.OAlternateId = diagnosisInfo.OAlternateId;
                record.OAlternateText = diagnosisInfo.OAlternateText;
                record.OutlierCodingSystem = diagnosisInfo.OutlierCodingSystem;
                record.OutlierDays = diagnosisInfo.OutlierDays;
                record.OutlierDenomination = diagnosisInfo.OutlierDenomination;
                record.OutlierId = diagnosisInfo.OutlierId;
                record.OutlierQuantity = diagnosisInfo.OutlierQuantity;
                record.OutlierText = diagnosisInfo.OutlierText;
                record.PhysicianFName = diagnosisInfo.PhysicianFName;
                record.PhysicianLName = diagnosisInfo.PhysicianLName;
                record.PhysicianNPI = diagnosisInfo.PhysicianNPI;
                record.PriceType = diagnosisInfo.PriceType;
                record.RangeAltCodingSystem = diagnosisInfo.RangeAltCodingSystem;
                record.RangeAltId = diagnosisInfo.RangeAltId;
                record.RangeAltText = diagnosisInfo.RangeAltText;
                record.RangeCodingSystem = diagnosisInfo.RangeCodingSystem;
                record.RangeId = diagnosisInfo.RangeId;
                record.RangeText = diagnosisInfo.RangeText;
                record.RangeType = diagnosisInfo.RangeType;
                record.AttestationDate = diagnosisInfo.AttestationDate;
                record.DiagnosisType = diagnosisInfo.DiagnosisType;
                record.PDiagnosis_Status = diagnosisInfo.PDiagnosis_Status;
                record.PDiagnosis_CreatedBy = diagnosisInfo.PDiagnosis_CreatedBy;
                record.PDiagnosis_CreatedDate = diagnosisInfo.PDiagnosis_CreatedDate;
                record.PDGOutBoundFileStatus = diagnosisInfo.PDGOutBoundFileStatus;
                record.PDGOutBoundApproval = diagnosisInfo.PDGOutBoundApproval;
                record.PDGOutBoundApprovalBy = diagnosisInfo.PDGOutBoundApprovalBy;
                record.PDGOutBoundApprovalOn = diagnosisInfo.PDGOutBoundApprovalOn;

                context.DiagnosisInfoes.Add(record);
                context.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Diagnosis,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.PDiagnosis_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);


                return 1;
            }
            else
            {
                record.Patient_Id = (int)diagnosisInfo.Patient_Id;
                record.ICD10_Id = diagnosisInfo.ICD10_Id;
                record.CodingMethod = diagnosisInfo.CodingMethod;
                record.DCodingType_Id = diagnosisInfo.DCodingType_Id;
                record.AltCodingId = diagnosisInfo.AltCodingId;
                record.AltCodingText = diagnosisInfo.AltCodingText;
                record.AltCodingMethod = diagnosisInfo.AltCodingMethod;
                record.DiagnosisDescription = diagnosisInfo.DiagnosisDescription;
                record.DiagnosisDate = diagnosisInfo.DiagnosisDate;
                record.AttestationDate = diagnosisInfo.AttestationDate;
                record.ConfidentialIndicator = diagnosisInfo.ConfidentialIndicator;
                record.DGAltCodingSystem = diagnosisInfo.DGAltCodingSystem;
                record.DGAlternateId = diagnosisInfo.DGAlternateId;
                record.DGAlternateText = diagnosisInfo.DGAlternateText;
                record.DGCodingSystem = diagnosisInfo.DGCodingSystem;
                record.DiagnosisActionCode = diagnosisInfo.DiagnosisActionCode;
                record.DiagnosisClassification = diagnosisInfo.DiagnosisClassification;
                record.DiagnosisIdentifier = diagnosisInfo.DiagnosisIdentifier;
                record.DiagnosisPriority = diagnosisInfo.DiagnosisPriority;
                record.DiagnosticGroupId = diagnosisInfo.DiagnosticGroupId;
                record.DiagnosticGroupText = diagnosisInfo.DiagnosticGroupText;
                record.DRGApprovalIndicator = diagnosisInfo.DRGApprovalIndicator;
                record.DRGGrouperReviewCode = diagnosisInfo.DRGGrouperReviewCode;
                record.FromValue = diagnosisInfo.FromValue;
                record.GrouperVersion = diagnosisInfo.GrouperVersion;
                record.MajorDiagnosticId = diagnosisInfo.MajorDiagnosticId;
                record.MajorDiagnosticText = diagnosisInfo.MajorDiagnosticText;
                record.MDAltCodingSystem = diagnosisInfo.MDAltCodingSystem;
                record.MDAlternateId = diagnosisInfo.MDAlternateId;
                record.MDAlternateText = diagnosisInfo.MDAlternateText;
                record.MDCodingSystem = diagnosisInfo.MDCodingSystem;
                record.OAltCodingSystem = diagnosisInfo.OAltCodingSystem;
                record.OAlternateId = diagnosisInfo.OAlternateId;
                record.OAlternateText = diagnosisInfo.OAlternateText;
                record.OutlierCodingSystem = diagnosisInfo.OutlierCodingSystem;
                record.OutlierDays = diagnosisInfo.OutlierDays;
                record.OutlierDenomination = diagnosisInfo.OutlierDenomination;
                record.OutlierId = diagnosisInfo.OutlierId;
                record.OutlierQuantity = diagnosisInfo.OutlierQuantity;
                record.OutlierText = diagnosisInfo.OutlierText;
                record.PhysicianFName = diagnosisInfo.PhysicianFName;
                record.PhysicianLName = diagnosisInfo.PhysicianLName;
                record.PhysicianNPI = diagnosisInfo.PhysicianNPI;
                record.PriceType = diagnosisInfo.PriceType;
                record.RangeAltCodingSystem = diagnosisInfo.RangeAltCodingSystem;
                record.RangeAltId = diagnosisInfo.RangeAltId;
                record.RangeAltText = diagnosisInfo.RangeAltText;
                record.RangeCodingSystem = diagnosisInfo.RangeCodingSystem;
                record.RangeId = diagnosisInfo.RangeId;
                record.RangeText = diagnosisInfo.RangeText;
                record.RangeType = diagnosisInfo.RangeType;
                record.AttestationDate = diagnosisInfo.AttestationDate;
                record.ToValue = diagnosisInfo.ToValue;
                record.DiagnosisType = diagnosisInfo.DiagnosisType;
                record.PDiagnosis_Status = diagnosisInfo.PDiagnosis_Status;
                record.PDiagnosis_CreatedBy = diagnosisInfo.PDiagnosis_CreatedBy;
                record.PDiagnosis_CreatedDate = diagnosisInfo.PDiagnosis_CreatedDate;
                record.PDGOutBoundFileStatus = diagnosisInfo.PDGOutBoundFileStatus;
                record.PDGOutBoundApproval = diagnosisInfo.PDGOutBoundApproval;
                record.PDGOutBoundApprovalBy = diagnosisInfo.PDGOutBoundApprovalBy;
                record.PDGOutBoundApprovalOn = diagnosisInfo.PDGOutBoundApprovalOn;
                context.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Diagnosis,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.PDiagnosis_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);

                return 1;

            }

        }

        public int InsertUpdateApprovalRefill(ApprovalRefillEntity refillObj)
        {
            var refill = this.autoMapper.Map<ApprovalRefillEntity, ApprovalRefill>(refillObj);

            this.dbContext.ApprovalRefills.Add(refill);
            this.dbContext.SaveChanges();

            return (int)refill.Refill_Id;
        }
        public int DiscontinuedSplitOrder(OrdersCommonStatusEntity entity, EMAREntities context)
        {
            int totalSplitRecords = context.QuantityDetails.Where(e => e.POrder_Id == entity.OrderId).Count();
            int discontinueSplitRecords = context.QuantityDetails.Where(e => e.POrder_Id == entity.OrderId && e.OrderStatus == 2).Count();
            if (totalSplitRecords == discontinueSplitRecords)
            {
                return 1;
            }
            return 0;
        }
        public int DiscontinueOrder(OrdersCommonStatusEntity entity, EMAREntities context)
        {
            //var record = this.autoMapper.Map<OrdersCommonStatusEntity, CommonOrderInfo>(entity);
            if (entity.OrderType == "Discontinue")
            {
                var OrderRecord = context.CommonOrderInfoes.Where(cm => cm.POrder_Id == entity.OrderId).FirstOrDefault();
                if(OrderRecord!=null)
                {
                    OrderRecord.POrder_CreatedBy= entity.POrderCreatedBy;
                    OrderRecord.POrder_CreatedDate= (DateTime)entity.DiscontinuedOn;
                }
                if (entity.Split == 0 || entity.DiscontinueAllSplits == 0)
                {
                    var data = context.QuantityDetails.Where(e => e.POrder_Id == entity.OrderId && e.PQuantity_Id == entity.QuantityId).FirstOrDefault();
                    if (data != null)
                    {
                        data.OrderStatus = entity.POrderStatus;
                        data.DiscontinueFlag = entity.DiscontinueFlag;
                        data.DiscontinueReason = entity.DiscontinueReason;
                        data.PQuantity_CreatedBy = entity.POrderCreatedBy;
                        data.PQuantity_CreatedDate = (DateTime)entity.DiscontinuedOn;
                        context.SaveChanges();
                        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                        {
                            Screen_Id = (int)ScreenEntity.Screens.Orders,
                            Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                            Comments = entity.QuantityId.ToString(),
                            Session_Id = 0,
                            Time = DateTime.Now ,
                            UserActivity_Id = 0,
                        };

                        _userActivityRepository.InsertUserActivityDetails(activityEntity);

                        return 1;
                    }
                }
                else
                {
                    var records = context.QuantityDetails.Where(e => e.POrder_Id == entity.OrderId && e.OrderStatus != 2).ToList();
                    if (records.Count()>0)
                    {
                        foreach (var data in records)
                        {
                            data.OrderStatus = entity.POrderStatus;
                            data.DiscontinueFlag = entity.DiscontinueFlag;
                            data.DiscontinueReason = entity.DiscontinueReason;
                            data.PQuantity_CreatedBy = entity.POrderCreatedBy;
                            data.PQuantity_CreatedDate = (DateTime)entity.DiscontinuedOn;
                            context.SaveChanges();
                            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                            {
                                Screen_Id = (int)ScreenEntity.Screens.Orders,
                                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                                Comments = entity.QuantityId.ToString(),
                                Session_Id = 0,
                                Time = DateTime.Now ,
                                UserActivity_Id = 0,
                            };
                            _userActivityRepository.InsertUserActivityDetails(activityEntity);
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
        public int InsertApprovedRefillFileId(int patientId, int orderId, int fileId, EMAREntities context)
        {
            var record = context.ApprovalRefills.Where(ar => ar.Patient_Id == patientId && ar.Porder_Id == orderId).OrderByDescending(ar=>ar.Refill_CreatedDate).FirstOrDefault();
            if (record != null)
            {
                record.File_Id = fileId;
                context.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int PatientReAdmission(int patientId,DateTime AdmitDate)
        {
            var record = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == patientId).OrderByDescending(vs => vs.PVisit_Id).FirstOrDefault();
            if(record!=null)
            {
                record.AdmitDate = AdmitDate;
                record.ReAdmissionIndicator = "R";
                record.PVisit_Status = 1;
                record.DischargeDate = null;
                this.dbContext.SaveChanges();
                return patientId;
            }
            else
            {
                return 0;
            }
        }
        public int IsCPOEOrder(long approvalId, EMAREntities context)
        {
            var record = context.ApprovalOrders.AsEnumerable().Where(ap => ap.PApprovalOrder_Id == approvalId).FirstOrDefault();
            if(record!=null && record.WrittenDate!=null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

}
