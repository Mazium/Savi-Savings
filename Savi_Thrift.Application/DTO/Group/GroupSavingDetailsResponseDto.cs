using Savi_Thrift.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupSavingDetailsResponseDto
    {
        public string GroupName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public SavingFrequency Frequency { get; set; }

        public int MemberCount = 5;
    }
}
