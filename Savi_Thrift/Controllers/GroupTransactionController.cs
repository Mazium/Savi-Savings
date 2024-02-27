using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupTransactionController : ControllerBase
    {
        private readonly IGroupTransactionService _groupTransactionService;

        public GroupTransactionController(IGroupTransactionService groupTransactionService)
        {
           _groupTransactionService = groupTransactionService;
        }

        [HttpPost("fund-group")]
        public async Task<IActionResult> FundGroup([FromBody] GroupFundDto groupFundDto)
        {
            return Ok(await _groupTransactionService.FundGroup(groupFundDto));
        }

		[HttpPost("Auto-fund-group")]
		public async Task<IActionResult> AutoFundGroup(string groupId)
		{
			return Ok(await _groupTransactionService.AutoFundGroup(groupId));
		}

		[HttpGet("get-group-transactions")]
		public async Task<IActionResult> GetGroupTransaction(string groupId)
		{
			return Ok(await _groupTransactionService.GetGroupTransactions(groupId));
		}

		[HttpGet("get-group-transactions-by-userId")]
		public async Task<IActionResult> GetGroupTransactionByUserId(string userId)
		{
			return Ok(await _groupTransactionService.GetGroupTransactionsByUserId(userId));
		}

		[HttpGet("get-total-transactions")]
		public async Task<IActionResult> GetTotalGroupSavings(string userId)
		{
			return Ok(await _groupTransactionService.TotalGroupSavings(userId));
		}
	}
}
