using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BidService.Models;

[BsonIgnoreExtraElements]
public class Bid
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public Customer? Customer { get; set; }
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string AuctionId { get; set; }


   
}