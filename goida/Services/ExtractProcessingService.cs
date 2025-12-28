using goida.Data;
using goida.Entities;
using Microsoft.Extensions.Options;

namespace goida.Services;

public interface IExtractProcessingService
{
    Task ProcessAsync(ApplicantProfile profile, StoredFile storedFile, CancellationToken cancellationToken = default);
}

public class ExtractProcessingService(
    ApplicationDbContext dbContext,
    IExtractParser parser,
    IOptions<FileStorageOptions> options) : IExtractProcessingService
{
    public async Task ProcessAsync(ApplicantProfile profile, StoredFile storedFile, CancellationToken cancellationToken = default)
    {
        var storageRoot = Path.GetFullPath(options.Value.RootPath);
        var storagePath = Path.Combine(storageRoot, storedFile.StoragePath);
        await using var stream = File.OpenRead(storagePath);

        var parsed = await parser.ParseAsync(stream, cancellationToken);

        dbContext.ExperienceRecords.RemoveRange(profile.Experiences);
        profile.Experiences.Clear();

        foreach (var experience in parsed)
        {
            profile.Experiences.Add(new ExperienceRecord
            {
                ApplicantProfileId = profile.Id,
                CompanyName = experience.CompanyName,
                Position = experience.Position,
                DateFrom = experience.DateFrom,
                DateTo = experience.DateTo,
                RawTextLine = experience.RawLine,
                Source = "GosuslugiExtract"
            });
        }

        profile.Validated = parsed.Count > 0;
        profile.ValidatedAt = DateTimeOffset.UtcNow;
        profile.ValidatedFileHash = storedFile.HashSha256;
        profile.ExtractFileId = storedFile.Id;

        dbContext.ApplicantProfiles.Update(profile);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
