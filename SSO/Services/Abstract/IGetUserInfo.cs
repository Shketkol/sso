using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface IGetUserInfo
    {
        string GettingUserInfo(string inputToken);
        string GettingRequiredUserInfo(int id);
    }
}
