using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagement.Domain.ProcessSegments.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Enums;

namespace RecipeManagement.Infrastructure.Persistence.Configurations;

internal sealed class ProcessSegmentConfiguration : IEntityTypeConfiguration<ProcessSegment>
{
    public void Configure(EntityTypeBuilder<ProcessSegment> builder)
    {
        builder.ToTable("ProcessSegments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired(true);

        builder.Property(e => e.StableId)
            .IsRequired(true);

        builder.Property(e => e.Version)
            .IsRequired(true);

        builder.HasMany(x => x.Parameters)
            .WithOne()
            .HasForeignKey(e => e.ProcessSegmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ProductSegments)
           .WithOne(e => e.ProcessSegment)
           .HasForeignKey(e => e.ProcessSegmentId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.State)
            .HasConversion<string>()
            .HasDefaultValue(ProcessSegmentState.Draft)
            .IsRequired();
    }
}
