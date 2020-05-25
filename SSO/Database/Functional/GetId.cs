using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Database
{
    /// <summary>
    /// Class to get record id from a certain DataBase Table.
    /// </summary>
    public class GetId
    {
        /// <summary>
        /// Method to get searched record id from Companies-Table.
        /// </summary>
        /// <param name="companyName">Company name string.</param>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        /// <returns>An integer.</returns>
        public static int CompanyID(string companyName, SSOContext db)
        {
            var getStuff = (from company in db.Companies
                          where company.Slug == companyName.Replace(' ', '-').ToLower()
                            select company).First();
            if (getStuff == null)
                throw new ArgumentNullException();
            return getStuff.Id;
        }

        /// <summary>
        /// Method to get searched record id from Users-Table.
        /// </summary>
        /// <param name="userName">User name string.</param>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        /// <returns>An integer.</returns>
        public static int UserID(string userName, SSOContext db)
        {
            var getStuff = (from users in db.Users
                          where users.Name == userName
                          select users).First();
            if (getStuff==null)
                throw new ArgumentNullException();
            return getStuff.Id;
        }

        /// <summary>
        /// Method to get searched record id from Roles-Table.
        /// </summary>
        /// <param name="roleName">Role name string.</param>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        /// <returns>An integer.</returns>
        public static int RoleID(string roleName, SSOContext db)
        {
            var getStuff = (from roles in db.Roles
                            where roles.Slug == roleName.Replace(' ', '-').ToLower()
                            select roles).First();
            if (getStuff == null)
                throw new ArgumentNullException();
            return getStuff.Id;
        }

        /// <summary>
        /// Method to get searched record id from Permissions-Table.
        /// </summary>
        /// <param name="permissionName">Permission name string.</param>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        /// <returns>An integer.</returns>
        public static int PermissionID(string permissionName, SSOContext db)
        {
            var getStuff = (from permissions in db.Permissions
                            where permissions.Slug == permissionName.Replace(' ', '-').ToLower()
                            select permissions).First();
            if (getStuff == null)
                throw new ArgumentNullException();
            return getStuff.Id;
        }
    }
}
