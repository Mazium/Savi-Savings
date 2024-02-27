namespace Savi_Thrift.Application.DTO.AppUser
{
    public class NewUserResponseDto
    {
       // public string UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
