using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LTCPro.Repositories
{
    public interface IDocumentManagerRepository
    {
        int UploadResidentDocument(UploadedDocumentEntity entity, HttpPostedFile file);
        List<UploadedDocumentEntity> GetUploadedDocsByfolderid(int folderid);
        List<DocFloderCustomEntity> GetDocFolders();
        string GetFolderLocation(int FolderId);
        string GetPatientName(int patientId);
        List<UploadedDocumentEntity> GetUploadedDocuments(int patientId, string folderIds);
        UploadedDocumentEntity DocumentsDownload(int patientdocId);
        int DeleteDocument(int patientdocId);
        List<DocFolderEntity> GetFolderNames();
        int InsertFolders(DocFolderEntity entity);
    }
}
