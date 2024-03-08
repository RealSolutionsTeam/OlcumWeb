using MimeKit;
using OlcumWeb.Araclar;
using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class FasonController : Controller
    {

        OlcumContext db = new OlcumContext();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FasonFormPartial()
        {
            try
            {
                List<OrderDTO> OrderList = db.Order.Join(db.OlcuTabloDetay, x => x.ID, y => y.orderID, (x, y) => new { Order = x, KazanDetay = y }).Select(x => new OrderDTO
                {
                    orderID = x.Order.ID,
                    orderNo = x.Order.orderNo
                }).OrderBy(x => x.orderNo).Distinct().ToList();

                List<DTOKaliteAtolye> AtolyeList = getAtolyes();
                List<DTOKaliteUtu> UtuList = getUtuPakets();

                List<OlcuTurleri> OlcuTurList = db.OlcuTurleri.Where(x => (x.olcuTuruAd == "Zemin" || x.olcuTuruAd == "Nötr" || x.olcuTuruAd == "Random" || x.olcuTuruAd == "Ütü Paket Ön Final" || x.olcuTuruAd == "Ütü Paket Son Final" || x.olcuTuruAd == "Fason Ham Ölçü" || x.olcuTuruAd == "Ütü Paket Sevk") && x.tabloTuru == "Kazan").ToList();
                List<OlcuTurleri> OlcuTabloOlcuTurList = db.OlcuTurleri.Where(x => x.tabloTuru == "Ölçü Tablosu").ToList();

                List<GiysiTurleri> GiysiTurList = db.GiysiTurleri.OrderByDescending(x => x.giysiTuruAd).ToList();


                ViewBag.orderlar = OrderList;
                ViewBag.atolyeler = AtolyeList;
                ViewBag.olcuTurleri = OlcuTurList;
                ViewBag.olcutabloOlcuTurleri = OlcuTabloOlcuTurList;
                ViewBag.giysiTurleri = GiysiTurList;

                ViewBag.utupaketler = UtuList;

                return PartialView();
            }
            catch (Exception hata)
            {
                TempData["hata"] = "Hata " + hata.Message.ToString() + ". İçerik: " + hata.InnerException.Message.ToString();
                return PartialView();
            }
        }

        [HttpPost]
        public JsonResult FasonFormKaydet(DTOKazanDetayPost postParam)
        {
            try
            {
                if (db.OlcuTabloDetay.Any(x => x.orderID == postParam.orderID && x.olcuTuruID == postParam.oTolcuTurId && x.tabloTuru == postParam.tabloTurId))
                {

                    OlcuTabloDetay olcutabloObj = db.OlcuTabloDetay.Where(x => x.orderID == postParam.orderID && x.olcuTuruID == postParam.oTolcuTurId && x.tabloTuru == postParam.tabloTurId).FirstOrDefault();

                    KazanDetay addObj = new KazanDetay();

                    addObj.orderID = postParam.orderID;
                    addObj.atolyeID = postParam.atolyeID;
                    addObj.giysiTuruID = postParam.giysiTuruID;
                    addObj.kullaniciID = postParam.kullaniciId;
                    addObj.olcuTuruID = postParam.olcuTuruID;
                    addObj.tabloTuru = postParam.tabloTurId;
                    addObj.utuPaketID = postParam.utuId;

                    addObj.yikamaYeri = "";
                    addObj.yikamaSorumlu = "";

                    if (db.KazanDetay.Any(x => x.orderID == addObj.orderID && x.tabloTuru == addObj.tabloTuru && x.olcuTuruID == addObj.olcuTuruID && x.durum == true))
                    {
                        addObj.olcumNo = db.KazanDetay.Where(x => x.orderID == addObj.orderID && x.tabloTuru == addObj.tabloTuru && x.olcuTuruID == addObj.olcuTuruID && x.durum == true).OrderBy(s => s.olcumNo).ToList().LastOrDefault().olcumNo + 1;
                    }
                    else
                    {
                        addObj.olcumNo = 1;
                    }

                    addObj.aciklama = "";
                    addObj.tarih = DateTime.Now;
                    addObj.aktarimMi = false;
                    addObj.olcuTabloOlcuTurId = postParam.oTolcuTurId;
                    addObj.kaliteKontrolId = postParam.kontrolId;
                    addObj.kaliteInspectionId = postParam.inspectionId;
                    addObj.kaliteUrunKabulId = postParam.urunKabulId;
                    addObj.kDerece = 0;
                    addObj.kDakika = 0;

                    addObj.aparatliMi = postParam.aparatliMi;
                    addObj.utuluMu = postParam.utuluMu;

                    db.KazanDetay.Add(addObj);
                    db.SaveChanges();

                    return Json(new { kazanDetayId = addObj.id, olcuTaloDetayId = olcutabloObj.id, devamMi = false, orderId = postParam.orderID, olcuTabloOlcuTurId = postParam.oTolcuTurId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception hata)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult devamEtFasonFormKaydet(int kazanDetayId, int newKontrolId, int newUrunKabulId, int newInspectionId)
        {
            try
            {
                KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

                if (newKontrolId != 0 || newUrunKabulId != 0 || newInspectionId != 0)
                {
                    dbKazanDetay.kaliteKontrolId = newKontrolId;
                    dbKazanDetay.kaliteUrunKabulId = newUrunKabulId;
                    dbKazanDetay.kaliteInspectionId = newInspectionId;
                    db.SaveChanges();
                }

                OlcuTurleri secilenOlcuTur = dbKazanDetay.OlcuTurleri;

                int olcuTaloDetayId = db.OlcuTabloDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == dbKazanDetay.olcuTabloOlcuTurId && x.tabloTuru == dbKazanDetay.tabloTuru).FirstOrDefault().id;

                return Json(new { kazanDetayId = dbKazanDetay.id, olcuTaloDetayId = olcuTaloDetayId, devamMi = true, orderId = dbKazanDetay.orderID, olcuTabloOlcuTurId = dbKazanDetay.olcuTabloOlcuTurId }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception hata)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult FasonTablePartial(int kazanDetayId, int olcuTaloDetayId, int orderId, int olcuTabloOlcuTurId, bool devamMi = false)
        {
            try
            {
                KazanDetay dbkazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

                OlcuTabloDetay dbOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();
                int? olcuTabloId = dbOlcuTabloDetay.id;

                var olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();
                List<int> dbOlcuTabloAraList = olcuTabloAras.Select(x => x.olcuNoktaID).Distinct().ToList();

                List<OlcuNoktalari> dbOlcuNoktalariList = db.OlcuNoktalari.Where(x => dbOlcuTabloAraList.Contains(x.id) && !x.olcuNoktasi.Contains("YERİ")).ToList();

                List<string> cekmeList = db.OlcuTabloDetay.Where(x => x.enBoyCekme != "" && x.orderID == dbOlcuTabloDetay.orderID && x.olcuTuruID == dbOlcuTabloDetay.olcuTuruID && x.tabloTuru == dbOlcuTabloDetay.tabloTuru).Select(y => y.enBoyCekme).ToList();
                ViewBag.cekmeList = cekmeList;


                List<int> BedenIdList = olcuTabloAras.Select(x => x.bedenID).Distinct().ToList();
                List<Bedenler> bedenList = db.Bedenler.Where(x => BedenIdList.Contains(x.ID)).ToList().Select(y => new Bedenler { beden = y.beden, bedenSistemi = y.bedenSistemi, ID = y.ID }).ToList();
                ViewBag.bedenList = bedenList;


                ViewBag.MusteriModel = new { musteri = dbOlcuTabloDetay.Order.musteri, model = dbOlcuTabloDetay.Order.modelAd, artikel = dbOlcuTabloDetay.Order.modelNo };

                ViewBag.isYikamaOncesi = cekmeList.Count() > 0;

                ViewBag.devamMi = devamMi;
                ViewBag.aciklama = dbkazanDetay.aciklama;

                ViewBag.kazanOlcuTurId = dbkazanDetay.olcuTuruID;

                return PartialView(Tuple.Create(dbOlcuNoktalariList));
            }
            catch (Exception hata)
            {
                TempData["hata"] = "Hata " + hata.Message.ToString() + ". İçerik: " + hata.InnerException.Message.ToString();
                return PartialView();
            }
        }

        public ActionResult getVeriGirisTablosu(
            int orderId, int olcuTabloOlcuTurId, int kazanDetayId, int silinenPno,
            string cekme, int bedenId, List<OlcuNokIdwithSiraNoDto> secilenNoktalar, List<DtoOlcuNokDeger> returnYeriList, Boolean isYikamaOncesi,
            List<pListDto> pantolonList, List<kazanGirilenListDto> degerler, Boolean silindiMi, List<DtoOlcuNokDeger> olmasiGerekenList)
        {

            #region orijinal Kodlar
            if (pantolonList == null) { pantolonList = new List<pListDto>(); }
            if (degerler == null) { degerler = new List<kazanGirilenListDto>(); }
            if (olmasiGerekenList == null) { olmasiGerekenList = new List<DtoOlcuNokDeger>(); }
            if (cekme == "") { cekme = null; }
            if (returnYeriList == null) { returnYeriList = new List<DtoOlcuNokDeger>(); }
            List<DtoOlcuNokDeger> returnList = new List<DtoOlcuNokDeger>();



            if (!silindiMi)
            {
                Bedenler secilenBeden = db.Bedenler.Where(x => x.ID == bedenId).FirstOrDefault();

                if (pantolonList.Count() == 0)
                {
                    pantolonList.Add(new pListDto { bedenad = secilenBeden.beden, cekme = cekme, pno = 1, bedenid = secilenBeden.ID, bpantno = 1 });
                }
                else
                {
                    if (pantolonList.Where(x => x.bedenad == secilenBeden.beden).Count() == 0)
                    {
                        pantolonList.Add(new pListDto { bedenad = secilenBeden.beden, cekme = cekme, pno = pantolonList.OrderBy(x => x.pno).LastOrDefault().pno + 1, bedenid = secilenBeden.ID, bpantno = 1 });
                    }
                    else
                    {
                        pantolonList.Add(new pListDto { bedenad = secilenBeden.beden, cekme = cekme, pno = pantolonList.OrderBy(x => x.pno).LastOrDefault().pno + 1, bedenid = secilenBeden.ID, bpantno = pantolonList.Where(x => x.bedenad == secilenBeden.beden).OrderBy(x => x.bpantno).LastOrDefault().bpantno + 1 });

                    }

                }

                int lastPno = pantolonList.OrderBy(x => x.pno).LastOrDefault().pno;
                int lastBpantno = pantolonList.Where(x => x.bedenad == secilenBeden.beden).OrderBy(x => x.bpantno).LastOrDefault().bpantno;

                foreach (var item in secilenNoktalar.OrderBy(x => x.sirano).ToList())
                {
                    KazanAra newKazanAra = new KazanAra()
                    {
                        bedenID = bedenId,
                        kazanDetayID = kazanDetayId,
                        olcuNoktaID = item.olcunokid,
                        deger = null,
                        pantNo = lastPno,
                        enBoyCekme = cekme ?? "",
                        bpantNo = lastBpantno
                    };


                    kazanGirilenListDto newdegerler = new kazanGirilenListDto()
                    {
                        bedenad = secilenBeden.beden,
                        bedenid = bedenId,
                        cekme = cekme,
                        olcunokid = item.olcunokid,
                        deger = null,
                        pno = lastPno,
                        bpantno = lastBpantno
                    };

                    degerler.Add(newdegerler);

                    db.KazanAra.Add(newKazanAra);
                }


                if (!db.KazanHesaplama.Any(x => x.kazanDetayID == kazanDetayId))
                {
                    foreach (var item in secilenNoktalar.OrderBy(x => x.sirano).ToList())
                    {
                        KazanHesaplama addKazanHesaplama = new KazanHesaplama()
                        {
                            kazanDetayID = kazanDetayId,
                            asilTablo = 0,
                            gerceklesenTolCekme = 0,
                            id = 0,
                            olcuNoktasiID = item.olcunokid,
                            oncekiTablo = 0,
                            ortalamaDeger = 0,
                            uygulananTolCekme = 0,
                            yoOlculen = 0,
                        };

                        db.KazanHesaplama.Add(addKazanHesaplama);
                    }
                }


                db.SaveChanges();


                if (isYikamaOncesi)
                {
                    if (!olmasiGerekenList.Any(x => x.bedenId == bedenId && x.cekme == cekme))
                    {

                        OlcuTabloDetay objDetay = db.OlcuTabloDetay.Where(x => x.orderID == orderId && x.olcuTuruID == olcuTabloOlcuTurId && x.enBoyCekme == cekme && x.tabloTuru == 0).FirstOrDefault();

                        int olcuTabloId = objDetay.id;

                        List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

                        foreach (var item in olcuTabloAras.Where(x => x.bedenID == bedenId).ToList())
                        {

                            DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();

                            addObj.olcuNokId = item.OlcuNoktalari.id; // OlcunNoktasiObj.id;
                            addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;

                            addObj.cekme = cekme;
                            addObj.bedenAd = secilenBeden.beden;
                            addObj.bedenId = bedenId;

                            addObj.deger = item.deger.ToString();

                            if (secilenNoktalar.Any(x => x.olcunokid == item.olcuNoktaID))
                            {
                                addObj.sirano = secilenNoktalar.Where(x => x.olcunokid == item.OlcuNoktalari.id).FirstOrDefault().sirano;
                                returnList.Add(addObj);

                            }
                            else
                            {
                                addObj.sirano = 1;
                            }

                            if (addObj.olcuNokAd.Contains("YERİ"))
                            {
                                returnYeriList.Add(addObj);
                            }


                        }
                    }
                    else
                    {
                        returnList = olmasiGerekenList.Where(x => x.bedenId == bedenId && x.cekme == cekme).ToList();
                    }
                }
                else
                {
                    if (!olmasiGerekenList.Any(x => x.bedenId == bedenId))
                    {

                        OlcuTabloDetay objDetay = db.OlcuTabloDetay.Where(x => x.orderID == orderId && x.olcuTuruID == olcuTabloOlcuTurId && x.tabloTuru == 1).FirstOrDefault();


                        int olcuTabloId = objDetay.id;

                        List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

                        foreach (var item in olcuTabloAras.Where(x => x.bedenID == bedenId).ToList())
                        {

                            DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();

                            addObj.olcuNokId = item.OlcuNoktalari.id;
                            addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;
                            addObj.deger = item.deger.ToString();
                            addObj.cekme = null;
                            addObj.bedenAd = secilenBeden.beden;
                            addObj.bedenId = bedenId;

                            if (secilenNoktalar.Any(x => x.olcunokid == item.olcuNoktaID))
                            {
                                addObj.sirano = secilenNoktalar.Where(x => x.olcunokid == item.OlcuNoktalari.id).FirstOrDefault().sirano;
                                returnList.Add(addObj);
                            }
                            else
                            {
                                addObj.sirano = 1;
                            }

                            if (addObj.olcuNokAd.Contains("YERİ"))
                            {
                                returnYeriList.Add(addObj);
                            }

                        }
                    }
                    else
                    {
                        returnList = olmasiGerekenList.Where(x => x.bedenId == bedenId).ToList();
                    }
                }




            }
            else
            {
                pListDto silinenP = pantolonList.Where(x => x.pno == silinenPno).FirstOrDefault();

                var pAzalacakList = pantolonList.Where(x => x.pno > silinenPno).OrderBy(y => y.pno).ToList();
                var bpantAzalacakList = pantolonList.Where(x => x.bedenid == silinenP.bedenid && x.bpantno > silinenP.bpantno).OrderBy(y => y.pno).ToList();

                List<KazanAra> dbKazanAraList = db.KazanAra.Where(x => x.kazanDetayID == kazanDetayId).ToList();

                db.KazanAra.RemoveRange(dbKazanAraList.Where(x => x.pantNo == silinenPno).ToList());
                degerler.RemoveAll(s => s.pno == silinenPno);
                pantolonList.RemoveAll(s => s.pno == silinenPno);
                dbKazanAraList.RemoveAll(s => s.pantNo == silinenPno);

                foreach (var item in pAzalacakList)
                {
                    var degerlerAzalacakList = degerler.Where(x => x.pno == item.pno).ToList();

                    for (int i = 0; i < degerlerAzalacakList.Count(); i++)
                    {
                        degerlerAzalacakList[i].pno--;
                    }

                    var dbKazanAraAzalacakList = dbKazanAraList.Where(x => x.pantNo == item.pno).ToList();

                    for (int i = 0; i < dbKazanAraAzalacakList.Count(); i++)
                    {
                        dbKazanAraAzalacakList[i].pantNo--;
                    }

                    pantolonList.Where(x => x.pno == item.pno).FirstOrDefault().pno--;
                }

                foreach (var item in bpantAzalacakList)
                {
                    var degerlerAzalacakList = degerler.Where(x => x.pno == item.pno).ToList();

                    for (int i = 0; i < degerlerAzalacakList.Count(); i++)
                    {
                        degerlerAzalacakList[i].bpantno--;
                    }

                    var dbKazanAraAzalacakList = dbKazanAraList.Where(x => x.pantNo == item.pno).ToList();

                    for (int i = 0; i < dbKazanAraAzalacakList.Count(); i++)
                    {
                        dbKazanAraAzalacakList[i].bpantNo--;
                    }

                    pantolonList.Where(x => x.pno == item.pno).FirstOrDefault().bpantno--;
                }


                kazanGirilenListDto sonDeger = degerler.OrderBy(x => x.pno).ToList().LastOrDefault();
                if (sonDeger != null)
                {
                    if (isYikamaOncesi)
                    {
                        returnList = olmasiGerekenList.Where(x => x.bedenAd == sonDeger.bedenad && x.cekme == sonDeger.cekme).ToList();
                    }
                    else
                    {
                        returnList = olmasiGerekenList.Where(x => x.bedenAd == sonDeger.bedenad).ToList();
                    }
                }

                db.SaveChanges();

            }

            ViewBag.silindiMi = silindiMi;
            ViewBag.olcuYeriList = returnYeriList;


            #endregion


            return PartialView(Tuple.Create(returnList.OrderBy(x => x.sirano).ToList(), pantolonList.OrderByDescending(x => x.pno).ToList(), degerler, secilenNoktalar));
        }

        public ActionResult setVeriGirisTablosu(int kazanDetayId, int olcuTaloDetayId)
        {
            KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();
            List<DtoOlcuNokDeger> returnList = new List<DtoOlcuNokDeger>();
            List<DtoOlcuNokDeger> returnYeriList = new List<DtoOlcuNokDeger>();

            List<pListDto> pantolonList = new List<pListDto>();
            List<kazanGirilenListDto> degerler = new List<kazanGirilenListDto>();

            List<OlcuNokIdwithSiraNoDto> seciliOlcuNokIdList = new List<OlcuNokIdwithSiraNoDto>();

            List<DtoOlcuNokDeger> olmasiGerekenListAll = new List<DtoOlcuNokDeger>();

            List<KazanAra> kazanAraList = db.KazanAra.Where(x => x.kazanDetayID == dbKazanDetay.id).ToList();
            List<KazanHesaplama> kazanHesaplamaList = db.KazanHesaplama.Where(x => x.kazanDetayID == dbKazanDetay.id).ToList();

            foreach (var item in kazanAraList.Select(x => x.pantNo).Distinct())
            {
                KazanAra KazanDetayItem = kazanAraList.Where(x => x.pantNo == item).FirstOrDefault();

                pantolonList.Add(new pListDto { bedenad = KazanDetayItem.Bedenler.beden, bedenid = KazanDetayItem.bedenID, bpantno = (int)KazanDetayItem.bpantNo, cekme = KazanDetayItem.enBoyCekme == "" ? null : KazanDetayItem.enBoyCekme, pno = (int)KazanDetayItem.pantNo });
            }

            foreach (var item in kazanAraList)
            {
                degerler.Add(new kazanGirilenListDto { deger = item.deger, olcunokid = item.olcuNoktaID, bedenad = item.Bedenler.beden, bedenid = item.bedenID, bpantno = (int)item.bpantNo, cekme = item.enBoyCekme == "" ? null : item.enBoyCekme, pno = (int)item.pantNo });
            }

            if (dbKazanDetay.tabloTuru == 0)
            {
                OlcuTabloDetay firstOlcuTablo = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                foreach (var pantolonItem in pantolonList)
                {
                    if (!olmasiGerekenListAll.Any(x => x.bedenId == pantolonItem.bedenid && x.cekme == pantolonItem.cekme))
                    {
                        OlcuTabloDetay objDetay = db.OlcuTabloDetay.Where(x => x.orderID == firstOlcuTablo.orderID && x.olcuTuruID == firstOlcuTablo.olcuTuruID && x.enBoyCekme == pantolonItem.cekme && x.tabloTuru == firstOlcuTablo.tabloTuru).FirstOrDefault();

                        int olcuTabloId = objDetay.id;

                        List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

                        foreach (var item in olcuTabloAras.Where(x => x.bedenID == pantolonItem.bedenid).ToList())
                        {
                            DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();

                            addObj.olcuNokId = item.OlcuNoktalari.id;
                            addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;

                            addObj.cekme = pantolonItem.cekme;
                            addObj.bedenAd = pantolonItem.bedenad;
                            addObj.bedenId = pantolonItem.bedenid;

                            addObj.deger = item.deger.ToString();

                            if (kazanHesaplamaList.Select(s => s.olcuNoktasiID).ToList().Any(x => x == item.olcuNoktaID))
                            {
                                addObj.sirano = kazanHesaplamaList.Select(s => s.olcuNoktasiID).ToList().FindIndex(x => x == item.OlcuNoktalari.id) + 1;
                                if (!seciliOlcuNokIdList.Any(x => x.olcunokid == item.OlcuNoktalari.id))
                                {
                                    seciliOlcuNokIdList.Add(new OlcuNokIdwithSiraNoDto { olcunokid = item.OlcuNoktalari.id, sirano = addObj.sirano });
                                }
                            }
                            else
                            {
                                addObj.sirano = 1;
                            }

                            olmasiGerekenListAll.Add(addObj);

                        }

                    }
                }



            }
            else
            {
                OlcuTabloDetay objDetay = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                foreach (var pantolonItem in pantolonList)
                {
                    if (!olmasiGerekenListAll.Any(x => x.bedenId == pantolonItem.bedenid))
                    {
                        int olcuTabloId = objDetay.id;

                        List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

                        foreach (var item in olcuTabloAras.Where(x => x.bedenID == pantolonItem.bedenid))
                        {
                            DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();
                            addObj.olcuNokId = item.OlcuNoktalari.id;
                            addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;
                            addObj.deger = item.deger.ToString();
                            addObj.cekme = null;
                            addObj.bedenAd = pantolonItem.bedenad;
                            addObj.bedenId = pantolonItem.bedenid;

                            if (kazanHesaplamaList.Select(s => s.olcuNoktasiID).ToList().Any(x => x == item.olcuNoktaID))
                            {
                                addObj.sirano = kazanHesaplamaList.Select(s => s.olcuNoktasiID).ToList().FindIndex(x => x == item.OlcuNoktalari.id) + 1;

                                if (!seciliOlcuNokIdList.Any(x => x.olcunokid == item.OlcuNoktalari.id))
                                {
                                    seciliOlcuNokIdList.Add(new OlcuNokIdwithSiraNoDto { olcunokid = item.OlcuNoktalari.id, sirano = addObj.sirano });
                                }
                            }
                            else
                            {
                                addObj.sirano = 1;
                            }

                            olmasiGerekenListAll.Add(addObj);

                        }

                    }
                }



            }

            if (kazanAraList.Count() != 0)
            {
                Bedenler lastBeden = kazanAraList.LastOrDefault().Bedenler;
                string lastCekme = kazanAraList.LastOrDefault().enBoyCekme;
                string serachCekme = lastCekme == "" ? null : lastCekme;

                returnList = olmasiGerekenListAll.Where(x => kazanHesaplamaList.Select(y => y.olcuNoktasiID).Contains(x.olcuNokId) && x.bedenId == lastBeden.ID && x.cekme == serachCekme).ToList();
            }



            returnYeriList = olmasiGerekenListAll.Where(x => x.olcuNokAd.Contains("YERİ")).ToList();

            ViewBag.silindiMi = true;
            ViewBag.olcuYeriList = returnYeriList;

            ViewBag.setReturnList = olmasiGerekenListAll.Where(x => kazanHesaplamaList.Select(y => y.olcuNoktasiID).Contains(x.olcuNokId) && !x.olcuNokAd.Contains("YERİ")).ToList();

            if (seciliOlcuNokIdList.Count() == 0)
            {
                int sayacSiraNo = 1;

                foreach (var item in kazanHesaplamaList.ToList())
                {
                    seciliOlcuNokIdList.Add(new OlcuNokIdwithSiraNoDto { olcunokid = item.olcuNoktasiID, sirano = sayacSiraNo });
                    sayacSiraNo++;
                }
            }

            return PartialView("~/Views/Fason/getVeriGirisTablosu.cshtml", Tuple.Create(returnList.OrderBy(x => x.sirano).ToList(), pantolonList.OrderByDescending(x => x.pno).ToList(), degerler, seciliOlcuNokIdList));

        }

        public ActionResult getOlcuYerleri(string cekme, int bedenId, List<DtoOlcuNokDeger> olcuYerleriList)
        {
            if (cekme == "") { cekme = null; }
            List<DtoOlcuNokDeger> returnList = olcuYerleriList.Where(x => x.cekme == cekme & x.bedenId == bedenId).ToList();

            return PartialView("~/Views/KazanFormu/olcuYerleriTableP.cshtml", Tuple.Create(returnList));

        }

        public ActionResult changeOlcuNoktalari(int olcuTaloDetayId, int kazanDetayId, List<OlcuNokIdwithSiraNoDto> secilenNoktalar, List<OlcuNokIdwithSiraNoDto> oldSecilenNoktalar)
        {
            KazanDetay kazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

            List<KazanAra> kazanAras = db.KazanAra.Where(x => x.kazanDetayID == kazanDetay.id).ToList();
            List<KazanHesaplama> kazanHesaplamas = db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetay.id).ToList(); 

            List<KazanAra> addKazanAraList = new List<KazanAra>();

            foreach (var newOlcuNokta in secilenNoktalar)
            {
                if (!oldSecilenNoktalar.Any(x => x.olcunokid == newOlcuNokta.olcunokid))
                {
                    foreach (var itemPno in kazanAras.Select(x => x.pantNo).Distinct())
                    {
                        KazanAra pantolonKazanAra = kazanAras.Where(x => x.pantNo == itemPno).FirstOrDefault();

                        addKazanAraList.Add(new KazanAra { id = 0, kazanDetayID = pantolonKazanAra.kazanDetayID, olcuNoktaID = newOlcuNokta.olcunokid, bedenID = pantolonKazanAra.bedenID, enBoyCekme = pantolonKazanAra.enBoyCekme, deger = null, pantNo = pantolonKazanAra.pantNo, bpantNo = pantolonKazanAra.bpantNo });
                    }
                }

            }


            db.KazanAra.AddRange(addKazanAraList.OrderBy(x => x.pantNo).ThenBy(y => y.bpantNo).ToList());


            foreach (var oldOlcuNokta in oldSecilenNoktalar)
            {
                if (!secilenNoktalar.Any(x => x.olcunokid == oldOlcuNokta.olcunokid))
                {
                    db.KazanAra.RemoveRange(kazanAras.Where(x => x.olcuNoktaID == oldOlcuNokta.olcunokid).ToList());
                }
            }


            db.KazanHesaplama.RemoveRange(kazanHesaplamas.ToList());


            foreach (var item in secilenNoktalar.OrderBy(x => x.sirano).ToList())
            {
                KazanHesaplama addKazanHesaplama = new KazanHesaplama()
                {
                    kazanDetayID = kazanDetayId,
                    asilTablo = 0,
                    gerceklesenTolCekme = 0,
                    id = 0,
                    olcuNoktasiID = item.olcunokid,
                    oncekiTablo = 0,
                    ortalamaDeger = 0,
                    uygulananTolCekme = 0,
                    yoOlculen = 0,
                };

                db.KazanHesaplama.Add(addKazanHesaplama);
            }


            db.SaveChanges();



            OlcuTabloDetay dbOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();
            int olcuTabloId = dbOlcuTabloDetay.id;

            List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

            List<int> dbOlcuTabloAraList = olcuTabloAras.Select(x => x.olcuNoktaID).Distinct().ToList();

            List<OlcuNoktalari> dbOlcuNoktalariList = db.OlcuNoktalari.Where(x => dbOlcuTabloAraList.Contains(x.id) && !x.olcuNoktasi.Contains("YERİ")).ToList();

            List<string> cekmeList = db.OlcuTabloDetay.Where(x => x.enBoyCekme != "" && x.orderID == dbOlcuTabloDetay.orderID && x.olcuTuruID == dbOlcuTabloDetay.olcuTuruID && x.tabloTuru == dbOlcuTabloDetay.tabloTuru).Select(y => y.enBoyCekme).ToList();
            ViewBag.cekmeList = cekmeList;


            List<int> BedenIdList = olcuTabloAras.Select(x => x.bedenID).Distinct().ToList();
            List<Bedenler> bedenList = db.Bedenler.Where(x => BedenIdList.Contains(x.ID)).ToList().Select(y => new Bedenler { beden = y.beden, bedenSistemi = y.bedenSistemi, ID = y.ID }).ToList();
            ViewBag.bedenList = bedenList;


            ViewBag.MusteriModel = new { musteri = dbOlcuTabloDetay.Order.musteri, model = dbOlcuTabloDetay.Order.modelAd, artikel = dbOlcuTabloDetay.Order.modelNo };

            ViewBag.isYikamaOncesi = cekmeList.Count() > 0;

            ViewBag.devamMi = true;
            ViewBag.aciklama = kazanDetay.aciklama;

            ViewBag.kazanOlcuTurId = kazanDetay.olcuTuruID;

            return PartialView("~/Views/Fason/FasonTablePartial.cshtml", Tuple.Create(dbOlcuNoktalariList));


        }

        [HttpPost]
        public JsonResult SetKazanAra(int bedenID, int kazanDetayID, int olcuNoktaID, double? deger, int pantNo, string enBoyCekme, int bpantNo)
        {

            if (db.KazanAra.Any(x => x.bedenID == bedenID && x.kazanDetayID == kazanDetayID && x.olcuNoktaID == olcuNoktaID && x.pantNo == pantNo && x.enBoyCekme == enBoyCekme && x.bpantNo == bpantNo))
            {
                KazanAra newKazanAra = db.KazanAra.Where(x => x.bedenID == bedenID && x.kazanDetayID == kazanDetayID && x.olcuNoktaID == olcuNoktaID && x.pantNo == pantNo && x.enBoyCekme == enBoyCekme && x.bpantNo == bpantNo).FirstOrDefault();
                newKazanAra.deger = deger;
                db.SaveChanges();
            }
            else
            {
                KazanAra newKazanAra = new KazanAra()
                {
                    bedenID = bedenID,
                    kazanDetayID = kazanDetayID,
                    olcuNoktaID = olcuNoktaID,
                    deger = deger,
                    pantNo = pantNo,
                    enBoyCekme = enBoyCekme,
                    bpantNo = bpantNo
                };

                db.KazanAra.Add(newKazanAra);
                db.SaveChanges();
            }


            return Json("ok", JsonRequestBehavior.AllowGet);



        }

        public JsonResult kaydet(string aciklama, int kazanDetayId, bool durum, string atolyeAd, string utuPaketAd)
        {
            try
            {
                KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();
                dbKazanDetay.aciklama = aciklama;
                dbKazanDetay.durum = durum;

                db.SaveChanges();

                List<KazanAra> kazanAras = db.KazanAra.Where(x => x.kazanDetayID == dbKazanDetay.id).ToList();

                foreach (var item in kazanAras.Select(x => x.olcuNoktaID).Distinct())
                {
                    double ortalamaDeger = 0;
                    double yoOlculen = 0;
                    double gerceklesenTolCekme = 0;
                    double oncekiTablo = 0;
                    double uygulananTolCekme = 0;
                    double asilTablo = 0;

                    KazanHesaplama newKazanHesaplama = new KazanHesaplama();
                    newKazanHesaplama.kazanDetayID = kazanDetayId;
                    newKazanHesaplama.olcuNoktasiID = Convert.ToInt32(item);


                    if (dbKazanDetay.tabloTuru == 0)
                    {

                        SqlParameter param1 = new SqlParameter("@KazanDetayID", kazanDetayId);
                        SqlParameter param2 = new SqlParameter("@olcuNoktaID", Convert.ToInt32(item));

                        var cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandText = "[dbo].[OrtalamaDegerBulmaYO] @KazanDetayID, @olcuNoktaID";
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);

                        db.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();

                        reader.Read();
                        if (!Convert.IsDBNull(reader.GetValue(0)))
                        {
                            ortalamaDeger = Math.Round(reader.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                            newKazanHesaplama.ortalamaDeger = ortalamaDeger;
                        }
                        else
                        {
                            newKazanHesaplama.ortalamaDeger = 0;
                        }
                        reader.Close();

                        SqlParameter yoParam = new SqlParameter("@kazandetayID", kazanDetayId);
                        SqlParameter yoParam1 = new SqlParameter("@OrderID", dbKazanDetay.orderID);
                        SqlParameter yoParam2 = new SqlParameter("@olcuNoktaID", item);

                        var cmd2 = db.Database.Connection.CreateCommand();
                        cmd2.CommandText = "[dbo].[YikamaOncesiKalip] @OrderID,@olcuNoktaID,@kazanDetayID";

                        cmd2.Parameters.Add(yoParam1);
                        cmd2.Parameters.Add(yoParam2);
                        cmd2.Parameters.Add(yoParam);

                        var reader2 = cmd2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                if (!Convert.IsDBNull(reader2.GetValue(0)))
                                {
                                    oncekiTablo = Math.Round(reader2.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                                }
                                newKazanHesaplama.oncekiTablo = oncekiTablo;

                                if (!Convert.IsDBNull(reader2.GetValue(1)))
                                {
                                    uygulananTolCekme = Math.Round(reader2.GetDouble(1), 2, MidpointRounding.AwayFromZero);
                                }

                                newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;

                                if (!Convert.IsDBNull(reader2.GetValue(2)))
                                {
                                    asilTablo = Math.Round(reader2.GetDouble(2), 2, MidpointRounding.AwayFromZero);
                                }
                                newKazanHesaplama.asilTablo = asilTablo;
                            }
                            reader2.Close();
                        }


                        yoOlculen = Math.Round(ortalamaDeger + asilTablo, 2, MidpointRounding.AwayFromZero);
                        newKazanHesaplama.yoOlculen = yoOlculen;

                        gerceklesenTolCekme = Math.Round(oncekiTablo - yoOlculen, 2, MidpointRounding.AwayFromZero);
                        newKazanHesaplama.gerceklesenTolCekme = gerceklesenTolCekme;


                    }
                    else if (dbKazanDetay.tabloTuru == 1)
                    {
                        int olcuturuID = 0;

                        int yoKazanDetayID = 0;

                        SqlParameter param1 = new SqlParameter("@KazanDetayID", kazanDetayId);
                        SqlParameter param2 = new SqlParameter("@olcuNoktaID", Convert.ToInt32(item));

                        var cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandText = "[dbo].[OrtalamaDegerBulma] @KazanDetayID, @olcuNoktaID";
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);

                        db.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();

                        reader.Read();
                        if (!reader.IsDBNull(0))
                        {
                            ortalamaDeger = Math.Round(reader.GetDouble(0), 2, MidpointRounding.AwayFromZero);

                        }
                        else
                        {
                            ortalamaDeger = 0;
                        }
                        newKazanHesaplama.ortalamaDeger = ortalamaDeger;
                        reader.Close();


                        if (Convert.ToInt32(dbKazanDetay.olcuTuruID) == 18 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 19)
                        {
                            olcuturuID = 17;

                            yoKazanDetayID = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == olcuturuID && x.tabloTuru == 0).Select(x => x.id).FirstOrDefault();

                            if (yoKazanDetayID == 0)
                            {
                                yoKazanDetayID = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == 16 && x.tabloTuru == 0 && db.KazanAra.Where(y=>y.kazanDetayID == x.id).FirstOrDefault() != null).Select(x => x.id).FirstOrDefault();
                            }
                        }
                        else
                        {
                            olcuturuID = dbKazanDetay.olcuTuruID;

                            yoKazanDetayID = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == olcuturuID && x.tabloTuru == 0 && db.KazanAra.Where(y => y.kazanDetayID == x.id).FirstOrDefault() != null).Select(x => x.id).FirstOrDefault();
                        }



                        var cmd2 = db.Database.Connection.CreateCommand();
                        SqlParameter ysParam1 = new SqlParameter("@kazanDetayID", dbKazanDetay.id);
                        SqlParameter ysParam2 = new SqlParameter("@kazanDetayIDYO", yoKazanDetayID);
                        SqlParameter ysParam3 = new SqlParameter("@olcuNoktaID", item);
                        SqlParameter ysParam4 = new SqlParameter("@orderID", dbKazanDetay.orderID);

                        cmd2.CommandText = "[dbo].[YikamaSonrasiCekme] @kazanDetayID,@kazanDetayIDYO,@olcuNoktaID,@orderID";

                        cmd2.Parameters.Add(ysParam1);
                        cmd2.Parameters.Add(ysParam2);
                        cmd2.Parameters.Add(ysParam3);
                        cmd2.Parameters.Add(ysParam4);

                        var reader2 = cmd2.ExecuteReader();

                        while (reader2.Read())
                        {
                            if (!Convert.IsDBNull(reader2.GetValue(0)))
                            {
                                yoOlculen = Math.Round(reader2.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                            }

                            newKazanHesaplama.yoOlculen = yoOlculen;

                            if (!Convert.IsDBNull(reader2.GetValue(1)))
                            {
                                uygulananTolCekme = Math.Round(reader2.GetDouble(1), 2, MidpointRounding.AwayFromZero);
                            }

                            newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;

                            if (!Convert.IsDBNull(reader2.GetValue(2)))
                            {
                                asilTablo = Math.Round(reader2.GetDouble(2), 2, MidpointRounding.AwayFromZero);
                            }

                            newKazanHesaplama.asilTablo = asilTablo;
                        }
                        reader2.Close();

                        oncekiTablo = Math.Round(asilTablo + ortalamaDeger, 2, MidpointRounding.AwayFromZero);
                        newKazanHesaplama.oncekiTablo = oncekiTablo;

                        if (yoOlculen != 0)
                        {
                            gerceklesenTolCekme = Math.Round(((yoOlculen - oncekiTablo) / yoOlculen) * 100, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            gerceklesenTolCekme = 0;
                        }
                        newKazanHesaplama.gerceklesenTolCekme = gerceklesenTolCekme;
                    }


                    if (db.KazanHesaplama.Any(x => x.kazanDetayID == kazanDetayId && x.olcuNoktasiID == item))
                    {
                        KazanHesaplama dbKazanHesaplama = db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId && x.olcuNoktasiID == item).FirstOrDefault();
                        dbKazanHesaplama.asilTablo = newKazanHesaplama.asilTablo;
                        dbKazanHesaplama.gerceklesenTolCekme = newKazanHesaplama.gerceklesenTolCekme;
                        dbKazanHesaplama.oncekiTablo = newKazanHesaplama.oncekiTablo;
                        dbKazanHesaplama.ortalamaDeger = newKazanHesaplama.ortalamaDeger;
                        dbKazanHesaplama.uygulananTolCekme = newKazanHesaplama.uygulananTolCekme;
                        dbKazanHesaplama.yoOlculen = newKazanHesaplama.yoOlculen;

                    }
                    else
                    {
                        db.KazanHesaplama.Add(newKazanHesaplama);
                    }


                    db.Database.Connection.Close();

                    db.SaveChanges();

                }

                try
                {
                    if (dbKazanDetay.durum == true)
                    {
                        sendMail(dbKazanDetay, atolyeAd, utuPaketAd);
                    }

                    return Json(new { message = "Kayıt Başarılı", hata = "" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    throw;
                    return Json(new { message = "Kayıt Başarılı", hata = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { message = "Kayıt sırasında bir hata oluştu", hata = e.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult goKalite(int personelId)
        {
            PersonelTablo personel = db.PersonelTablo.Where(x => x.id == personelId).FirstOrDefault();
            return Json(new { pAd = personel.personelkAdi, pSif = personel.personelSifre }, JsonRequestBehavior.AllowGet);
        }

        public List<DTOKaliteAtolye> getAtolyes()
        {
            List<DTOKaliteAtolye> returnAtolyeList = new List<DTOKaliteAtolye>();

            using (var connection = new SqlConnection(ConnectionStrings.DikimhaneConnection))
            {
                connection.Open();

                SqlCommand cmd_Atolyes = new SqlCommand(@"select * from Atolyes", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd_Atolyes);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                returnAtolyeList = ConvertDataTable<DTOKaliteAtolye>(dt);

                connection.Close();
            }

            return returnAtolyeList;
        }

        public List<DTOKaliteUtu> getUtuPakets()
        {
            List<DTOKaliteUtu> returnUtuList = new List<DTOKaliteUtu>();

            using (var connection = new SqlConnection(ConnectionStrings.DikimhaneConnection))
            {
                connection.Open();

                SqlCommand cmd_Utupaket = new SqlCommand(@"select * from Utupakets", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd_Utupaket);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                returnUtuList = ConvertDataTable<DTOKaliteUtu>(dt);

                connection.Close();
            }

            return returnUtuList;
        }

        public void sendMail(KazanDetay dbKazanDetay, string atolyeAd = "", string utuPaketAd = "")
        {

            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Recore Quality Managing", "recorequality@realkom.com"));

                List<string> toList = new List<string> { "nursen@realkom.com" };

                List<string> ccList = new List<string> { };

                if (dbKazanDetay.atolyeID != 0)
                {
                    List<DTOKaliteAtolye> AtolyeList = new List<DTOKaliteAtolye>();

                    using (var connection = new SqlConnection(ConnectionStrings.DikimhaneConnection))
                    {
                        connection.Open();

                        SqlCommand cmd_Atolyes = new SqlCommand(@"select * from Atolyes where Id = " + dbKazanDetay.atolyeID, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd_Atolyes);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        AtolyeList = ConvertDataTable<DTOKaliteAtolye>(dt);

                        connection.Close();
                    }

                    DTOKaliteAtolye atolye = AtolyeList.FirstOrDefault();

                    if (atolye.Email != null && atolye.Email != "")
                    {
                        toList.Add(atolye.Email);
                    }


                }

                foreach (string itemTO in toList)
                {
                    message.To.Add(new MailboxAddress("", itemTO));
                }

                foreach (string itemCC in ccList)
                {
                    message.Cc.Add(new MailboxAddress("", itemCC));
                }

                message.Subject += dbKazanDetay.Order.orderNo + " Ölçü Raporu";


                string atolyeStr = "";
                if (dbKazanDetay.atolyeID != 0)
                {
                    if (atolyeAd != null && atolyeAd != "")
                    {
                        atolyeStr = "<span><b> Atölye : </b> " + atolyeAd + "</span>";
                    }
                    else
                    {
                        atolyeStr = "<span><b> Atölye : </b> " + getAtolyes().Where(x => x.Id == dbKazanDetay.atolyeID).FirstOrDefault().AtolyeAd + "</span>";
                    }
                }


                string utuStr = "";
                if (dbKazanDetay.utuPaketID != 0)
                {
                    if (utuPaketAd != null && utuPaketAd != "")
                    {
                        utuStr = "&nbsp; -  &nbsp; <span><b> Ütü/Paket : </b>" + utuPaketAd + " </span> &nbsp;";
                    }
                    else
                    {
                        utuStr = "&nbsp; -  &nbsp; <span><b> Ütü/Paket : </b>" + getUtuPakets().Where(x => x.Id == dbKazanDetay.utuPaketID).FirstOrDefault().UtuPaketAd + " </span> &nbsp;";
                    }
                }


                string yoysStr = "";
                if (dbKazanDetay.tabloTuru == 0)
                {
                    yoysStr = "Yıkama Öncesi";
                }
                else
                {
                    yoysStr = "Yıkama Sonrası";
                }


                string aciklamaStr = "";
                if (dbKazanDetay.aciklama != "")
                    aciklamaStr = "<b>Not : </b>" + dbKazanDetay.aciklama + "<br><br>";


                string linkKaliteStr = "";
                if (dbKazanDetay.kaliteKontrolId != 0)
                {
                    linkKaliteStr = "<a target='_blank' href='https://atolye.realkom.com/RaporKontrol?kId=" + dbKazanDetay.kaliteKontrolId + "'><span>Kalite Raporuna Git</span></a> ";
                }

                string linkInsStr = "";
                if (dbKazanDetay.kaliteInspectionId != 0)
                {
                    linkInsStr = "<a target='_blank' href='https://atolye.realkom.com/RaporKontrolInspection?kId=" + dbKazanDetay.kaliteInspectionId + "'>Inspection Raporuna Git</a>";
                }

                var builder = new BodyBuilder();

                builder.HtmlBody = string.Format(@"<html><head><style type='text/css'></style></head><body>" +
                        "<span><b>" + dbKazanDetay.OlcuTurleri.olcuTuruAd + " " + yoysStr + " Ölçü Raporu</b></span>" +

                        "<br> <br>" +

                        "<span><b> Order : </b>" + dbKazanDetay.Order.orderNo + "</span> &nbsp; -  &nbsp;" +
                        "<span><b> Model : </b>" + dbKazanDetay.Order.modelAd + "</span> &nbsp; - &nbsp;" +
                        "<span><b> Tarih : </b>" + dbKazanDetay.tarih.ToString("dd.MM.yyyy HH:mm:ss") + "</span> &nbsp;" +

                        "<br> <br>" +
                        "<span><b> Kullanıcı : </b>" + dbKazanDetay.PersonelTablo.personelAd + " " + dbKazanDetay.PersonelTablo.personelSoyad + "</span> &nbsp; -  &nbsp;" +
                         atolyeStr +
                         utuStr +
                        "<br><br>" +

                        aciklamaStr + "<a target='_blank' href='https://olcu.realkom.com/Raporlama/TabloGoster?id=" + dbKazanDetay.id + "&tur=0'><span>Öçlü Raporuna Git</span></a>" +
                        "<br><br>" +
                        linkKaliteStr + linkInsStr +
                        "<br><br></body></html>"
                );


                message.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp-mail.outlook.com", 587, false);
                    client.Authenticate("recorequality@realkom.com", "Re*6836#al");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {

                throw;
            }

        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr.IsNull(column.ColumnName) ? null : dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }



    }
}