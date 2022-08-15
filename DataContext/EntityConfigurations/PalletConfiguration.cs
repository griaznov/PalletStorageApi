using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfigurations;

public class PalletConfiguration : IEntityTypeConfiguration<Pallet>
{
    public void Configure(EntityTypeBuilder<Pallet> builder)
    {
        builder.ConfigureUniversalBoxEntity();

        builder.Property(p => p.PalletWeight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.HasMany(p => p.Boxes);
    }
}
