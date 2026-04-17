using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagement.Domain.MaterialDefinitions.Entities;

namespace RecipeManagement.Infrastructure.Persistence.Configurations;

internal sealed class MaterialDefinitionConfiguration : IEntityTypeConfiguration<MaterialDefinition>
{
    public void Configure(EntityTypeBuilder<MaterialDefinition> builder)
    {
        builder.ToTable("MaterialDefinitions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Sku)
            .IsRequired(true);

        builder.Property(e => e.Name)
            .IsRequired(true);

        builder.HasMany(e => e.Properties)
            .WithOne()
            .HasForeignKey(e => e.MaterialDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
