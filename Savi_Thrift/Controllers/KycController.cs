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
        public KycController(IKycService kycService) { _kycService = kycService; }

        [HttpPost("AddKyc")]
        public async Task<IActionResult> AddKyc(string userId, [FromForm] KycRequestDto kycRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _kycService.AddKyc(userId, kycRequestDto));   
        }

        [HttpDelete("{kycId}")]
        public async Task<IActionResult> DeleteKyc(string kycId) 
            => Ok(await _kycService.DeleteKycById(kycId));

        [HttpGet("get-kycs")]
        public async Task<IActionResult> GetAllKycs([FromQuery] int page, [FromQuery] int perPage)
            => Ok(await _kycService.GetAllKycs(page, perPage));

        [HttpGet("{kycId}")]
        public async Task<IActionResult> GetKycById(string kycId) 
            => Ok(await _kycService.GetKycById(kycId));

        [HttpPut("{kycId}")]
        public async Task<IActionResult> UpdateKyc(string kycId, [FromBody] KycRequestDto updatedKyc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _kycService.UpdateKyc(kycId, updatedKyc));
        }
    }
}
