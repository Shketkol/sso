using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Database.Seeds
{
    /// <summary>
    /// Class to set Seeds into DataBase Table.
    /// </summary>
    public class UserSeed
    {
        private static readonly int salt = ConfigurationExtensions.GetConfig().Salt.Value;

        /// <summary>
        /// Method to set Seeds into Users-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<User> seedList = new List<User>
                    {
                        new User { Name="greentee5",FirstName="Yan",LastName="Kolovorotny",
                            Email ="yankolovorotny@gmail.com",Password= BCrypt.Net.BCrypt.HashPassword("zev1982z", salt),
                            PhoneNumber="+380501689853", CompanyId= GetId.CompanyID("PROFI-IT",db) },
                        new User { Name="sipdrenma",FirstName="Piter",LastName="Parker",
                            Email ="spiderman@gmail.com",Password= BCrypt.Net.BCrypt.HashPassword("uncleBen", salt),
                            PhoneNumber="+3808008808", CompanyId= GetId.CompanyID("PROFI-IT",db) }
                    };
            foreach (var item in seedList)
            {
                db.Users.Add(item);
            }
            db.SaveChanges();
        }
    }
}
