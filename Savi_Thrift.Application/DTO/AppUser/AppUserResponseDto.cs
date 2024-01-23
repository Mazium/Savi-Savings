using Savi_Thrift.Application.DTO.CardDetail;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Application.DTO.Savings;
using Savi_Thrift.Application.DTO.UserTransaction;

namespace Savi_Thrift.Application.DTO.Appuser
{
    public class AppUserResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateModified { get; set; }
        public List<GroupResponseDto> Groups { get; set; }
        public List<CardDetailDto> CardDetails { get; set; }
        public List<SavingDto> Savings { get; set; }
        public List<UserTransactionDto> UserTransactions { get; set; }
        public List<GroupTransactionResponseDto> GroupTransactions { get; set; }
    }
}
