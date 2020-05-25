using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.DatabaseModels
{
    /// <summary>
    /// Class using as a model of Users-Table.
    /// </summary>
    public class User : IDateSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public string RememberToken { get; set; }
        public virtual Company Company { get; set; }
        public DateTimeOffset EmailVerifiedAt { get; set; }
        public DateTimeOffset  CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset?  DeletedAt { get; set; }
    }
}
