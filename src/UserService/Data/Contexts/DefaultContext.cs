using Microsoft.EntityFrameworkCore;
using UserService.Configurations;
using UserService.Data.Configurations;
using UserService.Domain.Entities;

namespace UserService.Data.Contexts
{
    public class DefaultContext : DbContext
    {
        public DbSet<Import> Import { get; set; }
        public DbSet<PreviousImportItem> PreviousImportItem { get; set; }
        public DbSet<User> User { get; set; }

        public DefaultContext()
        {
            Database.SetCommandTimeout(120);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(UserServiceConfiguration.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImportConfiguration());
            modelBuilder.ApplyConfiguration(new PreviousImportItemConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}