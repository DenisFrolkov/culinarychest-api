using Entities.Models;

namespace Repository.Extensions;

public static class RepositoryRecipeExtensions
{
    public static IQueryable<Recipe> Search(this IQueryable<Recipe> recipes, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return recipes;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return recipes.Where(recipe => recipe.Title.ToLower().Contains(lowerCaseTerm));
    }
}