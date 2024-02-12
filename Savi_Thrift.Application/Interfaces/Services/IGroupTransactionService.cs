using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IGroupTransactionService
    {
        Task<ApiResponse<List<GroupRecentTransactionDto>>> GetGroupRecentTransaction();
    }
}
