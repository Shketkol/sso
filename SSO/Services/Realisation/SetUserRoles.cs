using SSO.Database;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    public class SetUserRoles : ISetUserRoles
    {
        /// <summary>
        /// Method to set new roles to a specific user and remove old ones. Working with UserRoles Table.
        /// </summary>
        /// <param name="userId">User identificator as an integer.</param>
        /// <param name="rolesArray">An integer array of roles id.</param>
        /// <returns>Status code as integer.</returns>
        public bool AssignUserToRoles(int userId, int[] rolesArray)
        {
            var newRoles = new List<UserRole>();
            foreach (var role in rolesArray)
            {
                newRoles.Add(new UserRole {  UserId = userId, RoleId = role });
            }

            if (newRoles.Count < 1 || newRoles.First() == null)
                throw new ArgumentNullException();

            using (var db = new SSOContext())
            {
                var oldRoles = (from user_role in db.UserRoles
                                where user_role.UserId == userId
                                select user_role).ToList();

                if (oldRoles.Any())
                {
                    foreach (var oldRole in oldRoles)
                    {
                        db.UserRoles.Remove(oldRole);
                    }
                    db.SaveChanges();
                }
                foreach (var newRole in newRoles)
                {
                    db.UserRoles.Add(newRole);
                }

                db.SaveChanges();
            }
            return true;
        }
    }
}
