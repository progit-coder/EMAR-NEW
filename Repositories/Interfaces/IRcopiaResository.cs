using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IRcopiaResository
    {
        RcopiaEntity GetXMLData();
        string InsertXMLData(Int32 Id, string Response);
    }
}
