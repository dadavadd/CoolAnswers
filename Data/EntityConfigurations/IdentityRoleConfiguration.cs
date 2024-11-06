using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolAnswers.Data.EntityConfigurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                }
            );
        }
    }
}
