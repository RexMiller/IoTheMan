using System.Threading.Tasks;
using System.Web.Mvc;
using IoTheMan.Web.Models;
using MongoDB.Bson;

namespace IoTheMan.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dbContext;

        public HomeController()
        {
            _dbContext = new DataContext();
        }

        public async Task<ViewResult> Index()
        {
            var buildInfoCommand = new BsonDocument("buildinfo", 1);
            var buildInfo = await _dbContext.Database.RunCommandAsync<BsonDocument>(buildInfoCommand);
        
            ViewData.Add("DbInfo", buildInfo.ToString());

            return View();
        }
    }
}
