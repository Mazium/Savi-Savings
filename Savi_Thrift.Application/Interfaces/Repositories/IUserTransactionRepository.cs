using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
    public interface IUserTransactionRepository : IGenericRepository<UserTransaction>
    {
       // List<UserTransaction> GetTransaction();
        //void AddTransaction(UserTransaction userTransaction);
        //void DeleteTransaction(UserTransaction userTransaction);
        //Task<List<UserTransaction>> FindTransaction(Expression<Func<UserTransaction, bool>> condition);
        //Task<UserTransaction> GetTransactionById(string id);
        //void UpdateTransaction(UserTransaction userTransaction);
        //Task<List<GetTransactionDto>> GetUserTransactions();
        Task<List<UserTransaction>> GetListOfTransactions();
    }
}
