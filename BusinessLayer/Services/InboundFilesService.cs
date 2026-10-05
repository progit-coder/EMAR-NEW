using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using EncDec;
using LTCPro.Repositories;
using FluentFTP;
using System.Net;
using System.IO;
using System.Configuration;
namespace LTCPro.ServiceLayer
{
    public class InboundFilesService : IInboundFilesService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IFTEConfigurationRepository _fteConfigRepository;
        private readonly IFileInformationRepository _fileInfoRepository;
        private readonly ILogger _log;
        private readonly ICommonRepository _commonRepository;

        public InboundFilesService(IAutoMapper autoMapper, IFTEConfigurationRepository fteConfigRepository, IFileInformationRepository fileInfoRepository, ILogger log, CommonRepository commonRepository)
        {
            this._autoMapper = autoMapper;
            this._fteConfigRepository = fteConfigRepository;
            this._fileInfoRepository = fileInfoRepository;
            this._log = log;
            this._commonRepository = commonRepository;
        }
        public async Task<FTPCustomEntity> DecryptFTPData(int companyId)
        {
            FTPCustomEntity entity = new FTPCustomEntity();
            this._log.Debug("---Executing GetAllFTEConfigurationsList() in FTEConfigurationService----");
            FTEConfigurationEntity config = this._fteConfigRepository.GetFTEConfigurationDetailsByID(companyId);
            if (config != null)
            {
                clsEncDec decrypt = new clsEncDec();
                entity.Company_Id = config.Company_Id;
                entity.ServerIp = decrypt.psDecrypt(config.ServerIp);
                entity.UserName = decrypt.psDecrypt(config.UserName);
                entity.Password = decrypt.psDecrypt(config.Password);
            }
            return await Task.FromResult<FTPCustomEntity>(entity);
        }
        public async Task<List<InboundFileInformationEntity>> GetInboundFilesFromFTP(int companyId)
        {
            this._log.Debug("---Executing GetInboundFiles() in InboundFilesService---");

            var fteConfigs = this.DecryptFTPData(companyId).Result;
            string[] ar = new string[] { };
            string path = "";
            if (fteConfigs.ServerIp != "")
            {
                ar = fteConfigs.ServerIp.Split('\\');
            }
            using (FtpClient conn = new FtpClient())
            {
                if (ar.Length > 0)
                {
                    conn.Host = ar[2].ToString();
                    path = ar[3].ToString();
                }
                else
                {
                    throw new Exception("Invalid login details");
                }
                conn.Credentials = new NetworkCredential(fteConfigs.UserName, fteConfigs.Password);
                //conn.EncryptionMode = FtpEncryptionMode.Explicit;
                //conn.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
                conn.Connect();
                //Get latest modified files first
                List<string> savedFiles = this._fteConfigRepository.GetFilesList();
                List<FtpListItem> listing;
                string inboundFolderPath = ConfigurationManager.AppSettings.GetValues("InboundPath")[0].ToString();
                if (savedFiles.Count > 0)
                {
                    listing = conn.GetListing("/" + path + inboundFolderPath).Where(ft => !savedFiles.Any(sf => sf == ft.Name)).OrderByDescending(o => o.Modified).ToList();
                    //listing = conn.GetListing("/ITTeam/InBound/").Where(ft => !savedFiles.Any(sf => sf == ft.Name)).OrderByDescending(o => o.Modified).ToList();
                }
                else
                {
                    listing = conn.GetListing("/" + path + inboundFolderPath).OrderByDescending(o => o.Modified).ToList();
                }
                List<InboundFileInformationEntity> filesList = listing.Select(i => new InboundFileInformationEntity
                {
                    CompanyId = companyId,
                    FileName = i.Name,
                    FileSize = i.Size,
                    ModifiedDate = i.Modified,
                    EncryptedDate = i.Modified.AddMinutes(-1),
                    DecryptedDate = i.Modified.AddSeconds(-20)
                }).ToList();
                //This code is for Downloading
                List<string> files = listing.Select(o => o.FullName).ToList();
                conn.DownloadFiles(ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString(), files, false, FtpVerify.None, FtpError.None);

                conn.Disconnect();
                return await Task.FromResult<List<InboundFileInformationEntity>>(filesList);
            }
        }
        public async Task<List<InboundFileInformationEntity>> GetInboundFilesFromLocal(int companyId)
        {
            List<InboundFileInformationEntity> filesList = new List<InboundFileInformationEntity>();
            this._log.Debug("---Executing GetInboundFilesFromLocal() in InboundFilesService---");
            InboundFileInformationEntity fileEntity;
            string folderPath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            var directory = new DirectoryInfo(folderPath);
            List<FileInfo> pendingFiles = directory.GetFiles().OrderByDescending(f => f.LastWriteTimeUtc).Take(50).ToList();
            if (pendingFiles.Count > 0)
            {
                foreach (FileInfo item in pendingFiles)
                {
                    fileEntity = new InboundFileInformationEntity();
                    fileEntity.FileName = item.Name;
                    fileEntity.FileSize = item.Length;
                    fileEntity.ModifiedDate = File.GetLastWriteTimeUtc(folderPath + "/" + item.Name);
                    fileEntity.EncryptedDate = fileEntity.ModifiedDate.AddMinutes(-1);
                    fileEntity.DecryptedDate = fileEntity.ModifiedDate.AddSeconds(-20);
                    fileEntity.CompanyId = companyId;
                    filesList.Add(fileEntity);
                }
            }
            else
            {
                return await this.GetInboundFilesFromFTP(companyId);
            }
            return await Task.FromResult<List<InboundFileInformationEntity>>(filesList);
        }
        public async Task<int> SaveInboundFilesData(List<string> records)
        {
            this._log.Debug("---Executing SaveInboundFilesData() in InboundFilesService---");
            string sourcePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            string destPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
            FileInformationEntity entity;
            int result = 1;
            foreach (string item in records)
            {
                entity = new FileInformationEntity();
                //string[] filesInDir = Directory.GetFiles(folderPath,item.FileName);
                string contents = File.ReadAllText(sourcePath + item);
                entity.File_Name = item;
                entity.Company_Id = 1;
                entity.File_Data = contents;
                entity.File_Category = 1;
                entity.File_CreatedDate = File.GetLastWriteTimeUtc(sourcePath + item);
                entity.File_Status = 1;
                entity.File_Error = string.Empty;
                File.Move(sourcePath + item, destPath + item);
                result = this._fileInfoRepository.InsertFileInformation(entity);
            }

            return await Task.FromResult<int>(result);
        }
        public async Task<int> SaveInboundFilesDownGridData(List<int> records, int UserId)
        {
            this._log.Debug("---Executing SaveInboundFilesDownGridData() in InboundFilesService----");
            return await Task.FromResult<int>(this._fileInfoRepository.SaveInboundFilesDownGridData(records, UserId));
        }
        public async Task<List<FileInformationEntity>> GetSavedFilesList(int companyId, string fileCategory)
        {
            this._log.Debug("---Executing GetSavedFilesList() in InboundFilesService----");
            return await Task.FromResult<List<FileInformationEntity>>(this._fileInfoRepository.GetInboundOutboundFiles(companyId, fileCategory));

        }
        public async Task<List<HlFieldsEntity>> GetHlFieldsInformation(int FileID)
        {
            this._log.Debug("---Executing GetHlFiledsInformation() in InboundFilesService----");
            return await Task.FromResult<List<HlFieldsEntity>>(this._fileInfoRepository.GetHlFieldsInformation(FileID));
        }
        public int ImportInboundFiles()
        {
            this._log.Debug("---Executing ImportInboundFiles() in InboundFilesService----");
            System.Net.ServicePointManager.Expect100Continue = false;
            WebReference.WebServiceSender importFiles = new WebReference.WebServiceSender();
            importFiles.ImportInboundFiles();
            this._log.Debug("---Executing Completed ImportInboundFiles() in InboundFilesService----");
            return 1;
        }
        public async Task<FileInformationViewCustomEntity> GetFileAckData(int FileId)
        {
            this._log.Debug("---Executing GetFileAckData() in InboundFilesService----");
            return await Task.FromResult<FileInformationViewCustomEntity>(this._fileInfoRepository.GetFileAckData(FileId));
        }
        public async Task<InboundDashBoardDisplay> GetInboundDashboard()
        {
            this._log.Debug("---Executing GetInboundDashboard() in InboundFilesService----");
            return await Task.FromResult<InboundDashBoardDisplay>(this._fileInfoRepository.GetInboundDashboard());

        }
        public async Task<InboundDashBoardDisplay> GetInboundDashboardPopup(int companyId, string createdDate)
        {
            this._log.Debug("---Executing GetInboundDashboardPopup() in InboundFilesService----");
            return await Task.FromResult<InboundDashBoardDisplay>(this._fileInfoRepository.GetInboundDashboardPopup(companyId,createdDate));
        }
        public async Task<int> GetInboundFileCountByTime(string body)
        {
            string folderPath = "";
            if(body=="1")
            folderPath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            else
                folderPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
            DirectoryInfo dirinfo = new DirectoryInfo(folderPath);
            //int val = DateTime.Compare(Convert.ToDateTime("2/3/2019 4:32:38 PM"), time);
            //var filesInOrder2 = from f in dirinfo.EnumerateFiles()
            //                        //where f.CreationTime.ToString() == dt.ToString()
            //                    orderby f.CreationTime
            //                    select f;
            DateTime dt = DateTime.Now.AddSeconds(-3);
            var filesInOrder = from f in dirinfo.EnumerateFiles()
                               where f.CreationTime.ToString()==dt.ToString()
                               orderby f.CreationTime
                               select f;
            return await Task.FromResult<int>(filesInOrder.Count());
        }
        public async Task<int> GetInboundFileErrorCountByTime(string body)
        {
            this._log.Debug("---Executing GetInboundFileErrorCountByTime() in InboundFilesService----");
            string folderCompletedPath = "", folderErrorPath = "";
            folderErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
            folderCompletedPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
            DirectoryInfo dirinfoError = new DirectoryInfo(folderErrorPath);
            DirectoryInfo dirinfoCompleted = new DirectoryInfo(folderCompletedPath);
            //int val = DateTime.Compare(Convert.ToDateTime("2/3/2019 4:32:38 PM"), time);            
            DateTime dt = DateTime.Now.AddSeconds(-3);
            var filesInOrder = from f in dirinfoError.EnumerateFiles()
                               where f.CreationTime.ToString() == dt.ToString()
                               orderby f.CreationTime
                               select f;
            var filesInOrder2 = from f in dirinfoCompleted.EnumerateFiles()
                               where f.CreationTime.ToString() == dt.ToString()
                               orderby f.CreationTime
                               select f;
            return await Task.FromResult<int>(filesInOrder.Count()+ filesInOrder2.Count());
        }
        public async Task<List<FlotChartEntity>> GetInboundFoltChartData(string body)
        {
            string folderPath = "";
            if (body == "1")
                folderPath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            else
                folderPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
            DirectoryInfo dirinfo = new DirectoryInfo(folderPath);
            List<FlotChartEntity> flotData = new List<FlotChartEntity>();
            //int val = DateTime.Compare(Convert.ToDateTime("2/3/2019 4:32:38 PM"), time);   
            TimeSpan lastAccess = DateTime.Now - dirinfo.LastAccessTime;         
            for (int i = -19; i <= 0; i++)
            {
                FlotChartEntity obj = new FlotChartEntity();
                DateTime dt = DateTime.Now.AddSeconds(i);                
                var filesInOrder = from f in dirinfo.EnumerateFiles()
                                   where f.CreationTime.ToString() == dt.ToString()
                                   orderby f.CreationTime
                                   select f;
                obj.count = filesInOrder.Count();
                obj.date = dt;
                obj.lastAccess = lastAccess;
                flotData.Add(obj);
            }
            this._log.Debug("---Executing GetInboundFoltChartData() in InboundFilesService----");
            return await Task.FromResult<List<FlotChartEntity>>(flotData);
        }
        public async Task<List<FlotChartEntity>> GetInboundImportFoltChartData(string body)
        {
            this._log.Debug("---Executing GetInboundImportFoltChartData() in InboundFilesService----");
            string folderCompletedPath = "", folderErrorPath = "";
            folderErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
            folderCompletedPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
            DirectoryInfo dirinfoError = new DirectoryInfo(folderErrorPath);
            DirectoryInfo dirinfoCompleted = new DirectoryInfo(folderCompletedPath);
            List<FlotChartEntity> flotData = new List<FlotChartEntity>();
            //int val = DateTime.Compare(Convert.ToDateTime("2/3/2019 4:32:38 PM"), time);   
            TimeSpan lastAccess = DateTime.Now - dirinfoCompleted.LastAccessTime;
            for (int i = -19; i <= 0; i++)
            {
                FlotChartEntity obj = new FlotChartEntity();
                DateTime dt = DateTime.Now.AddSeconds(i);
                var filesInOrder = from f in dirinfoError.EnumerateFiles()
                                   where f.CreationTime.ToString() == dt.ToString()
                                   orderby f.CreationTime
                                   select f;
                var filesInOrder2 = from f in dirinfoCompleted.EnumerateFiles()
                                   where f.CreationTime.ToString() == dt.ToString()
                                   orderby f.CreationTime
                                   select f;
                obj.count = filesInOrder.Count()+ filesInOrder2.Count();
                obj.date = dt;
                obj.lastAccess = lastAccess;
                flotData.Add(obj);
            }
            return await Task.FromResult<List<FlotChartEntity>>(flotData);
        }
        public string ReceiveMessageService(int fteConfigId)
        {
            int companyId = 0,category=0;
            this._log.Debug("---Executing ReceiveMessageService() in InboundFilesService----");
            System.Net.ServicePointManager.Expect100Continue = false;
            WebReference.WebServiceSender importFiles = new WebReference.WebServiceSender();
            var fteData = _fteConfigRepository.GetFTEConfigurationDetailsByID(fteConfigId);
            companyId = fteData.Company_Id;
            category = fteData.Category;
            string result = string.Empty;
            try
            {
                if (category == 1)
                    result = importFiles.ReceiveMessage();
                else
                    result = importFiles.SendMessages();
            }
            catch(System.Net.WebException ex)
            {
                if (ex.Status.ToString().Contains("Timeout"))
                    return ex.Message;
            }
            this._log.Debug("---Executing Completed ReceiveMessageService() in InboundFilesService----");
            if (result.Contains("Service Already Running"))
                result = "Service Started.";
            return result;
        }
        public async Task<List<InboundHeaderCountEntity>> GetInboundSuccessErrorCount(int companyId)
        {
            this._log.Debug("---Executing GetInboundImportFoltChartData() in InboundFilesService----");
            string folderCompletedPath = "", folderErrorPath = "";
            folderErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
            folderCompletedPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
            DirectoryInfo dirinfoError = new DirectoryInfo(folderErrorPath);
            DirectoryInfo dirinfoCompleted = new DirectoryInfo(folderCompletedPath);
            List<InboundHeaderCountEntity> objList = new List<InboundHeaderCountEntity>();
            int errorCount = dirinfoError.EnumerateFiles().Count();
            InboundHeaderCountEntity objError = new InboundHeaderCountEntity();
            objError.type = "Error";
            objError.count = errorCount;
            objList.Add(objError);
            int completedCount = dirinfoCompleted.EnumerateFiles().Count();
            InboundHeaderCountEntity objCompleted = new InboundHeaderCountEntity();
            objCompleted.type = "Completed";
            objCompleted.count = completedCount;
            objList.Add(objCompleted);
            int totalCount = completedCount + errorCount;
            InboundHeaderCountEntity objTotal = new InboundHeaderCountEntity();
            objTotal.type = "Total";
            objTotal.count = totalCount;
            objList.Add(objTotal);
            return await Task.FromResult< List < InboundHeaderCountEntity >>(objList);
        }
        public async Task<List<FileInformationEntity>> GetInboundFilesByStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName)
        {
            this._log.Debug("---Executing GetInboundFilesByStatus() in InboundFilesService----");
            return await Task.FromResult<List<FileInformationEntity>>(this._fileInfoRepository.GetInboundFilesByStatus(companyId, fileStatus, fromDate, toDate, patientName));

        }
        public string HLSevenServiceStatus(int Port)
        {
            try
            {
                this._log.Debug("---Executing HLSevenServiceStatus() in InboundFilesService----");
                WebReference.WebServiceSender importFiles = new WebReference.WebServiceSender();
                System.Net.ServicePointManager.Expect100Continue = false;
                string Status = importFiles.RecievingMessageStatus(Port);
                this._log.Debug("---Executing Completed HLSevenServiceStatus() in InboundFilesService----");
                return Status;
            }
            catch(Exception ex)
            {
                return "connectionerror";
            }
        }
        public async Task<FilesCountEntity> GetFilesCount(int companyId)
        {
            this._log.Debug("---Executing GetFilesCount() in InboundFilesService----");
            return await Task.FromResult<FilesCountEntity>(this._fileInfoRepository.GetFilesCount(companyId));
        }
        public async Task<List<ResidentDropEntity>> GetInboundResidentsDrop(int userId,int companyId)
        {
            this._log.Debug("---Executing GetInboundResidentsDrop() in InboundFilesService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._fileInfoRepository.GetInboundResidentsDrop(userId,companyId));
        }
        public async Task<InboundFilesGridEntity> GetInboundFilesByResident(int companyId, string fileCategory, string fromDate, string toDate, int currentPage, int pageSize, string searchValue, string patientName = "", int status = 2)
        {
            this._log.Debug("---Executing GetInboundResidentsDrop() in InboundFilesService----");
            return await Task.FromResult<InboundFilesGridEntity>(this._fileInfoRepository.GetInboundFilesByResident(companyId, fileCategory, fromDate, toDate, currentPage, pageSize, searchValue, patientName, status));
        }
        //public async Task<List<FileInformationEntity>> GetInboundRejectedFiles(string fromDate, string toDate)
        //{
        //    this._log.Debug("---Executing GetInboundRejectedFiles() in InboundFilesService----");
        //    return await Task.FromResult<List<FileInformationEntity>>(this._fileInfoRepository.GetInboundRejectedFiles(fromDate, toDate));
        //}
        public int ResendInboundFile(ResendFileEntity entity)
        {
            this._log.Debug("---Executing ResendInboundFile() in InboundFilesService----");
            string filePath = string.Empty;
            string destPath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            filePath = destPath + "/" + "Resend_"+ this._commonRepository.GetNursingStationTimeZoneDate().ToString("MMddyyyyhhmmss") + ".hl7";
            entity.FileData = entity.FileData.Replace("\n","\r\n").Replace("\r", "\r\n");
            System.IO.File.WriteAllText(@filePath, (entity.FileData+"\n"));
            this._log.Debug("---Insert ResendInboundFile " + filePath + " in InboundFilesService----");
            return 1;
        }
    }
}
