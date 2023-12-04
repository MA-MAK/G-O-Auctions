using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;



namespace AuctionService.Models
{
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

    public enum AuctionType
    {
        English,
        Dutch
    }
}