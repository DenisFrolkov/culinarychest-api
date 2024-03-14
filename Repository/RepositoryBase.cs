using System.Linq.Expressions;
using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;


//Класс RepositoryBase<T> является абстрактным базовым классом, который реализует интерфейс IRepositoryBase<T>.
//Этот класс предоставляет базовую реализацию для работы с сущностями типа T в базе данных, используя RepositoryContext.
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;

    public RepositoryBase(RepositoryContext repositoryContext)
    {
        //принимает экземпляр RepositoryContext, который используется для взаимодействия с базой данных.
        //Этот контекст сохраняется в защищенном поле RepositoryContext, чтобы его можно было использовать в методах класса.
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> FindAll(bool trackChanges) => //Возвращает IQueryable<T>, который представляет собой набор всех сущностей типа T.
                                                       //Параметр trackChanges указывает, следует ли отслеживать изменения в сущностях.
                                                       //Если trackChanges равно false, используется метод AsNoTracking(),
                                                       //который улучшает производительность за счет отключения отслеживания изменений в сущностях.
        !trackChanges
            ? RepositoryContext.Set<T>()
                .AsNoTracking()
            : RepositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        //Возвращает IQueryable<T>, который представляет собой набор сущностей типа T, соответствующих заданному условию.
        //Параметр trackChanges также указывает, следует ли отслеживать изменения в сущностях.
        !trackChanges
            ? RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking()
            : RepositoryContext.Set<T>()
                .Where(expression);

    //Эти методы выполняют операции создания, обновления и удаления сущностей типа T в базе данных.
    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
}