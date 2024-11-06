using CoolAnswers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolAnswers.Data.EntityConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Title).IsRequired().HasMaxLength(200);
        builder.Property(q => q.Content).IsRequired().HasMaxLength(2000);

        builder.HasOne(q => q.User)
            .WithMany()
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
