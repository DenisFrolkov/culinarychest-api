using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

public class FavoriteRecipeConfiguration : IEntityTypeConfiguration<FavoriteRecipe>
{
    public void Configure(EntityTypeBuilder<FavoriteRecipe> builder)
    {
        builder.HasData
        (
            new FavoriteRecipe
            {
                FavoriteRecipeId = 1,
                AuthorId = 1,
                RecipeId = 1,
                AddedDate = new DateTime(2024, 03, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}