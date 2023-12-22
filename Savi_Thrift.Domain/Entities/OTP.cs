﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class OTP : BaseEntity
	{
		[ForeignKey("AppUserId")]
		public string UserId { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
		public bool IsUsed { get; set; }
	}
}
