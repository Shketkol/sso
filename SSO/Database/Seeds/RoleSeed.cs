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
    public class RoleSeed
    {
        /// <summary>
        /// Method to set Seeds into Roles-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<Role> seedList = new List<Role>
                    {
                        new Role { Slug="admin",CompanyId=GetId.CompanyID("PROFI-IT",db) },
                        new Role { Slug="dispatcher",CompanyId= GetId.CompanyID("ODS",db) }
                    };
            foreach (var item in seedList)
            {
                item.Slug = item.Slug.Replace(' ', '-').ToLower();
                db.Roles.Add(item);
            }
            db.SaveChanges();
        }
    }
}
