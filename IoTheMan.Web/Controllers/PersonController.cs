using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using IoTheMan.Web.Models;
using MongoDB.Driver;

namespace IoTheMan.Web.Controllers
{
    public class PersonController : Controller
    {
        private readonly IDataContext _dataContext;

        public PersonController()
            : this(new DataContext()) { }

        public PersonController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ActionResult> Index()
        {
            var people = await _dataContext.People.ToListAsync();

            return View(people);
        }

        public async Task<ActionResult> Details(string id)
        {
            var person = await _dataContext.People.FindByIdAsync(id.ToObjectId());

            return View(person);
        }

        public ActionResult Create()
        {
            var model = new Person();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Person person)
        {
            person.Id = DataContext.NewObjectId();

            await _dataContext.People.InsertOneAsync(person);

            return RedirectToAction("Details", new { id = person.Id });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var person = await _dataContext.People.FindByIdAsync(id.ToObjectId());

            return View(person);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Person person)
        {
            var update = Builders<Person>.Update
                .Set(p => p.Name, person.Name)
                .Set(p => p.Email, person.Email)
                .Set(p => p.PhoneNumber, person.PhoneNumber);

            await _dataContext.People.FindOneAndUpdateAsync(p => p.Id == person.Id, update);

            return RedirectToAction("Details", new { id = person.Id });
        }

        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}
