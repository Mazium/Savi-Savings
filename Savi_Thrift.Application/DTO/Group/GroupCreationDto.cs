using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }


     

        [Range(0, double.MaxValue, ErrorMessage = "Contribution amount must be non-negative")]
        public decimal ContributionAmount { get; set; }

        [Required(ErrorMessage = "IsOpen is required")]
        public bool IsOpen { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Max number of participants must be at least 1")]
        public int MaxNumberOfParticipants { get; set; }

        [MaxLength(500, ErrorMessage = "Terms cannot exceed 500 characters")]
        public string Terms { get; set; }

    

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

       
    }
}
