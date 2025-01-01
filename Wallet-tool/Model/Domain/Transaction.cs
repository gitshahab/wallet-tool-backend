namespace Wallet_tool.Model.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; } 
        public Guid WalletAccountId { get; set; }
        public Guid? ToId { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string Transaction_Type { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; } 
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
