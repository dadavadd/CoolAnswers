using CoolAnswers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolAnswers.Data.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Rank).HasMaxLength(100);
            builder.Property(u => u.Point).HasDefaultValue(0);
        }
    }
}
