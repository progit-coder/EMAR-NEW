using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IFileInformationRepository
    {
        int InsertFileInformation(FileInformationEntity entity);
        List<FileInformationEntity> GetInboundOutboundFiles(int companyId, string fileCategory);
        List<HlFieldsEntity> GetHlFieldsInformation(int FileID);
        FileInformationEntity GetInboundOutboundFileById(int fileId);
        int SaveInboundFilesDownGridData(List<int> fileId, int userID);
        FileInformationViewCustomEntity GetFileAckData(int FileId);
        InboundDashBoardDisplay GetInboundDashboard();
        InboundDashBoardDisplay GetInboundDashboardPopup(int companyId, string createdDate);
        List<FileInformationEntity> GetInboundFilesByStatus(int companyId, int fileStatus, string fromDate, string toDate,string patientName);
        FilesCountEntity GetFilesCount(int CompanyId);
        List<ResidentDropEntity> GetInboundResidentsDrop(int userId, int companyId);
        InboundFilesGridEntity GetInboundFilesByResident(int companyId, string fileCategory, string fromDate, string toDate, int currentPage, int pageSize, string searchValue, string patientName = "", int status = 2);
        //List<FileInformationEntity> GetInboundRejectedFiles(string fromDate, string toDate);


    }
}
