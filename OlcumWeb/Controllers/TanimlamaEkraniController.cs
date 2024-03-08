using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class TanimlamaEkraniController : Controller
    {
        OlcumContext db = new OlcumContext();

        public ActionResult Index(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        public PartialViewResult TabloTanimlama()
        {
            return PartialView();
        }

        public PartialViewResult Musteriler()
        {
            List<Musteriler> musteriler = db.Musteriler.ToList();
            return PartialView(musteriler);
        }

        public PartialViewResult Modeller(int musteriID)
        {

            List<Modeller> modeller = db.Modeller.Where(x => x.musteriID == musteriID).ToList();

            List<int> modelHazirTabloVarmi = new List<int>();

            foreach(var item in modeller)
            {
                HazirTabloDetay detay = db.HazirTabloDetay.Where(x => x.modelID == item.id).FirstOrDefault();

                if(detay == null)
                {
                    modelHazirTabloVarmi.Add(0);
                }
                else
                {
                    modelHazirTabloVarmi.Add(1);
                }
            }
            return PartialView(Tuple.Create(modeller,modelHazirTabloVarmi));
        }

        public PartialViewResult HazirTablo(int modelID)
        {
            string modelAd = db.Modeller.Where(x => x.id == modelID).Select(x => x.modelAd).FirstOrDefault();

            string giysiTuru = db.Order.Where(x => x.fit == modelAd && x.giysiTuru != null).Select(x => x.giysiTuru).FirstOrDefault();

            int giysiTuruID = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuru).Select(x => x.id).FirstOrDefault();

            List<OlcuNoktalari> olcuNoktalaris = db.OlcuNoktalari.Where(x=>x.giysiTuruID == giysiTuruID && x.siraNo != null).OrderBy(x=>x.siraNo).ToList();

            List<OlcumWeb.Models.DTO.DTOOlcuNoktalari> deneme = new List<Models.DTO.DTOOlcuNoktalari>();

            foreach(var item in olcuNoktalaris)
            {
                Models.DTO.DTOOlcuNoktalari olcuNoktasi = new Models.DTO.DTOOlcuNoktalari();
                olcuNoktasi.id = item.id;
                olcuNoktasi.olcuNoktasi = item.olcuNoktasi;
                olcuNoktasi.tabanID = item.tabanID;
                olcuNoktasi.anaNoktami = item.anaNoktami;
                deneme.Add(olcuNoktasi);
            }
            
            return PartialView(Tuple.Create(deneme,modelID));
        }

        public PartialViewResult HazirTabloVar(int modelID)
        {
            List<HazirTabloDetay> detayTabloList = db.HazirTabloDetay.Where(x => x.modelID == modelID).OrderByDescending(c => c.tarih).ToList();
            
            List<Bedenler> bedenler = db.Bedenler.ToList();
            List<OlcuNoktalari> olcuNoktalari = db.OlcuNoktalari.ToList();

            ViewBag.olcuNoktalari = olcuNoktalari;

            List<List<Models.DTO.DTOHazirTablo>> tablolar = new List<List<Models.DTO.DTOHazirTablo>>();
            foreach(var item in detayTabloList)
            {
                List<Models.DTO.DTOHazirTablo> tablo = new List<Models.DTO.DTOHazirTablo>();
                List<HazirTabloAra> hazirTabloAras = db.HazirTabloAra.Where(x => x.hazirtabloID == item.id).OrderBy(x=>x.satirIndexi).ToList();

                foreach (var item2 in hazirTabloAras)
                {
                    Models.DTO.DTOHazirTablo dto = new Models.DTO.DTOHazirTablo();
                    dto.id = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.id).ToList();
                    dto.beden = bedenler.Where(x => x.ID == item2.bedenID).Select(x => x.beden).FirstOrDefault();
                    dto.bedenID = item2.bedenID;
                    dto.tabanID = olcuNoktalari.Where(x => x.id == item2.olcuNoktaID).Select(x => x.tabanID).FirstOrDefault();
                    dto.olcuNoktasi = olcuNoktalari.Where(x => x.id == item2.olcuNoktaID).Select(x => x.olcuNoktasi).FirstOrDefault();
                    dto.olcuNoktasiID = item2.olcuNoktaID;
                    dto.deger = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.deger).ToList();

                    tablo.Add(dto);


                }
                tablolar.Add(tablo);
            }


            return PartialView(Tuple.Create(detayTabloList,tablolar));
        }

        public PartialViewResult HazirTabloDuzenle(int detayID)
        {
            HazirTabloDetay detayTabloList = db.HazirTabloDetay.Where(x=>x.id == detayID).FirstOrDefault();

            List<Bedenler> bedenler = db.Bedenler.ToList();
            List<OlcuNoktalari> olcuNoktalari = db.OlcuNoktalari.ToList();

            List<Models.DTO.DTOHazirTablo> tablo = new List<Models.DTO.DTOHazirTablo>();
            List<HazirTabloAra> hazirTabloAras = db.HazirTabloAra.Where(x => x.hazirtabloID == detayID).ToList();

            foreach (var item2 in hazirTabloAras)
            {
                Models.DTO.DTOHazirTablo dto = new Models.DTO.DTOHazirTablo();
                dto.id = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.id).ToList();
                dto.beden = bedenler.Where(x => x.ID == item2.bedenID).Select(x => x.beden).FirstOrDefault();
                dto.olcuNoktasi = olcuNoktalari.Where(x => x.id == item2.olcuNoktaID).Select(x => x.olcuNoktasi).FirstOrDefault();
                dto.deger = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.deger).ToList();

                tablo.Add(dto);
            }


            return PartialView(Tuple.Create(detayTabloList,tablo));
        }

        public JsonResult CheckTabloExist(int modelID)
        {
            HazirTabloDetay detay = db.HazirTabloDetay.Where(x => x.modelID == modelID).FirstOrDefault();

            if(detay == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);

            }

        }

        public PartialViewResult Bedenler(int bedenSistemi,int selectedIndex)
        {
            //List<Bedenler> isimBedenler = db.Bedenler.GroupBy(x => x.ID).Select(y => y.FirstOrDefault()).Where(x => x.bedenSistemi == bedenSistemi).ToList();

            List<Bedenler> bedenler;

            if(bedenSistemi == 0)
            {
                bedenler = db.Bedenler.GroupBy(x => x.ID).Select(y => y.FirstOrDefault()).Where(x => x.bedenSistemi == bedenSistemi).ToList();
            }
            else
            {
                bedenler = db.Bedenler.GroupBy(x => x.ID).Select(y => y.FirstOrDefault()).Where(x => x.bedenSistemi == bedenSistemi).OrderBy(x =>x.beden).ToList();
            }

            return PartialView(bedenler);
        }

        public JsonResult TabloKaydet(int modelID,List<OlcumWeb.Models.DTO.DTOHazirTabloKaydet> tablo)
        {
            try
            {
                int KulId = 10;

                //if (User.Identity.Name == null)
                //{
                //    KulId = 16;
                //}
                //else
                //{
                //    KulId = Convert.ToInt32(User.Identity.Name.ToString());
                //}


                HazirTabloDetay detay = new HazirTabloDetay();

                detay.tarih = DateTime.Now;
                detay.kullanıcıID = KulId;
                detay.modelID = modelID;

                db.HazirTabloDetay.Add(detay);
                db.SaveChanges();


                int satirIndexi = 0;

                int detayID = detay.id;
                
                for(int i = 0; i < tablo.Count; i++)
                {
                    HazirTabloAra ara = new HazirTabloAra();
                    int tempOlcuNoktaid = tablo[i].olcuNoktaID;

                    ara.hazirtabloID = detayID;
                    ara.olcuNoktaID = tablo[i].olcuNoktaID;
                    ara.bedenID = tablo[i].bedenID;
                    ara.deger = Math.Round(Convert.ToDouble(tablo[i].deger.Replace(".", ",")), 2, MidpointRounding.AwayFromZero);
                    ara.satirIndexi = satirIndexi;

                    if (tablo.Count > i + 1)
                    {
                        if (tempOlcuNoktaid != tablo[i + 1].olcuNoktaID)
                            satirIndexi++;
                    }

                    db.HazirTabloAra.Add(ara);
                    db.SaveChanges();
                }

                return Json(1,JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(0,JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult TabloGuncelle(int detayID, List<OlcumWeb.Models.DTO.DTOHazirTabloKaydet> tablo)
        {
            try
            {
                HazirTabloDetay detay = db.HazirTabloDetay.Where(x => x.id == detayID).FirstOrDefault();

                int KulId = 10;

                //if (User.Identity.Name == null)
                //{
                //    KulId = 16;
                //}
                //else
                //{
                //    KulId = Convert.ToInt32(User.Identity.Name.ToString());
                //}


                detay.tarih = DateTime.Now;
                detay.kullanıcıID = KulId;
                db.SaveChanges();

                int satirIndexi = 0;


                for(int i = 0; i < tablo.Count; i++)
                {
                    int araID = tablo[i].araID;

                    HazirTabloAra ara = db.HazirTabloAra.Where(x => x.id == araID).FirstOrDefault();

                    if (ara != null)
                    {
                        int tempOlcuNoktaid = tablo[i].olcuNoktaID;
                        ara.olcuNoktaID = tablo[i].olcuNoktaID;
                        ara.bedenID = tablo[i].bedenID;
                        ara.deger = Convert.ToDouble(tablo[i].deger.Replace('.', ','));
                        ara.satirIndexi = satirIndexi;

                        if (tablo.Count > i + 1)
                        {
                            if (tempOlcuNoktaid != tablo[i + 1].olcuNoktaID)
                                satirIndexi++;
                        }


                    }
                    else
                    {
                        int tempOlcuNoktaid = tablo[i].olcuNoktaID;

                        ara.hazirtabloID = detay.id;
                        ara.olcuNoktaID = tablo[i].olcuNoktaID;
                        ara.bedenID = tablo[i].bedenID;
                        ara.deger = Convert.ToDouble(tablo[i].deger.Replace('.', ','));
                        ara.satirIndexi = satirIndexi;

                        if (tablo.Count > i + 1)
                        {
                            if (tempOlcuNoktaid != tablo[i + 1].olcuNoktaID)
                                satirIndexi++;
                        }
                    }
                }
                db.SaveChanges();


                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);

            }

        }

        public PartialViewResult BreadCrumb(int sayfaID,int musteriID,int modelID,int hazirtabloID)
        {

            if(sayfaID == 0) // AnaSayfa
            {
                musteriID = 0;
                modelID = 0;
                hazirtabloID = 0;
                return PartialView(Tuple.Create(sayfaID,musteriID,modelID,hazirtabloID));
            }
            else if(sayfaID == 1) // Tanımlama Ekranı
            {
                musteriID = 0;
                modelID = 0;
                hazirtabloID = 0;
                return PartialView(Tuple.Create(sayfaID, musteriID, modelID, hazirtabloID));
            }
            else if(sayfaID == 2) // Ölçü Tablosu Tanımlama
            {
                musteriID = 0;
                modelID = 0;
                hazirtabloID = 0;
                return PartialView(Tuple.Create(sayfaID, musteriID, modelID, hazirtabloID));
            }
            else if(sayfaID == 3)// Müşteriler
            {
                modelID = 0;
                hazirtabloID = 0;
                return PartialView(Tuple.Create(sayfaID, musteriID, modelID, hazirtabloID));
            }
            else if(sayfaID == 4)// Modeller
            {

                hazirtabloID = 0;
                return PartialView(Tuple.Create(sayfaID, musteriID, modelID, hazirtabloID));
            }
            else if(sayfaID == 5)// Hazir Tablo ismi
            {
                return PartialView(Tuple.Create(sayfaID, musteriID, modelID, hazirtabloID));
            }
            else
            {
                musteriID = 0;
                modelID = 0;
                hazirtabloID = 0;
                return PartialView(Tuple.Create(sayfaID, musteriID, modelID, hazirtabloID));
            }          
        }
    }
}