using LTCPro.Entities;
using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public interface IUserService
    {
        Task<IList> GetUsersMasterGrid(int UserId);
        Task<List<UserDropEntity>> GetUserDropData();
        Task<int> InsertUpdateUserMaster(UserEntity user);
        Task<UserEntity> GetUserDetailsByID(int userId);
        Task<UserCustomEntity> FindUser(string userName, string password);
        Task<int> InsertUserToken(int userId, string userToken);
        Task<int> LogoutUser(int userId);
        Task<List<UserEntity>> GetStockReportUserDropData();
        Task<int> UpdateUsersStatus(List<UserEntity> data);
        Task<int> ResetUserPwdByAdmin(UserEntity Obj);
        Task<List<UserDropEntity>> GetUsersByNSID(int nS_Id);
        Task<int> AutoLogoutUser(int userId);
    }
}
