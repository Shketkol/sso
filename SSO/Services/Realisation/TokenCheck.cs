using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Database.Functional;
using SSO.Models;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    /// <summary>
    /// Class for token validation.
    /// </summary>
    public class TokenCheck : ITokenCheck
    {
        /// <summary>
        /// Method to check if Token is valid.
        /// </summary>
        /// <param name="inputToken">Input token in JSON format as a string.</param>
        /// <returns>Status code as integer.</returns>
        public bool TokenChecking(string inputToken)
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
                        return true;
                    throw new ArgumentNullException();
                }
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message);
            }
            throw new AuthenticationException();
        }

    }
}
