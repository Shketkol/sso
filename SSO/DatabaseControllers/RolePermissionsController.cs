using System;
using System.Collections.Generic;
using SSO.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using SSO.Services.Abstract;
using SSO.Models.Errors;
using System.Threading.Tasks;
using SSO.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;

        public RolePermissionsController(ITokenCheck tokenCheck)
        {
            this.tokenCheck = tokenCheck;
        }
        /// <summary>
        /// Method to get all RolePermissions-table records by role id.
        /// </summary>
        /// <returns>
        /// <see cref="List{RolePermission}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.RolePermissionByRole)]
        public async Task<ActionResult<IEnumerable<RolePermission>>> GetRolePermissionsByRoleId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var rolesPermissionList = await (from role_perm in db.RolePermissions
                                                    .Include(x => x.Permission)
                                                    .Include(z => z.Role)
                                                    .ThenInclude(z1 => z1.Company)
                                                     where role_perm.RoleId == id
                                                     select role_perm).ToListAsync();

                    if (rolesPermissionList.Any())
                        return rolesPermissionList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all RolePermissions-table records by permission id.
        /// </summary>
        /// <returns>
        /// <see cref="List{RolePermission}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.RolePermissionByPermission)]
        public async Task<ActionResult<IEnumerable<RolePermission>>> GetRolePermissionsByPermissionId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var rolesPermissionList = await (from role_perm in db.RolePermissions
                                               .Include(x => x.Permission)
                                               .Include(z => z.Role)
                                               .ThenInclude(z1 => z1.Company)
                                                     where role_perm.PermissionId == id
                                                     select role_perm).ToListAsync();

                    if (rolesPermissionList.Any())
                        return rolesPermissionList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all records from RolePermissions-table.
        /// Calls method <see cref="GetAllRolePermissions()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{RolePermission}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.RolePermissionList)]
        public async Task<ActionResult<IEnumerable<RolePermission>>> GetAllRolePermissions([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                var rolesPermissionList = await (from role_perm in db.RolePermissions
                                                .Include(x => x.Permission)
                                                .Include(z => z.Role)
                                                .ThenInclude(z1 => z1.Company)
                                                 select role_perm).ToListAsync();

                if (rolesPermissionList.Any())
                    return rolesPermissionList;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from RolePermissions-table.
        /// </summary>
        /// <returns>
        /// <see cref="RolePermission"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">RolePermission record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.RolePermissionItem)]
        public async Task<ActionResult<RolePermission>> GetRolePermission(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var rolePermission = await (from role_perm in db.RolePermissions
                                          .Include(x => x.Permission)
                                               .Include(z => z.Role)
                                               .ThenInclude(z1 => z1.Company)
                                          where role_perm.Id == id
                                          select role_perm).FirstAsync();

                    if (rolePermission != null)
                        return rolePermission;
                }
            throw new ArgumentNullException();
        }

        /*
        /// <summary>
        /// Method to create a specific record for RolePermissions-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="RolePermission"/> object is null.</exception>
        /// <param name="rolePermission"><see cref="RolePermission"/> object.</param>
        [HttpPost]
        [Route(Routes.RolePermissionCreate)]
        public IActionResult InsertRolePermission([FromBody]RolePermission rolePermission, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken) == 200)
                if (rolePermission != null)
                {
                    if (RolePermissionsCRUD.AddRolePermission(rolePermission))
                        throw new SuccessCustomException();
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from RolePermissions-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="RolePermission"/> object is null.</exception>
        /// <param name="rolePermission"><see cref="RolePermission"/> object.</param>
        [HttpPut]
        [Route(Routes.RolePermissionUpdate)]
        public IActionResult UpdateRolePermission([FromBody]RolePermission rolePermission, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken) == 200)
                if (rolePermission != null)
                {
                    if (RolePermissionsCRUD.UpdateRolePermission(rolePermission))
                        throw new SuccessCustomException();
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from RolePermissions-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="RolePermission"/> object is null.</exception>
        /// <param name="id">RolePermission record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.RolePermissionDelete)]
        public IActionResult DeleteRolePermission(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken) == 200)
                if (id != 0)
                {
                    if (RolePermissionsCRUD.DeleteRolePermission(id))
                        throw new SuccessCustomException();
                }
            throw new ArgumentNullException();
        }*/
    }
}
