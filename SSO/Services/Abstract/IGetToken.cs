using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface IGetToken
    {
        string Token(string email, string password);
    }
}
