using CoolAnswers.Services.Interfaces;

namespace CoolAnswers.Services;

public class RankService : IRankService
{
    private readonly IConfiguration _configuration;

    public RankService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetRank(int pointCount)
    {
        return CalculateRank(pointCount);
    }

    private string CalculateRank(int pointCount)
    {
        return pointCount switch
        {
            >= 10000 => _configuration["Ranks:Expert"],
            >= 2500 => _configuration["Ranks:Advanced"],
            >= 500 => _configuration["Ranks:ActiveUser"],
            _ => _configuration["Ranks:Beginner"]
        };
    }
}
