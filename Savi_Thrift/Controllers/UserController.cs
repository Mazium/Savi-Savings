using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Application.ServicesImplementation;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
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


		[HttpGet("all-new-Users")]
		public async Task<IActionResult> GetAllNewUsers()
		{
			var newUsers = await _userService.GetNewUsers();
			if (newUsers == null)
				return NotFound(newUsers);
			return Ok(newUsers);
		}
		[HttpGet("getUserById")]
		public async Task<IActionResult> GetUserById(string userId)
		{
			return Ok(await _userService.GetUserById(userId));
		}

		[HttpGet("dashboard-user-data")]
		public async Task<IActionResult> GetDashboardUserData()
		{
			return Ok(await _userService.AdminDashboardUserInfo());
		}

		[HttpPatch("update-photo")]
		public async Task<IActionResult> UpdatePhoto([FromForm] UpdatePhotoDto updatePhotoDto)
		{
			return Ok(await _userService.UpdatePhoto(updatePhotoDto));
		}
	}


}
