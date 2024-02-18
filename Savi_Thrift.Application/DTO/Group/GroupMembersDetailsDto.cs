using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO.Group
{
	public class GroupMembersDetailsDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }
		public decimal Performance { get; set; }
	}
}
