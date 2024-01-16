using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupResponseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public bool? IsActive { get; set; }
        public decimal? ContributionAmount { get; set; }
        public bool? IsOpen { get; set; }
        public int? MaxNumberOfParticipants { get; set; }
		public string AvailableSlots { get; set; }
		public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SavingFrequency? SavingFrequency { get; set; }
    }
}
