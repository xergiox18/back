using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Configurations;

public class EvaluationAnswerConfiguration : IEntityTypeConfiguration<EvaluationAnswer>
{
    public void Configure(EntityTypeBuilder<EvaluationAnswer> builder)
    {
        builder.ToTable("evaluation_answer");

        builder.HasKey(x => new { x.EvaluationId, x.QuestionId });

        builder.Property(x => x.EvaluationId)
            .HasColumnName("EvaluationId")
            .IsRequired();

        builder.Property(x => x.QuestionId)
            .HasColumnName("QuestionId")
            .IsRequired();

        builder.Property(x => x.Answer)
            .HasColumnName("Answer")
            .IsRequired();

        builder.HasOne(x => x.Evaluation)
            .WithMany()
            .HasForeignKey(x => x.EvaluationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.QuestionId);
    }
}