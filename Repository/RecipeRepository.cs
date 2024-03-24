using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class RecipeRepository: RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Recipe> GetRecipes(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.RecipeId)
            .ToList();

    public IEnumerable<Recipe> GetApplicationUserRecipes(int authorId, bool trackChanges) =>
        FindByCondition(recipe => 
            recipe.AuthorId.Equals(authorId), trackChanges).OrderBy(e => e.Title);

    public Recipe GetApplicationUserRecipe(int authorId, int recipeId, bool trackChanges) =>
        FindByCondition(recipe => 
            recipe.AuthorId.Equals(authorId) && recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefault();

    public Recipe GetRecipe(int recipeId, bool trackChanges) =>
        FindByCondition(recipe => 
            recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefault();

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