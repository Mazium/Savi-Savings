using Savi_Thrift.Application.DTO.UserTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IUserTransactionServices
    {
        Task<ApiResponse<List<GetTransactionDto>>> GetRecentTransactions();
    }
}
