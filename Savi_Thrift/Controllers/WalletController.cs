using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class WalletController : ControllerBase
	{
		private readonly IWalletService _walletService;
		public WalletController(IWalletService walletService) { 
		_walletService = walletService;
		}


		[HttpGet("GetAllWallets")]
		public async Task<IActionResult> AllWallets()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest( ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			return Ok(await _walletService.GetAllWallets());
		}

		[HttpGet("GetWalletByNumber")]
		public async Task<IActionResult> GetWalletByNumber(string number)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			return Ok(await _walletService.GetWalletByNumber(number));
		}


		[HttpPost("FundWallet")]
		public async Task<IActionResult> FundWallet(FundWalletDto fundWalletDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
			}

			return Ok(await _walletService.FundWallet(fundWalletDto));
		}

        [HttpPost("DebitWallet")]
        public async Task<IActionResult> DebitWallet(DebitWalletDto debitWalletDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Failed("Wallet debit failed. This operation is not supported.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));

            }

            return Ok(await _walletService.DebitWallet(debitWalletDto));
        }
	




        //[HttpPost]
        //public async Task<IActionResult> CreateWallet(CreateWalletDto createWalletDto)
        //{
        //	if (!ModelState.IsValid)
        //	{
        //		return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
        //	}

        //	return Ok(await _walletService.CreateWallet(createWalletDto));
        //}
    }
}
