using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaleService.Models;

[BsonIgnoreExtraElements]
public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonIgnore]
    public string? Name { get; set; }
    [BsonIgnore]
    public string? Email { get; set; }
    [BsonIgnore]
    public bool? Premium { get; set; }
}