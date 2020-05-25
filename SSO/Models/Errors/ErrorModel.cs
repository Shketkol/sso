using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models
{
    /// <summary>
    /// Class using as a model for custom errors.
    /// </summary>
    public class ErrorModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
