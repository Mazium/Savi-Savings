using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupResponseDto
    {
		public string Id { get; set; }
		public string GroupName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public SavingFrequency Frequency { get; set; }
        public int MembersMaximumCount { get; set; }
        public DateTime RunTime { get; set; }
        public string PurposeAndGoal { get; set; }
        public string TermsAndConditions { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public string SafePortraitImageURL { get; set; }
        public string SafeLandScapeImageURL { get; set; }
        public DateTime NextRunTime { get; set; }
        public bool IsOpen { get; set; }
		public ICollection<GroupSavingsMembers> GroupSavingsMembers { get; set; }
	}
}
