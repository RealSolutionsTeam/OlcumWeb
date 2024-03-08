using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DtoBedenCekmeOlcuNokDeger //Girilen degeler list
    {   
        public string beden { get; set; }
        public string bedenid { get; set; }
        public string cekme { get; set; }
        public string olcunokid { get; set; }
        public string deger { get; set; }
        public int pno { get; set; }
        public int bpantno { get; set; }
    }
}