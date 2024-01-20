using Microsoft.AspNetCore.Identity;

namespace Savi_Thrift.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateModified {  get; set; } 
        public ICollection<Group>? Groups { get; set; }
        public ICollection<CardDetail>? CardDetails { get; set; }
		public ICollection<Saving>? Savings { get; set; }
		public ICollection<UserTransaction>? UserTransactions { get; set; }
		public ICollection<GroupTransaction>? GroupTransactions { get; set; }
	}
}