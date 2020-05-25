using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Database.Functional;
using SSO.Models;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    /// <summary>
    /// Class to refresh token expiration date.
    /// </summary>
    public class TokenRefresh : ITokenRefresh
    {
        /// <summary>
        /// Method for refreshing token expiration date.
        /// </summary>
        /// <param name="inputToken">Token variable as a string.</param>
        /// <returns>Token as a string in JSON-format.</returns>
        public string TokenRefreshing(string inputToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(inputToken))
                {
                    inputToken = inputToken.Replace("Bearer ", "");
                    var tokenValidationParameters = ConfigurationExtensions.GetTokenValidation();
                    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(inputToken, tokenValidationParameters, out SecurityToken securityToken);

                    if (DBGetStuff.CheckUser(tokenValid))
                    {
                        var jwt = new JwtSecurityToken(
                        issuer: ConfigurationExtensions.GetConfig().AuthConfig.Issuer,
                        notBefore: DateTime.UtcNow,
                        claims: tokenValid.Claims,
                        expires: DateTime.UtcNow.AddMinutes(ConfigurationExtensions.GetConfig().AuthConfig.LifeTime),
                        signingCredentials: new SigningCredentials(ConfigurationExtensions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512));

                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                        var response = new TokenModel
                        {
                            AccessToken = encodedJwt,
                            TokenType = jwt.Header.Typ,
                            UserName = tokenValid.Identity.Name,
                            ExpirationDate = ((DateTimeOffset)jwt.ValidTo).ToUnixTimeSeconds()
                        };

                        return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    }
                }
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message);
            }
            throw new ArgumentNullException();
        }
    }
}
