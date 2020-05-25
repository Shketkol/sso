using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.DatabaseModels
{
    public class InviteRole : IDateSet
    {
        public int Id { get; set; }
        public int InviteId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Invite Invite { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
