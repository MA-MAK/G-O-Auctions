using AuctionService.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Bid
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public Customer Bidder { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string AuctionId { get; set; }

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
