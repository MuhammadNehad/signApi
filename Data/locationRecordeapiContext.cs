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
        }
        public DbSet<locationRecordeapi.roles> roles { get; set; }
        public DbSet<locationRecordeapi.Permissions> Permissions { get; set; }
        public DbSet<locationRecordeapi.roles_perms_rel> roles_perms_rel { get; set; }
    }
}
