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
        Task<ApiResponse<PageResult<IEnumerable<KycResponseDto>>>> GetAllKycs(int page, int perPage);
        Task<ApiResponse<KycResponseDto>> GetKycById(string kycId);
        Task<ApiResponse<bool>> UpdateKyc(string kycId, KycRequestDto kycRequest);
        Task<ApiResponse<CloudinaryUploadResponse>> UploadIdentificationDocument(string kycId, IFormFile file);
        Task<ApiResponse<CloudinaryUploadResponse>> UploadProofOfAddressDocument(string kycId, IFormFile file);
    }
}
