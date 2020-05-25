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
    public class RolePermissionSeed
    {
        /// <summary>
        /// Method to set Seeds into RolePermissions-Table.
        /// </summary>
        /// <param name="db"><see cref="DBConfig"/> object.</param>
        public static void SetSeeds(SSOContext db)
        {
            List<RolePermission> seedList = new List<RolePermission>
                    {
                        new RolePermission { RoleId= GetId.RoleID("admin", db), PermissionId=GetId.PermissionID("Root",db) },
                        new RolePermission { RoleId= GetId.RoleID("admin", db), PermissionId=GetId.PermissionID("Edit plan",db) },
                        new RolePermission { RoleId= GetId.RoleID("dispatcher", db), PermissionId=GetId.PermissionID("Edit plan",db) },
                        new RolePermission { RoleId= GetId.RoleID("dispatcher", db), PermissionId=GetId.PermissionID("Edit arrival",db) },
                        new RolePermission { RoleId= GetId.RoleID("dispatcher", db), PermissionId=GetId.PermissionID("Edit departure",db) }
                    };
            foreach (var item in seedList)
            {
                db.RolePermissions.Add(item);
            }
            db.SaveChanges();
        }
    }
}
