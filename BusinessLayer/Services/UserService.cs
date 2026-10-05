using LTCPro.Entities;
using LTCPro.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Repositories;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public class UserService : IUserService
    {

        private readonly IAutoMapper _autoMapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _log;

        public UserService(IAutoMapper autoMapper, IUserRepository userRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._userRepository = userRepository;
            this._log = log;
        }
        public async Task<IList> GetUsersMasterGrid(int UserId)
        {
            this._log.Debug("---Executing GetUsersMasterGrid() in UserService----");
            return await Task.FromResult<IList>(this._userRepository.GetUsersMasterGrid(UserId));
        }
        public async Task<List<UserDropEntity>> GetUserDropData()
        {
            this._log.Debug("---Executing GetUserDropData() in UserService----");
            return await Task.FromResult<List<UserDropEntity>>(this._userRepository.GetUserDropData());
        }
        public async Task<int> InsertUpdateUserMaster(UserEntity user)
        {
            this._log.Debug("---Executing InsertUpdateUserMaster() in UserService----");
            return await Task.FromResult<int>(this._userRepository.InsertUpdateUserMaster(user));
        }

        public async Task<UserEntity> GetUserDetailsByID(int userId)
        {
            this._log.Debug("---Executing GetUserDetailsByID() in UserService----");
            return await Task.FromResult<UserEntity>(this._userRepository.GetUserDetailsByID(userId));
        }
        public async Task<UserCustomEntity> FindUser(string userName, string password)
        {
            this._log.Debug("---Executing FindUser() in UserService----");
            return await Task.FromResult<UserCustomEntity>(this._userRepository.FindUser(userName, password));
        }
        public async Task<int> CheckUser(string userName, string password)
        {
            this._log.Debug("---Executing CheckUser() in UserService----");
            return await Task.FromResult<int>(this._userRepository.CheckUser(userName, password));
        }
        public Task<int> InsertUserToken(int userId, string userToken)
        {
            this._log.Debug("---Executing InsertUserToken() in UserService----");
            return Task.FromResult<int>(this._userRepository.InsertUserToken(userId, userToken));
        }
        public async Task<string> GetUserToken(int userId)
        {
            this._log.Debug("---Executing GetUserToken() in UserService----");
            return await Task.FromResult<string>(this._userRepository.GetUserToken(userId));
        }
        public Task<Int64> InsertUserSession(int userId, int roleId, string userIp, string browser)
        {
            this._log.Debug("---Executing InsertUserSession() in UserService----");
            return Task.FromResult<Int64>(this._userRepository.InsertUserSession(userId, roleId, userIp, browser));
        }
        public async Task<int> LogoutUser(int userId)
        {
            this._log.Debug("---Executing LogoutUser() in UserService----");
            return await Task.FromResult<int>(this._userRepository.LogoutUser(userId));
        }
        public async Task<List<UserEntity>> GetStockReportUserDropData()
        {
            this._log.Debug("---Executing GetStockReportUserDropData() in UserService----");
            return await Task.FromResult<List<UserEntity>>(this._userRepository.GetStockReportUserDropData());
        }
        public async Task<int> UpdateUsersStatus(List<UserEntity> data)
        {
            this._log.Debug("---Executing UpdateUsersStatus() in UserService----");
            return await Task.FromResult<int>(this._userRepository.UpdateUsersStatus(data));
        }
        public async Task<int> ResetUserPwdByAdmin(UserEntity Obj)
        {
            this._log.Debug("---Executing ResetUserPwdByAdmin() in UserService----");
            return await Task.FromResult<int>(this._userRepository.ResetUserPwdByAdmin(Obj));
        }

        public async Task<List<UserDropEntity>> GetUsersByNSID(int nS_Id)
        {
            this._log.Debug("---Executing GetUsersByNSID() in UserService----");
            return await Task.FromResult<List<UserDropEntity>>(this._userRepository.GetUsersByNSID(nS_Id));
        }
        public async Task<int> AutoLogoutUser(int userId)
        {
            this._log.Debug("---Executing AutoLogoutUser() in UserService----");
            return await Task.FromResult<int>(this._userRepository.AutoLogoutUser(userId));
        }
    }
}
