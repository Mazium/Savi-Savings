

namespace Savi_Thrift.Domain.Entities
{
    public class GroupSavingsMembers :BaseEntity
    {
        public string UserId { get; set; }
        public string GroupSavingsId { get; set; }
        public bool IsGroupOwner { get; set; }
        public string Position { get; set;}
        public DateTime LastSavingDate { get;set; }
    }
}
