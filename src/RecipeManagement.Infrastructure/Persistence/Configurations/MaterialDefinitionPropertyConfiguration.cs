using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagement.Domain.MaterialDefinitions.Entities;

namespace RecipeManagement.Infrastructure.Persistence.Configurations;

internal sealed class MaterialDefinitionPropertyConfiguration : IEntityTypeConfiguration<MaterialDefinitionProperty>
{
    public void Configure(EntityTypeBuilder<MaterialDefinitionProperty> builder)
    {
        builder.ToTable("MaterialDefinitionProperties");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired(true);

        builder.Property(e => e.Value)
            .IsRequired(true);

        builder.Property(e => e.DataType)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .IsRequired(false);

        builder.HasOne(e => e.MaterialDefinition)
            .WithMany(e => e.Properties)
            .HasForeignKey(e => e.MaterialDefinitionId);
    }
}
