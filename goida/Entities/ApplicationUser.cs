using Microsoft.AspNetCore.Identity;

namespace goida.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicantProfile? Profile { get; set; }
}
