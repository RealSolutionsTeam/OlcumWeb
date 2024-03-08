using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTOImalatTabloDetay
    {
        public int id { get; set; }
        public int orderID { get; set; }

        public string orderNo { get; set; }
        public int giysiTuruID { get; set; }

        public string giysiTuru { get; set; }
        public int olcuTuruID { get; set; }

        public string olcuTUru { get; set; }
        public string anaBeden { get; set; }
        public string enBoyCekme { get; set; }
        public Nullable<int> tabloTuru { get; set; }
        public string aciklama { get; set; }
        public DateTime tarih { get; set; }

        public string tarihString { get; set; }
        public int kullaniciID { get; set; }

        public string kullanıcıAdı { get; set; }

        public string artikel { get; set; }

        public string musteri { get; set; }
        public string modelAd { get; set; }
        public string modelNo { get; set; }
        public string kumas { get; set; }
    }
}