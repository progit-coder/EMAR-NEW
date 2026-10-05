using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.IO;
using System.Web;
using System.Configuration;

namespace LTCPro.Repositories
{
    public class DocumentManagerRepository : IDocumentManagerRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public DocumentManagerRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
        }
        public int UploadResidentDocument(UploadedDocumentEntity entity, HttpPostedFile file)
        {
            var record = this.autoMapper.Map<UploadedDocumentEntity, UploadedDocument>(entity);
            if (record.PatientDoc_Id == 0)
            {

                string parentFolderLocation = GetFolderLocation((int)record.FolderID);
                var httpRequest = HttpContext.Current.Request;

                var patientname = GetPatientName((int)record.Patient_Id);
                string filename = patientname + "_" + record.DocName;
                var postedFile = httpRequest.Files[0];
                var filePath = ConfigurationManager.AppSettings.GetValues("DocumentManger")[0].ToString() + parentFolderLocation  + patientname + "_" + record.DocName;
                var createDirectory = ConfigurationManager.AppSettings.GetValues("DocumentManger")[0].ToString() + parentFolderLocation;
                if (!Directory.Exists(createDirectory))
                {
                     Directory.CreateDirectory(createDirectory);
                }
                postedFile.SaveAs(filePath);

                record.DocName = patientname + "_" + record.DocName;
                record.DocLocation = parentFolderLocation;
                this.dbContext.UploadedDocuments.Add(record);
                this.dbContext.SaveChanges();

                return 1;
            }
            return 0;
        }
        public string GetFolderLocation(int FolderId)
        {
            var filepath = this.dbContext.DocFolders.Where(id => id.FolderID == FolderId).Select(f => f.FolderLocation).FirstOrDefault();
            return filepath;
        }

        public List<UploadedDocumentEntity> GetUploadedDocsByfolderid(int folderid)
        {
            var uploaddoc = (from pd in this.dbContext.UploadedDocuments
                             join co in this.dbContext.DocFolders on pd.FolderID equals co.FolderID
                             where pd.FolderID == folderid
                             select new UploadedDocumentEntity
                             {
                                 PatientDoc_Id = pd.PatientDoc_Id,
                                 Patient_Id = pd.Patient_Id,
                                 DocName = pd.DocName,
                                 DocType = pd.DocType,
                                 DocLocation = pd.DocLocation,
                                 DocDescription = pd.DocDescription,
                                 FolderID = pd.FolderID,
                                 UDocuments_CreatedDate = pd.UDocuments_CreatedDate,
                                 UDocuments_CreatedBy = pd.UDocuments_CreatedBy,
                                 FolderName = co.FolderName

                             }).ToList();

            return uploaddoc;
        }

        public List<DocFloderCustomEntity> GetDocFolders()
        {
            //ParentFolders
            var pfolders = this.dbContext.DocFolders.Where(f => f.FolderParentID == 0).Select(e => new DocFloderCustomEntity
            {
                text = e.FolderName,
                value = e.FolderID,
                @checked = false,
                FolderParentID = e.FolderParentID,


            }).ToList();

            //   var cfolders = this.dbContext.DocFolders.Where(f => f.FolderParentID != 0).ToList();

            foreach (var item in pfolders)
            {
                // entity = new DocFloderCustomEntity();
                if (item.FolderParentID == 0)
                {
                    //item.children = cfolders.Where(sm => sm.FolderParentID == item.value).Select(e => new DocFloderCustomEntity
                    //{
                    //    text = e.FolderName,
                    //    value = e.FolderID,
                    //    @checked = false,
                    //    FolderParentID = e.FolderParentID,
                    //}).ToList();
                    item.children = GetChildren(item);

                }

            }
            return pfolders;

        }

        private List<DocFloderCustomEntity> GetChildren(DocFloderCustomEntity item)
        {
            var cfolders = this.dbContext.DocFolders.Where(f => f.FolderParentID != 0).ToList();

            item.children = cfolders.Where(sm => sm.FolderParentID == item.value).Select(e => new DocFloderCustomEntity
            {
                text = e.FolderName,
                value = e.FolderID,
                @checked = false,
                FolderParentID = e.FolderParentID,
            }).ToList();

            foreach (var child in item.children)
            {

                child.children = GetChildren(child);
            }
            return item.children;
        }

        public string GetPatientName(int patientId)
        {
            var patientname = this.dbContext.Demographics.Where(id => id.Patient_Id == patientId).Select(name => name.PatientFirstName).FirstOrDefault();
            return patientname;
        }

        public List<UploadedDocumentEntity> GetUploadedDocuments(int patientId, string folderIds)
        {
            if (folderIds == "null" )
            {
                return (from pd in this.dbContext.UploadedDocuments
                        join co in this.dbContext.DocFolders on pd.FolderID equals co.FolderID
                        where pd.Patient_Id == patientId
                        select new UploadedDocumentEntity
                        {
                            PatientDoc_Id = pd.PatientDoc_Id,
                            Patient_Id = pd.Patient_Id,
                            DocName = pd.DocName,
                            DocType = pd.DocType,
                            DocLocation = pd.DocLocation,
                            DocDescription = pd.DocDescription,
                            FolderID = pd.FolderID,
                            UDocuments_CreatedDate = pd.UDocuments_CreatedDate,
                            UDocuments_CreatedBy = pd.UDocuments_CreatedBy,
                            FolderName = co.FolderName

                        }).OrderBy(item=>item.FolderName).ToList();
            }
            else
            {
                var folders = folderIds.Split(',').Select(Int32.Parse).ToList();
                return (from pd in this.dbContext.UploadedDocuments
                        join co in this.dbContext.DocFolders on pd.FolderID equals co.FolderID
                        where folders.Contains((int)pd.FolderID) && pd.Patient_Id == patientId
                        select new UploadedDocumentEntity
                        {
                            PatientDoc_Id = pd.PatientDoc_Id,
                            Patient_Id = pd.Patient_Id,
                            DocName = pd.DocName,
                            DocType = pd.DocType,
                            DocLocation = pd.DocLocation,
                            DocDescription = pd.DocDescription,
                            FolderID = pd.FolderID,
                            UDocuments_CreatedDate = pd.UDocuments_CreatedDate,
                            UDocuments_CreatedBy = pd.UDocuments_CreatedBy,
                            FolderName = co.FolderName

                        }).OrderBy(item => item.FolderName).ToList();
            }
        }

        public UploadedDocumentEntity DocumentsDownload(int patientdocId)
        {
            var doc = this.dbContext.UploadedDocuments.Where(pi => pi.PatientDoc_Id == patientdocId).FirstOrDefault();
            return this.autoMapper.Map<UploadedDocument, UploadedDocumentEntity>(doc);

        }

        public int DeleteDocument(int patientdocId)
        {
            var remove = this.dbContext.UploadedDocuments.Where(pi => pi.PatientDoc_Id == patientdocId).FirstOrDefault();
            var record = this.dbContext.UploadedDocuments.Remove(remove);
            this.dbContext.SaveChanges();
            string destPath = ConfigurationManager.AppSettings.GetValues("DocumentManger")[0].ToString();
            var filePath = destPath + remove.DocLocation + remove.DocName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return 1;
            }
            else
                return 0;
        }

        public List<DocFolderEntity> GetFolderNames()
        {
            var records = this.dbContext.DocFolders.Where(f => f.Folder_Status == 1).ToList();
            return this.autoMapper.Map<List<DocFolder>, List<DocFolderEntity>>(records);
        }

        public int InsertFolders(DocFolderEntity entity)
        {
            var records = this.autoMapper.Map<DocFolderEntity, DocFolder>(entity);
            var fname = this.dbContext.DocFolders.Where(id => id.FolderID == entity.FolderParentID).Select(name => name.FolderName).FirstOrDefault();
            if (records.FolderParentID == 0)
            {
                records.FolderLocation = "/" + records.FolderName + "/";
                var path = ConfigurationManager.AppSettings["DocumentManger"].ToString();
                string pathString = System.IO.Path.Combine(path, records.FolderName);
                System.IO.Directory.CreateDirectory(pathString);
                this.dbContext.DocFolders.Add(records);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DocumentManager,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = records.FolderID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return records.FolderID;
            }
            else if (records.FolderParentID != 0)
            {
                string parentFolderLocation = GetFolderLocation((int)records.FolderParentID);
                records.FolderLocation = parentFolderLocation + records.FolderName + "/";
                var path = ConfigurationManager.AppSettings["DocumentManger"].ToString() + records.FolderLocation;
                System.IO.Directory.CreateDirectory(path);
                this.dbContext.DocFolders.Add(records);
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DocumentManager,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = records.FolderID.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return records.FolderID;
            }
            return 0;
        }
    }
}
