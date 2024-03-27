﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Savi_Thrift.Persistence.Context;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    [DbContext(typeof(SaviDbContext))]
    [Migration("20240320131254_PostgresMigration")]
    partial class PostgresMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.Actions", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("ActionId")
                        .HasColumnType("integer");

                    b.Property<string>("ActionName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.CardDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<string>("CVV")
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Expiry")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NameOnCard")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("CardDetails");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.DefaultingUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("ActualDebitDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("AmountDefaulted")
                        .HasColumnType("numeric");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DefaultingPaymentStatus")
                        .HasColumnType("integer");

                    b.Property<string>("GroupSavingId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecipientUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DefaultingUsers");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupSavings", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ActualStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<decimal>("ContributionAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpectedStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<string>("GroupName")
                        .HasColumnType("text");

                    b.Property<int>("GroupStatus")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("NextRunTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PurposeAndGoal")
                        .HasColumnType("text");

                    b.Property<DateTime>("RunTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SafeLandScapeImageURL")
                        .HasColumnType("text");

                    b.Property<string>("SafePortraitImageURL")
                        .HasColumnType("text");

                    b.Property<string>("TermsAndConditions")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("GroupSavings");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupSavingsMembers", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GroupSavingsId")
                        .HasColumnType("text");

                    b.Property<bool>("HasCollected")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsGroupOwner")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastSavingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Position")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupSavingsId");

                    b.ToTable("GroupSavingsMembers");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupTransactions", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ActionId")
                        .HasColumnType("text");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GroupSavingsId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("GroupSavingsId");

                    b.ToTable("GroupTransactions");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.KYC", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<string>("BVN")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("IdentificationDocumentUrl")
                        .HasColumnType("text");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("text");

                    b.Property<int>("IdentificationType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Occupation")
                        .HasColumnType("integer");

                    b.Property<string>("ProofOfAddressUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("KYCs");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.Saving", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<decimal>("AmountToAdd")
                        .HasColumnType("numeric");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<decimal>("GoalAmount")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsAutomatic")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("WalletNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Savings");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.UserTransaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("ActionId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SavingsId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("WalletNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserTransactions");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.Wallet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PaystackCustomerCode")
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<string>("TransactionPin")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("WalletNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.WalletFunding", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("ActionId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("CumulativeAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("FundAmount")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Narration")
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<string>("WalletId")
                        .HasColumnType("text");

                    b.Property<string>("WalletNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletFundings");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.CardDetail", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany("CardDetails")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupSavings", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany("GroupSavings")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupSavingsMembers", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.GroupSavings", null)
                        .WithMany("GroupSavingsMembers")
                        .HasForeignKey("GroupSavingsId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupTransactions", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany("GroupTransactions")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Savi_Thrift.Domain.Entities.GroupSavings", null)
                        .WithMany("GroupSavingsFundings")
                        .HasForeignKey("GroupSavingsId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.Saving", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany("Savings")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.UserTransaction", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.AppUser", null)
                        .WithMany("UserTransactions")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.WalletFunding", b =>
                {
                    b.HasOne("Savi_Thrift.Domain.Entities.Wallet", null)
                        .WithMany("WalletFundings")
                        .HasForeignKey("WalletId");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.AppUser", b =>
                {
                    b.Navigation("CardDetails");

                    b.Navigation("GroupSavings");

                    b.Navigation("GroupTransactions");

                    b.Navigation("Savings");

                    b.Navigation("UserTransactions");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.GroupSavings", b =>
                {
                    b.Navigation("GroupSavingsFundings");

                    b.Navigation("GroupSavingsMembers");
                });

            modelBuilder.Entity("Savi_Thrift.Domain.Entities.Wallet", b =>
                {
                    b.Navigation("WalletFundings");
                });
#pragma warning restore 612, 618
        }
    }
}