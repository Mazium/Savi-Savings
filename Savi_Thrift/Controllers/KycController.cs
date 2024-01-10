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

        [HttpDelete("{kycId}")]
        public async Task<IActionResult> DeleteKyc(string kycId)
        {
            return Ok(await _kycService.DeleteKycById(kycId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKycs([FromQuery] int page, [FromQuery] int perPage)
        {
            return Ok(await _kycService.GetAllKycs(page, perPage));
        }

        [HttpGet("{kycId}")]
        public async Task<IActionResult> GetKycById(string kycId)
        {
            return Ok(await _kycService.GetKycById(kycId));
        }

        [HttpPut("{kycId}")]
        public async Task<IActionResult> UpdateKyc(string kycId, [FromBody] KycRequestDto updatedKyc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _kycService.UpdateKyc(kycId, updatedKyc));
        }

        [HttpPost("{kycId}/upload-identification-document")]
        public async Task<IActionResult> UploadIdentificationDocument(string kycId, [FromForm] IFormFile file)
        {
            return Ok(await _kycService.UploadIdentificationDocument(kycId, file));
        }

        [HttpPost("{kycId}/upload-proof-of-address-document")]
        public async Task<IActionResult> UploadProofOfAddressDocument(string kycId, [FromForm] IFormFile file)
        {
            return Ok(await _kycService.UploadProofOfAddressDocument(kycId, file));
        }
    }
}
