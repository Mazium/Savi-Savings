using Microsoft.AspNetCore.Identity;

namespace Savi_Thrift.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateModified {  get; set; }
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public ICollection<GroupSavings> GroupSavings { get; set; }
        public ICollection<CardDetail> CardDetails { get; set; }
		public ICollection<Saving> Savings { get; set; }
		public ICollection<UserTransaction> UserTransactions { get; set; }
		public ICollection<GroupTransactions> GroupTransactions { get; set; }
	}
}