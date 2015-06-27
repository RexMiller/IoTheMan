using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IoTheMan.Web.Models
{
    public class Recipient
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        [BsonIgnoreIfNull]
        public string PaymentUrl { get; set; }
    }
}