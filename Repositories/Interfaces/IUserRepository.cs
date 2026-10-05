using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LTCPro.Repositories
{
    public interface IUserRepository
    {
        IList GetUsersMasterGrid(int UserId);
        List<UserDropEntity> GetUserDropData();
        int InsertUpdateUserMaster(UserEntity user);
        UserEntity GetUserDetailsByID(int userId);
        UserDetailsCustomEntity GetUserDetailsByUserId(int userId);
        UserCustomEntity FindUser(string userName, string password);
        int CheckUser(string userName, string password);
        int ValidateUser(string userName, string password);
        int InsertUserToken(int userId, string userToken);
        string GetUserToken(int userId);
        int GetUserId(string UserName, string Password);
        int LogoutUser(int userId);
        Int64 InsertUserSession(int userId, int role, string systemIp, string browserName);
        List<UserEntity> GetStockReportUserDropData();
        int UpdateUsersStatus(List<UserEntity> data);
        int ResetUserPwdByAdmin(UserEntity Obj);
        int GetFacilityOfUser(int dUserId, int facility_Id);
        List<UserDropEntity> GetUsersByNSID(int nS_Id);
        int AutoLogoutUser(int userId);
        int LogoutUserTime(int userId);
    }
}
