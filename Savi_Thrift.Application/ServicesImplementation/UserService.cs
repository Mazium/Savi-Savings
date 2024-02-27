

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
		private readonly ICloudinaryServices<UserService> _cloudinaryServices;
		public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryServices<UserService> cloudinaryServices)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_cloudinaryServices = cloudinaryServices;
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
			if (user == null)
			{
				return ApiResponse<bool>.Failed("User not found", StatusCodes.Status404NotFound, new List<string>());

			}
			else
			{
				user.IsDeleted = true;
				_unitOfWork.UserRepository.Update(user);
				await _unitOfWork.SaveChangesAsync();
				return ApiResponse<bool>.Success(true, "User deleted successfully", StatusCodes.Status200OK);

			}
		}

		public async Task<ApiResponse<string>> UpdatePhoto(UpdatePhotoDto updatePhotoDto)
		{
			if(updatePhotoDto.ImageUrl == null)
			{
				return ApiResponse<string>.Failed("No file selected. Please select an image", StatusCodes.Status200OK, new List<string>());
			}
			if (updatePhotoDto.UserId == null)
			{
				return ApiResponse<string>.Failed("No user.", StatusCodes.Status200OK, new List<string>());
			}
			var user = await _unitOfWork.UserRepository.GetByIdAsync(updatePhotoDto.UserId);
			if (user == null)
			{
				return ApiResponse<string>.Failed("User not found", StatusCodes.Status200OK, new List<string>());

			}

			var img = await _cloudinaryServices.UploadImage(updatePhotoDto.ImageUrl);
			user.ImageUrl= img.Url;
			_unitOfWork.UserRepository.Update(user);
			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<string>.Success(img.Url, "User photo updated successfully", StatusCodes.Status200OK);


		}

		public async Task<ApiResponse<List<NewUserResponseDto>>> GetNewUsers()
		{
			var newUsers = await _unitOfWork.UserRepository.GetNewUsers();
			var users = new List<NewUserResponseDto>();
			foreach (var user in newUsers)
			{
				var registerResponseDto = new NewUserResponseDto
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					DateModified = user.DateModified,
				};

				users.Add(registerResponseDto);
			}
			return ApiResponse<List<NewUserResponseDto>>.Success(users, "List of New Users", StatusCodes.Status200OK);
		}

	}
}
