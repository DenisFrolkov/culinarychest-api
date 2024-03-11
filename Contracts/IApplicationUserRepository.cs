using Entities.Models;

namespace Contracts;

public interface IApplicationUserRepository
{
    public void Create(ApplicationUser applicationUser); //метод предназначен для создания нового пользователя в базе данных.
    //Он принимает объект ApplicationUser в качестве параметра, который содержит данные нового пользователя.
    public void Update(ApplicationUser applicationUser); //метод используется для обновления существующего пользователя в базе данных.
    //Он также принимает объект ApplicationUser, но в этом случае объект должен содержать обновленные данные пользователя.
    public void Delete(ApplicationUser applicationUser); //метод предназначен для удаления пользователя из базы данных.
    //Он принимает объект ApplicationUser, который идентифицирует пользователя, которого нужно удалить.
}