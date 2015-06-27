using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IoTheMan.Tests.MongoFeatureDemos
{
    [BsonIgnoreExtraElements]
    public class Human
    {
        private IList<string> _address;
        private ContactInfo _contactInfo;

        [BsonElement]
        private string _implementationDetail = "The details";

        [BsonId]
        public int PersonId { get; set; }

        [BsonIgnoreIfNull]
        public string FirstName { get; set; }

        public int Age { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public Decimal NetWorth { get; set; }

        [BsonElement("UpdatedMemberName")]
        public string ObsoleteMemberName { get; set; }

        [BsonIgnore]
        public string IgnoreMe { get; set; }

        public IList<string> Address
        {
            get { return _address ?? (_address = new List<string>()); }
            set { _address = value; }
        }

        public ContactInfo ContactInfo
        {
            get { return _contactInfo ?? (_contactInfo = new ContactInfo()); }
            set { _contactInfo = value; }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LocalTime { get; set; }

        public DateTime? UnmodifiedTime { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime? BirthDate { get; set; }

        public string GetImplementationDetail()
        {
            return _implementationDetail;
        }
    }
}