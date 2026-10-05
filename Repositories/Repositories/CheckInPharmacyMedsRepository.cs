using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LTCPro.DAL;
using LTCPro.Entities;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

namespace LTCPro.Repositories
{
    public class CheckInPharmacyMedsRepository : ICheckInPharmacyMedsRepository
    {
        private readonly SQLHelper dbHelper;
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        string storedprocedure = "";
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly ICommonRepository _commonRepository;
        public CheckInPharmacyMedsRepository(IAutoMapper autoMapper, IUserActivityRepository userActivityRepository, IDbContextEmar dbContext, CommonRepository commonRepository, SQLHelper _dbContext)
        {
            this.dbHelper = _dbContext;
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
            this._commonRepository = commonRepository;
        }
        public CheckInMedsGridEntity GetCheckInMedsList(int nsId, string barcode)
        {
            var result = new CheckInMedsGridEntity();
            var records = new List<CheckInMedsEntity>();
            if (barcode != "" && barcode != null)
            {
                //It returns only one record
                var completeData = (from pc in this.dbContext.CommonOrderInfoes
                                    join pq in this.dbContext.QuantityDetails on pc.POrder_Id equals pq.POrder_Id
                                    join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
                                    join pb in this.dbContext.BarcodeDetails on pc.POrder_Id equals pb.POrder_Id
                                    join po in this.dbContext.OrderStocks on pq.PQuantity_Id equals po.PQuantity_Id into pos
                                    join pd in this.dbContext.VisitInfoes on pc.Patient_Id equals pd.Patient_Id into pv
                                    from os in pos.DefaultIfEmpty()
                                    where pc.OrderStockFlag == true && pc.OrderTypeID == 1
                                    && pb.PBarcode_Status == 1 && pb.BarcodeDetail1 == barcode
                                    select new
                                    {
                                        commonOrder = pc,
                                        encodedOrder = pe,
                                        orderStock = os,
                                        quantitydetail = pq,
                                        visit = pv.Count() > 1 ? (pv.Where(v => v.DischargeDate == null).Count() == 0 ? pv.OrderByDescending(v => v.DischargeDate).FirstOrDefault() : pv.Where(v => v.DischargeDate == null).FirstOrDefault()) : pv.FirstOrDefault(),
                                    }).AsEnumerable();


                completeData = completeData.Where(v => v.visit.NursingStationId == nsId).AsEnumerable();

                var nsRecord = completeData.Where(v => v.visit.NursingStationId == nsId).FirstOrDefault();
                string nsName = string.Empty;
                if (nsRecord != null)
                    result.ResidentNSFlag = 1;
                else
                {
                    result.ResidentNSFlag = 0;
                    var resNSId = completeData.Select(v => v.visit.NursingStationId).FirstOrDefault();
                    nsName = this.dbContext.NursingStations.Where(ns => ns.NurseStation_Id == resNSId).Select(ns => ns.NurseStation_Name).FirstOrDefault();
                }
                result.ResidentName = (from v in completeData
                                       join pd in this.dbContext.Demographics on v.visit.Patient_Id equals pd.Patient_Id


                                       select pd.PatientLastName + ", " + pd.PatientFirstName + " " + (pd.PatientMiddleInitial != null ? pd.PatientMiddleInitial : "") + " " + (pd.DOB != null ? "(" + Convert.ToDateTime(pd.DOB).ToString("MM/dd/yyyy") + ")" : "")


                                       ).FirstOrDefault();


                string[] controlIds = { "I", "II", "III", "IV", "V" };
                result.Data = completeData.Select(c => new CheckInMedsEntity()
                {
                    ResidentName = result.ResidentName,
                    ResidentNSFlag = result.ResidentNSFlag,
                    POrder_Id = c.commonOrder.POrder_Id,
                    PQuantity_Id = c.quantitydetail.PQuantity_Id,
                    DrugName = c.encodedOrder.GiveCodeText,
                    OnHand = "",//c.orderStock != null ? (c.orderStock.Remaining == null ? "0" : c.orderStock.Remaining) : "0",
                    OrderStatus = (int)c.quantitydetail.OrderStatus,
                    NSName = nsName,
                    ControlledMed = controlIds.Contains(c.encodedOrder.ControlledSubstanceSchedule) == true ? 1 : 0,
                    ResidentStatus = c.visit.PVisit_Status,
                    barcodeCheck = 1,
                    LotNumber = "", //c.orderStock != null?c.orderStock.LotNumber:"",
                    ExpirationDate = "",//c.orderStock != null ? c.orderStock.ExpirationDate == null ? "" : c.orderStock.ExpirationDate.ToString() : "",
                    Directions = c.quantitydetail.TextInstruction,
                    nursetationID = c.visit.NursingStationId ?? 0,
                }).OrderBy(c => c.DrugName).ToList();

                // this.dbContext.ControlSubstanceCounts.Where(cs=> records. cs.Porder_Id)
                if (result.Data.Count > 0)
                {
                    int patientId = completeData.Select(c => c.visit.Patient_Id).FirstOrDefault();
                    List<int> qtyIds = result.Data.Select(c => c.PQuantity_Id).ToList();
                    var completeDataByPatientId = (from pc in this.dbContext.CommonOrderInfoes
                                                   join pq in this.dbContext.QuantityDetails on pc.POrder_Id equals pq.POrder_Id
                                                   join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
                                                   join pb in this.dbContext.BarcodeDetails on pc.POrder_Id equals pb.POrder_Id
                                                   join po in this.dbContext.OrderStocks on pq.PQuantity_Id equals po.PQuantity_Id into pos
                                                   join pd in this.dbContext.VisitInfoes on pc.Patient_Id equals pd.Patient_Id into pv
                                                   from os in pos.DefaultIfEmpty()
                                                   where pc.OrderStockFlag == true && pc.OrderTypeID == 1
                                                   && pb.PBarcode_Status == 1 && pc.Patient_Id == patientId && !qtyIds.Contains(pq.PQuantity_Id)
                                                   select new
                                                   {
                                                       commonOrder = pc,
                                                       encodedOrder = pe,
                                                       orderStock = os,
                                                       quantitydetail = pq,
                                                       visit = pv.Count() > 1 ? (pv.Where(v => v.DischargeDate == null).Count() == 0 ? pv.OrderByDescending(v => v.DischargeDate).FirstOrDefault() : pv.Where(v => v.DischargeDate == null).FirstOrDefault()) : pv.FirstOrDefault(),
                                                   }).Distinct().AsEnumerable();

                    var allOrdersList = completeDataByPatientId.Select(c => new CheckInMedsEntity()
                    {
                        ResidentName = result.ResidentName,
                        ResidentNSFlag = result.ResidentNSFlag,
                        POrder_Id = c.commonOrder.POrder_Id,
                        PQuantity_Id = c.quantitydetail.PQuantity_Id,
                        DrugName = c.encodedOrder.GiveCodeText,
                        OnHand = "",//c.orderStock != null ? (c.orderStock.Remaining == null ? "0" : c.orderStock.Remaining) : "0",
                        OrderStatus = (int)c.quantitydetail.OrderStatus,
                        NSName = nsName,
                        ControlledMed = controlIds.Contains(c.encodedOrder.ControlledSubstanceSchedule) == true ? 1 : 0,
                        ResidentStatus = c.visit.PVisit_Status,
                        barcodeCheck = 0,
                        LotNumber = "",//c.orderStock != null ? c.orderStock.LotNumber : "",
                        ExpirationDate = "",// c.orderStock != null che.orderStock.ExpirationDate==null?"": c.orderStock.ExpirationDate.ToString() : "",
                        Directions = c.quantitydetail.TextInstruction,
                        nursetationID = c.visit.NursingStationId ?? 0,
                        FacilityID = c.visit.FacilityId ?? 0
                    }).OrderBy(c => c.DrugName).Distinct().ToList();
                    result.Data.AddRange(allOrdersList);
                }

            }
            result.Data = result.Data.Where(v => v.nursetationID == nsId).ToList();
            return result;

            //else
            //{
            //    //no need of this
            //    count = (from pc in this.dbContext.CommonOrderInfoes
            //             join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
            //             join po in this.dbContext.OrderStocks on pc.POrder_Id equals po.Porder_Id into pos
            //             from os in pos.DefaultIfEmpty()
            //             where pc.OrderStockFlag == true && pc.OrderTypeID == 1 && pc.POrder_Status == 1 && pc.Patient_Id == residentId
            //             select new CheckInMedsEntity()
            //             {
            //                 POrder_Id = pc.POrder_Id,
            //                 DrugName = pe.GiveCodeText,
            //                 OnHand = os.Remaining == null ? "0" : os.Remaining
            //             }).Count();

            //    records = (from pc in this.dbContext.CommonOrderInfoes
            //               join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
            //               join po in this.dbContext.OrderStocks on pc.POrder_Id equals po.Porder_Id into pos
            //               from os in pos.DefaultIfEmpty()
            //               where pc.OrderStockFlag == true && pc.OrderTypeID == 1 && pc.POrder_Status == 1 && pc.Patient_Id == residentId
            //               select new CheckInMedsEntity()
            //               {
            //                   POrder_Id = pc.POrder_Id,
            //                   DrugName = pe.GiveCodeText,
            //                   OnHand = os.Remaining == null ? "0" : os.Remaining
            //               }).OrderBy(c => c.DrugName).Skip(skipRows).Take(pageSize).ToList();
            //}
            //return new CheckInMedsGridEntity()
            //{
            //    TotalRecords = count,
            //    Data = records
            //};

        }
        //public CheckInMedsGridEntity GetCheckInMedsList(int nsId, string barcode)
        //{
        //    var result = new CheckInMedsGridEntity();
        //    var records = new List<CheckInMedsEntity>();
        //    if (barcode != "" && barcode != null)
        //    {
        //        //It returns only one record
        //        var completeData = (from pc in this.dbContext.CommonOrderInfoes
        //                            join pq in this.dbContext.QuantityDetails on pc.POrder_Id equals pq.POrder_Id
        //                            join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
        //                            join pb in this.dbContext.BarcodeDetails on pc.POrder_Id equals pb.POrder_Id
        //                            join po in this.dbContext.OrderStocks on pq.PQuantity_Id equals po.PQuantity_Id into pos
        //                            join pd in this.dbContext.VisitInfoes on pc.Patient_Id equals pd.Patient_Id into pv
        //                            from os in pos.DefaultIfEmpty()
        //                            where pc.OrderStockFlag == true && pc.OrderTypeID == 1
        //                            && pb.PBarcode_Status == 1 && pb.BarcodeDetail1 == barcode
        //                            select new
        //                            {
        //                                commonOrder = pc,
        //                                encodedOrder = pe,
        //                                orderStock = os,
        //                                quantitydetail = pq,
        //                                visit = pv.Count() > 1 ? (pv.Where(v => v.DischargeDate == null).Count() == 0 ? pv.OrderByDescending(v => v.DischargeDate).FirstOrDefault() : pv.Where(v => v.DischargeDate == null).FirstOrDefault()) : pv.FirstOrDefault(),
        //                            }).AsEnumerable();

        //        var nsRecord = completeData.Where(v => v.visit.NursingStationId == nsId).FirstOrDefault();
        //        string nsName = string.Empty;
        //        if (nsRecord != null)
        //            result.ResidentNSFlag = 1;
        //        else
        //        {
        //            result.ResidentNSFlag = 0;
        //            var resNSId = completeData.Select(v => v.visit.NursingStationId).FirstOrDefault();
        //            nsName = this.dbContext.NursingStations.Where(ns => ns.NurseStation_Id == resNSId).Select(ns => ns.NurseStation_Name).FirstOrDefault();
        //        }
        //        result.ResidentName = (from v in completeData
        //                               join pd in this.dbContext.Demographics on v.visit.Patient_Id equals pd.Patient_Id
        //                               select pd.PatientLastName + ", " + pd.PatientFirstName + " " + (pd.PatientMiddleInitial != null ? pd.PatientMiddleInitial : "") + " " + (pd.DOB != null ? "(" + Convert.ToDateTime(pd.DOB).ToString("MM/dd/yyyy") + ")" : "")).FirstOrDefault();
        //        string[] controlIds = { "I", "II", "III", "IV", "V" };
        //        result.Data = completeData.Select(c => new CheckInMedsEntity()
        //        {
        //            ResidentName = result.ResidentName,
        //            ResidentNSFlag = result.ResidentNSFlag,
        //            POrder_Id = c.commonOrder.POrder_Id,
        //            PQuantity_Id = c.quantitydetail.PQuantity_Id,
        //            DrugName = c.encodedOrder.GiveCodeText,
        //            OnHand = "",//c.orderStock != null ? (c.orderStock.Remaining == null ? "0" : c.orderStock.Remaining) : "0",
        //            OrderStatus = (int)c.quantitydetail.OrderStatus,
        //            NSName = nsName,
        //            ControlledMed = controlIds.Contains(c.encodedOrder.ControlledSubstanceSchedule) == true ? 1 : 0,
        //            ResidentStatus = c.visit.PVisit_Status,
        //            barcodeCheck = 1,
        //            LotNumber = "", //c.orderStock != null?c.orderStock.LotNumber:"",
        //            ExpirationDate = "",//c.orderStock != null ? c.orderStock.ExpirationDate == null ? "" : c.orderStock.ExpirationDate.ToString() : "",
        //            Directions = c.quantitydetail.TextInstruction,
        //        }).OrderBy(c => c.DrugName).ToList();

        //        // this.dbContext.ControlSubstanceCounts.Where(cs=> records. cs.Porder_Id)
        //        if (result.Data.Count > 0)
        //        {
        //            int patientId = completeData.Select(c => c.visit.Patient_Id).FirstOrDefault();
        //            List<int> qtyIds = result.Data.Select(c => c.PQuantity_Id).ToList();
        //            var completeDataByPatientId = (from pc in this.dbContext.CommonOrderInfoes
        //                                           join pq in this.dbContext.QuantityDetails on pc.POrder_Id equals pq.POrder_Id
        //                                           join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
        //                                           join pb in this.dbContext.BarcodeDetails on pc.POrder_Id equals pb.POrder_Id
        //                                           join po in this.dbContext.OrderStocks on pq.PQuantity_Id equals po.PQuantity_Id into pos
        //                                           join pd in this.dbContext.VisitInfoes on pc.Patient_Id equals pd.Patient_Id into pv
        //                                           from os in pos.DefaultIfEmpty()
        //                                           where pc.OrderStockFlag == true && pc.OrderTypeID == 1
        //                                           && pb.PBarcode_Status == 1 && pc.Patient_Id == patientId && !qtyIds.Contains(pq.PQuantity_Id)
        //                                           select new
        //                                           {
        //                                               commonOrder = pc,
        //                                               encodedOrder = pe,
        //                                               orderStock = os,
        //                                               quantitydetail = pq,
        //                                               visit = pv.Count() > 1 ? (pv.Where(v => v.DischargeDate == null).Count() == 0 ? pv.OrderByDescending(v => v.DischargeDate).FirstOrDefault() : pv.Where(v => v.DischargeDate == null).FirstOrDefault()) : pv.FirstOrDefault(),
        //                                           }).Distinct().AsEnumerable();

        //            var allOrdersList = completeDataByPatientId.Select(c => new CheckInMedsEntity()
        //            {
        //                ResidentName = result.ResidentName,
        //                ResidentNSFlag = result.ResidentNSFlag,
        //                POrder_Id = c.commonOrder.POrder_Id,
        //                PQuantity_Id = c.quantitydetail.PQuantity_Id,
        //                DrugName = c.encodedOrder.GiveCodeText,
        //                OnHand = "",//c.orderStock != null ? (c.orderStock.Remaining == null ? "0" : c.orderStock.Remaining) : "0",
        //                OrderStatus = (int)c.quantitydetail.OrderStatus,
        //                NSName = nsName,
        //                ControlledMed = controlIds.Contains(c.encodedOrder.ControlledSubstanceSchedule) == true ? 1 : 0,
        //                ResidentStatus = c.visit.PVisit_Status,
        //                barcodeCheck = 0,
        //                LotNumber = "",//c.orderStock != null ? c.orderStock.LotNumber : "",
        //                ExpirationDate = "",// c.orderStock != null che.orderStock.ExpirationDate==null?"": c.orderStock.ExpirationDate.ToString() : "",
        //                Directions = c.quantitydetail.TextInstruction,
        //            }).OrderBy(c => c.DrugName).Distinct().ToList();
        //            result.Data.AddRange(allOrdersList);
        //        }

        //    }

        //    return result;

        //else
        //{
        //    //no need of this
        //    count = (from pc in this.dbContext.CommonOrderInfoes
        //             join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
        //             join po in this.dbContext.OrderStocks on pc.POrder_Id equals po.Porder_Id into pos
        //             from os in pos.DefaultIfEmpty()
        //             where pc.OrderStockFlag == true && pc.OrderTypeID == 1 && pc.POrder_Status == 1 && pc.Patient_Id == residentId
        //             select new CheckInMedsEntity()
        //             {
        //                 POrder_Id = pc.POrder_Id,
        //                 DrugName = pe.GiveCodeText,
        //                 OnHand = os.Remaining == null ? "0" : os.Remaining
        //             }).Count();

        //    records = (from pc in this.dbContext.CommonOrderInfoes
        //               join pe in this.dbContext.EncodedOrderDetails on pc.POrder_Id equals pe.POrder_Id
        //               join po in this.dbContext.OrderStocks on pc.POrder_Id equals po.Porder_Id into pos
        //               from os in pos.DefaultIfEmpty()
        //               where pc.OrderStockFlag == true && pc.OrderTypeID == 1 && pc.POrder_Status == 1 && pc.Patient_Id == residentId
        //               select new CheckInMedsEntity()
        //               {
        //                   POrder_Id = pc.POrder_Id,
        //                   DrugName = pe.GiveCodeText,
        //                   OnHand = os.Remaining == null ? "0" : os.Remaining
        //               }).OrderBy(c => c.DrugName).Skip(skipRows).Take(pageSize).ToList();
        //}
        //return new CheckInMedsGridEntity()
        //{
        //    TotalRecords = count,
        //    Data = records
        //};

    //}
        public Int64 CheckBarcode(string barcode, int porderId)
        {
            if (barcode != null && barcode != "")
            {
                var barcodeRecord = this.dbContext.BarcodeDetails.Where(o => o.BarcodeDetail1 == barcode).ToList();
                if (barcodeRecord.Count > 0)
                {
                    List<int> orderIds = barcodeRecord.Select(b => (int)b.POrder_Id).ToList();
                    if (orderIds.Contains(porderId) == true)
                    {
                        return porderId;
                    }
                    else
                    {
                        return barcodeRecord[0].POrder_Id.Value;
                    }
                }
                else
                    return 0;
            }
            return 0;
        }
        public List<EkitCustomEntity> GetEkitInMedsDetails(int FacilityId, int NsId)
        {
            int userId = 0;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = userId,
                    Facility_Id = (int)FacilityId,
                    NurseStation_Id = NsId.ToString(),
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            var records = this.dbContext.Ekits.Where(p => p.Ekit_Status == 1 && ((p.Facility_Id == FacilityId && p.NurseStation_Id == null) || p.NurseStation_Id == NsId)).Distinct().OrderBy(item => item.DrugName).ToList();
            return this.autoMapper.Map<List<Ekit>, List<EkitCustomEntity>>(records);

        }
        public int UpdateEkitDetailsInfo(EKitMedsEntity obj)
        {
            obj.Ekit_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            Ekit records = this.dbContext.Ekits.Find(obj.Ekit_Id);
            records.Facility_Id = obj.Facility_Id;
            records.NurseStation_Id = obj.NurseStation_Id;
            records.DrugName = obj.DrugName;
            records.InHand = obj.InHand;
            records.LotNumber = obj.LotNumber;
            records.CheckInFlag = 1;
            records.ExpiryDate = obj.ExpiryDate;
            records.Ekit_Status = obj.Ekit_Status;
            records.Ekit_CreatedBy = obj.Ekit_CreatedBy;
            records.Ekit_CreatedOn = obj.Ekit_CreatedOn;
            this.dbContext.SaveChanges();
            if (obj.BarCodeDetails != "")
            {
                BarcodeDetail bd = new BarcodeDetail();
                bd.Ekit_Id = obj.Ekit_Id;
                bd.BarcodeDetail1 = obj.BarCodeDetails;
                bd.PBarcode_Status = Convert.ToInt32(obj.Ekit_Status);
                bd.PBarcode_CreatedBy = obj.Ekit_CreatedBy;
                bd.PBarcode_CreatedDate = Convert.ToDateTime(obj.Ekit_CreatedOn);
                this.dbContext.BarcodeDetails.Add(bd);
                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public int InsertUpdatePharmacyInfo(PharmacyInfoEntity obj)
        {

            if (obj.Pharmacy_Id == 0)
            {
                storedprocedure = "[admin].[PrcInsertPharmacyInfo]";
                var parameter = new SqlParameter[23];
                parameter[0] = new SqlParameter("@Facility_Id", obj.Facility_Id);
                parameter[1] = new SqlParameter("@NurseStation_Id", obj.NurseStation_Id);
                parameter[2] = new SqlParameter("@Pharmacy_Name", obj.Pharmacy_Name);
                parameter[3] = new SqlParameter("@Pharmacy_Address1", obj.Pharmacy_Address1);
                parameter[4] = new SqlParameter("@Pharmacy_Address2", "Address2");
                parameter[5] = new SqlParameter("@Pharmacy_City", obj.Pharmacy_City == null ?"" : obj.Pharmacy_City);
                parameter[6] = new SqlParameter("@Pharmacy_State", obj.Pharmacy_State == null ? "" : obj.Pharmacy_State);
                parameter[7] = new SqlParameter("@Pharmacy_Zip", obj.Pharmacy_Zip == null ? "" : obj.Pharmacy_Zip);
                parameter[8] = new SqlParameter("@Pharmacy_CountryId", obj.Pharmacy_CountryId);
                parameter[9] = new SqlParameter("@Pharmacy_Retail", obj.Pharmacy_Retail);
                parameter[10] = new SqlParameter("@Pharmacy_Mail_Order", obj.Pharmacy_Mail_Order);
                parameter[11] = new SqlParameter("@Pharmacy_Speciality", obj.Pharmacy_Speciality);
                parameter[12] = new SqlParameter("@Pharmacy_LTC", obj.Pharmacy_LTC);
                parameter[13] = new SqlParameter("@Pharmacy_IHD", obj.Pharmacy_IHD);
                parameter[14] = new SqlParameter("@Pharmacy_24_Hours", obj.Pharmacy_24_Hours);
                parameter[15] = new SqlParameter("@Pharmacy_EPCS_Enabled", obj.Pharmacy_EPCS_Enabled);
                parameter[16] = new SqlParameter("@Pharmacy_Phone", obj.Pharmacy_Phone == null ? "" : obj.Pharmacy_Phone);
                parameter[17] = new SqlParameter("@Pharmacy_Fax", obj.Pharmacy_Fax == null ? "" : obj.Pharmacy_Fax);
                parameter[18] = new SqlParameter("@Pharmacy_Status", obj.Pharmacy_Status);
                parameter[19] = new SqlParameter("@Pharmacy_CreatedBy", obj.Pharmacy_CreatedBy);
                parameter[20] = new SqlParameter("@Pharmacy_Id", obj.Pharmacy_Id);
                parameter[21] = new SqlParameter("@NCPDP", obj.NCPDP == null ? "" : obj.NCPDP);
                parameter[22] = new SqlParameter("@NPI", obj.NPI == null ? "" : obj.NPI);


                DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);
                return 1;
            }
            else
            {
                storedprocedure = "[admin].[PrcUpdatePharmacyInfo]";
                var parameter = new SqlParameter[23];
                parameter[0] = new SqlParameter("@Facility_Id", obj.Facility_Id);
                parameter[1] = new SqlParameter("@NurseStation_Id", obj.NurseStation_Id);
                parameter[2] = new SqlParameter("@Pharmacy_Name", obj.Pharmacy_Name);
                parameter[3] = new SqlParameter("@Pharmacy_Address1", obj.Pharmacy_Address1);
                parameter[4] = new SqlParameter("@Pharmacy_Address2", obj.Pharmacy_Address2);


                parameter[5] = new SqlParameter("@Pharmacy_City", obj.Pharmacy_City == null ? "" : obj.Pharmacy_City);
                parameter[6] = new SqlParameter("@Pharmacy_State", obj.Pharmacy_State == null ? "" : obj.Pharmacy_State);
                parameter[7] = new SqlParameter("@Pharmacy_Zip", obj.Pharmacy_Zip == null ? "" : obj.Pharmacy_Zip);



                parameter[8] = new SqlParameter("@Pharmacy_CountryId", obj.Pharmacy_CountryId);
                parameter[9] = new SqlParameter("@Pharmacy_Retail", obj.Pharmacy_Retail);
                parameter[10] = new SqlParameter("@Pharmacy_Mail_Order", obj.Pharmacy_Mail_Order);
                parameter[11] = new SqlParameter("@Pharmacy_Speciality", obj.Pharmacy_Speciality);
                parameter[12] = new SqlParameter("@Pharmacy_LTC", obj.Pharmacy_LTC);
                parameter[13] = new SqlParameter("@Pharmacy_IHD", obj.Pharmacy_IHD);
                parameter[14] = new SqlParameter("@Pharmacy_24_Hours", obj.Pharmacy_24_Hours);
                parameter[15] = new SqlParameter("@Pharmacy_EPCS_Enabled", obj.Pharmacy_EPCS_Enabled);


                parameter[16] = new SqlParameter("@Pharmacy_Phone", obj.Pharmacy_Phone == null ? "" : obj.Pharmacy_Phone);
                parameter[17] = new SqlParameter("@Pharmacy_Fax", obj.Pharmacy_Fax == null ? "" : obj.Pharmacy_Fax);

                parameter[18] = new SqlParameter("@Pharmacy_Status", obj.Pharmacy_Status);
                parameter[19] = new SqlParameter("@Pharmacy_CreatedBy", obj.Pharmacy_CreatedBy);
                parameter[20] = new SqlParameter("@Pharmacy_Id", obj.Pharmacy_Id);
                parameter[21] = new SqlParameter("@NCPDP", obj.NCPDP == null ? "" : obj.NCPDP);
                parameter[22] = new SqlParameter("@NPI", obj.NPI == null ? "" : obj.NPI);

                DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);
                return 1;
            }
        }

        public int updatePharmacyFav(Int64 PId, int Fav, int UserId)
        {
            storedprocedure = "[Admin].[PrcInsertUpdateTblPhyFav]";
            var parameter = new SqlParameter[4];

            parameter[0] = new SqlParameter("@Pharmacy_Fav", Fav);
            parameter[1] = new SqlParameter("@EId", UserId);
            parameter[2] = new SqlParameter("@Pharmacy_id", PId);
            parameter[3] = new SqlParameter("@CreatedBy", UserId);
        


            DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);

            return 1;
        }
        public int updatePharmacyStatus(string PIds)
        {
            storedprocedure = "[Admin].[pharmacyinfoupdate]";
            var parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@Pharmacy_Id", PIds);


            DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);

            return 1;
        }

        public IList GetGetPharmacyData(int UserId)
        {
            storedprocedure = "[Admin].[PrcGetPharmacyData]";
            var parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@UserId", UserId);

            DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);

            var records = (from d in dt.AsEnumerable()
                           select new
                           {
                               Facility_Id = Convert.ToInt32(d["Facility_Id"]),
                               Facility_Name = d["Facility_Name"].ToString(),
                               NurseStation_Name = d["NurseStation_Name"].ToString(),
                               PharmacyName = d["PharmacyName"].ToString(),
                               Pharmacy_City = d["Pharmacy_City"].ToString(),
                               Pharmacy_State = (d["Pharmacy_State"]).ToString(),
                               PhysicianCountry = (d["PhysicianCountry"]).ToString(),
                               Pharmacy_Zip = (d["Pharmacy_Zip"]).ToString(),
                               Pharmacy_Status = (d["Pharmacy_Status"]).ToString(),

                               Pharmacy_Id = (d["Pharmacy_Id"]).ToString(),
                               Pharamcy_Type = (d["Pharamcy_Type"]).ToString(),
                               Pharmacy_Options = (d["Pharmacy_Options"]).ToString(),

                               Pharmacy_Fav = (d["Pharmacy_Fav"]).ToString(),
                               Pharmacy_Address1 = (d["Pharmacy_Address1"]).ToString(),

                               NCPDP = d["NCPDP"].ToString(),
                               NPI = d["NPI"].ToString(),
                               Pharmacy_Phone = d["Pharmacy_Phone"].ToString(),
                               Pharmacy_Details = d["PharmacyName"].ToString() + "," + (d["Pharmacy_Address1"]).ToString(),


                           }).ToList();

            return records;

            // return records.OrderBy(item => item.DisplayName).ThenBy(item => item.Company_Facility_Nursestation).ToList();

        }
        public IList GetPharmacyInfo_ById(int Pharmacy_Id)
        {
            storedprocedure = "[admin].[PrcGetPharmacyInfo_ById]";
            var parameter = new SqlParameter[1];
            parameter[0] = new SqlParameter("@Pharmacy_Id", Pharmacy_Id);

            DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);

            var records = (from d in dt.AsEnumerable()
                           select new
                           {


                               Pharmacy_Id = d["Pharmacy_Id"].ToString(),
                               Facility_Name = d["Facility_Name"].ToString(),

                               NurseStation_Name = d["NurseStation_Name"].ToString(),
                               Pharmacy_Name = d["Pharmacy_Name"].ToString(),
                               Pharmacy_Address1 = d["Pharmacy_Address1"].ToString(),
                               Pharmacy_Address2 = d["Pharmacy_Address2"].ToString(),
                               Pharmacy_City = d["Pharmacy_City"].ToString(),
                               Pharmacy_State = d["Pharmacy_State"].ToString(),
                               Pharmacy_CountryId = d["Pharmacy_CountryId"].ToString(),
                               Pharmacy_Retail = d["Pharmacy_Retail"].ToString(),
                               Pharmacy_Mail_Order = d["Pharmacy_Mail_Order"].ToString(),
                               Pharmacy_Speciality = d["Pharmacy_Speciality"].ToString(),
                               Pharmacy_LTC = d["Pharmacy_LTC"].ToString(),
                               Pharmacy_IHD = d["Pharmacy_IHD"].ToString(),
                               Pharmacy_24_Hours = d["Pharmacy_24_Hours"].ToString(),
                               Pharmacy_EPCS_Enabled = d["Pharmacy_EPCS_Enabled"].ToString(),
                               Pharmacy_Phone = d["Pharmacy_Phone"].ToString(),
                               Pharmacy_Fax = d["Pharmacy_Fax"].ToString(),
                               Pharmacy_Status = d["Pharmacy_Status"].ToString(),
                               Pharmacy_Zip = d["Pharmacy_Zip"].ToString(),
                                NCPDP = d["NCPDP"].ToString(),
                                 NPI = d["NPI"].ToString()
                           }).ToList();

            return records;

            // return records.OrderBy(item => item.DisplayName).ThenBy(item => item.Company_Facility_Nursestation).ToList();

        }


        public int CheckInSelectedMeds(List<CheckInSelectedMedsEntity> entity)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                //            select pc.POrder_Id,
                //pe.GiveCodeText, pe.GiveDosageForm,
                //po.Inhand,pb.BarcodeDetail from Patient.CommonOrderInfo pc
                //join Patient.EncodedOrderDetails pe on pc.POrder_Id = pe.POrder_Id
                //left join Patient.BarcodeDetails pb on pc.POrder_Id = pb.POrder_Id
                //left join Patient.OrderStock po on pc.POrder_Id = po.Porder_Id
                //where pc.OrderStockFlag = 1 and pc.OrderTypeID = 1 and pc.Patient_Id = 12985
                if (entity.Count() > 0)
                {
                    foreach (var item in entity)
                    {
                        item.CheckInDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                        if (item.OnHand != null || item.LotNumber != null || item.ExpirationDate != null)
                        {
                            var record = this.dbContext.OrderStocks.Where(o => o.PQuantity_Id == item.PQuantity_Id).FirstOrDefault();
                            if (record != null)
                            {
                                //record.Inhand = item.OnHand;
                                //if (item.OnHand != null)
                                //{

                                OrderStockTran obj = new OrderStockTran();
                                obj.OrderStock_Id = record.OrderStock_Id;
                                obj.Inhand = item.OnHand;
                                obj.LotNumber = item.LotNumber;
                                obj.ExpirationDate = item.ExpirationDate;
                                obj.TransOS_CreatedBy = userId;
                                obj.TransOS_Status = 1;
                                obj.TransOS_CreatedDate = item.CheckInDate;
                                this.dbContext.OrderStockTrans.Add(obj);
                                this.dbContext.SaveChanges();

                                //}
                                record.Remaining = item.OnHand != null ? (record.Remaining == "" ? 0 : Convert.ToDecimal(record.Remaining) + Convert.ToDecimal(item.OnHand)).ToString() : record.Remaining;//record.Remaining != null && record.Remaining != "0" ? (Convert.ToInt32(record.Remaining) + Convert.ToInt32(item.OnHand)).ToString() : item.OnHand;
                                record.LotNumber = item.LotNumber != null ? item.LotNumber : record.LotNumber;
                                record.ExpirationDate = item.ExpirationDate;
                                record.OrderStock_CreatedBy = userId;
                                record.OrderStock_CreatedDate = item.CheckInDate;
                                this.dbContext.SaveChanges();
                                this.dbContext.InsertOrderChangesforReport(item.POrder_Id);
                                //Controlsubstance qty
                                if (item.OnHand != null)
                                {
                                    string[] controlIds = { "I", "II", "III", "IV", "V" };
                                    List<int> records = new List<int>();
                                    var isOrderConsolidated = this.dbContext.ControlSubstanceCounts.Where(c => c.Porder_Id == item.POrder_Id && c.PQuantity_Id == item.PQuantity_Id).Select(c => c.ConsolidateFlag).FirstOrDefault();
                                    var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == item.POrder_Id).Select(c => c.Patient_Id).FirstOrDefault();
                                    if (isOrderConsolidated == 1)
                                    {
                                        var gpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == item.POrder_Id).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
                                        var list = (from dm in this.dbContext.Demographics
                                                    join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                                                    join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                                                    //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
                                                    join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                                                    join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
                                                    //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
                                                    where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi && cs.ConsolidateFlag == 1
                                                    //&& pq.ReviewFlag == 1
                                                    select new
                                                    {
                                                        Controlsub = cs,
                                                    }).Distinct().ToList();
                                        records = list.Select(l => (int)l.Controlsub.PQuantity_Id).ToList();
                                    }
                                    else
                                    {
                                        records.Add(item.PQuantity_Id);
                                    }
                                    if (records.Count() > 0)
                                    {
                                        foreach (var qty in records)
                                        {
                                            var VisitsLatest = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
                                            var controlMed = this.dbContext.ControlSubstanceCounts.Where(co => co.PQuantity_Id == qty).FirstOrDefault();
                                            if (controlMed != null)
                                            {
                                                controlMed.NurseStation_Id = Convert.ToInt32(VisitsLatest.NursingStationId);
                                                controlMed.Quantity = (Convert.ToDecimal(controlMed.Quantity) + Convert.ToDecimal(item.OnHand)).ToString();
                                                //controlMed.InitialQuantity = (Convert.ToDecimal(controlMed.InitialQuantity) + Convert.ToDecimal(item.OnHand)).ToString();
                                                controlMed.CheckInFlag = 1;
                                                this.dbContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var orderStock = new OrderStock()
                                {
                                    Porder_Id = item.POrder_Id,
                                    PQuantity_Id = item.PQuantity_Id,
                                    //Inhand = item.OnHand,
                                    Remaining = item.OnHand,
                                    LotNumber = item.LotNumber,
                                    ExpirationDate = item.ExpirationDate,
                                    OrderStock_CreatedBy = userId,
                                    OrderStock_CreatedDate = item.CheckInDate,
                                    OrderStock_Status = 1
                                };
                                this.dbContext.OrderStocks.Add(orderStock);
                                if (item.OnHand != null)
                                {
                                    OrderStockTran obj = new OrderStockTran();
                                    obj.OrderStock_Id = orderStock.OrderStock_Id;
                                    obj.Inhand = item.OnHand;
                                    obj.LotNumber = item.LotNumber;
                                    obj.ExpirationDate = item.ExpirationDate;
                                    obj.TransOS_CreatedBy = userId;
                                    obj.TransOS_Status = 1;
                                    obj.TransOS_CreatedDate = item.CheckInDate;
                                    this.dbContext.OrderStockTrans.Add(obj);
                                    this.dbContext.SaveChanges();
                                    //ControlMed Qty
                                    string[] controlIds = { "I", "II", "III", "IV", "V" };
                                    List<int> records = new List<int>();
                                    var isOrderConsolidated = this.dbContext.ControlSubstanceCounts.Where(c => c.Porder_Id == item.POrder_Id && c.PQuantity_Id == item.PQuantity_Id).Select(c => c.ConsolidateFlag).FirstOrDefault();
                                    var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == item.POrder_Id).Select(c => c.Patient_Id).FirstOrDefault();
                                    if (isOrderConsolidated == 1)
                                    {
                                        var gpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == item.POrder_Id).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
                                        var list = (from dm in this.dbContext.Demographics
                                                    join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                                                    join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                                                    //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
                                                    join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                                                    join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
                                                    //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
                                                    where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi && cs.ConsolidateFlag == 1
                                                    //&& pq.ReviewFlag == 1
                                                    select new
                                                    {
                                                        Controlsub = cs,
                                                    }).Distinct().ToList();
                                        records = list.Select(l => (int)l.Controlsub.PQuantity_Id).ToList();
                                    }
                                    else
                                    {
                                        records.Add(item.PQuantity_Id);
                                    }
                                    if (records.Count() > 0)
                                    {
                                        foreach (var qty in records)
                                        {
                                            var VisitsLatest = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
                                            var controlMed = this.dbContext.ControlSubstanceCounts.Where(co => co.PQuantity_Id == qty).FirstOrDefault();
                                            if (controlMed != null)
                                            {
                                                controlMed.NurseStation_Id = Convert.ToInt32(VisitsLatest.NursingStationId);
                                                controlMed.Quantity = (Convert.ToDecimal(controlMed.Quantity) + Convert.ToDecimal(item.OnHand)).ToString();
                                                //controlMed.InitialQuantity = (Convert.ToDecimal(controlMed.InitialQuantity) + Convert.ToDecimal(item.OnHand)).ToString();
                                                controlMed.CheckInFlag = 1;
                                                this.dbContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (item.Barcode != null && item.Barcode != "")
                        {
                            var barcode = new BarcodeDetail()
                            {
                                POrder_Id = item.POrder_Id,
                                BarcodeDetail1 = item.Barcode,
                                PBarcode_CreatedBy = userId,
                                PBarcode_CreatedDate = item.CheckInDate,
                                PBarcode_Status = 1
                            };
                            this.dbContext.BarcodeDetails.Add(barcode);
                            this.dbContext.SaveChanges();
                        }
                    }
                    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                    {
                        Screen_Id = (int)ScreenEntity.Screens.CheckInMeds,
                        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                        Comments = "",
                        Session_Id = 0,
                        Time = DateTime.Now,
                        UserActivity_Id = 0,

                    };

                    _userActivityRepository.InsertUserActivityDetails(activityEntity);
                    return 1;
                }
            }
            return 0;
        }
        public List<OrderStockTransEntity> GetOrderStockTrans(int quantityId)
        {
            var orderId = this.dbContext.QuantityDetails.Where(q => q.PQuantity_Id == quantityId).Select(q => q.POrder_Id).FirstOrDefault();
            var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == orderId).Select(c => c.Patient_Id).FirstOrDefault();
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            //12/09/2022 changed company id to facility id
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var orderStockId = this.dbContext.OrderStocks.Where(or => or.PQuantity_Id == quantityId).Select(or => or.OrderStock_Id).FirstOrDefault();
            var recentCheckin = this.dbContext.OrderStockTrans.Where(tr => tr.OrderStock_Id == orderStockId).OrderByDescending(tr => tr.TransOS_CreatedDate).Select(tr => tr.TransOS_CreatedDate).FirstOrDefault();
            var beforeRecentCheckin = this.dbContext.OrderStockTrans.Where(tr => tr.OrderStock_Id == orderStockId && tr.TransOS_CreatedDate != recentCheckin).OrderByDescending(tr => tr.TransOS_CreatedDate).Select(tr => tr.TransOS_CreatedDate).FirstOrDefault();
            var records = (from ost in this.dbContext.OrderStockTrans
                           join os in this.dbContext.OrderStocks on ost.OrderStock_Id equals os.OrderStock_Id
                           join en in this.dbContext.EncodedOrderDetails on os.Porder_Id equals en.POrder_Id
                           join us in this.dbContext.Users on ost.TransOS_CreatedBy equals us.User_Id
                           where os.PQuantity_Id == quantityId && (EntityFunctions.TruncateTime(ost.TransOS_CreatedDate) == EntityFunctions.TruncateTime(recentCheckin) || EntityFunctions.TruncateTime(ost.TransOS_CreatedDate) == EntityFunctions.TruncateTime(beforeRecentCheckin))
                           select new //OrderStockTransEntity
                           {
                               orderStock = os,
                               orderStockTrans = ost,
                               encoded = en,
                               user = us,
                               //POrder_Id=os.Porder_Id,
                               //DrugName = en.GiveCodeText,
                               //TransOS_CreatedBy = us.UserName,
                               //TransOS_CreatedDate =  ost.TransOS_CreatedDate,
                               //Inhand = ost.Inhand,
                               //LotNumber=ost.LotNumber,
                           }).OrderByDescending(item => item.orderStockTrans.TransOS_CreatedDate).ToList()
                           .Select(x => new OrderStockTransEntity
                           {
                               POrder_Id = x.orderStock.Porder_Id,
                               DrugName = x.encoded.GiveCodeText,
                               TransOS_CreatedBy = x.user.UserName,
                               TransOS_CreatedDate = GetTimeZoneDateTime(x.orderStockTrans.TransOS_CreatedDate, companyId),
                               Inhand = x.orderStockTrans.Inhand,
                               LotNumber = x.orderStockTrans.LotNumber,
                           }).ToList();
            return records;
        }
        public Nullable<DateTime> GetTimeZoneDateTime(DateTime? dateTime, int? companyId)
        {
            var convetedDate = (this.dbContext.GetTimeZoneConvertedDateTime(dateTime, companyId).FirstOrDefault());
            return convetedDate;
        }
        public List<BarcodeCheckEntity> UnionBarcodeDetails(int facility_Id)
        {

            DataTable dt = new DataTable();
            string query = "[Admin].[unionBarcodeDetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facility_Id;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }



            var EkitGridData = (from d in dt.AsEnumerable()
                                select new BarcodeCheckEntity
                                {

                                    Barcode = d["barcodedetail"].ToString(),
                                    AGiveCodeIdentifier = d["AGiveCodeIdentifier"].ToString()
                                    //Inhand = string.IsNullOrEmpty(d["inhand"].ToString()) ? (Int32?) null : Convert.ToInt32(d("inhand")),


                                }).ToList();
            return EkitGridData;
        }
        public List<EkitdrugEntity> GetEkitGridDetails(int FacilityId, int NsId)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_EkitDetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@nursing_id", SqlDbType.Int).Value = NsId == null ? (object)DBNull.Value : NsId;
                    cmd.Parameters.Add("@facility_id", SqlDbType.Int).Value = FacilityId == null ? (object)DBNull.Value : FacilityId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }



            var EkitGridData = (from d in dt.AsEnumerable()
                                select new EkitdrugEntity
                                {
                                    facility_id = string.IsNullOrEmpty(d["facility_id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["facility_id"]),
                                    NurseStation_Id = string.IsNullOrEmpty(d["NurseStation_Id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["NurseStation_Id"]),
                                    DrugName = d["DrugName"].ToString(),
                                    //GpiCode = string.IsNullOrEmpty(d["GpiCode"].ToString()) ? null : d["GpiCode"].ToString(),
                                    GpiCode = d["GpiCode"].ToString(),
                                    TotalNonExpiredInhand = string.IsNullOrEmpty(d["TotalNonExpiredInhand"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["TotalNonExpiredInhand"]), //nullable decimal
                                    TotalExpiredInhand = string.IsNullOrEmpty(d["TotalExpiredInhand"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["TotalExpiredInhand"]), //nullable decimal

                                }).OrderBy(item => item.DrugName).ToList();
            return EkitGridData;
        }
        public List<DestroyEkitGrid> GetExpiredEkitMedDestruction(int facilityId, int nurseStationId)
        {

            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_GetExpiredEkitMedDestruction]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@facility_id", SqlDbType.Int).Value = facilityId;
                    cmd.Parameters.Add("@nursing_id", SqlDbType.Int).Value = nurseStationId;



                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }

                }
            }



            var EkitGridData = (from d in dt.AsEnumerable()
                                select new DestroyEkitGrid
                                {
                                    Drugname = d["Drugname"].ToString(),
                                    Ekit_Id = string.IsNullOrEmpty(d["Ekit_id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Ekit_id"]),
                                    LotNumber = d["LotNumber"].ToString(),
                                    ExpiryDate = string.IsNullOrEmpty(d["ExpiryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["ExpiryDate"]),//date  //na
                                                                                                                                                          // Inhand = string.IsNullOrEmpty(d["inhand"].ToString()) ? (Int32?)null : Convert.ToInt32(d["inhand"]), //nullable int
                                    Inhand = d["inhand"].ToString(),
                                    BarcodeDetail = d["BarcodeDetail"].ToString(),
                                    //NonExpiredQty=d["NonExpiredQty"].ToString(),
                                    //Inhand = string.IsNullOrEmpty(d["inhand"].ToString()) ? (Int32?) null : Convert.ToInt32(d("inhand"))
                                    currentQuantity = d["currentQuantity"].ToString(),
                                    reason = d["reason"].ToString(),
                                    nursestationname = d["nursestationname"].ToString()

                                }).ToList();
            return EkitGridData;
        }
        public List<EkitlotEntity> GetEkitLotDetails(int FacilityId, int NsId, string drugName)
        {
            //string decodedDrugName = Uri.UnescapeDataString(drugName);

            //decodedDrugName = decodedDrugName.Replace('#', '/');
            //decodedDrugName = decodedDrugName.Replace('$', '.');
            //decodedDrugName = decodedDrugName.Replace('%', '@');

            string decodedDrugName = Uri.UnescapeDataString(drugName);

            // Split the drug name by space
            var drugNameParts = decodedDrugName.ToCharArray();

            // Loop through each part and replace special characters
            for (int i = 0; i < drugNameParts.Length; i++)
            {
                drugNameParts[i] = ReplaceSpecialCharacters(drugNameParts[i]);
            }

            // Join the parts back into a single string
            decodedDrugName = string.Join("", drugNameParts).Trim();

            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_GetEkitLotDetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@nursing_id", SqlDbType.Int).Value = (NsId == null || NsId == 0) ? (object)DBNull.Value : NsId;
                    cmd.Parameters.Add("@facility_id", SqlDbType.Int).Value = FacilityId == null ? (object)DBNull.Value : FacilityId;
                    cmd.Parameters.Add("@drugname", SqlDbType.VarChar).Value = decodedDrugName;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }



            var EkitGridData = (from d in dt.AsEnumerable()
                                select new EkitlotEntity
                                {
                                    LotNumber = d["lotnumber"].ToString(),
                                    barcodedetail = d["barcodedetails"].ToString(),
                                    ExpiryDate = string.IsNullOrEmpty(d["expirydate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["ExpiryDate"]),//date  //na
                                    //Inhand = string.IsNullOrEmpty(d["inhand"].ToString()) ? (Int32?)null : Convert.ToInt32(d["inhand"]), //nullable int
                                    Inhand = d["inhand"].ToString(),

                                    Ekit_Id = Convert.ToInt32(d["ekit_id"]), //nullable int 
                                    Updated_date = string.IsNullOrEmpty(d["ekit_createdOn"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["ekit_createdOn"]),

                                }).OrderBy(item => item.ExpiryDate).ToList();
            return EkitGridData;
        }
        private char ReplaceSpecialCharacters(char input)
        {
            switch (input)
            {
                case '~':
                    return '/';
                case '$':
                    return '.';
                case '@':
                    return '%';
                case '!':
                    return ':';
                case '{':
                    return '*';
                //case '}':
                //    return '\'';
                case '_':
                    return '>';
                case '}':
                    return '<';
                case '^':
                    return '+';
                case '`':
                    return '&';
                // Add more replacements if needed
                default:
                    return input;
            }
        }
        public string EkitDestroyQuantity(List<EkitDestroyQuantity> EkitDestroyObj)
        {
            foreach (var item in EkitDestroyObj)
            {
                //item.Ekit_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());

                item.Ekit_CreatedOn = DateTime.Now;

                DataTable dt = new DataTable();
                string query = "[Admin].[Prc_UpdateEkitOnhandQty]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@Ekit_id", SqlDbType.Int).Value = item.Ekit_Id;
                        cmd.Parameters.Add("@updatedqty", SqlDbType.VarChar).Value = item.InHandqty;
                        cmd.Parameters.Add("@updatedby", SqlDbType.Int).Value = item.DestroyerUserId;
                        cmd.Parameters.Add("@witness", SqlDbType.Int).Value = item.ApprovalUserId;
                        cmd.Parameters.Add("@updatedOn", SqlDbType.VarChar).Value = item.Ekit_CreatedOn;
                        cmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = item.Reason;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            return "Done";
        }
        public int InsertEkitDetails(InsertEkitLotEntity obj)
        {
            //obj.Ekit_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            obj.Ekit_CreatedOn = DateTime.Now;

            DataTable dt = new DataTable();
            string query = obj.EkitUpdate == 0 ? "[Admin].[Prc_InsertEkitDetails]" : "[Admin].[Prc_UpdateEkitLotDetails_edit]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@facility_id", SqlDbType.Int).Value = obj.Facility_Id == null ? (object)DBNull.Value : obj.Facility_Id;
                    cmd.Parameters.Add("@nursing_id", SqlDbType.Int).Value = (obj.NurseStation_Id == null || obj.NurseStation_Id == 0) ? (object)DBNull.Value : obj.NurseStation_Id;
                    cmd.Parameters.Add("@drugname", SqlDbType.VarChar).Value = obj.DrugName;
                    cmd.Parameters.Add("@lotnumber", SqlDbType.VarChar).Value = obj.LotNumber;
                    cmd.Parameters.Add("@expdate", SqlDbType.DateTime).Value = obj.ExpiryDate;
                    cmd.Parameters.Add("@inhand", SqlDbType.VarChar).Value = obj.Inhand;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = obj.Barcode;
                    cmd.Parameters.Add("@barcodeCreatedby", SqlDbType.Int).Value = obj.Ekit_CreatedBy == null ? (object)DBNull.Value : obj.Ekit_CreatedBy;
                    cmd.Parameters.Add("@gpicode", SqlDbType.VarChar).Value = obj.GpiCode;
                    cmd.Parameters.Add("@ekit_CreatedOn", SqlDbType.VarChar).Value = obj.Ekit_CreatedOn;


                    if (obj.EkitUpdate == 1)
                    {
                        cmd.Parameters.Add("@Ekit_id", SqlDbType.VarChar).Value = obj.EditEkit_id;
                        cmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = obj.Reason;

                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var result = (from d in dt.AsEnumerable()
                          select d["alertMsg"]).FirstOrDefault();
            return (int)result;

        }
        public int UpdateEkitDrugQuantity(List<UpdateEkitDrugQty> obj)
        {
            foreach (var item in obj)
            {
                DataTable dt = new DataTable();
                string query = "[Admin].[Prc_UpdateEkitDrugQuantity]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = item.InHandqty;
                        cmd.Parameters.Add("@ekitid", SqlDbType.Int).Value = item.Ekit_Id;
                        cmd.Parameters.Add("@LotNumber", SqlDbType.VarChar).Value = item.LotNumber;
                        cmd.Parameters.Add("@remainingekitids", SqlDbType.VarChar).Value = item.remainingekitids;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            return 1;
        }
        public int FlagEkitLotsGrid(FlagekitGrid obj)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_FlagEkitLotsGrid]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@ekit_id ", SqlDbType.VarChar).Value = obj.Ekit_Id;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = obj.Barcode;
                    cmd.Parameters.Add("@lotnumber", SqlDbType.VarChar).Value = obj.LotNumber;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var result = (from d in dt.AsEnumerable()
                          select d["barcodematch"]).FirstOrDefault();

            return (int)result;

        }
        public List<ekitLostGrid> GetEkitLotsGrid(string drugName, string barcode, int facilityId, int nursingStationId)
        {
            //string decodedDrugName = Uri.UnescapeDataString(drugName);

            //decodedDrugName = decodedDrugName.Replace('#', '/');
            //decodedDrugName = decodedDrugName.Replace('$', '.');
            //decodedDrugName = decodedDrugName.Replace('%', '@');
            string decodedDrugName = Uri.UnescapeDataString(drugName);

            // Split the drug name by space
            var drugNameParts = decodedDrugName.ToCharArray();

            // Loop through each part and replace special characters
            for (int i = 0; i < drugNameParts.Length; i++)
            {
                drugNameParts[i] = ReplaceSpecialCharacters(drugNameParts[i]);
            }

            // Join the parts back into a single string
            decodedDrugName = string.Join("", drugNameParts).Trim();


            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_GetEkitLotsGrid]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@drugname", SqlDbType.VarChar).Value = decodedDrugName;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = barcode;
                    cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilityId;
                    cmd.Parameters.Add("@nursingStationId", SqlDbType.Int).Value = nursingStationId;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }



            var EkitGridData = (from d in dt.AsEnumerable()
                                select new ekitLostGrid
                                {
                                    Ekit_Id = string.IsNullOrEmpty(d["ekit_id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["ekit_id"]),
                                    LotNumber = d["LotNumber"].ToString(),
                                    ExpiryDate = string.IsNullOrEmpty(d["ExpiryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["ExpiryDate"]),//date  //na
                                                                                                                                                          // Inhand = string.IsNullOrEmpty(d["inhand"].ToString()) ? (Int32?)null : Convert.ToInt32(d["inhand"]), //nullable int
                                    Inhand = d["inhand"].ToString(),
                                    //BarcodeMatch = d["barcodeMatch"].ToString(),
                                    Barcode = d["barcode"].ToString(),
                                    //Inhand = string.IsNullOrEmpty(d["inhand"].ToString()) ? (Int32?) null : Convert.ToInt32(d("inhand")),
                                    QtyAdminster = string.IsNullOrEmpty(d["QtyAdministered"].ToString()) ? (Int32?)0 : Convert.ToInt32(d["inhand"]),
                                    remaining_ekit_ids = d["remaining_ekit_ids"].ToString(),



                                }).ToList();
            return EkitGridData;
        }
        public int AlertEkitAdminister(InsertDrugBarcEkit obj)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_AlertEkitAdminister]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = obj.Barcode;
                    cmd.Parameters.Add("@drugname", SqlDbType.VarChar).Value = obj.DrugName;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var result = (from d in dt.AsEnumerable()
                          select d["alertcode"]).FirstOrDefault();
            return (int)result;
        }
        public List<DrugEkitEntity> GetEkitadministration(string gpi)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_GetEkitadministration]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@GpiCode", SqlDbType.VarChar).Value = gpi;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var EkitGridData = (from d in dt.AsEnumerable()
                                select new DrugEkitEntity
                                {
                                    DrugName = d["DrugName"].ToString(),
                                }).OrderBy(item => item.DrugName).ToList();
            return EkitGridData;
        }
        public List<DtmsEntity> DrugToDrugINT_DTMS(string gpicode, string patientId, string route, int? freqId, string drug)
        {
            string decodedDrugName = Uri.UnescapeDataString(drug);

            // Split the drug name by space
            var drugNameParts = decodedDrugName.ToCharArray();

            // Loop through each part and replace special characters
            for (int i = 0; i < drugNameParts.Length; i++)
            {
                drugNameParts[i] = ReplaceSpecialCharacters(drugNameParts[i]);
            }

            // Join the parts back into a single string
            decodedDrugName = string.Join("", drugNameParts).Trim();



            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_DrugToDrugINT_DTMS]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@gpicode", SqlDbType.VarChar).Value = (object)gpicode ?? DBNull.Value;
                    cmd.Parameters.Add("@frequency", SqlDbType.Int).Value = freqId.HasValue ? (object)freqId.Value : DBNull.Value;
                    cmd.Parameters.Add("@route", SqlDbType.VarChar).Value = (object)route ?? DBNull.Value;
                    cmd.Parameters.Add("@patientid", SqlDbType.VarChar).Value = (object)patientId ?? DBNull.Value;
                    cmd.Parameters.Add("@drugname", SqlDbType.VarChar).Value = (object)decodedDrugName ?? DBNull.Value;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }

                }
            }
            var EkitGridData = (from d in dt.AsEnumerable()
                                select new DtmsEntity
                                {
                                    AlertId = Convert.ToInt32(d["ID"]),
                                    AlertContent = d["AlertContent"].ToString(),
                                }).ToList();
            return EkitGridData;
        }
        public List<DrugAllergyEntity> DrugToAllergyINT_DTMS(string gpicode, int patientId, string drug)
        {
            string decodedDrugName = Uri.UnescapeDataString(drug);

            // Split the drug name by space
            var drugNameParts = decodedDrugName.ToCharArray();

            // Loop through each part and replace special characters
            for (int i = 0; i < drugNameParts.Length; i++)
            {
                drugNameParts[i] = ReplaceSpecialCharacters(drugNameParts[i]);
            }

            // Join the parts back into a single string
            decodedDrugName = string.Join("", drugNameParts).Trim();


            DataTable dt = new DataTable();
            string query = "[dbo].[usp_CheckDrugAllergy]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@gpicode", SqlDbType.VarChar).Value = gpicode;
                    cmd.Parameters.Add("@patientid", SqlDbType.Int).Value = patientId;
                    cmd.Parameters.Add("@drugname", SqlDbType.VarChar).Value = decodedDrugName;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }

                }
            }
            var EkitAllergyGridData = (from d in dt.AsEnumerable()
                                       select new DrugAllergyEntity
                                       {
                                           allergyAlertId = Convert.ToInt32(d["DEFINITION_ID"]),
                                           alertContent = d["ALERT"].ToString(),
                                       }).ToList();
            return EkitAllergyGridData;
        }
    }
}
