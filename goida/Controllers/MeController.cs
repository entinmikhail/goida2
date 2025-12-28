using goida.Data;
using goida.Dtos;
using goida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace goida.Controllers;

[ApiController]
[Authorize(Roles = Roles.Applicant)]
[Route("api/me")]
public class MeController(
    ApplicationDbContext dbContext,
    ICurrentUserService currentUser,
    IFileStorage fileStorage,
    IExtractProcessingService extractProcessingService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProfileResponse>> GetProfile()
    {
        var userId = currentUser.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        var profile = await dbContext.ApplicantProfiles
            .Include(p => p.Skills)
            .Include(p => p.ExtractFile)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(new ProfileResponse(
            profile.DisplayName,
            profile.Validated,
            profile.ValidatedAt,
            profile.ExtractFileId,
            profile.ExtractFile?.FileName,
            profile.Skills.Select(s => s.Tag).OrderBy(s => s).ToList()));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        var userId = currentUser.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        var profile = await dbContext.ApplicantProfiles
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
        {
            return NotFound();
        }

        profile.DisplayName = request.DisplayName;
        profile.Skills.Clear();
        foreach (var tag in request.Skills.Select(t => t.Trim().ToLowerInvariant()).Where(t => !string.IsNullOrWhiteSpace(t)))
        {
            profile.Skills.Add(new goida.Entities.ApplicantSkill
            {
                ApplicantProfileId = profile.Id,
                Tag = tag
            });
        }

        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("upload-extract")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<ActionResult> UploadExtract(IFormFile file, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        if (file == null)
        {
            return BadRequest(new { message = "File is required." });
        }

        var profile = await dbContext.ApplicantProfiles
            .Include(p => p.Experiences)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
        {
            return NotFound();
        }

        var stored = await fileStorage.SaveAsync(file, userId, cancellationToken);
        await extractProcessingService.ProcessAsync(profile, stored, cancellationToken);

        return Ok(new { stored.Id });
    }

    [HttpGet("experiences")]
    public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences()
    {
        var userId = currentUser.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        var profile = await dbContext.ApplicantProfiles
            .Include(p => p.Experiences)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile.Experiences.Select(exp => new ExperienceDto(
            exp.CompanyName,
            exp.Position,
            exp.DateFrom,
            exp.DateTo,
            exp.Source,
            exp.RawTextLine)));
    }
}
