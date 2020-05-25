using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.Errors
{
    [Serializable]
    class SuccessCustomException : Exception
    {
        public SuccessCustomException()
            : base(String.Format("OK"))
        {
        }

        public SuccessCustomException(string message)
            : base(String.Format(message))
        {
        }
    }
}
