using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Configurations;

public class EvaluationConfiguration : IEntityTypeConfiguration<Evaluation>
{
    public void Configure(EntityTypeBuilder<Evaluation> builder)
    {
        builder.ToTable("evaluation");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.TemplateId)
            .HasColumnName("template_id")
            .IsRequired(false);

        builder.Property(x => x.RecommendedContractModelId)
            .HasColumnName("recommended_contract_model_id")
            .IsRequired();

        builder.Property(x => x.ProjectName)
            .HasColumnName("project_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ClientName)
            .HasColumnName("client_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.EvaluatedAt)
            .HasColumnName("evaluated_at")
            .IsRequired();

        builder.Property(x => x.Score)
            .HasColumnName("score")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Confidence)
            .HasColumnName("confidence")
            .HasPrecision(18, 8)
            .IsRequired();

        builder.Property(x => x.TemplateName)
            .HasColumnName("template_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.TemplateDescription)
            .HasColumnName("template_description")
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasOne(x => x.Template)
            .WithMany()
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.RecommendedContractModel)
            .WithMany()
            .HasForeignKey(x => x.RecommendedContractModelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
