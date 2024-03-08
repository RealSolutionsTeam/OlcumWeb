using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTOOlcuNoktalari
    {
        public int id { get; set; }

        public string olcuNoktasi { get; set; }

        public int tabanID { get; set; }

        public bool? anaNoktami { get; set; }
    }
}