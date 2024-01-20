

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UserService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<RegisterResponseDto>>> GetUsers()
		{
			var users = await _unitOfWork.UserRepository.GetAllAsync();
			List<RegisterResponseDto> result = new();
			foreach (var user in users)
			{
				var reponseDto = _mapper.Map<RegisterResponseDto>(user);
				result.Add(reponseDto);
			}
			return new ApiResponse<List<RegisterResponseDto>>(result, "Users retrieved successfully");
		}

		public async Task<ApiResponse<bool>> DeleteUser(string id)
		{
			var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
			if(user == null)
			{
				return ApiResponse<bool>.Failed("User not found", StatusCodes.Status404NotFound, new List<string>());

            }
			else
			{
                 _unitOfWork.UserRepository.DeleteAsync(user);
				return ApiResponse<bool>.Success(true, "User deleted successfully", StatusCodes.Status200OK);

            }
		}

	}
}
