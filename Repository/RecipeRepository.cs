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
    
    // public async Task<List<Recipe>> GetRecipesAsync(RecipeParameters recipeParameters, bool trackChanges) =>  await FindAll(trackChanges)
    //     .OrderBy(e => e.Title)
    //     .Skip((recipeParameters.PageNumber - 1) * recipeParameters.PageSize) 
    //     .Take(recipeParameters.PageSize)
    //     .ToListAsync();

    public async Task<PagedList<Recipe>> GetRecipesAsync(RecipeParameters recipeParameters, bool trackChanges)
    {
        var recipe = await FindAll(trackChanges)
            .Search(recipeParameters.SearchTerm)
            .OrderBy(e => e.Title)
            .ToListAsync();
        return PagedList<Recipe>
            .ToPagedList(recipe, recipeParameters.PageNumber, recipeParameters.PageSize);
    }

    public async Task<PagedList<Recipe>> GetApplicationUserRecipesAsync(int authorId,
        ApplicationUserRecipeParameters applicationUserRecipeParameters, bool trackChanges)
    {
        var recipe =  await FindByCondition(recipe =>
                recipe.AuthorId.Equals(authorId), trackChanges)
            .OrderBy(e => e.Title)
            .Skip((applicationUserRecipeParameters.PageNumber - 1) * applicationUserRecipeParameters.PageSize)
            .Take(applicationUserRecipeParameters.PageSize)
            .ToListAsync();
        return PagedList<Recipe>
            .ToPagedList(recipe, applicationUserRecipeParameters.PageNumber, applicationUserRecipeParameters.PageSize);
    }

    public async Task<Recipe> GetApplicationUserRecipeAsync(int authorId, int recipeId, bool trackChanges) =>
        await FindByCondition(recipe => 
            recipe.AuthorId.Equals(authorId) && recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefaultAsync();

    public async Task<Recipe> GetRecipeAsync(int recipeId, bool trackChanges) =>
        await FindByCondition(recipe => 
            recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefaultAsync();

    public void CreateApplicationUserRecipe(int authorId, Recipe recipe)
    {
        recipe.AuthorId = authorId;
        Create(recipe);
    }
    public void DeleteRecipe(Recipe recipe)
    {
        Delete(recipe);
    }
}