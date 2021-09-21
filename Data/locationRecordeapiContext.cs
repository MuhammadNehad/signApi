using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi;

namespace locationRecordeapi.Data
{
    public class locationRecordeapiContext : DbContext
    {
        public locationRecordeapiContext (DbContextOptions<locationRecordeapiContext> options)
            : base(options)
        {
        }

        public DbSet<locationRecordeapi.Emplyees> Emplyees { get; set; }

        public DbSet<locationRecordeapi.EmpsLocation> EmpsLocation { get; set; }

        public DbSet<locationRecordeapi.attendings> attendings { get; set; }
        public DbSet<locationRecordeapi.Emps_Locs_View> emps_Locs_View { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emps_Locs_View>().HasNoKey().ToView("Emps_Locs_View");

            modelBuilder.Entity<roles_perms_rel>().HasKey(rpr => new { rpr.id });
            modelBuilder.Entity<roles_perms_rel>().HasOne(rpr => rpr.perm);
            modelBuilder.Entity<roles_perms_rel>().HasOne(rpr => rpr.role);

            modelBuilder.Entity<roles_perms_rel>().HasOne(rpr => rpr.role).WithMany(r => r._roles_perms_rel).HasForeignKey(rpr => rpr.role_id).HasConstraintName("FK_role_role_perms");
            modelBuilder.Entity<roles_perms_rel>().HasOne(rpr => rpr.perm).WithMany(p => p._roles_perms_rel).HasForeignKey(rpr => rpr.perm_id);

            modelBuilder.Entity<Emplyees>().HasKey(e => new {e.id});

            modelBuilder.Entity<Emplyees>().HasOne(e => e._role).WithOne(r=>r.emplyees).HasForeignKey<Emplyees>(e=>e.role);
            //modelBuilder.Entity<roles_perms_rel>().HasKey(rpr => new { rpr.perm_id, rpr.role_id });

        }
        public DbSet<locationRecordeapi.roles> roles { get; set; }
        public DbSet<locationRecordeapi.Permissions> Permissions { get; set; }
        public DbSet<locationRecordeapi.roles_perms_rel> roles_perms_rel { get; set; }
    }
}
