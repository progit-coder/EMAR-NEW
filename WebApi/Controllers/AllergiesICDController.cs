using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using ExcelDataReader;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.ComponentModel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI;
using System.Text;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [CustomAuthorizationFilter]
    [RoutePrefix("AllergiesICD")]
    public class AllergiesICDController : ApiController
    {
        private readonly IAllergiesICDService _allergiesICDService;
        private readonly ILogger _log;
        public AllergiesICDController(IAllergiesICDService allergiesICDService, ILogger log)
        {
            this._allergiesICDService = allergiesICDService;
            this._log = log;
        }

        [Route("GetAllergiesByName/{SearchPattern}/{Type}")]
        [HttpGet]
        public JsonResult<List<AllergyInfoMastersEntity>> GetAllergiesByName(string searchPattern, int type)
        {
            this._log.Debug("---Executing GetAllergiesByName() in AllergiesICDController----");
            var records = this._allergiesICDService.GetAllergiesByName(searchPattern, type).Result;
            this._log.Debug("---Executed GetAllergiesByName() in AllergiesICDController----");
            return Json<List<AllergyInfoMastersEntity>>(records);
        }
        [Route("GetActiveAllergiesByName/{SearchPattern}")]
        [HttpGet]
        public JsonResult<List<AllergyInfoMastersEntity>> GetActiveAllergiesByName(string searchPattern)
        {
            this._log.Debug("---Executing GetActiveAllergiesByName() in AllergiesICDController----");
            var records = this._allergiesICDService.GetActiveAllergiesByName(searchPattern).Result;
            this._log.Debug("---Executed GetActiveAllergiesByName() in AllergiesICDController----");
            return Json<List<AllergyInfoMastersEntity>>(records);
        }
        [Route("GetAllAllergiesList/{Type}")]
        [HttpGet]
        public JsonResult<List<AllergyInfoMastersEntity>> GetAllAllergiesList(int type)
        {
            this._log.Debug("---Executing GetAllAllergiesList() in AllergiesICDController----");
            var records = this._allergiesICDService.GetAllAllergiesList(type).Result;
            this._log.Debug("---Executed Successfully GetAllAllergiesList() in AllergiesICDController----");
            return Json<List<AllergyInfoMastersEntity>>(records);
        }
        [Route("GetAllICD10")]
        [HttpGet]
        public JsonResult<List<ICD10Entity>> GetAllICD10()
        {
            this._log.Debug("---Executing GetAllICD10() in AllergiesICDController----");
            var records = this._allergiesICDService.GetAllICD10().Result;
            this._log.Debug("---Executed GetAllICD10() in AllergiesICDController----");
            return Json<List<ICD10Entity>>(records);
        }
        //[Route("GetICDGrid")]
        //[HttpGet]
        //public JsonResult<List<ICD10Entity>> GetICDGrid()
        //{
        //    this._log.Debug("---Executing GetICDGrid() in AllergiesICDController----");
        //    var records = this._allergiesICDService.GetICDGrid().Result;
        //    this._log.Debug("---Executed GetICDGrid() in AllergiesICDController----");
        //    return Json<List<ICD10Entity>>(records);
        //}
        [Route("GetICD10ByRawFormat")]
        [HttpPost]
        public JsonResult<ICD10GridEntity> GetICD10ByRawFormat(SearchCustomEntity data)
        {
            this._log.Debug("---Executing GetICD10ByRawFormat() in AllergiesICDController----");
            var records = this._allergiesICDService.GetICD10ByRawFormat(data).Result;
            this._log.Debug("---Executed GetICD10ByRawFormat() in AllergiesICDController----");
            return Json<ICD10GridEntity>(records);
        }
        [Route("GetActiveICD10ByRawFormat/{SearchPattern}")]
        [HttpGet]
        public JsonResult<List<ICD10Entity>> GetActiveICD10ByRawFormat(string searchPattern)
        {
            this._log.Debug("---Executing GetActiveICD10ByRawFormat() in AllergiesICDController----");
            var records = this._allergiesICDService.GetActiveICD10ByRawFormat(searchPattern).Result;
            this._log.Debug("---Executed GetActiveICD10ByRawFormat() in AllergiesICDController----");
            return Json<List<ICD10Entity>>(records);
        }

        [Route("InsertUpdateAllergy")]
        [HttpPost]
        public int InsertUpdateAllergy(AllergyInfoMastersEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateAllergy() in AllergiesICDController----");
            int result = this._allergiesICDService.InsertUpdateAllergy(entity).Result;
            this._log.Debug("---Executed InsertUpdateAllergy() in AllergiesICDController----");
            return result;
        }
        [Route("InsertAllergyByClassFromFile")]
        [HttpPost]
        public int InsertUpdateAllergyByClassFromFile(string body)
        {
            AllergyInfoMastersEntity entity = JsonConvert.DeserializeObject<AllergyInfoMastersEntity>(body);
            List<AllergyInfoMastersEntity> entities = new List<AllergyInfoMastersEntity>();
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                Stream stream = httpRequest.Files[0].InputStream;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSetResult = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }

                    });
                    entities = dataSetResult.Tables[0].AsEnumerable().Select(dataRow => new AllergyInfoMastersEntity
                    {
                        Allergy_Id = entity.Allergy_Id,
                        AllergyTypeMaster_Id = entity.AllergyTypeMaster_Id,
                        AllergyDesc_Id = dataRow.Field<string>("ClassID"),
                        AllergyDesc = dataRow.Field<string>("ClassName"),
                        Allergy_CreatedBy = entity.Allergy_CreatedBy,
                        AllergyStatus = entity.AllergyStatus,
                        Allergy_CreatedOn = entity.Allergy_CreatedOn
                    }).ToList();
                }
            }
            this._log.Debug("---Executing InsertUpdateAllergyByClassFromFile() in AllergiesICDController----");
            int result = this._allergiesICDService.InsertUpdateAllergyFromFile(entities).Result;
            this._log.Debug("---Executed InsertUpdateAllergyByClassFromFile() in AllergiesICDController----");
            return result;

        }
        [Route("InsertAllergyByDrugFromFile")]
        [HttpPost]
        public int InsertUpdateAllergyByDrugFromFile(string body)
        {
            AllergyInfoMastersEntity entity = JsonConvert.DeserializeObject<AllergyInfoMastersEntity>(body);
            List<AllergyInfoMastersEntity> entities = new List<AllergyInfoMastersEntity>();
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                Stream stream = httpRequest.Files[0].InputStream;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSetResult = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }

                    });
                    entities = dataSetResult.Tables[0].AsEnumerable().Select(dataRow => new AllergyInfoMastersEntity
                    {
                        Allergy_Id = entity.Allergy_Id,
                        AllergyTypeMaster_Id = entity.AllergyTypeMaster_Id,
                        AllergyDesc_Id = dataRow.Field<string>("DrugID"),
                        AllergyDesc = dataRow.Field<string>("DrugName"),
                        Allergy_CreatedBy = entity.Allergy_CreatedBy,
                        AllergyStatus = entity.AllergyStatus,
                        Allergy_CreatedOn = entity.Allergy_CreatedOn

                    }).ToList();
                }
            }

            this._log.Debug("---Executing InsertUpdateAllergyByDrugFromFile() in AllergiesICDController----");
            int result = this._allergiesICDService.InsertUpdateAllergyFromFile(entities).Result;
            this._log.Debug("---Executed InsertUpdateAllergyByDrugFromFile() in AllergiesICDController----");
            return result;

        }
        [Route("InsertICD10")]
        [HttpPost]
        public int InsertUpdateICD10(ICD10Entity entity)
        {
            this._log.Debug("---Executing InsertUpdateICD10() in AllergiesICDController----");
            int result = this._allergiesICDService.InsertUpdateICD10(entity).Result;
            this._log.Debug("---Executed InsertUpdateICD10() in AllergiesICDController----");
            return result;

        }
        [Route("UpdateICDsStatus")]
        [HttpPost]
        public int UpdateICDsStatus(List<ICD10Entity> data)
        {
            this._log.Debug("---Executing UpdateICDsStatus() in AllergiesICDController----");
            var result = this._allergiesICDService.UpdateICDsStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateICDsStatus() in AllergiesICDController----");
            return result;
        }
        [Route("UpdateAllergysStatus")]
        [HttpPost]
        public int UpdateAllergysStatus(List<AllergyInfoMastersEntity> data)
        {
            this._log.Debug("---Executing UpdateAllergysStatus() in AllergiesICDController----");
            var result = this._allergiesICDService.UpdateAllergysStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateAllergysStatus() in AllergiesICDController----");
            return result;
        }
        [Route("InsertICD10FromFile")]
        [HttpPost]
        public int InsertUpdateICD10FromFile(string body)
        {
            ICD10Entity entity = JsonConvert.DeserializeObject<ICD10Entity>(body);
            List<ICD10Entity> entities = new List<ICD10Entity>();
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                Stream stream = httpRequest.Files[0].InputStream;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSetResult = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }

                    });
                    entities = dataSetResult.Tables[0].AsEnumerable().Select(dataRow => new ICD10Entity
                    {
                        ICD10_Id = entity.ICD10_Id,
                        ICD10_RawFormat = dataRow.Field<string>("ICD10"),
                        ICD10_Formatted = dataRow.Field<string>("FormattedICD10"),
                        ICD10_Description = dataRow.Field<string>("Description"),
                        ICD10_CreatedBy = entity.ICD10_CreatedBy,
                        ICD10_Status = entity.ICD10_Status,
                        ICD10_CreatedDate = entity.ICD10_CreatedDate
                    }).ToList();
                }
            }

            this._log.Debug("---Executing InsertUpdateICD10FromFile() in AllergiesICDController----");
            int result = this._allergiesICDService.InsertUpdateICD10FromFile(entities).Result;
            this._log.Debug("---Executed InsertUpdateICD10FromFile() in AllergiesICDController----");
            return result;

        }

        //[Route("ExportAllICD10ToPDF")]
        //[HttpGet]
        //public IHttpActionResult ExportAllICD10ToPdf()
        //{
        //    this._log.Debug("---Executing ExportAllICD10ToPdf() in AllergiesICDController----");
        //    var records = this._allergiesICDService.GetAllICD10().Result;
        //    DataTable dt = this._allergiesICDService.ToDataTable<ICD10Entity>(records);

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //        {
        //            StringBuilder sb = new StringBuilder();

        //            //Generate Header.

        //            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
        //            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>ICD10</b></td></tr>");
        //            sb.Append("<tr><td colspan = '2'></td></tr>");
        //            sb.Append("</table>");
        //            sb.Append("<br />");
        //            sb.Append("<table border = '1'>");
        //            sb.Append("<thead>");
        //            sb.Append("<tr>");
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                sb.Append("<th>");
        //                sb.Append(column.ColumnName);
        //                sb.Append("</th>");
        //            }
        //            sb.Append("</tr>");
        //            sb.Append("</thead>");
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                sb.Append("<tr>");
        //                foreach (DataColumn column in dt.Columns)
        //                {
        //                    sb.Append("<td>");
        //                    sb.Append(row[column]);
        //                    sb.Append("</td>");
        //                }
        //                sb.Append("</tr>");
        //            }

        //            sb.Append("</table>");

        //            //Export HTML String as PDF.
        //            StringReader sr = new StringReader(sb.ToString());

        //            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

        //            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

        //            MemoryStream stream = new MemoryStream();

        //            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

        //            pdfDoc.Open();

        //            htmlparser.Parse(sr);

        //            pdfDoc.Close();

        //            byte[] content = stream.ToArray();

        //            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(content)
        //            };

        //            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //            {
        //                FileName = "ICD10" + DateTime.Now.ToString("yyyy/MM/dd")
        //            };
        //            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
        //            this._log.Debug("---Executed Successfully ExportAllICD10ToPdf() in AllergiesICDController----");
        //            return responseMessageResult;
        //        }

        //    }
        //}
    }
}