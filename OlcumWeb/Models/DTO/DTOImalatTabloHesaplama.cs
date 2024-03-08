using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTOImalatTabloHesaplama
    {
        public int id { get; set; }

        public int olcuTabloID { get; set; }

        public string olcuNoktasi { get; set; }

        public string tolerans { get; set; }

        public string cekme { get; set; }

        public string oran { get; set; }
    }
}