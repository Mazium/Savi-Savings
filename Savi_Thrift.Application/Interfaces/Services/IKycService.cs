using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IKycService
    {
        Task<ApiResponse<KycRequestDto>> GetKycByIdAsync(string id);
        Task<ApiResponse<GetAllKycsDto>> GetKycsByPaginationAsync(int page, int perPage);
        Task<ApiResponse<KycResponseDto>> AddKycAsync(string userId, KycRequestDto kycDto);
        Task<ApiResponse<KycResponseDto>> UpdateKycAsync(string kycId, UpdateKycDto updateKycDto);
        Task<ApiResponse<bool>> DeleteKycByIdAsync(string kycId);
    }
}
