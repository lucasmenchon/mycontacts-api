﻿// <auto-generated />
using System;
using ContactsManage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyContactsAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyContactsAPI.Models.ContactModels.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CellPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("MyContactsAPI.Models.UserModels.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("varchar")
                        .HasColumnName("Name");

                    b.Property<DateTimeOffset>("RegisterDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("RegisterDate");

                    b.Property<DateTimeOffset?>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdateDate");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("varchar")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("MyContactsAPI.Models.ContactModels.Contact", b =>
                {
                    b.HasOne("MyContactsAPI.Models.UserModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyContactsAPI.Models.UserModels.User", b =>
                {
                    b.OwnsOne("MyContactsAPI.Models.EmailModels.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.OwnsOne("MyContactsAPI.Models.EmailModels.Verification", "Verification", b2 =>
                                {
                                    b2.Property<Guid>("EmailUserId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("Code")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("EmailVerificationCode");

                                    b2.Property<DateTimeOffset?>("ExpiresAt")
                                        .HasColumnType("timestamp with time zone")
                                        .HasColumnName("EmailVerificationExpiresAt");

                                    b2.Property<DateTimeOffset?>("VerifiedAt")
                                        .HasColumnType("timestamp with time zone")
                                        .HasColumnName("EmailVerificationVerifiedAt");

                                    b2.HasKey("EmailUserId");

                                    b2.ToTable("Users");

                                    b2.WithOwner()
                                        .HasForeignKey("EmailUserId");
                                });

                            b1.Navigation("Verification")
                                .IsRequired();
                        });

                    b.OwnsOne("MyContactsAPI.Models.PasswordModels.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Hash")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("PasswordHash");

                            b1.Property<string>("ResetCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("PasswordResetCode");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Password")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
