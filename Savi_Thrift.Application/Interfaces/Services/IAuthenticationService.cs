using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {

        Task<ApiResponse<string>> ValidateTokenAsync(string token);
        ApiResponse<string> ExtractUserIdFromToken(string authToken);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO);
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(AppUserCreateDto appUserCreateDto);


	}
}
