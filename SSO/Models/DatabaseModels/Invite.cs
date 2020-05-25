using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.DatabaseModels
{
    /// <summary>
    /// Class using as a model of Invites-Table.
    /// </summary>
    public class Invite : IDateSet
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string Token { get; set; } //some unique hash(like bcrypt)

        public virtual Company Company { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}