using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace SaleService.Models;

[BsonIgnoreExtraElements]
public class Sale
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public decimal Amount { get; set; }
    public Auction? Auction { get; set; }
    public Customer? Customer { get; set; }
}