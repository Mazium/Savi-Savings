using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Domain.Entities.Helper
{
    public class CloudinaryConfig
    {
        public string CloudName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ApiSecret { get; set; } = string.Empty;
        public string SectionName { get; set; } = "CloudinarySettings";
    }
}
