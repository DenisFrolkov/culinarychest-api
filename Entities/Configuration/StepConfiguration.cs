using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

public class StepConfiguration : IEntityTypeConfiguration<Step>
{
    public void Configure(EntityTypeBuilder<Step> builder)
    {
        builder.HasData
        (
            new Step
            {
                StepId = 1,
                Description = "1",
                Order = 1,
                RecipeId = 1
            }
        );
    }
}