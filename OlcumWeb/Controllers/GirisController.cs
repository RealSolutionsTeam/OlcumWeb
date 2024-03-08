using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;


namespace OlcumWeb.Controllers
{
    public class GirisController : Controller
    {
        OlcumContext db = new OlcumContext();

        public ActionResult Index()
        {
            return User.Identity.IsAuthenticated ? (ActionResult)Redirect("/Anasayfa/Index") : View("Index", new DtoLoginUser());
        }

        public JsonResult GirisLocal(string username,string password)
        {
            var p = db.PersonelTablo.Where(x => x.personelkAdi == username && x.personelSifre == password).Select(x=> new  { id = x.id , personelAd = x.personelAd, personelSoyad = x.personelSoyad,
            
            personelkAdi = x.personelkAdi , personelSifre = x.personelSifre, personelGorev = x.personelGorev, personelAdminmi = x.personelAdminmi}).FirstOrDefault();


            return Json(p, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(DtoLoginUser personel, string returnUrl)
        {
            if(db.PersonelTablo.Any(x=>x.personelkAdi == personel.personelkAdi && x.personelSifre == personel.personelSifre))
            {
                PersonelTablo dbPersonel = db.PersonelTablo.Where(x => x.personelkAdi == personel.personelkAdi && x.personelSifre == personel.personelSifre).FirstOrDefault();



                return ((returnUrl == null) || (returnUrl == "")) ? RedirectToAction("Index","Anasayfa") : (ActionResult)Redirect(returnUrl);
            }

            TempData["hata"] = "Kullanıcı adı veya şifre hatalı";

            return View(personel);
        }


        public ActionResult Logout()
        {            
            return RedirectToAction("Index", "Giris");
        }

    }
}