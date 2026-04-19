using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagement.Domain.ProcessSegments.Entities;
using RecipeManagement.Domain.ProductSegments.Entities;

namespace RecipeManagement.Infrastructure.Persistence.Configurations;

internal sealed class ProductSegmentParameterConfiguration : ParameterDefinitionConfiguration<ProductSegmentParameter>
{
    public override void Configure(EntityTypeBuilder<ProductSegmentParameter> builder)
    {
        base.Configure(builder);

        builder.ToTable("ProductSegmentParameters");

        // Map the Foreign Key to the template
        builder.HasOne<ProcessSegmentParameter>()
            .WithMany(e => e.ProductSegmentParameters)
            .HasForeignKey(x => x.ProcessSegmentParameterId);

        builder.HasOne(e => e.ProductSegment)
            .WithMany(ps => ps.Parameters)
            .HasForeignKey(e => e.ProductSegmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
