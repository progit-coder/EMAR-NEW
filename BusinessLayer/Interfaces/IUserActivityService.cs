using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IUserActivityService
    {
        //Task<Int64> InsertUserSession(int userId, string systemIp, string browserName);
        Task<int> InsertUserActivityDetails(UserActivityDetailEntity entity);
    }
}
