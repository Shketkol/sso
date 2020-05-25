using Microsoft.EntityFrameworkCore;
using SSO.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Database
{
    public static class ModelBuilderExtensions
    {
        public static void DbModelCreate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<CompanyAirports>(entity =>
            {
                entity.HasIndex(e => e.CompanyId);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Invite>(entity =>
            {
                entity.HasIndex(e => e.CompanyId);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<InviteRole>(entity =>
            {
                entity.HasIndex(e => e.InviteId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasIndex(e => e.PermissionId);

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.CompanyId);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<UserParams>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.CompanyId);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EmailVerifiedAt).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });
        }
    }
}
