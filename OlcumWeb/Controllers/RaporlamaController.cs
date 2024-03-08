using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqKit;
using System.Linq.Expressions;
using OlcumWeb.Recipe;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.Entity.SqlServer;
using OlcumWeb.Araclar;
using System.Reflection;

namespace OlcumWeb.Controllers
{
    public class RaporlamaController : Controller
    {
        OlcumContext db = new OlcumContext();
        RecipeContext dbRecipe = new RecipeContext();
        public List<Tuple<int, int>> tabloCount = new List<Tuple<int, int>>();
        FasonController newController = new FasonController();
        KazanFormuController kazanController = new KazanFormuController();

        // GET: Raporlama
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult RaporlamaCardPartial()
        {
            return PartialView();
        }

        public JsonResult GetPrintHtml(int kazanDetayID)
        {
            string html = db.CiktiHtml.Where(x => x.kazanDetayID == kazanDetayID).Select(x => x.stringHtml).FirstOrDefault();

            string decodeHtml = Server.HtmlDecode(html);

            return Json(decodeHtml, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GelismisRaporlama()
        {
            return View();
        }

        public PartialViewResult GelismisRaporlamaIndex(int isFromNumunePage = 0, int toleransMi = 0, List<RECIPE_STEPS> recipeStepsFromNumunePage = null, KumasKarakteri kumasKarakteriFromNumunePage = null, int isAddingOrder = 0)
        {
            ViewBag.olcuTurleri = db.OlcuTurleri.Where(x => x.tabloTuru != "Ölçü Tablosu").ToList();

            ViewBag.isFromNumunePage = isFromNumunePage;
            ViewBag.isAddingOrder = isAddingOrder;
            ViewBag.toleransMi = toleransMi;

            if (recipeStepsFromNumunePage != null && recipeStepsFromNumunePage[0] != null)
            {
                ViewBag.recipeStepsFromNumunePage = recipeStepsFromNumunePage;
            }

            if (kumasKarakteriFromNumunePage != null)
            {
                ViewBag.kumasKarakteriFromNumunePage = kumasKarakteriFromNumunePage;
            }

            return PartialView();
        }

        public PartialViewResult ArsivRaporlamaIndex()
        {
            return PartialView();
        }

        public ActionResult ArsivRaporlamaTable(string raporAdi = "", string orderNo = "", string modelNo = "", string kumas = "", string musteri = "", string atolye = "", string modelAdi = "", string yikamaYeri = "", string tarih1 = "", string tarih2 = "")
        {
            List<ArsivDataModel> data = GetArsivDataModel(raporAdi, orderNo, modelNo, kumas, musteri, atolye, modelAdi, yikamaYeri, tarih1, tarih2);

            return PartialView(data);
        }

        public PartialViewResult CheckReport(int id)
        {
            ArsivDataModel data = new ArsivDataModel();

            string raporAdi = db.RaporDetay.Where(x => x.id == id).Select(x => x.raporAdi).FirstOrDefault();

            data.raporAdi = raporAdi;

            var kazanDetayIdList = db.RaporAra.Where(x => x.raporID == id).Select(x => x.kazanDetayID).ToList();

            List<ArsivDataModel.ReportDetailDTO> reportDetailList = new List<ArsivDataModel.ReportDetailDTO>();
            List<DTOKaliteAtolye> atolyes = newController.getAtolyes().ToList();


            foreach (var item in kazanDetayIdList)
            {
                int atolyeId = db.KazanDetay.Where(x => x.id == item).Select(x => x.atolyeID).FirstOrDefault();
                ArsivDataModel.ReportDetailDTO reportDetail = new ArsivDataModel.ReportDetailDTO()
                {
                    OrderNo = db.RaporAra.Where(x => x.raporID == id && x.kazanDetayID == item).Select(x => x.orderNo).FirstOrDefault(),
                    ModelAd = db.RaporAra.Where(x => x.raporID == id && x.kazanDetayID == item).Select(x => x.Order.modelAd).FirstOrDefault(),
                    ModelNo = db.RaporAra.Where(x => x.raporID == id && x.kazanDetayID == item).Select(x => x.Order.modelNo).FirstOrDefault(),
                    Kumas = db.RaporAra.Where(x => x.raporID == id && x.kazanDetayID == item).Select(x => x.Order.kumas).FirstOrDefault(),
                    AtolyeAd = atolyes.Where(x => x.Id == atolyeId).Select(x => x.AtolyeAd).FirstOrDefault(),
                    Musteri = db.RaporAra.Where(x => x.raporID == id && x.kazanDetayID == item).Select(x => x.Order.musteri).FirstOrDefault(),
                    YikamaYeri = db.KazanDetay.Where(x => x.id == item).Select(x => x.yikamaYeri).FirstOrDefault()
                };

                reportDetailList.Add(reportDetail);
            }
            data.reportDetailList = reportDetailList;

            return PartialView(data);
        }

        public ActionResult ArsivRapor(int id)
        {
            RaporDetay detay = db.RaporDetay.Where(x => x.id == id).FirstOrDefault();
            RaporlamaDataModel data = new RaporlamaDataModel();

            RaporlamaDataModel.RaporDetayDTO detayDto = new RaporlamaDataModel.RaporDetayDTO()
            {
                raporID = detay.id,
                raporAdi = detay.raporAdi,
                kullaniciID = (int)detay.kullaniciID,
                kullanıcıAdSoyad = detay.PersonelTablo.personelAd + " " + detay.PersonelTablo.personelSoyad,
                tabloTuru = (int)detay.tabloTuru,
                tarih = (DateTime)detay.tarih,
                anaCekmeVarMi = (int)(detay.anaCekmeVarMi == null ? 0 : detay.anaCekmeVarMi),
                raporNotu = detay.raporNotu == null ? "" : detay.raporNotu
            };

            data.detay = detayDto;

            List<RaporAra> raporAraList = db.RaporAra.Where(x => x.raporID == id).ToList();
            List<RaporlamaDataModel.RaporAraDTO> araDtos = new List<RaporlamaDataModel.RaporAraDTO>();

            List<List<RaporlamaDataModel.RaporHesaplamaDTO>> hesaplamaDTOs = new List<List<RaporlamaDataModel.RaporHesaplamaDTO>>();
            for (int i = 0; i < raporAraList.Count; i++)
            {
                RaporlamaDataModel.RaporAraDTO dto = new RaporlamaDataModel.RaporAraDTO()
                {
                    id = raporAraList[i].id,
                    raporID = raporAraList[i].raporID,
                    orderID = raporAraList[i].orderID,
                    olcuTuruID = raporAraList[i].olcuTuruID,
                    olcuTuruAd = raporAraList[i].olcuTuruAd,
                    enBoyCekme = raporAraList[i].enBoyCekme,
                    orderNo = raporAraList[i].orderNo,
                    kazanDetayID = (int)raporAraList[i].kazanDetayID,
                    beden = raporAraList[i].beden == null ? "" : raporAraList[i].beden,
                    raporTuru = (int)raporAraList[i].raporTuru,
                    receteKod = raporAraList[i].receteKod == null ? "" : raporAraList[i].receteKod,
                };
                araDtos.Add(dto);

                int raporAraID = raporAraList[i].id;

                List<RaporHesaplama> hesaplamas = db.RaporHesaplama.Where(x => x.raporID == id && x.raporAraID == raporAraID).ToList();
                List<RaporlamaDataModel.RaporHesaplamaDTO> hesaplamaDtoList = new List<RaporlamaDataModel.RaporHesaplamaDTO>();
                for (int j = 0; j < hesaplamas.Count; j++)
                {
                    RaporlamaDataModel.RaporHesaplamaDTO hesaplamaDto = new RaporlamaDataModel.RaporHesaplamaDTO()
                    {
                        id = hesaplamas[j].id,
                        raporID = hesaplamas[j].raporID,
                        raporAraID = hesaplamas[j].raporAraID,
                        olcuNoktaID = hesaplamas[j].olcuNoktaID,
                        olcuNoktaAd = hesaplamas[j].olcuNoktaAd,
                        gerceklesenCekmeTolerans = hesaplamas[j].gerceklesenCekmeTolerans.ToString(),
                        uygulananCekmeTolerans = hesaplamas[j].uygulananCekmeTolerans.ToString(),
                        ortalamaDeger = hesaplamas[j].ortalamaDeger.ToString()
                    };

                    hesaplamaDtoList.Add(hesaplamaDto);
                }
                hesaplamaDTOs.Add(hesaplamaDtoList);
            }

            List<RaporOrtalama> hesaplamaList = db.RaporOrtalama.Where(x => x.raporID == id).ToList();
            List<RaporlamaDataModel.RaporOrtalamaDTO> ortalamaDTOs = new List<RaporlamaDataModel.RaporOrtalamaDTO>();

            List<RaporAnaCekme> anaCekmeList = db.RaporAnaCekme.Where(x => x.RaporId == id).ToList();
            List<RaporlamaDataModel.RaporAnaCekmeDTO> anaCekmeDTOs = new List<RaporlamaDataModel.RaporAnaCekmeDTO>();

            for (int i = 0; i < hesaplamaList.Count; i++)
            {
                RaporlamaDataModel.RaporOrtalamaDTO dto = new RaporlamaDataModel.RaporOrtalamaDTO()
                {
                    id = hesaplamaList[i].id,
                    raporID = hesaplamaList[i].raporID,
                    olcuNoktaID = hesaplamaList[i].olcuNoktaID,
                    olcuNoktaAd = hesaplamaList[i].olcuNoktaAd,
                    ortGerceklesenCekmeTolerans = hesaplamaList[i].ortGerceklesenCekmeTolerans.ToString(),
                    ortUygulananCekmeTolerans = hesaplamaList[i].ortUygulananCekmeTolerans.ToString(),
                    ortOrtalamaDeger = hesaplamaList[i].ortOrtalamaDeger.ToString(),
                    aciklama = hesaplamaList[i].aciklama,
                    kararVerilenCekmetolerans = hesaplamaList[i].kararVerilenCekmeTolerans.ToString()
                };

                ortalamaDTOs.Add(dto);

                if (anaCekmeList.Count != 0)
                {
                    RaporlamaDataModel.RaporAnaCekmeDTO anaCekmeDTO = new RaporlamaDataModel.RaporAnaCekmeDTO()
                    {
                        Id = anaCekmeList[i].Id,
                        RaporId = anaCekmeList[i].RaporId,
                        OlcuNoktaId = anaCekmeList[i].OlcuNoktaId,
                        OlcuNoktaAd = anaCekmeList[i].OlcuNoktaAd,
                        AnaCekmeCekmeTolerans = anaCekmeList[i].AnaCekmeCekmeTolerans.ToString()
                    };

                    anaCekmeDTOs.Add(anaCekmeDTO);
                }

            }

            List<RaporBagliOrder> raporBagliOrderList = db.RaporBagliOrder.Where(x => x.raporId == detay.id).ToList();
            List<RaporlamaDataModel.RaporBagliOrderDTO> bagliOrderDTOs = new List<RaporlamaDataModel.RaporBagliOrderDTO>();

            if (raporBagliOrderList.Count > 0)
            {
                for (int i = 0; i < raporBagliOrderList.Count; i++)
                {
                    RaporlamaDataModel.RaporBagliOrderDTO raporBagliOrderDTO = new RaporlamaDataModel.RaporBagliOrderDTO()
                    {
                        Id = raporBagliOrderList[i].id,
                        OrderId = raporBagliOrderList[i].orderId,
                        OrderNo = raporBagliOrderList[i].orderNo,
                        RaporId = raporBagliOrderList[i].raporId
                    };

                    bagliOrderDTOs.Add(raporBagliOrderDTO);
                }


            }

            data.bagliOrderList = bagliOrderDTOs;
            data.araList = araDtos;
            data.hesaplamaList = hesaplamaDTOs;
            data.ortalamaList = ortalamaDTOs;
            data.anaCekmeList = anaCekmeDTOs;
            ViewBag.orders = GetOrders(x => x.orderNo.StartsWith("Ü21") || x.orderNo.StartsWith("EKS21") || x.orderNo.StartsWith("S21") || x.orderNo.StartsWith("T21") || x.orderNo.StartsWith("K21"));

            return View(data);
        }

        public ActionResult KarsilastirmaliRaporlamaTable(GelismisRaporlamaParameters tableParameters = null)
        {
            List<KazanDetayModel> kazanDetayModels = GetKazanDetayModel(tableParameters);
            ViewBag.isFromNumunePage = tableParameters.IsFromNumunePage;
            ViewBag.isAddingOrder = tableParameters.isAddingOrder;
            return PartialView(kazanDetayModels);
        }
        public JsonResult SearchOrders(string term)
        {
            List<OrderDTO> searchedOrders = db.Order.Where(x => x.orderNo.Contains(term)).Select(x => new OrderDTO { orderID = x.ID, orderNo = x.orderNo }).ToList();

            return Json(searchedOrders, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SearchCustomers(string term)
        {
            var searchedSilks = db.Order.Where(x => x.musteri.Contains(term)).Select(x => new { MusteriId = x.musteri, Musteri = x.musteri }).Distinct().ToList();

            return Json(searchedSilks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchModels(string term)
        {
            var searchedModels = db.Order.Where(x => x.modelAd.Contains(term)).Select(x => new { ModelId = x.modelAd, ModelAd = x.modelAd }).Distinct().ToList();

            return Json(searchedModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchSilks(string term)
        {
            var searchedSilks = db.Order.Where(x => x.kumas.Contains(term)).Select(x => new { KumasId = x.kumas, Kumas = x.kumas }).Distinct().ToList();

            return Json(searchedSilks, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SearchWorkShop(string term)
        {
            List<DTOKaliteAtolye> returnAtolyeList = new List<DTOKaliteAtolye>();

            using (var connection = new SqlConnection(ConnectionStrings.DikimhaneConnection))
            {
                connection.Open();

                SqlCommand cmd_Atolyes = new SqlCommand(@"select Id,AtolyeAd from Atolyes where AtolyeAd like '%"+term+"%'", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd_Atolyes);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                returnAtolyeList = ConvertDataTable<DTOKaliteAtolye>(dt);

                connection.Close();
            }

            return Json(returnAtolyeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchSingleOrder(string term)
        {
            OrderDTO searchedOrders = db.Order.Where(x => x.orderNo == term).Select(x => new OrderDTO { orderID = x.ID, orderNo = x.orderNo }).FirstOrDefault();

            return Json(searchedOrders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectedList(List<int> id = null, int isFromNumune = 0, List<int> raporTuruListe = null, int isAddingOrder = 0)
        {
            ViewBag.isFromNumunePage = isFromNumune;
            ViewBag.isAddingOrder = isAddingOrder;
            if (id != null)
            {
                List<KazanDetayModel> kazanlar = new List<KazanDetayModel>();

                for (int i = 0; i < id.Count; i++)
                {
                    if (raporTuruListe[i] == 0)
                    {
                        int id2 = id[i];
                        int raporTuru = raporTuruListe[i];

                        KazanDetay kazanDetay = db.KazanDetay.Where(x => x.id == id2).FirstOrDefault();

                        KazanDetayModel kazan = new KazanDetayModel()
                        {
                            kazanDetayID = kazanDetay.id,
                            orderNo = kazanDetay.Order.orderNo,
                            raporTuru = raporTuru
                        };

                        kazanlar.Add(kazan);
                    }
                    else
                    {
                        int id2 = id[i];
                        int raporTuru = raporTuruListe[i];

                        NumuneDetay kazanDetay = db.NumuneDetay.Where(x => x.id == id2).FirstOrDefault();

                        KazanDetayModel kazan = new KazanDetayModel()
                        {
                            kazanDetayID = kazanDetay.id,
                            orderNo = kazanDetay.Order.orderNo,
                            raporTuru = raporTuru

                        };

                        kazanlar.Add(kazan);
                    }
                }

                return PartialView(kazanlar);

            }
            else
            {
                return PartialView();
            }
        }

        public ViewResult KarsilastirmaModal(string idList, string orderNoList, int tabloTuru, string raporTuruList)
        {
            RaporlamaDataModel data = GetRaporlamaDataModel(idList, orderNoList, tabloTuru, raporTuruList);
            //ViewBag.orders = GetOrders();
            ViewBag.isFromNumunePage = 0;
            return View(data);
        }

        public JsonResult goToRecipePage(string orderNo)
        {
            try
            {
                int id = dbRecipe.RECIPE.Where(x => x.ORDER_NO == orderNo).Select(x => x.ID).FirstOrDefault();

                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PrintModal(string orderNo, int kazanDetayID, int raporTuru)
        {
            /*
             * To-Do: 
             * Önce Tablosunu alacağım.Done
             * Sonra Yıkama Öncesi Kazanını.Done
             * Sonra Yıkama Sonrası Ölçüsünü.Done
             */

            List<PrintModalDataModel> printedList = new List<PrintModalDataModel>();

            if (raporTuru == 0)
            {
                var kazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayID).FirstOrDefault();

                PrintModalDataModel modelKazan = new PrintModalDataModel()
                {
                    id = kazanDetayID,
                    kazanDetay = kazanDetay,
                    tur = 0,
                    raporTuru = 0
                };

                int tabloTuru = kazanDetay.tabloTuru;

                int olcuTuru = kazanDetay.olcuTuruID;

                if (tabloTuru == 1)
                {
                    //Yıkama sonrasıysa bağlı olduğu yıkama öncesi tablosunu alıyorum
                    int yoKazanDetayID = 0;
                    if (olcuTuru != 22)
                    {
                        if(olcuTuru != 16)
                        {
                            yoKazanDetayID = db.KazanDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == 17 && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();
                        }
                        else
                        {
                            yoKazanDetayID = db.KazanDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == 16 && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();

                        }
                        if (yoKazanDetayID == 0)
                        {
                            yoKazanDetayID = db.KazanDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == 16 && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();
                        }
                    }
                    else
                    {
                        yoKazanDetayID = db.KazanDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == olcuTuru && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();
                    }

                    var yoKazanDetay = db.KazanDetay.Where(x => x.id == yoKazanDetayID).FirstOrDefault();

                    PrintModalDataModel modelKazanYO = new PrintModalDataModel()
                    {
                        id = yoKazanDetayID,
                        kazanDetay = yoKazanDetay,
                        tur = 0
                    };

                    // Bunu imalat tablosunu alırken kullanacağım.
                    string maxEnBoyCekme = db.Database.SqlQuery<string>("[dbo].[MaxCekmeGetir] @kazanDetayID", new SqlParameter("@kazanDetayID", yoKazanDetayID)).FirstOrDefault();

                    modelKazan.enBoyCekme = maxEnBoyCekme;
                    modelKazanYO.enBoyCekme = maxEnBoyCekme;
                    int olcuTablosuOlcuTuruId = 0;
                    if (olcuTuru == 20)
                    {
                        olcuTablosuOlcuTuruId = 29;
                    }
                    else if (olcuTuru == 21)
                    {
                        olcuTablosuOlcuTuruId = 28;
                    }
                    else
                    {
                        olcuTablosuOlcuTuruId = 27;
                    }

                    var ysOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == olcuTablosuOlcuTuruId && x.tabloTuru == 1).FirstOrDefault();

                    PrintModalDataModel modelYSOlcuTablosu = new PrintModalDataModel()
                    {
                        id = ysOlcuTabloDetay.id,
                        olcuTabloDetay = ysOlcuTabloDetay,
                        tur = 1,
                        raporTuru = 0
                    };

                    printedList.Add(modelYSOlcuTablosu);


                    var yoOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == olcuTablosuOlcuTuruId && x.tabloTuru == 0 && x.enBoyCekme == maxEnBoyCekme).FirstOrDefault();
                    PrintModalDataModel modelYOOlcuTablosu = new PrintModalDataModel()
                    {
                        id = yoOlcuTabloDetay.id,
                        olcuTabloDetay = yoOlcuTabloDetay,
                        tur = 1,
                        raporTuru = 0
                    };
                    printedList.Add(modelYOOlcuTablosu);

                    printedList.Add(modelKazanYO);
                    printedList.Add(modelKazan);

                }
                else
                {
                    string maxEnBoyCekme = db.Database.SqlQuery<string>("[dbo].[MaxCekmeGetir] @kazanDetayID", new SqlParameter("@kazanDetayID", kazanDetayID)).FirstOrDefault();
                    modelKazan.enBoyCekme = maxEnBoyCekme;
                    int olcuTablosuOlcuTuruId = 0;
                    if (olcuTuru == 20)
                    {
                        olcuTablosuOlcuTuruId = 29;
                    }
                    else if (olcuTuru == 21)
                    {
                        olcuTablosuOlcuTuruId = 28;
                    }
                    else
                    {
                        olcuTablosuOlcuTuruId = 27;
                    }

                    var ysOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == olcuTablosuOlcuTuruId && x.tabloTuru == 1).FirstOrDefault();

                    PrintModalDataModel modelYSOlcuTablosu = new PrintModalDataModel()
                    {
                        id = ysOlcuTabloDetay.id,
                        olcuTabloDetay = ysOlcuTabloDetay,
                        tur = 1
                    };

                    printedList.Add(modelYSOlcuTablosu);


                    var yoOlcuTabloDetay = db.OlcuTabloDetay.Where(x => x.Order.orderNo == orderNo && x.olcuTuruID == olcuTablosuOlcuTuruId && x.tabloTuru == 0 && x.enBoyCekme == maxEnBoyCekme).FirstOrDefault();
                    PrintModalDataModel modelYOOlcuTablosu = new PrintModalDataModel()
                    {
                        id = yoOlcuTabloDetay.id,
                        olcuTabloDetay = yoOlcuTabloDetay,
                        tur = 1
                    };
                    printedList.Add(modelYOOlcuTablosu);
                    printedList.Add(modelKazan);
                }
            }
            else
            {
                var numuneDetay = db.NumuneDetay.Where(x => x.id == kazanDetayID).FirstOrDefault();

                List<string> bedenler = db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayID).Select(x => x.Bedenler.beden).Distinct().ToList();

                foreach (var item in bedenler)
                {
                    PrintModalDataModel modelNumune = new PrintModalDataModel()
                    {
                        id = kazanDetayID,
                        numuneDetay = numuneDetay,
                        tur = 2,
                        raporTuru = 1,
                        enBoyCekme = numuneDetay.kalipAdi,
                        bedenAd = item
                    };
                    printedList.Add(modelNumune);
                }


            }



            return PartialView(printedList);
        }

        public ViewResult KurutmaRapor()
        {
            return View();
        }

        public PartialViewResult SearchOrder(string orderNo)
        {

            KazanDetay detay = db.KazanDetay.Where(x => x.Order.orderNo.Contains(orderNo) && x.tabloTuru == 1 && x.kDerece != null && x.kDakika != null).OrderByDescending(x => x.tarih).FirstOrDefault();
            KurutmaModel kurutma = null;
            if (detay != null)
            {
                kurutma = new KurutmaModel()
                {
                    orderNo = detay.Order.orderNo,
                    derece = (int)detay.kDerece,
                    dakika = (int)detay.kDakika
                };

                return PartialView(kurutma);
            }
            else
            {
                return PartialView(kurutma);
            }

        }

        public ViewResult OrderKimlik(string orderNo = "")
        {
            if (orderNo == "")
            {
                return View();
            }
            else
            {
                List<KazanDetayModel> orderList = new List<KazanDetayModel>();

                List<KazanDetay> detayList = db.KazanDetay.Where(x => x.Order.orderNo == orderNo).ToList();

                if (detayList != null)
                {
                    foreach (var item in detayList)
                    {
                        KazanDetayModel order = new KazanDetayModel();


                        order.kazanDetayID = item.id;
                        order.orderNo = item.Order.orderNo;
                        order.modelNo = item.Order.modelNo;
                        order.kumasAdi = item.Order.kumas;
                        order.musteri = item.Order.musteri;

                        SqlParameter atolyeAd = new SqlParameter("@atolyeAd", item.atolyeID);

                        var result = db.Database.SqlQuery<string>(@"exec AtolyeBul @atolyeAd", atolyeAd).FirstOrDefault();

                        order.atolyeAd = result;
                        order.modelAdi = item.Order.modelAd;
                        order.yikamaYeri = item.yikamaYeri;
                        order.olcuTuru = item.OlcuTurleri.olcuTuruAd;

                        int tabloTuru = item.tabloTuru;

                        if (tabloTuru == 0)
                        {
                            order.tabloTuru = "Yıkama Öncesi";
                        }
                        else
                        {
                            order.tabloTuru = "Yıkama Sonrası";
                        }

                        order.tarih = item.tarih;
                        order.kullanıcıAdSoyad = item.PersonelTablo.personelAd + " " + item.PersonelTablo.personelSoyad;


                        orderList.Add(order);
                    }
                    return View(orderList);
                }
                else
                {
                    return View();
                }

            }
        }

        public ViewResult KumasRapor()
        {
            return View();
        }

        public ViewResult TabloGoster(int id, int tur, string bedenAd = "")
        {
            ViewBag.tur = tur;
            TabloDTOModel table = GetTable(id, tur, bedenAd);

            if (tur == 0)
            {
                KazanDetay DbKazan = db.KazanDetay.Where(x => x.id == id).FirstOrDefault();

                ViewBag.tarih = DbKazan.tarih.ToString("dd-MM-yyyy");
                ViewBag.olcuTuruAd = DbKazan.OlcuTurleri.olcuTuruAd;
                ViewBag.tabloTuruAd = DbKazan.tabloTuru == 0 ? "Yıkama Öncesi" : "Yıkama Sonrası";
                ViewBag.model = DbKazan.Order.modelAd;
                ViewBag.musteri = DbKazan.Order.musteri;
                ViewBag.artikel = DbKazan.Order.modelNo;
                ViewBag.orderNo = DbKazan.Order.orderNo;

                if (DbKazan.atolyeID != 0)
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

                ViewBag.fasonMu = (DbKazan.OlcuTurleri.olcuTuruAd == "Fason Ham Ölçü" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Ön Final" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Son Final" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Sevk");
            }
            else
            {

            }

            return View(table);
        }

        public PartialViewResult TablePrint(List<int> id, List<int> tur, List<string> beden = null)
        {
            List<TabloDTOModel> dtos = new List<TabloDTOModel>();

            for (int i = 0; i < id.Count; i++)
            {
                TabloDTOModel dto = new TabloDTOModel();

                if (beden.Count > 0)
                {
                    dto = GetTable(id[i], tur[i], beden[i]);
                }
                else
                {
                    dto = GetTable(id[i], tur[i]);
                }
                dtos.Add(dto);
            }
            dtos = dtos.OrderBy(x => x.tur).ToList();

            return PartialView(dtos);
        }

        public JsonResult SaveReport(RaporlamaDataModel data, int id = 0, int yeniKayitMi = 0)
        {
            try
            {
                RaporDetay raporDetay = SaveArsiv(data, id, yeniKayitMi);

                if (raporDetay == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                return Json(raporDetay.id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        public PartialViewResult PrintRapor(RaporlamaDataModel data)
        {
            return PartialView(data);
        }

        public PartialViewResult AddOrderToArsiv(int tabloTuru)
        {
            ViewBag.tabloTuru = tabloTuru;
            return PartialView();
        }

        public PartialViewResult AddOrderToArsivTable(string orderNo = "", string modelNo = "", string kumasAdi = "", string musteri = "", string atolye = "", string modelAdi = "", string yikamaYeri = "", int tabloTuru = 0, string tarih1 = "", string tarih2 = "")
        {
            GelismisRaporlamaParameters parameters = new GelismisRaporlamaParameters()
            {
                ModelAdi = modelAdi,
                OrderNo = orderNo,
                StartDate = tarih1,
                EndDate = tarih2,
                Atolye = atolye,
                TabloTuru = tabloTuru,
                YikamaYeri = yikamaYeri,
                ModelNo = modelNo,
                Kumas = kumasAdi,
                Musteri = musteri
            };
            List<KazanDetayModel> kazanDetayModels = GetKazanDetayModel(parameters);
            return PartialView(kazanDetayModels);
        }

        public PartialViewResult SelectedListArsiv(List<int> id = null)
        {
            if (id != null)
            {
                List<KazanDetayModel> kazanlar = new List<KazanDetayModel>();
                foreach (var item in id)
                {
                    int id2 = item;
                    KazanDetay kazanDetay = db.KazanDetay.Where(x => x.id == id2).FirstOrDefault();

                    KazanDetayModel kazan = new KazanDetayModel()
                    {
                        kazanDetayID = kazanDetay.id,
                        orderNo = kazanDetay.Order.orderNo,

                    };

                    kazanlar.Add(kazan);
                }


                return PartialView(kazanlar);

            }
            else
            {
                return PartialView();
            }
        }

        public JsonResult GetKazanDetayValues(List<int> kazanDetayId = null, int firstKazanDetayId = 0, List<int> raporTuruList = null)
        {
            try
            {
                RaporlamaDataModel data = new RaporlamaDataModel();

                List<RaporlamaDataModel.RaporAraDTO> raporAraDTOs = new List<RaporlamaDataModel.RaporAraDTO>();
                List<List<RaporlamaDataModel.RaporHesaplamaDTO>> raporHesaplamaDTOsList = new List<List<RaporlamaDataModel.RaporHesaplamaDTO>>();

                List<int> tabanIDMustList = new List<int> { 1, 4, 3, 140, 83, 5, 85, 7, 88, 9, 13, 14, 11, 12 };

                List<int> tabanID = db.KazanAra.Where(x => x.kazanDetayID == firstKazanDetayId && x.OlcuNoktalari.tabanID != 82 && x.OlcuNoktalari.tabanID != 21).Select(x => x.OlcuNoktalari.tabanID).Distinct().ToList();

                tabanID = tabanID.OrderBy(x => tabanIDMustList.IndexOf(x)).ToList();

                foreach (var item in kazanDetayId)
                {
                    var kazanDetay = db.KazanDetay.Where(x => x.id == item).FirstOrDefault();

                    string enBoyCekme = GetMaxCekme(item);

                    RaporlamaDataModel.RaporAraDTO raporAraDTO = new RaporlamaDataModel.RaporAraDTO()
                    {
                        kazanDetayID = item,
                        olcuTuruID = kazanDetay.olcuTuruID,
                        olcuTuruAd = kazanDetay.OlcuTurleri.olcuTuruAd,
                        orderID = kazanDetay.orderID,
                        orderNo = kazanDetay.Order.orderNo,
                        enBoyCekme = enBoyCekme
                    };

                    raporAraDTOs.Add(raporAraDTO);
                    //Burayı Düzelt
                    List<RaporlamaDataModel.RaporHesaplamaDTO> raporHesaplamaDTOs = GetHesaplamaList(item, tabanID, raporTuruList[0]);

                    raporHesaplamaDTOsList.Add(raporHesaplamaDTOs);
                }

                data.araList = raporAraDTOs;
                data.hesaplamaList = raporHesaplamaDTOsList;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult AddColumn(List<RaporlamaDataModel.RaporAraDTO> data)
        {
            return PartialView(data);
        }

        public JsonResult GetTableNameString(int firstOrderNoId, int tabloTuru)
        {
            var order = db.Order.Where(x => x.ID == firstOrderNoId).FirstOrDefault();

            List<int?> raporDetayList = db.RaporAra.Where(x => x.orderID == firstOrderNoId && x.RaporDetay.tabloTuru == tabloTuru).Select(x => x.RaporDetay.kalipNo).Distinct().ToList();

            int lastKalipNo = 1;

            if (raporDetayList.Count > 0)
            {
                lastKalipNo = (int)raporDetayList.Max(x => x);
            }

            int index = order.modelNo.IndexOf("-");
            if (index > 0)
                order.modelNo = order.modelNo.Substring(0, index);

            return Json(new { modelNo = order.modelNo, modelAd = order.modelAd, lastKalipNo = lastKalipNo }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AnaCekmeModal()
        {
            return PartialView();
        }
        public PartialViewResult KazanPartiRaporlama()
        {
            var detayList = db.KazanDetay.Where(x => x.durum == true).OrderByDescending(x => x.id).Take(20).ToList();

            List<DTORaporOrderList> dtos = new List<DTORaporOrderList>();

            foreach (var item in detayList)
            {
                int id = item.id;
                bool checkExist = db.KazanHesaplama.Any(x => x.kazanDetayID == id);

                if (checkExist)
                {
                    DTORaporOrderList dto = new DTORaporOrderList();

                    int orderID = item.orderID;
                    int kullanıcıID = item.kullaniciID;
                    int olcuTuruID = item.olcuTuruID;
                    int kazandetayID = item.id;

                    dto.id = kazandetayID;
                    dto.orderNo = db.Order.Where(x => x.ID == orderID).Select(x => x.orderNo).FirstOrDefault();
                    dto.kullanıcıAdı = db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelAd).FirstOrDefault();
                    dto.olcuTuruAd = db.OlcuTurleri.Where(x => x.id == olcuTuruID).Select(x => x.olcuTuruAd).FirstOrDefault();
                    dto.tarih = item.tarih;

                    dtos.Add(dto);
                }


            }

            return PartialView(dtos);
        }


        public PartialViewResult AnaCekmeModalTable(string raporAdi = "", string orderNo = "", string modelNo = "", string kumas = "", string musteri = "", string atolye = "", string modelAdi = "", string yikamaYeri = "", string tarih1 = "", string tarih2 = "")
        {
            List<ArsivDataModel> data = GetArsivDataModel(raporAdi, orderNo, modelNo, kumas, musteri, atolye, modelAdi, yikamaYeri, tarih1, tarih2);

            return PartialView(data);
        }

        public JsonResult SetAnaCekme(int raporId)
        {
            RaporlamaDataModel data = GetRaporlamaValues(raporId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult KumasDetailModal(KumasKarakteri kumasKarakteri = null, int isAddingOrder = 0)
        {
            List<string> distinctKonstruksiyonParameters = db.KumasKarakteri.Select(x => x.konstrüksiyonParametre1).Distinct().ToList();

            distinctKonstruksiyonParameters = distinctKonstruksiyonParameters.Concat(db.KumasKarakteri.Select(x => x.konstrüksiyonParametre2).Distinct().ToList()).ToList();

            distinctKonstruksiyonParameters = distinctKonstruksiyonParameters.Concat(db.KumasKarakteri.Select(x => x.konstrüksiyonParametre3).Distinct().ToList()).ToList();

            distinctKonstruksiyonParameters = distinctKonstruksiyonParameters.Concat(db.KumasKarakteri.Select(x => x.konstrüksiyonParametre4).Distinct().ToList()).ToList();

            distinctKonstruksiyonParameters = distinctKonstruksiyonParameters.Concat(db.KumasKarakteri.Select(x => x.konstrüksiyonParametre5).Distinct().ToList()).ToList();

            distinctKonstruksiyonParameters = distinctKonstruksiyonParameters.Concat(db.KumasKarakteri.Select(x => x.konstrüksiyonParametre6).Distinct().ToList()).ToList();

            distinctKonstruksiyonParameters = distinctKonstruksiyonParameters.Where(x => x != null).Distinct().ToList();

            ViewBag.KumasAdList = db.KumasKarakteri.Select(x => x.kumasAdı).ToList();
            ViewBag.KumasKonstruksiyon = distinctKonstruksiyonParameters;
            ViewBag.isAddingOrder = isAddingOrder;

            if (kumasKarakteri != null)
            {
                if (kumasKarakteri.id != 0)
                {
                    if (kumasKarakteri.kesilebilirEnOrtalama == null)
                    {
                        kumasKarakteri.kesilebilirEnOrtalama = (kumasKarakteri.kesilebilirEnMin + kumasKarakteri.kesilebilirEnMax) / 2;
                    }

                    if (kumasKarakteri.tamEnOrtalama == null)
                    {
                        kumasKarakteri.tamEnOrtalama = (kumasKarakteri.tamEnMin + kumasKarakteri.tamEnMax) / 2;
                    }

                    if (kumasKarakteri.kumasBoyCekmeOrtalama == null)
                    {
                        kumasKarakteri.kumasBoyCekmeOrtalama = (kumasKarakteri.kumasBoyCekmeMin + kumasKarakteri.kumasBoyCekmeMax) / 2;
                    }

                    if (kumasKarakteri.kumasEnCekmeOrtalama == null)
                    {
                        kumasKarakteri.kumasEnCekmeOrtalama = (kumasKarakteri.kumasEnCekmeMin + kumasKarakteri.kumasEnCekmeMax) / 2;
                    }
                }

                ViewBag.kumasKarakteriFromNumunePage = kumasKarakteri;
            }

            return PartialView();
        }

        public PartialViewResult ReceteDetailModal(List<RECIPE_STEPS> recipeStepsFromNumunePage = null, int isAddingOrder = 0)
        {
            var receteAdımList = dbRecipe.RECIPE_STEPS.Where(x => x.WASHING_TYPE_NAME == "Ön Yıkama" || x.WASHING_TYPE_NAME == "Taş" || x.WASHING_TYPE_NAME == "Enzim" || x.WASHING_TYPE_NAME == "Kasar" || x.WASHING_TYPE_NAME == "Ağartma Hypo" ||
            x.WASHING_TYPE_NAME == "Enzim Tüy" || x.WASHING_TYPE_NAME == "Yumuşatma" || x.WASHING_TYPE_NAME == "Kurutma").Select(x => x.WASHING_TYPE_NAME).Distinct().ToList();

            ViewBag.yikamaKodList = dbRecipe.RECIPE.Where(x => x.WASHING_CODE != null).Select(x => x.WASHING_CODE).Distinct().ToList();
            ViewBag.recipeSteps = receteAdımList;
            ViewBag.isAddingOrder = isAddingOrder;

            if (recipeStepsFromNumunePage != null)
            {
                ViewBag.recipeStepsFromNumunePage = recipeStepsFromNumunePage;
            }

            return PartialView();

        }

        public PartialViewResult KumasList(KumasDetailModel kumasDetail, int isAddingOrder = 0)
        {
            List<KumasKarakteriDto> kumasKarakteris = FindClosestFabrics(kumasDetail);

            ViewBag.kumasDetail = kumasDetail;
            ViewBag.isAddingOrder = isAddingOrder;

            return PartialView(kumasKarakteris);
        }

        public JsonResult GetKumasDetail(string kumasAd)
        {
            KumasKarakteri kumas = db.KumasKarakteri.Where(x => x.kumasAdı == kumasAd).FirstOrDefault();

            return Json(kumas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReceteDetail(string yikamaKodu)
        {
            var recipeSteps = db.Database.SqlQuery<RECIPE_STEPS>(@"exec [dbo].[GetRecipeStepsByRDCode] @receteKodu", new SqlParameter("@receteKodu", yikamaKodu)).ToList();
            return Json(recipeSteps, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetOrder(string orderNo)
        {
            if (String.IsNullOrEmpty(orderNo))
            {
                var detayList = db.KazanDetay.Where(x => x.durum == true).OrderByDescending(x => x.id).Take(20).ToList();

                List<DTORaporOrderList> dtos = new List<DTORaporOrderList>();

                foreach (var item in detayList)
                {
                    int id = item.id;
                    bool checkExist = db.KazanHesaplama.Any(x => x.kazanDetayID == id);

                    if (checkExist)
                    {
                        DTORaporOrderList dto = new DTORaporOrderList();

                        int orderID = item.orderID;
                        int kullanıcıID = item.kullaniciID;
                        int olcuTuruID = item.olcuTuruID;
                        int kazandetayID = item.id;

                        dto.id = kazandetayID;
                        dto.orderNo = db.Order.Where(x => x.ID == orderID).Select(x => x.orderNo).FirstOrDefault();
                        dto.kullanıcıAdı = db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelAd).FirstOrDefault();
                        dto.olcuTuruAd = db.OlcuTurleri.Where(x => x.id == olcuTuruID).Select(x => x.olcuTuruAd).FirstOrDefault();
                        dto.tarih = item.tarih;

                        dtos.Add(dto);
                    }
                }

                return PartialView(dtos);
            }
            else
            {
                List<Order> orders = db.Order.Where(x => x.orderNo.Contains(orderNo)).ToList();
                List<KazanDetay> detayList = new List<KazanDetay>();

                foreach (var item2 in orders)
                {
                    int orderID = item2.ID;
                    List<KazanDetay> orderDetay = db.KazanDetay.Where(x => x.orderID == orderID && x.durum == true).ToList();

                    detayList.AddRange(orderDetay);
                }


                List<DTORaporOrderList> dtos = new List<DTORaporOrderList>();

                foreach (var item in detayList)
                {
                    int id = item.id;
                    bool checkExist = db.KazanHesaplama.Any(x => x.kazanDetayID == id);

                    if (checkExist)
                    {
                        DTORaporOrderList dto = new DTORaporOrderList();

                        int orderID = item.orderID;
                        int kullanıcıID = item.kullaniciID;
                        int olcuTuruID = item.olcuTuruID;
                        int kazandetayID = item.id;

                        dto.id = kazandetayID;
                        dto.orderNo = db.Order.Where(x => x.ID == orderID).Select(x => x.orderNo).FirstOrDefault();
                        dto.kullanıcıAdı = db.PersonelTablo.Where(x => x.id == kullanıcıID).Select(x => x.personelAd).FirstOrDefault();
                        dto.olcuTuruAd = db.OlcuTurleri.Where(x => x.id == olcuTuruID).Select(x => x.olcuTuruAd).FirstOrDefault();
                        dto.tarih = item.tarih;

                        dtos.Add(dto);
                    }
                }

                return PartialView(dtos);
            }
        }

        //Helper Methods
        #region

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

        public List<RaporlamaDataModel.RaporHesaplamaDTO> GetHesaplamaList(int kazanDetayId, List<int> tabanIdList, int raporTuru, int bedenId = 0, int tabloTuru = 0, int yikamaOncesiMi = 0)
        {
            List<RaporlamaDataModel.RaporHesaplamaDTO> raporHesaplamaDTOs = new List<RaporlamaDataModel.RaporHesaplamaDTO>();

            for (int j = 0; j < tabanIdList.Count; j++)
            {
                int tabanIDGet = tabanIdList[j];
                var olcuNoktasi = db.OlcuNoktalari.Where(x => x.tabanID == tabanIDGet && x.anaNoktami == true).FirstOrDefault();

                double ortalamaDeger = 0;
                double uygulananCekme = 0;
                double gerceklesenCekme = 0;
                bool icboyYapildimi = false;

                if (raporTuru == 0)
                {
                    if (tabanIDGet != 11 && tabanIDGet != 12)
                    {
                        SqlParameter param1 = new SqlParameter("@kazanid", kazanDetayId);
                        SqlParameter param2 = new SqlParameter("@tabanID", tabanIDGet);
                        var cmd = db.Database.Connection.CreateCommand();

                        cmd.CommandText = "[dbo].[RaporlamaTolCekme] @tabanID,@kazanid";
                        cmd.Parameters.Add(param2);
                        cmd.Parameters.Add(param1);

                        db.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ortalamaDeger = reader.GetDouble(0);
                                uygulananCekme = reader.GetDouble(1);
                                gerceklesenCekme = reader.GetDouble(2);
                            }
                            reader.Close();
                        }

                        db.Database.Connection.Close();
                    }
                    else
                    {
                        if (icboyYapildimi == false)
                        {
                            List<int> olcunoktaidList = new List<int>();

                            if (tabanIDGet == 11)
                            {
                                olcunoktaidList = db.KazanAra.Where(x => x.kazanDetayID == kazanDetayId && (x.OlcuNoktalari.tabanID == 11)).Select(x => x.olcuNoktaID).Distinct().ToList();
                            }
                            else if (tabanIDGet == 12)
                            {
                                olcunoktaidList = db.KazanAra.Where(x => x.kazanDetayID == kazanDetayId && (x.OlcuNoktalari.tabanID == 12)).Select(x => x.olcuNoktaID).Distinct().ToList();
                            }

                            double ortalamaToplam = 0;
                            double gerceklesenTolCekmeToplam = 0;
                            double uygulananTolCekmeToplam = 0;
                            double countToplam = 0;

                            for (int k = 0; k < olcunoktaidList.Count; k++)
                            {
                                int olcunoktaID = olcunoktaidList[k];
                                int count = db.KazanAra.Where(x => x.kazanDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.deger != null).Count();

                                double ortalamaDegerIcBoy = (db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId && x.olcuNoktasiID == olcunoktaID).Select(x => x.ortalamaDeger).FirstOrDefault()) * count;
                                double gerceklesenTolCekmeIcBoy = (db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId && x.olcuNoktasiID == olcunoktaID).Select(x => x.gerceklesenTolCekme).FirstOrDefault()) * count;
                                double uygulananTolCekmeIcBoy = (db.KazanHesaplama.Where(x => x.kazanDetayID == kazanDetayId && x.olcuNoktasiID == olcunoktaID).Select(x => x.uygulananTolCekme).FirstOrDefault()) * count;

                                if (gerceklesenTolCekmeIcBoy != 0)
                                {
                                    ortalamaToplam += ortalamaDegerIcBoy;
                                    gerceklesenTolCekmeToplam += gerceklesenTolCekmeIcBoy;
                                    uygulananTolCekmeToplam += uygulananTolCekmeIcBoy;
                                    countToplam += count;
                                }
                            }


                            if (countToplam != 0)
                            {
                                ortalamaToplam = Math.Round(ortalamaToplam / countToplam, 2, MidpointRounding.AwayFromZero);
                                gerceklesenTolCekmeToplam = Math.Round(gerceklesenTolCekmeToplam / countToplam, 2, MidpointRounding.AwayFromZero);
                                uygulananTolCekmeToplam = Math.Round(uygulananTolCekmeToplam / countToplam, 2, MidpointRounding.AwayFromZero);

                                ortalamaDeger = ortalamaToplam;
                                uygulananCekme = uygulananTolCekmeToplam;
                                gerceklesenCekme = gerceklesenTolCekmeToplam;

                            }
                            else
                            {
                                ortalamaDeger = 0;
                                uygulananCekme = 0;
                                gerceklesenCekme = 0;
                            }

                            icboyYapildimi = true;
                        }
                    }
                }
                else
                {
                    if (tabanIDGet != 11 && tabanIDGet != 12)
                    {
                        SqlParameter param1 = new SqlParameter("@numuneDetayId", kazanDetayId);
                        SqlParameter param2 = new SqlParameter("@tabanID", tabanIDGet);
                        SqlParameter param3 = new SqlParameter("@bedenID", bedenId);
                        SqlParameter param4 = new SqlParameter("@tabloTuru", tabloTuru);
                        SqlParameter param5 = new SqlParameter("@yikamaOncesiMi", yikamaOncesiMi);

                        var cmd = db.Database.Connection.CreateCommand();

                        cmd.CommandText = "[dbo].[RaporlamaTolCekmeNumune] @tabanID,@numuneDetayId,@bedenID,@tabloTuru,@yikamaOncesiMi";
                        cmd.Parameters.Add(param2);
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param3);
                        cmd.Parameters.Add(param4);
                        cmd.Parameters.Add(param5);

                        db.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ortalamaDeger = reader.GetDouble(0);
                                uygulananCekme = reader.GetDouble(1);
                                gerceklesenCekme = reader.GetDouble(2);
                            }
                            reader.Close();
                        }

                        db.Database.Connection.Close();
                    }
                    else
                    {
                        if (icboyYapildimi == false)
                        {

                            List<int> olcunoktaidList = db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.bedenID == bedenId && (x.OlcuNoktalari.tabanID == 11 || x.OlcuNoktalari.tabanID == 12)).Select(x => x.olcuNoktaID).Distinct().ToList();

                            double ortalamaToplam = 0;
                            double gerceklesenTolCekmeToplam = 0;
                            double uygulananTolCekmeToplam = 0;
                            double countToplam = 0;

                            for (int k = 0; k < olcunoktaidList.Count; k++)
                            {
                                int olcunoktaID = olcunoktaidList[k];

                                int count = 0;

                                if (tabloTuru == 0)
                                {
                                    count = db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.yikamaOncesiFark != null).Count();

                                    double ortalamaDegerIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaOncesiFark).FirstOrDefault()) * count;

                                    double yikOncesiTabloIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaOncesiTablo).FirstOrDefault()) * count;
                                    double kalipOlcusuIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.kalipOlcusu).FirstOrDefault()) * count;
                                    double yikSonrasiTabloIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaSonrasiTablo).FirstOrDefault()) * count;


                                    double uygulananTolCekmeIcBoy = Math.Round(kalipOlcusuIcBoy - yikOncesiTabloIcBoy, 2, MidpointRounding.AwayFromZero);
                                    double gerceklesenTolCekmeIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => (x.yikamaOncesiOlculen) - kalipOlcusuIcBoy).FirstOrDefault()) * count;

                                    if (gerceklesenTolCekmeIcBoy != 0)
                                    {
                                        ortalamaToplam += ortalamaDegerIcBoy;
                                        gerceklesenTolCekmeToplam += gerceklesenTolCekmeIcBoy;
                                        uygulananTolCekmeToplam += uygulananTolCekmeIcBoy;
                                        countToplam += count;
                                    }
                                }
                                else
                                {
                                    count = db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.yikamaSonrasiFark != null && x.bedenID == bedenId).Count();

                                    double yikOncesiTabloIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaOncesiTablo).FirstOrDefault()) * count;
                                    double kalipOlcusuIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.kalipOlcusu).FirstOrDefault()) * count;
                                    double yikSonrasiTabloIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaSonrasiTablo).FirstOrDefault()) * count;
                                    double yikSonrasiIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaSonrasiOlculen).FirstOrDefault()) * count;
                                    double yikOncesiIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaOncesiOlculen).FirstOrDefault()) * count;

                                    double ortalamaDegerIcBoy = (double)(db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayId && x.olcuNoktaID == olcunoktaID && x.bedenID == bedenId).Select(x => x.yikamaSonrasiFark).FirstOrDefault()) * count;

                                    double uygulananTolCekmeIcBoy = Math.Round(((yikOncesiTabloIcBoy - yikSonrasiTabloIcBoy) / yikOncesiTabloIcBoy) * 100, 2, MidpointRounding.AwayFromZero);

                                    if (Double.IsNaN(uygulananTolCekmeIcBoy) || Double.IsInfinity(uygulananTolCekmeIcBoy))
                                    {
                                        uygulananTolCekmeIcBoy = 0;
                                    }

                                    double gerceklesenTolCekmeIcBoy = Math.Round(((yikOncesiIcBoy - yikSonrasiIcBoy) / yikOncesiIcBoy) * 100, 2, MidpointRounding.AwayFromZero);

                                    if (Double.IsNaN(gerceklesenTolCekmeIcBoy) || Double.IsInfinity(gerceklesenTolCekmeIcBoy) || gerceklesenTolCekmeIcBoy == 100)
                                    {
                                        gerceklesenTolCekmeIcBoy = 0;
                                    }




                                    if (gerceklesenTolCekmeIcBoy != 0)
                                    {
                                        ortalamaToplam += ortalamaDegerIcBoy;
                                        gerceklesenTolCekmeToplam += gerceklesenTolCekmeIcBoy;
                                        uygulananTolCekmeToplam += uygulananTolCekmeIcBoy;
                                        countToplam += count;
                                    }

                                }



                            }


                            if (countToplam != 0)
                            {
                                ortalamaToplam = Math.Round(ortalamaToplam / countToplam, 2, MidpointRounding.AwayFromZero);
                                gerceklesenTolCekmeToplam = Math.Round(gerceklesenTolCekmeToplam / countToplam, 2, MidpointRounding.AwayFromZero);
                                uygulananTolCekmeToplam = Math.Round(uygulananTolCekmeToplam / countToplam, 2, MidpointRounding.AwayFromZero);

                                ortalamaDeger = ortalamaToplam;
                                uygulananCekme = uygulananTolCekmeToplam;
                                gerceklesenCekme = gerceklesenTolCekmeToplam;

                            }
                            else
                            {
                                ortalamaDeger = 0;
                                uygulananCekme = 0;
                                gerceklesenCekme = 0;
                            }

                            icboyYapildimi = true;
                        }
                    }
                }


                RaporlamaDataModel.RaporHesaplamaDTO raporHesaplamaDTO = new RaporlamaDataModel.RaporHesaplamaDTO()
                {
                    olcuNoktaAd = olcuNoktasi.olcuNoktasi,
                    olcuNoktaID = olcuNoktasi.id,
                    gerceklesenCekmeTolerans = gerceklesenCekme.ToString(),
                    uygulananCekmeTolerans = uygulananCekme.ToString(),
                    ortalamaDeger = ortalamaDeger.ToString()
                };

                raporHesaplamaDTOs.Add(raporHesaplamaDTO);
            }

            return raporHesaplamaDTOs;
        }

        public RaporlamaDataModel GetRaporlamaValues(int raporId)
        {
            RaporlamaDataModel data = new RaporlamaDataModel();

            RaporDetay detay = db.RaporDetay.Where(x => x.id == raporId).FirstOrDefault();

            RaporlamaDataModel.RaporDetayDTO detayDto = new RaporlamaDataModel.RaporDetayDTO()
            {
                raporID = detay.id,
                kalipNo = (int)detay.kalipNo,
                anaCekmeVarMi = (int)detay.anaCekmeVarMi,
                raporAdi = detay.raporAdi
            };

            List<RaporOrtalama> raporOrtalamas = db.RaporOrtalama.Where(x => x.raporID == raporId).ToList();
            List<RaporlamaDataModel.RaporOrtalamaDTO> ortalamaDtoList = new List<RaporlamaDataModel.RaporOrtalamaDTO>();

            foreach (var item in raporOrtalamas)
            {
                RaporlamaDataModel.RaporOrtalamaDTO ortalamaDTO = new RaporlamaDataModel.RaporOrtalamaDTO()
                {
                    id = item.id,
                    raporID = item.raporID,
                    olcuNoktaID = item.olcuNoktaID,
                    olcuNoktaAd = item.olcuNoktaAd,
                    kararVerilenCekmetolerans = item.kararVerilenCekmeTolerans.ToString(),
                    ortGerceklesenCekmeTolerans = item.ortGerceklesenCekmeTolerans.ToString(),
                    ortUygulananCekmeTolerans = item.ortUygulananCekmeTolerans.ToString(),
                    ortOrtalamaDeger = item.ortOrtalamaDeger.ToString()
                };

                ortalamaDtoList.Add(ortalamaDTO);
            }

            data.detay = detayDto;
            data.ortalamaList = ortalamaDtoList;

            return data;
        }

        public RaporDetay SaveArsiv(RaporlamaDataModel data, int id = 0, int yeniKayitMi = 0, int orderId = 0)
        {
            try
            {

                RaporDetay raporDetay = new RaporDetay();
                if (yeniKayitMi == 1)
                {
                    raporDetay = new RaporDetay();
                }
                else
                {
                    raporDetay = db.RaporDetay.Where(x => x.id == id).FirstOrDefault();
                }


                raporDetay.kullaniciID = data.detay.kullaniciID;
                if (data.detay.raporAdi == null)
                {
                    data.detay.raporAdi = ReportNameBuilder(data, orderId);
                }

                raporDetay.raporAdi = data.detay.raporAdi;
                raporDetay.tabloTuru = data.detay.tabloTuru;
                raporDetay.tarih = DateTime.Now;
                raporDetay.anaCekmeVarMi = data.detay.anaCekmeVarMi;
                raporDetay.raporNotu = data.detay.raporNotu;

                if (data.detay.kalipNo == null || data.detay.kalipNo == 0)
                {
                    raporDetay.kalipNo = 1;
                }

                if (yeniKayitMi == 1)
                {
                    db.RaporDetay.Add(raporDetay);
                    db.SaveChanges();
                }
                else
                {
                    db.SaveChanges();
                }

                List<RaporAra> araList;
                List<RaporHesaplama> hesaplamaList;

                if (yeniKayitMi == 1)
                {
                    araList = new List<RaporAra>();
                    hesaplamaList = new List<RaporHesaplama>();

                }
                else
                {
                    araList = db.RaporAra.Where(x => x.raporID == id).ToList();
                    hesaplamaList = db.RaporHesaplama.Where(x => x.raporID == id).ToList();
                }

                for (int i = 0; i < data.hesaplamaList.Count; i++)
                {
                    if (yeniKayitMi == 1)
                    {
                        string orderNo = data.araList[i].orderNo;
                        string olcuTuruAd = data.araList[i].olcuTuruAd;
                        RaporAra araData = new RaporAra()
                        {
                            orderNo = data.araList[i].orderNo,
                            orderID = data.araList[i].orderID,
                            olcuTuruAd = data.araList[i].olcuTuruAd,
                            olcuTuruID = data.araList[i].olcuTuruID,
                            enBoyCekme = data.araList[i].enBoyCekme,
                            raporID = raporDetay.id,
                            kazanDetayID = data.araList[i].kazanDetayID,
                            raporTuru = data.araList[i].raporTuru,
                            receteKod = data.araList[i].receteKod,
                            beden = data.araList[i].beden

                        };

                        db.RaporAra.Add(araData);
                        db.SaveChanges();

                        for (int j = 0; j < data.hesaplamaList[i].Count; j++)
                        {
                            RaporHesaplama hesaplama = new RaporHesaplama()
                            {
                                olcuNoktaAd = data.hesaplamaList[i][j].olcuNoktaAd,
                                olcuNoktaID = data.hesaplamaList[i][j].olcuNoktaID,
                                raporAraID = araData.id,
                                raporID = raporDetay.id,
                                ortalamaDeger = Convert.ToDouble(data.hesaplamaList[i][j].ortalamaDeger),
                                gerceklesenCekmeTolerans = Convert.ToDouble(data.hesaplamaList[i][j].gerceklesenCekmeTolerans),
                                uygulananCekmeTolerans = Convert.ToDouble(data.hesaplamaList[i][j].uygulananCekmeTolerans),

                            };

                            db.RaporHesaplama.Add(hesaplama);
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        int raporAraId = data.araList[i].id;

                        RaporAra raporAra = db.RaporAra.Where(x => x.id == raporAraId).FirstOrDefault();

                        raporAra.orderNo = data.araList[i].orderNo;
                        raporAra.orderID = data.araList[i].orderID;
                        raporAra.olcuTuruAd = data.araList[i].olcuTuruAd;
                        raporAra.olcuTuruID = data.araList[i].olcuTuruID;
                        raporAra.enBoyCekme = data.araList[i].enBoyCekme;
                        raporAra.raporID = raporDetay.id;
                        raporAra.kazanDetayID = data.araList[i].kazanDetayID;
                        raporAra.raporTuru = data.araList[i].raporTuru;
                        raporAra.beden = data.araList[i].beden;
                        raporAra.enBoyCekme = data.araList[i].enBoyCekme;
                        raporAra.receteKod = data.araList[i].receteKod;

                        db.SaveChanges();
                    }
                }

                for (int i = 0; i < data.ortalamaList.Count; i++)
                {
                    if(yeniKayitMi == 1)
                    {
                        string olcuNoktaAd = data.ortalamaList[i].olcuNoktaAd;
                        RaporOrtalama ortalama = new RaporOrtalama()
                        {
                            raporID = raporDetay.id,
                            olcuNoktaAd = olcuNoktaAd,
                            olcuNoktaID = data.ortalamaList[i].olcuNoktaID,
                            ortGerceklesenCekmeTolerans = Convert.ToDouble(data.ortalamaList[i].ortGerceklesenCekmeTolerans),
                            kararVerilenCekmeTolerans = Convert.ToDouble(data.ortalamaList[i].kararVerilenCekmetolerans),
                            ortUygulananCekmeTolerans = Convert.ToDouble(data.ortalamaList[i].ortUygulananCekmeTolerans),
                            ortOrtalamaDeger = Convert.ToDouble(data.ortalamaList[i].ortOrtalamaDeger),
                            aciklama = data.ortalamaList[i].aciklama

                        };
                        db.RaporOrtalama.Add(ortalama);
                    }
                    else
                    {
                        int raporOrtalamaId = data.ortalamaList[i].id;
                        string olcuNoktaAd = data.ortalamaList[i].olcuNoktaAd;

                        RaporOrtalama ortalama = db.RaporOrtalama.Where(x => x.id == raporOrtalamaId).FirstOrDefault();
                        ortalama.raporID = raporDetay.id;
                        ortalama.olcuNoktaAd = olcuNoktaAd;
                        ortalama.olcuNoktaID = data.ortalamaList[i].olcuNoktaID;
                        ortalama.ortGerceklesenCekmeTolerans = Convert.ToDouble(data.ortalamaList[i].ortGerceklesenCekmeTolerans);
                        ortalama.kararVerilenCekmeTolerans = Convert.ToDouble(data.ortalamaList[i].kararVerilenCekmetolerans);
                        ortalama.ortUygulananCekmeTolerans = Convert.ToDouble(data.ortalamaList[i].ortUygulananCekmeTolerans);
                        ortalama.ortOrtalamaDeger = Convert.ToDouble(data.ortalamaList[i].ortOrtalamaDeger);
                        ortalama.aciklama = data.ortalamaList[i].aciklama;

                        db.SaveChanges();
                    }

                   

                    if (data.anaCekmeList != null)
                    {
                        RaporAnaCekme anaCekme = new RaporAnaCekme()
                        {
                            RaporId = raporDetay.id,
                            OlcuNoktaId = data.ortalamaList[i].olcuNoktaID,
                            OlcuNoktaAd = data.ortalamaList[i].olcuNoktaAd,
                            AnaCekmeCekmeTolerans = Convert.ToDouble(data.anaCekmeList[i].AnaCekmeCekmeTolerans)
                        };
                        db.RaporAnaCekme.Add(anaCekme);
                    }

                }

                if (data.bagliOrderList != null)
                {
                    foreach (var item in data.bagliOrderList)
                    {
                        RaporBagliOrder bagliOrder = new RaporBagliOrder()
                        {
                            raporId = raporDetay.id,
                            orderId = item.OrderId,
                            orderNo = item.OrderNo

                        };

                        db.RaporBagliOrder.Add(bagliOrder);
                    }
                }
                else if (orderId != 0)
                {
                    string orderNo = db.Order.Where(x => x.ID == orderId).Select(x => x.orderNo).FirstOrDefault();

                    RaporBagliOrder bagliOrder = new RaporBagliOrder()
                    {
                        raporId = raporDetay.id,
                        orderId = orderId,
                        orderNo = orderNo
                    };

                    db.RaporBagliOrder.Add(bagliOrder);
                }

                db.SaveChanges();


                return raporDetay;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Order> GetOrders(Expression<Func<Order, bool>> expression = null)
        {
            return expression == null ? db.Order.ToList() : db.Order.Where(expression).ToList();
        }

        public Order GetSingleOrder(Expression<Func<Order, bool>> expression)
        {
            return db.Order.Where(expression).FirstOrDefault();
        }

        public string GetMaxCekme(int kazanDetayId)
        {
            string enBoyCekme = "";

            var kazanDetay = db.KazanDetay.Where(x => x.id == kazanDetayId).FirstOrDefault();

            int tabloTuru = kazanDetay.tabloTuru;

            int kazanDetayIdYo = 0;

            if (tabloTuru == 1)
            {
                string orderNoGelen = kazanDetay.Order.orderNo;
                int olcuTuruID = kazanDetay.olcuTuruID;


                if (olcuTuruID == 18 || olcuTuruID == 33 || olcuTuruID == 19 || olcuTuruID == 32 || olcuTuruID == 31)
                {
                    kazanDetayIdYo = db.KazanDetay.Where(x => x.Order.orderNo == orderNoGelen && x.olcuTuruID == 17 && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();

                    if (kazanDetayIdYo == 0)
                    {
                        kazanDetayIdYo = db.KazanDetay.Where(x => x.Order.orderNo == orderNoGelen && x.olcuTuruID == 16 && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();
                    }
                }
                else
                {
                    kazanDetayIdYo = db.KazanDetay.Where(x => x.Order.orderNo == orderNoGelen && x.olcuTuruID == olcuTuruID && x.tabloTuru == 0 && x.durum == true).Select(x => x.id).FirstOrDefault();
                }
            }
            else
            {
                kazanDetayIdYo = kazanDetayId;
            }

            SqlParameter paramKazanDetay = new SqlParameter("@kazanDetayID", kazanDetayIdYo);
            var cmdKazanDetay = db.Database.Connection.CreateCommand();

            cmdKazanDetay.CommandText = "[dbo].[MaxCekmeGetir] @kazanDetayID";
            cmdKazanDetay.Parameters.Add(paramKazanDetay);

            db.Database.Connection.Open();

            var readerKazan = cmdKazanDetay.ExecuteReader();

            if (readerKazan.HasRows)
            {
                readerKazan.Read();
                if (!readerKazan.IsDBNull(0))
                {
                    enBoyCekme = readerKazan.GetString(0);
                }
            }
            readerKazan.Close();
            db.Database.Connection.Close();

            return enBoyCekme;
        }

        public TabloDTOModel GetTable(int id, int tur, string bedenAd = "")
        {
            TabloDTOModel table = new TabloDTOModel();
            table.tur = tur;

            if (tur == 0)
            {
                KazanDetay DbKazan = db.KazanDetay.Where(x => x.id == id).FirstOrDefault();
                List<KazanAra> dbKazanAraList = db.KazanAra.Where(x=>x.kazanDetayID == DbKazan.id).ToList();

                List<int?> pnoList = dbKazanAraList.Select(x => x.pantNo).Distinct().ToList();
                List<KazanAra> headerList = new List<KazanAra>();

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

                List<KazanHesaplama> kazanHesaplamas = kazanController.GerceklesenUygulananCekmeTolAlmaForController(id);

                table.headerList = headerList;
                table.GerceklesenUygulananCekmeTol = kazanHesaplamas;
                table.dbKazanAraList = dbKazanAraList;

                table.kazanDetay = DbKazan;

                if (DbKazan.atolyeID != 0)
                {
                    table.atolyeAdList = newController.getAtolyes().Where(x => x.Id == DbKazan.atolyeID).FirstOrDefault().AtolyeAd;

                }
                else
                {
                    table.atolyeAdList = "";

                }


                if (DbKazan.utuPaketID != 0)
                {
                    table.utuPaketAdList = newController.getUtuPakets().Where(x => x.Id == DbKazan.utuPaketID).FirstOrDefault().UtuPaketAd;
                }
                else
                {
                    table.utuPaketAdList = "";
                }


                table.fasonMuList = (DbKazan.OlcuTurleri.olcuTuruAd == "Fason Ham Ölçü" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Ön Final" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Son Final" || DbKazan.OlcuTurleri.olcuTuruAd == "Ütü Paket Sevk");

            }
            else if (tur == 1)
            {
                //
                List<DTOImalatTabloDetay> detayList = new List<DTOImalatTabloDetay>();
                List<List<DTOImalatTabloGoster>> tablo = new List<List<DTOImalatTabloGoster>>();
                List<DTOImalatTabloGoster> araDTOList = new List<DTOImalatTabloGoster>();

                //
                OlcuTabloDetay detay = db.OlcuTabloDetay.Where(x => x.id == id).FirstOrDefault();
                List<OlcuTabloAra> araList = db.OlcuTabloAra.Where(x => x.olcuTabloID == id).ToList();
                List<OlcuTabloHesaplama> hesapList = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == id).ToList();


                List<string> bedenler = new List<string>();
                List<int> bedenid = db.OlcuTabloAra.Where(x => x.olcuTabloID == id).Select(x => x.bedenID).Distinct().ToList();

                int anaBedenID = detay.anaBedenID;

                string anaBeden = db.Bedenler.Where(x => x.ID == anaBedenID).Select(x => x.beden).FirstOrDefault();

                DTOImalatTabloDetay detayDTO = new DTOImalatTabloDetay()
                {
                    id = detay.id,
                    orderID = detay.orderID,
                    orderNo = detay.Order.orderNo,
                    musteri = detay.Order.musteri,
                    aciklama = detay.aciklama,
                    anaBeden = anaBeden,
                    artikel = detay.Order.modelNo + "-" + detay.Order.modelAd,
                    enBoyCekme = detay.enBoyCekme,
                    tabloTuru = detay.tabloTuru,
                    giysiTuru = detay.GiysiTurleri.giysiTuruAd,
                    olcuTUru = detay.OlcuTurleri.olcuTuruAd,
                    tarih = (DateTime)detay.tarih,
                    kullaniciID = detay.kullaniciID,
                    kullanıcıAdı = detay.PersonelTablo.personelAd,
                    kumas = detay.Order.kumas,
                    tarihString = detay.tarih.ToString(),
                    modelAd = detay.Order.modelAd,
                    modelNo = detay.Order.modelNo,
                };

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
                foreach (var item in araList.GroupBy(x => x.olcuNoktaID).Select(x => x.FirstOrDefault()).OrderBy(x => x.id).ToList())
                {
                    DTOImalatTabloGoster araDTO = new DTOImalatTabloGoster();
                    araDTO.id = item.id;
                    int olcuNoktasiID = item.olcuNoktaID;
                    araDTO.olcuNoktasi = item.OlcuNoktalari.olcuNoktasi;
                    int bedenID = item.bedenID;
                    araDTO.beden = bedenler;
                    araDTO.deger = db.OlcuTabloAra.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.deger.ToString()).ToList();
                    araDTO.tolerans = hesapList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.tolerans.ToString()).FirstOrDefault();
                    araDTO.cekme = hesapList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.cekme.ToString()).FirstOrDefault();
                    araDTO.oran = hesapList.Where(x => x.olcuNoktaID == olcuNoktasiID && x.olcuTabloID == id).Select(x => x.oran.ToString()).FirstOrDefault();
                    araDTOList.Add(araDTO);
                }

                tablo.Add(araDTOList);
                detayList.Add(detayDTO);

                table.dtoAra = tablo;
                table.dtoDetay = detayList;
                table.anaBedenList = anaBeden;
                table.kullanıcıAdSoyadList = detay.PersonelTablo.personelAd + " " + detay.PersonelTablo.personelSoyad;
                table.tabloTuruList = (int)detay.tabloTuru;
            }
            else if (tur == 2)
            {
                NumuneDetay numuneDetay = db.NumuneDetay.Where(x => x.id == id).FirstOrDefault();

                List<NumuneAra> numuneAras = db.NumuneAra.Where(x => x.numuneDetayID == id && x.Bedenler.beden == bedenAd).OrderBy(x=>x.id).ToList();

                List<DTOKaliteAtolye> atolyeList = newController.getAtolyes();

                int? atolyeId = numuneDetay.atolyeID;
                string atolyeAd = atolyeList.Where(x => x.Id == atolyeId).Select(x => x.AtolyeAd).FirstOrDefault();

                DTONumuneDetay dtoNumuneDetay = new DTONumuneDetay()
                {
                    Aciklama = numuneDetay.aciklama,
                    aktarimMi = numuneDetay.aktarimMi,
                    AtolyeAd = atolyeAd,
                    Id = numuneDetay.id,
                    AtolyeId = numuneDetay.atolyeID,
                    BagliOrderId = numuneDetay.bagliOrderId,
                    DikimNedeni = numuneDetay.dikimNedeni,
                    ElliListe = numuneDetay.elliListe,
                    ElliNumune = numuneDetay.elliNumune,
                    HasPreviousOrder = numuneDetay.hasPreviousOrder,
                    KalipAdi = numuneDetay.kalipAdi,
                    KID = numuneDetay.KID,
                    KullaniciAd = numuneDetay.PersonelTablo.personelAd,
                    KullaniciId = numuneDetay.kullaniciID,
                    Kumas = numuneDetay.Order.kumas,
                    ModelAd = numuneDetay.Order.modelAd,
                    Musteri = numuneDetay.Order.musteri,
                    OlcumNo = numuneDetay.olcumNo,
                    OlcuTuruAd = numuneDetay.OlcuTurleri.olcuTuruAd,
                    OlcuTuruId = numuneDetay.olcuTuruID,
                    OrderId = numuneDetay.orderID,
                    OrderNo = numuneDetay.Order.orderNo,
                    ReceteKod = numuneDetay.receteKod,
                    tabloTuru = numuneDetay.tabloTuru,
                    Tarih = numuneDetay.tarih,
                    TopNo = numuneDetay.topNo,
                    UpdatedDate = numuneDetay.updatedDate,
                    YikamadanGelis = numuneDetay.yikamadanGelis,
                    YikamayaGidis = numuneDetay.yikamayaGidis
                };

                List<DtoNumuneAra> dtoNumuneAras = new List<DtoNumuneAra>();

                foreach (var item in numuneAras)
                {
                    DtoNumuneAra dtoNumuneAra = new DtoNumuneAra()
                    {
                        BedenAd = item.Bedenler.beden,
                        BedenId = item.bedenID,
                        GerceklesenCekme = (double)item.gerceklesenCekme,
                        GerceklesenTolerans = (double)item.gerceklesenTolerans,
                        Id = item.id,
                        KalipOlcusu = (double)item.kalipOlcusu,
                        MudahaleMi = item.mudahaleMi,
                        NumuneDetayId = item.numuneDetayID,
                        OlcuNoktaAd = item.OlcuNoktalari.olcuNoktasi,
                        OlcuNoktaId = item.olcuNoktaID,
                        UygulananCekme = (double)item.uygulananCekme,
                        UygulananTolerans = (double)item.uygulananTolerans,
                        YikamaOncesiFark = (double)item.yikamaOncesiFark,
                        YikamaOncesiOlculen = (double)item.yikamaOncesiOlculen,
                        YikamaOncesiTablo = (double)item.yikamaOncesiTablo,
                        YikamaSonrasiFark = (double)item.yikamaSonrasiFark,
                        YikamaSonrasiOlculen = (double)item.yikamaSonrasiOlculen,
                        YikamaSonrasiTablo = (double)item.yikamaSonrasiTablo
                    };

                    dtoNumuneAras.Add(dtoNumuneAra);
                }

                table.DtoNumuneDetay = dtoNumuneDetay;
                table.dtoNumuneAraList = dtoNumuneAras;

            }
            return table;
        }

        public List<KazanDetayModel> GetKazanDetayModel(GelismisRaporlamaParameters parameters)
        {
            List<KazanDetayModel> kazanDetayModels = new List<KazanDetayModel>();


            int isFirstTime = 0;

            if ((parameters.OrderNo == "" || parameters.OrderNo == null) && (parameters.ModelNo == "" || parameters.ModelNo == null) && (parameters.Kumas == "" || parameters.Kumas == null) && (parameters.Musteri == "" || parameters.Musteri == null) && (parameters.Atolye == "" || parameters.Atolye == null) && (parameters.ModelAdi == "" || parameters.ModelAdi == null) && (parameters.YikamaYeri == "" || parameters.YikamaYeri == null) && (parameters.StartDate == "" || parameters.StartDate == null) && (parameters.EndDate == "" || parameters.EndDate == null) && parameters.ReceteDetail.receteActive == 0)
            {
                isFirstTime = 1;

            }
            else
            {
                isFirstTime = 0;
            }


            string query = QueryBuilder(parameters);

            //kazanDetayModels = db.Database.SqlQuery<KazanDetayModel>(@"exec KarsilastirmaRaporProcLast @orderNo,@modelNo,@kumas,@musteri,@modelAd,@yikamaYeri,@atolye,@tabloTuru,@tarih1,@tarih2,@isFirstTime", DenemeParameters).ToList();

            kazanDetayModels = db.Database.SqlQuery<KazanDetayModel>(@"" + query + "").ToList();

            if (parameters.ReceteDetail.receteActive != 0)
            {
                List<string> closestRecipeOrders = FindClosestRecipes(parameters.ReceteDetail);

                kazanDetayModels = kazanDetayModels.Where(x => closestRecipeOrders.Contains(x.orderNo.Split('-')[0])).ToList();
            }

            return kazanDetayModels;
        }

        private string QueryBuilder(GelismisRaporlamaParameters parameters)
        {
            string query = "";

            int isFirstTime = 0;

            if ((parameters.OrderNo == "" || parameters.OrderNo == null) && (parameters.ModelNo == "" || parameters.ModelNo == null) && (parameters.Kumas == "" || parameters.Kumas == null) && (parameters.Musteri == "" || parameters.Musteri == null) && (parameters.Atolye == "" || parameters.Atolye == null) && (parameters.ModelAdi == "" || parameters.ModelAdi == null) && (parameters.YikamaYeri == "" || parameters.YikamaYeri == null) && (parameters.StartDate == "" || parameters.StartDate == null) && (parameters.EndDate == "" || parameters.EndDate == null) && parameters.ReceteDetail.receteActive == 0)
            {
                isFirstTime = 1;

            }
            else
            {
                isFirstTime = 0;
            }

            string valueIsFirstTime = "";
            if (isFirstTime != 0)
            {
                valueIsFirstTime = "TOP 50";
            }

            string whereParameters = "";
            string whereParametersNumune = "";

            if (!String.IsNullOrEmpty(parameters.OrderNo))
            {
                //whereParameters += "o.orderNo in (" + parameters.OrderNo + ") and ";
                //whereParametersNumune += "o.orderNo in (" + parameters.OrderNo + ") and ";

                whereParameters += "o.orderNo like '%" + parameters.OrderNo + "%' and ";
                whereParametersNumune += "o.orderNo like  '%" + parameters.OrderNo + "%' and ";
            }

            if (!String.IsNullOrEmpty(parameters.ModelNo))
            {
                whereParameters += "o.modelNo like '%" + parameters.ModelNo + "%' and ";
                whereParametersNumune += "o.modelNo like '%" + parameters.ModelNo + "%' and ";

            }

            if (!String.IsNullOrEmpty(parameters.Kumas))
            {
                //whereParameters += "o.kumas in (" + parameters.Kumas + ") and ";
                //whereParametersNumune += "o.kumas in (" + parameters.Kumas + ") and ";

                whereParameters += "o.kumas like '%" + parameters.Kumas + "%' and ";
                whereParametersNumune += "o.kumas like '%" + parameters.Kumas + "%' and ";
            }

            if (!String.IsNullOrEmpty(parameters.Musteri))
            {
                //whereParameters += "o.musteri in (" + parameters.Musteri + ") and ";
                //whereParametersNumune += "o.musteri in (" + parameters.Musteri + ") and ";

                whereParameters += "o.musteri like '%" + parameters.Musteri + "%' and ";
                whereParametersNumune += "o.musteri like '%" + parameters.Musteri + "%' and ";
            }


            if (!String.IsNullOrEmpty(parameters.ModelAdi))
            {
                //whereParameters += "o.modelAd in (" + parameters.ModelAdi + ") and ";
                //whereParametersNumune += "o.modelAd in (" + parameters.ModelAdi + ") and ";

                whereParameters += "o.modelAd like '%" + parameters.ModelAdi + "%' and ";
                whereParametersNumune += "o.modelAd like '%" + parameters.ModelAdi + "%' and ";
            }

            if (!String.IsNullOrEmpty(parameters.YikamaYeri))
            {
                whereParameters += "kd.yikamaYeri like '%" + parameters.YikamaYeri + "%' and ";
                whereParametersNumune += "nd.yikamaYeri like '%" + parameters.YikamaYeri + "%' and ";

            }

            if (!String.IsNullOrEmpty(parameters.Atolye))
            {
                //whereParameters += "a.atolyeAd in (" + parameters.Atolye + ") and ";
                //whereParametersNumune += "a.atolyeAd in (" + parameters.Atolye + ") and ";

                whereParameters += "a.atolyeAd like '%" + parameters.Atolye + "%' and ";
                whereParametersNumune += "a.atolyeAd like '%" + parameters.Atolye + "%' and ";

            }

            if (parameters.OlcuTuruParameter.KazanList != null || parameters.OlcuTuruParameter.NumuneList != null)
            {
                if (parameters.OlcuTuruParameter.KazanList != null)
                {
                    if (parameters.OlcuTuruParameter.KazanList.Count > 0)
                    {
                        string olcuTurleriList = string.Join<int>(",", parameters.OlcuTuruParameter.KazanList);

                        whereParameters += " kd.olcuTuruID in (" + olcuTurleriList + ") and ";

                    }
                }

                if (parameters.OlcuTuruParameter.NumuneList != null)
                {
                    if (parameters.OlcuTuruParameter.NumuneList.Count > 0)
                    {
                        string olcuTurleriList = string.Join<int>(",", parameters.OlcuTuruParameter.NumuneList);

                        whereParametersNumune += " nd.olcuTuruID in (" + olcuTurleriList + ") and ";

                    }
                }
            }

            DateTime gelenTarih1 = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime gelenTarih2 = DateTime.Now;

            if (!string.IsNullOrEmpty(parameters.StartDate))
            {
                gelenTarih1 = Convert.ToDateTime(parameters.StartDate);

                whereParameters += "kd.tarih > '" + gelenTarih1.Date.ToString("yyyy-MM-dd HH:mm:ss") + "' and ";
                whereParametersNumune += "nd.tarih > '" + gelenTarih1.Date.ToString("yyyy-MM-dd HH:mm:ss") + "' and ";

            }

            if (!string.IsNullOrEmpty(parameters.EndDate))
            {
                gelenTarih2 = Convert.ToDateTime(parameters.EndDate);


                whereParameters += "kd.tarih < '" + gelenTarih2.Date.ToString("yyyy-MM-dd HH:mm:ss") + "' and ";
                whereParametersNumune += "nd.tarih < '" + gelenTarih2.Date.ToString("yyyy-MM-dd HH:mm:ss") + "' and ";
            }


            if (parameters.TabloTuru == 0)
            {
                if (parameters.OlcuTuruParameter.KazanList != null && parameters.OlcuTuruParameter.NumuneList == null)
                {
                    if(isFirstTime == 1)
                    {
                        valueIsFirstTime = "TOP 100";
                    }

                    query = "select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',yikamaYeri,olcuTuruAd as 'olcuTuru',kd.id as 'kazanDetayID',CAST(CASE WHEN kd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN kd.tabloTuru = 1 THEN 'Yıkama Sonrası'" +
               "WHEN kd.tabloTuru = 2 THEN 'Zemin' WHEN kd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar) as tabloTuru,tarih,0 as 'raporTuru','' as receteKod from KazanDetay kd join[Order] o on  kd.orderID = o.ID join Dikimhane.dbo.Atolyes a on kd.atolyeID = a.Id " +
               "join OlcuTurleri ot on kd.olcuTuruID = ot.id " +
               "where " + whereParameters + " kd.tabloTuru = 0 and kd.durum = 1 order by tarih desc ";
                }
                else if(parameters.OlcuTuruParameter.NumuneList != null && parameters.OlcuTuruParameter.KazanList == null)
                {
                    if (isFirstTime == 1)
                    {
                        valueIsFirstTime = "TOP 100";
                    }


                    query = "select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',nd.yikamaYeri,olcuTuruAd as 'olcuTuru',nd.id as 'kazanDetayID'," +
                "CAST(CASE WHEN nd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN nd.tabloTuru = 1 THEN 'Yıkama Sonrası'WHEN nd.tabloTuru = 2 THEN 'Zemin' WHEN nd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar)" +
                ",tarih,1 as 'raporTuru',receteKod from NumuneDetay nd  join [Order] o on  nd.orderID=o.ID join Dikimhane.dbo.Atolyes a on nd.atolyeID = a.Id " +
                "join OlcuTurleri ot on nd.olcuTuruID = ot.id " +
                "where " + whereParametersNumune + " (nd.tabloTuru = 0 or nd.tabloTuru = 1) order by tarih desc";
                }
                else
                {
                    query = "select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',yikamaYeri,olcuTuruAd as 'olcuTuru',kd.id as 'kazanDetayID',CAST(CASE WHEN kd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN kd.tabloTuru = 1 THEN 'Yıkama Sonrası'" +
                                   "WHEN kd.tabloTuru = 2 THEN 'Zemin' WHEN kd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar) as tabloTuru,tarih,0 as 'raporTuru','' as receteKod from KazanDetay kd join[Order] o on  kd.orderID = o.ID join Dikimhane.dbo.Atolyes a on kd.atolyeID = a.Id " +
                                   "join OlcuTurleri ot on kd.olcuTuruID = ot.id " +
                                   "where " + whereParameters + " kd.tabloTuru = 0 and kd.durum = 1 " +
                                   " UNION " +
                                   "select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',nd.yikamaYeri,olcuTuruAd as 'olcuTuru',nd.id as 'kazanDetayID'," +
                                   "CAST(CASE WHEN nd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN nd.tabloTuru = 1 THEN 'Yıkama Sonrası'WHEN nd.tabloTuru = 2 THEN 'Zemin' WHEN nd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar)" +
                                   ",tarih,1 as 'raporTuru',receteKod from NumuneDetay nd  join [Order] o on  nd.orderID=o.ID join Dikimhane.dbo.Atolyes a on nd.atolyeID = a.Id " +
                                   "join OlcuTurleri ot on nd.olcuTuruID = ot.id " +
                                   "where " + whereParametersNumune + " (nd.tabloTuru = 0 or nd.tabloTuru = 1) order by tarih desc";
                }           
            }
            else
            {
                if (parameters.OlcuTuruParameter.KazanList != null && parameters.OlcuTuruParameter.NumuneList == null)
                {
                    if (isFirstTime == 1)
                    {
                        valueIsFirstTime = "TOP 100";
                    }


                    query = "select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',yikamaYeri,olcuTuruAd as 'olcuTuru',kd.id as 'kazanDetayID',CAST(CASE WHEN kd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN kd.tabloTuru = 1 THEN 'Yıkama Sonrası'" +
               "WHEN kd.tabloTuru = 2 THEN 'Zemin' WHEN kd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar) as tabloTuru,tarih,0 as 'raporTuru','' as receteKod from KazanDetay kd join[Order] o on  kd.orderID = o.ID join Dikimhane.dbo.Atolyes a on kd.atolyeID = a.Id " +
               "join OlcuTurleri ot on kd.olcuTuruID = ot.id " +
               "where " + whereParameters + " (kd.tabloTuru = 1 or kd.tabloTuru = 2 or kd.tabloTuru = 3) and kd.durum = 1 order by tarih desc";
                }
                else if (parameters.OlcuTuruParameter.NumuneList != null && parameters.OlcuTuruParameter.KazanList == null)
                {
                    if (isFirstTime == 1)
                    {
                        valueIsFirstTime = "TOP 100";
                    }


                    query = " select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',nd.yikamaYeri,olcuTuruAd as 'olcuTuru',nd.id as 'kazanDetayID',CAST(CASE WHEN nd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN nd.tabloTuru = 1 THEN 'Yıkama Sonrası'WHEN nd.tabloTuru = 2 THEN 'Zemin' WHEN nd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar)" +
               ",tarih,1 as 'raporTuru',receteKod from NumuneDetay nd  join [Order] o on  nd.orderID=o.ID join Dikimhane.dbo.Atolyes a on nd.atolyeID = a.Id " +
               " join OlcuTurleri ot on nd.olcuTuruID = ot.id " +
               " where " + whereParametersNumune + " (nd.tabloTuru = 1 or nd.tabloTuru = 2 or nd.tabloTuru = 3) order by tarih desc";
                }
                else
                {
                    query = "select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',yikamaYeri,olcuTuruAd as 'olcuTuru',kd.id as 'kazanDetayID',CAST(CASE WHEN kd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN kd.tabloTuru = 1 THEN 'Yıkama Sonrası'" +
              "WHEN kd.tabloTuru = 2 THEN 'Zemin' WHEN kd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar) as tabloTuru,tarih,0 as 'raporTuru','' as receteKod from KazanDetay kd join[Order] o on  kd.orderID = o.ID join Dikimhane.dbo.Atolyes a on kd.atolyeID = a.Id " +
              "join OlcuTurleri ot on kd.olcuTuruID = ot.id " +
              "where " + whereParameters + " (kd.tabloTuru = 1 or kd.tabloTuru = 2 or kd.tabloTuru = 3) and kd.durum = 1 " +
              " UNION " +
              " select " + valueIsFirstTime + " orderNo,modelNo,kumas as 'kumasAdi',musteri,AtolyeAd as atolyeAd,modelAd as 'modelAdi',nd.yikamaYeri,olcuTuruAd as 'olcuTuru',nd.id as 'kazanDetayID',CAST(CASE WHEN nd.tabloTuru = 0 THEN 'Yıkama Öncesi' WHEN nd.tabloTuru = 1 THEN 'Yıkama Sonrası'WHEN nd.tabloTuru = 2 THEN 'Zemin' WHEN nd.tabloTuru = 3 THEN 'Kimyasal Öncesi' end as nvarchar)" +
              ",tarih,1 as 'raporTuru',receteKod from NumuneDetay nd  join [Order] o on  nd.orderID=o.ID join Dikimhane.dbo.Atolyes a on nd.atolyeID = a.Id " +
              " join OlcuTurleri ot on nd.olcuTuruID = ot.id " +
              " where " + whereParametersNumune + " (nd.tabloTuru = 1 or nd.tabloTuru = 2 or nd.tabloTuru = 3) order by tarih desc";
                }             
            }
            return query;
        }

        public List<ArsivDataModel> GetArsivDataModel(string raporAdi = "", string orderNo = "", string modelNo = "", string kumas = "", string musteri = "", string atolye = "", string modelAdi = "", string yikamaYeri = "", string tarih1 = "", string tarih2 = "")
        {
            List<ArsivDataModel> data = new List<ArsivDataModel>();
            DateTime gelenTarih1 = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime gelenTarih2 = DateTime.Now;

            if (!string.IsNullOrEmpty(tarih1))
            {
                gelenTarih1 = Convert.ToDateTime(tarih1);
            }

            if (!string.IsNullOrEmpty(tarih2))
            {
                gelenTarih2 = Convert.ToDateTime(tarih2);

            }

            SqlParameter param0 = new SqlParameter("raporAdi", raporAdi);
            SqlParameter param1 = new SqlParameter("orderNo", orderNo);
            SqlParameter param2 = new SqlParameter("modelNo", modelNo);
            SqlParameter param3 = new SqlParameter("kumas", kumas);
            SqlParameter param4 = new SqlParameter("musteri", musteri);
            SqlParameter param7 = new SqlParameter("atolye", atolye);
            SqlParameter param5 = new SqlParameter("modelAd", modelAdi);
            SqlParameter param6 = new SqlParameter("yikamaYeri", yikamaYeri);
            //SqlParameter param9 = new SqlParameter("tarih1", gelenTarih1);
            //SqlParameter param10 = new SqlParameter("tarih2", "2021-03-12 10:18:06.737");
            //SqlParameter param11 = new SqlParameter();

            //if (raporAdi == "" && orderNo == "" && modelNo == "" && kumas == "" && musteri == "" && atolye == "" && modelAdi == "" && yikamaYeri == "" && tarih1 == "" && tarih2 == "")
            //{
            //    param11 = new SqlParameter("@bosparam", 1);
            //}
            //else
            //{
            //    param11 = new SqlParameter("@bosparam", 0);

            //}


            /*@tarih1, @tarih2*/
            var cmd2 = db.Database.Connection.CreateCommand();
            cmd2.CommandText = "exec [dbo].[ArsivRaporProc] @raporAdi, @orderNo, @modelNo, @kumas, @musteri, @atolye, @modelAd, @yikamaYeri ";
            cmd2.Parameters.Add(param0);
            cmd2.Parameters.Add(param2);
            cmd2.Parameters.Add(param3);
            cmd2.Parameters.Add(param4);
            cmd2.Parameters.Add(param1);
            cmd2.Parameters.Add(param7);
            cmd2.Parameters.Add(param5);
            cmd2.Parameters.Add(param6);
            //cmd2.Parameters.Add(param9);
            //cmd2.Parameters.Add(param10);
            //cmd2.Parameters.Add(param11);

            db.Database.Connection.Open();

            var reader = cmd2.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ArsivDataModel model = new ArsivDataModel()
                    {
                        raporID = reader.GetInt32(0),
                        raporAdi = reader.GetString(1),
                        tabloTuru = reader.GetInt32(2),
                        tarih = reader.GetDateTime(3),
                        personelAd = reader.GetString(4),
                        personelSoyad = reader.GetString(5)

                    };

                    data.Add(model);
                }
            }
            reader.Close();



            data = data.OrderByDescending(x => x.tarih).ToList();

            return data;
        }

        public List<string> FindClosestRecipes(ReceteDetailModel receteDetailModel)
        {
            List<string> closestOrders = new List<string>();

            var recipeSteps = receteDetailModel.recipeSteps;

            List<int?> recipeIds = new List<int?>();

            List<RecipeStepsDto> dbRecipeStepsList = new List<RecipeStepsDto>();

            bool isFirst = true;

            if (recipeSteps != null)
            {
                foreach (var item in recipeSteps)
                {
                    var recipeParameter = item.RecipeParameter;
                    int recipeMinValue = item.RecipeMinValue;
                    int recipeMaxvalue = item.RecipeMaxValue;

                    SqlParameter[] RecipeParameters = {
                new SqlParameter("@washingTypeParameter", recipeParameter),
                new SqlParameter("@timeParameterMin", recipeMinValue),
                new SqlParameter("@timeParameterMax", recipeMaxvalue)
            };

                    string query = @"SELECT [ID],[RECIPE_ID],[GUID],[TYPE]
      ,[PARAM_ID],[PARAM_NAME],[PERSONEL],[WASHING_DETAIL],[USER_ID],[USER_NAME],[MACHINE_ID],[MACHINE_NAME],[IS_HAVE_WASHING_TYPE],[WASHING_TYPE_NAME], CONVERT(FLOAT, REPLACE(REPLACE(REPLACE(TIME,',',''),'$',''),'SN',''))  as 'TIME'
      ,[TEMPERATURE],[LR],[SIKMA_SANIYESI],[KURUTMA_SICAKLIK],[KURUTMA_DAKIKA],[BOLGE],[TUR_SAYISI],[FIRIN_1_DERECE],[FIRIN_2_DERECE]
      ,[FIRIN_1_DAKIKA],[FIRIN_2_DAKIKA] ,[PROJE],[YAKMA_DERECESI] ,[YAKMA_DERECESI2],[YAKMA_SURESI],[DPI],[DPI2],[ZIMPARA_DERECESI],[SARJ_ADEDI],[PH],[HAVLU_MIKTARI],[HAVLU_BOYUT]
      ,[HAVLU_DONME_DK],[HAVLU_SIKMA_DK],[RANDOM_DK],[SIKMA_DEVRI],[KOVA_TAS_MIKTARI],[FILE_TIPI],[IP_TIPI],[CUVAL_TAS_MIKTARI],[PANTOLON_SIKMA_DK],[ACIKLAMA],[WASHING_TYPE_ID],[SUB_MACHINE_ID],[SUB_MACHINE_NAME]
      ,[IS_USING_PLANNING],[YURUTME_ADEDI],[HAM_AGIRLIK],[IS_FIRIN_3_DERECE],[FIRIN_3_DAKIKA],[LT_PANT_ADET],[SON_AGIRLIK],[KATLAMA_SAYISI],[KIVRILMA_SAYISI],[TEXTBOX_PARAMETER_ID],[BASKI_KUVVETI],[BASKI_SURESI]
      ,[TOPLAM_SURE],[SIKMA_SURE] ,[NEM],[SU] ,[GUC] ,[AGIRLIK],[AKIS_HIZI],[NBP],[SIFIR_TAS_MIKTARI],[ESKI_TAS_MIKTARI] ,[BOS_DONDURME],[ADET],[TIME2] FROM[u6872686_IM_SCH].[dbo].[RECIPE_STEPS]
where WASHING_TYPE_NAME = @washingTypeParameter and CONVERT(FLOAT, REPLACE(REPLACE(REPLACE(TIME,',',''),'$',''),'SN',''))  between @timeParameterMin and @timeParameterMax";

                    //List<RECIPE_STEPS> tempRecipeLists = dbRecipe.RECIPE_STEPS.Where(x => x.WASHING_TYPE_NAME == recipeParameter && (x.TIME >= recipeMinValue && x.TIME <= recipeMaxvalue)).ToList();
                    List<RecipeStepsDto> tempRecipeLists = dbRecipe.Database.SqlQuery<RecipeStepsDto>(query, RecipeParameters).ToList();

                    if (isFirst)
                    {
                        dbRecipeStepsList = tempRecipeLists;
                        isFirst = false;
                    }
                    else
                    {
                        var recipeIdList = dbRecipeStepsList.Select(x => x.RECIPE_ID).Distinct().ToList();
                        dbRecipeStepsList = tempRecipeLists.Where(x => x.WASHING_TYPE_NAME == recipeParameter && (x.TIME >= recipeMinValue && x.TIME <= recipeMaxvalue) && recipeIdList.Contains(x.RECIPE_ID)).ToList();
                    }
                }

                recipeIds = dbRecipeStepsList.Select(x => x.RECIPE_ID).Distinct().ToList();

                closestOrders = dbRecipe.RECIPE.Where(x => recipeIds.Contains(x.ID)).Select(x => x.ORDER_NO_YEDEK).Distinct().ToList();
            }



            return closestOrders;
        }

        public List<KumasKarakteriDto> FindClosestFabrics(KumasDetailModel kumasDetailModel)
        {
            List<KumasKarakteriDto> kumasKarakteris = db.KumasKarakteri.Select(x => new KumasKarakteriDto
            {
                id = x.id,
                kumasAdı = x.kumasAdı,
                elastikiyetAtkiMax = x.elastikiyetAtkiMax,
                elastikiyetAtkiMin = x.elastikiyetAtkiMin,
                elastikiyetAtkiOrtalama = x.elastikiyetAtkiOrtalama,
                elastikiyetCozguMax = x.elastikiyetCozguMax,
                elastikiyetCozguMin = x.elastikiyetCozguMin,
                elastikiyetCozguOrtalama = x.elastikiyetCozguOrtalama,
                kesilebilirEnMax = x.kesilebilirEnMax,
                kesilebilirEnMin = x.kesilebilirEnMin,
                kesilebilirEnOrtalama = x.kesilebilirEnOrtalama,
                konstrüksiyon = x.konstrüksiyon,
                konstrüksiyonDeger1 = x.konstrüksiyonDeger1,
                konstrüksiyonDeger2 = x.konstrüksiyonDeger2,
                konstrüksiyonDeger3 = x.konstrüksiyonDeger3,
                konstrüksiyonDeger4 = x.konstrüksiyonDeger4,
                konstrüksiyonDeger5 = x.konstrüksiyonDeger5,
                konstrüksiyonDeger6 = x.konstrüksiyonDeger6,
                konstrüksiyonParametre1 = x.konstrüksiyonParametre1,
                konstrüksiyonParametre2 = x.konstrüksiyonParametre2,
                konstrüksiyonParametre3 = x.konstrüksiyonParametre3,
                konstrüksiyonParametre4 = x.konstrüksiyonParametre4,
                konstrüksiyonParametre5 = x.konstrüksiyonParametre5,
                konstrüksiyonParametre6 = x.konstrüksiyonParametre6,
                kumasEnCekmeMin = x.kumasEnCekmeMin,
                kumasEnCekmeMax = x.kumasEnCekmeMax,
                kumasEnCekmeOrtalama = x.kumasEnCekmeOrtalama,
                kumasBoyCekmeMin = x.kumasBoyCekmeMin,
                kumasBoyCekmeMax = x.kumasBoyCekmeMax,
                kumasBoyCekmeOrtalama = x.kumasBoyCekmeOrtalama,
                kumasKodu = x.kumasKodu,
                tamEnMax = x.tamEnMax,
                tamEnMin = x.tamEnMin,
                tamEnOrtalama = x.tamEnOrtalama
            }).ToList();

            List<string> kumasList = GetOrders().Select(x => x.kumas).ToList();

            // İlk başta kumaş konstrüksiyonunu filtreliyorum
            List<KumasDetailModel.Konstruksiyon> konstruksiyons = kumasDetailModel.konstruksiyonList;

            if (konstruksiyons != null)
            {
                //bool isFirst = true;
                //foreach (var item in konstruksiyons)
                //{
                //    var konstruksiyonParameter = item.konstruksiyonParameter;
                //    var konstruksiyonValue = item.konstruksiyonValue;
                //    if (isFirst)
                //    {
                //        kumasKarakteris = kumasKarakteris.Where(x => (x.konstrüksiyonParametre1 == konstruksiyonParameter && x.konstrüksiyonDeger1 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre2 == konstruksiyonParameter && x.konstrüksiyonDeger2 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre3 == konstruksiyonParameter && x.konstrüksiyonDeger3 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre4 == konstruksiyonParameter && x.konstrüksiyonDeger4 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre5 == konstruksiyonParameter && x.konstrüksiyonDeger5 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre6 == konstruksiyonParameter && x.konstrüksiyonDeger6 == konstruksiyonValue)).ToList();

                //        isFirst = false;
                //    }
                //    else
                //    {
                //        kumasKarakteris = kumasKarakteris.Where(x =>
                //        (x.konstrüksiyonParametre1 == konstruksiyonParameter && x.konstrüksiyonDeger1 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre2 == konstruksiyonParameter && x.konstrüksiyonDeger2 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre3 == konstruksiyonParameter && x.konstrüksiyonDeger3 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre4 == konstruksiyonParameter && x.konstrüksiyonDeger4 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre5 == konstruksiyonParameter && x.konstrüksiyonDeger5 == konstruksiyonValue)
                //            || (x.konstrüksiyonParametre6 == konstruksiyonParameter && x.konstrüksiyonDeger6 == konstruksiyonValue)).ToList();
                //    }
                //}
            }

            //Kalan parametrelere göre filtreliyorum.

            if (kumasDetailModel.kumasEnMin != 0 && kumasDetailModel.kumasEniMax != 0)
            {
                kumasKarakteris = kumasKarakteris.Where(x => x.kesilebilirEnOrtalama >= (kumasDetailModel.kumasEnMin) && x.kesilebilirEnOrtalama <= (kumasDetailModel.kumasEniMax)).ToList();
            }


            if (kumasDetailModel.kumasStrechMax != 0 && kumasDetailModel.kumasStrechMin != 0)
            {
                kumasKarakteris = kumasKarakteris.Where(x => x.elastikiyetAtkiOrtalama >= (kumasDetailModel.kumasStrechMin) && x.elastikiyetAtkiOrtalama <= (kumasDetailModel.kumasStrechMax)).ToList();
            }

            List<KumasKarakteriDto> kumasKarakteriDtoList = new List<KumasKarakteriDto>();

            //if (kumasDetailModel.kumasEnCekmeMax != 0 && kumasDetailModel.kumasEnCekmeMin != 0)
            //{
            //    if (kumasDetailModel.kumasBoyCekmeMax != 0 && kumasDetailModel.kumasBoyCekmeMin != 0)
            //    {
            //        int fark = Convert.ToInt32((float)kumasDetailModel.kumasEnCekmeMax - (float)kumasDetailModel.kumasEnCekmeMin);

            //        for (int i = 0; i < fark + 1; i++)
            //        {
            //            float enCekme = kumasDetailModel.kumasEnCekmeMin + i;
            //            float boyCekme = kumasDetailModel.kumasBoyCekmeMin + i;

            //            kumasKarakteriDtoList.AddRange(kumasKarakteris.Where(x => x.kumasEnCekmeOrtalama == enCekme && x.kumasBoyCekmeOrtalama == boyCekme).ToList()); 
            //        }
            //    }

            //}

            if (kumasDetailModel.kumasEnCekmeMax != 0 && kumasDetailModel.kumasEnCekmeMin != 0)
            {
                kumasKarakteris = kumasKarakteris.Where(x => x.kumasEnCekmeOrtalama >= (kumasDetailModel.kumasEnCekmeMin) && x.kumasEnCekmeOrtalama <= (kumasDetailModel.kumasEnCekmeMax)).ToList();

            }

            if (kumasDetailModel.kumasBoyCekmeMax != 0 && kumasDetailModel.kumasBoyCekmeMin != 0)
            {
                kumasKarakteris = kumasKarakteris.Where(x => x.kumasBoyCekmeOrtalama >= (kumasDetailModel.kumasBoyCekmeMin) && x.kumasBoyCekmeOrtalama <= (kumasDetailModel.kumasBoyCekmeMax)).ToList();
            }

            foreach (var item in kumasKarakteris)
            {
                if (kumasList.Contains(item.kumasAdı))
                {
                    item.isExistOnOrderTable = 1;
                }
                else
                {
                    item.isExistOnOrderTable = 0;
                }
            }

            return kumasKarakteris;
        }

        public RaporlamaDataModel GetRaporlamaDataModel(string idList, string orderNoList, int tabloTuru, string raporTuruList, int yikamaOncesiMi = 0)
        {
            RaporlamaDataModel data = new RaporlamaDataModel();

            //
            List<string> tempidList = idList.Replace("[", "").Replace("]", "").Split(',').ToList();

            List<int> id = new List<int>();

            id = tempidList.Select(int.Parse).ToList();

            List<string> orderNo = orderNoList.Replace("[", "").Replace("]", "").Replace("\"", "").Split(',').ToList();

            var raporTuru = raporTuruList.Replace("[", "").Replace("]", "").Replace("\"", "").Split(',').ToList();
            List<int> raporTuruInt = raporTuru.Select(int.Parse).ToList();
            //
            List<int> tabanIDMustList = new List<int>();
            var firstOrder = orderNo[0];
            var giysiTuru = db.Order.Where(x => x.orderNo == firstOrder).Select(x => x.giysiTuru).FirstOrDefault();

            //BURAYI DÜZELT

            tabanIDMustList = new List<int> { 1, 4, 3,159, 140, 83, 151, 152, 5, 85, 7, 88, 9, 13, 14, 11, 12 };

            int firstID = id[0];
            List<int> tabanID = new List<int>();
            if (raporTuruInt[0] == 0)
            {
                tabanID = db.KazanAra.Where(x => x.kazanDetayID == firstID && x.OlcuNoktalari.tabanID != 82 && x.OlcuNoktalari.tabanID != 21 && x.OlcuNoktalari.tabanID != 23).Select(x => x.OlcuNoktalari.tabanID).Distinct().ToList();
            }
            else
            {
                tabanID = db.NumuneAra.Where(x => x.numuneDetayID == firstID && tabanIDMustList.Contains(x.OlcuNoktalari.tabanID)).Select(x => x.OlcuNoktalari.tabanID).Distinct().ToList();
            }


            tabanID = tabanID.OrderBy(x => tabanIDMustList.IndexOf(x)).ToList();

            RaporlamaDataModel.RaporDetayDTO detayDto = new RaporlamaDataModel.RaporDetayDTO()
            {
                tabloTuru = tabloTuru,
            };


            List<RaporlamaDataModel.RaporAraDTO> araDtoList = new List<RaporlamaDataModel.RaporAraDTO>();
            List<List<RaporlamaDataModel.RaporHesaplamaDTO>> hesaplamaDtoList = new List<List<RaporlamaDataModel.RaporHesaplamaDTO>>();

            for (int i = 0; i < id.Count; i++)
            {
                int kazanDetayID = id[i];


                string orderNoSend = orderNo[i];
                int orderId = db.Order.Where(x => x.orderNo == orderNoSend).Select(x => x.ID).FirstOrDefault();

                int olcuTuruID = 0;

                if (raporTuruInt[i] == 0)
                {
                    olcuTuruID = db.KazanDetay.Where(x => x.id == kazanDetayID).Select(x => x.olcuTuruID).FirstOrDefault();
                }
                else
                {
                    olcuTuruID = db.NumuneDetay.Where(x => x.id == kazanDetayID).Select(x => x.olcuTuruID).FirstOrDefault();
                }
                string olcuTuruAd = db.OlcuTurleri.Where(x => x.id == olcuTuruID).Select(x => x.olcuTuruAd).FirstOrDefault();

                string enBoyCekme = "";

                if (raporTuruInt[i] == 0)
                {
                    enBoyCekme = GetMaxCekme(kazanDetayID);
                }
                else
                {
                    enBoyCekme = db.NumuneDetay.Where(x => x.id == kazanDetayID).Select(x => x.kalipAdi).FirstOrDefault();
                }

                if (raporTuruInt[i] == 0)
                {
                    RaporlamaDataModel.RaporAraDTO araDto = new RaporlamaDataModel.RaporAraDTO()
                    {
                        kazanDetayID = kazanDetayID,
                        orderNo = orderNoSend,
                        olcuTuruID = olcuTuruID,
                        olcuTuruAd = olcuTuruAd,
                        orderID = orderId,
                        enBoyCekme = enBoyCekme,
                        raporTuru = raporTuruInt[i]
                    };

                    araDtoList.Add(araDto);

                    List<RaporlamaDataModel.RaporHesaplamaDTO> raporHesaplamaDTOs = GetHesaplamaList(kazanDetayID, tabanID, raporTuruInt[i]);


                    hesaplamaDtoList.Add(raporHesaplamaDTOs);
                }
                else
                {
                    List<int> beden = db.NumuneAra.Where(x => x.numuneDetayID == kazanDetayID).Select(x => x.bedenID).Distinct().ToList();

                    for (int j = 0; j < beden.Count; j++)
                    {
                        int raporTuruAsInt = raporTuruInt[i];
                        int bedenId = beden[j];
                        RaporlamaDataModel.RaporAraDTO araDto = new RaporlamaDataModel.RaporAraDTO()
                        {
                            kazanDetayID = kazanDetayID,
                            orderNo = orderNoSend,
                            olcuTuruID = olcuTuruID,
                            olcuTuruAd = olcuTuruAd,
                            orderID = orderId,
                            enBoyCekme = enBoyCekme,
                            raporTuru = raporTuruAsInt,
                            beden = db.Bedenler.Where(x => x.ID == bedenId).Select(x => x.beden).FirstOrDefault(),
                            receteKod = db.NumuneDetay.Where(x => x.id == kazanDetayID).Select(x => x.receteKod).FirstOrDefault()
                        };

                        araDtoList.Add(araDto);

                        List<RaporlamaDataModel.RaporHesaplamaDTO> raporHesaplamaDTOs = GetHesaplamaList(kazanDetayID, tabanID, raporTuruInt[i], beden[j], tabloTuru, yikamaOncesiMi);
                        hesaplamaDtoList.Add(raporHesaplamaDTOs);

                    }
                }



            }

            data.detay = detayDto;
            data.araList = araDtoList;
            data.hesaplamaList = hesaplamaDtoList;

            return data;
        }

        public string ReportNameBuilder(RaporlamaDataModel data, int orderId)
        {
            string raporAdi = "";

            RaporlamaDataModel.RaporAraDTO raporAraDto = data.araList[0];


            Order selectedOrder = db.Order.Where(x => x.ID == orderId).FirstOrDefault();

            if (data.detay.tabloTuru == 0)
            {
                raporAdi = selectedOrder.modelNo.Split('-')[0] + "-" + selectedOrder.modelAd + "- 1. Kalıp Töleransı";
            }
            else
            {
                raporAdi = selectedOrder.modelNo.Split('-')[0] + "-" + selectedOrder.modelAd + "- 1. Kalıp Çekmesi";

            }

            return raporAdi;
        }
        #endregion

    }
    //Helper Classes
    #region
    public class KazanDetayModel
    {
        public int kazanDetayID { get; set; }
        public string orderNo { get; set; }

        public string modelNo { get; set; }

        public string kumasAdi { get; set; }

        public string musteri { get; set; }

        public string atolyeAd { get; set; }

        public string modelAdi { get; set; }

        public string yikamaYeri { get; set; }

        public string olcuTuru { get; set; }

        public string tabloTuru { get; set; }

        public DateTime tarih { get; set; }

        public string tarih1 { get; set; }

        public string tarih2 { get; set; }

        public string kullanıcıAdSoyad { get; set; }

        public int raporTuru { get; set; } = 0; //0 Kazan , 1 Numune

        public string receteKod { get; set; }

    }

    public class GelismisRaporlamaModel
    {
        public string orderNo { get; set; }

        public string modelNo { get; set; }

        public string kumas { get; set; }

        public string musteri { get; set; }

        public string AtolyeAd { get; set; }

        public string modelAd { get; set; }

        public string yikamaYeri { get; set; }

        public string olcuTuruAd { get; set; }

        public int id { get; set; }

        public int tabloTuru { get; set; }

        public DateTime tarih { get; set; }

    }

    public class ViewDataModel
    {
        public int tabloTuru { get; set; }
        public List<string> enBoyCekmeler { get; set; }
        public List<string> yollanacakOrderlar { get; set; }
        public List<string> olcuNoktalari { get; set; }
        public List<string> olcuTurleri { get; set; }

        public List<List<Tuple<int, double, double, double>>> data { get; set; }
    }

    public class DenemeClass
    {
        public int tabanID { get; set; }
        public string ortalamaDeger { get; set; }
        public string uygulananCekme { get; set; }
        public string gerceklesenCekme { get; set; }
    }

    public class KurutmaModel
    {
        public string orderNo { get; set; }
        public int derece { get; set; }

        public int dakika { get; set; }
    }

    public class PrintModalDataModel
    {
        public int id { get; set; }
        public NumuneDetay numuneDetay { get; set; }

        public KazanDetay kazanDetay { get; set; }

        public OlcuTabloDetay olcuTabloDetay { get; set; }
        public int tur { get; set; } // Tür 0 ise kazan tür 1 ise ölçü tablosu tür 2 ise numune

        public string enBoyCekme { get; set; } // kazan için en yüksek sayıda girilen en boy çekme.

        public int raporTuru { get; set; } // 0 Kazan 1 Numune

        public string bedenAd { get; set; } // Numune için beden beden ayırma.
    }

    public class TabloDTOModel
    {
        public int tur { get; set; }
        public List<DTOImalatTabloDetay> dtoDetay { get; set; }
        public List<List<DTOImalatTabloGoster>> dtoAra { get; set; }

        public List<KazanAra> headerList { get; set; }

        public List<KazanAra> dbKazanAraList { get; set; }

        public List<KazanHesaplama> GerceklesenUygulananCekmeTol { get; set; }
        public KazanDetay kazanDetay { get; set; }

        public DTONumuneDetay DtoNumuneDetay { get; set; }
        public List<DtoNumuneAra> dtoNumuneAraList { get; set; }

        //Kazan ViewBagları için

        public string atolyeAdList { get; set; }

        public string utuPaketAdList { get; set; }
        public bool fasonMuList { get; set; }

        //Ölçü Tablo ViewBagları için
        public string kullanıcıAdSoyadList { get; set; }

        public string anaBedenList { get; set; }

        public int tabloTuruList { get; set; }

        //Numune için

        public string bedenAd { get; set; }

    }

    public class ArsivDataModel
    {
        public class ReportDetailDTO
        {
            public string OrderNo { get; set; }
            public string ModelNo { get; set; }
            public string Kumas { get; set; }
            public string Musteri { get; set; }
            public string AtolyeAd { get; set; }
            public string ModelAd { get; set; }
            public string YikamaYeri { get; set; }
        }

        public int raporID { get; set; }
        public string raporAdi { get; set; }

        public List<ReportDetailDTO> reportDetailList { get; set; }

        public int tabloTuru { get; set; }

        public DateTime tarih { get; set; }

        public int kullanıcıID { get; set; }

        public string personelAd { get; set; }

        public string personelSoyad { get; set; }
    }

    public class RaporlamaDataModel
    {
        public class RaporDetayDTO
        {
            public int raporID { get; set; }
            public string raporAdi { get; set; }
            public int tabloTuru { get; set; }
            public DateTime tarih { get; set; }
            public int kullaniciID { get; set; }
            public int kalipNo { get; set; }
            public string kullanıcıAdSoyad { get; set; }
            public int anaCekmeVarMi { get; set; }
            public string raporNotu { get; set; }
        }

        public class RaporAraDTO
        {
            public int id { get; set; }
            public int raporID { get; set; }
            public int orderID { get; set; }
            public string orderNo { get; set; }
            public int olcuTuruID { get; set; }
            public string olcuTuruAd { get; set; }
            public string enBoyCekme { get; set; }
            public int kazanDetayID { get; set; }
            public int raporTuru { get; set; } // 0 Kazan,1 Numune
            public string beden { get; set; }
            public string receteKod { get; set; }
        }

        public class RaporHesaplamaDTO
        {
            public int id { get; set; }
            public int raporID { get; set; }
            public int raporAraID { get; set; }
            public int olcuNoktaID { get; set; }
            public string olcuNoktaAd { get; set; }
            public string uygulananCekmeTolerans { get; set; }
            public string gerceklesenCekmeTolerans { get; set; }
            public string ortalamaDeger { get; set; }
        }

        public class RaporOrtalamaDTO
        {
            public int id { get; set; }
            public int raporID { get; set; }
            public int olcuNoktaID { get; set; }
            public string olcuNoktaAd { get; set; }
            public string ortUygulananCekmeTolerans { get; set; }
            public string ortGerceklesenCekmeTolerans { get; set; }
            public string ortOrtalamaDeger { get; set; }

            public string kararVerilenCekmetolerans { get; set; }
            public string aciklama { get; set; }
        }

        public class RaporBagliOrderDTO
        {
            public int Id { get; set; }
            public int RaporId { get; set; }
            public int OrderId { get; set; }
            public string OrderNo { get; set; }
        }

        public class RaporAnaCekmeDTO
        {
            public int Id { get; set; }
            public int RaporId { get; set; }
            public int OlcuNoktaId { get; set; }
            public string OlcuNoktaAd { get; set; }
            public string AnaCekmeCekmeTolerans { get; set; }
        }

        public RaporDetayDTO detay { get; set; }
        public List<RaporAraDTO> araList { get; set; }
        public List<List<RaporHesaplamaDTO>> hesaplamaList { get; set; }
        public List<RaporOrtalamaDTO> ortalamaList { get; set; }
        public List<RaporBagliOrderDTO> bagliOrderList { get; set; }
        public List<RaporAnaCekmeDTO> anaCekmeList { get; set; }
    }

    public class KumasDetailModel
    {
        public class Konstruksiyon
        {
            public string konstruksiyonParameter { get; set; }
            public double konstruksiyonValue { get; set; }
        }

        public string kumasAd { get; set; }
        public float kumasEnMin { get; set; }
        public float kumasEniMax { get; set; }
        public float kumasEniOrtalama { get; set; }
        public float kumasStrechMin { get; set; }
        public float kumasStrechMax { get; set; }
        public float kumasStrectOrtalama { get; set; }
        public float kumasEnCekmeMin { get; set; }
        public float kumasEnCekmeMax { get; set; }
        public float kumasEnCekmeOrtalama { get; set; }
        public float kumasBoyCekmeMin { get; set; }
        public float kumasBoyCekmeMax { get; set; }
        public float kumasBoyCekmeOrtalama { get; set; }
        public int IsKumasExistInOrderTable { get; set; }
        public List<Konstruksiyon> konstruksiyonList { get; set; }
    }

    public class ReceteDetailModel
    {
        public class RecipeStep
        {
            public string RecipeParameter { get; set; }
            public int RecipeMinValue { get; set; }
            public int RecipeMaxValue { get; set; }

        }

        public List<RecipeStep> recipeSteps { get; set; }

        public int receteActive { get; set; } = 0;
    }

    public class GelismisRaporlamaParameters
    {
        public string OrderNo { get; set; } = "";
        public string ModelAdi { get; set; } = "";
        public string ModelNo { get; set; } = "";
        public string Kumas { get; set; } = "";
        public string Musteri { get; set; } = "";
        public string Atolye { get; set; } = "";
        public string YikamaYeri { get; set; } = "";
        public int TabloTuru { get; set; } = 0;
        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
        public KumasDetailModel KumasDetail { get; set; } = new KumasDetailModel();
        public ReceteDetailModel ReceteDetail { get; set; } = new ReceteDetailModel();
        public int IsFromNumunePage { get; set; } = 0;

        public int isAddingOrder { get; set; } = 0;

        public OlcuTuruParameter OlcuTuruParameter { get; set; } = new OlcuTuruParameter();

    }

    public class OlcuTuruParameter
    {
        public List<int> NumuneList { get; set; }
        public List<int> KazanList { get; set; }
    }

    #endregion
}