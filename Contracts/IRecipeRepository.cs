using Entities.Models;

namespace Contracts;

public interface IRecipeRepository
{
    public void Create(Recipe recipe); //метод предназначен для создания нового пользователя в базе данных.
    //Он принимает объект Recipe в качестве параметра, который содержит данные нового пользователя.
    
    public void Update(Recipe recipe); //метод используется для обновления существующего пользователя в базе данных.
    //Он также принимает объект Recipe, но в этом случае объект должен содержать обновленные данные пользователя.
    
    public void Delete(Recipe recipe); //метод предназначен для создания нового пользователя в базе данных.
    //Он принимает объект Recipe в качестве параметра, который содержит данные нового пользователя.
    
    IEnumerable<Recipe> GetAllRecipes(bool trackChanges); //этот метод, должен вернуть коллекцию всех рецептов из базы данных.
    IEnumerable<Recipe> GetApplicationUserAllRecipes(int authorId ,bool trackChanges);
    Recipe GetRecipe(int recipeId, bool trackChanges);

}