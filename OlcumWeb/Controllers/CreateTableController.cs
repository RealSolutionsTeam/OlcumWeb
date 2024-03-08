using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class CreateTableController : Controller
    {
        // GET: CreateTable
        public ActionResult Index(int id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}