using Entities.Models;

namespace Contracts;

public interface IStepRepository
{
    public void Create(Step step);  //метод предназначен для создания нового пользователя в базе данных.
    //Он принимает объект Step в качестве параметра, который содержит данные нового пользователя.
    
    public void Update(Step step); //метод используется для обновления существующего пользователя в базе данных.
    //Он также принимает объект Step, но в этом случае объект должен содержать обновленные данные пользователя.
    
    public void Delete(Step step); //метод предназначен для создания нового пользователя в базе данных.
    //Он принимает объект Step в качестве параметра, который содержит данные нового пользователя.
    
    IEnumerable<Step> GetAllSteps(bool trackChanges); //этот метод, должен вернуть коллекцию всех шагов из базы данных.
    Step GetStep(int stepId, bool trackChanges);
}