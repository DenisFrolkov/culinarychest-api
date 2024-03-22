using Entities.Models;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    IEnumerable<FavoriteRecipe> GetApplicationUserFavoriteRecipes(int authorId, bool trackChanges);

    void CreateApplicationUserFavoriteRecipe(int authorId, int recipeId,  FavoriteRecipe favoriteRecipe);
}
