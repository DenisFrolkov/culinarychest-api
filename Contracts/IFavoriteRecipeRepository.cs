using Entities.Models;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    Task<List<FavoriteRecipe>> GetApplicationUserFavoriteRecipes(int authorId, bool trackChanges);

    Task<FavoriteRecipe> GetApplicationUserFavoriteRecipe(int authorId, int favoriteRecipeId, bool trackChanges);

    void CreateApplicationUserFavoriteRecipe(int authorId, int recipeId,  FavoriteRecipe favoriteRecipe);
    
    void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe);
}
