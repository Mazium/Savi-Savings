using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO
{
    public class GetAllKycsDto
    {
        public List<KycResponseDto> Kycs { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
    }
}
