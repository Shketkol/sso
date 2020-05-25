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
    public class UserRoleSeed
    {
        /// <summary>
        /// Method to set Seeds into UserRoles-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<UserRole> seedList = new List<UserRole>
                    {
                        new UserRole { RoleId=GetId.RoleID("admin",db),UserId=GetId.UserID("greentee5",db) },
                        new UserRole { RoleId=GetId.RoleID("admin",db),UserId=GetId.UserID("sipdrenma",db) }
                    };
            foreach (var item in seedList)
            {
                db.UserRoles.Add(item);
            }
            db.SaveChanges();
        }
    }
}
