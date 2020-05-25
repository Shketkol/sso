using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SSO.Database;
using SSO.Models.DatabaseModels;
using SSO.Models.Errors;
using SSO.Services.Abstract;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;

        public UserRolesController(ITokenCheck tokenCheck)
        {
            this.tokenCheck = tokenCheck;
        }

        /// <summary>
        /// Method to get all UserRoles-table records by role id.
        /// </summary>
        /// <returns>
        /// <see cref="List{UserRole}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.UserRoleByRole)]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoleByRoleId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var userRolesList = await (from u_roles in db.UserRoles
                                         .Include(z => z.User)
                                            .ThenInclude(z1 => z1.Company)
                                         .Include(x => x.Role)
                                            .ThenInclude(x1 => x1.Company)
                                               where u_roles.RoleId == id
                                               select u_roles).ToListAsync();

                    if (userRolesList.Any())
                        return userRolesList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all UserRoles-table records by user id.
        /// </summary>
        /// <returns>
        /// <see cref="List{UserRole}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.UserRoleByUser)]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoleByUserId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var userRolesList = await (from u_roles in db.UserRoles
                                         .Include(z => z.User)
                                            .ThenInclude(z1 => z1.Company)
                                         .Include(x => x.Role)
                                            .ThenInclude(x1 => x1.Company)
                                               where u_roles.UserId == id
                                               select u_roles).ToListAsync();

                    if (userRolesList.Any())
                        return userRolesList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all records from UserRoles-table.
        /// Calls method <see cref="GetAllUserRoles()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{UserRole}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.UserRoleList)]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAllUserRoles([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                var userRolesList = await (from u_roles in db.UserRoles
                                     .Include(z => z.User)
                                        .ThenInclude(z1 => z1.Company)
                                     .Include(x => x.Role)
                                        .ThenInclude(x1 => x1.Company)
                                           select u_roles).ToListAsync();

                if (userRolesList.Any())
                    return userRolesList;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from UserRoles-table.
        /// </summary>
        /// <returns>
        /// <see cref="UserRole"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">UserRole record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.UserRoleItem)]
        public async Task<ActionResult<UserRole>> GetUserRole(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var userRole = await (from u_roles in db.UserRoles
                                    .Include(z => z.User)
                                         .ThenInclude(z1 => z1.Company)
                                    .Include(x => x.Role)
                                         .ThenInclude(x1 => x1.Company)
                                          where u_roles.Id == id
                                          select u_roles).FirstAsync();

                    if (userRole != null)
                        return userRole;
                }
            throw new ArgumentNullException();
        }

        /*
        /// <summary>
        /// Method to create a specific record for UserRoles-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="UserRole"/> object is null.</exception>
        /// <param name="company"><see cref="UserRole"/> object.</param>
        [HttpPost]
        [Route(Routes.UserRoleCreate)]
        public bool InsertUser([FromBody]UserRole userRole, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken) == 200)
                if (userRole != null)
                {
                    if (UserRolesCRUD.AddUserRole(userRole))
                        throw new SuccessCustomException();
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from UserRoles-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="UserRole"/> object is null.</exception>
        /// <param name="user"><see cref="UserRole"/> object.</param>
        [HttpPut]
        [Route(Routes.UserRoleUpdate)]
        public bool UpdateUser([FromBody]UserRole user, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken) == 200)
                if (user != null)
                {
                    if (UserRolesCRUD.UpdateUserRole(user))
                        throw new SuccessCustomException();
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from UserRoles-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="UserRole"/> object is null.</exception>
        /// <param name="id">UserRoles record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.UserRoleDelete)]
        public bool DeleteUser(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken) == 200)
                if (id != 0)
                {
                    if (UserRolesCRUD.DeleteUserRole(id))
                        throw new SuccessCustomException();
                }
            throw new ArgumentNullException();
        }*/
    }
}
