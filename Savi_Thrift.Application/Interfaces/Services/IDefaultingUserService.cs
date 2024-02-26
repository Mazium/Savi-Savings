using Savi_Thrift.Application.DTO.DefaultUser;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
	public interface IDefaultingUserService
	{
		Task<ApiResponse<List<DefaultUserDto>>> GetDefaultingUsers(string groupSavingsId);
		Task<ApiResponse<List<DefaultUserDto>>> GetAllDefaultingUsers();
	}
}
