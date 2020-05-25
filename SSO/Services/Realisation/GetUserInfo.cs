using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Database;
using SSO.Models;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    /// <summary>
    /// Class to get info about specific user.
    /// </summary>
    public class GetUserInfo : IGetUserInfo
    {
        /// <summary>
        /// Method to get info about current user.
        /// </summary>
        /// <param name="inputToken">Token variable string type.</param>
        /// <returns>JSON converted <see cref="UserInfoModel"/> as a string.</returns>
        public string GettingUserInfo(string inputToken)
        {
            try
            {
                var claims = GetUserClaims(inputToken);
                var outputModel = CreateOutputModel(Convert.ToInt32(claims[2].Value));
                return outputModel;
            }
            catch (Exception)
            {
                throw new ArgumentNullException("Token is invalid.");
            }
        }

        /// <summary>
        /// Method to get info about user you are searching for.
        /// </summary>
        /// <param name="id">User identificator integer.</param>
        /// <returns>JSON converted <see cref="UserInfoModel"/> as a string.</returns>
        public string GettingRequiredUserInfo(int id)
        {
            try
            {
                var outputModel = CreateOutputModel(id);
                return outputModel;
            }
            catch (Exception)
            {
                throw new ArgumentNullException("Token is invalid.");
            }
        }

        /// <summary>
        /// Method to get <see cref="Claim"/> array with user info.
        /// </summary>
        /// <param name="inputToken">Token as a string.</param>
        /// <returns><see cref="List{Claim}"/> object.</returns>
        public List<Claim> GetUserClaims(string inputToken)
        {
            try
            {
                inputToken = inputToken.Replace("Bearer ", "");
                var tokenValidationParameters = ConfigurationExtensions.GetTokenValidation();
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal tokenClaims = jwtSecurityTokenHandler.ValidateToken(inputToken, tokenValidationParameters, out SecurityToken securityToken);

                var identity = (ClaimsIdentity)tokenClaims.Identity;
                var claims = identity.Claims.ToList();
                return claims;
            }
            catch (Exception)
            {
                throw new ArgumentNullException("Token is invalid.");
            }
        }

        /// <summary>
        /// Method to create <see cref="UserInfoModel"/> for an output.
        /// </summary>
        /// <param name="id">User identificator integer.</param>
        /// <returns>JSON-formated <see cref="UserInfoModel"/> as a string type.</returns>
        public string CreateOutputModel(int id)
        {
            using var db = new SSOContext();
            var funcParams = (from uparams in db.UserParameters
                              .Include(x => x.User)
                                .ThenInclude(x1 => x1.Company)
                              where uparams.UserId == id
                              select uparams).ToList();

            var funcPermissions = (from roleperms in db.RolePermissions
                                  .Include(x => x.Permission)
                                  .Join(db.UserRoles,
                                  p => p.RoleId,
                                  c => c.RoleId,
                                  (p, c) => new { c.UserId, p.Permission })
                                   where roleperms.UserId == id
                                   select roleperms).ToList();

            var userPermissions = new List<Permission>();
            if (funcPermissions.Any())
            {
                foreach (var item in funcPermissions)
                {
                    userPermissions.Add(item.Permission);
                }
            }
            else { userPermissions=null; }

            var user = new User();

            var userParams = new List<UserParams>();
            if (funcParams.Any())
            {
                foreach (var item in funcParams)
                {
                    userParams.Add(item);
                }
                user = userParams.First().User;
            }
            else { user = null; }

            var userInfo = new UserInfoModel
            {
                User = new Сustomer(user, userParams, userPermissions)
            };
            return JsonConvert.SerializeObject(userInfo, new JsonSerializerSettings { Formatting = Formatting.Indented });
            throw new ArgumentNullException("Token is invalid.");
        }
    }
}
