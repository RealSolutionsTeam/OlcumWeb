using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DtoOlcuNokDeger //Olması Gereken List - Olcu Yerleri List
    {
        public int olcuNokId { get; set; }
        public string olcuNokAd { get; set; }
        public string deger { get; set; }
        public string cekme { get; set; }
        public int bedenId { get; set; }
        public string bedenAd { get; set; }
        public int sirano { get; set; }


    }
}