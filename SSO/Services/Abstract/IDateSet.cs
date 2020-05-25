using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Abstract
{
    public interface IDateSet
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        DateTimeOffset? DeletedAt{ get; set; }
    }
}
