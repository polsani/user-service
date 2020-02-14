using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("User")
                .HasIndex(x=>x.Email)
                .HasName("uq_email")
                .IsUnique();
            
            builder
                .HasKey(x => x.Id);
            
            builder
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder
                .Property(x => x.BirthDate)
                .HasColumnName("birth_date")
                .HasColumnType("datetime")
                .IsRequired();
            
            builder
                .Property(x => x.Gender)
                .HasColumnName("gender")
                .HasColumnType("int")
                .IsRequired();
            
            builder
                .Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(256)")
                .IsRequired();
        }
    }
}