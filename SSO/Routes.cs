using System;

namespace SSO
{
    /// <summary>
    /// Class for setting path for traffic.
    /// </summary>
    public class Routes
    {
        //Functional controllers
        public const string CheckLogin = "login-authorize";

        public const string CheckToken = "check-token";

        public const string RefreshToken = "refresh-token";

        public const string GetRequiredUserInfo = "get-user-info/{id}";

        public const string GetMyUserInfo = "my-user-info";

        public const string GetAllowedPermissions = "allowed-permissions";

        public const string SetPermissionsToRole = "set-permissions-to-role/{id}";

        public const string SetRolesToUser = "set-roles-to-user/{id}";

        //RolePermission
        public const string RolePermissionCreate = "role-permission";

        public const string RolePermissionUpdate = "role-permission/{id}";

        public const string RolePermissionDelete = "role-permission/{id}";

        public const string RolePermissionList = "role-permission";

        public const string RolePermissionItem = "role-permission/{id}";

        public const string RolePermissionByPermission = "role-permission/permission/{id}";

        public const string RolePermissionByRole = "role-permission/role/{id}";

        //roles
        public const string RoleCreate = "role";

        public const string RoleUpdate = "role/{id}";

        public const string RoleDelete = "role/{id}";

        public const string RoleList = "role";

        public const string RoleItem = "role/{id}";

        public const string RoleByCompany = "role/company/{id}";

        //permissions

        public const string PermissionCreate = "permission";

        public const string PermissionUpdate = "permission/{id}";

        public const string PermissionDelete = "permission/{id}";

        public const string PermissionSearchList = "permission";

        public const string PermissionItem = "permission/{id}";

        //companies
        public const string CompanyCreate = "company";

        public const string CompanyUpdate = "company/{id}";

        public const string CompanyDelete = "company/{id}";

        public const string CompanyList = "company";

        public const string CompanyItem = "company/{id}";

        //user params
        public const string UserParamsCreate = "user-params";

        public const string UserParamsUpdate = "user-params/{id}";

        public const string UserParamsDelete = "user-params/{id}";

        public const string UserParamsList = "user-params";

        public const string UserParamsItem = "user-params/{id}";

        public const string UserParamsByUser = "user-params/user/{id}";

        //user roles
        public const string UserRoleCreate = "user-role";

        public const string UserRoleUpdate = "user-role/{id}";

        public const string UserRoleDelete = "user-role/{id}";

        public const string UserRoleList = "user-role";

        public const string UserRoleItem = "user-role/{id}";

        public const string UserRoleByRole = "user-role/role/{id}";

        public const string UserRoleByUser = "user-role/user/{id}";

        //user
        public const string UserCreate = "users";

        public const string UserUpdate = "users/{id}";

        public const string UserDelete = "users/{id}";

        public const string UserList = "users";

        public const string UserItem = "users/{id}";

        public const string UserByCompany = "users/company/{id}";

        //company-airports
        public const string CompanyAirportsCreate = "company-airports";

        public const string CompanyAirportsUpdate = "company-airports/{id}";

        public const string CompanyAirportsDelete = "company-airports/{id}";

        public const string CompanyAirportsList = "company-airports";

        public const string CompanyAirportsItem = "company-airports/{id}";

        public const string CompanyAirportsByCompany = "company-airports/company/{id}";

        //invite
        public const string InviteCreate = "invite";

        public const string InviteUpdate = "invite/{id}";

        public const string InviteDelete = "invite/{id}";

        public const string InviteList = "invite";

        public const string InviteItem = "invite/{id}";

        public const string InviteByCompany = "invite/company/{id}";

        //invite-roles
        public const string InviteRolesCreate = "invite-roles";

        public const string InviteRolesUpdate = "invite-roles/{id}";

        public const string InviteRolesDelete = "invite-roles/{id}";

        public const string InviteRolesList = "invite-roles";

        public const string InviteRolesItem = "invite-roles/{id}";

        public const string InviteRolesByInvite = "invite-roles/invite/{id}";

        public const string InviteRolesByRole = "invite-roles/role/{id}";
    }
}
