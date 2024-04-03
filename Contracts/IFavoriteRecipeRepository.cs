using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    Task<PagedList<FavoriteRecipe>> GetApplicationUserFavoriteRecipes(int authorId, ApplicationUserFavoriteRecipeParameters applicationUserFavoriteRecipeParameters, bool trackChanges);

    Task<FavoriteRecipe> GetApplicationUserFavoriteRecipe(int authorId, int favoriteRecipeId, bool trackChanges);

    void CreateApplicationUserFavoriteRecipe(int authorId, int recipeId,  FavoriteRecipe favoriteRecipe);
    
    void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe);
}
