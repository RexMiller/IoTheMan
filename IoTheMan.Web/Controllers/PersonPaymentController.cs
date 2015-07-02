using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IoTheMan.Web.Controllers
{
    public class PersonPaymentController : Controller
    {
        [Route("person/{id}/payment/create")]
        public ActionResult Create(string id)
        {
            return View();
        }
    }
}