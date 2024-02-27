using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IUserService
	{
		Task<ApiResponse<List<RegisterResponseDto>>> GetUsers();
        Task<ApiResponse<bool>> DeleteUser(string id);
        Task<ApiResponse<List<NewUserResponseDto>>> GetNewUsers();

		Task<ApiResponse<string>> UpdatePhoto(UpdatePhotoDto updatePhotoDto);

	}
}
