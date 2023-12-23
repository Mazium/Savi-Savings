using System.Linq.Expressions;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T GetByIdAsync(string id);
        List<T> GetAll();
        List<T> FindAsync(Expression<Func<T, bool>> expression);
        void AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        void DeleteAllAsync(List<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        void SaveChangesAsync();
    }
}
