

using Hangfire;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class RecurringGroupJobs : IRecurringGroupJobs
	{
		private readonly IGroupTransactionService _groupTransactionService;
        public RecurringGroupJobs(IGroupTransactionService groupTransactionService)
        {
			_groupTransactionService = groupTransactionService;
        }

        public async Task<string> FundNow(string groupId)
		{
			await _groupTransactionService.AutoFundGroup(groupId);


			//string cronExpression = $"0 {scheduledTime.Minute} {scheduledTime.Hour} * * *";
			//RecurringJob.AddOrUpdate<IRecurringGroupJobs>("1. Recurring job for group " + group.GroupName, (jobs) => jobs.FundNow(group.Id), cronExpression);


			return "success";
		}
	}
}
