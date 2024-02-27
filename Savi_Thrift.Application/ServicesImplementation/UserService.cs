

using AutoMapper;
using Microsoft.AspNetCore.Http;
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
		public UserService(IUnitOfWork unitOfWork, IMapper mapper, IGroupSavingsService groupSavingsService, 
			IDefaultingUserService defaultingUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _groupSavingsService = groupSavingsService;
            _defaultingUserService = defaultingUserService;
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
        public async Task<ApiResponse<List<NewUserResponseDto>>> GetNewUsers()
        {
            DateTime today = DateTime.Today;
            var newUsers = await _unitOfWork.UserRepository.FindAsync(u => u.IsDeleted == false && u.CreatedAt >= today && u.CreatedAt < today.AddDays(1));
            var users = _mapper.Map<List<NewUserResponseDto>>(newUsers);          
            return ApiResponse<List<NewUserResponseDto>>.Success(users, "List of New Users", StatusCodes.Status200OK);
        }
        public async Task<ApiResponse<int[]>> AdminDashboardUserInfo()
        {
			try
			{
                var response = await GetNewUsers();
                var data = response.Data.Count;

                var groupResponse = await _groupSavingsService.GetRecentGroup();
                var groupData = groupResponse.Data.Count;

                var defaultUserResponse = await _defaultingUserService.GetTodayDefaultingUsers();
                var defaultData = defaultUserResponse.Data.Count;

                var dashboardUserData = new int[] { data, groupData, defaultData };

                return ApiResponse<int[]>.Success(dashboardUserData, "Dashboard User Data Retrieved Successfully", StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return ApiResponse<int[]>.Failed(new List<string>(){ex.Message});

            }

        }

    }
}
