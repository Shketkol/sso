using SSO.Database;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    /// <summary>
    /// Class for permission setting.
    /// </summary>
    public class SetPermissionsToRole : ISetPermissionsToRole
    {
        /// <summary>
        /// Method to set new permissions to a specific role and remove old ones. Working with RolePermissions Table.
        /// </summary>
        /// <param name="roleId">Role identificator as an integer.</param>
        /// <param name="permissionsArray">An integer array of permissions id.</param>
        /// <returns>Status code as integer.</returns>
        public bool SetPermissions(int roleId, int[] permissionsArray)
        {
            var newPermissions = new List<RolePermission>();
            foreach (var permission in permissionsArray)
            {
                newPermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permission });
            }

            if (newPermissions.Count < 1)   
                throw new ArgumentNullException();

            using (var db = new SSOContext())
            {
                var oldPermissions = (from role_perm in db.RolePermissions.AsEnumerable()
                                      where role_perm.RoleId == roleId
                                      select role_perm).ToList();

                if (oldPermissions.Any())
                {
                    foreach (var oldPermission in oldPermissions)
                    {
                        db.RolePermissions.Remove(oldPermission);
                    }
                    db.SaveChanges();
                }
                foreach (var newPermission in newPermissions)
                {
                    db.RolePermissions.Add(newPermission);
                }
                db.SaveChanges();

            }
            return true;
        }
    }
}
