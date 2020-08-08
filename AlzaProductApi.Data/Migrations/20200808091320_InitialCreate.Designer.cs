﻿// <auto-generated />
using AlzaProductApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlzaProductApi.Data.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20200808091320_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlzaProductApi.Business.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "SportlineDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "Sportline",
                            Price = 299000.9m
                        },
                        new
                        {
                            Id = 2,
                            Description = "RsDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "RS",
                            Price = 599999m
                        },
                        new
                        {
                            Id = 3,
                            Description = "ActiveDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "Active",
                            Price = 180000m
                        },
                        new
                        {
                            Id = 4,
                            Description = "AmbitionDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "Ambition",
                            Price = 423123m
                        },
                        new
                        {
                            Id = 5,
                            Description = "ActiveFabiaDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "ActiveFabia",
                            Price = 123000m
                        },
                        new
                        {
                            Id = 6,
                            Description = "AmbitionFabiaDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "AmbitionFabia",
                            Price = 333333m
                        },
                        new
                        {
                            Id = 7,
                            Description = "AmbitionOctaviaDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "AmbitionOctavia",
                            Price = 350000m
                        },
                        new
                        {
                            Id = 8,
                            Description = "StyleOctaviaDescription",
                            ImgUri = "http:\\temp.uri",
                            Name = "StyleOctavia",
                            Price = 699000m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
