﻿// <auto-generated />
using System;
using DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataContext.Migrations
{
    [DbContext(typeof(StorageContext))]
    partial class StorageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("DataContext.Entities.Box", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("DATETIME");

                    b.Property<double>("Height")
                        .HasColumnType("DOUBLE");

                    b.Property<double>("Length")
                        .HasColumnType("DOUBLE");

                    b.Property<int?>("PalletId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ProductionDate")
                        .HasColumnType("DATETIME");

                    b.Property<double>("Weight")
                        .HasColumnType("DOUBLE");

                    b.Property<double>("Width")
                        .HasColumnType("DOUBLE");

                    b.HasKey("Id");

                    b.HasIndex("PalletId");

                    b.HasIndex(new[] { "Id" }, "Id");

                    b.ToTable("Boxes");
                });

            modelBuilder.Entity("DataContext.Entities.Pallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Height")
                        .HasColumnType("DOUBLE");

                    b.Property<double>("Length")
                        .HasColumnType("DOUBLE");

                    b.Property<double>("PalletWeight")
                        .HasColumnType("DOUBLE");

                    b.Property<double>("Width")
                        .HasColumnType("DOUBLE");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "Id")
                        .HasDatabaseName("Id1");

                    b.ToTable("Pallets");
                });

            modelBuilder.Entity("DataContext.Entities.Box", b =>
                {
                    b.HasOne("DataContext.Entities.Pallet", "Pallet")
                        .WithMany("Boxes")
                        .HasForeignKey("PalletId");

                    b.Navigation("Pallet");
                });

            modelBuilder.Entity("DataContext.Entities.Pallet", b =>
                {
                    b.Navigation("Boxes");
                });
#pragma warning restore 612, 618
        }
    }
}
