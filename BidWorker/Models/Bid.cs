using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BidWorker.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
[BsonIgnoreExtraElements]
public class Bid
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string? Id { get; set; }
    public Customer? Customer { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string AuctionId { get; set; }


   
}
