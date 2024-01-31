namespace Savi_Thrift.Application.DTO
{
    public class ResponseDto<T>
    {
        public string DisplayMessage { get; set; }
        public int StatusCode { get; set; }
        public T Result { get; set; }
    }
}
