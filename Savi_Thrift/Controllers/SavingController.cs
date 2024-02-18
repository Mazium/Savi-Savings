using Hangfire;
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
        private readonly IBackgroundJobClient _backgroundJobClient;
		public SavingController(IBackgroundJobClient backgroundJobClient, ISavingService savingService) 
        {
			_savingService = savingService;
            _backgroundJobClient = backgroundJobClient;
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
		public async Task<IActionResult> CreateGoal([FromForm] CreateGoalDto createGoalDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state my friend.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			return Ok(await _savingService.CreateGoal(createGoalDto));
		}

		[HttpGet("list/{walletNumber}")]
		public async Task<IActionResult> GetAllGoals(string walletNumber)
		{
			var response = await _savingService.GetListOfAllUserGoals(walletNumber);
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
        public async Task<IActionResult> GetPersonalSaving(string savingsId)
        {
            var response = await _savingService.GetPersonalSavings(savingsId);

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

		[HttpGet("GetTotalSavingBalance")]
		public async Task<IActionResult> GetTotalSavingBalance(string walletNumber)
		{
			var response = await _savingService.GetTotalSavingBalance(walletNumber);
			return Ok(response);
		}

		[HttpPost("fund-personal-saving")]
        public async Task<IActionResult> FundPersonalGoal(FundsPersonalGoalDto personalGoalDto)
        {
             _backgroundJobClient.Schedule<ISavingService>(service => service
           .FundsPersonalGoal(personalGoalDto), TimeSpan.FromMinutes(1));
            //var response = await _savingService.FundsPersonalGoal(personalGoalDto);
            //if (response.StatusCode == 200)
            //{
            //    return Ok(response);
            //}
            //return BadRequest(response);
            return Ok();
        }
    }
}
