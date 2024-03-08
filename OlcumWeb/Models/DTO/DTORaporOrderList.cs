using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTORaporOrderList
    {
        public string orderNo { get; set; }

        public string modelAd { get; set; }

        public string kullanıcıAdı { get; set; }

        public string olcuTuruAd { get; set; }

        public DateTime tarih { get; set; }

        public int id { get; set; }

        public int olcuTuruID { get; set; }

        public int raporTuru { get; set; }

        public string tabloTuru { get; set; }
    }
}