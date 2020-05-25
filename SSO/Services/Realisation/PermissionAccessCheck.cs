using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSO.Database;
using SSO.Models;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    public class PermissionAccessCheck : IPermissionAccessCheck
    {
        public string Check(string inputToken, List<InputPermissionModel> inputPermissions)
        {
            if (!inputPermissions.Any()) throw new ArgumentNullException();

            var claims = new GetUserInfo().GetUserClaims(inputToken);

            using var db = new SSOContext();
            var getUserPermissions = (from rolePermissions in db.RolePermissions
                            .Include(x1 => x1.Permission)
                            .Join(db.UserRoles,
                                       p => p.RoleId,
                                       c => c.RoleId,
                                       (p, c) => new { p.Permission, c.User })
                            .Where(z => z.User.Email == claims[0].Value
                                      && z.User.Name == claims[1].Value
                                      && z.User.Id.ToString() == claims[2].Value)
                                      select rolePermissions).ToList();

            if (!getUserPermissions.Any()) throw new ArgumentNullException();

            var outputPermissions = new List<OutputPermissionModel>();
            foreach (var permission in inputPermissions)
            {
                outputPermissions.Add(new OutputPermissionModel
                {
                    Slug = permission.Slug,
                    Table = permission.Table,
                    HasPermission = false
                });
            }

            foreach (var outputPermission in outputPermissions)
            {
                foreach (var truePermission in getUserPermissions)
                {
                    if (truePermission.Permission.Table == outputPermission.Table
                        && truePermission.Permission.Slug == outputPermission.Slug)
                    {
                        outputPermission.HasPermission = true;
                    }
                }
            }
            return JsonConvert.SerializeObject(outputPermissions, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
