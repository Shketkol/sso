using Microsoft.EntityFrameworkCore;
using SSO.Models.DatabaseModels;
using SSO.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Database
{
    /// <summary>
    /// DbContext class.
    /// Holds a database configuration information, as declared in the engine configuration file. Can be used to get access to the database object. 
    /// </summary>
    public partial class SSOContext : DbContext
    {
        public SSOContext()
        {
            Database.EnsureCreated();
        }

        public SSOContext(DbContextOptions<SSOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<UserParams> UserParameters { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<CompanyAirports> CompanyAirportsSet { get; set; }
        public virtual DbSet<Invite> Invites { get; set; }
        public virtual DbSet<InviteRole> InviteRoles { get; set; }


        /// <summary>
        /// method to configure the database (and other options) to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var getSetting = ConfigurationExtensions.GetConfig();
                optionsBuilder.UseNpgsql(getSetting.DbConfig.DbValue);
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.DbModelCreate();
            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DateSetting();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            DateSetting();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Method to update TimeStamps for modified Table records.
        /// </summary>
        private void DateSetting()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IDateSet datesetting)
                {
                    var now = DateTimeOffset.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            datesetting.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            datesetting.CreatedAt = now;
                            datesetting.UpdatedAt = now;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Method to update TimeStamps for modified Table records.
        /// </summary>
        public DbSet<SSO.Models.DatabaseModels.CompanyAirports> CompanyAirports { get; set; }
    }
}
