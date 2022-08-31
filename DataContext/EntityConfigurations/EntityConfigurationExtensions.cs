using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataContext.Entities.AbstractEntities;

namespace DataContext.EntityConfigurations;

internal static class EntityConfigurationExtensions
{
    public static void ConfigureUniversalBoxEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUniversalBox
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
    }
}
