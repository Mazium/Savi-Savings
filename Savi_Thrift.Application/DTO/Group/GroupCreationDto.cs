using Microsoft.AspNetCore.Http;
using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupCreationDto 
    {
        
        public string GroupName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public SavingFrequency Frequency { get; set; }

        public int MemberCount = 5;
        public DateTime RunTime { get; set; }
        public string PurposeAndGoal { get; set; }
        public string TermsAndConditions { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public IFormFile SafePortraitImageURL { get; set; }
        public IFormFile SafeLandScapeImageURL { get; set; }
        public DateTime NextRunTime { get; set; }
        public bool IsOpen { get; set; }


    }
}
