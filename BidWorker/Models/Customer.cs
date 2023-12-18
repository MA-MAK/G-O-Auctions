using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace BidWorker.Models

{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        public string Name { get; set; }
        [JsonIgnore]
        
        [BsonIgnore]
        public string Email { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public bool Premium { get; set; }
    }
}
