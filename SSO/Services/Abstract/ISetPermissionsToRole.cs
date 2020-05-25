using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface ISetPermissionsToRole
    {
        bool SetPermissions(int roleId, int[] permissionsArray);
    }
}
