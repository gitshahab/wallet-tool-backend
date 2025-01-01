namespace Wallet_tool.Model.Domain
{
    public class WalletAccount
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManager { get; set; }
        public string Role { get; set; }
        public decimal Balance { get; set; }
        public decimal Expenditure { get; set; } = 0;
        public string Status { get; set; } = "inActive";
    }
}
