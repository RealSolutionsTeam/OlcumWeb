using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class kazanGirilenListDto
    {
        public string bedenad { get; set; }
        public int bedenid { get; set; }
        public string cekme { get; set; }
        public int olcunokid { get; set; }
        public double? deger { get; set; }
        public int pno { get; set; }
        public int bpantno { get; set; }
    }
}