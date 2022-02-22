﻿// <auto-generated />
using Blazor.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blazor.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220222192551_seedProducts")]
    partial class seedProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Blazor.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 7,
                            Description = "adidas LITE RACER 2.0 I su dječije patike za dječake. U njima će vaši mališani da nauče da balansiraju između snage i odgovornosti.",
                            ImageUrl = "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/EH1/EH1425/images/thumbs_800/thumbs_w/EH1425_800_800px_w.jpg",
                            Name = "adidas LITE RACER 2.0",
                            Price = 47.40m
                        },
                        new
                        {
                            Id = 8,
                            Description = "Jaketa",
                            ImageUrl = "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/KRA/KRA203M504-01/images/thumbs_800/thumbs_w/KRA203M504-01_800_800px_w.jpg",
                            Name = "KRONOS Jakna Bee Jacket",
                            Price = 69.50m
                        },
                        new
                        {
                            Id = 9,
                            Description = "dukserica",
                            ImageUrl = "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/KRA/KRA221F602-80/images/thumbs_800/thumbs_w/KRA221F602-80_800_800px_w.jpg",
                            Name = "KRONOS LADIES CREWNECK",
                            Price = 52.20m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
