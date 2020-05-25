using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSO.Database.Seeds;

namespace SSO.Database
{
    /// <summary>
    /// Class to set Seeds
    /// </summary>
    class DBSeedsCreation
    {
        /// <summary>
        /// Method to set seeds into database.
        /// </summary>
        public static void SetSeeds()
        {
            using var db = new SSOContext();
            if (!db.Users.Any())
            {
                CompanySeed.SetSeeds(db);
                PermissionSeed.SetSeeds(db);
                UserSeed.SetSeeds(db);
                RoleSeed.SetSeeds(db);
                UserParamsSeed.SetSeeds(db);
                RolePermissionSeed.SetSeeds(db);
                UserRoleSeed.SetSeeds(db);
                CompanyAirportsSeed.SetSeeds(db);
            }
        }
    }
}
