using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ItemService.Models

{
    [BsonIgnoreExtraElements]
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonIgnore]
        public string? Name { get; set; }
        [BsonIgnore]
        public string? Email { get; set; }

        [BsonIgnore]
        public bool? Premium { get; set; }

        // Constructor
        /*
        public Customer(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        */

    }
}
