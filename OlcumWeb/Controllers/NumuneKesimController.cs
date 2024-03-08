using Newtonsoft.Json;
using OlcumWeb.Araclar;
using OlcumWeb.dbOlcum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlcumWeb.Controllers
{
    public class NumuneKesimController : Controller
    {
        SqlConnection baglanti = new SqlConnection(ConnectionStrings.OlcumConnection);
        OlcumContext db = new OlcumContext();

        public List<NumuneDetay> form;
        public List<string> garniList;
        public string tela;
        public Order orders;
        public bool isExist;

        public ActionResult Index(string ad,string soyad)
        {
            ViewBag.id = db.PersonelTablo.Where(x => x.personelAd == ad && x.personelSoyad == soyad).Select(x=>x.id).FirstOrDefault();
            ViewBag.order = db.Order.ToList();
            ViewBag.ad = ad;
            ViewBag.soyad = soyad;
            ViewBag.adSoyad = ad + " " + soyad;

            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]

        public JsonResult KeyPress(string order)
        {
            orders = db.Order.Where(x => x.orderNo.Contains(order)).FirstOrDefault();
            int orderID = 0;
            if (orders != null)
            {
                orderID = orders.ID;
            }

            form = db.NumuneDetay.Where(x => x.orderID.Equals(orderID)).ToList();

            if (form.Count == 0)
            {
                garniList = new List<string>();
                tela = "";
                baglanti.Open();
                //Garnileri listeye aktarıyoruz.
                string garniQuery = "select * from " +
                         "(Select kumasCins, ROW_NUMBER() OVER(PARTITION BY kumasCins order by kumasMetre ASC) AS RowNumber " +
                         " from KumasDetay where orderID = '" + orderID + "' AND kumasLokasyonu  NOT LIKE '%TELA%' and kumasLokasyonu NOT LIKE '%ÜRT%' and " +
                         " kumasLokasyonu NOT LIKE '%SER%' and kumasLokasyonu NOT LIKE '%KOL%' and kumasLokasyonu NOT LIKE '%PİLOT%' " +
                         " and kumasLokasyonu NOT LIKE 'THMN' and kumasLokasyonu NOT LIKE '%PLT%' and kumasLokasyonu NOT LIKE '%IML%') AS a " +
                         " where a.RowNumber = 1";
                SqlCommand garniCommand = new SqlCommand(garniQuery, baglanti);
                SqlDataReader garniDr = garniCommand.ExecuteReader();
                while (garniDr.Read())
                {
                    if (!garniDr.IsDBNull(0))
                        garniList.Add(garniDr.GetString(0));
                }
                garniDr.Close();

                //TELAsı varsa aktarıyoruz.
                string telaQuery = "select kumasCins from KumasDetay where orderID = '" + orderID + "' and kumasLokasyonu = 'TELA'";
                SqlCommand telaCommand = new SqlCommand(telaQuery, baglanti);
                SqlDataReader telaReader = telaCommand.ExecuteReader();

                while (telaReader.Read())
                {
                    if (!telaReader.IsDBNull(0))
                        tela = telaReader.GetString(0);
                }
                telaReader.Close();

                baglanti.Close();

                var deneme = new { orders, garniList, tela };

                return new JsonResult { Data = deneme, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult { Data = form, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public void SaveData(DateTime? tarih, string orderNo, int formNo, string cinsi, string müsteri = "",
            string modelNo = "", string modelAdi = "", string kumas = "", string garni1 = "", string garni2 = "", string garni3 = "",
            string tela = "", string modelist = "", string dikimNedeni = "", string dikilecekBedenBoy = "", string adet = "",
            string olcuNoktasi1 = "", string olcuNoktasi2 = "", string olcuNoktasi3 = "", string olcuNoktasi4 = "", string olcuNoktasi5 = "", string olcuNoktasi6 = "",
            string olcu1 = "", string olcu2 = "", string olcu3 = "", string olcu4 = "", string olcu5 = "", string olcu6 = "",
            string cekme = "", string kumasEni = "", string topNo = "", string kid = "", string baski = "", string nakis = "")
        {
            if (baglanti.State == ConnectionState.Closed) { baglanti.Open(); }

            //OlcumRaporlamaEntities or = new OlcumRaporlamaEntities();
            //bool idare = or.NumuneFormu.Any(x => x.formNo == formNo & x.orderNo.Contains(orderNo));

            string existQuery = "select top 1 orderNo from NumuneFormu where orderNo = '" + orderNo + "' and formNo = '" + formNo.ToString() + "'";
            SqlCommand command = new SqlCommand(existQuery, baglanti);
            SqlDataReader dr2 = command.ExecuteReader();

            while (dr2.Read())
            {
                if (dr2.IsDBNull(0))
                {
                    isExist = false;
                }
                else
                {
                    isExist = true;
                }
            }
            dr2.Close();

            if (isExist)
            {
                //Güncelleme
                string updateQuery = "update NumuneFormu set orderNo = @orderNo,formNo = @formNo, tarih = @tarih, müsteri = @müsteri, modelNo = @modelNo, modelAdi = @modelAdi, kumasTuru = @kumasTuru, garni = @garni," +
                        "garni2 = @garni2,garni3 = @garni3,tela = @tela, cinsi = @cinsi, " +
" dikimNedeni = @dikimNedeni, dikilecekBedenBoy = @dikilecekBedenBoy, adet = @adet,olcuNoktasi1 = @olcuNoktasi1, olcu1 = @olcu1,olcuNoktasi2 = @olcuNoktasi2, olcu2 = @olcu2,olcuNoktasi3 = @olcuNoktasi3, olcu3 = @olcu3, " +
" olcuNoktasi4 = @olcuNoktasi4,olcu4 = @olcu4, olcuNoktasi5 = @olcuNoktasi5,olcu5 = @olcu5, olcuNoktasi6 = @olcuNoktasi6,olcu6 = @olcu6, cekme = @cekme, kumasEni = @kumasEni, topNo = @topNo, " +
" kid = @kid, baski = @baski, nakis = @nakis, modelist = @modelist,aciklama = @aciklama where orderNo = '" + orderNo + "' and formNo = " + formNo.ToString();

                SqlCommand updateCommand = new SqlCommand(updateQuery, baglanti);

                updateCommand.Parameters.AddWithValue("@orderNo", orderNo);
                updateCommand.Parameters.AddWithValue("@formNo", formNo);
                updateCommand.Parameters.Add("@tarih", SqlDbType.Date).Value = tarih;
                updateCommand.Parameters.AddWithValue("@müsteri", müsteri);
                updateCommand.Parameters.AddWithValue("@modelNo", modelNo);
                updateCommand.Parameters.AddWithValue("@modelAdi", modelAdi);
                updateCommand.Parameters.AddWithValue("@kumasTuru", kumas);
                updateCommand.Parameters.AddWithValue("@garni", garni1);
                updateCommand.Parameters.AddWithValue("@garni2", garni2);
                updateCommand.Parameters.AddWithValue("@garni3", garni3);
                updateCommand.Parameters.AddWithValue("@tela", tela);
                updateCommand.Parameters.AddWithValue("@cinsi", cinsi);
                updateCommand.Parameters.AddWithValue("@dikimNedeni", dikimNedeni);
                updateCommand.Parameters.AddWithValue("@dikilecekBedenBoy", dikilecekBedenBoy);
                updateCommand.Parameters.AddWithValue("@adet", adet);
                updateCommand.Parameters.AddWithValue("@olcuNoktasi1", olcuNoktasi1);
                updateCommand.Parameters.AddWithValue("@olcu1", olcu1);
                updateCommand.Parameters.AddWithValue("@olcuNoktasi2", olcuNoktasi2);
                updateCommand.Parameters.AddWithValue("@olcu2", olcu2);
                updateCommand.Parameters.AddWithValue("@olcuNoktasi3", olcuNoktasi3);
                updateCommand.Parameters.AddWithValue("@olcu3", olcu3);
                updateCommand.Parameters.AddWithValue("@olcuNoktasi4", olcuNoktasi4);
                updateCommand.Parameters.AddWithValue("@olcu4", olcu4);
                updateCommand.Parameters.AddWithValue("@olcuNoktasi5", olcuNoktasi5);
                updateCommand.Parameters.AddWithValue("@olcu5", olcu5);
                updateCommand.Parameters.AddWithValue("@olcuNoktasi6", olcuNoktasi6);
                updateCommand.Parameters.AddWithValue("@olcu6", olcu6);
                updateCommand.Parameters.AddWithValue("@cekme", cekme);
                updateCommand.Parameters.AddWithValue("@kumasEni", kumasEni);
                updateCommand.Parameters.AddWithValue("@topNo", topNo);
                updateCommand.Parameters.AddWithValue("@kid", kid);
                updateCommand.Parameters.AddWithValue("@baski", baski);
                updateCommand.Parameters.AddWithValue("@nakis", nakis);
                updateCommand.Parameters.AddWithValue("@modelist", modelist);
                updateCommand.ExecuteNonQuery();

                isExist = true;
            }
            else
            {
                //Kaydetme

                string insertQuery = "insert into NumuneFormu(orderNo,formNo, tarih, müsteri, modelNo, modelAdi, kumasTuru, garni,garni2,garni3,tela, cinsi, dikimNedeni, dikilecekBedenBoy, adet,olcuNoktasi1, olcu1,olcuNoktasi2, olcu2,olcuNoktasi3, olcu3,olcuNoktasi4, olcu4,olcuNoktasi5, olcu5,olcuNoktasi6, olcu6, cekme, kumasEni, topNo, kid, baski, nakis, modelist) " +
               " values(@orderNo,@formNo, @tarih, @müsteri, @modelNo, @modelAdi, @kumasTuru, @garni,@garni2,@garni3,@tela, @cinsi, @dikimNedeni, @dikilecekBedenBoy, @adet,@olcuNoktasi1, @olcu1,@olcuNoktasi2, @olcu2,@olcuNoktasi3, @olcu3,@olcuNoktasi4, @olcu4,@olcuNoktasi5, @olcu5,@olcuNoktasi6, @olcu6, @cekme, @kumasEni, @topNo, @kid, @baski, @nakis, @modelist)";

                SqlCommand insertCommand = new SqlCommand(insertQuery, baglanti);
                insertCommand.Parameters.AddWithValue("@orderNo", orderNo);
                insertCommand.Parameters.AddWithValue("@formNo", formNo);
                insertCommand.Parameters.Add("@tarih", SqlDbType.Date).Value = tarih;
                insertCommand.Parameters.AddWithValue("@müsteri", müsteri);
                insertCommand.Parameters.AddWithValue("@modelNo", modelNo);
                insertCommand.Parameters.AddWithValue("@modelAdi", modelAdi);
                insertCommand.Parameters.AddWithValue("@kumasTuru", kumas);
                insertCommand.Parameters.AddWithValue("@garni", garni1);
                insertCommand.Parameters.AddWithValue("@garni2", garni2);
                insertCommand.Parameters.AddWithValue("@garni3", garni3);
                insertCommand.Parameters.AddWithValue("@tela", tela);
                insertCommand.Parameters.AddWithValue("@cinsi", cinsi);
                insertCommand.Parameters.AddWithValue("@dikimNedeni", dikimNedeni);
                insertCommand.Parameters.AddWithValue("@dikilecekBedenBoy", dikilecekBedenBoy);
                insertCommand.Parameters.AddWithValue("@adet", adet);
                insertCommand.Parameters.AddWithValue("@olcuNoktasi1", olcuNoktasi1);
                insertCommand.Parameters.AddWithValue("@olcu1", olcu1);
                insertCommand.Parameters.AddWithValue("@olcuNoktasi2", olcuNoktasi2);
                insertCommand.Parameters.AddWithValue("@olcu2", olcu2);
                insertCommand.Parameters.AddWithValue("@olcuNoktasi3", olcuNoktasi3);
                insertCommand.Parameters.AddWithValue("@olcu3", olcu3);
                insertCommand.Parameters.AddWithValue("@olcuNoktasi4", olcuNoktasi4);
                insertCommand.Parameters.AddWithValue("@olcu4", olcu4);
                insertCommand.Parameters.AddWithValue("@olcuNoktasi5", olcuNoktasi5);
                insertCommand.Parameters.AddWithValue("@olcu5", olcu5);
                insertCommand.Parameters.AddWithValue("@olcuNoktasi6", olcuNoktasi6);
                insertCommand.Parameters.AddWithValue("@olcu6", olcu6);
                insertCommand.Parameters.AddWithValue("@cekme", cekme);
                insertCommand.Parameters.AddWithValue("@kumasEni", kumasEni);
                insertCommand.Parameters.AddWithValue("@topNo", topNo);
                insertCommand.Parameters.AddWithValue("@kid", kid);
                insertCommand.Parameters.AddWithValue("@baski", baski);
                insertCommand.Parameters.AddWithValue("@nakis", nakis);
                insertCommand.Parameters.AddWithValue("@modelist", modelist);
                //insertCommand.Parameters.AddWithValue("@aciklama", richTextBox1.Text);
                insertCommand.ExecuteNonQuery();

                isExist = true;
            }
        }
    }
}