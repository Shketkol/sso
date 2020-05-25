using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Models;
using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SSO
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Method for getting settings from configuration file.
        /// </summary>
        /// <returns><see cref="ConfigModel"/> object full of settings.</returns>
        public static ConfigModel GetConfig()
        {
            using var r = new StreamReader("Properties/appsettings.json");
            var json = r.ReadToEnd();
            var appSettings = JsonConvert.DeserializeObject<ConfigModel>(json);
            return appSettings;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GetConfig().AuthConfig.Key));
        }
        public static TokenValidationParameters GetTokenValidation()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}
