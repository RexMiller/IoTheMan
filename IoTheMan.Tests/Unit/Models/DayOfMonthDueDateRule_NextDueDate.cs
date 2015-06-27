using System;
using IoTheMan.Web.Models;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Models
{
    public class DayOfMonthDueDateRule_NextDueDate
    {
        [Test]
        public void Should_ReturnNextOccurrenceInCurrentMonth_WhenNotPast()
        {
            var dueDateRule = new DayOfMonthDueDateRule(15);

            var startDate = new DateTime(2015, 6, 14);

            var nextDueDate = dueDateRule.NextDueDate(startDate);

            var expected = new DateTime(2015, 6, 15);

            Assert.AreEqual(expected, nextDueDate);
        }
    
        [Test]
        public void Should_ReturnNextOccurrenceInFollowingMonth_WhenPast()
        {
            var dueDateRule = new DayOfMonthDueDateRule(15);

            var startDate = new DateTime(2015, 6, 16);

            var nextDueDate = dueDateRule.NextDueDate(startDate);

            var expected = new DateTime(2015, 7, 15);

            Assert.AreEqual(expected, nextDueDate);
        }

        [Test]
        public void Should_ReturnNakedDateOnly()
        {
            var dueDateRule = new DayOfMonthDueDateRule(15);

            var startDate = new DateTime(2015, 6, 16).AddHours(7.5);

            var nextDueDate = dueDateRule.NextDueDate(startDate);

            var expected = new DateTime(2015, 7, 15);

            Assert.AreEqual(expected, nextDueDate);
        }
    }
}