using goida.Entities;

namespace goida.Services;

public interface IExperienceCalculator
{
    int CalculateTotalMonths(IEnumerable<ExperienceRecord> experiences);
    decimal CalculateYears(IEnumerable<ExperienceRecord> experiences);
}

public class ExperienceCalculator : IExperienceCalculator
{
    public int CalculateTotalMonths(IEnumerable<ExperienceRecord> experiences)
    {
        var months = 0;
        foreach (var exp in experiences)
        {
            var end = exp.DateTo ?? DateTime.UtcNow.Date;
            var start = exp.DateFrom.Date;
            if (end < start)
            {
                continue;
            }
            months += (end.Year - start.Year) * 12 + end.Month - start.Month + 1;
        }
        return Math.Max(0, months);
    }

    public decimal CalculateYears(IEnumerable<ExperienceRecord> experiences)
    {
        var months = CalculateTotalMonths(experiences);
        return Math.Round(months / 12m, 1);
    }
}
