
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.Interfaces.Services;
using TicketEase.Domain;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
		public AuthenticationController(IAuthenticationService authenticationService) => _authenticationService = authenticationService;

		[HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AppUserCreateDto appUserCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest( ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }
            return Ok(await _authenticationService.RegisterAsync(appUserCreateDto));
        }

		[HttpPost("Login")]
		public async Task<IActionResult> Login(AppUserLoginDto loginDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest( ApiResponse<string>.Failed("Invalid model state.", 400, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}
			return Ok(await _authenticationService.LoginAsync(loginDTO));
		}
	}
}
