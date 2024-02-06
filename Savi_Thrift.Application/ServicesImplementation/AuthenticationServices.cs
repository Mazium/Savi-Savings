using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Entities.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Savi_Thrift.Domain;
using System.Web;
using Google.Apis.Auth;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class AuthenticationServices : IAuthenticationServices
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ILogger<AuthenticationServices> _logger;
		private readonly IConfiguration _config;
		private readonly IWalletService _walletService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailServices _emailServices;

		public AuthenticationServices(IEmailServices emailServices, IUnitOfWork unitOfWork, IWalletService walletService, IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AuthenticationServices> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_config = config;
			_walletService = walletService;
			_unitOfWork = unitOfWork;
			_emailServices = emailServices;
		}

        public async Task<ApiResponse<string>> VerifyAndAuthenticateUserAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings());
                var userId = payload.Subject;
                var userEmail = payload.Email;
                var userName = payload.Name;
                var firstName = payload.GivenName;
                var lastName = payload.FamilyName;
                var existingUser = await _userManager.FindByEmailAsync(userEmail);

                if (existingUser == null)
                {
                    var newUser = new AppUser
                    {
                        Email = userEmail,
                        UserName = userEmail,
                        FirstName = firstName,
                        LastName = lastName,
                    };

                    var result = await _userManager.CreateAsync(newUser);

                    if (result.Succeeded)
                    {
                  
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return ApiResponse<string>.Success("","User created and authenticated successfully on the server side", StatusCodes.Status200OK);
                    }
                    else
                    {
                      
                        return ApiResponse<string>.Failed("User creation failed", StatusCodes.Status400BadRequest, new List<string>());
                    }
                }
                else
                {
                
                    await _signInManager.SignInAsync(existingUser, isPersistent: false);

                    return ApiResponse<string>.Success("","User authenticated successfully on the server side", StatusCodes.Status200OK);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password");
                return ApiResponse<string>.Failed("Error occurred while authenticating user", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(AppUserCreateDto appUserCreateDto)
        {
            var user = await _userManager.FindByEmailAsync(appUserCreateDto.Email);
            if (user != null)
            {
                return ApiResponse<RegisterResponseDto>.Failed("User with this email already exists.", StatusCodes.Status400BadRequest, new List<string>());
            }

            var userr = await _unitOfWork.UserRepository.FindAsync(x => x.PhoneNumber == appUserCreateDto.PhoneNumber);
            if (userr.Count > 0)
            {
                return ApiResponse<RegisterResponseDto>.Failed("User with this phone number already exists.", StatusCodes.Status400BadRequest, new List<string>());
            }

            var appUser = new AppUser()
            {
                FirstName = appUserCreateDto.FirstName,
                LastName = appUserCreateDto.LastName,
                Email = appUserCreateDto.Email,
                PhoneNumber = appUserCreateDto.PhoneNumber,
                UserName = appUserCreateDto.Email,
                PasswordResetToken = ""
            };

            try
            {
                var token = "";

                var result = await _userManager.CreateAsync(appUser, appUserCreateDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "User");
                    token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    token = HttpUtility.UrlEncode(token);
                    var createWalletDto = new CreateWalletDto
                    {
                        PhoneNumber = appUser.PhoneNumber,
                        UserId = appUser.Id
                    };

                    var walletCreated = await _walletService.CreateWallet(createWalletDto);
                    if (walletCreated.Succeeded)
                    {
                        var response = new RegisterResponseDto
                        {
                            Id = appUser.Id,
                            Email = appUser.Email,
                            PhoneNumber = appUser.PhoneNumber,
                            FirstName = appUser.FirstName,
                            LastName = appUser.LastName,
                            Token = token
                        };

                        //await _emailServices.SendEmailAsync(new MailRequest
                        //{
                        //    ToEmail = appUser.Email,
                        //    Subject = "Confirm Your Email",
                        //    Body = "Thank you for registering. ",
                        //}, confirmationLink);

                        return ApiResponse<RegisterResponseDto>.Success(response, "User registered successfully. Please click on the link sent to your email to confirm your account", StatusCodes.Status201Created);
                    }
                    else
                    {
                        _unitOfWork.UserRepository.DeleteAsync(appUser);
                        return ApiResponse<RegisterResponseDto>.Failed(walletCreated.Message, StatusCodes.Status201Created, new List<string>());
                    }
                }
                else
                {
                    return ApiResponse<RegisterResponseDto>.Failed("Error occurred: Failed to create wallet", StatusCodes.Status201Created, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a manager " + ex.InnerException);
                return ApiResponse<RegisterResponseDto>.Failed("Error creating user.", StatusCodes.Status500InternalServerError, new List<string>() { ex.InnerException.ToString() });
            }
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(loginDTO.Email);
				if (user == null)
				{
					return ApiResponse<LoginResponseDto>.Failed("User not found.", StatusCodes.Status404NotFound, new List<string>());
				}
				var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

				switch (result)
				{
					case { Succeeded: true }:
						var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
						var response = new LoginResponseDto
						{
							JWToken = GenerateJwtToken(user, role)
						};
						return ApiResponse<LoginResponseDto>.Success(response, "Logged In Successfully", StatusCodes.Status200OK);

					case { IsLockedOut: true }:
						return ApiResponse<LoginResponseDto>.Failed($"Account is locked out. Please try again later or contact support." +
							$" You can unlock your account after {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} minutes.", StatusCodes.Status403Forbidden, new List<string>());

					case { RequiresTwoFactor: true }:
						return ApiResponse<LoginResponseDto>.Failed("Two-factor authentication is required.", StatusCodes.Status401Unauthorized, new List<string>());

					case { IsNotAllowed: true }:
						return ApiResponse<LoginResponseDto>.Failed("Login failed. Email confirmation is required.", StatusCodes.Status401Unauthorized, new List<string>());

					default:
						return ApiResponse<LoginResponseDto>.Failed("Login failed. Invalid email or password.", StatusCodes.Status401Unauthorized, new List<string>());
				}
			}
			catch (Exception ex)
			{
				return ApiResponse<LoginResponseDto>.Failed("Some error occurred while loggin in." + ex.InnerException, StatusCodes.Status500InternalServerError, new List<string>());
			}
		}
		private string GenerateJwtToken(AppUser contact, string roles)
		{
			var jwtSettings = _config.GetSection("JwtSettings:Secret").Value;
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, contact.Id),
				new Claim(JwtRegisteredClaimNames.Email, contact.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.GivenName, contact.FirstName+" "+contact.LastName),
				new Claim(ClaimTypes.Role, roles)
			};

			var token = new JwtSecurityToken(
                issuer: _config.GetValue<string>("JwtSettings:ValidIssuer"),
                audience: _config.GetValue<string>("JwtSettings:ValidAudience"),
                //issuer: null,
				//audience: null,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JwtSettings:AccessTokenExpiration").Value)),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<ApiResponse<string>> ValidateTokenAsync(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Secret").Value);

				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = _config.GetSection("JwtSettings:ValidIssuer").Value,
					ValidAudience = _config.GetSection("JwtSettings:ValidAudience").Value,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

				return new ApiResponse<string>(true, "Token is valid.", 200, null, new List<string>());
			}
			catch (SecurityTokenException ex)
			{
				_logger.LogError(ex, "Token validation failed");
				var errorList = new List<string> { ex.Message };
				return new ApiResponse<string>(false, "Token validation failed.", 400, null, errorList);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred during token validation");
				var errorList = new List<string> { ex.Message };
				return new ApiResponse<string>(false, "Error occurred during token validation", 500, null, errorList);
			}
		}

		public ApiResponse<string> ExtractUserIdFromToken(string authToken)
		{
			try
			{
				var token = authToken.Replace("Bearer ", "");

				var handler = new JwtSecurityTokenHandler();
				var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

				var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;

				if (string.IsNullOrWhiteSpace(userId))
				{
					return new ApiResponse<string>(false, "Invalid or expired token.", 401, null, new List<string>());
				}

				return new ApiResponse<string>(true, "User ID extracted successfully.", 200, userId, new List<string>());
			}
			catch (Exception ex)
			{
				return new ApiResponse<string>(false, "Error extracting user ID from token.", 500, null, new List<string> { ex.Message });
			}
		}
        public async Task<ApiResponse<string>> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            try
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (result.Succeeded)
                {
                    return new ApiResponse<string>(true, "Password changed successfully.", 200, null, new List<string>());
                }
                else
                {
                    return new ApiResponse<string>(false, "Password change failed.", 400, null, result.Errors.Select(error => error.Description).ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password");
                var errorList = new List<string> { ex.Message };
                return new ApiResponse<string>(true, "Error occurred while changing password", 500, null, errorList);
            }
        }
        public async Task<ApiResponse<string>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return new ApiResponse<string>(false, "User not found.", 404, null, new List<string>());
                }

                // Additional token validation logic can be added here

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                {
                    // Update user properties if needed
                    user.PasswordResetToken = null;
                    user.ResetTokenExpires = null;
                    await _userManager.UpdateAsync(user);

                    return new ApiResponse<string>(true, "Password reset successful.", 200, null, new List<string>());
                }
                else
                {
                    return new ApiResponse<string>(false, "Password reset failed.", 400, null, result.Errors.Select(error => error.Description).ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting password for user with email {Email}", email);
                var errorList = new List<string> { "An unexpected error occurred while resetting the password." };
                return new ApiResponse<string>(true, "Error occurred while resetting password", 500, null, errorList);
            }
        }

        public async Task<ApiResponse<string>> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null || !user.EmailConfirmed)
                {
                    return new ApiResponse<string>(false, "User not found or email not confirmed.", StatusCodes.Status404NotFound, null, new List<string>());
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
               
                //token = HttpUtility.UrlEncode(token);

                user.PasswordResetToken = token;
                user.ResetTokenExpires = DateTime.UtcNow.AddHours(24);

                await _userManager.UpdateAsync(user);

                var resetPasswordUrl = "https://localhost:7226/api/Authentication/reset-password?email=" +Uri.EscapeDataString(email) + "&token=" + token;
                //var resetPasswordUrl = "https://localhost:7226/api/Authentication/reset-password?email=" + email + "&token=" + token;


                var mailRequest = new MailRequest
                {
                    ToEmail = email,
                    Subject = "Your Savi Password Reset Instructions",
                    Body = $"Please reset your password by clicking <a href='{resetPasswordUrl}'>here</a>."
                };
                await _emailServices.SendMailAsync(mailRequest);

                return new ApiResponse<string>(true, "Password reset email sent successfully.", 200, null, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing forgot password for user with email {Email}", email);
                var errorList = new List<string> { "An unexpected error occurred while processing the forgot password request." };
                return new ApiResponse<string>(true, "Error occurred while processing forgot password", 500, null, errorList);
            }
        }

        public async Task<ApiResponse<string>> ConfirmEmail(string userid, string token)
		{
			if (userid == null || token== null)
			{
				return ApiResponse<string>.Failed("userid or token cannot be null", StatusCodes.Status400BadRequest, new List<string>() { });
			}

			var user = await _userManager.FindByIdAsync(userid);
			if(user == null)
			{
                return ApiResponse<string>.Failed("Invalid User", StatusCodes.Status404NotFound, new List<string>() { });
            }

			var result = await _userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
                return ApiResponse<string>.Success("success", "Email confirmed successfully", StatusCodes.Status200OK);
            }
			else
			{
                return ApiResponse<string>.Failed("Email confirmation failed", StatusCodes.Status500InternalServerError, new List<string>() { });
            }
		}
    }
}
