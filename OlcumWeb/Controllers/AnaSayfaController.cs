using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using OlcumWeb.dbOlcum;

namespace OlcumWeb.Controllers
{
    public class AnaSayfaController : Controller
    {
        OlcumContext db = new OlcumContext();
        string ad, soyad;

        public ActionResult Index(int id)
        {
            if(id != 0)
            {
                PersonelTablo dbpersonel = db.PersonelTablo.Where(x => x.id == id).FirstOrDefault();

                ad = dbpersonel.personelAd;
                soyad = dbpersonel.personelSoyad;

                ViewBag.personelAd = ad;
                ViewBag.personelSoyad = soyad;
                ViewBag.id = id;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Giris");
            }
        }

        public ActionResult AlternatifIndex(string kullaniciad, string sifre)
        {
            try
            {
                PersonelTablo nesneCalisan = db.PersonelTablo.Where(x => x.personelkAdi == kullaniciad & x.personelSifre == sifre).FirstOrDefault();

                if (nesneCalisan == null)
                {
                    return Redirect("~/Giris/Index");
                }
                else
                {
                    return Redirect("~/Anasayfa/Index?id=" + nesneCalisan.id);
                }
            }
            catch (Exception hata)
            {
                TempData["hata"] = "Hata " + hata.Message.ToString();
                return View();
            }
        }

        public ActionResult NumuneKesim()
        {
            ViewBag.order = db.Order.ToList();
            ViewBag.adSoyad = ad + " " + soyad;

            return View();
        }

        public JsonResult GetOrderNo(string term)
        {
            List<string> orders = db.Order.Where(x => x.orderNo.Contains(term)).Select(x => x.orderNo).ToList();
            
            return new JsonResult { Data = orders, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ImalatOlcu()
        {
            ViewBag.order = db.Order.ToList();

            return View();
        }

        public ActionResult OlcuFormu()
        {
            ViewBag.order = db.Order.ToList();

            return View();
        }

        public ActionResult Raporlama()
        {
            return View();
        }

        public ActionResult TanimlamaEkrani()
        {
            ViewBag.order = db.Order.ToList();
            ViewBag.giysiTurleri = db.GiysiTurleri.ToList();
            ViewBag.olcuTurleri = db.OlcuTurleri.ToList();

            return View();
        }

        public ActionResult CogaltmaOlcu()
        {
            return View();
        }
    }
}