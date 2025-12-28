using goida.Data;
using goida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace goida.Controllers;

[ApiController]
[Authorize]
[Route("api/files")]
public class FilesController(
    ApplicationDbContext dbContext,
    ICurrentUserService currentUser,
    IFileStorage fileStorage) : ControllerBase
{
    [HttpGet("{fileId}")]
    public async Task<IActionResult> Download(string fileId, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId == null)
        {
            return Unauthorized();
        }

        var file = await dbContext.StoredFiles.FirstOrDefaultAsync(f => f.Id == fileId, cancellationToken);
        if (file == null)
        {
            return NotFound();
        }

        if (!currentUser.IsInRole(Roles.Hr))
        {
            var profile = await dbContext.ApplicantProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ExtractFileId == fileId, cancellationToken);
            if (profile == null)
            {
                return Forbid();
            }
        }

        var (stored, stream) = await fileStorage.OpenReadAsync(fileId, cancellationToken);
        return File(stream, stored.ContentType, stored.FileName);
    }
}
