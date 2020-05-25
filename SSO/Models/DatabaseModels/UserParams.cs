using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.DatabaseModels
{
    /// <summary>
    /// Class using as a model of UserParameters-Table.
    /// </summary>
    public class UserParams : IDateSet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
