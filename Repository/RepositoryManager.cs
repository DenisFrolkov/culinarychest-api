using Contracts;
using Entities;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    
    private RepositoryContext _repositoryContext;
    private IApplicationUserRepository _applicationUserRepository;
    private IFavoriteRecipeRepository _favoriteRecipeRepository;
    private IRecipeRepository _recipeRepository;
    private IStepRepository _stepRepository;

    public RepositoryManager(RepositoryContext repositoryContext) //public RepositoryManager(RepositoryContext repositoryContext)
                                                                  //принимает экземпляр RepositoryContext, который используется для взаимодействия с базой данных.
                                                                  //Этот контекст передается в конструкторы репозиториев при их создании.
    {
        _repositoryContext = repositoryContext;
    }

    
    //Каждое свойство (ApplicationUser, FavoriteRecipe, Recipe, Step) возвращает экземпляр соответствующего репозитория.
    //Если репозиторий еще не был создан, он создается внутри свойства с использованием контекста базы данных.
    //Это обеспечивает ленивую инициализацию репозиториев, что означает, что они создаются только при первом обращении к свойству.
    public IApplicationUserRepository ApplicationUser
    {
        get
        {
            if (_applicationUserRepository == null)
                _applicationUserRepository = new ApplicationUserRepository(_repositoryContext);
            return _applicationUserRepository;
        }
    }

    public IFavoriteRecipeRepository FavoriteRecipe
    {
        get
        {
            if (_favoriteRecipeRepository == null)
                _favoriteRecipeRepository = new FavoriteRecipeRepository(_repositoryContext);
            return _favoriteRecipeRepository;
        }
    }

    public IRecipeRepository Recipe
    {
        get
        {
            if (_recipeRepository == null)
                _recipeRepository = new RecipeRepository(_repositoryContext);
            return _recipeRepository;
        }
    }

    public IStepRepository Step
    {
        get
        {
            if (_stepRepository == null)
                _stepRepository = new StepRepository(_repositoryContext);
            return _stepRepository;
        }
    }

    public Task SaveAsync() => _repositoryContext.SaveChangesAsync(); 
}