using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO.AppUser
{
    public class GroupMemberDto
    {
        public string UserId { get; set; }
        public string GroupSavingsId { get; set; }
        public bool IsGroupOwner { get; set; }
       // public string Position { get; set; }
        public DateTime LastSavingDate { get; set; }
    }
}
