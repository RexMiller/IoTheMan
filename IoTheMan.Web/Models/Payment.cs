using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IoTheMan.Web.Models
{
    public class Payment
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PersonId { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public decimal Amount { get; set; }

        public Recipient Recipient { get; set; }
        
        public DueDateRule DueDateRule { get; set;}

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime DueOn { get; set;}

        [BsonIgnoreIfNull, BsonDateTimeOptions(DateOnly = true)]
        public DateTime? PaidOn { get; set; }
    }
}