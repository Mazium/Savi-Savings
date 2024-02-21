
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationServices _authenticationService;
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly IConfiguration _configuration;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailServices _emailServices;
		private readonly IUserService _userService;

		public AuthenticationController(IBackgroundJobClient backgroundJobClient, IConfiguration configuration, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUserService userService, IEmailServices emailService, IAuthenticationServices authenticationService)
		{
			_authenticationService = authenticationService;
			_emailServices = emailService;
			_userService = userService;
			_userManager = userManager;
			_backgroundJobClient = backgroundJobClient;
			_configuration = configuration;
			_signInManager = signInManager;

		}

		[HttpPost("signin-google/{token}")]
		public async Task<IActionResult> GoogleAuth([FromRoute] string token)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(new ApiResponse<string>(false, "Invalid model state.", StatusCodes.Status400BadRequest, "", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}
			return Ok(await _authenticationService.VerifyAndAuthenticateUserAsync(token));
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] AppUserCreateDto appUserCreateDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			// Call registration service
			var registrationResult = await _authenticationService.RegisterAsync(appUserCreateDto);



			if (registrationResult.Succeeded)
			{
				var data = registrationResult.Data;
				//  _backgroundJobClient.Enqueue(() => Console.WriteLine(data));
				var confirmationLink = GenerateConfirmEmailLink(data.Id, data.Token);
				return Ok(await _emailServices.SendEmailAsync(confirmationLink, data.Email, data.Id));
			}
			else
			{
				return BadRequest(new { Message = registrationResult.Message, Errors = registrationResult.Errors });
			}
		}


		[HttpPost("Login")]
		public async Task<IActionResult> Login(AppUserLoginDto loginDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state.", 400, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}
			return Ok(await _authenticationService.LoginAsync(loginDTO));
		}


		private static string GenerateConfirmEmailLink(string id, string token)
		{
			return "http://localhost:3000/email-confirmed?UserId=" + id + "&token=" + token;
		}



		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail(string userid, string token)
		{
			return Ok(await _authenticationService.ConfirmEmail(userid, token));
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(new ApiResponse<string>(false, "Invalid model state.", 400, null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			var response = await _authenticationService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);

			if (response.Succeeded)
			{
				return Ok(new ApiResponse<string>(true, response.Message, response.StatusCode, null, new List<string>()));
			}
			else
			{
				return BadRequest(new ApiResponse<string>(false, response.Message, response.StatusCode, null, response.Errors));
			}


		}
		[HttpPost("validate-token")]
		public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new ApiResponse<string>(false, "Invalid model state.", 400, null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			var response = await _authenticationService.ValidateTokenAsync(model.Token);

			if (response.Succeeded)
			{
				return Ok(new ApiResponse<string>(true, response.Message, response.StatusCode, null, new List<string>()));
			}
			else
			{
				return BadRequest(new ApiResponse<string>(false, response.Message, response.StatusCode, null, response.Errors));
			}
		}

		[HttpPost("update-password")]
		public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto model, [FromHeader(Name = "Authorization")] string authToken)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new ApiResponse<string>(false, "Invalid model state.", 400, null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			if (string.IsNullOrWhiteSpace(authToken))
			{
				return Unauthorized(new ApiResponse<string>(false, "Authorization token is missing.", 401, null, new List<string>()));
			}

			var userIdResponse = _authenticationService.ExtractUserIdFromToken(authToken);

			if (!userIdResponse.Succeeded)
			{
				return Unauthorized(userIdResponse);
			}
			var userId = userIdResponse.Data;

			var user = await _userManager.FindByEmailAsync(userId);

			if (user == null)
			{
				return Unauthorized(new ApiResponse<string>(false, "User not found.", 401, null, new List<string>()));
			}

			var response = await _authenticationService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

			if (response.Succeeded)
			{
				return Ok(new ApiResponse<string>(true, response.Message, response.StatusCode, null, new List<string>()));
			}
			else
			{
				return BadRequest(new ApiResponse<string>(false, response.Message, response.StatusCode, null, response.Errors));
			}
		}
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			return Ok(new ApiResponse<string>(true, "Logout successful", 200, null, new List<string>()));
		}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new ApiResponse<string>(false, "Invalid model state.", 400, null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			var response = await _authenticationService.ForgotPasswordAsync(model.Email);

			if (response.Succeeded)
			{
				return Ok(new ApiResponse<string>(true, response.Message, response.StatusCode, null, new List<string>()));
			}
			else
			{
				return BadRequest(new ApiResponse<string>(false, response.Message, response.StatusCode, null, response.Errors));
			}
		}
	}
}
