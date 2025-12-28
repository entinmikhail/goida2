using goida.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace goida.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ApplicantProfile> ApplicantProfiles => Set<ApplicantProfile>();
    public DbSet<ApplicantSkill> ApplicantSkills => Set<ApplicantSkill>();
    public DbSet<ExperienceRecord> ExperienceRecords => Set<ExperienceRecord>();
    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicantProfile>(entity =>
        {
            entity.HasIndex(x => x.UserId).IsUnique();
            entity.Property(x => x.DisplayName).HasMaxLength(200);
            entity.Property(x => x.ExtractFileId).HasMaxLength(64);
            entity.HasOne(x => x.ExtractFile)
                .WithMany()
                .HasForeignKey(x => x.ExtractFileId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<ApplicantProfile>(x => x.UserId);
        });

        builder.Entity<ApplicantSkill>(entity =>
        {
            entity.Property(x => x.Tag).HasMaxLength(64);
            entity.HasIndex(x => new { x.ApplicantProfileId, x.Tag }).IsUnique();
        });

        builder.Entity<ExperienceRecord>(entity =>
        {
            entity.Property(x => x.CompanyName).HasMaxLength(200);
            entity.Property(x => x.Position).HasMaxLength(200);
            entity.Property(x => x.Source).HasMaxLength(100);
        });

        builder.Entity<StoredFile>(entity =>
        {
            entity.Property(x => x.Id).HasMaxLength(64);
            entity.Property(x => x.FileName).HasMaxLength(255);
            entity.Property(x => x.ContentType).HasMaxLength(100);
            entity.Property(x => x.StoragePath).HasMaxLength(512);
            entity.Property(x => x.HashSha256).HasMaxLength(128);
        });
    }
}
