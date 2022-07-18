﻿using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfigurations;

public class BoxConfiguration : IEntityTypeConfiguration<Box>
{
    public void Configure(EntityTypeBuilder<Box> builder)
    {
        builder.HasKey(box => box.Id);

        builder.Property(box => box.Length)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(box => box.Width)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(box => box.Height)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(box => box.Weight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        builder.Property(box => box.ProductionDate)
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property(box => box.ExpirationDate)
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property(box => box.PalletId)
            .HasColumnType("INTEGER");

        builder.HasOne(b => b.Pallet)
            .WithMany(p => p.Boxes)
            .HasForeignKey(b => b.PalletId);
    }
}
