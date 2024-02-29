

using Hangfire;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using System.Text.RegularExpressions;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class RecurringGroupJobs : IRecurringGroupJobs
	{
		private readonly IGroupTransactionService _groupTransactionService;
		private readonly ISavingService _savingService;
        public RecurringGroupJobs(IGroupTransactionService groupTransactionService, ISavingService savingService)
        {
			_groupTransactionService = groupTransactionService;
			_savingService = savingService;
        }

        public async Task<string> FundNow(string groupId)
		{
			await _groupTransactionService.AutoFundGroup(groupId);
			return "success";
		}

		public async Task<string> AutoFundPersonalSavings(string goalId)
		{
			await _savingService.AutoFundPersonalGoal(goalId);
			return "success";
		}
	}
}
