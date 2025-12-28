namespace goida.Entities;

public class ApplicantProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string DisplayName { get; set; } = string.Empty;
    public bool Validated { get; set; }
    public string? ExtractFileId { get; set; }
    public StoredFile? ExtractFile { get; set; }
    public DateTimeOffset? ValidatedAt { get; set; }
    public string? ValidatedFileHash { get; set; }
    public List<ApplicantSkill> Skills { get; set; } = [];
    public List<ExperienceRecord> Experiences { get; set; } = [];
}
