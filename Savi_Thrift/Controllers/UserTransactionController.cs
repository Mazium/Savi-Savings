using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTransactionController : ControllerBase
    {
        private readonly IUserTransactionServices _userTransactionServices;
        public UserTransactionController(IUserTransactionServices userTransactionServices)
        {
             _userTransactionServices = userTransactionServices;
        }


        [HttpGet("get-all-recentTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _userTransactionServices.GetRecentTransactions();
            return result.Data.Count < 1 ? NotFound() : Ok(result);
        }
    }
}

