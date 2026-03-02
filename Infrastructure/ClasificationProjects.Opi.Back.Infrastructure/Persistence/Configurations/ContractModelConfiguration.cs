using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Configurations;

public class ContractModelConfiguration : IEntityTypeConfiguration<ContractModel>
{
    public void Configure(EntityTypeBuilder<ContractModel> builder)
    {
        builder.ToTable("contract_model");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new ContractModel
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Code = "FIXED_PRICE",
                DisplayName = "Alcance y Precio Fijo"
            },
            new ContractModel
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                Code = "TURNKEY",
                DisplayName = "Llave en Mano (Turnkey)"
            },
            new ContractModel
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                Code = "TIME_AND_MATERIALS",
                DisplayName = "Time & Materials (T&M)"
            },
            new ContractModel
            {
                Id = new Guid("66666666-6666-6666-6666-666666666666"),
                Code = "STAFF_AUGMENTATION",
                DisplayName = "Staff Augmentation"
            }
        );
    }
}

