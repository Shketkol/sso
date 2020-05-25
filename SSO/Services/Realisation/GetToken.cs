using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Database.Functional;
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
    /// Class for token getting.
    /// </summary>
    public class GetToken : IGetToken
    {
        /// <summary>
        /// Method to check id LogIn is valid. Creates JWT-token.
        /// Filling <see cref="TokenModel"/> and converting it in JSON.
        /// </summary>
        /// <param name="email">Email string variable.</param>
        /// <param name="password">Password string variable</param>
        /// <returns>Token as a string in JSON-format.</returns>
        public string Token(string email, string password)
        {
            var identity = DBGetStuff.GetUserObject(email, password);

            if (identity == null)
                throw new ArgumentNullException();

            var jwt = new JwtSecurityToken(
                    issuer: ConfigurationExtensions.GetConfig().AuthConfig.Issuer,
                    audience: ConfigurationExtensions.GetConfig().AuthConfig.Audience,
                    notBefore: DateTime.UtcNow,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.AddHours(ConfigurationExtensions.GetConfig().AuthConfig.LifeTime),
                    signingCredentials: new SigningCredentials(ConfigurationExtensions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenModel
            {
                AccessToken = encodedJwt,
                TokenType = jwt.Header.Typ,
                UserName = identity.Name,
                ExpirationDate = ((DateTimeOffset)jwt.ValidTo).ToUnixTimeSeconds()
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
