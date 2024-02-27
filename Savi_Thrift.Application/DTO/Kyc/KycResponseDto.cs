using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Kyc
{
    public class KycResponseDto
    {
        public string KycId { get; set; }
        public string UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Occupation Occupation { get; set; }
        public string Address { get; set; }
        public string BVN { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationDocumentUrl { get; set; }
        public string ProofOfAddressUrl { get; set; }
    }
}
