using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Configurations;

public class QuestionImpactConfiguration : IEntityTypeConfiguration<QuestionImpact>
{
    public void Configure(EntityTypeBuilder<QuestionImpact> builder)
    {
        builder.ToTable("question_impact");

        builder.HasKey(x => new { x.QuestionId, x.ContractModelId });

        builder.Property(x => x.QuestionId).HasColumnName("question_id");
        builder.Property(x => x.ContractModelId).HasColumnName("contract_model_id");

        builder.Property(x => x.ImpactValue)
            .HasColumnName("impact_value")
            .IsRequired();

        builder.HasOne(x => x.Question)
            .WithMany()
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ContractModel)
            .WithMany()
            .HasForeignKey(x => x.ContractModelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

