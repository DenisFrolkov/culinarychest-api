namespace Contracts;

public interface IRepositoryManager
{
    IFavoriteRecipeRepository FavoriteRecipe { get; }
    IRecipeRepository Recipe { get; }
    IStepRepository Step { get; }
    Task SaveAsync();
}