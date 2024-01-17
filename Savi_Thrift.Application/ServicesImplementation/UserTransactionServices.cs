using Savi_Thrift.Application.DTO.UserTransaction;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class UserTransactionServices : IUserTransactionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserTransactionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetTransactionDto>> GetRecentTransactions()
        {
            var recentTransaction = await _unitOfWork.UserTransactionRepository.GetListOfTransactions();

            var transactionReturn = new List<GetTransactionDto>();
            foreach (var transaction in recentTransaction)
            {
                transactionReturn.Add(new GetTransactionDto
                {
                    Amount = transaction.Amount,
                    FullName = GetUserFullname(transaction.UserId),
                    CreatedAt = transaction.CreatedAt,
                    Description = transaction.Description,
                });
            }
            return transactionReturn;
        }
        private string GetUserFullname(string id)
        {
            var user = _unitOfWork.UserRepository.GetByIdAsync(id);
            return user == null ? " " : user.FirstName + " " + user.LastName;

        }
    }
}
