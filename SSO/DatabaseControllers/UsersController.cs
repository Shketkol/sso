using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SSO.Database;
using System.Linq;
using SSO.Models.DatabaseModels;
using SSO.Models.Errors;
using SSO.Services.Abstract;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;

        public UsersController(ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }

        /// <summary>
        /// Method to get all Users-table records by company id.
        /// </summary>
        /// <returns>
        /// <see cref="List{User}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.UserByCompany)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByCompanyId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var userList = await (from users in db.Users
                                    .Include(z => z.Company)
                                          where users.CompanyId == id
                                          select users).ToListAsync();

                    if (userList.Any())
                        return userList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all records from Users-table.
        /// Calls method <see cref="GetAllUsers()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{User}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.UserList)]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                var userList = await (from users in db.Users
                                .Include(z => z.Company)
                                      select users).ToListAsync();

                if (userList.Any())
                    return userList;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from Users-table.
        /// </summary>
        /// <returns>
        /// <see cref="User"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">Users record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.UserItem)]
        public async Task<ActionResult<User>> GetUser(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var user = await (from users in db.Users
                                     .Include(z => z.Company)
                                      where users.Id == id
                                      select users).FirstAsync();

                    if (user != null)
                        return user;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to create a specific record for Users-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="User"/> object is null.</exception>
        /// <param name="user"><see cref="User"/> object.</param>
        [HttpPost]
        [Route(Routes.UserCreate)]
        public async Task<ActionResult<User>> InsertUser([FromBody]User user, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (user != null)
                    if (permissionCheck.IsAllowed(inputToken, "edit_user"))
                    {
                        using var db = new SSOContext();
                        var getUsers = await (from users in db.Users
                                              where users.Email == user.Email
                                              select users).IgnoreQueryFilters().ToListAsync();
                        if (getUsers.Any())
                        {
                            foreach (var item in getUsers)
                            {
                                if (item.DeletedAt == null)
                                    throw new ArgumentNullException("Existing User.");
                            }
                        }
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, ConfigurationExtensions.GetConfig().Salt.Value);
                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                        db.Entry(user).State = EntityState.Detached;
                        return await db.Users.Include(z => z.Company)
                            .Where(x => x.Id == user.Id).FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from Users-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="User"/> object is null.</exception>
        /// <param name="user"><see cref="User"/> object.</param>
        [HttpPut]
        [Route(Routes.UserUpdate)]
        public async Task<ActionResult<User>> UpdateUser([FromBody]User user,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == user.Id)
                    if (permissionCheck.IsAllowed(inputToken, "edit_user"))
                    {
                        using var db = new SSOContext();
                        var oldUser = await db.Users.FindAsync(id);
                        if (oldUser == null)
                            throw new ArgumentNullException();

                        user.CreatedAt = oldUser.CreatedAt;
                        db.Entry(oldUser).State = EntityState.Detached;

                        db.Users.Update(user);
                        await db.SaveChangesAsync();

                        db.Entry(user).State = EntityState.Detached;
                        return await db.Users.Include(z => z.Company)
                            .Where(x => x.Id == id).FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from Users-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="User"/> object is null.</exception>
        /// <param name="id">Users record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.UserDelete)]
        public async Task<ActionResult<User>> DeleteUser(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "edit_user"))
                    {
                        using var db = new SSOContext();
                        var getUser = await (from users in db.Users
                                       .Include(z => z.Company)
                                       where users.Id == id
                                       select users).FirstAsync();

                        if (getUser == null)
                            throw new ArgumentNullException();

                        getUser.DeletedAt = DateTimeOffset.UtcNow;
                        db.Users.Update(getUser);
                        await db.SaveChangesAsync();

                        db.Entry(getUser).State = EntityState.Detached;
                        return await db.Users
                            .IgnoreQueryFilters()
                            .Include(z => z.Company)
                            .Where(x => x.Id == id).FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
