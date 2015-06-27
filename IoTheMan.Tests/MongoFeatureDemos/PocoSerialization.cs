using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;

namespace IoTheMan.Tests.MongoFeatureDemos
{
    [TestFixture]
    public class PocoSerialization
    {
        public PocoSerialization()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }
        [Test]
        public void ToJson_AutomaticallySerializes()
        {
            var person = new Human
            {
                Age = 54,
                FirstName = "Bob"
            };

            person.Address.Add("Your mom's house");
            person.Address.Add("Cloud City");

            person.ContactInfo.Email = "yourmom@acme.local";
            person.ContactInfo.Phone = "123-456-7890";

            string serialized = null;

            Assert.DoesNotThrow(() => serialized = person.ToJson());

            Console.WriteLine(serialized);
        }

        [Test]
        public void WhenBsonIgnoreAttribute_FieldIsNotSerialized()
        {
            var person = new Human();

            var document = person.ToBsonDocument();

            Console.WriteLine(person.ToJson());

            Assert.False(document.Contains("IgnoreMe"));
        }

        [Test]
        public void WhenBsonElementAttribute_CustomFieldNameSerializedToDocument()
        {
            var person = new Human();

            var document = person.ToBsonDocument();

            Console.WriteLine(person.ToJson());

            Assert.True(document.Contains("UpdatedMemberName"));
            Assert.False(document.Contains("Obsolete"));
        }

        [Test] public void WhenBsonElementAttribute_NonPublicMembersIncludedInDocument()
        {
            var person = new Human();

            var serialized = person.ToJson();

            Console.WriteLine(person.ToJson());

            Assert.True(serialized.Contains("mplementationDetail"));
        }

        [Test]
        public void WhenBsonIgnoreAttribute_PropertyOmittedFromDocument()
        {
            var person = new Human();

            var document = person.ToBsonDocument();

            Console.WriteLine(person.ToJson());

            Assert.False(document.Contains("FirstName"));
        }

        [Test]
        public void WhenBsonRepresentationDouble_DecimalSerializedAsDouble()
        {
            var person = new Human { NetWorth = 100.5m };

            var document = person.ToBsonDocument();

            Console.WriteLine(document["NetWorth"]);

            Assert.True(document["NetWorth"].IsDouble);
        }

        [Test]
        public void DateTime_Always_SerializesDateTimeAsUtc()
        {
            var localTime = DateTime.Now;
            var utcTime = DateTime.UtcNow;

            var person = new Human
            {
                LocalTime = localTime,
                UnmodifiedTime = localTime
            };

            var document = person.ToBsonDocument();
            var serializedWithLocalAttributeTime = (DateTime)document["LocalTime"];
            var serializedUnmodifiedTimeTime = (DateTime)document["UnmodifiedTime"];

            Console.WriteLine(serializedWithLocalAttributeTime);
            Console.WriteLine(serializedUnmodifiedTimeTime);

            Assert.AreEqual(utcTime.Hour, serializedWithLocalAttributeTime.Hour);
            Assert.AreEqual(utcTime.Hour, serializedUnmodifiedTimeTime.Hour);
        }

        [Test]
        public void WhenNoBsonDateTimeLocalAttribute_DateTimeDeserializedAsUtc()
        {
            var localTime = DateTime.Now;
            var utcTime = DateTime.UtcNow;

            var document = new BsonDocument
            {
                { "UnmodifiedTime", localTime }
            };

            var person = BsonSerializer.Deserialize<Human>(document);

            var deserializedTime = person.UnmodifiedTime.GetValueOrDefault();

            Console.WriteLine(deserializedTime);

            Assert.AreEqual(utcTime.Hour, deserializedTime.Hour);
        }

        [Test]
        public void WhenBsonDateTimeLocalAttribute_DateTimeDeserializedAsLocal()
        {
            var localTime = DateTime.Now;

            var document = new BsonDocument
            {
                { "LocalTime", localTime }
            };

            var person = BsonSerializer.Deserialize<Human>(document);

            var deserializedTime = person.LocalTime.GetValueOrDefault();

            Console.WriteLine(deserializedTime);

            Assert.AreEqual(localTime.Hour, deserializedTime.Hour);
        }

        [Test]
        public void WhenBsonIdAttribute_NonIdFieldUsedAsId()
        {
            var person = new Human { PersonId = 666 };

            var document = person.ToBsonDocument();

            Console.WriteLine(person.ToJson());

            Assert.AreEqual((int)document["_id"], 666);
        }

        [Test]
        public void WhenNoDateTimeDateOnlyAttribute_DateTimeSerializedWithUtcTime_()
        {
            var today = DateTime.Today;
            var todayUtc = today.ToUniversalTime();

            var person = new Human { UnmodifiedTime = today };

            var document = person.ToBsonDocument();

            var serializedDate = (DateTime)document["UnmodifiedTime"];

            Console.WriteLine(serializedDate);

            Assert.AreEqual(todayUtc.Hour, serializedDate.Hour);
        }

        [Test]
        public void WhenDateTimeDateOnlyAttribute_DateTimeSerializedWithZeroTime()
        {
            var today = DateTime.Today;

            var person = new Human { BirthDate = today };

            var document = person.ToBsonDocument();

            var serializedDate = (DateTime)document["BirthDate"];

            Console.WriteLine(serializedDate);

            Assert.AreEqual(today.Hour, serializedDate.Hour);
        }

        [Test]
        public void Serialization_WhenDateTimeDateOnlyAttribute_RequiresDateTimeToHaveZeroTime()
        {
            var dateTimeNow = DateTime.Now;

            var person = new Human { BirthDate = dateTimeNow };

            Assert.Throws<BsonSerializationException>(() => person.ToBsonDocument());

            person.BirthDate = dateTimeNow.Date;

            Assert.DoesNotThrow(() => person.ToBsonDocument());
        }

        [Test]
        public void WhenBsonIgnoreExtraElements_CanDeserializeDocumentWithExtraElements()
        {
            var document = new BsonDocument
            {
                { "FirstName", "Reynaldo" }, 
                { "Age", 40 }, 
                { "Weight", 180 }
            };

            Assert.DoesNotThrow(() => BsonSerializer.Deserialize<Human>(document));
        }

        [Test]
        public void WhenIdPropertyIsStringType_IdSerializedAsBsonObjectId()
        {
            var contact = new ContactInfo
            {
                Id = ObjectId.GenerateNewId().ToString()
            };

            var document = contact.ToBsonDocument();

            Assert.True(document["_id"].IsObjectId);
        }
    }
}