using Microsoft.AspNetCore.Http;
using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.Kyc
{
    public class KycRequestDto
    {
        [Required(ErrorMessage = "Date ofBirth is required")]
        public DateTime DateOfBirth { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [EnumDataType(typeof(Occupation), ErrorMessage = "Occupation is required")]
        public Occupation Occupation { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "BVN is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "BVN must be exactly 11 digits")]
        public string BVN { get; set; }

        [Required]
        [EnumDataType(typeof(IdentificationType), ErrorMessage = "Identification Type is required")]
        public IdentificationType IdentificationType { get; set; }

        [Required(ErrorMessage = "Identification Number is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Identification Number must be between 10 and 20 characters")]
        [RegularExpression("^[A-Z0-9]*$", ErrorMessage = "Identification Number must be alphanumeric with capital letters")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Identification Document is required")]
        public IFormFile IdentificationDocumentUrl { get; set; }

        [Required(ErrorMessage = "Proof of Address is required")]
        public IFormFile ProofOfAddressUrl { get; set; }
    }
}
