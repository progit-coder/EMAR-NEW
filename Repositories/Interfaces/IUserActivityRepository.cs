using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IUserActivityRepository
    {
        //Int64 InsertUserSession(int userId, string systemIp, string browserName);
        int InsertUserActivityDetails(UserActivityDetailEntity entity);
    }
}
