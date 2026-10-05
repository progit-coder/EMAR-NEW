using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;

namespace LTCPro.Repositories
{
    public class AllergyInfoRepository : IAllergyInfoRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        public AllergyInfoRepository(IAutoMapper autoMapper, IDbContextEmar dbContext)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
        }

        //public List<AllergyInfoEntity> GetOutBoundAllergies()
        //{
        //    var approvedOBAllergies = this.dbContext.AllergyInfoes.Where(a => a.PAOutBoundFileStatus == 1 && a.PAOutBoundApproval == 1).ToList();
        //    return this.autoMapper.Map<List<AllergyInfo>, List<AllergyInfoEntity>>(approvedOBAllergies);
        //}
        //public List<AllergyInfoEntity> GetApprovalPendingAllergies(int patientId)
        //{
        //    var pendingAllergies = this.dbContext.AllergyInfoes.Where(a => a.Patient_Id == patientId && a.PAOutBoundFileStatus == 1 && (a.PAOutBoundApproval == 0 || a.PAOutBoundApproval == null)).ToList();
        //    return this.autoMapper.Map<List<AllergyInfo>, List<AllergyInfoEntity>>(pendingAllergies);
        //}
        //public List<AllergyInfoEntity> GetApprovalPendingAllergies()
        //{
        //    var pendingAllergies = this.dbContext.AllergyInfoes.Where(a => a.PAOutBoundFileStatus == 1 && (a.PAOutBoundApproval == 0 || a.PAOutBoundApproval == null)).ToList();
        //    return this.autoMapper.Map<List<AllergyInfo>, List<AllergyInfoEntity>>(pendingAllergies);
        //}

        public List<AllergyInfoCustomEntity> GetResidentAllergies(int patientId)
        {
            //    List<AllergyInfoCustomEntity> allergies = this.dbContext.AllergyInfoes.Where(p => p.Patient_Id == patientId).Select(a => new AllergyInfoCustomEntity
            //    {
            //        PAllergy_Id = a.PAllergy_Id,
            //        Patient_Id = a.Patient_Id,
            //        ClassDrug_Name = a.ClassDrug_Name,
            //        AllergyReactionCode = a.AllergyReactionCode,
            //        PAllergy_Status = a.PAllergy_Status,
            //        PAllergy_CreatedBy = a.PAllergy_CreatedBy,
            //        PAllergy_CreatedDate = a.PAllergy_CreatedDate
            //    }).ToList();


            //  return allergies;
            List<AllergyInfoCustomEntity> allergies = (from allergyinfo in this.dbContext.AllergyInfoes
                                                       join user in this.dbContext.Users on allergyinfo.PAllergy_CreatedBy equals user.User_Id
                                                       where allergyinfo.Patient_Id == patientId
                                                       select new AllergyInfoCustomEntity
                                                       {
                                                           AllergyReactionCode = allergyinfo.AllergyReactionCode,
                                                           ClassDrug_Name = allergyinfo.ClassDrug_Name,
                                                           Allergy_CreatedBy = user.User_DisplayName,
                                                           PAllergy_CreatedDate = allergyinfo.PAllergy_CreatedDate,
                                                           PAllergy_Id = allergyinfo.PAllergy_Id,
                                                           PAllergy_Status = allergyinfo.PAllergy_Status,
                                                           Patient_Id = allergyinfo.Patient_Id,
                                                           ClassDrugType=allergyinfo.ClassDrugType
                                                       }

                                                    ).ToList();
            return allergies;
        }

        //Need to remove this. Moved functionality to AdminApprovalAllergieies

        //public int InsertUpdateAllergyInfo(AllergyInfoEntity entity)
        //{
        //    //var record = this.autoMapper.Map<AllergyInfoEntity, AllergyInfo>(entity);
        //    //if (record.PAllergy_Id == 0)
        //    //{
        //    //    int namecode = Convert.ToInt32(record.NameOfCoding);
        //    //    if (namecode == 1)
        //    //        record.NameOfCoding = "MDDX";
        //    //    else if (namecode == 2)
        //    //        record.NameOfCoding = "NDC";
        //    //    else if (namecode == 3)
        //    //        record.NameOfCoding = "KDC10";
        //    //    this.dbContext.AllergyInfoes.Add(record);
        //    //    this.dbContext.SaveChanges();

        //    //    return 1;
        //    //}
        //    //else
        //    //{
        //    //    return 0;
        //    //}
        //    return 0;
        //}

        //public int GenerateOutboundFileForUpdated(int patientId, string category)
        //{
        //    var result = this.dbContext.PrcGenerateHL7forPatientUpdate(patientId, category);
        //    return 1;
        //}
        //public int ApproveAllergy(ApprovalPendingCustomEntity entity)
        //{
        //    AllergyInfo allergyInfo = this.dbContext.AllergyInfoes.Find(entity.Record_Id);
        //    if (allergyInfo.Patient_Id == entity.Patient_Id)
        //    {
        //        allergyInfo.PAOutBoundApprovalBy = entity.ApprovedBy;
        //        allergyInfo.PAOutBoundApproval = entity.ApprovalStatus;
        //        allergyInfo.PAOutBoundApprovalOn = entity.ApprovedDate;
        //        this.dbContext.SaveChanges();

        //        return 1;
        //    }
        //    return 0;
        //}
        //public AllergyInfoEntity GetAllergyInfoDetails(int PAllergy_Id)
        //{
        //    //var integrationdetails = this.dbContext.ApiIntegrations.Find(integrationID);
        //    //return this.autoMapper.Map<ApiIntegration, ApiIntegrationEntity>(integrationdetails);
        //    return null;
        //}
        public AllergyInfoEntity GetAllegyDetails(int AllergyId)
        {
            var allergies = this.dbContext.AllergyInfoes.Where(p => p.PAllergy_Id == AllergyId).FirstOrDefault();
            var approvalallergies = this.dbContext.ApprovalAllergyInfoes.Where(ap => ap.Patient_Id == allergies.Patient_Id && ap.PAllergy_Id == AllergyId && (ap.PAOutBoundApproval == null || ap.PAOutBoundApproval == 0)).FirstOrDefault();

            AllergyInfoEntity allergyData = new AllergyInfoEntity();
            allergyData.PAllergy_Id = AllergyId;
            allergyData.Patient_Id = allergies.Patient_Id;
            allergyData.AllergyType_Id = allergies.AllergyType_Id;
            allergyData.ClassDrug_Id = allergies.ClassDrug_Id;
            allergyData.ClassDrug_Name = allergies.ClassDrug_Name;
            allergyData.NameOfCoding = allergies.NameOfCoding;
            allergyData.AllergySeverityCode = allergies.AllergySeverityCode;
            allergyData.AllergyReactionCode = allergies.AllergyReactionCode;
            allergyData.AllergyIdentificationDate = allergies.AllergyIdentificationDate;
            allergyData.PAllergy_Status = allergies.PAllergy_Status;
            allergyData.PAllergy_CreatedBy = allergies.PAllergy_CreatedBy;
            allergyData.PAllergy_CreatedDate = allergies.PAllergy_CreatedDate;
            allergyData.ClassDrugType = allergies.ClassDrugType;
            allergyData.PAOutBoundFileStatus = allergies.PAOutBoundFileStatus;
            if (approvalallergies != null)
                allergyData.PAOutBoundApproval = 0;
            else
                allergyData.PAOutBoundApproval = 1;
            allergyData.PAOutBoundApprovalBy = allergies.PAOutBoundApprovalBy;
            allergyData.PAOutBoundApprovalOn = allergies.PAOutBoundApprovalOn;
            return allergyData;

        }

        public int RemoveResidentAllergiesInfo(int pAllergyId)
        {
            var record = this.dbContext.AllergyInfoes.Where(item => item.PAllergy_Id == pAllergyId).FirstOrDefault();
            if (record != null)
            {
                this.dbContext.AllergyInfoes.Remove(record);
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
         
        }


    }
}
