using SSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface IPermissionAccessCheck
    {
        string Check(string inputToken, List<InputPermissionModel> permissionList);
    }
}
