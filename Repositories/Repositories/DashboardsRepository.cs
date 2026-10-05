using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace LTCPro.Repositories
{
    public class DashboardsRepository : IDashboardsRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly ICommonRepository _commonRepository;
        public DashboardsRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, CommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._commonRepository = commonRepository;
        }
        //public DashBoardEntity GetCensusDashboard(string moduleName, string seriesName, string yAxis, string category, int userId, string fromDate, string toDate, int filterType, int reportType, string nurseStations)
        //{
        //    if (moduleName.Contains("vitals"))
        //        return GetVitalDashboard(moduleName, category, seriesName);
        //    else if (moduleName.Contains("census"))
        //        return GetCensusDashboard(moduleName, seriesName, yAxis, category, userId, fromDate, toDate, filterType, reportType, nurseStations);
        //    return null;
        //}
        private DashBoardEntity GetVitalDashboard(string moduleName, string category, string seriesName)
        {
            DashBoardEntity entity = new DashBoardEntity();

            if (moduleName == "vitals" && category == string.Empty && seriesName == string.Empty)
            {
                var records = this.dbContext.PrcDashboardVitalsdata().ToList();
                List<int> lowStatus = new List<int>();
                lowStatus.Add(records.Where(e => e.BloodPressure == "Low").Count());
                lowStatus.Add(records.Where(e => e.BloodSugarLevels == "Low").Count());
                lowStatus.Add(records.Where(e => e.HeartRate == "Low").Count());
                lowStatus.Add(records.Where(e => e.PulseRate == "Low").Count());
                lowStatus.Add(records.Where(e => e.RespiratoryRate == "Low").Count());
                lowStatus.Add(records.Where(e => e.Temperature == "Low").Count());

                List<int> normalStatus = new List<int>();
                normalStatus.Add(records.Where(e => e.BloodPressure == "Normal").Count());
                normalStatus.Add(records.Where(e => e.BloodSugarLevels == "Normal").Count());
                normalStatus.Add(records.Where(e => e.HeartRate == "Normal").Count());
                normalStatus.Add(records.Where(e => e.PulseRate == "Normal").Count());
                normalStatus.Add(records.Where(e => e.RespiratoryRate == "Normal").Count());
                normalStatus.Add(records.Where(e => e.Temperature == "Normal").Count());

                List<int> highStatus = new List<int>();
                highStatus.Add(records.Where(e => e.BloodPressure == "High").Count());
                highStatus.Add(records.Where(e => e.BloodSugarLevels == "High").Count());
                highStatus.Add(records.Where(e => e.HeartRate == "High").Count());
                highStatus.Add(records.Where(e => e.PulseRate == "High").Count());
                highStatus.Add(records.Where(e => e.RespiratoryRate == "High").Count());
                highStatus.Add(records.Where(e => e.Temperature == "High").Count());

                var data = new List<SeriesDataEntity>() { new SeriesDataEntity { name = "High", data = highStatus }, new SeriesDataEntity { name = "Normal", data = normalStatus }, new SeriesDataEntity { name = "Low", data = lowStatus } };

                entity.ColumnNames = new List<DashboardColumnsEntity>() { new DashboardColumnsEntity() {name= "Patient_Id", displayName = "Patient Id", display=false },
                                        new DashboardColumnsEntity() {name= "ResidentName", displayName = "Resident Name", display=true },
                                        new  DashboardColumnsEntity(){name= "Vitals_ID",displayName = "Vitals ID", display=true },
                                        new  DashboardColumnsEntity(){name = "vitaldate", displayName="Vital Date", display=true },
                                        new DashboardColumnsEntity() {name = "VitalTime", displayName="Vital Time", display=false },
                                        new DashboardColumnsEntity() {name = "BloodPressure", displayName="Blood Pressure", display=true },
                                        new DashboardColumnsEntity() {name = "BloodSugarLevels", displayName="Blood Sugar", display=true },
                                        new DashboardColumnsEntity() {name = "HeartRate", displayName="Heart Rate", display=true },
                                        new DashboardColumnsEntity() {name = "RespiratoryRate", displayName="Respiratory Rate", display=true },
                                        new DashboardColumnsEntity() {name = "Temperature", displayName="Temperature", display=true },
                                        new DashboardColumnsEntity() {name = "PulseRate", displayName="Pulse Rate", display=true }
            };
                entity.YaxisData = data;
                entity.XaxisData = new List<string>() { "BP", "Blood Sugar", "Heart Rate", "Pulse Rate", "Respiratory Rate", "Temperature" };
                entity.GridData = records;
            }
            else if (moduleName == "vitals1" && category != string.Empty && seriesName != string.Empty)
            {
                var records = this.dbContext.PrcDashboardVitalsdata().ToList();
                string columnName = string.Empty;
                switch (seriesName)
                {
                    case "BP":
                        columnName = "BloodPressure";
                        break;
                    case "Blood Sugar":
                        columnName = "BloodSugarLevels";
                        break;
                    case "Heart Rate":
                        columnName = "HeartRate";
                        break;
                    case "Pulse Rate":
                        columnName = "PulseRate";
                        break;
                    case "Respiratory Rate":
                        columnName = "RespiratoryRate";
                        break;
                    case "Temperature":
                        columnName = "Temperature";
                        break;
                }
                var gridData = records.Select(e => new
                {
                    Patient_Id = e.Patient_Id,
                    ResidentName = e.ResidentName,
                    Vitals_ID = e.Vitals_ID,
                    vitaldate = e.vitaldate,
                    VitalTime = e.VitalTime,
                    VitalType = e.GetType().GetProperty(columnName).GetValue(e)
                }).ToList();

                var data = records.Where(e => e.GetType().GetProperty(columnName).GetValue(e).ToString() == category)
                    .GroupBy(e => new { e.Patient_Id, e.GetType().GetProperty(columnName).Name })
                    .Select(e => e.Select(m => m.GetType().GetProperty(columnName)).Count()).ToList();
                var ydata = new List<SeriesDataEntity>() { new SeriesDataEntity { name = category, data = data } };

                entity.ColumnNames = new List<DashboardColumnsEntity>() { new DashboardColumnsEntity() {name= "Patient_Id", displayName = "Patient Id", display=false },
                                        new  DashboardColumnsEntity() {name= "ResidentName", displayName = "Resident Name", display=true },
                                        new DashboardColumnsEntity() {name= "Vitals_ID",displayName = "Vitals ID", display=false },
                                        new DashboardColumnsEntity() {name = "vitaldate", displayName="Vital Date", display=true },
                                        new DashboardColumnsEntity() {name = "VitalTime", displayName="Vital Time", display=false },
                                        new DashboardColumnsEntity() {name = "VitalType", displayName=seriesName, display=true }
                                        //new {name = "BloodPressure", displayName="Blood Pressure", display=true },
                                        //new {name = "BloodSugarLevels", displayName="Blood Sugar", display=true },
                                        //new {name = "HeartRate", displayName="Heart Rate", display=true },
                                        //new {name = "RespiratoryRate", displayName="Respiratory Rate", display=true },
                                        //new {name = "Temperature", displayName="Temperature", display=true },
                                        //new {name = "PulseRate", displayName="Pulse Rate", display=true }
            };
                entity.YaxisData = ydata;
                entity.XaxisData = records.Select(e => e.ResidentName).Distinct().ToList();
                entity.GridData = gridData;
            }
            else if (moduleName == "vitals2" && category != string.Empty && seriesName != string.Empty)
            {
                var records = this.dbContext.PrcDashboardVitalsdata().ToList();
                string columnName = string.Empty;
                switch (seriesName)
                {
                    case "BP":
                        columnName = "BloodPressure";
                        break;
                    case "Blood Sugar":
                        columnName = "BloodSugarLevels";
                        break;
                    case "Heart Rate":
                        columnName = "HeartRate";
                        break;
                    case "Pulse Rate":
                        columnName = "PulseRate";
                        break;
                    case "Respiratory Rate":
                        columnName = "RespiratoryRate";
                        break;
                    case "Temperature":
                        columnName = "Temperature";
                        break;
                }
                var gridData = records.Select(e => new
                {
                    Patient_Id = e.Patient_Id,
                    ResidentName = e.ResidentName,
                    Vitals_ID = e.Vitals_ID,
                    vitaldate = e.vitaldate,
                    VitalTime = e.VitalTime,
                    VitalType = e.GetType().GetProperty(columnName).GetValue(e)
                }).ToList();

                var data = records.Where(e => e.GetType().GetProperty(columnName).GetValue(e).ToString() == category)
                    .GroupBy(e => new { e.Patient_Id, e.GetType().GetProperty(columnName).Name })
                    .Select(e => e.Select(m => m.GetType().GetProperty(columnName)).Count()).ToList();
                var ydata = new List<SeriesDataEntity>() { new SeriesDataEntity { name = category, data = data } };

                entity.ColumnNames = new List<DashboardColumnsEntity>() { new DashboardColumnsEntity() {name= "Patient_Id", displayName = "Patient Id", display=false },
                                        new DashboardColumnsEntity() {name= "ResidentName", displayName = "Resident Name", display=true },
                                        new DashboardColumnsEntity() {name= "Vitals_ID",displayName = "Vitals ID", display=false },
                                        new DashboardColumnsEntity() {name = "vitaldate", displayName="Vital Date", display=true },
                                        new DashboardColumnsEntity() {name = "VitalTime", displayName="Vital Time", display=false },
                                        new DashboardColumnsEntity() {name = "VitalType", displayName=seriesName, display=true }
                                        //new {name = "BloodPressure", displayName="Blood Pressure", display=true },
                                        //new {name = "BloodSugarLevels", displayName="Blood Sugar", display=true },
                                        //new {name = "HeartRate", displayName="Heart Rate", display=true },
                                        //new {name = "RespiratoryRate", displayName="Respiratory Rate", display=true },
                                        //new {name = "Temperature", displayName="Temperature", display=true },
                                        //new {name = "PulseRate", displayName="Pulse Rate", display=true }
            };
                entity.YaxisData = ydata;
                entity.XaxisData = records.Select(e => e.ResidentName).Distinct().ToList();
                entity.GridData = gridData;
            }

            return entity;
        }


        public DashBoardEntity GetCensusDashboard(string moduleName, string seriesName, string yAxis, string category, int userId, string fromDate, string toDate, int filterType, int reportType, string nurseStations)
        {
            DateTime fromdate = Convert.ToDateTime(fromDate);
            DateTime todate = Convert.ToDateTime(toDate);
            if (nurseStations == "")
                nurseStations = null;
            DashBoardEntity entity = new DashBoardEntity();
            if (moduleName == "census2" && (filterType == 2 || filterType == 3))
            {
                moduleName = "census1";
                filterType = 1;
            }
            if (moduleName == "census")
            {
                if (seriesName == "date" && yAxis == "count" && category == "nursestation")
                {
                    var records = (from cd in this.dbContext.PrcgetReportsCensusDatainfo(fromdate, todate, userId, nurseStations, filterType, reportType)
                                   select new
                                   {
                                       Count = (int)cd.Count,
                                       FromDate = cd.FromDate,
                                       NurseStation_Name = cd.NurseStation_Name,
                                       Month = (int)cd.Month,
                                       Year = (int)cd.Year

                                   }).OrderBy(e => e.Year).ThenBy(e => e.Month).ToList();
                    List<string> admitDates = new List<string>();
                    if (filterType == 3)
                        admitDates = records.OrderBy(e => e.FromDate).Select(e => e.FromDate).Distinct().ToList();
                    else if (filterType == 2)
                        admitDates = records.OrderBy(e => e.Year).ThenBy(e => e.Month).Select(e => e.FromDate).Distinct().ToList();
                    else if (filterType == 1)
                        admitDates = records.OrderBy(e => e.Year).ThenBy(e => e.Month).Select(e => e.FromDate).Distinct().ToList();

                    var nstations = records.Select(e => e.NurseStation_Name).Distinct().ToList();

                    var data = new List<SeriesDataEntity>();
                    foreach (var ns in nstations)
                    {
                        //var grpByData = records.Where(e => e.NurseStation_Name == ns).GroupBy(e => new { e.FromDate, e.Count }).ToList();
                        // .Select(e => new { count = e.Key.Count, val = e.Key.FromDate).ToList();
                        //data.Add(new { name = ns, data = grpByData.Select(e => e.Key.Count) });
                        List<int> census = new List<int>();
                        var nsRecords = records.Where(e => e.NurseStation_Name == ns).ToList();
                        foreach (var date in admitDates)
                        {
                            census.Add(nsRecords.Where(e => e.FromDate == date).Select(e => e.Count).SingleOrDefault());
                        }
                        data.Add(new SeriesDataEntity { name = ns, data = census });
                    }

                    var columnNames = new List<DashboardColumnsEntity>();

                    var columns = new DashboardColumnsEntity() { name = "name", displayName = "Nursing Station Name", display = true, alignType = "left", columnType = "static" };
                    columnNames.Add(columns);
                    //columns = new DashboardColumnsEntity() { name = "PatientName", displayName = "Resident Name", display = false, alignType = "left", columnType = "static" };
                    //columnNames.Add(columns);

                    foreach (var date in admitDates)
                    {
                        columns = new DashboardColumnsEntity()
                        {
                            name = date,
                            displayName = date,
                            display = true,
                            alignType = "right",
                            columnType = "dynamic"
                        };
                        columnNames.Add(columns);
                    }

                    entity.ColumnNames = columnNames;
                    entity.YaxisData = data;
                    entity.XaxisData = admitDates;
                    entity.GridData = data;
                }
                //else if (seriesName == "nursestation" && yAxis == "count" && category == "date")
                //{
                //    var records = this.dbContext.PrcgetReportsCensusDatainfo(null, null, null, 1, null, null, null, null, null).ToList();
                //    var admitDates = records.Select(e => e.AdmitDate).Distinct().ToList();
                //    var nstations = records.Select(e => e.NurseStation_Name).Distinct().ToList();
                //    var data = new List<SeriesDataEntity>();
                //    foreach (var date in admitDates)
                //    {
                //        List<int> grpByData = records.Where(e => e.AdmitDate == date).GroupBy(e => new { e.NurseStation_Name, e.Census })
                //       .Select(e => (int)e.Key.Census).ToList();
                //        data.Add(new SeriesDataEntity { name = Convert.ToDateTime(date).ToString("MM-dd-yyyy"), data = grpByData });
                //    }

                //    entity.ColumnNames = new List<DashboardColumnsEntity>() {
                //                        new DashboardColumnsEntity() {name= "Resident_Name", displayName = "Resident Name", display=true },
                //                        new DashboardColumnsEntity() {name= "AdmitDate",displayName = "Admit Date", display=true },
                //                        new DashboardColumnsEntity() {name = "Census", displayName="Census", display=true },
                //                        new DashboardColumnsEntity() {name = "NurseStation_Name", displayName="NurseStation Name", display=false },
                //                        new DashboardColumnsEntity() {name = "Floor_Name", displayName="Floor Name", display=true },
                //                        new DashboardColumnsEntity() {name = "Bed_Name", displayName="Bed Name", display=true },
                //                        new DashboardColumnsEntity() {name = "Wing_Desc", displayName="Wing Description", display=true }
                //    };
                //    entity.YaxisData = data;
                //    entity.XaxisData = nstations;
                //    entity.GridData = records;
                //}
            }
            else if (moduleName == "census1" && category != string.Empty && seriesName != string.Empty)
            {

                //ca NS se date
                if (filterType == 1)
                {
                    var records = (from cd in this.dbContext.PrcgetReportsCensusDetail(fromdate, todate, userId, seriesName, filterType, reportType, category)
                                   select new
                                   {
                                       Count = (int)cd.Count,
                                       FromDate = cd.FromDate,
                                       NurseStation_Name = cd.NurseStation_Name,
                                       PatientName = cd.PatientName
                                   }).ToList();
                    var admitDates = records.Where(e => e.NurseStation_Name == seriesName).Select(e => e.FromDate).Distinct().ToList();

                    var data = new List<SeriesDataEntity>();

                    var residents = records.Where(e => e.NurseStation_Name == seriesName).Select(e => e.PatientName).Distinct().ToList();
                    List<int> census = new List<int>();
                    var nstations = records.Select(e => e.NurseStation_Name).Distinct().ToList();
                    var nsRecords = records.Where(e => e.NurseStation_Name == seriesName).ToList();
                    foreach (var item in residents)
                    {
                        census = new List<int>();
                        foreach (var date in admitDates)
                        {
                            census.Add(nsRecords.Where(e => e.PatientName == item && e.FromDate == date).Select(e => e.Count).SingleOrDefault());
                        }
                        data.Add(new SeriesDataEntity { name = item, data = census, nsname = seriesName });

                    }

                    var columnNames = new List<DashboardColumnsEntity>();
                    var columns = new DashboardColumnsEntity() { name = "nsname", displayName = "Nursing Station Name", display = true, alignType = "left", columnType = "static" };
                    columnNames.Add(columns);
                    columns = new DashboardColumnsEntity() { name = "name", displayName = "Resident Name", display = true, alignType = "left", columnType = "static" };
                    columnNames.Add(columns);
                    foreach (var date in admitDates)
                    {
                        columns = new DashboardColumnsEntity()
                        {
                            name = date,
                            displayName = date,
                            display = true,
                            alignType = "right",
                            columnType = "dynamic"
                        };
                        columnNames.Add(columns);
                    }
                    entity.ColumnNames = columnNames;
                    entity.YaxisData = data;
                    entity.XaxisData = admitDates;
                    entity.GridData = data;

                }
                if (filterType == 2 || filterType == 3)
                {
                    var records = (from cd in this.dbContext.PrcgetReportsCensusDetail(fromdate, todate, userId, seriesName, filterType, reportType, category)
                                   select new
                                   {
                                       Count = (int)cd.Count,
                                       FromDate = cd.FromDate,
                                       NurseStation_Name = cd.NurseStation_Name,
                                       PatientName = cd.PatientName
                                   }).ToList();
                    var admitDates = records.Where(e => e.NurseStation_Name == seriesName).Select(e => e.FromDate).Distinct().ToList();

                    var data = new List<SeriesDataEntity>();

                    //var grpByData = records.Where(e => e.NurseStation_Name == ns).GroupBy(e => new { e.FromDate, e.Count }).ToList();
                    // .Select(e => new { count = e.Key.Count, val = e.Key.FromDate).ToList();
                    //data.Add(new { name = ns, data = grpByData.Select(e => e.Key.Count) });
                    List<int> census = new List<int>();
                    foreach (var date in admitDates)
                    {
                        census.Add(records.Where(e => e.FromDate == date).Select(e => e.Count).SingleOrDefault());
                    }
                    data.Add(new SeriesDataEntity { name = seriesName, data = census });//, nsname = seriesName });

                    var columnNames = new List<DashboardColumnsEntity>();
                    //var columns = new DashboardColumnsEntity() { name = "Stack_Name", displayName = "Stack Name", display = false };
                    var columns = new DashboardColumnsEntity() { name = "name", displayName = "Nursing Station Name", display = true, alignType = "left", columnType = "static" };
                    columnNames.Add(columns);
                    foreach (var date in admitDates)
                    {
                        columns = new DashboardColumnsEntity()
                        {
                            name = date,
                            displayName = date,
                            display = true,
                            alignType = "right",
                            columnType = "dynamic"
                        };
                        columnNames.Add(columns);
                    }

                    entity.ColumnNames = columnNames;
                    entity.YaxisData = data;
                    entity.XaxisData = admitDates;
                    entity.GridData = data;

                }



                // //var residents = records.Where(e => e.NurseStation_Name == seriesName).Select(e => e.Resident_Name).Distinct().ToList();
                // //foreach (var item in residents)
                // //{
                // var grpByData = records.Where(e => e.NurseStation_Name == seriesName).GroupBy(e => new { e.FromDate, e.Count })
                //.Select(e => e.Key.Count).ToList();
                // data.Add(new SeriesDataEntity { name = seriesName, data = grpByData });
                // //}


                // var columnNames = new List<DashboardColumnsEntity>();
                // var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "NurseStation Name", display = true };

                // columnNames.Add(columns);
                // foreach (var date in admitDates)
                // {
                //     columns = new DashboardColumnsEntity()
                //     {
                //         name = date,
                //         displayName = date,
                //         display = true
                //     };
                //     columnNames.Add(columns);
                // }


            }
            //else if (moduleName == "census2" && category != string.Empty && seriesName != string.Empty)
            //{
            //    //change filtertype to 1 means month
            //    filterType = 1;
            //    var records = this.dbContext.PrcgetReportsCensusDetail(fromdate, todate, userId, seriesName, filterType, reportType, category).ToList();

            //    #region Commented for future
            //    /* var admitDates = records.Where(e => e.NurseStation_Name == seriesName).Select(e => Convert.ToDateTime(e.FromDate).ToString("MM-dd-yyyy")).Distinct().ToList();
            //     var residents = records.Where(e => e.NurseStation_Name == seriesName).Select(e => e.Resident_Name).Distinct().ToList();
            //     var data = new List<SeriesDataEntity>();
            //     foreach (var item in residents)
            //     {
            //         var grpByData = records.Where(e => e.NurseStation_Name == seriesName).GroupBy(e => new { e.FromDate, e.Count })
            //        .Select(e => (int)e.Key.Count).ToList();
            //         data.Add(new SeriesDataEntity { name = category, data = grpByData });
            //     }*/
            //    #endregion

            //    var admitDates = records.Select(e => e.FromDate).Distinct().ToList();
            //    var grpByData = records.Select(e => (int)e.Count).ToList();
            //    var data = new List<SeriesDataEntity>();
            //    data.Add(new SeriesDataEntity { name = category, data = grpByData });

            //    var columnNames = new List<DashboardColumnsEntity>();
            //    var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "NurseStation Name", display = true };

            //    columnNames.Add(columns);
            //    foreach (var date in admitDates)
            //    {
            //        columns = new DashboardColumnsEntity()
            //        {
            //            name = date,
            //            displayName = date,
            //            display = true
            //        };
            //        columnNames.Add(columns);
            //    }

            //    entity.ColumnNames = columnNames;
            //    entity.YaxisData = data;
            //    entity.XaxisData = admitDates;
            //    entity.GridData = records;
            //}
            return entity;
        }

        public DashBoardEntity GetCensusCompareDashboard(string years, int userId, int nursingstationId, int type, int reportType)
        {
            var records = this.dbContext.PrcgetCensusCompareDatainfo(years, userId, nursingstationId, type, reportType).ToList();
            var fromDates = records.OrderBy(e => e.Month).Select(e => e.FromDate).Distinct().ToList();
            string nurseStationName = records.Select(e => e.NurseStation_Name).FirstOrDefault();
            string[] yearsList = years.Split(',');
            if (yearsList[yearsList.Length - 1] == "")
                yearsList = yearsList.Take(yearsList.Count() - 1).ToArray();
            var data = new List<SeriesDataEntity>();
            foreach (string year in yearsList)
            {
                var census = new List<int>();
                foreach (var date in fromDates)
                {
                    census.Add(records.Where(e => e.Year == Convert.ToInt32(year) && e.FromDate == date).Select(e => (int)e.Count).SingleOrDefault());
                }
                data.Add(new SeriesDataEntity { name = year, data = census, nsname = nurseStationName });
            }
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity() { name = "nsname", displayName = "Nursing Station Name", display = true, alignType = "left", columnType = "static" };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "name", displayName = "Year", display = true, alignType = "right", columnType = "static" };
            columnNames.Add(columns);
            foreach (var date in fromDates)
            {
                columns = new DashboardColumnsEntity()
                {
                    name = date,
                    displayName = date,
                    display = true,
                    alignType = "right",
                    columnType = "dynamic"
                };
                columnNames.Add(columns);
            }


            DashBoardEntity entity = new DashBoardEntity();
            entity.ColumnNames = columnNames;
            entity.YaxisData = data;
            entity.XaxisData = fromDates;
            entity.GridData = data;

            return entity;
        }
        public DashBoardEntity GetAverageCensusDashboard(int year, int month, int userId, string nursingstation)
        {
            if (nursingstation != "" && nursingstation != null)
            {
                string[] strArray = nursingstation.Split(',');
                int nurseStationNewID = Convert.ToInt32(strArray[0]);
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = userId,
                    Facility_Id = (int)facilityId,
                    NurseStation_Id = nursingstation
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            var records = this.dbContext.PrcGetAverageCensusData(year, month, userId, nursingstation).ToList();
            //var fromDates = records.OrderBy(e => e.Month).Select(e => e.FromDate).Distinct().ToList();
            var nurseStationNames = records.Select(e => e.NurseStation_Name).Distinct().ToList();
            //string[] yearsList = years.Split(',');
            //if (yearsList[yearsList.Length - 1] == "")
            //    yearsList = yearsList.Take(yearsList.Count() - 1).ToArray();
            var data = new List<PieSeriesDataEntity>();
            foreach (string ns in nurseStationNames)
            {
                var census = records.Where(e => e.NurseStation_Name == ns).Select(e => (int)e.Average_Census).SingleOrDefault();
                data.Add(new PieSeriesDataEntity { name = ns, y = census });
            }
            var columnNames = new List<DashboardColumnsEntity>();
            //var columns = new DashboardColumnsEntity() { name = "Year", displayName = "Year", display = true };
            //columnNames.Add(columns);
            var columns = new DashboardColumnsEntity() { name = "name", displayName = "Nursing Station Name", display = true, alignType = "left" };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "y", displayName = "Average Census", display = true, alignType = "right" };
            columnNames.Add(columns);


            DashBoardEntity entity = new DashBoardEntity();
            entity.ColumnNames = columnNames;
            entity.YaxisData = data;
            entity.XaxisData = nurseStationNames;
            entity.GridData = data;

            return entity;
        }
        public DashBoardEntity GetCommonDashboard(string dashboardName, string fromDate, string toDate, int userId, string nursingstationId, int currentPage, int pageSize, string passTime, Nullable<int> orderType, int month, int year, int patientId, string patientName, string commentType, string userIds, string shiftTime, int facilityid,string MedicationReason,string datetime)
        {
            int skipRows = (currentPage - 1) * pageSize;
            DateTime fromdate = Convert.ToDateTime(fromDate);
            DateTime todate = Convert.ToDateTime(toDate);
            //if (nursingstationId == "all")
            //  nursingstationId = null;
            if (patientName == "all")
                patientName = null;
            if (userIds == "null")
                userIds = null;
            DashBoardEntity entity = new DashBoardEntity();


            if (nursingstationId != "" && nursingstationId != null && nursingstationId != "null")
            {
                string[] strArray = nursingstationId.Split(',');
                int nurseStationNewID = Convert.ToInt32(strArray[0]);
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = userId,
                    Facility_Id = (int)facilityId,
                    NurseStation_Id = nursingstationId
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }


            if (dashboardName == "seventytwo")
            {
                var recordsCount = this.dbContext.PrcGetReports72HoursData(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcGetReports72HoursData(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.NurseStation_Name,
                                   ResidentName = pg.ResidentName + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   DrugName = pg.DrugName,
                                   DispensedBy = pg.DispensedBy,
                                   StartDate = pg.StartDate,
                                   AdministrationSchedule = Convert.ToDateTime(pg.AdministrationSchedule).ToString("MM/dd/yyyy"),
                                   PassTime = pg.PassTime,
                                   Comment = pg.Comment,
                                   CommentedBy = pg.CommentedBy,
                                   DOB = pg.DOB,
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministrationSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PassTime", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DispensedBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Comment", displayName = "Comment", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "CommentedBy", displayName = "Commented By", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "prnr")
            {
                var recordsCount = this.dbContext.PrcReportsGetPRNDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetPRNDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   DrugName = pg.Drug_Name,
                                   Startdate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   AdministrationSchedule = Convert.ToDateTime(pg.AdministrationSchedule).ToString("MM/dd/yyyy"),
                                   PassTime = pg.Pass_Time,
                                   Status = pg.Status,
                                   DispensedBy = pg.Dispensed_By,
                                   PRNComment = pg.PRNComment,
                                   CommentedBy = pg.Commented_BY,
                                   DOB = pg.DOB
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Startdate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministrationSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PassTime", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Status", displayName = "Status", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DispensedBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PRNComment", displayName = "PRN Comment", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "CommentedBy", displayName = "Commented By", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
             if (dashboardName == "order")
            {
               
                if (passTime == "null")
                    passTime = null;
                else
                    passTime = passTime.Replace('-', ':');
                int shiftTimes = 0;
                if (shiftTime == "null")
                    shiftTimes = 0;
                else
                   shiftTimes = Convert.ToInt32(shiftTime);
                var recordsCount = this.dbContext.PrcReportsGetOrderDetails(passTime, fromdate, todate, orderType, userId, nursingstationId, shiftTimes).Count();
                               
                var records = (from pg in this.dbContext.PrcReportsGetOrderDetails(passTime, fromdate, todate, orderType, userId, nursingstationId, shiftTimes)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name +" ("+ (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   Order_Type = pg.Order_Type,
                                   DrugName = pg.Drug_Name,
                                   Startdate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   PassTime = pg.Pass_Time,
                                   Status = pg.Status,
                                   DispensedBy = pg.Dispensed_By,
                                   AdminsterSchedule = pg.AdminsterSchedule != null ? Convert.ToDateTime(pg.AdminsterSchedule).ToString("MM/dd/yyyy") : null,
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Order_Type", displayName = "Order Type", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Startdate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdminsterSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PassTime", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Status", displayName = "Medication Reason", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DispensedBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);
              

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "orderhold")
            {
                var recordsCount = this.dbContext.PrcReportsGetOrderHoldDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetOrderHoldDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName = pg.Drug_Name,
                                   Directions=pg.Directions,
                                   StartDate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   //EndDate = pg.enddate != null ? Convert.ToDateTime(pg.enddate).ToString("MM/dd/yyyy") : null,
                                   Physician = pg.Physician,
                                   HoldFrom = pg.HoldFrom != null ? Convert.ToDateTime(pg.HoldFrom).ToString("MM/dd/yyyy") : null,
                                   HoldTo = pg.HoldTo != null ? Convert.ToDateTime(pg.HoldTo).ToString("MM/dd/yyyy") : null,
                                   HoldReason = pg.HoldReason
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Directions", displayName = "Directions", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "EndDate", displayName = "End Date", display = true };
                //columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "HoldFrom", displayName = "Hold From", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "HoldTo", displayName = "Hold To", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "HoldReason", displayName = "Hold Reason", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "orderfav")
            {
                if (passTime == "null")
                    passTime = null;
                else
                    passTime = passTime.Replace('-', ':');
                int shiftTimes = 0;
                if (shiftTime == "null")
                    shiftTimes = 0;
                else
                    shiftTimes = Convert.ToInt32(shiftTime);
                var recordsCount = this.dbContext.PrcReportsGetOrderWithFavourites(passTime, fromdate, todate, userId, nursingstationId, shiftTimes).Count();
                var records = (from pg in this.dbContext.PrcReportsGetOrderWithFavourites(passTime, fromdate, todate, userId, nursingstationId,shiftTimes)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   OrderType = pg.Order_Type,
                                   DrugName = pg.Drug_Name,
                                   StartDate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   EndDate = pg.End_Date != null ? Convert.ToDateTime(pg.End_Date).ToString("MM/dd/yyyy") : null,
                                   AdminsterSchedule = pg.AdminsterSchedule != null ? Convert.ToDateTime(pg.AdminsterSchedule).ToString("MM/dd/yyyy") : null,
                                   Favourites_Added = pg.Prerequisite_checks,
                                   PassTime=pg.Pass_Time,
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "OrderType", displayName = "Order Type", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EndDate", displayName = "End Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdminsterSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PassTime", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Favourites_Added", displayName = "Require User Inputs", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }

            if (dashboardName == "DocAdminOrder")
            {
                var recordsCount = this.dbContext.PrcReportsGetDocAdminOrderDetails(fromdate, todate, orderType, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetDocAdminOrderDetails(fromdate, todate, orderType, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   DrugName = pg.Drug_Name,

                                   Scheduled_Date_Time = pg.Scheduled_Date_Time,
                                   Forced_Date_Time=pg.Forced_Date_Time,
                                   EnteredBy = pg.Entered_By,
                                   EnteredDate = pg.Entered_Date!=null ? Convert.ToDateTime(pg.Entered_Date).ToString("MM/dd/yyyy hh:mm:ss tt") :"",
                                   AdministeredBy = pg.Administered_By,

                                   DOB = pg.DOB 
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                
                columns = new DashboardColumnsEntity() { name = "Scheduled_Date_Time", displayName = "Scheduled Date/Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Forced_Date_Time", displayName = "Forced Date/Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministeredBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EnteredBy", displayName = "Entered By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EnteredDate", displayName = "Entered Date/Time", display = true };
                columnNames.Add(columns);
                

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "barcode")
            {
                var recordsCount = this.dbContext.PrcReportsGetWithoutBarcodeDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetWithoutBarcodeDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   DrugName = pg.Drug_Name,
                                   StartDate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   EndDate = pg.End_Date != null ? Convert.ToDateTime(pg.End_Date).ToString("MM/dd/yyyy") : null,
                                   Physician = pg.Physician,
                                   DOB = pg.DOB,
                                   Directions=pg.Directions
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Directions", displayName = "Direction", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EndDate", displayName = "End Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);
                
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "biometric")
            {
                var recordsCount = this.dbContext.PrcReportsGetWithoutBiometricsDetails(fromdate, todate, userId, nursingstationId). Count();
                var records = (from pg in this.dbContext.PrcReportsGetWithoutBiometricsDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   OrderType = pg.Order_Type,
                                   DrugName = pg.Drug_Name,
                                   PassTime = pg.Pass_Time,
                                   StartDate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   Reason_For_Biometric_Bypass = pg.Reason_For_Biometric_Bypass,
                                   Status = pg.Status,
                                   DispensedBy = pg.Dispensed_By,
                                   DOB = pg.DOB,
                                   AdministrationSchedule = Convert.ToDateTime(pg.AdministrationSchedule).ToString("MM/dd/yyyy"),
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "OrderType", displayName = "Order Type", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministrationSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PassTime", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Reason_For_Biometric_Bypass", displayName = "Reason For Biometric Bypass", display = true };
                columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Status", displayName = "Medication Reason", display = true };
                //columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DispensedBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "ordercs")
            {
                var recordsCount = this.dbContext.PrcReportsGetOrderControlSubstance(nursingstationId, userId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetOrderControlSubstance(nursingstationId, userId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName = pg.Drug_Name,
                                   StartDate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   EndDate = pg.End_Date != null ? Convert.ToDateTime(pg.End_Date).ToString("MM/dd/yyyy") : null,
                                   Physician = pg.Physician
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EndDate", displayName = "End Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "ordercsoff")
            {
                var recordsCount = this.dbContext.PrcReportsGetOrderControlSignoff(fromdate, todate, nursingstationId, userId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetOrderControlSignoff(fromdate, todate, nursingstationId, userId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName = pg.Drug_Name,
                                   LastCertify = pg.Last_Certify,
                                   Date_Certify_and_Time = Convert.ToDateTime(pg.Date_Certify_and_Time).ToString("MM/dd/yyyy hh:mm:ss tt"),
                                   Quantity = pg.Quantity,
                                   DiscrepancyReason=pg.DiscrepancyReason,
                                   Last_Witness = pg.Last_Witness
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "LastCertify", displayName = "Last Certified By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Date_Certify_and_Time", displayName = "Last Certified Date Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Quantity", displayName = "Quantity Certified", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DiscrepancyReason", displayName = "Discrepancy Reason", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Last_Witness", displayName = "Witness", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "orderdes")
            {
                var recordsCount = this.dbContext.PrcReportsGetDestructionDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetDestructionDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName = pg.Drug_Name,
                                   //QtyonHand=pg.QtyAfterDestroy,
                                   QtyBeforeDestroy=pg.QtyonHand,
                                   Destruction_Quantity = pg.Destruction_Quantity,
                                   Reason = pg.Reason,
                                   Destruction_Done_By = pg.Destruction_Done_By,
                                   DateTimeofDestruction = Convert.ToDateTime(pg.DateTime_of_Destruction).ToString("MM/dd/yyyy hh:mm:ss tt"),
                                   Witness=pg.Witness
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "QtyBeforeDestroy", displayName = "Quantity Before Destruction", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Destruction_Quantity", displayName = "Quantity Destroyed", display = true };
                columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "QtyonHand", displayName = "Quantity On Hand", display = true };
                //columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Reason", displayName = "Reason", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Destruction_Done_By", displayName = "Destroyed By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DateTimeofDestruction", displayName = "Date/Time of Destruction", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Witness", displayName = "Witness", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "fstock")
            {
                if (nursingstationId == "null")
                {
                    nursingstationId = null;
                }
                var recordsCount = this.dbContext.PrcReportsNewGetFloorStockDetails(userId, nursingstationId,facilityid).Count();
                var records = (from pg in this.dbContext.PrcReportsNewGetFloorStockDetails(userId, nursingstationId, facilityid)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   DrugName = pg.DrugName,
                                   QuantityonHand = pg.Quantity_on_Hand,
                                   BarcodeDetail = pg.BarcodeDetail,
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "QuantityonHand", displayName = "Quantity on Hand", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "BarcodeDetail", displayName = "Barcode", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "orderchange")
            {
                //var gridData = records.Select(e => new
                //{
                //    Patient_Id = e.Patient_Id,
                //    ResidentName = e.ResidentName,
                //    Vitals_ID = e.Vitals_ID,
                //    vitaldate = e.vitaldate,
                //    VitalTime = e.VitalTime,
                //    VitalType = e.GetType().GetProperty(columnName).GetValue(e)
                //}).ToList();
                var records = this.dbContext.PrcreportsGetOrderchange("1", fromdate, todate);
                //var records = (from pg in this.dbContext.PrcReportsGetOrderChangeDetails(fromdate, todate, userId, nursingstationId)
                //               select new
                //               {
                //                   date = pg.date,
                //                   ResidentName = pg.Patient_Name,
                //                   Drug = pg.Drug,
                //                   Dosage_form = pg.Dosage_form,
                //                   StartDate = pg.StartDate != null ? Convert.ToDateTime(pg.StartDate).ToString("MM/dd/yyyy") : null,
                //                   EndDate = pg.EndDate != null ? Convert.ToDateTime(pg.EndDate).ToString("MM/dd/yyyy") : null,
                //                   Additional_Instructions = pg.Additional_Instructions,
                //                   Physician = pg.Physician,
                //                   Route = pg.Route,
                //                   Diagnosis = pg.Diagnosis,
                //                   Favourites_Added = pg.Favourites_Added,
                //                   Refill_Remaining = pg.Refill_Remaining,
                //                   Quantity_On_Hand = pg.Quantity_on_Hand,
                //                   Barcode = pg.Barcode,
                //                   Created_By = pg.Created_By,
                //                   Created_Date = pg.Created_Date
                //               }).Skip(skipRows).Take(pageSize).ToList();


                //var columnNames = new List<DashboardColumnsEntity>();

                //var columns = new DashboardColumnsEntity() { name = "date", displayName = "Date", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Drug", displayName = "Drug", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Dosage_form", displayName = "Dosage Form", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "EndDate", displayName = "End Date", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Additional_Instructions", displayName = "Additional Instructions", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Physician", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Route", displayName = "Route", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Diagnosis", displayName = "Diagnosis", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Favourites_Added", displayName = "Require User Inputs", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Refill_Remaining", displayName = "Refill Remaining", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Quantity_On_Hand", displayName = "Quantity On Hand", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Barcode", displayName = "Barcode", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Created_By", displayName = "Created By", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Created_Date", displayName = "Created Date", display = true };
                //columnNames.Add(columns);
                //entity.ColumnNames = columnNames;
                //entity.YaxisData = null;
                //entity.XaxisData = null;
                //entity.GridData = records;
                //entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "pharmacy")
            {
                var recordsCount = this.dbContext.PrcReportsGetPharmacyMedsDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetPharmacyMedsDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.NursingStationName,
                                   ResidentName = pg.ResidentName + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   DrugName = pg.DrugName,
                                   checkindate = pg.Check_In_Date!= null ? Convert.ToDateTime(pg.Check_In_Date).ToString("MM/dd/yyyy") : null,
                                   Quantity = pg.QuantityReceived,
                                   Barcodes = pg.Barcode_s_,
                                   CheckinBy=pg.Check_In_By,
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Quantity", displayName = "Quantity Received", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Barcodes", displayName = "Barcode(s)", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "CheckinBy", displayName = "Check-in By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "checkindate", displayName = "Check-in Date", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "medref")
            {
                var recordsCount = this.dbContext.PrcReportsGetRefusedByResidentDetails(fromdate, todate, userId, nursingstationId, passTime).Count();
                var records = (from pg in this.dbContext.PrcReportsGetRefusedByResidentDetails(fromdate, todate, userId, nursingstationId, passTime)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   Pass_Time = pg.Pass_Time,
                                   OrderType = pg.Order_Type,
                                   DrugName = pg.Drug_Name,
                                   StartDate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   AdminsterSchedule = pg.AdminsterSchedule != null ? Convert.ToDateTime(pg.AdminsterSchedule).ToString("MM/dd/yyyy") : null,
                                   //Nurse_Comments = pg.Nurse_Comments,
                                   Physician = pg.Physician
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "OrderType", displayName = "Order Type", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdminsterSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Pass_Time", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Nurse_Comments", displayName = "Nurse Comments", display = true };
                //columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "refused")
           {
                // var recordsCount = this.dbContext.PrcReportsGetRefusedByResidentDetails(fromdate, todate, userId, nursingstationId,passTime).Count();

                string query = "[Patient].[PrcReportsGetTherapeuticDrugData]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = nursingstationId;
                        cmd.Parameters.Add("@Res", SqlDbType.VarChar).Value = datetime;
                        cmd.Parameters.Add("@GPI", SqlDbType.VarChar).Value = passTime;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }
                    }
                }

                int recordsCount = ds.Tables[0].Rows.Count;





                var records = (from d in ds.Tables[0].AsEnumerable()
                               select new
                               {

                                   NurseStation_Name = d["Nurse Station Name"].ToString(),
                                   ResidentName = d["Resident"].ToString(),
                                   // Pass_Time = d["Resident"].ToString(),
                                   TherapeuticDrugType = d["TherapeuticDrugType"].ToString(),
                                   DrugName = d["Drug Name"].ToString(),
                                   StartDate = d["Start Date"].ToString(),
                                   EndDate = d["EndDate"].ToString(),
                                   //AdminsterSchedule = pg.AdminsterSchedule != null ? Convert.ToDateTime(pg.AdminsterSchedule).ToString("MM/dd/yyyy") : null,
                                   //Nurse_Comments = pg.Nurse_Comments,
                                   Physician = d["Physician"].ToString(),
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "TherapeuticDrugType", displayName = "Therapeutic DrugType", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EndDate", displayName = "End Date", display = true };
                columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "AdminsterSchedule", displayName = "Administration Schedule", display = true };
                //columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Pass_Time", displayName = "Pass Time", display = true };
                // columnNames.Add(columns);
                //columns = new DashboardColumnsEntity() { name = "Nurse_Comments", displayName = "Nurse Comments", display = true };
                //columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "scan")
            {
                var recordsCount = this.dbContext.PrcReportsGetWithoutScanningDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetWithoutScanningDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName = pg.Drug_Name,
                                   Direction = pg.Directions,
                                   Barcode = pg.Barcode_s_,
                                   AdministrationSchedule = pg.AdministrationSchedule != null ? Convert.ToDateTime(pg.AdministrationSchedule).ToString("MM/dd/yyyy") : null,
                                   Dispensed_By = pg.Dispensed_By,
                                  PassTime=pg.PassTime,
                                  ReasonForByPassing=pg.ReasonforBypassing
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Direction", displayName = "Directions", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Barcode", displayName = "Barcode(s)", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministrationSchedule", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "PassTime", displayName = "Pass Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Dispensed_By", displayName = "Administred By", display = true };
                columnNames.Add(columns);
                
                columns = new DashboardColumnsEntity() { name = "ReasonForByPassing", displayName = "Reason For Bypass", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "psychiatric")
            {
                var recordsCount = this.dbContext.PrcReportsGetPsychiatricDetails(fromdate, todate, userId, nursingstationId).Count();
                var records = (from pg in this.dbContext.PrcReportsGetPsychiatricDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   Resident_Status = pg.Resident_Status,
                                   Psychiatric_Drug_Count = pg.Psychiatric_Drug_Count,
                                   Physician =pg.Physician
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Resident_Status", displayName = "Resident Status", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Psychiatric_Drug_Count", displayName = "Psychiatric Drug Count", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "nursenote")
            {
                if(commentType !="1")
                {
                    MedicationReason = null;
                }
                var recordsCount = this.dbContext.PrcReportsGetNurseNotesDetails(fromdate, todate, userId, nursingstationId, commentType, MedicationReason).Count();
                var records = (from pg in this.dbContext.PrcReportsGetNurseNotesDetails(fromdate, todate, userId, nursingstationId, commentType, MedicationReason)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   Drug_Name = pg.Drug_Name,
                                   Comment_Type = pg.Comment_Type,
                                   Dispensed_By = pg.Dispensed_By,
                                   Nurse_Comments = pg.Nurse_Comments,
                                   Comment_On = pg.Comment_On != null ? Convert.ToDateTime(pg.Comment_On).ToString("MM/dd/yyyy  hh:mm:ss tt") : null,
                                   MedicationReason = pg.MedicationReason
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Drug_Name", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Comment_Type", displayName = "Comment Type", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Dispensed_By", displayName = "Dispensed By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Nurse_Comments", displayName = "Nurse Comments", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Comment_On", displayName = "Comment On", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "MedicationReason", displayName = "MedicationReason Desc", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "prescribernote")
            {
                var records = (from pg in this.dbContext.PrcReportsGetPrescriberNotesDetails(fromdate, todate, userId, nursingstationId)
                               select new
                               {
                                   NurseStation_Name = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name,
                                   Order_Type = pg.Order_Type,
                                   Drug_Name = pg.Drug_Name,
                                   Start_Date = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                                   End_Date = pg.End_Date != null ? Convert.ToDateTime(pg.End_Date).ToString("MM/dd/yyyy") : null,
                                   Prescriber_Notes = pg.Prescriber_Notes,
                                   Physician = pg.Physician
                               }).OrderBy(item => item.NurseStation_Name).ThenBy(item => item.ResidentName).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Order_Type", displayName = "Order Type", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Drug_Name", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Start_Date", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "End_Date", displayName = "End Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Prescriber_Notes", displayName = "Prescriber Notes", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Physician", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                return entity;
            }
            if (dashboardName == "inbound")
            {
                if (patientName == "")
                    patientName = null;
                var records = (from pg in this.dbContext.PrcReportsGetInboundDetails(fromdate, todate, patientName, null)
                               select new
                               {
                                   File_CreatedDate = pg.File_CreatedDate != null ? Convert.ToDateTime(pg.File_CreatedDate).ToString("MM/dd/yyyy") : null,
                                   File_Data = pg.File_Data,
                                   FileAck_Data = pg.FileAck_Data
                               }).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "File_CreatedDate", displayName = "File Created Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "File_Data", displayName = "File Data", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "FileAck_Data", displayName = "File Acknowledgement", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                return entity;
            }
            if (dashboardName == "outbounderror")
            {
                var records = (from pg in this.dbContext.PrcReportsGetOutBoundErrorDetails(fromdate, todate)
                               select new
                               {
                                   File_Data = pg.File_Data,
                                   File_Acknowledge = pg.File_Acknowledge,
                                   File_AcknowledgeDate = pg.File_AcknowledgeDate != null ? Convert.ToDateTime(pg.File_AcknowledgeDate).ToString("MM/dd/yyyy") : null,
                               }).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "File_Data", displayName = "File Data", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "File_Acknowledge", displayName = "File Acknowledgement", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "File_AcknowledgeDate", displayName = "File Acknowledgement Date", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                return entity;
            }
            if (dashboardName == "emarhole")
            {
                var recordsCount = this.dbContext.PrcGetMARHoledetails(month, year, nursingstationId, userId, patientName).Count();
                var records = (from pg in this.dbContext.PrcGetMARHoledetails(month, year, nursingstationId, userId, patientName)
                               select new
                               {
                                   PatientName = pg.PatientName + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName = pg.Drug_Name,
                                   StartDate = pg.StartDate != null ? Convert.ToDateTime(pg.StartDate).ToString("MM/dd/yyyy") : null,
                                   ScheduleDate = pg.ScheduleDate != null ? Convert.ToDateTime(pg.ScheduleDate).ToString("MM/dd/yyyy") : null,
                                   time = pg.time,

                               }).Skip(skipRows).Take(pageSize).OrderBy(item => item.PatientName).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "PatientName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ScheduleDate", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "time", displayName = "Time", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "emarhistory")
            {
                var recordsCount = this.dbContext.PrcGetMARHistorydetails(month, year, nursingstationId, userId, patientName).Count();
                var records = (from pg in this.dbContext.PrcGetMARHistorydetails(month, year, nursingstationId, userId, patientName)
                               select new
                               {
                                   PatientName = pg.PatientName + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                                   DrugName=pg.Drug_Name,
                                   StartDate = pg.StartDate != null ? Convert.ToDateTime(pg.StartDate).ToString("MM/dd/yyyy") : null,
                                   ScheduleDate = pg.ScheduleDate != null ? Convert.ToDateTime(pg.ScheduleDate).ToString("MM/dd/yyyy") : null,
                                   time = pg.time,
                                   AdministerBy = pg.AdministerBy
                               }).Skip(skipRows).Take(pageSize).OrderBy(item => item.PatientName).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "PatientName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "StartDate", displayName = "Start Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ScheduleDate", displayName = "Administration Schedule", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "time", displayName = "Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministerBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "useractivity")
            {
                string[] usersArray = userIds.Split(',');
                var records = (from ss in this.dbContext.UserSessions.AsEnumerable()
                               join us in this.dbContext.Users on ss.user_Id equals us.User_Id
                               join rl in this.dbContext.Roles on ss.role_Id equals rl.Role_Id
                               where ss.Session_Status == 1 && usersArray.Contains(ss.user_Id.ToString()) && Convert.ToDateTime(ss.LoginTime).Date >= fromdate.Date && Convert.ToDateTime(ss.LoginTime).Date <= todate.Date
                               select new
                               {
                                   Session_Id = ss.Session_Id,
                                   User_Name = us.UserName,
                                   User_DisplayName=us.User_DisplayName,
                                   Role_Name = rl.Role_Desc,
                                   SystemIp = ss.SystemIP,
                                   Browser_Name = ss.BrowserName,
                                   LogIn_Time = ss.LoginTime != null ? Convert.ToDateTime(ss.LoginTime).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                                   LogOut_Time = ss.LogOutTime != null ? Convert.ToDateTime(ss.LogOutTime).ToString("MM/dd/yyyy hh:mm:ss tt") : null
                               }).OrderByDescending(item=>item.Session_Id).OrderBy(U=>U.User_Name).Skip(skipRows).Take(pageSize).ToList();
                var recordsCount = (from ss in this.dbContext.UserSessions.AsEnumerable()
                               join us in this.dbContext.Users on ss.user_Id equals us.User_Id
                               join rl in this.dbContext.Roles on ss.role_Id equals rl.Role_Id
                               where ss.Session_Status == 1 && usersArray.Contains(ss.user_Id.ToString()) && Convert.ToDateTime(ss.LoginTime).Date >= fromdate.Date && Convert.ToDateTime(ss.LoginTime).Date <= todate.Date
                                    select new
                               {
                                   Session_Id = ss.Session_Id,
                                   User_Name = us.UserName,
                                   User_DisplayName = us.User_DisplayName,
                                   Role_Name = rl.Role_Desc,
                                   SystemIp = ss.SystemIP,
                                   Browser_Name = ss.BrowserName,
                                   LogIn_Time = ss.LoginTime != null ? Convert.ToDateTime(ss.LoginTime).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                                   LogOut_Time = ss.LogOutTime != null ? Convert.ToDateTime(ss.LogOutTime).ToString("MM/dd/yyyy hh:mm:ss tt") : null
                               }).ToList().Count();
                var columnNames = new List<DashboardColumnsEntity>();

                //var columns = new DashboardColumnsEntity() { name = "Session_Id", displayName = "Session Id", display = false };
                //columnNames.Add(columns);
                var columns = new DashboardColumnsEntity() { name = "User_Name", displayName = "User Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "User_DisplayName", displayName = "User Display Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Role_Name", displayName = "Role", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "SystemIp", displayName = "System IP", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Browser_Name", displayName = "Browser Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "LogIn_Time", displayName = "Start Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "LogOut_Time", displayName = "End Time", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            //if (dashboardName == "ekit")
            //{
            //    string query = "[Admin].[PrcReportsGetEKitDetails]";
            //    string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            //    DataSet ds = new DataSet();
            //    using (SqlConnection con = new SqlConnection(constrEmar))
            //    {
            //        con.Open();
            //        using (SqlCommand cmd = new SqlCommand(query))
            //        {
            //            cmd.Connection = con;
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.CommandTimeout = 180;
            //            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
            //            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
            //            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
            //            cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = nursingstationId;
            //            cmd.Parameters.Add("@Status", SqlDbType.Int).Value = orderType;
            //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //            {
            //                sda.Fill(ds);
            //            }
            //        }
            //    }
            //    int recordsCount = ds.Tables[0].Rows.Count;
            //    var records = (from pg in ds.Tables[0].AsEnumerable()
            //                   select new
            //                   {
            //                       NurseStationName = Convert.ToString(pg["Nurse Station Name"]),
            //                       DrugName = Convert.ToString(pg["DrugName"]),
            //                       QuantityonHand = Convert.ToString(pg["Quantity on Hand"]),
            //                       //assessedon = Convert.ToString(pg["assessedon"]),
            //                       assessedon = string.IsNullOrEmpty(pg["assessedon"].ToString()) ? "" : Convert.ToDateTime(pg["assessedon"]).ToString("MM/dd/yyyy hh:mm:ss tt"),
            //                       // assessedon = pg["assessedon"] != null ? Convert.ToDateTime(pg["assessedon"]).ToString("MM/dd/yyyy hh:mm:ss tt") : null

            //                   }).Skip(skipRows).Take(pageSize).ToList();
            //    var columnNames = new List<DashboardColumnsEntity>();

            //    var columns = new DashboardColumnsEntity() { name = "NurseStationName", displayName = "Nursing Station", display = true };
            //    columnNames.Add(columns);
            //    columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
            //    columnNames.Add(columns);
            //    columns = new DashboardColumnsEntity() { name = "QuantityonHand", displayName = "Qty on Hand", display = true };
            //    columnNames.Add(columns);
            //    columns = new DashboardColumnsEntity() { name = "assessedon", displayName = "Assessed On", display = true };
            //    columnNames.Add(columns);
            //    entity.ColumnNames = columnNames;
            //    entity.YaxisData = null;
            //    entity.XaxisData = null;
            //    entity.GridData = records;
            //    entity.TotalRecordsCount = recordsCount;
            //    return entity;
            //}
            if (dashboardName == "refill")
            {
                var recordsCount = this.dbContext.PrcReportsGetRefillDetails(userId, nursingstationId, fromdate, todate).Count();
                var records = (from pg in this.dbContext.PrcReportsGetRefillDetails(userId, nursingstationId, fromdate, todate)
                               select new
                               {
                                   NurseStationName = pg.Nurse_Station_Name,
                                   ResidentName = pg.Resident_Name + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                                   DrugName = pg.Drug_Name,
                                   NumberOfRefillsRemaining = pg.NumberOfRefillsRemaining,
                                   RefillSubmittedBy = pg.RefillSubmittedBy,
                                   Status = pg.Status,
                                   RefillCreatedDate = pg.Refill_CreatedDate != null ? Convert.ToDateTime(pg.Refill_CreatedDate).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStationName", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "NumberOfRefillsRemaining", displayName = "Refills Remaining", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "RefillSubmittedBy", displayName = "Refill Submitted By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "RefillCreatedDate", displayName = "Date/Time Refill Requested", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Status", displayName = "Refill Request Status", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "ekitdispensing")
            {
                string query = "[Admin].[PrcReportsGetEKitDispensingDetails]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = nursingstationId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }
                    }
                }

                int recordsCount = ds.Tables[0].Rows.Count;
                var records = (from pg in ds.Tables[0].AsEnumerable()
                               select new ekitdispensingentity
                               {
                                   NurseStationName = Convert.ToString(pg["NursingStationName"]),
                                   ResidentName = Convert.ToString(pg["ResidentName"]) + " " + (pg["DOB"].ToString() == null ? "" : "(" + Convert.ToDateTime(pg["DOB"]).ToString("MM/dd/yyyy") + ")"),
                                   DrugName = Convert.ToString(pg["DrugName"]),
                                   QtyOnHandRemaining = Convert.ToString(pg["QtyOnHandRemaining"]),
                                   //QuantityAdministered = Convert.ToString(pg["QuantityAdministered"]),
                                   QuantityAdministered = string.IsNullOrEmpty(pg["QuantityAdministered"].ToString()) ? (decimal?)null : Math.Round(Convert.ToDecimal(pg["QuantityAdministered"]), 2),
                                   EkitAdministeredBy = Convert.ToString(pg["EkitAdministeredBy"]),
                                   //AdministeredDateTime = string.IsNullOrEmpty(pg["AdministeredDateTime"].ToString()) ? (DateTime?)null : (DateTime?)Convert.ToDateTime(pg["AdministeredDateTime"]),
                                   //AdministeredDateTime = string.IsNullOrEmpty(pg["AdministeredDateTime"].ToString()) != null ? Convert.ToDateTime(pg["AdministeredDateTime"]).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                                   //AdministeredDateTime = pg["AdministeredDateTime"] != null ? Convert.ToDateTime(pg["AdministeredDateTime"]).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                                   AdministeredDateTime = string.IsNullOrEmpty(pg["AdministeredDateTime"].ToString()) ? "" : Convert.ToDateTime(pg["AdministeredDateTime"]).ToString("MM/dd/yyyy hh:mm:ss tt"),
                                   Physician = Convert.ToString(pg["Physician"]),
                                   Expirydate = string.IsNullOrEmpty(pg["Expirydate"].ToString()) ? "" : Convert.ToDateTime(pg["Expirydate"]).ToString("MM/dd/yyyy"),
                                   LotNumber = Convert.ToString(pg["LotNumber"])
                               }).Skip(skipRows).Take(pageSize).ToList();
                //     var recordsCount = this.dbContext.PrcReportsGetEKitDispensingDetails(fromdate, todate, userId, nursingstationId).Count();
                //var records = (from pg in this.dbContext.PrcReportsGetEKitDispensingDetails(fromdate, todate, userId, nursingstationId)
                //               select new
                //               {
                //                   NurseStationName = pg.NursingStationName,
                //                   ResidentName = pg.ResidentName + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                //                   DrugName = pg.DrugName,
                //                   QtyOnHandRemaining = pg.QtyOnHandRemaining,
                //                   QuantityAdministered = string.Format("{0:0.00}", pg.QuantityAdministered),
                //                   EkitAdministeredBy = pg.EkitAdministeredBy,
                //                   AdministeredDateTime = pg.AdministeredDateTime != null ? Convert.ToDateTime(pg.AdministeredDateTime).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                //                   Physician=pg.Physician,
                //                   Expirydate=pg.ExpiryDate,
                //                   LotNumber=pg.LotNumber
                //               }).Skip(skipRows).Take(pageSize).ToList();

                var columnNames = new List<DashboardColumnsEntity>();

                var columns = new DashboardColumnsEntity() { name = "NurseStationName", displayName = "Nursing Station Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "QuantityAdministered", displayName = "Qty Administered", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Physician", displayName = "Prescriber", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Expirydate", displayName = "Expiry Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "LotNumber", displayName = "Lot Number", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "EkitAdministeredBy", displayName = "Administered By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "AdministeredDateTime", displayName = "Administration  Date/Time", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "QtyOnHandRemaining", displayName = "Qty on Hand Remaining", display = true };
                columnNames.Add(columns);
                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "medicationcheckin")
            {
                string query = "[Admin].[Prc_GetEkitMedCheckInReport]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        // cmd.Parameters.Add("@facility_id", SqlDbType.Int).Value = facilityid;
                        cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = nursingstationId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }
                    }
                }
                int recordsCount = ds.Tables[0].Rows.Count;
                var records = (from pg in ds.Tables[0].AsEnumerable()
                               select new MedicationCheckinDr
                               {
                                   NurseStationName = Convert.ToString(pg["Nurse Station Name"]),
                                   DrugName = Convert.ToString(pg["Drug Name"]),
                                   Barcode = Convert.ToString(pg["Barcode"]),
                                   Lot = Convert.ToString(pg["#Lot"]),
                                   ExpirationDate = string.IsNullOrEmpty(pg["Expiration Date"].ToString()) ? "" : Convert.ToDateTime(pg["Expiration Date"]).ToString("MM/dd/yyyy"),
                                   QtyReceived = Convert.ToString(pg["Qty Received"]),
                                   ReceivedBy = Convert.ToString(pg["Received By"]),
                                   ReceivedDate = string.IsNullOrEmpty(pg["Received Date"].ToString()) ? "" : Convert.ToDateTime(pg["Received Date"]).ToString("MM/dd/yyyy"),
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();
                var columns = new DashboardColumnsEntity() { name = "NurseStationName", displayName = "Nursing Station", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Barcode", displayName = "Barcode", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Lot", displayName = "Lot#", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ExpirationDate", displayName = "Exp Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "QtyReceived", displayName = "Qty Received", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ReceivedBy", displayName = "Received By", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ReceivedDate", displayName = "Received On", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            if (dashboardName == "MedicationQtyonhandUpdateReport")
            {
                string query = "[Admin].[Prc_ReportGetEkitQtyOnHandUpdate]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        // cmd.Parameters.Add("@facility_id", SqlDbType.Int).Value = facilityid;
                        cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = nursingstationId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }
                    }
                }
                int recordsCount = ds.Tables[0].Rows.Count;
                var records = (from pg in ds.Tables[0].AsEnumerable()
                               select new EkitQtyOnHandUpdate
                               {
                                   NurseStation_Name = Convert.ToString(pg["NurseStation_Name"]),
                                   Drugname = Convert.ToString(pg["Drugname"]),
                                   BarcodeDetail = Convert.ToString(pg["BarcodeDetail"]),
                                   LotNumber = Convert.ToString(pg["LotNumber"]),
                                   ExpiryDate = string.IsNullOrEmpty(pg["ExpiryDate"].ToString()) ? "" : Convert.ToDateTime(pg["ExpiryDate"]).ToString("MM/dd/yyyy"),
                                   qtybeforeupdate = Convert.ToString(pg["qtybeforeupdate"]),
                                   qtyafterupdate = Convert.ToString(pg["qtyafterupdate"]),
                                   reason = Convert.ToString(pg["reason"]),
                                   updatedby = Convert.ToString(pg["updatedby"]),
                                   updateddate = string.IsNullOrEmpty(pg["updateddate"].ToString()) ? "" : Convert.ToDateTime(pg["updateddate"]).ToString("MM/dd/yyyy hh:mm:ss tt"),
                                   witness = Convert.ToString(pg["witness"]),
                               }).Skip(skipRows).Take(pageSize).ToList();
                var columnNames = new List<DashboardColumnsEntity>();
                var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "Drugname", displayName = "Drug Name", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "BarcodeDetail", displayName = "Barcode", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "LotNumber", displayName = "Lot#", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "ExpiryDate", displayName = "Exp Date", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "qtybeforeupdate", displayName = "Qty before Update", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "qtyafterupdate", displayName = "Qty after Update", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "reason", displayName = "Reason for Update", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "updatedby", displayName = "Updated by", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "updateddate", displayName = "Updated on", display = true };
                columnNames.Add(columns);
                columns = new DashboardColumnsEntity() { name = "witness", displayName = "Witness", display = true };
                columnNames.Add(columns);

                entity.ColumnNames = columnNames;
                entity.YaxisData = null;
                entity.XaxisData = null;
                entity.GridData = records;
                entity.TotalRecordsCount = recordsCount;
                return entity;
            }
            return null;
        }

        public string GetPatientAllergiesByName(string patientName)
        {
            string allergies = this.dbContext.PrcGetAllergyInfoByPatient(patientName).FirstOrDefault();
            return allergies;
        }
        public List<UserActivityReportEntity> GetUserActivityDetails(Int64 sessionId)
        {
            var record = (from ua in this.dbContext.UserActivityDetails
                          join sc in this.dbContext.Screens on ua.Screen_Id equals sc.Screen_Id
                          join ac in this.dbContext.ActivityMasters on ua.Activity_Id equals ac.Activity_Id
                          where ua.Session_Id == sessionId
                          select new UserActivityReportEntity
                          {
                              Screen_Name = sc.Screen_Desc,
                              Activity_Name = ac.Activity_Desc,
                              Activity_Date = (DateTime)ua.Time,
                              Activity_Time = ua.Time.ToString(),
                              Comments = ua.Comments
                          }).ToList();
            return record;
        }
        public PrcReportGetStockDataGridEntity GetStockDashboardGrid(int userId, int companyId, int currentPage, int pageSize)
        {
            int skipRows = (currentPage - 1) * pageSize;
            var entity = new PrcReportGetStockDataGridEntity();
            var recordsCount = this.dbContext.PrcReportsGetStockData(userId, companyId).Count();
            var stock = this.dbContext.PrcReportsGetStockData(userId, companyId).Skip(skipRows).Take(pageSize).ToList();
            var records= this.autoMapper.Map<List<PrcReportsGetStockData_Result>, List<PrcReportGetStockData_ResultEntity>>(stock);
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        public PrcReportAdminUsersGridEntity GetAdminUseraDashboardGrid(string companyId, int userId, int currentPage, int pageSize)
        {
            int skipRows = (currentPage - 1) * pageSize;
            var entity = new PrcReportAdminUsersGridEntity();
            var recordsCount = this.dbContext.PrcReportsAdminUsers(companyId, userId).Count();
            var users = this.dbContext.PrcReportsAdminUsers(companyId,userId).Skip(skipRows).Take(pageSize).ToList();
            var records=this.autoMapper.Map<List<PrcReportsAdminUsers_Result>, List<PrcReportsAdminUsers_ResultEntity>>(users);
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        public PrcReportSetUpConfigGridEntity GetSetUpConfigDashboardGrid(string companyId, int currentPage, int pageSize)
        {
            int skipRows = (currentPage - 1) * pageSize;
            var entity = new PrcReportSetUpConfigGridEntity();
            var recordsCount = this.dbContext.PrcReportsSetUpConfigData(companyId).Count();
            var config = this.dbContext.PrcReportsSetUpConfigData(companyId).OrderBy(item => item.CompanyName).Skip(skipRows).Take(pageSize).ToList();
            var records = this.autoMapper.Map<List<PrcReportsSetUpConfigData_Result>, List<PrcReportsSetUpConfigData_ResultEntity>>(config);
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        public DashBoardEntity GetInboundDetailsDashboard(InboundDashboardCustomEntity obj)
        {
            int skipRows = (obj.currentPage - 1) * obj.pageSize;
            DashBoardEntity entity = new DashBoardEntity();
            DateTime fromdate = Convert.ToDateTime(obj.FromDate);
            DateTime todate = Convert.ToDateTime(obj.ToDate);
            if (obj.ResidentName == "")
                obj.ResidentName = null;
            var recordsCount = this.dbContext.PrcReportsGetInboundDetails(fromdate, todate, obj.ResidentName, obj.Status).Count();
            var records = (from pg in this.dbContext.PrcReportsGetInboundDetails(fromdate, todate, obj.ResidentName, obj.Status)
                           select new
                           {
                               File_CreatedDate = pg.File_CreatedDate != null ? Convert.ToDateTime(pg.File_CreatedDate).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                               File_Data = pg.File_Data.Replace("\n", string.Empty).Replace("\r","\n").Replace("\r\u001c", string.Empty),
                               FileAck_Data = pg.FileAck_Data.Replace("\r\n","\n").Replace("\v", string.Empty).Replace("\u001c\r\n", string.Empty).Replace("\u001c\r", string.Empty)
                           }).Skip(skipRows).Take(obj.pageSize).ToList();

            var columnNames = new List<DashboardColumnsEntity>();

            var columns = new DashboardColumnsEntity() { name = "File_CreatedDate", displayName = "File Created Date", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "File_Data", displayName = "File Data", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "FileAck_Data", displayName = "File Acknowledgement", display = true };
            columnNames.Add(columns);

            entity.ColumnNames = columnNames;
            entity.YaxisData = null;
            entity.XaxisData = null;
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        public DashBoardEntity GetOutboundDetailsDashboard(OutboundDashboardCustomEntity obj)
        {
            int skipRows = (obj.currentPage - 1) * obj.pageSize;
            DashBoardEntity entity = new DashBoardEntity();
            DateTime fromdate = Convert.ToDateTime(obj.FromDate);
            DateTime todate = Convert.ToDateTime(obj.ToDate);
            var recordsCount = this.dbContext.PrcReportsGetOutBoundErrorDetails(fromdate, todate).Count();
            var records = (from pg in this.dbContext.PrcReportsGetOutBoundErrorDetails(fromdate, todate)
                           select new
                           {
                               File_Data = pg.File_Data,
                               File_Acknowledge = pg.File_Acknowledge,
                               File_AcknowledgeDate = pg.File_AcknowledgeDate != null ? Convert.ToDateTime(pg.File_AcknowledgeDate).ToString("MM/dd/yyyy hh:mm:ss tt") : null,
                           }).Skip(skipRows).Take(obj.pageSize).ToList();

            var columnNames = new List<DashboardColumnsEntity>();

            var columns = new DashboardColumnsEntity() { name = "File_Data", displayName = "File Data", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "File_Acknowledge", displayName = "File Acknowledgement", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "File_AcknowledgeDate", displayName = "File Acknowledgement Date", display = true };
            columnNames.Add(columns);

            entity.ColumnNames = columnNames;
            entity.YaxisData = null;
            entity.XaxisData = null;
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        public DashBoardEntity GetPharmacyMedsExpiryDashboard(PharmacyMedsExpiryDateDashboardd obj)
        {
            if (obj.NusingStationId != "" && obj.NusingStationId != null && obj.NusingStationId != "null")
            {
                string[] strArray = obj.NusingStationId.Split(',');
                int nurseStationNewID = Convert.ToInt32(strArray[0]);
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = obj.UserId,
                    Facility_Id = (int)facilityId,
                    NurseStation_Id = obj.NusingStationId
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            int skipRows = (obj.currentPage - 1) * obj.pageSize;
            DashBoardEntity entity = new DashBoardEntity();
            DateTime checkinfromdate = Convert.ToDateTime(obj.CheckinFromDate);
            DateTime checkintodate = Convert.ToDateTime(obj.CheckinToDate);
            DateTime expirefromdate = Convert.ToDateTime(obj.ExpiryFromDate);
            DateTime expiretodate = Convert.ToDateTime(obj.ExpiryToDate);
            var recordsCount = this.dbContext.PrcReportsGetPharmacyExpirationDetails(checkinfromdate, checkintodate,expirefromdate,expiretodate,obj.NusingStationId,obj.UserId).Count();
            var records = (from pg in this.dbContext.PrcReportsGetPharmacyExpirationDetails(checkinfromdate, checkintodate, expirefromdate, expiretodate, obj.NusingStationId, obj.UserId)
                           select new
                           {
                               DrugName = pg.DrugName,
                               LotNumber = pg.LotNumber,
                               ResidentName = pg.ResidentName + " " + (pg.DOB != null ? "(" + Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") + ")" : ""),
                               QuantityReceived = pg.QuantityReceived,
                               Checkin_On = pg.Check_In_Date != null ? Convert.ToDateTime(pg.Check_In_Date).ToString("MM/dd/yyyy") : null,
                               ExpirationDate = pg.ExpirationDate != null ? Convert.ToDateTime(pg.ExpirationDate).ToString("MM/dd/yyyy") : null,
                           }).Skip(skipRows).Take(obj.pageSize).ToList();

            var columnNames = new List<DashboardColumnsEntity>();

            var columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Patient Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Checkin_On", displayName = "Most Recent Check-in Date", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "QuantityReceived", displayName = "Most Recent Quantity Received", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "ExpirationDate", displayName = "Most Recent Expiration Date", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "LotNumber", displayName = "Lot#", display = true };
            columnNames.Add(columns);

            entity.ColumnNames = columnNames;
            entity.YaxisData = null;
            entity.XaxisData = null;
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        //public DashBoardEntity GetEkitMedsExpiryDashboard(PharmacyMedsExpiryDateDashboardd obj)
        //{
            //if (obj.NusingStationId != "" && obj.NusingStationId != null && obj.NusingStationId != "null")
            //{
            //    string[] strArray = obj.NusingStationId.Split(',');
            //    int nurseStationNewID = Convert.ToInt32(strArray[0]);
            //    var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

            //    RecentFacEntity userRecentFacObj = new RecentFacEntity()
            //    {
            //        User_Id = obj.UserId,
            //        Facility_Id = (int)facilityId,
            //        NurseStation_Id = obj.NusingStationId
            //    };
            //    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            //}
            //int skipRows = (obj.currentPage - 1) * obj.pageSize;
            //DashBoardEntity entity = new DashBoardEntity();
            //DateTime checkinfromdate = Convert.ToDateTime(obj.CheckinFromDate);
            //DateTime checkintodate = Convert.ToDateTime(obj.CheckinToDate);
            //DateTime expirefromdate = Convert.ToDateTime(obj.ExpiryFromDate);
            //DateTime expiretodate = Convert.ToDateTime(obj.ExpiryToDate);
            //var recordsCount = this.dbContext.PrcReportsGetEkitExpirationDetails(checkinfromdate, checkintodate,expirefromdate,expiretodate,obj.NusingStationId,obj.FacilityId,obj.UserId).Count();
            //var records = (from pg in this.dbContext.PrcReportsGetEkitExpirationDetails(checkinfromdate, checkintodate, expirefromdate, expiretodate, obj.NusingStationId, obj.FacilityId, obj.UserId)
            //select new
            //               {
            //                   DrugName = pg.DrugName,
            //                   Lot_ = pg.Lot_,
            //                   Quantity_on_Hand = pg.Quantity_on_Hand,
            //                   Checkin_On = pg.Checkin_On != null ? Convert.ToDateTime(pg.Checkin_On).ToString("MM/dd/yyyy") : null,
            //                   ExpirationDate = pg.ExpirationDate != null ? Convert.ToDateTime(pg.ExpirationDate).ToString("MM/dd/yyyy") : null,
            //               }).Skip(skipRows).Take(obj.pageSize).ToList();

            //var columnNames = new List<DashboardColumnsEntity>();

            //var columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name ", display = true };
            //columnNames.Add(columns);
            //columns = new DashboardColumnsEntity() { name = "Checkin_On", displayName = "Most Recent Check-in Date", display = true };
            //columnNames.Add(columns);
            //columns = new DashboardColumnsEntity() { name = "Quantity_on_Hand", displayName = "Most Recent Quantity Received", display = true };
            //columnNames.Add(columns);
            //columns = new DashboardColumnsEntity() { name = "ExpirationDate", displayName = "Most Recent Expiration Date", display = true };
            //columnNames.Add(columns);
            //columns = new DashboardColumnsEntity() { name = "Lot_", displayName = "Lot#", display = true };
            //columnNames.Add(columns);


            //entity.ColumnNames = columnNames;
            //entity.YaxisData = null;
            //entity.XaxisData = null;
            //entity.GridData = records;
            //entity.TotalRecordsCount = recordsCount;
            //return entity;
            public DashBoardEntity GetEkitMedsExpiryDashboard(EKitMedsExpiryDateDashboard obj)
        {
            if (obj.NusingStationId != "" && obj.NusingStationId != null && obj.NusingStationId != "null")
            {
                string[] strArray = obj.NusingStationId.Split(',');
                int nurseStationNewID = Convert.ToInt32(strArray[0]);
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = obj.UserId,
                    Facility_Id = (int)facilityId,
                    NurseStation_Id = obj.NusingStationId
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            int skipRows = (obj.currentPage - 1) * obj.pageSize;
            DashBoardEntity entity = new DashBoardEntity();
            //DateTime checkinfromdate = Convert.ToDateTime(obj.CheckinFromDate);
            //DateTime checkintodate = Convert.ToDateTime(obj.CheckinToDate);
            //DateTime expirefromdate = Convert.ToDateTime(obj.ExpiryFromDate);
            //DateTime expiretodate = Convert.ToDateTime(obj.ExpiryToDate);
            string query = "[Admin].[PrcReportsGetEkitExpirationDetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    //cmd.Parameters.Add("@CheckinFromDate", SqlDbType.DateTime).Value = checkinfromdate;
                    //cmd.Parameters.Add("@CheckinToDate", SqlDbType.DateTime).Value = checkintodate;
                    //cmd.Parameters.Add("@ExpireFromDate", SqlDbType.DateTime).Value = expirefromdate;
                    // cmd.Parameters.Add("@ExpireToDate", SqlDbType.DateTime).Value = expiretodate;
                    cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = obj.NusingStationId;
                    cmd.Parameters.Add("@FacilityId", SqlDbType.Int).Value = obj.FacilityId;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = obj.UserId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }

            int recordsCount = ds.Tables[0].Rows.Count;
            var records = (from pg in ds.Tables[0].AsEnumerable()
                           select new EKitExpiration
                           {
                               NurseStation_Name = Convert.ToString(pg["NurseStationName"]),
                               Drugname = Convert.ToString(pg["DrugName"]),
                               BarcodeDetail = Convert.ToString(pg["Barcode"]),
                               LotNumber = Convert.ToString(pg["Lot#"]),
                               ExpiryDate = string.IsNullOrEmpty(pg["ExpirationDate"].ToString()) ? "" : Convert.ToDateTime(pg["ExpirationDate"]).ToString("MM/dd/yyyy"),
                               Expired = Convert.ToString(pg["Expired"]),
                               Quantity_on_Hand = Convert.ToString(pg["Quantity on Hand"]),
                           }).Skip(skipRows).Take(obj.pageSize).ToList();
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Drugname", displayName = "Drug Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "BarcodeDetail", displayName = "Barcode", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "LotNumber", displayName = "Lot#", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "ExpiryDate", displayName = "Exp Date", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Quantity_on_Hand", displayName = "Qty on Hand", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Expired", displayName = "Expired", display = true };
            columnNames.Add(columns);
            entity.ColumnNames = columnNames;
            entity.YaxisData = null;
            entity.XaxisData = null;
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }

        public DashBoardEntity GetScheduledOrderDashboard(ScheduleOrderEntity obj)
        {
            if (obj.NusingStationId != "" && obj.NusingStationId != null && obj.NusingStationId != "null")
            {
                string[] strArray = obj.NusingStationId.Split(',');
                int nurseStationNewID = Convert.ToInt32(strArray[0]);
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = obj.UserId,
                    Facility_Id = (int)facilityId,
                    NurseStation_Id = obj.NusingStationId
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            int skipRows = (obj.currentPage - 1) * obj.pageSize;
            DashBoardEntity entity = new DashBoardEntity();
            DateTime fromdate = Convert.ToDateTime(obj.FromDate);
            DateTime todate = Convert.ToDateTime(obj.ToDate);
            var recordsCount = this.dbContext.PrcReportsGetScheduleOrderDetails(obj.PatientIds, fromdate, todate, obj.OrderType, obj.UserId, obj.NusingStationId).Count();

            var records = (from pg in this.dbContext.PrcReportsGetScheduleOrderDetails(obj.PatientIds, fromdate, todate, obj.OrderType, obj.UserId, obj.NusingStationId)
                           select new
                           {
                               NurseStation_Name = pg.Nurse_Station_Name,
                               ResidentName = pg.Resident_Name + " (" + (pg.DOB != null ? Convert.ToDateTime(pg.DOB).ToString("MM/dd/yyyy") : null) + ")",
                               DrugName = pg.Order,
                               Startdate = pg.Start_Date != null ? Convert.ToDateTime(pg.Start_Date).ToString("MM/dd/yyyy") : null,
                               Gender=pg.Gender,
                               Location=pg.Location,
                               HOA=pg.HOA
                           }).Skip(skipRows).Take(obj.pageSize).ToList();
            var columnNames = new List<DashboardColumnsEntity>();

            var columns = new DashboardColumnsEntity() { name = "NurseStation_Name", displayName = "Nursing Station Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "ResidentName", displayName = "Resident Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Gender", displayName = "Gender", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Location", displayName = "Location", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "Startdate", displayName = "Start Date", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "HOA", displayName = "Administration Times", display = true };
            columnNames.Add(columns);

            entity.ColumnNames = columnNames;
            entity.YaxisData = null;
            entity.XaxisData = null;
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
        public DashBoardEntity GetMedicationActiveInventoryReportDashboard(GetMedicationActiveInventoryReportDashboard obj)
        {
            if (obj.NusingStationId != "" && obj.NusingStationId != null && obj.NusingStationId != "null")
            {
                string[] strArray = obj.NusingStationId.Split(',');
                int nurseStationNewID = Convert.ToInt32(strArray[0]);
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationNewID).Select(n => n.Facility_Id).FirstOrDefault();

                RecentFacEntity userRecentFacObj = new RecentFacEntity()
                {
                    User_Id = obj.UserId,
                    Facility_Id = (int)facilityId,
                    NurseStation_Id = obj.NusingStationId
                };
                this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
            }
            int skipRows = (obj.currentPage - 1) * obj.pageSize;
            DashBoardEntity entity = new DashBoardEntity();
            string query = "[Admin].[PrcReportsGetEKitDetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = obj.NusingStationId;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = obj.Status;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = obj.UserId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            int recordsCount = ds.Tables[0].Rows.Count;
            var records = (from pg in ds.Tables[0].AsEnumerable()
                           select new
                           {
                               NurseStationName = Convert.ToString(pg["Nurse Station Name"]),
                               DrugName = Convert.ToString(pg["DrugName"]),
                               QuantityonHand = Convert.ToString(pg["Quantity on Hand"]),
                               assessedon = string.IsNullOrEmpty(pg["assessedon"].ToString()) ? "" : Convert.ToDateTime(pg["assessedon"]).ToString("MM/dd/yyyy hh:mm:ss tt"),
                           }).Skip(skipRows).Take(obj.pageSize).ToList();
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity() { name = "NurseStationName", displayName = "Nursing Station", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "DrugName", displayName = "Drug Name", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "QuantityonHand", displayName = "Qty on Hand", display = true };
            columnNames.Add(columns);
            columns = new DashboardColumnsEntity() { name = "assessedon", displayName = "Assessed On", display = true };
            columnNames.Add(columns);
            entity.ColumnNames = columnNames;
            entity.YaxisData = null;
            entity.XaxisData = null;
            entity.GridData = records;
            entity.TotalRecordsCount = recordsCount;
            return entity;
        }
    }
}
