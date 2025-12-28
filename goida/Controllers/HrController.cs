using goida.Data;
using goida.Dtos;
using goida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace goida.Controllers;

[ApiController]
[Authorize(Roles = Roles.Hr)]
[Route("api/hr/candidates")]
public class HrController(
    ApplicationDbContext dbContext,
    IExperienceCalculator experienceCalculator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidateSummaryDto>>> GetCandidates(
        [FromQuery] string? sort,
        [FromQuery] string? filter,
        [FromQuery] bool validatedOnly = false)
    {
        var query = dbContext.ApplicantProfiles
            .Include(p => p.Skills)
            .Include(p => p.Experiences)
            .AsNoTracking();

        if (validatedOnly)
        {
            query = query.Where(p => p.Validated);
        }

        if (!string.IsNullOrWhiteSpace(filter))
        {
            var tag = filter.Trim().ToLowerInvariant();
            query = query.Where(p => p.Skills.Any(s => s.Tag == tag));
        }

        var candidates = await query.ToListAsync();
        var results = candidates.Select(profile => new CandidateSummaryDto(
            profile.Id,
            profile.DisplayName,
            profile.Validated,
            experienceCalculator.CalculateYears(profile.Experiences),
            profile.Skills.Select(s => s.Tag).OrderBy(s => s).ToList(),
            profile.ExtractFileId
        ));

        results = sort switch
        {
            "validated" => results.OrderByDescending(r => r.Validated).ThenBy(r => r.DisplayName),
            "experienceYears" => results.OrderByDescending(r => r.ExperienceYears),
            "name" => results.OrderBy(r => r.DisplayName),
            _ => results.OrderBy(r => r.DisplayName)
        };

        return Ok(results.ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CandidateDetailsDto>> GetCandidate(Guid id)
    {
        var profile = await dbContext.ApplicantProfiles
            .Include(p => p.Skills)
            .Include(p => p.Experiences)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(new CandidateDetailsDto(
            profile.Id,
            profile.DisplayName,
            profile.Validated,
            experienceCalculator.CalculateYears(profile.Experiences),
            profile.Skills.Select(s => s.Tag).OrderBy(s => s).ToList(),
            profile.Experiences.Select(exp => new ExperienceDto(
                exp.CompanyName,
                exp.Position,
                exp.DateFrom,
                exp.DateTo,
                exp.Source,
                exp.RawTextLine)).ToList(),
            profile.ExtractFileId));
    }
}
