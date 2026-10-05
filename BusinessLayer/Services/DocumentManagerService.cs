using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using LTCPro.DAL;
using System.Data.Entity;
using System.IO;
using System.Configuration;
using System.Web;

namespace LTCPro.ServiceLayer
{
    public class DocumentManagerService : IDocumentManagerService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDocumentManagerRepository _documentmanagerRepository;
        private readonly ILogger _log;
        public DocumentManagerService(IAutoMapper autoMapper, IDocumentManagerRepository documentmanagerRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._documentmanagerRepository = documentmanagerRepository;
            this._log = log;
        }
        public async Task<int> UploadResidentDocument(UploadedDocumentEntity entity, HttpPostedFile file)
        {
            this._log.Debug("---Executing UploadResidentDocument() in DocumentManagerService----");
            return await Task.FromResult<int>(this._documentmanagerRepository.UploadResidentDocument(entity, file));
        }
        public async Task<List<UploadedDocumentEntity>> GetUploadedDocsByfolderid(int folderid)
        {
            this._log.Debug("---Executing GetUploadedDocsByfolderid() in DocumentManagerService----");
            return await Task.FromResult<List<UploadedDocumentEntity>>(this._documentmanagerRepository.GetUploadedDocsByfolderid(folderid));
        }

        public async Task<List<DocFloderCustomEntity>> GetDocFolders()
        {
            this._log.Debug("---Executing GetDocFolders() in DocumentManagerService----");
            return await Task.FromResult<List<DocFloderCustomEntity>>(this._documentmanagerRepository.GetDocFolders());

        }

        public async Task<List<UploadedDocumentEntity>> GetUploadedDocuments(int patientId, string folderIds)
        {
            this._log.Debug("---Executing GetUploadedDocuments() in DocumentManagerService----");
            return await Task.FromResult<List<UploadedDocumentEntity>>(this._documentmanagerRepository.GetUploadedDocuments(patientId, folderIds));
        }

        public async Task<UploadedDocumentEntity> DocumentsDownload(int patientdocId)
        {
            this._log.Debug("---Executing DocumentsDownload() in DocumentManagerService----");
            return await Task.FromResult<UploadedDocumentEntity>(this._documentmanagerRepository.DocumentsDownload(patientdocId));

        }

        public async Task<int> DeleteDocument(int patientdocId)
        {
            this._log.Debug("---Executing DeleteDocument() in DocumentManagerService----");
            return await Task.FromResult<int>(this._documentmanagerRepository.DeleteDocument(patientdocId));
        }

        public async Task<List<DocFolderEntity>> GetFolderNames()
        {
            this._log.Debug("---Executing GetFolderNames() in DocumentManagerService----");
            return await Task.FromResult<List<DocFolderEntity>>(this._documentmanagerRepository.GetFolderNames());
        }

        public async Task<int> InsertFolders(DocFolderEntity entity)
        {
            this._log.Debug("---Executing InsertFolders() in DocumentManagerService----");
            return await Task.FromResult<int>(this._documentmanagerRepository.InsertFolders(entity));
        }
    }
}
