using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;

namespace LTCPro.Repositories
{
    public class DiagnosisInfoRepository : IDiagnosisInfoRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        public DiagnosisInfoRepository(IAutoMapper autoMapper, IDbContextEmar dbContext)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
        }

        //public List<DiagnosisInfoEntity> GetApprovalPendingDiagnosis(int patientId)
        //{
        //    var pendingDiagnosis = this.dbContext.DiagnosisInfoes.Where(a => a.Patient_Id == patientId && a.PDGOutBoundFileStatus == 1 && (a.PDGOutBoundApproval == 1 || a.PDGOutBoundApproval == null)).ToList();
        //    return this.autoMapper.Map<List<DiagnosisInfo>, List<DiagnosisInfoEntity>>(pendingDiagnosis);
        //}
        //public List<DiagnosisInfoEntity> GetApprovalPendingDiagnosis()
        //{
        //    var pendingDiagnosis = this.dbContext.DiagnosisInfoes.Where(a => a.PDGOutBoundFileStatus == 1 && (a.PDGOutBoundApproval == 1 || a.PDGOutBoundApproval == null)).ToList();
        //    return this.autoMapper.Map<List<DiagnosisInfo>, List<DiagnosisInfoEntity>>(pendingDiagnosis);
        //}

        //public List<DiagnosisInfoEntity> GetOutBoundDiagnosis()
        //{
        //    var approvedOBDiagnosis = this.dbContext.DiagnosisInfoes.Where(a => a.PDGOutBoundFileStatus == 1 && a.PDGOutBoundApproval == 1).ToList();
        //    return this.autoMapper.Map<List<DiagnosisInfo>, List<DiagnosisInfoEntity>>(approvedOBDiagnosis);
        //}

        public List<DiagnosisInfoCustomEntity> GetResidentDiagnosisInfo(int patientId)
        {
            List<DiagnosisInfoCustomEntity> diagnosis =
                (from d in this.dbContext.DiagnosisInfoes
                 join u in this.dbContext.Users on d.PDiagnosis_CreatedBy equals u.User_Id
                 where d.Patient_Id == patientId
                 select new DiagnosisInfoCustomEntity
                {
                    PDiagnosis_Id = d.PDiagnosis_Id,
                    Patient_Id = d.Patient_Id,
                    DiagnosisDescription = d.DiagnosisDescription==null?d.AltCodingText : d.DiagnosisDescription,
                    AltCodingId=d.AltCodingId,
                    PDiagnosis_Status = d.PDiagnosis_Status,
                    PDiagnosis_CreatedBy = d.PDiagnosis_CreatedBy,
                    PDiagnosis_CreatedDate = d.PDiagnosis_CreatedDate,
                    CreatedBy = u.User_DisplayName,
                }).ToList();
        
            return diagnosis;
        }
        //public int ApproveDiagnosis(ApprovalPendingCustomEntity entity)
        //{
        //    DiagnosisInfo diagnosisInfo = this.dbContext.DiagnosisInfoes.Find(entity.Record_Id);
        //    if (diagnosisInfo.Patient_Id == entity.Patient_Id)
        //    {
        //        diagnosisInfo.PDGOutBoundApprovalBy = entity.ApprovedBy;
        //        diagnosisInfo.PDGOutBoundApproval = entity.ApprovalStatus;
        //        diagnosisInfo.PDGOutBoundApprovalOn = entity.ApprovedDate;
        //        this.dbContext.SaveChanges();

        //        return 1;
        //    }
        //    return 0;
        //}
        //public int InsertUpdateDiagnosisInfo(DiagnosisInfoCustomEntity entity)
        //{
        //    DiagnosisInfo record = new DiagnosisInfo();
        //    if (entity.PDiagnosis_Id == 0)
        //    {
        //        this.dbContext.DiagnosisInfoes.Add(record);
        //        this.dbContext.SaveChanges();

        //        return 1;
                
        //    }
        //    else
        //    {
        //        record.Patient_Id = entity.Patient_Id;
        //        record.ICD10_Id = entity.ICD10_Id;
        //        record.CodingMethod = entity.CodingMethod;
        //        record.DCodingType_Id = entity.DCodingType_Id;
        //        record.AltCodingId = entity.AltCodingId;
        //        record.AltCodingText = entity.AltCodingText;
        //        record.AltCodingMethod = entity.AltCodingMethod;
        //        record.DiagnosisDescription = entity.DiagnosisDescription;
        //        record.DiagnosisDate = entity.DiagnosisDate;
        //        record.PDiagnosis_Status = entity.PDiagnosis_Status;
        //        record.PDiagnosis_CreatedBy = entity.PDiagnosis_CreatedBy;
        //        record.PDiagnosis_CreatedDate = entity.PDiagnosis_CreatedDate;
        //        record.PDGOutBoundFileStatus = entity.PDGOutBoundFileStatus;
        //        record.PDGOutBoundApproval = entity.PDGOutBoundApproval;
        //        record.PDGOutBoundApprovalBy = entity.PDGOutBoundApprovalBy;
        //        record.PDGOutBoundApprovalOn = null;

        //        this.dbContext.SaveChanges();

        //        return 1;
        //    }
        //}
        public DiagnosisInfoEntity GetDiagnosisDetails(int DiagnosisId)
        {
            var diagnosis = this.dbContext.DiagnosisInfoes.Where(p => p.PDiagnosis_Id == DiagnosisId).FirstOrDefault();
            if (diagnosis != null)
            {
                var approvaldiagnosis = this.dbContext.ApprovalDiagnosisInfoes.Where(ad => ad.Patient_Id == diagnosis.Patient_Id && ad.PDiagnosis_Id == DiagnosisId && (ad.PDGOutBoundApproval == null || ad.PDGOutBoundApproval == 0)).FirstOrDefault();

                DiagnosisInfoEntity diagnosisData = new DiagnosisInfoEntity();
                diagnosisData.PDiagnosis_Id = DiagnosisId;
                diagnosisData.Patient_Id = diagnosis.Patient_Id;
                diagnosisData.CodingMethod = diagnosis.CodingMethod;
                diagnosisData.ICD10_Id = diagnosis.ICD10_Id;
                diagnosisData.DCodingType_Id = diagnosis.DCodingType_Id;
                diagnosisData.AltCodingId = diagnosis.AltCodingId;
                diagnosisData.AltCodingText = diagnosis.AltCodingText;
                diagnosisData.AltCodingMethod = diagnosis.AltCodingMethod;
                diagnosisData.DiagnosisDescription = diagnosis.DiagnosisDescription;
                diagnosisData.DiagnosisDate = diagnosis.DiagnosisDate;
                diagnosisData.DiagnosisType = diagnosis.DiagnosisType;
                diagnosisData.MajorDiagnosticId = diagnosis.MajorDiagnosticId;
                diagnosisData.MajorDiagnosticText = diagnosis.MajorDiagnosticText;
                diagnosisData.MDCodingSystem = diagnosis.MDCodingSystem;
                diagnosisData.MDAlternateId = diagnosis.MDAlternateId;
                diagnosisData.MDAlternateText = diagnosis.MDAlternateText;
                diagnosisData.MDAltCodingSystem = diagnosis.MDAltCodingSystem;
                diagnosisData.DiagnosticGroupId = diagnosis.DiagnosticGroupId;
                diagnosisData.DiagnosticGroupText = diagnosis.DiagnosticGroupText;
                diagnosisData.DGCodingSystem = diagnosis.DGCodingSystem;
                diagnosisData.DGAlternateId = diagnosis.DGAlternateId;
                diagnosisData.DGAlternateText = diagnosis.DGAlternateText;
                diagnosisData.DGAltCodingSystem = diagnosis.DGAltCodingSystem;
                diagnosisData.DRGApprovalIndicator = diagnosis.DRGApprovalIndicator;
                diagnosisData.DRGGrouperReviewCode = diagnosis.DRGGrouperReviewCode;
                diagnosisData.OutlierId = diagnosis.OutlierId;
                diagnosisData.OutlierText = diagnosis.OutlierText;
                diagnosisData.OutlierCodingSystem = diagnosis.OutlierCodingSystem;
                diagnosisData.OAlternateId = diagnosis.OAlternateId;
                diagnosisData.OAlternateText = diagnosis.OAlternateText;
                diagnosisData.OAltCodingSystem = diagnosis.OAltCodingSystem;
                diagnosisData.OutlierDays = diagnosis.OutlierDays;
                diagnosisData.OutlierQuantity = diagnosis.OutlierQuantity;
                diagnosisData.OutlierDenomination = diagnosis.OutlierDenomination;
                diagnosisData.PriceType = diagnosis.PriceType;
                diagnosisData.FromValue = diagnosis.FromValue;
                diagnosisData.ToValue = diagnosis.ToValue;
                diagnosisData.RangeId = diagnosis.RangeId;
                diagnosisData.RangeText = diagnosis.RangeText;
                diagnosisData.RangeCodingSystem = diagnosis.RangeCodingSystem;
                diagnosisData.RangeAltId = diagnosis.RangeAltId;
                diagnosisData.RangeAltText = diagnosis.RangeAltText;
                diagnosisData.RangeAltCodingSystem = diagnosis.RangeAltCodingSystem;
                diagnosisData.GrouperVersion = diagnosis.GrouperVersion;
                diagnosisData.DiagnosisPriority = diagnosis.DiagnosisPriority;
                diagnosisData.ConfidentialIndicator = diagnosis.ConfidentialIndicator;
                diagnosisData.AttestationDate = diagnosis.AttestationDate;
                diagnosisData.DiagnosisIdentifier = diagnosis.DiagnosisIdentifier;
                diagnosisData.DiagnosisActionCode = diagnosis.DiagnosisActionCode;
                diagnosisData.PDiagnosis_Status = diagnosis.PDiagnosis_Status;
                diagnosisData.PDiagnosis_CreatedBy = diagnosis.PDiagnosis_CreatedBy;
                diagnosisData.PDiagnosis_CreatedDate = diagnosis.PDiagnosis_CreatedDate;
                diagnosisData.PDGOutBoundFileStatus = diagnosis.PDGOutBoundFileStatus;
                diagnosisData.RangeType = diagnosis.RangeType;
                diagnosisData.AttestationDate = diagnosis.AttestationDate;
                diagnosisData.PhysicianNPI = diagnosis.PhysicianNPI;
                diagnosisData.PhysicianFName = diagnosis.PhysicianFName;
                diagnosisData.PhysicianLName = diagnosis.PhysicianLName;
                diagnosisData.DiagnosisClassification = diagnosis.DiagnosisClassification;
                diagnosisData.ICD10_RawFormat = this.dbContext.ICD10.Where(icd => icd.ICD10_Id == diagnosis.ICD10_Id).Select(ic => ic.ICD10_Description).FirstOrDefault();

                if (approvaldiagnosis != null)
                    diagnosisData.PDGOutBoundApproval = 0;
                else
                    diagnosisData.PDGOutBoundApproval = 1;
                diagnosisData.PDGOutBoundApprovalBy = diagnosis.PDGOutBoundApprovalBy;
                diagnosisData.PDGOutBoundApprovalOn = diagnosis.PDGOutBoundApprovalOn;

                return diagnosisData;
            }
            else
            {
                return null;
            }
        }
        public int RemoveResidentDiagnosis(int pDiagnosisId)
        {
            var record = this.dbContext.DiagnosisInfoes.Where(item => item.PDiagnosis_Id == pDiagnosisId).FirstOrDefault();
            if (record != null)
            {
                this.dbContext.DiagnosisInfoes.Remove(record);
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        
    }
}
