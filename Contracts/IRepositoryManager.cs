namespace Contracts;

public interface IRepositoryManager
{
    IApplicationUserRepository ApplicationUser { get; }
    IFavoriteRecipeRepository FavoriteRecipe { get; }
    IRecipeRepository Recipe { get; }
    IStepRepository Step { get; }
    Task SaveAsync();
}