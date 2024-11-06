using Microsoft.AspNetCore.Identity;

namespace CoolAnswers.Models;

public class User : IdentityUser
{
    public int Point { get; set; }
    public string Rank { get; set; }
}
