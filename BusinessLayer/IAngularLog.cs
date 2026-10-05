using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IAngularLog
    {
        int ErrorLogging(string errorMessage);
    }
}
