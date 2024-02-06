using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.Wallet
{
    public class DebitWalletDto
    {
        [Required]
        public string WalletNumber { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public string Narration { get; set; } = string.Empty;
        public int ActionId { get; set; }
    }
}
