using System;
using IoTheMan.Web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Models
{
    [TestFixture]
    public class DueDateRule_Serialization
    {
        private DayOfMonthDueDateRule _monthRule;
        private WeekdayOfWeekDueDateRule _weekRule;

        [TestFixtureSetUp]
        public void OncePerFixture()
        {
            _monthRule = new DayOfMonthDueDateRule(15);
            _weekRule = new WeekdayOfWeekDueDateRule(DayOfWeek.Wednesday, 3);
        }

        [Test]
        public void DayOfMonthDueDateRule_CanSerializeAsBaseType_DeserializeAsImplementationType()
        {
            var monthDoc = _monthRule.ToBsonDocument();

            var monthObj = BsonSerializer.Deserialize<DueDateRule>(monthDoc);

            Assert.AreEqual(typeof (DayOfMonthDueDateRule), monthObj.GetType());
        }

        [Test]
        public void WeekdayOfWeekDueDateRule_CanSerializeAsBaseType_DeserializeAsImplementationType()
        {
            var weekDoc = _weekRule.ToBsonDocument();

            var weekObj = BsonSerializer.Deserialize<DueDateRule>(weekDoc);

            Assert.AreEqual(typeof (WeekdayOfWeekDueDateRule), weekObj.GetType());
        }

        [Test]
        public void DayOfMonthNextDueDateRule_AfterDeserializingAsBaseType_ReturnsCorrectDate()
        {
            var monthDoc = _monthRule.ToBsonDocument();

            var monthObj = BsonSerializer.Deserialize<DueDateRule>(monthDoc);

            var dueDate = monthObj.NextDueDate(DateTime.Today);

            Assert.AreEqual(15, dueDate.Day);
        }

        [Test]
        public void WeekdayOfWeekDueDateRule_AfterDeserializingAsBaseType_ReturnsCorrectDate()
        {
            var weekDoc = _weekRule.ToBsonDocument();

            var weekObj = BsonSerializer.Deserialize<DueDateRule>(weekDoc);

            var dueDate = weekObj.NextDueDate(DateTime.Today);

            Assert.AreEqual(DayOfWeek.Wednesday, dueDate.DayOfWeek);
        }
    }
}