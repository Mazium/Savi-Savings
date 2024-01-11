using Microsoft.AspNetCore.Http;
using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO
{
    public class KycRequestDto
    {
        [Required(ErrorMessage = "Date ofBirth is required")]
        public DateOnly DateOfBirth { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [EnumDataType(typeof(Occupation), ErrorMessage = "Occupation is required")]
        public Occupation Occupation { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "BVN is required")]
        public string BVN { get; set; } = string.Empty;

        [EnumDataType(typeof(IdentificationType), ErrorMessage = "Identification Type is required")]
        public IdentificationType IdentificationType { get; set; }

        [Required(ErrorMessage = "Identification Number is required")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Identification Document is required")]
        public IFormFile IdentificationDocumentUrl { get; set; }

        [Required(ErrorMessage = "Proof of Address is required")]
        public IFormFile ProofOfAddressUrl { get; set; }
    }
}
