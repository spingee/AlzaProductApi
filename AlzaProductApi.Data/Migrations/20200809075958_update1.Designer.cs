﻿// <auto-generated />
using System;
using AlzaProductApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlzaProductApi.Data.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20200809075958_update1")]
    partial class update1
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

                    b.Property<byte[]>("_timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "SSD MVE",
                            ImgUri = "http:\\temp.uri",
                            Name = "Hardrive",
                            Price = 2000.89m
                        },
                        new
                        {
                            Id = 2,
                            Description = "SSD MVE",
                            ImgUri = "http:\\temp.uri",
                            Name = "Hardrive",
                            Price = 2000.89m
                        },
                        new
                        {
                            Id = 3,
                            Description = "SSD MVE",
                            ImgUri = "http:\\temp.uri",
                            Name = "Hardrive",
                            Price = 2000.89m
                        },
                        new
                        {
                            Id = 4,
                            Description = "SSD MVE",
                            ImgUri = "http:\\temp.uri",
                            Name = "Hardrive",
                            Price = 2000.89m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
