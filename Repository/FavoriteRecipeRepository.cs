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
                favoriteRecipe.AuthorId.Equals(authorId), trackChanges)
            .OrderBy(e => e.RecipeId)
            .ToListAsync();
        return PagedList<FavoriteRecipe>
            .ToPagedList(favoriteRecipe, applicationUserFavoriteRecipeParameters.PageNumber,
                applicationUserFavoriteRecipeParameters.PageSize);
    }

    public async Task<FavoriteRecipe> GetApplicationUserFavoriteRecipe(string authorId, int favoriteRecipeId, bool trackChanges) =>
        await FindByCondition(favoriteRecipe => 
                favoriteRecipe.AuthorId.Equals(authorId) && favoriteRecipe.FavoriteRecipeId.Equals(favoriteRecipeId), trackChanges)
            .SingleOrDefaultAsync();

    public void CreateApplicationUserFavoriteRecipe(string authorId, int recipeId, FavoriteRecipe favoriteRecipe)
    {
        favoriteRecipe.AuthorId = authorId;
        favoriteRecipe.RecipeId = recipeId;
        Create(favoriteRecipe);
    }

    public void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe)
    {
        Delete(favoriteRecipe);
    }
}