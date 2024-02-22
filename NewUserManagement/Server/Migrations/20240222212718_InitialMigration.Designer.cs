﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewUserManagement.Server.Data;

#nullable disable

namespace NewUserManagement.Server.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240222212718_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("NewUserManagement.Shared.Models.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Action")
                        .HasColumnType("TEXT");

                    b.Property<string>("Details")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LogEntries", (string)null);
                });

            modelBuilder.Entity("NewUserManagement.Shared.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ploew@example.com",
                            Forename = "Peter",
                            IsActive = true,
                            Surname = "Loew"
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "bfgates@example.com",
                            Forename = "Benjamin Franklin",
                            IsActive = true,
                            Surname = "Gates"
                        },
                        new
                        {
                            Id = 3,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ctroy@example.com",
                            Forename = "Castor",
                            IsActive = false,
                            Surname = "Troy"
                        },
                        new
                        {
                            Id = 4,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mraines@example.com",
                            Forename = "Memphis",
                            IsActive = true,
                            Surname = "Raines"
                        },
                        new
                        {
                            Id = 5,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sgodspeed@example.com",
                            Forename = "Stanley",
                            IsActive = true,
                            Surname = "Goodspeed"
                        },
                        new
                        {
                            Id = 6,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "himcdunnough@example.com",
                            Forename = "H.I.",
                            IsActive = true,
                            Surname = "McDunnough"
                        },
                        new
                        {
                            Id = 7,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "cpoe@example.com",
                            Forename = "Cameron",
                            IsActive = false,
                            Surname = "Poe"
                        },
                        new
                        {
                            Id = 8,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "emalus@example.com",
                            Forename = "Edward",
                            IsActive = false,
                            Surname = "Malus"
                        },
                        new
                        {
                            Id = 9,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "dmacready@example.com",
                            Forename = "Damon",
                            IsActive = false,
                            Surname = "Macready"
                        },
                        new
                        {
                            Id = 10,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jblaze@example.com",
                            Forename = "Johnny",
                            IsActive = true,
                            Surname = "Blaze"
                        },
                        new
                        {
                            Id = 11,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "rfeld@example.com",
                            Forename = "Robin",
                            IsActive = true,
                            Surname = "Feld"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}