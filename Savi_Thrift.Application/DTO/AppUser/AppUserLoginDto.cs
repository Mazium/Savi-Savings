
using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.AppUser
{
    public class AppUserLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
