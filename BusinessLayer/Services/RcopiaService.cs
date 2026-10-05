using LTCPro.Entities;
using LTCPro.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
   public class RcopiaService : IRcopiaService
    {

        private readonly IAutoMapper _autoMapper;
        private readonly IRcopiaResository _RcopiaResository;
        private readonly ILogger _log;
        public RcopiaService(IAutoMapper autoMapper, IRcopiaResository RcopiaResository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._RcopiaResository = RcopiaResository;
            this._log = log;
        }

        public async Task<RcopiaEntity> GetXMLData()
        {
            this._log.Debug("---Executing PostDrFirstTest() in FacilityService----");
            return await Task.FromResult<RcopiaEntity>(this._RcopiaResository.GetXMLData());
        }
        public async Task<string> InsertXMLData(Int32 Id,string Response)
        {
            this._log.Debug("---Executing PostDrFirstTest() in FacilityService----");
            return await Task.FromResult<string>(this._RcopiaResository.InsertXMLData(Id, Response));
        }

    }
}
