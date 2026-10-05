using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.Collections;
using LTCPro.Repositories;
namespace LTCPro.Repositories
{
    public class DrFirstIntegrationRepository : IDrFirstIntegrationRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public DrFirstIntegrationRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }
        public int InsertUpdateApiIntegration(ApiIntegrationEntity integrationdata)
        {
            var integrationrecord = this.autoMapper.Map<ApiIntegrationEntity, ApiIntegration>(integrationdata);
            if (integrationrecord.Integration_Id == 0)
            {
                this.dbContext.ApiIntegrations.Add(integrationrecord);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Integrations,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = integrationrecord.Integration_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            else
            {
                ApiIntegration record = this.dbContext.ApiIntegrations.Find(integrationrecord.Integration_Id);
                record.Integration_TypeId = integrationrecord.Integration_TypeId;
                record.ApiPath = integrationrecord.ApiPath;
                record.UserName = integrationrecord.UserName;
                record.Password = integrationrecord.Password;
                record.Integration_Status = integrationrecord.Integration_Status;

                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Integrations,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.Integration_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            this.dbContext.SaveChanges();
            return 1;
        }
        public List<ApiIntegrationEntity> GetApiIntegrationAllData(int companyID)
        {
            var records = (from api in this.dbContext.ApiIntegrations
                           join us in this.dbContext.Users on api.Integration_CreatedBy equals us.User_Id
                           where api.Company_Id == companyID
                           select new ApiIntegrationEntity
                           {
                               Integration_Id = api.Integration_Id,
                               Integration_TypeId = api.Integration_TypeId,
                               Company_Id = api.Company_Id,
                               ApiPath = api.ApiPath,
                               UserName = api.UserName,
                               Password = api.Password,
                               Integration_Status = api.Integration_Status,
                               Integration_CreatedBy = api.Integration_CreatedBy,
                               Integration_CreatedDate = api.Integration_CreatedDate,
                               CratedUserName = us.User_DisplayName,
                           }).ToList();
            return records;
            //var integrationdata = this.dbContext.ApiIntegrations.Where(ap => ap.Company_Id == companyID).ToList();
            //return this.autoMapper.Map<List<ApiIntegration>, List<ApiIntegrationEntity>>(integrationdata);
        }
        public ApiIntegrationEntity GetApiIntegrationDetails(int integrationID)
        {
            var integrationdetails = this.dbContext.ApiIntegrations.Find(integrationID);
            return this.autoMapper.Map<ApiIntegration, ApiIntegrationEntity>(integrationdetails);
        }
        public string DrFirstXMLSave(string xmlData)
        {
            var result = this.dbContext.PrcInsertDrFirstData(xmlData);
            return "Done";
        }

        public List<DrFirstOrderCheck_ResultEntity> GetDrFirstOrderInfo(int orderId)
        {
            var drFirstOrderInfo = this.dbContext.DrFirstOrderCheck(orderId).ToList();
            return this.autoMapper.Map<List<DrFirstOrderCheck_Result>, List<DrFirstOrderCheck_ResultEntity>>(drFirstOrderInfo);
        }
        public int UpdateApprovalStatus(int prescriptionId, string userName, string password)
        {
            var user = this.dbContext.Users.Where(us => us.UserName == userName && us.Password == password).FirstOrDefault();
            if (user == null)
            {
                return 0;
            }
            else
            {
                DrFirstOrderXMLTran details = new DrFirstOrderXMLTran();
                details = this.dbContext.DrFirstOrderXMLTrans.Where(dr => dr.PrescriptionID == prescriptionId.ToString()).FirstOrDefault();
                if (details != null)
                {
                    details.DrFirstOrderXMLTrans_Approval = 1;
                    details.DrFirstOrderXMLTrans_ApprovalOn = DateTime.Now;
                    details.DrFirstOrderXMLTrans_ApprovalBy = user.User_Id;
                    this.dbContext.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }

            }

        }

        public List<DrFirstOrderCheck_ResultEntity> GetDrFirstOrderCheck(int patientId)
        {
            var drFirstOrderInfo = this.dbContext.DrFirstOrderCheck(patientId).ToList();
            return this.autoMapper.Map<List<DrFirstOrderCheck_Result>, List<DrFirstOrderCheck_ResultEntity>>(drFirstOrderInfo);
        }

        public List<PrcDrFirstOrderHL7_ResultEntity> GetHL7DrFirstOrderCheck(int patientId)
        {
            var hl7DrFirstOrderInfo = this.dbContext.PrcDrFirstOrderHL7(patientId).ToList();
            return this.autoMapper.Map<List<PrcDrFirstOrderHL7_Result>, List<PrcDrFirstOrderHL7_ResultEntity>>(hl7DrFirstOrderInfo);
        }

        public int InsertDrFirstMap(List<DrFirstEntity> entity)
        {
            string PrescriptionId = "";
            foreach (var record in entity)
            {

                if (record.type == 1)
                {
                    PrescriptionId = record.Prescription;
                    var data = this.dbContext.DrFirstOrderXMLTrans.Where(dr => dr.PrescriptionID == record.Prescription).FirstOrDefault();
                    if (data != null)
                    {
                        data.DrFirstOrderXMLTrans_Approval = 1;
                        data.DrFirstOrderXMLTrans_ApprovalBy = record.ApprovalBy;
                        data.DrFirstOrderXMLTrans_ApprovalOn = DateTime.Now;
                        this.dbContext.SaveChanges();

                    }
                }
                else if (record.type == 2)
                {
                    int orderId = Convert.ToInt16(record.Prescription);
                    var data = this.dbContext.CommonOrderInfoes.Where(cm => cm.POrder_Id == orderId).FirstOrDefault();
                    if (data != null)
                    {
                        data.DrFirstPrescriptionId = PrescriptionId;
                        this.dbContext.SaveChanges();
                    }

                }
            }

            return 1;
        }
        public List<NurseCommentTypeDropEntity> GetNurseComments()
        {
            var nurseComments = this.dbContext.NurseCommentTypes.ToList();
            return this.autoMapper.Map<List<NurseCommentType>, List<NurseCommentTypeDropEntity>>(nurseComments);
        }
        public List<MedicationReasonEntity> GetNoteComments()
        {
            var nurseComments = this.dbContext.MedicationReasons.Where(x=>x.MedicationReason_ID!=1).ToList();
            return this.autoMapper.Map<List<MedicationReason>, List<MedicationReasonEntity>>(nurseComments);
        }


        public List<PrcGetReportDrFirst_ResultEntity> PrcGetReportDrFirst(int patientId)
        {
            var hl7DrFirstOrderInfo = this.dbContext.PrcGetReportDrFirst(patientId.ToString() == "0" ? null : patientId.ToString()).ToList();
            return this.autoMapper.Map<List<PrcGetReportDrFirst_Result>, List<PrcGetReportDrFirst_ResultEntity>>(hl7DrFirstOrderInfo);
        }
        public List<DemographicResidentDropEnity> GetDrFirstResidentDrop(int userId)
        {
            List<int> patientIds = null;
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


            patientIds = this.dbContext.VisitInfoes.Where(p => p.DischargeDate == null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
            if (patientIds != null)
            {
                var records = (from co in this.dbContext.CommonOrderInfoes
                               join de in this.dbContext.Demographics on co.Patient_Id equals de.Patient_Id
                               where co.POrder_Status == 1 && co.POOutBoundApproval == 1 && patientIds.Distinct().Contains(co.Patient_Id) 

                               select new DemographicResidentDropEnity
                               {
                                   Patient_Id = co.Patient_Id,
                                   PatientFirstName = de.PatientFirstName,
                                   PatientLastName = de.PatientLastName,
                                   PatientMiddleInitial = de.PatientMiddleInitial
                               }).Distinct().ToList();
                return records;
            }
            return null;
        }
        public int InsertHlSevenApprove(HlSevenApproveEntity entity)
        {
            CommonOrderInfo record = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == entity.POrderId).FirstOrDefault();
            if(record!=null)
            {
                if (entity.DAdminId != 0)
                {
                    this.dbContext.PrcOrderScheduleTime(entity.POrderId, entity.DAdminId);
                }
                record.POOutBoundApproval = entity.ApprovalStatus;
                record.POOutBoundApprovalBy = entity.ApprovedBy;
                record.POOutBoundApprovalOn = entity.ApprovedDate;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public int InsertDrFirstApprove(int usetId,int DrFirstOrderId)
        {
            DrFirstOrderXMLTran record = this.dbContext.DrFirstOrderXMLTrans.Where(co => co.DrFirstOrder_Id == DrFirstOrderId).FirstOrDefault();
            if (record != null)
            {
                record.DrFirstOrderXMLTrans_Approval =1;
                record.DrFirstOrderXMLTrans_ApprovalBy = usetId;
                record.DrFirstOrderXMLTrans_ApprovalOn = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public string DownloadXML(int fileId)
        {
            return this.dbContext.DrFirstFileDatas.Where(i => i.DrFirstFile_Id == fileId).Select(i=>i.FilePath).FirstOrDefault();
        }
    }
}
