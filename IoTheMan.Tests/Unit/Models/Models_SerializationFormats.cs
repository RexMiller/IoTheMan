using System;
using IoTheMan.Web.Models;
using MongoDB.Bson;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Models
{
    public class Model_Serialization
    {
        [TestCase(typeof(Person))]
        [TestCase(typeof(Recipient))]
        [TestCase(typeof(Payment))]
        [TestCase(typeof(DayOfMonthDueDateRule))]
        [TestCase(typeof(WeekdayOfWeekDueDateRule))]
        public void IdProperty_ShouldBeSerializedAsBsonObjectId(Type type)
        {
            var instance = InitializeModel(type);

            var document = instance.ToBsonDocument();

            var idSerialized = document["_id"];

            Assert.True(idSerialized.IsObjectId, "Id is not serialized as BsonObjectId");
        }

        [Test]
        public void RecipientPaymentUrl_ShouldNotBeSerialized_WhenNull()
        {
            var recipient = new Recipient();

            Assert.Null(recipient.PaymentUrl);

            var document = recipient.ToBsonDocument();

            Assert.False(document.Contains("PaymentUrl"));
        }

        private static object InitializeModel(Type type)
        {
            object instance;

            switch (type.Name)
            {
                case "DayOfMonthDueDateRule":
                    instance = Activator.CreateInstance(type, 15);
                    break;

                case "WeekdayOfWeekDueDateRule":
                    instance = Activator.CreateInstance(type, DayOfWeek.Wednesday, 3);
                    break;

                default:
                    instance = Activator.CreateInstance(type);
                    break;
            }


            var idPropertyInfo = type.GetProperty("Id");

            idPropertyInfo.SetValue(instance, ObjectId.GenerateNewId().ToString());
            return instance;
        }
    }


    [TestFixture]
    public class Person_Serialization
    {
        [Test]
        public void PaymentHistoryShouldBeIgnored()
        {
            var person = new Person();

            person.PaymentHistory.Add(new Payment { Amount = 100m });

            var doc = person.ToBsonDocument();

            Assert.False(doc.Contains("PaymentHistory"));
        }
    }
}