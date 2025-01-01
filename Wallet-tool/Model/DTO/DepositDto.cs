namespace Wallet_tool.Model.DTO
{
    public class DepositDto
    {
        public Guid AccountId { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}
