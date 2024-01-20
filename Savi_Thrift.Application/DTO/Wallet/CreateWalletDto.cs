

using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Application.DTO.Wallet
{
    public class CreateWalletDto
    {
        public string PhoneNumber { get; set; } = string.Empty;

        [ForeignKey("AppUserId")]
        public string UserId { get; set; } = string.Empty;
    }
}
