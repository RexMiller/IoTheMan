using System;
using IoTheMan.Web.Models;
using MongoDB.Bson;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Models
{
    [TestFixture]
    public class Payment_Serialization
    {
        [Test]
        public void PaymentPersonId_ShouldBeSerializedAsBsonIbjectId()
        {
            var payment = new Payment { PersonId = DataContext.NewObjectId() };

            var document = payment.ToBsonDocument();

            var serialized = document["PersonId"];

            Assert.True(serialized.IsObjectId, "PersonId is not serialized as BsonObjectId");
        }

        [Test]
        public void PaymentAmount_ShouldBeSerializedAsNumericType()
        {
            var payment = new Payment { Amount = 100.5m };

            var document = payment.ToBsonDocument();

            var amountDeserialized = document["Amount"];

            Assert.True(amountDeserialized.IsDouble, "Payment amount serialized as non-numeric");
        }

        [Test]
        public void PaymentPaidOn_ShouldNotBeSerialized_WhenNull()
        {
            var payment = new Payment { PaidOn = null };

            var doc = payment.ToBsonDocument();

            Assert.False(doc.Contains("PaidOn"));
        }

        [Test]
        public void PaymentPaidOn_MustBeSerializedAsNakedDate()
        {
            var payment = new Payment { PaidOn = DateTime.Now };

            Assert.Throws<BsonSerializationException>(() => payment.ToBsonDocument());
        }

        [Test]
        public void PaymentDueOn_MustBeSerializedAsNakedDate()
        {
            var payment = new Payment { DueOn = DateTime.Now };

            Assert.Throws<BsonSerializationException>(() => payment.ToBsonDocument());
        }    
    }
}