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
    public class UserParamsSeed
    {
        /// <summary>
        /// Method to set Seeds into UserParameters-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<UserParams> seedList = new List<UserParams>
                    {
                        new UserParams { UserId=GetId.UserID("greentee5",db), Key="phone",Value="+380501689853" },
                        new UserParams { UserId=GetId.UserID("greentee5",db), Key="email",Value="yankolovorotny@gmail.com" },
                        new UserParams { UserId=GetId.UserID("sipdrenma",db), Key="email",Value="spiderman@gmail.com" }
                    };
            foreach (var item in seedList)
            {
                db.UserParameters.Add(item);
            }
            db.SaveChanges();
        }
    }
}
