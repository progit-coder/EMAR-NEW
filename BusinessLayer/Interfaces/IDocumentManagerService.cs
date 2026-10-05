using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LTCPro.ServiceLayer
{


    public interface IDocumentManagerService
    {
        Task<int> UploadResidentDocument(UploadedDocumentEntity entity, HttpPostedFile file);
        Task<List<UploadedDocumentEntity>> GetUploadedDocsByfolderid(int folderid);
        Task<List<DocFloderCustomEntity>> GetDocFolders();
        Task<List<UploadedDocumentEntity>> GetUploadedDocuments(int patientId, string folderIds);
        Task<UploadedDocumentEntity> DocumentsDownload(int patientdocId);
        Task<int> DeleteDocument(int patientdocId);
        Task<List<DocFolderEntity>> GetFolderNames();
        Task<int> InsertFolders(DocFolderEntity entity);

    }
}
