using Entities.Models;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    public void Create(FavoriteRecipe favoriteRecipe); //метод предназначен для создания нового пользователя в базе данных.
    //Он принимает объект FavoriteRecipe в качестве параметра, который содержит данные нового пользователя.
    
    public void Update(FavoriteRecipe favoriteRecipe); //метод используется для обновления существующего пользователя в базе данных.
    //Он также принимает объект FavoriteRecipe, но в этом случае объект должен содержать обновленные данные пользователя.
    
    public void Delete(FavoriteRecipe favoriteRecipe); //метод предназначен для удаления пользователя из базы данных.
    //Он принимает объект FavoriteRecipe, который идентифицирует пользователя, которого нужно удалить.

    IEnumerable<FavoriteRecipe> GetAllFavoriteRecipes(bool trackChanges); //этот метод, должен вернуть коллекцию всех избранных рецептов из базы данных.

}