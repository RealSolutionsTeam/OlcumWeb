using OlcumWeb.Araclar;
using OlcumWeb.dbOlcum;
using OlcumWeb.Models.DTO;
using OlcumWeb.Recipe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class NumuneFormuController : Controller
    {
        OlcumContext db = new OlcumContext();
        FasonController fason = new FasonController();
        RaporlamaController raporlama = new RaporlamaController();
        RecipeContext recipe = new RecipeContext();
        List<NumuneDetay> baglantiliNumunes = new List<NumuneDetay>();
        List<int> baglantiliNumuneIds = new List<int>();

        public bool checkExist = false;

        // GET: NumuneFormu
        public ActionResult Index(int id, int fromModelist)
        {
            ViewBag.fromModelist = fromModelist;
            ViewBag.id = id;
            ViewBag.olcuNoktalari = db.OlcuNoktalari.ToList();
            return View();
        }

        public PartialViewResult OrderDetay(int orderId)
        {
            DTONumuneDetay numuneDetay = GetDTONumuneDetayByOrderId(orderId);
            ViewBag.atolyeler = fason.getAtolyes();
            return PartialView(numuneDetay);
        }

        public PartialViewResult OrderDetayWithId(int numuneDetayId)
        {
            NumuneDetay numuneDetay = db.NumuneDetay.Where(x => x.id == numuneDetayId).FirstOrDefault();
            ViewBag.atolyeler = fason.getAtolyes();

            DTONumuneDetay dtoNumuneDetay = GetDTONumuneDetay(numuneDetay);
            return PartialView("~/Views/NumuneFormu/OrderDetay.cshtml", dtoNumuneDetay);
        }


        public PartialViewResult TableView(int orderId, List<string> bedenler, RaporlamaDataModel tolerans, RaporlamaDataModel cekme, int fromModelist)
        {
            List<DtoNumuneAra> dtos = ConvertDtoNumuneAra(orderId, bedenler, tolerans, cekme);
            ViewBag.cekme = cekme;
            ViewBag.tolerans = tolerans;
            ViewBag.bedens = bedenler;
            ViewBag.isFromModelist = fromModelist;
            ViewBag.isReference = 0;
            return PartialView(dtos);
        }

        public PartialViewResult TableViewWithId(int numuneDetayId, int isFromModelist = 0)
        {

            List<NumuneAra> numuneAras = db.NumuneAra.Where(x => x.numuneDetayID == numuneDetayId).OrderBy(x=>x.id).ToList();

            List<DtoNumuneAra> dtoNumuneAras = GetDtoNumuneAras(numuneAras);

            ViewBag.isFromModelist = isFromModelist;

            ViewBag.bedens = numuneAras.GroupBy(x => new { x.Bedenler.beden, x.mudahaleMi }).Select(x => x.Key.beden).ToList();

            var numune = db.NumuneDetay.Where(x => x.id == numuneDetayId).FirstOrDefault();

            var receteKods = db.NumuneDetay.Where(x => x.orderID == numune.orderID && x.olcumNo == numune.olcumNo && x.tabloTuru != -1).ToList();

            List<DTONumuneDetay> dtoNumuneDetays = GetDTONumuneDetays(receteKods);

            ViewBag.receteKods = dtoNumuneDetays;
            ViewBag.selectedReceteKod = numune.id;

            return PartialView("~/Views/NumuneFormu/TableView.cshtml", dtoNumuneAras);
        }

        public PartialViewResult TableViewWithIdReference(int numuneDetayId, List<string> bedenList)
        {

            List<NumuneAra> numuneAras = db.NumuneAra.Where(x => x.numuneDetayID == numuneDetayId && bedenList.Contains(x.Bedenler.beden)).OrderBy(x=>x.id).ToList();

            List<DtoNumuneAra> dtoNumuneAras = GetDtoNumuneAras(numuneAras);

            ViewBag.isFromModelist = 1;
            ViewBag.isReference = 1;

            ViewBag.bedens = numuneAras.GroupBy(x => new { x.Bedenler.beden, x.mudahaleMi }).Select(x => x.Key.beden).ToList();


            return PartialView("~/Views/NumuneFormu/TableView.cshtml", dtoNumuneAras);
        }

        public PartialViewResult TableViewWithImalatIdReference(int olcuTabloId, List<string> bedenList)
        {
            var olcuTabloDetay = db.OlcuTabloDetay.Where(x => x.id == olcuTabloId).FirstOrDefault();
            int orderId = olcuTabloDetay.orderID;

            var yikamaSonrasiTablo = db.OlcuTabloDetay.Where(x => x.orderID == orderId && x.tabloTuru == 1).FirstOrDefault();
            int yikamaSonrasiOlcuTabloId = yikamaSonrasiTablo.id;


            List<OlcuTabloAra> yikamaSonrasi = db.OlcuTabloAra.Where(x => x.olcuTabloID == yikamaSonrasiOlcuTabloId && bedenList.Contains(x.Bedenler.beden)).OrderBy(x => x.id).ToList();
            List<OlcuTabloAra> olcuTabloAras = db.OlcuTabloAra.Where(x => x.olcuTabloID == olcuTabloId && bedenList.Contains(x.Bedenler.beden)).OrderBy(x => x.id).ToList();
            List<OlcuTabloHesaplama> olcuTabloHesaplamas = db.OlcuTabloHesaplama.Where(x => x.olcuTabloID == olcuTabloId).OrderBy(x=>x.id).ToList();

            List<DtoNumuneAra> dtoNumuneAras = GetDtoNumuneArasWithImalat(yikamaSonrasi,olcuTabloAras,olcuTabloHesaplamas, bedenList);

            ViewBag.isFromModelist = 1;
            ViewBag.isReference = 1;

            ViewBag.bedens = olcuTabloAras.Select(x => x.Bedenler.beden).Distinct().ToList();
            return PartialView("~/Views/NumuneFormu/TableView.cshtml", dtoNumuneAras);
        }

       

        public PartialViewResult TableViewWithAddedSizes(List<BedenDto> bedenDtos, List<DtoNumuneAra> dtoNumuneAras,int isFromModelist, RaporlamaDataModel tolerans = null, RaporlamaDataModel cekme = null, List<DTONumuneDetay> receteKods = null,int selectedReceteKod = 0)
        {

            string firstBeden = dtoNumuneAras.Select(x => x.BedenAd).FirstOrDefault().Split('-')[0];

            List<DtoNumuneAra> firstBedenList = dtoNumuneAras.Where(x => x.BedenAd == firstBeden).ToList();



            if(isFromModelist == 1)
            {
                if(bedenDtos != null)
                {
                    List<string> existingSizes = dtoNumuneAras.Select(x => x.BedenAd).Distinct().ToList();

                    List<string> selectedSizes = bedenDtos.Select(x => x.BedenAd).Distinct().ToList();

                    List<string> removedSizes = existingSizes.Where(x => !selectedSizes.Contains(x)).ToList();

                    if (removedSizes.Count > 0 && bedenDtos[0].MüdahaleMi != 1)
                    {
                        foreach (var item in removedSizes)
                        {
                            dtoNumuneAras.RemoveAll(x => x.BedenAd == item);
                        }
                    }
                }

            }
            else
            {
                if(bedenDtos != null)
                {
                    List<string> existingSizes = dtoNumuneAras.Where(x => x.MudahaleMi == true).Select(x => x.BedenAd).Distinct().ToList();

                    List<string> selectedSizes = bedenDtos.Where(x => x.MüdahaleMi == 1).Select(x => x.BedenAd).Distinct().ToList();

                    List<string> removedSizes = existingSizes.Where(x => !selectedSizes.Contains(x)).ToList();

                    if (removedSizes.Count > 0)
                    {
                        foreach (var item in removedSizes)
                        {
                            dtoNumuneAras.RemoveAll(x => x.BedenAd == item && x.MudahaleMi == true);
                        }
                    }
                }
                else
                {
                    dtoNumuneAras.RemoveAll(x => x.MudahaleMi == true);
                }
    
            }

            if(bedenDtos != null)
            {
                foreach (var item in bedenDtos)
                {
                    if (item.MüdahaleMi == 1)
                    {
                        if (dtoNumuneAras.Select(x => x.BedenAd).Distinct().ToList().Contains(item.BedenAd))
                        {
                            List<DtoNumuneAra> mudahaleBedenList = dtoNumuneAras.Where(x => x.BedenId == item.BedenId).ToList();

                            List<DtoNumuneAra> tempMudahaleBedenList = DeepCopy(mudahaleBedenList);
                            foreach (var tempNumuneAra in tempMudahaleBedenList)
                            {
                                tempNumuneAra.BedenAd = item.BedenAd;
                                tempNumuneAra.BedenId = item.BedenId;
                                tempNumuneAra.MudahaleMi = true;
                            }

                            dtoNumuneAras.AddRange(tempMudahaleBedenList);
                        }

                    }
                    else
                    {
                        if (!dtoNumuneAras.Select(x => x.BedenAd).Distinct().ToList().Contains(item.BedenAd))
                        {
                            List<DtoNumuneAra> tempNumuneAras = DeepCopy(firstBedenList);

                            foreach (var tempNumuneAra in tempNumuneAras)
                            {
                                tempNumuneAra.BedenAd = item.BedenAd;
                                tempNumuneAra.BedenId = item.BedenId;

                                if (isFromModelist == 0)
                                {
                                    tempNumuneAra.GerceklesenCekme = 0;
                                    tempNumuneAra.GerceklesenTolerans = 0;
                                    tempNumuneAra.YikamaOncesiOlculen = 0;
                                    tempNumuneAra.YikamaSonrasiOlculen = 0;
                                    tempNumuneAra.YikamaOncesiFark = 0;
                                    tempNumuneAra.YikamaSonrasiFark = 0;
                                }
                                else
                                {
                                    tempNumuneAra.KalipOlcusu = 0;
                                    tempNumuneAra.YikamaOncesiTablo = 0;
                                    tempNumuneAra.YikamaSonrasiTablo = 0;
                                }

                            }

                            dtoNumuneAras.AddRange(tempNumuneAras);

                        }
                    }
                }
            }
           

            if(tolerans != null)
            {
                if (tolerans.araList.Count > 0)
                {
                    ViewBag.tolerans = tolerans;
                }
            }

            if(cekme != null)
            {
                if (cekme.araList.Count > 0)
                {
                    ViewBag.cekme = cekme;
                }
            }

            ViewBag.isFromModelist = isFromModelist;
            ViewBag.receteKods = receteKods == null ? new List<DTONumuneDetay>() : receteKods;
            ViewBag.selectedReceteKod = selectedReceteKod;


            List<string> bedenler = dtoNumuneAras.GroupBy(x => new { x.BedenAd, x.MudahaleMi }).Select(x => x.Key.BedenAd).ToList();

            int firstBedenId = dtoNumuneAras.Select(x => x.BedenId).FirstOrDefault();

            int? sayiSistemi = db.Bedenler.Where(x => x.ID == firstBedenId).Select(x => x.bedenSistemi).FirstOrDefault();

            bedenler = OrderSize(sayiSistemi, bedenler);

            ViewBag.bedens = bedenler;

            return PartialView("~/Views/NumuneFormu/TableView.cshtml", dtoNumuneAras);
        }

        public PartialViewResult TableWithCustomerTable(List<string> bedenler,int orderId)
        {
            List<DtoNumuneAra> dtoNumuneAras = GetFromCustomerTable(bedenler, orderId);

            ViewBag.isFromModelist = 1;
            ViewBag.isReference = 1;
            ViewBag.bedens = bedenler;

            return PartialView("~/Views/NumuneFormu/TableView.cshtml", dtoNumuneAras);
        }

        public JsonResult CheckOrderExists(int orderId)
        {
            checkExist = db.NumuneDetay.Any(x => x.orderID == orderId);

            return Json(checkExist, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult StepsModal()
        {
            return PartialView();
        }

        public PartialViewResult OrderTablesModal(int orderId, int isReference = 0,int isNumune = 1)
        {
            if(isNumune == 1)
            {
                List<NumuneDetay> numuneDetays = new List<NumuneDetay>();

                if (isReference == 1)
                {
                    numuneDetays = db.NumuneDetay.Where(x => x.orderID == orderId && (x.tabloTuru == -1 || x.tabloTuru == 0)).ToList();
                }
                else
                {
                    numuneDetays = db.NumuneDetay.Where(x => x.orderID == orderId).ToList();
                }

                List<DTONumuneDetay> numuneDetaysDto = GetDTONumuneDetays(numuneDetays);

                ViewBag.isReference = isReference;

                return PartialView(numuneDetaysDto);
            }
            else
            {
                List<OlcuTabloDetay> olcuTabloDetays = db.OlcuTabloDetay.Where(x => x.orderID == orderId && x.tabloTuru == 0).ToList();

                List<DTOImalatTabloDetay> imalatTabloDetays = GetDtoImalatTabloDetays(olcuTabloDetays);

                return PartialView("OrderTablesImalat", imalatTabloDetays);
            }
          
        }

      


        //Adım 1
        public PartialViewResult OlcuTurleriModal()
        {
            List<OlcuTurleri> olcuTurleris = db.OlcuTurleri.Where(x => x.tabloTuru == "Numune").ToList();

            List<OlcuTuruOranDto> olcuTurleriOrani = new List<OlcuTuruOranDto>();

            int toplamNumuneCount = db.NumuneDetay.Count();

            foreach (var item in olcuTurleris)
            {
                int olcuTuruId = item.id;

                int numuneCount = db.NumuneDetay.Where(x => x.olcuTuruID == olcuTuruId).Count();


                double oran = (numuneCount * 100 / toplamNumuneCount) ;

                string olcuTuruAd = item.olcuTuruAd;

                OlcuTuruOranDto olcuTuruOranDto = new OlcuTuruOranDto()
                {
                    OlcuTuruAd = olcuTuruAd,
                    Oran = oran
                };

                olcuTurleriOrani.Add(olcuTuruOranDto);
            }


            olcuTurleriOrani = olcuTurleriOrani.OrderByDescending(x => x.Oran).ToList();

            List<string> olcuTurleriAd = olcuTurleriOrani.Select(x => x.OlcuTuruAd).ToList();

            olcuTurleris = olcuTurleris.OrderBy(x => olcuTurleriAd.IndexOf(x.olcuTuruAd)).ToList();

            return PartialView(olcuTurleris);
        }
        //Adım 2
        public PartialViewResult ChooseOrderModal(int olcuTuruId, int isFromModelist)
        {
            string olcuTuru = db.OlcuTurleri.Where(x => x.id == olcuTuruId).Select(x => x.olcuTuruAd).FirstOrDefault();

            if (olcuTuru == "ÇOĞALTMA HAMI")
            {
                olcuTuru = "ÇOĞALTMA";
            }

            if (isFromModelist == 1)
            {
                if (olcuTuru == "TOPLANTI")
                {
                    ViewBag.orders = db.Order.Where(x => x.orderTipi == olcuTuru || x.orderTipi == "ÇOĞALTMA").ToList();

                }
                else if(olcuTuru == "YIKAMA OTURTMA")
                {
                    ViewBag.orders = db.Order.Where(x => x.orderTipi == olcuTuru || x.orderNo.Contains("YON")).ToList();
                }
                else if(olcuTuru == "METRAJ")
                {
                    ViewBag.orders = db.Order.Where(x => x.orderTipi == olcuTuru || x.orderNo.Contains("MTR")).ToList();
                }
                else if (olcuTuru == "OFFER")
                {
                    ViewBag.orders = db.Order.Where(x => x.orderTipi == olcuTuru || x.orderNo.Contains("OFR")).ToList();
                }
                else if(olcuTuru == "TASARIM")
                {
                    ViewBag.orders = db.Order.Where(x => x.orderTipi == olcuTuru || x.orderNo.StartsWith("T")).ToList();
                }
                else
                {
                    ViewBag.orders = db.Order.Where(x => x.orderTipi == olcuTuru).ToList();
                }
            }
            else
            {
                ViewBag.orders = db.NumuneDetay.Where(x => x.olcuTuruID == olcuTuruId).Select(x => x.Order).Distinct().ToList();
            }
            return PartialView();
        }

        //Adım 3
        public PartialViewResult HasPreviousOrderModal(int olcuTuruId)
        {
            string olcuTuruAd = db.OlcuTurleri.Where(x => x.id == olcuTuruId).Select(x => x.olcuTuruAd).FirstOrDefault();

            string oncekiOrderTipleri = "";

            List<Order> orders = new List<Order>();

            if (olcuTuruAd == "TASARIM" || olcuTuruAd == "PROTO")
            {
                oncekiOrderTipleri = "Tasarım";

                orders = raporlama.GetOrders(x => x.orderTipi == "TASARIM").ToList();
            }
            else if (olcuTuruAd == "ÇOĞALTMA" || olcuTuruAd == "TOPLANTI" || olcuTuruAd == "SMS")
            {
                oncekiOrderTipleri = "Tasarım, Proto veya Çoğaltma";
                orders = raporlama.GetOrders(x => x.orderTipi == "TASARIM" || x.orderTipi == "PROTO" || x.orderTipi == "ÇOĞALTMA").ToList();

            }
            else
            {
                oncekiOrderTipleri = "Tasarım veya Proto";
                orders = raporlama.GetOrders(x => x.orderTipi == "TASARIM" || x.orderTipi == "PROTO").ToList();

            }

            ViewBag.previousOrders = orders;
            ViewBag.oncekiOrderTipleri = oncekiOrderTipleri;

            return PartialView();
        }

        public ActionResult SetReferenceTable(int orderId)
        {
            GetConnectedOrdersRecursive(orderId);

            if (baglantiliNumuneIds.Count > 0)
            {
                //var connectedOrders = String.Join(",", baglantiliNumunes.Select(x => x.Order.orderNo).ToArray());

                //var connectedOrderIds = String.Join(",", baglantiliNumunes.Select(x => x.id).ToArray());

                //List<int> raporTurus = new List<int>();

                //for (int i = 0; i < baglantiliNumunes.Count; i++)
                //{
                //    raporTurus.Add(1);
                //}

                //string raporTuruList = String.Join(",", raporTurus.ToArray());

                //RaporlamaDataModel data = raporlama.GetRaporlamaDataModel(connectedOrderIds, connectedOrders, 1, raporTuruList);
                //ViewBag.orders = raporlama.GetOrders();
                //ViewBag.isFromNumunePage = 1;
                //ViewBag.tabloTuru = 1;

                return Json(baglantiliNumuneIds, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return PartialView("~/Views/Errors/NoConnectedOrders.cshtml");
            }

        }

        public PartialViewResult ChooseTableComeFrom()
        {
            return PartialView();
        }

        public PartialViewResult NewTableModal(int tabloTuru)
        {
            ViewBag.tabloTuru = tabloTuru;

            return PartialView();
        }

        public PartialViewResult GetFromReferenceTableModal()
        {
            var numuneOrders = db.NumuneDetay.Where(x => x.tabloTuru == -1 || x.tabloTuru == 0).Select(x => new ReferenceOrderDto { Order = x.Order, IsNumune = 1 }).Distinct().ToList();
            var imalatOrders = db.OlcuTabloDetay.Select(x => new ReferenceOrderDto { Order = x.Order, IsNumune = 0 }).Distinct().ToList();

            var tempList = numuneOrders.Concat(imalatOrders).ToList();

            ViewBag.orders = tempList;
            return PartialView();
        }

        public PartialViewResult GelismisRaporlamaModal(int orderId, int toleransMi = 0, int isAddingOrder = 0)
        {
            List<Order> orders = raporlama.GetOrders(x => !x.orderNo.StartsWith("K") && !x.orderNo.StartsWith("T") && !x.orderNo.StartsWith("EKS16") && !x.orderNo.StartsWith("EKS17") && !x.orderNo.StartsWith("EKS18") && !x.orderNo.StartsWith("EKS19")
           && !x.orderNo.StartsWith("Ü16") && !x.orderNo.StartsWith("Ü17") && !x.orderNo.StartsWith("Ü18") && !x.orderNo.StartsWith("Ü19"));
            ViewBag.orderNo = orders.Select(x => x.orderNo).ToList();
            ViewBag.modelAd = orders.Where(x => x.modelAd != null).Select(x => x.modelAd).Distinct().ToList();
            ViewBag.modelNo = orders.Where(x => (x.modelNo != null) && (!x.modelNo.Contains("-"))).Select(x => x.modelNo).Distinct().ToList();
            ViewBag.kumas = orders.Where(x => x.kumas != null).Select(x => x.kumas).Distinct().ToList();
            ViewBag.musteri = orders.Where(x => x.musteri != null).Select(x => x.musteri).Distinct().ToList();
            ViewBag.atolyeAd = fason.getAtolyes().Select(x => x.AtolyeAd).Distinct().ToList();

            ViewBag.isFromNumunePage = 1;
            ViewBag.toleransMi = toleransMi;
            ViewBag.isAddingOrder = isAddingOrder;

            Order order = raporlama.GetSingleOrder(x => x.ID == orderId);
            ViewBag.orderFromNumunePage = order;

            string orderNo = order.orderNo;
            bool proxyCreation = recipe.Configuration.ProxyCreationEnabled;
            recipe.Configuration.ProxyCreationEnabled = false;



            var recipeSteps = recipe.RECIPE_STEPS.Join(recipe.RECIPE, x => x.RECIPE_ID, y => y.ID, (x, y) => new { RECIPE_STEPS = x, RECIPE = y }).Where(x => x.RECIPE.ORDER_NO_YEDEK == orderNo || x.RECIPE.ORDER_NO == orderNo).Where(x => x.RECIPE_STEPS.WASHING_TYPE_NAME == "Ön Yıkama" || x.RECIPE_STEPS.WASHING_TYPE_NAME == "Taş" || x.RECIPE_STEPS.WASHING_TYPE_NAME == "Enzim" || x.RECIPE_STEPS.WASHING_TYPE_NAME == "Kasar" || x.RECIPE_STEPS.WASHING_TYPE_NAME == "Ağartma Hypo" ||
            x.RECIPE_STEPS.WASHING_TYPE_NAME == "Enzim Tüy" || x.RECIPE_STEPS.WASHING_TYPE_NAME == "Yumuşatma" || x.RECIPE_STEPS.WASHING_TYPE_NAME == "Kurutma").Select(x => x.RECIPE_STEPS).Distinct().ToList();

            if (recipeSteps.Count > 0)
            {
                ViewBag.recipeStepsFromNumunePage = recipeSteps;
            }

            recipe.Configuration.ProxyCreationEnabled = proxyCreation;

            if (order.kumas != null)
            {
                string kumas = order.kumas;

                KumasKarakteri kumasKarakteri = db.KumasKarakteri.Where(x => x.kumasAdı == kumas).FirstOrDefault();

                if (kumasKarakteri != null)
                {
                    ViewBag.kumasKarakteriFromNumunePage = kumasKarakteri;
                }
            }

            return PartialView();
        }

        public PartialViewResult KarsilastirmaModal(string idList, string orderNoList, int tabloTuru, string raporTuruList)
        {
            RaporlamaDataModel data = raporlama.GetRaporlamaDataModel(idList, orderNoList, tabloTuru, raporTuruList);
            ViewBag.orders = raporlama.GetOrders();
            ViewBag.isFromNumunePage = 1;
            ViewBag.tabloTuru = tabloTuru;
            return PartialView("~/Views/Raporlama/KarsilastirmaModal.cshtml", data);
        }

        public PartialViewResult KarsilastirmaModalNext(RaporlamaDataModel data)
        {
            List<string> orderNoList = data.araList.Select(x => x.orderNo).ToList();
            List<int> raporTuruList = data.araList.Select(x => x.raporTuru).ToList();

            List<int> idList = new List<int>();
            List<string> orderNoListFromidList = new List<string>();
            List<int> raporTuruListFromidList = new List<int>();

            ViewBag.orders = raporlama.GetOrders();
            ViewBag.isFromNumunePage = 1;
            ViewBag.tabloTuru = 0;

            for (int i = 0; i < orderNoList.Count; i++)
            {
                string orderNo = orderNoList[i];

                if (raporTuruList[i] == 0)
                {
                    List<int> idListByOrderNo = db.KazanDetay.Where(x => x.Order.orderNo == orderNo && x.tabloTuru == 0 && (x.olcuTuruID == 16 || x.olcuTuruID == 17)).Select(x => x.id).ToList();
                    List<string> orderNoListByOrderNo = db.KazanDetay.Where(x => x.Order.orderNo == orderNo && x.tabloTuru == 0 && (x.olcuTuruID == 16 || x.olcuTuruID == 17)).Select(x => x.Order.orderNo).ToList();

                    if (!idList.Any(x => idListByOrderNo.Any(y => y == x)))
                    {
                        idList.AddRange(idListByOrderNo);
                        orderNoListFromidList.AddRange(orderNoListByOrderNo);

                        for (int j = 0; j < idListByOrderNo.Count; j++)
                        {
                            raporTuruListFromidList.Add(0);
                        }
                    }
                }
                else
                {
                    var numuneDetayList = db.NumuneDetay.Where(x => x.Order.orderNo == orderNo).ToList();

                    if (numuneDetayList.Any(x => x.receteKod != "" && x.receteKod != null))
                    {
                        numuneDetayList = numuneDetayList.Where(x => x.tabloTuru == 1).ToList();
                    }
                    else
                    {
                        numuneDetayList = numuneDetayList.Where(x => x.tabloTuru == 0).ToList();

                    }

                    List<int> idListByOrderNo = numuneDetayList.Select(x => x.id).ToList();
                    List<string> orderNoListByOrderNo = numuneDetayList.Select(x => x.Order.orderNo).ToList();


                    if (!idList.Any(x => idListByOrderNo.Any(y => y == x)))
                    {
                        idList.AddRange(idListByOrderNo);
                        orderNoListFromidList.AddRange(orderNoListByOrderNo);

                        for (int j = 0; j < idListByOrderNo.Count; j++)
                        {
                            raporTuruListFromidList.Add(1);
                        }
                    }
                }
            }

            string idListString = String.Join(",", idList.ToArray());

            string orderNoListString = String.Join(",", orderNoListFromidList.ToArray());

            string raporTuruListString = String.Join(",", raporTuruListFromidList.ToArray());

            RaporlamaDataModel dataNext = raporlama.GetRaporlamaDataModel(idListString, orderNoListString, 0, raporTuruListString, 1);

            return PartialView("~/Views/Raporlama/KarsilastirmaModal.cshtml", dataNext);
        }

        public PartialViewResult BedenlerModal(int bedenSistemi = 0,int isFromHazirTablo = 0)
        {
            List<string> bedenTurList = new List<string>()
            {
                "Harf Sistemi(S, M, L, XL vb.)",
                "Sayı Sistemi(24, 26, 28 vb.)",
                "Çocuk Pantolonu(104, 110, 116 vb.)"
            };

            string selectedValue = "";

            if (bedenSistemi == 0)
            {
                selectedValue = "Harf Sistemi(S, M, L, XL vb.)";
            }
            else if (bedenSistemi == 1)
            {
                selectedValue = "Sayı Sistemi(24, 26, 28 vb.)";
            }
            else if (bedenSistemi == 2)
            {
                selectedValue = "Çocuk Pantolonu(104, 110, 116 vb.)";
            }

            ViewBag.bedenTurs = new SelectList(bedenTurList, selectedValue);

            var bedens = db.Bedenler.ToList();
            ViewBag.bedenSistemi = bedenSistemi;
            ViewBag.isFromReference = 0;
            ViewBag.isFromHazirTablo = isFromHazirTablo;


            return PartialView(bedens);
        }

        public PartialViewResult TableEditModal(int numuneDetayId)
        {
            ViewBag.numuneDetayIdTableEdit = numuneDetayId;
            return PartialView();
        }

        public PartialViewResult GetRaporlama(RaporlamaDataModel data, int tabloTuru)
        {
            ViewBag.isFromNumunePage = 1;
            ViewBag.tabloTuru = tabloTuru;
            return PartialView("~/Views/Raporlama/KarsilastirmaModal.cshtml", data);
        }

        public JsonResult SaveTableWithRecipeCode(int numuneDetayId, string recipeCode)
        {
            NumuneDetay numuneDetay = db.NumuneDetay.Where(x => x.id == numuneDetayId).FirstOrDefault();

            DTONumuneDetay numuneDetayDto = GetDTONumuneDetay(numuneDetay);

            numuneDetayDto.ReceteKod = recipeCode;

            numuneDetayDto.Tarih = DateTime.Now;
            numuneDetayDto.UpdatedDate = DateTime.Now;

            numuneDetayDto.tabloTuru = 1;

            List<NumuneAra> numuneAraList = db.NumuneAra.Where(x => x.numuneDetayID == numuneDetayId).ToList();

            List<DtoNumuneAra> numuneAraListDto = GetDtoNumuneAras(numuneAraList);

            int isSaved = AddTable(numuneDetayDto, numuneAraListDto);

            if (isSaved != 0)
            {
                return Json(isSaved, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult SaveTable(DTONumuneDetay detay, List<DtoNumuneAra> araList, RaporlamaDataModel tolerans = null, RaporlamaDataModel cekme = null, int numuneDetayId = 0)
        {
            try
            {
                int isSaved = AddTable(detay, araList, tolerans, cekme, numuneDetayId);

                if (isSaved != 0)
                {
                    return Json(isSaved, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveTable(int numuneDetayId)
        {
            try
            {
                int isSuccess = RemoveNumune(numuneDetayId);

                return Json(isSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public int AddTable(DTONumuneDetay detay, List<DtoNumuneAra> araList, RaporlamaDataModel tolerans = null, RaporlamaDataModel cekme = null, int numuneDetayId = 0)
        {
            int isSuccess = 0;

            try
            {
                int orderID = detay.OrderId;
                int olcuTuruID = detay.OlcuTuruId;
                int olcumNo = (int)detay.OlcumNo;
                string receteKod = detay.ReceteKod == null ? "" : detay.ReceteKod;
                string giysiTuruAd = db.Order.Where(x => x.ID == orderID).Select(x => x.giysiTuru).FirstOrDefault();
                int giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuruAd).Select(x => x.id).FirstOrDefault();

                NumuneDetay detayTablo = new NumuneDetay();
                NumuneDetay previosNumune = db.NumuneDetay.Where(x => x.orderID == orderID && x.olcuTuruID == olcuTuruID && x.giysiTuruID == giysiTuruId && x.tabloTuru == detay.tabloTuru && x.receteKod == detay.ReceteKod).OrderByDescending(x=>x.id).FirstOrDefault();

                if (numuneDetayId == 0)
                {
                    if(previosNumune != null)
                    {
                        detay.OlcumNo = (int)previosNumune.olcumNo + 1;
                    }

                    detayTablo = new NumuneDetay()
                    {
                        aciklama = detay.Aciklama,
                        aktarimMi = false,
                        atolyeID = detay.AtolyeId,
                        bagliOrderId = detay.BagliOrderId,
                        dikimNedeni = detay.DikimNedeni,
                        elliListe = detay.ElliListe,
                        elliNumune = detay.ElliNumune,
                        giysiTuruID = giysiTuruId,
                        kalipAdi = detay.KalipAdi,
                        hasPreviousOrder = detay.HasPreviousOrder,
                        KID = detay.KID,
                        kullaniciID = detay.KullaniciId,
                        olcumNo = detay.OlcumNo,
                        olcuTuruID = detay.OlcuTuruId,
                        orderID = detay.OrderId,
                        receteKod = detay.ReceteKod == null ? "" : detay.ReceteKod,
                        tabloTuru = detay.tabloTuru,
                        tarih = Convert.ToDateTime(detay.Tarih),
                        topNo = detay.TopNo,
                        updatedDate = Convert.ToDateTime(detay.UpdatedDate),
                        updatedUserId = detay.UpdatedUserId,
                        yikamadanGelis = Convert.ToDateTime(detay.YikamadanGelis),
                        yikamayaGidis = Convert.ToDateTime(detay.YikamayaGidis),
                        referansOrderId = detay.ReferansOrderId
                    };

                    db.NumuneDetay.Add(detayTablo);
                    db.SaveChanges();


                }
                else
                {
                    detayTablo = db.NumuneDetay.Where(x => x.id == numuneDetayId).FirstOrDefault();

                    if (detayTablo.tabloTuru != detay.tabloTuru)
                    {

                        detayTablo = new NumuneDetay()
                        {
                            aciklama = detay.Aciklama,
                            aktarimMi = false,
                            atolyeID = detay.AtolyeId,
                            bagliOrderId = detay.BagliOrderId,
                            dikimNedeni = detay.DikimNedeni,
                            elliListe = detay.ElliListe,
                            elliNumune = detay.ElliNumune,
                            giysiTuruID = giysiTuruId,
                            kalipAdi = detay.KalipAdi,
                            hasPreviousOrder = detay.HasPreviousOrder,
                            KID = detay.KID,
                            kullaniciID = detay.KullaniciId,
                            olcumNo = detay.OlcumNo,
                            olcuTuruID = detay.OlcuTuruId,
                            orderID = detay.OrderId,
                            receteKod = detay.ReceteKod == null ? "" : detay.ReceteKod,
                            tabloTuru = detay.tabloTuru,
                            tarih = Convert.ToDateTime(detay.Tarih),
                            topNo = detay.TopNo,
                            updatedDate = Convert.ToDateTime(detay.UpdatedDate),
                            updatedUserId = detay.UpdatedUserId,
                            yikamadanGelis = Convert.ToDateTime(detay.YikamadanGelis),
                            yikamayaGidis = Convert.ToDateTime(detay.YikamayaGidis),
                            referansOrderId = detay.ReferansOrderId
                        };

                        db.NumuneDetay.Add(detayTablo);
                    }
                    else
                    {
                        if (previosNumune != null)
                        {
                            detay.OlcumNo = (int)previosNumune.olcumNo + 1;
                        }


                        detayTablo.aciklama = detay.Aciklama;
                        detayTablo.aktarimMi = false;
                        detayTablo.atolyeID = detay.AtolyeId;
                        detayTablo.bagliOrderId = detay.BagliOrderId;
                        detayTablo.dikimNedeni = detay.DikimNedeni;
                        detayTablo.elliListe = detay.ElliListe;
                        detayTablo.elliNumune = detay.ElliNumune;
                        detayTablo.giysiTuruID = giysiTuruId;
                        detayTablo.kalipAdi = detay.KalipAdi;
                        detayTablo.hasPreviousOrder = detay.HasPreviousOrder;
                        detayTablo.KID = detay.KID;
                        detayTablo.kullaniciID = detay.KullaniciId;
                        detayTablo.olcumNo = detay.OlcumNo;
                        detayTablo.olcuTuruID = detay.OlcuTuruId;
                        detayTablo.orderID = detay.OrderId;
                        detayTablo.receteKod = detay.ReceteKod == null ? "" : detay.ReceteKod;
                        detayTablo.tabloTuru = detay.tabloTuru;
                        detayTablo.topNo = detay.TopNo;
                        detayTablo.updatedDate = Convert.ToDateTime(detay.UpdatedDate);
                        detayTablo.updatedUserId = detay.UpdatedUserId;
                        detayTablo.yikamadanGelis = Convert.ToDateTime(detay.YikamadanGelis);
                        detayTablo.yikamayaGidis = Convert.ToDateTime(detay.YikamayaGidis);

                        db.Database.ExecuteSqlCommand("delete from NumuneAra where numuneDetayID = @numuneDetayId", new SqlParameter("@numuneDetayId", numuneDetayId));

                    }



                    db.SaveChanges();

                }

                foreach (var item in araList)
                {
                    item.NumuneDetayId = detayTablo.id;
                }

                using (var connection = new SqlConnection(ConnectionStrings.OlcumConnection))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Id", "id"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NumuneDetayId", "numuneDetayID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OlcuNoktaId", "olcuNoktaID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BedenId", "bedenID"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("KalipOlcusu", "kalipOlcusu"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UygulananTolerans", "uygulananTolerans"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YikamaOncesiTablo", "yikamaOncesiTablo"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UygulananCekme", "uygulananCekme"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YikamaSonrasiTablo", "yikamaSonrasiTablo"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("GerceklesenTolerans", "gerceklesenTolerans"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YikamaOncesiOlculen", "yikamaOncesiOlculen"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("GerceklesenCekme", "gerceklesenCekme"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YikamaSonrasiOlculen", "yikamaSonrasiOlculen"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YikamaSonrasiFark", "yikamaSonrasiFark"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YikamaOncesiFark", "yikamaOncesiFark"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("MudahaleMi", "mudahaleMi"));


                        bulkCopy.BatchSize = 500;
                        bulkCopy.BulkCopyTimeout = 45;
                        bulkCopy.DestinationTableName = "[dbo].[NumuneAra]";
                        try
                        {
                            bulkCopy.WriteToServer(araList.AsDataTable());
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            connection.Close();
                            isSuccess = 0;

                            return isSuccess;
                        }
                    }

                    transaction.Commit();
                    connection.Close();

                }

                if(tolerans!= null)
                {
                    if (tolerans.hesaplamaList != null)
                    {
                        //Töleransı kaydediyorum.
                        RaporDetay raporDetay = raporlama.SaveArsiv(tolerans, 0, 1, detay.OrderId);
                    }
                }

                if(cekme != null)
                {
                    if (cekme.hesaplamaList != null)
                    {
                        RaporDetay raporDetay = raporlama.SaveArsiv(cekme, 0, 1, detay.OrderId);
                    }
                }
               
                isSuccess = detayTablo.id;

                return isSuccess;
            }
            catch (Exception ex)
            {
                isSuccess = 0;

                return isSuccess;
            }
        }

        public PartialViewResult BedenlerPrint(List<BedenDto> bedenler)
        {
            ViewBag.bedenler = bedenler;


            return PartialView();
        }

        public PartialViewResult PrintTable(DTONumuneDetay detay, List<DtoNumuneAra> araList, List<string> bedenler)
        {
            List<DtoNumuneAra> dtoNumuneAras = new List<DtoNumuneAra>();

            if (bedenler.Any(x=> x.Contains("Müdahale")))
            {

                foreach (var item in bedenler)
                {
                    if (item.Contains("Müdahale"))
                    {
                        string bedenAd = item.Split('-')[0];
                        dtoNumuneAras.AddRange(araList.Where(x => x.BedenAd == bedenAd && x.MudahaleMi == true).ToList());
                    }
                    else
                    {
                        dtoNumuneAras.AddRange(araList.Where(x => x.BedenAd == item && x.MudahaleMi == false).ToList());
                    }
                }
            }
            else
            {
                dtoNumuneAras = araList.Where(x => bedenler.Contains(x.BedenAd) && x.MudahaleMi == false).OrderBy(x => x.BedenAd).ToList();
            }


            NumuneDto numuneDto = new NumuneDto();

            numuneDto.detay = detay;
            numuneDto.araList = dtoNumuneAras;

            return PartialView(numuneDto);
        }

        public JsonResult GetKazanDetayModel(string idList, string orderNoList, int tabloTuru, string raporTuruList)
        {
            try
            {
                RaporlamaDataModel data = raporlama.GetRaporlamaDataModel(idList, orderNoList, tabloTuru, raporTuruList);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public PartialViewResult BedenlerWithReference(int numuneDetayId, int orderId,int IsNumune = 1)
        {
            List<Bedenler> bedenIds = new List<Bedenler>();

            if(IsNumune == 1)
            {
                bedenIds = db.NumuneAra.Where(x => x.numuneDetayID == numuneDetayId).Select(x => x.Bedenler).Distinct().ToList();
            }
            else
            {
                bedenIds = db.OlcuTabloAra.Where(x => x.olcuTabloID == numuneDetayId).Select(x => x.Bedenler).Distinct().ToList();
            }

            ViewBag.bedenSistemi = bedenIds.Select(x => x.bedenSistemi).FirstOrDefault();

            ViewBag.isFromReference = 1;
            ViewBag.orderId = orderId;
            ViewBag.numuneDetayId = numuneDetayId;
            ViewBag.isNumune = IsNumune;

            return PartialView("~/Views/NumuneFormu/BedenlerModal.cshtml", bedenIds);
        }

        public PartialViewResult AddSizeModal(int isFromModelist, List<string> bedenler,List<BedenDto> bedenDtos = null)
        {
            string firstBeden = bedenler.FirstOrDefault().Split('-')[0];

            int bedenSistemi = (int)db.Bedenler.Where(x => x.beden == firstBeden).Select(x => x.bedenSistemi).FirstOrDefault();

            List<Bedenler> bedens = db.Bedenler.Where(x => x.bedenSistemi == bedenSistemi && x.ID != -1).ToList();

            if(isFromModelist == 1)
            {
                ViewBag.olanBedenler = bedenler;
            }
            else
            {
                ViewBag.olanBedenler = bedenDtos.Where(x => x.MüdahaleMi == 1).Select(x => x.BedenAd).ToList();
            }
            ViewBag.isFromModelist = isFromModelist;

            return PartialView(bedens);
        }

        public PartialViewResult DifferentOrderModal()
        {
            return PartialView();
        }

        //Helper Methods
        #region 
        public DTONumuneDetay GetDTONumuneDetay(NumuneDetay numuneDetay)
        {
            DTONumuneDetay numuneDetayDto = new DTONumuneDetay()
            {
                Id = numuneDetay.id,
                OrderId = numuneDetay.orderID,
                OrderNo = numuneDetay.Order.orderNo,
                Aciklama = numuneDetay.aciklama == null ? "" : numuneDetay.aciklama,
                AtolyeAd = fason.getAtolyes().Where(x => x.Id == numuneDetay.atolyeID).Select(x => x.AtolyeAd).FirstOrDefault(),
                AtolyeId = (int)numuneDetay.atolyeID,
                DikimNedeni = numuneDetay.dikimNedeni,
                ElliListe = numuneDetay.elliListe,
                ElliNumune = numuneDetay.elliNumune,
                KalipAdi = numuneDetay.kalipAdi,
                KID = numuneDetay.KID,
                KullaniciId = numuneDetay.kullaniciID,
                KullaniciAd = numuneDetay.PersonelTablo.personelAd,
                OlcumNo = (int)numuneDetay.olcumNo,
                OlcuTuruAd = numuneDetay.OlcuTurleri.olcuTuruAd,
                OlcuTuruId = numuneDetay.olcuTuruID,
                ReceteKod = numuneDetay.receteKod,
                Tarih = numuneDetay.tarih,
                TopNo = numuneDetay.topNo,
                UpdatedDate = (DateTime)numuneDetay.updatedDate,
                UpdatedUserAd = (string)db.PersonelTablo.Where(x => x.id == numuneDetay.updatedUserId).Select(x => x.personelAd).FirstOrDefault(),
                YikamadanGelis = (DateTime)numuneDetay.yikamadanGelis,
                YikamayaGidis = (DateTime)numuneDetay.yikamayaGidis,
                Kumas = numuneDetay.Order.kumas,
                ModelAd = numuneDetay.Order.modelAd,
                Musteri = numuneDetay.Order.musteri,
                tabloTuru = numuneDetay.tabloTuru,
                BagliOrderId = numuneDetay.bagliOrderId,
                HasPreviousOrder = numuneDetay.hasPreviousOrder,
                UpdatedUserId = numuneDetay.updatedUserId

            };

            return numuneDetayDto;
        }

        public List<DTONumuneDetay> GetDTONumuneDetays(List<NumuneDetay> numuneDetays)
        {
            List<DTONumuneDetay> dTONumuneDetays = new List<DTONumuneDetay>();

            foreach (var item in numuneDetays)
            {
                DTONumuneDetay numuneDetayDto = new DTONumuneDetay()
                {
                    Id = item.id,
                    OrderId = item.orderID,
                    OrderNo = item.Order.orderNo,
                    Aciklama = item.aciklama == null ? "" : item.aciklama,
                    AtolyeAd = fason.getAtolyes().Where(x => x.Id == item.atolyeID).Select(x => x.AtolyeAd).FirstOrDefault(),
                    AtolyeId = (int)item.atolyeID,
                    DikimNedeni = item.dikimNedeni,
                    ElliListe = item.elliListe,
                    ElliNumune = item.elliNumune,
                    KalipAdi = item.kalipAdi,
                    KID = item.KID,
                    KullaniciId = item.kullaniciID,
                    KullaniciAd = item.PersonelTablo.personelAd,
                    OlcumNo = (int)item.olcumNo,
                    OlcuTuruAd = item.OlcuTurleri.olcuTuruAd,
                    OlcuTuruId = item.olcuTuruID,
                    ReceteKod = item.receteKod,
                    Tarih = item.tarih,
                    TopNo = item.topNo,
                    UpdatedDate = (DateTime)item.updatedDate,
                    UpdatedUserAd = (string)db.PersonelTablo.Where(x => x.id == item.updatedUserId).Select(x => x.personelAd).FirstOrDefault(),
                    YikamadanGelis = (DateTime)item.yikamadanGelis,
                    YikamayaGidis = (DateTime)item.yikamayaGidis,
                    tabloTuru = item.tabloTuru,
                    aktarimMi = item.aktarimMi,
                    Kumas = item.Order.kumas,
                    Musteri = item.Order.musteri,
                    ModelAd = item.Order.modelAd,
                    BagliOrderId = item.bagliOrderId,
                    HasPreviousOrder = item.hasPreviousOrder,
                    UpdatedUserId = item.updatedUserId

                };

                dTONumuneDetays.Add(numuneDetayDto);
            }

            return dTONumuneDetays;
        }

        public DtoNumuneAra GetDtoNumuneAra(NumuneAra numuneAra)
        {
            DtoNumuneAra dtoNumuneAra = new DtoNumuneAra()
            {
                Id = numuneAra.id,
                NumuneDetayId = numuneAra.numuneDetayID,
                OlcuNoktaId = numuneAra.olcuNoktaID,
                OlcuNoktaAd = numuneAra.OlcuNoktalari.olcuNoktasi,
                BedenId = numuneAra.bedenID,
                BedenAd = numuneAra.Bedenler.beden,
                KalipOlcusu = (double)numuneAra.kalipOlcusu,
                UygulananTolerans = (double)numuneAra.uygulananTolerans,
                YikamaOncesiTablo = (double)numuneAra.yikamaOncesiTablo,
                UygulananCekme = (double)numuneAra.uygulananCekme,
                YikamaSonrasiTablo = (double)numuneAra.yikamaSonrasiTablo,
                GerceklesenTolerans = (double)numuneAra.gerceklesenTolerans,
                YikamaOncesiOlculen = (double)numuneAra.yikamaSonrasiOlculen,
                GerceklesenCekme = (double)numuneAra.gerceklesenCekme,
                YikamaSonrasiOlculen = (double)numuneAra.yikamaSonrasiOlculen,
                YikamaOncesiFark = (double)numuneAra.yikamaOncesiFark,
                YikamaSonrasiFark = (double)numuneAra.yikamaSonrasiFark,
                MudahaleMi = (bool)numuneAra.mudahaleMi

            };

            return dtoNumuneAra;
        }

        public List<DTOImalatTabloDetay> GetDtoImalatTabloDetays(List<OlcuTabloDetay> olcuTabloDetays)
        {
            List<DTOImalatTabloDetay> imalatTabloDetays = new List<DTOImalatTabloDetay>();

            foreach (var item in olcuTabloDetays)
            {
                DTOImalatTabloDetay imalatTabloDetay = new DTOImalatTabloDetay()
                {
                    id = item.id,
                    orderNo = item.Order.orderNo,
                    orderID = item.orderID,
                    giysiTuru = item.GiysiTurleri.giysiTuruAd,
                    olcuTUru = item.OlcuTurleri.olcuTuruAd,
                    kullanıcıAdı = item.PersonelTablo.personelAd + " " + item.PersonelTablo.personelSoyad,
                    tarih = Convert.ToDateTime(item.tarih),
                    enBoyCekme = item.enBoyCekme
                };
                imalatTabloDetays.Add(imalatTabloDetay);
            }

            return imalatTabloDetays;
        }

        private List<DtoNumuneAra> GetDtoNumuneArasWithImalat(List<OlcuTabloAra> yikamaSonrasi, List<OlcuTabloAra> olcuTabloAras,List<OlcuTabloHesaplama> olcuTabloHesaplamas, List<string> bedenList)
        {
            List<DtoNumuneAra> dtoNumuneAras = new List<DtoNumuneAra>();

            foreach (var item in bedenList)
            {
                string beden = item;

                int bedenId = db.Bedenler.Where(x => x.beden == beden).Select(x => x.ID).FirstOrDefault();

                List<OlcuTabloAra> olcuTablos = olcuTabloAras.Where(x => x.Bedenler.beden == item).OrderBy(x => x.id).ToList();
                List<OlcuTabloAra> yikamaSonrasis = yikamaSonrasi.Where(x => x.Bedenler.beden == item).OrderBy(x => x.id).ToList();

                foreach (var olcuTabloAra in olcuTablos)
                {

                    int olcuNoktaId = olcuTabloAra.olcuNoktaID;

                    OlcuTabloHesaplama olcuTabloHesaplama = olcuTabloHesaplamas.Where(x => x.olcuNoktaID == olcuNoktaId).FirstOrDefault();
                    OlcuTabloAra yikamaSonrasiSelected = yikamaSonrasis.Where(x => x.olcuNoktaID == olcuNoktaId).FirstOrDefault();

                    DtoNumuneAra dtoNumuneAra = new DtoNumuneAra()
                    {
                        BedenAd = beden,
                        BedenId = bedenId,
                        OlcuNoktaAd = olcuTabloAra.OlcuNoktalari.olcuNoktasi,
                        OlcuNoktaId = olcuTabloAra.olcuNoktaID,
                        KalipOlcusu = olcuTabloAra.deger + Convert.ToDouble(olcuTabloHesaplama.tolerans == null ? 0 : olcuTabloHesaplama.tolerans),
                        UygulananTolerans = Convert.ToDouble(olcuTabloHesaplama.tolerans == null ? 0 : olcuTabloHesaplama.tolerans),
                        YikamaOncesiTablo = olcuTabloAra.deger,
                        UygulananCekme = Convert.ToDouble(olcuTabloHesaplama.cekme == null ? 0 : olcuTabloHesaplama.cekme),
                        YikamaSonrasiTablo = yikamaSonrasiSelected.deger

                    };

                    dtoNumuneAras.Add(dtoNumuneAra);
                }
            }

            return dtoNumuneAras;
        }
        public List<DtoNumuneAra> GetFromCustomerTable(List<string> bedenler,int orderId)
        {
            List<DtoNumuneAra> dtoNumuneAras = new List<DtoNumuneAra>();

            Order order = db.Order.Where(x => x.ID == orderId).FirstOrDefault();

            string musteri = order.musteri.ToUpper();
            string giysiTuru = order.giysiTuru;

            int musteriId = 0;
            int giysiTuruId = 0;

            if (musteri.Contains("JACK"))
            {
                musteriId = 1;
            }
            else if (musteri.Contains("ONLY"))
            {
                musteriId = 2;
            }
            else if(musteri.Contains("ANTONY"))
            {
                musteriId = 3;
            }
            else if (musteri.Contains("VOICE"))
            {
                musteriId = 4;
            }
            else if (musteri.Contains("STING"))
            {
                musteriId = 5;
            }
            else if (musteri.Contains("LOFT"))
            {
                musteriId = 6;
            }
            else if (musteri.Contains("MASSIMO"))
            {
                musteriId = 7;
            }
            else if (musteri.Contains("DANSK"))
            {
                musteriId = 8;
            }
            else if (musteri.Contains("PEPE"))
            {
                musteriId = 9;
            }
            else if (musteri.Contains("OLIVER"))
            {
                musteriId = 10;
            }
            else if (musteri.Contains("PULL"))
            {
                musteriId = 11;
            }
            else
            {
                musteriId = 1;
            }

            giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuru).Select(x => x.id).FirstOrDefault();

            if(giysiTuruId == 0 || giysiTuruId == null)
            {
                giysiTuruId = 1;
            }


            int numuneHazirTabloDetayId = db.NumuneHazirTabloDetay.Where(x => x.MusteriId == musteriId && x.GiysiTuruId == giysiTuruId).Select(x => x.Id).FirstOrDefault();

            if(numuneHazirTabloDetayId == 0 || numuneHazirTabloDetayId == null)
            {
                numuneHazirTabloDetayId = 9;
            }

            List<NumuneHazirTabloAra> numuneHazirTabloAras = db.NumuneHazirTabloAra.Where(x => x.HazirTabloId == numuneHazirTabloDetayId).OrderBy(x=>x.Id).ToList();

            foreach (var beden in bedenler)
            {
                int bedenId = db.Bedenler.Where(x => x.beden == beden).Select(x => x.ID).FirstOrDefault();

                foreach (var item in numuneHazirTabloAras)
                {
                    DtoNumuneAra dtoNumuneAra = new DtoNumuneAra()
                    {
                        BedenAd = beden,
                        BedenId = bedenId,
                        GerceklesenCekme = 0,
                        GerceklesenTolerans = 0,
                        KalipOlcusu = 0,
                        MudahaleMi = false,
                        OlcuNoktaAd = item.olcuNoktasi,
                        OlcuNoktaId = item.olcuNoktaId,
                        UygulananCekme = 0,
                        UygulananTolerans = 0,
                        YikamaOncesiFark = 0,
                        YikamaOncesiOlculen = 0,
                        YikamaOncesiTablo = 0,
                        YikamaSonrasiFark = 0,
                        YikamaSonrasiOlculen = 0,
                        YikamaSonrasiTablo = 0
                    };

                    dtoNumuneAras.Add(dtoNumuneAra);
                }
            }

         

            return dtoNumuneAras;
        }

        public List<DtoNumuneAra> GetDtoNumuneAras(List<NumuneAra> numuneAras)
        {
            List<DtoNumuneAra> dtoNumuneAras = new List<DtoNumuneAra>();

            foreach (var item in numuneAras)
            {
                DtoNumuneAra dtoNumuneAra = new DtoNumuneAra()
                {
                    Id = item.id,
                    NumuneDetayId = item.numuneDetayID,
                    OlcuNoktaId = item.olcuNoktaID,
                    OlcuNoktaAd = item.OlcuNoktalari.olcuNoktasi,
                    BedenId = item.bedenID,
                    BedenAd = item.Bedenler.beden,
                    KalipOlcusu = (double)item.kalipOlcusu,
                    UygulananTolerans = (double)item.uygulananTolerans,
                    YikamaOncesiTablo = (double)item.yikamaOncesiTablo,
                    UygulananCekme = (double)item.uygulananCekme,
                    YikamaSonrasiTablo = (double)item.yikamaSonrasiTablo,
                    GerceklesenTolerans = (double)item.gerceklesenTolerans,
                    YikamaOncesiOlculen = (double)item.yikamaOncesiOlculen,
                    GerceklesenCekme = (double)item.gerceklesenCekme,
                    YikamaSonrasiOlculen = (double)item.yikamaSonrasiOlculen,
                    YikamaOncesiFark = (double)item.yikamaOncesiFark,
                    YikamaSonrasiFark = (double)item.yikamaSonrasiFark,
                    MudahaleMi = (bool)item.mudahaleMi
                };

                dtoNumuneAras.Add(dtoNumuneAra);
            }

            return dtoNumuneAras;
        }

        public List<DtoNumuneAra> ConvertDtoNumuneAra(int orderId, List<string> bedenler, RaporlamaDataModel tolerans, RaporlamaDataModel cekme)
        {
            List<DtoNumuneAra> dtos = new List<DtoNumuneAra>();

            //Buranın içini doldur
            var order = db.Order.Where(x => x.ID == orderId).FirstOrDefault();
            //Önce hazır numune tablosundan ölçü noktalarını alıyorum.

            string giysiTuru = order.giysiTuru;

            int giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == giysiTuru).Select(x => x.id).FirstOrDefault();

            string musteri = order.musteri;

            if (musteri.Contains("JACK"))
            {
                musteri = "JACK & JONES";
            }
            else if (musteri.Contains("ONLY"))
            {
                musteri = "ONLY";

            }
            else if (musteri.Contains("ANTONY"))
            {
                musteri = "ANTONY MORATO";

            }
            else if (musteri.Contains("VOICE"))
            {
                musteri = "VOICE";

            }
            else if (musteri.Contains("MASSIMO"))
            {
                musteri = "MASSIMO DUTTI";
            }
            else if (musteri.Contains("STING"))
            {
                musteri = "STING";
            }
            else if (musteri.Contains("LOFT"))
            {
                musteri = "LOFT";
            }
            else if (musteri.Contains("ZARA"))
            {
                musteri = "ZARA";
            }

            int musteriId = db.Musteriler.Where(x => x.musteriAd == musteri).Select(x => x.id).FirstOrDefault();



            var hazirNumuneTablo = db.NumuneHazirTabloAra.Where(x => x.NumuneHazirTabloDetay.GiysiTuruId == giysiTuruId && x.NumuneHazirTabloDetay.MusteriId == musteriId).ToList();

            foreach (var bedens in bedenler)
            {
                foreach (var item in hazirNumuneTablo)
                {
                    int olcuNoktaId = item.olcuNoktaId;

                    int anaTabanId = db.OlcuNoktalari.Where(x => x.id == olcuNoktaId).Select(x => x.tabanID).FirstOrDefault();

                    int anaOlcuNoktaId = db.OlcuNoktalari.Where(x => x.tabanID == anaTabanId && x.anaNoktami == true).Select(x => x.id).FirstOrDefault();

                    int bedenId = db.Bedenler.Where(x => x.beden == bedens).Select(x => x.ID).FirstOrDefault();

                    double sayilanCekme = 0;

                    double sayilanTolerans = 0;

                    var gelenCekme = cekme.ortalamaList.Where(x => x.olcuNoktaID == anaOlcuNoktaId).Select(x => x.kararVerilenCekmetolerans).FirstOrDefault();

                    if (gelenCekme != null)
                    {
                        sayilanCekme = Convert.ToDouble(gelenCekme);
                    }

                    var gelenTolerans = tolerans.ortalamaList.Where(x => x.olcuNoktaID == anaOlcuNoktaId).Select(x => x.kararVerilenCekmetolerans).FirstOrDefault();

                    if (gelenTolerans != null)
                    {
                        sayilanTolerans = Convert.ToDouble(gelenTolerans);
                    }

                    DtoNumuneAra dto = new DtoNumuneAra()
                    {
                        BedenId = bedenId,
                        BedenAd = bedens,
                        OlcuNoktaId = olcuNoktaId,
                        OlcuNoktaAd = item.olcuNoktasi,
                        UygulananCekme = sayilanCekme,
                        UygulananTolerans = sayilanTolerans,
                    };
                    dtos.Add(dto);


                }
            }



            return dtos;
        }

        public DTONumuneDetay GetDTONumuneDetayByOrderId(int orderId)
        {

            var order = db.Order.Where(x => x.ID == orderId).FirstOrDefault();

            int giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == order.giysiTuru).Select(x => x.id).FirstOrDefault();
            int olcuTuruId = db.OlcuTurleri.Where(x => x.olcuTuruAd == order.orderTipi).Select(x => x.id).FirstOrDefault();

            int count = db.NumuneDetay.Where(x => x.orderID == order.ID && x.giysiTuruID == giysiTuruId && x.olcuTuruID == olcuTuruId && x.tabloTuru == -1).Count();

            DTONumuneDetay dto = new DTONumuneDetay()
            {
                Musteri = order.musteri,
                KID = "",
                Aciklama = "",
                AtolyeAd = "",
                AtolyeId = 65,
                DikimNedeni = order.orderTipi,
                ElliListe = "",
                ElliNumune = "",
                OlcuTuruAd = order.orderTipi,
                KalipAdi = "",
                Kumas = order.kumas,
                ModelAd = order.modelAd,
                OlcumNo = count + 1,
                OrderId = order.ID,
                OrderNo = order.orderNo,
                OlcuTuruId = olcuTuruId,
                ReceteKod = "",
                Tarih = DateTime.Now,
                TopNo = "",
                YikamadanGelis = DateTime.Now,
                YikamayaGidis = DateTime.Now,

            };

            return dto;
        }

        public void GetConnectedOrdersRecursive(int orderId)
        {
            List<NumuneDetay> numuneDetays = db.NumuneDetay.Where(x => x.orderID == orderId).ToList();

            if (numuneDetays.Count > 0)
            {
                int baglantiliOrderId = 0;
                int baglantiliNumuneId = 0;
                foreach (var item in numuneDetays)
                {
                    if (item.bagliOrderId != null)
                    {
                        baglantiliOrderId = (int)item.bagliOrderId;
                        baglantiliNumuneId = (int)item.id;

                    }
                    baglantiliNumunes.Add(item);
                    baglantiliNumuneIds.Add(baglantiliNumuneId);
                }

                GetConnectedOrdersRecursive(baglantiliOrderId);

            }
        }

        public int RemoveNumune(int numuneDetayId)
        {
            int isSuccess = 0;
            try
            {
                var parametersDelete = new[] { new SqlParameter("@numuneDetayId", SqlDbType.Int) { Value = numuneDetayId } };

                db.Database.ExecuteSqlCommand("DELETE FROM NumuneAra where numuneDetayID = @numuneDetayId", parametersDelete);

                db.Database.ExecuteSqlCommand("DELETE FROM [Olcum].[dbo].[NumuneDetay] where id = @numuneDetayId", parametersDelete);

                isSuccess = numuneDetayId;

                return isSuccess;
            }
            catch (Exception ex)
            {
                isSuccess = -1;
                return isSuccess;
            }
        }

        public List<DtoNumuneAra> ConvertDtoNumuneAraFromReference(int numuneDetayId, List<string> bedenler)
        {
            List<NumuneAra> numuneAras = db.NumuneAra.Where(x => x.numuneDetayID == numuneDetayId && bedenler.Contains(x.Bedenler.beden)).ToList();

            List<DtoNumuneAra> dtos = GetDtoNumuneAras(numuneAras);

            return dtos;
        }
        
        public List<string> OrderSize(int? bedenSistemi,List<string> bedenler)
        {
            List<string> stringBedenler = new List<string>() { "XXS", "XS", "S", "M", "L", "XL", "XXL", "XXXL", "XXXXL", "XXXXXL", "XXXXXXL" };
            List<string> intBedenler = db.Bedenler.Where(x => x.bedenSistemi == 1).Select(x => x.beden).OrderBy(x => x).ToList();
            List<string> intLargeBedenler = db.Bedenler.Where(x => x.bedenSistemi == 2).Select(x => x.beden).OrderBy(x => x).ToList();

            if (bedenSistemi == 0)
            {
                bedenler = bedenler.OrderBy(x => stringBedenler.IndexOf(x)).ToList();
            }
            else if (bedenSistemi == 1)
            {
                bedenler = bedenler.OrderBy(x => intBedenler.IndexOf(x)).ToList();

            }
            else if (bedenSistemi == 2)
            {
                bedenler = bedenler.OrderBy(x => intLargeBedenler.IndexOf(x)).ToList();

            }

            return bedenler;
        }
        
        #endregion

        public static T DeepCopy<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
    }

    public class NumuneDto
    {
        public DTONumuneDetay detay;
        public List<DtoNumuneAra> araList;
    }

    public class BedenDto
    {
        public int BedenId { get; set; }
        public string BedenAd { get; set; }

        public int BedenIndex { get; set; }

        public int MüdahaleMi { get; set; }
    }

    public class OlcuTuruOranDto
    {
        public string OlcuTuruAd { get; set; }
        public double Oran { get; set; }
    }

    public class ReferenceOrderDto
    {
        public Order Order { get; set; }
        public int IsNumune { get; set; }
    }
}