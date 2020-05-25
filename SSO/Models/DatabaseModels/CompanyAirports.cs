using System;
using SSO.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models.DatabaseModels
{
    /// <summary>
    /// Class using as a model of CompanyAirportsSet-Table.
    /// </summary>
    public class CompanyAirports : IDateSet
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int AirportId { get; set; }

        public virtual Company Company { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
