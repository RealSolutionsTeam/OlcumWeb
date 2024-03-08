using OlcumWeb.dbOlcum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OlcumWeb.Models.DTO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using OlcumWeb.Araclar;

namespace OlcumWeb.Controllers
{
    public class ImalatTablosuController : Controller
    {
        OlcumContext db = new OlcumContext();


        // GET: ImalatTablosu
        public ActionResult Index(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        public PartialViewResult OrderDetay(int id)
        {
            ViewBag.olcuTurleri = db.OlcuTurleri.Where(x => x.tabloTuru == "Ölçü Tablosu").ToList();
            ViewBag.order = db.Order.Where(x => x.orderSafhasi == "İMALAT").ToList();

            PersonelTablo personel = db.PersonelTablo.Where(x => x.id == id).FirstOrDefault();
            ViewBag.ad = personel.personelAd;
            ViewBag.soyAd = personel.personelSoyad;

            return PartialView();
        }

        public JsonResult CheckExist(int orderID, int olcuTuruID)
        {
            Order selectedOrder = db.Order.Where(x => x.ID == orderID).FirstOrDefault();

            string giysiTuru = selectedOrder.giysiTuru;

            OlcuTabloDetay detay = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.GiysiTurleri.giysiTuruAd == giysiTuru && x.olcuTuruID == olcuTuruID).FirstOrDefault();

            

            DTOImalatTabloDetay tabloDto = new DTOImalatTabloDetay();

            if (detay == null)
            {
                tabloDto.orderID = orderID;
                tabloDto.modelAd = selectedOrder.modelAd;
                tabloDto.musteri = selectedOrder.musteri;
                tabloDto.kumas = selectedOrder.kumas;
                tabloDto.artikel = selectedOrder.modelNo + " " + selectedOrder.modelAd;
                return Json(tabloDto, JsonRequestBehavior.AllowGet);
            }
            else
            {
                tabloDto.id = detay.id;
                tabloDto.orderID = orderID;
                tabloDto.olcuTuruID = olcuTuruID;
                tabloDto.modelAd = selectedOrder.modelAd;
                tabloDto.musteri = selectedOrder.musteri;
                tabloDto.kumas = selectedOrder.kumas;
                tabloDto.artikel = selectedOrder.modelNo + " " + selectedOrder.modelAd;
                tabloDto.aciklama = detay.aciklama;
                tabloDto.kullanıcıAdı = detay.PersonelTablo.personelAd + " " + detay.PersonelTablo.personelSoyad;
                tabloDto.tarih = (DateTime)detay.tarih;
                return Json(tabloDto, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ImalatAnaSayfa(int checkExist)
        {
            ViewBag.checkExist = checkExist;

            return PartialView("ImalatTabloGoster");
        }

        public PartialViewResult YikamaSonrasiTablo(int orderID, int olcuTuruID, int checkExist, int hazirtabloID = 0)
        {
            ViewBag.checkExist = checkExist;
            DTOImalatTabloDetay detay = new DTOImalatTabloDetay();
            List<DTOImalatTabloGoster> tablo = new List<DTOImalatTabloGoster>();

            if (checkExist == 0)
            {
                List<HazirTabloAra> hazirTabloAras = db.HazirTabloAra.Where(x => x.hazirtabloID == hazirtabloID).GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id).ToList();

                List<string> bedenler = new List<string>();

                List<int> bedenid = db.HazirTabloAra.Where(x => x.hazirtabloID == hazirtabloID).Select(x => x.bedenID).Distinct().ToList();
                ViewBag.anaBeden = "";
                if (hazirTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 0)
                {
                    foreach (var item in bedenid)
                    {
                        string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }

                }
                else if (hazirTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 1)
                {
                    foreach (var item in bedenid)
                    {
                        string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }

                    bedenler = bedenler.OrderBy(x => x).ToList();
                }
                else if (hazirTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 2)
                {
                    foreach (var item in bedenid)
                    {
                        string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }
                }
                else
                {
                    foreach (var item in bedenid)
                    {
                        string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }
                }

                foreach (var item in hazirTabloAras)
                {
                    DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                    ara.id = item.id;
                    int olcuNoktasiID = item.olcuNoktaID;
                    ara.olcuNoktasi = db.OlcuNoktalari.Where(x => x.id == olcuNoktasiID).Select(x => x.olcuNoktasi).FirstOrDefault();
                    ara.olcuNoktaId = olcuNoktasiID;
                    int bedenID = item.bedenID;
                    ara.beden = bedenler;
                    ara.bedenId = bedenid;
                    ara.deger = db.HazirTabloAra.Where(x => x.olcuNoktaID == olcuNoktasiID && x.hazirtabloID == hazirtabloID).Select(x => x.deger.ToString()).ToList();

                    tablo.Add(ara);
                }

                return PartialView("YikamaSonrasiGoster", Tuple.Create(tablo));
            }
            else
            {
                int olcuTabloDetaydetayid = db.OlcuTabloDetay.Where(x => x.orderID == orderID  && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.id).FirstOrDefault();
                int anaBedenID = db.OlcuTabloDetay.Where(x => x.orderID == orderID  && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.anaBedenID).FirstOrDefault();
                int kullanıcıID = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.kullaniciID).FirstOrDefault();


                string anaBeden = db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault();

                ViewBag.anaBeden = anaBeden;

                string kullanıcıAdSoyad = db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelAd).FirstOrDefault() + " " + db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelSoyad).FirstOrDefault();

                ViewBag.personel = kullanıcıAdSoyad;

                List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloDetaydetayid).ToList();

                List<string> bedenler = new List<string>();

                List<int> bedenid = olcuTabloAras.Where(x => x.olcuTabloID == olcuTabloDetaydetayid).Select(x => x.bedenID).Distinct().ToList();

                if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 0)
                {
                    foreach (var item in bedenid)
                    {
                        string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }

                }
                else if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 1)
                {
                    foreach (var item in bedenid)
                    {
                        string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }

                    bedenler = bedenler.OrderBy(x => x).ToList();
                }
                else if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 2)
                {
                    foreach (var item in bedenid)
                    {
                        string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }
                }
                else
                {
                    foreach (var item in bedenid)
                    {
                        string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                        bedenler.Add(beden);
                    }
                }

                foreach (var item in olcuTabloAras.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id).ToList())
                {

                    DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                    ara.id = item.id;
                    int olcuNoktasiID = item.olcuNoktaID;
                    ara.olcuNoktasi = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID).Select(x => x.OlcuNoktalari.olcuNoktasi).FirstOrDefault();
                    ara.olcuNoktaId = olcuNoktasiID;
                    int bedenID = item.bedenID;
                    ara.beden = bedenler;
                    ara.bedenId = bedenid;
                    ara.deger = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == olcuTabloDetaydetayid).Select(x => x.deger.ToString()).ToList();

                    tablo.Add(ara);
                }

                return PartialView("YikamaSonrasiGoster", Tuple.Create(tablo));
            }
        }

        public PartialViewResult YikamaOncesiTablo(int orderID, int olcuTuruID, int checkExist, int hazirtabloID = 0, string anaBeden = "", List<List<DTOImalatTabloGoster>> previousTablo = null, List<string> cekmeler = null)
        {
            List<List<DTOImalatTabloGoster>> tablo = new List<List<DTOImalatTabloGoster>>();

            ViewBag.checkExist = checkExist;
            if (checkExist == 0)
            {
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
                return PartialView("YikamaOncesiGoster", Tuple.Create(tablo));
            }
            else if (checkExist == 1)
            {
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
                tablo = previousTablo;
                return PartialView("YikamaOncesiGoster", Tuple.Create(tablo));
            }
            else
            {
                List<OlcuTabloDetay> detayList = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID&& x.tabloTuru == 0).ToList();

                if(detayList.Count > 0)
                {
                    int firstID = detayList[0].id;

                    int anaBedenID = detayList[0].anaBedenID;

                    ViewBag.anaBeden = db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault();

                    List<string> enBoyCekmeler = detayList.Select(x => x.enBoyCekme).ToList();
                    ViewBag.cekmeler = enBoyCekmeler;

                    List<string> bedenler = new List<string>();

                    List<int> bedenid = db.OlcuTabloAra.Where(x => x.olcuTabloID == firstID).Select(x => x.bedenID).Distinct().ToList();
                    int firstBedenID = bedenid[0];

                    if (db.Bedenler.Where(x => x.ID == firstBedenID).Select(x => x.bedenSistemi).FirstOrDefault() == 0)
                    {
                        foreach (var item in bedenid)
                        {
                            string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }

                    }
                    else if (db.Bedenler.Where(x => x.ID == firstBedenID).Select(x => x.bedenSistemi).FirstOrDefault() == 1)
                    {
                        foreach (var item in bedenid)
                        {
                            string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }

                        bedenler = bedenler.OrderBy(x => x).ToList();
                    }
                    else if (db.Bedenler.Where(x => x.ID == firstBedenID).Select(x => x.bedenSistemi).FirstOrDefault() == 2)
                    {
                        foreach (var item in bedenid)
                        {
                            string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }
                    }
                    else
                    {
                        foreach (var item in bedenid)
                        {
                            string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }
                    }


                    foreach (var item2 in detayList)
                    {
                        List<OlcuTabloAra> araList = db.OlcuTabloAra.Where(x => x.olcuTabloID == item2.id).ToList();
                        List<OlcuTabloHesaplama> hesaplamaList = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == item2.id).OrderBy(x => x.id).ToList();

                        List<DTOImalatTabloGoster> dto = new List<DTOImalatTabloGoster>();
                        foreach (var item in araList.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id).ToList())
                        {
                            DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                            ara.id = item.id;
                            int olcuNoktasiID = item.olcuNoktaID;
                            ara.olcuNoktasi = araList.Where(x => x.olcuNoktaID == item.olcuNoktaID).Select(x => x.OlcuNoktalari.olcuNoktasi).FirstOrDefault();
                            int bedenID = item.bedenID;
                            ara.beden = bedenler;
                            ara.deger = araList.Where(x => x.olcuNoktaID == olcuNoktasiID).Select(x => x.deger.ToString()).ToList();
                            ara.tolerans = hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.olcuTabloID).Select(x => x.tolerans.ToString()).FirstOrDefault();
                            ara.cekme = hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.olcuTabloID).Select(x => x.cekme.ToString()).FirstOrDefault();
                            ara.oran = hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.olcuTabloID).Select(x => x.oran.ToString()).FirstOrDefault();

                            dto.Add(ara);
                        }

                        tablo.Add(dto);
                    }

                }
                else
                {
                    ViewBag.checkExist = 0;
                    ViewBag.anaBeden = anaBeden;
                    ViewBag.cekmeler = cekmeler;
                    return PartialView("YikamaOncesiGoster", Tuple.Create(tablo));
                }


                return PartialView("YikamaOncesiGoster", Tuple.Create(tablo));
            }

        }

        public PartialViewResult ModalAnaSayfa(int orderID)
        {
            return PartialView(orderID);
        }

        public PartialViewResult HazirTabloModal(int orderID)
        {
            List<Tuple<int, string>> modeller = new List<Tuple<int, string>>();

            string fit = db.Order.Where(x => x.ID == orderID).Select(x => x.fit).FirstOrDefault();

            if (String.IsNullOrEmpty(fit))
            {
                string musteriAd = db.Order.Where(x => x.ID == orderID).Select(x => x.musteri).FirstOrDefault();

                musteriAd = musteriAd.Split(' ')[0];

                if (musteriAd == "BESTSELLER")
                {
                    musteriAd = "JACK & JONES";
                }

                int musteriID = db.Musteriler.Where(x => x.musteriAd.Contains(musteriAd)).Select(x => x.id).FirstOrDefault();

                List<int> fitler = db.Modeller.Where(x => x.musteriID == musteriID).Select(x => x.id).ToList();


                foreach (var item in fitler)
                {
                    string fitAd = db.Modeller.Where(x => x.id == item).Select(x => x.modelAd).FirstOrDefault();

                    bool isExist = db.HazirTabloDetay.Any(x => x.modelID == item);

                    Tuple<int, string> model = null;

                    if (isExist)
                    {
                        model = Tuple.Create(item, fitAd);
                        modeller.Add(model);

                    }

                }
            }
            else
            {
                int modelID = db.Modeller.Where(x => x.modelAd == fit).Select(x => x.id).FirstOrDefault();

                if (modelID == 0)
                {
                    string musteriAd = db.Order.Where(x => x.ID == orderID).Select(x => x.musteri).FirstOrDefault();

                    musteriAd = musteriAd.Split(' ')[0];

                    if (musteriAd == "BESTSELLER")
                    {
                        musteriAd = "JACK & JONES";
                    }

                    int musteriID = db.Musteriler.Where(x => x.musteriAd.Contains(musteriAd)).Select(x => x.id).FirstOrDefault();

                    List<int> fitler = db.Modeller.Where(x => x.musteriID == musteriID).Select(x => x.id).ToList();


                    foreach (var item in fitler)
                    {
                        string fitAd = db.Modeller.Where(x => x.id == item).Select(x => x.modelAd).FirstOrDefault();

                        bool isExist = db.HazirTabloDetay.Any(x => x.modelID == item);

                        Tuple<int, string> model = null;

                        if (isExist)
                        {
                            model = Tuple.Create(item, fitAd);
                            modeller.Add(model);

                        }

                    }
                }
                else
                {
                    Tuple<int, string> model = Tuple.Create(modelID, fit);
                    modeller.Add(model);

                }

            }



            return PartialView(modeller);
        }

        public PartialViewResult InnerHazirTablo(int modelID)
        {

            List<HazirTabloDetay> detaylar = db.HazirTabloDetay.Where(x => x.modelID == modelID).OrderByDescending(c => c.tarih).ToList();
            List<Bedenler> bedenler = db.Bedenler.ToList();
            List<OlcuNoktalari> olcuNoktalari = db.OlcuNoktalari.ToList();

            List<List<Models.DTO.DTOHazirTablo>> tablolar = new List<List<Models.DTO.DTOHazirTablo>>();


            foreach (var item in detaylar)
            {
                List<Models.DTO.DTOHazirTablo> tablo = new List<Models.DTO.DTOHazirTablo>();
                List<HazirTabloAra> hazirTabloAras = db.HazirTabloAra.Where(x => x.hazirtabloID == item.id).ToList();

                foreach (var item2 in hazirTabloAras)
                {
                    Models.DTO.DTOHazirTablo dto = new Models.DTO.DTOHazirTablo();
                    dto.id = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.id).ToList();
                    dto.beden = bedenler.Where(x => x.ID == item2.bedenID).Select(x => x.beden).FirstOrDefault();
                    dto.olcuNoktasi = olcuNoktalari.Where(x => x.id == item2.olcuNoktaID).Select(x => x.olcuNoktasi).FirstOrDefault();
                    dto.deger = db.HazirTabloAra.Where(x => x.olcuNoktaID == item2.olcuNoktaID && x.hazirtabloID == item2.hazirtabloID).Select(x => x.deger).ToList();

                    tablo.Add(dto);
                }
                tablolar.Add(tablo);

            }

            return PartialView(Tuple.Create(detaylar, tablolar));
        }

        public PartialViewResult CekmeGoster(int orderID, int olcuTuruID, string enBoyCekme, int index)
        {
            string giysiTuru = db.Order.Where(x => x.ID == orderID).Select(x => x.giysiTuru).FirstOrDefault();
            OlcuTabloDetay detaylar = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.GiysiTurleri.giysiTuruAd == giysiTuru && x.olcuTuruID == olcuTuruID && x.tabloTuru == 0 && x.enBoyCekme == enBoyCekme).FirstOrDefault();
            int anaBedenID = detaylar.anaBedenID;
            int kullanıcıID = detaylar.kullaniciID;
            DTOImalatTabloDetay detay1 = new DTOImalatTabloDetay()
            {
                id = detaylar.id,
                orderNo = detaylar.Order.orderNo,
                giysiTuru = detaylar.GiysiTurleri.giysiTuruAd,
                kumas = detaylar.Order.kumas,
                musteri = detaylar.Order.musteri,
                olcuTUru = detaylar.OlcuTurleri.olcuTuruAd,
                anaBeden = db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault(),
                enBoyCekme = detaylar.enBoyCekme,
                tabloTuru = detaylar.tabloTuru,
                aciklama = detaylar.aciklama,
                tarih = (DateTime)detaylar.tarih,
                kullanıcıAdı = detaylar.PersonelTablo.personelAd + " " + detaylar.PersonelTablo.personelSoyad,
                artikel = detaylar.Order.modelNo + " - " + detaylar.Order.modelAd
            };

            int id = detaylar.id;
            ViewBag.index = index;

            List<OlcuTabloAra> araList = db.OlcuTabloAra.Where(x => x.olcuTabloID == id).OrderBy(x => x.id).ToList();
            List<OlcuTabloHesaplama> hesaplamaList = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == id).OrderBy(x => x.id).ToList();

            List<DTOImalatTabloGoster> dtolar = new List<DTOImalatTabloGoster>();

            List<string> bedenler = new List<string>();
            List<int> bedenid = db.OlcuTabloAra.Where(x => x.olcuTabloID == id).Select(x => x.bedenID).Distinct().ToList();


            if (araList.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 0)
            {
                foreach (var item in bedenid)
                {
                    string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }

            }
            else if (araList.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 1)
            {
                foreach (var item in bedenid)
                {
                    string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }

                bedenler = bedenler.OrderBy(x => x).ToList();
            }
            else if (araList.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 2)
            {
                foreach (var item in bedenid)
                {
                    string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }
            }
            else
            {
                foreach (var item in bedenid)
                {
                    string beden = db.Bedenler.Where(x => x.ID == item).Select(x => x.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }
            }

            foreach (var item2 in araList.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id).ToList())
            {
                DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                ara.id = item2.id;
                int olcuNoktasiID = item2.olcuNoktaID;
                ara.olcuNoktasi = item2.OlcuNoktalari.olcuNoktasi;
                int bedenID = item2.bedenID;
                ara.beden = bedenler;
                ara.deger = araList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.deger.ToString()).ToList();
                ara.tolerans = hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.tolerans.ToString()).FirstOrDefault();
                ara.cekme = hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.cekme.ToString()).FirstOrDefault();
                ara.oran = hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.oran.ToString()).FirstOrDefault();
                dtolar.Add(ara);
            }



            return PartialView(Tuple.Create(detay1, dtolar));
        }

        public JsonResult Kaydet(List<DTOImalatTabloDetay> detaylar, List<List<DTOImalatTabloGoster>> tablolar, PersonelTablo personel)
        {
            try
            {

                int orderID = detaylar[0].orderID;
                string giysiTuru = db.Order.Where(x => x.ID == orderID).Select(x => x.giysiTuru).FirstOrDefault();

                int giysiTuruID = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuru).Select(x => x.id).FirstOrDefault();

                if (giysiTuruID == 0 || giysiTuruID == null)
                {
                    giysiTuruID = 1;
                }

                int olcuTuruID = detaylar[0].olcuTuruID;


                var parameters = new[] { new SqlParameter("@orderID", SqlDbType.Int) { Value = orderID } ,
                        new SqlParameter("@giysiTuruID", SqlDbType.Int) { Value = giysiTuruID },
                        new SqlParameter("@olcuTuruID", SqlDbType.Int) { Value = olcuTuruID }};
                OlcuTabloDetay checkExist = db.OlcuTabloDetay.SqlQuery("SELECT * FROM [Olcum].[dbo].[OlcuTabloDetay] WHERE  orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID=@olcuTuruID ", parameters).FirstOrDefault();


                // OlcuTabloDetay checkExist = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.giysiTuruID == giysiTuruID && x.tabloTuru == 1).FirstOrDefault();

                if (checkExist != null)
                {
                    var parametersDelete = new[] { new SqlParameter("@orderID", SqlDbType.Int) { Value = orderID } ,
                            new SqlParameter("@giysiTuruID", SqlDbType.Int) { Value = giysiTuruID },
                            new SqlParameter("@olcuTuruID", SqlDbType.Int) { Value = olcuTuruID },
                            new SqlParameter("@tabloTuru", SqlDbType.Int) { Value = 1 }};

                    db.Database.CommandTimeout = 180;
                    db.Database.ExecuteSqlCommand("DELETE oth  FROM [Olcum].[dbo].[OlcuTabloHesaplama] oth join OlcuTabloDetay otd on otd.id = oth.olcutabloID where orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID = @olcuTuruID", parametersDelete);

                    db.Database.ExecuteSqlCommand("DELETE ota FROM [Olcum].[dbo].[OlcuTabloAra] ota join OlcuTabloDetay otd on otd.id = ota.olcutabloID  where orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID = @olcuTuruID", parametersDelete);

                    db.Database.ExecuteSqlCommand("DELETE  FROM [Olcum].[dbo].[OlcuTabloDetay] where orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID = @olcuTuruID ", parametersDelete);
                }

                string anaBeden = detaylar[0].anaBeden;
                int anaBedenID = db.Bedenler.Where(x => x.beden == anaBeden).Select(x => x.ID).FirstOrDefault();

                List<OlcuTabloAra> listeOlcuTabloAra = new List<OlcuTabloAra>();
                List<OlcuTabloHesaplama> listeOlcuTabloHesaplama = new List<OlcuTabloHesaplama>();

                for (int i = 0; i < detaylar.Count; i++)
                {
                    if (detaylar[i].tabloTuru == 1)
                    {    
                        OlcuTabloDetay detayTablo = new OlcuTabloDetay();
                        detayTablo.orderID = detaylar[i].orderID;
                        detayTablo.giysiTuruID = giysiTuruID;
                        detayTablo.olcuTuruID = detaylar[i].olcuTuruID;
                        detayTablo.anaBedenID = anaBedenID;
                        detayTablo.enBoyCekme = "";
                        detayTablo.tabloTuru = detaylar[i].tabloTuru;
                        detayTablo.aciklama = detaylar[i].aciklama;
                        detayTablo.tarih = DateTime.Now;
                        detayTablo.kullaniciID = personel.id;
                        detayTablo.aktarimMi = false;

                        db.OlcuTabloDetay.Add(detayTablo);
                        db.SaveChanges();

                        for (int j = 0; j < tablolar[i].Count; j++)
                        {
                            string olcuNoktasi = tablolar[i][j].olcuNoktasi;
                            int _olcunoktaID = 0;
                            if (!int.TryParse(olcuNoktasi, out int var))
                            {
                                _olcunoktaID = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();
                            }
                            else
                            {
                                int stringID = Convert.ToInt32(olcuNoktasi);
                                _olcunoktaID = stringID;
                            }


                            for (int k = 0; k < tablolar[i][j].deger.Count; k++)
                            {
                                string beden = tablolar[i][j].beden[k].Replace("&nbsp;", " ").Replace("&nbsp", " ");
                                string deger = tablolar[i][j].deger[k].Trim();
                                int _bedenId = db.Bedenler.Where(x => x.beden == beden).Select(x => x.ID).FirstOrDefault();
                                if (_bedenId == 0)
                                {
                                    _bedenId = db.Bedenler.Where(x => x.beden.Contains(beden.Trim())).Select(x => x.ID).FirstOrDefault();
                                }

                                double hata = Convert.ToDouble(deger.Replace('.', ','));
                                OlcuTabloAra model = new OlcuTabloAra()
                                {
                                    olcuTabloID = detayTablo.id,
                                    olcuNoktaID = _olcunoktaID,
                                    bedenID = _bedenId,
                                    deger = Convert.ToDouble(deger.Replace('.', ','))
                                };
                                listeOlcuTabloAra.Add(model);
                            }
                        }               
                    }
                    else
                    {
                      
                        OlcuTabloDetay detayTablo = new OlcuTabloDetay();
                        detayTablo.orderID = detaylar[i].orderID;
                        detayTablo.giysiTuruID = giysiTuruID;
                        detayTablo.olcuTuruID = detaylar[i].olcuTuruID;
                        detayTablo.anaBedenID = anaBedenID;
                        detayTablo.enBoyCekme = detaylar[i].enBoyCekme;
                        detayTablo.tabloTuru = detaylar[i].tabloTuru;
                        detayTablo.aciklama = detaylar[i].aciklama;
                        detayTablo.tarih = DateTime.Now;
                        detayTablo.kullaniciID = personel.id;
                        detayTablo.aktarimMi = false;

                        db.OlcuTabloDetay.Add(detayTablo);
                        db.SaveChanges();

                      

                        for (int j = 0; j < tablolar[i].Count; j++)
                        {
                            string olcuNoktasi = tablolar[i][j].olcuNoktasi;
                            int _olcuNoktaID = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();

                            for (int k = 0; k < tablolar[i][j].deger.Count; k++)
                            {
                                string beden = tablolar[i][j].beden[k].Replace("&nbsp;", " ").Replace("&nbsp", " ");
                                string deger = tablolar[i][j].deger[k].Trim();
                                int _bedenId = db.Bedenler.Where(x => x.beden == beden).Select(x => x.ID).FirstOrDefault();
                                if (_bedenId == 0)
                                {
                                    _bedenId = db.Bedenler.Where(x => x.beden.Contains(beden.Trim())).Select(x => x.ID).FirstOrDefault();
                                }
                                OlcuTabloAra model = new OlcuTabloAra()
                                {
                                    olcuTabloID = detayTablo.id,
                                    olcuNoktaID = _olcuNoktaID,
                                    bedenID = _bedenId,
                                    deger = Convert.ToDouble(deger.Replace('.', ','))
                                };
                                listeOlcuTabloAra.Add(model);
                            }

                            OlcuTabloHesaplama modelHesaplama = new OlcuTabloHesaplama()
                            {
                                olcuTabloID = detayTablo.id,
                                olcuNoktaID = _olcuNoktaID,
                                tolerans = Convert.ToDouble(tablolar[i][j].tolerans.Replace('.', ',')),
                                cekme = Convert.ToDouble(tablolar[i][j].cekme.Replace('.', ',')),
                                oran = Convert.ToDouble(tablolar[i][j].oran.Replace('.', ',')),
                            };
                            listeOlcuTabloHesaplama.Add(modelHesaplama);
                        }

                     
                    }

                }

                using (var connection = new SqlConnection(ConnectionStrings.OlcumConnection))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("id", "id"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuTabloID", "olcuTabloID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuNoktaID", "olcuNoktaID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("bedenID", "bedenID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("deger", "deger"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("orijinalOlcuNok", "orijinalOlcuNok"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("hataOldu", "hataOldu"));
                        bulkCopy.BatchSize = 10000;
                        bulkCopy.BulkCopyTimeout = 120;
                        bulkCopy.DestinationTableName = "[dbo].[OlcuTabloAra]";
                        try
                        {
                            bulkCopy.WriteToServer(listeOlcuTabloAra.AsDataTable());
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            connection.Close();
                            return Json(ex.Message);
                        }
                    }

                    transaction.Commit();

                    SqlTransaction transaction1 = connection.BeginTransaction();

                    using (var bulkCopy2 = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction1))
                    {
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("id", "id"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuTabloID", "olcuTabloID"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuNoktaID", "olcuNoktaID"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tolerans", "tolerans"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("cekme", "cekme"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("oran", "oran"));
                        //bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("orijinalOlcuNok", "orijinalOlcuNok"));
                        //bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("hataOldu", "hataOldu"));
                        bulkCopy2.BatchSize = 10000;
                        bulkCopy2.BulkCopyTimeout = 120;
                        bulkCopy2.DestinationTableName = "[dbo].[OlcuTabloHesaplama]";
                        try
                        {
                            bulkCopy2.WriteToServer(listeOlcuTabloHesaplama.AsDataTable());
                        }
                        catch (Exception ex)
                        {
                            transaction1.Rollback();
                            connection.Close();
                            return Json(ex.Message);
                        }
                    }

                    transaction1.Commit();

                    connection.Close();

                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        public PartialViewResult OncekiTablodanGetirModal()
        {
            ViewBag.order = db.OlcuTabloDetay.Join(db.Order, x => x.orderID, order => order.ID, (x, order) => new { Order = order, OlcuTabloDetay = x }).Select(order => new { order.Order.orderNo, order.Order.ID }).Distinct().ToList();
            return PartialView();
        }

        public PartialViewResult EnBoyCekmeModal(string anaBeden, List<List<DTOImalatTabloGoster>> previousTablo)
        {
            return PartialView(Tuple.Create(anaBeden, previousTablo));
        }

        public PartialViewResult EnBoyCekmeModal2(string anaBeden, List<List<DTOImalatTabloGoster>> cekmeler, List<string> cekmeisimleri)
        {
            ViewBag.cekmeisimLeri = cekmeisimleri;
            return PartialView(Tuple.Create(anaBeden, cekmeler));
        }

        public PartialViewResult Cekmeler(string orderNo)
        {
            int orderID = db.Order.Where(x => x.orderNo == orderNo).Select(x => x.ID).FirstOrDefault();



            List<string> cekmeler = db.OlcuTabloDetay.Where(x => x.orderID == orderID && (x.tabloTuru != 2)).Select(x => x.enBoyCekme).ToList();

            for (int i = 0; i < cekmeler.Count; i++)
            {
                if (cekmeler[i] == null || cekmeler[i] == "")
                {
                    cekmeler[i] = "Yıkama Sonrası";
                }
            }

            return PartialView(cekmeler);
        }

        public PartialViewResult YikamaOncesiEkle(int checkExist = 1, string anaBeden = "", List<List<DTOImalatTabloGoster>> previousTablo = null, List<string> cekmeler = null, DTOImalatTabloDetay detayTablo = null)
        {
            ViewBag.checkExist = checkExist;
            ViewBag.anaBeden = anaBeden;
            ViewBag.cekmeler = cekmeler;


            string en = cekmeler[cekmeler.Count - 1].Split('/')[0];
            string boy = cekmeler[cekmeler.Count - 1].Split('/')[1];

            string anaEn = cekmeler[0].Split('/')[0];
            string anaBoy = cekmeler[0].Split('/')[1];

            if (en.Contains('+'))
            {
                en.Replace('+', '-');
            }

            if (boy.Contains('+'))
            {
                boy = boy.Replace('+', '-');
            }


            if (anaEn.Contains('+'))
            {
                anaEn.Replace('+', '-');
            }

            if (anaBoy.Contains('+'))
            {
                anaBoy = anaBoy.Replace('+', '-');
            }

            double enSayi = Convert.ToDouble(en);
            double boySayi = Convert.ToDouble(boy);

            double anaEnSayi = Convert.ToDouble(anaEn);
            double anaBoySayi = Convert.ToDouble(anaBoy);


            var enOran = enSayi - anaEnSayi;
            var boyOran = boySayi - anaBoySayi;


            for (int i = 0; i < previousTablo[previousTablo.Count - 1].Count; i++)
            {
                string olcuNoktasi = previousTablo[previousTablo.Count - 1][i].olcuNoktasi.Trim();

                bool? isEnBol = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.enBoy).FirstOrDefault();

                if (isEnBol == false)
                {
                    previousTablo[previousTablo.Count - 1][i].cekme = (Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ',')) + enOran).ToString().Replace(',', '.');
                }
                else if (isEnBol == true)
                {
                    previousTablo[previousTablo.Count - 1][i].cekme = (Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ',')) + boyOran).ToString().Replace(',', '.');
                }



                for (int j = 0; j < previousTablo[previousTablo.Count - 1][i].deger.Count; j++)
                {
                    double girilenCekme = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].cekme.Replace('.', ','));
                    double ilkCekme = Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ','));
                    double ilkDeger = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].deger[j].Replace('.', ','));
                    double sonCekme = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].cekme.Replace('.', ','));
                    double yıkamaSonrası = Math.Round(Convert.ToDouble(ilkDeger * (100-ilkCekme) / 100),1,MidpointRounding.AwayFromZero);
                    double yeniDeger = Math.Round(100 * yıkamaSonrası / (100 - sonCekme), 1, MidpointRounding.AwayFromZero);

                    if (Double.IsNaN(yeniDeger) || Double.IsInfinity(yeniDeger))
                    {
                        yeniDeger = 0;
                    }

                    string deger = yeniDeger.ToString().Replace(',', '.');

                    previousTablo[previousTablo.Count - 1][i].deger[j] = deger;

                }

                double ilkCekme2 = Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ','));
                double ilkDeger3 = Convert.ToDouble(previousTablo[0][i].deger[0].Replace('.', ','));
                double ilkDeger2 = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].deger[0].Replace('.', ','));
                double yıkamaSonrası2 = Math.Round(Convert.ToDouble(ilkDeger3 * (100 - ilkCekme2) / 100), 1, MidpointRounding.AwayFromZero);
                double oran = Math.Round(((ilkDeger2 - yıkamaSonrası2) / yıkamaSonrası2) * 100, 1, MidpointRounding.AwayFromZero);
                if(Double.IsNaN(oran) || Double.IsInfinity(oran))
                {
                    oran = 0;
                }
                previousTablo[previousTablo.Count - 1][i].oran = oran.ToString().Replace(',', '.');
            }


            try
            {
                string giysiTuru = db.Order.Where(x => x.ID == detayTablo.orderID).Select(x => x.giysiTuru).FirstOrDefault();

                int giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuru).Select(x => x.id).FirstOrDefault();


                int anaBedenId = db.Bedenler.Where(x => x.beden == anaBeden.Replace("&nbsp;", " ").Replace("&nbsp", " ")).Select(x => x.ID).FirstOrDefault();

                OlcuTabloDetay olcuTabloDetay = new OlcuTabloDetay()
                {
                    orderID = detayTablo.orderID,
                    giysiTuruID = giysiTuruId,
                    enBoyCekme = detayTablo.enBoyCekme,
                    aciklama = detayTablo.aciklama == null ? "" : detayTablo.aciklama,
                    kullaniciID = detayTablo.kullaniciID,
                    anaBedenID = anaBedenId,
                    olcuTuruID = detayTablo.olcuTuruID,
                    aktarimMi = false,
                    hataOldu = false,
                    tabloTuru = 0,
                    tarih = DateTime.Now,

                };

                db.OlcuTabloDetay.Add(olcuTabloDetay);
                db.SaveChanges();

                List<OlcuTabloAra> olcuTabloAras = new List<OlcuTabloAra>();

                List<OlcuTabloHesaplama> olcuTabloHesaplamas = new List<OlcuTabloHesaplama>();

                foreach (var item in previousTablo[previousTablo.Count - 1])
                {
                    string olcuNoktasi = item.olcuNoktasi;

                    int olcuNoktaId = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();

                    for (int i = 0; i < item.beden.Count; i++)
                    {
                        string beden = item.beden[i].Replace("&nbsp;", " ").Replace("&nbsp", " ");

                        int bedenId = db.Bedenler.Where(x => x.beden == beden).Select(x=>x.ID).FirstOrDefault();


                        OlcuTabloAra olcuTabloAra = new OlcuTabloAra()
                        {
                            olcuTabloID = olcuTabloDetay.id,
                            bedenID = bedenId,
                            olcuNoktaID = olcuNoktaId,
                            deger = Convert.ToDouble(item.deger[i].Replace('.',','))
                        };

                        olcuTabloAras.Add(olcuTabloAra);

                    }


                    OlcuTabloHesaplama olcuTabloHesaplama = new OlcuTabloHesaplama()
                    {
                        olcuTabloID = olcuTabloDetay.id,
                        olcuNoktaID = olcuNoktaId,
                        cekme = Convert.ToDouble(item.cekme.Replace('.', ',')),
                        tolerans = Convert.ToDouble(item.tolerans.Replace('.', ',')),
                        oran = Convert.ToDouble(item.oran.Replace('.', ',')),
                        
                    };

                    olcuTabloHesaplamas.Add(olcuTabloHesaplama);

                }


                using (var connection = new SqlConnection(ConnectionStrings.OlcumConnection))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("id", "id"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuTabloID", "olcuTabloID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuNoktaID", "olcuNoktaID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("bedenID", "bedenID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("deger", "deger"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("orijinalOlcuNok", "orijinalOlcuNok"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("hataOldu", "hataOldu"));
                        bulkCopy.BatchSize = 100000000;
                        bulkCopy.BulkCopyTimeout = 120;
                        bulkCopy.DestinationTableName = "[dbo].[OlcuTabloAra]";
                        try
                        {
                            bulkCopy.WriteToServer(olcuTabloAras.AsDataTable());
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            connection.Close();
                        }
                    }

                    transaction.Commit();

                    SqlTransaction transaction1 = connection.BeginTransaction();

                    using (var bulkCopy2 = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction1))
                    {
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("id", "id"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuTabloID", "olcuTabloID"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("olcuNoktaID", "olcuNoktaID"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tolerans", "tolerans"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("cekme", "cekme"));
                        bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("oran", "oran"));
                        //bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("orijinalOlcuNok", "orijinalOlcuNok"));
                        //bulkCopy2.ColumnMappings.Add(new SqlBulkCopyColumnMapping("hataOldu", "hataOldu"));
                        bulkCopy2.BatchSize = 100000000;
                        bulkCopy2.BulkCopyTimeout = 120;
                        bulkCopy2.DestinationTableName = "[dbo].[OlcuTabloHesaplama]";
                        try
                        {
                            bulkCopy2.WriteToServer(olcuTabloHesaplamas.AsDataTable());
                        }
                        catch (Exception ex)
                        {
                            transaction1.Rollback();
                            connection.Close();
                        }
                    }

                    transaction1.Commit();

                    connection.Close();

                }
            }
            catch(Exception ex)
            {
                return PartialView("YikamaOncesiGoster", Tuple.Create(previousTablo));
            }

            return PartialView("YikamaOncesiGoster", Tuple.Create(previousTablo));
        }

        public PartialViewResult Print(List<DTOImalatTabloDetay> param1, List<List<DTOImalatTabloGoster>> param2, List<string> cekme, PersonelTablo personel)
        {
            int orderID = param1[0].orderID;
            int olcuTuruID = param1[0].olcuTuruID;

            ViewBag.order = db.Order.Where(x => x.ID == orderID).FirstOrDefault();
            ViewBag.olcuTuru = db.OlcuTurleri.Where(x => x.id == olcuTuruID).Select(x => x.olcuTuruAd).FirstOrDefault();

            ViewBag.personel = personel;


            List<int> deletedIndexes = new List<int>();

            for (int i = 0; i < param1.Count; i++)
            {
                param1[i].tarih = Convert.ToDateTime(param1[i].tarihString);
                if (param1[i].enBoyCekme != "" && param1[i].enBoyCekme != null)
                {
                    string cekmeAd = param1[i].enBoyCekme;
                    int index = cekme.FindIndex(x => x == cekmeAd);

                    if (index == -1)
                    {
                        int param1Index = param1.FindIndex(x => x.enBoyCekme == cekmeAd);

                        deletedIndexes.Add(param1Index);
                    }
                }
            }

            foreach (var item in deletedIndexes.OrderByDescending(x=>x))
            {
                param1.RemoveAt(item);
                param2.RemoveAt(item);
            }

            return PartialView(Tuple.Create(param1, param2));
        }

        public ActionResult EnBoyCekmePrintModal(List<DTOImalatTabloDetay> param1, List<List<DTOImalatTabloGoster>> param2, List<string> cekme, PersonelTablo personel)
        {
            if(cekme == null)
            {
                cekme = new List<string>();
            }
            cekme.Insert(0, "Yıkama Sonrası");
            ViewBag.personel = personel;
            return PartialView(Tuple.Create(param1, param2, cekme));
        }

        public PartialViewResult OlcuNoktasiEkleme( int index, string oldOlcuNokta)
        {
            ViewBag.olcuNoktalari = db.OlcuNoktalari.ToList();
            ViewBag.index = index;
            ViewBag.oldOlcuNokta = oldOlcuNokta;
            return PartialView();
        }

        public PartialViewResult SatirEkleme(int columnCount, int index)
        {
            ViewBag.olcuNoktalari = db.OlcuNoktalari.ToList();
            ViewBag.columnCount = columnCount;
            ViewBag.index = index;
            return PartialView();
        }

        public PartialViewResult YikamaSonrasiOncekiTablo(int orderID, int olcuTuruID)
        {
            ViewBag.checkExist = 1;
            DTOImalatTabloDetay detay = new DTOImalatTabloDetay();
            List<DTOImalatTabloGoster> tablo = new List<DTOImalatTabloGoster>();

            var olcuTabloDetay = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).FirstOrDefault();


            int olcuTabloDetaydetayid = olcuTabloDetay.id;
            int anaBedenID = olcuTabloDetay.anaBedenID;
            int kullanıcıID = olcuTabloDetay.kullaniciID;


            string anaBeden = db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault();

            ViewBag.anaBeden = anaBeden;

            string kullanıcıAdSoyad = db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelAd).FirstOrDefault() + " " + db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelSoyad).FirstOrDefault();

            ViewBag.personel = kullanıcıAdSoyad;

            List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloDetaydetayid).ToList();

            List<string> bedenler = new List<string>();

            List<int> bedenid = olcuTabloAras.Where(x => x.olcuTabloID == olcuTabloDetaydetayid).Select(x => x.bedenID).Distinct().ToList();

            if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 0)
            {
                foreach (var item in bedenid)
                {
                    string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }

            }
            else if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 1)
            {
                foreach (var item in bedenid)
                {
                    string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }

                bedenler = bedenler.OrderBy(x => x).ToList();
            }
            else if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 2)
            {
                foreach (var item in bedenid)
                {
                    string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }
            }
            else
            {
                foreach (var item in bedenid)
                {
                    string beden = olcuTabloAras.Where(x => x.bedenID == item).Select(x => x.Bedenler.beden).FirstOrDefault();
                    bedenler.Add(beden);
                }
            }

            foreach (var item in olcuTabloAras.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id))
            {
                DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                ara.id = item.id;
                int olcuNoktasiID = item.olcuNoktaID;
                ara.olcuNoktasi = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID).Select(x => x.OlcuNoktalari.olcuNoktasi).FirstOrDefault();
                ara.olcuNoktaId = olcuNoktasiID;
                int bedenID = item.bedenID;
                ara.beden = bedenler;
                ara.bedenId = bedenid;
                ara.deger = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == olcuTabloDetaydetayid).Select(x => x.deger.ToString()).ToList();

                tablo.Add(ara);
            }

            return PartialView("YikamaSonrasiGoster", Tuple.Create(tablo));
        }

        public PartialViewResult YikamaOncesiOncekiTablo(int orderID, int olcuTuruID, List<string> cekmeler)
        {
            cekmeler.Remove("Yıkama Sonrası");

            if(cekmeler.Count != 0)
            {
                List<OlcuTabloDetay> detayList = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 0 && cekmeler.Contains(x.enBoyCekme)).ToList();

                int ysAnaBedenID = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.anaBedenID).FirstOrDefault();

                string anaBeden = db.Bedenler.Where(x => x.ID == ysAnaBedenID).Select(x => x.beden).FirstOrDefault();

                ViewBag.anaBeden = anaBeden;
                ViewBag.checkExist = 1;
                ViewBag.cekmeler = cekmeler;

                List<List<DTOImalatTabloGoster>> dtoList = new List<List<DTOImalatTabloGoster>>();

                foreach (var item in detayList)
                {
                    List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == item.id).ToList();
                    List<OlcuTabloHesaplama> olcuTabloHesaplamas = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == item.id).ToList();

                    List<string> bedenler = new List<string>();

                    List<int> bedenid = db.OlcuTabloAra.Where(x => x.olcuTabloID == item.id).Select(x => x.bedenID).Distinct().ToList();

                    List<DTOImalatTabloGoster> dto = new List<DTOImalatTabloGoster>();

                    if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 0)
                    {
                        foreach (var item2 in bedenid)
                        {
                            string beden = olcuTabloAras.Where(x => x.bedenID == item2).Select(x => x.Bedenler.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }

                    }
                    else if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 1)
                    {
                        foreach (var item2 in bedenid)
                        {
                            string beden = olcuTabloAras.Where(x => x.bedenID == item2).Select(x => x.Bedenler.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }

                        bedenler = bedenler.OrderBy(x => x).ToList();
                    }
                    else if (olcuTabloAras.Select(x => x.Bedenler.bedenSistemi).FirstOrDefault() == 2)
                    {
                        foreach (var item2 in bedenid)
                        {
                            string beden = olcuTabloAras.Where(x => x.bedenID == item2).Select(x => x.Bedenler.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }
                    }
                    else
                    {
                        foreach (var item2 in bedenid)
                        {
                            string beden = olcuTabloAras.Where(x => x.bedenID == item2).Select(x => x.Bedenler.beden).FirstOrDefault();
                            bedenler.Add(beden);
                        }
                    }

                    foreach (var item2 in olcuTabloAras.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id))
                    {
                        DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                        ara.id = item2.id;
                        int olcuNoktasiID = item2.olcuNoktaID;
                        ara.olcuNoktasi = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID).Select(x => x.OlcuNoktalari.olcuNoktasi).FirstOrDefault();
                        ara.olcuNoktaId = olcuNoktasiID;
                        int bedenID = item2.bedenID;
                        ara.beden = bedenler;
                        ara.bedenId = bedenid;
                        ara.deger = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.id).Select(x => x.deger.ToString()).ToList();
                        ara.tolerans = olcuTabloHesaplamas.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.id).Select(x => x.tolerans.ToString()).FirstOrDefault();
                        ara.cekme = olcuTabloHesaplamas.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.id).Select(x => x.cekme.ToString()).FirstOrDefault();
                        ara.oran = olcuTabloHesaplamas.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.id).Select(x => x.oran.ToString()).FirstOrDefault();
                        dto.Add(ara);
                    }

                    dtoList.Add(dto);
                }



                return PartialView("YikamaOncesiGoster", Tuple.Create(dtoList));
            }
            else
            {
                List<List<DTOImalatTabloGoster>> dtoList = new List<List<DTOImalatTabloGoster>>();
                int ysAnaBedenID = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.anaBedenID).FirstOrDefault();
                string anaBeden = db.Bedenler.Where(x => x.ID == ysAnaBedenID).Select(x => x.beden).FirstOrDefault();
                ViewBag.anaBeden = anaBeden;
                ViewBag.checkExist = 0;

                return PartialView("YikamaOncesiGoster", Tuple.Create(dtoList));
            }


        }

        public PartialViewResult SutunEkleme(string ilkBeden,int eklenecekIndex)
        {
            int sayiSistemi = (int)db.Bedenler.Where(x => x.beden == ilkBeden).Select(x => x.bedenSistemi).FirstOrDefault();

            ViewBag.eklenecekIndex = eklenecekIndex;

            ViewBag.bedenler = db.Bedenler.Where(x => x.bedenSistemi == sayiSistemi).ToList();
            return PartialView();
        }

        public PartialViewResult CekmeSil(string anaBeden, List<List<DTOImalatTabloGoster>> tablolar,List<string> cekmeler,string seciliCekme,DTOImalatTabloDetay imalatTabloDetay = null)
        {
            //Aktif çekmenin indexini alıyorum.
            int index = cekmeler.IndexOf(seciliCekme);

            string giysiTuru = db.Order.Where(x => x.ID == imalatTabloDetay.orderID).Select(X => X.giysiTuru).FirstOrDefault();

            int giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuru).Select(x => x.id).FirstOrDefault();

            var parametersDelete = new[] { new SqlParameter("@orderID", SqlDbType.Int) { Value = imalatTabloDetay.orderID } ,
                            new SqlParameter("@giysiTuruID", SqlDbType.Int) { Value = giysiTuruId },
                            new SqlParameter("@olcuTuruID", SqlDbType.Int) { Value = imalatTabloDetay.olcuTuruID },
                            new SqlParameter("@tabloTuru", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@enBoyCekme", SqlDbType.NVarChar) { Value = imalatTabloDetay.enBoyCekme }};
            db.Database.ExecuteSqlCommand("DELETE oth  FROM [Olcum].[dbo].[OlcuTabloHesaplama] oth join OlcuTabloDetay otd on otd.id = oth.olcutabloID where orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID = @olcuTuruID and enBoyCekme = @enBoyCekme", parametersDelete);

            db.Database.ExecuteSqlCommand("DELETE ota FROM [Olcum].[dbo].[OlcuTabloAra] ota join OlcuTabloDetay otd on otd.id = ota.olcutabloID  where orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID = @olcuTuruID and enBoyCekme = @enBoyCekme", parametersDelete);

            db.Database.ExecuteSqlCommand("DELETE  FROM [Olcum].[dbo].[OlcuTabloDetay] where orderID=@orderID and giysiTuruID=@giysiTuruID and olcuTuruID = @olcuTuruID and enBoyCekme = @enBoyCekme", parametersDelete);

            cekmeler.RemoveAt(index);
            tablolar.RemoveAt(index);

            if (cekmeler.Count == 0)
            {
                ViewBag.checkExist = 0;
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
            }
            else
            {
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
            }




            return PartialView("YikamaOncesiGoster", Tuple.Create(tablolar));
        }

        public PartialViewResult CekmeAdDuzenle(int activeTabIndex,string enBoyCekme)
        {
            ViewBag.index = activeTabIndex;
            ViewBag.enBoyCekme = enBoyCekme;
            return PartialView();
        }

        public PartialViewResult KalipOlcusuGoster(int orderID, int olcuTuruID,int checkExist, int hazirtabloID = 0, string anaBeden = "", List<List<DTOImalatTabloGoster>> previousTablo = null, List<string> cekmeler = null)
        {
            List<List<DTOImalatTabloGoster>> tablo = new List<List<DTOImalatTabloGoster>>();

            ViewBag.checkExist = checkExist;
            if (checkExist == 0)
            {
                ViewBag.cekmeler = cekmeler;
                return PartialView("KalipOlcusuGoster", Tuple.Create(tablo));
            }
            else if (checkExist == 1)
            {
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
                tablo = previousTablo;
                return PartialView("KalipOlcusuGoster", Tuple.Create(tablo));
            }
            else
            {
                List<OlcuTabloDetay> detayList = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 0).ToList();

                if (detayList.Count > 0)
                {
                    int firstID = detayList[0].id;

                    int anaBedenID = detayList[0].anaBedenID;

                    ViewBag.anaBeden = db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault();

                    List<string> enBoyCekmeler = detayList.Select(x => x.enBoyCekme).ToList();
                    ViewBag.cekmeler = enBoyCekmeler;

                    List<string> bedenler = new List<string>();
                    bedenler.Add(db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault());

                    
                    foreach (var item2 in detayList)
                    {
                        List<OlcuTabloAra> araList = db.OlcuTabloAra.Where(x => x.olcuTabloID == item2.id && x.bedenID == anaBedenID).OrderBy(x=>x.id).ToList();
                        List<OlcuTabloHesaplama> hesaplamaList = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == item2.id).OrderBy(x => x.id).ToList();

                        List<DTOImalatTabloGoster> dto = new List<DTOImalatTabloGoster>();
                        foreach (var item in araList.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id).ToList())
                        {
                            DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                            ara.id = item.id;
                            int olcuNoktasiID = item.olcuNoktaID;
                            ara.olcuNoktasi = araList.Where(x => x.olcuNoktaID == item.olcuNoktaID).Select(x => x.OlcuNoktalari.olcuNoktasi).FirstOrDefault();
                            int bedenID = item.bedenID;
                            ara.beden = bedenler;
                            double deger = araList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.bedenID == anaBedenID).Select(x => x.deger).FirstOrDefault();
                            double tolerans = (double)hesaplamaList.Where(x => x.olcuNoktaID == olcuNoktasiID).Select(x => x.tolerans).FirstOrDefault();
                            List<string> degerler = new List<string>();
                            degerler.Add((tolerans + deger).ToString());
                            ara.deger = degerler;

                            dto.Add(ara);
                        }

                        tablo.Add(dto);
                    }

                }
                else
                {
                    ViewBag.checkExist = 0;
                    ViewBag.anaBeden = anaBeden;
                    ViewBag.cekmeler = cekmeler;
                    return PartialView("KalipOlcusuGoster", Tuple.Create(tablo));
                }

                return PartialView("KalipOlcusuGoster", Tuple.Create(tablo));

            }
        }

        public PartialViewResult KalipOlcusuOncekiTablo(int orderID, int olcuTuruID, List<string> cekmeler)
        {
            cekmeler.Remove("Yıkama Sonrası");

            if (cekmeler.Count != 0)
            {
                List<OlcuTabloDetay> detayList = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 0 && cekmeler.Contains(x.enBoyCekme)).ToList();

                int ysAnaBedenID = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.anaBedenID).FirstOrDefault();

                string anaBeden = db.Bedenler.Where(x => x.ID == ysAnaBedenID).Select(x => x.beden).FirstOrDefault();

                ViewBag.anaBeden = anaBeden;
                ViewBag.checkExist = 1;
                ViewBag.cekmeler = cekmeler;

                List<List<DTOImalatTabloGoster>> dtoList = new List<List<DTOImalatTabloGoster>>();

                foreach (var item in detayList)
                {
                    List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == item.id && x.bedenID == ysAnaBedenID).OrderBy(x=>x.id).ToList();
                    List<OlcuTabloHesaplama> olcuTabloHesaplamas = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == item.id).ToList();

                    List<string> bedenler = new List<string>();
                    bedenler.Add(anaBeden);


                    List<DTOImalatTabloGoster> dto = new List<DTOImalatTabloGoster>();

                   

                    foreach (var item2 in olcuTabloAras.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id))
                    {
                        DTOImalatTabloGoster ara = new DTOImalatTabloGoster();
                        ara.id = item2.id;
                        int olcuNoktasiID = item2.olcuNoktaID;
                        ara.olcuNoktasi = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID).Select(x => x.OlcuNoktalari.olcuNoktasi).FirstOrDefault();
                        ara.olcuNoktaId = olcuNoktasiID;
                        int bedenID = item2.bedenID;
                        ara.beden = bedenler;
                        var deger = olcuTabloAras.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.id && x.bedenID == ysAnaBedenID).Select(x => x.deger).FirstOrDefault();
                        var tolerans = (double)olcuTabloHesaplamas.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == item.id).Select(x => x.tolerans).FirstOrDefault();
                        string kalipOlcu = Math.Round(deger + tolerans, 1, MidpointRounding.AwayFromZero).ToString();
                        List<string> degerList = new List<string>();
                        degerList.Add(kalipOlcu);
                        ara.deger = degerList;
                        dto.Add(ara);
                    }

                    dtoList.Add(dto);
                }



                return PartialView("KalipOlcusuGoster", Tuple.Create(dtoList));
            }
            else
            {
                List<List<DTOImalatTabloGoster>> dtoList = new List<List<DTOImalatTabloGoster>>();
                int ysAnaBedenID = db.OlcuTabloDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.tabloTuru == 1).Select(x => x.anaBedenID).FirstOrDefault();
                string anaBeden = db.Bedenler.Where(x => x.ID == ysAnaBedenID).Select(x => x.beden).FirstOrDefault();
                ViewBag.anaBeden = anaBeden;
                ViewBag.checkExist = 0;

                return PartialView("KalipOlcusuGoster", Tuple.Create(dtoList));
            }
        }

        public PartialViewResult KalipOlcusuEkle(int checkExist = 1, string anaBeden = "", List<List<DTOImalatTabloGoster>> previousTablo = null, List<string> cekmeler = null)
        {
            ViewBag.checkExist = checkExist;
            ViewBag.anaBeden = anaBeden;
            ViewBag.cekmeler = cekmeler;


            string en = cekmeler[cekmeler.Count - 1].Split('/')[0];
            string boy = cekmeler[cekmeler.Count - 1].Split('/')[1];

            string anaEn = cekmeler[0].Split('/')[0];
            string anaBoy = cekmeler[0].Split('/')[1];

            if (en.Contains('+'))
            {
                en.Replace('+', '-');
            }

            if (boy.Contains('+'))
            {
                boy.Replace('+', '-');
            }


            if (anaEn.Contains('+'))
            {
                anaEn.Replace('+', '-');
            }

            if (anaBoy.Contains('+'))
            {
                anaBoy.Replace('+', '-');
            }

            double enSayi = Convert.ToDouble(en);
            double boySayi = Convert.ToDouble(boy);

            double anaEnSayi = Convert.ToDouble(anaEn);
            double anaBoySayi = Convert.ToDouble(anaBoy);


            var enOran = enSayi - anaEnSayi;
            var boyOran = boySayi - anaBoySayi;


            for (int i = 0; i < previousTablo[previousTablo.Count - 1].Count; i++)
            {
                string olcuNoktasi = previousTablo[previousTablo.Count - 1][i].olcuNoktasi.Trim();

                bool? isEnBol = db.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.enBoy).FirstOrDefault();

                if (isEnBol == false)
                {
                    previousTablo[previousTablo.Count - 1][i].cekme = (Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ',')) + enOran).ToString().Replace(',', '.');
                }
                else if (isEnBol == true)
                {
                    previousTablo[previousTablo.Count - 1][i].cekme = (Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ',')) + boyOran).ToString().Replace(',', '.');
                }

                for (int j = 0; j < previousTablo[previousTablo.Count - 1][i].deger.Count; j++)
                {
                    double tolerans = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].tolerans.Replace('.', ','));
                    double girilenCekme = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].cekme.Replace('.', ','));
                    double ilkCekme = Convert.ToDouble(previousTablo[0][i].cekme.Replace('.', ','));
                    double sonCekme = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].cekme.Replace('.', ','));
                    double ilkDeger = Convert.ToDouble(previousTablo[previousTablo.Count - 1][i].deger[j].Replace('.', ','));
                    double yıkamaSonrası = Math.Round(Convert.ToDouble(ilkDeger * (100 - ilkCekme) / 100), 1, MidpointRounding.AwayFromZero);
                    double yeniDeger = Math.Round((100 * yıkamaSonrası / (100 - sonCekme)) + tolerans, 1, MidpointRounding.AwayFromZero);

                    if (Double.IsNaN(yeniDeger) || Double.IsInfinity(yeniDeger))
                    {
                        yeniDeger = 0;
                    }

                    string deger = yeniDeger.ToString().Replace(',', '.');

                    previousTablo[previousTablo.Count - 1][i].deger[j] = deger;
                }
            }

            for(int i = 0; i < previousTablo.Count - 1; i++)
            {
                for(int j = 0; j < previousTablo[i].Count; j++)
                {
                    for(int k = 0; k < previousTablo[i][j].deger.Count; k++)
                    {
                        double tolerans = Convert.ToDouble(previousTablo[i][j].tolerans.Replace('.', ','));
                        double deger = Convert.ToDouble(previousTablo[i][j].deger[k].Replace('.', ','));
                        previousTablo[i][j].deger[k] = Math.Round(tolerans + deger, 1, MidpointRounding.AwayFromZero).ToString();
                    }
                }
            }


            return PartialView("KalipOlcusuGoster", Tuple.Create(previousTablo));
        }

        public PartialViewResult KalipSil(string anaBeden, List<List<DTOImalatTabloGoster>> tablolar, List<string> cekmeler, string seciliCekme)
        {
            //Aktif çekmenin indexini alıyorum.
            int index = cekmeler.IndexOf(seciliCekme);

            cekmeler.RemoveAt(index);
            tablolar.RemoveAt(index);

            if (cekmeler.Count == 0)
            {
                ViewBag.checkExist = 0;
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
            }
            else
            {
                ViewBag.anaBeden = anaBeden;
                ViewBag.cekmeler = cekmeler;
            }

            return PartialView("KalipOlcusuGoster", Tuple.Create(tablolar));
        }

        public PartialViewResult KalipGuncelle(int checkExist = 1, string anaBeden = "", List<List<DTOImalatTabloGoster>> previousTablo = null, List<string> cekmeler = null)
        {
            ViewBag.checkExist = checkExist;
            ViewBag.anaBeden = anaBeden;
            ViewBag.cekmeler = cekmeler;

            for (int i = 0; i < previousTablo.Count; i++)
            {
                for (int j = 0; j < previousTablo[i].Count; j++)
                {
                    for (int k = 0; k < previousTablo[i][j].deger.Count; k++)
                    {
                        double tolerans = Convert.ToDouble(previousTablo[i][j].tolerans.Replace('.', ','));
                        double deger = Convert.ToDouble(previousTablo[i][j].deger[k].Replace('.', ','));
                        previousTablo[i][j].deger[k] = Math.Round(tolerans + deger, 1, MidpointRounding.AwayFromZero).ToString();
                    }
                }
            }

            return PartialView("KalipOlcusuGoster", Tuple.Create(previousTablo));
        }
    }
}