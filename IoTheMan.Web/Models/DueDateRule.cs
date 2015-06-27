using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IoTheMan.Web.Models
{
    [BsonIgnoreExtraElements]
    public abstract class DueDateRule
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public abstract DateTime NextDueDate(DateTime startingFrom);
    }

    [BsonDiscriminator(Required = true)]
    public class DayOfMonthDueDateRule : DueDateRule
    {
        public int DayOfMonth { get; set; }

        public DayOfMonthDueDateRule(int dayOfMonth)
        {
            DayOfMonth = dayOfMonth;
        }

        public override DateTime NextDueDate(DateTime startingFrom)
        {
            var thisMonth = startingFrom.Month;
            var nextMonth = startingFrom.Month + 1;

            var dueDateThisMonth = PaymentDateForMonth(thisMonth);

            var nextDueDate = startingFrom.Day > DayOfMonth 
                ? PaymentDateForMonth(nextMonth) 
                : dueDateThisMonth;

            return nextDueDate;
        }

        private DateTime PaymentDateForMonth(int month)
        {
            var today = DateTime.Today;
            var thisYear = today.Year;

            return new DateTime(thisYear, month, DayOfMonth);
        }
    }

    [BsonDiscriminator(Required = true)]
    public class WeekdayOfWeekDueDateRule : DueDateRule
    {
        private const int A_WEEK = 7;
        private readonly int _weeksOffset;

        public DayOfWeek DayOfWeek { get; set; }
        public int NthWeek { get; set; }

        public WeekdayOfWeekDueDateRule(DayOfWeek dayOfWeek, int nthWeek)
        {
            DayOfWeek = dayOfWeek;
            NthWeek = nthWeek;
            _weeksOffset = NthWeek - 1;
        }

        public override DateTime NextDueDate(DateTime startingFrom)
        {
            var thisMonth = startingFrom.Month;
            var nextMonth = startingFrom.Month + 1;

            var occurenceThisMonth = NthOccurrenceForMonth(thisMonth);

            var nextDueDate = startingFrom > occurenceThisMonth
                ? NthOccurrenceForMonth(nextMonth)
                : occurenceThisMonth;

            return nextDueDate;
        }

        private DateTime NthOccurrenceForMonth(int month)
        {
            var thisYear = DateTime.Today.Year;
            var firstOfMonth = new DateTime(thisYear, month, 1);
            var monthStartsOn = firstOfMonth.DayOfWeek;

            var daysToFirstOccurrence = DayOfWeek >= monthStartsOn
                ? DayOfWeek - monthStartsOn
                : (DayOfWeek - monthStartsOn) + A_WEEK;

            var firstOccurence = firstOfMonth.AddDays(daysToFirstOccurrence);

            var daysToAddToFirstOccurence = A_WEEK * _weeksOffset;

            var nthOccurrence = firstOccurence.AddDays(daysToAddToFirstOccurence);

            return nthOccurrence;
        }
    }
}