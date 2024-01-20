using Savi_Thrift.Application.DTO.UserTransaction;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IUserTransactionServices
    {
        Task<ApiResponse<List<GetTransactionDto>>> GetRecentTransactions();
    }
}
