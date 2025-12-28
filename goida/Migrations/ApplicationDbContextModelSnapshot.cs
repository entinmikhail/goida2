using System;
using goida.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace goida.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.5")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("goida.Entities.ApplicantProfile", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<string>("DisplayName")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<string>("ExtractFileId")
                .HasMaxLength(64)
                .HasColumnType("character varying(64)");

            b.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("text");

            b.Property<bool>("Validated")
                .HasColumnType("boolean");

            b.Property<DateTimeOffset?>("ValidatedAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("ValidatedFileHash")
                .HasColumnType("text");

            b.HasKey("Id");

            b.HasIndex("ExtractFileId");

            b.HasIndex("UserId")
                .IsUnique();

            b.ToTable("ApplicantProfiles");
        });

        modelBuilder.Entity("goida.Entities.ApplicantSkill", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<Guid>("ApplicantProfileId")
                .HasColumnType("uuid");

            b.Property<string>("Tag")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("character varying(64)");

            b.HasKey("Id");

            b.HasIndex("ApplicantProfileId", "Tag")
                .IsUnique();

            b.ToTable("ApplicantSkills");
        });

        modelBuilder.Entity("goida.Entities.ApplicationUser", b =>
        {
            b.Property<string>("Id")
                .HasColumnType("text");

            b.Property<int>("AccessFailedCount")
                .HasColumnType("integer");

            b.Property<string>("ConcurrencyStamp")
                .HasColumnType("text");

            b.Property<string>("Email")
                .HasMaxLength(256)
                .HasColumnType("character varying(256)");

            b.Property<bool>("EmailConfirmed")
                .HasColumnType("boolean");

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

            b.Property<string>("PhoneNumber")
                .HasColumnType("text");

            b.Property<bool>("PhoneNumberConfirmed")
                .HasColumnType("boolean");

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

        modelBuilder.Entity("goida.Entities.ExperienceRecord", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<Guid>("ApplicantProfileId")
                .HasColumnType("uuid");

            b.Property<string>("CompanyName")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<DateTime>("DateFrom")
                .HasColumnType("timestamp without time zone");

            b.Property<DateTime?>("DateTo")
                .HasColumnType("timestamp without time zone");

            b.Property<string>("Position")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<string>("RawTextLine")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("Source")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            b.HasKey("Id");

            b.HasIndex("ApplicantProfileId");

            b.ToTable("ExperienceRecords");
        });

        modelBuilder.Entity("goida.Entities.StoredFile", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("character varying(64)");

            b.Property<string>("ContentType")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            b.Property<string>("FileName")
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("character varying(255)");

            b.Property<string>("HashSha256")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("character varying(128)");

            b.Property<long>("SizeBytes")
                .HasColumnType("bigint");

            b.Property<string>("StoragePath")
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("character varying(512)");

            b.Property<DateTimeOffset>("UploadedAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("UploadedByUserId")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");

            b.ToTable("StoredFiles");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
        {
            b.Property<string>("Id")
                .HasColumnType("text");

            b.Property<string>("ConcurrencyStamp")
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
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

        modelBuilder.Entity("goida.Entities.ApplicantProfile", b =>
        {
            b.HasOne("goida.Entities.StoredFile", "ExtractFile")
                .WithMany()
                .HasForeignKey("ExtractFileId")
                .OnDelete(DeleteBehavior.SetNull);

            b.HasOne("goida.Entities.ApplicationUser", "User")
                .WithOne("Profile")
                .HasForeignKey("goida.Entities.ApplicantProfile", "UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("ExtractFile");
            b.Navigation("User");
        });

        modelBuilder.Entity("goida.Entities.ApplicantSkill", b =>
        {
            b.HasOne("goida.Entities.ApplicantProfile", "ApplicantProfile")
                .WithMany("Skills")
                .HasForeignKey("ApplicantProfileId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("ApplicantProfile");
        });

        modelBuilder.Entity("goida.Entities.ExperienceRecord", b =>
        {
            b.HasOne("goida.Entities.ApplicantProfile", "ApplicantProfile")
                .WithMany("Experiences")
                .HasForeignKey("ApplicantProfileId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("ApplicantProfile");
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
            b.HasOne("goida.Entities.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
        {
            b.HasOne("goida.Entities.ApplicationUser", null)
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

            b.HasOne("goida.Entities.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
        {
            b.HasOne("goida.Entities.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}
