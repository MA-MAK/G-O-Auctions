using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace SaleService.Models;

public class Sale
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public decimal Amount { get; set; }
    [BsonIgnore]
    public Customer? Customer { get; set; }
    public Auction? Auction { get; set; }
}