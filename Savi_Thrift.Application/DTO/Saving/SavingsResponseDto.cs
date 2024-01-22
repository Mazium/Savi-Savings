namespace Savi_Thrift.Application.DTO.Saving
{
    public class SavingsResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    //response format when crediting personal savings
}
