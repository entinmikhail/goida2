namespace goida.Dtos;

public record ProfileResponse(
    string DisplayName,
    bool Validated,
    DateTimeOffset? ValidatedAt,
    string? ExtractFileId,
    string? ExtractFileName,
    IReadOnlyList<string> Skills);

public record UpdateProfileRequest(string DisplayName, IReadOnlyList<string> Skills);

public record ExperienceDto(
    string CompanyName,
    string Position,
    DateTime DateFrom,
    DateTime? DateTo,
    string Source,
    string RawTextLine);

public record CandidateSummaryDto(
    Guid ProfileId,
    string DisplayName,
    bool Validated,
    decimal ExperienceYears,
    IReadOnlyList<string> Skills,
    string? ExtractFileId);

public record CandidateDetailsDto(
    Guid ProfileId,
    string DisplayName,
    bool Validated,
    decimal ExperienceYears,
    IReadOnlyList<string> Skills,
    IReadOnlyList<ExperienceDto> Experiences,
    string? ExtractFileId);
