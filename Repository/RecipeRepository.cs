using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class RecipeRepository: RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Recipe> GetAllRecipes(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.RecipeId)
            .ToList();

    public IEnumerable<Recipe> GetApplicationUserAllRecipes(int authorId, bool trackChanges) =>
        FindByCondition(recipe => 
            recipe.AuthorId.Equals(authorId), trackChanges).OrderBy(e => e.Title);

    public Recipe GetRecipe(int recipeId, bool trackChanges) =>
        FindByCondition(recipe => 
            recipe.RecipeId.Equals(recipeId), trackChanges).SingleOrDefault();
}