using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models
{
    /// <summary>
    /// Token model for JSON formatting.
    /// </summary>
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string UserName { get; set; }
        public long ExpirationDate { get; set; }
    }
}
