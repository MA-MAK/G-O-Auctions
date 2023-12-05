
namespace BidService.Models
{


    public class Bid
    {
        public int Id { get; set; }
        public Customer Bidder { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public int AuctionId { get; set; }


        /*
            public Bid(int id, Customer bidder, decimal amount, DateTime time)
            {
                Id = id;
                Bidder = bidder;
                Amount = amount;
                Time = time;
            }
            */
    }
}
