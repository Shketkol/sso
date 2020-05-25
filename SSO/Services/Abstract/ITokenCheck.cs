using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface ITokenCheck
    {
        bool TokenChecking(string inputToken);
    }
}
