using Savi_Thrift.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
