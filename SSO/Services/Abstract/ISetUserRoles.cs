using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface ISetUserRoles
    {
        bool AssignUserToRoles(int roleId, int[] permissionsArray);
    }
}
