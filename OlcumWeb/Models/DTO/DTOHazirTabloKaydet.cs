using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTOHazirTabloKaydet
    {
        public int araID { get; set; }
        public int olcuNoktaID { get; set; }

        public int bedenID { get; set; }

        public string deger { get; set; }

    }
}