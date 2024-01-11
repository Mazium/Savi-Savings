using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Common.Utilities;
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
        Task<ApiResponse<KycResponseDto>> AddKyc(string userId, KycRequestDto kycDto);
        Task<ApiResponse<bool>> DeleteKycById(string kycId);
        Task<ApiResponse<GetAllKycsDto>> GetAllKycs(int page, int perPage);
        Task<ApiResponse<KycResponseDto>> GetKycById(string kycId);
        Task<ApiResponse<KycResponseDto>> UpdateKyc(string kycId, KycRequestDto kycRequest);
    }
}
