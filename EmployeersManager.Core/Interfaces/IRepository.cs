namespace EmployeersManager.Core.Interfaces;

//default int key type
public interface IRepository<T> : IRepository<T,int> where T : class { }

public interface IRepository<T, KEY_TYPE> where T : class //create common interface for model
{
    //[skip] [take] for big data
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T employee);
    Task UpdateAsync(T employee);
    Task<bool> DeleteAsync(KEY_TYPE id);
}
