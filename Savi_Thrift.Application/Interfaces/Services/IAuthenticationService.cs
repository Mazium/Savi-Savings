using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {

        Task<ApiResponse<string>> ValidateTokenAsync(string token);
        ApiResponse<string> ExtractUserIdFromToken(string authToken);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO);
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(AppUserCreateDto appUserCreateDto);
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(string email, string token, string newPassword);

    }
}
