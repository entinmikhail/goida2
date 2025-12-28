namespace goida.Services;

public class FileStorageOptions
{
    public string RootPath { get; set; } = "/appdata";
    public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024;
    public string[] AllowedContentTypes { get; set; } = ["application/pdf"];
}
