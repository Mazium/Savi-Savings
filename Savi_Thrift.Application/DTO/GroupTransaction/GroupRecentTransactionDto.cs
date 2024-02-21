using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO.GroupTransaction
{
    public class GroupRecentTransactionDto : BaseEntity
    {
        public string ActionId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }
}
