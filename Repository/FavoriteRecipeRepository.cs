using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class FavoriteRecipeRepository : RepositoryBase<FavoriteRecipe>, IFavoriteRecipeRepository
{
    public FavoriteRecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
    
    public async Task<PagedList<FavoriteRecipe>> GetApplicationUserFavoriteRecipes(string authorId, ApplicationUserFavoriteRecipeParameters applicationUserFavoriteRecipeParameters, bool trackChanges)
    {
        var favoriteRecipe =  await FindByCondition(favoriteRecipe => 
                favoriteRecipe.Id.Equals(authorId), trackChanges)
            .OrderBy(e => e.RecipeId)
            .ToListAsync();
        return PagedList<FavoriteRecipe>
            .ToPagedList(favoriteRecipe, applicationUserFavoriteRecipeParameters.PageNumber,
                applicationUserFavoriteRecipeParameters.PageSize);
    }

    public async Task<FavoriteRecipe> GetApplicationUserFavoriteRecipe(string authorId, int recipeId, bool trackChanges) =>
        await FindByCondition(favoriteRecipe => 
                favoriteRecipe.Id.Equals(authorId) && favoriteRecipe.FavoriteRecipeId.Equals(recipeId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<FavoriteRecipe?> GetFavoriteRecipeByRecipeId(string authorId, int recipeId, bool trackChanges) =>
        await FindByCondition(favoriteRecipe => 
                favoriteRecipe.Id.Equals(authorId) && favoriteRecipe.RecipeId.Equals(recipeId), trackChanges)
            .SingleOrDefaultAsync();

    public void CreateApplicationUserFavoriteRecipe(string authorId, int recipeId, FavoriteRecipe favoriteRecipe)
    {
        favoriteRecipe.Id = authorId;
        favoriteRecipe.RecipeId = recipeId;
        Create(favoriteRecipe);
    }

    public void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe)
    {
        Delete(favoriteRecipe);
    }
}