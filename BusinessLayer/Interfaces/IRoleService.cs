using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IRoleService
    {
        Task<int> InsertRole(RoleEntity Role);
        Task<RoleEntity> GetRoleDetailsByID(int RoleID);
        Task<List<RoleGridData>> GetRoleDetailsAll();
        Task<List<RoleEntity>> GetRoleDropData();
        Task<int> InsertRoleConfigMaster(RoleConfigEntity roleConfig);
        Task<List<RoleConfigEntity>> GetRoleConfigDetailsAll();
        Task<List<RoleConfigGridEntity>> GetRoleConfigsGrid(int userId,int roleId);
        Task<int> InsertUserFacilityRoleConfig(UserRoleFacilityConfigCustomEntity userConfig);
        Task<List<UserRoleFacilityConfigEntity>> GetUserRoleFacilityConfigByID(int UserRole_Id);
        Task<List<ScreenPermissionsCustomEntity>> GetScreenPermissionsForLoggedInUser(int userId, int roleId);
        Task<List<ScreenEntity>> GetAllScreens(int screen);
        Task<string> GetRoleByUserId(int userId, int facilityId);
        Task<List<RoleConfigEntity>> GetRoleConfigDetailsByScreenID(RoleScreensEntity Ids);
        Task<UserIdCustomEntity> GetUserEmailByUserName(string userName);        
        int SendForgotPasswordMail(string userName, int? userId);
        Task<RoleConfigEntity> GetRoleConfigDetailsByID(int RoleConfig_Id);
        Task<UserRoleFacilityConfigCustomEntity> GetUserRoleFacilityConfigByRoleId(int roleId, int userId, int facilityId);
        Task<List<UserRolesEntity>> GetRolesByUser(string userName,string password);
        Task<string> GetUserOTPCheckStatus(int UserId, string OTP);
        Task<int> ResetPassword(int UserId, string PWD);
        Task<string> GetLockOTPCheckStatus(int UserId, string OTP);
        Task<List<UserRoleFacilityConfigGridEntity>> UserRoleFacilityConfigGrid(UserRoleConfigIds Ids);
        Task<int> UpdateNewUserpassword(string userName, string password, string newPassword);
        Task<int> UpdateRoleConfigsStatus(List<RoleConfigEntity> data);
        Task<int> UpdateRolesStatus(List<RoleEntity> data);
        Task<List<ScreenEntity>> GetDefaultScreenDropData();
        Task<int> InsertDefaultScreen(DefaultScreenEntity data);
        Task<List<RoleDropdownEntity>> GetSubRolesList();
        Task<List<ScreenEntity>> GetSubRolesScreenList(int roleID,string status);
        Task<MailConfigEntity> GetMailconfigDetails(int id);
    }
}
