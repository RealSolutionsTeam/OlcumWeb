using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class pListDto
    {
        public string cekme { get; set; }
        public string bedenad { get; set; }
        public int bedenid { get; set; }
        public int pno { get; set; }
        public int bpantno { get; set; }
        public KazanBarkodDto KazanBarkodDto { get; set; }
        public int? previousYikamaOncesiDetayId { get; set; }
    }
}