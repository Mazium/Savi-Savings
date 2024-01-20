using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class UserTransactionRepository : GenericRepository<UserTransaction>, IUserTransactionRepository
    {
        private readonly SaviDbContext _saviDbContext;

        public UserTransactionRepository(SaviDbContext saviDbContext) : base(saviDbContext)
        {
            _saviDbContext = saviDbContext;
        }

        //public void AddTransaction(UserTransaction userTransaction) => AddAsync(userTransaction);

        //public void DeleteTransaction(UserTransaction userTransaction) => DeleteAsync(userTransaction);

        //public async Task<List<UserTransaction>> FindTransaction(Expression<Func<UserTransaction, bool>> condition) => await FindAsync(condition);


        //public async Task<List<UserTransaction>> GetAllAsync() => await GetAllAsync();


        //public Task<UserTransaction> GetTransactionById(string id) =>  GetByIdAsync(id);

        
        
        //public void UpdateTransaction(UserTransaction userTransaction) => UpdateTransaction(userTransaction);

        public async Task<List<UserTransaction>> GetListOfTransactions()
        {
            return await _saviDbContext.UserTransactions
                .OrderByDescending(u => u.CreatedAt)
                .Take(5).ToListAsync();

        }

    }
}
