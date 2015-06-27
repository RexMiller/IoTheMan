using System;
using IoTheMan.Web.Models;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Models
{
    public class WeekdayOfWeekDueDateRule_NextDueDate
    {
        [Test]
        public void Should_ReturnNextOccurenceInCurrentMonth_WhenNotPast()
        {
            var dueDateRule = new WeekdayOfWeekDueDateRule(DayOfWeek.Wednesday, 3);

            var startDate = new DateTime(2015, 6, 16);

            var nextDueDate = dueDateRule.NextDueDate(startDate);

            var expected = new DateTime(2015, 6, 17);

            Assert.AreEqual(expected, nextDueDate);
        }

        [Test]
        public void Should_ReturnNextOccurrenceInFollowingMonth_WhenPast()
        {
            var dueDateRule = new WeekdayOfWeekDueDateRule(DayOfWeek.Wednesday, 3);

            var startDate = new DateTime(2015, 6, 16);

            var nextDueDate = dueDateRule.NextDueDate(startDate);

            var expected = new DateTime(2015, 6, 17);

            Assert.AreEqual(expected, nextDueDate);
        }

        [Test]
        public void Should_ReturnNakedDateOnly()
        {
            var dueDateRule = new WeekdayOfWeekDueDateRule(DayOfWeek.Wednesday, 3);

            var startDate = new DateTime(2015, 6, 16).AddHours(7.5);

            var nextDueDate = dueDateRule.NextDueDate(startDate);

            var expected = new DateTime(2015, 6, 17);

            Assert.AreEqual(expected, nextDueDate);
        }
    }
}