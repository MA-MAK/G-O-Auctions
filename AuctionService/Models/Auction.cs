
namespace AuctionService.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AuctionStatus Status { get; set; }
        public AuctionType Type { get; set; }
        public Item Item { get; set; }
        public int ItemId { get; set; }
    }
    public enum AuctionStatus
    {
        Pending,
        Active,
        Closed
    }

    public enum AuctionType
    {
        English,
        Dutch
    }
}