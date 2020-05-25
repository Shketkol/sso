using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSO.Services.Realisation;

namespace SSO.Models
{
    /// <summary>
    /// Class using as a model for <see cref="GetUserInfo"/>.
    /// </summary>
    public class UserInfoModel
    {
        public Сustomer User { get; set; }
    }
    public class MainPermissions
    {
        public string Table { get; set; }
        public List<Table> Content { get; set; }
    }
    public class Table
    {
        public int Id { get; set; }
        public string Slug { get; set; }
    }

    public class OutputParameters
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Сustomer
    {
        public Сustomer(User user, List<UserParams> userParams, List<Permission> permissions)
        {
            Params = GetOutputParams(userParams);

            Permissions = GetOutputPermissions(permissions);

            Id = user.Id;
            Name = user.Name;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Password = user.Password;
            PhoneNumber = user.PhoneNumber;
            CompanyId = user.CompanyId;
            RememberToken = user.RememberToken;
            Company = user.Company;
            EmailVerifiedAt = user.EmailVerifiedAt;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
            DeletedAt = user.DeletedAt;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public string RememberToken { get; set; }
        public Company Company { get; set; }
        public List<MainPermissions> Permissions { get; set; }
        public List<OutputParameters> Params { get; set; }
        public DateTimeOffset EmailVerifiedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        /// <summary>
        ///  Method of filtering old <see cref="OutputParameters"/> values.
        /// </summary>
        /// <param name="userParams"><see cref="List{UserParams}"/> object.</param>
        /// <returns><see cref="OutputParameters"/> array.</returns>
        private List<OutputParameters> GetOutputParams(List<UserParams> userParams)
        {
            var paramsList = new List<OutputParameters>();
            foreach (var item in userParams)
            {
                paramsList.Add(new OutputParameters { Key = item.Key, Value = item.Value });
            }
            return paramsList;
        }

        /// <summary>
        /// Method for filtering old <see cref="Permission"/> records by tables.
        /// </summary>
        /// <param name="permissions"><see cref="List{Permission}"/> object.</param>
        /// <returns><see cref="MainPermissions"/> array.</returns>
        private List<MainPermissions> GetOutputPermissions(List<Permission> permissions)
        {
            var sortedTables = new List<string>();
            foreach (var permission in permissions)
            {
                sortedTables.Add(permission.Table);
            }
            sortedTables.Sort();
            for (int i = 0; i < sortedTables.Count - 1; i++)
            {
                if (sortedTables[i] == sortedTables[i + 1])
                {
                    sortedTables.RemoveAt(i);
                    i = 0;
                }
            }

            var mainPermissions = new List<MainPermissions>();
            foreach (var sTable in sortedTables)
            {
                var tableContent= new List<Table>();
                foreach (var permission in permissions)
                {
                    if (sTable == permission.Table)
                    {
                        tableContent.Add(new Table { Id = permission.Id, Slug = permission.Slug });
                    }
                }
                mainPermissions.Add(new MainPermissions { Table = sTable, Content = tableContent });
            }
            return mainPermissions;
        }
    }
}
