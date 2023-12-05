using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
<<<<<<< HEAD
using System.Text.Json.Serialization;
//sing Json.Net;
=======
using System.Collections.Generic;


>>>>>>> 24c6decb25a646987ca42c4f09bd2f58589ab398


namespace AuctionService.Models;



public class Auction
{
<<<<<<< HEAD
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuctionStatus Status { get; set; }
    public string Title { get; set; }
    public AuctionType Type { get; set; }
    [BsonIgnore]
    public Item? Item { get; set; }
    public int ItemId { get; set; }
    [JsonIgnore]
    public List<Bid>? Bids { get; set; }
    public string Description { get; set; }
=======
    public class Auction
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonElement("StartTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("EndTime")]
        public DateTime EndTime { get; set; }

        [BsonElement("Status")]
        public AuctionStatus Status { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Type")]
        public AuctionType Type { get; set; }


        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("ItemId")]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public List<Bid> Bids { get; set; }
       
    }
    public enum AuctionStatus
    {
        Pending,
        Active,
        Closed
    }
>>>>>>> 24c6decb25a646987ca42c4f09bd2f58589ab398

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

