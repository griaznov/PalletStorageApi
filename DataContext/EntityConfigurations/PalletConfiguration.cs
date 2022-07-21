using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfigurations;

public class PalletConfiguration : IEntityTypeConfiguration<Pallet>
{
    public void Configure(EntityTypeBuilder<Pallet> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Length)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(p => p.Width)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(p => p.Height)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(p => p.PalletWeight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.HasMany(p => p.Boxes);
    }
}
