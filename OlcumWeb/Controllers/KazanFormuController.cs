using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;
using OlcumWeb.Models.RecoreProd;
using OlcumWeb.Models.YalinUretim;

namespace OlcumWeb.Controllers
{

    public class KazanFormuController : Controller
    {
        RecoreProdContext recoreProdContext = new RecoreProdContext();
        OlcumContext db = new OlcumContext();
        FasonController newController = new FasonController();
        YalinUretimContext yalinUretimContext = new YalinUretimContext();
        //OlcumRaporlamaTestContext db2 = new OlcumRaporlamaTestContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KazanFormuPartial()
        {
            try
            {

                List<DTOKaliteAtolye> AtolyeList = newController.getAtolyes();
                List<OlcuTurleri> OlcuTurList = db.OlcuTurleri.Where(x => (x.olcuTuruAd == "İmalat Kazanı" || x.olcuTuruAd == "İmalat Parti" || x.olcuTuruAd == "Yıkama İmalat" || x.olcuTuruAd == "Pilot" || x.olcuTuruAd == "İmalat Eksiği" ||x.olcuTuruAd == "Çoğaltma Kazanı" ||x.olcuTuruAd == "Toplantı Kazanı") && x.tabloTuru == "Kazan").ToList();
                List<GiysiTurleri> GiysiTurList = db.GiysiTurleri.OrderByDescending(x => x.giysiTuruAd).ToList();


                ViewBag.atolyeler = AtolyeList;
                ViewBag.olcuTurleri = OlcuTurList;
                ViewBag.giysiTurleri = GiysiTurList;

                KazanDetay kazanDetayObj = new KazanDetay();

                return PartialView("~/Views/KazanFormu/KazanFormPartial.cshtml", kazanDetayObj);
            }
            catch (Exception hata)
            {
                TempData["hata"] = "Hata " + hata.Message.ToString() + ". İçerik: " + hata.InnerException.Message.ToString();
                return PartialView();
            }
        }


        [HttpPost]
        public JsonResult deleteOlcu(int kazanDetayId)
        {

            KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

            dbKazanDetay.durum = null;

            db.SaveChanges();

            return Json("OK", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult KazanFormKaydet(KazanDetay postParam, int kullanıcıID)
        {
            try
            {
                //pilot ve imalat, eksiği aynı (ölçü tablosun), çoğlatma kazanında çoğaltmaya(çoğlatma) bak, geri kalanda imalatta bak(ölçü tablosun)
                OlcuTurleri secilenOlcuTur = db.OlcuTurleri.Where(x => x.id == postParam.olcuTuruID).FirstOrDefault();
                int arananOlcuTurId = 0;
                int arananTabloTuru = 0;

                if (secilenOlcuTur.olcuTuruAd == "Pilot" || secilenOlcuTur.olcuTuruAd == "İmalat Eksiği")
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == secilenOlcuTur.olcuTuruAd && x.tabloTuru == "Ölçü Tablosu").FirstOrDefault().id;
                }
                else if (secilenOlcuTur.olcuTuruAd == "Çoğaltma Kazanı")
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == "Çoğaltma" && x.tabloTuru == "Numune").FirstOrDefault().id;
                }
                else if (secilenOlcuTur.olcuTuruAd == "Toplantı Kazanı")
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == "Toplantı" && x.tabloTuru == "Numune").FirstOrDefault().id;
                }
                else
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == "İmalat" && x.tabloTuru == "Ölçü Tablosu").FirstOrDefault().id;
                }

                if (postParam.tabloTuru == 2 || postParam.tabloTuru == 3)
                {
                    arananTabloTuru = 1;
                }
                else
                {
                    arananTabloTuru = postParam.tabloTuru;
                }

                if(arananOlcuTurId == 23 || arananOlcuTurId == 24)
                {
                    if(db.NumuneDetay.Any(x=> x.orderID == postParam.orderID && x.olcuTuruID == arananOlcuTurId))
                    {
                        KazanDetay addObj = new KazanDetay();

                        addObj.orderID = postParam.orderID;
                        addObj.atolyeID = postParam.atolyeID;
                        addObj.giysiTuruID = postParam.giysiTuruID;

                        addObj.kullaniciID = kullanıcıID;
                        addObj.olcuTuruID = postParam.olcuTuruID;
                        addObj.yikamaYeri = postParam.yikamaYeri ?? "";
                        addObj.yikamaSorumlu = postParam.yikamaSorumlu ?? "";
                        addObj.tabloTuru = postParam.tabloTuru;
                        addObj.aktarimMi = false;

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
                        addObj.olcuTabloOlcuTurId = arananOlcuTurId;


                        db.KazanDetay.Add(addObj);
                        db.SaveChanges();

                        int olcuTaloDetayId = db.NumuneDetay.Where(x => x.orderID == postParam.orderID && x.olcuTuruID == arananOlcuTurId).FirstOrDefault().id;

                        int orderId = addObj.orderID;

                        string orderNo = db.Order.Where(x => x.ID == orderId).Select(x => x.orderNo).FirstOrDefault();


                        int orderIdYalinUretim = yalinUretimContext.IsEmriAlt.Where(x => x.IsEmriAltNo == orderNo).Select(x=>x.IsEmriAltID).FirstOrDefault();

                        OrderTakipKimlikBilgiler orderTakipKimlik = yalinUretimContext.OrderTakipKimlikBilgiler.Where(x => x.OrderID == orderIdYalinUretim).FirstOrDefault();

                        if(orderTakipKimlik != null)
                        {
                            if(orderTakipKimlik.OlcumVarMi != 1)
                            {
                                orderTakipKimlik.OlcumVarMi = 1;

                                yalinUretimContext.SaveChanges();
                            }
                        }

                        return Json(new { kazanDetayId = addObj.id, olcuTaloDetayId = olcuTaloDetayId, atabloturu = arananTabloTuru, devamMi = false ,numuneMi = 1}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (db.OlcuTabloDetay.Any(x => x.orderID == postParam.orderID && x.olcuTuruID == arananOlcuTurId && x.tabloTuru == arananTabloTuru))
                    {
                        KazanDetay addObj = new KazanDetay();

                        addObj.orderID = postParam.orderID;
                        addObj.atolyeID = postParam.atolyeID;
                        addObj.giysiTuruID = postParam.giysiTuruID;

                        addObj.kullaniciID = kullanıcıID;
                        addObj.olcuTuruID = postParam.olcuTuruID;
                        addObj.yikamaYeri = postParam.yikamaYeri ?? "";
                        addObj.yikamaSorumlu = postParam.yikamaSorumlu ?? "";
                        addObj.tabloTuru = postParam.tabloTuru;
                        addObj.aktarimMi = false;


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
                        addObj.olcuTabloOlcuTurId = arananOlcuTurId;


                        db.KazanDetay.Add(addObj);
                        db.SaveChanges();

                        int orderId = addObj.orderID;

                        string orderNo = db.Order.Where(x => x.ID == orderId).Select(x => x.orderNo).FirstOrDefault();

                        int olcuTaloDetayId = db.OlcuTabloDetay.Where(x => x.orderID == postParam.orderID && x.olcuTuruID == arananOlcuTurId && x.tabloTuru == arananTabloTuru).FirstOrDefault().id;

                        int orderIdYalinUretim = yalinUretimContext.IsEmriAlt.Where(x => x.IsEmriAltNo == orderNo).Select(x => x.IsEmriAltID).FirstOrDefault();

                        OrderTakipKimlikBilgiler orderTakipKimlik = yalinUretimContext.OrderTakipKimlikBilgiler.Where(x => x.OrderID == orderIdYalinUretim).FirstOrDefault();

                        if (orderTakipKimlik != null)
                        {
                            if (orderTakipKimlik.OlcumVarMi != 1)
                            {
                                orderTakipKimlik.OlcumVarMi = 1;

                                yalinUretimContext.SaveChanges();
                            }
                        }


                        return Json(new { kazanDetayId = addObj.id, olcuTaloDetayId = olcuTaloDetayId, atabloturu = arananTabloTuru, devamMi = false ,numuneMi = 0}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }

               

            }
            catch (Exception hata)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult devamEtKazanFormKaydet(int kazanDetayId)
        {
            try
            {
                KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

                OlcuTurleri secilenOlcuTur = dbKazanDetay.OlcuTurleri;

                int arananOlcuTurId = 0;
                int arananTabloTuru = 0;

                if (secilenOlcuTur.olcuTuruAd == "Pilot" || secilenOlcuTur.olcuTuruAd == "İmalat Eksiği")
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == secilenOlcuTur.olcuTuruAd && x.tabloTuru == "Ölçü Tablosu").FirstOrDefault().id;
                }
                else if (secilenOlcuTur.olcuTuruAd == "Çoğaltma Kazanı")
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == "Çoğaltma" && x.tabloTuru == "Numune").FirstOrDefault().id;
                }
                else if (secilenOlcuTur.olcuTuruAd == "Toplantı Kazanı")
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == "Toplantı" && x.tabloTuru == "Numune").FirstOrDefault().id;
                }
                else
                {
                    arananOlcuTurId = db.OlcuTurleri.Where(x => x.olcuTuruAd == "İmalat" && x.tabloTuru == "Ölçü Tablosu").FirstOrDefault().id;
                }

                arananTabloTuru = dbKazanDetay.tabloTuru;

                int olcuTaloDetayId = 0;

                if (arananOlcuTurId == 23 || arananOlcuTurId == 24)
                {
                    olcuTaloDetayId = db.NumuneDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == arananOlcuTurId && x.tabloTuru == -1).FirstOrDefault().id;

                }
                else
                {
                    olcuTaloDetayId = db.OlcuTabloDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == arananOlcuTurId && x.tabloTuru == arananTabloTuru).FirstOrDefault().id;
                }


                return Json(new { kazanDetayId = dbKazanDetay.id, olcuTaloDetayId = olcuTaloDetayId, atabloturu = arananTabloTuru, devamMi = true,numuneMi = (arananOlcuTurId == 23 || arananOlcuTurId == 24) ? 1 :0 }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception hata)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult KazanFormuTablePartial(int kazanDetayId, int olcuTaloDetayId, int atabloturu, Boolean devamMi = false,int numuneMi = 0)
        {
            try
            {
                KazanDetay dbkazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();
                ViewBag.numuneMi = numuneMi;
                if(numuneMi == 0)
                {
                    OlcuTabloDetay dbOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();
                    List<int> dbOlcuTabloAraList = db.OlcuTabloAra.Where(x => x.olcuTabloID == dbOlcuTabloDetay.id).Select(x => x.olcuNoktaID).Distinct().ToList();

                    List<OlcuNoktalari> dbOlcuNoktalariList = db.OlcuNoktalari.Where(x => dbOlcuTabloAraList.Contains(x.id) && !x.olcuNoktasi.Contains("YERİ")).ToList();

                    List<string> cekmeList = db.OlcuTabloDetay.Where(x => x.enBoyCekme != "" && x.orderID == dbOlcuTabloDetay.orderID && x.olcuTuruID == dbOlcuTabloDetay.olcuTuruID && x.tabloTuru == dbOlcuTabloDetay.tabloTuru).Select(y => y.enBoyCekme).ToList();
                    ViewBag.cekmeList = cekmeList;

                    List<int> BedenIdList = db.OlcuTabloAra.Where(x => x.olcuTabloID == dbOlcuTabloDetay.id).Select(x => x.bedenID).Distinct().ToList();
                    List<Bedenler> bedenList = db.Bedenler.Where(x => BedenIdList.Contains(x.ID)).ToList().Select(y => new Bedenler { beden = y.beden, bedenSistemi = y.bedenSistemi, ID = y.ID }).ToList();


                    ViewBag.bedenList = bedenList;

                    ViewBag.MusteriModel = new { musteri = dbOlcuTabloDetay.Order.musteri, model = dbOlcuTabloDetay.Order.modelAd, artikel = dbOlcuTabloDetay.Order.modelNo };

                    ViewBag.olcuTaloDetayId = olcuTaloDetayId;

                    ViewBag.atabloturu = atabloturu;

                    ViewBag.devamMi = devamMi;

                    ViewBag.kDerece = dbkazanDetay.kDerece;
                    ViewBag.kDakika = dbkazanDetay.kDakika;
                    ViewBag.aciklama = dbkazanDetay.aciklama;
                    return PartialView("~/Views/KazanFormu/KazanFormuTableP.cshtml", Tuple.Create(kazanDetayId, dbOlcuNoktalariList));

                }
                else
                {
                    NumuneDetay dbNumuneDetay = db.NumuneDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                    List<int> olcuNoktaIdList = db.NumuneAra.Where(x => x.numuneDetayID == dbNumuneDetay.id).Select(x => x.olcuNoktaID).Distinct().ToList();

                    List<OlcuNoktalari> dbOlcuNoktalariList = db.OlcuNoktalari.Where(x => olcuNoktaIdList.Contains(x.id) && !x.olcuNoktasi.Contains("YERİ")).ToList();

                    List<string> cekmeList = new List<string> { dbNumuneDetay.kalipAdi };
                    ViewBag.cekmeList = cekmeList;

                    List<Bedenler> bedenList = db.NumuneAra.Where(x => x.numuneDetayID == dbNumuneDetay.id).Select(x => x.Bedenler).Distinct().ToList();
                    ViewBag.bedenList = bedenList;

                    ViewBag.MusteriModel = new { musteri = dbNumuneDetay.Order.musteri, model = dbNumuneDetay.Order.modelAd, artikel = dbNumuneDetay.Order.modelNo };

                    ViewBag.olcuTaloDetayId = olcuTaloDetayId;

                    ViewBag.atabloturu = atabloturu;

                    ViewBag.devamMi = devamMi;

                    ViewBag.kDerece = dbkazanDetay.kDerece;
                    ViewBag.kDakika = dbkazanDetay.kDakika;
                    ViewBag.aciklama = dbkazanDetay.aciklama;
                    return PartialView("~/Views/KazanFormu/KazanFormuTableP.cshtml", Tuple.Create(kazanDetayId, dbOlcuNoktalariList));

                }




            }
            catch (Exception hata)
            {
                TempData["hata"] = "Hata " + hata.Message.ToString() + ". İçerik: " + hata.InnerException.Message.ToString();
                return PartialView();
            }
        }

        public ActionResult getIstenilenOlcuTablosu(string cekme, int bedenId, List<OlcuNokIdwithSiraNoDto> secilenNoktalar, List<DtoOlcuNokDeger> returnYeriList,
            int kazanDetayId, int silinenPno, List<pListDto> pantolonList, List<kazanGirilenListDto> degerler, Boolean silindiMi,
            List<DtoOlcuNokDeger> olmasiGerekenList, int olcuTaloDetayId, int tablotur, string barcodeVal, string topNo, string enBoyCekme, int numuneMi = 0)
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

                KazanBarkodDto kazanBarkodDto = new KazanBarkodDto();
                KazanBarkod kazanBarkod = null;

                if (!String.IsNullOrEmpty(barcodeVal))
                {
                    var barkod = recoreProdContext.BRC_Barcodes.Where(x => x.Barcode == barcodeVal).FirstOrDefault();

                    if(barkod != null)
                    {
                        kazanBarkod = new KazanBarkod()
                        {
                            BarcodeNo = barcodeVal,
                            BedenId = bedenId,
                            BedenPantNo = lastBpantno,
                            EnBoyCekme = barkod.Shrink,
                            TopNo = barkod.FabricRollNumber,
                            KazanDetayId = kazanDetayId,
                            PantNo = lastPno
                        };

                        db.KazanBarkod.Add(kazanBarkod);
                    }
                }
                else if(!String.IsNullOrEmpty(topNo) || !String.IsNullOrEmpty(enBoyCekme))
                {
                    string workOrder = db.KazanDetay.Where(x => x.id == kazanDetayId).Select(x => x.Order.orderNo).FirstOrDefault();
                    int workOrderId = recoreProdContext.RC_WorkOrders.Where(x => x.WorkOrderNo == workOrder).Select(x => x.Id).FirstOrDefault();

                    kazanBarkod = new KazanBarkod()
                    {
                        BarcodeNo = barcodeVal,
                        BedenId = bedenId,
                        BedenPantNo = lastBpantno,
                        EnBoyCekme = enBoyCekme,
                        TopNo = topNo,
                        KazanDetayId = kazanDetayId,
                        PantNo = lastPno
                    };

                    db.KazanBarkod.Add(kazanBarkod);
                }

     

                db.SaveChanges();

                if (kazanBarkod != null)
                {
                    kazanBarkodDto = GetKazanBarkodDto(kazanBarkod);

                    var lastPant = pantolonList.OrderByDescending(x => x.pno).FirstOrDefault();

                    lastPant.KazanBarkodDto = kazanBarkodDto;

                    lastPant.previousYikamaOncesiDetayId = GetPreviousYikamaOncesi(kazanBarkodDto);

                }
                else
                {
                    var lastPant = pantolonList.OrderByDescending(x => x.pno).FirstOrDefault();


                    string bedenAd = db.Bedenler.Where(x => x.ID == bedenId).Select(x => x.beden).FirstOrDefault();

                    kazanBarkodDto = new KazanBarkodDto()
                    {
                        BarcodeNo = barcodeVal,
                        BedenId = bedenId,
                        BedenPantNo = lastBpantno,
                        EnBoyCekme = enBoyCekme,
                        TopNo = topNo,
                        KazanDetayId = kazanDetayId,
                        PantNo = lastPno,
                        BedenAd = bedenAd                        
                    };

                    lastPant.KazanBarkodDto = kazanBarkodDto;

                    lastPant.previousYikamaOncesiDetayId = null;


                }

                if (numuneMi == 0)
                {
                    if (tablotur == 0)
                    {
                        if (!olmasiGerekenList.Any(x => x.bedenId == bedenId && x.cekme == cekme))
                        {
                            OlcuTabloDetay firstOlcuTablo = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                            OlcuTabloDetay objDetay = db.OlcuTabloDetay.Where(x => x.orderID == firstOlcuTablo.orderID && x.olcuTuruID == firstOlcuTablo.olcuTuruID && x.enBoyCekme == cekme && x.tabloTuru == firstOlcuTablo.tabloTuru).FirstOrDefault();

                            int olcuTabloId = objDetay.id;

                            List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

                            foreach (var item in olcuTabloAras.Where(x => x.bedenID == bedenId).ToList())
                            {
                                DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();

                                addObj.olcuNokId = item.OlcuNoktalari.id;
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
                            OlcuTabloDetay objDetay = db.OlcuTabloDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                            int olcuTabloId = objDetay.id;

                            List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId).ToList();

                            foreach (var item in olcuTabloAras.Where(x => x.bedenID == bedenId))
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
                    if(tablotur == 0)
                    {
                        if (!olmasiGerekenList.Any(x => x.bedenId == bedenId && x.cekme == cekme))
                        {
                            NumuneDetay firstOlcuTablo = db.NumuneDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                            NumuneDetay objDetay = db.NumuneDetay.Where(x => x.orderID == firstOlcuTablo.orderID && x.olcuTuruID == firstOlcuTablo.olcuTuruID && x.kalipAdi == cekme && x.tabloTuru == -1).FirstOrDefault();

                            foreach (var item in objDetay.NumuneAra.Where(x => x.bedenID == bedenId).ToList())
                            {
                                DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();

                                addObj.olcuNokId = item.OlcuNoktalari.id;
                                addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;

                                addObj.cekme = cekme;
                                addObj.bedenAd = secilenBeden.beden;
                                addObj.bedenId = bedenId;

                                addObj.deger = item.yikamaOncesiTablo.ToString();

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
                            NumuneDetay objDetay = db.NumuneDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                            foreach (var item in objDetay.NumuneAra.Where(x => x.bedenID == bedenId))
                            {
                                DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();
                                addObj.olcuNokId = item.OlcuNoktalari.id;
                                addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;
                                addObj.deger = item.yikamaSonrasiTablo.ToString();
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
                    if (tablotur == 0)
                    {
                        returnList = olmasiGerekenList.Where(x => x.bedenAd == sonDeger.bedenad && x.cekme == sonDeger.cekme).ToList();

                    }
                    else
                    {
                        returnList = olmasiGerekenList.Where(x => x.bedenAd == sonDeger.bedenad).ToList();

                    }
                }

                int kazanBarkodId = silinenP.KazanBarkodDto.Id;

                if(db.KazanBarkod.Where(x => x.Id == kazanBarkodId).FirstOrDefault() != null)
                {
                    db.KazanBarkod.Remove(db.KazanBarkod.Where(x => x.Id == kazanBarkodId).FirstOrDefault());
                }

                db.SaveChanges();

            }

            ViewBag.numuneMi = numuneMi;
            ViewBag.silindiMi = silindiMi;
            ViewBag.olcuYeriList = returnYeriList;
            #endregion


            return PartialView("~/Views/KazanFormu/getIstenilenOlcuTablosuP.cshtml", Tuple.Create(returnList.OrderBy(x => x.sirano).ToList(), pantolonList.OrderByDescending(x => x.pno).ToList(), degerler, secilenNoktalar));
        }

        private int? GetPreviousYikamaOncesi(KazanBarkodDto kazanBarkodDto)
        {
            int? kazanDetayId = null;

            int currKazanDetayId = kazanBarkodDto.KazanDetayId;

            KazanDetay kazanDetay = db.KazanDetay.Where(x => x.id == currKazanDetayId).FirstOrDefault();

            if(kazanDetay != null)
            {
                int orderId = kazanDetay.orderID;
                int olcuTuruId = kazanDetay.olcuTuruID;

                KazanDetay yikamaOncesiKazan = null;

                if(olcuTuruId == 17 || olcuTuruId == 18)
                {
                    yikamaOncesiKazan = db.KazanDetay.Where(x => x.orderID == orderId && x.olcuTuruID == 17 && x.tabloTuru == 0 && x.durum != null).OrderByDescending(x => x.olcumNo).FirstOrDefault();
                }
                else if(olcuTuruId == 16)
                {
                    yikamaOncesiKazan = db.KazanDetay.Where(x => x.orderID == orderId && x.olcuTuruID == olcuTuruId && x.tabloTuru == 0 && x.durum != null).OrderByDescending(x => x.olcumNo).FirstOrDefault();
                }


                if(yikamaOncesiKazan != null)
                {
                    kazanDetayId = yikamaOncesiKazan.id;
                }
            }

            return kazanDetayId;
        }

        public KazanBarkodDto GetKazanBarkodDto(KazanBarkod kazanBarkod)
        {
            string bedenAd = db.Bedenler.Where(x => x.ID == kazanBarkod.BedenId).Select(x => x.beden).FirstOrDefault();

            var kazanBarkodDto = new KazanBarkodDto()
            {
                BarcodeNo = kazanBarkod.BarcodeNo,
                BedenId = (int)kazanBarkod.BedenId,
                BedenPantNo =kazanBarkod.BedenPantNo,
                EnBoyCekme = kazanBarkod.EnBoyCekme,
                KazanDetayId = kazanBarkod.KazanDetayId,
                PantNo = kazanBarkod.PantNo,
                TopNo = kazanBarkod.TopNo,
                BedenAd = bedenAd,
                Id = kazanBarkod.Id
            };

            return kazanBarkodDto;
        }

        public ActionResult setIstenilenOlcuTablosu(int kazanDetayId, int olcuTaloDetayId,int numuneMi = 0)
        {
            KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

            List<KazanBarkod> kazanBarkodList =db.KazanBarkod.Where(x=>x.KazanDetayId ==dbKazanDetay.id).ToList();
            List<KazanHesaplama> kazanHesaplamas = db.KazanHesaplama.Where(x => x.kazanDetayID == dbKazanDetay.id).ToList();

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
                KazanBarkod kazanBarkod = kazanBarkodList.Where(x => x.PantNo == item).FirstOrDefault();

                var kazanBarkodDto = new KazanBarkodDto();

                pantolonList.Add(new pListDto { bedenad = KazanDetayItem.Bedenler.beden, bedenid = KazanDetayItem.bedenID, bpantno = (int)KazanDetayItem.bpantNo, cekme = KazanDetayItem.enBoyCekme == "" ? null : KazanDetayItem.enBoyCekme, pno = (int)KazanDetayItem.pantNo ,KazanBarkodDto = kazanBarkodDto });
            }

            foreach (var item in kazanAraList)
            {
                degerler.Add(new kazanGirilenListDto { deger = item.deger, olcunokid = item.olcuNoktaID, bedenad = item.Bedenler.beden, bedenid = item.bedenID, bpantno = (int)item.bpantNo, cekme = item.enBoyCekme == "" ? null : item.enBoyCekme, pno = (int)item.pantNo });
            }

            if(numuneMi == 0)
            {
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

                                if (kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().Any(x => x == item.olcuNoktaID))
                                {
                                    addObj.sirano = kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().FindIndex(x => x == item.OlcuNoktalari.id) + 1;

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

                            foreach (var item in olcuTabloAras.Where(x => x.bedenID == pantolonItem.bedenid).ToList())
                            {
                                DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();
                                addObj.olcuNokId = item.OlcuNoktalari.id;
                                addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;
                                addObj.deger = item.deger.ToString();
                                addObj.cekme = null;
                                addObj.bedenAd = pantolonItem.bedenad;
                                addObj.bedenId = pantolonItem.bedenid;

                                if (kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().Any(x => x == item.olcuNoktaID))
                                {
                                    addObj.sirano = kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().FindIndex(x => x == item.OlcuNoktalari.id) + 1;

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

            }
            else
            {
                if (dbKazanDetay.tabloTuru == 0)
                {
                    NumuneDetay firstOlcuTablo = db.NumuneDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                    foreach (var pantolonItem in pantolonList)
                    {
                        if (!olmasiGerekenListAll.Any(x => x.bedenId == pantolonItem.bedenid && x.cekme == pantolonItem.cekme))
                        {
                            NumuneDetay objDetay = db.NumuneDetay.Where(x => x.orderID == firstOlcuTablo.orderID && x.olcuTuruID == firstOlcuTablo.olcuTuruID && x.kalipAdi == pantolonItem.cekme && x.tabloTuru == -1).FirstOrDefault();

                            foreach (var item in objDetay.NumuneAra.Where(x => x.bedenID == pantolonItem.bedenid).ToList())
                            {
                                DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();

                                addObj.olcuNokId = item.OlcuNoktalari.id;
                                addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;

                                addObj.cekme = pantolonItem.cekme;
                                addObj.bedenAd = pantolonItem.bedenad;
                                addObj.bedenId = pantolonItem.bedenid;

                                addObj.deger = item.yikamaOncesiTablo.ToString();

                                if (kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().Any(x => x == item.olcuNoktaID))
                                {
                                    addObj.sirano = kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().FindIndex(x => x == item.OlcuNoktalari.id) + 1;

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
                    NumuneDetay objDetay = db.NumuneDetay.Where(x => x.id == olcuTaloDetayId).FirstOrDefault();

                    foreach (var pantolonItem in pantolonList)
                    {
                        if (!olmasiGerekenListAll.Any(x => x.bedenId == pantolonItem.bedenid))
                        {
                            foreach (var item in objDetay.NumuneAra.Where(x => x.bedenID == pantolonItem.bedenid).ToList())
                            {
                                DtoOlcuNokDeger addObj = new DtoOlcuNokDeger();
                                addObj.olcuNokId = item.OlcuNoktalari.id;
                                addObj.olcuNokAd = item.OlcuNoktalari.olcuNoktasi;
                                addObj.deger = item.yikamaSonrasiTablo.ToString();
                                addObj.cekme = null;
                                addObj.bedenAd = pantolonItem.bedenad;
                                addObj.bedenId = pantolonItem.bedenid;

                                if (kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().Any(x => x == item.olcuNoktaID))
                                {
                                    addObj.sirano = kazanHesaplamas.Select(s => s.olcuNoktasiID).ToList().FindIndex(x => x == item.OlcuNoktalari.id) + 1;

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

            }



            if (kazanAraList.Count() > 0)
            {
                Bedenler lastBeden = kazanAraList.LastOrDefault().Bedenler;
                string lastCekme = kazanAraList.LastOrDefault().enBoyCekme;

                string serachCekme = lastCekme == "" ? null : lastCekme;

                returnList = olmasiGerekenListAll.Where(x => kazanHesaplamaList.Select(y => y.olcuNoktasiID).Contains(x.olcuNokId) && x.bedenId == lastBeden.ID && x.cekme == serachCekme).ToList();
                returnYeriList = olmasiGerekenListAll.Where(x => x.olcuNokAd.Contains("YERİ")).ToList();
            }


            ViewBag.silindiMi = true;
            ViewBag.olcuYeriList = returnYeriList;

            ViewBag.setReturnList = olmasiGerekenListAll.Where(x => kazanHesaplamaList.Select(y=>y.olcuNoktasiID).Contains(x.olcuNokId) && !x.olcuNokAd.Contains("YERİ")).ToList();


            if (seciliOlcuNokIdList.Count() == 0)
            {
                int sayacSiraNo = 1;

                foreach (var item in kazanHesaplamaList.ToList())
                {
                    seciliOlcuNokIdList.Add(new OlcuNokIdwithSiraNoDto { olcunokid = item.olcuNoktasiID, sirano = sayacSiraNo });
                    sayacSiraNo++;
                }
            }

            return PartialView("~/Views/KazanFormu/getIstenilenOlcuTablosuP.cshtml", Tuple.Create(returnList.OrderBy(x => x.sirano).ToList(), pantolonList.OrderByDescending(x => x.pno).ToList(), degerler, seciliOlcuNokIdList));

        }

        public ActionResult GetBarcodeToolTip(int kazanDetayId,KazanBarkodDto kazanBarkodDto,List<OlcuNokIdwithSiraNoDto> olcuNokIdwithSiraNoDtos)
        {
            string barkodString = kazanBarkodDto.BarcodeNo;
            string topNo = kazanBarkodDto.TopNo;
            string cekme = kazanBarkodDto.EnBoyCekme;

            int? barkodYikamaOncesiPantolonNo = null;

            List<KazanAra> kazanAras = null;

            if (string.IsNullOrEmpty(barkodString))
            {
                barkodYikamaOncesiPantolonNo = db.KazanBarkod.Where(x => x.KazanDetayId == kazanDetayId && x.TopNo == topNo && x.EnBoyCekme == cekme).Select(x => x.PantNo).FirstOrDefault();
            }
            else
            {
                barkodYikamaOncesiPantolonNo = db.KazanBarkod.Where(x => x.KazanDetayId == kazanDetayId && x.BarcodeNo == barkodString).Select(x => x.PantNo).FirstOrDefault();
            }

            if(barkodYikamaOncesiPantolonNo != null)
            {
                kazanAras = db.KazanAra.Where(x => x.kazanDetayID == kazanDetayId && x.pantNo == barkodYikamaOncesiPantolonNo).ToList();

                if(kazanAras != null)
                {
                    olcuNokIdwithSiraNoDtos = olcuNokIdwithSiraNoDtos.OrderBy(x => x.sirano).ToList();
                    kazanAras = kazanAras.OrderBy(x => olcuNokIdwithSiraNoDtos.FindIndex(y => y.olcunokid == x.olcuNoktaID)).ToList();
                }
            }



            return PartialView(kazanAras);
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

        public JsonResult kaydet(string aciklama, int kazanDetayId, bool durum, int kDerece, int kDakika,int numuneMi = 0)
        {
            try
            {
                KazanDetay dbKazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();
                dbKazanDetay.aciklama = aciklama;
                dbKazanDetay.durum = durum;
                dbKazanDetay.kDerece = kDerece;
                dbKazanDetay.kDakika = kDakika;

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

                    //
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

                        if(numuneMi == 0)
                        {
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

                        }
                        else
                        {
                            int olcuTuruId = dbKazanDetay.olcuTuruID;
                            int searchedTypeId = 0;

                            if(olcuTuruId == 51)
                            {
                                searchedTypeId = 24;
                            }
                            else if(olcuTuruId == 22)
                            {
                                searchedTypeId = 23;
                            }

                            NumuneDetay selectedNumuneDetay = db.NumuneDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == searchedTypeId && x.tabloTuru == -1).FirstOrDefault();

                            if(selectedNumuneDetay != null)
                            {
                                int maxBedenId = kazanAras.Max(x => x.bedenID);

                                NumuneAra numuneAra = db.NumuneAra.Where(x => x.numuneDetayID == selectedNumuneDetay.id && x.olcuNoktaID == item && x.bedenID == maxBedenId).FirstOrDefault();

                                oncekiTablo = numuneAra.kalipOlcusu == null ? 0 : (double)numuneAra.kalipOlcusu;
                                newKazanHesaplama.oncekiTablo = oncekiTablo;

                                uygulananTolCekme = numuneAra.uygulananTolerans == null ? 0 : (double)numuneAra.uygulananTolerans;
                                newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;

                                asilTablo = numuneAra.yikamaOncesiTablo == null ? 0 : (double)numuneAra.yikamaOncesiTablo;
                                newKazanHesaplama.asilTablo = asilTablo;

                            }
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

                        if(numuneMi == 0)
                        {
                            if (Convert.ToInt32(dbKazanDetay.olcuTuruID) == 18 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 19)
                            {
                                olcuturuID = 17;

                                yoKazanDetayID = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == olcuturuID && x.tabloTuru == 0 && (x.durum != null && x.durum != false)).Select(x => x.id).FirstOrDefault();

                                if (yoKazanDetayID == 0)
                                {
                                    yoKazanDetayID = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == 16 && x.tabloTuru == 0 && db.KazanAra.Where(y => y.kazanDetayID == x.id).FirstOrDefault() != null && (x.durum != null && x.durum != false)).Select(x => x.id).FirstOrDefault();
                                }
                            }
                            else
                            {
                                olcuturuID = dbKazanDetay.olcuTuruID;

                                yoKazanDetayID = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == olcuturuID && x.tabloTuru == 0 && db.KazanAra.Where(y => y.kazanDetayID == x.id).FirstOrDefault() != null && (x.durum != null && x.durum != false)).Select(x => x.id).FirstOrDefault();
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
      
                        }
                        else
                        {
                            int olcuTuruId = dbKazanDetay.olcuTuruID;
                            int searchedTypeId = 0;

                            if (olcuTuruId == 51)
                            {
                                searchedTypeId = 24;
                            }
                            else if (olcuTuruId == 22)
                            {
                                searchedTypeId = 23;
                            }

                            int maxBedenId = kazanAras.Max(x => x.bedenID);


                            NumuneDetay selectedNumuneDetay = db.NumuneDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == searchedTypeId && x.tabloTuru == -1).FirstOrDefault();

                            KazanDetay yoNumuneDetay = db.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == dbKazanDetay.olcuTuruID && x.tabloTuru == 0 && (x.durum != null && x.durum != false) && db.KazanAra.Where(y => y.kazanDetayID == x.id).FirstOrDefault() != null).FirstOrDefault();

                            if(yoNumuneDetay != null)
                            {
                                KazanHesaplama yoKazanHesaplama = db.KazanHesaplama.Where(x => x.olcuNoktasiID == item && x.kazanDetayID == yoNumuneDetay.id).FirstOrDefault();
                                yoOlculen = yoKazanHesaplama.yoOlculen == null ? 0 : (double)yoKazanHesaplama.yoOlculen;
                                newKazanHesaplama.yoOlculen = yoOlculen;
                            }

                            var selectedNumuneAra = selectedNumuneDetay.NumuneAra.Where(x => x.olcuNoktaID == item && x.bedenID == maxBedenId).FirstOrDefault();
                            uygulananTolCekme = selectedNumuneAra.uygulananCekme == null ? 0 : (double)selectedNumuneAra.uygulananCekme;
                            newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;

                            asilTablo = selectedNumuneAra.yikamaSonrasiTablo == null ? 0 : (double)selectedNumuneAra.yikamaSonrasiTablo;
                            newKazanHesaplama.asilTablo = asilTablo;

                        }

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
                        newController.sendMail(dbKazanDetay, "", "");
                    }
                }
                catch (Exception e)
                {
                    throw;
                }


                return Json("Kayıt Başarılı", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("Kayıt sırasında bir hata oluştu", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getOlcuYerleri(string cekme, int bedenId, List<DtoOlcuNokDeger> olcuYerleriList)
        {
            if (cekme == "") { cekme = null; }
            List<DtoOlcuNokDeger> returnList = olcuYerleriList.Where(x => x.cekme == cekme & x.bedenId == bedenId).ToList();

            return PartialView("~/Views/KazanFormu/olcuYerleriTableP.cshtml", Tuple.Create(returnList));

        }

        public JsonResult GerceklesenUygulananCekmeTolAlma(int kazanDetayId)
        {

            //List<double> gerceklesenTolCekme = db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId).Select(x => x.gerceklesenTolCekme).ToList();

            //List<double> uygulananTolCekme = db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId).Select(x => x.uygulananTolCekme).ToList();

            //Tuple<List<double>, List<double>> gerceklesenUygulananList = Tuple.Create(gerceklesenTolCekme, uygulananTolCekme);

            //return Json(gerceklesenUygulananList, JsonRequestBehavior.AllowGet);

            List<KazanHesaplama> gerceklesenVeUygulananTolCekme = db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId).ToList();
            List<KazanHesaplama> a = gerceklesenVeUygulananTolCekme.Select(x => new KazanHesaplama
            {
                gerceklesenTolCekme = x.gerceklesenTolCekme,
                olcuNoktasiID = x.olcuNoktasiID,
                uygulananTolCekme = x.uygulananTolCekme,
                ortalamaDeger = x.ortalamaDeger
            }).ToList();

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public List<KazanHesaplama> GerceklesenUygulananCekmeTolAlmaForController(int kazanDetayId)
        {

            List<KazanHesaplama> gerceklesenVeUygulananTolCekme = db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId).ToList();

            return gerceklesenVeUygulananTolCekme.Select(x => new KazanHesaplama
            {
                gerceklesenTolCekme = x.gerceklesenTolCekme,
                olcuNoktasiID = x.olcuNoktasiID,
                uygulananTolCekme = x.uygulananTolCekme,
                ortalamaDeger = x.ortalamaDeger
            }).ToList();


        }

        public PartialViewResult kazanPrint(int kazanDetayId)
        {
            KazanDetay DbKazan = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

            ViewBag.tarih = DbKazan.tarih.ToString("dd-MM-yyyy");
            ViewBag.olcuTuruAd = DbKazan.OlcuTurleri.olcuTuruAd;
            ViewBag.tabloTuruAd = DbKazan.tabloTuru == 0 ? "Yıkama Öncesi" : "Yıkama Sonrası";
            ViewBag.model = DbKazan.Order.modelAd;
            ViewBag.musteri = DbKazan.Order.musteri;
            ViewBag.artikel = DbKazan.Order.modelNo;
            ViewBag.orderNo = DbKazan.Order.orderNo;

            if(DbKazan.atolyeID != 0)
            {
                ViewBag.atolyeAd = newController.getAtolyes().Where(x => x.Id == DbKazan.atolyeID).FirstOrDefault().AtolyeAd;

            }
            else
            {
                ViewBag.atolyeAd = "";

            }


            if (DbKazan.utuPaketID != 0)
            {
                ViewBag.utuPaketAd = newController.getUtuPakets().Where(x => x.Id == DbKazan.utuPaketID).FirstOrDefault().UtuPaketAd;
            }
            else
            {
                ViewBag.utuPaketAd = "";
            }


            ViewBag.yikamaYer = DbKazan.yikamaYeri;
            ViewBag.yikamaSor = DbKazan.yikamaSorumlu;

            ViewBag.aciklama = DbKazan.aciklama;

            List<KazanAra> dbKazanAraList = db.KazanAra.Where(x => x.kazanDetayID == DbKazan.id).ToList(); 

            List<KazanBarkod> dbKazanBarkodList = db.KazanBarkod.Where(x => x.KazanDetayId == DbKazan.id).ToList();

            List<KazanAra> headerList = new List<KazanAra>();

            List<int?> pnoList = dbKazanAraList.Select(x => x.pantNo).Distinct().ToList();

            foreach (var item in pnoList)
            {
                headerList.Add(dbKazanAraList.Where(x => x.pantNo == item).FirstOrDefault());
            }
            int ilkBeden = headerList.Select(x => x.bedenID).FirstOrDefault();

            int? sayiSistemi = db.Bedenler.Where(x => x.ID == ilkBeden).Select(x => x.bedenSistemi).FirstOrDefault();

            if (sayiSistemi == 0)
            {
                headerList = headerList.OrderBy(x => x.Bedenler.ID).ThenBy(x => x.pantNo).ToList();
            }
            else if (sayiSistemi == 1 || sayiSistemi == 2)
            {
                headerList = headerList.OrderBy(x => x.Bedenler.beden).ThenBy(x => x.pantNo).ToList();
            }

            ViewBag.fasonMu = (DbKazan.OlcuTurleri.olcuTuruAd == "Fason Ham Ölçü" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Ön Final" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Son Final" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Sevk");

            return PartialView("~/Views/KazanFormu/printP.cshtml", Tuple.Create(headerList, dbKazanAraList.ToList(), GerceklesenUygulananCekmeTolAlmaForController(kazanDetayId),dbKazanBarkodList));
        }

        [ValidateInput(false)]
        public JsonResult saveHtmlPrint(int kazanDetayId, string printHtml)
        {
            try
            {
                //Decode
                //string readFromDatabase = // read from database;
                //string originalString = Server.HtmlDecode(readFromDatabase);
                //originalString is "<b>some html tags</b>"


                string encodedString = Server.HtmlEncode(printHtml);

                if (db.CiktiHtml.Any(x => x.kazanDetayID == kazanDetayId))
                {
                    CiktiHtml dbCikti = db.CiktiHtml.Where(x => x.kazanDetayID == kazanDetayId).FirstOrDefault();
                    dbCikti.stringHtml = encodedString;
                }
                else
                {
                    db.CiktiHtml.Add(new CiktiHtml { kazanDetayID = kazanDetayId, stringHtml = encodedString });
                }

                db.SaveChanges();
                return Json("Kayıt Başarılı", JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        public JsonResult getOrderRapor(int orderId)
        {
            DateTime today = DateTime.Today.AddDays(-7);
            DateTime tomorrow = DateTime.Today.AddDays(1);

            var raporList = db.KazanDetay.Where(x => x.orderID == orderId && (x.durum == false || (x.durum == true && x.tarih >= today && x.tarih <= tomorrow))).Select(y => new
            {
                id = y.id,
                olcuTuruAd = y.OlcuTurleri.olcuTuruAd,
                kullaniciId = y.kullaniciID,
                tabloTuru = y.tabloTuru,
                tarih = y.tarih.Day + "." + y.tarih.Month + "." + y.tarih.Year + " " + y.tarih.Hour + ":" + y.tarih.Minute + ":" + y.tarih.Hour,
                kullaniciAd = db.PersonelTablo.Where(z => z.id == y.kullaniciID).FirstOrDefault().personelAd + " " + db.PersonelTablo.Where(z => z.id == y.kullaniciID).FirstOrDefault().personelSoyad,
                durum = y.durum,
                olcumNo = y.olcumNo,
                aparatliMi = y.aparatliMi,
                utuluMu = y.utuluMu
            }).ToList();

            return Json(raporList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DevamEtModalBody(List<kazanDevamEtModalDto> dbKazan)
        {
            return PartialView(Tuple.Create(dbKazan));
        }

        public ActionResult changeOlcuNoktalari(int olcuTaloDetayId, int kazanDetayId, int atabloturu, List<OlcuNokIdwithSiraNoDto> secilenNoktalar, List<OlcuNokIdwithSiraNoDto> oldSecilenNoktalar)
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
            List<int> dbOlcuTabloAraList = db.OlcuTabloAra.Where(x => x.olcuTabloID == dbOlcuTabloDetay.id).Select(x => x.olcuNoktaID).Distinct().ToList();

            List<OlcuNoktalari> dbOlcuNoktalariList = db.OlcuNoktalari.Where(x => dbOlcuTabloAraList.Contains(x.id) && !x.olcuNoktasi.Contains("YERİ")).ToList();

            List<string> cekmeList = db.OlcuTabloDetay.Where(x => x.enBoyCekme != "" && x.orderID == dbOlcuTabloDetay.orderID && x.olcuTuruID == dbOlcuTabloDetay.olcuTuruID && x.tabloTuru == dbOlcuTabloDetay.tabloTuru).Select(y => y.enBoyCekme).ToList();
            ViewBag.cekmeList = cekmeList;

            List<int> BedenIdList = db.OlcuTabloAra.Where(x => x.olcuTabloID == dbOlcuTabloDetay.id).Select(x => x.bedenID).Distinct().ToList();
            List<Bedenler> bedenList = db.Bedenler.Where(x => BedenIdList.Contains(x.ID)).ToList().Select(y => new Bedenler { beden = y.beden, bedenSistemi = y.bedenSistemi, ID = y.ID }).ToList();
            ViewBag.bedenList = bedenList;

            ViewBag.MusteriModel = new { musteri = dbOlcuTabloDetay.Order.musteri, model = dbOlcuTabloDetay.Order.modelAd, artikel = dbOlcuTabloDetay.Order.modelNo };

            ViewBag.olcuTaloDetayId = olcuTaloDetayId;

            ViewBag.atabloturu = atabloturu;

            ViewBag.devamMi = true;

            ViewBag.kDerece = kazanDetay.kDerece;
            ViewBag.kDakika = kazanDetay.kDakika;
            ViewBag.aciklama = kazanDetay.aciklama;


            return PartialView("~/Views/KazanFormu/KazanFormuTableP.cshtml", Tuple.Create(kazanDetayId, dbOlcuNoktalariList));
        }

    }

    public class OrderDTO
    {
        public int orderID { get; set; }
        public string orderNo { get; set; }
    }
}