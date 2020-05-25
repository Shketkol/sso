using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSO.Models;
using SSO.Models.DatabaseModels;
using SSO.Models.Errors;
using SSO.Services.Abstract;

namespace SSO.Controllers
{
    [ApiController]
    public class ValuesChangingController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly ISetPermissionsToRole setPermissionsToRole;
        private readonly ISetUserRoles setUserRoles;
        private readonly IPermissionCheck permissionCheck;
        public ValuesChangingController(ITokenCheck tokenCheck, ISetPermissionsToRole setPermissionsToRole, ISetUserRoles setUserRoles, IPermissionCheck permissionCheck)
        {
            this.tokenCheck = tokenCheck;
            this.setPermissionsToRole = setPermissionsToRole;
            this.setUserRoles = setUserRoles;
            this.permissionCheck = permissionCheck;
        }

        /// <summary>
        /// Method to set a certain amount of roles for a specific user.
        /// <para>This method can validate token using <see cref="ITokenCheck.TokenChecking(string)"/> 
        /// and other inputs. Calls method <see cref="ISetUserRoles.AssignUserToRoles(int,int[])"/>.</para>
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when one parameter is null.</exception>
        /// <param name="id">An {id} integer from URL.</param>
        /// <param name="rolesArray">An integer array of roles id.</param>
        /// <param name="inputToken">A json-token in string format.</param>
        [HttpPost]
        [Route(Routes.SetRolesToUser)]
        public IActionResult SetUserRoles(int id, [FromBody] RolesIdModel rolesIdModel, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                if (permissionCheck.IsAllowed(inputToken, "set_roles_to_user"))
                    if (setUserRoles.AssignUserToRoles(id, rolesIdModel.RolesId))
                        throw new SuccessCustomException();
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to set a certain amount of permissions for a specific role.
        /// <para>This method can validate token using <see cref="ITokenCheck.TokenChecking(string)"/>
        /// and other inputs. Calls method <see cref="ISetPermissionsToRole.SetPermissions(int,int[])"/>.</para>
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when one parameter is null.</exception>
        /// <param name="id">An {id} integer from URL.</param>
        /// <param name="rolesArray">An integer array of permission id.</param>
        /// <param name="inputToken">A json-token in string format.</param>
        [HttpPost] 
        [Route(Routes.SetPermissionsToRole)]
        public IActionResult SetRolePermissions(int id, [FromBody] PermissionsIdModel permissionsIdModel, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                if (permissionCheck.IsAllowed(inputToken, "set_permissions_to_role"))
                    if (setPermissionsToRole.SetPermissions(id, permissionsIdModel.PermissionsId))
                    throw new SuccessCustomException();
            }
            throw new ArgumentNullException();
        }

    }
}
