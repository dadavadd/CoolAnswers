using CoolAnswers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolAnswers.Data.EntityConfigurations;

public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Reason).IsRequired().HasMaxLength(500);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Question)
            .WithMany()
            .HasForeignKey(c => c.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
