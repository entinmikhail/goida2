using goida.Entities;
using goida.Services;
using Xunit;

namespace goida.Tests;

public class ExperienceCalculatorTests
{
    [Fact]
    public void Calculates_years_from_experiences()
    {
        var calculator = new ExperienceCalculator();
        var experiences = new List<ExperienceRecord>
        {
            new()
            {
                DateFrom = new DateTime(2020, 1, 1),
                DateTo = new DateTime(2021, 12, 31)
            },
            new()
            {
                DateFrom = new DateTime(2022, 1, 1),
                DateTo = new DateTime(2022, 12, 31)
            }
        };

        var years = calculator.CalculateYears(experiences);

        Assert.Equal(3.0m, years);
    }
}
