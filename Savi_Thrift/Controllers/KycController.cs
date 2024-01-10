using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly IKycService _kycService;
        public KycController(IKycService kycService)
        {
            _kycService = kycService;
        }

        [HttpPost]
        public async Task<IActionResult> AddKyc(string userId, [FromBody] KycRequestDto kycRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _kycService.AddKyc(userId, kycRequestDto));   
        }
    }
}
