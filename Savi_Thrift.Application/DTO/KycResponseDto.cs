﻿using Savi_Thrift.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.DTO
{
    public class KycResponseDto
    {
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Occupation Occupation { get; set; }
        public string Address { get; set; } = string.Empty;
        public string BVN { get; set; } = string.Empty;
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationDocumentUrl { get; set; }
        public string ProofOfAddressUrl { get; set; }
    }
}
