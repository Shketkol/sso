using System;
using System.Collections.Generic;
using System.Linq;
using SSO.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SSO.Services.Abstract;
using SSO.Models.Errors;
using SSO.Database;
using Microsoft.EntityFrameworkCore;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;

        public PermissionsController(ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }

        /// <summary>
        /// Method to get all records from Permissions-table.
        /// Calls method <see cref="GetAllPermissions()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{Permission}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.PermissionSearchList)]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAllPermissions([FromHeader(Name = "Authorization")] string inputToken, string searchValue= "")
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                List<Permission> permissionList;
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    permissionList = await (from permissions in db.Permissions
                                            select permissions).ToListAsync();
                }
                else
                {
                    permissionList = await db.Permissions
                       .Where(x => x.Slug.Contains(searchValue) || x.Table.Contains(searchValue))
                       .ToListAsync();
                }

                if (permissionList.Any())
                    return permissionList;
            }

            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from Permissions-table.
        /// </summary>
        /// <returns>
        /// <see cref="Permission"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">Permission record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.PermissionItem)]
        public async Task<ActionResult<Permission>> GetPermission(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var getPermission = await (from permissions in db.Permissions
                                               where permissions.Id == id
                                               select permissions).FirstAsync();

                    if (getPermission != null)
                        return getPermission;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to create a specific record for Permissions-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Permission"/> object is null.</exception>
        /// <param name="permission"><see cref="Permission"/> object.</param>
        [HttpPost]
        [Route(Routes.PermissionCreate)]
        public async Task<ActionResult<Permission>> InsertPermission([FromBody]Permission permission, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (permission != null)
                {
                    if (permissionCheck.IsAllowed(inputToken, "edit_permissions"))
                    {
                        using var db = new SSOContext();
                        db.Permissions.Add(permission);
                        await db.SaveChangesAsync();
                        db.Entry(permission).State = EntityState.Detached;
                        return await db.Permissions.FindAsync(permission.Id);
                    }
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from Permissions-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Permission"/> object is null.</exception>
        /// <param name="permission"><see cref="Permission"/> object.</param>
        [HttpPut]
        [Route(Routes.PermissionUpdate)]
        public async Task<ActionResult<Permission>> UpdatePermission([FromBody]Permission permission,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == permission.Id)
                    if (permissionCheck.IsAllowed(inputToken, "edit_permissions"))
                    {
                        using var db = new SSOContext();
                        var oldPermission = await db.Permissions.FindAsync(id);
                        if (oldPermission == null)
                            throw new ArgumentNullException();

                        permission.CreatedAt = oldPermission.CreatedAt;
                        db.Entry(oldPermission).State = EntityState.Detached;

                        db.Permissions.Update(permission);
                        await db.SaveChangesAsync();

                        db.Entry(permission).State = EntityState.Detached;
                        return await db.Permissions.FindAsync(permission.Id);
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from Permissions-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Permission"/> object is null.</exception>
        /// <param name="id">Permission record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.PermissionDelete)]
        public async Task<ActionResult<Permission>> DeletePermission(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "edit_permissions"))
                    {
                        using var db = new SSOContext();
                        var getPermission = await (from permissions in db.Permissions
                                                   where permissions.Id == id
                                                   select permissions).FirstAsync();
                        if (getPermission == null)
                            throw new ArgumentNullException();

                        getPermission.DeletedAt = DateTimeOffset.UtcNow;
                        db.Permissions.Update(getPermission);
                        await db.SaveChangesAsync();

                        db.Entry(getPermission).State = EntityState.Detached;
                        return await db.Permissions
                            .IgnoreQueryFilters()
                            .Where(x => x.Id == id).FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
