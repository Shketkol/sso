using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Database.Seeds
{
    /// <summary>
    /// Class to set Seeds into DataBase Table.
    /// </summary>
    public class CompanyAirportsSeed
    {
        /// <summary>
        /// Method to set Seeds into CompanyAirportsSet-Table.
        /// </summary>
        /// <param name="db">DB context.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<CompanyAirports> seedList = new List<CompanyAirports>
                    {
                        new CompanyAirports {CompanyId=GetId.CompanyID("PROFI-IT",db), AirportId=1 },
                        new CompanyAirports {CompanyId= GetId.CompanyID("ODS",db), AirportId=1 }
                    };
            db.AddRange(seedList);
            db.SaveChanges();
        }
    }
}
