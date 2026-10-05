using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IOutboundFilesService
    {
        Task<List<OutBoundFileInformationEntity>> GetOutboundFiles(int companyId, string fileCategory, string fromDate, string toDate, string patientName);
        Task<OutBoundFileInformationEntity> GetOutboundFileById(int fileId);
        //Task<List<OutboundRecordsCustomEntity>> GetOutBoundList();
        //Task<int> GenerateFileSaveData(List<OutboundRecordsCustomEntity> records);
        Task<FileInformationViewCustomEntity> GetFileAckDataForOutbound(int FileId);
        Task<List<OutBoundFileInformationEntity>> GetOutboundFilesStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName);
        Task<List<ResidentDropEntity>> GetOutboundResidentsDrop(int userId, int companyId);
    }
}
