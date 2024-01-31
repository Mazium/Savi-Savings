using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Saving
{
    public class GetPersonalSavingsDTO
    {
        public string Id { get; set; }

        public string WalletId { get; set; }

        public string Title { get; set; }

        public decimal GoalAmount { get; set; }

        public DateTime NextSaveDate { get; set; }

        public DateTime ModifiedAt { get; set;}

        public bool AutoSave { get; set; }

        public decimal AutoSaveAmount { get; set;}         

        public SavingFrequency Frequency { get; set; }

        public string Avatar { get; set; }

    }
}
