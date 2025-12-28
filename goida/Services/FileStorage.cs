using System.Security.Cryptography;
using goida.Data;
using goida.Entities;
using Microsoft.Extensions.Options;

namespace goida.Services;

public interface IFileStorage
{
    Task<StoredFile> SaveAsync(IFormFile file, string userId, CancellationToken cancellationToken = default);
    Task<(StoredFile File, Stream Content)> OpenReadAsync(string fileId, CancellationToken cancellationToken = default);
}

public class FileStorage(ApplicationDbContext dbContext, IOptions<FileStorageOptions> options) : IFileStorage
{
    private readonly FileStorageOptions _options = options.Value;

    public async Task<StoredFile> SaveAsync(IFormFile file, string userId, CancellationToken cancellationToken = default)
    {
        var contentType = file.ContentType ?? string.Empty;
        if (!_options.AllowedContentTypes.Contains(contentType))
        {
            throw new InvalidOperationException("Only PDF files are allowed.");
        }

        if (file.Length <= 0 || file.Length > _options.MaxFileSizeBytes)
        {
            throw new InvalidOperationException("Invalid file size.");
        }

        var safeFileName = Path.GetFileName(file.FileName);
        var storageFileName = $"{Guid.NewGuid():N}.pdf";
        var rootPath = Path.GetFullPath(_options.RootPath);
        Directory.CreateDirectory(rootPath);
        var storagePath = Path.Combine(rootPath, storageFileName);

        await using var targetStream = File.Create(storagePath);
        await file.CopyToAsync(targetStream, cancellationToken);

        targetStream.Position = 0;
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(await File.ReadAllBytesAsync(storagePath, cancellationToken));
        var hash = Convert.ToHexString(hashBytes).ToLowerInvariant();

        var stored = new StoredFile
        {
            FileName = safeFileName,
            ContentType = contentType,
            SizeBytes = file.Length,
            StoragePath = storageFileName,
            HashSha256 = hash,
            UploadedByUserId = userId
        };

        dbContext.StoredFiles.Add(stored);
        await dbContext.SaveChangesAsync(cancellationToken);

        return stored;
    }

    public async Task<(StoredFile File, Stream Content)> OpenReadAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var stored = await dbContext.StoredFiles.FindAsync([fileId], cancellationToken)
                     ?? throw new KeyNotFoundException("File not found");

        var rootPath = Path.GetFullPath(_options.RootPath);
        var storagePath = Path.Combine(rootPath, stored.StoragePath);

        var stream = File.OpenRead(storagePath);
        return (stored, stream);
    }
}
