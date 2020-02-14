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
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(UserServiceConfiguration.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImportConfiguration());
            modelBuilder.ApplyConfiguration(new PreviousImportItemConfiguration());
        }
    }
}