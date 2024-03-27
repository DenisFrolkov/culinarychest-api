namespace Contracts;

public interface IRepositoryManager
{
    
    // свойства, которые предоставляют доступ к репозиториям для различных сущностей в вашем приложении.
    // Каждое свойство возвращает интерфейс репозитория, который определяет операции, которые можно выполнять с соответствующей сущностью
    IApplicationUserRepository ApplicationUser { get; }
    IFavoriteRecipeRepository FavoriteRecipe { get; }
    IRecipeRepository Recipe { get; }
    IStepRepository Step { get; }
    Task SaveAsync();
}