

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IGroupSavingsService	_groupSavingsService;
		private readonly IDefaultingUserService _defaultingUserService;
		private readonly ICloudinaryServices<UserService> _cloudinaryServices;
		private readonly ISavingService _savingService;
		public UserService(IUnitOfWork unitOfWork, IMapper mapper, IGroupSavingsService groupSavingsService, 
			IDefaultingUserService defaultingUserService, ICloudinaryServices<UserService> cloudinaryServices, ISavingService savingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _groupSavingsService = groupSavingsService;
            _defaultingUserService = defaultingUserService;
			_cloudinaryServices = cloudinaryServices;
			 _savingService = savingService;
        }

        public async Task<ApiResponse<List<RegisterResponseDto>>> GetUsers()
		{
			var users = await _unitOfWork.UserRepository.GetAllAsync();
			var result = _mapper.Map<List<RegisterResponseDto>>(users);
			return new ApiResponse<List<RegisterResponseDto>>(result, "Users retrieved successfully");
		}
		public async Task<ApiResponse<NewUserResponseDto>> GetUserById(string userId)
		{
			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user == null)
			{
				return ApiResponse< NewUserResponseDto>.Failed(new List<string>() {"User id does not exits" });
			}
			var userdata = _mapper.Map<NewUserResponseDto>(user);
			return new ApiResponse<NewUserResponseDto>(userdata, "User information retrieved successfully");
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
            DateTime today = DateTime.Today;
            var newUsers = await _unitOfWork.UserRepository.FindAsync(u => u.IsDeleted == false && u.CreatedAt >= today && u.CreatedAt < today.AddDays(1));
            var users = _mapper.Map<List<NewUserResponseDto>>(newUsers);          
            return ApiResponse<List<NewUserResponseDto>>.Success(users, "List of New Users", StatusCodes.Status200OK);
        }
        public async Task<ApiResponse<decimal[]>> AdminDashboardUserInfo()
        {
			try
			{
                var response = await GetNewUsers();
                var data = response.Data.Count;

                var groupResponse = await _groupSavingsService.GetRecentGroup();
                var groupData = groupResponse.Data.Count;

                var defaultUserResponse = await _defaultingUserService.GetTodayDefaultingUsers();
                var defaultData = defaultUserResponse.Data.Count;

				var groupResponse2 = await _groupSavingsService.GetAllGroups();
				var totalG = groupResponse2.Data.Count;

				var resp = await _savingService.GetAllUsersSavingBalance();
				var totpersonal = resp.Data;

				var monthly = await _savingService.GetMonthlySavingBalancePercentage();
				var totmonthlybalance = monthly.Data;

				var monthlygroupresponse = await _savingService.GetMonthlyGroupCreationPercentage();
				var monthlygroupcreation = monthlygroupresponse.Data;

				var dashboardUserData = new decimal[] { data, groupData, defaultData, totalG, totpersonal, totmonthlybalance, monthlygroupcreation };

                return ApiResponse<decimal[]>.Success(dashboardUserData, "Dashboard User Data Retrieved Successfully", StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return ApiResponse<decimal[]>.Failed(new List<string>(){ex.Message});

            }

        }

	}
}
