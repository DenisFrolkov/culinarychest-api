using Entities.Models;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    IEnumerable<FavoriteRecipe> GetApplicationUserFavoriteRecipes(int authorId, bool trackChanges);

    FavoriteRecipe GetApplicationUserFavoriteRecipe(int authorId, int favoriteRecipeId, bool trackChanges);

    void CreateApplicationUserFavoriteRecipe(int authorId, int recipeId,  FavoriteRecipe favoriteRecipe);
    
    void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe);
}
