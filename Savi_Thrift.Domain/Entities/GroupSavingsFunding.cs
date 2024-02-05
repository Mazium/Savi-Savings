using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Domain.Entities
{
    public class GroupSavingsFunding :BaseEntity
    {
        public string UserId { get; set; }
        public string ActionId { get; set; }
        public string GroupSavingsId { get; set; } 
        public decimal Amount { get; set; } 




    }
}
