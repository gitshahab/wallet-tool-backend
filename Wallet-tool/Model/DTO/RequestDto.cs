namespace Wallet_tool.Model.DTO
{
    public class RequestDto
    {
        public Guid FromId { get; set; }
        public string From { get; set; }
        public Guid ToId { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}
