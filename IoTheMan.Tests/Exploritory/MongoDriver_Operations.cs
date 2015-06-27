using System;
using System.Collections.Generic;
using System.Linq;
using IoTheMan.Web.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace IoTheMan.Tests.Exploritory
{
    [TestFixture]
    public class MongoDriver_Operations
    {
        private readonly IDataContext _dataContext = new DataContext();
        private const string PERSON_ID = "558705f85292420cf034b504";
        private List<Person> _personList;

        [TestFixtureSetUp]
        public void OnceBeforeFixture()
        {
            //var doc = new BsonDocument();
            
            //_dataContext.People.DeleteManyAsync(doc);

            //var count = _dataContext.People.ToListAsync().Result.Count;
            
            //Console.WriteLine(count);

            DeleteTestPersonIfExists();

            InitializePersonList();

            _dataContext.People.InsertManyAsync(_personList).Wait();

            var people = _dataContext.People.ToListAsync().Result;

            Console.WriteLine("{0} people in collection", people.Count);
        }

        [TestFixtureTearDown]
        public void OnceAfterFixture()
        {
            var ids = _personList.Select(p => p.Id);
            var filter = Builders<Person>.Filter.Where(p => ids.Contains(p.Id));

            var deleted = _dataContext.People.DeleteManyAsync(filter).Result;
            Console.WriteLine("{0} deleted", deleted.DeletedCount);

            var remaining = _dataContext.People.ToListAsync().Result;
            Console.WriteLine("{0} remaining", remaining.Count);

            //var doc = new BsonDocument();
            
            //_dataContext.People.DeleteManyAsync(doc);

            //var count = _dataContext.People.ToListAsync().Result.Count;
            
            //Console.WriteLine(count);
        }

        [Test]
        public void A_Inserting_Person()
        {
            var person = new Person { Id = PERSON_ID, Name = "Your Mom" };

            _dataContext.People.InsertOneAsync(person).Wait();
        }

        [Test]
        public void B_Finding_PersonById()
        {
            var person = _dataContext.People.FindByIdAsync(PERSON_ID.ToObjectId()).Result;

            Assert.NotNull(person);
        }

        [Test]
        public void B1_Updating_Person()
        {
            var firstPerson = _dataContext.People.ToListAsync().Result.First();

            var id = firstPerson.Id.ToObjectId();

            var updateDef = Builders<Person>.Update.Set(p => p.Name, "0987");

            _dataContext.People.FindOneAndUpdateAsync(p => p.Id == firstPerson.Id, updateDef).Wait();

            var updated = _dataContext.People.FindByIdAsync(id).Result;

            Assert.AreEqual("0987", updated.Name);
        }

        [Test]
        public void C_Deleting_InsertedPerson()
        {
            var filter = Builders<Person>.Filter.Eq(p => p.Id, PERSON_ID);

            _dataContext.People.DeleteOneAsync(filter).Wait();

            var nonExistantPerson = _dataContext.People.FindByIdAsync(PERSON_ID.ToObjectId()).Result;

            Assert.Null(nonExistantPerson);
        }

        private void DeleteTestPersonIfExists()
        {
            var filter = Builders<Person>.Filter.Eq(p => p.Id, PERSON_ID);

            _dataContext.People.DeleteOneAsync(filter).Wait();
        }

        private void InitializePersonList()
        {
            _personList = new List<Person>(10);

            for (var i = 100; i < 110; i++)
            {
                var person = InitializePerson(i);

                _personList.Add(person);
            }
        }

        private static Person InitializePerson(int i)
        {
            var person = new Person
            {
                Id = DataContext.NewObjectId(),
                Name = string.Format("Person {0}", i),
            };

            person.UpcomingPayments.Add(new Payment
            {
                Amount = i,
                DueDateRule = new DayOfMonthDueDateRule(15 + (i - 100)),
                Recipient = new Recipient
                {
                    Name = string.Format("Recipient {0}", i),
                    PaymentUrl = string.Format("https://acme-{0}.local", i)
                }
            });
            return person;
        }
    }
}
