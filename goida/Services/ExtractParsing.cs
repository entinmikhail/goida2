using goida.Entities;

namespace goida.Services;

public record ParsedExperience(string CompanyName, string Position, DateTime DateFrom, DateTime? DateTo, string RawLine);

public interface IExtractParser
{
    Task<IReadOnlyList<ParsedExperience>> ParseAsync(Stream pdfStream, CancellationToken cancellationToken = default);
}

public class StubExtractParser : IExtractParser
{
    public async Task<IReadOnlyList<ParsedExperience>> ParseAsync(Stream pdfStream, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(pdfStream, leaveOpen: true);
        var content = await reader.ReadToEndAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(content))
        {
            return [
                new ParsedExperience(
                    "ООО Пример",
                    "Software Engineer",
                    DateTime.UtcNow.AddYears(-3).Date,
                    DateTime.UtcNow.Date,
                    "ООО Пример — Software Engineer 2021-2024")
            ];
        }

        var lines = content.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
        var parsed = new List<ParsedExperience>();
        foreach (var line in lines)
        {
            parsed.Add(new ParsedExperience(
                "Компания из выписки",
                "Должность из выписки",
                DateTime.UtcNow.AddYears(-1).Date,
                null,
                line.Trim()));
        }

        return parsed.Count > 0 ? parsed : [
            new ParsedExperience(
                "ООО Пример",
                "Software Engineer",
                DateTime.UtcNow.AddYears(-3).Date,
                DateTime.UtcNow.Date,
                "ООО Пример — Software Engineer 2021-2024")
        ];
    }
}
