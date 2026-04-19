using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagement.Domain.ProcessSegments.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Entities;

namespace RecipeManagement.Infrastructure.Persistence.Configurations;

internal sealed class ProcessSegmentParameterConfiguration : ParameterDefinitionConfiguration<ProcessSegmentParameter>
{
    public override void Configure(EntityTypeBuilder<ProcessSegmentParameter> builder)
    {
        base.Configure(builder);

        builder.ToTable("ProcessSegmentParameters");

        builder.Property(x => x.IsReadOnly)
            .IsRequired();

        builder.HasOne<ProcessSegment>()
           .WithMany(ps => ps.Parameters)
           .HasForeignKey(x => x.ProcessSegmentId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ProductSegmentParameters)
            .WithOne(e => e.ProcessSegmentParameter)
            .HasForeignKey(e => e.ProcessSegmentParameterId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
