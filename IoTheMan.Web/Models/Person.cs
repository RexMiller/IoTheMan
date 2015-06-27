using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IoTheMan.Web.Models
{
    public class Person
    {
        public Person()
        {
            UpcomingPayments = new List<Payment>();
            PaymentHistory = new List<Payment>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonRequired]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public IList<Payment> UpcomingPayments { get; set; }

        [BsonIgnore]
        public IList<Payment> PaymentHistory { get; set; }
    }
}