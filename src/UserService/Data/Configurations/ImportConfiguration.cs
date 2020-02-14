using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Data.Configurations
{
    public class ImportConfiguration : IEntityTypeConfiguration<Import>
    {
        public void Configure(EntityTypeBuilder<Import> builder)
        {
            builder
                .ToTable("Import");
            
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            
            builder
                .Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(x => x.Approved)
                .HasColumnName("approved")
                .HasColumnType("bit")
                .IsRequired();

            builder
                .Property(x => x.ImportDate)
                .HasColumnName("import_date")
                .HasColumnType("datetime");

            builder
                .Property(x => x.AmountRows)
                .HasColumnName("amount_rows")
                .HasColumnType("int")
                .IsRequired();
            
            builder
                .HasMany(x => x.PreviousImportItems)
                .WithOne(x => x.Import)
                .HasForeignKey(x => x.ImportId);
        }
    }
}