using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RD.Domain.Identity;
using RD.Domain.Entities;

namespace RD.Core.EFContext
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<RDData> RDData { get; set; }
        public DbSet<LogData> LogData { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}