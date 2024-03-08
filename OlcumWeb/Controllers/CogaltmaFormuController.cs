using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class CogaltmaFormuController : Controller
    {
        OlcumContext db = new OlcumContext();

        // GET: CogaltmaFormu
        public ActionResult Index(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public PartialViewResult OrderDetay()
        {
            ViewBag.olcuTurleri = db.OlcuTurleri.Where(x => x.tabloTuru == "Çoğaltma").ToList();

            ViewBag.giysiTurleri = db.GiysiTurleri.ToList();
            FasonController deneme = new FasonController();

            List<DTOKaliteAtolye> atolyeler = deneme.getAtolyes();

            ViewBag.atolyeler = atolyeler;

            ViewBag.order = db.Order.ToList();

            return PartialView();
        }

        //public PartialViewResult Bedenler(int DetayID, List<string> bedenList)
        //{
        //    return PartialView(Tuple.Create(DetayID, bedenList));
        //}

        //public JsonResult CheckExist(int orderNo, int olcuTuru)
        //{
        //    int orderID = orderNo;
        //    int olcuTuruID = olcuTuru;
        //    CogaltmaDetay detay = db.CogaltmaDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID).FirstOrDefault();
        //    Order selectedOrder = db.Order.Where(x => x.ID == orderID).FirstOrDefault();

        //    DTONumuneTablo tabloDto = new DTONumuneTablo();

        //    if (detay == null)
        //    {
        //        tabloDto.orderID = orderID;
        //        tabloDto.olcuTuruID = olcuTuruID;
        //        tabloDto.modelAd = selectedOrder.modelAd;
        //        tabloDto.musteri = selectedOrder.musteri;
        //        tabloDto.kumas = selectedOrder.kumas;
        //        tabloDto.dikimNedeni = db.OlcuTurleri.Where(x => x.id == olcuTuruID).Select(x => x.olcuTuruAd).FirstOrDefault();
        //        return new JsonResult { Data = tabloDto, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //    else
        //    {
        //        tabloDto.id = detay.id;
        //        tabloDto.orderID = detay.orderID;
        //        tabloDto.giysiTuruID = detay.giysiTuruID;
        //        tabloDto.olcuTuruID = detay.olcuTuruID;
        //        tabloDto.modelAd = selectedOrder.modelAd;
        //        tabloDto.topNo = detay.topNo;
        //        tabloDto.musteri = selectedOrder.musteri;
        //        tabloDto.atolyeID = detay.atolyeID;
        //        tabloDto.kumas = selectedOrder.kumas;
        //        tabloDto.kalipAdi = detay.kalipAdi;
        //        tabloDto.yikamayaGidis = detay.yikamayaGidis;
        //        tabloDto.yikamadanGelis = detay.yikamadanGidis;
        //        tabloDto.kesimeGidis = detay.kesimeGidis;
        //        tabloDto.dikimeGidis = detay.dikimeGidis;

        //        return new JsonResult { Data = tabloDto, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //}

        //public PartialViewResult Tablolar(string beden)
        //{
        //    ViewBag.beden = beden;
        //    return PartialView();
        //}

        //public PartialViewResult TabloVar(int orderID, int olcuTuru, int giysiTuru)
        //{
        //    List<CogaltmaDetay> detay = db.CogaltmaDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuru && x.giysiTuruID == giysiTuru).ToList();
        //    //Burası beden beden ayrılacak şekilde düzenlenecek.
        //    List<List<List<DTONumuneTabloGetir>>> deneme = new List<List<List<DTONumuneTabloGetir>>>();
        //    List<string> adSoyadList = new List<string>();

        //    foreach (var item in detay)
        //    {
        //        List<int> bedenIDler = db.CogaltmaAra.Where(x => x.cogaltmaDetayID == item.id).Select(x => x.bedenID).Distinct().ToList();
        //        List<List<DTONumuneTabloGetir>> listgetirTablos = new List<List<DTONumuneTabloGetir>>();

        //        foreach (var item2 in bedenIDler)
        //        {
        //            List<DTONumuneTabloGetir> getirTablos = new List<DTONumuneTabloGetir>();
        //            List<CogaltmaAra> araTablo = db.CogaltmaAra.Where(x => x.cogaltmaDetayID == item.id && x.bedenID == item2).ToList();

        //            foreach (var item3 in araTablo)
        //            {
        //                DTONumuneTabloGetir getirTablo = new DTONumuneTabloGetir();
        //                getirTablo.id = db.NumuneAra.Where(x => x.id == item3.id).Select(x => x.id).FirstOrDefault();
        //                getirTablo.olcuNoktasi = db.OlcuNoktalari.Where(x => x.id == item3.id).Select(x => x.olcuNoktasi).FirstOrDefault();
        //                getirTablo.beden = db.Bedenler.Where(x => x.ID == item2).Select(x => x.beden).FirstOrDefault();
        //                getirTablo.deger.Add(item3.kalipOlcusu.ToString());
        //                getirTablo.deger.Add(item3.tolerans.ToString());
        //                getirTablo.deger.Add(item3.yoOlcu.ToString());
        //                getirTablo.deger.Add(item3.yoFark.ToString());
        //                getirTablo.deger.Add(item3.yoOlculen.ToString());
        //                getirTablo.deger.Add(item3.cekme.ToString());
        //                getirTablo.deger.Add(item3.ysOlculen.ToString());
        //                getirTablo.deger.Add(item3.ysFark.ToString());
        //                getirTablo.deger.Add(item3.ysOlcu.ToString());

        //                getirTablos.Add(getirTablo);
        //            }
        //            listgetirTablos.Add(getirTablos);
        //        }
        //        deneme.Add(listgetirTablos);

        //        string adSoyad = db.PersonelTablo.Where(x => x.id == item.kullaniciID).Select(x => x.personelAd).FirstOrDefault() + " " + db.PersonelTablo.Where(x => x.id == item.kullaniciID).Select(x => x.personelSoyad).FirstOrDefault();
        //        adSoyadList.Add(adSoyad);


        //    }
        //    return PartialView(Tuple.Create(detay, deneme, adSoyadList));
        //}

        //public PartialViewResult TabloYok(int detayID, List<string> bedenLer)
        //{
        //    HazirTabloDetay detayTablo = db.HazirTabloDetay.Where(x => x.id == detayID).FirstOrDefault();
        //    List<HazirTabloAra> araTablolar = db.HazirTabloAra.Where(x => x.hazirtabloID == detayID).ToList();
        //    List<Bedenler> bedenler = db.Bedenler.ToList();
        //    List<OlcuNoktalari> olcuNoktalari = db.OlcuNoktalari.ToList();
        //    List<DTONumuneHazirTablo> tablo = new List<DTONumuneHazirTablo>();

        //    foreach (var item2 in araTablolar)
        //    {
        //        DTONumuneHazirTablo dto = new DTONumuneHazirTablo();

        //        dto.id = item2.id;
        //        dto.beden = bedenler.Where(x => x.ID == item2.bedenID).Select(x => x.beden).FirstOrDefault();
        //        dto.olcuNoktasi = olcuNoktalari.Where(x => x.id == item2.olcuNoktaID).Select(x => x.olcuNoktasi).FirstOrDefault();
        //        dto.deger = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID && x.bedenID == item2.bedenID).Select(x => x.deger).FirstOrDefault();

        //        tablo.Add(dto);
        //    }

        //    return PartialView(Tuple.Create(tablo, bedenLer));
        //}

        //public PartialViewResult HazirTabloModal(int orderID)
        //{
        //    List<Tuple<int, string>> modeller = new List<Tuple<int, string>>();

        //    string fit = db.Order.Where(x => x.ID == orderID).Select(x => x.fit).FirstOrDefault();

        //    if (String.IsNullOrEmpty(fit))
        //    {
        //        string musteriAd = db.Order.Where(x => x.ID == orderID).Select(x => x.musteri).FirstOrDefault();

        //        musteriAd = musteriAd.Split(' ')[0];

        //        if(musteriAd == "BESTSELLER")
        //        {
        //            musteriAd = "JACK & JONES";
        //        }

        //        int musteriID = db.Musteriler.Where(x => x.musteriAd.Contains(musteriAd)).Select(x => x.id).FirstOrDefault();

        //        List<int> fitler = db.Modeller.Where(x => x.musteriID == musteriID).Select(x => x.id).ToList();


        //        foreach (var item in fitler)
        //        {
        //            string fitAd = db.Modeller.Where(x => x.id == item).Select(x => x.modelAd).FirstOrDefault();

        //            bool isExist = db.HazirTabloDetay.Any(x => x.modelID == item);

        //            Tuple<int, string> model = null;

        //            if (isExist)
        //            {
        //                model = Tuple.Create(item, fitAd);
        //                modeller.Add(model);
        //            }

        //        }
        //    }
        //    else
        //    {
        //        int modelID = db.Modeller.Where(x => x.modelAd == fit).Select(x => x.id).FirstOrDefault();

        //        Tuple<int, string> model = Tuple.Create(modelID, fit);

        //        modeller.Add(model);
        //    }
                       
        //    return PartialView(modeller);
        //}

        //public PartialViewResult InnerHazirTablo(int modelID)
        //{

        //    List<HazirTabloDetay> detaylar = db.HazirTabloDetay.Where(x => x.modelID == modelID).OrderByDescending(c => c.tarih).ToList();
        //    List<Bedenler> bedenler = db.Bedenler.ToList();
        //    List<OlcuNoktalari> olcuNoktalari = db.OlcuNoktalari.ToList();

        //    List<List<Models.DTO.DTOHazirTablo>> tablolar = new List<List<Models.DTO.DTOHazirTablo>>();


        //    foreach (var item in detaylar)
        //    {
        //        List<Models.DTO.DTOHazirTablo> tablo = new List<Models.DTO.DTOHazirTablo>();
        //        List<HazirTabloAra> hazirTabloAras = db.HazirTabloAra.Where(x => x.hazirtabloID == item.id).ToList();

        //        foreach (var item2 in hazirTabloAras)
        //        {
        //            Models.DTO.DTOHazirTablo dto = new Models.DTO.DTOHazirTablo();
        //            dto.id = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.id).ToList();
        //            dto.beden = bedenler.Where(x => x.ID == item2.bedenID).Select(x => x.beden).FirstOrDefault();
        //            dto.olcuNoktasi = olcuNoktalari.Where(x => x.id == item2.olcuNoktaID).Select(x => x.olcuNoktasi).FirstOrDefault();
        //            dto.deger = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.deger).ToList();

        //            tablo.Add(dto);
        //        }
        //        tablolar.Add(tablo);

        //    }

        //    return PartialView(Tuple.Create(detaylar, tablolar));
        //}

        //public JsonResult TabloKaydet(DTONumuneDetay detay, List<List<DTONumuneTabloKaydet>> tablo,int kullanıcıID)
        //{
        //    try
        //    {
        //        //Numune Detayı kaydettiğim kısım. Null değerler düzeltilmeli.
        //        CogaltmaDetay detayTablo = new CogaltmaDetay();

        //        detayTablo.orderID = detay.orderID;
        //        detayTablo.atolyeID = (int)detay.atolye;
        //        detayTablo.giysiTuruID = detay.giysiTuru;
        //        detayTablo.olcuTuruID = detay.olcuTuru;
        //        detayTablo.kesimeGidis = Convert.ToDateTime(detay.kesimeGidis);
        //        detayTablo.dikimeGidis = Convert.ToDateTime(detay.dikimeGidis);
        //        detayTablo.yikamayaGidis = Convert.ToDateTime(detay.yikamayaGidis);
        //        detayTablo.yikamadanGidis = Convert.ToDateTime(detay.yikamadanGelis);
        //        detayTablo.kalipAdi = detay.kalipAdi;
        //        detayTablo.topNo = detay.topNo;
        //        detayTablo.olcumNo = 1;
        //        detayTablo.aciklama = null;
        //        detayTablo.tarih = DateTime.Now;
        //        detayTablo.kullaniciID = kullanıcıID;
        //        detayTablo.receteKod = null;

        //        db.CogaltmaDetay.Add(detayTablo);
        //        db.SaveChanges();

        //        foreach (var item in tablo)
        //        {
        //            for (int i = 0; i < item.Count; i++)
        //            {
        //                CogaltmaAra ara = new CogaltmaAra();

        //                string olcuNoktasi = item[i].olcuNoktasi;
        //                string beden = item[i].beden;

        //                ara.cogaltmaDetayID = detayTablo.id;
        //                ara.olcuNoktaID = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();
        //                ara.bedenID = db.Bedenler.Where(x => x.beden == beden).Select(x => x.ID).FirstOrDefault();
        //                ara.kalipOlcusu = Convert.ToDouble(item[i].deger[0].Replace('.', ','));
        //                ara.tolerans = Convert.ToDouble(item[i].deger[1].Replace('.', ','));
        //                ara.yoOlcu = Convert.ToDouble(item[i].deger[2].Replace('.', ','));
        //                ara.yoFark = Convert.ToDouble(item[i].deger[3].Replace('.', ','));
        //                ara.yoOlculen = Convert.ToDouble(item[i].deger[4].Replace('.', ','));
        //                ara.cekme = Convert.ToDouble(item[i].deger[5].Replace('.', ','));
        //                ara.ysOlculen = Convert.ToDouble(item[i].deger[6].Replace('.', ','));
        //                ara.ysFark = Convert.ToDouble(item[i].deger[7].Replace('.', ','));
        //                ara.ysOlcu = Convert.ToDouble(item[i].deger[8].Replace('.', ','));
        //                ara.mudahaleMi = false;
        //                db.CogaltmaAra.Add(ara);
        //                db.SaveChanges();
        //            }
        //        }


        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(0, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult TabloGuncelle(DTONumuneDetay detay, List<List<DTONumuneTabloKaydet>> tablo)
        //{
        //    try
        //    {
        //        var check = db.CogaltmaDetay.Where(x => x.id == detay.id).FirstOrDefault();
        //        if (check != null)
        //        {
        //            check.atolyeID = (int)detay.atolye;
        //            check.giysiTuruID = detay.giysiTuru;
        //            check.olcuTuruID = detay.olcuTuru;
        //            check.kesimeGidis = Convert.ToDateTime(detay.kesimeGidis);
        //            check.dikimeGidis = Convert.ToDateTime(detay.dikimeGidis);
        //            check.yikamayaGidis = Convert.ToDateTime(detay.yikamayaGidis);
        //            check.kalipAdi = detay.kalipAdi;
        //            check.topNo = detay.topNo;                  
        //            check.olcumNo = 1;
        //            check.aciklama = null;
        //            check.tarih = DateTime.Now;
        //            check.kullaniciID = Convert.ToInt32(User.Identity.Name);
        //            check.receteKod = detay.receteKod;

        //            db.SaveChanges();

        //            foreach (var item in tablo)
        //            {
        //                for (int i = 0; i < item.Count; i++)
        //                {
        //                    var id = item[i].id;
        //                    var ara = db.CogaltmaAra.Where(x => x.id == id).FirstOrDefault();

        //                    if (ara != null)
        //                    {
        //                        string olcuNoktasi = item[i].olcuNoktasi;
        //                        string beden = item[i].beden;
        //                        ara.cogaltmaDetayID = check.id;
        //                        ara.olcuNoktaID = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();
        //                        ara.bedenID = db.Bedenler.Where(x => x.beden == beden).Select(x => x.ID).FirstOrDefault();
        //                        ara.kalipOlcusu = Convert.ToDouble(item[i].deger[0].Replace('.', ','));
        //                        ara.tolerans = Convert.ToDouble(item[i].deger[1].Replace('.', ','));
        //                        ara.yoOlcu = Convert.ToDouble(item[i].deger[2].Replace('.', ','));
        //                        ara.yoFark = Convert.ToDouble(item[i].deger[3].Replace('.', ','));
        //                        ara.yoOlculen = Convert.ToDouble(item[i].deger[4].Replace('.', ','));
        //                        ara.cekme = Convert.ToDouble(item[i].deger[5].Replace('.', ','));
        //                        ara.ysOlculen = Convert.ToDouble(item[i].deger[6].Replace('.', ','));
        //                        ara.ysFark = Convert.ToDouble(item[i].deger[7].Replace('.', ','));
        //                        ara.ysOlcu = Convert.ToDouble(item[i].deger[8].Replace('.', ','));
        //                        ara.mudahaleMi = false;
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //        }

        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(0, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult ChangeTab(int detayID)
        //{
        //    CogaltmaDetay detay = db.CogaltmaDetay.Where(x => x.id == detayID).FirstOrDefault();
        //    DTONumuneTablo tabloDto = new DTONumuneTablo();

        //    tabloDto.id = detay.id;
        //    tabloDto.orderID = detay.orderID;
        //    tabloDto.giysiTuruID = detay.giysiTuruID;
        //    tabloDto.olcuTuruID = detay.olcuTuruID;
        //    tabloDto.topNo = detay.topNo;
        //    tabloDto.atolyeID = detay.atolyeID;
        //    tabloDto.kalipAdi = detay.kalipAdi;
        //    tabloDto.yikamayaGidis = detay.yikamayaGidis;
        //    tabloDto.kesimeGidis = detay.kesimeGidis;
        //    tabloDto.dikimeGidis = detay.dikimeGidis;
        //    return Json(tabloDto, JsonRequestBehavior.AllowGet);
        //}
    }
}
