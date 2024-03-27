using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RecipeRepository: RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<List<Recipe>> GetRecipes(bool trackChanges) =>
        await FindAll(trackChanges)
            .OrderBy(c => c.RecipeId)
            .ToListAsync();

    public async Task<List<Recipe>> GetApplicationUserRecipes(int authorId, bool trackChanges) =>
        await FindByCondition(recipe => 
            recipe.AuthorId.Equals(authorId), trackChanges).OrderBy(e => e.Title).ToListAsync();

    public async Task<Recipe> GetApplicationUserRecipe(int authorId, int recipeId, bool trackChanges) =>
        await FindByCondition(recipe => 
            recipe.AuthorId.Equals(authorId) && recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefaultAsync();

    public async Task<Recipe> GetRecipe(int recipeId, bool trackChanges) =>
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