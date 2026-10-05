using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.Repositories
{
    public interface IRoleRepository
    {
        int InsertRole(RoleEntity Role);
        RoleEntity GetRoleDetailsByID(int RoleID);
        List<RoleGridData> GetRoleDetailsAll();
        List<RoleEntity> GetRoleDropData();
        int InsertRoleConfigMaster(RoleConfigEntity roleConfig);
        List<RoleConfigEntity> GetRoleConfigDetailsAll();
        List<RoleConfigGridEntity> GetRoleConfigsGrid(int userId,int roleId);
        int InsertUserFacilityRoleConfig(UserRoleFacilityConfigCustomEntity userConfig);
        List<UserRoleFacilityConfigEntity> GetUserRoleFacilityConfigByID(int UserRole_Id);
        List<ScreenPermissionsCustomEntity> GetScreenPermissionsForLoggedInUser(int userId, int roleId);
        List<ScreenEntity> GetAllScreens(int screen);
        string GetRoleByUserId(int userId, int facilityId);
        List<RoleConfigEntity> GetRoleConfigDetailsByScreenID(RoleScreensEntity Ids);
        UserIdCustomEntity GetUserEmailByUserName(string userName);
        UserEntity GetUserDetails(string userName);
        MailConfigEntity GetMailconfigDetails(int Id);
        RoleConfigEntity GetRoleConfigDetailsByID(int RoleConfig_Id);
        UserRoleFacilityConfigCustomEntity GetUserRoleFacilityConfigByRoleId(int roleId, int userId, int facilityId);
        List<UserRolesEntity> GetRolesByUser(int userId);
        int InsertUserOTP(int userId, string otp,int type);
        string GetUserOTPCheckStatus(int UserId, string OTP);
        string GetLockOTPCheckStatus(int UserId, string OTP);
        int ResetPassword(int UserId, string PWD);
        List<UserRoleFacilityConfigGridEntity> UserRoleFacilityConfigGrid(UserRoleConfigIds Ids);
        int UpdateNewUserpassword(string userName, string password, string newPassword);
        int UpdateRoleConfigsStatus(List<RoleConfigEntity> data);
        int UpdateRolesStatus(List<RoleEntity> data);
        List<ScreenEntity> GetDefaultScreenDropData();
        int InsertDefaultScreen(DefaultScreenEntity data);
        List<RoleDropdownEntity> GetSubRolesList();
        List<ScreenEntity> GetSubRolesScreenList(int roleID,string status);

    }
}
