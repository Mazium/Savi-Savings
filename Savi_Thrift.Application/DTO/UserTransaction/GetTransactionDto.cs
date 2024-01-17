using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO.UserTransaction
{
    public class GetTransactionDto
    {
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
