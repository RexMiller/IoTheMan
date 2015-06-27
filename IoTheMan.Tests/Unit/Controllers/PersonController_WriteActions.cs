// ReSharper disable UnusedVariable

using System.Web.Mvc;
using FakeItEasy;
using IoTheMan.Web.Controllers;
using IoTheMan.Web.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Controllers
{
    [TestFixture]
    public class PersonController_WriteActions
    {
        private PersonController _controller;
        private IDataContext _dataContext;
        private IMongoCollection<Person> _peopleCollection;

        [TestFixtureSetUp]
        public void OnceBeforeFixture()
        {
            _peopleCollection = A.Fake<IMongoCollection<Person>>();

            _dataContext = A.Fake<IDataContext>();

            A.CallTo(() => _dataContext.People).Returns(_peopleCollection);

            _controller = new PersonController(_dataContext);
        }

        [Test]
        public void CreateShouldInsertNewPerson()
        {
            var person = new Person { Name = "Your mom" };
            var result = _controller.Create(person).Result;

            _peopleCollection.VerifyInsertOneAsync();
        }

        [Test]
        public void CreateShouldRedirectToDetails()
        {
            var person = new Person { Name = "Your mom" };
            var result = (RedirectToRouteResult)_controller.Create(person).Result;

            var action = ((string)result.RouteValues["action"]).ToLower();

            Assert.AreEqual("details", action);
        }

        [Test]
        public void EditShouldCallUpdateOneAsync()
        {
            var person = new Person { Name = "Your mom" };
            var result = _controller.Edit(person);

            _peopleCollection.VerifyFindOneAndUpdateAsync();
        }
    }
}