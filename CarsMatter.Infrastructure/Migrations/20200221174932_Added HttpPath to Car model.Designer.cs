﻿// <auto-generated />
using System;
using CarsMatter.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarsMatter.Infrastructure.Migrations
{
    [DbContext(typeof(CarsMatterDbContext))]
    [Migration("20200221174932_Added HttpPath to Car model")]
    partial class AddedHttpPathtoCarmodel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.Brand", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandName");

                    b.Property<string>("HttpPath");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.BrandModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandId");

                    b.Property<string>("HttpPath");

                    b.Property<string>("ModelName");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("BrandModels");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.Car", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvitoUri");

                    b.Property<string>("Base64CarImage");

                    b.Property<string>("BodyType");

                    b.Property<string>("BrandModelId");

                    b.Property<string>("CarImagePath");

                    b.Property<string>("CarName");

                    b.Property<int>("HighPrice");

                    b.Property<string>("HttpPath");

                    b.Property<int>("LowPrice");

                    b.Property<string>("ManufactureEndDate");

                    b.Property<string>("ManufactureStartDate");

                    b.HasKey("Id");

                    b.HasIndex("BrandModelId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.ConsumablesNote", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("KindOfService");

                    b.Property<string>("Location");

                    b.Property<string>("Notes");

                    b.Property<int>("Odo");

                    b.Property<float>("Price");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ConsumablesNotes");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.FavoriteCar", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CarId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteCars");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.RefillNote", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Location");

                    b.Property<int>("Odo");

                    b.Property<float>("Petrol");

                    b.Property<float>("Price");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("RefillNotes");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.BrandModel", b =>
                {
                    b.HasOne("CarsMatter.Infrastructure.Models.MsSQL.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.Car", b =>
                {
                    b.HasOne("CarsMatter.Infrastructure.Models.MsSQL.BrandModel", "BrandModel")
                        .WithMany()
                        .HasForeignKey("BrandModelId");
                });

            modelBuilder.Entity("CarsMatter.Infrastructure.Models.MsSQL.FavoriteCar", b =>
                {
                    b.HasOne("CarsMatter.Infrastructure.Models.MsSQL.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId");

                    b.HasOne("CarsMatter.Infrastructure.Models.MsSQL.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
