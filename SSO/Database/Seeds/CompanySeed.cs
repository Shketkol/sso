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
    public class CompanySeed
    {
        /// <summary>
        /// Method to set Seeds into Companies-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<Company> seedList = new List<Company>
                    {
                        new Company { CompanyName="PROFI-IT", Slug="" },
                        new Company { CompanyName="ODS", Slug="" }
                    };
            foreach (var item in seedList)
            {
                item.Slug = item.CompanyName.Replace(' ', '-').ToLower();
                db.Companies.Add(item);
            }
            db.SaveChanges();
        }
    }
}
