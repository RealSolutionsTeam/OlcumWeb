using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class kazanDevamEtModalDto
    {
        public int id { get; set; }
        public string olcuTuruAd { get; set; }
        public int tabloTuru { get; set; }
        public DateTime tarih { get; set; }
        public string kullaniciAd { get; set; }
        public int kullaniciId { get; set; }
        public Boolean durum { get; set; }
        public int? olcumNo { get; set; }
        public Boolean aparatliMi { get; set; }
        public Boolean? utuluMu { get; set; }
    }
}