using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface IPermissionCheck
    {
        bool IsAllowed(string inputToken, string permission, string tablename = "common");
    }
}
