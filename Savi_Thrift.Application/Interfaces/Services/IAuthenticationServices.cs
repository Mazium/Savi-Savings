using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;


namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IAuthenticationServices
    {

        Task<ApiResponse<string>> ValidateTokenAsync(string token);
        ApiResponse<string> ExtractUserIdFromToken(string authToken);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO);
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(AppUserCreateDto appUserCreateDto);
        Task<ApiResponse<string>> ResetPasswordAsync(string email, string token, string newPassword);
        Task<ApiResponse<string>> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        //Task<ApiResponse<string>> SendConfirmationEmail(string email, string link);
        Task<ApiResponse<string>> ConfirmEmail(string userid, string token);
        Task<ApiResponse<string>> VerifyAndAuthenticateUserAsync(string idToken);
    }
}
