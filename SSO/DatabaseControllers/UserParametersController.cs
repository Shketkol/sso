using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Linq;
using SSO.Database;
using SSO.Models.DatabaseModels;
using SSO.Models.Errors;
using SSO.Services;
using SSO.Services.Abstract;

namespace SSO.DatabaseControllers
{
    [ApiController]
    public class UserParametersController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly IPermissionCheck permissionCheck;
        public UserParametersController(ITokenCheck tokenCheck, IPermissionCheck permissionCheck)
        {
            this.tokenCheck = tokenCheck;
            this.permissionCheck = permissionCheck;
        }
        /// <summary>
        /// Method to get all UserParameters-table records by user id.
        /// </summary>
        /// <returns>
        /// <see cref="List{UserParams}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">An integer variable.</param>
        [HttpGet]
        [Route(Routes.UserParamsByUser)]
        public async Task<ActionResult<IEnumerable<UserParams>>> GetUserParamsByUserId(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var userRolesList = await (from u_params in db.UserParameters
                                         .Include(z => z.User)
                                            .ThenInclude(x => x.Company)
                                               where u_params.UserId == id
                                               select u_params).ToListAsync();

                    if (userRolesList.Any())
                        return userRolesList;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get all records from UserParameters-table.
        /// Calls method <see cref="GetAllUserParams()"/>.
        /// </summary>
        /// <returns>
        /// <see cref="List{UserParams}"/>.
        /// </returns>
        [HttpGet]
        [Route(Routes.UserParamsList)]
        public async Task<ActionResult<IEnumerable<UserParams>>> GetAllUserParams([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                using var db = new SSOContext();
                var usersParamsList = await (from u_params in db.UserParameters
                                       .Include(z => z.User)
                                        .ThenInclude(x => x.Company)
                                             select u_params).ToListAsync();

                if (usersParamsList.Any())
                    return usersParamsList;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get a specific record from UserParameters-table.
        /// </summary>
        /// <returns>
        /// <see cref="UserParams"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when id equals 0.</exception>
        /// <param name="id">UserParams record identificator. Integer.</param>
        [HttpGet]
        [Route(Routes.UserParamsItem)]
        public async Task<ActionResult<UserParams>> GetUserParams(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                {
                    using var db = new SSOContext();
                    var userParams = await (from u_params in db.UserParameters
                                      .Include(z => z.User)
                                            .ThenInclude(x => x.Company)
                                            where u_params.Id == id
                                            select u_params).FirstAsync();

                    if (userParams != null)
                        return userParams;
                }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to create a specific record for UserParameters-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="UserParams"/> object is null.</exception>
        /// <param name="company"><see cref="UserParams"/> object.</param>
        [HttpPost]
        [Route(Routes.UserParamsCreate)]
        public async Task<ActionResult<UserParams>> InsertUserParams([FromBody]UserParams userParams, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (userParams != null)
                    if (permissionCheck.IsAllowed(inputToken, "edit_user_params"))
                    {
                        using var db = new SSOContext();
                        db.UserParameters.Add(userParams);
                        await db.SaveChangesAsync();
                        db.Entry(userParams).State = EntityState.Detached;
                        return await db.UserParameters
                            .Include(z => z.User)
                                .ThenInclude(x => x.Company)
                            .Where(x => x.Id == userParams.Id)
                            .FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to update a specific record from UserParameters-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="UserParams"/> object is null.</exception>
        /// <param name="company"><see cref="UserParams"/> object.</param>
        [HttpPut]
        [Route(Routes.UserParamsUpdate)]
        public async Task<ActionResult<UserParams>> UpdateUserParams([FromBody]UserParams userParams,
            [FromHeader(Name = "Authorization")] string inputToken,
            [FromRoute] int id)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id == userParams.Id)
                    if (permissionCheck.IsAllowed(inputToken, "edit_user_params"))
                    {
                        using var db = new SSOContext();
                        var oldParams = await db.UserParameters.FindAsync(id);

                        if (oldParams == null)
                            throw new ArgumentNullException();

                        userParams.CreatedAt = oldParams.CreatedAt;
                        db.Entry(oldParams).State = EntityState.Detached;

                        db.UserParameters.Update(userParams);
                        await db.SaveChangesAsync();

                        db.Entry(userParams).State = EntityState.Detached;
                        return await db.UserParameters
                           .Include(z => z.User)
                               .ThenInclude(x => x.Company)
                           .Where(x => x.Id == id)
                           .FirstAsync();
                    }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to delete a specific record from UserParameters-table.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="UserParams"/> object is null.</exception>
        /// <param name="id">UserParams record identificator. Integer.</param>
        [HttpDelete]
        [Route(Routes.UserParamsDelete)]
        public async Task<ActionResult<UserParams>> DeleteUserParams(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                if (id != 0)
                    if (permissionCheck.IsAllowed(inputToken, "edit_user_params"))
                    {
                        using var db = new SSOContext();

                        var getUserParams = await db.UserParameters.FindAsync(id);

                        if (getUserParams == null)
                            throw new ArgumentNullException();

                        getUserParams.DeletedAt = DateTimeOffset.UtcNow;
                        db.UserParameters.Update(getUserParams);
                        await db.SaveChangesAsync();

                        db.Entry(getUserParams).State = EntityState.Detached;
                        return await (from u_params in db.UserParameters
                                             .IgnoreQueryFilters()
                                             .Include(z => z.User)
                                                .ThenInclude(x => x.Company)
                                      where u_params.Id == id
                                      select u_params).FirstAsync();
                    }
            throw new ArgumentNullException();
        }
    }
}
