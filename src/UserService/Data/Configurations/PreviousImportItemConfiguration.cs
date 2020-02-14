using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Data.Configurations
{
    public class PreviousImportItemConfiguration : IEntityTypeConfiguration<PreviousImportItem>
    {
        public void Configure(EntityTypeBuilder<PreviousImportItem> builder)
        {
            builder
                .ToTable("PreviousImportItem")
                .HasIndex(x=>x.Email)
                .HasName("uq_email")
                .IsUnique();
            
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            
            builder
                .Property(x => x.ImportId)
                .HasColumnName("import_id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(128)")
                .IsRequired();
            
            builder
                .Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(256)")
                .IsRequired();
            
            builder
                .Property(x => x.BirthDate)
                .HasColumnName("birth_date")
                .HasColumnType("varchar(16)")
                .IsRequired();
            
            builder
                .Property(x => x.Gender)
                .HasColumnName("gender")
                .HasColumnType("varchar(16)")
                .IsRequired();
            
            builder
                .Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder
                .HasOne(x => x.Import)
                .WithMany(x => x.PreviousImportItems)
                .HasForeignKey(x => x.ImportId);
        }
    }
}