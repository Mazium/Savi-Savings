using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Domain.Entities
{
    public class GroupSavings : BaseEntity
    {

        public string GroupName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set;}
        public SavingFrequency Frequency { get; set;}

        public int MembersMaximumCount = 5;
        public DateTime RunTime { get; set; }
        public string PurposeAndGoal { get; set;}
        public string TermsAndConditions { get; set;}
        public GroupStatus GroupStatus { get; set;}
        public string SafePortraitImageURL  { get; set;}
        public string SafeLandScapeImageURL { get; set; }
        public DateTime? NextRunTime { get; set; }
        public bool IsOpen { get; set; }

        public ICollection<GroupTransactions> GroupSavingsFundings{ get; set; }

        public ICollection<GroupSavingsMembers> GroupSavingsMembers { get; set; }




    }
}
