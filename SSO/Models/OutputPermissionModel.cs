using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models
{
    public class OutputPermissionModel
    {
        public string Slug { get; set; }
        public string Table { get; set; }
        public bool HasPermission { get; set; }
    }
}
