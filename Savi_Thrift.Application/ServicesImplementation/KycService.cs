﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Domain.Entities;
using Serilog;
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

                var identificationDocumentUrl = await _cloudinaryServices.UploadImage(kycDto.IdentificationDocumentUrl);
                var proofOfAddressUrl = await _cloudinaryServices.UploadImage(kycDto.ProofOfAddressUrl);
                if (identificationDocumentUrl == null || proofOfAddressUrl == null)
                {
                    return ApiResponse<KycResponseDto>.Failed(false, "Failed to upload one or more documents.", StatusCodes.Status500InternalServerError, null);
                }
                
                var newKyc = _mapper.Map<KYC>(kycDto);
                newKyc.IdentificationDocumentUrl = identificationDocumentUrl.Url;
                newKyc.ProofOfAddressUrl = proofOfAddressUrl.Url;

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

        public async Task<ApiResponse<GetAllKycsDto>> GetAllKycs(int page, int perPage)
        {
            try
            {
                var kycs = _unitOfWork.KycRepository.GetAllKycs();
                var kycDtos = _mapper.Map<List<KycResponseDto>>(kycs);

                var pagedResult = await PagePagination<KycResponseDto>.GetPager(
                    kycDtos,
                    perPage,
                    page,
                    kyc => kyc.IdentificationDocumentUrl,
                    kyc => kyc.IdentificationNumber
                );

                var response = new GetAllKycsDto
                {
                    Kycs = pagedResult.Data.ToList(),
                    TotalCount = pagedResult.TotalCount,
                    TotalPageCount = pagedResult.TotalPageCount,
                    PerPage = pagedResult.PerPage,
                    CurrentPage = pagedResult.CurrentPage
                };

                return ApiResponse<GetAllKycsDto>.Success(response, "KYCs retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all KYCs");
                return new ApiResponse<GetAllKycsDto>(false, "Error occurred while processing your request", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<KycResponseDto>> GetKycById(string kycId)
        {
            try
            {
                var kyc = await _unitOfWork.KycRepository.GetKycByIdAsync(kycId);
                if (kyc == null)
                {
                    return ApiResponse<KycResponseDto>.Failed(false, "KYC not found.", StatusCodes.Status404NotFound, null);
                }
                var kycDto = _mapper.Map<KycResponseDto>(kyc);
                return ApiResponse<KycResponseDto>.Success(kycDto, "KYC retrieved successfully.", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting KYC: {ex}");
                return ApiResponse<KycResponseDto>.Failed(false, "Error getting KYC.", StatusCodes.Status500InternalServerError, null);
            }
        }

        public async Task<ApiResponse<KycResponseDto>> UpdateKyc(string kycId, KycRequestDto kycRequest)
        {
            try
            {
                var existingKyc = await _unitOfWork.KycRepository.GetKycByIdAsync(kycId);
                if (existingKyc == null)
                {
                    return ApiResponse<KycResponseDto>.Failed(false, "KYC not found.", StatusCodes.Status404NotFound, null);
                }
                var kyc = _mapper.Map(kycRequest, existingKyc);
                _unitOfWork.KycRepository.UpdateKyc(existingKyc);
                _unitOfWork.SaveChanges();
                var response = _mapper.Map<KycResponseDto>(kyc);
                return ApiResponse<KycResponseDto>.Success(response, "KYC updated successfully.", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating KYC");
                return new ApiResponse<KycResponseDto>(false, "Error occurred while processing your request", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
    }
}