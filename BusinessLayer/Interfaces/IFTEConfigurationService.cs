using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IFTEConfigurationService
    {
        Task<int> InsertUpdateFTEConfiguration(FTEConfigurationEntity fteConfiguration);
        Task<List<FTConfigurationGridEntity>> GetAllFTEConfigurationsList();
        Task<FTEConfigurationEntity> GetFTEConfigurationDetailsByID(int fteConfigId);


    }
}
