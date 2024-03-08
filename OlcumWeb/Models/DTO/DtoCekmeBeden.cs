using OlcumWeb.dbOlcum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DtoCekmeBeden
    {
        public string cekme { get; set; }
        public List<Bedenler> bedenler { get; set; }
    }
}