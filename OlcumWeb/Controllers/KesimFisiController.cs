using OlcumWeb.Araclar;
using OlcumWeb.dbOlcum;
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
    public class KesimFisiController : Controller
    {
        OlcumContext db = new OlcumContext();

        // GET: KesimFisi
        public ActionResult Index(int id)
        {
            PersonelTablo personel = db.PersonelTablo.Where(x => x.id == id).FirstOrDefault();

            ViewBag.personel = personel; 

            ViewBag.giysiTurleri = db.GiysiTurleri.ToList();
            ViewBag.bedenler = db.Bedenler.Where(x=>x.ID != -1).ToList();
            ViewBag.boylar = db.Boylar.ToList();
            ViewBag.olcuNoktalari = db.OlcuNoktalari.Where(x => x.id != -1).ToList();
            return View();
        }


        public JsonResult GetKesimFisiDto(int orderId,string orderNo)
        {
            //prosedürün ismi kumas_recete_getir 
            List<KesimFisiDetay> kesimFisiDetayList = db.KesimFisiDetay.Where(x => x.OrderId == orderId).ToList();

            List<KesimFisiDto> kesimFisiDtoList = new List<KesimFisiDto>();

            if(kesimFisiDetayList.Count > 0)
            {
                foreach (var item in kesimFisiDetayList)
                {
                    KesimFisiDto kesimFisiDto = new KesimFisiDto()
                    {
                        Id = item.Id,
                        OrderId = item.OrderId,
                        OrderNo = item.OrderNo,
                        aciklama = item.aciklama,
                        Kumas = item.Order.kumas,
                        ModelAd = item.Order.modelAd,
                        ModelNo = item.Order.modelNo,
                        Musteri = item.Order.musteri,
                        Baski = item.Baski,
                        Cekme = item.Cekme,
                        Garni1 = item.Garni1,
                        Garni2 = item.Garni2,
                        Garni3 = item.Garni3,
                        GiysiTuruId = item.GiysiTuruId,
                        KullaniciId = item.KullaniciId,
                        KumasEni = item.KumasEni,
                        Nakis = item.Nakis,
                        OlcuTuruId = item.OlcuTuruId,
                        Tela = item.Tela,
                        TopNo = item.TopNo,
                        HasExistOnDb = 1,
                    };

                    kesimFisiDtoList.Add(kesimFisiDto);
                }
            }
            else
            {
                List<KumasReceteDto> kumasReceteDto = new List<KumasReceteDto>();

                using (var connection = new SqlConnection(ConnectionStrings.OlcumConnection))
                {
                    connection.Open();

                    SqlCommand cmd_Firma = new SqlCommand(@"exec LIVE.SentezLive.dbo.kumas_recete_getir '" + orderNo + "'", connection);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd_Firma);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    kumasReceteDto = ConvertDataTable<KumasReceteDto>(dt);

                    connection.Close();
                }

                List<string> garniList = kumasReceteDto.Where(x => x.PastalAdi != "Ana Kumaş" && x.PastalAdi != "Tela").Select(x => x.KumasCinsi).ToList();

                KesimFisiDto kesimFisiDto = new KesimFisiDto()
                {
                    OrderId = orderId,
                    OrderNo = orderNo,
                    Tela = kumasReceteDto.Where(x => x.PastalAdi == "Tela").Select(x => x.KumasCinsi).FirstOrDefault(),
                    HasExistOnDb = 0
                };

                Order order = db.Order.Where(x => x.ID == orderId).FirstOrDefault();

                kesimFisiDto.ModelAd = order.modelAd;
                kesimFisiDto.ModelNo = order.modelNo;
                kesimFisiDto.Kumas = order.kumas;
                kesimFisiDto.Musteri = order.musteri;

                string orderGiysiTuru = order.giysiTuru;

                int giysiTuruId = db.GiysiTurleri.Where(x => x.giysiTuruAd == orderGiysiTuru).Select(x => x.id).FirstOrDefault();

                kesimFisiDto.GiysiTuruId = giysiTuruId;

                if (garniList.Count > 0)
                {
                    kesimFisiDto.Garni1 = garniList[0];

                    if(garniList.Count > 1)
                    {
                        kesimFisiDto.Garni2 = garniList[1];

                        if(garniList.Count > 2)
                        {
                            kesimFisiDto.Garni3 = garniList[2];
                        }
                    }
                }

                kesimFisiDtoList.Add(kesimFisiDto);
            }

            return Json(kesimFisiDtoList, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult KesimFisiListModal(List<KesimFisiDto> kesimFisiDtos)
        {
            return PartialView();
        }

        //Helper Methods
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

    public class KesimFisiDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string OrderNo { get; set; }

        public int KullaniciId { get; set; }

        public int GiysiTuruId { get; set; }

        public int OlcuTuruId { get; set; }

        public string ModelNo { get; set; }

        public string ModelAd { get; set; }

        public string Kumas { get; set; }

        public string Musteri { get; set; }

        public string Garni1 { get; set; }

        public string Garni2 { get; set; }

        public string Garni3 { get; set; }

        public string Tela { get; set; }

        public string Cekme { get; set; }

        public string KumasEni { get; set; }

        public string TopNo { get; set; }

        public string Baski { get; set; }

        public string Nakis { get; set; }

        public string aciklama { get; set; }

        public int HasExistOnDb { get; set; }
    }

    public class KesimFisiAraDto
    {
        public int Id { get; set; }

        public int KesimFisiId { get; set; }

        public int? BedenId { get; set; }

        public int? BoyId { get; set; }

        public int? Adet { get; set; }

        public int? OlcuNoktaId { get; set; }

        public string OlcuNoktaAd { get; set; }
    }

    public class KumasReceteDto
    {
        public string KumasKodu { get; set; }
        public string KumasCinsi { get; set; }
        public string PastalAdi { get; set; }
        public string KullanimYeri { get; set; }
        public string BirimSarfiyat { get; set; }
    }
}