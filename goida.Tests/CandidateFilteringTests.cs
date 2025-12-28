using goida.Controllers;
using goida.Data;
using goida.Entities;
using goida.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace goida.Tests;

public class CandidateFilteringTests
{
    [Fact]
    public async Task Filters_and_sorts_candidates()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ApplicationDbContext(options);
        var profile1 = new ApplicantProfile
        {
            DisplayName = "Alice",
            Validated = true,
            Skills = [new ApplicantSkill { Tag = "c#" }],
            Experiences = [new ExperienceRecord { DateFrom = DateTime.UtcNow.AddYears(-5), DateTo = DateTime.UtcNow.AddYears(-3) }]
        };
        var profile2 = new ApplicantProfile
        {
            DisplayName = "Bob",
            Validated = false,
            Skills = [new ApplicantSkill { Tag = "java" }],
            Experiences = [new ExperienceRecord { DateFrom = DateTime.UtcNow.AddYears(-2), DateTo = DateTime.UtcNow.AddYears(-1) }]
        };
        context.ApplicantProfiles.AddRange(profile1, profile2);
        await context.SaveChangesAsync();

        var controller = new HrController(context, new ExperienceCalculator());

        var result = await controller.GetCandidates("name", "c#", false);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsAssignableFrom<IEnumerable<goida.Dtos.CandidateSummaryDto>>(ok.Value);

        var list = data.ToList();
        Assert.Single(list);
        Assert.Equal("Alice", list[0].DisplayName);
    }
}
