using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace AuctionService.Models;

public class Auction
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuctionStatus Status { get; set; }
    public string Title { get; set; }
    public AuctionType Type { get; set; }
    public Item? Item { get; set; }
    [BsonIgnore]
    public string? ItemId { get; set; }//TODO: remove this
    public List<Bid>? Bids { get; set; }
    public string Description { get; set; }

    public Auction()
    {

    }
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

