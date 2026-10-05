using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using LTCPro.Entities;
using LTCPro.Repositories;
using LTCPro.ServiceLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;


namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [CustomAuthorizationFilter]
    [RoutePrefix("Reports")]
    public class ReportsController : ApiController
    {
        private readonly IReportsService _reportsService;
        private IDashboardsService _dashboardService;
        private readonly ILogger _log;
        private readonly IAllergiesICDService _allergiesICDService;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IFacilityService _facilityService;
        enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        public ReportsController(IAllergiesICDService allergiesICDService, IReportsService reportsService, IDashboardsService dashboardService, ILogger log, IUserActivityRepository userActivityRepository,IFacilityService facilityService)
        {
            this._allergiesICDService = allergiesICDService;

            this._reportsService = reportsService;
            this._dashboardService = dashboardService;
            this._log = log;
            this._userActivityRepository = userActivityRepository;
            this._facilityService = facilityService;
        }
        [Route("GetReports")]
        [HttpGet]
        public IHttpActionResult GetReports()
        {
            this._log.Debug("---Executing GetReports() in ReportsController----");
            var records = this._reportsService.GetReports().Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptDashboardReport.rpt";
            rd.Load(reportsPath);
            rd.SetDataSource(records);
            rd.SetParameterValue("Facility Name", "KIMS");
            rd.SetParameterValue("ReportName", "Vital DashBoard Report");
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ICD10" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetReports() in ReportsController----");
            return responseMessageResult;

        }

        [Route("GetCompanyReports/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetCompanyReports(string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetCompanyReports() in ReportsController----");
            var records = this._reportsService.GetCompanyReports().Result;
            var emarlogo = this._reportsService.GetEmarlogo().Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCompanyReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptEmarlogoReport.rpt - 01").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 120)
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            // rd.SetParameterValue("Facility Name", "KIMS");
            rd.SetParameterValue("ReportName", "Company Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "CompanyReports" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetCompanyReports() in ReportsController----");
            return responseMessageResult;
        }

        [Route("GetFacilityReports/{companyId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetFacilityReports(int companyId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetFacilityReports() in ReportsController----");
            var records = this._reportsService.GetFacilityReports(companyId).Result;
            var emarlogo = this._reportsService.GetEmarlogo().Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptFacilityReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptEmarlogoReport.rpt - 01").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 120)
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Facility Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "FacilityReports" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetFacilityReports() in ReportsController----");
            return responseMessageResult;
        }

        [Route("GetNursestationReports/{companyId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetNursestationReports(int companyId, int facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetNursestationReports() in ReportsController----");
            var records = this._reportsService.GetNursestationReports(companyId, facilityId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptNurseStationReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Nursing Station Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "NurseStationReport" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetNursestationReports() in ReportsController----");
            return responseMessageResult;
        }


        #region DeleteIfNotNecessary
        [Route("GetCensusReports/{userId}/{fromDate}/{toDate}")]
        [HttpGet]
        public IHttpActionResult GetCensusReports(int userId, string fromDate, string toDate)
        {
            this._log.Debug("---Executing GetCensusReports() in ReportsController----");
            var records = this._reportsService.GetCensusReports(userId, fromDate, toDate).Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(0, 0).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCensusReport.rpt";

            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Census Report");
            var fromdate = Convert.ToDateTime(fromDate).GetDateTimeFormats()[3];
            var todate = Convert.ToDateTime(toDate).GetDateTimeFormats()[3];
            if (fromDate == "null" && toDate == "null")
            {

                fromDate = "";
                toDate = "";
                rd.SetParameterValue("FromDate", fromdate);
                rd.SetParameterValue("ToDate", todate);
            }
            else
            {
                rd.SetParameterValue("FromDate", "From :" + fromdate);
                rd.SetParameterValue("ToDate", "To :" + todate);
            }
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "CensusReport" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetCensusReports() in ReportsController----");
            return responseMessageResult;
        }

        #endregion


        [Route("GetFloorReport/{companyId}/{facilityId}/{nursestationId}/{FloorId}/{WingId}/{RoomId}/{BedId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetFloorReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetFloorReport() in ReportsController----");
            var records = this._reportsService.GetFloorReport(companyId, facilityId, nursestationId,floorId,wingId,roomId,bedId ).Result;
            var emarlogo = this._reportsService.GetEmarlogo(companyId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(0, companyId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptFloorReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptEmarlogoReport.rpt - 01").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 120)
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Floor Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Floor" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetFloorReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetWingReport/{companyId}/{facilityId}/{nursestationId}/{FloorId}/{WingId}/{RoomId}/{BedId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetWingReport(int companyId, int facilityId, int nursestationId, int floorId,int wingId,int roomId, int bedId,string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetWingReport() in ReportsController----");
            var records = this._reportsService.GetWingReport(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId ).Result;
            var emarlogo = this._reportsService.GetEmarlogo(companyId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(0, companyId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptWingReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptEmarlogoReport.rpt - 01").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 120)
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Wing Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Wing" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetWingReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetRoomReport/{companyId}/{facilityId}/{nursestationId}/{FloorId}/{WingId}/{RoomId}/{BedId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetRoomReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetRoomReport() in ReportsController----");
            var records = this._reportsService.GetRoomReport(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId ).Result;
            var emarlogo = this._reportsService.GetEmarlogo(companyId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(0, companyId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptRoomReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptEmarlogoReport.rpt - 01").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 120)
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Room Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Room" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetRoomReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetBedReport/{companyId}/{facilityId}/{nursestationId}/{FloorId}/{WingId}/{RoomId}/{BedId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetBedReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetBedReport() in ReportsController----");
            var records = this._reportsService.GetBedReport(companyId, facilityId, nursestationId, floorId, wingId, roomId,bedId ).Result;
            var emarlogo = this._reportsService.GetEmarlogo(companyId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(0, companyId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptBedReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptEmarlogoReport.rpt - 01").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 120)
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Bed Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Bed" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetBedReport() in ReportsController----");
            return responseMessageResult;
        }

        [Route("GetCensusReportByfilter/{fromdate}/{todate}/{userId}/{nursestationId}/{type}/{reportType}/{category}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetCensusReportByfilter(string fromdate, string todate, int userId, string nursestationId, int type, int reportType, int category, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetCensusReportByfilter() in ReportsController----");
            var records = this._reportsService.GetCensusReportByfilter(fromdate, todate, userId, nursestationId, type, reportType).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCensusCustomDataReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Census Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            if (category == 1)
            {
                rd.SetParameterValue("ReportCategory", "Category : Active Residents");
            }
            if (category == 2)
            {
                rd.SetParameterValue("ReportCategory", "Category : Discharge Residents");
            }
            if (category == 3)
            {
                rd.SetParameterValue("ReportCategory", "Category : Census with Allergy");
            }
            if (type == 1)
            {
                rd.SetParameterValue("SelectType", "Type : Month");
            }
            if (type == 2)
            {
                rd.SetParameterValue("SelectType", "Type : Quarter");
            }
            if (type == 3)
            {
                rd.SetParameterValue("SelectType", "Type : Year");
            }
            if (type == 4)
            {
                rd.SetParameterValue("SelectType", "Type : Compare");
            }
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From :" + fromDate);
            rd.SetParameterValue("ToDate", "To :" + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Census" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetCensusReportByfilter() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetCensusReportByDates/{fromdate}/{todate}/{userId}/{nursestationname}/{type}/{reportType}/{value}/{category}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetCensusReportByDates(string fromdate, string todate, int userId, string nursestationname, int type, int reportType, string value, int category, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetCensusReportByDates() in ReportsController----");
            var records = this._reportsService.GetCensusReportByDates(fromdate, todate, userId, nursestationname, type, reportType, value).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            if (type == 2 || type == 3)
            {
                ReportDocument rd = new ReportDocument();
                string customreportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCensusCustomfilterReport.rpt";
                rd.Load(customreportsPath);
                rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (image.Size.Width >= image.Size.Height + 120)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
                rd.SetDataSource(records.Tables[0]);
                rd.SetParameterValue("ReportName", "Census Report");
                rd.SetParameterValue("DateTime", reportDateTime);
                if (category == 1)
                {
                    rd.SetParameterValue("ReportCategory", "Category : Active Residents");
                }
                if (category == 2)
                {
                    rd.SetParameterValue("ReportCategory", "Category : Discharge Residents");
                }
                if (category == 3)
                {
                    rd.SetParameterValue("ReportCategory", "Category : Census with Allergy");
                }
                if (type == 1)
                {
                    rd.SetParameterValue("Type", "Type : Month");
                }
                if (type == 2)
                {
                    rd.SetParameterValue("Type", "Type : Quarter");
                }
                if (type == 3)
                {
                    rd.SetParameterValue("Type", "Type : Year");
                }
                if (type == 4)
                {
                    rd.SetParameterValue("Type", "Type : Compare");
                }
                var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
                var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
                rd.SetParameterValue("FromDate", "From :" + fromDate);
                rd.SetParameterValue("ToDate", "To :" + toDate);
                rd.SetParameterValue("DateTime", reportDateTime);
                Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                MemoryStream ms = new MemoryStream();

                s.CopyTo(ms);
                rd.Close();
                rd.Dispose();

                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };

                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Census" + DateTime.Now.ToString("yyyy/MM/dd")
                };
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                this._log.Debug("---Executed Successfully GetCensusReportByDates() in ReportsController----");
                return responseMessageResult;
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCensusCustomDatesReport.rpt";
                rd.Load(reportsPath);
                rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (image.Size.Width >= image.Size.Height + 120)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
                rd.SetDataSource(records.Tables[0]);
                rd.SetParameterValue("ReportName", "Census Report");
                if (category == 1)
                {
                    rd.SetParameterValue("ReportCategory", "Category : Active Residents");
                }
                if (category == 2)
                {
                    rd.SetParameterValue("ReportCategory", "Category : Discharge Residents");
                }
                if (category == 3)
                {
                    rd.SetParameterValue("ReportCategory", "Category : Census with Allergy");
                }
                if (type == 1)
                {
                    rd.SetParameterValue("Type", "Type : Month");
                }
                if (type == 2)
                {
                    rd.SetParameterValue("Type", "Type : Quarter");
                }
                if (type == 3)
                {
                    rd.SetParameterValue("Type", "Type : Year");
                }
                if (type == 4)
                {
                    rd.SetParameterValue("Type", "Type : Compare");
                }
                var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
                var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
                rd.SetParameterValue("FromDate", "From :" + fromDate);
                rd.SetParameterValue("ToDate", "To :" + toDate);
                rd.SetParameterValue("DateTime", reportDateTime);
                Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                MemoryStream ms = new MemoryStream();

                s.CopyTo(ms);
                rd.Close();
                rd.Dispose();

                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };

                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Census" + DateTime.Now.ToString("yyyy/MM/dd")
                };
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                this._log.Debug("---Executed Successfully GetCensusReportByDates() in ReportsController----");
                return responseMessageResult;
            }
        }
        [Route("GetAllAllergiesByClassReport/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetAllAllergiesByClassReport(int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetAllAllergiesByClassReport() in ReportsController----");
            var records = this._reportsService.GetAllAllergiesByClassReport().Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptAllergyByClassReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Allergy By Class Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "AllergyByClass" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetAllAllergiesByClassReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetAllAllergiesByDrugReport/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetAllAllergiesByDrugReport(int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetAllAllergiesByDrugReport() in ReportsController----");
            var records = this._reportsService.GetAllAllergiesByDrugReport().Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptAllergyByDrugReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "Allergy By Drug Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "AllergyByDrug" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetAllAllergiesByDrugReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetAllICD10Report/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetAllICD10Report(int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetAllICD10Report() in ReportsController----");
            var records = this._reportsService.GetAllICD10Report().Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptICD10Report.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records);
            rd.SetParameterValue("ReportName", "ICD10 Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ICD10" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            return responseMessageResult;
        }
        /* [Route("ExportAllICD10ToPDF/{ModuleName}/{UserId}/{FromDate}/{ToDate}/{FilterType}/{ReportType}/{SeriesName?}/{Yaxis?}/{Category?}")]
         [HttpGet]
         public IHttpActionResult ExportDashboardToPdf(string moduleName, int userId, string fromDate, string toDate, int filterType, int reportType, string seriesName = "", string yAxis = "", string category = "")
         {
             this._log.Debug("---Executing ExportDashboardToPdf() in ReportsController----");
             var records = this._dashboardService.GetCensusDashboard(moduleName, seriesName, yAxis, category, userId, fromDate, toDate, filterType, reportType, null).Result;
             //DataTable dt = this._allergiesICDService.ToDataTable<SeriesDataEntity>(records.YaxisData.ToList());

             string templateUrl = HttpContext.Current.Server.MapPath("~") + "\\Controllers\\PdfTemplate.html";

             string htmlTemplate;
             using (StreamReader reader = new StreamReader(templateUrl))
             {
                 htmlTemplate = reader.ReadToEnd();
             }
             string cssUrl = HttpContext.Current.Server.MapPath("~") + "\\Scripts\\adminlte.css";

             string cssFile;
             using (StreamReader reader = new StreamReader(cssUrl))
             {
                 cssFile = reader.ReadToEnd();
             }
             using (StringWriter sw = new StringWriter())
             {
                 using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                 {
                     StringBuilder sb = new StringBuilder();

                     //Generate Header.
                     //id = "tbltable" class="table table-bordered table-hover"
                     //sb.Append("<table width='100%' cellspacing='0' cellpadding='0' style='font-size:8px;'>");
                     //sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Census Report</b></td></tr>");
                     //sb.Append("<tr><td colspan = '2'></td></tr>");
                     //sb.Append("</table>");
                     // sb.Append("<br />");
                     sb.Append("<table id = 'tbltable' class='table table-bordered table-hover'>");
                     sb.Append("<thead>");
                     sb.Append("<tr>");

                     foreach (var column in records.ColumnNames)
                     {
                         sb.Append("<th>");
                         sb.Append(column.displayName);
                         sb.Append("</th>");
                     }
                     sb.Append("</tr>");
                     sb.Append("</thead>");
                     foreach (var row in records.YaxisData)
                     {
                         sb.Append("<tr>");
                         sb.Append("<td>");
                         sb.Append(row.name);
                         sb.Append("</td>");
                         foreach (var d in row.data)
                         {
                             sb.Append("<td>");
                             sb.Append(d);
                             sb.Append("</td>");
                         }

                         sb.Append("</tr>");
                     }

                     sb.Append("</table>");

                     htmlTemplate = htmlTemplate.Replace("##TablerData##", sb.ToString());
                     //Export HTML String as PDF.
                     StringReader sr = new StringReader(htmlTemplate);// sb.ToString());


                     Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                     //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                     MemoryStream stream = new MemoryStream();

                     PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                     pdfDoc.Open();

                     //using (var myCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssFile)))
                     //{
                     //    using (var myheml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlTemplate)))
                     //    {
                     //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, myheml, myCss);
                     //    }

                     //    //XMLWorkerHelper.GetCSS(myCss);

                     //}
                     //htmlparser.Parse(sr);

                     var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                     tagProcessors.RemoveProcessor(HTML.Tag.IMG);
                     tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor());


                     CssFilesImpl cssFiles = new CssFilesImpl();
                     cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                     var cssResolver = new StyleAttrCSSResolver(cssFiles);
                     cssResolver.AddCss(cssFile, "utf-8", true);
                     var charset = Encoding.UTF8;
                     var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                     hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                     var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(pdfDoc, writer));
                     var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                     var worker = new XMLWorker(pipeline, true);
                     var xmlParser = new XMLParser(true, worker, charset);
                     xmlParser.Parse(new StringReader(htmlTemplate));

                     HeaderFooter ev = new HeaderFooter();
                     Rectangle rect = new Rectangle(36, 54, 559, 788);
                     writer.SetBoxSize("art", rect);
                     writer.PageEvent = ev;

                     pdfDoc.Close();

                     byte[] content = stream.ToArray();

                     HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                     {
                         Content = new ByteArrayContent(content)
                     };

                     httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                     {
                         FileName = "AllergyByDrug_" + DateTime.Now.ToString("yyyy/MM/dd")
                     };
                     httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                     ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                     this._log.Debug("---Executing ExportDashboardToPdf() in ReportsController----");
                     return responseMessageResult;

                 }

             }
         }
         */
        [Route("Get72HoursReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult Get72HoursReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing Get72HoursReport() in ReportsController----");
            var records = this._reportsService.Get72HoursReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crpt72HoursReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length>0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));

                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "72 Hour Follow-Up Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "72 Hours" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully Get72HoursReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetPRNDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetPRNDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            try
            {
              //  HttpContext.Current.Session["SomeData"] = "";


            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetPRNDetailsReport() in ReportsController----");
                this._log.Debug("---facilityId----"+ facilityId.ToString());
                var records = this._reportsService.GetPRNDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
                this._log.Debug(emarlogo);
                var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
                this._log.Debug(facilitylogo);
                ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptPRNDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "PRN Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Prndr" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetPRNDetailsReport() in ReportsController----");
            return responseMessageResult;
            }
            catch(Exception ex)
            {
                this._log.Debug("---Report Issue");
                this._log.Debug(ex.Message.ToString());
                this._log.Debug(ex.InnerException.Message.ToString());
                return null;
            }
        }
        [Route("GetOrderDetailsReport")]
        [HttpPost]
        public IHttpActionResult GetOrderDetailsReport(ScheduleOrderEntity entity)
        {
            var reportDateTime = entity.DateTime.Replace('-', '/').Replace(',', ':');
            //var passtime = time.Replace('-', ':');
            //int shiftTimes = 0;
            //if (shiftTime == "null")
             //   shiftTimes = 0;
            //else
             //   shiftTimes = Convert.ToInt32(shiftTime);
            this._log.Debug("---Executing GetOrderDetailsReport() in ReportsController----");
            var records = this._reportsService.GetOrderDetailsReport(entity.PatientIds, entity.FromDate, entity.ToDate,Convert.ToInt32(entity.OrderType), entity.UserId, entity.NusingStationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(entity.FacilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, entity.FacilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOrderDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Scheduled Order Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            if (entity.OrderType == 1)
            {
                rd.SetParameterValue("ReportCategory", "Order Type: Drug");
            }
            if (entity.OrderType == 2)
            {
                rd.SetParameterValue("ReportCategory", "Order Type: Literal");
            }
            if (entity.OrderType == 0)
            {
                rd.SetParameterValue("ReportCategory", "Order Type: Drug & Literal");
            }
            var fromDate = Convert.ToDateTime(entity.FromDate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(entity.ToDate).GetDateTimeFormats()[3];
                rd.SetParameterValue("Type", "");
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Order" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetOrderDetailsReport() in ReportsController----");
            return responseMessageResult;

        }

        [Route("GetWithoutBarcodeDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetWithoutBarcodeDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetWithoutBarcodeDetailsReport() in ReportsController----");
            var records = this._reportsService.GetWithoutBarcodeDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptBarcodeDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Orders Without Barcode Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "BarCode" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetWithoutBarcodeDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetBiometricsDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetBiometricsDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetBiometricsDetailsReport() in ReportsController----");
            var records = this._reportsService.GetBiometricsDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptBypassReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Biometric Bypass Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "BarCode" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetBiometricsDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOrderControlSignoffReport/{fromdate}/{todate}/{nursestationId}/{userId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetOrderControlSignoffReport(string fromdate, string todate, string nursestationId, int userId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetOrderControlSignoffReport() in ReportsController----");
            var records = this._reportsService.GetOrderControlSignoffReport(fromdate, todate, nursestationId, userId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOrderControlSubstanceReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Controlled Medication Sign-Off  Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "OrderControlSignoff" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetOrderControlSignoffReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOrderControlSubstanceReport/{nursestationId}/{userId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetOrderControlSubstanceReport(string nursestationId, int userId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetOrderControlSubstanceReport() in ReportsController----");
            var records = this._reportsService.GetOrderControlSubstanceReport(nursestationId, userId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOrderControlSignoffReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Orders for Controlled Substances Report");
            rd.SetParameterValue("DateTime", reportDateTime);

            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "OrderControlSubstance" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetOrderControlSubstanceReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOrderHoldDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetOrderHoldDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetOrderHoldDetailsReport() in ReportsController----");
            var records = this._reportsService.GetOrderHoldDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOrderHoldDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Orders on Hold Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "OrderHoldDetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetOrderHoldDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOrderWithFavouritesReport/{time}/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{shiftTime}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetOrderWithFavouritesReport(string time, string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string shiftTime, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            var passtime = time.Replace('-', ':');
            int shiftTimes = 0;
            if (shiftTime == "null")
                shiftTimes = 0;
            else
                shiftTimes = Convert.ToInt32(shiftTime);
            this._log.Debug("---Executing GetOrderWithFavouritesReport() in ReportsController----");
            var records = this._reportsService.GetOrderWithFavouritesReport(passtime, fromdate, todate, userId, nursestationId, shiftTimes).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOrderWithFavouritesReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Measurements and Other Checks Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            if (passtime == "null")
            {
                rd.SetParameterValue("Passtime", "");
            }
            else
            {
                rd.SetParameterValue("Passtime", "Pass Time:"+" " + passtime);
            }
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Order" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetOrderWithFavouritesReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetCompareCensusDataReport/{year}/{userId}/{nursestationId}/{type}/{reportType}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetCompareCensusDataReport(string year, int userId, string nursestationId, int type, int reportType, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetCompareCensusDataReport() in ReportsController----");
            var records = this._reportsService.GetCompareCensusDataReport(year, userId, nursestationId, type, reportType).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCensusDashboardCompareReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Census Compare Report");
            rd.SetParameterValue("ReportCategory", "Years:" + year);
            rd.SetParameterValue("DateTime", reportDateTime);

            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Census compare" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetCompareCensusDataReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetRefusedByResidentDetailsReport")]
        [HttpPost]
        public IHttpActionResult GetRefusedByResidentDetailsReport(MedRefEntity data)
        {
            var reportDateTime = data.dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetRefusedByResidentDetailsReport() in ReportsController----");
            var records = this._reportsService.GetRefusedByResidentDetailsReport(data.fromdate, data.todate, data.userId, data.nursingstationId, data.passTime,data.dashboardName).Result;
            var emarlogo = this._reportsService.GetEmarlogo(data.facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null,data.facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptRefusedByResidenReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Orders by Therapeutic Classification Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(data.fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(data.todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Refused" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetRefusedByResidentDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetMedrefReport")]
        [HttpPost]
        public IHttpActionResult GetMedrefReport(MedRefEntity data)
        {
            var reportDateTime = data.dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetMedrefReport() in ReportsController----");
            var records = this._reportsService.GetMedrefReport(data.fromdate, data.todate, data.userId, data.nursingstationId, data.passTime).Result;
            var emarlogo = this._reportsService.GetEmarlogo(data.facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, data.facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptMedref.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Medication Refusal Report with Custom Dates");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(data.fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(data.todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:" + " " + fromDate);
            rd.SetParameterValue("ToDate", "To:" + " " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Refused" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetMedrefReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOrderChangeDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{residentId}/{facilityId}/{residentStatus}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetOrderChangeDetailsReport(string fromdate, string todate, int userId, string nursestationId, string residentId, int facilityId, int residentStatus, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing  GetOrderChangeDetailsReport() in ReportsController----");
            var records = this._reportsService.GetOrderChangeDetailsReport(fromdate, todate, userId, nursestationId, residentId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOrderChangeDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Order Change Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            if (residentStatus == 1)
            {
                rd.SetParameterValue("residentStatus", "Resident Status: Active");
            }
            else if (residentStatus == 2)
            {
                rd.SetParameterValue("residentStatus", "Resident Status: Inactive");
            }
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "orderchange" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully  GetOrderChangeDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOrderChangeDetailsGrid/{fromdate}/{todate}/{userId}/{nursestationId}/{residentId}")]
        [HttpGet]
        public JsonResult<EMARResidentEntity> GetOrderChangeDetailsGrid(string fromdate, string todate, int userId, string nursestationId, string residentId)
        {
            this._log.Debug("---Executing GetOrderChangeDetailsGrid() in ReportsController----");
            var records = this._reportsService.GetOrderChangeDetailsReport(fromdate, todate, userId, nursestationId, Convert.ToString(residentId)).Result;

            var entity = new EMARResidentEntity();
            records.Tables[0].Columns.Add("Resident Name", typeof(string), "[PatientName]+' ('+[DOB]+')'");

            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "NursingStationName", "Resident Name", "DrugName", "Barcode", "ColumnName", "OldValue", "NewValue", "UpdatedBy", "UpdatedOn");
            var columnsList = newTable.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity();
            foreach (var item in columnsList)
            {
                if (item == "NursingStationName")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Nursing Station Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "Resident Name")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Resident Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "DrugName")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Drug Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "Barcode")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Barcode", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "ColumnName")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Column Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "OldValue")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Old Value", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "NewValue")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "New Value", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "UpdatedBy")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Updated By", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "UpdatedOn")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Updated On", display = true };
                    columnNames.Add(columns);
                }
                else
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = item, display = true };
                    columnNames.Add(columns);
                }
            }
            entity.ColumnNames = columnNames;
            entity.GridData = records.Tables[0];
            return Json<EMARResidentEntity>(entity);
        }
        [Route("GetWithoutScanningDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetWithoutScanningDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            this._log.Debug("---Executing GetWithoutScanningDetailsReport() in ReportsController----");
            var records = this._reportsService.GetWithoutScanningDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptWithoutScanningReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Orders Administered without Scanning");
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "scanning" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetWithoutScanningDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetDestructionDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetDestructionDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetDestructionDetailsReport() in ReportsController----");
            var records = this._reportsService.GetDestructionDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptDestructionDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Medication Destruction Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Destruction" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetDestructionDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetFloorStockDetailsReport/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetFloorStockDetailsReport(int userId, string nursestationId, int facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetFloorStockDetailsReport() in ReportsController----");
            var records = this._reportsService.GetFloorStockDetailsReport(userId, nursestationId, facilityId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptNewFloorStockReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Floor Stock Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Floorstock" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetFloorStockDetailsReport() in ReportsController----");
            return responseMessageResult;
        }

        [Route("GetAdminUsersDetailsReport/{companyId}/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetAdminUsersDetailsReport(string companyId, int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetAdminUsersDetailsReport() in ReportsController----");
            var records = this._reportsService.GetAdminUsersDetailsReport(companyId, userId).Result;
            var emarlogo = this._reportsService.GetEmarlogo().Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptAdminUsersDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            if (companyId.Contains(','))
            {
                var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image Headerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (Headerimage.Size.Width >= Headerimage.Size.Height + 150)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                var facilitylogo = this._reportsService.GetHeaderlogo(Convert.ToInt32(companyId), null).Result;
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image Headerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (Headerimage.Size.Width >= Headerimage.Size.Height + 150)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }

            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "List of Users Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "AdminUsers" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetAdminUsersDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetSetUpConfigDetailsReport/{companyId}/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetSetUpConfigDetailsReport(string companyId, int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetSetUpConfigDetailsReport() in ReportsController----");
            var records = this._reportsService.GetSetUpConfigDetailsReport(companyId).Result;
            var emarlogo = this._reportsService.GetEmarlogo().Result;
            // var facilitylogo = this._reportsService.GetHeaderlogo(null,null).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptSetUpConfigDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            if (companyId.Contains(','))
            {
                var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image Headerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (Headerimage.Size.Width >= Headerimage.Size.Height + 150)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                var facilitylogo = this._reportsService.GetHeaderlogo(Convert.ToInt32(companyId), null).Result;
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image Headerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (Headerimage.Size.Width >= Headerimage.Size.Height + 150)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Setup Configuration Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "AdminUsers" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetAdminUsersDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetAverageCensusDataReport/{year}/{month}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetAverageCensusDataReport(int year, int month, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {

            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetAverageCensusDataReport() in ReportsController----");
            var records = this._reportsService.GetAverageCensusDataReport(year, month, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptAverageCensusDataReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Average Census Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("ReportCategory", "Category: Average Census");
            rd.SetParameterValue("Year", "Year:"+" " + year);
            rd.SetParameterValue("Month", "Month:"+" " + Enum.GetName(typeof(Months), month));
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "average census" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetAverageCensusDataReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetPharmacyMedsDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetPharmacyMedsDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetPharmacyMedsDetailsReport() in ReportsController----");
            var records = this._reportsService.GetPharmacyMedsDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptPharmacyMedsDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Medication Check-in Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "pharmacymed's" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetPharmacyMedsDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetPsychiatricDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetPsychiatricDetailsReport(string fromdate, string todate, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetPsychiatricDetailsReport() in ReportsController----");
            var records = this._reportsService.GetPsychiatricDetailsReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptPsychiatricDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Psychiatric Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Psychiatric" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetPsychiatricDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetNurseNotesDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{commentType}/{facilityId}/{dateTime}/{MedicationReason?}/{MedicationReasonText?}")]
        [HttpGet]
        public IHttpActionResult GetNurseNotesDetailsReport(string fromdate, string todate, int userId, string nursestationId, string commentType, Nullable<int> facilityId, string dateTime,string MedicationReason = "",string MedicationReasonText="")
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetNurseNotesDetailsReport() in ReportsController----");
            var records = this._reportsService.GetNurseNotesDetailsReport(fromdate, todate, userId, nursestationId, commentType,MedicationReason).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptNurseNotesDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Nurses' Notes Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            if (commentType == "1")
            {
                rd.SetParameterValue("CommentType", "CommentType: eMAR Comments");
            }
            if (commentType == "2")
            {
                rd.SetParameterValue("CommentType", "CommentType: PRN Comments");
            }
            if (commentType == "3")
            {
                rd.SetParameterValue("CommentType", "CommentType: Seventy two hours Comments");
            }
            rd.SetParameterValue("Medication", "Medication Reason:"+MedicationReasonText);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "NurseNotes" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetNurseNotesDetailsReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetMARHoledetails")]
        [HttpPost]
        public IHttpActionResult GetMARHoledetails(EmarReportEntity entity)
        {
            var reportDateTime = entity.DateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetMARHoledetails() in ReportsController----");
            if (entity.NursingstationId == "all")
                entity.NursingstationId = null;
            if (entity.patientName == "all")
                entity.patientName = null;
            var records = this._reportsService.GetMARHoledetails(entity.Month, entity.Year, entity.NursingstationId, entity.UserId, entity.patientName).Result;
            var emarlogo = this._reportsService.GetEmarlogo(entity.FacilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, entity.FacilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportspath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptMARHoledetailsReport.rpt";
            rd.Load(reportspath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "MAR Hole Details Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("Month", "Month:"+" " + Enum.GetName(typeof(Months), entity.Month));
            rd.SetParameterValue("Year", "Year:"+" " + entity.Year);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "MARHoledetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = new ResponseMessageResult(httpResponseMessage);
            this._log.Debug("---Executing GetMARHoledetails() successfully in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetMARHistorydetails")]
        [HttpPost]
        public IHttpActionResult GetMARHistorydetails(EmarReportEntity entity)
        {
            var reportDateTime = entity.DateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetMARHistorydetails() in ReportsController----");
            if (entity.NursingstationId == "all")
                entity.NursingstationId = null;
            if (entity.patientName == "all")
                entity.patientName = null;
            var records = this._reportsService.GetMARHistorydetails(entity.Month, entity.Year, entity.NursingstationId, entity.UserId, entity.patientName).Result;
            var emarlogo = this._reportsService.GetEmarlogo(entity.FacilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, entity.FacilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportspath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptMARHistorydetailsReport.rpt";
            rd.Load(reportspath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "MAR History Details Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("Month", "Month:"+" " + Enum.GetName(typeof(Months), entity.Month));
            rd.SetParameterValue("Year", "Year:"+" " + entity.Year);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "MARHistorydetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = new ResponseMessageResult(httpResponseMessage);
            this._log.Debug("---Executed successfully GetMARHistorydetails() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetOutBoundErrorDetails/{fromdate}/{todate}/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetOutBoundErrorDetails(string fromdate, string todate, int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetOutBoundErrorDetails() in ReportsController----");
            var records = this._reportsService.GetOutBoundErrorDetails(fromdate, todate).Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
            ReportDocument rd = new ReportDocument();
            string reportspath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptOutBoundErrorDetailsReport.rpt";
            rd.Load(reportspath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "OutBound Error Details Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponsemessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            httpResponsemessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "OutBoundErrorDetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponsemessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponsemessage);
            this._log.Debug("---Executed Successfully GetOutBoundErrorDetails() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetInboundDetails/{fromdate}/{todate}/{inboundStatus}/{userId}/{dateTime}/{facilityId}/{residentName}")]
        [HttpGet]
        public IHttpActionResult GetInboundDetails(string fromdate, string todate, int inboundStatus, int userId, string dateTime, int facilityId, string residentName)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetInboundDetails() in ReportsController----");
            var records = this._reportsService.GetInboundDetails(fromdate, todate, residentName, inboundStatus).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportspath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptInboundDetailsReport.rpt";
            rd.Load(reportspath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "HL7 Inbound Details Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            if (residentName == "null")
            {
                rd.SetParameterValue("ResidentName", "");
            }
            else
            {
                rd.SetParameterValue("ResidentName", "ResidentName: " + residentName);
            }
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "InBoundDetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetInboundDetails() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetPrescriberNotesReport/{fromdate}/{todate}/{userId}/{nursestationId}")]
        [HttpGet]
        public IHttpActionResult GetPrescriberNotesReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetPrescriberNotesReport() in ReportsController----");
            var records = this._reportsService.GetPrescriberNotesReport(fromdate, todate, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptPrescriberNotesReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Prescriber Notes Report");
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From :" + fromDate);
            rd.SetParameterValue("ToDate", "To :" + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "PrescriberNotes" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetPrescriberNotesReport() in ReportsController----");
            return responseMessageResult;

        }
        [Route("GetEmarResidentdetailsReport")]
        [HttpPost]
        public IHttpActionResult GetEmarResidentdetailsReport(EmarReportEntity entity)
        {
            var reportDateTime = entity.DateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetEmarResidentdetailsReport() in ReportsController----");
            if (entity.NursingstationId == "all")
                entity.NursingstationId = null;
            if (entity.patientName == "all")
                entity.patientName = null;
            string legend = this._reportsService.GetEmarResidentlegendReport(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.UserId).Result;
            var records = this._reportsService.GetEmarResidentdetailsReport(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.UserId).Result;
            var recordAllDia = this._reportsService.GetEmarResidentdetailsReportAllergyAndDiagnosis(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.UserId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(entity.FacilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, entity.FacilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptEmarResidentdetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            if (recordAllDia.Tables[0].Rows.Count > 0)
            {
                rd.OpenSubreport("crptEmarResidentdetailsAllergies").SetDataSource(recordAllDia.Tables[0]);
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "EMAR Resident Detail Report");
            rd.SetParameterValue("Rname", legend);
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("Month", "Month: " + Enum.GetName(typeof(Months), entity.Month));
            rd.SetParameterValue("Year", "Year: " + entity.Year);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "EmarResidentdetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEmarResidentdetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetResidentnamesmarReport/{nursingStationIds}/{ResidentStatus}")]
        [HttpGet]
        public IList GetResidentnamesmarReport(string nursingStationIds, int? ResidentStatus)
        {

            this._log.Debug("---Executing GetResidentnamesmarReport() in ReportsController----");
            var records = this._reportsService.GetResidentnamesmarReport(nursingStationIds, ResidentStatus).Result;
            return records;
        }
        //[Route("GetNurseNotesDetailsReport/{fromdate}/{todate}/{userId}/{nursestationId}/{commentType}/{facilityId}")]
        //[HttpGet]
        //public JsonResult<DataTable> GetNurseNotesDetailsReport(string fromdate, string todate, int userId, string nursestationId, string commentType, int facilityId)
        //{
        //    this._log.Debug("---Executing GetNurseNotesDetailsReport() in ReportsController----");
        //    var records = this._reportsService.GetNurseNotesDetailsReport(fromdate, todate, userId, nursestationId, commentType).Result;
        //    return Json<DataTable>(records.Tables[0]);
        //}
        [Route("GetEmarResidentReportSecurity/{nursingStationId}/{date}/{passTime}/{shiftId}/{window}/{userId}/{facilityId}/{dateTime}/{timeFormate}/{reportType}")]
        [HttpGet]
        public IHttpActionResult GetEmarResidentReportSecurity(int nursingStationId, string date, string passTime, int shiftId, int window, int userId, int facilityId, string dateTime, int timeFormate, int reportType)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            var passtime = passTime == "null" ? null : passTime.Replace('-', ':');
            this._log.Debug("---Executing GetEmarResidentReportSecurity() in ReportsController----");
            //var records = this._reportsService.GetEmarResidentReportSecurity(nursingStationId, date, passtime, shiftId, window).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            if (reportType == 1)
            {
                var records = this._reportsService.GetEmarResidentReportSecurity(nursingStationId, date, null, 0, window).Result;
                ReportDocument rd = new ReportDocument();
                string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptEmarSecurityreportondatewise.rpt";
                rd.Load(reportsPath);
                rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (image.Size.Width >= image.Size.Height + 120)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
                rd.SetDataSource(records.Tables[0]);
                rd.SetParameterValue("ReportName", "Residents with Orders Report");
                rd.SetParameterValue("Adminster Schedule", Convert.ToDateTime(date).ToString("MM/dd/yyyy"));
                rd.SetParameterValue("DateTime", reportDateTime);
                //rd.SetParameterValue("AdministerSchedule", "Resident Scheduled Report");
                // var test =records.Tables[0].Rows[0].ItemArray[0].ToString();
                // string path = HttpContext.Current.Server.MapPath("~/" + records.Tables[0].Rows[0].ItemArray[0]);
                // rd.SetParameterValue("image", path);
                var Date = Convert.ToDateTime(date).GetDateTimeFormats()[3];
                //rd.SetParameterValue("Date", "Date :" + Date);
                //rd.SetParameterValue("Passtime", "PassTime :" + passtime);
                //rd.SetParameterValue("image", "HttpContext.Current.Server.MapPath('~/ ' + records.Tables[0].Rows[0].ItemArray[0])");
                // var imagePath = "imagelocation"+ConvertImage(records.Tables[0].Rows[0].ItemArray[0].ToString());
                Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                MemoryStream ms = new MemoryStream();
                s.CopyTo(ms);
                rd.Close();
                rd.Dispose();
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };

                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Emar Resident" + DateTime.Now.ToString("yyyy/MM/dd")
                };
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                this._log.Debug("---Executed Successfully GetEmarResidentReportSecurity() in ReportsController----");
                return responseMessageResult;
            }
            else
            {
                var records = this._reportsService.GetEmarResidentReportSecurity(nursingStationId, date, passtime, shiftId, window).Result;
                ReportDocument rd = new ReportDocument();
                string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptResidentScheduledReport_Date_Time.rpt";
                rd.Load(reportsPath);
                rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
                System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
                if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                    if (image.Size.Width >= image.Size.Height + 120)
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                    }
                    else
                    {
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                        rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                    }
                }
                else
                {
                    rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                }
                if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                }
                rd.SetDataSource(records.Tables[0]);
                rd.SetParameterValue("ReportName", "Resident Scheduled Report");
                rd.SetParameterValue("DateTime", reportDateTime);
                // var test =records.Tables[0].Rows[0].ItemArray[0].ToString();
                //string path = HttpContext.Current.Server.MapPath("~/" + records.Tables[0].Rows[0].ItemArray[0]);
                //rd.SetParameterValue("image", path);
                if (shiftId != 0 && window == 0)
                {
                    var shiftTime = this._reportsService.GetNurseStationShiftTime(shiftId, nursingStationId).Result;
                    rd.SetParameterValue("Passtime", "ScheduleTime :" + shiftTime);
                }
                else if (shiftId == 0 && passtime != null && window == 0)
                {
                    rd.SetParameterValue("Passtime", "PassTime :" + passtime);
                }
                else if(shiftId == 0 && passtime == null && (window == 0 || window == 1))
                {
                    rd.SetParameterValue("Passtime", "");
                }
                else if (shiftId == 0 && passtime != null && window == 1)
                {
                    string oneHourBeforeTime;
                    string oneHourAfterTime;
                    List<string> dateValue = date.Split('-').ToList();
                    if (timeFormate == 1)
                    {
                        List<string> timeValue = passTime.Split('-').ToList();
                        DateTime twoHourDateTime = new DateTime(Convert.ToInt32(dateValue[0]), Convert.ToInt32(dateValue[1]), Convert.ToInt32(dateValue[2]), Convert.ToInt32(timeValue[0]), Convert.ToInt32(timeValue[1]), 0);
                        oneHourBeforeTime = twoHourDateTime.AddHours(-1).ToString("HH:mm");
                        oneHourAfterTime = twoHourDateTime.AddHours(1).ToString("HH:mm");
                    }
                    else
                    {
                        DateTime time = Convert.ToDateTime(passTime.Replace('-', ':'));
                        var convertedtime = time.ToString("HH:mm");
                        List<string> timeData = convertedtime.Split(':').ToList();
                        DateTime twoHourDateTime = new DateTime(Convert.ToInt32(dateValue[0]), Convert.ToInt32(dateValue[1]), Convert.ToInt32(dateValue[2]), Convert.ToInt32(timeData[0]), Convert.ToInt32(timeData[1]), 0);
                        oneHourBeforeTime = twoHourDateTime.AddHours(-1).ToString("hh:mm tt");
                        oneHourAfterTime = twoHourDateTime.AddHours(1).ToString("hh:mm tt");
                    }
                    rd.SetParameterValue("Passtime", "PassTime :" + oneHourBeforeTime + " - " + oneHourAfterTime);
                }
                var Date = Convert.ToDateTime(date).GetDateTimeFormats()[3];
                rd.SetParameterValue("Date", "Date :" + Date);
                //rd.SetParameterValue("image", "HttpContext.Current.Server.MapPath('~/ ' + records.Tables[0].Rows[0].ItemArray[0])");
                // var imagePath = "imagelocation"+ConvertImage(records.Tables[0].Rows[0].ItemArray[0].ToString());
                Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                MemoryStream ms = new MemoryStream();
                s.CopyTo(ms);
                rd.Close();
                rd.Dispose();
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };

                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Emar Resident" + DateTime.Now.ToString("yyyy/MM/dd")
                };
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                this._log.Debug("---Executed Successfully GetEmarResidentReportSecurity() in ReportsController----");
                return responseMessageResult;
            }
            return null;
        }

        public byte[] ConvertImage(string url)
        {
            if (url != null)
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath("~/" + url);
                if (path != null && File.Exists(path))
                    buffer = File.ReadAllBytes(path);
                else
                {
                    path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                    buffer = File.ReadAllBytes(path);
                }
                return buffer;
            }
            else
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                if (path != null)
                    buffer = File.ReadAllBytes(path);
                return buffer;
            }
        }
        [Route("GetEmarResidentdetailsGrid")]
        [HttpPost]
        public JsonResult<EMARResidentEntity> GetEmarResidentdetailsGrid(EmarHoleHistoryEntity objEntity)
        {
            this._log.Debug("---Executing GetEmarResidentdetailsReport() in ReportsController----");
            if (objEntity.NursingstationId == "all")
                objEntity.NursingstationId = null;
            if (objEntity.patientName == "all")
                objEntity.patientName = null;
            var records = this._reportsService.GetEmarResidentdetailsReport(objEntity.Month, objEntity.Year, objEntity.NursingstationId, objEntity.patientName, objEntity.UserId).Result;
            string legend = this._reportsService.GetEmarResidentlegendReport(objEntity.Month, objEntity.Year, objEntity.NursingstationId, objEntity.patientName, objEntity.UserId).Result; ;
            var entity = new EMARResidentEntity();
            var columnsList = records.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity();
            foreach (var item in columnsList)
            {
                if (item == "PatientName")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Resident Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "MRNo")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "MR Number", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "Sex")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Gender", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "GiveCodeText")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Medication Code", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "time")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Time", display = true };
                    columnNames.Add(columns);
                }
                else
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = item, display = true };
                    columnNames.Add(columns);
                }
            }
            entity.ColumnNames = columnNames;

            //DataTable dtUpdated = new DataTable();
            //dtUpdated = records.Tables[0].Clone();
            //foreach (DataRow row in records.Tables[0].Rows)
            //{
            //    for (int i = 0; i < records.Tables[0].Columns.Count; i++)
            //    {
            //        string oldVal = row[i].ToString();
            //        if (oldVal == "0")
            //        {
            //            string newVal = "";
            //            row[i] = newVal;
            //        }
            //        else if (oldVal == "1")
            //        {
            //            string newVal = "X";
            //            row[i] = newVal;
            //        }
            //        else if (oldVal == "2")
            //        {
            //            string newVal = "X";
            //            row[i] = newVal;
            //        }


            //    }
            //    entity.GridData = records.Tables[0];
            //}
            entity.GridData = records.Tables[0];
            entity.Legend = legend;
            return Json<EMARResidentEntity>(entity);
        }
        [Route("GetTherapeuticaltwo")]
        [HttpPost]
        public JsonResult<EMARResidentEntity> GetTherapeuticaltwo(EmarHoleHistoryEntity objEntity)
        {
            this._log.Debug("---Executing GetTherapeuticaltwo() in ReportsController----");
            if (objEntity.NursingstationId == "all")
                objEntity.NursingstationId = null;
            if (objEntity.patientName == "all")
                objEntity.patientName = null;

            ////patientName is classification
            var records = this._reportsService.GetTherapeuticaltwo(objEntity.Month, objEntity.Year, objEntity.NursingstationId, objEntity.patientName,objEntity.commentType).Result;
            string legend = this._reportsService.GetTherapeuticaltwolegend(objEntity.Month, objEntity.Year, objEntity.NursingstationId, objEntity.patientName, objEntity.commentType).Result;

            var entity = new EMARResidentEntity();
            var columnsList = records.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity();
            foreach (var item in columnsList)
            {
                if (item == "PatientName")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Resident Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "MRNo")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "MR Number", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "Sex")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Gender", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "GiveCodeText")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Medication Code", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "time")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Time", display = true };
                    columnNames.Add(columns);
                }
                else
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = item, display = true };
                    columnNames.Add(columns);
                }
            }
            entity.ColumnNames = columnNames;
            entity.Legend = legend;
          
            entity.GridData = records.Tables[0];
            return Json<EMARResidentEntity>(entity);
        }



        [Route("GetTherapeuticaltwoReport")]
        [HttpPost]
        public IHttpActionResult GetTherapeuticaltwoReport(EmarthReportEntity entity)
        {
            var reportDateTime = entity.DateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetMARHoledetails() in ReportsController----");
            if (entity.NursingstationId == "all")
                entity.NursingstationId = null;
            if (entity.patientName == "all")
                entity.patientName = null;


            //patientName is classification
            var records = this._reportsService.GetTherapeuticaltwo(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.druglassification).Result;
            string legend = this._reportsService.GetTherapeuticaltwolegend(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.druglassification).Result;
            //var records = this._reportsService.GetMARHoledetails(entity.Month, entity.Year, entity.NursingstationId, entity.UserId, entity.patientName).Result;
            var emarlogo = this._reportsService.GetEmarlogo(entity.FacilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, entity.FacilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportspath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptTherapeuticalResidentReport.rpt";
            rd.Load(reportspath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Order Administration Report by Therapeutic Category");
            rd.SetParameterValue("Rname", legend);
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("Month", "Month:" + " " + Enum.GetName(typeof(Months), entity.Month));
            rd.SetParameterValue("Year", "Year:" + " " + entity.Year);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "TherapeuticalResidentdetails" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = new ResponseMessageResult(httpResponseMessage);
            this._log.Debug("---Executing GetTherapeuticalResidentdetails() successfully in ReportsController----");
            return responseMessageResult;

        }

        [Route("GetTherapeuticalMedication")]
        [HttpPost]
        public JsonResult<EMARResidentEntity> GetTherapeuticalMedication(EmarHoleHistoryEntity objEntity)
        {
            this._log.Debug("---Executing GetTherapeuticalMedication() in ReportsController----");
            if (objEntity.NursingstationId == "all")
                objEntity.NursingstationId = null;
            if (objEntity.patientName == "all")
                objEntity.patientName = null;

            ////patientName is classification
            var records = this._reportsService.GetTherapeuticalMedication(objEntity.Month, objEntity.Year, objEntity.NursingstationId, objEntity.patientName, objEntity.commentType, objEntity.shiftTime).Result;
            string legend = this._reportsService.GetTherapeuticalMedicationlegend(objEntity.Month, objEntity.Year, objEntity.NursingstationId, objEntity.patientName, objEntity.commentType, objEntity.shiftTime).Result;
            var entity = new EMARResidentEntity();
            var columnsList = records.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            var columnNames = new List<DashboardColumnsEntity>();
            var columns = new DashboardColumnsEntity();
            foreach (var item in columnsList)
            {
                if (item == "PatientName")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Resident Name", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "MRNo")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "MR Number", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "Sex")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Gender", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "GiveCodeText")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Medication Code", display = true };
                    columnNames.Add(columns);
                }
                else if (item == "time")
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = "Time", display = true };
                    columnNames.Add(columns);
                }
                else
                {
                    columns = new DashboardColumnsEntity() { name = item, displayName = item, display = true };
                    columnNames.Add(columns);
                }
            }
            entity.ColumnNames = columnNames;
            entity.GridData = records.Tables[0];
            entity.Legend = legend;
            return Json<EMARResidentEntity>(entity);
        }

        [Route("GetTherapeuticalMedicationReport")]
        [HttpPost]
        public IHttpActionResult GetTherapeuticalMedicationReport(EmarthReportEntity entity)
        {
            var reportDateTime = entity.DateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetTherapeuticalResidentdetails() in ReportsController----");
            if (entity.NursingstationId == "all")
                entity.NursingstationId = null;
            if (entity.patientName == "all")
                entity.patientName = null;


            //patientName is classification
            var records = this._reportsService.GetTherapeuticalMedication(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.druglassification, entity.shiftTime).Result;
            string legend = this._reportsService.GetTherapeuticalMedicationlegend(entity.Month, entity.Year, entity.NursingstationId, entity.patientName, entity.druglassification, entity.shiftTime).Result;
            //var records = this._reportsService.GetMARHoledetails(entity.Month, entity.Year, entity.NursingstationId, entity.UserId, entity.patientName).Result;
            var emarlogo = this._reportsService.GetEmarlogo(entity.FacilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, entity.FacilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportspath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptTherapeuticalMedicationReport.rpt";
            rd.Load(reportspath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Medication Refusal Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("Rname", legend);
            rd.SetParameterValue("Month", "Month:" + " " + Enum.GetName(typeof(Months), entity.Month));
            rd.SetParameterValue("Year", "Year:" + " " + entity.Year);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "MedicationRefusal" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = new ResponseMessageResult(httpResponseMessage);
            this._log.Debug("---Executing GetTherapeuticalMedication() successfully in ReportsController----");
            return responseMessageResult;

        }
        
        [Route("GetUserActivityDetailsReport")]
        [HttpPost]
        public IHttpActionResult GetUserActivityDetailsReport(UserActvivtysReportEntity Model)
        {
            var reportDateTime = Model.dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetUserActivityDetailsReport() in ReportsController----");
            var records = this._reportsService.GetUserActivityDetailsReport(Model.userIds, Model.fromdate, Model.todate).Result;
            var emarlogo = this._reportsService.GetEmarlogo(0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, null).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptUserActivityReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }

            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "User Activity  Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(Model.fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(Model.todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "User Activity" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetUserActivityDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetUserActivityExcel")]
        [HttpPost]
        public JsonResult<DataTable> GetUserActivityExcel(UserActvivtysExcelEntity Model)
        {
            this._log.Debug("---Executing GetUserActivityDetailsReport() in ReportsController----");
            var records = this._reportsService.GetUserActivityDetailsReport(Model.userIds, Model.fromdate, Model.todate).Result;
            records.Tables[0].Columns["IP Address"].ColumnName = "System IP";
            records.Tables[0].Columns["Browser"].ColumnName = "Browser Name";
            records.Tables[0].Columns["LogIn Time"].ColumnName = "Start Time";
            records.Tables[0].Columns["LogOut Time"].ColumnName = "End Time";
            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "User Name", "User Display Name", "Role", "System IP", "Browser Name", "Start Time", "End Time", "Session Time", "Screen", "Activity");
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.UserActivityDashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return Json<DataTable>(newTable);
        }
        [Route("GetEkitDetailsReport/{userId}/{nursestationid}/{facilityId}/{ekitType}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetEkitDetailsReport(/*string fromdate, string todate,*/ int userId, string nursestationid, Nullable<int> facilityId, int ekitType, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetEkitDetailsReport() in ReportsController----");
            var records = this._reportsService.GetEkitDetailsReport(/*fromdate, todate,*/ userId, nursestationid, ekitType).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptEkitDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "E-Kit/On-site Medication Active Inventory Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            //var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            //var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            //rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            //rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            //rd.SetParameterValue("Status", "Status:"+" " + (ekitType==1?"Active":"Inactive"));
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ekit" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetRefillDetailsReport/{fromdate}/{todate}/{userId}/{nursestationid}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetRefillDetailsReport(string fromdate, string todate, int userId, string nursestationid, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetRefillDetailsReport() in ReportsController----");
            var records = this._reportsService.GetRefillDetailsReport(fromdate, todate, userId, nursestationid).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptRefillDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Refill Request Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:" +" "+ fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Refill" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetRefillDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetStockDetailsReport/{selectedUser}/{userId}/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetStockDetailsReport(int selectedUser, int userId, int companyId)
        {

            this._log.Debug("---Executing GetStockDetailsReport() in ReportsController----");
            var records = this._reportsService.GetStockDetailsReport(selectedUser, companyId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(companyId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(companyId, null).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptStockDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "E-Kit/On-site Meds Dispensing Report");
            //var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            //var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            //rd.SetParameterValue("FromDate", "From :" + fromDate);
            //rd.SetParameterValue("ToDate", "To :" + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ekit" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetRefdataExcel")]
        [HttpPost]
        public JsonResult<DataTable> GetRefdataExcel(MedRefEntity data)
        {
            try
            {
                var records = this._reportsService.GetRefusedByResidentDetailsReport(data.fromdate, data.todate, data.userId, data.nursingstationId, data.passTime, data.dateTime).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RefusedByResidentDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Order Type"].ColumnName = "Therapeutic DrugType";
                records.Tables[0].Columns["AdminsterSchedule"].ColumnName = "End Date";
                records.Tables[0].Columns["Physician"].ColumnName = "Prescriber";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Therapeutic DrugType", "Drug Name", "Start Date", "End Date", "Prescriber");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            catch(Exception ex)

            {
                return null;
            }           
        }
        [Route("GetMedRefdataExcel")]
        [HttpPost]
        public JsonResult<DataTable> GetMedRefdataExcel(MedRefEntity data)
        {
            var records = this._reportsService.GetMedrefReport(data.fromdate, data.todate, data.userId, data.nursingstationId, data.passTime).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.RefusedByResidentDashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
            records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
            records.Tables[0].Columns["AdminsterSchedule"].ColumnName = "Administration Schedule";
            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Order Type", "Drug Name", "Start Date", "Administration Schedule", "Pass Time", "Physician");
            newTable.Columns["ResidentName"].ColumnName = "Resident Name";
            return Json<DataTable>(newTable);
        }
        [Route("GetCommonExcel/{module}/{fromdate}/{todate}/{userId}/{nursestationId}/{passTime?}/{orderType?}/{commentType?}/{patientName?}/{year?}/{month?}/{type?}/{reportType?}/{years?}/{value?}/{residentId?}/{shiftTime?}/{facilityId?}/{MedicationReason?}")]
        [HttpGet]
        public JsonResult<DataTable> GetCommonExcel(string module, string fromdate, string todate, int userId, string nursestationId, string passTime = "", int orderType = 0, string commentType = "", string patientName = "", int year = 0, int month = 0, int type = 0, int reportType = 0, string years = "", string value = "", string residentId = "", string shiftTime = "", int facilityId = 0,string MedicationReason="")
        {
            this._log.Debug("---Executing GetCommonExcel() in ReportsController----");
            if (module == "seventytwo")
            {
                var records = this._reportsService.Get72HoursReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.SeventyTwoHoursDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NurseStation_Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("Resident Name", typeof(string), "[ResidentName]+' ('+[DOB]+')'");
                records.Tables[0].Columns["DispensedBy"].ColumnName = "Administered By";
                records.Tables[0].Columns["AdministrationSchedule"].ColumnName = "Administration Schedule";
                records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
                records.Tables[0].Columns["PassTime"].ColumnName = "Pass Time";
                records.Tables[0].Columns["CommentedBy"].ColumnName = "Commented By";
                records.Tables[0].Columns["StartDate"].ColumnName = "Start Date";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "Resident Name", "Drug Name", "Start Date", "Administration Schedule", "Pass Time", "Administered By", "Comment", "Commented By");
                return Json<DataTable>(newTable);
            }
            if (module == "prnr")
            {
                var records = this._reportsService.GetPRNDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PRNDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Dispensed By"].ColumnName = "Administered By";
                records.Tables[0].Columns["AdministrationSchedule"].ColumnName = "Administration Schedule";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Start Date", "Administration Schedule", "Pass Time", "Status", "Administered By", "PRNComment", "Commented BY");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                newTable.Columns["Commented BY"].ColumnName = "Commented By";
                return Json<DataTable>(newTable);

            }
            if (module == "order")
            {
                int shiftTimes = 0;
                if (shiftTime == "null")
                    shiftTimes = 0;
                else
                    shiftTimes = Convert.ToInt32(shiftTime);
                var records = this._reportsService.GetOrderDetailsReport("null", fromdate, todate, orderType, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["AdminsterSchedule"].ColumnName = "Administration Schedule";
                records.Tables[0].Columns["Status"].ColumnName = "Medication Reason";
                records.Tables[0].Columns["Dispensed By"].ColumnName = "Administered By";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Order Type", "Drug Name", "Start Date", "Administration Schedule", "Pass Time", "Medication Reason", "Administered By");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }

            // doc admin order
            if (module == "DocAdminOrder")
            {
                //int nsID = Convert.ToInt32(nursestationId);
                var records = this._reportsService.GetDocAdministerOrderReport(fromdate, todate, orderType, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DocumentAdminReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Entered Date"].ColumnName = "Entered Date/Time";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Scheduled Date/Time", "Forced Date/Time", "Administered By", "Entered By", "Entered Date/Time");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "barcode")
            {
                var records = this._reportsService.GetWithoutBarcodeDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.BarcodeDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Physician"].ColumnName = "Prescriber";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Start Date", "End Date", "Prescriber");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
             
                return Json<DataTable>(newTable);
            }
            if (module == "biometric")
            {
                var records = this._reportsService.GetBiometricsDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.BiometericDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Dispensed By"].ColumnName = "Administered By";
                records.Tables[0].Columns["AdministrationSchedule"].ColumnName = "Administration Schedule";
                //records.Tables[0].Columns["Status"].ColumnName = "Medication Reason";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Order Type", "Drug Name", "Start Date", "Administration Schedule", "Pass Time", "Reason For Biometric Bypass", "Administered By");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "ordercs")
            {
                var records = this._reportsService.GetOrderControlSubstanceReport(nursestationId, userId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderControlSubstanceDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Physician"].ColumnName = "Prescriber";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Start Date", "End Date", "Prescriber");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
              
                return Json<DataTable>(newTable);
            }
            if (module == "orderfav")
            {
                int shiftTimes = 0;
                if (shiftTime == "null")
                    shiftTimes = 0;
                else
                    shiftTimes = Convert.ToInt32(shiftTime);
                var records = this._reportsService.GetOrderWithFavouritesReport(passTime, fromdate, todate, userId, nursestationId, shiftTimes).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderwithFavouritesDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["AdminsterSchedule"].ColumnName = "Administration Schedule";
                records.Tables[0].Columns["Prerequisite checks"].ColumnName = "Require User Inputs";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Order Type", "Drug Name", "Start Date", "End Date", "Administration Schedule", "Pass Time", "Require User Inputs");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "ordercsoff")
            {
                var records = this._reportsService.GetOrderControlSignoffReport(fromdate, todate, nursestationId, userId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderSignoffDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Last Certify"].ColumnName = "Last Certified By";
                records.Tables[0].Columns["Date Certify and Time"].ColumnName = "Last Certified Date Time";
                records.Tables[0].Columns["Quantity"].ColumnName = "Quantity Certified";
                records.Tables[0].Columns["DiscrepancyReason"].ColumnName = "Discrepancy Reason";
                records.Tables[0].Columns["Last Witness"].ColumnName = "Witness";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Last Certified By", "Last Certified Date Time", "Quantity Certified", "Discrepancy Reason", "Witness");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "orderhold")
            {
                var records = this._reportsService.GetOrderHoldDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderHoldDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["HoldFrom"].ColumnName = "Hold From";
                records.Tables[0].Columns["HoldTo"].ColumnName = "Hold Until";
                records.Tables[0].Columns["HoldReason"].ColumnName = "Hold Reason";
                records.Tables[0].Columns["Physician"].ColumnName = "Prescriber";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Directions", "Start Date", "Prescriber", "Hold From", "Hold Until", "Hold Reason");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "refused")
            {
                var records = this._reportsService.GetRefusedByResidentDetailsReport(fromdate, todate, userId, nursestationId, passTime,"").Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RefusedByResidentDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["AdminsterSchedule"].ColumnName = "Administration Schedule";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Order Type", "Drug Name", "Start Date", "Administration Schedule", "Pass Time", "Physician");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "orderchange")
            {
                var records = this._reportsService.GetOrderChangeDetailsReport(fromdate, todate, userId, nursestationId, residentId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderChangeDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NursingStationName"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("Resident Name", typeof(string), "[PatientName]+' ('+[DOB]+')'");
                records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
                records.Tables[0].Columns["ColumnName"].ColumnName = "Column Name";
                records.Tables[0].Columns["OldValue"].ColumnName = "Old Value";
                records.Tables[0].Columns["NewValue"].ColumnName = "New Value";
                records.Tables[0].Columns["UpdatedBy"].ColumnName = "Updated By";
                records.Tables[0].Columns["UpdatedOn"].ColumnName = "Updated On";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "Resident Name", "Drug Name", "Barcode", "Column Name", "Old Value", "New Value", "Updated By", "Updated On");
                return Json<DataTable>(newTable);
            }
            if (module == "scan")
            {
                var records = this._reportsService.GetWithoutScanningDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ScanningBypassDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Dispensed By"].ColumnName = "Administered By";
                records.Tables[0].Columns["AdministrationSchedule"].ColumnName = "Administration Schedule";
                records.Tables[0].Columns["Barcode(s)"].ColumnName = "Barcode(s)";
                records.Tables[0].Columns["ReasonforBypassing"].ColumnName = "Reason For Bypass";
                records.Tables[0].Columns["PassTime"].ColumnName = "Pass Time";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Directions", "Barcode(s)", "Administration Schedule", "Pass Time", "Administered By", "Reason For Bypass");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "orderdes")
            {
                var records = this._reportsService.GetDestructionDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DestructionDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["QtyonHand"].ColumnName = "Quantity Before Destruction";
                //records.Tables[0].Columns["QtyAfterDestroy"].ColumnName = "Quantity On Hand";
                records.Tables[0].Columns["Destruction Quantity"].ColumnName = "Quantity Destroyed";
                records.Tables[0].Columns["Destruction Done By"].ColumnName = "Destroyed By";
                records.Tables[0].Columns["DateTime of Destruction"].ColumnName = "Date/Time of Destruction";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Quantity Before Destruction", "Quantity Destroyed", "Reason", "Destroyed By", "Date/Time of Destruction", "Witness");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "fstock")
            {
                var records = this._reportsService.GetFloorStockDetailsReport(userId, nursestationId, facilityId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FloorStockDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
                records.Tables[0].Columns["BarcodeDetail"].ColumnName = "Barcode";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "Drug Name", "Quantity on Hand", "Barcode");
                return Json<DataTable>(newTable);
            }
            if (module == "pharmacy")
            {
                var records = this._reportsService.GetPharmacyMedsDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PharmacyMedsDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NursingStationName"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("Resident Name", typeof(string), "[ResidentName]+' ('+[DOB]+')'");
                records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
                records.Tables[0].Columns["Check-In By"].ColumnName = "Check-in By";
                records.Tables[0].Columns["Check-In Date"].ColumnName = "Check-in Date";
                records.Tables[0].Columns["QuantityReceived"].ColumnName = "Quantity Received";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "Resident Name", "Drug Name", "Quantity Received", "Barcode(s)", "Check-in By", "Check-in Date");
                return Json<DataTable>(newTable);
            }
            if (module == "psychiatric")
            {
                var records = this._reportsService.GetPsychiatricDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PsychiatricDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["Physician"].ColumnName = "Prescriber";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Resident Status", "Psychiatric Drug Count", "Prescriber");
                return Json<DataTable>(newTable);
            }
            if (module == "nursenote")
            {
                var records = this._reportsService.GetNurseNotesDetailsReport(fromdate, todate, userId, nursestationId, commentType, MedicationReason).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseNotesDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Comment Type", "Dispensed By", "Nurse Comments", "Comment On", "MedicationReason");
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                return Json<DataTable>(newTable);
            }
            if (module == "prescribernote")
            {
                var records = this._reportsService.GetPrescriberNotesReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PrescriberNotesDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                return Json<DataTable>(records.Tables[0]);
            }
            if (module == "outbounderror")
            {
                var records = this._reportsService.GetOutBoundErrorDetails(fromdate, todate).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.HL7OutboundErrorDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["File_AcknowledgeDate"].ColumnName = "File Acknowledgement Date";
                records.Tables[0].Columns["File_Data"].ColumnName = "File Data";
                records.Tables[0].Columns["File_Acknowledge"].ColumnName = "File Acknowledgement";
                return Json<DataTable>(records.Tables[0]);
            }
            //if (module == "inbound")
            //{
            //    var records = this._reportsService.GetInboundDetails(fromdate, todate, patientName,null).Result;
            //    UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            //    {
            //        Screen_Id = (int)ScreenEntity.Screens.HL7InboundDashboard,
            //        Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
            //        Comments = "",
            //        Session_Id = 0,
            //        Time = DateTime.Now,
            //        UserActivity_Id = 0,

            //    };
            //    _userActivityRepository.InsertUserActivityDetails(activityEntity);
            //    return Json<DataTable>(records.Tables[0]);
            //}
            if (module == "average")
            {
                var records = this._reportsService.GetAverageCensusDataReport(year, month, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NurseStation_Name"].ColumnName = "Nursing Station Name";
                return Json<DataTable>(records.Tables[0]);
            }

            if (module == "compare")
            {
                var records = this._reportsService.GetCompareCensusDataReport(years, userId, nursestationId, type, reportType).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return Json<DataTable>(records.Tables[0]);
            }
            if (module == "census")
            {
                var records = this._reportsService.GetCensusReportByfilter(fromdate, todate, userId, nursestationId, type, reportType).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NurseStation_Name"].ColumnName = "Nursing Station Name";
                return Json<DataTable>(records.Tables[0]);
            }
            if (module == "census1")
            {
                var records = this._reportsService.GetCensusReportByDates(fromdate, todate, userId, nursestationId, type, reportType, value).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return Json<DataTable>(records.Tables[0]);
            }
            if (module == "census2")
            {
                var records = this._reportsService.GetCensusReportByDates(fromdate, todate, userId, nursestationId, type, reportType, value).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return Json<DataTable>(records.Tables[0]);
            }
            if (module == "refill")
            {
                var records = this._reportsService.GetRefillDetailsReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RefillReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
                records.Tables[0].Columns["NumberOfRefillsRemaining"].ColumnName = "Refills Remaining";
                records.Tables[0].Columns["RefillSubmittedBy"].ColumnName = "Refill Submitted By";
                records.Tables[0].Columns["Refill_CreatedDate"].ColumnName = "Date/Time Refill Requested" ;
                records.AcceptChanges();
                records.Tables[0].Columns["Status"].ColumnName = "Refill Request Status";
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName", "Drug Name", "Refills Remaining", "Refill Submitted By", "Date/Time Refill Requested", "Refill Request Status");
                newTable.AcceptChanges();
                newTable.Columns["ResidentName"].ColumnName = "Resident Name";
                
              
                return Json<DataTable>(newTable);
            }

            if (module == "ekitdispensing")
            {
                var records = this._reportsService.GetEkitMedsDispensingReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NursingStationName"].ColumnName = "Nursing Station Name";
                records.Tables[0].Columns["Physician"].ColumnName = "Prescriber";
                records.Tables[0].Columns.Add("Resident Name", typeof(string), "[ResidentName]+' ('+[DOB]+')'");
                records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
                records.Tables[0].Columns["QtyOnHandRemaining"].ColumnName = "Qty On Hand Remaining";
                records.Tables[0].Columns.Add("Qty Administered", typeof(string), "QuantityAdministered");
                records.Tables[0].Columns["EkitAdministeredBy"].ColumnName = "Ekit Administered By";
                records.Tables[0].Columns["AdministeredDateTime"].ColumnName = "Administered Date/Time";
                records.Tables[0].Columns["Expirydate"].ColumnName = "Expiry Date";
                records.Tables[0].Columns["LotNumber"].ColumnName = "Lot#";



                DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "Resident Name", "Drug Name", "Qty Administered", "Prescriber", "Expiry Date", "Lot#", "Ekit Administered By", "Administered Date/Time", "Qty On Hand Remaining");
                // newTable.Columns["Administered Date Time"].ColumnName = "Administered Date/Time";
                return Json<DataTable>(newTable);
            }
            if (module == "MedicationQtyonhandUpdateReport")
            {
                var records = this._reportsService.GetMedicationQtyonhandUpdateReport(fromdate, todate, userId, nursestationId).Result;
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                records.Tables[0].Columns["NurseStation_Name"].ColumnName = "Nurse Station Name";
                records.Tables[0].Columns["Drugname"].ColumnName = "Drug Name";
                records.Tables[0].Columns["BarcodeDetail"].ColumnName = "Barcode";
                records.Tables[0].Columns["LotNumber"].ColumnName = "Lot#";
                records.Tables[0].Columns["ExpiryDate"].ColumnName = "Exp Date";
                records.Tables[0].Columns["qtybeforeupdate"].ColumnName = "Qty before Update";
                records.Tables[0].Columns["qtyafterupdate"].ColumnName = "Qty after Update";
                records.Tables[0].Columns["reason"].ColumnName = "Reason for Update";
                records.Tables[0].Columns["updatedby"].ColumnName = "Updated By";
                records.Tables[0].Columns["updateddate"].ColumnName = "Updated Date";
                records.Tables[0].Columns["witness"].ColumnName = "Witness";

                // Creating the new DataTable with the updated column names
                DataTable newTable = records.Tables[0].DefaultView.ToTable(false,
                    "Nurse Station Name",
                    "Drug Name",
                    "Barcode",
                    "Lot#",
                    "Exp Date",
                    "Qty before Update",
                    "Qty after Update",
                    "Reason for Update",
                    "Updated By",
                    "Updated Date",
                    "Witness");
                return Json<DataTable>(newTable);
            }

            return null;
        }
        [Route("GetInboundExcel/{fromdate}/{todate}/{inboundStatus}/{patientName}")]
        [HttpGet]
        public JsonResult<DataTable> GetInboundExcel(string fromdate, string todate, int inboundStatus, string patientName)
        {

            var records = this._reportsService.GetInboundDetails(fromdate, todate, patientName, inboundStatus).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.HL7InboundDashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["File_CreatedDate"].ColumnName = "File Created Date";
            records.Tables[0].Columns["File_Data"].ColumnName = "File Data";
            records.Tables[0].Columns["FileAck_Data"].ColumnName = "File Acknowledgement";
            return Json<DataTable>(records.Tables[0]);
        }
        [Route("GetEkitMedsDispensingExcel/{userId}/{companyId}")]
        [HttpGet]
        public JsonResult<DataTable> GetEkitMedsDispesdingExcel(int userId, int companyId)
        {

            var records = this._reportsService.GetStockDetailsReport(userId, companyId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.StockDashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["Company_Name"].ColumnName = "Company Name";
            records.Tables[0].Columns["Facility_Name"].ColumnName = "Facility Name";
            records.Tables[0].Columns["NurseStationName"].ColumnName = "Nursing Station Name";
            records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
            records.Tables[0].Columns["QuantityUsed"].ColumnName = "Total Qty Administered";
            records.Tables[0].Columns["QuantityRemaining"].ColumnName = "Qty On Hand Remaining";
            int status = records.Tables[0].Rows[0].Field<int>(7);
            DataTable newTable = new DataTable();
            if (status == 1)
            {
                newTable = records.Tables[0].DefaultView.ToTable(false, "Company Name", "Facility Name", "Nursing Station Name", "Drug Name", "Total Qty Administered", "Qty On Hand Remaining");
            }
            else if (status == 2)
            {
                newTable = records.Tables[0].DefaultView.ToTable(false, "Facility Name", "Nursing Station Name", "Drug Name", "Total Qty Administered", "Qty On Hand Remaining");

            }
            else if (status == 3)
            {
                newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "Drug Name", "Total Qty Administered", "Qty On Hand Remaining");

            }
            return Json<DataTable>(newTable);
        }
        [Route("GetAdminUsersDetailsExcel/{companyId}/{userId}")]
        [HttpGet]
        public JsonResult<DataTable> GetAdminUsersDetailsExcel(string companyId, int userId)
        {
            var records = this._reportsService.GetAdminUsersDetailsReport(companyId, userId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.AdminUsersReport,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["company_Name"].ColumnName = "Company Name";
            records.Tables[0].Columns["Name"].ColumnName = "Name";
            records.Tables[0].Columns["UserName"].ColumnName = "User Name";
            records.Tables[0].Columns["DisplayName"].ColumnName = "User Display Name";
            records.Tables[0].Columns["EmailId"].ColumnName = "Email Address";
            records.Tables[0].Columns["RoleName"].ColumnName = "Role Name";
            records.Tables[0].Columns["userStatus"].ColumnName = "User Status";
            return Json<DataTable>(records.Tables[0]);
        }
        [Route("GetSetupConfigDetailsExcel/{companyId}")]
        [HttpGet]
        public JsonResult<DataTable> GetSetupConfigDetailsExcel(string companyId)
        {
            var records = this._reportsService.GetSetUpConfigDetailsReport(companyId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.SetupConfigReport,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["CompanyName"].ColumnName = "Company Name";
            records.Tables[0].Columns["HL7DirectionalWay"].ColumnName = "HL7 Directional Way";
            records.Tables[0].Columns["FteCategory_Desc"].ColumnName = "Fte Category";
            records.Tables[0].Columns["Events"].ColumnName = "Events";
            return Json<DataTable>(records.Tables[0]);
        }
        [Route("GetAllergyMasterExcel/{type}")]
        [HttpGet]
        public JsonResult<DataTable> GetAllergyMasterExcel(int type)
        {
            var records = this._reportsService.GetAllergyMasterExcel(type).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.AllergyMaster,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["AllergyDesc_Id"].ColumnName = type == 1 ? "Allergy Class ID" : "Allergy Drug ID";
            records.Tables[0].Columns["AllergyDesc"].ColumnName = type == 1 ? "Allergy Class Name" : "Allergy Drug Name";
            return Json<DataTable>(records.Tables[0]);
        }
        [Route("GetICDMasterExcel")]
        [HttpGet]
        public JsonResult<DataTable> GetICDMasterExcel()
        {
            var records = this._reportsService.GetICDMasterExcel().Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.ICD10Master,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return Json<DataTable>(records.Tables[0]);
        }
        [Route("GetRoleConfigExcel/{userId}/{roleId}")]
        [HttpGet]
        public JsonResult<DataTable> GetRoleConfigExcel(int userId, int roleId)
        {
            var records = this._reportsService.GetRoleConfigExcel(userId, roleId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.RoleConfig,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["Excel"].ColumnName = "Print Excel";
            records.Tables[0].Columns["PDF"].ColumnName = "Print PDF";
            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Role Name", "Screen", "Read", "Write", "Print Excel", "Print PDF");
            return Json<DataTable>(newTable);
        }
        [Route("GetPharmacyMedsExpiryDateExcel/{checkinFromDate}/{checkinToDate}/{expireFromDate}/{expireTodate}/{nursestationId}/{userId}")]
        [HttpGet]
        public JsonResult<DataTable> GetPharmacyMedsExpiryDateExcel(string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate, string nursestationId,int userId)
        {

            var records = this._reportsService.GetPharmacyMedsExpiryReport(checkinFromDate, checkinToDate, expireFromDate, expireTodate, nursestationId,userId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.MedicationExpirationDateReport,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns.Add("Patient Name", typeof(string), "[ResidentName]+' ('+[DOB]+')'");
            records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
            records.Tables[0].Columns["Check-In Date"].ColumnName = "Most Recent Check-in Date";
            records.Tables[0].Columns["QuantityReceived"].ColumnName = "Most Recent Quantity Received";
            records.Tables[0].Columns["ExpirationDate"].ColumnName = "Most Recent Expiration Date";
            records.Tables[0].Columns["LotNumber"].ColumnName = "Lot#";
            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Patient Name", "Drug Name", "Most Recent Check-in Date", "Most Recent Quantity Received", "Most Recent Expiration Date", "Lot#");
            return Json<DataTable>(newTable);
        }
        [Route("GetEkitMedsExpiryDateExcel/{nursestationId}/{facilityId}/{userId}")]
        [HttpGet]
        public JsonResult<DataTable> GetEkitMedsExpiryDateExcel(string nursestationId, int facilityId, int userId)
        {

            var records = this._reportsService.GetEkitMedsExpiryReport(nursestationId, facilityId, userId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.EKitMedsExpirationDateReport,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["NurseStationName"].ColumnName = "Nursing Station";
            records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
            records.Tables[0].Columns["Barcode"].ColumnName = "Barcode";
            records.Tables[0].Columns["Lot#"].ColumnName = "Lot#";
            records.Tables[0].Columns["ExpirationDate"].ColumnName = "Exp Date";
            records.Tables[0].Columns["Quantity on Hand"].ColumnName = "Qty on Hand";
            records.Tables[0].Columns["Expired"].ColumnName = "Expired";


            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station", "Drug Name", "Barcode", "Lot#", "Exp Date", "Qty on Hand", "Expired");
            return Json<DataTable>(newTable);
        }

        [Route("GetEkitMedsDispensingReport/{fromdate}/{todate}/{userId}/{nursestationid}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetEkitMedsDispensingReport(string fromdate, string todate, int userId, string nursestationid, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetEkitMedsDispensingReport() in ReportsController----");
            var records = this._reportsService.GetEkitMedsDispensingReport(fromdate, todate, userId, nursestationid).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptEkitMedsDispensingReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "E-Kit/On-site Medication  Dispensing Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ekit" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetDocAdministerOrderReport/{fromdate}/{todate}/{OrderType}/{userId}/{nursestationid}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetDocAdministerOrderReport(string fromdate, string todate, int OrderType, int userId, string nursestationId, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetDocAdministerOrderReport() in ReportsController----");
            var records = this._reportsService.GetDocAdministerOrderReport(fromdate, todate, OrderType, userId, nursestationId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId??0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptDocAdministerOrderReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (facilitylogo.Count>0 && ((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Forced Pass Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:"+" " + fromDate);
            rd.SetParameterValue("ToDate", "To:"+" " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "DocAdminOrder" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetCertifiedOrderReport/{userId}/{certTimeId}/{patientId}/{facilityId}/{nursingStationName}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetCertifiedOrderReport(int userId, int certTimeId, int patientId, int facilityId, string nursingStationName, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetCertifiedOrderReport() in ReportsController----");
            if(nursingStationName=="null")
            {
                nursingStationName = this._reportsService.GetNursingStationNameByPId(patientId).Result;
            }
            var records = this._reportsService.GetCertifiedOrderReport(userId, certTimeId,patientId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptCertifiedOrders.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (facilitylogo.Count!=0 && ((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Order Certification Report");
            rd.SetParameterValue("NursingStationName", nursingStationName);
            rd.SetParameterValue("DateTime", reportDateTime);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "CertifiedOrders" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetCertifiedOrderReport() in ReportsController----");
            return responseMessageResult;
        }

        [Route("GetCertifiedOrderReportExcel/{userId}/{certTimeId}/{patientId}/{facilityId}/{nursingStationName}/{dateTime}")]
        [HttpGet]
        public JsonResult<DataTable> GetCertifiedOrderReportExcel(int userId, int certTimeId, int patientId, int facilityId, string nursingStationName, string dateTime)
        {

          
            this._log.Debug("---Executing GetCertifiedOrderReportExcel() in ReportsController----");
            if (nursingStationName == "null")
            {
                nursingStationName = this._reportsService.GetNursingStationNameByPId(patientId).Result;
            }
            var records = this._reportsService.GetCertifiedOrderReport(userId, certTimeId, patientId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.CertifiedOrdersReport,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            // var j = Json<DataTable>(records.Tables[0].Columns);

            records.Tables[0].Columns["DrugName"].ColumnName = "Drug Orders and Treatments";
            //  records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
            records.Tables[0].Columns["OrderType"].ColumnName = "Order Type";
            records.Tables[0].Columns["Directions"].ColumnName = "Directions";
            records.Tables[0].Columns["StartDate"].ColumnName = "Start Date";
            records.Tables[0].Columns["EndDate"].ColumnName = "End Date";
            records.Tables[0].Columns["Schedule"].ColumnName = "Administration Time";




            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Drug Orders and Treatments", "Order Type", "Directions", "Start Date", "End Date", "Administration Time");

            //  newTable.Columns["ResidentName"].ColumnName = "Resident Name";
            return Json<DataTable>(newTable);
        }


        [Route("GetPharmacyMedsExpiryReport/{checkinFromDate}/{checkinToDate}/{expireFromDate}/{expireTodate}/{nursestationId}/{userId}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetPharmacyMedsExpiryReport(string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate, string nursestationId,int userId,int facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetPharmacyMedsExpiryReport() in ReportsController----");
            var records = this._reportsService.GetPharmacyMedsExpiryReport(checkinFromDate, checkinToDate, expireFromDate, expireTodate, nursestationId,userId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptPharmacyMedsExpirationReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (facilitylogo.Count > 0 && ((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]); 
            rd.SetParameterValue("ReportName", "Medication Expiration Date Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var checkinfromDate = Convert.ToDateTime(checkinFromDate).GetDateTimeFormats()[3];
            var CheckintoDate = Convert.ToDateTime(checkinToDate).GetDateTimeFormats()[3];
            var ExpirefromDate = Convert.ToDateTime(expireFromDate).GetDateTimeFormats()[3];
            var ExpiretoDate = Convert.ToDateTime(expireTodate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "Check-inFrom:"+" " + checkinfromDate);
            rd.SetParameterValue("ToDate", "Check-inTo:"+" " + CheckintoDate);
            rd.SetParameterValue("ExpireFromDate", "ExpiryFrom:"+" " + ExpirefromDate);
            rd.SetParameterValue("ExpireToDate", "ExpiryTo:"+" " + ExpiretoDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "MedicationExpiration" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetPharmacyMedsExpiryReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetEkitMedsExpiryReport/{nursestationId}/{facilityId}/{userId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetEkitMedsExpiryReport(/*string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate,*/ string nursestationId, int facilityId,int userId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetEkitMedsExpiryReport() in ReportsController----");
            var records = this._reportsService.GetEkitMedsExpiryReport(/*checkinFromDate, checkinToDate, expireFromDate, expireTodate,*/ nursestationId,facilityId, userId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "crptEkitExpirationDateReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (facilitylogo.Count > 0 && ((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "Ekit/On-site Meds Expiration Date Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            //var checkinfromDate = Convert.ToDateTime(checkinFromDate).GetDateTimeFormats()[3];
            //var CheckintoDate = Convert.ToDateTime(checkinToDate).GetDateTimeFormats()[3];
            //var ExpirefromDate = Convert.ToDateTime(expireFromDate).GetDateTimeFormats()[3];
            //var ExpiretoDate = Convert.ToDateTime(expireTodate).GetDateTimeFormats()[3];
            //rd.SetParameterValue("FromDate", "Check-inFrom:"+" " + checkinfromDate);
            //rd.SetParameterValue("ToDate", "Check-inTo:"+" " + CheckintoDate);
            //rd.SetParameterValue("ExpireFromDate", "ExpiryFrom:"+" " + ExpirefromDate);
            //rd.SetParameterValue("ExpireToDate", "ExpiryTo:"+" " + ExpiretoDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Ekit/On-siteMedsExpiration" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitMedsExpiryReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetCPOEOrderDetailsReport/{porder_id}/{quantityId}/{nsId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetCPOEOrderDetailsReport(int porder_id, int quantityId, int nsId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            int facilityId = Convert.ToInt32( this._facilityService.GetNurseStationById(nsId).Result.Facility_Id);
            this._log.Debug("---Executing GetCPOEOrderDetailsReport() in ReportsController----");
            var records = this._reportsService.GetCPOEOrderDetailsReport(porder_id, quantityId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "CrptCPOEOrderDetailsReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (facilitylogo.Count > 0 && ((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            string recordAllDia = this._reportsService.GetEmarResidentdetailsReportAllergyAndDiagnosisCPOE(0, 0, "", "", porder_id).Result;

            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "CPOE Order Details Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            rd.SetParameterValue("Allergies", recordAllDia);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "CPOE Order Details" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitMedsExpiryReport() in ReportsController----");
            return responseMessageResult;
        }

        [Route("GetProfileCertifiedOrderReport/{userId}/{certTimeId}/{patientId}/{facilityId}/{nursingStationName}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetProfileCertifiedOrderReport(int userId, string certTimeId, int patientId, int facilityId, string nursingStationName, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetCertifiedOrderReport() in ReportsController----");
            if (nursingStationName == "null")
            {
                nursingStationName = this._reportsService.GetNursingStationNameByPId(patientId).Result;
            }
            int timeId = certTimeId.StartsWith("P") ? Convert.ToInt32(certTimeId.Substring(1)) : Convert.ToInt32(certTimeId);
            var records = certTimeId.StartsWith("P") ? this._reportsService.GetProfileCertifiedOrderReport(userId, timeId, patientId).Result :this._reportsService.GetCertifiedOrderReport(userId, timeId, patientId).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;
            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "CrptProfileCertifiedOrdersReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (facilitylogo.Count != 0 && ((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "");//Resident Profile Certified Orders Report
            rd.SetParameterValue("NursingStationName", nursingStationName);
            rd.SetParameterValue("DateTime", "");//reportDateTime
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "CertifiedOrders" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetCertifiedOrderReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetScheduleOrderExcel")]
        [HttpPost]
        public JsonResult<DataTable> GetScheduleOrderExcel(ScheduleOrderEntity entity)
        {

            int shiftTimes = 0;
            var records = this._reportsService.GetOrderDetailsReport(entity.PatientIds, entity.FromDate, entity.ToDate,Convert.ToInt32(entity.OrderType), entity.UserId, entity.NusingStationId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.OrderDashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            
            records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
            records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
            records.Tables[0].Columns["Order"].ColumnName = "Drug Name";
            records.Tables[0].Columns["HOA"].ColumnName = "Administration Times";

            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station Name", "ResidentName","Gender","Location", "Drug Name", "Start Date", "Administration Times");
            newTable.Columns["ResidentName"].ColumnName = "Resident Name";
            return Json<DataTable>(newTable);
        }
        [Route("GetProfileCertifiedOrderExcel/{userId}/{certTimeId}/{patientId}/{facilityId}/{nursingStationName}/{dateTime}")]
        [HttpGet]
        public JsonResult<DataTable> GetProfileCertifiedOrderExcel(int userId, string certTimeId, int patientId, int facilityId, string nursingStationName, string dateTime)
        {

           
            this._log.Debug("---Executing GetProfileCertifiedOrderExcel() in ReportsController----");
            if (nursingStationName == "null")
            {
                nursingStationName = this._reportsService.GetNursingStationNameByPId(patientId).Result;
            }
            int timeId = certTimeId.StartsWith("P") ? Convert.ToInt32(certTimeId.Substring(1)) : Convert.ToInt32(certTimeId);
            var records = certTimeId.StartsWith("P") ? this._reportsService.GetProfileCertifiedOrderReport(userId, timeId, patientId).Result : this._reportsService.GetCertifiedOrderReport(userId, timeId, patientId).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.ResidentProfileCertifiedOrdersReport,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
          // var j = Json<DataTable>(records.Tables[0].Columns);

            records.Tables[0].Columns["DrugName"].ColumnName = "Drug Orders and Treatments";
            //  records.Tables[0].Columns.Add("ResidentName", typeof(string), "[Resident Name]+' ('+[DOB]+')'");
            records.Tables[0].Columns["OrderType"].ColumnName = "Order Type";
            records.Tables[0].Columns["Directions"].ColumnName = "Directions";
            records.Tables[0].Columns["StartDate"].ColumnName = "Start Date";
            records.Tables[0].Columns["EndDate"].ColumnName = "End Date";
            records.Tables[0].Columns["Schedule"].ColumnName = "Administration Time";




            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Drug Orders and Treatments", "Order Type", "Directions", "Start Date", "End Date", "Administration Time");
       
          //  newTable.Columns["ResidentName"].ColumnName = "Resident Name";
            return Json<DataTable>(newTable);
        }
        [Route("GetEkitMedicationCheckInReportExcel/{fromdate}/{todate}/{userId}/{nursestationId}")]
        [HttpGet]
        public JsonResult<DataTable> GetEkitMedicationCheckInReportExcel(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetEkitMedicationCheckInReportExcel() in ReportsController----");

            // Get the records
            var records = this._reportsService.GetEkitMedicationCheckInReport(fromdate, todate, userId, nursestationId).Result;

            if (records == null || records.Tables.Count == 0 || records.Tables[0] == null)
            {
                // If records or the first table is null, handle gracefully
                this._log.Error("No records found or table is null.");
                return Json(new DataTable()); // or return an appropriate error response
            }

            // Check if the required columns exist in the dataset
            var table = records.Tables[0];

            if (!table.Columns.Contains("Nurse Station Name") ||
                !table.Columns.Contains("Drug Name") ||
                !table.Columns.Contains("Barcode") ||
                !table.Columns.Contains("#Lot") ||
                !table.Columns.Contains("Expiration Date") ||
                !table.Columns.Contains("Qty Received") ||
                !table.Columns.Contains("Received By") ||
                !table.Columns.Contains("Received Date"))
            {
                this._log.Error("One or more expected columns are missing in the dataset.");
                return Json(new DataTable()); // or return an appropriate error response
            }

            // Perform the renaming of columns
            table.Columns["Nurse Station Name"].ColumnName = "Nursing Station Name";
            table.Columns["Drug Name"].ColumnName = "Drug Name";
            table.Columns["Barcode"].ColumnName = "Barcode";
            table.Columns["#Lot"].ColumnName = "#Lot";
            table.Columns["Expiration Date"].ColumnName = "Expiration Date";
            table.Columns["Qty Received"].ColumnName = "Qty Received";
            table.Columns["Received By"].ColumnName = "Received By";
            table.Columns["Received Date"].ColumnName = "Received Date";  // Make sure no trailing space here

            // Create new table with selected columns
            DataTable newTable = table.DefaultView.ToTable(false,
                "Nursing Station Name", "Drug Name", "Barcode", "#Lot", "Expiration Date",
                "Qty Received", "Received By", "Received Date");

            return Json<DataTable>(newTable);
        }
        [Route("GetEkitMedicationCheckInReport/{fromdate}/{todate}/{userId}/{nursestationid}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetEkitMedicationCheckInReport(string fromdate, string todate, int userId, string nursestationid, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetEkitMedicationCheckInReport() in ReportsController----");
            var records = this._reportsService.GetEkitMedicationCheckInReport(fromdate, todate, userId, nursestationid).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "GetEkitMedicationCheckInReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "E-Kit/On-site Medication Check-In Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:" + " " + fromDate);
            rd.SetParameterValue("ToDate", "To:" + " " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ekit" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetEkitDetailsReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetMedicationQtyonhandUpdateReport/{fromdate}/{todate}/{userId}/{nursestationid}/{facilityId}/{dateTime}")]
        [HttpGet]
        public IHttpActionResult GetMedicationQtyonhandUpdateReport(string fromdate, string todate, int userId, string nursestationid, Nullable<int> facilityId, string dateTime)
        {
            var reportDateTime = dateTime.Replace('-', '/').Replace(',', ':');
            this._log.Debug("---Executing GetMedicationQtyonhandUpdateReport() in ReportsController----");
            var records = this._reportsService.GetMedicationQtyonhandUpdateReport(fromdate, todate, userId, nursestationid).Result;
            var emarlogo = this._reportsService.GetEmarlogo(facilityId ?? 0).Result;
            var facilitylogo = this._reportsService.GetHeaderlogo(null, facilityId).Result;

            ReportDocument rd = new ReportDocument();
            string reportsPath = ConfigurationManager.AppSettings["Reports"].ToString() + "GetMedicationQtyonhandUpdateReport.rpt";
            rd.Load(reportsPath);
            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(emarlogo);
            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.Entities.LogoEntity)(emarlogo[0])).Footerlogo));
            if (((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo.Length > 0)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(((LTCPro.DAL.PrcGetHeaderLogo_Result)(facilitylogo[0])).ReportHeaderLogo));
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                if (image.Size.Width >= image.Size.Height + 120)
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 2500;
                }
                else
                {
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Height = 900;
                    rd.Subreports[1].ReportDefinition.ReportObjects[1].Width = 1000;
                }
            }
            else
            {
                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
            }
            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
            }
            else
            {
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
            }
            rd.SetDataSource(records.Tables[0]);
            rd.SetParameterValue("ReportName", "E-Kit/On-site Medication Qty On-Hand Updates Report");
            rd.SetParameterValue("DateTime", reportDateTime);
            var fromDate = Convert.ToDateTime(fromdate).GetDateTimeFormats()[3];
            var toDate = Convert.ToDateTime(todate).GetDateTimeFormats()[3];
            rd.SetParameterValue("FromDate", "From:" + " " + fromDate);
            rd.SetParameterValue("ToDate", "To:" + " " + toDate);
            Stream s = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            MemoryStream ms = new MemoryStream();

            s.CopyTo(ms);
            rd.Close();
            rd.Dispose();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ekit" + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully GetMedicationQtyonhandUpdateReport() in ReportsController----");
            return responseMessageResult;
        }
        [Route("GetEkitDetailsReportExcel/{userId}/{nursestationId}/{orderType}")]
        [HttpGet]
        public JsonResult<DataTable> GetEkitDetailsReportExcel(int userId, string nursestationId, int orderType)
        {

            var records = this._reportsService.GetEkitDetailsReport(userId, nursestationId, orderType).Result;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Excel,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            records.Tables[0].Columns["Nurse Station Name"].ColumnName = "Nursing Station";
            records.Tables[0].Columns["DrugName"].ColumnName = "Drug Name";
            //records.Tables[0].Columns["ExpirationDate"].ColumnName = "Expiration Date";
            records.Tables[0].Columns["Quantity on Hand"].ColumnName = "Qty on Hand";
            records.Tables[0].Columns["assessedon"].ColumnName = "Assessed On";
            DataTable newTable = records.Tables[0].DefaultView.ToTable(false, "Nursing Station", "Drug Name", "Qty on Hand", "Assessed On");
            return Json<DataTable>(newTable);
        }

    }


    //[Route("GetNurseComments")]
    //[HttpGet]
    //public JsonResult<NurseCommentTypeDropEntity> GetNurseComments()
    //{
    //    //this._log.Debug("---Executing GetWeightList(Patient_Id) in AssessmentController----");
    //    var nurseComments = this._reportsService.GetNurseComments().Result;
    //    //this._log.Debug("---Executed Successfully GetWeightList(Patient_Id) in AssessmentController----");
    //    return Json<List<NurseCommentTypeDropEntity>>(nurseComments);
    //}

    public class CustomImageTagProcessor : iTextSharp.tool.xml.html.Image
    {
        public override IList<IElement> End(IWorkerContext ctx, Tag tag, IList<IElement> currentContent)
        {
            IDictionary<string, string> attributes = tag.Attributes;
            string src;
            if (!attributes.TryGetValue(HTML.Attribute.SRC, out src))
                return new List<IElement>(1);

            if (string.IsNullOrEmpty(src))
                return new List<IElement>(1);

            if (src.StartsWith("data:image/", StringComparison.InvariantCultureIgnoreCase))
            {
                // data:[<MIME-type>][;charset=<encoding>][;base64],<data>
                var base64Data = src.Substring(src.IndexOf(",") + 1);
                var imagedata = Convert.FromBase64String(base64Data);
                var image = iTextSharp.text.Image.GetInstance(imagedata);

                var list = new List<IElement>();
                var htmlPipelineContext = GetHtmlPipelineContext(ctx);
                list.Add(GetCssAppliers().Apply(new Chunk((iTextSharp.text.Image)GetCssAppliers().Apply(image, tag, htmlPipelineContext), 0, 0, true), tag, htmlPipelineContext));
                return list;
            }
            else
            {
                return base.End(ctx, tag, currentContent);
            }
        }

    }


}