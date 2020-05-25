using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface ITokenRefresh
    {
        string TokenRefreshing(string inputToken);
    }
}
