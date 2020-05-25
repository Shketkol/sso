using Microsoft.AspNetCore.Mvc;
using System;
using SSO.Models.DatabaseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSO.Services.Abstract;
using SSO.Models.Errors;
using SSO.Database;
using Microsoft.EntityFrameworkCore;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;

        public RolesController(ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }

        /// <summary>
        /// Method to get all Roles-table records by company id.
        /// </summary>
        /// <returns>
        /// <see cref="List{Role}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.RoleByCompany)]
        public async Task<ActionResult<IEnumerable<Role>>> GetRolesByCompanyId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var roleList = await (from roles in db.Roles
                                         .Include(z => z.Company)
                                          where roles.CompanyId == id
                                          select roles).ToListAsync();

                    if (roleList.Any())
                        return roleList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all records from Roles-table.
        /// Calls method <see cref="GetAllRoles()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{Role}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.RoleList)]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                var roleList = await (from roles in db.Roles
                                      .Include(z => z.Company)
                                      select roles).ToListAsync();

                if (roleList.Any())
                    return roleList;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from Roles-table.
        /// </summary>
        /// <returns>
        /// <see cref="Role"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">Role record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.RoleItem)]
        public async Task<ActionResult<Role>> GetRole(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var getRole = await (from roles in db.Roles
                                        .Include(z => z.Company)
                                         where roles.Id == id
                                         select roles).FirstAsync();

                    if (getRole != null)
                        return getRole;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to create a specific record for Roles-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Role"/> object is null.</exception>
        /// <param name="role"><see cref="Role"/> object.</param>
        [HttpPost]
        [Route(Routes.RoleCreate)]
        public async Task<ActionResult<Role>> InsertRole([FromBody]Role role, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (role != null)
                    if (permissionCheck.IsAllowed(inputToken, "edit_roles"))
                    {
                        using var db = new SSOContext();
                        db.Roles.Add(role);
                        await db.SaveChangesAsync();

                        db.Entry(role).State = EntityState.Detached;
                        return await (from roles in db.Roles
                                        .Include(z => z.Company)
                                      where roles.Id == role.Id
                                      select roles).FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from Roles-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Role"/> object is null.</exception>
        /// <param name="company"><see cref="Role"/> object.</param>
        [HttpPut]
        [Route(Routes.RoleUpdate)]
        public async Task<ActionResult<Role>> UpdateRole([FromBody]Role role,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id==role.Id)
                    if (permissionCheck.IsAllowed(inputToken, "edit_roles"))
                    {
                        using var db = new SSOContext();
                        var oldRole = await db.Roles.FindAsync(id);
                        if (oldRole == null)
                            throw new ArgumentNullException();

                        role.CreatedAt = oldRole.CreatedAt;
                        db.Entry(oldRole).State = EntityState.Detached;

                        db.Roles.Update(role);
                        await db.SaveChangesAsync();

                        db.Entry(role).State = EntityState.Detached;
                        return await (from roles in db.Roles
                                        .Include(z => z.Company)
                                      where roles.Id == id
                                      select roles).FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from Roles-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Role"/> object is null.</exception>
        /// <param name="id">Role record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.RoleDelete)]
        public async Task<ActionResult<Role>> DeleteRole(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "edit_roles"))
                    {
                        using var db = new SSOContext();
                        var getRole = await db.Roles.FindAsync(id);

                        if (getRole == null)
                            throw new ArgumentNullException();

                        getRole.DeletedAt = DateTimeOffset.UtcNow;
                        db.Roles.Update(getRole);
                        await db.SaveChangesAsync();

                        db.Entry(getRole).State = EntityState.Detached;
                        return await (from roles in db.Roles
                                      .IgnoreQueryFilters()
                                        .Include(z => z.Company)
                                      where roles.Id == id
                                      select roles).FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
