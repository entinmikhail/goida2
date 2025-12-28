using goida.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace goida.Data;

public class DataSeeder(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task SeedAsync()
    {
        if (!await roleManager.RoleExistsAsync(Roles.Hr))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Hr));
        }

        if (!await roleManager.RoleExistsAsync(Roles.Applicant))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Applicant));
        }

        var hrEmail = Environment.GetEnvironmentVariable("HR_EMAIL") ?? "hr@example.com";
        var hrPassword = Environment.GetEnvironmentVariable("HR_PASSWORD") ?? "ChangeMe123!";
        var hrUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == hrEmail);

        if (hrUser == null)
        {
            hrUser = new ApplicationUser
            {
                UserName = hrEmail,
                Email = hrEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(hrUser, hrPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(hrUser, Roles.Hr))
        {
            await userManager.AddToRoleAsync(hrUser, Roles.Hr);
        }

        if (!await dbContext.ApplicantProfiles.AnyAsync())
        {
            var demoUser = new ApplicationUser
            {
                UserName = "applicant@example.com",
                Email = "applicant@example.com",
                EmailConfirmed = true
            };
            var created = await userManager.CreateAsync(demoUser, "ChangeMe123!");
            if (created.Succeeded)
            {
                await userManager.AddToRoleAsync(demoUser, Roles.Applicant);
                dbContext.ApplicantProfiles.Add(new ApplicantProfile
                {
                    UserId = demoUser.Id,
                    DisplayName = "Иван Петров",
                    Validated = false
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

public static class Roles
{
    public const string Applicant = "Applicant";
    public const string Hr = "HR";
}
