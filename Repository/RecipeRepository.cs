using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository;

public class RecipeRepository: RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<PagedList<Recipe>> GetRecipesAsync(RecipeParameters recipeParameters, bool trackChanges)
    {
        var recipe = await FindAll(trackChanges)
            .Search(recipeParameters.SearchTerm)
            .OrderBy(e => e.Title)
            .ToListAsync();
        return PagedList<Recipe>
            .ToPagedList(recipe, recipeParameters.PageNumber, recipeParameters.PageSize);
    }
    
    public async Task<PagedList<Recipe>> GetRecipeByIdsAsync(List<int> recipeIds, RecipeParameters recipeParameters, bool trackChanges)
    {
        var query = FindAll(trackChanges)
            .Where(r => recipeIds.Contains(r.RecipeId)) 
            .Search(recipeParameters.SearchTerm)
            .OrderBy(e => e.Title);

        var recipes = await query.ToListAsync();
    
        return PagedList<Recipe>.ToPagedList(recipes, recipeParameters.PageNumber, recipeParameters.PageSize);
    }


    public async Task<PagedList<Recipe>> GetApplicationUserRecipesAsync(string authorId,
        ApplicationUserRecipeParameters applicationUserRecipeParameters, bool trackChanges)
    {
        var recipe =  await FindByCondition(recipe =>
                recipe.Id.Equals(authorId), trackChanges)
            .OrderBy(e => e.Title)
            .Skip((applicationUserRecipeParameters.PageNumber - 1) * applicationUserRecipeParameters.PageSize)
            .Take(applicationUserRecipeParameters.PageSize)
            .ToListAsync();
        return PagedList<Recipe>
            .ToPagedList(recipe, applicationUserRecipeParameters.PageNumber, applicationUserRecipeParameters.PageSize);
    }

    public async Task<Recipe> GetApplicationUserRecipeAsync(string authorId, int recipeId, bool trackChanges) =>
        await FindByCondition(recipe => 
            recipe.Id.Equals(authorId) && recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefaultAsync();

    public async Task<PagedList<Recipe>> GetRecipeByIdsAsync(int recipeId, RecipeParameters recipeParameters, bool trackChanges)
    {
        var recipe = await FindByCondition(recipe =>
                recipe.RecipeId.Equals(recipeId), trackChanges)
            .ToListAsync();
        return PagedList<Recipe>.ToPagedList(recipe, recipeParameters.PageNumber, recipeParameters.PageSize);
    }
    
    public async Task<Recipe> GetRecipe(int recipeId, bool trackChanges) =>
        await FindByCondition(recipe => 
            recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefaultAsync();

    public void CreateApplicationUserRecipe(string authorId, Recipe recipe)
    {
        recipe.Id = authorId;
        Create(recipe);
    }
    public void DeleteRecipe(Recipe recipe)
    {
        Delete(recipe);
    }
}