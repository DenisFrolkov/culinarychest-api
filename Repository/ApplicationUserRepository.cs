using Contracts;
using Entities;
using Entities.Models;

namespace Repository;


//Класс ApplicationUserRepository является специализированным репозиторием, который наследуется от абстрактного базового класса
//RepositoryBase<T>, предназначенного для работы с сущностями типа ApplicationUser. Этот класс реализует интерфейс
//IApplicationUserRepository, что позволяет ему предоставлять специализированные методы для работы с пользователями в вашем приложении
public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
{
    //конструктор принимает экземпляр RepositoryContext, который используется для взаимодействия с базой данных.
    //Этот контекст передается в базовый класс RepositoryBase<T> через вызов base(repositoryContext),
    //что позволяет ApplicationUserRepository использовать базовую реализацию операций CRUD для сущностей типа ApplicationUser
    public ApplicationUserRepository(RepositoryContext repositoryContext) : base(repositoryContext){  }
    public IEnumerable<ApplicationUser> GetAllApplicationUsers(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.UserId)
            .ToList();

    public ApplicationUser GetApplicationUser(int userId, bool trackChanges) => 
        FindByCondition(applicationUser => 
            applicationUser.UserId.Equals(userId), trackChanges).SingleOrDefault();

    public void CreateApplicationUser(ApplicationUser applicationUser) => Create(applicationUser);
}