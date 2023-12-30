//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;
//using Savi_Thrift.Application.DTO;
//using Savi_Thrift.Application.Interfaces.Services;
//using Savi_Thrift.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using TicketEase.Domain;

//namespace Savi_Thrift.Application.ServicesImplementation
//{
//    public class AuthenticationService : IAuthenticationService
//    {
//        private readonly UserManager<AppUser> _userManager;
//        private readonly SignInManager<AppUser> _signInManager;
//        private readonly ILogger _logger;
//        private readonly IConfiguration _config;
//        public AuthenticationService(IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AuthenticationService> logger)
//        {

//            _userManager = userManager;
//            _signInManager = signInManager;
//            _logger = logger;
//            _config = config;

//        }

//        public async Task<ApiResponse<string>> LoginAsync(AppUserDto loginDTO)
//        {
//            try
//            {
//                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
//                if (user == null)
//                {
//                    return new ApiResponse<string>(false, StatusCodes.Status404NotFound, "User not found.");
//                }
//                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

//                switch (result)
//                {
//                    case { Succeeded: true }:
//                        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
//                        return new ApiResponse<string>(true, StatusCodes.Status200OK, GenerateJwtToken(user, role));

//                    case { IsLockedOut: true }:
//                        return new ApiResponse<string>(false, StatusCodes.Status403Forbidden, $"Account is locked out. Please try again later or contact support." +
//                            $" You can unlock your account after {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} minutes.");

//                    case { RequiresTwoFactor: true }:
//                        return new ApiResponse<string>(false, StatusCodes.Status401Unauthorized, "Two-factor authentication is required.");

//                    case { IsNotAllowed: true }:
//                        return new ApiResponse<string>(false, StatusCodes.Status401Unauthorized, "Login failed. Email confirmation is required.");

//                    default:
//                        return new ApiResponse<string>(false, StatusCodes.Status401Unauthorized, "Login failed. Invalid email or password.");
//                }
//            }
//            catch (Exception ex)
//            {
//                return new ApiResponse<string>(false, StatusCodes.Status500InternalServerError, "Some error occurred while loggin in." + ex.InnerException);
//            }
//        }
//        private string GenerateJwtToken(AppUser contact, string roles)
//        {
//            var jwtSettings = _config.GetSection("JwtSettings:Secret").Value;
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings));
//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, contact.UserName),
//                new Claim(JwtRegisteredClaimNames.Email, contact.Email),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(ClaimTypes.Role, roles)
//            };

//            var token = new JwtSecurityToken(
//                issuer: null,
//                audience: null,
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JwtSettings:AccessTokenExpiration").Value)),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        public async Task<ApiResponse<string>> ValidateTokenAsync(string token)
//        {
//            try
//            {
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Secret").Value);

//                var validationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = _config.GetSection("JwtSettings:ValidIssuer").Value,
//                    ValidAudience = _config.GetSection("JwtSettings:ValidAudience").Value,
//                    IssuerSigningKey = new SymmetricSecurityKey(key)
//                };

//                SecurityToken securityToken;
//                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

//                var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

//                return new ApiResponse<string>(true, "Token is valid.", 200, null, new List<string>());
//            }
//            catch (SecurityTokenException ex)
//            {
//                _logger.LogError(ex, "Token validation failed");
//                var errorList = new List<string> { ex.Message };
//                return new ApiResponse<string>(false, "Token validation failed.", 400, null, errorList);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred during token validation");
//                var errorList = new List<string> { ex.Message };
//                return new ApiResponse<string>(false, "Error occurred during token validation", 500, null, errorList);
//            }
//        }

//        public ApiResponse<string> ExtractUserIdFromToken(string authToken)
//        {
//            try
//            {
//                var token = authToken.Replace("Bearer ", "");

//                var handler = new JwtSecurityTokenHandler();
//                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

//                var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;

//                if (string.IsNullOrWhiteSpace(userId))
//                {
//                    return new ApiResponse<string>(false, "Invalid or expired token.", 401, null, new List<string>());
//                }

//                return new ApiResponse<string>(true, "User ID extracted successfully.", 200, userId, new List<string>());
//            }
//            catch (Exception ex)
//            {
//                return new ApiResponse<string>(false, "Error extracting user ID from token.", 500, null, new List<string> { ex.Message });
//            }
//        }
//    }
//}
