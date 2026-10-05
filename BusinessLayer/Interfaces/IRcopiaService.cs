using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
   public interface IRcopiaService
    {

        Task<RcopiaEntity> GetXMLData();
        Task<string> InsertXMLData(Int32 Id, string Response);
    }
}
