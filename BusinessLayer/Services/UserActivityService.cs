using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Repositories;
using LTCPro.Entities;

namespace LTCPro.ServiceLayer
{
    public class UserActivityService:IUserActivityService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly ILogger _log;

        public UserActivityService(IAutoMapper autoMapper, IUserActivityRepository userActivityRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._userActivityRepository = userActivityRepository;
            this._log = log;
        }
        //public async Task<Int64> InsertUserSession(int userId, string systemIp, string browserName)
        //{
        //    this._log.Debug("---Executing InsertUserSession() in UserActivityService----");
        //    return await Task.FromResult<Int64>(this._userActivityRepository.InsertUserSession(userId, systemIp, browserName));
        //}
        public async Task<int> InsertUserActivityDetails(UserActivityDetailEntity entity)
        {
            this._log.Debug("---Executing InsertUserActivityDetails() in UserActivityService----");
            return await Task.FromResult<int>(this._userActivityRepository.InsertUserActivityDetails(entity));
        }
    }
}
