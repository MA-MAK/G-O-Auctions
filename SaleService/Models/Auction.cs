using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace SaleService.Models;

public class Auction
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonIgnore]
    public DateTime? StartTime { get; set; }
    [BsonIgnore]
    public DateTime? EndTime { get; set; }
    [BsonIgnore]
    public AuctionStatus? Status { get; set; }
    [BsonIgnore]
    public string? Title { get; set; }
    [BsonIgnore]
    public AuctionType? Type { get; set; }
    [BsonIgnore]
    public Item? Item { get; set; }
    // public List<Bid>? Bids { get; set; }
    [BsonIgnore]
    public string? Description { get; set; }
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

