using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSO.Models;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;

namespace SSO.Controllers
{
    [ApiController]
    public class FunctionalController : ControllerBase
    {
        private readonly ITokenCheck tokenCheck;
        private readonly IGetUserInfo getUserInfo;
        private readonly IPermissionAccessCheck permissionAccessCheck;
        public FunctionalController(ITokenCheck tokenCheck, IGetUserInfo getUserInfo, IPermissionAccessCheck permissionAccessCheck)
        {
            this.tokenCheck = tokenCheck;
            this.getUserInfo = getUserInfo;
            this.permissionAccessCheck = permissionAccessCheck;
        }

        /// <summary>
        /// Method to get a list of checked permissions for a concrete user
        /// in json format.
        /// </summary>
        /// <param name="permissionList">List of permissions to check.</param>
        /// <param name="inputToken">Token to validate it and get info about user from it.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(Routes.GetAllowedPermissions)]
        public string GetAllowedPermissions([FromBody] List<InputPermissionModel> permissionList, [FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                var outputJSON = permissionAccessCheck.Check(inputToken, permissionList);
                return (outputJSON);
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get information about yourself.
        /// <para>This method can validate token using <see cref="ITokenCheck.TokenChecking(string)"/>.
        /// Calls method <see cref="IGetUserInfo.GettingUserInfo(string)"/>.</para>
        /// </summary>
        /// <returns>
        /// <see cref="UserParams"/> model in JSON-format.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when one parameter is null.</exception>
        /// <param name="inputToken">A json-token in string format.</param>
        [HttpPost]
        [Route(Routes.GetMyUserInfo)]
        public string GetMyUserInfo([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
            {
                var output = getUserInfo.GettingUserInfo(inputToken);
                return output;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to get information about user by user_params id.
        /// <para>This method can validate token using <see cref="ITokenCheck.TokenChecking(string)"/>.
        /// Calls method <see cref="IGetUserInfo.GettingRequiredUserInfo(int)"/>.</para>
        /// </summary>
        /// <returns>
        /// <see cref="UserParams"/> model in JSON-format.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when one parameter is null.</exception>
        /// <param name="id"><see cref="UserParams"/> id int type.</param>
        [HttpPost]
        [Route(Routes.GetRequiredUserInfo)]
        public string GetRequiredUserInfo(int id, [FromHeader(Name = "Authorization")] string inputToken)
        {
            //var inputToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (tokenCheck.TokenChecking(inputToken))
            {
                var output = getUserInfo.GettingRequiredUserInfo(id);
                return output;
            }
            throw new ArgumentNullException();
        }
    }
}
