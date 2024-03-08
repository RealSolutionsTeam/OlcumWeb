using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTORaporBagliOrderlar
    {
        public int tabloID { get; set;}

        public string tabloOrderNo { get; set; }

        public string tabloOlcuTuru { get; set; }

        public int tabloRaporTuru { get; set; }
    }
}