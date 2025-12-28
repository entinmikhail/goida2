namespace goida.Entities;

public class ApplicantSkill
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicantProfileId { get; set; }
    public ApplicantProfile ApplicantProfile { get; set; } = null!;
    public string Tag { get; set; } = string.Empty;
}
