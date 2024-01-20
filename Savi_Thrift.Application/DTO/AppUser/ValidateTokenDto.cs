using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.AppUser
{
    public class ValidateTokenDto
    {
        [Required]
        public string Token { get; set; }
    }
}
