using CoolAnswers.Models;

namespace CoolAnswers.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, IList<string> roles);
}
