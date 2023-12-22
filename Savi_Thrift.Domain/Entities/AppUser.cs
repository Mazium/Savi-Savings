using Microsoft.AspNetCore.Identity;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateModified {  get; set; } 
        public ICollection<Group> Groups { get; set; }
        public ICollection<CardDetail> CardDetails { get; set; }
    }
}