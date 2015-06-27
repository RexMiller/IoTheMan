using System.Collections.Generic;
using System.Web.Mvc;
using FakeItEasy;
using IoTheMan.Web.Controllers;
using IoTheMan.Web.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace IoTheMan.Tests.Unit.Controllers
{
    [TestFixture]
    public class PersonController_ReadActions
    {
        private PersonController _controller;
        private IDataContext _dataContext;
        private IMongoCollection<Person> _peopleCollection;
        private const string ID_ONE = "558abc4c0b4186cab4b12caf";
        private const string ID_TWO = "558abc550b4186cab4b12cb0";

        [SetUp]
        public void OnceBeforeEachTest()
        {
            var person1 = new Person { Id = ID_ONE, Name = "one" };
            var person2 = new Person { Id = ID_TWO, Name = "two" };
            var people = new List<Person> { person1, person2 };

            _peopleCollection = FakeFactory.MongoCollection(people);

            _dataContext = A.Fake<IDataContext>();

            A.CallTo(() => _dataContext.People).Returns(_peopleCollection);

            _controller = new PersonController(_dataContext);
        }

        [Test, Ignore]
        public void Index_ShouldReturnViewWithPersonList()
        {
            var result = _controller.Index().Result;
            var model = (Person) ((ViewResult)result).Model;

            Assert.IsInstanceOf<IEnumerable<Person>>(model);
        }

        [Test]
        public void Details_ShouldReturnViewWithPersonModel()
        {
            var result = (ViewResult)_controller.Details(DataContext.NewObjectId()).Result;
            var resultModelType = result.Model.GetType();
            var expectedModelType = typeof(Person);

            Assert.AreEqual(expectedModelType, resultModelType);
        }

        [Test]
        public void Create_ShouldReturnViewWithPersonModel()
        {
            var result = (ViewResult)_controller.Create();
            var resultModelType = result.Model.GetType();
            var expectedModelType = typeof(Person);

            Assert.AreEqual(expectedModelType, resultModelType);
        }

        [Test]
        public void Edit_ShouldReturnViewWithPersonModel()
        {
            var result = (ViewResult) _controller.Edit(ID_TWO).Result;
            var modelType = result.Model.GetType();
            var expectedType = typeof (Person);

            Assert.AreEqual(expectedType, modelType);
        }

        [Test]
        public void Edit_ViewModelShouldMatchIdParameter()
        {
            var result = (ViewResult) _controller.Edit(ID_ONE).Result;
            var model = (Person)result.Model;

            Assert.AreEqual(ID_ONE, model.Id);
        }

        [Test]
        public void Edit_ShouldRetrieveModelFromDatabase()
        {
            var result = (ViewResult) _controller.Edit(ID_ONE).Result;

            _peopleCollection.VerifyFindByIdAsync();
        }
    }
}