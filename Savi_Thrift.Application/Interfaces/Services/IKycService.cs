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
        //Task<ApiResponse<KycRequestDto>> GetKycByIdAsync(string id);
        //Task<ApiResponse<GetAllKycsDto>> GetKycsByPaginationAsync(int page, int perPage);
        //Task<ApiResponse<KycResponseDto>> AddKycAsync(string userId, KycRequestDto kycDto);
        //Task<ApiResponse<KycResponseDto>> UpdateKycAsync(string kycId, UpdateKycDto updateKycDto);
        //Task<ApiResponse<bool>> DeleteKycByIdAsync(string kycId);


        //Task<ApiResponse<KycResponseDto>> AddKyc(KycRequestDto kycRequest);
        //Task<ApiResponse<bool>> DeleteKyc(string kycId);
        //Task<ApiResponse<PageResult<KycResponseDto>>> GetAllKycs(int page, int perPage);
        //Task<ApiResponse<KycResponseDto>> GetKycById(string kycId);
        //Task<ApiResponse<bool>> UpdateKyc(string kycId, KycRequestDto updatedKyc);
        //Task<ApiResponse<string>> UploadIdentificationDocument(string kycId, IFormFile file);
        //Task<ApiResponse<string>> UploadProofOfAddressDocument(string kycId, IFormFile file);

        Task<ApiResponse<KycResponseDto>> AddKyc(KycRequestDto kycRequest);
        Task<ApiResponse<bool>> DeleteKyc(string kycId);
        Task<ApiResponse<PageResult<KycResponseDto>>> GetAllKycs(int page, int perPage);
        Task<ApiResponse<KycResponseDto>> GetKycById(string kycId);
        Task<ApiResponse<bool>> UpdateKyc(string kycId, KycRequestDto kycRequest);
        Task<ApiResponse<CloudinaryUploadResponse>> UploadIdentificationDocument(string kycId, IFormFile file);
        Task<ApiResponse<CloudinaryUploadResponse>> UploadProofOfAddressDocument(string kycId, IFormFile file);


    }
}
