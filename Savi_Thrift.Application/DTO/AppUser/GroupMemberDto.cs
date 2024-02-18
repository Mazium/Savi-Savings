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
