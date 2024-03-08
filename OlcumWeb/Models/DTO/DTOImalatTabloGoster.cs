using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTOImalatTabloGoster
    {
        public int id { get; set; }

        public string olcuNoktasi { get; set; }

        public int olcuNoktaId { get; set; }

        public List<string> beden { get; set; }

        public List<int> bedenId { get; set; }

        public List<string> deger { get; set; }


        public string tolerans { get; set; }

        public string cekme { get; set; }

        public string oran { get; set; }
        
    }
}