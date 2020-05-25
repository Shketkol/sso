using SSO.Database.Functional;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.DatabaseModels
{
    /// <summary>
    /// Class using as a model of Companies-Table.
    /// </summary>
    public class Company : IDateSet
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Slug { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }

}
