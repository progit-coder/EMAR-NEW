using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        public AuditRepository(IAutoMapper autoMapper, IDbContextEmar dbContext)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
        }

        public AuditCustomEntity GetAuditTableData(string tableName, int recordId)
        {
            AuditCustomEntity auditEntity = new AuditCustomEntity();

            if (tableName == "User")
            {
                //auditEntity.ColumnNames = typeof(User1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetUserChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "Company")
            {
                //auditEntity.ColumnNames = typeof(Company1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetCompanyChangesdetails(recordId).OrderByDescending(r=>r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "Facility")
            {
                //auditEntity.ColumnNames = typeof(Facility1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetFacilityChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "NurseStation")
            {
                //auditEntity.ColumnNames = typeof(NurseStation1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetNursingStationChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "Wing")
            {
                //auditEntity.ColumnNames = typeof(Wing1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetWingChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "Floor")
            {
                //auditEntity.ColumnNames = typeof(Floor1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetFloorChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "Room")
            {
                //auditEntity.ColumnNames = typeof(Room1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetRoomChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "Bed")
            {
                //auditEntity.ColumnNames = typeof(Bed1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetBedChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "CommonOrderInfo")
            {
                //auditEntity.ColumnNames = typeof(CommonOrderInfo1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                //auditEntity.Records = (from commonorderinfo in this.dbContext.CommonOrderInfo1


                //                       join u1 in this.dbContext.Users on commonorderinfo.POrder_UpdatedBy equals u1.User_Id
                //                       where commonorderinfo.POrder_Id == recordId
                //                       select new CommonOrderInfo1Entity
                //                       {
                //                           AuditPOrder_Id = commonorderinfo.AuditPOrder_Id,
                //                           POrder_Id = commonorderinfo.POrder_Id,
                //                           Patient_Id = commonorderinfo.Patient_Id,
                //                           OrderingPhysicianID = commonorderinfo.OrderingPhysicianID,
                //                           OrderControl = commonorderinfo.OrderControl,
                //                           PlacerOrderNumber = commonorderinfo.PlacerOrderNumber,
                //                           FacilityId = commonorderinfo.FacilityId,
                //                           PatientId = commonorderinfo.PatientId,
                //                           Room = commonorderinfo.Room,
                //                           OrderTypeID = commonorderinfo.OrderTypeID,
                //                           PlacerGroupNumber = commonorderinfo.PlacerGroupNumber,
                //                           OrderStatus = commonorderinfo.OrderStatus,
                //                           ResponseFlag = commonorderinfo.ResponseFlag,
                //                           QuantityTiming = commonorderinfo.QuantityTiming,
                //                           Parent = commonorderinfo.Parent,
                //                           TransactionDate = commonorderinfo.TransactionDate,
                //                           EnteredBy = commonorderinfo.EnteredBy,
                //                           EPharmacistLName = commonorderinfo.EPharmacistLName,
                //                           EPharmacistFName = commonorderinfo.EPharmacistFName,
                //                           VerifiedBy = commonorderinfo.VerifiedBy,
                //                           VPharmacistLName = commonorderinfo.VPharmacistLName,
                //                           VPharmacistFName = commonorderinfo.VPharmacistFName,
                //                           VEffectivedate = commonorderinfo.VEffectivedate,
                //                           OrderingPhysicianNPI = commonorderinfo.OrderingPhysicianNPI,
                //                           OPhysicianLname = commonorderinfo.OPhysicianLname,
                //                           OPhysicianFname = commonorderinfo.OPhysicianFname,
                //                           EntererLocation = commonorderinfo.EntererLocation,
                //                           CallBackPhoneNumber = commonorderinfo.CallBackPhoneNumber,
                //                           OrderEffectiveDate = commonorderinfo.OrderEffectiveDate,
                //                           OrderControlCodeReason = commonorderinfo.OrderControlCodeReason,
                //                           EnteringOrganisation = commonorderinfo.EnteringOrganisation,
                //                           EnteringDevice = commonorderinfo.EnteringDevice,
                //                           AltCodingSystem = commonorderinfo.AltCodingSystem,
                //                           AdvBeneficiaryNoticeCode = commonorderinfo.AdvBeneficiaryNoticeCode,
                //                           OrderingFacilityName = commonorderinfo.OrderingFacilityName,
                //                           OrderingFacilityAddress1 = commonorderinfo.OrderingFacilityAddress1,
                //                           OrderingFacilityAddress2 = commonorderinfo.OrderingFacilityAddress2,
                //                           OrderingFacilityCity = commonorderinfo.OrderingFacilityCity,
                //                           OrderingFacilityState = commonorderinfo.OrderingFacilityState,
                //                           OrderingFacilityZip = commonorderinfo.OrderingFacilityZip,
                //                           OrderingFacilityPhone = commonorderinfo.OrderingFacilityPhone,
                //                           OrderingPhysicianAddress1 = commonorderinfo.OrderingPhysicianAddress1,
                //                           OrderingPhysicianAddress2 = commonorderinfo.OrderingPhysicianAddress2,
                //                           OrderingPhysicianCity = commonorderinfo.OrderingPhysicianCity,
                //                           OrderingPhysicianState = commonorderinfo.OrderingPhysicianState,
                //                           OrderingProviderZip = commonorderinfo.OrderingProviderZip,
                //                           OrderStatusModifier = commonorderinfo.OrderStatusModifier,
                //                           AdvBeneficiaryNoticeOverrideReason = commonorderinfo.AdvBeneficiaryNoticeOverrideReason,
                //                           ExpectedAvailabilityDate = commonorderinfo.ExpectedAvailabilityDate,
                //                           ConfidentialityCode = commonorderinfo.ConfidentialityCode,
                //                           OrderType = commonorderinfo.OrderType,
                //                           EntererAuthorizationMode = commonorderinfo.EntererAuthorizationMode,
                //                           POrder_Status = commonorderinfo.POrder_Status,
                //                           POrder_UpdatedOn = commonorderinfo.POrder_UpdatedOn,
                //                           POOutBoundFileStatus = commonorderinfo.POOutBoundFileStatus,
                //                           POOutBoundApproval = commonorderinfo.POOutBoundApproval,
                //                           POOutBoundApprovalBy = commonorderinfo.POOutBoundApprovalBy,
                //                           POOutBoundApprovalOn = commonorderinfo.POOutBoundApprovalOn,
                //                           AlertText = commonorderinfo.AlertText,
                //                           MaxPerdays = commonorderinfo.MaxPerdays,
                //                           OrderStockFlag = commonorderinfo.OrderStockFlag,
                //                           PRNFlag = commonorderinfo.PRNFlag,
                //                           SelfAdministeredFlag = commonorderinfo.SelfAdministeredFlag,
                //                           TreatmentFlag = commonorderinfo.TreatmentFlag,
                //                           Maysubstitute = commonorderinfo.Maysubstitute,
                //                           InsulinComments = commonorderinfo.InsulinComments,
                //                           UpdatedBy = u1.UserName

                //                       }).OrderByDescending(commonorderinfo => commonorderinfo.POrder_UpdatedOn).ToList();

            }
            else if (tableName == "Demographics")
            {
                auditEntity.ColumnNames = typeof(Demographic1Entity).GetProperties()
                            .Select(property => property.Name)
                            .ToList();

                auditEntity.Records = this.dbContext.PrcGetDemographicChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();
            }
            //else if (tableName == "TreatmentInfo")
            //{
            //    auditEntity.ColumnNames = typeof(TreatmentInfo1Entity).GetProperties()
            //                .Select(property => property.Name)
            //                .ToList();

            //    auditEntity.Records = (from treatmentinfo in this.dbContext.TreatmentInfo1


            //                           join u1 in this.dbContext.Users on treatmentinfo.PTreatment_UpdatedBy equals u1.User_Id
            //                           where treatmentinfo.PTreatment_Id == recordId
            //                           select new TreatmentInfo1Entity
            //                           {
            //                              AuditPTreatment_Id=treatmentinfo.AuditPTreatment_Id,
            //                               PTreatment_Id=treatmentinfo.PTreatment_Id,
            //                               POrder_Id=treatmentinfo.POrder_Id,
            //                               ReqGiveCodeIdentifier=treatmentinfo.ReqGiveCodeIdentifier,
            //                               RequestedGiveCode=treatmentinfo.RequestedGiveCode,
            //                               RequestedGiveAmtMin=treatmentinfo.RequestedGiveAmtMin,
            //                               RequestedGiveAmtMax=treatmentinfo.RequestedGiveAmtMax,
            //                               RequestedGiveUnits=treatmentinfo.RequestedGiveUnits,
            //                               RequestedDosageForm=treatmentinfo.RequestedDosageForm,
            //                               ProvidersTreatmentInstructions=treatmentinfo.ProvidersTreatmentInstructions,
            //                               ProvidersAdministrationInstructions=treatmentinfo.ProvidersAdministrationInstructions,
            //                               DeliverToLocation=treatmentinfo.DeliverToLocation,
            //                               AllowSubstitutions=treatmentinfo.AllowSubstitutions,
            //                               RequestedDispenseCode=treatmentinfo.RequestedDispenseCode,
            //                               RequestedDispenseAmount=treatmentinfo.RequestedDispenseAmount,
            //                               RequestedDispenseUnits=treatmentinfo.RequestedDispenseUnits,
            //                               NumberOfRefills=treatmentinfo.NumberOfRefills,
            //                               OrderingProviderDEANumber=treatmentinfo.OrderingProviderDEANumber,
            //                               TreatmentSupplierVerifierID=treatmentinfo.TreatmentSupplierVerifierID,
            //                               NeedsHumanReview=treatmentinfo.NeedsHumanReview,
            //                               RequestedGivePer=treatmentinfo.RequestedGivePer,
            //                               RequestedGiveStrength=treatmentinfo.RequestedGiveStrength,
            //                               RequestedGiveStrengthUnits=treatmentinfo.RequestedGiveStrengthUnits,
            //                               IndicationIdentifier=treatmentinfo.IndicationIdentifier,
            //                               IndicationText=treatmentinfo.IndicationText,
            //                               IndicationCodingSystem=treatmentinfo.IndicationCodingSystem,
            //                               AIndicationIdentifier=treatmentinfo.AIndicationIdentifier,
            //                               AIndicationText=treatmentinfo.AIndicationText,
            //                               AIndicationCodingSystem=treatmentinfo.AIndicationCodingSystem,
            //                               RequestedGiveRateAmount=treatmentinfo.RequestedGiveRateAmount,
            //                               RequestedGiveRateUnits=treatmentinfo.RequestedGiveRateUnits,
            //                               TotalDailyDose=treatmentinfo.TotalDailyDose,
            //                               SupplementaryCode=treatmentinfo.SupplementaryCode,
            //                               RequestedDrugStrengthVol=treatmentinfo.RequestedDrugStrengthVol,
            //                               RequestedDrugStrengthVolUnits=treatmentinfo.RequestedDrugStrengthVolUnits,
            //                               PharmacyOrderType=treatmentinfo.PharmacyOrderType,
            //                               DispensingInterval=treatmentinfo.DispensingInterval,
            //                               PTreatment_Status=treatmentinfo.PTreatment_Status,
            //                               PTreatment_UpdatedOn=treatmentinfo.PTreatment_UpdatedOn,
            //                               PTIOutBoundFileStatus=treatmentinfo.PTIOutBoundFileStatus,
            //                               PTIOutBoundApproval=treatmentinfo.PTIOutBoundApproval,
            //                               PTIOutBoundApprovalBy=treatmentinfo.PTIOutBoundApprovalBy,
            //                               PTIOutBoundApprovalOn=treatmentinfo.PTIOutBoundApprovalOn,
            //                               UpdatedBy = u1.UserName

            //                           }).OrderByDescending(treatmentinfo => treatmentinfo.PTreatment_UpdatedOn).ToList();

            //}
            else if (tableName == "Role")
            {
                //auditEntity.ColumnNames = typeof(Role1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetRoleChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();


            }
            else if (tableName == "RoleConfig")
            {
                //auditEntity.ColumnNames = typeof(RoleConfig1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetRoleConfigChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "UserRoleFacilityConfig")
            {
                //auditEntity.ColumnNames = typeof(UserRoleFacilityConfig1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                //auditEntity.Records = this.dbContext.PrcGetUserRoleConfigChangesDetails(recordId).Select(c => new
                //{
                //    ColumnName = c.ColumnName,
                //    OldValue = c.OldValue,
                //    NewValue = c.NewValue,
                //    UpdatedBy = c.UpdatedBy,
                //    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                //}).OrderByDescending(c => c.UpdatedOn).ToList();

            }
            else if (tableName == "NursingFrequencyConfig")
            {
                //auditEntity.ColumnNames = typeof(NursingFrequencyConfig1Entity).GetProperties()
                //            .Select(property => property.Name)
                //            .ToList();

                auditEntity.Records = this.dbContext.PrcGetFreqMappChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "VisitInfo")
            {


                auditEntity.Records = this.dbContext.PrcGetVisitChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "AllergyInfo")
            {
                auditEntity.Records = this.dbContext.PrcGetAllergyChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();
            }

            else if (tableName == "Diagnosis")
            {
                auditEntity.Records = this.dbContext.PrcGetDiagnosisChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();
            }
            else if (tableName == "AllergyInfoMaster")
            {


                auditEntity.Records = this.dbContext.PrcGetAllergyMasterChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "ComapnyBedConfig")
            {


                auditEntity.Records = this.dbContext.PrcGetCompanyBedConfigChangesdetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "ResidentColorConfig")
            {


                auditEntity.Records = this.dbContext.PrcGetPatientTypeChangesdetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "ICD")
            {


                auditEntity.Records = this.dbContext.PrcGetICD10ChangesDetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "CompanyConfig")
            {
                auditEntity.Records = this.dbContext.PrcGetCompanyConfigChanges(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "PhysicianDetails")
            {
                auditEntity.Records = this.dbContext.PrcGetPhysicianChangesdetails(recordId).OrderByDescending(r => r.UpdatedOn).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();

            }
            else if (tableName == "CompanyToBedMap")
            {
                //auditEntity.Records = this.dbContext.PrcGetPhysicianChangesdetails(recordId).Select(c => new
                //{
                //    ColumnName = c.ColumnName,
                //    OldValue = c.OldValue,
                //    NewValue = c.NewValue,
                //    UpdatedBy = c.UpdatedBy,
                //    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                //}).OrderByDescending(c => c.UpdatedOn).ToList();
                return auditEntity;

            }

            //else if (tableName == "DrugAdminister")
            //{


            //    auditEntity.Records = this.dbContext.PrcGetDrugAdministerDetails(recordId).Select(c => new
            //    {
            //        ColumnName = c.ColumnName,
            //        OldValue = c.OldValue,
            //        NewValue = c.NewValue,
            //        UpdatedBy = c.UpdatedBy,
            //        UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
            //    }).OrderByDescending(c => c.UpdatedOn).ToList();

            //}
            else if(tableName=="AllResidentChanges")
            {
                var visitIds = this.dbContext.VisitInfoes.Where(v => v.Patient_Id == recordId).Select(v => v.PVisit_Id).ToList();
                var allergiesIds = this.dbContext.AllergyInfoes.Where(a => a.Patient_Id == recordId).Select(a => a.PAllergy_Id).ToList();
                var diagnosisIds = this.dbContext.DiagnosisInfoes.Where(d => d.Patient_Id == recordId).Select(d => d.PDiagnosis_Id).ToList();
                var demographics = this.dbContext.PrcGetDemographicChangesDetails(recordId).Select(c => new
                {
                    ColumnName = c.ColumnName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                }).ToList();
                foreach (var visit in visitIds)
                {
                    int visitId = (int)visit;
                    var visitInfo = this.dbContext.PrcGetVisitChangesDetails(visitId).Select(c => new
                    {
                        ColumnName = c.ColumnName,
                        OldValue = c.OldValue,
                        NewValue = c.NewValue,
                        UpdatedBy = c.UpdatedBy,
                        UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                    }).ToList();
                    demographics.AddRange(visitInfo);
                }
                foreach (var allergy in allergiesIds)
                {
                    int allergyId = (int)allergy;
                    var allergyInfo = this.dbContext.PrcGetAllergyChangesDetails(allergyId).Select(c => new
                    {
                        ColumnName = c.ColumnName,
                        OldValue = c.OldValue,
                        NewValue = c.NewValue,
                        UpdatedBy = c.UpdatedBy,
                        UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                    }).ToList();
                    demographics.AddRange(allergyInfo);
                }
                foreach (var diagnosis in diagnosisIds)
                {
                    int diagnosisId = (int)diagnosis;
                    var diagnosisInfo = this.dbContext.PrcGetDiagnosisChangesDetails(diagnosisId).Select(c => new
                    {
                        ColumnName = c.ColumnName,
                        OldValue = c.OldValue,
                        NewValue = c.NewValue,
                        UpdatedBy = c.UpdatedBy,
                        UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
                    }).ToList();
                    demographics.AddRange(diagnosisInfo);
                }
                auditEntity.Records = demographics.OrderByDescending(c => c.UpdatedOn).Distinct().ToList();
                return auditEntity;
            }
            return auditEntity;
        }
        public AuditCustomEntity GetUserRoleConfigAudit(int userId, int roleId, int facilityId)
        {
            AuditCustomEntity auditEntity = new AuditCustomEntity();
            auditEntity.Records = this.dbContext.PrcGetUserRoleConfigChangesDetails(userId, roleId, facilityId).OrderByDescending(r => r.UpdatedOn).Select(c => new
            {
                ColumnName = c.ColumnName,
                OldValue = c.OldValue,
                NewValue = c.NewValue,
                UpdatedBy = c.UpdatedBy,
                UpdatedOn = ((DateTime)c.UpdatedOn).ToString("MM/dd/yyyy hh:mm:ss tt")
            }).Distinct().ToList();
            return auditEntity;
        }
    }

}
