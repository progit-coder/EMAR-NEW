using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;

namespace LTCPro.Repositories
{
    public interface IOutboundFileInformationRepository
    {
        int InsertFileInformation(OutBoundFileInformationEntity entity, int patientId,EMAREntities context);
        List<OutBoundFileInformationEntity> GetOutboundFiles(int companyId, string fileCategory, string fromDate, string toDate, string patientName);
        OutBoundFileInformationEntity GetOutboundFileById(int fileId);
        //int GenerateFileSaveData(List<OutboundRecordsCustomEntity> records);
        FileInformationViewCustomEntity GetFileAckDataForOutbound(int FileId);
        List<OutBoundFileInformationEntity> GetOutboundFilesStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName);
        List<ResidentDropEntity> GetOutboundResidentsDrop(int userId, int companyId);
    }
}
