using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Web.UI;
using iTextSharp.text.html.simpleparser;

namespace LTCPro.ServiceLayer
{
    public class AllergiesICDService : IAllergiesICDService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IAllergiesICDRepository _allergiesICDRepository;
        private readonly ILogger _log;
        public AllergiesICDService(IAutoMapper autoMapper, IAllergiesICDRepository allergiesICDRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._allergiesICDRepository = allergiesICDRepository;
            this._log = log;
        }

        public async Task<List<AllergyInfoMastersEntity>> GetAllergiesByName(string searchPattern, int type)                 
        {
            this._log.Debug("---Executing GetAllergiesByName() in AllergiesICDService----");
            return await Task.FromResult<List<AllergyInfoMastersEntity>>(this._allergiesICDRepository.GetAllergiesByName(searchPattern, type));
        }
        public async Task<List<AllergyInfoMastersEntity>> GetActiveAllergiesByName(string searchPattern)
        {
            this._log.Debug("---Executing GetActiveAllergiesByName() in AllergiesICDService----");
            return await Task.FromResult<List<AllergyInfoMastersEntity>>(this._allergiesICDRepository.GetActiveAllergiesByName(searchPattern));
        }
        public async Task<List<ICD10Entity>> GetAllICD10()
        {
            this._log.Debug("---Executing GetAllICD10() in AllergiesICDService----");
            return await Task.FromResult<List<ICD10Entity>>(this._allergiesICDRepository.GetAllICD10());
        }
        public async Task<List<ICD10Entity>> GetICDGrid()
        {
            this._log.Debug("---Executing GetICDGrid() in AllergiesICDService----");
            return await Task.FromResult<List<ICD10Entity>>(this._allergiesICDRepository.GetICDGrid());
        }
        public async Task<int> UpdateICDsStatus(List<ICD10Entity> data)
        {
            this._log.Debug("---Executing UpdateICDsStatus() in AllergiesICDService----");
            return await Task.FromResult<int>(this._allergiesICDRepository.UpdateICDsStatus(data));
        }
        public async Task<int> UpdateAllergysStatus(List<AllergyInfoMastersEntity> data)
        {
            this._log.Debug("---Executing UpdateAllergysStatus() in AllergiesICDService----");
            return await Task.FromResult<int>(this._allergiesICDRepository.UpdateAllergysStatus(data));
        }
        public async Task<ICD10GridEntity> GetICD10ByRawFormat(SearchCustomEntity data)
        {
            this._log.Debug("---Executing GetICD10ByRawFormat() in AllergiesICDService----");
            return await Task.FromResult<ICD10GridEntity>(this._allergiesICDRepository.GetICD10ByRawFormat(data));
        }
        public async Task<List<ICD10Entity>> GetActiveICD10ByRawFormat(string searchPattern)
        {
            this._log.Debug("---Executing GetActiveICD10ByRawFormat() in AllergiesICDService----");
            return await Task.FromResult<List<ICD10Entity>>(this._allergiesICDRepository.GetActiveICD10ByRawFormat(searchPattern));
        }
        public async Task<int> InsertUpdateAllergy(AllergyInfoMastersEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateAllergy() in AllergiesICDService----");
            return await Task.FromResult<int>(this._allergiesICDRepository.InsertUpdateAllergy(entity));
        }

        public async Task<int> InsertUpdateAllergyFromFile(List<AllergyInfoMastersEntity> entities)
        {
            this._log.Debug("---Executing InsertUpdateAllergyFromFile() in AllergiesICDService----");
            return await Task.FromResult<int>(this._allergiesICDRepository.InsertUpdateAllergyFromFile(entities));
        }
        public async Task<List<AllergyInfoMastersEntity>> GetAllAllergiesList(int type)
        {
            this._log.Debug("---Executing GetAllAllergiesList() in AllergiesICDService----");
            return await Task.FromResult<List<AllergyInfoMastersEntity>>(this._allergiesICDRepository.GetAllAllergiesList(type));
        }

        public async Task<int> InsertUpdateICD10(ICD10Entity entity)
        {
            this._log.Debug("---Executing InsertUpdateICD10() in AllergiesICDService----");
            return await Task.FromResult<int>(this._allergiesICDRepository.InsertUpdateICD10(entity));
        }

        public async Task<int> InsertUpdateICD10FromFile(List<ICD10Entity> entities)
        {
            this._log.Debug("---Executing InsertUpdateICD10FromFile() in AllergiesICDService----");
            return await Task.FromResult<int>(this._allergiesICDRepository.InsertUpdateICD10FromFile(entities));
        }
        public void ExportAllICD10ToPdf()
        {

        }
        public DataTable ToDataTable<T>(List<T> iList)
        {

            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);
                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public StringReader GeneratePDF()
        {
            return null;
            //var records = Task.FromResult<List<AllergyByClassEntity>>(this._allergiesICDRepository.GetAllAllergiesByClass()).Result;
            //DataTable dt = ToDataTable<AllergyByClassEntity>(records);

            //using (StringWriter sw = new StringWriter())
            //{
            //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))     GetAllergiesByNameExcel
            //    {
            //        StringBuilder sb = new StringBuilder();

            //        //Generate Header.

            //        sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
            //        sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
            //        sb.Append("<tr><td colspan = '2'></td></tr>");
            //        sb.Append("<tr><td><b>Order No: </b>");
            //        sb.Append("orderNo");

            //        sb.Append("</td><td align = 'right'><b>Date: </b>");

            //        sb.Append(DateTime.Now);

            //        sb.Append(" </td></tr>");

            //        sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");

            //        sb.Append("companyName");

            //        sb.Append("</td></tr>");

            //        sb.Append("</table>");

            //        sb.Append("<br />");



            //        //Generate Invoice (Bill) Items Grid.

            //        sb.Append("<table border = '1'>");

            //        sb.Append("<thead>");

            //        sb.Append("<tr>");

            //        foreach (DataColumn column in dt.Columns)

            //        {

            //            sb.Append("<th>");

            //            sb.Append(column.ColumnName);

            //            sb.Append("</th>");

            //        }

            //        sb.Append("</tr>");

            //        sb.Append("</thead>");

            //        foreach (DataRow row in dt.Rows)

            //        {

            //            sb.Append("<tr>");

            //            foreach (DataColumn column in dt.Columns)

            //            {

            //                sb.Append("<td>");

            //                sb.Append(row[column]);

            //                sb.Append("</td>");

            //            }

            //            sb.Append("</tr>");

            //        }

            //        // sb.Append("<tr><td align = 'right' colspan = '");

            //        // sb.Append(dt.Columns.Count - 1);

            //        // sb.Append("'>Total</td>");

            //        // sb.Append("<td>");

            //        //// sb.Append(dt.Compute("sum(Total)", ""));

            //        // sb.Append("</td>");

            //        // sb.Append("</tr></table>");

            //        sb.Append("</table>");

            //        //Export HTML String as PDF.

            //        StringReader sr = new StringReader(sb.ToString());

            //        return sr;

            //    }
            //}
        }
        /*
void ExportDataTableToPdf()
{
   var records = Task.FromResult<List<ICD10Entity>>(this._allergiesICDRepository.GetAllICD10());

   Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
   System.IO.MemoryStream mStream = new System.IO.MemoryStream();
   PdfWriter writer = PdfWriter.GetInstance(pdfDoc, mStream);
   int cols = 3;
   int rows = records.Result.Count;


   //HeaderFooter header = new HeaderFooter(new Phrase(Name), false);

   //// Remove the border that is set by default  
   //header.Border = iTextSharp.text.Rectangle.TITLE;
   //// Align the text: 0 is left, 1 center and 2 right.  
   //header.Alignment = Element.ALIGN_CENTER;
   pdfDoc.AddHeader("Test", "Testing");
   // Header.  
   pdfDoc.Open();
   PdfPTable pdfTable = new PdfPTable(cols);
   //pdfTable.BorderWidth = 1; pdfTable.Width = 100;
   //pdfTable.Padding = 1; pdfTable.Spacing = 4;

   //creating table headers  
   for (int i = 0; i < cols; i++)
   {
       PdfPCell cellCols = new PdfPCell();
       Chunk chunkCols = new Chunk();
       cellCols.BackgroundColor = new BaseColor(0, 0, 0);
       iTextSharp.text.Font ColFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, new BaseColor(255, 255, 255));

       chunkCols = new Chunk("Header", ColFont);

       cellCols.AddElement(chunkCols);
       pdfTable.AddCell(cellCols);
   }
   //creating table data (actual result)   

   for (int k = 0; k < rows; k++)
   {
       for (int j = 0; j < cols; j++)
       {
           PdfPCell cellRows = new PdfPCell();
           if (k % 2 == 0)
           {
               cellRows.BackgroundColor = new BaseColor(204, 204, 204);
           }
           else
           {
               cellRows.BackgroundColor = new BaseColor(0, 0, 0);
           }
           iTextSharp.text.Font RowFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
           Chunk chunkRows = new Chunk(records.Rows[k][j].ToString(), RowFont);
           cellRows.Add(chunkRows);

           pdfTable.AddCell(cellRows);
       }
   }

   pdfDoc.Add(pdfTable);
   pdfDoc.Close();
   Response.ContentType = "application/octet-stream";
   Response.AddHeader("Content-Disposition", "attachment; filename=" + Name + "_" + DateTime.Now.ToString() + ".pdf");
   Response.Clear();
   Response.BinaryWrite(mStream.ToArray());
   Response.End();
}
*/
    }
}
