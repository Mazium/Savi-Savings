using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Common.Utilities
{
	public static class WalletGenerator
	{
		public static string SetWalletID(string phoneNumber)
		{
			if (phoneNumber.StartsWith("+234"))
			{
				return phoneNumber.Substring(4);
			}
			else if (phoneNumber.StartsWith("0"))
			{
				return phoneNumber.Substring(1);
			}
			else
			{
				if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out long walletNumber))
				{
					return walletNumber.ToString();
				}
				else
				{
					throw new Exception("Invalid Phone Number Format");
				}
			}

		}
	}
}
