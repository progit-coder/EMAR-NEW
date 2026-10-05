using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LTCPro.Repositories
{
    public class StockEkitRepository : IStockEkitRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly ICommonRepository _commonRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        public StockEkitRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, CommonRepository commonRepository, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._commonRepository = commonRepository;
            _userActivityRepository = userActivityRepository;
        }
        public List<BarcodeEntity> GetAllBarcodes(int facilityId)
        {
            //var records = this.dbContext.BarcodeDetails.ToList();
            //return this.autoMapper.Map<List<BarcodeDetail>, List<BarcodeEntity>>(records);
            if (facilityId != 0)
            {
                var records = (from bd in this.dbContext.BarcodeDetails
                               join gp in this.dbContext.EncodedOrderDetails on bd.POrder_Id equals gp.POrder_Id into gptab
                               from gpc in gptab.DefaultIfEmpty()
                               join cm in this.dbContext.CommonOrderInfoes on gpc.POrder_Id equals cm.POrder_Id
                               join vi in this.dbContext.VisitInfoes on cm.Patient_Id equals vi.Patient_Id
                               where vi.FacilityId == facilityId && bd.PBarcode_Status == 1
                               select new BarcodeEntity
                               {
                                   PBarcode_Id = bd.PBarcode_Id,
                                   BarcodeDetail1 = bd.BarcodeDetail1,
                                   GPICode = gpc.AGiveCodeIdentifier,
                                   Patient_Id = cm.Patient_Id
                               }).ToList();

                return records;
            }

            else
            {
                var records = (from bd in this.dbContext.BarcodeDetails
                               join gp in this.dbContext.EncodedOrderDetails on bd.POrder_Id equals gp.POrder_Id into gptab
                               from gpc in gptab.DefaultIfEmpty()
                               join cm in this.dbContext.CommonOrderInfoes on gpc.POrder_Id equals cm.POrder_Id
                               // join vi in this.dbContext.VisitInfoes on cm.Patient_Id equals vi.Patient_Id
                               where /*vi.NursingStationId == nurseStationId &&*/ bd.PBarcode_Status == 1
                               select new BarcodeEntity
                               {
                                   PBarcode_Id = bd.PBarcode_Id,
                                   BarcodeDetail1 = bd.BarcodeDetail1,
                                   GPICode = gpc.AGiveCodeIdentifier,
                                   Patient_Id = cm.Patient_Id
                               }).ToList();

                return records;
            }
            
        }
        public List<BarcodeEntity> GetAllStockEkitBarcodes(int Flag)
        {
            // var records = this.dbContext.BarcodeDetails.Where(U=>U.BarcodeDetail1 == "3434").ToList();
            //return this.autoMapper.Map<List<BarcodeDetail>, List<BarcodeEntity>>(records);
            if (Flag == 0)
            {


                var records = (from bd in this.dbContext.BarcodeDetails
                               join st in this.dbContext.Stocks on bd.Stock_Id equals st.Stock_Id into sttab
                               from sti in sttab.DefaultIfEmpty()
                               join gp in this.dbContext.EncodedOrderDetails on bd.POrder_Id equals gp.POrder_Id into gptab
                               from gpc in gptab.DefaultIfEmpty()
                               join cm in this.dbContext.CommonOrderInfoes on gpc.POrder_Id equals cm.POrder_Id into com
                               from comi in com.DefaultIfEmpty()
                               where bd.PBarcode_Status==1
                               select new BarcodeEntity
                               {
                                   PBarcode_Id = bd.PBarcode_Id,
                                   BarcodeDetail1 = bd.BarcodeDetail1,
                                   GPICode = sti.GPICode == null ? gpc.AGiveCodeIdentifier : sti.GPICode,
                                   Patient_Id = comi.Patient_Id,
                                   Facility_Id = sti.Facility_Id
                               }).ToList();
                return records;

            }
            else
            {


                var records = (from bd in this.dbContext.BarcodeDetails
                               join st in this.dbContext.Ekits on bd.Ekit_Id equals st.Ekit_Id into sttab
                               from sti in sttab.DefaultIfEmpty()
                               join gp in this.dbContext.EncodedOrderDetails on bd.POrder_Id equals gp.POrder_Id into gptab
                               from gpc in gptab.DefaultIfEmpty()
                               join cm in this.dbContext.CommonOrderInfoes on gpc.POrder_Id equals cm.POrder_Id into com
                               from comi in com.DefaultIfEmpty()
                               where bd.PBarcode_Status == 1
                               select new BarcodeEntity
                               {
                                   PBarcode_Id = bd.PBarcode_Id,
                                   BarcodeDetail1 = bd.BarcodeDetail1,
                                   GPICode = sti.GPICode == null ? gpc.AGiveCodeIdentifier : sti.GPICode,
                                   Patient_Id = comi.Patient_Id,
                                   Facility_Id = sti.Facility_Id
                               }).ToList();
                return records;
            }
        }
        //public int InsertUpdateStock(StockCustomEntity entity)
        //{
        //    entity.Stock_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
        //    if (entity.Facility_Id != 0 && (entity.NurseStation_Id != 0 && entity.NurseStation_Id !=null))
        //    {
        //        //var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == entity.NurseStation_Id).Select(n => n.Facility_Id).FirstOrDefault();
        //        var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //        if (claimsIdentity.FindFirst("UserId").Value != "")
        //        {
        //            int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
        //            RecentFacEntity userRecentFacObj = new RecentFacEntity()
        //            {
        //                User_Id = userId,
        //                Facility_Id = (int)entity.Facility_Id,
        //                NurseStation_Id = entity.NurseStation_Id.ToString()
        //            };
        //            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
        //        }
        //    }
        //    int id = 0;
        //    string[] stockbarcodes = this.dbContext.BarcodeDetails.Where(c => c.Stock_Id == entity.Stock_Id).Select(c => c.BarcodeDetail1.ToString()).ToArray();
        //    var record = this.autoMapper.Map<StockCustomEntity, Stock>(entity);
        //    var stock = this.dbContext.Stocks.Where(pt => pt.Stock_Id == entity.Stock_Id).FirstOrDefault();
        //    if (stock == null)
        //    {
        //        if (entity.Sharedstockbit == 0 && entity.NurseStation_Id != 0)
        //        {
        //            //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode).FirstOrDefault();
        //            var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.DrugName==entity.DrugName).FirstOrDefault();
        //            if (exisitngRecordWithSameGPI == null)
        //            {
        //                this.dbContext.Stocks.Add(record);
        //                this.dbContext.SaveChanges();
        //                id = record.Stock_Id;
        //            }
        //            else
        //                return -1;
        //        }
        //        else if (entity.Sharedstockbit == 1)
        //        {
        //            //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode).FirstOrDefault();
        //            var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.DrugName==entity.DrugName).FirstOrDefault();
        //            if (exisitngRecordWithSameGPI == null)
        //            {
        //                this.dbContext.Stocks.Add(record);
        //                this.dbContext.SaveChanges();
        //                id = record.Stock_Id;
        //            }
        //            else
        //                return -2;
        //        }
        //    }
        //    else
        //    {
        //        if (entity.Sharedstockbit == 0 && entity.NurseStation_Id != 0)
        //        {
        //            //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.Stock_Id != record.Stock_Id).FirstOrDefault();
        //            var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.DrugName== entity.DrugName && n.Stock_Id != record.Stock_Id).FirstOrDefault();
        //            if (exisitngRecordWithSameGPI == null)
        //            {
        //                Stock data = this.dbContext.Stocks.Find(record.Stock_Id);
        //                data.Facility_Id = entity.Facility_Id;
        //                data.NurseStation_Id = entity.Sharedstockbit == 1 ? null : entity.NurseStation_Id;
        //                data.Sharedstockbit = entity.Sharedstockbit;
        //                data.DrugName = entity.DrugName;
        //                data.GPICode = entity.GPICode;
        //                data.TrackableBit = entity.TrackableBit;
        //                data.InHand = entity.InHand;
        //                data.Stock_Status = entity.Stock_Status;
        //                data.Stock_CreatedBy = entity.Stock_CreatedBy;
        //                data.Stock_CreatedDate = entity.Stock_CreatedDate;
        //                this.dbContext.SaveChanges();
        //            }
        //            else
        //                return -1;
        //        }
        //        else if (entity.Sharedstockbit == 1)
        //        {
        //            //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.Stock_Id != record.Stock_Id).FirstOrDefault();
        //            var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.DrugName==entity.DrugName && n.Stock_Id != record.Stock_Id).FirstOrDefault();
        //            if (exisitngRecordWithSameGPI == null)
        //            {
        //                Stock data = this.dbContext.Stocks.Find(record.Stock_Id);
        //                data.Facility_Id = entity.Facility_Id;
        //                data.NurseStation_Id = entity.Sharedstockbit == 1 ? null : entity.NurseStation_Id;
        //                data.Sharedstockbit = entity.Sharedstockbit;
        //                data.DrugName = entity.DrugName;
        //                data.GPICode = entity.GPICode;
        //                data.TrackableBit = entity.TrackableBit;
        //                data.InHand = entity.InHand;
        //                data.Stock_Status = entity.Stock_Status;
        //                data.Stock_CreatedBy = entity.Stock_CreatedBy;
        //                data.Stock_CreatedDate = entity.Stock_CreatedDate;
        //                this.dbContext.SaveChanges();
        //            }
        //            else
        //                return -2;
        //        }
        //    }
        //    if (entity.Barcode.Length > 0)
        //    {
        //        for (int i = 0; i < entity.Barcode.Length; i++)
        //        {
        //            string barcode = entity.Barcode[i];
        //            var barcodeRecord = this.dbContext.BarcodeDetails.Where(br => br.Stock_Id == entity.Stock_Id && br.BarcodeDetail1 == barcode).FirstOrDefault();
        //            if (barcodeRecord == null)
        //            {
        //                barcodeRecord = new BarcodeDetail();
        //                barcodeRecord.PBarcode_Id = 0;
        //                barcodeRecord.POrder_Id = null;
        //                barcodeRecord.Stock_Id = entity.Stock_Id == 0 ? id : entity.Stock_Id;
        //                barcodeRecord.Ekit_Id = null;
        //                barcodeRecord.BarcodeDetail1 = entity.Barcode[i];
        //                barcodeRecord.PBarcode_CreatedBy = entity.Stock_CreatedBy;
        //                barcodeRecord.PBarcode_Status = (int)entity.Stock_Status;
        //                barcodeRecord.PBarcode_CreatedDate = (DateTime)entity.Stock_CreatedDate;
        //                this.dbContext.BarcodeDetails.Add(barcodeRecord);
        //                this.dbContext.SaveChanges();
        //            }
        //            else
        //            {
        //                barcodeRecord = this.dbContext.BarcodeDetails.Where(br => br.Stock_Id == entity.Stock_Id && br.BarcodeDetail1 == barcode).FirstOrDefault();
        //                barcodeRecord.POrder_Id = null;
        //                barcodeRecord.Ekit_Id = null;
        //                barcodeRecord.BarcodeDetail1 = entity.Barcode[i];
        //                barcodeRecord.PBarcode_CreatedBy = entity.Stock_CreatedBy;
        //                barcodeRecord.PBarcode_Status = (int)entity.Stock_Status;
        //                barcodeRecord.PBarcode_CreatedDate = (DateTime)entity.Stock_CreatedDate;
        //                this.dbContext.SaveChanges();


        //                var items = stockbarcodes.Except(entity.Barcode);
        //                foreach (var item in items)
        //                {
        //                    BarcodeDetail list = this.dbContext.BarcodeDetails.Where(c => c.Stock_Id == entity.Stock_Id && c.BarcodeDetail1 == item).FirstOrDefault();
        //                    if (list != null)
        //                    {
        //                        this.dbContext.BarcodeDetails.Remove(list);
        //                        this.dbContext.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (entity.Barcode.Length == 0)
        //    {
        //        foreach (var item in stockbarcodes)
        //        {
        //            BarcodeDetail list = this.dbContext.BarcodeDetails.Where(c => c.Stock_Id == entity.Stock_Id && c.BarcodeDetail1 == item).FirstOrDefault();
        //            if (list != null)
        //            {
        //                this.dbContext.BarcodeDetails.Remove(list);
        //                this.dbContext.SaveChanges();
        //            }
        //        }
        //    }
        //    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //    {
        //        Screen_Id = (int)ScreenEntity.Screens.Stock,
        //        Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //        Comments = record.Stock_Id.ToString(),
        //        Session_Id = 0,
        //        Time = DateTime.Now,
        //        UserActivity_Id = 0,

        //    };

        //    _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //    return 1;
        //}
        public int InsertUpdateStock(StockCustomEntity entity)
        {
            entity.Stock_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (entity.Facility_Id != 0 && (entity.NurseStation_Id != 0 && entity.NurseStation_Id != null))
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity()
                    {
                        User_Id = userId,
                        Facility_Id = (int)entity.Facility_Id,
                        NurseStation_Id = entity.NurseStation_Id.ToString()
                    };
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            int id = 0;
            string[] stockbarcodes = this.dbContext.BarcodeDetails.Where(c => c.Stock_Id == entity.Stock_Id).Select(c => c.BarcodeDetail1.ToString()).ToArray();
            var record = this.autoMapper.Map<StockCustomEntity, Stock>(entity);
            var stock = this.dbContext.Stocks.Where(pt => pt.Stock_Id == entity.Stock_Id).FirstOrDefault();
            if (stock == null)
            {
                if (entity.Sharedstockbit == 0 && entity.NurseStation_Id != 0)
                {
                    //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode).FirstOrDefault();
                    var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode.Trim() && n.DrugName == entity.DrugName.Trim() && (n.Sharedstockbit == 0 || n.Sharedstockbit == null)).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {
                        this.dbContext.Stocks.Add(record);
                        this.dbContext.SaveChanges();
                        id = record.Stock_Id;
                    }
                    else
                        return -1;
                }
                else if (entity.Sharedstockbit == 1)
                {
                    //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode).FirstOrDefault();
                    var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.DrugName.Trim() == entity.DrugName.Trim() && n.Sharedstockbit == 1).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {
                        this.dbContext.Stocks.Add(record);
                        this.dbContext.SaveChanges();
                        id = record.Stock_Id;
                    }
                    else
                        return -2;
                }
            }
            else
            {

                if (entity.Sharedstockbit == 0 && entity.NurseStation_Id != 0)
                {
                    //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.Stock_Id != record.Stock_Id).FirstOrDefault();
                    var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.DrugName.Trim() == entity.DrugName.Trim() && n.Stock_Id != record.Stock_Id && (n.Sharedstockbit == 0 || n.Sharedstockbit == null)).FirstOrDefault();

                    // if(exisitngRecordWithSameGPI.Sharedstockbit == 1)

                    if (exisitngRecordWithSameGPI == null && entity.MergedStockIds.Length == 0)
                    {
                        Stock data = this.dbContext.Stocks.Find(record.Stock_Id);
                        data.Facility_Id = entity.Facility_Id;
                        data.NurseStation_Id = entity.Sharedstockbit == 1 ? null : entity.NurseStation_Id;
                        data.Sharedstockbit = entity.Sharedstockbit;
                        data.DrugName = entity.DrugName;
                        data.GPICode = entity.GPICode;
                        data.TrackableBit = entity.TrackableBit;
                        data.InHand = entity.InHand;
                        data.Stock_Status = entity.Stock_Status;
                        data.Stock_CreatedBy = entity.Stock_CreatedBy;
                        data.Stock_CreatedDate = entity.Stock_CreatedDate;
                        this.dbContext.SaveChanges();
                        if (entity.Stock_Status == 0)
                        {
                            InactiveToActiveStrockBarcode(entity.Stock_Id, entity.Stock_CreatedBy);

                        }
                    }
                    else if (entity.MergedStockIds.Length != 0)
                    {
                        UpdatedSecondaryStockDetails(entity.Stock_Id, entity.MergedStockIds, entity.Barcode, entity.InHand, entity.Stock_CreatedBy, (DateTime)entity.Stock_CreatedDate);
                        Stock data = this.dbContext.Stocks.Find(record.Stock_Id);
                        data.Facility_Id = entity.Facility_Id;
                        data.NurseStation_Id = entity.Sharedstockbit == 1 ? null : entity.NurseStation_Id;
                        data.Sharedstockbit = entity.Sharedstockbit;
                        data.DrugName = entity.DrugName;
                        data.GPICode = entity.GPICode;
                        data.TrackableBit = entity.TrackableBit;
                        data.Stock_Status = entity.Stock_Status;
                        data.Stock_CreatedDate = entity.Stock_CreatedDate;
                        this.dbContext.SaveChanges();
                        if (entity.Stock_Status == 0)
                        {
                            InactiveToActiveStrockBarcode(entity.Stock_Id, entity.Stock_CreatedBy);

                        }
                    }
                    else
                        return -1;
                }
                else if (entity.Sharedstockbit == 1)
                {
                    //var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.Stock_Id != record.Stock_Id).FirstOrDefault();
                    var exisitngRecordWithSameGPI = this.dbContext.Stocks.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.DrugName.Trim() == entity.DrugName.Trim() && n.Stock_Id != record.Stock_Id && n.Sharedstockbit == 1).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {
                        Stock data = this.dbContext.Stocks.Find(record.Stock_Id);
                        data.Facility_Id = entity.Facility_Id;
                        data.NurseStation_Id = entity.Sharedstockbit == 1 ? null : entity.NurseStation_Id;
                        data.Sharedstockbit = entity.Sharedstockbit;
                        data.DrugName = entity.DrugName;
                        data.GPICode = entity.GPICode;
                        data.TrackableBit = entity.TrackableBit;
                        data.InHand = entity.InHand;
                        data.Stock_Status = entity.Stock_Status;
                        data.Stock_CreatedBy = entity.Stock_CreatedBy;
                        data.Stock_CreatedDate = entity.Stock_CreatedDate;
                        this.dbContext.SaveChanges();
                        if (entity.Stock_Status == 0)
                        {
                            InactiveToActiveStrockBarcode(entity.Stock_Id, entity.Stock_CreatedBy);
                        }
                    }
                    else if (entity.MergedStockIds.Length != 0)
                    {
                        UpdatedSecondaryStockDetails(entity.Stock_Id, entity.MergedStockIds, entity.Barcode, entity.InHand, entity.Stock_CreatedBy, (DateTime)entity.Stock_CreatedDate);
                        Stock data = this.dbContext.Stocks.Find(record.Stock_Id);
                        data.Facility_Id = entity.Facility_Id;
                        data.NurseStation_Id = entity.Sharedstockbit == 1 ? null : entity.NurseStation_Id;
                        data.Sharedstockbit = entity.Sharedstockbit;
                        data.DrugName = entity.DrugName;
                        data.GPICode = entity.GPICode;
                        data.TrackableBit = entity.TrackableBit;
                        data.Stock_Status = entity.Stock_Status;
                        data.Stock_CreatedDate = entity.Stock_CreatedDate;
                        this.dbContext.SaveChanges();
                        if (entity.Stock_Status == 0)
                        {
                            InactiveToActiveStrockBarcode(entity.Stock_Id, entity.Stock_CreatedBy);

                        }
                    }
                    else
                        return -2;
                }
            }
            if (entity.Barcode.Length > 0 && entity.MergedStockIds.Length == 0)
            {
                for (int i = 0; i < entity.Barcode.Length; i++)
                {
                    string barcode = entity.Barcode[i];
                    var barcodeRecord = this.dbContext.BarcodeDetails.Where(br => br.Stock_Id == entity.Stock_Id && br.BarcodeDetail1 == barcode).FirstOrDefault();
                    if (barcodeRecord == null)
                    {
                        barcodeRecord = new BarcodeDetail();
                        barcodeRecord.PBarcode_Id = 0;
                        barcodeRecord.POrder_Id = null;
                        barcodeRecord.Stock_Id = entity.Stock_Id == 0 ? id : entity.Stock_Id;
                        barcodeRecord.Ekit_Id = null;
                        barcodeRecord.BarcodeDetail1 = entity.Barcode[i];
                        barcodeRecord.PBarcode_CreatedBy = entity.Stock_CreatedBy;
                        barcodeRecord.PBarcode_Status = (int)entity.Stock_Status;
                        barcodeRecord.PBarcode_CreatedDate = (DateTime)entity.Stock_CreatedDate;
                        this.dbContext.BarcodeDetails.Add(barcodeRecord);
                        this.dbContext.SaveChanges();
                    }
                    else
                    {
                        barcodeRecord = this.dbContext.BarcodeDetails.Where(br => br.Stock_Id == entity.Stock_Id && br.BarcodeDetail1 == barcode).FirstOrDefault();
                        barcodeRecord.POrder_Id = null;
                        barcodeRecord.Ekit_Id = null;
                        barcodeRecord.BarcodeDetail1 = entity.Barcode[i];
                        barcodeRecord.PBarcode_CreatedBy = entity.Stock_CreatedBy;
                        barcodeRecord.PBarcode_Status = (int)entity.Stock_Status;
                        barcodeRecord.PBarcode_CreatedDate = (DateTime)entity.Stock_CreatedDate;
                        this.dbContext.SaveChanges();


                        var items = stockbarcodes.Except(entity.Barcode);
                        foreach (var item in items)
                        {
                            BarcodeDetail list = this.dbContext.BarcodeDetails.Where(c => c.Stock_Id == entity.Stock_Id && c.BarcodeDetail1 == item).FirstOrDefault();
                            if (list != null)
                            {
                                this.dbContext.BarcodeDetails.Remove(list);
                                this.dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            else if (entity.Barcode.Length == 0 && entity.MergedStockIds.Length == 0)
            {
                foreach (var item in stockbarcodes)
                {
                    BarcodeDetail list = this.dbContext.BarcodeDetails.Where(c => c.Stock_Id == entity.Stock_Id && c.BarcodeDetail1 == item).FirstOrDefault();
                    if (list != null)
                    {
                        this.dbContext.BarcodeDetails.Remove(list);
                        this.dbContext.SaveChanges();
                    }
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Stock,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.Stock_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public StockGridEntity GetAllStock(StockEkitSearchCustomEntity obj)
        {
            var count = 0;
            var records = new List<StockEntity>();
            string searchText = obj.SearchText != null && obj.SearchText != string.Empty ? obj.SearchText.ToLower() : string.Empty;

            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == obj.UserId
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            // Common query to fetch stocks based on filters
            var baseQuery = from a in this.dbContext.Stocks
                            join user in this.dbContext.Users on a.Stock_CreatedBy equals user.User_Id
                            join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
                            from nur in ns.Where(n => stationIds.Contains((int)n.NurseStation_Id)).DefaultIfEmpty()
                            join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
                            join bc in this.dbContext.BarcodeDetails on a.Stock_Id equals bc.Stock_Id into barCodes
                            where a.Stock_Status == obj.Status && facilityIds.Contains(fc.Facility_Id)
                            select new
                            {
                                Stock = a,
                                Facility = fc,
                                NurseStation = nur,
                                BarcodeDetails = barCodes
                            };

            if (!string.IsNullOrEmpty(searchText))
            {
                baseQuery = baseQuery.Where(x => x.NurseStation.NurseStation_Name.ToLower().Contains(searchText)
                                                || x.Facility.Facility_Name.ToLower().Contains(searchText)
                                                || x.Stock.DrugName.ToLower().Contains(searchText)
                                                || x.Stock.InHand.ToLower().Contains(searchText)
                                                || x.Stock.GPICode.ToLower().Contains(searchText)
                                                || x.BarcodeDetails.Any(b => b.BarcodeDetail1.Contains(searchText)));
            }
            var allRecords = baseQuery
               .AsEnumerable() // Execute the query in memory to enable grouping logic
               .GroupBy(x => new
               {
                   x.Stock.GPICode,
                   GroupKey = x.Stock.NurseStation_Id != null && x.Stock.NurseStation_Id != 0
                       ? x.Stock.NurseStation_Id // Group by NursingStation_Id if present
                       : x.Stock.Facility_Id     // Otherwise group by Facility_Id
               })
               .Select(g => new
               {
                   GPI = g.Key.GPICode,
                   GroupKey = g.Key.GroupKey,
                   FirstAlphabeticalStock = g.OrderBy(x => x.Stock.DrugName).FirstOrDefault(),
                   //TotalInHand = g.Sum(x =>
                   //{
                   //    // Convert InHand to decimal for summation
                   //    decimal inHandValue;
                   //    return decimal.TryParse(x.Stock.InHand, out inHandValue) ? inHandValue : 0;
                   //}),
                   TotalInHand = g.Sum(x => ConvertStringToNumber(x.Stock.InHand)),

                   MergedBarcodes = string.Join(", ",
                g.SelectMany(x => x.BarcodeDetails
                    .Where(b => b.PBarcode_Status == 1) // Consider only active barcodes
                    .Select(b => b.BarcodeDetail1)
                )
                .Distinct()
                .Where(barcode => !string.IsNullOrEmpty(barcode)) // Optional: to filter out any null or empty barcodes
            ),
                   SecondaryDrugNames = string.Join(",", g
                    .Select(x => x.Stock.DrugName)
                    .OrderBy(drugName => drugName) // Ensure consistent ordering
                    .Skip(1)), // Exclude the first drug name (MainDrugName),
                   MergedStockIds = string.Join(",", g
                    .OrderBy(x => x.Stock.DrugName)
                    .Skip(1)
                    .Select(x => x.Stock.Stock_Id))

               }).ToList();

            var pagedRecords = allRecords
                .OrderBy(x => x.FirstAlphabeticalStock.Facility.Facility_Name)
                .ThenBy(x => x.FirstAlphabeticalStock.NurseStation?.NurseStation_Name)
                .ThenBy(x => x.FirstAlphabeticalStock.Stock.DrugName)
                .Skip((obj.CurrentPage - 1) * obj.PageSize)
                .Take(obj.PageSize)
                .Select(x => new StockEntity
                {
                    Stock_Id = x.FirstAlphabeticalStock.Stock.Stock_Id,
                    NurseStation_Id = x.FirstAlphabeticalStock.Stock.NurseStation_Id,
                    NurseStationName = x.FirstAlphabeticalStock.NurseStation != null ? x.FirstAlphabeticalStock.NurseStation.NurseStation_Name : "",
                    FacilityName = x.FirstAlphabeticalStock.Facility.Facility_Name,
                    DrugName = x.FirstAlphabeticalStock.Stock.DrugName,
                    MergedDrugNames = x.SecondaryDrugNames,
                    GPICode = x.FirstAlphabeticalStock.Stock.GPICode,
                    Trackable = x.FirstAlphabeticalStock.Stock.TrackableBit == 1 ? "Yes" : "No",
                    InHand = x.TotalInHand.ToString(),
                    Barcode = x.MergedBarcodes,
                    Stock_Status = x.FirstAlphabeticalStock.Stock.Stock_Status,
                    FacilityStatus = x.FirstAlphabeticalStock.Facility.Facility_Status,
                    NurseStatioStatus = x.FirstAlphabeticalStock.NurseStation != null ? x.FirstAlphabeticalStock.NurseStation.NurseStation_Status : 1,
                    Shared = x.FirstAlphabeticalStock.Stock.Sharedstockbit == 1 ? "Yes" : "No",
                    MergedStockIds = x.MergedStockIds
                })
                .ToList();

            var entity = new StockGridEntity
            {
                GridData = pagedRecords,
                TotalRecords = allRecords.Count
            };

            return entity;
        }
        public StockEntity GetStockById(int stockId)
        {

            var record = (from a in this.dbContext.Stocks
                          //join barcode in this.dbContext.BarcodeDetails on a.Stock_Id equals barcode.Stock_Id into bc
                          //from b in bc.DefaultIfEmpty()
                          //join user in this.dbContext.Users on a.Stock_CreatedBy equals user.User_Id
                          where a.Stock_Id == stockId
                          select new StockEntity
                          {
                              Stock_Id = a.Stock_Id,
                              NurseStation_Id = a.NurseStation_Id,
                              Facility_Id = a.Facility_Id,
                              DrugName = a.DrugName,
                              GPICode = a.GPICode,
                              TrackableBit = a.TrackableBit == null ? 0 : (int)a.TrackableBit,
                              InHand = a.InHand,
                              Barcode = "",
                              Stock_Status = a.Stock_Status,
                              Stock_CreatedBy = a.Stock_CreatedBy,
                              Stock_CreatedDate = a.Stock_CreatedDate,
                              //UserName = user.User_DisplayName,
                              SharedStockBit = a.Sharedstockbit == null ? 0 : (int)a.Sharedstockbit,
                          }).FirstOrDefault();
            var stock = this.dbContext.BarcodeDetails.Where(br => br.Stock_Id == stockId).Select(br => br.BarcodeDetail1).Distinct().ToArray();
            if (stock.Length > 0)
            {
                record.Barcode = string.Join(", ", stock);
            }
            else
            {
                record.Barcode = null;
            }
            return record;
        }
        public int InsertUpdateEkit(EkitCustomEntity entity)
        {
            entity.Ekit_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (entity.Facility_Id != 0 && (entity.NurseStation_Id != 0 && entity.NurseStation_Id !=null))
            {
                //var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == entity.NurseStation_Id).Select(n => n.Facility_Id).FirstOrDefault();
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity()
                    {
                        User_Id = userId,
                        Facility_Id = (int)entity.Facility_Id,
                        NurseStation_Id = entity.NurseStation_Id.ToString()
                    };
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            int id = 0;
            string[] ekitbarcodes = this.dbContext.BarcodeDetails.Where(c => c.Ekit_Id == entity.Ekit_Id).Select(c => c.BarcodeDetail1.ToString()).ToArray();
            var record = this.autoMapper.Map<EkitCustomEntity, Ekit>(entity);
            var ekit = this.dbContext.Ekits.Where(pt => pt.Ekit_Id == entity.Ekit_Id).FirstOrDefault();
            if (ekit == null)
            {
                if (entity.Sharedekitbit == 0 && entity.NurseStation_Id != 0)
                {
                    var exisitngRecordWithSameGPI = this.dbContext.Ekits.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.DrugName==entity.DrugName).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {
                        this.dbContext.Ekits.Add(record);
                        this.dbContext.SaveChanges();
                        id = record.Ekit_Id;
                    }
                    else
                        return -1;
                }
                else if (entity.Sharedekitbit == 1)
                {
                    var exisitngRecordWithSameGPI = this.dbContext.Ekits.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.DrugName == entity.DrugName).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {
                        this.dbContext.Ekits.Add(record);
                        this.dbContext.SaveChanges();
                        id = record.Ekit_Id;
                    }
                    else
                        return -2;
                }
            }
            else
            {
                if (entity.Sharedekitbit == 0 && entity.NurseStation_Id != 0)
                {
                    var exisitngRecordWithSameGPI = this.dbContext.Ekits.Where(n => n.NurseStation_Id == entity.NurseStation_Id && n.GPICode == entity.GPICode && n.Ekit_Id != entity.Ekit_Id && n.DrugName == entity.DrugName).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {

                        Ekit data = this.dbContext.Ekits.Find(record.Ekit_Id);
                        data.Facility_Id = entity.Facility_Id;
                        data.NurseStation_Id = entity.Sharedekitbit == 1 ? null : entity.NurseStation_Id;
                        data.Sharedekitbit = entity.Sharedekitbit;
                        data.DrugName = entity.DrugName;
                        data.GPICode = entity.GPICode;
                        data.TrackableBit = entity.TrackableBit;
                        data.InHand = entity.InHand;
                        data.LotNumber = entity.LotNumber;
                        data.NDC = entity.NDC;
                        data.ExpiryDate = entity.ExpiryDate;
                        data.ControlSubstance = entity.ControlSubstance;
                        data.Ekit_Status = entity.Ekit_Status;
                        data.Ekit_CreatedBy = entity.Ekit_CreatedBy;
                        data.Ekit_CreatedOn = entity.Ekit_CreatedOn;
                        this.dbContext.SaveChanges();
                    }
                    else
                        return -1;
                }
                else if (entity.Sharedekitbit == 1)
                {
                    var exisitngRecordWithSameGPI = this.dbContext.Ekits.Where(n => n.Facility_Id == entity.Facility_Id && n.GPICode == entity.GPICode && n.Ekit_Id != entity.Ekit_Id && n.DrugName == entity.DrugName).FirstOrDefault();
                    if (exisitngRecordWithSameGPI == null)
                    {
                        Ekit data = this.dbContext.Ekits.Find(record.Ekit_Id);
                        data.Facility_Id = entity.Facility_Id;
                        data.NurseStation_Id = entity.Sharedekitbit == 1 ? null : entity.NurseStation_Id;
                        data.Sharedekitbit = entity.Sharedekitbit;
                        data.DrugName = entity.DrugName;
                        data.GPICode = entity.GPICode;
                        data.TrackableBit = entity.TrackableBit;
                        data.InHand = entity.InHand;
                        data.LotNumber = entity.LotNumber;
                        data.NDC = entity.NDC;
                        data.ExpiryDate = entity.ExpiryDate;
                        data.ControlSubstance = entity.ControlSubstance;
                        data.Ekit_Status = entity.Ekit_Status;
                        data.Ekit_CreatedBy = entity.Ekit_CreatedBy;
                        data.Ekit_CreatedOn = entity.Ekit_CreatedOn;
                        this.dbContext.SaveChanges();
                    }
                    else
                        return -2;
                }
            }
            if (entity.Barcode.Length > 0)
            {
                for (int i = 0; i < entity.Barcode.Length; i++)
                {
                    string barcode = entity.Barcode[i];
                    var barcodeRecord = this.dbContext.BarcodeDetails.Where(br => br.Ekit_Id == entity.Ekit_Id && br.BarcodeDetail1 == barcode).FirstOrDefault();
                    if (barcodeRecord == null)
                    {
                        barcodeRecord = new BarcodeDetail();
                        barcodeRecord.PBarcode_Id = 0;
                        barcodeRecord.POrder_Id = null;
                        barcodeRecord.Stock_Id = null;
                        barcodeRecord.Ekit_Id = entity.Ekit_Id == 0 ? id : entity.Ekit_Id;
                        barcodeRecord.BarcodeDetail1 = entity.Barcode[i];
                        barcodeRecord.PBarcode_CreatedBy = entity.Ekit_CreatedBy;
                        barcodeRecord.PBarcode_Status = (int)entity.Ekit_Status;
                        barcodeRecord.PBarcode_CreatedDate = (DateTime)entity.Ekit_CreatedOn;
                        this.dbContext.BarcodeDetails.Add(barcodeRecord);
                        this.dbContext.SaveChanges();
                    }
                    else
                    {
                        barcodeRecord = this.dbContext.BarcodeDetails.Where(br => br.Ekit_Id == entity.Ekit_Id && br.BarcodeDetail1 == barcode).FirstOrDefault();
                        barcodeRecord.POrder_Id = null;
                        barcodeRecord.BarcodeDetail1 = entity.Barcode[i];
                        barcodeRecord.PBarcode_CreatedBy = entity.Ekit_CreatedBy;
                        barcodeRecord.PBarcode_Status = (int)entity.Ekit_Status;
                        barcodeRecord.PBarcode_CreatedDate = (DateTime)entity.Ekit_CreatedOn;
                        this.dbContext.SaveChanges();

                        var items = ekitbarcodes.Except(entity.Barcode);
                        foreach (var item in items)
                        {
                            BarcodeDetail list = this.dbContext.BarcodeDetails.Where(c => c.Ekit_Id == entity.Ekit_Id && c.BarcodeDetail1 == item).FirstOrDefault();
                            if (list != null)
                            {
                                this.dbContext.BarcodeDetails.Remove(list);
                                this.dbContext.SaveChanges();
                            }
                        }

                    }
                }
            }
            else if (entity.Barcode.Length == 0)
            {
                foreach (var item in ekitbarcodes)
                {
                    BarcodeDetail list = this.dbContext.BarcodeDetails.Where(c => c.Ekit_Id == entity.Ekit_Id && c.BarcodeDetail1 == item).FirstOrDefault();
                    if (list != null)
                    {
                        this.dbContext.BarcodeDetails.Remove(list);
                        this.dbContext.SaveChanges();
                    }
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Ekit,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.Ekit_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public static decimal ConvertStringToNumber(string input)
        {
            decimal number;
            // Check for null or whitespace and treat as invalid
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            // Attempt to parse as a number
            if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number))
            {
                return number; // Valid number
            }

            return 0; // Invalid number
        }
        //public EkitGridEntity GetAllEkit(StockEkitSearchCustomEntity obj)
        //{
        //    var count = 0;
        //    var records = new List<EkitEntity>();
        //    string searchText = obj.SearchText != null && obj.SearchText != string.Empty ? obj.SearchText.ToLower() : string.Empty;
        //    var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
        //                                    join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
        //                                    where us.User_Id == obj.UserId
        //                                    select new
        //                                    {
        //                                        NurseStationId = ns.NurseStation_Id,
        //                                        FacilityId = us.Facility_id
        //                                    }).Distinct().ToList();

        //    List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
        //    List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();
        //    if (searchText != string.Empty)
        //    {
        //        count = (from a in this.dbContext.Ekits
        //                 join user in this.dbContext.Users on a.Ekit_CreatedBy equals user.User_Id
        //                 join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
        //                 from nur in ns.Where(n => stationIds.Contains((int)n.NurseStation_Id)).DefaultIfEmpty()
        //                 join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
        //                 join bc in this.dbContext.BarcodeDetails on a.Ekit_Id equals bc.Ekit_Id into barCodes
        //                 where (nur.NurseStation_Name.ToLower().Contains(obj.SearchText) || fc.Facility_Name.ToLower().Contains(obj.SearchText) || a.DrugName.ToLower().Contains(obj.SearchText) || a.InHand.ToLower().Contains(obj.SearchText) || a.LotNumber.ToLower().Contains(obj.SearchText) || (a.ExpiryDate == null ? "" : EntityFunctions.TruncateTime(a.ExpiryDate).ToString()).Contains(obj.SearchText) || a.GPICode.ToLower().Contains(obj.SearchText) || barCodes.Any(b => b.BarcodeDetail1.Contains(obj.SearchText)))
        //                 && a.Ekit_Status == obj.Status && facilityIds.Contains(fc.Facility_Id)
        //                 select a.Ekit_Id).Distinct().Count();

        //        int skipRows = (obj.CurrentPage - 1) * obj.PageSize;
        //        records = (from a in this.dbContext.Ekits
        //                   join user in this.dbContext.Users on a.Ekit_CreatedBy equals user.User_Id
        //                   join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
        //                   from nur in ns.Where(n => stationIds.Contains((int)n.NurseStation_Id)).DefaultIfEmpty()
        //                   join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
        //                   join bc in this.dbContext.BarcodeDetails on a.Ekit_Id equals bc.Ekit_Id into barCodes
        //                   where (nur.NurseStation_Name.ToLower().Contains(obj.SearchText) || fc.Facility_Name.ToLower().Contains(obj.SearchText) || a.DrugName.ToLower().Contains(obj.SearchText) || a.InHand.ToLower().Contains(obj.SearchText) || a.LotNumber.ToLower().Contains(obj.SearchText) || (a.ExpiryDate == null ? "" : EntityFunctions.TruncateTime(a.ExpiryDate).ToString()).Contains(obj.SearchText) || a.GPICode.ToLower().Contains(obj.SearchText) || barCodes.Any(b => b.BarcodeDetail1.Contains(obj.SearchText)))
        //                   && a.Ekit_Status == obj.Status && facilityIds.Contains(fc.Facility_Id)
        //                   orderby fc.Facility_Name, nur.NurseStation_Name, a.DrugName
        //                   select new
        //                   {
        //                       Ekit = a,
        //                       Facility = fc,
        //                       NurseStation = nur,
        //                       //User = user,
        //                       BarcodeDetails = barCodes//.Select(br => br.BarcodeDetail1).ToList().Distinct(),
        //                   }).Skip(skipRows).Take(obj.PageSize).AsEnumerable()

        //                       .Select(x => new EkitEntity()
        //                       {
        //                           Ekit_Id = x.Ekit.Ekit_Id,
        //                           FacilityName = x.Facility.Facility_Name,
        //                           NurseStation_Id = x.Ekit.NurseStation_Id,
        //                           NurseStationName = x.NurseStation != null ? x.NurseStation.NurseStation_Name : "",
        //                           DrugName = x.Ekit.DrugName,
        //                           GPICode = x.Ekit.GPICode,
        //                           Trackable = x.Ekit.TrackableBit == 1 ? "Yes" : "No",
        //                           InHand =x.Ekit.ControlSubstance==1 && x.Ekit.CheckInFlag==1?"0": x.Ekit.InHand,
        //                           Barcode = x.BarcodeDetails.Count() != 0 ? string.Join(",", x.BarcodeDetails.Where(b => b.PBarcode_Status == 1).Select(b => b.BarcodeDetail1).Distinct()) : "",
        //                           LotNumber = x.Ekit.LotNumber,
        //                           ExpiryDate = x.Ekit.ExpiryDate,
        //                           Ekit_Status = x.Ekit.Ekit_Status,
        //                           //Ekit_CreatedBy = x.Ekit.Ekit_CreatedBy,
        //                           //Ekit_CreatedOn = x.Ekit.Ekit_CreatedOn,
        //                           //UserName = x.User.User_DisplayName,
        //                           FacilityStatus = x.Facility.Facility_Status,
        //                           NurseStatioStatus = x.NurseStation != null ? x.NurseStation.NurseStation_Status : 1,
        //                           Shared = x.Ekit.Sharedekitbit == 1 ? "Yes" : "No",
        //                           ControlSubstance= x.Ekit.ControlSubstance == 1 ? "Yes" : "No"
        //                       }).ToList();
        //    }
        //    else
        //    {

        //        count = (from a in this.dbContext.Ekits
        //                 join user in this.dbContext.Users on a.Ekit_CreatedBy equals user.User_Id
        //                 join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
        //                 from nur in ns.Where(n => stationIds.Contains((int)n.NurseStation_Id)).DefaultIfEmpty()
        //                 join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
        //                 where a.Ekit_Status == obj.Status && facilityIds.Contains(fc.Facility_Id)
        //                 select a.Ekit_Id).Distinct().Count();

        //        int skipRows = (obj.CurrentPage - 1) * obj.PageSize;
        //        records = (from a in this.dbContext.Ekits
        //                   join user in this.dbContext.Users on a.Ekit_CreatedBy equals user.User_Id
        //                   join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
        //                   from nur in ns.Where(n => stationIds.Contains((int)n.NurseStation_Id)).DefaultIfEmpty()
        //                   join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
        //                   join bc in this.dbContext.BarcodeDetails on a.Ekit_Id equals bc.Ekit_Id into barCodes
        //                   where a.Ekit_Status == obj.Status && facilityIds.Contains(fc.Facility_Id)
        //                   orderby fc.Facility_Name, nur.NurseStation_Name, a.DrugName
        //                   select new
        //                   {
        //                       Ekit = a,
        //                       Facility = fc,
        //                       NurseStation = nur,
        //                       //User = user,
        //                       BarcodeDetails = barCodes//.Select(br => br.BarcodeDetail1).ToList().Distinct(),
        //                   }).Skip(skipRows).Take(obj.PageSize).AsEnumerable()
        //                       .Select(x => new EkitEntity()
        //                       {
        //                           Ekit_Id = x.Ekit.Ekit_Id,
        //                           FacilityName = x.Facility.Facility_Name,
        //                           NurseStation_Id = x.Ekit.NurseStation_Id,
        //                           NurseStationName = x.NurseStation!=null? x.NurseStation.NurseStation_Name:"",
        //                           DrugName = x.Ekit.DrugName,
        //                           GPICode = x.Ekit.GPICode,
        //                           Trackable = x.Ekit.TrackableBit == 1 ? "Yes" : "No",
        //                           InHand = x.Ekit.ControlSubstance == 1 && x.Ekit.CheckInFlag == 1 ? "0" : x.Ekit.InHand,
        //                           Barcode = x.BarcodeDetails.Count() != 0 ? string.Join(",", x.BarcodeDetails.Where(b => b.PBarcode_Status == 1).Select(b => b.BarcodeDetail1).Distinct()) : "",
        //                           LotNumber = x.Ekit.LotNumber,
        //                           ExpiryDate = x.Ekit.ExpiryDate,
        //                           Ekit_Status = x.Ekit.Ekit_Status,
        //                           //Ekit_CreatedBy = x.Ekit.Ekit_CreatedBy,
        //                           //Ekit_CreatedOn = x.Ekit.Ekit_CreatedOn,
        //                           //UserName = x.User.User_DisplayName,
        //                           FacilityStatus = x.Facility.Facility_Status,
        //                           NurseStatioStatus = x.NurseStation != null ? x.NurseStation.NurseStation_Status : 1,
        //                           Shared = x.Ekit.Sharedekitbit == 1 ? "Yes" : "No",
        //                           ControlSubstance = x.Ekit.ControlSubstance == 1 ? "Yes" : "No"
        //                       }).ToList();
        //    }
        //    EkitGridEntity entity = new EkitGridEntity();
        //    entity.GridData = records;
        //    entity.TotalRecords = count;
        //    return entity;
        //}
        public EkitGridEntity GetAllEkit(StockEkitSearchCustomEntity obj)
        {
            var count = 0;
            var records = new List<EkitEntity>();
            string searchText = obj.SearchText != null && obj.SearchText != string.Empty ? obj.SearchText.ToLower() : string.Empty;

            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == obj.UserId
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();

            var todayDate = DateTime.Now;
            todayDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var baseQuery = from a in this.dbContext.Ekits
                            join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
                            from nur in ns.Where(n => stationIds.Contains((int)n.NurseStation_Id)).DefaultIfEmpty()
                            join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
                            where a.Ekit_Status == obj.Status && (a.ExpiryDate == null || a.ExpiryDate >= todayDate.Date) && facilityIds.Contains(fc.Facility_Id)
                            select new
                            {
                                Ekit = a,
                                Facility = fc,
                                NurseStation = nur
                            };

            if (!string.IsNullOrEmpty(searchText))
            {
                baseQuery = baseQuery.Where(x => x.NurseStation.NurseStation_Name.ToLower().Contains(searchText)
                                                || x.Facility.Facility_Name.ToLower().Contains(searchText)
                                                || x.Ekit.DrugName.ToLower().Contains(searchText)
                                                || x.Ekit.InHand.ToLower().Contains(searchText)
                                                || x.Ekit.GPICode.ToLower().Contains(searchText));
            }

            
            var groupedRecords = baseQuery
    .AsEnumerable()
    .GroupBy(x => new
    {
        x.Ekit.GPICode,
        GroupKey = x.Ekit.NurseStation_Id != null && x.Ekit.NurseStation_Id != 0
            ? x.Ekit.NurseStation_Id // Group by NursingStation_Id if present
            : x.Ekit.Facility_Id     // Otherwise group by Facility_Id
    })
    .Select(g => new
    {
        GPI = g.Key,
        FirstAlphabeticalEkit = g.OrderBy(x => x.Ekit.DrugName).FirstOrDefault(),

        TotalInHand = Math.Max(
            g.Sum(x =>
            {
                decimal inHandValue = 0;
                decimal.TryParse(x.Ekit.InHand, out inHandValue); // Parse to decimal
                return inHandValue;
            }),
            0
        ),
        SecondaryDrugNames = string.Join(",", g
            .Select(x => x.Ekit.DrugName)
            .Distinct()
            .OrderBy(drugName => drugName) // Ensure consistent ordering
            .Skip(1)), // Exclude the first drug name (MainDrugName)
    })
    .ToList();

            count = groupedRecords.Count;


            // Pagination
            var pagedRecords = groupedRecords
                .OrderBy(x => x.FirstAlphabeticalEkit.Facility.Facility_Name)
                .ThenBy(x => x.FirstAlphabeticalEkit.NurseStation?.NurseStation_Name)
                .ThenBy(x => x.FirstAlphabeticalEkit.Ekit.DrugName)
                .Skip((obj.CurrentPage - 1) * obj.PageSize)
                .Take(obj.PageSize)
                .Select(x => new EkitEntity
                {
                    Ekit_Id = x.FirstAlphabeticalEkit.Ekit.Ekit_Id,
                    FacilityName = x.FirstAlphabeticalEkit.Facility.Facility_Name,
                    NurseStation_Id = x.FirstAlphabeticalEkit.Ekit.NurseStation_Id,
                    NurseStationName = x.FirstAlphabeticalEkit.NurseStation != null ? x.FirstAlphabeticalEkit.NurseStation.NurseStation_Name : "",
                    DrugName = x.FirstAlphabeticalEkit.Ekit.DrugName,
                    GPICode = x.FirstAlphabeticalEkit.Ekit.GPICode,
                    Trackable = x.FirstAlphabeticalEkit.Ekit.TrackableBit == 1 ? "Yes" : "No",
                    InHand = x.TotalInHand.ToString(),
                    LotNumber = x.FirstAlphabeticalEkit.Ekit.LotNumber,
                    ExpiryDate = x.FirstAlphabeticalEkit.Ekit.ExpiryDate,
                    Ekit_Status = x.FirstAlphabeticalEkit.Ekit.Ekit_Status,
                    FacilityStatus = x.FirstAlphabeticalEkit.Facility.Facility_Status,
                    NurseStatioStatus = x.FirstAlphabeticalEkit.NurseStation != null ? x.FirstAlphabeticalEkit.NurseStation.NurseStation_Status : 1,
                    Shared = x.FirstAlphabeticalEkit.Ekit.Sharedekitbit == 1 ? "Yes" : "No",
                    ControlSubstance = x.FirstAlphabeticalEkit.Ekit.ControlSubstance == 1 ? "Yes" : "No",
                    MergedDrugNames = x.SecondaryDrugNames,

                })
                .ToList();

            EkitGridEntity entity = new EkitGridEntity
            {
                GridData = pagedRecords,
                TotalRecords = count
            };

            return entity;
        }

        public EkitEntity GetEkitById(int ekitId)
        {

            var record = (from a in this.dbContext.Ekits
                          //join fc in this.dbContext.NursingStations on a.NurseStation_Id equals fc.NurseStation_Id
                          //join barcode in this.dbContext.BarcodeDetails on a.Ekit_Id equals barcode.Ekit_Id into bc
                          //from b in bc.DefaultIfEmpty()
                          //join user in this.dbContext.Users on a.Ekit_CreatedBy equals user.User_Id
                          where a.Ekit_Id == ekitId
                          select new EkitEntity
                          {
                              Ekit_Id = a.Ekit_Id,
                              NurseStation_Id = a.NurseStation_Id,
                              Facility_Id = a.Facility_Id,
                              DrugName = a.DrugName,
                              GPICode = a.GPICode,
                              TrackableBit = a.TrackableBit == null ? 0 : (int)a.TrackableBit,
                              InHand = a.ControlSubstance == 1 && a.CheckInFlag == 1 ? "0" : a.InHand,
                              Barcode = "",
                              LotNumber = a.LotNumber,
                              NDC = a.NDC,
                              ExpiryDate = a.ExpiryDate,
                              Ekit_Status = a.Ekit_Status,
                              Ekit_CreatedBy = a.Ekit_CreatedBy,
                              Ekit_CreatedOn = a.Ekit_CreatedOn,
                              //UserName = user.User_DisplayName,
                              SharedeKitBit = a.Sharedekitbit == null ? 0 : (int)a.Sharedekitbit,
                              ControlSubstanceBit = a.ControlSubstance==null?0: (int)a.ControlSubstance
                          }).FirstOrDefault();
            var stock = this.dbContext.BarcodeDetails.Where(br => br.Ekit_Id == ekitId).Select(br => br.BarcodeDetail1).Distinct().ToArray();
            if (stock.Length > 0)
            {
                record.Barcode = string.Join(", ", stock);
            }
            else
            {
                record.Barcode = null;
            }
            return record;
        }
        public List<DrugInfo> GetGenericName(string searchPattern)
        {
            // return this.dbContext.Drugs.Where(d => d.GenericName.Contains(searchPattern) && d.Drug_Status == 1).Select(d => d.GenericName).ToList();
            //return generic;
            //var generic = (from dru in this.dbContext.Drugs
            //               where dru.GenericName.Contains(searchPattern)
            //               select new
            //               {
            //                   GenericName = dru.GenericName,
            //               }).ToList();
            //return generic;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))

            using (SqlCommand cmd = new SqlCommand("[Admin].[prc_GetDrugsNamesLike]", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 360;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@drugname", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@drugname"].Value = searchPattern;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                List<DrugInfo> drugList = new List<DrugInfo>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DrugInfo drugInfo = new DrugInfo
                    {
                        DrugName = Convert.ToString(ds.Tables[0].Rows[i]["drugname"]),
                        GPI = Convert.ToString(ds.Tables[0].Rows[i]["gpi"])
                    };

                    drugList.Add(drugInfo);
                }

                return drugList;

            }
        }
        public int UpdateEkitsStatus(List<EkitEntity> data)
        {
            foreach (var item in data)
            {
                var record = this.dbContext.Ekits.Find(item.Ekit_Id);
                if (record != null)
                {
                    record.Ekit_Status = record.Ekit_Status == 1 ? 0 : 1;
                    record.Ekit_CreatedBy = item.Ekit_CreatedBy;
                    record.Ekit_CreatedOn = item.Ekit_CreatedOn;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int UpdateStocksStatus(List<StockEntity> data)
        {
            foreach (var item in data)
            {
                var record = this.dbContext.Stocks.Find(item.Stock_Id);
                if (record != null)
                {
                    record.Stock_Status = record.Stock_Status == 1 ? 0 : 1;
                    record.Stock_CreatedBy = item.Stock_CreatedBy;
                    record.Stock_CreatedDate = item.Stock_CreatedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public List<StockCustomEntity> GetAllStockGPICodes()
        {
            var records = (from st in this.dbContext.Stocks
                           select new StockCustomEntity
                           {
                               Stock_Id = st.Stock_Id,
                               GPICode = st.GPICode,

                           }).ToList();
            return records;
        }
        public List<EkitCustomEntity> GetAllEkitGPICodes()
        {
            var records = (from ek in this.dbContext.Ekits
                           select new EkitCustomEntity
                           {
                               Ekit_Id = ek.Ekit_Id,
                               GPICode = ek.GPICode,

                           }).ToList();
            return records;
        }
        public int InsertStockEkitClone(StockEkitCloneEntity stockClone)
        {
            var result = this.dbContext.PrcInsertStockEkitClone(stockClone.Input, stockClone.clone, stockClone.sharedstockbit, stockClone.stockekit, stockClone.createdBy);
            return 1;
        }
        public int IsDataAvailableToCloneStock(int facilityId, int nsId)
        {
            int count = 0;
            if (facilityId != 0)
            {
                count = this.dbContext.Stocks.Where(s => s.Facility_Id == facilityId && s.NurseStation_Id == null && s.Sharedstockbit == 1).Count();
            }
            else if (nsId != 0)
            {
                count = this.dbContext.Stocks.Where(s => s.NurseStation_Id == nsId && (s.Sharedstockbit == 0 || s.Sharedstockbit == null)).Count();
            }
            return count;
        }
        public int IsDataAvailableToCloneEkit(int facilityId, int nsId)
        {
            int count = 0;
            if (facilityId != 0)
            {
                count = this.dbContext.Ekits.Where(s => s.Facility_Id == facilityId && s.NurseStation_Id == null && s.Sharedekitbit == 1).Count();
            }
            else if (nsId != 0)
            {
                count = this.dbContext.Ekits.Where(s => s.NurseStation_Id == nsId && (s.Sharedekitbit == 0 || s.Sharedekitbit==null)).Count();
            }
            return count;
        }
        public int CheckAutoBarcodeAlert(string BarcodeData)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_AutobarcodeCheck]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = BarcodeData == null ? (object)DBNull.Value : BarcodeData;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return 1;
        }
        public List<stockGpiInfo> StockGPIAlert(int facilityId, int nsId, string gpiCode)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_StockGPIAlert]";
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
                    cmd.Parameters.Add("@nursingstation_id", SqlDbType.Int).Value = (nsId == null || nsId == 0) ? (object)DBNull.Value : nsId;
                    cmd.Parameters.Add("@gpicode", SqlDbType.VarChar).Value = gpiCode;



                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var result = (from d in dt.AsEnumerable()
                          select new stockGpiInfo
                          {
                              DrugName = d["drugname"].ToString(),
                              StockId = Convert.ToInt32(d["Stock_Id"])
                          }).ToList();

            return result;
        }
        public int UpdateStockQtyGpi(StockUpdateEntity stockUpdate)
        {
            string barcodes = string.Join(", ", stockUpdate.Barcode);

            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_UpdateStockQtyGpi]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;

                    cmd.Parameters.Add("@stock_id", SqlDbType.Int).Value = stockUpdate.StockId;
                    cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = stockUpdate.InHand;
                    cmd.Parameters.Add("@barcodes", SqlDbType.VarChar).Value = barcodes;
                    cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = stockUpdate.StockCreatedBy;
                    cmd.Parameters.Add("@createddate", SqlDbType.DateTime).Value = stockUpdate.StockCreatedDate;




                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return 1;
        }
        public int InactiveToActiveStrockBarcode(int stockid, int? createdBy)
        {

            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_InactiveToActiveStrockBarcode]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;

                    cmd.Parameters.Add("@stockids", SqlDbType.Int).Value = stockid.ToString();
                    cmd.Parameters.Add("@createdby", SqlDbType.VarChar).Value = createdBy;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return 1;
        }
        public int UpdatedSecondaryStockDetails(int stockid, string secondaryIds, string[] Barcodes, string inhand, int? createdBy, DateTime createdDate)
        {
            string barcodes = string.Join(",", Barcodes);
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_UpdateStockDetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;

                    cmd.Parameters.Add("@PriStockid", SqlDbType.Int).Value = stockid;
                    cmd.Parameters.Add("@SecStockids", SqlDbType.VarChar).Value = secondaryIds;
                    cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = inhand;
                    cmd.Parameters.Add("@barcodes", SqlDbType.VarChar).Value = barcodes;
                    cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = createdBy;
                    cmd.Parameters.Add("@createddate", SqlDbType.DateTime).Value = createdDate;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return 1;
        }
    }
}
