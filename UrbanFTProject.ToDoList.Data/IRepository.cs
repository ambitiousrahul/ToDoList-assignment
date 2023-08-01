using System.Data;

namespace UrbanFTProject.Repository
{
    /// <summary>
    /// Repository Pattern Generic Implementation 
    /// </summary>
    /// <typeparam name="T">The Entitity which we need to abstract</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<DataRowState> DeleteAsync(int entityId);
    }
}
