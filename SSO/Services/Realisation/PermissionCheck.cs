using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Database;
using SSO.Models;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Services.Realisation
{
    public class PermissionCheck : IPermissionCheck
    {
        public bool IsAllowed(string inputToken, string permission, string table)
        {
            var claims = new GetUserInfo().GetUserClaims(inputToken);

            //get permission and user objects
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

            foreach (var infoItem in getUserPermissions)
            {
                if (string.Equals(infoItem.Permission.Slug, "root") && string.Equals(infoItem.Permission.Table, table))
                    return true;
                if (string.Equals(infoItem.Permission.Slug, permission)&&string.Equals(infoItem.Permission.Table, table))
                    return true;
            }
            return false;
        }
    }
}
