using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.Saving;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SavingController : ControllerBase
	{
		private readonly ISavingService _savingService;
		public SavingController(ISavingService savingService) {
			_savingService = savingService;
		}


        [HttpGet("AllPersonalSavings")]
        public async Task<IActionResult> GetAllPersonalSavings()
        {
            // No ModelState validation is performed for this action.

            var apiResponse = await _savingService.ViewGoals();

            if (apiResponse.Data != null)
            {
                return Ok(apiResponse);
            }
            else
            {
                return NotFound(apiResponse.Message);
            }
        }


        [HttpPost("createPersonalSaving")]
		public async Task<IActionResult> CreateGoal(CreateGoalDto createGoalDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			return Ok(await _savingService.CreateGoal(createGoalDto));
		}

		[HttpGet("list/{UserId}")]
		public async Task<IActionResult> GetAllGoals(string UserId)
		{
			var response = await _savingService.GetListOfAllUserGoals(UserId);
			if (response.StatusCode == 200)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}

        [HttpPost("credit")]
        public async Task<IActionResult> CreditPersonalSavings(CreditSavingsDto creditDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }

            var response = await _savingService.CreditPersonalSavings(creditDto);

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }

            return BadRequest(ApiResponse<string>.Failed($"Failed to credit personal savings: {response.Message}", response.StatusCode, response.Errors));
        }


        [HttpPost("debit-goal-credit-wallet")]
        public async Task<IActionResult> DebitGoal(CreditWalletFromGoalDto createGoalDto)
        {
            var response = await _savingService.WithdrawFundsFromGoalToWallet(createGoalDto);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("PersonalSavingDetails")]
        public async Task<IActionResult> GetPersonalSaving(string Id)
        {
            var response = await _savingService.GetPersonalSavings(Id);

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("fund-saving(goal)")]
        public async Task<IActionResult> FundPersonalGoal(FundsPersonalGoalDto personalGoalDto)
        {
            var response = await _savingService.FundsPersonalGoal(personalGoalDto);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
