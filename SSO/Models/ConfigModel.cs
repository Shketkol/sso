using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models
{
    /// <summary>
    /// Class using as a model for getting stuff from config file.
    /// </summary>
    public class ConfigModel
    {
        public AuthConfig AuthConfig { get; set; }
        public DbConfigs DbConfig { get; set; }
        public Salt Salt { get; set; }
        public SNS SNS { get; set; }
        public string SignUpURL { get; set; }
    }
    public class SNS
    {
        public string Url { get; set; }
    }
    public class AuthConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public double LifeTime { get; set; }
    }
    public class DbConfigs
    { 
        public string DbValue { get; set; }
    }
    public class Salt
    {
        public int Value { get; set; }
    }
}
