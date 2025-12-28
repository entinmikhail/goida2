namespace goida.Entities;

public class ExperienceRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicantProfileId { get; set; }
    public ApplicantProfile ApplicantProfile { get; set; } = null!;
    public string CompanyName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string Source { get; set; } = "GosuslugiExtract";
    public string RawTextLine { get; set; } = string.Empty;
}
