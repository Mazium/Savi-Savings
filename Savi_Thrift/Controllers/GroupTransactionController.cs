using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Application.ServicesImplementation;

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

        [HttpGet("group-recent-transaction")]
        public async Task<IActionResult> GetGroupRecentTranaction()
        {
            var transaction = await _groupTransactionService.GetGroupRecentTransaction();
            if(transaction != null) 
                return Ok(transaction);
            return BadRequest(transaction);
        }
    }
}
