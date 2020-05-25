using Microsoft.EntityFrameworkCore;
using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace SSO.Database.Functional
{
    public class DBGetStuff
    {
        /// <summary>
        /// Method to check if user record exists.
        /// </summary>
        /// <returns>
        /// <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when some variable is null.</exception>
        /// <param name="token">A <see cref="ClaimsPrincipal"/> object.</param>
        public static bool CheckUser(ClaimsPrincipal token)
        {
            using var db = new SSOContext();
            var identity = (ClaimsIdentity)token.Identity;
            var claims = identity.Claims.ToList();

            var getUsers = db.Users.Select(p => p)
                           .Where(p => p.Email == claims[0].Value
                                  && p.Name == claims[1].Value
                                  && p.Id.ToString() == claims[2].Value)
                           .IgnoreQueryFilters().First();
            if (getUsers==null)
            {
                throw new ArgumentNullException("user is not exist");
            }
            return true;
        }

        /// <summary>
        /// Method to get user object.
        /// </summary>
        /// <returns>
        /// <see cref="ClaimsIdentity"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when one parameter is null.</exception>
        /// <param name="email">A string.</param>
        /// <param name="password">A string.</param>
        public static ClaimsIdentity GetUserObject(string email, string password)
        {
            using var db = new SSOContext();

            var getStuff = (from users in db.Users
                            select users).ToList();

            if (getStuff.Any())
                foreach (var user in getStuff)
                {
                    if (email == user.Email && BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        var claims = new List<Claim>
                             {
                                  new Claim(ClaimTypes.Email, user.Email),
                                  new Claim(ClaimTypes.Name, user.Name),
                                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
                        return claimsIdentity;
                    }
                }

            throw new ArgumentNullException();
        }
    }
}

