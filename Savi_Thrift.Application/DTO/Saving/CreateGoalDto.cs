using Microsoft.AspNetCore.Http;
using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.Saving
{
    public class CreateGoalDto
    {
		[Required]
		public string Title { get; set; }
		[Required]
		public decimal GoalAmount { get; set; }
		public IFormFile Avatar { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime TargetDate { get; set; }
		public bool IsAutomatic { get; set; }
		public decimal AmountToAdd { get; set; }
		public SavingFrequency Frequency { get; set; }
		public string WalletNumber { get; set; }
	}
}
