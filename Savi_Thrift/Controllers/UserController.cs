using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.Interfaces.Services;
using TicketEase.Domain;

namespace Savi_Thrift.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService) {
			_userService = userService;
		}


		[HttpGet("AllUsers")]
		public async Task<IActionResult> AllUser()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			return Ok(await _userService.GetUsers());
		}

	}
}
