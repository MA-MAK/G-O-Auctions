using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CustomerService.Models;


public class Bid
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    public Customer Bidder { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string? AuctionId { get; set; }
}
