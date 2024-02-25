using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.Interfaces.Services
{
	public interface IRecurringGroupJobs
	{
		Task<string> FundNow(string groupId);
	}
}
