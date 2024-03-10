using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasData
        (
            new Recipe
            {
                Id = 1,
                AuthorId = 1,
                Title = "Рецепт 1",
                RecipeImage = new byte[123],
                Ingredients = "Ингредиент 1",
                Steps = new List<Step>(),
                CreationDate = new DateTime(2024, 03, 10, 0, 0, 0, DateTimeKind.Utc),
                PreparationTime = new TimeSpan(0, 0, 30, 0, 0, 0),
                SavedCount = 0
            }
        );
    }
}