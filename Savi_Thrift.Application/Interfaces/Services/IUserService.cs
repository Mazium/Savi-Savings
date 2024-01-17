using Savi_Thrift.Application.DTO.AppUser;
using TicketEase.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IUserService
	{
		Task<ApiResponse<List<RegisterResponseDto>>> GetUsers();
	}
}
