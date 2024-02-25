namespace Savi_Thrift.Application.DTO.DefaultUser
{
	public class DefaultUserDto
	{
		public string Id { get; set; }
		public string AppUserId { get; set; }
		public string GroupSavingId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastLoginTime { get; set; }
	}
}
