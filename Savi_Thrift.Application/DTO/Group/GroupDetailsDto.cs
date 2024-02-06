using Savi_Thrift.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupDetailsDto
    {
        public string Name { get; set; } = string.Empty;
        public string PurposeAndGoal { get; set; }
        public decimal ContributionAmount { get; set; }
        public int DurationInMonths { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate => StartDate.AddMonths(DurationInMonths);
        public int MemberCount { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public GroupStatus  GroupStatus { get; set; }        
        public int MaxNumberOfParticipants { get; set; }        
        public string SafePortraitImageURL { get; set; }         
        public ICollection<Domain.Entities.GroupTransaction> GroupTransactions { get; set; } = new List<Domain.Entities.GroupTransaction>();

    }
}
