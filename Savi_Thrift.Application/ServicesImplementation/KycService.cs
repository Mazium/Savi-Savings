using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class KycService : IKycService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<KycService> _logger;
        private readonly ICloudinaryServices _cloudinaryServices;

        public KycService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<KycService> logger, ICloudinaryServices cloudinaryServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cloudinaryServices = cloudinaryServices;
        }

        public async Task<ApiResponse<KycResponseDto>> AddKyc(string userId, KycRequestDto kycDto)
        {
            try
            {
                var existingKyc = await _unitOfWork.KycRepository.GetKycByIdAsync(userId);
                if (existingKyc != null)
                {
                    return ApiResponse<KycResponseDto>.Failed(false, "KYC already exists for the user", StatusCodes.Status400BadRequest, new List<string>());
                }
                var newKyc = _mapper.Map<KYC>(kycDto);
                await _unitOfWork.KycRepository.AddKycAsync(newKyc);
                _unitOfWork.SaveChanges();
                var addedKycDto = _mapper.Map<KycResponseDto>(newKyc);
                return ApiResponse<KycResponseDto>.Success(addedKycDto, "KYC added successfully.", StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding KYC: {ex}");
                return ApiResponse<KycResponseDto>.Failed(false, "Error adding KYC.", StatusCodes.Status500InternalServerError, null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteKycById(string kycId)
        {
            try
            {
                var existingKyc = await _unitOfWork.KycRepository.GetKycByIdAsync(kycId);
                if (existingKyc == null)
                {
                    return ApiResponse<bool>.Failed(false, "KYC not found.", StatusCodes.Status404NotFound, null);
                }
                await _unitOfWork.KycRepository.DeleteKycAsync(existingKyc);
                _unitOfWork.SaveChanges();
                return ApiResponse<bool>.Success(true, "KYC deleted successfully.", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting KYC: {ex}");
                return ApiResponse<bool>.Failed(false, "Error deleting KYC.", StatusCodes.Status500InternalServerError, null);
            }
        }

        public Task<ApiResponse<PageResult<IEnumerable<KycResponseDto>>>> GetAllKycs(int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<KycResponseDto>> GetKycById(string kycId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateKyc(string kycId, KycRequestDto kycRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<CloudinaryUploadResponse>> UploadIdentificationDocument(string kycId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<CloudinaryUploadResponse>> UploadProofOfAddressDocument(string kycId, IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
