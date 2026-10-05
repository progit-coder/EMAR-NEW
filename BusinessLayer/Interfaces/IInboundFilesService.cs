using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IInboundFilesService
    {
        Task<FTPCustomEntity> DecryptFTPData(int companyId);
        Task<List<InboundFileInformationEntity>> GetInboundFilesFromFTP(int companyId);
        Task<List<InboundFileInformationEntity>> GetInboundFilesFromLocal(int companyId);
        Task<int> SaveInboundFilesData(List<string> records);
        Task<List<FileInformationEntity>> GetSavedFilesList(int companyId, string fileCategory);
        Task<List<HlFieldsEntity>> GetHlFieldsInformation(int FileID);
        Task<int> SaveInboundFilesDownGridData(List<int> records, int UserId);
        int ImportInboundFiles();
        Task<FileInformationViewCustomEntity> GetFileAckData(int FileId);
        Task<InboundDashBoardDisplay> GetInboundDashboard();
        Task<InboundDashBoardDisplay> GetInboundDashboardPopup(int companyId, string createdDate);
        Task<int> GetInboundFileCountByTime(string body);
        Task<List<FlotChartEntity>> GetInboundFoltChartData(string body);
        Task<int> GetInboundFileErrorCountByTime(string body);
        Task<List<FlotChartEntity>> GetInboundImportFoltChartData(string body);
        string ReceiveMessageService(int fteConfigId);
        Task<List<InboundHeaderCountEntity>> GetInboundSuccessErrorCount(int companyId);
        Task<List<FileInformationEntity>> GetInboundFilesByStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName);
        string HLSevenServiceStatus(int Port);
        Task<FilesCountEntity> GetFilesCount(int companyId);
        Task<List<ResidentDropEntity>> GetInboundResidentsDrop(int userId, int companyId);
        Task<InboundFilesGridEntity> GetInboundFilesByResident(int companyId, string fileCategory, string fromDate, string toDate, int currentPage, int pageSize, string searchValue, string patientName = "", int status = 2);
        int ResendInboundFile(ResendFileEntity entity);
        //Task<List<FileInformationEntity>> GetInboundRejectedFiles(string fromDate, string toDate);
    }
}
