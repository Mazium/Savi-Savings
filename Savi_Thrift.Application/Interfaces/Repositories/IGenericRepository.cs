using System.Linq.Expressions;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAll();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
        void AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        void DeleteAllAsync(List<T> entities);
        void SaveChangesAsync();
    }
}
