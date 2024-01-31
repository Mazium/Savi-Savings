using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class Wallet : BaseEntity
	{
		public string WalletNumber { get; set; } = string.Empty;
		public decimal Balance { get; set; }
		public Currency Currency { get; set; }
        public string? Reference { get; set; }
        public string PaystackCustomerCode { get; set; } = string.Empty;
		public string TransactionPin { get; set; } = string.Empty;

		[ForeignKey("AppUserId")]
		public string UserId { get; set; } = string.Empty;
		public ICollection<WalletFunding> WalletFundings { get; set; } = new List<WalletFunding>();

		public void SetWalletID(string phoneNumber)
		{
			if (phoneNumber.StartsWith("+234"))
			{
				WalletNumber = phoneNumber.Substring(4);
			}
			else if (phoneNumber.StartsWith("0"))
			{
				WalletNumber = phoneNumber.Substring(1);
			}
			else
			{
				if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out long walletNumber))
				{
					WalletNumber = walletNumber.ToString();
				}
				else
				{
					throw new Exception("Invalid Phone Number Format");
				}
			}

		}
	}
}
