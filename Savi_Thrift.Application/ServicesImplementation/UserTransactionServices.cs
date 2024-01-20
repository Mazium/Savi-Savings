using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.UserTransaction;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class UserTransactionServices : IUserTransactionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserTransactionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }      

        public async Task<ApiResponse<List<GetTransactionDto>>> GetRecentTransactions()
        {
            var recentTransaction = await _unitOfWork.UserTransactionRepository.GetListOfTransactions();

            try
            {
                var transactionReturn = new List<GetTransactionDto>();
                foreach (var transaction in recentTransaction)
                {
                    var user = await _unitOfWork.UserRepository.GetByIdAsync(transaction.UserId);

                    transactionReturn.Add(new GetTransactionDto
                    {
                        Amount = transaction.Amount,
                        FullName = user == null ? " " : $"{user.FirstName} {user.LastName}",
                        CreatedAt = transaction.CreatedAt,
                        Description = transaction.Description,
                    });
                }
                //return transactionReturn;
                return ApiResponse<List<GetTransactionDto>>.Success(transactionReturn, "Goal Created Successfully", StatusCodes.Status200OK);                
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetTransactionDto>>.Failed("Error occurred while getting all goals", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }

        }

    }
}
