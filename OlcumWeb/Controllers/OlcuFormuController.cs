using OlcumWeb.dbOlcum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class OlcuFormuController : Controller
    {
        OlcumContext yu = new OlcumContext();

        // GET: OlcuFormu
        public ActionResult Index(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public PartialViewResult HucreGorunumu(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }

        public PartialViewResult ListeGorunumu()
        {
            return PartialView();
        }
    }
}