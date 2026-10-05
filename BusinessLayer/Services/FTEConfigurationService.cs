using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using EncDec;

namespace LTCPro.ServiceLayer
{
    public class FTEConfigurationService : IFTEConfigurationService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IFTEConfigurationRepository _fteConfigRepository;
        private readonly IInboundFilesService _iInboundFilesService;
        private readonly ILogger _log;
        public FTEConfigurationService(IAutoMapper autoMapper, IFTEConfigurationRepository fteConfigRepository, ILogger log, IInboundFilesService iInboundFilesService)
        {
            this._autoMapper = autoMapper;
            this._fteConfigRepository = fteConfigRepository;
            this._iInboundFilesService = iInboundFilesService;
            this._log = log;
        }

        public async Task<List<FTConfigurationGridEntity>> GetAllFTEConfigurationsList()
        {
            this._log.Debug("---Executing GetAllFTEConfigurationsList() in FTEConfigurationService----");
            //return await Task.FromResult<List<FTEConfigurationEntity>>(this._fteConfigRepository.GetAllFTEConfigurationsList());
            var arFTE = this._fteConfigRepository.GetAllFTEConfigurationsList();
            clsEncDec encrypt = new clsEncDec();
            //if(arFTE.Count>0)
            //{
            //    
            //    for (int i=0;i<arFTE.Count;i++)
            //    {                    
            //        arFTE[i].UserName = encrypt.psDecrypt(arFTE[i].UserName);
            //        arFTE[i].Password = encrypt.psDecrypt(arFTE[i].Password);
            //        arFTE[i].ServerIp = encrypt.psDecrypt(arFTE[i].ServerIp);
            //        if (arFTE[i].Port != null)
            //            arFTE[i].ConnectionStatus = this._iInboundFilesService.HLSevenServiceStatus(Convert.ToInt16(arFTE[i].Port));
            //    }
            //}
            //Commented By Anusha
            foreach (var item in arFTE)
            {
                item.ServerIp = encrypt.psDecrypt(item.ServerIp);
                //if (item.Port != null)
                //    item.ConnectionStatus = this._iInboundFilesService.HLSevenServiceStatus(Convert.ToInt16(item.Port));
            }
            return await Task.FromResult<List<FTConfigurationGridEntity>>(arFTE);
        }

        public async Task<FTEConfigurationEntity> GetFTEConfigurationDetailsByID(int fteConfigId)
        {
            this._log.Debug("---Executing GetFTEConfigurationDetailsByID() in FTEConfigurationService----");
            //return await Task.FromResult<FTEConfigurationEntity>(this._fteConfigRepository.GetFTEConfigurationDetailsByID(fteConfigId));
            FTEConfigurationEntity ObjEntity = this._fteConfigRepository.GetFTEConfigurationDetailsByID(fteConfigId);
            clsEncDec encrypt = new clsEncDec();
            ObjEntity.UserName = encrypt.psDecrypt(ObjEntity.UserName);
            ObjEntity.Password = encrypt.psDecrypt(ObjEntity.Password);
            ObjEntity.ServerIp = encrypt.psDecrypt(ObjEntity.ServerIp);
            return await Task.FromResult<FTEConfigurationEntity>(ObjEntity);

        }

        public async Task<int> InsertUpdateFTEConfiguration(FTEConfigurationEntity fteConfiguration)
        {
            this._log.Debug("---Executing InsertUpdateFTEConfiguration() in CompanyService----");
            clsEncDec encrypt = new clsEncDec();
            FTEConfigurationEntity entity = new FTEConfigurationEntity();
            entity.FteConfig_Id = fteConfiguration.FteConfig_Id;
            entity.Company_Id = fteConfiguration.Company_Id;
            entity.ServerIp = encrypt.psEncrypt(fteConfiguration.ServerIp);
            entity.Category = fteConfiguration.Category;
            entity.ConnectionType = fteConfiguration.ConnectionType;
            entity.Port = fteConfiguration.Port;
            entity.UserName = encrypt.psEncrypt(fteConfiguration.UserName);
            entity.Password = encrypt.psEncrypt(fteConfiguration.Password);
            entity.FteConfig_Status = fteConfiguration.FteConfig_Status;
            entity.FteConfig_CreatedBy = fteConfiguration.FteConfig_CreatedBy;
            entity.FteConfig_CreatedDate = fteConfiguration.FteConfig_CreatedDate;
            return await Task.FromResult<int>(this._fteConfigRepository.InsertUpdateFTEConfiguration(entity));
        }
    }
}
