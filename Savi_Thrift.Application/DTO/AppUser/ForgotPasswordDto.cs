using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.AppUser
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
