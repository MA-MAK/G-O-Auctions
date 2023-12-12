using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace AuctionService.Models;
public class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonIgnore]
    public string? Title { get; set; }
    [BsonIgnore]
    public decimal StartPrice { get; set; }
    [BsonIgnore]
    public decimal AssesmentPrice { get; set; }
    [BsonIgnore]
    public string? Description { get; set; }
    [BsonIgnore]
    public int Year { get; set; }
    [BsonIgnore]
    public string? Location { get; set; }
    [BsonIgnore]
    public Customer? Customer { get; set; }
    [BsonIgnore]
    public Category Category { get; set; }
    [BsonIgnore]
    public Condition Condition { get; set; }
    [BsonIgnore]
    public Status Status { get; set; }
}
public enum Category
{
    Electronics,
    Fashion,
    Home,
    Garden,
    Sports,
    Toys,
    Vehicles,
    Other
}
public enum Condition
{
    New,
    Good,
    Fair,
    Poor
}
public enum Status
{
    Registered,
    ReadyForAuction,
    Auctioning,
    Sold,
    NotSold,
}
