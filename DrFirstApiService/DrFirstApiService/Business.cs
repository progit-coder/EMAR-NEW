using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DrFirstApiService
{
    public class Business
    {
        DataAccess dataAccess;
        public Business()
        {
            dataAccess = new DataAccess();
        }
        public void GetResidentsList()
        {
            //Get Company Ids
            DataTable dtCompanyID = dataAccess.GetCompanyID();

            foreach (DataRow drRows in dtCompanyID.Rows)
            {
                int companyId = Convert.ToInt32(drRows["Company_Id"].ToString());

                DataSet ds = dataAccess.GetResidentsList(companyId);

                DataTable dtTable = ds.Tables[0];

                string folderPath = ConfigurationManager.AppSettings["DrFirstResponseXML"].ToString() + DateTime.Now.ToString("MMddyyyyhhmmss");
                Directory.CreateDirectory(folderPath);
                //Directory.CreateDirectory(folderPath + "/NoPrescriptions");
                //Directory.CreateDirectory(folderPath + "/Prescriptions");
                foreach (DataRow dtRow in dtTable.Rows)
                {
                    foreach (string dc in dtRow.ItemArray)
                    {
                        var mrNumber = dc;
                        string xmlData = GetXmlFromDrFirst(mrNumber);

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(xmlData);

                        XmlNodeList xNodeList = xml.SelectNodes("/RCExtResponse/Response/PrescriptionList");
                        string prescriptionList = string.Empty;
                        foreach (XmlNode xNode in xNodeList)
                        {
                            prescriptionList = xNode["Number"].InnerText;
                        }
                        if (prescriptionList != "0")
                        {
                            //ToDo: Check against existing prescription RcopiaID 
                            string result = SaveXML(xmlData, mrNumber, folderPath, companyId);// + "/Prescriptions");
                        }
                        //else {
                        //    string result = SaveXML(xmlData, mrNumber, folderPath + "/NoPrescriptions");
                        //}
                    }
                }
            }
        }
        public string SaveXML(string xmlData, string mrNumber, string folderPath, int companyId)
        {

            string filePath = folderPath + "/" + mrNumber + "_" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".xml";

            System.IO.File.WriteAllText(@filePath, xmlData);
           // int CompanyID = Convert.ToInt32(ConfigurationManager.AppSettings["CompanyId"]);
            int val = dataAccess.SaveDownloadedPrescriptionsDetails(companyId, mrNumber, filePath, DateTime.Now);
            var result = dataAccess.SavePrescriptions(xmlData);
            return result;
        }
        public string GetXmlFromDrFirst(string mrNumber)
        {
            int apiMinusHours = Convert.ToInt32(ConfigurationManager.AppSettings["ApiMinusHours"]);

            string xmlData = @"<RCExtRequest version = '4'>
	<Caller>
		<VendorName>pavendor2991</VendorName>
		<VendorPassword>zrpxtgch</VendorPassword>
	</Caller>
	<SystemName>pavendor2991</SystemName>
	<RcopiaPracticeUsername>ph98003</RcopiaPracticeUsername>
	<Request>
		<Command>update_prescription</Command>
		<LastUpdateDate>" + DateTime.Now.AddHours(-apiMinusHours) + @"</LastUpdateDate>
		<Patient>
			<RcopiaID></RcopiaID>
			<ExternalID>" + mrNumber + @"</ExternalID>
		</Patient>
		<Status>completed</Status>
	</Request>
</RCExtRequest>";

            string url = "https://update201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes("xml=" + xmlData);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "text/xml";
            req.ContentLength = data.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string returnXML = sr.ReadToEnd();
            return returnXML;
        }
    }
}
