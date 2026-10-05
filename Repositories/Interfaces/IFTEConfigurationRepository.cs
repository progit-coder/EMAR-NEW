using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IFTEConfigurationRepository
    {
        int InsertUpdateFTEConfiguration(FTEConfigurationEntity fteConfiguration);
        List<FTConfigurationGridEntity> GetAllFTEConfigurationsList();
        FTEConfigurationEntity GetFTEConfigurationDetailsByID(int fteConfigId);
        List<string> GetFilesList();
    }
}
