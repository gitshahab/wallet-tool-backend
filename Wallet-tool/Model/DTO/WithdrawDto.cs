namespace Wallet_tool.Model.DTO
{
    public class WithdrawDto
    {
        public Guid AccountId { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}
