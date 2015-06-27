using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IoTheMan.Tests.MongoFeatureDemos
{
    public class ContactInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set;}

        [BsonIgnoreIfNull]
        public string Email { get; set; }
        [BsonIgnoreIfNull]
        public string Phone { get; set; }
    }
}